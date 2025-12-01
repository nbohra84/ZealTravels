using System;
using System.Threading.Tasks;
using log4net;
using ZealTravel.Application.UserManagement.Queries;
using ZealTravel.Domain.Interfaces.Handlers;
using ZealTravel.Domain.Interfaces.IRepository;
using ZealTravel.Domain.Data.Entities;

namespace ZealTravel.Application.Handlers
{
    public class GetUserDetailsQueryHandler : IHandlesQueryAsync<GetUserDetailsQuery, CompanyRegister?>
    {
        private static readonly ILog _logger = LogManager.GetLogger(typeof(GetUserDetailsQueryHandler));
        private readonly ICompanyRegisterRepository _userRepository;

        public GetUserDetailsQueryHandler(ICompanyRegisterRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<CompanyRegister?> HandleAsync(GetUserDetailsQuery userDetail)
        {
            _logger.Info($"Handling GetUserDetailsQuery for Email: {userDetail.Email}, Host: {userDetail.HostName}");

            try
            {
                var user = await _userRepository.FindAsync(user =>
                    user.Email == userDetail.Email &&
                    user.Pwd == userDetail.Password &&
                    user.Host == userDetail.HostName);

                if (user != null)
                {
                    _logger.Info($"User found: {user.Email} - Company: {user.CompanyName}, UserType: {user.UserType}");
                }
                else
                {
                    _logger.Warn($"No user found for Email: {userDetail.Email}, Host: {userDetail.HostName}");
                }

                return user;
            }
            catch (Exception ex)
            {
                _logger.Error($"Error fetching user details for Email: {userDetail.Email}", ex);
                throw;
            }
        }
    }
}
