using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZealTravel.Domain.Data.Entities;
using ZealTravel.Domain.Interfaces.IRepository;
using ZealTravel.Domain.Interfaces.Whitelabel;
using ZealTravelWebsite.Infrastructure.Context;

namespace ZealTravel.Infrastructure.Repository.Whitelabel
{
    public class WhitelabelDetailRepository : RepositoryBase<WhitelabelDetail>, IWhitelabelDetailRepository
    {
        private readonly ZealdbNContext _context;

        public WhitelabelDetailRepository(ZealdbNContext context) : base(context)
        {

        }
    }
}
