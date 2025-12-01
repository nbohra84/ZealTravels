using AutoMapper;
using ZealTravel.Application.AgencyManagement.Queries;
using ZealTravel.Application.AgencyManagement.Queries.AgencyDashboard;
using ZealTravel.Domain.Data.Models;
using ZealTravel.Backoffice.Web.Models;
using ZealTravel.Backoffice.Web.Models.Credentials;
using ZealTravel.Application.CredentialManagement.Query;
//using ZealTravel.Backoffice.Web.Models.Agency.Dashboard;

namespace ZealTravel.Backoffice.Web.Mappers
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<CompanyProfileData, CompanyProfiles>()
                 .ForMember(dest => dest.Gst, opt => opt.MapFrom(src => src.GstNo));
            //CreateMap<DailyBooking, DailyBookingReportsResponse>()
            //    .ForMember(dest => dest.NoOfPassengers, opt => opt.MapFrom(src => src.NoOfPassengers))
            //    .ForMember(dest => dest.NoOfBookings, opt => opt.MapFrom(src => src.BookingRef));
            //CreateMap<CompanyProfileData, ProfileDetail>()
            //    .ForMember(dest => dest.AvailableBalance, opt => opt.MapFrom(src => src.AvailableBalance));
            //CreateMap<LedgerReports, LedgerReportResponse>();
            //CreateMap<FlightBookingReport, FlightBookingResponse>();
            //CreateMap<DashboardLedgerData, DashboardLedgerResponse>();
            //CreateMap<DashboardNotificationData, DashboardNotificationResponse>();
            //CreateMap<DashboardChartData, DashboardChartResponse>();
            //CreateMap<DashboardCorporateData, DashboardCorporateResponse>();

            CreateMap<SearchAgencyListData, SearchAgencyData>()
           .ForMember(dest => dest.CompanyName, opt => opt.MapFrom(src => src.CompanyName))
           .ForMember(dest => dest.AccountID, opt => opt.MapFrom(src => src.AccountID))
           .ForMember(dest => dest.City, opt => opt.MapFrom(src => src.City))
           .ForMember(dest => dest.UserType, opt => opt.MapFrom(src => src.UserType))
           //.ForMember(dest => dest.ActiveStatus, opt => opt.MapFrom(src => src.ActiveStatus))
           .ForMember(dest => dest.Active_Status, opt => opt.MapFrom(src => src.Active_Status))
           .ForMember(dest => dest.EventTime, opt => opt.MapFrom(src => src.EventTime))
           .ForMember(dest => dest.AvailableBalance, opt => opt.MapFrom(src => src.AvailableBalance));
            CreateMap<GalelioSupplierAirline, GalileoSupplierDetailViewModel>();
            CreateMap<LccSupplierAirline, LccAirlineViewModel>();
            CreateMap<AirlineApiDetails, AirlineApiViewModel>();
            CreateMap<UapiFopDetail, UapiFopViewModel>();
            CreateMap<UapiCcDetails, UapiCcDetail>();
            CreateMap<SupplierCredProductDetails, SupplierProductDetailsViewModel>();
            CreateMap<PnrMakeDaysDetails, PnrMakeDaysDetail>();
            CreateMap<ProductStatusDetails, ProductStatusViewModel>();
            CreateMap<UapiCcDetails, SupplierUapiCcViewModel>()
            .ForMember(dest => dest.UapiCcDetails, opt => opt.MapFrom(src => new List<UapiCcDetails> { src }))
            .ForMember(dest => dest.NewUapiCc, opt => opt.Ignore());
        }
    }
}
