using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZealTravel.Application.CredentialManagement.Query;
using ZealTravel.Domain.Interfaces.Handlers;
using ZealTravelWebsite.Infrastructure.Context;

namespace ZealTravel.Application.CredentialManagement.Handlers
{
    public class GetSupplierProductStatusHandler : IHandlesQueryAsync<List<ProductStatusDetails>>
    {
        private readonly ZealdbNContext _dbContext;
        private readonly IMapper _mapper;

        public GetSupplierProductStatusHandler(ZealdbNContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<List<ProductStatusDetails>> HandleAsync()
        {
            try
            {
                var supplierDetails = await _dbContext.SupplierProductStatuses
                    .AsNoTracking()
                    .Join(
                        _dbContext.SupplierProductDetails, // Joining with SupplierProductDetails
                        sps => sps.SupplierId,             // Foreign Key in SupplierProductStatuses
                        spd => spd.SupplierId,             // Primary Key in SupplierProductDetails
                        (sps, spd) => new ProductStatusDetails
                        {
                            Id = sps.Id,
                            SupplierId = sps.SupplierId,
                            SupplierCode = sps.SupplierCode,
                            Pcc = sps.Pcc,
                            Product = sps.Product,
                            B2b = sps.B2b,
                            B2c = sps.B2c,
                            Rt = sps.Rt,
                            Int = sps.Int,
                            MultiCity = sps.MultiCity,
                            ImportPnr = sps.ImportPnr,
                            Pnr = sps.Pnr,
                            Ticketting = sps.Ticketting,
                            SupplierType = spd.SupplierType // Fetching the SupplierType from SupplierProductDetails
                        }
                    )
                    .OrderBy(s => s.Id) // Ordering by ID
                    .ToListAsync();

                return supplierDetails;
            }
            catch (Exception ex)
            {
                throw new ApplicationException("An error occurred while retrieving supplier product details.", ex);
            }
        }
    }
}
