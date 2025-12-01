using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZealTravel.Domain.Interfaces.PNRManagement;
using ZealTravelWebsite.Infrastructure.Context;


namespace ZealTravel.Infrastructure.Services.Aireline
{
    public class PNRService : IPNRService
    {
        private readonly ZealdbNContext _context;

        public PNRService(ZealdbNContext context)
        {
            _context = context;
        }

        public async Task<int> GetPNRstatusByPriceTypeFare(string CompanyID, string Supplierid, string CarrierCode, string Sector, string PriceType)
        {
            int i = -1;
            try
            {
                var result = await _context.Database.SqlQuery<bool>($"EXEC Group_Commission_PriceType_Proc @ProcNo = 7, @Supplierid = {Supplierid}, @CarrierCode = {CarrierCode}, @Sector = {Sector}, @PriceType = {PriceType}").ToListAsync();

                if (result.FirstOrDefault())
                {
                    i = 1;
                }
                else
                {
                    i = 0;
                }
            }
            catch (Exception ex)
            {
                DBCommon.Logger.dbLogg(CompanyID, 0, Supplierid, "getPNRstatusByPriceTypeFare", CarrierCode, Sector, ex.Message);
            }
            return i;
        }
        public async Task<bool> GetPNRstatus(string CompanyID, string CarrierCode, string Sector)
        {
            bool Status = false;

            try
            {
                var result = await _context.Database.SqlQuery<bool>($"EXEC Group_Commission_Rule_New_Proc @ProcNo = 10, @CompanyID = {CompanyID}, @CarrierCode = {CarrierCode}, @Sector = {Sector}").ToListAsync();
                Status = result.FirstOrDefault();
            }
            catch (Exception ex)
            {
                DBCommon.Logger.dbLogg(CompanyID, 0, "", "getPNRstatus", CarrierCode, Sector, ex.Message);
            }

            return Status;
        }
    }
}
