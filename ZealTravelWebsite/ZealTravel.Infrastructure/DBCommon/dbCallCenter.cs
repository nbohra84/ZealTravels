using System;
using System.Collections;
using System.Linq;
using System.Data;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.IO;
using System.Text;
using System.Data.SqlClient;
using System.Xml;
using System.Xml.Linq;
using System.Globalization;
using System.Xml.Schema;

namespace ZealTravel.Infrastructure.DBCommon
{
    public class dbCallCenter
    {
        public bool setOwnPnr(int BookingRef, string Pnr)
        {
            bool Status = true;

            try
            {
                using (SqlConnection connection = new SqlConnection(ConnectionString.dbConnect))
                {
                    SqlCommand cmd = new SqlCommand("Company_FLight_Own_Pnr_Proc", connection);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 10;
                    cmd.Parameters.Add(@"ProcNo", SqlDbType.Int).Value = 1;
                    cmd.Parameters.Add(@"BookingRef", SqlDbType.Int).Value = BookingRef;
                    cmd.Parameters.Add(@"Pnr", SqlDbType.VarChar).Value = Pnr;

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
                dbCommon.Logger.dbLogg("dbCallCenter", BookingRef, "setOwnPnr", "clsDB", "", Pnr, ex.Message);
            }

            return Status;
        }
        public bool BookingStatus(Int32 BookingRef, string CompanyID)
        {
            bool Status = false;

            try
            {
                using (SqlConnection connection = new SqlConnection(ConnectionString.dbConnect))
                {
                    SqlCommand cmd = new SqlCommand("Company_Flight_Detail_Airline_Proc", connection);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 10;
                    cmd.Parameters.Add(@"ProcNo", SqlDbType.Int).Value = 11;
                    cmd.Parameters.Add(@"BookingRef", SqlDbType.Int).Value = BookingRef;
                    cmd.Parameters.Add(@"CompanyID", SqlDbType.VarChar).Value = CompanyID;

                    connection.Open();
                    int i = (int)cmd.ExecuteScalar();
                    connection.Close();

                    if (i > 0)
                    {
                        Status = true;
                    }
                }
            }
            catch (Exception ex)
            {
                dbCommon.Logger.dbLogg(CompanyID, BookingRef, "BookingStatus", "dbCallCenter", "", "", ex.Message);
            }

            return Status;

        }
        public bool getPNRcount(string PNR)
        {
            bool Status = true;

            try
            {
                using (SqlConnection connection = new SqlConnection(ConnectionString.dbConnect))
                {
                    SqlCommand cmd = new SqlCommand("Company_Flight_Detail_Airline_Proc", connection);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 10;
                    cmd.Parameters.Add(@"ProcNo", SqlDbType.Int).Value = 14;
                    cmd.Parameters.Add(@"PNR", SqlDbType.VarChar).Value = PNR;

                    connection.Open();
                    int i = (int)cmd.ExecuteScalar();
                    connection.Close();

                    if (i > 0)
                    {
                        Status = false;
                    }
                }
            }
            catch (Exception ex)
            {
                dbCommon.Logger.dbLogg("dbCallCenter", 0, "getPNRcount", "dbCallCenter", "", PNR, ex.Message);
            }

            return Status;
        }
        public static bool Update_Airline_Staus_Staff(Int32 BookingRef, string StaffID, bool Status)
        {
            bool bStatus = false;

            try
            {
                using (SqlConnection connection = new SqlConnection(ConnectionString.dbConnect))
                {
                    SqlCommand cmd = new SqlCommand("Airline_CallCenter_Proc", connection);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 10;
                    cmd.Parameters.Add(@"ProcNo", SqlDbType.Int).Value = 4;
                    cmd.Parameters.Add(@"BookingRef", SqlDbType.Int).Value = BookingRef;
                    cmd.Parameters.Add(@"StaffID", SqlDbType.VarChar).Value = StaffID;
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
                dbCommon.Logger.dbLogg(StaffID, BookingRef, "Update_Airline_Staus_Staff", "dbCallCenter", "Airline_CallCenter_Proc", "", ex.Message);
            }

            return bStatus;
        }
        public static DataTable getAirlinePendingBooking()
        {
            DataTable dtAirline = new DataTable();

            try
            {
                using (SqlConnection connection = new SqlConnection(ConnectionString.dbConnect))
                {
                    SqlCommand cmd = new SqlCommand("Airline_CallCenter_Proc", connection);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 10;
                    cmd.Parameters.Add(@"ProcNo", SqlDbType.Int).Value = 2;
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dtAirline);
                }
            }
            catch (Exception ex)
            {
                dbCommon.Logger.dbLogg("", 0, "getAirlinePendingBooking", "dbCallCenter", "Airline_CallCenter_Proc", "", ex.Message);
            }

            return dtAirline;
        }
        public static bool DeleteToAirlineCallCenter(Int32 BookingRef)
        {
            bool bStatus = false;

            try
            {
                using (SqlConnection connection = new SqlConnection(ConnectionString.dbConnect))
                {
                    SqlCommand cmd = new SqlCommand("Airline_CallCenter_Proc", connection);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 10;
                    cmd.Parameters.Add(@"ProcNo", SqlDbType.Int).Value = 3;
                    cmd.Parameters.Add(@"BookingRef", SqlDbType.Int).Value = BookingRef;

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
                dbCommon.Logger.dbLogg("", BookingRef, "DeleteToAirlineCallCenter", "dbCallCenter", "Airline_CallCenter_Proc", "", ex.Message);
            }

            return bStatus;
        }
        public static bool FallInToAirlineCallCenter(Int32 BookingRef, string CompanyID)
        {
            bool bStatus = false;

            try
            {
                using (SqlConnection connection = new SqlConnection(ConnectionString.dbConnect))
                {
                    SqlCommand cmd = new SqlCommand("Airline_CallCenter_Proc", connection);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 10;
                    cmd.Parameters.Add(@"ProcNo", SqlDbType.Int).Value = 1;
                    cmd.Parameters.Add(@"BookingRef", SqlDbType.Int).Value = BookingRef;
                    cmd.Parameters.Add(@"CompanyID", SqlDbType.VarChar).Value = CompanyID;

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
                dbCommon.Logger.dbLogg(CompanyID, BookingRef, "FallInToAirlineCallCenter", "dbCallCenter", "Airline_CallCenter_Proc", "", ex.Message);
            }

            return bStatus;
        }

        public static bool UpdatePNR(Int32 BookingRef, string StaffID, string FltType, string Airline_PNR, string GDS_PNR, string SupplierID)
        {
            bool Status = false;

            try
            {
                using (SqlConnection connection = new SqlConnection(ConnectionString.dbConnect))
                {
                    SqlCommand cmd = new SqlCommand("Company_Flight_Detail_Airline_Proc", connection);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 10;
                    cmd.Parameters.Add(@"ProcNo", SqlDbType.Int).Value = 3;
                    cmd.Parameters.Add(@"BookingRef", SqlDbType.Int).Value = BookingRef;
                    cmd.Parameters.Add(@"FltType", SqlDbType.VarChar, 1).Value = FltType;
                    cmd.Parameters.Add(@"Airline_PNR", SqlDbType.VarChar, 15).Value = Airline_PNR;
                    cmd.Parameters.Add(@"GDS_PNR", SqlDbType.VarChar, 15).Value = GDS_PNR;
                    cmd.Parameters.Add(@"StaffID", SqlDbType.VarChar).Value = StaffID;
                    cmd.Parameters.Add(@"SupplierID", SqlDbType.VarChar).Value = SupplierID;

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
                dbCommon.Logger.dbLogg(StaffID, BookingRef, "UpdatePNR", "dbCallCenter", "Company_Flight_Detail_Airline_Proc", Airline_PNR + "-" + GDS_PNR, ex.Message);
            }

            return Status;

        }
        //public static bool UpdatePNR(Int32 BookingRef, string StaffID, string Airline_PNR, string GDS_PNR)
        //{
        //    bool Status = false;

        //    try
        //    {
        //        using (SqlConnection connection = new SqlConnection(ConnectionString.dbConnect))
        //        {
        //            SqlCommand cmd = new SqlCommand("Company_Flight_Detail_Airline_Proc", connection);
        //            cmd.CommandType = CommandType.StoredProcedure;
        //            cmd.CommandTimeout = 10;
        //            cmd.Parameters.Add(@"ProcNo", SqlDbType.Int).Value = 4;
        //            cmd.Parameters.Add(@"BookingRef", SqlDbType.Int).Value = BookingRef;
        //            cmd.Parameters.Add(@"Airline_PNR", SqlDbType.VarChar, 15).Value = Airline_PNR;
        //            cmd.Parameters.Add(@"GDS_PNR", SqlDbType.VarChar, 15).Value = GDS_PNR;
        //            cmd.Parameters.Add(@"StaffID", SqlDbType.VarChar).Value = StaffID;

        //            connection.Open();
        //            int i = cmd.ExecuteNonQuery();
        //            connection.Close();

        //            if (i > 0)
        //            {
        //                Status = true;
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        dbCommon.Logger.dbLogg(StaffID, BookingRef, "UpdatePNR", "dbCallCenter", "Company_Flight_Detail_Airline_Proc", Airline_PNR + "-" + GDS_PNR, ex.Message);
        //    }

        //    return Status;

        //}

        public static bool RejectBooking(Int32 BookingRef, string StaffID, string RejectDetail)
        {
            bool Status = RejectBookingDetail(BookingRef, RejectDetail);

            try
            {
                using (SqlConnection connection = new SqlConnection(ConnectionString.dbConnect))
                {
                    SqlCommand cmd = new SqlCommand("Company_Transaction_Detail_Proc", connection);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 10;
                    cmd.Parameters.Add(@"ProcNo", SqlDbType.Int).Value = 3;
                    cmd.Parameters.Add(@"BookingRef", SqlDbType.Int).Value = BookingRef;
                    cmd.Parameters.Add(@"StaffID", SqlDbType.VarChar).Value = StaffID;

                    connection.Open();
                    int i = cmd.ExecuteNonQuery();
                    connection.Close();

                    if (i > 0)
                    {
                        Status = true;
                    }
                    else
                    {
                        Status = false;
                    }
                }
            }
            catch (Exception ex)
            {
                dbCommon.Logger.dbLogg(StaffID, BookingRef, "RejectBooking", "dbCallCenter", "Company_Transaction_Detail_Proc", "", ex.Message);
            }

            return Status;
        }
        public static bool RejectBookingDetail(Int32 BookingRef, string RejectDetail)
        {
            bool Status = false;

            try
            {
                using (SqlConnection connection = new SqlConnection(ConnectionString.dbConnect))
                {
                    SqlCommand cmd = new SqlCommand("Company_Flight_Reject_Detail_Airline_Proco", connection);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 10;
                    cmd.Parameters.Add(@"ProcNo", SqlDbType.Int).Value = 1;
                    cmd.Parameters.Add(@"BookingRef", SqlDbType.Int).Value = BookingRef;
                    cmd.Parameters.Add(@"RejectDetail", SqlDbType.VarChar).Value = RejectDetail;

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
                dbCommon.Logger.dbLogg(RejectDetail, BookingRef, "RejectBookingDetail", "dbCallCenter", "Company_Flight_Reject_Detail_Airline_Proco", "", ex.Message);
            }

            return Status;
        }

        public static bool UpdateTicketNumber(Int32 BookingRef, ArrayList TicketNumber, bool IsModify)
        {
            int i = 0;
            string strTktNumber = string.Empty;
            string StoredProcedure = "Company_Pax_Detail_Airline_Update_TicketNumber";
            try
            {
                if (IsModify.Equals(true))
                {
                    StoredProcedure = "Company_Pax_Detail_Airline_Update_TicketNumber_Modify";
                }

                strTktNumber = dbCommon.CommonFunction.ArrayListToString(TicketNumber, ",");
                ArrayList PassengerID = new ArrayList();
                PassengerID = getPassengerID(BookingRef);

                int Count = PassengerID.Count;
                SqlConnection SqlCon = new SqlConnection(ConnectionString.dbConnect);
                SqlCommand dbCmd = new SqlCommand(StoredProcedure, SqlCon);
                dbCmd.CommandType = CommandType.StoredProcedure;

                if (TicketNumber.Count == 1)
                {
                    dbCmd.Parameters.Add(new SqlParameter("@ProcNo", SqlDbType.Int)).Value = Count;
                    dbCmd.Parameters.Add(new SqlParameter("@Pax1", SqlDbType.VarChar, 50)).Value = TicketNumber[0].ToString();
                    dbCmd.Parameters.Add(new SqlParameter("@Pax1id", SqlDbType.Int)).Value = int.Parse(PassengerID[0].ToString());

                    try
                    {
                        SqlCon.Open();
                        i = dbCmd.ExecuteNonQuery();
                        SqlCon.Close();
                    }
                    catch (Exception ex)
                    {
                        dbCommon.Logger.dbLogg(PassengerID.ToString(), BookingRef, "UpdateTicketNumber", "dbCallCenter", "Company_Pax_Detail_Airline_Update_TicketNumber", strTktNumber, ex.Message);
                    }
                    finally
                    {
                        if (SqlCon.State == ConnectionState.Open)
                        {
                            SqlCon.Close();
                        }
                    }
                }
                else if (TicketNumber.Count == 2)
                {
                    dbCmd.Parameters.Add(new SqlParameter("@ProcNo", SqlDbType.Int)).Value = Count;
                    dbCmd.Parameters.Add(new SqlParameter("@Pax1", SqlDbType.VarChar, 50)).Value = TicketNumber[0].ToString();
                    dbCmd.Parameters.Add(new SqlParameter("@Pax2", SqlDbType.VarChar, 50)).Value = TicketNumber[1].ToString();

                    dbCmd.Parameters.Add(new SqlParameter("@Pax1id", SqlDbType.Int)).Value = int.Parse(PassengerID[0].ToString());
                    dbCmd.Parameters.Add(new SqlParameter("@Pax2id", SqlDbType.Int)).Value = int.Parse(PassengerID[1].ToString());

                    try
                    {
                        SqlCon.Open();
                        i = dbCmd.ExecuteNonQuery();
                        SqlCon.Close();
                    }
                    catch (Exception ex)
                    {
                        dbCommon.Logger.dbLogg(PassengerID.ToString(), BookingRef, "UpdateTicketNumber", "dbCallCenter", "Company_Pax_Detail_Airline_Update_TicketNumber", strTktNumber, ex.Message);
                    }
                    finally
                    {
                        if (SqlCon.State == ConnectionState.Open)
                        {
                            SqlCon.Close();
                        }
                    }
                }
                else if (TicketNumber.Count == 3)
                {
                    dbCmd.Parameters.Add(new SqlParameter("@ProcNo", SqlDbType.Int)).Value = Count;
                    dbCmd.Parameters.Add(new SqlParameter("@Pax1", SqlDbType.VarChar, 50)).Value = TicketNumber[0].ToString();
                    dbCmd.Parameters.Add(new SqlParameter("@Pax2", SqlDbType.VarChar, 50)).Value = TicketNumber[1].ToString();
                    dbCmd.Parameters.Add(new SqlParameter("@Pax3", SqlDbType.VarChar, 50)).Value = TicketNumber[2].ToString();

                    dbCmd.Parameters.Add(new SqlParameter("@Pax1id", SqlDbType.Int)).Value = int.Parse(PassengerID[0].ToString());
                    dbCmd.Parameters.Add(new SqlParameter("@Pax2id", SqlDbType.Int)).Value = int.Parse(PassengerID[1].ToString());
                    dbCmd.Parameters.Add(new SqlParameter("@Pax3id", SqlDbType.Int)).Value = int.Parse(PassengerID[2].ToString());


                    try
                    {
                        SqlCon.Open();
                        i = dbCmd.ExecuteNonQuery();
                        SqlCon.Close();
                    }
                    catch (Exception ex)
                    {
                        dbCommon.Logger.dbLogg(PassengerID.ToString(), BookingRef, "UpdateTicketNumber", "dbCallCenter", "Company_Pax_Detail_Airline_Update_TicketNumber", strTktNumber, ex.Message);
                    }
                    finally
                    {
                        if (SqlCon.State == ConnectionState.Open)
                        {
                            SqlCon.Close();
                        }
                    }
                }
                else if (TicketNumber.Count == 4)
                {
                    dbCmd.Parameters.Add(new SqlParameter("@ProcNo", SqlDbType.Int)).Value = Count;
                    dbCmd.Parameters.Add(new SqlParameter("@Pax1", SqlDbType.VarChar, 50)).Value = TicketNumber[0].ToString();
                    dbCmd.Parameters.Add(new SqlParameter("@Pax2", SqlDbType.VarChar, 50)).Value = TicketNumber[1].ToString();
                    dbCmd.Parameters.Add(new SqlParameter("@Pax3", SqlDbType.VarChar, 50)).Value = TicketNumber[2].ToString();
                    dbCmd.Parameters.Add(new SqlParameter("@Pax4", SqlDbType.VarChar, 50)).Value = TicketNumber[3].ToString();

                    dbCmd.Parameters.Add(new SqlParameter("@Pax1id", SqlDbType.Int)).Value = int.Parse(PassengerID[0].ToString());
                    dbCmd.Parameters.Add(new SqlParameter("@Pax2id", SqlDbType.Int)).Value = int.Parse(PassengerID[1].ToString());
                    dbCmd.Parameters.Add(new SqlParameter("@Pax3id", SqlDbType.Int)).Value = int.Parse(PassengerID[2].ToString());
                    dbCmd.Parameters.Add(new SqlParameter("@Pax4id", SqlDbType.Int)).Value = int.Parse(PassengerID[3].ToString());

                    try
                    {
                        SqlCon.Open();
                        i = dbCmd.ExecuteNonQuery();
                        SqlCon.Close();
                    }
                    catch (Exception ex)
                    {
                        dbCommon.Logger.dbLogg(PassengerID.ToString(), BookingRef, "UpdateTicketNumber", "dbCallCenter", "Company_Pax_Detail_Airline_Update_TicketNumber", strTktNumber, ex.Message);
                    }
                    finally
                    {
                        if (SqlCon.State == ConnectionState.Open)
                        {
                            SqlCon.Close();
                        }
                    }
                }
                else if (TicketNumber.Count == 5)
                {
                    dbCmd.Parameters.Add(new SqlParameter("@ProcNo", SqlDbType.Int)).Value = Count;
                    dbCmd.Parameters.Add(new SqlParameter("@Pax1", SqlDbType.VarChar, 50)).Value = TicketNumber[0].ToString();
                    dbCmd.Parameters.Add(new SqlParameter("@Pax2", SqlDbType.VarChar, 50)).Value = TicketNumber[1].ToString();
                    dbCmd.Parameters.Add(new SqlParameter("@Pax3", SqlDbType.VarChar, 50)).Value = TicketNumber[2].ToString();
                    dbCmd.Parameters.Add(new SqlParameter("@Pax4", SqlDbType.VarChar, 50)).Value = TicketNumber[3].ToString();
                    dbCmd.Parameters.Add(new SqlParameter("@Pax5", SqlDbType.VarChar, 50)).Value = TicketNumber[4].ToString();

                    dbCmd.Parameters.Add(new SqlParameter("@Pax1id", SqlDbType.Int)).Value = int.Parse(PassengerID[0].ToString());
                    dbCmd.Parameters.Add(new SqlParameter("@Pax2id", SqlDbType.Int)).Value = int.Parse(PassengerID[1].ToString());
                    dbCmd.Parameters.Add(new SqlParameter("@Pax3id", SqlDbType.Int)).Value = int.Parse(PassengerID[2].ToString());
                    dbCmd.Parameters.Add(new SqlParameter("@Pax4id", SqlDbType.Int)).Value = int.Parse(PassengerID[3].ToString());
                    dbCmd.Parameters.Add(new SqlParameter("@Pax5id", SqlDbType.Int)).Value = int.Parse(PassengerID[4].ToString());

                    try
                    {
                        SqlCon.Open();
                        i = dbCmd.ExecuteNonQuery();
                        SqlCon.Close();
                    }
                    catch (Exception ex)
                    {
                        dbCommon.Logger.dbLogg(PassengerID.ToString(), BookingRef, "UpdateTicketNumber", "dbCallCenter", "Company_Pax_Detail_Airline_Update_TicketNumber", strTktNumber, ex.Message);
                    }
                    finally
                    {
                        if (SqlCon.State == ConnectionState.Open)
                        {
                            SqlCon.Close();
                        }
                    }
                }
                else if (TicketNumber.Count == 6)
                {
                    dbCmd.Parameters.Add(new SqlParameter("@ProcNo", SqlDbType.Int)).Value = Count;
                    dbCmd.Parameters.Add(new SqlParameter("@Pax1", SqlDbType.VarChar, 50)).Value = TicketNumber[0].ToString();
                    dbCmd.Parameters.Add(new SqlParameter("@Pax2", SqlDbType.VarChar, 50)).Value = TicketNumber[1].ToString();
                    dbCmd.Parameters.Add(new SqlParameter("@Pax3", SqlDbType.VarChar, 50)).Value = TicketNumber[2].ToString();
                    dbCmd.Parameters.Add(new SqlParameter("@Pax4", SqlDbType.VarChar, 50)).Value = TicketNumber[3].ToString();
                    dbCmd.Parameters.Add(new SqlParameter("@Pax5", SqlDbType.VarChar, 50)).Value = TicketNumber[4].ToString();
                    dbCmd.Parameters.Add(new SqlParameter("@Pax6", SqlDbType.VarChar, 50)).Value = TicketNumber[5].ToString();

                    dbCmd.Parameters.Add(new SqlParameter("@Pax1id", SqlDbType.Int)).Value = int.Parse(PassengerID[0].ToString());
                    dbCmd.Parameters.Add(new SqlParameter("@Pax2id", SqlDbType.Int)).Value = int.Parse(PassengerID[1].ToString());
                    dbCmd.Parameters.Add(new SqlParameter("@Pax3id", SqlDbType.Int)).Value = int.Parse(PassengerID[2].ToString());
                    dbCmd.Parameters.Add(new SqlParameter("@Pax4id", SqlDbType.Int)).Value = int.Parse(PassengerID[3].ToString());
                    dbCmd.Parameters.Add(new SqlParameter("@Pax5id", SqlDbType.Int)).Value = int.Parse(PassengerID[4].ToString());
                    dbCmd.Parameters.Add(new SqlParameter("@Pax6id", SqlDbType.Int)).Value = int.Parse(PassengerID[5].ToString());

                    try
                    {
                        SqlCon.Open();
                        i = dbCmd.ExecuteNonQuery();
                        SqlCon.Close();
                    }
                    catch (Exception ex)
                    {
                        dbCommon.Logger.dbLogg(PassengerID.ToString(), BookingRef, "UpdateTicketNumber", "dbCallCenter", "Company_Pax_Detail_Airline_Update_TicketNumber", strTktNumber, ex.Message);
                    }
                    finally
                    {
                        if (SqlCon.State == ConnectionState.Open)
                        {
                            SqlCon.Close();
                        }
                    }

                }
                else if (TicketNumber.Count == 7)
                {
                    dbCmd.Parameters.Add(new SqlParameter("@ProcNo", SqlDbType.Int)).Value = Count;
                    dbCmd.Parameters.Add(new SqlParameter("@Pax1", SqlDbType.VarChar, 50)).Value = TicketNumber[0].ToString();
                    dbCmd.Parameters.Add(new SqlParameter("@Pax2", SqlDbType.VarChar, 50)).Value = TicketNumber[1].ToString();
                    dbCmd.Parameters.Add(new SqlParameter("@Pax3", SqlDbType.VarChar, 50)).Value = TicketNumber[2].ToString();
                    dbCmd.Parameters.Add(new SqlParameter("@Pax4", SqlDbType.VarChar, 50)).Value = TicketNumber[3].ToString();
                    dbCmd.Parameters.Add(new SqlParameter("@Pax5", SqlDbType.VarChar, 50)).Value = TicketNumber[4].ToString();
                    dbCmd.Parameters.Add(new SqlParameter("@Pax6", SqlDbType.VarChar, 50)).Value = TicketNumber[5].ToString();
                    dbCmd.Parameters.Add(new SqlParameter("@Pax7", SqlDbType.VarChar, 50)).Value = TicketNumber[6].ToString();

                    dbCmd.Parameters.Add(new SqlParameter("@Pax1id", SqlDbType.Int)).Value = int.Parse(PassengerID[0].ToString());
                    dbCmd.Parameters.Add(new SqlParameter("@Pax2id", SqlDbType.Int)).Value = int.Parse(PassengerID[1].ToString());
                    dbCmd.Parameters.Add(new SqlParameter("@Pax3id", SqlDbType.Int)).Value = int.Parse(PassengerID[2].ToString());
                    dbCmd.Parameters.Add(new SqlParameter("@Pax4id", SqlDbType.Int)).Value = int.Parse(PassengerID[3].ToString());
                    dbCmd.Parameters.Add(new SqlParameter("@Pax5id", SqlDbType.Int)).Value = int.Parse(PassengerID[4].ToString());
                    dbCmd.Parameters.Add(new SqlParameter("@Pax6id", SqlDbType.Int)).Value = int.Parse(PassengerID[5].ToString());
                    dbCmd.Parameters.Add(new SqlParameter("@Pax7id", SqlDbType.Int)).Value = int.Parse(PassengerID[6].ToString());

                    try
                    {
                        SqlCon.Open();
                        i = dbCmd.ExecuteNonQuery();
                        SqlCon.Close();
                    }
                    catch (Exception ex)
                    {
                        dbCommon.Logger.dbLogg(PassengerID.ToString(), BookingRef, "UpdateTicketNumber", "dbCallCenter", "Company_Pax_Detail_Airline_Update_TicketNumber", strTktNumber, ex.Message);
                    }
                    finally
                    {
                        if (SqlCon.State == ConnectionState.Open)
                        {
                            SqlCon.Close();
                        }
                    }
                }
                else if (TicketNumber.Count == 8)
                {
                    dbCmd.Parameters.Add(new SqlParameter("@ProcNo", SqlDbType.Int)).Value = Count;
                    dbCmd.Parameters.Add(new SqlParameter("@Pax1", SqlDbType.VarChar, 50)).Value = TicketNumber[0].ToString();
                    dbCmd.Parameters.Add(new SqlParameter("@Pax2", SqlDbType.VarChar, 50)).Value = TicketNumber[1].ToString();
                    dbCmd.Parameters.Add(new SqlParameter("@Pax3", SqlDbType.VarChar, 50)).Value = TicketNumber[2].ToString();
                    dbCmd.Parameters.Add(new SqlParameter("@Pax4", SqlDbType.VarChar, 50)).Value = TicketNumber[3].ToString();
                    dbCmd.Parameters.Add(new SqlParameter("@Pax5", SqlDbType.VarChar, 50)).Value = TicketNumber[4].ToString();
                    dbCmd.Parameters.Add(new SqlParameter("@Pax6", SqlDbType.VarChar, 50)).Value = TicketNumber[5].ToString();
                    dbCmd.Parameters.Add(new SqlParameter("@Pax7", SqlDbType.VarChar, 50)).Value = TicketNumber[6].ToString();
                    dbCmd.Parameters.Add(new SqlParameter("@Pax8", SqlDbType.VarChar, 50)).Value = TicketNumber[7].ToString();


                    dbCmd.Parameters.Add(new SqlParameter("@Pax1id", SqlDbType.Int)).Value = int.Parse(PassengerID[0].ToString());
                    dbCmd.Parameters.Add(new SqlParameter("@Pax2id", SqlDbType.Int)).Value = int.Parse(PassengerID[1].ToString());
                    dbCmd.Parameters.Add(new SqlParameter("@Pax3id", SqlDbType.Int)).Value = int.Parse(PassengerID[2].ToString());
                    dbCmd.Parameters.Add(new SqlParameter("@Pax4id", SqlDbType.Int)).Value = int.Parse(PassengerID[3].ToString());
                    dbCmd.Parameters.Add(new SqlParameter("@Pax5id", SqlDbType.Int)).Value = int.Parse(PassengerID[4].ToString());
                    dbCmd.Parameters.Add(new SqlParameter("@Pax6id", SqlDbType.Int)).Value = int.Parse(PassengerID[5].ToString());
                    dbCmd.Parameters.Add(new SqlParameter("@Pax7id", SqlDbType.Int)).Value = int.Parse(PassengerID[6].ToString());
                    dbCmd.Parameters.Add(new SqlParameter("@Pax8id", SqlDbType.Int)).Value = int.Parse(PassengerID[7].ToString());


                    try
                    {
                        SqlCon.Open();
                        i = dbCmd.ExecuteNonQuery();
                        SqlCon.Close();
                    }
                    catch (Exception ex)
                    {
                        dbCommon.Logger.dbLogg(PassengerID.ToString(), BookingRef, "UpdateTicketNumber", "dbCallCenter", "Company_Pax_Detail_Airline_Update_TicketNumber", strTktNumber, ex.Message);
                    }
                    finally
                    {
                        if (SqlCon.State == ConnectionState.Open)
                        {
                            SqlCon.Close();
                        }
                    }
                }
                else if (TicketNumber.Count == 9)
                {
                    dbCmd.Parameters.Add(new SqlParameter("@ProcNo", SqlDbType.Int)).Value = Count;
                    dbCmd.Parameters.Add(new SqlParameter("@Pax1", SqlDbType.VarChar, 50)).Value = TicketNumber[0].ToString();
                    dbCmd.Parameters.Add(new SqlParameter("@Pax2", SqlDbType.VarChar, 50)).Value = TicketNumber[1].ToString();
                    dbCmd.Parameters.Add(new SqlParameter("@Pax3", SqlDbType.VarChar, 50)).Value = TicketNumber[2].ToString();
                    dbCmd.Parameters.Add(new SqlParameter("@Pax4", SqlDbType.VarChar, 50)).Value = TicketNumber[3].ToString();
                    dbCmd.Parameters.Add(new SqlParameter("@Pax5", SqlDbType.VarChar, 50)).Value = TicketNumber[4].ToString();
                    dbCmd.Parameters.Add(new SqlParameter("@Pax6", SqlDbType.VarChar, 50)).Value = TicketNumber[5].ToString();
                    dbCmd.Parameters.Add(new SqlParameter("@Pax7", SqlDbType.VarChar, 50)).Value = TicketNumber[6].ToString();
                    dbCmd.Parameters.Add(new SqlParameter("@Pax8", SqlDbType.VarChar, 50)).Value = TicketNumber[7].ToString();
                    dbCmd.Parameters.Add(new SqlParameter("@Pax9", SqlDbType.VarChar, 50)).Value = TicketNumber[8].ToString();

                    dbCmd.Parameters.Add(new SqlParameter("@Pax1id", SqlDbType.Int)).Value = int.Parse(PassengerID[0].ToString());
                    dbCmd.Parameters.Add(new SqlParameter("@Pax2id", SqlDbType.Int)).Value = int.Parse(PassengerID[1].ToString());
                    dbCmd.Parameters.Add(new SqlParameter("@Pax3id", SqlDbType.Int)).Value = int.Parse(PassengerID[2].ToString());
                    dbCmd.Parameters.Add(new SqlParameter("@Pax4id", SqlDbType.Int)).Value = int.Parse(PassengerID[3].ToString());
                    dbCmd.Parameters.Add(new SqlParameter("@Pax5id", SqlDbType.Int)).Value = int.Parse(PassengerID[4].ToString());
                    dbCmd.Parameters.Add(new SqlParameter("@Pax6id", SqlDbType.Int)).Value = int.Parse(PassengerID[5].ToString());
                    dbCmd.Parameters.Add(new SqlParameter("@Pax7id", SqlDbType.Int)).Value = int.Parse(PassengerID[6].ToString());
                    dbCmd.Parameters.Add(new SqlParameter("@Pax8id", SqlDbType.Int)).Value = int.Parse(PassengerID[7].ToString());
                    dbCmd.Parameters.Add(new SqlParameter("@Pax9id", SqlDbType.Int)).Value = int.Parse(PassengerID[8].ToString());


                    try
                    {
                        SqlCon.Open();
                        i = dbCmd.ExecuteNonQuery();
                        SqlCon.Close();
                    }
                    catch (Exception ex)
                    {
                        dbCommon.Logger.dbLogg(PassengerID.ToString(), BookingRef, "UpdateTicketNumber", "dbCallCenter", "Company_Pax_Detail_Airline_Update_TicketNumber", strTktNumber, ex.Message);
                    }
                    finally
                    {
                        if (SqlCon.State == ConnectionState.Open)
                        {
                            SqlCon.Close();
                        }
                    }
                }
                else if (TicketNumber.Count == 10)
                {
                    dbCmd.Parameters.Add(new SqlParameter("@ProcNo", SqlDbType.Int)).Value = Count;
                    dbCmd.Parameters.Add(new SqlParameter("@Pax1", SqlDbType.VarChar, 50)).Value = TicketNumber[0].ToString();
                    dbCmd.Parameters.Add(new SqlParameter("@Pax2", SqlDbType.VarChar, 50)).Value = TicketNumber[1].ToString();
                    dbCmd.Parameters.Add(new SqlParameter("@Pax3", SqlDbType.VarChar, 50)).Value = TicketNumber[2].ToString();
                    dbCmd.Parameters.Add(new SqlParameter("@Pax4", SqlDbType.VarChar, 50)).Value = TicketNumber[3].ToString();
                    dbCmd.Parameters.Add(new SqlParameter("@Pax5", SqlDbType.VarChar, 50)).Value = TicketNumber[4].ToString();
                    dbCmd.Parameters.Add(new SqlParameter("@Pax6", SqlDbType.VarChar, 50)).Value = TicketNumber[5].ToString();
                    dbCmd.Parameters.Add(new SqlParameter("@Pax7", SqlDbType.VarChar, 50)).Value = TicketNumber[6].ToString();
                    dbCmd.Parameters.Add(new SqlParameter("@Pax8", SqlDbType.VarChar, 50)).Value = TicketNumber[7].ToString();
                    dbCmd.Parameters.Add(new SqlParameter("@Pax9", SqlDbType.VarChar, 50)).Value = TicketNumber[8].ToString();
                    dbCmd.Parameters.Add(new SqlParameter("@Pax10", SqlDbType.VarChar, 50)).Value = TicketNumber[9].ToString();

                    dbCmd.Parameters.Add(new SqlParameter("@Pax1id", SqlDbType.Int)).Value = int.Parse(PassengerID[0].ToString());
                    dbCmd.Parameters.Add(new SqlParameter("@Pax2id", SqlDbType.Int)).Value = int.Parse(PassengerID[1].ToString());
                    dbCmd.Parameters.Add(new SqlParameter("@Pax3id", SqlDbType.Int)).Value = int.Parse(PassengerID[2].ToString());
                    dbCmd.Parameters.Add(new SqlParameter("@Pax4id", SqlDbType.Int)).Value = int.Parse(PassengerID[3].ToString());
                    dbCmd.Parameters.Add(new SqlParameter("@Pax5id", SqlDbType.Int)).Value = int.Parse(PassengerID[4].ToString());
                    dbCmd.Parameters.Add(new SqlParameter("@Pax6id", SqlDbType.Int)).Value = int.Parse(PassengerID[5].ToString());
                    dbCmd.Parameters.Add(new SqlParameter("@Pax7id", SqlDbType.Int)).Value = int.Parse(PassengerID[6].ToString());
                    dbCmd.Parameters.Add(new SqlParameter("@Pax8id", SqlDbType.Int)).Value = int.Parse(PassengerID[7].ToString());
                    dbCmd.Parameters.Add(new SqlParameter("@Pax9id", SqlDbType.Int)).Value = int.Parse(PassengerID[8].ToString());
                    dbCmd.Parameters.Add(new SqlParameter("@Pax10id", SqlDbType.Int)).Value = int.Parse(PassengerID[9].ToString());

                    try
                    {
                        SqlCon.Open();
                        i = dbCmd.ExecuteNonQuery();
                        SqlCon.Close();
                    }
                    catch (Exception ex)
                    {
                        dbCommon.Logger.dbLogg(PassengerID.ToString(), BookingRef, "UpdateTicketNumber", "dbCallCenter", "Company_Pax_Detail_Airline_Update_TicketNumber", strTktNumber, ex.Message);
                    }
                    finally
                    {
                        if (SqlCon.State == ConnectionState.Open)
                        {
                            SqlCon.Close();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                dbCommon.Logger.dbLogg("", BookingRef, "UpdateTicketNumber", "dbCallCenter", "Company_Pax_Detail_Airline_Update_TicketNumber", strTktNumber, ex.Message);
            }

            if (i > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        private static ArrayList getPassengerID(Int32 BookingRef)
        {
            ArrayList RowId = new ArrayList();

            try
            {
                using (SqlConnection dbCon = new SqlConnection(ConnectionString.dbConnect))
                {
                    SqlCommand dbCmd = new SqlCommand("Company_Pax_Detail_Airline_Update_TicketNumber", dbCon);
                    dbCmd.CommandType = CommandType.StoredProcedure;

                    dbCmd.Parameters.Add(@"ProcNo", SqlDbType.Int).Value = 0;
                    dbCmd.Parameters.Add(@"BookingRef", SqlDbType.Int).Value = BookingRef;

                    dbCon.Open();
                    SqlDataReader Dr = dbCmd.ExecuteReader();
                    while (Dr.Read())
                    {
                        RowId.Add(Dr["ID"].ToString());
                    }
                    dbCon.Close();
                }
            }
            catch (Exception ex)
            {
                dbCommon.Logger.dbLogg("", BookingRef, "getPassengerID", "dbCallCenter", "Company_Pax_Detail_Airline_Update_TicketNumber", "", ex.Message);
            }
            finally
            {

            }

            return RowId;
        }
    }
}
