
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using ZealTravel.Infrastructure.Akaasa;
using Type = ZealTravel.Infrastructure.Akaasa.Type;

namespace ZealTravel.Infrastructure.Akaasha
{
    /// <summary>
    /// Summary description for CS_FARE_SSR
    /// </summary>
    public class CS_FARE_SSR
    {
        public CS_FARE_SSR()
        {

        }

        public string GetFlightFareRulesRQ_V2(string SearchID, DataTable dtBound)
        {
            string jsonFlightRQ = "";
            int i = 0;
            string Idx = "";
            try
            {
                //string[] splitIdx = Idx.Split('_');
                string flightJson = string.Empty;
                string searchJson = string.Empty;
                string fareruleJson = string.Empty;


                QuoteRqQpV1 requestQuote = new QuoteRqQpV1();
                foreach (DataRow _row in dtBound.Rows)
                {
                    Idx = _row["API_AirlineID"].ToString();
                    string[] splitIdx = Idx.Split('_');

                    Ssr _ssr = new Ssr();
                    Identifier _identifier = new Identifier();
                    Market _market = new Market();
                    List<Item> _items = new List<Item>();
                    Item _item = new Item();
                    SsrItem _ssrItem = new SsrItem();
                    Designator4Quot _designator = new Designator4Quot();
                    List<Ssr> _Ssrs = new List<Ssr>();
                    List<Key> _keys = new List<Key>();

                    _identifier.identifier = splitIdx[0];
                    _identifier.CarrierCode = splitIdx[1];


                    _market.identifier = _identifier;
                    _market.destination = _row["Destination"].ToString();
                    _market.origin = _row["Origin"].ToString();
                    var _dt = _row["DepartureDate"].ToString();
                    _market.departureDate = Convert.ToDateTime(_dt);



                    _item.passengerType = "ADT";


                    _ssrItem.ssrCode = "INFT";
                    _ssrItem.count = 1; // Convert.ToInt16((_row["Inf"] == null) ? 0 : _row["Inf"]);


                    _designator.Destination = _row["Destination"].ToString();
                    _designator.Origin = _row["Origin"].ToString();
                    _designator.departureDate = Convert.ToDateTime(_dt);
                    _ssrItem.Designator = _designator;
                    _item.ssrs.Add(_ssrItem);
                    _items.Add(_item);

                    _ssr.Market = _market;
                    _ssr.items = _items;

                    _Ssrs.Add(_ssr);

                    requestQuote.ssrs = _Ssrs;

                    Key _key = new Key();
                    _key.journeyKey = _row["JourneySellKey"].ToString(); // need to fill
                    _key.fareAvailabilityKey = _row["BookingFareID"].ToString();  // need to fill
                    _key.standbyPriorityCode = ""; // need to fill
                    _key.inventoryControl = ""; // need to fill
                    _keys.Add(_key);

                    requestQuote.keys = _keys;

                    //------- passanger  details
                    List<Type> _types = new List<Type>();
                    Type _type = new Type();
                    _type.type = "ADT";
                    _type.count = Convert.ToInt16((_row["Adt"] == null) ? 0 : _row["Adt"]);
                    _types.Add(_type);

                    _type = new Type();
                    _type.type = "CHD";
                    _type.count = Convert.ToInt16((_row["Chd"] == null) ? 0 : _row["Chd"]);
                    if (_type.count > 0)
                    {
                        _types.Add(_type);
                    }

                    Passengers4Quot _Passengers4Quot = new Passengers4Quot();
                    _Passengers4Quot.types = _types;
                    _Passengers4Quot.residentCountry = "IN";

                    requestQuote.passengers = _Passengers4Quot;
                    requestQuote.currencyCode = "INR";


                }
                /* CommonComponents.FlightDBOperation flightDBOperation = new CommonComponents.FlightDBOperation("FTI");
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
                 */
                jsonFlightRQ = Newtonsoft.Json.JsonConvert.SerializeObject(requestQuote);                            //CommonComponents.SerializeDeserialize.SerializeInJsonString(objFlightRQ, "FlightRQ");
            }
            catch (Exception ex)
            {
                //DBCommon.Logger.dbLogg("", 0, "GetFlightFareRulesRQ-TI_FARE_SSR", jsonFlightRQ, Idx, SearchID, ex.Message);
            }
            return jsonFlightRQ;
        }

        

        public string GetFlightFareRulesRQ_V4(string SearchID, DataTable dtBound)
        {
            string jsonFlightRQ = "";
            int i = 0;
            string Idx = "";
            try
            {
                //string[] splitIdx = Idx.Split('_');
                string flightJson = string.Empty;
                string searchJson = string.Empty;
                string fareruleJson = string.Empty;


                RequestQuoteV4 requestQuote = new RequestQuoteV4();
                foreach (DataRow _row in dtBound.Rows)
                {
                    Idx = _row["API_AirlineID"].ToString();
                    string[] splitIdx = Idx.Split('_');

                    List<Key> _keys = new List<Key>();
                    Key _key = new Key();
                    _key.journeyKey = _row["JourneySellKey"].ToString(); // need to fill
                    _key.fareAvailabilityKey = _row["BookingFareID"].ToString();  // need to fill
                    _key.standbyPriorityCode = ""; // need to fill
                    _key.inventoryControl = ""; // need to fill
                    _keys.Add(_key);

                    requestQuote.keys = _keys;

                    //------- passanger  details
                    Passengers4Quot _Passengers4Quot = new Passengers4Quot();
                    List<Type> _types = new List<Type>();
                    Type _type = new Type();
                    _type.type = "ADT";
                    _type.count = Convert.ToInt16((_row["Adt"] == null) ? 0 : _row["Adt"]);
                    _types.Add(_type);


                        _type = new Type();
                        _type.type = "CHD";
                        _type.count = Convert.ToInt16((_row["Chd"] == null) ? 0 : _row["Chd"]);
                    if (_type.count > 0)
                    {
                        _types.Add(_type);
                    }
                    
                    _Passengers4Quot.types = _types;
                    _Passengers4Quot.residentCountry = "IN";

                    requestQuote.passengers = _Passengers4Quot;
                    requestQuote.currencyCode = "INR";

                }
                jsonFlightRQ = Newtonsoft.Json.JsonConvert.SerializeObject(requestQuote);                            //CommonComponents.SerializeDeserialize.SerializeInJsonString(objFlightRQ, "FlightRQ");
            }
            catch (Exception ex)
            {
               // DBCommon.Logger.dbLogg("", 0, "GetFlightFareRulesRQ-TI_FARE_SSR", jsonFlightRQ, Idx, SearchID, ex.Message);
            }
            return jsonFlightRQ;
        }

        public string GetFlightFareRulesRQ_RTF(string SearchID, DataTable dtBound,out string _JornyKey,out string _SegmantKey )
        {
            string jsonFlightRQ = "";
            //int i = 0;
            //string Idx = "";
            _JornyKey = "";
            _SegmantKey = "";

            try
            {
                //string[] splitIdx = Idx.Split('_');
                string flightJson = string.Empty;
                string searchJson = string.Empty;
                string fareruleJson = string.Empty;


                RequestQuoteRTF requestQuote = new RequestQuoteRTF();
                foreach (DataRow _row in dtBound.Rows)
                {
                    _JornyKey= _row["JourneySellKey"].ToString();
                    _SegmantKey= _row["JourneySellKey"].ToString();

                    requestQuote.fareAvailabilityKey= _row["BookingFareID"].ToString();

                    //------- passanger  details
                    passengerPriceType _passengerPriceType = new passengerPriceType();
                    _passengerPriceType.passengerType = "ADT";
                    _passengerPriceType.passengerDiscountCode = null;
                    _passengerPriceType.passengerCount = Convert.ToInt16((_row["Adt"] == null) ? 0 : _row["Adt"]);
                    requestQuote.passengerPriceTypes.Add(_passengerPriceType);

                     _passengerPriceType = new passengerPriceType();
                    _passengerPriceType.passengerType = "CHD";
                    _passengerPriceType.passengerDiscountCode = null;
                    _passengerPriceType.passengerCount = Convert.ToInt16((_row["Chd"] == null) ? 0 : _row["Chd"]);
                    if (_passengerPriceType.passengerCount > 0)
                    {
                        requestQuote.passengerPriceTypes.Add(_passengerPriceType);
                    }
                    _passengerPriceType = new passengerPriceType();
                    _passengerPriceType.passengerType = "INFT";
                    _passengerPriceType.passengerDiscountCode = null;
                    _passengerPriceType.passengerCount = Convert.ToInt16((_row["Inf"] == null) ? 0 : _row["Inf"]);
                    if (_passengerPriceType.passengerCount > 0)
                    {
                        requestQuote.passengerPriceTypes.Add(_passengerPriceType);
                    }

                }
                jsonFlightRQ = Newtonsoft.Json.JsonConvert.SerializeObject(requestQuote);                            //CommonComponents.SerializeDeserialize.SerializeInJsonString(objFlightRQ, "FlightRQ");
            }
            catch (Exception ex)
            {
                //DBCommon.Logger.dbLogg("", 0, "GetFlightFareRulesRQ-TI_FARE_SSR", jsonFlightRQ, "", SearchID, ex.Message);
            }
            return jsonFlightRQ;
        }

    }
}