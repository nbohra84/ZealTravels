using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ZealTravel.Domain.Interfaces.Handlers;
using ZealTravel.Domain.Data.Entities;
using ZealTravelWebsite.Infrastructure.Context;
using ZealTravel.Application.CredentialManagement.Query;

namespace ZealTravel.Application.CredentialManagement.Handlers
{
    public class GetAirlineApiHandler : IHandlesQueryAsync<List<AirlineApiDetails>>
    {
        private readonly ZealdbNContext _dbContext;
        private readonly IMapper _mapper;

        public GetAirlineApiHandler(ZealdbNContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<List<AirlineApiDetails>> HandleAsync()
        {
            var supplierDetails = await _dbContext.SupplierDetailApiAirlines.ToListAsync();

            if (supplierDetails == null || supplierDetails.Count == 0)
            {
                return new List<AirlineApiDetails>();
            }

            return _mapper.Map<List<AirlineApiDetails>>(supplierDetails);
        }
    }
}
