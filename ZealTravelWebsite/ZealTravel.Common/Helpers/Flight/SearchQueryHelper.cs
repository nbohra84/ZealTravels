using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZealTravel.Common.Helpers.Flight
{
    public class SearchQueryHelper
    {
        public static string GetSearchQuery(string Availability)
        {
            string SearchQuery = string.Empty;

            try
            {
                DataSet dsResponse = new DataSet();
                dsResponse.ReadXml(new System.IO.StringReader(Availability));

                SearchQuery += dsResponse.Tables[0].Rows[0]["AirlineID"].ToString().Trim() + "-";
                SearchQuery += dsResponse.Tables[0].Rows[0]["Origin"].ToString().Trim() + "-";
                SearchQuery += dsResponse.Tables[0].Rows[0]["Destination"].ToString().Trim() + "-";
                SearchQuery += dsResponse.Tables[0].Rows[0]["Adt"].ToString().Trim() + "-";
                SearchQuery += dsResponse.Tables[0].Rows[0]["Chd"].ToString().Trim() + "-";
                SearchQuery += dsResponse.Tables[0].Rows[0]["Inf"].ToString().Trim() + "-";

                if (dsResponse.Tables[0].Rows[0]["DepDate"].ToString().Trim().Length > 8)
                {
                    SearchQuery += Convert.ToDateTime(dsResponse.Tables[0].Rows[0]["DepDate"].ToString().Trim()).ToString("yyyyMMdd") + "-";
                }
                else
                {
                    SearchQuery += dsResponse.Tables[0].Rows[0]["DepDate"].ToString().Trim() + "-";
                }

                DataRow[] drSelect = dsResponse.Tables[0].Select("FltType='" + "I" + "'");
                if (drSelect.Length > 0)
                {
                    //if (drSelect.CopyToDataTable().Rows[0]["DepDate"].ToString().Trim().Length > 8)
                    //{
                    //    SearchQuery += Convert.ToDateTime(drSelect.CopyToDataTable().Rows[0]["DepDate"].ToString().Trim()).ToString("yyyyMMdd") + "-";
                    //}
                    //else
                    //{
                    //    SearchQuery += drSelect.CopyToDataTable().Rows[0]["DepDate"].ToString().Trim() + "-";
                    //}
                    if (drSelect[0]["DepDate"].ToString().Trim().Length > 8)
                    {
                        SearchQuery += Convert.ToDateTime(drSelect[0]["DepDate"].ToString().Trim()).ToString("yyyyMMdd") + "-";
                    }
                    else
                    {
                        SearchQuery += drSelect[0]["DepDate"].ToString().Trim() + "-";
                    }
                }
            }
            catch (Exception ex)
            {

            }

            return SearchQuery;
        }
    }
}
