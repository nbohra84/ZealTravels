//using air_spicejet;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
/// <summary>
/// Summary description for ConverterV4
/// </summary>
/// 
namespace ZealTravel.Infrastructure.Akaasha {

    public class ConverterV4
    {

        public string errorMessage;
        //public DataTable dtOutbound;
        //public DataTable dtInbound;
        //public DataTable dtCombine;
        //-----------------------------------------------------------------------------------------------
        private string Searchid;
        private string Companyid;
        private string Supplierid;
        private string PriceType;
        private string Signature;
        private string Origin;
        private string Destination;
        private string Sector;
        private int Adt;
        private int Chd;
        private int Inf;
        private string RefundType;
        //-----------------------------------------------------------------------------------------------
        /*public ConverterV4(string Searchid, string Companyid, string AirlineID, string Signature, string PriceType, string JourneyType,  string Sector)

        {
            this.Searchid = Searchid;
            this.Companyid = Companyid;
            this.Supplierid = AirlineID;
            this.Signature = Signature;

            this.PriceType = "QP";

            if (PriceType.Equals("LCC"))
            {
                this.RefundType = "Y";
                this.PriceType = "PUB";
            }


            this.Sector = Sector;

        }
        public DataTable FlightModifier(ResponseV4 objSearchResult, string FltType,string AirRQ)
        {

            DataSet ResponseDs = new DataSet();
            ResponseDs.ReadXml(new System.IO.StringReader(AirRQ));
            this.Origin = ResponseDs.Tables["AvailabilityRequest"].Rows[0]["DepartureStation"].ToString();
            this.Destination = ResponseDs.Tables["AvailabilityRequest"].Rows[0]["ArrivalStation"].ToString();
            this.Adt = Convert.ToInt16(ResponseDs.Tables["AvailabilityRequest"].Rows[0]["Adult"].ToString());
            this.Chd = Convert.ToInt16(ResponseDs.Tables["AvailabilityRequest"].Rows[0]["Child"].ToString());
            this.Inf = Convert.ToInt16(ResponseDs.Tables["AvailabilityRequest"].Rows[0]["Infant"].ToString());
            FltType = ResponseDs.Tables["AvailabilityRequest"].Rows[0]["Cabin"].ToString();


            //svc_booking.Journey[] journey ;
            DataTable dtFlights = DBCommon.Schema.SchemaFlights;


            try
            {
                int Rowid = 1;
                int Refid = 6000;

                GetCommonFunctions objcf = new GetCommonFunctions();


                int OrderNo = 0;

                //for (int i = 0; i < journey.Length; i++)

                foreach (var journey in objSearchResult.Data.Results.FirstOrDefault().Trips.FirstOrDefault().JourneysAvailableByMarket.Values.FirstOrDefault())
                {
                    //List<Journey> _JourneyLists = new List<Journey>();
                    //_JourneyLists = objFlt.JourneysAvailableByMarket.Value;


                    string JourneySellKey = journey.JourneyKey;   //journey[i].JourneySellKey.Trim();
                     //svc_booking.Segment[] SegmentCs = journey[i].Segments;
                    int SegmentCount = journey.Segments.Count();

                    if (SegmentCount.Equals(1))
                    {
                        //svc_booking.Fare[] FareCs = objSearchResult.Data.FaresAvailable;  // SegmentCs[0].Fares;

                        var _FareAvailabilityKey = ""; // journey.Fares.First().FareAvailabilityKey;
                        var _FaresAvailable = objSearchResult.Data.FaresAvailable;                                //var varPriceInfo = objFlt.PriceInfo.Where(x => x.Origin == objRSeg.Dep && x.Destination == objRSeg.Arr && x.PaxType == "ADT").FirstOrDefault();
                        //var FareCs = _FaresAvailable.Values.Where(x => x.FareAvailabilityKey == _FareAvailabilityKey).FirstOrDefault().Fares;


                        //int FareCount = FareCs.Count();

                        //for (int j = 0; j < FareCount; j++)
                        foreach (var _fare1 in journey.Fares)
                        {
                            var _fare = _FaresAvailable[_fare1.FareAvailabilityKey].Fares.FirstOrDefault();
                            _FareAvailabilityKey = _fare1.FareAvailabilityKey;

                            OrderNo = 1;
                            Refid++;
                            var _segment = journey.Segments[0]; // .Where(x => x.SegmentKey == _FareAvailabilityKey).FirstOrDefault();
                            //svc_booking.Leg[] LegCs = SegmentCs[0].Legs;
                            //int LegCount = LegCs.Length;
                            //for (int n = 0; n < LegCount; n++)
                            foreach (var _leg in _segment.Legs)
                            {
                                DataRow drAdd = dtFlights.NewRow();
                                drAdd["Sector"] = Sector;
                                drAdd["Rowid"] = Rowid;
                                drAdd["RefID"] = Refid;
                                drAdd["OrderNo"] = OrderNo;
                                //drAdd["Flightid"] = FltType + Supplierid + Refid;
                                drAdd["AirlineID"] = Supplierid;
                                //drAdd["ContractVersion"] = ContractVersion;

                                drAdd["DepartureTerminal"] = _leg.LegInfo.DepartureTerminal;  //LegCs[n].LegInfo.DepartureTerminal.Trim();
                                drAdd["ArrivalTerminal"] = _leg.LegInfo.ArrivalTerminal; //LegCs[n].LegInfo.ArrivalTerminal.Trim();
                                drAdd["DepartureStation"] = _leg.Designator.Origin;   //LegCs[n].DepartureStation.Trim();
                                drAdd["ArrivalStation"] = _leg.Designator.Arrival; // LegCs[n].ArrivalStation.Trim();
                                drAdd["CarrierCode"] = _segment.Identifier.CarrierCode; //LegCs[n].FlightDesignator.CarrierCode.Trim();
                                //drAdd["CarrierName"] = "Goair";
                                drAdd["FlightNumber"] = _segment.Identifier.identifier;  //LegCs[n].FlightDesignator.FlightNumber.Trim();

                                drAdd["DepartureDate"] = objcf.DateFormat(_leg.Designator.Departure);     //objcf.DateFormat(LegCs[n].STD);
                                drAdd["ArrivalDate"] = objcf.DateFormat(_leg.Designator.Arrival);  //objcf.DateFormat(LegCs[n].STA);
                                drAdd["DepartureTime"] = objcf.TimeFormat(_leg.LegInfo.DepartureTime);                     //objcf.TimeFormat(LegCs[n].STD);
                                drAdd["ArrivalTime"] = objcf.TimeFormat(_leg.LegInfo.ArrivalTime);               //objcf.TimeFormat(LegCs[n].STA);

                                drAdd["DepDate"] = _leg.Designator.Departure.ToString();   // LegCs[n].STD.ToString().Trim();
                                drAdd["ArrDate"] = _leg.Designator.Arrival.ToString();  // LegCs[n].STA.ToString().Trim();
                                drAdd["DepTime"] = _leg.LegInfo.DepartureTime.ToString();    // LegCs[n].STD.ToString().Trim();
                                drAdd["ArrTime"] = _leg.LegInfo.ArrivalTime.ToString();   // LegCs[n].STA.ToString().Trim();


                                drAdd["EquipmentType"] = _leg.LegInfo.EquipmentType;  // LegCs[n].LegInfo.EquipmentType.Trim();
                                drAdd["JourneySellKey"] = _FareAvailabilityKey;  //   //JourneySellKey.Trim();
                                drAdd["ClassOfService"] = _leg.LegInfo.ScheduleServiceType;   //   FareCs[j].ClassOfService.Trim();
                                drAdd["FareSellKey"] = _FareAvailabilityKey;      //FareCs[j].FareSellKey.Trim();
                                drAdd["FareBasisCode"] = _fare.FareBasisCode;    //FareCs[j].FareBasisCode.Trim();
                                drAdd["RuleNumber"] = _fare.RuleNumber;   //FareCs[j].RuleNumber.Trim();
                                drAdd["RuleTarrif"] = _fare.RuleTariff;   //FareCs[j].RuleTariff.Trim();
                                drAdd["ProductClass"] = _fare.ProductClass;      //FareCs[j].ProductClass.Trim();

                                drAdd["Cabin"] = _fare.ProductClass;            //FareCs[j].ProductClass.Trim();
                                drAdd["PriceType"] = PriceType.Trim();

                                drAdd["SeatsAvailable"] = _segment.IsSeatmapViewable;  // ((svc_booking.AvailableFare)(FareCs[j])).AvailableCount.ToString().Trim();

                                drAdd["FltType"] = FltType;
                                drAdd["Api_SessionID"] = Signature;
                                drAdd["RefundType"] = RefundType;

                                drAdd["Adt"] = Adt;
                                drAdd["Chd"] = Chd;
                                drAdd["Inf"] = Inf;


                                drAdd["Origin"] = Origin;
                                drAdd["Destination"] = Destination;


                                dtFlights.Rows.Add(drAdd);
                                Rowid++;
                                OrderNo++;
                            }
                        }
                    }
                    else if (SegmentCount.Equals(2))
                    {
                        string _FareAvailabilityKey1 = journey.Segments[0].SegmentKey;
                        string _FareAvailabilityKey2 = journey.Segments[1].SegmentKey;

                        var _FaresAvailable = objSearchResult.Data.FaresAvailable;                                //var varPriceInfo = objFlt.PriceInfo.Where(x => x.Origin == objRSeg.Dep && x.Destination == objRSeg.Arr && x.PaxType == "ADT").FirstOrDefault();
                        //var FareCs1 = _FaresAvailable.Values.Where(x => x.FareAvailabilityKey == _FareAvailabilityKey1).FirstOrDefault().Fares;
                        //var FareCs2 = _FaresAvailable.Where(x => x.Key == _FareAvailabilityKey2).FirstOrDefault().Value.Fares;

                        //svc_booking.Fare[] FareCs1 = SegmentCs[0].Fares;
                        //svc_booking.Fare[] FareCs2 = SegmentCs[1].Fares;
                        //int FareCount1 = FareCs1.Count();
                        //int FareCount2 = FareCs2.Count();

                        //for (int j = 0; j < FareCount1; j++)
                        foreach (var _fare1 in journey.Fares)
                        {
                            var _fare = _FaresAvailable[_fare1.FareAvailabilityKey].Fares.FirstOrDefault();
                            _FareAvailabilityKey1 = _fare1.FareAvailabilityKey;

                            OrderNo = 1;
                            Refid++;
                            string FareSellKey = _FareAvailabilityKey1 + "^" + _FareAvailabilityKey2;   // FareCs1[j].FareSellKey.Trim() + "^" + FareCs2[j].FareSellKey.Trim();
                            //svc_booking.Leg[] LegCs1 = SegmentCs[0].Legs;
                            //int LegCount1 = LegCs1.Length;
                            var _segment = journey.Segments[0]; // .Where(x => x.SegmentKey == _FareAvailabilityKey1).FirstOrDefault();
                            //for (int n = 0; n < LegCount1; n++)
                            foreach (var _leg in _segment.Legs)
                            {
                                DataRow drAdd = dtFlights.NewRow();
                                drAdd["Sector"] = Sector;
                                drAdd["Rowid"] = Rowid;
                                drAdd["RefID"] = Refid;
                                drAdd["OrderNo"] = OrderNo;
                                //drAdd["Flightid"] = FltType + Supplierid + Refid;
                                drAdd["AirlineID"] = Supplierid;
                                //drAdd["ContractVersion"] = ContractVersion;


                                drAdd["DepartureTerminal"] = _leg.LegInfo.DepartureTerminal;  //LegCs[n].LegInfo.DepartureTerminal.Trim();
                                drAdd["ArrivalTerminal"] = _leg.LegInfo.ArrivalTerminal; //LegCs[n].LegInfo.ArrivalTerminal.Trim();
                                drAdd["DepartureStation"] = _leg.Designator.Origin;   //LegCs[n].DepartureStation.Trim();
                                drAdd["ArrivalStation"] = _leg.Designator.Arrival; // LegCs[n].ArrivalStation.Trim();
                                drAdd["CarrierCode"] = _segment.Identifier.CarrierCode; //LegCs[n].FlightDesignator.CarrierCode.Trim();
                                //drAdd["CarrierName"] = "Goair";
                                drAdd["FlightNumber"] = _segment.Identifier.identifier;  //LegCs[n].FlightDesignator.FlightNumber.Trim();


                                drAdd["DepartureDate"] = objcf.DateFormat(_leg.Designator.Departure);     //objcf.DateFormat(LegCs[n].STD);
                                drAdd["ArrivalDate"] = objcf.DateFormat(_leg.Designator.Arrival);  //objcf.DateFormat(LegCs[n].STA);
                                drAdd["DepartureTime"] = objcf.TimeFormat(_leg.LegInfo.DepartureTime);                     //objcf.TimeFormat(LegCs[n].STD);
                                drAdd["ArrivalTime"] = objcf.TimeFormat(_leg.LegInfo.ArrivalTime);               //objcf.TimeFormat(LegCs[n].STA);

                                drAdd["DepDate"] = _leg.Designator.Departure.ToString();   // LegCs[n].STD.ToString().Trim();
                                drAdd["ArrDate"] = _leg.Designator.Arrival.ToString();  // LegCs[n].STA.ToString().Trim();
                                drAdd["DepTime"] = _leg.LegInfo.DepartureTime.ToString();    // LegCs[n].STD.ToString().Trim();
                                drAdd["ArrTime"] = _leg.LegInfo.ArrivalTime.ToString();   // LegCs[n].STA.ToString().Trim();


                                //drAdd["Origin"] = Origin;
                                //drAdd["Destination"] = Destination;

                                drAdd["EquipmentType"] = _leg.LegInfo.EquipmentType;  // LegCs[n].LegInfo.EquipmentType.Trim();
                                drAdd["JourneySellKey"] = _FareAvailabilityKey1;  //   //JourneySellKey.Trim();
                                drAdd["ClassOfService"] = _leg.LegInfo.ScheduleServiceType;   //   FareCs[j].ClassOfService.Trim();
                                drAdd["FareSellKey"] = _FareAvailabilityKey1;      //FareCs[j].FareSellKey.Trim();
                                drAdd["FareBasisCode"] = _fare.FareBasisCode;    //FareCs[j].FareBasisCode.Trim();
                                drAdd["RuleNumber"] = _fare.RuleNumber;   //FareCs[j].RuleNumber.Trim();
                                drAdd["RuleTarrif"] = _fare.RuleTariff;   //FareCs[j].RuleTariff.Trim();
                                drAdd["ProductClass"] = _fare.ProductClass;      //FareCs[j].ProductClass.Trim();

                                drAdd["Cabin"] = _fare.ProductClass;            //FareCs[j].ProductClass.Trim();
                                drAdd["PriceType"] = PriceType.Trim();

                                drAdd["SeatsAvailable"] = _segment.IsSeatmapViewable;  // ((svc_booking.AvailableFare)(FareCs[j])).AvailableCount.ToString().Trim();

                                drAdd["FltType"] = FltType;
                                drAdd["Api_SessionID"] = Signature;
                                drAdd["RefundType"] = RefundType;

                                drAdd["Adt"] = Adt;
                                drAdd["Chd"] = Chd;
                                drAdd["Inf"] = Inf;


                                drAdd["Origin"] = Origin;
                                drAdd["Destination"] = Destination;


                                dtFlights.Rows.Add(drAdd);
                                Rowid++;
                                OrderNo++;
                            }
                        }
                        //svc_booking.Leg[] LegCs2 = SegmentCs[1].Legs;
                        //int LegCount2 = LegCs2.Length;
                        //for (int n = 0; n < LegCount2; n++)

                        var FareCs2 = _FaresAvailable.Values.Where(x => x.FareAvailabilityKey == _FareAvailabilityKey2).FirstOrDefault().Fares;
                        foreach (var _fare in FareCs2)
                        {
                            var _segment = journey.Segments[1]; //.Where(x => x.SegmentKey == _FareAvailabilityKey2).FirstOrDefault();

                            foreach (var _leg in _segment.Legs)
                            {
                                DataRow drAdd = dtFlights.NewRow();
                                drAdd["Sector"] = Sector;
                                drAdd["Rowid"] = Rowid;
                                drAdd["RefID"] = Refid;
                                drAdd["OrderNo"] = OrderNo;
                                //drAdd["Flightid"] = FltType + Supplierid + Refid;
                                drAdd["AirlineID"] = Supplierid;
                                //drAdd["ContractVersion"] = ContractVersion;

                                drAdd["DepartureTerminal"] = _leg.LegInfo.DepartureTerminal; // LegCs2[n].LegInfo.DepartureTerminal.Trim();
                                drAdd["ArrivalTerminal"] = _leg.LegInfo.ArrivalTerminal; // LegCs2[n].LegInfo.ArrivalTerminal.Trim();
                                drAdd["DepartureStation"] = _leg.Designator.Origin;   //LegCs2[n].DepartureStation.Trim();
                                drAdd["ArrivalStation"] = _leg.Designator.Arrival;  // LegCs2[n].ArrivalStation.Trim();
                                drAdd["CarrierCode"] = _segment.Identifier.CarrierCode;  //LegCs2[n].FlightDesignator.CarrierCode.Trim();
                                //drAdd["CarrierName"] = "Goair";
                                drAdd["FlightNumber"] = _segment.Identifier.identifier; // LegCs2[n].FlightDesignator.FlightNumber.Trim();

                                drAdd["DepartureDate"] = objcf.DateFormat(_leg.Designator.Departure);  //objcf.DateFormat(LegCs2[n].STD);
                                drAdd["ArrivalDate"] = objcf.DateFormat(_leg.Designator.Arrival);  //objcf.DateFormat(LegCs2[n].STA);
                                drAdd["DepartureTime"] = objcf.TimeFormat(_leg.LegInfo.DepartureTime); // objcf.TimeFormat(LegCs2[n].STD);
                                drAdd["ArrivalTime"] = objcf.TimeFormat(_leg.LegInfo.ArrivalTime);  //objcf.TimeFormat(LegCs2[n].STA);


                                drAdd["DepDate"] = _leg.Designator.Departure.ToString(); // LegCs2[n].STD.ToString().Trim();
                                drAdd["ArrDate"] = _leg.Designator.Arrival.ToString();  //LegCs2[n].STA.ToString().Trim();
                                drAdd["DepTime"] = _leg.LegInfo.DepartureTime.ToString();  //LegCs2[n].STD.ToString().Trim();
                                drAdd["ArrTime"] = _leg.LegInfo.ArrivalTime.ToString();  //LegCs2[n].STA.ToString().Trim();



                                drAdd["EquipmentType"] = _leg.LegInfo.EquipmentType; // LegCs2[n].LegInfo.EquipmentType.Trim();
                                drAdd["JourneySellKey"] = _FareAvailabilityKey2; // JourneySellKey.Trim();
                                drAdd["ClassOfService"] = _leg.LegInfo.ScheduleServiceType; // FareCs2[j].ClassOfService.Trim();
                                drAdd["FareSellKey"] = _FareAvailabilityKey2;  //FareSellKey.Trim();
                                drAdd["FareBasisCode"] = _fare.FareBasisCode; // FareCs2[j].FareBasisCode.Trim();
                                drAdd["RuleNumber"] = _fare.RuleNumber;  //FareCs2[j].RuleNumber.Trim();
                                drAdd["RuleTarrif"] = _fare.RuleTariff;  //FareCs2[j].RuleTariff.Trim();
                                drAdd["ProductClass"] = _fare.ProductClass; // FareCs2[j].ProductClass.Trim();



                                drAdd["Cabin"] = _fare.ProductClass;  //FareCs2[j].ProductClass.Trim();
                                drAdd["PriceType"] = PriceType;

                                drAdd["SeatsAvailable"] = _segment.IsSeatmapViewable;  //((svc_booking.AvailableFare)(FareCs2[j])).AvailableCount.ToString().Trim();


                                drAdd["FltType"] = FltType;
                                drAdd["Api_SessionID"] = Signature;
                                drAdd["RefundType"] = RefundType;

                                drAdd["Adt"] = Adt;
                                drAdd["Chd"] = Chd;
                                drAdd["Inf"] = Inf;


                                drAdd["Origin"] = Origin;
                                drAdd["Destination"] = Destination;


                                dtFlights.Rows.Add(drAdd);
                                Rowid++;
                                OrderNo++;
                            }
                        }
                    }
                    else if (SegmentCount.Equals(3))
                    {

                        string _FareAvailabilityKey1 = journey.Segments[0].SegmentKey;
                        string _FareAvailabilityKey2 = journey.Segments[1].SegmentKey;
                        string _FareAvailabilityKey3 = journey.Segments[1].SegmentKey;

                        var _FaresAvailable = objSearchResult.Data.FaresAvailable;

                        var FareCs1 = _FaresAvailable.Values.Where(x => x.FareAvailabilityKey == _FareAvailabilityKey1).FirstOrDefault().Fares;

                        //svc_booking.Fare[] FareCs1 = SegmentCs[0].Fares;
                        //svc_booking.Fare[] FareCs2 = SegmentCs[1].Fares;
                        //svc_booking.Fare[] FareCs3 = SegmentCs[2].Fares;
                        //int FareCount1 = FareCs1.Length;
                        //int FareCount2 = FareCs2.Length;
                        //int FareCount3 = FareCs3.Length;

                        //for (int j = 0; j < FareCount1; j++)
                        foreach (var _fare in FareCs1)
                        {
                            OrderNo = 1;
                            Refid++;
                            string FareSellKey = _FareAvailabilityKey1 + "^" + _FareAvailabilityKey2 + "^" + _FareAvailabilityKey3;
                            //string FareSellKey = FareCs1[j].FareSellKey.Trim() + "^" + FareCs2[j].FareSellKey.Trim() + "^" + FareCs3[j].FareSellKey.Trim();
                            //svc_booking.Leg[] LegCs1 = SegmentCs[0].Legs;
                            //int LegCount1 = LegCs1.Length;
                            //for (int n = 0; n < LegCount1; n++)
                            var _segment = journey.Segments[0]; //.Where(x => x.SegmentKey == _FareAvailabilityKey1).FirstOrDefault();
                            //for (int n = 0; n < LegCount1; n++)
                            foreach (var _leg in _segment.Legs)
                            {
                                DataRow drAdd = dtFlights.NewRow();
                                drAdd["Sector"] = Sector;
                                drAdd["Rowid"] = Rowid;
                                drAdd["RefID"] = Refid;
                                drAdd["OrderNo"] = OrderNo;
                                //drAdd["Flightid"] = FltType + Supplierid + Refid;
                                drAdd["AirlineID"] = Supplierid;
                                //drAdd["ContractVersion"] = ContractVersion;


                                drAdd["DepartureTerminal"] = _leg.LegInfo.DepartureTerminal;  //LegCs[n].LegInfo.DepartureTerminal.Trim();
                                drAdd["ArrivalTerminal"] = _leg.LegInfo.ArrivalTerminal; //LegCs[n].LegInfo.ArrivalTerminal.Trim();
                                drAdd["DepartureStation"] = _leg.Designator.Origin;   //LegCs[n].DepartureStation.Trim();
                                drAdd["ArrivalStation"] = _leg.Designator.Arrival; // LegCs[n].ArrivalStation.Trim();
                                drAdd["CarrierCode"] = _segment.Identifier.CarrierCode; //LegCs[n].FlightDesignator.CarrierCode.Trim();
                                //drAdd["CarrierName"] = "Goair";
                                drAdd["FlightNumber"] = _segment.Identifier.identifier;  //LegCs[n].FlightDesignator.FlightNumber.Trim();


                                drAdd["DepartureDate"] = objcf.DateFormat(_leg.Designator.Departure);     //objcf.DateFormat(LegCs[n].STD);
                                drAdd["ArrivalDate"] = objcf.DateFormat(_leg.Designator.Arrival);  //objcf.DateFormat(LegCs[n].STA);
                                drAdd["DepartureTime"] = objcf.TimeFormat(_leg.LegInfo.DepartureTime);                     //objcf.TimeFormat(LegCs[n].STD);
                                drAdd["ArrivalTime"] = objcf.TimeFormat(_leg.LegInfo.ArrivalTime);               //objcf.TimeFormat(LegCs[n].STA);

                                drAdd["DepDate"] = _leg.Designator.Departure.ToString();   // LegCs[n].STD.ToString().Trim();
                                drAdd["ArrDate"] = _leg.Designator.Arrival.ToString();  // LegCs[n].STA.ToString().Trim();
                                drAdd["DepTime"] = _leg.LegInfo.DepartureTime.ToString();    // LegCs[n].STD.ToString().Trim();
                                drAdd["ArrTime"] = _leg.LegInfo.ArrivalTime.ToString();   // LegCs[n].STA.ToString().Trim();


                                //drAdd["Origin"] = Origin;
                                //drAdd["Destination"] = Destination;

                                drAdd["EquipmentType"] = _leg.LegInfo.EquipmentType;  // LegCs[n].LegInfo.EquipmentType.Trim();
                                drAdd["JourneySellKey"] = _FareAvailabilityKey1;  //   //JourneySellKey.Trim();
                                drAdd["ClassOfService"] = _leg.LegInfo.ScheduleServiceType;   //   FareCs[j].ClassOfService.Trim();
                                drAdd["FareSellKey"] = _FareAvailabilityKey1;      //FareCs[j].FareSellKey.Trim();
                                drAdd["FareBasisCode"] = _fare.FareBasisCode;    //FareCs[j].FareBasisCode.Trim();
                                drAdd["RuleNumber"] = _fare.RuleNumber;   //FareCs[j].RuleNumber.Trim();
                                drAdd["RuleTarrif"] = _fare.RuleTariff;   //FareCs[j].RuleTariff.Trim();
                                drAdd["ProductClass"] = _fare.ProductClass;      //FareCs[j].ProductClass.Trim();

                                drAdd["Cabin"] = _fare.ProductClass;            //FareCs[j].ProductClass.Trim();
                                drAdd["PriceType"] = PriceType.Trim();

                                drAdd["SeatsAvailable"] = _segment.IsSeatmapViewable;  // ((svc_booking.AvailableFare)(FareCs[j])).AvailableCount.ToString().Trim();

                                drAdd["FltType"] = FltType;
                                drAdd["Api_SessionID"] = Signature;
                                drAdd["RefundType"] = RefundType;

                                drAdd["Adt"] = Adt;
                                drAdd["Chd"] = Chd;
                                drAdd["Inf"] = Inf;


                                drAdd["Origin"] = Origin;
                                drAdd["Destination"] = Destination;

                                dtFlights.Rows.Add(drAdd);
                                Rowid++;
                                OrderNo++;
                            }

                        }


                        var FareCs2 = _FaresAvailable.Values.Where(x => x.FareAvailabilityKey == _FareAvailabilityKey2).FirstOrDefault().Fares;
                        foreach (var _fare in FareCs2)
                        {
                            var _segment = journey.Segments[1];   //.Where(x => x.SegmentKey == _FareAvailabilityKey2).FirstOrDefault();

                            foreach (var _leg in _segment.Legs)
                            {
                                DataRow drAdd = dtFlights.NewRow();
                                drAdd["Sector"] = Sector;
                                drAdd["Rowid"] = Rowid;
                                drAdd["RefID"] = Refid;
                                drAdd["OrderNo"] = OrderNo;
                                //drAdd["Flightid"] = FltType + Supplierid + Refid;
                                drAdd["AirlineID"] = Supplierid;
                                //drAdd["ContractVersion"] = ContractVersion;

                                drAdd["DepartureTerminal"] = _leg.LegInfo.DepartureTerminal; // LegCs2[n].LegInfo.DepartureTerminal.Trim();
                                drAdd["ArrivalTerminal"] = _leg.LegInfo.ArrivalTerminal; // LegCs2[n].LegInfo.ArrivalTerminal.Trim();
                                drAdd["DepartureStation"] = _leg.Designator.Origin;   //LegCs2[n].DepartureStation.Trim();
                                drAdd["ArrivalStation"] = _leg.Designator.Arrival;  // LegCs2[n].ArrivalStation.Trim();
                                drAdd["CarrierCode"] = _segment.Identifier.CarrierCode;  //LegCs2[n].FlightDesignator.CarrierCode.Trim();
                                //drAdd["CarrierName"] = "Goair";
                                drAdd["FlightNumber"] = _segment.Identifier.identifier; // LegCs2[n].FlightDesignator.FlightNumber.Trim();

                                drAdd["DepartureDate"] = objcf.DateFormat(_leg.Designator.Departure);  //objcf.DateFormat(LegCs2[n].STD);
                                drAdd["ArrivalDate"] = objcf.DateFormat(_leg.Designator.Arrival);  //objcf.DateFormat(LegCs2[n].STA);
                                drAdd["DepartureTime"] = objcf.TimeFormat(_leg.LegInfo.DepartureTime); // objcf.TimeFormat(LegCs2[n].STD);
                                drAdd["ArrivalTime"] = objcf.TimeFormat(_leg.LegInfo.ArrivalTime);  //objcf.TimeFormat(LegCs2[n].STA);


                                drAdd["DepDate"] = _leg.Designator.Departure.ToString(); // LegCs2[n].STD.ToString().Trim();
                                drAdd["ArrDate"] = _leg.Designator.Arrival.ToString();  //LegCs2[n].STA.ToString().Trim();
                                drAdd["DepTime"] = _leg.LegInfo.DepartureTime.ToString();  //LegCs2[n].STD.ToString().Trim();
                                drAdd["ArrTime"] = _leg.LegInfo.ArrivalTime.ToString();  //LegCs2[n].STA.ToString().Trim();



                                drAdd["EquipmentType"] = _leg.LegInfo.EquipmentType; // LegCs2[n].LegInfo.EquipmentType.Trim();
                                drAdd["JourneySellKey"] = _FareAvailabilityKey2; // JourneySellKey.Trim();
                                drAdd["ClassOfService"] = _leg.LegInfo.ScheduleServiceType; // FareCs2[j].ClassOfService.Trim();
                                drAdd["FareSellKey"] = _FareAvailabilityKey2;  //FareSellKey.Trim();
                                drAdd["FareBasisCode"] = _fare.FareBasisCode; // FareCs2[j].FareBasisCode.Trim();
                                drAdd["RuleNumber"] = _fare.RuleNumber;  //FareCs2[j].RuleNumber.Trim();
                                drAdd["RuleTarrif"] = _fare.RuleTariff;  //FareCs2[j].RuleTariff.Trim();
                                drAdd["ProductClass"] = _fare.ProductClass; // FareCs2[j].ProductClass.Trim();



                                drAdd["Cabin"] = _fare.ProductClass;  //FareCs2[j].ProductClass.Trim();
                                drAdd["PriceType"] = PriceType;

                                drAdd["SeatsAvailable"] = _segment.IsSeatmapViewable;  //((svc_booking.AvailableFare)(FareCs2[j])).AvailableCount.ToString().Trim();


                                drAdd["FltType"] = FltType;
                                drAdd["Api_SessionID"] = Signature;
                                drAdd["RefundType"] = RefundType;

                                drAdd["Adt"] = Adt;
                                drAdd["Chd"] = Chd;
                                drAdd["Inf"] = Inf;


                                drAdd["Origin"] = Origin;
                                drAdd["Destination"] = Destination;



                                dtFlights.Rows.Add(drAdd);
                                Rowid++;
                                OrderNo++;
                            }

                        }

                        var FareCs3 = _FaresAvailable.Values.Where(x => x.FareAvailabilityKey == _FareAvailabilityKey3).FirstOrDefault().Fares;

                        foreach (var _fare in FareCs3)
                        {
                            var _segment = journey.Segments[2]; //.Where(x => x.SegmentKey == _FareAvailabilityKey3).FirstOrDefault();

                            foreach (var _leg in _segment.Legs)
                            {

                                DataRow drAdd = dtFlights.NewRow();
                                drAdd["Sector"] = Sector;
                                drAdd["Rowid"] = Rowid;
                                drAdd["RefID"] = Refid;
                                drAdd["OrderNo"] = OrderNo;
                                //drAdd["Flightid"] = FltType + Supplierid + Refid;
                                drAdd["AirlineID"] = Supplierid;
                                //drAdd["ContractVersion"] = ContractVersion;

                                drAdd["DepartureTerminal"] = _leg.LegInfo.DepartureTerminal; // LegCs2[n].LegInfo.DepartureTerminal.Trim();
                                drAdd["ArrivalTerminal"] = _leg.LegInfo.ArrivalTerminal; // LegCs2[n].LegInfo.ArrivalTerminal.Trim();
                                drAdd["DepartureStation"] = _leg.Designator.Origin;   //LegCs2[n].DepartureStation.Trim();
                                drAdd["ArrivalStation"] = _leg.Designator.Arrival;  // LegCs2[n].ArrivalStation.Trim();
                                drAdd["CarrierCode"] = _segment.Identifier.CarrierCode;  //LegCs2[n].FlightDesignator.CarrierCode.Trim();
                                //drAdd["CarrierName"] = "Goair";
                                drAdd["FlightNumber"] = _segment.Identifier.identifier; // LegCs2[n].FlightDesignator.FlightNumber.Trim();

                                drAdd["DepartureDate"] = objcf.DateFormat(_leg.Designator.Departure);  //objcf.DateFormat(LegCs2[n].STD);
                                drAdd["ArrivalDate"] = objcf.DateFormat(_leg.Designator.Arrival);  //objcf.DateFormat(LegCs2[n].STA);
                                drAdd["DepartureTime"] = objcf.TimeFormat(_leg.LegInfo.DepartureTime); // objcf.TimeFormat(LegCs2[n].STD);
                                drAdd["ArrivalTime"] = objcf.TimeFormat(_leg.LegInfo.ArrivalTime);  //objcf.TimeFormat(LegCs2[n].STA);


                                drAdd["DepDate"] = _leg.Designator.Departure.ToString(); // LegCs2[n].STD.ToString().Trim();
                                drAdd["ArrDate"] = _leg.Designator.Arrival.ToString();  //LegCs2[n].STA.ToString().Trim();
                                drAdd["DepTime"] = _leg.LegInfo.DepartureTime.ToString();  //LegCs2[n].STD.ToString().Trim();
                                drAdd["ArrTime"] = _leg.LegInfo.ArrivalTime.ToString();  //LegCs2[n].STA.ToString().Trim();



                                drAdd["EquipmentType"] = _leg.LegInfo.EquipmentType; // LegCs2[n].LegInfo.EquipmentType.Trim();
                                drAdd["JourneySellKey"] = _FareAvailabilityKey3; // JourneySellKey.Trim();
                                drAdd["ClassOfService"] = _leg.LegInfo.ScheduleServiceType; // FareCs2[j].ClassOfService.Trim();
                                drAdd["FareSellKey"] = _FareAvailabilityKey3;  //FareSellKey.Trim();
                                drAdd["FareBasisCode"] = _fare.FareBasisCode; // FareCs2[j].FareBasisCode.Trim();
                                drAdd["RuleNumber"] = _fare.RuleNumber;  //FareCs2[j].RuleNumber.Trim();
                                drAdd["RuleTarrif"] = _fare.RuleTariff;  //FareCs2[j].RuleTariff.Trim();
                                drAdd["ProductClass"] = _fare.ProductClass; // FareCs2[j].ProductClass.Trim();



                                drAdd["Cabin"] = _fare.ProductClass;  //FareCs2[j].ProductClass.Trim();
                                drAdd["PriceType"] = PriceType;

                                drAdd["SeatsAvailable"] = _segment.IsSeatmapViewable;  //((svc_booking.AvailableFare)(FareCs2[j])).AvailableCount.ToString().Trim();


                                drAdd["FltType"] = FltType;
                                drAdd["Api_SessionID"] = Signature;
                                drAdd["RefundType"] = RefundType;

                                drAdd["Adt"] = Adt;
                                drAdd["Chd"] = Chd;
                                drAdd["Inf"] = Inf;


                                drAdd["Origin"] = Origin;
                                drAdd["Destination"] = Destination;


                                dtFlights.Rows.Add(drAdd);
                                Rowid++;
                                OrderNo++;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                DBCommon.Logger.dbLogg(Companyid, 0, "SetAvailabilityResponseModifier", "Akasa_qp", Supplierid, Searchid, ex.Message + "," + ex.StackTrace);
            }
            return dtFlights;
        }*/


    }

}
