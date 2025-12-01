using AutoMapper;
using DocumentFormat.OpenXml.ExtendedProperties;
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
    public class GetCompanyPopupDetailsQueryHandler : IHandlesQueryAsync<GetCompanyPopupDetailsQuery, CompanyPopupDetails>
    {
        private readonly ICompanyRegisterRepository _companyRegister;
        private readonly IMapper _mapper;

        public GetCompanyPopupDetailsQueryHandler(ICompanyRegisterRepository companyRegister, IMapper mapper)
        {
            _companyRegister = companyRegister;
            _mapper = mapper;
        }

        public async Task<CompanyPopupDetails> HandleAsync(GetCompanyPopupDetailsQuery query)
        {
            var companyDetails = await _companyRegister.FindAsync(c => c.CompanyId == query.CompanyID);
            if (companyDetails == null)
            {
                return null;
            }
            return _mapper.Map<CompanyPopupDetails>(companyDetails);
        }
    }
}


