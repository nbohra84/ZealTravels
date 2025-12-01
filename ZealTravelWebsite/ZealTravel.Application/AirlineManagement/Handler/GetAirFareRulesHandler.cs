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
    public class GetAirFareRulesHandler : IHandlesQueryAsync<GetAirFareRulesQuery, string>
    {
        IGetAirFareRulesService _getAirFareRuleService;
        IDBLoggerService _dbLoggerService;

        public GetAirFareRulesHandler(IGetAirFareRulesService getAirFareRuleService, IDBLoggerService dbLoggerService)
        {
            _getAirFareRuleService = getAirFareRuleService;
            _dbLoggerService = dbLoggerService;
        }
        public async Task<string> HandleAsync(GetAirFareRulesQuery query)
        {
            string fareRules = string.Empty;
            try
            {
                var parameters = new Domain.Models.AirAvaibilityModel
                {
                    AirRQ = query.AirRS,
                    Companyid = query.CompanyID,
                    JourneyType = query.JourneyType,
                    Searchid = query.SearchID
                };

                fareRules = await _getAirFareRuleService.GetAirFareRules(parameters);
            }
            catch (Exception ex)
            {
                await _dbLoggerService.dbLogg(query.CompanyID, 0, " GetAirFareRules", "GetAirFareRulesHandler", query.AirRS, query.SearchID, ex.Message);
            }
            return fareRules;

        }
    }
}
