using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZealTravel.Common.Helpers;
using ZealTravelWebsite.Infrastructure.Context;

namespace ZealTravel.Infrastructure.Context
{
    public class DatabaseContextFactory
    {
        private static ZealdbNContext _context;

        public static ZealdbNContext CreateDbContext()
        {
            if (_context == null)
            {
                var optionsBuilder = new DbContextOptionsBuilder<ZealdbNContext>();
                optionsBuilder.UseSqlServer(ConfigurationHelper.GetConnectionString("DefaultConnection"));
                _context = new ZealdbNContext(optionsBuilder.Options);
            }
            return _context;
        }
    }
}
