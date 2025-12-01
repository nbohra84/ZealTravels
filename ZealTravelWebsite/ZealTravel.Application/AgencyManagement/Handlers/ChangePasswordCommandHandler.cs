using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZealTravel.Application.AgencyManagement.Command;
using ZealTravel.Domain.Interfaces.Handlers;
using ZealTravel.Domain.Interfaces.IRepository;
using ZealTravelWebsite.Infrastructure.Context;


namespace ZealTravel.Application.AgencyManagement.Handlers
{
    public class ChangePasswordCommandHandler : IHandlesCommandAsync<ChangePasswordCommand>
    {
        private readonly ICompanyRegisterRepository _companyRegisterRepository;
        private readonly ZealdbNContext _context;

        public ChangePasswordCommandHandler(ICompanyRegisterRepository companyRegisterRepository, ZealdbNContext context)
        {
            _companyRegisterRepository = companyRegisterRepository ?? throw new ArgumentNullException(nameof(companyRegisterRepository));
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }
        public async Task HandleAsync(ChangePasswordCommand command)
        {
            var user = await _companyRegisterRepository.FindAsync(u => u.Email == command.Email);

            if (user != null)
            {
                if (user.Pwd == command.OldPassword)
                {
                    if (command.NewPassword == user.Email || command.NewPassword == user.CompanyMobile || command.NewPassword == user.CompanyPhoneNo || command.NewPassword == user.Mobile || command.NewPassword == user.PhoneNo)
                    {
                        throw new ArgumentException("Password should not be your email or mobile number.");
                    }
                    user.Pwd = command.NewPassword;
                     await _companyRegisterRepository.UpdateAsync(user);
                }
                else
                {
                    throw new ArgumentException("The old password is incorrect.");
                }
            }
        }

}
    }
