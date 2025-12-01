using log4net;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZealTravel.Domain.Data.Models;
using ZealTravel.Domain.Interfaces.AirlineSupplierManagement;
using ZealTravelWebsite.Infrastructure.Context;

namespace ZealTravel.Infrastructure.Services
{
    public class AirSupplierService : IAirlineSupplierService
    {
        private readonly ZealdbNContext _context;
        private static readonly ILog _logger = LogManager.GetLogger(typeof(ReportingService));

        public AirSupplierService(ZealdbNContext context)
        {
            _context = context;
        }

        public async Task<List<AirlineSuppliers>> GetAirSupplierListAsync()
        {
            try
            {
                var suppliers = await _context.Database.SqlQuery<AirlineSuppliers>(
                    $"EXECUTE Supplier_Detail_Proc @ProcNo = 1"
                ).ToListAsync();

                return suppliers;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error retrieving daily booking reports: {ex.Message}", ex);
            }
        }



    }
}
