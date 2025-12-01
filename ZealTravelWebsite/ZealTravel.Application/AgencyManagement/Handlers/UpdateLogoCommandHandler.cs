using Microsoft.Extensions.Logging;
using ZealTravel.Application.AgencyManagement.Command;
using ZealTravel.Domain.Interfaces.Handlers;
using ZealTravel.Domain.Interfaces.IRepository;

public class UpdateLogoCommandHandler : IHandlesCommandAsync<UpdateLogoCommand>
{
    private readonly ICompanyRegisterRepository _companyRegisterRepository;
    private readonly ILogger<UpdateLogoCommandHandler> _logger;

    public UpdateLogoCommandHandler(ICompanyRegisterRepository companyRegisterRepository, ILogger<UpdateLogoCommandHandler> logger)
    {
        _companyRegisterRepository = companyRegisterRepository ?? throw new ArgumentNullException(nameof(companyRegisterRepository));
        _logger = logger;
    }

    public async Task HandleAsync(UpdateLogoCommand command)
    {
        if (command == null)
            throw new ArgumentNullException(nameof(command));

        if (string.IsNullOrEmpty(command.CompanyID))
            throw new ArgumentException("Company ID must be provided.");

        var companyRegister = await _companyRegisterRepository.FindAsync(c => c.CompanyId == command.CompanyID);
        if (companyRegister == null)
        {
            _logger.LogWarning($"Company with ID {command.CompanyID} not found.");
            throw new Exception($"Company with ID {command.CompanyID} not found.");
        }

        if (string.IsNullOrWhiteSpace(command.CompanyLogo))
            throw new ArgumentException("Logo URL cannot be null or empty.");

        companyRegister.CompanyLogo = command.CompanyLogo;
        await _companyRegisterRepository.UpdateAsync(companyRegister);
    }
}
