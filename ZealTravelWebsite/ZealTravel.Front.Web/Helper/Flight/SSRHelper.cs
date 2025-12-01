using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using ZealTravel.Application.AirlineManagement.Handler;
using ZealTravel.Application.AirlineManagement.Queries;
using ZealTravel.Domain.Interfaces.Handlers;
using ZealTravel.Front.Web.Helper.Flight;
using ZealTravel.Front.Web.Models.Flight;

namespace ZealTravel.Common.Helpers.Flight
{
    public class SSRHelper
    {
        public async static Task<FlightSSRAvailabilityInfo> GetSSRAvailability(string SearchID, string CompanyID, string AirRS, string RefID, string FltType, IHandlesQueryAsync<GetAirSSRQuery, string> getAirSSRHandler)

        {
            string SelectAirRS = string.Empty;
            var SSRresponse = string.Empty;
            var getSSRoutbound = string.Empty;
            try
            {
                if (AirRS != null && AirRS.IndexOf("RefID") != -1 && RefID != null && RefID.Length > 0 && FltType != null)
                {
                    if (FltType.Equals("O"))
                    {
                        SelectAirRS = SelectedResponseHelper.GETSelectedResponse(RefID, "", AirRS, false);
                    }
                    else
                    {
                        SelectAirRS = SelectedResponseHelper.GETSelectedResponse("", RefID, AirRS, false);
                    }

                   
                    if (HttpContextHelper.Current?.Session?.GetString("SSRInfo") != null)
                    {
                        string serializedDataTable = HttpContextHelper.Current?.Session.GetString("SSRInfo");
                        DataTable dtSSRItinerary = new DataTable();
                        if (!string.IsNullOrEmpty(serializedDataTable))
                        {
                            dtSSRItinerary = JsonConvert.DeserializeObject<DataTable>(serializedDataTable);
                        }

                        
                        dtSSRItinerary.TableName = "SSRInfo";

                        DataSet dsBound = new DataSet();
                        dsBound.Tables.Add(dtSSRItinerary.Copy());

                        dsBound.DataSetName = "root";
                        SSRresponse = CommonFunction.DataSetToString(dsBound);
                    }
                    else
                    {
                        if (SelectAirRS.IndexOf("<Trip>M</Trip>") != -1)
                        {
                            var getAirSSQuery = new GetAirSSRQuery
                            {
                                JourneyType = "MC",
                                SearchID = SearchID,
                                CompanyID = CompanyID,
                                AirRS = SelectAirRS
                            };
                            SSRresponse = await getAirSSRHandler.HandleAsync(getAirSSQuery);
                        }
                        else
                        {
                            var getAirSSQuery = new GetAirSSRQuery
                            {
                                JourneyType = "OW",
                                SearchID = SearchID,
                                CompanyID = CompanyID,
                                AirRS = SelectAirRS
                            };
                            SSRresponse = await getAirSSRHandler.HandleAsync(getAirSSQuery);
                        }
                    }

                    if (SSRresponse.IndexOf("SSRInfo") != -1)
                    {
                        SSRresponse = AddColumnInSSR(SSRresponse);
                        getSSRoutbound = FilterSSR(SSRresponse, FltType);
                    }
                }
               
            }
            catch (Exception ex)
            {
                //dbCommon.Logger.dbLogg(CompanyID, 0, "GetSSRAvailability", "clsSSRavailability", SelectAirRS, SearchID, ex.Message);
            }

            var ssrAvailability = new FlightSSRAvailabilityInfo
            {
                SSRresponse = SSRresponse,
                SSRresponseOut = getSSRoutbound,
                SSRresponseIn = string.Empty
            };

            return ssrAvailability;
        }
        public static async Task<FlightSSRAvailabilityInfo> GetSSRAvailabilityCombine(string SearchID, string CompanyID, string AirRS, string RefID, IHandlesQueryAsync<GetAirSSRQuery, string> getAirSSRHandler)
        {
            string SelectAirRS = string.Empty;
            var getSSRoutbound = string.Empty;
            var getSSRinbound = string.Empty;
            string SSRresponse = string.Empty;
            try
            {
                if (AirRS != null && AirRS.IndexOf("RefID") != -1 && RefID != null && RefID.Length > 0)
                {
                    SelectAirRS = SelectedResponseHelper.GETSelectedResponse(RefID, "", AirRS, true);
                    string userIpAddress = HttpContextHelper.Current?.Request.HttpContext.Connection.RemoteIpAddress.ToString();
                    string Host = HttpContextHelper.Current?.Request.Host + "," + userIpAddress;
                    
                    //dbCommon.Logger.dbSearchLogg(CompanyID, 0, Initializer.get_StaffID(), SearchID, "SSR", "DISPLAY", "FO", SelectAirRS, "", Host);

                    
                    var sessionResult = HttpContextHelper.Current?.Session?.GetString("SSRInfo");
                    if (!string.IsNullOrEmpty(sessionResult))
                    {
                        DataTable dtSSRItinerary = JsonConvert.DeserializeObject<DataTable>(sessionResult);
                        dtSSRItinerary.TableName = "SSRInfo";

                        DataSet dsBound = new DataSet();
                        dsBound.Tables.Add(dtSSRItinerary.Copy());

                        dsBound.DataSetName = "root";
                        SSRresponse = CommonFunction.DataSetToString(dsBound);
                    }
                    else
                    {
                        var getAirSSQuery = new GetAirSSRQuery
                        {
                            JourneyType = "RT",
                            SearchID = SearchID,
                            CompanyID = CompanyID,
                            AirRS = SelectAirRS
                        };
                        SSRresponse = await getAirSSRHandler.HandleAsync(getAirSSQuery);                        
                    }

                    if (SSRresponse.IndexOf("SSRInfo") != -1)
                    {
                        SSRresponse = AddColumnInSSR(SSRresponse);
                        getSSRoutbound = FilterSSR(SSRresponse, "O");
                        getSSRinbound = FilterSSR(SSRresponse, "I");
                    }
                }
            }
            catch (Exception ex)
            {
               // dbCommon.Logger.dbLogg(CompanyID, 0, "GetSSRAvailabilityCombine", "clsSSRavailability", SelectAirRS, SearchID, ex.Message);
            }

            var ssrAvailability = new FlightSSRAvailabilityInfo
            {
                SSRresponse = SSRresponse,
                SSRresponseOut = getSSRoutbound,
                SSRresponseIn = getSSRinbound
            };

            return ssrAvailability;
        }
        public static async Task<FlightSSRAvailabilityInfo>  GetSSRAvailabilityRT(string SearchID, string CompanyID, string AirRS, string RefID_O, string RefID_I, IHandlesQueryAsync<GetAirSSRQuery, string> getAirSSRHandler)
        {
            string SelectAirRS = string.Empty;
            var getSSRoutbound = string.Empty;
            var getSSRinbound = string.Empty;
            string SSRresponse = string.Empty;
            try
            {
                if (AirRS != null && AirRS.IndexOf("RefID") != -1 && RefID_O != null && RefID_O.Length > 0 && RefID_I != null && RefID_I.Length > 0)
                {
                    SelectAirRS = SelectedResponseHelper.GETSelectedResponse(RefID_O, RefID_I, AirRS, false);
                    string userIpAddress = HttpContextHelper.Current?.Request.HttpContext.Connection.RemoteIpAddress.ToString();
                    string Host = HttpContextHelper.Current?.Request.Host + "," + userIpAddress;

                    //dbCommon.Logger.dbSearchLogg(CompanyID, 0, Initializer.get_StaffID(), SearchID, "SSR", "DISPLAY", "FO", SelectAirRS, "", Host);

                    var sessionResult = HttpContextHelper.Current?.Session?.GetString("SSRInfo");
                    if (!string.IsNullOrEmpty(sessionResult) && GetCarrierCode(SelectAirRS).Equals("6E"))
                    {
                        DataTable dtSSRItinerary = JsonConvert.DeserializeObject<DataTable>(sessionResult);
                        dtSSRItinerary.TableName = "SSRInfo";

                        DataSet dsBound = new DataSet();
                        dsBound.Tables.Add(dtSSRItinerary.Copy());

                        dsBound.DataSetName = "root";
                        SSRresponse = CommonFunction.DataSetToString(dsBound);
                    }
                    else
                    {
                        
                        var getAirSSQuery = new GetAirSSRQuery
                        {
                            JourneyType = "RTLCC",
                            SearchID = SearchID,
                            CompanyID = CompanyID,
                            AirRS = SelectAirRS
                        };
                        SSRresponse = await getAirSSRHandler.HandleAsync(getAirSSQuery);
                    }

                    if (SSRresponse.IndexOf("SSRInfo") != -1)
                    {
                        SSRresponse = AddColumnInSSR(SSRresponse);
                        getSSRoutbound = FilterSSR(SSRresponse, "O");
                        getSSRinbound = FilterSSR(SSRresponse, "I");
                    }
                }
            }
            catch (Exception ex)
            {
               // dbCommon.Logger.dbLogg(CompanyID, 0, "GetSSRAvailabilityRT", "clsSSRavailability", SelectAirRS, SearchID, ex.Message);
            }

            var ssrAvailability = new FlightSSRAvailabilityInfo
            {
                SSRresponse = SSRresponse,
                SSRresponseOut = getSSRoutbound,
                SSRresponseIn = getSSRinbound
            };

            return ssrAvailability;
        }
        private static string FilterSSR(string SSRresponse, string FltType)
        {
            string SelectedResponse = string.Empty;

            try
            {
                if (SSRresponse.Trim() != "")
                {
                    XmlDocument xmldoc = new XmlDocument();
                    xmldoc.LoadXml(SSRresponse);

                    if (FltType.Equals("O"))
                    {
                        XmlNodeList xnList1 = xmldoc.SelectNodes("/root/SSRInfo[FltType='O']");
                        SelectedResponse = "<root>";
                        if (xnList1 != null)
                        {
                            foreach (XmlNode node in xnList1)
                            {
                                SelectedResponse += node.OuterXml;
                            }
                        }
                        SelectedResponse += "</root>";
                    }
                    else
                    {
                        XmlNodeList xnList1 = xmldoc.SelectNodes("/root/SSRInfo[FltType='I']");
                        SelectedResponse = "<root>";
                        if (xnList1 != null)
                        {
                            foreach (XmlNode node in xnList1)
                            {
                                SelectedResponse += node.OuterXml;
                            }
                        }
                        SelectedResponse += "</root>";
                    }
                }
            }
            catch
            {

            }

            return SelectedResponse;
        }
        
        private static string GetCarrierCode(string TResponse)
        {
            if (TResponse.Trim() != "")
            {
                DataSet dsAvailability = new DataSet();
                dsAvailability.ReadXml(new System.IO.StringReader(TResponse));

                return dsAvailability.Tables["AvailabilityInfo"].Rows[0]["CarrierCode"].ToString();
            }
            return "";
        }
        public string SecDetails { get; set; }
        public string AirlineCode { get; set; }
        public string GetSSRResult()
        {
            return "";
        }
        private static string AddColumnInSSR(string SSRdetail)
        {
            DataSet dsSSRdetail = CommonFunction.StringToDataSet(SSRdetail);
            DataTable dtBound = dsSSRdetail.Tables["SSRInfo"].Copy();

            if (dtBound.Columns.Contains("AmountINR").Equals(false))
            {
                DataColumn col = new DataColumn();
                col.ColumnName = "AmountINR";
                col.DataType = typeof(Int32);
                col.DefaultValue = 0;
                dtBound.Columns.Add(col);
                dtBound.AcceptChanges();
            }

            foreach (DataRow dr in dtBound.Rows)
            {
                if (dr["Detail"].ToString().Trim().Equals(""))
                {
                    dr["Detail"] = "No-Data";
                }

                dr["AmountINR"] = dr["Amount"];
                dr["Amount"] = ShowFlightDataHelper.GetConvert(Convert.ToInt32(dr["Amount"].ToString()));
            }
            dtBound.AcceptChanges();

            return CommonFunction.DataTableToString(dtBound, "SSRInfo", "root");
        }
       
    }
}
