using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Security.Claims;
using System.Threading.Tasks;
using ZealTravel.Application.UserManagement.Queries;
using ZealTravel.Application.Handlers;
using ZealTravel.Backoffice.Web.Models;
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
using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml.Bibliography;
using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using ZealTravel.Common.Helpers;
using Microsoft.AspNetCore.Mvc.Rendering;
using log4net.Core;

namespace ZealTravel.Backoffice.Web.Controllers
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
        private static IConfiguration _configuration;
        private readonly IHandlesQueryAsync<GetCompanyProfileQuery, CompanyProfileData> _getCompanyProfileHandler;
        private readonly IHandlesCommandAsync<CompanyProfileCommand> _updateCompanyProfileHandler;
        private readonly IMapper _mapper;
        private readonly IHandlesCommandAsync<UpdateLogoCommand> _updateLogoCommandHandler;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public AccountController(ICompanyRegisterRepository userRepository,
            IHandlesQueryAsync<GetUserDetailsQuery, CompanyRegister> getUserDetailsQueryHandler,
            ILogger<AccountController> logger,
            IWebHostEnvironment env,
            IHandlesCommandAsync<ForgotPasswordCommand> forgetPasswordCommandHandler,
            IHandlesCommandAsync<RegisterUserCommand> registerUserCommandHandler,
            IHandlesCommandAsync<ChangePasswordCommand> changePasswordCommandHandler,
            IHandlesQueryAsync<GetStatesQuery, List<GstStates>> getStatesQueryHandler,
            IHandlesQueryAsync<GetCitiesByStateQuery, List<CityState>> getCitiesByStateQueryHandler,
            IHandlesQueryAsync<GetAvailableBalanceQuery, decimal> getAvailableBalanceQueryHandler,
            IConfiguration configuration,
            IHandlesQueryAsync<GetCompanyProfileQuery, CompanyProfileData> getCompanyProfileQueryHandler,
            IHandlesCommandAsync<CompanyProfileCommand> updateCompanyProfileHandler,
            IMapper mapper,
            IHandlesCommandAsync<UpdateLogoCommand> updateLogoCommandHandler,
            IWebHostEnvironment webHostEnvironment) 
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
            _configuration = configuration;
            _getCompanyProfileHandler = getCompanyProfileQueryHandler;
            _updateCompanyProfileHandler = updateCompanyProfileHandler;
            _mapper = mapper;
            _updateLogoCommandHandler = updateLogoCommandHandler;
            _webHostEnvironment = webHostEnvironment;
        }

        [HttpGet]
        public IActionResult Login([FromQuery] string? ReturnUrl)
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Dashboard", "Dashboard");
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
                    if (user.AccessStatus == true) 
                    {
                        if (user.UserType == "AD" || user.UserType == "ST")
                        {
                            _logger.LogInformation("Login successful for {Email} from Host {HostName}",
                                login.Email, hostName);

                            var claims = new[]
                            {
                        new Claim(ClaimTypes.Name, user.Email),
                        new Claim("CompanyId", user.CompanyId),
                        new Claim("AccountID", user.AccountId.ToString()),
                        new Claim("CompanyName", user.CompanyName),
                        new Claim("UserType", user.UserType)
                    };

                            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                            var authProperties = new AuthenticationProperties
                            {
                                IsPersistent = true,
                                ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(60)
                            };

                            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);

                            if (!string.IsNullOrEmpty(login.ReturnUrl))
                            {
                                return Redirect(login.ReturnUrl);
                            }

                            return RedirectToAction("Dashboard", "Dashboard");
                        }
                        else
                        {
                            _logger.LogWarning("Unauthorized login attempt for {Email} with UserType {UserType}", login.Email, user.UserType);
                            ModelState.AddModelError("", "Username or Password does not match.");
                        }
                    }
                    else
                    {
                        _logger.LogWarning("Login attempt for disabled account {Email}", login.Email);
                        ModelState.AddModelError("", "Your account is disabled. Kindly contact our team.");
                    }
                }
                else
                {
                    _logger.LogWarning("Login failed for {Email}", login.Email);
                    ModelState.AddModelError("", "Username or Password does not match.");
                }
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

        //[HttpGet]
        //public IActionResult ForgotPassword()
        //{
        //    return View();
        //}

        //[AllowAnonymous]
        //[HttpPost]
        //public async Task<IActionResult> ForgotPassword([FromBody] ForgotPassword model)
        //{
        //    try
        //    {
        //        if (ModelState.IsValid)
        //        {
        //            var command = new ForgotPasswordCommand
        //            {
        //                Email = model.Email
        //            };

        //            await _forgetPasswordCommandHandler.HandleAsync(command);

        //            return Ok();
        //        }
        //        else
        //        {
        //            return BadRequest();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError($"Error in ForgotPassword action: {ex.Message}");
        //        return StatusCode(500, new { Error = $"An error occurred. Please try again." });
        //    }
        //}

        [HttpGet]
        [Authorize]
        [Route("agency/Register")]
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



        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        [Route("agency/Register")]
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
                UserType = "B2B",
            };

            try
            {
                await _registerUserCommandHandler.HandleAsync(command);
                TempData["SuccessMessage"] = "Thank You for Registering with us.We will Get back to you soon.Your Password Will send to your entered email Id";
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
        [Route("admin/changepassword")]
        public async Task<IActionResult> ChangePassword()
        {
            return View();
        }


        [HttpPost]
        [Authorize]
        [Route("admin/changepassword")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(ChangePassword model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
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

                return View(model);
            }
            catch (ArgumentException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                ViewBag.ErrorMessage = ex.Message;
                return View();
            }
        }



        [HttpGet]
        [Authorize]
        [Route("admin/registerstaff")]
        public IActionResult CreateStaff()
        {
            var companyName = User.FindFirst("CompanyName")?.Value;
            var model = new StaffRegister
            {
                CompanyName = companyName,
            };

            return View("~/Views/Admin/Staff.cshtml", model);
        }





        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("admin/registerstaff")]
        public async Task<IActionResult> CreateStaff(StaffRegister model)
        {
            var companyName = User.FindFirst("CompanyName")?.Value;
            var companyId = User.FindFirst("CompanyId")?.Value;
            if (!ModelState.IsValid)
            {
                foreach (var state in ModelState.Values)
                {
                    foreach (var error in state.Errors)
                    {
                        _logger.LogError($"Validation error: {error.ErrorMessage}");
                    }
                }
                return View("~/Views/Admin/Staff.cshtml", model);
            }

            string userType = "ST";
            if (companyId.IndexOf("AD") != -1)
            {
                userType = "ST";
            }
            else if (companyId.IndexOf("A-") != -1 && companyId.IndexOf("-SA-") == -1)
            {
                userType = "B2B2ST";
            }
            else if (companyId.IndexOf("A-") != -1 && companyId.IndexOf("-SA-") != -1)
            {
                userType = "B2B2B2ST";
            }

            var hostName = HttpContext.Request.Host.Host;
            var command = new RegisterUserCommand
            {
                Title = model.Title,
                Email = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName,
                Company_PhoneNo = model.Company_PhoneNo,
                Address = model.Address,
                Country = model.Country,
                State = model.State,
                City = model.City,
                PostalCode = Convert.ToInt32(model.PostalCode),
                Company_Mobile = model.Company_Mobile,
                Host = hostName,
                StaffType = model.StaffType,
                CompanyName = companyName,
                UserType = userType
            };

            try
            {
                await _registerUserCommandHandler.HandleAsync(command);
                TempData["SuccessMessage"] = "Staff Successfully Added";
                TempData["ErrorMessage"] = null;
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError($"Registration failed: {ex.Message}");
                TempData["SuccessMessage"] = null;
                TempData["ErrorMessage"] = "The email you have entered already exists.";
                return View("~/Views/Admin/Staff.cshtml", model);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Registration failed: {ex.Message}");
                ModelState.Clear();
                ModelState.AddModelError("", "Registration failed. Please try again.");
                return View("~/Views/Admin/Staff.cshtml", model);
            }

            return View("~/Views/Admin/Staff.cshtml", model);
        }






       

        [HttpGet]
        [Authorize]
        [Route("admin/Profile")]
        public async Task<IActionResult> CompanyProfiles()
        {
            var companyid = User.FindFirst("CompanyId")?.Value;

            var companyProfileData = await _getCompanyProfileHandler.HandleAsync(new GetCompanyProfileQuery
            {
                CompanyID = companyid
            });

            if (companyProfileData == null)
            {
                _logger.LogWarning($"Company profile not found for email {companyid}.");
                return RedirectToAction("Error", "Home");
            }

            var model = _mapper.Map<CompanyProfiles>(companyProfileData);

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


            return View("~/Views/Admin/CompanyProfile.cshtml", model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateLogo(CompanyProfiles model)
        {
            ModelState.Clear();

            if (model.LogoFile == null || model.LogoFile.Length == 0)
            {
                TempData["ErrorMessage"] = "Please select a logo file.";
                return RedirectToAction("CompanyProfiles"); // Redirect to reload the view with updated data
            }

            try
            {
                var companyIdString = User.FindFirst("CompanyId")?.Value;
                if (string.IsNullOrEmpty(companyIdString))
                {
                    TempData["ErrorMessage"] = "Company ID is not found.";
                    return RedirectToAction("CompanyProfiles"); // Redirect to reload the view with updated data
                }

                string uploadPath = Path.Combine(_webHostEnvironment.WebRootPath, "assets", "company", "logo");
                string uniqueFileName = $"{companyIdString}@{Guid.NewGuid()}{Path.GetExtension(model.LogoFile.FileName)}";

                await FileUploadHelper.UploadFileAsync(uploadPath, model.LogoFile.OpenReadStream(), uniqueFileName);

                string logoUrl = $"/assets/company/logo/{uniqueFileName}";
                model.CompanyLogo = logoUrl;

                var updateLogoCommand = new UpdateLogoCommand
                {
                    CompanyID = companyIdString,
                    CompanyLogo = model.CompanyLogo
                };
                await _updateLogoCommandHandler.HandleAsync(updateLogoCommand);

                TempData["SuccessMessage"] = "Logo updated successfully.";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating logo.");
                TempData["ErrorMessage"] = "An error occurred while uploading the logo. Please try again.";
            }

            return RedirectToAction("CompanyProfiles"); 
        }



        [HttpPost]
        [Route("admin/Profile")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CompanyProfiles(CompanyProfiles model)
        {
            try
            {
                var companyId = User.FindFirst("CompanyId")?.Value;

                if (string.IsNullOrEmpty(companyId))
                {
                    TempData["ErrorMessage"] = "Company ID is not found.";
                    return RedirectToAction("Error", "Home");
                }

                var currentCompanyProfile = await _getCompanyProfileHandler.HandleAsync(new GetCompanyProfileQuery
                {
                    CompanyID = companyId
                });

                if (currentCompanyProfile == null)
                {
                    _logger.LogWarning($"Company profile not found for CompanyId {companyId}.");
                    return RedirectToAction("Error", "Home");
                }

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
                    Mobile = model.Mobile,
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
                    Title = model.Title ?? "",
                };

                await _updateCompanyProfileHandler.HandleAsync(command);

                TempData["SuccessMessage"] = "Company profile updated successfully!";

                var updatedCompanyProfileData = await _getCompanyProfileHandler.HandleAsync(new GetCompanyProfileQuery
                {
                    CompanyID = companyId
                });

                if (updatedCompanyProfileData == null)
                {
                    _logger.LogWarning($"Company profile not found after update for CompanyId {companyId}.");
                    return RedirectToAction("Error", "Home");
                }

                model.CompanyName = updatedCompanyProfileData.CompanyName;

                return View("~/Views/Admin/CompanyProfile.cshtml", model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating company profile.");
                TempData["ErrorMessage"] = "Failed";
            }

            return View("~/Views/Admin/CompanyProfile.cshtml", model);
        }



    }

}

