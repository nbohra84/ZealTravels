using System.Threading.Tasks;
using ZealTravel.Application.UserManagement.Queries;
using ZealTravel.Domain.Interfaces.Handlers;
using ZealTravel.Domain.Interfaces.IRepository;
using ZealTravel.Domain.Data.Entities;
using CompanyRegisterCorporateUserDetails = ZealTravel.Application.UserManagement.Queries.CompanyRegisterCorporateUserDetails;
using AutoMapper;

namespace ZealTravel.Application.Handlers
{
    public class GetCompanyRegisterCorporateUserDetailsQueryHandler : IHandlesQueryAsync<string , CompanyRegisterCorporateUserDetails>
    {
        private readonly ICompanyRegisterCorporateUsersRepository _userRepository;
        private readonly IMapper _mapper;

        public GetCompanyRegisterCorporateUserDetailsQueryHandler(ICompanyRegisterCorporateUsersRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<CompanyRegisterCorporateUserDetails> HandleAsync(string companyID)
        {
         var corporateUser = await _userRepository.FindAsync(user => user.CompanyId == companyID);
         var user = _mapper.Map<CompanyRegisterCorporateUserDetails>(corporateUser);
            return user;
        }
    }
}



