using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ZealTravel.Application.CredentialManagement.Query;
using ZealTravel.Domain.Interfaces.Handlers;
using ZealTravelWebsite.Infrastructure.Context;

namespace ZealTravel.Application.CredentialManagement.Handlers
{
    public class GetUapiFopHandler : IHandlesQueryAsync<List<UapiFopDetail>>
    {
        private readonly ZealdbNContext _dbContext;
        private readonly IMapper _mapper;

        public GetUapiFopHandler(ZealdbNContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<List<UapiFopDetail>> HandleAsync()
        {
            var supplierDetails = await _dbContext.UapiFormOfPayments.ToListAsync();

            if (supplierDetails == null || supplierDetails.Count == 0)
            {
                return new List<UapiFopDetail>();
            }

            return _mapper.Map<List<UapiFopDetail>>(supplierDetails);
        }
    }
}
