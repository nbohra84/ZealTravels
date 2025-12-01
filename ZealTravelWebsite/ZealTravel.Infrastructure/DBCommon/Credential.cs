using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using ZealTravelWebsite.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using ZealTravel.Domain.Data.Entities;
using CommonComponents;
using ZealTravel.Domain.Interfaces.DBCommon;
using ZealTravel.Domain.Models;

namespace ZealTravel.Infrastructure.DBCommon
{
    public class Credential:ICredential
    {
        ZealdbNContext _nContext;
        public Credential(ZealdbNContext nContext)
        {
            _nContext = nContext;
        }
        public async Task<List<T>> AirlineCredentialDetail<T>(string SupplierType, string SupplierID) where T : class 
        {
            List<T> CredentialDT = new List<T>();

            try
            {
                //using (SqlConnection connection = new SqlConnection(ConnectionString.dbConnect))
                // {
                //    SqlCommand cmd = new SqlCommand("SupplierCredentialDetail_Proc", connection);
                //    cmd.CommandType = CommandType.StoredProcedure;
                //    //cmd.CommandTimeout = 10;

                //    cmd.Parameters.Add(@"ProcNo", SqlDbType.Int).Value = 1;
                //    cmd.Parameters.Add(@"SupplierType", SqlDbType.VarChar, 50).Value = SupplierType;
                //    cmd.Parameters.Add(@"SupplierID", SqlDbType.VarChar, 50).Value = SupplierID;

                //    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                //    adapter.Fill(CredentialDT);
                //}
                
                if (SupplierType.ToLower() == "api")
                {
                    var data = _nContext.Database.SqlQuery<SupplierDetailApiAirline>($"EXECUTE SupplierCredentialDetail_Proc @ProcNo =1,@SupplierType={SupplierType}, @SupplierID={SupplierID}").ToList();
                    CredentialDT = data.Cast<T>().ToList(); ;//.Select(x => new CredModel { Username = x.UserId, Password = x.Password, SupplierCode="" }).ToList();
                }
                else if (SupplierType.ToLower() == "lcc")
                {
                    var sup = _nContext.SupplierDetailLccAirlines.FirstOrDefault(x => x.SupplierId == SupplierID);
                    if (sup?.CarrierCode?.ToLower() == "6e")
                    {
                        var data = _nContext.Database.SqlQuery<_6EModel>($"EXECUTE SupplierCredentialDetail_Proc @ProcNo =1,@SupplierType={SupplierType}, @SupplierID={SupplierID}").ToList();
                        CredentialDT = data.Cast<T>().ToList();// as List<T>;// data.Select(x => new CredModel { Username = x.NetworkUserName, Password = x.NetworkPassword, SupplierCode=x.CarrierCode }).ToList();
                    }
                    else
                    {
                        var data = _nContext.Database.SqlQuery<SupplierCREDDetailLccAirline>($"EXECUTE SupplierCredentialDetail_Proc @ProcNo =1,@SupplierType={SupplierType}, @SupplierID={SupplierID}").ToList();
                        CredentialDT = data.Cast<T>().ToList();// as List<T>; //data.Select(x => new CredModel { Username = x.LoginId, Password = x.Password, SupplierCode = x.CarrierCode }).ToList();
                    }
                }
                else if (SupplierType.ToLower() == "amd")
                {
                    var data = _nContext.Database.SqlQuery<SupplierDetailAmadeusAirline>($"EXECUTE SupplierCredentialDetail_Proc @ProcNo =1,@SupplierType={SupplierType}, @SupplierID={SupplierID}").ToList();
                    CredentialDT = data.Cast<T>().ToList(); ; //data.Select(x => new CredModel { Username = x.OfficeId, Password = x.Password , SupplierCode = "" }).ToList();
                }
                else if (SupplierType.ToLower() == "gds")
                {
                    var data = _nContext.Database.SqlQuery<SupplierCREDGalileoAirline>($"EXECUTE SupplierCredentialDetail_Proc @ProcNo =1,@SupplierType={SupplierType}, @SupplierID={SupplierID}").ToList();
                    CredentialDT = data.Cast<T>().ToList(); //data.Select(x => new CredModel { Username = x.Userid, Password = x.Password, SupplierCode = "" }).ToList();
                }
                
            }
            catch (Exception ex)
            {
                //Logger.dbLogg("", 0, "AirlineCredentialDetail", "Credential", "", "", ex.Message);
            }

            return CredentialDT;
        }
        
        public List<SupplierDetails> GetSuppliers()
        {

            var result = _nContext.Database.SqlQuery<SupplierDetails>($"EXEC Supplier_Product_Status_Proc @ProcNo=10");
            return result.ToList();
        }

        public List<SupplierApiAirline> InactiveCarriersAPI(string SupplierID)
        {
            var carriers = new List<SupplierApiAirline>();
            try
            {

                var result =   _nContext.Database.SqlQuery<SupplierApiAirline>($"EXEC Supplier_Product_Status_Proc @ProcNo=10");
            }
            catch (Exception ex)
            {
                //dbCommon.Logger.dbLogg("", 0, "Inactive_Carriers_API", "apiCaller", SupplierID, "", ex.Message);
            }
            return carriers;
        }

        //public static DataTable ActiveAirline(string SalesType)
        //{
        //    DataTable CredentialDT = new DataTable();

        //    try
        //    {
        //        using (SqlConnection connection = new SqlConnection(ConnectionString.dbConnect))
        //        {
        //            SqlCommand cmd = new SqlCommand("Supplier_Product_Status_Proc", connection);
        //            cmd.CommandType = CommandType.StoredProcedure;
        //            cmd.CommandTimeout = 10;

        //            cmd.Parameters.Add(@"ProcNo", SqlDbType.Int).Value = 5;

        //            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
        //            adapter.Fill(CredentialDT);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Logger.dbLogg("", 0, "", "", "", "", ex.Message);
        //    }

        //    return CredentialDT;
        //}

        //public static DataTable AirlineCredential(string CarrierCode)
        //{
        //    DataTable CredentialDT = new DataTable();

        //    return CredentialDT;
        //}
        //public static DataTable AirlineCredential(string CarrierCode, string AirlineID)
        //{
        //    DataTable table = new DataTable();

        //    try
        //    {
        //        using (SqlConnection connection = new SqlConnection(ConnectionString.dbConnect))
        //        {
        //            SqlCommand cmd = new SqlCommand("Air_Credential_Proc", connection);
        //            cmd.CommandType = CommandType.StoredProcedure;
        //            cmd.CommandTimeout = 10;

        //            cmd.Parameters.Add(@"ProcNo", SqlDbType.Int).Value = 2;
        //            cmd.Parameters.Add(@"CarrierCode", SqlDbType.VarChar, 2).Value = CarrierCode;
        //            cmd.Parameters.Add(@"AirlineId", SqlDbType.VarChar, 50).Value = AirlineID;

        //            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
        //            adapter.Fill(table);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Logg.dbLogg("", 0, "", "", "", ex.Message);
        //    }

        //    return table;
        //}
    }
}
