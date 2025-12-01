using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using ZealTravel.Common.Helpers;
using ZealTravel.Domain.BookingManagement;
using ZealTravel.Infrastructure.Akaasa;

namespace ZealTravel.Infrastructure.UAPI
{
    public class SetPaymentforHoldBooking
    {
        public string ErrorMessage { get; private set; }
        private readonly IBookingManagementService _bookingService;
        private readonly IConfiguration _config;

        public SetPaymentforHoldBooking(IBookingManagementService bookingService,IConfiguration config)
        {
            _bookingService = bookingService;
            _config = config;
        }

        public async Task<string> Set6eHoldTicketPayment(string universalLocatorCode,string targetBranch,string networkUserName,string networkPassword, string UserName, string Password, string searchID,string providerLocatorCode,string reservationLocatorCode,string airPricingInfoKey,string version,string carrierCode, int BookingRef, string CompanyID, string FltType)
        {
            string BookResponse = "";
            string PassengerRequest = string.Empty;
            var sb = new StringBuilder();
            try
            {
                sb.AppendLine(@"<?xml version=""1.0"" encoding=""utf-8""?>");
                sb.AppendLine(@"<soapenv:Envelope xmlns:soapenv=""http://schemas.xmlsoap.org/soap/envelope/""");
                sb.AppendLine(@"                  xmlns:univ=""http://www.travelport.com/schema/universal_v51_0""");
                sb.AppendLine(@"                  xmlns:com=""http://www.travelport.com/schema/common_v51_0""");
                sb.AppendLine(@"                  xmlns:air=""http://www.travelport.com/schema/air_v51_0"">");
                sb.AppendLine(@"  <soapenv:Body>");
                sb.AppendLine($@"    <univ:UniversalRecordModifyReq TargetBranch=""{targetBranch}"" AuthorizedBy=""ZEALTRAVELS"" TraceId=""{searchID}"" ReturnRecord=""true"" Version=""{version}"">");
                sb.AppendLine(@"      <com:BillingPointOfSaleInfo OriginApplication=""UAPI"" />");
                sb.AppendLine($@"      <univ:RecordIdentifier UniversalLocatorCode=""{universalLocatorCode}"" ProviderCode=""ACH"" ProviderLocatorCode=""{providerLocatorCode}""  />");
                sb.AppendLine($@"      <univ:UniversalModifyCmd Key=""{GuidHelper.NewGuid24()}""> ");
                sb.AppendLine($@"        <univ:AirAdd ReservationLocatorCode=""{reservationLocatorCode}"">");
                sb.AppendLine(@"          <air:AirPricingPayment>");

                var env = ConfigurationHelper.GetSetting("ASPNETCORE_ENVIRONMENT")?.ToLower();
                if (env != "production")
                {
                    sb.AppendLine($@"            <com:FormOfPayment Key=""{GuidHelper.NewGuid24()}"" Type=""Credit"">");
                    sb.AppendLine(@"              <com:CreditCard BankCountryCode=""IN"" BankName=""VISA"" CVV=""123"" ExpDate=""2040-09"" Name=""Test Travelport"" Number=""4444333322221111"" Type=""VI"" Key=""1"">");
                    sb.AppendLine(@"                <com:BillingAddress>");
                    sb.AppendLine(@"                  <com:AddressName>Home</com:AddressName>");
                    sb.AppendLine(@"                  <com:Street>Richmond Road</com:Street>");
                    sb.AppendLine(@"                  <com:City>Bangalore</com:City>");
                    sb.AppendLine(@"                  <com:State>Karnataka</com:State>");
                    sb.AppendLine(@"                  <com:PostalCode>560025</com:PostalCode>");
                    sb.AppendLine(@"                  <com:Country>IN</com:Country>");
                    sb.AppendLine(@"                </com:BillingAddress>");
                    sb.AppendLine(@"              </com:CreditCard>");
                    sb.AppendLine(@"            </com:FormOfPayment>");
                }
                else
                {
                    sb.AppendLine($@"            <com:FormOfPayment Key=""{GuidHelper.NewGuid24()}"" Type=""AgencyPayment"">");
                    sb.AppendLine($@"              <com:AgencyPayment AgencyBillingIdentifier=""{UserName}"" AgencyBillingPassword=""{Password}""/>");
                    sb.AppendLine(@"            </com:FormOfPayment>");
                }
                sb.AppendLine($@"            <air:AirPricingInfoRef Key=""{airPricingInfoKey}"" />");
                sb.AppendLine(@"          </air:AirPricingPayment>");
                sb.AppendLine(@"        </univ:AirAdd>");
                sb.AppendLine(@"      </univ:UniversalModifyCmd>");
                sb.AppendLine(@"    </univ:UniversalRecordModifyReq>");
                sb.AppendLine(@"  </soapenv:Body>");
                sb.AppendLine(@"</soapenv:Envelope>");

                string soapRequestXml = sb.ToString();

                CommonUapi cs = new CommonUapi();
                BookResponse = cs.GetResponseUapi(networkUserName, networkPassword, searchID, soapRequestXml, "UniversalRecordService", "Book");

                DBCommon.Logger.dbLoggAPI(searchID, CompanyID, BookingRef, FltType, "SearchCriteria", "PAYMENTFORHOLDBOOKING" , soapRequestXml, BookResponse, PassengerRequest);

                return BookResponse;

            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
                return null;
            }
        }

        public async Task<string> SetUAPIHoldTicketPayment(string universalLocatorCode, string targetBranch, string networkUserName, string networkPassword, string searchID, string providerLocatorCode, string reservationLocatorCode, string airPricingInfoKey, string version,string carrierCode, int BookingRef, string CompanyID, string FltType)
        {
            string BookResponse = "";
            string PassengerRequest = string.Empty;
            var sb = new StringBuilder();
            try
            {
                sb.AppendLine(@"<?xml version=""1.0"" encoding=""utf-8""?>");
                sb.AppendLine($@"<soapenv:Envelope xmlns:soapenv=""http://schemas.xmlsoap.org/soap/envelope/"" 
                            xmlns:univ=""http://www.travelport.com/schema/universal_v51_0"" 
                            xmlns:com=""http://www.travelport.com/schema/common_v51_0"" 
                            xmlns:air=""http://www.travelport.com/schema/air_v51_0"">");
                sb.AppendLine(@"  <soapenv:Body>");
                sb.AppendLine($@"    <univ:UniversalRecordModifyReq TargetBranch=""{targetBranch}"" AuthorizedBy=""ZEALTRAVELS"" TraceId=""{searchID}"" ReturnRecord=""true"" Version=""{version}"">");
                sb.AppendLine(@"      <com:BillingPointOfSaleInfo OriginApplication=""UAPI"" />");
                sb.AppendLine($@"      <univ:RecordIdentifier UniversalLocatorCode=""{universalLocatorCode}"" ProviderCode=""1G"" ProviderLocatorCode=""{providerLocatorCode}"" />");
                sb.AppendLine($@"      <univ:UniversalModifyCmd Key=""{GuidHelper.NewGuid24()}"">");

                sb.Append(@"<univ:UniversalAdd>");



                var activeFop = await _bookingService.GetActiveFormOfPayment();
                if (string.Equals(activeFop, "CASH", StringComparison.OrdinalIgnoreCase))
                {
                    sb.AppendLine($@"            <com:FormOfPayment Key=""{GuidHelper.NewGuid24()}"" Type=""Cash""/>");
                }
                else
                {
                    DataRow drCC = await _bookingService.GetCCDetails(carrierCode);
                    if (drCC == null)
                        throw new InvalidOperationException($"No credit‐card on file for carrier {carrierCode}");

                    bool isProd = _config["ASPNETCORE_ENVIRONMENT"].Equals("Production", StringComparison.OrdinalIgnoreCase);

                    sb.AppendLine($@"            <com:FormOfPayment Key=""{GuidHelper.NewGuid24()}"" Type=""Credit"">");

                    if (!isProd)
                    {                        
                        sb.AppendLine(@"              <com:CreditCard Key=""1"" Number=""4111111111111111"" Type=""VI"" Name=""Test Travelport"" ExpDate=""2040-09"" CVV=""123"" BankName=""VISA"" BankCountryCode=""IN"">");
                        sb.AppendLine(@"                <com:BillingAddress>");
                        sb.AppendLine(@"                  <com:AddressName>Home</com:AddressName>");
                        sb.AppendLine(@"                  <com:Street>Richmond Road</com:Street>");
                        sb.AppendLine(@"                  <com:City>Bangalore</com:City>");
                        sb.AppendLine(@"                  <com:State>Karnataka</com:State>");
                        sb.AppendLine(@"                  <com:PostalCode>560025</com:PostalCode>");
                        sb.AppendLine(@"                  <com:Country>IN</com:Country>");
                        sb.AppendLine(@"                </com:BillingAddress>");
                        sb.AppendLine(@"              </com:CreditCard>");
                    }
                    else
                    { 
                        sb.AppendLine($@"              <com:CreditCard Key=""1"" Number=""{drCC["Number"]}"" Type=""{drCC["Type"]}"" Name=""{drCC["Name"]}"" ExpDate=""{drCC["ExpDate"]}"" CVV=""{drCC["CVV"]}"" BankName=""{drCC["BankName"]}"" BankCountryCode=""{drCC["BankCountryCode"]}"">");
                        sb.AppendLine(@"                <com:BillingAddress>");
                        sb.AppendLine($@"                  <com:AddressName>{drCC["AddressName"]}</com:AddressName>");
                        sb.AppendLine($@"                  <com:Street>{drCC["Street"]}</com:Street>");
                        sb.AppendLine($@"                  <com:City>{drCC["City"]}</com:City>");
                        sb.AppendLine($@"                  <com:State>{drCC["State"]}</com:State>");
                        sb.AppendLine($@"                  <com:PostalCode>{drCC["PostalCode"]}</com:PostalCode>");
                        sb.AppendLine($@"                  <com:Country>{drCC["Country"]}</com:Country>");
                        sb.AppendLine(@"                </com:BillingAddress>");
                        sb.AppendLine(@"              </com:CreditCard>");
                    }
                    sb.AppendLine(@"            </com:FormOfPayment>");
                }
                sb.AppendLine(@"         </univ:UniversalAdd>");
                sb.AppendLine(@"      </univ:UniversalModifyCmd>");
                sb.AppendLine(@"    </univ:UniversalRecordModifyReq>");
                sb.AppendLine(@"  </soapenv:Body>");
                sb.AppendLine(@"</soapenv:Envelope>");

                string soapRequestXml = sb.ToString();

                CommonUapi cs = new CommonUapi();
                BookResponse = cs.GetResponseUapi(networkUserName, networkPassword, searchID, soapRequestXml, "UniversalRecordService", "Book");

                DBCommon.Logger.dbLoggAPI(searchID, CompanyID, BookingRef, FltType, "SearchCriteria", "PAYMENTFORHOLDBOOKING", soapRequestXml, BookResponse, PassengerRequest);


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
