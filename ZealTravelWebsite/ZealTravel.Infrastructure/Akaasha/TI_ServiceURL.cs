using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZealTravel.Infrastructure.Akaasha
{
    public class TI_ServiceURL
    {
        public static string GetSearchURL
        {
            get
            {
                //return "http://services.zealtravels.in:8080/api/flight/LowFareSearch";
                //return "https://t-extprt-reyalrb.qp.akasaair.com/api/flight/LowFareSearch";
                //return "https://t-extprt-reyalrb.qp.akasaair.com/api/nsk/v4/availability/search/simple";
                return "http://localhost:62905/api/flight/LowFareSearch";
            }
        }
        public static string GetFareURL
        {
            get
            {
                return "http://services.zealtravels.in:8080/api/flight/GetFlightFareRules";
                //return "http://localhost:27987/api/flight/GetFlightFareRules";
            }
        }
        public static string GetBookURL
        {
            get
            {
                return "http://services.zealtravels.in:8080/api/flight/FlightBook";
                //return "http://localhost:27987/api/flight/FlightBook";
            }
        }
    }
}
