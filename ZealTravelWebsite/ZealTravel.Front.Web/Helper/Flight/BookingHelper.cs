using System.Data;
using ZealTravel.Application.BookingManagement.Handler;
using ZealTravel.Application.BookingManagement.Query;
using ZealTravel.Common;
using ZealTravel.Common.Helpers.Flight;
using ZealTravel.Domain.Data.Entities;
using ZealTravel.Domain.Interfaces.Handlers;

namespace ZealTravel.Front.Web.Helper.Flight
{
    public class BookingHelper
    {
        public static Decimal GETTransactionAmount(string SearchID, string CompanyID, Int32 BookingRef, string AvailabilityResponse, string PassengerResponse, bool IsCombi, bool IsRTfare, string RefID_O, string RefID_I)//flight display transactiom amount
        {
            AvailabilityResponse = SelectedResponseHelper.GETSelectedResponse(RefID_O, RefID_I, AvailabilityResponse, IsCombi);
            DataSet dsAvailability = CommonFunction.StringToDataSet(AvailabilityResponse);
            DataSet dsPassenger = CommonFunction.StringToDataSet(PassengerResponse);

            Decimal Transaction_Amount = 0;
            Decimal dTotalSSR = SSRTotalHelper.GETTotalSSR(SearchID, CompanyID, BookingRef, dsPassenger.Tables[0]);
            if (dsAvailability.Tables["AvailabilityInfo"] != null)
            {
                if (IsCombi.Equals(true))
                {
                    DataRow[] dr1 = dsAvailability.Tables["AvailabilityInfo"].Select("FltType='" + "O" + "'");
                    if (dr1.Length > 0)
                    {
                        Transaction_Amount = Convert.ToDecimal(dr1.CopyToDataTable().Rows[0]["TotalFare"].ToString());
                        Transaction_Amount -= Convert.ToDecimal(dr1.CopyToDataTable().Rows[0]["TotalMarkup"].ToString());

                        if (CompanyID.IndexOf("-SA-") != -1)
                        {
                            Transaction_Amount -= Convert.ToDecimal(dr1.CopyToDataTable().Rows[0]["TotalCommission_SA"].ToString());
                        }
                        else
                        {
                            Transaction_Amount -= Convert.ToDecimal(dr1.CopyToDataTable().Rows[0]["TotalCommission"].ToString());
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
                        Transaction_Amount = Convert.ToDecimal(dr1.CopyToDataTable().Rows[0]["TotalFare"].ToString());
                        Transaction_Amount -= Convert.ToDecimal(dr1.CopyToDataTable().Rows[0]["TotalMarkup"].ToString());

                        if (CompanyID.IndexOf("-SA-") != -1)
                        {
                            Transaction_Amount -= Convert.ToDecimal(dr1.CopyToDataTable().Rows[0]["TotalCommission_SA"].ToString());
                        }
                        else
                        {
                            Transaction_Amount -= Convert.ToDecimal(dr1.CopyToDataTable().Rows[0]["TotalCommission"].ToString());
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
                            Transaction_Amount += Convert.ToDecimal(dr2.CopyToDataTable().Rows[0]["TotalFare"].ToString());
                            Transaction_Amount -= Convert.ToDecimal(dr2.CopyToDataTable().Rows[0]["TotalMarkup"].ToString());

                            if (CompanyID.IndexOf("-SA-") != -1)
                            {
                                Transaction_Amount -= Convert.ToDecimal(dr2.CopyToDataTable().Rows[0]["TotalCommission_SA"].ToString());
                            }
                            else
                            {
                                Transaction_Amount -= Convert.ToDecimal(dr2.CopyToDataTable().Rows[0]["TotalCommission"].ToString());
                            }
                        }
                    }
                }
            }
            return Transaction_Amount;
        }
        public static async Task<Decimal> GetTransactionAmount(string SearchID, string CompanyID, string AdminID, Int32 BookingRef, string AvailabilityResponse, string PassengerResponse, bool IsCombi, bool IsRTfare, string UserType, string RefID_O, string RefID_I, IHandlesQueryAsync<GetCfeeQuery, DataTable> getCFeeQueryHandler)
        {
            Decimal Transaction_Amount = 0;
            try
            {
                
                string SelectedAvailabilityResponse = SelectedResponseHelper.GETSelectedResponse(RefID_O, RefID_I, AvailabilityResponse, IsCombi);

                int pax = 0;
                DataSet dsResponse = CommonFunction.StringToDataSet(SelectedAvailabilityResponse);
                if (dsResponse != null && dsResponse.Tables[0] != null && dsResponse.Tables[0].Rows.Count > 0)
                {
                    pax = int.Parse(dsResponse.Tables[0].Rows[0]["Adt"].ToString());
                    pax += int.Parse(dsResponse.Tables[0].Rows[0]["Chd"].ToString());
                }

                if (HttpContextHelper.Current?.Session.GetString("Curr") == null || HttpContextHelper.Current.Session.GetString("Curr").Equals("INR"))
                {
                   
                    Transaction_Amount = SaveBookingHelper.GetTransactionAmount(SearchID, CompanyID, BookingRef, SelectedAvailabilityResponse, PassengerResponse, IsCombi, IsRTfare, UserType, RefID_O, RefID_I);
                   

                    if (Transaction_Amount.Equals(0))
                    {
                        Transaction_Amount = SaveBookingHelper.GetTransactionAmount(SearchID, CompanyID, BookingRef, SelectedAvailabilityResponse, PassengerResponse, IsCombi, IsRTfare, UserType, RefID_O, RefID_I);
                       
                    }
                }
                else
                {
                    DataTable dtPassenger = CommonFunction.StringToDataSet(PassengerResponse).Tables[0];
                    var objsr=  SSRTotalHelper.GETCalculateSSR(SearchID, CompanyID, BookingRef, dtPassenger);

                    int TotalFare = 0;
                    if (HttpContextHelper.Current?.Session?.GetString("SearchValue")?.ToString().Equals("RW") == true)
                    {
                        DataRow[] drSelect = dsResponse.Tables[0].Select("FltType='" + "O" + "'");
                        TotalFare = ShowFlightDataHelper.ReturnFinalFare(drSelect.CopyToDataTable().Rows[0], CompanyID);

                        drSelect = dsResponse.Tables[0].Select("FltType='" + "I" + "'");
                        TotalFare += ShowFlightDataHelper.ReturnFinalFare(drSelect.CopyToDataTable().Rows[0], CompanyID);
                    }
                    else
                    {
                        TotalFare = ShowFlightDataHelper.ReturnFinalFare(dsResponse.Tables[0].Rows[0], CompanyID);
                    }

                    int TotalSSR = ShowFlightDataHelper.GetConvert(Decimal.ToInt32(objsr.dTotalSSR));
                    Transaction_Amount = TotalFare + TotalSSR;
                }

                if (CompanyID.Equals(string.Empty) || CompanyID.IndexOf("-C-") != -1 || CompanyID.IndexOf("-SA-") != -1)
                {
                    string Sector = "D";
                    if (HttpContextHelper.Current.Session.GetString("SEARCH_TYPE") == "INT")
                    {
                        Sector = "I";
                    }

                    
                    DataTable dtCfee = new DataTable();
                    if (CompanyID.Equals(string.Empty))
                    {
                        var query = new GetCfeeQuery()
                        {  CompanyID = AdminID,
                            BookingType = "AIRLINE", 
                            Sector = Sector,
                        };  
                        dtCfee = await getCFeeQueryHandler.HandleAsync(query);
                    }
                    else
                    {
                        var query = new GetCfeeQuery()
                        {
                            CompanyID = CommonFunction.getCompany_by_SubCompany_Customer(CompanyID),
                            BookingType = "AIRLINE",
                            Sector = Sector,
                        };

                        dtCfee = await getCFeeQueryHandler.HandleAsync(query);
                    }

                    if (dtCfee != null && dtCfee.Rows.Count > 0)
                    {
                        if (CompanyID.Equals(string.Empty) || CompanyID.IndexOf("-C-") != -1)
                        {
                            Transaction_Amount += pax * ShowFlightDataHelper.GetConvert(Convert.ToInt32(dtCfee.Rows[0]["Cfee_cu"].ToString()));
                        }
                        else if (CompanyID.IndexOf("-SA-") != -1)
                        {
                            Transaction_Amount += pax * ShowFlightDataHelper.GetConvert(Convert.ToInt32(dtCfee.Rows[0]["Cfee_sa"].ToString()));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
               // dbCommon.Logger.dbLogg(CompanyID, BookingRef, "GET_Transaction_Amount", "FO", "clsDB", SearchID, ex.Message);
            }
            return Transaction_Amount;
        }
    }
}
