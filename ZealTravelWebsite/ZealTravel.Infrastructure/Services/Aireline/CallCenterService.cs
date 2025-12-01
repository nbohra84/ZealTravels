using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZealTravel.Domain.Interfaces.CallCenterManagement;
using ZealTravelWebsite.Infrastructure.Context;

namespace ZealTravel.Infrastructure.Services.Aireline
{
    public class CallCenterService : ICallCenterService
    {
        private readonly ZealdbNContext _context;

        public CallCenterService(ZealdbNContext context)
        {
            _context = context;
        }

        public async Task<bool> FallInToAirlineCallCenter(Int32 BookingRef, string CompanyID)
        {
            bool bStatus = false;

            try
            {
                var result = await _context.Database.SqlQuery<int>($"EXEC Airline_CallCenter_Proc @ProcNo = 1, @BookingRef = {BookingRef}, @CompanyID = {CompanyID}").ToListAsync();

                if (result.FirstOrDefault() > 0)
                {
                    bStatus = true;
                }
            }
            catch (Exception ex)
            {
               //dbCommon.Logger.dbLogg(CompanyID, BookingRef, "FallInToAirlineCallCenter", "dbCallCenter", "Airline_CallCenter_Proc", "", ex.Message);
            }

            return bStatus;
        }
    }
}
