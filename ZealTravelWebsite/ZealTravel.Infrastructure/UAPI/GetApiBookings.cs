using System;
using System.Text;

namespace ZealTravel.Infrastructure.UAPI
{
    class GetApiBookings
    {
        public string ErrorMessage { get; private set; }

        public string Get6eBookingDataByRecordLocator(string universalRecordLocatorCode, string targetBranch,string NetworkUserName, string NetworkPassword,string SearchID)
        {
            try
            {
                string BookResponse = "";
                var sb = new StringBuilder();

                // XML declaration + static envelope
                sb.AppendLine(@"<?xml version=""1.0"" encoding=""utf-8""?>");
                sb.AppendLine(@"<soapenv:Envelope xmlns:soapenv=""http://schemas.xmlsoap.org/soap/envelope/"">");
                sb.AppendLine(@"    <soapenv:Body>");
                sb.AppendLine($@"        <univ:UniversalRecordRetrieveReq
                                                xmlns:com=""http://www.travelport.com/schema/common_v51_0""
                                                xmlns:univ=""http://www.travelport.com/schema/universal_v51_0""
                                                TargetBranch=""{targetBranch}""
                                                AuthorizedBy=""ZEALTRAVELS""
                                                TraceId=""{SearchID}"">");
                sb.AppendLine(@"            <com:BillingPointOfSaleInfo OriginApplication=""UAPI"" />");
                sb.AppendLine($@"            <univ:UniversalRecordLocatorCode>{universalRecordLocatorCode}</univ:UniversalRecordLocatorCode>");
                sb.AppendLine(@"        </univ:UniversalRecordRetrieveReq>");
                sb.AppendLine(@"    </soapenv:Body>");
                sb.AppendLine(@"</soapenv:Envelope>");

                string soapRequestXml = sb.ToString();

                CommonUapi cs = new CommonUapi();
                BookResponse = cs.GetResponseUapi(NetworkUserName, NetworkPassword, SearchID, soapRequestXml, "UniversalRecordService", "Book");

                return BookResponse;
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
                return null;
            }
        }

        public string GetBookingDataByRecordLocator(string universalRecordLocatorCode, string targetBranch, string NetworkUserName, string NetworkPassword, string SearchID)
        {
            try
            {
                string BookResponse = "";
                var sb = new StringBuilder();

                // XML declaration + Envelope with soap, xsi, xsd namespaces
                sb.AppendLine(@"<?xml version=""1.0"" encoding=""utf-8""?>");
                sb.AppendLine(@"<soap:Envelope xmlns:soap=""http://schemas.xmlsoap.org/soap/envelope/""");
                sb.AppendLine(@"               xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance""");
                sb.AppendLine(@"               xmlns:xsd=""http://www.w3.org/2001/XMLSchema"">");
                sb.AppendLine(@"    <soap:Body>");

                // UniversalRecordRetrieveReq: univ namespace v52_0, hard‑coded AuthorizedBy & TraceId
                sb.AppendLine($@"        <univ:UniversalRecordRetrieveReq xmlns:univ=""http://www.travelport.com/schema/universal_v52_0""
                                            AuthorizedBy=""ZEALTRAVELS""
                                            TargetBranch=""{targetBranch}""
                                            TraceId=""{SearchID}"">");
                sb.AppendLine(@"            <com:BillingPointOfSaleInfo xmlns:com=""http://www.travelport.com/schema/common_v52_0""
                                          OriginApplication=""UAPI"" />");
                sb.AppendLine($@"            <univ:UniversalRecordLocatorCode>{universalRecordLocatorCode}</univ:UniversalRecordLocatorCode>");
                sb.AppendLine(@"        </univ:UniversalRecordRetrieveReq>");
                sb.AppendLine(@"    </soap:Body>");
                sb.AppendLine(@"</soap:Envelope>");

                string soapRequestXml = sb.ToString();

                CommonUapi cs = new CommonUapi();
                BookResponse = cs.GetResponseUapi(NetworkUserName, NetworkPassword, SearchID, soapRequestXml, "UniversalRecordService", "Book");

                return BookResponse;
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
                return null;
            }
        }

    }
}
