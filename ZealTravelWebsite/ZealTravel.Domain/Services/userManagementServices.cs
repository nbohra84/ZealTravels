using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using ZealTravel.Domain.Data.Entities;
using Microsoft.Extensions.Logging;
using ZealTravel.Domain.Interfaces.IRepository;
using ZealTravel.Common.Services;


namespace ZealTravel.Domain.Services
{
    public class UserManagementService
    {
        private readonly ICompanyRegisterRepository _companyRegisterRepository;
        private readonly IConfiguration _configuration;
        private readonly ILogger<UserManagementService> _logger;
        private readonly EmailService _emailService;
        public UserManagementService(ICompanyRegisterRepository companyRegisterRepository, IConfiguration configuration, ILogger<UserManagementService> logger, EmailService emailService)
        {
            _companyRegisterRepository = companyRegisterRepository;
            _configuration = configuration;
            _logger = logger;
            _emailService = emailService;
        }

        public async Task<bool> RegisterUser(CompanyRegister companyRegister)
        {
            try
            {
                _logger.LogInformation("Starting user registration for email: {Email}", companyRegister.Email);
                await _companyRegisterRepository.AddAsync(companyRegister);
                var emailBody = GenerateRegistrationEmailBody(companyRegister);
                await EmailService.SendEmail(companyRegister.Email, "Registration Successful", emailBody);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred during user registration for email: {Email}", companyRegister.Email);
                return false;
            }
        }
        private string GenerateRegistrationEmailBody(CompanyRegister companyRegister)
        {
            string body;

            if (companyRegister.UserType == "ST")
            {
                body = "<html><body><table style='height: auto; width: 680px; background-color: rgba(204, 204, 204, 0.13); margin: 0 auto; border: 1px solid rgba(33, 150, 243, 0.28); border-bottom: 1px; border-top: 1px solid ;'>";
                body += "<tr><td colspan='2'><h5>Dear " + companyRegister.FirstName + ",</h5><h4>Thank you for registering as a Staff member with us. Below are your account details:</h4></td></tr>";
                body += "<tr><td>Company Name: " + companyRegister.CompanyName + "</td><td>Registration Date: " + DateTime.Now.ToString("MM/dd/yyyy") + "</td></tr>";
                body += "<tr><td>Account ID: " + companyRegister.AccountId + "</td><td>Contact No: " + companyRegister.CompanyMobile + "</td></tr>";
                body += "<tr><td>Email: " + companyRegister.Email + "</td></tr>";
                body += "<tr><td>Password: " + companyRegister.Pwd + "</td></tr>";
                body += "<tr><td>User Type: " + companyRegister.UserType + "</td><td>Staff Type: " + companyRegister.StaffType + "</td></tr>";
                body += "<tr><td><h5>Thanks & Regards,</h5></td></tr>";
                body += "<tr><td><h5>" + companyRegister.CompanyName + "</h5></td></tr>";
                body += "</table></body></html>";
            }
            else if (companyRegister.UserType == "B2B2ST")
            {
                body = "<html><body><table style='height: auto; width: 680px; background-color: rgba(204, 204, 204, 0.13); margin: 0 auto; border: 1px solid rgba(33, 150, 243, 0.28); border-bottom: 1px; border-top: 1px solid ;'>";
                body += "<tr><td colspan='2'><h5>Dear " + companyRegister.FirstName + ",</h5><h4>Thank you for registering as a B2B2 Staff member with us. Below are your account details:</h4></td></tr>";
                body += "<tr><td>Company Name: " + companyRegister.CompanyName + "</td><td>Registration Date: " + DateTime.Now.ToString("MM/dd/yyyy") + "</td></tr>";
                body += "<tr><td>Account ID: " + companyRegister.AccountId + "</td><td>Contact No: " + companyRegister.CompanyMobile + "</td></tr>";
                body += "<tr><td>Email: " + companyRegister.Email + "</td></tr>";
                body += "<tr><td>Password: " + companyRegister.Pwd + "</td></tr>";
                body += "<tr><td>User Type: " + companyRegister.UserType + "</td><td>Staff Type: " + companyRegister.StaffType + "</td></tr>";
                body += "<tr><td><h5>Thanks & Regards,</h5></td></tr>";
                body += "<tr><td><h5>" + companyRegister.CompanyName + "</h5></td></tr>";
                body += "</table></body></html>";
            }
            else if (companyRegister.UserType == "B2B2B2ST")
            {
                body = "<html><body><table style='height: auto; width: 680px; background-color: rgba(204, 204, 204, 0.13); margin: 0 auto; border: 1px solid rgba(33, 150, 243, 0.28); border-bottom: 1px; border-top: 1px solid ;'>";
                body += "<tr><td colspan='2'><h5>Dear " + companyRegister.FirstName + ",</h5><h4>Thank you for registering as a B2B2B2 Staff member with us. Below are your account details:</h4></td></tr>";
                body += "<tr><td>Company Name: " + companyRegister.CompanyName + "</td><td>Registration Date: " + DateTime.Now.ToString("MM/dd/yyyy") + "</td></tr>";
                body += "<tr><td>Account ID: " + companyRegister.AccountId + "</td><td>Contact No: " + companyRegister.CompanyMobile + "</td></tr>";
                body += "<tr><td>Email: " + companyRegister.Email + "</td></tr>";
                body += "<tr><td>Password: " + companyRegister.Pwd + "</td></tr>";
                body += "<tr><td>User Type: " + companyRegister.UserType + "</td><td>Staff Type: " + companyRegister.StaffType + "</td></tr>";
                body += "<tr><td><h5>Thanks & Regards,</h5></td></tr>";
                body += "<tr><td><h5>" + companyRegister.CompanyName + "</h5></td></tr>";
                body += "</table></body></html>";
            }
            else
            {
                body = "<html><body><table style='height: auto; width: 680px; background-color: rgba(204, 204, 204, 0.13); margin: 0 auto; border: 1px solid rgba(33, 150, 243, 0.28); border-bottom: 1px; border-top: 1px solid ;'>";
                body += "<tr><td colspan='2'><h5>Dear " + companyRegister.FirstName + ",</h5><h4>Thank you for registering with us. Below are your account details:</h4></td></tr>";
                body += "<tr><td>Company Name: " + companyRegister.CompanyName + "</td><td>Registration Date: " + DateTime.Now.ToString("MM/dd/yyyy") + "</td></tr>";
                body += "<tr><td>Account ID: " + companyRegister.AccountId + "</td><td>Contact No: " + companyRegister.CompanyMobile + "</td></tr>";
                body += "<tr><td>Email: " + companyRegister.Email + "</td></tr>";
                body += "<tr><td>Password: " + companyRegister.Pwd + "</td></tr>";
                body += "<tr><td><h5>Thanks & Regards,</h5></td></tr>";
                body += "<tr><td><h5>" + companyRegister.CompanyName + "</h5></td></tr>";
                body += "</table></body></html>";
            }

            return body;
        }



    }
}
