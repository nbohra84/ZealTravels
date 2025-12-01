using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZealTravel.Application.AirelineManagement.Queries;
using ZealTravel.Domain.Interfaces.Akasha;
using ZealTravel.Domain.Interfaces.AirelineManagement;
using ZealTravel.Domain.Interfaces.Handlers;
using ZealTravel.Domain.Interfaces.TBO;
using ZealTravel.Domain.Interfaces.UniversalAPI;

namespace ZealTravel.Application.AirelineManagement.Handler
{
    public class GetAvailableFlightsHandler : IHandlesQueryAsync<SearchFlightAvailabilityQuery, string>
    {
        IGetOneWayFlightService _onewayflightService;
        
        public GetAvailableFlightsHandler(IGetOneWayFlightService onewayFlightService)
        {
            _onewayflightService = onewayFlightService;
        }
        public async Task<string> HandleAsync(SearchFlightAvailabilityQuery query)
        {
            var parameters = new Domain.Models.AirAvaibilityModel
            {
                AirRQ = query.AirRQ,
                Companyid = query.CompanyID,
                EndUserIp = "125.63.102.34",
                JourneyType = query.JourneyType,
                Searchid = query.SearchID
            };
            
            return _onewayflightService.GetAvailableFights(parameters);
        }

        


    }
}
