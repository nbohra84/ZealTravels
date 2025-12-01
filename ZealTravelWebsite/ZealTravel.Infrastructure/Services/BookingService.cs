using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZealTravel.Domain.AgencyManagement;
using ZealTravel.Domain.Data.Models;
using ZealTravelWebsite.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using Dapper;
using DocumentFormat.OpenXml.InkML;
using System.Data;
using ZealTravel.Domain.Data.Models.Booking;
using ZealTravel.Domain.BookingManagement;
using ZealTravel.Domain.Data.Entities;
using Microsoft.Data.SqlClient;
using ZealTravel.Common;
using ZealTravel.Domain.Models;
using DocumentFormat.OpenXml.Office.Word;
using System.Collections;
using ZealTravel.Common.Helpers.Flight;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using iText.Commons.Actions.Contexts;
using ZealTravel.Infrastructure.Akaasa;
using ZealTravel.Common.CommonUtility;
using Microsoft.Extensions.Logging;
using ZealTravel.Domain.Data.Models.Agency;

namespace ZealTravel.Infrastructure.Services
{
    public class BookingService : IBookingManagementService
    {

        private readonly ZealdbNContext _context;
        private readonly ILogger<BookingService> _logger;

        public BookingService(ZealdbNContext context, ILogger<BookingService> logger)
        {
            _context = context;
            _logger = logger;

        }


        public async Task<BookingDetail> GetBooking(Int32 bookingRef)
        {
            try
            {
                var connection = _context.Database.GetDbConnection();
                var bookingDetail = new BookingDetail();
                using (var multi = connection.QueryMultiple("getBookingDetail_Proc", new { ProcNo = 1, BookingRef = bookingRef }, commandType: CommandType.StoredProcedure))
                {

                    var CompanyFlightDetailAirlines = multi.Read<CompanyFlightDetailAirlines>().ToList();
                    var Company_Flight_Segment_Detail_Airline = multi.Read<CompanyFlightSegmentDetailAirlines>().ToList();
                    var Company_Fare_Detail_Airline = multi.Read<CompanyFareDetailAirlines>().ToList();
                    var Company_Fare_Detail_Segment_Airline = multi.Read<CompanyFareDetailSegmentAirlines>().ToList();
                    var Company_Pax_Detail_Airline = multi.Read<CompanyPaxDetailAirlines>().ToList();
                    var Company_Pax_Segment_Detail_Airlines = multi.Read<CompanyPaxSegmentDetailAirlines>().ToList();
                    var Company_Flight_Details = multi.Read<CompanyFlightDetails>().ToList();
                    var Company_Flight_Segment_Rule_Detail_Airline = multi.Read<CompanyFlightSegmentRuleDetailAirlines>().ToList();
                    var Company_Flight_GST_Details = multi.Read<CompanyFlightGSTDetails>().ToList();
                    var Company_Transaction_Details = multi.Read<CompanyTransactionDetails>().ToList();
                    var Flight_Own_Segments_Pnrs = multi.Read<FlightOwnSegmentsPnrs>().ToList();
                    var Company_Flight_Own_Segments_Pnr = multi.Read<CompanyFlightOwnSegmentsPnrs>().ToList();
                    var Payment_Gateway_Loggers = multi.Read<PaymentGatewayLoggers>().ToList();
                    var Booking_Airline_Log_For_PGs = multi.Read<BookingAirlineLogForPGs>().ToList();
                    bookingDetail.CompanyFlightDetailAirlines = CompanyFlightDetailAirlines;
                    bookingDetail.CompanyFlightSegmentDetailAirlines = Company_Flight_Segment_Detail_Airline;
                    bookingDetail.CompanyFareDetailAirlines = Company_Fare_Detail_Airline;
                    bookingDetail.CompanyFareDetailSegmentAirlines = Company_Fare_Detail_Segment_Airline;
                    bookingDetail.CompanyPaxDetailAirlines = Company_Pax_Detail_Airline;
                    bookingDetail.CompanyPaxSegmentDetailAirlines = Company_Pax_Segment_Detail_Airlines;
                    bookingDetail.CompanyFlightDetails = Company_Flight_Details;
                    bookingDetail.CompanyFlightSegmentRuleDetailAirlines = Company_Flight_Segment_Rule_Detail_Airline;
                    bookingDetail.CompanyFlightGSTDetails = Company_Flight_GST_Details;
                    bookingDetail.CompanyTransactionDetails = Company_Transaction_Details;
                    bookingDetail.FlightOwnSegmentsPnrs = Flight_Own_Segments_Pnrs;
                    bookingDetail.CompanyFlightOwnSegmentsPnrs = Company_Flight_Own_Segments_Pnr;
                    bookingDetail.PaymentGatewayLoggers = Payment_Gateway_Loggers;
                    bookingDetail.BookingAirlineLogForPGs = Booking_Airline_Log_For_PGs;
                    //FlightDetail = multi.Read<FlightDetail>().ToList();

                    // var resultSet2 = multi.Read<ResultSet2>().ToList();
                }

                return bookingDetail;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error retrieving booking: {ex.Message}");
                throw new Exception($"Error retrieving booking: {ex.Message}", ex);
            }
        }


        public async Task<BookingDetail> GetAirBooking(Int32 bookingRef)
        {
            try
            {
                var connection = _context.Database.GetDbConnection();
                var bookingDetail = new BookingDetail();
                using (var multi = connection.QueryMultiple("getBookingDetail_Proc", new { ProcNo = 7, BookingRef = bookingRef }, commandType: CommandType.StoredProcedure))
                {

                    var CompanyFlightDetailAirlines = multi.Read<CompanyFlightDetailAirlines>().ToList();

                    var Company_Flight_Segment_Detail_Airline = multi.Read<CompanyFlightSegmentDetailAirlines>().ToList();
                    var Company_Fare_Detail_Airline = multi.Read<CompanyFareDetailAirlines>().ToList();
                    var Company_Fare_Detail_Segment_Airline = multi.Read<CompanyFareDetailSegmentAirlines>().ToList();
                    var Company_Pax_Detail_Airline = multi.Read<CompanyPaxDetailAirlines>().ToList();
                    var Company_Pax_Segment_Detail_Airlines = multi.Read<CompanyPaxSegmentDetailAirlines>().ToList();
                    var Company_Flight_Details = multi.Read<CompanyFlightDetails>().ToList();
                    bookingDetail.CompanyFlightDetailAirlines = CompanyFlightDetailAirlines;
                    bookingDetail.CompanyFlightSegmentDetailAirlines = Company_Flight_Segment_Detail_Airline;
                    bookingDetail.CompanyFareDetailAirlines = Company_Fare_Detail_Airline;
                    bookingDetail.CompanyFareDetailSegmentAirlines = Company_Fare_Detail_Segment_Airline;
                    bookingDetail.CompanyPaxDetailAirlines = Company_Pax_Detail_Airline;
                    bookingDetail.CompanyPaxSegmentDetailAirlines = Company_Pax_Segment_Detail_Airlines;
                    bookingDetail.CompanyFlightDetails = Company_Flight_Details;
                }

                return bookingDetail;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error retrieving booking: {ex.Message}");
                throw new Exception($"Error retrieving booking: {ex.Message}", ex);
            }
        }

        public async Task<List<FlightRefrenceData>> GetBookingRef(DateTime startDate, DateTime endDate)
        {
            try
            {
                var reports = await _context.Database.SqlQuery<FlightRefrenceData>(
                    $"EXECUTE Company_Flight_Detail_Airline_Proc @ProcNo = 12, @Dt1 = {startDate.ToString("dd-MMM-yy 12:00:00 tt")}, @Dt2 = {endDate.AddDays(1).ToString("dd-MMM-yy 12:00:00 tt")}"
                ).ToListAsync();


                return reports;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error retrieving daily booking reports: {ex.Message}", ex);
            }
        }


        public async Task<bool> GetBookingPnrStatus(int bookingRef)
        {
            try
            {
                var connection = _context.Database.GetDbConnection();
                var status = await connection.ExecuteScalarAsync<bool>(
                    "getBookingDetail_Proc",
                    new { ProcNo = 3, BookingRef = bookingRef },
                    commandType: CommandType.StoredProcedure
                );
                return status;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error retrieving PNR status for booking {bookingRef}: {ex.Message}");
                throw new Exception($"Error retrieving PNR status for booking {bookingRef}: {ex.Message}", ex);
            }
        }

        public async Task<bool> GetBookingTicketStatus(int bookingRef)
        {
            try
            {
                var connection = _context.Database.GetDbConnection();
                var status = await connection.ExecuteScalarAsync<bool>(
                    "getBookingDetail_Proc",
                    new { ProcNo = 4, BookingRef = bookingRef },
                    commandType: CommandType.StoredProcedure
                );
                return status;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error retrieving ticket status for booking {bookingRef}: {ex.Message}");
                throw new Exception($"Error retrieving ticket status for booking {bookingRef}: {ex.Message}", ex);
            }
        }

        public async Task<bool> IsWalletCfee(string companyID, string bookingType, string sector)
        {
            var isWalletCfee = false;
            try
            {
                var bookingsCfee = await _context.Database.SqlQuery<bool>($"EXECUTE Company_Bookings_Cfee_Proc @ProcNo = 1, @CompanyID = {companyID},  @BookingType = {bookingType},  @Sector = {sector}").ToListAsync();
                if (bookingsCfee.Count > 0)
                {
                    isWalletCfee = bookingsCfee.FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                //dbCommon.Logger.dbLogg(CompanyID, 0, "IsWallet_Cfee", "IsWallet_Cfee", BookingType, "Company_Bookings_Cfee_Proc-4", ex.Message);
            }
            return isWalletCfee;
        }

        public async Task<bool> IsWalletCfee(string CompanyID, string BookingType)
        {
            bool IsValid = false;
            try
            {
                if (CompanyID.IndexOf("-SA-") != -1 || CompanyID.IndexOf("-C-") != -1)
                {
                    var companyID = CommonFunction.getCompany_by_SubCompany_Customer(CompanyID);
                    var bookingsCfee = await _context.Database.SqlQuery<bool>($"EXECUTE Company_Bookings_Cfee_Proc @ProcNo = 4, @CompanyID = {companyID},  @BookingType = {BookingType}").ToListAsync();
                    IsValid = bookingsCfee.FirstOrDefault();


                }
            }
            catch (Exception ex)
            {
                //dbCommon.Logger.dbLogg(CompanyID, 0, "IsWallet_Cfee", "IsWallet_Cfee", BookingType, "Company_Bookings_Cfee_Proc-4", ex.Message);
            }
            return IsValid;
        }

        public async Task<DataTable> GetCfee(string CompanyID, string BookingType, string Sector)
        {
            var dtCfee = await _context.Database.SqlQuery<DataTable>($"EXECUTE Company_Bookings_Cfee_Proc @ProcNo = 1, @CompanyID = {CompanyID},  @BookingType = {BookingType},  @Sector = {Sector}").ToListAsync();
            return dtCfee.FirstOrDefault();
        }

        public async Task<Int32> GetBookingRefFlightDetailAirline(string SearchID, string SearchCriteria, string CompanyID, string MakerID, bool IsQueue, bool IsOffline, CompanyFlightFareDetailAirline obj, bool IsWallet_Cfee)
        {
            Int32 BookingRef = 0;

            try
            {
                var totalcfee = IsWallet_Cfee ? obj.Totalcfee : 0;
                var result = await _context.Database.SqlQuery<Int32>($"EXECUTE Company_Flight_Detail_Airline_Proc @ProcNo = 1, @SupplierID = {obj.SupplierID}, @CompanyID = {CompanyID}, @Sector = {obj.Sector}, @Trip = {obj.Trip}, @Origin = {obj.Origin}, @Destination = {obj.Destination}, @IsImport = {IsQueue}, @IsOffline = {IsOffline}, @MakerID = {MakerID}, @Adt = {obj.Adt}, @Chd = {obj.Chd}, @Inf = {obj.Inf}, @CarrierCode_D = {obj.CarrierCode_D}, @DepartureDate_D = {obj.DepartureDate_D}, @CarrierCode_A = {obj.CarrierCode_A}, @DepartureDate_A = {obj.DepartureDate_A}, @TotalTax = {obj.TotalTax}, @TotalBasic = {obj.TotalBasic}, @TotalYq = {obj.TotalYq}, @TotalFare = {obj.TotalFare}, @TotalServiceTax = {obj.TotalServiceTax}, @TotalMarkup = {obj.TotalMarkup}, @TotalBasic_deal = {obj.TotalBasic_deal}, @TotalYQ_deal = {obj.TotalYQ_deal}, @TotalCB_deal = {obj.TotalCB_deal}, @TotalPromo_deal = {obj.TotalPromo_deal}, @TotalServiceFee_deal = {obj.TotalServiceFee}, @TotalCommission = {obj.TotalCommission}, @TotalTds = {obj.TotalTds}, @TotalImport = {obj.TotalQueue}, @TotalSeat = 0, @TotalMeal = {obj.TotalMeal}, @TotalBaggage = {obj.TotalBaggage}, @TotalMarkup1 = {obj.TotalMarkup_SA}, @TotalBasic_deal1 = {obj.TotalBasic_deal_SA}, @TotalYQ_deal1 = {obj.TotalYQ_deal_SA}, @TotalCB_deal1 = {obj.TotalCB_deal_SA}, @TotalPromo_deal1 = {obj.TotalPromo_deal_SA}, @TotalCommission1 = {obj.TotalCommission_SA}, @TotalTds1 = {obj.TotalTds_SA}, @TotalFare1 = {obj.TotalFare_SA}, @PriceType_D = {obj.PriceType_D}, @PriceType_A = {obj.PriceType_A}, @AG_Markup_D = {obj.AG_Markup_D}, @AG_Markup_A = {obj.AG_Markup_A}, @Totalcfee = {totalcfee}").ToListAsync();
                BookingRef = result.FirstOrDefault();
            }
            catch (Exception ex)
            {
                //dbCommon.Logger.dbLogg(CompanyID, BookingRef, "GET_BookingRef_Flight_Detail_Airline", "LibaryAirlineBooking", SearchCriteria, SearchID, ex.Message);
            }
            return BookingRef;
        }

        public async Task<bool> SetCustomerFareDetailAirline(string CompanyID, Int32 BookingRef, Decimal CompanyDebit, Decimal CustomerDebit, Decimal CompanyCredit, Decimal CustomerCredit, Int32 Markup, Decimal CompanyTds)
        {
            bool SaveStatus = false;
            try
            {
                var parameters = new[]
                {
                    new SqlParameter("@ProcNo", 1),
                    new SqlParameter("@CompanyID", CompanyID),
                    new SqlParameter("@BookingRef", BookingRef),
                    new SqlParameter("@CompanyDebit", CompanyDebit),
                    new SqlParameter("@CompanyCredit", CompanyCredit),
                    new SqlParameter("@CompanyTds", CompanyTds),
                    new SqlParameter("@CustomerDebit", CustomerDebit),
                    new SqlParameter("@CustomerCredit", CustomerCredit),
                    new SqlParameter("@Markup", Markup),
                };

                var result = _context.Database.ExecuteSqlRawAsync("EXECUTE Customer_Fare_Detail_Airline1_Proc @ProcNo, @CompanyID, @BookingRef, @CompanyDebit, @CompanyCredit, @CompanyTds, @CustomerDebit, @CustomerCredit, @Markup", parameters);

                if (result.Result > 0)
                {
                    SaveStatus = true;
                }
            }
            catch (Exception ex)
            {
                // dbCommon.Logger.dbLogg(CompanyID, BookingRef, "", "set_CustomerFareDetailAirline", "", "", ex.Message);
            }
            return SaveStatus;
        }

        public async Task<bool> SetFlightSegmentRuleDetailAirline(string SearchID, string SearchCriteria, string CompanyID, Int32 BookingRef, DataTable dtBound)
        {
            bool Status = false;

            try
            {
                DataRow[] drSelect = dtBound.Select("FltType='" + "O" + "'");
                if (drSelect.Length > 0)
                {
                    var parameters = new[]
                    {
                        new SqlParameter("@ProcNo", 1),
                        new SqlParameter("@CompanyID", CompanyID),
                        new SqlParameter("@BookingRef", BookingRef),
                        new SqlParameter("@Conn", "O"),
                        new SqlParameter("@EquipmentType", drSelect.CopyToDataTable().Rows[0]["EquipmentType"].ToString().Trim().ToUpper()),
                        new SqlParameter("@PriceType", drSelect.CopyToDataTable().Rows[0]["PriceType"].ToString().Trim().ToUpper()),
                        new SqlParameter("@FareRule", drSelect.CopyToDataTable().Rows[0]["FareRule"].ToString().Trim()),
                        new SqlParameter("@FareRuledb", drSelect.CopyToDataTable().Rows[0]["FareRuledb"].ToString().Trim()),
                        new SqlParameter("@BaggageDetail", drSelect.CopyToDataTable().Rows[0]["BaggageDetail"].ToString().Trim()),
                        new SqlParameter("@SearchID", SearchID),
                        new SqlParameter("@API_SearchID", drSelect.CopyToDataTable().Rows[0]["Api_SessionID"].ToString().Trim().ToUpper()),
                        new SqlParameter("@API_TraceID", drSelect.CopyToDataTable().Rows[0]["JourneySellKey"].ToString().Trim().ToUpper()),
                    };

                    var result = _context.Database.ExecuteSqlRawAsync("Company_Flight_Segment_Rule_Detail_Airline_Proc @ProcNo, @CompanyID, @BookingRef,  @Conn,  @EquipmentType, @PriceType, @FareRule, @FareRuledb, @BaggageDetail, @SearchID, @API_SearchID, @API_TraceID", parameters);

                    if (result.Result > 0)
                    {
                        Status = true;
                    }
                }

                drSelect = dtBound.Select("FltType='" + "I" + "'");
                if (drSelect.Length > 0)
                {
                    var parameters = new[]
                    {
                        new SqlParameter("@ProcNo", 1),
                        new SqlParameter("@CompanyID", CompanyID),
                        new SqlParameter("@BookingRef", BookingRef),
                        new SqlParameter("@Conn", "I"),
                        new SqlParameter("@EquipmentType", drSelect.CopyToDataTable().Rows[0]["EquipmentType"].ToString().Trim().ToUpper()),
                        new SqlParameter("@PriceType", drSelect.CopyToDataTable().Rows[0]["PriceType"].ToString().Trim().ToUpper()),
                        new SqlParameter("@FareRule", drSelect.CopyToDataTable().Rows[0]["FareRule"].ToString().Trim()),
                        new SqlParameter("@FareRuledb", drSelect.CopyToDataTable().Rows[0]["FareRuledb"].ToString().Trim()),
                        new SqlParameter("@BaggageDetail", drSelect.CopyToDataTable().Rows[0]["BaggageDetail"].ToString().Trim()),
                        new SqlParameter("@SearchID", SearchID),
                        new SqlParameter("@API_SearchID", drSelect.CopyToDataTable().Rows[0]["Api_SessionID"].ToString().Trim().ToUpper()),
                        new SqlParameter("@API_TraceID", drSelect.CopyToDataTable().Rows[0]["JourneySellKey"].ToString().Trim().ToUpper()),
                    };

                    var result = _context.Database.ExecuteSqlRawAsync("EXECUTE Company_Flight_Segment_Rule_Detail_Airline_Proc @ProcNo, @CompanyID, @BookingRef, @Conn, @EquipmentType, @PriceType, @FareRule, @FareRuledb, @BaggageDetail, @SearchID, @API_SearchID, @API_TraceID", parameters);

                    if (result.Result > 0)
                    {
                        Status = true;
                    }

                }
            }
            catch (Exception ex)
            {
                // dbCommon.Logger.dbLogg(CompanyID, BookingRef, "SET_Flight_Segment_Rule_Detail_Airline", "LibraryAirlineBooking", SearchCriteria, SearchID, ex.Message);
            }
            return Status;
        }

        public async Task<bool> SetFlightSegmentDetailAirline(string SearchID, string SearchCriteria, string CompanyID, Int32 BookingRef, DataTable dtBound, bool IsCombi, bool IsRTfare)
        {
            bool Status = false;
            try
            {
                for (int iRow = 0; iRow < dtBound.Rows.Count; iRow++)
                {
                    DataRow dr = dtBound.Rows[iRow];
                    string FareType = IsCombi ? "COM" : (IsRTfare ? "RT" : "NF");

                    // Raw SQL command
                    string sql = @"
                EXEC Company_Flight_Segment_Detail_Airline_Proc
                    @ProcNo = {0},
                    @CompanyID = {1},
                    @BookingRef = {2},
                    @CarrierCode = {3},
                    @FlightNumber = {4},
                    @Origin = {5},
                    @Destination = {6},
                    @DepartureStation = {7},
                    @ArrivalStation = {8},
                    @DepartureDate = {9},
                    @ArrivalDate = {10},
                    @DepartureTime = {11},
                    @ArrivalTime = {12},
                    @JourneyTime = {13},
                    @Duration = {14},
                    @Stops = {15},
                    @Via = {16},
                    @ViaName = {17},
                    @DepartureTerminal = {18},
                    @ArrivalTerminal = {19},
                    @ClassOfService = {20},
                    @ProductClass = {21},
                    @FareBasisCode = {22},
                    @Cabin = {23},
                    @RefundType = {24},
                    @ConnOrder = {25},
                    @BaggageDetail = {26},
                    @FareRule = {27},
                    @RuleTarrif = {28},
                    @FareType = {29}";

                    // Execute the raw SQL
                    var affectedRows =  _context.Database.ExecuteSqlRawAsync(sql,
                        1, // ProcNo
                        CompanyID,
                        BookingRef,
                        dr["CarrierCode"].ToString().Trim().ToUpper(),
                        dr["FlightNumber"].ToString().Trim().ToUpper(),
                        dr["Origin"].ToString().Trim().ToUpper(),
                        dr["Destination"].ToString().Trim().ToUpper(),
                        dr["DepartureStation"].ToString().Trim().ToUpper(),
                        dr["ArrivalStation"].ToString().Trim().ToUpper(),
                        dr["DepartureDate"].ToString().Trim().ToUpper(),
                        dr["ArrivalDate"].ToString().Trim().ToUpper(),
                        dr["DepartureTime"].ToString().Trim().ToUpper(),
                        dr["ArrivalTime"].ToString().Trim().ToUpper(),
                        Convert.ToInt32(dr["JourneyTime"].ToString().Trim()),
                        Convert.ToInt32(dr["Duration"].ToString().Trim()),
                        Convert.ToInt32(dr["Stops"].ToString().Trim()),
                        Convert.ToInt32(dr["Via"].ToString().Trim()),
                        dr["ViaName"].ToString().Trim().ToUpper(),
                        dr["DepartureTerminal"].ToString().Trim().ToUpper(),
                        dr["ArrivalTerminal"].ToString().Trim().ToUpper(),
                        dr["ClassOfService"].ToString().Trim().ToUpper(),
                        dr["ProductClass"].ToString().Trim().ToUpper(),
                        dr["FareBasisCode"].ToString().Trim().ToUpper(),
                        dr["Cabin"].ToString().Trim().ToUpper(),
                        dr["RefundType"].ToString().Trim().ToUpper(),
                        dr["FltType"].ToString().Trim().ToUpper() + (iRow + 1),
                        dr["BaggageDetail"].ToString().Trim(),
                        dr["FareRule"].ToString().Trim(),
                        dr["RuleTarrif"].ToString().Trim(),
                        FareType
                    );

                    if (affectedRows.Result > 0)
                    {
                        Status = true;
                    }
                }
            }
            catch (Exception ex)
            {
                // Log the exception
                //dbCommon.Logger.dbLogg(CompanyID, BookingRef, "SET_Flight_Segment_Detail_Airline", "LibraryAirlineBooking", SearchCriteria, SearchID, ex.Message);
            }

            return Status;
        }

        public async Task<bool> SetFareDetailAirline(string SearchID, string SearchCriteria, string CompanyID, Int32 BookingRef, string Conn, Decimal TotalMeal, Decimal TotalBaggage, Decimal TotalSeat, CompanyFlightFareDetailAirline obj, bool IsSingleFare)
        {
            bool Status = false;

            try
            {
                string sql = @"
            EXECUTE Company_Fare_Detail_Airline_Proc 
                @ProcNo = {0}, 
                @CompanyID = {1}, 
                @BookingRef = {2}, 
                @Conn = {3}, 
                @Adt = {4}, 
                @Chd = {5}, 
                @Inf = {6}, 
                @TotalTax = {7}, 
                @TotalBasic = {8}, 
                @TotalYq = {9}, 
                @TotalFare = {10}, 
                @TotalServiceTax = {11}, 
                @TotalMarkup = {12}, 
                @TotalBasic_deal = {13}, 
                @TotalYQ_deal = {14}, 
                @TotalCB_deal = {15}, 
                @TotalPromo_deal = {16}, 
                @TotalServiceFee_deal = {17}, 
                @TotalCommission = {18}, 
                @TotalTds = {19}, 
                @TotalSeat = {20}, 
                @TotalMeal = {21}, 
                @TotalBaggage = {22}, 
                @TotalQueue = {23}";

                // Prepare parameters dynamically
                var parameters = new object[]
                {
                    1, // ProcNo
                    CompanyID,
                    BookingRef,
                    Conn,
                    obj.Adt,
                    obj.Chd,
                    obj.Inf,
                    IsSingleFare ? 0 : obj.TotalTax,
                    IsSingleFare ? 0 : obj.TotalBasic,
                    IsSingleFare ? 0 : obj.TotalYq,
                    IsSingleFare ? 0 : obj.TotalFare,
                    IsSingleFare ? 0 : obj.TotalServiceTax,
                    IsSingleFare ? 0 : obj.TotalMarkup,
                    IsSingleFare ? 0 : obj.TotalBasic_deal,
                    IsSingleFare ? 0 : obj.TotalYQ_deal,
                    IsSingleFare ? 0 : obj.TotalCB_deal,
                    IsSingleFare ? 0 : obj.TotalPromo_deal,
                    IsSingleFare ? 0 : obj.TotalServiceFee,
                    IsSingleFare ? 0 : obj.TotalCommission,
                    IsSingleFare ? 0 : obj.TotalTds,
                    TotalSeat,
                    TotalMeal,
                    TotalBaggage,
                    IsSingleFare ? 0 : obj.TotalQueue
                };

                // Execute the query
                var rowsAffected =  _context.Database.ExecuteSqlRawAsync(sql, parameters);

                // Set status based on result
                if (rowsAffected.Result > 0)
                {
                    Status = true;
                }
            }

            catch (Exception ex)
            {
                // dbCommon.Logger.dbLogg(CompanyID, BookingRef, "SET_Fare_Detail_Airline", "LibraryAirlineBooking", SearchCriteria, SearchID, Conn + Environment.NewLine + ex.Message);
            }
            return Status;
        }

        public async Task<bool> SetFareDetailSegmentAirline(string SearchID, string SearchCriteria, string CompanyID, Int32 BookingRef, string Conn, string PaxType, Decimal dMeal, Decimal dSeat, Decimal dBaggage, DataRow drSelect, bool IsSingleFare)
        {
            var Status = false;

            try
            {
                // Base parameters
                var parameters = new List<object>
            {
                1, // ProcNo
                CompanyID,
                BookingRef,
                PaxType,
                Conn,
                Convert.ToInt32(drSelect[PaxType]?.ToString()?.Trim()?.ToUpper() ?? "0")
            };

                // Additional parameters for non-single fare cases
                if (!IsSingleFare)
                {
                    if (PaxType == "ADT" || PaxType == "CHD")
                    {
                        parameters.AddRange(new object[]
                        {
                    dMeal,
                    dSeat,
                    dBaggage,
                    Convert.ToDecimal(drSelect[$"{PaxType}_BASIC"]?.ToString()?.Trim()?.ToUpper() ?? "0"),
                    Convert.ToDecimal(drSelect[$"{PaxType}_YQ"]?.ToString()?.Trim()?.ToUpper() ?? "0"),
                    Convert.ToDecimal(drSelect[$"{PaxType}_PSF"]?.ToString()?.Trim()?.ToUpper() ?? "0"),
                    Convert.ToDecimal(drSelect[$"{PaxType}_UDF"]?.ToString()?.Trim()?.ToUpper() ?? "0"),
                    Convert.ToDecimal(drSelect[$"{PaxType}_AUDF"]?.ToString()?.Trim()?.ToUpper() ?? "0"),
                    Convert.ToDecimal(drSelect[$"{PaxType}_CUTE"]?.ToString()?.Trim()?.ToUpper() ?? "0"),
                    Convert.ToDecimal(drSelect[$"{PaxType}_GST"]?.ToString()?.Trim()?.ToUpper() ?? "0"),
                    Convert.ToDecimal(drSelect[$"{PaxType}_TF"]?.ToString()?.Trim()?.ToUpper() ?? "0"),
                    Convert.ToDecimal(drSelect[$"{PaxType}_CESS"]?.ToString()?.Trim()?.ToUpper() ?? "0"),
                    Convert.ToDecimal(drSelect[$"{PaxType}_EX"]?.ToString()?.Trim()?.ToUpper() ?? "0"),
                    Convert.ToDecimal(drSelect[$"{PaxType}_BAS"]?.ToString()?.Trim()?.ToUpper() ?? "0"),
                    Convert.ToDecimal(drSelect[$"{PaxType}_Y"]?.ToString()?.Trim()?.ToUpper() ?? "0"),
                    Convert.ToDecimal(drSelect[$"{PaxType}_CB"]?.ToString()?.Trim()?.ToUpper() ?? "0"),
                    Convert.ToDecimal(drSelect[$"{PaxType}_PR"]?.ToString()?.Trim()?.ToUpper() ?? "0"),
                    Convert.ToDecimal(drSelect[$"{PaxType}_TDS"]?.ToString()?.Trim()?.ToUpper() ?? "0"),
                    Convert.ToDecimal(drSelect[$"{PaxType}_MU"]?.ToString()?.Trim()?.ToUpper() ?? "0"),
                    Convert.ToDecimal(drSelect[$"{PaxType}_Import"]?.ToString()?.Trim()?.ToUpper() ?? "0"),
                    Convert.ToDecimal(drSelect[$"{PaxType}_SF"]?.ToString()?.Trim()?.ToUpper() ?? "0"),
                    Convert.ToDecimal(drSelect[$"{PaxType}_ST"]?.ToString()?.Trim()?.ToUpper() ?? "0")
                        });
                    }
                    else if (PaxType == "INF")
                    {
                        parameters.AddRange(new object[]
                        {
                    Convert.ToDecimal(drSelect[$"{PaxType}_BASIC"]?.ToString()?.Trim()?.ToUpper() ?? "0"),
                    Convert.ToDecimal(drSelect["Inf_TAX"]?.ToString()?.Trim()?.ToUpper() ?? "0")
                        });
                    }
                }

                // Define SQL query with placeholders
                string sql = @"
            EXECUTE Company_Fare_Detail_Segment_Airline_Proc 
                @ProcNo = {0}, 
                @CompanyID = {1}, 
                @BookingRef = {2}, 
                @PaxType = {3}, 
                @Conn = {4}, 
                @No_Of_Passenger = {5}";

                // Add additional placeholders for dynamic parameters
                if (!IsSingleFare)
                {
                    if (PaxType == "ADT" || PaxType == "CHD")
                    {
                        sql += @", @Meal = {6}, @Seat = {7}, @Baggage = {8}, @Basic = {9}, @Yq = {10}, @Psf = {11}, 
                         @Udf = {12}, @AUDF = {13}, @Cute = {14}, @Gst = {15}, @TF = {16}, @Cess = {17}, 
                         @Ex = {18}, @Basic_Deal = {19}, @Yq_Deal = {20}, @Cb_Deal = {21}, @Promo_Deal = {22}, 
                         @Tds = {23}, @Markup = {24}, @Import = {25}, @Service_Fee = {26}, @ServiceTax = {27}";
                    }
                    else if (PaxType == "INF")
                    {
                        sql += ", @Basic = {6}, @Ex = {7}";
                    }
                }

                // Execute the SQL query
                var result = _context.Database.ExecuteSqlRawAsync(sql, parameters.ToArray());

                // Update the status based on result
                if (result.Result > 0)
                {
                    Status = true;
                }
            }

            catch (Exception ex)
            {
                //dbCommon.Logger.dbLogg(CompanyID, BookingRef, "SET_Fare_Detail_Segment_Airline", "LibraryAirlineBooking", SearchCriteria, SearchID, Conn + Environment.NewLine + PaxType + Environment.NewLine + ex.Message);
            }
            return Status;
        }

        public async Task<bool> SetPaxDetailAirline(string SearchID, string SearchCriteria, string CompanyID, Int32 BookingRef, DataTable dtPassenger)
        {
            bool Status = false;

            try
            {
                string passenger = "";
                var parameters = new object[dtPassenger.Rows.Count];
                for (int iRow = 0; iRow < dtPassenger.Rows.Count; iRow++)
                {
                    int Pax_SegmentID = 0;
                    DataRow dr = dtPassenger.Rows[iRow];

                    passenger += dr["Title"].ToString().Trim().ToUpper() + ",";
                    passenger += dr["First_Name"].ToString().Trim().ToUpper() + ",";
                    passenger += dr["Middle_Name"].ToString().Trim().ToUpper() + ",";
                    passenger += dr["Last_Name"].ToString().Trim().ToUpper() + ",";
                    passenger += dr["MobileNo"].ToString().Trim().ToUpper() + ",";
                    passenger += dr["Email"].ToString().Trim().ToUpper() + ",";
                    //passenger += dr["Address"].ToString().Trim().ToUpper() + ",";
                    passenger += dr["FFN"].ToString().Trim().ToUpper() + ",";
                    passenger += dr["TourCode"].ToString().Trim().ToUpper() + ",";
                    passenger += dr["DOB"].ToString().Trim().ToUpper() + ",";
                    passenger += dr["PaxType"].ToString().Trim().ToUpper() + ",";
                    passenger += dr["PpNumber"].ToString().Trim().ToUpper() + ",";
                    passenger += dr["PPIssueDate"].ToString().Trim().ToUpper() + ",";
                    passenger += dr["PPExpirayDate"].ToString().Trim().ToUpper() + ",";
                    //passenger += dr["Nationality"].ToString().Trim().ToUpper() + ",";

                    var result = _context.Database.SqlQuery<int>($@"
                        EXECUTE Company_Pax_Detail_Airline_Proc 
                            @ProcNo=1, 
                            @CompanyID={CompanyID}, 
                            @BookingRef={BookingRef}, 
                            @Title={dr["Title"].ToString().Trim().ToUpper()}, 
                            @First_Name={dr["First_Name"].ToString().Trim().ToUpper()}, 
                            @Middle_Name={dr["Middle_Name"].ToString().Trim().ToUpper()}, 
                            @Last_Name={dr["Last_Name"].ToString().Trim().ToUpper()}, 
                            @MobileNo={dr["MobileNo"].ToString().Trim().ToUpper()}, 
                            @Email={dr["Email"].ToString().Trim().ToUpper()}, 
                            @FFN={dr["FFN"].ToString().Trim().ToUpper()}, 
                            @TourCode={dr["TourCode"].ToString().Trim().ToUpper()}, 
                            @DOB={dr["DOB"].ToString().Trim().ToUpper()}, 
                            @PaxType={dr["PaxType"].ToString().Trim().ToUpper()}, 
                            @PpNumber={dr["PpNumber"].ToString().Trim().ToUpper()}, 
                            @PPIssueDate={dr["PPIssueDate"].ToString().Trim().ToUpper()}, 
                            @PPExpirayDate={dr["PPExpirayDate"].ToString().Trim().ToUpper()}
                    ").ToListAsync();

                    if (result.Result.FirstOrDefault() > 0)
                    {
                        Pax_SegmentID = result.Result.FirstOrDefault();
                        Status = true;

                        try
                        {
                            if (dr["PaxType"].ToString().Trim().ToUpper().Equals("ADT") || dr["PaxType"].ToString().Trim().ToUpper().Equals("CHD"))
                            {
                                if (dr["MealCode_O"].ToString().Trim().Length > 2)
                                {
                                    SetPaxSSRDetailAirlineCharge(CompanyID, BookingRef, Pax_SegmentID, "M", dr["MealCode_O"].ToString().Trim(), dr["MealDesc_O"].ToString().Trim(), dr["MealChg_O"].ToString().Trim(), "O");
                                }
                                if (dr["MealCode_I"].ToString().Trim().Length > 2)
                                {
                                    SetPaxSSRDetailAirlineCharge(CompanyID, BookingRef, Pax_SegmentID, "M", dr["MealCode_I"].ToString().Trim(), dr["MealDesc_I"].ToString().Trim(), dr["MealChg_I"].ToString().Trim(), "I");
                                }
                                if (dr["BaggageCode_O"].ToString().Trim().Length > 2)
                                {
                                    SetPaxSSRDetailAirlineCharge(CompanyID, BookingRef, Pax_SegmentID, "B", dr["BaggageCode_O"].ToString().Trim(), dr["BaggageDesc_O"].ToString().Trim(), dr["BaggageChg_O"].ToString().Trim(), "O");
                                }
                                if (dr["BaggageCode_I"].ToString().Trim().Length > 2)
                                {
                                    SetPaxSSRDetailAirlineCharge(CompanyID, BookingRef, Pax_SegmentID, "B", dr["BaggageCode_I"].ToString().Trim(), dr["BaggageDesc_I"].ToString().Trim(), dr["BaggageChg_I"].ToString().Trim(), "I");
                                }

                            }
                        }
                        catch (Exception ex)
                        {
                            //dbCommon.Logger.dbLogg(CompanyID, BookingRef, "SET_Pax_SSR_Detail_Airline_Charge", "LibraryAirlineBooking", "Pax_SegmentID-" + Pax_SegmentID.ToString(), passenger, ex.Message);
                            _logger.LogError(ex, $"Company_Pax_Detail_Airline_Proc: {ex.Message}", new { BookingRef, V = Pax_SegmentID.ToString(), passenger });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //dbCommon.Logger.dbLogg(CompanyID, BookingRef, "SET_Pax_Detail_Airline", "LibraryAirlineBooking", SearchCriteria, SearchID, ex.Message);
            }
            return Status;
        }
        private async Task<bool> SetPaxSSRDetailAirlineCharge(string CompanyID, Int32 BookingRef, Int32 Pax_SegmentID, string ChargeType, string ChargeCode, string ChargeDescription, string Charge_Amount, string Conn)
        {
            bool Status = false;

            try
            {
                string sql = @"EXECUTE Company_Pax_Detail_Airline_Charge_Proc @ProcNo = {0}, @CompanyID = {1}, @BookingRef = {2}, @Pax_SegmentID = {3}, @Conn = {4}, @ChargeType = {5}, @ChargeCode = {6}, @ChargeDescription = {7}, @Charge_Amount = {8}";
                var iRow =  _context.Database.ExecuteSqlRawAsync(
                    sql,
                    1, // ProcNo
                    CompanyID,
                    BookingRef,
                    Pax_SegmentID,
                    Conn,
                    ChargeType.Trim().ToUpper(),
                    ChargeCode.Trim().ToUpper(),
                    ChargeDescription.Trim(),
                    Convert.ToDecimal(Charge_Amount.Trim().ToUpper()));

                if (iRow.Result > 0)
                {
                    Status = true;
                }
            }
            catch (Exception ex)
            {
                 _logger.LogError(ex, $"SET_Pax_SSR_Detail_Airline_Charge : {ex.Message}", new { CompanyID, BookingRef, Pax_SegmentID, Charge_Amount, ChargeCode, ChargeDescription, ChargeType });
            }
            return Status;
        }

        public async Task<bool> BookingStatus(Int32 BookingRef, bool Status)
        {
            bool bStatus = false;

            try
            {
                string sql = @"EXECUTE BookingStatus_Proc @ProcNo = {0}, @BookingRef = {1}, @Status = {2}";
                var iRows =  _context.Database.ExecuteSqlRawAsync(sql, 1, BookingRef, Status);
                if (iRows.Result > 0)
                {
                    bStatus = true;
                }
            }
            catch (Exception ex)
            {
                // _logger.LogError(ex, "BookingStatus", new { BookingRef, Status });
            }

            return bStatus;
        }

        public async Task<bool> SetBookingAirlineLogForPG(bool IsCombi, bool IsRT, bool IsMC, string SearchID, string CompanyID, Int32 BookingRef, string AvailabilityResponse, string PassengerResponse, string RefID_O, string RefID_I, int PaymentID, string PaymentType, string Currency, double CurrencyValue)
        {
            bool Status = false;
            try
            {
                string sql = @"EXECUTE BookingAirlineLogForPG_Proc @ProcNo = {0}, @CompanyID = {1}, @BookingRef = {2}, @AvailabilityResponse = {3}, @PassengerResponse = {4}, @RefID_O = {5}, @RefID_I = {6}, @SearchID = {7}, @IsCombi = {8}, @IsRT = {9}, @IsMC = {10}, @PaymentID = {11}, @PaymentType = {12}, @Currency = {13}, @CurrencyValue = {14}";
                var i =  _context.Database.ExecuteSqlRawAsync(
                    sql,
                    1, // ProcNo
                    CompanyID,
                    BookingRef,
                    AvailabilityResponse,
                    PassengerResponse,
                    RefID_O,
                    RefID_I,
                    SearchID,
                    IsCombi,
                    IsRT,
                    IsMC,
                    PaymentID,
                    PaymentType,
                    Currency,
                    CurrencyValue);

                if (i.Result > 0)
                {
                    Status = true;
                }
            }
            catch (Exception ex)
            {
                //dbCommon.Logger.dbLogg(CompanyID, BookingRef, PassengerResponse, "set_BookingAirlineLogForPG", SearchID, AvailabilityResponse, ex.Message);
            }
            return Status;
        }

        public async Task<bool> UpdatePNR(Int32 BookingRef, string StaffID, string FltType, string Airline_PNR, string GDS_PNR, string SupplierID, string? UniversalRecordLocatorCode = "")
        {
            bool Status = false;

            try
            {

                string sql = @"EXECUTE Company_Flight_Detail_Airline_Proc @ProcNo = {0}, @BookingRef = {1}, @FltType = {2}, @Airline_PNR = {3}, @GDS_PNR = {4}, @StaffID = {5}, @SupplierID = {6}, @UniversalRecordLocatorCode = {7}";
                var rowsAffected = _context.Database.ExecuteSqlRawAsync(
                 sql,
                 3, // ProcNo
                 BookingRef,
                 FltType,
                 Airline_PNR,
                 GDS_PNR,
                 StaffID,
                 SupplierID,
                 UniversalRecordLocatorCode);

                if (rowsAffected.Result > 0)
                {
                    Status = true;
                }
            }
            catch (Exception ex)
            {
                // dbCommon.Logger.dbLogg(StaffID, BookingRef, "UpdatePNR", "dbCallCenter", "Company_Flight_Detail_Airline_Proc", Airline_PNR + "-" + GDS_PNR, ex.Message);
            }

            return Status;

        }

        public async Task<bool> UpdateTicketNumber(Int32 bookingRef, ArrayList ticketNumbers, bool isModify)
        {

            var passengerIDs = await GetPassengerID(bookingRef, _context);
            if (ticketNumbers == null || passengerIDs == null || ticketNumbers.Count == 0 || passengerIDs.Count == 0)
                throw new ArgumentException("TicketNumbers and PassengerIDs cannot be null or empty.");

            if (ticketNumbers.Count != passengerIDs.Count)
                throw new ArgumentException("The number of TicketNumbers must match the number of PassengerIDs.");

            string storedProcedure = isModify
                ? "Company_Pax_Detail_Airline_Update_TicketNumber_Modify"
                : "Company_Pax_Detail_Airline_Update_TicketNumber";

            // Build SQL with dynamic placeholders
            string sql = $"EXECUTE {storedProcedure} @ProcNo = {{0}}, ";
            var parameters = new List<object>
               {
                ticketNumbers.Count, // ProcNo
                };

            for (int i = 0; i < ticketNumbers.Count; i++)
            {
                sql += $"@Pax{i + 1} = {{{parameters.Count}}}, @Pax{i + 1}id = {{{parameters.Count + 1}}}, ";
                parameters.Add(ticketNumbers[i]);
                parameters.Add(passengerIDs[i]);
            }

            // Remove trailing comma and space
            sql = sql.TrimEnd(',', ' ');

            // Execute the stored procedure
            try
            {
                var rowsAffected =  _context.Database.ExecuteSqlRawAsync(sql, parameters.ToArray());
                return rowsAffected.Result > 0;

            }
            catch (Exception ex)
            {
                // dbCommon.Logger.dbLogg("", BookingRef, "UpdateTicketNumber", "dbCallCenter", "Company_Pax_Detail_Airline_Update_TicketNumber", strTktNumber, ex.Message);
            }
            return false;

        }

        private static async Task<List<int>> GetPassengerID(Int32 BookingRef, ZealdbNContext context)
        {
            List<int> RowId = new List<int>();

            try
            {
                var data = context.Database.SqlQuery<int>($"EXEC Company_Pax_Detail_Airline_Update_TicketNumber @ProcNo=0, @BookingRef={BookingRef}").ToListAsync();
                RowId = data.Result;
            }
            catch (Exception ex)
            {
                // dbCommon.Logger.dbLogg("", BookingRef, "getPassengerID", "dbCallCenter", "Company_Pax_Detail_Airline_Update_TicketNumber", "", ex.Message);
            }
            finally
            {

            }

            return RowId;
        }

        public async Task<bool> IsTicketAutoMode(string Companyid, string CarrierCode, string Sector, int BookingRef)
        {
            try
            {
                var data =  _context.Database.SqlQuery<bool>($"EXEC Group_Commission_Rule_New_Proc @ProcNo=11, @CompanyID={Companyid}, @CarrierCode={CarrierCode}, @Sector={Sector}").ToListAsync();
                return data.Result.FirstOrDefault();
            }
            catch (Exception ex)
            {
                DBCommon.Logger.dbLogg(Companyid, BookingRef, "IsTicketAutoMode", "apiAir_uapi", "Group_Commission_Rule_New_Proc-11", "", ex.Message);
            }
            return false;
        }

        public async Task<string> GetActiveFormOfPayment()
        {
            string FOP = "CASH";
            try
            {
                var data =  _context.Database.SqlQuery<string>($"EXEC UAPI_FORM_OF_PAYMENT @row=2").ToListAsync();
                FOP = data.Result.FirstOrDefault();
            }
            catch (Exception ex)
            {
                DBCommon.Logger.dbLogg("", 0, "GetActiveFormOfPayment", "apiAir_uapi", "", "", ex.Message);
            }

            return FOP;
        }

        public async Task<DataRow> GetCCDetails(string CarrierCode)
        {
            DataTable dtCreditcard = new DataTable();
            try
            {
                var data = _context.UapiCcDetails.FromSql($"EXEC UAPI_FORM_OF_PAYMENT @row=1").ToListAsync();
                dtCreditcard = data.Result.ToDataTable();
            }
            catch (Exception ex)    
            {
                DBCommon.Logger.dbLogg("", 0, "GetCCDetails", "apiAir_uapi", "", "", ex.Message);
            }

            if (dtCreditcard != null && dtCreditcard.Rows.Count > 0)
            {
                foreach (DataRow dr in dtCreditcard.Rows)
                {
                    if (dr["Carriers"].ToString().IndexOf(CarrierCode) != -1)
                    {
                        return dr;
                    }
                }
                return dtCreditcard.Rows[0];
            }
            else if (dtCreditcard != null && dtCreditcard.Rows.Count.Equals(1))
            {
                return dtCreditcard.Rows[0];
            }

            return null;
        }
    }

}
