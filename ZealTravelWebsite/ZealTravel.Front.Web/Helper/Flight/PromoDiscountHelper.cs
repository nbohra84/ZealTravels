using System.Data;
using ZealTravel.Application.Handlers;
using ZealTravel.Common;
using ZealTravel.Common.Helpers;
using ZealTravel.Domain.Interfaces.Handlers;

namespace ZealTravel.Front.Web.Helper.Flight
{
    public class PromoDiscountHelper
    {
        public static void AddPromo(IHandlesQueryAsync<string, string> gettWhitelabelAdminidFromHostQueryHandler, IHandlesQueryAsync<string, int> getPromoAmountQueryHandler)
        {
            if (HttpContextHelper.Current.Request.Query.ContainsKey("prom"))
            {
                int Amount =  getPromoAmountQueryHandler.HandleAsync(HttpContextHelper.Current.Request.Query["prom"].ToString().Trim()).Result;
                if (Amount > 0)
                {
                    string strcmpid = UserHelper.GetCompanyID(HttpContextHelper.Current.User);
                    if (string.IsNullOrEmpty(strcmpid))
                    {
                        strcmpid = gettWhitelabelAdminidFromHostQueryHandler.HandleAsync(HttpContextHelper.Current.Request.Host.Host).Result;
                        
                    }
                    if (strcmpid.StartsWith("AD-", StringComparison.OrdinalIgnoreCase) || strcmpid.StartsWith("C-", StringComparison.OrdinalIgnoreCase))
                    {
                        string SelectedFltOut = HttpContextHelper.Current?.Session.GetString("SelectedFltOut");
                        DataSet dsAvailability = new DataSet();
                        dsAvailability.ReadXml(new System.IO.StringReader(HttpContextHelper.Current?.Session.GetString("FinalResult")));

                        DataRow[] rows = dsAvailability.Tables["AvailabilityInfo"].Select("RefID='" + SelectedFltOut + "'");
                        if (rows.Length > 0)
                        {
                            int k = 0;
                            foreach (DataRow dr in rows)
                            {
                                if (k == 0)
                                {
                                    dr["Adt_PR"] = Amount;
                                    k++;
                                }
                                dr["TotalCommission_SA"] = Amount;

                                dr.AcceptChanges();
                                dsAvailability.Tables[0].AcceptChanges();
                                dsAvailability.AcceptChanges();
                            }
                            HttpContextHelper.Current?.Session.SetString("FinalResult", CommonFunction.DataSetToString(dsAvailability));
                        }
                    }
                }
            }
        }
    }
}
