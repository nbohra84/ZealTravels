using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZealTravel.Domain.Interfaces.AirCalculation;
using ZealTravel.Domain.Interfaces.AirlineManagement.Akasaa;
using ZealTravel.Domain.Interfaces.DBCommon;
using ZealTravel.Domain.Interfaces.FlightManagement.Akasaa;
using ZealTravel.Infrastructure.AirCalculations;

/// <summary>
/// Summary description for GetFare
/// </summary>
namespace ZealTravel.Infrastructure.Akaasha
{
    public class GetFare: IGetFare
    {
        IRR_Layer _rr_layer;
        ICredential _credential;
        IGetApiAvailabilityFare _getApiAvailabilityFare;
        public GetFare(IRR_Layer rr_layer, IGetApiFlightFareRule getapiFlightFareRule, ICredential credential, IGetApiAvailabilityFare getApiAvailabilityFare)
        {
            _rr_layer = rr_layer;
            _credential = credential;
            _getApiAvailabilityFare = getApiAvailabilityFare;
        }
        public string GetFareOneWay(string Searchid, string Companyid, string RS_Availability)
        {

            DataTable dtBound = GetCommonFunctions.StringToDataSet(RS_Availability).Tables["AvailabilityInfo"];
            if (dtBound != null && dtBound.Rows.Count > 0)
            {
                bool IsFareAvailable = _getApiAvailabilityFare.GetOneWayFares(Searchid,dtBound, true);
                if (IsFareAvailable)
                {
                    //air_db_cal.rr_Layer objCal = new air_db_cal.rr_Layer();
                    dtBound = _rr_layer.GetAvailabilityCal(true, true, Searchid, Companyid, dtBound);

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
                
                bool IsFareAvailable = _getApiAvailabilityFare.GetRTFares(dtBound, true, Searchid,Companyid);
                if (IsFareAvailable)
                {
                    
                    dtBound = _rr_layer.GetAvailabilityCal(true, true, Searchid, Companyid, dtBound);

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