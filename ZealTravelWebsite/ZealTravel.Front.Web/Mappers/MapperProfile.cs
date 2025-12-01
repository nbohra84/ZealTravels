using AutoMapper;
using ZealTravel.Application.AgencyManagement.Queries;
using ZealTravel.Application.AgencyManagement.Queries.AgencyDashboard;
using ZealTravel.Application.BookingManagement.Query;
using ZealTravel.Application.ReportManagement.Queries;
using ZealTravel.Domain.Data.Models;
using ZealTravel.Domain.Data.Models.Booking;
using ZealTravel.Front.Web.Models;
using ZealTravel.Front.Web.Models.Agency.Booking;
using ZealTravel.Front.Web.Models.Agency.Dashboard;

namespace ZealTravel.Front.Web.Mappers
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<DailyBooking, DailyBookingReportsResponse>()
                .ForMember(dest => dest.NoOfPassengers, opt => opt.MapFrom(src => src.NoOfPassengers))
                .ForMember(dest => dest.NoOfBookings, opt => opt.MapFrom(src => src.BookingRef));
            CreateMap<CompanyProfileData, ProfileDetail>()
                .ForMember(dest => dest.AvailableBalance, opt => opt.MapFrom(src => src.AvailableBalance));
            CreateMap<LedgerReports, LedgerReportResponse>();
            CreateMap<AirBookingReport, FlightBookingResponse>();
            CreateMap<DashboardLedgerData, DashboardLedgerResponse>();
            CreateMap<DashboardNotificationData, DashboardNotificationResponse>();
            CreateMap<DashboardChartData, DashboardChartResponse>();
            CreateMap<DashboardCorporateData, DashboardCorporateResponse>();
            CreateMap<BookingDetail, BookingDetailData>()
.ForMember(dest => dest.CompanyFlightDetailAirlines, opt => opt.MapFrom(src => src.CompanyFlightDetailAirlines))
.ForMember(dest => dest.CompanyFlightSegmentDetailAirlines, opt => opt.MapFrom(src => src.CompanyFlightSegmentDetailAirlines))
.ForMember(dest => dest.CompanyFareDetailAirlines, opt => opt.MapFrom(src => src.CompanyFareDetailAirlines))
.ReverseMap();// If needed
              //.ForMember(dest => dest.CompanyFlightSegmentDetailAirlines, opt => opt.MapFrom(src => src.CompanyFlightSegmentDetailAirlines))
              //.ForMember(dest => dest.CompanyFareDetailAirlines, opt => opt.MapFrom(src => src.CompanyFareDetailAirlines))
              //.ReverseMap();
        }
    }
}
