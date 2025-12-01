using Azure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZealTravel.Application.AirelineManagement.Queries;
using ZealTravel.Application.AirlineManagement.Queries;
using ZealTravel.Domain.Interfaces.AirelineManagement;
using ZealTravel.Domain.Interfaces.AirlineManagement;
using ZealTravel.Domain.Interfaces.DBCommon;
using ZealTravel.Domain.Interfaces.Handlers;

namespace ZealTravel.Application.AirlineManagement.Handler
{
    public class GetAirFareHandler : IHandlesQueryAsync<GetAirFareQuery, string>
    {
        IGetAirFareService _getAirFareService;
        IDBLoggerService _dbLoggerService;

        public GetAirFareHandler(IGetAirFareService getAirFareService, IDBLoggerService dbLoggerService)
        {
            _getAirFareService = getAirFareService;
            _dbLoggerService = dbLoggerService;
        }
        public async Task<string> HandleAsync(GetAirFareQuery query)
        {
            var fare = string.Empty;
            try
            {
                var parameters = new Domain.Models.AirAvaibilityModel
                {
                    AirRQ = query.AirRS,
                    Companyid = query.CompanyID,
                    JourneyType = query.JourneyType,
                    Searchid = query.SearchID
                };

                fare = await _getAirFareService.GetAirFareQuote(parameters);
            }
            catch (Exception ex)
            {
               await _dbLoggerService.dbLogg(query.CompanyID, 0, " GetAirFare", "GetAirFareHandler", query.AirRS, query.SearchID, ex.Message);
                
            }
            return fare;

        }
    }
}

