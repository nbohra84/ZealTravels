using ZealTravel.Application.UserManagement.Commands;
using ZealTravel.Domain.Interfaces.Handlers;
using ZealTravel.Domain.Interfaces.IRepository;
using ZealTravel.Common.Services;

public class ForgetPasswordCommandHandler : IHandlesCommandAsync<ForgotPasswordCommand>
{
    private readonly ICompanyRegisterRepository _userRepository;
    private readonly EmailService _emailService;
    public ForgetPasswordCommandHandler(ICompanyRegisterRepository userRepository, EmailService emailService)
    {
        _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
    }

    public async Task HandleAsync(ForgotPasswordCommand command)
    {
        if (command == null) throw new ArgumentNullException(nameof(command));
        if (string.IsNullOrWhiteSpace(command.Email)) throw new ArgumentException("Email cannot be null or empty.", nameof(command.Email));

        var user = await _userRepository.FindAsync(u => u.Email == command.Email);
        if (user != null)
        {
            if (string.IsNullOrEmpty(user.Pwd))
            {
                throw new InvalidOperationException("User password is null or empty.");
            }
            var emailBody = GenerateEmailBody(user.FirstName, user.Email, user.Pwd, user.CompanyName);
            await EmailService.SendEmail(user.Email, "Password Reset", emailBody);
        }
    }

    private string GenerateEmailBody(string firstName, string email, string password, string companyName)
    {
        var body = "<html><body>";
        body += "<table style='height: auto; width: 680px; background-color: rgba(204, 204, 204, 0.13); margin: 0 auto; border: 1px solid rgba(33, 150, 243, 0.28); border-bottom: 1px; border-top: 1px solid ;'>";
        body += "<tr><td colSpan='2'><h5> Dear " + firstName + ", </h5><h4>Please Check your Account. Following are your account details -</h4></td></tr>";
        body += "<tr><td>Email: " + email + "</td></tr>";
        body += "<tr><td>Password: " + password + "</td></tr>";
        body += "<tr><td><h5>Thanks & Regards,</h5></td></tr>";
        body += "<tr><td><h5>" + companyName + "</h5></td></tr>";
        body += "</table></body></html>";

        return body;
    }
}
