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
    public class GetAirCommitHandler : IHandlesQueryAsync<GetAirCommitQuery, bool>
    {
        IAirCommitService _airCommitService;
        IDBLoggerService _dbLoggerService;

        public GetAirCommitHandler(IAirCommitService airCommitService, IDBLoggerService dbLoggerService)
        {
            _airCommitService = airCommitService;
            _dbLoggerService = dbLoggerService;

        }
        public async Task<bool> HandleAsync(GetAirCommitQuery query)
        {
            var confirmBooking = false;
            try
            {
                var parameters = new Domain.Models.AirAvaibilityModel
                {
                    AirRQ = query.AirRS,
                    Companyid = query.CompanyID,
                    JourneyType = query.JourneyType,
                    Searchid = query.SearchID,
                    PaymentType = query.PaymentType,
                };

                confirmBooking = await _airCommitService.GetConfirm(parameters, query.BookingRef, query.PassengerRS, query.GstRS,query.PaymentType);
            }
            catch (Exception ex)
            {
                await _dbLoggerService.dbLogg(query.CompanyID, 0, " GetAirCommit", "GetAirCommitHandler", query.AirRS, query.SearchID, ex.Message);
                throw new Exception(ex.Message);
            }
            return confirmBooking;

        }
    }
}

