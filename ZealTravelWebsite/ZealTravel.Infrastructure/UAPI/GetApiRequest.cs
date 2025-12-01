using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZealTravel.Infrastructure.UAPI
{
    class GetApiRequest
    {
        public string GetTicketingRQ(string SearchID, string AirReservationLocatorCode, string TargetBranch)
        {
            string RQ = string.Empty;
            RQ += @"<?xml version=""1.0"" encoding=""utf-16""?>";
            RQ += @"<air:AirTicketingReq ReturnInfoOnFail=""true"" xmlns:air=""http://www.travelport.com/schema/air_v51_0"" TargetBranch=" + TargetBranch + " TraceId=" + "\"" + SearchID + "\"" + " >";
            RQ += @"<com:BillingPointOfSaleInfo OriginApplication=""uapi"" xmlns:com=""http://www.travelport.com/schema/common_v51_0"" />";
            RQ += @"<air:AirReservationLocatorCode>" + AirReservationLocatorCode + "</air:AirReservationLocatorCode>";
            RQ += @"</air:AirTicketingReq>";

            return RQ;
        }
    }
}
