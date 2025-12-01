using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZealTravel.Domain.Data.Entities;
using ZealTravel.Application.Enum;
using ZealTravel.Application.UserManagement.Queries;
using ZealTravel.Application.UserManagement.Commands;
using ZealTravel.Application.AgencyManagement.Queries;
using ZealTravel.Domain.Data.Models;
using ZealTravel.Application.AgencyManagement.Queries.AgencyDashboard;
using ZealTravel.Application.ReportManagement.Queries;
using ZealTravel.Application.AirlineSupplierManagement.Queries;
using ZealTravel.Application.BackofficeManagement.Queries.Dashboard;
using ZealTravel.Application.BackofficeManagement.Handlers.Dashboard;
using ZealTravel.Domain.Data.Models.Backoffice;
using ZealTravel.Application.BookingManagement.Query;
using ZealTravel.Domain.Data.Models.Booking;
using ZealTravel.Domain.Data.Models.Agency;
using ZealTravel.Domain.Data.Models.Country;
using ZealTravel.Domain.Models;
using DocumentFormat.OpenXml.ExtendedProperties;
using ZealTravel.Application.BankManagement.Query;
using DocumentFormat.OpenXml.InkML;
using ZealTravel.Application.CredentialManagement.Query;
using ZealTravel.Application.GSTManagement.Queries;

namespace ZealTravel.Application.Mappers
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<CompanyRegister, User>()
              .ReverseMap();
            CreateMap<RegisterUserCommand, CompanyRegister>()
             .ForMember(dest => dest.Gst, opt => opt.MapFrom(src => src.GST))
                .ForMember(dest => dest.PanName, opt => opt.MapFrom(src => src.Pan_Name))
                .ReverseMap();
            CreateMap<CompanyRegister, CompanyProfileData>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => $"{src.FirstName} {src.LastName}"))
                .ForMember(dest => dest.CompanyAddress, opt => opt.MapFrom(src => src.Address))
                .ForMember(dest => dest.CompanyPhone, opt => opt.MapFrom(src => src.CompanyPhoneNo))
                .ForMember(dest => dest.PanCardNo, opt => opt.MapFrom(src => src.PanNo))
                .ForMember(dest => dest.GstNo, opt => opt.MapFrom(src => src.Gst))
                .ForMember(dest => dest.GstName, opt => opt.MapFrom(src => src.GstName))
                .ForMember(dest => dest.AvailableBalance, opt => opt.Ignore());
            CreateMap<GstState, GstStates>();
            CreateMap<CityState, GstCityByState>();
            CreateMap<DailyBookingReport, DailyBooking>()
            .ForMember(dest => dest.NoOfPassengers, opt => opt.MapFrom(src => src.NoOfPassenger));
            CreateMap<BookingDetail, AirSheetReportData>();
            CreateMap<FlightRefrenceData, FlightBookingRefrence>()
            .ForMember(dest => dest.BookingRef, opt => opt.MapFrom(src => src.BookingRef))
            .ForMember(dest => dest.CompanyId, opt => opt.MapFrom(src => src.CompanyId));
            CreateMap<LedgerReport, LedgerReport>();
            CreateMap<DailyBookingReport, DailyBookingReport>()
         .ForMember(dest => dest.BookingRef, opt => opt.MapFrom(src => src.BookingRef));
            CreateMap<LedgerReport, LedgerReports>();
            CreateMap<AirlineBookings, AirBookingReport>();
            CreateMap<DashboardLedger, DashboardLedgerData>();
            CreateMap<DashboardNotification, DashboardNotificationData>();
            CreateMap<DashBoardChart, DashboardChartData>();
            CreateMap<DashboardCorporate, DashboardCorporateData>();
            CreateMap<CancelFlightBookingReport, CancelAirBooking>();
            CreateMap<AirlineSuppliers, AirSupplier>();
            CreateMap<DashboardAirlinePendingBooking, DashboardAirlinePendingBookingData>();
            CreateMap<DashboardAirlineFlightBooking, DashboardAirlineTopTenBookingData>();
            CreateMap<CompanyRegister, CompanyPopupDetails>()
                .ForMember(dest => dest.Pan_Name, opt => opt.MapFrom(src => src.PanName))
                .ForMember(dest => dest.Pan_No, opt => opt.MapFrom(src => src.PanNo));
          
            CreateMap<CompanyDetails, CompanyAfterLoginDetails>();
            CreateMap<BookingDetail, BookingDetailData>();
            CreateMap<CompanyFlightDetailAirlines, CompanyFlightDetailAirlinesData>();
            CreateMap<CompanyFlightSegmentDetailAirlines, CompanyFlightSegmentDetailAirlinesData>();
            CreateMap<CompanyFareDetailAirlines, CompanyFareDetailAirlinesData>();
            CreateMap<CompanyFareDetailSegmentAirlines, CompanyFareDetailSegmentAirlinesData>();
            CreateMap<CompanyPaxDetailAirlines, CompanyPaxDetailAirlinesData>();
            CreateMap<CompanyPaxSegmentDetailAirlines, CompanyPaxSegmentDetailAirlinesData>();
            CreateMap<CompanyFlightDetails, CompanyFlightDetailsData>();
            CreateMap<CompanyFlightSegmentRuleDetailAirlines, CompanyFlightSegmentRuleDetailAirlinesData>();
            CreateMap<CompanyFlightGSTDetails, CompanyFlightGSTDetailsData>();
            CreateMap<CompanyTransactionDetails, CompanyTransactionDetailsData>();
            CreateMap<FlightOwnSegmentsPnrs, FlightOwnSegmentsPnrsData>();
            CreateMap<CompanyFlightOwnSegmentsPnrs, CompanyFlightOwnSegmentsPnrsData>();
            CreateMap<PaymentGatewayLoggers, PaymentGatewayLoggersData>();
            CreateMap<BookingAirlineLogForPGs, BookingAirlineLogForPGsData>();
            CreateMap<CompanyFlightDetails, CompanyFlightDetailsData>();
            CreateMap<CompanyProfileCommand, CompanyRegister>()
                .ForMember(dest => dest.Email, opt => opt.Ignore());
            CreateMap<ManageBankDetail, AdminBankDetails>();
            CreateMap<SupplierDetailGalileoAirline, GalelioSupplierAirline>();
            CreateMap<SupplierDetailLccAirline, LccSupplierAirline>();
            CreateMap<CompanyRegisterCorporateUser, CompanyRegisterCorporateUserDetails>();
            CreateMap<CompanyRegisterCorporateUsersLimit, CompanyRegisterCorporateUserLimitDetails>();
            CreateMap<CountryList, CountryManagement.Queries.CountryList>();
            CreateMap<PaymentGatewayDisplayOption, PaymentGatewayManagement.Queries.PaymentGatewayDisplayOption>();
            //CreateMap<AgencyDataModel, AgencyDataQuery>();
            CreateMap<AgencyDataModel, SearchAgencyListData>()
           .ForMember(dest => dest.CompanyName, opt => opt.MapFrom(src => src.CompanyName))
           .ForMember(dest => dest.AccountID, opt => opt.MapFrom(src => src.AccountID))
           .ForMember(dest => dest.City, opt => opt.MapFrom(src => src.City))
           .ForMember(dest => dest.UserType, opt => opt.MapFrom(src => src.UserType))
           //.ForMember(dest => dest.ActiveStatus, opt => opt.MapFrom(src => src.ActiveStatus))
           .ForMember(dest => dest.Active_Status, opt => opt.MapFrom(src => src.Active_Status))
           .ForMember(dest => dest.EventTime, opt => opt.MapFrom(src => src.EventTime))
           .ForMember(dest => dest.AvailableBalance, opt => opt.MapFrom(src => src.AvailableBalance));
            CreateMap<CompanyBalanceTransactionDetailEvent, GetAgencyBalanceTransactionEvents>();
            CreateMap<SupplierDetailGalileoAirline, GalelioSupplierAirline>();
            CreateMap<SupplierDetailApiAirline, AirlineApiDetails>();
            CreateMap<UapiCcDetail, UapiCcDetails>();
            CreateMap<UapiFormOfPayment, UapiFopDetail>();
            CreateMap<SupplierProductDetail, SupplierCredProductDetails>();
            CreateMap<AirlinePnrMakeDay, PnrMakeDaysDetails>();
            CreateMap<SupplierProductStatus, ProductStatusDetails>();
            CreateMap<CompanyRegisterGst, GSTDetails>();

        }
    }


}