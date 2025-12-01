using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace ZealTravel.Infrastructure.DBCommon
{
    public class GetBookingdetail
    {
        public bool set_BookingAirlineLogForPG(bool IsCombi, bool IsRT, string SearchID, string CompanyID, Int32 BookingRef, string AvailabilityResponse, string PassengerResponse, string RefID_O, string RefID_I, int PaymentID, string PaymentType)
        {
            bool Status = false;
            try
            {
                using (SqlConnection connection = new SqlConnection(ConnectionString.dbConnect))
                {
                    SqlCommand cmd = new SqlCommand("BookingAirlineLogForPG_Proc", connection);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 10;
                    cmd.Parameters.Add(@"ProcNo", SqlDbType.Int).Value = 1;
                    cmd.Parameters.Add(@"CompanyID", SqlDbType.VarChar).Value = CompanyID;
                    cmd.Parameters.Add(@"BookingRef", SqlDbType.Int).Value = BookingRef;
                    cmd.Parameters.Add(@"AvailabilityResponse", SqlDbType.VarChar).Value = AvailabilityResponse;
                    cmd.Parameters.Add(@"PassengerResponse", SqlDbType.VarChar).Value = PassengerResponse;
                    cmd.Parameters.Add(@"RefID_O", SqlDbType.VarChar).Value = RefID_O;
                    cmd.Parameters.Add(@"RefID_I", SqlDbType.VarChar).Value = RefID_I;
                    cmd.Parameters.Add(@"SearchID", SqlDbType.VarChar).Value = SearchID;
                    cmd.Parameters.Add(@"IsCombi", SqlDbType.Bit).Value = IsCombi;
                    cmd.Parameters.Add(@"IsRT", SqlDbType.Bit).Value = IsRT;
                    cmd.Parameters.Add(@"PaymentID", SqlDbType.Int).Value = PaymentID;
                    cmd.Parameters.Add(@"PaymentType", SqlDbType.VarChar).Value = PaymentType;

                    connection.Open();
                    int i = cmd.ExecuteNonQuery();
                    connection.Close();

                    if (i > 0)
                    {
                        Status = true;
                    }
                }
            }
            catch (Exception ex)
            {
                dbCommon.Logger.dbLogg(CompanyID, BookingRef, PassengerResponse, "set_BookingAirlineLogForPG", SearchID, AvailabilityResponse, ex.Message);
            }
            return Status;
        }
        public DataTable get_BookingAirlineLogForPG(string CompanyID, Int32 PaymentID)
        {
            DataTable dtHoteldata = new DataTable();
            try
            {
                using (SqlConnection connection = new SqlConnection(ConnectionString.dbConnect))
                {
                    SqlCommand cmd = new SqlCommand("BookingAirlineLogForPG_Proc", connection);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 10;
                    cmd.Parameters.Add(@"ProcNo", SqlDbType.Int).Value = 2;
                    cmd.Parameters.Add(@"PaymentID", SqlDbType.Int).Value = PaymentID;
                    cmd.Parameters.Add(@"CompanyID", SqlDbType.VarChar).Value = CompanyID;
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dtHoteldata);
                }
            }
            catch (Exception ex)
            {
                dbCommon.Logger.dbLogg(CompanyID, 0, "", "get_BookingAirlineLogForPG", PaymentID.ToString(), "PayemntID", ex.Message);
            }
            return dtHoteldata;
        }
        public static bool Booking_PNR_Status(Int32 BookingRef)
        {
            bool Status = false;

            try
            {
                using (SqlConnection connection = new SqlConnection(ConnectionString.dbConnect))
                {
                    SqlCommand cmd = new SqlCommand("getBookingDetail_Proc", connection);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 10;
                    cmd.Parameters.Add(@"ProcNo", SqlDbType.Int).Value = 3;
                    cmd.Parameters.Add(@"BookingRef", SqlDbType.Int).Value = BookingRef;

                    connection.Open();
                    Status = (bool)cmd.ExecuteScalar();
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                dbCommon.Logger.dbLogg("", BookingRef, "Booking_PNR_Status", "dbCommon", "getBookingDetail_Proc", "", ex.Message);
            }

            return Status;
        }
        public static bool Booking_Ticket_Status(Int32 BookingRef)
        {
            bool Status = false;

            try
            {
                using (SqlConnection connection = new SqlConnection(ConnectionString.dbConnect))
                {
                    SqlCommand cmd = new SqlCommand("getBookingDetail_Proc", connection);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 10;
                    cmd.Parameters.Add(@"ProcNo", SqlDbType.Int).Value = 4;
                    cmd.Parameters.Add(@"BookingRef", SqlDbType.Int).Value = BookingRef;

                    connection.Open();
                    Status = (bool)cmd.ExecuteScalar();
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                dbCommon.Logger.dbLogg("", BookingRef, "Booking_Ticket_Status", "dbCommon", "getBookingDetail_Proc", "", ex.Message);
            }

            return Status;
        }
        public static bool BookingStatus(Int32 BookingRef, bool Status)
        {
            bool bStatus = false;

            try
            {
                using (SqlConnection connection = new SqlConnection(ConnectionString.dbConnect))
                {
                    SqlCommand cmd = new SqlCommand("BookingStatus_Proc", connection);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 10;
                    cmd.Parameters.Add(@"ProcNo", SqlDbType.Int).Value = 1;
                    cmd.Parameters.Add(@"BookingRef", SqlDbType.Int).Value = BookingRef;
                    cmd.Parameters.Add(@"Status", SqlDbType.Bit).Value = Status;

                    connection.Open();
                    int iRows = cmd.ExecuteNonQuery();
                    connection.Close();

                    if (iRows > 0)
                    {
                        bStatus = true;
                    }
                }
            }
            catch (Exception ex)
            {
                dbCommon.Logger.dbLogg("", BookingRef, "BookingStatus", "dbCommon", "BookingStatus_Proc", "", ex.Message);
            }

            return bStatus;
        }
        public DataSet getBooking(Int32 BookingRef)
        {
            DataSet dsBooking = new DataSet();

            try
            {
                using (SqlConnection connection = new SqlConnection(ConnectionString.dbConnect))
                {
                    SqlCommand cmd = new SqlCommand("getBookingDetail_Proc", connection);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 10;
                    cmd.Parameters.Add(@"ProcNo", SqlDbType.Int).Value = 1;
                    cmd.Parameters.Add(@"BookingRef", SqlDbType.Int).Value = BookingRef;
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);

                    adapter.TableMappings.Add("Table", "Flight_Detail");
                    adapter.TableMappings.Add("Table1", "Flight_Segment_Detail");
                    adapter.TableMappings.Add("Table2", "Fare_Detail");
                    adapter.TableMappings.Add("Table3", "Fare_Detail_Segment");
                    adapter.TableMappings.Add("Table4", "Pax_Detail");
                    adapter.TableMappings.Add("Table5", "Pax_Segment_Detail");
                    adapter.TableMappings.Add("Table6", "CompanyDetail");
                    adapter.TableMappings.Add("Table7", "Flight_FareRule_Detail");
                    adapter.TableMappings.Add("Table8", "Flight_Gst_Detail");
                    adapter.TableMappings.Add("Table9", "Transaction_Detail");
                    adapter.TableMappings.Add("Table10", "Flight_Own_Segments_Pnr");
                    adapter.TableMappings.Add("Table11", "Flight_Own_Pnr");
                    adapter.TableMappings.Add("Table12", "Payment_Gateway_Logger");
                    adapter.TableMappings.Add("Table13", "BookingAirlineLogForPG");
                    adapter.Fill(dsBooking);
                }
            }
            catch (Exception ex)
            {
                dbCommon.Logger.dbLogg("", BookingRef, "getBooking", "dbCommon", "getBookingDetail_Proc", "", ex.Message);
            }

            return dsBooking;
        }
        //public DataSet getCancelBooking(Int32 BookingRef)
        //{
        //    DataSet dsBooking = new DataSet();

        //    try
        //    {
        //        using (SqlConnection connection = new SqlConnection(ConnectionString.dbConnect))
        //        {
        //            SqlCommand cmd = new SqlCommand("getBookingDetail_Proc", connection);
        //            cmd.CommandType = CommandType.StoredProcedure;
        //            cmd.CommandTimeout = 10;
        //            cmd.Parameters.Add(@"ProcNo", SqlDbType.Int).Value = 20;
        //            cmd.Parameters.Add(@"BookingRef", SqlDbType.Int).Value = BookingRef;
        //            SqlDataAdapter adapter = new SqlDataAdapter(cmd);

        //            adapter.TableMappings.Add("Table", "Flight_Detail");
        //            adapter.TableMappings.Add("Table1", "Flight_Segment_Detail");
        //            adapter.TableMappings.Add("Table2", "Fare_Detail_Segment");
        //            adapter.TableMappings.Add("Table3", "Pax_Segment_Cancellation");
        //            adapter.TableMappings.Add("Table4", "Pax_Segment_Detail_SSR");
        //            adapter.TableMappings.Add("Table5", "CompanyDetail");
        //            adapter.Fill(dsBooking);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        dbCommon.Logger.dbLogg("", BookingRef, "getCancelBooking", "dbCommon", "getBookingDetail_Proc", "", ex.Message);
        //    }

        //    return dsBooking;
        //}
    }
}
