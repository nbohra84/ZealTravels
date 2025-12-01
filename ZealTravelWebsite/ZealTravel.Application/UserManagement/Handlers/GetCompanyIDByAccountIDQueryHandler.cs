using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZealTravel.Application.UserManagement.Queries;
using ZealTravel.Domain.AgencyManagement;
using ZealTravel.Domain.Data.Entities;
using ZealTravel.Domain.Interfaces.Handlers;

namespace ZealTravel.Application.UserManagement.Handlers
{
    public class GetCompanyIDByAccountIDQueryHandler : IHandlesQueryAsync<CompanyIdByAccountIdQuery, string>
    {
        private readonly IUserService _reportingService;

        public GetCompanyIDByAccountIDQueryHandler(IUserService reportingService)
        {
            _reportingService = reportingService;
        }

        public async Task<string> HandleAsync(CompanyIdByAccountIdQuery query)
        {
            var companyId = await _reportingService.GetCompanyIDByAccountID(query.AccountId);

            if (string.IsNullOrEmpty(companyId))
            {
                throw new InvalidOperationException("Company ID not found.");
            }

            return companyId;
        }

    }


}
