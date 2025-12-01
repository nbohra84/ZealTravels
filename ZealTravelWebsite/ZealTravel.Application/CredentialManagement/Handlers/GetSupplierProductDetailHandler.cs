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
    public class GetSupplierProductDetailHandler : IHandlesQueryAsync<List<SupplierCredProductDetails>>
    {
        private readonly ZealdbNContext _dbContext;
        private readonly IMapper _mapper;
        public GetSupplierProductDetailHandler(ZealdbNContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<List<SupplierCredProductDetails>> HandleAsync()
        {
            try
            {
                var supplierDetails = await _dbContext.SupplierProductDetails.ToListAsync();
                var mappedSupplierDetails = _mapper.Map<List<SupplierCredProductDetails>>(supplierDetails);

                return mappedSupplierDetails;
            }
            catch (Exception ex)
            {
                throw new ApplicationException("An error occurred while retrieving supplier product details.", ex);
            }
        }
    }
}
