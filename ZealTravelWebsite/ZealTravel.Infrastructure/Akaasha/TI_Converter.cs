using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using TIRequestResponse;
using ZealTravel.Infrastructure.Akaasa;

namespace ZealTravel.Infrastructure.Akaasha
{
    public class TI_Converter
    {
        //=========== new API Res V2
        
        public DataTable TISearch_2_Common(string SearchID, string CompanyID, SearchRsQpV2 objSearchResult, string JourneyType, string Sector, string AirRQ, string AirlineID)
        {
            DataTable dtBound = Common.Schema.SchemaFlights;
            try
            {
                DataSet ResponseDs = new DataSet();
                ResponseDs.ReadXml(new System.IO.StringReader(AirRQ));
                string SRC = ResponseDs.Tables["AvailabilityRequest"].Rows[0]["DepartureStation"].ToString();
                string DES = ResponseDs.Tables["AvailabilityRequest"].Rows[0]["ArrivalStation"].ToString();
                Int32 Adults = Convert.ToInt16(ResponseDs.Tables["AvailabilityRequest"].Rows[0]["Adult"].ToString());
                Int32 Childs = Convert.ToInt16(ResponseDs.Tables["AvailabilityRequest"].Rows[0]["Child"].ToString());
                Int32 Infants = Convert.ToInt16(ResponseDs.Tables["AvailabilityRequest"].Rows[0]["Infant"].ToString());
                string Cabin = ResponseDs.Tables["AvailabilityRequest"].Rows[0]["Cabin"].ToString();

                bool VerifyRSeg = false;

                int RowID = 1;
                Cabin = Cabin == "Y" ? "Economy" : Cabin == "A" ? "PremiumEconomy" : Cabin == "C" ? "Business" : Cabin == "B" ? "FirstClass" : "EC";
                foreach (var objFlt in objSearchResult.Data.Results.FirstOrDefault().Trips)    //objSearchResult.Airlines.Flight
                    {
                        if (JourneyType.Equals("RT") && objSearchResult.Data.Results.FirstOrDefault().Trips.Count().Equals(2))   //objFlt.Details.RSeg.Count()
                        {
                            VerifyRSeg = true;
                        }
                        else if ((JourneyType.Equals("OW") || JourneyType.Equals("RW")) && objSearchResult.Data.Results.FirstOrDefault().Trips.Count().Equals(1))
                        {
                            VerifyRSeg = true;
                        }
                        else if (JourneyType.Equals("MC") && objSearchResult.Data.Results.FirstOrDefault().Trips.Count() > 2)
                        {
                            VerifyRSeg = true;
                        }

                        if (VerifyRSeg)
                        {
                            string baggage = "15 KG*7 KG//CabinBaggage:7KG | CheckInBaggage:15KG";

                        //var _jorny = objFlt.JourneysAvailableByMarket.Where(x => x.Key == SRC + "|" + DES).FirstOrDefault().Value.FirstOrDefault();
                        //var _jorny = objSearchResult.Data.FaresAvailable.Where(x => x.Key == SRC + "|" + DES).FirstOrDefault().Value;
                        var _jorny = objFlt.JourneysAvailableByMarket.Where(x => x.Key == SRC + "|" + DES).FirstOrDefault().Value.FirstOrDefault();


                        /*if (objFlt.Trips.FirstOrDefault().JourneysAvailableByMarket.Values.FirstOrDefault().ss. != null && objFlt.BaggageInformation.BaggageInfoAry != null && objFlt.BaggageInformation.BaggageInfoAry.Count() > 0)
                        //    if (objFlt.BaggageInformation != null && objFlt.BaggageInformation.BaggageInfoAry != null && objFlt.BaggageInformation.BaggageInfoAry.Count() > 0)
                            {
                            string DepArr = objRSeg.Dep + "-" + objRSeg.Arr;
                            foreach (var ObjBaggage in objFlt.BaggageInformation.BaggageInfoAry.FirstOrDefault().BaggageInfo)
                            {
                                if (ObjBaggage.DepArr == DepArr)
                                {
                                    baggage = ObjBaggage.BagCountName;//15 KG*7 KG//CabinBaggage:7KG | CheckInBaggage:15KG
                                    break;
                                }
                            }
                        }*/

                        if (baggage.IndexOf("|") != -1)
                            {
                                string[] split = baggage.Split('|');
                                string CabinBaggage = split[0].ToString().Trim().Replace("CabinBaggage:", "").Trim();
                                string CheckInBaggage = split[1].ToString().Trim().Replace("CheckInBaggage:", "").Trim();
                                baggage = CheckInBaggage + "*" + CabinBaggage;
                            }
                        List<Journey> _JourneyLists = new List<Journey>();
                        _JourneyLists = objFlt.JourneysAvailableByMarket.Where(x => x.Key == SRC + "|" + DES).FirstOrDefault().Value;
                        int OrderNo = 1;
                        //foreach (var objRSeg in _JourneyLists.Where(x => x.Designator.Origin == SRC && x.Designator.Destination == DES).FirstOrDefault().Fares)
                        foreach (var objRSeg in _JourneyLists)
                        {
                            foreach (var objRsgPrice in objRSeg.Fares)
                            {
                                foreach (var objRsgPriceDtl in objRsgPrice.Details)
                                {
                                    //int OrderNo = 1;
                                    foreach (var objFSeg in objRSeg.Segments)

                                    {
                                        DataRow drAdd = dtBound.NewRow();
                                        drAdd["RowID"] = RowID;
                                        drAdd["AirlineID"] = AirlineID;
                                        if (JourneyType.Equals("RT") || JourneyType.Equals("OW") || JourneyType.Equals("MC"))
                                        {
                                            drAdd["RefID"] = 6000 + RowID; //(objFSeg.Identifier.identifier + 6000);   //(objFlt.Idx + 6000);
                                        }
                                        else if (JourneyType.Equals("RW"))
                                        {
                                            if (_jorny.FlightType.Equals(1))    //if (objFlt.FlightType.Equals("O"))
                                            {
                                                drAdd["RefID"] = 6000 + RowID; // (objFSeg.Identifier.identifier + 6000);
                                            }
                                            else
                                            {
                                                drAdd["RefID"] = 16000 + RowID; // (objFSeg.Identifier.identifier + 16000);
                                            }
                                        }

                                        drAdd["OrderNo"] = OrderNo;

                                        drAdd["Origin"] = _jorny.Designator.Origin;//  objFSeg.Designator.Origin; //objRSeg.Dep;
                                        drAdd["Destination"] = _jorny.Designator.Destination; // objFSeg.Designator.Destination; //objRSeg.Arr;

                                        drAdd["CarrierName"] = objFSeg.Identifier.CarrierCode;   //objFSeg.ANm;
                                        drAdd["CarrierCode"] = objFSeg.Identifier.CarrierCode;  //objFSeg.MA;
                                        drAdd["FlightNumber"] = objFSeg.Identifier.identifier;  //objFSeg.FN;

                                        Leg leg = new Leg();
                                        leg = objFSeg.Legs.Where(x => x.Designator.Departure == objFSeg.Designator.Departure && x.Designator.Arrival == objFSeg.Designator.Arrival).FirstOrDefault();

                                        drAdd["DepartureStation"] = leg.Designator.Origin;  // objFSeg.Designator.Departure; //  leg.LegInfo.; //objFSeg.DApt;
                                        drAdd["ArrivalStation"] = leg.Designator.Destination; //leg.LegInfo.ArrivalTerminal; //objFSeg.AApt;

                                        drAdd["DepartureDate"] = leg.Designator.Departure.ToString("dd-MMM-yyyy"); //objFSeg.Ddat;
                                        drAdd["ArrivalDate"] = leg.Designator.Arrival.ToString("dd-MMM-yyyy");  // objFSeg.Adat;

                                        drAdd["DepartureTime"] = leg.Designator.Departure.ToString("HH:mm");    //objFSeg.DDtime;
                                        drAdd["ArrivalTime"] = leg.Designator.Arrival.ToString("HH:mm"); // objFSeg.DAtime;

                                        drAdd["DepDate"] = leg.LegInfo.DepartureTime.ToString("dd-MMM-yyyy");  //objFSeg.Ddat;
                                        drAdd["ArrDate"] = leg.LegInfo.ArrivalTime.ToString("dd-MMM-yyyy"); //objFSeg.Adat;

                                        drAdd["DepTime"] = leg.LegInfo.DepartureTime.ToString("HH:mm"); //objFSeg.DDtime;
                                        drAdd["ArrTime"] = leg.LegInfo.ArrivalTime.ToString("HH:mm");  //objFSeg.DAtime;



                                        drAdd["ClassOfService"] = leg.LegInfo.ScheduleServiceType;   // objFSeg.AC_Code;     //****** need to clarify this wiht API team  **

                                        if (JourneyType.Equals("RT"))
                                        {
                                            //if (objRSeg.Dep.Equals(SRC) && objRSeg.Arr.Equals(DES))
                                            if (leg.Designator.Origin.Equals(SRC) && leg.Designator.Destination.Equals(DES))
                                            {
                                                drAdd["FltType"] = "O";
                                            }
                                            else
                                            {
                                                drAdd["FltType"] = "I";
                                            }
                                        }
                                        else if (JourneyType.Equals("OW") || JourneyType.Equals("MC"))
                                        {
                                            drAdd["FltType"] = "O";
                                        }
                                        else if (JourneyType.Equals("RW"))
                                        {
                                            drAdd["FltType"] = objRSeg.FlightType; // _jorny.FlightType;  // objFlt.FlightType;
                                        }

                                        drAdd["Stops"] = objRSeg.Stops;   // _jorny.Stops;   // objRSeg.FSeg.Count() - 1;
                                                                          /*  this commented  of new implemantation V4  19 Nov 2023 by CS
                                                                          if (objFSeg.IsTechnicalStop)
                                                                          {
                                                                              drAdd["Via"] = 1;
                                                                          }*/
                                        drAdd["ViaName"] = "";

                                        drAdd["SeatsAvailable"] = objFSeg.IsSeatmapViewable;   //objFSeg.Seat;

                                        //var _FareAvailabilityKey = objRSeg.Fares.FirstOrDefault().FareAvailabilityKey;
                                        var _FareAvailabilityKey = objRsgPrice.FareAvailabilityKey;
                                        var _FareAvailabilityReferance = objRsgPriceDtl.Reference;
                                        //var _FaresAvailable = objSearchResult.Data.FaresAvailable;                                //var varPriceInfo = objFlt.PriceInfo.Where(x => x.Origin == objRSeg.Dep && x.Destination == objRSeg.Arr && x.PaxType == "ADT").FirstOrDefault();
                                        var _FaresAvailable = objSearchResult.Data.FaresAvailable[_FareAvailabilityKey].Fares.Where(x => x.Reference == _FareAvailabilityReferance).FirstOrDefault();
                                        //var varPriceInfo = _FaresAvailable[_FareAvailabilityKey].Fares.FirstOrDefault().PassengerFares.Where(x => x.PassengerType == "ADT");  // _FaresAvailable.Where(x => x.Key == _FareAvailabilityKey && x.Value.Fares.FirstOrDefault().PassengerFares.FirstOrDefault().PassengerType == "ADT").FirstOrDefault().Value;
                                        var varPriceInfo = _FaresAvailable.PassengerFares.Where(x => x.PassengerType == "ADT");
                                        //var varPriceInfo = _FaresAvailable.PassengerFares.Where(x => x.PassengerType == "ADT");
                                        if (varPriceInfo != null && varPriceInfo.FirstOrDefault() != null)
                                        {
                                            var _TaxFeeSum = varPriceInfo.FirstOrDefault().ServiceCharges.Where(x => x.Detail == "TaxFeeSum").FirstOrDefault();
                                            var _FeeSum = varPriceInfo.FirstOrDefault().ServiceCharges.Where(x => x.Detail == null || x.Detail != "TaxFeeSum").FirstOrDefault();



                                            drAdd["AdtTotalBasic"] = varPriceInfo.FirstOrDefault().FareAmount;    //   Decimal.ToInt32(varPriceInfo.SupplierBaseAmt);
                                            drAdd["AdtTotalTax"] = varPriceInfo.FirstOrDefault().RevenueFare;         //Decimal.ToInt32(varPriceInfo.SupplierTaxes);
                                            drAdd["AdtTotalFare"] = 0;

                                            drAdd["Adt_BASIC"] = _FeeSum == null ? 0 : _FeeSum.Amount; // Decimal.ToInt32(varPriceInfo.SupplierBaseAmt);
                                            drAdd["Adt_YQ"] = 0;
                                            drAdd["Adt_PSF"] = _TaxFeeSum == null ? 0 : _TaxFeeSum.Amount;  // Decimal.ToInt32(varPriceInfo.SupplierTaxes);
                                            drAdd["Adt_UDF"] = 0;
                                            drAdd["Adt_AUDF"] = 0;
                                            drAdd["Adt_CUTE"] = 0;
                                            drAdd["Adt_GST"] = 0;
                                            drAdd["Adt_TF"] = 0;
                                            drAdd["Adt_CESS"] = 0;
                                            drAdd["Adt_EX"] = 0;
                                        }

                                        //varPriceInfo = objFlt.PriceInfo.Where(x => x.Origin == objRSeg.Dep && x.Destination == objRSeg.Arr && x.PaxType == "CHD").FirstOrDefault();
                                        varPriceInfo = null;
                                        //varPriceInfo = _FaresAvailable[_FareAvailabilityKey].Fares.FirstOrDefault().PassengerFares.Where(x => x.PassengerType == "CHD");  // _FaresAvailable.Where(x => x.Key == _FareAvailabilityKey && x.Value.Fares.FirstOrDefault().PassengerFares.FirstOrDefault().PassengerType == "CHD").FirstOrDefault().Value;
                                        varPriceInfo = _FaresAvailable.PassengerFares.Where(x => x.PassengerType == "CHD");                                                                                                                                  //varPriceInfo = _FaresAvailable.PassengerFares.Where(x => x.PassengerType == "CHD");

                                        if (varPriceInfo != null && varPriceInfo.FirstOrDefault() != null)
                                        {
                                            var _TaxFeeSum = varPriceInfo.FirstOrDefault().ServiceCharges.Where(x => x.Detail == "TaxFeeSum").FirstOrDefault();
                                            var _FeeSum = varPriceInfo.FirstOrDefault().ServiceCharges.Where(x => x.Detail == null || x.Detail != "TaxFeeSum").FirstOrDefault();  // varPriceInfo.Fares.FirstOrDefault().PassengerFares.FirstOrDefault().ServiceCharges.Where(x => x.Detail == null || x.Detail != "TaxFeeSum").FirstOrDefault();

                                            drAdd["ChdTotalBasic"] = varPriceInfo.FirstOrDefault().FareAmount; // _FeeSum.Amount;   //Decimal.ToInt32(varPriceInfo.SupplierBaseAmt);
                                            drAdd["ChdTotalTax"] = varPriceInfo.FirstOrDefault().RevenueFare;      //Decimal.ToInt32(varPriceInfo.SupplierTaxes);
                                            drAdd["ChdTotalFare"] = 0;

                                            drAdd["Chd_BASIC"] = _FeeSum == null ? 0 : _FeeSum.Amount;   //Decimal.ToInt32(varPriceInfo.SupplierBaseAmt);
                                            drAdd["Chd_YQ"] = 0;
                                            drAdd["Chd_PSF"] = _TaxFeeSum == null ? 0 : _TaxFeeSum.Amount;  // Decimal.ToInt32(varPriceInfo.SupplierTaxes);
                                            drAdd["Chd_UDF"] = 0;
                                            drAdd["Chd_AUDF"] = 0;
                                            drAdd["Chd_CUTE"] = 0;
                                            drAdd["Chd_GST"] = 0;
                                            drAdd["Chd_TF"] = 0;
                                            drAdd["Chd_CESS"] = 0;
                                            drAdd["Chd_EX"] = 0;
                                        }

                                        //varPriceInfo = objFlt.PriceInfo.Where(x => x.Origin == objRSeg.Dep && x.Destination == objRSeg.Arr && x.PaxType == "INF").FirstOrDefault();
                                        varPriceInfo = null;
                                        //varPriceInfo = _FaresAvailable[_FareAvailabilityKey].Fares.FirstOrDefault().PassengerFares.Where(x => x.PassengerType == "INF"); // _FaresAvailable.Where(x => x.Key == _FareAvailabilityKey && x.Value.Fares.FirstOrDefault().PassengerFares.FirstOrDefault().PassengerType == "INF").FirstOrDefault().Value;
                                        varPriceInfo = _FaresAvailable.PassengerFares.Where(x => x.PassengerType == "INF");                                                                                                                                 //varPriceInfo = _FaresAvailable.PassengerFares.Where(x => x.PassengerType == "INF");
                                        if (varPriceInfo != null && varPriceInfo.FirstOrDefault() != null)
                                        {
                                            var _TaxFeeSum = varPriceInfo.FirstOrDefault().ServiceCharges.Where(x => x.Detail == "TaxFeeSum").FirstOrDefault();
                                            var _FeeSum = varPriceInfo.FirstOrDefault().ServiceCharges.Where(x => x.Detail == null || x.Detail != "TaxFeeSum").FirstOrDefault();


                                            drAdd["InfTotalBasic"] = varPriceInfo.FirstOrDefault().FareAmount; // _FeeSum.Amount;  //Decimal.ToInt32(varPriceInfo.SupplierBaseAmt);
                                            drAdd["InfTotalTax"] = varPriceInfo.FirstOrDefault().RevenueFare;   //Decimal.ToInt32(varPriceInfo.SupplierTaxes);
                                            drAdd["InfTotalFare"] = 0;

                                            drAdd["Inf_BASIC"] = _FeeSum == null ? 0 : _FeeSum.Amount;  // Decimal.ToInt32(varPriceInfo.SupplierBaseAmt);
                                            drAdd["Inf_TAX"] = _TaxFeeSum == null ? 0 : _TaxFeeSum.Amount; // Decimal.ToInt32(varPriceInfo.SupplierTaxes);
                                        }

                                        drAdd["Adt"] = Adults;
                                        drAdd["Chd"] = Childs;
                                        drAdd["Inf"] = Infants;

                                        drAdd["TotalFare"] = 0;
                                        drAdd["TotalBasic"] = 0;
                                        drAdd["TotalTax"] = 0;

                                        drAdd["TotalServiceTax"] = 0;
                                        drAdd["TotalServiceFee"] = 0;
                                        drAdd["TotalMarkup"] = 0;
                                        drAdd["TotalCommission"] = 0;
                                        drAdd["TotalCommission_SA"] = 0;
                                        drAdd["TotalTds"] = 0;
                                        drAdd["TotalTds_SA"] = 0;
                                        drAdd["SA_deal"] = "";
                                        drAdd["TotalCfee"] = 0;

                                        //var _FirstFares = objSearchResult.Data.FaresAvailable[_FareAvailabilityKey].Fares[0]; //_FaresAvailable[_FareAvailabilityKey].Fares[0];   // _FaresAvailable.Where(x => x.Key == _FareAvailabilityKey && x.Value.Fares.FirstOrDefault().PassengerFares.FirstOrDefault().PassengerType == "ADT").FirstOrDefault().Value.Fares.FirstOrDefault();
                                        var _FirstFares = _FaresAvailable;
                                        if (_FirstFares.ProductClass == "EC")
                                        {
                                            drAdd["PriceType"] = "@Saver"; // : " + _FirstFares.ProductClass;  //   objFlt.FareFamily;
                                        }
                                        else if (_FirstFares.ProductClass == "AV")
                                        {
                                            drAdd["PriceType"] = "@Flexi"; // : " + _FirstFares.ProductClass;  //   objFlt.FareFamily;
                                        }
                                        else if (_FirstFares.ProductClass == "SP")
                                        {
                                            drAdd["PriceType"] = "@Special"; // Fares  // : " + _FirstFares.ProductClass;  //   objFlt.FareFamily;
                                        }
                                        else if (_FirstFares.ProductClass == "SM")
                                        {
                                            drAdd["PriceType"] = "@Corporate";  // Select : " + _FirstFares.ProductClass;  //   objFlt.FareFamily;
                                        }
                                        else
                                        {
                                            drAdd["PriceType"] = "@" + _FirstFares.ProductClass;  //   objFlt.FareFamily;
                                        }


                                        drAdd["DepartureTerminal"] = leg.LegInfo.DepartureTerminal;  // objFSeg.ATer;
                                        drAdd["ArrivalTerminal"] = leg.LegInfo.ArrivalTerminal;  //objFSeg.DTer;
                                        drAdd["FareBasisCode"] = _FirstFares.FareBasisCode;  //objFSeg.AFBC;

                                        drAdd["FareStatus"] = true;
                                        drAdd["IsPriceChanged"] = false;


                                        drAdd["Cabin"] = Cabin;

                                        drAdd["EquipmentType"] = leg.LegInfo.EquipmentType;  // objFSeg.AirEquipType;
                                        drAdd["RefundType"] = "";
                                        drAdd["BaggageDetail"] = baggage;

                                        if (JourneyType.Equals("RW") || JourneyType.Equals("RT"))
                                        {
                                            drAdd["Trip"] = "R";
                                        }
                                        else if (JourneyType.Equals("MC"))
                                        {
                                            drAdd["Trip"] = "M";
                                        }
                                        else
                                        {
                                            drAdd["Trip"] = "O";
                                        }

                                        drAdd["Sector"] = Sector;
                                        drAdd["API_AirlineID"] = objFSeg.Identifier.identifier + "_" + objFSeg.Identifier.CarrierCode;   // leg.FlightReference;   // objFlt.Idx + "_" + objFSeg.MA;
                                        drAdd["BookingFareID"] = _FareAvailabilityKey;    //objFSeg.Identifier.identifier;   // _FirstFares.Reference;   // objFlt.Idx;
                                        drAdd["JourneySellKey"] = objRSeg.JourneyKey; //  _jorny.JourneyKey;

                                        TimeSpan timeDifference = leg.Designator.Arrival - leg.Designator.Departure;

                                        var _Duratio = ((timeDifference.Days * 24) * 60 + timeDifference.Hours * 60 + timeDifference.Minutes);

                                        drAdd["JourneyTime"] = _Duratio; //  leg.LegInfo.ArrivalTimeVariant;    //TI_dbData.TimeToMinustes(objRSeg.Dur);
                                        drAdd["Duration"] = _Duratio;  // leg.LegInfo.ArrivalTimeVariant; // TI_dbData.TimeToMinustes(objFSeg.Dur);

                                        drAdd["JourneyTimeDesc"] = leg.LegInfo.ArrivalTimeVariant;   // objRSeg.Dur;
                                        drAdd["DurationDesc"] = leg.LegInfo.ArrivalTimeVariant;  // objFSeg.Dur;

                                        dtBound.Rows.Add(drAdd);

                                    }
                                    OrderNo++;
                                    RowID++;
                                }// fare details
                            } //  journy list separate prices
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //DBCommon.Logger.dbLogg(CompanyID, 0, "TI_Converter-TISearch_2_Common", "", AirRQ, SearchID, ex.Message + "," + ex.StackTrace);
            }
            return dtBound;
        }
        
        //======== end new  API ResV2

        public DataTable TISearch_2_Common(string SearchID, string CompanyID, SearchResult objSearchResult, string JourneyType, string Sector, string AirRQ, string AirlineID)
        {
            DataTable dtBound = Common.Schema.SchemaFlights;
            try
            {
                DataSet ResponseDs = new DataSet();
                ResponseDs.ReadXml(new System.IO.StringReader(AirRQ));
                string SRC = ResponseDs.Tables["AvailabilityRequest"].Rows[0]["DepartureStation"].ToString();
                string DES = ResponseDs.Tables["AvailabilityRequest"].Rows[0]["ArrivalStation"].ToString();
                Int32 Adults = Convert.ToInt16(ResponseDs.Tables["AvailabilityRequest"].Rows[0]["Adult"].ToString());
                Int32 Childs = Convert.ToInt16(ResponseDs.Tables["AvailabilityRequest"].Rows[0]["Child"].ToString());
                Int32 Infants = Convert.ToInt16(ResponseDs.Tables["AvailabilityRequest"].Rows[0]["Infant"].ToString());
                string Cabin = ResponseDs.Tables["AvailabilityRequest"].Rows[0]["Cabin"].ToString();

                bool VerifyRSeg = false;

                int RowID = 1;
                Cabin = Cabin == "Y" ? "Economy" : Cabin == "A" ? "PremiumEconomy" : Cabin == "C" ? "Business" : Cabin == "B" ? "FirstClass" : "EC";
                foreach (var objFlt in objSearchResult.Airlines.Flight)
                {
                    if (JourneyType.Equals("RT") && objFlt.Details.RSeg.Count().Equals(2))
                    {
                        VerifyRSeg = true;
                    }
                    else if ((JourneyType.Equals("OW") || JourneyType.Equals("RW")) && objFlt.Details.RSeg.Count().Equals(1))
                    {
                        VerifyRSeg = true;
                    }
                    else if (JourneyType.Equals("MC") && objFlt.Details.RSeg.Count() > 2)
                    {
                        VerifyRSeg = true;
                    }

                    if (VerifyRSeg)
                    {
                        string baggage = "";
                        foreach (var objRSeg in objFlt.Details.RSeg)
                        {
                            if (objFlt.BaggageInformation != null && objFlt.BaggageInformation.BaggageInfoAry != null && objFlt.BaggageInformation.BaggageInfoAry.Count() > 0)
                            {
                                string DepArr = objRSeg.Dep + "-" + objRSeg.Arr;
                                foreach (var ObjBaggage in objFlt.BaggageInformation.BaggageInfoAry.FirstOrDefault().BaggageInfo)
                                {
                                    if (ObjBaggage.DepArr == DepArr)
                                    {
                                        baggage = ObjBaggage.BagCountName;//15 KG*7 KG//CabinBaggage:7KG | CheckInBaggage:15KG
                                        break;
                                    }
                                }
                            }

                            if (baggage.IndexOf("|") != -1)
                            {
                                string[] split = baggage.Split('|');
                                string CabinBaggage = split[0].ToString().Trim().Replace("CabinBaggage:", "").Trim();
                                string CheckInBaggage = split[1].ToString().Trim().Replace("CheckInBaggage:", "").Trim();
                                baggage = CheckInBaggage + "*" + CabinBaggage;
                            }


                            int OrderNo = 1;
                            foreach (var objFSeg in objRSeg.FSeg)
                            {
                                DataRow drAdd = dtBound.NewRow();
                                drAdd["RowID"] = RowID;
                                drAdd["AirlineID"] = AirlineID;
                                if (JourneyType.Equals("RT") || JourneyType.Equals("OW") || JourneyType.Equals("MC"))
                                {
                                    drAdd["RefID"] = (objFlt.Idx + 6000);
                                }
                                else if (JourneyType.Equals("RW"))
                                {
                                    if (objFlt.FlightType.Equals("O"))
                                    {
                                        drAdd["RefID"] = (objFlt.Idx + 6000);
                                    }
                                    else
                                    {
                                        drAdd["RefID"] = (objFlt.Idx + 16000);
                                    }
                                }

                                drAdd["OrderNo"] = OrderNo;

                                drAdd["Origin"] = objRSeg.Dep;
                                drAdd["Destination"] = objRSeg.Arr;

                                drAdd["CarrierName"] = objFSeg.ANm;
                                drAdd["CarrierCode"] = objFSeg.MA;
                                drAdd["FlightNumber"] = objFSeg.FN;

                                drAdd["DepartureStation"] = objFSeg.DApt;
                                drAdd["ArrivalStation"] = objFSeg.AApt;

                                drAdd["DepartureDate"] = objFSeg.Ddat;
                                drAdd["ArrivalDate"] = objFSeg.Adat;

                                drAdd["DepartureTime"] = objFSeg.DDtime;
                                drAdd["ArrivalTime"] = objFSeg.DAtime;

                                drAdd["DepDate"] = objFSeg.Ddat;
                                drAdd["ArrDate"] = objFSeg.Adat;

                                drAdd["DepTime"] = objFSeg.DDtime;
                                drAdd["ArrTime"] = objFSeg.DAtime;

                                drAdd["ClassOfService"] = objFSeg.AC_Code;

                                if (JourneyType.Equals("RT"))
                                {
                                    if (objRSeg.Dep.Equals(SRC) && objRSeg.Arr.Equals(DES))
                                    {
                                        drAdd["FltType"] = "O";
                                    }
                                    else
                                    {
                                        drAdd["FltType"] = "I";
                                    }
                                }
                                else if (JourneyType.Equals("OW") || JourneyType.Equals("MC"))
                                {
                                    drAdd["FltType"] = "O";
                                }
                                else if (JourneyType.Equals("RW"))
                                {
                                    drAdd["FltType"] = objFlt.FlightType;
                                }

                                drAdd["Stops"] = objRSeg.FSeg.Count() - 1;
                                if (objFSeg.IsTechnicalStop)
                                {
                                    drAdd["Via"] = 1;
                                }
                                drAdd["ViaName"] = "";

                                drAdd["SeatsAvailable"] = objFSeg.Seat;

                                var varPriceInfo = objFlt.PriceInfo.Where(x => x.Origin == objRSeg.Dep && x.Destination == objRSeg.Arr && x.PaxType == "ADT").FirstOrDefault();
                                if (varPriceInfo != null)
                                {
                                    string Taxes = varPriceInfo.SupplierTaxBreakup.Value;
                                    if (Taxes != null && Taxes.IndexOf(";") != -1)
                                    {
                                        string[] split = Taxes.Split(';');
                                        for (int i = 0; i < split.Length; i++)
                                        {
                                            if (split[i].ToString().Trim().Length > 0)
                                            {

                                            }
                                        }
                                    }

                                    drAdd["AdtTotalBasic"] = Decimal.ToInt32(varPriceInfo.SupplierBaseAmt);
                                    drAdd["AdtTotalTax"] = Decimal.ToInt32(varPriceInfo.SupplierTaxes);
                                    drAdd["AdtTotalFare"] = 0;

                                    drAdd["Adt_BASIC"] = Decimal.ToInt32(varPriceInfo.SupplierBaseAmt);
                                    drAdd["Adt_YQ"] = 0;
                                    drAdd["Adt_PSF"] = Decimal.ToInt32(varPriceInfo.SupplierTaxes);
                                    drAdd["Adt_UDF"] = 0;
                                    drAdd["Adt_AUDF"] = 0;
                                    drAdd["Adt_CUTE"] = 0;
                                    drAdd["Adt_GST"] = 0;
                                    drAdd["Adt_TF"] = 0;
                                    drAdd["Adt_CESS"] = 0;
                                    drAdd["Adt_EX"] = 0;
                                }

                                varPriceInfo = objFlt.PriceInfo.Where(x => x.Origin == objRSeg.Dep && x.Destination == objRSeg.Arr && x.PaxType == "CHD").FirstOrDefault();
                                if (varPriceInfo != null)
                                {
                                    drAdd["ChdTotalBasic"] = Decimal.ToInt32(varPriceInfo.SupplierBaseAmt);
                                    drAdd["ChdTotalTax"] = Decimal.ToInt32(varPriceInfo.SupplierTaxes);
                                    drAdd["ChdTotalFare"] = 0;

                                    drAdd["Chd_BASIC"] = Decimal.ToInt32(varPriceInfo.SupplierBaseAmt);
                                    drAdd["Chd_YQ"] = 0;
                                    drAdd["Chd_PSF"] = Decimal.ToInt32(varPriceInfo.SupplierTaxes);
                                    drAdd["Chd_UDF"] = 0;
                                    drAdd["Chd_AUDF"] = 0;
                                    drAdd["Chd_CUTE"] = 0;
                                    drAdd["Chd_GST"] = 0;
                                    drAdd["Chd_TF"] = 0;
                                    drAdd["Chd_CESS"] = 0;
                                    drAdd["Chd_EX"] = 0;
                                }

                                varPriceInfo = objFlt.PriceInfo.Where(x => x.Origin == objRSeg.Dep && x.Destination == objRSeg.Arr && x.PaxType == "INF").FirstOrDefault();
                                if (varPriceInfo != null)
                                {
                                    drAdd["InfTotalBasic"] = Decimal.ToInt32(varPriceInfo.SupplierBaseAmt);
                                    drAdd["InfTotalTax"] = Decimal.ToInt32(varPriceInfo.SupplierTaxes);
                                    drAdd["InfTotalFare"] = 0;

                                    drAdd["Inf_BASIC"] = Decimal.ToInt32(varPriceInfo.SupplierBaseAmt);
                                    drAdd["Inf_TAX"] = Decimal.ToInt32(varPriceInfo.SupplierTaxes);
                                }

                                drAdd["Adt"] = Adults;
                                drAdd["Chd"] = Childs;
                                drAdd["Inf"] = Infants;

                                drAdd["TotalFare"] = 0;
                                drAdd["TotalBasic"] = 0;
                                drAdd["TotalTax"] = 0;

                                drAdd["TotalServiceTax"] = 0;
                                drAdd["TotalServiceFee"] = 0;
                                drAdd["TotalMarkup"] = 0;
                                drAdd["TotalCommission"] = 0;
                                drAdd["TotalCommission_SA"] = 0;
                                drAdd["TotalTds"] = 0;
                                drAdd["TotalTds_SA"] = 0;
                                drAdd["SA_deal"] = "";
                                drAdd["TotalCfee"] = 0;

                                drAdd["PriceType"] = "@" + objFlt.FareFamily;
                                drAdd["DepartureTerminal"] = objFSeg.ATer;
                                drAdd["ArrivalTerminal"] = objFSeg.DTer;
                                drAdd["FareBasisCode"] = objFSeg.AFBC;

                                drAdd["FareStatus"] = true;
                                drAdd["IsPriceChanged"] = false;


                                drAdd["Cabin"] = Cabin;

                                drAdd["EquipmentType"] = objFSeg.AirEquipType;
                                drAdd["RefundType"] = "";
                                drAdd["BaggageDetail"] = baggage;

                                if (JourneyType.Equals("RW") || JourneyType.Equals("RT"))
                                {
                                    drAdd["Trip"] = "R";
                                }
                                else if (JourneyType.Equals("MC"))
                                {
                                    drAdd["Trip"] = "M";
                                }
                                else
                                {
                                    drAdd["Trip"] = "O";
                                }

                                drAdd["Sector"] = Sector;
                                drAdd["API_AirlineID"] = objFlt.Idx + "_" + objFSeg.MA;
                                drAdd["BookingFareID"] = objFlt.Idx;
                                drAdd["JourneyTime"] = TI_dbData.TimeToMinustes(objRSeg.Dur);
                                drAdd["Duration"] = TI_dbData.TimeToMinustes(objFSeg.Dur);

                                drAdd["JourneyTimeDesc"] = objRSeg.Dur;
                                drAdd["DurationDesc"] = objFSeg.Dur;

                                dtBound.Rows.Add(drAdd);
                                OrderNo++;
                                RowID++;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                DBCommon.Logger.dbLogg(CompanyID, 0, "TI_Converter-TISearch_2_Common", "", AirRQ, SearchID, ex.Message + "," + ex.StackTrace);
            }
            return dtBound;
        }
        public DataTable TIFare_2_Common(string SearchID, string CompanyID, Flight objFlt, string JourneyType, DataTable dtBound)
        {
            try
            {
                bool VerifyRSeg = false;
                if (JourneyType.Equals("RT") && objFlt.Details.RSeg.Count().Equals(2))
                {
                    VerifyRSeg = true;
                }
                else if ((JourneyType.Equals("OW") || JourneyType.Equals("RW")) && objFlt.Details.RSeg.Count().Equals(1))
                {
                    VerifyRSeg = true;
                }
                else if (JourneyType.Equals("MC") && objFlt.Details.RSeg.Count() > 2)
                {
                    VerifyRSeg = true;
                }

                if (VerifyRSeg && objFlt.Idx.Equals(Convert.ToInt16(dtBound.Rows[0]["BookingFareID"].ToString())))
                {
                    BindFlight(dtBound, objFlt);
                    BindFare(dtBound, objFlt);
                    BindFareRule(dtBound, objFlt);
                }
            }
            catch (Exception ex)
            {
                DBCommon.Logger.dbLogg(CompanyID, 0, "TI_Converter-TIFare_2_Common", "", "", SearchID, ex.Message + "," + ex.StackTrace);
            }
            return dtBound;
        }
        private void BindFlight(DataTable dtBound, TIRequestResponse.Flight objFlt)
        {
            int k = 0;
            string baggage = "";
            foreach (DataRow dr in dtBound.Rows)
            {
                foreach (var objRSeg in objFlt.Details.RSeg)
                {
                    if (objRSeg.FSeg.Where(x => x.DApt == dr["DepartureStation"].ToString().Trim() && x.AApt == dr["ArrivalStation"].ToString().Trim() && x.FN == dr["FlightNumber"].ToString().Trim() && x.MA == dr["CarrierCode"].ToString().Trim()).Count().Equals(1))
                    {
                        var objFSeg = objRSeg.FSeg.Where(x => x.DApt == dr["DepartureStation"].ToString().Trim() && x.AApt == dr["ArrivalStation"].ToString().Trim() && x.FN == dr["FlightNumber"].ToString().Trim() && x.MA == dr["CarrierCode"].ToString().Trim());
                        if (objFlt.BaggageInformation != null && objFlt.BaggageInformation.BaggageInfoAry != null && objFlt.BaggageInformation.BaggageInfoAry.Count() > 0)
                        {
                            //string DepArr = objFlt.Details.RSeg.FirstOrDefault().Dep + "-" + objFlt.Details.RSeg.FirstOrDefault().Arr;
                            string DepArr = dr["Origin"].ToString() + "-" + dr["Destination"].ToString();
                            foreach (var ObjBaggage in objFlt.BaggageInformation.BaggageInfoAry.FirstOrDefault().BaggageInfo)
                            {
                                if (ObjBaggage.DepArr == DepArr)
                                {
                                    baggage = ObjBaggage.BagCountName;//15 KG*7 KG//CabinBaggage:7KG | CheckInBaggage:15KG
                                    break;
                                }
                            }
                        }

                        if (baggage.IndexOf("|") != -1)
                        {
                            string[] split = baggage.Split('|');
                            string CabinBaggage = split[0].ToString().Trim().Replace("CabinBaggage:", "").Trim();
                            string CheckInBaggage = split[1].ToString().Trim().Replace("CheckInBaggage:", "").Trim();
                            baggage = CheckInBaggage + "*" + CabinBaggage;
                        }

                        dr["DepartureDate"] = objFSeg.FirstOrDefault().Ddat;
                        dr["ArrivalDate"] = objFSeg.FirstOrDefault().Adat;

                        dr["DepartureTime"] = objFSeg.FirstOrDefault().DDtime;
                        dr["ArrivalTime"] = objFSeg.FirstOrDefault().DAtime;

                        dr["DepDate"] = objFSeg.FirstOrDefault().Ddat;
                        dr["ArrDate"] = objFSeg.FirstOrDefault().Adat;

                        dr["DepTime"] = objFSeg.FirstOrDefault().DDtime;
                        dr["ArrTime"] = objFSeg.FirstOrDefault().DAtime;

                        dr["ClassOfService"] = objFSeg.FirstOrDefault().AC_Code;

                        dr["Stops"] = objFSeg.Count() - 1;
                        if (objFSeg.FirstOrDefault().IsTechnicalStop)
                        {
                            dr["Via"] = 1;
                        }
                        dr["ViaName"] = "";

                        dr["SeatsAvailable"] = objFSeg.FirstOrDefault().Seat;

                        dr["PriceType"] = objFlt.FareFamily;
                        dr["DepartureTerminal"] = objFSeg.FirstOrDefault().ATer;
                        dr["ArrivalTerminal"] = objFSeg.FirstOrDefault().DTer;
                        dr["FareBasisCode"] = objFSeg.FirstOrDefault().AFBC;

                        dr["FareStatus"] = true;
                        dr["FareQuote"] = "Updated";

                        dr["EquipmentType"] = objFSeg.FirstOrDefault().AirEquipType;
                        dr["RefundType"] = "";
                        dr["BaggageDetail"] = baggage;

                        dr["JourneyTime"] = TI_dbData.TimeToMinustes(objFlt.Details.RSeg.FirstOrDefault().Dur);
                        dr["Duration"] = TI_dbData.TimeToMinustes(objFSeg.FirstOrDefault().Dur);

                        dr["JourneyTimeDesc"] = objFlt.Details.RSeg.FirstOrDefault().Dur;
                        dr["DurationDesc"] = objFSeg.FirstOrDefault().Dur;
                        k++;
                    }
                    //break;
                }
            }
        }
        private void BindFare(DataTable dtBound, TIRequestResponse.Flight objFlt)
        {
            foreach (DataRow dr in dtBound.Rows)
            {
                var varPriceInfo = objFlt.PriceInfo.Where(x => x.Origin == dr["Origin"].ToString() && x.Destination == dr["Destination"].ToString() && x.PaxType == "ADT").FirstOrDefault();
                if (varPriceInfo != null)
                {
                    dr["AdtTotalBasic"] = Decimal.ToInt32(varPriceInfo.SupplierBaseAmt);
                    dr["AdtTotalTax"] = Decimal.ToInt32(varPriceInfo.SupplierTaxes);
                    dr["Adt_PSF"] = Decimal.ToInt32(varPriceInfo.SupplierTaxes);
                    dr["Adt_BASIC"] = Decimal.ToInt32(varPriceInfo.SupplierBaseAmt);

                    Hashtable HtTaxes = GetTaxes(varPriceInfo.SupplierTaxBreakup.Value);
                    if (HtTaxes != null && HtTaxes.Count > 0)
                    {
                        dr["Adt_YQ"] = Decimal.ToInt32(Convert.ToDecimal(HtTaxes["YQ"].ToString()));
                        dr["Adt_PSF"] = Decimal.ToInt32(Convert.ToDecimal(HtTaxes["PSF"].ToString()));
                        dr["Adt_GST"] = Decimal.ToInt32(Convert.ToDecimal(HtTaxes["GST"].ToString()));
                        dr["Adt_CUTE"] = Decimal.ToInt32(Convert.ToDecimal(HtTaxes["CUTE"].ToString()));
                        dr["Adt_EX"] = Decimal.ToInt32(Convert.ToDecimal(HtTaxes["EX"].ToString()));
                    }
                }

                varPriceInfo = objFlt.PriceInfo.Where(x => x.Origin == dr["Origin"].ToString() && x.Destination == dr["Destination"].ToString() && x.PaxType == "CHD").FirstOrDefault();
                if (varPriceInfo != null)
                {
                    dr["ChdTotalBasic"] = Decimal.ToInt32(varPriceInfo.SupplierBaseAmt);
                    dr["ChdTotalTax"] = Decimal.ToInt32(varPriceInfo.SupplierTaxes);
                    dr["Chd_BASIC"] = Decimal.ToInt32(varPriceInfo.SupplierBaseAmt);
                    dr["Chd_PSF"] = Decimal.ToInt32(varPriceInfo.SupplierTaxes);

                    Hashtable HtTaxes = GetTaxes(varPriceInfo.SupplierTaxBreakup.Value);
                    if (HtTaxes != null && HtTaxes.Count > 0)
                    {
                        dr["Chd_YQ"] = Decimal.ToInt32(Convert.ToDecimal(HtTaxes["YQ"].ToString()));
                        dr["Chd_PSF"] = Decimal.ToInt32(Convert.ToDecimal(HtTaxes["PSF"].ToString()));
                        dr["Chd_GST"] = Decimal.ToInt32(Convert.ToDecimal(HtTaxes["GST"].ToString()));
                        dr["Chd_CUTE"] = Decimal.ToInt32(Convert.ToDecimal(HtTaxes["CUTE"].ToString()));
                        dr["Chd_EX"] = Decimal.ToInt32(Convert.ToDecimal(HtTaxes["EX"].ToString()));
                    }
                }

                varPriceInfo = objFlt.PriceInfo.Where(x => x.Origin == dr["Origin"].ToString() && x.Destination == dr["Destination"].ToString() && x.PaxType == "INF").FirstOrDefault();
                if (varPriceInfo != null)
                {
                    dr["InfTotalBasic"] = Decimal.ToInt32(varPriceInfo.SupplierBaseAmt);
                    dr["InfTotalTax"] = Decimal.ToInt32(varPriceInfo.SupplierTaxes);
                    dr["InfTotalFare"] = 0;

                    dr["Inf_BASIC"] = Decimal.ToInt32(varPriceInfo.SupplierBaseAmt);
                    dr["Inf_TAX"] = Decimal.ToInt32(varPriceInfo.SupplierTaxes);
                }
            }
        }
        private Hashtable GetTaxes(string Taxes)
        {
            Hashtable HTTaxes = new Hashtable();
            decimal YQ = 0;
            decimal PSF = 0;
            decimal CUTE = 0;
            decimal GST = 0;
            decimal EX = 0;
            if (Taxes != null && Taxes.IndexOf(";") != -1)
            {
                string[] split = Taxes.Split(';');
                for (int i = 0; i < split.Length; i++)
                {
                    if (split[i].ToString().Trim().Length > 0)
                    {
                        if (split[i].ToString().Trim().IndexOf("ASF") != -1 || split[i].ToString().Trim().IndexOf("PSF") != -1 || split[i].ToString().Trim().IndexOf("UDF") != -1)
                        {
                            PSF += FilterTax(split[i].ToString().Trim());
                        }
                        else if (split[i].ToString().Trim().IndexOf("CUTE") != -1)
                        {
                            CUTE += FilterTax(split[i].ToString().Trim());
                        }
                        else if (split[i].ToString().Trim().IndexOf("YQ") != -1)
                        {
                            YQ += FilterTax(split[i].ToString().Trim());
                        }
                        else if (split[i].ToString().Trim().IndexOf("C07") != -1 || split[i].ToString().Trim().IndexOf("U07") != -1)
                        {
                            GST += FilterTax(split[i].ToString().Trim());
                        }
                        else
                        {
                            EX += FilterTax(split[i].ToString().Trim());
                        }
                    }
                }

                HTTaxes.Add("YQ", YQ);
                HTTaxes.Add("PSF", PSF);
                HTTaxes.Add("CUTE", CUTE);
                HTTaxes.Add("GST", GST);
                HTTaxes.Add("EX", EX);
            }
            return HTTaxes;
        }
        private decimal FilterTax(string Data)
        {
            if (Data.IndexOf(":") != -1)
            {
                return Convert.ToDecimal(Data.Split(':')[1].ToString().Trim());
            }
            return 0;
        }

        private void BindFareRule(DataTable dtBound, TIRequestResponse.Flight objFlt)
        {
            if (objFlt.FaresRulesReply != null && objFlt.FaresRulesReply.Description != null && objFlt.FaresRulesReply.Description.Count() > 0)
            {
                foreach (DataRow dr in dtBound.Rows)
                {
                    var FareRule = objFlt.FaresRulesReply.Description.Where(x => x.Dep == dr["Origin"].ToString() && x.Arr == dr["Destination"].ToString());
                    if (FareRule != null && FareRule.Count() > 0)
                    {
                        dr["FareRule"] = RemoveHtmlTags(FareRule.FirstOrDefault().Description);
                    }
                }
            }          
        }
        public string RemoveHtmlTags(string text)
        {
            List<int> openTagIndexes = Regex.Matches(text, "<").Cast<Match>().Select(m => m.Index).ToList();
            List<int> closeTagIndexes = Regex.Matches(text, ">").Cast<Match>().Select(m => m.Index).ToList();
            if (closeTagIndexes.Count > 0)
            {
                StringBuilder sb = new StringBuilder();
                int previousIndex = 0;
                foreach (int closeTagIndex in closeTagIndexes)
                {
                    var openTagsSubset = openTagIndexes.Where(x => x >= previousIndex && x < closeTagIndex);
                    if (openTagsSubset.Count() > 0 && closeTagIndex - openTagsSubset.Max() > 1)
                    {
                        sb.Append(text.Substring(previousIndex, openTagsSubset.Max() - previousIndex));
                        sb.Append(" ");
                    }
                    else
                    {
                        sb.Append(text.Substring(previousIndex, closeTagIndex - previousIndex + 1));
                        sb.Append(" ");
                    }
                    previousIndex = closeTagIndex + 1;
                }
                if (closeTagIndexes.Max() < text.Length)
                {
                    sb.Append(" ");
                    sb.Append(text.Substring(closeTagIndexes.Max() + 1));
                }
                sb.ToString();
                string s1 = Regex.Replace(sb.ToString(), @"<[^>]+>|&nbsp;", "").Trim();
                return Regex.Replace(s1, @"\s{2,}", " ");
            }
            else
            {
                return text;
            }
        }
    }
}
