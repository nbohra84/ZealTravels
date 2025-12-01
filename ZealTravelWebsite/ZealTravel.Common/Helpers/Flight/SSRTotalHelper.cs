using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZealTravel.Common.Models.Flight;

namespace ZealTravel.Common.Helpers.Flight
{
    public class SSRTotalHelper
    {
        public static Decimal GETTotalSSR(string SearchID, string CompanyID, int BookingRef, DataTable dtPassenger)
        {
            Decimal dSSR = 0;

            try
            {
                foreach (DataRow dr in dtPassenger.Rows)
                {
                    if (dr["MealChg_O"].ToString().Trim().Equals(string.Empty))
                    {
                        dr["MealChg_O"] = "0";
                    }
                    if (dr["MealChg_I"].ToString().Trim().Equals(string.Empty))
                    {
                        dr["MealChg_I"] = "0";
                    }

                    if (dr["BaggageChg_O"].ToString().Trim().Equals(string.Empty))
                    {
                        dr["BaggageChg_O"] = "0";
                    }
                    if (dr["BaggageChg_I"].ToString().Trim().Equals(string.Empty))
                    {
                        dr["BaggageChg_I"] = "0";
                    }
                }


                //---------------------------------ADULT------------------------------------------------------
                DataRow[] dtSelectPax = dtPassenger.Select("TRIM(MealChg_O) > 0 AND TRIM(PaxType)='" + "ADT" + "'");
                if (dtSelectPax.Length > 0)
                {
                    foreach (DataRow dr in dtSelectPax.CopyToDataTable().Rows)
                    {
                        dSSR += Convert.ToDecimal(dr["MealChg_O"].ToString().Trim());
                    }
                }

                dtSelectPax = dtPassenger.Select("TRIM(BaggageChg_O) > 0 AND TRIM(PaxType)='" + "ADT" + "'");
                if (dtSelectPax.Length > 0)
                {
                    foreach (DataRow dr in dtSelectPax.CopyToDataTable().Rows)
                    {
                        dSSR += Convert.ToDecimal(dr["BaggageChg_O"].ToString().Trim());
                    }
                }
                //--------------------------------------------------------------------------------------------
                dtSelectPax = dtPassenger.Select("TRIM(MealChg_I) > 0 AND TRIM(PaxType)='" + "ADT" + "'");
                if (dtSelectPax.Length > 0)
                {
                    foreach (DataRow dr in dtSelectPax.CopyToDataTable().Rows)
                    {
                        dSSR += Convert.ToDecimal(dr["MealChg_I"].ToString().Trim());
                    }
                }

                dtSelectPax = dtPassenger.Select("TRIM(BaggageChg_I) > 0 AND TRIM(PaxType)='" + "ADT" + "'");
                if (dtSelectPax.Length > 0)
                {
                    foreach (DataRow dr in dtSelectPax.CopyToDataTable().Rows)
                    {
                        dSSR += Convert.ToDecimal(dr["BaggageChg_I"].ToString().Trim());
                    }
                }

                //--------------------------CHILD-----------------------------------------------------
                dtSelectPax = dtPassenger.Select("TRIM(MealChg_O) > 0 AND TRIM(PaxType)='" + "CHD" + "'");
                if (dtSelectPax.Length > 0)
                {
                    foreach (DataRow dr in dtSelectPax.CopyToDataTable().Rows)
                    {
                        dSSR += Convert.ToDecimal(dr["MealChg_O"].ToString().Trim());
                    }
                }


                dtSelectPax = dtPassenger.Select("TRIM(BaggageChg_O) > 0 AND TRIM(PaxType)='" + "CHD" + "'");
                if (dtSelectPax.Length > 0)
                {
                    foreach (DataRow dr in dtSelectPax.CopyToDataTable().Rows)
                    {
                        dSSR += Convert.ToDecimal(dr["BaggageChg_O"].ToString().Trim());
                    }
                }
                //--------------------------------------------------------------------------------------------
                dtSelectPax = dtPassenger.Select("TRIM(MealChg_I) > 0 AND TRIM(PaxType)='" + "CHD" + "'");
                if (dtSelectPax.Length > 0)
                {
                    foreach (DataRow dr in dtSelectPax.CopyToDataTable().Rows)
                    {
                        dSSR += Convert.ToDecimal(dr["MealChg_I"].ToString().Trim());
                    }
                }



                dtSelectPax = dtPassenger.Select("TRIM(BaggageChg_I) > 0 AND TRIM(PaxType)='" + "CHD" + "'");
                if (dtSelectPax.Length > 0)
                {
                    foreach (DataRow dr in dtSelectPax.CopyToDataTable().Rows)
                    {
                        dSSR += Convert.ToDecimal(dr["BaggageChg_I"].ToString().Trim());
                    }
                }
            }
            catch (Exception ex)
            {
                // dbCommon.Logger.dbLogg(CompanyID, BookingRef, "GET_TotalSSR", "LibraryAirlineBooking", "SSRdetail", SearchID, ex.Message);
            }

            return dSSR;
        }
        public static CalculateSSRDetail GETCalculateSSR(string SearchID, string CompanyID, int BookingRef, DataTable dtPassenger)
        {
            Decimal dTotalMealAdt_O = 0;
            Decimal dTotalBaggageAdt_O = 0;

            Decimal dTotalMealChd_O = 0;
            Decimal dTotalBaggageChd_O = 0;

            Decimal dTotalMealAdt_I = 0;
            Decimal dTotalBaggageAdt_I = 0;

            Decimal dTotalMealChd_I = 0;
            Decimal dTotalBaggageChd_I = 0;

            try
            {
                
                foreach (DataRow dr in dtPassenger.Rows)
                {
                    if (dr["MealChg_O"].ToString().Trim().Equals(string.Empty))
                    {
                        dr["MealChg_O"] = "0";
                    }
                    if (dr["MealChg_I"].ToString().Trim().Equals(string.Empty))
                    {
                        dr["MealChg_I"] = "0";
                    }
                    //if (dr["SeatChg_O"].ToString().Trim().Equals(string.Empty))
                    //{
                    //    dr["SeatChg_O"] = "0";
                    //}
                    //if (dr["SeatChg_I"].ToString().Trim().Equals(string.Empty))
                    //{
                    //    dr["SeatChg_I"] = "0";
                    //}
                    if (dr["BaggageChg_O"].ToString().Trim().Equals(string.Empty))
                    {
                        dr["BaggageChg_O"] = "0";
                    }
                    if (dr["BaggageChg_I"].ToString().Trim().Equals(string.Empty))
                    {
                        dr["BaggageChg_I"] = "0";
                    }
                }

                //---------------------------------ADULT------------------------------------------------------
                DataRow[] dtSelectPax = dtPassenger.Select("TRIM(MealChg_O) > 0 AND TRIM(PaxType)='" + "ADT" + "'");
                if (dtSelectPax.Length > 0)
                {
                    foreach (DataRow dr in dtSelectPax.CopyToDataTable().Rows)
                    {
                        dTotalMealAdt_O += Convert.ToDecimal(dr["MealChg_O"].ToString().Trim());
                    }
                }

                dtSelectPax = dtPassenger.Select("TRIM(BaggageChg_O) > 0 AND TRIM(PaxType)='" + "ADT" + "'");
                if (dtSelectPax.Length > 0)
                {
                    foreach (DataRow dr in dtSelectPax.CopyToDataTable().Rows)
                    {
                        dTotalBaggageAdt_O += Convert.ToDecimal(dr["BaggageChg_O"].ToString().Trim());
                    }
                }
                //--------------------------------------------------------------------------------------------
                dtSelectPax = dtPassenger.Select("TRIM(MealChg_I) > 0 AND TRIM(PaxType)='" + "ADT" + "'");
                if (dtSelectPax.Length > 0)
                {
                    foreach (DataRow dr in dtSelectPax.CopyToDataTable().Rows)
                    {
                        dTotalMealAdt_I += Convert.ToDecimal(dr["MealChg_I"].ToString().Trim());
                    }
                }

                dtSelectPax = dtPassenger.Select("TRIM(BaggageChg_I) > 0 AND TRIM(PaxType)='" + "ADT" + "'");
                if (dtSelectPax.Length > 0)
                {
                    foreach (DataRow dr in dtSelectPax.CopyToDataTable().Rows)
                    {
                        dTotalBaggageAdt_I += Convert.ToDecimal(dr["BaggageChg_I"].ToString().Trim());
                    }
                }

                //--------------------------CHILD-----------------------------------------------------
                dtSelectPax = dtPassenger.Select("TRIM(MealChg_O) > 0 AND TRIM(PaxType)='" + "CHD" + "'");
                if (dtSelectPax.Length > 0)
                {
                    foreach (DataRow dr in dtSelectPax.CopyToDataTable().Rows)
                    {
                        dTotalMealChd_O += Convert.ToDecimal(dr["MealChg_O"].ToString().Trim());
                    }
                }

                dtSelectPax = dtPassenger.Select("TRIM(BaggageChg_O) > 0 AND TRIM(PaxType)='" + "CHD" + "'");
                if (dtSelectPax.Length > 0)
                {
                    foreach (DataRow dr in dtSelectPax.CopyToDataTable().Rows)
                    {
                        dTotalBaggageChd_O += Convert.ToDecimal(dr["BaggageChg_O"].ToString().Trim());
                    }
                }
                //--------------------------------------------------------------------------------------------
                dtSelectPax = dtPassenger.Select("TRIM(MealChg_I) > 0 AND TRIM(PaxType)='" + "CHD" + "'");
                if (dtSelectPax.Length > 0)
                {
                    foreach (DataRow dr in dtSelectPax.CopyToDataTable().Rows)
                    {
                        dTotalMealChd_I += Convert.ToDecimal(dr["MealChg_I"].ToString().Trim());
                    }
                }

                dtSelectPax = dtPassenger.Select("TRIM(BaggageChg_I) > 0 AND TRIM(PaxType)='" + "CHD" + "'");
                if (dtSelectPax.Length > 0)
                {
                    foreach (DataRow dr in dtSelectPax.CopyToDataTable().Rows)
                    {
                        dTotalBaggageChd_I += Convert.ToDecimal(dr["BaggageChg_I"].ToString().Trim());
                    }
                }
            }
            catch (Exception ex)
            {
               // dbCommon.Logger.dbLogg(CompanyID, BookingRef, "GET_CalculateSSR", "LibraryAirlineBooking", "db_Company_Fare_Detail_Airline", SearchID, ex.Message);
            }
            var ssrDetails = new CalculateSSRDetail();
            ssrDetails.dTotalMealAdt_O = dTotalMealAdt_O;
            ssrDetails.dTotalBaggageAdt_O = dTotalBaggageAdt_O;
            ssrDetails.dTotalMealAdt_I  = dTotalMealAdt_I;
            ssrDetails.dTotalBaggageAdt_I = dTotalBaggageAdt_I;
            ssrDetails.dTotalMealChd_O = dTotalMealChd_O;
            ssrDetails.dTotalBaggageChd_O = dTotalBaggageChd_O;
            ssrDetails.dTotalMealChd_I = dTotalMealChd_I;
            ssrDetails.dTotalBaggageChd_I = dTotalBaggageChd_I;

            ssrDetails.dTotalMealAdt = dTotalMealAdt_O + dTotalMealAdt_I;
            ssrDetails.dTotalMealChd = dTotalMealChd_O + dTotalMealChd_I;

            ssrDetails.dTotalBaggageAdt = dTotalBaggageAdt_O + dTotalBaggageAdt_I;
            ssrDetails.dTotalBaggageChd = dTotalBaggageChd_O + dTotalBaggageChd_I;

            ssrDetails.dTotalMeal = ssrDetails.dTotalMealAdt + ssrDetails.dTotalMealChd;
            ssrDetails.dTotalBaggage = ssrDetails.dTotalBaggageAdt + ssrDetails.dTotalBaggageChd;
            ssrDetails.dTotalSSR = ssrDetails.dTotalMeal + ssrDetails.dTotalBaggage;

            return ssrDetails;
        }
    }
}
