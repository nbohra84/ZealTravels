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
    public class GetCompanyDetailsAfterLoginHandler : IHandlesQueryAsync<GetCompanyDetailsAfterLoginQuery, List<CompanyAfterLoginDetails>>
    {
        private readonly IAgencyDashboardService _dashboardService;
        private readonly IMapper _mapper;

        public GetCompanyDetailsAfterLoginHandler(IAgencyDashboardService dashboardService, IMapper mapper)
        {
            _dashboardService = dashboardService;
            _mapper = mapper;
        }

        public async Task<List<CompanyAfterLoginDetails>> HandleAsync(GetCompanyDetailsAfterLoginQuery query)
        {
            var companyDetails = await _dashboardService.GetCompanyDetailAfterLogin(query.AccountID);
            if (companyDetails != null)
            {
                return _mapper.Map<List<CompanyAfterLoginDetails>>(companyDetails.CompanyDetails);
            }
            else
            {
                return null;
            }
        }
    }
}
