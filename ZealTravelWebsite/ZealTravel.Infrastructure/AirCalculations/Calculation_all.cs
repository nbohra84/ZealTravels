using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZealTravel.Infrastructure.AirCalculations
{
    class Calculation_all
    {
        public void CalculateDeal(string SearchCriteria, bool IsOnline, bool IsDeal, DataTable dtBound, DataTable dtCommission, DataTable dtST, DataTable dtTDS, DataTable dtCountry, DataTable dtMU, string UserType, DataTable dtCommission_C, bool IsSOTO, DataTable dtPriceTypeCommission, DataTable dtCfee, DataTable dtAGmarkup, bool IsApiUser)
        {
            DataTable dtExtraCommission = dbCommission.SupplierwiseExtraCommission(dtBound.Rows[0]["Sector"].ToString().Trim());
            ArrayList arRefId = DBCommon.CommonFunction.DataTable2ArrayList(dtBound, "RefID", true);
            for (int i = 0; i < arRefId.Count; i++)
            {
                CalculateDeal(SearchCriteria, IsOnline, IsDeal, arRefId[i].ToString(), dtBound, dtCommission, dtST, dtTDS, dtCountry, dtMU, UserType, dtCommission_C, IsSOTO, dtPriceTypeCommission, dtCfee, dtExtraCommission, dtAGmarkup, IsApiUser);
            }
        }
        private void CalculateDeal(string SearchCriteria, bool IsOnline, bool IsDeal, string RefID, DataTable dtBound, DataTable dtCommission, DataTable dtST, DataTable dtTDS, DataTable dtCountry, DataTable dtMU, string UserType, DataTable dtCommission_C, bool IsSOTO, DataTable dtPriceTypeCommission, DataTable dtCfee, DataTable dtExtraCommission, DataTable dtAGmarkup, bool IsApiUser)
        {
            DataRow[] rows = dtBound.Select("RefID ='" + RefID + "'");
            if (rows.Length > 0)
            {
                Decimal Adt_BAS = 0;
                Decimal Adt_Y = 0;
                Decimal Adt_BAS_Iata = 0;
                Decimal Adt_Y_Iata = 0;
                Decimal Adt_CB = 0;
                Decimal Adt_PR = 0;
                Decimal Adt_MU = 0;
                Decimal Adt_SF = 0;
                Decimal Adt_ST = 0;
                Decimal Adt_TDS = 0;
                Decimal cfee = 0;

                Decimal Chd_BAS = 0;
                Decimal Chd_Y = 0;
                Decimal Chd_BAS_Iata = 0;
                Decimal Chd_Y_Iata = 0;
                Decimal Chd_CB = 0;
                Decimal Chd_PR = 0;
                Decimal Chd_MU = 0;
                Decimal Chd_SF = 0;
                Decimal Chd_ST = 0;
                Decimal Chd_TDS = 0;
                int count = 0;

                int AG_Markup = 0;


                foreach (DataRow dr in rows)
                {
                    int Adt = int.Parse(dr["Adt"].ToString());
                    int Chd = int.Parse(dr["Chd"].ToString());
                    int Inf = int.Parse(dr["Inf"].ToString());
                    string cc = dr["CarrierCode"].ToString();
                    if (cc.Equals("CI") || cc.Equals("CA"))
                    {

                    }

                    if (count.Equals(0))
                    {
                        if (IsDeal.Equals(true))
                        {
                            if (IsOnline.Equals(true))
                            {
                                // online deal calculation
                                if (dr["Sector"].ToString().IndexOf("I") != -1)
                                {
                                    // online inetrnational deal
                                    Decimal BasicIata = 0;
                                    Decimal YQIata = 0;
                                    if (IsSOTO.Equals(false))
                                    {
                                        Adt_BAS_Iata = CommissionCalC.Basic_Iata("A", dr, dtCommission, dtST, out BasicIata);
                                        Adt_Y_Iata = CommissionCalC.YQ_Iata("A", dr, dtCommission, dtST, out YQIata);
                                    }

                                    bool Valid_Origin = CommissionCalC.Valid_Origin(dr, dtCommission, dtCountry);
                                    if (Valid_Origin.Equals(true))
                                    {
                                        bool Valid_Destination = CommissionCalC.Valid_Destination(dr, dtCommission, dtCountry);
                                        if (Valid_Destination.Equals(true))
                                        {
                                            bool Valid_BookingDate = CommissionCalC.Valid_BookingDate(dr, dtCommission);
                                            if (Valid_BookingDate.Equals(true))
                                            {
                                                bool Valid_TripDate = CommissionCalC.Valid_TripDate(dr, dtCommission);
                                                if (Valid_TripDate.Equals(true))
                                                {
                                                    bool Valid_Class_Not = CommissionCalC.Valid_Class_Not(dr, dtCommission);
                                                    if (Valid_Class_Not.Equals(true))
                                                    {
                                                        Adt_BAS = CommissionCalC.Basic("A", dr, dtCommission, BasicIata, dtST, dtPriceTypeCommission);
                                                        Adt_Y = CommissionCalC.YQ("A", dr, dtCommission, YQIata, dtST, dtPriceTypeCommission);
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    // online domestic deal
                                    Decimal BasicIata = 0;
                                    Decimal YQIata = 0;
                                    Adt_BAS_Iata = CommissionCalC.Basic_Iata("A", dr, dtCommission, dtST, out BasicIata);
                                    Adt_Y_Iata = CommissionCalC.YQ_Iata("A", dr, dtCommission, dtST, out YQIata);

                                    bool Valid_Class_Not = CommissionCalC.Valid_Class_Not(dr, dtCommission);
                                    if (Valid_Class_Not.Equals(true))
                                    {
                                        Adt_BAS = CommissionCalC.Basic("A", dr, dtCommission, Adt_BAS_Iata, dtST, dtPriceTypeCommission);
                                        Adt_Y = CommissionCalC.YQ("A", dr, dtCommission, Adt_Y_Iata, dtST, dtPriceTypeCommission);
                                    }
                                }

                                Adt_CB = CommissionCalC.CB("A", dr, dtCommission, dtST);
                                Adt_PR = CommissionCalC.Promo("A", dr, dtCommission, dtST);
                            }
                            else
                            {
                                // offline deal calculation
                                Decimal BasicIata = 0;
                                Decimal YQIata = 0;
                                Adt_BAS_Iata = CommissionCalC.Basic_Iata_Offline("A", dr, Convert.ToDecimal(dr["Adt_BAS"].ToString()), dtST, out BasicIata);
                                Adt_Y_Iata = CommissionCalC.YQ_Iata_Offline("A", dr, Convert.ToDecimal(dr["Adt_Y"].ToString()), dtST, out YQIata);

                                Adt_BAS = CommissionCalC.Basic_offline("A", dr, Convert.ToDecimal(dr["Adt_ST"].ToString()), BasicIata, dtST);
                                Adt_Y = CommissionCalC.YQ_offline("A", dr, Convert.ToDecimal(dr["Adt_TDS"].ToString()), YQIata, dtST);

                                Adt_CB = Convert.ToInt32(dr["Adt_CB"].ToString());
                                Adt_PR = Convert.ToInt32(dr["Adt_PR"].ToString());
                            }
                        }

                        if (UserType.Equals("B2B2C") || UserType.Equals("B2C"))
                        {
                            Adt_MU = CommissionCalC.MarkupC("A", dr, dtCommission_C);// markup for customer
                        }
                        else
                        {
                            Adt_MU = CommissionCalC.Markup("A", dr, dtMU);// markup for company n Subcompany
                        }

                        if (IsOnline.Equals(true))
                        {
                            Adt_SF = CommissionCalC.ServiceFee("A", dr, dtCommission, dtPriceTypeCommission); // service fee for company/sub/customers
                        }
                        else
                        {
                            Adt_SF = Convert.ToDecimal(dr["Adt_SF"].ToString().Trim()); // service fee for offline Booking Admin
                        }

                        if (UserType.Equals("B2B2C") || UserType.Equals("B2C") || UserType.Equals("B2B2B"))
                        {
                            cfee = CommissionCalC.GET_Cfee(UserType, dr, dtCfee);
                        }

                        if (UserType.Equals("B2B2B"))
                        {
                            AG_Markup = CommissionCalC.MarkupAgentToSubAgent(dr, dtAGmarkup);// markup to subagency
                        }
                    }

                    if (dr.Table.Columns.Contains("AG_Markup"))
                    {
                        dr["AG_Markup"] = AG_Markup;
                    }

                    if (dr.Table.Columns.Contains("TotalCfee"))
                    {
                        dr["TotalCfee"] = cfee;
                    }

                    dr["Adt_BAS"] = Adt_BAS + Adt_BAS_Iata;
                    dr["Adt_Y"] = Adt_Y + Adt_Y_Iata;
                    dr["Adt_CB"] = Adt_CB;
                    dr["Adt_PR"] = Adt_PR;


                    if (UserType.Equals("B2B") && IsApiUser)
                    {
                        Adt_MU = 0;
                    }

                    dr["Adt_MU"] = Adt_MU;
                    dr["Adt_SF"] = Adt_SF;

                    int k = 0;
                    if (dtExtraCommission != null && dtExtraCommission.Rows.Count > 0)
                    {
                        DataRow[] drResults = dtExtraCommission.Select("SupplierID='" + dr["AirlineID"].ToString().ToString() + "'");
                        if (drResults.Length > 0)
                        {
                            DataRow[] drSelect = drResults.CopyToDataTable().Select("SupplierID='" + dr["AirlineID"].ToString().ToString() + "' AND CarrierCode='" + cc + "'");
                            if (drSelect.Length > 0 && Convert.ToInt32(drSelect.CopyToDataTable().Rows[0]["ExtraCommission"].ToString()) > 0)
                            {
                                k = 1;

                                int iExtraCommission = Convert.ToInt32(drSelect.CopyToDataTable().Rows[0]["ExtraCommission"].ToString());
                                Decimal dValue = 0;
                                Decimal.TryParse(dr["API_SearchID"].ToString(), out dValue);
                                int iApiCommission = Decimal.ToInt32(dValue);
                                //int iTCommission = Decimal.ToInt32(Adt_BAS) + Decimal.ToInt32(Adt_BAS_Iata) + Decimal.ToInt32(Adt_Y) + Decimal.ToInt32(Adt_Y_Iata) + Decimal.ToInt32(Adt_CB);

                                dr["Adt_BAS"] = (iApiCommission + iExtraCommission);
                                dr["Adt_Y"] = 0;
                                dr["Adt_CB"] = 0;
                                dr["Adt_PR"] = 0;
                            }

                            if (k.Equals(0))
                            {
                                Decimal dValue = 0;
                                Decimal.TryParse(dr["API_SearchID"].ToString(), out dValue);
                                int iValue = Decimal.ToInt32(dValue);
                                if ((Adt_BAS + Adt_BAS_Iata + Adt_Y + Adt_Y_Iata + Adt_CB) > iValue)
                                {
                                    dr["Adt_BAS"] = iValue;
                                    dr["Adt_Y"] = 0;
                                    dr["Adt_CB"] = 0;
                                    dr["Adt_PR"] = 0;
                                }
                            }
                        }
                    }




                    //if (dr["AirlineID"].ToString().IndexOf("DELV568") != -1 || dr["AirlineID"].ToString().IndexOf("47159412") != -1)
                    //{
                    //    if (dr["Sector"].ToString().IndexOf("D") != -1 && (cc.Equals("UK") || cc.Equals("AI") || cc.Equals("SG") || cc.Equals("6E") || cc.Equals("G8") || cc.Equals("I5")))
                    //    {
                    //        Decimal dValue = 0;
                    //        Decimal.TryParse(dr["API_SearchID"].ToString(), out dValue);
                    //        int iValue = Decimal.ToInt32(dValue);
                    //        if ((Adt_BAS + Adt_BAS_Iata + Adt_Y + Adt_Y_Iata + Adt_CB) > iValue)
                    //        {
                    //            dr["Adt_BAS"] = iValue;
                    //            dr["Adt_Y"] = 0;
                    //            dr["Adt_CB"] = 0;
                    //            dr["Adt_PR"] = 0;
                    //        }
                    //    }
                    //    else if (dr["Sector"].ToString().IndexOf("I") != -1 && (cc.Equals("SG") || cc.Equals("6E") || cc.Equals("G8")))
                    //    {
                    //        Decimal dValue = 0;
                    //        Decimal.TryParse(dr["API_SearchID"].ToString(), out dValue);
                    //        int iValue = Decimal.ToInt32(dValue);
                    //        if ((Adt_BAS + Adt_BAS_Iata + Adt_Y + Adt_Y_Iata + Adt_CB) > iValue)
                    //        {
                    //            dr["Adt_BAS"] = iValue;
                    //            dr["Adt_Y"] = 0;
                    //            dr["Adt_CB"] = 0;
                    //            dr["Adt_PR"] = 0;
                    //        }
                    //    }
                    //}

                    if (count.Equals(0))
                    {
                        Adt_ST = CommissionCalC.ServiceTax("A", dr, dtST);
                        Adt_TDS = CommissionCalC.TDS("A", dr, dtTDS);
                    }

                    dr["Adt_ST"] = Adt_ST;
                    dr["Adt_TDS"] = Adt_TDS;
                    dr.AcceptChanges();

                    if (Chd > 0)
                    {
                        if (count.Equals(0))
                        {
                            if (IsDeal.Equals(true))
                            {
                                if (IsOnline.Equals(true))
                                {
                                    // Online deal calculation
                                    if (dr["Sector"].ToString().IndexOf("I") != -1)
                                    {
                                        // online International deal
                                        Decimal BasicIata = 0;
                                        Decimal YQIata = 0;
                                        if (IsSOTO.Equals(false))
                                        {
                                            Chd_BAS_Iata = CommissionCalC.Basic_Iata("C", dr, dtCommission, dtST, out BasicIata);
                                            Chd_Y_Iata = CommissionCalC.YQ_Iata("C", dr, dtCommission, dtST, out YQIata);
                                        }

                                        bool Valid_Origin = CommissionCalC.Valid_Origin(dr, dtCommission, dtCountry);
                                        if (Valid_Origin.Equals(true))
                                        {
                                            bool Valid_Destination = CommissionCalC.Valid_Destination(dr, dtCommission, dtCountry);
                                            if (Valid_Destination.Equals(true))
                                            {
                                                bool Valid_BookingDate = CommissionCalC.Valid_BookingDate(dr, dtCommission);
                                                if (Valid_BookingDate.Equals(true))
                                                {
                                                    bool Valid_TripDate = CommissionCalC.Valid_TripDate(dr, dtCommission);
                                                    if (Valid_TripDate.Equals(true))
                                                    {
                                                        bool Valid_Class_Not = CommissionCalC.Valid_Class_Not(dr, dtCommission);
                                                        if (Valid_Class_Not.Equals(true))
                                                        {
                                                            Chd_BAS = CommissionCalC.Basic("C", dr, dtCommission, BasicIata, dtST, dtPriceTypeCommission);
                                                            Chd_Y = CommissionCalC.YQ("C", dr, dtCommission, YQIata, dtST, dtPriceTypeCommission);
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    else
                                    {
                                        // online domestic deal
                                        Decimal BasicIata = 0;
                                        Decimal YQIata = 0;
                                        Chd_BAS_Iata = CommissionCalC.Basic_Iata("C", dr, dtCommission, dtST, out BasicIata);
                                        Chd_Y_Iata = CommissionCalC.YQ_Iata("C", dr, dtCommission, dtST, out YQIata);

                                        bool Valid_Class_Not = CommissionCalC.Valid_Class_Not(dr, dtCommission);
                                        if (Valid_Class_Not.Equals(true))
                                        {
                                            Chd_BAS = CommissionCalC.Basic("C", dr, dtCommission, Chd_BAS_Iata, dtST, dtPriceTypeCommission);
                                            Chd_Y = CommissionCalC.YQ("C", dr, dtCommission, Chd_Y_Iata, dtST, dtPriceTypeCommission);
                                        }
                                    }

                                    Chd_CB = CommissionCalC.CB("C", dr, dtCommission, dtST);
                                    Chd_PR = CommissionCalC.Promo("C", dr, dtCommission, dtST);
                                }
                                else
                                {
                                    // offline deal calculation
                                    Decimal BasicIata = 0;
                                    Decimal YQIata = 0;
                                    Chd_BAS_Iata = CommissionCalC.Basic_Iata_Offline("C", dr, Convert.ToDecimal(dr["Chd_BAS"].ToString()), dtST, out BasicIata);
                                    Chd_Y_Iata = CommissionCalC.YQ_Iata_Offline("C", dr, Convert.ToDecimal(dr["Chd_Y"].ToString()), dtST, out YQIata);

                                    Chd_BAS = CommissionCalC.Basic_offline("C", dr, Convert.ToDecimal(dr["Chd_ST"].ToString()), BasicIata, dtST);
                                    Chd_Y = CommissionCalC.YQ_offline("C", dr, Convert.ToDecimal(dr["Chd_TDS"].ToString()), YQIata, dtST);

                                    Chd_CB = Convert.ToInt32(dr["Chd_CB"].ToString());
                                    Chd_PR = Convert.ToInt32(dr["Chd_PR"].ToString());
                                }
                            }

                            if (UserType.Equals("B2B2C") || UserType.Equals("B2C"))
                            {
                                Chd_MU = CommissionCalC.MarkupC("C", dr, dtCommission_C);// markup for customer
                            }
                            else
                            {
                                Chd_MU = CommissionCalC.Markup("C", dr, dtMU);// markup for Company/Subcompany
                            }

                            if (IsOnline.Equals(true))
                            {
                                Chd_SF = CommissionCalC.ServiceFee("C", dr, dtCommission, dtPriceTypeCommission);//service fee for company/sub/customer
                            }
                            else
                            {
                                Chd_SF = Convert.ToDecimal(dr["Chd_SF"].ToString().Trim());//service fee for offline booking admin
                            }
                        }

                        dr["Chd_BAS"] = Chd_BAS + Chd_BAS_Iata;
                        dr["Chd_Y"] = Chd_Y + Chd_Y_Iata;
                        dr["Chd_CB"] = Chd_CB;
                        dr["Chd_PR"] = Chd_PR;

                        if (UserType.Equals("B2B") && IsApiUser)
                        {
                            Chd_MU = 0;
                        }

                        dr["Chd_MU"] = Chd_MU;
                        dr["Chd_SF"] = Chd_SF;

                        k = 0;
                        if (dtExtraCommission != null && dtExtraCommission.Rows.Count > 0)
                        {
                            DataRow[] drResults = dtExtraCommission.Select("SupplierID='" + dr["AirlineID"].ToString().ToString() + "'");
                            if (drResults.Length > 0)
                            {
                                DataRow[] drSelect = drResults.CopyToDataTable().Select("SupplierID='" + dr["AirlineID"].ToString().ToString() + "' AND CarrierCode='" + cc + "'");
                                if (drSelect.Length > 0 && Convert.ToInt32(drSelect.CopyToDataTable().Rows[0]["ExtraCommission"].ToString()) > 0)
                                {
                                    k = 1;

                                    int iExtraCommission = Convert.ToInt32(drSelect.CopyToDataTable().Rows[0]["ExtraCommission"].ToString());
                                    Decimal dValue = 0;
                                    Decimal.TryParse(dr["API_SearchID"].ToString(), out dValue);
                                    int iApiCommission = Decimal.ToInt32(dValue);
                                    //int iTCommission = Decimal.ToInt32(Adt_BAS) + Decimal.ToInt32(Adt_BAS_Iata) + Decimal.ToInt32(Adt_Y) + Decimal.ToInt32(Adt_Y_Iata) + Decimal.ToInt32(Adt_CB);

                                    dr["Chd_BAS"] = (iApiCommission + iExtraCommission);
                                    dr["Chd_Y"] = 0;
                                    dr["Chd_CB"] = 0;
                                    dr["Chd_PR"] = 0;
                                }

                                if (k.Equals(0))
                                {
                                    Decimal dValue = 0;
                                    Decimal.TryParse(dr["API_SearchID"].ToString(), out dValue);
                                    int iValue = Decimal.ToInt32(dValue);
                                    if ((Chd_BAS + Chd_BAS_Iata + Chd_Y + Chd_Y_Iata + Chd_CB) > iValue)
                                    {
                                        dr["Chd_BAS"] = iValue;
                                        dr["Chd_Y"] = 0;
                                        dr["Chd_CB"] = 0;
                                        dr["Chd_PR"] = 0;
                                    }
                                }
                            }
                        }

                        //if (dr["AirlineID"].ToString().IndexOf("DELV568") != -1 || dr["AirlineID"].ToString().IndexOf("47159412") != -1)
                        //{
                        //    if (dr["Sector"].ToString().IndexOf("D") != -1 && (cc.Equals("UK") || cc.Equals("AI") || cc.Equals("SG") || cc.Equals("6E") || cc.Equals("G8") || cc.Equals("I5")))
                        //    {
                        //        Decimal dValue = 0;
                        //        Decimal.TryParse(dr["API_SearchID"].ToString(), out dValue);
                        //        int iValue = Decimal.ToInt32(dValue);
                        //        if ((Chd_BAS + Chd_BAS_Iata + Chd_Y + Chd_Y_Iata + Chd_CB) > iValue)
                        //        {
                        //            dr["Chd_BAS"] = iValue;
                        //            dr["Chd_Y"] = 0;
                        //            dr["Chd_CB"] = 0;
                        //            dr["Chd_PR"] = 0;
                        //        }
                        //    }
                        //    else if (dr["Sector"].ToString().IndexOf("I") != -1 && (cc.Equals("SG") || cc.Equals("6E") || cc.Equals("G8")))
                        //    {
                        //        Decimal dValue = 0;
                        //        Decimal.TryParse(dr["API_SearchID"].ToString(), out dValue);
                        //        int iValue = Decimal.ToInt32(dValue);
                        //        if ((Chd_BAS + Chd_BAS_Iata + Chd_Y + Chd_Y_Iata + Chd_CB) > iValue)
                        //        {
                        //            dr["Chd_BAS"] = iValue;
                        //            dr["Chd_Y"] = 0;
                        //            dr["Chd_CB"] = 0;
                        //            dr["Chd_PR"] = 0;
                        //        }
                        //    }
                        //}

                        if (count.Equals(0))
                        {
                            Chd_ST = CommissionCalC.ServiceTax("C", dr, dtST);
                            Chd_TDS = CommissionCalC.TDS("C", dr, dtTDS);
                        }

                        dr["Chd_ST"] = Chd_ST;
                        dr["Chd_TDS"] = Chd_TDS;
                        dr.AcceptChanges();
                    }

                    dtBound.AcceptChanges();
                    count++;
                }
            }
        }
        //=================================================================================================================================================
        public void getCdeal(string SearchCriteria, string CompanyID, DataTable dtBound, DataTable dtCommission_C)
        {
            foreach (DataRow dr in dtBound.Rows)
            {
                int Adt = int.Parse(dr["Adt"].ToString());
                int Chd = int.Parse(dr["Chd"].ToString());
                int Inf = int.Parse(dr["Inf"].ToString());

                Int32 Adt_BAS = 0;
                Int32 Adt_Y = 0;
                Int32 Adt_CB = 0;   //Convert.ToInt32(CommissionCalC.C_Deal(dr["CarrierCode"].ToString(), dtCommission_C));
                Int32 Adt_PR = 0;
                Int32 Adt_TDS = 0;

                Int32 Chd_BAS = 0;
                Int32 Chd_Y = 0;
                Int32 Chd_CB = 0;
                Int32 Chd_PR = 0;
                Int32 Chd_TDS = 0;

                //if (Chd > 0)
                //{
                //    Chd_BAS = 0;
                //    Chd_Y = 0;
                //    Chd_CB = Convert.ToInt32(CommissionCalC.C_Deal(dr["CarrierCode"].ToString(), dtCommission_C));
                //    Chd_PR = 0;
                //}

                Adt_CB += Convert.ToInt32(CommissionCalC.C_Deal_Percent(dr["CarrierCode"].ToString(), Convert.ToInt32(dr["Adt_BAS"].ToString()), dtCommission_C));
                Adt_CB += Convert.ToInt32(CommissionCalC.C_Deal_Percent(dr["CarrierCode"].ToString(), Convert.ToInt32(dr["Adt_Y"].ToString()), dtCommission_C));
                Adt_CB += Convert.ToInt32(CommissionCalC.C_Deal_Percent(dr["CarrierCode"].ToString(), Convert.ToInt32(dr["Adt_CB"].ToString()), dtCommission_C));
                Adt_CB += Convert.ToInt32(CommissionCalC.C_Deal_Percent(dr["CarrierCode"].ToString(), Convert.ToInt32(dr["Adt_PR"].ToString()), dtCommission_C));

                if (Chd > 0)
                {
                    Chd_CB += Convert.ToInt32(CommissionCalC.C_Deal_Percent(dr["CarrierCode"].ToString(), Convert.ToInt32(dr["Chd_BAS"].ToString()), dtCommission_C));
                    Chd_CB += Convert.ToInt32(CommissionCalC.C_Deal_Percent(dr["CarrierCode"].ToString(), Convert.ToInt32(dr["Chd_Y"].ToString()), dtCommission_C));
                    Chd_CB += Convert.ToInt32(CommissionCalC.C_Deal_Percent(dr["CarrierCode"].ToString(), Convert.ToInt32(dr["Chd_CB"].ToString()), dtCommission_C));
                    Chd_CB += Convert.ToInt32(CommissionCalC.C_Deal_Percent(dr["CarrierCode"].ToString(), Convert.ToInt32(dr["Chd_PR"].ToString()), dtCommission_C));
                }

                string add = Adt_TDS + ",";
                add += Adt_BAS + ",";
                add += Adt_Y + ",";
                add += Adt_CB + ",";
                add += Adt_PR + "*";
                add += Chd_TDS + ",";
                add += Chd_BAS + ",";
                add += Chd_Y + ",";
                add += Chd_CB + ",";
                add += Chd_PR;

                Decimal iTotalCommission = Adt * Adt_BAS + Chd * Chd_BAS;
                iTotalCommission += Adt * Adt_Y + Chd * Chd_Y;
                iTotalCommission += Adt * Adt_CB + Chd * Chd_CB;
                iTotalCommission += Adt * Adt_PR + Chd * Chd_PR;

                dr["TotalCommission_SA"] = iTotalCommission;
                dr["TotalTds_SA"] = 0;
                dr["SA_deal"] = add;
            }
            dtBound.AcceptChanges();
        }
        public void getSAdeal(string SearchCriteria, string CompanyID, DataTable dtBound, DataTable dtCommission_SA, DataTable dtTDS)
        {
            foreach (DataRow dr in dtBound.Rows)
            {
                int Adt = int.Parse(dr["Adt"].ToString());
                int Chd = int.Parse(dr["Chd"].ToString());
                int Inf = int.Parse(dr["Inf"].ToString());

                Int32 Adt_BAS = Convert.ToInt32(CommissionCalC.SA_Deal_Percent(dr["CarrierCode"].ToString(), Convert.ToInt32(dr["Adt_BAS"].ToString()), dtCommission_SA));
                Int32 Adt_Y = Convert.ToInt32(CommissionCalC.SA_Deal_Percent(dr["CarrierCode"].ToString(), Convert.ToInt32(dr["Adt_Y"].ToString()), dtCommission_SA));
                Int32 Adt_CB = Convert.ToInt32(CommissionCalC.SA_Deal_Percent(dr["CarrierCode"].ToString(), Convert.ToInt32(dr["Adt_CB"].ToString()), dtCommission_SA));
                Int32 Adt_PR = Convert.ToInt32(CommissionCalC.SA_Deal_Percent(dr["CarrierCode"].ToString(), Convert.ToInt32(dr["Adt_PR"].ToString()), dtCommission_SA));
                Int32 Adt_TDS = Convert.ToInt32(CommissionCalC.SA_TDS((Adt_BAS + Adt_Y + Adt_CB + Adt_PR), dtTDS));

                Int32 Chd_BAS = 0;
                Int32 Chd_Y = 0;
                Int32 Chd_CB = 0;
                Int32 Chd_PR = 0;
                Int32 Chd_TDS = 0;

                if (Chd > 0)
                {
                    Chd_BAS = Convert.ToInt32(CommissionCalC.SA_Deal_Percent(dr["CarrierCode"].ToString(), Convert.ToInt32(dr["Chd_BAS"].ToString()), dtCommission_SA));
                    Chd_Y = Convert.ToInt32(CommissionCalC.SA_Deal_Percent(dr["CarrierCode"].ToString(), Convert.ToInt32(dr["Chd_Y"].ToString()), dtCommission_SA));
                    Chd_CB = Convert.ToInt32(CommissionCalC.SA_Deal_Percent(dr["CarrierCode"].ToString(), Convert.ToInt32(dr["Chd_CB"].ToString()), dtCommission_SA));
                    Chd_PR = Convert.ToInt32(CommissionCalC.SA_Deal_Percent(dr["CarrierCode"].ToString(), Convert.ToInt32(dr["Chd_PR"].ToString()), dtCommission_SA));
                    Chd_TDS = Convert.ToInt32(CommissionCalC.SA_TDS((Chd_BAS + Chd_Y + Chd_CB + Chd_PR), dtTDS));
                }

                string add = Adt_TDS + ",";
                add += Adt_BAS + ",";
                add += Adt_Y + ",";
                add += Adt_CB + ",";
                add += Adt_PR + "*";
                add += Chd_TDS + ",";
                add += Chd_BAS + ",";
                add += Chd_Y + ",";
                add += Chd_CB + ",";
                add += Chd_PR;

                Decimal iTotalCommission = Adt * Adt_BAS + Chd * Chd_BAS;
                iTotalCommission += Adt * Adt_Y + Chd * Chd_Y;
                iTotalCommission += Adt * Adt_CB + Chd * Chd_CB;
                iTotalCommission += Adt * Adt_PR + Chd * Chd_PR;

                dr["TotalCommission_SA"] = iTotalCommission;
                dr["TotalTds_SA"] = (Adt * Adt_TDS + Chd * Chd_TDS);
                dr["SA_deal"] = add;
            }
            dtBound.AcceptChanges();
        }
    }
}
