using Microsoft.AspNetCore.Http;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZealTravel.Common;
using ZealTravel.Domain.Data.Entities;
using ZealTravel.Domain.Interfaces.GSTManagement;
using ZealTravelWebsite.Infrastructure.Context;

namespace ZealTravel.Infrastructure.Services.Aireline
{
    public class GSTService : IGSTService
    {
        private readonly ZealdbNContext _context;

        public GSTService(ZealdbNContext context)
        {
            _context = context;
        }

        //public bool DELETE_GSTdetail(string CompanyID)
        //{
        //    bool Status = false;
        //    try
        //    {
        //        using (SqlConnection dbCon = new SqlConnection(dbCommon.ConnectionString.dbConnect))
        //        {
        //            SqlCommand dbCmd = new SqlCommand("Company_Register_Gst_Proc", dbCon);
        //            dbCmd.CommandType = CommandType.StoredProcedure;
        //            dbCmd.Parameters.Add(@"ProcNo", SqlDbType.Int).Value = 4;
        //            dbCmd.Parameters.Add(@"CompanyID", SqlDbType.VarChar).Value = CompanyID;

        //            dbCon.Open();
        //            int i = dbCmd.ExecuteNonQuery();
        //            dbCon.Close();

        //            if (i > 0)
        //            {
        //                Status = true;
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        dbCommon.Logger.dbLogg(CompanyID, 0, "SET_GSTdetail", "cls_gstRegis", Initializer.get_StaffID(), "", ex.Message);
        //    }
        //    return Status;
        //}
        //public bool SET_GSTdetail(string CompanyID, string GSTNumber, string GSTCompanyEmail, string GSTCompanyName, string GSTCompanyContactNumber, string GSTCompanyAddress)
        //{
        //    bool Status = false;
        //    try
        //    {
        //        using (SqlConnection dbCon = new SqlConnection(dbCommon.ConnectionString.dbConnect))
        //        {
        //            SqlCommand dbCmd = new SqlCommand("Company_Register_Gst_Proc", dbCon);
        //            dbCmd.CommandType = CommandType.StoredProcedure;
        //            dbCmd.Parameters.Add(@"ProcNo", SqlDbType.Int).Value = 3;
        //            dbCmd.Parameters.Add(@"CompanyID", SqlDbType.VarChar).Value = CompanyID;
        //            dbCmd.Parameters.Add(@"GSTCompanyEmail", SqlDbType.VarChar).Value = GSTCompanyEmail;
        //            dbCmd.Parameters.Add(@"GSTCompanyName", SqlDbType.VarChar).Value = GSTCompanyName;
        //            dbCmd.Parameters.Add(@"GSTCompanyContactNumber", SqlDbType.VarChar).Value = GSTCompanyContactNumber;
        //            dbCmd.Parameters.Add(@"GSTCompanyAddress", SqlDbType.VarChar).Value = GSTCompanyAddress;
        //            dbCmd.Parameters.Add(@"GSTNumber", SqlDbType.VarChar).Value = GSTNumber;

        //            dbCon.Open();
        //            int i = dbCmd.ExecuteNonQuery();
        //            dbCon.Close();

        //            if (i > 0)
        //            {
        //                Status = true;
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        dbCommon.Logger.dbLogg(CompanyID, 0, "SET_GSTdetail", "cls_gstRegis", Initializer.get_StaffID(), "", ex.Message);
        //    }
        //    return Status;
        //}
        //public bool ADD_GSTdetail(string CompanyID, string GSTNumber, string GSTCompanyEmail, string GSTCompanyName, string GSTCompanyContactNumber, string GSTCompanyAddress)
        //{
        //    bool Status = false;
        //    try
        //    {
        //        using (SqlConnection dbCon = new SqlConnection(dbCommon.ConnectionString.dbConnect))
        //        {
        //            SqlCommand dbCmd = new SqlCommand("Company_Register_Gst_Proc", dbCon);
        //            dbCmd.CommandType = CommandType.StoredProcedure;
        //            dbCmd.Parameters.Add(@"ProcNo", SqlDbType.Int).Value = 1;
        //            dbCmd.Parameters.Add(@"CompanyID", SqlDbType.VarChar).Value = CompanyID;
        //            dbCmd.Parameters.Add(@"GSTCompanyEmail", SqlDbType.VarChar).Value = GSTCompanyEmail;
        //            dbCmd.Parameters.Add(@"GSTCompanyName", SqlDbType.VarChar).Value = GSTCompanyName;
        //            dbCmd.Parameters.Add(@"GSTCompanyContactNumber", SqlDbType.VarChar).Value = GSTCompanyContactNumber;
        //            dbCmd.Parameters.Add(@"GSTCompanyAddress", SqlDbType.VarChar).Value = GSTCompanyAddress;
        //            dbCmd.Parameters.Add(@"GSTNumber", SqlDbType.VarChar).Value = GSTNumber;

        //            dbCon.Open();
        //            int i = dbCmd.ExecuteNonQuery();
        //            dbCon.Close();

        //            if (i > 0)
        //            {
        //                Status = true;
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        dbCommon.Logger.dbLogg(CompanyID, 0, "SET_GSTdetail", "cls_gstRegis", Initializer.get_StaffID(), "", ex.Message);
        //    }
        //    return Status;
        //}
        public async Task<CompanyRegisterGst> GetGSTdetail(string CompanyID)
        {
            try
            {
                var result = await _context.Database.SqlQuery<CompanyRegisterGst>(
                    $"EXEC Company_Register_Gst_Proc @ProcNo = 2, @CompanyID = {CompanyID}"
                ).ToListAsync();

                return result?.FirstOrDefault();
            }
            catch (Exception ex)
            {
               // dbCommon.Logger.dbLogg(CompanyID, 0, "GET_GSTdetail", "cls_gstRegis", Initializer.get_StaffID(), "", ex.Message);
                return null;
            }
        }
        public async Task<bool> SETGSTdetail(string CompanyID, string PassengerResponse, string GSTInfo)
        {
            bool Status = false;
            try
            {
                DataTable dtPassenger = CommonFunction.StringToDataSet(GSTInfo).Tables["GstInfo"];

                string GSTCompanyEmail = dtPassenger?.Rows[0]?["GSTCompanyEmail"]?.ToString().Trim() ?? string.Empty;
                string GSTCompanyName = dtPassenger?.Rows[0]?["GSTCompanyName"]?.ToString().Trim() ?? string.Empty;
                string GSTCompanyContactNumber = dtPassenger?.Rows[0]?["GSTCompanyContactNumber"]?.ToString().Trim() ?? string.Empty;
                string GSTCompanyAddress = dtPassenger?.Rows[0]?["GSTCompanyAddress"]?.ToString().Trim() ?? string.Empty;
                string GSTNumber = dtPassenger?.Rows[0]?["GSTNumber"]?.ToString().Trim() ?? string.Empty;
                string sql = @"
            EXECUTE Company_Register_Gst_Proc 
                @ProcNo = {0}, 
                @CompanyID = {1}, 
                @GSTCompanyEmail = {2}, 
                @GSTCompanyName = {3}, 
                @GSTCompanyContactNumber = {4}, 
                @GSTCompanyAddress = {5}, 
                @GSTNumber = {6}";

                // Execute the stored procedure
                var rowsAffected =  _context.Database.ExecuteSqlRawAsync(
                    sql,
                    1, // ProcNo
                    CompanyID,
                    GSTCompanyEmail,
                    GSTCompanyName,
                    GSTCompanyContactNumber,
                    GSTCompanyAddress,
                    GSTNumber
                );
                if (rowsAffected.Result > 0)
                {
                    Status = true;
                }

            }
            catch (Exception ex)
            {
                // dbCommon.Logger.dbLogg(CompanyID, 0, "SET_GSTdetail", "cls_gstRegis", Initializer.get_StaffID(), "", ex.Message);
            }
            return Status;
        }

        public async Task<bool> SetGSTAirline(string CompanyID, int BookingRef, DataTable dtPassenger)
        {
            bool SaveStatus = false;
            try
            { 
                string sql = @"EXECUTE Company_Flight_Gst_Detail_Proc @ProcNo = {0}, @BookingRef = {1}, @GSTCompanyAddress = {2}, @GSTCompanyContactNumber = {3}, @GSTCompanyName = {4}, @GSTNumber = {5}, @GSTCompanyEmail = {6}";
                var iRow =  _context.Database.ExecuteSqlRawAsync(
                    sql,
                    1, // ProcNo
                    BookingRef,
                    dtPassenger.Rows[0]["GSTCompanyAddress"].ToString().Trim(),
                    dtPassenger.Rows[0]["GSTCompanyContactNumber"].ToString().Trim(),
                    dtPassenger.Rows[0]["GSTCompanyName"].ToString().Trim(),
                    dtPassenger.Rows[0]["GSTNumber"].ToString().Trim(),
                    dtPassenger.Rows[0]["GSTCompanyEmail"].ToString().Trim()
                );

                if (iRow.Result > 0)
                {
                    SaveStatus = true;
                }
            }
            catch (Exception ex)
            {
                // dbCommon.Logger.dbLogg(CompanyID, BookingRef, "", "set_GST_Airline", "", "", ex.Message);
            }
            return SaveStatus;
        }

    }
}
