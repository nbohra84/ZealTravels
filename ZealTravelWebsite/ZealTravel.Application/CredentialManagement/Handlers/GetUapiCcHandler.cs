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
    public class GetUapiCcHandler : IHandlesQueryAsync<List<UapiCcDetails>>
    {
        private readonly ZealdbNContext _dbContext;
        private readonly IMapper _mapper;

        public GetUapiCcHandler(ZealdbNContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<List<UapiCcDetails>> HandleAsync() 
        {
            var supplierDetails = await _dbContext.UapiCcDetails.ToListAsync();

            return _mapper.Map<List<UapiCcDetails>>(supplierDetails);
        }
    }
}
