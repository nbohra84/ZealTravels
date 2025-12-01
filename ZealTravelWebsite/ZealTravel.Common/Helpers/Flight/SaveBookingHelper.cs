using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZealTravel.Common.Helpers.Flight
{
    public class SaveBookingHelper
    {
        public static Decimal GETTransactionAmount(string SearchID, string CompanyID, Int32 BookingRef, DataSet dsAvailability, DataSet dsPassenger, bool IsCombi, bool IsRTfare, string UserType)
        {
            Decimal Transaction_Amount = 0;
            Decimal dTotalSSR = SSRTotalHelper.GETTotalSSR(SearchID, CompanyID, BookingRef, dsPassenger.Tables[0]);
            if (dsAvailability.Tables["AvailabilityInfo"] != null)
            {
                if (IsCombi.Equals(true))
                {
                    DataRow[] dr1 = dsAvailability.Tables["AvailabilityInfo"].Select("FltType='" + "O" + "'");
                    if (dr1.Length > 0)
                    {
                        int AG_Markup = 0;
                        Transaction_Amount = Convert.ToDecimal(dr1.CopyToDataTable().Rows[0]["TotalFare"].ToString());
                        Transaction_Amount -= Convert.ToDecimal(dr1.CopyToDataTable().Rows[0]["TotalMarkup"].ToString());
                        if (dr1.CopyToDataTable().Columns.Contains("AG_Markup") && Convert.ToInt32(dr1.CopyToDataTable().Rows[0]["AG_Markup"].ToString()) > 0)
                        {
                            int pax = Convert.ToInt32(dr1.CopyToDataTable().Rows[0]["Adt"].ToString());
                            pax += Convert.ToInt32(dr1.CopyToDataTable().Rows[0]["Chd"].ToString());

                            AG_Markup = pax * Convert.ToInt32(dr1.CopyToDataTable().Rows[0]["AG_Markup"].ToString());
                            Transaction_Amount -= AG_Markup;
                        }

                        if (UserType.Equals("B2B") || UserType.Equals("B2B2ST"))
                        {
                            Transaction_Amount -= Convert.ToDecimal(dr1.CopyToDataTable().Rows[0]["TotalCommission"].ToString());
                        }
                        else if (UserType.Equals("B2B2B") || UserType.Equals("B2B2B2ST"))
                        {
                            Transaction_Amount -= Convert.ToDecimal(dr1.CopyToDataTable().Rows[0]["TotalCommission_SA"].ToString());
                            Transaction_Amount -= Convert.ToDecimal(dr1.CopyToDataTable().Rows[0]["TotalTds"].ToString());
                            Transaction_Amount += Convert.ToDecimal(dr1.CopyToDataTable().Rows[0]["TotalTds_SA"].ToString());
                            Transaction_Amount += AG_Markup;
                        }
                        else if (UserType.Equals("B2B2C") || UserType.Equals("B2C"))
                        {
                            Transaction_Amount += Convert.ToDecimal(dr1.CopyToDataTable().Rows[0]["TotalMarkup"].ToString());
                            Transaction_Amount -= Convert.ToDecimal(dr1.CopyToDataTable().Rows[0]["TotalCommission_SA"].ToString());
                            Transaction_Amount -= Convert.ToDecimal(dr1.CopyToDataTable().Rows[0]["TotalTds"].ToString());
                            Transaction_Amount += Convert.ToDecimal(dr1.CopyToDataTable().Rows[0]["TotalTds_SA"].ToString());
                        }

                        Transaction_Amount += dTotalSSR;
                    }
                }
                else
                {
                    DataRow[] dr1 = dsAvailability.Tables["AvailabilityInfo"].Select("FltType='" + "O" + "'");
                    DataRow[] dr2 = dsAvailability.Tables["AvailabilityInfo"].Select("FltType='" + "I" + "'");
                    if (dr1.Length > 0)
                    {
                        int AG_Markup = 0;
                        Transaction_Amount = Convert.ToDecimal(dr1.CopyToDataTable().Rows[0]["TotalFare"].ToString());
                        Transaction_Amount -= Convert.ToDecimal(dr1.CopyToDataTable().Rows[0]["TotalMarkup"].ToString());
                        if (dr1.CopyToDataTable().Columns.Contains("AG_Markup") && Convert.ToInt32(dr1.CopyToDataTable().Rows[0]["AG_Markup"].ToString()) > 0)
                        {
                            int pax = Convert.ToInt32(dr1.CopyToDataTable().Rows[0]["Adt"].ToString());
                            pax += Convert.ToInt32(dr1.CopyToDataTable().Rows[0]["Chd"].ToString());

                            AG_Markup = pax * Convert.ToInt32(dr1.CopyToDataTable().Rows[0]["AG_Markup"].ToString());
                            Transaction_Amount -= AG_Markup;
                        }


                        if (UserType.Equals("B2B") || UserType.Equals("B2B2ST"))
                        {
                            Transaction_Amount -= Convert.ToDecimal(dr1.CopyToDataTable().Rows[0]["TotalCommission"].ToString());
                        }
                        else if (UserType.Equals("B2B2B") || UserType.Equals("B2B2B2ST"))
                        {
                            Transaction_Amount -= Convert.ToDecimal(dr1.CopyToDataTable().Rows[0]["TotalCommission_SA"].ToString());
                            Transaction_Amount -= Convert.ToDecimal(dr1.CopyToDataTable().Rows[0]["TotalTds"].ToString());
                            Transaction_Amount += Convert.ToDecimal(dr1.CopyToDataTable().Rows[0]["TotalTds_SA"].ToString());
                            Transaction_Amount += AG_Markup;
                        }
                        else if (UserType.Equals("B2B2C") || UserType.Equals("B2C"))
                        {
                            Transaction_Amount += Convert.ToDecimal(dr1.CopyToDataTable().Rows[0]["TotalMarkup"].ToString());
                            Transaction_Amount -= Convert.ToDecimal(dr1.CopyToDataTable().Rows[0]["TotalCommission_SA"].ToString());
                            Transaction_Amount -= Convert.ToDecimal(dr1.CopyToDataTable().Rows[0]["TotalTds"].ToString());
                            Transaction_Amount += Convert.ToDecimal(dr1.CopyToDataTable().Rows[0]["TotalTds_SA"].ToString());
                        }

                        Transaction_Amount += dTotalSSR;
                    }
                    if (IsRTfare.Equals(true))
                    {
                        //if (dr2.Length > 0)
                        //{
                        //    Transaction_Amount -= Convert.ToDecimal(dr2.CopyToDataTable().Rows[0]["TotalMarkup"].ToString());
                        //}
                    }
                    else
                    {
                        if (dr2.Length > 0)
                        {
                            int AG_Markup = 0;
                            Transaction_Amount += Convert.ToDecimal(dr2.CopyToDataTable().Rows[0]["TotalFare"].ToString());
                            Transaction_Amount -= Convert.ToDecimal(dr2.CopyToDataTable().Rows[0]["TotalMarkup"].ToString());
                            if (dr2.CopyToDataTable().Columns.Contains("AG_Markup") && Convert.ToInt32(dr2.CopyToDataTable().Rows[0]["AG_Markup"].ToString()) > 0)
                            {
                                int pax = Convert.ToInt32(dr2.CopyToDataTable().Rows[0]["Adt"].ToString());
                                pax += Convert.ToInt32(dr2.CopyToDataTable().Rows[0]["Chd"].ToString());

                                AG_Markup = pax * Convert.ToInt32(dr2.CopyToDataTable().Rows[0]["AG_Markup"].ToString());
                                Transaction_Amount -= AG_Markup;
                            }

                            if (UserType.Equals("B2B") || UserType.Equals("B2B2ST"))
                            {
                                Transaction_Amount -= Convert.ToDecimal(dr2.CopyToDataTable().Rows[0]["TotalCommission"].ToString());
                            }
                            else if (UserType.Equals("B2B2B") || UserType.Equals("B2B2B2ST"))
                            {
                                Transaction_Amount -= Convert.ToDecimal(dr2.CopyToDataTable().Rows[0]["TotalCommission_SA"].ToString());
                                Transaction_Amount -= Convert.ToDecimal(dr2.CopyToDataTable().Rows[0]["TotalTds"].ToString());
                                Transaction_Amount += Convert.ToDecimal(dr2.CopyToDataTable().Rows[0]["TotalTds_SA"].ToString());
                                Transaction_Amount += AG_Markup;
                            }
                            else if (UserType.Equals("B2B2C") || UserType.Equals("B2C"))
                            {
                                Transaction_Amount += Convert.ToDecimal(dr2.CopyToDataTable().Rows[0]["TotalMarkup"].ToString());
                                Transaction_Amount -= Convert.ToDecimal(dr2.CopyToDataTable().Rows[0]["TotalCommission_SA"].ToString());
                                Transaction_Amount -= Convert.ToDecimal(dr2.CopyToDataTable().Rows[0]["TotalTds"].ToString());
                                Transaction_Amount += Convert.ToDecimal(dr2.CopyToDataTable().Rows[0]["TotalTds_SA"].ToString());
                            }
                        }
                    }
                }
            }
            return Transaction_Amount;
        }

        public static Decimal GetTransactionAmount(string SearchID, string CompanyID, Int32 BookingRef, string AvailabilityResponse, string PassengerResponse, bool IsCombi, bool IsRTfare, string UserType, string RefID_O, string RefID_I)
        {
            Decimal Transaction_Amount = 0;
            try
            {
                string SelectedAvailabilityResponse = SelectedResponseHelper.GETSelectedResponse(RefID_O, RefID_I, AvailabilityResponse, IsCombi);

                DataSet dsAvailability = CommonFunction.StringToDataSet(SelectedAvailabilityResponse);
                DataSet dsPassenger = CommonFunction.StringToDataSet(PassengerResponse);
                Transaction_Amount = GETTransactionAmount(SearchID, CompanyID, BookingRef, dsAvailability, dsPassenger, IsCombi, IsRTfare, UserType);
            }
            catch (Exception ex)
            {
               // dbCommon.Logger.dbLogg(CompanyID, BookingRef, "GET_Transaction_Amount", "LibraryAirlineBooking", "clsDB", SearchID, ex.Message);
            }

            return Transaction_Amount;
        }
        public static Hashtable GetSAdeal(string SAdeal)
        {
            //6,127,0,0,0*0,0,0,0,0
            Hashtable HTdeal = new Hashtable();
            if (SAdeal.Length > 0 && SAdeal.IndexOf("*") != -1)
            {
                string[] split = SAdeal.Split('*');
                if (split.Length.Equals(2))
                {
                    string[] split1 = split[0].ToString().Split(',');
                    if (split1.Length.Equals(5))
                    {
                        HTdeal.Add("ADT_TDS", split1[0].ToString());
                        HTdeal.Add("ADT_BAS", split1[1].ToString());
                        HTdeal.Add("ADT_Y", split1[2].ToString());
                        HTdeal.Add("ADT_CB", split1[3].ToString());
                        HTdeal.Add("ADT_PR", split1[4].ToString());
                    }

                    string[] split2 = split[1].ToString().Split(',');
                    if (split2.Length.Equals(5))
                    {
                        HTdeal.Add("CHD_TDS", split2[0].ToString());
                        HTdeal.Add("CHD_BAS", split2[1].ToString());
                        HTdeal.Add("CHD_Y", split2[2].ToString());
                        HTdeal.Add("CHD_CB", split2[3].ToString());
                        HTdeal.Add("CHD_PR", split2[4].ToString());
                    }
                }
            }
            return HTdeal;
        }
    }
}
