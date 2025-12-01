using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZealTravel.Common.CommonUtility;
using ZealTravel.Infrastructure.Context;
using ZealTravel.Domain.Models;


namespace ZealTravel.Infrastructure.Spicejet
{
    class GetDataBaseData
    {
        public DataTable GetSSR(string Searchid, string SSRtype, string Sector, bool Is48Hour, bool Is24Hour, bool Is6Hour, bool IsBoeing, bool IsQ400, bool T0301_1130, bool T1131_1500, bool T1501_1900, bool T1901_2300, bool T2301_0300)
        {
            try
            {
                var db = DatabaseContextFactory.CreateDbContext();
                //var ssrLists = db.Database.SqlQueryRaw<SpicejetSSRDTO>(@"EXEC SpicejetSSR_Proc @ProcNo = 1, @SSRtype = {0}, @Sector = {1}, @Is48Hour = {2}, @Is24Hour = {3}, @Is6Hour = {4}, @IsBoeing = {5}, @IsQ400 = {6}, @T0301_1130 = {7}, @T1131_1500 = {8}, @T1501_1900 = {9}, @T1901_2300 = {10}, @T2301_0300 = {11}", SSRtype, Sector, Is48Hour, Is24Hour, Is6Hour, IsBoeing, IsQ400, T0301_1130, T1131_1500, T1501_1900, T1901_2300, T2301_0300).ToListAsync();
                var ssrList = db.Database.SqlQuery<SpicejetSSRDTO>($"EXEC SpicejetSSR_Proc @ProcNo = 1, @SSRtype = {SSRtype}, @Sector = {Sector}, @Is48Hour = {Is48Hour}, @Is24Hour = {Is24Hour}, @Is6Hour = {Is6Hour}, @IsBoeing = {IsBoeing}, @IsQ400 = {IsQ400}, @T0301_1130 = {T0301_1130}, @T1131_1500 = {T1131_1500}, @T1501_1900 = {T1501_1900}, @T1901_2300 = {T1901_2300}, @T2301_0300 = {T2301_0300}").ToListAsync();
                return ssrList.Result.ToDataTable();
            }
            catch (Exception ex)
            {
                //dbCommon.Logger.dbLogg("", 0, "GetSSR", "air_spicejet", "", Searchid, ex.Message + "," + ex.StackTrace);
            }
            return null;
        }
    }
}

