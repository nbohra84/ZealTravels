using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Security.Claims;
using System.Threading.Tasks;
using ZealTravel.Application.UserManagement.Queries;
using ZealTravel.Application.Handlers;
using ZealTravel.Front.Web.Models;
using Microsoft.AspNetCore.Identity.Data;
using ZealTravel.Domain.Interfaces.Handlers;
using ZealTravel.Domain.Data.Entities;
using Microsoft.AspNetCore.Authorization;
using ZealTravel.Domain.Interfaces.IRepository;
using ZealTravel.Application.UserManagement.Commands;
using ZealTravel.Application.UserManagement.Handlers;
using ZealTravel.Infrastructure.Repository;
using ZealTravel.Application.AgencyManagement.Command;
using ZealTravel.Infrastructure.Services;
using System.Data;
using ZealTravel.Domain.Data.Models;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using ZealTravel.Application.AgencyManagement.Handlers;
using ZealTravel.Application.AgencyManagement.Queries;
using System.Text.Json;
using log4net.Core;

namespace ZealTravel.Front.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly IHandlesQueryAsync<GetUserDetailsQuery, CompanyRegister> _getUserDetailsQueryHandler;
        private readonly IHandlesCommandAsync<ForgotPasswordCommand> _forgetPasswordCommandHandler;
        private readonly IHandlesCommandAsync<ChangePasswordCommand> _changePasswordCommandHandler;
        private readonly IHandlesCommandAsync<RegisterUserCommand> _registerUserCommandHandler;
        private readonly ILogger<AccountController> _logger;
        private readonly IWebHostEnvironment _env;
        private readonly ICompanyRegisterRepository _userRepository;
        private readonly IHandlesQueryAsync<GetStatesQuery, List<GstStates>> _getStatesQueryHandler;
        private readonly IHandlesQueryAsync<GetCitiesByStateQuery, List<CityState>> _getCitiesByStateQueryHandler;
        private readonly IHandlesQueryAsync<GetAvailableBalanceQuery, decimal> _getAvailableBalanceQueryHandler;
        private readonly IHandlesQueryAsync<GetCompanyDetailsAfterLoginQuery, List<CompanyAfterLoginDetails>> _getCompanyDetailsAfterLoginHandler;


        public AccountController(
            ICompanyRegisterRepository userRepository,
            IHandlesQueryAsync<GetUserDetailsQuery, CompanyRegister> getUserDetailsQueryHandler,
            ILogger<AccountController> logger, 
            IWebHostEnvironment env, 
            IHandlesCommandAsync<ForgotPasswordCommand> forgetPasswordCommandHandler, 
            IHandlesCommandAsync<RegisterUserCommand> registerUserCommandHandler, 
            IHandlesCommandAsync<ChangePasswordCommand> changePasswordCommandHandler,
            IHandlesQueryAsync<GetStatesQuery, List<GstStates>> getStatesQueryHandler, 
            IHandlesQueryAsync<GetCitiesByStateQuery, List<CityState>> getCitiesByStateQueryHandler, 
            IHandlesQueryAsync<GetAvailableBalanceQuery, decimal> getAvailableBalanceQueryHandler, 
            IHandlesQueryAsync<GetCompanyDetailsAfterLoginQuery, List<CompanyAfterLoginDetails>> getCompanyDetailsAfterLoginHandler)
        {
            _getUserDetailsQueryHandler = getUserDetailsQueryHandler;
            _forgetPasswordCommandHandler = forgetPasswordCommandHandler;
            _changePasswordCommandHandler = changePasswordCommandHandler;
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            _registerUserCommandHandler = registerUserCommandHandler;
            _logger = logger;
            _env = env;
            _getStatesQueryHandler = getStatesQueryHandler;
            _getCitiesByStateQueryHandler = getCitiesByStateQueryHandler;
            _getAvailableBalanceQueryHandler = getAvailableBalanceQueryHandler;
            _getCompanyDetailsAfterLoginHandler = getCompanyDetailsAfterLoginHandler;

        }


        [HttpGet]
        public IActionResult Login([FromQuery] string? ReturnUrl)
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Flight");
            }

            Login login = new Login
            {
                ReturnUrl = ReturnUrl
            };
            return View();
        }

        [HttpPost]

        public async Task<IActionResult> Login(Login login)
        {
            _logger.LogInformation("Login attempt for {Email}", login.Email);

            if (ModelState.IsValid)
            {
                var hostName = HttpContext.Request.Host.Host;
                var query = new GetUserDetailsQuery
                {
                    Email = login.Email,
                    Password = login.Password,
                    HostName = hostName,
                };

                var user = await _getUserDetailsQueryHandler.HandleAsync(query);

                if (user != null)
                {
                    if (user.UserType != "B2B")
                    {
                        ModelState.AddModelError("", "Unauthorized access");
                        return View();
                    }

                    _logger.LogInformation("Login successful for {Email} from Host {HostName}", login.Email, hostName);

                    var companyDetails = await _getCompanyDetailsAfterLoginHandler.HandleAsync(new GetCompanyDetailsAfterLoginQuery { AccountID = user.AccountId.Value });
                    var companyDetailsJson = companyDetails != null && companyDetails.Any() ? JsonSerializer.Serialize(companyDetails.FirstOrDefault()) : string.Empty;

                    var claims = new[]
                    {
                        new Claim(ClaimTypes.Name, user.Email),
                        new Claim("CompanyId", user.CompanyId),
                        new Claim("AccountID", user.AccountId.ToString()),
                        new Claim("CompanyName", user.CompanyName),
                        new Claim("CompanyDetails", companyDetailsJson)
                    };

                    var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    var authProperties = new AuthenticationProperties
                    {
                        IsPersistent = true,
                        ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(60)
                    };

                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);
                    // Get company details after logn

                   


                    if (!string.IsNullOrEmpty(login.ReturnUrl))
                    {
                        return Redirect(login.ReturnUrl);
                    }
                    return RedirectToAction("Index", "Flight");
                }

                _logger.LogWarning("Login failed for {Email}", login.Email);
                ModelState.AddModelError("", "Username Or Password Does not Match");
            }
            else
            {
                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                {
                    _logger.LogWarning("Validation error: {ErrorMessage}", error.ErrorMessage);
                }
                _logger.LogWarning("Login model validation failed for {Email}", login.Email);
            }

            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            _logger.LogInformation("Logout method called");

            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            Response.Cookies.Delete(CookieAuthenticationDefaults.AuthenticationScheme);
            Response.Cookies.Delete(".AspNetCore.Antiforgery");

            _logger.LogInformation("Cookies deleted");

            return RedirectToAction("Login", "Account");
        }

        [HttpGet]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPassword model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var command = new ForgotPasswordCommand
                    {
                        Email = model.Email
                    };

                    await _forgetPasswordCommandHandler.HandleAsync(command);

                    return Ok();
                }
                else
                {
                    return BadRequest();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in ForgotPassword action: {ex.Message}");
                return StatusCode(500, new { Error = $"An error occurred. Please try again." });
            }
        }

        [HttpGet]
        [Route("/Registration")]
        public IActionResult RegisterAgency()
        {
            return View("RegisterAgency");
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
                stateCode = state.StateCode,
                stateName = state.State
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

            return Ok(new { Cities = cityStates });
        }




        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("/Registration")]
        public async Task<IActionResult> RegisterAgency(Registration model)
        {
            if (!ModelState.IsValid)
            {
                foreach (var state in ModelState.Values)
                {
                    foreach (var error in state.Errors)
                    {
                        _logger.LogError($"Validation error: {error.ErrorMessage}");
                    }
                }
                return View(model);
            }
            var hostName = HttpContext.Request.Host.Host;
            var command = new RegisterUserCommand
            {
                Title = model.Title,
                Email = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName,
                CompanyName = model.CompanyName,
                Address = model.Address,
                Country = model.Country,
                State = model.State,
                City = model.City,
                PostalCode = model.PostalCode,
                GST = model.GST,
                PanNo = model.PanNo,
                Pan_Name = model.Pan_Name,
                Company_Mobile = model.Company_Mobile,
                Company_PhoneNo = model.Company_PhoneNo,
                Host = hostName,
                UserType = "B2B"
            };

            try
            {
                await _registerUserCommandHandler.HandleAsync(command);
                TempData["SuccessMessage"] = "Thank you for registering with us. We have sent you an email with your login ID and password. Please change your password after logging in to your account. If you haven't received our email, please check your spam folder.";
                return RedirectToAction("RegisterAgency", new { success = true });
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError($"Registration failed: {ex.Message}");
                ModelState.AddModelError("", "The Email You Have Entered is already Exist");
                return View("RegisterAgency", model);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Registration failed: {ex.Message}");
                ModelState.AddModelError("", "Registration failed. Please try again");
                return View("RegisterAgency", model);
            }
        }
        [HttpGet]
        [Authorize]
        [Route("agency/changepassword")]
        public async Task<IActionResult> ChangePassword()
        {
            var companyId = User.FindFirst("CompanyId")?.Value;
            if (companyId != null)
            {
                var balanceQuery = new GetAvailableBalanceQuery(companyId);
                var availableBalance = await _getAvailableBalanceQueryHandler.HandleAsync(balanceQuery);
                ViewBag.AvailableBalance = availableBalance;
            }
            return View("~/Views/Agency/ChangePassword.cshtml");
        }


        [HttpPost]
        [Route("agency/changepassword")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(ChangePassword model)
        {
            if (!ModelState.IsValid)
            {
                return View("~/Views/Agency/ChangePassword.cshtml", model);
            }

            try
            {
                var command = new ChangePasswordCommand
                {
                    Email = User.FindFirstValue(ClaimTypes.Name),
                    OldPassword = model.OldPassword,
                    NewPassword = model.NewPassword
                };

                await _changePasswordCommandHandler.HandleAsync(command);

                ViewBag.SuccessMessage = "Password successfully changed.";

                var companyId = User.FindFirst("CompanyId")?.Value;
                if (companyId != null)
                {
                    var balanceQuery = new GetAvailableBalanceQuery(companyId);
                    var availableBalance = await _getAvailableBalanceQueryHandler.HandleAsync(balanceQuery);
                    ViewBag.AvailableBalance = availableBalance;
                }

                return View("~/Views/Agency/ChangePassword.cshtml", model);
            }
            catch (ArgumentException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                ViewBag.ErrorMessage = ex.Message;
                return View("~/Views/Agency/ChangePassword.cshtml");
            }
        }
    }
}


