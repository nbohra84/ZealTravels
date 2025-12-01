using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZealTravel.Domain.Interfaces.AirCalculation;
using ZealTravel.Infrastructure.AirCalculations;

namespace ZealTravel.Infrastructure.Spicejet
{
    public class GetFare
    {
        IRR_Layer _rr_Layer;
        public GetFare()
        {
            _rr_Layer = new rr_Layer();
        }
        public string GetFareOneWay(string Searchid, string Companyid, string RS_Availability)
        {

            DataTable dtBound = GetCommonFunctions.StringToDataSet(RS_Availability).Tables["AvailabilityInfo"];
            if (dtBound != null && dtBound.Rows.Count > 0)
            {
                GetApiAvailabilityFare objFare = new GetApiAvailabilityFare(Searchid, Companyid, 0);
                bool IsFareAvailable = objFare.GetOneWayFares(dtBound, true);
                if (IsFareAvailable)
                {
                    dtBound = _rr_Layer.GetAvailabilityCal(true, true, Searchid, Companyid, dtBound);

                    if (dtBound != null && dtBound.Rows.Count > 0)
                    {
                        dtBound.TableName = "AvailabilityInfo";
                        DataSet dsBound = new DataSet();
                        dsBound.Tables.Add(dtBound.Copy());

                        GetCommonFunctions objCommonFunctions = new GetCommonFunctions();
                        objCommonFunctions.Add_Journey_Duration_TimeDetail(dsBound);


                        dsBound.DataSetName = "root";
                        return GetCommonFunctions.DataSetToString(dsBound);
                    }
                }
                else
                {

                }
            }

            return string.Empty;
        }
        public string GetFareRT(string Searchid, string Companyid, string RS_Availability)
        {

            DataTable dtBound = GetCommonFunctions.StringToDataSet(RS_Availability).Tables["AvailabilityInfo"];
            if (dtBound != null && dtBound.Rows.Count > 0)
            {
                GetApiAvailabilityFare objFare = new GetApiAvailabilityFare(Searchid, Companyid, 0);
                bool IsFareAvailable = objFare.GetRTFares(dtBound, true);
                if (IsFareAvailable)
                {
                    
                    dtBound = _rr_Layer.GetAvailabilityCal(true, true, Searchid, Companyid, dtBound);

                    if (dtBound != null && dtBound.Rows.Count > 0)
                    {
                        dtBound.TableName = "AvailabilityInfo";
                        DataSet dsBound = new DataSet();
                        dsBound.Tables.Add(dtBound.Copy());

                        GetCommonFunctions objCommonFunctions = new GetCommonFunctions();
                        objCommonFunctions.Add_Journey_Duration_TimeDetail(dsBound);

                        dsBound.DataSetName = "root";
                        return GetCommonFunctions.DataSetToString(dsBound);
                    }
                }
                else
                {

                }
            }

            return string.Empty;
        }
    }
}
