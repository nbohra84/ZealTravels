using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZealTravel.Infrastructure.TBO
{
    class GetApiFareCalender
    {
        public string errorMessage;
        //-----------------------------------------------------------------------------------------------
        private string Searchid;
        private string Companyid;
        //-----------------------------------------------------------------------------------------------
        public GetApiFareCalender(string Searchid, string Companyid)
        {
            this.Searchid = Searchid;
            this.Companyid = Companyid;
        }
        //================================================================================================================================================
        public string GetCalenderData(string Supplierid, string Password, string Origin, string Destination, string BeginDate, string Cabin, string EndUserIp)
        {
            string GetApiRequest = string.Empty;
            string GetApiResponse = string.Empty;

            try
            {
                GetApiLogin objLogin = new GetApiLogin();
                string Tokenid = objLogin.GetTokenid(Supplierid, Password, Searchid, Companyid, EndUserIp);
                if (Tokenid != null && Tokenid.Length > 0)
                {
                    GetApiRequests objApiRequests = new GetApiRequests();
                    GetApiRequest = objApiRequests.GetFareCalenderRequest(Tokenid, Origin, Destination, BeginDate, Cabin, EndUserIp);

                    GetApiCommonFunctions objApiCommonFunctions = new GetApiCommonFunctions();
                    GetApiResponse = objApiCommonFunctions.GetApiHttpResponse(Supplierid, Searchid, Companyid, 0, GetApiRequest, GetApiServiceURL.getcalendarfare_url, "GetCalendarFare");
                    errorMessage = objApiCommonFunctions.errorMessage;

                    if (GetApiResponse != null && GetApiResponse.IndexOf("SearchResults") != -1)
                    {
                        DataTable dtBound = new DataTable();
                        DataSet dsCalender = GetCommonFunctions.StringToDataSet(GetApiResponse);
                        dtBound = dsCalender.Tables["SearchResults"].Clone();
                        dtBound = dsCalender.Tables["SearchResults"].Copy();
                        dtBound.TableName = "AvailabilityResponse";

                        DataSet dsBound = new DataSet();
                        dsBound.Tables.Add(dtBound);
                        dsBound.DataSetName = "root";
                        return GetCommonFunctions.DataSetToString(dsBound);
                    }
                }
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                DBCommon.Logger.dbLogg(Companyid, 0, "GetCalenderData", "air_tbo-GetApiFareCalender", Supplierid, Searchid, errorMessage);
            }

            //GetCommonFunctions.SetApiLogs(Searchid, "GetBookingData", "GetApiBookings", GetApiRequest, GetApiResponse, RecordLocator);

            return "";
        }
    }
}
