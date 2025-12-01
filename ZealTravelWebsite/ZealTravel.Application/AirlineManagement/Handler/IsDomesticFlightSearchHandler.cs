using DocumentFormat.OpenXml.Spreadsheet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZealTravel.Application.AirelineManagement.Queries;
using ZealTravel.Application.AirlineManagement.Queries;
using ZealTravel.Domain.Interfaces.AirelineManagement;
using ZealTravel.Domain.Interfaces.DBCommon;
using ZealTravel.Domain.Interfaces.Handlers;

namespace ZealTravel.Application.AirlineManagement.Handler
{
    public class IsDomesticFlightSearchHandler : IHandlesQueryAsync<IsDomesticFlightSearchQuery, bool>
    {
        IAirlineDetailService _airlineDetailService;

        public IsDomesticFlightSearchHandler(IAirlineDetailService airlineDetailService)
        {
            _airlineDetailService = airlineDetailService;
        }

        public async Task<bool> HandleAsync(IsDomesticFlightSearchQuery query)
        {
            return await _airlineDetailService.IsDomestic(query.Origin, query.Destination);

        }
    }
}
