using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZealTravel.Application.AirlineManagement.Queries;
using ZealTravel.Domain.Interfaces.AirlineManagement;
using ZealTravel.Domain.Interfaces.DBCommon;
using ZealTravel.Domain.Interfaces.Handlers;
using ZealTravel.Domain.Interfaces.UniversalAPI;

namespace ZealTravel.Application.AirlineManagement.Handler
{
    public class GetUAPIAirlineBookingQueryHandler : IHandlesQueryAsync<GetUAPIAirlineBookingQuery, string>
    {
        IGetAirlineBookingService _getAirBookingservice;
        IDBLoggerService _dbLoggerService;

        public GetUAPIAirlineBookingQueryHandler(
        IGetAirlineBookingService getAirBookingservice,
        IDBLoggerService dbLoggerService)
        {
            _dbLoggerService = dbLoggerService;
            _getAirBookingservice = getAirBookingservice;
        }
        public async Task<string> HandleAsync(GetUAPIAirlineBookingQuery query)
        {
            string confirmBooking = string.Empty;
            try
            {

                confirmBooking = await _getAirBookingservice.GetAirlineBookingData(query.UniversalRecordLocatorCode, query.Supplier, query.SearchId);
            }
            catch (Exception ex)
            {
                //await _dbLoggerService.dbLogg(query.CompanyID, 0, " GetAirCommit", "GetAirCommitHandler", query.AirRS, query.SearchID, ex.Message);
                throw new Exception(ex.Message);
            }
            return confirmBooking;
        }
    }
}
