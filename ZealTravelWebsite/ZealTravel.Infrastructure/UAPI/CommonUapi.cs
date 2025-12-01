using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Xml;
using ZealTravel.Common.CommonUtility;
using ZealTravel.Common.Helpers;
using ZealTravel.Infrastructure.Context;

namespace ZealTravel.Infrastructure.UAPI
{
    class CommonUapi
    {
        public int GetRefidStartid()
        {
            return 3000;
        }
        public DataTable RemoveClosePriceTypeWiseFare(string SearchID, DataTable dtBound, string SearchType, string Supplierid, string CarrierCode, string Sector)
        {
            DataTable ndtBound = dtBound.Clone();

            try
            {
                DataTable dtClosePriceType = GetClosePriceTypeFares(Supplierid, CarrierCode, Sector);
                if (dtClosePriceType != null && dtClosePriceType.Rows.Count > 0)
                {
                    if (SearchType.Equals("OW") || SearchType.Equals("RW"))
                    {
                        ArrayList ArRefid = new ArrayList();
                        foreach (DataRow dr in dtClosePriceType.Rows)
                        {
                            DataRow[] drSelect = dtBound.Select("PriceType='" + dr["PriceType"].ToString().Trim() + "'");
                            if (drSelect.Length > 0)
                            {
                                ArRefid.AddRange(DBCommon.CommonFunction.DataTable2ArrayList(drSelect.CopyToDataTable(), "Refid", true));
                            }
                        }

                        if (ArRefid != null && ArRefid.Count > 0)
                        {
                            ArrayList nList = new ArrayList();
                            for (int i = 0; i < ArRefid.Count; i++)
                            {
                                nList.Add(int.Parse(ArRefid[i].ToString()));
                            }

                            var results = (from row in dtBound.AsEnumerable()
                                           where !nList.Contains(row.Field<Int32>("RefID"))
                                           select row).ToList();
                            ndtBound = results.CopyToDataTable();

                            return ndtBound;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                DBCommon.Logger.dbLogg("", 0, "", "RemoveClosePriceTypeWiseFare", Supplierid, SearchID, ex.Message);
            }
            return dtBound;
        }
        public DataTable OnlyPriceTypeWiseFare(string SearchID, DataTable dtBound, string SearchType, string Supplierid, string CarrierCode, string Sector)
        {
            DataTable ndtBound = dtBound.Clone();

            try
            {
                string PriceType = "#Corporate";
                if (SearchType.Equals("OW") || SearchType.Equals("RW"))
                {
                    ArrayList ArRefid = new ArrayList();
                    DataRow[] drSelect = dtBound.Select("PriceType='" + PriceType + "'");
                    if (drSelect.Length > 0)
                    {
                        ArRefid.AddRange(DBCommon.CommonFunction.DataTable2ArrayList(drSelect.CopyToDataTable(), "RefID", true));
                    }

                    if (ArRefid != null && ArRefid.Count > 0)
                    {
                        ArrayList nList = new ArrayList();
                        for (int i = 0; i < ArRefid.Count; i++)
                        {
                            nList.Add(int.Parse(ArRefid[i].ToString()));
                        }

                        var results = (from row in dtBound.AsEnumerable()
                                       where nList.Contains(row.Field<Int32>("RefID"))
                                       select row).ToList();
                        ndtBound = results.CopyToDataTable();

                        return ndtBound;
                    }
                }
            }
            catch (Exception ex)
            {
                DBCommon.Logger.dbLogg("", 0, "", "OnlyPriceTypeWiseFare", Supplierid, SearchID, ex.Message);
            }
            return dtBound;
        }
        public DataTable GetClosePriceTypeFares(string Supplierid, string CarrierCode, string Sector)
        {
            DataTable dtPriceTypeFares = new DataTable();
            try
            {
                //using (SqlConnection connection = new SqlConnection(DBCommon.ConnectionString.dbConnect))
                //{
                //    SqlCommand cmd = new SqlCommand("Group_Commission_PriceType_Proc", connection);
                //    cmd.CommandType = CommandType.StoredProcedure;
                //    cmd.CommandTimeout = 10;
                //    cmd.Parameters.Add(@"ProcNo", SqlDbType.Int).Value = 6;
                //    cmd.Parameters.Add(@"CarrierCode", SqlDbType.VarChar).Value = CarrierCode;
                //    cmd.Parameters.Add(@"Supplierid", SqlDbType.VarChar).Value = Supplierid;
                //    cmd.Parameters.Add(@"Sector", SqlDbType.VarChar).Value = Sector;
                //    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                //    adapter.Fill(dtPriceTypeFares);
                //}
            }
            catch (Exception ex)
            {
                DBCommon.Logger.dbLogg("", 0, "", "GetClosePriceTypeFares", Supplierid, CarrierCode, ex.Message);
            }
            return dtPriceTypeFares;
        }
        //public DataTable RemoveUncombinedData(string SearchID, string TRequest, string TResponse, DataTable dtBound)
        //{
        //    try
        //    {
        //        DataTable dtUpdatedBound = dtBound.Clone();
        //        ArrayList Ar_RefId = new ArrayList();
        //        Ar_RefId = DBCommon.CommonFunction.DataTable2ArrayList(dtBound, "RefId", true);
        //        for (int i = 0; i < Ar_RefId.Count; i++)
        //        {
        //            DataRow[] drOutbound = dtBound.Select("RefID='" + Ar_RefId[i].ToString() + "' AND FltType='" + "O" + "'");
        //            DataRow[] drInbound = dtBound.Select("RefID='" + Ar_RefId[i].ToString() + "' AND FltType='" + "I" + "'");
        //            if (drOutbound.Length > 0 && drInbound.Length > 0)
        //            {
        //                dtUpdatedBound.Merge(drOutbound.CopyToDataTable().Copy());
        //                dtUpdatedBound.Merge(drInbound.CopyToDataTable().Copy());
        //            }
        //        }

        //        return dtUpdatedBound;
        //    }
        //    catch (Exception ex)
        //    {
        //        DBCommon.Logger.dbLogg("", 0, "RemoveUncombinedData", TResponse, TRequest, SearchID, ex.Message);
        //        return dtBound;
        //    }
        //}
        public DataTable RemoveUncombinedData(string SearchID, string TRequest, string TResponse, DataTable dtBound)
        {
            try
            {
                DataTable dtUpdatedBound = dtBound.Clone();
                ArrayList Ar_RefId = new ArrayList();
                Ar_RefId = DBCommon.CommonFunction.DataTable2ArrayList(dtBound, "RefId", true);
                for (int i = 0; i < Ar_RefId.Count; i++)
                {
                    DataRow[] drOutbound = dtBound.Select("RefID='" + Ar_RefId[i].ToString() + "' AND FltType='" + "O" + "'");
                    DataRow[] drInbound = dtBound.Select("RefID='" + Ar_RefId[i].ToString() + "' AND FltType='" + "I" + "'");
                    if (drOutbound.Length > 0 && drInbound.Length > 0)
                    {
                        if (ValidBetweenSector(drOutbound.CopyToDataTable()))
                        {
                            if (ValidBetweenSector(drInbound.CopyToDataTable()))
                            {
                                dtUpdatedBound.Merge(drOutbound.CopyToDataTable().Copy());
                                dtUpdatedBound.Merge(drInbound.CopyToDataTable().Copy());
                            }
                            else
                            {

                            }
                        }
                        else
                        {

                        }                       
                    }
                }
                Add_Stops(dtUpdatedBound);
                return dtUpdatedBound;
            }
            catch (Exception ex)
            {
                DBCommon.Logger.dbLogg("", 0, "RemoveUncombinedData", TResponse, TRequest, SearchID, ex.Message);
                return dtBound;
            }
        }
        public DataTable RemoveUncombinedDataOW(string SearchID, string TRequest, string TResponse, DataTable dtBound)
        {
            try
            {
                DataTable dtUpdatedBound = dtBound.Clone();
                ArrayList Ar_RefId = new ArrayList();
                Ar_RefId = DBCommon.CommonFunction.DataTable2ArrayList(dtBound, "RefId", true);
                for (int i = 0; i < Ar_RefId.Count; i++)
                {
                    DataRow[] drOutbound = dtBound.Select("RefID='" + Ar_RefId[i].ToString() + "'");
                    if (drOutbound.Length > 0)
                    {
                        if (ValidBetweenSector(drOutbound.CopyToDataTable()))
                        {
                            dtUpdatedBound.Merge(drOutbound.CopyToDataTable().Copy());
                        }
                        else
                        {

                        }
                    }
                }
                Add_Stops(dtUpdatedBound);
                return dtUpdatedBound;
            }
            catch (Exception ex)
            {
                DBCommon.Logger.dbLogg("", 0, "RemoveUncombinedDataOW", TResponse, TRequest, SearchID, ex.Message);
                return dtBound;
            }
        }
        public bool ValidBetweenSector(DataTable dtBound)
        {
            int count = dtBound.Rows.Count;
            if (dtBound.Rows.Count.Equals(1))
            {
                string src1 = dtBound.Rows[0]["DepartureStation"].ToString();
                string des1 = dtBound.Rows[0]["ArrivalStation"].ToString();

                string org = dtBound.Rows[0]["Origin"].ToString();
                string des = dtBound.Rows[0]["Destination"].ToString();

                if (src1.Equals(org) && des.Equals(des1))
                {
                    return true;
                }
            }
            else if (dtBound.Rows.Count.Equals(2))
            {
                string src1 = dtBound.Rows[0]["DepartureStation"].ToString();
                string des1 = dtBound.Rows[0]["ArrivalStation"].ToString();
                string src2 = dtBound.Rows[1]["DepartureStation"].ToString();
                string des2 = dtBound.Rows[1]["ArrivalStation"].ToString();

                if (des1.Equals(src2))
                {
                    return true;
                }
            }
            else if (dtBound.Rows.Count.Equals(3))
            {
                string src1 = dtBound.Rows[0]["DepartureStation"].ToString();
                string des1 = dtBound.Rows[0]["ArrivalStation"].ToString();
                string src2 = dtBound.Rows[1]["DepartureStation"].ToString();
                string des2 = dtBound.Rows[1]["ArrivalStation"].ToString();
                string src3 = dtBound.Rows[2]["DepartureStation"].ToString();
                string des4 = dtBound.Rows[2]["ArrivalStation"].ToString();

                if (des1 == src2 && des2 == src3)
                {
                    return true;
                }
            }
            return false;
        }
        private void Add_Stops(DataTable dtBound)
        {
            try
            {
                foreach (DataRow dr in dtBound.Rows)
                {
                    if (Convert.ToInt32(dr["Stops"].ToString().Trim()).Equals(0))
                    {
                        if (dr["FltType"].ToString().Equals("O"))
                        {
                            DataRow[] drStops = dtBound.Select("RefID='" + dr["RefID"].ToString() + "' And FltType='" + "O" + "'");
                            dr["Stops"] = drStops.Length - 1;
                        }
                        else if (dr["FltType"].ToString().Equals("I"))
                        {
                            DataRow[] drStops = dtBound.Select("RefID='" + dr["RefID"].ToString() + "' And FltType='" + "I" + "'");
                            dr["Stops"] = drStops.Length - 1;
                        }
                    }
                }

                dtBound.AcceptChanges();
            }
            catch
            {

            }
        }
        //public string GetBaggageDetails(string Carrier, string Sector, string Origin, string Destination)
        //{
        //    string Baggage = "";
        //    try
        //    {
        //        using (SqlConnection connection = new SqlConnection(DBCommon.ConnectionString.dbConnect))
        //        {
        //            SqlCommand cmd = new SqlCommand("Airline_Baggage_Details_Proc", connection);
        //            cmd.CommandType = CommandType.StoredProcedure;
        //            cmd.CommandTimeout = 10;
        //            cmd.Parameters.Add(@"ProcNo", SqlDbType.Int).Value = 1;
        //            cmd.Parameters.Add(@"Carrier", SqlDbType.VarChar).Value = Carrier;
        //            cmd.Parameters.Add(@"Sector", SqlDbType.VarChar).Value = Sector;
        //            cmd.Parameters.Add(@"Origin", SqlDbType.VarChar).Value = Origin;
        //            cmd.Parameters.Add(@"Destination", SqlDbType.VarChar).Value = Destination;

        //            connection.Open();
        //            Baggage = cmd.ExecuteScalar().ToString();
        //            connection.Close();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        DBCommon.Logger.dbLogg(Sector, 0, Sector, "GetBaggageDetails", "Airline_Baggage_Details_Proc", Carrier, ex.Message);
        //    }
        //    return Baggage;
        //}
        public DataTable GetdbSSRList(string CarrierCode, string ProductClass)
        {
            var dtSSR = new DataTable();
            try
            {
                var db = DatabaseContextFactory.CreateDbContext();
                var airlineSsrLists =  db.AirlineSsrLists.FromSqlRaw("EXEC Airline_SSR_List_Proc @ProcNo = 2, @CarrierCode = {0}, @ProductClass = {1}", CarrierCode, ProductClass).ToListAsync();
                return airlineSsrLists.Result.ToDataTable();
            }
            catch (Exception ex)
            {
                DBCommon.Logger.dbLogg("", 0, "", "GetdbSSRList", "air_uapi", CarrierCode, ex.Message);
            }

            return dtSSR;
        }
        public static string RemoveSpces(string text)
        {
            text = Regex.Replace(text, @"\t", " ").Trim();
            text = Regex.Replace(text, @"\n", " ").Trim();
            text = Regex.Replace(text, @"\r", " ").Trim();
            return text;
        }
        //=====================================================================================================================================
        public static void Add_Journey_Duration_TimeDetail(DataTable dtBound)
        {
            try
            {
                SetJourneyTime(dtBound);
                //foreach (DataRow dr in dtBound.Rows)
                //{
                //    dr["JourneyTimeDesc"] = DBCommon.DateTimeFormatter.ConvertTmDesc(dr["JourneyTime"].ToString());

                //    if (Convert.ToInt32(dr["Duration"].ToString()) > 0)
                //    {
                //        dr["DurationDesc"] = DBCommon.DateTimeFormatter.ConvertTmDesc(dr["Duration"].ToString());
                //    }
                //    else
                //    {
                //        dr["Duration"] = DBCommon.DateTimeFormatter.GetDuration(dr["DepDate"].ToString(), dr["ArrDate"].ToString());
                //        dr["DurationDesc"] = DBCommon.DateTimeFormatter.ConvertTmDesc(dr["Duration"].ToString());
                //    }

                //    dr["DepartureDate"] = DBCommon.DateTimeFormatter.DateFormat(dr["DepDate"].ToString());
                //    dr["ArrivalDate"] = DBCommon.DateTimeFormatter.DateFormat(dr["ArrDate"].ToString());
                //    dr["DepartureTime"] = DBCommon.DateTimeFormatter.TimeFormat(dr["DepTime"].ToString());
                //    dr["ArrivalTime"] = DBCommon.DateTimeFormatter.TimeFormat(dr["ArrTime"].ToString());
                //}


                dtBound.AcceptChanges();
            }
            catch
            {

            }
        }
        private static void SetJourneyTime(DataTable dtBound)
        {
            foreach (DataRow dr in dtBound.Rows)
            {
                if (Convert.ToInt32(dr["JourneyTime"].ToString()).Equals(0))
                {
                    SetJourneyTime(dr["RefID"].ToString(), dr["FltType"].ToString(), dtBound);
                }
            }
        }
        private static void SetJourneyTime(string RefID, string FltType, DataTable dtBound)
        {
            int iJourneyTime = 0;
            DataRow[] rows = dtBound.Select("RefID ='" + RefID + "' And FltType='" + FltType + "'");
            if (rows.Length > 0)
            {
                foreach (DataRow row in rows)
                {
                    if (Convert.ToInt32(row["JourneyTime"].ToString()) > 0)
                    {
                        iJourneyTime = Convert.ToInt32(row["JourneyTime"].ToString());
                        break;
                    }
                }
                if (iJourneyTime > 0)
                {
                    foreach (DataRow row in rows)
                    {
                        if (Convert.ToInt32(row["JourneyTime"].ToString()).Equals(0))
                        {
                            row["JourneyTime"] = iJourneyTime;
                        }
                        dtBound.AcceptChanges();
                        row.SetModified();
                    }
                }
                else
                {
                    //iJourneyTime = DBCommon.DateTimeFormatter.GetDuration(rows.CopyToDataTable().Rows[0]["DepDate"].ToString(), rows.CopyToDataTable().Rows[rows.CopyToDataTable().Rows.Count - 1]["ArrDate"].ToString());
                    //if (iJourneyTime > 0)
                    //{
                    //    foreach (DataRow row in rows)
                    //    {
                    //        if (Convert.ToInt32(row["JourneyTime"].ToString()).Equals(0))
                    //        {
                    //            row["JourneyTime"] = iJourneyTime;
                    //        }
                    //        dtBound.AcceptChanges();
                    //        row.SetModified();
                    //    }
                    //}
                }
            }
        }
        //=====================================================================================================================================
        public static string RetrievePNR(string SearchID, string CompanyID, int BookingRef, string Response, out string GalileoPNR,out string AirReservationLocatorCode,out string UniversalRecordLocatorCode)
        {
            string AirlinePNR = "";
            GalileoPNR = "";
            AirReservationLocatorCode="";
            UniversalRecordLocatorCode = "";
            try
            {
                XmlDocument xmlflt = new XmlDocument();
                xmlflt.LoadXml(Response);
                XmlElement root = xmlflt.DocumentElement;
                if (root.HasChildNodes)
                {
                    for (int i = 0; i < root.ChildNodes.Count; i++)
                    {
                        string s12 = root.ChildNodes[i].InnerXml;

                        XmlDocument xmlflt1 = new XmlDocument();
                        xmlflt1.LoadXml(s12);
                        XmlElement root1 = xmlflt1.DocumentElement;
                        if (root1.HasChildNodes)
                        {
                            for (int j = 0; j < root1.ChildNodes.Count; j++)
                            {
                                string s123 = root1.ChildNodes[j].OuterXml;

                                XmlDocument xmlflt2 = new XmlDocument();
                                xmlflt2.LoadXml(s123);
                                XmlElement root2 = xmlflt2.DocumentElement;

                                // --- INJECT: UniversalRecordLocatorCode from <universal:UniversalRecord> ---
                                if (root2.LocalName == "UniversalRecord"
                                    && root2.NamespaceURI.Contains("universal"))
                                {
                                    // the "LocatorCode" attribute on the UniversalRecord element
                                    var attr = root2.GetAttribute("LocatorCode");
                                    if (!string.IsNullOrEmpty(attr))
                                        UniversalRecordLocatorCode = attr;
                                }

                                if (root2.HasChildNodes)
                                {
                                    for (int k = 0; k < root2.ChildNodes.Count; k++)
                                    {
                                        string Nodes = root2.ChildNodes[k].OuterXml;
                                        if (Nodes.IndexOf("air:AirReservation") != -1)
                                        {
                                            DataSet dsAvailability = new DataSet();
                                            dsAvailability.ReadXml(new System.IO.StringReader(Nodes));

                                            if (dsAvailability != null && dsAvailability.Tables["AirReservation"] != null)
                                            {
                                                AirReservationLocatorCode = dsAvailability.Tables["AirReservation"].Rows[0]["LocatorCode"].ToString();
                                            }

                                            if (dsAvailability != null && dsAvailability.Tables.Count > 1)
                                            {
                                                if (dsAvailability.Tables["SupplierLocator"] != null)
                                                {
                                                    AirlinePNR = dsAvailability.Tables["SupplierLocator"].Rows[0]["SupplierLocatorCode"].ToString();
                                                }
                                            }
                                        }

                                        if (Nodes.IndexOf("universal:ProviderReservationInfo") != -1)
                                        {
                                            DataSet dsAvailability = new DataSet();
                                            dsAvailability.ReadXml(new System.IO.StringReader(Nodes));

                                            if (dsAvailability.Tables["ProviderReservationInfo"] != null)
                                            {
                                                GalileoPNR = dsAvailability.Tables["ProviderReservationInfo"].Rows[0]["LocatorCode"].ToString();
                                            }
                                        }
                                    }                             
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                DBCommon.Logger.dbLogg(CompanyID, BookingRef, "RetrievePNR", "air_uapi", Response, SearchID, ex.Message);
            }
            return AirlinePNR;
        }
        public static string GetBookingTravelerRef(string BookingTravelerRef, int RowID)
        {
            if (BookingTravelerRef != null && BookingTravelerRef.Length > 0 && BookingTravelerRef.IndexOf("?") != -1)
            {
                string[] split = BookingTravelerRef.Split('?');
                return split[RowID - 1].ToString();
            }
            else if (BookingTravelerRef != null && BookingTravelerRef.Length > 0)
            {
                return BookingTravelerRef;
            }
            return "";
        }
        public static string FareQuote_rr(string CompanyID, string SearchID, string SearchCriteria, string FlightStatus, bool IsPriceChanged, bool IsTimeChanged, bool IsETicketEligible, String Status, String ResponseStatus, int ErrorCode, string ErrorMessage, bool IsUpdated)
        {
            string FareQuote = string.Empty;
            try
            {
                FareQuote += "FlightStatus:" + FlightStatus.Trim() + ",";
                FareQuote += "IsPriceChanged:" + IsPriceChanged + ",";
                FareQuote += "IsTimeChanged:" + IsTimeChanged + ",";
                FareQuote += "IsETicketEligible:" + IsETicketEligible + ",";
                FareQuote += "Status:" + Status.Trim() + ",";
                FareQuote += "ResponseStatus:" + ResponseStatus.Trim() + ",";
                FareQuote += "ErrorCode:" + ErrorCode + ",";
                FareQuote += "ErrorMessage:" + ErrorMessage.Trim() + ",";
                FareQuote += "IsUpdated:" + IsUpdated;
            }
            catch (Exception ex)
            {
                DBCommon.Logger.dbLogg(CompanyID, 0, "FareQuote_rr", "Schema", SearchCriteria, SearchID, ex.Message);
            }
            return FareQuote;
        }
        public static string GetRequestMC(ArrayList AirlineList, string Cabin, Int16 Adult, Int16 Child, Int16 Infant)
        {
            StringBuilder ReqXml = new StringBuilder();
            ReqXml.Append("<AvailabilityRequest>");
            ReqXml.Append("<Cabin>" + Cabin + "</Cabin>");

            ReqXml.Append("<AirSearch>");

            ReqXml.Append("<AirSrchInfo>");
            ReqXml.Append("<DepartureStation>" + "DEL" + " </DepartureStation>");
            ReqXml.Append("<ArrivalStation>" + "BOM" + "</ArrivalStation>");
            ReqXml.Append("<StartDate>" + "20200110" + "</StartDate>");
            ReqXml.Append("</AirSrchInfo>");

            ReqXml.Append("<AirSrchInfo>");
            ReqXml.Append("<DepartureStation>" + "BOM" + " </DepartureStation>");
            ReqXml.Append("<ArrivalStation>" + "AMD" + "</ArrivalStation>");
            ReqXml.Append("<StartDate>" + "20200112" + "</StartDate>");
            ReqXml.Append("</AirSrchInfo>");

            ReqXml.Append("<AirSrchInfo>");
            ReqXml.Append("<DepartureStation>" + "BLR" + " </DepartureStation>");
            ReqXml.Append("<ArrivalStation>" + "DEL" + "</ArrivalStation>");
            ReqXml.Append("<StartDate>" + "20200115" + "</StartDate>");
            ReqXml.Append("</AirSrchInfo>");

            //ReqXml.Append("<AirSrchInfo>");
            //ReqXml.Append("<DepartureStation>" + "DEL" + " </DepartureStation>");
            //ReqXml.Append("<ArrivalStation>" + "BOM" + "</ArrivalStation>");
            //ReqXml.Append("<StartDate>" + "20191017" + "</StartDate>");
            //ReqXml.Append("</AirSrchInfo>");


            ReqXml.Append("</AirSearch>");

            ReqXml.Append("<AirVAry>");
            for (int i = 0; i < AirlineList.Count; i++)
            {
                ReqXml.Append("<AirVInfo>");
                ReqXml.Append("<AirV>" + AirlineList[i].ToString() + "</AirV>");
                ReqXml.Append("</AirVInfo>");
            }
            ReqXml.Append("</AirVAry>");

            ReqXml.Append("<Adult>" + Adult + "</Adult>");
            ReqXml.Append("<Child>" + Child + "</Child>");
            ReqXml.Append("<Infant>" + Infant + "</Infant>");
            ReqXml.Append("</AvailabilityRequest>");
            return ReqXml.ToString();
        }
        public static string GetRequest(string DepartureStation, string ArrivalStation, string Cabin, ArrayList AirlineList, string BeginDate, string ArrivalDate, int Adult, int Child, int Infant)
        {
            StringBuilder ReqXml = new StringBuilder();
            ReqXml.Append("<AvailabilityRequest>");
            ReqXml.Append("<DepartureStation>" + DepartureStation + "</DepartureStation>");
            ReqXml.Append("<ArrivalStation>" + ArrivalStation + "</ArrivalStation>");

            ReqXml.Append("<Cabin>" + Cabin + "</Cabin>");
            ReqXml.Append("<AirVAry>");
            for (int i = 0; i < AirlineList.Count; i++)
            {
                ReqXml.Append("<AirVInfo>");
                ReqXml.Append("<AirV>" + AirlineList[i].ToString() + "</AirV>");
                ReqXml.Append("</AirVInfo>");
            }

            ReqXml.Append("</AirVAry>");
            ReqXml.Append("<StartDate>" + BeginDate + "</StartDate>");
            ReqXml.Append("<EndDate>" + ArrivalDate + "</EndDate>");
            ReqXml.Append("<Adult>" + Adult + "</Adult>");
            ReqXml.Append("<Child>" + Child + "</Child>");
            ReqXml.Append("<Infant>" + Infant + "</Infant>");
            ReqXml.Append("<SplFare>" + false + "</SplFare>");
            ReqXml.Append("</AvailabilityRequest>");
            return ReqXml.ToString();
        }

        public string GetResponseUapi(string NetworkUserName, string NetworkPassword, string SearchID, string requestData, string Method, string ServiceName)
        {
            string responseXML = string.Empty;
            string responseJSON = string.Empty;

            try
            {
                //P7025891 //test
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                //ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
                string url = ConfigurationHelper.GetSetting("GalileoAirline:BaseURL") + Method; 

                byte[] data = Encoding.UTF8.GetBytes(requestData);
               
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "POST";
                request.ContentType = "text/xml";
                request.Accept = "text/xml";
                request.Headers.Add(@"SOAP:Action");
                request.Headers.Add("Accept-Encoding", "gzip");
                request.KeepAlive = true;

                request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
                NetworkCredential cred = new NetworkCredential(NetworkUserName, NetworkPassword);//live
                //NetworkCredential cred = new NetworkCredential("Universal API/uAPI6776316127-4450f3c2", "9f&QrX2!G_"); //test
                //NetworkCredential cred = new NetworkCredential("Universal API/uAPI4997970939-5270ffc4", "n/8JX4i}5m"); //test
                request.Credentials = cred;

                Stream dataStream = request.GetRequestStream();
                dataStream.Write(data, 0, data.Length);
                dataStream.Close();

                using (WebResponse response = request.GetResponse())
                {
                    using (StreamReader rd = new StreamReader(response.GetResponseStream()))
                    {
                        responseJSON = rd.ReadToEnd();
                    }
                }
            }
            catch (WebException ex)
            {
                DBCommon.Logger.dbLogg("", 0, "GetResponseUapi", requestData, responseXML, SearchID, Method + "-" + ServiceName + "-" + ex.Message);
            }

           // WriteFileToxml(ServiceName, requestData, ServiceName + "_RQ");
           // WriteFileToxml(ServiceName, responseJSON, ServiceName + "RS");
            return responseJSON;
        }

        public async System.Threading.Tasks.Task<string> GetResponseUapiAsync(string NetworkUserName, string NetworkPassword, string SearchID, string requestData, string Method, string ServiceName)
        {
            string responseXML = string.Empty;
            string responseJSON = string.Empty;

            try
            {
                
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                string url = ConfigurationHelper.GetSetting("GalileoAirline:BaseURL") + Method; //test
             

                byte[] data = Encoding.UTF8.GetBytes(requestData);

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "POST";
                request.ContentType = "text/xml";
                request.Accept = "text/xml";
                request.Headers.Add(@"SOAP:Action");
                request.Headers.Add("Accept-Encoding", "gzip");
                request.KeepAlive = true;

                request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
                NetworkCredential cred = new NetworkCredential(NetworkUserName, NetworkPassword);//live
                request.Credentials = cred;

                Stream dataStream = request.GetRequestStream();
                dataStream.Write(data, 0, data.Length);
                dataStream.Close();

                using (WebResponse response = await request.GetResponseAsync())
                {
                    
                    using (StreamReader rd = new StreamReader(response.GetResponseStream()))
                    {
                        responseJSON =await rd.ReadToEndAsync();
                    }
                }
            }
            catch (WebException ex)
            {
                DBCommon.Logger.dbLogg("", 0, "GetResponseUapi", requestData, responseXML, SearchID, Method + "-" + ServiceName + "-" + ex.Message);
            }

            WriteFileToxml(ServiceName, requestData, ServiceName + "_RQ");
            WriteFileToxml(ServiceName, responseJSON, ServiceName + "RS");
            return responseJSON;
        }

        private static void WriteFileToxml(string SearchID, string Response, string FileName)
        {
            try
            {
                string Path = @"C://uapiLog//" + FileName + ".xml";
                System.IO.StreamWriter file = new System.IO.StreamWriter(Path);
                file.WriteLine(Response);
                file.Close();
            }
            catch(Exception ex)
            {

            }
        }
        /*private static void WriteFileToxmlAsync(string SearchID, string Response, string FileName)
        {
            try
            {
                string Path = @"C://uapiLog//" + FileName + ".xml";
                System.IO.StreamWriter file = new System.IO.StreamWriter(Path);
                file.WriteLineAsync(Response);
                file.Close();
            }
            catch (Exception ex)
            {

            }
        }*/
        public static DataTable getCompanyDetail_Pnr(string CompanyID)
        {
            DataTable dtCompany = new DataTable();
            //try
            //{
            //    using (SqlConnection connection = new SqlConnection(DBCommon.ConnectionString.dbConnect))
            //    {
            //        SqlCommand cmd = new SqlCommand("Company_Register_Proc", connection);
            //        cmd.CommandType = CommandType.StoredProcedure;
            //        cmd.CommandTimeout = 10;
            //        cmd.Parameters.Add(@"ProcNo", SqlDbType.Int).Value = 35;
            //        cmd.Parameters.Add(@"CompanyID", SqlDbType.VarChar).Value = CompanyID;
            //        SqlDataAdapter da = new SqlDataAdapter(cmd);
            //        da.Fill(dtCompany);
            //    }
            //}
            //catch (Exception ex)
            //{
            //    DBCommon.Logger.dbLogg(CompanyID, 0, "getCompanyName", "apiAir_tbo", "tboQuery", "", ex.Message);
            //}

            return dtCompany;
        }
        public DataRow GetCCDetails(string CarrierCode)
        {
            DataTable dtCreditcard = new DataTable();
            try
            {
                //using (SqlConnection connection = new SqlConnection(DBCommon.ConnectionString.dbConnect))
                //{
                //    SqlCommand cmd = new SqlCommand("UAPI_FORM_OF_PAYMENT", connection);
                //    cmd.CommandType = CommandType.StoredProcedure;
                //    cmd.CommandTimeout = 10;
                //    cmd.Parameters.Add(@"row", SqlDbType.Int).Value = 1;
                //    SqlDataAdapter da = new SqlDataAdapter(cmd);
                //    da.Fill(dtCreditcard);
                //}
            }
            catch (Exception ex)
            {
                DBCommon.Logger.dbLogg("", 0, "GetCCDetails", "apiAir_uapi", "", "", ex.Message);
            }

            if (dtCreditcard != null && dtCreditcard.Rows.Count > 0)
            {
                foreach (DataRow dr in dtCreditcard.Rows)
                {
                    if (dr["Carriers"].ToString().IndexOf(CarrierCode) != -1)
                    {
                        return dr;
                    }
                }
                return dtCreditcard.Rows[0];
            }
            else if (dtCreditcard != null && dtCreditcard.Rows.Count.Equals(1))
            {
                return dtCreditcard.Rows[0];
            }

            return null;
        }
        public string GetPromoAirlinewise(string CarrierCode)
        {
            //if (CarrierCode.Equals("AI"))
            //{
            //    return "TMU1Z0.50";
            //}
            //else if (CarrierCode.Equals("UK"))
            //{
            //    return "FQ/CUK-:ZEAL0";
            //}
            //else
            //{
                return "";
            //}
        }
        public string GetAccountCodeUK()
        {
            return "";
            //return "ZEAL0";
        }
        public string GetAccountCodeAI()
        {
            return "SME";
        }
        public string GetAccountCommissionAirlineWise(string CarrierCode)
        {
            if (CarrierCode.Equals("AI"))
            {
                return "0.50";
            }
            else
            {
                return "";
            }
        }
        public string GetActiveFormOfPayment()
        {
            string FOP = "CASH";
            try
            {
                DataTable dtCreditcard = new DataTable();
                //using (SqlConnection connection = new SqlConnection(DBCommon.ConnectionString.dbConnect))
                //{
                //    SqlCommand cmd = new SqlCommand("UAPI_FORM_OF_PAYMENT", connection);
                //    cmd.CommandType = CommandType.StoredProcedure;
                //    cmd.CommandTimeout = 10;
                //    cmd.Parameters.Add(@"row", SqlDbType.Int).Value = 2;

                //    connection.Open();
                //    FOP = cmd.ExecuteScalar().ToString();
                //    connection.Close();
                //}
            }
            catch (Exception ex)
            {
                DBCommon.Logger.dbLogg("", 0, "GetActiveFormOfPayment", "apiAir_uapi", "", "", ex.Message);
            }

            return FOP;
        }
        public bool IsTicketAutoMode(string Companyid, string CarrierCode, string Sector, int BookingRef)
        {
            try
            {
                //using (SqlConnection connection = new SqlConnection(DBCommon.ConnectionString.dbConnect))
                //{
                //    SqlCommand cmd = new SqlCommand("Group_Commission_Rule_New_Proc", connection);
                //    cmd.CommandType = CommandType.StoredProcedure;
                //    cmd.CommandTimeout = 10;
                //    cmd.Parameters.Add(@"ProcNo", SqlDbType.Int).Value = 11;
                //    cmd.Parameters.Add(@"CompanyID", SqlDbType.VarChar).Value = Companyid;
                //    cmd.Parameters.Add(@"CarrierCode", SqlDbType.VarChar).Value = CarrierCode;
                //    cmd.Parameters.Add(@"Sector", SqlDbType.VarChar).Value = Sector;

                //    connection.Open();
                //    bool Status = (bool)cmd.ExecuteScalar();
                //    connection.Close();

                //    return Status;
                //}
            }
            catch (Exception ex)
            {
                DBCommon.Logger.dbLogg(Companyid, BookingRef, "IsTicketAutoMode", "apiAir_uapi", "Group_Commission_Rule_New_Proc-11", "", ex.Message);
            }
            return false;
        }

        /// <summary>
        /// Extracts from the **retrieve** response:
        ///   • universalRecordLocatorCode
        ///   • universalRecordVersion
        ///   • providerLocatorCode
        ///   • reservationLocatorCode
        ///   • airPricingInfoKey
        /// </summary>
        public static void RetrieveBookingIdentifiers(
            string responseXml,
            out string universalRecordLocatorCode,
            out string universalRecordVersion,
            out string providerLocatorCode,
            out string reservationLocatorCode,
            out string airPricingInfoKey,
            out string carrierCode)
        {
            universalRecordLocatorCode = "";
            universalRecordVersion = "";
            providerLocatorCode = "";
            reservationLocatorCode = "";
            airPricingInfoKey = "";
            carrierCode = "";

            var xml = new XmlDocument();
            xml.LoadXml(responseXml);
            var root = xml.DocumentElement;
            if (root == null) return;

            for (int i = 0; i < root.ChildNodes.Count; i++)
            {
                // level 1: Envelope → Body
                var xml1 = new XmlDocument();
                xml1.LoadXml(root.ChildNodes[i].InnerXml);
                var body = xml1.DocumentElement;
                if (body == null) continue;

                for (int j = 0; j < body.ChildNodes.Count; j++)
                {
                    // level 2: Body → UniversalRecordRetrieveRsp → children
                    var xml2 = new XmlDocument();
                    xml2.LoadXml(body.ChildNodes[j].OuterXml);
                    var ur = xml2.DocumentElement;
                    if (ur == null) continue;

                    // 1) PNR + Version
                    if (ur.LocalName == "UniversalRecord" &&
                        ur.NamespaceURI.Contains("universal"))
                    {
                        var loc = ur.GetAttribute("LocatorCode");
                        if (!string.IsNullOrEmpty(loc))
                            universalRecordLocatorCode = loc;

                        var ver = ur.GetAttribute("Version");
                        if (!string.IsNullOrEmpty(ver))
                            universalRecordVersion = ver;
                    }

                    // level 3: children of UniversalRecord
                    foreach (XmlNode child in ur.ChildNodes)
                    {
                        var nodeXml = child.OuterXml;

                        // 2) AirReservation → Locator + Pricing Key
                        if (nodeXml.Contains("air:AirReservation"))
                        {
                            var ds = new DataSet();
                            ds.ReadXml(new StringReader(nodeXml));

                            if (ds.Tables["AirReservation"] != null)
                                reservationLocatorCode =
                                    ds.Tables["AirReservation"].Rows[0]["LocatorCode"].ToString();

                            if (ds.Tables["AirPricingInfo"] != null)
                                airPricingInfoKey =
                                    ds.Tables["AirPricingInfo"].Rows[0]["Key"].ToString();

                            
                            var segTbl = ds.Tables["AirSegment"];
                            if (segTbl != null && segTbl.Rows.Count > 0)
                                carrierCode = segTbl.Rows[0]["Carrier"].ToString();
                        }

                        // 3) ProviderReservationInfo → LocatorCode
                        if (nodeXml.Contains("universal:ProviderReservationInfo"))
                        {
                            var dsProv = new DataSet();
                            dsProv.ReadXml(new StringReader(nodeXml));

                            if (dsProv.Tables["ProviderReservationInfo"] != null)
                                providerLocatorCode =
                                    dsProv.Tables["ProviderReservationInfo"]
                                          .Rows[0]["LocatorCode"]
                                          .ToString();
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Extracts from the **modify** response:
        ///   • firstCarrier     (first air:AirSegment → Carrier)
        ///   • hkSegmentKey     (Status="HK" segment → Key)
        ///   • supplierLocator  (common:SupplierLocator → SupplierLocatorCode)
        ///   • airReservationLocatorCode
        ///   • segment status
        /// </summary>
        public static void RetrieveBookingIdentifiersAfterPayment(
            string modifyResponseXml,
            out string firstCarrier,
            out string hkSegmentKey,
            out string supplierLocatorCode,
            out string airReservationLocatorCode,
            out string status)
        {
            firstCarrier = "";
            hkSegmentKey = "";
            supplierLocatorCode = "";
            airReservationLocatorCode = "";
            status = "";

            var xml = new XmlDocument();
            xml.LoadXml(modifyResponseXml);
            var root = xml.DocumentElement;
            if (root == null) return;

            for (int i = 0; i < root.ChildNodes.Count; i++)
            {
                var xml1 = new XmlDocument();
                xml1.LoadXml(root.ChildNodes[i].InnerXml);
                var body = xml1.DocumentElement;
                if (body == null) continue;

                for (int j = 0; j < body.ChildNodes.Count; j++)
                {
                    var xml2 = new XmlDocument();
                    xml2.LoadXml(body.ChildNodes[j].OuterXml);
                    var ur = xml2.DocumentElement;
                    if (ur == null) continue;

                    foreach (XmlNode child in ur.ChildNodes)
                    {
                        var nodeXml = child.OuterXml;

                        // AirReservation → Locator + segments
                        if (nodeXml.Contains("air:AirReservation"))
                        {
                            // Locator
                            var dsRes = new DataSet();
                            dsRes.ReadXml(new StringReader(nodeXml));
                            if (dsRes.Tables["AirReservation"] != null)
                                airReservationLocatorCode =
                                    dsRes.Tables["AirReservation"].Rows[0]["LocatorCode"].ToString();

                            // segments
                            var segDoc = new XmlDocument();
                            segDoc.LoadXml(nodeXml);
                            var nsm = new XmlNamespaceManager(segDoc.NameTable);
                            nsm.AddNamespace("air", "http://www.travelport.com/schema/air_v51_0");

                            var segs = segDoc.SelectNodes("//air:AirSegment", nsm);
                            if (segs != null && segs.Count > 0)
                            {
                                // firstCarrier
                                var firstSeg = (XmlElement)segs[0];
                                if (firstSeg.HasAttribute("Carrier"))
                                    firstCarrier = firstSeg.GetAttribute("Carrier");

                                // HK segment
                                foreach (XmlElement seg in segs)
                                {
                                    if (seg.GetAttribute("Status") == "HK")
                                    {
                                        if (seg.HasAttribute("Key"))
                                            hkSegmentKey = seg.GetAttribute("Key");
                                        status = "HK";
                                        break;
                                    }
                                }
                            }
                        }

                        // SupplierLocator → SupplierLocatorCode
                        if (nodeXml.Contains("common_v51_0:SupplierLocator"))
                        {
                            var dsSup = new DataSet();
                            dsSup.ReadXml(new StringReader(nodeXml));
                            if (dsSup.Tables["SupplierLocator"] != null)
                                supplierLocatorCode =
                                    dsSup.Tables["SupplierLocator"].Rows[0]["SupplierLocatorCode"]
                                    .ToString();
                        }
                    }
                }
            }
        }




    }
}
