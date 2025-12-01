using AutoMapper;
using Microsoft.Extensions.Logging;
using ZealTravel.Application.UserManagement.Commands;
using ZealTravel.Domain.Interfaces.Handlers;
using ZealTravel.Domain.Interfaces.IRepository;

public class CompanyProfileCommandHandler : IHandlesCommandAsync<CompanyProfileCommand>
{
    private readonly ICompanyRegisterRepository _companyRegisterRepository;
    private readonly ILogger<CompanyProfileCommandHandler> _logger;
    private readonly IMapper _mapper;

    // Constructor
    public CompanyProfileCommandHandler(ICompanyRegisterRepository companyRegisterRepository, ILogger<CompanyProfileCommandHandler> logger, IMapper mapper)
    {
        _companyRegisterRepository = companyRegisterRepository;
        _logger = logger;
        _mapper = mapper;
    }

    // Handle method
    public async Task HandleAsync(CompanyProfileCommand command)
    {
        if (command == null)
            throw new ArgumentNullException(nameof(command));

        var companyRegister = await _companyRegisterRepository.FindAsync(c => c.CompanyId == command.CompanyId);
        if (companyRegister == null)
        {
            _logger.LogWarning($"Company with ID {command.CompanyId} not found.");
            throw new Exception($"Company with ID {command.CompanyId} not found.");
        }

        companyRegister.FirstName = command.FirstName ?? companyRegister.FirstName;
        companyRegister.MiddleName = command.MiddleName ?? companyRegister.MiddleName; ;
        companyRegister.LastName = command.LastName ?? companyRegister.LastName;
        companyRegister.CompanyMobile = command.CompanyMobile ?? companyRegister.CompanyMobile;
        companyRegister.CompanyPhoneNo = command.CompanyPhoneNo ?? companyRegister.CompanyPhoneNo;
        companyRegister.PhoneNo = command.PhoneNo ?? companyRegister.PhoneNo;
        companyRegister.Address = command.Address ?? companyRegister.Address;
        companyRegister.State = command.State ?? companyRegister.State;
        companyRegister.City = command.City ?? companyRegister.City;
        companyRegister.PostalCode = command.PostalCode ?? companyRegister.PostalCode;
        companyRegister.PanName = command.PanName ?? companyRegister.PanName;
        companyRegister.PanNo = command.PanNo ?? companyRegister.PanNo;
        companyRegister.Country = command.Country ?? companyRegister.Country;
        companyRegister.Gst = command.Gst ?? companyRegister.Gst;
        companyRegister.GstName = command.GstName ?? companyRegister.GstName;
        companyRegister.Mobile = command.Mobile ?? companyRegister.Mobile;
        companyRegister.YrsInBusiness = command.YrsInBusiness ?? companyRegister.YrsInBusiness;
        companyRegister.TotalEmployee = command.TotalEmployee ?? companyRegister.TotalEmployee;
        companyRegister.TotalBranches = command.TotalBranches ?? companyRegister.TotalBranches;
        companyRegister.AnnualTurnover = command.AnnualTurnover ?? companyRegister.AnnualTurnover;
        companyRegister.MonthlyBookingVolume = command.MonthlyBookingVolume ?? companyRegister.MonthlyBookingVolume;
        companyRegister.ReferredBy = command.ReferredBy ?? companyRegister.ReferredBy;
        companyRegister.Reference = command.Reference ?? companyRegister.Reference;
        companyRegister.EventTime = command.EventTime ?? companyRegister.EventTime;
        companyRegister.CompanyLogo = command.CompanyLogo ?? companyRegister.CompanyLogo;
        companyRegister.CompanyName = command.CompanyName ?? companyRegister.CompanyName;
        companyRegister.AccountId = command.AccountId ?? companyRegister.AccountId;
        companyRegister.Email = command.Email;
        companyRegister.EventTime = command.EventTime ?? companyRegister.EventTime;
        companyRegister.Title = command.Title ?? companyRegister.Title;
        companyRegister.CompanyEmail = command.CompanyEmail;

        await _companyRegisterRepository.UpdateAsync(companyRegister);
    }
}
