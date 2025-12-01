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
    public class GetGalelioSupplierHandler : IHandlesQueryAsync<List<GalelioSupplierAirline>>
    {
        private readonly ZealdbNContext _dbContext;
        private readonly IMapper _mapper;

        public GetGalelioSupplierHandler(ZealdbNContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<List<GalelioSupplierAirline>> HandleAsync()
        {
            var supplierDetails = await _dbContext.SupplierDetailGalileoAirlines.ToListAsync();

            if (supplierDetails == null || supplierDetails.Count == 0)
            {
                return new List<GalelioSupplierAirline>();
            }

            return _mapper.Map<List<GalelioSupplierAirline>>(supplierDetails);
        }
    }
}
