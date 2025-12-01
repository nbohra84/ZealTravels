using Newtonsoft.Json;
using System.Collections;
using System.Text.Json.Serialization;
using ZealTravel.Common.Models;

namespace ZealTravel.Front.Web.Models.Flight
{
    public class SearchRequest
    {
        [JsonProperty("DepartureStation")]
        public string DepartureStation { get; set; }
        [JsonProperty("ArrivalStation")]
        public string ArrivalStation { get; set; }
        [JsonProperty("BeginDate ")]
        public string BeginDate { get; set; }
        [JsonProperty("EndDate")]
        public string EndDate { get; set; }
        [JsonProperty("NumberofAdult")]
        public int NumberofAdult{ get; set; }
        [JsonProperty("NumberofChild")]
        public int NumberofChild { get; set; }
        [JsonProperty("NumberofInfant")]
        public int NumberofInfant { get; set; }
        [JsonProperty("SearchType")]
        public string SearchType { get; set; }
        [JsonProperty("PreferredAirlines")]
        public string PreferredAirlines { get; set; }
        [JsonProperty("TravelClass")]
        public string TravelClass { get; set; }
        [JsonProperty("SpecialFare")]
        public string SpecialFare { get; set; }
        [JsonProperty("Place")]
        public string Place { get; set; }
    }

    public class SearchFlightsModel
    {
        [JsonProperty("SearchFlights")]
        public List<SearchRequest> SearchFlights { get; set; }
    }
    public class testModel
    {
        public int Value { get; set; }
    }


    public class clsPSQBORequest
    {
        //[JsonPropertyName("objclsPSQ")]
        public List<clsPSQBO> objclsPSQ { get; set; }
    }
        public class clsPSQBO
    {
        public string _Multisector { get; set; }
        public string _DepartureStation { get; set; }
        public string _ArrivalStation { get; set; }
        public string _BeginDate { get; set; }
        public string _EndDate { get; set; }
        public string _NoOfAdult { get; set; }
        public string _NoOfChild { get; set; }
        public string _NoOfInfant { get; set; }
        public string _SearchType { get; set; }
        public string _SectorType { get; set; }
        public string _PreferedAirlines { get; set; }
        public string _TravelClass { get; set; }
        public string _Place { get; set; }
        public string _Currency { get; set; }
        public int _SpecialFare { get; set; }

        public int _SrNo { get; set; }
        private ArrayList _array = new ArrayList();

        private List<clsPSQBO> _clsPSQBOList = new List<clsPSQBO>();

        public string _companyId { get; set; }

        public ArrayList _AirlineList
        {
            get { return _array; } // do not implement setter as to avoid outside to overwrite the object's array instance.
        }
        public List<clsPSQBO> clsPSQBOList
        {
            get { return _clsPSQBOList; }
        }
    }
    public class SessionResult4MC
    {
        public int _SrNo { get; set; }
        public string FinalResult { get; set; }  // Flight available list
        public string FinalResultFirstList { get; set; }  // this is 4 when goback  from the payment modeule reset the result
        public string SearchID { get; set; }  // system gen ID GUID:  "4c8f547a-e81a-4238-aa79-4c539e430a9f-28-44-0"
        public string SelectedFltOut { get; set; }  // selected flight out list   List<ShowFlightOutBound>
        public string SelectedFltIn { get; set; }  // seelct flight in list List<ShowFlightInBound>
        public string PaxXML { get; set; }
        public string OriginalHotlPSQ { get; set; }
        public string BOOK_HOTEL { get; set; }
        public string hoteldata { get; set; }
        public string hotel { get; set; }
        public string room { get; set; }
        public string block { get; set; }
        public string hotelblock { get; set; }
        public string hotelinfo { get; set; }
        public string paxdetail { get; set; }
        public string addre { get; set; }
        public string BOOK { get; set; }
        public string SearchValue { get; set; }   // OW : Onwway   , Two way etc
        public string Guest { get; set; }
        //public string OriginalPSQ { get; set; }
        public string StopCheck { get; set; }


        public string PSQ { get; set; }  // XML File of request with Flight code and Guest totals  AvailabilityRequest

        public clsPSQBORequest OriginalPSQ { get; set; }
        public List<k_ShowFlightOutBound> ShowFlightOutBoundList { get; set; }

        public SessionResult4MC()
        {
            OriginalPSQ = new clsPSQBORequest();
            ShowFlightOutBoundList = new List<k_ShowFlightOutBound>();
        }

    }

}
