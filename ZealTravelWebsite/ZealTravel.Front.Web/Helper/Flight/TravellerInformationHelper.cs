using System;
using System.Data;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using ZealTravel.Common;
using ZealTravel.Common.Helpers;
using ZealTravel.Common.Helpers.Flight;
using ZealTravel.Front.Web.Models.Flight;

namespace ZealTravel.Front.Web.Helper.Flight
{
    public class TravellerInformationHelper
    {
        public static string GetTravellerInformation()
        {
            string passengers = HttpContextHelper.Current?.Session.GetString("PaxXML") ?? string.Empty;
            DataTable dtPassenger = CommonFunction.StringToDataSet(passengers).Tables[0];

            StringBuilder Itinary = new StringBuilder();
            Itinary.Append(@"<div class=""col-md-12 col-xs-12 offset-0"">");
            int k = 1;
            foreach (DataRow dr in dtPassenger.Rows)
            {
                string paxtype = dr["paxType"].ToString();
                string TITLE = dr["title"].ToString();
                if (dr["paxType"].ToString().Equals("ADT"))
                {
                    paxtype = "adt";
                }
                else if (dr["paxType"].ToString().Equals("CHD"))
                {
                    paxtype = "chd";
                }
                else if (dr["paxType"].ToString().Equals("INF"))
                {
                    paxtype = "inf";
                }

                Itinary.Append(@"<div class=""col-md-12  offset-0"" style=""padding-top: 10px; padding-bottom: 10px;"">");
                Itinary.Append(@"<div class=""col-md-12 offset-0 m-top-5"">");
                Itinary.Append(@"<span style = ""font-size: 15px; color: rgb(51, 51, 51);"" >");
                Itinary.Append(@"<img src=""/assets/img/travelicon.png"" class=""hisedicon"" />");
                Itinary.Append(@"<span class=""srno""> " + k.ToString() + " </span>");
                Itinary.Append(@"<span class=""name""> " + TITLE + " " + StringHelper.GetFirstCharacterCapital(dr["First_Name"].ToString() + " " + dr["Last_Name"].ToString()) + "," + paxtype + " </span>");
                Itinary.Append(@"</span>");
                Itinary.Append(@"</div>");
                Itinary.Append(@"</div>");
                k++;
            }

            Itinary.Append(@"<div class=""col-md-12  offset-0"" style=""padding-top: 2px;padding-bottom: 7px;"">");
            Itinary.Append(@"<div class=""col-md-12 offset-0 m-top-5"">");
            Itinary.Append(@"<span class=""name"" style = ""font-size:15px;""> Contact Details</span>");
            Itinary.Append(@"</div>");
            Itinary.Append(@"<div class=""col-md-12 offset-0 finagrossinr"" style=""font-size: 15px;""> <span>   <img src = ""/assets/img/mailbox.png"" class=""hisedicon"" />  " + dtPassenger.Rows[0]["Email"].ToString().ToLower() + "</span> </div>");
            Itinary.Append(@"<div class=""col-md-12 offset-0 finagrossinr"" style=""font-size: 15px;""> <span>  <img src = ""/assets/img/phonic.png"" class=""hisedicon"" />   " + dtPassenger.Rows[0]["MobileNo"].ToString().ToLower() + " </span> </div>");
            Itinary.Append(@"</div>");

            Itinary.Append(@"</div>");

            return Itinary.ToString();
        }
        public static void GetFilghtDisplayJsFareInformation(out int markup, out int tds, out int totalfare, out int discount, out int cfee)
        {
            markup = 0;
            tds = 0;
            totalfare = 0;
            discount = 0;
            cfee = 0;
            Int32 dssr = GETTotalSSR();
            var companyID = UserHelper.GetCompanyID(HttpContextHelper.Current.User);

            var sessionResult4MCList = HttpContextHelper.Current?.Session.GetComplexObject<List<SessionResult4MC>>("SessionResult4MCList");
            if (sessionResult4MCList != null)
            {

                string SelectedAvailabilityResponse = "";
                markup = 0;
                totalfare = 0;
                cfee = 0;
                tds = 0;
                discount = 0;

                if (sessionResult4MCList != null)
                {
                    foreach (SessionResult4MC o in (sessionResult4MCList).OrderBy(x => x._SrNo).ToList())
                    {
                       
                        SelectedAvailabilityResponse = SelectedResponseHelper.GETSelectedResponse(o.SelectedFltOut.ToString().Trim(), "", o.FinalResult.ToString(), false);
                        DataTable dtresponse = CommonFunction.StringToDataSet(SelectedAvailabilityResponse).Tables[0];

                        markup += Convert.ToInt32(dtresponse.Rows[0]["TotalMarkup"].ToString());
                        totalfare += Convert.ToInt32(dtresponse.Rows[0]["TotalFare"].ToString());
                        cfee += Convert.ToInt32(dtresponse.Rows[0]["TotalCfee"].ToString());
                        tds += Convert.ToInt32(dtresponse.Rows[0]["TotalTds"].ToString());
                        discount += Convert.ToInt32(dtresponse.Rows[0]["TotalCommission"].ToString());
                        if (companyID.Contains("-SA-", StringComparison.CurrentCulture) || companyID.Contains("C-", StringComparison.CurrentCulture) || companyID.Equals(string.Empty))
                        {
                            tds = Convert.ToInt32(dtresponse.Rows[0]["TotalTds_SA"].ToString());
                            discount = Convert.ToInt32(dtresponse.Rows[0]["TotalCommission_SA"].ToString());
                        }
                    }
                }
                totalfare = totalfare + dssr - tds;
            }
            else if (HttpContextHelper.Current?.Session?.GetString("SearchValue")?.ToString().Equals("OW") == true)
            {

                string SelectedAvailabilityResponse = SelectedResponseHelper.GETSelectedResponse(HttpContextHelper.Current?.Session?.GetString("SelectedFltOut").Trim(), "", HttpContextHelper.Current?.Session?.GetString("FinalResult").ToString(), false);
                DataTable dtresponse = CommonFunction.StringToDataSet(SelectedAvailabilityResponse).Tables[0];

                markup = Convert.ToInt32(dtresponse.Rows[0]["TotalMarkup"].ToString());
                totalfare = Convert.ToInt32(dtresponse.Rows[0]["TotalFare"].ToString());
                cfee = Convert.ToInt32(dtresponse.Rows[0]["TotalCfee"].ToString());
                tds = Convert.ToInt32(dtresponse.Rows[0]["TotalTds"].ToString());
                discount = Convert.ToInt32(dtresponse.Rows[0]["TotalCommission"].ToString());

                if (companyID.Contains("-SA-", StringComparison.CurrentCulture) || companyID.Contains("C-", StringComparison.CurrentCulture) || companyID.Equals(string.Empty))
                {
                    tds = Convert.ToInt32(dtresponse.Rows[0]["TotalTds_SA"].ToString());
                    discount = Convert.ToInt32(dtresponse.Rows[0]["TotalCommission_SA"].ToString());
                }

                totalfare = totalfare + dssr - tds;
            }
            else if (HttpContextHelper.Current?.Session?.GetString("SearchValue")?.ToString().Equals("RW") == true)
            {
                string SelectedAvailabilityResponse = SelectedResponseHelper.GETSelectedResponse(HttpContextHelper.Current?.Session?.GetString("SelectedFltOut").Trim(), HttpContextHelper.Current?.Session?.GetString("SelectedFltIn").Trim(), HttpContextHelper.Current?.Session?.GetString("FinalResult").ToString(), false);
                DataTable dtresponse = CommonFunction.StringToDataSet(SelectedAvailabilityResponse).Tables[0];

                DataRow[] drSelect = dtresponse.Select("FltType='" + "O" + "'");

                markup = Convert.ToInt32(drSelect.CopyToDataTable().Rows[0]["TotalMarkup"].ToString());
                totalfare = Convert.ToInt32(drSelect.CopyToDataTable().Rows[0]["TotalFare"].ToString());
                cfee = Convert.ToInt32(drSelect.CopyToDataTable().Rows[0]["TotalCfee"].ToString());

                if (companyID.Contains("-SA-", StringComparison.CurrentCulture) || companyID.Contains("C-", StringComparison.CurrentCulture) || companyID.Equals(string.Empty))
                {
                    tds = Convert.ToInt32(drSelect.CopyToDataTable().Rows[0]["TotalTds_SA"].ToString());
                    discount = Convert.ToInt32(drSelect.CopyToDataTable().Rows[0]["TotalCommission_SA"].ToString());
                }
                else
                {
                    tds = Convert.ToInt32(drSelect.CopyToDataTable().Rows[0]["TotalTds"].ToString());
                    discount = Convert.ToInt32(drSelect.CopyToDataTable().Rows[0]["TotalCommission"].ToString());
                }

                //=================================================================================================================

                drSelect = dtresponse.Select("FltType='" + "I" + "'");

                markup += Convert.ToInt32(drSelect.CopyToDataTable().Rows[0]["TotalMarkup"].ToString());
                totalfare += Convert.ToInt32(drSelect.CopyToDataTable().Rows[0]["TotalFare"].ToString());

                if (companyID.Contains("-SA-", StringComparison.CurrentCulture) || companyID.Contains("C-", StringComparison.CurrentCulture) || companyID.Equals(string.Empty))
                {
                    tds += Convert.ToInt32(drSelect.CopyToDataTable().Rows[0]["TotalTds_SA"].ToString());
                    discount += Convert.ToInt32(drSelect.CopyToDataTable().Rows[0]["TotalCommission_SA"].ToString());
                }
                else
                {
                    tds += Convert.ToInt32(drSelect.CopyToDataTable().Rows[0]["TotalTds"].ToString());
                    discount += Convert.ToInt32(drSelect.CopyToDataTable().Rows[0]["TotalCommission"].ToString());
                }

                totalfare = totalfare + dssr - tds;
            }
            else if (HttpContextHelper.Current?.Session?.GetString("SearchValue")?.ToString().Equals("RT") == true)
            {
                string SelectedAvailabilityResponse = SelectedResponseHelper.GETSelectedResponse(HttpContextHelper.Current?.Session?.GetString("SelectedFltOut").Trim(), HttpContextHelper.Current?.Session?.GetString("SelectedFltIn").Trim(), HttpContextHelper.Current?.Session?.GetString("FinalResult").ToString(), false);
                DataTable dtresponse = CommonFunction.StringToDataSet(SelectedAvailabilityResponse).Tables[0];

                markup = Convert.ToInt32(dtresponse.Rows[0]["TotalMarkup"].ToString());
                totalfare = Convert.ToInt32(dtresponse.Rows[0]["TotalFare"].ToString());
                cfee = Convert.ToInt32(dtresponse.Rows[0]["TotalCfee"].ToString());
                tds = Convert.ToInt32(dtresponse.Rows[0]["TotalTds"].ToString());
                discount = Convert.ToInt32(dtresponse.Rows[0]["TotalCommission"].ToString());

                if (companyID.Contains("-SA-", StringComparison.CurrentCulture) || companyID.Contains("C-", StringComparison.CurrentCulture) || companyID.Equals(string.Empty))
                {
                    tds = Convert.ToInt32(dtresponse.Rows[0]["TotalTds_SA"].ToString());
                    discount = Convert.ToInt32(dtresponse.Rows[0]["TotalCommission_SA"].ToString());
                }

                totalfare = totalfare + dssr - tds;
            }
            else if (HttpContextHelper.Current?.Session?.GetString("SearchValue")?.ToString().Equals("INT") == true)
            {

                string SelectedAvailabilityResponse = SelectedResponseHelper.GETSelectedResponse(HttpContextHelper.Current?.Session?.GetString("SelectedFltOut").Trim(), "", HttpContextHelper.Current?.Session?.GetString("FinalResult").ToString(), true);
                DataTable dtresponse = CommonFunction.StringToDataSet(SelectedAvailabilityResponse).Tables[0];

                markup = Convert.ToInt32(dtresponse.Rows[0]["TotalMarkup"].ToString());
                totalfare = Convert.ToInt32(dtresponse.Rows[0]["TotalFare"].ToString());
                cfee = Convert.ToInt32(dtresponse.Rows[0]["TotalCfee"].ToString());
                tds = Convert.ToInt32(dtresponse.Rows[0]["TotalTds"].ToString());
                discount = Convert.ToInt32(dtresponse.Rows[0]["TotalCommission"].ToString());

                if (companyID.Contains("-SA-", StringComparison.CurrentCulture) || companyID.Contains("C-", StringComparison.CurrentCulture) || companyID.Equals(string.Empty))
                {
                    tds = Convert.ToInt32(dtresponse.Rows[0]["TotalTds_SA"].ToString());
                    discount = Convert.ToInt32(dtresponse.Rows[0]["TotalCommission_SA"].ToString());
                }

                totalfare = totalfare + dssr - tds;
            }
        }
        public static void SetXmlNode(out string Outbound, out string Inbound)
        {
            string SelectedFltOut = HttpContextHelper.Current?.Session?.GetString("SelectedFltOut").Trim() ?? string.Empty;
            var FinalResult = HttpContextHelper.Current?.Session?.GetString("FinalResult") ?? string.Empty;
            SetXmlNode(out Outbound, out Inbound, SelectedFltOut, FinalResult);
        }
        public static void SetXmlNode(out string Outbound, out string Inbound, string SelectedFltOut, string FinalResult)
        {
            Outbound = string.Empty;
            Inbound = string.Empty;
            var searchValue = HttpContextHelper.Current?.Session.GetString("SearchValue")?.ToString();
            if (SelectedFltOut == "" || FinalResult == "")
            {
                return;
            }

            //string SelectedFltOut = HttpContext.Current.Session["SelectedFltOut"].ToString().Trim();
            string SelectedFltIn = string.Empty;
            if (HttpContextHelper.Current?.Session.GetString("SelectedFltIn") != null)
            {
                SelectedFltIn = HttpContextHelper.Current?.Session.GetString("SelectedFltIn");
            }

            #region
            if (searchValue.Equals("INT") == true)
            {
                List<ClsFareResultInfo> objClsFareResultInfo = new List<ClsFareResultInfo>();
                DataSet ds = new DataSet();

                //var FinalResult = HttpContext.Current.Session["FinalResult"].ToString();
                ds = XMLHelper.SessionFilterResult(FinalResult);
                DataTable firstTable = ds.Tables[0];
                DataTable SecondTable = ds.Tables[1];
                string OB_Result = XMLHelper.ConvertDatatableToXMLstring(firstTable);
                string IB_Result = XMLHelper.ConvertDatatableToXMLstring(SecondTable);

                string v = OB_Result.Replace("NewDataSet", "root");
                OB_Result = v.Replace("Outbond", "AvailabilityInfo");
                string tt = IB_Result.Replace("NewDataSet", "root");
                IB_Result = tt.Replace("Inbond", "AvailabilityInfo");
                XmlDocument xml = new XmlDocument();

                if (OB_Result.Trim() != "")
                {
                    xml.LoadXml(OB_Result);

                    XmlNodeList xnList = xml.SelectNodes("/root/AvailabilityInfo[RefID='" + SelectedFltOut + "']");
                    foreach (XmlNode node in xnList)
                    {
                        objClsFareResultInfo.Add(new ClsFareResultInfo()
                        {
                            RowID = int.Parse(node["RowID"].InnerText),
                            Trip = node["Trip"].InnerText,
                            Origin = node["Origin"].InnerText,
                            Destination = node["Destination"].InnerText,
                            Stops = Convert.ToInt32(node["Stops"].InnerText),
                            Sector = node["Sector"].InnerText == null || node["Sector"].InnerText == "" ? "D" : node["Sector"].InnerText,
                            CarrierCode = node["CarrierCode"].InnerText,
                            Adt = Convert.ToInt32(node["Adt"].InnerText),
                            Chd = Convert.ToInt32(node["Chd"].InnerText),
                            Inf = Convert.ToInt32(node["Inf"].InnerText),
                            TotalFare = Convert.ToInt32(node["TotalFare"].InnerText),
                            TotalTds = Convert.ToInt32(node["TotalTds"].InnerText),
                            TotalTds_SA = Convert.ToInt32(node["TotalTds_SA"].InnerText),
                            TotalCommission = Convert.ToInt32(node["TotalCommission"].InnerText),
                            TotalCommission_SA = Convert.ToInt32(node["TotalCommission_SA"].InnerText),
                            TotalMarkup = Convert.ToInt32(node["TotalMarkup"].InnerText)
                        });
                    }

                    var xEleIntOutbound = new XElement("SelectedResponse",
                                from obj in objClsFareResultInfo
                                select new XElement("AvailabilityResponseOut",
                                    new XElement("RowID", obj.RowID),
                                    new XElement("Trip", obj.Trip),
                                    new XElement("Origin", obj.Origin),
                                    new XElement("Destination", obj.Destination),
                                    new XElement("Stops", obj.Stops),
                                    new XElement("Sector", obj.Sector),
                                    new XElement("CarrierCode", obj.CarrierCode),
                                    new XElement("Adt", obj.Adt),
                                    new XElement("Chd", obj.Chd),
                                    new XElement("Inf", obj.Inf),
                                    new XElement("TotalFare", obj.TotalFare),
                                    new XElement("TotalMarkup", obj.TotalMarkup),
                                    new XElement("TotalCommission", obj.TotalCommission),
                                    new XElement("TotalCommission_SA", obj.TotalCommission_SA),
                                    new XElement("TotalTds", obj.TotalTds),
                                    new XElement("TotalTds_SA", obj.TotalTds_SA),
                                    new XElement("TotalCfee", obj.TotalCfee)));
                    Outbound = xEleIntOutbound.ToString();
                }
                if (IB_Result.Trim() != "")
                {
                    xml.LoadXml(IB_Result);
                    List<ClsFareResultInfo> objClsFareResultInfoINBond = new List<ClsFareResultInfo>();
                    List<ClsFareResultInfo> obClsFareResultInfo = new List<ClsFareResultInfo>();
                    XmlNodeList xnListInBond = xml.SelectNodes("/root/AvailabilityInfo[RefID='" + SelectedFltOut + "']");
                    foreach (XmlNode nodeIn in xnListInBond)
                    {
                        objClsFareResultInfoINBond.Add(new ClsFareResultInfo()
                        {
                            RowID = int.Parse(nodeIn["RowID"].InnerText),
                            Trip = nodeIn["Trip"].InnerText,
                            Origin = nodeIn["Origin"].InnerText,
                            Destination = nodeIn["Destination"].InnerText,
                            Stops = Convert.ToInt32(nodeIn["Stops"].InnerText),
                            Sector = nodeIn["Sector"].InnerText == null || nodeIn["Sector"].InnerText == "" ? "D" : nodeIn["Sector"].InnerText,
                            CarrierCode = nodeIn["CarrierCode"].InnerText,
                            Adt = Convert.ToInt32(nodeIn["Adt"].InnerText),
                            Chd = Convert.ToInt32(nodeIn["Chd"].InnerText),
                            Inf = Convert.ToInt32(nodeIn["Inf"].InnerText),
                            TotalFare = Convert.ToInt32(nodeIn["TotalFare"].InnerText),
                            TotalTds = Convert.ToInt32(nodeIn["TotalTds"].InnerText),
                            TotalTds_SA = Convert.ToInt32(nodeIn["TotalTds_SA"].InnerText),
                            TotalCommission = Convert.ToInt32(nodeIn["TotalCommission"].InnerText),
                            TotalCommission_SA = Convert.ToInt32(nodeIn["TotalCommission_SA"].InnerText),
                            TotalMarkup = Convert.ToInt32(nodeIn["TotalMarkup"].InnerText)
                        });
                    }

                    var xEleIntInbound = new XElement("SelectedResponse",
                                from obj in objClsFareResultInfoINBond// obClsFareResultInfo
                                select new XElement("AvailabilityResponseIN",
                                new XElement("RowID", obj.RowID),
                                new XElement("Trip", obj.Trip),
                                new XElement("Origin", obj.Origin),
                                new XElement("Destination", obj.Destination),
                                new XElement("Stops", obj.Stops),
                                new XElement("Sector", obj.Sector),
                                new XElement("CarrierCode", obj.CarrierCode),
                                new XElement("Adt", obj.Adt),
                                new XElement("Chd", obj.Chd),
                                new XElement("Inf", obj.Inf),
                                new XElement("TotalFare", obj.TotalFare),
                                new XElement("TotalMarkup", obj.TotalMarkup),
                                new XElement("TotalCommission", obj.TotalCommission),
                                new XElement("TotalCommission_SA", obj.TotalCommission_SA),
                                new XElement("TotalTds", obj.TotalTds),
                                new XElement("TotalTds_SA", obj.TotalTds_SA),
                                new XElement("TotalCfee", obj.TotalCfee)));
                    Inbound = xEleIntInbound.ToString();
                }
            }
            #endregion
            #region
            else if (searchValue.Equals("RT"))
            {
                if (SelectedFltOut != null && SelectedFltOut.Length > 0)
                {
                    List<ClsFareResultInfo> objClsFareResultInfo = new List<ClsFareResultInfo>();
                    //var FinalResult = HttpContext.Current.Session["FinalResult"].ToString();
                    XmlDocument xml = new XmlDocument();
                    xml.LoadXml(FinalResult);
                    XmlNodeList xnList = xml.SelectNodes("/root/AvailabilityInfo[RefID='" + SelectedFltOut + "']");
                    foreach (XmlNode node in xnList)
                    {
                        objClsFareResultInfo.Add(new ClsFareResultInfo()
                        {
                            RowID = int.Parse(node["RowID"].InnerText),
                            Trip = node["Trip"].InnerText,
                            Origin = node["Origin"].InnerText,
                            Destination = node["Destination"].InnerText,
                            Stops = Convert.ToInt32(node["Stops"].InnerText),
                            Sector = node["Sector"].InnerText == null || node["Sector"].InnerText == "" ? "D" : node["Sector"].InnerText,
                            CarrierCode = node["CarrierCode"].InnerText,
                            Adt = Convert.ToInt32(node["Adt"].InnerText),
                            Chd = Convert.ToInt32(node["Chd"].InnerText),
                            Inf = Convert.ToInt32(node["Inf"].InnerText),
                            TotalFare = Convert.ToInt32(node["TotalFare"].InnerText),
                            TotalTds = Convert.ToInt32(node["TotalTds"].InnerText),
                            TotalTds_SA = Convert.ToInt32(node["TotalTds_SA"].InnerText),
                            TotalCommission = Convert.ToInt32(node["TotalCommission"].InnerText),
                            TotalCommission_SA = Convert.ToInt32(node["TotalCommission_SA"].InnerText),
                            TotalMarkup = Convert.ToInt32(node["TotalMarkup"].InnerText)
                        });
                    }

                    var xEleRTOutbound = new XElement("SelectedResponse",
                                from obj in objClsFareResultInfo
                                select new XElement("AvailabilityResponseOut",
                        new XElement("RowID", obj.RowID),
                        new XElement("Trip", obj.Trip),
                        new XElement("Origin", obj.Origin),
                        new XElement("Destination", obj.Destination),
                        new XElement("Stops", obj.Stops),
                        new XElement("Sector", obj.Sector),
                        new XElement("CarrierCode", obj.CarrierCode),
                        new XElement("Adt", obj.Adt),
                        new XElement("Chd", obj.Chd),
                        new XElement("Inf", obj.Inf),
                        new XElement("TotalFare", obj.TotalFare),
                        new XElement("TotalMarkup", obj.TotalMarkup),
                        new XElement("TotalCommission", obj.TotalCommission),
                        new XElement("TotalCommission_SA", obj.TotalCommission_SA),
                        new XElement("TotalTds", obj.TotalTds),
                        new XElement("TotalTds_SA", obj.TotalTds_SA),
                        new XElement("TotalCfee", obj.TotalCfee)));
                    Outbound = xEleRTOutbound.ToString();
                }

                if (SelectedFltIn != null && SelectedFltIn.Length > 0)
                {
                    //var FinalResult = HttpContext.Current.Session["FinalResult"].ToString();
                    XmlDocument xml = new XmlDocument();
                    xml.LoadXml(FinalResult);
                    List<ClsFareResultInfo> objClsFareResultInfoINBond = new List<ClsFareResultInfo>();
                    List<ClsFareResultInfo> obClsFareResultInfo = new List<ClsFareResultInfo>();
                    XmlNodeList xnListInBond = xml.SelectNodes("/root/AvailabilityInfo[RefID='" + SelectedFltIn + "']");//(List<ClsFareResultInfo>)Session["SelectedFltIn"];

                    foreach (XmlNode nodeIn in xnListInBond)
                    {
                        objClsFareResultInfoINBond.Add(new ClsFareResultInfo()
                        {
                            RowID = int.Parse(nodeIn["RowID"].InnerText),
                            Trip = nodeIn["Trip"].InnerText,
                            Origin = nodeIn["Origin"].InnerText,
                            Destination = nodeIn["Destination"].InnerText,
                            Stops = Convert.ToInt32(nodeIn["Stops"].InnerText),
                            Sector = nodeIn["Sector"].InnerText == null || nodeIn["Sector"].InnerText == "" ? "" : nodeIn["Sector"].InnerText,
                            CarrierCode = nodeIn["CarrierCode"].InnerText,
                            Adt = Convert.ToInt32(nodeIn["Adt"].InnerText),
                            Chd = Convert.ToInt32(nodeIn["Chd"].InnerText),
                            Inf = Convert.ToInt32(nodeIn["Inf"].InnerText),
                            TotalFare = Convert.ToInt32(nodeIn["TotalFare"].InnerText),
                            TotalTds = Convert.ToInt32(nodeIn["TotalTds"].InnerText),
                            TotalTds_SA = Convert.ToInt32(nodeIn["TotalTds_SA"].InnerText),
                            TotalCommission = Convert.ToInt32(nodeIn["TotalCommission"].InnerText),
                            TotalCommission_SA = Convert.ToInt32(nodeIn["TotalCommission_SA"].InnerText),
                            TotalMarkup = Convert.ToInt32(nodeIn["TotalMarkup"].InnerText)
                        });
                    }

                    var xEleRTInbound = new XElement("SelectedResponse",
                                from obj in objClsFareResultInfoINBond// obClsFareResultInfo
                                select new XElement("AvailabilityResponseIN",
                        new XElement("RowID", obj.RowID),
                        new XElement("Trip", obj.Trip),
                        new XElement("Origin", obj.Origin),
                        new XElement("Destination", obj.Destination),
                        new XElement("Stops", obj.Stops),
                        new XElement("Sector", obj.Sector),
                        new XElement("CarrierCode", obj.CarrierCode),
                        new XElement("Adt", obj.Adt),
                        new XElement("Chd", obj.Chd),
                        new XElement("Inf", obj.Inf),
                        new XElement("TotalFare", obj.TotalFare),
                        new XElement("TotalMarkup", obj.TotalMarkup),
                        new XElement("TotalCommission", obj.TotalCommission),
                        new XElement("TotalCommission_SA", obj.TotalCommission_SA),
                        new XElement("TotalTds", obj.TotalTds),
                        new XElement("TotalTds_SA", obj.TotalTds_SA),
                        new XElement("TotalCfee", obj.TotalCfee)));
                    Inbound = xEleRTInbound.ToString();
                }
            }
            #endregion
            #region
            else
            {
                if (SelectedFltOut != null && SelectedFltOut.Length > 0)
                {
                    List<ClsFareResultInfo> objClsFareResultInfo = new List<ClsFareResultInfo>();
                    //var FinalResult = HttpContext.Current.Session["FinalResult"].ToString();
                    XmlDocument xml = new XmlDocument();
                    xml.LoadXml(FinalResult);
                    XmlNodeList xnList = xml.SelectNodes("/root/AvailabilityInfo[RefID='" + SelectedFltOut + "']");
                    foreach (XmlNode node in xnList)
                    {
                        objClsFareResultInfo.Add(new ClsFareResultInfo()
                        {
                            RowID = int.Parse(node["RowID"].InnerText),
                            Trip = node["Trip"].InnerText,
                            Origin = node["Origin"].InnerText,
                            Destination = node["Destination"].InnerText,
                            Stops = Convert.ToInt32(node["Stops"].InnerText),
                            Sector = node["Sector"].InnerText == null || node["Sector"].InnerText == "" ? "D" : node["Sector"].InnerText,
                            CarrierCode = node["CarrierCode"].InnerText,
                            Adt = Convert.ToInt32(node["Adt"].InnerText),
                            Chd = Convert.ToInt32(node["Chd"].InnerText),
                            Inf = Convert.ToInt32(node["Inf"].InnerText),
                            TotalFare = Convert.ToInt32(node["TotalFare"].InnerText),
                            TotalTds = Convert.ToInt32(node["TotalTds"].InnerText),
                            TotalTds_SA = Convert.ToInt32(node["TotalTds_SA"].InnerText),
                            TotalCommission = Convert.ToInt32(node["TotalCommission"].InnerText),
                            TotalCommission_SA = Convert.ToInt32(node["TotalCommission_SA"].InnerText),
                            TotalMarkup = Convert.ToInt32(node["TotalMarkup"].InnerText)
                        });
                    }

                    var xEleOutbound = new XElement("SelectedResponse",
                                from obj in objClsFareResultInfo
                                select new XElement("AvailabilityResponseOut",
                    new XElement("RowID", obj.RowID),
                    new XElement("Trip", obj.Trip),
                    new XElement("Origin", obj.Origin),
                    new XElement("Destination", obj.Destination),
                    new XElement("Stops", obj.Stops),
                    new XElement("Sector", obj.Sector),
                    new XElement("CarrierCode", obj.CarrierCode),
                    new XElement("Adt", obj.Adt),
                    new XElement("Chd", obj.Chd),
                    new XElement("Inf", obj.Inf),
                    new XElement("TotalFare", obj.TotalFare),
                    new XElement("TotalMarkup", obj.TotalMarkup),
                    new XElement("TotalCommission", obj.TotalCommission),
                    new XElement("TotalCommission_SA", obj.TotalCommission_SA),
                    new XElement("TotalTds", obj.TotalTds),
                    new XElement("TotalTds_SA", obj.TotalTds_SA),
                    new XElement("TotalCfee", obj.TotalCfee)));
                    Outbound = xEleOutbound.ToString();
                }

                if (SelectedFltIn != null && SelectedFltIn.Length > 0)
                {
                    //var FinalResult = HttpContext.Current.Session["FinalResult"].ToString();
                    XmlDocument xml = new XmlDocument();
                    xml.LoadXml(FinalResult);
                    List<ClsFareResultInfo> objClsFareResultInfoINBond = new List<ClsFareResultInfo>();
                    List<ClsFareResultInfo> obClsFareResultInfo = new List<ClsFareResultInfo>();
                    XmlNodeList xnListInBond = xml.SelectNodes("/root/AvailabilityInfo[RefID='" + SelectedFltIn + "']");

                    foreach (XmlNode nodeIn in xnListInBond)
                    {
                        objClsFareResultInfoINBond.Add(new ClsFareResultInfo()
                        {
                            RowID = int.Parse(nodeIn["RowID"].InnerText),
                            Trip = nodeIn["Trip"].InnerText,
                            Origin = nodeIn["Origin"].InnerText,
                            Destination = nodeIn["Destination"].InnerText,
                            Stops = Convert.ToInt32(nodeIn["Stops"].InnerText),
                            Sector = nodeIn["Sector"].InnerText == null || nodeIn["Sector"].InnerText == "" ? "D" : nodeIn["Sector"].InnerText,
                            CarrierCode = nodeIn["CarrierCode"].InnerText,
                            Adt = Convert.ToInt32(nodeIn["Adt"].InnerText),
                            Chd = Convert.ToInt32(nodeIn["Chd"].InnerText),
                            Inf = Convert.ToInt32(nodeIn["Inf"].InnerText),
                            TotalFare = Convert.ToInt32(nodeIn["TotalFare"].InnerText),
                            TotalTds = Convert.ToInt32(nodeIn["TotalTds"].InnerText),
                            TotalTds_SA = Convert.ToInt32(nodeIn["TotalTds_SA"].InnerText),
                            TotalCommission = Convert.ToInt32(nodeIn["TotalCommission"].InnerText),
                            TotalCommission_SA = Convert.ToInt32(nodeIn["TotalCommission_SA"].InnerText),
                            TotalMarkup = Convert.ToInt32(nodeIn["TotalMarkup"].InnerText)
                        });
                    }

                    var xEleInbound = new XElement("SelectedResponse",
                                from obj in objClsFareResultInfoINBond// obClsFareResultInfo
                                select new XElement("AvailabilityResponseIN",
                               new XElement("RowID", obj.RowID),
                               new XElement("Trip", obj.Trip),
                               new XElement("Origin", obj.Origin),
                               new XElement("Destination", obj.Destination),
                               new XElement("Stops", obj.Stops),
                               new XElement("Sector", obj.Sector),
                               new XElement("CarrierCode", obj.CarrierCode),
                               new XElement("Adt", obj.Adt),
                               new XElement("Chd", obj.Chd),
                               new XElement("Inf", obj.Inf),
                               new XElement("TotalFare", obj.TotalFare),
                               new XElement("TotalMarkup", obj.TotalMarkup),
                               new XElement("TotalCommission", obj.TotalCommission),
                               new XElement("TotalCommission_SA", obj.TotalCommission_SA),
                               new XElement("TotalTds", obj.TotalTds),
                               new XElement("TotalTds_SA", obj.TotalTds_SA),
                               new XElement("TotalCfee", obj.TotalCfee)));
                    Inbound = xEleInbound.ToString();
                }
            }
            #endregion
        }
        public static string Convert2SelectedResponse(List<XmlNode> xnList)
        {
            string Outbound = string.Empty;

            List<ClsFareResultInfo> objClsFareResultInfo = new List<ClsFareResultInfo>();
            foreach (XmlNode node in xnList)
            {
                objClsFareResultInfo.Add(new ClsFareResultInfo()
                {
                    RowID = int.Parse(node["RowID"].InnerText),
                    Trip = node["Trip"].InnerText,
                    Origin = node["Origin"].InnerText,
                    Destination = node["Destination"].InnerText,
                    Stops = Convert.ToInt32(node["Stops"].InnerText),
                    Sector = node["Sector"].InnerText == null || node["Sector"].InnerText == "" ? "D" : node["Sector"].InnerText,
                    CarrierCode = node["CarrierCode"].InnerText,
                    Adt = Convert.ToInt32(node["Adt"].InnerText),
                    Chd = Convert.ToInt32(node["Chd"].InnerText),
                    Inf = Convert.ToInt32(node["Inf"].InnerText),
                    TotalFare = Convert.ToInt32(node["TotalFare"].InnerText),
                    TotalTds = Convert.ToInt32(node["TotalTds"].InnerText),
                    TotalTds_SA = Convert.ToInt32(node["TotalTds_SA"].InnerText),
                    TotalCommission = Convert.ToInt32(node["TotalCommission"].InnerText),
                    TotalCommission_SA = Convert.ToInt32(node["TotalCommission_SA"].InnerText),
                    TotalMarkup = Convert.ToInt32(node["TotalMarkup"].InnerText)
                });
            }

            var xEleIntOutbound = new XElement("SelectedResponse",
                        from obj in objClsFareResultInfo
                        select new XElement("AvailabilityResponseOut",
                            new XElement("RowID", obj.RowID),
                            new XElement("Trip", obj.Trip),
                            new XElement("Origin", obj.Origin),
                            new XElement("Destination", obj.Destination),
                            new XElement("Stops", obj.Stops),
                            new XElement("Sector", obj.Sector),
                            new XElement("CarrierCode", obj.CarrierCode),
                            new XElement("Adt", obj.Adt),
                            new XElement("Chd", obj.Chd),
                            new XElement("Inf", obj.Inf),
                            new XElement("TotalFare", obj.TotalFare),
                            new XElement("TotalMarkup", obj.TotalMarkup),
                            new XElement("TotalCommission", obj.TotalCommission),
                            new XElement("TotalCommission_SA", obj.TotalCommission_SA),
                            new XElement("TotalTds", obj.TotalTds),
                            new XElement("TotalTds_SA", obj.TotalTds_SA),
                            new XElement("TotalCfee", obj.TotalCfee)));
            Outbound = xEleIntOutbound.ToString();

            return Outbound;
        }
        public static void SetXmlNode(string _SelectedFltOut, string _FinalResult, out string Outbound)
        {
            Outbound = string.Empty;
            //Inbound = string.Empty;

            string SelectedFltOut = _SelectedFltOut;
            string SelectedFltIn = string.Empty;
            //if (HttpContext.Current.Session["SelectedFltIn"] != null)
            //{
            //    SelectedFltIn = HttpContext.Current.Session["SelectedFltIn"].ToString();
            //}

            if (SelectedFltOut != null && SelectedFltOut.Length > 0)
            {
                List<ClsFareResultInfo> objClsFareResultInfo = new List<ClsFareResultInfo>();
                var FinalResult = _FinalResult;// HttpContext.Current.Session["FinalResult"].ToString();
                XmlDocument xml = new XmlDocument();
                xml.LoadXml(FinalResult);
                XmlNodeList xnList = xml.SelectNodes("/root/AvailabilityInfo[RefID='" + SelectedFltOut + "']");
                foreach (XmlNode node in xnList)
                {
                    objClsFareResultInfo.Add(new ClsFareResultInfo()
                    {
                        RowID = int.Parse(node["RowID"].InnerText),
                        Trip = node["Trip"].InnerText,
                        Origin = node["Origin"].InnerText,
                        Destination = node["Destination"].InnerText,
                        Stops = Convert.ToInt32(node["Stops"].InnerText),
                        Sector = node["Sector"].InnerText == null || node["Sector"].InnerText == "" ? "D" : node["Sector"].InnerText,
                        CarrierCode = node["CarrierCode"].InnerText,
                        Adt = Convert.ToInt32(node["Adt"].InnerText),
                        Chd = Convert.ToInt32(node["Chd"].InnerText),
                        Inf = Convert.ToInt32(node["Inf"].InnerText),
                        TotalFare = Convert.ToInt32(node["TotalFare"].InnerText),
                        TotalTds = Convert.ToInt32(node["TotalTds"].InnerText),
                        TotalTds_SA = Convert.ToInt32(node["TotalTds_SA"].InnerText),
                        TotalCommission = Convert.ToInt32(node["TotalCommission"].InnerText),
                        TotalCommission_SA = Convert.ToInt32(node["TotalCommission_SA"].InnerText),
                        TotalMarkup = Convert.ToInt32(node["TotalMarkup"].InnerText)
                    });
                }


                var xEleRTOutbound = new XElement("SelectedResponse",
                            from obj in objClsFareResultInfo
                            select new XElement("AvailabilityResponseOut",
                    new XElement("RowID", obj.RowID),
                    new XElement("Trip", obj.Trip),
                    new XElement("Origin", obj.Origin),
                    new XElement("Destination", obj.Destination),
                    new XElement("Stops", obj.Stops),
                    new XElement("Sector", obj.Sector),
                    new XElement("CarrierCode", obj.CarrierCode),
                    new XElement("Adt", obj.Adt),
                    new XElement("Chd", obj.Chd),
                    new XElement("Inf", obj.Inf),
                    new XElement("TotalFare", obj.TotalFare),
                    new XElement("TotalMarkup", obj.TotalMarkup),
                    new XElement("TotalCommission", obj.TotalCommission),
                    new XElement("TotalCommission_SA", obj.TotalCommission_SA),
                    new XElement("TotalTds", obj.TotalTds),
                    new XElement("TotalTds_SA", obj.TotalTds_SA),
                    new XElement("TotalCfee", obj.TotalCfee)));
                Outbound = xEleRTOutbound.ToString();
            }



        }
        public static string GetBackPageName(string k_FlightDisplayAccountId)
        {
            return GetBackPageName(k_FlightDisplayAccountId, false);
        }
        public static string GetBackPageName(string k_FlightDisplayAccountId, bool _IsMultiCity)
        {
            string k_FlightDisplay_pageredirect = string.Empty;
            string searchValue = HttpContextHelper.Current.Session.GetString("SearchValue");

            if (ConfigurationHelper.GetSetting("Company:IsBO").Equals("0"))
            {
                if (_IsMultiCity == true)
                {
                    k_FlightDisplay_pageredirect = "/Flight/multicity";
                }
                else if (searchValue.Equals("OW"))
                {
                    k_FlightDisplay_pageredirect = "/flight/oneway/";
                }
                else if (searchValue.Equals("RW"))
                {
                    k_FlightDisplay_pageredirect = "/flight/round";
                }
                else if (searchValue.Equals("RT"))
                {
                    k_FlightDisplay_pageredirect = "/flight/int";
                }
                else if (searchValue.Equals("INT"))
                {
                    k_FlightDisplay_pageredirect = "/flight/int";
                }
                else if (searchValue.Equals("MC"))
                {
                    k_FlightDisplay_pageredirect = "k_multicity.aspx";
                }
            }
            else if (k_FlightDisplayAccountId != "")
            {
                if (_IsMultiCity == true)
                {
                    k_FlightDisplay_pageredirect = "/Flight/multicity?value=" + EncodeDecodeHelper.EncodeTo64(k_FlightDisplayAccountId.ToString());
                }
                else if (searchValue.Equals("OW"))
                {
                    k_FlightDisplay_pageredirect = "/flight/oneway?value=" + EncodeDecodeHelper.EncodeTo64(k_FlightDisplayAccountId.ToString());
                }
                else if (searchValue.Equals("RW"))
                {
                    k_FlightDisplay_pageredirect = "/flight/round?value=" + EncodeDecodeHelper.EncodeTo64(k_FlightDisplayAccountId.ToString());
                }
                else if (searchValue.Equals("RT"))
                {
                    k_FlightDisplay_pageredirect = "/flight/int?value=" + EncodeDecodeHelper.EncodeTo64(k_FlightDisplayAccountId.ToString());
                }
                else if (searchValue.Equals("INT"))
                {
                    k_FlightDisplay_pageredirect = "/flight/int?value=" + EncodeDecodeHelper.EncodeTo64(k_FlightDisplayAccountId.ToString());
                }
                else if (searchValue.Equals("MC"))
                {
                    k_FlightDisplay_pageredirect = "/Flight/multicity?value=" + EncodeDecodeHelper.EncodeTo64(k_FlightDisplayAccountId.ToString());
                }
            }
            return k_FlightDisplay_pageredirect;
        }
        public static void GetFilghtShow(out StringBuilder OutbountItinary, out StringBuilder Faredetaildiv, bool IsPaymentPage)
        {
            StringBuilder sbHtmlItinary = new StringBuilder();
            StringBuilder sbHtmlFare = new StringBuilder();
            int totalfare = 0;
            string searchValue = HttpContextHelper.Current?.Session.GetString("SearchValue");

            if (searchValue.Equals("OW"))
            {
                string SelectedAvailabilityResponse = SelectedResponseHelper.GETSelectedResponse(HttpContextHelper.Current.Session.GetString("SelectedFltOut").Trim(), "", HttpContextHelper.Current.Session.GetString("FinalResult"), false);
                DataTable dtresponse = CommonFunction.StringToDataSet(SelectedAvailabilityResponse).Tables[0];
                DataSet dsresponse = CommonFunction.StringToDataSet(SelectedAvailabilityResponse);
                sbHtmlItinary = FlightDisplayItnaryHelper.Itinary(dtresponse, out sbHtmlFare, out totalfare, IsPaymentPage);
            }
            else if (searchValue.Equals("RW"))
            {
                string SelectedAvailabilityResponse = SelectedResponseHelper.GETSelectedResponse(HttpContextHelper.Current.Session.GetString("SelectedFltOut").Trim(), HttpContextHelper.Current.Session.GetString("SelectedFltIn").Trim(), HttpContextHelper.Current.Session.GetString("FinalResult"), false);
                DataTable dtresponse = CommonFunction.StringToDataSet(SelectedAvailabilityResponse).Tables[0];
                DataSet dsresponse = CommonFunction.StringToDataSet(SelectedAvailabilityResponse);
                sbHtmlItinary = FlightDisplayItnaryHelper.Itinary(dtresponse, out sbHtmlFare, out totalfare, IsPaymentPage);
            }
            else if (searchValue.Equals("RT"))
            {
                string SelectedAvailabilityResponse = SelectedResponseHelper.GETSelectedResponse(HttpContextHelper.Current.Session.GetString("SelectedFltOut").Trim(), HttpContextHelper.Current.Session.GetString("SelectedFltIn").Trim(), HttpContextHelper.Current.Session.GetString("FinalResult"), false);
                DataTable dtresponse = CommonFunction.StringToDataSet(SelectedAvailabilityResponse).Tables[0];
                DataSet dsresponse = CommonFunction.StringToDataSet(SelectedAvailabilityResponse);
                sbHtmlItinary = FlightDisplayItnaryHelper.Itinary(dtresponse, out sbHtmlFare, out totalfare, IsPaymentPage);
            }
            else if (searchValue.Equals("INT"))
            {
                string SelectedAvailabilityResponse = SelectedResponseHelper.GETSelectedResponse(HttpContextHelper.Current.Session.GetString("SelectedFltOut").Trim(), "", HttpContextHelper.Current.Session.GetString("FinalResult"), true);
                DataTable dtresponse = CommonFunction.StringToDataSet(SelectedAvailabilityResponse).Tables[0];
                DataSet dsresponse = CommonFunction.StringToDataSet(SelectedAvailabilityResponse);
                sbHtmlItinary = FlightDisplayItnaryHelper.Itinary(dtresponse, out sbHtmlFare, out totalfare, IsPaymentPage);
            }
            else if (searchValue.Equals("MC"))
            {
                HttpContextHelper.Current.Session.SetString("SearchValue", "OW");

                DataTable dtresponse = new DataTable();
                string Outbound = string.Empty;
                string Inbound = string.Empty;

                var sessionResult4MCList = HttpContextHelper.Current?.Session.GetComplexObject<List<SessionResult4MC>>("SessionResult4MCList");
                if (sessionResult4MCList != null)
                {
                    foreach (SessionResult4MC o in sessionResult4MCList.OrderBy(x => x._SrNo).ToList())
                    {
                        HttpContextHelper.Current.Session.SetString("FinalResult", o.FinalResult);
                        HttpContextHelper.Current.Session.SetString("SearchID", o.SearchID);
                        HttpContextHelper.Current.Session.SetString("SelectedFltOut", o.SelectedFltOut);
                        HttpContextHelper.Current.Session.SetString("SearchValue", o.SearchValue);

                        string SelectedAvailabilityResponse = SelectedResponseHelper.GETSelectedResponse(HttpContextHelper.Current.Session.GetString("SelectedFltOut").Trim(), "", HttpContextHelper.Current.Session.GetString("FinalResult"), false);
                        DataTable _Innerdtresponse = CommonFunction.StringToDataSet(SelectedAvailabilityResponse).Tables[0];

                        if (dtresponse.Rows.Count == 0)
                        {
                            dtresponse = _Innerdtresponse.Clone();
                        }
                        foreach (DataRow r in _Innerdtresponse.Rows)
                        {
                            dtresponse.ImportRow(r);
                        }
                    }
                }

                sbHtmlItinary = FlightDisplayItnaryHelper.Itinary(dtresponse, out sbHtmlFare, out totalfare, IsPaymentPage, true);
            }

            OutbountItinary = sbHtmlItinary;
            Faredetaildiv = sbHtmlFare;
        }
        public static Int32 GETTotalSSR()
        {
            Int32 dSSR = 0;
            string passengers = HttpContextHelper.Current.Session.GetString("PaxXML");
            DataTable dtPassenger = CommonFunction.StringToDataSet(passengers).Tables[0];

            try
            {
                foreach (DataRow dr in dtPassenger.Rows)
                {
                    if (dr["MealChg_O"].ToString().Trim().Equals(string.Empty))
                    {
                        dr["MealChg_O"] = "0";
                    }
                    if (dr["MealChg_I"].ToString().Trim().Equals(string.Empty))
                    {
                        dr["MealChg_I"] = "0";
                    }
                    if (dr["BaggageChg_O"].ToString().Trim().Equals(string.Empty))
                    {
                        dr["BaggageChg_O"] = "0";
                    }
                    if (dr["BaggageChg_I"].ToString().Trim().Equals(string.Empty))
                    {
                        dr["BaggageChg_I"] = "0";
                    }

                }

                //---------------------------------ADULT------------------------------------------------------
                DataRow[] dtSelectPax = dtPassenger.Select("TRIM(MealChg_O) > 0 AND TRIM(PaxType)='" + "ADT" + "'");
                if (dtSelectPax.Length > 0)
                {
                    foreach (DataRow drp in dtSelectPax.CopyToDataTable().Rows)
                    {
                        dSSR += decimal.ToInt32(Convert.ToDecimal(drp["MealChg_O"].ToString().Trim()));
                    }
                }


                dtSelectPax = dtPassenger.Select("TRIM(BaggageChg_O) > 0 AND TRIM(PaxType)='" + "ADT" + "'");
                if (dtSelectPax.Length > 0)
                {
                    foreach (DataRow drp in dtSelectPax.CopyToDataTable().Rows)
                    {
                        dSSR += decimal.ToInt32(Convert.ToDecimal(drp["BaggageChg_O"].ToString().Trim()));
                    }
                }
                //--------------------------------------------------------------------------------------------
                dtSelectPax = dtPassenger.Select("TRIM(MealChg_I) > 0 AND TRIM(PaxType)='" + "ADT" + "'");
                if (dtSelectPax.Length > 0)
                {
                    foreach (DataRow drp in dtSelectPax.CopyToDataTable().Rows)
                    {
                        dSSR += decimal.ToInt32(Convert.ToDecimal(drp["MealChg_I"].ToString().Trim()));
                    }
                }


                dtSelectPax = dtPassenger.Select("TRIM(BaggageChg_I) > 0 AND TRIM(PaxType)='" + "ADT" + "'");
                if (dtSelectPax.Length > 0)
                {
                    foreach (DataRow drp in dtSelectPax.CopyToDataTable().Rows)
                    {
                        dSSR += decimal.ToInt32(Convert.ToDecimal(drp["BaggageChg_I"].ToString().Trim()));
                    }
                }

                //--------------------------CHILD-----------------------------------------------------
                dtSelectPax = dtPassenger.Select("TRIM(MealChg_O) > 0 AND TRIM(PaxType)='" + "CHD" + "'");
                if (dtSelectPax.Length > 0)
                {
                    foreach (DataRow drp in dtSelectPax.CopyToDataTable().Rows)
                    {
                        dSSR += decimal.ToInt32(Convert.ToDecimal(drp["MealChg_O"].ToString().Trim()));
                    }
                }
            }
            catch (Exception ex)
            {
                // dbCommon.Logger.dbLogg("", 0, "GET_TotalSSR", "LibraryAirlineBooking", "SSRdetail", "", ex.Message);
            }

            return dSSR;
        }
        public static string GetPassengerRequest(string PassengerData)
        {
            DataTable dtPassenger = Schema.SchemaPassengers;
            DataSet dsPassenger = CommonFunction.StringToDataSet(PassengerData);
            if (dsPassenger != null && dsPassenger.Tables.Count > 0)
            {
                int k = 1;
                foreach (DataRow dr in dsPassenger.Tables[0].Rows)
                {
                    //< FFNOAirlineOB />
                    //< FFNNumberOB />
                    //< FFNOAirlineIB />
                    //< FFNNumberIB />
                    //< Nationality />

                    DataRow drAdd = dtPassenger.NewRow();
                    drAdd["RowID"] = k;
                    drAdd["PaxType"] = dr["PaxType"].ToString().ToUpper();
                    drAdd["Title"] = dr["Title"];
                    drAdd["First_Name"] = dr["Fname"];
                    drAdd["Middle_Name"] = "";
                    drAdd["Last_Name"] = dr["Lname"];
                    drAdd["DOB"] = dr["DOB"].ToString().Trim().Length.Equals(0)
                        ? "1980-10-15"
                        : getFormattedDOB(dr["DOB"].ToString());

                    drAdd["MealDesc_O"] = SSRname(dr["SSROB_M"].ToString());
                    drAdd["MealCode_O"] = SSRcode(dr["SSROBValue_M"].ToString());

                    //drAdd["MealChg_O"] = SSRchg(dr["SSROB_M"].ToString());
                    drAdd["MealChg_O"] = SSRchg(dr["SSROBValue_M"].ToString());
                    drAdd["MealDetail_O"] = SSRdetail(dr["SSROBValue_M"].ToString());

                    if (dsPassenger.Tables[0].Columns.Contains("FFNNumberOB") && dr["FFNOAirlineOB"].ToString().Length.Equals(2) && dr["FFNNumberOB"].ToString().Length > 5)
                    {
                        drAdd["FFN"] = dr["FFNOAirlineOB"].ToString().ToUpper() + "-" + dr["FFNNumberOB"].ToString().ToUpper();
                    }
                    if (dsPassenger.Tables[0].Columns.Contains("FFNNumberIB") && dr["FFNOAirlineIB"].ToString().Length.Equals(2) && dr["FFNNumberIB"].ToString().Length > 5)
                    {
                        drAdd["FFN"] = drAdd["FFN"].ToString().Trim() + "#" + dr["FFNOAirlineIB"].ToString().ToUpper() + "-" + dr["FFNNumberIB"].ToString().ToUpper();
                    }

                    if (dsPassenger.Tables[0].Columns.Contains("SSRIB_M"))
                    {
                        drAdd["MealDesc_I"] = SSRname(dr["SSRIB_M"].ToString());
                        drAdd["MealCode_I"] = SSRcode(dr["SSRIBValue_M"].ToString());
                        //drAdd["MealChg_I"] = SSRchg(dr["SSRIB_M"].ToString());
                        drAdd["MealChg_I"] = SSRchg(dr["SSRIBValue_M"].ToString());
                        drAdd["MealDetail_I"] = SSRdetail(dr["SSRIBValue_M"].ToString());
                    }


                    drAdd["BaggageDesc_O"] = SSRname(dr["SSROB_B"].ToString());
                    drAdd["BaggageCode_O"] = SSRcode(dr["SSROBValue_B"].ToString());
                    //drAdd["BaggageChg_O"] = SSRchg(dr["SSROB_B"].ToString());
                    drAdd["BaggageChg_O"] = SSRchg(dr["SSROBValue_B"].ToString());
                    drAdd["BaggageDetail_O"] = SSRdetail(dr["SSROBValue_B"].ToString());

                    if (dsPassenger.Tables[0].Columns.Contains("SSRIB_B"))
                    {
                        drAdd["BaggageDesc_I"] = SSRname(dr["SSRIB_B"].ToString());
                        drAdd["BaggageCode_I"] = SSRcode(dr["SSRIBValue_B"].ToString());
                        //drAdd["BaggageChg_I"] = SSRchg(dr["SSRIB_B"].ToString());
                        drAdd["BaggageChg_I"] = SSRchg(dr["SSRIBValue_B"].ToString());
                        drAdd["BaggageDetail_I"] = SSRdetail(dr["SSRIBValue_B"].ToString());
                    }

                    drAdd["Email"] = dr["Email"];
                    drAdd["MobileNo"] = dr["MobileNo"];
                    //drAdd["City"] = "New Delhi";
                    //drAdd["State"] = "Delhi";
                    //drAdd["LandLine"] = Initializer.GetCompanyPhone();
                    //drAdd["Address"] = dr["Adderssss"];
                    drAdd["Nationality"] = dr["Nationality"].ToString().Trim().Length.Equals(0) ? "IN" : dr["Nationality"].ToString();

                    drAdd["PpNumber"] = dr["PPNo"];
                    drAdd["PPIssueDate"] = dr["PPExpiery"].ToString().Trim().Length.Equals(0)
                        ? ""
                        : getFormattedDOB(dr["PPExpiery"].ToString());
                    drAdd["PPExpirayDate"] = dr["PPExpiery"].ToString().Trim().Length.Equals(0)
                        ? ""
                        : getFormattedDOB(dr["PPExpiery"].ToString());


                    if (dr["gstreg_Number"].ToString().Length > 0)
                    {
                        GstDetails(dr["gstreg_CompanyName"].ToString(), dr["gstreg_Number"].ToString(), dr["gstreg_CompanyEmail"].ToString(), dr["gstreg_Companycontactno"].ToString(), dr["gstreg_compnyaddress"].ToString());
                    }

                   

                    dtPassenger.Rows.Add(drAdd);
                    k++;
                }
            }

            return CommonFunction.DataTableToString(dtPassenger, "PassengerInfo", "root");
        }

        private static void GstDetails(string GSTCompanyName, string GSTNumber, string GSTCompanyEmail, string GSTCompanyContactNumber, string GSTCompanyAddress)
        {
            DataTable dtGstInfo = Schema.SchemaGstInfo;
            DataRow drAdd = dtGstInfo.NewRow();
            drAdd["GSTCompanyName"] = GSTCompanyName;
            drAdd["GSTNumber"] = GSTNumber;
            drAdd["GSTCompanyEmail"] = GSTCompanyEmail;
            drAdd["GSTCompanyContactNumber"] = GSTCompanyContactNumber;
            drAdd["GSTCompanyAddress"] = GSTCompanyAddress;
            dtGstInfo.Rows.Add(drAdd);
            string GstInfo = CommonFunction.DataTableToString(dtGstInfo, "GstInfo", "root");
            HttpContextHelper.Current.Session.SetString("GstInfo", GstInfo);
        }

        private static string SSRcode(string ssr)// done
        {
            if (ssr.IndexOf("||") != -1)
            {
                string[] Split = ssr.Split(new string[] { "||" }, StringSplitOptions.None);
                return Split[1].ToString();
            }
            return "";
        }

        private static string SSRdetail(string ssr)// done
        {
            if (ssr.IndexOf("||") != -1)
            {
                string[] Split = ssr.Split(new string[] { "||" }, StringSplitOptions.None);
                return Split[2].ToString();
            }
            return "";
        }

        private static string SSRname(string ssr)// no change
        {
            if (ssr.IndexOf("--") != -1)
            {
                string[] Split = ssr.Split(new string[] { "--" }, StringSplitOptions.None);
                return Split[0].ToString();
            }
            return "";
        }

        private static string SSRchg(string ssr)
        {
            
            if (ssr.IndexOf("||") != -1)
            {
                string[] Split = ssr.Split(new string[] { "||" }, StringSplitOptions.None);
                return Split[0].ToString();
            }
            return "0";
        }
        private static string getFormattedDOB(string DOB)
        {
            if (DOB.Trim().Length.Equals(0))
            {
                return "1980-10-15";
            }
            else
            {
                string nDOB = "";
                string[] split = DOB.Split('/');
                for (int i = 0; i < split.Length; i++)
                {
                    nDOB += split[2].ToString();
                    nDOB += "-";
                    nDOB += split[1].ToString();
                    nDOB += "-";
                    nDOB += split[0].ToString();
                    break;
                }

                return nDOB;
            }
        }

        public static void GetCustomerDetail(out string Mobile, out string Address)
        {
            Mobile = string.Empty;
            Address = string.Empty;
            string passenger = HttpContextHelper.Current.Session.GetString("PaxXML").ToString();
            DataTable dtPassenger = CommonFunction.StringToDataSet(passenger).Tables[0];
            Mobile = dtPassenger.Rows[0]["MobileNo"].ToString();
            Address = dtPassenger.Rows[0]["Address"].ToString();
        }
    }
}
