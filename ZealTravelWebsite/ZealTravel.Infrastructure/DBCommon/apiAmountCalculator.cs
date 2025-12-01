using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace ZealTravel.Infrastructure.DBCommon
{
    public class apiAmountCalculator
    {
        private string SearchCriteria;
        private string SearchID;
        private string CompanyID;
        private Int32 BookingRef;

        public apiAmountCalculator(string SearchID, string SearchCriteria, string CompanyID, Int32 BookingRef)
        {
            this.SearchCriteria = SearchCriteria;
            this.SearchID = SearchID;
            this.CompanyID = CompanyID;
            this.BookingRef = BookingRef;
        }
        public Int32 getSSRmealbaggage(DataTable dtPassenger, string Trip)
        {
            Int32 iTotalSSRFare = 0;
            try
            {
                if (Trip.Equals("O"))
                {
                    DataRow[] drMeal = dtPassenger.Select("MealChg_O > 0 AND (PaxType='" + "ADT" + "' OR PaxType='" + "CHD" + "')");
                    if (drMeal.Length > 0)
                    {
                        foreach (DataRow dr in drMeal.CopyToDataTable().Rows)
                        {
                            iTotalSSRFare += int.Parse(dr["MealChg_O"].ToString().Trim().ToString());
                        }
                    }

                    DataRow[] drBaggage = dtPassenger.Select("BaggageChg_O > 0 AND (PaxType='" + "ADT" + "' OR PaxType='" + "CHD" + "')");
                    if (drBaggage.Length > 0)
                    {
                        foreach (DataRow dr in drBaggage.CopyToDataTable().Rows)
                        {
                            iTotalSSRFare += int.Parse(dr["BaggageChg_O"].ToString().Trim().ToString());
                        }
                    }
                }
                else
                {
                    DataRow[] drMeal = dtPassenger.Select("MealChg_I > 0 AND (PaxType='" + "ADT" + "' OR PaxType='" + "CHD" + "')");
                    if (drMeal.Length > 0)
                    {
                        foreach (DataRow dr in drMeal.CopyToDataTable().Rows)
                        {
                            iTotalSSRFare += int.Parse(dr["MealChg_I"].ToString().Trim().ToString());
                        }
                    }

                    DataRow[] drBaggage = dtPassenger.Select("BaggageChg_I > 0 AND (PaxType='" + "ADT" + "' OR PaxType='" + "CHD" + "')");
                    if (drBaggage.Length > 0)
                    {
                        foreach (DataRow dr in drBaggage.CopyToDataTable().Rows)
                        {
                            iTotalSSRFare += int.Parse(dr["BaggageChg_I"].ToString().Trim().ToString());
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                dbCommon.Logger.dbLogg(CompanyID, BookingRef, "getSSRmealbaggage", "PNR", SearchCriteria, SearchID, ex.Message);
            }
            return iTotalSSRFare;
        }
        public Int32 getSSRmeal(DataTable dtPassenger, string Trip)
        {
            Int32 iTotalSSRFare = 0;

            try
            {
                if (Trip.Equals("O"))
                {
                    DataRow[] drMeal = dtPassenger.Select("MealChg_O > 0 AND (PaxType='" + "ADT" + "' OR PaxType='" + "CHD" + "')");
                    if (drMeal.Length > 0)
                    {
                        foreach (DataRow dr in drMeal.CopyToDataTable().Rows)
                        {
                            iTotalSSRFare += int.Parse(dr["MealChg_O"].ToString().Trim().ToString());
                        }
                    }
                }
                else
                {
                    DataRow[] drMeal = dtPassenger.Select("MealChg_I > 0 AND (PaxType='" + "ADT" + "' OR PaxType='" + "CHD" + "')");
                    if (drMeal.Length > 0)
                    {
                        foreach (DataRow dr in drMeal.CopyToDataTable().Rows)
                        {
                            iTotalSSRFare += int.Parse(dr["MealChg_I"].ToString().Trim().ToString());
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                dbCommon.Logger.dbLogg(CompanyID, BookingRef, "getSSRmeal", "PNR", SearchCriteria, SearchID, ex.Message);
            }
            return iTotalSSRFare;
        }
        public Int32 getSSRbaggage(DataTable dtPassenger, string Trip)
        {
            Int32 iTotalSSRFare = 0;
            try
            {
                if (Trip.Equals("O"))
                {
                    DataRow[] drBaggage = dtPassenger.Select("BaggageChg_O > 0 AND (PaxType='" + "ADT" + "' OR PaxType='" + "CHD" + "')");
                    if (drBaggage.Length > 0)
                    {
                        foreach (DataRow dr in drBaggage.CopyToDataTable().Rows)
                        {
                            iTotalSSRFare += int.Parse(dr["BaggageChg_O"].ToString().Trim().ToString());
                        }
                    }
                }
                else
                {
                    DataRow[] drBaggage = dtPassenger.Select("BaggageChg_I > 0 AND (PaxType='" + "ADT" + "' OR PaxType='" + "CHD" + "')");
                    if (drBaggage.Length > 0)
                    {
                        foreach (DataRow dr in drBaggage.CopyToDataTable().Rows)
                        {
                            iTotalSSRFare += int.Parse(dr["BaggageChg_I"].ToString().Trim().ToString());
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                dbCommon.Logger.dbLogg(CompanyID, BookingRef, "getSSRbaggage", "PNR", SearchCriteria, SearchID, ex.Message);
            }
            return iTotalSSRFare;
        }
        public Int32 getTotalfare(DataRow dr)
        {
            Int32 iTotalFare = 0;
            try
            {
                iTotalFare = Convert.ToInt32(dr["Adt"].ToString()) * Convert.ToInt32(dr["AdtTotalFare"].ToString()) + Convert.ToInt32(dr["Chd"].ToString()) * Convert.ToInt32(dr["ChdTotalFare"].ToString()) + Convert.ToInt32(dr["Inf"].ToString()) * Convert.ToInt32(dr["InfTotalFare"].ToString());
            }
            catch (Exception ex)
            {
                dbCommon.Logger.dbLogg(CompanyID, BookingRef, "getTotalfare", "PNR", SearchCriteria, SearchID, ex.Message);
            }
            return iTotalFare;
        }
        public Int32 getTotalfare(DataRow dr1, DataRow dr2)
        {
            Int32 iTotalFare = 0;
            try
            {
                iTotalFare = Convert.ToInt32(dr1["Adt"].ToString()) * Convert.ToInt32(dr1["AdtTotalFare"].ToString()) + Convert.ToInt32(dr1["Chd"].ToString()) * Convert.ToInt32(dr1["ChdTotalFare"].ToString()) + Convert.ToInt32(dr1["Inf"].ToString()) * Convert.ToInt32(dr1["InfTotalFare"].ToString());
                iTotalFare += Convert.ToInt32(dr2["Adt"].ToString()) * Convert.ToInt32(dr2["AdtTotalFare"].ToString()) + Convert.ToInt32(dr2["Chd"].ToString()) * Convert.ToInt32(dr2["ChdTotalFare"].ToString()) + Convert.ToInt32(dr2["Inf"].ToString()) * Convert.ToInt32(dr2["InfTotalFare"].ToString());
            }
            catch (Exception ex)
            {
                dbCommon.Logger.dbLogg(CompanyID, BookingRef, "getTotalfare", "PNR", SearchCriteria, SearchID, ex.Message);
            }
            return iTotalFare;
        }


        public static Int32 TotalMeal(string Trip, DataTable dtPassenger)
        {
            Int32 iTotalSSRFare = 0;
            try
            {
                if (Trip.Equals("O"))
                {
                    DataRow[] drMeal = dtPassenger.Select("MealChg_O > 0 AND (PaxType='" + "ADT" + "' OR PaxType='" + "CHD" + "')");
                    if (drMeal.Length > 0)
                    {
                        foreach (DataRow dr in drMeal.CopyToDataTable().Rows)
                        {
                            iTotalSSRFare += int.Parse(dr["MealChg_O"].ToString().Trim().ToString());
                        }
                    }
                }
                else
                {
                    DataRow[] drMeal = dtPassenger.Select("MealChg_I > 0 AND (PaxType='" + "ADT" + "' OR PaxType='" + "CHD" + "')");
                    if (drMeal.Length > 0)
                    {
                        foreach (DataRow dr in drMeal.CopyToDataTable().Rows)
                        {
                            iTotalSSRFare += int.Parse(dr["MealChg_I"].ToString().Trim().ToString());
                        }
                    }
                }
            }
            catch
            {

            }

            return iTotalSSRFare;
        }
        public static Int32 TotalBaggage(string Trip, DataTable dtPassenger)
        {
            Int32 iTotalSSRFare = 0;
            try
            {
                if (Trip.Equals("O"))
                {
                    DataRow[] drBaggage = dtPassenger.Select("BagChg_O > 0 AND (PaxType='" + "ADT" + "' OR PaxType='" + "CHD" + "')");
                    if (drBaggage.Length > 0)
                    {
                        foreach (DataRow dr in drBaggage.CopyToDataTable().Rows)
                        {
                            iTotalSSRFare += int.Parse(dr["BagChg_O"].ToString().Trim().ToString());
                        }
                    }
                }
                else
                {
                    DataRow[] drBaggage = dtPassenger.Select("BagChg_I > 0 AND (PaxType='" + "ADT" + "' OR PaxType='" + "CHD" + "')");
                    if (drBaggage.Length > 0)
                    {
                        foreach (DataRow dr in drBaggage.CopyToDataTable().Rows)
                        {
                            iTotalSSRFare += int.Parse(dr["BagChg_I"].ToString().Trim().ToString());
                        }
                    }
                }
            }
            catch
            {

            }

            return iTotalSSRFare;
        }
        public static Int32 TotalFare(DataRow dr)
        {
            Int32 iTotalFare = 0;
            try
            {
                iTotalFare = Convert.ToInt32(dr["AdtTotalFare"].ToString()) + Convert.ToInt32(dr["ChdTotalFare"].ToString()) + Convert.ToInt32(dr["InfTotalFare"].ToString());
            }
            catch
            {

            }
            return iTotalFare;
        }
        public static Int32 TotalFareCombine(DataRow dr1, DataRow dr2)
        {
            Int32 iTotalFare = 0;
            try
            {
                iTotalFare = Convert.ToInt32(dr1["AdtTotalFare"].ToString()) + Convert.ToInt32(dr1["ChdTotalFare"].ToString()) + Convert.ToInt32(dr1["InfTotalFare"].ToString());
                iTotalFare += Convert.ToInt32(dr2["AdtTotalFare"].ToString()) + Convert.ToInt32(dr2["ChdTotalFare"].ToString()) + Convert.ToInt32(dr2["InfTotalFare"].ToString());
            }
            catch
            {

            }

            return iTotalFare;
        }
    }
}
