using System.Threading.Tasks;
using ZealTravel.Application.UserManagement.Queries;
using ZealTravel.Domain.Interfaces.Handlers;
using ZealTravel.Domain.Interfaces.IRepository;
using ZealTravel.Domain.Data.Entities;
using AutoMapper;

namespace ZealTravel.Application.Handlers
{
    public class GetCompanyRegisterCorporateUserLimitQueryHandler : IHandlesQueryAsync<string , CompanyRegisterCorporateUserLimitDetails>
    {
        private readonly ICompanyRegisterCorporateUsersLimitRepository _userRepository;
        private readonly IMapper _mapper;

        public GetCompanyRegisterCorporateUserLimitQueryHandler(ICompanyRegisterCorporateUsersLimitRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<CompanyRegisterCorporateUserLimitDetails> HandleAsync(string companyID)
        {
         var corporateUserLimit = await _userRepository.FindAsync(user => user.CompanyId == companyID);
         var user = _mapper.Map<CompanyRegisterCorporateUserLimitDetails>(corporateUserLimit);
            return user;
        }
    }
}



