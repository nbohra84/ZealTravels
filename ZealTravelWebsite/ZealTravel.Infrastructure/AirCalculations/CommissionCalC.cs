using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace ZealTravel.Infrastructure.AirCalculations
{
    class CommissionCalC
    {
        public static Decimal GET_Cfee(string UserType, DataRow drSelect, DataTable dtCfee)
        {
            Decimal iValue = 0;
            try
            {
                if (dtCfee != null && dtCfee.Rows.Count > 0)
                {
                    int pax = int.Parse(drSelect["Adt"].ToString());
                    pax += int.Parse(drSelect["Chd"].ToString());

                    if (UserType.Equals("B2B2C") || UserType.Equals("B2C"))
                    {
                        iValue = Convert.ToDecimal(dtCfee.Rows[0]["Cfee_cu"].ToString());
                    }
                    else if (UserType.Equals("B2B2B"))
                    {
                        iValue = Convert.ToDecimal(dtCfee.Rows[0]["Cfee_sa"].ToString());
                    }
                    iValue = iValue * pax;
                }
            }
            catch
            {

            }

            iValue = DBCommon.CommonFunction.AvgAmount(iValue.ToString());
            return iValue;
        }
        public static Int32 Supplierwise_PercentDeal(string PaxType, DataRow drSelect, DataTable dtSuppierWiseComm)
        {
            Decimal iValue = 0;
            try
            {
                if (dtSuppierWiseComm != null && dtSuppierWiseComm.Rows.Count > 0)
                {
                    DataRow[] drComm = dtSuppierWiseComm.Select("CarrierCode='" + drSelect["CarrierCode"].ToString() + "'");
                    if (drComm.Length > 0)
                    {
                        Decimal TotalDeal = 0;
                        Decimal.TryParse(drSelect["API_SearchID"].ToString(), out TotalDeal);

                        Decimal iDeal = 0;
                        Decimal.TryParse(drComm.CopyToDataTable().Rows[0]["Value"].ToString(), out iDeal);

                        if (TotalDeal > 0 && iDeal > 0)
                        {
                            iValue = (TotalDeal) * (iDeal) / 100;
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }

            return DBCommon.CommonFunction.AvgAmount1(iValue.ToString());
        }
        public static Decimal Basic_offline(string PaxType, DataRow drSelect, Decimal iDeal, Decimal Basic_Iata, DataTable dtST)
        {
            Decimal iValue = 0;
            try
            {
                if (iDeal > 0)
                {
                    if (PaxType.Equals("A"))
                    {
                        iValue = (Convert.ToDecimal(drSelect["ADT_BASIC"].ToString()) - Basic_Iata) * (iDeal) / 100;
                    }
                    else
                    {
                        iValue = (Convert.ToDecimal(drSelect["CHD_BASIC"].ToString()) - Basic_Iata) * (iDeal) / 100;
                    }
                }
            }
            catch
            {

            }

            iValue = Deduct_Service_Tax(iValue, dtST);
            iValue = DBCommon.CommonFunction.AvgAmount(iValue.ToString());
            return iValue;
        }
        public static Decimal YQ_offline(string PaxType, DataRow drSelect, Decimal iDeal, Decimal YQ_Iata, DataTable dtST)
        {
            Decimal iValue = 0;
            try
            {
                if (iDeal > 0)
                {
                    if (PaxType.Equals("A"))
                    {
                        iValue = (Convert.ToDecimal(drSelect["ADT_YQ"].ToString()) - YQ_Iata) * (iDeal) / 100;
                    }
                    else
                    {
                        iValue = (Convert.ToDecimal(drSelect["CHD_YQ"].ToString()) - YQ_Iata) * (iDeal) / 100;
                    }
                }
            }
            catch
            {

            }

            iValue = Deduct_Service_Tax(iValue, dtST);
            iValue = DBCommon.CommonFunction.AvgAmount(iValue.ToString());
            return iValue;
        }

        public static Decimal Basic_Iata_Offline(string PaxType, DataRow drSelect, Decimal iDeal, DataTable dtST, out Decimal BasicIata)
        {
            Decimal iValue = 0;
            try
            {
                if (iDeal > 0)
                {
                    if (PaxType.Equals("A"))
                    {
                        iValue = Convert.ToDecimal(drSelect["ADT_BASIC"].ToString()) * (iDeal) / 100;
                    }
                    else
                    {
                        iValue = Convert.ToDecimal(drSelect["CHD_BASIC"].ToString()) * (iDeal) / 100;
                    }
                }
            }
            catch
            {

            }

            BasicIata = iValue;
            iValue = Deduct_Service_Tax(iValue, dtST);
            iValue = DBCommon.CommonFunction.AvgAmount(iValue.ToString());
            return iValue;
        }
        public static Decimal YQ_Iata_Offline(string PaxType, DataRow drSelect, Decimal iDeal, DataTable dtST, out Decimal YQIata)
        {
            Decimal iValue = 0;
            try
            {
                if (iDeal > 0)
                {
                    if (PaxType.Equals("A"))
                    {
                        iValue = Convert.ToDecimal(drSelect["ADT_YQ"].ToString()) * (iDeal) / 100;
                    }
                    else
                    {
                        iValue = Convert.ToDecimal(drSelect["CHD_YQ"].ToString()) * (iDeal) / 100;
                    }
                }
            }
            catch
            {

            }
            YQIata = iValue;
            iValue = Deduct_Service_Tax(iValue, dtST);
            iValue = DBCommon.CommonFunction.AvgAmount(iValue.ToString());
            return iValue;
        }

        public static Decimal Basic(string PaxType, DataRow drSelect, DataTable dtCommission, Decimal Basic_Iata, DataTable dtST, DataTable dtPriceTypeCommission)
        {
            Decimal iValue = 0;
            try
            {
                if (dtCommission != null && dtCommission.Rows.Count > 0)
                {
                    int iPriority = -1;
                    DataRow[] drComm = dtCommission.Select("CarrierCode='" + drSelect["CarrierCode"].ToString() + "'");
                    if (drComm.Length > 0)
                    {
                        string Class = drSelect["ClassOfService"].ToString().Trim().ToUpper();
                        foreach (DataRow dr in drComm.CopyToDataTable().Rows)
                        {
                            if (dr["BookingClass"].ToString().Trim().Length > 0 && dr["BookingClass"].ToString().Trim().IndexOf(Class) != -1)
                            {
                                iPriority = Convert.ToInt32(dr["Priority"].ToString());
                                break;
                            }
                        }

                        if (iPriority != -1)
                        {
                            drComm = dtCommission.Select("CarrierCode='" + drSelect["CarrierCode"].ToString() + "' And Priority=" + iPriority + "");
                            if (drComm.Length > 0)
                            {
                                Decimal iDeal = 0;
                                Decimal.TryParse(drComm.CopyToDataTable().Rows[0]["Basic"].ToString(), out iDeal);

                                if (iDeal > 0)
                                {
                                    if (PaxType.Equals("A"))
                                    {
                                        iValue = (Convert.ToDecimal(drSelect["ADT_BASIC"].ToString()) - Basic_Iata) * (iDeal) / 100;
                                    }
                                    else
                                    {
                                        iValue = (Convert.ToDecimal(drSelect["CHD_BASIC"].ToString()) - Basic_Iata) * (iDeal) / 100;
                                    }
                                }
                            }
                        }
                        else
                        {
                            Decimal iDeal = 0;
                            Decimal.TryParse(drComm.CopyToDataTable().Rows[0]["Basic"].ToString(), out iDeal);

                            if (dtPriceTypeCommission != null && dtPriceTypeCommission.Rows.Count > 0)
                            {
                                DataRow[] drPriceType = dtPriceTypeCommission.Select("CarrierCode='" + drSelect["CarrierCode"].ToString().Trim() + "' And PriceType ='" + drSelect["PriceType"].ToString().Trim() + "'");
                                if (drPriceType.Length > 0)
                                {
                                    iDeal = Convert.ToDecimal(drPriceType.CopyToDataTable().Rows[0]["Basic"].ToString());
                                }
                            }

                            if (iDeal > 0)
                            {
                                if (PaxType.Equals("A"))
                                {
                                    iValue = (Convert.ToDecimal(drSelect["ADT_BASIC"].ToString()) - Basic_Iata) * (iDeal) / 100;
                                }
                                else
                                {
                                    iValue = (Convert.ToDecimal(drSelect["CHD_BASIC"].ToString()) - Basic_Iata) * (iDeal) / 100;
                                }
                            }
                        }
                    }
                }
            }
            catch
            {

            }

            iValue = Deduct_Service_Tax(iValue, dtST);
            iValue = DBCommon.CommonFunction.AvgAmount(iValue.ToString());
            return iValue;
        }
        public static Decimal YQ(string PaxType, DataRow drSelect, DataTable dtCommission, Decimal YQ_Iata, DataTable dtST, DataTable dtPriceTypeCommission)
        {
            Decimal iValue = 0;
            try
            {
                if (dtCommission != null && dtCommission.Rows.Count > 0)
                {
                    int iPriority = -1;
                    DataRow[] drComm = dtCommission.Select("CarrierCode='" + drSelect["CarrierCode"].ToString() + "'");
                    if (drComm.Length > 0)
                    {
                        string Class = drSelect["ClassOfService"].ToString().Trim().ToUpper();
                        foreach (DataRow dr in drComm.CopyToDataTable().Rows)
                        {
                            if (dr["BookingClass"].ToString().Trim().Length > 0 && dr["BookingClass"].ToString().Trim().IndexOf(Class) != -1)
                            {
                                iPriority = Convert.ToInt32(dr["Priority"].ToString());
                                break;
                            }
                        }

                        if (iPriority != -1)
                        {
                            drComm = dtCommission.Select("CarrierCode='" + drSelect["CarrierCode"].ToString() + "' And Priority=" + iPriority + "");
                            if (drComm.Length > 0)
                            {
                                Decimal iDeal = 0;
                                Decimal.TryParse(drComm.CopyToDataTable().Rows[0]["YQ"].ToString(), out iDeal);

                                if (iDeal > 0)
                                {
                                    if (PaxType.Equals("A"))
                                    {
                                        iValue = (Convert.ToDecimal(drSelect["ADT_YQ"].ToString()) - YQ_Iata) * (iDeal) / 100;
                                    }
                                    else
                                    {
                                        iValue = (Convert.ToDecimal(drSelect["CHD_YQ"].ToString()) - YQ_Iata) * (iDeal) / 100;
                                    }
                                }
                            }
                        }
                        else
                        {
                            Decimal iDeal = 0;
                            Decimal.TryParse(drComm.CopyToDataTable().Rows[0]["YQ"].ToString(), out iDeal);

                            if (dtPriceTypeCommission != null && dtPriceTypeCommission.Rows.Count > 0)
                            {
                                DataRow[] drPriceType = dtPriceTypeCommission.Select("CarrierCode='" + drSelect["CarrierCode"].ToString().Trim() + "' And PriceType ='" + drSelect["PriceType"].ToString().Trim() + "'");
                                if (drPriceType.Length > 0)
                                {
                                    iDeal = Convert.ToDecimal(drPriceType.CopyToDataTable().Rows[0]["Yq"].ToString());
                                }
                            }

                            if (iDeal > 0)
                            {
                                if (PaxType.Equals("A"))
                                {
                                    iValue = (Convert.ToDecimal(drSelect["ADT_YQ"].ToString()) - YQ_Iata) * (iDeal) / 100;
                                }
                                else
                                {
                                    iValue = (Convert.ToDecimal(drSelect["CHD_YQ"].ToString()) - YQ_Iata) * (iDeal) / 100;
                                }
                            }
                        }
                    }
                }
            }
            catch
            {

            }

            iValue = Deduct_Service_Tax(iValue, dtST);
            iValue = DBCommon.CommonFunction.AvgAmount(iValue.ToString());
            return iValue;
        }
        public static Decimal Basic_Iata(string PaxType, DataRow drSelect, DataTable dtCommission, DataTable dtST, out Decimal BasicIata)
        {
            Decimal iValue = 0;
            BasicIata = 0;
            try
            {
                if (dtCommission != null && dtCommission.Rows.Count > 0)
                {
                    DataRow[] drComm = dtCommission.Select("CarrierCode='" + drSelect["CarrierCode"].ToString() + "' And Priority=" + 0 + "");
                    if (drComm.Length > 0)
                    {
                        Decimal iDeal = 0;
                        Decimal.TryParse(drComm.CopyToDataTable().Rows[0]["Basic_Iata"].ToString(), out iDeal);

                        if (iDeal > 0)
                        {
                            if (PaxType.Equals("A"))
                            {
                                iValue = Convert.ToDecimal(drSelect["ADT_BASIC"].ToString()) * (iDeal) / 100;
                            }
                            else
                            {
                                iValue = Convert.ToDecimal(drSelect["CHD_BASIC"].ToString()) * (iDeal) / 100;
                            }
                        }
                    }
                }
            }
            catch
            {

            }

            BasicIata = iValue;
            iValue = Deduct_Service_Tax(iValue, dtST);
            iValue = DBCommon.CommonFunction.AvgAmount(iValue.ToString());
            return iValue;
        }
        public static Decimal YQ_Iata(string PaxType, DataRow drSelect, DataTable dtCommission, DataTable dtST, out Decimal YQIata)
        {
            Decimal iValue = 0;
            YQIata = 0;
            try
            {
                if (dtCommission != null && dtCommission.Rows.Count > 0)
                {
                    DataRow[] drComm = dtCommission.Select("CarrierCode='" + drSelect["CarrierCode"].ToString() + "' And Priority=" + 0 + "");
                    if (drComm.Length > 0)
                    {
                        Decimal iDeal = 0;
                        Decimal.TryParse(drComm.CopyToDataTable().Rows[0]["YQ_Iata"].ToString(), out iDeal);

                        if (iDeal > 0)
                        {
                            if (PaxType.Equals("A"))
                            {
                                iValue = Convert.ToDecimal(drSelect["ADT_YQ"].ToString()) * (iDeal) / 100;
                            }
                            else
                            {
                                iValue = Convert.ToDecimal(drSelect["CHD_YQ"].ToString()) * (iDeal) / 100;
                            }
                        }
                    }
                }
            }
            catch
            {

            }
            YQIata = iValue;
            iValue = Deduct_Service_Tax(iValue, dtST);
            iValue = DBCommon.CommonFunction.AvgAmount(iValue.ToString());
            return iValue;
        }
        public static Decimal CB(string PaxType, DataRow drSelect, DataTable dtCommission, DataTable dtST)
        {
            Decimal iValue = 0;
            try
            {
                if (dtCommission != null && dtCommission.Rows.Count > 0)
                {
                    DataRow[] drComm = dtCommission.Select("CarrierCode='" + drSelect["CarrierCode"].ToString() + "' And Priority=" + 0 + "");
                    if (drComm.Length > 0)
                    {
                        Decimal.TryParse(drComm.CopyToDataTable().Rows[0]["CB"].ToString(), out iValue);
                    }
                }
            }
            catch
            {

            }

            iValue = Deduct_Service_Tax(iValue, dtST);
            iValue = DBCommon.CommonFunction.AvgAmount(iValue.ToString());
            return iValue;
        }
        public static Decimal Promo(string PaxType, DataRow drSelect, DataTable dtCommission, DataTable dtST)
        {
            Decimal iValue = 0;
            try
            {
                if (dtCommission != null && dtCommission.Rows.Count > 0)
                {
                    DataRow[] drComm = dtCommission.Select("CarrierCode='" + drSelect["CarrierCode"].ToString() + "' And Priority=" + 0 + "");
                    if (drComm.Length > 0)
                    {
                        Decimal.TryParse(drComm.CopyToDataTable().Rows[0]["Promo"].ToString(), out iValue);
                    }
                }
            }
            catch
            {

            }

            iValue = Deduct_Service_Tax(iValue, dtST);
            iValue = DBCommon.CommonFunction.AvgAmount(iValue.ToString());
            return iValue;
        }
        public static Decimal ServiceFee(string PaxType, DataRow drSelect, DataTable dtCommission, DataTable dtPriceTypeCommission)
        {
            Decimal iValue = 0;
            try
            {
                int k = 0;
                string CC = drSelect["CarrierCode"].ToString().Trim();
                string PT = drSelect["PriceType"].ToString().Trim();

                if (dtPriceTypeCommission != null && dtPriceTypeCommission.Rows.Count > 0)
                {
                    DataRow[] drComm = dtPriceTypeCommission.Select("CarrierCode='" + drSelect["CarrierCode"].ToString() + "' And PriceType='" + drSelect["PriceType"].ToString() + "' And Supplierid='" + drSelect["AirlineID"].ToString() + "'");
                    if (drComm.Length > 0)
                    {
                        k = 1;
                        if (drComm.CopyToDataTable().Columns.Contains("SF").Equals(true))
                        {
                            Decimal.TryParse(drComm.CopyToDataTable().Rows[0]["SF"].ToString(), out iValue);
                        }
                    }
                }

                if (dtCommission != null && dtCommission.Rows.Count > 0 && k.Equals(0))
                {
                    DataRow[] drComm = dtCommission.Select("CarrierCode='" + drSelect["CarrierCode"].ToString() + "' And Priority=" + 0 + "");
                    if (drComm.Length > 0)
                    {
                        if (drComm.CopyToDataTable().Columns.Contains("SF").Equals(true))
                        {
                            Decimal.TryParse(drComm.CopyToDataTable().Rows[0]["SF"].ToString(), out iValue);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                DBCommon.Logger.dbLogg("", 0, "ServiceFee", "dbAirCalc", "", "", ex.Message);
            }

            iValue = DBCommon.CommonFunction.AvgAmount(iValue.ToString());
            return iValue;
        }
        public static Decimal Markup(string PaxType, DataRow drSelect, DataTable dtMU)
        {
            Decimal iValue = 0;
            try
            {
                if (dtMU != null && dtMU.Rows.Count > 0)
                {
                    DataRow[] drComm = dtMU.Select("CarrierCode='" + drSelect["CarrierCode"].ToString() + "'");
                    if (drComm.Length > 0)
                    {
                        Decimal iDeal = 0;
                        Decimal.TryParse(drComm.CopyToDataTable().Rows[0]["iValue"].ToString(), out iDeal);
                        if (iDeal > 0)
                        {
                            bool bFixed = false;
                            bool.TryParse(drComm.CopyToDataTable().Rows[0]["Fixed"].ToString(), out bFixed);
                            if (bFixed.Equals(true))
                            {
                                iValue = iDeal;
                            }
                            else
                            {
                                bool bPercent = false;
                                bool.TryParse(drComm.CopyToDataTable().Rows[0]["Perce"].ToString(), out bPercent);
                                if (bPercent.Equals(true))
                                {
                                    bool bBasic = false;
                                    bool.TryParse(drComm.CopyToDataTable().Rows[0]["Basic"].ToString(), out bBasic);

                                    bool bYQ = false;
                                    bool.TryParse(drComm.CopyToDataTable().Rows[0]["YQ"].ToString(), out bYQ);

                                    bool bTotal = false;
                                    bool.TryParse(drComm.CopyToDataTable().Rows[0]["Total"].ToString(), out bTotal);

                                    if (bTotal.Equals(true))
                                    {
                                        Decimal iTotalFare = 0;
                                        if (PaxType.Equals("A"))
                                        {
                                            iTotalFare = Convert.ToDecimal(drSelect["AdtTotalFare"].ToString());
                                        }
                                        else
                                        {
                                            iTotalFare = Convert.ToDecimal(drSelect["ChdTotalFare"].ToString());
                                        }

                                        iValue = iTotalFare * (iDeal) / 100;
                                    }
                                    else if (bBasic.Equals(true) && bYQ.Equals(true))
                                    {
                                        Decimal iBasicYQ = 0;
                                        if (PaxType.Equals("A"))
                                        {
                                            iBasicYQ = Convert.ToDecimal(drSelect["Adt_BASIC"].ToString());
                                            iBasicYQ += Convert.ToDecimal(drSelect["Adt_YQ"].ToString());
                                        }
                                        else
                                        {
                                            iBasicYQ = Convert.ToDecimal(drSelect["Chd_BASIC"].ToString());
                                            iBasicYQ += Convert.ToDecimal(drSelect["Chd_YQ"].ToString());
                                        }

                                        iValue = iBasicYQ * (iDeal) / 100;
                                    }
                                    else if (bBasic.Equals(true) || bYQ.Equals(true))
                                    {
                                        Decimal iBasicYQ = 0;
                                        if (PaxType.Equals("A"))
                                        {
                                            if (bBasic.Equals(true))
                                            {
                                                iBasicYQ = Convert.ToDecimal(drSelect["Adt_BASIC"].ToString());
                                            }
                                            else
                                            {
                                                iBasicYQ = Convert.ToDecimal(drSelect["Adt_YQ"].ToString());
                                            }
                                        }
                                        else
                                        {
                                            if (bBasic.Equals(true))
                                            {
                                                iBasicYQ = Convert.ToDecimal(drSelect["Chd_BASIC"].ToString());
                                            }
                                            else
                                            {
                                                iBasicYQ = Convert.ToDecimal(drSelect["Chd_YQ"].ToString());
                                            }
                                        }

                                        iValue = iBasicYQ * (iDeal) / 100;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch
            {

            }

            iValue = DBCommon.CommonFunction.AvgAmount(iValue.ToString());
            return iValue;
        }
        public static Int32 MarkupAgentToSubAgent(DataRow drSelect, DataTable dtAGmarkup)
        {
            Int32 iValue = 0;
            try
            {
                if (dtAGmarkup != null && dtAGmarkup.Rows.Count > 0)
                {
                    DataRow[] drComm = dtAGmarkup.Select("CarrierCode='" + drSelect["CarrierCode"].ToString() + "'");
                    if (drComm.Length > 0)
                    {
                        if (drSelect["PriceType"].ToString().Equals("*") || drSelect["PriceType"].ToString().Equals("#") || drSelect["PriceType"].ToString().Equals("@"))
                        {
                            Int32.TryParse(drComm.CopyToDataTable().Rows[0]["NormalMarkup"].ToString(), out iValue);
                        }
                        else
                        {
                            Int32.TryParse(drComm.CopyToDataTable().Rows[0]["SpecialMarkup"].ToString(), out iValue);
                        }
                    }
                }
            }
            catch
            {

            }
            return iValue;
        }
        public static Decimal MarkupC(string PaxType, DataRow drSelect, DataTable dtMU)
        {
            Decimal iValue = 0;
            try
            {
                if (dtMU != null && dtMU.Rows.Count > 0)
                {
                    DataRow[] drComm = dtMU.Select("CarrierCode='" + drSelect["CarrierCode"].ToString() + "'");
                    if (drComm.Length > 0)
                    {
                        iValue = Convert.ToInt32(drComm.CopyToDataTable().Rows[0]["Markup"].ToString());
                    }
                }
            }
            catch
            {

            }
            return iValue;
        }
        public static Decimal ServiceTax(string PaxType, DataRow drSelect, DataTable dtST)
        {
            Decimal iValue = 0;
            try
            {
                if (dtST != null)
                {
                    Decimal iServiceTaxValue = Convert.ToDecimal(dtST.Rows[0]["Flight_ServiceTax"].ToString());
                    Decimal iCommission = 0;
                    if (PaxType.Equals("A"))
                    {
                        iCommission = Convert.ToDecimal(drSelect["Adt_SF"].ToString());
                    }
                    else
                    {
                        iCommission = Convert.ToDecimal(drSelect["Chd_SF"].ToString());
                    }
                    iValue = (iCommission * iServiceTaxValue / 100);
                }
            }
            catch
            {

            }

            iValue = DBCommon.CommonFunction.AvgAmount(iValue.ToString());
            return iValue;
        }
        public static Decimal TDS(string PaxType, DataRow drSelect, DataTable dtTDS)
        {
            Decimal iValue = 0;
            try
            {
                if (dtTDS != null)
                {
                    Decimal iTDSValue = Convert.ToDecimal(dtTDS.Rows[0]["Flight_TDS"].ToString());
                    Decimal iCommission = 0;
                    if (PaxType.Equals("A"))
                    {
                        iCommission = Convert.ToDecimal(drSelect["Adt_BAS"].ToString());
                        iCommission += Convert.ToDecimal(drSelect["Adt_Y"].ToString());
                        iCommission += Convert.ToDecimal(drSelect["Adt_CB"].ToString());
                        iCommission += Convert.ToDecimal(drSelect["Adt_PR"].ToString());
                    }
                    else
                    {
                        iCommission = Convert.ToDecimal(drSelect["Chd_BAS"].ToString());
                        iCommission += Convert.ToDecimal(drSelect["Chd_Y"].ToString());
                        iCommission += Convert.ToDecimal(drSelect["Chd_CB"].ToString());
                        iCommission += Convert.ToDecimal(drSelect["Chd_PR"].ToString());
                    }
                    iValue = (iCommission * iTDSValue / 100);
                }
            }
            catch
            {

            }

            iValue = DBCommon.CommonFunction.AvgAmount1(iValue.ToString());
            return iValue;
        }
        private static Decimal Deduct_Service_Tax(Decimal iTotalCommission, DataTable dtST)
        {
            Decimal iValue = iTotalCommission;
            try
            {
                if (dtST != null)
                {
                    Decimal iServiceTaxValue = Convert.ToDecimal(dtST.Rows[0]["Flight_ServiceTax"].ToString());
                    iValue = (iTotalCommission * iServiceTaxValue / 100);
                    iTotalCommission = iTotalCommission - iValue;
                }
            }
            catch
            {

            }

            iValue = DBCommon.CommonFunction.AvgAmount(iTotalCommission.ToString());
            return iValue;
        }
        public static Decimal SA_Deal_Percent(string CarrierCode, Int32 iDealAmount, DataTable dtCommission_SA)
        {
            Decimal iValue = 0;
            try
            {
                if (dtCommission_SA != null && dtCommission_SA.Rows.Count > 0)
                {
                    DataRow[] drComm = dtCommission_SA.Select("CarrierCode='" + CarrierCode + "'");
                    if (drComm.Length > 0)
                    {
                        Decimal iDeal = 0;
                        Decimal.TryParse(drComm.CopyToDataTable().Rows[0]["Commission"].ToString(), out iDeal);

                        if (iDealAmount > 0 && iDeal > 0)
                        {
                            iValue = (iDealAmount) * (iDeal) / 100;
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }

            iValue = DBCommon.CommonFunction.AvgAmount(iValue.ToString());
            return iValue;
        }
        public static Decimal SA_TDS(Decimal iCommission, DataTable dtTDS)
        {
            Decimal iValue = 0;
            try
            {
                if (dtTDS != null)
                {
                    Decimal iTDSValue = Convert.ToDecimal(dtTDS.Rows[0]["Flight_TDS"].ToString());
                    iValue = (iCommission * iTDSValue / 100);
                }
            }
            catch
            {

            }

            iValue = DBCommon.CommonFunction.AvgAmount1(iValue.ToString());
            return iValue;
        }
        public static Int32 C_Deal(string CarrierCode, DataTable dtCommission_C)
        {
            Int32 iValue = 0;
            try
            {
                if (dtCommission_C != null && dtCommission_C.Rows.Count > 0)
                {
                    DataRow[] drComm = dtCommission_C.Select("CarrierCode='" + CarrierCode + "'");
                    if (drComm.Length > 0)
                    {
                        iValue = Int32.Parse(drComm.CopyToDataTable().Rows[0]["CB"].ToString());
                    }
                }
            }
            catch
            {

            }
            return iValue;
        }
        public static Decimal C_Deal_Percent(string CarrierCode, Int32 iDealAmount, DataTable dtCommission_SA)
        {
            Decimal iValue = 0;
            try
            {
                if (dtCommission_SA != null && dtCommission_SA.Rows.Count > 0)
                {
                    DataRow[] drComm = dtCommission_SA.Select("CarrierCode='" + CarrierCode + "'");
                    if (drComm.Length > 0)
                    {
                        Decimal iDeal = 0;
                        Decimal.TryParse(drComm.CopyToDataTable().Rows[0]["CB"].ToString(), out iDeal);

                        if (iDealAmount > 0 && iDeal > 0)
                        {
                            iValue = (iDealAmount) * (iDeal) / 100;
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }

            iValue = DBCommon.CommonFunction.AvgAmount(iValue.ToString());
            return iValue;
        }
        //=============================================================================================================================================
        public static bool Valid_Origin(DataRow drSelect, DataTable dtCommission, DataTable dtCountry)
        {
            bool Valid = true;
            try
            {
                if (dtCommission != null && dtCommission.Rows.Count > 0)
                {
                    DataRow[] drComm = dtCommission.Select("CarrierCode='" + drSelect["CarrierCode"].ToString() + "'");
                    if (drComm.Length > 0)
                    {
                        string CarrierCode = drSelect["CarrierCode"].ToString().Trim().ToUpper();
                        string Origin = drSelect["Origin"].ToString().Trim().ToUpper();
                        string Destination = drSelect["Destination"].ToString().Trim().ToUpper();
                        string OriginCountry = "(" + dtCountry.Select("airportcode='" + Origin + "' Or citycode='" + Origin + "'").CopyToDataTable().Rows[0]["CountryCode"].ToString().ToUpper().Trim() + ")";
                        foreach (DataRow dr in drComm.CopyToDataTable().Rows)
                        {
                            if (dr["OriginCountry"].ToString().Trim().Length > 0)
                            {
                                if (dr["OriginCountry"].ToString().Trim().IndexOf(OriginCountry) != -1)
                                {
                                    Valid = true;
                                }
                                else
                                {
                                    Valid = false;
                                }
                            }
                        }
                    }
                }
            }
            catch
            {

            }
            return Valid;
        }
        public static bool Valid_Destination(DataRow drSelect, DataTable dtCommission, DataTable dtCountry)
        {
            bool Valid = true;
            try
            {
                if (dtCommission != null && dtCommission.Rows.Count > 0)
                {
                    DataRow[] drComm = dtCommission.Select("CarrierCode='" + drSelect["CarrierCode"].ToString() + "'");
                    if (drComm.Length > 0)
                    {
                        string CarrierCode = drSelect["CarrierCode"].ToString().Trim().ToUpper();
                        string Origin = drSelect["Origin"].ToString().Trim().ToUpper();
                        string Destination = drSelect["Destination"].ToString().Trim().ToUpper();
                        string DestinationCountry = "(" + dtCountry.Select("airportcode='" + Destination + "' Or citycode='" + Destination + "'").CopyToDataTable().Rows[0]["CountryCode"].ToString().ToUpper().Trim() + ")";
                        foreach (DataRow dr in drComm.CopyToDataTable().Rows)
                        {
                            if (dr["DestinationCountry"].ToString().Trim().Length > 0)
                            {
                                if (dr["DestinationCountry"].ToString().Trim().IndexOf(DestinationCountry) != -1)
                                {
                                    Valid = false;
                                }
                                else
                                {
                                    Valid = true;
                                }
                            }
                        }
                    }
                }
            }
            catch
            {

            }
            return Valid;
        }
        public static bool Valid_BookingDate(DataRow drSelect, DataTable dtCommission)
        {
            bool Valid = true;
            try
            {
                if (dtCommission != null && dtCommission.Rows.Count > 0)
                {
                    DataRow[] drComm = dtCommission.Select("CarrierCode='" + drSelect["CarrierCode"].ToString() + "'");
                    if (drComm.Length > 0)
                    {
                        foreach (DataRow dr in drComm.CopyToDataTable().Rows)
                        {
                            if (dr["BookingStartDate"] != null && dr["BookingEndDate"] != null)
                            {
                                DateTime startDate;
                                DateTime endDate;
                                DateTime dateToCheck = System.DateTime.Today;

                                bool b1 = DateTime.TryParse(dr["BookingStartDate"].ToString(), out startDate);
                                bool b2 = DateTime.TryParse(dr["BookingEndDate"].ToString(), out endDate);
                                if (b1.Equals(true) && b2.Equals(true))
                                {
                                    if (dateToCheck >= startDate && dateToCheck < endDate)
                                    {

                                    }
                                    else
                                    {
                                        Valid = false;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch
            {

            }
            return Valid;
        }
        public static bool Valid_TripDate(DataRow drSelect, DataTable dtCommission)
        {
            bool Valid = true;
            try
            {
                if (dtCommission != null && dtCommission.Rows.Count > 0)
                {
                    DataRow[] drComm = dtCommission.Select("CarrierCode='" + drSelect["CarrierCode"].ToString() + "'");
                    if (drComm.Length > 0)
                    {
                        foreach (DataRow dr in drComm.CopyToDataTable().Rows)
                        {
                            if (dr["TripStartDate"] != null && dr["TripEndDate"] != null)
                            {
                                DateTime startDate;
                                DateTime endDate;
                                DateTime dateToCheck = Convert.ToDateTime(drSelect["DepartureDate"].ToString());

                                bool b1 = DateTime.TryParse(dr["TripStartDate"].ToString(), out startDate);
                                bool b2 = DateTime.TryParse(dr["TripEndDate"].ToString(), out endDate);
                                if (b1.Equals(true) && b2.Equals(true))
                                {
                                    if (dateToCheck >= startDate && dateToCheck < endDate)
                                    {

                                    }
                                    else
                                    {
                                        Valid = false;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch
            {

            }
            return Valid;
        }
        public static bool Valid_Class_Not(DataRow drSelect, DataTable dtCommission)
        {
            bool Valid = true;
            try
            {
                if (dtCommission != null && dtCommission.Rows.Count > 0)
                {
                    DataRow[] drComm = dtCommission.Select("CarrierCode='" + drSelect["CarrierCode"].ToString() + "'");
                    if (drComm.Length > 0)
                    {
                        string Class = drSelect["ClassOfService"].ToString().Trim().ToUpper();
                        foreach (DataRow dr in drComm.CopyToDataTable().Rows)
                        {
                            if (dr["BookingClass_notValid"].ToString().Trim().Length > 0 && dr["BookingClass_notValid"].ToString().Trim().IndexOf(Class) != -1)
                            {
                                Valid = false;
                                break;
                            }
                        }
                    }
                }
            }
            catch
            {

            }
            return Valid;
        }
        public static void Calculate_decimal_to_int_fare(string SearchID, string CompanyID, string SearchCriteria, DataTable dtBound)
        {
            try
            {
                short Adt = short.Parse(dtBound.Rows[0]["Adt"].ToString().Trim());
                short Chd = short.Parse(dtBound.Rows[0]["Chd"].ToString().Trim());
                short Inf = short.Parse(dtBound.Rows[0]["Inf"].ToString().Trim());
                foreach (DataRow dr in dtBound.Rows)
                {
                    if (dr["Adt_BASIC"].ToString().IndexOf(".") != -1)
                    {
                        dr["Adt_BASIC"] = DBCommon.CommonFunction.AvgAmount1(dr["Adt_BASIC"].ToString());
                    }
                    if (dr["Adt_YQ"].ToString().IndexOf(".") != -1)
                    {
                        dr["Adt_YQ"] = DBCommon.CommonFunction.AvgAmount1(dr["Adt_YQ"].ToString());
                    }
                    if (dr["Adt_PSF"].ToString().IndexOf(".") != -1)
                    {
                        dr["Adt_PSF"] = DBCommon.CommonFunction.AvgAmount1(dr["Adt_PSF"].ToString());
                    }
                    if (dr["Adt_UDF"].ToString().IndexOf(".") != -1)
                    {
                        dr["Adt_UDF"] = DBCommon.CommonFunction.AvgAmount1(dr["Adt_UDF"].ToString());
                    }
                    if (dr["Adt_AUDF"].ToString().IndexOf(".") != -1)
                    {
                        dr["Adt_AUDF"] = DBCommon.CommonFunction.AvgAmount1(dr["Adt_AUDF"].ToString());
                    }
                    if (dr["Adt_CUTE"].ToString().IndexOf(".") != -1)
                    {
                        dr["Adt_CUTE"] = DBCommon.CommonFunction.AvgAmount1(dr["Adt_CUTE"].ToString());
                    }
                    if (dr["Adt_GST"].ToString().IndexOf(".") != -1)
                    {
                        dr["Adt_GST"] = DBCommon.CommonFunction.AvgAmount1(dr["Adt_GST"].ToString());
                    }
                    if (dr["Adt_TF"].ToString().IndexOf(".") != -1)
                    {
                        dr["Adt_TF"] = DBCommon.CommonFunction.AvgAmount1(dr["Adt_TF"].ToString());
                    }
                    if (dr["Adt_CESS"].ToString().IndexOf(".") != -1)
                    {
                        dr["Adt_CESS"] = DBCommon.CommonFunction.AvgAmount1(dr["Adt_CESS"].ToString());
                    }
                    if (dr["Adt_EX"].ToString().IndexOf(".") != -1)
                    {
                        dr["Adt_EX"] = DBCommon.CommonFunction.AvgAmount1(dr["Adt_EX"].ToString());
                    }

                    if (Chd > 0)
                    {
                        if (dr["Chd_BASIC"].ToString().IndexOf(".") != -1)
                        {
                            dr["Chd_BASIC"] = DBCommon.CommonFunction.AvgAmount1(dr["Chd_BASIC"].ToString());
                        }
                        if (dr["Chd_YQ"].ToString().IndexOf(".") != -1)
                        {
                            dr["Chd_YQ"] = DBCommon.CommonFunction.AvgAmount1(dr["Chd_YQ"].ToString());
                        }
                        if (dr["Chd_PSF"].ToString().IndexOf(".") != -1)
                        {
                            dr["Chd_PSF"] = DBCommon.CommonFunction.AvgAmount1(dr["Chd_PSF"].ToString());
                        }
                        if (dr["Chd_UDF"].ToString().IndexOf(".") != -1)
                        {
                            dr["Chd_UDF"] = DBCommon.CommonFunction.AvgAmount1(dr["Chd_UDF"].ToString());
                        }
                        if (dr["Chd_AUDF"].ToString().IndexOf(".") != -1)
                        {
                            dr["Chd_AUDF"] = DBCommon.CommonFunction.AvgAmount1(dr["Chd_AUDF"].ToString());
                        }
                        if (dr["Chd_CUTE"].ToString().IndexOf(".") != -1)
                        {
                            dr["Chd_CUTE"] = DBCommon.CommonFunction.AvgAmount1(dr["Chd_CUTE"].ToString());
                        }
                        if (dr["Chd_GST"].ToString().IndexOf(".") != -1)
                        {
                            dr["Chd_GST"] = DBCommon.CommonFunction.AvgAmount1(dr["Chd_GST"].ToString());
                        }
                        if (dr["Chd_TF"].ToString().IndexOf(".") != -1)
                        {
                            dr["Chd_TF"] = DBCommon.CommonFunction.AvgAmount1(dr["Chd_TF"].ToString());
                        }
                        if (dr["Chd_CESS"].ToString().IndexOf(".") != -1)
                        {
                            dr["Chd_CESS"] = DBCommon.CommonFunction.AvgAmount1(dr["Chd_CESS"].ToString());
                        }
                        if (dr["Chd_EX"].ToString().IndexOf(".") != -1)
                        {
                            dr["Chd_EX"] = DBCommon.CommonFunction.AvgAmount1(dr["Chd_EX"].ToString());
                        }
                    }

                    if (Inf > 0)
                    {
                        if (dr["Inf_BASIC"].ToString().IndexOf(".") != -1)
                        {
                            dr["Inf_BASIC"] = DBCommon.CommonFunction.AvgAmount1(dr["Inf_BASIC"].ToString());
                        }
                        if (dr["Inf_TAX"].ToString().IndexOf(".") != -1)
                        {
                            dr["Inf_TAX"] = DBCommon.CommonFunction.AvgAmount1(dr["Inf_TAX"].ToString());
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                DBCommon.Logger.dbLogg(CompanyID, 0, "Calculate_decimal_to_int_fare", "CAL", SearchCriteria, SearchID, ex.Message);
            }

            dtBound.AcceptChanges();
        }
    }
}
