using DocumentFormat.OpenXml.Office2016.Drawing.ChartDrawing;
using DocumentFormat.OpenXml.Wordprocessing;
using iText.Html2pdf;
using iText.Kernel.Pdf;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging.Abstractions;
using System.Collections;
using System.Data;
using System.Text;
using ZealTravel.Application.AgencyManagement.Queries;
using ZealTravel.Application.BookingManagement.Query;
using ZealTravel.Common.Helpers;
using ZealTravel.Common.Services;
using ZealTravel.Domain.BookingManagement;
using ZealTravel.Domain.Data.Entities;
using ZealTravel.Domain.Data.Models.Booking;
using ZealTravel.Domain.Interfaces.Handlers;
using ZealTravel.Front.Web.Models;
using ZealTravel.Front.Web.Models.Agency.Booking;
using ZealTravel.Infrastructure.Akaasa;
using log4net.Core;
using ZealTravel.Front.Web.Helper;
using ZealTravel.Application.DBCommonManagement.Commands;

namespace ZealTravel.Front.Web.Controllers
{
    [Authorize]
    public class PrintPopupController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly IHandlesQueryAsync<BookingDetailQuery, BookingDetailData> _bookingTicketQueryHandler;
        private readonly IHandlesQueryAsync<int, bool> _getBookingPnrStatusHandler;
        private readonly IHandlesQueryAsync<int, bool> _getTicketStatusHandler;
        private readonly IHandlesQueryAsync<GetAvailableBalanceQuery, decimal> _getAvailableBalanceQueryHandler;
        private IHandlesCommandAsync<AddDBLogCommand> _addDbLogCommandHandler;
        private readonly ILogger<PrintPopupController> _logger;

        public PrintPopupController(
            IConfiguration configuration,
            IHandlesQueryAsync<BookingDetailQuery, BookingDetailData> bookingTicketQueryHandler,
            [FromKeyedServices("BookingPnrStatusHandler")]  IHandlesQueryAsync<int, bool> getBookingPnrStatusHandler,
            [FromKeyedServices("TicketStatusHandler")]  IHandlesQueryAsync<int, bool> getTicketStatusHandler,
            IHandlesQueryAsync<GetAvailableBalanceQuery, decimal> getAvailableBalanceQueryHandler,
            IHandlesCommandAsync<AddDBLogCommand> addDbLogCommandHandler,
            ILogger<PrintPopupController> logger
            )
        {
            _configuration = configuration;
            _bookingTicketQueryHandler = bookingTicketQueryHandler;
            _getBookingPnrStatusHandler = getBookingPnrStatusHandler;
            _getTicketStatusHandler = getTicketStatusHandler;
            _getAvailableBalanceQueryHandler = getAvailableBalanceQueryHandler;
            _addDbLogCommandHandler = addDbLogCommandHandler;
            _logger = logger;

        }

        [Route("PrintPopup")]
        public async Task<IActionResult> PrintPopup([FromQuery] string bookingRef)
        {
            if (!string.IsNullOrEmpty(bookingRef) && bookingRef.Trim().Length > 0)
            {
                string bookingRefDecoded = EncodeDecodeHelper.DecodeFrom64(bookingRef.Trim());
                int iBookingRef = 0;

                bool parseSuccess = int.TryParse(bookingRefDecoded, out iBookingRef);

                if (!parseSuccess || iBookingRef <= 0)
                {
                    ViewBag.ErrorMessage = "Invalid booking reference. Please check the provided reference and try again.";
                    return View();
                }

                var query = new BookingDetailQuery { BookingRef = iBookingRef };
                var flightbooking = await _bookingTicketQueryHandler.HandleAsync(query);

                if (flightbooking == null)
                {
                    ViewBag.ErrorMessage = "No booking found for the provided reference. Please check the reference and try again.";
                    return View();
                }

                StringBuilder reportHTML = GetFlightTicketPopup(flightbooking, iBookingRef, UserHelper.GetCompanyID(User));
                string hostName = _configuration["SiteURL:BasePath"];

                var viewModel = new PrintPopupResponse
                {
                    BookingRef = bookingRef,
                    ReportTableHTML = !string.IsNullOrEmpty(reportHTML.ToString()) ? reportHTML.ToString() : string.Empty,
                    HdnhostName = hostName,
                    ShowLogo = true,
                    HideFare = false
                };

                bool IsSingleFare = false;
                if (flightbooking.CompanyFlightDetails.FirstOrDefault()?.Trip == "R")
                {
                    var fareDetails = flightbooking.CompanyFareDetailSegmentAirlines
                        .Where(fare => fare.Conn == "I" && fare.PaxType == "ADT");

                    if (fareDetails.Any())
                    {
                        foreach (var fare in fareDetails)
                        {
                            decimal dCount = Convert.ToDecimal(fare.Yq.ToString().Trim())
                                             + Convert.ToDecimal(fare.Psf.ToString().Trim())
                                             + Convert.ToDecimal(fare.Udf.ToString().Trim())
                                             + Convert.ToDecimal(fare.Audf.ToString().Trim())
                                             + Convert.ToDecimal(fare.Cute.ToString().Trim())
                                             + Convert.ToDecimal(fare.Gst.ToString().Trim())
                                             + Convert.ToDecimal(fare.Tf.ToString().Trim())
                                             + Convert.ToDecimal(fare.Cess.ToString().Trim())
                                             + Convert.ToDecimal(fare.Ex.ToString().Trim())
                                             + Convert.ToDecimal(fare.Basic.ToString().Trim());

                            if (dCount <= 0)
                            {
                                IsSingleFare = true;
                                break;
                            }
                        }
                    }
                }

                var firstFlightDetail = flightbooking.CompanyFlightDetails.FirstOrDefault();
                if (firstFlightDetail != null)
                {
                    string tripType = firstFlightDetail.Trip?.ToString();
                    if ((tripType == "R" && IsSingleFare) || tripType == "O")
                    {
                        IsSingleFare = true;
                    }
                }
                viewModel.IsSingleFare = IsSingleFare;
                return View("~/Views/Booking/PrintPopup.cshtml", viewModel);
            }

            ViewBag.ErrorMessage = "Booking reference is required.";
            return View();
        }

        [Route("PrintInvoice")]
        public async Task<IActionResult> PrintInvoice([FromQuery] string bookingRef)
        {
            if (!string.IsNullOrEmpty(bookingRef) && bookingRef.Trim().Length > 0)
            {
                string bookingRefDecoded = EncodeDecodeHelper.DecodeFrom64(bookingRef.Trim());
                int iBookingRef = 0;

                bool parseSuccess = int.TryParse(bookingRefDecoded, out iBookingRef);

                if (!parseSuccess || iBookingRef <= 0)
                {
                    ViewBag.ErrorMessage = "Invalid booking reference. Please check the provided reference and try again.";
                    return View();
                }

                var query = new BookingDetailQuery { BookingRef = iBookingRef };
                var flightbooking = await _bookingTicketQueryHandler.HandleAsync(query);

                if (flightbooking == null)
                {
                    ViewBag.ErrorMessage = "No booking found for the provided reference. Please check the reference and try again.";
                    return View();
                }

                StringBuilder reportHTML = PrintInvoice(flightbooking, iBookingRef);
                string hostName = _configuration["SiteURL:BasePath"];

                var viewModel = new PrintPopupResponse
                {
                    BookingRef = bookingRef,
                    ReportTableHTML = !string.IsNullOrEmpty(reportHTML.ToString()) ? reportHTML.ToString() : string.Empty,
                    HdnhostName = hostName
                };

                return View("~/Views/Booking/PrintInvoice.cshtml", viewModel);
            }

            ViewBag.ErrorMessage = "Booking reference is required.";
            return View();
        }



        [HttpPost]
        public async Task<IActionResult> ExportInvoiceToPDF([FromBody] PrintPopupPdfRequest printPopupPdfRequest)
        {
            if (printPopupPdfRequest == null || string.IsNullOrEmpty(printPopupPdfRequest.BookingRef?.Trim()))
            {
                ViewBag.ErrorMessage = "Booking reference is required.";
                return View();
            }

            string bookingRefDecoded = EncodeDecodeHelper.DecodeFrom64(printPopupPdfRequest.BookingRef.Trim());

            if (int.TryParse(bookingRefDecoded, out int iBookingRef) && iBookingRef > 0)
            {
                var query = new BookingDetailQuery { BookingRef = iBookingRef };
                var flightbooking = await _bookingTicketQueryHandler.HandleAsync(query);

                if (flightbooking != null)
                {
                    string sessionCompany = UserHelper.GetCompanyID(User);

                    StringBuilder reportHTML = new StringBuilder(PrintInvoicePdf(flightbooking, iBookingRef));

                    string baseUri = Directory.GetCurrentDirectory();

                    var margins = (Top: 10f, Right: 10f, Bottom: 0f, Left: 10f);

                    var pdfStream = GeneratePDF.GeneratePdfStream(reportHTML.ToString(), margins, baseUri);

                    return File(pdfStream, "application/pdf", $"AirInvoice_{iBookingRef}.pdf");
                }
                else
                {
                    ViewBag.ErrorMessage = "No booking found for the provided reference.";
                    return View();
                }
            }
            else
            {
                ViewBag.ErrorMessage = "Invalid booking reference. Please check the provided reference.";
                return View();
            }
        }

        [HttpPost]
        public async Task<IActionResult> SendInvoiceEmail([FromBody] PrintPoupEmailRequest printPoupEmailRequest)
        {
            if (!string.IsNullOrWhiteSpace(printPoupEmailRequest.BookingRef) && !string.IsNullOrWhiteSpace(printPoupEmailRequest.Email))
            {
                var bookingRef = printPoupEmailRequest.BookingRef;
                var email = printPoupEmailRequest.Email;
                string bookingRefDecoded = EncodeDecodeHelper.DecodeFrom64(bookingRef.Trim());
                if (int.TryParse(bookingRefDecoded, out int iBookingRef))
                {
                    var query = new BookingDetailQuery { BookingRef = iBookingRef };
                    var flightbooking = await _bookingTicketQueryHandler.HandleAsync(query);
                    if (flightbooking != null)
                    {
                        string reportHTMLString = PrintInvoicePdf(flightbooking, iBookingRef);
                        StringBuilder reportHTML = new StringBuilder(reportHTMLString);

                        string baseUri = Directory.GetCurrentDirectory();
                        var margins = (Top: 10f, Right: 10f, Bottom: 0f, Left: 10f);
                        var pdfStream = GeneratePDF.GeneratePdfStream(reportHTML.ToString(), margins, baseUri);

                        byte[] pdfBytes;
                        using (var memoryStream = new MemoryStream())
                        {
                            pdfStream.CopyTo(memoryStream);
                            pdfBytes = memoryStream.ToArray();
                        }

                        string emailSubject = $"Invoice of BookingRef: {iBookingRef}";
                        string emailBody = $"Please find the attached Booking Ref: {iBookingRef} Invoice.";

                        bool emailSent = await EmailService.SendEmail(email, emailSubject, emailBody, pdfBytes, $"ETicket_{iBookingRef}.pdf");

                        if (emailSent)
                        {
                            return Json(new { success = true, message = "Invoice email sent successfully." });
                        }
                        else
                        {
                            return Json(new { success = false, message = "Failed to send invoice email. Please try again later." });
                        }
                    }
                    else
                    {
                        return Json(new { success = false, message = "No booking found for the provided reference." });
                    }
                }
                else
                {
                    return Json(new { success = false, message = "Invalid booking reference." });
                }
            }
            else
            {
                return Json(new { success = false, message = "Booking reference and email are required." });
            }
        }

        public StringBuilder PrintInvoice(BookingDetailData bookingDetail, int iBookingRef)

        {

            StringBuilder sbResponse = new StringBuilder();


            try
            {
                var flightDetails = bookingDetail.CompanyFlightDetailAirlines;
                var flightSegDetails = bookingDetail.CompanyFlightSegmentDetailAirlines;
                var fareDetails = bookingDetail.CompanyFareDetailAirlines?.FirstOrDefault();
                var fareDetailSeg = bookingDetail.CompanyFareDetailSegmentAirlines;
                var paxDetails = bookingDetail.CompanyPaxDetailAirlines;
                var compDetails = bookingDetail.CompanyFlightDetails?.FirstOrDefault();
                var paxSegmentDetails = bookingDetail.CompanyPaxSegmentDetailAirlines;
                var paxSegmentRuleDetails = bookingDetail.CompanyFlightSegmentRuleDetailAirlines;
                var companyTransactionDetail = bookingDetail.CompanyTransactionDetails;
                var bookingAirlineLogForPGs = bookingDetail.BookingAirlineLogForPGs;
                string Trip = "O";
                int TotalAmount = 0;
                string Amount = string.Empty;
                string clsnm = string.Empty;
                string Class1 = string.Empty;
                string Class2 = string.Empty;
                string flightno1 = string.Empty;
                string flightno2 = string.Empty;
                int Totalcommission = 0;
                int basicplustax = 0;
                int totstax = 0;
                int tds = 0;
                int ServiceFee = 0;
                string departdate1 = string.Empty;
                string departdate2 = string.Empty;
                string Gdspnr_O = string.Empty;
                string Gdspnr_I = string.Empty;
                string Airpnr_O = string.Empty;
                string Airpnr_I = string.Empty;
                Int32 TotalServices = 0;

                string companyNm = string.Empty;
                string companyAddress = string.Empty;
                string companyMobile = string.Empty;
                string companyEmail = string.Empty;
                string Postalcode = string.Empty;
                string GSTNO = string.Empty;
                string Pannno = string.Empty;

                string companyNm_1 = string.Empty;
                string companyAddress_1 = string.Empty;
                string companyMobile_1 = string.Empty;
                string companyEmail_1 = string.Empty;
                string Postalcode_1 = string.Empty;
                string GSTNO_1 = string.Empty;
                string Pannno_1 = string.Empty;
                string bookingref = string.Empty;
                string Bookingdate = string.Empty;
                int SurchargeAmount = 0;

                Double dCurrencyValue = 1;
                string Currency = "INR";

                if (companyTransactionDetail != null && companyTransactionDetail.Any() && bookingAirlineLogForPGs != null && bookingAirlineLogForPGs.Any())
                {
                    dCurrencyValue = Convert.ToDouble(bookingAirlineLogForPGs.FirstOrDefault().CurrencyValue.ToString());
                    Currency = bookingAirlineLogForPGs.FirstOrDefault().Currency.ToString().Trim();
                }
                if (companyTransactionDetail?.FirstOrDefault()?.SurchargeAmount != null && decimal.TryParse(companyTransactionDetail.FirstOrDefault().SurchargeAmount.ToString(), out var surchargeAmount))
                {
                    SurchargeAmount = Decimal.ToInt32(surchargeAmount);
                }
                else
                {
                    SurchargeAmount = 0;
                }

                Trip = flightDetails.FirstOrDefault().Trip.ToString();


                Decimal TotalSeat = Convert.ToDecimal(flightDetails.FirstOrDefault().TotalSeat.ToString());
                Decimal TotalMeal = Convert.ToDecimal(flightDetails.FirstOrDefault().TotalMeal.ToString());
                Decimal TotalBaggage = Convert.ToDecimal(flightDetails.FirstOrDefault().TotalBaggage.ToString());
                TotalServices = Convert.ToInt32(TotalSeat + TotalMeal + TotalBaggage);

                string CompanyID = flightDetails.FirstOrDefault().CompanyId.ToString();

                TotalAmount = Convert.ToInt32(
    string.IsNullOrEmpty(flightDetails.FirstOrDefault()?.TotalFare.ToString())
    ? 0
    : Convert.ToDouble(flightDetails.FirstOrDefault()?.TotalFare.ToString())
) - Convert.ToInt32(
    string.IsNullOrEmpty(flightDetails.FirstOrDefault()?.TotalMarkup.ToString())
    ? 0
    : Convert.ToDouble(flightDetails.FirstOrDefault()?.TotalMarkup.ToString())
);

                Totalcommission = Convert.ToInt32(
                    string.IsNullOrEmpty(flightDetails.FirstOrDefault()?.TotalCommission.ToString())
                    ? 0
                    : Convert.ToDouble(flightDetails.FirstOrDefault()?.TotalCommission.ToString())
                );

                basicplustax = Convert.ToInt32(
                    string.IsNullOrEmpty(flightDetails.FirstOrDefault()?.TotalBasic.ToString())
                    ? 0
                    : Convert.ToDouble(flightDetails.FirstOrDefault()?.TotalBasic.ToString())
                ) + Convert.ToInt32(
                    string.IsNullOrEmpty(flightDetails.FirstOrDefault()?.TotalTax.ToString())
                    ? 0
                    : Convert.ToDouble(flightDetails.FirstOrDefault()?.TotalTax.ToString())
                );

                totstax = Convert.ToInt32(
                    string.IsNullOrEmpty(flightDetails.FirstOrDefault()?.TotalServiceTax.ToString())
                    ? 0
                    : Convert.ToDouble(flightDetails.FirstOrDefault()?.TotalServiceTax.ToString())
                );

                tds = Convert.ToInt32(
                    string.IsNullOrEmpty(flightDetails.FirstOrDefault()?.TotalTds.ToString())
                    ? 0
                    : Convert.ToDouble(flightDetails.FirstOrDefault()?.TotalTds.ToString())
                );

                ServiceFee = Convert.ToInt32(
                    string.IsNullOrEmpty(flightDetails.FirstOrDefault()?.TotalServiceFeeDeal.ToString())
                    ? 0
                    : Convert.ToDouble(flightDetails.FirstOrDefault()?.TotalServiceFeeDeal.ToString())
                );

                TotalAmount = (TotalAmount + TotalServices + SurchargeAmount) - Totalcommission;


                companyNm = StringHelper.GetUpper(compDetails.AA_CompanyName.ToString());
                GSTNO = StringHelper.GetUpper(compDetails.AA_Gst.ToString());
                Pannno = StringHelper.GetUpper(compDetails.AA_Pan_No.ToString());
                Postalcode = compDetails.AA_PostalCode.ToString();
                companyAddress = StringHelper.GetALLFirstCharacterCapital(compDetails.AA_Address.ToString());
                companyMobile = compDetails.AA_Mobile.ToString() + "/";
                companyMobile += compDetails.AA_PhoneNo.ToString();
                companyEmail = StringHelper.GetLower(compDetails.AA_Email.ToString());


                if (CompanyID.IndexOf("-SA-") != -1 || CompanyID.IndexOf("C-") != -1)
                {
                    companyNm_1 = StringHelper.GetUpper(compDetails.A_CompanyName.ToString());
                    GSTNO_1 = StringHelper.GetUpper(compDetails.A_Gst.ToString());
                    Pannno_1 = StringHelper.GetUpper(compDetails.A_Pan_No.ToString());
                    Postalcode_1 = compDetails.A_PostalCode.ToString();
                    companyAddress_1 = StringHelper.GetALLFirstCharacterCapital(compDetails.A_Address.ToString());
                    companyMobile_1 = compDetails.A_Mobile.ToString() + "/";
                    companyMobile_1 += compDetails.A_PhoneNo.ToString();
                    companyEmail_1 = StringHelper.GetLower(compDetails.A_Email.ToString());
                }
                else
                {
                    companyNm_1 = StringHelper.GetUpper(compDetails.CompanyName.ToString());
                    GSTNO_1 = StringHelper.GetUpper(compDetails.Gst.ToString());
                    Pannno_1 = StringHelper.GetUpper(compDetails.Pan_No.ToString());
                    Postalcode_1 = compDetails.PostalCode.ToString();
                    companyAddress_1 = StringHelper.GetALLFirstCharacterCapital(compDetails.Address.ToString().ToLower());
                    companyMobile_1 = compDetails.Mobile.ToString() + "/";
                    companyMobile_1 += compDetails.PhoneNo.ToString();
                    companyEmail_1 = StringHelper.GetLower(compDetails.Email.ToString().ToLower());
                }


                Amount = StringHelper.NumberToText(TotalAmount, false);



                string Airline_PNR_D = flightDetails.FirstOrDefault().Airline_Pnr_D.ToString();
                string Airline_PNR_A = flightDetails.FirstOrDefault().Airline_Pnr_A.ToString();
                string GDS_PNR_D = flightDetails.FirstOrDefault().Gds_Pnr_D.ToString();
                string GDS_PNR_A = flightDetails.FirstOrDefault().Gds_Pnr_A.ToString();
                string Trip11 = flightDetails.FirstOrDefault().Trip.ToString();
                Gdspnr_O = GDS_PNR_D;
                Gdspnr_I = string.Empty;
                if (GDS_PNR_A.Length > 0 && Trip11.Equals("R"))
                {
                    Gdspnr_I = GDS_PNR_A;
                }
                Airpnr_O = Airline_PNR_D;
                Airpnr_I = string.Empty;
                if (Airline_PNR_A.Length > 0 && Trip11.Equals("R"))
                {
                    Airpnr_I = Airline_PNR_A;
                }

                string eventtime = Convert.ToDateTime(flightSegDetails.FirstOrDefault().EventTime.ToString()).ToString("yyyy-MM-dd");
                foreach (var flightDet in flightSegDetails)
                {
                    if (flightDet.ConnOrder.Equals("O1"))
                    {
                        flightno1 = flightDet.FlightNumber.ToString();
                        departdate1 = flightDet.DepartureDate.ToString();
                        break;
                    }
                }
                foreach (var flightDet in flightSegDetails)
                {
                    if (flightDet.ConnOrder.IndexOf("I") != -1)
                    {
                        flightno2 = flightDet.FlightNumber.ToString();
                        departdate2 = flightDet.DepartureDate.ToString();
                        break;
                    }
                }

                foreach (var flightDet in flightSegDetails)
                {
                    if (flightDet.ConnOrder.Equals("O1"))
                    {
                        Class1 = flightDet.ClassOfService.ToString();
                    }
                    else if (flightDet.ConnOrder.IndexOf("I") != -1)
                    {
                        Class2 = flightDet.ClassOfService.ToString();
                    }
                }


                bookingref = flightSegDetails.FirstOrDefault().BookingRef.ToString();
                Bookingdate = Convert.ToDateTime(flightSegDetails.FirstOrDefault().EventTime.ToString()).ToString("yyyy-MM-dd").ToString();




                sbResponse.Append("<table style='width:785px; margin:0 auto;' >");
                sbResponse.Append("<tbody>");
                sbResponse.Append("<tr>");
                sbResponse.Append("<td>");
                sbResponse.Append("<table align='center' style='width:785px;border: 1px solid #ccc;font-size:15px;font-family:math;' cellspacing='0'  >");
                sbResponse.Append("<tbody>");
                sbResponse.Append("<tr>");
                sbResponse.Append("<td>");
                sbResponse.Append("<table width='100%' style='font-size:15px;' cellspacing='0' cellpadding='5' border='0' >");
                sbResponse.Append("<tbody>");
                sbResponse.Append("<tr>");
                sbResponse.Append("<td style='font-size:35px;' width='33%' align='center'>   </td>");
                sbResponse.Append("<td style='font-size:35px;' width='33%' align='center'> INVOICE  </td>");
                sbResponse.Append("<td style='font-size:35px;' width='33%' align='center'>   </td>");
                sbResponse.Append("</tr>");
                sbResponse.Append("<tr>");

                sbResponse.Append("<td style=' border-bottom: 1px solid #ccc;' width='33%' align='left'>");
                sbResponse.Append("<span>BookingRef </span>:<span><b style='font-size:16px'> " + bookingref + " </b> </span>");
                sbResponse.Append("</td>");

                sbResponse.Append("<td style='font-size:35px; border-bottom: 1px solid #ccc;' width='33%' align='center'>   </td>");
                sbResponse.Append("<td style='border-bottom: 1px solid #ccc;' width='30%' align='left'>");

                sbResponse.Append("<span> Invoice On  :</span><span> " + Bookingdate + " </span>");
                sbResponse.Append("</td>");

                sbResponse.Append("</tr>");

                sbResponse.Append("<tr>");
                sbResponse.Append("<td colspan='3'>");
                sbResponse.Append("<table width='100%'>");
                sbResponse.Append("<tr>");

                sbResponse.Append("<td width='55%'>");
                sbResponse.Append("<span style='font-weight:600; font-size: 17px;'>To M/s :</span><span style='font-weight:600; font-size: 17px;'>" + companyNm_1 + "</span> <br />");
                sbResponse.Append("<span style='color: #929292;' >Tel No </span>:<span style='color: #929292;' >" + companyMobile_1 + "</span><br />");
                sbResponse.Append("<span style='color: #929292;' >Email </span>:<span style='color: #929292;' >" + companyEmail_1 + "</span><br />");
                sbResponse.Append("<span style='color: #929292;' >Pan No </span>:<span style='color: #929292;' >" + Pannno_1 + "</span><br />");
                sbResponse.Append("<span style='color: #929292;' >GSTIN </span>:<span style='color: #929292;' >" + GSTNO_1 + "</span><br/>");
                sbResponse.Append("<span style='color: #929292;' >" + companyAddress_1 + "</span><span style='color: #929292;' > " + Postalcode_1 + "</span><br />");
                sbResponse.Append("</td>");

                sbResponse.Append("<td width='10%' style='font-size:25px;font-weight:bold;padding:15px 0' valign='top' align='center'>");
                sbResponse.Append("</td>");

                sbResponse.Append("<td align='left' width='35%'>");
                sbResponse.Append("<b style='font-size: 17px;' >" + companyNm + "</b><br />");
                sbResponse.Append("<span style='color: #929292;' >Tel No </span>:<span style='color: #929292;'>" + companyMobile + "</span><br />");
                sbResponse.Append("<span style='color: #929292;' >Email </span>:<span style='color: #929292;'>" + companyEmail + "</span><br />");
                sbResponse.Append("<span style='color: #929292;' >Pan No </span>:<span style='color: #929292;' >" + Pannno + "</span><br />");
                sbResponse.Append("<span style='color: #929292;' >GSTIN </span>:<span style='color: #929292;' >" + GSTNO + "</span> <br/>");
                sbResponse.Append("<span style='color: #929292;' >" + companyAddress + "</span>");
                sbResponse.Append("<span style='color: #929292;' > " + Postalcode + "</span>");
                sbResponse.Append("</td>");

                sbResponse.Append("</tr>");
                sbResponse.Append("</table>");
                sbResponse.Append("</td>");
                sbResponse.Append("</tr>");


                sbResponse.Append("<tr>");
                sbResponse.Append("<td colspan='3'>");
                sbResponse.Append("<table style='font-size:15px;  border-top: 1px solid #d4d4d4;    background: ;' width='100%' cellpadding='10' cellspacing='0' >");
                sbResponse.Append("<tr>");
                sbResponse.Append("<td width='50%'>");
                sbResponse.Append(" <span style='font-size: 15px;   color: #929292;'> <img src='/assets/img/airlogo_square/onwardflight.png' width='20px' /> Onward Flight</span>");
                sbResponse.Append("<span style='font-size: 15px;'> <b>  " + flightDetails.FirstOrDefault().OriginCity.ToString().Trim() + " to  </b> </span> ");
                sbResponse.Append("<span style='font-size: 15px;'> <b>  " + flightDetails.FirstOrDefault().DestinationCity.ToString().Trim() + " </b> </span>");
                sbResponse.Append("</td>");

                sbResponse.Append("<td width='50%' align='right' >");
                sbResponse.Append("<span style='font-size: 15px;'>  Airline PNR  </span>");
                sbResponse.Append("<span style='font-size: 22px;'>  <b> " + flightDetails.FirstOrDefault().Airline_Pnr_D.ToString() + " </b>  </span>");
                sbResponse.Append("</td>");
                sbResponse.Append("</tr>");
                sbResponse.Append("</table>");
                sbResponse.Append("</td>");
                sbResponse.Append("</tr>");
                sbResponse.Append("<tr>");
                sbResponse.Append("<td colspan='3'>");
                sbResponse.Append("<table width='100%' style='font-size:15px;    border: 1px solid #d4d4d4; ' cellpadding='10' cellspacing='0'>");
                sbResponse.Append("<tbody>");
                sbResponse.Append("<tr style=' background: ; font-size:15px; font-weight:bold;' >");
                sbResponse.Append("<td width='5'>  #  </td>");
                sbResponse.Append("<td width='25'>  TicketNo   </td>");
                sbResponse.Append("<td width='25'>   Passengers  </td>");
                sbResponse.Append("<td width='15'>   Sector   </td>");
                sbResponse.Append("<td width='15'>    Flight   </td>");
                sbResponse.Append("<td width='15'>  BFare     </td>");
                sbResponse.Append("<td width='15'>   Taxes    </td>");
                sbResponse.Append("</tr>");

                string PaxType = ""; string PaxName = "";
                foreach (var flightDet in flightDetails)
                {
                    string CarrierName_D = flightDet.CarrierNameD?.ToString() ?? string.Empty;
                    string Carriercode_D = flightDet.CarrierCode_D;
                    string sec = flightDet.Sector?.ToString() ?? string.Empty;
                    string Adlt = flightDet.Adt?.ToString() ?? string.Empty;
                    string Chd = flightDet.Chd?.ToString() ?? string.Empty;
                    string Inf = flightDet.Inf?.ToString() ?? string.Empty;
                    string DepartureDate_D = flightDet.DepartureDate_D?.ToString() ?? string.Empty;
                    string Origin = flightDet.Origin?.ToString() ?? string.Empty;
                    string Destination = flightDet.Destination?.ToString() ?? string.Empty;
                    int AdltInt = 0, ChdInt = 0, InfInt = 0;

                    int.TryParse(Adlt, out AdltInt);
                    int.TryParse(Chd, out ChdInt);
                    int.TryParse(Inf, out InfInt);



                    string TotalFare = "0";
                    string BAsic = "0";
                    string Totaltax = "0";

                    string C_TotalFare = "0";
                    string C_BAsic = "0";
                    string C_Totaltax = "0";

                    string I_TotalFare = "0";
                    string I_BAsic = "0";
                    string I_Totaltax = "0";

                    if (Adlt.Any())
                    {
                        foreach (var fareDetail in fareDetailSeg)
                        {
                            if (fareDetail.PaxType.ToString().Trim().Equals("ADT") && fareDetail.Conn.ToString().Trim().Equals("O"))
                            {
                                Decimal basicfare = Convert.ToDecimal(fareDetail.Basic.ToString().Trim());
                                Decimal YQ = Convert.ToDecimal(fareDetail.Yq.ToString().Trim());
                                Decimal Psf = Convert.ToDecimal(fareDetail.Psf.ToString().Trim());
                                Decimal UDF = Convert.ToDecimal(fareDetail.Udf.ToString().Trim());
                                Decimal AUDF = Convert.ToDecimal(fareDetail.Audf.ToString().Trim());
                                Decimal Cute = Convert.ToDecimal(fareDetail.Cute.ToString().Trim());
                                Decimal GST = Convert.ToDecimal(fareDetail.Gst.ToString().Trim());
                                Decimal TF = Convert.ToDecimal(fareDetail.Tf.ToString().Trim());
                                Decimal CESS = Convert.ToDecimal(fareDetail.Cess.ToString().Trim());
                                Decimal EX = Convert.ToDecimal(fareDetail.Ex.ToString().Trim());
                                Decimal MU = Convert.ToDecimal(fareDetail.Markup1.ToString().Trim());


                                TotalFare = (basicfare + YQ + UDF + Psf + AUDF + Cute + GST + TF + CESS + EX).ToString();
                                BAsic = basicfare.ToString();
                                Totaltax = (YQ + UDF + Psf + AUDF + Cute + GST + TF + CESS + EX).ToString();
                            }
                        }
                    }

                    if (Chd.Any())
                    {
                        foreach (var fareDetail in fareDetailSeg)
                        {
                            if (fareDetail.PaxType.ToString().Trim().Equals("CHD") && fareDetail.Conn.ToString().Trim().Equals("O"))
                            {
                                Decimal basicfare = Convert.ToDecimal(fareDetail.Basic.ToString().Trim());
                                Decimal YQ = Convert.ToDecimal(fareDetail.Yq.ToString().Trim());
                                Decimal Psf = Convert.ToDecimal(fareDetail.Psf.ToString().Trim());
                                Decimal UDF = Convert.ToDecimal(fareDetail.Udf.ToString().Trim());
                                Decimal AUDF = Convert.ToDecimal(fareDetail.Audf.ToString().Trim());
                                Decimal Cute = Convert.ToDecimal(fareDetail.Cute.ToString().Trim());
                                Decimal GST = Convert.ToDecimal(fareDetail.Gst.ToString().Trim());
                                Decimal TF = Convert.ToDecimal(fareDetail.Tf.ToString().Trim());
                                Decimal CESS = Convert.ToDecimal(fareDetail.Cess.ToString().Trim());
                                Decimal EX = Convert.ToDecimal(fareDetail.Ex.ToString().Trim());
                                Decimal MU = Convert.ToDecimal(fareDetail.Markup1.ToString().Trim());

                                C_TotalFare = (basicfare + YQ + UDF + Psf + AUDF + Cute + GST + TF + CESS + EX).ToString();
                                C_BAsic = basicfare.ToString();
                                C_Totaltax = (YQ + UDF + Psf + AUDF + Cute + GST + TF + CESS + EX).ToString();
                            }
                        }

                    }

                    if (Inf.Any())
                    {
                        foreach (var fareDetail in fareDetailSeg)
                        {
                            if (fareDetail.PaxType.ToString().Trim().Equals("INF") && fareDetail.Conn.ToString().Trim().Equals("O"))
                            {
                                Decimal basicfare = Convert.ToDecimal(fareDetail.Basic.ToString().Trim());
                                Decimal YQ = Convert.ToDecimal(fareDetail.Yq.ToString().Trim());
                                Decimal Psf = Convert.ToDecimal(fareDetail.Psf.ToString().Trim());
                                Decimal UDF = Convert.ToDecimal(fareDetail.Udf.ToString().Trim());
                                Decimal AUDF = Convert.ToDecimal(fareDetail.Audf.ToString().Trim());
                                Decimal Cute = Convert.ToDecimal(fareDetail.Cute.ToString().Trim());
                                Decimal GST = Convert.ToDecimal(fareDetail.Gst.ToString().Trim());
                                Decimal TF = Convert.ToDecimal(fareDetail.Tf.ToString().Trim());
                                Decimal CESS = Convert.ToDecimal(fareDetail.Cess.ToString().Trim());
                                Decimal EX = Convert.ToDecimal(fareDetail.Ex.ToString().Trim());
                                Decimal MU = Convert.ToDecimal(fareDetail.Markup1.ToString().Trim());

                                I_TotalFare = (basicfare + YQ + UDF + Psf + AUDF + Cute + GST + TF + CESS + EX).ToString();
                                I_BAsic = basicfare.ToString();
                                I_Totaltax = (YQ + UDF + Psf + AUDF + Cute + GST + TF + CESS + EX).ToString();
                            }
                        }

                    }

                    string Sector = Origin + " " + Destination;
                    int totalpax = AdltInt + ChdInt + InfInt;
                    int srno = 1;
                    foreach (var paxDetail in paxDetails)
                    {
                        PaxType = paxDetail.PaxType.ToString();
                        string Fname = paxDetail.First_Name.ToString();
                        string MName = paxDetail.Middle_Name.ToString();
                        string LName = paxDetail.Last_Name.ToString();
                        string ticketno = paxDetail.TicketNo.ToString();
                        PaxName = Fname + " " + MName + " " + LName;

                        sbResponse.Append("<tr>");
                        sbResponse.Append("<td style='border-top: 1px solid rgba(0, 0, 0, 0.17);border-right: 1px solid #ccc;'>");
                        sbResponse.Append("<span>" + srno + "</span>");
                        sbResponse.Append("</td>");
                        sbResponse.Append("<td style='border-top: 1px solid rgba(0, 0, 0, 0.17);border-right: 1px solid #ccc;'>");
                        sbResponse.Append("<span>" + ticketno + "</span>");
                        sbResponse.Append("</td>");
                        sbResponse.Append("<td style='border-top: 1px solid rgba(0, 0, 0, 0.17);border-right: 1px solid #ccc;'>");
                        sbResponse.Append("<span>" + StringHelper.GetUpper(PaxName) + "(" + PaxType + ")</span>");
                        sbResponse.Append("</td>");
                        sbResponse.Append("<td style='border-top: 1px solid rgba(0, 0, 0, 0.17);border-right: 1px solid #ccc;'>");
                        sbResponse.Append("<span> " + Sector + "</span>");
                        sbResponse.Append("</td>");
                        sbResponse.Append("<td style='border-top: 1px solid rgba(0, 0, 0, 0.17);border-right: 1px solid #ccc;'>");
                        sbResponse.Append("<span>" + Carriercode_D + "-" + flightno1 + "</span>");
                        sbResponse.Append("</td>");

                        if (PaxType == "ADT")
                        {
                            sbResponse.Append("<td style='border-top: 1px solid rgba(0, 0, 0, 0.17);border-right: 1px solid #ccc;'>");
                            sbResponse.Append("<span>" + BAsic + "</span>");
                            sbResponse.Append("</td>");
                            sbResponse.Append("<td style='border-top: 1px solid rgba(0, 0, 0, 0.17);border-right: 1px solid #ccc;'>");
                            sbResponse.Append("<span>" + Totaltax + "</span>");
                            sbResponse.Append("</td>");
                        }
                        if (PaxType == "CHD")
                        {
                            sbResponse.Append("<td style='border-top: 1px solid rgba(0, 0, 0, 0.17);border-right: 1px solid #ccc;'>");
                            sbResponse.Append("<span>" + C_BAsic + "</span>");
                            sbResponse.Append("</td>");
                            sbResponse.Append("<td style='border-top: 1px solid rgba(0, 0, 0, 0.17);border-right: 1px solid #ccc;'>");
                            sbResponse.Append("<span>" + C_Totaltax + "</span>");
                            sbResponse.Append("</td>");
                        }
                        if (PaxType == "INF")
                        {
                            sbResponse.Append("<td style='border-top: 1px solid rgba(0, 0, 0, 0.17);border-right: 1px solid #ccc;'>");
                            sbResponse.Append("<span>" + I_BAsic + "</span>");
                            sbResponse.Append("</td>");
                            sbResponse.Append("<td style='border-top: 1px solid rgba(0, 0, 0, 0.17);border-right: 1px solid #ccc;'>");
                            sbResponse.Append("<span>" + I_Totaltax + "</span>");
                            sbResponse.Append("</td>");
                        }
                        srno++;
                    }
                }
                sbResponse.Append("</tbody>");
                sbResponse.Append("</table>");
                sbResponse.Append("</td>");
                sbResponse.Append("</tr>");
                if (Trip == "R")
                {

                    sbResponse.Append("<tr>");
                    sbResponse.Append("<td colspan='3'>");
                    sbResponse.Append("<table style='font-size:15px;   background: ;' width='100%' cellpadding='10' cellspacing='0' >");
                    sbResponse.Append("<tr>");
                    sbResponse.Append("<td width='50%'>");
                    sbResponse.Append(" <span style='font-size: 15px; color: #929292;'> <img src='/assets/img/airlogo_square/Inwardflight.png' width='20px' /> Return Flight </span>");
                    sbResponse.Append("<span style='font-size: 15px;'> <b> " + flightDetails.FirstOrDefault().DestinationCity.ToString().Trim() + " to  </b> </span> ");
                    sbResponse.Append("<span style='font-size: 15px;'> <b> " + flightDetails.FirstOrDefault().OriginCity.ToString().Trim() + "  </b> </span>");
                    sbResponse.Append("</td>");

                    sbResponse.Append("<td width='50%' align='right' >");
                    sbResponse.Append("<span style='font-size: 15px;'>  Airline PNR  </span>");
                    sbResponse.Append("<span style='font-size: 22px;'>  <b>" + Airpnr_I + " </b>  </span>");
                    sbResponse.Append("</td>");
                    sbResponse.Append("</tr>");
                    sbResponse.Append("</table>");
                    sbResponse.Append("</td>");
                    sbResponse.Append("</tr>");

                    sbResponse.Append("<tr>");
                    sbResponse.Append("<td colspan='3'>");
                    sbResponse.Append("<table width='100%' style='font-size:15px;    border: 1px solid #d4d4d4; ' cellpadding='10' cellspacing='0'>");
                    sbResponse.Append("<tbody>");
                    sbResponse.Append("<tr style=' background: ; font-size:15px; font-weight:bold;' >");
                    sbResponse.Append("<td width='5'>  #  </td>");
                    sbResponse.Append(" <td width='25'>  TicketNo   </td>");
                    sbResponse.Append("<td width='25'>   Passengers  </td>");
                    sbResponse.Append("<td width='15'>   Sector   </td>");
                    sbResponse.Append("<td width='15'>    Flight   </td>");
                    sbResponse.Append("<td width='15'>  BFare     </td>");
                    sbResponse.Append("<td width='15'>   Taxes    </td>");
                    sbResponse.Append("</tr>");

                    foreach (var fltDetails in flightDetails)
                    {
                        string CarrierName_A = fltDetails.CarrierName_A.ToString();
                        string Carriercode_A = fltDetails.CarrierCode_A.ToString();
                        string sec = fltDetails.Sector.ToString();
                        string PaxTypeIB = "";
                        string PaxNameIB = "";
                        int Adlt = Convert.ToInt32(fltDetails.Adt);
                        int Chd = Convert.ToInt32(fltDetails.Chd);
                        int Inf = Convert.ToInt32(fltDetails.Inf);
                        string DepartureDate_A = fltDetails.DepartureDate_A.ToString();
                        string Origin = fltDetails.Destination.ToString();
                        string Destination = fltDetails.Origin.ToString();

                        string TotalFare = "0";
                        string BAsic = "0";
                        string Totaltax = "0";

                        string C_TotalFare = "0";
                        string C_BAsic = "0";
                        string C_Totaltax = "0";

                        string I_TotalFare = "0";
                        string I_BAsic = "0";
                        string I_Totaltax = "0";

                        if (Adlt > 0)
                        {
                            foreach (var faredetail in fareDetailSeg)
                            {
                                if (faredetail.PaxType.Trim().Equals("ADT") && faredetail.Conn.Trim().Equals("I"))
                                {
                                    Decimal basicfare = Convert.ToDecimal(faredetail.Basic.ToString().Trim());
                                    Decimal YQ = Convert.ToDecimal(faredetail.Yq.ToString().Trim());
                                    Decimal Psf = Convert.ToDecimal(faredetail.Psf.ToString().Trim());
                                    Decimal UDF = Convert.ToDecimal(faredetail.Udf.ToString().Trim());
                                    Decimal AUDF = Convert.ToDecimal(faredetail.Audf.ToString().Trim());
                                    Decimal Cute = Convert.ToDecimal(faredetail.Cute.ToString().Trim());
                                    Decimal GST = Convert.ToDecimal(faredetail.Gst.ToString().Trim());
                                    Decimal TF = Convert.ToDecimal(faredetail.Tf.ToString().Trim());
                                    Decimal CESS = Convert.ToDecimal(faredetail.Cess.ToString().Trim());
                                    Decimal EX = Convert.ToDecimal(faredetail.Ex.ToString().Trim());

                                    TotalFare = (basicfare + YQ + UDF + Psf + AUDF + Cute + GST + TF + CESS + EX).ToString();
                                    BAsic = basicfare.ToString();
                                    Totaltax = (YQ + UDF + Psf + AUDF + Cute + GST + TF + CESS + EX).ToString();
                                }
                            }

                        }
                        if (Chd > 0)
                        {
                            foreach (var faredetailseg in fareDetailSeg)
                            {
                                if (faredetailseg.PaxType.Trim().Equals("CHD") && faredetailseg.Conn.Trim().Equals("I"))
                                {
                                    Decimal basicfare = Convert.ToDecimal(faredetailseg.Basic.ToString().Trim());
                                    Decimal YQ = Convert.ToDecimal(faredetailseg.Yq.ToString().Trim());
                                    Decimal Psf = Convert.ToDecimal(faredetailseg.Psf.ToString().Trim());
                                    Decimal UDF = Convert.ToDecimal(faredetailseg.Udf.ToString().Trim());
                                    Decimal AUDF = Convert.ToDecimal(faredetailseg.Audf.ToString().Trim());
                                    Decimal Cute = Convert.ToDecimal(faredetailseg.Cute.ToString().Trim());
                                    Decimal GST = Convert.ToDecimal(faredetailseg.Gst.ToString().Trim());
                                    Decimal TF = Convert.ToDecimal(faredetailseg.Tf.ToString().Trim());
                                    Decimal CESS = Convert.ToDecimal(faredetailseg.Cess.ToString().Trim());
                                    Decimal EX = Convert.ToDecimal(faredetailseg.Ex.ToString().Trim());

                                    C_TotalFare = (basicfare + YQ + UDF + Psf + AUDF + Cute + GST + TF + CESS + EX).ToString();
                                    C_BAsic = basicfare.ToString();
                                    C_Totaltax = (YQ + UDF + Psf + AUDF + Cute + GST + TF + CESS + EX).ToString();
                                }
                            }

                        }
                        if (Inf > 0)
                        {
                            foreach (var faredetailseg in fareDetailSeg)
                            {
                                if (faredetailseg.PaxType.Trim().Equals("INF") && faredetailseg.Conn.Trim().Equals("I"))
                                {
                                    Decimal basicfare = Convert.ToDecimal(faredetailseg.Basic.ToString().Trim());
                                    Decimal YQ = Convert.ToDecimal(faredetailseg.Yq.ToString().Trim());
                                    Decimal Psf = Convert.ToDecimal(faredetailseg.Psf.ToString().Trim());
                                    Decimal UDF = Convert.ToDecimal(faredetailseg.Udf.ToString().Trim());
                                    Decimal AUDF = Convert.ToDecimal(faredetailseg.Audf.ToString().Trim());
                                    Decimal Cute = Convert.ToDecimal(faredetailseg.Cute.ToString().Trim());
                                    Decimal GST = Convert.ToDecimal(faredetailseg.Gst.ToString().Trim());
                                    Decimal TF = Convert.ToDecimal(faredetailseg.Tf.ToString().Trim());
                                    Decimal CESS = Convert.ToDecimal(faredetailseg.Cess.ToString().Trim());
                                    Decimal EX = Convert.ToDecimal(faredetailseg.Ex.ToString().Trim());

                                    I_TotalFare = (basicfare + YQ + UDF + Psf + AUDF + Cute + GST + TF + CESS + EX).ToString();
                                    I_BAsic = basicfare.ToString();
                                    I_Totaltax = (YQ + UDF + Psf + AUDF + Cute + GST + TF + CESS + EX).ToString();
                                }
                            }

                        }

                        string Sector = Origin + " " + Destination;
                        int totalpax = Adlt + Chd + Inf;
                        if (totalpax > 0)
                        {
                            int srno = 1;
                            foreach (var paxDetail in paxDetails)
                            {
                                PaxTypeIB = paxDetail.PaxType.ToString();
                                string Fname = paxDetail.First_Name.ToString();
                                string MName = paxDetail.Middle_Name.ToString();
                                string LName = paxDetail.Last_Name.ToString();
                                string ticketno = paxDetail.TicketNo.ToString();
                                PaxNameIB = Fname + " " + MName + " " + LName;
                                sbResponse.Append("<tr>");
                                sbResponse.Append("<td style='border-top: 1px solid rgba(0, 0, 0, 0.17);border-right: 1px solid #ccc;'>");
                                sbResponse.Append("<span>" + srno + "</span>");
                                sbResponse.Append("</td>");
                                sbResponse.Append("<td style='border-top: 1px solid rgba(0, 0, 0, 0.17);border-right: 1px solid #ccc;'>");
                                sbResponse.Append("<span> " + ticketno + "</span>");
                                sbResponse.Append("</td>");
                                sbResponse.Append("<td style='border-top: 1px solid rgba(0, 0, 0, 0.17);border-right: 1px solid #ccc;'>");
                                sbResponse.Append(" <span>" + StringHelper.GetUpper(PaxNameIB) + "(" + PaxTypeIB + ")</span>");
                                sbResponse.Append("</td>");
                                sbResponse.Append("<td style='border-top: 1px solid rgba(0, 0, 0, 0.17);border-right: 1px solid #ccc;'>");
                                sbResponse.Append("<span>" + Sector + "</span>");
                                sbResponse.Append("</td>");
                                sbResponse.Append("<td style='border-top: 1px solid rgba(0, 0, 0, 0.17);border-right: 1px solid #ccc;'>");
                                sbResponse.Append("<span>" + Carriercode_A + "-" + flightno2 + "</span>");
                                sbResponse.Append("</td>");
                                if (PaxTypeIB == "ADT")
                                {
                                    sbResponse.Append("<td style='border-top: 1px solid rgba(0, 0, 0, 0.17);border-right: 1px solid #ccc;'>");
                                    sbResponse.Append("<span>" + BAsic + "</span>");
                                    sbResponse.Append("</td>");
                                    sbResponse.Append("<td style='border-top: 1px solid rgba(0, 0, 0, 0.17);border-right: 1px solid #ccc;'>");
                                    sbResponse.Append("<span> " + Totaltax + "</span>");
                                    sbResponse.Append("</td>");
                                }
                                if (PaxTypeIB == "CHD")
                                {
                                    sbResponse.Append("<td style='border-top: 1px solid rgba(0, 0, 0, 0.17);border-right: 1px solid #ccc;'>");
                                    sbResponse.Append("<span>" + C_BAsic + "</span>");
                                    sbResponse.Append("</td>");
                                    sbResponse.Append("<td style='border-top: 1px solid rgba(0, 0, 0, 0.17);border-right: 1px solid #ccc;'>");
                                    sbResponse.Append("<span>" + C_Totaltax + "</span>");
                                    sbResponse.Append("</td>");
                                }
                                if (PaxTypeIB == "INF")
                                {
                                    sbResponse.Append("<td style='border-top: 1px solid rgba(0, 0, 0, 0.17);border-right: 1px solid #ccc;'>");
                                    sbResponse.Append("<span>" + I_BAsic + "</span>");
                                    sbResponse.Append("</td>");
                                    sbResponse.Append("<td style='border-top: 1px solid rgba(0, 0, 0, 0.17);border-right: 1px solid #ccc;'>");
                                    sbResponse.Append("<span> " + I_Totaltax + "</span>");
                                    sbResponse.Append("</td>");
                                }
                                sbResponse.Append("</tr>");
                                srno++;
                            }
                        }
                        sbResponse.Append("</tbody>");

                        sbResponse.Append("</table>");
                        sbResponse.Append("</td>");
                        sbResponse.Append("</tr>");
                    }
                }

                sbResponse.Append("<tr>");
                sbResponse.Append("<td colspan='3'>");
                sbResponse.Append("<table width='100%'>");
                sbResponse.Append("<tr>");
                sbResponse.Append("<td width='50%' valign='' align='right'>");
                sbResponse.Append("<table style='font-size:15px;' width='100%' align='right' >");
                sbResponse.Append("<tbody>");

                sbResponse.Append("<tr>");
                sbResponse.Append("<td width='76%' ></td> ");
                sbResponse.Append("<td>ADD :</td>");
                sbResponse.Append("<td>Services</td>");
                sbResponse.Append("</tr>");
                sbResponse.Append("<tr>");
                sbResponse.Append("<td width='76%' ></td>");
                sbResponse.Append("<td>Less  :</td>");
                sbResponse.Append("<td> Commission</td>");
                sbResponse.Append("</tr>");
                sbResponse.Append("<tr>");
                sbResponse.Append("<td width='76%' ></td>");
                sbResponse.Append("<td>ADD    :</td>");
                sbResponse.Append("<td>  TDS</td>");
                sbResponse.Append("</tr>");
                sbResponse.Append("<tr>");
                sbResponse.Append("<td width='76%' ></td>");
                sbResponse.Append("<td>ADD  :</td>");
                sbResponse.Append("<td> Service Charge</td>");
                sbResponse.Append("</tr>");
                sbResponse.Append("<tr>");
                sbResponse.Append("<td width='76%' ></td>");
                sbResponse.Append("<td>ADD  :</td>");
                sbResponse.Append("<td> GST</td>");
                sbResponse.Append("</tr>");

                sbResponse.Append("<tr>");
                sbResponse.Append("<td width='76%' ></td>");
                sbResponse.Append("<td>ADD  :</td>");
                sbResponse.Append("<td> Conv.fee</td>");
                sbResponse.Append("</tr>");

                sbResponse.Append("</tbody>");
                sbResponse.Append("</table>");
                sbResponse.Append("</td>");

                sbResponse.Append("<td width='15%' valign='top' align='right'>");
                sbResponse.Append("<table style='font-size:15px;'>");
                sbResponse.Append("<tbody>");
                sbResponse.Append("<tr>");
                sbResponse.Append("<td align='right'>Total  :</td>");
                sbResponse.Append("<td>" + basicplustax + "" + Currency + "</td>");
                sbResponse.Append("</tr>");
                sbResponse.Append("<tr>");
                sbResponse.Append("<td align='right'> Total  :  </td>");
                sbResponse.Append("<td align='right'>" + TotalServices + "" + Currency + "</td>");
                sbResponse.Append("</tr>");
                sbResponse.Append("<tr>");
                sbResponse.Append("<td align='right'> Total  :  </td>");
                sbResponse.Append("<td align='right'>" + Totalcommission + "" + Currency + "</td>");
                sbResponse.Append("</tr>");
                sbResponse.Append("<tr>");
                sbResponse.Append("<td align='right'> Total  :  </td>");
                sbResponse.Append("<td align='right'>" + tds + "" + Currency + "</td>");
                sbResponse.Append("</tr>");
                sbResponse.Append("<tr>");
                sbResponse.Append("<td align='right'>Total  :  </td>");
                sbResponse.Append("<td align='right'>" + ServiceFee + "" + Currency + "</td>");
                sbResponse.Append("</tr>");
                sbResponse.Append("<tr>");
                sbResponse.Append("<td align='right'> @ 18.00 %:  </td>");
                sbResponse.Append("<td align='right'>" + totstax + "" + Currency + "</td>");
                sbResponse.Append("</tr>");

                sbResponse.Append("<tr>");
                sbResponse.Append("<td align='right'>Total  :  </td>");
                sbResponse.Append("<td align='right'>" + SurchargeAmount + "" + Currency + "</td>");
                sbResponse.Append("</tr>");


                sbResponse.Append("<tr>");
                sbResponse.Append("<td align='right'><b> Total Amount </b> </td>");
                sbResponse.Append("<td align='right'>" + TotalAmount + "" + Currency + "</td>");
                sbResponse.Append("</tr>");
                sbResponse.Append("</tbody>");
                sbResponse.Append("</table>");
                sbResponse.Append("</td>");
                sbResponse.Append("</tr>");
                sbResponse.Append("<tr>");
                sbResponse.Append("<td colspan='2'>");
                sbResponse.Append("<table width='100%' style='border-bottom:1px solid #ccc; font-size:15px;' >");
                sbResponse.Append("<tr>");
                sbResponse.Append("<td align='right'>");
                sbResponse.Append("<span style='font-size:13px;'> Amount In Word : </span> <span> INR " + Amount + " </span>");
                sbResponse.Append("</td>");
                sbResponse.Append("</tr>");
                sbResponse.Append("</table>");
                sbResponse.Append("</td>");
                sbResponse.Append("</tr>");
                sbResponse.Append("</table>");
                sbResponse.Append("</td>");
                sbResponse.Append("</tr>");
                sbResponse.Append("<tr>");
                sbResponse.Append("<td colspan='3' style='padding-top: 0px;'>");
                sbResponse.Append("<table style='font-size:15px;' width='100%' >");
                sbResponse.Append("<tr>");
                sbResponse.Append("<td>E.& O.E.</td>");
                sbResponse.Append("<td align='right'>For " + companyNm + "</td>");
                sbResponse.Append("</tr>");
                sbResponse.Append("<tr>");
                sbResponse.Append("<td colspan='2'><strong>Computer Generated Report. Requires No Signature.</strong></td>");
                sbResponse.Append("</tr>");
                sbResponse.Append("</table>");
                sbResponse.Append("</td>");
                sbResponse.Append("</tr>");
                sbResponse.Append("</tbody>");
                sbResponse.Append("</table>");
                sbResponse.Append("</td>");
                sbResponse.Append("</tr>");
                sbResponse.Append("</tbody>");
                sbResponse.Append("</table>");
                sbResponse.Append("</td>");
                sbResponse.Append("</tr>");
                sbResponse.Append("</tbody>");
                sbResponse.Append("</table>");

            }
            catch (Exception ex)
            {
            }
            return sbResponse;
        }

        public string PrintInvoicePdf(BookingDetailData bookingDetail, int iBookingRef)

        {

            StringBuilder sbResponse = new StringBuilder();


            try
            {
                var flightDetails = bookingDetail.CompanyFlightDetailAirlines;
                var flightSegDetails = bookingDetail.CompanyFlightSegmentDetailAirlines;
                var fareDetails = bookingDetail.CompanyFareDetailAirlines?.FirstOrDefault();
                var fareDetailSeg = bookingDetail.CompanyFareDetailSegmentAirlines;
                var paxDetails = bookingDetail.CompanyPaxDetailAirlines;
                var compDetails = bookingDetail.CompanyFlightDetails?.FirstOrDefault();
                var paxSegmentDetails = bookingDetail.CompanyPaxSegmentDetailAirlines;
                var paxSegmentRuleDetails = bookingDetail.CompanyFlightSegmentRuleDetailAirlines;
                var companyTransactionDetail = bookingDetail.CompanyTransactionDetails;
                var bookingAirlineLogForPGs = bookingDetail.BookingAirlineLogForPGs;
                string Trip = "O";
                int TotalAmount = 0;
                string Amount = string.Empty;
                string clsnm = string.Empty;
                string Class1 = string.Empty;
                string Class2 = string.Empty;
                string flightno1 = string.Empty;
                string flightno2 = string.Empty;
                int Totalcommission = 0;
                int basicplustax = 0;
                int totstax = 0;
                int tds = 0;
                int ServiceFee = 0;
                string departdate1 = string.Empty;
                string departdate2 = string.Empty;
                string Gdspnr_O = string.Empty;
                string Gdspnr_I = string.Empty;
                string Airpnr_O = string.Empty;
                string Airpnr_I = string.Empty;
                Int32 TotalServices = 0;

                string companyNm = string.Empty;
                string companyAddress = string.Empty;
                string companyMobile = string.Empty;
                string companyEmail = string.Empty;
                string Postalcode = string.Empty;
                string GSTNO = string.Empty;
                string Pannno = string.Empty;

                string companyNm_1 = string.Empty;
                string companyAddress_1 = string.Empty;
                string companyMobile_1 = string.Empty;
                string companyEmail_1 = string.Empty;
                string Postalcode_1 = string.Empty;
                string GSTNO_1 = string.Empty;
                string Pannno_1 = string.Empty;
                string bookingref = string.Empty;
                string Bookingdate = string.Empty;
                int SurchargeAmount = 0;

                Double dCurrencyValue = 1;
                string Currency = "INR";

                if (companyTransactionDetail != null && companyTransactionDetail.Any() && bookingAirlineLogForPGs != null && bookingAirlineLogForPGs.Any())
                {
                    dCurrencyValue = Convert.ToDouble(bookingAirlineLogForPGs.FirstOrDefault().CurrencyValue.ToString());
                    Currency = bookingAirlineLogForPGs.FirstOrDefault().Currency.ToString().Trim();
                }
                if (companyTransactionDetail?.FirstOrDefault()?.SurchargeAmount != null && decimal.TryParse(companyTransactionDetail.FirstOrDefault().SurchargeAmount.ToString(), out var surchargeAmount))
                {
                    SurchargeAmount = Decimal.ToInt32(surchargeAmount);
                }
                else
                {
                    SurchargeAmount = 0;
                }


                Trip = flightDetails.FirstOrDefault().Trip.ToString();


                Decimal TotalSeat = Convert.ToDecimal(flightDetails.FirstOrDefault().TotalSeat.ToString());
                Decimal TotalMeal = Convert.ToDecimal(flightDetails.FirstOrDefault().TotalMeal.ToString());
                Decimal TotalBaggage = Convert.ToDecimal(flightDetails.FirstOrDefault().TotalBaggage.ToString());
                TotalServices = Convert.ToInt32(TotalSeat + TotalMeal + TotalBaggage);

                string CompanyID = flightDetails.FirstOrDefault().CompanyId.ToString();

                TotalAmount = Convert.ToInt32(
    string.IsNullOrEmpty(flightDetails.FirstOrDefault()?.TotalFare.ToString())
    ? 0
    : Convert.ToDouble(flightDetails.FirstOrDefault()?.TotalFare.ToString())
) - Convert.ToInt32(
    string.IsNullOrEmpty(flightDetails.FirstOrDefault()?.TotalMarkup.ToString())
    ? 0
    : Convert.ToDouble(flightDetails.FirstOrDefault()?.TotalMarkup.ToString())
);

                Totalcommission = Convert.ToInt32(
                    string.IsNullOrEmpty(flightDetails.FirstOrDefault()?.TotalCommission.ToString())
                    ? 0
                    : Convert.ToDouble(flightDetails.FirstOrDefault()?.TotalCommission.ToString())
                );

                basicplustax = Convert.ToInt32(
                    string.IsNullOrEmpty(flightDetails.FirstOrDefault()?.TotalBasic.ToString())
                    ? 0
                    : Convert.ToDouble(flightDetails.FirstOrDefault()?.TotalBasic.ToString())
                ) + Convert.ToInt32(
                    string.IsNullOrEmpty(flightDetails.FirstOrDefault()?.TotalTax.ToString())
                    ? 0
                    : Convert.ToDouble(flightDetails.FirstOrDefault()?.TotalTax.ToString())
                );

                totstax = Convert.ToInt32(
                    string.IsNullOrEmpty(flightDetails.FirstOrDefault()?.TotalServiceTax.ToString())
                    ? 0
                    : Convert.ToDouble(flightDetails.FirstOrDefault()?.TotalServiceTax.ToString())
                );

                tds = Convert.ToInt32(
                    string.IsNullOrEmpty(flightDetails.FirstOrDefault()?.TotalTds.ToString())
                    ? 0
                    : Convert.ToDouble(flightDetails.FirstOrDefault()?.TotalTds.ToString())
                );

                ServiceFee = Convert.ToInt32(
                    string.IsNullOrEmpty(flightDetails.FirstOrDefault()?.TotalServiceFeeDeal.ToString())
                    ? 0
                    : Convert.ToDouble(flightDetails.FirstOrDefault()?.TotalServiceFeeDeal.ToString())
                );

                TotalAmount = (TotalAmount + TotalServices + SurchargeAmount) - Totalcommission;


                companyNm = StringHelper.GetUpper(compDetails.AA_CompanyName.ToString());
                GSTNO = StringHelper.GetUpper(compDetails.AA_Gst.ToString());
                Pannno = StringHelper.GetUpper(compDetails.AA_Pan_No.ToString());
                Postalcode = compDetails.AA_PostalCode.ToString();
                companyAddress = StringHelper.GetALLFirstCharacterCapital(compDetails.AA_Address.ToString());
                companyMobile = compDetails.AA_Mobile.ToString() + "/";
                companyMobile += compDetails.AA_PhoneNo.ToString();
                companyEmail = StringHelper.GetLower(compDetails.AA_Email.ToString());


                if (CompanyID.IndexOf("-SA-") != -1 || CompanyID.IndexOf("C-") != -1)
                {
                    companyNm_1 = StringHelper.GetUpper(compDetails.A_CompanyName.ToString());
                    GSTNO_1 = StringHelper.GetUpper(compDetails.A_Gst.ToString());
                    Pannno_1 = StringHelper.GetUpper(compDetails.A_Pan_No.ToString());
                    Postalcode_1 = compDetails.A_PostalCode.ToString();
                    companyAddress_1 = StringHelper.GetALLFirstCharacterCapital(compDetails.A_Address.ToString());
                    companyMobile_1 = compDetails.A_Mobile.ToString() + "/";
                    companyMobile_1 += compDetails.A_PhoneNo.ToString();
                    companyEmail_1 = StringHelper.GetLower(compDetails.A_Email.ToString());
                }
                else
                {
                    companyNm_1 = StringHelper.GetUpper(compDetails.CompanyName.ToString());
                    GSTNO_1 = StringHelper.GetUpper(compDetails.Gst.ToString());
                    Pannno_1 = StringHelper.GetUpper(compDetails.Pan_No.ToString());
                    Postalcode_1 = compDetails.PostalCode.ToString();
                    companyAddress_1 = StringHelper.GetALLFirstCharacterCapital(compDetails.Address.ToString().ToLower());
                    companyMobile_1 = compDetails.Mobile.ToString() + "/";
                    companyMobile_1 += compDetails.PhoneNo.ToString();
                    companyEmail_1 = StringHelper.GetLower(compDetails.Email.ToString().ToLower());
                }


                Amount = StringHelper.NumberToText(TotalAmount, false);



                string Airline_PNR_D = flightDetails.FirstOrDefault().Airline_Pnr_D.ToString();
                string Airline_PNR_A = flightDetails.FirstOrDefault().Airline_Pnr_A.ToString();
                string GDS_PNR_D = flightDetails.FirstOrDefault().Gds_Pnr_D.ToString();
                string GDS_PNR_A = flightDetails.FirstOrDefault().Gds_Pnr_A.ToString();
                string Trip11 = flightDetails.FirstOrDefault().Trip.ToString();
                Gdspnr_O = GDS_PNR_D;
                Gdspnr_I = string.Empty;
                if (GDS_PNR_A.Length > 0 && Trip11.Equals("R"))
                {
                    Gdspnr_I = GDS_PNR_A;
                }
                Airpnr_O = Airline_PNR_D;
                Airpnr_I = string.Empty;
                if (Airline_PNR_A.Length > 0 && Trip11.Equals("R"))
                {
                    Airpnr_I = Airline_PNR_A;
                }

                string eventtime = Convert.ToDateTime(flightSegDetails.FirstOrDefault().EventTime.ToString()).ToString("yyyy-MM-dd");
                foreach (var flightDet in flightSegDetails)
                {
                    if (flightDet.ConnOrder.Equals("O1"))
                    {
                        flightno1 = flightDet.FlightNumber.ToString();
                        departdate1 = flightDet.DepartureDate.ToString();
                        break;
                    }
                }
                foreach (var flightDet in flightSegDetails)
                {
                    if (flightDet.ConnOrder.IndexOf("I") != -1)
                    {
                        flightno2 = flightDet.FlightNumber.ToString();
                        departdate2 = flightDet.DepartureDate.ToString();
                        break;
                    }
                }

                foreach (var flightDet in flightSegDetails)
                {
                    if (flightDet.ConnOrder.Equals("O1"))
                    {
                        Class1 = flightDet.ClassOfService.ToString();
                    }
                    else if (flightDet.ConnOrder.IndexOf("I") != -1)
                    {
                        Class2 = flightDet.ClassOfService.ToString();
                    }
                }


                bookingref = flightSegDetails.FirstOrDefault().BookingRef.ToString();
                Bookingdate = Convert.ToDateTime(flightSegDetails.FirstOrDefault().EventTime.ToString()).ToString("yyyy-MM-dd").ToString();




                sbResponse.Append("<table style='width:785px;margin-top:-40px;margin-left:-45px;' >");
                sbResponse.Append("<tbody>");
                sbResponse.Append("<tr>");
                sbResponse.Append("<td>");
                sbResponse.Append("<table align='center' style='border: 1px solid #ccc;width:785px;font-size:15px;font-family:math;' cellspacing='0'  >");
                sbResponse.Append("<tbody>");
                sbResponse.Append("<tr>");
                sbResponse.Append("<td>");
                sbResponse.Append("<table width:785px;' style='font-size:15px;' cellspacing='0' cellpadding='5' border='0' >");
                sbResponse.Append("<tbody>");
                sbResponse.Append("<tr>");
                sbResponse.Append("<td style='font-size:35px;' width='33%' align='center'>   </td>");
                sbResponse.Append("<td style='font-size:35px;' width='33%' align='center'> INVOICE  </td>");
                sbResponse.Append("<td style='font-size:35px;' width='33%' align='center'>   </td>");
                sbResponse.Append("</tr>");
                sbResponse.Append("<tr>");

                sbResponse.Append("<td style=' border-bottom: 1px solid #ccc;' width='33%' align='left'>");
                sbResponse.Append("<span>BookingRef </span>:<span><b style='font-size:16px'> " + bookingref + " </b> </span>");
                sbResponse.Append("</td>");

                sbResponse.Append("<td style='font-size:35px; border-bottom: 1px solid #ccc;' width='33%' align='center'>   </td>");
                sbResponse.Append("<td style='border-bottom: 1px solid #ccc;' width='30%' align='left'>");

                sbResponse.Append("<span> Invoice On  :</span><span> " + Bookingdate + " </span>");
                sbResponse.Append("</td>");

                sbResponse.Append("</tr>");

                sbResponse.Append("<tr>");
                sbResponse.Append("<td colspan='3'>");
                sbResponse.Append("<table width='100%'>");
                sbResponse.Append("<tr>");

                sbResponse.Append("<td width='55%'>");
                sbResponse.Append("<span style='font-weight:600; font-size: 17px;'>To M/s :</span><span style='font-weight:600; font-size: 17px;'>" + companyNm_1 + "</span> <br />");
                sbResponse.Append("<span style='color: #929292;' >Tel No </span>:<span style='color: #929292;' >" + companyMobile_1 + "</span><br />");
                sbResponse.Append("<span style='color: #929292;' >Email </span>:<span style='color: #929292;' >" + companyEmail_1 + "</span><br />");
                sbResponse.Append("<span style='color: #929292;' >Pan No </span>:<span style='color: #929292;' >" + Pannno_1 + "</span><br />");
                sbResponse.Append("<span style='color: #929292;' >GSTIN </span>:<span style='color: #929292;' >" + GSTNO_1 + "</span><br/>");
                sbResponse.Append("<span style='color: #929292;' >" + companyAddress_1 + "</span><span style='color: #929292;' > " + Postalcode_1 + "</span><br />");
                sbResponse.Append("</td>");

                sbResponse.Append("<td width='10%' style='font-size:25px;font-weight:bold;padding:15px 0' valign='top' align='center'>");
                sbResponse.Append("</td>");

                sbResponse.Append("<td align='left' width='35%'>");
                sbResponse.Append("<b style='font-size: 17px;' >" + companyNm + "</b><br />");
                sbResponse.Append("<span style='color: #929292;' >Tel No </span>:<span style='color: #929292;'>" + companyMobile + "</span><br />");
                sbResponse.Append("<span style='color: #929292;' >Email </span>:<span style='color: #929292;'>" + companyEmail + "</span><br />");
                sbResponse.Append("<span style='color: #929292;' >Pan No </span>:<span style='color: #929292;' >" + Pannno + "</span><br />");
                sbResponse.Append("<span style='color: #929292;' >GSTIN </span>:<span style='color: #929292;' >" + GSTNO + "</span> <br/>");
                sbResponse.Append("<span style='color: #929292;' >" + companyAddress + "</span>");
                sbResponse.Append("<span style='color: #929292;' > " + Postalcode + "</span>");
                sbResponse.Append("</td>");

                sbResponse.Append("</tr>");
                sbResponse.Append("</table>");
                sbResponse.Append("</td>");
                sbResponse.Append("</tr>");


                sbResponse.Append("<tr>");
                sbResponse.Append("<td colspan='3'>");
                sbResponse.Append("<table style='font-size:15px;  border-top: 1px solid #d4d4d4;    background: ;' width='100%' cellpadding='10' cellspacing='0' >");
                sbResponse.Append("<tr>");
                sbResponse.Append("<td width='50%'>");
                sbResponse.Append(" <span style='font-size: 15px;   color: #929292;'> <img src='wwwroot\\assets\\img\\airlogo_square\\onwardflight.png' width='20px' /> Onward Flight</span>");
                sbResponse.Append("<span style='font-size: 15px;'> <b>  " + flightDetails.FirstOrDefault().OriginCity.ToString().Trim() + " to  </b> </span> ");
                sbResponse.Append("<span style='font-size: 15px;'> <b>  " + flightDetails.FirstOrDefault().DestinationCity.ToString().Trim() + " </b> </span>");
                sbResponse.Append("</td>");

                sbResponse.Append("<td width='50%' align='right' >");
                sbResponse.Append("<span style='font-size: 15px;'>  Airline PNR  </span>");
                sbResponse.Append("<span style='font-size: 22px;'>  <b> " + flightDetails.FirstOrDefault().Airline_Pnr_D.ToString() + " </b>  </span>");
                sbResponse.Append("</td>");
                sbResponse.Append("</tr>");
                sbResponse.Append("</table>");
                sbResponse.Append("</td>");
                sbResponse.Append("</tr>");
                sbResponse.Append("<tr>");
                sbResponse.Append("<td colspan='3'>");
                sbResponse.Append("<table width='100%' style='font-size:15px;    border: 1px solid #d4d4d4; ' cellpadding='10' cellspacing='0'>");
                sbResponse.Append("<tbody>");
                sbResponse.Append("<tr style=' background: ; font-size:15px; font-weight:bold;' >");
                sbResponse.Append("<td width='5'>  #  </td>");
                sbResponse.Append("<td width='25'>  TicketNo   </td>");
                sbResponse.Append("<td width='25'>   Passengers  </td>");
                sbResponse.Append("<td width='15'>   Sector   </td>");
                sbResponse.Append("<td width='15'>    Flight   </td>");
                sbResponse.Append("<td width='15'>  BFare     </td>");
                sbResponse.Append("<td width='15'>   Taxes    </td>");
                sbResponse.Append("</tr>");

                string PaxType = ""; string PaxName = "";
                foreach (var flightDet in flightDetails)
                {
                    string CarrierName_D = flightDet.CarrierNameD?.ToString() ?? string.Empty;
                    string Carriercode_D = flightDet.CarrierCode_D?.ToString() ?? string.Empty;
                    string sec = flightDet.Sector?.ToString() ?? string.Empty;
                    string Adlt = flightDet.Adt?.ToString() ?? string.Empty;
                    string Chd = flightDet.Chd?.ToString() ?? string.Empty;
                    string Inf = flightDet.Inf?.ToString() ?? string.Empty;
                    string DepartureDate_D = flightDet.DepartureDate_D?.ToString() ?? string.Empty;
                    string Origin = flightDet.Origin?.ToString() ?? string.Empty;
                    string Destination = flightDet.Destination?.ToString() ?? string.Empty;
                    int AdltInt = 0, ChdInt = 0, InfInt = 0;

                    int.TryParse(Adlt, out AdltInt);
                    int.TryParse(Chd, out ChdInt);
                    int.TryParse(Inf, out InfInt);



                    string TotalFare = "0";
                    string BAsic = "0";
                    string Totaltax = "0";

                    string C_TotalFare = "0";
                    string C_BAsic = "0";
                    string C_Totaltax = "0";

                    string I_TotalFare = "0";
                    string I_BAsic = "0";
                    string I_Totaltax = "0";

                    if (Adlt.Any())
                    {
                        foreach (var fareDetail in fareDetailSeg)
                        {
                            if (fareDetail.PaxType.ToString().Trim().Equals("ADT") && fareDetail.Conn.ToString().Trim().Equals("O"))
                            {
                                Decimal basicfare = Convert.ToDecimal(fareDetail.Basic.ToString().Trim());
                                Decimal YQ = Convert.ToDecimal(fareDetail.Yq.ToString().Trim());
                                Decimal Psf = Convert.ToDecimal(fareDetail.Psf.ToString().Trim());
                                Decimal UDF = Convert.ToDecimal(fareDetail.Udf.ToString().Trim());
                                Decimal AUDF = Convert.ToDecimal(fareDetail.Audf.ToString().Trim());
                                Decimal Cute = Convert.ToDecimal(fareDetail.Cute.ToString().Trim());
                                Decimal GST = Convert.ToDecimal(fareDetail.Gst.ToString().Trim());
                                Decimal TF = Convert.ToDecimal(fareDetail.Tf.ToString().Trim());
                                Decimal CESS = Convert.ToDecimal(fareDetail.Cess.ToString().Trim());
                                Decimal EX = Convert.ToDecimal(fareDetail.Ex.ToString().Trim());
                                Decimal MU = Convert.ToDecimal(fareDetail.Markup1.ToString().Trim());


                                TotalFare = (basicfare + YQ + UDF + Psf + AUDF + Cute + GST + TF + CESS + EX).ToString();
                                BAsic = basicfare.ToString();
                                Totaltax = (YQ + UDF + Psf + AUDF + Cute + GST + TF + CESS + EX).ToString();
                            }
                        }
                    }

                    if (Chd.Any())
                    {
                        foreach (var fareDetail in fareDetailSeg)
                        {
                            if (fareDetail.PaxType.ToString().Trim().Equals("CHD") && fareDetail.Conn.ToString().Trim().Equals("O"))
                            {
                                Decimal basicfare = Convert.ToDecimal(fareDetail.Basic.ToString().Trim());
                                Decimal YQ = Convert.ToDecimal(fareDetail.Yq.ToString().Trim());
                                Decimal Psf = Convert.ToDecimal(fareDetail.Psf.ToString().Trim());
                                Decimal UDF = Convert.ToDecimal(fareDetail.Udf.ToString().Trim());
                                Decimal AUDF = Convert.ToDecimal(fareDetail.Audf.ToString().Trim());
                                Decimal Cute = Convert.ToDecimal(fareDetail.Cute.ToString().Trim());
                                Decimal GST = Convert.ToDecimal(fareDetail.Gst.ToString().Trim());
                                Decimal TF = Convert.ToDecimal(fareDetail.Tf.ToString().Trim());
                                Decimal CESS = Convert.ToDecimal(fareDetail.Cess.ToString().Trim());
                                Decimal EX = Convert.ToDecimal(fareDetail.Ex.ToString().Trim());
                                Decimal MU = Convert.ToDecimal(fareDetail.Markup1.ToString().Trim());

                                C_TotalFare = (basicfare + YQ + UDF + Psf + AUDF + Cute + GST + TF + CESS + EX).ToString();
                                C_BAsic = basicfare.ToString();
                                C_Totaltax = (YQ + UDF + Psf + AUDF + Cute + GST + TF + CESS + EX).ToString();
                            }
                        }

                    }

                    if (Inf.Any())
                    {
                        foreach (var fareDetail in fareDetailSeg)
                        {
                            if (fareDetail.PaxType.ToString().Trim().Equals("INF") && fareDetail.Conn.ToString().Trim().Equals("O"))
                            {
                                Decimal basicfare = Convert.ToDecimal(fareDetail.Basic.ToString().Trim());
                                Decimal YQ = Convert.ToDecimal(fareDetail.Yq.ToString().Trim());
                                Decimal Psf = Convert.ToDecimal(fareDetail.Psf.ToString().Trim());
                                Decimal UDF = Convert.ToDecimal(fareDetail.Udf.ToString().Trim());
                                Decimal AUDF = Convert.ToDecimal(fareDetail.Audf.ToString().Trim());
                                Decimal Cute = Convert.ToDecimal(fareDetail.Cute.ToString().Trim());
                                Decimal GST = Convert.ToDecimal(fareDetail.Gst.ToString().Trim());
                                Decimal TF = Convert.ToDecimal(fareDetail.Tf.ToString().Trim());
                                Decimal CESS = Convert.ToDecimal(fareDetail.Cess.ToString().Trim());
                                Decimal EX = Convert.ToDecimal(fareDetail.Ex.ToString().Trim());
                                Decimal MU = Convert.ToDecimal(fareDetail.Markup1.ToString().Trim());

                                I_TotalFare = (basicfare + YQ + UDF + Psf + AUDF + Cute + GST + TF + CESS + EX).ToString();
                                I_BAsic = basicfare.ToString();
                                I_Totaltax = (YQ + UDF + Psf + AUDF + Cute + GST + TF + CESS + EX).ToString();
                            }
                        }

                    }

                    string Sector = Origin + " " + Destination;
                    int totalpax = AdltInt + ChdInt + InfInt;
                    int srno = 1;
                    foreach (var paxDetail in paxDetails)
                    {
                        PaxType = paxDetail.PaxType.ToString();
                        string Fname = paxDetail.First_Name.ToString();
                        string MName = paxDetail.Middle_Name.ToString();
                        string LName = paxDetail.Last_Name.ToString();
                        string ticketno = paxDetail.TicketNo.ToString();
                        PaxName = Fname + " " + MName + " " + LName;

                        sbResponse.Append("<tr>");
                        sbResponse.Append("<td style='border-top: 1px solid rgba(0, 0, 0, 0.17);border-right: 1px solid #ccc;'>");
                        sbResponse.Append("<span>" + srno + "</span>");
                        sbResponse.Append("</td>");
                        sbResponse.Append("<td style='border-top: 1px solid rgba(0, 0, 0, 0.17);border-right: 1px solid #ccc;'>");
                        sbResponse.Append("<span>" + ticketno + "</span>");
                        sbResponse.Append("</td>");
                        sbResponse.Append("<td style='border-top: 1px solid rgba(0, 0, 0, 0.17);border-right: 1px solid #ccc;'>");
                        sbResponse.Append("<span>" + StringHelper.GetUpper(PaxName) + "(" + PaxType + ")</span>");
                        sbResponse.Append("</td>");
                        sbResponse.Append("<td style='border-top: 1px solid rgba(0, 0, 0, 0.17);border-right: 1px solid #ccc;'>");
                        sbResponse.Append("<span> " + Sector + "</span>");
                        sbResponse.Append("</td>");
                        sbResponse.Append("<td style='border-top: 1px solid rgba(0, 0, 0, 0.17);border-right: 1px solid #ccc;'>");
                        sbResponse.Append("<span>" + Carriercode_D + "-" + flightno1 + "</span>");
                        sbResponse.Append("</td>");

                        if (PaxType == "ADT")
                        {
                            sbResponse.Append("<td style='border-top: 1px solid rgba(0, 0, 0, 0.17);border-right: 1px solid #ccc;'>");
                            sbResponse.Append("<span>" + BAsic + "</span>");
                            sbResponse.Append("</td>");
                            sbResponse.Append("<td style='border-top: 1px solid rgba(0, 0, 0, 0.17);border-right: 1px solid #ccc;'>");
                            sbResponse.Append("<span>" + Totaltax + "</span>");
                            sbResponse.Append("</td>");
                        }
                        if (PaxType == "CHD")
                        {
                            sbResponse.Append("<td style='border-top: 1px solid rgba(0, 0, 0, 0.17);border-right: 1px solid #ccc;'>");
                            sbResponse.Append("<span>" + C_BAsic + "</span>");
                            sbResponse.Append("</td>");
                            sbResponse.Append("<td style='border-top: 1px solid rgba(0, 0, 0, 0.17);border-right: 1px solid #ccc;'>");
                            sbResponse.Append("<span>" + C_Totaltax + "</span>");
                            sbResponse.Append("</td>");
                        }
                        if (PaxType == "INF")
                        {
                            sbResponse.Append("<td style='border-top: 1px solid rgba(0, 0, 0, 0.17);border-right: 1px solid #ccc;'>");
                            sbResponse.Append("<span>" + I_BAsic + "</span>");
                            sbResponse.Append("</td>");
                            sbResponse.Append("<td style='border-top: 1px solid rgba(0, 0, 0, 0.17);border-right: 1px solid #ccc;'>");
                            sbResponse.Append("<span>" + I_Totaltax + "</span>");
                            sbResponse.Append("</td>");
                        }
                        srno++;
                    }
                }



                sbResponse.Append("</tbody>");

                sbResponse.Append("</table>");
                sbResponse.Append("</td>");
                sbResponse.Append("</tr>");



                // onward end
                if (Trip == "R")
                {

                    sbResponse.Append("<tr>");
                    sbResponse.Append("<td colspan='3'>");
                    sbResponse.Append("<table style='font-size:15px;   background: ;' width='100%' cellpadding='10' cellspacing='0' >");
                    sbResponse.Append("<tr>");
                    sbResponse.Append("<td width='50%'>");
                    sbResponse.Append(" <span style='font-size: 15px; color: #929292;'> <img src='wwwroot\\assets\\img\\airlogo_square\\Inwardflight.png' width='20px' /> Return Flight </span>");
                    sbResponse.Append("<span style='font-size: 15px;'> <b> " + flightDetails.FirstOrDefault().DestinationCity.ToString().Trim() + " to  </b> </span> ");
                    sbResponse.Append("<span style='font-size: 15px;'> <b> " + flightDetails.FirstOrDefault().OriginCity.ToString().Trim() + "  </b> </span>");
                    sbResponse.Append("</td>");

                    sbResponse.Append("<td width='50%' align='right' >");
                    sbResponse.Append("<span style='font-size: 15px;'>  Airline PNR  </span>");
                    sbResponse.Append("<span style='font-size: 22px;'>  <b>" + Airpnr_I + " </b>  </span>");
                    sbResponse.Append("</td>");
                    sbResponse.Append("</tr>");
                    sbResponse.Append("</table>");
                    sbResponse.Append("</td>");
                    sbResponse.Append("</tr>");

                    sbResponse.Append("<tr>");
                    sbResponse.Append("<td colspan='3'>");
                    sbResponse.Append("<table width='100%' style='font-size:15px;    border: 1px solid #d4d4d4; ' cellpadding='10' cellspacing='0'>");
                    sbResponse.Append("<tbody>");
                    sbResponse.Append("<tr style=' background: ; font-size:15px; font-weight:bold;' >");
                    sbResponse.Append("<td width='5'>  #  </td>");
                    sbResponse.Append(" <td width='25'>  TicketNo   </td>");
                    sbResponse.Append("<td width='25'>   Passengers  </td>");
                    sbResponse.Append("<td width='15'>   Sector   </td>");
                    sbResponse.Append("<td width='15'>    Flight   </td>");
                    sbResponse.Append("<td width='15'>  BFare     </td>");
                    sbResponse.Append("<td width='15'>   Taxes    </td>");
                    sbResponse.Append("</tr>");

                    foreach (var fltDetails in flightDetails)
                    {
                        string CarrierName_A = fltDetails.CarrierName_A.ToString();
                        string Carriercode_A = fltDetails.CarrierCode_A.ToString();
                        string sec = fltDetails.Sector.ToString();
                        string PaxTypeIB = "";
                        string PaxNameIB = "";
                        int Adlt = Convert.ToInt32(fltDetails.Adt);
                        int Chd = Convert.ToInt32(fltDetails.Chd);
                        int Inf = Convert.ToInt32(fltDetails.Inf);
                        string DepartureDate_A = fltDetails.DepartureDate_A.ToString();
                        string Origin = fltDetails.Destination.ToString();
                        string Destination = fltDetails.Origin.ToString();

                        string TotalFare = "0";
                        string BAsic = "0";
                        string Totaltax = "0";

                        string C_TotalFare = "0";
                        string C_BAsic = "0";
                        string C_Totaltax = "0";

                        string I_TotalFare = "0";
                        string I_BAsic = "0";
                        string I_Totaltax = "0";

                        if (Adlt > 0)
                        {
                            foreach (var faredetail in fareDetailSeg)
                            {
                                if (faredetail.PaxType.Trim().Equals("ADT") && faredetail.Conn.Trim().Equals("I"))
                                {
                                    Decimal basicfare = Convert.ToDecimal(faredetail.Basic.ToString().Trim());
                                    Decimal YQ = Convert.ToDecimal(faredetail.Yq.ToString().Trim());
                                    Decimal Psf = Convert.ToDecimal(faredetail.Psf.ToString().Trim());
                                    Decimal UDF = Convert.ToDecimal(faredetail.Udf.ToString().Trim());
                                    Decimal AUDF = Convert.ToDecimal(faredetail.Audf.ToString().Trim());
                                    Decimal Cute = Convert.ToDecimal(faredetail.Cute.ToString().Trim());
                                    Decimal GST = Convert.ToDecimal(faredetail.Gst.ToString().Trim());
                                    Decimal TF = Convert.ToDecimal(faredetail.Tf.ToString().Trim());
                                    Decimal CESS = Convert.ToDecimal(faredetail.Cess.ToString().Trim());
                                    Decimal EX = Convert.ToDecimal(faredetail.Ex.ToString().Trim());

                                    TotalFare = (basicfare + YQ + UDF + Psf + AUDF + Cute + GST + TF + CESS + EX).ToString();
                                    BAsic = basicfare.ToString();
                                    Totaltax = (YQ + UDF + Psf + AUDF + Cute + GST + TF + CESS + EX).ToString();
                                }
                            }

                        }
                        if (Chd > 0)
                        {
                            foreach (var faredetailseg in fareDetailSeg)
                            {
                                if (faredetailseg.PaxType.Trim().Equals("CHD") && faredetailseg.Conn.Trim().Equals("I"))
                                {
                                    Decimal basicfare = Convert.ToDecimal(faredetailseg.Basic.ToString().Trim());
                                    Decimal YQ = Convert.ToDecimal(faredetailseg.Yq.ToString().Trim());
                                    Decimal Psf = Convert.ToDecimal(faredetailseg.Psf.ToString().Trim());
                                    Decimal UDF = Convert.ToDecimal(faredetailseg.Udf.ToString().Trim());
                                    Decimal AUDF = Convert.ToDecimal(faredetailseg.Audf.ToString().Trim());
                                    Decimal Cute = Convert.ToDecimal(faredetailseg.Cute.ToString().Trim());
                                    Decimal GST = Convert.ToDecimal(faredetailseg.Gst.ToString().Trim());
                                    Decimal TF = Convert.ToDecimal(faredetailseg.Tf.ToString().Trim());
                                    Decimal CESS = Convert.ToDecimal(faredetailseg.Cess.ToString().Trim());
                                    Decimal EX = Convert.ToDecimal(faredetailseg.Ex.ToString().Trim());

                                    C_TotalFare = (basicfare + YQ + UDF + Psf + AUDF + Cute + GST + TF + CESS + EX).ToString();
                                    C_BAsic = basicfare.ToString();
                                    C_Totaltax = (YQ + UDF + Psf + AUDF + Cute + GST + TF + CESS + EX).ToString();
                                }
                            }

                        }
                        if (Inf > 0)
                        {
                            foreach (var faredetailseg in fareDetailSeg)
                            {
                                if (faredetailseg.PaxType.Trim().Equals("INF") && faredetailseg.Conn.Trim().Equals("I"))
                                {
                                    Decimal basicfare = Convert.ToDecimal(faredetailseg.Basic.ToString().Trim());
                                    Decimal YQ = Convert.ToDecimal(faredetailseg.Yq.ToString().Trim());
                                    Decimal Psf = Convert.ToDecimal(faredetailseg.Psf.ToString().Trim());
                                    Decimal UDF = Convert.ToDecimal(faredetailseg.Udf.ToString().Trim());
                                    Decimal AUDF = Convert.ToDecimal(faredetailseg.Audf.ToString().Trim());
                                    Decimal Cute = Convert.ToDecimal(faredetailseg.Cute.ToString().Trim());
                                    Decimal GST = Convert.ToDecimal(faredetailseg.Gst.ToString().Trim());
                                    Decimal TF = Convert.ToDecimal(faredetailseg.Tf.ToString().Trim());
                                    Decimal CESS = Convert.ToDecimal(faredetailseg.Cess.ToString().Trim());
                                    Decimal EX = Convert.ToDecimal(faredetailseg.Ex.ToString().Trim());

                                    I_TotalFare = (basicfare + YQ + UDF + Psf + AUDF + Cute + GST + TF + CESS + EX).ToString();
                                    I_BAsic = basicfare.ToString();
                                    I_Totaltax = (YQ + UDF + Psf + AUDF + Cute + GST + TF + CESS + EX).ToString();
                                }
                            }

                        }

                        string Sector = Origin + " " + Destination;
                        int totalpax = Adlt + Chd + Inf;
                        if (totalpax > 0)
                        {
                            int srno = 1;
                            foreach (var paxDetail in paxDetails)
                            {
                                PaxTypeIB = paxDetail.PaxType.ToString();
                                string Fname = paxDetail.First_Name.ToString();
                                string MName = paxDetail.Middle_Name.ToString();
                                string LName = paxDetail.Last_Name.ToString();
                                string ticketno = paxDetail.TicketNo.ToString();
                                PaxNameIB = Fname + " " + MName + " " + LName;
                                sbResponse.Append("<tr>");
                                sbResponse.Append("<td style='border-top: 1px solid rgba(0, 0, 0, 0.17);border-right: 1px solid #ccc;'>");
                                sbResponse.Append("<span>" + srno + "</span>");
                                sbResponse.Append("</td>");
                                sbResponse.Append("<td style='border-top: 1px solid rgba(0, 0, 0, 0.17);border-right: 1px solid #ccc;'>");
                                sbResponse.Append("<span> " + ticketno + "</span>");
                                sbResponse.Append("</td>");
                                sbResponse.Append("<td style='border-top: 1px solid rgba(0, 0, 0, 0.17);border-right: 1px solid #ccc;'>");
                                sbResponse.Append(" <span>" + StringHelper.GetUpper(PaxNameIB) + "(" + PaxTypeIB + ")</span>");
                                sbResponse.Append("</td>");
                                sbResponse.Append("<td style='border-top: 1px solid rgba(0, 0, 0, 0.17);border-right: 1px solid #ccc;'>");
                                sbResponse.Append("<span>" + Sector + "</span>");
                                sbResponse.Append("</td>");
                                sbResponse.Append("<td style='border-top: 1px solid rgba(0, 0, 0, 0.17);border-right: 1px solid #ccc;'>");
                                sbResponse.Append("<span>" + Carriercode_A + "-" + flightno2 + "</span>");
                                sbResponse.Append("</td>");
                                if (PaxTypeIB == "ADT")
                                {
                                    sbResponse.Append("<td style='border-top: 1px solid rgba(0, 0, 0, 0.17);border-right: 1px solid #ccc;'>");
                                    sbResponse.Append("<span>" + BAsic + "</span>");
                                    sbResponse.Append("</td>");
                                    sbResponse.Append("<td style='border-top: 1px solid rgba(0, 0, 0, 0.17);border-right: 1px solid #ccc;'>");
                                    sbResponse.Append("<span> " + Totaltax + "</span>");
                                    sbResponse.Append("</td>");
                                }
                                if (PaxTypeIB == "CHD")
                                {
                                    sbResponse.Append("<td style='border-top: 1px solid rgba(0, 0, 0, 0.17);border-right: 1px solid #ccc;'>");
                                    sbResponse.Append("<span>" + C_BAsic + "</span>");
                                    sbResponse.Append("</td>");
                                    sbResponse.Append("<td style='border-top: 1px solid rgba(0, 0, 0, 0.17);border-right: 1px solid #ccc;'>");
                                    sbResponse.Append("<span>" + C_Totaltax + "</span>");
                                    sbResponse.Append("</td>");
                                }
                                if (PaxTypeIB == "INF")
                                {
                                    sbResponse.Append("<td style='border-top: 1px solid rgba(0, 0, 0, 0.17);border-right: 1px solid #ccc;'>");
                                    sbResponse.Append("<span>" + I_BAsic + "</span>");
                                    sbResponse.Append("</td>");
                                    sbResponse.Append("<td style='border-top: 1px solid rgba(0, 0, 0, 0.17);border-right: 1px solid #ccc;'>");
                                    sbResponse.Append("<span> " + I_Totaltax + "</span>");
                                    sbResponse.Append("</td>");
                                }
                                sbResponse.Append("</tr>");
                                srno++;
                            }
                        }
                        sbResponse.Append("</tbody>");

                        sbResponse.Append("</table>");
                        sbResponse.Append("</td>");
                        sbResponse.Append("</tr>");
                    }
                }


                sbResponse.Append("<tr>");
                sbResponse.Append("<td colspan='3'>");
                sbResponse.Append("<table width='100%'>");
                sbResponse.Append("<tr>");
                sbResponse.Append("<td width='50%' valign='' align='right'>");
                sbResponse.Append("<table style='font-size:15px;' width='100%' align='right' >");
                sbResponse.Append("<tbody>");

                sbResponse.Append("<tr>");
                sbResponse.Append("<td width='76%' ></td> ");
                sbResponse.Append("<td>ADD :</td>");
                sbResponse.Append("<td>Services</td>");
                sbResponse.Append("</tr>");
                sbResponse.Append("<tr>");
                sbResponse.Append("<td width='76%' ></td>");
                sbResponse.Append("<td>Less  :</td>");
                sbResponse.Append("<td> Commission</td>");
                sbResponse.Append("</tr>");
                sbResponse.Append("<tr>");
                sbResponse.Append("<td width='76%' ></td>");
                sbResponse.Append("<td>ADD    :</td>");
                sbResponse.Append("<td>  TDS</td>");
                sbResponse.Append("</tr>");
                sbResponse.Append("<tr>");
                sbResponse.Append("<td width='76%' ></td>");
                sbResponse.Append("<td>ADD  :</td>");
                sbResponse.Append("<td> Service Charge</td>");
                sbResponse.Append("</tr>");
                sbResponse.Append("<tr>");
                sbResponse.Append("<td width='76%' ></td>");
                sbResponse.Append("<td>ADD  :</td>");
                sbResponse.Append("<td> GST</td>");
                sbResponse.Append("</tr>");

                sbResponse.Append("<tr>");
                sbResponse.Append("<td width='76%' ></td>");
                sbResponse.Append("<td>ADD  :</td>");
                sbResponse.Append("<td> Conv.fee</td>");
                sbResponse.Append("</tr>");

                sbResponse.Append("</tbody>");
                sbResponse.Append("</table>");
                sbResponse.Append("</td>");

                sbResponse.Append("<td width='15%' valign='top' align='right'>");
                sbResponse.Append("<table style='font-size:15px;'>");
                sbResponse.Append("<tbody>");
                sbResponse.Append("<tr>");
                sbResponse.Append("<td align='right'>Total  :</td>");
                sbResponse.Append("<td>" + basicplustax + "" + Currency + "</td>");
                sbResponse.Append("</tr>");
                sbResponse.Append("<tr>");
                sbResponse.Append("<td align='right'> Total  :  </td>");
                sbResponse.Append("<td align='right'>" + TotalServices + "" + Currency + "</td>");
                sbResponse.Append("</tr>");
                sbResponse.Append("<tr>");
                sbResponse.Append("<td align='right'> Total  :  </td>");
                sbResponse.Append("<td align='right'>" + Totalcommission + "" + Currency + "</td>");
                sbResponse.Append("</tr>");
                sbResponse.Append("<tr>");
                sbResponse.Append("<td align='right'> Total  :  </td>");
                sbResponse.Append("<td align='right'>" + tds + "" + Currency + "</td>");
                sbResponse.Append("</tr>");
                sbResponse.Append("<tr>");
                sbResponse.Append("<td align='right'>Total  :  </td>");
                sbResponse.Append("<td align='right'>" + ServiceFee + "" + Currency + "</td>");
                sbResponse.Append("</tr>");
                sbResponse.Append("<tr>");
                sbResponse.Append("<td align='right'> @ 18.00 %:  </td>");
                sbResponse.Append("<td align='right'>" + totstax + "" + Currency + "</td>");
                sbResponse.Append("</tr>");

                sbResponse.Append("<tr>");
                sbResponse.Append("<td align='right'>Total  :  </td>");
                sbResponse.Append("<td align='right'>" + SurchargeAmount + "" + Currency + "</td>");
                sbResponse.Append("</tr>");


                sbResponse.Append("<tr>");
                sbResponse.Append("<td align='right'><b> Total Amount </b> </td>");
                sbResponse.Append("<td align='right'>" + TotalAmount + "" + Currency + "</td>");
                sbResponse.Append("</tr>");
                sbResponse.Append("</tbody>");
                sbResponse.Append("</table>");
                sbResponse.Append("</td>");
                sbResponse.Append("</tr>");
                sbResponse.Append("<tr>");
                sbResponse.Append("<td colspan='2'>");
                sbResponse.Append("<table width='100%' style='border-bottom:1px solid #ccc; font-size:15px;' >");
                sbResponse.Append("<tr>");
                sbResponse.Append("<td align='right'>");
                sbResponse.Append("<span style='font-size:13px;'> Amount In Word : </span> <span> INR " + Amount + " </span>");
                sbResponse.Append("</td>");
                sbResponse.Append("</tr>");
                sbResponse.Append("</table>");
                sbResponse.Append("</td>");
                sbResponse.Append("</tr>");
                sbResponse.Append("</table>");
                sbResponse.Append("</td>");
                sbResponse.Append("</tr>");
                sbResponse.Append("<tr>");
                sbResponse.Append("<td colspan='3' style='padding-top: 0px;'>");
                sbResponse.Append("<table style='font-size:15px;' width='100%' >");
                sbResponse.Append("<tr>");
                sbResponse.Append("<td>E.& O.E.</td>");
                sbResponse.Append("<td align='right'>For " + companyNm + "</td>");
                sbResponse.Append("</tr>");
                sbResponse.Append("<tr>");
                sbResponse.Append("<td colspan='2'><strong>Computer Generated Report. Requires No Signature.</strong></td>");
                sbResponse.Append("</tr>");

                sbResponse.Append("</table>");
                sbResponse.Append("</td>");
                sbResponse.Append("</tr>");
                sbResponse.Append("</tbody>");
                sbResponse.Append("</table>");
                sbResponse.Append("</td>");
                sbResponse.Append("</tr>");
                sbResponse.Append("</tbody>");
                sbResponse.Append("</table>");
                sbResponse.Append("</td>");
                sbResponse.Append("</tr>");
                sbResponse.Append("</tbody>");
                sbResponse.Append("</table>");

            }
            catch (Exception ex)
            {
            }
            return sbResponse.ToString();
        }

        [Route("PrintPaxPopup")]
        public async Task<IActionResult> PrintPassengerPopup([FromQuery] string bookingRef, [FromQuery] string paxSegmentID)
        {
            if (!string.IsNullOrEmpty(bookingRef) && bookingRef.Trim().Length > 0)
            {
                string bookingRefDecoded = EncodeDecodeHelper.DecodeFrom64(bookingRef.Trim());
                int iBookingRef = 0;

                bool parseSuccess = int.TryParse(bookingRefDecoded, out iBookingRef);

                if (!parseSuccess || iBookingRef <= 0)
                {
                    ViewBag.ErrorMessage = "Invalid booking reference. Please check the provided reference and try again.";
                    return View();
                }


                int iPaxSegmentID = 0;
                if (!string.IsNullOrEmpty(paxSegmentID))
                {
                    string paxSegmentDecoded = EncodeDecodeHelper.DecodeFrom64(paxSegmentID.Trim());
                    bool parsePaxSegmentSuccess = int.TryParse(paxSegmentDecoded, out iPaxSegmentID);

                    if (!parsePaxSegmentSuccess || iPaxSegmentID <= 0)
                    {
                        ViewBag.ErrorMessage = "Invalid passenger segment ID. Please check the provided ID and try again.";
                        return View();
                    }

                    var query = new BookingDetailQuery { BookingRef = iBookingRef };
                    var flightbooking = await _bookingTicketQueryHandler.HandleAsync(query);

                    if (flightbooking == null)
                    {
                        ViewBag.ErrorMessage = "No booking found for the provided reference. Please check the reference and try again.";
                        return View();
                    }

                    StringBuilder reportHTML = GetPassengerTicketPopup(flightbooking, iBookingRef, UserHelper.GetCompanyID(User), iPaxSegmentID);
                    string hostName = _configuration["SiteURL:BasePath"];


                    var viewModel = new PrintPopupResponse
                    {
                        BookingRef = bookingRef,
                        Pax_SegmentId = paxSegmentID,
                        HdnhostName = hostName,
                        ReportTableHTML = !string.IsNullOrEmpty(reportHTML.ToString()) ? reportHTML.ToString() : string.Empty,
                        ShowLogo = true,
                        HideFare = false
                    };

                    bool IsSingleFare = false;
                    if (flightbooking.CompanyFlightDetails.FirstOrDefault()?.Trip == "R")
                    {
                        var fareDetails = flightbooking.CompanyFareDetailSegmentAirlines
                            .Where(fare => fare.Conn == "I" && fare.PaxType == "ADT");

                        if (fareDetails.Any())
                        {
                            foreach (var fare in fareDetails)
                            {
                                decimal dCount = Convert.ToDecimal(fare.Yq.ToString().Trim())
                                                 + Convert.ToDecimal(fare.Psf.ToString().Trim())
                                                 + Convert.ToDecimal(fare.Udf.ToString().Trim())
                                                 + Convert.ToDecimal(fare.Audf.ToString().Trim())
                                                 + Convert.ToDecimal(fare.Cute.ToString().Trim())
                                                 + Convert.ToDecimal(fare.Gst.ToString().Trim())
                                                 + Convert.ToDecimal(fare.Tf.ToString().Trim())
                                                 + Convert.ToDecimal(fare.Cess.ToString().Trim())
                                                 + Convert.ToDecimal(fare.Ex.ToString().Trim())
                                                 + Convert.ToDecimal(fare.Basic.ToString().Trim());

                                if (dCount <= 0)
                                {
                                    IsSingleFare = true;
                                    break;
                                }
                            }
                        }
                    }

                    var firstFlightDetail = flightbooking.CompanyFlightDetails.FirstOrDefault();
                    if (firstFlightDetail != null)
                    {
                        string tripType = firstFlightDetail.Trip?.ToString();
                        if ((tripType == "R" && IsSingleFare) || tripType == "O")
                        {
                            IsSingleFare = true;
                        }
                    }
                    viewModel.IsSingleFare = IsSingleFare;
                    return View("~/Views/Booking/PrintPassengerPopup.cshtml", viewModel);
                }
            }

            ViewBag.ErrorMessage = "Booking reference is required.";
            return View();
        }

        public StringBuilder GetPassengerTicketPopup(BookingDetailData bookingDetail, int iBookingRef, string SessionCompany, int iPaxSegmentID)

        {

            StringBuilder sbResponse = new StringBuilder();


            try
            {
                var flightDetails = bookingDetail.CompanyFlightDetailAirlines;
                var flightSegDetails = bookingDetail.CompanyFlightSegmentDetailAirlines;
                var fareDetails = bookingDetail.CompanyFareDetailAirlines?.FirstOrDefault();
                var fareDetailAirline = bookingDetail.CompanyFareDetailAirlines;
                var fareDetailSeg = bookingDetail.CompanyFareDetailSegmentAirlines;
                var paxDetails = bookingDetail.CompanyPaxDetailAirlines;
                var compDetails = bookingDetail.CompanyFlightDetails?.FirstOrDefault();
                var paxSegmentDetails = bookingDetail.CompanyPaxSegmentDetailAirlines;
                var paxSegmentRuleDetails = bookingDetail.CompanyFlightSegmentRuleDetailAirlines;
                string IssueON = string.Empty;
                string Originn = string.Empty;
                string Destinationn = string.Empty;
                string Adt = string.Empty;
                string Chd = string.Empty;
                string Inf = string.Empty;
                string Trip = string.Empty;
                int SurchargeAmount = 0;
                string PaxType = "";
                Double dCurrencyValue = 1;
                string Currency = "INR";

                Trip = flightDetails.FirstOrDefault().Trip.ToString().Trim();
                IssueON = StringHelper.GetConvertedDateTime(flightDetails.FirstOrDefault().EventTime.ToString());
                Originn = flightDetails.FirstOrDefault().Origin.ToString().Trim();
                Destinationn = flightDetails.FirstOrDefault().Destination.ToString().Trim();
                Adt = flightDetails.FirstOrDefault().Adt.ToString().Trim();
                Chd = flightDetails.FirstOrDefault().Chd.ToString().Trim();
                Inf = flightDetails.FirstOrDefault().Inf.ToString().Trim();


                string CompanyLogo = string.Empty;
                string CompanyName = string.Empty;
                string Address = string.Empty;
                string Mobile = string.Empty;
                string Email = string.Empty;
                string CompanyID = flightDetails.FirstOrDefault().CompanyId.ToString();

                if (CompanyID.IndexOf("-SA-") != -1 || CompanyID.IndexOf("C-") != -1)
                {
                    if (CompanyID.IndexOf("C-") != -1)
                    {
                        Mobile = compDetails.A_Mobile.ToString();
                        Email = StringHelper.GetLower(compDetails.A_Email.ToString());
                        bool compactiv = Convert.ToBoolean(compDetails.AgencyPrintB2c.ToString());
                        if (compactiv)
                        {
                            CompanyName = StringHelper.GetUpper(compDetails.A_CompanyName.ToString());
                            CompanyLogo = compDetails.A_CompanyLogo.ToString();
                        }
                    }
                    else
                    {
                        CompanyLogo = compDetails.A_CompanyLogo.ToString();
                        CompanyName = StringHelper.GetUpper(compDetails.A_CompanyName.ToString());
                        Address = StringHelper.GetALLFirstCharacterCapital(compDetails.A_Address.ToString());
                        Mobile = compDetails.A_Mobile.ToString();
                        Email = StringHelper.GetLower(compDetails.A_Email.ToString());
                    }
                }
                else
                {
                    CompanyLogo = compDetails.CompanyLogo.ToString();
                    CompanyName = StringHelper.GetUpper(compDetails.CompanyName.ToString());
                    Address = StringHelper.GetALLFirstCharacterCapital(compDetails.Address.ToString());
                    Mobile = compDetails.Mobile.ToString();
                    Email = StringHelper.GetLower(compDetails.Email.ToString());
                }
                sbResponse.Append("<table align='center' style='border: 1px solid #D6D8E7; width: 785px;  font-size: 11px; font-family: math;' cellspacing='3' id='showmndetail' runat='server'>");
                //if (compDetails.Count() > 0)
                //{
                sbResponse.Append("<tr id='logodiv'>");
                sbResponse.Append("<td style='border-bottom: 1px solid #d6d8e7;'>");
                sbResponse.Append("<img id='imgeLogo' src='" + CompanyLogo + "' style='  height: 63px;  width: 218px; '/>");
                sbResponse.Append("</td>");
                sbResponse.Append("</tr>");


                sbResponse.Append("<tr>");
                sbResponse.Append("<td>");
                sbResponse.Append("<table width='100%'  style='border-bottom: 1px solid #D6D8E7;background: #fff;' cellpadding='10' >");
                sbResponse.Append("<tr>");

                sbResponse.Append("<td width='40%'  >");
                sbResponse.Append("<span style='font-size: 15px;   color: #929292;'> <img src='/assets/img/airlogo_square/onwardflight.png' width='20px' /> Onward Flight </span> ");
                sbResponse.Append("<span style='font-size: 15px;'> <b> " + flightDetails.FirstOrDefault().OriginCity.ToString().Trim() + " to </b> </span>");
                sbResponse.Append("<span style='font-size: 15px;'> <b> " + flightDetails.FirstOrDefault().DestinationCity.ToString().Trim() + " </b> </span>");

                sbResponse.Append("</td>");


                sbResponse.Append("<td width='20%'>");
                // sbResponse.Append("<b> E - Ticket </b>");
                sbResponse.Append("</td>");

                sbResponse.Append("<td width='40%' style='font-size: 15px;'>");

                sbResponse.Append("<span style='font-size: 15px;' >Airline PNR:</span><span id='spnpnr' style='font-size: 22px;' ><b> " + flightDetails.FirstOrDefault().Airline_Pnr_D.ToString() + " </b> </span><br />");
                if (flightDetails.FirstOrDefault().Airline_Pnr_D.ToString() != flightDetails.FirstOrDefault().Gds_Pnr_D.ToString())
                {
                    sbResponse.Append("<span style='font-size: 15px;' >GDS PNR:</span><span id='spnpnr' style='font-size: 22px;' ><b> " + flightDetails.FirstOrDefault().Gds_Pnr_D.ToString() + " </b> </span><br />");
                }
                sbResponse.Append("</td>");

                sbResponse.Append("</tr>");
                sbResponse.Append("</table>");
                sbResponse.Append("</td>");
                sbResponse.Append("</tr>");

                if (flightDetails.FirstOrDefault().Trip.ToString().Equals("R"))
                {
                    sbResponse.Append("<tr>");
                    sbResponse.Append("<td style=''>");
                    sbResponse.Append("<table width='100%'  style='border-bottom: 1px solid #D6D8E7;background: #fff;' cellpadding='10' >");
                    sbResponse.Append("<tr>");

                    sbResponse.Append("<td width='40%'  >");
                    sbResponse.Append("<span style='font-size: 15px;   color: #929292;'> <img src='/assets/img/airlogo_square/Inwardflight.png' width='20px' /> Return Flight </span> ");
                    sbResponse.Append("<span style='font-size: 15px;'> <b> " + flightDetails.FirstOrDefault().DestinationCity.ToString().Trim() + " to  </b></span>");
                    sbResponse.Append("<span style='font-size: 15px;'> <b> " + flightDetails.FirstOrDefault().OriginCity.ToString().Trim() + " </b> </span>");
                    sbResponse.Append("</td>");


                    sbResponse.Append("<td width='20%'>");
                    sbResponse.Append("</td>");

                    sbResponse.Append("<td width='40%' style='font-size: 15px;'>");

                    sbResponse.Append("<span style='font-size: 15px;' >Airline PNR:</span><span id='spnpnr' style='font-size: 22px;' ><b> " + flightDetails.FirstOrDefault().Airline_Pnr_A.ToString() + "</b> </span><br />");
                    if (flightDetails.FirstOrDefault().Airline_Pnr_D.ToString() != flightDetails.FirstOrDefault().Gds_Pnr_D.ToString())
                    {
                        sbResponse.Append("<span style='font-size: 15px;' >GDS PNR:</span><span id='spnpnr' style='font-size: 22px;' ><b> " + flightDetails.FirstOrDefault().Gds_Pnr_A.ToString() + "</b> </span><br />");
                    }
                    sbResponse.Append("</td>");

                    sbResponse.Append("</tr>");
                    sbResponse.Append("</table>");
                    sbResponse.Append("</td>");
                    sbResponse.Append("</tr>");
                }


                sbResponse.Append("<tr>");
                sbResponse.Append("<td>");
                sbResponse.Append("<table width='100%' style='border-bottom: 1px solid #D6D8E7;' cellpadding='10' >");
                sbResponse.Append("<tr>");
                sbResponse.Append("<td width='33%' style='font-size:15px;' ><span> Booking Ref. </span> <span id='bookinref'> <b> " + iBookingRef.ToString() + " </b> </span> ");
                sbResponse.Append("</td> <td width='33%' align='center' style='font-size: 25px;' >");
                // sbResponse.Append("<b> E - Ticket </b>");
                sbResponse.Append("</td>");
                sbResponse.Append("<td width='33%' align='right' style='font-size:15px;' ><span>Created On:</span><span id='pnrheadingIssueOn'> " + IssueON + "</span>  ");
                sbResponse.Append("</td>");
                sbResponse.Append("</tr>");
                sbResponse.Append("</table>");
                sbResponse.Append("</td>");
                sbResponse.Append("</tr>");



                sbResponse.Append("<tr>");
                sbResponse.Append("<td>");
                sbResponse.Append("<table width='100%' cellpadding='10'>");
                sbResponse.Append("<tr>");
                sbResponse.Append("<td width='33%' valign='top' style='font-size: 15px;' >");
                sbResponse.Append("<span style='font-size: 17px;' ><b id='txtCompany_Name'>" + CompanyName + "</b></span><br />");

                sbResponse.Append("<span style='color: #929292;' >Phone: <span id='CMobile'>" + Mobile + "</span></span><br />");
                sbResponse.Append("<span style='color: #929292;'>Email: <span id='CEmail'>" + Email.ToLower() + "</span>");
                sbResponse.Append("</span><br />");
                sbResponse.Append("<span id='txtAddressCompany' style='color: #929292;' >" + Address + "");
                sbResponse.Append("</span>");
                sbResponse.Append("</td>");

                sbResponse.Append("<td width='33%' align='center' >");
                sbResponse.Append("<b style='font-size:27px'> E-Ticket  </b>");
                sbResponse.Append("</td>");

                sbResponse.Append("<td width='33%' valign='top' align='left' style='font-size: 15px;' >");
                sbResponse.Append("");

                if (flightDetails.FirstOrDefault().Trip.ToString().Equals("O"))
                {
                    //sbResponse.Append("<span>PNR:</span><span id='spnpnr'> " + flightDetails.FirstOrDefault().Airline_Pnr_D.ToString() + "</span><br />");
                }
                else
                {
                    //sbResponse.Append("<span >PNR:</span><span id='spnpnr'> " + flightDetails.FirstOrDefault().ToString() + "/" + flightDetails.FirstOrDefault().Airline_Pnr_A.ToString() + "</span><br />");
                }

                string strStatus = GetbookingPaxstatus(flightDetails, paxDetails, iPaxSegmentID, fareDetails);

                if (strStatus.Equals("Confirm"))
                {
                    sbResponse.Append("<span style='font-size:18px;'> <img src='/assets/img/airlogo_square/confirmcheck.png' width='22px' /> </span><span id='spnstatus' style='Color:#00e300; font-size: 25px;'> Confirm</span><br />");
                }
                else if (strStatus.Equals("Unconfirm"))
                {
                    sbResponse.Append("<span style='font-size:18px;'> <img src='/assets/img/airlogo_square/unconfirmcross.png' width='22px' /> </span><span id='spnstatus' style='Color:Red; font-size: 25px;'> Unconfirm</span><br />");
                }
                else if (strStatus.Equals("On Hold"))
                {
                    sbResponse.Append("<span style='font-size:18px;'> <img src='/assets/img/airlogo_square/unconfirmcross.png' width='22px' /> </span><span id='spnstatus' style='Color:Red; font-size: 25px;'> On Hold</span><br />");
                }
                else
                {
                    sbResponse.Append("<span style='font-size:18px;'> <img src='/assets/img/airlogo_square/unconfirmcross.png' width='22px' /> </span><span id='spnstatus' style='Color:Red; font-size: 25px;'> " + strStatus + "</span><br />");
                }
                if (paxDetails != null && paxDetails.Any())
                {
                    sbResponse.Append("<span style='color: #929292;' >MobileNo:</span><span id='spnpaxmobile' style='color: #929292;' > " + paxDetails.FirstOrDefault().MobileNo.ToString().Trim() + "</span><br />");
                    sbResponse.Append("<span style='color: #929292;' >Email:</span><span id='spanpaxEmail' style='color: #929292;' > " + StringHelper.GetLower(paxDetails.FirstOrDefault().Email.ToString().Trim().ToLower()) + "</span>");
                }

                sbResponse.Append("</td>");
                sbResponse.Append("</tr>");
                sbResponse.Append("</table>");
                sbResponse.Append("</td>");
                sbResponse.Append("</tr>");


                sbResponse.Append("<tr>");
                sbResponse.Append("<td>");
                sbResponse.Append("<table width='100%' style='border: 1px solid #D6D8E7;' cellspacing='0' cellpadding='10'>");
                sbResponse.Append("<tr style='font-size:15px;'>");
                sbResponse.Append("<td width='34%' style='border-bottom: 1px solid #D6D8E7; font-size:15px;' > <b> Passengers </b> </td>");
                sbResponse.Append("<td width='27%' style='border-bottom: 1px solid #D6D8E7; font-size:15px;' > <b> Ticket Number </b> </td>");
                sbResponse.Append("<td width='20%' style='border-bottom: 1px solid #D6D8E7; font-size:15px;' > <b> Frequent flyer no. </b> </td>");
                sbResponse.Append("<td width='16%' style='border-bottom: 1px solid #D6D8E7; font-size:15px;' align='center' > <b> Services </b> </td>");
                sbResponse.Append("</tr>");


                if (paxDetails != null && paxDetails.Any())
                {
                    var paxSegment = paxDetails.Where(p => p.Pax_SegmentId == iPaxSegmentID).FirstOrDefault();

                    if (paxSegment != null)
                    {
                        string dob = "--";
                        string fnn = "--";
                        string Meal = string.Empty;
                        string Bagg = string.Empty;

                        string fullname = StringHelper.GetUpper(
                            paxSegment?.Title?.ToString().Trim() + " " +
                            paxSegment?.First_Name?.ToString().Trim() + " " +
                            paxSegment?.Middle_Name?.ToString().Trim() + " " +
                            paxSegment?.Last_Name?.ToString().Trim()
                        );

                        string tktno = "--";
                        if (!string.IsNullOrWhiteSpace(paxSegment?.TicketNo?.ToString().Trim()))
                        {
                            tktno = paxSegment?.TicketNo?.ToString().Trim();
                        }

                        if (!string.IsNullOrWhiteSpace(paxSegment?.Ffn?.ToString().Trim()))
                        {
                            fnn = paxSegment?.Ffn?.ToString().Trim();
                        }

                        if (!string.IsNullOrWhiteSpace(paxSegment?.Pax_SegmentId?.ToString().Trim()))
                        {
                            string paxsegid = paxSegment?.Pax_SegmentId?.ToString().Trim();
                            Meal = GetMealDetails(paxsegid, paxSegmentDetails);
                            Bagg = GetBaggDetails(paxsegid, paxSegmentDetails);
                        }

                        if (!string.IsNullOrWhiteSpace(paxSegment?.PaxType?.ToString().Trim()))
                        {
                            PaxType = paxSegment?.PaxType?.ToString().Trim();
                        }

                        if (!string.IsNullOrWhiteSpace(paxSegment?.Dob?.ToString().Trim()))
                        {
                            dob = paxSegment?.Dob?.ToString().Trim();
                        }

                        sbResponse.Append("<tr>");
                        sbResponse.Append("<td style='font-size:15px;' >" + fullname + "</td>");
                        sbResponse.Append("<td style='font-size:15px;' >" + tktno + "</td>");
                        sbResponse.Append("<td style='font-size:15px;' >" + fnn + "</td>");
                        sbResponse.Append("<td style='font-size:15px;' >");
                        if (Bagg != "")
                        {
                            sbResponse.Append("<span><img src='/assets/img/Commonimages/baggage.png' title='Baggage' style='width: 16px;' /></span>");
                        }
                        else
                        {
                            sbResponse.Append("<span>--</span>");
                        }
                        if (Meal != "")
                        {
                            sbResponse.Append("<span>  <img src='/assets/img/Commonimages/meal.png' style='width: 19px;' title='Meal' /></span>");
                        }
                        else
                        {
                            sbResponse.Append("<span>--</span>");
                        }
                        sbResponse.Append("</td>");
                        sbResponse.Append("</tr>");
                    }
                }
                sbResponse.Append("</table>");
                sbResponse.Append("</td>");
                sbResponse.Append("</tr>");
                sbResponse.Append("<tr>");
                sbResponse.Append("<td>" + GetSectorWiseDetailsOB(flightDetails, flightSegDetails));
                sbResponse.Append("</td>");
                sbResponse.Append("</tr>");

                if (Trip == "R")
                {
                    sbResponse.Append("<tr>");
                    sbResponse.Append("<td>" + GetSectorWiseDetailsIB(flightDetails, flightSegDetails));
                    sbResponse.Append("</td>");
                    sbResponse.Append("</tr>");
                }

                bool IsSingleFare = false;
                if (Trip == "R")
                {
                    var drFareType = fareDetailSeg.Where(f => f.Conn == "I" && f.PaxType == "ADT"); ;
                    if (drFareType.Any())
                    {
                        Decimal dCount = 0;
                        foreach (var dr in drFareType)
                        {
                            dCount = Convert.ToDecimal(dr.Yq.ToString().Trim());
                            dCount += Convert.ToDecimal(dr.Psf.ToString().Trim());
                            dCount += Convert.ToDecimal(dr.Udf.ToString().Trim());
                            dCount += Convert.ToDecimal(dr.Audf.ToString().Trim());
                            dCount += Convert.ToDecimal(dr.Cute.ToString().Trim());
                            dCount += Convert.ToDecimal(dr.Gst.ToString().Trim());
                            dCount += Convert.ToDecimal(dr.Tf.ToString().Trim());
                            dCount += Convert.ToDecimal(dr.Cess.ToString().Trim());
                            dCount += Convert.ToDecimal(dr.Ex.ToString().Trim());
                            dCount += Convert.ToDecimal(dr.Basic.ToString().Trim());

                            if (dCount <= 0)
                            {
                                IsSingleFare = true;

                                break;
                            }
                        }
                    }
                }



                //============Fare Breakup Start==============//
                #region
                sbResponse.Append("<tr id='divfarebreakup' style='width: 100%; display:none'>");
                sbResponse.Append("<td>");
                sbResponse.Append("<table width='100%' cellpadding='10' >");
                sbResponse.Append("<tr>");
                sbResponse.Append("<td>");
                sbResponse.Append("</td>");
                sbResponse.Append("</tr>");
                #endregion
                //============Fare Breakup End==============//

                sbResponse.Append("</table>");

                sbResponse.Append("</td>");
                sbResponse.Append("</tr>");

                //======Fare Detail partr start=============//
                #region
                sbResponse.Append("<tr id='divfaredetail' style='width: 100%; font-size:15px;'>");
                sbResponse.Append("<td>");

                sbResponse.Append("<table width='100%' style='' cellpadding='10' >");
                sbResponse.Append("<tr style='font-size:15px;'>");

                sbResponse.Append("<td width='73%' style='border: 1px solid #D6D8E7;'> <b> Service Information </b>");
                sbResponse.Append("</td>");

                sbResponse.Append("<td style='border: 1px solid #D6D8E7;'> <b> Fare Information  </b>");
                sbResponse.Append("</td>");

                sbResponse.Append("</tr>");
                sbResponse.Append("</table>");

                sbResponse.Append("<table>");
                sbResponse.Append("<tr>");
                sbResponse.Append("<td width='45%' valign='top' align='left' style='font-size:14px;'>");
                sbResponse.Append("<table width='100%' >");
                sbResponse.Append("<tbody>");


                if (paxSegmentDetails != null && paxSegmentDetails.Any())
                {
                    foreach (var paxSegment in paxSegmentDetails)
                    {
                        if (paxSegment.Conn != null && paxSegment.Conn.Equals("O"))
                        {
                            string maealbagagge = paxSegment.ChargeType.Equals("M") ? "Meal" : "Baggage";
                            sbResponse.Append("<tr>");
                            sbResponse.Append("<td><b>* </b>" + Originn + "-" + Destinationn + " added " + maealbagagge + "<span>&nbsp;" + paxSegment.ChargeDescription + "</span>  <span>" + paxSegment.Charge_Amount + "Rs.</span></td>");
                            sbResponse.Append("</tr>");
                        }
                        else
                        {
                            string maealbagagge = paxSegment.ChargeType.Equals("M") ? "Meal" : "Baggage";
                            sbResponse.Append("<tr>");
                            sbResponse.Append("<td><b>* </b>" + Destinationn + "-" + Originn + " added " + maealbagagge + "<span>&nbsp;" + paxSegment.ChargeDescription + "</span>  <span>" + paxSegment.Charge_Amount + "Rs.</span></td>");
                            sbResponse.Append("</tr>");
                        }
                    }
                }




                if (paxSegmentRuleDetails != null && paxSegmentRuleDetails.Any())
                {
                    foreach (var paxSegment in paxSegmentRuleDetails)
                    {
                        if (paxSegment.Conn != null && paxSegment.Conn == "O")
                        {
                            sbResponse.Append("<tr>");
                            if (paxSegment.BaggageDetail != null && paxSegment.BaggageDetail.IndexOf("*") != -1)
                            {
                                string bagg = paxSegment.BaggageDetail;
                                sbResponse.Append("<td><b>* </b>" + Originn + "-" + Destinationn + " available baggage on per pax  <span>" + bagg.Split('*')[0].Trim() + " Check IN </span>  </td>");
                            }
                            else
                            {
                                sbResponse.Append("<td><b>* </b>" + Originn + "-" + Destinationn + " available baggage on per pax  <span>Check IN: APAP </span>  </td>");
                            }
                            sbResponse.Append("</tr>");
                        }
                        else
                        {
                            sbResponse.Append("<tr>");
                            if (paxSegment.BaggageDetail != null && paxSegment.BaggageDetail.IndexOf("*") != -1)
                            {
                                string bagg = paxSegment.BaggageDetail;
                                sbResponse.Append("<td><b>* </b>" + Destinationn + "-" + Originn + " available baggage on per pax  <span>" + bagg.Split('*')[0].Trim() + " Check IN </span>  </td>");
                            }
                            else
                            {
                                sbResponse.Append("<td><b>* </b>" + Destinationn + "-" + Originn + " available baggage on per pax  <span>Check IN: APAP </span>  </td>");
                            }
                            sbResponse.Append("</tr>");
                        }
                    }
                }

                sbResponse.Append("<tr>");
                sbResponse.Append("<td>This is an Electronic ticket. Please carry a positive identification for Check in.</td>");
                sbResponse.Append("</tr>");

                sbResponse.Append("</tbody>");
                sbResponse.Append("</table>");
                sbResponse.Append("</td>");

                sbResponse.Append("<td width='28%' valign='top' align='right'>");

                if (flightDetails != null && flightDetails.Any())
                {
                    string adt = flightDetails.FirstOrDefault().Adt.ToString().Trim();
                    string chd = flightDetails.FirstOrDefault().Chd.ToString().Trim();
                    string inf = flightDetails.FirstOrDefault().Inf.ToString().Trim();

                    int TotalServiceFee = 0;
                    int TotalServiceTax = 0;
                    int TotalMealcharges = 0;
                    int TotalBaggCharges = 0;
                    int TotalFare = 0;
                    int TotalTDS = 0;
                    int TotalBasic = 0;
                    int TotalTax = 0;

                    var drSelectFare = fareDetailSeg.Where(x => x.PaxType == PaxType);
                    if (drSelectFare.Any())
                    {
                        foreach (var drget in drSelectFare)
                        {
                            TotalBasic += Decimal.ToInt32(drget.Basic.Value);

                            TotalTax += Decimal.ToInt32(drget.Yq.Value + drget.Psf.Value + drget.Udf.Value + drget.Audf.Value + drget.Cute.Value + drget.Gst.Value + drget.Tf.Value + drget.Cess.Value + drget.Ex.Value + drget.Markup.Value);

                            TotalMealcharges += Decimal.ToInt32(drget.Meal.Value);
                            TotalBaggCharges += Decimal.ToInt32(drget.Baggage.Value);

                            TotalServiceFee += Decimal.ToInt32(drget.Service_Fee.Value);

                            TotalServiceTax += Decimal.ToInt32(drget.ServiceTax.Value);
                            TotalTDS += Decimal.ToInt32(drget.Tds.Value);
                        }
                        TotalFare += (TotalBasic + TotalTax + TotalMealcharges + TotalBaggCharges + TotalServiceFee + TotalServiceTax + SurchargeAmount);
                    }


                    sbResponse.Append("<table width='100%'>");
                    sbResponse.Append("<tbody>");
                    sbResponse.Append("<tr>");
                    sbResponse.Append("<td align='right'>BFare :</td>");
                    sbResponse.Append("<td align='right'>" + TotalBasic + "" + Currency + "</td>");
                    sbResponse.Append("</tr>");
                    sbResponse.Append("<tr>");
                    sbResponse.Append("<td align='right'>  Taxes :  </td>");
                    sbResponse.Append("<td align='right'> <span id='oldtotaltax'>" + TotalTax + "" + Currency + "</span> </td>");
                    sbResponse.Append("</tr>");
                    sbResponse.Append("<tr>");
                    sbResponse.Append("<td align='right'> Fee & Surcharge :  </td>");
                    sbResponse.Append("<td align='right'>" + (TotalServiceFee + TotalServiceTax).ToString() + "" + Currency + "</td>");
                    sbResponse.Append("</tr>");
                    sbResponse.Append("<tr id='divEditTxnFeeDiv' style='display:none;'>");
                    sbResponse.Append("<td align='right'>Txn Fee:</td>");
                    sbResponse.Append("<td align='right'>Rs. <input type='text' style='width: 80px;' id='CurrentTxnFee' name='CurrentTxnFee' value='0' /></td>");
                    sbResponse.Append("</tr>");
                    sbResponse.Append("<tr id='divApplyDiscount' style='display:none;'>");
                    sbResponse.Append("<td align='right'> Discount(-) :</td>");
                    sbResponse.Append("<td align='right'><input type='text' style='width: 80px;' id='CurrentDiscount' name='CurrentDiscount' value='0' /></td>");
                    sbResponse.Append("</tr>");
                    sbResponse.Append("<tr id='Discountdiv' style='display:none;' align='right' >");
                    sbResponse.Append("<td align='right'> Discount(-) :</td>");
                    sbResponse.Append("<td align='right'>Rs <span id='spndiscount'>0</span></td>");
                    sbResponse.Append("</tr>");
                    sbResponse.Append("<tr>");
                    sbResponse.Append("<td align='right'> Services :  </td>");
                    sbResponse.Append("<td align='right'>" + (TotalMealcharges + TotalBaggCharges).ToString() + "" + Currency + "</td>");
                    sbResponse.Append("</tr>");

                    sbResponse.Append("<tr>");
                    sbResponse.Append("<td align='right'> Conv.fee :  </td>");
                    sbResponse.Append("<td align='right'>" + (SurchargeAmount).ToString() + "" + Currency + "</td>");
                    sbResponse.Append("</tr>");

                    sbResponse.Append("<tr style='font-weight:600;'>");
                    sbResponse.Append("<td align='right'> Total Amount :  </td>");
                    sbResponse.Append("<td align='right'><span id='oldtotalamt'>" + TotalFare + "" + Currency + "</span></td>");
                    sbResponse.Append("</tr>");
                    sbResponse.Append("</tbody>");
                    sbResponse.Append("</table>");
                    sbResponse.Append("</td>");
                    sbResponse.Append("</tr>");
                    sbResponse.Append("</table>");
                    sbResponse.Append("</td>");
                    sbResponse.Append("</tr>");
                }
                #endregion
                //======Fare Detail partr End=============//




                //======Fare Detail partr new start=============//
                #region
                sbResponse.Append("<tr id='divfaredetailnew' style='display:none;'>");
                sbResponse.Append("<td>");

                sbResponse.Append("<table width='100%' style='' cellpadding='10' >");
                sbResponse.Append("<tr style='font-size:15px;'>");

                sbResponse.Append("<td  style='border: 1px solid #D6D8E7;' colspan='2'> <b> Service Information </b>");
                sbResponse.Append("</td>");



                sbResponse.Append("</tr>");
                sbResponse.Append("</table>");

                sbResponse.Append("<table>");
                sbResponse.Append("<tr>");
                sbResponse.Append("<td width='45%' valign='top' align='left' style='font-size:14px;'>");
                sbResponse.Append("<table width='100%' >");
                sbResponse.Append("<tbody>");

                if (paxSegmentDetails.Any())
                {
                    foreach (var paxSegment in paxSegmentDetails)
                    {
                        if (paxSegment.Conn != null && paxSegment.Conn.Equals("O"))
                        {
                            string maealbagagge = "";
                            if (paxSegment.ChargeType.Equals("M"))
                            {
                                maealbagagge = "Meal";
                            }
                            else
                            {
                                maealbagagge = "Baggage";
                            }

                            sbResponse.Append("<tr>");
                            sbResponse.Append("<td><b>* </b>" + Originn + "-" + Destinationn + " added " + maealbagagge + "<span>&nbsp;" + paxSegment.ChargeDescription + "</span>  <span>" + paxSegment.Charge_Amount + "Rs.</span></td>");
                            sbResponse.Append("</tr>");
                        }
                        else
                        {
                            string maealbagagge = "";
                            if (paxSegment.ChargeType.Equals("M"))
                            {
                                maealbagagge = "Meal";
                            }
                            else
                            {
                                maealbagagge = "Baggage";
                            }

                            sbResponse.Append("<tr>");
                            sbResponse.Append("<td><b>* </b>" + Destinationn + "-" + Originn + " added " + maealbagagge + "<span>&nbsp;" + paxSegment.ChargeDescription + "</span>  <span>" + paxSegment.Charge_Amount + "Rs.</span></td>");
                            sbResponse.Append("</tr>");
                        }
                    }
                }

                if (paxSegmentRuleDetails != null && paxSegmentRuleDetails.Any())
                {
                    foreach (var paxSegment in paxSegmentRuleDetails)
                    {
                        if (paxSegment.Conn != null && paxSegment.Conn.Equals("O"))
                        {
                            sbResponse.Append("<tr>");
                            if (paxSegment.BaggageDetail != null && paxSegment.BaggageDetail.IndexOf("*") != -1)
                            {
                                string bagg = paxSegment.BaggageDetail;
                                sbResponse.Append("<td><b>* </b>" + Originn + "-" + Destinationn + " available baggage on per pax  <span>" + bagg.Split('*')[0].Trim() + " Check IN </span>  </td>");
                            }
                            else
                            {
                                sbResponse.Append("<td><b>* </b>" + Originn + "-" + Destinationn + " available baggage on per pax  <span>Check IN: APAP </span>  </td>");
                            }
                            sbResponse.Append("</tr>");
                        }
                        else
                        {
                            sbResponse.Append("<tr>");
                            if (paxSegment.BaggageDetail != null && paxSegment.BaggageDetail.IndexOf("*") != -1)
                            {
                                string bagg = paxSegment.BaggageDetail;
                                sbResponse.Append("<td><b>* </b>" + Destinationn + "-" + Originn + " available baggage on per pax  <span>" + bagg.Split('*')[0].Trim() + " Check IN </span>  </td>");
                            }
                            else
                            {
                                sbResponse.Append("<td><b>* </b>" + Destinationn + "-" + Originn + " available baggage on per pax  <span>Check IN: APAP </span>  </td>");
                            }
                            sbResponse.Append("</tr>");
                        }
                    }
                }

                sbResponse.Append("<tr>");
                sbResponse.Append("<td>This is an Electronic ticket. Please carry a positive identification for Check in.</td>");
                sbResponse.Append("</tr>");
                sbResponse.Append("</tbody>");
                sbResponse.Append("</table>");
                sbResponse.Append("</td>");

                sbResponse.Append("<td width='28%' valign='top' align='right'>");
                sbResponse.Append("</td>");

                sbResponse.Append("</tr>");


                sbResponse.Append("</table>");
                sbResponse.Append("</td>");

                sbResponse.Append("</tr>");
                #endregion
                //======Fare Detail partr new End=============//

                //======GST Detail start=============//
                var gstDetail = bookingDetail.CompanyFlightGSTDetails.FirstOrDefault();
                if (gstDetail != null)
                {

                    sbResponse.Append("<tr>");
                    sbResponse.Append("<td colspan='2'>");
                    sbResponse.Append("<table width='100%' style='border: 1px solid #D6D8E7;' cellspacing='0' cellpadding='10' >");
                    sbResponse.Append("<tr style='font-size:15px;'>");
                    sbResponse.Append("<td width='73%' style=''>GST Detail");
                    sbResponse.Append("</td>");
                    sbResponse.Append("</tr>");
                    sbResponse.Append("</table>");
                    sbResponse.Append("</td>");
                    sbResponse.Append("</tr>");

                    sbResponse.Append("<tr>");
                    sbResponse.Append("<td colspan='2'>");
                    sbResponse.Append("<table width='100%' style='font-size:11px;' cellspacing='0' cellpadding='3' >");
                    sbResponse.Append("<tr>");
                    sbResponse.Append("<td>Gst CompanyName:</td>");
                    sbResponse.Append("<td>" + StringHelper.GetUpper(gstDetail.GstcompanyName.ToString().Trim()) + "</td>");
                    sbResponse.Append("</tr>");
                    sbResponse.Append("<tr>");
                    sbResponse.Append("<td>Gst Number:</td>");
                    sbResponse.Append("<td>" + StringHelper.GetUpper(gstDetail.Gstnumber.ToString().Trim()) + "</td>");
                    sbResponse.Append("</tr>");
                    sbResponse.Append("</table>");
                    sbResponse.Append("</td>");
                    sbResponse.Append("</tr>");
                }
                //======GST Detail End =============//

                sbResponse.Append("<tr>");
                sbResponse.Append("<td colspan='2'>");
                sbResponse.Append("<table width='100%' style='border: 1px solid #D6D8E7;' cellspacing='0' cellpadding='10' >");
                sbResponse.Append("<tr style='font-size:15px;'>");
                sbResponse.Append("<td width='73%' style=''><b> Terms & Conditions </b>");
                sbResponse.Append("</td>");
                sbResponse.Append("</tr>");
                sbResponse.Append("</table>");
                sbResponse.Append("</td>");
                sbResponse.Append("</tr>");

                sbResponse.Append("<tr>");
                sbResponse.Append("<td colspan='2'>");
                sbResponse.Append("<table width='100%' style='font-size:12px;' cellspacing='0' cellpadding='3' >");
                sbResponse.Append("<tr>");
                sbResponse.Append("<td><b>*</b> All Guests, including children and infants, must present valid identification at check-in.");
                sbResponse.Append("</td>");
                sbResponse.Append("</tr>");
                sbResponse.Append("<tr>");
                sbResponse.Append("<td><b>*</b> Carriage and other services provided by the carrier are subject to conditions of carriage, which are hereby incorporated by reference. These conditions may be obtained from the issuing carrier.");
                sbResponse.Append("</td>");
                sbResponse.Append("</tr>");
                sbResponse.Append("<tr>");
                sbResponse.Append("<td><b>*</b> Changes to your reservation will result in a fee of Airline terms & condition,change plus any difference in the fare between the original fare paid and the fare for the revised booking.");
                sbResponse.Append("</td>");
                sbResponse.Append("</tr>");

                sbResponse.Append("<tr>");
                sbResponse.Append("<td><b>*</b> Cancellation charges as per Airlines terms & condition.");
                sbResponse.Append("</td>");
                sbResponse.Append("</tr>");

                sbResponse.Append("<tr>");
                sbResponse.Append("<td><b>*</b> Airline Free Baggages are per seat only.");
                sbResponse.Append("</td>");
                sbResponse.Append("</tr>");


                sbResponse.Append("<tr>");
                sbResponse.Append("<td><b>*</b> Check your itinerary with airline also,revert with in 2 hour. we are here to help you.");
                sbResponse.Append("</td>");
                sbResponse.Append("</tr>");

                sbResponse.Append("<tr>");
                sbResponse.Append("<td><b>*</b> As per Government guidelines, check-in counters at all airports will now close 45 minutes before departure with immediate effect. Please plan your Airport arrival accordingly.");
                sbResponse.Append("</td>");
                sbResponse.Append("</tr>");
                sbResponse.Append("<tr>");
                sbResponse.Append("<td><b>*</b> Wish You a Happy Journey.");
                sbResponse.Append("</td>");
                sbResponse.Append("</tr>");

                sbResponse.Append("</table>");
                sbResponse.Append("</td>");
                sbResponse.Append("</tr>");

                sbResponse.Append("</table>");
                //}
            }
            catch (Exception ex)
            {
                DBLoggerHelper.DBLogAsync("", iBookingRef, "Print_Popup_Tkt", "PrintPopupController", "", "", ex.Message, _addDbLogCommandHandler );
            }
            return sbResponse;
        }

        [HttpPost]
        public async Task<IActionResult> SendPassengerTicketMail([FromBody] PrintPoupEmailRequest printPoupEmailRequest)
        {
            if (!string.IsNullOrWhiteSpace(printPoupEmailRequest.BookingRef) && !string.IsNullOrWhiteSpace(printPoupEmailRequest.Pax_SegmentId) && !string.IsNullOrWhiteSpace(printPoupEmailRequest.Email))
            {
                var bookingRef = printPoupEmailRequest.BookingRef;
                var email = printPoupEmailRequest.Email;
                var paxSegmentID = printPoupEmailRequest.Pax_SegmentId;
                string bookingRefDecoded = EncodeDecodeHelper.DecodeFrom64(bookingRef.Trim());
                string paxSegmentDecoded = EncodeDecodeHelper.DecodeFrom64(paxSegmentID.Trim());

                if (int.TryParse(bookingRefDecoded, out int iBookingRef) && int.TryParse(paxSegmentDecoded, out int iPaxSegmentID))
                {
                    var query = new BookingDetailQuery { BookingRef = iBookingRef };
                    var flightbooking = await _bookingTicketQueryHandler.HandleAsync(query);

                    if (flightbooking != null)
                    {
                        var flightDetails = flightbooking.CompanyFlightDetailAirlines;
                        var paxDetails = flightbooking.CompanyPaxDetailAirlines;
                        var fareDetails = flightbooking.CompanyFareDetailAirlines?.FirstOrDefault();

                        string BookingStatus = GetbookingPaxstatus(flightDetails, paxDetails, iPaxSegmentID, fareDetails).ToUpper();

                        if (BookingStatus.Equals("REJECTED"))
                        {
                            BookingStatus = "REJECT";
                        }

                        string MailType;
                        if (BookingStatus.Equals("CONFIRM"))
                        {
                            MailType = " Confirmation";
                        }
                        else if (BookingStatus.Equals("REJECT"))
                        {
                            MailType = " Reject";
                        }
                        else if (BookingStatus.Equals("UNCONFIRM"))
                        {
                            MailType = " Confirmation";
                        }
                        else if (BookingStatus.Equals("CANCEL"))
                        {
                            MailType = " Cancellation Request";
                        }
                        else if (BookingStatus.Equals("INV"))
                        {
                            MailType = " Invoice";
                        }
                        else if (BookingStatus.Equals("ON HOLD"))
                        {
                            MailType = " On Hold";
                        }
                        else
                        {
                            MailType = " "; // Provide a default fallback text
                        }

                        string emailSubject = MailType + " of BookingRef: " + iBookingRef;
                        string emailBody = $"<html><body><h4>Please find the attached Booking Ref: {iBookingRef} {MailType} </h4></body></html>";

                        string sessionCompany = UserHelper.GetCompanyID(User);

                        string reportHTML = PrintPassengerPopupPdf(flightbooking, iBookingRef, printPoupEmailRequest.ShowLogo, printPoupEmailRequest.HideFare, sessionCompany, iPaxSegmentID, printPoupEmailRequest.Tax);

                        string baseUri = Directory.GetCurrentDirectory();
                        var margins = (Top: 10f, Right: 10f, Bottom: 0f, Left: 10f);

                        var pdfStream = GeneratePDF.GeneratePdfStream(reportHTML, margins, baseUri);

                        byte[] pdfBytes;
                        using (var memoryStream = new MemoryStream())
                        {
                            pdfStream.CopyTo(memoryStream);
                            pdfBytes = memoryStream.ToArray();
                        }
                        bool emailSent = await EmailService.SendEmail(email, emailSubject, emailBody, pdfBytes, $"ETicket_{iBookingRef}.pdf");

                        if (emailSent)
                        {
                            return Json(new { success = true, message = "Email sent successfully." });
                        }
                        else
                        {
                            return Json(new { success = false, message = "Failed to send email. Please try again later." });
                        }
                    }
                    else
                    {
                        return Json(new { success = false, message = "No booking found for the provided reference." });
                    }
                }
                else
                {
                    return Json(new { success = false, message = "Invalid booking reference or passenger segment ID." });
                }
            }
            else
            {
                return Json(new { success = false, message = "Booking reference, passenger segment ID, and email are required." });
            }
        }

        [HttpPost]
        public async Task<IActionResult> PrintPassengerPopupExportToPDF([FromBody] PrintPopupPdfRequest printPopupPdfRequest)
        {
            if (printPopupPdfRequest == null || string.IsNullOrEmpty(printPopupPdfRequest.BookingRef?.Trim()))
            {
                ViewBag.ErrorMessage = "Booking reference is required.";
                return View();
            }

            string bookingRefDecoded = EncodeDecodeHelper.DecodeFrom64(printPopupPdfRequest.BookingRef.Trim());
            if (int.TryParse(bookingRefDecoded, out int iBookingRef) && iBookingRef > 0)
            {
                int iPaxSegmentID = 0;
                if (!string.IsNullOrEmpty(printPopupPdfRequest.Pax_SegmentId))
                {
                    string paxSegmentDecoded = EncodeDecodeHelper.DecodeFrom64(printPopupPdfRequest.Pax_SegmentId.Trim());
                    if (!int.TryParse(paxSegmentDecoded, out iPaxSegmentID) || iPaxSegmentID <= 0)
                    {
                        ViewBag.ErrorMessage = "Invalid passenger segment ID. Please check the provided ID.";
                        return View();
                    }

                    var query = new BookingDetailQuery { BookingRef = iBookingRef };
                    var flightbooking = await _bookingTicketQueryHandler.HandleAsync(query);

                    if (flightbooking != null)
                    {
                        string sessionCompany = UserHelper.GetCompanyID(User);

                        string reportHTML = PrintPassengerPopupPdf(
                            flightbooking,
                            iBookingRef,
                            printPopupPdfRequest.ShowLogo,
                            printPopupPdfRequest.HideFare,
                            sessionCompany,
                            iPaxSegmentID,
                            printPopupPdfRequest.Tax
                        );

                        string baseUri = Directory.GetCurrentDirectory();

                        var margins = (Top: 10f, Right: 10f, Bottom: 0f, Left: 10f);

                        var pdfStream = GeneratePDF.GeneratePdfStream(reportHTML, margins, baseUri);

                        return File(pdfStream, "application/pdf", $"Print_{iBookingRef}.pdf");
                    }
                    else
                    {
                        ViewBag.ErrorMessage = "No booking found for the provided reference.";
                        return View();
                    }
                }
                else
                {
                    ViewBag.ErrorMessage = "Passenger segment ID is required.";
                    return View();
                }
            }
            else
            {
                ViewBag.ErrorMessage = "Invalid booking reference. Please check the provided reference.";
                return View();
            }
        }

        public string PrintPassengerPopupPdf(BookingDetailData bookingDetail, int iBookingRef, string Logo, string HideFare, string SessionCompany, int iPaxSegmentID, string Tax)

        {

            StringBuilder sbResponse = new StringBuilder();


            try
            {
                var flightDetails = bookingDetail.CompanyFlightDetailAirlines;
                var flightSegDetails = bookingDetail.CompanyFlightSegmentDetailAirlines;
                var fareDetails = bookingDetail.CompanyFareDetailAirlines?.FirstOrDefault();
                var fareDetailAirline = bookingDetail.CompanyFareDetailAirlines;
                var fareDetailSeg = bookingDetail.CompanyFareDetailSegmentAirlines;
                var paxDetails = bookingDetail.CompanyPaxDetailAirlines;
                var compDetails = bookingDetail.CompanyFlightDetails?.FirstOrDefault();
                var paxSegmentDetails = bookingDetail.CompanyPaxSegmentDetailAirlines;
                var paxSegmentRuleDetails = bookingDetail.CompanyFlightSegmentRuleDetailAirlines;
                string IssueON = string.Empty;
                string Originn = string.Empty;
                string Destinationn = string.Empty;
                string Adt = string.Empty;
                string Chd = string.Empty;
                string Inf = string.Empty;
                string Trip = string.Empty;
                int SurchargeAmount = 0;

                Double dCurrencyValue = 1;
                string Currency = "INR";

                Trip = flightDetails.FirstOrDefault().Trip.ToString().Trim();
                IssueON = StringHelper.GetConvertedDateTime(flightDetails.FirstOrDefault().EventTime.ToString());
                Originn = flightDetails.FirstOrDefault().Origin.ToString().Trim();
                Destinationn = flightDetails.FirstOrDefault().Destination.ToString().Trim();
                Adt = flightDetails.FirstOrDefault().Adt.ToString().Trim();
                Chd = flightDetails.FirstOrDefault().Chd.ToString().Trim();
                Inf = flightDetails.FirstOrDefault().Inf.ToString().Trim();


                string CompanyLogo = string.Empty;
                string CompanyName = string.Empty;
                string Address = string.Empty;
                string Mobile = string.Empty;
                string Email = string.Empty;
                string CompanyID = flightDetails.FirstOrDefault().CompanyId.ToString();

                if (CompanyID.IndexOf("-SA-") != -1 || CompanyID.IndexOf("C-") != -1)
                {
                    if (CompanyID.IndexOf("C-") != -1)
                    {
                        Mobile = compDetails.A_Mobile.ToString();
                        Email = StringHelper.GetLower(compDetails.A_Email.ToString());
                        bool compactiv = Convert.ToBoolean(compDetails.AgencyPrintB2c.ToString());
                        if (compactiv)
                        {
                            CompanyName = StringHelper.GetUpper(compDetails.A_CompanyName.ToString());
                            CompanyLogo = compDetails.A_CompanyLogo.ToString();
                        }
                    }
                    else
                    {
                        CompanyLogo = compDetails.A_CompanyLogo.ToString();
                        CompanyName = StringHelper.GetUpper(compDetails.A_CompanyName.ToString());
                        Address = StringHelper.GetALLFirstCharacterCapital(compDetails.A_Address.ToString());
                        Mobile = compDetails.A_Mobile.ToString();
                        Email = StringHelper.GetLower(compDetails.A_Email.ToString());
                    }
                }
                else
                {
                    CompanyLogo = compDetails.CompanyLogo.ToString();
                    CompanyName = StringHelper.GetUpper(compDetails.CompanyName.ToString());
                    Address = StringHelper.GetALLFirstCharacterCapital(compDetails.Address.ToString());
                    Mobile = compDetails.Mobile.ToString();
                    Email = StringHelper.GetLower(compDetails.Email.ToString());
                }

                sbResponse.Append("<table align='center' style='margin-left:-45px;margin-top:-40px;border: 1px solid #D6D8E7; width: 785px;  font-size: 11px; font-family: math;' cellspacing='3' id='showmndetail' runat='server'>");
                //if (compDetails.Count() > 0)
                //{
                sbResponse.Append("<tr id='logodiv'>");
                sbResponse.Append("<td style='border-bottom: 1px solid #d6d8e7;'>");
                sbResponse.Append("<img id='imgeLogo' src='" + CompanyLogo + "' style='  height: 63px;  width: 218px; '/>");
                sbResponse.Append("</td>");
                sbResponse.Append("</tr>");


                sbResponse.Append("<tr>");
                sbResponse.Append("<td>");
                sbResponse.Append("<table width='100%'  style='border-bottom: 1px solid #D6D8E7;background: #fff;' cellpadding='10' >");
                sbResponse.Append("<tr>");

                sbResponse.Append("<td width='40%'  >");
                sbResponse.Append("<span style='font-size: 15px;   color: #929292;'> <img src='wwwroot\\assets\\img\\airlogo_square\\onwardflight.png' width='20px' /> Onward Flight test pdf </span> ");
                sbResponse.Append("<span style='font-size: 15px;'> <b> " + flightDetails.FirstOrDefault().OriginCity.ToString().Trim() + " to </b> </span>");
                sbResponse.Append("<span style='font-size: 15px;'> <b> " + flightDetails.FirstOrDefault().DestinationCity.ToString().Trim() + " </b> </span>");

                sbResponse.Append("</td>");


                sbResponse.Append("<td width='20%'>");
                sbResponse.Append("</td>");

                sbResponse.Append("<td width='40%' style='font-size: 15px;'>");

                sbResponse.Append("<span style='font-size: 15px;' >Airline PNR:</span><span id='spnpnr' style='font-size: 22px;' ><b> " + flightDetails.FirstOrDefault().Airline_Pnr_D.ToString() + " </b> </span><br />");

                sbResponse.Append("</td>");

                sbResponse.Append("</tr>");
                sbResponse.Append("</table>");
                sbResponse.Append("</td>");
                sbResponse.Append("</tr>");

                if (flightDetails.FirstOrDefault().Trip.ToString().Equals("R"))
                {
                    sbResponse.Append("<tr>");
                    sbResponse.Append("<td style=''>");
                    sbResponse.Append("<table width='100%'  style='border-bottom: 1px solid #D6D8E7;background: #fff;' cellpadding='10' >");
                    sbResponse.Append("<tr>");

                    sbResponse.Append("<td width='40%'  >");
                    sbResponse.Append("<span style='font-size: 15px;   color: #929292;'> <img src='wwwroot\\assets\\img\\airlogo_square\\Inwardflight.png' width='20px' /> Return Flight </span> ");
                    sbResponse.Append("<span style='font-size: 15px;'> <b> " + flightDetails.FirstOrDefault().DestinationCity.ToString().Trim() + " to  </b></span>");
                    sbResponse.Append("<span style='font-size: 15px;'> <b> " + flightDetails.FirstOrDefault().OriginCity.ToString().Trim() + " </b> </span>");
                    sbResponse.Append("</td>");


                    sbResponse.Append("<td width='20%'>");
                    sbResponse.Append("</td>");

                    sbResponse.Append("<td width='40%' style='font-size: 15px;'>");

                    sbResponse.Append("<span style='font-size: 15px;' >Airline PNR:</span><span id='spnpnr' style='font-size: 22px;' ><b> " + flightDetails.FirstOrDefault().Airline_Pnr_A.ToString() + "</b> </span><br />");

                    sbResponse.Append("</td>");

                    sbResponse.Append("</tr>");
                    sbResponse.Append("</table>");
                    sbResponse.Append("</td>");
                    sbResponse.Append("</tr>");
                }


                sbResponse.Append("<tr>");
                sbResponse.Append("<td>");
                sbResponse.Append("<table width='100%' style='border-bottom: 1px solid #D6D8E7;' cellpadding='10' >");
                sbResponse.Append("<tr>");
                sbResponse.Append("<td width='33%' style='font-size:15px;' ><span> Booking Ref. </span> <span id='bookinref'> <b> " + iBookingRef.ToString() + " </b> </span> ");
                sbResponse.Append("</td> <td width='33%' align='center' style='font-size: 25px;' >");
                sbResponse.Append("</td>");
                sbResponse.Append("<td width='33%' align='right' style='font-size:15px;' ><span>Issued Date:</span><span id='pnrheadingIssueOn'> " + IssueON + "</span>  ");
                sbResponse.Append("</td>");
                sbResponse.Append("</tr>");
                sbResponse.Append("</table>");
                sbResponse.Append("</td>");
                sbResponse.Append("</tr>");



                sbResponse.Append("<tr>");
                sbResponse.Append("<td>");
                sbResponse.Append("<table width='100%' cellpadding='10'>");
                sbResponse.Append("<tr>");
                sbResponse.Append("<td width='33%' valign='top' style='font-size: 15px;' >");
                sbResponse.Append("<span style='font-size: 17px;' ><b id='txtCompany_Name'>" + CompanyName + "</b></span><br />");

                sbResponse.Append("<span style='color: #929292;' >Phone: <span id='CMobile'>" + Mobile + "</span></span><br />");
                sbResponse.Append("<span style='color: #929292;'>Email: <span id='CEmail'>" + Email.ToLower() + "</span>");
                sbResponse.Append("</span><br />");
                sbResponse.Append("<span id='txtAddressCompany' style='color: #929292;' >" + Address + "");
                sbResponse.Append("</span>");
                sbResponse.Append("</td>");

                sbResponse.Append("<td width='33%' align='center' >");
                sbResponse.Append("<b style='font-size:27px'> E-Ticket  </b>");
                sbResponse.Append("</td>");

                sbResponse.Append("<td width='33%' valign='top' align='left' style='font-size: 15px;' >");
                sbResponse.Append("");

                if (flightDetails.FirstOrDefault().Trip.ToString().Equals("O"))
                {
                    //sbResponse.Append("<span>PNR:</span><span id='spnpnr'> " + flightDetails.FirstOrDefault().Airline_Pnr_D.ToString() + "</span><br />");
                }
                else
                {
                    //sbResponse.Append("<span >PNR:</span><span id='spnpnr'> " + flightDetails.FirstOrDefault().ToString() + "/" + flightDetails.FirstOrDefault().Airline_Pnr_A.ToString() + "</span><br />");
                }

                string strStatus = GetbookingPaxstatus(flightDetails, paxDetails, iPaxSegmentID, fareDetails);

                if (strStatus.Equals("Confirm"))
                {
                    sbResponse.Append("<span style='font-size:18px;'> <img src='wwwroot\\assets\\img\\airlogo_square\\confirmcheck.png' width='22px' /> </span><span id='spnstatus' style='Color:#00e300; font-size: 25px;'> Confirm</span><br />");
                }
                else if (strStatus.Equals("Unconfirm"))
                {
                    sbResponse.Append("<span style='font-size:18px;'> <img src='wwwroot\\assets\\img\\airlogo_square\\unconfirmcross.png' width='22px' /> </span><span id='spnstatus' style='Color:Red; font-size: 25px;'> Unconfirm</span><br />");
                }
                else if (strStatus.Equals("On Hold"))
                {
                    sbResponse.Append("<span style='font-size:18px;'> <img src='wwwroot\\assets\\img\\airlogo_square\\unconfirmcross.png' width='22px' /> </span><span id='spnstatus' style='Color:Red; font-size: 25px;'> On Hold</span><br />");
                }
                else
                {
                    sbResponse.Append("<span style='font-size:18px;'> <img src='wwwroot\\assets\\img\\airlogo_square\\unconfirmcross.png' width='22px' /> </span><span id='spnstatus' style='Color:Red; font-size: 25px;'> " + strStatus + "</span><br />");
                }
                if (paxDetails != null && paxDetails.Any())
                {
                    sbResponse.Append("<span style='color: #929292;' >MobileNo:</span><span id='spnpaxmobile' style='color: #929292;' > " + paxDetails.FirstOrDefault().MobileNo.ToString().Trim() + "</span><br />");
                    sbResponse.Append("<span style='color: #929292;' >Email:</span><span id='spanpaxEmail' style='color: #929292;' > " + StringHelper.GetLower(paxDetails.FirstOrDefault().Email.ToString().Trim().ToLower()) + "</span>");
                }

                sbResponse.Append("</td>");
                sbResponse.Append("</tr>");
                sbResponse.Append("</table>");
                sbResponse.Append("</td>");
                sbResponse.Append("</tr>");


                sbResponse.Append("<tr>");
                sbResponse.Append("<td>");
                sbResponse.Append("<table width='100%' style='border: 1px solid #D6D8E7;' cellspacing='0' cellpadding='10'>");
                sbResponse.Append("<tr style='font-size:15px;'>");
                sbResponse.Append("<td width='34%' style='border-bottom: 1px solid #D6D8E7; font-size:15px;' > <b> Passengers </b> </td>");
                sbResponse.Append("<td width='27%' style='border-bottom: 1px solid #D6D8E7; font-size:15px;' > <b> Ticket Number </b> </td>");
                sbResponse.Append("<td width='20%' style='border-bottom: 1px solid #D6D8E7; font-size:15px;' > <b> Frequent flyer no. </b> </td>");
                sbResponse.Append("<td width='16%' style='border-bottom: 1px solid #D6D8E7; font-size:15px;' align='center' > <b> Services </b> </td>");
                sbResponse.Append("</tr>");



                if (paxDetails != null && paxDetails.Any())
                {
                    var paxSegment = paxDetails.Where(p => p.Pax_SegmentId == iPaxSegmentID).FirstOrDefault();

                    if (paxSegment != null)
                    {
                        string dob = "--";
                        string fnn = "--";
                        string Meal = string.Empty;
                        string Bagg = string.Empty;

                        string fullname = StringHelper.GetUpper(
                            paxSegment?.Title?.ToString().Trim() + " " +
                            paxSegment?.First_Name?.ToString().Trim() + " " +
                            paxSegment?.Middle_Name?.ToString().Trim() + " " +
                            paxSegment?.Last_Name?.ToString().Trim()
                        );

                        string tktno = "--";
                        if (!string.IsNullOrWhiteSpace(paxSegment?.TicketNo?.ToString().Trim()))
                        {
                            tktno = paxSegment?.TicketNo?.ToString().Trim();
                        }

                        if (!string.IsNullOrWhiteSpace(paxSegment?.Ffn?.ToString().Trim()))
                        {
                            fnn = paxSegment?.Ffn?.ToString().Trim();
                        }

                        if (!string.IsNullOrWhiteSpace(paxSegment?.Pax_SegmentId?.ToString().Trim()))
                        {
                            string paxsegid = paxSegment?.Pax_SegmentId?.ToString().Trim();
                            Meal = GetMealDetails(paxsegid, paxSegmentDetails);
                            Bagg = GetBaggDetails(paxsegid, paxSegmentDetails);
                        }

                        string PaxType = "";
                        if (!string.IsNullOrWhiteSpace(paxSegment?.PaxType?.ToString().Trim()))
                        {
                            PaxType = paxSegment?.PaxType?.ToString().Trim();
                        }

                        if (!string.IsNullOrWhiteSpace(paxSegment?.Dob?.ToString().Trim()))
                        {
                            dob = paxSegment?.Dob?.ToString().Trim();
                        }

                        sbResponse.Append("<tr>");
                        sbResponse.Append("<td style='font-size:15px;' >" + fullname + "</td>");
                        sbResponse.Append("<td style='font-size:15px;' >" + tktno + "</td>");
                        sbResponse.Append("<td style='font-size:15px;' >" + fnn + "</td>");
                        sbResponse.Append("<td style='font-size:15px;' >");
                        if (Bagg != "")
                        {
                            sbResponse.Append("<span><img src='wwwroot\\assets\\img\\Commonimages\\baggage.png' title='Baggage' style='width: 16px; height: 16px;' /></span>");
                        }
                        else
                        {
                            sbResponse.Append("<span>--</span>");
                        }
                        if (Meal != "")
                        {
                            sbResponse.Append("<span>  <img src='wwwroot\\assets\\img\\Commonimages\\meal.png' style='width: 19px;' title='Meal' /></span>");
                        }
                        else
                        {
                            sbResponse.Append("<span>--</span>");
                        }
                        sbResponse.Append("</td>");
                        sbResponse.Append("</tr>");
                    }
                }

                sbResponse.Append("</table>");
                sbResponse.Append("</td>");
                sbResponse.Append("</tr>");
                sbResponse.Append("<tr>");
                sbResponse.Append("<td>" + GetSectorWiseDetailsOB(flightDetails, flightSegDetails,true));
                sbResponse.Append("</td>");
                sbResponse.Append("</tr>");

                if (Trip == "R")
                {
                    sbResponse.Append("<tr>");
                    sbResponse.Append("<td>" + GetSectorWiseDetailsIB(flightDetails, flightSegDetails,true));
                    sbResponse.Append("</td>");
                    sbResponse.Append("</tr>");
                }

                bool IsSingleFare = false;
                if (Trip == "R")
                {
                    var drFareType = fareDetailSeg.Where(f => f.Conn == "I" && f.PaxType == "ADT"); ;
                    if (drFareType.Any())
                    {
                        Decimal dCount = 0;
                        foreach (var dr in drFareType)
                        {
                            dCount = Convert.ToDecimal(dr.Yq.ToString().Trim());
                            dCount += Convert.ToDecimal(dr.Psf.ToString().Trim());
                            dCount += Convert.ToDecimal(dr.Udf.ToString().Trim());
                            dCount += Convert.ToDecimal(dr.Audf.ToString().Trim());
                            dCount += Convert.ToDecimal(dr.Cute.ToString().Trim());
                            dCount += Convert.ToDecimal(dr.Gst.ToString().Trim());
                            dCount += Convert.ToDecimal(dr.Tf.ToString().Trim());
                            dCount += Convert.ToDecimal(dr.Cess.ToString().Trim());
                            dCount += Convert.ToDecimal(dr.Ex.ToString().Trim());
                            dCount += Convert.ToDecimal(dr.Basic.ToString().Trim());

                            if (dCount <= 0)
                            {
                                IsSingleFare = true;

                                break;
                            }
                        }
                    }
                }


                if (HideFare != null && HideFare != "")
                {
                    if (HideFare == "False")
                    {
                        //======Fare Detail partr strat=============//
                        #region
                        sbResponse.Append("<tr id='divfaredetail'>");
                        sbResponse.Append("<td>");

                        sbResponse.Append("<table width='100%' style='font-size: 15px;' cellpadding='10' >");
                        sbResponse.Append("<tr >");
                        sbResponse.Append("<td width='73%' style='border: 1px solid #D6D8E7;'> <b> Service Information </b> ");
                        sbResponse.Append("</td>");
                        sbResponse.Append("<td style='border: 1px solid #D6D8E7;'> <b> Fare Information </b> ");
                        sbResponse.Append("</td>");
                        sbResponse.Append("</tr>");
                        sbResponse.Append("</table>");

                        sbResponse.Append("<table>");
                        sbResponse.Append("<tr>");

                        sbResponse.Append("<td width='45%' valign='top' align='left'>");

                        sbResponse.Append("<table width='100%' style='font-size: 15px;' >");

                        sbResponse.Append("<tbody>");


                        if (paxSegmentDetails != null && paxSegmentDetails.Any())
                        {
                            foreach (var paxSegment in paxSegmentDetails)
                            {
                                if (paxSegment.Conn != null && paxSegment.Conn.Equals("O"))
                                {
                                    string maealbagagge = paxSegment.ChargeType.Equals("M") ? "Meal" : "Baggage";
                                    sbResponse.Append("<tr>");
                                    sbResponse.Append("<td><b>* </b>" + Originn + "-" + Destinationn + " added " + maealbagagge + "<span>&nbsp;" + paxSegment.ChargeDescription + "</span>  <span>" + paxSegment.Charge_Amount + "Rs.</span></td>");
                                    sbResponse.Append("</tr>");
                                }
                                else
                                {
                                    string maealbagagge = paxSegment.ChargeType.Equals("M") ? "Meal" : "Baggage";
                                    sbResponse.Append("<tr>");
                                    sbResponse.Append("<td><b>* </b>" + Destinationn + "-" + Originn + " added " + maealbagagge + "<span>&nbsp;" + paxSegment.ChargeDescription + "</span>  <span>" + paxSegment.Charge_Amount + "Rs.</span></td>");
                                    sbResponse.Append("</tr>");
                                }
                            }
                        }


                        if (paxSegmentRuleDetails != null && paxSegmentRuleDetails.Any())
                        {
                            foreach (var paxSegment in paxSegmentRuleDetails)
                            {
                                if (paxSegment.Conn != null && paxSegment.Conn == "O")
                                {
                                    sbResponse.Append("<tr>");
                                    if (paxSegment.BaggageDetail != null && paxSegment.BaggageDetail.IndexOf("*") != -1)
                                    {
                                        string bagg = paxSegment.BaggageDetail;
                                        sbResponse.Append("<td><b>* </b>" + Originn + "-" + Destinationn + " available baggage on per pax  <span>" + bagg.Split('*')[0].Trim() + " Check IN </span>  </td>");
                                    }
                                    else
                                    {
                                        sbResponse.Append("<td><b>* </b>" + Originn + "-" + Destinationn + " available baggage on per pax  <span>Check IN: APAP </span>  </td>");
                                    }
                                    sbResponse.Append("</tr>");
                                }
                                else
                                {
                                    sbResponse.Append("<tr>");
                                    if (paxSegment.BaggageDetail != null && paxSegment.BaggageDetail.IndexOf("*") != -1)
                                    {
                                        string bagg = paxSegment.BaggageDetail;
                                        sbResponse.Append("<td><b>* </b>" + Destinationn + "-" + Originn + " available baggage on per pax  <span>" + bagg.Split('*')[0].Trim() + " Check IN </span>  </td>");
                                    }
                                    else
                                    {
                                        sbResponse.Append("<td><b>* </b>" + Destinationn + "-" + Originn + " available baggage on per pax  <span>Check IN: APAP </span>  </td>");
                                    }
                                    sbResponse.Append("</tr>");
                                }
                            }
                        }
                        sbResponse.Append("<tr>");
                        sbResponse.Append("<td>This is an Electronic ticket. Please carry a positive identification for Check in.</td>");
                        sbResponse.Append("</tr>");

                        sbResponse.Append("</tbody>");
                        sbResponse.Append("</table>");
                        sbResponse.Append("</td>");

                        sbResponse.Append("<td width='28%' valign='top' align='right'>");
                        if (flightDetails != null && flightDetails.Any())
                        {
                            string adt = flightDetails.FirstOrDefault().Adt.ToString().Trim();
                            string chd = flightDetails.FirstOrDefault().Chd.ToString().Trim();
                            string inf = flightDetails.FirstOrDefault().Inf.ToString().Trim();

                            int TotalServiceFee = Convert.ToInt32(Convert.ToDouble(flightDetails.FirstOrDefault().TotalServiceFee_deal.ToString().Trim()));
                            int TotalServiceTax = Convert.ToInt32(Convert.ToDouble(flightDetails.FirstOrDefault().TotalServiceTax.ToString().Trim()));
                            int TotalMealcharges = Convert.ToInt32(Convert.ToDouble(flightDetails.FirstOrDefault().TotalMeal.ToString().Trim()));
                            int TotalBaggCharges = Convert.ToInt32(Convert.ToDouble(flightDetails.FirstOrDefault().TotalBaggage.ToString().Trim()));
                            int TotalFare = Convert.ToInt32(Convert.ToDouble(flightDetails.FirstOrDefault().TotalFare.ToString().Trim()));
                            int TotalTDS = Convert.ToInt32(Convert.ToDouble(flightDetails.FirstOrDefault().TotalTds.ToString().Trim()));
                            int TotalMarkup = Convert.ToInt32(Convert.ToDouble(flightDetails.FirstOrDefault().TotalMarkup.ToString().Trim()));

                            int TotalBasic = Convert.ToInt32(Convert.ToDouble(flightDetails.FirstOrDefault().TotalBasic.ToString().Trim()));
                            int TotalTax = Convert.ToInt32(Convert.ToDouble(flightDetails.FirstOrDefault().TotalTax.ToString().Trim()));
                            TotalTax += TotalMarkup;

                            int TotalCommission = Convert.ToInt32(Convert.ToDouble(flightDetails.FirstOrDefault().TotalCommission.ToString().Trim()));
                            TotalFare = (TotalFare + TotalMealcharges + TotalBaggCharges + SurchargeAmount) - (TotalTDS);


                            if (Tax != null && Tax != "")
                            {
                                if (Trip == "R" && IsSingleFare.Equals(true) || Trip == "O")
                                {
                                    int noofpx = Convert.ToInt32(adt) + Convert.ToInt32(chd);
                                    int tottax = noofpx * Convert.ToInt32(Tax);
                                    TotalTax = TotalTax + tottax;
                                    TotalFare = TotalFare + tottax;
                                }
                                if (flightDetails.FirstOrDefault().Trip.ToString().Equals("R") && IsSingleFare.Equals(false))
                                {
                                    int noofpx = Convert.ToInt32(adt) + Convert.ToInt32(chd);
                                    noofpx = noofpx * 2;
                                    int tottax = noofpx * Convert.ToInt32(Tax);
                                    TotalTax = TotalTax + tottax;
                                    TotalFare = TotalFare + tottax;
                                }
                            }


                            sbResponse.Append("<table width='100%' style='font-size:15px;' >");
                            sbResponse.Append("<tbody>");

                            sbResponse.Append("<tr>");
                            sbResponse.Append("<td align='right'>BFare :</td>");
                            sbResponse.Append("<td align='right'>" + TotalBasic + "" + Currency + "</td>");
                            sbResponse.Append("</tr>");
                            sbResponse.Append("<tr>");
                            sbResponse.Append("<td align='right'>  Tax :  </td>");
                            sbResponse.Append("<td align='right'> <span id='oldtotaltax'>" + TotalTax + "" + Currency + "</span> </td>");
                            sbResponse.Append("</tr>");
                            sbResponse.Append("<tr>");
                            sbResponse.Append("<td align='right'> Fee & Surcharge :  </td>");
                            sbResponse.Append("<td align='right'>" + (TotalServiceFee + TotalServiceTax).ToString() + "" + Currency + "</td>");
                            sbResponse.Append("</tr>");

                            //sbResponse.Append("<tr id='divEditTxnFeeDiv' style='display:none;'>");
                            //sbResponse.Append("<td align='right'>Txn Fee:</td>");
                            //sbResponse.Append("<td align='right'>Rs. <input type='text' style='width: 80px;' id='CurrentTxnFee' name='CurrentTxnFee' value='0' /></td>");
                            //sbResponse.Append("</tr>");

                            //if (Disc != null && Disc != "")
                            //{
                                //sbResponse.Append("<tr id='divApplyDiscount' style='display:none;'>");
                                //sbResponse.Append("<td align='right'> Discount(-) :</td>");
                                //sbResponse.Append("<td align='right'><input type='text' style='width: 80px;' id='CurrentDiscount' name='CurrentDiscount' value='0' /></td>");
                                //sbResponse.Append("</tr>");

                                //sbResponse.Append("<tr id='Discountdiv'  align='right' >");
                                //sbResponse.Append("<td align='right'> Discount(-) :</td>");
                                //sbResponse.Append("<td align='right'>Rs <span id='spndiscount'>" + Disc + "" + Currency + "</span></td>");
                                //sbResponse.Append("</tr>");
                                //TotalFare = TotalFare - Convert.ToInt32(Disc);
                            //}
                            sbResponse.Append("<tr>");
                            sbResponse.Append("<td align='right'> Services :  </td>");
                            sbResponse.Append("<td align='right'>" + (TotalMealcharges + TotalBaggCharges).ToString() + "" + Currency + "</td>");
                            sbResponse.Append("</tr>");


                            sbResponse.Append("<tr>");
                            sbResponse.Append("<td align='right'> Conv.fee :  </td>");
                            sbResponse.Append("<td align='right'>" + (SurchargeAmount).ToString() + "" + Currency + "</td>");
                            sbResponse.Append("</tr>");

                            sbResponse.Append("<tr >");
                            sbResponse.Append("<td align='right'> Total Amount :  </td>");
                            sbResponse.Append("<td align='right'><span id='oldtotalamt'>" + TotalFare + "" + Currency + "</span></td>");
                            sbResponse.Append("</tr>");

                            sbResponse.Append("</tbody>");
                            sbResponse.Append("</table>");
                        }

                        sbResponse.Append("</td>");

                        sbResponse.Append("</tr>");


                        sbResponse.Append("</table>");
                        sbResponse.Append("</td>");

                        sbResponse.Append("</tr>");
                        #endregion
                        //======Fare Detail partr End=============//
                    }
                    else
                    {
                        //======Fare Detail partr new start=============//
                        #region
                        sbResponse.Append("<tr id='divfaredetailnew'>");
                        sbResponse.Append("<td>");

                        sbResponse.Append("<table width='100%' style='' cellpadding='10' >");
                        sbResponse.Append("<tr style='font-size:15px;'>");

                        sbResponse.Append("<td  style='border: 1px solid #D6D8E7;' colspan='2'> Service Information ");
                        sbResponse.Append("</td>");



                        sbResponse.Append("</tr>");
                        sbResponse.Append("</table>");

                        sbResponse.Append("<table>");
                        sbResponse.Append("<tr>");
                        sbResponse.Append("<td width='45%' valign='top' align='left'>");
                        sbResponse.Append("<table width='100%' style='font-size: 15px;' >");
                        sbResponse.Append("<tbody>");


                        if (paxSegmentDetails.Any())
                        {
                            foreach (var paxSegment in paxSegmentDetails)
                            {
                                if (paxSegment.Conn != null && paxSegment.Conn.Equals("O"))
                                {
                                    string maealbagagge = "";
                                    if (paxSegment.ChargeType.Equals("M"))
                                    {
                                        maealbagagge = "Meal";
                                    }
                                    else
                                    {
                                        maealbagagge = "Baggage";
                                    }

                                    sbResponse.Append("<tr>");
                                    sbResponse.Append("<td><b>* </b>" + Originn + "-" + Destinationn + " added " + maealbagagge + "<span>&nbsp;" + paxSegment.ChargeDescription + "</span>  <span>" + paxSegment.Charge_Amount + "Rs.</span></td>");
                                    sbResponse.Append("</tr>");
                                }
                                else
                                {
                                    string maealbagagge = "";
                                    if (paxSegment.ChargeType.Equals("M"))
                                    {
                                        maealbagagge = "Meal";
                                    }
                                    else
                                    {
                                        maealbagagge = "Baggage";
                                    }

                                    sbResponse.Append("<tr>");
                                    sbResponse.Append("<td><b>* </b>" + Destinationn + "-" + Originn + " added " + maealbagagge + "<span>&nbsp;" + paxSegment.ChargeDescription + "</span>  <span>" + paxSegment.Charge_Amount + "Rs.</span></td>");
                                    sbResponse.Append("</tr>");
                                }
                            }
                        }




                        if (paxSegmentRuleDetails != null && paxSegmentRuleDetails.Any())
                        {
                            foreach (var paxSegment in paxSegmentRuleDetails)
                            {
                                if (paxSegment.Conn != null && paxSegment.Conn.Equals("O"))
                                {
                                    sbResponse.Append("<tr>");
                                    if (paxSegment.BaggageDetail != null && paxSegment.BaggageDetail.IndexOf("*") != -1)
                                    {
                                        string bagg = paxSegment.BaggageDetail;
                                        sbResponse.Append("<td><b>* </b>" + Originn + "-" + Destinationn + " available baggage on per pax  <span>" + bagg.Split('*')[0].Trim() + " Check IN </span>  </td>");
                                    }
                                    else
                                    {
                                        sbResponse.Append("<td><b>* </b>" + Originn + "-" + Destinationn + " available baggage on per pax  <span>Check IN: APAP </span>  </td>");
                                    }
                                    sbResponse.Append("</tr>");
                                }
                                else
                                {
                                    sbResponse.Append("<tr>");
                                    if (paxSegment.BaggageDetail != null && paxSegment.BaggageDetail.IndexOf("*") != -1)
                                    {
                                        string bagg = paxSegment.BaggageDetail;
                                        sbResponse.Append("<td><b>* </b>" + Destinationn + "-" + Originn + " available baggage on per pax  <span>" + bagg.Split('*')[0].Trim() + " Check IN </span>  </td>");
                                    }
                                    else
                                    {
                                        sbResponse.Append("<td><b>* </b>" + Destinationn + "-" + Originn + " available baggage on per pax  <span>Check IN: APAP </span>  </td>");
                                    }
                                    sbResponse.Append("</tr>");
                                }
                            }
                        }
                        sbResponse.Append("<tr>");
                        sbResponse.Append("<td>This is an Electronic ticket. Please carry a positive identification for Check in.</td>");
                        sbResponse.Append("</tr>");
                        sbResponse.Append("</tbody>");
                        sbResponse.Append("</table>");
                        sbResponse.Append("</td>");

                        sbResponse.Append("<td width='28%' valign='top' align='right'>");
                        sbResponse.Append("</td>");

                        sbResponse.Append("</tr>");


                        sbResponse.Append("</table>");
                        sbResponse.Append("</td>");

                        sbResponse.Append("</tr>");
                        #endregion
                        //======Fare Detail partr new End=============//
                    }
                }

                //======GST Detail start=============//
                var gstDetail = bookingDetail.CompanyFlightGSTDetails.FirstOrDefault();
                if (gstDetail != null)
                {

                    sbResponse.Append("<tr>");
                    sbResponse.Append("<td colspan='2'>");
                    sbResponse.Append("<table width='100%' style='border: 1px solid #D6D8E7;' cellspacing='0' cellpadding='10' >");
                    sbResponse.Append("<tr style='font-size:15px;'>");
                    sbResponse.Append("<td width='73%' style=''>GST Detail");
                    sbResponse.Append("</td>");
                    sbResponse.Append("</tr>");
                    sbResponse.Append("</table>");
                    sbResponse.Append("</td>");
                    sbResponse.Append("</tr>");

                    sbResponse.Append("<tr>");
                    sbResponse.Append("<td colspan='2'>");
                    sbResponse.Append("<table width='100%' style='font-size:11px;' cellspacing='0' cellpadding='3' >");
                    sbResponse.Append("<tr>");
                    sbResponse.Append("<td>Gst CompanyName:</td>");
                    sbResponse.Append("<td>" + StringHelper.GetUpper(gstDetail.GstcompanyName.ToString().Trim()) + "</td>");
                    sbResponse.Append("</tr>");
                    sbResponse.Append("<tr>");
                    sbResponse.Append("<td>Gst Number:</td>");
                    sbResponse.Append("<td>" + StringHelper.GetUpper(gstDetail.Gstnumber.ToString().Trim()) + "</td>");
                    sbResponse.Append("</tr>");
                    sbResponse.Append("</table>");
                    sbResponse.Append("</td>");
                    sbResponse.Append("</tr>");
                }
                //======GST Detail End =============//

                sbResponse.Append("<tr>");
                sbResponse.Append("<td colspan='2'>");
                sbResponse.Append("<table width='100%' style='border: 1px solid #D6D8E7;' cellspacing='0' cellpadding='10' >");
                sbResponse.Append("<tr style='font-size:15px;'>");
                sbResponse.Append("<td width='73%' style=''><b> Terms & Conditions </b>");
                sbResponse.Append("</td>");
                sbResponse.Append("</tr>");
                sbResponse.Append("</table>");
                sbResponse.Append("</td>");
                sbResponse.Append("</tr>");

                sbResponse.Append("<tr>");
                sbResponse.Append("<td colspan='2'>");
                sbResponse.Append("<table width='100%' style='font-size:10px;' cellspacing='0' cellpadding='3' >");
                sbResponse.Append("<tr>");
                sbResponse.Append("<td><b>*</b> All Guests, including children and infants, must present valid identification at check-in.");
                sbResponse.Append("</td>");
                sbResponse.Append("</tr>");
                sbResponse.Append("<tr>");
                sbResponse.Append("<td><b>*</b> Carriage and other services provided by the carrier are subject to conditions of carriage, which are hereby incorporated by reference. These conditions may be obtained from the issuing carrier.");
                sbResponse.Append("</td>");
                sbResponse.Append("</tr>");
                sbResponse.Append("<tr>");
                sbResponse.Append("<td><b>*</b> Changes to your reservation will result in a fee of Airline terms & condition,change plus any difference in the fare between the original fare paid and the fare for the revised booking.");
                sbResponse.Append("</td>");
                sbResponse.Append("</tr>");

                sbResponse.Append("<tr>");
                sbResponse.Append("<td><b>*</b> Cancellation charges as per Airlines terms & condition.");
                sbResponse.Append("</td>");
                sbResponse.Append("</tr>");

                sbResponse.Append("<tr>");
                sbResponse.Append("<td><b>*</b> Airline Free Baggages are per seat only.");
                sbResponse.Append("</td>");
                sbResponse.Append("</tr>");


                sbResponse.Append("<tr>");
                sbResponse.Append("<td><b>*</b> Check your itinerary with airline also,revert with in 2 hour. we are here to help you.");
                sbResponse.Append("</td>");
                sbResponse.Append("</tr>");

                sbResponse.Append("<tr>");
                sbResponse.Append("<td><b>*</b> As per Government guidelines, check-in counters at all airports will now close 45 minutes before departure with immediate effect. Please plan your Airport arrival accordingly.");
                sbResponse.Append("</td>");
                sbResponse.Append("</tr>");
                sbResponse.Append("<tr>");
                sbResponse.Append("<td><b>*</b> Wish You a Happy Journey.");
                sbResponse.Append("</td>");
                sbResponse.Append("</tr>");

                sbResponse.Append("</table>");
                sbResponse.Append("</td>");
                sbResponse.Append("</tr>");

                sbResponse.Append("</table>");
            }
            //}
            //}

            catch (Exception ex)
            {
                DBLoggerHelper.DBLogAsync("", iBookingRef, "Print_Popup_Tkt", "PrintPopupController", "", "", ex.Message, _addDbLogCommandHandler);
            }

            return sbResponse.ToString();
        }

        [HttpPost]
        public async Task<IActionResult> PrintPopupExportToPDF([FromBody] PrintPopupPdfRequest printPopupPdfRequest)
        {
            if (!string.IsNullOrEmpty(printPopupPdfRequest.BookingRef?.Trim()))
            {
                string bookingRefDecoded = EncodeDecodeHelper.DecodeFrom64(printPopupPdfRequest.BookingRef.Trim());
                if (int.TryParse(bookingRefDecoded, out int iBookingRef) && iBookingRef > 0)
                {
                    var query = new BookingDetailQuery { BookingRef = iBookingRef };
                    var flightbooking = await _bookingTicketQueryHandler.HandleAsync(query);

                    if (flightbooking != null)
                    {
                        string sessionCompany = UserHelper.GetCompanyID(User);

                        StringBuilder reportHTML = new StringBuilder(PrintPopupPdf(
                            flightbooking,
                            iBookingRef,
                            printPopupPdfRequest.Tax,
                            printPopupPdfRequest.ShowLogo,
                            sessionCompany,
                            printPopupPdfRequest.HideFare,
                            printPopupPdfRequest.Disc
                        ));

                        string baseUri = Directory.GetCurrentDirectory();

                        var margins = (Top: 10f, Right: 10f, Bottom: 0f, Left: 10f);

                        var pdfStream = GeneratePDF.GeneratePdfStream(reportHTML.ToString(), margins, baseUri);

                        return File(pdfStream, "application/pdf", $"Print_{iBookingRef}.pdf");
                    }
                    else
                    {
                        return BadRequest(new { ErrorMessage = "No booking found for the provided reference." });
                    }
                }
                else
                {
                    return BadRequest(new { ErrorMessage = "Invalid booking reference. Please check the provided reference." });
                }
            }
            else
            {
                return BadRequest(new { ErrorMessage = "Booking reference is required." });
            }
        }

        [HttpPost]
        public async Task<IActionResult> SendPrintPopupTicketMail([FromBody] PrintPoupEmailRequest printPoupEmailRequest)
        {
            if (!string.IsNullOrWhiteSpace(printPoupEmailRequest.BookingRef) && !string.IsNullOrWhiteSpace(printPoupEmailRequest.Email))
            {
                var bookingRef = printPoupEmailRequest.BookingRef;
                var email = printPoupEmailRequest.Email;

                string bookingRefDecoded = EncodeDecodeHelper.DecodeFrom64(bookingRef.Trim());
                if (int.TryParse(bookingRefDecoded, out int iBookingRef))
                {
                    var query = new BookingDetailQuery { BookingRef = iBookingRef };
                    var flightbooking = await _bookingTicketQueryHandler.HandleAsync(query);
                    if (flightbooking != null)
                    {

                        var flightDetails = flightbooking.CompanyFlightDetailAirlines;
                        var paxDetails = flightbooking.CompanyPaxDetailAirlines;
                        var fareDetails = flightbooking.CompanyFareDetailAirlines?.FirstOrDefault();

                        string BookingStatus = Getbookingstatus(flightDetails, paxDetails, fareDetails).ToUpper();

                        if (BookingStatus.Equals("REJECTED"))
                        {
                            BookingStatus = "REJECT";
                        }

                        string MailType;
                        if (BookingStatus.Equals("CONFIRM"))
                        {
                            MailType = " Confirmation";
                        }
                        else if (BookingStatus.Equals("REJECT"))
                        {
                            MailType = " Reject";
                        }
                        else if (BookingStatus.Equals("UNCONFIRM"))
                        {
                            MailType = " Confirmation";
                        }
                        else if (BookingStatus.Equals("CANCEL"))
                        {
                            MailType = " Cancellation Request";
                        }
                        else if (BookingStatus.Equals("INV"))
                        {
                            MailType = " Invoice";
                        }
                        else if(BookingStatus.Equals("ON HOLD"))
                        {
                            MailType = " On Hold";
                        }
                        else
                        {
                            MailType = " "; // Provide a default fallback text
                        }

                        string emailSubject = MailType + " of BookingRef: " + iBookingRef;
                        string emailBody = $"<html><body><h4>Please find the attached Booking Ref: {iBookingRef} {MailType} </h4></body></html>";


                        string reportHTML = PrintPopupPdf(
                  flightbooking,
                  iBookingRef,
                  printPoupEmailRequest.Tax,
                  printPoupEmailRequest.ShowLogo,
                  UserHelper.GetCompanyID(User),
                  printPoupEmailRequest.HideFare,
                  printPoupEmailRequest.Disc
              );
                        string baseUri = Directory.GetCurrentDirectory();
                        var margins = (Top: 10f, Right: 10f, Bottom: 0f, Left: 10f);
                        var pdfStream = GeneratePDF.GeneratePdfStream(reportHTML, margins, baseUri);
                        byte[] pdfBytes;
                        using (var memoryStream = new MemoryStream())
                        {
                            pdfStream.CopyTo(memoryStream);
                            pdfBytes = memoryStream.ToArray();
                        }

                        bool emailSent = await EmailService.SendEmail(email, emailSubject, emailBody, pdfBytes, $"ETicket_{iBookingRef}.pdf");

                        if (emailSent)
                        {
                            return Json(new { success = true, message = "Email sent successfully." });
                        }
                        else
                        {
                            return Json(new { success = false, message = "Failed to send email. Please try again later." });
                        }
                    }
                    else
                    {
                        return Json(new { success = false, message = "No booking found for the provided reference." });
                    }
                }
                else
                {
                    return Json(new { success = false, message = "Invalid booking reference." });
                }
            }
            else
            {
                return Json(new { success = false, message = "Booking reference and email are required." });
            }
        }
        public string PrintPopupPdf(BookingDetailData bookingDetail, int iBookingRef, string Tax, string Logo, string SessionCompany, string HideFare, string Disc)
        {

            StringBuilder sbResponse = new StringBuilder();
            try
            {
                var flightDetails = bookingDetail.CompanyFlightDetailAirlines;
                var flightSegDetails = bookingDetail.CompanyFlightSegmentDetailAirlines;
                var fareDetails = bookingDetail.CompanyFareDetailAirlines?.FirstOrDefault();
                var fareDetailAirline = bookingDetail.CompanyFareDetailAirlines;
                var fareDetailSeg = bookingDetail.CompanyFareDetailSegmentAirlines;
                var paxDetails = bookingDetail.CompanyPaxDetailAirlines;
                //var flightFareRule = bookingDetail.CompanyFlightDetailAirlines;
                var compDetails = bookingDetail.CompanyFlightDetails?.FirstOrDefault();
                var paxSegmentDetails = bookingDetail.CompanyPaxSegmentDetailAirlines;
                var paxSegmentRuleDetails = bookingDetail.CompanyFlightSegmentRuleDetailAirlines;
                var companyTransactionDetail = bookingDetail.CompanyTransactionDetails;
                var bookingAirlineLogForPGs = bookingDetail.BookingAirlineLogForPGs;
                string IssueON = string.Empty;
                string Originn = string.Empty;
                string Destinationn = string.Empty;
                string Adt = string.Empty;
                string Chd = string.Empty;
                string Inf = string.Empty;
                string Trip = string.Empty;
                int SurchargeAmount = 0;


                Double dCurrencyValue = 1;
                string Currency = "INR";

                if (companyTransactionDetail != null && companyTransactionDetail.Any() && bookingAirlineLogForPGs != null && bookingAirlineLogForPGs.Any())
                {
                    dCurrencyValue = Convert.ToDouble(bookingAirlineLogForPGs.FirstOrDefault().CurrencyValue.ToString());
                    Currency = bookingAirlineLogForPGs.FirstOrDefault().Currency.ToString().Trim();
                }

                if (companyTransactionDetail?.FirstOrDefault()?.SurchargeAmount != null &&
        decimal.TryParse(companyTransactionDetail.FirstOrDefault().SurchargeAmount.ToString(), out var surchargeAmount))
                {
                    SurchargeAmount = Decimal.ToInt32(surchargeAmount);
                }
                else
                {
                    SurchargeAmount = 0;
                }


                Trip = flightDetails.FirstOrDefault().Trip.ToString().Trim();
                IssueON = StringHelper.GetConvertedDateTime(flightDetails.FirstOrDefault().EventTime.ToString());
                Originn = flightDetails.FirstOrDefault().Origin.ToString().Trim();
                Destinationn = flightDetails.FirstOrDefault().Destination.ToString().Trim();
                Adt = flightDetails.FirstOrDefault().Adt.ToString().Trim();
                Chd = flightDetails.FirstOrDefault().Chd.ToString().Trim();
                Inf = flightDetails.FirstOrDefault().Inf.ToString().Trim();

                string CompanyLogo = string.Empty;
                string CompanyName = string.Empty;
                string Address = string.Empty;
                string Mobile = string.Empty;
                string Email = string.Empty;
                string CompanyID = flightDetails.FirstOrDefault().CompanyId.ToString();


                if (CompanyID.IndexOf("-SA-") != -1 || CompanyID.IndexOf("C-") != -1)
                {
                    if (CompanyID.IndexOf("C-") != -1)
                    {
                        Mobile = compDetails.A_Mobile.ToString();
                        Email = StringHelper.GetLower(compDetails.A_Email.ToString());
                        bool compactiv = Convert.ToBoolean(compDetails.AgencyPrintB2c.ToString());
                        if (compactiv)
                        {
                            CompanyName = StringHelper.GetUpper(compDetails.A_CompanyName.ToString());
                            CompanyLogo = compDetails.A_CompanyLogo.ToString();
                        }
                    }
                    else
                    {
                        CompanyLogo = compDetails.A_CompanyLogo.ToString();
                        CompanyName = StringHelper.GetUpper(compDetails.A_CompanyName.ToString());
                        Address = StringHelper.GetALLFirstCharacterCapital(compDetails.A_Address.ToString());
                        Mobile = compDetails.A_Mobile.ToString();
                        Email = StringHelper.GetLower(compDetails.A_Email.ToString());
                    }
                }
                else
                {
                    CompanyLogo = compDetails.CompanyLogo.ToString();
                    CompanyName = StringHelper.GetUpper(compDetails.CompanyName.ToString());
                    Address = StringHelper.GetALLFirstCharacterCapital(compDetails.Address.ToString());
                    Mobile = compDetails.Mobile.ToString();
                    Email = StringHelper.GetLower(compDetails.Email.ToString());
                }

                sbResponse.Append("<div style='margin-top:-40px;margin-left:-45px;'>");
                sbResponse.Append("<table style='border: 1px solid #D6D8E7; width: 785px;  font-size: 11px; font-family: Arial,Sans-Serif;' cellspacing='3' id='showmndetail' runat='server'>");
                if (Logo != null && Logo != "")
                {
                    if (Logo != "False")
                    {
                        sbResponse.Append("<tr id='logodiv'>");
                        sbResponse.Append("<td style='border-bottom: 1px solid #d6d8e7;'>");
                        sbResponse.Append("<img id='imgeLogo' src='" + CompanyLogo + "' style='  height: 63px;  width: 218px; '/>");
                        sbResponse.Append("</td>");
                        sbResponse.Append("</tr>");
                    }
                }


                sbResponse.Append("<tr>");
                sbResponse.Append("<td>");
                sbResponse.Append("<table width='100%'  style='border: 1px solid #D6D8E7;background: #fff;' cellpadding='10' >");
                sbResponse.Append("<tr>");

                sbResponse.Append("<td width='40%'  >");
                sbResponse.Append("<span style='font-size: 15px;   color: #929292;'> <img src='wwwroot\\assets\\img\\airlogo_square\\onwardflight.png' width='20px' /> Onward Flight </span>");
                sbResponse.Append("<span style='font-size: 15px;'> <b> " + flightDetails.FirstOrDefault().OriginCity.ToString().Trim() + " to </b> </span>");
                sbResponse.Append("<span style='font-size: 15px;'> <b> " + flightDetails.FirstOrDefault().DestinationCity.ToString().Trim() + " </b> </span>");

                sbResponse.Append("</td>");

                sbResponse.Append("<td width='20%'>");
                sbResponse.Append("</td>");

                sbResponse.Append("<td width='40%'>");

                sbResponse.Append("<span>Airline PNR:</span><span id='spnpnr' style='font-size: 22px;' > <b>" + flightDetails.FirstOrDefault().Airline_Pnr_D.ToString() + "</b> </span><br />");

                sbResponse.Append("</td>");

                sbResponse.Append("</tr>");
                sbResponse.Append("</table>");
                sbResponse.Append("</td>");
                sbResponse.Append("</tr>");

                if (flightDetails.FirstOrDefault().Trip.ToString().Equals("R"))
                {

                    sbResponse.Append("<tr>");
                    sbResponse.Append("<td>");
                    sbResponse.Append("<table width='100%'  style='border: 1px solid #D6D8E7;background: #fff;' cellpadding='10' >");
                    sbResponse.Append("<tr>");

                    sbResponse.Append("<td width='40%'  >");
                    sbResponse.Append("<span style='font-size: 15px;   color: #929292;'> <img src='wwwroot\\assets\\img\\airlogo_square\\Inwardflight.png' width='20px' /> Return Flight </span>");
                    sbResponse.Append("<span style='font-size: 15px;'> " + flightDetails.FirstOrDefault().DestinationCity.ToString().Trim() + " to </span>");
                    sbResponse.Append("<span style='font-size: 15px;'> " + flightDetails.FirstOrDefault().OriginCity.ToString().Trim() + "</span>");

                    sbResponse.Append("</td>");

                    sbResponse.Append("<td width='20%'>");
                    sbResponse.Append("</td>");

                    sbResponse.Append("<td width='40%' >");

                    sbResponse.Append("<span>Airline PNR:</span><span id='spnpnr' style='font-size: 22px;' ><b> " + flightDetails.FirstOrDefault().Airline_Pnr_A.ToString() + " </b> </span><br />");

                    sbResponse.Append("</td>");

                    sbResponse.Append("</tr>");
                    sbResponse.Append("</table>");
                    sbResponse.Append("</td>");
                    sbResponse.Append("</tr>");
                }



                sbResponse.Append("<tr>");
                sbResponse.Append("<td>");
                sbResponse.Append("<table width='100%' style='border: 1px solid #D6D8E7;' cellpadding='10' >");
                sbResponse.Append("<tr >");
                sbResponse.Append("<td width='33%' style='font-size:15px;' ><span> Booking Ref. </span> <span id='bookinref'> " + iBookingRef.ToString() + "  </span> ");
                sbResponse.Append("</td>");
                sbResponse.Append("<td width='33%' align='center' style='font-size: 55px;' > ");
                sbResponse.Append("</td>");
                sbResponse.Append("<td width='33%' align='right' style='font-size:15px;' ><span>Issued Date:</span><span id='pnrheadingIssueOn'> " + IssueON + "</span>  ");
                sbResponse.Append("</td>");
                sbResponse.Append("</tr>");
                sbResponse.Append("</table>");
                sbResponse.Append("</td>");
                sbResponse.Append("</tr>");

                sbResponse.Append("<tr>");
                sbResponse.Append("<td>");
                sbResponse.Append("<table width='100%' cellpadding='10'>");
                sbResponse.Append("<tr>");
                sbResponse.Append("<td width='33%' valign='top'>");
                sbResponse.Append("<span style='font-size: 16px;'><span id='txtCompany_Name'><b>" + CompanyName + "</b></span></span><br />");
                sbResponse.Append("<span style='font-size: 15px; color: #929292;' id='txtAddressCompany'>" + Address + "");
                sbResponse.Append("</span><br />");
                sbResponse.Append("<span style='font-size: 15px; color: #929292;' >Phone: <span id='CMobile'>" + Mobile + "</span><br />");
                sbResponse.Append("<span style='font-size: 15px; color: #929292;'>Email: </span><span id='CEmail'>" + Email + "</span>");
                sbResponse.Append("</span>");
                sbResponse.Append("</td>");
                sbResponse.Append("<td width='33%' align='center' >");
                sbResponse.Append("<b style=' font-size:27px;'>E - Ticket </b>");
                sbResponse.Append("</td>");
                sbResponse.Append("<td width='33%' valign='top' align='left' style='font-size: 15px;' >");
                sbResponse.Append("");

                //if (flightDetails.FirstOrDefault().Trip.ToString().Equals("O"))
                //{
                //    sbResponse.Append("<span>PNR:</span><span id='spnpnr'> " + flightDetails.FirstOrDefault().Airline_Pnr_D.ToString() + "</span><br />");
                //}
                //else
                //{
                //    sbResponse.Append("<span >PNR:</span><span id='spnpnr'> " + flightDetails.FirstOrDefault().Airline_Pnr_D.ToString() + "/" + flightDetails.FirstOrDefault().Airline_Pnr_A.ToString() + "</span><br />");
                //}

                string strStatus = Getbookingstatus(flightDetails, paxDetails, fareDetails);

                if (strStatus.Equals("Confirm"))
                {
                    sbResponse.Append("<span style='font-size:18px;'> <img src='wwwroot\\assets\\img\\airlogo_square\\confirmcheck.png' width='22px' /> </span><span id='spnstatus' style='Color:#00e300; font-size: 25px;'> Confirm</span><br />");
                }
                else if (strStatus.Equals("Unconfirm"))
                {
                    sbResponse.Append("<span style='font-size:18px;'> <img src='wwwroot\\assets\\img\\airlogo_square\\unconfirmcross.png' width='22px' /> </span><span id='spnstatus' style='Color:Red; font-size: 25px;'> Unconfirm</span><br />");
                }
                else if (strStatus.Equals("On Hold"))
                {
                    sbResponse.Append("<span style='font-size:18px;'> <img src='wwwroot\\assets\\img\\airlogo_square\\unconfirmcross.png' width='22px' /> </span><span id='spnstatus' style='Color:Red; font-size: 25px;'> On Hold</span><br />");
                }
                else
                {
                    sbResponse.Append("<span style='font-size:18px;'> <img src='wwwroot\\assets\\img\\airlogo_square\\unconfirmcross.png' width='22px' /> </span><span id='spnstatus' style='Color:Red; font-size: 25px;'> " + strStatus + "</span><br />");
                }
                if (paxDetails != null && paxDetails.Any())
                {
                    sbResponse.Append("<span style='color: #929292;' >MobileNo:</span><span id='spnpaxmobile' style='color: #929292;' > " + paxDetails.FirstOrDefault().MobileNo.ToString().Trim() + "</span><br />");
                    sbResponse.Append("<span style='color: #929292;' >Email:</span><span id='spanpaxEmail' style='color: #929292;' > " + StringHelper.GetLower(paxDetails.FirstOrDefault().Email.ToString().Trim().ToLower()) + "</span>");
                }

                sbResponse.Append("</td>");
                sbResponse.Append("</tr>");
                sbResponse.Append("</table>");
                sbResponse.Append("</td>");
                sbResponse.Append("</tr>");


                sbResponse.Append("<tr>");
                sbResponse.Append("<td>");
                sbResponse.Append("<table width='100%' style='border: 1px solid #D6D8E7;' cellspacing='0' cellpadding='10'>");
                sbResponse.Append("<tr style=' font-size:15px;'>");
                sbResponse.Append("<td width='34%' style='border-bottom: 1px solid #D6D8E7; '> <b> Passengers  </b> </td>");
                sbResponse.Append("<td width='27%' style='border-bottom: 1px solid #D6D8E7;' > <b> Ticket Number  </b> </td>");
                sbResponse.Append("<td width='20%' style='border-bottom: 1px solid #D6D8E7;' > <b> Frequent flyer no. </b> </td>");
                sbResponse.Append("<td width='16%' style='border-bottom: 1px solid #D6D8E7;' align='center' > <b> Services </b> </td>");
                sbResponse.Append("</tr>");
                foreach (var paxItem in paxDetails)
                {
                    string dob = "--";
                    string fnn = "--";
                    string Meal = string.Empty;
                    string Bagg = string.Empty;
                    string fullname = StringHelper.GetUpper(paxItem.Title.ToString().Trim() + " " + paxItem.First_Name.ToString().Trim() + " " + paxItem.Middle_Name.ToString().Trim() + " " + paxItem.Last_Name.ToString().Trim());
                    string tktno = "--";
                    if (paxItem.TicketNo.ToString().Trim() != "")
                    {
                        tktno = paxItem.TicketNo.ToString().Trim();
                    }
                    if (paxItem.Ffn.ToString().Trim() != "")
                    {
                        fnn = paxItem.Ffn.ToString().Trim();
                    }
                    if (paxItem.Pax_SegmentId.ToString().Trim() != "")
                    {
                        string paxsegid = paxItem.Pax_SegmentId.ToString().Trim();
                        Meal = GetMealDetails(paxsegid, paxSegmentDetails);
                        Bagg = GetBaggDetails(paxsegid, paxSegmentDetails);
                    }
                    string PaxType = "";
                    if (paxItem.PaxType.ToString().Trim() != "")
                    {
                        PaxType = paxItem.PaxType.ToString().Trim();
                    }
                    if (paxItem.Dob.ToString().Trim() != "")
                    {
                        dob = paxItem.Dob.ToString().Trim();
                    }
                    sbResponse.Append("<tr>");
                    sbResponse.Append("<td style='font-size:15px;'>" + fullname + "</td>");
                    sbResponse.Append("<td style='font-size:15px;'>" + tktno + "</td>");
                    sbResponse.Append("<td style='font-size:15px;'>" + fnn + "</td>");
                    sbResponse.Append("<td style='font-size:15px;' align='center'>");
                    if (Bagg != "")
                    {
                        sbResponse.Append("<span><img src='wwwroot\\assets\\img\\Commonimages\\baggage.png' title='Baggage' style='width: 16px;' /></span>");
                    }
                    else
                    {
                        sbResponse.Append("<span>--</span>");
                    }
                    if (Meal != "")
                    {
                        sbResponse.Append("<span>  <img src='wwwroot\\assets\\img\\Commonimages\\meal.png' style='width: 19px;' title='Meal' /></span>");
                    }
                    else
                    {
                        sbResponse.Append("<span>--</span>");
                    }
                    sbResponse.Append("</td>");
                    sbResponse.Append("</tr>");
                }
                sbResponse.Append("</table>");
                sbResponse.Append("</td>");
                sbResponse.Append("</tr>");

                sbResponse.Append("<tr>");
                sbResponse.Append("<td>" + GetSectorWiseDetailsOB(flightDetails, flightSegDetails,true));
                sbResponse.Append("</td>");
                sbResponse.Append("</tr>");

                if (Trip == "R")
                {
                    sbResponse.Append("<tr>");
                    sbResponse.Append("<td>" + GetSectorWiseDetailsIB(flightDetails, flightSegDetails,true));
                    sbResponse.Append("</td>");
                    sbResponse.Append("</tr>");
                }
                bool IsSingleFare = false;
                if (Trip == "R")
                {
                    var drFareType = fareDetailSeg.Where(f => f.Conn == "I" && f.PaxType == "ADT"); ;
                    if (drFareType.Any())
                    {
                        Decimal dCount = 0;
                        foreach (var dr in drFareType)
                        {
                            dCount = Convert.ToDecimal(dr.Yq.ToString().Trim());
                            dCount += Convert.ToDecimal(dr.Psf.ToString().Trim());
                            dCount += Convert.ToDecimal(dr.Udf.ToString().Trim());
                            dCount += Convert.ToDecimal(dr.Audf.ToString().Trim());
                            dCount += Convert.ToDecimal(dr.Cute.ToString().Trim());
                            dCount += Convert.ToDecimal(dr.Gst.ToString().Trim());
                            dCount += Convert.ToDecimal(dr.Tf.ToString().Trim());
                            dCount += Convert.ToDecimal(dr.Cess.ToString().Trim());
                            dCount += Convert.ToDecimal(dr.Ex.ToString().Trim());
                            dCount += Convert.ToDecimal(dr.Basic.ToString().Trim());

                            if (dCount <= 0)
                            {
                                IsSingleFare = true;

                                break;
                            }
                        }
                    }
                }

                if (HideFare != null && HideFare != "")
                {
                    if (HideFare == "False")
                    {
                        //======Fare Detail partr strat=============//
                        #region
                        sbResponse.Append("<tr id='divfaredetail'>");
                        sbResponse.Append("<td>");

                        sbResponse.Append("<table width='100%' style='font-size: 15px;' cellpadding='10' >");
                        sbResponse.Append("<tr >");
                        sbResponse.Append("<td width='73%' style='border: 1px solid #D6D8E7;'> <b> Service Information </b> ");
                        sbResponse.Append("</td>");
                        sbResponse.Append("<td style='border: 1px solid #D6D8E7;'> <b> Fare Information </b> ");
                        sbResponse.Append("</td>");
                        sbResponse.Append("</tr>");
                        sbResponse.Append("</table>");

                        sbResponse.Append("<table>");
                        sbResponse.Append("<tr>");

                        sbResponse.Append("<td width='45%' valign='top' align='left'>");

                        sbResponse.Append("<table width='100%' style='font-size: 15px;' >");

                        sbResponse.Append("<tbody>");


                        if (paxSegmentDetails != null && paxSegmentDetails.Any())
                        {
                            foreach (var paxSegment in paxSegmentDetails)
                            {
                                if (paxSegment.Conn != null && paxSegment.Conn.Equals("O"))
                                {
                                    string maealbagagge = paxSegment.ChargeType.Equals("M") ? "Meal" : "Baggage";
                                    sbResponse.Append("<tr>");
                                    sbResponse.Append("<td><b>* </b>" + Originn + "-" + Destinationn + " added " + maealbagagge + "<span>&nbsp;" + paxSegment.ChargeDescription + "</span>  <span>" + paxSegment.Charge_Amount + "Rs.</span></td>");
                                    sbResponse.Append("</tr>");
                                }
                                else
                                {
                                    string maealbagagge = paxSegment.ChargeType.Equals("M") ? "Meal" : "Baggage";
                                    sbResponse.Append("<tr>");
                                    sbResponse.Append("<td><b>* </b>" + Destinationn + "-" + Originn + " added " + maealbagagge + "<span>&nbsp;" + paxSegment.ChargeDescription + "</span>  <span>" + paxSegment.Charge_Amount + "Rs.</span></td>");
                                    sbResponse.Append("</tr>");
                                }
                            }
                        }
                        if (paxSegmentRuleDetails != null && paxSegmentRuleDetails.Any())
                        {
                            foreach (var paxSegment in paxSegmentRuleDetails)
                            {
                                if (paxSegment.Conn != null && paxSegment.Conn == "O")
                                {
                                    sbResponse.Append("<tr>");
                                    if (paxSegment.BaggageDetail != null && paxSegment.BaggageDetail.IndexOf("*") != -1)
                                    {
                                        string bagg = paxSegment.BaggageDetail;
                                        sbResponse.Append("<td><b>* </b>" + Originn + "-" + Destinationn + " available baggage on per pax  <span>" + bagg.Split('*')[0].Trim() + " Check IN </span>  </td>");
                                    }
                                    else
                                    {
                                        sbResponse.Append("<td><b>* </b>" + Originn + "-" + Destinationn + " available baggage on per pax  <span>Check IN: APAP </span>  </td>");
                                    }
                                    sbResponse.Append("</tr>");
                                }
                                else
                                {
                                    sbResponse.Append("<tr>");
                                    if (paxSegment.BaggageDetail != null && paxSegment.BaggageDetail.IndexOf("*") != -1)
                                    {
                                        string bagg = paxSegment.BaggageDetail;
                                        sbResponse.Append("<td><b>* </b>" + Destinationn + "-" + Originn + " available baggage on per pax  <span>" + bagg.Split('*')[0].Trim() + " Check IN </span>  </td>");
                                    }
                                    else
                                    {
                                        sbResponse.Append("<td><b>* </b>" + Destinationn + "-" + Originn + " available baggage on per pax  <span>Check IN: APAP </span>  </td>");
                                    }
                                    sbResponse.Append("</tr>");
                                }
                            }
                        }

                        sbResponse.Append("<tr>");
                        sbResponse.Append("<td>This is an Electronic ticket. Please carry a positive identification for Check in.</td>");
                        sbResponse.Append("</tr>");

                        sbResponse.Append("</tbody>");
                        sbResponse.Append("</table>");
                        sbResponse.Append("</td>");

                        sbResponse.Append("<td width='28%' valign='top' align='right'>");
                        if (flightDetails != null && flightDetails.Any())
                        {
                            string adt = flightDetails.FirstOrDefault().Adt.ToString().Trim();
                            string chd = flightDetails.FirstOrDefault().Chd.ToString().Trim();
                            string inf = flightDetails.FirstOrDefault().Inf.ToString().Trim();
                            int TotalServiceFee = NumberHelper.ConvertToInt(flightDetails.FirstOrDefault().TotalServiceFeeDeal);
                            int TotalServiceTax = NumberHelper.ConvertToInt(flightDetails.FirstOrDefault().TotalServiceTax);
                            int TotalMealcharges = NumberHelper.ConvertToInt(flightDetails.FirstOrDefault().TotalMeal);
                            int TotalBaggCharges = NumberHelper.ConvertToInt(flightDetails.FirstOrDefault().TotalBaggage);
                            int TotalFare = NumberHelper.ConvertToInt(flightDetails.FirstOrDefault().TotalFare);
                            int TotalTDS = NumberHelper.ConvertToInt(flightDetails.FirstOrDefault().TotalTds);
                            int TotalMarkup = NumberHelper.ConvertToInt(flightDetails.FirstOrDefault().TotalMarkup);
                            int TotalBasic = NumberHelper.ConvertToInt(flightDetails.FirstOrDefault().TotalBasic);
                            int TotalTax = NumberHelper.ConvertToInt(flightDetails.FirstOrDefault().TotalTax) + TotalMarkup;
                            int TotalCommission = NumberHelper.ConvertToInt(flightDetails.FirstOrDefault().TotalCommission);
                            TotalTax += TotalMarkup;

                            TotalFare = (TotalFare + TotalMealcharges + TotalBaggCharges + SurchargeAmount) - (TotalTDS);

                            if (Tax != null && Tax != "")
                            {
                                if (Trip == "R" && IsSingleFare.Equals(true) || Trip == "O")
                                {
                                    int noofpx = Convert.ToInt32(adt) + Convert.ToInt32(chd);
                                    int tottax = noofpx * Convert.ToInt32(Tax);
                                    TotalTax = TotalTax + tottax;
                                    TotalFare = TotalFare + tottax;
                                }
                                if (flightDetails.FirstOrDefault().Trip.ToString().Equals("R") && IsSingleFare.Equals(false))
                                {
                                    int noofpx = Convert.ToInt32(adt) + Convert.ToInt32(chd);
                                    noofpx = noofpx * 2;
                                    int tottax = noofpx * Convert.ToInt32(Tax);
                                    TotalTax = TotalTax + tottax;
                                    TotalFare = TotalFare + tottax;
                                }
                            }
                            sbResponse.Append("<table width='100%' style='font-size:15px;' >");
                            sbResponse.Append("<tbody>");

                            sbResponse.Append("<tr>");
                            sbResponse.Append("<td align='right'>BFare :</td>");
                            sbResponse.Append("<td align='right'>" + TotalBasic + "" + Currency + "</td>");
                            sbResponse.Append("</tr>");
                            sbResponse.Append("<tr>");
                            sbResponse.Append("<td align='right'>  Tax :  </td>");
                            sbResponse.Append("<td align='right'> <span id='oldtotaltax'>" + TotalTax + "" + Currency + "</span> </td>");
                            sbResponse.Append("</tr>");
                            sbResponse.Append("<tr>");
                            sbResponse.Append("<td align='right'> Fee & Surcharge :  </td>");
                            sbResponse.Append("<td align='right'>" + (TotalServiceFee + TotalServiceTax).ToString() + "" + Currency + "</td>");
                            sbResponse.Append("</tr>");


                            if (Disc != null && Disc != "")
                            {

                                sbResponse.Append("<tr id='Discountdiv'  align='right' >");
                                sbResponse.Append("<td align='right'> Discount(-) :</td>");
                                sbResponse.Append("<td align='right'>Rs <span id='spndiscount'>" + Disc + "" + Currency + "</span></td>");
                                sbResponse.Append("</tr>");
                                TotalFare = TotalFare - Convert.ToInt32(Disc);
                            }
                            sbResponse.Append("<tr>");
                            sbResponse.Append("<td align='right'> Services :  </td>");
                            sbResponse.Append("<td align='right'>" + (TotalMealcharges + TotalBaggCharges).ToString() + "" + Currency + "</td>");
                            sbResponse.Append("</tr>");


                            sbResponse.Append("<tr>");
                            sbResponse.Append("<td align='right'> Conv.fee :  </td>");
                            sbResponse.Append("<td align='right'>" + (SurchargeAmount).ToString() + "" + Currency + "</td>");
                            sbResponse.Append("</tr>");

                            sbResponse.Append("<tr >");
                            sbResponse.Append("<td align='right'> Total Amount :  </td>");
                            sbResponse.Append("<td align='right'><span id='oldtotalamt'>" + TotalFare + "" + Currency + "</span></td>");
                            sbResponse.Append("</tr>");

                            sbResponse.Append("</tbody>");
                            sbResponse.Append("</table>");

                        }
                        sbResponse.Append("</td>");

                        sbResponse.Append("</tr>");


                        sbResponse.Append("</table>");
                        sbResponse.Append("</td>");

                        sbResponse.Append("</tr>");
                        #endregion
                    }      //======Fare Detail partr End=============//
                    else
                    {
                        //======Fare Detail partr new start=============//
                        #region
                        sbResponse.Append("<tr id='divfaredetailnew'>");
                        sbResponse.Append("<td>");

                        sbResponse.Append("<table width='100%' style='' cellpadding='10' >");
                        sbResponse.Append("<tr style='font-size:15px;'>");

                        sbResponse.Append("<td  style='border: 1px solid #D6D8E7;' colspan='2'><b> Service Information </b>");
                        sbResponse.Append("</td>");



                        sbResponse.Append("</tr>");
                        sbResponse.Append("</table>");

                        sbResponse.Append("<table>");
                        sbResponse.Append("<tr>");
                        sbResponse.Append("<td width='45%' valign='top' align='left'>");
                        sbResponse.Append("<table width='100%' style='font-size: 15px;' >");
                        sbResponse.Append("<tbody>");


                        if (paxSegmentDetails.Any())
                        {
                            foreach (var paxSegment in paxSegmentDetails)
                            {
                                if (paxSegment.Conn != null && paxSegment.Conn.Equals("O"))
                                {
                                    string maealbagagge = "";
                                    if (paxSegment.ChargeType.Equals("M"))
                                    {
                                        maealbagagge = "Meal";
                                    }
                                    else
                                    {
                                        maealbagagge = "Baggage";
                                    }

                                    sbResponse.Append("<tr>");
                                    sbResponse.Append("<td><b>* </b>" + Originn + "-" + Destinationn + " added " + maealbagagge + "<span>&nbsp;" + paxSegment.ChargeDescription + "</span>  <span>" + paxSegment.Charge_Amount + "Rs.</span></td>");
                                    sbResponse.Append("</tr>");
                                }
                                else
                                {
                                    string maealbagagge = "";
                                    if (paxSegment.ChargeType.Equals("M"))
                                    {
                                        maealbagagge = "Meal";
                                    }
                                    else
                                    {
                                        maealbagagge = "Baggage";
                                    }

                                    sbResponse.Append("<tr>");
                                    sbResponse.Append("<td><b>* </b>" + Destinationn + "-" + Originn + " added " + maealbagagge + "<span>&nbsp;" + paxSegment.ChargeDescription + "</span>  <span>" + paxSegment.Charge_Amount + "Rs.</span></td>");
                                    sbResponse.Append("</tr>");
                                }
                            }
                        }


                        if (paxSegmentRuleDetails != null && paxSegmentRuleDetails.Any())
                        {
                            foreach (var paxSegment in paxSegmentRuleDetails)
                            {
                                if (paxSegment.Conn != null && paxSegment.Conn.Equals("O"))
                                {
                                    sbResponse.Append("<tr>");
                                    if (paxSegment.BaggageDetail != null && paxSegment.BaggageDetail.IndexOf("*") != -1)
                                    {
                                        string bagg = paxSegment.BaggageDetail;
                                        sbResponse.Append("<td><b>* </b>" + Originn + "-" + Destinationn + " available baggage on per pax  <span>" + bagg.Split('*')[0].Trim() + " Check IN </span>  </td>");
                                    }
                                    else
                                    {
                                        sbResponse.Append("<td><b>* </b>" + Originn + "-" + Destinationn + " available baggage on per pax  <span>Check IN: APAP </span>  </td>");
                                    }
                                    sbResponse.Append("</tr>");
                                }
                                else
                                {
                                    sbResponse.Append("<tr>");
                                    if (paxSegment.BaggageDetail != null && paxSegment.BaggageDetail.IndexOf("*") != -1)
                                    {
                                        string bagg = paxSegment.BaggageDetail;
                                        sbResponse.Append("<td><b>* </b>" + Destinationn + "-" + Originn + " available baggage on per pax  <span>" + bagg.Split('*')[0].Trim() + " Check IN </span>  </td>");
                                    }
                                    else
                                    {
                                        sbResponse.Append("<td><b>* </b>" + Destinationn + "-" + Originn + " available baggage on per pax  <span>Check IN: APAP </span>  </td>");
                                    }
                                    sbResponse.Append("</tr>");
                                }
                            }
                        }
                        sbResponse.Append("<tr>");
                        sbResponse.Append("<td>This is an Electronic ticket. Please carry a positive identification for Check in.</td>");
                        sbResponse.Append("</tr>");
                        sbResponse.Append("</tbody>");
                        sbResponse.Append("</table>");
                        sbResponse.Append("</td>");

                        sbResponse.Append("<td width='28%' valign='top' align='right'>");
                        sbResponse.Append("</td>");

                        sbResponse.Append("</tr>");


                        sbResponse.Append("</table>");
                        sbResponse.Append("</td>");

                        sbResponse.Append("</tr>");
                        #endregion
                        //======Fare Detail partr new End=============//
                    }
                }




                //======GST Detail start=============//
                var gstDetail = bookingDetail.CompanyFlightGSTDetails.FirstOrDefault();
                if (gstDetail != null)
                {


                    sbResponse.Append("<tr>");
                    sbResponse.Append("<td>");
                    sbResponse.Append("<table width='100%' style='border: 1px solid #D6D8E7;' cellspacing='0' cellpadding='3' >");
                    sbResponse.Append("<tr style='background: #F1F0F0; font-size:13px;'>");
                    sbResponse.Append("<td width='73%' style=''>GST Detail");
                    sbResponse.Append("</td>");
                    sbResponse.Append("</tr>");
                    sbResponse.Append("</table>");
                    sbResponse.Append("</td>");
                    sbResponse.Append("</tr>");
                    sbResponse.Append("<tr>");
                    sbResponse.Append("<td>");
                    sbResponse.Append("<table width='100%' style='font-size:11px;' cellspacing='0' cellpadding='3' >");
                    sbResponse.Append("<tr>");
                    sbResponse.Append("<td>Gst CompanyName:</td>");
                    sbResponse.Append("<td>" + StringHelper.GetUpper(gstDetail.GstcompanyName.ToString().Trim()) + "</td>");
                    sbResponse.Append("</tr>");
                    sbResponse.Append("<tr>");
                    sbResponse.Append("<td>Gst Number:</td>");
                    sbResponse.Append("<td>" + StringHelper.GetUpper(gstDetail.Gstnumber.ToString().Trim()) + "</td>");
                    sbResponse.Append("</tr>");
                    sbResponse.Append("</table>");
                    sbResponse.Append("</td>");
                    sbResponse.Append("</tr>");

                }
                //======GST Detail End =============//



                sbResponse.Append("<tr>");
                sbResponse.Append("<td>");
                sbResponse.Append("<table width='100%' style='border: 1px solid #D6D8E7;' cellspacing='0' cellpadding='10' >");
                sbResponse.Append("<tr style=' font-size:15px;'>");
                sbResponse.Append("<td width='73%' style=''> <b> Terms & Conditions </b> ");
                sbResponse.Append("</td>");
                sbResponse.Append("</tr>");
                sbResponse.Append("</table>");
                sbResponse.Append("</td>");
                sbResponse.Append("</tr>");


                sbResponse.Append("<tr>");
                sbResponse.Append("<td>");
                sbResponse.Append("<table width='100%' style='font-size:10px;' cellspacing='0' cellpadding='3' >");
                sbResponse.Append("<tr>");
                sbResponse.Append("<td><b>*</b> All Guests, including children and infants, must present valid identification at check-in.");
                sbResponse.Append("</td>");
                sbResponse.Append("</tr>");
                sbResponse.Append("<tr>");
                sbResponse.Append("<td><b>*</b> Carriage and other services provided by the carrier are subject to conditions of carriage, which are hereby incorporated by reference. These conditions may be obtained from the issuing carrier.");
                sbResponse.Append("</td>");
                sbResponse.Append("</tr>");
                sbResponse.Append("<tr>");
                sbResponse.Append("<td><b>*</b> Changes to your reservation will result in a fee of Airline terms & condition,change plus any difference in the fare between the original fare paid and the fare for the revised booking.");
                sbResponse.Append("</td>");
                sbResponse.Append("</tr>");
                sbResponse.Append("<tr>");
                sbResponse.Append("<td><b>*</b> Cancellation charges as per Airlines terms & condition.");
                sbResponse.Append("</td>");
                sbResponse.Append("</tr>");

                sbResponse.Append("<tr>");
                sbResponse.Append("<td><b>*</b> Airline Free Baggages are per seat only.");
                sbResponse.Append("</td>");
                sbResponse.Append("</tr>");

                sbResponse.Append("<tr>");
                sbResponse.Append("<td><b>*</b> Check your itinerary with airline also,revert with in 2 hour. we are here to help you.");
                sbResponse.Append("</td>");
                sbResponse.Append("</tr>");
                sbResponse.Append("<tr>");
                sbResponse.Append("<td><b>*</b> As per Government guidelines, check-in counters at all airports will now close 45 minutes before departure with immediate effect. Please plan your Airport arrival accordingly.");
                sbResponse.Append("</td>");
                sbResponse.Append("</tr>");
                sbResponse.Append("<tr>");
                sbResponse.Append("<td><b>*</b> Wish You a Happy Journey.");
                sbResponse.Append("</td>");
                sbResponse.Append("</tr>");

                sbResponse.Append("</table>");
                sbResponse.Append("</td>");
                sbResponse.Append("</tr>");
                sbResponse.Append("</table>");
                sbResponse.Append("</div>");

            }
            catch (Exception ex)
            {
                DBLoggerHelper.DBLogAsync("", iBookingRef, "Print_Popup_Tkt", "PrintPopupController", "", "", ex.Message, _addDbLogCommandHandler);
            }
            return sbResponse.ToString();
        }
        public StringBuilder GetFlightTicketPopup(BookingDetailData bookingDetail, int iBookingRef, string SessionCompany)

        {

            StringBuilder sbResponse = new StringBuilder();


            try
            {
                var flightDetails = bookingDetail.CompanyFlightDetailAirlines;
                var flightSegDetails = bookingDetail.CompanyFlightSegmentDetailAirlines;
                var fareDetails = bookingDetail.CompanyFareDetailAirlines?.FirstOrDefault();
                var fareDetailAirline = bookingDetail.CompanyFareDetailAirlines;
                var fareDetailSeg = bookingDetail.CompanyFareDetailSegmentAirlines;
                var paxDetails = bookingDetail.CompanyPaxDetailAirlines;
                var compDetails = bookingDetail.CompanyFlightDetails?.FirstOrDefault();
                var paxSegmentDetails = bookingDetail.CompanyPaxSegmentDetailAirlines;
                var paxSegmentRuleDetails = bookingDetail.CompanyFlightSegmentRuleDetailAirlines;
                string IssueON = string.Empty;
                string Originn = string.Empty;
                string Destinationn = string.Empty;
                string Adt = string.Empty;
                string Chd = string.Empty;
                string Inf = string.Empty;
                string Trip = string.Empty;
                int SurchargeAmount = 0;

                Double dCurrencyValue = 1;
                string Currency = "INR";

                Trip = flightDetails.FirstOrDefault().Trip.ToString().Trim();
                IssueON = StringHelper.GetConvertedDateTime(flightDetails.FirstOrDefault().EventTime.ToString());
                Originn = flightDetails.FirstOrDefault().Origin.ToString().Trim();
                Destinationn = flightDetails.FirstOrDefault().Destination.ToString().Trim();
                Adt = flightDetails.FirstOrDefault().Adt.ToString().Trim();
                Chd = flightDetails.FirstOrDefault().Chd.ToString().Trim();
                Inf = flightDetails.FirstOrDefault().Inf.ToString().Trim();


                string CompanyLogo = string.Empty;
                string CompanyName = string.Empty;
                string Address = string.Empty;
                string Mobile = string.Empty;
                string Email = string.Empty;
                string CompanyID = flightDetails.FirstOrDefault().CompanyId.ToString();

                if (CompanyID.IndexOf("-SA-") != -1 || CompanyID.IndexOf("C-") != -1)
                {
                    if (CompanyID.IndexOf("C-") != -1)
                    {
                        Mobile = compDetails.A_Mobile.ToString();
                        Email = StringHelper.GetLower(compDetails.A_Email.ToString());
                        bool compactiv = Convert.ToBoolean(compDetails.AgencyPrintB2c.ToString());
                        if (compactiv)
                        {
                            CompanyName = StringHelper.GetUpper(compDetails.A_CompanyName.ToString());
                            CompanyLogo = compDetails.A_CompanyLogo.ToString();
                        }
                    }
                    else
                    {
                        CompanyLogo = compDetails.A_CompanyLogo.ToString();
                        CompanyName = StringHelper.GetUpper(compDetails.A_CompanyName.ToString());
                        Address = StringHelper.GetALLFirstCharacterCapital(compDetails.A_Address.ToString());
                        Mobile = compDetails.A_Mobile.ToString();
                        Email = StringHelper.GetLower(compDetails.A_Email.ToString());
                    }
                }
                else
                {
                    CompanyLogo = compDetails.CompanyLogo.ToString();
                    CompanyName = StringHelper.GetUpper(compDetails.CompanyName.ToString());
                    Address = StringHelper.GetALLFirstCharacterCapital(compDetails.Address.ToString());
                    Mobile = compDetails.Mobile.ToString();
                    Email = StringHelper.GetLower(compDetails.Email.ToString());
                }

                sbResponse.Append("<table align='center' style='border: 1px solid #D6D8E7; width: 785px;  font-size: 11px; font-family: math;' cellspacing='3' id='showmndetail' runat='server'>");
                //if (compDetails.Count() > 0)
                //{
                sbResponse.Append("<tr id='logodiv'>");
                sbResponse.Append("<td style='border-bottom: 1px solid #d6d8e7;'>");
                sbResponse.Append("<img id='imgeLogo' src='" + CompanyLogo + "' style='  height: 63px;  width: 218px; '/>");
                sbResponse.Append("</td>");
                sbResponse.Append("</tr>");


                sbResponse.Append("<tr>");
                sbResponse.Append("<td>");
                sbResponse.Append("<table width='100%'  style='border-bottom: 1px solid #D6D8E7;background: #fff;' cellpadding='10' >");
                sbResponse.Append("<tr>");

                sbResponse.Append("<td width='40%'  >");
                sbResponse.Append("<span style='font-size: 15px;   color: #929292;'> <img src='/assets/img/airlogo_square/onwardflight.png' width='20px' /> Onward Flight </span> ");
                sbResponse.Append("<span style='font-size: 15px;'> <b> " + flightDetails.FirstOrDefault().OriginCity.ToString().Trim() + " to </b> </span>");
                sbResponse.Append("<span style='font-size: 15px;'> <b> " + flightDetails.FirstOrDefault().DestinationCity.ToString().Trim() + " </b> </span>");

                sbResponse.Append("</td>");


                sbResponse.Append("<td width='20%'>");
                sbResponse.Append("</td>");

                sbResponse.Append("<td width='40%' style='font-size: 15px;'>");

                sbResponse.Append("<span style='font-size: 15px;' >Airline PNR:</span><span id='spnpnr' style='font-size: 22px;' ><b> " + flightDetails.FirstOrDefault().Airline_Pnr_D.ToString() + " </b> </span><br />");
                if (flightDetails.FirstOrDefault().Airline_Pnr_D.ToString() != flightDetails.FirstOrDefault().Gds_Pnr_D.ToString())
                {
                    sbResponse.Append("<span style='font-size: 15px;' >GDS PNR:</span><span id='spnpnr' style='font-size: 22px;' ><b> " + flightDetails.FirstOrDefault().Gds_Pnr_D.ToString() + " </b> </span><br />");
                }
                sbResponse.Append("</td>");

                sbResponse.Append("</tr>");
                sbResponse.Append("</table>");
                sbResponse.Append("</td>");
                sbResponse.Append("</tr>");

                if (flightDetails.FirstOrDefault().Trip.ToString().Equals("R"))
                {
                    sbResponse.Append("<tr>");
                    sbResponse.Append("<td style=''>");
                    sbResponse.Append("<table width='100%'  style='border-bottom: 1px solid #D6D8E7;background: #fff;' cellpadding='10' >");
                    sbResponse.Append("<tr>");

                    sbResponse.Append("<td width='40%'  >");
                    sbResponse.Append("<span style='font-size: 15px;   color: #929292;'> <img src='/assets/img/airlogo_square/Inwardflight.png' width='20px' /> Return Flight </span> ");
                    sbResponse.Append("<span style='font-size: 15px;'> <b> " + flightDetails.FirstOrDefault().DestinationCity.ToString().Trim() + " to  </b></span>");
                    sbResponse.Append("<span style='font-size: 15px;'> <b> " + flightDetails.FirstOrDefault().OriginCity.ToString().Trim() + " </b> </span>");
                    sbResponse.Append("</td>");


                    sbResponse.Append("<td width='20%'>");
                    sbResponse.Append("</td>");

                    sbResponse.Append("<td width='40%' style='font-size: 15px;'>");

                    sbResponse.Append("<span style='font-size: 15px;' >Airline PNR:</span><span id='spnpnr' style='font-size: 22px;' ><b> " + flightDetails.FirstOrDefault().Airline_Pnr_A.ToString() + "</b> </span><br />");
                    if (flightDetails.FirstOrDefault().Airline_Pnr_D.ToString() != flightDetails.FirstOrDefault().Gds_Pnr_D.ToString())
                    {
                        sbResponse.Append("<span style='font-size: 15px;' >GDS PNR:</span><span id='spnpnr' style='font-size: 22px;' ><b> " + flightDetails.FirstOrDefault().Gds_Pnr_A.ToString() + "</b> </span><br />");
                    }
                    sbResponse.Append("</td>");

                    sbResponse.Append("</tr>");
                    sbResponse.Append("</table>");
                    sbResponse.Append("</td>");
                    sbResponse.Append("</tr>");
                }


                sbResponse.Append("<tr>");
                sbResponse.Append("<td>");
                sbResponse.Append("<table width='100%' style='border-bottom: 1px solid #D6D8E7;' cellpadding='10' >");
                sbResponse.Append("<tr>");
                sbResponse.Append("<td width='33%' style='font-size:15px;' ><span> Booking Ref. </span> <span id='bookinref'> <b> " + iBookingRef.ToString() + " </b> </span> ");
                sbResponse.Append("</td> <td width='33%' align='center' style='font-size: 25px;' >");
                sbResponse.Append("</td>");
                sbResponse.Append("<td width='33%' align='right' style='font-size:15px;' ><span>Created On:</span><span id='pnrheadingIssueOn'> " + IssueON + "</span>  ");
                sbResponse.Append("</td>");
                sbResponse.Append("</tr>");
                sbResponse.Append("</table>");
                sbResponse.Append("</td>");
                sbResponse.Append("</tr>");



                sbResponse.Append("<tr>");
                sbResponse.Append("<td>");
                sbResponse.Append("<table width='100%' cellpadding='10'>");
                sbResponse.Append("<tr>");
                sbResponse.Append("<td width='33%' valign='top' style='font-size: 15px;' >");
                sbResponse.Append("<span style='font-size: 17px;' ><b id='txtCompany_Name'>" + CompanyName + "</b></span><br />");

                sbResponse.Append("<span style='color: #929292;' >Phone: <span id='CMobile'>" + Mobile + "</span></span><br />");
                sbResponse.Append("<span style='color: #929292;'>Email: <span id='CEmail'>" + Email.ToLower() + "</span>");
                sbResponse.Append("</span><br />");
                sbResponse.Append("<span id='txtAddressCompany' style='color: #929292;' >" + Address + "");
                sbResponse.Append("</span>");
                sbResponse.Append("</td>");

                sbResponse.Append("<td width='33%' align='center' >");
                sbResponse.Append("<b style='font-size:27px'> E-Ticket  </b>");
                sbResponse.Append("</td>");

                sbResponse.Append("<td width='33%' valign='top' align='left' style='font-size: 15px;' >");
                sbResponse.Append("");

                if (flightDetails.FirstOrDefault().Trip.ToString().Equals("O"))
                {
                    //sbResponse.Append("<span>PNR:</span><span id='spnpnr'> " + flightDetails.FirstOrDefault().Airline_Pnr_D.ToString() + "</span><br />");
                }
                else
                {
                    //sbResponse.Append("<span >PNR:</span><span id='spnpnr'> " + flightDetails.FirstOrDefault().ToString() + "/" + flightDetails.FirstOrDefault().Airline_Pnr_A.ToString() + "</span><br />");
                }

                string strStatus = Getbookingstatus(flightDetails, paxDetails, fareDetails);

                if (strStatus.Equals("Confirm"))
                {
                    sbResponse.Append("<span style='font-size:18px;'> <img src='/assets/img/airlogo_square/confirmcheck.png' width='22px' /> </span><span id='spnstatus' style='Color:#00e300; font-size: 25px;'> Confirm</span><br />");
                }
                else if (strStatus.Equals("Unconfirm"))
                {
                    sbResponse.Append("<span style='font-size:18px;'> <img src='/assets/img/airlogo_square/unconfirmcross.png' width='22px' /> </span><span id='spnstatus' style='Color:Red; font-size: 25px;'> Unconfirm</span><br />");
                }
                else if (strStatus.Equals("On Hold"))
                {
                    sbResponse.Append("<span style='font-size:18px;'> <img src='/assets/img/airlogo_square/unconfirmcross.png' width='22px' /> </span><span id='spnstatus' style='Color:Red; font-size: 25px;'> On Hold</span><br />");
                }
                else
                {
                    sbResponse.Append("<span style='font-size:18px;'> <img src='/assets/img/airlogo_square/unconfirmcross.png' width='22px' /> </span><span id='spnstatus' style='Color:Red; font-size: 25px;'> " + strStatus + "</span><br />");
                }
                if (paxDetails != null && paxDetails.Any())
                {
                    sbResponse.Append("<span style='color: #929292;' >MobileNo:</span><span id='spnpaxmobile' style='color: #929292;' > " + paxDetails.FirstOrDefault().MobileNo.ToString().Trim() + "</span><br />");
                    sbResponse.Append("<span style='color: #929292;' >Email:</span><span id='spanpaxEmail' style='color: #929292;' > " + StringHelper.GetLower(paxDetails.FirstOrDefault().Email.ToString().Trim().ToLower()) + "</span>");
                }

                sbResponse.Append("</td>");
                sbResponse.Append("</tr>");
                sbResponse.Append("</table>");
                sbResponse.Append("</td>");
                sbResponse.Append("</tr>");


                sbResponse.Append("<tr>");
                sbResponse.Append("<td>");
                sbResponse.Append("<table width='100%' style='border: 1px solid #D6D8E7;' cellspacing='0' cellpadding='10'>");
                sbResponse.Append("<tr style='font-size:15px;'>");
                sbResponse.Append("<td width='34%' style='border-bottom: 1px solid #D6D8E7; font-size:15px;' > <b> Passengers </b> </td>");
                sbResponse.Append("<td width='27%' style='border-bottom: 1px solid #D6D8E7; font-size:15px;' > <b> Ticket Number </b> </td>");
                sbResponse.Append("<td width='20%' style='border-bottom: 1px solid #D6D8E7; font-size:15px;' > <b> Frequent flyer no. </b> </td>");
                sbResponse.Append("<td width='16%' style='border-bottom: 1px solid #D6D8E7; font-size:15px;' align='center' > <b> Services </b> </td>");
                sbResponse.Append("</tr>");

                foreach (var paxItem in paxDetails)
                {

                    string dob = "--";
                    string fnn = "--";
                    string Meal = string.Empty;
                    string Bagg = string.Empty;
                    string fullname = StringHelper.GetUpper(paxItem.Title.ToString().Trim() + " " + paxItem.First_Name.ToString().Trim() + " " + paxItem.Middle_Name.ToString().Trim() + " " + paxItem.Last_Name.ToString().Trim());
                    string tktno = "--";
                    if (paxItem.TicketNo.ToString().Trim() != "")
                    {
                        tktno = paxItem.TicketNo.ToString().Trim();
                    }
                    if (paxItem.Ffn.ToString().Trim() != "")
                    {
                        fnn = paxItem.Ffn.ToString().Trim();
                    }
                    if (paxItem.Pax_SegmentId.ToString().Trim() != "")
                    {
                        string paxsegid = paxItem.Pax_SegmentId.ToString().Trim();
                        Meal = GetMealDetails(paxsegid, paxSegmentDetails);
                        Bagg = GetBaggDetails(paxsegid, paxSegmentDetails);
                    }
                    string PaxType = "";
                    if (paxItem.PaxType.ToString().Trim() != "")
                    {
                        PaxType = paxItem.PaxType.ToString().Trim();
                    }
                    if (paxItem.Dob.ToString().Trim() != "")
                    {
                        dob = paxItem.Dob.ToString().Trim();
                    }
                    sbResponse.Append("<tr>");
                    sbResponse.Append("<td style='font-size:15px;'>" + fullname + "</td>");
                    sbResponse.Append("<td style='font-size:15px;'>" + tktno + "</td>");
                    sbResponse.Append("<td style='font-size:15px;'>" + fnn + "</td>");
                    sbResponse.Append("<td style='font-size:15px;' align='center'>");
                    if (Bagg != "")
                    {
                        sbResponse.Append("<span><img src='/assets/img/Commonimages/baggage.png' title='Baggage' style='width: 16px;' /></span>");
                    }
                    else
                    {
                        sbResponse.Append("<span>--</span>");
                    }
                    if (Meal != "")
                    {
                        sbResponse.Append("<span>  <img src='/assets/img/Commonimages/meal.png' style='width: 19px;' title='Meal' /></span>");
                    }
                    else
                    {
                        sbResponse.Append("<span>--</span>");
                    }
                    sbResponse.Append("</td>");
                    sbResponse.Append("</tr>");
                }
                //}
                sbResponse.Append("</table>");
                sbResponse.Append("</td>");
                sbResponse.Append("</tr>");
                sbResponse.Append("<tr>");
                sbResponse.Append("<td>" + GetSectorWiseDetailsOB(flightDetails, flightSegDetails));
                sbResponse.Append("</td>");
                sbResponse.Append("</tr>");

                if (Trip == "R")
                {
                    sbResponse.Append("<tr>");
                    sbResponse.Append("<td>" + GetSectorWiseDetailsIB(flightDetails, flightSegDetails));
                    sbResponse.Append("</td>");
                    sbResponse.Append("</tr>");
                }

                bool IsSingleFare = false;
                if (Trip == "R")
                {
                    var drFareType = fareDetailSeg.Where(f => f.Conn == "I" && f.PaxType == "ADT"); ;
                    if (drFareType.Any())
                    {
                        Decimal dCount = 0;
                        foreach (var dr in drFareType)
                        {
                            dCount = Convert.ToDecimal(dr.Yq.ToString().Trim());
                            dCount += Convert.ToDecimal(dr.Psf.ToString().Trim());
                            dCount += Convert.ToDecimal(dr.Udf.ToString().Trim());
                            dCount += Convert.ToDecimal(dr.Audf.ToString().Trim());
                            dCount += Convert.ToDecimal(dr.Cute.ToString().Trim());
                            dCount += Convert.ToDecimal(dr.Gst.ToString().Trim());
                            dCount += Convert.ToDecimal(dr.Tf.ToString().Trim());
                            dCount += Convert.ToDecimal(dr.Cess.ToString().Trim());
                            dCount += Convert.ToDecimal(dr.Ex.ToString().Trim());
                            dCount += Convert.ToDecimal(dr.Basic.ToString().Trim());

                            if (dCount <= 0)
                            {
                                IsSingleFare = true;

                                break;
                            }
                        }
                    }
                }



                //============Fare Breakup Start==============//
                #region
                sbResponse.Append("<tr id='divfarebreakup' style='width: 100%; display:none'>");
                sbResponse.Append("<td>");
                sbResponse.Append("<table width='100%' cellpadding='10' >");
                sbResponse.Append("<tr>");
                sbResponse.Append("<td>");
                sbResponse.Append("<table width='100%' style='' cellpadding='10' >");
                sbResponse.Append("<tr style='font-size:15px;'>");

                if (Trip == "R" && IsSingleFare || Trip == "O")
                {
                    sbResponse.Append("<td width='50%' style='border: 1px solid #D6D8E7;'> Farebreakup ");
                }
                else
                {
                    sbResponse.Append("<td width='50%' style='border: 1px solid #D6D8E7;'> Outbound Farebreakup ");
                }

                sbResponse.Append("</td>");
                if (flightDetails.FirstOrDefault().Trip.ToString().Equals("R") && !IsSingleFare)
                {
                    sbResponse.Append("<td style='border: 1px solid #D6D8E7;'width='50%'> Inbound Farebreakup ");
                    sbResponse.Append("</td>");
                }
                else
                {
                    sbResponse.Append("<td style='border: 1px solid #D6D8E7;'width='50%'>");
                    sbResponse.Append("</td>");
                }

                sbResponse.Append("</tr>");
                sbResponse.Append("</table>");
                sbResponse.Append("</td>");
                sbResponse.Append("</tr>");



                sbResponse.Append("<tr>");
                sbResponse.Append("<td>");

                sbResponse.Append("<table width='100%'>");
                int _totalfare = 0;
                sbResponse.Append("<tr>");
                sbResponse.Append("<td width='50%'>");
                sbResponse.Append("<table >");
                sbResponse.Append("<tr>");
                sbResponse.Append("<td width='120px' >Passenger ");
                sbResponse.Append("</td>");
                sbResponse.Append("<td width='70px'>Basic ");
                sbResponse.Append("</td>");
                sbResponse.Append("<td width='60px'>Yq ");
                sbResponse.Append("</td>");
                sbResponse.Append("<td width='50px'>Tax ");
                sbResponse.Append("</td>");
                sbResponse.Append("<td width='70px'>Total ");
                sbResponse.Append("</td>");
                sbResponse.Append("</tr>");
                sbResponse.Append("</table >");
                sbResponse.Append("</td>");

                if (flightDetails.FirstOrDefault().Trip.ToString().Equals("R") && !IsSingleFare)
                {
                    sbResponse.Append("<td width='50%'>");
                    sbResponse.Append("<table >");
                    sbResponse.Append("<tr>");
                    sbResponse.Append("<td width='120px'>Passenger ");
                    sbResponse.Append("</td>");
                    sbResponse.Append("<td width='70px'>Basic ");
                    sbResponse.Append("</td>");
                    sbResponse.Append("<td width='60px'>Yq ");
                    sbResponse.Append("</td>");
                    sbResponse.Append("<td width='50px'>Tax ");
                    sbResponse.Append("</td>");
                    sbResponse.Append("<td width='70px'>Total ");
                    sbResponse.Append("</td>");
                    sbResponse.Append("</tr>");
                    sbResponse.Append("</table >");
                    sbResponse.Append("</td>");
                }
                else
                {
                    //sbResponse.Append("<td width='50%'>");
                    //sbResponse.Append("<table >");
                    //sbResponse.Append("<tr>");
                    //sbResponse.Append("<td>Passenger ");
                    //sbResponse.Append("</td>");
                    //sbResponse.Append("<td>Basic ");
                    //sbResponse.Append("</td>");
                    //sbResponse.Append("<td>Yq ");
                    //sbResponse.Append("</td>");
                    //sbResponse.Append("<td>Tax ");
                    //sbResponse.Append("</td>");
                    //sbResponse.Append("<td>Total ");
                    //sbResponse.Append("</td>");
                    //sbResponse.Append("</tr>");
                    //sbResponse.Append("</table >");
                    //sbResponse.Append("</td>");
                }

                sbResponse.Append("</tr>");
                //heading

                ArrayList ar_PaxType = new ArrayList();
                ar_PaxType.Add("ADT");
                ar_PaxType.Add("CHD");
                ar_PaxType.Add("INF");

                for (int i = 0; i < ar_PaxType.Count; i++)
                {

                    var fareSegs = fareDetailSeg.ToList().Where(f => f.PaxType == ar_PaxType[i].ToString() && f.Conn == "O");
                    if (fareSegs.Any())
                    {

                        sbResponse.Append("<tr>");
                        var fareSeg = fareSegs.FirstOrDefault();
                        string PaxType = fareSeg.PaxType.ToString().Trim();
                        int yq = Convert.ToInt32(Convert.ToDouble(fareSeg.Yq.ToString().Trim()));
                        int Psf = Convert.ToInt32(Convert.ToDouble(fareSeg.Psf.ToString().Trim()));
                        int Udf = Convert.ToInt32(Convert.ToDouble(fareSeg.Udf.ToString().Trim()));
                        int AUDF = Convert.ToInt32(Convert.ToDouble(fareSeg.Audf.ToString().Trim()));
                        int Cute = Convert.ToInt32(Convert.ToDouble(fareSeg.Cute.ToString().Trim()));
                        int Gst = Convert.ToInt32(Convert.ToDouble(fareSeg.Gst.ToString().Trim()));
                        int TF = Convert.ToInt32(Convert.ToDouble(fareSeg.Tf.ToString().Trim()));
                        int Cess = Convert.ToInt32(Convert.ToDouble(fareSeg.Cess.ToString().Trim()));
                        int Ex = Convert.ToInt32(Convert.ToDouble(fareSeg.Ex.ToString().Trim()));
                        int Basic = Convert.ToInt32(Convert.ToDouble(fareSeg.Basic.ToString().Trim()));
                        int ServiceTax = Convert.ToInt32(Convert.ToDouble(fareSeg.ServiceTax.ToString().Trim()));
                        int Markup = Convert.ToInt32(Convert.ToDouble(fareSeg.Markup.ToString().Trim()));
                        int Service_Fee = Convert.ToInt32(Convert.ToDouble(fareSeg.Service_Fee.ToString().Trim()));
                        int totalTax = 0;
                        string totalFare = "0";
                        if (PaxType == "INF")
                        {
                            totalTax = Psf + Udf + AUDF + Cute + Gst + TF + Cess + Ex;
                        }
                        else
                        {
                            totalTax = Psf + Udf + AUDF + Cute + Gst + TF + Cess + Ex + Markup;
                        }

                        totalFare = (Basic + yq + totalTax).ToString();
                        if ((totalTax + Basic) != 0)
                        {
                            _totalfare = Convert.ToInt32(totalFare) + _totalfare;
                            if (PaxType == "ADT")
                            {
                                PaxType = PaxType + " X " + Adt;
                                totalFare = totalFare + " X " + Adt;
                            }
                            if (PaxType == "CHD")
                            {
                                PaxType = PaxType + " X " + Chd;
                                totalFare = totalFare + " X " + Chd;
                            }
                            if (PaxType == "INF")
                            {
                                PaxType = PaxType + " X " + Inf;
                                totalFare = totalFare + " X " + Inf;
                            }
                        }
                        sbResponse.Append("<td width='50%'>");
                        sbResponse.Append("<table >");
                        sbResponse.Append("<tr>");
                        sbResponse.Append("<td width='120px'>" + PaxType);
                        sbResponse.Append("</td>");
                        sbResponse.Append("<td width='70px'>" + Basic);
                        sbResponse.Append("</td>");
                        sbResponse.Append("<td width='60px'>" + yq);
                        sbResponse.Append("</td>");
                        if (fareSeg.PaxType.ToString().Trim() == "ADT")
                        {
                            sbResponse.Append("<td width='50px' id='farebreakuptaxOB' class='frebrkuptsx'>" + totalTax);
                            sbResponse.Append("</td>");
                            sbResponse.Append("<td width='70px' id='farebreakuptotfareOB' class='frebrkuptotl'>" + totalFare);
                            sbResponse.Append("</td>");
                        }
                        if (fareSeg.PaxType.ToString().Trim() == "CHD")
                        {
                            sbResponse.Append("<td width='50px' id='farebreakuptaxOB' class='frebrkuptsx'>" + totalTax);
                            sbResponse.Append("</td>");
                            sbResponse.Append("<td width='70px' id='farebreakuptotfareOB' class='frebrkuptotl'>" + totalFare);
                            sbResponse.Append("</td>");
                        }
                        if (fareSeg.PaxType.ToString().Trim() == "INF")
                        {
                            sbResponse.Append("<td width='50px'>" + totalTax);
                            sbResponse.Append("</td>");
                            sbResponse.Append("<td width='70px'>" + totalFare);
                            sbResponse.Append("</td>");
                        }
                        sbResponse.Append("</tr>");
                        sbResponse.Append("</table >");
                        sbResponse.Append("</td>");

                        if (!IsSingleFare)
                        {
                            var fareSegList = fareSegs.ToList();

                            var filteredFareSeg = fareSegList.Where(f => f.Conn == "I").ToList();
                            if (filteredFareSeg.Any())
                            {
                                var fareSegItem = filteredFareSeg.First();
                                PaxType = fareSegItem.PaxType.ToString().Trim();
                                yq = Convert.ToInt32(Convert.ToDouble(fareSegItem.Yq.ToString().Trim()));
                                Psf = Convert.ToInt32(Convert.ToDouble(fareSegItem.Psf.ToString().Trim()));
                                Udf = Convert.ToInt32(Convert.ToDouble(fareSegItem.Udf.ToString().Trim()));
                                AUDF = Convert.ToInt32(Convert.ToDouble(fareSegItem.Audf.ToString().Trim()));
                                Cute = Convert.ToInt32(Convert.ToDouble(fareSegItem.Cute.ToString().Trim()));
                                Gst = Convert.ToInt32(Convert.ToDouble(fareSegItem.Gst.ToString().Trim()));
                                TF = Convert.ToInt32(Convert.ToDouble(fareSegItem.Tf.ToString().Trim()));
                                Cess = Convert.ToInt32(Convert.ToDouble(fareSegItem.Cess.ToString().Trim()));
                                Ex = Convert.ToInt32(Convert.ToDouble(fareSegItem.Ex.ToString().Trim()));
                                Basic = Convert.ToInt32(Convert.ToDouble(fareSegItem.Basic.ToString().Trim()));
                                ServiceTax = Convert.ToInt32(Convert.ToDouble(fareSegItem.ServiceTax.ToString().Trim()));
                                Markup = Convert.ToInt32(Convert.ToDouble(fareSegItem.Markup.ToString().Trim()));
                                Service_Fee = Convert.ToInt32(Convert.ToDouble(fareSegItem.Service_Fee.ToString().Trim()));
                                totalTax = 0;
                                totalFare = "0";
                                if (PaxType == "INF")
                                {
                                    totalTax = Psf + Udf + AUDF + Cute + Gst + TF + Cess + Ex;
                                }
                                else
                                {
                                    totalTax = Psf + Udf + AUDF + Cute + Gst + TF + Cess + Ex + Markup;
                                }

                                totalFare = (Basic + yq + totalTax).ToString();
                                if ((totalTax + Basic) != 0)
                                {
                                    _totalfare = Convert.ToInt32(totalFare) + _totalfare;
                                    if (PaxType == "ADT")
                                    {
                                        PaxType = PaxType + " X " + Adt;
                                        totalFare = totalFare + " X " + Adt;
                                    }
                                    if (PaxType == "CHD")
                                    {
                                        PaxType = PaxType + " X " + Chd;
                                        totalFare = totalFare + " X " + Chd;
                                    }
                                    if (PaxType == "INF")
                                    {
                                        PaxType = PaxType + " X " + Inf;
                                        totalFare = totalFare + " X " + Inf;
                                    }
                                }
                                sbResponse.Append("<td width='50%'>");
                                sbResponse.Append("<table >");
                                sbResponse.Append("<tr>");
                                sbResponse.Append("<td width='120px'>" + PaxType);
                                sbResponse.Append("</td>");
                                sbResponse.Append("<td width='70px'>" + Basic);
                                sbResponse.Append("</td>");
                                sbResponse.Append("<td width='60px'>" + yq);
                                sbResponse.Append("</td>");
                                if (fareSegItem.PaxType.ToString().Trim() == "ADT")
                                {
                                    sbResponse.Append("<td width='50px' id='farebreakuptaxIB' class='frebrkuptsx'>" + totalTax);
                                    sbResponse.Append("</td>");
                                    sbResponse.Append("<td width='70px' id='farebreakuptotfareIB' class='frebrkuptotl'>" + totalFare);
                                    sbResponse.Append("</td>");
                                }
                                if (fareSegItem.PaxType.ToString().Trim() == "CHD")
                                {
                                    sbResponse.Append("<td width='50px' id='farebreakuptaxIB' class='frebrkuptsx'>" + totalTax);
                                    sbResponse.Append("</td>");
                                    sbResponse.Append("<td width='70px' id='farebreakuptotfareIB' class='frebrkuptotl'>" + totalFare);
                                    sbResponse.Append("</td>");
                                }
                                if (fareSegItem.PaxType.ToString().Trim() == "INF")
                                {
                                    sbResponse.Append("<td width='50px'>" + totalTax);
                                    sbResponse.Append("</td>");
                                    sbResponse.Append("<td width='70px'>" + totalFare);
                                    sbResponse.Append("</td>");
                                }
                                sbResponse.Append("</tr>");
                                sbResponse.Append("</table >");
                                sbResponse.Append("</td>");
                            }
                        }

                        sbResponse.Append("</tr>");
                    }
                }

                sbResponse.Append("</table>");

                sbResponse.Append("</td>");
                sbResponse.Append("</tr>");

                #endregion
                //============Fare Breakup End==============//

                sbResponse.Append("</table>");

                sbResponse.Append("</td>");
                sbResponse.Append("</tr>");

                //======Fare Detail partr start=============//
                #region
                sbResponse.Append("<tr id='divfaredetail' style='width: 100%; font-size:15px;'>");
                sbResponse.Append("<td>");

                sbResponse.Append("<table width='100%' style='' cellpadding='10' >");
                sbResponse.Append("<tr style='font-size:15px;'>");

                sbResponse.Append("<td width='73%' style='border: 1px solid #D6D8E7;'> <b> Service Information </b>");
                sbResponse.Append("</td>");

                sbResponse.Append("<td style='border: 1px solid #D6D8E7;'> <b> Fare Information  </b>");
                sbResponse.Append("</td>");

                sbResponse.Append("</tr>");
                sbResponse.Append("</table>");

                sbResponse.Append("<table>");
                sbResponse.Append("<tr>");
                sbResponse.Append("<td width='45%' valign='top' align='left' style='font-size:14px;'>");
                sbResponse.Append("<table width='100%' >");
                sbResponse.Append("<tbody>");


                if (paxSegmentDetails != null && paxSegmentDetails.Any())
                {
                    foreach (var paxSegment in paxSegmentDetails)
                    {
                        if (paxSegment.Conn != null && paxSegment.Conn.Equals("O"))
                        {
                            string maealbagagge = paxSegment.ChargeType.Equals("M") ? "Meal" : "Baggage";
                            sbResponse.Append("<tr>");
                            sbResponse.Append("<td><b>* </b>" + Originn + "-" + Destinationn + " added " + maealbagagge + "<span>&nbsp;" + paxSegment.ChargeDescription + "</span>  <span>" + paxSegment.Charge_Amount + "Rs.</span></td>");
                            sbResponse.Append("</tr>");
                        }
                        else
                        {
                            string maealbagagge = paxSegment.ChargeType.Equals("M") ? "Meal" : "Baggage";
                            sbResponse.Append("<tr>");
                            sbResponse.Append("<td><b>* </b>" + Destinationn + "-" + Originn + " added " + maealbagagge + "<span>&nbsp;" + paxSegment.ChargeDescription + "</span>  <span>" + paxSegment.Charge_Amount + "Rs.</span></td>");
                            sbResponse.Append("</tr>");
                        }
                    }
                }




                if (paxSegmentRuleDetails != null && paxSegmentRuleDetails.Any())
                {
                    foreach (var paxSegment in paxSegmentRuleDetails)
                    {
                        if (paxSegment.Conn != null && paxSegment.Conn == "O")
                        {
                            sbResponse.Append("<tr>");
                            if (paxSegment.BaggageDetail != null && paxSegment.BaggageDetail.IndexOf("*") != -1)
                            {
                                string bagg = paxSegment.BaggageDetail;
                                sbResponse.Append("<td><b>* </b>" + Originn + "-" + Destinationn + " available baggage on per pax  <span>" + bagg.Split('*')[0].Trim() + " Check IN </span>  </td>");
                            }
                            else
                            {
                                sbResponse.Append("<td><b>* </b>" + Originn + "-" + Destinationn + " available baggage on per pax  <span>Check IN: APAP </span>  </td>");
                            }
                            sbResponse.Append("</tr>");
                        }
                        else
                        {
                            sbResponse.Append("<tr>");
                            if (paxSegment.BaggageDetail != null && paxSegment.BaggageDetail.IndexOf("*") != -1)
                            {
                                string bagg = paxSegment.BaggageDetail;
                                sbResponse.Append("<td><b>* </b>" + Destinationn + "-" + Originn + " available baggage on per pax  <span>" + bagg.Split('*')[0].Trim() + " Check IN </span>  </td>");
                            }
                            else
                            {
                                sbResponse.Append("<td><b>* </b>" + Destinationn + "-" + Originn + " available baggage on per pax  <span>Check IN: APAP </span>  </td>");
                            }
                            sbResponse.Append("</tr>");
                        }
                    }
                }

                sbResponse.Append("<tr>");
                sbResponse.Append("<td>This is an Electronic ticket. Please carry a positive identification for Check in.</td>");
                sbResponse.Append("</tr>");

                sbResponse.Append("</tbody>");
                sbResponse.Append("</table>");
                sbResponse.Append("</td>");

                sbResponse.Append("<td width='28%' valign='top' align='right'>");



                string adt = flightDetails.FirstOrDefault().Adt.ToString().Trim();
                string chd = flightDetails.FirstOrDefault().Chd.ToString().Trim();
                string inf = flightDetails.FirstOrDefault().Inf.ToString().Trim();
                int TotalServiceFee = NumberHelper.ConvertToInt(flightDetails.FirstOrDefault().TotalServiceFeeDeal);
                int TotalServiceTax = NumberHelper.ConvertToInt(flightDetails.FirstOrDefault().TotalServiceTax);
                int TotalMealcharges = NumberHelper.ConvertToInt(flightDetails.FirstOrDefault().TotalMeal);
                int TotalBaggCharges = NumberHelper.ConvertToInt(flightDetails.FirstOrDefault().TotalBaggage);
                int TotalFare = NumberHelper.ConvertToInt(flightDetails.FirstOrDefault().TotalFare);
                int TotalTDS = NumberHelper.ConvertToInt(flightDetails.FirstOrDefault().TotalTds);
                int TotalMarkup = NumberHelper.ConvertToInt(flightDetails.FirstOrDefault().TotalMarkup);
                int TotalBasic = NumberHelper.ConvertToInt(flightDetails.FirstOrDefault().TotalBasic);
                int TotalTax = NumberHelper.ConvertToInt(flightDetails.FirstOrDefault().TotalTax) + TotalMarkup;
                int TotalCommission = NumberHelper.ConvertToInt(flightDetails.FirstOrDefault().TotalCommission);
                TotalTax += TotalMarkup;

                TotalFare = (TotalFare + TotalMealcharges + TotalBaggCharges + SurchargeAmount) - (TotalTDS);
                sbResponse.Append("<table width='100%'>");
                sbResponse.Append("<tbody>");

                sbResponse.Append("<tr>");
                sbResponse.Append("<td align='right'>BFare :</td>");
                sbResponse.Append("<td align='right'>" + TotalBasic + "" + Currency + "</td>");
                sbResponse.Append("</tr>");
                sbResponse.Append("<tr>");
                sbResponse.Append("<td align='right'>  Taxes :  </td>");
                sbResponse.Append("<td align='right'> <span id='oldtotaltax'>" + TotalTax + "" + Currency + "</span> </td>");
                sbResponse.Append("</tr>");
                sbResponse.Append("<tr>");
                sbResponse.Append("<td align='right'> Fee & Surcharge :  </td>");
                sbResponse.Append("<td align='right'>" + (TotalServiceFee + TotalServiceTax).ToString() + "" + Currency + "</td>");
                sbResponse.Append("</tr>");
                sbResponse.Append("<tr id='divEditTxnFeeDiv' style='display:none;'>");
                sbResponse.Append("<td align='right'>Txn Fee:</td>");
                sbResponse.Append("<td align='right'>Rs. <input type='text' style='width: 80px;' id='CurrentTxnFee' name='CurrentTxnFee' value='0' /></td>");
                sbResponse.Append("</tr>");
                sbResponse.Append("<tr id='divApplyDiscount' style='display:none;'>");
                sbResponse.Append("<td align='right'> Discount(-) :</td>");
                sbResponse.Append("<td align='right'><input type='text' style='width: 80px;' id='CurrentDiscount' name='CurrentDiscount' value='0' /></td>");
                sbResponse.Append("</tr>");
                sbResponse.Append("<tr id='Discountdiv' style='display:none;' align='right' >");
                sbResponse.Append("<td align='right'> Discount(-) :</td>");
                sbResponse.Append("<td align='right'>Rs <span id='spndiscount'>0</span></td>");
                sbResponse.Append("</tr>");
                sbResponse.Append("<tr>");
                sbResponse.Append("<td align='right'> Services :  </td>");
                sbResponse.Append("<td align='right'>" + (TotalMealcharges + TotalBaggCharges).ToString() + "" + Currency + "</td>");
                sbResponse.Append("</tr>");

                sbResponse.Append("<tr>");
                sbResponse.Append("<td align='right'> Conv.fee :  </td>");
                sbResponse.Append("<td align='right'>" + (SurchargeAmount).ToString() + "" + Currency + "</td>");
                sbResponse.Append("</tr>");

                sbResponse.Append("<tr style='font-weight:600;'>");
                sbResponse.Append("<td align='right'> Total Amount :  </td>");
                sbResponse.Append("<td align='right'><span id='oldtotalamt'>" + TotalFare + "" + Currency + "</span></td>");
                sbResponse.Append("</tr>");
                sbResponse.Append("</tbody>");
                sbResponse.Append("</table>");
                //}

                sbResponse.Append("</td>");

                sbResponse.Append("</tr>");


                sbResponse.Append("</table>");
                sbResponse.Append("</td>");

                sbResponse.Append("</tr>");
                #endregion
                //======Fare Detail partr End=============//




                //======Fare Detail partr new start=============//
                #region
                sbResponse.Append("<tr id='divfaredetailnew' style='display:none;'>");
                sbResponse.Append("<td>");

                sbResponse.Append("<table width='100%' style='' cellpadding='10' >");
                sbResponse.Append("<tr style='font-size:15px;'>");

                sbResponse.Append("<td  style='border: 1px solid #D6D8E7;' colspan='2'> <b> Service Information </b>");
                sbResponse.Append("</td>");



                sbResponse.Append("</tr>");
                sbResponse.Append("</table>");

                sbResponse.Append("<table>");
                sbResponse.Append("<tr>");
                sbResponse.Append("<td width='45%' valign='top' align='left' style='font-size:14px;'>");
                sbResponse.Append("<table width='100%' >");
                sbResponse.Append("<tbody>");

                if (paxSegmentDetails.Any())
                {
                    foreach (var paxSegment in paxSegmentDetails)
                    {
                        if (paxSegment.Conn != null && paxSegment.Conn.Equals("O"))
                        {
                            string maealbagagge = "";
                            if (paxSegment.ChargeType.Equals("M"))
                            {
                                maealbagagge = "Meal";
                            }
                            else
                            {
                                maealbagagge = "Baggage";
                            }

                            sbResponse.Append("<tr>");
                            sbResponse.Append("<td><b>* </b>" + Originn + "-" + Destinationn + " added " + maealbagagge + "<span>&nbsp;" + paxSegment.ChargeDescription + "</span>  <span>" + paxSegment.Charge_Amount + "Rs.</span></td>");
                            sbResponse.Append("</tr>");
                        }
                        else
                        {
                            string maealbagagge = "";
                            if (paxSegment.ChargeType.Equals("M"))
                            {
                                maealbagagge = "Meal";
                            }
                            else
                            {
                                maealbagagge = "Baggage";
                            }

                            sbResponse.Append("<tr>");
                            sbResponse.Append("<td><b>* </b>" + Destinationn + "-" + Originn + " added " + maealbagagge + "<span>&nbsp;" + paxSegment.ChargeDescription + "</span>  <span>" + paxSegment.Charge_Amount + "Rs.</span></td>");
                            sbResponse.Append("</tr>");
                        }
                    }
                }

                if (paxSegmentRuleDetails != null && paxSegmentRuleDetails.Any())
                {
                    foreach (var paxSegment in paxSegmentRuleDetails)
                    {
                        if (paxSegment.Conn != null && paxSegment.Conn.Equals("O"))
                        {
                            sbResponse.Append("<tr>");
                            if (paxSegment.BaggageDetail != null && paxSegment.BaggageDetail.IndexOf("*") != -1)
                            {
                                string bagg = paxSegment.BaggageDetail;
                                sbResponse.Append("<td><b>* </b>" + Originn + "-" + Destinationn + " available baggage on per pax  <span>" + bagg.Split('*')[0].Trim() + " Check IN </span>  </td>");
                            }
                            else
                            {
                                sbResponse.Append("<td><b>* </b>" + Originn + "-" + Destinationn + " available baggage on per pax  <span>Check IN: APAP </span>  </td>");
                            }
                            sbResponse.Append("</tr>");
                        }
                        else
                        {
                            sbResponse.Append("<tr>");
                            if (paxSegment.BaggageDetail != null && paxSegment.BaggageDetail.IndexOf("*") != -1)
                            {
                                string bagg = paxSegment.BaggageDetail;
                                sbResponse.Append("<td><b>* </b>" + Destinationn + "-" + Originn + " available baggage on per pax  <span>" + bagg.Split('*')[0].Trim() + " Check IN </span>  </td>");
                            }
                            else
                            {
                                sbResponse.Append("<td><b>* </b>" + Destinationn + "-" + Originn + " available baggage on per pax  <span>Check IN: APAP </span>  </td>");
                            }
                            sbResponse.Append("</tr>");
                        }
                    }
                }

                sbResponse.Append("<tr>");
                sbResponse.Append("<td>This is an Electronic ticket. Please carry a positive identification for Check in.</td>");
                sbResponse.Append("</tr>");
                sbResponse.Append("</tbody>");
                sbResponse.Append("</table>");
                sbResponse.Append("</td>");

                sbResponse.Append("<td width='28%' valign='top' align='right'>");
                sbResponse.Append("</td>");

                sbResponse.Append("</tr>");


                sbResponse.Append("</table>");
                sbResponse.Append("</td>");

                sbResponse.Append("</tr>");
                #endregion
                //======Fare Detail partr new End=============//

                //======GST Detail start=============//
                var gstDetail = bookingDetail.CompanyFlightGSTDetails.FirstOrDefault();
                if (gstDetail != null)
                {

                    sbResponse.Append("<tr>");
                    sbResponse.Append("<td colspan='2'>");
                    sbResponse.Append("<table width='100%' style='border: 1px solid #D6D8E7;' cellspacing='0' cellpadding='10' >");
                    sbResponse.Append("<tr style='font-size:15px;'>");
                    sbResponse.Append("<td width='73%' style=''>GST Detail");
                    sbResponse.Append("</td>");
                    sbResponse.Append("</tr>");
                    sbResponse.Append("</table>");
                    sbResponse.Append("</td>");
                    sbResponse.Append("</tr>");

                    sbResponse.Append("<tr>");
                    sbResponse.Append("<td colspan='2'>");
                    sbResponse.Append("<table width='100%' style='font-size:11px;' cellspacing='0' cellpadding='3' >");
                    sbResponse.Append("<tr>");
                    sbResponse.Append("<td>Gst CompanyName:</td>");
                    sbResponse.Append("<td>" + StringHelper.GetUpper(gstDetail.GstcompanyName.ToString().Trim()) + "</td>");
                    sbResponse.Append("</tr>");
                    sbResponse.Append("<tr>");
                    sbResponse.Append("<td>Gst Number:</td>");
                    sbResponse.Append("<td>" + StringHelper.GetUpper(gstDetail.Gstnumber.ToString().Trim()) + "</td>");
                    sbResponse.Append("</tr>");
                    sbResponse.Append("</table>");
                    sbResponse.Append("</td>");
                    sbResponse.Append("</tr>");
                }
                //======GST Detail End =============//

                sbResponse.Append("<tr>");
                sbResponse.Append("<td colspan='2'>");
                sbResponse.Append("<table width='100%' style='border: 1px solid #D6D8E7;' cellspacing='0' cellpadding='10' >");
                sbResponse.Append("<tr style='font-size:15px;'>");
                sbResponse.Append("<td width='73%' style=''><b> Terms & Conditions </b>");
                sbResponse.Append("</td>");
                sbResponse.Append("</tr>");
                sbResponse.Append("</table>");
                sbResponse.Append("</td>");
                sbResponse.Append("</tr>");

                sbResponse.Append("<tr>");
                sbResponse.Append("<td colspan='2'>");
                sbResponse.Append("<table width='100%' style='font-size:12px;' cellspacing='0' cellpadding='3' >");
                sbResponse.Append("<tr>");
                sbResponse.Append("<td><b>*</b> All Guests, including children and infants, must present valid identification at check-in.");
                sbResponse.Append("</td>");
                sbResponse.Append("</tr>");
                sbResponse.Append("<tr>");
                sbResponse.Append("<td><b>*</b> Carriage and other services provided by the carrier are subject to conditions of carriage, which are hereby incorporated by reference. These conditions may be obtained from the issuing carrier.");
                sbResponse.Append("</td>");
                sbResponse.Append("</tr>");
                sbResponse.Append("<tr>");
                sbResponse.Append("<td><b>*</b> Changes to your reservation will result in a fee of Airline terms & condition,change plus any difference in the fare between the original fare paid and the fare for the revised booking.");
                sbResponse.Append("</td>");
                sbResponse.Append("</tr>");

                sbResponse.Append("<tr>");
                sbResponse.Append("<td><b>*</b> Cancellation charges as per Airlines terms & condition.");
                sbResponse.Append("</td>");
                sbResponse.Append("</tr>");

                sbResponse.Append("<tr>");
                sbResponse.Append("<td><b>*</b> Airline Free Baggages are per seat only.");
                sbResponse.Append("</td>");
                sbResponse.Append("</tr>");


                sbResponse.Append("<tr>");
                sbResponse.Append("<td><b>*</b> Check your itinerary with airline also,revert with in 2 hour. we are here to help you.");
                sbResponse.Append("</td>");
                sbResponse.Append("</tr>");

                sbResponse.Append("<tr>");
                sbResponse.Append("<td><b>*</b> As per Government guidelines, check-in counters at all airports will now close 45 minutes before departure with immediate effect. Please plan your Airport arrival accordingly.");
                sbResponse.Append("</td>");
                sbResponse.Append("</tr>");
                sbResponse.Append("<tr>");
                sbResponse.Append("<td><b>*</b> Wish You a Happy Journey.");
                sbResponse.Append("</td>");
                sbResponse.Append("</tr>");

                sbResponse.Append("</table>");
                sbResponse.Append("</td>");
                sbResponse.Append("</tr>");

                sbResponse.Append("</table>");
                //}
            }
            catch (Exception ex)
            {
                DBLoggerHelper.DBLogAsync("", iBookingRef, "Print_Popup_Tkt", "PrintPopupController", "", "", ex.Message, _addDbLogCommandHandler);
            }
            return sbResponse;
        }



        [Route("PrintTicket")]
        public async Task<IActionResult> PrintTicket([FromQuery] string bookingRef)
        {
            var companyId = User.FindFirst("CompanyId")?.Value;
            if (companyId != null)
            {
                var balanceQuery = new GetAvailableBalanceQuery(companyId);
                var availableBalance = await _getAvailableBalanceQueryHandler.HandleAsync(balanceQuery);
                ViewBag.AvailableBalance = availableBalance;
            }
            if (!string.IsNullOrEmpty(bookingRef) && bookingRef.Trim().Length > 0)
            {
                string bookingRefDecoded = EncodeDecodeHelper.DecodeFrom64(bookingRef.Trim());
                int iBookingRef = 0;

                bool parseSuccess = int.TryParse(bookingRefDecoded, out iBookingRef);

                if (!parseSuccess || iBookingRef <= 0)
                {
                    ViewBag.ErrorMessage = "Invalid booking reference. Please check the provided reference and try again.";
                    return View();
                }

                var query = new BookingDetailQuery { BookingRef = iBookingRef };
                var flightbooking = await _bookingTicketQueryHandler.HandleAsync(query);

                if (flightbooking == null)
                {
                    ViewBag.ErrorMessage = "No booking found for the provided reference. Please check the reference and try again.";
                    return View();
                }

                //clear all booking session

                var sessionKeys = new[]
                {
            "dCurrency", "Curr", "FinalResult", "SearchID", "SelectedFltOut",
            "SelectedFltIn", "PaxXML", "OriginalHotlPSQ", "BOOK_HOTEL",
            "hoteldata", "hotel", "room", "block", "hotelblock",
            "hotelinfo", "paxdetail", "addre", "BOOK", "SearchValue",
            "Guest", "OriginalPSQ", "PSQ" , "SessionResult4MCList"
                };

                foreach (var key in sessionKeys)
                {
                    HttpContext.Session.Remove(key);
                }

                StringBuilder reportHTML = GetPrintTicketPopup(flightbooking, iBookingRef, UserHelper.GetCompanyID(User));
                string hostName = _configuration["SiteURL:BasePath"];

                var viewModel = new PrintPopupResponse
                {
                    BookingRef = bookingRef,
                    ReportTableHTML = !string.IsNullOrEmpty(reportHTML.ToString()) ? reportHTML.ToString() : string.Empty,
                    HdnhostName = hostName,
                    ShowLogo = true,
                    HideFare = false
                };

                bool IsSingleFare = false;
                if (flightbooking.CompanyFlightDetails.FirstOrDefault()?.Trip == "R")
                {
                    var fareDetails = flightbooking.CompanyFareDetailSegmentAirlines
                        .Where(fare => fare.Conn == "I" && fare.PaxType == "ADT");

                    if (fareDetails.Any())
                    {
                        foreach (var fare in fareDetails)
                        {
                            decimal dCount = Convert.ToDecimal(fare.Yq.ToString().Trim())
                                             + Convert.ToDecimal(fare.Psf.ToString().Trim())
                                             + Convert.ToDecimal(fare.Udf.ToString().Trim())
                                             + Convert.ToDecimal(fare.Audf.ToString().Trim())
                                             + Convert.ToDecimal(fare.Cute.ToString().Trim())
                                             + Convert.ToDecimal(fare.Gst.ToString().Trim())
                                             + Convert.ToDecimal(fare.Tf.ToString().Trim())
                                             + Convert.ToDecimal(fare.Cess.ToString().Trim())
                                             + Convert.ToDecimal(fare.Ex.ToString().Trim())
                                             + Convert.ToDecimal(fare.Basic.ToString().Trim());

                            if (dCount <= 0)
                            {
                                IsSingleFare = true;
                                break;
                            }
                        }
                    }
                }

                var firstFlightDetail = flightbooking.CompanyFlightDetails.FirstOrDefault();
                if (firstFlightDetail != null)
                {
                    string tripType = firstFlightDetail.Trip?.ToString();
                    if ((tripType == "R" && IsSingleFare) || tripType == "O")
                    {
                        IsSingleFare = true;
                    }
                }
                viewModel.IsSingleFare = IsSingleFare;
                return View("~/Views/Booking/PrintTicket.cshtml", viewModel);
            }

            ViewBag.ErrorMessage = "Booking reference is required.";
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> PrintTicketExportToPDF([FromBody] PrintPopupPdfRequest printPopupPdfRequest)
        {
            if (!string.IsNullOrEmpty(printPopupPdfRequest.BookingRef?.Trim()))
            {
                string bookingRefDecoded = EncodeDecodeHelper.DecodeFrom64(printPopupPdfRequest.BookingRef.Trim());
                if (int.TryParse(bookingRefDecoded, out int iBookingRef) && iBookingRef > 0)
                {
                    var query = new BookingDetailQuery { BookingRef = iBookingRef };
                    var flightbooking = await _bookingTicketQueryHandler.HandleAsync(query);

                    if (flightbooking != null)
                    {
                        string sessionCompany = UserHelper.GetCompanyID(User);

                        StringBuilder reportHTML = new StringBuilder(PrintTicketPdf(
                            flightbooking,
                            iBookingRef,
                            printPopupPdfRequest.Tax,
                            printPopupPdfRequest.ShowLogo,
                            sessionCompany,
                            printPopupPdfRequest.HideFare,
                            printPopupPdfRequest.Disc
                        ));

                        string baseUri = Directory.GetCurrentDirectory();

                        var margins = (Top: 10f, Right: 10f, Bottom: 0f, Left: 10f);

                        var pdfStream = GeneratePDF.GeneratePdfStream(reportHTML.ToString(), margins, baseUri);

                        return File(pdfStream, "application/pdf", $"Print_{iBookingRef}.pdf");
                    }
                    else
                    {
                        return BadRequest(new { ErrorMessage = "No booking found for the provided reference." });
                    }
                }
                else
                {
                    return BadRequest(new { ErrorMessage = "Invalid booking reference. Please check the provided reference." });
                }
            }
            else
            {
                return BadRequest(new { ErrorMessage = "Booking reference is required." });
            }
        }

        [HttpPost]
        public async Task<IActionResult> SendPrintTicketMail([FromBody] PrintPoupEmailRequest printPoupEmailRequest)
        {
            if (!string.IsNullOrWhiteSpace(printPoupEmailRequest.BookingRef) && !string.IsNullOrWhiteSpace(printPoupEmailRequest.Email))
            {
                var bookingRef = printPoupEmailRequest.BookingRef;
                var email = printPoupEmailRequest.Email;

                string bookingRefDecoded = EncodeDecodeHelper.DecodeFrom64(bookingRef.Trim());
                if (int.TryParse(bookingRefDecoded, out int iBookingRef))
                {
                    var query = new BookingDetailQuery { BookingRef = iBookingRef };
                    var flightbooking = await _bookingTicketQueryHandler.HandleAsync(query);
                    if (flightbooking != null)
                    {

                        var flightDetails = flightbooking.CompanyFlightDetailAirlines;
                        var paxDetails = flightbooking.CompanyPaxDetailAirlines;
                        var fareDetails = flightbooking.CompanyFareDetailAirlines?.FirstOrDefault();

                        string BookingStatus = Getbookingstatus(flightDetails, paxDetails, fareDetails).ToUpper();

                        if (BookingStatus.Equals("REJECTED"))
                        {
                            BookingStatus = "REJECT";
                        }

                        string MailType;
                        if (BookingStatus.Equals("CONFIRM"))
                        {
                            MailType = " Confirmation";
                        }
                        else if (BookingStatus.Equals("REJECT"))
                        {
                            MailType = " Reject";
                        }
                        else if (BookingStatus.Equals("UNCONFIRM"))
                        {
                            MailType = " Confirmation";
                        }
                        else if (BookingStatus.Equals("CANCEL"))
                        {
                            MailType = " Cancellation Request";
                        }
                        else if (BookingStatus.Equals("INV"))
                        {
                            MailType = " Invoice";
                        }
                        else if(BookingStatus.Equals("ON HOLD"))
                        {
                            MailType = " On Hold";
                        }
                        else
                        {
                            MailType = " "; // Provide a default fallback text
                        }

                        string emailSubject = MailType + " of BookingRef: " + iBookingRef;
                        string emailBody = $"<html><body><h4>Please find the attached Booking Ref: {iBookingRef} {MailType} </h4></body></html>";


                        string reportHTML = PrintTicketPdf(
                  flightbooking,
                  iBookingRef,
                  printPoupEmailRequest.Tax,
                  printPoupEmailRequest.ShowLogo,
                  UserHelper.GetCompanyID(User),
                  printPoupEmailRequest.HideFare,
                  printPoupEmailRequest.Disc
              );
                        string baseUri = Directory.GetCurrentDirectory();
                        var margins = (Top: 10f, Right: 10f, Bottom: 0f, Left: 10f);
                        var pdfStream = GeneratePDF.GeneratePdfStream(reportHTML, margins, baseUri);
                        byte[] pdfBytes;
                        using (var memoryStream = new MemoryStream())
                        {
                            pdfStream.CopyTo(memoryStream);
                            pdfBytes = memoryStream.ToArray();
                        }

                        bool emailSent = await EmailService.SendEmail(email, emailSubject, emailBody, pdfBytes, $"ETicket_{iBookingRef}.pdf");

                        if (emailSent)
                        {
                            return Json(new { success = true, message = "Email sent successfully." });
                        }
                        else
                        {
                            return Json(new { success = false, message = "Failed to send email. Please try again later." });
                        }
                    }
                    else
                    {
                        return Json(new { success = false, message = "No booking found for the provided reference." });
                    }
                }
                else
                {
                    return Json(new { success = false, message = "Invalid booking reference." });
                }
            }
            else
            {
                return Json(new { success = false, message = "Booking reference and email are required." });
            }
        }
        public StringBuilder GetPrintTicketPopup(BookingDetailData bookingDetail, int iBookingRef, string SessionCompany)

        {

            StringBuilder sbResponse = new StringBuilder();


            try
            {
                var flightDetails = bookingDetail.CompanyFlightDetailAirlines;
                var flightSegDetails = bookingDetail.CompanyFlightSegmentDetailAirlines;
                var fareDetails = bookingDetail.CompanyFareDetailAirlines?.FirstOrDefault();
                var fareDetailAirline = bookingDetail.CompanyFareDetailAirlines;
                var fareDetailSeg = bookingDetail.CompanyFareDetailSegmentAirlines;
                var paxDetails = bookingDetail.CompanyPaxDetailAirlines;
                var compDetails = bookingDetail.CompanyFlightDetails?.FirstOrDefault();
                var paxSegmentDetails = bookingDetail.CompanyPaxSegmentDetailAirlines;
                var paxSegmentRuleDetails = bookingDetail.CompanyFlightSegmentRuleDetailAirlines;
                string IssueON = string.Empty;
                string Originn = string.Empty;
                string Destinationn = string.Empty;
                string Adt = string.Empty;
                string Chd = string.Empty;
                string Inf = string.Empty;
                string Trip = string.Empty;
                int SurchargeAmount = 0;

                Double dCurrencyValue = 1;
                string Currency = "INR";

                Trip = flightDetails.FirstOrDefault().Trip.ToString().Trim();
                IssueON = StringHelper.GetConvertedDateTime(flightDetails.FirstOrDefault().EventTime.ToString());
                Originn = flightDetails.FirstOrDefault().Origin.ToString().Trim();
                Destinationn = flightDetails.FirstOrDefault().Destination.ToString().Trim();
                Adt = flightDetails.FirstOrDefault().Adt.ToString().Trim();
                Chd = flightDetails.FirstOrDefault().Chd.ToString().Trim();
                Inf = flightDetails.FirstOrDefault().Inf.ToString().Trim();


                string CompanyLogo = string.Empty;
                string CompanyName = string.Empty;
                string Address = string.Empty;
                string Mobile = string.Empty;
                string Email = string.Empty;
                string CompanyID = flightDetails.FirstOrDefault().CompanyId.ToString();

                if (CompanyID.IndexOf("-SA-") != -1 || CompanyID.IndexOf("C-") != -1)
                {
                    if (CompanyID.IndexOf("C-") != -1)
                    {
                        Mobile = compDetails.A_Mobile.ToString();
                        Email = StringHelper.GetLower(compDetails.A_Email.ToString());
                        bool compactiv = Convert.ToBoolean(compDetails.AgencyPrintB2c.ToString());
                        if (compactiv)
                        {
                            CompanyName = StringHelper.GetUpper(compDetails.A_CompanyName.ToString());
                            CompanyLogo = compDetails.A_CompanyLogo.ToString();
                        }
                    }
                    else
                    {
                        CompanyLogo = compDetails.A_CompanyLogo.ToString();
                        CompanyName = StringHelper.GetUpper(compDetails.A_CompanyName.ToString());
                        Address = StringHelper.GetALLFirstCharacterCapital(compDetails.A_Address.ToString());
                        Mobile = compDetails.A_Mobile.ToString();
                        Email = StringHelper.GetLower(compDetails.A_Email.ToString());
                    }
                }
                else
                {
                    CompanyLogo = compDetails.CompanyLogo.ToString();
                    CompanyName = StringHelper.GetUpper(compDetails.CompanyName.ToString());
                    Address = StringHelper.GetALLFirstCharacterCapital(compDetails.Address.ToString());
                    Mobile = compDetails.Mobile.ToString();
                    Email = StringHelper.GetLower(compDetails.Email.ToString());
                }

                sbResponse.Append("<table align='center' style='border: 1px solid #D6D8E7; width: 785px;  font-size: 11px; font-family: math;' cellspacing='3' id='showmndetail' runat='server'>");
                //if (compDetails.Count() > 0)
                //{
                sbResponse.Append("<tr id='logodiv'>");
                sbResponse.Append("<td style='border-bottom: 1px solid #d6d8e7;'>");
                sbResponse.Append("<img id='imgeLogo' src='" + CompanyLogo + "' style='  height: 63px;  width: 218px; '/>");
                sbResponse.Append("</td>");
                sbResponse.Append("</tr>");


                sbResponse.Append("<tr>");
                sbResponse.Append("<td>");
                sbResponse.Append("<table width='100%'  style='border-bottom: 1px solid #D6D8E7;background: #fff;' cellpadding='10' >");
                sbResponse.Append("<tr>");

                sbResponse.Append("<td width='40%'  >");
                sbResponse.Append("<span style='font-size: 15px;   color: #929292;'> <img src='/assets/img/airlogo_square/onwardflight.png' width='20px' /> Onward Flight </span> ");
                sbResponse.Append("<span style='font-size: 15px;'> <b> " + flightDetails.FirstOrDefault().OriginCity.ToString().Trim() + " to </b> </span>");
                sbResponse.Append("<span style='font-size: 15px;'> <b> " + flightDetails.FirstOrDefault().DestinationCity.ToString().Trim() + " </b> </span>");

                sbResponse.Append("</td>");


                sbResponse.Append("<td width='20%'>");
                sbResponse.Append("</td>");

                sbResponse.Append("<td width='40%' style='font-size: 15px;'>");

                sbResponse.Append("<span style='font-size: 15px;' >Airline PNR:</span><span id='spnpnr' style='font-size: 22px;' ><b> " + flightDetails.FirstOrDefault().Airline_Pnr_D.ToString() + " </b> </span><br />");
                if (flightDetails.FirstOrDefault().Airline_Pnr_D.ToString() != flightDetails.FirstOrDefault().Gds_Pnr_D.ToString())
                {
                    sbResponse.Append("<span style='font-size: 15px;' >GDS PNR:</span><span id='spnpnr' style='font-size: 22px;' ><b> " + flightDetails.FirstOrDefault().Gds_Pnr_D.ToString() + " </b> </span><br />");
                }
                sbResponse.Append("</td>");

                sbResponse.Append("</tr>");
                sbResponse.Append("</table>");
                sbResponse.Append("</td>");
                sbResponse.Append("</tr>");

                if (flightDetails.FirstOrDefault().Trip.ToString().Equals("R"))
                {
                    sbResponse.Append("<tr>");
                    sbResponse.Append("<td style=''>");
                    sbResponse.Append("<table width='100%'  style='border-bottom: 1px solid #D6D8E7;background: #fff;' cellpadding='10' >");
                    sbResponse.Append("<tr>");

                    sbResponse.Append("<td width='40%'  >");
                    sbResponse.Append("<span style='font-size: 15px;   color: #929292;'> <img src='/assets/img/airlogo_square/Inwardflight.png' width='20px' /> Return Flight </span> ");
                    sbResponse.Append("<span style='font-size: 15px;'> <b> " + flightDetails.FirstOrDefault().DestinationCity.ToString().Trim() + " to  </b></span>");
                    sbResponse.Append("<span style='font-size: 15px;'> <b> " + flightDetails.FirstOrDefault().OriginCity.ToString().Trim() + " </b> </span>");
                    sbResponse.Append("</td>");


                    sbResponse.Append("<td width='20%'>");
                    sbResponse.Append("</td>");

                    sbResponse.Append("<td width='40%' style='font-size: 15px;'>");

                    sbResponse.Append("<span style='font-size: 15px;' >Airline PNR:</span><span id='spnpnr' style='font-size: 22px;' ><b> " + flightDetails.FirstOrDefault().Airline_Pnr_A.ToString() + "</b> </span><br />");
                    if (flightDetails.FirstOrDefault().Airline_Pnr_D.ToString() != flightDetails.FirstOrDefault().Gds_Pnr_D.ToString())
                    {
                        sbResponse.Append("<span style='font-size: 15px;' >GDS PNR:</span><span id='spnpnr' style='font-size: 22px;' ><b> " + flightDetails.FirstOrDefault().Gds_Pnr_A.ToString() + "</b> </span><br />");
                    }
                    sbResponse.Append("</td>");

                    sbResponse.Append("</tr>");
                    sbResponse.Append("</table>");
                    sbResponse.Append("</td>");
                    sbResponse.Append("</tr>");
                }


                sbResponse.Append("<tr>");
                sbResponse.Append("<td>");
                sbResponse.Append("<table width='100%' style='border-bottom: 1px solid #D6D8E7;' cellpadding='10' >");
                sbResponse.Append("<tr>");
                sbResponse.Append("<td width='33%' style='font-size:15px;' ><span> Booking Ref. </span> <span id='bookinref'> <b> " + iBookingRef.ToString() + " </b> </span> ");
                sbResponse.Append("</td> <td width='33%' align='center' style='font-size: 25px;' >");
                sbResponse.Append("</td>");
                sbResponse.Append("<td width='33%' align='right' style='font-size:15px;' ><span>Created On:</span><span id='pnrheadingIssueOn'> " + IssueON + "</span>  ");
                sbResponse.Append("</td>");
                sbResponse.Append("</tr>");
                sbResponse.Append("</table>");
                sbResponse.Append("</td>");
                sbResponse.Append("</tr>");



                sbResponse.Append("<tr>");
                sbResponse.Append("<td>");
                sbResponse.Append("<table width='100%' cellpadding='10'>");
                sbResponse.Append("<tr>");
                sbResponse.Append("<td width='33%' valign='top' style='font-size: 15px;' >");
                sbResponse.Append("<span style='font-size: 17px;' ><b id='txtCompany_Name'>" + CompanyName + "</b></span><br />");

                sbResponse.Append("<span style='color: #929292;' >Phone: <span id='CMobile'>" + Mobile + "</span></span><br />");
                sbResponse.Append("<span style='color: #929292;'>Email: <span id='CEmail'>" + Email.ToLower() + "</span>");
                sbResponse.Append("</span><br />");
                sbResponse.Append("<span id='txtAddressCompany' style='color: #929292;' >" + Address + "");
                sbResponse.Append("</span>");
                sbResponse.Append("</td>");

                sbResponse.Append("<td width='33%' align='center' >");
                sbResponse.Append("<b style='font-size:27px'> E-Ticket  </b>");
                sbResponse.Append("</td>");

                sbResponse.Append("<td width='33%' valign='top' align='left' style='font-size: 15px;' >");
                sbResponse.Append("");

                if (flightDetails.FirstOrDefault().Trip.ToString().Equals("O"))
                {
                    //sbResponse.Append("<span>PNR:</span><span id='spnpnr'> " + flightDetails.FirstOrDefault().Airline_Pnr_D.ToString() + "</span><br />");
                }
                else
                {
                    //sbResponse.Append("<span >PNR:</span><span id='spnpnr'> " + flightDetails.FirstOrDefault().ToString() + "/" + flightDetails.FirstOrDefault().Airline_Pnr_A.ToString() + "</span><br />");
                }

                string strStatus = Getbookingstatus(flightDetails, paxDetails, fareDetails);

                if (strStatus.Equals("Confirm"))
                {
                    sbResponse.Append("<span style='font-size:18px;'> <img src='/assets/img/airlogo_square/confirmcheck.png' width='22px' /> </span><span id='spnstatus' style='Color:#00e300; font-size: 25px;'> Confirm</span><br />");
                }
                else if (strStatus.Equals("Unconfirm"))
                {
                    sbResponse.Append("<span style='font-size:18px;'> <img src='/assets/img/airlogo_square/unconfirmcross.png' width='22px' /> </span><span id='spnstatus' style='Color:Red; font-size: 25px;'> Unconfirm</span><br />");
                }
                else if (strStatus.Equals("On Hold"))
                {
                    sbResponse.Append("<span style='font-size:18px;'> <img src='/assets/img/airlogo_square/unconfirmcross.png' width='22px' /> </span><span id='spnstatus' style='Color:Red; font-size: 25px;'> On Hold</span><br />");
                }
                else
                {
                    sbResponse.Append("<span style='font-size:18px;'> <img src='/assets/img/airlogo_square/unconfirmcross.png' width='22px' /> </span><span id='spnstatus' style='Color:Red; font-size: 25px;'> " + strStatus + "</span><br />");
                }
                if (paxDetails != null && paxDetails.Any())
                {
                    sbResponse.Append("<span style='color: #929292;' >MobileNo:</span><span id='spnpaxmobile' style='color: #929292;' > " + paxDetails.FirstOrDefault().MobileNo.ToString().Trim() + "</span><br />");
                    sbResponse.Append("<span style='color: #929292;' >Email:</span><span id='spanpaxEmail' style='color: #929292;' > " + StringHelper.GetLower(paxDetails.FirstOrDefault().Email.ToString().Trim().ToLower()) + "</span>");
                }

                sbResponse.Append("</td>");
                sbResponse.Append("</tr>");
                sbResponse.Append("</table>");
                sbResponse.Append("</td>");
                sbResponse.Append("</tr>");


                sbResponse.Append("<tr>");
                sbResponse.Append("<td>");
                sbResponse.Append("<table width='100%' style='border: 1px solid #D6D8E7;' cellspacing='0' cellpadding='10'>");
                sbResponse.Append("<tr style='font-size:15px;'>");
                sbResponse.Append("<td width='34%' style='border-bottom: 1px solid #D6D8E7; font-size:15px;' > <b> Passengers </b> </td>");
                sbResponse.Append("<td width='27%' style='border-bottom: 1px solid #D6D8E7; font-size:15px;' > <b> Ticket Number </b> </td>");
                sbResponse.Append("<td width='20%' style='border-bottom: 1px solid #D6D8E7; font-size:15px;' > <b> Frequent flyer no. </b> </td>");
                sbResponse.Append("<td width='16%' style='border-bottom: 1px solid #D6D8E7; font-size:15px;' align='center' > <b> Services </b> </td>");
                sbResponse.Append("</tr>");

                foreach (var paxItem in paxDetails)
                {

                    string dob = "--";
                    string fnn = "--";
                    string Meal = string.Empty;
                    string Bagg = string.Empty;
                    string fullname = StringHelper.GetUpper(paxItem.Title.ToString().Trim() + " " + paxItem.First_Name.ToString().Trim() + " " + paxItem.Middle_Name.ToString().Trim() + " " + paxItem.Last_Name.ToString().Trim());
                    string tktno = "--";
                    if (paxItem.TicketNo.ToString().Trim() != "")
                    {
                        tktno = paxItem.TicketNo.ToString().Trim();
                    }
                    if (paxItem.Ffn.ToString().Trim() != "")
                    {
                        fnn = paxItem.Ffn.ToString().Trim();
                    }
                    if (paxItem.Pax_SegmentId.ToString().Trim() != "")
                    {
                        string paxsegid = paxItem.Pax_SegmentId.ToString().Trim();
                        Meal = GetMealDetails(paxsegid, paxSegmentDetails);
                        Bagg = GetBaggDetails(paxsegid, paxSegmentDetails);
                    }
                    string PaxType = "";
                    if (paxItem.PaxType.ToString().Trim() != "")
                    {
                        PaxType = paxItem.PaxType.ToString().Trim();
                    }
                    if (paxItem.Dob.ToString().Trim() != "")
                    {
                        dob = paxItem.Dob.ToString().Trim();
                    }
                    sbResponse.Append("<tr>");
                    sbResponse.Append("<td style='font-size:15px;'>" + fullname + "</td>");
                    sbResponse.Append("<td style='font-size:15px;'>" + tktno + "</td>");
                    sbResponse.Append("<td style='font-size:15px;'>" + fnn + "</td>");
                    sbResponse.Append("<td style='font-size:15px;' align='center'>");
                    if (Bagg != "")
                    {
                        sbResponse.Append("<span><img src='/assets/img/Commonimages/baggage.png' title='Baggage' style='width: 16px;' /></span>");
                    }
                    else
                    {
                        sbResponse.Append("<span>--</span>");
                    }
                    if (Meal != "")
                    {
                        sbResponse.Append("<span>  <img src='/assets/img/Commonimages/meal.png' style='width: 19px;' title='Meal' /></span>");
                    }
                    else
                    {
                        sbResponse.Append("<span>--</span>");
                    }
                    sbResponse.Append("</td>");
                    sbResponse.Append("</tr>");
                }
                //}
                sbResponse.Append("</table>");
                sbResponse.Append("</td>");
                sbResponse.Append("</tr>");
                sbResponse.Append("<tr>");
                sbResponse.Append("<td>" + GetSectorWiseDetailsOB(flightDetails, flightSegDetails));
                sbResponse.Append("</td>");
                sbResponse.Append("</tr>");

                if (Trip == "R")
                {
                    sbResponse.Append("<tr>");
                    sbResponse.Append("<td>" + GetSectorWiseDetailsIB(flightDetails, flightSegDetails));
                    sbResponse.Append("</td>");
                    sbResponse.Append("</tr>");
                }

                bool IsSingleFare = false;
                if (Trip == "R")
                {
                    var drFareType = fareDetailSeg.Where(f => f.Conn == "I" && f.PaxType == "ADT"); ;
                    if (drFareType.Any())
                    {
                        Decimal dCount = 0;
                        foreach (var dr in drFareType)
                        {
                            dCount = Convert.ToDecimal(dr.Yq.ToString().Trim());
                            dCount += Convert.ToDecimal(dr.Psf.ToString().Trim());
                            dCount += Convert.ToDecimal(dr.Udf.ToString().Trim());
                            dCount += Convert.ToDecimal(dr.Audf.ToString().Trim());
                            dCount += Convert.ToDecimal(dr.Cute.ToString().Trim());
                            dCount += Convert.ToDecimal(dr.Gst.ToString().Trim());
                            dCount += Convert.ToDecimal(dr.Tf.ToString().Trim());
                            dCount += Convert.ToDecimal(dr.Cess.ToString().Trim());
                            dCount += Convert.ToDecimal(dr.Ex.ToString().Trim());
                            dCount += Convert.ToDecimal(dr.Basic.ToString().Trim());

                            if (dCount <= 0)
                            {
                                IsSingleFare = true;

                                break;
                            }
                        }
                    }
                }



                //============Fare Breakup Start==============//
                #region
                sbResponse.Append("<tr id='divfarebreakup' style='width: 100%; display:none'>");
                sbResponse.Append("<td>");
                sbResponse.Append("<table width='100%' cellpadding='10' >");
                sbResponse.Append("<tr>");
                sbResponse.Append("<td>");
                sbResponse.Append("<table width='100%' style='' cellpadding='10' >");
                sbResponse.Append("<tr style='font-size:15px;'>");

                if (Trip == "R" && IsSingleFare || Trip == "O")
                {
                    sbResponse.Append("<td width='50%' style='border: 1px solid #D6D8E7;'> Farebreakup ");
                }
                else
                {
                    sbResponse.Append("<td width='50%' style='border: 1px solid #D6D8E7;'> Outbound Farebreakup ");
                }

                sbResponse.Append("</td>");
                if (flightDetails.FirstOrDefault().Trip.ToString().Equals("R") && !IsSingleFare)
                {
                    sbResponse.Append("<td style='border: 1px solid #D6D8E7;'width='50%'> Inbound Farebreakup ");
                    sbResponse.Append("</td>");
                }
                else
                {
                    sbResponse.Append("<td style='border: 1px solid #D6D8E7;'width='50%'>");
                    sbResponse.Append("</td>");
                }

                sbResponse.Append("</tr>");
                sbResponse.Append("</table>");
                sbResponse.Append("</td>");
                sbResponse.Append("</tr>");



                sbResponse.Append("<tr>");
                sbResponse.Append("<td>");

                sbResponse.Append("<table width='100%'>");
                int _totalfare = 0;
                sbResponse.Append("<tr>");
                sbResponse.Append("<td width='50%'>");
                sbResponse.Append("<table >");
                sbResponse.Append("<tr>");
                sbResponse.Append("<td width='120px' >Passenger ");
                sbResponse.Append("</td>");
                sbResponse.Append("<td width='70px'>Basic ");
                sbResponse.Append("</td>");
                sbResponse.Append("<td width='60px'>Yq ");
                sbResponse.Append("</td>");
                sbResponse.Append("<td width='50px'>Tax ");
                sbResponse.Append("</td>");
                sbResponse.Append("<td width='70px'>Total ");
                sbResponse.Append("</td>");
                sbResponse.Append("</tr>");
                sbResponse.Append("</table >");
                sbResponse.Append("</td>");

                if (flightDetails.FirstOrDefault().Trip.ToString().Equals("R") && !IsSingleFare)
                {
                    sbResponse.Append("<td width='50%'>");
                    sbResponse.Append("<table >");
                    sbResponse.Append("<tr>");
                    sbResponse.Append("<td width='120px'>Passenger ");
                    sbResponse.Append("</td>");
                    sbResponse.Append("<td width='70px'>Basic ");
                    sbResponse.Append("</td>");
                    sbResponse.Append("<td width='60px'>Yq ");
                    sbResponse.Append("</td>");
                    sbResponse.Append("<td width='50px'>Tax ");
                    sbResponse.Append("</td>");
                    sbResponse.Append("<td width='70px'>Total ");
                    sbResponse.Append("</td>");
                    sbResponse.Append("</tr>");
                    sbResponse.Append("</table >");
                    sbResponse.Append("</td>");
                }
                else
                {
                    //sbResponse.Append("<td width='50%'>");
                    //sbResponse.Append("<table >");
                    //sbResponse.Append("<tr>");
                    //sbResponse.Append("<td>Passenger ");
                    //sbResponse.Append("</td>");
                    //sbResponse.Append("<td>Basic ");
                    //sbResponse.Append("</td>");
                    //sbResponse.Append("<td>Yq ");
                    //sbResponse.Append("</td>");
                    //sbResponse.Append("<td>Tax ");
                    //sbResponse.Append("</td>");
                    //sbResponse.Append("<td>Total ");
                    //sbResponse.Append("</td>");
                    //sbResponse.Append("</tr>");
                    //sbResponse.Append("</table >");
                    //sbResponse.Append("</td>");
                }

                sbResponse.Append("</tr>");
                //heading

                ArrayList ar_PaxType = new ArrayList();
                ar_PaxType.Add("ADT");
                ar_PaxType.Add("CHD");
                ar_PaxType.Add("INF");

                for (int i = 0; i < ar_PaxType.Count; i++)
                {

                    var fareSegs = fareDetailSeg.ToList().Where(f => f.PaxType == ar_PaxType[i].ToString() && f.Conn == "O");
                    if (fareSegs.Any())
                    {

                        sbResponse.Append("<tr>");
                        var fareSeg = fareSegs.FirstOrDefault();
                        string PaxType = fareSeg.PaxType.ToString().Trim();
                        int yq = Convert.ToInt32(Convert.ToDouble(fareSeg.Yq.ToString().Trim()));
                        int Psf = Convert.ToInt32(Convert.ToDouble(fareSeg.Psf.ToString().Trim()));
                        int Udf = Convert.ToInt32(Convert.ToDouble(fareSeg.Udf.ToString().Trim()));
                        int AUDF = Convert.ToInt32(Convert.ToDouble(fareSeg.Audf.ToString().Trim()));
                        int Cute = Convert.ToInt32(Convert.ToDouble(fareSeg.Cute.ToString().Trim()));
                        int Gst = Convert.ToInt32(Convert.ToDouble(fareSeg.Gst.ToString().Trim()));
                        int TF = Convert.ToInt32(Convert.ToDouble(fareSeg.Tf.ToString().Trim()));
                        int Cess = Convert.ToInt32(Convert.ToDouble(fareSeg.Cess.ToString().Trim()));
                        int Ex = Convert.ToInt32(Convert.ToDouble(fareSeg.Ex.ToString().Trim()));
                        int Basic = Convert.ToInt32(Convert.ToDouble(fareSeg.Basic.ToString().Trim()));
                        int ServiceTax = Convert.ToInt32(Convert.ToDouble(fareSeg.ServiceTax.ToString().Trim()));
                        int Markup = Convert.ToInt32(Convert.ToDouble(fareSeg.Markup.ToString().Trim()));
                        int Service_Fee = Convert.ToInt32(Convert.ToDouble(fareSeg.Service_Fee.ToString().Trim()));
                        int totalTax = 0;
                        string totalFare = "0";
                        if (PaxType == "INF")
                        {
                            totalTax = Psf + Udf + AUDF + Cute + Gst + TF + Cess + Ex;
                        }
                        else
                        {
                            totalTax = Psf + Udf + AUDF + Cute + Gst + TF + Cess + Ex + Markup;
                        }

                        totalFare = (Basic + yq + totalTax).ToString();
                        if ((totalTax + Basic) != 0)
                        {
                            _totalfare = Convert.ToInt32(totalFare) + _totalfare;
                            if (PaxType == "ADT")
                            {
                                PaxType = PaxType + " X " + Adt;
                                totalFare = totalFare + " X " + Adt;
                            }
                            if (PaxType == "CHD")
                            {
                                PaxType = PaxType + " X " + Chd;
                                totalFare = totalFare + " X " + Chd;
                            }
                            if (PaxType == "INF")
                            {
                                PaxType = PaxType + " X " + Inf;
                                totalFare = totalFare + " X " + Inf;
                            }
                        }
                        sbResponse.Append("<td width='50%'>");
                        sbResponse.Append("<table >");
                        sbResponse.Append("<tr>");
                        sbResponse.Append("<td width='120px'>" + PaxType);
                        sbResponse.Append("</td>");
                        sbResponse.Append("<td width='70px'>" + Basic);
                        sbResponse.Append("</td>");
                        sbResponse.Append("<td width='60px'>" + yq);
                        sbResponse.Append("</td>");
                        if (fareSeg.PaxType.ToString().Trim() == "ADT")
                        {
                            sbResponse.Append("<td width='50px' id='farebreakuptaxOB' class='frebrkuptsx'>" + totalTax);
                            sbResponse.Append("</td>");
                            sbResponse.Append("<td width='70px' id='farebreakuptotfareOB' class='frebrkuptotl'>" + totalFare);
                            sbResponse.Append("</td>");
                        }
                        if (fareSeg.PaxType.ToString().Trim() == "CHD")
                        {
                            sbResponse.Append("<td width='50px' id='farebreakuptaxOB' class='frebrkuptsx'>" + totalTax);
                            sbResponse.Append("</td>");
                            sbResponse.Append("<td width='70px' id='farebreakuptotfareOB' class='frebrkuptotl'>" + totalFare);
                            sbResponse.Append("</td>");
                        }
                        if (fareSeg.PaxType.ToString().Trim() == "INF")
                        {
                            sbResponse.Append("<td width='50px'>" + totalTax);
                            sbResponse.Append("</td>");
                            sbResponse.Append("<td width='70px'>" + totalFare);
                            sbResponse.Append("</td>");
                        }
                        sbResponse.Append("</tr>");
                        sbResponse.Append("</table >");
                        sbResponse.Append("</td>");

                        if (!IsSingleFare)
                        {
                            var fareSegList = fareSegs.ToList();

                            var filteredFareSeg = fareSegList.Where(f => f.Conn == "I").ToList();
                            if (filteredFareSeg.Any())
                            {
                                var fareSegItem = filteredFareSeg.First();
                                PaxType = fareSegItem.PaxType.ToString().Trim();
                                yq = Convert.ToInt32(Convert.ToDouble(fareSegItem.Yq.ToString().Trim()));
                                Psf = Convert.ToInt32(Convert.ToDouble(fareSegItem.Psf.ToString().Trim()));
                                Udf = Convert.ToInt32(Convert.ToDouble(fareSegItem.Udf.ToString().Trim()));
                                AUDF = Convert.ToInt32(Convert.ToDouble(fareSegItem.Audf.ToString().Trim()));
                                Cute = Convert.ToInt32(Convert.ToDouble(fareSegItem.Cute.ToString().Trim()));
                                Gst = Convert.ToInt32(Convert.ToDouble(fareSegItem.Gst.ToString().Trim()));
                                TF = Convert.ToInt32(Convert.ToDouble(fareSegItem.Tf.ToString().Trim()));
                                Cess = Convert.ToInt32(Convert.ToDouble(fareSegItem.Cess.ToString().Trim()));
                                Ex = Convert.ToInt32(Convert.ToDouble(fareSegItem.Ex.ToString().Trim()));
                                Basic = Convert.ToInt32(Convert.ToDouble(fareSegItem.Basic.ToString().Trim()));
                                ServiceTax = Convert.ToInt32(Convert.ToDouble(fareSegItem.ServiceTax.ToString().Trim()));
                                Markup = Convert.ToInt32(Convert.ToDouble(fareSegItem.Markup.ToString().Trim()));
                                Service_Fee = Convert.ToInt32(Convert.ToDouble(fareSegItem.Service_Fee.ToString().Trim()));
                                totalTax = 0;
                                totalFare = "0";
                                if (PaxType == "INF")
                                {
                                    totalTax = Psf + Udf + AUDF + Cute + Gst + TF + Cess + Ex;
                                }
                                else
                                {
                                    totalTax = Psf + Udf + AUDF + Cute + Gst + TF + Cess + Ex + Markup;
                                }

                                totalFare = (Basic + yq + totalTax).ToString();
                                if ((totalTax + Basic) != 0)
                                {
                                    _totalfare = Convert.ToInt32(totalFare) + _totalfare;
                                    if (PaxType == "ADT")
                                    {
                                        PaxType = PaxType + " X " + Adt;
                                        totalFare = totalFare + " X " + Adt;
                                    }
                                    if (PaxType == "CHD")
                                    {
                                        PaxType = PaxType + " X " + Chd;
                                        totalFare = totalFare + " X " + Chd;
                                    }
                                    if (PaxType == "INF")
                                    {
                                        PaxType = PaxType + " X " + Inf;
                                        totalFare = totalFare + " X " + Inf;
                                    }
                                }
                                sbResponse.Append("<td width='50%'>");
                                sbResponse.Append("<table >");
                                sbResponse.Append("<tr>");
                                sbResponse.Append("<td width='120px'>" + PaxType);
                                sbResponse.Append("</td>");
                                sbResponse.Append("<td width='70px'>" + Basic);
                                sbResponse.Append("</td>");
                                sbResponse.Append("<td width='60px'>" + yq);
                                sbResponse.Append("</td>");
                                if (fareSegItem.PaxType.ToString().Trim() == "ADT")
                                {
                                    sbResponse.Append("<td width='50px' id='farebreakuptaxIB' class='frebrkuptsx'>" + totalTax);
                                    sbResponse.Append("</td>");
                                    sbResponse.Append("<td width='70px' id='farebreakuptotfareIB' class='frebrkuptotl'>" + totalFare);
                                    sbResponse.Append("</td>");
                                }
                                if (fareSegItem.PaxType.ToString().Trim() == "CHD")
                                {
                                    sbResponse.Append("<td width='50px' id='farebreakuptaxIB' class='frebrkuptsx'>" + totalTax);
                                    sbResponse.Append("</td>");
                                    sbResponse.Append("<td width='70px' id='farebreakuptotfareIB' class='frebrkuptotl'>" + totalFare);
                                    sbResponse.Append("</td>");
                                }
                                if (fareSegItem.PaxType.ToString().Trim() == "INF")
                                {
                                    sbResponse.Append("<td width='50px'>" + totalTax);
                                    sbResponse.Append("</td>");
                                    sbResponse.Append("<td width='70px'>" + totalFare);
                                    sbResponse.Append("</td>");
                                }
                                sbResponse.Append("</tr>");
                                sbResponse.Append("</table >");
                                sbResponse.Append("</td>");
                            }
                        }

                        sbResponse.Append("</tr>");
                    }
                }

                sbResponse.Append("</table>");

                sbResponse.Append("</td>");
                sbResponse.Append("</tr>");

                #endregion
                //============Fare Breakup End==============//

                sbResponse.Append("</table>");

                sbResponse.Append("</td>");
                sbResponse.Append("</tr>");

                //======Fare Detail partr start=============//
                #region
                sbResponse.Append("<tr id='divfaredetail' style='width: 100%; font-size:15px;'>");
                sbResponse.Append("<td>");

                sbResponse.Append("<table width='100%' style='' cellpadding='10' >");
                sbResponse.Append("<tr style='font-size:15px;'>");

                sbResponse.Append("<td width='73%' style='border: 1px solid #D6D8E7;'> <b> Service Information </b>");
                sbResponse.Append("</td>");

                sbResponse.Append("<td style='border: 1px solid #D6D8E7;'> <b> Fare Information  </b>");
                sbResponse.Append("</td>");

                sbResponse.Append("</tr>");
                sbResponse.Append("</table>");

                sbResponse.Append("<table>");
                sbResponse.Append("<tr>");
                sbResponse.Append("<td width='45%' valign='top' align='left' style='font-size:14px;'>");
                sbResponse.Append("<table width='100%' >");
                sbResponse.Append("<tbody>");


                if (paxSegmentDetails != null && paxSegmentDetails.Any())
                {
                    foreach (var paxSegment in paxSegmentDetails)
                    {
                        if (paxSegment.Conn != null && paxSegment.Conn.Equals("O"))
                        {
                            string maealbagagge = paxSegment.ChargeType.Equals("M") ? "Meal" : "Baggage";
                            sbResponse.Append("<tr>");
                            sbResponse.Append("<td><b>* </b>" + Originn + "-" + Destinationn + " added " + maealbagagge + "<span>&nbsp;" + paxSegment.ChargeDescription + "</span>  <span>" + paxSegment.Charge_Amount + "Rs.</span></td>");
                            sbResponse.Append("</tr>");
                        }
                        else
                        {
                            string maealbagagge = paxSegment.ChargeType.Equals("M") ? "Meal" : "Baggage";
                            sbResponse.Append("<tr>");
                            sbResponse.Append("<td><b>* </b>" + Destinationn + "-" + Originn + " added " + maealbagagge + "<span>&nbsp;" + paxSegment.ChargeDescription + "</span>  <span>" + paxSegment.Charge_Amount + "Rs.</span></td>");
                            sbResponse.Append("</tr>");
                        }
                    }
                }




                if (paxSegmentRuleDetails != null && paxSegmentRuleDetails.Any())
                {
                    foreach (var paxSegment in paxSegmentRuleDetails)
                    {
                        if (paxSegment.Conn != null && paxSegment.Conn == "O")
                        {
                            sbResponse.Append("<tr>");
                            if (paxSegment.BaggageDetail != null && paxSegment.BaggageDetail.IndexOf("*") != -1)
                            {
                                string bagg = paxSegment.BaggageDetail;
                                sbResponse.Append("<td><b>* </b>" + Originn + "-" + Destinationn + " available baggage on per pax  <span>" + bagg.Split('*')[0].Trim() + " Check IN </span>  </td>");
                            }
                            else
                            {
                                sbResponse.Append("<td><b>* </b>" + Originn + "-" + Destinationn + " available baggage on per pax  <span>Check IN: APAP </span>  </td>");
                            }
                            sbResponse.Append("</tr>");
                        }
                        else
                        {
                            sbResponse.Append("<tr>");
                            if (paxSegment.BaggageDetail != null && paxSegment.BaggageDetail.IndexOf("*") != -1)
                            {
                                string bagg = paxSegment.BaggageDetail;
                                sbResponse.Append("<td><b>* </b>" + Destinationn + "-" + Originn + " available baggage on per pax  <span>" + bagg.Split('*')[0].Trim() + " Check IN </span>  </td>");
                            }
                            else
                            {
                                sbResponse.Append("<td><b>* </b>" + Destinationn + "-" + Originn + " available baggage on per pax  <span>Check IN: APAP </span>  </td>");
                            }
                            sbResponse.Append("</tr>");
                        }
                    }
                }

                sbResponse.Append("<tr>");
                sbResponse.Append("<td>This is an Electronic ticket. Please carry a positive identification for Check in.</td>");
                sbResponse.Append("</tr>");

                sbResponse.Append("</tbody>");
                sbResponse.Append("</table>");
                sbResponse.Append("</td>");

                sbResponse.Append("<td width='28%' valign='top' align='right'>");



                string adt = flightDetails.FirstOrDefault().Adt.ToString().Trim();
                string chd = flightDetails.FirstOrDefault().Chd.ToString().Trim();
                string inf = flightDetails.FirstOrDefault().Inf.ToString().Trim();
                int TotalServiceFee = NumberHelper.ConvertToInt(flightDetails.FirstOrDefault().TotalServiceFeeDeal);
                int TotalServiceTax = NumberHelper.ConvertToInt(flightDetails.FirstOrDefault().TotalServiceTax);
                int TotalMealcharges = NumberHelper.ConvertToInt(flightDetails.FirstOrDefault().TotalMeal);
                int TotalBaggCharges = NumberHelper.ConvertToInt(flightDetails.FirstOrDefault().TotalBaggage);
                int TotalFare = NumberHelper.ConvertToInt(flightDetails.FirstOrDefault().TotalFare);
                int TotalTDS = NumberHelper.ConvertToInt(flightDetails.FirstOrDefault().TotalTds);
                int TotalMarkup = NumberHelper.ConvertToInt(flightDetails.FirstOrDefault().TotalMarkup);
                int TotalBasic = NumberHelper.ConvertToInt(flightDetails.FirstOrDefault().TotalBasic);
                int TotalTax = NumberHelper.ConvertToInt(flightDetails.FirstOrDefault().TotalTax) + TotalMarkup;
                int TotalCommission = NumberHelper.ConvertToInt(flightDetails.FirstOrDefault().TotalCommission);
                TotalTax += TotalMarkup;

                TotalFare = (TotalFare + TotalMealcharges + TotalBaggCharges + SurchargeAmount) - (TotalTDS);
                sbResponse.Append("<table width='100%'>");
                sbResponse.Append("<tbody>");

                sbResponse.Append("<tr>");
                sbResponse.Append("<td align='right'>BFare :</td>");
                sbResponse.Append("<td align='right'>" + TotalBasic + "" + Currency + "</td>");
                sbResponse.Append("</tr>");
                sbResponse.Append("<tr>");
                sbResponse.Append("<td align='right'>  Taxes :  </td>");
                sbResponse.Append("<td align='right'> <span id='oldtotaltax'>" + TotalTax + "" + Currency + "</span> </td>");
                sbResponse.Append("</tr>");
                sbResponse.Append("<tr>");
                sbResponse.Append("<td align='right'> Fee & Surcharge :  </td>");
                sbResponse.Append("<td align='right'>" + (TotalServiceFee + TotalServiceTax).ToString() + "" + Currency + "</td>");
                sbResponse.Append("</tr>");
                sbResponse.Append("<tr id='divEditTxnFeeDiv' style='display:none;'>");
                sbResponse.Append("<td align='right'>Txn Fee:</td>");
                sbResponse.Append("<td align='right'>Rs. <input type='text' style='width: 80px;' id='CurrentTxnFee' name='CurrentTxnFee' value='0' /></td>");
                sbResponse.Append("</tr>");
                sbResponse.Append("<tr id='divApplyDiscount' style='display:none;'>");
                sbResponse.Append("<td align='right'> Discount(-) :</td>");
                sbResponse.Append("<td align='right'><input type='text' style='width: 80px;' id='CurrentDiscount' name='CurrentDiscount' value='0' /></td>");
                sbResponse.Append("</tr>");
                sbResponse.Append("<tr id='Discountdiv' style='display:none;' align='right' >");
                sbResponse.Append("<td align='right'> Discount(-) :</td>");
                sbResponse.Append("<td align='right'>Rs <span id='spndiscount'>0</span></td>");
                sbResponse.Append("</tr>");
                sbResponse.Append("<tr>");
                sbResponse.Append("<td align='right'> Services :  </td>");
                sbResponse.Append("<td align='right'>" + (TotalMealcharges + TotalBaggCharges).ToString() + "" + Currency + "</td>");
                sbResponse.Append("</tr>");

                sbResponse.Append("<tr>");
                sbResponse.Append("<td align='right'> Conv.fee :  </td>");
                sbResponse.Append("<td align='right'>" + (SurchargeAmount).ToString() + "" + Currency + "</td>");
                sbResponse.Append("</tr>");

                sbResponse.Append("<tr style='font-weight:600;'>");
                sbResponse.Append("<td align='right'> Total Amount :  </td>");
                sbResponse.Append("<td align='right'><span id='oldtotalamt'>" + TotalFare + "" + Currency + "</span></td>");
                sbResponse.Append("</tr>");
                sbResponse.Append("</tbody>");
                sbResponse.Append("</table>");
                //}

                sbResponse.Append("</td>");

                sbResponse.Append("</tr>");


                sbResponse.Append("</table>");
                sbResponse.Append("</td>");

                sbResponse.Append("</tr>");
                #endregion
                //======Fare Detail partr End=============//




                //======Fare Detail partr new start=============//
                #region
                sbResponse.Append("<tr id='divfaredetailnew' style='display:none;'>");
                sbResponse.Append("<td>");

                sbResponse.Append("<table width='100%' style='' cellpadding='10' >");
                sbResponse.Append("<tr style='font-size:15px;'>");

                sbResponse.Append("<td  style='border: 1px solid #D6D8E7;' colspan='2'> <b> Service Information </b>");
                sbResponse.Append("</td>");



                sbResponse.Append("</tr>");
                sbResponse.Append("</table>");

                sbResponse.Append("<table>");
                sbResponse.Append("<tr>");
                sbResponse.Append("<td width='45%' valign='top' align='left' style='font-size:14px;'>");
                sbResponse.Append("<table width='100%' >");
                sbResponse.Append("<tbody>");

                if (paxSegmentDetails.Any())
                {
                    foreach (var paxSegment in paxSegmentDetails)
                    {
                        if (paxSegment.Conn != null && paxSegment.Conn.Equals("O"))
                        {
                            string maealbagagge = "";
                            if (paxSegment.ChargeType.Equals("M"))
                            {
                                maealbagagge = "Meal";
                            }
                            else
                            {
                                maealbagagge = "Baggage";
                            }

                            sbResponse.Append("<tr>");
                            sbResponse.Append("<td><b>* </b>" + Originn + "-" + Destinationn + " added " + maealbagagge + "<span>&nbsp;" + paxSegment.ChargeDescription + "</span>  <span>" + paxSegment.Charge_Amount + "Rs.</span></td>");
                            sbResponse.Append("</tr>");
                        }
                        else
                        {
                            string maealbagagge = "";
                            if (paxSegment.ChargeType.Equals("M"))
                            {
                                maealbagagge = "Meal";
                            }
                            else
                            {
                                maealbagagge = "Baggage";
                            }

                            sbResponse.Append("<tr>");
                            sbResponse.Append("<td><b>* </b>" + Destinationn + "-" + Originn + " added " + maealbagagge + "<span>&nbsp;" + paxSegment.ChargeDescription + "</span>  <span>" + paxSegment.Charge_Amount + "Rs.</span></td>");
                            sbResponse.Append("</tr>");
                        }
                    }
                }

                if (paxSegmentRuleDetails != null && paxSegmentRuleDetails.Any())
                {
                    foreach (var paxSegment in paxSegmentRuleDetails)
                    {
                        if (paxSegment.Conn != null && paxSegment.Conn.Equals("O"))
                        {
                            sbResponse.Append("<tr>");
                            if (paxSegment.BaggageDetail != null && paxSegment.BaggageDetail.IndexOf("*") != -1)
                            {
                                string bagg = paxSegment.BaggageDetail;
                                sbResponse.Append("<td><b>* </b>" + Originn + "-" + Destinationn + " available baggage on per pax  <span>" + bagg.Split('*')[0].Trim() + " Check IN </span>  </td>");
                            }
                            else
                            {
                                sbResponse.Append("<td><b>* </b>" + Originn + "-" + Destinationn + " available baggage on per pax  <span>Check IN: APAP </span>  </td>");
                            }
                            sbResponse.Append("</tr>");
                        }
                        else
                        {
                            sbResponse.Append("<tr>");
                            if (paxSegment.BaggageDetail != null && paxSegment.BaggageDetail.IndexOf("*") != -1)
                            {
                                string bagg = paxSegment.BaggageDetail;
                                sbResponse.Append("<td><b>* </b>" + Destinationn + "-" + Originn + " available baggage on per pax  <span>" + bagg.Split('*')[0].Trim() + " Check IN </span>  </td>");
                            }
                            else
                            {
                                sbResponse.Append("<td><b>* </b>" + Destinationn + "-" + Originn + " available baggage on per pax  <span>Check IN: APAP </span>  </td>");
                            }
                            sbResponse.Append("</tr>");
                        }
                    }
                }

                sbResponse.Append("<tr>");
                sbResponse.Append("<td>This is an Electronic ticket. Please carry a positive identification for Check in.</td>");
                sbResponse.Append("</tr>");
                sbResponse.Append("</tbody>");
                sbResponse.Append("</table>");
                sbResponse.Append("</td>");

                sbResponse.Append("<td width='28%' valign='top' align='right'>");
                sbResponse.Append("</td>");

                sbResponse.Append("</tr>");


                sbResponse.Append("</table>");
                sbResponse.Append("</td>");

                sbResponse.Append("</tr>");
                #endregion
                //======Fare Detail partr new End=============//

                //======GST Detail start=============//
                var gstDetail = bookingDetail.CompanyFlightGSTDetails.FirstOrDefault();
                if (gstDetail != null)
                {

                    sbResponse.Append("<tr>");
                    sbResponse.Append("<td colspan='2'>");
                    sbResponse.Append("<table width='100%' style='border: 1px solid #D6D8E7;' cellspacing='0' cellpadding='10' >");
                    sbResponse.Append("<tr style='font-size:15px;'>");
                    sbResponse.Append("<td width='73%' style=''>GST Detail");
                    sbResponse.Append("</td>");
                    sbResponse.Append("</tr>");
                    sbResponse.Append("</table>");
                    sbResponse.Append("</td>");
                    sbResponse.Append("</tr>");

                    sbResponse.Append("<tr>");
                    sbResponse.Append("<td colspan='2'>");
                    sbResponse.Append("<table width='100%' style='font-size:11px;' cellspacing='0' cellpadding='3' >");
                    sbResponse.Append("<tr>");
                    sbResponse.Append("<td>Gst CompanyName:</td>");
                    sbResponse.Append("<td>" + StringHelper.GetUpper(gstDetail.GstcompanyName.ToString().Trim()) + "</td>");
                    sbResponse.Append("</tr>");
                    sbResponse.Append("<tr>");
                    sbResponse.Append("<td>Gst Number:</td>");
                    sbResponse.Append("<td>" + StringHelper.GetUpper(gstDetail.Gstnumber.ToString().Trim()) + "</td>");
                    sbResponse.Append("</tr>");
                    sbResponse.Append("</table>");
                    sbResponse.Append("</td>");
                    sbResponse.Append("</tr>");
                }
                //======GST Detail End =============//

                sbResponse.Append("<tr>");
                sbResponse.Append("<td colspan='2'>");
                sbResponse.Append("<table width='100%' style='border: 1px solid #D6D8E7;' cellspacing='0' cellpadding='10' >");
                sbResponse.Append("<tr style='font-size:15px;'>");
                sbResponse.Append("<td width='73%' style=''><b> Terms & Conditions </b>");
                sbResponse.Append("</td>");
                sbResponse.Append("</tr>");
                sbResponse.Append("</table>");
                sbResponse.Append("</td>");
                sbResponse.Append("</tr>");

                sbResponse.Append("<tr>");
                sbResponse.Append("<td colspan='2'>");
                sbResponse.Append("<table width='100%' style='font-size:12px;' cellspacing='0' cellpadding='3' >");
                sbResponse.Append("<tr>");
                sbResponse.Append("<td><b>*</b> All Guests, including children and infants, must present valid identification at check-in.");
                sbResponse.Append("</td>");
                sbResponse.Append("</tr>");
                sbResponse.Append("<tr>");
                sbResponse.Append("<td><b>*</b> Carriage and other services provided by the carrier are subject to conditions of carriage, which are hereby incorporated by reference. These conditions may be obtained from the issuing carrier.");
                sbResponse.Append("</td>");
                sbResponse.Append("</tr>");
                sbResponse.Append("<tr>");
                sbResponse.Append("<td><b>*</b> Changes to your reservation will result in a fee of Airline terms & condition,change plus any difference in the fare between the original fare paid and the fare for the revised booking.");
                sbResponse.Append("</td>");
                sbResponse.Append("</tr>");

                sbResponse.Append("<tr>");
                sbResponse.Append("<td><b>*</b> Cancellation charges as per Airlines terms & condition.");
                sbResponse.Append("</td>");
                sbResponse.Append("</tr>");

                sbResponse.Append("<tr>");
                sbResponse.Append("<td><b>*</b> Airline Free Baggages are per seat only.");
                sbResponse.Append("</td>");
                sbResponse.Append("</tr>");


                sbResponse.Append("<tr>");
                sbResponse.Append("<td><b>*</b> Check your itinerary with airline also,revert with in 2 hour. we are here to help you.");
                sbResponse.Append("</td>");
                sbResponse.Append("</tr>");

                sbResponse.Append("<tr>");
                sbResponse.Append("<td><b>*</b> As per Government guidelines, check-in counters at all airports will now close 45 minutes before departure with immediate effect. Please plan your Airport arrival accordingly.");
                sbResponse.Append("</td>");
                sbResponse.Append("</tr>");
                sbResponse.Append("<tr>");
                sbResponse.Append("<td><b>*</b> Wish You a Happy Journey.");
                sbResponse.Append("</td>");
                sbResponse.Append("</tr>");

                sbResponse.Append("</table>");
                sbResponse.Append("</td>");
                sbResponse.Append("</tr>");

                sbResponse.Append("</table>");
                //}
            }
            catch (Exception ex)
            {
                DBLoggerHelper.DBLogAsync("", iBookingRef, "Print_Popup_Tkt", "PrintPopupController", "", "", ex.Message, _addDbLogCommandHandler);
            }
            return sbResponse;
        }

        public string PrintTicketPdf(BookingDetailData bookingDetail, int iBookingRef, string Tax, string Logo, string SessionCompany, string HideFare, string Disc)
        {

            StringBuilder sbResponse = new StringBuilder();
            try
            {
                var flightDetails = bookingDetail.CompanyFlightDetailAirlines;
                var flightSegDetails = bookingDetail.CompanyFlightSegmentDetailAirlines;
                var fareDetails = bookingDetail.CompanyFareDetailAirlines?.FirstOrDefault();
                var fareDetailAirline = bookingDetail.CompanyFareDetailAirlines;
                var fareDetailSeg = bookingDetail.CompanyFareDetailSegmentAirlines;
                var paxDetails = bookingDetail.CompanyPaxDetailAirlines;
                //var flightFareRule = bookingDetail.CompanyFlightDetailAirlines;
                var compDetails = bookingDetail.CompanyFlightDetails?.FirstOrDefault();
                var paxSegmentDetails = bookingDetail.CompanyPaxSegmentDetailAirlines;
                var paxSegmentRuleDetails = bookingDetail.CompanyFlightSegmentRuleDetailAirlines;
                var companyTransactionDetail = bookingDetail.CompanyTransactionDetails;
                var bookingAirlineLogForPGs = bookingDetail.BookingAirlineLogForPGs;
                string IssueON = string.Empty;
                string Originn = string.Empty;
                string Destinationn = string.Empty;
                string Adt = string.Empty;
                string Chd = string.Empty;
                string Inf = string.Empty;
                string Trip = string.Empty;
                int SurchargeAmount = 0;


                Double dCurrencyValue = 1;
                string Currency = "INR";

                if (companyTransactionDetail != null && companyTransactionDetail.Any() && bookingAirlineLogForPGs != null && bookingAirlineLogForPGs.Any())
                {
                    dCurrencyValue = Convert.ToDouble(bookingAirlineLogForPGs.FirstOrDefault().CurrencyValue.ToString());
                    Currency = bookingAirlineLogForPGs.FirstOrDefault().Currency.ToString().Trim();
                }

                if (companyTransactionDetail?.FirstOrDefault()?.SurchargeAmount != null &&
        decimal.TryParse(companyTransactionDetail.FirstOrDefault().SurchargeAmount.ToString(), out var surchargeAmount))
                {
                    SurchargeAmount = Decimal.ToInt32(surchargeAmount);
                }
                else
                {
                    SurchargeAmount = 0;
                }


                Trip = flightDetails.FirstOrDefault().Trip.ToString().Trim();
                IssueON = StringHelper.GetConvertedDateTime(flightDetails.FirstOrDefault().EventTime.ToString());
                Originn = flightDetails.FirstOrDefault().Origin.ToString().Trim();
                Destinationn = flightDetails.FirstOrDefault().Destination.ToString().Trim();
                Adt = flightDetails.FirstOrDefault().Adt.ToString().Trim();
                Chd = flightDetails.FirstOrDefault().Chd.ToString().Trim();
                Inf = flightDetails.FirstOrDefault().Inf.ToString().Trim();

                string CompanyLogo = string.Empty;
                string CompanyName = string.Empty;
                string Address = string.Empty;
                string Mobile = string.Empty;
                string Email = string.Empty;
                string CompanyID = flightDetails.FirstOrDefault().CompanyId.ToString();


                if (CompanyID.IndexOf("-SA-") != -1 || CompanyID.IndexOf("C-") != -1)
                {
                    if (CompanyID.IndexOf("C-") != -1)
                    {
                        Mobile = compDetails.A_Mobile.ToString();
                        Email = StringHelper.GetLower(compDetails.A_Email.ToString());
                        bool compactiv = Convert.ToBoolean(compDetails.AgencyPrintB2c.ToString());
                        if (compactiv)
                        {
                            CompanyName = StringHelper.GetUpper(compDetails.A_CompanyName.ToString());
                            CompanyLogo = compDetails.A_CompanyLogo.ToString();
                        }
                    }
                    else
                    {
                        CompanyLogo = compDetails.A_CompanyLogo.ToString();
                        CompanyName = StringHelper.GetUpper(compDetails.A_CompanyName.ToString());
                        Address = StringHelper.GetALLFirstCharacterCapital(compDetails.A_Address.ToString());
                        Mobile = compDetails.A_Mobile.ToString();
                        Email = StringHelper.GetLower(compDetails.A_Email.ToString());
                    }
                }
                else
                {
                    CompanyLogo = compDetails.CompanyLogo.ToString();
                    CompanyName = StringHelper.GetUpper(compDetails.CompanyName.ToString());
                    Address = StringHelper.GetALLFirstCharacterCapital(compDetails.Address.ToString());
                    Mobile = compDetails.Mobile.ToString();
                    Email = StringHelper.GetLower(compDetails.Email.ToString());
                }

                sbResponse.Append("<div style='margin-top:-40px;margin-left:-45px;'>");
                sbResponse.Append("<table style='border: 1px solid #D6D8E7; width: 785px;  font-size: 11px; font-family: Arial,Sans-Serif;' cellspacing='3' id='showmndetail' runat='server'>");
                if (Logo != null && Logo != "")
                {
                    if (Logo != "False")
                    {
                        sbResponse.Append("<tr id='logodiv'>");
                        sbResponse.Append("<td style='border-bottom: 1px solid #d6d8e7;'>");
                        sbResponse.Append("<img id='imgeLogo' src='" + CompanyLogo + "' style='  height: 63px;  width: 218px; '/>");
                        sbResponse.Append("</td>");
                        sbResponse.Append("</tr>");
                    }
                }


                sbResponse.Append("<tr>");
                sbResponse.Append("<td>");
                sbResponse.Append("<table width='100%'  style='border: 1px solid #D6D8E7;background: #fff;' cellpadding='10' >");
                sbResponse.Append("<tr>");

                sbResponse.Append("<td width='40%'  >");
                sbResponse.Append("<span style='font-size: 15px;   color: #929292;'> <img src='wwwroot\\assets\\img\\airlogo_square\\onwardflight.png' width='20px' /> Onward Flight </span>");
                sbResponse.Append("<span style='font-size: 15px;'> <b> " + flightDetails.FirstOrDefault().OriginCity.ToString().Trim() + " to </b> </span>");
                sbResponse.Append("<span style='font-size: 15px;'> <b> " + flightDetails.FirstOrDefault().DestinationCity.ToString().Trim() + " </b> </span>");

                sbResponse.Append("</td>");

                sbResponse.Append("<td width='20%'>");
                sbResponse.Append("</td>");

                sbResponse.Append("<td width='40%'>");

                sbResponse.Append("<span>Airline PNR:</span><span id='spnpnr' style='font-size: 22px;' > <b>" + flightDetails.FirstOrDefault().Airline_Pnr_D.ToString() + "</b> </span><br />");

                sbResponse.Append("</td>");

                sbResponse.Append("</tr>");
                sbResponse.Append("</table>");
                sbResponse.Append("</td>");
                sbResponse.Append("</tr>");

                if (flightDetails.FirstOrDefault().Trip.ToString().Equals("R"))
                {

                    sbResponse.Append("<tr>");
                    sbResponse.Append("<td>");
                    sbResponse.Append("<table width='100%'  style='border: 1px solid #D6D8E7;background: #fff;' cellpadding='10' >");
                    sbResponse.Append("<tr>");

                    sbResponse.Append("<td width='40%'  >");
                    sbResponse.Append("<span style='font-size: 15px;   color: #929292;'> <img src='wwwroot\\assets\\img\\airlogo_square\\Inwardflight.png' width='20px' /> Return Flight </span>");
                    sbResponse.Append("<span style='font-size: 15px;'> " + flightDetails.FirstOrDefault().DestinationCity.ToString().Trim() + " to </span>");
                    sbResponse.Append("<span style='font-size: 15px;'> " + flightDetails.FirstOrDefault().OriginCity.ToString().Trim() + "</span>");

                    sbResponse.Append("</td>");

                    sbResponse.Append("<td width='20%'>");
                    sbResponse.Append("</td>");

                    sbResponse.Append("<td width='40%' >");

                    sbResponse.Append("<span>Airline PNR:</span><span id='spnpnr' style='font-size: 22px;' ><b> " + flightDetails.FirstOrDefault().Airline_Pnr_A.ToString() + " </b> </span><br />");

                    sbResponse.Append("</td>");

                    sbResponse.Append("</tr>");
                    sbResponse.Append("</table>");
                    sbResponse.Append("</td>");
                    sbResponse.Append("</tr>");
                }



                sbResponse.Append("<tr>");
                sbResponse.Append("<td>");
                sbResponse.Append("<table width='100%' style='border: 1px solid #D6D8E7;' cellpadding='10' >");
                sbResponse.Append("<tr >");
                sbResponse.Append("<td width='33%' style='font-size:15px;' ><span> Booking Ref. </span> <span id='bookinref'> " + iBookingRef.ToString() + "  </span> ");
                sbResponse.Append("</td>");
                sbResponse.Append("<td width='33%' align='center' style='font-size: 55px;' > ");
                sbResponse.Append("</td>");
                sbResponse.Append("<td width='33%' align='right' style='font-size:15px;' ><span>Issued Date:</span><span id='pnrheadingIssueOn'> " + IssueON + "</span>  ");
                sbResponse.Append("</td>");
                sbResponse.Append("</tr>");
                sbResponse.Append("</table>");
                sbResponse.Append("</td>");
                sbResponse.Append("</tr>");

                sbResponse.Append("<tr>");
                sbResponse.Append("<td>");
                sbResponse.Append("<table width='100%' cellpadding='10'>");
                sbResponse.Append("<tr>");
                sbResponse.Append("<td width='33%' valign='top'>");
                sbResponse.Append("<span style='font-size: 16px;'><span id='txtCompany_Name'><b>" + CompanyName + "</b></span></span><br />");
                sbResponse.Append("<span style='font-size: 15px; color: #929292;' id='txtAddressCompany'>" + Address + "");
                sbResponse.Append("</span><br />");
                sbResponse.Append("<span style='font-size: 15px; color: #929292;' >Phone: <span id='CMobile'>" + Mobile + "</span><br />");
                sbResponse.Append("<span style='font-size: 15px; color: #929292;'>Email: </span><span id='CEmail'>" + Email + "</span>");
                sbResponse.Append("</span>");
                sbResponse.Append("</td>");
                sbResponse.Append("<td width='33%' align='center' >");
                sbResponse.Append("<b style=' font-size:27px;'>E - Ticket </b>");
                sbResponse.Append("</td>");
                sbResponse.Append("<td width='33%' valign='top' align='left' style='font-size: 15px;' >");
                sbResponse.Append("");

                //if (flightDetails.FirstOrDefault().Trip.ToString().Equals("O"))
                //{
                //    sbResponse.Append("<span>PNR:</span><span id='spnpnr'> " + flightDetails.FirstOrDefault().Airline_Pnr_D.ToString() + "</span><br />");
                //}
                //else
                //{
                //    sbResponse.Append("<span >PNR:</span><span id='spnpnr'> " + flightDetails.FirstOrDefault().Airline_Pnr_D.ToString() + "/" + flightDetails.FirstOrDefault().Airline_Pnr_A.ToString() + "</span><br />");
                //}

                string strStatus = Getbookingstatus(flightDetails, paxDetails, fareDetails);

                if (strStatus.Equals("Confirm"))
                {
                    sbResponse.Append("<span style='font-size:18px;'> <img src='wwwroot\\assets\\img\\airlogo_square\\confirmcheck.png' width='22px' /> </span><span id='spnstatus' style='Color:#00e300; font-size: 25px;'> Confirm</span><br />");
                }
                else if (strStatus.Equals("Unconfirm"))
                {
                    sbResponse.Append("<span style='font-size:18px;'> <img src='wwwroot\\assets\\img\\airlogo_square\\unconfirmcross.png' width='22px' /> </span><span id='spnstatus' style='Color:Red; font-size: 25px;'> Unconfirm</span><br />");
                }
                else if (strStatus.Equals("On Hold"))
                {
                    sbResponse.Append("<span style='font-size:18px;'> <img src='wwwroot\\assets\\img\\airlogo_square\\unconfirmcross.png' width='22px' /> </span><span id='spnstatus' style='Color:Red; font-size: 25px;'> On Hold</span><br />");
                }
                else
                {
                    sbResponse.Append("<span style='font-size:18px;'> <img src='\\assets\\img\\airlogo_square\\unconfirmcross.png' width='22px' /> </span><span id='spnstatus' style='Color:Red; font-size: 25px;'> " + strStatus + "</span><br />");
                }
                if (paxDetails != null && paxDetails.Any())
                {
                    sbResponse.Append("<span style='color: #929292;' >MobileNo:</span><span id='spnpaxmobile' style='color: #929292;' > " + paxDetails.FirstOrDefault().MobileNo.ToString().Trim() + "</span><br />");
                    sbResponse.Append("<span style='color: #929292;' >Email:</span><span id='spanpaxEmail' style='color: #929292;' > " + StringHelper.GetLower(paxDetails.FirstOrDefault().Email.ToString().Trim().ToLower()) + "</span>");
                }

                sbResponse.Append("</td>");
                sbResponse.Append("</tr>");
                sbResponse.Append("</table>");
                sbResponse.Append("</td>");
                sbResponse.Append("</tr>");


                sbResponse.Append("<tr>");
                sbResponse.Append("<td>");
                sbResponse.Append("<table width='100%' style='border: 1px solid #D6D8E7;' cellspacing='0' cellpadding='10'>");
                sbResponse.Append("<tr style=' font-size:15px;'>");
                sbResponse.Append("<td width='34%' style='border-bottom: 1px solid #D6D8E7; '> <b> Passengers  </b> </td>");
                sbResponse.Append("<td width='27%' style='border-bottom: 1px solid #D6D8E7;' > <b> Ticket Number  </b> </td>");
                sbResponse.Append("<td width='20%' style='border-bottom: 1px solid #D6D8E7;' > <b> Frequent flyer no. </b> </td>");
                sbResponse.Append("<td width='16%' style='border-bottom: 1px solid #D6D8E7;' align='center' > <b> Services </b> </td>");
                sbResponse.Append("</tr>");
                foreach (var paxItem in paxDetails)
                {
                    string dob = "--";
                    string fnn = "--";
                    string Meal = string.Empty;
                    string Bagg = string.Empty;
                    string fullname = StringHelper.GetUpper(paxItem.Title.ToString().Trim() + " " + paxItem.First_Name.ToString().Trim() + " " + paxItem.Middle_Name.ToString().Trim() + " " + paxItem.Last_Name.ToString().Trim());
                    string tktno = "--";
                    if (paxItem.TicketNo.ToString().Trim() != "")
                    {
                        tktno = paxItem.TicketNo.ToString().Trim();
                    }
                    if (paxItem.Ffn.ToString().Trim() != "")
                    {
                        fnn = paxItem.Ffn.ToString().Trim();
                    }
                    if (paxItem.Pax_SegmentId.ToString().Trim() != "")
                    {
                        string paxsegid = paxItem.Pax_SegmentId.ToString().Trim();
                        Meal = GetMealDetails(paxsegid, paxSegmentDetails);
                        Bagg = GetBaggDetails(paxsegid, paxSegmentDetails);
                    }
                    string PaxType = "";
                    if (paxItem.PaxType.ToString().Trim() != "")
                    {
                        PaxType = paxItem.PaxType.ToString().Trim();
                    }
                    if (paxItem.Dob.ToString().Trim() != "")
                    {
                        dob = paxItem.Dob.ToString().Trim();
                    }
                    sbResponse.Append("<tr>");
                    sbResponse.Append("<td style='font-size:15px;'>" + fullname + "</td>");
                    sbResponse.Append("<td style='font-size:15px;'>" + tktno + "</td>");
                    sbResponse.Append("<td style='font-size:15px;'>" + fnn + "</td>");
                    sbResponse.Append("<td style='font-size:15px;' align='center'>");
                    if (Bagg != "")
                    {
                        sbResponse.Append("<span><img src='wwwroot\\assets\\img\\Commonimages\\baggage.png' title='Baggage' style='width: 16px;' /></span>");
                    }
                    else
                    {
                        sbResponse.Append("<span>--</span>");
                    }
                    if (Meal != "")
                    {
                        sbResponse.Append("<span>  <img src='wwwroot\\assets\\img\\Commonimages\\meal.png' style='width: 19px;' title='Meal' /></span>");
                    }
                    else
                    {
                        sbResponse.Append("<span>--</span>");
                    }
                    sbResponse.Append("</td>");
                    sbResponse.Append("</tr>");
                }
                sbResponse.Append("</table>");
                sbResponse.Append("</td>");
                sbResponse.Append("</tr>");

                sbResponse.Append("<tr>");
                sbResponse.Append("<td>" + GetSectorWiseDetailsOB(flightDetails, flightSegDetails,true));
                sbResponse.Append("</td>");
                sbResponse.Append("</tr>");

                if (Trip == "R")
                {
                    sbResponse.Append("<tr>");
                    sbResponse.Append("<td>" + GetSectorWiseDetailsIB(flightDetails, flightSegDetails,true));
                    sbResponse.Append("</td>");
                    sbResponse.Append("</tr>");
                }
                bool IsSingleFare = false;
                if (Trip == "R")
                {
                    var drFareType = fareDetailSeg.Where(f => f.Conn == "I" && f.PaxType == "ADT"); ;
                    if (drFareType.Any())
                    {
                        Decimal dCount = 0;
                        foreach (var dr in drFareType)
                        {
                            dCount = Convert.ToDecimal(dr.Yq.ToString().Trim());
                            dCount += Convert.ToDecimal(dr.Psf.ToString().Trim());
                            dCount += Convert.ToDecimal(dr.Udf.ToString().Trim());
                            dCount += Convert.ToDecimal(dr.Audf.ToString().Trim());
                            dCount += Convert.ToDecimal(dr.Cute.ToString().Trim());
                            dCount += Convert.ToDecimal(dr.Gst.ToString().Trim());
                            dCount += Convert.ToDecimal(dr.Tf.ToString().Trim());
                            dCount += Convert.ToDecimal(dr.Cess.ToString().Trim());
                            dCount += Convert.ToDecimal(dr.Ex.ToString().Trim());
                            dCount += Convert.ToDecimal(dr.Basic.ToString().Trim());

                            if (dCount <= 0)
                            {
                                IsSingleFare = true;

                                break;
                            }
                        }
                    }
                }

                if (HideFare != null && HideFare != "")
                {
                    if (HideFare == "False")
                    {
                        //======Fare Detail partr strat=============//
                        #region
                        sbResponse.Append("<tr id='divfaredetail'>");
                        sbResponse.Append("<td>");

                        sbResponse.Append("<table width='100%' style='font-size: 15px;' cellpadding='10' >");
                        sbResponse.Append("<tr >");
                        sbResponse.Append("<td width='73%' style='border: 1px solid #D6D8E7;'> <b> Service Information </b> ");
                        sbResponse.Append("</td>");
                        sbResponse.Append("<td style='border: 1px solid #D6D8E7;'> <b> Fare Information </b> ");
                        sbResponse.Append("</td>");
                        sbResponse.Append("</tr>");
                        sbResponse.Append("</table>");

                        sbResponse.Append("<table>");
                        sbResponse.Append("<tr>");

                        sbResponse.Append("<td width='45%' valign='top' align='left'>");

                        sbResponse.Append("<table width='100%' style='font-size: 15px;' >");

                        sbResponse.Append("<tbody>");


                        if (paxSegmentDetails != null && paxSegmentDetails.Any())
                        {
                            foreach (var paxSegment in paxSegmentDetails)
                            {
                                if (paxSegment.Conn != null && paxSegment.Conn.Equals("O"))
                                {
                                    string maealbagagge = paxSegment.ChargeType.Equals("M") ? "Meal" : "Baggage";
                                    sbResponse.Append("<tr>");
                                    sbResponse.Append("<td><b>* </b>" + Originn + "-" + Destinationn + " added " + maealbagagge + "<span>&nbsp;" + paxSegment.ChargeDescription + "</span>  <span>" + paxSegment.Charge_Amount + "Rs.</span></td>");
                                    sbResponse.Append("</tr>");
                                }
                                else
                                {
                                    string maealbagagge = paxSegment.ChargeType.Equals("M") ? "Meal" : "Baggage";
                                    sbResponse.Append("<tr>");
                                    sbResponse.Append("<td><b>* </b>" + Destinationn + "-" + Originn + " added " + maealbagagge + "<span>&nbsp;" + paxSegment.ChargeDescription + "</span>  <span>" + paxSegment.Charge_Amount + "Rs.</span></td>");
                                    sbResponse.Append("</tr>");
                                }
                            }
                        }
                        if (paxSegmentRuleDetails != null && paxSegmentRuleDetails.Any())
                        {
                            foreach (var paxSegment in paxSegmentRuleDetails)
                            {
                                if (paxSegment.Conn != null && paxSegment.Conn == "O")
                                {
                                    sbResponse.Append("<tr>");
                                    if (paxSegment.BaggageDetail != null && paxSegment.BaggageDetail.IndexOf("*") != -1)
                                    {
                                        string bagg = paxSegment.BaggageDetail;
                                        sbResponse.Append("<td><b>* </b>" + Originn + "-" + Destinationn + " available baggage on per pax  <span>" + bagg.Split('*')[0].Trim() + " Check IN </span>  </td>");
                                    }
                                    else
                                    {
                                        sbResponse.Append("<td><b>* </b>" + Originn + "-" + Destinationn + " available baggage on per pax  <span>Check IN: APAP </span>  </td>");
                                    }
                                    sbResponse.Append("</tr>");
                                }
                                else
                                {
                                    sbResponse.Append("<tr>");
                                    if (paxSegment.BaggageDetail != null && paxSegment.BaggageDetail.IndexOf("*") != -1)
                                    {
                                        string bagg = paxSegment.BaggageDetail;
                                        sbResponse.Append("<td><b>* </b>" + Destinationn + "-" + Originn + " available baggage on per pax  <span>" + bagg.Split('*')[0].Trim() + " Check IN </span>  </td>");
                                    }
                                    else
                                    {
                                        sbResponse.Append("<td><b>* </b>" + Destinationn + "-" + Originn + " available baggage on per pax  <span>Check IN: APAP </span>  </td>");
                                    }
                                    sbResponse.Append("</tr>");
                                }
                            }
                        }

                        sbResponse.Append("<tr>");
                        sbResponse.Append("<td>This is an Electronic ticket. Please carry a positive identification for Check in.</td>");
                        sbResponse.Append("</tr>");

                        sbResponse.Append("</tbody>");
                        sbResponse.Append("</table>");
                        sbResponse.Append("</td>");

                        sbResponse.Append("<td width='28%' valign='top' align='right'>");
                        if (flightDetails != null && flightDetails.Any())
                        {
                            string adt = flightDetails.FirstOrDefault().Adt.ToString().Trim();
                            string chd = flightDetails.FirstOrDefault().Chd.ToString().Trim();
                            string inf = flightDetails.FirstOrDefault().Inf.ToString().Trim();
                            int TotalServiceFee = NumberHelper.ConvertToInt(flightDetails.FirstOrDefault().TotalServiceFeeDeal);
                            int TotalServiceTax = NumberHelper.ConvertToInt(flightDetails.FirstOrDefault().TotalServiceTax);
                            int TotalMealcharges = NumberHelper.ConvertToInt(flightDetails.FirstOrDefault().TotalMeal);
                            int TotalBaggCharges = NumberHelper.ConvertToInt(flightDetails.FirstOrDefault().TotalBaggage);
                            int TotalFare = NumberHelper.ConvertToInt(flightDetails.FirstOrDefault().TotalFare);
                            int TotalTDS = NumberHelper.ConvertToInt(flightDetails.FirstOrDefault().TotalTds);
                            int TotalMarkup = NumberHelper.ConvertToInt(flightDetails.FirstOrDefault().TotalMarkup);
                            int TotalBasic = NumberHelper.ConvertToInt(flightDetails.FirstOrDefault().TotalBasic);
                            int TotalTax = NumberHelper.ConvertToInt(flightDetails.FirstOrDefault().TotalTax) + TotalMarkup;
                            int TotalCommission = NumberHelper.ConvertToInt(flightDetails.FirstOrDefault().TotalCommission);
                            TotalTax += TotalMarkup;

                            TotalFare = (TotalFare + TotalMealcharges + TotalBaggCharges + SurchargeAmount) - (TotalTDS);

                            if (Tax != null && Tax != "")
                            {
                                if (Trip == "R" && IsSingleFare.Equals(true) || Trip == "O")
                                {
                                    int noofpx = Convert.ToInt32(adt) + Convert.ToInt32(chd);
                                    int tottax = noofpx * Convert.ToInt32(Tax);
                                    TotalTax = TotalTax + tottax;
                                    TotalFare = TotalFare + tottax;
                                }
                                if (flightDetails.FirstOrDefault().Trip.ToString().Equals("R") && IsSingleFare.Equals(false))
                                {
                                    int noofpx = Convert.ToInt32(adt) + Convert.ToInt32(chd);
                                    noofpx = noofpx * 2;
                                    int tottax = noofpx * Convert.ToInt32(Tax);
                                    TotalTax = TotalTax + tottax;
                                    TotalFare = TotalFare + tottax;
                                }
                            }
                            sbResponse.Append("<table width='100%' style='font-size:15px;' >");
                            sbResponse.Append("<tbody>");

                            sbResponse.Append("<tr>");
                            sbResponse.Append("<td align='right'>BFare :</td>");
                            sbResponse.Append("<td align='right'>" + TotalBasic + "" + Currency + "</td>");
                            sbResponse.Append("</tr>");
                            sbResponse.Append("<tr>");
                            sbResponse.Append("<td align='right'>  Tax :  </td>");
                            sbResponse.Append("<td align='right'> <span id='oldtotaltax'>" + TotalTax + "" + Currency + "</span> </td>");
                            sbResponse.Append("</tr>");
                            sbResponse.Append("<tr>");
                            sbResponse.Append("<td align='right'> Fee & Surcharge :  </td>");
                            sbResponse.Append("<td align='right'>" + (TotalServiceFee + TotalServiceTax).ToString() + "" + Currency + "</td>");
                            sbResponse.Append("</tr>");


                            if (Disc != null && Disc != "")
                            {

                                sbResponse.Append("<tr id='Discountdiv'  align='right' >");
                                sbResponse.Append("<td align='right'> Discount(-) :</td>");
                                sbResponse.Append("<td align='right'>Rs <span id='spndiscount'>" + Disc + "" + Currency + "</span></td>");
                                sbResponse.Append("</tr>");
                                TotalFare = TotalFare - Convert.ToInt32(Disc);
                            }
                            sbResponse.Append("<tr>");
                            sbResponse.Append("<td align='right'> Services :  </td>");
                            sbResponse.Append("<td align='right'>" + (TotalMealcharges + TotalBaggCharges).ToString() + "" + Currency + "</td>");
                            sbResponse.Append("</tr>");


                            sbResponse.Append("<tr>");
                            sbResponse.Append("<td align='right'> Conv.fee :  </td>");
                            sbResponse.Append("<td align='right'>" + (SurchargeAmount).ToString() + "" + Currency + "</td>");
                            sbResponse.Append("</tr>");

                            sbResponse.Append("<tr >");
                            sbResponse.Append("<td align='right'> Total Amount :  </td>");
                            sbResponse.Append("<td align='right'><span id='oldtotalamt'>" + TotalFare + "" + Currency + "</span></td>");
                            sbResponse.Append("</tr>");

                            sbResponse.Append("</tbody>");
                            sbResponse.Append("</table>");

                        }
                        sbResponse.Append("</td>");

                        sbResponse.Append("</tr>");


                        sbResponse.Append("</table>");
                        sbResponse.Append("</td>");

                        sbResponse.Append("</tr>");
                        #endregion
                    }      //======Fare Detail partr End=============//
                    else
                    {
                        //======Fare Detail partr new start=============//
                        #region
                        sbResponse.Append("<tr id='divfaredetailnew'>");
                        sbResponse.Append("<td>");

                        sbResponse.Append("<table width='100%' style='' cellpadding='10' >");
                        sbResponse.Append("<tr style='font-size:15px;'>");

                        sbResponse.Append("<td  style='border: 1px solid #D6D8E7;' colspan='2'><b> Service Information </b>");
                        sbResponse.Append("</td>");



                        sbResponse.Append("</tr>");
                        sbResponse.Append("</table>");

                        sbResponse.Append("<table>");
                        sbResponse.Append("<tr>");
                        sbResponse.Append("<td width='45%' valign='top' align='left'>");
                        sbResponse.Append("<table width='100%' style='font-size: 15px;' >");
                        sbResponse.Append("<tbody>");


                        if (paxSegmentDetails.Any())
                        {
                            foreach (var paxSegment in paxSegmentDetails)
                            {
                                if (paxSegment.Conn != null && paxSegment.Conn.Equals("O"))
                                {
                                    string maealbagagge = "";
                                    if (paxSegment.ChargeType.Equals("M"))
                                    {
                                        maealbagagge = "Meal";
                                    }
                                    else
                                    {
                                        maealbagagge = "Baggage";
                                    }

                                    sbResponse.Append("<tr>");
                                    sbResponse.Append("<td><b>* </b>" + Originn + "-" + Destinationn + " added " + maealbagagge + "<span>&nbsp;" + paxSegment.ChargeDescription + "</span>  <span>" + paxSegment.Charge_Amount + "Rs.</span></td>");
                                    sbResponse.Append("</tr>");
                                }
                                else
                                {
                                    string maealbagagge = "";
                                    if (paxSegment.ChargeType.Equals("M"))
                                    {
                                        maealbagagge = "Meal";
                                    }
                                    else
                                    {
                                        maealbagagge = "Baggage";
                                    }

                                    sbResponse.Append("<tr>");
                                    sbResponse.Append("<td><b>* </b>" + Destinationn + "-" + Originn + " added " + maealbagagge + "<span>&nbsp;" + paxSegment.ChargeDescription + "</span>  <span>" + paxSegment.Charge_Amount + "Rs.</span></td>");
                                    sbResponse.Append("</tr>");
                                }
                            }
                        }


                        if (paxSegmentRuleDetails != null && paxSegmentRuleDetails.Any())
                        {
                            foreach (var paxSegment in paxSegmentRuleDetails)
                            {
                                if (paxSegment.Conn != null && paxSegment.Conn.Equals("O"))
                                {
                                    sbResponse.Append("<tr>");
                                    if (paxSegment.BaggageDetail != null && paxSegment.BaggageDetail.IndexOf("*") != -1)
                                    {
                                        string bagg = paxSegment.BaggageDetail;
                                        sbResponse.Append("<td><b>* </b>" + Originn + "-" + Destinationn + " available baggage on per pax  <span>" + bagg.Split('*')[0].Trim() + " Check IN </span>  </td>");
                                    }
                                    else
                                    {
                                        sbResponse.Append("<td><b>* </b>" + Originn + "-" + Destinationn + " available baggage on per pax  <span>Check IN: APAP </span>  </td>");
                                    }
                                    sbResponse.Append("</tr>");
                                }
                                else
                                {
                                    sbResponse.Append("<tr>");
                                    if (paxSegment.BaggageDetail != null && paxSegment.BaggageDetail.IndexOf("*") != -1)
                                    {
                                        string bagg = paxSegment.BaggageDetail;
                                        sbResponse.Append("<td><b>* </b>" + Destinationn + "-" + Originn + " available baggage on per pax  <span>" + bagg.Split('*')[0].Trim() + " Check IN </span>  </td>");
                                    }
                                    else
                                    {
                                        sbResponse.Append("<td><b>* </b>" + Destinationn + "-" + Originn + " available baggage on per pax  <span>Check IN: APAP </span>  </td>");
                                    }
                                    sbResponse.Append("</tr>");
                                }
                            }
                        }
                        sbResponse.Append("<tr>");
                        sbResponse.Append("<td>This is an Electronic ticket. Please carry a positive identification for Check in.</td>");
                        sbResponse.Append("</tr>");
                        sbResponse.Append("</tbody>");
                        sbResponse.Append("</table>");
                        sbResponse.Append("</td>");

                        sbResponse.Append("<td width='28%' valign='top' align='right'>");
                        sbResponse.Append("</td>");

                        sbResponse.Append("</tr>");


                        sbResponse.Append("</table>");
                        sbResponse.Append("</td>");

                        sbResponse.Append("</tr>");
                        #endregion
                        //======Fare Detail partr new End=============//
                    }
                }




                //======GST Detail start=============//
                var gstDetail = bookingDetail.CompanyFlightGSTDetails.FirstOrDefault();
                if (gstDetail != null)
                {


                    sbResponse.Append("<tr>");
                    sbResponse.Append("<td>");
                    sbResponse.Append("<table width='100%' style='border: 1px solid #D6D8E7;' cellspacing='0' cellpadding='3' >");
                    sbResponse.Append("<tr style='background: #F1F0F0; font-size:13px;'>");
                    sbResponse.Append("<td width='73%' style=''>GST Detail");
                    sbResponse.Append("</td>");
                    sbResponse.Append("</tr>");
                    sbResponse.Append("</table>");
                    sbResponse.Append("</td>");
                    sbResponse.Append("</tr>");
                    sbResponse.Append("<tr>");
                    sbResponse.Append("<td>");
                    sbResponse.Append("<table width='100%' style='font-size:11px;' cellspacing='0' cellpadding='3' >");
                    sbResponse.Append("<tr>");
                    sbResponse.Append("<td>Gst CompanyName:</td>");
                    sbResponse.Append("<td>" + StringHelper.GetUpper(gstDetail.GstcompanyName.ToString().Trim()) + "</td>");
                    sbResponse.Append("</tr>");
                    sbResponse.Append("<tr>");
                    sbResponse.Append("<td>Gst Number:</td>");
                    sbResponse.Append("<td>" + StringHelper.GetUpper(gstDetail.Gstnumber.ToString().Trim()) + "</td>");
                    sbResponse.Append("</tr>");
                    sbResponse.Append("</table>");
                    sbResponse.Append("</td>");
                    sbResponse.Append("</tr>");

                }
                //======GST Detail End =============//



                sbResponse.Append("<tr>");
                sbResponse.Append("<td>");
                sbResponse.Append("<table width='100%' style='border: 1px solid #D6D8E7;' cellspacing='0' cellpadding='10' >");
                sbResponse.Append("<tr style=' font-size:15px;'>");
                sbResponse.Append("<td width='73%' style=''> <b> Terms & Conditions </b> ");
                sbResponse.Append("</td>");
                sbResponse.Append("</tr>");
                sbResponse.Append("</table>");
                sbResponse.Append("</td>");
                sbResponse.Append("</tr>");


                sbResponse.Append("<tr>");
                sbResponse.Append("<td>");
                sbResponse.Append("<table width='100%' style='font-size:10px;' cellspacing='0' cellpadding='3' >");
                sbResponse.Append("<tr>");
                sbResponse.Append("<td><b>*</b> All Guests, including children and infants, must present valid identification at check-in.");
                sbResponse.Append("</td>");
                sbResponse.Append("</tr>");
                sbResponse.Append("<tr>");
                sbResponse.Append("<td><b>*</b> Carriage and other services provided by the carrier are subject to conditions of carriage, which are hereby incorporated by reference. These conditions may be obtained from the issuing carrier.");
                sbResponse.Append("</td>");
                sbResponse.Append("</tr>");
                sbResponse.Append("<tr>");
                sbResponse.Append("<td><b>*</b> Changes to your reservation will result in a fee of Airline terms & condition,change plus any difference in the fare between the original fare paid and the fare for the revised booking.");
                sbResponse.Append("</td>");
                sbResponse.Append("</tr>");
                sbResponse.Append("<tr>");
                sbResponse.Append("<td><b>*</b> Cancellation charges as per Airlines terms & condition.");
                sbResponse.Append("</td>");
                sbResponse.Append("</tr>");

                sbResponse.Append("<tr>");
                sbResponse.Append("<td><b>*</b> Airline Free Baggages are per seat only.");
                sbResponse.Append("</td>");
                sbResponse.Append("</tr>");

                sbResponse.Append("<tr>");
                sbResponse.Append("<td><b>*</b> Check your itinerary with airline also,revert with in 2 hour. we are here to help you.");
                sbResponse.Append("</td>");
                sbResponse.Append("</tr>");
                sbResponse.Append("<tr>");
                sbResponse.Append("<td><b>*</b> As per Government guidelines, check-in counters at all airports will now close 45 minutes before departure with immediate effect. Please plan your Airport arrival accordingly.");
                sbResponse.Append("</td>");
                sbResponse.Append("</tr>");
                sbResponse.Append("<tr>");
                sbResponse.Append("<td><b>*</b> Wish You a Happy Journey.");
                sbResponse.Append("</td>");
                sbResponse.Append("</tr>");

                sbResponse.Append("</table>");
                sbResponse.Append("</td>");
                sbResponse.Append("</tr>");
                sbResponse.Append("</table>");
                sbResponse.Append("</div>");

            }
            catch (Exception ex)
            {
                DBLoggerHelper.DBLogAsync("", iBookingRef, "Print_Popup_Tkt", "PrintPopupController", "", "", ex.Message, _addDbLogCommandHandler);
            }
            return sbResponse.ToString();
        }

        private string GetMealDetails(string Pax_SegmentID, List<CompanyPaxSegmentDetailAirlinesData> dtPaxSegment)
        {
            string MealDes = string.Empty;
            if (dtPaxSegment != null && dtPaxSegment.Any())
            {
                var drSelect = dtPaxSegment
        .Where(f => f.Pax_SegmentId.ToString() == Pax_SegmentID && f.ChargeType == "M")
        .ToList();
                if (drSelect.Any())
                {
                    foreach (var dr in drSelect)
                    {
                        if (dr.Conn == "O")
                        {
                            MealDes = dr.ChargeDescription;
                        }
                        else if (dr.Conn == "I")
                        {
                            MealDes += "/" + dr.ChargeDescription;
                        }
                    }
                }
            }
            return MealDes;
        }
        private string GetBaggDetails(string Pax_SegmentID, List<CompanyPaxSegmentDetailAirlinesData> dtPaxSegment)
        {
            string Baggage = string.Empty;
            if (dtPaxSegment != null && dtPaxSegment.Any())
            {
                var drSelect = dtPaxSegment
        .Where(f => f.Pax_SegmentId.ToString() == Pax_SegmentID && f.ChargeType == "B")
        .ToList();
                if (drSelect.Any())
                {
                    foreach (var dr in drSelect)
                    {
                        if (dr.Conn == "O")
                        {
                            Baggage = dr.ChargeDescription.ToString();
                        }
                        else if (dr.Conn.ToString() == "I")
                        {
                            Baggage = Baggage + "/" + dr.ChargeDescription.ToString();
                        }
                    }
                }
            }
            return Baggage;
        }
        private string GetSectorWiseDetailsOB(List<CompanyFlightDetailAirlinesData> flightDetail, List<CompanyFlightSegmentDetailAirlinesData> flightSegmentDetail, bool isItCallingInPDFMethod = false)
        {
            string Trip = flightDetail.FirstOrDefault().Trip;
            string Adt = flightDetail.FirstOrDefault().Adt.ToString();
            string Chd = flightDetail.FirstOrDefault().Chd.ToString();
            string Inf = flightDetail.FirstOrDefault().Inf.ToString();
            int TotalPax = Convert.ToInt32(Adt) + Convert.ToInt32(Chd) + Convert.ToInt32(Inf);

            string _HTMLItinery = "";
            string CarrierCode = "";

            string origin = string.Empty;
            string destination = string.Empty;
            string depart_d = string.Empty;
            int temp = 0;

            _HTMLItinery = _HTMLItinery + "<table width='100%' style='border: 1px solid #D6D8E7;' cellspacing='0' cellpadding='10' >";
            _HTMLItinery = _HTMLItinery + "<tr style='font-size:15px;'>";
            _HTMLItinery = _HTMLItinery + "<td width='23%' style='border-bottom: 1px solid #D6D8E7;'><b> Flight </b></td>";
            _HTMLItinery = _HTMLItinery + "<td width='26%' style='border-bottom: 1px solid #D6D8E7;'><b> Departure </b></td>";
            _HTMLItinery = _HTMLItinery + "<td width='24%' style='border-bottom: 1px solid #D6D8E7;'><b> Arrival </b></td>";
            string Pnr = "Unconfirmed";


            if (flightDetail.FirstOrDefault().Origin != "")
            {
                origin = flightDetail.FirstOrDefault().Origin;
            }
            if (flightDetail.FirstOrDefault().Destination != "")
            {
                destination = flightDetail.FirstOrDefault().Destination;
            }
            if (flightDetail.FirstOrDefault().DepartureDate_D != "")
            {
                depart_d = flightDetail.FirstOrDefault().DepartureDate_D;
                depart_d = StringHelper.GetFirstCharacterCapital(depart_d);
            }
            if (flightDetail.FirstOrDefault().Airline_Pnr_D != "")
            {
                Pnr = flightDetail.FirstOrDefault().Airline_Pnr_D;
            }

            _HTMLItinery = _HTMLItinery + "</tr>";
            foreach (var paxSegment in flightSegmentDetail)
            {
                if (temp == 0)
                {
                    temp = temp + 1;
                }
                string CarrierName = "";
                string ConnOrder;
                if (paxSegment.ConnOrder != "")
                {
                    ConnOrder = paxSegment.ConnOrder;
                }
                int n = paxSegment.ConnOrder.IndexOf("O");
                if (n > -1)
                {
                    CarrierCode = ""; CarrierName = "";
                    if (paxSegment.CarrierName != "")
                    {
                        CarrierName = paxSegment.CarrierName;
                    }
                    if (paxSegment.CarrierCode != "")
                    {
                        CarrierCode = paxSegment.CarrierCode;
                    }
                    string GdsPnr = "";
                    string ClassOfService = "";
                    GdsPnr = "Unconfirmed";
                    if (paxSegment.Gds_Pnr != "")
                    {
                        GdsPnr = paxSegment.Gds_Pnr;
                    }
                    if (paxSegment.ClassOfService != "")
                    {
                        ClassOfService = paxSegment.ClassOfService;
                    }
                    string FareType = "";
                    if (paxSegment.RefundType != "")
                    {
                        string _FareType = paxSegment.RefundType;
                        if (_FareType == "N")
                        {
                            if (isItCallingInPDFMethod)
                            {
                                FareType = "<img src='\\wwwroot\\assets\\img\\Commonimages\\refunf.png' alt='Refundable' />";
                            }
                            else
                            {
                            FareType = "<img src='/assets/img/Commonimages/refunf.png' alt='Refundable' />";
                            }
                        }
                        else
                        {
                            if (isItCallingInPDFMethod)
                            {

                                FareType = "<img src='wwwroot\\assets\\img\\Commonimages\\non-refund.png' alt='Non Refundable' /> ";
                            }else
                            {

                            FareType = "<img src='wwwroot\\assets\\img\\Commonimages\\non-refund.png' alt='Non Refundable' /> ";
                            }
                        }
                    }
                    string Cabin = "Class";
                    if (paxSegment.Cabin != "")
                    {
                        Cabin = paxSegment.Cabin;
                        Cabin = Cabin.Substring(0, 1).ToUpper() + Cabin.Substring(1, Cabin.Length - 1).ToLower();
                    }

                    _HTMLItinery = _HTMLItinery + "<tr>";
                    _HTMLItinery = _HTMLItinery + "<td valign='top' width='20%' style='font-size:15px' >";
                    if (isItCallingInPDFMethod)
                    {
                        _HTMLItinery = _HTMLItinery + "<span style='margin-top: 5px;width: max-content;height: 29px;float: left;margin-right: 10px;'><img src='wwwroot\\assets\\img\\airlogo_square\\" + CarrierCode + ".gif' height='30px' width='30px' alt='" + CarrierCode + "' />";
                    }
                    else
                    {
                        _HTMLItinery = _HTMLItinery + "<span style='margin-top: 5px;width: max-content;height: 29px;float: left;margin-right: 10px;'><img src='assets/img/airlogo_square/" + CarrierCode + ".gif' height='30px' width='30px' alt='" + CarrierCode + "' />";
                    }
                   
                    _HTMLItinery = _HTMLItinery + "</span><span style='margin-top: 5px;width: 70%;float: left;'>";
                    _HTMLItinery = _HTMLItinery + "<span style='margin-left: 5px;'>" + CarrierName + "</span><span style='margin-left: 5px;'>" + CarrierCode + "</span><span style='margin-left: 5px;'>" + paxSegment.FlightNumber + "</span><span style='margin-left: 5px;'>" + ClassOfService + "</span><span style='margin-left:5px;'> Class</span><br /></span>";
                    _HTMLItinery = _HTMLItinery + "</td>";
                    string DepartureStation = "";
                    if (paxSegment.sDepartureStation != "")
                    {
                        DepartureStation = paxSegment.sDepartureStation;
                    }
                    string DepartureTerminal = "";
                    if (paxSegment.DepartureTerminal != "")
                    {
                        DepartureTerminal = paxSegment.DepartureTerminal;
                    }
                    string DepartureStationAirport = "";
                    if (paxSegment.DepartureStationAirport != "")
                    {
                        DepartureStationAirport = paxSegment.DepartureStationAirport;
                    }
                    string DepartureDate = "";
                    if (paxSegment.DepartureDate != "")
                    {
                        DepartureDate = StringHelper.GetFirstCharacterCapital(paxSegment.DepartureDate);
                    }

                    _HTMLItinery = _HTMLItinery + "<td  width='40%' valign='top' style='font-size:15px'>";
                    _HTMLItinery = _HTMLItinery + "<span style='margin-top: 5px; width: 100%; float: left;margin-left: 5px;'>" + paxSegment.DepartureStation + " (" + DepartureStationAirport + " , " + DepartureStation + " ) </span><span style='margin-top: 5px; width: 100%; float: left;margin-left: 5px;'>" + paxSegment.DepartureTime + ", " + DepartureDate + "</span>";

                    _HTMLItinery = _HTMLItinery + "<span style = 'margin-top: 5px; width: 100%; float: left;margin-left: 5px;'>Terminal-" + DepartureTerminal + " </ span > ";

                    _HTMLItinery = _HTMLItinery + "</td>";
                    string ArrivalStation = "";
                    if (paxSegment.sArrivalStation != "")
                    {
                        ArrivalStation = paxSegment.sArrivalStation;
                    }
                    string ArrivalTerminal = "";
                    if (paxSegment.ArrivalTerminal != "")
                    {
                        ArrivalTerminal = paxSegment.ArrivalTerminal;
                    }

                    string ArrivalStationAirport = "";
                    if (paxSegment.ArrivalStationAirport != "")
                    {
                        ArrivalStationAirport = paxSegment.ArrivalStationAirport;
                    }

                    string ArrivalDate = "";
                    if (paxSegment.ArrivalDate != "")
                    {
                        ArrivalDate = StringHelper.GetFirstCharacterCapital(paxSegment.ArrivalDate);
                    }
                    _HTMLItinery = _HTMLItinery + "<td width='40%' valign='top' style='font-size:15px'>";
                    _HTMLItinery = _HTMLItinery + "<span style='margin-top: 5px; width: 100%; float: left;margin-left: 5px;'> " + paxSegment.ArrivalStation + " (" + ArrivalStationAirport + ",   " + ArrivalStation + ") </span>";
                    _HTMLItinery = _HTMLItinery + "<span style='margin-top: 5px; width: 100%; float: left;margin-left: 5px;'>" + paxSegment.ArrivalTime + " , " + StringHelper.GetFirstCharacterCapital(paxSegment.ArrivalDate) + " </span>";
                    _HTMLItinery = _HTMLItinery + "<span style = 'margin-top: 5px; width: 100%; float: left;margin-left: 5px;'>Terminal-" + ArrivalTerminal + " </ span > ";
                    _HTMLItinery = _HTMLItinery + "</td>";
                    _HTMLItinery = _HTMLItinery + "</tr>";
                }
            }

            _HTMLItinery = _HTMLItinery + "</table>";
            return _HTMLItinery;
        }
        private string GetSectorWiseDetailsIB(List<CompanyFlightDetailAirlinesData> flightDetail, List<CompanyFlightSegmentDetailAirlinesData> flightSegmentDetail,bool isItCallingInPDFMethod = false)
        {

            string Trip = flightDetail.FirstOrDefault().Trip;
            string Adt = flightDetail.FirstOrDefault().Adt.ToString();
            string Chd = flightDetail.FirstOrDefault().Chd.ToString();
            string Inf = flightDetail.FirstOrDefault().Inf.ToString();

            int TotalPax = Convert.ToInt32(Adt) + Convert.ToInt32(Chd) + Convert.ToInt32(Inf);
            string _HTMLItinery = "";
            string CarrierCode = "";
            string origin = string.Empty;
            string destination = string.Empty;
            string depart_A = string.Empty;


            if (flightDetail.FirstOrDefault().Airline_Pnr_A != "")
            {
            }
            if (flightDetail.FirstOrDefault().Origin != "")
            {
                origin = flightDetail.FirstOrDefault().Origin;
            }
            if (flightDetail.FirstOrDefault().Destination != "")
            {
                destination = flightDetail.FirstOrDefault().Destination;
            }
            if (flightDetail.FirstOrDefault().DepartureDate_A != "")
            {
                depart_A = flightDetail.FirstOrDefault().DepartureDate_A;
                depart_A = StringHelper.GetFirstCharacterCapital(depart_A);
            }

            _HTMLItinery = _HTMLItinery + "<table width='100%' style='border: 1px solid #D6D8E7;' cellspacing='0' cellpadding='10'> ";
            _HTMLItinery = _HTMLItinery + "<tr style='font-size:15px;'>";
            _HTMLItinery = _HTMLItinery + "<td  width='23%' style='border-bottom: 1px solid #D6D8E7;'> <b> Flight </b> </td>";
            _HTMLItinery = _HTMLItinery + "<td  width='26%' style='border-bottom: 1px solid #D6D8E7;'> <b> Departure </b> </td>";
            _HTMLItinery = _HTMLItinery + "<td  width='24%' style='border-bottom: 1px solid #D6D8E7;'> <b> Arrival </b> </td>";

            _HTMLItinery = _HTMLItinery + "</tr>";
            int temp = 0;
            foreach (var paxSegment in flightSegmentDetail)
            {
                var ConnOrder = "";
                ConnOrder = paxSegment.ConnOrder != "" ? paxSegment.ConnOrder : "";
                if (temp == 0)
                {
                    int n = paxSegment.ConnOrder.IndexOf("I");
                    if (n > -1)
                    {
                        temp = temp + 1;
                    }
                }
                string CarrierName = "";
                int m = paxSegment.ConnOrder.IndexOf("I");
                if (m > -1)
                {



                    CarrierCode = paxSegment.CarrierCode != "" ? paxSegment.CarrierCode : "";
                    CarrierName = paxSegment.CarrierName != "" ? paxSegment.CarrierName : "";
                    string GdsPnr = "";
                    GdsPnr = paxSegment.Gds_Pnr != "" ? paxSegment.Gds_Pnr : "Unconfirmed";
                    string ClassOfService = "";
                    ClassOfService = paxSegment.ClassOfService != "" ? paxSegment.ClassOfService : "";

                    string FareType = "";
                    if (paxSegment.RefundType != "")
                    {
                        string _FareType = paxSegment.RefundType;
                        if (_FareType == "N")
                        {
                            if (isItCallingInPDFMethod)
                            {
                                FareType = "<img src='wwwroot\\assets\\img\\Commonimages\\refunf.png' alt='Refundable' />";

                            }
                            else
                            {

                            FareType = "<img src='wwwroot\\assets\\img\\Commonimages\\refunf.png' alt='Refundable' />";
                            }
                        }
                        else
                        {
                            if (isItCallingInPDFMethod)
                            { 
                                FareType = "<img src='wwwroot\\assets\\img\\Commonimages\\non-refund.png' alt='Non Refundable' />";
                            }
                            else
                            {

                                FareType = "<img src='/assets/img/Commonimages/non-refund.png' alt='Non Refundable' />";
                            }
                        }
                    }
                    string Cabin = "Class";
                    if (paxSegment.Cabin != "")
                    {
                        Cabin = paxSegment.Cabin;
                        Cabin = Cabin.Substring(0, 1).ToUpper() + Cabin.Substring(1, Cabin.Length - 1).ToLower();
                    }
                    _HTMLItinery = _HTMLItinery + "<tr >";
                    _HTMLItinery = _HTMLItinery + "<td valign='top' width='20%' style='font-size:15px'>";
                    if (isItCallingInPDFMethod)
                    {
                    _HTMLItinery = _HTMLItinery + "<span style='width:max-content;margin-right:10px;margin-top: 5px;height: 29px;float: left;'><img src='wwwroot\\assets\\img\\airlogo_square\\" + CarrierCode + ".gif' height='30px' width='30px' alt='" + CarrierCode + "' />";
                    }
                    else
                    {
                    _HTMLItinery = _HTMLItinery + "<span style='width:max-content;margin-right:10px;margin-top: 5px;height: 29px;float: left;'><img src='/assets/img/airlogo_square/" + CarrierCode + ".gif' height='30px' width='30px' alt='" + CarrierCode + "' />";
                    }
                    _HTMLItinery = _HTMLItinery + "</span><span style='margin-top: 5px;width: 70%;float: left;'>";
                    _HTMLItinery = _HTMLItinery + "<span style='margin-left: 5px;'>" + CarrierName + "</span><span style='margin-left: 5px;'>" + CarrierCode + "</span><span style='margin-left: 5px;'>" + paxSegment.FlightNumber + "</span><span style='margin-left: 5px;'>" + ClassOfService + "</span><span style='margin-left:5px;'> Class</span><br /></span>";
                    _HTMLItinery = _HTMLItinery + "</td>";

                    string DepartureStation = paxSegment.sOrigin != "" ? paxSegment.sOrigin : "";
                    string DepartureTerminal = paxSegment.DepartureTerminal != "" ? paxSegment.DepartureTerminal : "";
                    string DepartureStationAirport = paxSegment.DepartureStationAirport != "" ? paxSegment.DepartureStationAirport : "";
                    string DepartureDate = paxSegment.DepartureDate != "" ? paxSegment.DepartureDate : "";



                    string ArrivalStation = paxSegment.sDestination != "" ? paxSegment.sDestination : "";
                    string ArrivalTerminal = paxSegment.ArrivalTerminal != "" ? paxSegment.ArrivalTerminal : "";
                    string ArrivalStationAirport = paxSegment.ArrivalStationAirport != "" ? paxSegment.ArrivalStationAirport : "";
                    string ArrivalDate = paxSegment.ArrivalDate != "" ? paxSegment.ArrivalDate : "";
                    _HTMLItinery = _HTMLItinery + "<td valign='top' width='40%' style='font-size:15px;' >";
                    _HTMLItinery = _HTMLItinery + "<span style='margin-top: 5px; width: 100%; float: left;margin-left: 5px;'>" + paxSegment.DepartureStation + "  (" + DepartureStationAirport + " , " + paxSegment.sDepartureStation + "  ) </span>";
                    _HTMLItinery = _HTMLItinery + "<span style='margin-top: 5px; width: 100%; float: left;margin-left: 5px;'>" + paxSegment.DepartureTime + ", " + StringHelper.GetFirstCharacterCapital(DepartureDate) + "</span>";
                    _HTMLItinery = _HTMLItinery + "<span style = 'margin-top: 5px; width: 100%; float: left;margin-left: 5px;'> Terminal-" + DepartureTerminal + " </ span > ";
                    _HTMLItinery = _HTMLItinery + "</td>";
                    _HTMLItinery = _HTMLItinery + "<td valign='top' width='40%' style='font-size:15px;'>";
                    _HTMLItinery = _HTMLItinery + "<span style='margin-top: 5px; width: 100%; float: left;margin-left: 5px;'> " + paxSegment.ArrivalStation + " (" + ArrivalStationAirport + ",   " + paxSegment.sArrivalStation + ") </span>";
                    _HTMLItinery = _HTMLItinery + "<span style='margin-top: 5px; width: 100%; float: left;margin-left: 5px;'>  " + paxSegment.ArrivalTime + " , " + StringHelper.GetFirstCharacterCapital(paxSegment.ArrivalDate) + " </span>";
                    _HTMLItinery = _HTMLItinery + "<span style = 'margin-top: 5px; width: 100%; float: left;margin-left: 5px;'> Terminal-" + ArrivalTerminal + " </ span > ";
                    _HTMLItinery = _HTMLItinery + " </td>";
                    _HTMLItinery = _HTMLItinery + "</tr>";
                    _HTMLItinery = _HTMLItinery + "<tr>";
                }

            }
            _HTMLItinery = _HTMLItinery + "</table>";
            return _HTMLItinery;
        }
        public string Getbookingstatus(List<CompanyFlightDetailAirlinesData> flightDetail, List<CompanyPaxDetailAirlinesData> paxDetail,CompanyFareDetailAirlinesData fareDetails)
        {
            string status = "Unconfirm";
            if (flightDetail.FirstOrDefault().IsCancelRequested.Value || flightDetail.FirstOrDefault().IsCanceled.Value || flightDetail.FirstOrDefault().IsCanceledRejected.Value)
            {
                status = "Cancel";
                if (flightDetail.FirstOrDefault().Trip.ToString().Equals("o") || flightDetail.FirstOrDefault().Trip.ToString().Equals("m"))
                {
                    foreach (var dr in paxDetail)
                    {
                        if ((dr.Outbound.ToString()).Equals(0))
                        {
                            status = "Confirm";
                            break;
                        }
                    }
                }
                else
                {
                    foreach (var dr in paxDetail)
                    {
                        if ((dr.Outbound.ToString()).Equals(0) || (dr.Inbound.ToString()).Equals(0))
                        {
                            status = "Confirm";
                            break;
                        }
                    }
                }
            }
            else if (flightDetail.FirstOrDefault().IsRejected.Value)
            {
                status = "Rejected";
            }
            else
            {
                if (flightDetail.FirstOrDefault().IsUpdated.Value)
                {
                    bool pnrStatus = _getBookingPnrStatusHandler.HandleAsync(flightDetail.FirstOrDefault()?.BookingRef ?? 0).GetAwaiter().GetResult();
                    if (pnrStatus)
                    {
                        if (fareDetails.IsPaymentHold == true)
                        {
                            return "On Hold";
                        }
                        bool tktStatus = _getTicketStatusHandler.HandleAsync(flightDetail.FirstOrDefault()?.BookingRef ?? 0).GetAwaiter().GetResult();
                        if (tktStatus)
                        {
                            status = "Confirm";
                        }
                    }
                }
            }
            return status;
        }
        public string GetbookingPaxstatus(List<CompanyFlightDetailAirlinesData> flightDetail, List<CompanyPaxDetailAirlinesData> paxDetail, int PaxSegmentID, CompanyFareDetailAirlinesData fareDetails)
        {
            string status = "Unconfirm";
            if (flightDetail.FirstOrDefault().IsCancelRequested.Value || flightDetail.FirstOrDefault().IsCanceled.Value || flightDetail.FirstOrDefault().IsCanceledRejected.Value)
            {
                status = "Cancel";
                if (flightDetail.FirstOrDefault().Trip.ToString().Equals("o") || flightDetail.FirstOrDefault().Trip.ToString().Equals("m"))
                {
                    foreach (var dr in paxDetail)
                    {
                        if ((dr.Outbound.ToString()).Equals(0))
                        {
                            status = "Confirm";
                            break;
                        }
                    }
                }
                else
                {
                    foreach (var dr in paxDetail)
                    {
                        if ((dr.Outbound.ToString()).Equals(0) || (dr.Inbound.ToString()).Equals(0))
                        {
                            status = "Confirm";
                            break;
                        }
                    }
                }
            }
            else if (flightDetail.FirstOrDefault().IsRejected.Value)
            {
                status = "Rejected";
            }
            else
            {
                if (flightDetail.FirstOrDefault().IsUpdated.Value)
                {
                    bool pnrStatus = _getBookingPnrStatusHandler.HandleAsync(flightDetail.FirstOrDefault()?.BookingRef ?? 0).GetAwaiter().GetResult();
                    if (pnrStatus)
                    {
                        if (fareDetails.IsPaymentHold == true)
                        {
                            return "On Hold";
                        }
                        bool tktStatus = _getTicketStatusHandler.HandleAsync(flightDetail.FirstOrDefault()?.BookingRef ?? 0).GetAwaiter().GetResult();
                        if (tktStatus)
                        {
                            status = "Confirm";
                        }
                    }
                }
            }
            return status;
        }
    }

}
