using DocumentFormat.OpenXml.Wordprocessing;
using log4net;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZealTravel.Domain.Data.Models;
using ZealTravel.Domain.Data.Models.Backoffice;
using ZealTravel.Domain.Interfaces.Backoffice;
using ZealTravel.Infrastructure.Services.Agency;
using ZealTravelWebsite.Infrastructure.Context;

namespace ZealTravel.Infrastructure.Services.Backoffice
{
    public class DashboardService : IDashboardService
    {
        private readonly ZealdbNContext _context;
        private static readonly ILog _logger = LogManager.GetLogger(typeof(DashboardService));

        public DashboardService(ZealdbNContext context)
        {
            _context = context;
        }

        public async Task<List<DashboardAirlinePendingBooking>> GetAirlinePendingBookings()
        {
            try
            {
                var bookings = await _context.Database.SqlQuery<DashboardAirlinePendingBooking>(
                    $"EXECUTE Airline_CallCenter_Proc @ProcNo = 2"
                ).ToListAsync();

                return bookings;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error retrieving airline pending booking reports: {ex.Message}", ex);
            }
        }

        public async Task<List<DashboardAirlineFlightBooking>> GetTopTenAirlineBookings(string companyId)
        {
            try
            {
                var bookings = await _context.Database.SqlQuery<DashboardAirlineFlightBooking>(
                    $"EXECUTE Company_Transaction_Detail_Proc @ProcNo = 6, @CompanyID = {companyId}"
                ).ToListAsync();

                return bookings;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error retrieving airline pending booking reports: {ex.Message}", ex);
            }
        }
    }
}
