using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZealTravel.Infrastructure.Spicejet
{
    class GetAddOn
    {
        public string GetSSROneWay(string Searchid, string Companyid, string AirRS)
        {
           
                DataTable dtBound = GetCommonFunctions.StringToDataSet(AirRS).Tables["AvailabilityInfo"];
                if (dtBound != null && dtBound.Rows.Count > 0)
                {
                    GetSsrAvailability objSsrAvailability = new GetSsrAvailability(Searchid);
                    DataTable dtAddOnFlights = objSsrAvailability.GetOneWaySSR(dtBound);

                    if (dtAddOnFlights != null && dtAddOnFlights.Rows.Count > 0)
                    {
                        dtAddOnFlights.TableName = "SSRInfo";
                        DataSet dsBound = new DataSet();
                        dsBound.Tables.Add(dtAddOnFlights.Copy());

                        dsBound.DataSetName = "root";
                        return GetCommonFunctions.DataSetToString(dsBound);
                    }
                }
            
            return string.Empty;
        }
        public string GetSSRRoundWay(string Searchid, string Companyid, string AirRS)
        {
           
                DataTable dtBound = GetCommonFunctions.StringToDataSet(AirRS).Tables["AvailabilityInfo"];
                if (dtBound != null && dtBound.Rows.Count > 0)
                {
                    DataRow[] drOutbound = dtBound.Select("FltType='" + "O" + "'");
                    DataRow[] drInbound = dtBound.Select("FltType='" + "I" + "'");

                    GetSsrAvailability objobjSsrAvailability = new GetSsrAvailability(Searchid);
                    DataTable dtAddOnFlights = objobjSsrAvailability.GetRoundWaySSR(drOutbound.CopyToDataTable(), drInbound.CopyToDataTable());

                    if (dtAddOnFlights != null && dtAddOnFlights.Rows.Count > 0)
                    {
                        dtAddOnFlights.TableName = "SSRInfo";
                        DataSet dsBound = new DataSet();
                        dsBound.Tables.Add(dtAddOnFlights.Copy());

                        dsBound.DataSetName = "root";
                        return GetCommonFunctions.DataSetToString(dsBound);
                    }
                }
            
            return string.Empty;
        }
    }
}
