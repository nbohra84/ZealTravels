using Microsoft.AspNetCore.Mvc;
using ZealTravel.Application.AgencyManagement.Command;
using ZealTravel.Application.AgencyManagement.Queries;
using ZealTravel.Application.AgencyManagement.Handlers;
using ZealTravel.Domain.Interfaces.Handlers;
using ZealTravel.Front.Web.Models;
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
using ZealTravel.Application.ReportManagement.Queries;
using log4net.Core;
using ZealTravel.Application.BookingManagement.Handler;
using ZealTravel.Application.BookingManagement.Query;
using System.Xml;
using ZealTravel.Application.BankManagement.Command;
using DocumentFormat.OpenXml.Office.Word;
using DocumentFormat.OpenXml.Bibliography;
using ZealTravel.Domain.Interfaces.AirlineManagement;
using svc_booking;
using ZealTravel.Application.UserManagement.Handlers;
using ZealTravel.Application.AirlineManagement.Command;

namespace ZealTravel.Front.Web.Controllers
{
    [Authorize]
    public class AgencyController : AgencyBaseController
    {
        private readonly IHandlesQueryAsync<DailyBookingReportsQuery, List<DailyBooking>> _dailyBookingQueryHandler;
        private readonly IHandlesQueryAsync<GetProfileQuery, CompanyProfileData> _getCompanyProfileQueryHandler;
        private readonly IHandlesCommandAsync<UpdateLogoCommand> _updateLogoCommandHandler;
        private readonly IHandlesQueryAsync<GetUserDetailsQuery, CompanyRegister> _getUserDetailsQueryHandler;
        private readonly IHandlesQueryAsync<LedgerReportQuery, List<LedgerReports>> _ledgerReportQueryHandler;
        private readonly IHandlesQueryAsync<AirBookingQuery, List<AirBookingReport>> _flightBookingReportQueryHandler;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IMapper _mapper;
        private readonly ILogger<AgencyController> _logger;
        private readonly IHandlesQueryAsync<BookingDetailQuery, BookingDetailData> _bookingTicketQueryHandler;
        private readonly IHandlesCommandAsync<SetPaymentForHoldBookingCommand> _setPaymentHoldTicketCommandHandler;
        private readonly ICompanyFlightDetailAirlinesService _companyFlightDetailAirlinesService;
        private readonly IHandlesQueryAsync<CompanyIdByAccountIdQuery, string> _getCompanyIDByAccountIDQueryHandler;
        private readonly IConfiguration _configuration;

        public AgencyController(
            IHandlesQueryAsync<DailyBookingReportsQuery, List<DailyBooking>> dailyBookingQueryHandler, 
            IHandlesQueryAsync<GetProfileQuery, CompanyProfileData> getCompanyProfileQueryHandler, 
            IHandlesQueryAsync<GetAvailableBalanceQuery, decimal> getAvailableBalanceQueryHandler, 
            IHandlesCommandAsync<UpdateLogoCommand> updateLogoCommandHandler, 
            IHandlesQueryAsync<GetUserDetailsQuery, CompanyRegister> getUserDetailsQueryHandler, 
            IWebHostEnvironment webHostEnvironment, 
            IHandlesQueryAsync<LedgerReportQuery, List<LedgerReports>> ledgerReportQueryHandler, 
            IHandlesQueryAsync<AirBookingQuery, List<AirBookingReport>> flightBookingQueryHandler, 
            IMapper mapper,
             ILogger<AgencyController> logger,
             IHandlesQueryAsync<BookingDetailQuery, BookingDetailData> bookingTicketQueryHandler,
             IHandlesCommandAsync<SetPaymentForHoldBookingCommand> setPaymentHoldTicketCommandHandler,
             ICompanyFlightDetailAirlinesService companyFlightDetailAirlinesService,
             IHandlesQueryAsync<CompanyIdByAccountIdQuery, string> getCompanyIDByAccountIDQueryHandler,
            IConfiguration configuration) : base(getAvailableBalanceQueryHandler)
        {
            _dailyBookingQueryHandler = dailyBookingQueryHandler;
            _getCompanyProfileQueryHandler = getCompanyProfileQueryHandler;
            _updateLogoCommandHandler = updateLogoCommandHandler;
            _getUserDetailsQueryHandler = getUserDetailsQueryHandler;
            _ledgerReportQueryHandler = ledgerReportQueryHandler;
            _flightBookingReportQueryHandler = flightBookingQueryHandler;
            _webHostEnvironment = webHostEnvironment;
            _mapper = mapper;
            _logger = logger;
            _bookingTicketQueryHandler = bookingTicketQueryHandler;
            _setPaymentHoldTicketCommandHandler = setPaymentHoldTicketCommandHandler;
            _companyFlightDetailAirlinesService = companyFlightDetailAirlinesService;
            _getCompanyIDByAccountIDQueryHandler = getCompanyIDByAccountIDQueryHandler;
            _configuration = configuration;
        }
        [HttpGet]
        [Route("agency/report/dailybookings")]
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
        [Route("agency/report/dailybookings")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DailyBookingReports(DailyBookingReports model)
        {
            var companyId = User.FindFirstValue("CompanyId");

            if (string.IsNullOrEmpty(companyId))
            {
                ModelState.AddModelError("", "Company ID not found.");
                return View(new DailyBookingReportsListResponse { BookingReports = new List<DailyBookingReportsResponse>() });
            }

            model.CompanyID = companyId;

            if (!ModelState.IsValid)
            {
                return View(model);
            }


            try
            {
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
                BookingReports = webReports
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

        //Ledger methods
        [HttpGet]
        [Route("agency/report/ledger")]
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
        [Route("agency/report/ledger")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Ledger(LedgerReport model)
        {
            var companyID = UserHelper.GetCompanyID(User);
            var viewModel = new LedgerReportListResponse();

            if (string.IsNullOrEmpty(companyID))
            {
                ModelState.AddModelError("", "Company ID not found.");
                return View(viewModel);
            }

            model.CompanyID = companyID;

            if (ModelState.IsValid)
            {
                try
                {
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
                        ReportTableCompanyPopup = companyHTMLModal.ToString()


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
            }

            return View(viewModel);
        }

        private async Task<List<LedgerReports>> GetLedgerData(LedgerReport model)
        {

            var txtPNR = !string.IsNullOrEmpty(model.PNR) ? model.PNR.Trim() : string.Empty;
            var Ticketno = !string.IsNullOrEmpty(model.TicketNumber) ? model.TicketNumber : string.Empty;
            var PaxName = !string.IsNullOrEmpty(model.PassengerName) ? model.PassengerName : string.Empty;
            var bookingref = !string.IsNullOrEmpty(model.BookingReference) ? model.BookingReference.Trim() : string.Empty;
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
                EventID = model.EventID
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
        [Route("Booking/PrintPayment4HoldBookPopup")]
        public async Task<IActionResult> PrintPayment4HoldBookPopup([FromBody] string BookingRef)
        {

            if (!int.TryParse(BookingRef, out var iBookingRef) || iBookingRef <= 0)
                return BadRequest("Invalid booking reference.");


            var query = new BookingDetailQuery { BookingRef = iBookingRef };
            var flightbooking = await _bookingTicketQueryHandler.HandleAsync(query);

            if (flightbooking == null)
                return NotFound("Booking not found.");

            // serialize the DTO to XML
            string alldata;
            try
            {

                alldata = XMLHelper.ConvertObjectToXml(flightbooking);
            }
            catch (InvalidOperationException)
            {

                return NotFound("Booking not found.");
            }


            var xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(alldata);
            var root = xmlDoc.DocumentElement;
            if (root == null || !root.HasChildNodes)
                return NotFound("Booking not found.");

            
            return Content(alldata, "application/xml", Encoding.UTF8);
        }

        [HttpPost]
        [Route("Booking/SendPayment4HoldBookRequest")]
        public async Task<IActionResult> SendPayment4HoldBookRequest([FromBody] HoldPaymentRequest req)
        {

            if (!int.TryParse(req.BookingRef, out var iBookingRef) || iBookingRef <= 0)
                return BadRequest("Invalid booking reference.");

            var searchId = await ResolveSearchIdAsync();
            try
            {
                var sendHoldPayment = new SetPaymentForHoldBookingCommand
                {
                    BookingRef = req.BookingRef,
                    Remarks = req.Remarks,
                    PaymentType = req.PaymentType,
                    SearchId = searchId
                };
                await _setPaymentHoldTicketCommandHandler.HandleAsync(sendHoldPayment);
                
                
                // no exception = success
                return Ok(new { message = "Payment has been successfully processed for this ticket." });



            }
            catch (Exception ex)
            {
                // any exception maps to 400 Bad Request with the exception message
                _logger.LogError($"Error in PaymentForHoldBooking action: {ex.Message}");
                return BadRequest(new { error = ex.Message });
            }
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

        [HttpGet]
        [Route("agency/report/flightbookings")]
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
            var eligibilityMap = await GetPaymentEligibilityAsync(flightBookingReports);

            StringBuilder strCompnayHTMLMOdal = new StringBuilder();

            StringBuilder reportHTML = new StringBuilder();
            if (flightBookingReports.Any())
            {
                reportHTML = GetFlightBookingReportHTML(flightBookingReports, false, out StringBuilder strHTMMOdal, out StringBuilder strSale, UserHelper.GetCompanyID(User), eligibilityMap);
                strCompnayHTMLMOdal = strHTMMOdal;
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
                ReportTableCompanyPopup = strCompnayHTMLMOdal.ToString()

            };
            //return viewModel;
            if (string.IsNullOrEmpty(viewModel.ReportTableHTML))
            {
                viewModel.ReportTableHTML = string.Empty;
            }

            return View(viewModel);
        }

        [HttpPost]
        [Route("agency/report/flightbookings")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> FlightBookings(FlightBookingReports model)
        {
            var companyID = UserHelper.GetCompanyID(User);
            var viewModel = new FlightBookingListResponse();

            if (string.IsNullOrEmpty(companyID))
            {
                ModelState.AddModelError("", "Company ID not found.");
                return View(viewModel);
            }

            model.CompanyID = companyID;

            if (ModelState.IsValid)
            {
                try
                {
                    var flightBookingReports = await GetFlightBookingData(model);

                    var eligibilityMap = await GetPaymentEligibilityAsync(flightBookingReports);

                    StringBuilder strCompnayHTMLMOdal = new StringBuilder();

                    StringBuilder reportHTML = new StringBuilder();
                    if (flightBookingReports.Any())
                    {

                        reportHTML = GetFlightBookingReportHTML(flightBookingReports, false, out StringBuilder strHTMMOdal, out StringBuilder strSale, UserHelper.GetCompanyID(User), eligibilityMap);
                        strCompnayHTMLMOdal = strHTMMOdal;
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
                        ReportTableCompanyPopup = strCompnayHTMLMOdal.ToString()


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

        /// <summary>
        /// For each AirBookingReport in the list, checks carrier‐specific locator/PNR rules
        /// and returns a map of BookingRef → whether “Pay” should be shown (true/false).
        /// </summary>
        private async Task<Dictionary<int, bool>> GetPaymentEligibilityAsync(List<AirBookingReport> airBookingReports)
        {
            var result = new Dictionary<int, bool>();

            foreach (var report in airBookingReports)
            {
                bool ok = true;

                // Load the persisted booking record
                var booking = await _companyFlightDetailAirlinesService
                                     .FindAsync(x => x.BookingRef == report.BookingRef)
                                     .ConfigureAwait(false);

                if (booking == null)
                {
                    ok = false;
                }
                else
                {
                    // Determine if it’s a round trip
                    bool isRoundTrip = booking.Trip == "R" && booking.Sector == "D";

                    // Pull codes & carriers
                    string depCode = booking.UniversalLocatorCodeD;
                    string arrCode = booking.UniversalLocatorCodeA;
                    string airPNRD = booking.AirlinePnrD;
                    string airPNRA = booking.AirlinePnrA;
                    string carrierD = booking.CarrierCodeD;
                    string carrierA = booking.CarrierCodeA;

                    // Combined rule for 6E, AI or SG 
                    if ((carrierD == "6E" || carrierD == "AI" || carrierD == "SG")
                     || (carrierA == "6E" || carrierA == "AI" || carrierA == "SG"))
                    {
                        // Departure check:
                        bool missingDep = carrierD == "SG" ? string.IsNullOrWhiteSpace(airPNRD) : string.IsNullOrWhiteSpace(depCode);

                        // Arrival check (only when round-trip):
                        bool missingArr = carrierA == "SG" ? isRoundTrip && string.IsNullOrWhiteSpace(airPNRA) : isRoundTrip && string.IsNullOrWhiteSpace(arrCode);

                        if (missingDep || missingArr)
                        {
                            ok = false;
                        }
                    }
                }

                result[report.BookingRef] = ok;
            }

            return result;
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



        public StringBuilder GetFlightBookingReportHTML(List<AirBookingReport> flightBookingReportItems, bool FlagforCompanyname, out StringBuilder strCompnayHTMLMOdal, out StringBuilder strSale, string SessionCompany, Dictionary<int, bool> eligibilityMap)
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
           IsUpdated = g.FirstOrDefault()?.IsUpdated ?? false,
           IsRejected = g.FirstOrDefault()?.IsRejected ?? false,
           IsPaymentHold_D = g.FirstOrDefault()?.IsPaymentHold_D ?? false,
           IsPaymentHold_A = g.FirstOrDefault()?.IsPaymentHold_A ?? false,
           IsPaymentHold = g.FirstOrDefault()?.IsPaymentHold ?? string.Empty,



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
                    sbhtml.Append(@"<td style='width:300px; font-size: 12px;   border-bottom: 1px solid #ccc; '>Ref.  " + reportItem.BookingRef.ToString() + "  |  " + reportItem.UpdatedBy.ToString().Trim() + "</td>");
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
                sbhtml.Append(@"<a href='#' class='not-active'>Service Request</a><a href='#' class='not-active'> Service </a>");


                if ((SessionCompany.IndexOf("AD") != -1 || SessionCompany.IndexOf("ST-") != -1) || (SessionCompany.IndexOf("A-") != -1 && SessionCompany.IndexOf("-SA-") == -1 && SessionCompany.IndexOf("C-") == -1))
                {
                    if (Isupdated && !Isrejected)
                    {
                        sbhtml.Append(@"<a  id='invoice_" + i + "' accesskey='" + reportItem.BookingRef.ToString().Trim() + "'  style='cursor: pointer;' onclick='printinvoice(this);'> <i class='fa fa-file' title='Invoice' style='font-size: 17px; color: #ff0b0b;' ></i>  </a>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;");
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

                if (reportItem.IsUpdated && Convert.ToBoolean(reportItem.IsPaymentHold.ToString().Trim()) == true && eligibilityMap.TryGetValue(reportItem.BookingRef, out var canPay)&& canPay)
                {
                    sbhtml.Append(@"<a  class='btn btn-default paybtn' id='PayT_" + i + "' accesskey='" + reportItem.BookingRef.ToString().Trim() + "' style='cursor:pointer;font-size:11px;background-color:OrangeRed;color:white;font-weight:bold;padding-left:20px;padding-right:20px; ' onclick='Payticket(this);'>  Pay  </a>");
                }

                if (reportItem.IsUpdated && reportItem.BookingRef.ToString().Trim().ToUpper() != "REJECT")
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

            //getSale(flightBookingReportItems, out TotalSale, out Totalfare, out TotalCancel, out TotalReject);

            strSale.Append(@"<div class=""col-md-12 col-xs-12 offset-0 hdr"" style=""font-size: 12px;"" >Total Sale Information In INR</div>");
            strSale.Append(@"<div class=""col-md-12 col-xs-12  offset-0"" style=""padding-top: 3px; padding-bottom: 3px;"" >");
            strSale.Append(@"<div class=""col-md-6 col-xs-6 offset-0 tgh"">Total Sale &nbsp;</div><div class=""col-md-6 col-xs-6 offset-0 tgh1"">&nbsp; : " + TotalSale + "</div>");
            strSale.Append(@"</div>");
            strSale.Append(@"<div class=""col-md-12 col-xs-12 offset-0"" style=""padding-top: 3px; padding-bottom: 3px;"" >");
            strSale.Append(@"<div class=""col-md-6 col-xs-6 offset-0 tgh"">Total Rejected &nbsp;</div><div class=""col-md-6 col-xs-6 offset-0 tgh1"">&nbsp;  : " + TotalReject + "</div>");
            strSale.Append(@"</div>");
            strSale.Append(@"<div class=""col-md-12 col-xs-12 offset-0"" style=""padding-top: 3px; padding-bottom: 3px;"" >");
            strSale.Append(@"<div class=""col-md-6 col-xs-6 offset-0 tgh"">Total Cancel &nbsp;</div><div class=""col-md-6 col-xs-6 offset-0 tgh1"">&nbsp;  : " + TotalCancel + "</div>");
            strSale.Append(@" </div>");
            strSale.Append(@"<div class=""col-md-12 col-xs-12 offset-0"" style=""padding-top: 3px; padding-bottom: 3px;"" >");
            strSale.Append(@"<div class=""col-md-6 col-xs-6 offset-0 tgh"">Accurate Sales &nbsp;</div><div class=""col-md-6 col-xs-6 offset-0 tgh1"">&nbsp; : " + Totalfare + "</div>");
            strSale.Append(@"</div>");

            return sbhtml;
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

