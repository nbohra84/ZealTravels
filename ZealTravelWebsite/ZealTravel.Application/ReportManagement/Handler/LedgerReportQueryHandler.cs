using AutoMapper;
using ZealTravel.Application.ReportManagement.Queries;
using ZealTravel.Domain.AgencyManagement;
using ZealTravel.Domain.Data.Models;
using ZealTravel.Domain.Interfaces.Handlers;

public class LedgerReportQueryHandler : IHandlesQueryAsync<LedgerReportQuery, List<LedgerReports>>
{
    private readonly IReportingService _reportingService;
    private readonly IMapper _mapper;

    public LedgerReportQueryHandler(IReportingService reportingService, IMapper mapper)
    {
        _reportingService = reportingService;
        _mapper = mapper;
    }

    public async Task<List<LedgerReports>> HandleAsync(LedgerReportQuery query)
    {
        var data = await _reportingService.LedgerDescriptiveReportAsync(
            query.CompanyId,
            query.FromDate,
            query.ToDate,
            query.SearchBy,
            query.TicketSearchType,
            query.SearchByValue,
            query.EventID
        );

        return _mapper.Map<List<LedgerReports>>(data);
    }
}
