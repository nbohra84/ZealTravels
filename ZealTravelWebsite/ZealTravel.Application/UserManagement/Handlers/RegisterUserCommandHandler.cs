using Microsoft.Data.SqlClient;
using System;
using AutoMapper;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZealTravel.Application.UserManagement.Commands;
using ZealTravel.Domain.Data.Entities;
using ZealTravel.Domain.Interfaces.Handlers;
using ZealTravel.Domain.Interfaces.IRepository;
using ZealTravel.Common.Services;
using ZealTravel.Domain.Services;

namespace ZealTravel.Application.UserManagement.Handlers
{
    public class RegisterUserCommandHandler : IHandlesCommandAsync<RegisterUserCommand>
    {
        private readonly ICompanyRegisterRepository _companyRegisterRepository;
        private readonly IMapper _mapper;
        private readonly UserManagementService _userManagementService;
        public RegisterUserCommandHandler(ICompanyRegisterRepository companyRegisterRepository, IMapper mapper, UserManagementService userManagementService)
        {
            _userManagementService = userManagementService ?? throw new ArgumentNullException(nameof(userManagementService));
            _companyRegisterRepository = companyRegisterRepository ?? throw new ArgumentNullException(nameof(companyRegisterRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task HandleAsync(RegisterUserCommand command)
        {
            var existingUser = await _companyRegisterRepository.FindAsync(u => u.Email == command.Email);
            if (existingUser != null) 
            {
                throw new InvalidOperationException("The Email You Have Entered is already Exist");
            }
            string generatedPassword = GeneratePassword();
            command.pwd = generatedPassword;
            var companyRegister = _mapper.Map<CompanyRegister>(command);
            await _userManagementService.RegisterUser(companyRegister);
        }
        private string GeneratePassword()
        {
            Random random = new Random();
            int randomDigits = random.Next(1000, 9999);
            return $"zeal{randomDigits}";
        }

    }
}



