using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ZealTravel.Application.CredentialManagement.Command;
using ZealTravel.Application.CredentialManagement.Handlers;
using ZealTravel.Application.CredentialManagement.Query;
using ZealTravel.Backoffice.Web.Models.Credentials;
using ZealTravel.Domain.Interfaces.Handlers;
using log4net.Core;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace ZealTravel.Backoffice.Web.Controllers
{
    [Authorize]
    public class AdminCredentionalManagementController : Controller
    {
        private readonly IHandlesQueryAsync<List<GalelioSupplierAirline>> _galileoSupplierHandler;
        private readonly IHandlesCommandAsync<UpdateGalelioCommand> _updateGalileoHandler;
        private readonly IHandlesQueryAsync<List<LccSupplierAirline>> _getLccAirlineHandler;
        private readonly IHandlesCommandAsync<UpdateLccAirlineCommand> _updateLccAirlineHandler;
        private readonly IHandlesQueryAsync<List<AirlineApiDetails>> _airlineApiHandler;
        private readonly IHandlesCommandAsync<UpdateAirlineApiCommand> _updateAirlineApiHandler;
        private readonly IHandlesQueryAsync<List<UapiFopDetail>> _uapiFopHandler;
        private readonly IHandlesCommandAsync<UpdateUapiFopCommand> _updateUapiHandler;
        private readonly IHandlesCommandAsync<UpdateUapiCcCommand> _updateUapiCcHandler;
        private readonly IHandlesQueryAsync<List<UapiCcDetails>> _getUapiCcHandler;
        private readonly IHandlesCommandAsync<int> _deleteUapiCcHandler;
        private readonly IHandlesQueryAsync<List<SupplierCredProductDetails>> _supplierProductDetailsHandler;
        private readonly IHandlesCommandAsync<SupplierProductDetailCommand> _updateProductDetailHandler;
        private readonly IHandlesCommandAsync<PnrMakeDaysCommand> _updatePnrMakeDaysHandler;
        private readonly IHandlesQueryAsync<List<PnrMakeDaysDetails>> _getPnrMakeDaysHandler;
        private readonly IHandlesCommandAsync<InsertPnrMakeDaysCommand> _insertPnrMakeDaysHandler;
        private readonly IHandlesCommandAsync<int> _deletePnrMakeDaysHandler;
        private readonly IHandlesQueryAsync<List<ProductStatusDetails>> _getSupplierProductStatusHandler;
        private readonly IHandlesCommandAsync<UpdateProductStatusCommand> _updateProductStatusHandler;
        private readonly IHandlesCommandAsync<InsertUapiCcCommand> _addUapiCcHandler;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;
        private readonly ILogger<AdminCredentionalManagementController> _logger;

        public AdminCredentionalManagementController(
            IHandlesQueryAsync<List<GalelioSupplierAirline>> galileoSupplierHandler,
            IHandlesCommandAsync<UpdateGalelioCommand> updateGalileoHandler,
            IHandlesQueryAsync<List<AirlineApiDetails>> airlineApiHandler,
            IHandlesCommandAsync<UpdateAirlineApiCommand> updateAirlineApiHandler,
            IHandlesCommandAsync<UpdateUapiCcCommand> updateUapiCcHandler,
            IHandlesQueryAsync<List<UapiCcDetails>> getUapiCcHandler,
            IHandlesQueryAsync<List<LccSupplierAirline>> getLccAirlineHandler,
            IHandlesCommandAsync<UpdateLccAirlineCommand> updateLccAirlineHandler,
            IHandlesQueryAsync<List<UapiFopDetail>> uapiFopHandler,
            IHandlesCommandAsync<UpdateUapiFopCommand> updateUapiHandler,
            IHandlesQueryAsync<List<SupplierCredProductDetails>> supplierProductDetailsHandler,
            IHandlesCommandAsync<SupplierProductDetailCommand> updateProductDetailHandler,
            IHandlesCommandAsync<PnrMakeDaysCommand> updatePnrMakeDaysHandler,
            IHandlesCommandAsync<InsertPnrMakeDaysCommand> insertPnrMakeDaysHandler,
            IHandlesQueryAsync<List<PnrMakeDaysDetails>> getPnrMakeDaysHandler,
            IHandlesQueryAsync<List<ProductStatusDetails>> getSupplierProductStatusHandler,
            IHandlesCommandAsync<UpdateProductStatusCommand> updateProductStatusHandler,
            IHandlesCommandAsync<InsertUapiCcCommand> addUapiCcHandler,
            IConfiguration configuration,
            IMapper mapper,
            ILogger<AdminCredentionalManagementController> logger,
            [FromKeyedServices("DeleteUapiCc")] IHandlesCommandAsync<int> deleteUapiCcHandler,
            [FromKeyedServices("DeletePnrMakeDays")] IHandlesCommandAsync<int> deletePnrMakeDaysHandler
        )
        {
            _configuration = configuration;
            _mapper = mapper;
            _galileoSupplierHandler = galileoSupplierHandler ?? throw new ArgumentNullException(nameof(galileoSupplierHandler));
            _updateGalileoHandler = updateGalileoHandler ?? throw new ArgumentNullException(nameof(updateGalileoHandler));
            _airlineApiHandler = airlineApiHandler ?? throw new ArgumentNullException(nameof(airlineApiHandler));
            _updateAirlineApiHandler = updateAirlineApiHandler ?? throw new ArgumentNullException(nameof(updateAirlineApiHandler));
            _updateUapiCcHandler = updateUapiCcHandler ?? throw new ArgumentNullException(nameof(updateUapiCcHandler));
            _getUapiCcHandler = getUapiCcHandler ?? throw new ArgumentNullException(nameof(getUapiCcHandler));
            _deleteUapiCcHandler = deleteUapiCcHandler ?? throw new ArgumentNullException(nameof(deleteUapiCcHandler));
            _updateGalileoHandler = updateGalileoHandler ?? throw new ArgumentNullException(nameof(updateGalileoHandler));
            _getLccAirlineHandler = getLccAirlineHandler ?? throw new ArgumentNullException(nameof(getLccAirlineHandler));
            _updateLccAirlineHandler = updateLccAirlineHandler ?? throw new ArgumentNullException(nameof(updateLccAirlineHandler));
            _uapiFopHandler = uapiFopHandler ?? throw new ArgumentNullException(nameof(uapiFopHandler));
            _uapiFopHandler = uapiFopHandler ?? throw new ArgumentNullException(nameof(uapiFopHandler));
            _updateUapiHandler = updateUapiHandler ?? throw new ArgumentNullException(nameof(updateUapiHandler));
            _supplierProductDetailsHandler = supplierProductDetailsHandler ?? throw new ArgumentNullException(nameof(supplierProductDetailsHandler));
            _updateProductDetailHandler = updateProductDetailHandler ?? throw new ArgumentNullException(nameof(updateProductDetailHandler));
            _updatePnrMakeDaysHandler = updatePnrMakeDaysHandler ?? throw new ArgumentNullException(nameof(updatePnrMakeDaysHandler));
            _getPnrMakeDaysHandler = getPnrMakeDaysHandler ?? throw new ArgumentNullException(nameof(getPnrMakeDaysHandler));
            _deletePnrMakeDaysHandler = deletePnrMakeDaysHandler ?? throw new ArgumentNullException(nameof(deletePnrMakeDaysHandler));
            _getSupplierProductStatusHandler = getSupplierProductStatusHandler ?? throw new ArgumentNullException(nameof(getSupplierProductStatusHandler));
            _updateProductStatusHandler = updateProductStatusHandler ?? throw new ArgumentNullException(nameof(updateProductStatusHandler));
            _addUapiCcHandler = addUapiCcHandler ?? throw new ArgumentNullException(nameof(addUapiCcHandler));
            _insertPnrMakeDaysHandler = insertPnrMakeDaysHandler ?? throw new ArgumentNullException(nameof(insertPnrMakeDaysHandler));
            _logger = logger;
        }


        [Route("/admin/managecredentials")]
        public IActionResult ManageCredentials()
        {
            return View("~/Views/Admin/Credentials/ManageCredentials.cshtml");
        }

        [HttpGet]
        [Route("/admin/managecredentials/productstatus")]
        public async Task<IActionResult> ProductStatus()
        {
            var supplierList = await _getSupplierProductStatusHandler.HandleAsync();

            if (supplierList == null || !supplierList.Any())
                return View("~/Views/Admin/Credentials/ProductStatus.cshtml", new List<ProductStatusViewModel>());

            var viewModelList = _mapper.Map<List<ProductStatusViewModel>>(supplierList);
            return View("~/Views/Admin/Credentials/ProductStatus.cshtml", viewModelList);
        }


        [HttpPost]
        [Route("/admin/managecredentials/productstatus")]
        public async Task<IActionResult> UpdateProductStatus(ProductStatusViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var supplierList = await _getSupplierProductStatusHandler.HandleAsync();
                var viewModelList = _mapper.Map<List<ProductStatusViewModel>>(supplierList);
                return View("~/Views/Admin/Credentials/ProductStatus.cshtml", viewModelList);
            }

            try
            {
                var updateCommand = new UpdateProductStatusCommand
                {
                    Id = model.Id,
                    SupplierId = model.SupplierId,
                    SupplierCode = model.SupplierCode,
                    Pcc = model.Pcc,
                    Product = model.Product,
                    B2b = model.B2b,
                    B2c = model.B2c,
                    Rt = model.Rt,
                    Int = model.Int,
                    MultiCity = model.MultiCity,
                    ImportPnr = model.ImportPnr,
                    Pnr = model.Pnr,
                    Ticketting = model.Ticketting
                };

                await _updateProductStatusHandler.HandleAsync(updateCommand);
                TempData["SuccessMessage"] = "Product status updated successfully!";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Failed to update product status.";
            }

            return RedirectToAction("ProductStatus");
        }


        [Route("/admin/managecredentials/hotelsupplier")]
        public IActionResult HotelSupplier()
        {
            return View("~/Views/Admin/Credentials/HotelSupplier.cshtml");
        }

        [HttpGet]
        [Route("/admin/managecredentials/galileoairline")]
        public async Task<IActionResult> GalileoAirline()
        {
            var supplierList = await _galileoSupplierHandler.HandleAsync();

            if (supplierList == null || !supplierList.Any())
                return View("~/Views/Admin/Credentials/GalileoAirline.cshtml", new List<GalileoSupplierDetailViewModel>());

            var viewModelList = _mapper.Map<List<GalileoSupplierDetailViewModel>>(supplierList);

            return View("~/Views/Admin/Credentials/GalileoAirline.cshtml", viewModelList);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("/admin/managecredentials/galileoairline")]
        public async Task<IActionResult> UpdateGalileoAirline(GalileoSupplierDetailViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var supplierList = await _galileoSupplierHandler.HandleAsync();
                var viewModelList = _mapper.Map<List<GalileoSupplierDetailViewModel>>(supplierList);
                return View("~/Views/Admin/Credentials/GalileoAirline.cshtml", viewModelList);
            }
            try
            {
                var galileoCommand = new UpdateGalelioCommand
                {
                    Hap = model.Hap,
                    ImportQueue = model.ImportQueue,
                    Pcc = model.Pcc,
                    Password = model.Password,
                    SoapUrl = model.SoapUrl,
                    TicketIfFareGaurantee = model.TicketIfFareGaurantee,
                    TktdQueue = model.TktdQueue,
                    Userid = model.Userid,
                    Id = model.Id
                };
                await _updateGalileoHandler.HandleAsync(galileoCommand);
                TempData["SuccessMessage"] = "Galileo supplier details updated successfully!";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Failed to update supplier details.";
            }

            return RedirectToAction("GalileoAirline");
        }




        [HttpGet]
        [Route("/admin/managecredentials/airlineapidetail")]
        public async Task<IActionResult> AirlineAPIDetail()
        {
            var supplierList = await _airlineApiHandler.HandleAsync();

            if (supplierList == null || !supplierList.Any())
                return View("~/Views/Admin/Credentials/AirlineAPIDetail.cshtml", new List<AirlineApiViewModel>());

            var viewModelList = _mapper.Map<List<AirlineApiViewModel>>(supplierList);
            return View("~/Views/Admin/Credentials/AirlineAPIDetail.cshtml", viewModelList);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("/admin/managecredentials/airlineapidetail")]
        public async Task<IActionResult> UpdateAirlineApiDetail(AirlineApiViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var supplierList = await _airlineApiHandler.HandleAsync();
                var viewModelList = _mapper.Map<List<AirlineApiViewModel>>(supplierList);

                return View("~/Views/Admin/Credentials/AirlineAPIDetail.cshtml", viewModelList);
            }
            try
            {
                var galileoCommand = new UpdateAirlineApiCommand
                {
                    UserId = model.UserId,
                    SupplierId = model.SupplierId,
                    Password = model.Password,
                    Id = model.Id
                };
                await _updateAirlineApiHandler.HandleAsync(galileoCommand);
                TempData["SuccessMessage"] = "Galileo supplier details updated successfully!";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Failed to update supplier details.";
            }

            return RedirectToAction("AirlineAPIDetail");
        }

        [HttpGet]
        [Route("/admin/managecredentials/lccairline")]
        public async Task<IActionResult> LCCAirlines()
        {
            var supplierList = await _getLccAirlineHandler.HandleAsync();
            if (supplierList == null || !supplierList.Any())
            {
                return View("~/Views/Admin/Credentials/LCCAirlines.cshtml", new List<LccAirlineViewModel>());
            }

            var viewModelList = _mapper.Map<List<LccAirlineViewModel>>(supplierList);
            return View("~/Views/Admin/Credentials/LCCAirlines.cshtml", viewModelList);
        }


        [HttpPost]
        [Route("/admin/managecredentials/lccairline")]
        public async Task<IActionResult> UpdateLccAirlines(LccAirlineViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var supplierList = await _getLccAirlineHandler.HandleAsync();
                var viewModelList = _mapper.Map<List<LccAirlineViewModel>>(supplierList);
                return View("~/Views/Admin/Credentials/GalileoAirline.cshtml", viewModelList);
            }
            try
            {
                var galileoCommand = new UpdateLccAirlineCommand
                {
                    Id = model.Id,
                    SupplierName = model.SupplierName,
                    SupplierId = model.SupplierId,
                    SupplierCode = model.SupplierCode,
                    TargetBranch = model.TargetBranch,
                    OrganizationCode = model.OrganizationCode,
                    CarrierCode = model.CarrierCode,
                    AgentId = model.AgentId,
                    AgentDomain = model.AgentDomain,
                    Password = model.Password,
                    LocationCode = model.LocationCode,
                    ContractVersion = model.ContractVersion,
                    PromoCode = model.PromoCode,
                    CorporateCode = model.CorporateCode,
                    Currency = model.Currency,
                    LoginId = model.LoginId,
                    Pwd = model.Pwd,
                    SessionUrl = model.SessionUrl,
                    BookingUrl = model.BookingUrl,
                    FareUrl = model.FareUrl,
                    ContentUrl = model.ContentUrl,
                    AgentUrl = model.AgentUrl,
                    LookupUrl = model.LookupUrl,
                    OperationUrl = model.OperationUrl
                };
                await _updateLccAirlineHandler.HandleAsync(galileoCommand);
                TempData["SuccessMessage"] = "Galileo supplier details updated successfully!";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Failed to update supplier details.";
            }

            return RedirectToAction("LCCAirlines");
        }

        [HttpGet]
        [Route("/admin/managecredentials/pnrmakedays")]
        public async Task<IActionResult> PnrMakeDays()
        {
            var supplierList = await _getPnrMakeDaysHandler.HandleAsync();

            if (supplierList == null || !supplierList.Any())
            {
                return View("~/Views/Admin/Credentials/PnrMakeDays.cshtml", new List<PnrMakeDaysDetail>());
            }
            var viewModelList = new AirlinePnrViewModel
            {
                PnrMakeDaysDetails = _mapper.Map<List<PnrMakeDaysDetail>>(supplierList),
                NewPnrMakeDaysDetails = new PnrMakeDaysDetail() 
            };
           


            return View("~/Views/Admin/Credentials/PnrMakeDays.cshtml", viewModelList);
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("/admin/managecredentials/pnrmakedays")]
        public async Task<IActionResult> UpdatePnrMakeDays(PnrMakeDaysDetail model)
        {
            if (!ModelState.IsValid)
            {
                var supplierList = await _getPnrMakeDaysHandler.HandleAsync();
                var viewModelList = _mapper.Map<List<PnrMakeDaysDetail>>(supplierList);

                return View("~/Views/Admin/Credentials/PnrMakeDays.cshtml", viewModelList);
            }

            try
            {
                var pnrCommand = new PnrMakeDaysCommand
                {
                    Id = model.Id,
                    CarrierCode = model.CarrierCode,
                    Sector = model.Sector,
                    PnrDays = model.PnrDays
                };

                await _updatePnrMakeDaysHandler.HandleAsync(pnrCommand);
                TempData["SuccessMessage"] = "PNR Make Days details updated successfully!";
            }
            catch (Exception)
            {
                TempData["ErrorMessage"] = "Failed to update PNR Make Days details.";
            }

            return RedirectToAction("PnrMakeDays");
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> InsertPnrMakeDays(AirlinePnrViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var supplierList = await _getPnrMakeDaysHandler.HandleAsync();
                var viewModelList = _mapper.Map<List<AirlinePnrViewModel>>(supplierList);

                return View("~/Views/Admin/Credentials/PnrMakeDays.cshtml", viewModelList);
            }

            try
            {
                var pnrCommand = new InsertPnrMakeDaysCommand
                {
                    CarrierCode = model.NewPnrMakeDaysDetails.CarrierCode,
                    Sector = model.NewPnrMakeDaysDetails.Sector,
                    PnrDays = model.NewPnrMakeDaysDetails.PnrDays
                };

                await _insertPnrMakeDaysHandler.HandleAsync(pnrCommand);
                TempData["SuccessMessage"] = "PNR Make Days details updated successfully!";
            }
            catch (Exception)
            {
                TempData["ErrorMessage"] = "Failed to update PNR Make Days details.";
            }

            return RedirectToAction("PnrMakeDays");
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeletePnrMakeDays(int id)
        {
            try
            {
                await _deletePnrMakeDaysHandler.HandleAsync(id);
                TempData["SuccessMessage"] = "PNR Make Days entry deleted successfully!";
                return RedirectToAction("PnrMakeDays");
            }
            catch (KeyNotFoundException)
            {
                TempData["ErrorMessage"] = "PNR Make Days entry not found.";
                return RedirectToAction("PnrMakeDays");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Failed to delete PNR Make Days entry.";
                return StatusCode(500, $"Error: {ex.Message}");
            }
        }


        [Route("/admin/managecredentials/supplierdetail")]
        public IActionResult SupplierDetail()
        {
            return View("~/Views/Admin/Credentials/SupplierDetail.cshtml");
        }

        [HttpGet]
        [Route("/admin/managecredentials/uapifop")]
        public async Task<IActionResult> UAPIFOP()
        {
            var supplierList = await _uapiFopHandler.HandleAsync();

            if (supplierList == null || !supplierList.Any())
                return View("~/Views/Admin/Credentials/UAPIFOP.cshtml", new List<UapiFopViewModel>());

            var viewModelList = _mapper.Map<List<UapiFopViewModel>>(supplierList);
            return View("~/Views/Admin/Credentials/UAPIFOP.cshtml", viewModelList);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("/admin/managecredentials/uapifop")]
        public async Task<IActionResult> UpdateUapiFop(int selectedFopId)
        {
            try
            {
                var command = new UpdateUapiFopCommand
                {
                    Id = selectedFopId
                };
                await _updateUapiHandler.HandleAsync(command);
                TempData["SuccessMessage"] = "Successfully Updated!!!";
            }
            catch (Exception)
            {
                TempData["ErrorMessage"] = "Failed to update payment method.";
            }

            return RedirectToAction("UAPIFOP");
        }

        [HttpGet]
        [Route("/admin/managecredentials/uapicc")]
        public async Task<IActionResult> UAPICC()
        {
            var supplierList = await _getUapiCcHandler.HandleAsync();

            if (supplierList == null || !supplierList.Any())
                return View("~/Views/Admin/Credentials/UAPICC.cshtml", new SupplierUapiCcViewModel());

            // Map the list of UapiCcDetails to SupplierUapiCcViewModel
            var viewModel = new SupplierUapiCcViewModel
            {
                UapiCcDetails = _mapper.Map<List<UapiCcDetail>>(supplierList),
                NewUapiCc = new UapiCcDetail() // Initialize NewUapiCc if needed
            };

            return View("~/Views/Admin/Credentials/UAPICC.cshtml", viewModel);
        }




        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("/admin/managecredentials/uapicc")]
        public async Task<IActionResult> UpdateUapiCc(UapiCcDetail model)
        {

            if (!ModelState.IsValid)
            {
                var supplierList = await _getUapiCcHandler.HandleAsync();
                var viewModel = _mapper.Map<UapiCcDetail>(supplierList.FirstOrDefault());
                return View("~/Views/Admin/Credentials/UAPICC.cshtml");
            }


            try
            {
                var updateCommand = new UpdateUapiCcCommand
                {
                    Id = model.Id,
                    BankCountryCode = model.BankCountryCode,
                    BankName = model.BankName,
                    Cvv = model.Cvv,
                    ExpDate = model.ExpDate,
                    Name = model.Name,
                    Number = model.Number,
                    Type = model.Type,
                    AddressName = model.AddressName,
                    Street = model.Street,
                    City = model.City,
                    State = model.State,
                    PostalCode = model.PostalCode,
                    Country = model.Country,
                    Carriers = model.Carriers
                };

                await _updateUapiCcHandler.HandleAsync(updateCommand);

                TempData["SuccessMessage"] = "UAPI CC details updated successfully!";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Failed to update UAPI CC details. Error: {ex.Message}";
            }

            return RedirectToAction("UAPICC");
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> InsertUapiCc(SupplierUapiCcViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var supplierList = await _getUapiCcHandler.HandleAsync();
                var viewModel = _mapper.Map<SupplierUapiCcViewModel>(supplierList.FirstOrDefault()); // Use SupplierUapiCcViewModel
                return View("~/Views/Admin/Credentials/UAPICC.cshtml", viewModel);
            }

            try
            {
                var addCommand = new InsertUapiCcCommand
                {
                    BankCountryCode = model.NewUapiCc.BankCountryCode,
                    BankName = model.NewUapiCc.BankName,
                    Cvv = model.NewUapiCc.Cvv,
                    ExpDate = model.NewUapiCc.ExpDate,
                    Name = model.NewUapiCc.Name,
                    Number = model.NewUapiCc.Number,
                    Type = model.NewUapiCc.Type,
                    AddressName = model.NewUapiCc.AddressName,
                    Street = model.NewUapiCc.Street,
                    City = model.NewUapiCc.City,
                    State = model.NewUapiCc.State,
                    PostalCode = model.NewUapiCc.PostalCode,
                    Country = model.NewUapiCc.Country,
                    Carriers = model.NewUapiCc.Carriers
                };

                await _addUapiCcHandler.HandleAsync(addCommand);

                TempData["SuccessMessage"] = "UAPI CC details added successfully!";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Failed to add UAPI CC details. Error: {ex.Message}";
            }

            return RedirectToAction("UAPICC");
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteUapiCc(int id)
        {
            try
            {
                await _deleteUapiCcHandler.HandleAsync(id);

                return RedirectToAction("UAPICC");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Failed to delete UAPI CC details. Error: {ex.Message}");
            }
        }




        [HttpGet]
        [Route("/admin/managecredentials/productdetail")]
        public async Task<IActionResult> ProductDetail()
        {
            var supplierList = await _supplierProductDetailsHandler.HandleAsync();

            if (supplierList == null || !supplierList.Any())
            {
                return View("~/Views/Admin/Credentials/ProductDetail.cshtml", new List<SupplierProductDetailsViewModel>());
            }

            var viewModelList = _mapper.Map<List<SupplierProductDetailsViewModel>>(supplierList);

            return View("~/Views/Admin/Credentials/ProductDetail.cshtml", viewModelList);
        }

        [HttpPost]
        [Route("/admin/managecredentials/productdetail")]
        public async Task<IActionResult> UpdateProductDetail(SupplierProductDetailsViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var supplierList = await _supplierProductDetailsHandler.HandleAsync();
                var viewModelList = _mapper.Map<List<SupplierProductDetailsViewModel>>(supplierList);
                return View("~/Views/Admin/Credentials/ProductDetail.cshtml", viewModelList);
            }

            try
            {
                var updateCommand = new SupplierProductDetailCommand
                {
                    Id = model.Id,
                    SupplierId = model.SupplierId,
                    SupplierName = model.SupplierName,
                    SupplierType = model.SupplierType,
                    SupplierCode = model.SupplierCode,
                    FareType = model.FareType,
                    Email = model.Email,
                    PhoneNumber = model.PhoneNumber,
                    CityName = model.CityName,
                    Flight = model.Flight,
                    Hotel = model.Hotel,
                    Remarks = model.Remarks,
                    Status = model.Status
                };

                await _updateProductDetailHandler.HandleAsync(updateCommand);
                TempData["SuccessMessage"] = "Supplier product details updated successfully!";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Failed to update supplier product details. Error: {ex.Message}";
            }
            return RedirectToAction("ProductDetail");
        }

    }
}
