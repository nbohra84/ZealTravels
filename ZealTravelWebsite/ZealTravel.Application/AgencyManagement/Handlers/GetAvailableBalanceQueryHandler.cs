using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZealTravel.Application.AgencyManagement.Queries;
using ZealTravel.Domain.Interfaces.Handlers;
using ZealTravel.Domain.AgencyManagement;

namespace ZealTravel.Application.AgencyManagement.Handlers
{
    public class GetAvailableBalanceQueryHandler : IHandlesQueryAsync<GetAvailableBalanceQuery, decimal>
    {
        private readonly IAgencyService _agencyService;

        public GetAvailableBalanceQueryHandler(IAgencyService agencyService)
        {
            _agencyService = agencyService;
        }

        public async Task<decimal> HandleAsync(GetAvailableBalanceQuery query)
        {
            return await _agencyService.GetAvailableBalanceAsync(query.CompanyId);
        }
    }
}

