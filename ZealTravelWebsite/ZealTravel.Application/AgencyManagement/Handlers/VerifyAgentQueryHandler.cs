using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZealTravel.Application.AgencyManagement.Queries;
using ZealTravel.Domain.AgencyManagement;
using ZealTravel.Domain.Interfaces.Handlers;

namespace ZealTravel.Application.AgencyManagement.Handlers
{
    public class VerifyAgentQueryHandler : IHandlesQueryAsync<VerifyAgentBookingQuery, bool>
    {
        private readonly IAgencyService _agencyService;

        public VerifyAgentQueryHandler(IAgencyService agencyService)
        {
            _agencyService = agencyService;

        }

        public async Task<bool> HandleAsync(VerifyAgentBookingQuery query)
        {
            var result = await _agencyService.VerifyAgentBooking(query.CompanyId, query.BookingRef, query.BookingType);
            return result;
        }
    }
}
