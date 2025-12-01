using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Xml;

namespace ZealTravel.Infrastructure.UAPI
{
    class GetAvailabilityFunctions
    {
        //public static string GetMulticitySector(string TPSQ)
        //{
        //    bool bDomestic = true;
        //    DataSet dsRequest = DBCommon.CommonFunction.StringToDataSet(TPSQ);
        //    foreach (DataRow dr in dsRequest.Tables["AirSrchInfo"].Rows)
        //    {
        //        DBCommon.Airline_Detail objad = new DBCommon.Airline_Detail();
        //        bDomestic = objad.IsDomestic(dr["DepartureStation"].ToString(), dr["ArrivalStation"].ToString());
        //        if (bDomestic.Equals(false))
        //        {
        //            break;
        //        }
        //    }

        //    if (bDomestic)
        //    {
        //        return "D";
        //    }
        //    else
        //    {
        //        return "I";
        //    }
        //}
        public static string GetFilterDate(string Date)
        {
            string yyyy = Date.Substring(0, 4);
            string mm = Date.Substring(4, 2);
            string dd = Date.Substring(6, 2);

            return yyyy + "-" + mm + "-" + dd;
        }
        //public void SetFare1(string SearchID, string CompanyID, DataRow drAdd, string AirPricingInfo_Id, DataSet dsResponse, string BookingCode, string SegmentRef)
        //{
        //    try
        //    {
        //        int Adt = Convert.ToInt32(drAdd["Adt"].ToString());
        //        int Chd = Convert.ToInt32(drAdd["Chd"].ToString());
        //        int Inf = Convert.ToInt32(drAdd["Inf"].ToString());

        //        string PaxType = dsResponse.Tables["PassengerType"].Select("AirPricingInfo_Id='" + AirPricingInfo_Id + "'").CopyToDataTable().Rows[0]["Code"].ToString();
        //        if (PaxType.Equals("ADT"))
        //        {
        //            SetFare2(SearchID, CompanyID, true, false, false, AirPricingInfo_Id, drAdd, dsResponse, BookingCode, SegmentRef);
        //        }

        //        if (Chd > 0 || Inf > 0)
        //        {
        //            if (Chd > 0 && Inf > 0)
        //            {
        //                int Chd_AirPricingInfo_Id = Convert.ToInt32(AirPricingInfo_Id) + 1;
        //                //==========================================================================================================================================
        //                if (Chd > 0)
        //                {
        //                    PaxType = dsResponse.Tables["PassengerType"].Select("AirPricingInfo_Id='" + Chd_AirPricingInfo_Id.ToString() + "'").CopyToDataTable().Rows[0]["Code"].ToString();
        //                    if (PaxType.Equals("CNN") || PaxType.Equals("CHD"))
        //                    {
        //                        SetFare2(SearchID, CompanyID, false, true, false, Chd_AirPricingInfo_Id.ToString(), drAdd, dsResponse, BookingCode, SegmentRef);
        //                    }
        //                }
        //                //==========================================================================================================================================
        //                if (Inf > 0)
        //                {
        //                    int Inf_AirPricingInfo_Id = Convert.ToInt32(AirPricingInfo_Id) + 2;
        //                    PaxType = dsResponse.Tables["PassengerType"].Select("AirPricingInfo_Id='" + Inf_AirPricingInfo_Id.ToString() + "'").CopyToDataTable().Rows[0]["Code"].ToString();
        //                    if (PaxType.Equals("INF"))
        //                    {
        //                        SetFare2(SearchID, CompanyID, false, false, true, Inf_AirPricingInfo_Id.ToString(), drAdd, dsResponse, BookingCode, SegmentRef);
        //                    }
        //                }

        //            }
        //            else
        //            {
        //                int Chd_AirPricingInfo_Id = Convert.ToInt32(AirPricingInfo_Id) + 1;
        //                //==========================================================================================================================================
        //                if (Chd > 0)
        //                {
        //                    PaxType = dsResponse.Tables["PassengerType"].Select("AirPricingInfo_Id='" + Chd_AirPricingInfo_Id.ToString() + "'").CopyToDataTable().Rows[0]["Code"].ToString();
        //                    if (PaxType.Equals("CNN") || PaxType.Equals("CHD"))
        //                    {
        //                        SetFare2(SearchID, CompanyID, false, true, false, Chd_AirPricingInfo_Id.ToString(), drAdd, dsResponse, BookingCode, SegmentRef);
        //                    }
        //                }
        //                //==========================================================================================================================================
        //                if (Inf > 0)
        //                {
        //                    int Inf_AirPricingInfo_Id = Convert.ToInt32(AirPricingInfo_Id) + 1;
        //                    PaxType = dsResponse.Tables["PassengerType"].Select("AirPricingInfo_Id='" + Inf_AirPricingInfo_Id.ToString() + "'").CopyToDataTable().Rows[0]["Code"].ToString();
        //                    if (PaxType.Equals("INF"))
        //                    {
        //                        SetFare2(SearchID, CompanyID, false, false, true, Inf_AirPricingInfo_Id.ToString(), drAdd, dsResponse, BookingCode, SegmentRef);
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        DBCommon.Logger.dbLogg(CompanyID, 0, "SetFare1", "Uapi", "", SearchID, ex.Message);
        //    }
        //}
        //public void SetFare2(string SearchID, string CompanyID, bool IsAdt, bool IsChd, bool IsInf, string AirPricingInfo_Id, DataRow drAdd, DataSet dsResponse, string BookingCode, string SegmentRef)
        //{
        //    try
        //    {
        //        if (IsAdt)
        //        {
        //            DataRow[] drBookingInfo = dsResponse.Tables["BookingInfo"].Select("AirPricingInfo_Id='" + AirPricingInfo_Id + "' And BookingCode='" + BookingCode + "' And SegmentRef='" + SegmentRef + "'");
        //            if (drBookingInfo.Length > 0)
        //            {
        //                drAdd["ViaName"] = "AADT-" + drBookingInfo.CopyToDataTable().Rows[0]["FareInfoRef"].ToString();

        //                DataRow[] drSelectFareInfoData = dsResponse.Tables["FareInfo"].Select("key='" + drBookingInfo.CopyToDataTable().Rows[0]["FareInfoRef"].ToString() + "'");
        //                if (drSelectFareInfoData.Length > 0)
        //                {
        //                    drAdd["ProductClass"] = "AADT-" + drSelectFareInfoData.CopyToDataTable().Rows[0]["FareBasis"].ToString();
        //                }

        //                if (drBookingInfo.CopyToDataTable().Columns.Contains("HostTokenRef"))
        //                {
        //                    string HostTokenRef = drBookingInfo.CopyToDataTable().Rows[0]["HostTokenRef"].ToString();

        //                    var varHostToken = from myRow in dsResponse.Tables["HostToken"].AsEnumerable()
        //                                       where myRow.Field<string>("Key") == HostTokenRef
        //                                       select myRow;
        //                    if (varHostToken.Any())
        //                    {
        //                        drAdd["API_BookingFareID"] = HostTokenRef + "?" + varHostToken.CopyToDataTable().Rows[0]["HostToken_Text"].ToString();

        //                        if (varHostToken.CopyToDataTable().Rows.Count > 1)
        //                        {

        //                        }
        //                    }
        //                }
        //            }

        //            DataRow[] drSelectFareInfo = dsResponse.Tables["AirPricingInfo"].Select("AirPricingInfo_Id='" + AirPricingInfo_Id + "'");
        //            if (drSelectFareInfo.Length > 0)
        //            {
        //                DataRow[] drAirPricingSolution = dsResponse.Tables["AirPricingSolution"].Select("AirPricingSolution_Id='" + drSelectFareInfo.CopyToDataTable().Rows[0]["AirPricingSolution_Id"].ToString() + "'");
        //                if (drAirPricingSolution.Length > 0)
        //                {
        //                    drAdd["BookingFareID"] = drAirPricingSolution.CopyToDataTable().Rows[0]["Key"].ToString();
        //                }

        //                drAdd["IsPriceChanged"] = "AADT-" + drSelectFareInfo.CopyToDataTable().Rows[0]["PricingMethod"].ToString();
        //                drAdd["API_SearchID"] = "AADT-" + drSelectFareInfo.CopyToDataTable().Rows[0]["Key"].ToString();


        //                Int32 discount = 0;
        //                if (drSelectFareInfo.CopyToDataTable().Columns.Contains("Fees") && drSelectFareInfo.CopyToDataTable().Columns.Contains("ApproximateFees"))
        //                {
        //                    if (drSelectFareInfo.CopyToDataTable().Rows[0]["Fees"].ToString().IndexOf("INR") != -1)
        //                    {
        //                        discount = decimal.ToInt32(Convert.ToDecimal(drSelectFareInfo.CopyToDataTable().Rows[0]["Fees"].ToString().Replace("INR", "").Trim()));
        //                    }
        //                    else if (drSelectFareInfo.CopyToDataTable().Rows[0]["ApproximateFees"].ToString().IndexOf("INR") != -1)
        //                    {
        //                        discount = decimal.ToInt32(Convert.ToDecimal(drSelectFareInfo.CopyToDataTable().Rows[0]["ApproximateFees"].ToString().Replace("INR", "").Trim()));
        //                    }
        //                }

        //                if (discount < 0)
        //                {
        //                    if (drSelectFareInfo.CopyToDataTable().Rows[0]["BasePrice"].ToString().IndexOf("INR") != -1)
        //                    {
        //                        drAdd["Adt_BASIC"] = Decimal.ToInt32(Convert.ToDecimal(drSelectFareInfo.CopyToDataTable().Rows[0]["BasePrice"].ToString().Replace("INR", "").Trim())) + discount;
        //                    }
        //                    else if (drSelectFareInfo.CopyToDataTable().Rows[0]["ApproximateBasePrice"].ToString().IndexOf("INR") != -1)
        //                    {
        //                        drAdd["Adt_BASIC"] = Decimal.ToInt32(Convert.ToDecimal(drSelectFareInfo.CopyToDataTable().Rows[0]["ApproximateBasePrice"].ToString().Replace("INR", "").Trim())) + discount;
        //                    }
        //                }
        //                else
        //                {
        //                    if (drSelectFareInfo.CopyToDataTable().Rows[0]["BasePrice"].ToString().IndexOf("INR") != -1)
        //                    {
        //                        drAdd["Adt_BASIC"] = Decimal.ToInt32(Convert.ToDecimal(drSelectFareInfo.CopyToDataTable().Rows[0]["BasePrice"].ToString().Replace("INR", "").Trim()));
        //                    }
        //                    else if (drSelectFareInfo.CopyToDataTable().Rows[0]["ApproximateBasePrice"].ToString().IndexOf("INR") != -1)
        //                    {
        //                        drAdd["Adt_BASIC"] = Decimal.ToInt32(Convert.ToDecimal(drSelectFareInfo.CopyToDataTable().Rows[0]["ApproximateBasePrice"].ToString().Replace("INR", "").Trim()));
        //                    }
        //                }


        //                drAdd["AdtTotalTax"] = Decimal.ToInt32(Convert.ToDecimal(drSelectFareInfo.CopyToDataTable().Rows[0]["Taxes"].ToString().Replace("INR", "").Trim()));


        //                if (discount < 0)
        //                {
        //                    DataRow[] drFeeInfo = dsResponse.Tables["FeeInfo"].Select("AirPricingInfo_Id='" + AirPricingInfo_Id + "'");
        //                    if (drFeeInfo.Length > 0)
        //                    {
        //                        drAdd["TempData1"] = "AADT-" + drFeeInfo.CopyToDataTable().Rows[0]["Key"].ToString() + "," + drFeeInfo.CopyToDataTable().Rows[0]["Code"].ToString();
        //                    }

        //                    drAdd["Adt_Import"] = discount;
        //                    //drAdd["PriceType"] = "Flexi plus";
        //                }

        //                if (drAdd["CarrierCode"].ToString().Equals("6E"))
        //                {
        //                    drAdd["RefundType"] = "N";
        //                }
        //                else
        //                {
        //                    if (drSelectFareInfo.CopyToDataTable().Columns.Contains("Refundable"))
        //                    {
        //                        if (drSelectFareInfo.CopyToDataTable().Rows[0]["Refundable"].ToString().Trim().Length > 0 && Convert.ToBoolean(drSelectFareInfo.CopyToDataTable().Rows[0]["Refundable"].ToString().Trim()))
        //                        {
        //                            drAdd["RefundType"] = "N";
        //                        }
        //                        else
        //                        {
        //                            drAdd["RefundType"] = "Y";
        //                        }
        //                    }
        //                    else
        //                    {
        //                        drAdd["RefundType"] = "Y";
        //                    }
        //                }

        //                DataRow[] drSelectTaxInfo = dsResponse.Tables["TaxInfo"].Select("AirPricingInfo_Id='" + AirPricingInfo_Id + "'");
        //                if (drSelectTaxInfo.Length > 0)
        //                {
        //                    int K3 = 0;
        //                    int YR = 0;
        //                    int YQ = 0;
        //                    int WO = 0;
        //                    int JN = 0;
        //                    int IN = 0;
        //                    int OT = 0;

        //                    foreach (DataRow drTax in drSelectTaxInfo.CopyToDataTable().Rows)
        //                    {
        //                        if (drTax["Category"].ToString().Trim().Equals("K3"))
        //                        {
        //                            K3 += Decimal.ToInt32(Convert.ToDecimal(drTax["Amount"].ToString().Trim().Replace("INR", "").Trim()));
        //                        }
        //                        else if (drTax["Category"].ToString().Trim().Equals("YR"))
        //                        {
        //                            YR += Decimal.ToInt32(Convert.ToDecimal(drTax["Amount"].ToString().Trim().Replace("INR", "").Trim()));
        //                        }
        //                        else if (drTax["Category"].ToString().Trim().Equals("YQ"))
        //                        {
        //                            YQ += Decimal.ToInt32(Convert.ToDecimal(drTax["Amount"].ToString().Trim().Replace("INR", "").Trim()));
        //                        }
        //                        else if (drTax["Category"].ToString().Trim().Equals("WO"))
        //                        {
        //                            WO += Decimal.ToInt32(Convert.ToDecimal(drTax["Amount"].ToString().Trim().Replace("INR", "").Trim()));
        //                        }
        //                        else if (drTax["Category"].ToString().Trim().Equals("JN"))
        //                        {
        //                            JN += Decimal.ToInt32(Convert.ToDecimal(drTax["Amount"].ToString().Trim().Replace("INR", "").Trim()));
        //                        }
        //                        else if (drTax["Category"].ToString().Trim().Equals("IN") || drTax["Category"].ToString().Trim().Equals("YM"))
        //                        {
        //                            IN += Decimal.ToInt32(Convert.ToDecimal(drTax["Amount"].ToString().Trim().Replace("INR", "").Trim()));
        //                        }
        //                        else
        //                        {
        //                            OT += Decimal.ToInt32(Convert.ToDecimal(drTax["Amount"].ToString().Trim().Replace("INR", "").Trim()));
        //                        }
        //                    }

        //                    if (discount > 0)
        //                    {
        //                        OT += Decimal.ToInt32(Convert.ToDecimal(discount));
        //                    }

        //                    drAdd["Adt_YQ"] = YQ;
        //                    drAdd["Adt_PSF"] = WO;
        //                    drAdd["Adt_UDF"] = IN;
        //                    drAdd["Adt_AUDF"] = 0;
        //                    drAdd["Adt_CUTE"] = YR;
        //                    drAdd["Adt_GST"] = K3;
        //                    drAdd["Adt_TF"] = JN;
        //                    drAdd["Adt_CESS"] = 0;
        //                    drAdd["Adt_EX"] = OT;
        //                }
        //            }
        //        }
        //        else if (IsChd)
        //        {
        //            DataRow[] drBookingInfo = dsResponse.Tables["BookingInfo"].Select("AirPricingInfo_Id='" + AirPricingInfo_Id + "' And BookingCode = '" + BookingCode + "' And SegmentRef = '" + SegmentRef + "'");
        //            if (drBookingInfo.Length > 0)
        //            {
        //                drAdd["ViaName"] = drAdd["ViaName"].ToString() + "?" + "CCHD-" + drBookingInfo.CopyToDataTable().Rows[0]["FareInfoRef"].ToString();
        //                DataRow[] drSelectFareInfoData = dsResponse.Tables["FareInfo"].Select("key='" + drBookingInfo.CopyToDataTable().Rows[0]["FareInfoRef"].ToString() + "'");
        //                if (drSelectFareInfoData.Length > 0)
        //                {
        //                    drAdd["ProductClass"] = drAdd["ProductClass"].ToString() + "?" + "CCHD-" + drSelectFareInfoData.CopyToDataTable().Rows[0]["FareBasis"].ToString();
        //                }

        //                //string HostTokenRef = drBookingInfo.CopyToDataTable().Rows[0]["HostTokenRef"].ToString();
        //                //drAdd["API_BookingFareID"] = drAdd["API_BookingFareID"].ToString() + "?" + "CCHD-" + HostTokenRef;
        //            }

        //            DataRow[] drSelectFareInfo = dsResponse.Tables["AirPricingInfo"].Select("AirPricingInfo_Id='" + AirPricingInfo_Id.ToString() + "'");
        //            if (drSelectFareInfo.Length > 0)
        //            {
        //                drAdd["IsPriceChanged"] = drAdd["IsPriceChanged"].ToString() + "?" + "CCHD-" + drSelectFareInfo.CopyToDataTable().Rows[0]["PricingMethod"].ToString();
        //                drAdd["API_SearchID"] = drAdd["API_SearchID"].ToString() + "?" + "CCHD-" + drSelectFareInfo.CopyToDataTable().Rows[0]["Key"].ToString();

        //                int discount = 0;
        //                if (drSelectFareInfo.CopyToDataTable().Columns.Contains("Fees") && drSelectFareInfo.CopyToDataTable().Columns.Contains("ApproximateFees"))
        //                {
        //                    if (drSelectFareInfo.CopyToDataTable().Rows[0]["Fees"].ToString().IndexOf("INR") != -1)
        //                    {
        //                        discount = decimal.ToInt32(Convert.ToDecimal(drSelectFareInfo.CopyToDataTable().Rows[0]["Fees"].ToString().Replace("INR", "").Trim()));
        //                    }
        //                    else if (drSelectFareInfo.CopyToDataTable().Rows[0]["ApproximateFees"].ToString().IndexOf("INR") != -1)
        //                    {
        //                        discount = decimal.ToInt32(Convert.ToDecimal(drSelectFareInfo.CopyToDataTable().Rows[0]["ApproximateFees"].ToString().Replace("INR", "").Trim()));
        //                    }
        //                }

        //                if (discount < 0)
        //                {
        //                    if (drSelectFareInfo.CopyToDataTable().Rows[0]["BasePrice"].ToString().IndexOf("INR") != -1)
        //                    {
        //                        drAdd["Chd_BASIC"] = Decimal.ToInt32(Convert.ToDecimal(drSelectFareInfo.CopyToDataTable().Rows[0]["BasePrice"].ToString().Replace("INR", "").Trim())) + discount;
        //                    }
        //                    else if (drSelectFareInfo.CopyToDataTable().Rows[0]["ApproximateBasePrice"].ToString().IndexOf("INR") != -1)
        //                    {
        //                        drAdd["Chd_BASIC"] = Decimal.ToInt32(Convert.ToDecimal(drSelectFareInfo.CopyToDataTable().Rows[0]["ApproximateBasePrice"].ToString().Replace("INR", "").Trim())) + discount;
        //                    }
        //                }
        //                else
        //                {
        //                    if (drSelectFareInfo.CopyToDataTable().Rows[0]["BasePrice"].ToString().IndexOf("INR") != -1)
        //                    {
        //                        drAdd["Chd_BASIC"] = Decimal.ToInt32(Convert.ToDecimal(drSelectFareInfo.CopyToDataTable().Rows[0]["BasePrice"].ToString().Replace("INR", "").Trim()));
        //                    }
        //                    else if (drSelectFareInfo.CopyToDataTable().Rows[0]["ApproximateBasePrice"].ToString().IndexOf("INR") != -1)
        //                    {
        //                        drAdd["Chd_BASIC"] = Decimal.ToInt32(Convert.ToDecimal(drSelectFareInfo.CopyToDataTable().Rows[0]["ApproximateBasePrice"].ToString().Replace("INR", "").Trim()));
        //                    }                           
        //                }

        //                drAdd["ChdTotalTax"] = Decimal.ToInt32(Convert.ToDecimal(drSelectFareInfo.CopyToDataTable().Rows[0]["Taxes"].ToString().Replace("INR", "").Trim()));

        //                if (discount < 0)
        //                {
        //                    DataRow[] drFeeInfo = dsResponse.Tables["FeeInfo"].Select("AirPricingInfo_Id='" + AirPricingInfo_Id + "'");
        //                    if (drFeeInfo.Length > 0)
        //                    {
        //                        drAdd["TempData1"] = drAdd["TempData1"].ToString() + "?" + "CCHD-" + drFeeInfo.CopyToDataTable().Rows[0]["Key"].ToString() + "," + drFeeInfo.CopyToDataTable().Rows[0]["Code"].ToString();
        //                    }

        //                    drAdd["Chd_Import"] = discount;
        //                }

        //                DataRow[] drSelectTaxInfo = dsResponse.Tables["TaxInfo"].Select("AirPricingInfo_Id='" + AirPricingInfo_Id.ToString() + "'");
        //                if (drSelectTaxInfo.Length > 0)
        //                {
        //                    int K3 = 0;
        //                    int YR = 0;
        //                    int YQ = 0;
        //                    int WO = 0;
        //                    int JN = 0;
        //                    int IN = 0;
        //                    int OT = 0;

        //                    foreach (DataRow drTax in drSelectTaxInfo.CopyToDataTable().Rows)
        //                    {
        //                        if (drTax["Category"].ToString().Trim().Equals("K3"))
        //                        {
        //                            K3 += Decimal.ToInt32(Convert.ToDecimal(drTax["Amount"].ToString().Trim().Replace("INR", "").Trim()));
        //                        }
        //                        else if (drTax["Category"].ToString().Trim().Equals("YR"))
        //                        {
        //                            YR += Decimal.ToInt32(Convert.ToDecimal(drTax["Amount"].ToString().Trim().Replace("INR", "").Trim()));
        //                        }
        //                        else if (drTax["Category"].ToString().Trim().Equals("YQ"))
        //                        {
        //                            YQ += Decimal.ToInt32(Convert.ToDecimal(drTax["Amount"].ToString().Trim().Replace("INR", "").Trim()));
        //                        }
        //                        else if (drTax["Category"].ToString().Trim().Equals("WO"))
        //                        {
        //                            WO += Decimal.ToInt32(Convert.ToDecimal(drTax["Amount"].ToString().Trim().Replace("INR", "").Trim()));
        //                        }
        //                        else if (drTax["Category"].ToString().Trim().Equals("JN"))
        //                        {
        //                            JN += Decimal.ToInt32(Convert.ToDecimal(drTax["Amount"].ToString().Trim().Replace("INR", "").Trim()));
        //                        }
        //                        else if (drTax["Category"].ToString().Trim().Equals("IN") || drTax["Category"].ToString().Trim().Equals("YM"))
        //                        {
        //                            IN += Decimal.ToInt32(Convert.ToDecimal(drTax["Amount"].ToString().Trim().Replace("INR", "").Trim()));
        //                        }
        //                        else
        //                        {
        //                            OT += Decimal.ToInt32(Convert.ToDecimal(drTax["Amount"].ToString().Trim().Replace("INR", "").Trim()));
        //                        }
        //                    }

        //                    if (discount > 0)
        //                    {
        //                        OT += Decimal.ToInt32(Convert.ToDecimal(discount));
        //                    }

        //                    drAdd["Chd_YQ"] = YQ;
        //                    drAdd["Chd_PSF"] = WO;
        //                    drAdd["Chd_UDF"] = IN;
        //                    drAdd["Chd_AUDF"] = 0;
        //                    drAdd["Chd_CUTE"] = YR;
        //                    drAdd["Chd_GST"] = K3;
        //                    drAdd["Chd_TF"] = JN;
        //                    drAdd["Chd_CESS"] = 0;
        //                    drAdd["Chd_EX"] = OT;
        //                }
        //            }
        //        }
        //        else if (IsInf)
        //        {
        //            DataRow[] drBookingInfo = dsResponse.Tables["BookingInfo"].Select("AirPricingInfo_Id='" + AirPricingInfo_Id + "' And BookingCode='" + BookingCode + "' And SegmentRef='" + SegmentRef + "'");
        //            if (drBookingInfo.Length > 0)
        //            {
        //                drAdd["ViaName"] = drAdd["ViaName"].ToString() + "?" + "IINF-" + drBookingInfo.CopyToDataTable().Rows[0]["FareInfoRef"].ToString();
        //                DataRow[] drSelectFareInfoData = dsResponse.Tables["FareInfo"].Select("key='" + drBookingInfo.CopyToDataTable().Rows[0]["FareInfoRef"].ToString() + "'");
        //                if (drSelectFareInfoData.Length > 0)
        //                {
        //                    drAdd["ProductClass"] = drAdd["ProductClass"].ToString() + "?" + "IINF-" + drSelectFareInfoData.CopyToDataTable().Rows[0]["FareBasis"].ToString();
        //                }

        //                //string HostTokenRef = drBookingInfo.CopyToDataTable().Rows[0]["HostTokenRef"].ToString();
        //                //drAdd["API_BookingFareID"] = drAdd["API_BookingFareID"].ToString() + "?" + "IINF-" + HostTokenRef;
        //            }

        //            DataRow[] drSelectFareInfo = dsResponse.Tables["AirPricingInfo"].Select("AirPricingInfo_Id='" + AirPricingInfo_Id + "'");
        //            if (drSelectFareInfo.Length > 0)
        //            {
        //                drAdd["IsPriceChanged"] = drAdd["IsPriceChanged"].ToString() + "?" + "IINF-" + drSelectFareInfo.CopyToDataTable().Rows[0]["PricingMethod"].ToString();
        //                drAdd["API_SearchID"] = drAdd["API_SearchID"].ToString() + "?" + "IINF-" + drSelectFareInfo.CopyToDataTable().Rows[0]["Key"].ToString();
        //                if (drSelectFareInfo.CopyToDataTable().Rows[0]["BasePrice"].ToString().IndexOf("INR") != -1)
        //                {
        //                    drAdd["Inf_BASIC"] = Decimal.ToInt32(Convert.ToDecimal(drSelectFareInfo.CopyToDataTable().Rows[0]["BasePrice"].ToString().Replace("INR", "").Trim()));
        //                }
        //                else if (drSelectFareInfo.CopyToDataTable().Rows[0]["ApproximateBasePrice"].ToString().IndexOf("INR") != -1)
        //                {
        //                    drAdd["Inf_BASIC"] = Decimal.ToInt32(Convert.ToDecimal(drSelectFareInfo.CopyToDataTable().Rows[0]["ApproximateBasePrice"].ToString().Replace("INR", "").Trim()));
        //                }

        //                int TotalPrice = Decimal.ToInt32(Convert.ToDecimal(drSelectFareInfo.CopyToDataTable().Rows[0]["TotalPrice"].ToString().Replace("INR", "").Trim()));

        //                drAdd["InfTotalTax"] = TotalPrice - Convert.ToInt32(drAdd["Inf_BASIC"].ToString());
        //                drAdd["Inf_TAX"] = TotalPrice - Convert.ToInt32(drAdd["Inf_BASIC"].ToString());
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        DBCommon.Logger.dbLogg(CompanyID, 0, "SetFare2", "Uapi", "", SearchID, ex.Message);
        //    }
        //}
    }
}
