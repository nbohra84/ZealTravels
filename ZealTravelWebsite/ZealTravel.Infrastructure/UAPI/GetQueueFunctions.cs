using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Xml;
using System.Xml.Linq;
using System.Collections;


namespace ZealTravel.Infrastructure.UAPI
{
    public class GetQueueFunctions
    {
        private string ActionStatus = "";
        private string AgencyInfo = "";
        private string AirReservation = "";
        private string BookingTraveler = "";
        private string ProviderReservationInfo = "";
        private string AgencyContactInfo = "";
        private string OSI = "";

        public DataTable dtFlights;
        public DataTable dtPassengers;
        public ArrayList arAirlinePnr;

        private string Email;
        private string Mobile;
        private string FareBasis;
        public void GetQueueFilter(string SearchType, string CompanyID, int FareRows, string Start, string End, string PNR)
        {
            dtPassengers = DBCommon.Schema.SchemaPassengers;
            dtFlights = DBCommon.Schema.SchemaFlights;

            int iActionStatus = 0;
            int iAgencyInfo = 0;
            int iAirReservation = 0;
            int iBookingTraveler = 0;
            int iProviderReservationInfo = 0;
            int iAgencyContactInfo = 0;
            int iOSI = 0;

            GetQueue objQ = new GetQueue();
            string QueueResponse = objQ.GetOwnPnr(PNR);
            if (QueueResponse.IndexOf("universal:UniversalRecord") != -1 && QueueResponse.IndexOf("AirReservation") != -1 && QueueResponse.IndexOf("AirPricingInfo") != -1)
            {
                XmlDocument xmlflt = new XmlDocument();
                xmlflt.LoadXml(QueueResponse);

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
                                if (s123.IndexOf("universal:UniversalRecord") != -1)
                                {
                                    XmlDocument xmlflt2 = new XmlDocument();
                                    xmlflt1.LoadXml(s123);
                                    XmlElement root2 = xmlflt1.DocumentElement;

                                    if (root2.HasChildNodes)
                                    {
                                        for (int k = 0; k < root2.ChildNodes.Count; k++)
                                        {
                                            string Nodes = root2.ChildNodes[k].OuterXml;
                                            if (Nodes.IndexOf("ActionStatus") != -1)
                                            {
                                                iActionStatus++;
                                                ActionStatus = Nodes;
                                            }
                                            else if (Nodes.IndexOf("AirReservation") != -1)
                                            {
                                                iAirReservation++;
                                                AirReservation = Nodes;
                                            }
                                            else if (Nodes.IndexOf("BookingTraveler") != -1)
                                            {
                                                iBookingTraveler++;
                                                BookingTraveler = Nodes;

                                                GetPassengerList(dtPassengers, Nodes, iBookingTraveler);
                                            }
                                            else if (Nodes.IndexOf("OSI") != -1)
                                            {
                                                iOSI++;
                                                OSI = Nodes;
                                            }
                                            else if (Nodes.IndexOf("AgencyInfo") != -1)
                                            {
                                                iAgencyInfo++;
                                                AgencyInfo = Nodes;
                                            }
                                            else if (Nodes.IndexOf("AgencyContactInfo") != -1 && Nodes.IndexOf("PhoneNumber") != -1)
                                            {
                                                iAgencyContactInfo++;
                                                AgencyContactInfo = Nodes;
                                            }
                                            else if (Nodes.IndexOf("ProviderReservationInfo") != -1)
                                            {
                                                iProviderReservationInfo++;
                                                ProviderReservationInfo = Nodes;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

                GetPassengerMobile1(CompanyID, dtPassengers);

                int Adt = 0;
                int Chd = 0;
                int Inf = 0;

                DataRow[] drPax = dtPassengers.Select("PaxType='" + "ADT" + "'");
                if (drPax.Length > 0)
                {
                    Adt = drPax.CopyToDataTable().Rows.Count;
                }
                drPax = dtPassengers.Select("PaxType='" + "CHD" + "'");
                if (drPax.Length > 0)
                {
                    Chd = drPax.CopyToDataTable().Rows.Count;
                }
                drPax = dtPassengers.Select("PaxType='" + "INF" + "'");
                if (drPax.Length > 0)
                {
                    Inf = drPax.CopyToDataTable().Rows.Count;
                }

                GetFlightsSingleFare(AirReservation, Adt, Chd, Inf);
                GetSector(SearchType, Start, End, dtFlights);
            }
        }
        private void GetFlightsSingleFare(string FlightsXml, int Adt, int Chd, int Inf)
        {
            DataSet dsResponse = new DataSet();
            dsResponse.ReadXml(new System.IO.StringReader(FlightsXml));

            string CabinBaggage = "0 KG";
            string Baggage = "0 KG";

            if (dsResponse != null)
            {
                if (dsResponse.Tables["SupplierLocator"] != null && dsResponse.Tables["SupplierLocator"].Rows.Count > 0)
                {
                    arAirlinePnr = new ArrayList();
                    foreach (DataRow dr in dsResponse.Tables["SupplierLocator"].Rows)
                    {
                        arAirlinePnr.Add(dr["SupplierLocatorCode"].ToString());
                    }
                }
                foreach (DataRow dr in dsResponse.Tables["AirSegment"].Rows)
                {
                    DataRow drAdd = dtFlights.NewRow();
                    if (dsResponse.Tables["MaxWeight"] != null && dsResponse.Tables["MaxWeight"].Rows.Count > 0)
                    {
                        Baggage = dsResponse.Tables["MaxWeight"].Rows[0]["Value"].ToString().Trim() + " " + dsResponse.Tables["MaxWeight"].Rows[0]["Unit"].ToString().Trim();
                    }

                    drAdd["BaggageDetail"] = Baggage + "*" + CabinBaggage;

                    drAdd["Adt"] = Adt;
                    drAdd["Chd"] = Chd;
                    drAdd["Inf"] = Inf;

                    drAdd["RowID"] = (Convert.ToInt32(dr["AirSegment_Id"].ToString().ToUpper().Trim()) + 1);
                    drAdd["RefID"] = 0;
                    drAdd["CarrierCode"] = dr["Carrier"].ToString().ToUpper().Trim();
                    drAdd["FlightNumber"] = dr["FlightNumber"].ToString().ToUpper().Trim();
                    drAdd["DepartureStation"] = dr["Origin"].ToString().ToUpper().Trim();
                    drAdd["ArrivalStation"] = dr["Destination"].ToString().ToUpper().Trim();
                    drAdd["Cabin"] = dr["CabinClass"].ToString().ToUpper().Trim();
                    drAdd["DepDate"] = dr["DepartureTime"].ToString().ToUpper().Trim();
                    drAdd["ArrDate"] = dr["ArrivalTime"].ToString().ToUpper().Trim();

                    if (dr["DepartureTime"].ToString().IndexOf("T") != -1)
                    {
                        string[] split = dr["DepartureTime"].ToString().Split('T');
                        drAdd["DepartureDate"] = split[0].ToString();

                        drAdd["DepTime"] = GetTime(split[1].ToString().Trim());
                    }
                    if (dr["ArrivalTime"].ToString().IndexOf("T") != -1)
                    {
                        string[] split = dr["ArrivalTime"].ToString().Split('T');
                        drAdd["ArrivalDate"] = split[0].ToString();

                        drAdd["ArrTime"] = GetTime(split[1].ToString().Trim());
                    }


                    drAdd["ClassOfService"] = dr["ClassOfService"].ToString().ToUpper().Trim();
                    drAdd["API_RefID"] = dr["TravelOrder"].ToString().ToUpper().Trim();

                    DataRow[] drFare = dsResponse.Tables["FareInfo"].Select("Origin='" + dr["Origin"].ToString().ToUpper().Trim() + "' And Destination='" + dr["Destination"].ToString().ToUpper().Trim() + "' And PassengerTypeCode='" + "ADT" + "'");
                    if (drFare.Length > 0)
                    {
                        string AirPricingInfo_Id = drFare.CopyToDataTable().Rows[0]["AirPricingInfo_Id"].ToString();

                        DataRow[] drFareResults = dsResponse.Tables["AirPricingInfo"].Select("AirPricingInfo_Id='" + AirPricingInfo_Id + "'");
                        if (drFareResults.Length > 0)
                        {
                            DataRow[] drFareBasisResults = dsResponse.Tables["FareInfo"].Select("AirPricingInfo_Id='" + AirPricingInfo_Id + "'");
                            if (drFareBasisResults.Length > 0)
                            {
                                FareBasis = drFareBasisResults.CopyToDataTable().Rows[0]["FareBasis"].ToString().ToUpper().Trim();
                            }

                            if (drFareResults.CopyToDataTable().Rows[0]["BasePrice"].ToString().IndexOf("INR") != -1)
                            {
                                drAdd["Adt_BASIC"] = drFareResults.CopyToDataTable().Rows[0]["BasePrice"].ToString().Replace("INR", "").Trim();
                            }
                            else if (drFareResults.CopyToDataTable().Rows[0]["ApproximateBasePrice"].ToString().IndexOf("INR") != -1)
                            {
                                drAdd["Adt_BASIC"] = drFareResults.CopyToDataTable().Rows[0]["ApproximateBasePrice"].ToString().Replace("INR", "").Trim();
                            }
                            drAdd["AdtTotalTax"] = drFareResults.CopyToDataTable().Rows[0]["Taxes"].ToString().Replace("INR", "").Trim();
                        }

                        DataRow[] drTaxResults = dsResponse.Tables["TaxInfo"].Select("AirPricingInfo_Id='" + AirPricingInfo_Id + "'");
                        if (drTaxResults.Length > 0)
                        {
                            int K3 = 0;
                            int YR = 0;
                            int YQ = 0;
                            int WO = 0;
                            int JN = 0;
                            int IN = 0;
                            int OT = 0;

                            foreach (DataRow drTax in drTaxResults.CopyToDataTable().Rows)
                            {
                                if (drTax["Category"].ToString().Trim().Equals("K3"))
                                {
                                    K3 += Convert.ToInt32(drTax["Amount"].ToString().Trim().Replace("INR", "").Trim());
                                }
                                else if (drTax["Category"].ToString().Trim().Equals("YR"))
                                {
                                    YR += Convert.ToInt32(drTax["Amount"].ToString().Trim().Replace("INR", "").Trim());
                                }
                                else if (drTax["Category"].ToString().Trim().Equals("YQ"))
                                {
                                    YQ += Convert.ToInt32(drTax["Amount"].ToString().Trim().Replace("INR", "").Trim());
                                }
                                else if (drTax["Category"].ToString().Trim().Equals("WO"))
                                {
                                    WO += Convert.ToInt32(drTax["Amount"].ToString().Trim().Replace("INR", "").Trim());
                                }
                                else if (drTax["Category"].ToString().Trim().Equals("JN"))
                                {
                                    JN += Convert.ToInt32(drTax["Amount"].ToString().Trim().Replace("INR", "").Trim());
                                }
                                else if (drTax["Category"].ToString().Trim().Equals("IN") || drTax["Category"].ToString().Trim().Equals("YM"))
                                {
                                    IN += Convert.ToInt32(drTax["Amount"].ToString().Trim().Replace("INR", "").Trim());
                                }
                                else
                                {
                                    OT += Convert.ToInt32(drTax["Amount"].ToString().Trim().Replace("INR", "").Trim());
                                }
                            }

                            drAdd["Adt_YQ"] = YQ;
                            drAdd["Adt_PSF"] = WO;
                            drAdd["Adt_UDF"] = IN;
                            drAdd["Adt_AUDF"] = 0;
                            drAdd["Adt_CUTE"] = YR;
                            drAdd["Adt_GST"] = K3;
                            drAdd["Adt_TF"] = JN;
                            drAdd["Adt_CESS"] = 0;
                            drAdd["Adt_EX"] = OT;
                        }
                    }


                    drFare = dsResponse.Tables["FareInfo"].Select("Origin='" + dr["Origin"].ToString().ToUpper().Trim() + "' And Destination='" + dr["Destination"].ToString().ToUpper().Trim() + "' And PassengerTypeCode='" + "CNN" + "'");
                    if (drFare.Length > 0)
                    {
                        string AirPricingInfo_Id = drFare.CopyToDataTable().Rows[0]["AirPricingInfo_Id"].ToString();

                        DataRow[] drFareResults = dsResponse.Tables["AirPricingInfo"].Select("AirPricingInfo_Id='" + AirPricingInfo_Id + "'");
                        if (drFareResults.Length > 0)
                        {
                            DataRow[] drFareBasisResults = dsResponse.Tables["FareInfo"].Select("AirPricingInfo_Id='" + AirPricingInfo_Id + "'");
                            if (drFareBasisResults.Length > 0)
                            {
                                FareBasis += "|";
                                FareBasis += drFareBasisResults.CopyToDataTable().Rows[0]["FareBasis"].ToString().ToUpper().Trim();
                            }


                            if (drFareResults.CopyToDataTable().Rows[0]["BasePrice"].ToString().IndexOf("INR") != -1)
                            {
                                drAdd["Chd_BASIC"] = drFareResults.CopyToDataTable().Rows[0]["BasePrice"].ToString().Replace("INR", "").Trim();
                            }
                            else if (drFareResults.CopyToDataTable().Rows[0]["ApproximateBasePrice"].ToString().IndexOf("INR") != -1)
                            {
                                drAdd["Chd_BASIC"] = drFareResults.CopyToDataTable().Rows[0]["ApproximateBasePrice"].ToString().Replace("INR", "").Trim();
                            }

                            drAdd["ChdTotalTax"] = drFareResults.CopyToDataTable().Rows[0]["Taxes"].ToString().Replace("INR", "").Trim();
                        }

                        DataRow[] drTaxResults = dsResponse.Tables["TaxInfo"].Select("AirPricingInfo_Id='" + AirPricingInfo_Id + "'");
                        if (drTaxResults.Length > 0)
                        {
                            int K3 = 0;
                            int YR = 0;
                            int YQ = 0;
                            int WO = 0;
                            int JN = 0;
                            int IN = 0;
                            int OT = 0;

                            foreach (DataRow drTax in drTaxResults.CopyToDataTable().Rows)
                            {
                                if (drTax["Category"].ToString().Trim().Equals("K3"))
                                {
                                    K3 += Convert.ToInt32(drTax["Amount"].ToString().Trim().Replace("INR", "").Trim());
                                }
                                else if (drTax["Category"].ToString().Trim().Equals("YR"))
                                {
                                    YR += Convert.ToInt32(drTax["Amount"].ToString().Trim().Replace("INR", "").Trim());
                                }
                                else if (drTax["Category"].ToString().Trim().Equals("YQ"))
                                {
                                    YQ += Convert.ToInt32(drTax["Amount"].ToString().Trim().Replace("INR", "").Trim());
                                }
                                else if (drTax["Category"].ToString().Trim().Equals("WO"))
                                {
                                    WO += Convert.ToInt32(drTax["Amount"].ToString().Trim().Replace("INR", "").Trim());
                                }
                                else if (drTax["Category"].ToString().Trim().Equals("JN"))
                                {
                                    JN += Convert.ToInt32(drTax["Amount"].ToString().Trim().Replace("INR", "").Trim());
                                }
                                else if (drTax["Category"].ToString().Trim().Equals("IN") || drTax["Category"].ToString().Trim().Equals("YM"))
                                {
                                    IN += Convert.ToInt32(drTax["Amount"].ToString().Trim().Replace("INR", "").Trim());
                                }
                                else
                                {
                                    OT += Convert.ToInt32(drTax["Amount"].ToString().Trim().Replace("INR", "").Trim());
                                }
                            }

                            drAdd["Chd_YQ"] = YQ;
                            drAdd["Chd_PSF"] = WO;
                            drAdd["Chd_UDF"] = IN;
                            drAdd["Chd_AUDF"] = 0;
                            drAdd["Chd_CUTE"] = YR;
                            drAdd["Chd_GST"] = K3;
                            drAdd["Chd_TF"] = JN;
                            drAdd["Chd_CESS"] = 0;
                            drAdd["Chd_EX"] = OT;
                        }
                    }

                    drFare = dsResponse.Tables["FareInfo"].Select("Origin='" + dr["Origin"].ToString().ToUpper().Trim() + "' And Destination='" + dr["Destination"].ToString().ToUpper().Trim() + "' And PassengerTypeCode='" + "INF" + "'");
                    if (drFare.Length > 0)
                    {
                        string AirPricingInfo_Id = drFare.CopyToDataTable().Rows[0]["AirPricingInfo_Id"].ToString();
                        DataRow[] drFareResults = dsResponse.Tables["AirPricingInfo"].Select("AirPricingInfo_Id='" + AirPricingInfo_Id + "'");
                        if (drFareResults.Length > 0)
                        {
                            DataRow[] drFareBasisResults = dsResponse.Tables["FareInfo"].Select("AirPricingInfo_Id='" + AirPricingInfo_Id + "'");
                            if (drFareBasisResults.Length > 0)
                            {
                                FareBasis += "|";
                                FareBasis += drFareBasisResults.CopyToDataTable().Rows[0]["FareBasis"].ToString().ToUpper().Trim();
                            }

                            if (drFareResults.CopyToDataTable().Rows[0]["BasePrice"].ToString().IndexOf("INR") != -1)
                            {
                                drAdd["Inf_BASIC"] = drFareResults.CopyToDataTable().Rows[0]["BasePrice"].ToString().Replace("INR", "").Trim();
                            }
                            else if (drFareResults.CopyToDataTable().Rows[0]["ApproximateBasePrice"].ToString().IndexOf("INR") != -1)
                            {
                                drAdd["Inf_BASIC"] = drFareResults.CopyToDataTable().Rows[0]["ApproximateBasePrice"].ToString().Replace("INR", "").Trim();
                            }

                            drAdd["InfTotalTax"] = drFareResults.CopyToDataTable().Rows[0]["Taxes"].ToString().Replace("INR", "").Trim();
                        }

                        DataRow[] drTaxResults = dsResponse.Tables["TaxInfo"].Select("AirPricingInfo_Id='" + AirPricingInfo_Id + "'");
                        if (drTaxResults.Length > 0)
                        {
                            int OT = 0;
                            foreach (DataRow drTax in drTaxResults.CopyToDataTable().Rows)
                            {
                                OT += Convert.ToInt32(drTax["Amount"].ToString().Trim().Replace("INR", "").Trim());
                            }
                            drAdd["Inf_TAX"] = OT;
                        }
                    }


                    drAdd["FareBasisCode"] = FareBasis;

                    DataRow[] drResult = dsResponse.Tables["FlightDetails"].Select("AirSegment_Id='" + dr["AirSegment_Id"].ToString().Trim() + "'");
                    if (drResult.Length > 0)
                    {
                        DataRow drr = drResult.CopyToDataTable().Rows[0];
                        int FlightTime = 0;
                        if (drr.Table.Columns.Contains("FlightTime"))
                        {

                            int.TryParse(drr["FlightTime"].ToString().ToUpper().Trim(), out FlightTime);
                        }
                        drAdd["Duration"] = FlightTime;

                        int TravelTime = 0;
                        if (drr.Table.Columns.Contains("TravelTime"))
                        {
                            int.TryParse(drr["TravelTime"].ToString().ToUpper().Trim(), out TravelTime);
                        }
                        drAdd["JourneyTime"] = TravelTime;

                        if (drr.Table.Columns.Contains("Equipment"))
                        {
                            drAdd["EquipmentType"] = drr["Equipment"].ToString().ToUpper().Trim();
                        }

                        if (drResult.CopyToDataTable().Columns.Contains("OriginTerminal"))
                        {
                            drAdd["DepartureTerminal"] = drr["OriginTerminal"].ToString().ToUpper().Trim();
                        }

                        if (drResult.CopyToDataTable().Columns.Contains("DestinationTerminal"))
                        {
                            drAdd["ArrivalTerminal"] = drr["DestinationTerminal"].ToString().ToUpper().Trim();
                        }
                    }
                    dtFlights.Rows.Add(drAdd);
                }
            }
        }
        private void GetPassengerList(DataTable dtPassenger, string PassengerXml, int iRow)
        {
            DataSet dsResponse = new DataSet();
            dsResponse.ReadXml(new System.IO.StringReader(PassengerXml));

            if (dsResponse != null && dsResponse.Tables["BookingTravelerName"] != null)
            {
                if (dsResponse.Tables["Email"] != null)
                {
                    Email = dsResponse.Tables["Email"].Rows[0]["EmailID"].ToString();
                }

                foreach (DataRow dr in dsResponse.Tables["BookingTravelerName"].Rows)
                {
                    DataRow drAdd = dtPassenger.NewRow();
                    drAdd["RowID"] = iRow;

                    string fname = dr["First"].ToString().ToUpper().Trim();
                    //if (fname.IndexOf("MR") != -1)
                    //{
                    //    fname = fname.Replace("MR", "").Trim();
                    //}
                    //else if (fname.IndexOf("MS") != -1)
                    //{
                    //    fname = fname.Replace("MS", "").Trim();
                    //}
                    //else if (fname.IndexOf("MRS") != -1)
                    //{
                    //    fname = fname.Replace("MRS", "").Trim();
                    //}
                    //else if (fname.IndexOf("MISS") != -1)
                    //{
                    //    fname = fname.Replace("MISS", "").Trim();
                    //}
                    //else if (fname.IndexOf("MSTR") != -1)
                    //{
                    //    fname = fname.Replace("MSTR", "").Trim();
                    //}
                    //else if (fname.IndexOf("INF") != -1)
                    //{
                    //    fname = fname.Replace("INF", "").Trim();
                    //}

                    drAdd["First_Name"] = fname;
                    drAdd["Last_Name"] = dr["Last"].ToString().ToUpper().Trim();

                    drAdd["PaxType"] = "ADT";
                    drAdd["DOB"] = "1980-01-01";

                    if (dsResponse.Tables["NameRemark"] != null)
                    {
                        if (dsResponse.Tables["NameRemark"].Rows[0]["RemarkData"].ToString().IndexOf("P-C") != -1)
                        {
                            drAdd["PaxType"] = "CHD";
                            drAdd["DOB"] = GetDOB(dsResponse.Tables["NameRemark"].Rows[0]["RemarkData"].ToString());
                        }
                        else
                        {
                            if (dsResponse.Tables["BookingTraveler"] != null)
                            {
                                if (dsResponse.Tables["BookingTraveler"].Columns.Contains("TravelerType"))
                                {
                                    drAdd["PaxType"] = dsResponse.Tables["BookingTraveler"].Rows[0]["TravelerType"].ToString();
                                    drAdd["DOB"] = dsResponse.Tables["BookingTraveler"].Rows[0]["DOB"].ToString();
                                }
                            }
                        }
                    }

                    if (drAdd["PaxType"].ToString().Equals("ADT"))
                    {
                        if (dr["First"].ToString().ToUpper().IndexOf("MRS") != -1)
                        {
                            drAdd["Title"] = "MRS";
                        }
                        else if (dr["First"].ToString().ToUpper().IndexOf("MR") != -1)
                        {
                            drAdd["Title"] = "MR";
                        }
                        else if (dr["First"].ToString().ToUpper().IndexOf("MS") != -1)
                        {
                            drAdd["Title"] = "MS";
                        }
                    }
                    else if (drAdd["PaxType"].ToString().Equals("CHD"))
                    {
                        if (dr["First"].ToString().ToUpper().IndexOf("MISS") != -1)
                        {
                            drAdd["Title"] = "MISS";
                        }
                        else if (dr["First"].ToString().ToUpper().IndexOf("MSTR") != -1)
                        {
                            drAdd["Title"] = "MSTR";
                        }
                    }
                    else if (drAdd["PaxType"].ToString().Equals("INF"))
                    {
                        if (dr["First"].ToString().ToUpper().IndexOf("INF") != -1)
                        {
                            drAdd["Title"] = "INF";
                        }
                        else if (dr["First"].ToString().ToUpper().IndexOf("MISS") != -1)
                        {
                            drAdd["Title"] = "MISS";
                        }
                        else if (dr["First"].ToString().ToUpper().IndexOf("MSTR") != -1)
                        {
                            drAdd["Title"] = "MSTR";
                        }
                    }

                    string FFN = "";
                    if (dsResponse.Tables["LoyaltyCard"] != null)
                    {
                        FFN = dsResponse.Tables["LoyaltyCard"].Rows[0]["SupplierCode"].ToString() + "-";
                        FFN += dsResponse.Tables["LoyaltyCard"].Rows[0]["CardNumber"].ToString();
                    }


                    drAdd["FFN"] = FFN;
                    drAdd["TourCode"] = "";

                    drAdd["Email"] = Email;
                    drAdd["MobileNo"] = Mobile;
                    drAdd["City"] = "";
                    drAdd["State"] = "";
                    drAdd["LandLine"] = "";
                    drAdd["Address"] = "";
                    drAdd["Nationality"] = "IN";
                    dtPassenger.Rows.Add(drAdd);
                }
            }
        }
        private void GetPassengerMobile(DataTable dtPassenger, string OSI)
        {
            DataSet dsOSIResponse = new DataSet();
            dsOSIResponse.ReadXml(new System.IO.StringReader(OSI));
            foreach (DataRow dr in dsOSIResponse.Tables["OSI"].Rows)
            {
                string text = dr["Text"].ToString();
                text = text.Replace("PAX", "").Trim();
                text = text.Replace("CTC", "").Trim();
                Mobile = text;
            }

            if (Mobile.Length > 0)
            {
                foreach (DataRow dr in dtPassenger.Rows)
                {
                    dr["MobileNo"] = Mobile;
                    dr["Email"] = Email;
                }
                dtPassenger.AcceptChanges();
            }
            else
            {
                foreach (DataRow dr in dtPassenger.Rows)
                {
                    dr["MobileNo"] = "8882233324";
                    dr["Email"] = "ticketing@tourista.asia";
                }
                dtPassenger.AcceptChanges();
            }
        }
        private void GetPassengerMobile1(string CompanyID, DataTable dtPassenger)
        {
            DBCommon.db_Company cs = new DBCommon.db_Company();

            DataTable dtCompany = cs.GetCompanyDetailbyCompanyID(CompanyID);
            if (dtCompany != null && dtCompany.Rows.Count > 0)
            {
                foreach (DataRow dr in dtPassenger.Rows)
                {
                    dr["MobileNo"] = dtCompany.Rows[0]["Mobile"].ToString();
                    if (dr["Email"].ToString().Trim().Length.Equals(0))
                    {
                        dr["Email"] = dtCompany.Rows[0]["Email"].ToString();
                    }
                }
            }
            dtPassenger.AcceptChanges();
        }
        private string GetTime(string Tm)
        {
            string[] split = Tm.Split('+');
            return split[0].ToString().Trim().Substring(0, 5).Trim();
        }
        private string GetDOB(string Age)
        {
            int year = 0;
            Age = Age.Replace("P-C", "").Trim();
            int.TryParse(Age, out year);

            year = DateTime.Now.Year - year;

            DateTime Date = new DateTime(year, 1, 1);
            return Date.ToString("yyyy-MM-dd");
        }
        private void GetSector(string SearchType, string Start, string End, DataTable dtFlights)
        {
            if (SearchType.Equals("OW") || SearchType.Equals("MC"))
            {
                foreach (DataRow dr in dtFlights.Rows)
                {
                    dr["Origin"] = Start;
                    dr["Destination"] = End;
                    dr["FltType"] = "O";
                }
                dtFlights.AcceptChanges();
            }
            else
            {
                if (dtFlights.Rows.Count.Equals(2))
                {
                    for (int i = 0; i < dtFlights.Rows.Count; i++)
                    {
                        DataRow dr = dtFlights.Rows[i];
                        if (i.Equals(0))
                        {
                            dr["Origin"] = Start;
                            dr["Destination"] = End;
                            dr["FltType"] = "O";
                        }
                        else
                        {
                            dr["Origin"] = End;
                            dr["Destination"] = Start;
                            dr["FltType"] = "I";
                        }
                    }
                    dtFlights.AcceptChanges();
                }
                if (dtFlights.Rows.Count.Equals(3))
                {
                    DataRow dr1 = dtFlights.Rows[0];
                    DataRow dr2 = dtFlights.Rows[1];
                    DataRow dr3 = dtFlights.Rows[2];

                    if (dr1["DepartureStation"].ToString().Equals(Start) && dr1["ArrivalStation"].ToString().Equals(End))
                    {
                        dr1["Origin"] = Start;
                        dr1["Destination"] = End;
                        dr1["FltType"] = "O";
                        dr1.AcceptChanges();

                        dr2["Origin"] = End;
                        dr2["Destination"] = Start;
                        dr2["FltType"] = "I";
                        dr2.AcceptChanges();

                        dr3["Origin"] = End;
                        dr3["Destination"] = Start;
                        dr3["FltType"] = "I";
                        dr3.AcceptChanges();
                    }
                    else if (dr1["DepartureStation"].ToString().Equals(Start) && dr2["ArrivalStation"].ToString().Equals(End))
                    {
                        dr1["Origin"] = Start;
                        dr1["Destination"] = End;
                        dr1["FltType"] = "O";
                        dr1.AcceptChanges();

                        dr2["Origin"] = Start;
                        dr2["Destination"] = End;
                        dr2["FltType"] = "O";
                        dr2.AcceptChanges();

                        dr3["Origin"] = End;
                        dr3["Destination"] = Start;
                        dr3["FltType"] = "I";
                        dr3.AcceptChanges();
                    }
                    dtFlights.AcceptChanges();
                }
                if (dtFlights.Rows.Count.Equals(4))
                {
                    DataRow dr1 = dtFlights.Rows[0];
                    DataRow dr2 = dtFlights.Rows[1];
                    DataRow dr3 = dtFlights.Rows[2];
                    DataRow dr4 = dtFlights.Rows[3];

                    if (dr1["DepartureStation"].ToString().Equals(Start) && dr1["ArrivalStation"].ToString().Equals(End))
                    {
                        dr1["Origin"] = Start;
                        dr1["Destination"] = End;
                        dr1["FltType"] = "O";
                        dr1.AcceptChanges();

                        dr2["Origin"] = End;
                        dr2["Destination"] = Start;
                        dr2["FltType"] = "I";
                        dr2.AcceptChanges();

                        dr3["Origin"] = End;
                        dr3["Destination"] = Start;
                        dr3["FltType"] = "I";
                        dr3.AcceptChanges();

                        dr4["Origin"] = End;
                        dr4["Destination"] = Start;
                        dr4["FltType"] = "I";
                        dr4.AcceptChanges();
                    }
                    else if (dr1["DepartureStation"].ToString().Equals(Start) && dr2["ArrivalStation"].ToString().Equals(End))
                    {
                        dr1["Origin"] = Start;
                        dr1["Destination"] = End;
                        dr1["FltType"] = "O";
                        dr1.AcceptChanges();

                        dr2["Origin"] = Start;
                        dr2["Destination"] = End;
                        dr2["FltType"] = "O";
                        dr2.AcceptChanges();

                        dr3["Origin"] = End;
                        dr3["Destination"] = Start;
                        dr3["FltType"] = "I";
                        dr3.AcceptChanges();

                        dr4["Origin"] = End;
                        dr4["Destination"] = Start;
                        dr4["FltType"] = "I";
                        dr4.AcceptChanges();
                    }
                    else if (dr1["DepartureStation"].ToString().Equals(Start) && dr3["ArrivalStation"].ToString().Equals(End))
                    {
                        dr1["Origin"] = Start;
                        dr1["Destination"] = End;
                        dr1["FltType"] = "O";
                        dr1.AcceptChanges();

                        dr2["Origin"] = Start;
                        dr2["Destination"] = End;
                        dr2["FltType"] = "O";
                        dr2.AcceptChanges();

                        dr3["Origin"] = Start;
                        dr3["Destination"] = End;
                        dr3["FltType"] = "O";
                        dr3.AcceptChanges();

                        dr4["Origin"] = End;
                        dr4["Destination"] = Start;
                        dr4["FltType"] = "I";
                        dr4.AcceptChanges();
                    }
                    dtFlights.AcceptChanges();
                }
                if (dtFlights.Rows.Count.Equals(5))
                {
                    DataRow dr1 = dtFlights.Rows[0];
                    DataRow dr2 = dtFlights.Rows[1];
                    DataRow dr3 = dtFlights.Rows[2];
                    DataRow dr4 = dtFlights.Rows[3];
                    DataRow dr5 = dtFlights.Rows[4];

                    if (dr1["DepartureStation"].ToString().Equals(Start) && dr2["ArrivalStation"].ToString().Equals(End))
                    {
                        dr1["Origin"] = Start;
                        dr1["Destination"] = End;
                        dr1["FltType"] = "O";
                        dr1.AcceptChanges();

                        dr2["Origin"] = Start;
                        dr2["Destination"] = End;
                        dr2["FltType"] = "O";
                        dr2.AcceptChanges();

                        dr3["Origin"] = End;
                        dr3["Destination"] = Start;
                        dr3["FltType"] = "I";
                        dr3.AcceptChanges();

                        dr4["Origin"] = End;
                        dr4["Destination"] = Start;
                        dr4["FltType"] = "I";
                        dr4.AcceptChanges();

                        dr5["Origin"] = End;
                        dr5["Destination"] = Start;
                        dr5["FltType"] = "I";
                        dr5.AcceptChanges();
                    }
                    else if (dr1["DepartureStation"].ToString().Equals(Start) && dr3["ArrivalStation"].ToString().Equals(End))
                    {
                        dr1["Origin"] = Start;
                        dr1["Destination"] = End;
                        dr1["FltType"] = "O";
                        dr1.AcceptChanges();

                        dr2["Origin"] = Start;
                        dr2["Destination"] = End;
                        dr2["FltType"] = "O";
                        dr2.AcceptChanges();

                        dr3["Origin"] = Start;
                        dr3["Destination"] = End;
                        dr3["FltType"] = "O";
                        dr3.AcceptChanges();

                        dr4["Origin"] = End;
                        dr4["Destination"] = Start;
                        dr4["FltType"] = "I";
                        dr4.AcceptChanges();

                        dr5["Origin"] = End;
                        dr5["Destination"] = Start;
                        dr5["FltType"] = "I";
                        dr5.AcceptChanges();
                    }
                    dtFlights.AcceptChanges();
                }
                if (dtFlights.Rows.Count.Equals(6))
                {
                    DataRow dr1 = dtFlights.Rows[0];
                    DataRow dr2 = dtFlights.Rows[1];
                    DataRow dr3 = dtFlights.Rows[2];
                    DataRow dr4 = dtFlights.Rows[3];
                    DataRow dr5 = dtFlights.Rows[4];
                    DataRow dr6 = dtFlights.Rows[5];

                    if (dr1["DepartureStation"].ToString().Equals(Start) && dr3["ArrivalStation"].ToString().Equals(End))
                    {
                        dr1["Origin"] = Start;
                        dr1["Destination"] = End;
                        dr1["FltType"] = "O";
                        dr1.AcceptChanges();

                        dr2["Origin"] = Start;
                        dr2["Destination"] = End;
                        dr2["FltType"] = "O";
                        dr2.AcceptChanges();

                        dr3["Origin"] = Start;
                        dr3["Destination"] = End;
                        dr3["FltType"] = "O";
                        dr3.AcceptChanges();

                        dr4["Origin"] = End;
                        dr4["Destination"] = Start;
                        dr4["FltType"] = "I";
                        dr4.AcceptChanges();

                        dr5["Origin"] = End;
                        dr5["Destination"] = Start;
                        dr5["FltType"] = "I";
                        dr5.AcceptChanges();

                        dr6["Origin"] = End;
                        dr6["Destination"] = Start;
                        dr6["FltType"] = "I";
                        dr6.AcceptChanges();

                        dtFlights.AcceptChanges();
                    }
                }
            }
        }

        //private void GetFlightsDoubleFare(string FlightsXml)
        //{
        //    string CabinBaggage = "0 KG";
        //    string Baggage = "0 KG";
        //    DataSet dsResponse = new DataSet();
        //    dsResponse.ReadXml(new System.IO.StringReader(FlightsXml));
        //    if (dsResponse != null)
        //    {
        //        if (dsResponse.Tables["SupplierLocator"] != null && dsResponse.Tables["SupplierLocator"].Rows.Count > 0)
        //        {
        //            AirlinePNR = dsResponse.Tables["SupplierLocator"].Rows[0]["SupplierLocatorCode"].ToString();
        //        }
        //        foreach (DataRow dr in dsResponse.Tables["AirSegment"].Rows)
        //        {
        //            DataRow drAdd = dtFlights.NewRow();
        //            if (dsResponse.Tables["MaxWeight"] != null && dsResponse.Tables["MaxWeight"].Rows.Count > 0)
        //            {
        //                Baggage = dsResponse.Tables["MaxWeight"].Rows[0]["Value"].ToString().Trim() + " " + dsResponse.Tables["MaxWeight"].Rows[0]["Unit"].ToString().Trim();
        //            }

        //            drAdd["BaggageDetail"] = Baggage + "*" + CabinBaggage;

        //            drAdd["RowID"] = (Convert.ToInt32(dr["AirSegment_Id"].ToString().ToUpper().Trim()) + 1);
        //            drAdd["RefID"] = 0;
        //            drAdd["CarrierCode"] = dr["Carrier"].ToString().ToUpper().Trim();
        //            drAdd["FlightNumber"] = dr["FlightNumber"].ToString().ToUpper().Trim();
        //            drAdd["DepartureStation"] = dr["Origin"].ToString().ToUpper().Trim();
        //            drAdd["ArrivalStation"] = dr["Destination"].ToString().ToUpper().Trim();
        //            drAdd["Cabin"] = dr["CabinClass"].ToString().ToUpper().Trim();
        //            drAdd["DepDate"] = dr["DepartureTime"].ToString().ToUpper().Trim();
        //            drAdd["ArrDate"] = dr["ArrivalTime"].ToString().ToUpper().Trim();

        //            if (dr["DepartureTime"].ToString().IndexOf("T") != -1)
        //            {
        //                string[] split = dr["DepartureTime"].ToString().Split('T');
        //                drAdd["DepartureDate"] = split[0].ToString();

        //                drAdd["DepTime"] = GetTime(split[1].ToString().Trim());
        //            }
        //            if (dr["ArrivalTime"].ToString().IndexOf("T") != -1)
        //            {
        //                string[] split = dr["ArrivalTime"].ToString().Split('T');
        //                drAdd["ArrivalDate"] = split[0].ToString();

        //                drAdd["ArrTime"] = GetTime(split[1].ToString().Trim());
        //            }


        //            drAdd["ClassOfService"] = dr["ClassOfService"].ToString().ToUpper().Trim();
        //            drAdd["API_RefID"] = dr["TravelOrder"].ToString().ToUpper().Trim();

        //            GetPassengerFare(drAdd, dsResponse);

        //            drAdd["FareBasisCode"] = FareBasis;

        //            DataRow[] drResult = dsResponse.Tables["FlightDetails"].Select("AirSegment_Id='" + dr["AirSegment_Id"].ToString().Trim() + "'");
        //            if (drResult.Length > 0)
        //            {
        //                DataRow drr = drResult.CopyToDataTable().Rows[0];

        //                int FlightTime = 0;
        //                int.TryParse(drr["FlightTime"].ToString().ToUpper().Trim(), out FlightTime);
        //                drAdd["Duration"] = FlightTime;

        //                int TravelTime = 0;
        //                int.TryParse(drr["TravelTime"].ToString().ToUpper().Trim(), out TravelTime);
        //                drAdd["JourneyTime"] = TravelTime;

        //                drAdd["EquipmentType"] = drr["Equipment"].ToString().ToUpper().Trim();
        //                drAdd["DepartureTerminal"] = drr["OriginTerminal"].ToString().ToUpper().Trim();
        //                drAdd["ArrivalTerminal"] = drr["DestinationTerminal"].ToString().ToUpper().Trim();
        //            }
        //            dtFlights.Rows.Add(drAdd);
        //        }
        //    }
        //}
        //private void GetPassengerFare(DataRow drAdd, DataSet dsResponse)
        //{
        //    FareBasis = "";
        //    if (dsResponse.Tables["PassengerType"] != null)
        //    {
        //        DataRow[] drPassenger = dsResponse.Tables["PassengerType"].Select("Code='" + "ADT" + "'");
        //        drAdd["Adt"] = drPassenger.CopyToDataTable().Rows.Count;
        //        string AirPricingInfo_Id = drPassenger.CopyToDataTable().Rows[0]["AirPricingInfo_Id"].ToString();

        //        DataRow[] drFareResults = dsResponse.Tables["AirPricingInfo"].Select("AirPricingInfo_Id='" + AirPricingInfo_Id + "'");
        //        if (drFareResults.Length > 0)
        //        {
        //            DataRow[] drFareBasisResults = dsResponse.Tables["FareInfo"].Select("AirPricingInfo_Id='" + AirPricingInfo_Id + "'");
        //            if (drFareBasisResults.Length > 0)
        //            {
        //                FareBasis = drFareBasisResults.CopyToDataTable().Rows[0]["FareBasis"].ToString().ToUpper().Trim();
        //            }

        //            if (drFareResults.CopyToDataTable().Rows[0]["BasePrice"].ToString().IndexOf("INR") != -1)
        //            {
        //                drAdd["Adt_BASIC"] = drFareResults.CopyToDataTable().Rows[0]["BasePrice"].ToString().Replace("INR", "").Trim();
        //            }
        //            else if (drFareResults.CopyToDataTable().Rows[0]["ApproximateBasePrice"].ToString().IndexOf("INR") != -1)
        //            {
        //                drAdd["Adt_BASIC"] = drFareResults.CopyToDataTable().Rows[0]["ApproximateBasePrice"].ToString().Replace("INR", "").Trim();
        //            }
        //            drAdd["AdtTotalTax"] = drFareResults.CopyToDataTable().Rows[0]["Taxes"].ToString().Replace("INR", "").Trim();
        //        }

        //        DataRow[] drTaxResults = dsResponse.Tables["TaxInfo"].Select("AirPricingInfo_Id='" + AirPricingInfo_Id + "'");
        //        if (drTaxResults.Length > 0)
        //        {
        //            int K3 = 0;
        //            int YR = 0;
        //            int YQ = 0;
        //            int WO = 0;
        //            int JN = 0;
        //            int IN = 0;
        //            int OT = 0;

        //            foreach (DataRow drTax in drTaxResults.CopyToDataTable().Rows)
        //            {
        //                if (drTax["Category"].ToString().Trim().Equals("K3"))
        //                {
        //                    K3 += Convert.ToInt32(drTax["Amount"].ToString().Trim().Replace("INR", "").Trim());
        //                }
        //                else if (drTax["Category"].ToString().Trim().Equals("YR"))
        //                {
        //                    YR += Convert.ToInt32(drTax["Amount"].ToString().Trim().Replace("INR", "").Trim());
        //                }
        //                else if (drTax["Category"].ToString().Trim().Equals("YQ"))
        //                {
        //                    YQ += Convert.ToInt32(drTax["Amount"].ToString().Trim().Replace("INR", "").Trim());
        //                }
        //                else if (drTax["Category"].ToString().Trim().Equals("WO"))
        //                {
        //                    WO += Convert.ToInt32(drTax["Amount"].ToString().Trim().Replace("INR", "").Trim());
        //                }
        //                else if (drTax["Category"].ToString().Trim().Equals("JN"))
        //                {
        //                    JN += Convert.ToInt32(drTax["Amount"].ToString().Trim().Replace("INR", "").Trim());
        //                }
        //                else if (drTax["Category"].ToString().Trim().Equals("IN") || drTax["Category"].ToString().Trim().Equals("YM"))
        //                {
        //                    IN += Convert.ToInt32(drTax["Amount"].ToString().Trim().Replace("INR", "").Trim());
        //                }
        //                else
        //                {
        //                    OT += Convert.ToInt32(drTax["Amount"].ToString().Trim().Replace("INR", "").Trim());
        //                }
        //            }

        //            drAdd["Adt_YQ"] = YQ;
        //            drAdd["Adt_PSF"] = WO;
        //            drAdd["Adt_UDF"] = IN;
        //            drAdd["Adt_AUDF"] = 0;
        //            drAdd["Adt_CUTE"] = YR;
        //            drAdd["Adt_GST"] = K3;
        //            drAdd["Adt_TF"] = JN;
        //            drAdd["Adt_CESS"] = 0;
        //            drAdd["Adt_EX"] = OT;
        //        }

        //        //==============================================================================================
        //        drPassenger = dsResponse.Tables["PassengerType"].Select("Code='" + "CNN" + "'");
        //        if (drPassenger.Length > 0 && drPassenger.CopyToDataTable().Rows.Count > 0)
        //        {
        //            drAdd["Chd"] = drPassenger.CopyToDataTable().Rows.Count;
        //            AirPricingInfo_Id = drPassenger.CopyToDataTable().Rows[0]["AirPricingInfo_Id"].ToString();
        //            drFareResults = dsResponse.Tables["AirPricingInfo"].Select("AirPricingInfo_Id='" + AirPricingInfo_Id + "'");
        //            if (drFareResults.Length > 0)
        //            {
        //                DataRow[] drFareBasisResults = dsResponse.Tables["FareInfo"].Select("AirPricingInfo_Id='" + AirPricingInfo_Id + "'");
        //                if (drFareBasisResults.Length > 0)
        //                {
        //                    FareBasis += "|";
        //                    FareBasis += drFareBasisResults.CopyToDataTable().Rows[0]["FareBasis"].ToString().ToUpper().Trim();
        //                }


        //                if (drFareResults.CopyToDataTable().Rows[0]["BasePrice"].ToString().IndexOf("INR") != -1)
        //                {
        //                    drAdd["Chd_BASIC"] = drFareResults.CopyToDataTable().Rows[0]["BasePrice"].ToString().Replace("INR", "").Trim();
        //                }
        //                else if (drFareResults.CopyToDataTable().Rows[0]["ApproximateBasePrice"].ToString().IndexOf("INR") != -1)
        //                {
        //                    drAdd["Chd_BASIC"] = drFareResults.CopyToDataTable().Rows[0]["ApproximateBasePrice"].ToString().Replace("INR", "").Trim();
        //                }

        //                drAdd["ChdTotalTax"] = drFareResults.CopyToDataTable().Rows[0]["Taxes"].ToString().Replace("INR", "").Trim();
        //            }

        //            drTaxResults = dsResponse.Tables["TaxInfo"].Select("AirPricingInfo_Id='" + AirPricingInfo_Id + "'");
        //            if (drTaxResults.Length > 0)
        //            {
        //                int K3 = 0;
        //                int YR = 0;
        //                int YQ = 0;
        //                int WO = 0;
        //                int JN = 0;
        //                int IN = 0;
        //                int OT = 0;

        //                foreach (DataRow drTax in drTaxResults.CopyToDataTable().Rows)
        //                {
        //                    if (drTax["Category"].ToString().Trim().Equals("K3"))
        //                    {
        //                        K3 += Convert.ToInt32(drTax["Amount"].ToString().Trim().Replace("INR", "").Trim());
        //                    }
        //                    else if (drTax["Category"].ToString().Trim().Equals("YR"))
        //                    {
        //                        YR += Convert.ToInt32(drTax["Amount"].ToString().Trim().Replace("INR", "").Trim());
        //                    }
        //                    else if (drTax["Category"].ToString().Trim().Equals("YQ"))
        //                    {
        //                        YQ += Convert.ToInt32(drTax["Amount"].ToString().Trim().Replace("INR", "").Trim());
        //                    }
        //                    else if (drTax["Category"].ToString().Trim().Equals("WO"))
        //                    {
        //                        WO += Convert.ToInt32(drTax["Amount"].ToString().Trim().Replace("INR", "").Trim());
        //                    }
        //                    else if (drTax["Category"].ToString().Trim().Equals("JN"))
        //                    {
        //                        JN += Convert.ToInt32(drTax["Amount"].ToString().Trim().Replace("INR", "").Trim());
        //                    }
        //                    else if (drTax["Category"].ToString().Trim().Equals("IN") || drTax["Category"].ToString().Trim().Equals("YM"))
        //                    {
        //                        IN += Convert.ToInt32(drTax["Amount"].ToString().Trim().Replace("INR", "").Trim());
        //                    }
        //                    else
        //                    {
        //                        OT += Convert.ToInt32(drTax["Amount"].ToString().Trim().Replace("INR", "").Trim());
        //                    }
        //                }

        //                drAdd["Chd_YQ"] = YQ;
        //                drAdd["Chd_PSF"] = WO;
        //                drAdd["Chd_UDF"] = IN;
        //                drAdd["Chd_AUDF"] = 0;
        //                drAdd["Chd_CUTE"] = YR;
        //                drAdd["Chd_GST"] = K3;
        //                drAdd["Chd_TF"] = JN;
        //                drAdd["Chd_CESS"] = 0;
        //                drAdd["Chd_EX"] = OT;
        //            }
        //        }

        //        //==============================================================================================
        //        drPassenger = dsResponse.Tables["PassengerType"].Select("Code='" + "INF" + "'");
        //        if (drPassenger.Length > 0 && drPassenger.CopyToDataTable().Rows.Count > 0)
        //        {
        //            drAdd["Inf"] = drPassenger.CopyToDataTable().Rows.Count;

        //            AirPricingInfo_Id = drPassenger.CopyToDataTable().Rows[0]["AirPricingInfo_Id"].ToString();
        //            drFareResults = dsResponse.Tables["AirPricingInfo"].Select("AirPricingInfo_Id='" + AirPricingInfo_Id + "'");
        //            if (drFareResults.Length > 0)
        //            {
        //                DataRow[] drFareBasisResults = dsResponse.Tables["FareInfo"].Select("AirPricingInfo_Id='" + AirPricingInfo_Id + "'");
        //                if (drFareBasisResults.Length > 0)
        //                {
        //                    FareBasis += "|";
        //                    FareBasis += drFareBasisResults.CopyToDataTable().Rows[0]["FareBasis"].ToString().ToUpper().Trim();
        //                }

        //                if (drFareResults.CopyToDataTable().Rows[0]["BasePrice"].ToString().IndexOf("INR") != -1)
        //                {
        //                    drAdd["Inf_BASIC"] = drFareResults.CopyToDataTable().Rows[0]["BasePrice"].ToString().Replace("INR", "").Trim();
        //                }
        //                else if (drFareResults.CopyToDataTable().Rows[0]["ApproximateBasePrice"].ToString().IndexOf("INR") != -1)
        //                {
        //                    drAdd["Inf_BASIC"] = drFareResults.CopyToDataTable().Rows[0]["ApproximateBasePrice"].ToString().Replace("INR", "").Trim();
        //                }

        //                drAdd["InfTotalTax"] = drFareResults.CopyToDataTable().Rows[0]["Taxes"].ToString().Replace("INR", "").Trim();
        //            }

        //            drTaxResults = dsResponse.Tables["TaxInfo"].Select("AirPricingInfo_Id='" + AirPricingInfo_Id + "'");
        //            if (drTaxResults.Length > 0)
        //            {
        //                int OT = 0;
        //                foreach (DataRow drTax in drTaxResults.CopyToDataTable().Rows)
        //                {
        //                    OT += Convert.ToInt32(drTax["Amount"].ToString().Trim().Replace("INR", "").Trim());
        //                }
        //                drAdd["Inf_TAX"] = OT;
        //            }
        //        }

        //    }
        //}
    }
}
