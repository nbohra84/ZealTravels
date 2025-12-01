using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZealTravel.Infrastructure.FlightManagement
{
    public class AddAirlineDetails
    {
        string _connectionstring;
        public AddAirlineDetails(IConfiguration config) {
            _connectionstring = config.GetConnectionString("DefaultConnection");
        }
        public void Add_Detail(DataSet dsBound)
        {
            //Add_Cancellation_Rescheduling_FareRule_Baggage(dsBound);
            Add_AiportName(dsBound);
            Add_CarrierName(dsBound);
            //Add_PriceTypeDetail(dsBound);
            Add_Stops(dsBound);
            Add_Journey_Duration_TimeDetail(dsBound);
        }
        public  DataTable CityAirportName(string AirportList)
        {
            DataTable dtAirport = new DataTable();

            try
            {
                if (!String.IsNullOrEmpty(_connectionstring))
                {
                    using (SqlConnection connection = new SqlConnection(_connectionstring))
                    {
                        SqlCommand cmd = new SqlCommand("St_CityAirportName_Proc", connection);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandTimeout = 10;
                        cmd.Parameters.Add(@"ProcNo", SqlDbType.Int).Value = 2;
                        cmd.Parameters.Add(@"AirportList", SqlDbType.VarChar).Value = AirportList;

                        SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                        adapter.Fill(dtAirport);
                    }
                }
            }
            catch (Exception ex)
            {

            }

            return dtAirport;
        }

        private  void Add_AiportName(DataSet dsBound)
        {
            try
            {
                ArrayList AirportCode = new ArrayList();
                if (dsBound.Tables["AvailabilityInfo"] != null)
                {
                    AirportCode.AddRange(DBCommon.CommonFunction.DataTable2ArrayList(dsBound.Tables["AvailabilityInfo"], "DepartureStation", true));
                    AirportCode.AddRange(DBCommon.CommonFunction.DataTable2ArrayList(dsBound.Tables["AvailabilityInfo"], "ArrivalStation", true));
                    AirportCode = DBCommon.CommonFunction.RemoveDuplicates(AirportCode);
                }

                string AirportList = DBCommon.CommonFunction.ArrayListToString(AirportCode, ",");
                DataTable dtAiportName = CityAirportName(AirportList);
                if (dtAiportName != null && dtAiportName.Rows.Count > 0)
                {
                    foreach (DataTable table in dsBound.Tables)
                    {
                        foreach (DataRow dr in table.Rows)
                        {
                            DataRow[] drResult = dtAiportName.Select("Airport_Code='" + dr["DepartureStation"].ToString().Trim() + "'");
                            if (drResult.Length > 0)
                            {
                                dr["DepartureStationAirport"] = drResult.CopyToDataTable().Rows[0]["Airport_Name"].ToString().Trim();
                            }
                            else
                            {
                                dr["DepartureStationAirport"] = "Not Found";
                            }

                            drResult = dtAiportName.Select("Airport_Code='" + dr["ArrivalStation"].ToString().Trim() + "'");
                            if (drResult.Length > 0)
                            {
                                dr["ArrivalStationAirport"] = drResult.CopyToDataTable().Rows[0]["Airport_Name"].ToString().Trim();
                            }
                            else
                            {
                                dr["ArrivalStationAirport"] = "Not Found";
                            }

                            drResult = dtAiportName.Select("Airport_Code='" + dr["DepartureStation"].ToString().Trim() + "'");
                            if (drResult.Length > 0)
                            {
                                dr["DepartureStationName"] = drResult.CopyToDataTable().Rows[0]["City_Name"].ToString().Trim();
                            }
                            else
                            {
                                dr["DepartureStationName"] = "Not Found";
                            }

                            drResult = dtAiportName.Select("Airport_Code='" + dr["ArrivalStation"].ToString().Trim() + "'");
                            if (drResult.Length > 0)
                            {
                                dr["ArrivalStationName"] = drResult.CopyToDataTable().Rows[0]["City_Name"].ToString().Trim();
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
        public  DataTable CarrierName(string CarrierList)
        {
            DataTable dtCarrier = new DataTable();

            try
            {
                if (!String.IsNullOrEmpty(_connectionstring))
                {
                    using (SqlConnection connection = new SqlConnection(_connectionstring))
                    {
                        SqlCommand cmd = new SqlCommand("CarrierDetail_Proc", connection);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandTimeout = 10;
                        cmd.Parameters.Add(@"ProcNo", SqlDbType.Int).Value = 2;
                        cmd.Parameters.Add(@"CarrierList", SqlDbType.VarChar).Value = CarrierList;

                        SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                        adapter.Fill(dtCarrier);
                    }
                }
            }
            catch (Exception ex)
            {

            }

            return dtCarrier;
        }

      

        private  void Add_CarrierName(DataSet dsBound)
        {
            try
            {
                ArrayList CarrierList = DBCommon.CommonFunction.DataTable2ArrayList(dsBound.Tables[0], "CarrierCode", true);
                string Carrier = DBCommon.CommonFunction.ArrayListToString(CarrierList, ",");
                DataTable dtCarrierName = CarrierName(Carrier);
                if (dtCarrierName != null && dtCarrierName.Rows.Count > 0)
                {
                    foreach (DataTable table in dsBound.Tables)
                    {
                        foreach (DataRow dr in table.Rows)
                        {
                            DataRow[] drResult = dtCarrierName.Select("CarrierCode='" + dr["CarrierCode"].ToString().Trim() + "'");
                            if (drResult.Length > 0)
                            {
                                dr["CarrierName"] = drResult.CopyToDataTable().Rows[0]["CarrierName"].ToString().Trim();
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

        private static void Add_Stops(DataSet dsBound)
        {
            try
            {
                foreach (DataRow dr in dsBound.Tables["AvailabilityInfo"].Rows)
                {
                    if (Convert.ToInt32(dr["Stops"].ToString().Trim()).Equals(0))
                    {
                        if (dr["FltType"].ToString().Equals("O"))
                        {
                            DataRow[] drStops = dsBound.Tables["AvailabilityInfo"].Select("RefID='" + dr["RefID"].ToString() + "' And FltType='" + "O" + "'");
                            dr["Stops"] = drStops.Length - 1;
                        }
                        else if (dr["FltType"].ToString().Equals("I"))
                        {
                            DataRow[] drStops = dsBound.Tables["AvailabilityInfo"].Select("RefID='" + dr["RefID"].ToString() + "' And FltType='" + "I" + "'");
                            dr["Stops"] = drStops.Length - 1;
                        }
                    }
                }

                dsBound.AcceptChanges();
            }
            catch
            {

            }
        }
        public static void Add_Journey_Duration_TimeDetail(DataSet dsBound)
        {
            try
            {
                SetJourneyTime(dsBound);
                foreach (DataTable table in dsBound.Tables)
                {
                    foreach (DataRow dr in table.Rows)
                    {
                        dr["JourneyTimeDesc"] = DBCommon.DateTimeFormatter.ConvertTmDesc(dr["JourneyTime"].ToString());

                        if (Convert.ToInt32(dr["Duration"].ToString()) > 0)
                        {
                            dr["DurationDesc"] = DBCommon.DateTimeFormatter.ConvertTmDesc(dr["Duration"].ToString());
                        }
                        else
                        {
                            dr["Duration"] = DBCommon.DateTimeFormatter.GetDuration(dr["DepDate"].ToString(), dr["ArrDate"].ToString());
                            dr["DurationDesc"] = DBCommon.DateTimeFormatter.ConvertTmDesc(dr["Duration"].ToString());
                        }

                        if (dr["DepartureDate"].ToString().IndexOf(",") == -1)
                        {
                            dr["DepartureDate"] = DBCommon.DateTimeFormatter.DateFormat(dr["DepDate"].ToString());
                            dr["ArrivalDate"] = DBCommon.DateTimeFormatter.DateFormat(dr["ArrDate"].ToString());
                            dr["DepartureTime"] = DBCommon.DateTimeFormatter.TimeFormat(dr["DepTime"].ToString());
                            dr["ArrivalTime"] = DBCommon.DateTimeFormatter.TimeFormat(dr["ArrTime"].ToString());
                        }
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
                    iJourneyTime = DBCommon.DateTimeFormatter.GetDuration(rows.CopyToDataTable().Rows[0]["DepDate"].ToString(), rows.CopyToDataTable().Rows[rows.CopyToDataTable().Rows.Count - 1]["ArrDate"].ToString());
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
