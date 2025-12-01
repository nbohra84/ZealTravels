using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using Microsoft.Identity.Client;
using ZealTravelWebsite.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using ZealTravel.Domain.Interfaces.DBCommon;

namespace ZealTravel.Infrastructure.DBCommon
{
   public class AirlineDetail : IAirlineDetailService
    {
        private readonly ZealdbNContext _context;

        public AirlineDetail(ZealdbNContext context)
        {
            _context = context;
        }

        //public static string getCarrierName(string CarrierCode)
        //{
        //    string Carrier = string.Empty;
        //    try
        //    {
        //        using (SqlConnection connection = new SqlConnection(ConnectionString.dbConnect))
        //        {
        //            SqlCommand cmd = new SqlCommand("CarrierDetail_Proc", connection);
        //            cmd.CommandType = CommandType.StoredProcedure;
        //            cmd.CommandTimeout = 10;
        //            cmd.Parameters.Add(@"ProcNo", SqlDbType.Int).Value = 6;
        //            cmd.Parameters.Add(@"CarrierCode", SqlDbType.VarChar).Value = CarrierCode;

        //            connection.Open();
        //            Carrier = cmd.ExecuteScalar().ToString();
        //            connection.Close();
        //        }
        //    }
        //    catch (Exception ex)
        //    {

        //    }
        //    return Carrier;
        //}
        public async Task<bool> IsDomestic(string Origin, string Destination)
        {
            bool status = false;
            try
            {
                var result = await _context.Database.SqlQuery<bool>($"EXECUTE AirportCities_Proc @ProcNo = 3, @Origin = {Origin}, @Destination = {Destination}").ToListAsync();
                status = result.FirstOrDefault();
                return status;
            }
            catch (Exception ex)
            {
                // Handle exception
            }
            return status;
        }
        // public static DataTable getSupplierDetail()
        //{
        //    DataTable dtSupplier = new DataTable();
        //    try
        //    {
        //        using (SqlConnection connection = new SqlConnection(ConnectionString.dbConnect))
        //        {
        //            SqlCommand cmd = new SqlCommand("Supplier_Detail_Proc", connection);
        //            cmd.CommandType = CommandType.StoredProcedure;
        //            cmd.CommandTimeout = 10;
        //            cmd.Parameters.Add(@"ProcNo", SqlDbType.Int).Value = 1;
        //            SqlDataAdapter da = new SqlDataAdapter(cmd);
        //            dtSupplier.TableName = "SupplierDetail";
        //            da.Fill(dtSupplier);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        dbCommon.Logger.dbLogg("", 0, "getSupplierDetail", "Airline_Detail", "Supplier_Detail_Proc", "", ex.Message);
        //    }

        //    return dtSupplier;
        //}

        //public static bool getPNRstatus(string CarrierCode, string Sector)
        //{
        //    bool Status = false;

        //    try
        //    {
        //        using (SqlConnection connection = new SqlConnection(ConnectionString.dbConnect))
        //        {
        //            SqlCommand cmd = new SqlCommand("Group_Commission_Rule_New_Proc", connection);
        //            cmd.CommandType = CommandType.StoredProcedure;
        //            cmd.CommandTimeout = 10;
        //            cmd.Parameters.Add(@"ProcNo", SqlDbType.Int).Value = 10;
        //            cmd.Parameters.Add(@"CarrierCode", SqlDbType.VarChar).Value = CarrierCode;
        //            cmd.Parameters.Add(@"Sector", SqlDbType.VarChar).Value = Sector;

        //            connection.Open();
        //            Status = (bool)cmd.ExecuteScalar();
        //            connection.Close();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Logger.dbLogg("", 0, "", "", "", "", ex.Message);
        //    }

        //    return Status;
        //}
        //public static bool getTicketstatus(string CarrierCode, string Sector)
        //{
        //    bool Status = false;

        //    try
        //    {
        //        using (SqlConnection connection = new SqlConnection(ConnectionString.dbConnect))
        //        {
        //            SqlCommand cmd = new SqlCommand("Group_Commission_Rule_New_Proc", connection);
        //            cmd.CommandType = CommandType.StoredProcedure;
        //            cmd.CommandTimeout = 10;
        //            cmd.Parameters.Add(@"ProcNo", SqlDbType.Int).Value = 11;
        //            cmd.Parameters.Add(@"CarrierCode", SqlDbType.VarChar).Value = CarrierCode;
        //            cmd.Parameters.Add(@"Sector", SqlDbType.VarChar).Value = Sector;

        //            connection.Open();
        //            Status = (bool)cmd.ExecuteScalar();
        //            connection.Close();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Logger.dbLogg("", 0, "", "", "", "", ex.Message);
        //    }

        //    return Status;
        //}

        //public static DataTable CityAirportName(string AirportList)
        //{
        //    DataTable dtAirport = new DataTable();

        //    try
        //    {
        //        using (SqlConnection connection = new SqlConnection(ConnectionString.dbConnect))
        //        {
        //            SqlCommand cmd = new SqlCommand("St_CityAirportName_Proc", connection);
        //            cmd.CommandType = CommandType.StoredProcedure;
        //            cmd.CommandTimeout = 10;
        //            cmd.Parameters.Add(@"ProcNo", SqlDbType.Int).Value = 2;
        //            cmd.Parameters.Add(@"AirportList", SqlDbType.VarChar).Value = AirportList;

        //            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
        //            adapter.Fill(dtAirport);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Logger.dbLogg("", 0, "", "", "", "", ex.Message);
        //    }

        //    return dtAirport;
        //}
        // public static DataTable CityAirportName()
        // {
        //     DataTable dtAirport = new DataTable();

        //     try
        //     {
        //         using (SqlConnection connection = new SqlConnection(ConnectionString.dbConnect))
        //         {
        //             SqlCommand cmd = new SqlCommand("St_CityAirportName_Proc", connection);
        //             cmd.CommandType = CommandType.StoredProcedure;
        //             cmd.CommandTimeout = 10;
        //             cmd.Parameters.Add(@"ProcNo", SqlDbType.Int).Value = 1;

        //             SqlDataAdapter adapter = new SqlDataAdapter(cmd);
        //             adapter.Fill(dtAirport);
        //         }
        //     }
        //     catch (Exception ex)
        //     {
        //         Logger.dbLogg("", 0, "", "", "", "", ex.Message);
        //     }

        //     return dtAirport;
        // }
        // public static DataTable CarrierName()
        // {
        //     DataTable dtCarrier = new DataTable();

        //     try
        //     {
        //         using (SqlConnection connection = new SqlConnection(ConnectionString.dbConnect))
        //         {
        //             SqlCommand cmd = new SqlCommand("CarrierDetail_Proc", connection);
        //             cmd.CommandType = CommandType.StoredProcedure;
        //             cmd.CommandTimeout = 10;
        //             cmd.Parameters.Add(@"ProcNo", SqlDbType.Int).Value = 1;

        //             SqlDataAdapter adapter = new SqlDataAdapter(cmd);
        //             adapter.Fill(dtCarrier);
        //         }
        //     }
        //     catch (Exception ex)
        //     {
        //         Logger.dbLogg("", 0, "", "", "", "", ex.Message);
        //     }

        //     return dtCarrier;
        // }
        // public static DataTable CarrierName(string CarrierList)
        // {
        //     DataTable dtCarrier = new DataTable();

        //     try
        //     {
        //         using (SqlConnection connection = new SqlConnection(ConnectionString.dbConnect))
        //         {
        //             SqlCommand cmd = new SqlCommand("CarrierDetail_Proc", connection);
        //             cmd.CommandType = CommandType.StoredProcedure;
        //             cmd.CommandTimeout = 10;
        //             cmd.Parameters.Add(@"ProcNo", SqlDbType.Int).Value = 2;
        //             cmd.Parameters.Add(@"CarrierList", SqlDbType.VarChar).Value = CarrierList;

        //             SqlDataAdapter adapter = new SqlDataAdapter(cmd);
        //             adapter.Fill(dtCarrier);
        //         }
        //     }
        //     catch (Exception ex)
        //     {
        //         Logger.dbLogg("", 0, "", "", "", "", ex.Message);
        //     }

        //     return dtCarrier;
        // }
        public async Task<bool> GetDateValidationDays(string CarrierCode, string Sector, string DepartureDate)
        {
            int PnrDays = await GetDateValidationDays(CarrierCode, Sector);
            if (PnrDays > 0)
            {
                return CaclulateDateValidation(DepartureDate, PnrDays);
            }
            else
            {
                return true;
            }
        }
        private bool CaclulateDateValidation(string DepartureDate, int PnrDays)
        {
            DateTime dDeparture = Convert.ToDateTime(DepartureDate);
            DateTime dCurrentDate = DateTime.Today;
            TimeSpan ts = dDeparture - dCurrentDate;

            if (ts.Days > PnrDays)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        private async Task<Int32> GetDateValidationDays(string CarrierCode, string Sector)
        {
            int PnrDays = 0;
            try
            {
                var result = await _context.Database.SqlQuery<int>($"EXEC Group_Commission_PriceType_Proc @ProcNo = 3, @CarrierCode = {CarrierCode}, @Sector = {Sector}").ToListAsync();
                PnrDays = result.FirstOrDefault();
            }
            catch (Exception ex)
            {
                //dbCommon.Logger.dbLogg(CarrierCode, 0, "getDateValidationDays", "LibraryTBOapi", "tboQuery", CarrierCode, ex.Message);
            }
            return PnrDays;
        }


    }
}
