namespace ZealTravel.Front.Web.Models.Flight
{
    public class ClsFareResultInfo
    {
        public int AG_Markup;
        public string F_Status_Remark { get; set; }
        public bool FltStatus { get; set; }
        public string AirlineID { get; set; }
        public string BookingFareID { get; set; }
        public int RefID { get; set; }
        public int RowID { get; set; }
        public string API_AirlineID { get; set; }
        public string API_BookingFareID { get; set; }
        public int API_RefID { get; set; }

        public string Origin { get; set; }
        public string Destination { get; set; }

        public string OriginCityName { get; set; }
        public string DestinationCityName { get; set; }
        public string CarrierName { get; set; }
        public string CarrierCode { get; set; }
        public string FlightNumber { get; set; }
        public string DepartureStationAirport { get; set; }

        public string ArrivalStationAirport { get; set; }
        public string DepartureStationName { get; set; }
        public string ArrivalStationName { get; set; }
        public string DepartureStation { get; set; }
        public string ArrivalStation { get; set; }
        public string DepartureDate { get; set; }
        public string ArrivalDate { get; set; }
        public string DepartureTime { get; set; }

        public string ArrivalTime { get; set; }
        public string DepDate { get; set; }
        public string ArrDate { get; set; }
        public string DepTime { get; set; }
        public string ArrTime { get; set; }
        public string JourneyTimeDesc { get; set; }
        public string JourneyTime { get; set; }
        public string Duration { get; set; }

        public string DurationDesc { get; set; }
        public string ProductClass { get; set; }
        public string ClassOfService { get; set; }
        public string FareBasisCode { get; set; }
        public string Cabin { get; set; }
        public string EquipmentType { get; set; }
        public string PriceType { get; set; }
        public int Stops { get; set; }
        public string Classes { get; set; }

        public string ValidFBCode { get; set; }
        public string CancellationFee { get; set; }
        public string DateChangeFee { get; set; }
        public double Chd_CUT { get; set; }
        public int TotalSeat { get; set; }
        public double TotalMeal { get; set; }
        public double TotalBaggage { get; set; }
        public double TotalImport { get; set; }

        public string Via { get; set; }
        public string ViaName { get; set; }
        public int SeatsAvailable { get; set; }
        public string RuleNumber { get; set; }
        public string RuleTarrif { get; set; }
        public string RefundType { get; set; }
        public string FareRule { get; set; }
        public string FareRuledb { get; set; }

        public string BaggageDetail { get; set; }
        public string Api_SessionID { get; set; }
        public string JourneySellKey { get; set; }
        public string FareSellKey { get; set; }
        public string DepartureTerminal { get; set; }
        public string ArrivalTerminal { get; set; }
        public int Adt { get; set; }
        public int Chd { get; set; }

        public int Inf { get; set; }
        public double AdtTotalBasic { get; set; }
        public double AdtTotalTax { get; set; }
        public double AdtTotalFare { get; set; }
        public double Adt_BASIC { get; set; }
        public double Adt_YQ { get; set; }
        public double Adt_PSF { get; set; }
        public double Adt_UDF { get; set; }
        public double Adt_AUDF { get; set; }

        public double Adt_CUTE { get; set; }
        public double Adt_GST { get; set; }
        public double Adt_TF { get; set; }
        public double Adt_CESS { get; set; }
        public double Adt_EX { get; set; }
        public double ChdTotalBasic { get; set; }
        public double ChdTotalTax { get; set; }
        public double ChdTotalFare { get; set; }

        public double Chd_BASIC { get; set; }
        public double Chd_YQ { get; set; }
        public double Chd_PSF { get; set; }
        public double Chd_UDF { get; set; }
        public double Chd_AUDF { get; set; }
        public double Chd_CUTE { get; set; }
        public double Chd_GST { get; set; }
        public double Chd_TF { get; set; }

        public double Chd_CESS { get; set; }
        public double Chd_EX { get; set; }
        public double InfTotalBasic { get; set; }
        public double InfTotalTax { get; set; }
        public double InfTotalFare { get; set; }
        public double Inf_BASIC { get; set; }
        public double Inf_TAX { get; set; }
        public double TotalFare { get; set; }
        public int TotalCfee { get; set; }

        public double TotalBasic { get; set; }
        public double TotalTax { get; set; }
        public double TotalServiceTax { get; set; }
        public double TotalServiceFee { get; set; }
        public double TotalMarkup { get; set; }
        public double TotalCommission { get; set; }
        public double TotalTds { get; set; }

        public double TotalCommission_SA { get; set; }
        public double TotalTds_SA { get; set; }

        public double A_ST { get; set; }

        public double A_TDS { get; set; }
        public double A_BAS { get; set; }
        public double A_YQ { get; set; }
        public double A_CB { get; set; }
        public double A_PR { get; set; }
        public double A_MU { get; set; }
        public double A_SF { get; set; }
        public double C_ST { get; set; }

        public double C_TDS { get; set; }
        public double C_BAS { get; set; }
        public double C_YQ { get; set; }
        public double C_CB { get; set; }
        public double C_PR { get; set; }
        public double C_SF { get; set; }
        public double C_MU { get; set; }
        public string FltType { get; set; }
        public string Sector { get; set; }


        public double Adt_ST { get; set; }
        public double Adt_TDS { get; set; }
        public double Adt_SF { get; set; }
        public double Adt_Import { get; set; }
        public double Adt_BAS { get; set; }
        public double Adt_Y { get; set; }

        public double Adt_CB { get; set; }
        public double Adt_PR { get; set; }
        //--------
        public double Adt_MU { get; set; }
        public double Chd_ST { get; set; }
        public double Chd_TDS { get; set; }
        public double Chd_Import { get; set; }
        public double Chd_SF { get; set; }
        public double Chd_Y { get; set; }
        public double Chd_BAS { get; set; }
        public double Chd_MU { get; set; }
        public double Chd_PR { get; set; }
        public double Chd_CB { get; set; }
        public string Trip { get; set; }
        public string API_SearchID { get; set; }
        public DateTime FilterDepDate { get; set; }
        public int shortJournyTime { get; set; }

    }
}
