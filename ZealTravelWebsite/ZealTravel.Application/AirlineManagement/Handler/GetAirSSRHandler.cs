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
using ZealTravel.Infrastructure.Services.DBLogger;

namespace ZealTravel.Application.AirlineManagement.Handler
{
    public class GetAirSSRHandler : IHandlesQueryAsync<GetAirSSRQuery, string>
    {
        IGetAirSSRService _getAirSSRService;
        IDBLoggerService _dbLoggerService;

        public GetAirSSRHandler(IGetAirSSRService getAirSSRService, IDBLoggerService dbLoggerService)
        {
            _getAirSSRService = getAirSSRService;
            _dbLoggerService = dbLoggerService;

        }
        public async Task<string> HandleAsync(GetAirSSRQuery query)
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

                fare = await _getAirSSRService.GetAirSSR(parameters);
            }
            catch (Exception ex)
            {
                await _dbLoggerService.dbLogg(query.CompanyID, 0, " GetAirSSR", "GetAirSSRHandler", query.AirRS, query.SearchID, ex.Message);
            }
            return fare;

        }
    }
}

