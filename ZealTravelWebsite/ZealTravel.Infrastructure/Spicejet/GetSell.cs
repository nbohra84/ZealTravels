using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZealTravel.Infrastructure.Spicejet
{
    class GetSell
    {
        public Decimal GetSellOneWay(string Searchid, string Companyid, string RS_Availability)
        {

            DataTable dtBound = GetCommonFunctions.StringToDataSet(RS_Availability).Tables["AvailabilityInfo"];
            if (dtBound != null && dtBound.Rows.Count > 0)
            {
                GetApiSell objSell = new GetApiSell(Searchid);
                Decimal GetApiTotalFare = objSell.GetSellOneWay(dtBound);

                if (Convert.ToInt32(dtBound.Rows[0]["Inf"].ToString()) > 0 && GetApiTotalFare > 0)
                {
                    GetApiTotalFare = objSell.GetSellOneWayInfant(dtBound);
                }

                return GetApiTotalFare;
            }

            return 0;
        }
        public Decimal GetSellRoundWay(string Searchid, string Companyid, string RS_Availability)
        {

            DataTable dtBound = GetCommonFunctions.StringToDataSet(RS_Availability).Tables["AvailabilityInfo"];
            if (dtBound != null && dtBound.Rows.Count > 0)
            {
                DataRow[] drOutbound = dtBound.Select("FltType='" + "O" + "'");
                DataRow[] drInbound = dtBound.Select("FltType='" + "I" + "'");

                GetApiSell objSell = new GetApiSell(Searchid);
                Decimal GetApiTotalFare = objSell.GetSellRoundWay(drOutbound.CopyToDataTable(), drInbound.CopyToDataTable());

                if (Convert.ToInt32(dtBound.Rows[0]["Inf"].ToString()) > 0 && GetApiTotalFare > 0)
                {
                    GetApiTotalFare = objSell.GetSellRoundWayInfant(drOutbound.CopyToDataTable(), drInbound.CopyToDataTable());
                }
                return GetApiTotalFare;
            }

            return 0;
        }
    }
}
