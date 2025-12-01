using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace ZealTravel.Infrastructure.AirCalculations
{
    class dbCommission
    {
        public static bool IsApiAgent(string CompanyID)
        {
            try
            {
                //using (SqlConnection connection = new SqlConnection(DBCommon.ConnectionString.dbConnect))
                //{
                //    SqlCommand cmd = new SqlCommand("Company_Register_Proc", connection);
                //    cmd.CommandType = CommandType.StoredProcedure;
                //    cmd.CommandTimeout = 10;
                //    cmd.Parameters.Add(@"ProcNo", SqlDbType.Int).Value = 44;
                //    cmd.Parameters.Add(@"CompanyID", SqlDbType.VarChar).Value = CompanyID;

                //    connection.Open();

                    

                //    bool ApiAgent = Convert.ToBoolean(cmd.ExecuteScalar().ToString());
                //    connection.Close();

                //    return ApiAgent;
                //}
            }
            catch (Exception ex)
            {
                DBCommon.Logger.dbLogg(CompanyID, 0, "IsApiAgent", "dbCommission", "", CompanyID, ex.Message);
            }
            return false;
        }
        public static DataTable AgencyToSubAgencyMarkup(string CompanyID, string Sector)
        {
            DataTable dtSubAgencyMarkup = new DataTable();
            try
            {
                //using (SqlConnection connection = new SqlConnection(DBCommon.ConnectionString.dbConnect))
                //{
                //    SqlCommand cmd = new SqlCommand("Air_Markup_Rule_SA_Proc", connection);
                //    cmd.CommandType = CommandType.StoredProcedure;
                //    cmd.CommandTimeout = 10;

                //    cmd.Parameters.Add(@"ProcNo", SqlDbType.Int).Value = 3;
                //    cmd.Parameters.Add(@"CompanyID", SqlDbType.VarChar).Value = CompanyID;
                //    cmd.Parameters.Add(@"Sector", SqlDbType.VarChar).Value = Sector;
                //    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                //    adapter.Fill(dtSubAgencyMarkup);
                //}
            }
            catch (Exception ex)
            {
                DBCommon.Logger.dbLogg(CompanyID, 0, "dbCommission", "AgencyToSubAgencyMarkup", "Air_Markup_Rule_SA_Proc-3", "", ex.Message);
            }
            return dtSubAgencyMarkup;
        }
        public static DataTable SupplierwiseExtraCommission(string Sector)
        {
            DataTable dtCommission = new DataTable();

            try
            {
                //using (SqlConnection connection = new SqlConnection(DBCommon.ConnectionString.dbConnect))
                //{
                //    SqlCommand cmd = new SqlCommand("Air_ExtraCommission_Proc", connection);
                //    cmd.CommandType = CommandType.StoredProcedure;
                //    cmd.CommandTimeout = 10;

                //    cmd.Parameters.Add(@"ProcNo", SqlDbType.Int).Value = 1;
                //    cmd.Parameters.Add(@"Sector", SqlDbType.VarChar, 1).Value = Sector;

                //    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                //    adapter.Fill(dtCommission);
                //}
            }
            catch (Exception ex)
            {
                DBCommon.Logger.dbLogg("", 0, "SupplierwiseExtraCommission", "dbCommission", "Air_ExtraCommission_Proc-1", Sector, ex.Message);
            }

            return dtCommission;
        }
        public static DataTable Cfee(string CompanyID, string BookingType, string Sector)
        {
            DataTable dtCfee = new DataTable();
            try
            {
                CompanyID = DBCommon.CommonFunction.getCompany_by_SubCompany_Customer(CompanyID);
                //using (SqlConnection connection = new SqlConnection(DBCommon.ConnectionString.dbConnect))
                //{
                //    SqlCommand cmd = new SqlCommand("Company_Bookings_Cfee_Proc", connection);
                //    cmd.CommandType = CommandType.StoredProcedure;
                //    cmd.CommandTimeout = 10;

                //    cmd.Parameters.Add(@"ProcNo", SqlDbType.Int).Value = 1;
                //    cmd.Parameters.Add(@"CompanyID", SqlDbType.VarChar).Value = CompanyID;
                //    cmd.Parameters.Add(@"BookingType", SqlDbType.VarChar).Value = BookingType;
                //    cmd.Parameters.Add(@"Sector", SqlDbType.VarChar).Value = Sector;
                //    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                //    adapter.Fill(dtCfee);
                //}
            }
            catch (Exception ex)
            {
                DBCommon.Logger.dbLogg(CompanyID, 0, "dbCommission", "Cfee", "Company_Bookings_Cfee_Proc-1", BookingType, ex.Message);
            }
            return dtCfee;
        }
        public static DataTable Supplierwise_CommissionRule(string Sector)
        {
            DataTable dtCommission = new DataTable();

            try
            {
                //using (SqlConnection connection = new SqlConnection(DBCommon.ConnectionString.dbConnect))
                //{
                //    SqlCommand cmd = new SqlCommand("Commission_api_Proc", connection);
                //    cmd.CommandType = CommandType.StoredProcedure;
                //    cmd.CommandTimeout = 10;

                //    cmd.Parameters.Add(@"ProcNo", SqlDbType.Int).Value = 1;
                //    cmd.Parameters.Add(@"Sector", SqlDbType.VarChar, 1).Value = Sector;

                //    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                //    adapter.Fill(dtCommission);
                //}
            }
            catch (Exception ex)
            {
                DBCommon.Logger.dbLogg("", 0, "Supplierwise_CommissionRule", "dbCommission", "Commission_api_Proc-1", Sector, ex.Message);
            }

            return dtCommission;
        }
        public static bool IsSotoSector(string Origin)
        {
            bool IsSOTO = false;
            try
            {
                string CountryCode = string.Empty;
                //using (SqlConnection connection = new SqlConnection(DBCommon.ConnectionString.dbConnect))
                //{
                //    SqlCommand cmd = new SqlCommand("AirportCities_Proc", connection);
                //    cmd.CommandType = CommandType.StoredProcedure;
                //    cmd.CommandTimeout = 10;
                //    cmd.Parameters.Add(@"ProcNo", SqlDbType.Int).Value = 2;
                //    cmd.Parameters.Add(@"Origin", SqlDbType.VarChar).Value = Origin;

                //    connection.Open();
                //    if (cmd.ExecuteScalar() != null)
                //    {
                //        CountryCode = cmd.ExecuteScalar().ToString();
                //    }
                //    connection.Close();

                //    if (CountryCode != "IN" || CountryCode.Length.Equals(0))
                //    {
                //        IsSOTO = true;
                //    }
                //}
            }
            catch (Exception ex)
            {
                DBCommon.Logger.dbLogg(Origin, 0, "IsSotoSector", "dbCommission", "", "", ex.Message);
            }
            return IsSOTO;
        }
        public static string getUserType(string CompanyID)
        {
            string UserType = "";
            if (CompanyID.IndexOf("AD-") != -1)
            {
                UserType = "AD";
            }
            else if (CompanyID.IndexOf("A-") != -1 && CompanyID.IndexOf("-SA-") != -1)
            {
                UserType = "B2B2B";
            }
            else if (CompanyID.IndexOf("A-") != -1 && CompanyID.IndexOf("-C-") != -1)
            {
                UserType = "B2B2C";
            }
            else if (CompanyID.IndexOf("A-") != -1 && CompanyID.IndexOf("-SA-") == -1 && CompanyID.IndexOf("-C-") == -1)
            {
                UserType = "B2B";
            }
            else if (CompanyID.IndexOf("C-") != -1 && CompanyID.IndexOf("-C-") == -1)
            {
                UserType = "B2C";
            }
            return UserType;
        }
        public static string getCustomerAdminID(string SearchID)
        {
            string CompanyID = string.Empty;
            try
            {
                //using (SqlConnection connection = new SqlConnection(DBCommon.ConnectionString.dbConnect))
                //{
                //    SqlCommand cmd = new SqlCommand("LoggerSearch_Proc", connection);
                //    cmd.CommandType = CommandType.StoredProcedure;
                //    cmd.CommandTimeout = 10;
                //    cmd.Parameters.Add(@"ProcNo", SqlDbType.Int).Value = 2;
                //    cmd.Parameters.Add(@"SearchID", SqlDbType.VarChar).Value = SearchID;

                //    connection.Open();
                //    CompanyID = cmd.ExecuteScalar().ToString();
                //    connection.Close();
                //}
            }
            catch (Exception ex)
            {
                DBCommon.Logger.dbLogg(CompanyID, 0, "getCustomerAdminID", "dbCommission", "", SearchID, ex.Message);
            }

            if (CompanyID.IndexOf("A-") != -1)
            {
                CompanyID = CompanyID + "-C-0000";
            }

            return CompanyID;
        }
        public static DataTable CountryCode(string Origin, string Destination)
        {
            DataTable dtCountry = new DataTable();

            try
            {
                //using (SqlConnection connection = new SqlConnection(DBCommon.ConnectionString.dbConnect))
                //{
                //    SqlCommand cmd = new SqlCommand("AirportCities_Proc", connection);
                //    cmd.CommandType = CommandType.StoredProcedure;
                //    cmd.CommandTimeout = 10;

                //    cmd.Parameters.Add(@"ProcNo", SqlDbType.Int).Value = 1;
                //    cmd.Parameters.Add(@"Origin", SqlDbType.VarChar).Value = Origin;
                //    cmd.Parameters.Add(@"Destination", SqlDbType.VarChar).Value = Destination;

                //    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                //    adapter.Fill(dtCountry);
                //}
            }
            catch (Exception ex)
            {
                DBCommon.Logger.dbLogg(Origin, 0, "CountryCode", "dbCommission", Destination, "AirportCities_Proc", ex.Message);
            }

            return dtCountry;
        }
        public static DataTable CompanyMarkupDetail(string CompanyID, string Sector)
        {
            DataTable dtMU = new DataTable();

            try
            {
                //using (SqlConnection connection = new SqlConnection(DBCommon.ConnectionString.dbConnect))
                //{
                //    SqlCommand cmd = new SqlCommand("Company_Markup_Airline_Proc", connection);
                //    cmd.CommandType = CommandType.StoredProcedure;
                //    cmd.CommandTimeout = 10;

                //    cmd.Parameters.Add(@"ProcNo", SqlDbType.Int).Value = 1;
                //    cmd.Parameters.Add(@"CompanyID", SqlDbType.VarChar, 50).Value = CompanyID;
                //    cmd.Parameters.Add(@"Sector", SqlDbType.VarChar, 1).Value = Sector;

                //    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                //    adapter.Fill(dtMU);
                //}
            }
            catch (Exception ex)
            {
                DBCommon.Logger.dbLogg(CompanyID, 0, "CompanyMarkupDetail", "dbCommission", Sector, "", ex.Message);
            }

            return dtMU;
        }
        public static DataTable CompanyServiceTaxDetail(string CompanyID)
        {
            DataTable dtST = new DataTable();

            try
            {
                //using (SqlConnection connection = new SqlConnection(DBCommon.ConnectionString.dbConnect))
                //{
                //    SqlCommand cmd = new SqlCommand("Company_Product_ServiceTax_Detail_Proc", connection);
                //    cmd.CommandType = CommandType.StoredProcedure;
                //    cmd.CommandTimeout = 10;

                //    cmd.Parameters.Add(@"ProcNo", SqlDbType.Int).Value = 3;
                //    cmd.Parameters.Add(@"CompanyID", SqlDbType.VarChar, 50).Value = CompanyID;

                //    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                //    adapter.Fill(dtST);
                //}
            }
            catch (Exception ex)
            {
                DBCommon.Logger.dbLogg(CompanyID, 0, "CompanyServiceTaxDetail", "dbCommission", "", "", ex.Message);
            }

            return dtST;
        }
        public static DataTable CompanyTDSDetail(string CompanyID)
        {
            DataTable dtTDS = new DataTable();

            try
            {
                //using (SqlConnection connection = new SqlConnection(DBCommon.ConnectionString.dbConnect))
                //{
                //    SqlCommand cmd = new SqlCommand("Company_Product_TDS_Detail_Proc", connection);
                //    cmd.CommandType = CommandType.StoredProcedure;
                //    cmd.CommandTimeout = 10;

                //    cmd.Parameters.Add(@"ProcNo", SqlDbType.Int).Value = 3;
                //    cmd.Parameters.Add(@"CompanyID", SqlDbType.VarChar, 50).Value = CompanyID;

                //    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                //    adapter.Fill(dtTDS);
                //}
            }
            catch (Exception ex)
            {
                DBCommon.Logger.dbLogg(CompanyID, 0, "CompanyTDSDetail", "dbCommission", "", "", ex.Message);
            }

            return dtTDS;
        }
        public static DataTable CompanyCommissionRule(string Sector, string CompanyID)
        {
            DataTable dtCommission = new DataTable();

            try
            {
                //using (SqlConnection connection = new SqlConnection(DBCommon.ConnectionString.dbConnect))
                //{
                //    SqlCommand cmd = new SqlCommand("Group_Commission_Rule_New_Proc", connection);
                //    cmd.CommandType = CommandType.StoredProcedure;
                //    cmd.CommandTimeout = 10;

                //    cmd.Parameters.Add(@"ProcNo", SqlDbType.Int).Value = 7;
                //    cmd.Parameters.Add(@"CompanyID", SqlDbType.VarChar, 50).Value = CompanyID;
                //    cmd.Parameters.Add(@"Sector", SqlDbType.VarChar, 1).Value = Sector;

                //    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                //    adapter.Fill(dtCommission);
                //}
            }
            catch (Exception ex)
            {
                DBCommon.Logger.dbLogg(CompanyID, 0, "CompanyCommissionRule", "dbCommission", "Group_Commission_Rule_New_Proc", Sector, ex.Message);
            }

            return dtCommission;
        }
        public static DataTable CompanyCommissionRule_SA(string CompanyID, string Sector)
        {
            DataTable dtGroup = new DataTable();
            try
            {
                //using (SqlConnection connection = new SqlConnection(DBCommon.ConnectionString.dbConnect))
                //{
                //    SqlCommand cmd = new SqlCommand("Group_Commission_Rule_SA_Proc", connection);
                //    cmd.CommandType = CommandType.StoredProcedure;
                //    cmd.CommandTimeout = 10;
                //    cmd.Parameters.Add(@"ProcNo", SqlDbType.Int).Value = 4;
                //    cmd.Parameters.Add(@"CompanyID", SqlDbType.VarChar).Value = CompanyID;
                //    cmd.Parameters.Add(@"Sector", SqlDbType.VarChar).Value = Sector;

                //    SqlDataAdapter da = new SqlDataAdapter(cmd);
                //    da.Fill(dtGroup);
                //}
            }
            catch (Exception ex)
            {
                DBCommon.Logger.dbLogg(CompanyID, 0, "CompanyCommissionRule_SA", "dbCommission", "", Sector, ex.Message);
            }

            return dtGroup;
        }
        public static DataTable CompanyCommissionRule_C(string Sector, string CompanyID)
        {
            DataTable dtGroup = new DataTable();
            try
            {
                //using (SqlConnection connection = new SqlConnection(DBCommon.ConnectionString.dbConnect))
                //{
                //    SqlCommand cmd = new SqlCommand("Group_Commission_C_N_Proc", connection);
                //    cmd.CommandType = CommandType.StoredProcedure;
                //    cmd.CommandTimeout = 10;
                //    cmd.Parameters.Add(@"ProcNo", SqlDbType.Int).Value = 1;
                //    cmd.Parameters.Add(@"CompanyID", SqlDbType.VarChar).Value = CompanyID;
                //    cmd.Parameters.Add(@"Sector", SqlDbType.VarChar).Value = Sector;

                //    SqlDataAdapter da = new SqlDataAdapter(cmd);
                //    da.Fill(dtGroup);
                //}
            }
            catch (Exception ex)
            {
                DBCommon.Logger.dbLogg(CompanyID, 0, "CompanyCommissionRule_C", "dbCommission", "", Sector, ex.Message);
            }

            return dtGroup;
        }
        public static DataTable CompanyCommissionRule_PriceType(string Sector)
        {
            DataTable dtGroup = new DataTable();
            try
            {
                //using (SqlConnection connection = new SqlConnection(DBCommon.ConnectionString.dbConnect))
                //{
                //    SqlCommand cmd = new SqlCommand("Group_Commission_PriceType_Proc", connection);
                //    cmd.CommandType = CommandType.StoredProcedure;
                //    cmd.CommandTimeout = 10;
                //    cmd.Parameters.Add(@"ProcNo", SqlDbType.Int).Value = 1;
                //    cmd.Parameters.Add(@"Sector", SqlDbType.VarChar).Value = Sector;

                //    SqlDataAdapter da = new SqlDataAdapter(cmd);
                //    da.Fill(dtGroup);
                //}
            }
            catch (Exception ex)
            {
                DBCommon.Logger.dbLogg("", 0, "CompanyCommissionRule_PriceType", "dbCommission", "", Sector, ex.Message); ;
            }

            return dtGroup;
        }
    }
}
