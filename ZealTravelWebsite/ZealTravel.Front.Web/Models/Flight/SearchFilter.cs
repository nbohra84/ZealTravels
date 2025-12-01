namespace ZealTravel.Front.Web.Models.Flight.SearchFilter
{
    /*public class k_ShowFlightOutBoundOnlyShow
{
    public string Curr { get; set; }
    public bool F_Status { get; set; }
    public string F_Remark { get; set; }
    public string RuleTarrif { get; set; }
    public string InboundOutbound { get; set; }
    public int AgentType { get; set; }
    public string FlightName { get; set; }
    public string Cabin { get; set; }
    public string DepartStationName { get; set; }
    public string ArrivalStationName { get; set; }
    public string DepartureStationAirport { get; set; }
    public string ArrivalStationAirport { get; set; }
    public string DepartureStationAirport1 { get; set; }
    public string ArrivalStationAirport1 { get; set; }
    public string DepartureStationAirport2 { get; set; }
    public string ArrivalStationAirport2 { get; set; }
    public string DepartureStationAirport3 { get; set; }
    public string ArrivalStationAirport3 { get; set; }
    public string CompanyID { get; set; }
    public int TaxandCharges { get; set; }
    public string GrossF { get; set; }
    public string CarrierName { get; set; }
    public string FlightNumber { get; set; }

}*/



    public class FlightOutBound
    {
        public string Curr { get; set; }
        public bool F_Status { get; set; }
        public string F_Remark { get; set; }
        public string RuleTarrif { get; set; }
        public string InboundOutbound { get; set; }
        public int AgentType { get; set; }
        public string FlightName { get; set; }
        public string Cabin { get; set; }
        public string DepartStationName { get; set; }
        public string ArrivalStationName { get; set; }
        public string DepartureStationAirport { get; set; }
        public string ArrivalStationAirport { get; set; }
        public string DepartureStationAirport1 { get; set; }
        public string ArrivalStationAirport1 { get; set; }
        public string DepartureStationAirport2 { get; set; }
        public string ArrivalStationAirport2 { get; set; }
        public string DepartureStationAirport3 { get; set; }
        public string ArrivalStationAirport3 { get; set; }
        public string CompanyID { get; set; }
        public int TaxandCharges { get; set; }
        public string GrossF { get; set; }
        public string CarrierName { get; set; }
        public string FlightNumber { get; set; }

        public string FlightNumberCmb { get; set; }
        public string FlightDepDate { get; set; }
        public string FlightArrDate { get; set; }
        public string FlightDepTime { get; set; }
        public string FlightArrTime { get; set; }
        public string Stop { get; set; }
        public int NoOFAdult { get; set; }
        public int NoOFChild { get; set; }
        public int NoOFInfant { get; set; }
        public int AdultbaseFare { get; set; }
        public int ChildbaseFare { get; set; }
        public int InfantbaseFare { get; set; }
        public string AdultNumber { get; set; }
        public string ChildNumber { get; set; }
        public string InfantNumber { get; set; }
        public string CHECKINBaggage { get; set; }
        public string CABINBaggage { get; set; }
        public string Adt_BASIC { get; set; }
        public string Chd_BASIC { get; set; }
        public string Inf_BASIC { get; set; }
        public int TotalAmount { get; set; }
        public int FinalFare { get; set; }
        public int TotalFare { get; set; }
        public int TotalTax { get; set; }
        public int TotalBasic { get; set; }
        public int TotalServiceFee { get; set; }
        public int TotalMarkUp { get; set; }
        public int TotalServiceTax { get; set; }
        public string PriceType { get; set; }
        public string TotalCommission { get; set; }
        public string TotalCommission_SA { get; set; }
        public int Adt_YQ { get; set; }
        public string Adt_AcuTax { get; set; }
        public string Chd_AcuTax { get; set; }
        public string Inf_AcuTax { get; set; }
        public int AdtTotalTax { get; set; }
        public int Chd_YQ { get; set; }
        public int ChdTotalTax { get; set; }
        public int Inf_TAX { get; set; }
        public string TotalTds_SA { get; set; }
        public string TotalTds { get; set; }
        public string FlightAmount { get; set; }
        public string BaseAmount { get; set; }
        public string TaxAmount { get; set; }
        public string ServiceFee { get; set; }
        public string AHC { get; set; }
        public string SF { get; set; }
        public string FlightRefid { get; set; }
        public string FlightRefidOnward { get; set; }
        public string FlightRefidReturn { get; set; }
        public string SRC { get; set; }
        public string DEST { get; set; }
        public string PrimarySRC { get; set; }
        public string PrimaryDEST { get; set; }
        public string logo { get; set; }
        public string Duration { get; set; }
        public int connection { get; set; }
        public int ArrivalNextDayCheck { get; set; }
        public string Description { get; set; }
        public string RefundType { get; set; }
        public string AvailableSeat { get; set; }
        public string FlightDepDate1 { get; set; }
        public string FlightArrDate1 { get; set; }
        public string FlightDepTime1 { get; set; }
        public string FlightArrTime1 { get; set; }
        public string FlightDepDate2 { get; set; }
        public string FlightArrDate2 { get; set; }
        public string FlightDepTime2 { get; set; }
        public string FlightArrTime2 { get; set; }
        public string FlightDepDate3 { get; set; }
        public string FlightArrDate3 { get; set; }
        public string FlightDepTime3 { get; set; }
        public string FlightArrTime3 { get; set; }
        public string FlightName1 { get; set; }
        public string DepartStationName1 { get; set; }
        public string ArrivalStationName1 { get; set; }
        public string Cabin1 { get; set; }
        public string CarrierName1 { get; set; }
        public string FlightNumber1 { get; set; }
        public string FlightName2 { get; set; }
        public string DepartStationName2 { get; set; }
        public string ArrivalStationName2 { get; set; }
        public string DepartStationName3 { get; set; }
        public string ArrivalStationName3 { get; set; }
        public string Cabin2 { get; set; }
        public string CarrierName2 { get; set; }
        public string FlightNumber2 { get; set; }
        public string FlightName3 { get; set; }
        public string Cabin3 { get; set; }
        public string CarrierName3 { get; set; }
        public string FlightNumber3 { get; set; }
        public string logo1 { get; set; }
        public string logo2 { get; set; }
        public string Duration1 { get; set; }
        public string Duration2 { get; set; }
        public string Duration3 { get; set; }
        public string Layover1 { get; set; }
        public string Layover2 { get; set; }
        public string Via { get; set; }
        public string Via1 { get; set; }
        public string Via2 { get; set; }
        public string ViaName { get; set; }
        public string ViaName1 { get; set; }
        public string ViaName2 { get; set; }
        public string SRC1 { get; set; }
        public string DEST1 { get; set; }
        public string SRC2 { get; set; }
        public string DEST2 { get; set; }
        public string SRC3 { get; set; }
        public string DEST3 { get; set; }
        public string Class1 { get; set; }
        public string Class2 { get; set; }
        public string Class3 { get; set; }
        public string TerminalSRC { get; set; }
        public string TerminalDEST { get; set; }
        public string TerminalSRC1 { get; set; }
        public string TerminalDEST1 { get; set; }
        public string TerminalSRC2 { get; set; }
        public string TerminalDEST2 { get; set; }
        public string Yq { get; set; }
        public string Dis { get; set; }
        public string Cb { get; set; }
        public int PromoChek { get; set; }
        public string AdtTotalFare { get; set; }
        public string AdtBFare { get; set; }
        public string AdtTax { get; set; }
        public string AdtFCharge { get; set; }
        public string AdtPsf { get; set; }
        public string AdtUdf { get; set; }
        public string AdtTax1 { get; set; }
        public string AdtTax2 { get; set; }
        public string AdtTax3 { get; set; }
        public string AdtTax4 { get; set; }

        public string ChdTotalFare { get; set; }
        public string ChdBFare { get; set; }
        public string ChdTax { get; set; }
        public string ChdFCharge { get; set; }
        public string ChdPsf { get; set; }
        public string ChdUdf { get; set; }
        public string ChdTax1 { get; set; }
        public string ChdTax2 { get; set; }
        public string ChdTax3 { get; set; }
        public string ChdTax4 { get; set; }
        public string InfTotalFare { get; set; }
        public string InfBFare { get; set; }
        public string InfTax { get; set; }
        public string InfFCharge { get; set; }
        public string InfPsf { get; set; }
        public string InfUdf { get; set; }
        public string InfTax1 { get; set; }
        public string InfTax2 { get; set; }
        public string InfTax3 { get; set; }
        public string InfTax4 { get; set; }
        public string FareUpdateMsg { get; set; }
        public int FareUpdateMsgChek { get; set; }
        public string FareRules { get; set; }
        public int SMSACTIVES { get; set; }
        public string FilterTipe { get; set; }
        public string FareRule { get; set; }

        public int _SrNo { get; set; } /// this is for MC
    }
    public class ShowFlightInternational
    {
        public bool F_Status { get; set; }
        public string F_Remark { get; set; }
        public string Class { get; set; }
        public string SRC { get; set; }
        public string DEST { get; set; }
        public string ClassR { get; set; }
        public string Curr { get; set; }


        public string RuleTarrif { get; set; }
        public string InboundOutbound { get; set; }
        public int AgentType { get; set; }
        public string FlightName { get; set; }
        public string FlightNumber { get; set; }
        public string FlightDepDate { get; set; }
        public string FlightArrDate { get; set; }
        public string FlightDepTime { get; set; }
        public string FlightArrTime { get; set; }
        public string Stop { get; set; }
        public string CompanyID { get; set; }
        public int NoOFAdult { get; set; }
        public int NoOFChild { get; set; }
        public int NoOFInfant { get; set; }
        public int AdultbaseFare { get; set; }
        public int ChildbaseFare { get; set; }
        public int InfantbaseFare { get; set; }
        public string Adt_BASIC { get; set; }
        public string Chd_BASIC { get; set; }
        public string Inf_BASIC { get; set; }

        public string AdultNumber { get; set; }
        public string ChildNumber { get; set; }
        public string InfantNumber { get; set; }
        public int TotalAmount { get; set; }
        public int TotalFare { get; set; }

        public int Adt_YQ { get; set; }
        public int AdtTotalTax { get; set; }
        public int Chd_YQ { get; set; }
        public int ChdTotalTax { get; set; }
        public int Inf_TAX { get; set; }
        public string Adt_AcuTax { get; set; }
        public string Chd_AcuTax { get; set; }
        public string Inf_AcuTax { get; set; }
        public int FlightAmount { get; set; }
        public int TotalBasic { get; set; }
        public int TotalTax { get; set; }
        public int TotalServiceFee { get; set; }
        public int TotalMarkup { get; set; }
        public int TotalServiceTax { get; set; }
        public int TaxandCharges { get; set; }
        public string TotalCommission { get; set; }
        public string TotalTds { get; set; }
        public int FinalFare { get; set; }

        public string CHECKINBaggage { get; set; }
        public string CABINBaggage { get; set; }

        public string CHECKINBaggageR { get; set; }
        public string CABINBaggageR { get; set; }



        public string FlightRefid { get; set; }
        public string PrimarySRC { get; set; }
        public string PrimaryDEST { get; set; }
        public string logo { get; set; }
        public string Duration { get; set; }
        public int connection { get; set; }
        public int ArrivalNextDayCheck { get; set; }
        public string Description { get; set; }
        public string RefundType { get; set; }
        public string AvailableSeat { get; set; }

        public string PriceType { get; set; }
        public string PriceTypeR { get; set; }


        public string FlightDepDate1 { get; set; }
        public string FlightArrDate1 { get; set; }
        public string FlightDepTime1 { get; set; }
        public string FlightArrTime1 { get; set; }

        public string FlightDepDate2 { get; set; }
        public string FlightArrDate2 { get; set; }
        public string FlightDepTime2 { get; set; }
        public string FlightArrTime2 { get; set; }

        public string FlightDepDate3 { get; set; }
        public string FlightArrDate3 { get; set; }
        public string FlightDepTime3 { get; set; }
        public string FlightArrTime3 { get; set; }



        public string FlightName1 { get; set; }
        public string FlightNumber1 { get; set; }
        public string FlightName2 { get; set; }
        public string FlightNumber2 { get; set; }
        public string FlightName3 { get; set; }
        public string FlightNumber3 { get; set; }

        public string logo1 { get; set; }
        public string logo2 { get; set; }

        public string DepartStationName1 { get; set; }
        public string DepartStationName2 { get; set; }
        public string DepartStationName3 { get; set; }
        public string ArrivalStationName1 { get; set; }
        public string ArrivalStationName2 { get; set; }
        public string ArrivalStationName3 { get; set; }
        public string DepartureStationAirport1 { get; set; }
        public string DepartureStationAirport2 { get; set; }
        public string DepartureStationAirport3 { get; set; }
        public string ArrivalStationAirport1 { get; set; }
        public string ArrivalStationAirport2 { get; set; }
        public string ArrivalStationAirport3 { get; set; }





        public string DepartStationName { get; set; }
        public string ArrivalStationName { get; set; }
        public string DepartureStationAirport { get; set; }
        public string ArrivalStationAirport { get; set; }

        public string Duration1 { get; set; }
        public string Duration2 { get; set; }
        public string Duration3 { get; set; }

        public string Layover1 { get; set; }
        public string Layover2 { get; set; }

        public string Via { get; set; }
        public string Via1 { get; set; }
        public string Via2 { get; set; }
        public string ViaName2R { get; set; }
        public string ViaName1R { get; set; }
        public string ViaNameR { get; set; }
        public string ViaName2 { get; set; }
        public string ViaName1 { get; set; }
        public string ViaName { get; set; }

        public string SRC1 { get; set; }
        public string DEST1 { get; set; }
        public string SRC2 { get; set; }
        public string DEST2 { get; set; }
        public string SRC3 { get; set; }
        public string DEST3 { get; set; }

        public string Class1 { get; set; }
        public string Class2 { get; set; }
        public string Class3 { get; set; }


        public string Cabin { get; set; }
        public string CabinR { get; set; }
        public string Cabin1 { get; set; }
        public string Cabin2 { get; set; }
        public string Cabin3 { get; set; }
        public string Cabin1R { get; set; }
        public string Cabin2R { get; set; }
        public string Cabin3R { get; set; }
        public string TerminalSRC { get; set; }
        public string TerminalDEST { get; set; }
        public string TerminalSRC1 { get; set; }
        public string TerminalDEST1 { get; set; }
        public string TerminalSRC2 { get; set; }
        public string TerminalDEST2 { get; set; }

        public string Yq { get; set; }
        public string Dis { get; set; }
        public string Cb { get; set; }
        public int PromoChek { get; set; }

        public string AdtTotalFare { get; set; }
        public string AdtBFare { get; set; }
        public string AdtTax { get; set; }
        public string AdtFCharge { get; set; }
        public string AdtPsf { get; set; }
        public string AdtUdf { get; set; }
        public string AdtTax1 { get; set; }
        public string AdtTax2 { get; set; }
        public string AdtTax3 { get; set; }
        public string AdtTax4 { get; set; }

        public string ChdTotalFare { get; set; }
        public string ChdBFare { get; set; }
        public string ChdTax { get; set; }
        public string ChdFCharge { get; set; }
        public string ChdPsf { get; set; }
        public string ChdUdf { get; set; }
        public string ChdTax1 { get; set; }
        public string ChdTax2 { get; set; }
        public string ChdTax3 { get; set; }
        public string ChdTax4 { get; set; }

        public string InfTotalFare { get; set; }
        public string InfBFare { get; set; }
        public string InfTax { get; set; }
        public string InfFCharge { get; set; }
        public string InfPsf { get; set; }
        public string InfUdf { get; set; }
        public string InfTax1 { get; set; }
        public string InfTax2 { get; set; }
        public string InfTax3 { get; set; }
        public string InfTax4 { get; set; }

        public string FareUpdateMsg { get; set; }
        public int FareUpdateMsgChek { get; set; }
        public string FareRules { get; set; }
        public string FareRulesR { get; set; }
        public int SMSACTIVES { get; set; }
        public string FilterTipe { get; set; }

        public string CarrierName { get; set; }
        public string CarrierName1 { get; set; }
        public string CarrierName2 { get; set; }
        public string CarrierName3 { get; set; }
        public string CarrierNameR { get; set; }
        public string CarrierName1R { get; set; }
        public string CarrierName2R { get; set; }
        public string CarrierName3R { get; set; }

        public string FlightNameR { get; set; }
        public string FlightNumberR { get; set; }
        public string FlightDepDateR { get; set; }
        public string FlightArrDateR { get; set; }
        public string FlightDepTimeR { get; set; }
        public string FlightArrTimeR { get; set; }
        public string StopR { get; set; }



        public string DepartStationNameR { get; set; }
        public string ArrivalStationNameR { get; set; }
        public string DepartureStationAirportR { get; set; }
        public string ArrivalStationAirportR { get; set; }

        public string DepartStationName1R { get; set; }
        public string DepartStationName2R { get; set; }
        public string DepartStationName3R { get; set; }
        public string ArrivalStationName1R { get; set; }
        public string ArrivalStationName2R { get; set; }
        public string ArrivalStationName3R { get; set; }
        public string DepartureStationAirport1R { get; set; }
        public string DepartureStationAirport2R { get; set; }
        public string DepartureStationAirport3R { get; set; }
        public string ArrivalStationAirport1R { get; set; }
        public string ArrivalStationAirport2R { get; set; }
        public string ArrivalStationAirport3R { get; set; }


        public string SRCR { get; set; }
        public string DESTR { get; set; }
        public string PrimarySRCR { get; set; }
        public string PrimaryDESTR { get; set; }
        public string logoR { get; set; }
        public string DurationR { get; set; }
        public int connectionR { get; set; }
        public int ArrivalNextDayCheckR { get; set; }


        public string FlightDepDate1R { get; set; }
        public string FlightArrDate1R { get; set; }
        public string FlightDepTime1R { get; set; }
        public string FlightArrTime1R { get; set; }

        public string FlightDepDate2R { get; set; }
        public string FlightArrDate2R { get; set; }
        public string FlightDepTime2R { get; set; }
        public string FlightArrTime2R { get; set; }

        public string FlightDepDate3R { get; set; }
        public string FlightArrDate3R { get; set; }
        public string FlightDepTime3R { get; set; }
        public string FlightArrTime3R { get; set; }



        public string FlightName1R { get; set; }
        public string FlightNumber1R { get; set; }
        public string FlightName2R { get; set; }
        public string FlightNumber2R { get; set; }
        public string FlightName3R { get; set; }
        public string FlightNumber3R { get; set; }

        public string logo1R { get; set; }
        public string logo2R { get; set; }

        public string Duration1R { get; set; }
        public string Duration2R { get; set; }
        public string Duration3R { get; set; }

        public string Layover1R { get; set; }
        public string Layover2R { get; set; }

        public string ViaR { get; set; }
        public string Via1R { get; set; }
        public string Via2R { get; set; }

        public string SRC1R { get; set; }
        public string DEST1R { get; set; }
        public string SRC2R { get; set; }
        public string DEST2R { get; set; }
        public string SRC3R { get; set; }
        public string DEST3R { get; set; }

        public string Class1R { get; set; }
        public string Class2R { get; set; }
        public string Class3R { get; set; }

        public string TerminalSRCR { get; set; }
        public string TerminalDESTR { get; set; }
        public string TerminalSRC1R { get; set; }
        public string TerminalDEST1R { get; set; }
        public string TerminalSRC2R { get; set; }
        public string TerminalDEST2R { get; set; }

        public string DescriptionR { get; set; }
        public string RefundTypeR { get; set; }
        public string AvailableSeatR { get; set; }

        public int _SrNo { get; set; } /// this is for MC

    }//filter airlines
    public class FlightAirlines
    {
        public string Airlines { get; set; }
        public string Airlinespath { get; set; }
        public int NoOfAirlines { get; set; }
    }
    public class FlightStops
    {
        public string Stops { get; set; }
        public int NoOfStops { get; set; }
    }
    public class FlightMatrixs
    {
        public string Stops { get; set; }
        public int NoOfStops { get; set; }
        public string Airlines { get; set; }
        public string Airlinespath { get; set; }
        public string Fare { get; set; }
        public string type { get; set; }

        public string FareM { get; set; }
        public string FareA { get; set; }
        public string FareN { get; set; }
        public string FareMN { get; set; }
        public string FlightRefid { get; set; }

        public string FlightRefidM { get; set; }
        public string FlightRefidA { get; set; }
        public string FlightRefidN { get; set; }
        public string FlightRefidMN { get; set; }
    }
    public class Promo
    {
        public string msgs { get; set; }
    }
    public class Showslide
    {
        public int slideno { get; set; }
        public string slide { get; set; }
        public string slideorder { get; set; }
        public string slidetext { get; set; }
    }
    public class FlightBook
    {
        public int msgs { get; set; }
        public string tabid { get; set; }
    }
}
