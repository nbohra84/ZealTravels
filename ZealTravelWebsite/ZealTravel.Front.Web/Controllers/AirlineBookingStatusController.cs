using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using ZealTravel.Application.AgencyManagement.Queries;
using ZealTravel.Application.AirelineManagement.Queries;
using ZealTravel.Application.AirlineManagement.Queries;
using ZealTravel.Application.UserManagement.Handlers;
using ZealTravel.Application.UserManagement.Queries;
using ZealTravel.Common.Helpers;
using ZealTravel.Domain.Data.Entities;
using ZealTravel.Domain.Interfaces.Handlers;
using SpicejetGetServices = ZealTravel.Domain.Interfaces.FlightManagement.Spicejet.IGetServices;
using UniversalGetServices = ZealTravel.Domain.Interfaces.UniversalAPI.IGetServices;

namespace ZealTravel.Front.Web.ApiControllers
{

    [Authorize]

    [Route("AirlineBookingStatus")]
   
    public class AirlineBookingStatusController : ControllerBase
    {
        private readonly SpicejetGetServices _spicejetServices;
        private readonly UniversalGetServices _uapiServices;
        private readonly IWebHostEnvironment _env;
        private readonly IHandlesQueryAsync<GetUAPIAirlineBookingQuery, string> _getUAPIBookingDataHandler;
        private readonly IHandlesQueryAsync<CompanyIdByAccountIdQuery, string> _getCompanyIDByAccountIDQueryHandler;
        private readonly IConfiguration _configuration;

        public AirlineBookingStatusController(
            SpicejetGetServices spicejetServices,
            UniversalGetServices uapiServices,
            IWebHostEnvironment env,
            IHandlesQueryAsync<GetUAPIAirlineBookingQuery, string> getUAPIBookingDataHandler,
            IHandlesQueryAsync<CompanyIdByAccountIdQuery, string> getCompanyIDByAccountIDQueryHandler,
            IConfiguration configuration)
        {
            _spicejetServices = spicejetServices;
            _uapiServices = uapiServices;
            _env = env;
            _getUAPIBookingDataHandler = getUAPIBookingDataHandler;
            _getCompanyIDByAccountIDQueryHandler = getCompanyIDByAccountIDQueryHandler;
            _configuration = configuration;
        }

        /// <summary>
        /// GET: /AirlineBookingStatus/GetSpicejetAirelineStatus?pnr=OCMGTD
        /// </summary>
        [HttpGet("GetSpicejetAirelineStatus")]
        public async Task<IActionResult> GetSpicejetAirelineStatus([FromQuery] string pnr)
        {
            if (!(_env.IsDevelopment() || _env.IsStaging()))
                return NotFound();

            if (string.IsNullOrWhiteSpace(pnr))
                return BadRequest("PNR (record locator) is required.");

            var searchId = HttpContext.Session.GetString("SearchID");
            string supplierid = string.Empty;
            string password = string.Empty;

            try
            {
                var bookingData = await _spicejetServices.GetBookingByRecordLocatorAsync(searchId,supplierid,password, pnr);

                if (string.IsNullOrWhiteSpace(bookingData))
                    return NotFound("Booking data not found.");

                return Ok(bookingData); 
            }
            catch (Exception ex)
            {
                
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

        /// <summary>
        /// GET: /AirlineBookingStatus/GetUAPI6eAirlineBookingStatus?pnr=OCMGTD
        /// </summary>
        [HttpGet("GetUAPI6eAirlineBookingStatus")]
        public async Task<IActionResult> GetUAPI6eAirlineBookingStatus([FromQuery] string universalRecordLocatorCode, string supplier = "IGS2528")
        {
            if (!(_env.IsDevelopment() || _env.IsStaging()))
                return NotFound();

            if (string.IsNullOrWhiteSpace(universalRecordLocatorCode))
                return BadRequest("PNR (record locator) is required.");

            var searchId = await ResolveSearchIdAsync();

            try
            {
                var query = new GetUAPIAirlineBookingQuery(universalRecordLocatorCode, supplier, searchId);

                var bookingData = await _getUAPIBookingDataHandler.HandleAsync(query);

                //var bookingData = await _uapiServices.Get6eBookingData(universalRecordLocatorCode, targetBranch);

                if (string.IsNullOrWhiteSpace(bookingData))
                    return NotFound("Booking data not found.");

                return Ok(bookingData);
            }
            catch (Exception ex)
            {

                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }


        /// <summary>
        /// GET: /AirlineBookingStatus/GetUAPIAirlineBookingStatus?pnr=OCMGTD
        /// </summary>
        [HttpGet("GetUAPIAirlineBookingStatus")]
        public async Task<IActionResult> GetUAPIAirlineBookingStatus([FromQuery] string universalRecordLocatorCode, string supplier = "P7151745")
        {
            if (!(_env.IsDevelopment() || _env.IsStaging()))
                return NotFound();

            if (string.IsNullOrWhiteSpace(universalRecordLocatorCode))
                return BadRequest("PNR (record locator) is required.");

            var searchId = await ResolveSearchIdAsync();


            try
            {
                var query = new GetUAPIAirlineBookingQuery(universalRecordLocatorCode, supplier, searchId);

                var bookingData = await _getUAPIBookingDataHandler.HandleAsync(query);

                if (string.IsNullOrWhiteSpace(bookingData))
                    return NotFound("Booking data not found.");

                return Ok(bookingData);
            }
            catch (Exception ex)
            {

                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }



        /// <summary>
        /// Consolidated searchId, companyId & accountID 
        /// </summary>
        private async Task<string> ResolveSearchIdAsync()
        {
            // Retrieve existing session ID (if needed in future)
            var sessionId = HttpContext.Session.GetString("SearchID");
            string companyId = string.Empty;
            int accountID = 0;

            // Determine account context
            if (_configuration["Company:IsBO"].Equals("0"))
            {
                if (User.Identity.IsAuthenticated)
                {
                    companyId = UserHelper.GetCompanyID(User);
                    accountID = UserHelper.GetStaffAccountID(User);
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(companyId) && int.TryParse(companyId, out var parsedId) && parsedId > 0)
                {
                    var query = new CompanyIdByAccountIdQuery { AccountId = parsedId };
                    companyId = await _getCompanyIDByAccountIDQueryHandler.HandleAsync(query);
                    accountID = parsedId;
                }
            }

            // Always generate a fresh search token
            return $"{Guid.NewGuid()}-{DateTime.Now.Minute}-{DateTime.Now.Second}-{accountID}";
        }
    }
}
