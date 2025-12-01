using Microsoft.AspNetCore.Mvc;
using ZealTravel.Application.AgencyManagement.Queries;
using ZealTravel.Domain.Interfaces.Handlers;
using System.Linq;
using System.Threading.Tasks;
using System;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using ZealTravel.Application.UserManagement.Handlers;
using ZealTravel.Application.UserManagement.Queries;
using ZealTravel.Domain.Data.Models;
using ZealTravel.Backoffice.Web.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Hosting;
using ZealTravel.Application.AgencyManagement.Command;
using ZealTravel.Application.UserManagement.Commands;
using ZealTravel.Common.Helpers;
using AutoMapper;
using Microsoft.Identity.Client;
using log4net.Core;

namespace ZealTravel.Backoffice.Web.Controllers
{
    [Authorize]
    public class AgencyController : Controller
    {
        private readonly IHandlesQueryAsync<SearchAgencyTextQuery, List<SearchAgencyListData>> _searchAgencyBySearchTextQueryHandler;
        private readonly IHandlesQueryAsync<GetStatesQuery, List<GstStates>> _getStatesQueryHandler;
        private readonly IHandlesQueryAsync<GetCitiesByStateQuery, List<CityState>> _getCitiesByStateQueryHandler;
        private readonly ILogger<AccountController> _logger;
        private readonly IHandlesQueryAsync<GetCompanyProfileQuery, CompanyProfileData> _getCompanyProfileHandler;
        private readonly IHandlesCommandAsync<CompanyProfileCommand> _updateCompanyProfileHandler;
        private readonly IMapper _mapper;
        private readonly IHandlesCommandAsync<UpdateLogoCommand> _updateLogoCommandHandler;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IHandlesQueryAsync<AgencyListBySearchTextQuery, List<string>> _getAgencyListbySerachTextQueryHandler;
        private readonly IHandlesQueryAsync<CompanyIdByAccountIdQuery, string> _getCompanyIDByAccountIDQueryHandler;
        private readonly IHandlesQueryAsync<GetAvailableBalanceQuery, decimal> _getAvailableBalanceQueryHandler;
        private readonly IHandlesQueryAsync<GetAgencyBalanceTransactionEventQuery, List<GetAgencyBalanceTransactionEvents>> _getCompanyEventDetailHandler;
        private readonly IHandlesCommandAsync<UpdateBalanceTransactionCommand> _balanceTransactionCommandHandler;
        private readonly IHandlesQueryAsync<VerifyAgentBookingQuery, bool> _verifyAgentQueryHandler;

        public AgencyController(
            IHandlesQueryAsync<SearchAgencyTextQuery, List<SearchAgencyListData>> searchAgencyBySearchTextQueryHandler,
            IHandlesQueryAsync<GetStatesQuery, List<GstStates>> getStatesQueryHandler,
            IHandlesQueryAsync<GetCitiesByStateQuery, List<CityState>> getCitiesByStateQueryHandler,
            ILogger<AccountController> logger,
            IHandlesQueryAsync<GetCompanyProfileQuery, CompanyProfileData> getCompanyProfileQueryHandler,
            IHandlesCommandAsync<CompanyProfileCommand> updateCompanyProfileHandler,
            IMapper mapper,
            IHandlesCommandAsync<UpdateLogoCommand> updateLogoCommandHandler,
            IWebHostEnvironment webHostEnvironment,
            IHandlesQueryAsync<AgencyListBySearchTextQuery, List<string>> getAgencyListbySerachTextQueryHandler,
            IHandlesQueryAsync<CompanyIdByAccountIdQuery, string> getCompanyIDByAccountIDQueryHandler,
            IHandlesQueryAsync<GetAvailableBalanceQuery, decimal> getAvailableBalanceQueryHandler,
            IHandlesQueryAsync<GetAgencyBalanceTransactionEventQuery, List<GetAgencyBalanceTransactionEvents>> getCompanyEventDetailHandler,
            IHandlesCommandAsync<UpdateBalanceTransactionCommand> balanceTransactionCommandHandler,
            IHandlesQueryAsync<VerifyAgentBookingQuery, bool> verifyAgentQueryHandler
            )
        {
            _searchAgencyBySearchTextQueryHandler = searchAgencyBySearchTextQueryHandler;
            _getStatesQueryHandler = getStatesQueryHandler;
            _getCitiesByStateQueryHandler = getCitiesByStateQueryHandler;
            _logger = logger;
            _getCompanyProfileHandler = getCompanyProfileQueryHandler;
            _updateCompanyProfileHandler = updateCompanyProfileHandler;
            _mapper = mapper;
            _updateLogoCommandHandler = updateLogoCommandHandler;
            _webHostEnvironment = webHostEnvironment;
            _getAgencyListbySerachTextQueryHandler = getAgencyListbySerachTextQueryHandler;
            _getCompanyIDByAccountIDQueryHandler = getCompanyIDByAccountIDQueryHandler;
            _getAvailableBalanceQueryHandler = getAvailableBalanceQueryHandler;
            _getCompanyEventDetailHandler = getCompanyEventDetailHandler;
            _balanceTransactionCommandHandler = balanceTransactionCommandHandler;
            _verifyAgentQueryHandler = verifyAgentQueryHandler;
        }


        [HttpGet]
        [Route("agency/searchAgency")]
        public async Task<IActionResult> GetAgencyList(string searchText)
        {
            try
            {
                var accountIdString = User.FindFirstValue("AccountID");

                if (string.IsNullOrEmpty(accountIdString) || !int.TryParse(accountIdString, out int accountId))
                {
                    return BadRequest("Invalid or missing Account ID in session.");
                }
                //var accountId = 101;
                var query = new AgencyListBySearchTextQuery { AccountId = accountId, SearchText = searchText };
                var agencies = await _getAgencyListbySerachTextQueryHandler.HandleAsync(query);

                if (!agencies.Any())
                {
                    return NotFound("No agencies found with the provided search text.");
                }

                return Ok(agencies);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }

        public async Task<IActionResult> GetAllStates()
        {
            var query = new GetStatesQuery();
            var cityStates = await _getStatesQueryHandler.HandleAsync(query);

            if (cityStates == null || !cityStates.Any())
            {
                return NotFound("No states found.");
            }

            var states = cityStates.Select(state => new
            {
                StateCode = state.StateCode,
                StateName = state.State
            }).ToList();

            return Ok(states);
        }

        public async Task<IActionResult> GetCityByState(string stateName = null)
        {
            var query = new GetCitiesByStateQuery(stateName);
            var cityStates = await _getCitiesByStateQueryHandler.HandleAsync(query);

            if (cityStates == null || !cityStates.Any())
            {
                return NotFound("No states found.");
            }

            return Ok(new { cities = cityStates });
        }

        [HttpGet]
        [Route("agency/search")]
        public IActionResult SearchAgency()
        {
            var model = new SearchAgencyResponse
            {
                AgencyData = new List<SearchAgencyData>()
            };
            return View(model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("agency/search")]
        public async Task<IActionResult> SearchAgency(SearchAgencyResponse model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var companyId = User.FindFirst("CompanyId")?.Value;

            if (string.IsNullOrEmpty(companyId))
            {
                ModelState.AddModelError(string.Empty, "Unable to retrieve company ID from session.");
                return View(model);
            }
            if (string.IsNullOrWhiteSpace(model.CompanyName) &&
        string.IsNullOrWhiteSpace(model.AccountId.ToString()) &&
        string.IsNullOrWhiteSpace(model.State) &&
        string.IsNullOrWhiteSpace(model.City))
            {
                model.ShowNoInputError = true; // Set the flag
                return View(model); ;
            }

            var queryStateSearch = new GetStatesQuery();
            var cityStates = await _getStatesQueryHandler.HandleAsync(queryStateSearch);

            if (cityStates == null || !cityStates.Any())
            {
                ModelState.AddModelError(string.Empty, "No states found.");
                return View(model);
            }

            if (string.IsNullOrEmpty(model.State))
            {
                model.State = null;
            }

            var selectedState = !string.IsNullOrEmpty(model.State)
              ? cityStates.FirstOrDefault(state => state.StateCode == model.State)
              : null;

            var query = new SearchAgencyTextQuery
            {
                AccountId = model.AccountId,
                CompanyID = companyId,
                CompanyName = model.CompanyName,
                State = selectedState?.State,
                City = model.City
            };

            var agencies = await _searchAgencyBySearchTextQueryHandler.HandleAsync(query);

            if (agencies == null || !agencies.Any())
            {
                ModelState.AddModelError(string.Empty, "No agencies found for the given criteria.");
                return View(model);
            }

            model.AgencyData = agencies.Select((agency, index) => new SearchAgencyData
            {
                ID = index + 1,
                CompanyName = agency.CompanyName,
                AccountID = agency.AccountID,
                City = agency.City,
                UserType = agency.UserType,
                Active_Status = agency.Active_Status,
                EventTime = agency.EventTime,
                AvailableBalance = agency.AvailableBalance
            }).ToList();

            return View(model);
        }

        [HttpGet]
        [Authorize]
        [Route("agency/Profile")]
        public async Task<IActionResult> AgencyProfiles([FromQuery] string value)
        {
           
            string decodedValue = EncodeDecodeHelper.DecodeFrom64(value.Trim());

            if (!int.TryParse(decodedValue, out int accountId))
            {
                _logger.LogWarning($"Invalid account ID received: {decodedValue}");
            }

            var query = new CompanyIdByAccountIdQuery { AccountId = accountId };
            var companyId = await _getCompanyIDByAccountIDQueryHandler.HandleAsync(query);

            if (string.IsNullOrWhiteSpace(companyId))
            {
                _logger.LogWarning($"Company ID not found for Account ID {accountId}.");
            }

            var companyProfileData = await _getCompanyProfileHandler.HandleAsync(new GetCompanyProfileQuery
            {
                CompanyID = companyId
            });

          

            var model = _mapper.Map<CompanyProfiles>(companyProfileData);
            model.AccountIdValue = value;

            var states = await _getStatesQueryHandler.HandleAsync(new GetStatesQuery());
            model.States = states.Select(s => new SelectListItem
            {
                Value = s.StateCode,
                Text = s.State,
                Selected = s.StateCode == companyProfileData.State
            }).ToList();


            var cityStates = await _getCitiesByStateQueryHandler.HandleAsync(new GetCitiesByStateQuery(model.State));
            if (cityStates != null && cityStates.Any())
            {
                var cities = cityStates.SelectMany(cs => cs.Cities)
                                       .Distinct()
                                       .Select(cityName => new SelectListItem
                                       {
                                           Value = cityName,
                                           Text = cityName,
                                           Selected = cityName == model.City
                                       }).ToList();

                model.Cities = cities;
            }
            else
            {
                model.Cities = new List<SelectListItem>();
            }


            return View("~/Views/Agency/AgencyProfile.cshtml", model);
        }





        [HttpPost]
        [Route("agency/Profile")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AgencyProfiles(CompanyProfiles model)
        {
            try
            {
                string decodedValue = EncodeDecodeHelper.DecodeFrom64(model.AccountIdValue.Trim());

                if (!int.TryParse(decodedValue, out int accountId))
                {
                    _logger.LogWarning($"Invalid account ID received: {decodedValue}");
                    TempData["ErrorMessage"] = "Invalid account ID.";
                }

                // Fetch Company ID using the Account ID
                var query = new CompanyIdByAccountIdQuery { AccountId = accountId };
                var companyId = await _getCompanyIDByAccountIDQueryHandler.HandleAsync(query);

                if (string.IsNullOrWhiteSpace(companyId))
                {
                    _logger.LogWarning($"Company ID not found for Account ID {accountId}.");
                    TempData["ErrorMessage"] = "Company ID not found.";
                }

                // Fetch current company profile data
                var currentCompanyProfile = await _getCompanyProfileHandler.HandleAsync(new GetCompanyProfileQuery
                {
                    CompanyID = companyId
                });

                if (currentCompanyProfile == null)
                {
                    _logger.LogWarning($"Company profile not found for CompanyId {companyId}.");
                }

                // Handle logo file upload
                if (model.LogoFile != null && model.LogoFile.Length > 0)
                {
                    string uploadPath = Path.Combine(_webHostEnvironment.WebRootPath, "assets", "company", "logo");
                    string uniqueFileName = $"{companyId}@{Guid.NewGuid()}{Path.GetExtension(model.LogoFile.FileName)}";

                    await FileUploadHelper.UploadFileAsync(uploadPath, model.LogoFile.OpenReadStream(), uniqueFileName);

                    string logoUrl = $"/assets/company/logo/{uniqueFileName}";
                    model.CompanyLogo = logoUrl;

                    var updateLogoCommand = new UpdateLogoCommand
                    {
                        CompanyID = companyId,
                        CompanyLogo = model.CompanyLogo
                    };

                    await _updateLogoCommandHandler.HandleAsync(updateLogoCommand);
                    TempData["SuccessMessage"] = "Logo updated successfully.";
                }

                // Update company profile
                var command = new CompanyProfileCommand
                {
                    CompanyId = companyId,
                    FirstName = model.FirstName ?? "",
                    MiddleName = model.MiddleName ?? "",
                    LastName = model.LastName ?? "",
                    CompanyMobile = model.CompanyMobile ?? "",
                    CompanyName = model.CompanyName ?? "",
                    CompanyPhoneNo = model.CompanyPhoneNo ?? "",
                    Address = model.Address ?? "",
                    State = model.State,
                    City = model.City,
                    PostalCode = model.PostalCode,
                    PanName = model.PanName ?? "",
                    PanNo = model.PanNo ?? "",
                    Country = model.Country,
                    Gst = model.Gst ?? "",
                    GstName = model.GstName ?? "",
                    Mobile = model.Mobile ?? "",
                    PhoneNo = model.PhoneNo ?? "",
                    YrsInBusiness = model.YrsInBusiness ?? 0,
                    TotalEmployee = model.TotalEmployee ?? 0,
                    TotalBranches = model.TotalBranches ?? 0,
                    AnnualTurnover = model.AnnualTurnover ?? 0,
                    MonthlyBookingVolume = model.MonthlyBookingVolume,
                    ReferredBy = model.ReferredBy,
                    Reference = model.Reference,
                    EventTime = model.EventTime,
                    CompanyLogo = model.CompanyLogo,
                    AccountId = model.AccountId,
                    Email = model.Email ?? "",
                    CompanyEmail = model.CompanyEmail ?? "",
                    Title = model.Title,
                };

                await _updateCompanyProfileHandler.HandleAsync(command);
                TempData["SuccessMessage"] = "Company profile updated successfully!";

                // Fetch updated company profile data
                var updatedCompanyProfileData = await _getCompanyProfileHandler.HandleAsync(new GetCompanyProfileQuery
                {
                    CompanyID = companyId
                });

                if (updatedCompanyProfileData == null)
                {
                    _logger.LogWarning($"Company profile not found after update for CompanyId {companyId}.");
                }

                model.CompanyName = updatedCompanyProfileData.CompanyName;

                return View("~/Views/Agency/AgencyProfile.cshtml", model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating company profile.");
                TempData["ErrorMessage"] = "Failed to update profile.";
            }

            return View("~/Views/Agency/AgencyProfile.cshtml", model);
        }

        [HttpPost]
        public async Task<IActionResult> GetTransactionEvents([FromBody] string eventType)
        {
            if (string.IsNullOrWhiteSpace(eventType))
            {
                return BadRequest("EventType is required.");
            }

            var events = await _getCompanyEventDetailHandler.HandleAsync(new GetAgencyBalanceTransactionEventQuery { EventType = eventType });

            if (events == null || !events.Any())
            {
                return NotFound("No events found.");
            }

            return Ok(new { EventDetails = events });
        }





        [HttpGet]
        [Route("agency/balancetransaction")]
        public IActionResult BalanceTransaction()
        {
            try
            {
                ModelState.Clear();
                var model = new BalanceTransactionResponse();

                return View(model);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Error = "An error occurred while processing your request.", Details = ex.Message });
            }
        }


        [HttpGet]   
        [Route("agency/getcompanybalancebyaccountid")]
        public async Task<IActionResult> GetCompanyBalancebyAccountID(string companyNameWithAccountId)
        {
            try
            {
                if (string.IsNullOrEmpty(companyNameWithAccountId))
                    return BadRequest("Company name is required.");

                string[] compin = companyNameWithAccountId.Split('[');
                string[] datare = compin[1].Split(']');
                string accountId = datare[0];

                var query = new CompanyIdByAccountIdQuery { AccountId = Convert.ToInt32(accountId) };
                var companyId = await _getCompanyIDByAccountIDQueryHandler.HandleAsync(query);

                var balanceQuery = new GetAvailableBalanceQuery(companyId);
                var availableBalance = await _getAvailableBalanceQueryHandler.HandleAsync(balanceQuery);

                return Ok(new { AvailableBalance = availableBalance.ToString("F2") });

            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Error = "An error occurred while fetching balance.", Details = ex.Message });
            }
        }





        [HttpPost]
        [Route("agency/balancetransaction")]
        public async Task<IActionResult> BalanceTransaction(BalanceTransactionResponse model)
        {
            try
            {
                var companyId = User.FindFirstValue("CompanyID");

                if (!string.IsNullOrEmpty(model.CompanyNameWithAccountId))
                {
                    string[] compin = model.CompanyNameWithAccountId.Split('[');
                    string[] datare = compin[1].Split(']');
                    string accountId = datare[0];

                    var query = new CompanyIdByAccountIdQuery { AccountId = Convert.ToInt32(accountId) };
                    companyId = await _getCompanyIDByAccountIDQueryHandler.HandleAsync(query);
                }

                model.CompanyId = companyId;

                string BookingType = string.Empty;
                if (model.PaymentType == "Hotel")
                {
                    model.IsHotel = true;
                    model.IsAirline = false;
                    BookingType = "HOTEL";
                }
                else if (model.PaymentType == "Flight")
                {
                    model.IsAirline = true;
                    model.IsHotel = false;
                    BookingType = "FLIGHT";
                }

                var verifyQuery = new VerifyAgentBookingQuery
                {
                    CompanyId = companyId,
                    BookingRef = model.BookingRef,
                    BookingType = BookingType
                };

                bool isVerified = await _verifyAgentQueryHandler.HandleAsync(verifyQuery);

                if (!isVerified)
                {
                    TempData["ErrorMessage"] = "BookingRef is not matched with company !!!";
                    var balanceQuerys = new GetAvailableBalanceQuery(companyId);
                    var availableBalances = await _getAvailableBalanceQueryHandler.HandleAsync(balanceQuerys);
                    var partialModel = new BalanceTransactionResponse
                    {
                        AvailableBalance = availableBalances,
                        CompanyNameWithAccountId = model.CompanyNameWithAccountId
                    };

                    return View(partialModel);
                }

                var command = new UpdateBalanceTransactionCommand
                {
                    CompanyId = companyId,
                    TransactionType = model.TransactionType,
                    EventId = model.EventId,
                    TransactionAmount = Convert.ToDecimal(model.TransactionAmount),
                    BookingRef = model.BookingRef,
                    UpdatedBy = User.FindFirstValue("CompanyID"),
                    Remark = model.Remark,
                    IsAirline = model.IsAirline,
                    IsHotel = model.IsHotel,
                    PassengerId = model.PassengerId
                };

                await _balanceTransactionCommandHandler.HandleAsync(command);
                var balanceQuery = new GetAvailableBalanceQuery(companyId);
                var availableBalance = await _getAvailableBalanceQueryHandler.HandleAsync(balanceQuery);

                // Reset the form fields after successful submission
                var resetModel = new BalanceTransactionResponse
                {
                    AvailableBalance = availableBalance,
                    CompanyNameWithAccountId = model.CompanyNameWithAccountId,
                    PaymentType = string.Empty,
                    BookingRef = 0,
                    TransactionType = string.Empty,
                    TransactionAmount = string.Empty,
                    Remark = string.Empty,
                    IsAirline = false,
                    IsHotel = false,
                };
                ModelState.Clear();
                TempData["SuccessMessage"] = "Balance updated successfully!";
                return View(resetModel);
            }
            catch (Exception ex)
            {
                return View(model);
            }
        }


    }
}