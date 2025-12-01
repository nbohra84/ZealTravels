using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Web;

namespace ZealTravel.Infrastructure.DBCommon
{
    public class db_Company
    {
        public DataTable getSubAgencyBalanceList(string CompanyID)
        {
            DataTable dtCompany = new DataTable();
            //try
            //{
            //    using (SqlConnection connection = new SqlConnection(ConnectionString.dbConnect))
            //    {
            //        SqlCommand cmd = new SqlCommand("Company_Balance_Transaction_Detail_Proc", connection);
            //        cmd.CommandType = CommandType.StoredProcedure;
            //        cmd.CommandTimeout = 10;
            //        cmd.Parameters.Add(@"ProcNo", SqlDbType.Int).Value = 5;
            //        cmd.Parameters.Add(@"CompanyID", SqlDbType.VarChar).Value = CompanyID;

            //        SqlDataAdapter da = new SqlDataAdapter(cmd);
            //        da.Fill(dtCompany);
            //    }
            //}
            //catch (Exception ex)
            //{
            //    DBCommon.Logger.dbLogg(CompanyID, 0, "clsDB", "getSubAgencyBalanceList", "", "Company_Balance_Transaction_Detail_Proc", ex.Message);
            //}

            return dtCompany;
        }
        public DataSet Get_Company_Register(string CompanyID)
        {
            DataSet CompanyDT = new DataSet();
            //try
            //{

            //    using (SqlConnection dbCon = new SqlConnection(ConnectionString.dbConnect))
            //    {
            //        SqlCommand dbCmd = new SqlCommand("Company_Register_Proc", dbCon);
            //        dbCmd.CommandType = CommandType.StoredProcedure;
            //        dbCmd.Parameters.Add(@"ProcNo", SqlDbType.Int).Value = 2;
            //        dbCmd.Parameters.Add(@"CompanyID", SqlDbType.VarChar).Value = CompanyID;

            //        SqlDataAdapter dbDa = new SqlDataAdapter(dbCmd);
            //        dbDa.Fill(CompanyDT);
            //        if (CompanyDT != null)
            //        {
            //            CompanyDT.Tables["Table"].TableName = "CompanyDetail";
            //            CompanyDT.Tables["Table1"].TableName = "CompanyProductDetail";
            //        }
            //    }
            //}
            //catch (Exception ex)
            //{
            //    DBCommon.Logger.dbLogg(CompanyID, 0, "Get_Company_Register", "Company_Register_Proc", "db_Company", "", ex.Message);
            //}
            //finally
            //{

            //}

            return CompanyDT;
        }
        public DataTable GetCompanyDetail(string Email)
        {
            DataTable CompanyDT = new DataTable();
            //try
            //{

            //    using (SqlConnection dbCon = new SqlConnection(ConnectionString.dbConnect))
            //    {
            //        SqlCommand dbCmd = new SqlCommand("Company_Register_Proc", dbCon);
            //        dbCmd.CommandType = CommandType.StoredProcedure;
            //        dbCmd.Parameters.Add(@"ProcNo", SqlDbType.Int).Value = 19;
            //        dbCmd.Parameters.Add(@"Email", SqlDbType.VarChar).Value = Email;

            //        SqlDataAdapter dbDa = new SqlDataAdapter(dbCmd);
            //        dbDa.Fill(CompanyDT);
            //        if (CompanyDT != null)
            //        {
            //            CompanyDT.TableName = "CompanyDetail";
            //        }
            //    }
            //}
            //catch (Exception ex)
            //{
            //    DBCommon.Logger.dbLogg("", 0, "GetCompanyDetail", "Company_Register_Proc", "db_Company", Email, ex.Message);
            //}
            //finally
            //{

            //}

            return CompanyDT;
        }
        public DataTable GetCompanyDetailbyCompanyID(string CompanyID)
        {
            DataTable CompanyDT = new DataTable();
            //try
            //{

            //    using (SqlConnection dbCon = new SqlConnection(ConnectionString.dbConnect))
            //    {
            //        SqlCommand dbCmd = new SqlCommand("Company_Register_Proc", dbCon);
            //        dbCmd.CommandType = CommandType.StoredProcedure;
            //        dbCmd.Parameters.Add(@"ProcNo", SqlDbType.Int).Value = 21;
            //        dbCmd.Parameters.Add(@"CompanyID", SqlDbType.VarChar).Value = CompanyID;

            //        SqlDataAdapter dbDa = new SqlDataAdapter(dbCmd);
            //        dbDa.Fill(CompanyDT);
            //        if (CompanyDT != null)
            //        {
            //            CompanyDT.TableName = "CompanyDetail";
            //        }
            //    }
            //}
            //catch (Exception ex)
            //{
            //    DBCommon.Logger.dbLogg(CompanyID, 0, "GetCompanyDetailbyCompanyID", "Company_Register_Proc", "db_Company", "", ex.Message);
            //}
            //finally
            //{

            //}

            return CompanyDT;
        }
        public DataTable Get_CompanyDetail(Int32 AccountID)
        {
            DataTable CompanyDT = new DataTable();
            //try
            //{

            //    using (SqlConnection dbCon = new SqlConnection(ConnectionString.dbConnect))
            //    {
            //        SqlCommand dbCmd = new SqlCommand("Company_Register_Proc", dbCon);
            //        dbCmd.CommandType = CommandType.StoredProcedure;
            //        dbCmd.Parameters.Add(@"ProcNo", SqlDbType.Int).Value = 33;
            //        dbCmd.Parameters.Add(@"AccountID", SqlDbType.VarChar).Value = AccountID;

            //        SqlDataAdapter dbDa = new SqlDataAdapter(dbCmd);
            //        dbDa.Fill(CompanyDT);
            //        if (CompanyDT != null)
            //        {
            //            CompanyDT.TableName = "CompanyDetail";
            //        }
            //    }
            //}
            //catch (Exception ex)
            //{
            //    DBCommon.Logger.dbLogg("", AccountID, "Get_CompanyDetail", "Company_Register_Proc", "db_Company", "", ex.Message);
            //}
            //finally
            //{

            //}

            return CompanyDT;
        }

        public static DataTable getCompanyDetailByHost(string Host)
        {
            DataTable dtCompany = new DataTable();
            //try
            //{
            //    using (SqlConnection connection = new SqlConnection(ConnectionString.dbConnect))
            //    {
            //        SqlCommand cmd = new SqlCommand("Whitelabel_Detail_Proc", connection);
            //        cmd.CommandType = CommandType.StoredProcedure;
            //        cmd.CommandTimeout = 10;
            //        cmd.Parameters.Add(@"ProcNo", SqlDbType.Int).Value = 1;
            //        cmd.Parameters.Add(@"Host", SqlDbType.VarChar).Value = Host;

            //        SqlDataAdapter da = new SqlDataAdapter(cmd);
            //        da.Fill(dtCompany);
            //    }
            //}
            //catch (Exception ex)
            //{
            //    DBCommon.Logger.dbLogg("dbCallCenter", 0, "db_Company", "getCompanyDetailByHost", "", "", ex.Message);
            //}

            return dtCompany;
        }
        public bool BookingStatus(Int32 BookingRef, string CompanyID)
        {
            bool Status = false;

            //try
            //{
            //    using (SqlConnection connection = new SqlConnection(ConnectionString.dbConnect))
            //    {
            //        SqlCommand cmd = new SqlCommand("Company_Flight_Detail_Airline_Proc", connection);
            //        cmd.CommandType = CommandType.StoredProcedure;
            //        cmd.CommandTimeout = 10;
            //        cmd.Parameters.Add(@"ProcNo", SqlDbType.Int).Value = 11;
            //        cmd.Parameters.Add(@"BookingRef", SqlDbType.Int).Value = BookingRef;
            //        cmd.Parameters.Add(@"CompanyID", SqlDbType.VarChar).Value = CompanyID;

            //        connection.Open();
            //        int i = (int)cmd.ExecuteScalar();
            //        connection.Close();

            //        if (i > 0)
            //        {
            //            Status = true;
            //        }
            //    }
            //}
            //catch (Exception ex)
            //{
            //    DBCommon.Logger.dbLogg("dbCallCenter", BookingRef, "db_Company", "BookingStatus", "", "", ex.Message);
            //}

            return Status;

        }
        public static string getAdminid_from_Host()
        {
            string AdminID = string.Empty;
            //try
            //{
            //    try
            //    {
            //        string Host = HttpContext.Current.Request.Url.Host;
            //        if (Host.IndexOf("www.") != -1)
            //        {
            //            Host = Host.Replace("www.", "").Trim();
            //        }
            //        using (SqlConnection connection = new SqlConnection(ConnectionString.dbConnect))
            //        {
            //            SqlCommand cmd = new SqlCommand("Whitelabel_Detail_Proc", connection);
            //            cmd.CommandType = CommandType.StoredProcedure;
            //            cmd.CommandTimeout = 10;
            //            cmd.Parameters.Add(@"ProcNo", SqlDbType.Int).Value = 2;
            //            cmd.Parameters.Add(@"Host", SqlDbType.VarChar).Value = Host;

            //            connection.Open();
            //            AdminID = cmd.ExecuteScalar().ToString();
            //            connection.Close();
            //        }
            //    }
            //    catch (Exception ex)
            //    {
            //        DBCommon.Logger.dbLogg("", 0, "db_Company", "getAdminid_from_Host", "", "", ex.Message);
            //    }
            //}
            //catch
            //{

            //}

            return AdminID;
        }
        public static string getEmail_from_Host(string Host)
        {
            string AdminID = string.Empty;
            //try
            //{
            //    try
            //    {
            //        using (SqlConnection connection = new SqlConnection(ConnectionString.dbConnect))
            //        {
            //            SqlCommand cmd = new SqlCommand("Whitelabel_Detail_Proc", connection);
            //            cmd.CommandType = CommandType.StoredProcedure;
            //            cmd.CommandTimeout = 10;
            //            cmd.Parameters.Add(@"ProcNo", SqlDbType.Int).Value = 3;
            //            cmd.Parameters.Add(@"Host", SqlDbType.VarChar).Value = Host;

            //            connection.Open();
            //            AdminID = cmd.ExecuteScalar().ToString();
            //            connection.Close();
            //        }
            //    }
            //    catch (Exception ex)
            //    {
            //        DBCommon.Logger.dbLogg("", 0, "", "getAdminid_from_Host", Host, "db_Company", ex.Message);
            //    }
            //}
            //catch
            //{

            //}

            return AdminID;
        }

        public static DataTable getCompanyDetailbyemail(string Email)
        {
            DataTable dtCompany = new DataTable();

            //try
            //{
            //    using (SqlConnection connection = new SqlConnection(ConnectionString.dbConnect))
            //    {
            //        SqlCommand cmd = new SqlCommand("Company_Register_Proc", connection);
            //        cmd.CommandType = CommandType.StoredProcedure;
            //        cmd.CommandTimeout = 10;
            //        cmd.Parameters.Add(@"ProcNo", SqlDbType.Int).Value = 25;
            //        cmd.Parameters.Add(@"Email", SqlDbType.VarChar).Value = Email;

            //        SqlDataAdapter da = new SqlDataAdapter(cmd);
            //        da.Fill(dtCompany);

            //    }
            //}
            //catch (Exception ex)
            //{
            //    DBCommon.Logger.dbLogg("dbCallCenter", 0, "", "getCompanyDetailbyemail", "db_Company", Email, ex.Message);
            //}

            return dtCompany;
        }
        public static Decimal Company_AvailableBalance(string CompanyID)
        {
            Decimal dAvailableBalance = 0;

            //try
            //{
            //    using (SqlConnection connection = new SqlConnection(ConnectionString.dbConnect))
            //    {
            //        SqlCommand cmd = new SqlCommand("Company_Balance_Transaction_Detail_Proc", connection);
            //        cmd.CommandType = CommandType.StoredProcedure;
            //        cmd.CommandTimeout = 10;
            //        cmd.Parameters.Add(@"ProcNo", SqlDbType.Int).Value = 1;
            //        cmd.Parameters.Add(@"CompanyID", SqlDbType.VarChar).Value = CompanyID;

            //        connection.Open();
            //        dAvailableBalance = (Decimal)cmd.ExecuteScalar();
            //        connection.Close();
            //    }
            //}
            //catch (Exception ex)
            //{
            //    DBCommon.Logger.dbLogg(CompanyID, 0, "Company_AvailableBalance", "db_Company_Balance_Transaction", "db_Company", "", ex.Message);
            //}

            return dAvailableBalance;
        }
        public static string getCompanyID(Int32 AccountID)
        {
            string CompanyID = string.Empty;

            //try
            //{
            //    using (SqlConnection connection = new SqlConnection(ConnectionString.dbConnect))
            //    {
            //        SqlCommand cmd = new SqlCommand("Company_Register_Proc", connection);
            //        cmd.CommandType = CommandType.StoredProcedure;
            //        cmd.CommandTimeout = 10;

            //        cmd.Parameters.Add(@"ProcNo", SqlDbType.Int).Value = 16;
            //        cmd.Parameters.Add(@"AccountID", SqlDbType.Int).Value = AccountID;

            //        connection.Open();
            //        CompanyID = cmd.ExecuteScalar().ToString();
            //        connection.Close();
            //    }
            //}
            //catch (Exception ex)
            //{
            //    DBCommon.Logger.dbLogg("", 0, "", "", "", "db_Company", ex.Message);
            //}
            return CompanyID;
        }
        public bool SET_Transaction_Detail(string SearchID, string SearchCriteria, string CompanyID, Int32 BookingRef, Decimal Debit, Decimal Credit, string PaymentType, string PaymentID, string UpdatedBy, string Remark, bool IsAirline, bool IsHotel)
        {
            bool bStatus = false;

            //try
            //{
            //    using (SqlConnection connection = new SqlConnection(ConnectionString.dbConnect))
            //    {
            //        SqlCommand cmd = new SqlCommand("Company_Transaction_Detail_Proc", connection);
            //        cmd.CommandType = CommandType.StoredProcedure;
            //        cmd.CommandTimeout = 10;

            //        cmd.Parameters.Add(@"ProcNo", SqlDbType.Int).Value = 1;
            //        cmd.Parameters.Add(@"CompanyID", SqlDbType.VarChar).Value = CompanyID;
            //        cmd.Parameters.Add(@"BookingRef", SqlDbType.Int).Value = BookingRef;
            //        cmd.Parameters.Add(@"Debit", SqlDbType.Decimal).Value = Debit;
            //        cmd.Parameters.Add(@"Credit", SqlDbType.Decimal).Value = Credit;
            //        cmd.Parameters.Add(@"PaymentType", SqlDbType.VarChar).Value = PaymentType;
            //        cmd.Parameters.Add(@"PaymentID", SqlDbType.VarChar).Value = PaymentID;
            //        cmd.Parameters.Add(@"UpdatedBy", SqlDbType.VarChar).Value = UpdatedBy;
            //        cmd.Parameters.Add(@"Remark", SqlDbType.VarChar).Value = Remark;
            //        cmd.Parameters.Add(@"IsAirline", SqlDbType.Bit).Value = IsAirline;
            //        cmd.Parameters.Add(@"IsHotel", SqlDbType.Bit).Value = IsHotel;

            //        connection.Open();
            //        int iRows = cmd.ExecuteNonQuery();
            //        connection.Close();

            //        if (iRows > 0)
            //        {
            //            bStatus = true;
            //        }
            //    }
            //}
            //catch (Exception ex)
            //{
            //    bStatus = false;
            //    DBCommon.Logger.dbLogg(CompanyID, BookingRef, "SET_Transaction_Detail", "db_Company", SearchCriteria, SearchID, "Debit- " + Debit.ToString() + "- Credit- " + Credit.ToString() + "- PaymentType-" + PaymentType + "- UpdatedBy-" + UpdatedBy + "-" + "- Remark-" + Remark + "-" + ex.Message);
            //}
            return bStatus;
        }
        public bool SET_Transaction_Detail(string SearchID, string SearchCriteria, string CompanyID, Int32 BookingRef, Decimal Debit, Decimal Credit, string PaymentType, string PaymentID, string UpdatedBy, string Remark, bool IsAirline, bool IsHotel, bool Status)
        {
            bool bStatus = false;

            //try
            //{
            //    using (SqlConnection connection = new SqlConnection(ConnectionString.dbConnect))
            //    {
            //        SqlCommand cmd = new SqlCommand("Company_Transaction_Detail_Proc", connection);
            //        cmd.CommandType = CommandType.StoredProcedure;
            //        cmd.CommandTimeout = 10;

            //        cmd.Parameters.Add(@"ProcNo", SqlDbType.Int).Value = 1;
            //        cmd.Parameters.Add(@"CompanyID", SqlDbType.VarChar).Value = CompanyID;
            //        cmd.Parameters.Add(@"BookingRef", SqlDbType.Int).Value = BookingRef;
            //        cmd.Parameters.Add(@"Debit", SqlDbType.Decimal).Value = Debit;
            //        cmd.Parameters.Add(@"Credit", SqlDbType.Decimal).Value = Credit;
            //        cmd.Parameters.Add(@"PaymentType", SqlDbType.VarChar).Value = PaymentType;
            //        cmd.Parameters.Add(@"PaymentID", SqlDbType.VarChar).Value = PaymentID;
            //        cmd.Parameters.Add(@"UpdatedBy", SqlDbType.VarChar).Value = UpdatedBy;
            //        cmd.Parameters.Add(@"Remark", SqlDbType.VarChar).Value = Remark;
            //        cmd.Parameters.Add(@"IsAirline", SqlDbType.Bit).Value = IsAirline;
            //        cmd.Parameters.Add(@"IsHotel", SqlDbType.Bit).Value = IsHotel;
            //        cmd.Parameters.Add(@"Status", SqlDbType.Bit).Value = Status;

            //        connection.Open();
            //        int iRows = cmd.ExecuteNonQuery();
            //        connection.Close();

            //        if (iRows > 0)
            //        {
            //            bStatus = true;
            //        }
            //    }
            //}
            //catch (Exception ex)
            //{
            //    bStatus = false;
            //    DBCommon.Logger.dbLogg(CompanyID, BookingRef, "SET_Transaction_Detail", "db_Company", SearchCriteria, SearchID, "Debit- " + Debit.ToString() + "- Credit- " + Credit.ToString() + "- PaymentType-" + PaymentType + "- UpdatedBy-" + UpdatedBy + "-" + "- Remark-" + Remark + "-" + ex.Message);
            //}
            return bStatus;
        }
        public bool SET_Transaction_Detail(string SearchID, string SearchCriteria, string CompanyID, Int32 BookingRef, Decimal Debit, Decimal Credit, string PaymentType, string PaymentID, string UpdatedBy, string Remark, bool IsAirline, bool IsHotel, bool Status, string EventID)
        {
            bool bStatus = false;

            //try
            //{
            //    using (SqlConnection connection = new SqlConnection(ConnectionString.dbConnect))
            //    {
            //        SqlCommand cmd = new SqlCommand("Company_Transaction_Detail_Proc", connection);
            //        cmd.CommandType = CommandType.StoredProcedure;
            //        cmd.CommandTimeout = 10;

            //        cmd.Parameters.Add(@"ProcNo", SqlDbType.Int).Value = 1;
            //        cmd.Parameters.Add(@"CompanyID", SqlDbType.VarChar).Value = CompanyID;
            //        cmd.Parameters.Add(@"BookingRef", SqlDbType.Int).Value = BookingRef;
            //        cmd.Parameters.Add(@"Debit", SqlDbType.Decimal).Value = Debit;
            //        cmd.Parameters.Add(@"Credit", SqlDbType.Decimal).Value = Credit;
            //        cmd.Parameters.Add(@"PaymentType", SqlDbType.VarChar).Value = PaymentType;
            //        cmd.Parameters.Add(@"PaymentID", SqlDbType.VarChar).Value = PaymentID;
            //        cmd.Parameters.Add(@"UpdatedBy", SqlDbType.VarChar).Value = UpdatedBy;
            //        cmd.Parameters.Add(@"Remark", SqlDbType.VarChar).Value = Remark;
            //        cmd.Parameters.Add(@"IsAirline", SqlDbType.Bit).Value = IsAirline;
            //        cmd.Parameters.Add(@"IsHotel", SqlDbType.Bit).Value = IsHotel;
            //        cmd.Parameters.Add(@"Status", SqlDbType.Bit).Value = Status;
            //        cmd.Parameters.Add(@"EventID", SqlDbType.VarChar).Value = EventID;

            //        connection.Open();
            //        int iRows = cmd.ExecuteNonQuery();
            //        connection.Close();

            //        if (iRows > 0)
            //        {
            //            bStatus = true;
            //        }
            //    }
            //}
            //catch (Exception ex)
            //{
            //    bStatus = false;
            //    DBCommon.Logger.dbLogg(CompanyID, BookingRef, "SET_Transaction_Detail", "db_Company", SearchCriteria, SearchID, "Debit- " + Debit.ToString() + "- Credit- " + Credit.ToString() + "- PaymentType-" + PaymentType + "- UpdatedBy-" + UpdatedBy + "-" + "- Remark-" + Remark + "-" + ex.Message);
            //}
            return bStatus;
        }
        public bool SET_Transaction_Detail(string SearchID, string SearchCriteria, string CompanyID, Int32 BookingRef, Decimal Debit, Decimal Credit, string PaymentType, string PaymentID, string UpdatedBy, string Remark, bool IsAirline, bool IsHotel, bool Status, string EventID, int PaxSegmentID)
        {
            bool bStatus = false;

            //try
            //{
            //    using (SqlConnection connection = new SqlConnection(ConnectionString.dbConnect))
            //    {
            //        SqlCommand cmd = new SqlCommand("Company_Transaction_Detail_Proc", connection);
            //        cmd.CommandType = CommandType.StoredProcedure;
            //        cmd.CommandTimeout = 10;

            //        cmd.Parameters.Add(@"ProcNo", SqlDbType.Int).Value = 1;
            //        cmd.Parameters.Add(@"CompanyID", SqlDbType.VarChar).Value = CompanyID;
            //        cmd.Parameters.Add(@"BookingRef", SqlDbType.Int).Value = BookingRef;
            //        cmd.Parameters.Add(@"PaxSegmentID", SqlDbType.Int).Value = PaxSegmentID;
            //        cmd.Parameters.Add(@"Debit", SqlDbType.Decimal).Value = Debit;
            //        cmd.Parameters.Add(@"Credit", SqlDbType.Decimal).Value = Credit;
            //        cmd.Parameters.Add(@"PaymentType", SqlDbType.VarChar).Value = PaymentType;
            //        cmd.Parameters.Add(@"PaymentID", SqlDbType.VarChar).Value = PaymentID;
            //        cmd.Parameters.Add(@"UpdatedBy", SqlDbType.VarChar).Value = UpdatedBy;
            //        cmd.Parameters.Add(@"Remark", SqlDbType.VarChar).Value = Remark;
            //        cmd.Parameters.Add(@"IsAirline", SqlDbType.Bit).Value = IsAirline;
            //        cmd.Parameters.Add(@"IsHotel", SqlDbType.Bit).Value = IsHotel;
            //        cmd.Parameters.Add(@"Status", SqlDbType.Bit).Value = Status;
            //        cmd.Parameters.Add(@"EventID", SqlDbType.VarChar).Value = EventID;

            //        connection.Open();
            //        int iRows = cmd.ExecuteNonQuery();
            //        connection.Close();

            //        if (iRows > 0)
            //        {
            //            bStatus = true;
            //        }
            //    }
            //}
            //catch (Exception ex)
            //{
            //    bStatus = false;
            //    DBCommon.Logger.dbLogg(CompanyID, BookingRef, "SET_Transaction_Detail", "db_Company", SearchCriteria, SearchID, "Debit- " + Debit.ToString() + "- Credit- " + Credit.ToString() + "- PaymentType-" + PaymentType + "- UpdatedBy-" + UpdatedBy + "-" + "- Remark-" + Remark + "-" + ex.Message);
            //}
            return bStatus;
        }
        public bool Verify_tkt_Balance(string CompanyID, Decimal Transaction_Amount)
        {
            bool Status = false;

            //try
            //{
            //    using (SqlConnection connection = new SqlConnection(ConnectionString.dbConnect))
            //    {
            //        SqlCommand cmd = new SqlCommand("Company_Balance_Transaction_Detail_Proc", connection);
            //        cmd.CommandType = CommandType.StoredProcedure;
            //        cmd.CommandTimeout = 10;
            //        cmd.Parameters.Add(@"ProcNo", SqlDbType.Int).Value = 3;
            //        cmd.Parameters.Add(@"CompanyID", SqlDbType.VarChar).Value = CompanyID;
            //        cmd.Parameters.Add(@"Transaction_Amount", SqlDbType.Decimal).Value = Transaction_Amount;

            //        connection.Open();
            //        int i = Convert.ToInt32(cmd.ExecuteScalar().ToString());
            //        connection.Close();

            //        if (i > 0)
            //        {
            //            Status = true;
            //        }
            //    }
            //}
            //catch (Exception ex)
            //{
            //    DBCommon.Logger.dbLogg(CompanyID, 0, "Verify_tkt_Balance", "db_Company", Transaction_Amount.ToString(), "db_Company", ex.Message);
            //}

            return Status;
        }
        public bool SET_GET_Company_Amount_Transaction(string SearchID, string SearchCriteria, string CompanyID, Decimal Transaction_Amount, string TransType, Int32 BookingRef, string UpdatedBy, string Remark)
        {
            bool bStatus = false;

            //try
            //{
            //    using (SqlConnection connection = new SqlConnection(ConnectionString.dbConnect))
            //    {
            //        SqlCommand cmd = new SqlCommand("Company_Balance_Transaction_Detail_Proc", connection);
            //        cmd.CommandType = CommandType.StoredProcedure;
            //        cmd.CommandTimeout = 10;

            //        cmd.Parameters.Add(@"ProcNo", SqlDbType.Int).Value = 2;
            //        cmd.Parameters.Add(@"CompanyID", SqlDbType.VarChar).Value = CompanyID;
            //        cmd.Parameters.Add(@"Transaction_Amount", SqlDbType.Decimal).Value = Transaction_Amount;
            //        cmd.Parameters.Add(@"TransType", SqlDbType.VarChar, 1).Value = TransType;
            //        cmd.Parameters.Add(@"BookingRef", SqlDbType.Int).Value = BookingRef;
            //        cmd.Parameters.Add(@"UpdatedBy", SqlDbType.VarChar).Value = UpdatedBy;
            //        cmd.Parameters.Add(@"Remark", SqlDbType.VarChar).Value = Remark;

            //        connection.Open();
            //        int iRows = cmd.ExecuteNonQuery();
            //        connection.Close();

            //        if (iRows > 0)
            //        {
            //            bStatus = true;
            //        }
            //    }
            //}
            //catch (Exception ex)
            //{
            //    bStatus = false;
            //    DBCommon.Logger.dbLogg(CompanyID, BookingRef, "SET_GET_Company_Amount_Transaction", "db_Company", SearchCriteria, SearchID, TransType + Environment.NewLine + Remark + Environment.NewLine + ex.Message);
            //}
            return bStatus;
        }
        public bool SET_Debit_Company_Amount_Transaction(string SearchID, string SearchCriteria, string CompanyID, Decimal Transaction_Amount, string TransType, Int32 BookingRef, string UpdatedBy, string Remark)
        {
            bool bStatus = false;

            //try
            //{
            //    using (SqlConnection connection = new SqlConnection(ConnectionString.dbConnect))
            //    {
            //        SqlCommand cmd = new SqlCommand("Company_Balance_Transaction_Detail_Proc", connection);
            //        cmd.CommandType = CommandType.StoredProcedure;
            //        cmd.CommandTimeout = 10;

            //        cmd.Parameters.Add(@"ProcNo", SqlDbType.Int).Value = 6;
            //        cmd.Parameters.Add(@"CompanyID", SqlDbType.VarChar).Value = CompanyID;
            //        cmd.Parameters.Add(@"Transaction_Amount", SqlDbType.Decimal).Value = Transaction_Amount;
            //        cmd.Parameters.Add(@"TransType", SqlDbType.VarChar, 1).Value = TransType;
            //        cmd.Parameters.Add(@"BookingRef", SqlDbType.Int).Value = BookingRef;
            //        cmd.Parameters.Add(@"UpdatedBy", SqlDbType.VarChar).Value = UpdatedBy;
            //        cmd.Parameters.Add(@"Remark", SqlDbType.VarChar).Value = Remark;

            //        connection.Open();
            //        int iRows = cmd.ExecuteNonQuery();
            //        connection.Close();

            //        if (iRows > 0)
            //        {
            //            bStatus = true;
            //        }
            //    }
            //}
            //catch (Exception ex)
            //{
            //    bStatus = false;
            //    DBCommon.Logger.dbLogg(CompanyID, BookingRef, "SET_Debit_Company_Amount_Transaction", "db_Company", SearchCriteria, SearchID, TransType + Environment.NewLine + Remark + Environment.NewLine + ex.Message);
            //}
            return bStatus;
        }
    }
}
