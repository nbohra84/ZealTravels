using Microsoft.AspNetCore.Mvc;
using ZealTravel.Application.AgencyManagement.Command;
using ZealTravel.Application.AgencyManagement.Queries;
using ZealTravel.Application.AgencyManagement.Handlers;
using ZealTravel.Domain.Interfaces.Handlers;
using ZealTravel.Backoffice.Web.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using ZealTravel.Application.UserManagement.Queries;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using ZealTravel.Application.Handlers;
using ZealTravel.Domain.Data.Entities;
using ZealTravel.Common.Helpers;
using AutoMapper;
using System.Text;
using System.Collections;
using System.Data;
using System.Linq;
using Microsoft.Identity.Client;
using ZealTravel.Application.ReportManagement.Queries;
using ZealTravel.Application.ReportManagement.Handler;
using Microsoft.AspNetCore.Mvc.Rendering;
using ZealTravel.Application.AirlineSupplierManagement.Queries;
using ZealTravel.Application.AirlineSupplierManagement.Handlers;
using log4net.Core;
using ZealTravel.Application.BookingManagement.Query;
using ZealTravel.Domain.Models;
using ZealTravel.Domain.Data.Models.Booking;
namespace ZealTravel.Backoffice.Web.Controllers
{
    [Authorize]
    public class ReportController : Controller
    {
        private readonly IHandlesQueryAsync<DailyBookingReportsQuery, List<DailyBooking>> _dailyBookingQueryHandler;
        private readonly IHandlesQueryAsync<GetProfileQuery, CompanyProfileData> _getCompanyProfileQueryHandler;
        private readonly IHandlesCommandAsync<UpdateLogoCommand> _updateLogoCommandHandler;
        private readonly IHandlesQueryAsync<GetUserDetailsQuery, CompanyRegister> _getUserDetailsQueryHandler;
        private readonly IHandlesQueryAsync<LedgerReportQuery, List<LedgerReports>> _ledgerReportQueryHandler;
        private readonly IHandlesQueryAsync<AirBookingQuery, List<AirBookingReport>> _flightBookingReportQueryHandler;
        private readonly IHandlesQueryAsync<AgencyListBySearchTextQuery, List<string>> _getAgencyListbySerachTextQueryHandler;
        private readonly IHandlesQueryAsync<CompanyIdByAccountIdQuery, string> _getCompanyIDByAccountIDQueryHandler;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IMapper _mapper;
        private readonly IHandlesQueryAsync<CancelAirBookingReportsQuery, List<CancelAirBooking>> _canelFlightBookingQueryHandler;
        private readonly IHandlesQueryAsync<GetAirSupplierQuery, List<AirSupplier>> _getAirSupplierQueryHandler;
        private readonly ILogger<ReportController> _logger;
        private readonly IHandlesQueryAsync<AirBookingDetailQuery, AirSheetReportData> _airSheetReportHandler;
        private readonly IHandlesQueryAsync<BookingRefrenceQuery, List<FlightBookingRefrence>> _flightReferenceHandler;
        private readonly IHandlesQueryAsync<CancelAirRefrenceQuery, List<FlightBookingRefrence>> _cancelRefundReportHandler;



        public ReportController(IHandlesQueryAsync<DailyBookingReportsQuery,
            List<DailyBooking>> dailyBookingQueryHandler,
            IHandlesQueryAsync<GetProfileQuery, CompanyProfileData> getCompanyProfileQueryHandler,
            IHandlesCommandAsync<UpdateLogoCommand> updateLogoCommandHandler,
            IHandlesQueryAsync<GetUserDetailsQuery, CompanyRegister> getUserDetailsQueryHandler,
            IWebHostEnvironment webHostEnvironment,
            IHandlesQueryAsync<LedgerReportQuery, List<LedgerReports>> ledgerReportQueryHandler,
            IHandlesQueryAsync<AirBookingQuery, List<AirBookingReport>> flightBookingQueryHandler,
            IHandlesQueryAsync<AgencyListBySearchTextQuery, List<string>> getAgencyListbySerachTextQueryHandler,
            IHandlesQueryAsync<CompanyIdByAccountIdQuery, string> getCompanyIDByAccountIDQueryHandler,
            IHandlesQueryAsync<CancelAirBookingReportsQuery, List<CancelAirBooking>> canelFlightBookingQueryHandler,
            IHandlesQueryAsync<GetAirSupplierQuery, List<AirSupplier>> getAirSupplierQueryHandler,
            IHandlesQueryAsync<AirBookingDetailQuery, AirSheetReportData> airSheetReportHandler,
            [FromKeyedServices("FlightReference")] IHandlesQueryAsync<BookingRefrenceQuery, List<FlightBookingRefrence>> flightReferenceHandler,
            [FromKeyedServices("CancelRefund")] IHandlesQueryAsync<CancelAirRefrenceQuery, List<FlightBookingRefrence>> cancelRefundReportHandler,
            IMapper mapper,
            ILogger<ReportController> logger)
        {
            _dailyBookingQueryHandler = dailyBookingQueryHandler;
            _getCompanyProfileQueryHandler = getCompanyProfileQueryHandler;
            _updateLogoCommandHandler = updateLogoCommandHandler;
            _getUserDetailsQueryHandler = getUserDetailsQueryHandler;
            _ledgerReportQueryHandler = ledgerReportQueryHandler;
            _flightBookingReportQueryHandler = flightBookingQueryHandler;
            _canelFlightBookingQueryHandler = canelFlightBookingQueryHandler;
            _getAgencyListbySerachTextQueryHandler = getAgencyListbySerachTextQueryHandler;
            _getCompanyIDByAccountIDQueryHandler = getCompanyIDByAccountIDQueryHandler;
            _getAirSupplierQueryHandler = getAirSupplierQueryHandler;
            _webHostEnvironment = webHostEnvironment;
            _flightReferenceHandler = flightReferenceHandler;
            _cancelRefundReportHandler = cancelRefundReportHandler;
            _airSheetReportHandler = airSheetReportHandler;
            _mapper = mapper;
            _logger = logger;
        }
        #region #Dailybookings
        [HttpGet]
        [Route("/report/dailybookings")]
        public async Task<IActionResult> DailyBookingReports()
        {
            var today = DateTime.Now.Date;

            var model = new DailyBookingReports
            {
                StartDate = today,
                EndDate = today
            };
            var viewModel = await GetDailyBooking(model);

            return View(viewModel);
        }



        [HttpPost]
        [Route("/report/dailybookings")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DailyBookingReports(DailyBookingReports model)
        {

            if (!ModelState.IsValid)
            {
                return View(model);
            }


            try
            {
                var companyId = User.FindFirstValue("CompanyID");


                if (!string.IsNullOrEmpty(model.CompanyNameWithAccountId))
                {
                    string[] compin = model.CompanyNameWithAccountId.Split('[');
                    string[] datare = compin[1].Split(']');
                    string accoutnId = datare[0];
                    var query = new CompanyIdByAccountIdQuery { AccountId = Convert.ToInt32(accoutnId) };
                    companyId = await _getCompanyIDByAccountIDQueryHandler.HandleAsync(query);

                }
                if (string.IsNullOrEmpty(companyId))
                {
                    ModelState.AddModelError("", "Company ID not found.");
                    return View(new DailyBookingReportsListResponse { BookingReports = new List<DailyBookingReportsResponse>() });
                }
                model.CompanyID = companyId;
                var viewModel = await GetDailyBooking(model);


                if (viewModel.BookingReports == null || !viewModel.BookingReports.Any())
                {
                    viewModel.ErrorMessage = "No Record Found!";
                }
                return View(viewModel);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "An error occurred while fetching the reports: " + ex.Message);
            }

            return View(new DailyBookingReportsListResponse { BookingReports = new List<DailyBookingReportsResponse>() });
        }


        private async Task<DailyBookingReportsListResponse> GetDailyBooking(DailyBookingReports model)
        {
            var query = new DailyBookingReportsQuery
            {
                CompanyId = model.CompanyID,
                StartDate = model.StartDate,
                EndDate = model.EndDate
            };

            var domainReports = await _dailyBookingQueryHandler.HandleAsync(query);

            var groupedReports = domainReports
                .GroupBy(row => row.CarrierCode)
                .Select(g => new DailyBookingReportsResponse
                {
                    CarrierCode = g.Key,
                    CarrierName = g.FirstOrDefault()?.CarrierName,
                    TotalBasic = g.Sum(x => x.TotalBasic),
                    TotalYQ = g.Sum(x => x.TotalYQ),
                    TotalFare = g.Sum(x => x.TotalFare),
                    TotalCommission = g.Sum(x => x.TotalCommission),
                    NoOfPassengers = g.Sum(x => x.NoOfPassengers),
                    NoOfBookings = g.Count()
                })
                .ToList();

            var webReports = _mapper.Map<List<DailyBookingReportsResponse>>(groupedReports);

            var viewModel = new DailyBookingReportsListResponse
            {
                CompanyID = model.CompanyID,
                StartDate = model.StartDate,
                EndDate = model.EndDate,
                BookingReports = webReports,
                CompanyNameWithAccountId = model.CompanyNameWithAccountId
            };

            return viewModel;
        }


        public async Task<IActionResult> ExportToExcel(DailyBookingReports model)
        {
            if (model == null || string.IsNullOrEmpty(model.CompanyID) || model.StartDate == null || model.EndDate == null)
            {
                return BadRequest("Invalid parameters for exporting reports.");
            }

            var viewModel = await GetDailyBooking(model);
            var bookingReportsDict = new List<DailyBookingReportsResponse>();

            foreach (var report in viewModel.BookingReports)
            {
                var mappedReport = _mapper.Map<DailyBookingReportsResponse>(report);

                bookingReportsDict.Add(mappedReport);
            }

            if (!bookingReportsDict.Any())
            {
                return BadRequest("No reports to export.");
            }

            var filePath = Path.Combine(Path.GetTempPath(), $"DailyBookingReports_{DateTime.Now:yyyyMMdd}.xlsx");
            var excelGenerator = new GenerateExcel();

            excelGenerator.ExportToExcel(bookingReportsDict, filePath);

            var fileBytes = System.IO.File.ReadAllBytes(filePath);
            return File(fileBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "DailyBookingReports.xlsx");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ExportToPDF(DailyBookingReports model)
        {
            if (model == null || string.IsNullOrEmpty(model.CompanyID) || model.StartDate == null || model.EndDate == null)
            {
                return BadRequest("Invalid parameters for exporting reports.");
            }

            var viewModel = await GetDailyBooking(model);

            if (!viewModel.BookingReports.Any())
            {
                return BadRequest("No reports to export.");
            }

            var pdfFilePath = Path.Combine(Path.GetTempPath(), $"DailyBookingReports_{DateTime.Now:yyyyMMdd}.pdf");
            var pdfGenerator = new GeneratePDF();

            var bookingReportsList = viewModel.BookingReports.Select((report, index) => new
            {
                SNo = index + 1,
                report.CarrierCode,
                report.CarrierName,
                report.TotalBasic,
                report.TotalYQ,
                report.TotalFare,
                report.TotalCommission,
                report.NoOfPassengers,
                report.NoOfBookings
            }).ToList();

            var headers = new List<string>
    {
        "SNo",
        "CarrierCode",
        "CarrierName",
        "TotalBasic",
        "TotalYQ",
        "TotalFare",
        "TotalCommission",
        "NoOfPassengers",
        "NoOfBookings"
    };

            pdfGenerator.ExportToPDF(pdfFilePath, bookingReportsList, headers);

            var fileBytes = System.IO.File.ReadAllBytes(pdfFilePath);
            return File(fileBytes, "application/pdf", "DailyBookingReports.pdf");
        }
        #endregion

        #region Profile
        public async Task<IActionResult> Profile()
        {
            var email = User.Identity.Name;
            if (string.IsNullOrEmpty(email))
            {
                return BadRequest();
            }
            var query = new GetProfileQuery(email);
            var companyProfile = await _getCompanyProfileQueryHandler.HandleAsync(query);
            if (companyProfile == null)
            {
                return NotFound();
            }

            var profileViewModel = _mapper.Map<ProfileDetail>(companyProfile);
            return View(profileViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateProfile(ProfileDetail model)
        {
            ModelState.Clear();
            if (model.LogoFile == null || model.LogoFile.Length == 0)
            {
                TempData["ErrorMessage"] = "Please select a logo file.";
                return RedirectToAction("Profile");
            }

            try
            {
                var companyIdString = User.FindFirst("CompanyId")?.Value;
                if (string.IsNullOrEmpty(companyIdString))
                {
                    TempData["ErrorMessage"] = "Company ID is not found.";
                    return View(model);
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
                TempData["ErrorMessage"] = "An error occurred while uploading the image. Please try again.";
                ModelState.AddModelError("LogoUrl", "There was a problem uploading the logo. Please try again.");
                return View(model);
            }

            return RedirectToAction("Profile");
        }

        #endregion

        #region #Ledger
        [HttpGet]
        [Route("/report/ledger")]
        public async Task<IActionResult> Ledger()
        {
            var companyID = UserHelper.GetCompanyID(User);
            var today = DateTime.Now.Date;

            var model = new LedgerReport
            {
                FromDate = today,
                ToDate = today,
                FilterFromDate = today,
                FilterToDate = today,
                CompanyID = companyID

            };

            var ledgerReports = await GetLedgerData(model);

            StringBuilder companyHTMLModal = new StringBuilder();

            StringBuilder reportHTML = new StringBuilder();

            if (ledgerReports.Any())
            {
                reportHTML = GetLedgerReportHTML(ledgerReports, false, out var strCompanyHTMLModal, UserHelper.GetCompanyID(User));
                companyHTMLModal = strCompanyHTMLModal;
            }
            var viewModel = new LedgerReportListResponse
            {
                CompanyID = model.CompanyID,
                FromDate = model.FromDate,
                ToDate = model.ToDate,
                FilterFromDate = DateTime.Now.Date,
                FilterToDate = DateTime.Now.Date,
                PNR = model.PNR,
                TicketNumber = model.TicketNumber,
                PassengerName = model.PassengerName,
                BookingReference = model.BookingReference,
                EventID = model.EventID,
                ReportTableHTML = !string.IsNullOrEmpty(reportHTML.ToString()) ? reportHTML.ToString() : null,
                ReportTableCompanyPopup = companyHTMLModal.ToString()


            };

            if (string.IsNullOrEmpty(viewModel.ReportTableHTML))
            {
                viewModel.ReportTableHTML = string.Empty;
            }

            return View(viewModel);
        }

        [HttpPost]
        [Route("/report/ledger")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Ledger(LedgerReport model)
        {
            var viewModel = new LedgerReportListResponse();
            if (!ModelState.IsValid)
            {
                return View(model);
            }


            try
            {
                var companyId = User.FindFirstValue("CompanyID");


                if (!string.IsNullOrEmpty(model.CompanyNameWithAccountId))
                {
                    string[] compin = model.CompanyNameWithAccountId.Split('[');
                    string[] datare = compin[1].Split(']');
                    string accoutnId = datare[0];
                    var query = new CompanyIdByAccountIdQuery { AccountId = Convert.ToInt32(accoutnId) };
                    companyId = await _getCompanyIDByAccountIDQueryHandler.HandleAsync(query);

                }

                if (string.IsNullOrEmpty(companyId))
                {
                    ModelState.AddModelError("", "Company ID not found.");
                    return View(viewModel);
                }

                model.CompanyID = companyId;

                var ledgerReports = await GetLedgerData(model);
                StringBuilder companyHTMLModal = new StringBuilder();

                StringBuilder reportHTML = new StringBuilder();

                if (ledgerReports.Any())
                {
                    reportHTML = GetLedgerReportHTML(ledgerReports, false, out var strCompanyHTMLModal, UserHelper.GetCompanyID(User));
                    companyHTMLModal = strCompanyHTMLModal;
                }

                viewModel = new LedgerReportListResponse
                {
                    CompanyID = model.CompanyID,
                    FromDate = model.FromDate,
                    ToDate = model.ToDate,
                    FilterFromDate = model.FilterFromDate == DateTime.MinValue ? DateTime.Now.Date : model.FilterFromDate,
                    FilterToDate = model.FilterToDate == DateTime.MinValue ? DateTime.Now.Date : model.FilterToDate,
                    PNR = model.PNR,
                    TicketNumber = model.TicketNumber,
                    PassengerName = model.PassengerName,
                    BookingReference = model.BookingReference,
                    EventID = model.EventID,
                    ReportTableHTML = !string.IsNullOrEmpty(reportHTML.ToString()) ? reportHTML.ToString() : null,
                    ReportTableCompanyPopup = companyHTMLModal.ToString(),
                    CompanyNameWithAccountId = model.CompanyNameWithAccountId
                };

                if (string.IsNullOrEmpty(viewModel.ReportTableHTML))
                {
                    viewModel.ReportTableHTML = string.Empty;
                }


            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "An error occurred while fetching the ledger reports: " + ex.Message);
            }


            return View(viewModel);
        }

        private async Task<List<LedgerReports>> GetLedgerData(LedgerReport model)
        {

            var txtPNR = !string.IsNullOrEmpty(model.PNR) ? model.PNR.Trim() : string.Empty;
            var Ticketno = !string.IsNullOrEmpty(model.TicketNumber) ? model.TicketNumber : string.Empty;
            var PaxName = !string.IsNullOrEmpty(model.PassengerName) ? model.PassengerName : string.Empty;
            var bookingref = !string.IsNullOrEmpty(model.BookingReference) ? model.BookingReference.Trim() : string.Empty;
            var eventId = !string.IsNullOrEmpty(model.EventID) ? model.EventID : string.Empty;
            var searchtype = string.Empty;
            var searchbyval = string.Empty;
            var fromDate = model.FromDate;
            var toDate = model.ToDate;



            if (PaxName != "")
            {
                searchtype = "PAX";
                searchbyval = PaxName;
                fromDate = model.FilterFromDate;
                toDate = model.FilterToDate;
            }
            else
            {
                if (bookingref != "")
                {
                    searchtype = "REF";
                    searchbyval = bookingref;
                }
                else if (Ticketno != "")
                {
                    searchtype = "TKT";
                    searchbyval = Ticketno;
                }
                else if (txtPNR != "")
                {
                    searchtype = "PNR";
                    searchbyval = txtPNR;
                }
            }

            var ledgerQuery = new LedgerReportQuery
            {
                CompanyId = model.CompanyID,
                FromDate = fromDate,
                ToDate = toDate,
                SearchBy = model.CompanyID,
                TicketSearchType = searchtype,
                SearchByValue = searchbyval,
                EventID = eventId,

            };
            return await _ledgerReportQueryHandler.HandleAsync(ledgerQuery);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LedgerExportToExcel(LedgerReport model)
        {
            var companyID = UserHelper.GetCompanyID(User);
            model.CompanyID = companyID;
            if (model == null || model.CompanyID == null || model.FromDate == null || model.ToDate == null)
            {
                return BadRequest("Invalid parameters for exporting ledger reports.");
            }

            var ledgerReports = await GetLedgerData(model);
            if (!ledgerReports.Any())
            {
                return BadRequest("No reports available to export.");
            }

            var filePath = Path.Combine(Path.GetTempPath(), $"LedgerReports_{DateTime.Now:yyyyMMdd}.xlsx");

            var headers = new List<string>
{
    "id",
    "CompanyName",
    "CompanyAccountNo",
    "CompanyEmail",
    "CompanyMobile",
    "CompanyUser",
    "CompanyPan",
    "CompanyCity",
    "CompanyState",
    "BookingRef",
    "PassengerId",
    "Balance",
    "Debit",
    "Credit",
    "BookingStatus",
    "PaymentType",
    "paymentId",
    "CPaymentType",
    "Trip",
    "PNR",
    "Remark",
    "EventTime",
};

            var ledgerReportData = ledgerReports.Select((report, index) => new
            {
                SNo = index + 1,
                report.CompanyName,
                report.AccountID,
                report.Email,
                report.Mobile,
                report.UserType,
                report.Pan_No,
                report.City,
                report.State,
                report.BookingRef,
                report.PaxSegmentID,
                report.Balance,
                report.Debit,
                report.Credit,
                report.BookingStatus,
                report.PaymentType,
                report.PaymentID,
                report.CPaymentType,
                report.Trip,
                report.Airline_PNR_D,
                report.Remark,
                report.EventTime,
            }).ToList();

            var excelGenerator = new GenerateExcel();
            excelGenerator.ExportToExcel(ledgerReportData, filePath, headers, "Ledger_Report");

            var fileBytes = System.IO.File.ReadAllBytes(filePath);
            System.IO.File.Delete(filePath);

            return File(fileBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "LedgerReports.xlsx");
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LedgerExportToPDF(LedgerReport model)
        {
            var companyID = UserHelper.GetCompanyID(User);
            model.CompanyID = companyID;
            if (model == null || model.CompanyID == null || model.FromDate == null || model.ToDate == null)
            {
                return BadRequest("Invalid parameters for exporting ledger reports.");
            }

            var ledgerReports = await GetLedgerData(model);
            if (!ledgerReports.Any())
            {
                return BadRequest("No reports available to export.");
            }

            var pdfFilePath = Path.Combine(Path.GetTempPath(), $"LedgerReports_{DateTime.Now:yyyyMMdd}.pdf");
            var pdfGenerator = new GeneratePDF();

            var ledgerReportData = ledgerReports.Select((report, index) => new
            {
                SNo = index + 1,
                report.CompanyName,
                report.BookingRef,
                report.EventTime,
                report.PaymentType,
                report.Debit,
                report.Credit,
                report.Balance,
                report.Airline_PNR_D,
                report.BookingStatus,
            }).ToList();

            var headers = new List<string>
{
    "SNo",
    "Company",
    "RefNo",
    "Created On",
    "Payments",
    "Debit",
    "Credit",
    "Balance",
    "PNR",
    "Booking Status",
};

            pdfGenerator.ExportToPDF(pdfFilePath, ledgerReportData, headers);

            var fileBytes = System.IO.File.ReadAllBytes(pdfFilePath);
            System.IO.File.Delete(pdfFilePath);

            return File(fileBytes, "application/pdf", "Ledger_Report.pdf");
        }





        public StringBuilder GetLedgerReportHTML(List<LedgerReports> ledgerReportItems, bool FlagforCompanyname, out StringBuilder strCompnayHTMLMOdal, string SessionCompany)
        {
            StringBuilder strHTM = new StringBuilder();
            strCompnayHTMLMOdal = new StringBuilder();




            strHTM.Append(@"<table class='defaultText table ' style='width: 100%; font-size: 11px;  border: 1px solid #e4e6eb;   ' cellspacing='0' cellpadding='5' border='0' id=''>");
            strHTM.Append(@"<thead>");
            strHTM.Append(@"<tr class='hg' style='text-align:left;'>");
            strHTM.Append(@"<td style='width: 3%; text-align:left; padding-left:0px;' ><b> Sno. </b></td>");

            if (FlagforCompanyname.Equals(false))
            {
                strHTM.Append(@"<td style='width: 10%;text-align:left; padding-left:0px;'><b>Company </b></td>");
            }

            //strHTM.Append(@"<td style='width: 7%; text-align:left; padding-left:0px;'><b style='padding:0px;'>Status</b>|<b>Refno. </b></td>");
            strHTM.Append(@"<td style='width: 7%; text-align:left; padding-left:0px;'><b style='padding:0px;'>Refno. </b></td>");
            strHTM.Append(@"<td style='width: 3%; text-align:left; padding-left:0px;'><b style='padding:0px;'>Cnote. </b></td>");
            strHTM.Append(@"<td style='text-align:left;padding-left:0px;'><b>Created On </b></td>");
            strHTM.Append(@"<td style='text-align:left; padding-left:0px;'><b>Payments </b></td>");
            //strHTM.Append(@"<td><b>Remark </b></td>");
            strHTM.Append(@"<td style='text-align:left; padding-left:0px;'><b>Debit </b></td>");
            strHTM.Append(@"<td style='text-align:left; padding-left:0px;'><b>Credit </b></td>");
            strHTM.Append(@"<td style='text-align:left; padding-left:0px;'><b>Balance </b></td>");
            strHTM.Append(@"<td style='text-align:left; padding-left:0px;'><b>PNR </b></td>");
            strHTM.Append(@"<td style='text-align:left; padding-left:0px;border-right: 1px solid #e4e6eb;'><b>Remark </b></td>");
            strHTM.Append(@"<td style='text-align:left; padding-left:0px;border-right: 1px solid #e4e6eb;'><b>By </b></td>");
            strHTM.Append(@"</tr>");
            strHTM.Append(@"</thead>");
            strHTM.Append(@"<tbody>");
            var i = 0;
            foreach (var reportItem in ledgerReportItems)
            {


                int Sno = i + 1;
                string bookref = reportItem.BookingRef.ToString();
                string companyName = reportItem.CompanyName;
                string companyID = reportItem.CompanyID;
                //string Trip = Dtl.Rows[j]["Trip"].ToString();
                string companynm = StringHelper.GetFirstTenCharacter(reportItem.CompanyName);
                companynm = StringHelper.GetFirstCharacterCapital(companynm);
                string eventtime = StringHelper.GetConvertedDateTime(reportItem.EventTime.ToString());
                strHTM.Append(@"<tr style='text-align:left;'>");
                strHTM.Append(@"<td style='width: 3%;'>" + Sno + "</td>");

                if (FlagforCompanyname.Equals(false))
                {
                    strHTM.Append(@"<td style='width: 10%;text-align:left;'>
                        <a class='' id='cmpnynm_" + i + "' accesskey='" + reportItem.CompanyID + "' style='cursor: pointer; padding: 3px; ' onclick='showDetail(\"myModal_" + i + "\");'>" + companynm + "</a>&nbsp;</td>");
                }

                if (int.Parse(bookref) > 0)
                {
                    if (reportItem.Remark.Equals("BOOK-HOTEL") || reportItem.Remark.Equals("REJECT-HOTEL"))
                    {
                        if (SessionCompany.IndexOf("-SA-") != -1 || SessionCompany.IndexOf("C") != -1)
                        {
                            strHTM.Append(@"<td style='width: 7%;'> <a class='' id='bokref_" + i + "' accesskey='" + bookref + "' style='cursor: pointer; padding: 3px; '  onclick='bookrefffhotel_sa(this);'>" + bookref + "</a>&nbsp;</td>");
                        }
                        else
                        {
                            strHTM.Append(@"<td style='width: 7%;'> <a class='' id='bokref_" + i + "' accesskey='" + bookref + "' style='cursor: pointer; padding: 3px; '  onclick='bookrefffhotel(this);'>" + bookref + "</a>&nbsp;</td>");
                        }
                    }
                    else
                    {
                        if (reportItem.IsAirlineOff)
                        {
                            strHTM.Append(@"<td style='width: 7%;'> <a class='' id='bokref_" + i + "' accesskey='" + bookref + "' style='cursor: pointer; padding: 3px; '  onclick='bookAirOffline(this);'>" + bookref + "</a>&nbsp;</td>");
                        }
                        else
                        {
                            if (SessionCompany.IndexOf("-SA-") != -1 || SessionCompany.IndexOf("C") != -1)
                            {
                                strHTM.Append(@"<td style='width: 7%; padding-left:0px; padding-right:0px; '><a class='' id='bokref_" + i + "' accesskey='" + bookref + "' style='cursor: pointer; padding: 3px; '  onclick='printTicketCust(this);'>" + bookref + "</a>&nbsp;</td>");
                            }
                            else
                            {
                                strHTM.Append(@"<td style='width: 7%; padding-left:0px; padding-right:0px; '><a class='' id='bokref_" + i + "' accesskey='" + bookref + "' style='cursor: pointer; padding: 3px; '  onclick='bookrefff(this);'>" + bookref + "</a>&nbsp;</td>");
                            }
                        }
                    }
                }
                else
                {
                    strHTM.Append(@"<td style='width: 7%;'><span>" + bookref + "</td>");
                }

                int iPid = reportItem.PaxSegmentID;

                if (iPid > 0)
                {
                    if (companyID.IndexOf("-SA-") != -1 || companyID.ToString().IndexOf("-C") != -1)
                    {
                        strHTM.Append(@"<td style='width: 7%; padding-left:0px; padding-right:0px; '><a class='' id='PaxNM_" + i + "' accesskey='" + bookref + "," + iPid.ToString() + "' style='cursor: pointer;' onclick='Perpaxticketttt_sa(this);'>" + iPid.ToString() + "</a>&nbsp;</td>");
                    }
                    else
                    {
                        strHTM.Append(@"<td style='width: 7%; padding-left:0px; padding-right:0px; '><a class='' id='PaxNM_" + i + "' accesskey='" + bookref + "," + iPid.ToString() + "' style='cursor: pointer;' onclick='Perpaxticketttt(this);'>" + iPid.ToString() + "</a>&nbsp;</td>");
                    }
                }
                else
                {
                    strHTM.Append(@"<td>" + iPid.ToString() + "</td>");
                }



                strHTM.Append(@"<td>" + eventtime + "</td>");
                //strHTM.Append( @"<td><a href='#' onclick='Payment_Detail(" + j + ")'>Try it</a></td>");
                //strHTM.Append( @"<td><a href='#' onclick='Remark_Detal(" + j + ")'>?</a></td>");
                //strHTM.Append( @"<td><a href='#' onclick='Debet_Detail(" + j + ")'>Debit</a></td>");

                int iPaymentid = 0;
                int.TryParse(reportItem.PaymentID.ToString().Trim(), out iPaymentid);

                if (iPaymentid > 0)
                {
                    strHTM.Append(@"<td>" + reportItem.PaymentType + "-" + iPaymentid.ToString() + "</td>");
                }
                else
                {
                    strHTM.Append(@"<td>" + reportItem.PaymentType + "</td>");
                }




                //strHTM.Append(@"<td><a style='cursor: pointer;' onclick='ShowDetaildiv(DebitDiv_" + j + ")'>" + Dtl.Rows[j]["Debit"].ToString() + "</a></td>");


                strHTM.Append(@"<td>" + reportItem.Debit.ToString() + "</td>");

                strHTM.Append(@"<td>" + reportItem.Credit.ToString() + "</td>");
                //strHTM.Append( @"<td>9</td>");
                //strHTM.Append( @"<td>10</td>");
                strHTM.Append(@"<td>" + reportItem.Balance.ToString() + "</td>");
                string PNR = string.Empty;

                if (reportItem.Trip.Equals("O"))
                {
                    strHTM.Append(@"<td>" + reportItem.Airline_PNR_D.Trim() + "</td>");
                    PNR = reportItem.Airline_PNR_D.Trim();
                }
                else
                {
                    if (reportItem.Airline_PNR_D.Trim() != reportItem.Airline_PNR_A.Trim())
                    {
                        strHTM.Append(@"<td>" + reportItem.Airline_PNR_D.Trim() + " / " + reportItem.Airline_PNR_A.Trim() + "</td>");
                        PNR = reportItem.Airline_PNR_D.Trim() + " / " + reportItem.Airline_PNR_A.Trim();
                    }
                    else
                    {
                        strHTM.Append(@"<td>" + reportItem.Airline_PNR_D.Trim() + "</td>");
                        PNR = reportItem.Airline_PNR_D.Trim();
                    }
                }

                if (reportItem.Remark.Trim().Length > 19)
                {
                    string rem = reportItem.Remark.Trim().Substring(0, 14);
                    strHTM.Append(@"<td style='width: 10%; border-right: 1px solid #e4e6eb;'> <a class='' id='Remarkdet_" + i + "' accesskey='" + reportItem.Remark + "' style='cursor: pointer; padding: 3px; ' data-toggle='collapse in' onclick='showdetailRemark(myModalRemark_" + i + ");'>" + rem + "</a>&nbsp;</td>");
                }
                else
                {
                    strHTM.Append(@"<td style='border-right: 1px solid #e4e6eb;'>" + reportItem.Remark + "</td>");
                }

                if (reportItem.UpdatedBy.Trim().Length > 0 && !reportItem.Remark.Trim().ToUpper().Equals("BOOK-AIR") && !reportItem.Remark.ToUpper().Trim().Equals("REJECT-AIR") && !reportItem.Remark.Trim().ToUpper().Equals("BOOK-HOTEL") && !reportItem.Remark.Trim().ToUpper().Equals("REJECT-HOTEL"))
                {
                    strHTM.Append(@"<td>" + reportItem.UpdatedBy + "</td>");
                }
                else
                {
                    strHTM.Append(@"<td>" + "#" + "</td>");
                }


                strHTM.Append(@"</tr>");
                strHTM.Append(@"<tr>");
                strHTM.Append(@"<td colspan='10' id='myModalRemark_" + i + "' class='collapse' style='display: none; '>");





                //----------------------------------------Remark PopUp Start-------------------------------------------------------------------------

                strHTM.Append(@"<div>");
                strHTM.Append(@"<div class='ipad_position' style='border: 1px solid #D7DFF4; background: #fff; border-radius: 8px; -webkit-border-radius: 8px; -ms-border-radius: 8px; width: 240px; padding-bottom: 1%; box-shadow: 2px 2px 3px #D5DCEF; z-index: 100;min-height: 42px; height: 100%; float:left; margin-left:70%;'>");
                strHTM.Append(@"<div style='float: right; margin: -6px -6px 0 0;'>");
                strHTM.Append(@"<img alt='Close' id='Closebtnpopup_" + i + "' src='//style.zealtravels.in/Airline/image/close.gif' onclick='Closepopupremark(myModalRemark_" + i + ")' />");
                strHTM.Append(@"</div>");
                strHTM.Append(@"<div style='float: left; width: 90%; padding: 5%;'>");
                strHTM.Append(@"<span id='' style='font-size: 10px; height: 20px;'>" + reportItem.Remark + "</span>");
                strHTM.Append(@"</div>");
                strHTM.Append(@"</div>");
                strHTM.Append(@"</div>");

                //----------------------------------------Remark PopUp Start-------------------------------------------------------------------------

                strHTM.Append(@"</td>");

                strHTM.Append(@"</tr>");


                //----------------------------------------Company Detail Model Box Stat-------------------------------------------------------------------------
                strCompnayHTMLMOdal.Append(@"<div class='modal fade in' id='myModal_" + i + "' role='dialog' style='pointer-events: none; display:none;'>");
                strCompnayHTMLMOdal.Append(@"<div class='modal-dialog'>");
                strCompnayHTMLMOdal.Append(@"<!-- Modal content-->");
                strCompnayHTMLMOdal.Append(@"<div class='modal-content' style='pointer-events: auto;'>");
                strCompnayHTMLMOdal.Append(@"<div class='modal-header' style='margin-top: 9px; padding: 6px;'>");
                strCompnayHTMLMOdal.Append(@"<h1 class='pophdi'>DETAIL</h1>");
                strCompnayHTMLMOdal.Append(@"<button id='closeButton_" + i + "' type='button' class='close uc ptpop popclo' data-modal-id='myModal_" + i + "' style=''>&times;</button>");
                strCompnayHTMLMOdal.Append(@"</div>");
                strCompnayHTMLMOdal.Append(@"<div class='panel-body' style='font-size: 11px;padding: 0%;' >");
                strCompnayHTMLMOdal.Append(@"<div class='modal-body hg' style='padding: 0%;'>");
                strCompnayHTMLMOdal.Append(@"<div class='panel-body panel-body1' style=''>");
                strCompnayHTMLMOdal.Append(@"<span class='col-md-3 col-xs-4 offset-0'>");
                strCompnayHTMLMOdal.Append(@"<span style='font-weight: 100;'>Company  :</span>");
                strCompnayHTMLMOdal.Append(@"</span><span class='col-md-6 cc col-xs-8 font-12' id='Compnynm_" + i + "'>" + companyName + "</span>");
                strCompnayHTMLMOdal.Append(@"</div>");
                strCompnayHTMLMOdal.Append(@"<div class='panel-body panel-body1' style=''>");
                strCompnayHTMLMOdal.Append(@"<span class='col-md-3 col-xs-4 offset-0'>");
                strCompnayHTMLMOdal.Append(@"<span style='font-weight: 100;'>AccountID  :</span>");
                strCompnayHTMLMOdal.Append(@"</span><span class='col-md-6 cc col-xs-8 font-12' id='spanaccountt_" + i + "'>" + reportItem.AccountID.ToString() + " | " + reportItem.UserType + "</span>");
                strCompnayHTMLMOdal.Append(@"</div>");
                strCompnayHTMLMOdal.Append(@"<div class='panel-body panel-body1' style=''>");
                strCompnayHTMLMOdal.Append(@"<span class='col-md-3 col-xs-4 offset-0'>");
                strCompnayHTMLMOdal.Append(@"<span style='font-weight: 100;'>Name  :</span>");
                strCompnayHTMLMOdal.Append(@"</span><span class='col-md-6 cc col-xs-8 font-12' id='name_" + i + "'>" + reportItem.Fname + " " + reportItem.Lname + "</span>");
                strCompnayHTMLMOdal.Append(@"</div>");
                strCompnayHTMLMOdal.Append(@"<div class='panel-body panel-body1' style=''>");
                strCompnayHTMLMOdal.Append(@"<span class='col-md-3 col-xs-4 offset-0'>");
                strCompnayHTMLMOdal.Append(@"<span style='font-weight: 100;'>Mobile  :</span>");
                strCompnayHTMLMOdal.Append(@"</span><span class='col-md-6 cc col-xs-8 font-12' id='mobile_" + i + "'>" + reportItem.Mobile + "</span>");
                strCompnayHTMLMOdal.Append(@"</div>");
                strCompnayHTMLMOdal.Append(@"<div class='panel-body panel-body1' style=''>");
                strCompnayHTMLMOdal.Append(@"<span class='col-md-3 col-xs-4 offset-0'>");
                strCompnayHTMLMOdal.Append(@"<span style='font-weight: 100;'>Email  :</span>");
                strCompnayHTMLMOdal.Append(@"</span><span class='col-md-6 cc col-xs-8 font-12' id='email_" + i + "'>" + reportItem.Email + "</span>");
                strCompnayHTMLMOdal.Append(@"</div>");
                strCompnayHTMLMOdal.Append(@"<div class='panel-body panel-body1' style=''>");
                strCompnayHTMLMOdal.Append(@"<span class='col-md-3 col-xs-4 offset-0'>");
                strCompnayHTMLMOdal.Append(@"<span style='font-weight: 100;'>City  :</span>");
                strCompnayHTMLMOdal.Append(@"</span><span class='col-md-6 cc col-xs-8 font-12' id='city_" + i + "'>" + reportItem.State + " | " + reportItem.City + "</span>");
                strCompnayHTMLMOdal.Append(@"</div>");
                strCompnayHTMLMOdal.Append(@"<div class='panel-body panel-body1' style=''>");
                strCompnayHTMLMOdal.Append(@"<span class='col-md-3 col-xs-4 offset-0'>");
                strCompnayHTMLMOdal.Append(@"<span style='font-weight: 100;' >Pan No.  :</span>");
                strCompnayHTMLMOdal.Append(@"</span><span class='col-md-6 cc col-xs-8 font-12' id='panno_" + i + "'>" + reportItem.Pan_No + "</span>");
                strCompnayHTMLMOdal.Append(@"</div>");
                strCompnayHTMLMOdal.Append(@"</div>");
                strCompnayHTMLMOdal.Append(@"</div>");
                strCompnayHTMLMOdal.Append(@"</div>");
                strCompnayHTMLMOdal.Append(@"</div>");
                strCompnayHTMLMOdal.Append(@"</div>");
                //----------------------------------------Company Detail Model Box End-------------------------------------------------------------------------
                i++;
            }
            strHTM.Append(@"</tbody>");
            strHTM.Append(@"</table>");

            return strHTM;
        }

        #endregion

        #region #FligtBooking
        [HttpGet]
        [Route("/report/flightbookings")]
        public async Task<IActionResult> FlightBookings()
        {
            var companyID = UserHelper.GetCompanyID(User);
            var today = DateTime.Now.Date;

            var model = new FlightBookingReports
            {
                FromDate = today,
                ToDate = today,
                CompanyID = companyID,
                FilterFromDate = today,
                FilterToDate = today,
                TicketSearchType = "SEARCH"
            };

            var flightBookingReports = await GetFlightBookingData(model);
            StringBuilder strCompnayHTMLMOdal = new StringBuilder();
            StringBuilder strSales = new StringBuilder();

            StringBuilder reportHTML = new StringBuilder();
            if (flightBookingReports.Any())
            {
                reportHTML = GetFlightBookingReportHTML(flightBookingReports, false, out StringBuilder strHTMMOdal, out StringBuilder strSale, UserHelper.GetCompanyID(User));
                strCompnayHTMLMOdal = strHTMMOdal;
                strSales = strSale;
            }
            var viewModel = new FlightBookingListResponse
            {
                CompanyID = model.CompanyID,
                FromDate = model.FromDate,
                ToDate = model.ToDate,
                FilterFromDate = model.FilterFromDate == DateTime.MinValue ? DateTime.Now.Date : model.FilterFromDate,
                FilterToDate = model.FilterToDate == DateTime.MinValue ? DateTime.Now.Date : model.FilterToDate,
                PNR = model.PNR,
                TicketNumber = model.TicketNumber,
                PassengerName = model.PassengerName,
                BookingReference = model.BookingReference,
                TicketSearchType = model.TicketSearchType,
                ReportTableHTML = !string.IsNullOrEmpty(reportHTML.ToString()) ? reportHTML.ToString() : null,
                ReportTableCompanyPopup = strCompnayHTMLMOdal.ToString(),
                SalesTable = strSales.ToString()


            };
            //return viewModel;
            if (string.IsNullOrEmpty(viewModel.ReportTableHTML))
            {
                viewModel.ReportTableHTML = string.Empty;
            }

            return View(viewModel);
        }

        [HttpPost]
        [Route("/report/flightbookings")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> FlightBookings(FlightBookingReports model)
        {
            //var companyID = UserHelper.GetCompanyID(User);
            var viewModel = new FlightBookingListResponse();

            if (ModelState.IsValid)
            {
                try
                {


                    var companyId = User.FindFirstValue("CompanyID");


                    if (!string.IsNullOrEmpty(model.CompanyNameWithAccountId))
                    {
                        string[] compin = model.CompanyNameWithAccountId.Split('[');
                        string[] datare = compin[1].Split(']');
                        string accoutnId = datare[0];
                        var query = new CompanyIdByAccountIdQuery { AccountId = Convert.ToInt32(accoutnId) };
                        companyId = await _getCompanyIDByAccountIDQueryHandler.HandleAsync(query);

                    }

                    if (string.IsNullOrEmpty(companyId))
                    {
                        ModelState.AddModelError("", "Company ID not found.");
                        return View(viewModel);
                    }

                    model.CompanyID = companyId;
                    var flightBookingReports = await GetFlightBookingData(model);

                    StringBuilder strCompnayHTMLMOdal = new StringBuilder();
                    StringBuilder strSales = new StringBuilder();
                    StringBuilder reportHTML = new StringBuilder();
                    if (flightBookingReports.Any())
                    {

                        reportHTML = GetFlightBookingReportHTML(flightBookingReports, false, out StringBuilder strHTMMOdal, out StringBuilder strSale, UserHelper.GetCompanyID(User));
                        strCompnayHTMLMOdal = strHTMMOdal;
                        strSales = strSale;
                    }
                    viewModel = new FlightBookingListResponse
                    {
                        CompanyID = model.CompanyID,
                        FromDate = model.FromDate,
                        ToDate = model.ToDate,
                        FilterFromDate = model.FilterFromDate == DateTime.MinValue ? DateTime.Now.Date : model.FilterFromDate,
                        FilterToDate = model.FilterToDate == DateTime.MinValue ? DateTime.Now.Date : model.FilterToDate,
                        PNR = model.PNR,
                        TicketNumber = model.TicketNumber,
                        PassengerName = model.PassengerName,
                        BookingReference = model.BookingReference,
                        TicketSearchType = model.TicketSearchType,
                        ReportTableHTML = !string.IsNullOrEmpty(reportHTML.ToString()) ? reportHTML.ToString() : null,
                        ReportTableCompanyPopup = strCompnayHTMLMOdal.ToString(),
                        CompanyNameWithAccountId = model.CompanyNameWithAccountId,
                        SalesTable = strSales.ToString()


                    };
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "An error occurred while fetching the ledger reports: " + ex.Message);
                }
            }

            return View(viewModel);
        }


        private async Task<List<AirBookingReport>> GetFlightBookingData(FlightBookingReports model)
        {

            var txtPNR = !string.IsNullOrEmpty(model.PNR) ? model.PNR.Trim() : string.Empty;
            var Ticketno = !string.IsNullOrEmpty(model.TicketNumber) ? model.TicketNumber : string.Empty;
            var PaxName = !string.IsNullOrEmpty(model.PassengerName) ? model.PassengerName : string.Empty;
            var bookingref = !string.IsNullOrEmpty(model.BookingReference) ? model.BookingReference.Trim() : string.Empty;
            var searchtype = model.TicketSearchType;
            var searchbyval = string.Empty;
            var fromDate = model.FromDate;
            var toDate = model.ToDate;


            if (model.TicketSearchType == "PAYMENTHOLD")
            {
                searchtype = model.TicketSearchType;
                searchbyval = PaxName;
            }
            else if (PaxName != "")
            {
                searchtype = "PAX";
                searchbyval = PaxName;
                fromDate = model.FilterFromDate;
                toDate = model.FilterToDate;
            }
            else
            {
                if (bookingref != "")
                {
                    searchtype = "REF";
                    searchbyval = bookingref;
                }
                if (Ticketno != "")
                {
                    searchtype = "TKT";
                    searchbyval = Ticketno;
                }
                else if (txtPNR != "")
                {
                    searchtype = "PNR";
                    searchbyval = txtPNR;
                }
            }

            var FlightBookingQuery = new AirBookingQuery
            {
                CompanyId = model.CompanyID,
                FromDate = fromDate,
                ToDate = toDate,
                SearchBy = model.CompanyID,
                TicketSearchType = searchtype,
                SearchByValue = searchbyval,
            };
            return await _flightBookingReportQueryHandler.HandleAsync(FlightBookingQuery);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> FlightBookingExportToExcel(FlightBookingReports model)
        {
            var companyID = UserHelper.GetCompanyID(User);
            model.CompanyID = companyID;

            if (model == null || model.CompanyID == null || model.FromDate == null || model.ToDate == null)
            {
                return BadRequest("Invalid parameters for exporting flight bookings.");
            }

            var flightBookingReports = await GetFlightBookingData(model);

            if (flightBookingReports == null || !flightBookingReports.Any())
            {
                return BadRequest("No reports available to export.");
            }

            var filePath = Path.Combine(Path.GetTempPath(), $"FlightBookings_{DateTime.Now:yyyyMMdd}.xlsx");

            var headers = new List<string>
            {
                "id",
                "CompanyName",
                "CompanyAccountNo",
                "CompanyEmail",
                "CompanyMobile",
                "CompanyUser",
                "CompanyPan",
                "CompanyCity",
                "CompanyState",
                "CompanyPerson",
                "BookingRef",
                "Sector",
                "Trip",
                "BookingStatus",
                "Origin",
                "Destination",
                "OutboundCarrier",
                "InboundCarrier",
                "PassengerId",
                "PassengerType",
                "PassengerName",
                "PassengerMobileNo",
                "PassengerEmailId",
                "TickerNo",
                "OutboundPNR",
                "InboundPNR",
                "OutboundDeparture",
                "InboundDeparture",
                "TotalFare",
                "TotalCommission",
                "EventTime",

            };

            var flightBookingData = flightBookingReports.Select((report, index) => new
            {
                SNo = index + 1,
                report.CompanyName,
                report.AccountID,
                report.Email,
                report.Mobile,
                report.UserType,
                report.Pan_No,
                report.City,
                report.State,
                FullName = report.Fname + " " + report.Lname,
                report.BookingRef,
                report.Sector,
                report.Trip,
                report.BookingStatus,
                report.Origin,
                report.Destination,
                report.CarrierCode_D,
                report.CarrierCode_A,
                report.Pax_SegmentID,
                report.PaxType,
                FullPaxName = report.Title + " " + report.First_Name + " " + report.Last_Name,
                report.Pax_MobileNo,
                report.Pax_Email,
                report.TicketNo,
                report.Airline_PNR_D,
                report.Airline_PNR_A,
                report.DepartureDate_D,
                report.DepartureDate_A,
                report.TotalFare,
                report.TotalCommission,
                report.EventTime,
            }).ToList();

            var excelGenerator = new GenerateExcel();

            excelGenerator.ExportToExcel(flightBookingData, filePath, headers, "Ticket_Report");

            var fileBytes = System.IO.File.ReadAllBytes(filePath);
            System.IO.File.Delete(filePath);

            return File(fileBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "FlightBookings.xlsx");
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> FlightBookingExportToPDF(FlightBookingReports model)
        {
            var companyID = UserHelper.GetCompanyID(User);
            model.CompanyID = companyID;

            if (model == null || model.CompanyID == null || model.FromDate == null || model.ToDate == null)
            {
                return BadRequest("Invalid parameters for exporting flight bookings.");
            }

            var flightBookingReports = await GetFlightBookingData(model);

            if (flightBookingReports == null || !flightBookingReports.Any())
            {
                return BadRequest("No reports available to export.");
            }

            var pdfFilePath = Path.Combine(Path.GetTempPath(), $"FlightBookings_{DateTime.Now:yyyyMMdd}.pdf");

            var pdfGenerator = new GeneratePDF();

            var flightBookingData = flightBookingReports.Select((report, index) => new
            {
                SNo = index + 1,
                report.CompanyName,
                report.BookingRef,
                report.Trip,
                Sector = string.IsNullOrWhiteSpace(report.Origin)
            ? report.Origin
            : $"{report.Origin} - {report.Destination}",
                CarrierCode = string.IsNullOrWhiteSpace(report.CarrierCode_A)
            ? report.CarrierCode_D
            : $"{report.CarrierCode_D} | {report.CarrierCode_A}",
                PNR = string.IsNullOrWhiteSpace(report.Airline_PNR_A)
            ? report.Airline_PNR_D
            : $"{report.Airline_PNR_D} | {report.Airline_PNR_A}",
                DepartureDate = string.IsNullOrWhiteSpace(report.DepartureDate_A)
            ? report.DepartureDate_D
            : $"{report.DepartureDate_D} | {report.DepartureDate_A}",
                report.BookingStatus,
            }).ToList();

            var headers = new List<string>
{
    "SNo",
    "Company",
    "BookingRef",
    "Trip",
    "Sector",
    "CarrierCode",
    "PNR",
    "DepartureDate",
    "Status",
};

            pdfGenerator.ExportToPDF(pdfFilePath, flightBookingData, headers);
            var fileBytes = System.IO.File.ReadAllBytes(pdfFilePath);
            System.IO.File.Delete(pdfFilePath);
            return File(fileBytes, "application/pdf", "Ticket_Report.pdf");
        }



        public StringBuilder GetFlightBookingReportHTML(List<AirBookingReport> flightBookingReportItems, bool FlagforCompanyname, out StringBuilder strCompnayHTMLMOdal, out StringBuilder strSale, string SessionCompany)
        {
            StringBuilder sbhtml = new StringBuilder();
            strCompnayHTMLMOdal = new StringBuilder();
            strSale = new StringBuilder();
            ArrayList ARref = new ArrayList();
            string Trip = string.Empty;
            var i = 0;


            var groupedReports = flightBookingReportItems
       .GroupBy(row => row.BookingRef)
       .Select(g => new AirBookingReport
       {
           CompanyID = g.FirstOrDefault()?.CompanyID,
           BookingRef = g.Key,
           Trip = g.FirstOrDefault()?.Trip,
           CarrierCode_A = g.FirstOrDefault()?.CarrierCode_A,
           CarrierCode_D = g.FirstOrDefault()?.CarrierCode_D,
           UpdatedBy = g.FirstOrDefault()?.UpdatedBy,
           Origin = g.FirstOrDefault()?.Origin,
           Destination = g.FirstOrDefault()?.Destination,
           DepartureDate_A = g.FirstOrDefault()?.DepartureDate_A,
           DepartureDate_D = g.FirstOrDefault()?.DepartureDate_D,
           BookingStatus = g.FirstOrDefault()?.BookingStatus,
           CompanyName = g.FirstOrDefault()?.CompanyName,
           City = g.FirstOrDefault()?.City,
           State = g.FirstOrDefault()?.State,
           AccountID = g.Key,
           UserType = g.FirstOrDefault()?.UserType,
           Fname = g.FirstOrDefault()?.Fname,
           Lname = g.FirstOrDefault()?.Lname,
           Mobile = g.FirstOrDefault()?.Mobile,
           Email = g.FirstOrDefault()?.Email,
           Pan_No = g.FirstOrDefault()?.Pan_No,
           Airline_PNR_A = g.FirstOrDefault()?.Airline_PNR_A,
           Airline_PNR_D = g.FirstOrDefault()?.Airline_PNR_D,
           Pax_Email = g.FirstOrDefault()?.Pax_Email,
           Pax_MobileNo = g.FirstOrDefault()?.Pax_MobileNo,
           EventTime = g.FirstOrDefault()?.EventTime ?? DateTime.MinValue,
           IsPaymentHold_D = g.FirstOrDefault()?.IsPaymentHold_D ?? false,
           IsPaymentHold_A = g.FirstOrDefault()?.IsPaymentHold_A ?? false,
           IsPaymentHold = g.FirstOrDefault()?.IsPaymentHold ?? string.Empty,
           IsUpdated = g.FirstOrDefault()?.IsUpdated ?? false,
           IsRejected = g.FirstOrDefault()?.IsRejected ?? false,

       })
       .ToList();


            foreach (var reportItem in groupedReports)
            {
                int Sno = i + 1;
                Trip = reportItem.Trip.ToString();
                string companyName = reportItem.CompanyName;
                string companynm = StringHelper.GetUpper(reportItem.CompanyName);
                bool Isupdated = reportItem.IsUpdated;
                bool Isrejected = reportItem.IsRejected;
                //This Two Field is NotFound in Store Proc
                string IsPaymentHold_D = reportItem.IsPaymentHold_D.ToString().Trim().ToUpper() == "TRUE" ? " | P-HOLD" : "";
                string IsPaymentHold_A = reportItem.IsPaymentHold_A.ToString().Trim().ToUpper() == "TRUE" ? " | P-HOLD" : "";
                sbhtml.Append(@"<div class='col-md-12 col-xs-12 mainbo bgbox' >");
                sbhtml.Append(@"<div class='col-md-12 col-xs-12 offset-0 paddbor'>");
                sbhtml.Append(@"<div class='col-md-8 col-xs-12 offset-0' >");
                sbhtml.Append(@"<table style='font-size: 12px;'>");
                sbhtml.Append(@"<tr>");
                sbhtml.Append(@"<td colspan='2' style='width:300px;  border-bottom: 1px solid #ccc; padding-bottom:3px; '>");
                sbhtml.Append(@"<a class='' id='cmpnynm_" + i + "' accesskey='" + reportItem.CompanyID.ToString() + "' style='cursor: pointer; padding: 3px; font-size: 12px; ' onclick='showDetail(\"myModal_" + i + "\");'>" + companynm + "</a>&nbsp;");
                sbhtml.Append(@"</td>");

                if (SessionCompany.IndexOf("AD-") != -1 || SessionCompany.IndexOf("ST-") != -1)
                {
                    sbhtml.Append(@"<td style='width:300px; font-size: 12px;   border-bottom: 1px solid #ccc; '>Ref.  " + reportItem.BookingRef.ToString() + "  |  " + reportItem.UpdatedBy?.ToString().Trim() ?? "" + "</td>");
                }
                else
                {
                    sbhtml.Append(@"<td style='width:300px; font-size: 12px;   border-bottom: 1px solid #ccc; '>Ref.  " + reportItem.BookingRef.ToString() + "</td>");
                }

                sbhtml.Append(@"</tr>");
                sbhtml.Append(@"<tr>");
                sbhtml.Append(@"<td style='width:300px;   border-bottom: 1px solid #ccc; padding-bottom:3px; padding-top:3px;'>" + reportItem.CarrierCode_D.ToString() + " | " + reportItem.DepartureDate_D.ToString() + "<br/>(" + reportItem.Origin.ToString() + " - " + reportItem.Destination.ToString() + ")</td>");
                if (Trip == "R")
                {
                    sbhtml.Append(@"<td style='width:400px;  border-bottom: 1px solid #ccc; '>" + reportItem.CarrierCode_A.ToString() + " | " + reportItem.DepartureDate_A.ToString() + "<br/>(" + reportItem.Destination.ToString() + " - " + reportItem.Origin.ToString() + ")</td>");
                }

                string Status = reportItem.BookingStatus.ToString();
                if (Status.ToUpper() == "CNF")
                {

                    Status = "CNF";
                    Status = Status.Replace("CNF", "<span style='color: green;'><span>CNF</span></span>");

                }
                if (Status.ToUpper() == "REJECT")
                {
                    Status = "REJECT";
                    Status = Status.Replace("REJECT", "<span style='color: rgb(255,0,0);'><span>REJECT</span></span>");
                }
                if (Status.ToUpper() == "CANCEL")
                {
                    Status = "CANCEL";
                    Status = Status.Replace("CANCEL", "<span style='color:blue;'><span>CANCEL</span></span>");
                }
                if (Status.ToUpper() == "PENDING")
                {
                    Status = "PENDING";
                    Status = Status.Replace("PENDING", "<span style='color: #b0b01a;'><b>PENDING</span></span>");
                }
                if (Status.ToUpper() == "PAYMENT_HOLD")
                {
                    Status = "PAYMENT_HOLD";
                    Status = Status.Replace("PAYMENT_HOLD", "<span style='color: #b0b01a;'><b>PAYMENT_HOLD</span></span>");
                }

                sbhtml.Append(@"<td style='width:200px; border-bottom: 1px solid #ccc; '>Status | " + Status + "</td>");
                sbhtml.Append(@"</tr> <tr><td colspan='3' style='border-top:1px; solid #000;'></td></tr>");

                //for (int j = 0; j < reportItem.Count; j++)
                //{
                //int Sno = j + 1;
                foreach (var item in flightBookingReportItems.Where(r => r.BookingRef == reportItem.BookingRef))
                {
                    string Fullname = StringHelper.GetUpper(item.Title.ToString().Trim() + " " + item.First_Name.ToString().Trim() + " " + item.Last_Name.ToString().Trim());
                    sbhtml.Append(@"<tr>");


                    if (SessionCompany.Trim().IndexOf("-SA-") != -1 || SessionCompany.Trim().IndexOf("C-") != -1)
                    {
                        if (item.CancelDetail != null && item.CancelDetail.ToString().Trim().ToUpper().Length > 0)
                        {
                            sbhtml.Append(@"<td colspan='2' style='width:326px; padding-top: 6px; font-size:11px; padding-bottom: 3px;'>" + Sno + ".  <a class='' id='PaxNM_" + i + "' accesskey='" + item.BookingRef.ToString() + "," + item.Pax_SegmentID.ToString() + "' style='cursor: pointer;' onclick='PerpaxticketCus(this);'>" + Fullname + " (" + item.PaxType.ToString().Trim() + ")</a>&nbsp;&nbsp;(<span style='color:blue;'>CAN<span>)</td>");
                        }
                        else
                        {
                            sbhtml.Append(@"<td colspan='2' style='width:326px; padding-top: 6px; font-size:11px; padding-bottom: 3px;'>" + Sno + ".  <a class='' id='PaxNM_" + i + "' accesskey='" + item.BookingRef.ToString() + "," + item.Pax_SegmentID.ToString() + "' style='cursor: pointer;' onclick='PerpaxticketCus(this);'>" + Fullname + " (" + item.PaxType.ToString().Trim() + ")</a></td>");
                        }
                    }
                    else
                    {
                        if (item.CancelDetail != null && item.CancelDetail.ToString().Trim().ToUpper().Length > 0)
                        {
                            sbhtml.Append(@"<td colspan='2' style='width:326px; padding-top: 6px; font-size:11px; padding-bottom: 3px;'>" + Sno + ".  <a class='' id='PaxNM_" + i + "' accesskey='" + item.BookingRef.ToString() + "," + item.Pax_SegmentID.ToString() + "' style='cursor: pointer;' onclick='Perpaxticket(this);'>" + Fullname + " (" + item.PaxType.ToString().Trim() + ")</a>&nbsp;&nbsp;(<span style='color:blue;'>CAN<span>)</td>");
                        }
                        else
                        {
                            sbhtml.Append(@"<td colspan='2' style='width:326px; padding-top: 6px; font-size:11px; padding-bottom: 3px;'>" + Sno + ".  <a class='' id='PaxNM_" + i + "' accesskey='" + item.BookingRef.ToString() + "," + item.Pax_SegmentID.ToString() + "' style='cursor: pointer;' onclick='Perpaxticket(this);'>" + Fullname + " (" + item.PaxType.ToString().Trim() + ")</a></td>");
                        }
                    }



                    if (item.PaxType.ToString().Trim().ToUpper() != "ADT")
                    {
                        sbhtml.Append(@"<td style='width:300px; padding-top: 10px;'>(" + item.DOB.ToString().Trim() + ")</td>");
                    }
                    else
                    {
                        sbhtml.Append(@"<td style='width:300px; padding-top: 10px;'></td>");
                    }
                    sbhtml.Append(@"</tr>");
                }
                string bookingdate = StringHelper.GetConvertedDateTime(reportItem.EventTime.ToString());

                sbhtml.Append(@"</table>");
                sbhtml.Append(@"</div>");
                sbhtml.Append(@"<div class='col-md-1 col-xs-12'></div>");
                sbhtml.Append(@"<div class='col-md-3 offset-0 col-xs-12 pnbox' >");
                sbhtml.Append(@"<p style='margin: 7px;'><b style='font-size:12px;'>PNR :</b>");
                sbhtml.Append(@"<tt>" + reportItem.CarrierCode_D.ToString() + "</tt>  |  <tt id='PNR'>" + reportItem.Airline_PNR_D.ToString() + "</tt><tt>" + IsPaymentHold_D + "</tt></p>");

                if (Trip == "R")
                {
                    sbhtml.Append(@"<p style='margin: 7px;'><b style='font-size:12px;'>PNR :</b>");
                    sbhtml.Append(@"<tt>" + reportItem.CarrierCode_A.ToString() + "</tt>  |  <tt id='Tt1'>" + reportItem.Airline_PNR_A.ToString() + "</tt><tt>" + IsPaymentHold_A + "</tt></p>");
                }

                sbhtml.Append(@"</div>");
                sbhtml.Append(@"</div>");
                sbhtml.Append(@"<div class='col-md-12 offset-0'>");
                sbhtml.Append(@"<div class='col-md-12 text-right' style='font-size:11px;' >");
                sbhtml.Append(@"");
                sbhtml.Append(@"</div>");

                sbhtml.Append(@"<div class='col-md-12 offset-0' style='border-top:1px solid #ccc;'>");
                sbhtml.Append(@"<div class='col-md-9 offset-0'> <p style='margin: 4px;  font-size: 12px;'>");
                sbhtml.Append(@"<strong class='offset-0'>Booked On : &nbsp; </strong><span>" + bookingdate + " </span>");
                sbhtml.Append(@"<strong>Email : </strong><span>" + StringHelper.GetLower(reportItem.Pax_Email.ToString().Trim()) + " </span>");
                sbhtml.Append(@"<strong>Mobile : </strong><span>" + reportItem.Pax_MobileNo.ToString().Trim() + " </span>");
                sbhtml.Append(@"</p></div>");
                sbhtml.Append(@"</div>");
                sbhtml.Append(@"</div>");

                sbhtml.Append(@"<div class='col-md-12 offset-0' style='background:#EAF0F8;'>");
                sbhtml.Append(@"<div class='col-md-9 offset-0' >");
                sbhtml.Append(@"<p style='margin: 4px;  padding-top: 1px;  padding-bottom: 1px;  font-size: 12px;'>");
                sbhtml.Append(@"<a href='#' class='not-active'>Service Request</a>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<a href='#' class='not-active'> Service </a>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;");

                if ((SessionCompany.IndexOf("AD") != -1 || SessionCompany.IndexOf("ST-") != -1) || (SessionCompany.IndexOf("A-") != -1 && SessionCompany.IndexOf("-SA-") == -1 && SessionCompany.IndexOf("C-") == -1))
                {
                    if (Isupdated && !Isrejected)
                    {
                        sbhtml.Append(@"<a  id='invoice_" + i + "' accesskey='" + reportItem.BookingRef.ToString().Trim() + "'  style='cursor: pointer;     margin-left: -30px;' onclick='printinvoice(this);'> <i class='fa fa-file' title='Invoice' style='font-size: 17px; color: #ff0b0b;' ></i>  </a>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;");
                    }
                }

                if (reportItem.CompanyID.ToString().Trim().IndexOf("-SA-") != -1 || reportItem.CompanyID.ToString().Trim().IndexOf("C-") != -1)
                {
                    if (Isupdated && !Isrejected)
                    {
                        sbhtml.Append(@"<a  id='Cinvoice_" + i + "' accesskey='" + reportItem.BookingRef.ToString().Trim() + "'  style='cursor: pointer;     margin-left: -30px;' onclick='printinvoiceCust(this);'> <i class='fa fa-file' title='Customer Invoice' style='font-size: 17px; color: #ffbf0b;' ></i>  </a>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;");
                    }
                }

                if (reportItem.CompanyID.ToString().Trim().IndexOf("-SA-") != -1 || reportItem.CompanyID.ToString().Trim().IndexOf("C-") != -1)
                {
                    sbhtml.Append(@"<a  id='CTicket_" + i + "' accesskey='" + reportItem.BookingRef.ToString().Trim() + "'  style='cursor: pointer;     margin-left: -30px;' onclick='printTicketCust(this);'> <i class='fa fa-print' title='Customer Ticket' style='font-size: 17px; color: #ffbf0b;' ></i>  </a>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;");
                }

                if ((SessionCompany.IndexOf("AD") != -1 || SessionCompany.IndexOf("ST-") != -1) || (SessionCompany.IndexOf("A-") != -1 && SessionCompany.IndexOf("-SA-") == -1 && SessionCompany.IndexOf("C-") == -1))
                {
                    sbhtml.Append(@"<a  id='Print_" + i + "' accesskey='" + reportItem.BookingRef.ToString().Trim() + "' style='cursor: pointer;' onclick='bookrefff(this);'><i class='fa fa-print' title='Print Ticket' style='font-size: 17px; color: #0ba1ff;'></i></a>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;");
                }

                sbhtml.Append(@"</p>");
                sbhtml.Append(@"</div>");
                sbhtml.Append(@"<div class='col-md-3 offset-0' style='text-align: right;  margin-top: 2px;'>");

                //if (Convert.ToBoolean(reportItem.IsUpdated.ToString().Trim()) == true && Convert.ToBoolean(reportItem.IsPaymentHold.ToString().Trim()) == true)
                //{
                //    sbhtml.Append(@"<a  class='btn btn-default' id='PayT_" + i + "' accesskey='" + reportItem.BookingRef.ToString().Trim() + "' style='cursor:pointer;font-size:11px;background-color:OrangeRed;color:white;font-weight:bold;padding-left:20px;padding-right:20px; ' onclick='Payticket(this);'>  Pay  </a>");
                //}

                if (Convert.ToBoolean(reportItem.IsUpdated.ToString().Trim()) == true && reportItem.BookingRef.ToString().Trim().ToUpper() != "REJECT")
                {
                    sbhtml.Append(@"<a  class='btn btn-default' id='CancelT_" + i + "' accesskey='" + reportItem.BookingRef.ToString().Trim() + "' style='cursor: pointer;font-size: 11px; ' onclick='Cancelticket(this);'>Cancel</a>");
                }


                sbhtml.Append(@"</div>");
                sbhtml.Append(@"</div>");
                sbhtml.Append(@"</div>");

                strCompnayHTMLMOdal.Append(@"<div class='modal fade in' id='myModal_" + i + "' role='dialog' style='pointer-events: none;'>");
                strCompnayHTMLMOdal.Append(@"<div class='modal-dialog'>");
                strCompnayHTMLMOdal.Append(@"<!-- Modal content-->");
                strCompnayHTMLMOdal.Append(@"<div class='modal-content' style='pointer-events: auto;'>");
                strCompnayHTMLMOdal.Append(@"<div class='modal-header' style='margin-top: 10px; padding: 5px;'>");
                strCompnayHTMLMOdal.Append(@"<h1 class='pophdi'>DETAIL</h1>");
                strCompnayHTMLMOdal.Append(@"<button id='closeButton_" + i + "' type='button' class='close uc ptpop popclo' data-modal-id='myModal_" + i + "' style=''>&times;</button>");
                strCompnayHTMLMOdal.Append(@"</div>");
                strCompnayHTMLMOdal.Append(@"<div class='panel-body' style='padding: 0%;' >");
                strCompnayHTMLMOdal.Append(@"<div class='modal-body hg' style='padding: 0%;'>");
                strCompnayHTMLMOdal.Append(@"<div class='panel-body panel-body1' style=''>");
                strCompnayHTMLMOdal.Append(@"<span class='col-md-3 col-xs-6 offset-0'>");
                strCompnayHTMLMOdal.Append(@"<span style='font-weight: 100;'>Company  :</span>");
                strCompnayHTMLMOdal.Append(@"</span><span class='col-md-6 cc col-xs-6 font-12' id='Compnynm_" + i + "'>" + StringHelper.GetUpper(reportItem.CompanyName.ToString()) + "</span>");
                strCompnayHTMLMOdal.Append(@"</div>");
                strCompnayHTMLMOdal.Append(@"<div class='panel-body panel-body1' style=''>");
                strCompnayHTMLMOdal.Append(@"<span class='col-md-3 col-xs-6 offset-0'>");
                strCompnayHTMLMOdal.Append(@"<span style='font-weight: 100;'>AccountID  :</span>");
                strCompnayHTMLMOdal.Append(@"</span><span class='col-md-6 cc col-xs-6 font-12' id='spanaccountt_" + i + "'>" + reportItem.AccountID.ToString() + " | " + reportItem.UserType.ToString() + "</span>");
                strCompnayHTMLMOdal.Append(@"</div>");
                strCompnayHTMLMOdal.Append(@"<div class='panel-body panel-body1' style=''>");
                strCompnayHTMLMOdal.Append(@"<span class='col-md-3 col-xs-6 offset-0'>");
                strCompnayHTMLMOdal.Append(@"<span style='font-weight: 100;'>Name : </span>");
                strCompnayHTMLMOdal.Append(@"</span><span class='col-md-6 cc col-xs-6 font-12' id='name_" + i + "'>" + StringHelper.GetUpper(reportItem.Fname.ToString() + " " + reportItem.Lname.ToString()) + "</span>");
                strCompnayHTMLMOdal.Append(@"</div>");
                strCompnayHTMLMOdal.Append(@"<div class='panel-body panel-body1' style=''>");
                strCompnayHTMLMOdal.Append(@"<span class='col-md-3 col-xs-6 offset-0'>");
                strCompnayHTMLMOdal.Append(@"<span style='font-weight: 100;'>Mobile  :</span>");
                strCompnayHTMLMOdal.Append(@"</span><span class='col-md-6 cc col-xs-6 font-12' id='mobile_" + i + "'>" + reportItem.Mobile.ToString() + "</span>");
                strCompnayHTMLMOdal.Append(@"</div>");
                strCompnayHTMLMOdal.Append(@"<div class='panel-body panel-body1' style=''>");
                strCompnayHTMLMOdal.Append(@"<span class='col-md-3 col-xs-6 offset-0'>");
                strCompnayHTMLMOdal.Append(@"<span style='font-weight: 100;'>Email : </span>");
                strCompnayHTMLMOdal.Append(@"</span><span class='col-md-6 cc col-xs-6 font-12' id='email_" + i + "'>" + StringHelper.GetLower(reportItem.Email.ToString()) + "</span>");
                strCompnayHTMLMOdal.Append(@"</div>");
                strCompnayHTMLMOdal.Append(@"<div class='panel-body panel-body1' style=''>");
                strCompnayHTMLMOdal.Append(@"<span class='col-md-3 col-xs-6 offset-0'>");
                strCompnayHTMLMOdal.Append(@"<span style='font-weight: 100;'>City : </span>");
                strCompnayHTMLMOdal.Append(@"</span><span class='col-md-6 cc col-xs-6 font-12' id='city_" + i + "'>" + StringHelper.GetUpper(reportItem.State.ToString() + " | " + reportItem.City.ToString()) + "</span>");
                strCompnayHTMLMOdal.Append(@"</div>");
                strCompnayHTMLMOdal.Append(@"<div class='panel-body panel-body1' style=''>");
                strCompnayHTMLMOdal.Append(@"<span class='col-md-3 col-xs-6 offset-0'>");
                strCompnayHTMLMOdal.Append(@"<span style='font-weight: 100;'>Pan No. : </span>");
                strCompnayHTMLMOdal.Append(@"</span><span class='col-md-6 cc col-xs-6 font-12' id='panno_" + i + "'>" + StringHelper.GetUpper(reportItem.Pan_No.ToString()) + "</span>");
                strCompnayHTMLMOdal.Append(@"</div>");
                strCompnayHTMLMOdal.Append(@"</div>");
                strCompnayHTMLMOdal.Append(@"</div>");
                strCompnayHTMLMOdal.Append(@"</div>");
                strCompnayHTMLMOdal.Append(@"</div>");
                strCompnayHTMLMOdal.Append(@"</div>");
            }
            Decimal Totalfare = 0;
            Decimal TotalReject = 0;
            Decimal TotalCancel = 0;
            Decimal TotalSale = 0;

            getSale(flightBookingReportItems, out TotalSale, out Totalfare, out TotalCancel, out TotalReject);

            strSale.Append(@"<div class=""col-md-12 col-xs-12 offset-0 hdr"" style=""font-size: 12px;color: #CC0033 !important; text-align: left;"" >Total Sale Information In INR</div>");
            strSale.Append(@"<div class=""col-md-12 col-xs-12  offset-0"" style=""padding-top: 3px; padding-bottom: 3px;"" >");
            strSale.Append(@"<div class=""Flight_SalesDiv col-md-6 col-xs-6 offset-0 tgh"">Total Sale &nbsp;</div><div class=""col-md-6 col-xs-6 offset-0 tgh1"">&nbsp; : " + TotalSale + "</div>");
            strSale.Append(@"</div>");
            strSale.Append(@"<div class=""col-md-12 col-xs-12 offset-0"" style=""padding-top: 3px; padding-bottom: 3px;"" >");
            strSale.Append(@"<div class=""Flight_SalesDiv col-md-6 col-xs-6 offset-0 tgh"">Total Rejected &nbsp;</div><div class=""col-md-6 col-xs-6 offset-0 tgh1"">&nbsp;  : " + TotalReject + "</div>");
            strSale.Append(@"</div>");
            strSale.Append(@"<div class=""col-md-12 col-xs-12 offset-0"" style=""padding-top: 3px; padding-bottom: 3px;"" >");
            strSale.Append(@"<div class=""Flight_SalesDiv col-md-6 col-xs-6 offset-0 tgh"">Total Cancel &nbsp;</div><div class=""col-md-6 col-xs-6 offset-0 tgh1"">&nbsp;  : " + TotalCancel + "</div>");
            strSale.Append(@" </div>");
            strSale.Append(@"<div class=""col-md-12 col-xs-12 offset-0"" style=""padding-top: 3px; padding-bottom: 3px;"" >");
            strSale.Append(@"<div class=""Flight_SalesDiv col-md-6 col-xs-6 offset-0 tgh"">Accurate Sales &nbsp;</div><div class=""col-md-6 col-xs-6 offset-0 tgh1"">&nbsp; : " + Totalfare + "</div>");
            strSale.Append(@"</div>");



            return sbhtml;
        }
        private void getSale(List<AirBookingReport> dtReport, out Decimal TotalSale, out Decimal Totalfare, out Decimal TotalCancel, out Decimal TotalReject)
        {
            Totalfare = 0;
            TotalReject = 0;
            TotalCancel = 0;
            TotalSale = 0;

            try
            {
                var groupedRefs = dtReport.GroupBy(row => row.BookingRef).Select(g => g.Key);
                foreach (var dr in groupedRefs)
                {
                    var reportItem = dtReport.Where(r => r.BookingRef == dr).FirstOrDefault();
                    if (Convert.ToBoolean(reportItem.IsRejected.ToString()).Equals(true))
                    {
                        TotalReject += Convert.ToDecimal(reportItem.TotalFare.ToString());
                    }
                    else if (Convert.ToBoolean(reportItem.IsCancelRequested.ToString()).Equals(true))
                    {
                        TotalSale += Convert.ToDecimal(reportItem.TotalFare.ToString());
                        TotalCancel += Convert.ToDecimal(reportItem.TotalFare.ToString());
                    }
                    else
                    {
                        TotalSale += Convert.ToDecimal(reportItem.TotalFare.ToString());
                        Totalfare += Convert.ToDecimal(reportItem.TotalFare.ToString());
                    }

                }
            }
            catch
            {

            }
        }

        #endregion

        #region #cancel flight bookings
        [HttpGet]
        [Route("report/cancelflightbookings")]
        public async Task<IActionResult> CancelFlightBookings()
        {
            var today = DateTime.Now.Date;

            var model = new CancelFlightBookingsReport
            {
                FromDate = today,
                ToDate = today,
            };

            var viewModel = await GetCancelFlightBookings(model);
            return View(viewModel);
        }
        [HttpPost]
        [Route("report/cancelflightbookings")]
        public async Task<IActionResult> CancelFlightBookings(CancelFlightBookingsReport model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                var viewModel = await GetCancelFlightBookings(model);

                if (viewModel.BookingItems == null || !viewModel.BookingItems.Any())
                {
                    viewModel.ErrorMessage = "No Record Found!";
                }
                return View(viewModel);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "An error occurred while fetching the report: " + ex.Message);

            }

            return View(new CancelFlightBookingReportResponse { BookingItems = new List<CancelAirBooking>() });
        }

        private async Task<CancelFlightBookingReportResponse> GetCancelFlightBookings(CancelFlightBookingsReport model)
        {

            var query = new CancelAirBookingReportsQuery
            {
                SupplierId = model.SupplierID,
                FromDate = model.FromDate,
                ToDate = model.ToDate
            };

            var canelFlightBookings = await _canelFlightBookingQueryHandler.HandleAsync(query);
            var supplierList = await _getAirSupplierQueryHandler.HandleAsync(new GetAirSupplierQuery());
            var suppliers = new List<SelectListItem>();
            foreach (var supplier in supplierList)
            {
                suppliers.Add(new SelectListItem { Text = supplier.Name, Value = supplier.SupplierID });
            }

            var viewModel = new CancelFlightBookingReportResponse
            {
                SupplierID = model.SupplierID,
                FromDate = model.FromDate,
                ToDate = model.ToDate,
                BookingItems = canelFlightBookings,
                Suppliers = suppliers
            };

            return viewModel;
        }

        #endregion

        #region #Airsheet
        [HttpGet]
        [Route("report/airsheet")]
        public async Task<IActionResult> AirSheet()
        {
            var today = DateTime.Now.Date;

            var model = new AirSheet
            {
                FromDate = today,
                ToDate = today,
                ReportType = "AB"
            };

            var viewModel = await GetAirSheetItems(model);

           
            viewModel.ReportType = model.ReportType;

            return View(viewModel);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AirSheet(AirSheet model)
        {
            try
            {
                List<FlightBookingRefrence> bookingReferences;

                if (model.ReportType == "AB")
                {
                    var bookingQuery = new BookingRefrenceQuery
                    {
                        StartDate = model.FromDate,
                        EndDate = model.ToDate
                    };
                    bookingReferences = await _flightReferenceHandler.HandleAsync(bookingQuery);
                }
                else if (model.ReportType == "ACB")
                {
                    var cancelQuery = new CancelAirRefrenceQuery
                    {
                        StartDate = model.FromDate,
                        EndDate = model.ToDate
                    };
                    bookingReferences = await _cancelRefundReportHandler.HandleAsync(cancelQuery);
                }
                else
                {
                    ModelState.AddModelError("", "Invalid report type.");
                    return View(new AirSheetResponse { AirSheetItems = new List<AirSheetReports>() });
                }

                if (bookingReferences == null || !bookingReferences.Any())
                {
                    ModelState.AddModelError("", "No Record Found!");
                }

                var airSheetReports = new List<AirSheetReports>();

                var tasks = bookingReferences.Select(async bookingRef =>
                {
                    var bookingDetailQuery = new AirBookingDetailQuery { BookingRef = bookingRef.BookingRef };

                    var airSheetData = await _airSheetReportHandler.HandleAsync(bookingDetailQuery);

                    if (airSheetData != null)
                    {
                        var reports = await GetAirBookingForExcelSheet(airSheetData, bookingRef.BookingRef);
                        airSheetReports.AddRange(reports);
                    }
                });

                await Task.WhenAll(tasks);

                return View(new AirSheetResponse
                {
                    AirSheetItems = airSheetReports,
                    FromDate = model.FromDate,
                    ToDate = model.ToDate,
                    ReportType = model.ReportType 
                });
            }
            catch (Exception ex)
            {
                return View(new AirSheetResponse { ErrorMessage = "An error occurred while generating the report." });
            }
        }

        [HttpPost]
        [Route("report/airsheet/export/excel")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AirSheetExportToExcel(AirSheet model)
        {
            try
            {
                if (model == null || model.FromDate == null || model.ToDate == null)
                {
                    return BadRequest("Invalid parameters for exporting airsheet reports.");
                }

                List<FlightBookingRefrence> bookingReferences;

                if (model.ReportType == "AB")
                {
                    var bookingQuery = new BookingRefrenceQuery
                    {
                        StartDate = model.FromDate,
                        EndDate = model.ToDate
                    };
                    bookingReferences = await _flightReferenceHandler.HandleAsync(bookingQuery);
                }
                else if (model.ReportType == "ACB")
                {
                    var cancelQuery = new CancelAirRefrenceQuery
                    {
                        StartDate = model.FromDate,
                        EndDate = model.ToDate
                    };
                    bookingReferences = await _cancelRefundReportHandler.HandleAsync(cancelQuery);
                }
                else
                {
                    return BadRequest("Invalid report type.");
                }

                if (bookingReferences == null || !bookingReferences.Any())
                {
                    return Ok(new
                    {
                        Success = false,
                        Message = "No Record Found!",
                        FromDate = model.FromDate,
                        ToDate = model.ToDate
                    });
                }

                var airSheetReports = new List<AirSheetReports>();

                var tasks = bookingReferences.Select(async bookingRef =>
                {
                    var bookingDetailQuery = new AirBookingDetailQuery { BookingRef = bookingRef.BookingRef };
                    var airSheetData = await _airSheetReportHandler.HandleAsync(bookingDetailQuery);

                    if (airSheetData != null)
                    {
                        var reports = await GetAirBookingForExcelSheet(airSheetData, bookingRef.BookingRef);
                        airSheetReports.AddRange(reports);
                    }
                });

                await Task.WhenAll(tasks);

                if (!airSheetReports.Any())
                {
                    return BadRequest("No reports available to export.");
                }

                var airSheetReportData = airSheetReports.Select((report, index) => new
                {
                    SNo = index + 1,
                    report.PaxNo,
                    report.StaffName,
                    report.CreatedOn,
                    report.BookingRef,
                    report.CarrierCode,
                    report.FlightNo,
                    report.AccountNo,
                    report.Company,
                    report.RecordLocator,
                    report.TicketNo,
                    report.Adt,
                    report.Chd,
                    report.Inf,
                    report.PassengerType,
                    report.Passenger,
                    report.Sector,
                    report.DOJ,
                    report.BasicFare,
                    report.YQ,
                    report.TxnFee,
                    report.Taxes,
                    report.Meal,
                    report.Baggage,
                    report.ServiceCharge,
                    report.TDS,
                    report.Commission,
                    report.NetPaidByAgent,
                    report.Supplier
                }).ToList();

                var headers = new List<string>
        {
            "SNo",
            "PaxNo",
            "Staff",
            "CreatedOn",
            "BookingRef",
            "CarrierCode",
            "FlightNo",
            "AccountNo",
            "Company",
            "RecordLocator",
            "TicketNo",
            "Adt",
            "Chd",
            "Inf",
            "PassengerType",
            "Passenger",
            "Sector",
            "DOJ",
            "BasicFare",
            "YQ",
            "TxnFee",
            "Taxes",
            "Meal",
            "Baggage",
            "ServiceCharge",
            "TDS",
            "Commission",
            "Net_Paid_By_Agent",
            "Supplier"
        };

                var filePath = Path.Combine(Path.GetTempPath(), $"AirSheetReports_{DateTime.Now:yyyyMMddHHmmss}.xlsx");
                var excelGenerator = new GenerateExcel();
                excelGenerator.ExportToExcel(airSheetReportData, filePath, headers, "BillingReport");

                var fileBytes = System.IO.File.ReadAllBytes(filePath);
                System.IO.File.Delete(filePath);

                return File(fileBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "BillingReport.xlsx");
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while generating the report.");
            }
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AirSheetExportToPDF(AirSheet model)
        {
            try
            {
                if (model == null || model.FromDate == null || model.ToDate == null)
                {
                    return BadRequest("Invalid parameters for exporting airsheet reports.");
                }

                List<FlightBookingRefrence> bookingReferences;

                if (model.ReportType == "AB")
                {
                    var bookingQuery = new BookingRefrenceQuery
                    {
                        StartDate = model.FromDate,
                        EndDate = model.ToDate
                    };
                    bookingReferences = await _flightReferenceHandler.HandleAsync(bookingQuery);
                }
                else if (model.ReportType == "ACB")
                {
                    var cancelQuery = new CancelAirRefrenceQuery
                    {
                        StartDate = model.FromDate,
                        EndDate = model.ToDate
                    };
                    bookingReferences = await _cancelRefundReportHandler.HandleAsync(cancelQuery);
                }
                else
                {
                    return BadRequest("Invalid report type.");
                }

                if (bookingReferences == null || !bookingReferences.Any())
                {
                    return Ok(new
                    {
                        Success = false,
                        Message = "No Record Found!",
                        FromDate = model.FromDate,
                        ToDate = model.ToDate
                    });
                }

                var airSheetReports = new List<AirSheetReports>();

                var tasks = bookingReferences.Select(async bookingRef =>
                {
                    var bookingDetailQuery = new AirBookingDetailQuery { BookingRef = bookingRef.BookingRef };
                    var airSheetData = await _airSheetReportHandler.HandleAsync(bookingDetailQuery);

                    if (airSheetData != null)
                    {
                        var reports = await GetAirBookingForExcelSheet(airSheetData, bookingRef.BookingRef);
                        airSheetReports.AddRange(reports);
                    }
                });

                await Task.WhenAll(tasks);

                if (!airSheetReports.Any())
                {
                    return BadRequest("No reports available to export.");
                }

                var airSheetReportData = airSheetReports.Select((report, index) => new
                {
                    SNo = index + 1,
                    report.PaxNo,
                    report.StaffName,
                    report.CreatedOn,
                    report.BookingRef,
                    report.CarrierCode,
                    report.FlightNo,
                    report.AccountNo,
                    report.Company,
                    report.RecordLocator,
                    report.TicketNo,
                    report.Adt,
                    report.Chd,
                    report.Inf,
                    report.PassengerType,
                    report.Passenger,
                    report.Sector,
                    report.DOJ,
                    report.BasicFare,
                    report.YQ,
                    report.TxnFee,
                    report.Taxes,
                    report.Meal,
                    report.Baggage,
                    report.ServiceCharge,
                    report.TDS,
                    report.Commission,
                    report.NetPaidByAgent,
                    report.Supplier
                }).ToList();

                // Define headers for the PDF
                var headers = new List<string>
        {
            "SNo",
            "PaxNo",
            "Staff",
            "CreatedOn",
            "BookingRef",
            "CarrierCode",
            "FlightNo",
            "AccountNo",
            "Company",
            "RecordLocator",
            "TicketNo",
            "Adt",
            "Chd",
            "Inf",
            "PassengerType",
            "Passenger",
            "Sector",
            "DOJ",
            "BasicFare",
            "YQ",
            "TxnFee",
            "Taxes",
            "Meal",
            "Baggage",
            "ServiceCharge",
            "TDS",
            "Commission",
            "Net_Paid_By_Agent",
            "Supplier"
        };

                // Generate PDF
                var pdfFilePath = Path.Combine(Path.GetTempPath(), $"AirSheetReports_{DateTime.Now:yyyyMMddHHmmss}.pdf");
                var pdfGenerator = new GeneratePDF();
                pdfGenerator.ExportToPDF(pdfFilePath, airSheetReportData, headers,"A2");

                // Read the PDF file and return it
                var fileBytes = System.IO.File.ReadAllBytes(pdfFilePath);
                System.IO.File.Delete(pdfFilePath);

                return File(fileBytes, "application/pdf", "Report_list.pdf");
            }
            catch (Exception ex)
            {
                // Log the exception (ex) here if needed
                return StatusCode(500, "An error occurred while generating the report.");
            }
        }




        public async Task<List<AirSheetReports>> GetAirBookingForExcelSheet(AirSheetReportData dsBooking, int BookingRef)
        {
            List<AirSheetReports> airSheetReportList = new();

            try
            {
                dsBooking = await _airSheetReportHandler.HandleAsync(new AirBookingDetailQuery { BookingRef = BookingRef });

                if (dsBooking != null)
                {
                    var dtFlight_Detail = dsBooking.CompanyFlightDetailAirlines;
                    var dtFlight_Segment_Detail = dsBooking.CompanyFlightSegmentDetailAirlines;
                    var dtFare_Detail = dsBooking.CompanyFareDetailAirlines;
                    var dtFare_Detail_Segment = dsBooking.CompanyFareDetailSegmentAirlines;
                    var dtPax_Detail = dsBooking.CompanyPaxDetailAirlines;
                    var dtPax_Segment_Detail = dsBooking.CompanyPaxSegmentDetailAirlines;
                    var dtCompanyDetail = dsBooking.CompanyFlightDetails;

                    string AccountNo = dtCompanyDetail.FirstOrDefault()?.AccountId.ToString() ?? string.Empty;
                    string Company = dtCompanyDetail.FirstOrDefault()?.CompanyName ?? string.Empty;
                    string CompanyID = dtCompanyDetail.FirstOrDefault()?.CompanyId ?? string.Empty;

                    if (CompanyID.Contains("-SA-") || CompanyID.Contains("-C-"))
                    {
                        CompanyID = getCompany_by_SubCompany_Customer(CompanyID);
                        AccountNo = CompanyID.Replace("A-", "").Trim();
                        Company = dtCompanyDetail.FirstOrDefault()?.A_CompanyName ?? string.Empty;
                    }

                    int Rowid = 0;

                    foreach (var dr in dtPax_Detail)
                    {
                        Rowid++;
                        var report = CreateAirSheetReport(dr, dtFlight_Detail, dtFlight_Segment_Detail, dtFare_Detail, dtFare_Detail_Segment, dtPax_Segment_Detail, BookingRef, Rowid, AccountNo, Company, "O");
                        airSheetReportList.Add(report);
                    }

                    if (dtFlight_Detail.FirstOrDefault()?.Trip == "R")
                    {
                        foreach (var dr in dtPax_Detail)
                        {
                            Rowid++;
                            var report = CreateAirSheetReport(dr, dtFlight_Detail, dtFlight_Segment_Detail, dtFare_Detail, dtFare_Detail_Segment, dtPax_Segment_Detail, BookingRef, Rowid, AccountNo, Company, "I");
                            airSheetReportList.Add(report);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }

            return airSheetReportList;
        }


        private AirSheetReports CreateAirSheetReport(
     CompanyPaxDetailAirlinesData dr,
     List<CompanyFlightDetailAirlinesData> dtFlight_Detail,
     List<CompanyFlightSegmentDetailAirlinesData> dtFlight_Segment_Detail,
     List<CompanyFareDetailAirlinesData> dtFare_Detail,
     List<CompanyFareDetailSegmentAirlinesData> dtFare_Detail_Segment,
     List<CompanyPaxSegmentDetailAirlinesData> dtPax_Segment_Detail,
     int BookingRef,
     int Rowid,
     string AccountNo,
     string Company,
     string direction)
        {
            var report = new AirSheetReports
            {
                PaxNo = Rowid,
                PassengerType = dr.PaxType,
                Passenger = $"{dr.First_Name} {dr.Middle_Name} {dr.Last_Name}".Trim(),
                TicketNo = dr.TicketNo,
                AccountNo = AccountNo,
                Company = Company,
                Meal = SSRdetail(BookingRef, "M", dr.Pax_SegmentId.ToString(), dtPax_Segment_Detail, direction),
                Baggage = SSRdetail(BookingRef, "B", dr.Pax_SegmentId.ToString(), dtPax_Segment_Detail, direction)
            };

            // Convert Flight Details to match required type
            var flightDetails = dtFlight_Detail.Select(f => new CompanyFlightDetailAirlinesData
            {
                StaffName = f.StaffName,
                Staff = f.Staff,
                SupplierId_A = f.SupplierId_A,
                SupplierId_D = f.SupplierID_D,
                EventTime = f.EventTime,
                BookingRef = f.BookingRef,
                CarrierCode_A = f.CarrierCode_A,
                CarrierCode_D = f.CarrierCode_D,
                Airline_Pnr_A = f.Airline_Pnr_A,
                Airline_Pnr_D = f.Airline_Pnr_D,
                Adt = f.Adt,
                Chd = f.Chd,
                Inf = f.Inf,
                DepartureDate_A = f.DepartureDate_A,
                DepartureDate_D = f.DepartureDate_D
            }).ToList();

            var fareDetails = dtFare_Detail.Select(f => new CompanyFareDetailAirlinesData
            {
                PaxType = f.PaxType,
                Basic = f.Basic,
                Yq = f.Yq,
                Tf = f.Tf,
                Psf = f.Psf,
                Udf = f.Udf,
                Audf = f.Audf,
                Cute = f.Cute,
                Gst = f.Gst,
                Cess = f.Cess,
                Ex = f.Ex,
                ServiceTax = f.ServiceTax,
                ServiceFee = f.ServiceFee,
                Tds = f.Tds,
                BasicDeal = f.BasicDeal,
                YqDeal = f.YqDeal,
                CbDeal = f.CbDeal,
                PromoDeal = f.PromoDeal
            }).ToList();

            var fareSegmentDetails = dtFare_Detail_Segment.Select(f => new CompanyFareDetailSegmentAirline
            {
                PaxType = f.PaxType,
                Conn = f.Conn,
                Basic = f.Basic,
                Yq = f.Yq,
                Tf = f.Tf,
                Psf = f.Psf,
                Udf = f.Udf,
                Audf = f.Audf,
                Cute = f.Cute,
                Gst = f.Gst,
                Cess = f.Cess,
                Ex = f.Ex,
                ServiceTax = f.ServiceTax,
                ServiceFee = f.Service_Fee,
                Tds = f.Tds,
                BasicDeal = f.Basic_Deal,
                YqDeal = f.Yq_Deal,
                CbDeal = f.Cb_Deal,
                PromoDeal = f.Promo_Deal
            }).ToList();

            var flightSegmentDetails = dtFlight_Segment_Detail.Select(f => new CompanyFlightSegmentDetailAirline
            {
                FlightNumber = f.FlightNumber,
                DepartureStation = f.DepartureStation,
                ArrivalStation = f.ArrivalStation,
                ConnOrder = f.ConnOrder
            }).ToList();

            setFlightDetail(flightDetails, flightSegmentDetails, report, direction);

            setFareDetail(fareDetails, fareSegmentDetails, report, direction, dr.PaxType);

            // Adjust Net Paid Amount
            if (report.Meal > 0)
            {
                report.NetPaidByAgent += report.Meal;
            }
            if (report.Baggage > 0)
            {
                report.NetPaidByAgent += report.Baggage;
            }

            return report;
        }


        private int SSRdetail(int bookingRef, string ssrType, string paxSegmentId, List<CompanyPaxSegmentDetailAirlinesData> fareDetailSegments, string conn)
        {
            int ssrAmount = 0;

            try
            {
                if (fareDetailSegments != null && fareDetailSegments.Any())
                {
                    var selectedSegment = fareDetailSegments
                        .FirstOrDefault(segment => segment.Pax_SegmentId.ToString() == paxSegmentId && segment.Conn == conn && segment.ChargeType == ssrType);

                    if (selectedSegment != null)
                    {
                        ssrAmount = int.TryParse(selectedSegment.Charge_Amount.ToString(), out int amount) ? amount : 0;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in SSRdetail: {ex.Message}");
            }

            return ssrAmount;
        }

        private void setFareDetail(
      List<CompanyFareDetailAirlinesData> dtFare_Detail,
      List<CompanyFareDetailSegmentAirline> fareDetailSegments,
      AirSheetReports drAdd,
      string fltType,
      string paxType)
        {
            try
            {
                var fareSegment = fareDetailSegments.FirstOrDefault(f => f.PaxType == paxType && f.Conn == fltType);

                if (fareSegment != null)
                {
                    drAdd.BasicFare = fareSegment.Basic ?? 0;
                    drAdd.YQ = fareSegment.Yq ?? 0;
                    drAdd.TxnFee = fareSegment.Tf ?? 0;

                    drAdd.Taxes = new[]
                    {
              fareSegment.Psf, fareSegment.Udf, fareSegment.Audf, fareSegment.Cute,
              fareSegment.Gst, fareSegment.Cess, fareSegment.Ex
          }.Sum(x => x ?? 0);

                    drAdd.ServiceCharge = (fareSegment.ServiceTax ?? 0) + (fareSegment.ServiceFee ?? 0);
                    drAdd.TDS = fareSegment.Tds ?? 0;

                    drAdd.Commission = new[]
                    {
              fareSegment.BasicDeal, fareSegment.YqDeal, fareSegment.CbDeal, fareSegment.PromoDeal
          }.Sum(x => x ?? 0);

                    decimal totalPaidByAgent = new[]
                    {
              fareSegment.Basic, fareSegment.Yq, fareSegment.Tf, fareSegment.Psf, fareSegment.Udf,
              fareSegment.Audf, fareSegment.Cute, fareSegment.Gst, fareSegment.Cess, fareSegment.Ex,
              fareSegment.Tds, fareSegment.ServiceTax, fareSegment.ServiceFee
          }.Sum(x => x ?? 0) - drAdd.Commission;

                    drAdd.NetPaidByAgent = totalPaidByAgent;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in SetFareDetail: {ex.Message}");
            }
        }

        private void setFlightDetail(List<CompanyFlightDetailAirlinesData> flightDetails, List<CompanyFlightSegmentDetailAirline> flightSegmentDetails, AirSheetReports drAdd, string fltType)
        {
            try
            {
                var flightDetail = flightDetails.FirstOrDefault();
                if (flightDetail == null) return;

                if (fltType == "O")
                {
                    drAdd.Staff = flightDetail.Staff;
                    drAdd.StaffName = flightDetail.StaffName;
                    drAdd.Supplier = flightDetail.SupplierId_D;
                    drAdd.CreatedOn = Convert.ToDateTime(flightDetail.EventTime).ToString("yyyy-MM-dd");
                    drAdd.BookingRef = Convert.ToInt32(flightDetail.BookingRef);
                    drAdd.CarrierCode = flightDetail.CarrierCode_D;
                    drAdd.RecordLocator = flightDetail.Airline_Pnr_D;
                }
                else
                {
                    drAdd.Staff = flightDetail.Staff;
                    drAdd.StaffName = flightDetail.StaffName;
                    drAdd.Supplier = flightDetail.SupplierId_A;
                    drAdd.CreatedOn = Convert.ToDateTime(flightDetail.EventTime).ToString("yyyy-MM-dd");
                    drAdd.BookingRef = Convert.ToInt32(flightDetail.BookingRef);
                    drAdd.CarrierCode = flightDetail.CarrierCode_A;
                    drAdd.RecordLocator = flightDetail.Airline_Pnr_A;
                }

                drAdd.Adt = Convert.ToInt32(flightDetail.Adt);
                drAdd.Chd = Convert.ToInt32(flightDetail.Chd);
                drAdd.Inf = Convert.ToInt32(flightDetail.Inf);
                drAdd.DOJ = fltType == "O" ? flightDetail.DepartureDate_D : flightDetail.DepartureDate_A;

                var flightSegments = flightSegmentDetails
                    .Where(f => f.ConnOrder.StartsWith(fltType == "O" ? "O" : "I"))
                    .ToList();

                if (flightSegments.Any())
                {
                    drAdd.FlightNo = flightSegments.First().FlightNumber;

                    string sector = string.Join("/", flightSegments.Select(f => $"{f.DepartureStation}/{f.ArrivalStation}"));
                    drAdd.Sector = sector;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in SetFlightDetail: {ex.Message}");
            }
        }

        public static string getCompany_by_SubCompany_Customer(string CompanyID)
        {
            if (CompanyID.IndexOf("-SA-") != -1)
            {
                CompanyID = CompanyID.Substring(0, CompanyID.IndexOf("-SA"));
            }
            if (CompanyID.IndexOf("-C-") != -1)
            {
                CompanyID = CompanyID.Substring(0, CompanyID.IndexOf("-C"));
            }
            return CompanyID;
        }
        private async Task<AirSheetResponse> GetAirSheetItems(AirSheet model)
        {
            var bookingQuery = new BookingRefrenceQuery
            {
                StartDate = model.FromDate,
                EndDate = model.ToDate
            };

            var bookingReferences = await _flightReferenceHandler.HandleAsync(bookingQuery);

            if (bookingReferences == null || !bookingReferences.Any())
            {
                return new AirSheetResponse
                {
                    ErrorMessage = "No Record Found!",
                    FromDate = model.FromDate,
                    ToDate = model.ToDate
                };
            }


            var airSheetItems = new List<AirSheetReports>();

            foreach (var bookingRef in bookingReferences)
            {
                var bookingDetailQuery = new AirBookingDetailQuery { BookingRef = bookingRef.BookingRef };
                var airSheetData = await _airSheetReportHandler.HandleAsync(bookingDetailQuery);

                if (airSheetData != null)
                {
                    var flightDetails = airSheetData.CompanyFlightDetailAirlines;
                    var flightSegDetails = airSheetData.CompanyFlightSegmentDetailAirlines;
                    var fareDetails = airSheetData.CompanyFareDetailAirlines?.FirstOrDefault();
                    var fareDetailSeg = airSheetData.CompanyFareDetailSegmentAirlines;
                    var paxDetails = airSheetData.CompanyPaxDetailAirlines;
                    var compDetails = airSheetData.CompanyFlightDetails?.FirstOrDefault();
                    var paxSegmentDetails = airSheetData.CompanyPaxSegmentDetailAirlines;

                    int rowId = 0;
                    foreach (var pax in paxDetails)
                    {
                        rowId++;
                        var item = new AirSheetReports
                        {
                            BookingRef = bookingRef.BookingRef,
                            PaxNo = rowId,
                            PassengerType = pax.PaxType,
                            Passenger = $"{pax.First_Name} {pax.Middle_Name} {pax.Last_Name}",
                            TicketNo = pax.TicketNo,
                            AccountID = compDetails?.AccountId ?? 0,
                            Company = compDetails?.CompanyName,
                        };


                        if (item.Meal > 0)
                        {
                            item.NetPaidByAgent += item.Meal;
                        }

                        if (item.Baggage > 0)
                        {
                            item.NetPaidByAgent += item.Baggage;
                        }


                        airSheetItems.Add(item);
                    }

                    if (flightDetails.FirstOrDefault()?.Trip == "R")
                    {
                        foreach (var pax in paxDetails)
                        {
                            rowId++;
                            var item = new AirSheetReports
                            {
                                BookingRef = bookingRef.BookingRef,
                                PaxNo = rowId,
                                PassengerType = pax.PaxType,
                                Passenger = $"{pax.First_Name} {pax.Middle_Name} {pax.Last_Name}",
                                TicketNo = pax.TicketNo,
                                AccountID = compDetails?.AccountId ?? 0,
                                Company = compDetails?.CompanyName,
                            };
                            if (item.Meal > 0)
                            {
                                item.NetPaidByAgent += item.Meal;
                            }

                            if (item.Baggage > 0)
                            {
                                item.NetPaidByAgent += item.Baggage;
                            }

                            airSheetItems.Add(item);
                        }
                    }
                }
            }

            return new AirSheetResponse
            {
                FromDate = model.FromDate,
                ToDate = model.ToDate,
                //AirSheetItems = airSheetItems,
                BillingType = model.BillingType
            };
        }


        #endregion
    }
}
