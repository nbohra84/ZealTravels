using System;
using System.Collections;
using System.Linq;
using System.Text;
using System.Data;

namespace ZealTravel.Infrastructure.DBCommon
{
    public class Add_Airline_Detail
    {
        public static void Add_Detail(DataSet dsBound)
        {
            Add_Cancellation_Rescheduling_FareRule_Baggage(dsBound);
            Add_AiportName(dsBound);
            Add_CarrierName(dsBound);
            //Add_PriceTypeDetail(dsBound);
            Add_Stops(dsBound);
            Add_Journey_Duration_TimeDetail(dsBound);
        }

        private static void Add_Cancellation_Rescheduling_FareRule_Baggage(DataSet dsBound)
        {
            string Sector = dsBound.Tables["AvailabilityResponse"].Rows[0]["Sector"].ToString().Trim();
            DataSet dsDetail = AirlineBaggage.Airline_Cancellation_Rescheduling_Fare_Rule_Baggage_Detail(Sector);

            if (dsDetail != null && dsDetail.Tables.Count > 0)
            {
                Add_Cancellation_Rescheduling_Detail(dsBound, dsDetail.Tables["Airline_Cancellation_Rescheduling_Charge"]);
                Add_FareRuleDetail(dsBound, dsDetail.Tables["Airline_Fare_Rule"]);
                Add_BaggageDetail(dsBound, dsDetail.Tables["Airline_Baggage"]);
            }

            dsBound.AcceptChanges();
        }
        private static void Add_Cancellation_Rescheduling_Detail(DataSet dsBound, DataTable dtCanRes)
        {
            try
            {
                if (dtCanRes != null && dtCanRes.Rows.Count > 0)
                {
                    foreach (DataTable table in dsBound.Tables)
                    {
                        foreach (DataRow dr in table.Rows)
                        {
                            DataRow[] drSelect = dtCanRes.Select("CarrierCode='" + dr["CarrierCode"].ToString().Trim() + "'");
                            if (drSelect.Length > 0)
                            {
                               // dr["CancellationFee"] = Convert.ToInt32(drSelect.CopyToDataTable().Rows[0]["Cancellation_Fee"].ToString());

                                dr["CancellationFee"] = Convert.ToInt32(drSelect[0]["Cancellation_Fee"].ToString());

                                //dr["DateChangeFee"] = Convert.ToInt32(drSelect.CopyToDataTable().Rows[0]["Re_Booking_Fee"].ToString());
                                dr["DateChangeFee"] = Convert.ToInt32(drSelect[0]["Re_Booking_Fee"].ToString());
                            }
                        }
                    }
                }

                dsBound.AcceptChanges();
            }
            catch
            {

            }
        }
        private static void Add_FareRuleDetail(DataSet dsBound, DataTable dtFareRule)
        {
            try
            {
                if (dtFareRule != null && dtFareRule.Rows.Count > 0)
                {
                    foreach (DataTable table in dsBound.Tables)
                    {
                        foreach (DataRow dr in table.Rows)
                        {
                            DataRow[] drSelect = dtFareRule.Select("CarrierCode='" + dr["CarrierCode"].ToString().Trim() + "'");
                            if (drSelect.Length > 0)
                            {
                                // dr["FareRuledb"] = drSelect.CopyToDataTable().Rows[0]["Fare_Rule"].ToString().Trim();
                                dr["FareRuledb"] = drSelect[0]["Fare_Rule"].ToString().Trim();
                            }
                            else
                            {
                                dr["FareRuledb"] = "Contact our Callcenter";
                            }
                        }
                    }
                }

                dsBound.AcceptChanges();
            }
            catch
            {

            }
        }
        private static void Add_BaggageDetail(DataSet dsBound, DataTable dtBaggageDetail)
        {
            try
            {
                if (dtBaggageDetail != null && dtBaggageDetail.Rows.Count > 0)
                {
                    foreach (DataTable table in dsBound.Tables)
                    {
                        foreach (DataRow dr in table.Rows)
                        {
                            if (dr["BaggageDetail"].ToString().Trim().Length.Equals(0))
                            {
                                DataRow[] drSelect = dtBaggageDetail.Select("CarrierCode='" + dr["CarrierCode"].ToString().Trim() + "'");
                                if (drSelect.Length > 0)
                                {
                                    //dr["BaggageDetail"] = drSelect.CopyToDataTable().Rows[0]["Check_in_Baggage"].ToString().Trim() + "*" + drSelect.CopyToDataTable().Rows[0]["Cabin_Baggage"].ToString().Trim();
                                    dr["BaggageDetail"] = drSelect[0]["Check_in_Baggage"].ToString().Trim() + "*" + drSelect[0]["Cabin_Baggage"].ToString().Trim();

                                }
                                else if (dr["ProductClass"].ToString().Trim().Trim().Length.Equals(3))
                                {
                                    dr["BaggageDetail"] = dr["ProductClass"].ToString().Trim() + "*" + " 7K ";
                                }
                                else
                                {
                                    dr["BaggageDetail"] = " Contact to our CallCenter ";
                                }

                                if (dr["CarrierCode"].ToString().Trim().Equals("6E"))
                                {
                                    if (dr["ProductClass"].ToString().Trim().Equals("B"))
                                    {
                                        //dr["BaggageDetail"] = "0" + "*" + drSelect.CopyToDataTable().Rows[0]["Cabin_Baggage"].ToString().Trim();
                                        dr["BaggageDetail"] = "0" + "*" + drSelect[0]["Cabin_Baggage"].ToString().Trim();
                                    }
                                }
                                else if (dr["CarrierCode"].ToString().Trim().Equals("SG"))
                                {
                                    if (dr["ProductClass"].ToString().Trim().Equals("HO"))
                                    {
                                        //dr["BaggageDetail"] = "0" + "*" + drSelect.CopyToDataTable().Rows[0]["Cabin_Baggage"].ToString().Trim();
                                        dr["BaggageDetail"] = "0" + "*" + drSelect[0]["Cabin_Baggage"].ToString().Trim();
                                    }
                                }
                            }
                        }
                    }
                }

                dsBound.AcceptChanges();
            }
            catch
            {

            }
        }

        private static void Add_AiportName(DataSet dsBound)
        {
            try
            {
                ArrayList AirportCode = new ArrayList();
                if (dsBound.Tables["AvailabilityResponse"] != null)
                {
                    AirportCode.AddRange(CommonFunction.DataTable2ArrayList(dsBound.Tables["AvailabilityResponse"], "DepartureStation", true));
                    AirportCode.AddRange(CommonFunction.DataTable2ArrayList(dsBound.Tables["AvailabilityResponse"], "ArrivalStation", true));
                    AirportCode = CommonFunction.RemoveDuplicates(AirportCode);
                }

                string AirportList = CommonFunction.ArrayListToString(AirportCode, ",");
                DataTable dtAiportName = Airline_Detail.CityAirportName(AirportList);
                if (dtAiportName != null && dtAiportName.Rows.Count > 0)
                {
                    foreach (DataTable table in dsBound.Tables)
                    {
                        foreach (DataRow dr in table.Rows)
                        {
                            DataRow[] drResult = dtAiportName.Select("Airport_Code='" + dr["DepartureStation"].ToString().Trim() + "'");
                            if (drResult.Length > 0)
                            {
                                //dr["DepartureStationAirport"] = drResult.CopyToDataTable().Rows[0]["Airport_Name"].ToString().Trim();
                                dr["DepartureStationAirport"] = drResult[0]["Airport_Name"].ToString().Trim();
                            }
                            else
                            {
                                dr["DepartureStationAirport"] = "Not Found";
                            }

                            drResult = dtAiportName.Select("Airport_Code='" + dr["ArrivalStation"].ToString().Trim() + "'");
                            if (drResult.Length > 0)
                            {
                                //dr["ArrivalStationAirport"] = drResult.CopyToDataTable().Rows[0]["Airport_Name"].ToString().Trim();
                                dr["ArrivalStationAirport"] = drResult[0]["Airport_Name"].ToString().Trim();
                            }
                            else
                            {
                                dr["ArrivalStationAirport"] = "Not Found";
                            }

                            drResult = dtAiportName.Select("Airport_Code='" + dr["DepartureStation"].ToString().Trim() + "'");
                            if (drResult.Length > 0)
                            {
                                //dr["DepartureStationName"] = drResult.CopyToDataTable().Rows[0]["City_Name"].ToString().Trim();
                                dr["DepartureStationName"] = drResult[0]["City_Name"].ToString().Trim();
                            }
                            else
                            {
                                dr["DepartureStationName"] = "Not Found";
                            }

                            drResult = dtAiportName.Select("Airport_Code='" + dr["ArrivalStation"].ToString().Trim() + "'");
                            if (drResult.Length > 0)
                            {
                                //dr["ArrivalStationName"] = drResult.CopyToDataTable().Rows[0]["City_Name"].ToString().Trim();
                                dr["ArrivalStationName"] = drResult[0]["City_Name"].ToString().Trim();
                            }
                            else
                            {
                                dr["ArrivalStationName"] = "Not Found";
                            }
                        }
                    }
                }

                dsBound.AcceptChanges();
            }
            catch
            {

            }
        }
        private static void Add_CarrierName(DataSet dsBound)
        {
            try
            {
                ArrayList CarrierList = CommonFunction.DataTable2ArrayList(dsBound.Tables[0], "CarrierCode", true);
                string Carrier = CommonFunction.ArrayListToString(CarrierList, ",");
                DataTable dtCarrierName = Airline_Detail.CarrierName(Carrier);
                if (dtCarrierName != null && dtCarrierName.Rows.Count > 0)
                {
                    foreach (DataTable table in dsBound.Tables)
                    {
                        foreach (DataRow dr in table.Rows)
                        {
                            DataRow[] drResult = dtCarrierName.Select("CarrierCode='" + dr["CarrierCode"].ToString().Trim() + "'");
                            if (drResult.Length > 0)
                            {
                                //dr["CarrierName"] = drResult.CopyToDataTable().Rows[0]["CarrierName"].ToString().Trim();
                                dr["CarrierName"] = drResult[0]["CarrierName"].ToString().Trim();
                            }
                            else
                            {
                                dr["CarrierName"] = "NotFound";
                            }
                        }
                    }
                }
                dsBound.AcceptChanges();
            }
            catch
            {

            }
        }

        private static void Add_PriceTypeDetail(DataSet dsBound)
        {
            try
            {
                if (dsBound.Tables[0].Rows[0]["AirlineID"].ToString().IndexOf("TBO") == -1)
                {
                    foreach (DataTable table in dsBound.Tables)
                    {
                        foreach (DataRow dr in table.Rows)
                        {
                            if (dr["CarrierCode"].ToString().Trim().Equals("6E"))
                            {
                                if (dr["ProductClass"].ToString().Trim().Equals("A"))
                                {
                                    dr["PriceType"] = "Familyfare";
                                }
                                else if (dr["ProductClass"].ToString().Trim().Equals("S"))
                                {
                                    dr["PriceType"] = "Salefare";
                                }
                                else if (dr["ProductClass"].ToString().Trim().Equals("J"))
                                {
                                    dr["PriceType"] = "Flexifare";
                                }
                                else if (dr["ProductClass"].ToString().Trim().Equals("B"))
                                {
                                    dr["PriceType"] = "HBfare";
                                }
                                else if (dr["ProductClass"].ToString().Trim().Equals("R"))
                                {
                                    dr["PriceType"] = "Regularfare";
                                }
                                else if (dr["ProductClass"].ToString().Trim().Equals("N"))
                                {
                                    dr["PriceType"] = "RTfare";
                                }

                                if (dr["API_AirlineID"].ToString().IndexOf("KTDEL132") != -1)
                                {
                                    dr["PriceType"] = "Corporatefare";
                                }
                                else if (dr["API_AirlineID"].ToString().IndexOf("SCN7621") != -1)
                                {
                                    dr["PriceType"] = "Specialfare";
                                }
                                if (dr["API_AirlineID"].ToString().IndexOf("STAN0008") != -1)
                                {
                                    dr["PriceType"] = "Specialfare";
                                }

                                if (dr["PriceType"].ToString().Trim().Length.Equals(0))
                                {
                                    dr["PriceType"] = "Regularfare";
                                }
                            }
                            else if (dr["CarrierCode"].ToString().Trim().Equals("G8"))
                            {
                                if (dr["ProductClass"].ToString().Trim().Equals("GS"))
                                {
                                    dr["PriceType"] = "Regularfare";
                                }
                                else if (dr["ProductClass"].ToString().Trim().Equals("GF"))
                                {
                                    dr["PriceType"] = "Flexifare";
                                }
                                else if (dr["ProductClass"].ToString().Trim().Equals("GB"))
                                {
                                    dr["PriceType"] = "Businessfare";
                                }
                                else if (dr["ProductClass"].ToString().Trim().IndexOf("GT") != -1)
                                {
                                    dr["PriceType"] = "Valuefare";
                                }
                                else if (dr["ProductClass"].ToString().Trim().IndexOf("GV") != -1)
                                {
                                    dr["PriceType"] = "Valuefare";
                                }
                                else if (dr["ProductClass"].ToString().Trim().IndexOf("SP") != -1)
                                {
                                    dr["PriceType"] = "Specialfare";
                                }
                                else if (dr["ProductClass"].ToString().Trim().IndexOf("BC") != -1)
                                {
                                    dr["PriceType"] = "Corporatefare";
                                }
                                else
                                {
                                    dr["PriceType"] = "Regularfare";
                                }
                            }
                            else if (dr["CarrierCode"].ToString().Trim().Equals("SG"))
                            {
                                if (dr["ProductClass"].ToString().Trim().Equals("SS") || dr["ProductClass"].ToString().Trim().Equals("NF") || dr["ProductClass"].ToString().Trim().Equals("NN"))
                                {
                                    dr["PriceType"] = "Salefare";
                                }
                                else if (dr["ProductClass"].ToString().Trim().Equals("XB"))
                                {
                                    dr["PriceType"] = "Familyfare";
                                }
                                else if (dr["ProductClass"].ToString().Trim().Equals("XA"))
                                {
                                    dr["PriceType"] = "RTfare";
                                }
                                else if (dr["ProductClass"].ToString().Trim().Equals("HO"))
                                {
                                    dr["PriceType"] = "HBfare";
                                }
                                else if (dr["ProductClass"].ToString().Trim().Equals("RS"))
                                {
                                    dr["PriceType"] = "Saverfare";
                                }
                                else if (dr["ProductClass"].ToString().Trim().Equals("HF"))
                                {
                                    dr["PriceType"] = "Flexifare";
                                }

                                if (dr["API_AirlineID"].ToString().IndexOf("SGCPN_N008") != -1)
                                {
                                    dr["PriceType"] = "Specialfare";
                                }
                                else if (dr["API_AirlineID"].ToString().IndexOf("SVCORT0004") != -1)
                                {
                                    dr["PriceType"] = "Corporatefare";
                                }
                                if (dr["API_AirlineID"].ToString().IndexOf("STDELC2581") != -1)
                                {
                                    dr["PriceType"] = "Corporatefare";
                                }

                                if (dr["PriceType"].ToString().Trim().Length.Equals(0))
                                {
                                    dr["PriceType"] = "Regularfare";
                                }
                            }
                            else
                            {
                                dr["PriceType"] = "Regularfare";
                            }
                        }
                    }

                    dsBound.AcceptChanges();
                }
            }
            catch
            {

            }
        }
        private static void Add_Stops(DataSet dsBound)
        {
            try
            {
                foreach (DataRow dr in dsBound.Tables["AvailabilityResponse"].Rows)
                {
                    if (dr["FltType"].ToString().Equals("O"))
                    {
                        DataRow[] drStops = dsBound.Tables["AvailabilityResponse"].Select("RefID='" + dr["RefID"].ToString() + "' And FltType='" + "O" + "'");
                        dr["Stops"] = drStops.Length - 1;
                    }
                    else if (dr["FltType"].ToString().Equals("I"))
                    {
                        DataRow[] drStops = dsBound.Tables["AvailabilityResponse"].Select("RefID='" + dr["RefID"].ToString() + "' And FltType='" + "I" + "'");
                        dr["Stops"] = drStops.Length - 1;
                    }
                }

                dsBound.AcceptChanges();
            }
            catch
            {

            }
        }
        private static void Add_Journey_Duration_TimeDetail(DataSet dsBound)
        {
            try
            {
                SetJourneyTime(dsBound);
                foreach (DataTable table in dsBound.Tables)
                {
                    foreach (DataRow dr in table.Rows)
                    {
                        dr["JourneyTimeDesc"] = dbCommon.DateTimeFormatter.ConvertTmDesc(dr["JourneyTime"].ToString());

                        if (Convert.ToInt32(dr["Duration"].ToString()) > 0)
                        {
                            dr["DurationDesc"] = dbCommon.DateTimeFormatter.ConvertTmDesc(dr["Duration"].ToString());
                        }
                        else
                        {
                            dr["Duration"] = dbCommon.DateTimeFormatter.GetDuration(dr["DepDate"].ToString(), dr["ArrDate"].ToString());
                            dr["DurationDesc"] = dbCommon.DateTimeFormatter.ConvertTmDesc(dr["Duration"].ToString());
                        }

                        dr["DepartureDate"] = dbCommon.DateTimeFormatter.DateFormat(dr["DepDate"].ToString());
                        dr["ArrivalDate"] = dbCommon.DateTimeFormatter.DateFormat(dr["ArrDate"].ToString());
                        dr["DepartureTime"] = dbCommon.DateTimeFormatter.TimeFormat(dr["DepTime"].ToString());
                        dr["ArrivalTime"] = dbCommon.DateTimeFormatter.TimeFormat(dr["ArrTime"].ToString());
                    }
                }

                dsBound.AcceptChanges();
            }
            catch
            {

            }
        }

        private static void SetJourneyTime(DataSet dsBound)
        {
            foreach (DataTable table in dsBound.Tables)
            {
                foreach (DataRow dr in table.Rows)
                {
                    if (Convert.ToInt32(dr["JourneyTime"].ToString()).Equals(0))
                    {
                        SetJourneyTime(dr["RefID"].ToString(), dr["FltType"].ToString(), dsBound);
                    }
                }
            }
        }
        private static void SetJourneyTime(string RefID, string FltType, DataSet dsBound)
        {
            int iJourneyTime = 0;
            DataRow[] rows = dsBound.Tables[0].Select("RefID ='" + RefID + "' And FltType='" + FltType + "'");
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
                        dsBound.Tables[0].AcceptChanges();
                        row.SetModified();
                    }
                }
                else
                {
                    //iJourneyTime = DateTimeFormatter.GetDuration(rows.CopyToDataTable().Rows[0]["DepDate"].ToString(), rows.CopyToDataTable().Rows[rows.CopyToDataTable().Rows.Count - 1]["ArrDate"].ToString());
                    iJourneyTime = DateTimeFormatter.GetDuration(rows[0]["DepDate"].ToString(), rows[rows[0].ItemArray.Length  - 1]["ArrDate"].ToString());
                    if (iJourneyTime > 0)
                    {
                        foreach (DataRow row in rows)
                        {
                            if (Convert.ToInt32(row["JourneyTime"].ToString()).Equals(0))
                            {
                                row["JourneyTime"] = iJourneyTime;
                            }
                            dsBound.Tables[0].AcceptChanges();
                            row.SetModified();
                        }
                    }
                }
            }
        }
    }
}
