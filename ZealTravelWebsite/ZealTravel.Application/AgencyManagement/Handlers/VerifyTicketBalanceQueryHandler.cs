using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZealTravel.Application.AgencyManagement.Queries;
using ZealTravel.Domain.AgencyManagement;
using ZealTravel.Domain.Interfaces.Handlers;
using ZealTravel.Domain.Interfaces.IRepository;

namespace ZealTravel.Application.AgencyManagement.Handlers
{
    public class VerifyTicketBalanceQueryHandler : IHandlesQueryAsync<VerifyTicketBalanceQuery, bool>
    {
        private readonly IAgencyService _agencyService;

        public VerifyTicketBalanceQueryHandler(IAgencyService agencyService)
        {
            _agencyService = agencyService;

        }

        public async Task<bool> HandleAsync(VerifyTicketBalanceQuery query)
        {
            var result = await _agencyService.VerifyTicketBalance(query.CompanyID, query.TicketAmount);
            return result;
        }
    }
}
