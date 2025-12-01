using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZealTravel.Domain.Interfaces.Loggers;
using ZealTravel.Domain.Data;
namespace ZealTravel.Infrastructure.TBO
{
    public class GetApiAvailabilityFlight
    {
        public string errorMessage;
        public DataTable dtBound;
        //-----------------------------------------------------------------------------------------------
        private string Searchid;
        private string DomainCode;
        private string Supplierid;
        private string Password;
        private string EndUserIp;
        private string Companyid;
        //private ILogger _logger;
        //-----------------------------------------------------------------------------------------------
        public GetApiAvailabilityFlight(string Supplierid, string Password, string Searchid, string Companyid, string EndUserIp)
        {
            //_logger = logger;
            this.Searchid = Searchid;
            this.DomainCode = Companyid;
            this.Supplierid = Supplierid;
            this.Password = Password;
            this.EndUserIp = EndUserIp;
            this.Companyid = Companyid;
        }
        public void GetOneWay(string Origin, string Destination, string BeginDate, int Adt, int Chd, int Inf, string Cabin, ArrayList CarrierList, string Sector, string FltType)
        {
            dtBound = Schema.SchemaFlights;

            string GetRequest = string.Empty;
            string GetResponse = string.Empty;

            try
            {
                GetApiLogin objLogin = new GetApiLogin();
                string Tokenid = objLogin.GetTokenid(Supplierid, Password, Searchid, Companyid, EndUserIp);
                if (Tokenid != null && Tokenid.Length > 0)
                {
                    GetApiRequests objApiRequests = new GetApiRequests();
                    GetRequest = objApiRequests.GetOneWaySearchRequest(Tokenid, Origin, Destination, BeginDate, Adt, Chd, Inf, Cabin, CarrierList, EndUserIp);

                    GetApiCommonFunctions objApiCommonFunctions = new GetApiCommonFunctions();
                    GetResponse = objApiCommonFunctions.GetApiHttpResponse(Supplierid, Searchid, Companyid, 0, GetRequest, GetApiServiceURL.getsearch_url, "Search");
                    errorMessage = objApiCommonFunctions.errorMessage;

                    if (GetResponse != null && GetResponse.IndexOf("Segments") != -1)
                    {
                        SetAvailabilityModifiers objModifier = new SetAvailabilityModifiers(Supplierid, Searchid, Companyid, Origin, Destination, Cabin, Sector, Tokenid);
                        dtBound = objModifier.FlightModifier(GetResponse, false);
                        errorMessage = objModifier.errorMessage;
                    }
                }
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                //DBCommon.Logger.dbLogg(Companyid, 0, "GetOneWay", "air_tbo-GetApiAvailabilityFlight", Supplierid, Searchid, errorMessage);
            }
        }
        public async Task<DataTable> GetOneWayAsync(string Origin, string Destination, string BeginDate, int Adt, int Chd, int Inf, string Cabin, ArrayList CarrierList, string Sector, string FltType)
        {
            dtBound = Schema.SchemaFlights;

            string GetRequest = string.Empty;
            string GetResponse = string.Empty;

            try
            {
                GetApiLogin objLogin = new GetApiLogin();
                string Tokenid = objLogin.GetTokenid(Supplierid, Password, Searchid, Companyid, EndUserIp);
                if (Tokenid != null && Tokenid.Length > 0)
                {
                    GetApiRequests objApiRequests = new GetApiRequests();
                    GetRequest = objApiRequests.GetOneWaySearchRequest(Tokenid, Origin, Destination, BeginDate, Adt, Chd, Inf, Cabin, CarrierList, EndUserIp);

                    GetApiCommonFunctions objApiCommonFunctions = new GetApiCommonFunctions();
                    GetResponse =await objApiCommonFunctions.GetApiHttpResponseAsync(Supplierid, Searchid, Companyid, 0, GetRequest, GetApiServiceURL.getsearch_url, "Search");
                    errorMessage = objApiCommonFunctions.errorMessage;

                    if (GetResponse != null && GetResponse.IndexOf("Segments") != -1)
                    {
                        SetAvailabilityModifiers objModifier = new SetAvailabilityModifiers(Supplierid, Searchid, Companyid, Origin, Destination, Cabin, Sector, Tokenid);
                        dtBound = objModifier.FlightModifier(GetResponse, false);
                        errorMessage = objModifier.errorMessage;
                    }
                }
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                //DBCommon.Logger.dbLogg(Companyid, 0, "GetOneWay", "air_tbo-GetApiAvailabilityFlight", Supplierid, Searchid, errorMessage);
            }
            return dtBound;
        }
        public void GetRT(string Origin, string Destination, string BeginDate, string EndDate, int Adt, int Chd, int Inf, string Cabin, ArrayList CarrierList, string Sector)
        {
            string GetRequest = string.Empty;
            string GetResponse = string.Empty;

            try
            {
                GetApiLogin objLogin = new GetApiLogin();
                string Tokenid = objLogin.GetTokenid(Supplierid, Password, Searchid, Companyid, EndUserIp);
                if (Tokenid != null && Tokenid.Length > 0)
                {
                    GetApiRequests objApiRequests = new GetApiRequests();
                    GetRequest = objApiRequests.GetRoundWaySearchRequest(Tokenid, Origin, Destination, BeginDate, EndDate, Adt, Chd, Inf, Cabin, CarrierList, EndUserIp);
                    errorMessage = objApiRequests.errorMessage;

                    GetApiCommonFunctions objApiCommonFunctions = new GetApiCommonFunctions();
                    GetResponse = objApiCommonFunctions.GetApiHttpResponse(Supplierid, Searchid, Companyid, 0, GetRequest, GetApiServiceURL.getsearch_url, "Search");
                    errorMessage = objApiCommonFunctions.errorMessage;

                    if (GetResponse != null && GetResponse.IndexOf("Segments") != -1)
                    {
                        SetAvailabilityModifiers objModifier = new SetAvailabilityModifiers(Supplierid, Searchid, Companyid, Origin, Destination, Cabin, Sector, Tokenid);
                        if (Sector.Equals("D"))
                        {
                            dtBound = objModifier.FlightModifier(GetResponse, false);
                        }
                        else
                        {
                            dtBound = objModifier.FlightModifier(GetResponse, true);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                //DBCommon.Logger.dbLogg(Companyid, 0, "GetRT", "air_tbo-GetApiAvailabilityFlight", Supplierid, Searchid, errorMessage);
            }
        }
        public async Task<DataTable> GetRT_Async(string Origin, string Destination, string BeginDate, string EndDate, int Adt, int Chd, int Inf, string Cabin, ArrayList CarrierList, string Sector)
        {
            string GetRequest = string.Empty;
            string GetResponse = string.Empty;

            try
            {
                GetApiLogin objLogin = new GetApiLogin();
                string Tokenid = objLogin.GetTokenid(Supplierid, Password, Searchid, Companyid, EndUserIp);
                if (Tokenid != null && Tokenid.Length > 0)
                {
                    GetApiRequests objApiRequests = new GetApiRequests();
                    GetRequest = objApiRequests.GetRoundWaySearchRequest(Tokenid, Origin, Destination, BeginDate, EndDate, Adt, Chd, Inf, Cabin, CarrierList, EndUserIp);
                    errorMessage = objApiRequests.errorMessage;

                    GetApiCommonFunctions objApiCommonFunctions = new GetApiCommonFunctions();
                    GetResponse =await objApiCommonFunctions.GetApiHttpResponseAsync(Supplierid, Searchid, Companyid, 0, GetRequest, GetApiServiceURL.getsearch_url, "Search");
                    errorMessage = objApiCommonFunctions.errorMessage;

                    if (GetResponse != null && GetResponse.IndexOf("Segments") != -1)
                    {
                        SetAvailabilityModifiers objModifier = new SetAvailabilityModifiers(Supplierid, Searchid, Companyid, Origin, Destination, Cabin, Sector, Tokenid);
                        if (Sector.Equals("D"))
                        {
                            dtBound = objModifier.FlightModifier(GetResponse, false);
                        }
                        else
                        {
                            dtBound = objModifier.FlightModifier(GetResponse, true);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                //DBCommon.Logger.dbLogg(Companyid, 0, "GetRT", "air_tbo-GetApiAvailabilityFlight", Supplierid, Searchid, errorMessage);
            }
            return dtBound;
        }
        public void GetRTLCC(string Origin, string Destination, string BeginDate, string EndDate, int Adt, int Chd, int Inf, string Cabin, ArrayList CarrierList, string Sector)
        {
            string GetRequest = string.Empty;
            string GetResponse = string.Empty;

            try
            {
                GetApiLogin objLogin = new GetApiLogin();
                string Tokenid = objLogin.GetTokenid(Supplierid, Password, Searchid, Companyid, EndUserIp);
                if (Tokenid != null && Tokenid.Length > 0)
                {
                    GetApiRequests objApiRequests = new GetApiRequests();
                    GetRequest = objApiRequests.GetRoundWaySpecialDomesticSearchRequest(Tokenid, Origin, Destination, BeginDate, EndDate, Adt, Chd, Inf, Cabin, CarrierList, EndUserIp);

                    GetApiCommonFunctions objApiCommonFunctions = new GetApiCommonFunctions();
                    GetResponse = objApiCommonFunctions.GetApiHttpResponse(Supplierid, Searchid, Companyid, 0, GetRequest, GetApiServiceURL.getsearch_url, "Search");
                    errorMessage = objApiCommonFunctions.errorMessage;

                    if (GetResponse != null && GetResponse.IndexOf("Segments") != -1)
                    {
                        SetAvailabilityModifiers objModifier = new SetAvailabilityModifiers(Supplierid, Searchid, Companyid, Origin, Destination, Cabin, Sector, Tokenid);
                        dtBound = objModifier.FlightModifier(GetResponse, false);
                        errorMessage = objModifier.errorMessage;
                    }
                }
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                //DBCommon.Logger.dbLogg(Companyid, 0, "GetRTLCC", "air_tbo-GetApiAvailabilityFlight", Supplierid, Searchid, errorMessage);
            }
        }
        public async Task<DataTable> GetRTLCC_Async(string Origin, string Destination, string BeginDate, string EndDate, int Adt, int Chd, int Inf, string Cabin, ArrayList CarrierList, string Sector)
        {
            string GetRequest = string.Empty;
            string GetResponse = string.Empty;

            try
            {
                GetApiLogin objLogin = new GetApiLogin();
                string Tokenid = objLogin.GetTokenid(Supplierid, Password, Searchid, Companyid, EndUserIp);
                if (Tokenid != null && Tokenid.Length > 0)
                {
                    GetApiRequests objApiRequests = new GetApiRequests();
                    GetRequest = objApiRequests.GetRoundWaySpecialDomesticSearchRequest(Tokenid, Origin, Destination, BeginDate, EndDate, Adt, Chd, Inf, Cabin, CarrierList, EndUserIp);

                    GetApiCommonFunctions objApiCommonFunctions = new GetApiCommonFunctions();
                    GetResponse =await objApiCommonFunctions.GetApiHttpResponseAsync(Supplierid, Searchid, Companyid, 0, GetRequest, GetApiServiceURL.getsearch_url, "Search");
                    errorMessage = objApiCommonFunctions.errorMessage;

                    if (GetResponse != null && GetResponse.IndexOf("Segments") != -1)
                    {
                        SetAvailabilityModifiers objModifier = new SetAvailabilityModifiers(Supplierid, Searchid, Companyid, Origin, Destination, Cabin, Sector, Tokenid);
                        dtBound = objModifier.FlightModifier(GetResponse, false);
                        errorMessage = objModifier.errorMessage;
                    }
                }
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                //DBCommon.Logger.dbLogg(Companyid, 0, "GetRTLCC", "air_tbo-GetApiAvailabilityFlight", Supplierid, Searchid, errorMessage);
            }
            return dtBound;
        }
        public void GetMC(string RequestMC, string Cabin, string Sector)
        {
            string GetRequest = string.Empty;
            string GetResponse = string.Empty;

            try
            {
                GetApiLogin objLogin = new GetApiLogin();
                string Tokenid = objLogin.GetTokenid(Supplierid, Password, Searchid, Companyid, EndUserIp);
                if (Tokenid != null && Tokenid.Length > 0)
                {
                    GetApiRequests objApiRequests = new GetApiRequests();
                    GetRequest = objApiRequests.GetMulticitySearchRequest(Tokenid, RequestMC, EndUserIp);

                    GetApiCommonFunctions objApiCommonFunctions = new GetApiCommonFunctions();
                    GetResponse = objApiCommonFunctions.GetApiHttpResponse(Supplierid, Searchid, Companyid, 0, GetRequest, GetApiServiceURL.getsearch_url, "Search");
                    errorMessage = objApiCommonFunctions.errorMessage;

                    if (GetResponse != null && GetResponse.IndexOf("Segments") != -1)
                    {
                        SetAvailabilityModifierMC objModifier = new SetAvailabilityModifierMC(Supplierid, Searchid, Companyid, Cabin, Sector, Tokenid);
                        dtBound = objModifier.FlightModifier(RequestMC, GetResponse, false);
                        errorMessage = objModifier.errorMessage;
                    }
                }
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                //DBCommon.Logger.dbLogg(Companyid, 0, "GetMC", "air_tbo-GetApiAvailabilityFlight", Supplierid, Searchid, errorMessage);
            }
        }
        public async Task<DataTable> GetMC_Async(string RequestMC, string Cabin, string Sector)
        {
            string GetRequest = string.Empty;
            string GetResponse = string.Empty;

            try
            {
                GetApiLogin objLogin = new GetApiLogin();
                string Tokenid = objLogin.GetTokenid(Supplierid, Password, Searchid, Companyid, EndUserIp);
                if (Tokenid != null && Tokenid.Length > 0)
                {
                    GetApiRequests objApiRequests = new GetApiRequests();
                    GetRequest = objApiRequests.GetMulticitySearchRequest(Tokenid, RequestMC, EndUserIp);

                    GetApiCommonFunctions objApiCommonFunctions = new GetApiCommonFunctions();
                    GetResponse =await objApiCommonFunctions.GetApiHttpResponseAsync(Supplierid, Searchid, Companyid, 0, GetRequest, GetApiServiceURL.getsearch_url, "Search");
                    errorMessage = objApiCommonFunctions.errorMessage;

                    if (GetResponse != null && GetResponse.IndexOf("Segments") != -1)
                    {
                        SetAvailabilityModifierMC objModifier = new SetAvailabilityModifierMC(Supplierid, Searchid, Companyid, Cabin, Sector, Tokenid);
                        dtBound = objModifier.FlightModifier(RequestMC, GetResponse, false);
                        errorMessage = objModifier.errorMessage;
                    }
                }
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                //DBCommon.Logger.dbLogg(Companyid, 0, "GetMC", "air_tbo-GetApiAvailabilityFlight", Supplierid, Searchid, errorMessage);
            }
            return dtBound;
        }
    }
}

