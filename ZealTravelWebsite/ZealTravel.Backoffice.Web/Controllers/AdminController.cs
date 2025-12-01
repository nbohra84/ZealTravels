using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using ZealTravel.Application.BankManagement.Handler;
using ZealTravel.Application.BankManagement.Command;
using ZealTravel.Application.AgencyManagement.Queries;
using ZealTravel.Backoffice.Web.Models;
using Microsoft.Extensions.Configuration;
using AutoMapper;
using ZealTravel.Domain.Interfaces.IRepository;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using ZealTravel.Domain.Interfaces.Handlers;
using ZealTravel.Application.BankManagement.Query;
using ZealTravel.Application.UserManagement.Handlers;
using ZealTravel.Application.UserManagement.Queries;
using Microsoft.AspNetCore.Mvc.Rendering;
using log4net.Core;

namespace ZealTravel.Backoffice.Web.Controllers
{
    [Authorize]
    public class AdminController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;
        private readonly IHandlesQueryAsync<GetBankDetailQuery, List<AdminBankDetails>> _getBankDetailQueryHandler;
        private readonly IHandlesQueryAsync<BankNameQuery, List<GetBankNameQuery>> _getBankNameQueryHandler;
        private readonly IHandlesCommandAsync<UpdateBankCommand> _updateBankDetailCommandHandler;
        private readonly IHandlesCommandAsync<BankDetailCommand> _addBankDetailCommandHandler;
        private readonly DeleteBankDetailHandler _deleteBankDetailHandler;
        private readonly ILogger<AdminController> _logger;

        public AdminController(
            IConfiguration configuration,
            IMapper mapper,
            IHandlesQueryAsync<GetBankDetailQuery, List<AdminBankDetails>> getBankDetailQueryHandler,
            IHandlesQueryAsync<BankNameQuery, List<GetBankNameQuery>> getBankNameQueryHandler,
            IHandlesCommandAsync<UpdateBankCommand> updateBankDetailCommandHandler,
            IHandlesCommandAsync<BankDetailCommand> addBankDetailCommandHandler,
            DeleteBankDetailHandler deleteBankDetailHandler,
            ILogger<AdminController> logger
        )
        {
            _configuration = configuration;
            _mapper = mapper;
            _getBankDetailQueryHandler = getBankDetailQueryHandler;
            _getBankNameQueryHandler = getBankNameQueryHandler;
            _updateBankDetailCommandHandler = updateBankDetailCommandHandler;
            _deleteBankDetailHandler = deleteBankDetailHandler;
            _addBankDetailCommandHandler = addBankDetailCommandHandler;
            _logger = logger;
        }




        [HttpGet]
        public async Task<IActionResult> GetAllBanks()
        {
            var query = new BankNameQuery();
            var bankDetails = await _getBankNameQueryHandler.HandleAsync(query);

            if (bankDetails == null || !bankDetails.Any())
            {
                return NotFound("No banks found.");
            }

            var banks = bankDetails.Select(bank => new
            {
                BankName = bank.BankName,
                BankCode = bank.BankCode
            }).ToList();

            return Ok(banks);
        }



        [Route("admin/bankdetail")]
        public async Task<IActionResult> ManageBank()
        {
            var bankNamesQuery = new BankNameQuery();
            var bankNames = await _getBankNameQueryHandler.HandleAsync(bankNamesQuery);

            var bankNamesList = bankNames.Select(bank => new SelectListItem
            {
                Text = bank.BankName,
                Value = bank.BankCode
            }).ToList();

            ViewBag.BankNames = bankNamesList;
            
            var companyId = User.FindFirstValue("CompanyId");
            var query = new GetBankDetailQuery { CompanyId = companyId };

            var bankList = await _getBankDetailQueryHandler.HandleAsync(query);

            var model = new BankDetailViewModel
            {
                BankDetails = new List<BankDetail>(),
                NewBankDetail = new BankDetail()
            };

            if (bankList != null && bankList.Any())
            {
                foreach (var bankDetails in bankList)
                {
                    var bankDetailModel = new BankDetail
                    {
                        BankCode = bankDetails.BankCode,
                        BankName = bankDetails.BankName,
                        BankLogoCode = bankDetails.BankLogoCode,
                        BranchName = bankDetails.BranchName,
                        AccountNo = bankDetails.AccountNo,
                        Status = bankDetails.Status,
                        B2b = bankDetails.B2b,
                        D2b = bankDetails.D2b,
                        B2c = bankDetails.B2c,
                        B2b2c = bankDetails.B2b2c,
                        B2b2b = bankDetails.B2b2b,
                        GSTNumber = bankDetails.GSTNumber,
                        CompanyName = bankDetails.CompanyName,
                        PanNo = bankDetails.PanNo,
                        Id = bankDetails.Id,
                    };

                    model.BankDetails.Add(bankDetailModel);
                }
            }
            else
            {
                model.BankDetails = bankNames.Select(bank => new BankDetail
                {
                    // Here, you can assign default values for the properties you want.
                    BankCode = string.Empty, // You might need to set this, or leave it empty if you don't want to set a default.
                    BankName = string.Empty,  // You can assign a default bank name or leave it empty.
                    BankNames = bankNames.ToList(), // Assign all bank names to each BankDetail
                    BranchName = string.Empty, // Set defaults as necessary
                    AccountNo = string.Empty,  // Set defaults as necessary
                    Status = false,  // Default status
                    B2b = false,     // Default B2b
                    D2b = false,     // Default D2b
                    B2c = false,     // Default B2c
                    B2b2b = false,   // Default B2b2b
                    B2b2c = false,   // Default B2b2c
                    BankLogo = string.Empty,  // Set default BankLogo
                    GSTNumber = string.Empty, // Default GSTNumber
                    CompanyName = string.Empty, // Default CompanyName
                    PanNo = string.Empty // Default PanNo
                }).ToList();
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateBank(BankDetailViewModel viewModel)
        {
            if (viewModel.NewBankDetail == null)
            {
                viewModel.NewBankDetail = new BankDetail();
            }


            var companyId = User.FindFirstValue("CompanyId");
            //var companyId = "AD-101";

            var command = new UpdateBankCommand
            {
                CompanyId = companyId,
                BankName = viewModel.NewBankDetail.BankName,
                BankCode = viewModel.NewBankDetail.BankCode,
                BranchName = viewModel.NewBankDetail.BranchName,
                BankLogoCode = viewModel.NewBankDetail.BankLogoCode,
                AccountNo = viewModel.NewBankDetail.AccountNo,
                Status = viewModel.NewBankDetail.Status,
                B2b = viewModel.NewBankDetail.B2b,
                D2b = viewModel.NewBankDetail.D2b,
                B2c = viewModel.NewBankDetail.B2c,
                B2b2b = viewModel.NewBankDetail.B2b2b,
                B2b2c = viewModel.NewBankDetail.B2b2c,
                GSTNumber = viewModel.NewBankDetail.GSTNumber,
                PanNo = viewModel.NewBankDetail.PanNo,
                Id = viewModel.NewBankDetail.Id
            };

            try
            {
                // Execute the command to update the bank details
                await _updateBankDetailCommandHandler.HandleAsync(command);

                // Return success message as JSON for AJAX response
                TempData["UpdateMessage"] = "Updated successfully.";
                return RedirectToAction("bankdetail");
            }
            catch (Exception ex)
            {
                // Return error message as JSON for AJAX response
                TempData["UpdateMessage"] = $"Error updating bank details: {ex.Message}";
                return RedirectToAction("bankdetail");
            }
        }




        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddBankDetail(BankDetailViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                return View("~/Views/Admin/ManageBank.cshtml", viewModel);

            }
            var companyId = User.FindFirstValue("CompanyId");

                try
                {
                var command = new BankDetailCommand
                {
                    CompanyId = companyId,
                    BankName = viewModel.NewBankDetail.BankName,
                    BankCode = viewModel.NewBankDetail.BankCode,
                    BranchName = viewModel.NewBankDetail.BranchName,
                    BankLogoCode = viewModel.NewBankDetail.BankLogoCode,
                    AccountNo = viewModel.NewBankDetail.AccountNo,
                    Status = viewModel.NewBankDetail.Status,
                    B2b = viewModel.NewBankDetail.B2b,
                    D2b = viewModel.NewBankDetail.D2b,
                    B2c = viewModel.NewBankDetail.B2c,
                    B2b2b = viewModel.NewBankDetail.B2b2b,
                    B2b2c = viewModel.NewBankDetail.B2b2c,
                    GSTNumber = viewModel.NewBankDetail.GSTNumber,
                    PanNo = viewModel.NewBankDetail.PanNo
                };

                    await _addBankDetailCommandHandler.HandleAsync(command);

                    TempData["SuccessMessage"] = "Bank details added successfully.";
                    return RedirectToAction("ManageBank", "Admin"); // Redirect to a valid action
                }
                catch (Exception)
                {
                    TempData["ErrorMessage"] = "An error occurred while adding bank details.";
                    return RedirectToAction("ManageBank", "Admin"); // Ensure "ManageBank.cshtml" exists
                }
        }






        [HttpPost]
        public async Task<IActionResult> DeleteBankDetail([FromBody] GetBankDetailQuery command)
        {
            if (command == null || command.Id <= 0)
            {
                return BadRequest(new { Success = false, Message = "Invalid request. Please provide a valid Id." });
            }

            command.CompanyId = User.FindFirstValue("CompanyId");
            //command.CompanyId = "AD-101";

            try
            {
                await _deleteBankDetailHandler.HandleAsync(command);

                return Ok(new { Success = true, Message = "Bank detail deleted successfully." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Success = false, Message = $"Error deleting bank detail: {ex.Message}" });
            }
        }



    }
}
