using AutoMapper;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using ZealTravel.Application.UserManagement.Queries;
using ZealTravel.Domain.Interfaces.IRepository;
using ZealTravel.Domain.Data.Entities;
using ZealTravel.Domain.Interfaces.Handlers;
using ZealTravel.Application.AgencyManagement.Queries;

namespace ZealTravel.Application.UserManagement.Handlers
{
    public class GetCompanyProfileHandler : IHandlesQueryAsync<GetCompanyProfileQuery, CompanyProfileData>
    {
        private readonly ICompanyRegisterRepository _companyRegisterRepository;
        private readonly ILogger<GetCompanyProfileHandler> _logger;
        private readonly IMapper _mapper;

        public GetCompanyProfileHandler(ICompanyRegisterRepository companyRegisterRepository, ILogger<GetCompanyProfileHandler> logger, IMapper mapper)
        {
            _companyRegisterRepository = companyRegisterRepository ?? throw new ArgumentNullException(nameof(companyRegisterRepository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<CompanyProfileData> HandleAsync(GetCompanyProfileQuery query)
        {
            if (query == null)
                throw new ArgumentNullException(nameof(query));

            if (string.IsNullOrEmpty(query.CompanyID))
                throw new ArgumentException("Email must be provided.");

            var companyRegister = await _companyRegisterRepository.FindAsync(c => c.CompanyId == query.CompanyID);
            if (companyRegister == null)
            {
                _logger.LogWarning($"Company with CompanyId {query.CompanyID} not found.");
            }

            var response = _mapper.Map<CompanyProfileData>(companyRegister);
            return response;
        }
    }
}
