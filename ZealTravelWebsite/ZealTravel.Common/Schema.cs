using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZealTravel.Common
{
    public class Schema
    {
        public static DataTable SchemaFlights
        {
            get
            {
                DataTable Table = new DataTable();
                Table.TableName = "AvailabilityInfo";

                Table.Columns.Add("RowID", typeof(Int32)).DefaultValue = 0;
                Table.Columns.Add("AirlineID", typeof(string)).DefaultValue = string.Empty;
                Table.Columns.Add("FlightID", typeof(string)).DefaultValue = string.Empty;
                Table.Columns.Add("RefID", typeof(Int32)).DefaultValue = 0;
                Table.Columns.Add("OrderNo", typeof(Int32)).DefaultValue = 0;

                Table.Columns.Add("Origin", typeof(string)).DefaultValue = string.Empty;
                Table.Columns.Add("Destination", typeof(string)).DefaultValue = string.Empty;

                Table.Columns.Add("CarrierName", typeof(string)).DefaultValue = string.Empty;
                Table.Columns.Add("CarrierCode", typeof(string)).DefaultValue = string.Empty;
                Table.Columns.Add("FlightNumber", typeof(string)).DefaultValue = string.Empty;

                Table.Columns.Add("DepartureStation", typeof(string)).DefaultValue = string.Empty;
                Table.Columns.Add("ArrivalStation", typeof(string)).DefaultValue = string.Empty;

                Table.Columns.Add("DepartureDate", typeof(string)).DefaultValue = string.Empty;
                Table.Columns.Add("ArrivalDate", typeof(string)).DefaultValue = string.Empty;

                Table.Columns.Add("DepartureTime", typeof(string)).DefaultValue = string.Empty;
                Table.Columns.Add("ArrivalTime", typeof(string)).DefaultValue = string.Empty;

                Table.Columns.Add("FareQuote", typeof(string)).DefaultValue = string.Empty;
                Table.Columns.Add("ClassOfService", typeof(string)).DefaultValue = string.Empty;

                Table.Columns.Add("FltType", typeof(string)).DefaultValue = string.Empty;
                Table.Columns.Add("Stops", typeof(Int32)).DefaultValue = 0;
                Table.Columns.Add("Via", typeof(Int32)).DefaultValue = 0;
                Table.Columns.Add("ViaName", typeof(string)).DefaultValue = string.Empty;
                Table.Columns.Add("SeatsAvailable", typeof(Int32)).DefaultValue = 0;

                Table.Columns.Add("Adt", typeof(Int32)).DefaultValue = 0;
                Table.Columns.Add("Chd", typeof(Int32)).DefaultValue = 0;
                Table.Columns.Add("Inf", typeof(Int32)).DefaultValue = 0;

                Table.Columns.Add("AdtTotalBasic", typeof(Decimal)).DefaultValue = 0;
                Table.Columns.Add("AdtTotalTax", typeof(Decimal)).DefaultValue = 0;
                Table.Columns.Add("AdtTotalFare", typeof(Decimal)).DefaultValue = 0;

                Table.Columns.Add("ChdTotalBasic", typeof(Decimal)).DefaultValue = 0;
                Table.Columns.Add("ChdTotalTax", typeof(Decimal)).DefaultValue = 0;
                Table.Columns.Add("ChdTotalFare", typeof(Decimal)).DefaultValue = 0;

                Table.Columns.Add("InfTotalBasic", typeof(Decimal)).DefaultValue = 0;
                Table.Columns.Add("InfTotalTax", typeof(Decimal)).DefaultValue = 0;
                Table.Columns.Add("InfTotalFare", typeof(Decimal)).DefaultValue = 0;

                Table.Columns.Add("TotalFare", typeof(Decimal)).DefaultValue = 0;
                Table.Columns.Add("TotalBasic", typeof(Decimal)).DefaultValue = 0;
                Table.Columns.Add("TotalTax", typeof(Decimal)).DefaultValue = 0;

                Table.Columns.Add("TotalServiceTax", typeof(Decimal)).DefaultValue = 0;
                Table.Columns.Add("TotalServiceFee", typeof(Decimal)).DefaultValue = 0;
                Table.Columns.Add("TotalMarkup", typeof(Decimal)).DefaultValue = 0;
                Table.Columns.Add("TotalCommission", typeof(Decimal)).DefaultValue = 0;
                Table.Columns.Add("TotalCommission_SA", typeof(Decimal)).DefaultValue = 0;
                Table.Columns.Add("TotalTds", typeof(Decimal)).DefaultValue = 0;
                Table.Columns.Add("TotalTds_SA", typeof(Decimal)).DefaultValue = 0;
                Table.Columns.Add("SA_deal", typeof(string)).DefaultValue = "";
                Table.Columns.Add("TotalCfee", typeof(Decimal)).DefaultValue = 0;

                Table.Columns.Add("Adt_BASIC", typeof(Decimal)).DefaultValue = 0;
                Table.Columns.Add("Adt_YQ", typeof(Decimal)).DefaultValue = 0;
                Table.Columns.Add("Adt_PSF", typeof(Decimal)).DefaultValue = 0;
                Table.Columns.Add("Adt_UDF", typeof(Decimal)).DefaultValue = 0;
                Table.Columns.Add("Adt_AUDF", typeof(Decimal)).DefaultValue = 0;
                Table.Columns.Add("Adt_CUTE", typeof(Decimal)).DefaultValue = 0;
                Table.Columns.Add("Adt_GST", typeof(Decimal)).DefaultValue = 0;
                Table.Columns.Add("Adt_TF", typeof(Decimal)).DefaultValue = 0;
                Table.Columns.Add("Adt_CESS", typeof(Decimal)).DefaultValue = 0;
                Table.Columns.Add("Adt_EX", typeof(Decimal)).DefaultValue = 0;

                Table.Columns.Add("Chd_BASIC", typeof(Decimal)).DefaultValue = 0;
                Table.Columns.Add("Chd_YQ", typeof(Decimal)).DefaultValue = 0;
                Table.Columns.Add("Chd_PSF", typeof(Decimal)).DefaultValue = 0;
                Table.Columns.Add("Chd_UDF", typeof(Decimal)).DefaultValue = 0;
                Table.Columns.Add("Chd_AUDF", typeof(Decimal)).DefaultValue = 0;
                Table.Columns.Add("Chd_CUTE", typeof(Decimal)).DefaultValue = 0;
                Table.Columns.Add("Chd_GST", typeof(Decimal)).DefaultValue = 0;
                Table.Columns.Add("Chd_TF", typeof(Decimal)).DefaultValue = 0;
                Table.Columns.Add("Chd_CESS", typeof(Decimal)).DefaultValue = 0;
                Table.Columns.Add("Chd_EX", typeof(Decimal)).DefaultValue = 0;

                Table.Columns.Add("Inf_BASIC", typeof(Decimal)).DefaultValue = 0;
                Table.Columns.Add("Inf_TAX", typeof(Decimal)).DefaultValue = 0;

                Table.Columns.Add("TotalSeat", typeof(Decimal)).DefaultValue = 0;
                Table.Columns.Add("TotalMeal", typeof(Decimal)).DefaultValue = 0;
                Table.Columns.Add("TotalBaggage", typeof(Decimal)).DefaultValue = 0;
                Table.Columns.Add("TotalImport", typeof(Decimal)).DefaultValue = 0;

                Table.Columns.Add("Adt_ST", typeof(Decimal)).DefaultValue = 0;
                Table.Columns.Add("Adt_TDS", typeof(Decimal)).DefaultValue = 0;
                Table.Columns.Add("Adt_SF", typeof(Decimal)).DefaultValue = 0;
                Table.Columns.Add("Adt_Import", typeof(Decimal)).DefaultValue = 0;

                Table.Columns.Add("Adt_BAS", typeof(Decimal)).DefaultValue = 0;
                Table.Columns.Add("Adt_Y", typeof(Decimal)).DefaultValue = 0;
                Table.Columns.Add("Adt_CB", typeof(Decimal)).DefaultValue = 0;
                Table.Columns.Add("Adt_PR", typeof(Decimal)).DefaultValue = 0;
                Table.Columns.Add("Adt_MU", typeof(Decimal)).DefaultValue = 0;

                Table.Columns.Add("Chd_ST", typeof(Decimal)).DefaultValue = 0;
                Table.Columns.Add("Chd_TDS", typeof(Decimal)).DefaultValue = 0;
                Table.Columns.Add("Chd_Import", typeof(Decimal)).DefaultValue = 0;
                Table.Columns.Add("Chd_SF", typeof(Decimal)).DefaultValue = 0;

                Table.Columns.Add("Chd_BAS", typeof(Decimal)).DefaultValue = 0;
                Table.Columns.Add("Chd_Y", typeof(Decimal)).DefaultValue = 0;
                Table.Columns.Add("Chd_CB", typeof(Decimal)).DefaultValue = 0;
                Table.Columns.Add("Chd_PR", typeof(Decimal)).DefaultValue = 0;
                Table.Columns.Add("Chd_MU", typeof(Decimal)).DefaultValue = 0;

                Table.Columns.Add("JourneyTimeDesc", typeof(string)).DefaultValue = string.Empty;
                Table.Columns.Add("DurationDesc", typeof(string)).DefaultValue = 0;

                Table.Columns.Add("DepDate", typeof(string)).DefaultValue = string.Empty;
                Table.Columns.Add("ArrDate", typeof(string)).DefaultValue = string.Empty;

                Table.Columns.Add("DepTime", typeof(string)).DefaultValue = string.Empty;
                Table.Columns.Add("ArrTime", typeof(string)).DefaultValue = string.Empty;

                Table.Columns.Add("ProductClass", typeof(string)).DefaultValue = string.Empty;
                Table.Columns.Add("PriceType", typeof(string)).DefaultValue = string.Empty;

                Table.Columns.Add("DepartureTerminal", typeof(string)).DefaultValue = string.Empty;
                Table.Columns.Add("ArrivalTerminal", typeof(string)).DefaultValue = string.Empty;

                Table.Columns.Add("Classes", typeof(string)).DefaultValue = string.Empty;
                Table.Columns.Add("FareBasisCode", typeof(string)).DefaultValue = string.Empty;
                Table.Columns.Add("ValidFBCode", typeof(bool)).DefaultValue = false;
                Table.Columns.Add("FareStatus", typeof(string)).DefaultValue = true;
                Table.Columns.Add("IsPriceChanged", typeof(string)).DefaultValue = false;
                Table.Columns.Add("Cabin", typeof(string)).DefaultValue = string.Empty;
                Table.Columns.Add("EquipmentType", typeof(string)).DefaultValue = string.Empty;

                Table.Columns.Add("RuleNumber", typeof(string)).DefaultValue = string.Empty;
                Table.Columns.Add("RuleTarrif", typeof(string)).DefaultValue = string.Empty;
                Table.Columns.Add("RefundType", typeof(string)).DefaultValue = string.Empty;

                Table.Columns.Add("FareRuledb", typeof(string)).DefaultValue = string.Empty;
                Table.Columns.Add("BaggageDetail", typeof(string)).DefaultValue = string.Empty;

                Table.Columns.Add("AG_Markup", typeof(Int32)).DefaultValue = 0;

                Table.Columns.Add("CancellationFee", typeof(string)).DefaultValue = string.Empty;
                Table.Columns.Add("DateChangeFee", typeof(string)).DefaultValue = string.Empty;

                Table.Columns.Add("Trip", typeof(string)).DefaultValue = string.Empty;
                Table.Columns.Add("Sector", typeof(string)).DefaultValue = string.Empty;


                Table.Columns.Add("BookingFareID", typeof(string)).DefaultValue = string.Empty;

                Table.Columns.Add("API_AirlineID", typeof(string)).DefaultValue = string.Empty;
                Table.Columns.Add("API_BookingFareID", typeof(string)).DefaultValue = string.Empty;
                Table.Columns.Add("API_RefID", typeof(Int32)).DefaultValue = 0;
                Table.Columns.Add("API_SearchID", typeof(string)).DefaultValue = string.Empty;

                Table.Columns.Add("Api_SessionID", typeof(string)).DefaultValue = string.Empty;
                Table.Columns.Add("JourneySellKey", typeof(string)).DefaultValue = string.Empty;
                Table.Columns.Add("FareSellKey", typeof(string)).DefaultValue = string.Empty;

                Table.Columns.Add("JourneyTime", typeof(Int32)).DefaultValue = 0;
                Table.Columns.Add("Duration", typeof(Int32)).DefaultValue = 0;

                Table.Columns.Add("DepartureStationAirport", typeof(string)).DefaultValue = string.Empty;
                Table.Columns.Add("ArrivalStationAirport", typeof(string)).DefaultValue = string.Empty;

                Table.Columns.Add("DepartureStationName", typeof(string)).DefaultValue = string.Empty;
                Table.Columns.Add("ArrivalStationName", typeof(string)).DefaultValue = string.Empty;

                Table.Columns.Add("FareRule", typeof(string)).DefaultValue = string.Empty;
                Table.Columns.Add("TempData1", typeof(string)).DefaultValue = string.Empty;
                Table.Columns.Add("TempData2", typeof(string)).DefaultValue = string.Empty;
                Table.Columns.Add("TempData3", typeof(string)).DefaultValue = string.Empty;




                Table.Columns.Add("Group", typeof(string)).DefaultValue = string.Empty;
                Table.Columns.Add("FlightTime", typeof(string)).DefaultValue = string.Empty;
                Table.Columns.Add("Distance", typeof(string)).DefaultValue = string.Empty;
                Table.Columns.Add("ChangeOfPlane", typeof(string)).DefaultValue = string.Empty;
                Table.Columns.Add("ParticipantLevel", typeof(string)).DefaultValue = string.Empty;
                Table.Columns.Add("LinkAvailability", typeof(string)).DefaultValue = string.Empty;
                Table.Columns.Add("PolledAvailabilityOption", typeof(string)).DefaultValue = string.Empty;
                Table.Columns.Add("OptionalServicesIndicator", typeof(string)).DefaultValue = string.Empty;
                Table.Columns.Add("AvailabilitySource", typeof(string)).DefaultValue = string.Empty;
                Table.Columns.Add("AvailabilityDisplayType", typeof(string)).DefaultValue = string.Empty;

                Table.Columns.Add("CodeshareInfoOperatingCarrier", typeof(string)).DefaultValue = string.Empty;
                Table.Columns.Add("CodeshareInfoOperatingFlightNumber", typeof(string)).DefaultValue = string.Empty;
                Table.Columns.Add("CodeshareInfo", typeof(string)).DefaultValue = string.Empty;


                Table.Columns.Add("TravelTime", typeof(string)).DefaultValue = string.Empty;
                Table.Columns.Add("BookingCode", typeof(string)).DefaultValue = string.Empty;
                //Table.Columns.Add("BookingCount", typeof(string)).DefaultValue = string.Empty;
                Table.Columns.Add("CabinClass", typeof(string)).DefaultValue = string.Empty;

                Table.Columns.Add("LatestTicketingTime", typeof(string)).DefaultValue = string.Empty;
                Table.Columns.Add("PricingMethod", typeof(string)).DefaultValue = string.Empty;
                Table.Columns.Add("Refundable", typeof(string)).DefaultValue = string.Empty;
                Table.Columns.Add("ETicketability", typeof(string)).DefaultValue = string.Empty;
                Table.Columns.Add("PlatingCarrier", typeof(string)).DefaultValue = string.Empty;
                Table.Columns.Add("ProviderCode", typeof(string)).DefaultValue = string.Empty;
                Table.Columns.Add("Cat35Indicator", typeof(string)).DefaultValue = string.Empty;

                Table.Columns.Add("AirPricePointKey", typeof(string)).DefaultValue = string.Empty;//single      
                Table.Columns.Add("SegmentRef", typeof(string)).DefaultValue = string.Empty;//single
                Table.Columns.Add("FlightDetailsRefKey", typeof(string)).DefaultValue = string.Empty;//single
                Table.Columns.Add("AdtAirPricingInfoKey", typeof(string)).DefaultValue = string.Empty;
                Table.Columns.Add("ChdAirPricingInfoKey", typeof(string)).DefaultValue = string.Empty;
                Table.Columns.Add("InfAirPricingInfoKey", typeof(string)).DefaultValue = string.Empty;
                Table.Columns.Add("AdtFareInfoRef", typeof(string)).DefaultValue = string.Empty;
                Table.Columns.Add("ChdFareInfoRef", typeof(string)).DefaultValue = string.Empty;
                Table.Columns.Add("InfFareInfoRef", typeof(string)).DefaultValue = string.Empty;
                Table.Columns.Add("AdtOptionKey", typeof(string)).DefaultValue = string.Empty;
                Table.Columns.Add("ChdOptionKey", typeof(string)).DefaultValue = string.Empty;
                Table.Columns.Add("InfOptionKey", typeof(string)).DefaultValue = string.Empty;
                Table.Columns.Add("AdtTaxes", typeof(string)).DefaultValue = string.Empty;
                Table.Columns.Add("ChdTaxes", typeof(string)).DefaultValue = string.Empty;
                Table.Columns.Add("InfTaxes", typeof(string)).DefaultValue = string.Empty;

                Table.Columns.Add("AdtHTR", typeof(string)).DefaultValue = string.Empty;
                Table.Columns.Add("ChdHTR", typeof(string)).DefaultValue = string.Empty;
                Table.Columns.Add("InfHTR", typeof(string)).DefaultValue = string.Empty;

                Table.Columns.Add("HostTokenRef", typeof(string)).DefaultValue = string.Empty;

                Table.Columns.Add("FareRuleInfo_Text", typeof(string)).DefaultValue = string.Empty;

                Table.Columns.Add("AdtFareInfoRef_data", typeof(string)).DefaultValue = string.Empty;
                Table.Columns.Add("ChdFareInfoRef_data", typeof(string)).DefaultValue = string.Empty;
                Table.Columns.Add("InfFareInfoRef_data", typeof(string)).DefaultValue = string.Empty;

                Table.Columns.Add("AdtBTR", typeof(string)).DefaultValue = string.Empty;
                Table.Columns.Add("ChdBTR", typeof(string)).DefaultValue = string.Empty;
                Table.Columns.Add("InfBTR", typeof(string)).DefaultValue = string.Empty;

                Table.Columns.Add("Connection_O", typeof(string)).DefaultValue = string.Empty;
                Table.Columns.Add("Connection_I", typeof(string)).DefaultValue = string.Empty;
                return Table;
            }
        }
        public static DataTable SchemaConfirmation
        {
            get
            {
                DataTable Table = new DataTable();
                Table.TableName = "BookingInfo";
                Table.Columns.Add("Status", typeof(bool)).DefaultValue = false;
                Table.Columns.Add("AirlinePNR", typeof(string)).DefaultValue = string.Empty;
                Table.Columns.Add("PNR", typeof(string)).DefaultValue = string.Empty;
                Table.Columns.Add("FltType", typeof(string)).DefaultValue = string.Empty;
                Table.Columns.Add("TktNumber", typeof(string)).DefaultValue = string.Empty;
                return Table;
            }
        }
        public static DataTable SchemaSSR
        {
            get
            {
                DataTable Table = new DataTable();
                Table.TableName = "SSRInfo";

                Table.Columns.Add("RowID", typeof(Int32)).DefaultValue = 0;
                Table.Columns.Add("CarrierCode", typeof(string)).DefaultValue = string.Empty;
                Table.Columns.Add("Code", typeof(string)).DefaultValue = string.Empty;
                Table.Columns.Add("CodeType", typeof(string)).DefaultValue = string.Empty;
                Table.Columns.Add("Amount", typeof(Int32)).DefaultValue = 0;
                Table.Columns.Add("Description", typeof(string)).DefaultValue = string.Empty;
                Table.Columns.Add("Detail", typeof(string)).DefaultValue = string.Empty;
                Table.Columns.Add("FltType", typeof(string)).DefaultValue = string.Empty;
                Table.Columns.Add("DepartureStation", typeof(string)).DefaultValue = string.Empty;
                Table.Columns.Add("ArrivalStation", typeof(string)).DefaultValue = string.Empty;
                Table.Columns.Add("Temp1", typeof(string)).DefaultValue = string.Empty;
                Table.Columns.Add("Temp2", typeof(string)).DefaultValue = string.Empty;
                Table.Columns.Add("Temp3", typeof(string)).DefaultValue = string.Empty;
                Table.Columns.Add("Temp4", typeof(string)).DefaultValue = string.Empty;
                Table.Columns.Add("Temp5", typeof(string)).DefaultValue = string.Empty;

                return Table;
            }
        }
        //public static DataTable SchemaFlights
        //{
        //    get
        //    {
        //        DataTable Table = new DataTable();
        //        Table.TableName = "AvailabilityInfo";

        //        Table.Columns.Add("RowID", typeof(Int32)).DefaultValue = 0;
        //        Table.Columns.Add("AirlineID", typeof(string)).DefaultValue = string.Empty;
        //        Table.Columns.Add("FlightID", typeof(string)).DefaultValue = string.Empty;
        //        Table.Columns.Add("RefID", typeof(Int32)).DefaultValue = 0;
        //        Table.Columns.Add("OrderNo", typeof(Int32)).DefaultValue = 0;

        //        Table.Columns.Add("Origin", typeof(string)).DefaultValue = string.Empty;
        //        Table.Columns.Add("Destination", typeof(string)).DefaultValue = string.Empty;

        //        Table.Columns.Add("CarrierName", typeof(string)).DefaultValue = string.Empty;
        //        Table.Columns.Add("CarrierCode", typeof(string)).DefaultValue = string.Empty;
        //        Table.Columns.Add("FlightNumber", typeof(string)).DefaultValue = string.Empty;

        //        Table.Columns.Add("DepartureStation", typeof(string)).DefaultValue = string.Empty;
        //        Table.Columns.Add("ArrivalStation", typeof(string)).DefaultValue = string.Empty;

        //        Table.Columns.Add("DepartureDate", typeof(string)).DefaultValue = string.Empty;
        //        Table.Columns.Add("ArrivalDate", typeof(string)).DefaultValue = string.Empty;

        //        Table.Columns.Add("DepartureTime", typeof(string)).DefaultValue = string.Empty;
        //        Table.Columns.Add("ArrivalTime", typeof(string)).DefaultValue = string.Empty;

        //        Table.Columns.Add("FareQuote", typeof(string)).DefaultValue = string.Empty;
        //        Table.Columns.Add("ClassOfService", typeof(string)).DefaultValue = string.Empty;

        //        Table.Columns.Add("FltType", typeof(string)).DefaultValue = string.Empty;
        //        Table.Columns.Add("Stops", typeof(Int32)).DefaultValue = 0;
        //        Table.Columns.Add("Via", typeof(Int32)).DefaultValue = 0;
        //        Table.Columns.Add("ViaName", typeof(string)).DefaultValue = string.Empty;
        //        Table.Columns.Add("SeatsAvailable", typeof(Int32)).DefaultValue = 0;

        //        Table.Columns.Add("Adt", typeof(Int32)).DefaultValue = 0;
        //        Table.Columns.Add("Chd", typeof(Int32)).DefaultValue = 0;
        //        Table.Columns.Add("Inf", typeof(Int32)).DefaultValue = 0;

        //        Table.Columns.Add("AdtTotalBasic", typeof(Decimal)).DefaultValue = 0;
        //        Table.Columns.Add("AdtTotalTax", typeof(Decimal)).DefaultValue = 0;
        //        Table.Columns.Add("AdtTotalFare", typeof(Decimal)).DefaultValue = 0;

        //        Table.Columns.Add("ChdTotalBasic", typeof(Decimal)).DefaultValue = 0;
        //        Table.Columns.Add("ChdTotalTax", typeof(Decimal)).DefaultValue = 0;
        //        Table.Columns.Add("ChdTotalFare", typeof(Decimal)).DefaultValue = 0;

        //        Table.Columns.Add("InfTotalBasic", typeof(Decimal)).DefaultValue = 0;
        //        Table.Columns.Add("InfTotalTax", typeof(Decimal)).DefaultValue = 0;
        //        Table.Columns.Add("InfTotalFare", typeof(Decimal)).DefaultValue = 0;

        //        Table.Columns.Add("TotalFare", typeof(Decimal)).DefaultValue = 0;
        //        Table.Columns.Add("TotalBasic", typeof(Decimal)).DefaultValue = 0;
        //        Table.Columns.Add("TotalTax", typeof(Decimal)).DefaultValue = 0;

        //        Table.Columns.Add("TotalServiceTax", typeof(Decimal)).DefaultValue = 0;
        //        Table.Columns.Add("TotalServiceFee", typeof(Decimal)).DefaultValue = 0;
        //        Table.Columns.Add("TotalMarkup", typeof(Decimal)).DefaultValue = 0;
        //        Table.Columns.Add("TotalCommission", typeof(Decimal)).DefaultValue = 0;
        //        Table.Columns.Add("TotalCommission_SA", typeof(Decimal)).DefaultValue = 0;
        //        Table.Columns.Add("TotalTds", typeof(Decimal)).DefaultValue = 0;
        //        Table.Columns.Add("TotalTds_SA", typeof(Decimal)).DefaultValue = 0;
        //        Table.Columns.Add("SA_deal", typeof(string)).DefaultValue = "";
        //        Table.Columns.Add("TotalCfee", typeof(Decimal)).DefaultValue = 0;

        //        Table.Columns.Add("Adt_BASIC", typeof(Decimal)).DefaultValue = 0;
        //        Table.Columns.Add("Adt_YQ", typeof(Decimal)).DefaultValue = 0;
        //        Table.Columns.Add("Adt_PSF", typeof(Decimal)).DefaultValue = 0;
        //        Table.Columns.Add("Adt_UDF", typeof(Decimal)).DefaultValue = 0;
        //        Table.Columns.Add("Adt_AUDF", typeof(Decimal)).DefaultValue = 0;
        //        Table.Columns.Add("Adt_CUTE", typeof(Decimal)).DefaultValue = 0;
        //        Table.Columns.Add("Adt_GST", typeof(Decimal)).DefaultValue = 0;
        //        Table.Columns.Add("Adt_TF", typeof(Decimal)).DefaultValue = 0;
        //        Table.Columns.Add("Adt_CESS", typeof(Decimal)).DefaultValue = 0;
        //        Table.Columns.Add("Adt_EX", typeof(Decimal)).DefaultValue = 0;

        //        Table.Columns.Add("Chd_BASIC", typeof(Decimal)).DefaultValue = 0;
        //        Table.Columns.Add("Chd_YQ", typeof(Decimal)).DefaultValue = 0;
        //        Table.Columns.Add("Chd_PSF", typeof(Decimal)).DefaultValue = 0;
        //        Table.Columns.Add("Chd_UDF", typeof(Decimal)).DefaultValue = 0;
        //        Table.Columns.Add("Chd_AUDF", typeof(Decimal)).DefaultValue = 0;
        //        Table.Columns.Add("Chd_CUTE", typeof(Decimal)).DefaultValue = 0;
        //        Table.Columns.Add("Chd_GST", typeof(Decimal)).DefaultValue = 0;
        //        Table.Columns.Add("Chd_TF", typeof(Decimal)).DefaultValue = 0;
        //        Table.Columns.Add("Chd_CESS", typeof(Decimal)).DefaultValue = 0;
        //        Table.Columns.Add("Chd_EX", typeof(Decimal)).DefaultValue = 0;

        //        Table.Columns.Add("Inf_BASIC", typeof(Decimal)).DefaultValue = 0;
        //        Table.Columns.Add("Inf_TAX", typeof(Decimal)).DefaultValue = 0;

        //        Table.Columns.Add("TotalSeat", typeof(Decimal)).DefaultValue = 0;
        //        Table.Columns.Add("TotalMeal", typeof(Decimal)).DefaultValue = 0;
        //        Table.Columns.Add("TotalBaggage", typeof(Decimal)).DefaultValue = 0;
        //        Table.Columns.Add("TotalImport", typeof(Decimal)).DefaultValue = 0;

        //        Table.Columns.Add("Adt_ST", typeof(Decimal)).DefaultValue = 0;
        //        Table.Columns.Add("Adt_TDS", typeof(Decimal)).DefaultValue = 0;
        //        Table.Columns.Add("Adt_SF", typeof(Decimal)).DefaultValue = 0;
        //        Table.Columns.Add("Adt_Import", typeof(Decimal)).DefaultValue = 0;

        //        Table.Columns.Add("Adt_BAS", typeof(Decimal)).DefaultValue = 0;
        //        Table.Columns.Add("Adt_Y", typeof(Decimal)).DefaultValue = 0;
        //        Table.Columns.Add("Adt_CB", typeof(Decimal)).DefaultValue = 0;
        //        Table.Columns.Add("Adt_PR", typeof(Decimal)).DefaultValue = 0;
        //        Table.Columns.Add("Adt_MU", typeof(Decimal)).DefaultValue = 0;

        //        Table.Columns.Add("Chd_ST", typeof(Decimal)).DefaultValue = 0;
        //        Table.Columns.Add("Chd_TDS", typeof(Decimal)).DefaultValue = 0;
        //        Table.Columns.Add("Chd_Import", typeof(Decimal)).DefaultValue = 0;
        //        Table.Columns.Add("Chd_SF", typeof(Decimal)).DefaultValue = 0;

        //        Table.Columns.Add("Chd_BAS", typeof(Decimal)).DefaultValue = 0;
        //        Table.Columns.Add("Chd_Y", typeof(Decimal)).DefaultValue = 0;
        //        Table.Columns.Add("Chd_CB", typeof(Decimal)).DefaultValue = 0;
        //        Table.Columns.Add("Chd_PR", typeof(Decimal)).DefaultValue = 0;
        //        Table.Columns.Add("Chd_MU", typeof(Decimal)).DefaultValue = 0;

        //        Table.Columns.Add("JourneyTimeDesc", typeof(string)).DefaultValue = string.Empty;
        //        Table.Columns.Add("DurationDesc", typeof(string)).DefaultValue = 0;

        //        Table.Columns.Add("DepDate", typeof(string)).DefaultValue = string.Empty;
        //        Table.Columns.Add("ArrDate", typeof(string)).DefaultValue = string.Empty;

        //        Table.Columns.Add("DepTime", typeof(string)).DefaultValue = string.Empty;
        //        Table.Columns.Add("ArrTime", typeof(string)).DefaultValue = string.Empty;

        //        Table.Columns.Add("ProductClass", typeof(string)).DefaultValue = string.Empty;
        //        Table.Columns.Add("PriceType", typeof(string)).DefaultValue = string.Empty;

        //        Table.Columns.Add("DepartureTerminal", typeof(string)).DefaultValue = string.Empty;
        //        Table.Columns.Add("ArrivalTerminal", typeof(string)).DefaultValue = string.Empty;

        //        Table.Columns.Add("Classes", typeof(string)).DefaultValue = string.Empty;
        //        Table.Columns.Add("FareBasisCode", typeof(string)).DefaultValue = string.Empty;
        //        Table.Columns.Add("ValidFBCode", typeof(bool)).DefaultValue = false;
        //        Table.Columns.Add("FareStatus", typeof(string)).DefaultValue = true;
        //        Table.Columns.Add("IsPriceChanged", typeof(string)).DefaultValue = false;
        //        Table.Columns.Add("Cabin", typeof(string)).DefaultValue = string.Empty;
        //        Table.Columns.Add("EquipmentType", typeof(string)).DefaultValue = string.Empty;

        //        Table.Columns.Add("RuleNumber", typeof(string)).DefaultValue = string.Empty;
        //        Table.Columns.Add("RuleTarrif", typeof(string)).DefaultValue = string.Empty;
        //        Table.Columns.Add("RefundType", typeof(string)).DefaultValue = string.Empty;

        //        Table.Columns.Add("FareRuledb", typeof(string)).DefaultValue = string.Empty;
        //        Table.Columns.Add("BaggageDetail", typeof(string)).DefaultValue = string.Empty;

        //        Table.Columns.Add("AG_Markup", typeof(Int32)).DefaultValue = 0;

        //        Table.Columns.Add("CancellationFee", typeof(Int32)).DefaultValue = 0;
        //        Table.Columns.Add("DateChangeFee", typeof(Int32)).DefaultValue = 0;

        //        Table.Columns.Add("Trip", typeof(string)).DefaultValue = string.Empty;
        //        Table.Columns.Add("Sector", typeof(string)).DefaultValue = string.Empty;


        //        Table.Columns.Add("BookingFareID", typeof(string)).DefaultValue = string.Empty;

        //        Table.Columns.Add("API_AirlineID", typeof(string)).DefaultValue = string.Empty;
        //        Table.Columns.Add("API_BookingFareID", typeof(string)).DefaultValue = string.Empty;
        //        Table.Columns.Add("API_RefID", typeof(Int32)).DefaultValue = 0;
        //        Table.Columns.Add("API_SearchID", typeof(string)).DefaultValue = string.Empty;

        //        Table.Columns.Add("Api_SessionID", typeof(string)).DefaultValue = string.Empty;
        //        Table.Columns.Add("JourneySellKey", typeof(string)).DefaultValue = string.Empty;
        //        Table.Columns.Add("FareSellKey", typeof(string)).DefaultValue = string.Empty;

        //        Table.Columns.Add("JourneyTime", typeof(Int32)).DefaultValue = 0;
        //        Table.Columns.Add("Duration", typeof(Int32)).DefaultValue = 0;

        //        Table.Columns.Add("DepartureStationAirport", typeof(string)).DefaultValue = string.Empty;
        //        Table.Columns.Add("ArrivalStationAirport", typeof(string)).DefaultValue = string.Empty;

        //        Table.Columns.Add("DepartureStationName", typeof(string)).DefaultValue = string.Empty;
        //        Table.Columns.Add("ArrivalStationName", typeof(string)).DefaultValue = string.Empty;

        //        Table.Columns.Add("FareRule", typeof(string)).DefaultValue = string.Empty;
        //        Table.Columns.Add("TempData1", typeof(string)).DefaultValue = string.Empty;
        //        Table.Columns.Add("TempData2", typeof(string)).DefaultValue = string.Empty;
        //        Table.Columns.Add("TempData3", typeof(string)).DefaultValue = string.Empty;

        //        return Table;
        //    }
        //}
        public static DataTable SchemaPassengers
        {
            get
            {
                DataTable Table = new DataTable();
                Table.TableName = "PassengerInfo";
                Table.Columns.Add("RowID", typeof(Int32)).DefaultValue = 0;

                Table.Columns.Add("PaxType", typeof(string)).DefaultValue = string.Empty;
                Table.Columns.Add("Title", typeof(string)).DefaultValue = string.Empty;
                Table.Columns.Add("First_Name", typeof(string)).DefaultValue = string.Empty;
                Table.Columns.Add("Middle_Name", typeof(string)).DefaultValue = string.Empty;
                Table.Columns.Add("Last_Name", typeof(string)).DefaultValue = string.Empty;
                Table.Columns.Add("DOB", typeof(string)).DefaultValue = string.Empty;

                Table.Columns.Add("MealDesc_O", typeof(string)).DefaultValue = string.Empty;
                Table.Columns.Add("MealCode_O", typeof(string)).DefaultValue = string.Empty;
                Table.Columns.Add("MealChg_O", typeof(Int32)).DefaultValue = 0;
                Table.Columns.Add("MealDetail_O", typeof(string)).DefaultValue = 0;

                Table.Columns.Add("MealDesc_I", typeof(string)).DefaultValue = string.Empty;
                Table.Columns.Add("MealCode_I", typeof(string)).DefaultValue = string.Empty;
                Table.Columns.Add("MealChg_I", typeof(Int32)).DefaultValue = 0;
                Table.Columns.Add("MealDetail_I", typeof(string)).DefaultValue = 0;

                Table.Columns.Add("BaggageDesc_O", typeof(string)).DefaultValue = string.Empty;
                Table.Columns.Add("BaggageCode_O", typeof(string)).DefaultValue = string.Empty;
                Table.Columns.Add("BaggageChg_O", typeof(Int32)).DefaultValue = 0;
                Table.Columns.Add("BaggageDetail_O", typeof(string)).DefaultValue = 0;

                Table.Columns.Add("BaggageDesc_I", typeof(string)).DefaultValue = string.Empty;
                Table.Columns.Add("BaggageCode_I", typeof(string)).DefaultValue = string.Empty;
                Table.Columns.Add("BaggageChg_I", typeof(Int32)).DefaultValue = 0;
                Table.Columns.Add("BaggageDetail_I", typeof(string)).DefaultValue = 0;

                //Table.Columns.Add("SeatDesc_O", typeof(string)).DefaultValue = string.Empty;
                //Table.Columns.Add("SeatCode_O", typeof(string)).DefaultValue = string.Empty;
                //Table.Columns.Add("SeatChg_O", typeof(Int32)).DefaultValue = 0;

                //Table.Columns.Add("SeatDesc_I", typeof(string)).DefaultValue = string.Empty;
                //Table.Columns.Add("SeatCode_I", typeof(string)).DefaultValue = string.Empty;
                //Table.Columns.Add("SeatChg_I", typeof(Int32)).DefaultValue = 0;

                Table.Columns.Add("Email", typeof(string)).DefaultValue = string.Empty;
                Table.Columns.Add("MobileNo", typeof(string)).DefaultValue = string.Empty;

                //Table.Columns.Add("City", typeof(string)).DefaultValue = string.Empty;
                //Table.Columns.Add("State", typeof(string)).DefaultValue = string.Empty;
                //Table.Columns.Add("LandLine", typeof(string)).DefaultValue = string.Empty;
                //Table.Columns.Add("Address", typeof(string)).DefaultValue = string.Empty;

                Table.Columns.Add("Nationality", typeof(string)).DefaultValue = string.Empty;
                Table.Columns.Add("FFN", typeof(string)).DefaultValue = string.Empty;
                Table.Columns.Add("TourCode", typeof(string)).DefaultValue = string.Empty;

                Table.Columns.Add("PpNumber", typeof(string)).DefaultValue = string.Empty;
                Table.Columns.Add("PPIssueDate", typeof(string)).DefaultValue = string.Empty;
                Table.Columns.Add("PPExpirayDate", typeof(string)).DefaultValue = string.Empty;
                Table.Columns.Add("PpCountry", typeof(string)).DefaultValue = string.Empty;

                //Table.Columns.Add("GSTCompanyName", typeof(string)).DefaultValue = string.Empty;
                //Table.Columns.Add("GSTNumber", typeof(string)).DefaultValue = string.Empty;
                //Table.Columns.Add("GSTCompanyEmail", typeof(string)).DefaultValue = string.Empty;
                //Table.Columns.Add("GSTCompanyContactNumber", typeof(string)).DefaultValue = string.Empty;
                //Table.Columns.Add("GSTCompanyAddress", typeof(string)).DefaultValue = string.Empty;

                return Table;
            }
        }
        public static DataTable SchemaCompanyInfo
        {
            get
            {
                DataTable Table = new DataTable();
                Table.TableName = "CompanyInfo";
                Table.Columns.Add("CompanyName", typeof(string)).DefaultValue = string.Empty;
                Table.Columns.Add("PostalCode", typeof(string)).DefaultValue = string.Empty;
                Table.Columns.Add("StateCode", typeof(string)).DefaultValue = string.Empty;
                Table.Columns.Add("CountryCode", typeof(string)).DefaultValue = string.Empty;
                Table.Columns.Add("CountryName", typeof(string)).DefaultValue = string.Empty;
                Table.Columns.Add("Email", typeof(string)).DefaultValue = string.Empty;
                Table.Columns.Add("CityName", typeof(string)).DefaultValue = string.Empty;
                Table.Columns.Add("MobileNo", typeof(string)).DefaultValue = string.Empty;
                Table.Columns.Add("PhoneNo", typeof(string)).DefaultValue = string.Empty;
                Table.Columns.Add("Address", typeof(string)).DefaultValue = string.Empty;
                return Table;
            }
        }
        public static DataTable SchemaGstInfo
        {
            get
            {
                DataTable Table = new DataTable();
                Table.TableName = "GstInfo";
                Table.Columns.Add("GSTNumber", typeof(string)).DefaultValue = string.Empty;
                Table.Columns.Add("GSTCompanyName", typeof(string)).DefaultValue = string.Empty;
                Table.Columns.Add("GSTCompanyEmail", typeof(string)).DefaultValue = string.Empty;
                Table.Columns.Add("GSTCompanyContactNumber", typeof(string)).DefaultValue = string.Empty;
                Table.Columns.Add("GSTCompanyAddress", typeof(string)).DefaultValue = string.Empty;
                return Table;
            }
        }
        public static DataTable SchemaFareRule
        {
            get
            {
                DataTable Table = new DataTable();
                Table.TableName = "FareRuleInfo";

                Table.Columns.Add("Origin", typeof(string)).DefaultValue = string.Empty;
                Table.Columns.Add("Destination", typeof(string)).DefaultValue = string.Empty;
                Table.Columns.Add("RuleDetail", typeof(string)).DefaultValue = string.Empty;

                return Table;
            }
        }
    }
}
