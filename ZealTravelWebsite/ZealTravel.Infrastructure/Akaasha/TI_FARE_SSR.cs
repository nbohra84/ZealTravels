using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TIRequestResponse;
using CommonComponents;

namespace ZealTravel.Infrastructure.Akaasha
{
    public class TI_FARE_SSR
    {
        public string GetFlightFareRulesRQ(string SearchID, string Idx)
        {
            string jsonFlightRQ = "";
            try
            {
                string[] splitIdx = Idx.Split('_');
                string flightJson = string.Empty;
                string searchJson = string.Empty;
                string fareruleJson = string.Empty;

                CommonComponents.FlightDBOperation flightDBOperation = new CommonComponents.FlightDBOperation("FTI");
                flightDBOperation.GetFlightFromCache(SearchID, Idx, ref flightJson, ref searchJson, ref fareruleJson);

                TIRequestResponse.Authentication Auth = new TIRequestResponse.Authentication();

                TIRequestResponse.FlightRequestBody FltRQBody = new TIRequestResponse.FlightRequestBody();
                FltRQBody.FIdx = Idx;

                TIRequestResponse.FlightRequestHeader FltRQHeader = new TIRequestResponse.FlightRequestHeader();
                FltRQHeader.Action = "GetFlightFareRules";

                SearchRQ searchRQ = SerializeDeserialize.DeserializeObject<SearchRQ>(Convert.ToString(searchJson), true);
                TIRequestResponse.GeneralInfo GeneralInfo = new TIRequestResponse.GeneralInfo();
                GeneralInfo = searchRQ.GeneralInfo;

                TIRequestResponse.FlightRQ objFlightRQ = new TIRequestResponse.FlightRQ();
                objFlightRQ.Authentication = Auth;
                objFlightRQ.FlightRequestBody = FltRQBody;
                objFlightRQ.FlightRequestHeader = FltRQHeader;
                objFlightRQ.GeneralInfo = GeneralInfo;

                jsonFlightRQ = CommonComponents.SerializeDeserialize.SerializeInJsonString(objFlightRQ, "FlightRQ");
            }
            catch (Exception ex)
            {
                DBCommon.Logger.dbLogg("", 0, "GetFlightFareRulesRQ-TI_FARE_SSR", jsonFlightRQ, Idx, SearchID, ex.Message);
            }
            return jsonFlightRQ;
        }
    }
}
