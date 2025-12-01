using DocumentFormat.OpenXml.Spreadsheet;
using iText.IO.Font.Otf;
using Microsoft.AspNetCore.Mvc;
using System.Collections;
using System.Data;
using System.Net.NetworkInformation;
using System.Security.Claims;
using System.Web;
using System.Xml;
using ZealTravel.Application.AgencyManagement.Queries;
using ZealTravel.Application.AirelineManagement.Queries;
using ZealTravel.Application.AirlineManagement.Handler;
using ZealTravel.Application.AirlineManagement.Queries;
using ZealTravel.Application.Handlers;
using ZealTravel.Application.UserManagement.Queries;
using ZealTravel.Common.Helpers;
using ZealTravel.Common.Models;
using ZealTravel.Domain.Interfaces.Handlers;
using ZealTravel.Front.Web.Models.Flight;
using ZealTravel.Front.Web.Models.Flight.SearchFilter;

namespace ZealTravel.Front.Web.Helper.Flight
{

    public class ShowFlightDataHelper
    {
        private readonly IHandlesQueryAsync<string, CompanyRegisterCorporateUserDetails> _getCompanyRegisterCorporateUserDetailsQueryHandler;
        private readonly IHandlesQueryAsync<string, CompanyRegisterCorporateUserLimitDetails> _getCompanyRegisterCorporateUserLimitQueryHandler;
        public ShowFlightDataHelper(IHandlesQueryAsync<string, CompanyRegisterCorporateUserDetails> getCompanyRegisterCorporateUserDetailsQueryHandler, IHandlesQueryAsync<string, CompanyRegisterCorporateUserLimitDetails> getCompanyRegisterCorporateUserLimitQueryHandler)
        {
            getCompanyRegisterCorporateUserDetailsQueryHandler = _getCompanyRegisterCorporateUserDetailsQueryHandler;
            getCompanyRegisterCorporateUserLimitQueryHandler = _getCompanyRegisterCorporateUserLimitQueryHandler;
        }
        public static List<AirlineAvailabilityInfo> GetSelec(string Trip)
        {
            string availabilityResponse = HttpContextHelper.Current?.Session.GetString("FinalResult");
            if (string.IsNullOrEmpty(availabilityResponse))
                return new List<AirlineAvailabilityInfo>();

            List<XmlNode> xnList1 = new List<XmlNode>();
            XmlDocument xmldoc1 = new XmlDocument();
            xmldoc1.LoadXml(availabilityResponse);

            if (Trip.Equals("O"))
            {
                xnList1 = xmldoc1.SelectNodes("/root/AvailabilityInfo[FltType='O']")?.Cast<XmlNode>().ToList() ?? new List<XmlNode>();
            }
            else
            {
                xnList1 = xmldoc1.SelectNodes("/root/AvailabilityInfo[FltType='I']")?.Cast<XmlNode>().ToList() ?? new List<XmlNode>();
            }

            var flightList = new List<AirlineAvailabilityInfo>();

            foreach (XmlNode node in xnList1)
            {
                var flight = new AirlineAvailabilityInfo();
                XMLHelper.MapXmlToObject(node, flight);
                flightList.Add(flight);
            }
            var sortedList = flightList.OrderBy(item => item.RowID).ToList();
            return sortedList;

        }
        public static List<AirlineAvailabilityInfo> GetSelec()
        {
            string availabilityResponse = HttpContextHelper.Current?.Session.GetString("FinalResult");
            if (string.IsNullOrEmpty(availabilityResponse))
                return new List<AirlineAvailabilityInfo>();

            XmlDocument xmldoc = new XmlDocument();
            xmldoc.LoadXml(availabilityResponse);

            var flightList = new List<AirlineAvailabilityInfo>();

            foreach (XmlNode node in xmldoc.SelectNodes("/root/AvailabilityInfo"))
            {
                var flight = new AirlineAvailabilityInfo();
                XMLHelper.MapXmlToObject(node, flight);
                flightList.Add(flight);
            }
            var sortedList = flightList.OrderBy(item => item.RowID).ToList();
            return sortedList;
        }
        //============================================================================================================================
        public static List<k_ShowFlightOutBound> ResultData(string flterType, string SearchType, string CompanyID)
        {
            List<k_ShowFlightOutBound> FlightOutBound = new List<k_ShowFlightOutBound>();

            var flightList = new List<AirlineAvailabilityInfo>();
            if (flterType == "OUTBOUND")
            {
                flightList = GetSelec("O");
            }
            else if (flterType == "INBOUND")
            {
                flightList = GetSelec("I");
            }

            // Get distinct RefId values
            var distinctRefIds = flightList
                .GroupBy(x => x.RefID) // Replace RefId with the property representing RefId in your objects
                .Select(group => group.Key)
                .Where(refId => refId != 0)
                .ToList();

            if (distinctRefIds.Any())
            {
                string curr = "INR";
                // Check session or alternative storage for currency
                if (HttpContextHelper.Current?.Session.GetString("Curr") != null)
                {
                    curr = HttpContextHelper.Current.Session.GetString("Curr");
                }
                string rules = string.Empty;
                string defaultRule = string.Empty;
                int arrivalCheck = 0;
                int promo = 0;
                int checkA_C = 0;

                // Process each distinct RefId
                foreach (var refId in distinctRefIds)
                {
                    var relatedFlights = flightList
                        .Where(x => x.RefID == refId)
                        .ToList();

                    if (relatedFlights.Count == 1)
                    {
                        // Perform necessary processing on relatedFlights

                        var relatedFlight = relatedFlights.FirstOrDefault();
                        rules = GetFareRules(relatedFlights);
                        string departureDate = relatedFlight.DepartureDate.ToString(); // Assuming DepartureDate is a property of k_ShowFlightOutBound
                        string arrivalDate = relatedFlight.ArrivalDate.ToString();
                        if (departureDate != arrivalDate)
                        {
                            arrivalCheck = DateHelper.DayDiff(departureDate, arrivalDate);
                        }
                        FlightOutBound.Add(new k_ShowFlightOutBound()
                        {
                            Curr = curr,
                            AgentType = checkA_C,
                            CompanyID = CompanyID,
                            FlightRefid = relatedFlight.RefID.ToString(),

                            NoOFAdult = int.Parse(relatedFlight.Adt.ToString()),
                            NoOFChild = int.Parse(relatedFlight.Chd.ToString()),
                            NoOFInfant = int.Parse(relatedFlight.Inf.ToString()),

                            Adt_BASIC = GetConvert(relatedFlight.Adt_BASIC.ToString()),
                            Chd_BASIC = GetConvert(relatedFlight.Chd_BASIC.ToString()),
                            Inf_BASIC = GetConvert(relatedFlight.Inf_BASIC.ToString()),

                            AdultbaseFare = int.Parse(relatedFlight.Adt.ToString()) * GetConvert(relatedFlight.Adt_BASIC),
                            ChildbaseFare = int.Parse(relatedFlight.Chd.ToString()) * GetConvert(relatedFlight.Chd_BASIC),
                            InfantbaseFare = int.Parse(relatedFlight.Inf.ToString()) * GetConvert(relatedFlight.Inf_BASIC),

                            Adt_YQ = GetConvert(int.Parse(relatedFlight.Adt_YQ.ToString())),
                            AdtTotalTax = GetConvert(int.Parse(relatedFlight.AdtTotalTax.ToString())),
                            Chd_YQ = GetConvert(int.Parse(relatedFlight.Chd_YQ.ToString())),
                            ChdTotalTax = GetConvert(int.Parse(relatedFlight.ChdTotalTax.ToString())),
                            Inf_TAX = GetConvert(int.Parse(relatedFlight.Inf_TAX.ToString())),

                            Adt_AcuTax = ReturnTAXADT(relatedFlight).ToString(),
                            Chd_AcuTax = ReturnTAXCHD(relatedFlight).ToString(),
                            Inf_AcuTax = ReturnTAXINF(relatedFlight).ToString(),

                            Yq = "Commission : " + String.Format("{0:N0}", GetConvert(int.Parse(relatedFlight.TotalCommission.ToString()))),
                            PromoChek = promo,

                            TotalTax = GetTotalTax(relatedFlight),
                            TotalBasic = GetTotalBasic(relatedFlight),
                            TotalServiceFee = GetTotalServiceFee(relatedFlight),
                            TotalMarkUp = GetTotalMarkUp(relatedFlight),
                            TotalServiceTax = GetTotalServiceTax(relatedFlight),
                            TaxandCharges = GetTaxandCharges(relatedFlight),
                            TotalCommission = ReturnCommission(relatedFlight, CompanyID),
                            TotalTds = ReturnTDS(relatedFlight, CompanyID),

                            GrossF = ReturnGrossFare(relatedFlight).ToString(),
                            FinalFare = ReturnFinalFare(relatedFlight, CompanyID),
                            TotalAmount = ReturnTotalFareAmount(relatedFlight),
                            TotalFare = ReturnTotalFareAmount(relatedFlight),

                            FlightName = relatedFlight.CarrierCode.ToString(),
                            DepartStationName = relatedFlight.DepartureStationName.ToString(),
                            ArrivalStationName = relatedFlight.ArrivalStationName.ToString(),
                            DepartureStationAirport = relatedFlight.DepartureStationAirport.ToString(),
                            ArrivalStationAirport = relatedFlight.ArrivalStationAirport.ToString(),
                            Cabin = relatedFlight.Cabin.ToString(),
                            CarrierName = relatedFlight.CarrierName.ToString(),
                            FlightNumber = relatedFlight.FlightNumber.ToString(),
                            FlightDepDate = relatedFlight.DepartureDate.ToString(),
                            FlightDepTime = relatedFlight.DepartureTime.ToString(),
                            FlightArrDate = relatedFlight.ArrivalDate.ToString(),
                            FlightArrTime = relatedFlight.ArrivalTime.ToString(),
                            CHECKINBaggage = ReturnCheckINBaggage(relatedFlight).ToString(),
                            CABINBaggage = ReturnCabinINBaggage(relatedFlight).ToString(),
                            SRC = relatedFlight.Origin.ToString(),
                            DEST = relatedFlight.Destination.ToString(),
                            PrimarySRC = relatedFlight.Origin.ToString(),
                            PrimaryDEST = relatedFlight.Destination.ToString(),
                            Stop = "NonStop",
                            logo = "/assets/img/airlogo_square/" + relatedFlight.CarrierCode.ToString() + ".gif",
                            Duration = relatedFlight.DurationDesc.ToString(),
                            connection = 1,
                            SMSACTIVES = 0,
                            ArrivalNextDayCheck = arrivalCheck,
                            PriceType = relatedFlight.PriceType.ToString(),
                            RefundType = ReturnRefundType(relatedFlight).ToString(),
                            AvailableSeat = checkSeat(relatedFlight.SeatsAvailable.ToString()),
                            FareRules = rules,
                            RuleTarrif = relatedFlight.RuleTarrif.ToString(),

                            Class1 = ReturnCls(relatedFlight),
                            SRC1 = relatedFlight.DepartureStation.ToString(),
                            DEST1 = relatedFlight.ArrivalStation.ToString(),
                            DepartStationName1 = relatedFlight.DepartureStationName.ToString(),
                            ArrivalStationName1 = relatedFlight.ArrivalStationName.ToString(),
                            DepartureStationAirport1 = relatedFlight.DepartureStationAirport.ToString(),
                            ArrivalStationAirport1 = relatedFlight.ArrivalStationAirport.ToString(),
                            Via = checkVia(relatedFlight.Via.ToString()),
                            ViaName = checkViaName(relatedFlight.ViaName.ToString()),
                            TerminalSRC = checkTerminal(relatedFlight.DepartureTerminal.ToString()),
                            TerminalDEST = checkTerminal(relatedFlight.ArrivalTerminal.ToString()),
                            FlightName1 = relatedFlight.CarrierCode.ToString(),
                            Cabin1 = relatedFlight.Cabin.ToString(),
                            CarrierName1 = relatedFlight.CarrierName.ToString(),
                            FlightNumber1 = relatedFlight.FlightNumber.ToString(),
                            FlightDepDate1 = relatedFlight.DepartureDate.ToString(),
                            FlightDepTime1 = relatedFlight.DepartureTime.ToString(),
                            FlightArrDate1 = relatedFlight.ArrivalDate.ToString(),
                            FlightArrTime1 = relatedFlight.ArrivalTime.ToString(),
                            Layover1 = relatedFlight.JourneyTimeDesc.ToString(),
                            Duration1 = relatedFlight.DurationDesc.ToString(),
                            FlightNumberCmb = relatedFlight.CarrierCode.ToString() + relatedFlight.FlightNumber.ToString()
                        });
                    }

                    else if (relatedFlights.Count == 2)
                    {
                        //commented on 21Jan2024 by cpk if (ReturnCls(relatedFlights[0]).ToString() == ReturnCls(relatedFlights[1]).ToString() )
                        if (ReturnProviderCd(relatedFlights[0]).ToString() == ReturnProviderCd(relatedFlights[1]).ToString())   //added 21Jan2024 by cpk
                        {
                            rules = GetFareRules(relatedFlights);
                            if (relatedFlights[0].DepartureDate.ToString() != relatedFlights[1].ArrivalDate.ToString())
                            {
                                arrivalCheck = DateHelper.DayDiff(relatedFlights[0].DepartureDate.ToString(), relatedFlights[1].ArrivalDate.ToString());
                            }
                            FlightOutBound.Add(new k_ShowFlightOutBound()
                            {
                                Curr = curr,
                                AgentType = checkA_C,
                                CompanyID = CompanyID,
                                FlightRefid = relatedFlights[0].RefID.ToString(),

                                NoOFAdult = int.Parse(relatedFlights[0].Adt.ToString()),
                                NoOFChild = int.Parse(relatedFlights[0].Chd.ToString()),
                                NoOFInfant = int.Parse(relatedFlights[0].Inf.ToString()),
                                //==============================================================================================================================
                                Adt_BASIC = GetConvert(relatedFlights[0].Adt_BASIC.ToString()),
                                Chd_BASIC = GetConvert(relatedFlights[0].Chd_BASIC.ToString()),
                                Inf_BASIC = GetConvert(relatedFlights[0].Inf_BASIC.ToString()),

                                AdultbaseFare = int.Parse(relatedFlights[0].Adt.ToString()) * GetConvert(int.Parse(relatedFlights[0].Adt_BASIC.ToString())),
                                ChildbaseFare = int.Parse(relatedFlights[0].Chd.ToString()) * GetConvert(int.Parse(relatedFlights[0].Chd_BASIC.ToString())),
                                InfantbaseFare = int.Parse(relatedFlights[0].Inf.ToString()) * GetConvert(int.Parse(relatedFlights[0].Inf_BASIC.ToString())),

                                Adt_YQ = GetConvert(int.Parse(relatedFlights[0].Adt_YQ.ToString())),
                                AdtTotalTax = GetConvert(int.Parse(relatedFlights[0].AdtTotalTax.ToString())),
                                Chd_YQ = GetConvert(int.Parse(relatedFlights[0].Chd_YQ.ToString())),
                                ChdTotalTax = GetConvert(int.Parse(relatedFlights[0].ChdTotalTax.ToString())),
                                Inf_TAX = GetConvert(int.Parse(relatedFlights[0].Inf_TAX.ToString())),

                                Adt_AcuTax = ReturnTAXADT(relatedFlights[0]).ToString(),
                                Chd_AcuTax = ReturnTAXCHD(relatedFlights[0]).ToString(),
                                Inf_AcuTax = ReturnTAXINF(relatedFlights[0]).ToString(),

                                Yq = "Commission : " + String.Format("{0:N0}", GetConvert(int.Parse(relatedFlights[0].TotalCommission.ToString()))),
                                PromoChek = promo,

                                TotalTax = GetTotalTax(relatedFlights[0]),
                                TotalBasic = GetTotalBasic(relatedFlights[0]),
                                TotalServiceFee = GetTotalServiceFee(relatedFlights[0]),
                                TotalMarkUp = GetTotalMarkUp(relatedFlights[0]),
                                TotalServiceTax = GetTotalServiceTax(relatedFlights[0]),
                                TaxandCharges = GetTaxandCharges(relatedFlights[0]),
                                TotalCommission = ReturnCommission(relatedFlights[0], CompanyID),
                                TotalTds = ReturnTDS(relatedFlights[0], CompanyID),

                                GrossF = ReturnGrossFare(relatedFlights[0]).ToString(),
                                FinalFare = ReturnFinalFare(relatedFlights[0], CompanyID),
                                TotalAmount = ReturnTotalFareAmount(relatedFlights[0]),
                                TotalFare = ReturnTotalFareAmount(relatedFlights[0]),
                                //==============================================================================================================================

                                FlightName = relatedFlights[0].CarrierCode.ToString(),
                                CarrierName = relatedFlights[0].CarrierName.ToString(),
                                DepartStationName = relatedFlights[0].DepartureStationName.ToString(),
                                ArrivalStationName = relatedFlights[0].ArrivalStationName.ToString(),
                                DepartureStationAirport = relatedFlights[0].DepartureStationAirport.ToString(),
                                ArrivalStationAirport = relatedFlights[0].ArrivalStationAirport.ToString(),
                                Cabin = relatedFlights[0].Cabin.ToString(),
                                FlightNumber = relatedFlights[0].FlightNumber.ToString(),
                                FlightDepDate = relatedFlights[0].DepartureDate.ToString(),
                                FlightDepTime = relatedFlights[0].DepartureTime.ToString(),
                                FlightArrDate = relatedFlights[1].ArrivalDate.ToString(),
                                FlightArrTime = relatedFlights[1].ArrivalTime.ToString(),
                                CHECKINBaggage = ReturnCheckINBaggage(relatedFlights[0]).ToString(),
                                CABINBaggage = ReturnCabinINBaggage(relatedFlights[0]).ToString(),
                                RuleTarrif = relatedFlights[0].RuleTarrif.ToString(),
                                SRC = relatedFlights[0].Origin.ToString(),
                                DEST = relatedFlights[0].Destination.ToString(),
                                logo = "/assets/img/airlogo_square/" + relatedFlights[0].CarrierCode.ToString() + ".gif",
                                Duration = relatedFlights[0].DurationDesc.ToString(),
                                connection = 2,
                                SMSACTIVES = 0,// SMSACTIVE,
                                ArrivalNextDayCheck = arrivalCheck,
                                PriceType = relatedFlights[0].PriceType.ToString(),
                                RefundType = ReturnRefundType(relatedFlights[0]).ToString(),
                                AvailableSeat = checkSeat(relatedFlights[0].SeatsAvailable.ToString()),
                                FareRules = rules,
                                Stop = "OneStop",

                                Class1 = ReturnCls(relatedFlights[0]).ToString(),
                                SRC1 = relatedFlights[0].DepartureStation.ToString(),
                                DEST1 = relatedFlights[0].ArrivalStation.ToString(),
                                DepartStationName1 = relatedFlights[0].DepartureStationName.ToString(),
                                ArrivalStationName1 = relatedFlights[0].ArrivalStationName.ToString(),
                                DepartureStationAirport1 = relatedFlights[0].DepartureStationAirport.ToString(),
                                ArrivalStationAirport1 = relatedFlights[0].ArrivalStationAirport.ToString(),
                                Via = checkVia(relatedFlights[0].Via.ToString()),
                                ViaName = checkViaName(relatedFlights[0].ViaName.ToString()),
                                TerminalSRC = checkTerminal(relatedFlights[0].DepartureTerminal.ToString()),
                                TerminalDEST = checkTerminal(relatedFlights[0].ArrivalTerminal.ToString()),
                                FlightName1 = relatedFlights[0].CarrierCode.ToString(),
                                Cabin1 = relatedFlights[0].Cabin.ToString(),
                                CarrierName1 = relatedFlights[0].CarrierName.ToString(),
                                FlightNumber1 = relatedFlights[0].FlightNumber.ToString(),
                                FlightDepDate1 = relatedFlights[0].DepartureDate.ToString(),
                                FlightDepTime1 = relatedFlights[0].DepartureTime.ToString(),
                                FlightArrDate1 = relatedFlights[0].ArrivalDate.ToString(),
                                FlightArrTime1 = relatedFlights[0].ArrivalTime.ToString(),
                                Duration1 = relatedFlights[0].DurationDesc.ToString(),

                                Class2 = ReturnCls(relatedFlights[1]).ToString(),
                                SRC2 = relatedFlights[1].DepartureStation.ToString(),
                                DEST2 = relatedFlights[1].ArrivalStation.ToString(),
                                DepartStationName2 = relatedFlights[1].DepartureStationName.ToString(),
                                ArrivalStationName2 = relatedFlights[1].ArrivalStationName.ToString(),
                                DepartureStationAirport2 = relatedFlights[1].DepartureStationAirport.ToString(),
                                ArrivalStationAirport2 = relatedFlights[1].ArrivalStationAirport.ToString(),
                                Via1 = checkVia(relatedFlights[1].Via.ToString()),
                                ViaName1 = checkViaName(relatedFlights[1].ViaName.ToString()),
                                TerminalSRC1 = checkTerminal(relatedFlights[1].DepartureTerminal.ToString()),
                                TerminalDEST1 = checkTerminal(relatedFlights[1].ArrivalTerminal.ToString()),
                                logo1 = "/assets/img/airlogo_square/" + relatedFlights[1].CarrierCode.ToString() + ".gif",
                                FlightName2 = relatedFlights[1].CarrierCode.ToString(),
                                Cabin2 = relatedFlights[1].Cabin.ToString(),
                                CarrierName2 = relatedFlights[1].CarrierName.ToString(),
                                FlightNumber2 = relatedFlights[1].FlightNumber.ToString(),
                                FlightDepDate2 = relatedFlights[1].DepartureDate.ToString(),
                                FlightDepTime2 = relatedFlights[1].DepartureTime.ToString(),
                                FlightArrDate2 = relatedFlights[1].ArrivalDate.ToString(),
                                FlightArrTime2 = relatedFlights[1].ArrivalTime.ToString(),
                                Duration2 = relatedFlights[1].DurationDesc.ToString(),
                                Layover1 = relatedFlights[1].JourneyTimeDesc.ToString(),
                                FlightNumberCmb = relatedFlights[0].CarrierCode.ToString() + relatedFlights[0].FlightNumber.ToString()
                            });
                        }
                    }
                    else if (relatedFlights.Count == 3)
                    {
                        //commented on 21Jan2024 by cpk if ((ReturnCls(relatedFlights[0]).ToString() == ReturnCls(relatedFlights[1]).ToString()) && (ReturnCls(relatedFlights[0]).ToString() == ReturnCls(relatedFlights[2]).ToString()) && (ReturnCls(relatedFlights[1]).ToString() == ReturnCls(relatedFlights[2]).ToString()))
                        if ((ReturnProviderCd(relatedFlights[0]).ToString() == ReturnProviderCd(relatedFlights[1]).ToString()) && (ReturnProviderCd(relatedFlights[0]).ToString() == ReturnProviderCd(relatedFlights[2]).ToString()) && (ReturnProviderCd(relatedFlights[1]).ToString() == ReturnProviderCd(relatedFlights[2]).ToString()))   //added 21Jan2024 by cpk
                        {
                            rules = GetFareRules(relatedFlights);
                            if (relatedFlights[0].DepartureDate.ToString() != relatedFlights[2].ArrivalDate.ToString())
                            {
                                arrivalCheck = DateHelper.DayDiff(relatedFlights[0].DepartureDate.ToString(), relatedFlights[2].ArrivalDate.ToString());
                            }

                            FlightOutBound.Add(new k_ShowFlightOutBound()
                            {
                                Curr = curr,
                                AgentType = checkA_C,
                                CompanyID = CompanyID,
                                FlightRefid = relatedFlights[0].RefID.ToString(),

                                NoOFAdult = int.Parse(relatedFlights[0].Adt.ToString()),
                                NoOFChild = int.Parse(relatedFlights[0].Chd.ToString()),
                                NoOFInfant = int.Parse(relatedFlights[0].Inf.ToString()),

                                //==============================================================================================================================
                                Adt_BASIC = GetConvert(relatedFlights[0].Adt_BASIC.ToString()),
                                Chd_BASIC = GetConvert(relatedFlights[0].Chd_BASIC.ToString()),
                                Inf_BASIC = GetConvert(relatedFlights[0].Inf_BASIC.ToString()),

                                AdultbaseFare = int.Parse(relatedFlights[0].Adt.ToString()) * GetConvert(int.Parse(relatedFlights[0].Adt_BASIC.ToString())),
                                ChildbaseFare = int.Parse(relatedFlights[0].Chd.ToString()) * GetConvert(int.Parse(relatedFlights[0].Chd_BASIC.ToString())),
                                InfantbaseFare = int.Parse(relatedFlights[0].Inf.ToString()) * GetConvert(int.Parse(relatedFlights[0].Inf_BASIC.ToString())),

                                Adt_YQ = GetConvert(int.Parse(relatedFlights[0].Adt_YQ.ToString())),
                                AdtTotalTax = GetConvert(int.Parse(relatedFlights[0].AdtTotalTax.ToString())),
                                Chd_YQ = GetConvert(int.Parse(relatedFlights[0].Chd_YQ.ToString())),
                                ChdTotalTax = GetConvert(int.Parse(relatedFlights[0].ChdTotalTax.ToString())),
                                Inf_TAX = GetConvert(int.Parse(relatedFlights[0].Inf_TAX.ToString())),

                                Adt_AcuTax = ReturnTAXADT(relatedFlights[0]).ToString(),
                                Chd_AcuTax = ReturnTAXCHD(relatedFlights[0]).ToString(),
                                Inf_AcuTax = ReturnTAXINF(relatedFlights[0]).ToString(),

                                Yq = "Commission : " + String.Format("{0:N0}", GetConvert(int.Parse(relatedFlights[0].TotalCommission.ToString()))),
                                PromoChek = promo,

                                TotalTax = GetTotalTax(relatedFlights[0]),
                                TotalBasic = GetTotalBasic(relatedFlights[0]),
                                TotalServiceFee = GetTotalServiceFee(relatedFlights[0]),
                                TotalMarkUp = GetTotalMarkUp(relatedFlights[0]),
                                TotalServiceTax = GetTotalServiceTax(relatedFlights[0]),
                                TaxandCharges = GetTaxandCharges(relatedFlights[0]),
                                TotalCommission = ReturnCommission(relatedFlights[0], CompanyID),
                                TotalTds = ReturnTDS(relatedFlights[0], CompanyID),

                                GrossF = ReturnGrossFare(relatedFlights[0]).ToString(),
                                FinalFare = ReturnFinalFare(relatedFlights[0], CompanyID),
                                TotalAmount = ReturnTotalFareAmount(relatedFlights[0]),
                                TotalFare = ReturnTotalFareAmount(relatedFlights[0]),
                                //==============================================================================================================================

                                RuleTarrif = relatedFlights[0].RuleTarrif.ToString(),
                                FlightName = relatedFlights[0].CarrierCode.ToString(),
                                Cabin = relatedFlights[0].Cabin.ToString(),
                                CarrierName = relatedFlights[0].CarrierName.ToString(),
                                DepartStationName = relatedFlights[0].DepartureStationName.ToString(),
                                ArrivalStationName = relatedFlights[0].ArrivalStationName.ToString(),
                                DepartureStationAirport = relatedFlights[0].DepartureStationAirport.ToString(),
                                ArrivalStationAirport = relatedFlights[0].ArrivalStationAirport.ToString(),
                                FlightNumber = relatedFlights[0].FlightNumber.ToString(),
                                FlightDepDate = relatedFlights[0].DepartureDate.ToString(),
                                FlightDepTime = relatedFlights[0].DepartureTime.ToString(),
                                FlightArrDate = relatedFlights[2].ArrivalDate.ToString(),
                                FlightArrTime = relatedFlights[2].ArrivalTime.ToString(),
                                CHECKINBaggage = ReturnCheckINBaggage(relatedFlights[0]).ToString(),
                                CABINBaggage = ReturnCabinINBaggage(relatedFlights[0]).ToString(),
                                SRC = relatedFlights[0].Origin.ToString(),
                                DEST = relatedFlights[0].Destination.ToString(),
                                logo = "/assets/img/airlogo_square/" + relatedFlights[0].CarrierCode.ToString() + ".gif",
                                Duration = relatedFlights[0].DurationDesc.ToString(),
                                Stop = "TwoStop",
                                connection = 3,
                                ArrivalNextDayCheck = arrivalCheck,
                                PriceType = relatedFlights[0].PriceType.ToString(),
                                RefundType = ReturnRefundType(relatedFlights[0]).ToString(),
                                AvailableSeat = checkSeat(relatedFlights[0].SeatsAvailable.ToString()),
                                FareRules = rules,
                                FareRule = relatedFlights[0].FareRule.ToString(),
                                SMSACTIVES = 0,// SMSACTIVE,

                                Class1 = ReturnCls(relatedFlights[0]).ToString(),
                                SRC1 = relatedFlights[0].DepartureStation.ToString(),
                                DEST1 = relatedFlights[0].ArrivalStation.ToString(),
                                DepartStationName1 = relatedFlights[0].DepartureStationName.ToString(),
                                ArrivalStationName1 = relatedFlights[0].ArrivalStationName.ToString(),
                                DepartureStationAirport1 = relatedFlights[0].DepartureStationAirport.ToString(),
                                ArrivalStationAirport1 = relatedFlights[0].ArrivalStationAirport.ToString(),
                                Via = checkVia(relatedFlights[0].Via.ToString()),
                                ViaName = checkViaName(relatedFlights[0].ViaName.ToString()),
                                TerminalSRC = checkTerminal(relatedFlights[0].DepartureTerminal.ToString()),
                                TerminalDEST = checkTerminal(relatedFlights[0].ArrivalTerminal.ToString()),
                                FlightName1 = relatedFlights[0].CarrierCode.ToString(),
                                Cabin1 = relatedFlights[0].Cabin.ToString(),
                                CarrierName1 = relatedFlights[0].CarrierName.ToString(),
                                FlightNumber1 = relatedFlights[0].FlightNumber.ToString(),
                                FlightDepDate1 = relatedFlights[0].DepartureDate.ToString(),
                                FlightDepTime1 = relatedFlights[0].DepartureTime.ToString(),
                                FlightArrDate1 = relatedFlights[0].ArrivalDate.ToString(),
                                FlightArrTime1 = relatedFlights[0].ArrivalTime.ToString(),
                                Duration1 = relatedFlights[0].DurationDesc.ToString(),

                                Class2 = ReturnCls(relatedFlights[1]).ToString(),
                                SRC2 = relatedFlights[1].DepartureStation.ToString(),
                                DEST2 = relatedFlights[1].ArrivalStation.ToString(),
                                DepartStationName2 = relatedFlights[1].DepartureStationName.ToString(),
                                ArrivalStationName2 = relatedFlights[1].ArrivalStationName.ToString(),
                                DepartureStationAirport2 = relatedFlights[1].DepartureStationAirport.ToString(),
                                ArrivalStationAirport2 = relatedFlights[1].ArrivalStationAirport.ToString(),
                                Via1 = checkVia(relatedFlights[1].Via.ToString()),
                                ViaName1 = checkViaName(relatedFlights[1].ViaName.ToString()),
                                TerminalSRC1 = checkTerminal(relatedFlights[1].DepartureTerminal.ToString()),
                                TerminalDEST1 = checkTerminal(relatedFlights[1].ArrivalTerminal.ToString()),
                                logo1 = "/assets/img/airlogo_square/" + relatedFlights[1].CarrierCode.ToString() + ".gif",
                                FlightName2 = relatedFlights[1].CarrierCode.ToString(),
                                CarrierName2 = relatedFlights[1].CarrierName.ToString(),
                                Cabin2 = relatedFlights[1].Cabin.ToString(),
                                FlightNumber2 = relatedFlights[1].FlightNumber.ToString(),
                                FlightDepDate2 = relatedFlights[1].DepartureDate.ToString(),
                                FlightArrDate2 = relatedFlights[1].ArrivalDate.ToString(),
                                FlightArrTime2 = relatedFlights[1].ArrivalTime.ToString(),
                                Duration2 = relatedFlights[1].DurationDesc.ToString(),
                                Layover1 = relatedFlights[1].JourneyTimeDesc.ToString(),

                                Class3 = ReturnCls(relatedFlights[2]).ToString(),
                                SRC3 = relatedFlights[2].DepartureStation.ToString(),
                                DEST3 = relatedFlights[2].ArrivalStation.ToString(),
                                DepartStationName3 = relatedFlights[2].DepartureStationName.ToString(),
                                ArrivalStationName3 = relatedFlights[2].ArrivalStationName.ToString(),
                                DepartureStationAirport3 = relatedFlights[2].DepartureStationAirport.ToString(),
                                ArrivalStationAirport3 = relatedFlights[2].ArrivalStationAirport.ToString(),
                                Via2 = checkVia(relatedFlights[2].Via.ToString()),
                                ViaName2 = checkViaName(relatedFlights[2].ViaName.ToString()),
                                TerminalSRC2 = checkTerminal(relatedFlights[2].DepartureTerminal.ToString()),
                                TerminalDEST2 = checkTerminal(relatedFlights[2].ArrivalTerminal.ToString()),
                                logo2 = "/assets/img/airlogo_square/" + relatedFlights[2].CarrierCode.ToString() + ".gif",
                                FlightName3 = relatedFlights[2].CarrierCode.ToString(),
                                Cabin3 = relatedFlights[2].Cabin.ToString(),
                                CarrierName3 = relatedFlights[2].CarrierName.ToString(),
                                FlightNumber3 = relatedFlights[2].FlightNumber.ToString(),
                                FlightDepDate3 = relatedFlights[2].DepartureDate.ToString(),
                                FlightDepTime3 = relatedFlights[2].DepartureTime.ToString(),
                                FlightArrDate3 = relatedFlights[2].ArrivalDate.ToString(),
                                FlightArrTime3 = relatedFlights[2].ArrivalTime.ToString(),
                                Duration3 = relatedFlights[2].DurationDesc.ToString(),
                                Layover2 = relatedFlights[2].JourneyTimeDesc.ToString(),
                                FlightNumberCmb = relatedFlights[0].CarrierCode.ToString() + relatedFlights[0].FlightNumber.ToString() + "-" + relatedFlights[1].CarrierCode.ToString() + relatedFlights[1].FlightNumber.ToString() + "-" + relatedFlights[2].CarrierCode.ToString() + relatedFlights[2].FlightNumber.ToString()
                            });
                        }
                        else if (relatedFlights.Count == 4)
                        {

                        }
                    }
                }
                if (flterType == "OUTBOUND")
                {
                    var drSelect = flightList.Where(x => x.Stops == 2).ToList();
                    if (drSelect.Count > 0)
                    {
                        HttpContextHelper.Current.Session.SetString("StopCheck", "NonStop,OneStop,TwoStop");
                    }
                    else
                    {
                        drSelect = flightList.Where(x => x.Stops == 1).ToList();
                        if (drSelect.Count > 0)
                        {
                            HttpContextHelper.Current.Session.SetString("StopCheck", "NonStop,OneStop");
                        }
                        else
                        {
                            HttpContextHelper.Current.Session.SetString("StopCheck", "NonStop");
                        }
                    }
                }
                else if (flterType == "INBOUND")
                {
                    var drSelect = flightList.Where(x => x.Stops == 2).ToList();
                    if (drSelect.Count > 0)
                    {
                        HttpContextHelper.Current.Session.SetString("StopCheckR", "NonStop,OneStop,TwoStop");
                    }
                    else
                    {
                        drSelect = flightList.Where(x => x.Stops == 1).ToList();
                        if (drSelect.Count > 0)
                        {
                            HttpContextHelper.Current.Session.SetString("StopCheckR", "NonStop,OneStop");
                        }
                        else
                        {
                            HttpContextHelper.Current.Session.SetString("StopCheckR", "NonStop");
                        }
                    }
                }

            }

            var asc = (from sh in FlightOutBound
                       orderby sh.TotalAmount ascending
                       select sh).ToList();
            return asc;
        }
        public static Task<List<k_ShowFlightOutBound>> ResultDataAsync(string flterType, string SearchType, string CompanyID)
        {
            List<k_ShowFlightOutBound> FlightOutBound = new List<k_ShowFlightOutBound>();

            var flightList = new List<AirlineAvailabilityInfo>();
            if (flterType == "OUTBOUND")
            {
                flightList = GetSelec("O");
            }
            else if (flterType == "INBOUND")
            {
                flightList = GetSelec("I");
            }

            // Get distinct RefId values
            var distinctRefIds = flightList
                .GroupBy(x => x.RefID) // Replace RefId with the property representing RefId in your objects
                .Select(group => group.Key)
                .Where(refId => refId != 0)
                .ToList();

            if (distinctRefIds.Any())
            {
                string curr = "INR";
                // Check session or alternative storage for currency
                if (HttpContextHelper.Current?.Session.GetString("Curr") != null)
                {
                    curr = HttpContextHelper.Current.Session.GetString("Curr");
                }
                string rules = string.Empty;
                string defaultRule = string.Empty;
                int arrivalCheck = 0;
                int promo = 0;
                int checkA_C = 0;

                // Process each distinct RefId
                foreach (var refId in distinctRefIds)
                {
                    var relatedFlights = flightList
                        .Where(x => x.RefID == refId)
                        .ToList();

                    if (relatedFlights.Count == 1)
                    {
                        // Perform necessary processing on relatedFlights

                        var relatedFlight = relatedFlights.FirstOrDefault();
                        rules = GetFareRules(relatedFlights);
                        string departureDate = relatedFlight.DepartureDate.ToString(); // Assuming DepartureDate is a property of k_ShowFlightOutBound
                        string arrivalDate = relatedFlight.ArrivalDate.ToString();
                        if (departureDate != arrivalDate)
                        {
                            arrivalCheck = DateHelper.DayDiff(departureDate, arrivalDate);
                        }
                        FlightOutBound.Add(new k_ShowFlightOutBound()
                        {
                            Curr = curr,
                            AgentType = checkA_C,
                            CompanyID = CompanyID,
                            FlightRefid = relatedFlight.RefID.ToString(),

                            NoOFAdult = relatedFlight.Adt,
                            NoOFChild = relatedFlight.Chd,
                            NoOFInfant = relatedFlight.Inf,

                            Adt_BASIC = GetConvert(relatedFlight.Adt_BASIC.ToString()),
                            Chd_BASIC = GetConvert(relatedFlight.Chd_BASIC.ToString()),
                            Inf_BASIC = GetConvert(relatedFlight.Inf_BASIC.ToString()),

                            AdultbaseFare = relatedFlight.Adt * GetConvert(relatedFlight.Adt_BASIC),
                            ChildbaseFare = relatedFlight.Chd * GetConvert(relatedFlight.Chd_BASIC),
                            InfantbaseFare = relatedFlight.Inf * GetConvert(relatedFlight.Inf_BASIC),

                            Adt_YQ = GetConvert(relatedFlight.Adt_YQ),
                            AdtTotalTax = GetConvert(relatedFlight.AdtTotalTax),
                            Chd_YQ = GetConvert(relatedFlight.Chd_YQ),
                            ChdTotalTax = GetConvert(relatedFlight.ChdTotalTax),
                            Inf_TAX = GetConvert(relatedFlight.Inf_TAX),

                            Adt_AcuTax = ReturnTAXADT(relatedFlight).ToString(),
                            Chd_AcuTax = ReturnTAXCHD(relatedFlight).ToString(),
                            Inf_AcuTax = ReturnTAXINF(relatedFlight).ToString(),

                            Yq = "Commission : " + String.Format("{0:N0}", GetConvert(relatedFlight.TotalCommission)),
                            PromoChek = promo,

                            TotalTax = GetTotalTax(relatedFlight),
                            TotalBasic = GetTotalBasic(relatedFlight),
                            TotalServiceFee = GetTotalServiceFee(relatedFlight),
                            TotalMarkUp = GetTotalMarkUp(relatedFlight),
                            TotalServiceTax = GetTotalServiceTax(relatedFlight),
                            TaxandCharges = GetTaxandCharges(relatedFlight),
                            TotalCommission = ReturnCommission(relatedFlight, CompanyID),
                            TotalTds = ReturnTDS(relatedFlight, CompanyID),

                            GrossF = ReturnGrossFare(relatedFlight).ToString(),
                            FinalFare = ReturnFinalFare(relatedFlight, CompanyID),
                            TotalAmount = ReturnTotalFareAmount(relatedFlight),
                            TotalFare = ReturnTotalFareAmount(relatedFlight),

                            FlightName = relatedFlight.CarrierCode,
                            DepartStationName = relatedFlight.DepartureStationName,
                            ArrivalStationName = relatedFlight.ArrivalStationName,
                            DepartureStationAirport = relatedFlight.DepartureStationAirport,
                            ArrivalStationAirport = relatedFlight.ArrivalStationAirport,
                            Cabin = relatedFlight.Cabin,
                            CarrierName = relatedFlight.CarrierName,
                            FlightNumber = relatedFlight.FlightNumber,
                            FlightDepDate = relatedFlight.DepartureDate,
                            FlightDepTime = relatedFlight.DepartureTime,
                            FlightArrDate = relatedFlight.ArrivalDate,
                            FlightArrTime = relatedFlight.ArrivalTime,
                            CHECKINBaggage = ReturnCheckINBaggage(relatedFlight).ToString(),
                            CABINBaggage = ReturnCabinINBaggage(relatedFlight).ToString(),
                            SRC = relatedFlight.Origin,
                            DEST = relatedFlight.Destination,
                            PrimarySRC = relatedFlight.Origin,
                            PrimaryDEST = relatedFlight.Destination,
                            Stop = "NonStop",
                            logo = "/assets/img/airlogo_square/" + relatedFlight.CarrierCode + ".gif",
                            Duration = relatedFlight.DurationDesc,
                            connection = 1,
                            SMSACTIVES = 0,
                            ArrivalNextDayCheck = arrivalCheck,
                            PriceType = relatedFlight.PriceType,
                            RefundType = ReturnRefundType(relatedFlight).ToString(),
                            AvailableSeat = checkSeat(relatedFlight.SeatsAvailable.ToString()),
                            FareRules = rules,
                            RuleTarrif = relatedFlight.RuleTarrif ?? string.Empty,

                            Class1 = ReturnCls(relatedFlight),
                            SRC1 = relatedFlight.DepartureStation,
                            DEST1 = relatedFlight.ArrivalStation,
                            DepartStationName1 = relatedFlight.DepartureStationName,
                            ArrivalStationName1 = relatedFlight.ArrivalStationName,
                            DepartureStationAirport1 = relatedFlight.DepartureStationAirport,
                            ArrivalStationAirport1 = relatedFlight.ArrivalStationAirport,
                            Via = checkVia(relatedFlight.Via),
                            ViaName = checkViaName(relatedFlight.ViaName),
                            TerminalSRC = checkTerminal(relatedFlight.DepartureTerminal),
                            TerminalDEST = checkTerminal(relatedFlight.ArrivalTerminal),
                            FlightName1 = relatedFlight.CarrierCode,
                            Cabin1 = relatedFlight.Cabin,
                            CarrierName1 = relatedFlight.CarrierName,
                            FlightNumber1 = relatedFlight.FlightNumber,
                            FlightDepDate1 = relatedFlight.DepartureDate,
                            FlightDepTime1 = relatedFlight.DepartureTime,
                            FlightArrDate1 = relatedFlight.ArrivalDate,
                            FlightArrTime1 = relatedFlight.ArrivalTime,
                            Layover1 = relatedFlight.JourneyTimeDesc,
                            Duration1 = relatedFlight.DurationDesc,
                            FlightNumberCmb = relatedFlight.CarrierCode + relatedFlight.FlightNumber
                        });
                    }

                    else if (relatedFlights.Count == 2)
                    {
                        //commented on 21Jan2024 by cpk if (ReturnCls(relatedFlights[0]).ToString() == ReturnCls(relatedFlights[1]).ToString() )
                        if (ReturnProviderCd(relatedFlights[0]) == ReturnProviderCd(relatedFlights[1]))   //added 21Jan2024 by cpk
                        {
                            rules = GetFareRules(relatedFlights);
                            if (relatedFlights[0].DepartureDate.ToString() != relatedFlights[1].ArrivalDate.ToString())
                            {
                                arrivalCheck = DateHelper.DayDiff(relatedFlights[0].DepartureDate.ToString(), relatedFlights[1].ArrivalDate.ToString());
                            }
                            FlightOutBound.Add(new k_ShowFlightOutBound()
                            {
                                Curr = curr,
                                AgentType = checkA_C,
                                CompanyID = CompanyID,
                                FlightRefid = relatedFlights[0].RefID.ToString(),

                                NoOFAdult = relatedFlights[0].Adt,
                                NoOFChild = relatedFlights[0].Chd,
                                NoOFInfant = relatedFlights[0].Inf,
                                //==============================================================================================================================
                                Adt_BASIC = GetConvert(relatedFlights[0].Adt_BASIC).ToString(),
                                Chd_BASIC = GetConvert(relatedFlights[0].Chd_BASIC).ToString(),
                                Inf_BASIC = GetConvert(relatedFlights[0].Inf_BASIC).ToString(),

                                AdultbaseFare = relatedFlights[0].Adt * GetConvert(relatedFlights[0].Adt_BASIC),
                                ChildbaseFare = relatedFlights[0].Chd * GetConvert(relatedFlights[0].Chd_BASIC),
                                InfantbaseFare = relatedFlights[0].Inf * GetConvert(relatedFlights[0].Inf_BASIC),

                                Adt_YQ = GetConvert(relatedFlights[0].Adt_YQ),
                                AdtTotalTax = GetConvert(relatedFlights[0].AdtTotalTax),
                                Chd_YQ = GetConvert(relatedFlights[0].Chd_YQ),
                                ChdTotalTax = GetConvert(relatedFlights[0].ChdTotalTax),
                                Inf_TAX = GetConvert(relatedFlights[0].Inf_TAX),

                                Adt_AcuTax = ReturnTAXADT(relatedFlights[0]),
                                Chd_AcuTax = ReturnTAXCHD(relatedFlights[0]),
                                Inf_AcuTax = ReturnTAXINF(relatedFlights[0]),

                                Yq = "Commission : " + String.Format("{0:N0}", GetConvert(relatedFlights[0].TotalCommission)),
                                PromoChek = promo,

                                TotalTax = GetTotalTax(relatedFlights[0]),
                                TotalBasic = GetTotalBasic(relatedFlights[0]),
                                TotalServiceFee = GetTotalServiceFee(relatedFlights[0]),
                                TotalMarkUp = GetTotalMarkUp(relatedFlights[0]),
                                TotalServiceTax = GetTotalServiceTax(relatedFlights[0]),
                                TaxandCharges = GetTaxandCharges(relatedFlights[0]),
                                TotalCommission = ReturnCommission(relatedFlights[0], CompanyID),
                                TotalTds = ReturnTDS(relatedFlights[0], CompanyID),

                                GrossF = ReturnGrossFare(relatedFlights[0]),
                                FinalFare = ReturnFinalFare(relatedFlights[0], CompanyID),
                                TotalAmount = ReturnTotalFareAmount(relatedFlights[0]),
                                TotalFare = ReturnTotalFareAmount(relatedFlights[0]),
                                //==============================================================================================================================

                                FlightName = relatedFlights[0].CarrierCode,
                                CarrierName = relatedFlights[0].CarrierName,
                                DepartStationName = relatedFlights[0].DepartureStationName,
                                ArrivalStationName = relatedFlights[0].ArrivalStationName,
                                DepartureStationAirport = relatedFlights[0].DepartureStationAirport,
                                ArrivalStationAirport = relatedFlights[0].ArrivalStationAirport,
                                Cabin = relatedFlights[0].Cabin,
                                FlightNumber = relatedFlights[0].FlightNumber,
                                FlightDepDate = relatedFlights[0].DepartureDate,
                                FlightDepTime = relatedFlights[0].DepartureTime,
                                FlightArrDate = relatedFlights[1].ArrivalDate,
                                FlightArrTime = relatedFlights[1].ArrivalTime,
                                CHECKINBaggage = ReturnCheckINBaggage(relatedFlights[0]),
                                CABINBaggage = ReturnCabinINBaggage(relatedFlights[0]),
                                RuleTarrif = relatedFlights[0].RuleTarrif ?? string.Empty,
                                SRC = relatedFlights[0].Origin,
                                DEST = relatedFlights[0].Destination,
                                logo = "/assets/img/airlogo_square/" + relatedFlights[0].CarrierCode + ".gif",
                                Duration = relatedFlights[0].DurationDesc,
                                connection = 2,
                                SMSACTIVES = 0,// SMSACTIVE,
                                ArrivalNextDayCheck = arrivalCheck,
                                PriceType = relatedFlights[0].PriceType,
                                RefundType = ReturnRefundType(relatedFlights[0]),
                                AvailableSeat = checkSeat(relatedFlights[0].SeatsAvailable.ToString()),
                                FareRules = rules,
                                Stop = "OneStop",

                                Class1 = ReturnCls(relatedFlights[0]),
                                SRC1 = relatedFlights[0].DepartureStation,
                                DEST1 = relatedFlights[0].ArrivalStation,
                                DepartStationName1 = relatedFlights[0].DepartureStationName,
                                ArrivalStationName1 = relatedFlights[0].ArrivalStationName,
                                DepartureStationAirport1 = relatedFlights[0].DepartureStationAirport,
                                ArrivalStationAirport1 = relatedFlights[0].ArrivalStationAirport,
                                Via = checkVia(relatedFlights[0].Via),
                                ViaName = checkViaName(relatedFlights[0].ViaName),
                                TerminalSRC = checkTerminal(relatedFlights[0].DepartureTerminal),
                                TerminalDEST = checkTerminal(relatedFlights[0].ArrivalTerminal),
                                FlightName1 = relatedFlights[0].CarrierCode,
                                Cabin1 = relatedFlights[0].Cabin,
                                CarrierName1 = relatedFlights[0].CarrierName,
                                FlightNumber1 = relatedFlights[0].FlightNumber,
                                FlightDepDate1 = relatedFlights[0].DepartureDate,
                                FlightDepTime1 = relatedFlights[0].DepartureTime,
                                FlightArrDate1 = relatedFlights[0].ArrivalDate,
                                FlightArrTime1 = relatedFlights[0].ArrivalTime,
                                Duration1 = relatedFlights[0].DurationDesc,

                                Class2 = ReturnCls(relatedFlights[1]),
                                SRC2 = relatedFlights[1].DepartureStation,
                                DEST2 = relatedFlights[1].ArrivalStation,
                                DepartStationName2 = relatedFlights[1].DepartureStationName,
                                ArrivalStationName2 = relatedFlights[1].ArrivalStationName,
                                DepartureStationAirport2 = relatedFlights[1].DepartureStationAirport,
                                ArrivalStationAirport2 = relatedFlights[1].ArrivalStationAirport,
                                Via1 = checkVia(relatedFlights[1].Via),
                                ViaName1 = checkViaName(relatedFlights[1].ViaName),
                                TerminalSRC1 = checkTerminal(relatedFlights[1].DepartureTerminal),
                                TerminalDEST1 = checkTerminal(relatedFlights[1].ArrivalTerminal),
                                logo1 = "/assets/img/airlogo_square/" + relatedFlights[1].CarrierCode + ".gif",
                                FlightName2 = relatedFlights[1].CarrierCode,
                                Cabin2 = relatedFlights[1].Cabin,
                                CarrierName2 = relatedFlights[1].CarrierName,
                                FlightNumber2 = relatedFlights[1].FlightNumber,
                                FlightDepDate2 = relatedFlights[1].DepartureDate,
                                FlightDepTime2 = relatedFlights[1].DepartureTime,
                                FlightArrDate2 = relatedFlights[1].ArrivalDate,
                                FlightArrTime2 = relatedFlights[1].ArrivalTime,
                                Duration2 = relatedFlights[1].DurationDesc,
                                Layover1 = relatedFlights[1].JourneyTimeDesc,
                                FlightNumberCmb = relatedFlights[0].CarrierCode + relatedFlights[0].FlightNumber + "-" + relatedFlights[1].CarrierCode + relatedFlights[1].FlightNumber
                            });
                        }
                    }
                    else if (relatedFlights.Count == 3)
                    {
                        //commented on 21Jan2024 by cpk if ((ReturnCls(relatedFlights[0]).ToString() == ReturnCls(relatedFlights[1]).ToString()) && (ReturnCls(relatedFlights[0]).ToString() == ReturnCls(relatedFlights[2]).ToString()) && (ReturnCls(relatedFlights[1]).ToString() == ReturnCls(relatedFlights[2]).ToString()))
                        if ((ReturnProviderCd(relatedFlights[0]) == ReturnProviderCd(relatedFlights[1])) && (ReturnProviderCd(relatedFlights[0]) == ReturnProviderCd(relatedFlights[2])) && (ReturnProviderCd(relatedFlights[1]) == ReturnProviderCd(relatedFlights[2])))   //added 21Jan2024 by cpk
                        {
                            rules = GetFareRules(relatedFlights);
                            if (relatedFlights[0].DepartureDate.ToString() != relatedFlights[2].ArrivalDate.ToString())
                            {
                                arrivalCheck = DateHelper.DayDiff(relatedFlights[0].DepartureDate.ToString(), relatedFlights[2].ArrivalDate.ToString());
                            }

                            FlightOutBound.Add(new k_ShowFlightOutBound()
                            {
                                Curr = curr,
                                AgentType = checkA_C,
                                CompanyID = CompanyID,
                                FlightRefid = relatedFlights[0].RefID.ToString(),

                                NoOFAdult = int.Parse(relatedFlights[0].Adt.ToString()),
                                NoOFChild = int.Parse(relatedFlights[0].Chd.ToString()),
                                NoOFInfant = int.Parse(relatedFlights[0].Inf.ToString()),

                                //==============================================================================================================================
                                Adt_BASIC = GetConvert(relatedFlights[0].Adt_BASIC.ToString()),
                                Chd_BASIC = GetConvert(relatedFlights[0].Chd_BASIC.ToString()),
                                Inf_BASIC = GetConvert(relatedFlights[0].Inf_BASIC.ToString()),

                                AdultbaseFare = int.Parse(relatedFlights[0].Adt.ToString()) * GetConvert(int.Parse(relatedFlights[0].Adt_BASIC.ToString())),
                                ChildbaseFare = int.Parse(relatedFlights[0].Chd.ToString()) * GetConvert(int.Parse(relatedFlights[0].Chd_BASIC.ToString())),
                                InfantbaseFare = int.Parse(relatedFlights[0].Inf.ToString()) * GetConvert(int.Parse(relatedFlights[0].Inf_BASIC.ToString())),

                                Adt_YQ = GetConvert(relatedFlights[0].Adt_YQ),
                                AdtTotalTax = GetConvert(relatedFlights[0].AdtTotalTax),
                                Chd_YQ = GetConvert(relatedFlights[0].Chd_YQ),
                                ChdTotalTax = GetConvert(relatedFlights[0].ChdTotalTax),
                                Inf_TAX = GetConvert(relatedFlights[0].Inf_TAX),

                                Adt_AcuTax = ReturnTAXADT(relatedFlights[0]),
                                Chd_AcuTax = ReturnTAXCHD(relatedFlights[0]),
                                Inf_AcuTax = ReturnTAXINF(relatedFlights[0]),

                                Yq = "Commission : " + String.Format("{0:N0}", GetConvert(relatedFlights[0].TotalCommission)),
                                PromoChek = promo,

                                TotalTax = GetTotalTax(relatedFlights[0]),
                                TotalBasic = GetTotalBasic(relatedFlights[0]),
                                TotalServiceFee = GetTotalServiceFee(relatedFlights[0]),
                                TotalMarkUp = GetTotalMarkUp(relatedFlights[0]),
                                TotalServiceTax = GetTotalServiceTax(relatedFlights[0]),
                                TaxandCharges = GetTaxandCharges(relatedFlights[0]),
                                TotalCommission = ReturnCommission(relatedFlights[0], CompanyID),
                                TotalTds = ReturnTDS(relatedFlights[0], CompanyID),

                                GrossF = ReturnGrossFare(relatedFlights[0]),
                                FinalFare = ReturnFinalFare(relatedFlights[0], CompanyID),
                                TotalAmount = ReturnTotalFareAmount(relatedFlights[0]),
                                TotalFare = ReturnTotalFareAmount(relatedFlights[0]),
                                //==============================================================================================================================

                                RuleTarrif = relatedFlights[0].RuleTarrif ?? string.Empty,
                                FlightName = relatedFlights[0].CarrierCode,
                                Cabin = relatedFlights[0].Cabin,
                                CarrierName = relatedFlights[0].CarrierName,
                                DepartStationName = relatedFlights[0].DepartureStationName,
                                ArrivalStationName = relatedFlights[0].ArrivalStationName,
                                DepartureStationAirport = relatedFlights[0].DepartureStationAirport,
                                ArrivalStationAirport = relatedFlights[0].ArrivalStationAirport,
                                FlightNumber = relatedFlights[0].FlightNumber,
                                FlightDepDate = relatedFlights[0].DepartureDate,
                                FlightDepTime = relatedFlights[0].DepartureTime,
                                FlightArrDate = relatedFlights[2].ArrivalDate,
                                FlightArrTime = relatedFlights[2].ArrivalTime,
                                CHECKINBaggage = ReturnCheckINBaggage(relatedFlights[0]),
                                CABINBaggage = ReturnCabinINBaggage(relatedFlights[0]),
                                SRC = relatedFlights[0].Origin,
                                DEST = relatedFlights[0].Destination,
                                logo = "/assets/img/airlogo_square/" + relatedFlights[0].CarrierCode + ".gif",
                                Duration = relatedFlights[0].DurationDesc,
                                Stop = "TwoStop",
                                connection = 3,
                                ArrivalNextDayCheck = arrivalCheck,
                                PriceType = relatedFlights[0].PriceType,
                                RefundType = ReturnRefundType(relatedFlights[0]),
                                AvailableSeat = checkSeat(relatedFlights[0].SeatsAvailable.ToString()),
                                FareRules = rules,
                                FareRule = relatedFlights[0].FareRule,
                                SMSACTIVES = 0,// SMSACTIVE,

                                Class1 = ReturnCls(relatedFlights[0]),
                                SRC1 = relatedFlights[0].DepartureStation,
                                DEST1 = relatedFlights[0].ArrivalStation,
                                DepartStationName1 = relatedFlights[0].DepartureStationName,
                                ArrivalStationName1 = relatedFlights[0].ArrivalStationName,
                                DepartureStationAirport1 = relatedFlights[0].DepartureStationAirport,
                                ArrivalStationAirport1 = relatedFlights[0].ArrivalStationAirport,
                                Via = checkVia(relatedFlights[0].Via),
                                ViaName = checkViaName(relatedFlights[0].ViaName),
                                TerminalSRC = checkTerminal(relatedFlights[0].DepartureTerminal),
                                TerminalDEST = checkTerminal(relatedFlights[0].ArrivalTerminal),
                                FlightName1 = relatedFlights[0].CarrierCode,
                                Cabin1 = relatedFlights[0].Cabin,
                                CarrierName1 = relatedFlights[0].CarrierName,
                                FlightNumber1 = relatedFlights[0].FlightNumber,
                                FlightDepDate1 = relatedFlights[0].DepartureDate,
                                FlightDepTime1 = relatedFlights[0].DepartureTime,
                                FlightArrDate1 = relatedFlights[0].ArrivalDate,
                                FlightArrTime1 = relatedFlights[0].ArrivalTime,
                                Duration1 = relatedFlights[0].DurationDesc,

                                Class2 = ReturnCls(relatedFlights[1]),
                                SRC2 = relatedFlights[1].DepartureStation,
                                DEST2 = relatedFlights[1].ArrivalStation,
                                DepartStationName2 = relatedFlights[1].DepartureStationName,
                                ArrivalStationName2 = relatedFlights[1].ArrivalStationName,
                                DepartureStationAirport2 = relatedFlights[1].DepartureStationAirport,
                                ArrivalStationAirport2 = relatedFlights[1].ArrivalStationAirport,
                                Via1 = checkVia(relatedFlights[1].Via),
                                ViaName1 = checkViaName(relatedFlights[1].ViaName),
                                TerminalSRC1 = checkTerminal(relatedFlights[1].DepartureTerminal),
                                TerminalDEST1 = checkTerminal(relatedFlights[1].ArrivalTerminal),
                                logo1 = "/assets/img/airlogo_square/" + relatedFlights[1].CarrierCode + ".gif",
                                FlightName2 = relatedFlights[1].CarrierCode,
                                CarrierName2 = relatedFlights[1].CarrierName,
                                Cabin2 = relatedFlights[1].Cabin,
                                FlightNumber2 = relatedFlights[1].FlightNumber,
                                FlightDepDate2 = relatedFlights[1].DepartureDate,
                                FlightArrDate2 = relatedFlights[1].ArrivalDate,
                                FlightArrTime2 = relatedFlights[1].ArrivalTime,
                                Duration2 = relatedFlights[1].DurationDesc,
                                Layover1 = relatedFlights[1].JourneyTimeDesc,

                                Class3 = ReturnCls(relatedFlights[2]),
                                SRC3 = relatedFlights[2].DepartureStation,
                                DEST3 = relatedFlights[2].ArrivalStation,
                                DepartStationName3 = relatedFlights[2].DepartureStationName,
                                ArrivalStationName3 = relatedFlights[2].ArrivalStationName,
                                DepartureStationAirport3 = relatedFlights[2].DepartureStationAirport,
                                ArrivalStationAirport3 = relatedFlights[2].ArrivalStationAirport,
                                Via2 = checkVia(relatedFlights[2].Via),
                                ViaName2 = checkViaName(relatedFlights[2].ViaName),
                                TerminalSRC2 = checkTerminal(relatedFlights[2].DepartureTerminal),
                                TerminalDEST2 = checkTerminal(relatedFlights[2].ArrivalTerminal),
                                logo2 = "/assets/img/airlogo_square/" + relatedFlights[2].CarrierCode + ".gif",
                                FlightName3 = relatedFlights[2].CarrierCode,
                                Cabin3 = relatedFlights[2].Cabin,
                                CarrierName3 = relatedFlights[2].CarrierName,
                                FlightNumber3 = relatedFlights[2].FlightNumber,
                                FlightDepDate3 = relatedFlights[2].DepartureDate,
                                FlightDepTime3 = relatedFlights[2].DepartureTime,
                                FlightArrDate3 = relatedFlights[2].ArrivalDate,
                                FlightArrTime3 = relatedFlights[2].ArrivalTime,
                                Duration3 = relatedFlights[2].DurationDesc,
                                Layover2 = relatedFlights[2].JourneyTimeDesc,
                                FlightNumberCmb = relatedFlights[0].CarrierCode + relatedFlights[0].FlightNumber + "-" + relatedFlights[1].CarrierCode + relatedFlights[1].FlightNumber + "-" + relatedFlights[2].CarrierCode + relatedFlights[2].FlightNumber
                            });
                        }
                        else if (relatedFlights.Count == 4)
                        {

                        }
                    }
                }
                if (flterType == "OUTBOUND")
                {
                    var drSelect = flightList.Where(x => x.Stops == 2).ToList();
                    if (drSelect.Count > 0)
                    {
                        HttpContextHelper.Current.Session.SetString("StopCheck", "NonStop,OneStop,TwoStop");
                    }
                    else
                    {
                        drSelect = flightList.Where(x => x.Stops == 1).ToList();
                        if (drSelect.Count > 0)
                        {
                            HttpContextHelper.Current.Session.SetString("StopCheck", "NonStop,OneStop");
                        }
                        else
                        {
                            HttpContextHelper.Current.Session.SetString("StopCheck", "NonStop");
                        }
                    }
                }
                else if (flterType == "INBOUND")
                {
                    var drSelect = flightList.Where(x => x.Stops == 2).ToList();
                    if (drSelect.Count > 0)
                    {
                        HttpContextHelper.Current.Session.SetString("StopCheckR", "NonStop,OneStop,TwoStop");
                    }
                    else
                    {
                        drSelect = flightList.Where(x => x.Stops == 1).ToList();
                        if (drSelect.Count > 0)
                        {
                            HttpContextHelper.Current.Session.SetString("StopCheckR", "NonStop,OneStop");
                        }
                        else
                        {
                            HttpContextHelper.Current.Session.SetString("StopCheckR", "NonStop");
                        }
                    }
                }

            }

            var asc = (from sh in FlightOutBound
                       orderby sh.TotalAmount ascending
                       select sh).ToList();
            return Task.FromResult(asc);
        }

        public static Task<List<k_ShowFlightInternational>> ResultDataIntAsync(string flterType, string SearchType, string CompanyID)
        {
            List<k_ShowFlightInternational> FlightOutBound = new List<k_ShowFlightInternational>();
            var allFlightList = new List<AirlineAvailabilityInfo>();
            if (SearchType.Equals("INT"))
            {
                allFlightList = GetSelec();
            }

            var distinctRefIds = allFlightList
               .GroupBy(x => x.RefID) // Replace RefId with the property representing RefId in your objects
               .Select(group => group.Key)
               .Where(refId => refId != 0)
               .ToList();

            if (distinctRefIds.Any())
            {
                string curr = "INR";
                // Check session or alternative storage for currency
                if (HttpContextHelper.Current?.Session.GetString("Curr") != null)
                {
                    curr = HttpContextHelper.Current.Session.GetString("Curr");
                }

                string rules = string.Empty;
                string rulesIB = string.Empty;
                int arrvchack = 0;
                int arrvchackR = 0;
                int promo = 0;
                int ChekA_C = 0;
                //string CompanyID = Initializer.get_CompanyID();
                var flightList = GetSelec("O"); 
                var flightListInt = GetSelec("I");
                foreach (var refId in distinctRefIds)
                { 
                    arrvchack = 0;
                    arrvchackR = 0;
                    promo = 0;

                    var relatedFlights = flightList.Where(x => x.RefID == refId).ToList();
                    var relatedFlightsR = flightListInt.Where(x => x.RefID == refId).ToList();


                    #region
                    if (relatedFlights.Count == 1)
                    {
                        try
                        {
                            rules = GetFareRules(relatedFlights);
                            rulesIB = GetFareRules(relatedFlightsR);
                            string departureDate = relatedFlights[0].DepartureDate.ToString(); // Assuming DepartureDate is a property of k_ShowFlightOutBound
                            string arrivalDate = relatedFlights[0].ArrivalDate.ToString();
                            if (departureDate != arrivalDate)
                            {
                                arrvchack = DateHelper.DayDiff(departureDate, arrivalDate);
                            }

                            #region
                            if (relatedFlightsR.Count == 1)
                            {

                                string departureDateR = relatedFlightsR[0].DepartureDate.ToString(); // Assuming DepartureDate is a property of k_ShowFlightOutBound
                                string arrivalDateR = relatedFlightsR[0].ArrivalDate.ToString();
                                if (departureDateR != arrivalDateR)
                                {
                                    arrvchackR = DateHelper.DayDiff(departureDateR, arrivalDateR);
                                }

                                FlightOutBound.Add(new k_ShowFlightInternational()
                                {
                                    AgentType = ChekA_C,
                                    CompanyID = CompanyID,
                                    FlightRefid = relatedFlights[0].RefID.ToString(),
                                    Curr = curr,


                                    NoOFAdult = relatedFlights[0].Adt,
                                    NoOFChild = relatedFlights[0].Chd,
                                    NoOFInfant = relatedFlights[0].Inf,

                                    //==============================================================================================================================
                                    Adt_BASIC = GetConvert(relatedFlights[0].Adt_BASIC.ToString()),
                                    Chd_BASIC = GetConvert(relatedFlights[0].Chd_BASIC.ToString()),
                                    Inf_BASIC = GetConvert(relatedFlights[0].Inf_BASIC.ToString()),

                                    AdultbaseFare = relatedFlights[0].Adt * GetConvert(relatedFlights[0].Adt_BASIC),
                                    ChildbaseFare = relatedFlights[0].Chd * GetConvert(relatedFlights[0].Chd_BASIC),
                                    InfantbaseFare = relatedFlights[0].Inf * GetConvert(relatedFlights[0].Inf_BASIC),

                                    Adt_YQ = GetConvert(relatedFlights[0].Adt_YQ),
                                    AdtTotalTax = GetConvert(relatedFlights[0].AdtTotalTax),
                                    Chd_YQ = GetConvert(relatedFlights[0].Chd_YQ),
                                    ChdTotalTax = GetConvert(relatedFlights[0].ChdTotalTax),
                                    Inf_TAX = GetConvert(relatedFlights[0].Inf_TAX),

                                    Adt_AcuTax = ReturnTAXADT(relatedFlights[0]).ToString(),
                                    Chd_AcuTax = ReturnTAXCHD(relatedFlights[0]).ToString(),
                                    Inf_AcuTax = ReturnTAXINF(relatedFlights[0]).ToString(),

                                    Yq = "Commission : " + String.Format("{0:N0}", GetConvert(relatedFlights[0].TotalCommission)),
                                    PromoChek = promo,

                                    TotalAmount = ReturnTotalFareAmount(relatedFlights[0]),
                                    TotalFare = ReturnTotalFareAmount(relatedFlights[0]),
                                    FlightAmount = ReturnTotalFareAmount(relatedFlights[0]),
                                    TotalTax = GetTotalTax(relatedFlights[0]),
                                    TotalBasic = GetTotalBasic(relatedFlights[0]),
                                    TotalServiceFee = GetTotalServiceFee(relatedFlights[0]),
                                    TotalMarkup = GetTotalMarkUp(relatedFlights[0]),
                                    TotalServiceTax = GetTotalServiceTax(relatedFlights[0]),
                                    TaxandCharges = GetTaxandCharges(relatedFlights[0]),
                                    TotalCommission = ReturnCommission(relatedFlights[0], CompanyID),
                                    TotalTds = ReturnTDS(relatedFlights[0], CompanyID),
                                    FinalFare = ReturnFinalFare(relatedFlights[0], CompanyID),


                                    //==================================================================================================================

                                    CarrierName = relatedFlights[0].CarrierName,
                                    FlightName = relatedFlights[0].CarrierCode,
                                    FlightNumber = relatedFlights[0].FlightNumber,
                                    FlightDepDate = relatedFlights[0].DepartureDate,
                                    FlightDepTime = relatedFlights[0].DepartureTime,
                                    FlightArrDate = relatedFlights[0].ArrivalDate,
                                    FlightArrTime = relatedFlights[0].ArrivalTime,
                                    RuleTarrif = relatedFlights[0].RuleTarrif ?? string.Empty,

                                    CHECKINBaggage = ReturnCheckINBaggage(relatedFlights[0]).ToString(),
                                    CABINBaggage = ReturnCabinINBaggage(relatedFlights[0]).ToString(),
                                    SRC = relatedFlights[0].Origin,
                                    DEST = relatedFlights[0].Destination,
                                    Stop = "NonStop",
                                    Layover1 = relatedFlights[0].JourneyTimeDesc,

                                    logo = "/assets/img/airlogo_square/" + relatedFlights[0].CarrierCode + ".gif",
                                    Duration = relatedFlights[0].DurationDesc,
                                    connection = 1,
                                    ArrivalNextDayCheck = arrvchack,
                                    PriceType = relatedFlights[0].PriceType,
                                    RefundType = ReturnRefundType(relatedFlights[0]).ToString(),
                                    AvailableSeat = checkSeat(relatedFlights[0].SeatsAvailable.ToString()),
                                    FareRules = rules,


                                    SRC1 = relatedFlights[0].DepartureStation,
                                    DEST1 = relatedFlights[0].ArrivalStation,
                                    DepartStationName1 = relatedFlights[0].DepartureStationName,
                                    ArrivalStationName1 = relatedFlights[0].ArrivalStationName,
                                    DepartureStationAirport1 = relatedFlights[0].DepartureStationAirport,
                                    ArrivalStationAirport1 = relatedFlights[0].ArrivalStationAirport,
                                    Class1 = ReturnCls(relatedFlights[0]),
                                    Cabin1 = relatedFlights[0].Cabin,
                                    Via = checkVia(relatedFlights[0].Via),
                                    ViaName = checkViaName(relatedFlights[0].ViaName),
                                    TerminalSRC = checkTerminal(relatedFlights[0].DepartureTerminal),
                                    TerminalDEST = checkTerminal(relatedFlights[0].ArrivalTerminal),
                                    CarrierName1 = relatedFlights[0].CarrierName,
                                    FlightName1 = relatedFlights[0].CarrierCode,
                                    FlightNumber1 = relatedFlights[0].FlightNumber,
                                    FlightDepDate1 = relatedFlights[0].DepartureDate,
                                    FlightDepTime1 = relatedFlights[0].DepartureTime,
                                    FlightArrDate1 = relatedFlights[0].ArrivalDate,
                                    FlightArrTime1 = relatedFlights[0].ArrivalTime,
                                    Duration1 = relatedFlights[0].DurationDesc,


                                    CHECKINBaggageR = ReturnCheckINBaggage(relatedFlightsR[0]).ToString(),
                                    CABINBaggageR = ReturnCabinINBaggage(relatedFlightsR[0]).ToString(),
                                    FareRulesR = rulesIB,
                                    CarrierNameR = relatedFlightsR[0].CarrierName,
                                    FlightNameR = relatedFlightsR[0].CarrierCode,
                                    FlightNumberR = relatedFlightsR[0].FlightNumber,
                                    FlightDepDateR = relatedFlightsR[0].DepartureDate,
                                    FlightDepTimeR = relatedFlightsR[0].DepartureTime,
                                    FlightArrDateR = relatedFlightsR[0].ArrivalDate,
                                    FlightArrTimeR = relatedFlightsR[0].ArrivalTime,
                                    SRCR = relatedFlightsR[0].Origin,
                                    DESTR = relatedFlightsR[0].Destination,
                                    StopR = "NonStop",

                                    Layover1R = relatedFlightsR[0].JourneyTimeDesc,
                                    logoR = "/assets/img/airlogo_square/" + relatedFlightsR[0].CarrierCode + ".gif",
                                    DurationR = relatedFlightsR[0].DurationDesc,
                                    connectionR = 1,
                                    PriceTypeR = relatedFlightsR[0].PriceType,
                                    RefundTypeR = ReturnRefundType(relatedFlightsR[0]).ToString(),
                                    AvailableSeatR = checkSeat(relatedFlightsR[0].SeatsAvailable.ToString()),
                                    ArrivalNextDayCheckR = arrvchackR,


                                    DepartStationName1R = relatedFlightsR[0].DepartureStationName,
                                    ArrivalStationName1R = relatedFlightsR[0].ArrivalStationName,
                                    DepartureStationAirport1R = relatedFlightsR[0].DepartureStationAirport,
                                    ArrivalStationAirport1R = relatedFlightsR[0].ArrivalStationAirport,
                                    Class1R = ReturnCls(relatedFlightsR[0]).ToString(),
                                    Cabin1R = relatedFlightsR[0].Cabin,
                                    SRC1R = relatedFlightsR[0].DepartureStation,
                                    DEST1R = relatedFlightsR[0].ArrivalStation,
                                    ViaR = checkVia(relatedFlightsR[0].Via),
                                    ViaNameR = checkViaName(relatedFlightsR[0].ViaName),
                                    TerminalSRCR = checkTerminal(relatedFlightsR[0].DepartureTerminal),
                                    TerminalDESTR = checkTerminal(relatedFlightsR[0].ArrivalTerminal),
                                    FlightName1R = relatedFlightsR[0].CarrierCode,
                                    CarrierName1R = relatedFlightsR[0].CarrierName,
                                    FlightNumber1R = relatedFlightsR[0].FlightNumber,
                                    FlightDepDate1R = relatedFlightsR[0].DepartureDate,
                                    FlightDepTime1R = relatedFlightsR[0].DepartureTime,
                                    FlightArrDate1R = relatedFlightsR[0].ArrivalDate,
                                    FlightArrTime1R = relatedFlightsR[0].ArrivalTime,
                                    Duration1R = relatedFlightsR[0].DurationDesc
                                });
                            }
                            #endregion
                            #region
                            else if (relatedFlightsR.Count == 2)
                            {

                                string departureDateR = relatedFlightsR[0].DepartureDate.ToString(); // Assuming DepartureDate is a property of k_ShowFlightOutBound
                                string arrivalDateR = relatedFlightsR[1].ArrivalDate.ToString();
                                if (departureDateR != arrivalDateR)
                                {
                                    arrvchackR = DateHelper.DayDiff(departureDateR, arrivalDateR);
                                }

                                FlightOutBound.Add(new k_ShowFlightInternational()
                                {
                                    AgentType = ChekA_C,
                                    CompanyID = CompanyID,
                                    FlightRefid = relatedFlights[0].RefID.ToString(),
                                    Curr = curr,

                                    NoOFAdult = relatedFlights[0].Adt,
                                    NoOFChild = relatedFlights[0].Chd,
                                    NoOFInfant = relatedFlights[0].Inf,

                                    //==============================================================================================================================
                                    Adt_BASIC = GetConvert(relatedFlights[0].Adt_BASIC.ToString()),
                                    Chd_BASIC = GetConvert(relatedFlights[0].Chd_BASIC.ToString()),
                                    Inf_BASIC = GetConvert(relatedFlights[0].Inf_BASIC.ToString()),

                                    AdultbaseFare = relatedFlights[0].Adt * GetConvert(relatedFlights[0].Adt_BASIC),
                                    ChildbaseFare = relatedFlights[0].Chd * GetConvert(relatedFlights[0].Chd_BASIC),
                                    InfantbaseFare = relatedFlights[0].Inf * GetConvert(relatedFlights[0].Inf_BASIC),

                                    Adt_YQ = GetConvert(relatedFlights[0].Adt_YQ),
                                    AdtTotalTax = GetConvert(relatedFlights[0].AdtTotalTax),
                                    Chd_YQ = GetConvert(relatedFlights[0].Chd_YQ),
                                    ChdTotalTax = GetConvert(relatedFlights[0].ChdTotalTax),
                                    Inf_TAX = GetConvert(relatedFlights[0].Inf_TAX),

                                    Adt_AcuTax = ReturnTAXADT(relatedFlights[0]).ToString(),
                                    Chd_AcuTax = ReturnTAXCHD(relatedFlights[0]).ToString(),
                                    Inf_AcuTax = ReturnTAXINF(relatedFlights[0]).ToString(),

                                    Yq = "Commission : " + String.Format("{0:N0}", GetConvert(relatedFlights[0].TotalCommission)),
                                    PromoChek = promo,


                                    TotalAmount = ReturnTotalFareAmount(relatedFlights[0]),
                                    TotalFare = ReturnTotalFareAmount(relatedFlights[0]),
                                    FlightAmount = ReturnTotalFareAmount(relatedFlights[0]),
                                    TotalTax = GetTotalTax(relatedFlights[0]),
                                    TotalBasic = GetTotalBasic(relatedFlights[0]),
                                    TotalServiceFee = GetTotalServiceFee(relatedFlights[0]),
                                    TotalMarkup = GetTotalMarkUp(relatedFlights[0]),
                                    TotalServiceTax = GetTotalServiceTax(relatedFlights[0]),
                                    TaxandCharges = GetTaxandCharges(relatedFlights[0]),
                                    TotalCommission = ReturnCommission(relatedFlights[0], CompanyID),
                                    TotalTds = ReturnTDS(relatedFlights[0], CompanyID),
                                    FinalFare = ReturnFinalFare(relatedFlights[0], CompanyID),

                                    //==================================================================================================================

                                    RuleTarrif = relatedFlights[0].RuleTarrif ?? string.Empty,
                                    CarrierName = relatedFlights[0].CarrierName,
                                    FlightName = relatedFlights[0].CarrierCode,
                                    FlightNumber = relatedFlights[0].FlightNumber,
                                    FlightDepDate = relatedFlights[0].DepartureDate,
                                    FlightDepTime = relatedFlights[0].DepartureTime,
                                    FlightArrDate = relatedFlights[0].ArrivalDate,
                                    FlightArrTime = relatedFlights[0].ArrivalTime,


                                    CHECKINBaggage = ReturnCheckINBaggage(relatedFlights[0]).ToString(),
                                    CABINBaggage = ReturnCabinINBaggage(relatedFlights[0]).ToString(),
                                    SRC = relatedFlights[0].Origin,
                                    DEST = relatedFlights[0].Destination,
                                    ArrivalNextDayCheck = arrvchack,
                                    PriceType = relatedFlights[0].PriceType,
                                    RefundType = ReturnRefundType(relatedFlights[0]).ToString(),
                                    AvailableSeat = checkSeat(relatedFlights[0].SeatsAvailable.ToString()),
                                    FareRules = rules,
                                    Stop = "NonStop",
                                    Layover1 = relatedFlights[0].JourneyTimeDesc,
                                    logo = "/assets/img/airlogo_square/" + relatedFlights[0].CarrierCode + ".gif",
                                    Duration = relatedFlights[0].DurationDesc,
                                    connection = 1,

                                    DepartStationName1 = relatedFlights[0].DepartureStationName,
                                    ArrivalStationName1 = relatedFlights[0].ArrivalStationName,
                                    DepartureStationAirport1 = relatedFlights[0].DepartureStationAirport,
                                    ArrivalStationAirport1 = relatedFlights[0].ArrivalStationAirport,
                                    Class1 = ReturnCls(relatedFlights[0]),
                                    Cabin1 = relatedFlights[0].Cabin,
                                    SRC1 = relatedFlights[0].DepartureStation,
                                    DEST1 = relatedFlights[0].ArrivalStation,
                                    Via = checkVia(relatedFlights[0].Via),
                                    ViaName = checkViaName(relatedFlights[0].ViaName),
                                    TerminalSRC = checkTerminal(relatedFlights[0].DepartureTerminal),
                                    TerminalDEST = checkTerminal(relatedFlights[0].ArrivalTerminal),
                                    FlightName1 = relatedFlights[0].CarrierCode,
                                    CarrierName1 = relatedFlights[0].CarrierName,
                                    FlightNumber1 = relatedFlights[0].FlightNumber,
                                    FlightDepDate1 = relatedFlights[0].DepartureDate,
                                    FlightDepTime1 = relatedFlights[0].DepartureTime,
                                    FlightArrDate1 = relatedFlights[0].ArrivalDate,
                                    FlightArrTime1 = relatedFlights[0].ArrivalTime,
                                    Duration1 = relatedFlights[0].DurationDesc,


                                    CHECKINBaggageR = ReturnCheckINBaggage(relatedFlightsR[0]).ToString(),
                                    CABINBaggageR = ReturnCabinINBaggage(relatedFlightsR[0]).ToString(),
                                    FareRulesR = rulesIB,
                                    SRCR = relatedFlightsR[0].Origin,
                                    DESTR = relatedFlightsR[0].Destination,
                                    StopR = "OneStop",
                                    logoR = "/assets/img/airlogo_square/" + relatedFlightsR[0].CarrierCode + ".gif",
                                    DurationR = relatedFlightsR[0].DurationDesc,
                                    connectionR = 2,
                                    PriceTypeR = relatedFlightsR[0].PriceType,
                                    RefundTypeR = ReturnRefundType(relatedFlightsR[0]).ToString(),
                                    AvailableSeatR = checkSeat(relatedFlightsR[0].SeatsAvailable.ToString()),
                                    ArrivalNextDayCheckR = arrvchackR,
                                    CarrierNameR = relatedFlightsR[0].CarrierName,
                                    FlightNameR = relatedFlightsR[0].CarrierCode,
                                    FlightNumberR = relatedFlightsR[0].FlightNumber,
                                    FlightDepDateR = relatedFlightsR[0].DepartureDate,
                                    FlightDepTimeR = relatedFlightsR[0].DepartureTime,
                                    FlightArrDateR = relatedFlightsR[1].ArrivalDate,
                                    FlightArrTimeR = relatedFlightsR[1].ArrivalTime,

                                    Class1R = ReturnCls(relatedFlightsR[0]).ToString(),
                                    Cabin1R = relatedFlightsR[0].Cabin,
                                    SRC1R = relatedFlightsR[0].DepartureStation,
                                    DEST1R = relatedFlightsR[0].ArrivalStation,
                                    DepartStationName1R = relatedFlightsR[0].DepartureStationName,
                                    ArrivalStationName1R = relatedFlightsR[0].ArrivalStationName,
                                    DepartureStationAirport1R = relatedFlightsR[0].DepartureStationAirport,
                                    ArrivalStationAirport1R = relatedFlightsR[0].ArrivalStationAirport,
                                    ViaR = checkVia(relatedFlightsR[0].Via),
                                    ViaNameR = checkViaName(relatedFlightsR[0].ViaName),
                                    TerminalSRCR = checkTerminal(relatedFlightsR[0].DepartureTerminal),
                                    TerminalDESTR = checkTerminal(relatedFlightsR[0].ArrivalTerminal),
                                    CarrierName1R = relatedFlightsR[0].CarrierName,
                                    FlightName1R = relatedFlightsR[0].CarrierCode,
                                    FlightNumber1R = relatedFlightsR[0].FlightNumber,
                                    FlightDepDate1R = relatedFlightsR[0].DepartureDate,
                                    FlightDepTime1R = relatedFlightsR[0].DepartureTime,
                                    FlightArrDate1R = relatedFlightsR[0].ArrivalDate,
                                    FlightArrTime1R = relatedFlightsR[0].ArrivalTime,
                                    Duration1R = relatedFlightsR[0].DurationDesc,


                                    Class2R = ReturnCls(relatedFlightsR[1]).ToString(),
                                    Cabin2R = relatedFlightsR[1].Cabin,
                                    DepartStationName2R = relatedFlightsR[1].DepartureStationName,
                                    ArrivalStationName2R = relatedFlightsR[1].ArrivalStationName,
                                    DepartureStationAirport2R = relatedFlightsR[1].DepartureStationAirport,
                                    ArrivalStationAirport2R = relatedFlightsR[1].ArrivalStationAirport,
                                    SRC2R = relatedFlightsR[1].DepartureStation,
                                    DEST2R = relatedFlightsR[1].ArrivalStation,
                                    Via1R = checkVia(relatedFlightsR[1].Via),
                                    ViaName1R = checkViaName(relatedFlightsR[1].ViaName),
                                    TerminalSRC1R = checkTerminal(relatedFlightsR[1].DepartureTerminal),
                                    TerminalDEST1R = checkTerminal(relatedFlightsR[1].ArrivalTerminal),
                                    logo1R = "/assets/img/airlogo_square/" + relatedFlightsR[1].CarrierCode + ".gif",
                                    CarrierName2R = relatedFlightsR[1].CarrierName,
                                    FlightName2R = relatedFlightsR[1].CarrierCode,
                                    FlightNumber2R = relatedFlightsR[1].FlightNumber,
                                    FlightDepDate2R = relatedFlightsR[1].DepartureDate,
                                    FlightDepTime2R = relatedFlightsR[1].DepartureTime,
                                    FlightArrDate2R = relatedFlightsR[1].ArrivalDate,
                                    FlightArrTime2R = relatedFlightsR[1].ArrivalTime,
                                    Duration2R = relatedFlightsR[1].DurationDesc,
                                    Layover1R = relatedFlightsR[1].JourneyTimeDesc
                                    
                                });
                            }
                            #endregion
                            #region
                            else if (relatedFlightsR.Count == 3)
                            {

                                string departureDateR = relatedFlightsR[0].DepartureDate.ToString(); // Assuming DepartureDate is a property of k_ShowFlightOutBound
                                string arrivalDateR = relatedFlightsR[2].ArrivalDate.ToString();
                                if (departureDateR != arrivalDateR)
                                {
                                    arrvchackR = DateHelper.DayDiff(departureDateR, arrivalDateR);
                                }

                                FlightOutBound.Add(new k_ShowFlightInternational()
                                {
                                    AgentType = ChekA_C,
                                    CompanyID = CompanyID,
                                    FlightRefid = relatedFlights[0].RefID.ToString(),
                                    Curr = curr,


                                    NoOFAdult = relatedFlights[0].Adt,
                                    NoOFChild = relatedFlights[0].Chd,
                                    NoOFInfant = relatedFlights[0].Inf,

                                    //==============================================================================================================================
                                    Adt_BASIC = GetConvert(relatedFlights[0].Adt_BASIC.ToString()),
                                    Chd_BASIC = GetConvert(relatedFlights[0].Chd_BASIC.ToString()),
                                    Inf_BASIC = GetConvert(relatedFlights[0].Inf_BASIC.ToString()),

                                    AdultbaseFare = relatedFlights[0].Adt * GetConvert(relatedFlights[0].Adt_BASIC),
                                    ChildbaseFare = relatedFlights[0].Chd * GetConvert(relatedFlights[0].Chd_BASIC),
                                    InfantbaseFare = relatedFlights[0].Inf * GetConvert(relatedFlights[0].Inf_BASIC),

                                    Adt_YQ = GetConvert(relatedFlights[0].Adt_YQ),
                                    AdtTotalTax = GetConvert(relatedFlights[0].AdtTotalTax),
                                    Chd_YQ = GetConvert(relatedFlights[0].Chd_YQ),
                                    ChdTotalTax = GetConvert(relatedFlights[0].ChdTotalTax),
                                    Inf_TAX = GetConvert(relatedFlights[0].Inf_TAX),

                                    Adt_AcuTax = ReturnTAXADT(relatedFlights[0]).ToString(),
                                    Chd_AcuTax = ReturnTAXCHD(relatedFlights[0]).ToString(),
                                    Inf_AcuTax = ReturnTAXINF(relatedFlights[0]).ToString(),

                                    Yq = "Commission : " + String.Format("{0:N0}", GetConvert(relatedFlights[0].TotalCommission)),
                                    PromoChek = promo,

                                    TotalAmount = ReturnTotalFareAmount(relatedFlights[0]),
                                    TotalFare = ReturnTotalFareAmount(relatedFlights[0]),
                                    FlightAmount = ReturnTotalFareAmount(relatedFlights[0]),
                                    TotalTax = GetTotalTax(relatedFlights[0]),
                                    TotalBasic = GetTotalBasic(relatedFlights[0]),
                                    TotalServiceFee = GetTotalServiceFee(relatedFlights[0]),
                                    TotalMarkup = GetTotalMarkUp(relatedFlights[0]),
                                    TotalServiceTax = GetTotalServiceTax(relatedFlights[0]),
                                    TaxandCharges = GetTaxandCharges(relatedFlights[0]),
                                    TotalCommission = ReturnCommission(relatedFlights[0], CompanyID),
                                    TotalTds = ReturnTDS(relatedFlights[0], CompanyID),
                                    FinalFare = ReturnFinalFare(relatedFlights[0], CompanyID),

                                    //==================================================================================================================

                                    CarrierName = relatedFlights[0].CarrierName.ToString(),
                                    FlightName = relatedFlights[0].CarrierCode,
                                    FlightNumber = relatedFlights[0].FlightNumber,
                                    FlightDepDate = relatedFlights[0].DepartureDate,
                                    FlightDepTime = relatedFlights[0].DepartureTime,
                                    FlightArrDate = relatedFlights[0].ArrivalDate,
                                    FlightArrTime = relatedFlights[0].ArrivalTime,
                                    RuleTarrif = relatedFlights[0].RuleTarrif ?? string.Empty,
                                    CHECKINBaggage = ReturnCheckINBaggage(relatedFlights[0]).ToString(),
                                    CABINBaggage = ReturnCabinINBaggage(relatedFlights[0]).ToString(),
                                    SRC = relatedFlights[0].Origin,
                                    DEST = relatedFlights[0].Destination,
                                    Stop = "NonStop",
                                    Layover1 = relatedFlights[0].JourneyTimeDesc,
                                    logo = "/assets/img/airlogo_square/" + relatedFlights[0].CarrierCode + ".gif",
                                    Duration = relatedFlights[0].DurationDesc,
                                    connection = 1,
                                    ArrivalNextDayCheck = arrvchack,
                                    PriceType = relatedFlights[0].PriceType,
                                    RefundType = ReturnRefundType(relatedFlights[0]).ToString(),
                                    AvailableSeat = checkSeat(relatedFlights[0].SeatsAvailable.ToString()),
                                    FareRules = rules,

                                    DepartStationName1 = relatedFlights[0].DepartureStationName,
                                    ArrivalStationName1 = relatedFlights[0].ArrivalStationName,
                                    DepartureStationAirport1 = relatedFlights[0].DepartureStationAirport,
                                    ArrivalStationAirport1 = relatedFlights[0].ArrivalStationAirport,
                                    Class1 = ReturnCls(relatedFlights[0]),
                                    Cabin1 = relatedFlights[0].Cabin,
                                    SRC1 = relatedFlights[0].DepartureStation,
                                    DEST1 = relatedFlights[0].ArrivalStation,
                                    Via = checkVia(relatedFlights[0].Via),
                                    ViaName = checkViaName(relatedFlights[0].ViaName),
                                    TerminalSRC = checkTerminal(relatedFlights[0].DepartureTerminal),
                                    TerminalDEST = checkTerminal(relatedFlights[0].ArrivalTerminal),
                                    CarrierName1 = relatedFlights[0].CarrierName,
                                    FlightName1 = relatedFlights[0].CarrierCode,
                                    FlightNumber1 = relatedFlights[0].FlightNumber,
                                    FlightDepDate1 = relatedFlights[0].DepartureDate,
                                    FlightDepTime1 = relatedFlights[0].DepartureTime,
                                    FlightArrDate1 = relatedFlights[0].ArrivalDate,
                                    FlightArrTime1 = relatedFlights[0].ArrivalTime,
                                    Duration1 = relatedFlights[0].DurationDesc,

                                    CHECKINBaggageR = ReturnCheckINBaggage(relatedFlightsR[0]).ToString(),
                                    CABINBaggageR = ReturnCabinINBaggage(relatedFlightsR[0]).ToString(),
                                    FareRulesR = rulesIB,
                                    FlightNameR = relatedFlightsR[0].CarrierCode,
                                    FlightNumberR = relatedFlightsR[0].FlightNumber,
                                    FlightDepDateR = relatedFlightsR[0].DepartureDate,
                                    FlightDepTimeR = relatedFlightsR[0].DepartureTime,
                                    FlightArrDateR = relatedFlightsR[2].ArrivalDate,
                                    FlightArrTimeR = relatedFlightsR[2].ArrivalTime,
                                    SRCR = relatedFlightsR[0].Origin,
                                    DESTR = relatedFlightsR[0].Destination,
                                    StopR = "TwoStop",
                                    logoR = "/assets/img/airlogo_square/" + relatedFlightsR[0].CarrierCode + ".gif",
                                    DurationR = relatedFlightsR[0].DurationDesc,
                                    connectionR = 3,
                                    PriceTypeR = relatedFlightsR[0].PriceType,
                                    RefundTypeR = ReturnRefundType(relatedFlightsR[0]).ToString(),
                                    AvailableSeatR = checkSeat(relatedFlightsR[0].SeatsAvailable.ToString()),
                                    ArrivalNextDayCheckR = arrvchackR,

                                    Class1R = ReturnCls(relatedFlightsR[0]).ToString(),
                                    Cabin1R = relatedFlightsR[0].Cabin,
                                    SRC1R = relatedFlightsR[0].DepartureStation,
                                    DEST1R = relatedFlightsR[0].ArrivalStation,
                                    DepartStationName1R = relatedFlightsR[0].DepartureStationName,
                                    ArrivalStationName1R = relatedFlightsR[0].ArrivalStationName,
                                    DepartureStationAirport1R = relatedFlightsR[0].DepartureStationAirport,
                                    ArrivalStationAirport1R = relatedFlightsR[0].ArrivalStationAirport,
                                    ViaR = checkVia(relatedFlightsR[0].Via),
                                    ViaNameR = checkViaName(relatedFlightsR[0].ViaName),
                                    TerminalSRCR = checkTerminal(relatedFlightsR[0].DepartureTerminal),
                                    TerminalDESTR = checkTerminal(relatedFlightsR[0].ArrivalTerminal),
                                    FlightName1R = relatedFlightsR[0].CarrierCode,
                                    CarrierName1R = relatedFlightsR[0].CarrierName,
                                    FlightNumber1R = relatedFlightsR[0].FlightNumber,
                                    FlightDepDate1R = relatedFlightsR[0].DepartureDate,
                                    FlightDepTime1R = relatedFlightsR[0].DepartureTime,
                                    FlightArrDate1R = relatedFlightsR[0].ArrivalDate,
                                    FlightArrTime1R = relatedFlightsR[0].ArrivalTime,
                                    Duration1R = relatedFlightsR[0].DurationDesc,

                                    Class2R = ReturnCls(relatedFlightsR[1]).ToString(),
                                    Cabin2R = relatedFlightsR[1].Cabin,
                                    DepartStationName2R = relatedFlightsR[1].DepartureStationName,
                                    ArrivalStationName2R = relatedFlightsR[1].ArrivalStationName,
                                    DepartureStationAirport2R = relatedFlightsR[1].DepartureStationAirport,
                                    ArrivalStationAirport2R = relatedFlightsR[1].ArrivalStationAirport,
                                    SRC2R = relatedFlightsR[1].DepartureStation,
                                    DEST2R = relatedFlightsR[1].ArrivalStation,
                                    Via1R = checkVia(relatedFlightsR[1].Via),
                                    ViaName1R = checkViaName(relatedFlightsR[1].ViaName),
                                    TerminalSRC1R = checkTerminal(relatedFlightsR[1].DepartureTerminal),
                                    TerminalDEST1R = checkTerminal(relatedFlightsR[1].ArrivalTerminal),
                                    logo1R = "/assets/img/airlogo_square/" + relatedFlightsR[1].CarrierCode + ".gif",
                                    FlightName2R = relatedFlightsR[1].CarrierCode,
                                    CarrierName2R = relatedFlightsR[1].CarrierName,
                                    FlightNumber2R = relatedFlightsR[1].FlightNumber,
                                    FlightDepDate2R = relatedFlightsR[1].DepartureDate,
                                    FlightDepTime2R = relatedFlightsR[1].DepartureTime,
                                    FlightArrDate2R = relatedFlightsR[1].ArrivalDate,
                                    FlightArrTime2R = relatedFlightsR[1].ArrivalTime,
                                    Duration2R = relatedFlightsR[1].DurationDesc,
                                    Layover1R = relatedFlightsR[1].JourneyTimeDesc,

                                    Class3R = ReturnCls(relatedFlightsR[2]).ToString(),
                                    Cabin3R = relatedFlightsR[2].Cabin,
                                    DepartStationName3R = relatedFlightsR[2].DepartureStationName,
                                    ArrivalStationName3R = relatedFlightsR[2].ArrivalStationName,
                                    DepartureStationAirport3R = relatedFlightsR[2].DepartureStationAirport,
                                    ArrivalStationAirport3R = relatedFlightsR[2].ArrivalStationAirport,
                                    SRC3R = relatedFlightsR[2].DepartureStation,
                                    DEST3R = relatedFlightsR[2].ArrivalStation,
                                    Via2R = checkVia(relatedFlightsR[2].Via),
                                    ViaName2R = checkViaName(relatedFlightsR[2].ViaName),
                                    TerminalSRC2R = checkTerminal(relatedFlightsR[2].DepartureTerminal),
                                    TerminalDEST2R = checkTerminal(relatedFlightsR[2].ArrivalTerminal),
                                    logo2R = "/assets/img/airlogo_square/" + relatedFlightsR[2].CarrierCode + ".gif",
                                    FlightName3R = relatedFlightsR[2].CarrierCode,
                                    CarrierName3R = relatedFlightsR[2].CarrierName,
                                    FlightNumber3R = relatedFlightsR[2].FlightNumber,
                                    FlightDepDate3R = relatedFlightsR[2].DepartureDate,
                                    FlightDepTime3R = relatedFlightsR[2].DepartureTime,
                                    FlightArrDate3R = relatedFlightsR[2].ArrivalDate,
                                    FlightArrTime3R = relatedFlightsR[2].ArrivalTime,
                                    Duration3R = relatedFlightsR[2].DurationDesc,
                                    Layover2R = relatedFlightsR[2].JourneyTimeDesc
                                });
                            }
                            #endregion
                        }
                        catch (Exception ex)
                        {

                        }
                    }
                    #endregion
                    #region
                    else if (relatedFlights.Count == 2)
                    {
                        try
                        {
                            rules = GetFareRules(relatedFlights);
                            rulesIB = GetFareRules(relatedFlightsR);

                            string departureDate = relatedFlights[0].DepartureDate.ToString(); // Assuming DepartureDate is a property of k_ShowFlightOutBound
                            string arrivalDate = relatedFlights[1].ArrivalDate.ToString();
                            if (departureDate != arrivalDate)
                            {
                                arrvchack = DateHelper.DayDiff(departureDate, arrivalDate);
                            }
                            #region
                            if (relatedFlightsR.Count == 1)
                            {

                                string departureDateR = relatedFlightsR[0].DepartureDate.ToString(); // Assuming DepartureDate is a property of k_ShowFlightOutBound
                                string arrivalDateR = relatedFlightsR[0].ArrivalDate.ToString();
                                if (departureDateR != arrivalDateR)
                                {
                                    arrvchackR = DateHelper.DayDiff(departureDateR, arrivalDateR);
                                }

                                FlightOutBound.Add(new k_ShowFlightInternational()
                                {
                                    AgentType = ChekA_C,
                                    CompanyID = CompanyID,
                                    FlightRefid = relatedFlights[0].RefID.ToString(),
                                    Curr = curr,

                                    NoOFAdult = relatedFlights[0].Adt,
                                    NoOFChild = relatedFlights[0].Chd,
                                    NoOFInfant = relatedFlights[0].Inf,

                                    //==============================================================================================================================
                                    Adt_BASIC = GetConvert(relatedFlights[0].Adt_BASIC.ToString()),
                                    Chd_BASIC = GetConvert(relatedFlights[0].Chd_BASIC.ToString()),
                                    Inf_BASIC = GetConvert(relatedFlights[0].Inf_BASIC.ToString()),

                                    AdultbaseFare = relatedFlights[0].Adt * GetConvert(relatedFlights[0].Adt_BASIC),
                                    ChildbaseFare = relatedFlights[0].Chd * GetConvert(relatedFlights[0].Chd_BASIC),
                                    InfantbaseFare = relatedFlights[0].Inf * GetConvert(relatedFlights[0].Inf_BASIC),

                                    Adt_YQ = GetConvert(relatedFlights[0].Adt_YQ),
                                    AdtTotalTax = GetConvert(relatedFlights[0].AdtTotalTax),
                                    Chd_YQ = GetConvert(relatedFlights[0].Chd_YQ),
                                    ChdTotalTax = GetConvert(relatedFlights[0].ChdTotalTax),
                                    Inf_TAX = GetConvert(relatedFlights[0].Inf_TAX),

                                    Adt_AcuTax = ReturnTAXADT(relatedFlights[0]).ToString(),
                                    Chd_AcuTax = ReturnTAXCHD(relatedFlights[0]).ToString(),
                                    Inf_AcuTax = ReturnTAXINF(relatedFlights[0]).ToString(),

                                    Yq = "Commission : " + String.Format("{0:N0}", GetConvert(relatedFlights[0].TotalCommission)),
                                    PromoChek = promo,

                                    TotalAmount = ReturnTotalFareAmount(relatedFlights[0]),
                                    TotalFare = ReturnTotalFareAmount(relatedFlights[0]),
                                    FlightAmount = ReturnTotalFareAmount(relatedFlights[0]),
                                    TotalTax = GetTotalTax(relatedFlights[0]),
                                    TotalBasic = GetTotalBasic(relatedFlights[0]),
                                    TotalServiceFee = GetTotalServiceFee(relatedFlights[0]),
                                    TotalMarkup = GetTotalMarkUp(relatedFlights[0]),
                                    TotalServiceTax = GetTotalServiceTax(relatedFlights[0]),
                                    TaxandCharges = GetTaxandCharges(relatedFlights[0]),
                                    TotalCommission = ReturnCommission(relatedFlights[0], CompanyID),
                                    TotalTds = ReturnTDS(relatedFlights[0], CompanyID),
                                    FinalFare = ReturnFinalFare(relatedFlights[0], CompanyID),

                                    //==================================================================================================================

                                    RuleTarrif = relatedFlights[0].RuleTarrif ?? string.Empty,
                                    CarrierName = relatedFlights[0].CarrierName,
                                    FlightName = relatedFlights[0].CarrierCode,
                                    FlightNumber = relatedFlights[0].FlightNumber,
                                    FlightDepDate = relatedFlights[0].DepartureDate,
                                    FlightDepTime = relatedFlights[0].DepartureTime,
                                    FlightArrDate = relatedFlights[1].ArrivalDate,
                                    FlightArrTime = relatedFlights[1].ArrivalTime,


                                    CHECKINBaggage = ReturnCheckINBaggage(relatedFlights[0]).ToString(),
                                    CABINBaggage = ReturnCabinINBaggage(relatedFlights[0]).ToString(),
                                    SRC = relatedFlights[0].Origin,
                                    DEST = relatedFlights[0].Destination,
                                    logo = "/assets/img/airlogo_square/" + relatedFlights[0].CarrierCode + ".gif",
                                    Duration = relatedFlights[0].DurationDesc,
                                    connection = 2,
                                    ArrivalNextDayCheck = arrvchack,
                                    PriceType = relatedFlights[0].PriceType,
                                    RefundType = ReturnRefundType(relatedFlights[0]).ToString(),
                                    AvailableSeat = checkSeat(relatedFlights[0].SeatsAvailable.ToString()),
                                    FareRules = rules,
                                    Stop = "OneStop",

                                    SRC1 = relatedFlights[0].DepartureStation,
                                    DEST1 = relatedFlights[0].ArrivalStation,
                                    DepartStationName1 = relatedFlights[0].DepartureStationName,
                                    ArrivalStationName1 = relatedFlights[0].ArrivalStationName,
                                    DepartureStationAirport1 = relatedFlights[0].DepartureStationAirport,
                                    ArrivalStationAirport1 = relatedFlights[0].ArrivalStationAirport,
                                    Class1 = ReturnCls(relatedFlights[0]),
                                    Cabin1 = relatedFlights[0].Cabin,
                                    Via = checkVia(relatedFlights[0].Via),
                                    ViaName = checkViaName(relatedFlights[0].ViaName),
                                    TerminalSRC = checkTerminal(relatedFlights[0].DepartureTerminal),
                                    TerminalDEST = checkTerminal(relatedFlights[0].ArrivalTerminal),
                                    CarrierName1 = relatedFlights[0].CarrierName,
                                    FlightName1 = relatedFlights[0].CarrierCode,
                                    FlightNumber1 = relatedFlights[0].FlightNumber,
                                    FlightDepDate1 = relatedFlights[0].DepartureDate,
                                    FlightDepTime1 = relatedFlights[0].DepartureTime,
                                    FlightArrDate1 = relatedFlights[0].ArrivalDate,
                                    FlightArrTime1 = relatedFlights[0].ArrivalTime,
                                    Duration1 = relatedFlights[0].DurationDesc,

                                    DepartStationName2 = relatedFlights[1].DepartureStationName,
                                    ArrivalStationName2 = relatedFlights[1].ArrivalStationName,
                                    DepartureStationAirport2 = relatedFlights[1].DepartureStationAirport,
                                    ArrivalStationAirport2 = relatedFlights[1].ArrivalStationAirport,
                                    Class2 = ReturnCls(relatedFlights[1]),
                                    Cabin2 = relatedFlights[1].Cabin,
                                    SRC2 = relatedFlights[1].DepartureStation,
                                    DEST2 = relatedFlights[1].ArrivalStation,
                                    Via1 = checkVia(relatedFlights[1].Via),
                                    ViaName1 = checkViaName(relatedFlights[1].ViaName),
                                    TerminalSRC1 = checkTerminal(relatedFlights[1].DepartureTerminal),
                                    TerminalDEST1 = checkTerminal(relatedFlights[1].ArrivalTerminal),
                                    FlightName2 = relatedFlights[1].CarrierCode,
                                    CarrierName2 = relatedFlights[1].CarrierName,
                                    FlightNumber2 = relatedFlights[1].FlightNumber,
                                    FlightDepDate2 = relatedFlights[1].DepartureDate,
                                    FlightDepTime2 = relatedFlights[1].DepartureTime,
                                    FlightArrDate2 = relatedFlights[1].ArrivalDate,
                                    FlightArrTime2 = relatedFlights[1].ArrivalTime,
                                    Duration2 = relatedFlights[1].DurationDesc,
                                    Layover1 = relatedFlights[1].JourneyTimeDesc,


                                    CHECKINBaggageR = ReturnCheckINBaggage(relatedFlightsR[0]).ToString(),
                                    CABINBaggageR = ReturnCabinINBaggage(relatedFlightsR[0]).ToString(),
                                    FareRulesR = rulesIB,
                                    CarrierNameR = relatedFlightsR[0].CarrierName,
                                    FlightNameR = relatedFlightsR[0].CarrierCode,
                                    FlightNumberR = relatedFlightsR[0].FlightNumber,
                                    FlightDepDateR = relatedFlightsR[0].DepartureDate,
                                    FlightDepTimeR = relatedFlightsR[0].DepartureTime,
                                    FlightArrDateR = relatedFlightsR[0].ArrivalDate,
                                    FlightArrTimeR = relatedFlightsR[0].ArrivalTime,
                                    SRCR = relatedFlightsR[0].Origin,
                                    DESTR = relatedFlightsR[0].Destination,
                                    StopR = "NonStop",
                                    logoR = "/assets/img/airlogo_square/" + relatedFlightsR[0].CarrierCode + ".gif",
                                    DurationR = relatedFlightsR[0].DurationDesc,
                                    connectionR = 1,
                                    PriceTypeR = relatedFlightsR[0].PriceType,
                                    RefundTypeR = ReturnRefundType(relatedFlightsR[0]).ToString(),
                                    AvailableSeatR = checkSeat(relatedFlightsR[0].SeatsAvailable.ToString()),
                                    ArrivalNextDayCheckR = arrvchackR,
                                    Layover1R = relatedFlightsR[0].JourneyTimeDesc,

                                    DepartStationName1R = relatedFlightsR[0].DepartureStationName,
                                    ArrivalStationName1R = relatedFlightsR[0].ArrivalStationName,
                                    DepartureStationAirport1R = relatedFlightsR[0].DepartureStationAirport,
                                    ArrivalStationAirport1R = relatedFlightsR[0].ArrivalStationAirport,
                                    Class1R = ReturnCls(relatedFlightsR[0]).ToString(),
                                    Cabin1R = relatedFlightsR[0].Cabin,
                                    SRC1R = relatedFlightsR[0].DepartureStation,
                                    DEST1R = relatedFlightsR[0].ArrivalStation,
                                    ViaR = checkVia(relatedFlightsR[0].Via),
                                    ViaNameR = checkViaName(relatedFlightsR[0].ViaName),
                                    TerminalSRCR = checkTerminal(relatedFlightsR[0].DepartureTerminal),
                                    TerminalDESTR = checkTerminal(relatedFlightsR[0].ArrivalTerminal),
                                    CarrierName1R = relatedFlightsR[0].CarrierName,
                                    FlightName1R = relatedFlightsR[0].CarrierCode,
                                    FlightNumber1R = relatedFlightsR[0].FlightNumber,
                                    FlightDepDate1R = relatedFlightsR[0].DepartureDate,
                                    FlightDepTime1R = relatedFlightsR[0].DepartureTime,
                                    FlightArrDate1R = relatedFlightsR[0].ArrivalDate,
                                    FlightArrTime1R = relatedFlightsR[0].ArrivalTime,
                                    Duration1R = relatedFlightsR[0].DurationDesc
                                });
                            }
                            #endregion
                            #region
                            else if (relatedFlightsR.Count == 2)
                            {


                                string departureDateR = relatedFlightsR[0].DepartureDate.ToString(); // Assuming DepartureDate is a property of k_ShowFlightOutBound
                                string arrivalDateR = relatedFlightsR[1].ArrivalDate.ToString();
                                if (departureDateR != arrivalDateR)
                                {
                                    arrvchackR = DateHelper.DayDiff(departureDateR, arrivalDateR);
                                }

                                FlightOutBound.Add(new k_ShowFlightInternational()
                                {

                                    AgentType = ChekA_C,
                                    CompanyID = CompanyID,
                                    FlightRefid = relatedFlights[0].RefID.ToString(),
                                    Curr = curr,

                                    NoOFAdult = relatedFlights[0].Adt,
                                    NoOFChild = relatedFlights[0].Chd,
                                    NoOFInfant = relatedFlights[0].Inf,

                                    //==============================================================================================================================
                                    Adt_BASIC = GetConvert(relatedFlights[0].Adt_BASIC.ToString()),
                                    Chd_BASIC = GetConvert(relatedFlights[0].Chd_BASIC.ToString()),
                                    Inf_BASIC = GetConvert(relatedFlights[0].Inf_BASIC.ToString()),

                                    AdultbaseFare = relatedFlights[0].Adt * GetConvert(relatedFlights[0].Adt_BASIC),
                                    ChildbaseFare = relatedFlights[0].Chd * GetConvert(relatedFlights[0].Chd_BASIC),
                                    InfantbaseFare = relatedFlights[0].Inf * GetConvert(relatedFlights[0].Inf_BASIC),

                                    Adt_YQ = GetConvert(relatedFlights[0].Adt_YQ),
                                    AdtTotalTax = GetConvert(relatedFlights[0].AdtTotalTax),
                                    Chd_YQ = GetConvert(relatedFlights[0].Chd_YQ),
                                    ChdTotalTax = GetConvert(relatedFlights[0].ChdTotalTax),
                                    Inf_TAX = GetConvert(relatedFlights[0].Inf_TAX),

                                    Adt_AcuTax = ReturnTAXADT(relatedFlights[0]).ToString(),
                                    Chd_AcuTax = ReturnTAXCHD(relatedFlights[0]).ToString(),
                                    Inf_AcuTax = ReturnTAXINF(relatedFlights[0]).ToString(),

                                    Yq = "Commission : " + String.Format("{0:N0}", GetConvert(relatedFlights[0].TotalCommission)),
                                    PromoChek = promo,


                                    TotalAmount = ReturnTotalFareAmount(relatedFlights[0]),
                                    TotalFare = ReturnTotalFareAmount(relatedFlights[0]),
                                    FlightAmount = ReturnTotalFareAmount(relatedFlights[0]),
                                    TotalTax = GetTotalTax(relatedFlights[0]),
                                    TotalBasic = GetTotalBasic(relatedFlights[0]),
                                    TotalServiceFee = GetTotalServiceFee(relatedFlights[0]),
                                    TotalMarkup = GetTotalMarkUp(relatedFlights[0]),
                                    TotalServiceTax = GetTotalServiceTax(relatedFlights[0]),
                                    TaxandCharges = GetTaxandCharges(relatedFlights[0]),
                                    TotalCommission = ReturnCommission(relatedFlights[0], CompanyID),
                                    TotalTds = ReturnTDS(relatedFlights[0], CompanyID),
                                    FinalFare = ReturnFinalFare(relatedFlights[0], CompanyID),

                                    //==================================================================================================================


                                    RuleTarrif = relatedFlights[0].RuleTarrif ?? string.Empty,
                                    CarrierName = relatedFlights[0].CarrierName,
                                    FlightName = relatedFlights[0].CarrierCode,
                                    FlightNumber = relatedFlights[0].FlightNumber,
                                    FlightDepDate = relatedFlights[0].DepartureDate,
                                    FlightDepTime = relatedFlights[0].DepartureTime,
                                    FlightArrDate = relatedFlights[1].ArrivalDate,
                                    FlightArrTime = relatedFlights[1].ArrivalTime,

                                    CHECKINBaggage = ReturnCheckINBaggage(relatedFlights[0]).ToString(),
                                    CABINBaggage = ReturnCabinINBaggage(relatedFlights[0]).ToString(),
                                    SRC = relatedFlights[0].Origin,
                                    DEST = relatedFlights[0].Destination,
                                    logo = "/assets/img/airlogo_square/" + relatedFlights[0].CarrierCode + ".gif",
                                    Duration = relatedFlights[0].DurationDesc,
                                    connection = 2,
                                    ArrivalNextDayCheck = arrvchack,
                                    PriceType = relatedFlights[0].PriceType,
                                    RefundType = ReturnRefundType(relatedFlights[0]).ToString(),
                                    AvailableSeat = checkSeat(relatedFlights[0].SeatsAvailable.ToString()),
                                    FareRules = rules,
                                    Stop = "OneStop",


                                    DepartStationName1 = relatedFlights[0].DepartureStationName,
                                    ArrivalStationName1 = relatedFlights[0].ArrivalStationName,
                                    DepartureStationAirport1 = relatedFlights[0].DepartureStationAirport,
                                    ArrivalStationAirport1 = relatedFlights[0].ArrivalStationAirport,
                                    Class1 = ReturnCls(relatedFlights[0]),
                                    Cabin1 = relatedFlights[0].Cabin,
                                    SRC1 = relatedFlights[0].DepartureStation,
                                    DEST1 = relatedFlights[0].ArrivalStation,
                                    Via = checkVia(relatedFlights[0].Via),
                                    ViaName = checkViaName(relatedFlights[0].ViaName),
                                    TerminalSRC = checkTerminal(relatedFlights[0].DepartureTerminal),
                                    TerminalDEST = checkTerminal(relatedFlights[0].ArrivalTerminal),
                                    CarrierName1 = relatedFlights[0].CarrierName,
                                    FlightName1 = relatedFlights[0].CarrierCode,
                                    FlightNumber1 = relatedFlights[0].FlightNumber,
                                    FlightDepDate1 = relatedFlights[0].DepartureDate,
                                    FlightDepTime1 = relatedFlights[0].DepartureTime,
                                    FlightArrDate1 = relatedFlights[0].ArrivalDate,
                                    FlightArrTime1 = relatedFlights[0].ArrivalTime,
                                    Duration1 = relatedFlights[0].DurationDesc,


                                    DepartStationName2 = relatedFlights[1].DepartureStationName,
                                    ArrivalStationName2 = relatedFlights[1].ArrivalStationName,
                                    DepartureStationAirport2 = relatedFlights[1].DepartureStationAirport,
                                    ArrivalStationAirport2 = relatedFlights[1].ArrivalStationAirport,
                                    Class2 = ReturnCls(relatedFlights[1]),
                                    Cabin2 = relatedFlights[1].Cabin,
                                    SRC2 = relatedFlights[1].DepartureStation,
                                    DEST2 = relatedFlights[1].ArrivalStation,
                                    Via1 = checkVia(relatedFlights[1].Via),
                                    ViaName1 = checkViaName(relatedFlights[1].ViaName),
                                    TerminalSRC1 = checkTerminal(relatedFlights[1].DepartureTerminal),
                                    TerminalDEST1 = checkTerminal(relatedFlights[1].ArrivalTerminal),
                                    FlightName2 = relatedFlights[1].CarrierCode,
                                    CarrierName2 = relatedFlights[1].CarrierName,
                                    FlightNumber2 = relatedFlights[1].FlightNumber,
                                    FlightDepDate2 = relatedFlights[1].DepartureDate,
                                    FlightDepTime2 = relatedFlights[1].DepartureTime,
                                    FlightArrDate2 = relatedFlights[1].ArrivalDate,
                                    FlightArrTime2 = relatedFlights[1].ArrivalTime,
                                    Duration2 = relatedFlights[1].DurationDesc,
                                    Layover1 = relatedFlights[1].JourneyTimeDesc,


                                    CHECKINBaggageR = ReturnCheckINBaggage(relatedFlightsR[0]).ToString(),
                                    CABINBaggageR = ReturnCabinINBaggage(relatedFlightsR[0]).ToString(),
                                    FareRulesR = rulesIB,
                                    CarrierNameR = relatedFlightsR[0].CarrierName,
                                    FlightNameR = relatedFlightsR[0].CarrierCode,
                                    FlightNumberR = relatedFlightsR[0].FlightNumber,
                                    FlightDepDateR = relatedFlightsR[0].DepartureDate,
                                    FlightDepTimeR = relatedFlightsR[0].DepartureTime,
                                    FlightArrDateR = relatedFlightsR[1].ArrivalDate,
                                    FlightArrTimeR = relatedFlightsR[1].ArrivalTime,
                                    SRCR = relatedFlightsR[0].Origin,
                                    DESTR = relatedFlightsR[0].Destination,
                                    StopR = "OneStop",
                                    logoR = "/assets/img/airlogo_square/" + relatedFlightsR[0].CarrierCode + ".gif",
                                    DurationR = relatedFlightsR[0].DurationDesc,
                                    connectionR = 2,
                                    PriceTypeR = relatedFlightsR[0].PriceType,
                                    RefundTypeR = ReturnRefundType(relatedFlightsR[0]).ToString(),
                                    AvailableSeatR = checkSeat(relatedFlightsR[0].SeatsAvailable.ToString()),
                                    ArrivalNextDayCheckR = arrvchackR,

                                    Class1R = ReturnCls(relatedFlightsR[0]).ToString(),
                                    Cabin1R = relatedFlightsR[0].Cabin,
                                    SRC1R = relatedFlightsR[0].DepartureStation,
                                    DEST1R = relatedFlightsR[0].ArrivalStation,
                                    DepartStationName1R = relatedFlightsR[0].DepartureStationName,
                                    ArrivalStationName1R = relatedFlightsR[0].ArrivalStationName,
                                    DepartureStationAirport1R = relatedFlightsR[0].DepartureStationAirport,
                                    ArrivalStationAirport1R = relatedFlightsR[0].ArrivalStationAirport,
                                    ViaR = checkVia(relatedFlightsR[0].Via),
                                    ViaNameR = checkViaName(relatedFlightsR[0].ViaName),
                                    TerminalSRCR = checkTerminal(relatedFlightsR[0].DepartureTerminal),
                                    TerminalDESTR = checkTerminal(relatedFlightsR[0].ArrivalTerminal),
                                    FlightName1R = relatedFlightsR[0].CarrierCode,
                                    CarrierName1R = relatedFlightsR[0].CarrierName,
                                    FlightNumber1R = relatedFlightsR[0].FlightNumber,
                                    FlightDepDate1R = relatedFlightsR[0].DepartureDate,
                                    FlightDepTime1R = relatedFlightsR[0].DepartureTime,
                                    FlightArrDate1R = relatedFlightsR[0].ArrivalDate,
                                    FlightArrTime1R = relatedFlightsR[0].ArrivalTime,
                                    Duration1R = relatedFlightsR[0].DurationDesc,

                                    Class2R = ReturnCls(relatedFlightsR[1]).ToString(),
                                    Cabin2R = relatedFlightsR[1].Cabin,
                                    DepartStationName2R = relatedFlightsR[1].DepartureStationName,
                                    ArrivalStationName2R = relatedFlightsR[1].ArrivalStationName,
                                    DepartureStationAirport2R = relatedFlightsR[1].DepartureStationAirport,
                                    ArrivalStationAirport2R = relatedFlightsR[1].ArrivalStationAirport,
                                    SRC2R = relatedFlightsR[1].DepartureStation,
                                    DEST2R = relatedFlightsR[1].ArrivalStation,
                                    Via1R = checkVia(relatedFlightsR[1].Via),
                                    ViaName1R = checkViaName(relatedFlightsR[1].ViaName),
                                    TerminalSRC1R = checkTerminal(relatedFlightsR[1].DepartureTerminal),
                                    TerminalDEST1R = checkTerminal(relatedFlightsR[1].ArrivalTerminal),
                                    logo1R = "/assets/img/airlogo_square/" + relatedFlightsR[1].CarrierCode + ".gif",
                                    FlightName2R = relatedFlightsR[1].CarrierCode,
                                    CarrierName2R = relatedFlightsR[1].CarrierName,
                                    FlightNumber2R = relatedFlightsR[1].FlightNumber,
                                    FlightDepDate2R = relatedFlightsR[1].DepartureDate,
                                    FlightDepTime2R = relatedFlightsR[1].DepartureTime,
                                    FlightArrDate2R = relatedFlightsR[1].ArrivalDate,
                                    FlightArrTime2R = relatedFlightsR[1].ArrivalTime,
                                    Duration2R = relatedFlightsR[1].DurationDesc,
                                    Layover1R = relatedFlightsR[1].JourneyTimeDesc
                                });
                            }
                            #endregion
                            #region
                            else if (relatedFlightsR.Count == 3)
                            {
                                string departureDateR = relatedFlightsR[0].DepartureDate.ToString(); // Assuming DepartureDate is a property of k_ShowFlightOutBound
                                string arrivalDateR = relatedFlightsR[2].ArrivalDate.ToString();
                                if (departureDateR != arrivalDateR)
                                {
                                    arrvchackR = DateHelper.DayDiff(departureDateR, arrivalDateR);
                                }

                                FlightOutBound.Add(new k_ShowFlightInternational()
                                {
                                    AgentType = ChekA_C,
                                    CompanyID = CompanyID,
                                    FlightRefid = relatedFlights[0].RefID.ToString(),
                                    Curr = curr,

                                    NoOFAdult = relatedFlights[0].Adt,
                                    NoOFChild = relatedFlights[0].Chd,
                                    NoOFInfant = relatedFlights[0].Inf,

                                    //==============================================================================================================================
                                    Adt_BASIC = GetConvert(relatedFlights[0].Adt_BASIC.ToString()),
                                    Chd_BASIC = GetConvert(relatedFlights[0].Chd_BASIC.ToString()),
                                    Inf_BASIC = GetConvert(relatedFlights[0].Inf_BASIC.ToString()),

                                    AdultbaseFare = relatedFlights[0].Adt * GetConvert(relatedFlights[0].Adt_BASIC),
                                    ChildbaseFare = relatedFlights[0].Chd * GetConvert(relatedFlights[0].Chd_BASIC),
                                    InfantbaseFare = relatedFlights[0].Inf * GetConvert(relatedFlights[0].Inf_BASIC),

                                    Adt_YQ = GetConvert(relatedFlights[0].Adt_YQ),
                                    AdtTotalTax = GetConvert(relatedFlights[0].AdtTotalTax),
                                    Chd_YQ = GetConvert(relatedFlights[0].Chd_YQ),
                                    ChdTotalTax = GetConvert(relatedFlights[0].ChdTotalTax),
                                    Inf_TAX = GetConvert(relatedFlights[0].Inf_TAX),

                                    Adt_AcuTax = ReturnTAXADT(relatedFlights[0]).ToString(),
                                    Chd_AcuTax = ReturnTAXCHD(relatedFlights[0]).ToString(),
                                    Inf_AcuTax = ReturnTAXINF(relatedFlights[0]).ToString(),

                                    Yq = "Commission : " + String.Format("{0:N0}", GetConvert(relatedFlights[0].TotalCommission)),
                                    PromoChek = promo,

                                    TotalAmount = ReturnTotalFareAmount(relatedFlights[0]),
                                    TotalFare = ReturnTotalFareAmount(relatedFlights[0]),
                                    FlightAmount = ReturnTotalFareAmount(relatedFlights[0]),
                                    TotalTax = GetTotalTax(relatedFlights[0]),
                                    TotalBasic = GetTotalBasic(relatedFlights[0]),
                                    TotalServiceFee = GetTotalServiceFee(relatedFlights[0]),
                                    TotalMarkup = GetTotalMarkUp(relatedFlights[0]),
                                    TotalServiceTax = GetTotalServiceTax(relatedFlights[0]),
                                    TaxandCharges = GetTaxandCharges(relatedFlights[0]),
                                    TotalCommission = ReturnCommission(relatedFlights[0], CompanyID),
                                    TotalTds = ReturnTDS(relatedFlights[0], CompanyID),
                                    FinalFare = ReturnFinalFare(relatedFlights[0], CompanyID),

                                    //==================================================================================================================

                                    CarrierName = relatedFlights[0].CarrierName.ToString(),
                                    FlightName = relatedFlights[0].CarrierCode,
                                    FlightNumber = relatedFlights[0].FlightNumber,
                                    FlightDepDate = relatedFlights[0].DepartureDate,
                                    FlightDepTime = relatedFlights[0].DepartureTime,
                                    FlightArrDate = relatedFlights[1].ArrivalDate,
                                    FlightArrTime = relatedFlights[1].ArrivalTime,
                                    RuleTarrif = relatedFlights[0].RuleTarrif ?? string.Empty,
                                    CHECKINBaggage = ReturnCheckINBaggage(relatedFlights[0]).ToString(),
                                    CABINBaggage = ReturnCabinINBaggage(relatedFlights[0]).ToString(),
                                    SRC = relatedFlights[0].Origin,
                                    DEST = relatedFlights[0].Destination,
                                    Stop = "OneStop",
                                    logo = "/assets/img/airlogo_square/" + relatedFlights[0].CarrierCode + ".gif",
                                    Duration = relatedFlights[0].DurationDesc,
                                    connection = 2,
                                    ArrivalNextDayCheck = arrvchack,
                                    PriceType = relatedFlights[0].PriceType,
                                    RefundType = ReturnRefundType(relatedFlights[0]).ToString(),
                                    AvailableSeat = checkSeat(relatedFlights[0].SeatsAvailable.ToString()),
                                    FareRules = rules,

                                    DepartStationName1 = relatedFlights[0].DepartureStationName,
                                    ArrivalStationName1 = relatedFlights[0].ArrivalStationName,
                                    DepartureStationAirport1 = relatedFlights[0].DepartureStationAirport,
                                    ArrivalStationAirport1 = relatedFlights[0].ArrivalStationAirport,
                                    Class1 = ReturnCls(relatedFlights[0]),
                                    Cabin1 = relatedFlights[0].Cabin,
                                    SRC1 = relatedFlights[0].DepartureStation,
                                    DEST1 = relatedFlights[0].ArrivalStation,
                                    Via = checkVia(relatedFlights[0].Via),
                                    ViaName = checkViaName(relatedFlights[0].ViaName),
                                    TerminalSRC = checkTerminal(relatedFlights[0].DepartureTerminal),
                                    TerminalDEST = checkTerminal(relatedFlights[0].ArrivalTerminal),
                                    CarrierName1 = relatedFlights[0].CarrierName,
                                    FlightName1 = relatedFlights[0].CarrierCode,
                                    FlightNumber1 = relatedFlights[0].FlightNumber,
                                    FlightDepDate1 = relatedFlights[0].DepartureDate,
                                    FlightDepTime1 = relatedFlights[0].DepartureTime,
                                    FlightArrDate1 = relatedFlights[0].ArrivalDate,
                                    FlightArrTime1 = relatedFlights[0].ArrivalTime,
                                    Duration1 = relatedFlights[0].DurationDesc,

                                    DepartStationName2 = relatedFlights[1].DepartureStationName,
                                    ArrivalStationName2 = relatedFlights[1].ArrivalStationName,
                                    DepartureStationAirport2 = relatedFlights[1].DepartureStationAirport,
                                    ArrivalStationAirport2 = relatedFlights[1].ArrivalStationAirport,
                                    Class2 = ReturnCls(relatedFlights[1]),
                                    Cabin2 = relatedFlights[1].Cabin,
                                    SRC2 = relatedFlights[1].DepartureStation,
                                    DEST2 = relatedFlights[1].ArrivalStation,
                                    Via1 = checkVia(relatedFlights[1].Via),
                                    ViaName1 = checkViaName(relatedFlights[1].ViaName),
                                    TerminalSRC1 = checkTerminal(relatedFlights[1].DepartureTerminal),
                                    TerminalDEST1 = checkTerminal(relatedFlights[1].ArrivalTerminal),
                                    FlightName2 = relatedFlights[1].CarrierCode,
                                    CarrierName2 = relatedFlights[1].CarrierName,
                                    FlightNumber2 = relatedFlights[1].FlightNumber,
                                    FlightDepDate2 = relatedFlights[1].DepartureDate,
                                    FlightDepTime2 = relatedFlights[1].DepartureTime,
                                    FlightArrDate2 = relatedFlights[1].ArrivalDate,
                                    FlightArrTime2 = relatedFlights[1].ArrivalTime,
                                    Duration2 = relatedFlights[1].DurationDesc,
                                    Layover1 = relatedFlights[1].JourneyTimeDesc,


                                    CHECKINBaggageR = ReturnCheckINBaggage(relatedFlightsR[0]).ToString(),
                                    CABINBaggageR = ReturnCabinINBaggage(relatedFlightsR[0]).ToString(),
                                    FareRulesR = rulesIB,
                                    CarrierNameR = relatedFlightsR[0].CarrierName,
                                    FlightNameR = relatedFlightsR[0].CarrierCode,
                                    FlightNumberR = relatedFlightsR[0].FlightNumber,
                                    FlightDepDateR = relatedFlightsR[0].DepartureDate,
                                    FlightDepTimeR = relatedFlightsR[0].DepartureTime,
                                    FlightArrDateR = relatedFlightsR[2].ArrivalDate,
                                    FlightArrTimeR = relatedFlightsR[2].ArrivalTime,
                                    SRCR = relatedFlightsR[0].Origin,
                                    DESTR = relatedFlightsR[0].Destination,
                                    StopR = "TwoStop",
                                    logoR = "/assets/img/airlogo_square/" + relatedFlightsR[0].CarrierCode + ".gif",
                                    DurationR = relatedFlightsR[0].DurationDesc,
                                    connectionR = 3,
                                    PriceTypeR = relatedFlightsR[0].PriceType,
                                    RefundTypeR = ReturnRefundType(relatedFlightsR[0]).ToString(),
                                    AvailableSeatR = checkSeat(relatedFlightsR[0].SeatsAvailable.ToString()),
                                    ArrivalNextDayCheckR = arrvchackR,

                                    Class1R = ReturnCls(relatedFlightsR[0]).ToString(),
                                    Cabin1R = relatedFlightsR[0].Cabin,
                                    SRC1R = relatedFlightsR[0].DepartureStation,
                                    DEST1R = relatedFlightsR[0].ArrivalStation,
                                    DepartStationName1R = relatedFlightsR[0].DepartureStationName,
                                    ArrivalStationName1R = relatedFlightsR[0].ArrivalStationName,
                                    DepartureStationAirport1R = relatedFlightsR[0].DepartureStationAirport,
                                    ArrivalStationAirport1R = relatedFlightsR[0].ArrivalStationAirport,
                                    ViaR = checkVia(relatedFlightsR[0].Via),
                                    ViaNameR = checkViaName(relatedFlightsR[0].ViaName),
                                    TerminalSRCR = checkTerminal(relatedFlightsR[0].DepartureTerminal),
                                    TerminalDESTR = checkTerminal(relatedFlightsR[0].ArrivalTerminal),
                                    FlightName1R = relatedFlightsR[0].CarrierCode,
                                    CarrierName1R = relatedFlightsR[0].CarrierName,
                                    FlightNumber1R = relatedFlightsR[0].FlightNumber,
                                    FlightDepDate1R = relatedFlightsR[0].DepartureDate,
                                    FlightDepTime1R = relatedFlightsR[0].DepartureTime,
                                    FlightArrDate1R = relatedFlightsR[0].ArrivalDate,
                                    FlightArrTime1R = relatedFlightsR[0].ArrivalTime,
                                    Duration1R = relatedFlightsR[0].DurationDesc,

                                    Class2R = ReturnCls(relatedFlightsR[1]).ToString(),
                                    Cabin2R = relatedFlightsR[1].Cabin,
                                    DepartStationName2R = relatedFlightsR[1].DepartureStationName,
                                    ArrivalStationName2R = relatedFlightsR[1].ArrivalStationName,
                                    DepartureStationAirport2R = relatedFlightsR[1].DepartureStationAirport,
                                    ArrivalStationAirport2R = relatedFlightsR[1].ArrivalStationAirport,
                                    SRC2R = relatedFlightsR[1].DepartureStation,
                                    DEST2R = relatedFlightsR[1].ArrivalStation,
                                    Via1R = checkVia(relatedFlightsR[1].Via),
                                    ViaName1R = checkViaName(relatedFlightsR[1].ViaName),
                                    TerminalSRC1R = checkTerminal(relatedFlightsR[1].DepartureTerminal),
                                    TerminalDEST1R = checkTerminal(relatedFlightsR[1].ArrivalTerminal),
                                    logo1R = "/assets/img/airlogo_square/" + relatedFlightsR[1].CarrierCode + ".gif",
                                    FlightName2R = relatedFlightsR[1].CarrierCode,
                                    CarrierName2R = relatedFlightsR[1].CarrierName,
                                    FlightNumber2R = relatedFlightsR[1].FlightNumber,
                                    FlightDepDate2R = relatedFlightsR[1].DepartureDate,
                                    FlightDepTime2R = relatedFlightsR[1].DepartureTime,
                                    FlightArrDate2R = relatedFlightsR[1].ArrivalDate,
                                    FlightArrTime2R = relatedFlightsR[1].ArrivalTime,
                                    Duration2R = relatedFlightsR[1].DurationDesc,
                                    Layover1R = relatedFlightsR[1].JourneyTimeDesc,

                                     Class3R = ReturnCls(relatedFlightsR[2]).ToString(),
                                    Cabin3R = relatedFlightsR[2].Cabin,
                                    DepartStationName3R = relatedFlightsR[2].DepartureStationName,
                                    ArrivalStationName3R = relatedFlightsR[2].ArrivalStationName,
                                    DepartureStationAirport3R = relatedFlightsR[2].DepartureStationAirport,
                                    ArrivalStationAirport3R = relatedFlightsR[2].ArrivalStationAirport,
                                    SRC3R = relatedFlightsR[2].DepartureStation,
                                    DEST3R = relatedFlightsR[2].ArrivalStation,
                                    Via2R = checkVia(relatedFlightsR[2].Via),
                                    ViaName2R = checkViaName(relatedFlightsR[2].ViaName),
                                    TerminalSRC2R = checkTerminal(relatedFlightsR[2].DepartureTerminal),
                                    TerminalDEST2R = checkTerminal(relatedFlightsR[2].ArrivalTerminal),
                                    logo2R = "/assets/img/airlogo_square/" + relatedFlightsR[2].CarrierCode + ".gif",
                                    FlightName3R = relatedFlightsR[2].CarrierCode,
                                    CarrierName3R = relatedFlightsR[2].CarrierName,
                                    FlightNumber3R = relatedFlightsR[2].FlightNumber,
                                    FlightDepDate3R = relatedFlightsR[2].DepartureDate,
                                    FlightDepTime3R = relatedFlightsR[2].DepartureTime,
                                    FlightArrDate3R = relatedFlightsR[2].ArrivalDate,
                                    FlightArrTime3R = relatedFlightsR[2].ArrivalTime,
                                    Duration3R = relatedFlightsR[2].DurationDesc,
                                    Layover2R = relatedFlightsR[2].JourneyTimeDesc
                                });
                            }
                            #endregion
                        }
                        catch (Exception ex)
                        {

                        }
                    }
                    #endregion
                    #region
                    else if (relatedFlights.Count == 3)
                    {
                        try
                        {
                            rules = GetFareRules(relatedFlights);
                            rulesIB = GetFareRules(relatedFlightsR);
                            string departureDate = relatedFlights[0].DepartureDate.ToString(); // Assuming DepartureDate is a property of k_ShowFlightOutBound
                            string arrivalDate = relatedFlights[2].ArrivalDate.ToString();
                            if (departureDate != arrivalDate)
                            {
                                arrvchack = DateHelper.DayDiff(departureDate, arrivalDate);
                            }

                            #region
                            if (relatedFlightsR.Count == 1)
                            {

                                string departureDateR = relatedFlightsR[0].DepartureDate.ToString(); // Assuming DepartureDate is a property of k_ShowFlightOutBound
                                string arrivalDateR = relatedFlightsR[0].ArrivalDate.ToString();
                                if (departureDateR != arrivalDateR)
                                {
                                    arrvchackR = DateHelper.DayDiff(departureDateR, arrivalDateR);
                                }

                                FlightOutBound.Add(new k_ShowFlightInternational()
                                {

                                    AgentType = ChekA_C,
                                    CompanyID = CompanyID,
                                    FlightRefid = relatedFlights[0].RefID.ToString(),
                                    Curr = curr,


                                    NoOFAdult = relatedFlights[0].Adt,
                                    NoOFChild = relatedFlights[0].Chd,
                                    NoOFInfant = relatedFlights[0].Inf,

                                    //==============================================================================================================================
                                    Adt_BASIC = GetConvert(relatedFlights[0].Adt_BASIC.ToString()),
                                    Chd_BASIC = GetConvert(relatedFlights[0].Chd_BASIC.ToString()),
                                    Inf_BASIC = GetConvert(relatedFlights[0].Inf_BASIC.ToString()),

                                    AdultbaseFare = relatedFlights[0].Adt * GetConvert(relatedFlights[0].Adt_BASIC),
                                    ChildbaseFare = relatedFlights[0].Chd * GetConvert(relatedFlights[0].Chd_BASIC),
                                    InfantbaseFare = relatedFlights[0].Inf * GetConvert(relatedFlights[0].Inf_BASIC),

                                    Adt_YQ = GetConvert(relatedFlights[0].Adt_YQ),
                                    AdtTotalTax = GetConvert(relatedFlights[0].AdtTotalTax),
                                    Chd_YQ = GetConvert(relatedFlights[0].Chd_YQ),
                                    ChdTotalTax = GetConvert(relatedFlights[0].ChdTotalTax),
                                    Inf_TAX = GetConvert(relatedFlights[0].Inf_TAX),

                                    Adt_AcuTax = ReturnTAXADT(relatedFlights[0]).ToString(),
                                    Chd_AcuTax = ReturnTAXCHD(relatedFlights[0]).ToString(),
                                    Inf_AcuTax = ReturnTAXINF(relatedFlights[0]).ToString(),

                                    Yq = "Commission : " + String.Format("{0:N0}", GetConvert(relatedFlights[0].TotalCommission)),
                                    PromoChek = promo,

                                    TotalAmount = ReturnTotalFareAmount(relatedFlights[0]),
                                    TotalFare = ReturnTotalFareAmount(relatedFlights[0]),
                                    FlightAmount = ReturnTotalFareAmount(relatedFlights[0]),
                                    TotalTax = GetTotalTax(relatedFlights[0]),
                                    TotalBasic = GetTotalBasic(relatedFlights[0]),
                                    TotalServiceFee = GetTotalServiceFee(relatedFlights[0]),
                                    TotalMarkup = GetTotalMarkUp(relatedFlights[0]),
                                    TotalServiceTax = GetTotalServiceTax(relatedFlights[0]),
                                    TaxandCharges = GetTaxandCharges(relatedFlights[0]),
                                    TotalCommission = ReturnCommission(relatedFlights[0], CompanyID),
                                    TotalTds = ReturnTDS(relatedFlights[0], CompanyID),
                                    FinalFare = ReturnFinalFare(relatedFlights[0], CompanyID),

                                    //==================================================================================================================

                                    RuleTarrif = relatedFlights[0].RuleTarrif ?? string.Empty,
                                    CarrierName = relatedFlights[0].CarrierName,
                                    FlightName = relatedFlights[0].CarrierCode,
                                    FlightNumber = relatedFlights[0].FlightNumber,
                                    FlightDepDate = relatedFlights[0].DepartureDate,
                                    FlightDepTime = relatedFlights[0].DepartureTime,
                                    FlightArrDate = relatedFlights[2].ArrivalDate,
                                    FlightArrTime = relatedFlights[2].ArrivalTime,
                                    SRC = relatedFlights[0].Origin,
                                    DEST = relatedFlights[0].Destination,
                                    logo = "/assets/img/airlogo_square/" + relatedFlights[0].CarrierCode + ".gif",
                                    Duration = relatedFlights[0].DurationDesc,
                                    Stop = "TwoStop",
                                    CHECKINBaggage = ReturnCheckINBaggage(relatedFlights[0]).ToString(),
                                    CABINBaggage = ReturnCabinINBaggage(relatedFlights[0]).ToString(),
                                    connection = 3,
                                    ArrivalNextDayCheck = arrvchack,
                                    PriceType = relatedFlights[0].PriceType,
                                    RefundType = ReturnRefundType(relatedFlights[0]).ToString(),
                                    AvailableSeat = checkSeat(relatedFlights[0].SeatsAvailable.ToString()),
                                    FareRules = rules,

                                    SRC1 = relatedFlights[0].DepartureStation,
                                    DEST1 = relatedFlights[0].ArrivalStation,
                                    DepartStationName1 = relatedFlights[0].DepartureStationName,
                                    ArrivalStationName1 = relatedFlights[0].ArrivalStationName,
                                    DepartureStationAirport1 = relatedFlights[0].DepartureStationAirport,
                                    ArrivalStationAirport1 = relatedFlights[0].ArrivalStationAirport,
                                    Class1 = ReturnCls(relatedFlights[0]),
                                    Cabin1 = relatedFlights[0].Cabin,
                                    Via = checkVia(relatedFlights[0].Via),
                                    ViaName = checkViaName(relatedFlights[0].ViaName),
                                    TerminalSRC = checkTerminal(relatedFlights[0].DepartureTerminal),
                                    TerminalDEST = checkTerminal(relatedFlights[0].ArrivalTerminal),
                                    CarrierName1 = relatedFlights[0].CarrierName,
                                    FlightName1 = relatedFlights[0].CarrierCode,
                                    FlightNumber1 = relatedFlights[0].FlightNumber,
                                    FlightDepDate1 = relatedFlights[0].DepartureDate,
                                    FlightDepTime1 = relatedFlights[0].DepartureTime,
                                    FlightArrDate1 = relatedFlights[0].ArrivalDate,
                                    FlightArrTime1 = relatedFlights[0].ArrivalTime,
                                    Duration1 = relatedFlights[0].DurationDesc,

                                    DepartStationName2 = relatedFlights[1].DepartureStationName,
                                    ArrivalStationName2 = relatedFlights[1].ArrivalStationName,
                                    DepartureStationAirport2 = relatedFlights[1].DepartureStationAirport,
                                    ArrivalStationAirport2 = relatedFlights[1].ArrivalStationAirport,
                                    Class2 = ReturnCls(relatedFlights[1]),
                                    Cabin2 = relatedFlights[1].Cabin,
                                    SRC2 = relatedFlights[1].DepartureStation,
                                    DEST2 = relatedFlights[1].ArrivalStation,
                                    Via1 = checkVia(relatedFlights[1].Via),
                                    ViaName1 = checkViaName(relatedFlights[1].ViaName),
                                    TerminalSRC1 = checkTerminal(relatedFlights[1].DepartureTerminal),
                                    TerminalDEST1 = checkTerminal(relatedFlights[1].ArrivalTerminal),
                                    FlightName2 = relatedFlights[1].CarrierCode,
                                    CarrierName2 = relatedFlights[1].CarrierName,
                                    FlightNumber2 = relatedFlights[1].FlightNumber,
                                    FlightDepDate2 = relatedFlights[1].DepartureDate,
                                    FlightDepTime2 = relatedFlights[1].DepartureTime,
                                    FlightArrDate2 = relatedFlights[1].ArrivalDate,
                                    FlightArrTime2 = relatedFlights[1].ArrivalTime,
                                    Duration2 = relatedFlights[1].DurationDesc,
                                    Layover1 = relatedFlights[1].JourneyTimeDesc,

                                    DepartStationName3 = relatedFlights[2].DepartureStationName,
                                    ArrivalStationName3 = relatedFlights[2].ArrivalStationName,
                                    DepartureStationAirport3 = relatedFlights[2].DepartureStationAirport,
                                    ArrivalStationAirport3 = relatedFlights[2].ArrivalStationAirport,
                                    Class3 = ReturnCls(relatedFlights[2]),
                                    Cabin3 = relatedFlights[2].Cabin,
                                    SRC3 = relatedFlights[2].DepartureStation,
                                    DEST3 = relatedFlights[2].ArrivalStation,
                                    Via2 = checkVia(relatedFlights[2].Via),
                                    ViaName2 = checkViaName(relatedFlights[2].ViaName),
                                    TerminalSRC2 = checkTerminal(relatedFlights[2].DepartureTerminal),
                                    TerminalDEST2 = checkTerminal(relatedFlights[2].ArrivalTerminal),
                                    FlightName3 = relatedFlights[2].CarrierCode,
                                    FlightNumber3 = relatedFlights[2].FlightNumber,
                                    CarrierName3 = relatedFlights[2].CarrierName,
                                    FlightDepDate3 = relatedFlights[2].DepartureDate,
                                    FlightDepTime3 = relatedFlights[2].DepartureTime,
                                    FlightArrDate3 = relatedFlights[2].ArrivalDate,
                                    FlightArrTime3 = relatedFlights[2].ArrivalTime,
                                    Duration3 = relatedFlights[2].DurationDesc,
                                    Layover2 = relatedFlights[2].JourneyTimeDesc,

                                    CHECKINBaggageR = ReturnCheckINBaggage(relatedFlightsR[0]).ToString(),
                                    CABINBaggageR = ReturnCabinINBaggage(relatedFlightsR[0]).ToString(),
                                    FareRulesR = rulesIB,
                                    FlightNameR = relatedFlightsR[0].CarrierCode,
                                    CarrierNameR = relatedFlightsR[0].CarrierName,
                                    FlightNumberR = relatedFlightsR[0].FlightNumber,
                                    FlightDepDateR = relatedFlightsR[0].DepartureDate,
                                    FlightDepTimeR = relatedFlightsR[0].DepartureTime,
                                    FlightArrDateR = relatedFlightsR[0].ArrivalDate,
                                    FlightArrTimeR = relatedFlightsR[0].ArrivalTime,
                                    SRCR = relatedFlightsR[0].Origin,
                                    DESTR = relatedFlightsR[0].Destination,
                                    StopR = "NonStop",
                                    logoR = "/assets/img/airlogo_square/" + relatedFlightsR[0].CarrierCode + ".gif",
                                    DurationR = relatedFlightsR[0].DurationDesc,
                                    connectionR = 1,
                                    PriceTypeR = relatedFlightsR[0].PriceType,
                                    RefundTypeR = ReturnRefundType(relatedFlightsR[0]).ToString(),
                                    AvailableSeatR = checkSeat(relatedFlightsR[0].SeatsAvailable.ToString()),
                                    ArrivalNextDayCheckR = arrvchackR,

                                    SRC1R = relatedFlightsR[0].DepartureStation,
                                    DEST1R = relatedFlightsR[0].ArrivalStation,
                                    ViaR = checkVia(relatedFlightsR[0].Via),
                                    ViaNameR = checkViaName(relatedFlightsR[0].ViaName),
                                    TerminalSRCR = checkTerminal(relatedFlightsR[0].DepartureTerminal),
                                    TerminalDESTR = checkTerminal(relatedFlightsR[0].ArrivalTerminal),
                                    FlightName1R = relatedFlightsR[0].CarrierCode,
                                    CarrierName1R = relatedFlightsR[0].CarrierName,
                                    FlightNumber1R = relatedFlightsR[0].FlightNumber,
                                    FlightDepDate1R = relatedFlightsR[0].DepartureDate,
                                    FlightDepTime1R = relatedFlightsR[0].DepartureTime,
                                    FlightArrDate1R = relatedFlightsR[0].ArrivalDate,
                                    FlightArrTime1R = relatedFlightsR[0].ArrivalTime,
                                    Duration1R = relatedFlightsR[0].DurationDesc,
                                    DepartStationName1R = relatedFlightsR[0].DepartureStationName,
                                    ArrivalStationName1R = relatedFlightsR[0].ArrivalStationName,
                                    DepartureStationAirport1R = relatedFlightsR[0].DepartureStationAirport,
                                    ArrivalStationAirport1R = relatedFlightsR[0].ArrivalStationAirport,
                                    Class1R = ReturnCls(relatedFlightsR[0]).ToString(),
                                    Cabin1R = relatedFlightsR[0].Cabin,
                                    Layover1R = relatedFlightsR[0].JourneyTimeDesc,
                                });
                            }
                            #endregion
                            #region
                            else if (relatedFlightsR.Count == 2)
                            {
                                string departureDateR = relatedFlightsR[0].DepartureDate.ToString(); // Assuming DepartureDate is a property of k_ShowFlightOutBound
                                string arrivalDateR = relatedFlightsR[1].ArrivalDate.ToString();
                                if (departureDateR != arrivalDateR)
                                {
                                    arrvchackR = DateHelper.DayDiff(departureDateR, arrivalDateR);
                                }

                                FlightOutBound.Add(new k_ShowFlightInternational()
                                {

                                    AgentType = ChekA_C,
                                    CompanyID = CompanyID,
                                    FlightRefid = relatedFlights[0].RefID.ToString(),
                                    Curr = curr,

                                    NoOFAdult = relatedFlights[0].Adt,
                                    NoOFChild = relatedFlights[0].Chd,
                                    NoOFInfant = relatedFlights[0].Inf,

                                    //==============================================================================================================================
                                    Adt_BASIC = GetConvert(relatedFlights[0].Adt_BASIC.ToString()),
                                    Chd_BASIC = GetConvert(relatedFlights[0].Chd_BASIC.ToString()),
                                    Inf_BASIC = GetConvert(relatedFlights[0].Inf_BASIC.ToString()),

                                    AdultbaseFare = relatedFlights[0].Adt * GetConvert(relatedFlights[0].Adt_BASIC),
                                    ChildbaseFare = relatedFlights[0].Chd * GetConvert(relatedFlights[0].Chd_BASIC),
                                    InfantbaseFare = relatedFlights[0].Inf * GetConvert(relatedFlights[0].Inf_BASIC),

                                    Adt_YQ = GetConvert(relatedFlights[0].Adt_YQ),
                                    AdtTotalTax = GetConvert(relatedFlights[0].AdtTotalTax),
                                    Chd_YQ = GetConvert(relatedFlights[0].Chd_YQ),
                                    ChdTotalTax = GetConvert(relatedFlights[0].ChdTotalTax),
                                    Inf_TAX = GetConvert(relatedFlights[0].Inf_TAX),

                                    Adt_AcuTax = ReturnTAXADT(relatedFlights[0]).ToString(),
                                    Chd_AcuTax = ReturnTAXCHD(relatedFlights[0]).ToString(),
                                    Inf_AcuTax = ReturnTAXINF(relatedFlights[0]).ToString(),

                                    Yq = "Commission : " + String.Format("{0:N0}", GetConvert(relatedFlights[0].TotalCommission)),
                                    PromoChek = promo,


                                    TotalAmount = ReturnTotalFareAmount(relatedFlights[0]),
                                    TotalFare = ReturnTotalFareAmount(relatedFlights[0]),
                                    FlightAmount = ReturnTotalFareAmount(relatedFlights[0]),
                                    TotalTax = GetTotalTax(relatedFlights[0]),
                                    TotalBasic = GetTotalBasic(relatedFlights[0]),
                                    TotalServiceFee = GetTotalServiceFee(relatedFlights[0]),
                                    TotalMarkup = GetTotalMarkUp(relatedFlights[0]),
                                    TotalServiceTax = GetTotalServiceTax(relatedFlights[0]),
                                    TaxandCharges = GetTaxandCharges(relatedFlights[0]),
                                    TotalCommission = ReturnCommission(relatedFlights[0], CompanyID),
                                    TotalTds = ReturnTDS(relatedFlights[0], CompanyID),
                                    FinalFare = ReturnFinalFare(relatedFlights[0], CompanyID),

                                    //==================================================================================================================

                                    RuleTarrif = relatedFlights[0].RuleTarrif ?? string.Empty,
                                    CarrierName = relatedFlights[0].CarrierName,
                                    FlightName = relatedFlights[0].CarrierCode,
                                    FlightNumber = relatedFlights[0].FlightNumber,
                                    FlightDepDate = relatedFlights[0].DepartureDate,
                                    FlightDepTime = relatedFlights[0].DepartureTime,
                                    FlightArrDate = relatedFlights[2].ArrivalDate,
                                    FlightArrTime = relatedFlights[2].ArrivalTime,

                                    CHECKINBaggage = ReturnCheckINBaggage(relatedFlights[0]).ToString(),
                                    CABINBaggage = ReturnCabinINBaggage(relatedFlights[0]).ToString(),
                                    SRC = relatedFlights[0].Origin,
                                    DEST = relatedFlights[0].Destination,
                                    logo = "/assets/img/airlogo_square/" + relatedFlights[0].CarrierCode + ".gif",
                                    Duration = relatedFlights[0].DurationDesc,
                                    Stop = "TwoStop",
                                    connection = 3,
                                    ArrivalNextDayCheck = arrvchack,
                                    PriceType = relatedFlights[0].PriceType,
                                    RefundType = ReturnRefundType(relatedFlights[0]).ToString(),
                                    AvailableSeat = checkSeat(relatedFlights[0].SeatsAvailable.ToString()),
                                    FareRules = rules,


                                    DepartStationName1 = relatedFlights[0].DepartureStationName,
                                    ArrivalStationName1 = relatedFlights[0].ArrivalStationName,
                                    DepartureStationAirport1 = relatedFlights[0].DepartureStationAirport,
                                    ArrivalStationAirport1 = relatedFlights[0].ArrivalStationAirport,
                                    Class1 = ReturnCls(relatedFlights[0]),
                                    Cabin1 = relatedFlights[0].Cabin,
                                    SRC1 = relatedFlights[0].DepartureStation,
                                    DEST1 = relatedFlights[0].ArrivalStation,
                                    Via = checkVia(relatedFlights[0].Via),
                                    ViaName = checkViaName(relatedFlights[0].ViaName),
                                    TerminalSRC = checkTerminal(relatedFlights[0].DepartureTerminal),
                                    TerminalDEST = checkTerminal(relatedFlights[0].ArrivalTerminal),
                                    CarrierName1 = relatedFlights[0].CarrierName,
                                    FlightName1 = relatedFlights[0].CarrierCode,
                                    FlightNumber1 = relatedFlights[0].FlightNumber,
                                    FlightDepDate1 = relatedFlights[0].DepartureDate,
                                    FlightDepTime1 = relatedFlights[0].DepartureTime,
                                    FlightArrDate1 = relatedFlights[0].ArrivalDate,
                                    FlightArrTime1 = relatedFlights[0].ArrivalTime,
                                    Duration1 = relatedFlights[0].DurationDesc,


                                    DepartStationName2 = relatedFlights[1].DepartureStationName,
                                    ArrivalStationName2 = relatedFlights[1].ArrivalStationName,
                                    DepartureStationAirport2 = relatedFlights[1].DepartureStationAirport,
                                    ArrivalStationAirport2 = relatedFlights[1].ArrivalStationAirport,
                                    Class2 = ReturnCls(relatedFlights[1]),
                                    Cabin2 = relatedFlights[1].Cabin,
                                    SRC2 = relatedFlights[1].DepartureStation,
                                    DEST2 = relatedFlights[1].ArrivalStation,
                                    Via1 = checkVia(relatedFlights[1].Via),
                                    ViaName1 = checkViaName(relatedFlights[1].ViaName),
                                    TerminalSRC1 = checkTerminal(relatedFlights[1].DepartureTerminal),
                                    TerminalDEST1 = checkTerminal(relatedFlights[1].ArrivalTerminal),
                                    FlightName2 = relatedFlights[1].CarrierCode,
                                    CarrierName2 = relatedFlights[1].CarrierName,
                                    FlightNumber2 = relatedFlights[1].FlightNumber,
                                    FlightDepDate2 = relatedFlights[1].DepartureDate,
                                    FlightDepTime2 = relatedFlights[1].DepartureTime,
                                    FlightArrDate2 = relatedFlights[1].ArrivalDate,
                                    FlightArrTime2 = relatedFlights[1].ArrivalTime,
                                    Duration2 = relatedFlights[1].DurationDesc,
                                    Layover1 = relatedFlights[1].JourneyTimeDesc,


                                    DepartStationName3 = relatedFlights[2].DepartureStationName,
                                    ArrivalStationName3 = relatedFlights[2].ArrivalStationName,
                                    DepartureStationAirport3 = relatedFlights[2].DepartureStationAirport,
                                    ArrivalStationAirport3 = relatedFlights[2].ArrivalStationAirport,
                                    Class3 = ReturnCls(relatedFlights[2]),
                                    Cabin3 = relatedFlights[2].Cabin,
                                    SRC3 = relatedFlights[2].DepartureStation,
                                    DEST3 = relatedFlights[2].ArrivalStation,
                                    Via2 = checkVia(relatedFlights[2].Via),
                                    ViaName2 = checkViaName(relatedFlights[2].ViaName),
                                    TerminalSRC2 = checkTerminal(relatedFlights[2].DepartureTerminal),
                                    TerminalDEST2 = checkTerminal(relatedFlights[2].ArrivalTerminal),
                                    FlightName3 = relatedFlights[2].CarrierCode,
                                    FlightNumber3 = relatedFlights[2].FlightNumber,
                                    CarrierName3 = relatedFlights[2].CarrierName,
                                    FlightDepDate3 = relatedFlights[2].DepartureDate,
                                    FlightDepTime3 = relatedFlights[2].DepartureTime,
                                    FlightArrDate3 = relatedFlights[2].ArrivalDate,
                                    FlightArrTime3 = relatedFlights[2].ArrivalTime,
                                    Duration3 = relatedFlights[2].DurationDesc,
                                    Layover2 = relatedFlights[2].JourneyTimeDesc,


                                    CHECKINBaggageR = ReturnCheckINBaggage(relatedFlightsR[0]).ToString(),
                                    CABINBaggageR = ReturnCabinINBaggage(relatedFlightsR[0]).ToString(),
                                    FareRulesR = rulesIB,
                                    CarrierNameR = relatedFlightsR[0].CarrierName,
                                    FlightNameR = relatedFlightsR[0].CarrierCode,
                                    FlightNumberR = relatedFlightsR[0].FlightNumber,
                                    FlightDepDateR = relatedFlightsR[0].DepartureDate,
                                    FlightDepTimeR = relatedFlightsR[0].DepartureTime,
                                    FlightArrDateR = relatedFlightsR[1].ArrivalDate,
                                    FlightArrTimeR = relatedFlightsR[1].ArrivalTime,
                                    SRCR = relatedFlightsR[0].Origin,
                                    DESTR = relatedFlightsR[0].Destination,
                                    StopR = "OneStop",
                                    logoR = "/assets/img/airlogo_square/" + relatedFlightsR[0].CarrierCode + ".gif",
                                    DurationR = relatedFlightsR[0].DurationDesc,
                                    connectionR = 2,
                                    PriceTypeR = relatedFlightsR[0].PriceType,
                                    RefundTypeR = ReturnRefundType(relatedFlightsR[0]).ToString(),
                                    AvailableSeatR = checkSeat(relatedFlightsR[0].SeatsAvailable.ToString()),
                                    ArrivalNextDayCheckR = arrvchackR,

                                    Class1R = ReturnCls(relatedFlightsR[0]).ToString(),
                                    Cabin1R = relatedFlightsR[0].Cabin,
                                    SRC1R = relatedFlightsR[0].DepartureStation,
                                    DEST1R = relatedFlightsR[0].ArrivalStation,
                                    DepartStationName1R = relatedFlightsR[0].DepartureStationName,
                                    ArrivalStationName1R = relatedFlightsR[0].ArrivalStationName,
                                    DepartureStationAirport1R = relatedFlightsR[0].DepartureStationAirport,
                                    ArrivalStationAirport1R = relatedFlightsR[0].ArrivalStationAirport,
                                    ViaR = checkVia(relatedFlightsR[0].Via),
                                    ViaNameR = checkViaName(relatedFlightsR[0].ViaName),
                                    TerminalSRCR = checkTerminal(relatedFlightsR[0].DepartureTerminal),
                                    TerminalDESTR = checkTerminal(relatedFlightsR[0].ArrivalTerminal),
                                    FlightName1R = relatedFlightsR[0].CarrierCode,
                                    CarrierName1R = relatedFlightsR[0].CarrierName,
                                    FlightNumber1R = relatedFlightsR[0].FlightNumber,
                                    FlightDepDate1R = relatedFlightsR[0].DepartureDate,
                                    FlightDepTime1R = relatedFlightsR[0].DepartureTime,
                                    FlightArrDate1R = relatedFlightsR[0].ArrivalDate,
                                    FlightArrTime1R = relatedFlightsR[0].ArrivalTime,
                                    Duration1R = relatedFlightsR[0].DurationDesc,

                                    Class2R = ReturnCls(relatedFlightsR[1]).ToString(),
                                    Cabin2R = relatedFlightsR[1].Cabin,
                                    DepartStationName2R = relatedFlightsR[1].DepartureStationName,
                                    ArrivalStationName2R = relatedFlightsR[1].ArrivalStationName,
                                    DepartureStationAirport2R = relatedFlightsR[1].DepartureStationAirport,
                                    ArrivalStationAirport2R = relatedFlightsR[1].ArrivalStationAirport,
                                    SRC2R = relatedFlightsR[1].DepartureStation,
                                    DEST2R = relatedFlightsR[1].ArrivalStation,
                                    Via1R = checkVia(relatedFlightsR[1].Via),
                                    ViaName1R = checkViaName(relatedFlightsR[1].ViaName),
                                    TerminalSRC1R = checkTerminal(relatedFlightsR[1].DepartureTerminal),
                                    TerminalDEST1R = checkTerminal(relatedFlightsR[1].ArrivalTerminal),
                                    logo1R = "/assets/img/airlogo_square/" + relatedFlightsR[1].CarrierCode + ".gif",
                                    FlightName2R = relatedFlightsR[1].CarrierCode,
                                    CarrierName2R = relatedFlightsR[1].CarrierName,
                                    FlightNumber2R = relatedFlightsR[1].FlightNumber,
                                    FlightDepDate2R = relatedFlightsR[1].DepartureDate,
                                    FlightDepTime2R = relatedFlightsR[1].DepartureTime,
                                    FlightArrDate2R = relatedFlightsR[1].ArrivalDate,
                                    FlightArrTime2R = relatedFlightsR[1].ArrivalTime,
                                    Duration2R = relatedFlightsR[1].DurationDesc,
                                    Layover1R = relatedFlightsR[1].JourneyTimeDesc
                                });
                            }
                            #endregion
                            #region
                            else if (relatedFlightsR.Count == 3)
                            {

                                string departureDateR = relatedFlightsR[0].DepartureDate.ToString(); // Assuming DepartureDate is a property of k_ShowFlightOutBound
                                string arrivalDateR = relatedFlightsR[2].ArrivalDate.ToString();
                                if (departureDateR != arrivalDateR)
                                {
                                    arrvchackR = DateHelper.DayDiff(departureDateR, arrivalDateR);
                                }

                                FlightOutBound.Add(new k_ShowFlightInternational()
                                {
                                    AgentType = ChekA_C,
                                    CompanyID = CompanyID,
                                    FlightRefid = relatedFlights[0].RefID.ToString(),
                                    Curr = curr,


                                    NoOFAdult = relatedFlights[0].Adt,
                                    NoOFChild = relatedFlights[0].Chd,
                                    NoOFInfant = relatedFlights[0].Inf,

                                    //==============================================================================================================================
                                    Adt_BASIC = GetConvert(relatedFlights[0].Adt_BASIC.ToString()),
                                    Chd_BASIC = GetConvert(relatedFlights[0].Chd_BASIC.ToString()),
                                    Inf_BASIC = GetConvert(relatedFlights[0].Inf_BASIC.ToString()),

                                    AdultbaseFare = relatedFlights[0].Adt * GetConvert(relatedFlights[0].Adt_BASIC),
                                    ChildbaseFare = relatedFlights[0].Chd * GetConvert(relatedFlights[0].Chd_BASIC),
                                    InfantbaseFare = relatedFlights[0].Inf * GetConvert(relatedFlights[0].Inf_BASIC),

                                    Adt_YQ = GetConvert(relatedFlights[0].Adt_YQ),
                                    AdtTotalTax = GetConvert(relatedFlights[0].AdtTotalTax),
                                    Chd_YQ = GetConvert(relatedFlights[0].Chd_YQ),
                                    ChdTotalTax = GetConvert(relatedFlights[0].ChdTotalTax),
                                    Inf_TAX = GetConvert(relatedFlights[0].Inf_TAX),

                                    Adt_AcuTax = ReturnTAXADT(relatedFlights[0]).ToString(),
                                    Chd_AcuTax = ReturnTAXCHD(relatedFlights[0]).ToString(),
                                    Inf_AcuTax = ReturnTAXINF(relatedFlights[0]).ToString(),

                                    Yq = "Commission : " + String.Format("{0:N0}", GetConvert(relatedFlights[0].TotalCommission)),
                                    PromoChek = promo,

                                    TotalAmount = ReturnTotalFareAmount(relatedFlights[0]),
                                    TotalFare = ReturnTotalFareAmount(relatedFlights[0]),
                                    FlightAmount = ReturnTotalFareAmount(relatedFlights[0]),
                                    TotalTax = GetTotalTax(relatedFlights[0]),
                                    TotalBasic = GetTotalBasic(relatedFlights[0]),
                                    TotalServiceFee = GetTotalServiceFee(relatedFlights[0]),
                                    TotalMarkup = GetTotalMarkUp(relatedFlights[0]),
                                    TotalServiceTax = GetTotalServiceTax(relatedFlights[0]),
                                    TaxandCharges = GetTaxandCharges(relatedFlights[0]),
                                    TotalCommission = ReturnCommission(relatedFlights[0], CompanyID),
                                    TotalTds = ReturnTDS(relatedFlights[0], CompanyID),
                                    FinalFare = ReturnFinalFare(relatedFlights[0], CompanyID),


                                    //==================================================================================================================

                                    RuleTarrif = relatedFlights[0].RuleTarrif ?? string.Empty,
                                    CarrierName = relatedFlights[0].CarrierName,
                                    FlightName = relatedFlights[0].CarrierCode,
                                    FlightNumber = relatedFlights[0].FlightNumber,
                                    FlightDepDate = relatedFlights[0].DepartureDate,
                                    FlightDepTime = relatedFlights[0].DepartureTime,
                                    FlightArrDate = relatedFlights[2].ArrivalDate,
                                    FlightArrTime = relatedFlights[2].ArrivalTime,
                                    SRC = relatedFlights[0].Origin,
                                    DEST = relatedFlights[0].Destination,
                                    logo = "/assets/img/airlogo_square/" + relatedFlights[0].CarrierCode + ".gif",
                                    Duration = relatedFlights[0].DurationDesc,
                                    Stop = "TwoStop",
                                    CHECKINBaggage = ReturnCheckINBaggage(relatedFlights[0]).ToString(),
                                    CABINBaggage = ReturnCabinINBaggage(relatedFlights[0]).ToString(),
                                    connection = 3,
                                    ArrivalNextDayCheck = arrvchack,
                                    PriceType = relatedFlights[0].PriceType,
                                    RefundType = ReturnRefundType(relatedFlights[0]).ToString(),
                                    AvailableSeat = checkSeat(relatedFlights[0].SeatsAvailable.ToString()),
                                    FareRules = rules,

                                    SRC1 = relatedFlights[0].DepartureStation,
                                    DEST1 = relatedFlights[0].ArrivalStation,
                                    DepartStationName1 = relatedFlights[0].DepartureStationName,
                                    ArrivalStationName1 = relatedFlights[0].ArrivalStationName,
                                    DepartureStationAirport1 = relatedFlights[0].DepartureStationAirport,
                                    ArrivalStationAirport1 = relatedFlights[0].ArrivalStationAirport,
                                    Class1 = ReturnCls(relatedFlights[0]),
                                    Cabin1 = relatedFlights[0].Cabin,
                                    Via = checkVia(relatedFlights[0].Via),
                                    ViaName = checkViaName(relatedFlights[0].ViaName),
                                    TerminalSRC = checkTerminal(relatedFlights[0].DepartureTerminal),
                                    TerminalDEST = checkTerminal(relatedFlights[0].ArrivalTerminal),
                                    CarrierName1 = relatedFlights[0].CarrierName,
                                    FlightName1 = relatedFlights[0].CarrierCode,
                                    FlightNumber1 = relatedFlights[0].FlightNumber,
                                    FlightDepDate1 = relatedFlights[0].DepartureDate,
                                    FlightDepTime1 = relatedFlights[0].DepartureTime,
                                    FlightArrDate1 = relatedFlights[0].ArrivalDate,
                                    FlightArrTime1 = relatedFlights[0].ArrivalTime,
                                    Duration1 = relatedFlights[0].DurationDesc,
                                    DepartStationName2 = relatedFlights[1].DepartureStationName,
                                    ArrivalStationName2 = relatedFlights[1].ArrivalStationName,
                                    DepartureStationAirport2 = relatedFlights[1].DepartureStationAirport,
                                    ArrivalStationAirport2 = relatedFlights[1].ArrivalStationAirport,
                                    Class2 = ReturnCls(relatedFlights[1]),
                                    Cabin2 = relatedFlights[1].Cabin,
                                    SRC2 = relatedFlights[1].DepartureStation,
                                    DEST2 = relatedFlights[1].ArrivalStation,
                                    Via1 = checkVia(relatedFlights[1].Via),
                                    ViaName1 = checkViaName(relatedFlights[1].ViaName),
                                    TerminalSRC1 = checkTerminal(relatedFlights[1].DepartureTerminal),
                                    TerminalDEST1 = checkTerminal(relatedFlights[1].ArrivalTerminal),
                                    FlightName2 = relatedFlights[1].CarrierCode,
                                    CarrierName2 = relatedFlights[1].CarrierName,
                                    FlightNumber2 = relatedFlights[1].FlightNumber,
                                    FlightDepDate2 = relatedFlights[1].DepartureDate,
                                    FlightDepTime2 = relatedFlights[1].DepartureTime,
                                    FlightArrDate2 = relatedFlights[1].ArrivalDate,
                                    FlightArrTime2 = relatedFlights[1].ArrivalTime,
                                    Duration2 = relatedFlights[1].DurationDesc,
                                    Layover1 = relatedFlights[1].JourneyTimeDesc,
                                    DepartStationName3 = relatedFlights[2].DepartureStationName,
                                    ArrivalStationName3 = relatedFlights[2].ArrivalStationName,
                                    DepartureStationAirport3 = relatedFlights[2].DepartureStationAirport,
                                    ArrivalStationAirport3 = relatedFlights[2].ArrivalStationAirport,
                                    Class3 = ReturnCls(relatedFlights[2]),
                                    Cabin3 = relatedFlights[2].Cabin,
                                    SRC3 = relatedFlights[2].DepartureStation,
                                    DEST3 = relatedFlights[2].ArrivalStation,
                                    Via2 = checkVia(relatedFlights[2].Via),
                                    ViaName2 = checkViaName(relatedFlights[2].ViaName),
                                    TerminalSRC2 = checkTerminal(relatedFlights[2].DepartureTerminal),
                                    TerminalDEST2 = checkTerminal(relatedFlights[2].ArrivalTerminal),
                                    FlightName3 = relatedFlights[2].CarrierCode,
                                    FlightNumber3 = relatedFlights[2].FlightNumber,
                                    CarrierName3 = relatedFlights[2].CarrierName,
                                    FlightDepDate3 = relatedFlights[2].DepartureDate,
                                    FlightDepTime3 = relatedFlights[2].DepartureTime,
                                    FlightArrDate3 = relatedFlights[2].ArrivalDate,
                                    FlightArrTime3 = relatedFlights[2].ArrivalTime,
                                    Duration3 = relatedFlights[2].DurationDesc,
                                    Layover2 = relatedFlights[2].JourneyTimeDesc,


                                    CHECKINBaggageR = ReturnCheckINBaggage(relatedFlightsR[0]).ToString(),
                                    CABINBaggageR = ReturnCabinINBaggage(relatedFlightsR[0]).ToString(),
                                    FareRulesR = rulesIB,
                                    CarrierNameR = relatedFlightsR[0].CarrierName,
                                    FlightNameR = relatedFlightsR[0].CarrierCode,
                                    FlightNumberR = relatedFlightsR[0].FlightNumber,
                                    FlightDepDateR = relatedFlightsR[0].DepartureDate,
                                    FlightDepTimeR = relatedFlightsR[0].DepartureTime,
                                    FlightArrDateR = relatedFlightsR[2].ArrivalDate,
                                    FlightArrTimeR = relatedFlightsR[2].ArrivalTime,
                                    SRCR = relatedFlightsR[0].Origin,
                                    DESTR = relatedFlightsR[0].Destination,
                                    StopR = "TwoStop",
                                    logoR = "/assets/img/airlogo_square/" + relatedFlightsR[0].CarrierCode + ".gif",
                                    DurationR = relatedFlightsR[0].DurationDesc,
                                    connectionR = 3,
                                    PriceTypeR = relatedFlightsR[0].PriceType,
                                    RefundTypeR = ReturnRefundType(relatedFlightsR[0]).ToString(),
                                    AvailableSeatR = checkSeat(relatedFlightsR[0].SeatsAvailable.ToString()),
                                    ArrivalNextDayCheckR = arrvchackR,

                                    Class1R = ReturnCls(relatedFlightsR[0]).ToString(),
                                    Cabin1R = relatedFlightsR[0].Cabin,
                                    SRC1R = relatedFlightsR[0].DepartureStation,
                                    DEST1R = relatedFlightsR[0].ArrivalStation,
                                    DepartStationName1R = relatedFlightsR[0].DepartureStationName,
                                    ArrivalStationName1R = relatedFlightsR[0].ArrivalStationName,
                                    DepartureStationAirport1R = relatedFlightsR[0].DepartureStationAirport,
                                    ArrivalStationAirport1R = relatedFlightsR[0].ArrivalStationAirport,
                                    ViaR = checkVia(relatedFlightsR[0].Via),
                                    ViaNameR = checkViaName(relatedFlightsR[0].ViaName),
                                    TerminalSRCR = checkTerminal(relatedFlightsR[0].DepartureTerminal),
                                    TerminalDESTR = checkTerminal(relatedFlightsR[0].ArrivalTerminal),
                                    FlightName1R = relatedFlightsR[0].CarrierCode,
                                    CarrierName1R = relatedFlightsR[0].CarrierName,
                                    FlightNumber1R = relatedFlightsR[0].FlightNumber,
                                    FlightDepDate1R = relatedFlightsR[0].DepartureDate,
                                    FlightDepTime1R = relatedFlightsR[0].DepartureTime,
                                    FlightArrDate1R = relatedFlightsR[0].ArrivalDate,
                                    FlightArrTime1R = relatedFlightsR[0].ArrivalTime,
                                    Duration1R = relatedFlightsR[0].DurationDesc,


                                    Class2R = ReturnCls(relatedFlightsR[1]).ToString(),
                                    Cabin2R = relatedFlightsR[1].Cabin,
                                    DepartStationName2R = relatedFlightsR[1].DepartureStationName,
                                    ArrivalStationName2R = relatedFlightsR[1].ArrivalStationName,
                                    DepartureStationAirport2R = relatedFlightsR[1].DepartureStationAirport,
                                    ArrivalStationAirport2R = relatedFlightsR[1].ArrivalStationAirport,
                                    SRC2R = relatedFlightsR[1].DepartureStation,
                                    DEST2R = relatedFlightsR[1].ArrivalStation,
                                    Via1R = checkVia(relatedFlightsR[1].Via),
                                    ViaName1R = checkViaName(relatedFlightsR[1].ViaName),
                                    TerminalSRC1R = checkTerminal(relatedFlightsR[1].DepartureTerminal),
                                    TerminalDEST1R = checkTerminal(relatedFlightsR[1].ArrivalTerminal),
                                    logo1R = "/assets/img/airlogo_square/" + relatedFlightsR[1].CarrierCode + ".gif",
                                    FlightName2R = relatedFlightsR[1].CarrierCode,
                                    CarrierName2R = relatedFlightsR[1].CarrierName,
                                    FlightNumber2R = relatedFlightsR[1].FlightNumber,
                                    FlightDepDate2R = relatedFlightsR[1].DepartureDate,
                                    FlightDepTime2R = relatedFlightsR[1].DepartureTime,
                                    FlightArrDate2R = relatedFlightsR[1].ArrivalDate,
                                    FlightArrTime2R = relatedFlightsR[1].ArrivalTime,
                                    Duration2R = relatedFlightsR[1].DurationDesc,
                                    Layover1R = relatedFlightsR[1].JourneyTimeDesc,

                                   Class3R = ReturnCls(relatedFlightsR[2]).ToString(),
                                    Cabin3R = relatedFlightsR[2].Cabin,
                                    DepartStationName3R = relatedFlightsR[2].DepartureStationName,
                                    ArrivalStationName3R = relatedFlightsR[2].ArrivalStationName,
                                    DepartureStationAirport3R = relatedFlightsR[2].DepartureStationAirport,
                                    ArrivalStationAirport3R = relatedFlightsR[2].ArrivalStationAirport,
                                    SRC3R = relatedFlightsR[2].DepartureStation,
                                    DEST3R = relatedFlightsR[2].ArrivalStation,
                                    Via2R = checkVia(relatedFlightsR[2].Via),
                                    ViaName2R = checkViaName(relatedFlightsR[2].ViaName),
                                    TerminalSRC2R = checkTerminal(relatedFlightsR[2].DepartureTerminal),
                                    TerminalDEST2R = checkTerminal(relatedFlightsR[2].ArrivalTerminal),
                                    logo2R = "/assets/img/airlogo_square/" + relatedFlightsR[2].CarrierCode + ".gif",
                                    FlightName3R = relatedFlightsR[2].CarrierCode,
                                    CarrierName3R = relatedFlightsR[2].CarrierName,
                                    FlightNumber3R = relatedFlightsR[2].FlightNumber,
                                    FlightDepDate3R = relatedFlightsR[2].DepartureDate,
                                    FlightDepTime3R = relatedFlightsR[2].DepartureTime,
                                    FlightArrDate3R = relatedFlightsR[2].ArrivalDate,
                                    FlightArrTime3R = relatedFlightsR[2].ArrivalTime,
                                    Duration3R = relatedFlightsR[2].DurationDesc,
                                    Layover2R = relatedFlightsR[2].JourneyTimeDesc
                                });
                            }
                            #endregion
                        }
                        catch (Exception ex)
                        {

                        }
                    }
                    #endregion
                }

                
                    var drSelect = flightList.Where(x => x.Stops == 2).ToList();
                    if (drSelect.Count > 0)
                    {
                        HttpContextHelper.Current.Session.SetString("StopCheck", "NonStop,OneStop,TwoStop");
                    }
                    else
                    {
                        drSelect = flightList.Where(x => x.Stops == 1).ToList();
                        if (drSelect.Count > 0)
                        {
                            HttpContextHelper.Current.Session.SetString("StopCheck", "NonStop,OneStop");
                        }
                        else
                        {
                            HttpContextHelper.Current.Session.SetString("StopCheck", "NonStop");
                        }
                    }
                
                
                
                    drSelect = flightListInt.Where(x => x.Stops == 2).ToList();
                    if (drSelect.Count > 0)
                    {
                        HttpContextHelper.Current.Session.SetString("StopCheckR", "NonStop,OneStop,TwoStop");
                    }
                    else
                    {
                        drSelect = flightList.Where(x => x.Stops == 1).ToList();
                        if (drSelect.Count > 0)
                        {
                            HttpContextHelper.Current.Session.SetString("StopCheckR", "NonStop,OneStop");
                        }
                        else
                        {
                            HttpContextHelper.Current.Session.SetString("StopCheckR", "NonStop");
                        }
                    }

                //string s1 = HttpContext.Current.Session["StopCheck"].ToString();
                //string s2 = HttpContext.Current.Session["StopCheckR"].ToString();
            }
            var asc = (from sh in FlightOutBound
                       orderby sh.TotalAmount ascending
                       select sh).ToList();
            return Task.FromResult(asc);
        }

        public static int GetConvert(int value)
        {
            if (HttpContextHelper.Current.Session.GetString("Curr") == null || HttpContextHelper.Current.Session.GetString("Curr").Equals("INR"))
            {
                return value;
            }
            else
            {
                double dCurrency = 1;
                if (HttpContextHelper.Current.Session.GetString("dCurrency") != null)
                {
                    dCurrency = Convert.ToDouble(HttpContextHelper.Current.Session.GetString("dCurrency"));
                }
                return NumberHelper.AvgAmount1((Convert.ToInt32(value) * dCurrency).ToString());
            }
        }
        private static string GetConvert(string value)
        {
            if (HttpContextHelper.Current.Session.GetString("Curr") == null || HttpContextHelper.Current.Session.GetString("Curr").Equals("INR"))
            {
                return value;
            }
            else
            {
                double dCurrency = 1;
                if (HttpContextHelper.Current.Session.GetString("dCurrency") != null)
                {
                    dCurrency = Convert.ToDouble(HttpContextHelper.Current.Session.GetString("dCurrency"));
                }
                return NumberHelper.AvgAmount1((Convert.ToInt32(value) * dCurrency).ToString()).ToString();
            }
        }
        private static string GetFareRules(List<AirlineAvailabilityInfo> flightInfo)
        {
            string FareRule = string.Empty;
            foreach (var flight in flightInfo)
            {
                FareRule += flight.FareRule;
            }
            if (FareRule.Contains("lt;"))
            {
                FareRule = FareRule.Replace("lt;", "<");
            }
            if (FareRule.Contains("gt;"))
            {
                FareRule = FareRule.Replace("gt;", ">");
            }
            if (FareRule.Contains("amp;"))
            {
                FareRule = FareRule.Replace("amp;", "&");
            }

            if (FareRule.Trim().Length.Equals(0))
            {
                FareRule = "Fare Rules are just a guideline for your convenience and is subject to changes by the Airline from time to time. For any query related to Fare Rules, please contact our call center/book to get accurate Fare Rule.";
            }

            return FareRule;
        }
        private static string ReturnTAXADT(AirlineAvailabilityInfo firstFlight)
        {
            if (firstFlight == null)
            {
                return string.Empty;
            }

            int iTax = firstFlight.AdtTotalTax - firstFlight.Adt_YQ;
            if (HttpContextHelper.Current?.Session.GetString("Curr") == null ||
                HttpContextHelper.Current.Session.GetString("Curr").Equals("INR"))
            {
                return iTax.ToString();
            }
            else
            {
                double dCurrency = 1;
                if (HttpContextHelper.Current.Session.GetString("dCurrency") != null)
                {
                    dCurrency = Convert.ToDouble(HttpContextHelper.Current.Session.GetString("dCurrency"));
                }
                return NumberHelper.AvgAmount1((iTax * dCurrency).ToString()).ToString();
            }
        }
        private static string ReturnTAXCHD(AirlineAvailabilityInfo firstFlight)
        {
            if (firstFlight == null)
            {
                return string.Empty;
            }

            int iTax = firstFlight.ChdTotalTax - firstFlight.Chd_YQ;
            if (HttpContextHelper.Current?.Session.GetString("Curr") == null ||
                HttpContextHelper.Current.Session.GetString("Curr").Equals("INR"))
            {
                return iTax.ToString();
            }
            else
            {
                double dCurrency = 1;
                if (HttpContextHelper.Current.Session.GetString("dCurrency") != null)
                {
                    dCurrency = Convert.ToDouble(HttpContextHelper.Current.Session.GetString("dCurrency"));
                }
                return NumberHelper.AvgAmount1((iTax * dCurrency).ToString()).ToString();
            }
        }
        private static string ReturnTAXINF(AirlineAvailabilityInfo firstFlight)
        {
            if (firstFlight == null)
            {
                return string.Empty;
            }

            int iTax = firstFlight.InfTotalTax; // Assuming InfTotalTax is a property of AirlineAvailabilityInfo
            if (HttpContextHelper.Current?.Session.GetString("Curr") == null ||
                HttpContextHelper.Current.Session.GetString("Curr").Equals("INR"))
            {
                return iTax.ToString();
            }
            else
            {
                double dCurrency = 1;
                if (HttpContextHelper.Current.Session.GetString("dCurrency") != null)
                {
                    dCurrency = Convert.ToDouble(HttpContextHelper.Current.Session.GetString("dCurrency"));
                }
                return NumberHelper.AvgAmount1((iTax * dCurrency).ToString()).ToString();
            }
        }
        private static string ReturnProviderCd(AirlineAvailabilityInfo flightInfo)
        {
            return flightInfo.ProviderCode;
        }
        private static string ReturnTDS(AirlineAvailabilityInfo firstFlight, string companyID)//==========
        {
            int iTds = 0;
            if (companyID.IndexOf("-SA-") != -1 || companyID.IndexOf("C-") != -1 || companyID == "")
            {
                iTds = firstFlight.TotalTds_SA;
            }
            else
            {
                iTds = firstFlight.TotalTds;
            }

            if (HttpContextHelper.Current?.Session.GetString("Curr") == null ||
                HttpContextHelper.Current.Session.GetString("Curr").Equals("INR"))
            {
                return iTds.ToString();
            }
            else
            {
                double dCurrency = 1;
                if (HttpContextHelper.Current.Session.GetString("dCurrency") != null)
                {
                    dCurrency = Convert.ToDouble(HttpContextHelper.Current.Session.GetString("dCurrency"));
                }
                return NumberHelper.AvgAmount1((iTds * dCurrency).ToString()).ToString();
            }
        }
        private static string ReturnCommission(AirlineAvailabilityInfo firstFlight, string companyID)
        {
            int commission = 0;

            if (firstFlight == null)
            {
                return commission.ToString();
            }

            if (companyID.Contains("-SA-") || companyID.Contains("C-") || companyID == "")
            {
                commission = firstFlight.TotalCommission_SA;
            }
            else
            {
                commission = firstFlight.TotalCommission;
            }

            if (HttpContextHelper.Current?.Session.GetString("Curr") == null ||
                HttpContextHelper.Current.Session.GetString("Curr").Equals("INR"))
            {
                return commission.ToString();
            }
            else
            {
                double dCurrency = 1;
                if (HttpContextHelper.Current.Session.GetString("dCurrency") != null)
                {
                    dCurrency = Convert.ToDouble(HttpContextHelper.Current.Session.GetString("dCurrency"));
                }
                return NumberHelper.AvgAmount1((commission * dCurrency).ToString()).ToString();
            }
        }
        private static string ReturnRefundType(AirlineAvailabilityInfo firstFlight)
        {
            return string.IsNullOrWhiteSpace(firstFlight.RefundType) || firstFlight.RefundType.Trim() == "Y"
                ? "N"
                : "R";
        }
        private static string ReturnCheckINBaggage(AirlineAvailabilityInfo firstFlight)
        {
            if (firstFlight == null || string.IsNullOrWhiteSpace(firstFlight.BaggageDetail))
            {
                return string.Empty;
            }

            string checkIN = string.Empty;
            if (firstFlight.BaggageDetail.Contains("*"))
            {
                string[] bag = firstFlight.BaggageDetail.Split('*');
                if (bag.Length > 0)
                {
                    checkIN = bag[0];
                }
            }
            else
            {
                checkIN = firstFlight.BaggageDetail;
            }

            return checkIN;
        }
        private static string ReturnCabinINBaggage(AirlineAvailabilityInfo firstFlight)
        {
            if (firstFlight == null || string.IsNullOrWhiteSpace(firstFlight.BaggageDetail))
            {
                return string.Empty;
            }

            string cabIN = string.Empty;
            if (firstFlight.BaggageDetail.Contains("*"))
            {
                string[] bag = firstFlight.BaggageDetail.Split('*');
                if (bag.Length > 1)
                {
                    cabIN = bag[1];
                }
            }
            else
            {
                cabIN = firstFlight.BaggageDetail;
            }

            return cabIN;
        }
        public static int GetTotalTax(AirlineAvailabilityInfo firstFlight)
        {
            if (firstFlight == null)
            {
                return 0;
            }

            int totalTax = firstFlight.TotalTax;

            if (HttpContextHelper.Current?.Session.GetString("Curr") == null ||
                HttpContextHelper.Current.Session.GetString("Curr").Equals("INR"))
            {
                return totalTax;
            }
            else
            {
                int adtTotalTax = firstFlight.Adt * GetConvert(firstFlight.AdtTotalTax);
                int chdTotalTax = firstFlight.Chd * GetConvert(firstFlight.ChdTotalTax);
                int infTotalTax = firstFlight.Inf * GetConvert(firstFlight.InfTotalTax);

                return (adtTotalTax + chdTotalTax + infTotalTax);
            }
        }
        public static int GetTotalBasic(AirlineAvailabilityInfo firstFlight)
        {
            int TotalBasic = firstFlight.TotalBasic;
            if (HttpContextHelper.Current?.Session.GetString("Curr") == null ||
                HttpContextHelper.Current.Session.GetString("Curr").Equals("INR"))
            {
                return TotalBasic;
            }
            else
            {
                int Adt_BASIC = firstFlight.Adt * GetConvert(firstFlight.Adt_BASIC);
                int Chd_BASIC = firstFlight.Chd * GetConvert(firstFlight.Chd_BASIC);
                int Inf_BASIC = firstFlight.Inf * GetConvert(firstFlight.Inf_BASIC);

                return (Adt_BASIC + Chd_BASIC + Inf_BASIC);
            }
        }
        public static int GetTotalServiceFee(AirlineAvailabilityInfo firstFlight)
        {
            if (firstFlight == null)
            {
                return 0;
            }

            int totalServiceFee = firstFlight.TotalServiceFee;

            if (HttpContextHelper.Current?.Session.GetString("Curr") == null ||
                HttpContextHelper.Current.Session.GetString("Curr").Equals("INR"))
            {
                return totalServiceFee;
            }
            else
            {
                int adtServiceFee = firstFlight.Adt * GetConvert(firstFlight.Adt_SF);
                int chdServiceFee = firstFlight.Chd * GetConvert(firstFlight.Chd_SF);

                return (adtServiceFee + chdServiceFee);
            }
        }
        public static int GetTotalMarkUp(AirlineAvailabilityInfo firstFlight)
        {
            if (firstFlight == null)
            {
                return 0;
            }

            int totalMarkup = firstFlight.TotalMarkup;

            if (HttpContextHelper.Current?.Session.GetString("Curr") == null ||
                HttpContextHelper.Current.Session.GetString("Curr").Equals("INR"))
            {
                return totalMarkup;
            }
            else
            {
                int adtMarkup = firstFlight.Adt * GetConvert(firstFlight.Adt_MU);
                int chdMarkup = firstFlight.Chd * GetConvert(firstFlight.Chd_MU);

                return (adtMarkup + chdMarkup);
            }
        }
        public static int GetTotalServiceTax(AirlineAvailabilityInfo firstFlight)
        {
            int TotalServiceTax = firstFlight.TotalServiceTax;
            string currency = HttpContextHelper.Current?.Session.GetString("Curr") ?? "INR";

            if (currency.Equals("INR"))
            {
                return TotalServiceTax;
            }
            else
            {
                int NoOFAdult = firstFlight.Adt;
                int NoOFChild = firstFlight.Chd;

                int Adt_ST = NoOFAdult * GetConvert(firstFlight.Adt_ST);
                int Chd_ST = NoOFChild * GetConvert(firstFlight.Chd_ST);

                return (Adt_ST + Chd_ST);
            }
        }
        public static int GetTotalFare(AirlineAvailabilityInfo firstFlight)
        {
            int totalFare = firstFlight.TotalFare;
            string currency = HttpContextHelper.Current?.Session.GetString("Curr") ?? "INR";

            if (currency.Equals("INR"))
            {
                return totalFare;
            }
            else
            {
                int noOfAdult = firstFlight.Adt;
                int noOfChild = firstFlight.Chd;
                int noOfInfant = firstFlight.Inf;

                int adtBasic = noOfAdult * GetConvert(firstFlight.Adt_BASIC);
                int chdBasic = noOfChild * GetConvert(firstFlight.Chd_BASIC);
                int infBasic = noOfInfant * GetConvert(firstFlight.Inf_BASIC);

                int adtTotalTax = noOfAdult * GetConvert(firstFlight.AdtTotalTax);
                int chdTotalTax = noOfChild * GetConvert(firstFlight.ChdTotalTax);
                int infTotalTax = noOfInfant * GetConvert(firstFlight.InfTotalTax);

                int adtMU = noOfAdult * GetConvert(firstFlight.Adt_MU);
                int chdMU = noOfChild * GetConvert(firstFlight.Chd_MU);

                int adtSF = noOfAdult * GetConvert(firstFlight.Adt_SF);
                int chdSF = noOfChild * GetConvert(firstFlight.Chd_SF);

                int adtST = noOfAdult * GetConvert(firstFlight.Adt_ST);
                int chdST = noOfChild * GetConvert(firstFlight.Chd_ST);

                int totalTds = GetConvert(firstFlight.TotalTds);

                int total = (adtTotalTax + chdTotalTax + infTotalTax);
                total += (adtBasic + chdBasic + infBasic);
                total += (adtMU + chdMU);
                total += (adtSF + chdSF);
                total += (adtST + chdST);
                total += totalTds;

                return total;
            }
        }
        private static int GetTaxandCharges(AirlineAvailabilityInfo firstFlight)
        {
            if (HttpContextHelper.Current?.Session.GetString("Curr") == null || HttpContextHelper.Current.Session.GetString("Curr").Equals("INR"))
            {
                int TotalTax = firstFlight.TotalTax;
                TotalTax += firstFlight.TotalMarkup;
                TotalTax += firstFlight.TotalServiceFee;
                TotalTax += firstFlight.TotalServiceTax;
                return TotalTax;
            }
            else
            {
                int NoOFAdult = firstFlight.Adt;
                int NoOFChild = firstFlight.Chd;
                int NoOFInfant = firstFlight.Inf;

                int AdtTotalTax = NoOFAdult * GetConvert(firstFlight.AdtTotalTax);
                int ChdTotalTax = NoOFChild * GetConvert(firstFlight.ChdTotalTax);
                int InfTotalTax = NoOFInfant * GetConvert(firstFlight.InfTotalTax);

                int Adt_MU = NoOFAdult * GetConvert(firstFlight.Adt_MU);
                int Chd_MU = NoOFChild * GetConvert(firstFlight.Chd_MU);

                int Adt_SF = NoOFAdult * GetConvert(firstFlight.Adt_SF);
                int Chd_SF = NoOFChild * GetConvert(firstFlight.Chd_SF);

                int Adt_ST = NoOFAdult * GetConvert(firstFlight.Adt_ST);
                int Chd_ST = NoOFChild * GetConvert(firstFlight.Chd_ST);

                int total = (AdtTotalTax + ChdTotalTax + InfTotalTax);
                total += (Adt_MU + Chd_MU);
                total += (Adt_SF + Chd_SF);
                total += (Adt_ST + Chd_ST);

                return total;
            }
        }
        public static string ReturnGrossFare(AirlineAvailabilityInfo firstFlight)
        {
            if (HttpContextHelper.Current?.Session.GetString("Curr") == null || HttpContextHelper.Current.Session.GetString("Curr").Equals("INR"))
            {
                int totalFare = firstFlight.TotalFare;
                int totalTds = firstFlight.TotalTds;
                return (totalFare - totalTds).ToString();
            }
            else
            {
                int NoOFAdult = firstFlight.Adt;
                int NoOFChild = firstFlight.Chd;
                int NoOFInfant = firstFlight.Inf;

                int Adt_BASIC = NoOFAdult * GetConvert(firstFlight.Adt_BASIC);
                int Chd_BASIC = NoOFChild * GetConvert(firstFlight.Chd_BASIC);
                int Inf_BASIC = NoOFInfant * GetConvert(firstFlight.Inf_BASIC);

                int AdtTotalTax = NoOFAdult * GetConvert(firstFlight.AdtTotalTax);
                int ChdTotalTax = NoOFChild * GetConvert(firstFlight.ChdTotalTax);
                int InfTotalTax = NoOFInfant * GetConvert(firstFlight.InfTotalTax);

                int Adt_MU = NoOFAdult * GetConvert(firstFlight.Adt_MU);
                int Chd_MU = NoOFChild * GetConvert(firstFlight.Chd_MU);

                int Adt_SF = NoOFAdult * GetConvert(firstFlight.Adt_SF);
                int Chd_SF = NoOFChild * GetConvert(firstFlight.Chd_SF);

                int Adt_ST = NoOFAdult * GetConvert(firstFlight.Adt_ST);
                int Chd_ST = NoOFChild * GetConvert(firstFlight.Chd_ST);

                int total = (AdtTotalTax + ChdTotalTax + InfTotalTax);
                total += (Adt_BASIC + Chd_BASIC + Inf_BASIC);
                total += (Adt_MU + Chd_MU);
                total += (Adt_SF + Chd_SF);
                total += (Adt_ST + Chd_ST);

                return total.ToString();
            }
        }

        private static int ReturnTotalFareAmount(AirlineAvailabilityInfo firstFlight)
        {
            string currency = HttpContextHelper.Current?.Session.GetString("Curr") ?? "INR";

            if (currency.Equals("INR"))
            {
                return firstFlight.TotalFare - firstFlight.TotalTds;
            }
            else
            {
                int noOfAdult = firstFlight.Adt;
                int noOfChild = firstFlight.Chd;
                int noOfInfant = firstFlight.Inf;

                int adtBasic = noOfAdult * GetConvert(firstFlight.Adt_BASIC);
                int chdBasic = noOfChild * GetConvert(firstFlight.Chd_BASIC);
                int infBasic = noOfInfant * GetConvert(firstFlight.Inf_BASIC);

                int adtTotalTax = noOfAdult * GetConvert(firstFlight.AdtTotalTax);
                int chdTotalTax = noOfChild * GetConvert(firstFlight.ChdTotalTax);
                int infTotalTax = noOfInfant * GetConvert(firstFlight.InfTotalTax);

                int adtMU = noOfAdult * GetConvert(firstFlight.Adt_MU);
                int chdMU = noOfChild * GetConvert(firstFlight.Chd_MU);

                int adtSF = noOfAdult * GetConvert(firstFlight.Adt_SF);
                int chdSF = noOfChild * GetConvert(firstFlight.Chd_SF);

                int adtST = noOfAdult * GetConvert(firstFlight.Adt_ST);
                int chdST = noOfChild * GetConvert(firstFlight.Chd_ST);

                int total = (adtTotalTax + chdTotalTax + infTotalTax);
                total += (adtBasic + chdBasic + infBasic);
                total += (adtMU + chdMU);
                total += (adtSF + chdSF);
                total += (adtST + chdST);

                return total;
            }
        }
        public static int ReturnFinalFare(AirlineAvailabilityInfo firstFlight, string companyID)
        {
            string currency = HttpContextHelper.Current?.Session.GetString("Curr") ?? "INR";

            if (currency.Equals("INR"))
            {
                int totalFare = firstFlight.TotalFare - firstFlight.TotalTds;
                int commission = companyID.Contains("-SA-") || companyID.Contains("C-") || string.IsNullOrEmpty(companyID)
                    ? firstFlight.TotalCommission_SA
                    : firstFlight.TotalCommission;

                int finalFare = totalFare - commission;
                if ((companyID.Contains("A-") || companyID.Contains("-SA-")) && !companyID.Contains("C-"))
                {
                    finalFare -= firstFlight.TotalMarkup;
                }

                return finalFare;
            }
            else
            {
                int noOfAdult = firstFlight.Adt;
                int noOfChild = firstFlight.Chd;
                int noOfInfant = firstFlight.Inf;

                int adtBasic = noOfAdult * GetConvert(firstFlight.Adt_BASIC);
                int chdBasic = noOfChild * GetConvert(firstFlight.Chd_BASIC);
                int infBasic = noOfInfant * GetConvert(firstFlight.Inf_BASIC);

                int adtTotalTax = noOfAdult * GetConvert(firstFlight.AdtTotalTax);
                int chdTotalTax = noOfChild * GetConvert(firstFlight.ChdTotalTax);
                int infTotalTax = noOfInfant * GetConvert(firstFlight.InfTotalTax);

                int adtMU = noOfAdult * GetConvert(firstFlight.Adt_MU);
                int chdMU = noOfChild * GetConvert(firstFlight.Chd_MU);

                int adtSF = noOfAdult * GetConvert(firstFlight.Adt_SF);
                int chdSF = noOfChild * GetConvert(firstFlight.Chd_SF);

                int adtST = noOfAdult * GetConvert(firstFlight.Adt_ST);
                int chdST = noOfChild * GetConvert(firstFlight.Chd_ST);

                int total = adtTotalTax + chdTotalTax + infTotalTax +
                            adtBasic + chdBasic + infBasic +
                            adtMU + chdMU +
                            adtSF + chdSF +
                            adtST + chdST;

                int commission = companyID.Contains("-SA-") || companyID.Contains("C-") || string.IsNullOrEmpty(companyID)
                    ? GetConvert(firstFlight.TotalCommission_SA)
                    : GetConvert(firstFlight.TotalCommission);

                int finalFare = total - commission;
                if ((companyID.Contains("A-") || companyID.Contains("-SA-")) && !companyID.Contains("C-"))
                {
                    finalFare -= adtMU + chdMU;
                }

                return finalFare;
            }
        }
        public static Int32 ReturnFinalFare(DataRow dr, string companyID)//==========
        {
            string currency = HttpContextHelper.Current?.Session.GetString("Curr") ?? "INR";
            string CompanyID = companyID;
            if (currency.Equals("INR"))

            { 
                int FinalFare = 0;
                
                int TotFarewithoutTds = 0;
                int TotFare = 0;
                if (CompanyID.IndexOf("-SA-") != -1 || CompanyID.IndexOf("C-") != -1 || CompanyID == "")
                {
                    TotFarewithoutTds = int.Parse(dr["TotalFare"].ToString()) - int.Parse(dr["TotalTds"].ToString());
                    TotFare = TotFarewithoutTds - int.Parse(dr["TotalCommission_SA"].ToString());
                }
                else
                {
                    TotFarewithoutTds = int.Parse(dr["TotalFare"].ToString()) - int.Parse(dr["TotalTds"].ToString());
                    TotFare = TotFarewithoutTds - int.Parse(dr["TotalCommission"].ToString());
                }

                if ((CompanyID.IndexOf("A-") > -1 || CompanyID.IndexOf("-SA-") > -1) && CompanyID.IndexOf("C-") == -1)
                {
                    FinalFare = TotFare - int.Parse(dr["TotalMarkup"].ToString());
                }
                else
                {
                    FinalFare = TotFare;
                }

                return FinalFare;
            }
            else
            {
                int NoOFAdult = int.Parse(dr["Adt"].ToString());
                int NoOFChild = int.Parse(dr["Chd"].ToString());
                int NoOFInfant = int.Parse(dr["Inf"].ToString());

                int Adt_BASIC = NoOFAdult * GetConvert(Convert.ToInt32(dr["Adt_BASIC"].ToString()));
                int Chd_BASIC = NoOFChild * GetConvert(Convert.ToInt32(dr["Chd_BASIC"].ToString()));
                int Inf_BASIC = NoOFInfant * GetConvert(Convert.ToInt32(dr["Inf_BASIC"].ToString()));

                int AdtTotalTax = NoOFAdult * GetConvert(Convert.ToInt32(dr["AdtTotalTax"].ToString()));
                int ChdTotalTax = NoOFChild * GetConvert(Convert.ToInt32(dr["ChdTotalTax"].ToString()));
                int InfTotalTax = NoOFInfant * GetConvert(Convert.ToInt32(dr["InfTotalTax"].ToString()));

                int Adt_MU = NoOFAdult * GetConvert(Convert.ToInt32(dr["Adt_MU"].ToString()));
                int Chd_MU = NoOFChild * GetConvert(Convert.ToInt32(dr["Chd_MU"].ToString()));

                int Adt_SF = NoOFAdult * GetConvert(Convert.ToInt32(dr["Adt_SF"].ToString()));
                int Chd_SF = NoOFChild * GetConvert(Convert.ToInt32(dr["Chd_SF"].ToString()));

                int Adt_ST = NoOFAdult * GetConvert(Convert.ToInt32(dr["Adt_ST"].ToString()));
                int Chd_ST = NoOFChild * GetConvert(Convert.ToInt32(dr["Chd_ST"].ToString()));

                int total = (AdtTotalTax + ChdTotalTax + InfTotalTax);
                total += (Adt_BASIC + Chd_BASIC + Inf_BASIC);
                total += (Adt_MU + Chd_MU);
                total += (Adt_SF + Chd_SF);
                total += (Adt_ST + Chd_ST);

                int TotFarewithoutTds = 0;
                int TotFare = 0;

               
                if (CompanyID.IndexOf("-SA-") != -1 || CompanyID.IndexOf("C-") != -1 || CompanyID == "")
                {
                    TotFarewithoutTds = total; //- GetConvert(int.Parse(dr["TotalTds"].ToString()));
                    TotFare = TotFarewithoutTds - GetConvert(int.Parse(dr["TotalCommission_SA"].ToString()));
                }
                else
                {
                    TotFarewithoutTds = total; //- GetConvert(int.Parse(dr["TotalTds"].ToString()));
                    TotFare = TotFarewithoutTds - GetConvert(int.Parse(dr["TotalCommission"].ToString()));
                }

                if ((CompanyID.IndexOf("A-") > -1 || CompanyID.IndexOf("-SA-") > -1) && CompanyID.IndexOf("C-") == -1)
                {
                    total = TotFare - (Adt_MU + Chd_MU);
                }
                else
                {
                    total = TotFare;
                }

                return total;
            }
        }
      
        private static string ReturnCls(AirlineAvailabilityInfo firstFlight)
        {
            return firstFlight.ClassOfService;
        }
        private static string checkSeat(string seat)
        {
            if (seat != null && seat != string.Empty)
            {
                if (int.Parse(seat) > 9)
                {
                    seat = "9";
                }
                return seat;
            }
            else
            {
                return "0";
            }
        }
        private static string checkVia(string Via)
        {
            if (Via != null && Via != string.Empty)
            {
                //return "Via : " + Via;
                return Via;
            }
            else
            {
                return "";
            }

        }
        private static string checkViaName(string ViaName)
        {
            if (ViaName != null && ViaName != string.Empty)
            {
                if (ViaName.IndexOf("AADT") != -1)
                {
                    return "";
                }
                else
                {
                    return ViaName;
                }
            }
            else
            {
                return "1 technical stop";
            }

        }
        private static string checkTerminal(string Terminal)
        {
            if (Terminal != null && Terminal != string.Empty)
            {
                return "Terminal : " + Terminal;
            }
            else
            {
                return "";
            }

        }

        public static Task<List<FlightStopsFilterResponse>> FlightStop(string flterType)
        {
            List<FlightStopsFilterResponse> stp = new List<FlightStopsFilterResponse>();
            if (flterType == "OUTBOUND")
            {
                string stops = HttpContextHelper.Current?.Session.GetString("StopCheck") ?? string.Empty;
                if (stops != string.Empty)
                {
                    string[] stopssplit = stops.Split(',');

                    for (int i = 0; i < stopssplit.Length; i++)
                    {
                        stp.Add(new FlightStopsFilterResponse()
                        {
                            Stops = stopssplit[i].ToString(),
                            NoOfStops = 1
                        });
                    }
                }
                HttpContextHelper.Current?.Session.Remove("StopCheck");

            }
            else if (flterType == "INBOUND")
            {
                string stops = HttpContextHelper.Current?.Session.GetString("StopCheckR") ?? string.Empty;
                string[] stopssplit = stops.Split(',');

                for (int i = 0; i < stopssplit.Length; i++)
                {
                    stp.Add(new FlightStopsFilterResponse()
                    {
                        Stops = stopssplit[i].ToString(),
                        NoOfStops = 1
                    });
                }
                HttpContextHelper.Current?.Session.Remove("StopCheckR");
            }
            return Task.FromResult(stp);
        }

        public static Task<List<FlightMatrixFilterResponse>> FlightMatrix()
        {
            var flightMatrixFilterResponse = new List<FlightMatrixFilterResponse>();
            var flightMatrixInfo = new List<FlightMatrixInfo>();

            var airlineAvailabilityInfo = GetSelec("O");
            var sortedFlights = airlineAvailabilityInfo.OrderBy(f => f.TotalFare).ThenBy(f => f.RefID).ToList();

            var distinctRefIds = sortedFlights
                .GroupBy(f => f.RefID)
                .Select(g => g.Key)
                .ToList();

            foreach (var refId in distinctRefIds)
            {
                var relatedFlights = sortedFlights
                    .Where(f => f.RefID == refId)
                    .ToList();


                var flight = relatedFlights.First();
                flightMatrixInfo.Add(new FlightMatrixInfo
                {
                    FlightIndexRef = flight.RefID.ToString(),
                    Src = flight.Origin,
                    Dest = flight.Destination,
                    CarrierCode = flight.CarrierCode,
                    TotalAmount = flight.TotalFare - flight.TotalTds,
                    DeptTime = flight.DepartureTime.ToString(),
                    Stops = flight.Stops
                });

            }

            flightMatrixInfo = flightMatrixInfo.OrderBy(f => f.TotalAmount).ToList();
            var allFlights = new List<FlightMatrixInfo>();
            var morningFlights = flightMatrixInfo.Where(f => IsTimeInRange(f.DeptTime, 500, 1200)).ToList();
            allFlights.AddRange(morningFlights);
            var afternoonFlights = flightMatrixInfo.Where(f => IsTimeInRange(f.DeptTime, 1200, 1800)).ToList();
            allFlights.AddRange(afternoonFlights);
            var nightFlights = flightMatrixInfo.Where(f => IsTimeInRange(f.DeptTime, 1800, 2400)).ToList();
            allFlights.AddRange(nightFlights);
            var midnightFlights = flightMatrixInfo.Where(f => IsTimeInRange(f.DeptTime, 0, 500)).ToList();
            allFlights.AddRange(midnightFlights);

            var distinctCarriers = flightMatrixInfo.Select(f => f.CarrierCode).Distinct().ToList();

            foreach (var carrier in distinctCarriers)
            {
                var flights = allFlights.Where(f => f.CarrierCode == carrier).ToList();
                var totalFareM = GetFareForTimeType(morningFlights, carrier);
                var totalFareA = GetFareForTimeType(afternoonFlights, carrier);
                var totalFareN = GetFareForTimeType(nightFlights, carrier);
                var totalFareMN = GetFareForTimeType(midnightFlights, carrier);


                flightMatrixFilterResponse.Add(new FlightMatrixFilterResponse
                {

                    FlightRefid = flights.FirstOrDefault().FlightIndexRef,
                    Airlines = carrier,
                    Fare = flights.FirstOrDefault()?.TotalAmount.ToString(),
                    Airlinespath = carrier + ".gif",
                    FareM = totalFareM ?? "---",
                    FareA = totalFareA ?? "---",
                    FareN = totalFareN ?? "---",
                    FareMN = totalFareMN ?? "---",
                });
            }

            return Task.FromResult(flightMatrixFilterResponse);
        }

        public static Task<List<FlightMatrixFilterResponse>> FlightMatrixRoundInt()
        {
            var flightMatrixFilterResponse = new List<FlightMatrixFilterResponse>();
            var flightMatrixInfo = new List<FlightMatrixInfo>();

            var airlineAvailabilityInfo = GetSelec("O");
            var sortedFlights = airlineAvailabilityInfo.OrderBy(f => f.TotalFare).ThenBy(f => f.RefID).ToList();

            var distinctRefIds = sortedFlights
                .GroupBy(f => f.RefID)
                .Select(g => g.Key)
                .ToList();

            foreach (var refId in distinctRefIds)
            {
                var relatedFlights = sortedFlights
                    .Where(f => f.RefID == refId)
                    .ToList();


                var flight = relatedFlights.First();
                flightMatrixInfo.Add(new FlightMatrixInfo
                {
                    FlightIndexRef = flight.RefID.ToString(),
                    Src = flight.Origin,
                    Dest = flight.Destination,
                    CarrierCode = flight.CarrierCode,
                    TotalAmount = flight.TotalFare - flight.TotalTds,
                    DeptTime = flight.DepartureTime.ToString(),
                    Stops = flight.Stops
                });

            }

            flightMatrixInfo = flightMatrixInfo.OrderBy(f => f.TotalAmount).ToList();
            var allFlights = new List<FlightMatrixInfo>();
            var morningFlights = flightMatrixInfo.Where(f => IsTimeInRange(f.DeptTime, 500, 1200)).ToList();
            allFlights.AddRange(morningFlights);
            var afternoonFlights = flightMatrixInfo.Where(f => IsTimeInRange(f.DeptTime, 1200, 1800)).ToList();
            allFlights.AddRange(afternoonFlights);
            var nightFlights = flightMatrixInfo.Where(f => IsTimeInRange(f.DeptTime, 1800, 2400)).ToList();
            allFlights.AddRange(nightFlights);
            var midnightFlights = flightMatrixInfo.Where(f => IsTimeInRange(f.DeptTime, 0, 500)).ToList();
            allFlights.AddRange(midnightFlights);

            var distinctCarriers = flightMatrixInfo.Select(f => f.CarrierCode).Distinct().ToList();

            foreach (var carrier in distinctCarriers)
            {
                var flights = allFlights.Where(f => f.CarrierCode == carrier).ToList();
                var totalFareM = GetFareForTimeType(morningFlights, carrier);
                var FlightRefidM = GetlightRefidForTimeType(morningFlights, carrier);
                var totalFareA = GetFareForTimeType(afternoonFlights, carrier);
                var FlightRefidA = GetlightRefidForTimeType(afternoonFlights, carrier);
                var totalFareN = GetFareForTimeType(nightFlights, carrier);
                var FlightRefidN = GetlightRefidForTimeType(nightFlights, carrier);
                var totalFareMN = GetFareForTimeType(midnightFlights, carrier);
                var FlightRefidMN = GetlightRefidForTimeType(midnightFlights, carrier);


                flightMatrixFilterResponse.Add(new FlightMatrixFilterResponse
                {

                    FlightRefid = flights.FirstOrDefault().FlightIndexRef,
                    Airlines = carrier,
                    Fare = flights.FirstOrDefault()?.TotalAmount.ToString(),
                    Airlinespath = carrier + ".gif",
                    FareM = totalFareM ?? "---",
                    FareA = totalFareA ?? "---",
                    FareN = totalFareN ?? "---",
                    FareMN = totalFareMN ?? "---",
                    FlightRefidM = FlightRefidM ?? string.Empty,
                    FlightRefidA = FlightRefidA ?? string.Empty,
                    FlightRefidN = FlightRefidN ?? string.Empty,
                    FlightRefidMN = FlightRefidMN ?? string.Empty
                });
            }

            return Task.FromResult(flightMatrixFilterResponse);
        
        }

        private static bool IsTimeInRange(string time, int start, int end)
        {
            int timeValue = int.Parse(time.Replace(":", "").Trim());
            return timeValue > start && timeValue <= end;
        }

        private static string GetFareForTimeType(List<FlightMatrixInfo> flights, string carrier)
        {
            var flight = flights.FirstOrDefault(f => f.CarrierCode == carrier);
            return flight?.TotalAmount.ToString();
        }

        private static string GetlightRefidForTimeType(List<FlightMatrixInfo> flights, string carrier)
        {
            var flight = flights.FirstOrDefault(f => f.CarrierCode == carrier);
            return flight?.FlightIndexRef.ToString();
        }

        public static Task<List<PreferAirlinesFilterResponse>> PreferAirlines(string flterType, string SearchType)
        {
            var preferAirlinesFilterResponse = new List<PreferAirlinesFilterResponse>();
            var preferAirlinesList = GetPreferAirlines(flterType, SearchType);

            var distinctRefIds = preferAirlinesList.Select(info => info.RefId).Distinct().ToList();
            var distinctFlights = new List<(string FlightName, string Logo)>();

            foreach (var refId in distinctRefIds)
            {
                var relatedFlights = preferAirlinesList.Where(info => info.RefId == refId).ToList();

                if (relatedFlights.Count >= 1)
                {
                    var firstFlight = relatedFlights.First();
                    distinctFlights.Add((firstFlight.FlightName, firstFlight.Logo));
                }

                if (relatedFlights.Count == 2)
                {
                    distinctFlights.Add((relatedFlights[1].FlightName, relatedFlights[1].Logo));
                }

                if (relatedFlights.Count == 3)
                {
                    distinctFlights.Add((relatedFlights[2].FlightName, relatedFlights[2].Logo));
                }
            }

            var distinctFlightNames = distinctFlights
                .GroupBy(f => new { f.FlightName, f.Logo })
                .Select(g => g.First())
                .ToList();

            foreach (var flight in distinctFlightNames)
            {
                preferAirlinesFilterResponse.Add(new PreferAirlinesFilterResponse
                {
                    Airlines = flight.FlightName,
                    Airlinespath = flight.Logo,
                    NoOfAirlines = 1
                });
            }

            return Task.FromResult(preferAirlinesFilterResponse.OrderBy(p => p.Airlines).ToList());
        }

        public static Task<List<PreferAirlinesFilterResponseV1>> PreferAirlinesV1(string flterType, string SearchType)
        {
            var preferAirlinesFilterResponse = new List<PreferAirlinesFilterResponseV1>();
            var preferAirlinesList = GetPreferAirlinesV1(flterType, SearchType);

            var distinctRefIds = preferAirlinesList.Select(info => info.RefId).Distinct().ToList();
            var distinctFlights = new List<(string FlightCode, string Logo, int TotalFare, int TotalTds, int TotalTdsSA)>();

            foreach (var refId in distinctRefIds)
            {
                var relatedFlights = preferAirlinesList.Where(info => info.RefId == refId).ToList();

                if (relatedFlights.Count >= 1)
                {
                    var firstFlight = relatedFlights.First();
                    distinctFlights.Add((firstFlight.FlightCode, firstFlight.Logo, firstFlight.TotalFare, firstFlight.TotalTds, firstFlight.TotalTdsSA));
                }

                if (relatedFlights.Count == 2)
                {
                    distinctFlights.Add((relatedFlights[1].FlightCode, relatedFlights[1].Logo, relatedFlights[1].TotalFare, relatedFlights[1].TotalTds, relatedFlights[1].TotalTdsSA));
                }

                if (relatedFlights.Count == 3)
                {
                    distinctFlights.Add((relatedFlights[2].FlightCode, relatedFlights[2].Logo, relatedFlights[2].TotalFare, relatedFlights[2].TotalTds, relatedFlights[2].TotalTdsSA));
                }
            }

            var distinctFlightNames = distinctFlights
                .GroupBy(f => new { f.FlightCode, f.Logo })
                .Select(g => g.First())
                .ToList();

            foreach (var flight in distinctFlightNames)
            {
                preferAirlinesFilterResponse.Add(new PreferAirlinesFilterResponseV1
                {
                    Airlines = flight.FlightCode,
                    Airlinespath = flight.Logo,
                    TotalFare = flight.TotalFare - flight.TotalTds,
                    NoOfAirlines = 1
                });
            }

            return Task.FromResult(preferAirlinesFilterResponse.OrderBy(p => p.Airlines).ToList());
        }

        private static List<PreferAirlinesFilterInfoV1> GetPreferAirlinesV1(string FlightType, string ArrivalType)
        {
            var airlineAvailabilityInfo = FlightType.Equals("INBOUND") ? GetSelec("I") : GetSelec("O");

            var preferAirlinesList = new List<PreferAirlinesFilterInfoV1>();

            var refIds = airlineAvailabilityInfo.Select(info => info.RefID).Distinct().ToList();

            string fltType = FlightType.Equals("INBOUND") ? "I" : "O";

            foreach (var refId in refIds)
            {
                var flights = airlineAvailabilityInfo.Where(info => info.RefID == refId && info.FltType == fltType).OrderBy(info => info.RowID).ToList();

                if (flights.Any())
                {
                    var flight = flights.First();
                    var preferAirline = new PreferAirlinesFilterInfoV1
                    {
                        RowId = flight.RowID.ToString(),
                        RefId = flight.RefID.ToString().Trim(),
                        Logo = $"/assets/img/airlogo_square/{flight.CarrierCode}.gif",
                        Provider = flight.PriceType.Trim(),
                        ProviderType = flight.PriceType.Trim(),
                        DepDateDesc = flight.DepartureDate.Trim(),
                        ArrDateDesc = flight.ArrivalDate.Trim(),
                        Description = flight.PriceType.Trim(),
                        TotalFare = flight.TotalFare,
                        FlightCode = flight.CarrierCode.Trim(),
                        TotalTds = flight.TotalTds,
                        TotalTdsSA = flight.TotalTds_SA

                    };

                    preferAirlinesList.Add(preferAirline);
                }
            }

            return preferAirlinesList;
        }
        private static List<PreferAirlinesFilterInfo> GetPreferAirlines(string FlightType, string ArrivalType)
        {
            var airlineAvailabilityInfo = FlightType.Equals("INBOUND") ? GetSelec("I") : GetSelec("O");

            var preferAirlinesList = new List<PreferAirlinesFilterInfo>();

            var refIds = airlineAvailabilityInfo.Select(info => info.RefID).Distinct().ToList();

            string fltType = FlightType.Equals("INBOUND") ? "I" : "O";

            foreach (var refId in refIds)
            {
                var flights = airlineAvailabilityInfo.Where(info => info.RefID == refId && info.FltType == fltType).OrderBy(info => info.RowID).ToList();

                if (flights.Any())
                {
                    var flight = flights.First();
                    var preferAirline = new PreferAirlinesFilterInfo
                    {
                        RowId = flight.RowID.ToString(),
                        RefId = flight.RefID.ToString().Trim(),
                        Logo = $"/assets/img/airlogo_square/{flight.CarrierCode}.gif",
                        Provider = flight.PriceType.Trim(),
                        ProviderType = flight.PriceType.Trim(),
                        DepDateDesc = flight.DepartureDate.Trim(),
                        ArrDateDesc = flight.ArrivalDate.Trim(),
                        Description = flight.PriceType.Trim(),
                        TotalFare = int.Parse(flight.TotalFare.ToString().Trim()),
                        FlightName = flight.CarrierCode.Trim()
                    };

                    preferAirlinesList.Add(preferAirline);
                }
            }

            return preferAirlinesList;
        }

        public static async Task<List<k_ShowFlightOutBound>> SelectOneway(string flterType, string SearchType, string Refid, string CompanyID, ClaimsPrincipal user, IHandlesQueryAsync<GetAirFareQuery, string> getAirFareHandler, IHandlesQueryAsync<GetAirFareRulesQuery, string> getAirFareRulesHandler, IHandlesQueryAsync<string, CompanyRegisterCorporateUserDetails> getCompanyRegisterCorporateUserDetailsQueryHandler, IHandlesQueryAsync<string, CompanyRegisterCorporateUserLimitDetails> getCompanyRegisterCorporateUserLimitQueryHandler)
        {
            List<k_ShowFlightOutBound> FlightOutBound = new List<k_ShowFlightOutBound>();
            //=============================================================================================================

            var userInfo = await SelectedFlightUserInfo(user, Refid, getCompanyRegisterCorporateUserDetailsQueryHandler, getCompanyRegisterCorporateUserLimitQueryHandler);
            var fStatus = userInfo.ValidTimeUser;
            string fRemark = userInfo.Remark;
            int Limit = userInfo.Limit;

            if (fStatus)
            {

                var getfareResponse = await ShowFlightFareHelper.GetFare(CompanyID, Refid, "O", getAirFareHandler, getAirFareRulesHandler);
                fStatus = getfareResponse.FareStatus;
                if (!getfareResponse.FareStatus)
                {
                    fRemark = getfareResponse.FareRemark;
                }
            }


            //=============================================================================================================

            HttpContextHelper.Current.Session.SetString("SelectedFltOut", Refid);

            var flightList = new List<AirlineAvailabilityInfo>();
            if (flterType == "OUTBOUND")
            {
                flightList = GetSelec("O");
            }
            else
            {
                flightList = GetSelec("I");
            }



            var relatedFlights = flightList.Where(x => x.RefID == Convert.ToInt32(Refid)).ToList();

            if (relatedFlights.Any())
            {
                string curr = "INR";
                // Check session or alternative storage for currency
                if (HttpContextHelper.Current?.Session.GetString("Curr") != null)
                {
                    curr = HttpContextHelper.Current.Session.GetString("Curr");
                }
                string rules = "Fare Rules are just a guideline for your convenience and is subject to changes by the Airline from time to time.For Any query related to Fare Rules please contact to our callcenter.";
                string defaultRule = string.Empty;
                int arrivalCheck = 0;
                int promo = 0;
                int checkA_C = 0;


                if (relatedFlights.Count == 1)
                {
                    // Perform necessary processing on relatedFlights

                    var relatedFlight = relatedFlights.FirstOrDefault();
                    rules = GetFareRules(relatedFlights);
                    string departureDate = relatedFlight.DepartureDate.ToString(); // Assuming DepartureDate is a property of k_ShowFlightOutBound
                    string arrivalDate = relatedFlight.ArrivalDate.ToString();
                    if (departureDate != arrivalDate)
                    {
                        arrivalCheck = DateHelper.DayDiff(departureDate, arrivalDate);
                    }
                    FlightOutBound.Add(new k_ShowFlightOutBound()
                    {

                        CompanyID = CompanyID,
                        AgentType = checkA_C,
                        F_Status = fStatus,
                        F_Remark = fRemark,
                        Curr = curr,
                        FlightRefid = relatedFlight.RefID.ToString(),

                        NoOFAdult = relatedFlight.Adt,
                        NoOFChild = relatedFlight.Chd,
                        NoOFInfant = relatedFlight.Inf,

                        Adt_BASIC = GetConvert(relatedFlight.Adt_BASIC.ToString()),
                        Chd_BASIC = GetConvert(relatedFlight.Chd_BASIC.ToString()),
                        Inf_BASIC = GetConvert(relatedFlight.Inf_BASIC.ToString()),

                        AdultbaseFare = relatedFlight.Adt * GetConvert(relatedFlight.Adt_BASIC),
                        ChildbaseFare = relatedFlight.Chd * GetConvert(relatedFlight.Chd_BASIC),
                        InfantbaseFare = relatedFlight.Inf * GetConvert(relatedFlight.Inf_BASIC),

                        Adt_YQ = GetConvert(relatedFlight.Adt_YQ),
                        AdtTotalTax = GetConvert(relatedFlight.AdtTotalTax),
                        Chd_YQ = GetConvert(relatedFlight.Chd_YQ),
                        ChdTotalTax = GetConvert(relatedFlight.ChdTotalTax),
                        Inf_TAX = GetConvert(relatedFlight.Inf_TAX),

                        Adt_AcuTax = ReturnTAXADT(relatedFlight).ToString(),
                        Chd_AcuTax = ReturnTAXCHD(relatedFlight).ToString(),
                        Inf_AcuTax = ReturnTAXINF(relatedFlight).ToString(),

                        Yq = "Commission : " + String.Format("{0:N0}", GetConvert(relatedFlight.TotalCommission)),
                        PromoChek = promo,

                        TotalTax = GetTotalTax(relatedFlight),
                        TotalBasic = GetTotalBasic(relatedFlight),
                        TotalServiceFee = GetTotalServiceFee(relatedFlight),
                        TotalMarkUp = GetTotalMarkUp(relatedFlight),
                        TotalServiceTax = GetTotalServiceTax(relatedFlight),
                        TaxandCharges = GetTaxandCharges(relatedFlight),
                        TotalCommission = ReturnCommission(relatedFlight, CompanyID),
                        TotalTds = ReturnTDS(relatedFlight, CompanyID),

                        GrossF = ReturnGrossFare(relatedFlight).ToString(),
                        FinalFare = ReturnFinalFare(relatedFlight, CompanyID),
                        TotalAmount = ReturnTotalFareAmount(relatedFlight),
                        TotalFare = ReturnTotalFareAmount(relatedFlight),

                        FlightName = relatedFlight.CarrierCode,
                        DepartStationName = relatedFlight.DepartureStationName,
                        ArrivalStationName = relatedFlight.ArrivalStationName,
                        DepartureStationAirport = relatedFlight.DepartureStationAirport,
                        ArrivalStationAirport = relatedFlight.ArrivalStationAirport,
                        Cabin = relatedFlight.Cabin,
                        CarrierName = relatedFlight.CarrierName,
                        FlightNumber = relatedFlight.FlightNumber,
                        FlightDepDate = relatedFlight.DepartureDate,
                        FlightDepTime = relatedFlight.DepartureTime,
                        FlightArrDate = relatedFlight.ArrivalDate,
                        FlightArrTime = relatedFlight.ArrivalTime,
                        CHECKINBaggage = ReturnCheckINBaggage(relatedFlight).ToString(),
                        CABINBaggage = ReturnCabinINBaggage(relatedFlight).ToString(),
                        SRC = relatedFlight.Origin,
                        DEST = relatedFlight.Destination,
                        PrimarySRC = relatedFlight.Origin,
                        PrimaryDEST = relatedFlight.Destination,
                        Stop = "NonStop",
                        logo = "/assets/img/airlogo_square/" + relatedFlight.CarrierCode + ".gif",
                        Duration = relatedFlight.DurationDesc,
                        connection = 1,
                        SMSACTIVES = 0,
                        ArrivalNextDayCheck = arrivalCheck,
                        PriceType = relatedFlight.PriceType,
                        RefundType = ReturnRefundType(relatedFlight).ToString(),
                        AvailableSeat = checkSeat(relatedFlight.SeatsAvailable.ToString()),
                        FareRules = rules,
                        RuleTarrif = relatedFlight.RuleTarrif ?? string.Empty,

                        Class1 = ReturnCls(relatedFlight),
                        SRC1 = relatedFlight.DepartureStation,
                        DEST1 = relatedFlight.ArrivalStation,
                        DepartStationName1 = relatedFlight.DepartureStationName,
                        ArrivalStationName1 = relatedFlight.ArrivalStationName,
                        DepartureStationAirport1 = relatedFlight.DepartureStationAirport,
                        ArrivalStationAirport1 = relatedFlight.ArrivalStationAirport,
                        Via = checkVia(relatedFlight.Via),
                        ViaName = checkViaName(relatedFlight.ViaName),
                        TerminalSRC = checkTerminal(relatedFlight.DepartureTerminal),
                        TerminalDEST = checkTerminal(relatedFlight.ArrivalTerminal),
                        FlightName1 = relatedFlight.CarrierCode,
                        Cabin1 = relatedFlight.Cabin,
                        CarrierName1 = relatedFlight.CarrierName,
                        FlightNumber1 = relatedFlight.FlightNumber,
                        FlightDepDate1 = relatedFlight.DepartureDate,
                        FlightDepTime1 = relatedFlight.DepartureTime,
                        FlightArrDate1 = relatedFlight.ArrivalDate,
                        FlightArrTime1 = relatedFlight.ArrivalTime,
                        Layover1 = relatedFlight.JourneyTimeDesc,
                        Duration1 = relatedFlight.DurationDesc,
                    });
                }
                else if (relatedFlights.Count == 2)
                {
                    if (ReturnProviderCd(relatedFlights[0]) == ReturnProviderCd(relatedFlights[1]))
                    {
                        rules = GetFareRules(relatedFlights);
                        if (relatedFlights[0].DepartureDate.ToString() != relatedFlights[1].ArrivalDate.ToString())
                        {
                            arrivalCheck = DateHelper.DayDiff(relatedFlights[0].DepartureDate.ToString(), relatedFlights[1].ArrivalDate.ToString());
                        }
                        FlightOutBound.Add(new k_ShowFlightOutBound()
                        {

                            CompanyID = CompanyID,
                            AgentType = checkA_C,
                            F_Status = fStatus,
                            F_Remark = fRemark,
                            Curr = curr,
                            FlightRefid = relatedFlights[0].RefID.ToString(),


                            NoOFAdult = relatedFlights[0].Adt,
                            NoOFChild = relatedFlights[0].Chd,
                            NoOFInfant = relatedFlights[0].Inf,
                            //==============================================================================================================================
                            Adt_BASIC = GetConvert(relatedFlights[0].Adt_BASIC).ToString(),
                            Chd_BASIC = GetConvert(relatedFlights[0].Chd_BASIC).ToString(),
                            Inf_BASIC = GetConvert(relatedFlights[0].Inf_BASIC).ToString(),

                            AdultbaseFare = relatedFlights[0].Adt * GetConvert(relatedFlights[0].Adt_BASIC),
                            ChildbaseFare = relatedFlights[0].Chd * GetConvert(relatedFlights[0].Chd_BASIC),
                            InfantbaseFare = relatedFlights[0].Inf * GetConvert(relatedFlights[0].Inf_BASIC),

                            Adt_YQ = GetConvert(relatedFlights[0].Adt_YQ),
                            AdtTotalTax = GetConvert(relatedFlights[0].AdtTotalTax),
                            Chd_YQ = GetConvert(relatedFlights[0].Chd_YQ),
                            ChdTotalTax = GetConvert(relatedFlights[0].ChdTotalTax),
                            Inf_TAX = GetConvert(relatedFlights[0].Inf_TAX),

                            Adt_AcuTax = ReturnTAXADT(relatedFlights[0]),
                            Chd_AcuTax = ReturnTAXCHD(relatedFlights[0]),
                            Inf_AcuTax = ReturnTAXINF(relatedFlights[0]),

                            Yq = "Commission : " + String.Format("{0:N0}", GetConvert(relatedFlights[0].TotalCommission)),
                            PromoChek = promo,

                            TotalTax = GetTotalTax(relatedFlights[0]),
                            TotalBasic = GetTotalBasic(relatedFlights[0]),
                            TotalServiceFee = GetTotalServiceFee(relatedFlights[0]),
                            TotalMarkUp = GetTotalMarkUp(relatedFlights[0]),
                            TotalServiceTax = GetTotalServiceTax(relatedFlights[0]),
                            TaxandCharges = GetTaxandCharges(relatedFlights[0]),
                            TotalCommission = ReturnCommission(relatedFlights[0], CompanyID),
                            TotalTds = ReturnTDS(relatedFlights[0], CompanyID),

                            GrossF = ReturnGrossFare(relatedFlights[0]),
                            FinalFare = ReturnFinalFare(relatedFlights[0], CompanyID),
                            TotalAmount = ReturnTotalFareAmount(relatedFlights[0]),
                            TotalFare = ReturnTotalFareAmount(relatedFlights[0]),
                            //==============================================================================================================================

                            FlightName = relatedFlights[0].CarrierCode,
                            CarrierName = relatedFlights[0].CarrierName,
                            DepartStationName = relatedFlights[0].DepartureStationName,
                            ArrivalStationName = relatedFlights[1].ArrivalStationName,
                            DepartureStationAirport = relatedFlights[0].DepartureStationAirport,
                            ArrivalStationAirport = relatedFlights[1].ArrivalStationAirport,
                            Cabin = relatedFlights[0].Cabin,
                            FlightNumber = relatedFlights[0].FlightNumber,
                            FlightDepDate = relatedFlights[0].DepartureDate,
                            FlightDepTime = relatedFlights[0].DepartureTime,
                            FlightArrDate = relatedFlights[1].ArrivalDate,
                            FlightArrTime = relatedFlights[1].ArrivalTime,
                            CHECKINBaggage = ReturnCheckINBaggage(relatedFlights[0]),
                            CABINBaggage = ReturnCabinINBaggage(relatedFlights[0]),
                            RuleTarrif = relatedFlights[0].RuleTarrif ?? string.Empty,
                            SRC = relatedFlights[0].Origin,
                            DEST = relatedFlights[0].Destination,
                            logo = "/assets/img/airlogo_square/" + relatedFlights[0].CarrierCode + ".gif",
                            Duration = relatedFlights[0].DurationDesc,
                            connection = 2,
                            SMSACTIVES = 0,// SMSACTIVE,
                            ArrivalNextDayCheck = arrivalCheck,
                            PriceType = relatedFlights[0].PriceType,
                            RefundType = ReturnRefundType(relatedFlights[0]),
                            AvailableSeat = checkSeat(relatedFlights[0].SeatsAvailable.ToString()),
                            FareRules = rules,
                            Stop = "OneStop",

                            Class1 = ReturnCls(relatedFlights[0]),
                            SRC1 = relatedFlights[0].DepartureStation,
                            DEST1 = relatedFlights[0].ArrivalStation,
                            DepartStationName1 = relatedFlights[0].DepartureStationName,
                            ArrivalStationName1 = relatedFlights[0].ArrivalStationName,
                            DepartureStationAirport1 = relatedFlights[0].DepartureStationAirport,
                            ArrivalStationAirport1 = relatedFlights[0].ArrivalStationAirport,
                            Via = checkVia(relatedFlights[0].Via),
                            ViaName = checkViaName(relatedFlights[0].ViaName),
                            TerminalSRC = checkTerminal(relatedFlights[0].DepartureTerminal),
                            TerminalDEST = checkTerminal(relatedFlights[1].ArrivalTerminal),
                            FlightName1 = relatedFlights[0].CarrierCode,
                            Cabin1 = relatedFlights[0].Cabin,
                            CarrierName1 = relatedFlights[0].CarrierName,
                            FlightNumber1 = relatedFlights[0].FlightNumber,
                            FlightDepDate1 = relatedFlights[0].DepartureDate,
                            FlightDepTime1 = relatedFlights[0].DepartureTime,
                            FlightArrDate1 = relatedFlights[0].ArrivalDate,
                            FlightArrTime1 = relatedFlights[0].ArrivalTime,
                            Duration1 = relatedFlights[0].DurationDesc,

                            Class2 = ReturnCls(relatedFlights[1]),
                            SRC2 = relatedFlights[1].DepartureStation,
                            DEST2 = relatedFlights[1].ArrivalStation,
                            DepartStationName2 = relatedFlights[1].DepartureStationName,
                            ArrivalStationName2 = relatedFlights[1].ArrivalStationName,
                            DepartureStationAirport2 = relatedFlights[1].DepartureStationAirport,
                            ArrivalStationAirport2 = relatedFlights[1].ArrivalStationAirport,
                            Via1 = checkVia(relatedFlights[1].Via),
                            ViaName1 = checkViaName(relatedFlights[1].ViaName),
                            TerminalSRC1 = checkTerminal(relatedFlights[1].DepartureTerminal),
                            TerminalDEST1 = checkTerminal(relatedFlights[1].ArrivalTerminal),
                            logo1 = "/assets/img/airlogo_square/" + relatedFlights[1].CarrierCode + ".gif",
                            FlightName2 = relatedFlights[1].CarrierCode,
                            Cabin2 = relatedFlights[1].Cabin,
                            CarrierName2 = relatedFlights[1].CarrierName,
                            FlightNumber2 = relatedFlights[1].FlightNumber,
                            FlightDepDate2 = relatedFlights[1].DepartureDate,
                            FlightDepTime2 = relatedFlights[1].DepartureTime,
                            FlightArrDate2 = relatedFlights[1].ArrivalDate,
                            FlightArrTime2 = relatedFlights[1].ArrivalTime,
                            Duration2 = relatedFlights[1].DurationDesc,
                            Layover1 = relatedFlights[1].JourneyTimeDesc,
                        });
                    }
                }
                else if (relatedFlights.Count == 3)
                {

                    if ((ReturnProviderCd(relatedFlights[0]) == ReturnProviderCd(relatedFlights[1])) && (ReturnProviderCd(relatedFlights[0]) == ReturnProviderCd(relatedFlights[2])) && (ReturnProviderCd(relatedFlights[1]) == ReturnProviderCd(relatedFlights[2])))   //added 21Jan2024 by cpk
                    {
                        rules = GetFareRules(relatedFlights);
                        if (relatedFlights[0].DepartureDate.ToString() != relatedFlights[2].ArrivalDate.ToString())
                        {
                            arrivalCheck = DateHelper.DayDiff(relatedFlights[0].DepartureDate.ToString(), relatedFlights[2].ArrivalDate.ToString());
                        }

                        FlightOutBound.Add(new k_ShowFlightOutBound()
                        {
                            CompanyID = CompanyID,
                            AgentType = checkA_C,
                            F_Status = fStatus,
                            F_Remark = fRemark,
                            Curr = curr,
                            FlightRefid = relatedFlights[0].RefID.ToString(),

                            NoOFAdult = int.Parse(relatedFlights[0].Adt.ToString()),
                            NoOFChild = int.Parse(relatedFlights[0].Chd.ToString()),
                            NoOFInfant = int.Parse(relatedFlights[0].Inf.ToString()),

                            //==============================================================================================================================
                            Adt_BASIC = GetConvert(relatedFlights[0].Adt_BASIC.ToString()),
                            Chd_BASIC = GetConvert(relatedFlights[0].Chd_BASIC.ToString()),
                            Inf_BASIC = GetConvert(relatedFlights[0].Inf_BASIC.ToString()),

                            AdultbaseFare = int.Parse(relatedFlights[0].Adt.ToString()) * GetConvert(int.Parse(relatedFlights[0].Adt_BASIC.ToString())),
                            ChildbaseFare = int.Parse(relatedFlights[0].Chd.ToString()) * GetConvert(int.Parse(relatedFlights[0].Chd_BASIC.ToString())),
                            InfantbaseFare = int.Parse(relatedFlights[0].Inf.ToString()) * GetConvert(int.Parse(relatedFlights[0].Inf_BASIC.ToString())),

                            Adt_YQ = GetConvert(relatedFlights[0].Adt_YQ),
                            AdtTotalTax = GetConvert(relatedFlights[0].AdtTotalTax),
                            Chd_YQ = GetConvert(relatedFlights[0].Chd_YQ),
                            ChdTotalTax = GetConvert(relatedFlights[0].ChdTotalTax),
                            Inf_TAX = GetConvert(relatedFlights[0].Inf_TAX),

                            Adt_AcuTax = ReturnTAXADT(relatedFlights[0]),
                            Chd_AcuTax = ReturnTAXCHD(relatedFlights[0]),
                            Inf_AcuTax = ReturnTAXINF(relatedFlights[0]),

                            Yq = "Commission : " + String.Format("{0:N0}", GetConvert(relatedFlights[0].TotalCommission)),
                            PromoChek = promo,

                            TotalTax = GetTotalTax(relatedFlights[0]),
                            TotalBasic = GetTotalBasic(relatedFlights[0]),
                            TotalServiceFee = GetTotalServiceFee(relatedFlights[0]),
                            TotalMarkUp = GetTotalMarkUp(relatedFlights[0]),
                            TotalServiceTax = GetTotalServiceTax(relatedFlights[0]),
                            TaxandCharges = GetTaxandCharges(relatedFlights[0]),
                            TotalCommission = ReturnCommission(relatedFlights[0], CompanyID),
                            TotalTds = ReturnTDS(relatedFlights[0], CompanyID),

                            GrossF = ReturnGrossFare(relatedFlights[0]),
                            FinalFare = ReturnFinalFare(relatedFlights[0], CompanyID),
                            TotalAmount = ReturnTotalFareAmount(relatedFlights[0]),
                            TotalFare = ReturnTotalFareAmount(relatedFlights[0]),
                            //==============================================================================================================================

                            RuleTarrif = relatedFlights[0].RuleTarrif ?? string.Empty,
                            FlightName = relatedFlights[0].CarrierCode,
                            Cabin = relatedFlights[0].Cabin,
                            CarrierName = relatedFlights[0].CarrierName,
                            DepartStationName = relatedFlights[0].DepartureStationName,
                            ArrivalStationName = relatedFlights[2].ArrivalStationName,
                            DepartureStationAirport = relatedFlights[0].DepartureStationAirport,
                            ArrivalStationAirport = relatedFlights[2].ArrivalStationAirport,
                            FlightNumber = relatedFlights[0].FlightNumber,
                            FlightDepDate = relatedFlights[0].DepartureDate,
                            FlightDepTime = relatedFlights[0].DepartureTime,
                            FlightArrDate = relatedFlights[2].ArrivalDate,
                            FlightArrTime = relatedFlights[2].ArrivalTime,
                            CHECKINBaggage = ReturnCheckINBaggage(relatedFlights[0]),
                            CABINBaggage = ReturnCabinINBaggage(relatedFlights[0]),
                            SRC = relatedFlights[0].Origin,
                            DEST = relatedFlights[0].Destination,
                            logo = "/assets/img/airlogo_square/" + relatedFlights[0].CarrierCode + ".gif",
                            Duration = relatedFlights[0].DurationDesc,
                            Stop = "TwoStop",
                            connection = 3,
                            ArrivalNextDayCheck = arrivalCheck,
                            PriceType = relatedFlights[0].PriceType,
                            RefundType = ReturnRefundType(relatedFlights[0]),
                            AvailableSeat = checkSeat(relatedFlights[0].SeatsAvailable.ToString()),
                            FareRules = rules,
                            FareRule = relatedFlights[0].FareRule,
                            SMSACTIVES = 0,// SMSACTIVE,

                            Class1 = ReturnCls(relatedFlights[0]),
                            SRC1 = relatedFlights[0].DepartureStation,
                            DEST1 = relatedFlights[0].ArrivalStation,
                            DepartStationName1 = relatedFlights[0].DepartureStationName,
                            ArrivalStationName1 = relatedFlights[0].ArrivalStationName,
                            DepartureStationAirport1 = relatedFlights[0].DepartureStationAirport,
                            ArrivalStationAirport1 = relatedFlights[0].ArrivalStationAirport,
                            Via = checkVia(relatedFlights[0].Via),
                            ViaName = checkViaName(relatedFlights[0].ViaName),
                            TerminalSRC = checkTerminal(relatedFlights[0].DepartureTerminal),
                            TerminalDEST = checkTerminal(relatedFlights[2].ArrivalTerminal),
                            FlightName1 = relatedFlights[0].CarrierCode,
                            Cabin1 = relatedFlights[0].Cabin,
                            CarrierName1 = relatedFlights[0].CarrierName,
                            FlightNumber1 = relatedFlights[0].FlightNumber,
                            FlightDepDate1 = relatedFlights[0].DepartureDate,
                            FlightDepTime1 = relatedFlights[0].DepartureTime,
                            FlightArrDate1 = relatedFlights[0].ArrivalDate,
                            FlightArrTime1 = relatedFlights[0].ArrivalTime,
                            Duration1 = relatedFlights[0].DurationDesc,

                            Class2 = ReturnCls(relatedFlights[1]),
                            SRC2 = relatedFlights[1].DepartureStation,
                            DEST2 = relatedFlights[1].ArrivalStation,
                            DepartStationName2 = relatedFlights[1].DepartureStationName,
                            ArrivalStationName2 = relatedFlights[1].ArrivalStationName,
                            DepartureStationAirport2 = relatedFlights[1].DepartureStationAirport,
                            ArrivalStationAirport2 = relatedFlights[1].ArrivalStationAirport,
                            Via1 = checkVia(relatedFlights[1].Via),
                            ViaName1 = checkViaName(relatedFlights[1].ViaName),
                            TerminalSRC1 = checkTerminal(relatedFlights[1].DepartureTerminal),
                            TerminalDEST1 = checkTerminal(relatedFlights[1].ArrivalTerminal),
                            logo1 = "/assets/img/airlogo_square/" + relatedFlights[1].CarrierCode + ".gif",
                            FlightName2 = relatedFlights[1].CarrierCode,
                            CarrierName2 = relatedFlights[1].CarrierName,
                            Cabin2 = relatedFlights[1].Cabin,
                            FlightNumber2 = relatedFlights[1].FlightNumber,
                            FlightDepDate2 = relatedFlights[1].DepartureDate,
                            FlightArrDate2 = relatedFlights[1].ArrivalDate,
                            FlightArrTime2 = relatedFlights[1].ArrivalTime,
                            Duration2 = relatedFlights[1].DurationDesc,
                            Layover1 = relatedFlights[1].JourneyTimeDesc,

                            Class3 = ReturnCls(relatedFlights[2]),
                            SRC3 = relatedFlights[2].DepartureStation,
                            DEST3 = relatedFlights[2].ArrivalStation,
                            DepartStationName3 = relatedFlights[2].DepartureStationName,
                            ArrivalStationName3 = relatedFlights[2].ArrivalStationName,
                            DepartureStationAirport3 = relatedFlights[2].DepartureStationAirport,
                            ArrivalStationAirport3 = relatedFlights[2].ArrivalStationAirport,
                            Via2 = checkVia(relatedFlights[2].Via),
                            ViaName2 = checkViaName(relatedFlights[2].ViaName),
                            TerminalSRC2 = checkTerminal(relatedFlights[2].DepartureTerminal),
                            TerminalDEST2 = checkTerminal(relatedFlights[2].ArrivalTerminal),
                            logo2 = "/assets/img/airlogo_square/" + relatedFlights[2].CarrierCode + ".gif",
                            FlightName3 = relatedFlights[2].CarrierCode,
                            Cabin3 = relatedFlights[2].Cabin,
                            CarrierName3 = relatedFlights[2].CarrierName,
                            FlightNumber3 = relatedFlights[2].FlightNumber,
                            FlightDepDate3 = relatedFlights[2].DepartureDate,
                            FlightDepTime3 = relatedFlights[2].DepartureTime,
                            FlightArrDate3 = relatedFlights[2].ArrivalDate,
                            FlightArrTime3 = relatedFlights[2].ArrivalTime,
                            Duration3 = relatedFlights[2].DurationDesc,
                            Layover2 = relatedFlights[2].JourneyTimeDesc,
                        });
                    }
                    else if (relatedFlights.Count == 4)
                    {

                    }
                }

            }
            return FlightOutBound;

        }

        public static async Task<List<k_ShowFlightOutBound>> SeletRound(string refid, string refidR, string SearchType, string CompanyID, ClaimsPrincipal user, IHandlesQueryAsync<GetAirFareQuery, string> getAirFareHandler, IHandlesQueryAsync<GetAirFareRulesQuery, string> getAirFareRulesHandler, IHandlesQueryAsync<string, CompanyRegisterCorporateUserDetails> getCompanyRegisterCorporateUserDetailsQueryHandler, IHandlesQueryAsync<string, CompanyRegisterCorporateUserLimitDetails> getCompanyRegisterCorporateUserLimitQueryHandler)
        {
            List<k_ShowFlightOutBound> FlightOutBound = new List<k_ShowFlightOutBound>();

            //=============================================================================================================
            
            string fRemark = string.Empty;
            int Limit = 0;

            var userInfo = await SelectedFlightUserInfo(user, refid, getCompanyRegisterCorporateUserDetailsQueryHandler, getCompanyRegisterCorporateUserLimitQueryHandler);
            var fStatus = userInfo.ValidTimeUser;
           
            if (fStatus)
            {
               var getfareResponse = await ShowFlightFareHelper.GetFare(CompanyID, refid, "O", getAirFareHandler, getAirFareRulesHandler);
                fStatus = getfareResponse.FareStatus;
                if (!fStatus)
                {
                    fRemark = "Onward-" + getfareResponse.FareRemark;
                }

                if (fStatus)
                {
                    getfareResponse = await ShowFlightFareHelper.GetFare(CompanyID, refidR, "I", getAirFareHandler, getAirFareRulesHandler);
                    fStatus = getfareResponse.FareStatus;
                    if (!fStatus)
                    {
                        fRemark = "Inward-" + getfareResponse.FareRemark;
                    }
                }
            }
            //=============================================================================================================


            var relatedFlightsOutbound = SelectionFlight(refid, "OUTBOUND");
            var relatedFlightsdtInbound =SelectionFlight(refidR, "INBOUND");


            HttpContextHelper.Current.Session.SetString("SelectedFltOut", refid);
            HttpContextHelper.Current.Session.SetString("SelectedFltIn", refidR);

            string msg = string.Empty;
            if (relatedFlightsOutbound.Count > 0 && relatedFlightsdtInbound.Count > 0)
            {
                string curr = "INR";
                // Check session or alternative storage for currency
                if (HttpContextHelper.Current?.Session.GetString("Curr") != null)
                {
                    curr = HttpContextHelper.Current.Session.GetString("Curr");
                }

                string rules = "Fare Rules are just a guideline for your convenience and is subject to changes by the Airline from time to time.For Any query related to Fare Rules please contact to our callcenter.";
                string rules2 = string.Empty;

                int arrvchack = 0;
                int promo = 0;
                int arrivalCheck = 0;
                int FareUpdateMsgCheks = 0;

                string InboundOutboundFare = string.Empty;
                 relatedFlightsOutbound = relatedFlightsOutbound.OrderBy(x => x.TotalFare).ToList();
                int ChekA_C = 0;

                #region

                if (relatedFlightsOutbound.Count == 1)
                {
                    var relatedFlight = relatedFlightsOutbound.FirstOrDefault();
                    rules = GetFareRules(relatedFlightsOutbound);
                    string departureDate = relatedFlight.DepartureDate.ToString(); // Assuming DepartureDate is a property of k_ShowFlightOutBound
                    string arrivalDate = relatedFlight.ArrivalDate.ToString();
                    if (departureDate != arrivalDate)
                    {
                        arrivalCheck = DateHelper.DayDiff(departureDate, arrivalDate);
                    }

                    InboundOutboundFare = ReturnGrossFare(relatedFlight).ToString();
                    FlightOutBound.Add(new k_ShowFlightOutBound()
                    {
                        F_Status = fStatus,
                        F_Remark = fRemark,
                        CompanyID = CompanyID,
                        AgentType = ChekA_C,
                        FlightRefid = relatedFlight.RefID.ToString(),
                        Curr = curr,
                        FareUpdateMsg = msg,
                        FareUpdateMsgChek = FareUpdateMsgCheks,
                        InboundOutbound = InboundOutboundFare,
                        NoOFAdult = relatedFlight.Adt,
                        NoOFChild = relatedFlight.Chd,
                        NoOFInfant = relatedFlight.Inf,

                        Adt_BASIC = GetConvert(relatedFlight.Adt_BASIC.ToString()),
                        Chd_BASIC = GetConvert(relatedFlight.Chd_BASIC.ToString()),
                        Inf_BASIC = GetConvert(relatedFlight.Inf_BASIC.ToString()),

                        AdultbaseFare = relatedFlight.Adt * GetConvert(relatedFlight.Adt_BASIC),
                        ChildbaseFare = relatedFlight.Chd * GetConvert(relatedFlight.Chd_BASIC),
                        InfantbaseFare = relatedFlight.Inf * GetConvert(relatedFlight.Inf_BASIC),

                        Adt_YQ = GetConvert(relatedFlight.Adt_YQ),
                        AdtTotalTax = GetConvert(relatedFlight.AdtTotalTax),
                        Chd_YQ = GetConvert(relatedFlight.Chd_YQ),
                        ChdTotalTax = GetConvert(relatedFlight.ChdTotalTax),
                        Inf_TAX = GetConvert(relatedFlight.Inf_TAX),

                        Adt_AcuTax = ReturnTAXADT(relatedFlight).ToString(),
                        Chd_AcuTax = ReturnTAXCHD(relatedFlight).ToString(),
                        Inf_AcuTax = ReturnTAXINF(relatedFlight).ToString(),

                        Yq = "Commission : " + String.Format("{0:N0}", GetConvert(relatedFlight.TotalCommission)),
                        PromoChek = promo,

                        TotalTax = GetTotalTax(relatedFlight),
                        TotalBasic = GetTotalBasic(relatedFlight),
                        TotalServiceFee = GetTotalServiceFee(relatedFlight),
                        TotalMarkUp = GetTotalMarkUp(relatedFlight),
                        TotalServiceTax = GetTotalServiceTax(relatedFlight),
                        TaxandCharges = GetTaxandCharges(relatedFlight),
                        TotalCommission = ReturnCommission(relatedFlight, CompanyID),
                        TotalTds = ReturnTDS(relatedFlight, CompanyID),

                        GrossF = ReturnGrossFare(relatedFlight).ToString(),
                        FinalFare = ReturnFinalFare(relatedFlight, CompanyID),
                        TotalAmount = ReturnTotalFareAmount(relatedFlight),
                        TotalFare = ReturnTotalFareAmount(relatedFlight),
                        //==============================================================================================================================

                        FlightName = relatedFlight.CarrierCode,
                        DepartStationName = relatedFlight.DepartureStationName,
                        ArrivalStationName = relatedFlight.ArrivalStationName,
                        DepartureStationAirport = relatedFlight.DepartureStationAirport,
                        ArrivalStationAirport = relatedFlight.ArrivalStationAirport,
                        Cabin = relatedFlight.Cabin,
                        CarrierName = relatedFlight.CarrierName,
                        FlightNumber = relatedFlight.FlightNumber,
                        FlightDepDate = relatedFlight.DepartureDate,
                        FlightDepTime = relatedFlight.DepartureTime,
                        FlightArrDate = relatedFlight.ArrivalDate,
                        FlightArrTime = relatedFlight.ArrivalTime,
                        RuleTarrif = relatedFlight.RuleTarrif ?? string.Empty,
                        CHECKINBaggage = ReturnCheckINBaggage(relatedFlight).ToString(),
                        CABINBaggage = ReturnCabinINBaggage(relatedFlight).ToString(),
                        SRC = relatedFlight.Origin,
                        DEST = relatedFlight.Destination,
                        PrimarySRC = relatedFlight.Origin,
                        PrimaryDEST = relatedFlight.Destination,
                        Stop = "NonStop",
                        logo = "/assets/img/airlogo_square/" + relatedFlight.CarrierCode + ".gif",
                        Duration = relatedFlight.DurationDesc,
                        connection = 1,
                        SMSACTIVES = 0,
                        ArrivalNextDayCheck = arrivalCheck,
                        PriceType = relatedFlight.PriceType,
                        RefundType = ReturnRefundType(relatedFlight).ToString(),
                        AvailableSeat = checkSeat(relatedFlight.SeatsAvailable.ToString()),
                        FareRules = rules,

                        Class1 = ReturnCls(relatedFlight),
                        SRC1 = relatedFlight.DepartureStation,
                        DEST1 = relatedFlight.ArrivalStation,
                        DepartStationName1 = relatedFlight.DepartureStationName,
                        ArrivalStationName1 = relatedFlight.ArrivalStationName,
                        DepartureStationAirport1 = relatedFlight.DepartureStationAirport,
                        ArrivalStationAirport1 = relatedFlight.ArrivalStationAirport,
                        Via = checkVia(relatedFlight.Via),
                        ViaName = checkViaName(relatedFlight.ViaName),
                        TerminalSRC = checkTerminal(relatedFlight.DepartureTerminal),
                        TerminalDEST = checkTerminal(relatedFlight.ArrivalTerminal),
                        FlightName1 = relatedFlight.CarrierCode,
                        Cabin1 = relatedFlight.Cabin,
                        CarrierName1 = relatedFlight.CarrierName,
                        FlightNumber1 = relatedFlight.FlightNumber,
                        FlightDepDate1 = relatedFlight.DepartureDate,
                        FlightDepTime1 = relatedFlight.DepartureTime,
                        FlightArrDate1 = relatedFlight.ArrivalDate,
                        FlightArrTime1 = relatedFlight.ArrivalTime,
                        Layover1 = relatedFlight.JourneyTimeDesc,
                        Duration1 = relatedFlight.DurationDesc,
                    });
                }
                else if (relatedFlightsOutbound.Count == 2)
                {
                    
                    rules = GetFareRules(relatedFlightsOutbound);
                    if (relatedFlightsOutbound[0].DepartureDate.ToString() != relatedFlightsOutbound[1].ArrivalDate.ToString())
                    {
                        arrivalCheck = DateHelper.DayDiff(relatedFlightsOutbound[0].DepartureDate.ToString(), relatedFlightsOutbound[1].ArrivalDate.ToString());
                    }

                    InboundOutboundFare = ReturnGrossFare(relatedFlightsOutbound[0]).ToString();
                    FlightOutBound.Add(new k_ShowFlightOutBound()
                    {
                        F_Status = fStatus,
                        F_Remark = fRemark,
                        CompanyID = CompanyID,
                        AgentType = ChekA_C,
                        FlightRefid = relatedFlightsOutbound[0].RefID.ToString(),
                        Curr = curr,
                        FareUpdateMsg = msg,
                        FareUpdateMsgChek = FareUpdateMsgCheks,
                        InboundOutbound = InboundOutboundFare,

                        NoOFAdult = relatedFlightsOutbound[0].Adt,
                        NoOFChild = relatedFlightsOutbound[0].Chd,
                        NoOFInfant = relatedFlightsOutbound[0].Inf,
                        //==============================================================================================================================
                        Adt_BASIC = GetConvert(relatedFlightsOutbound[0].Adt_BASIC).ToString(),
                        Chd_BASIC = GetConvert(relatedFlightsOutbound[0].Chd_BASIC).ToString(),
                        Inf_BASIC = GetConvert(relatedFlightsOutbound[0].Inf_BASIC).ToString(),

                        AdultbaseFare = relatedFlightsOutbound[0].Adt * GetConvert(relatedFlightsOutbound[0].Adt_BASIC),
                        ChildbaseFare = relatedFlightsOutbound[0].Chd * GetConvert(relatedFlightsOutbound[0].Chd_BASIC),
                        InfantbaseFare = relatedFlightsOutbound[0].Inf * GetConvert(relatedFlightsOutbound[0].Inf_BASIC),


                        Adt_YQ = GetConvert(relatedFlightsOutbound[0].Adt_YQ),
                        AdtTotalTax = GetConvert(relatedFlightsOutbound[0].AdtTotalTax),
                        Chd_YQ = GetConvert(relatedFlightsOutbound[0].Chd_YQ),
                        ChdTotalTax = GetConvert(relatedFlightsOutbound[0].ChdTotalTax),
                        Inf_TAX = GetConvert(relatedFlightsOutbound[0].Inf_TAX),

                        Adt_AcuTax = ReturnTAXADT(relatedFlightsOutbound[0]),
                        Chd_AcuTax = ReturnTAXCHD(relatedFlightsOutbound[0]),
                        Inf_AcuTax = ReturnTAXINF(relatedFlightsOutbound[0]),

                        Yq = "Commission : " + String.Format("{0:N0}", GetConvert(relatedFlightsOutbound[0].TotalCommission)),
                        PromoChek = promo,

                        TotalTax = GetTotalTax(relatedFlightsOutbound[0]),
                        TotalBasic = GetTotalBasic(relatedFlightsOutbound[0]),
                        TotalServiceFee = GetTotalServiceFee(relatedFlightsOutbound[0]),
                        TotalMarkUp = GetTotalMarkUp(relatedFlightsOutbound[0]),
                        TotalServiceTax = GetTotalServiceTax(relatedFlightsOutbound[0]),
                        TaxandCharges = GetTaxandCharges(relatedFlightsOutbound[0]),
                        TotalCommission = ReturnCommission(relatedFlightsOutbound[0], CompanyID),
                        TotalTds = ReturnTDS(relatedFlightsOutbound[0], CompanyID),

                        GrossF = ReturnGrossFare(relatedFlightsOutbound[0]),
                        FinalFare = ReturnFinalFare(relatedFlightsOutbound[0], CompanyID),
                        TotalAmount = ReturnTotalFareAmount(relatedFlightsOutbound[0]),
                        TotalFare = ReturnTotalFareAmount(relatedFlightsOutbound[0]),
                        //==============================================================================================================================

                        FlightName = relatedFlightsOutbound[0].CarrierCode,
                        Cabin = relatedFlightsOutbound[0].Cabin,
                        DepartStationName = relatedFlightsOutbound[0].DepartureStationName,
                        ArrivalStationName = relatedFlightsOutbound[1].ArrivalStationName,
                        DepartureStationAirport = relatedFlightsOutbound[0].DepartureStationAirport,
                        ArrivalStationAirport = relatedFlightsOutbound[1].ArrivalStationAirport,
                        RuleTarrif = relatedFlightsOutbound[0].RuleTarrif ?? string.Empty,
                        CarrierName = relatedFlightsOutbound[0].CarrierName,
                        FlightNumber = relatedFlightsOutbound[0].FlightNumber,
                        FlightDepDate = relatedFlightsOutbound[0].DepartureDate,
                        FlightDepTime = relatedFlightsOutbound[0].DepartureTime,
                        FlightArrDate = relatedFlightsOutbound[1].ArrivalDate,
                        FlightArrTime = relatedFlightsOutbound[1].ArrivalTime,
                        CHECKINBaggage = ReturnCheckINBaggage(relatedFlightsOutbound[0]),
                        CABINBaggage = ReturnCabinINBaggage(relatedFlightsOutbound[0]),
                        SRC = relatedFlightsOutbound[0].Origin,
                        DEST = relatedFlightsOutbound[0].Destination,
                        logo = "/assets/img/airlogo_square/" + relatedFlightsOutbound[0].CarrierCode + ".gif",
                        Duration = relatedFlightsOutbound[0].DurationDesc,
                        connection = 2,
                        SMSACTIVES = 0,// SMSACTIVE,
                        Class1 = ReturnCls(relatedFlightsOutbound[0]),
                        Class2 = ReturnCls(relatedFlightsOutbound[1]),
                        ArrivalNextDayCheck = arrvchack,
                        PriceType = relatedFlightsOutbound[0].PriceType,
                        RefundType = ReturnRefundType(relatedFlightsOutbound[0]),
                        AvailableSeat = checkSeat(relatedFlightsOutbound[0].SeatsAvailable.ToString()),
                        FareRules = rules,

                        SRC1 = relatedFlightsOutbound[0].DepartureStation,
                        DEST1 = relatedFlightsOutbound[0].ArrivalStation,
                        DepartStationName1 = relatedFlightsOutbound[0].DepartureStationName,
                        ArrivalStationName1 = relatedFlightsOutbound[0].ArrivalStationName,
                        DepartureStationAirport1 = relatedFlightsOutbound[0].DepartureStationAirport,
                        ArrivalStationAirport1 = relatedFlightsOutbound[0].ArrivalStationAirport,
                        Stop = "OneStop",
                        Via = checkVia(relatedFlightsOutbound[0].Via),
                        ViaName = checkViaName(relatedFlightsOutbound[0].ViaName),
                        TerminalSRC = checkTerminal(relatedFlightsOutbound[0].DepartureTerminal),
                        TerminalDEST = checkTerminal(relatedFlightsOutbound[1].ArrivalTerminal),
                        FlightName1 = relatedFlightsOutbound[0].CarrierCode,
                        Cabin1 = relatedFlightsOutbound[0].Cabin,
                        CarrierName1 = relatedFlightsOutbound[0].CarrierName,
                        FlightNumber1 = relatedFlightsOutbound[0].FlightNumber,
                        FlightDepDate1 = relatedFlightsOutbound[0].DepartureDate,
                        FlightDepTime1 = relatedFlightsOutbound[0].DepartureTime,
                        FlightArrDate1 = relatedFlightsOutbound[0].ArrivalDate,
                        FlightArrTime1 = relatedFlightsOutbound[0].ArrivalTime,
                        Duration1 = relatedFlightsOutbound[0].DurationDesc,

                        SRC2 = relatedFlightsOutbound[1].DepartureStation,
                        DEST2 = relatedFlightsOutbound[1].ArrivalStation,
                        DepartStationName2 = relatedFlightsOutbound[1].DepartureStationName,
                        ArrivalStationName2 = relatedFlightsOutbound[1].ArrivalStationName,
                        DepartureStationAirport2 = relatedFlightsOutbound[1].DepartureStationAirport,
                        ArrivalStationAirport2 = relatedFlightsOutbound[1].ArrivalStationAirport,
                        Via1 = checkVia(relatedFlightsOutbound[1].Via),
                        ViaName1 = checkViaName(relatedFlightsOutbound[1].ViaName),
                        TerminalSRC1 = checkTerminal(relatedFlightsOutbound[1].DepartureTerminal),
                        TerminalDEST1 = checkTerminal(relatedFlightsOutbound[1].ArrivalTerminal),
                        logo1 = "/assets/img/airlogo_square/" + relatedFlightsOutbound[1].CarrierCode + ".gif",
                        FlightName2 = relatedFlightsOutbound[1].CarrierCode,
                        Cabin2 = relatedFlightsOutbound[1].Cabin,
                        CarrierName2 = relatedFlightsOutbound[1].CarrierName,
                        FlightNumber2 = relatedFlightsOutbound[1].FlightNumber,
                        FlightDepDate2 = relatedFlightsOutbound[1].DepartureDate,
                        FlightDepTime2 = relatedFlightsOutbound[1].DepartureTime,
                        FlightArrDate2 = relatedFlightsOutbound[1].ArrivalDate,
                        FlightArrTime2 = relatedFlightsOutbound[1].ArrivalTime,
                        Duration2 = relatedFlightsOutbound[1].DurationDesc,
                        Layover1 = relatedFlightsOutbound[1].JourneyTimeDesc,
                    });
                }
                else if (relatedFlightsOutbound.Count == 3)
                {

                    rules = GetFareRules(relatedFlightsOutbound);
                    if (relatedFlightsOutbound[0].DepartureDate.ToString() != relatedFlightsOutbound[2].ArrivalDate.ToString())
                    {
                        arrivalCheck = DateHelper.DayDiff(relatedFlightsOutbound[0].DepartureDate.ToString(), relatedFlightsOutbound[2].ArrivalDate.ToString());
                    }

                    InboundOutboundFare = ReturnGrossFare(relatedFlightsOutbound[0]).ToString();
                    FlightOutBound.Add(new k_ShowFlightOutBound()
                    {
                        F_Status = fStatus,
                        F_Remark = fRemark,
                        CompanyID = CompanyID,
                        AgentType = ChekA_C,
                        FlightRefid = relatedFlightsOutbound[0].RefID.ToString(),
                        Curr = curr,
                        FareUpdateMsg = msg,
                        FareUpdateMsgChek = FareUpdateMsgCheks,
                        InboundOutbound = InboundOutboundFare,

                        NoOFAdult = relatedFlightsOutbound[0].Adt,
                        NoOFChild = relatedFlightsOutbound[0].Chd,
                        NoOFInfant = relatedFlightsOutbound[0].Inf,
                        //==============================================================================================================================
                        Adt_BASIC = GetConvert(relatedFlightsOutbound[0].Adt_BASIC).ToString(),
                        Chd_BASIC = GetConvert(relatedFlightsOutbound[0].Chd_BASIC).ToString(),
                        Inf_BASIC = GetConvert(relatedFlightsOutbound[0].Inf_BASIC).ToString(),

                        AdultbaseFare = relatedFlightsOutbound[0].Adt * GetConvert(relatedFlightsOutbound[0].Adt_BASIC),
                        ChildbaseFare = relatedFlightsOutbound[0].Chd * GetConvert(relatedFlightsOutbound[0].Chd_BASIC),
                        InfantbaseFare = relatedFlightsOutbound[0].Inf * GetConvert(relatedFlightsOutbound[0].Inf_BASIC),


                        Adt_YQ = GetConvert(relatedFlightsOutbound[0].Adt_YQ),
                        AdtTotalTax = GetConvert(relatedFlightsOutbound[0].AdtTotalTax),
                        Chd_YQ = GetConvert(relatedFlightsOutbound[0].Chd_YQ),
                        ChdTotalTax = GetConvert(relatedFlightsOutbound[0].ChdTotalTax),
                        Inf_TAX = GetConvert(relatedFlightsOutbound[0].Inf_TAX),

                        Adt_AcuTax = ReturnTAXADT(relatedFlightsOutbound[0]),
                        Chd_AcuTax = ReturnTAXCHD(relatedFlightsOutbound[0]),
                        Inf_AcuTax = ReturnTAXINF(relatedFlightsOutbound[0]),

                        Yq = "Commission : " + String.Format("{0:N0}", GetConvert(relatedFlightsOutbound[0].TotalCommission)),
                        PromoChek = promo,

                        TotalTax = GetTotalTax(relatedFlightsOutbound[0]),
                        TotalBasic = GetTotalBasic(relatedFlightsOutbound[0]),
                        TotalServiceFee = GetTotalServiceFee(relatedFlightsOutbound[0]),
                        TotalMarkUp = GetTotalMarkUp(relatedFlightsOutbound[0]),
                        TotalServiceTax = GetTotalServiceTax(relatedFlightsOutbound[0]),
                        TaxandCharges = GetTaxandCharges(relatedFlightsOutbound[0]),
                        TotalCommission = ReturnCommission(relatedFlightsOutbound[0], CompanyID),
                        TotalTds = ReturnTDS(relatedFlightsOutbound[0], CompanyID),

                        GrossF = ReturnGrossFare(relatedFlightsOutbound[0]),
                        FinalFare = ReturnFinalFare(relatedFlightsOutbound[0], CompanyID),
                        TotalAmount = ReturnTotalFareAmount(relatedFlightsOutbound[0]),
                        TotalFare = ReturnTotalFareAmount(relatedFlightsOutbound[0]),
                        //==============================================================================================================================

                        FlightName = relatedFlightsOutbound[0].CarrierCode,
                        Cabin = relatedFlightsOutbound[0].Cabin,
                        DepartStationName = relatedFlightsOutbound[0].DepartureStationName,
                        ArrivalStationName = relatedFlightsOutbound[2].ArrivalStationName,
                        RuleTarrif = relatedFlightsOutbound[0].RuleTarrif ?? string.Empty,
                        DepartureStationAirport = relatedFlightsOutbound[0].DepartureStationAirport,
                        ArrivalStationAirport = relatedFlightsOutbound[2].ArrivalStationAirport,

                        CarrierName = relatedFlightsOutbound[0].CarrierName,
                        FlightNumber = relatedFlightsOutbound[0].FlightNumber,
                        FlightDepDate = relatedFlightsOutbound[0].DepartureDate,
                        FlightDepTime = relatedFlightsOutbound[0].DepartureTime,
                        FlightArrDate = relatedFlightsOutbound[2].ArrivalDate,
                        FlightArrTime = relatedFlightsOutbound[2].ArrivalTime,
                        SRC = relatedFlightsOutbound[0].Origin,
                        DEST = relatedFlightsOutbound[0].Destination,
                        logo = "/assets/img/airlogo_square/" + relatedFlightsOutbound[0].CarrierCode + ".gif",
                        Duration = relatedFlightsOutbound[0].DurationDesc,
                        Stop = "TwoStop",
                        CHECKINBaggage = ReturnCheckINBaggage(relatedFlightsOutbound[0]),
                        CABINBaggage = ReturnCabinINBaggage(relatedFlightsOutbound[0]),
                        connection = 3,
                        ArrivalNextDayCheck = arrvchack,
                        PriceType = relatedFlightsOutbound[0].PriceType,
                        RefundType = ReturnRefundType(relatedFlightsOutbound[0]),
                        AvailableSeat = checkSeat(relatedFlightsOutbound[0].SeatsAvailable.ToString()),
                        FareRules = rules,
                        SMSACTIVES = 0,

                        Class1 = ReturnCls(relatedFlightsOutbound[0]),
                        SRC1 = relatedFlightsOutbound[0].DepartureStation,
                        DEST1 = relatedFlightsOutbound[0].ArrivalStation,
                        DepartStationName1 = relatedFlightsOutbound[0].DepartureStationName,
                        ArrivalStationName1 = relatedFlightsOutbound[0].ArrivalStationName,
                        DepartureStationAirport1 = relatedFlightsOutbound[0].DepartureStationAirport,
                        ArrivalStationAirport1 = relatedFlightsOutbound[0].ArrivalStationAirport,
                        Via = checkVia(relatedFlightsOutbound[0].Via),
                        ViaName = checkViaName(relatedFlightsOutbound[0].ViaName),
                        TerminalSRC = checkTerminal(relatedFlightsOutbound[0].DepartureTerminal),
                        TerminalDEST = checkTerminal(relatedFlightsOutbound[1].ArrivalTerminal),
                        FlightName1 = relatedFlightsOutbound[0].CarrierCode,
                        Cabin1 = relatedFlightsOutbound[0].Cabin,
                        CarrierName1 = relatedFlightsOutbound[0].CarrierName,
                        FlightNumber1 = relatedFlightsOutbound[0].FlightNumber,
                        FlightDepDate1 = relatedFlightsOutbound[0].DepartureDate,
                        FlightDepTime1 = relatedFlightsOutbound[0].DepartureTime,
                        FlightArrDate1 = relatedFlightsOutbound[0].ArrivalDate,
                        FlightArrTime1 = relatedFlightsOutbound[0].ArrivalTime,
                        FareRule = relatedFlightsOutbound[0].FareRule,
                        Duration1 = relatedFlightsOutbound[0].DurationDesc,

                        Class2 = ReturnCls(relatedFlightsOutbound[1]),
                        SRC2 = relatedFlightsOutbound[1].DepartureStation,
                        DEST2 = relatedFlightsOutbound[1].ArrivalStation,
                        DepartStationName2 = relatedFlightsOutbound[1].DepartureStationName,
                        ArrivalStationName2 = relatedFlightsOutbound[1].ArrivalStationName,
                        DepartureStationAirport2 = relatedFlightsOutbound[1].DepartureStationAirport,
                        ArrivalStationAirport2 = relatedFlightsOutbound[1].ArrivalStationAirport,
                        Via1 = checkVia(relatedFlightsOutbound[1].Via),
                        ViaName1 = checkViaName(relatedFlightsOutbound[1].ViaName),
                        TerminalSRC1 = checkTerminal(relatedFlightsOutbound[1].DepartureTerminal),
                        TerminalDEST1 = checkTerminal(relatedFlightsOutbound[1].ArrivalTerminal),
                        logo1 = "/assets/img/airlogo_square/" + relatedFlightsOutbound[1].CarrierCode + ".gif",
                        FlightName2 = relatedFlightsOutbound[1].CarrierCode,
                        Cabin2 = relatedFlightsOutbound[1].Cabin,
                        CarrierName2 = relatedFlightsOutbound[1].CarrierName,
                        FlightNumber2 = relatedFlightsOutbound[1].FlightNumber,
                        FlightDepDate2 = relatedFlightsOutbound[1].DepartureDate,
                        FlightDepTime2 = relatedFlightsOutbound[1].DepartureTime,
                        FlightArrDate2 = relatedFlightsOutbound[1].ArrivalDate,
                        FlightArrTime2 = relatedFlightsOutbound[1].ArrivalTime,
                        Duration2 = relatedFlightsOutbound[1].DurationDesc,
                        Layover1 = relatedFlightsOutbound[1].JourneyTimeDesc,

                        Class3 = ReturnCls(relatedFlightsOutbound[2]),
                        SRC3 = relatedFlightsOutbound[2].DepartureStation,
                        DEST3 = relatedFlightsOutbound[2].ArrivalStation,
                        DepartStationName3 = relatedFlightsOutbound[2].DepartureStationName,
                        ArrivalStationName3 = relatedFlightsOutbound[2].ArrivalStationName,
                        DepartureStationAirport3 = relatedFlightsOutbound[2].DepartureStationAirport,
                        ArrivalStationAirport3 = relatedFlightsOutbound[2].ArrivalStationAirport,
                        Via2 = checkVia(relatedFlightsOutbound[2].Via),
                        ViaName2 = checkViaName(relatedFlightsOutbound[2].ViaName),
                        TerminalSRC2 = checkTerminal(relatedFlightsOutbound[2].DepartureTerminal),
                        TerminalDEST2 = checkTerminal(relatedFlightsOutbound[2].ArrivalTerminal),
                        logo2 = "/assets/img/airlogo_square/" + relatedFlightsOutbound[2].CarrierCode + ".gif",
                        FlightName3 = relatedFlightsOutbound[2].CarrierCode,
                        CarrierName3 = relatedFlightsOutbound[2].CarrierName,
                        Cabin3 = relatedFlightsOutbound[2].Cabin,
                        FlightNumber3 = relatedFlightsOutbound[2].FlightNumber,
                        FlightDepDate3 = relatedFlightsOutbound[2].DepartureDate,
                        FlightDepTime3 = relatedFlightsOutbound[2].DepartureTime,
                        FlightArrDate3 = relatedFlightsOutbound[2].ArrivalDate,
                        FlightArrTime3 = relatedFlightsOutbound[2].ArrivalTime,
                        Duration3 = relatedFlightsOutbound[2].DurationDesc,
                        Layover2 = relatedFlightsOutbound[2].JourneyTimeDesc,
                    });
                }
                else if (relatedFlightsOutbound.Count == 4)
                {

                }
                #endregion


                relatedFlightsdtInbound = relatedFlightsdtInbound.OrderBy(x => x.TotalFare).ToList();
                #region

                if (relatedFlightsdtInbound.Count == 1)
                {

                    var relatedFlight = relatedFlightsdtInbound.FirstOrDefault();
                    rules = GetFareRules(relatedFlightsdtInbound);
                    string departureDate = relatedFlight.DepartureDate.ToString(); // Assuming DepartureDate is a property of k_ShowFlightOutBound
                    string arrivalDate = relatedFlight.ArrivalDate.ToString();
                    if (departureDate != arrivalDate)
                    {
                        arrivalCheck = DateHelper.DayDiff(departureDate, arrivalDate);
                    }


                    FlightOutBound.Add(new k_ShowFlightOutBound()
                    {
                        F_Status = fStatus,
                        F_Remark = fRemark,
                        CompanyID = CompanyID,
                        AgentType = ChekA_C,
                        FlightRefid = relatedFlight.RefID.ToString(),
                        Curr = curr,
                        FareUpdateMsg = msg,
                        FareUpdateMsgChek = FareUpdateMsgCheks,
                        InboundOutbound = String.Format("{0:N0}", int.Parse(InboundOutboundFare.ToString()) + int.Parse(ReturnGrossFare(relatedFlight).ToString())),

                        NoOFAdult = relatedFlight.Adt,
                        NoOFChild = relatedFlight.Chd,
                        NoOFInfant = relatedFlight.Inf,

                        Adt_BASIC = GetConvert(relatedFlight.Adt_BASIC.ToString()),
                        Chd_BASIC = GetConvert(relatedFlight.Chd_BASIC.ToString()),
                        Inf_BASIC = GetConvert(relatedFlight.Inf_BASIC.ToString()),

                        AdultbaseFare = relatedFlight.Adt * GetConvert(relatedFlight.Adt_BASIC),
                        ChildbaseFare = relatedFlight.Chd * GetConvert(relatedFlight.Chd_BASIC),
                        InfantbaseFare = relatedFlight.Inf * GetConvert(relatedFlight.Inf_BASIC),

                        Adt_YQ = GetConvert(relatedFlight.Adt_YQ),
                        AdtTotalTax = GetConvert(relatedFlight.AdtTotalTax),
                        Chd_YQ = GetConvert(relatedFlight.Chd_YQ),
                        ChdTotalTax = GetConvert(relatedFlight.ChdTotalTax),
                        Inf_TAX = GetConvert(relatedFlight.Inf_TAX),

                        Adt_AcuTax = ReturnTAXADT(relatedFlight).ToString(),
                        Chd_AcuTax = ReturnTAXCHD(relatedFlight).ToString(),
                        Inf_AcuTax = ReturnTAXINF(relatedFlight).ToString(),

                        Yq = "Commission : " + String.Format("{0:N0}", GetConvert(relatedFlight.TotalCommission)),
                        PromoChek = promo,

                        TotalTax = GetTotalTax(relatedFlight),
                        TotalBasic = GetTotalBasic(relatedFlight),
                        TotalServiceFee = GetTotalServiceFee(relatedFlight),
                        TotalMarkUp = GetTotalMarkUp(relatedFlight),
                        TotalServiceTax = GetTotalServiceTax(relatedFlight),
                        TaxandCharges = GetTaxandCharges(relatedFlight),
                        TotalCommission = ReturnCommission(relatedFlight, CompanyID),
                        TotalTds = ReturnTDS(relatedFlight, CompanyID),

                        GrossF = ReturnGrossFare(relatedFlight).ToString(),
                        FinalFare = ReturnFinalFare(relatedFlight, CompanyID),
                        TotalAmount = ReturnTotalFareAmount(relatedFlight),
                        TotalFare = ReturnTotalFareAmount(relatedFlight),
                        //==============================================================================================================================

                        FlightName = relatedFlight.CarrierCode,
                        DepartStationName = relatedFlight.DepartureStationName,
                        ArrivalStationName = relatedFlight.ArrivalStationName,
                        DepartureStationAirport = relatedFlight.DepartureStationAirport,
                        ArrivalStationAirport = relatedFlight.ArrivalStationAirport,
                        Cabin = relatedFlight.Cabin,
                        CarrierName = relatedFlight.CarrierName,
                        FlightNumber = relatedFlight.FlightNumber,
                        FlightDepDate = relatedFlight.DepartureDate,
                        FlightDepTime = relatedFlight.DepartureTime,
                        FlightArrDate = relatedFlight.ArrivalDate,
                        FlightArrTime = relatedFlight.ArrivalTime,
                        RuleTarrif = relatedFlight.RuleTarrif ?? string.Empty,
                        CHECKINBaggage = ReturnCheckINBaggage(relatedFlight).ToString(),
                        CABINBaggage = ReturnCabinINBaggage(relatedFlight).ToString(),
                        SRC = relatedFlight.Origin,
                        DEST = relatedFlight.Destination,
                        PrimarySRC = relatedFlight.Origin,
                        PrimaryDEST = relatedFlight.Destination,
                        Stop = "NonStop",
                        logo = "/assets/img/airlogo_square/" + relatedFlight.CarrierCode + ".gif",
                        Duration = relatedFlight.DurationDesc,
                        connection = 1,
                        SMSACTIVES = 0,
                        ArrivalNextDayCheck = arrivalCheck,
                        PriceType = relatedFlight.PriceType,
                        RefundType = ReturnRefundType(relatedFlight).ToString(),
                        AvailableSeat = checkSeat(relatedFlight.SeatsAvailable.ToString()),
                        FareRules = rules,

                        Class1 = ReturnCls(relatedFlight),
                        SRC1 = relatedFlight.DepartureStation,
                        DEST1 = relatedFlight.ArrivalStation,
                        DepartStationName1 = relatedFlight.DepartureStationName,
                        ArrivalStationName1 = relatedFlight.ArrivalStationName,
                        DepartureStationAirport1 = relatedFlight.DepartureStationAirport,
                        ArrivalStationAirport1 = relatedFlight.ArrivalStationAirport,
                        Via = checkVia(relatedFlight.Via),
                        ViaName = checkViaName(relatedFlight.ViaName),
                        TerminalSRC = checkTerminal(relatedFlight.DepartureTerminal),
                        TerminalDEST = checkTerminal(relatedFlight.ArrivalTerminal),
                        FlightName1 = relatedFlight.CarrierCode,
                        Cabin1 = relatedFlight.Cabin,
                        CarrierName1 = relatedFlight.CarrierName,
                        FlightNumber1 = relatedFlight.FlightNumber,
                        FlightDepDate1 = relatedFlight.DepartureDate,
                        FlightDepTime1 = relatedFlight.DepartureTime,
                        FlightArrDate1 = relatedFlight.ArrivalDate,
                        FlightArrTime1 = relatedFlight.ArrivalTime,
                        Layover1 = relatedFlight.JourneyTimeDesc,
                        Duration1 = relatedFlight.DurationDesc,
                    });
                }
                else if (relatedFlightsdtInbound.Count == 2)
                {
                   
                    rules = GetFareRules(relatedFlightsdtInbound);
                    if (relatedFlightsdtInbound[0].DepartureDate.ToString() != relatedFlightsdtInbound[1].ArrivalDate.ToString())
                    {
                        arrivalCheck = DateHelper.DayDiff(relatedFlightsdtInbound[0].DepartureDate.ToString(), relatedFlightsdtInbound[1].ArrivalDate.ToString());
                    }

                    FlightOutBound.Add(new k_ShowFlightOutBound()
                    {
                        F_Status = fStatus,
                        F_Remark = fRemark,
                        CompanyID = CompanyID,
                        AgentType = ChekA_C,
                        FlightRefid = relatedFlightsdtInbound[0].RefID.ToString(),
                        Curr = curr,
                        FareUpdateMsg = msg,
                        FareUpdateMsgChek = FareUpdateMsgCheks,
                        InboundOutbound = String.Format("{0:N0}", int.Parse(InboundOutboundFare.ToString()) + int.Parse(ReturnGrossFare(relatedFlightsdtInbound[0]).ToString())),

                        NoOFAdult =  relatedFlightsdtInbound[0].Adt,
                        NoOFChild =  relatedFlightsdtInbound[0].Chd,
                        NoOFInfant =  relatedFlightsdtInbound[0].Inf,
                        //==============================================================================================================================
                        Adt_BASIC = GetConvert( relatedFlightsdtInbound[0].Adt_BASIC).ToString(),
                        Chd_BASIC = GetConvert( relatedFlightsdtInbound[0].Chd_BASIC).ToString(),
                        Inf_BASIC = GetConvert( relatedFlightsdtInbound[0].Inf_BASIC).ToString(),

                        AdultbaseFare =  relatedFlightsdtInbound[0].Adt * GetConvert( relatedFlightsdtInbound[0].Adt_BASIC),
                        ChildbaseFare =  relatedFlightsdtInbound[0].Chd * GetConvert( relatedFlightsdtInbound[0].Chd_BASIC),
                        InfantbaseFare =  relatedFlightsdtInbound[0].Inf * GetConvert( relatedFlightsdtInbound[0].Inf_BASIC),


                        Adt_YQ = GetConvert( relatedFlightsdtInbound[0].Adt_YQ),
                        AdtTotalTax = GetConvert( relatedFlightsdtInbound[0].AdtTotalTax),
                        Chd_YQ = GetConvert( relatedFlightsdtInbound[0].Chd_YQ),
                        ChdTotalTax = GetConvert( relatedFlightsdtInbound[0].ChdTotalTax),
                        Inf_TAX = GetConvert( relatedFlightsdtInbound[0].Inf_TAX),

                        Adt_AcuTax = ReturnTAXADT( relatedFlightsdtInbound[0]),
                        Chd_AcuTax = ReturnTAXCHD( relatedFlightsdtInbound[0]),
                        Inf_AcuTax = ReturnTAXINF( relatedFlightsdtInbound[0]),

                        Yq = "Commission : " + String.Format("{0:N0}", GetConvert( relatedFlightsdtInbound[0].TotalCommission)),
                        PromoChek = promo,

                        TotalTax = GetTotalTax( relatedFlightsdtInbound[0]),
                        TotalBasic = GetTotalBasic( relatedFlightsdtInbound[0]),
                        TotalServiceFee = GetTotalServiceFee( relatedFlightsdtInbound[0]),
                        TotalMarkUp = GetTotalMarkUp( relatedFlightsdtInbound[0]),
                        TotalServiceTax = GetTotalServiceTax( relatedFlightsdtInbound[0]),
                        TaxandCharges = GetTaxandCharges( relatedFlightsdtInbound[0]),
                        TotalCommission = ReturnCommission( relatedFlightsdtInbound[0], CompanyID),
                        TotalTds = ReturnTDS( relatedFlightsdtInbound[0], CompanyID),

                        GrossF = ReturnGrossFare( relatedFlightsdtInbound[0]),
                        FinalFare = ReturnFinalFare( relatedFlightsdtInbound[0], CompanyID),
                        TotalAmount = ReturnTotalFareAmount( relatedFlightsdtInbound[0]),
                        TotalFare = ReturnTotalFareAmount( relatedFlightsdtInbound[0]),
                        //==============================================================================================================================

                        FlightName =  relatedFlightsdtInbound[0].CarrierCode,
                        Cabin =  relatedFlightsdtInbound[0].Cabin,
                        DepartStationName =  relatedFlightsdtInbound[0].DepartureStationName,
                        ArrivalStationName = relatedFlightsdtInbound[1].ArrivalStationName,
                        DepartureStationAirport =  relatedFlightsdtInbound[0].DepartureStationAirport,
                        ArrivalStationAirport = relatedFlightsdtInbound[1].ArrivalStationAirport,
                        RuleTarrif =  relatedFlightsdtInbound[0].RuleTarrif ?? string.Empty,
                        CarrierName =  relatedFlightsdtInbound[0].CarrierName,
                        FlightNumber =  relatedFlightsdtInbound[0].FlightNumber,
                        FlightDepDate =  relatedFlightsdtInbound[0].DepartureDate,
                        FlightDepTime =  relatedFlightsdtInbound[0].DepartureTime,
                        FlightArrDate = relatedFlightsdtInbound[1].ArrivalDate,
                        FlightArrTime = relatedFlightsdtInbound[1].ArrivalTime,
                        CHECKINBaggage = ReturnCheckINBaggage( relatedFlightsdtInbound[0]),
                        CABINBaggage = ReturnCabinINBaggage( relatedFlightsdtInbound[0]),
                        SRC =  relatedFlightsdtInbound[0].Origin,
                        DEST =  relatedFlightsdtInbound[0].Destination,
                        logo = "/assets/img/airlogo_square/" +  relatedFlightsdtInbound[0].CarrierCode + ".gif",
                        Duration =  relatedFlightsdtInbound[0].DurationDesc,
                        connection = 2,
                        SMSACTIVES = 0,// SMSACTIVE,
                        ArrivalNextDayCheck = arrvchack,
                        PriceType =  relatedFlightsdtInbound[0].PriceType,
                        RefundType = ReturnRefundType( relatedFlightsdtInbound[0]),
                        AvailableSeat = checkSeat( relatedFlightsdtInbound[0].SeatsAvailable.ToString()),
                        FareRules = rules,

                        Class1 = ReturnCls(relatedFlightsdtInbound[0]).ToString(),
                        SRC1 = relatedFlightsdtInbound[0].DepartureStation,
                        DEST1 =  relatedFlightsdtInbound[0].ArrivalStation,
                        DepartStationName1 =  relatedFlightsdtInbound[0].DepartureStationName,
                        ArrivalStationName1 =  relatedFlightsdtInbound[0].ArrivalStationName,
                        DepartureStationAirport1 =  relatedFlightsdtInbound[0].DepartureStationAirport,
                        ArrivalStationAirport1 =  relatedFlightsdtInbound[0].ArrivalStationAirport,
                        Stop = "OneStop",
                        Via = checkVia( relatedFlightsdtInbound[0].Via),
                        ViaName = checkViaName( relatedFlightsdtInbound[0].ViaName),
                        TerminalSRC = checkTerminal( relatedFlightsdtInbound[0].DepartureTerminal),
                        TerminalDEST = checkTerminal(relatedFlightsdtInbound[1].ArrivalTerminal),
                        FlightName1 =  relatedFlightsdtInbound[0].CarrierCode,
                        Cabin1 =  relatedFlightsdtInbound[0].Cabin,
                        CarrierName1 =  relatedFlightsdtInbound[0].CarrierName,
                        FlightNumber1 =  relatedFlightsdtInbound[0].FlightNumber,
                        FlightDepDate1 =  relatedFlightsdtInbound[0].DepartureDate,
                        FlightDepTime1 =  relatedFlightsdtInbound[0].DepartureTime,
                        FlightArrDate1 =  relatedFlightsdtInbound[0].ArrivalDate,
                        FlightArrTime1 =  relatedFlightsdtInbound[0].ArrivalTime,
                        Duration1 = relatedFlightsdtInbound[0].DurationDesc,

                        Class2 = ReturnCls(relatedFlightsdtInbound[1]).ToString(),
                        SRC2 = relatedFlightsdtInbound[1].DepartureStation,
                        DEST2 =  relatedFlightsdtInbound[1].ArrivalStation,
                        DepartStationName2 =  relatedFlightsdtInbound[1].DepartureStationName,
                        ArrivalStationName2 =  relatedFlightsdtInbound[1].ArrivalStationName,
                        DepartureStationAirport2 =  relatedFlightsdtInbound[1].DepartureStationAirport,
                        ArrivalStationAirport2 =  relatedFlightsdtInbound[1].ArrivalStationAirport,
                        Via1 = checkVia( relatedFlightsdtInbound[1].Via),
                        ViaName1 = checkViaName( relatedFlightsdtInbound[1].ViaName),
                        TerminalSRC1 = checkTerminal( relatedFlightsdtInbound[1].DepartureTerminal),
                        TerminalDEST1 = checkTerminal( relatedFlightsdtInbound[1].ArrivalTerminal),
                        logo1 = "/assets/img/airlogo_square/" +  relatedFlightsdtInbound[1].CarrierCode + ".gif",
                        FlightName2 =  relatedFlightsdtInbound[1].CarrierCode,
                        Cabin2 =  relatedFlightsdtInbound[1].Cabin,
                        CarrierName2 =  relatedFlightsdtInbound[1].CarrierName,
                        FlightNumber2 =  relatedFlightsdtInbound[1].FlightNumber,
                        FlightDepDate2 =  relatedFlightsdtInbound[1].DepartureDate,
                        FlightDepTime2 =  relatedFlightsdtInbound[1].DepartureTime,
                        FlightArrDate2 =  relatedFlightsdtInbound[1].ArrivalDate,
                        FlightArrTime2 =  relatedFlightsdtInbound[1].ArrivalTime,
                        Duration2 =  relatedFlightsdtInbound[1].DurationDesc,
                        Layover1 = relatedFlightsdtInbound[1].JourneyTimeDesc,
                    });
                }
                else if (relatedFlightsdtInbound.Count == 3)
                {

                    rules = GetFareRules(relatedFlightsdtInbound);
                    if (relatedFlightsdtInbound[0].DepartureDate.ToString() != relatedFlightsdtInbound[2].ArrivalDate.ToString())
                    {
                        arrivalCheck = DateHelper.DayDiff(relatedFlightsdtInbound[0].DepartureDate.ToString(), relatedFlightsdtInbound[2].ArrivalDate.ToString());
                    }


                    FlightOutBound.Add(new k_ShowFlightOutBound()
                    {
                        F_Status = fStatus,
                        F_Remark = fRemark,
                        CompanyID = CompanyID,
                        AgentType = ChekA_C,
                        FlightRefid = relatedFlightsdtInbound[0].RefID.ToString(),
                        Curr = curr,
                        FareUpdateMsg = msg,
                        FareUpdateMsgChek = FareUpdateMsgCheks,
                        InboundOutbound = String.Format("{0:N0}", int.Parse(InboundOutboundFare.ToString()) + int.Parse(ReturnGrossFare(relatedFlightsdtInbound[0]).ToString())),
                        NoOFAdult = relatedFlightsdtInbound[0].Adt,
                        NoOFChild = relatedFlightsdtInbound[0].Chd,
                        NoOFInfant = relatedFlightsdtInbound[0].Inf,
                        //==============================================================================================================================
                        Adt_BASIC = GetConvert(relatedFlightsdtInbound[0].Adt_BASIC).ToString(),
                        Chd_BASIC = GetConvert(relatedFlightsdtInbound[0].Chd_BASIC).ToString(),
                        Inf_BASIC = GetConvert(relatedFlightsdtInbound[0].Inf_BASIC).ToString(),

                        AdultbaseFare = relatedFlightsdtInbound[0].Adt * GetConvert(relatedFlightsdtInbound[0].Adt_BASIC),
                        ChildbaseFare = relatedFlightsdtInbound[0].Chd * GetConvert(relatedFlightsdtInbound[0].Chd_BASIC),
                        InfantbaseFare = relatedFlightsdtInbound[0].Inf * GetConvert(relatedFlightsdtInbound[0].Inf_BASIC),


                        Adt_YQ = GetConvert(relatedFlightsdtInbound[0].Adt_YQ),
                        AdtTotalTax = GetConvert(relatedFlightsdtInbound[0].AdtTotalTax),
                        Chd_YQ = GetConvert(relatedFlightsdtInbound[0].Chd_YQ),
                        ChdTotalTax = GetConvert(relatedFlightsdtInbound[0].ChdTotalTax),
                        Inf_TAX = GetConvert(relatedFlightsdtInbound[0].Inf_TAX),

                        Adt_AcuTax = ReturnTAXADT(relatedFlightsdtInbound[0]),
                        Chd_AcuTax = ReturnTAXCHD(relatedFlightsdtInbound[0]),
                        Inf_AcuTax = ReturnTAXINF(relatedFlightsdtInbound[0]),

                        Yq = "Commission : " + String.Format("{0:N0}", GetConvert(relatedFlightsdtInbound[0].TotalCommission)),
                        PromoChek = promo,

                        TotalTax = GetTotalTax(relatedFlightsdtInbound[0]),
                        TotalBasic = GetTotalBasic(relatedFlightsdtInbound[0]),
                        TotalServiceFee = GetTotalServiceFee(relatedFlightsdtInbound[0]),
                        TotalMarkUp = GetTotalMarkUp(relatedFlightsdtInbound[0]),
                        TotalServiceTax = GetTotalServiceTax(relatedFlightsdtInbound[0]),
                        TaxandCharges = GetTaxandCharges(relatedFlightsdtInbound[0]),
                        TotalCommission = ReturnCommission(relatedFlightsdtInbound[0], CompanyID),
                        TotalTds = ReturnTDS(relatedFlightsdtInbound[0], CompanyID),

                        GrossF = ReturnGrossFare(relatedFlightsdtInbound[0]),
                        FinalFare = ReturnFinalFare(relatedFlightsdtInbound[0], CompanyID),
                        TotalAmount = ReturnTotalFareAmount(relatedFlightsdtInbound[0]),
                        TotalFare = ReturnTotalFareAmount(relatedFlightsdtInbound[0]),
                        //==============================================================================================================================

                        FlightName = relatedFlightsdtInbound[0].CarrierCode,
                        Cabin = relatedFlightsdtInbound[0].Cabin,
                        DepartStationName = relatedFlightsdtInbound[0].DepartureStationName,
                        ArrivalStationName = relatedFlightsdtInbound[2].ArrivalStationName,
                        DepartureStationAirport = relatedFlightsdtInbound[0].DepartureStationAirport,
                        ArrivalStationAirport = relatedFlightsdtInbound[2].ArrivalStationAirport,
                        RuleTarrif = relatedFlightsdtInbound[0].RuleTarrif ?? string.Empty,
                        CarrierName = relatedFlightsdtInbound[0].CarrierName,
                        FlightNumber = relatedFlightsdtInbound[0].FlightNumber,
                        FlightDepDate = relatedFlightsdtInbound[0].DepartureDate,
                        FlightDepTime = relatedFlightsdtInbound[0].DepartureTime,
                        FlightArrDate = relatedFlightsdtInbound[2].ArrivalDate,
                        FlightArrTime = relatedFlightsdtInbound[2].ArrivalTime,
                        CHECKINBaggage = ReturnCheckINBaggage(relatedFlightsdtInbound[0]),
                        CABINBaggage = ReturnCabinINBaggage(relatedFlightsdtInbound[0]),


                        SRC = relatedFlightsdtInbound[0].Origin,
                        DEST = relatedFlightsdtInbound[0].Destination,
                        logo = "/assets/img/airlogo_square/" + relatedFlightsdtInbound[0].CarrierCode + ".gif",
                        Duration = relatedFlightsdtInbound[0].DurationDesc,
                        Stop = "TwoStop",
                        PriceType = relatedFlightsdtInbound[0].PriceType,
                        RefundType = ReturnRefundType(relatedFlightsdtInbound[0]),
                        AvailableSeat = checkSeat(relatedFlightsdtInbound[0].SeatsAvailable.ToString()),
                        FareRules = rules,
                        connection = 3,
                        ArrivalNextDayCheck = arrvchack,
                        SMSACTIVES = 0,

                        Class1 = ReturnCls(relatedFlightsdtInbound[0]).ToString(),
                        SRC1 = relatedFlightsdtInbound[0].DepartureStation,
                        DEST1 = relatedFlightsdtInbound[0].ArrivalStation,
                        DepartStationName1 = relatedFlightsdtInbound[0].DepartureStationName,
                        ArrivalStationName1 = relatedFlightsdtInbound[0].ArrivalStationName,
                        DepartureStationAirport1 = relatedFlightsdtInbound[0].DepartureStationAirport,
                        ArrivalStationAirport1 = relatedFlightsdtInbound[0].ArrivalStationAirport,
                        Via = checkVia(relatedFlightsdtInbound[0].Via),
                        ViaName = checkViaName(relatedFlightsdtInbound[0].ViaName),
                        TerminalSRC = checkTerminal(relatedFlightsdtInbound[0].DepartureTerminal),
                        TerminalDEST = checkTerminal(relatedFlightsdtInbound[2].ArrivalTerminal),
                        FlightName1 = relatedFlightsdtInbound[0].CarrierCode,
                        Cabin1 = relatedFlightsdtInbound[0].Cabin,
                        CarrierName1 = relatedFlightsdtInbound[0].CarrierName,
                        FlightNumber1 = relatedFlightsdtInbound[0].FlightNumber,
                        FlightDepDate1 = relatedFlightsdtInbound[0].DepartureDate,
                        FlightDepTime1 = relatedFlightsdtInbound[0].DepartureTime,
                        FlightArrDate1 = relatedFlightsdtInbound[0].ArrivalDate,
                        FlightArrTime1 = relatedFlightsdtInbound[0].ArrivalTime,
                        Duration1 = relatedFlightsdtInbound[0].DurationDesc,

                        Class2 = ReturnCls(relatedFlightsdtInbound[1]).ToString(),
                        SRC2 = relatedFlightsdtInbound[1].DepartureStation,
                        DEST2 = relatedFlightsdtInbound[1].ArrivalStation,
                        DepartStationName2 = relatedFlightsdtInbound[1].DepartureStationName,
                        ArrivalStationName2 = relatedFlightsdtInbound[1].ArrivalStationName,
                        DepartureStationAirport2 = relatedFlightsdtInbound[1].DepartureStationAirport,
                        ArrivalStationAirport2 = relatedFlightsdtInbound[1].ArrivalStationAirport,
                        Via1 = checkVia(relatedFlightsdtInbound[1].Via),
                        ViaName1 = checkViaName(relatedFlightsdtInbound[1].ViaName),
                        TerminalSRC1 = checkTerminal(relatedFlightsdtInbound[1].DepartureTerminal),
                        TerminalDEST1 = checkTerminal(relatedFlightsdtInbound[1].ArrivalTerminal),
                        logo1 = "/assets/img/airlogo_square/" + relatedFlightsdtInbound[1].CarrierCode + ".gif",
                        FlightName2 = relatedFlightsdtInbound[1].CarrierCode,
                        Cabin2 = relatedFlightsdtInbound[1].Cabin,
                        CarrierName2 = relatedFlightsdtInbound[1].CarrierName,
                        FlightNumber2 = relatedFlightsdtInbound[1].FlightNumber,
                        FlightDepDate2 = relatedFlightsdtInbound[1].DepartureDate,
                        FlightDepTime2 = relatedFlightsdtInbound[1].DepartureTime,
                        FlightArrDate2 = relatedFlightsdtInbound[1].ArrivalDate,
                        FlightArrTime2 = relatedFlightsdtInbound[1].ArrivalTime,
                        Duration2 = relatedFlightsdtInbound[1].DurationDesc,
                        Layover1 = relatedFlightsdtInbound[1].JourneyTimeDesc,

                        Class3 = ReturnCls(relatedFlightsdtInbound[2]),
                        SRC3 = relatedFlightsdtInbound[2].DepartureStation,
                        DEST3 =  relatedFlightsdtInbound[2].ArrivalStation,
                        DepartStationName3 =  relatedFlightsdtInbound[2].DepartureStationName,
                        ArrivalStationName3 = relatedFlightsdtInbound[2].ArrivalStationName,
                        DepartureStationAirport3 =  relatedFlightsdtInbound[2].DepartureStationAirport,
                        ArrivalStationAirport3 =  relatedFlightsdtInbound[2].ArrivalStationAirport,
                        Via2 = checkVia( relatedFlightsdtInbound[2].Via),
                        ViaName2 = checkViaName( relatedFlightsdtInbound[2].ViaName),
                        TerminalSRC2 = checkTerminal( relatedFlightsdtInbound[2].DepartureTerminal),
                        TerminalDEST2 = checkTerminal( relatedFlightsdtInbound[2].ArrivalTerminal),
                        logo2 = "/assets/img/airlogo_square/" +  relatedFlightsdtInbound[2].CarrierCode + ".gif",
                        FlightName3 =  relatedFlightsdtInbound[2].CarrierCode,
                        CarrierName3 =  relatedFlightsdtInbound[2].CarrierName,
                        Cabin3 =  relatedFlightsdtInbound[2].Cabin,
                        FlightNumber3 =  relatedFlightsdtInbound[2].FlightNumber,
                        FlightDepDate3 =  relatedFlightsdtInbound[2].DepartureDate,
                        FlightDepTime3 =  relatedFlightsdtInbound[2].DepartureTime,
                        FlightArrDate3 =  relatedFlightsdtInbound[2].ArrivalDate,
                        FlightArrTime3 =  relatedFlightsdtInbound[2].ArrivalTime,
                        Duration3 =  relatedFlightsdtInbound[2].DurationDesc,
                        Layover2 = relatedFlightsdtInbound[2].JourneyTimeDesc,
                    });
                }
                else if (relatedFlightsdtInbound.Count == 4)
                {

                }
                #endregion
            }
            else
            {
                FlightOutBound.Add(new k_ShowFlightOutBound()
                {
                    FareUpdateMsg = msg,
                    FareUpdateMsgChek = 2
                });
            }
            return FlightOutBound;
        }

        public static async Task<List<k_ShowFlightInternational>> SelectInt(string refid, string CompanyID, ClaimsPrincipal user, IHandlesQueryAsync<GetAirFareQuery, string> getAirFareHandler, IHandlesQueryAsync<GetAirFareRulesQuery, string> getAirFareRulesHandler, IHandlesQueryAsync<string, CompanyRegisterCorporateUserDetails> getCompanyRegisterCorporateUserDetailsQueryHandler, IHandlesQueryAsync<string, CompanyRegisterCorporateUserLimitDetails> getCompanyRegisterCorporateUserLimitQueryHandler)
        {
            List<k_ShowFlightInternational> FlightOutBound = new List<k_ShowFlightInternational>();
            //=============================================================================================================
            string fRemark = string.Empty;
            int Limit = 0;

            var userInfo = await SelectedFlightUserInfo(user, refid, getCompanyRegisterCorporateUserDetailsQueryHandler, getCompanyRegisterCorporateUserLimitQueryHandler);
            var fStatus = userInfo.ValidTimeUser;

            if (fStatus)
            {
                var getfareResponse = await ShowFlightFareHelper.GetFareCO(CompanyID, refid, getAirFareHandler, getAirFareRulesHandler);
                fStatus = getfareResponse.FareStatus;
                fRemark = getfareResponse.FareRemark;
            }
            //=============================================================================================================

            HttpContextHelper.Current.Session.SetString("SelectedFltOut", refid);

          
            int arrvchack = 0;
            int arrvchackR = 0;
            int Promo = 0;
            int ChekA_C = 0;
            string msg = string.Empty;
            //int FareUpdateMsgCheks = 0;
            string rules = string.Empty;
            string rulesIB = string.Empty;
            arrvchack = 0;
            arrvchackR = 0;
            int promo = 0;
            rulesIB = "Fare Rules are just a guideline for your convenience and is subject to changes by the Airline from time to time.For Any query related to Fare Rules please contact to our callcenter.";
            rules = "Fare Rules are just a guideline for your convenience and is subject to changes by the Airline from time to time.For Any query related to Fare Rules please contact to our callcenter.";
            arrvchack = 0;
            arrvchackR = 0;
            Promo = 0;

            var flightList = GetSelec();
            //var flightListR = GetSelec("I");

            var relatedFlights = flightList.Where(x => x.RefID == Convert.ToInt32(refid) && x.FltType == "O").ToList();
            var relatedFlightsR = flightList.Where(x => x.RefID == Convert.ToInt32(refid) && x.FltType == "I").ToList();

            string curr = "INR";
            // Check session or alternative storage for currency
            if (HttpContextHelper.Current?.Session.GetString("Curr") != null)
            {
                curr = HttpContextHelper.Current.Session.GetString("Curr");
            }


            #region
            if (relatedFlights.Count == 1)
            {
                try
                {
                    rules = GetFareRules(relatedFlights);
                    rulesIB = GetFareRules(relatedFlightsR);
                    string departureDate = relatedFlights[0].DepartureDate.ToString(); // Assuming DepartureDate is a property of k_ShowFlightOutBound
                    string arrivalDate = relatedFlights[0].ArrivalDate.ToString();
                    if (departureDate != arrivalDate)
                    {
                        arrvchack = DateHelper.DayDiff(departureDate, arrivalDate);
                    }
                    #region
                    if (relatedFlightsR.Count == 1)
                    {

                        string departureDateR = relatedFlightsR[0].DepartureDate.ToString(); // Assuming DepartureDate is a property of k_ShowFlightOutBound
                        string arrivalDateR = relatedFlightsR[0].ArrivalDate.ToString();
                        if (departureDateR != arrivalDateR)
                        {
                            arrvchackR = DateHelper.DayDiff(departureDateR, arrivalDateR);
                        }

                        FlightOutBound.Add(new k_ShowFlightInternational()
                        {
                            F_Status = fStatus,
                            F_Remark = fRemark,
                            AgentType = ChekA_C,
                            CompanyID = CompanyID,
                            FlightRefid = relatedFlights[0].RefID.ToString(),
                            Curr = curr,

                            NoOFAdult = relatedFlights[0].Adt,
                            NoOFChild = relatedFlights[0].Chd,
                            NoOFInfant = relatedFlights[0].Inf,

                            //==============================================================================================================================
                            Adt_BASIC = GetConvert(relatedFlights[0].Adt_BASIC.ToString()),
                            Chd_BASIC = GetConvert(relatedFlights[0].Chd_BASIC.ToString()),
                            Inf_BASIC = GetConvert(relatedFlights[0].Inf_BASIC.ToString()),

                            AdultbaseFare = relatedFlights[0].Adt * GetConvert(relatedFlights[0].Adt_BASIC),
                            ChildbaseFare = relatedFlights[0].Chd * GetConvert(relatedFlights[0].Chd_BASIC),
                            InfantbaseFare = relatedFlights[0].Inf * GetConvert(relatedFlights[0].Inf_BASIC),

                            Adt_YQ = GetConvert(relatedFlights[0].Adt_YQ),
                            AdtTotalTax = GetConvert(relatedFlights[0].AdtTotalTax),
                            Chd_YQ = GetConvert(relatedFlights[0].Chd_YQ),
                            ChdTotalTax = GetConvert(relatedFlights[0].ChdTotalTax),
                            Inf_TAX = GetConvert(relatedFlights[0].Inf_TAX),

                            Adt_AcuTax = ReturnTAXADT(relatedFlights[0]).ToString(),
                            Chd_AcuTax = ReturnTAXCHD(relatedFlights[0]).ToString(),
                            Inf_AcuTax = ReturnTAXINF(relatedFlights[0]).ToString(),

                            Yq = "Commission : " + String.Format("{0:N0}", GetConvert(relatedFlights[0].TotalCommission)),
                            PromoChek = promo,


                            //==============================================================================================================================

                            TotalAmount = ReturnTotalFareAmount(relatedFlights[0]),
                            TotalFare = ReturnTotalFareAmount(relatedFlights[0]),
                            FlightAmount = ReturnTotalFareAmount(relatedFlights[0]),
                            TotalTax = GetTotalTax(relatedFlights[0]),
                            TotalBasic = GetTotalBasic(relatedFlights[0]),
                            TotalServiceFee = GetTotalServiceFee(relatedFlights[0]),
                            TotalMarkup = GetTotalMarkUp(relatedFlights[0]),
                            TotalServiceTax = GetTotalServiceTax(relatedFlights[0]),
                            TaxandCharges = GetTaxandCharges(relatedFlights[0]),
                            TotalCommission = ReturnCommission(relatedFlights[0], CompanyID),
                            TotalTds = ReturnTDS(relatedFlights[0], CompanyID),
                            FinalFare = ReturnFinalFare(relatedFlights[0], CompanyID),
                           

                            FlightName = relatedFlights[0].CarrierCode,
                            FlightNumber = relatedFlights[0].FlightNumber,
                            FlightDepDate = relatedFlights[0].DepartureDate,
                            FlightDepTime = relatedFlights[0].DepartureTime,
                            FlightArrDate = relatedFlights[0].ArrivalDate,
                            FlightArrTime = relatedFlights[0].ArrivalTime,
                            RuleTarrif = relatedFlights[0].RuleTarrif ?? string.Empty,
                            CHECKINBaggage = ReturnCheckINBaggage(relatedFlights[0]).ToString(),
                            CABINBaggage = ReturnCabinINBaggage(relatedFlights[0]).ToString(),
                            SRC = relatedFlights[0].Origin,
                            DEST = relatedFlights[0].Destination,
                            Stop = "NonStop",
                            logo = "/assets/img/airlogo_square/" + relatedFlights[0].CarrierCode + ".gif",
                            Duration = relatedFlights[0].DurationDesc,
                            connection = 1,
                            ArrivalNextDayCheck = arrvchack,
                            PriceType = relatedFlights[0].PriceType,
                            RefundType = ReturnRefundType(relatedFlights[0]).ToString(),
                            AvailableSeat = checkSeat(relatedFlights[0].SeatsAvailable.ToString()),
                            FareRules = rules,
                            Class = ReturnCls(relatedFlights[0]),
                            Cabin = relatedFlights[0].Cabin,

                            DepartStationName = relatedFlights[0].DepartureStationName,
                            ArrivalStationName = relatedFlights[0].ArrivalStationName,
                            DepartureStationAirport = relatedFlights[0].DepartureStationAirport,
                            ArrivalStationAirport = relatedFlights[0].ArrivalStationAirport,
                            TerminalSRC = checkTerminal(relatedFlights[0].DepartureTerminal),
                            TerminalDEST = checkTerminal(relatedFlights[0].ArrivalTerminal),
                            Via = checkVia(relatedFlights[0].Via),
                            ViaName = checkViaName(relatedFlights[0].ViaName),
                            Layover1 = relatedFlights[0].JourneyTimeDesc,

                            FlightName1 = relatedFlights[0].CarrierCode,
                            FlightNumber1 = relatedFlights[0].FlightNumber,
                            FlightDepDate1 = relatedFlights[0].DepartureDate,
                            FlightDepTime1 = relatedFlights[0].DepartureTime,
                            FlightArrDate1 = relatedFlights[0].ArrivalDate,
                            FlightArrTime1 = relatedFlights[0].ArrivalTime,
                            Duration1 = relatedFlights[0].DurationDesc,

                            CHECKINBaggageR = ReturnCheckINBaggage(relatedFlightsR[0]).ToString(),
                            CABINBaggageR = ReturnCabinINBaggage(relatedFlightsR[0]).ToString(),
                            FareRulesR = rulesIB,
                            FlightNameR = relatedFlightsR[0].CarrierCode,
                            FlightNumberR = relatedFlightsR[0].FlightNumber,
                            FlightDepDateR = relatedFlightsR[0].DepartureDate,
                            FlightDepTimeR = relatedFlightsR[0].DepartureTime,
                            FlightArrDateR = relatedFlightsR[0].ArrivalDate,
                            FlightArrTimeR = relatedFlightsR[0].ArrivalTime,
                            SRCR = relatedFlightsR[0].Origin,
                            DESTR = relatedFlightsR[0].Destination,
                            StopR = "NonStop",
                            logoR = "/assets/img/airlogo_square/" + relatedFlightsR[0].CarrierCode + ".gif",
                            DurationR = relatedFlightsR[0].DurationDesc,
                            connectionR = 1,
                            PriceTypeR = relatedFlightsR[0].PriceType,
                            RefundTypeR = ReturnRefundType(relatedFlightsR[0]).ToString(),
                            AvailableSeatR = checkSeat(relatedFlightsR[0].SeatsAvailable.ToString()),
                            ArrivalNextDayCheckR = arrvchackR,



                            DepartStationNameR = relatedFlightsR[0].DepartureStationName,
                            ArrivalStationNameR = relatedFlightsR[0].ArrivalStationName,
                            DepartureStationAirportR = relatedFlightsR[0].DepartureStationAirport,
                            ArrivalStationAirportR = relatedFlightsR[0].ArrivalStationAirport,
                            ClassR = ReturnCls(relatedFlightsR[0]).ToString(),
                            CabinR = relatedFlightsR[0].Cabin,

                            Layover1R = relatedFlightsR[0].JourneyTimeDesc,

                            ViaR = checkVia(relatedFlightsR[0].Via),
                            ViaNameR = checkViaName(relatedFlightsR[0].ViaName),
                            TerminalSRCR = checkTerminal(relatedFlightsR[0].DepartureTerminal),
                            TerminalDESTR = checkTerminal(relatedFlightsR[0].ArrivalTerminal),
                            FlightName1R = relatedFlightsR[0].CarrierCode,
                            FlightNumber1R = relatedFlightsR[0].FlightNumber,
                            FlightDepDate1R = relatedFlightsR[0].DepartureDate,
                            FlightDepTime1R = relatedFlightsR[0].DepartureTime,
                            FlightArrDate1R = relatedFlightsR[0].ArrivalDate,
                            FlightArrTime1R = relatedFlightsR[0].ArrivalTime,
                            Duration1R = relatedFlightsR[0].DurationDesc
                        });
                    }
                    #endregion
                    #region
                    else if (relatedFlightsR.Count == 2)
                    {
                        string departureDateR = relatedFlightsR[0].DepartureDate.ToString(); // Assuming DepartureDate is a property of k_ShowFlightOutBound
                        string arrivalDateR = relatedFlightsR[1].ArrivalDate.ToString();
                        if (departureDateR != arrivalDateR)
                        {
                            arrvchackR = DateHelper.DayDiff(departureDateR, arrivalDateR);
                        }


                        FlightOutBound.Add(new k_ShowFlightInternational()
                        {
                            F_Status = fStatus,
                            F_Remark = fRemark,
                            AgentType = ChekA_C,
                            CompanyID = CompanyID,
                            FlightRefid = relatedFlights[0].RefID.ToString(),
                            Curr = curr,

                            NoOFAdult = relatedFlights[0].Adt,
                            NoOFChild = relatedFlights[0].Chd,
                            NoOFInfant = relatedFlights[0].Inf,

                            //==============================================================================================================================
                            Adt_BASIC = GetConvert(relatedFlights[0].Adt_BASIC.ToString()),
                            Chd_BASIC = GetConvert(relatedFlights[0].Chd_BASIC.ToString()),
                            Inf_BASIC = GetConvert(relatedFlights[0].Inf_BASIC.ToString()),

                            AdultbaseFare = relatedFlights[0].Adt * GetConvert(relatedFlights[0].Adt_BASIC),
                            ChildbaseFare = relatedFlights[0].Chd * GetConvert(relatedFlights[0].Chd_BASIC),
                            InfantbaseFare = relatedFlights[0].Inf * GetConvert(relatedFlights[0].Inf_BASIC),

                            Adt_YQ = GetConvert(relatedFlights[0].Adt_YQ),
                            AdtTotalTax = GetConvert(relatedFlights[0].AdtTotalTax),
                            Chd_YQ = GetConvert(relatedFlights[0].Chd_YQ),
                            ChdTotalTax = GetConvert(relatedFlights[0].ChdTotalTax),
                            Inf_TAX = GetConvert(relatedFlights[0].Inf_TAX),

                            Adt_AcuTax = ReturnTAXADT(relatedFlights[0]).ToString(),
                            Chd_AcuTax = ReturnTAXCHD(relatedFlights[0]).ToString(),
                            Inf_AcuTax = ReturnTAXINF(relatedFlights[0]).ToString(),

                            Yq = "Commission : " + String.Format("{0:N0}", GetConvert(relatedFlights[0].TotalCommission)),
                            PromoChek = promo,


                            TotalAmount = ReturnTotalFareAmount(relatedFlights[0]),
                            TotalFare = ReturnTotalFareAmount(relatedFlights[0]),
                            FlightAmount = ReturnTotalFareAmount(relatedFlights[0]),
                            TotalTax = GetTotalTax(relatedFlights[0]),
                            TotalBasic = GetTotalBasic(relatedFlights[0]),
                            TotalServiceFee = GetTotalServiceFee(relatedFlights[0]),
                            TotalMarkup = GetTotalMarkUp(relatedFlights[0]),
                            TotalServiceTax = GetTotalServiceTax(relatedFlights[0]),
                            TaxandCharges = GetTaxandCharges(relatedFlights[0]),
                            TotalCommission = ReturnCommission(relatedFlights[0], CompanyID),
                            TotalTds = ReturnTDS(relatedFlights[0], CompanyID),
                            FinalFare = ReturnFinalFare(relatedFlights[0], CompanyID),

                            FlightName = relatedFlights[0].CarrierCode,
                            FlightNumber = relatedFlights[0].FlightNumber,
                            FlightDepDate = relatedFlights[0].DepartureDate,
                            FlightDepTime = relatedFlights[0].DepartureTime,
                            FlightArrDate = relatedFlights[0].ArrivalDate,
                            FlightArrTime = relatedFlights[0].ArrivalTime,
                            RuleTarrif = relatedFlights[0].RuleTarrif ?? string.Empty,
                            CHECKINBaggage = ReturnCheckINBaggage(relatedFlights[0]).ToString(),
                            CABINBaggage = ReturnCabinINBaggage(relatedFlights[0]).ToString(),
                            SRC = relatedFlights[0].Origin,
                            DEST = relatedFlights[0].Destination,
                            Stop = "NonStop",
                            logo = "/assets/img/airlogo_square/" + relatedFlights[0].CarrierCode + ".gif",
                            Duration = relatedFlights[0].DurationDesc,
                            connection = 1,
                            ArrivalNextDayCheck = arrvchack,
                            PriceType = relatedFlights[0].PriceType,
                            RefundType = ReturnRefundType(relatedFlights[0]).ToString(),
                            AvailableSeat = checkSeat(relatedFlights[0].SeatsAvailable.ToString()),
                            FareRules = rules,
                            DepartStationName = relatedFlights[0].DepartureStationName,
                            ArrivalStationName = relatedFlights[0].ArrivalStationName,
                            DepartureStationAirport = relatedFlights[0].DepartureStationAirport,
                            ArrivalStationAirport = relatedFlights[0].ArrivalStationAirport,
                            Class = ReturnCls(relatedFlights[0]),
                            Cabin = relatedFlights[0].Cabin,
                           
                            Via = checkVia(relatedFlights[0].Via),
                            ViaName = checkViaName(relatedFlights[0].ViaName),
                            TerminalSRC = checkTerminal(relatedFlights[0].DepartureTerminal),
                            TerminalDEST = checkTerminal(relatedFlights[0].ArrivalTerminal),
                            Layover1 = relatedFlights[0].JourneyTimeDesc,


                            FlightName1 = relatedFlights[0].CarrierCode,
                            FlightNumber1 = relatedFlights[0].FlightNumber,
                            FlightDepDate1 = relatedFlights[0].DepartureDate,
                            FlightDepTime1 = relatedFlights[0].DepartureTime,
                            FlightArrDate1 = relatedFlights[0].ArrivalDate,
                            FlightArrTime1 = relatedFlights[0].ArrivalTime,
                            Duration1 = relatedFlights[0].DurationDesc,

                            FareRulesR = rulesIB,
                            CHECKINBaggageR = ReturnCheckINBaggage(relatedFlightsR[0]).ToString(),
                            CABINBaggageR = ReturnCabinINBaggage(relatedFlightsR[0]).ToString(),
                            FlightNameR = relatedFlightsR[0].CarrierCode,
                            FlightNumberR = relatedFlightsR[0].FlightNumber,
                            FlightDepDateR = relatedFlightsR[0].DepartureDate,
                            FlightDepTimeR = relatedFlightsR[0].DepartureTime,
                            FlightArrDateR = relatedFlightsR[1].ArrivalDate,
                            FlightArrTimeR = relatedFlightsR[1].ArrivalTime,
                            SRCR = relatedFlightsR[0].Origin,
                            DESTR = relatedFlightsR[0].Destination,
                            StopR = "OneStop",
                            logoR = "/assets/img/airlogo_square/" + relatedFlightsR[0].CarrierCode + ".gif",
                            DurationR = relatedFlightsR[0].DurationDesc,
                            connectionR = 2,
                            PriceTypeR = relatedFlightsR[0].PriceType,
                            RefundTypeR = ReturnRefundType(relatedFlightsR[0]).ToString(),
                            AvailableSeatR = checkSeat(relatedFlightsR[0].SeatsAvailable.ToString()),
                            ArrivalNextDayCheckR = arrvchackR,
                            ClassR = ReturnCls(relatedFlightsR[0]).ToString(),
                            CabinR = relatedFlightsR[0].Cabin,
                            DepartStationNameR = relatedFlightsR[0].DepartureStationName,
                            ArrivalStationNameR = relatedFlightsR[1].ArrivalStationName,
                            DepartureStationAirportR = relatedFlightsR[0].DepartureStationAirport,
                            ArrivalStationAirportR = relatedFlightsR[1].ArrivalStationAirport,
                            Layover1R = relatedFlightsR[1].JourneyTimeDesc,

                            SRC1R = relatedFlightsR[0].DepartureStation,
                            DEST1R = relatedFlightsR[0].ArrivalStation,
                            ViaR = checkVia(relatedFlightsR[0].Via),
                            ViaNameR = checkViaName(relatedFlightsR[0].ViaName),
                            TerminalSRCR = checkTerminal(relatedFlightsR[0].DepartureTerminal),
                            TerminalDESTR = checkTerminal(relatedFlightsR[1].ArrivalTerminal),
                            FlightName1R = relatedFlightsR[0].CarrierCode,
                            FlightNumber1R = relatedFlightsR[0].FlightNumber,
                            FlightDepDate1R = relatedFlightsR[0].DepartureDate,
                            FlightDepTime1R = relatedFlightsR[0].DepartureTime,
                            FlightArrDate1R = relatedFlightsR[0].ArrivalDate,
                            FlightArrTime1R = relatedFlightsR[0].ArrivalTime,
                            Duration1R = relatedFlightsR[0].DurationDesc,


                            Class2R = ReturnCls(relatedFlightsR[1]).ToString(),
                            Cabin2R = relatedFlightsR[1].Cabin,
                            DepartStationName2R = relatedFlightsR[1].DepartureStationName,
                            ArrivalStationName2R = relatedFlightsR[1].ArrivalStationName,
                            DepartureStationAirport2R = relatedFlightsR[1].DepartureStationAirport,
                            ArrivalStationAirport2R = relatedFlightsR[1].ArrivalStationAirport,
                            SRC2R = relatedFlightsR[1].DepartureStation,
                            DEST2R = relatedFlightsR[1].ArrivalStation,
                            Via1R = checkVia(relatedFlightsR[1].Via),
                            ViaName1R = checkViaName(relatedFlightsR[1].ViaName),
                            TerminalSRC1R = checkTerminal(relatedFlightsR[1].DepartureTerminal),
                            TerminalDEST1R = checkTerminal(relatedFlightsR[1].ArrivalTerminal),
                            logo1R = "/assets/img/airlogo_square/" + relatedFlightsR[1].CarrierCode + ".gif",
                            FlightName2R = relatedFlightsR[1].CarrierCode,
                            FlightNumber2R = relatedFlightsR[1].FlightNumber,
                            FlightDepDate2R = relatedFlightsR[1].DepartureDate,
                            FlightDepTime2R = relatedFlightsR[1].DepartureTime,
                            FlightArrDate2R = relatedFlightsR[1].ArrivalDate,
                            FlightArrTime2R = relatedFlightsR[1].ArrivalTime,
                            Duration2R = relatedFlightsR[1].DurationDesc,

                        });
                    }
                    #endregion
                    #region
                    else if (relatedFlightsR.Count == 3)
                    {
                        string departureDateR = relatedFlightsR[0].DepartureDate.ToString(); // Assuming DepartureDate is a property of k_ShowFlightOutBound
                        string arrivalDateR = relatedFlightsR[2].ArrivalDate.ToString();
                        if (departureDateR != arrivalDateR)
                        {
                            arrvchackR = DateHelper.DayDiff(departureDateR, arrivalDateR);
                        }

                        FlightOutBound.Add(new k_ShowFlightInternational()
                        {
                            F_Status = fStatus,
                            F_Remark = fRemark,
                            AgentType = ChekA_C,
                            CompanyID = CompanyID,
                            FlightRefid = relatedFlights[0].RefID.ToString(),
                            Curr = curr,


                            NoOFAdult = relatedFlights[0].Adt,
                            NoOFChild = relatedFlights[0].Chd,
                            NoOFInfant = relatedFlights[0].Inf,

                            //==============================================================================================================================
                            Adt_BASIC = GetConvert(relatedFlights[0].Adt_BASIC.ToString()),
                            Chd_BASIC = GetConvert(relatedFlights[0].Chd_BASIC.ToString()),
                            Inf_BASIC = GetConvert(relatedFlights[0].Inf_BASIC.ToString()),

                            AdultbaseFare = relatedFlights[0].Adt * GetConvert(relatedFlights[0].Adt_BASIC),
                            ChildbaseFare = relatedFlights[0].Chd * GetConvert(relatedFlights[0].Chd_BASIC),
                            InfantbaseFare = relatedFlights[0].Inf * GetConvert(relatedFlights[0].Inf_BASIC),

                            Adt_YQ = GetConvert(relatedFlights[0].Adt_YQ),
                            AdtTotalTax = GetConvert(relatedFlights[0].AdtTotalTax),
                            Chd_YQ = GetConvert(relatedFlights[0].Chd_YQ),
                            ChdTotalTax = GetConvert(relatedFlights[0].ChdTotalTax),
                            Inf_TAX = GetConvert(relatedFlights[0].Inf_TAX),

                            Adt_AcuTax = ReturnTAXADT(relatedFlights[0]).ToString(),
                            Chd_AcuTax = ReturnTAXCHD(relatedFlights[0]).ToString(),
                            Inf_AcuTax = ReturnTAXINF(relatedFlights[0]).ToString(),

                            Yq = "Commission : " + String.Format("{0:N0}", GetConvert(relatedFlights[0].TotalCommission)),
                            PromoChek = promo,

                            TotalAmount = ReturnTotalFareAmount(relatedFlights[0]),
                            TotalFare = ReturnTotalFareAmount(relatedFlights[0]),
                            FlightAmount = ReturnTotalFareAmount(relatedFlights[0]),
                            TotalTax = GetTotalTax(relatedFlights[0]),
                            TotalBasic = GetTotalBasic(relatedFlights[0]),
                            TotalServiceFee = GetTotalServiceFee(relatedFlights[0]),
                            TotalMarkup = GetTotalMarkUp(relatedFlights[0]),
                            TotalServiceTax = GetTotalServiceTax(relatedFlights[0]),
                            TaxandCharges = GetTaxandCharges(relatedFlights[0]),
                            TotalCommission = ReturnCommission(relatedFlights[0], CompanyID),
                            TotalTds = ReturnTDS(relatedFlights[0], CompanyID),
                            FinalFare = ReturnFinalFare(relatedFlights[0], CompanyID),
                            //==============================================================================================================================

                            FlightName = relatedFlights[0].CarrierCode,
                            FlightNumber = relatedFlights[0].FlightNumber,
                            FlightDepDate = relatedFlights[0].DepartureDate,
                            FlightDepTime = relatedFlights[0].DepartureTime,
                            FlightArrDate = relatedFlights[0].ArrivalDate,
                            FlightArrTime = relatedFlights[0].ArrivalTime,
                            RuleTarrif = relatedFlights[0].RuleTarrif ?? string.Empty,
                            CHECKINBaggage = ReturnCheckINBaggage(relatedFlights[0]).ToString(),
                            CABINBaggage = ReturnCabinINBaggage(relatedFlights[0]).ToString(),
                            SRC = relatedFlights[0].Origin,
                            DEST = relatedFlights[0].Destination,
                            Stop = "NonStop",
                            logo = "/assets/img/airlogo_square/" + relatedFlights[0].CarrierCode + ".gif",
                            Duration = relatedFlights[0].DurationDesc,
                            connection = 1,
                            ArrivalNextDayCheck = arrvchack,
                            PriceType = relatedFlights[0].PriceType,
                            RefundType = ReturnRefundType(relatedFlights[0]).ToString(),
                            AvailableSeat = checkSeat(relatedFlights[0].SeatsAvailable.ToString()),
                            FareRules = rules,
                            DepartStationName = relatedFlights[0].DepartureStationName,
                            ArrivalStationName = relatedFlights[0].ArrivalStationName,
                            DepartureStationAirport = relatedFlights[0].DepartureStationAirport,
                            ArrivalStationAirport = relatedFlights[0].ArrivalStationAirport,
                            Class = ReturnCls(relatedFlights[0]),
                            Cabin = relatedFlights[0].Cabin,

                            Via = checkVia(relatedFlights[0].Via),
                            ViaName = checkViaName(relatedFlights[0].ViaName),
                            TerminalSRC = checkTerminal(relatedFlights[0].DepartureTerminal),
                            TerminalDEST = checkTerminal(relatedFlights[0].ArrivalTerminal),
                            Layover1 = relatedFlights[0].JourneyTimeDesc,


                            FlightName1 = relatedFlights[0].CarrierCode,
                            FlightNumber1 = relatedFlights[0].FlightNumber,
                            FlightDepDate1 = relatedFlights[0].DepartureDate,
                            FlightDepTime1 = relatedFlights[0].DepartureTime,
                            FlightArrDate1 = relatedFlights[0].ArrivalDate,
                            FlightArrTime1 = relatedFlights[0].ArrivalTime,
                            Duration1 = relatedFlights[0].DurationDesc,

                            FareRulesR = rulesIB,
                            CHECKINBaggageR = ReturnCheckINBaggage(relatedFlightsR[0]).ToString(),
                            CABINBaggageR = ReturnCabinINBaggage(relatedFlightsR[0]).ToString(),
                            FlightNameR = relatedFlightsR[0].CarrierCode,
                            FlightNumberR = relatedFlightsR[0].FlightNumber,
                            FlightDepDateR = relatedFlightsR[0].DepartureDate,
                            FlightDepTimeR = relatedFlightsR[0].DepartureTime,
                            FlightArrDateR = relatedFlightsR[2].ArrivalDate,
                            FlightArrTimeR = relatedFlightsR[2].ArrivalTime,
                            SRCR = relatedFlightsR[0].Origin,
                            DESTR = relatedFlightsR[0].Destination,
                            StopR = "TwoStop",
                            logoR = "/assets/img/airlogo_square/" + relatedFlightsR[0].CarrierCode + ".gif",
                            DurationR = relatedFlightsR[0].DurationDesc,
                            connectionR = 3,
                            PriceTypeR = relatedFlightsR[0].PriceType,
                            RefundTypeR = ReturnRefundType(relatedFlightsR[0]).ToString(),
                            AvailableSeatR = checkSeat(relatedFlightsR[0].SeatsAvailable.ToString()),
                            ArrivalNextDayCheckR = arrvchackR,
                            ClassR = ReturnCls(relatedFlightsR[0]).ToString(),
                            CabinR = relatedFlightsR[0].Cabin,
                            DepartStationNameR = relatedFlightsR[0].DepartureStationName,
                            ArrivalStationNameR = relatedFlightsR[2].ArrivalStationName,
                            DepartureStationAirportR = relatedFlightsR[0].DepartureStationAirport,
                            ArrivalStationAirportR = relatedFlightsR[2].ArrivalStationAirport,
                            Layover1R = relatedFlightsR[1].JourneyTimeDesc,

                            SRC1R = relatedFlightsR[0].DepartureStation,
                            DEST1R = relatedFlightsR[0].ArrivalStation,
                            ViaR = checkVia(relatedFlightsR[0].Via),
                            ViaNameR = checkViaName(relatedFlightsR[0].ViaName),
                            TerminalSRCR = checkTerminal(relatedFlightsR[0].DepartureTerminal),
                            TerminalDESTR = checkTerminal(relatedFlightsR[2].ArrivalTerminal),
                            FlightName1R = relatedFlightsR[0].CarrierCode,
                            FlightNumber1R = relatedFlightsR[0].FlightNumber,
                            FlightDepDate1R = relatedFlightsR[0].DepartureDate,
                            FlightDepTime1R = relatedFlightsR[0].DepartureTime,
                            FlightArrDate1R = relatedFlightsR[0].ArrivalDate,
                            FlightArrTime1R = relatedFlightsR[0].ArrivalTime,
                            Duration1R = relatedFlightsR[0].DurationDesc,

                            Class2R = ReturnCls(relatedFlightsR[1]).ToString(),
                            Cabin2R = relatedFlightsR[1].Cabin,
                            DepartStationName2R = relatedFlightsR[1].DepartureStationName,
                            ArrivalStationName2R = relatedFlightsR[1].ArrivalStationName,
                            DepartureStationAirport2R = relatedFlightsR[1].DepartureStationAirport,
                            ArrivalStationAirport2R = relatedFlightsR[1].ArrivalStationAirport,
                            SRC2R = relatedFlightsR[1].DepartureStation,
                            DEST2R = relatedFlightsR[1].ArrivalStation,
                            Via1R = checkVia(relatedFlightsR[1].Via),
                            ViaName1R = checkViaName(relatedFlightsR[1].ViaName),
                            TerminalSRC1R = checkTerminal(relatedFlightsR[1].DepartureTerminal),
                            TerminalDEST1R = checkTerminal(relatedFlightsR[1].ArrivalTerminal),
                            logo1R = "/assets/img/airlogo_square/" + relatedFlightsR[1].CarrierCode + ".gif",
                            FlightName2R = relatedFlightsR[1].CarrierCode,
                            FlightNumber2R = relatedFlightsR[1].FlightNumber,
                            FlightDepDate2R = relatedFlightsR[1].DepartureDate,
                            FlightDepTime2R = relatedFlightsR[1].DepartureTime,
                            FlightArrDate2R = relatedFlightsR[1].ArrivalDate,
                            FlightArrTime2R = relatedFlightsR[1].ArrivalTime,
                            Duration2R = relatedFlightsR[1].DurationDesc,

                            Class3R = ReturnCls(relatedFlightsR[2]).ToString(),
                            Cabin3R = relatedFlightsR[2].Cabin,
                            DepartStationName3R = relatedFlightsR[2].DepartureStationName,
                            ArrivalStationName3R = relatedFlightsR[2].ArrivalStationName,
                            DepartureStationAirport3R = relatedFlightsR[2].DepartureStationAirport,
                            ArrivalStationAirport3R = relatedFlightsR[2].ArrivalStationAirport,
                            SRC3R = relatedFlightsR[2].DepartureStation,
                            DEST3R = relatedFlightsR[2].ArrivalStation,
                            Via2R = checkVia(relatedFlightsR[2].Via),
                            ViaName2R = checkViaName(relatedFlightsR[2].ViaName),
                            TerminalSRC2R = checkTerminal(relatedFlightsR[2].DepartureTerminal),
                            TerminalDEST2R = checkTerminal(relatedFlightsR[2].ArrivalTerminal),
                            logo2R = "/assets/img/airlogo_square/" + relatedFlightsR[2].CarrierCode + ".gif",
                            FlightName3R = relatedFlightsR[2].CarrierCode,
                            FlightNumber3R = relatedFlightsR[2].FlightNumber,
                            FlightDepDate3R = relatedFlightsR[2].DepartureDate,
                            FlightDepTime3R = relatedFlightsR[2].DepartureTime,
                            FlightArrDate3R = relatedFlightsR[2].ArrivalDate,
                            FlightArrTime3R = relatedFlightsR[2].ArrivalTime,
                            Duration3R = relatedFlightsR[2].DurationDesc,
                            Layover2R = relatedFlightsR[2].JourneyTimeDesc

                        });
                    }
                    #endregion
                }
                catch (Exception ex)
                {

                }
            }
            #endregion
            #region
            else if (relatedFlights.Count == 2)
            {
                try
                {
                    rules = GetFareRules(relatedFlights);
                    rulesIB = GetFareRules(relatedFlightsR);
                   
                    string departureDate = relatedFlights[0].DepartureDate.ToString(); // Assuming DepartureDate is a property of k_ShowFlightOutBound
                    string arrivalDate = relatedFlights[1].ArrivalDate.ToString();
                    if (departureDate != arrivalDate)
                    {
                        arrvchack = DateHelper.DayDiff(departureDate, arrivalDate);
                    }
                    #region
                    if (relatedFlightsR.Count == 1)
                    {
                        string departureDateR = relatedFlightsR[0].DepartureDate.ToString(); // Assuming DepartureDate is a property of k_ShowFlightOutBound
                        string arrivalDateR = relatedFlightsR[0].ArrivalDate.ToString();
                        if (departureDateR != arrivalDateR)
                        {
                            arrvchackR = DateHelper.DayDiff(departureDateR, arrivalDateR);
                        }

                        FlightOutBound.Add(new k_ShowFlightInternational()
                        {
                            F_Status = fStatus,
                            F_Remark = fRemark,
                            AgentType = ChekA_C,
                            CompanyID = CompanyID,
                            FlightRefid = relatedFlights[0].RefID.ToString(),
                            Curr = curr,

                            NoOFAdult = relatedFlights[0].Adt,
                            NoOFChild = relatedFlights[0].Chd,
                            NoOFInfant = relatedFlights[0].Inf,

                            //==============================================================================================================================
                            Adt_BASIC = GetConvert(relatedFlights[0].Adt_BASIC.ToString()),
                            Chd_BASIC = GetConvert(relatedFlights[0].Chd_BASIC.ToString()),
                            Inf_BASIC = GetConvert(relatedFlights[0].Inf_BASIC.ToString()),

                            AdultbaseFare = relatedFlights[0].Adt * GetConvert(relatedFlights[0].Adt_BASIC),
                            ChildbaseFare = relatedFlights[0].Chd * GetConvert(relatedFlights[0].Chd_BASIC),
                            InfantbaseFare = relatedFlights[0].Inf * GetConvert(relatedFlights[0].Inf_BASIC),

                            Adt_YQ = GetConvert(relatedFlights[0].Adt_YQ),
                            AdtTotalTax = GetConvert(relatedFlights[0].AdtTotalTax),
                            Chd_YQ = GetConvert(relatedFlights[0].Chd_YQ),
                            ChdTotalTax = GetConvert(relatedFlights[0].ChdTotalTax),
                            Inf_TAX = GetConvert(relatedFlights[0].Inf_TAX),

                            Adt_AcuTax = ReturnTAXADT(relatedFlights[0]).ToString(),
                            Chd_AcuTax = ReturnTAXCHD(relatedFlights[0]).ToString(),
                            Inf_AcuTax = ReturnTAXINF(relatedFlights[0]).ToString(),

                            Yq = "Commission : " + String.Format("{0:N0}", GetConvert(relatedFlights[0].TotalCommission)),
                            PromoChek = promo,


                            //==============================================================================================================================

                            TotalAmount = ReturnTotalFareAmount(relatedFlights[0]),
                            TotalFare = ReturnTotalFareAmount(relatedFlights[0]),
                            FlightAmount = ReturnTotalFareAmount(relatedFlights[0]),
                            TotalTax = GetTotalTax(relatedFlights[0]),
                            TotalBasic = GetTotalBasic(relatedFlights[0]),
                            TotalServiceFee = GetTotalServiceFee(relatedFlights[0]),
                            TotalMarkup = GetTotalMarkUp(relatedFlights[0]),
                            TotalServiceTax = GetTotalServiceTax(relatedFlights[0]),
                            TaxandCharges = GetTaxandCharges(relatedFlights[0]),
                            TotalCommission = ReturnCommission(relatedFlights[0], CompanyID),
                            TotalTds = ReturnTDS(relatedFlights[0], CompanyID),
                            FinalFare = ReturnFinalFare(relatedFlights[0], CompanyID),

                            //==============================================================================================================================

                            RuleTarrif = relatedFlights[0].RuleTarrif ?? string.Empty,
                            FlightName = relatedFlights[0].CarrierCode,
                            FlightNumber = relatedFlights[0].FlightNumber,
                            FlightDepDate = relatedFlights[0].DepartureDate,
                            FlightDepTime = relatedFlights[0].DepartureTime,
                            FlightArrDate = relatedFlights[1].ArrivalDate,
                            FlightArrTime = relatedFlights[1].ArrivalTime,
                            CHECKINBaggage = ReturnCheckINBaggage(relatedFlights[0]).ToString(),
                            CABINBaggage = ReturnCabinINBaggage(relatedFlights[0]).ToString(),
                            SRC = relatedFlights[0].Origin,
                            DEST = relatedFlights[0].Destination,
                            logo = "/assets/img/airlogo_square/" + relatedFlights[0].CarrierCode + ".gif",
                            Duration = relatedFlights[0].DurationDesc,
                            connection = 2,
                            ArrivalNextDayCheck = arrvchack,
                            PriceType = relatedFlights[0].PriceType,
                            RefundType = ReturnRefundType(relatedFlights[0]).ToString(),
                            AvailableSeat = checkSeat(relatedFlights[0].SeatsAvailable.ToString()),
                            FareRules = rules,
                            DepartStationName = relatedFlights[0].DepartureStationName,
                            ArrivalStationName = relatedFlights[1].ArrivalStationName,
                            DepartureStationAirport = relatedFlights[0].DepartureStationAirport,
                            ArrivalStationAirport = relatedFlights[1].ArrivalStationAirport,
                            Class = ReturnCls(relatedFlights[0]),
                            Cabin = relatedFlights[0].Cabin,
                            Layover1 = relatedFlights[1].JourneyTimeDesc,
                            SRC1 = relatedFlights[0].DepartureStation,
                            DEST1 = relatedFlights[0].ArrivalStation,
                            Stop = "OneStop",
                            Via = checkVia(relatedFlights[0].Via),
                            ViaName = checkViaName(relatedFlights[0].ViaName),
                            TerminalSRC = checkTerminal(relatedFlights[0].DepartureTerminal),
                            TerminalDEST = checkTerminal(relatedFlights[1].ArrivalTerminal),
                            FlightName1 = relatedFlights[0].CarrierCode,
                            FlightNumber1 = relatedFlights[0].FlightNumber,
                            FlightDepDate1 = relatedFlights[0].DepartureDate,
                            FlightDepTime1 = relatedFlights[0].DepartureTime,
                            FlightArrDate1 = relatedFlights[0].ArrivalDate,
                            FlightArrTime1 = relatedFlights[0].ArrivalTime,
                            Duration1 = relatedFlights[0].DurationDesc,

                            DepartStationName2 = relatedFlights[1].DepartureStationName,
                            ArrivalStationName2 = relatedFlights[1].ArrivalStationName,
                            DepartureStationAirport2 = relatedFlights[1].DepartureStationAirport,
                            ArrivalStationAirport2 = relatedFlights[1].ArrivalStationAirport,
                            Class2 = ReturnCls(relatedFlights[1]),
                            Cabin2 = relatedFlights[1].Cabin,
                            SRC2 = relatedFlights[1].DepartureStation,
                            DEST2 = relatedFlights[1].ArrivalStation,
                            Via1 = checkVia(relatedFlights[1].Via),
                            ViaName1 = checkViaName(relatedFlights[1].ViaName),
                            TerminalSRC1 = checkTerminal(relatedFlights[1].DepartureTerminal),
                            TerminalDEST1 = checkTerminal(relatedFlights[1].ArrivalTerminal),
                            logo1 = "/assets/img/airlogo_square/" + relatedFlights[1].CarrierCode + ".gif",
                            FlightName2 = relatedFlights[1].CarrierCode,
                            FlightNumber2 = relatedFlights[1].FlightNumber,
                            FlightDepDate2 = relatedFlights[1].DepartureDate,
                            FlightDepTime2 = relatedFlights[1].DepartureTime,
                            FlightArrDate2 = relatedFlights[1].ArrivalDate,
                            FlightArrTime2 = relatedFlights[1].ArrivalTime,
                            Duration2 = relatedFlights[1].DurationDesc,



                            CHECKINBaggageR = ReturnCheckINBaggage(relatedFlightsR[0]).ToString(),
                            CABINBaggageR = ReturnCabinINBaggage(relatedFlightsR[0]).ToString(),
                            FareRulesR = rulesIB,
                            FlightNameR = relatedFlightsR[0].CarrierCode,
                            FlightNumberR = relatedFlightsR[0].FlightNumber,
                            FlightDepDateR = relatedFlightsR[0].DepartureDate,
                            FlightDepTimeR = relatedFlightsR[0].DepartureTime,
                            FlightArrDateR = relatedFlightsR[0].ArrivalDate,
                            FlightArrTimeR = relatedFlightsR[0].ArrivalTime,
                            SRCR = relatedFlightsR[0].Origin,
                            DESTR = relatedFlightsR[0].Destination,
                            StopR = "NonStop",
                            logoR = "/assets/img/airlogo_square/" + relatedFlightsR[0].CarrierCode + ".gif",
                            DurationR = relatedFlightsR[0].DurationDesc,
                            connectionR = 1,
                            PriceTypeR = relatedFlightsR[0].PriceType,
                            RefundTypeR = ReturnRefundType(relatedFlightsR[0]).ToString(),
                            AvailableSeatR = checkSeat(relatedFlightsR[0].SeatsAvailable.ToString()),
                            ArrivalNextDayCheckR = arrvchackR,
                            DepartStationNameR = relatedFlightsR[0].DepartureStationName,
                            ArrivalStationNameR = relatedFlightsR[0].ArrivalStationName,
                            DepartureStationAirportR = relatedFlightsR[0].DepartureStationAirport,
                            ArrivalStationAirportR = relatedFlightsR[0].ArrivalStationAirport,
                            ClassR = ReturnCls(relatedFlightsR[0]).ToString(),
                            CabinR = relatedFlightsR[0].Cabin,
                            Layover1R = relatedFlightsR[0].JourneyTimeDesc,

                            SRC1R = relatedFlightsR[0].DepartureStation,
                            DEST1R = relatedFlightsR[0].ArrivalStation,
                            ViaR = checkVia(relatedFlightsR[0].Via),
                            ViaNameR = checkViaName(relatedFlightsR[0].ViaName),
                            TerminalSRCR = checkTerminal(relatedFlightsR[0].DepartureTerminal),
                            TerminalDESTR = checkTerminal(relatedFlightsR[0].ArrivalTerminal),
                            FlightName1R = relatedFlightsR[0].CarrierCode,
                            FlightNumber1R = relatedFlightsR[0].FlightNumber,
                            FlightDepDate1R = relatedFlightsR[0].DepartureDate,
                            FlightDepTime1R = relatedFlightsR[0].DepartureTime,
                            FlightArrDate1R = relatedFlightsR[0].ArrivalDate,
                            FlightArrTime1R = relatedFlightsR[0].ArrivalTime,
                            Duration1R = relatedFlightsR[0].DurationDesc

                        });
                    }
                    #endregion
                    #region
                    else if (relatedFlightsR.Count == 2)
                    {

                        string departureDateR = relatedFlightsR[0].DepartureDate.ToString(); // Assuming DepartureDate is a property of k_ShowFlightOutBound
                        string arrivalDateR = relatedFlightsR[1].ArrivalDate.ToString();
                        if (departureDateR != arrivalDateR)
                        {
                            arrvchackR = DateHelper.DayDiff(departureDateR, arrivalDateR);
                        }

                        FlightOutBound.Add(new k_ShowFlightInternational()
                        {
                            F_Status = fStatus,
                            F_Remark = fRemark,
                            AgentType = ChekA_C,
                            CompanyID = CompanyID,
                            FlightRefid = relatedFlights[0].RefID.ToString(),
                            Curr = curr,
                            NoOFAdult = relatedFlights[0].Adt,
                            NoOFChild = relatedFlights[0].Chd,
                            NoOFInfant = relatedFlights[0].Inf,

                            //==============================================================================================================================
                            Adt_BASIC = GetConvert(relatedFlights[0].Adt_BASIC.ToString()),
                            Chd_BASIC = GetConvert(relatedFlights[0].Chd_BASIC.ToString()),
                            Inf_BASIC = GetConvert(relatedFlights[0].Inf_BASIC.ToString()),

                            AdultbaseFare = relatedFlights[0].Adt * GetConvert(relatedFlights[0].Adt_BASIC),
                            ChildbaseFare = relatedFlights[0].Chd * GetConvert(relatedFlights[0].Chd_BASIC),
                            InfantbaseFare = relatedFlights[0].Inf * GetConvert(relatedFlights[0].Inf_BASIC),

                            Adt_YQ = GetConvert(relatedFlights[0].Adt_YQ),
                            AdtTotalTax = GetConvert(relatedFlights[0].AdtTotalTax),
                            Chd_YQ = GetConvert(relatedFlights[0].Chd_YQ),
                            ChdTotalTax = GetConvert(relatedFlights[0].ChdTotalTax),
                            Inf_TAX = GetConvert(relatedFlights[0].Inf_TAX),

                            Adt_AcuTax = ReturnTAXADT(relatedFlights[0]).ToString(),
                            Chd_AcuTax = ReturnTAXCHD(relatedFlights[0]).ToString(),
                            Inf_AcuTax = ReturnTAXINF(relatedFlights[0]).ToString(),

                            Yq = "Commission : " + String.Format("{0:N0}", GetConvert(relatedFlights[0].TotalCommission)),
                            PromoChek = promo,


                            TotalAmount = ReturnTotalFareAmount(relatedFlights[0]),
                            TotalFare = ReturnTotalFareAmount(relatedFlights[0]),
                            FlightAmount = ReturnTotalFareAmount(relatedFlights[0]),
                            TotalTax = GetTotalTax(relatedFlights[0]),
                            TotalBasic = GetTotalBasic(relatedFlights[0]),
                            TotalServiceFee = GetTotalServiceFee(relatedFlights[0]),
                            TotalMarkup = GetTotalMarkUp(relatedFlights[0]),
                            TotalServiceTax = GetTotalServiceTax(relatedFlights[0]),
                            TaxandCharges = GetTaxandCharges(relatedFlights[0]),
                            TotalCommission = ReturnCommission(relatedFlights[0], CompanyID),
                            TotalTds = ReturnTDS(relatedFlights[0], CompanyID),
                            FinalFare = ReturnFinalFare(relatedFlights[0], CompanyID),
                            //==============================================================================================================================


                            FlightName = relatedFlights[0].CarrierCode,
                            FlightNumber = relatedFlights[0].FlightNumber,
                            FlightDepDate = relatedFlights[0].DepartureDate,
                            FlightDepTime = relatedFlights[0].DepartureTime,
                            FlightArrDate = relatedFlights[1].ArrivalDate,
                            FlightArrTime = relatedFlights[1].ArrivalTime,
                            RuleTarrif = relatedFlights[0].RuleTarrif ?? string.Empty,
                            CHECKINBaggage = ReturnCheckINBaggage(relatedFlights[0]).ToString(),
                            CABINBaggage = ReturnCabinINBaggage(relatedFlights[0]).ToString(),
                            SRC = relatedFlights[0].Origin,
                            DEST = relatedFlights[0].Destination,
                            logo = "/assets/img/airlogo_square/" + relatedFlights[0].CarrierCode + ".gif",
                            Duration = relatedFlights[0].DurationDesc,
                            connection = 2,
                            ArrivalNextDayCheck = arrvchack,
                            PriceType = relatedFlights[0].PriceType,
                            RefundType = ReturnRefundType(relatedFlights[0]).ToString(),
                            AvailableSeat = checkSeat(relatedFlights[0].SeatsAvailable.ToString()),
                            FareRules = rules,
                            DepartStationName = relatedFlights[0].DepartureStationName,
                            ArrivalStationName = relatedFlights[1].ArrivalStationName,
                            DepartureStationAirport = relatedFlights[0].DepartureStationAirport,
                            ArrivalStationAirport = relatedFlights[1].ArrivalStationAirport,
                            Class = ReturnCls(relatedFlights[0]),
                            Cabin = relatedFlights[0].Cabin,
                            Layover1 = relatedFlights[1].JourneyTimeDesc,

                            SRC1 = relatedFlights[0].DepartureStation,
                            DEST1 = relatedFlights[0].ArrivalStation,
                            Stop = "OneStop",
                            Via = checkVia(relatedFlights[0].Via),
                            ViaName = checkViaName(relatedFlights[0].ViaName),
                            TerminalSRC = checkTerminal(relatedFlights[0].DepartureTerminal),
                            TerminalDEST = checkTerminal(relatedFlights[1].ArrivalTerminal),
                            FlightName1 = relatedFlights[0].CarrierCode,
                            FlightNumber1 = relatedFlights[0].FlightNumber,
                            FlightDepDate1 = relatedFlights[0].DepartureDate,
                            FlightDepTime1 = relatedFlights[0].DepartureTime,
                            FlightArrDate1 = relatedFlights[0].ArrivalDate,
                            FlightArrTime1 = relatedFlights[0].ArrivalTime,
                            Duration1 = relatedFlights[0].DurationDesc,

                            DepartStationName2 = relatedFlights[1].DepartureStationName,
                            ArrivalStationName2 = relatedFlights[1].ArrivalStationName,
                            DepartureStationAirport2 = relatedFlights[1].DepartureStationAirport,
                            ArrivalStationAirport2 = relatedFlights[1].ArrivalStationAirport,
                            Class2 = ReturnCls(relatedFlights[1]),
                            Cabin2 = relatedFlights[1].Cabin,
                            SRC2 = relatedFlights[1].DepartureStation,
                            DEST2 = relatedFlights[1].ArrivalStation,
                            Via1 = checkVia(relatedFlights[1].Via),
                            ViaName1 = checkViaName(relatedFlights[1].ViaName),
                            TerminalSRC1 = checkTerminal(relatedFlights[1].DepartureTerminal),
                            TerminalDEST1 = checkTerminal(relatedFlights[1].ArrivalTerminal),
                            logo1 = "/assets/img/airlogo_square/" + relatedFlights[1].CarrierCode + ".gif",
                            FlightName2 = relatedFlights[1].CarrierCode,
                            FlightNumber2 = relatedFlights[1].FlightNumber,
                            FlightDepDate2 = relatedFlights[1].DepartureDate,
                            FlightDepTime2 = relatedFlights[1].DepartureTime,
                            FlightArrDate2 = relatedFlights[1].ArrivalDate,
                            FlightArrTime2 = relatedFlights[1].ArrivalTime,
                            Duration2 = relatedFlights[1].DurationDesc,


                            CHECKINBaggageR = ReturnCheckINBaggage(relatedFlightsR[0]).ToString(),
                            CABINBaggageR = ReturnCabinINBaggage(relatedFlightsR[0]).ToString(),
                            FareRulesR = rulesIB,
                            FlightNameR = relatedFlightsR[0].CarrierCode,
                            FlightNumberR = relatedFlightsR[0].FlightNumber,
                            FlightDepDateR = relatedFlightsR[0].DepartureDate,
                            FlightDepTimeR = relatedFlightsR[0].DepartureTime,
                            FlightArrDateR = relatedFlightsR[1].ArrivalDate,
                            FlightArrTimeR = relatedFlightsR[1].ArrivalTime,
                            SRCR = relatedFlightsR[0].Origin,
                            DESTR = relatedFlightsR[0].Destination,
                            StopR = "OneStop",
                            logoR = "/assets/img/airlogo_square/" + relatedFlightsR[0].CarrierCode + ".gif",
                            DurationR = relatedFlightsR[0].DurationDesc,
                            connectionR = 2,
                            PriceTypeR = relatedFlightsR[0].PriceType,
                            RefundTypeR = ReturnRefundType(relatedFlightsR[0]).ToString(),
                            AvailableSeatR = checkSeat(relatedFlightsR[0].SeatsAvailable.ToString()),
                            ArrivalNextDayCheckR = arrvchackR,
                            DepartStationNameR = relatedFlightsR[0].DepartureStationName,
                            ArrivalStationNameR = relatedFlightsR[1].ArrivalStationName,
                            DepartureStationAirportR = relatedFlightsR[0].DepartureStationAirport,
                            ArrivalStationAirportR = relatedFlightsR[1].ArrivalStationAirport,
                            ClassR = ReturnCls(relatedFlightsR[0]).ToString(),
                            CabinR = relatedFlightsR[0].Cabin,
                            Layover1R = relatedFlightsR[1].JourneyTimeDesc,

                            SRC1R = relatedFlightsR[0].DepartureStation,
                            DEST1R = relatedFlightsR[0].ArrivalStation,
                            ViaR = checkVia(relatedFlightsR[0].Via),
                            ViaNameR = checkViaName(relatedFlightsR[0].ViaName),
                            TerminalSRCR = checkTerminal(relatedFlightsR[0].DepartureTerminal),
                            TerminalDESTR = checkTerminal(relatedFlightsR[1].ArrivalTerminal),
                            FlightName1R = relatedFlightsR[0].CarrierCode,
                            FlightNumber1R = relatedFlightsR[0].FlightNumber,
                            FlightDepDate1R = relatedFlightsR[0].DepartureDate,
                            FlightDepTime1R = relatedFlightsR[0].DepartureTime,
                            FlightArrDate1R = relatedFlightsR[0].ArrivalDate,
                            FlightArrTime1R = relatedFlightsR[0].ArrivalTime,
                            Duration1R = relatedFlightsR[0].DurationDesc,

                            Class2R = ReturnCls(relatedFlightsR[1]).ToString(),
                            Cabin2R = relatedFlightsR[1].Cabin,
                            DepartStationName2R = relatedFlightsR[1].DepartureStationName,
                            ArrivalStationName2R = relatedFlightsR[1].ArrivalStationName,
                            DepartureStationAirport2R = relatedFlightsR[1].DepartureStationAirport,
                            ArrivalStationAirport2R = relatedFlightsR[1].ArrivalStationAirport,
                            SRC2R = relatedFlightsR[1].DepartureStation,
                            DEST2R = relatedFlightsR[1].ArrivalStation,
                            Via1R = checkVia(relatedFlightsR[1].Via),
                            ViaName1R = checkViaName(relatedFlightsR[1].ViaName),
                            TerminalSRC1R = checkTerminal(relatedFlightsR[1].DepartureTerminal),
                            TerminalDEST1R = checkTerminal(relatedFlightsR[1].ArrivalTerminal),
                            logo1R = "/assets/img/airlogo_square/" + relatedFlightsR[1].CarrierCode + ".gif",
                            FlightName2R = relatedFlightsR[1].CarrierCode,
                            FlightNumber2R = relatedFlightsR[1].FlightNumber,
                            FlightDepDate2R = relatedFlightsR[1].DepartureDate,
                            FlightDepTime2R = relatedFlightsR[1].DepartureTime,
                            FlightArrDate2R = relatedFlightsR[1].ArrivalDate,
                            FlightArrTime2R = relatedFlightsR[1].ArrivalTime,
                            Duration2R = relatedFlightsR[1].DurationDesc,

                        });
                    }
                    #endregion
                    #region
                    else if (relatedFlightsR.Count == 3)
                    {
                        
                        string departureDateR = relatedFlightsR[0].DepartureDate.ToString(); // Assuming DepartureDate is a property of k_ShowFlightOutBound
                        string arrivalDateR = relatedFlightsR[2].ArrivalDate.ToString();
                        if (departureDateR != arrivalDateR)
                        {
                            arrvchackR = DateHelper.DayDiff(departureDateR, arrivalDateR);
                        }

                        FlightOutBound.Add(new k_ShowFlightInternational()
                        {
                            F_Status = fStatus,
                            F_Remark = fRemark,
                            AgentType = ChekA_C,
                            CompanyID = CompanyID,
                            FlightRefid = relatedFlights[0].RefID.ToString(),
                            Curr = curr,


                            NoOFAdult = relatedFlights[0].Adt,
                            NoOFChild = relatedFlights[0].Chd,
                            NoOFInfant = relatedFlights[0].Inf,

                            //==============================================================================================================================
                            Adt_BASIC = GetConvert(relatedFlights[0].Adt_BASIC.ToString()),
                            Chd_BASIC = GetConvert(relatedFlights[0].Chd_BASIC.ToString()),
                            Inf_BASIC = GetConvert(relatedFlights[0].Inf_BASIC.ToString()),

                            AdultbaseFare = relatedFlights[0].Adt * GetConvert(relatedFlights[0].Adt_BASIC),
                            ChildbaseFare = relatedFlights[0].Chd * GetConvert(relatedFlights[0].Chd_BASIC),
                            InfantbaseFare = relatedFlights[0].Inf * GetConvert(relatedFlights[0].Inf_BASIC),

                            Adt_YQ = GetConvert(relatedFlights[0].Adt_YQ),
                            AdtTotalTax = GetConvert(relatedFlights[0].AdtTotalTax),
                            Chd_YQ = GetConvert(relatedFlights[0].Chd_YQ),
                            ChdTotalTax = GetConvert(relatedFlights[0].ChdTotalTax),
                            Inf_TAX = GetConvert(relatedFlights[0].Inf_TAX),

                            Adt_AcuTax = ReturnTAXADT(relatedFlights[0]).ToString(),
                            Chd_AcuTax = ReturnTAXCHD(relatedFlights[0]).ToString(),
                            Inf_AcuTax = ReturnTAXINF(relatedFlights[0]).ToString(),

                            Yq = "Commission : " + String.Format("{0:N0}", GetConvert(relatedFlights[0].TotalCommission)),
                            PromoChek = promo,

                            TotalAmount = ReturnTotalFareAmount(relatedFlights[0]),
                            TotalFare = ReturnTotalFareAmount(relatedFlights[0]),
                            FlightAmount = ReturnTotalFareAmount(relatedFlights[0]),
                            TotalTax = GetTotalTax(relatedFlights[0]),
                            TotalBasic = GetTotalBasic(relatedFlights[0]),
                            TotalServiceFee = GetTotalServiceFee(relatedFlights[0]),
                            TotalMarkup = GetTotalMarkUp(relatedFlights[0]),
                            TotalServiceTax = GetTotalServiceTax(relatedFlights[0]),
                            TaxandCharges = GetTaxandCharges(relatedFlights[0]),
                            TotalCommission = ReturnCommission(relatedFlights[0], CompanyID),
                            TotalTds = ReturnTDS(relatedFlights[0], CompanyID),
                            FinalFare = ReturnFinalFare(relatedFlights[0], CompanyID),
                            //==============================================================================================================================

                            FlightName = relatedFlights[0].CarrierCode,
                            FlightNumber = relatedFlights[0].FlightNumber,
                            FlightDepDate = relatedFlights[0].DepartureDate,
                            FlightDepTime = relatedFlights[0].DepartureTime,
                            FlightArrDate = relatedFlights[1].ArrivalDate,
                            FlightArrTime = relatedFlights[1].ArrivalTime,
                            RuleTarrif = relatedFlights[0].RuleTarrif ?? string.Empty,
                            CHECKINBaggage = ReturnCheckINBaggage(relatedFlights[0]).ToString(),
                            CABINBaggage = ReturnCabinINBaggage(relatedFlights[0]).ToString(),
                            SRC = relatedFlights[0].Origin,
                            DEST = relatedFlights[0].Destination,
                            logo = "/assets/img/airlogo_square/" + relatedFlights[0].CarrierCode + ".gif",
                            Duration = relatedFlights[0].DurationDesc,
                            connection = 2,
                            ArrivalNextDayCheck = arrvchack,
                            PriceType = relatedFlights[0].PriceType,
                            RefundType = ReturnRefundType(relatedFlights[0]).ToString(),
                            AvailableSeat = checkSeat(relatedFlights[0].SeatsAvailable.ToString()),
                            FareRules = rules,
                            DepartStationName = relatedFlights[0].DepartureStationName,
                            ArrivalStationName = relatedFlights[1].ArrivalStationName,
                            DepartureStationAirport = relatedFlights[0].DepartureStationAirport,
                            ArrivalStationAirport = relatedFlights[1].ArrivalStationAirport,
                            Class = ReturnCls(relatedFlights[0]),
                            Cabin = relatedFlights[0].Cabin,
                            Layover1 = relatedFlights[1].JourneyTimeDesc,

                            SRC1 = relatedFlights[0].DepartureStation,
                            DEST1 = relatedFlights[0].ArrivalStation,
                            Stop = "OneStop",
                            Via = checkVia(relatedFlights[0].Via),
                            ViaName = checkViaName(relatedFlights[0].ViaName),
                            TerminalSRC = checkTerminal(relatedFlights[0].DepartureTerminal),
                            TerminalDEST = checkTerminal(relatedFlights[1].ArrivalTerminal),
                            FlightName1 = relatedFlights[0].CarrierCode,
                            FlightNumber1 = relatedFlights[0].FlightNumber,
                            FlightDepDate1 = relatedFlights[0].DepartureDate,
                            FlightDepTime1 = relatedFlights[0].DepartureTime,
                            FlightArrDate1 = relatedFlights[0].ArrivalDate,
                            FlightArrTime1 = relatedFlights[0].ArrivalTime,
                            Duration1 = relatedFlights[0].DurationDesc,

                            DepartStationName2 = relatedFlights[1].DepartureStationName,
                            ArrivalStationName2 = relatedFlights[1].ArrivalStationName,
                            DepartureStationAirport2 = relatedFlights[1].DepartureStationAirport,
                            ArrivalStationAirport2 = relatedFlights[1].ArrivalStationAirport,
                            Class2 = ReturnCls(relatedFlights[1]),
                            Cabin2 = relatedFlights[1].Cabin,
                            SRC2 = relatedFlights[1].DepartureStation,
                            DEST2 = relatedFlights[1].ArrivalStation,
                            Via1 = checkVia(relatedFlights[1].Via),
                            ViaName1 = checkViaName(relatedFlights[1].ViaName),
                            TerminalSRC1 = checkTerminal(relatedFlights[1].DepartureTerminal),
                            TerminalDEST1 = checkTerminal(relatedFlights[1].ArrivalTerminal),
                            logo1 = "/assets/img/airlogo_square/" + relatedFlights[1].CarrierCode + ".gif",
                            FlightName2 = relatedFlights[1].CarrierCode,
                            FlightNumber2 = relatedFlights[1].FlightNumber,
                            FlightDepDate2 = relatedFlights[1].DepartureDate,
                            FlightDepTime2 = relatedFlights[1].DepartureTime,
                            FlightArrDate2 = relatedFlights[1].ArrivalDate,
                            FlightArrTime2 = relatedFlights[1].ArrivalTime,
                            Duration2 = relatedFlights[1].DurationDesc,

                            CHECKINBaggageR = ReturnCheckINBaggage(relatedFlightsR[0]).ToString(),
                            CABINBaggageR = ReturnCabinINBaggage(relatedFlightsR[0]).ToString(),
                            FareRulesR = rulesIB,
                            FlightNameR = relatedFlightsR[0].CarrierCode,
                            FlightNumberR = relatedFlightsR[0].FlightNumber,
                            FlightDepDateR = relatedFlightsR[0].DepartureDate,
                            FlightDepTimeR = relatedFlightsR[0].DepartureTime,
                            FlightArrDateR = relatedFlightsR[2].ArrivalDate,
                            FlightArrTimeR = relatedFlightsR[2].ArrivalTime,
                            SRCR = relatedFlightsR[0].Origin,
                            DESTR = relatedFlightsR[0].Destination,
                            StopR = "TwoStop",
                            logoR = "/assets/img/airlogo_square/" + relatedFlightsR[0].CarrierCode + ".gif",
                            DurationR = relatedFlightsR[0].DurationDesc,
                            connectionR = 3,
                            PriceTypeR = relatedFlightsR[0].PriceType,
                            RefundTypeR = ReturnRefundType(relatedFlightsR[0]).ToString(),
                            AvailableSeatR = checkSeat(relatedFlightsR[0].SeatsAvailable.ToString()),
                            ArrivalNextDayCheckR = arrvchackR,
                            DepartStationNameR = relatedFlightsR[0].DepartureStationName,
                            ArrivalStationNameR = relatedFlightsR[2].ArrivalStationName,
                            DepartureStationAirportR = relatedFlightsR[0].DepartureStationAirport,
                            ArrivalStationAirportR = relatedFlightsR[2].ArrivalStationAirport,
                            ClassR = ReturnCls(relatedFlightsR[0]).ToString(),
                            CabinR = relatedFlightsR[0].Cabin,
                            Layover1R = relatedFlightsR[2].JourneyTimeDesc,

                            SRC1R = relatedFlightsR[0].DepartureStation,
                            DEST1R = relatedFlightsR[0].ArrivalStation,
                            ViaR = checkVia(relatedFlightsR[0].Via),
                            ViaNameR = checkViaName(relatedFlightsR[0].ViaName),
                            TerminalSRCR = checkTerminal(relatedFlightsR[0].DepartureTerminal),
                            TerminalDESTR = checkTerminal(relatedFlightsR[2].ArrivalTerminal),
                            FlightName1R = relatedFlightsR[0].CarrierCode,
                            FlightNumber1R = relatedFlightsR[0].FlightNumber,
                            FlightDepDate1R = relatedFlightsR[0].DepartureDate,
                            FlightDepTime1R = relatedFlightsR[0].DepartureTime,
                            FlightArrDate1R = relatedFlightsR[0].ArrivalDate,
                            FlightArrTime1R = relatedFlightsR[0].ArrivalTime,
                            Duration1R = relatedFlightsR[0].DurationDesc,


                            Class2R = ReturnCls(relatedFlightsR[1]).ToString(),
                            Cabin2R = relatedFlightsR[1].Cabin,
                            DepartStationName2R = relatedFlightsR[1].DepartureStationName,
                            ArrivalStationName2R = relatedFlightsR[1].ArrivalStationName,
                            DepartureStationAirport2R = relatedFlightsR[1].DepartureStationAirport,
                            ArrivalStationAirport2R = relatedFlightsR[1].ArrivalStationAirport,
                            SRC2R = relatedFlightsR[1].DepartureStation,
                            DEST2R = relatedFlightsR[1].ArrivalStation,
                            Via1R = checkVia(relatedFlightsR[1].Via),
                            ViaName1R = checkViaName(relatedFlightsR[1].ViaName),
                            TerminalSRC1R = checkTerminal(relatedFlightsR[1].DepartureTerminal),
                            TerminalDEST1R = checkTerminal(relatedFlightsR[1].ArrivalTerminal),
                            logo1R = "/assets/img/airlogo_square/" + relatedFlightsR[1].CarrierCode + ".gif",
                            FlightName2R = relatedFlightsR[1].CarrierCode,
                            FlightNumber2R = relatedFlightsR[1].FlightNumber,
                            FlightDepDate2R = relatedFlightsR[1].DepartureDate,
                            FlightDepTime2R = relatedFlightsR[1].DepartureTime,
                            FlightArrDate2R = relatedFlightsR[1].ArrivalDate,
                            FlightArrTime2R = relatedFlightsR[1].ArrivalTime,
                            Duration2R = relatedFlightsR[1].DurationDesc,


                            Class3R = ReturnCls(relatedFlightsR[2]).ToString(),
                            Cabin3R = relatedFlightsR[2].Cabin,
                            DepartStationName3R = relatedFlightsR[2].DepartureStationName,
                            ArrivalStationName3R = relatedFlightsR[2].ArrivalStationName,
                            DepartureStationAirport3R = relatedFlightsR[2].DepartureStationAirport,
                            ArrivalStationAirport3R = relatedFlightsR[2].ArrivalStationAirport,
                            SRC3R = relatedFlightsR[2].DepartureStation,
                            DEST3R = relatedFlightsR[2].ArrivalStation,
                            Via2R = checkVia(relatedFlightsR[2].Via),
                            ViaName2R = checkViaName(relatedFlightsR[2].ViaName),
                            TerminalSRC2R = checkTerminal(relatedFlightsR[2].DepartureTerminal),
                            TerminalDEST2R = checkTerminal(relatedFlightsR[2].ArrivalTerminal),
                            logo2R = "/assets/img/airlogo_square/" + relatedFlightsR[2].CarrierCode + ".gif",
                            FlightName3R = relatedFlightsR[2].CarrierCode,
                            FlightNumber3R = relatedFlightsR[2].FlightNumber,
                            FlightDepDate3R = relatedFlightsR[2].DepartureDate,
                            FlightDepTime3R = relatedFlightsR[2].DepartureTime,
                            FlightArrDate3R = relatedFlightsR[2].ArrivalDate,
                            FlightArrTime3R = relatedFlightsR[2].ArrivalTime,
                            Duration3R = relatedFlightsR[2].DurationDesc,
                            Layover2R = relatedFlightsR[2].JourneyTimeDesc
                        });
                    }
                    #endregion
                }
                catch (Exception ex)
                {

                }
            }
            #endregion
            #region
            else if (relatedFlights.Count == 3)
            {
                try
                {
                    rules = GetFareRules(relatedFlights);
                    rulesIB = GetFareRules(relatedFlightsR);

                    string departureDate = relatedFlights[0].DepartureDate.ToString(); // Assuming DepartureDate is a property of k_ShowFlightOutBound
                    string arrivalDate = relatedFlights[2].ArrivalDate.ToString();
                    if (departureDate != arrivalDate)
                    {
                        arrvchack = DateHelper.DayDiff(departureDate, arrivalDate);
                    }


                    #region
                    if (relatedFlightsR.Count == 1)
                    {
                        string departureDateR = relatedFlightsR[0].DepartureDate.ToString(); // Assuming DepartureDate is a property of k_ShowFlightOutBound
                        string arrivalDateR = relatedFlightsR[0].ArrivalDate.ToString();
                        if (departureDateR != arrivalDateR)
                        {
                            arrvchackR = DateHelper.DayDiff(departureDateR, arrivalDateR);
                        }


                        FlightOutBound.Add(new k_ShowFlightInternational()
                        {
                            F_Status = fStatus,
                            F_Remark = fRemark,
                            AgentType = ChekA_C,
                            CompanyID = CompanyID,
                            FlightRefid = relatedFlights[0].RefID.ToString(),
                            Curr = curr,
                            NoOFAdult = relatedFlights[0].Adt,
                            NoOFChild = relatedFlights[0].Chd,
                            NoOFInfant = relatedFlights[0].Inf,

                            //==============================================================================================================================
                            Adt_BASIC = GetConvert(relatedFlights[0].Adt_BASIC.ToString()),
                            Chd_BASIC = GetConvert(relatedFlights[0].Chd_BASIC.ToString()),
                            Inf_BASIC = GetConvert(relatedFlights[0].Inf_BASIC.ToString()),

                            AdultbaseFare = relatedFlights[0].Adt * GetConvert(relatedFlights[0].Adt_BASIC),
                            ChildbaseFare = relatedFlights[0].Chd * GetConvert(relatedFlights[0].Chd_BASIC),
                            InfantbaseFare = relatedFlights[0].Inf * GetConvert(relatedFlights[0].Inf_BASIC),

                            Adt_YQ = GetConvert(relatedFlights[0].Adt_YQ),
                            AdtTotalTax = GetConvert(relatedFlights[0].AdtTotalTax),
                            Chd_YQ = GetConvert(relatedFlights[0].Chd_YQ),
                            ChdTotalTax = GetConvert(relatedFlights[0].ChdTotalTax),
                            Inf_TAX = GetConvert(relatedFlights[0].Inf_TAX),

                            Adt_AcuTax = ReturnTAXADT(relatedFlights[0]).ToString(),
                            Chd_AcuTax = ReturnTAXCHD(relatedFlights[0]).ToString(),
                            Inf_AcuTax = ReturnTAXINF(relatedFlights[0]).ToString(),

                            Yq = "Commission : " + String.Format("{0:N0}", GetConvert(relatedFlights[0].TotalCommission)),
                            PromoChek = promo,


                            //==============================================================================================================================

                            TotalAmount = ReturnTotalFareAmount(relatedFlights[0]),
                            TotalFare = ReturnTotalFareAmount(relatedFlights[0]),
                            FlightAmount = ReturnTotalFareAmount(relatedFlights[0]),
                            TotalTax = GetTotalTax(relatedFlights[0]),
                            TotalBasic = GetTotalBasic(relatedFlights[0]),
                            TotalServiceFee = GetTotalServiceFee(relatedFlights[0]),
                            TotalMarkup = GetTotalMarkUp(relatedFlights[0]),
                            TotalServiceTax = GetTotalServiceTax(relatedFlights[0]),
                            TaxandCharges = GetTaxandCharges(relatedFlights[0]),
                            TotalCommission = ReturnCommission(relatedFlights[0], CompanyID),
                            TotalTds = ReturnTDS(relatedFlights[0], CompanyID),
                            FinalFare = ReturnFinalFare(relatedFlights[0], CompanyID),
                            //==============================================================================================================================

                            RuleTarrif = relatedFlights[0].RuleTarrif ?? string.Empty,
                            FlightName = relatedFlights[0].CarrierCode,
                            FlightNumber = relatedFlights[0].FlightNumber,
                            FlightDepDate = relatedFlights[0].DepartureDate,
                            FlightDepTime = relatedFlights[0].DepartureTime,
                            FlightArrDate = relatedFlights[2].ArrivalDate,
                            FlightArrTime = relatedFlights[2].ArrivalTime,
                            CHECKINBaggage = ReturnCheckINBaggage(relatedFlights[0]).ToString(),
                            CABINBaggage = ReturnCabinINBaggage(relatedFlights[0]).ToString(),
                            SRC = relatedFlights[0].Origin,
                            DEST = relatedFlights[0].Destination,
                            logo = "/assets/img/airlogo_square/" + relatedFlights[0].CarrierCode + ".gif",
                            Duration = relatedFlights[0].DurationDesc,
                            Stop = "TwoStop",
                            connection = 3,
                            ArrivalNextDayCheck = arrvchack,
                            PriceType = relatedFlights[0].PriceType,
                            RefundType = ReturnRefundType(relatedFlights[0]).ToString(),
                            AvailableSeat = checkSeat(relatedFlights[0].SeatsAvailable.ToString()),
                            FareRules = rules,
                            DepartStationName = relatedFlights[0].DepartureStationName,
                            ArrivalStationName = relatedFlights[2].ArrivalStationName,
                            DepartureStationAirport = relatedFlights[0].DepartureStationAirport,
                            ArrivalStationAirport = relatedFlights[2].ArrivalStationAirport,
                            Class = ReturnCls(relatedFlights[0]),
                            Cabin = relatedFlights[0].Cabin,
                            Layover1 = relatedFlights[2].JourneyTimeDesc,

                            SRC1 = relatedFlights[0].DepartureStation,
                            DEST1 = relatedFlights[0].ArrivalStation,
                            Via = checkVia(relatedFlights[0].Via),
                            ViaName = checkViaName(relatedFlights[0].ViaName),
                            TerminalSRC = checkTerminal(relatedFlights[0].DepartureTerminal),
                            TerminalDEST = checkTerminal(relatedFlights[2].ArrivalTerminal),
                            FlightName1 = relatedFlights[0].CarrierCode,
                            FlightNumber1 = relatedFlights[0].FlightNumber,
                            FlightDepDate1 = relatedFlights[0].DepartureDate,
                            FlightDepTime1 = relatedFlights[0].DepartureTime,
                            FlightArrDate1 = relatedFlights[0].ArrivalDate,
                            FlightArrTime1 = relatedFlights[0].ArrivalTime,
                            Duration1 = relatedFlights[0].DurationDesc,

                            DepartStationName2 = relatedFlights[1].DepartureStationName,
                            ArrivalStationName2 = relatedFlights[1].ArrivalStationName,
                            DepartureStationAirport2 = relatedFlights[1].DepartureStationAirport,
                            ArrivalStationAirport2 = relatedFlights[1].ArrivalStationAirport,
                            Class2 = ReturnCls(relatedFlights[1]),
                            Cabin2 = relatedFlights[1].Cabin,
                            SRC2 = relatedFlights[1].DepartureStation,
                            DEST2 = relatedFlights[1].ArrivalStation,
                            Via1 = checkVia(relatedFlights[1].Via),
                            ViaName1 = checkViaName(relatedFlights[1].ViaName),
                            TerminalSRC1 = checkTerminal(relatedFlights[1].DepartureTerminal),
                            TerminalDEST1 = checkTerminal(relatedFlights[1].ArrivalTerminal),
                            logo1 = "/assets/img/airlogo_square/" + relatedFlights[1].CarrierCode + ".gif",
                            FlightName2 = relatedFlights[1].CarrierCode,
                            FlightNumber2 = relatedFlights[1].FlightNumber,
                            FlightDepDate2 = relatedFlights[1].DepartureDate,
                            FlightDepTime2 = relatedFlights[1].DepartureTime,
                            FlightArrDate2 = relatedFlights[1].ArrivalDate,
                            FlightArrTime2 = relatedFlights[1].ArrivalTime,
                            Duration2 = relatedFlights[1].DurationDesc,

                            DepartStationName3 = relatedFlights[2].DepartureStationName,
                            ArrivalStationName3 = relatedFlights[2].ArrivalStationName,
                            DepartureStationAirport3 = relatedFlights[2].DepartureStationAirport,
                            ArrivalStationAirport3 = relatedFlights[2].ArrivalStationAirport,
                            Class3 = ReturnCls(relatedFlights[2]),
                            Cabin3 = relatedFlights[2].Cabin,
                            SRC3 = relatedFlights[2].DepartureStation,
                            DEST3 = relatedFlights[2].ArrivalStation,
                            Via2 = checkVia(relatedFlights[2].Via),
                            ViaName2 = checkViaName(relatedFlights[2].ViaName),
                            TerminalSRC2 = checkTerminal(relatedFlights[2].DepartureTerminal),
                            TerminalDEST2 = checkTerminal(relatedFlights[2].ArrivalTerminal),
                            logo2 = "/assets/img/airlogo_square/" + relatedFlights[2].CarrierCode + ".gif",
                            FlightName3 = relatedFlights[2].CarrierCode,
                            FlightNumber3 = relatedFlights[2].FlightNumber,
                            FlightDepDate3 = relatedFlights[2].DepartureDate,
                            FlightDepTime3 = relatedFlights[2].DepartureTime,
                            FlightArrDate3 = relatedFlights[2].ArrivalDate,
                            FlightArrTime3 = relatedFlights[2].ArrivalTime,
                            Duration3 = relatedFlights[2].DurationDesc,
                            Layover2 = relatedFlights[2].JourneyTimeDesc,

                            CHECKINBaggageR = ReturnCheckINBaggage(relatedFlightsR[0]).ToString(),
                            CABINBaggageR = ReturnCabinINBaggage(relatedFlightsR[0]).ToString(),
                            FareRulesR = rulesIB,
                            FlightNameR = relatedFlightsR[0].CarrierCode,
                            FlightNumberR = relatedFlightsR[0].FlightNumber,
                            FlightDepDateR = relatedFlightsR[0].DepartureDate,
                            FlightDepTimeR = relatedFlightsR[0].DepartureTime,
                            FlightArrDateR = relatedFlightsR[0].ArrivalDate,
                            FlightArrTimeR = relatedFlightsR[0].ArrivalTime,
                            SRCR = relatedFlightsR[0].Origin,
                            DESTR = relatedFlightsR[0].Destination,
                            StopR = "NonStop",
                            logoR = "/assets/img/airlogo_square/" + relatedFlightsR[0].CarrierCode + ".gif",
                            DurationR = relatedFlightsR[0].DurationDesc,
                            connectionR = 1,
                            PriceTypeR = relatedFlightsR[0].PriceType,
                            RefundTypeR = ReturnRefundType(relatedFlightsR[0]).ToString(),
                            AvailableSeatR = checkSeat(relatedFlightsR[0].SeatsAvailable.ToString()),
                            ArrivalNextDayCheckR = arrvchackR,
                            DepartStationNameR = relatedFlightsR[0].DepartureStationName,
                            ArrivalStationNameR = relatedFlightsR[0].ArrivalStationName,
                            DepartureStationAirportR = relatedFlightsR[0].DepartureStationAirport,
                            ArrivalStationAirportR = relatedFlightsR[0].ArrivalStationAirport,
                            ClassR = ReturnCls(relatedFlightsR[0]).ToString(),
                            CabinR = relatedFlightsR[0].Cabin,
                            Layover1R = relatedFlightsR[0].JourneyTimeDesc,

                            SRC1R = relatedFlightsR[0].DepartureStation,
                            DEST1R = relatedFlightsR[0].ArrivalStation,
                            ViaR = checkVia(relatedFlightsR[0].Via),
                            ViaNameR = checkViaName(relatedFlightsR[0].ViaName),
                            TerminalSRCR = checkTerminal(relatedFlightsR[0].DepartureTerminal),
                            TerminalDESTR = checkTerminal(relatedFlightsR[0].ArrivalTerminal),
                            FlightName1R = relatedFlightsR[0].CarrierCode,
                            FlightNumber1R = relatedFlightsR[0].FlightNumber,
                            FlightDepDate1R = relatedFlightsR[0].DepartureDate,
                            FlightDepTime1R = relatedFlightsR[0].DepartureTime,
                            FlightArrDate1R = relatedFlightsR[0].ArrivalDate,
                            FlightArrTime1R = relatedFlightsR[0].ArrivalTime,
                            Duration1R = relatedFlightsR[0].DurationDesc
                        });
                    }
                    #endregion
                    #region
                    else if (relatedFlightsR.Count == 2)
                    {
                        string departureDateR = relatedFlightsR[0].DepartureDate.ToString(); // Assuming DepartureDate is a property of k_ShowFlightOutBound
                        string arrivalDateR = relatedFlightsR[1].ArrivalDate.ToString();
                        if (departureDateR != arrivalDateR)
                        {
                            arrvchackR = DateHelper.DayDiff(departureDateR, arrivalDateR);
                        }


                        FlightOutBound.Add(new k_ShowFlightInternational()
                        {
                            F_Status = fStatus,
                            F_Remark = fRemark,
                            AgentType = ChekA_C,
                            CompanyID = CompanyID,
                            FlightRefid = relatedFlights[0].RefID.ToString(),
                            Curr = curr,
                            NoOFAdult = relatedFlights[0].Adt,
                            NoOFChild = relatedFlights[0].Chd,
                            NoOFInfant = relatedFlights[0].Inf,

                            //==============================================================================================================================
                            Adt_BASIC = GetConvert(relatedFlights[0].Adt_BASIC.ToString()),
                            Chd_BASIC = GetConvert(relatedFlights[0].Chd_BASIC.ToString()),
                            Inf_BASIC = GetConvert(relatedFlights[0].Inf_BASIC.ToString()),

                            AdultbaseFare = relatedFlights[0].Adt * GetConvert(relatedFlights[0].Adt_BASIC),
                            ChildbaseFare = relatedFlights[0].Chd * GetConvert(relatedFlights[0].Chd_BASIC),
                            InfantbaseFare = relatedFlights[0].Inf * GetConvert(relatedFlights[0].Inf_BASIC),

                            Adt_YQ = GetConvert(relatedFlights[0].Adt_YQ),
                            AdtTotalTax = GetConvert(relatedFlights[0].AdtTotalTax),
                            Chd_YQ = GetConvert(relatedFlights[0].Chd_YQ),
                            ChdTotalTax = GetConvert(relatedFlights[0].ChdTotalTax),
                            Inf_TAX = GetConvert(relatedFlights[0].Inf_TAX),

                            Adt_AcuTax = ReturnTAXADT(relatedFlights[0]).ToString(),
                            Chd_AcuTax = ReturnTAXCHD(relatedFlights[0]).ToString(),
                            Inf_AcuTax = ReturnTAXINF(relatedFlights[0]).ToString(),

                            Yq = "Commission : " + String.Format("{0:N0}", GetConvert(relatedFlights[0].TotalCommission)),
                            PromoChek = promo,


                            TotalAmount = ReturnTotalFareAmount(relatedFlights[0]),
                            TotalFare = ReturnTotalFareAmount(relatedFlights[0]),
                            FlightAmount = ReturnTotalFareAmount(relatedFlights[0]),
                            TotalTax = GetTotalTax(relatedFlights[0]),
                            TotalBasic = GetTotalBasic(relatedFlights[0]),
                            TotalServiceFee = GetTotalServiceFee(relatedFlights[0]),
                            TotalMarkup = GetTotalMarkUp(relatedFlights[0]),
                            TotalServiceTax = GetTotalServiceTax(relatedFlights[0]),
                            TaxandCharges = GetTaxandCharges(relatedFlights[0]),
                            TotalCommission = ReturnCommission(relatedFlights[0], CompanyID),
                            TotalTds = ReturnTDS(relatedFlights[0], CompanyID),
                            FinalFare = ReturnFinalFare(relatedFlights[0], CompanyID),
                            //==============================================================================================================================

                            FlightName = relatedFlights[0].CarrierCode,
                            FlightNumber = relatedFlights[0].FlightNumber,
                            FlightDepDate = relatedFlights[0].DepartureDate,
                            FlightDepTime = relatedFlights[0].DepartureTime,
                            FlightArrDate = relatedFlights[2].ArrivalDate,
                            FlightArrTime = relatedFlights[2].ArrivalTime,
                            RuleTarrif = relatedFlights[0].RuleTarrif ?? string.Empty,
                            CHECKINBaggage = ReturnCheckINBaggage(relatedFlights[0]).ToString(),
                            CABINBaggage = ReturnCabinINBaggage(relatedFlights[0]).ToString(),
                            SRC = relatedFlights[0].Origin,
                            DEST = relatedFlights[0].Destination,
                            logo = "/assets/img/airlogo_square/" + relatedFlights[0].CarrierCode + ".gif",
                            Duration = relatedFlights[0].DurationDesc,
                            Stop = "TwoStop",
                            connection = 3,
                            ArrivalNextDayCheck = arrvchack,
                            PriceType = relatedFlights[0].PriceType,
                            RefundType = ReturnRefundType(relatedFlights[0]).ToString(),
                            AvailableSeat = checkSeat(relatedFlights[0].SeatsAvailable.ToString()),
                            FareRules = rules,
                            DepartStationName = relatedFlights[0].DepartureStationName,
                            ArrivalStationName = relatedFlights[2].ArrivalStationName,
                            DepartureStationAirport = relatedFlights[0].DepartureStationAirport,
                            ArrivalStationAirport = relatedFlights[2].ArrivalStationAirport,
                            Class = ReturnCls(relatedFlights[0]),
                            Cabin = relatedFlights[0].Cabin,
                            Layover1 = relatedFlights[2].JourneyTimeDesc,

                            SRC1 = relatedFlights[0].DepartureStation,
                            DEST1 = relatedFlights[0].ArrivalStation,
                            Via = checkVia(relatedFlights[0].Via),
                            ViaName = checkViaName(relatedFlights[0].ViaName),
                            TerminalSRC = checkTerminal(relatedFlights[0].DepartureTerminal),
                            TerminalDEST = checkTerminal(relatedFlights[2].ArrivalTerminal),
                            FlightName1 = relatedFlights[0].CarrierCode,
                            FlightNumber1 = relatedFlights[0].FlightNumber,
                            FlightDepDate1 = relatedFlights[0].DepartureDate,
                            FlightDepTime1 = relatedFlights[0].DepartureTime,
                            FlightArrDate1 = relatedFlights[0].ArrivalDate,
                            FlightArrTime1 = relatedFlights[0].ArrivalTime,
                            Duration1 = relatedFlights[0].DurationDesc,

                            DepartStationName2 = relatedFlights[1].DepartureStationName,
                            ArrivalStationName2 = relatedFlights[1].ArrivalStationName,
                            DepartureStationAirport2 = relatedFlights[1].DepartureStationAirport,
                            ArrivalStationAirport2 = relatedFlights[1].ArrivalStationAirport,
                            Class2 = ReturnCls(relatedFlights[1]),
                            Cabin2 = relatedFlights[1].Cabin,
                            SRC2 = relatedFlights[1].DepartureStation,
                            DEST2 = relatedFlights[1].ArrivalStation,
                            Via1 = checkVia(relatedFlights[1].Via),
                            ViaName1 = checkViaName(relatedFlights[1].ViaName),
                            TerminalSRC1 = checkTerminal(relatedFlights[1].DepartureTerminal),
                            TerminalDEST1 = checkTerminal(relatedFlights[1].ArrivalTerminal),
                            logo1 = "/assets/img/airlogo_square/" + relatedFlights[1].CarrierCode + ".gif",
                            FlightName2 = relatedFlights[1].CarrierCode,
                            FlightNumber2 = relatedFlights[1].FlightNumber,
                            FlightDepDate2 = relatedFlights[1].DepartureDate,
                            FlightDepTime2 = relatedFlights[1].DepartureTime,
                            FlightArrDate2 = relatedFlights[1].ArrivalDate,
                            FlightArrTime2 = relatedFlights[1].ArrivalTime,
                            Duration2 = relatedFlights[1].DurationDesc,

                            DepartStationName3 = relatedFlights[2].DepartureStationName,
                            ArrivalStationName3 = relatedFlights[2].ArrivalStationName,
                            DepartureStationAirport3 = relatedFlights[2].DepartureStationAirport,
                            ArrivalStationAirport3 = relatedFlights[2].ArrivalStationAirport,
                            Class3 = ReturnCls(relatedFlights[2]),
                            Cabin3 = relatedFlights[2].Cabin,
                            SRC3 = relatedFlights[2].DepartureStation,
                            DEST3 = relatedFlights[2].ArrivalStation,
                            Via2 = checkVia(relatedFlights[2].Via),
                            ViaName2 = checkViaName(relatedFlights[2].ViaName),
                            TerminalSRC2 = checkTerminal(relatedFlights[2].DepartureTerminal),
                            TerminalDEST2 = checkTerminal(relatedFlights[2].ArrivalTerminal),
                            logo2 = "/assets/img/airlogo_square/" + relatedFlights[2].CarrierCode + ".gif",
                            FlightName3 = relatedFlights[2].CarrierCode,
                            FlightNumber3 = relatedFlights[2].FlightNumber,
                            FlightDepDate3 = relatedFlights[2].DepartureDate,
                            FlightDepTime3 = relatedFlights[2].DepartureTime,
                            FlightArrDate3 = relatedFlights[2].ArrivalDate,
                            FlightArrTime3 = relatedFlights[2].ArrivalTime,
                            Duration3 = relatedFlights[2].DurationDesc,
                            Layover2 = relatedFlights[2].JourneyTimeDesc,

                            FareRulesR = rulesIB,
                            FlightNameR = relatedFlightsR[0].CarrierCode,
                            FlightNumberR = relatedFlightsR[0].FlightNumber,
                            FlightDepDateR = relatedFlightsR[0].DepartureDate,
                            FlightDepTimeR = relatedFlightsR[0].DepartureTime,
                            FlightArrDateR = relatedFlightsR[1].ArrivalDate,
                            FlightArrTimeR = relatedFlightsR[1].ArrivalTime,
                            SRCR = relatedFlightsR[0].Origin,
                            DESTR = relatedFlightsR[0].Destination,
                            StopR = "OneStop",
                            logoR = "/assets/img/airlogo_square/" + relatedFlightsR[0].CarrierCode + ".gif",
                            DurationR = relatedFlightsR[0].DurationDesc,
                            connectionR = 2,
                            PriceTypeR = relatedFlightsR[0].PriceType,
                            RefundTypeR = ReturnRefundType(relatedFlightsR[0]).ToString(),
                            AvailableSeatR = checkSeat(relatedFlightsR[0].SeatsAvailable.ToString()),
                            ArrivalNextDayCheckR = arrvchackR,
                            DepartStationNameR = relatedFlightsR[0].DepartureStationName,
                            ArrivalStationNameR = relatedFlightsR[1].ArrivalStationName,
                            DepartureStationAirportR = relatedFlightsR[0].DepartureStationAirport,
                            ArrivalStationAirportR = relatedFlightsR[1].ArrivalStationAirport,
                            ClassR = ReturnCls(relatedFlightsR[0]).ToString(),
                            CabinR = relatedFlightsR[0].Cabin,
                            Layover1R = relatedFlightsR[1].JourneyTimeDesc,

                            SRC1R = relatedFlightsR[0].DepartureStation,
                            DEST1R = relatedFlightsR[0].ArrivalStation,
                            ViaR = checkVia(relatedFlightsR[0].Via),
                            ViaNameR = checkViaName(relatedFlightsR[0].ViaName),
                            TerminalSRCR = checkTerminal(relatedFlightsR[0].DepartureTerminal),
                            TerminalDESTR = checkTerminal(relatedFlightsR[1].ArrivalTerminal),
                            FlightName1R = relatedFlightsR[0].CarrierCode,
                            FlightNumber1R = relatedFlightsR[0].FlightNumber,
                            FlightDepDate1R = relatedFlightsR[0].DepartureDate,
                            FlightDepTime1R = relatedFlightsR[0].DepartureTime,
                            FlightArrDate1R = relatedFlightsR[0].ArrivalDate,
                            FlightArrTime1R = relatedFlightsR[0].ArrivalTime,
                            Duration1R = relatedFlightsR[0].DurationDesc,


                             Class2R = ReturnCls(relatedFlightsR[1]).ToString(),
                            Cabin2R = relatedFlightsR[1].Cabin,
                            DepartStationName2R = relatedFlightsR[1].DepartureStationName,
                            ArrivalStationName2R = relatedFlightsR[1].ArrivalStationName,
                            DepartureStationAirport2R = relatedFlightsR[1].DepartureStationAirport,
                            ArrivalStationAirport2R = relatedFlightsR[1].ArrivalStationAirport,
                            SRC2R = relatedFlightsR[1].DepartureStation,
                            DEST2R = relatedFlightsR[1].ArrivalStation,
                            Via1R = checkVia(relatedFlightsR[1].Via),
                            ViaName1R = checkViaName(relatedFlightsR[1].ViaName),
                            TerminalSRC1R = checkTerminal(relatedFlightsR[1].DepartureTerminal),
                            TerminalDEST1R = checkTerminal(relatedFlightsR[1].ArrivalTerminal),
                            logo1R = "/assets/img/airlogo_square/" + relatedFlightsR[1].CarrierCode + ".gif",
                            FlightName2R = relatedFlightsR[1].CarrierCode,
                            FlightNumber2R = relatedFlightsR[1].FlightNumber,
                            FlightDepDate2R = relatedFlightsR[1].DepartureDate,
                            FlightDepTime2R = relatedFlightsR[1].DepartureTime,
                            FlightArrDate2R = relatedFlightsR[1].ArrivalDate,
                            FlightArrTime2R = relatedFlightsR[1].ArrivalTime,
                            Duration2R = relatedFlightsR[1].DurationDesc,

                        });
                    }
                    #endregion
                    #region
                    else if (relatedFlightsR.Count == 3)
                    {

                        string departureDateR = relatedFlightsR[0].DepartureDate.ToString(); // Assuming DepartureDate is a property of k_ShowFlightOutBound
                        string arrivalDateR = relatedFlightsR[2].ArrivalDate.ToString();
                        if (departureDateR != arrivalDateR)
                        {
                            arrvchackR = DateHelper.DayDiff(departureDateR, arrivalDateR);
                        }

                        FlightOutBound.Add(new k_ShowFlightInternational()
                        {
                            F_Status = fStatus,
                            F_Remark = fRemark,
                            AgentType = ChekA_C,
                            CompanyID = CompanyID,
                            FlightRefid = relatedFlights[0].RefID.ToString(),
                            Curr = curr,


                            NoOFAdult = relatedFlights[0].Adt,
                            NoOFChild = relatedFlights[0].Chd,
                            NoOFInfant = relatedFlights[0].Inf,

                            //==============================================================================================================================
                            Adt_BASIC = GetConvert(relatedFlights[0].Adt_BASIC.ToString()),
                            Chd_BASIC = GetConvert(relatedFlights[0].Chd_BASIC.ToString()),
                            Inf_BASIC = GetConvert(relatedFlights[0].Inf_BASIC.ToString()),

                            AdultbaseFare = relatedFlights[0].Adt * GetConvert(relatedFlights[0].Adt_BASIC),
                            ChildbaseFare = relatedFlights[0].Chd * GetConvert(relatedFlights[0].Chd_BASIC),
                            InfantbaseFare = relatedFlights[0].Inf * GetConvert(relatedFlights[0].Inf_BASIC),

                            Adt_YQ = GetConvert(relatedFlights[0].Adt_YQ),
                            AdtTotalTax = GetConvert(relatedFlights[0].AdtTotalTax),
                            Chd_YQ = GetConvert(relatedFlights[0].Chd_YQ),
                            ChdTotalTax = GetConvert(relatedFlights[0].ChdTotalTax),
                            Inf_TAX = GetConvert(relatedFlights[0].Inf_TAX),

                            Adt_AcuTax = ReturnTAXADT(relatedFlights[0]).ToString(),
                            Chd_AcuTax = ReturnTAXCHD(relatedFlights[0]).ToString(),
                            Inf_AcuTax = ReturnTAXINF(relatedFlights[0]).ToString(),

                            Yq = "Commission : " + String.Format("{0:N0}", GetConvert(relatedFlights[0].TotalCommission)),
                            PromoChek = promo,

                            TotalAmount = ReturnTotalFareAmount(relatedFlights[0]),
                            TotalFare = ReturnTotalFareAmount(relatedFlights[0]),
                            FlightAmount = ReturnTotalFareAmount(relatedFlights[0]),
                            TotalTax = GetTotalTax(relatedFlights[0]),
                            TotalBasic = GetTotalBasic(relatedFlights[0]),
                            TotalServiceFee = GetTotalServiceFee(relatedFlights[0]),
                            TotalMarkup = GetTotalMarkUp(relatedFlights[0]),
                            TotalServiceTax = GetTotalServiceTax(relatedFlights[0]),
                            TaxandCharges = GetTaxandCharges(relatedFlights[0]),
                            TotalCommission = ReturnCommission(relatedFlights[0], CompanyID),
                            TotalTds = ReturnTDS(relatedFlights[0], CompanyID),
                            FinalFare = ReturnFinalFare(relatedFlights[0], CompanyID),
                            //==============================================================================================================================

                            FlightName = relatedFlights[0].CarrierCode,
                            FlightNumber = relatedFlights[0].FlightNumber,
                            FlightDepDate = relatedFlights[0].DepartureDate,
                            FlightDepTime = relatedFlights[0].DepartureTime,
                            FlightArrDate = relatedFlights[2].ArrivalDate,
                            FlightArrTime = relatedFlights[2].ArrivalTime,
                            RuleTarrif = relatedFlights[0].RuleTarrif ?? string.Empty,
                            CHECKINBaggage = ReturnCheckINBaggage(relatedFlights[0]).ToString(),
                            CABINBaggage = ReturnCabinINBaggage(relatedFlights[0]).ToString(),
                            SRC = relatedFlights[0].Origin,
                            DEST = relatedFlights[0].Destination,
                            logo = "/assets/img/airlogo_square/" + relatedFlights[0].CarrierCode + ".gif",
                            Duration = relatedFlights[0].DurationDesc,
                            Stop = "TwoStop",
                            connection = 3,
                            ArrivalNextDayCheck = arrvchack,
                            PriceType = relatedFlights[0].PriceType,
                            RefundType = ReturnRefundType(relatedFlights[0]).ToString(),
                            AvailableSeat = checkSeat(relatedFlights[0].SeatsAvailable.ToString()),
                            FareRules = rules,
                            DepartStationName = relatedFlights[0].DepartureStationName,
                            ArrivalStationName = relatedFlights[2].ArrivalStationName,
                            DepartureStationAirport = relatedFlights[0].DepartureStationAirport,
                            ArrivalStationAirport = relatedFlights[2].ArrivalStationAirport,
                            Class = ReturnCls(relatedFlights[0]),
                            Cabin = relatedFlights[0].Cabin,
                            Layover1 = relatedFlights[2].JourneyTimeDesc,

                            SRC1 = relatedFlights[0].DepartureStation,
                            DEST1 = relatedFlights[0].ArrivalStation,
                            Via = checkVia(relatedFlights[0].Via),
                            ViaName = checkViaName(relatedFlights[0].ViaName),
                            TerminalSRC = checkTerminal(relatedFlights[0].DepartureTerminal),
                            TerminalDEST = checkTerminal(relatedFlights[2].ArrivalTerminal),
                            FlightName1 = relatedFlights[0].CarrierCode,
                            FlightNumber1 = relatedFlights[0].FlightNumber,
                            FlightDepDate1 = relatedFlights[0].DepartureDate,
                            FlightDepTime1 = relatedFlights[0].DepartureTime,
                            FlightArrDate1 = relatedFlights[0].ArrivalDate,
                            FlightArrTime1 = relatedFlights[0].ArrivalTime,
                            Duration1 = relatedFlights[0].DurationDesc,

                            DepartStationName2 = relatedFlights[1].DepartureStationName,
                            ArrivalStationName2 = relatedFlights[1].ArrivalStationName,
                            DepartureStationAirport2 = relatedFlights[1].DepartureStationAirport,
                            ArrivalStationAirport2 = relatedFlights[1].ArrivalStationAirport,
                            Class2 = ReturnCls(relatedFlights[1]),
                            Cabin2 = relatedFlights[1].Cabin,
                            SRC2 = relatedFlights[1].DepartureStation,
                            DEST2 = relatedFlights[1].ArrivalStation,
                            Via1 = checkVia(relatedFlights[1].Via),
                            ViaName1 = checkViaName(relatedFlights[1].ViaName),
                            TerminalSRC1 = checkTerminal(relatedFlights[1].DepartureTerminal),
                            TerminalDEST1 = checkTerminal(relatedFlights[1].ArrivalTerminal),
                            logo1 = "/assets/img/airlogo_square/" + relatedFlights[1].CarrierCode + ".gif",
                            FlightName2 = relatedFlights[1].CarrierCode,
                            FlightNumber2 = relatedFlights[1].FlightNumber,
                            FlightDepDate2 = relatedFlights[1].DepartureDate,
                            FlightDepTime2 = relatedFlights[1].DepartureTime,
                            FlightArrDate2 = relatedFlights[1].ArrivalDate,
                            FlightArrTime2 = relatedFlights[1].ArrivalTime,
                            Duration2 = relatedFlights[1].DurationDesc,

                            DepartStationName3 = relatedFlights[2].DepartureStationName,
                            ArrivalStationName3 = relatedFlights[2].ArrivalStationName,
                            DepartureStationAirport3 = relatedFlights[2].DepartureStationAirport,
                            ArrivalStationAirport3 = relatedFlights[2].ArrivalStationAirport,
                            Class3 = ReturnCls(relatedFlights[2]),
                            Cabin3 = relatedFlights[2].Cabin,
                            SRC3 = relatedFlights[2].DepartureStation,
                            DEST3 = relatedFlights[2].ArrivalStation,
                            Via2 = checkVia(relatedFlights[2].Via),
                            ViaName2 = checkViaName(relatedFlights[2].ViaName),
                            TerminalSRC2 = checkTerminal(relatedFlights[2].DepartureTerminal),
                            TerminalDEST2 = checkTerminal(relatedFlights[2].ArrivalTerminal),
                            logo2 = "/assets/img/airlogo_square/" + relatedFlights[2].CarrierCode + ".gif",
                            FlightName3 = relatedFlights[2].CarrierCode,
                            FlightNumber3 = relatedFlights[2].FlightNumber,
                            FlightDepDate3 = relatedFlights[2].DepartureDate,
                            FlightDepTime3 = relatedFlights[2].DepartureTime,
                            FlightArrDate3 = relatedFlights[2].ArrivalDate,
                            FlightArrTime3 = relatedFlights[2].ArrivalTime,
                            Duration3 = relatedFlights[2].DurationDesc,
                            Layover2 = relatedFlights[2].JourneyTimeDesc,

                            FareRulesR = rulesIB,
                            FlightNameR = relatedFlightsR[0].CarrierCode,
                            FlightNumberR = relatedFlightsR[0].FlightNumber,
                            FlightDepDateR = relatedFlightsR[0].DepartureDate,
                            FlightDepTimeR = relatedFlightsR[0].DepartureTime,
                            FlightArrDateR = relatedFlightsR[2].ArrivalDate,
                            FlightArrTimeR = relatedFlightsR[2].ArrivalTime,
                            SRCR = relatedFlightsR[0].Origin,
                            DESTR = relatedFlightsR[0].Destination,
                            StopR = "TwoStop",
                            logoR = "/assets/img/airlogo_square/" + relatedFlightsR[0].CarrierCode + ".gif",
                            DurationR = relatedFlightsR[0].DurationDesc,
                            connectionR = 3,
                            PriceTypeR = relatedFlightsR[0].PriceType,
                            RefundTypeR = ReturnRefundType(relatedFlightsR[0]).ToString(),
                            AvailableSeatR = checkSeat(relatedFlightsR[0].SeatsAvailable.ToString()),
                            ArrivalNextDayCheckR = arrvchackR,
                            DepartStationNameR = relatedFlightsR[0].DepartureStationName,
                            ArrivalStationNameR = relatedFlightsR[2].ArrivalStationName,
                            DepartureStationAirportR = relatedFlightsR[0].DepartureStationAirport,
                            ArrivalStationAirportR = relatedFlightsR[2].ArrivalStationAirport,
                            ClassR = ReturnCls(relatedFlightsR[0]).ToString(),
                            CabinR = relatedFlightsR[0].Cabin,
                            Layover1R = relatedFlightsR[2].JourneyTimeDesc,


                            SRC1R = relatedFlightsR[0].DepartureStation,
                            DEST1R = relatedFlightsR[0].ArrivalStation,
                            ViaR = checkVia(relatedFlightsR[0].Via),
                            ViaNameR = checkViaName(relatedFlightsR[0].ViaName),
                            TerminalSRCR = checkTerminal(relatedFlightsR[0].DepartureTerminal),
                            TerminalDESTR = checkTerminal(relatedFlightsR[2].ArrivalTerminal),
                            FlightName1R = relatedFlightsR[0].CarrierCode,
                            FlightNumber1R = relatedFlightsR[0].FlightNumber,
                            FlightDepDate1R = relatedFlightsR[0].DepartureDate,
                            FlightDepTime1R = relatedFlightsR[0].DepartureTime,
                            FlightArrDate1R = relatedFlightsR[0].ArrivalDate,
                            FlightArrTime1R = relatedFlightsR[0].ArrivalTime,
                            Duration1R = relatedFlightsR[0].DurationDesc,


                             Class2R = ReturnCls(relatedFlightsR[1]).ToString(),
                            Cabin2R = relatedFlightsR[1].Cabin,
                            DepartStationName2R = relatedFlightsR[1].DepartureStationName,
                            ArrivalStationName2R = relatedFlightsR[1].ArrivalStationName,
                            DepartureStationAirport2R = relatedFlightsR[1].DepartureStationAirport,
                            ArrivalStationAirport2R = relatedFlightsR[1].ArrivalStationAirport,
                            SRC2R = relatedFlightsR[1].DepartureStation,
                            DEST2R = relatedFlightsR[1].ArrivalStation,
                            Via1R = checkVia(relatedFlightsR[1].Via),
                            ViaName1R = checkViaName(relatedFlightsR[1].ViaName),
                            TerminalSRC1R = checkTerminal(relatedFlightsR[1].DepartureTerminal),
                            TerminalDEST1R = checkTerminal(relatedFlightsR[1].ArrivalTerminal),
                            logo1R = "/assets/img/airlogo_square/" + relatedFlightsR[1].CarrierCode + ".gif",
                            FlightName2R = relatedFlightsR[1].CarrierCode,
                            FlightNumber2R = relatedFlightsR[1].FlightNumber,
                            FlightDepDate2R = relatedFlightsR[1].DepartureDate,
                            FlightDepTime2R = relatedFlightsR[1].DepartureTime,
                            FlightArrDate2R = relatedFlightsR[1].ArrivalDate,
                            FlightArrTime2R = relatedFlightsR[1].ArrivalTime,
                            Duration2R = relatedFlightsR[1].DurationDesc,

                            Class3R = ReturnCls(relatedFlightsR[2]).ToString(),
                            Cabin3R = relatedFlightsR[2].Cabin,
                            DepartStationName3R = relatedFlightsR[2].DepartureStationName,
                            ArrivalStationName3R = relatedFlightsR[2].ArrivalStationName,
                            DepartureStationAirport3R = relatedFlightsR[2].DepartureStationAirport,
                            ArrivalStationAirport3R = relatedFlightsR[2].ArrivalStationAirport,
                            SRC3R = relatedFlightsR[2].DepartureStation,
                            DEST3R = relatedFlightsR[2].ArrivalStation,
                            Via2R = checkVia(relatedFlightsR[2].Via),
                            ViaName2R = checkViaName(relatedFlightsR[2].ViaName),
                            TerminalSRC2R = checkTerminal(relatedFlightsR[2].DepartureTerminal),
                            TerminalDEST2R = checkTerminal(relatedFlightsR[2].ArrivalTerminal),
                            logo2R = "/assets/img/airlogo_square/" + relatedFlightsR[2].CarrierCode + ".gif",
                            FlightName3R = relatedFlightsR[2].CarrierCode,
                            FlightNumber3R = relatedFlightsR[2].FlightNumber,
                            FlightDepDate3R = relatedFlightsR[2].DepartureDate,
                            FlightDepTime3R = relatedFlightsR[2].DepartureTime,
                            FlightArrDate3R = relatedFlightsR[2].ArrivalDate,
                            FlightArrTime3R = relatedFlightsR[2].ArrivalTime,
                            Duration3R = relatedFlightsR[2].DurationDesc,
                            Layover2R = relatedFlightsR[2].JourneyTimeDesc
                        });
                    }
                    #endregion
                }
                catch (Exception ex)
                {

                }
            }
            #endregion
            return FlightOutBound;
        }

        private static async Task<SelectedFlightUserInfo> SelectedFlightUserInfo(ClaimsPrincipal user, string RefId, IHandlesQueryAsync<string, CompanyRegisterCorporateUserDetails> getCompanyRegisterCorporateUserDetailsQueryHandler, IHandlesQueryAsync<string, CompanyRegisterCorporateUserLimitDetails> getCompanyRegisterCorporateUserLimitQueryHandler)
        {
            var fStatus = false;
            var fRemark = string.Empty;
            var limit = -1;

            //=========== Flight Depart Time is greater than 3hr from Current Date Time  Start ===========//
            string DepartureDate1 = string.Empty;
            string DepartureTime1 = string.Empty;
            string CarrierCode = string.Empty;

            GetDepartureDetail(RefId, out DepartureDate1, out DepartureTime1, out CarrierCode);

            DepartureDate1 = Convert.ToDateTime(DepartureDate1).ToString("yyyy-MM-dd");
            if (CarrierCode.Equals("I5") || CarrierCode.Equals("D7") || CarrierCode.Equals("FD") || CarrierCode.Equals("AK") || CarrierCode.Equals("XJ") || CarrierCode.Equals("XT") || CarrierCode.Equals("QZ") || CarrierCode.Equals("Z2"))
            {
                fStatus = DateHelper.TimeDiffCurrentTime(Convert.ToDateTime(DepartureDate1), Convert.ToDateTime(DepartureTime1));
                if (fStatus.Equals(false))
                {
                    fRemark = "Kindly select another flight, departure time should be greater than 4hr !!!";
                }
            }
            else
            {
                fStatus = true;
            }
            //=========== Flight Depart Time is greater than 3hr from Current Date Time  End ===========//

            if (fStatus)
            {
                var staffID = UserHelper.GetStaffID(user);
                if (!string.IsNullOrEmpty(staffID) && staffID.IndexOf("-ST-") != -1)
                {
                    //======================== CORPORATE USER ================================================//
                    bool Is_User = true;
                    Is_User = UserHelper.FindCorporateUser(user);
                    var corporateUser = await getCompanyRegisterCorporateUserDetailsQueryHandler.HandleAsync(staffID);
                    if (Is_User)
                    {

                        bool Is_Itinerary_User = corporateUser.IsItinerary.Value;
                        if (Is_Itinerary_User)
                        {
                            fStatus = false;
                            fRemark = "You are not authorised to book a flight, you can only request itineraries !!!";
                        }
                        else
                        {
                            bool Is_Booking_User = corporateUser.IsMaker.Value;
                            if (Is_Booking_User)
                            {
                                var corporateUserLimit = await getCompanyRegisterCorporateUserLimitQueryHandler.HandleAsync(staffID);
                                limit = corporateUserLimit.Limit.Value;
                            }
                        }
                    }
                    //======================== CORPORATE USER ================================================//
                }
            }

            return new SelectedFlightUserInfo
            {
                ValidTimeUser = fStatus,
                Remark = fRemark,
                Limit = limit
            };
        }

        public static void GetDepartureDetail(string RefID, out string DepartureDate, out string DepartureTime, out string CarrierCode)
        {
            DepartureDate = string.Empty;
            DepartureTime = string.Empty;
            CarrierCode = string.Empty;
            try
            {
                string availabilityResponse = HttpContextHelper.Current?.Session.GetString("FinalResult");
                if (string.IsNullOrEmpty(availabilityResponse))
                    return;

                XmlDocument xml = new XmlDocument();
                xml.LoadXml(availabilityResponse);
                XmlNodeList xnListOB = xml.SelectNodes($"/root/AvailabilityInfo[RefID='{RefID}']");
                foreach (XmlNode node in xnListOB)
                {
                    DepartureDate = node["DepartureDate"].InnerText;
                    DepartureTime = node["DepartureTime"].InnerText;
                    CarrierCode = node["CarrierCode"].InnerText;
                    break;
                }
            }
            catch
            {
                // Handle exceptions if needed
            }
        }

        private static List<AirlineAvailabilityInfo> SelectionFlight(string RefId, string FlightType)
        {
            var flightList = new List<AirlineAvailabilityInfo>();
            if (FlightType == "OUTBOUND")
            {
                flightList = GetSelec("O");
            }
            else
            {
                flightList = GetSelec("I");
            }

            

            string FltType = "O";
            if (FlightType.Equals("INBOUND"))
            {
                FltType = "I";
            }

            var result = flightList
                .Where(x => x.RefID == Convert.ToInt32(RefId) && x.FltType == FltType)
                .OrderBy(x => x.RowID)
                .ToList();


            return result;
        }

    }
}
