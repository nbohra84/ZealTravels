using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZealTravel.Infrastructure.Spicejet
{
    class SetAvailabilityModifier
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
        public SetAvailabilityModifier(string Searchid, string Companyid, string Supplierid, string Signature, string PriceType, string Origin, string Destination, int Adt, int Chd, int Inf, string Sector)
        {
            this.Searchid = Searchid;
            this.Companyid = Companyid;
            this.Supplierid = Supplierid;
            this.Signature = Signature;

            if (PriceType.Equals("LCC"))
            {
                this.RefundType= "Y";
                this.PriceType = "PUB";
            }
            if (PriceType.Equals("TBF"))
            {
                this.RefundType = "Y";
                this.PriceType = "TBF";
            }
            else if (PriceType.Equals("NDF"))
            {
                this.RefundType = "Y";
                this.PriceType = "NDF";
            }
            else if (PriceType.Equals("SME"))
            {
                this.RefundType = "Y";
                this.PriceType = "SME";
            }
            else if (PriceType.Equals("SPL"))
            {
                this.RefundType = "Y";
                this.PriceType = "SPL";
            }
            else if (PriceType.Equals("COUPON"))
            {
                this.RefundType = "Y";
                this.PriceType = "COU";
            }
            else if (PriceType.Equals("CORPORATE"))
            {
                this.RefundType = "N";
                this.PriceType = "COR";
            }
            else if (PriceType.Equals("MAX"))
            {
                this.RefundType = "N";
                this.PriceType = "MAX";
            }
            else
            {
                this.RefundType = "Y";
                this.PriceType = "PUB";
            }

            this.Origin = Origin;
            this.Destination = Destination;
            this.Sector = Sector;
            this.Adt = Adt;
            this.Chd = Chd;
            this.Inf = Inf;
        }
        public DataTable FlightModifier(svc_booking.Journey[] journey, string FltType, DataTable dtFlights)
        {
            try
            {
                int Rowid = 1;
                int Refid = 1500;
                if (Supplierid.Equals("MAAXT98402") && PriceType.Equals("COR"))
                {
                    PriceType = "!" + PriceType;
                    Refid = 1500;
                }
                else if (Supplierid.Equals("MAAXTA8402") && PriceType.Equals("Regular"))
                {
                    PriceType = "!!" + PriceType;
                    Refid = 2500;
                }
                else if (Supplierid.Equals("MAAXT98402") && PriceType.Equals("MAX"))
                {
                    PriceType = "!!!!!" + PriceType;
                    Refid = 5500;
                }
                else if (Supplierid.Equals("CPNMAA0030"))
                {
                    PriceType = "!!!" + PriceType;
                    Refid = 2000;
                }
                else
                {
                    PriceType = "!!!!" + PriceType;
                    Refid = 5000;
                }

                if (FltType.Equals("I"))
                {
                    Refid = Refid + 10000;
                }

                GetCommonFunctions objcf = new GetCommonFunctions();

                int OrderNo = 0;

                for (int i = 0; i < journey.Length; i++)
                {
                    string JourneySellKey = journey[i].JourneySellKey.Trim();
                    svc_booking.Segment[] SegmentCs = journey[i].Segments;
                    int SegmentCount = SegmentCs.Length;

                    if (SegmentCount.Equals(1))
                    {
                        svc_booking.Fare[] FareCs = SegmentCs[0].Fares;
                        int FareCount = FareCs.Length;

                        for (int j = 0; j < FareCount; j++)
                        {
                            OrderNo = 1;
                            Refid++;
                            svc_booking.Leg[] LegCs = SegmentCs[0].Legs;
                            int LegCount = LegCs.Length;
                            for (int n = 0; n < LegCount; n++)
                            {
                                DataRow drAdd = dtFlights.NewRow();
                                drAdd["Sector"] = Sector;
                                drAdd["Rowid"] = Rowid;
                                drAdd["RefID"] = Refid;
                                drAdd["OrderNo"] = OrderNo;
                                //drAdd["Flightid"] = FltType + Supplierid + Refid;
                                drAdd["AirlineID"] = Supplierid;
                                //drAdd["ContractVersion"] = ContractVersion;

                                drAdd["DepartureTerminal"] = LegCs[n].LegInfo.DepartureTerminal.Trim();
                                drAdd["ArrivalTerminal"] = LegCs[n].LegInfo.ArrivalTerminal.Trim();
                                drAdd["DepartureStation"] = LegCs[n].DepartureStation.Trim();
                                drAdd["ArrivalStation"] = LegCs[n].ArrivalStation.Trim();
                                drAdd["CarrierCode"] = LegCs[n].FlightDesignator.CarrierCode.Trim();
                                //drAdd["CarrierName"] = "Goair";
                                drAdd["FlightNumber"] = LegCs[n].FlightDesignator.FlightNumber.Trim();

                                drAdd["DepartureDate"] = objcf.DateFormat(LegCs[n].STD);
                                drAdd["ArrivalDate"] = objcf.DateFormat(LegCs[n].STA);
                                drAdd["DepartureTime"] = objcf.TimeFormat(LegCs[n].STD);
                                drAdd["ArrivalTime"] = objcf.TimeFormat(LegCs[n].STA);

                                drAdd["DepDate"] = LegCs[n].STD.ToString().Trim();
                                drAdd["ArrDate"] = LegCs[n].STA.ToString().Trim();
                                drAdd["DepTime"] = LegCs[n].STD.ToString().Trim();
                                drAdd["ArrTime"] = LegCs[n].STA.ToString().Trim();




                                drAdd["EquipmentType"] = LegCs[n].LegInfo.EquipmentType.Trim();
                                drAdd["JourneySellKey"] = JourneySellKey.Trim();
                                drAdd["ClassOfService"] = FareCs[j].ClassOfService.Trim();
                                drAdd["FareSellKey"] = FareCs[j].FareSellKey.Trim();
                                drAdd["FareBasisCode"] = FareCs[j].FareBasisCode.Trim();
                                drAdd["RuleNumber"] = FareCs[j].RuleNumber.Trim();
                                drAdd["RuleTarrif"] = FareCs[j].RuleTariff.Trim();
                                drAdd["ProductClass"] = FareCs[j].ProductClass.Trim();

                                drAdd["Cabin"] = FareCs[j].ProductClass.Trim();
                                drAdd["PriceType"] = PriceType.Trim();

                                drAdd["SeatsAvailable"] = ((svc_booking.AvailableFare)(FareCs[j])).AvailableCount.ToString().Trim();

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

                        svc_booking.Fare[] FareCs1 = SegmentCs[0].Fares;
                        svc_booking.Fare[] FareCs2 = SegmentCs[1].Fares;
                        int FareCount1 = FareCs1.Length;
                        int FareCount2 = FareCs2.Length;

                        for (int j = 0; j < FareCount1; j++)
                        {
                            OrderNo = 1;
                            Refid++;
                            string FareSellKey = FareCs1[j].FareSellKey.Trim() + "^" + FareCs2[j].FareSellKey.Trim();
                            svc_booking.Leg[] LegCs1 = SegmentCs[0].Legs;
                            int LegCount1 = LegCs1.Length;

                            for (int n = 0; n < LegCount1; n++)
                            {
                                DataRow drAdd = dtFlights.NewRow();
                                drAdd["Sector"] = Sector;
                                drAdd["Rowid"] = Rowid;
                                drAdd["RefID"] = Refid;
                                drAdd["OrderNo"] = OrderNo;
                                //drAdd["Flightid"] = FltType + Supplierid + Refid;
                                drAdd["AirlineID"] = Supplierid;
                                //drAdd["ContractVersion"] = ContractVersion;

                                drAdd["DepartureTerminal"] = LegCs1[n].LegInfo.DepartureTerminal.Trim();
                                drAdd["ArrivalTerminal"] = LegCs1[n].LegInfo.ArrivalTerminal.Trim();
                                drAdd["DepartureStation"] = LegCs1[n].DepartureStation.Trim();
                                drAdd["ArrivalStation"] = LegCs1[n].ArrivalStation.Trim();
                                drAdd["CarrierCode"] = LegCs1[n].FlightDesignator.CarrierCode.Trim();
                                //drAdd["CarrierName"] = "Goair";
                                drAdd["FlightNumber"] = LegCs1[n].FlightDesignator.FlightNumber.Trim();

                                drAdd["DepartureDate"] = objcf.DateFormat(LegCs1[n].STD);
                                drAdd["ArrivalDate"] = objcf.DateFormat(LegCs1[n].STA);
                                drAdd["DepartureTime"] = objcf.TimeFormat(LegCs1[n].STD);
                                drAdd["ArrivalTime"] = objcf.TimeFormat(LegCs1[n].STA);

                                drAdd["DepDate"] = LegCs1[n].STD.ToString().Trim();
                                drAdd["ArrDate"] = LegCs1[n].STA.ToString().Trim();
                                drAdd["DepTime"] = LegCs1[n].STD.ToString().Trim();
                                drAdd["ArrTime"] = LegCs1[n].STA.ToString().Trim();


                                drAdd["EquipmentType"] = LegCs1[n].LegInfo.EquipmentType.Trim();
                                drAdd["JourneySellKey"] = JourneySellKey.Trim();
                                drAdd["ClassOfService"] = FareCs1[j].ClassOfService.Trim();
                                drAdd["FareSellKey"] = FareSellKey.Trim();
                                drAdd["FareBasisCode"] = FareCs1[j].FareBasisCode.Trim();
                                drAdd["RuleNumber"] = FareCs1[j].RuleNumber.Trim();
                                drAdd["RuleTarrif"] = FareCs1[j].RuleTariff.Trim();
                                drAdd["ProductClass"] = FareCs1[j].ProductClass.Trim();

                                drAdd["Cabin"] = FareCs1[j].ProductClass.Trim();
                                drAdd["PriceType"] = PriceType;

                                drAdd["SeatsAvailable"] = ((svc_booking.AvailableFare)(FareCs1[j])).AvailableCount.ToString().Trim();

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

                            svc_booking.Leg[] LegCs2 = SegmentCs[1].Legs;
                            int LegCount2 = LegCs2.Length;
                            for (int n = 0; n < LegCount2; n++)
                            {
                                DataRow drAdd = dtFlights.NewRow();
                                drAdd["Sector"] = Sector;
                                drAdd["Rowid"] = Rowid;
                                drAdd["RefID"] = Refid;
                                drAdd["OrderNo"] = OrderNo;
                                //drAdd["Flightid"] = FltType + Supplierid + Refid;
                                drAdd["AirlineID"] = Supplierid;
                                //drAdd["ContractVersion"] = ContractVersion;

                                drAdd["DepartureTerminal"] = LegCs2[n].LegInfo.DepartureTerminal.Trim();
                                drAdd["ArrivalTerminal"] = LegCs2[n].LegInfo.ArrivalTerminal.Trim();
                                drAdd["DepartureStation"] = LegCs2[n].DepartureStation.Trim();
                                drAdd["ArrivalStation"] = LegCs2[n].ArrivalStation.Trim();
                                drAdd["CarrierCode"] = LegCs2[n].FlightDesignator.CarrierCode.Trim();
                                //drAdd["CarrierName"] = "Goair";
                                drAdd["FlightNumber"] = LegCs2[n].FlightDesignator.FlightNumber.Trim();

                                drAdd["DepartureDate"] = objcf.DateFormat(LegCs2[n].STD);
                                drAdd["ArrivalDate"] = objcf.DateFormat(LegCs2[n].STA);
                                drAdd["DepartureTime"] = objcf.TimeFormat(LegCs2[n].STD);
                                drAdd["ArrivalTime"] = objcf.TimeFormat(LegCs2[n].STA);


                                drAdd["DepDate"] = LegCs2[n].STD.ToString().Trim();
                                drAdd["ArrDate"] = LegCs2[n].STA.ToString().Trim();
                                drAdd["DepTime"] = LegCs2[n].STD.ToString().Trim();
                                drAdd["ArrTime"] = LegCs2[n].STA.ToString().Trim();

                                drAdd["EquipmentType"] = LegCs2[n].LegInfo.EquipmentType.Trim();
                                drAdd["JourneySellKey"] = JourneySellKey.Trim();
                                drAdd["ClassOfService"] = FareCs2[j].ClassOfService.Trim();
                                drAdd["FareSellKey"] = FareSellKey.Trim();
                                drAdd["FareBasisCode"] = FareCs2[j].FareBasisCode.Trim();
                                drAdd["RuleNumber"] = FareCs2[j].RuleNumber.Trim();
                                drAdd["RuleTarrif"] = FareCs2[j].RuleTariff.Trim();
                                drAdd["ProductClass"] = FareCs2[j].ProductClass.Trim();

                                drAdd["Cabin"] = FareCs2[j].ProductClass.Trim();
                                drAdd["PriceType"] = PriceType;

                                drAdd["SeatsAvailable"] = ((svc_booking.AvailableFare)(FareCs2[j])).AvailableCount.ToString().Trim();

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

                        svc_booking.Fare[] FareCs1 = SegmentCs[0].Fares;
                        svc_booking.Fare[] FareCs2 = SegmentCs[1].Fares;
                        svc_booking.Fare[] FareCs3 = SegmentCs[2].Fares;
                        int FareCount1 = FareCs1.Length;
                        int FareCount2 = FareCs2.Length;
                        int FareCount3 = FareCs3.Length;

                        for (int j = 0; j < FareCount1; j++)
                        {
                            OrderNo = 1;
                            Refid++;
                            string FareSellKey = FareCs1[j].FareSellKey.Trim() + "^" + FareCs2[j].FareSellKey.Trim() + "^" + FareCs3[j].FareSellKey.Trim();
                            svc_booking.Leg[] LegCs1 = SegmentCs[0].Legs;
                            int LegCount1 = LegCs1.Length;
                            for (int n = 0; n < LegCount1; n++)
                            {
                                DataRow drAdd = dtFlights.NewRow();
                                drAdd["Sector"] = Sector;
                                drAdd["Rowid"] = Rowid;
                                drAdd["RefID"] = Refid;
                                drAdd["OrderNo"] = OrderNo;
                                //drAdd["Flightid"] = FltType + Supplierid + Refid;
                                drAdd["AirlineID"] = Supplierid;
                                //drAdd["ContractVersion"] = ContractVersion;

                                drAdd["DepartureTerminal"] = LegCs1[n].LegInfo.DepartureTerminal.Trim();
                                drAdd["ArrivalTerminal"] = LegCs1[n].LegInfo.ArrivalTerminal.Trim();
                                drAdd["DepartureStation"] = LegCs1[n].DepartureStation.Trim();
                                drAdd["ArrivalStation"] = LegCs1[n].ArrivalStation.Trim();
                                drAdd["CarrierCode"] = LegCs1[n].FlightDesignator.CarrierCode.Trim();
                                //drAdd["CarrierName"] = "Goair";
                                drAdd["FlightNumber"] = LegCs1[n].FlightDesignator.FlightNumber.Trim();


                                drAdd["DepartureDate"] = objcf.DateFormat(LegCs1[n].STD);
                                drAdd["ArrivalDate"] = objcf.DateFormat(LegCs1[n].STA);
                                drAdd["DepartureTime"] = objcf.TimeFormat(LegCs1[n].STD);
                                drAdd["ArrivalTime"] = objcf.TimeFormat(LegCs1[n].STA);

                                drAdd["DepDate"] = LegCs1[n].STD.ToString().Trim();
                                drAdd["ArrDate"] = LegCs1[n].STA.ToString().Trim();
                                drAdd["DepTime"] = LegCs1[n].STD.ToString().Trim();
                                drAdd["ArrTime"] = LegCs1[n].STA.ToString().Trim();

                                drAdd["EquipmentType"] = LegCs1[n].LegInfo.EquipmentType.Trim();
                                drAdd["JourneySellKey"] = JourneySellKey.Trim();
                                drAdd["ClassOfService"] = FareCs1[j].ClassOfService.Trim();
                                drAdd["FareSellKey"] = FareSellKey.Trim();
                                drAdd["FareBasisCode"] = FareCs1[j].FareBasisCode.Trim();
                                drAdd["RuleNumber"] = FareCs1[j].RuleNumber.Trim();
                                drAdd["RuleTarrif"] = FareCs1[j].RuleTariff.Trim();
                                drAdd["ProductClass"] = FareCs1[j].ProductClass.Trim();

                                drAdd["Cabin"] = FareCs1[j].ProductClass.Trim();
                                drAdd["PriceType"] = PriceType;

                                drAdd["SeatsAvailable"] = ((svc_booking.AvailableFare)(FareCs1[j])).AvailableCount.ToString().Trim();

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

                            svc_booking.Leg[] LegCs2 = SegmentCs[1].Legs;
                            int LegCount2 = LegCs2.Length;
                            for (int n = 0; n < LegCount2; n++)
                            {
                                DataRow drAdd = dtFlights.NewRow();
                                drAdd["Sector"] = Sector;
                                drAdd["Rowid"] = Rowid;
                                drAdd["RefID"] = Refid;
                                drAdd["OrderNo"] = OrderNo;
                                //drAdd["Flightid"] = FltType + Supplierid + Refid;
                                drAdd["AirlineID"] = Supplierid;
                                //drAdd["ContractVersion"] = ContractVersion;

                                drAdd["DepartureTerminal"] = LegCs2[n].LegInfo.DepartureTerminal.Trim();
                                drAdd["ArrivalTerminal"] = LegCs2[n].LegInfo.ArrivalTerminal.Trim();
                                drAdd["DepartureStation"] = LegCs2[n].DepartureStation.Trim();
                                drAdd["ArrivalStation"] = LegCs2[n].ArrivalStation.Trim();
                                drAdd["CarrierCode"] = LegCs2[n].FlightDesignator.CarrierCode.Trim();
                                //drAdd["CarrierName"] = "Goair";
                                drAdd["FlightNumber"] = LegCs2[n].FlightDesignator.FlightNumber.Trim();

                                drAdd["DepartureDate"] = objcf.DateFormat(LegCs2[n].STD);
                                drAdd["ArrivalDate"] = objcf.DateFormat(LegCs2[n].STA);
                                drAdd["DepartureTime"] = objcf.TimeFormat(LegCs2[n].STD);
                                drAdd["ArrivalTime"] = objcf.TimeFormat(LegCs2[n].STA);

                                drAdd["DepDate"] = LegCs2[n].STD.ToString().Trim();
                                drAdd["ArrDate"] = LegCs2[n].STA.ToString().Trim();
                                drAdd["DepTime"] = LegCs2[n].STD.ToString().Trim();
                                drAdd["ArrTime"] = LegCs2[n].STA.ToString().Trim();

                                drAdd["EquipmentType"] = LegCs2[n].LegInfo.EquipmentType.Trim();
                                drAdd["JourneySellKey"] = JourneySellKey.Trim();
                                drAdd["ClassOfService"] = FareCs2[j].ClassOfService.Trim();
                                drAdd["FareSellKey"] = FareSellKey.Trim();
                                drAdd["FareBasisCode"] = FareCs2[j].FareBasisCode.Trim();
                                drAdd["RuleNumber"] = FareCs2[j].RuleNumber.Trim();
                                drAdd["RuleTarrif"] = FareCs2[j].RuleTariff.Trim();
                                drAdd["ProductClass"] = FareCs2[j].ProductClass.Trim();

                                drAdd["Cabin"] = FareCs2[j].ProductClass.Trim();
                                drAdd["PriceType"] = PriceType;

                                drAdd["SeatsAvailable"] = ((svc_booking.AvailableFare)(FareCs2[j])).AvailableCount.ToString().Trim();

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

                            svc_booking.Leg[] LegCs3 = SegmentCs[2].Legs;
                            int LegCount3 = LegCs3.Length;

                            for (int n = 0; n < LegCount3; n++)
                            {
                                DataRow drAdd = dtFlights.NewRow();
                                drAdd["Sector"] = Sector;
                                drAdd["Rowid"] = Rowid;
                                drAdd["RefID"] = Refid;
                                drAdd["OrderNo"] = OrderNo;
                                //drAdd["Flightid"] = FltType + Supplierid + Refid;
                                drAdd["AirlineID"] = Supplierid;
                                //drAdd["ContractVersion"] = ContractVersion;

                                drAdd["DepartureTerminal"] = LegCs3[n].LegInfo.DepartureTerminal.Trim();
                                drAdd["ArrivalTerminal"] = LegCs3[n].LegInfo.ArrivalTerminal.Trim();
                                drAdd["DepartureStation"] = LegCs3[n].DepartureStation.Trim();
                                drAdd["ArrivalStation"] = LegCs3[n].ArrivalStation.Trim();
                                drAdd["CarrierCode"] = LegCs3[n].FlightDesignator.CarrierCode.Trim();
                                //drAdd["CarrierName"] = "Goair";
                                drAdd["FlightNumber"] = LegCs3[n].FlightDesignator.FlightNumber.Trim();

                                drAdd["DepartureDate"] = objcf.DateFormat(LegCs3[n].STD);
                                drAdd["ArrivalDate"] = objcf.DateFormat(LegCs3[n].STA);
                                drAdd["DepartureTime"] = objcf.TimeFormat(LegCs3[n].STD);
                                drAdd["ArrivalTime"] = objcf.TimeFormat(LegCs3[n].STA);

                                drAdd["DepDate"] = LegCs3[n].STD.ToString().Trim();
                                drAdd["ArrDate"] = LegCs3[n].STA.ToString().Trim();
                                drAdd["DepTime"] = LegCs3[n].STD.ToString().Trim();
                                drAdd["ArrTime"] = LegCs3[n].STA.ToString().Trim();

                                drAdd["EquipmentType"] = LegCs3[n].LegInfo.EquipmentType.Trim();
                                drAdd["JourneySellKey"] = JourneySellKey.Trim();
                                drAdd["ClassOfService"] = FareCs3[j].ClassOfService.Trim();
                                drAdd["FareSellKey"] = FareSellKey.Trim();
                                drAdd["FareBasisCode"] = FareCs3[j].FareBasisCode.Trim();
                                drAdd["RuleNumber"] = FareCs3[j].RuleNumber.Trim();
                                drAdd["RuleTarrif"] = FareCs3[j].RuleTariff.Trim();
                                drAdd["ProductClass"] = FareCs3[j].ProductClass.Trim();
                                drAdd["Cabin"] = FareCs3[j].ProductClass.Trim();
                                drAdd["PriceType"] = PriceType;

                                drAdd["SeatsAvailable"] = ((svc_booking.AvailableFare)(FareCs3[j])).AvailableCount.ToString().Trim();

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
               // dbCommon.Logger.dbLogg(Companyid, 0, "SetAvailabilityResponseModifier", "air_spicejet", Supplierid, Searchid, ex.Message + "," + ex.StackTrace);
            }
            return dtFlights;
        }
    }
}
