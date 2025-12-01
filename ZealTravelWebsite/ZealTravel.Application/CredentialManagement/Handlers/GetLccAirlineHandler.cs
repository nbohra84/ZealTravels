using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ZealTravel.Application.CredentialManagement.Query;
using ZealTravel.Domain.Interfaces.Handlers;
using ZealTravelWebsite.Infrastructure.Context;

namespace ZealTravel.Application.CredentialManagement.Handlers
{
    public class GetLccAirlineHandler : IHandlesQueryAsync<List<LccSupplierAirline>>
    {
        private readonly ZealdbNContext _dbContext;
        private readonly IMapper _mapper;

        public GetLccAirlineHandler(ZealdbNContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<List<LccSupplierAirline>> HandleAsync()
        {
            try
            {
                var supplierDetails = await _dbContext.SupplierDetailLccAirlines.ToListAsync();

                return _mapper.Map<List<LccSupplierAirline>>(supplierDetails);
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while retrieving LCC airline details.", ex);
            }
        }
    }
}
