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
    public class GetPnrMakeDaysHandler : IHandlesQueryAsync<List<PnrMakeDaysDetails>>
    {
        private readonly ZealdbNContext _dbContext;
        private readonly IMapper _mapper;

        public GetPnrMakeDaysHandler(ZealdbNContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<List<PnrMakeDaysDetails>> HandleAsync()
        {
            try
            {
                var pnrMakeDays = await _dbContext.AirlinePnrMakeDays.ToListAsync();
                return _mapper.Map<List<PnrMakeDaysDetails>>(pnrMakeDays);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error retrieving PNR Make Days details.", ex);
            }
        }
    }
}
