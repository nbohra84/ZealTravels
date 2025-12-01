using AutoMapper;
using System.Threading.Tasks;
using ZealTravel.Application.AgencyManagement.Queries;
using ZealTravel.Domain.Data.Entities;
using ZealTravel.Domain.Interfaces.Handlers;
using ZealTravel.Domain.Interfaces.IRepository;

namespace ZealTravel.Application.AgencyManagement.Handlers
{
    public class GetProfileQueryHandler : IHandlesQueryAsync<GetProfileQuery, CompanyProfileData>
    {
        private readonly ICompanyRegisterRepository _repository;
        private readonly IMapper _mapper;

        public GetProfileQueryHandler(ICompanyRegisterRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<CompanyProfileData?> HandleAsync(GetProfileQuery query)
        {
            var company = await _repository.FindAsync(c => c.Email == query.Email);
            if (company == null)
            {
                return null; 
            }

            return _mapper.Map<CompanyProfileData>(company);
        }
    }
}
