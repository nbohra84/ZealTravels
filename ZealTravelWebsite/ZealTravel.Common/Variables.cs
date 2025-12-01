using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZealTravel.Common
{
    public struct GetApiServiceURL
    {
        public static string GetApiClientid
        {
            get
            {
                return "tboprod";
                //return "ApiIntegrationNew";
            }
        }
        public static string getauthenticate_url
        {
            get
            {
                return "https://api.travelboutiqueonline.com/SharedAPI/SharedData.svc/rest/authenticate";
                //return "http://api.tektravels.com/SharedServices/SharedData.svc/rest/Authenticate";
            }
        }
        public static string getAgencyBalance_url
        {
            get
            {
                return "https://api.travelboutiqueonline.com/SharedAPI/SharedData.svc/rest/getAgencyBalance";
                //return "http://api.tektravels.com/SharedServices/SharedData.svc/rest/GetAgencyBalance";
            }
        }
        public static string getlogout_url
        {
            get
            {
                return "https://api.travelboutiqueonline.com/SharedAPI/SharedData.svc/rest/Logout";
                //return "http://api.tektravels.com/SharedServices/SharedData.svc/rest/Logout";
            }
        }
        public static string getsearch_url
        {
            get
            {
                return "https://tboapi.travelboutiqueonline.com/AirAPI_V10/AirService.svc/rest/Search";
                //return "http://api.tektravels.com/BookingEngineService_Air/AirService.svc/rest/Search/";
            }
        }
        public static string getfarerule_url
        {
            get
            {
                return "https://tboapi.travelboutiqueonline.com/AirAPI_V10/AirService.svc/rest/FareRule";
                //return "http://api.tektravels.com/BookingEngineService_Air/AirService.svc/rest/FareRule/";
            }
        }
        public static string getfarequote_url
        {
            get
            {
                return "https://tboapi.travelboutiqueonline.com/AirAPI_V10/AirService.svc/rest/FareQuote";
                //return "http://api.tektravels.com/BookingEngineService_Air/AirService.svc/rest/FareQuote/";
            }
        }
        public static string getssr_url
        {
            get
            {
                return "https://tboapi.travelboutiqueonline.com/AirAPI_V10/AirService.svc/rest/SSR";
                //return "http://api.tektravels.com/BookingEngineService_Air/AirService.svc/rest/SSR/";
            }
        }
        public static string getbook_url
        {
            get
            {
                return "https://booking.travelboutiqueonline.com/AirAPI_V10/AirService.svc/rest/Book";
                //return "http://api.tektravels.com/BookingEngineService_Air/AirService.svc/rest/Book/";
            }
        }
        public static string getticket_url
        {
            get
            {
                return "https://booking.travelboutiqueonline.com/AirAPI_V10/AirService.svc/rest/Ticket";
                //return "http://api.tektravels.com/BookingEngineService_Air/AirService.svc/rest/Ticket/";
            }
        }
        public static string getbookingdetails_url
        {
            get
            {
                return "https://booking.travelboutiqueonline.com/AirAPI_V10/AirService.svc/rest/GetBookingDetails/";
                //return "http://api.tektravels.com/BookingEngineService_Air/AirService.svc/rest/GetBookingDetails/";
            }
        }
        public static string getcalendarfare_url
        {
            get
            {
                return "http://tboapi.travelboutiqueonline.com/AirAPI_V10/AirService.svc/rest/GetCalendarFare/";
                //return "http://api.tektravels.com/BookingEngineService_Air/AirService.svc/rest/GetCalendarFare/";
            }
        }
        public static string getpricerbd_url
        {
            get
            {
                //return "";
                return "http://api.tektravels.com/BookingEngineService_Air/AirService.svc/rest/PriceRBD/";
            }
        }
    }
}
