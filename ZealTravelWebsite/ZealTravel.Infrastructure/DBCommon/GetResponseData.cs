using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZealTravel.Common.CommonUtility;
using ZealTravel.Domain.Interfaces.AirCalculation;
using ZealTravel.Domain.Interfaces.DBCommon;
using ZealTravel.Infrastructure.FlightManagement;

namespace ZealTravel.Infrastructure.DBCommon
{
    public class GetResponseData: IGetResponseData
    {
        IConfiguration _configuration;
        IRR_Layer _layer;
        public GetResponseData(IConfiguration configuration, IRR_Layer layer) 
        {
            _configuration = configuration;
            _layer = layer;
        }
        public DataTable GetResponse(string Resp_tbo, string Resp_spicejet_max, string Resp_spicejet, string Resp_spicejet_coupon, string Resp_spicejet_corporate, string Resp_uapi, string Resp_uapi_sme, string Resp_6e, string Resp_qp,string journeyType, string Resp_db=null )
        {
            DataTable dtBound = DBCommon.Schema.SchemaFlights;

            // CommonUtility objcu = new air_api_collector.CommonUtility();
            if (Resp_db != null && Resp_db.IndexOf("RefID") != -1)
            {
                CommonUtility.Collectdata(Resp_db, dtBound);
            }
            if (Resp_tbo != null && Resp_tbo.IndexOf("RefID") != -1)
            {
                CommonUtility.Collectdata(Resp_tbo, dtBound);
            }
            if (Resp_spicejet_max != null && Resp_spicejet_max.IndexOf("RefID") != -1)
            {
                CommonUtility.Collectdata(Resp_spicejet_max, dtBound);
            }
            if (Resp_spicejet != null && Resp_spicejet.IndexOf("RefID") != -1)
            {
                CommonUtility.Collectdata(Resp_spicejet, dtBound);
            }
            if (Resp_spicejet_coupon != null && Resp_spicejet_coupon.IndexOf("RefID") != -1)
            {
                CommonUtility.Collectdata(Resp_spicejet_coupon, dtBound);
            }
            if (Resp_spicejet_corporate != null && Resp_spicejet_corporate.IndexOf("RefID") != -1)
            {
                CommonUtility.Collectdata(Resp_spicejet_corporate, dtBound);
            }
            if (Resp_uapi != null && Resp_uapi.IndexOf("RefID") != -1)
            {
                CommonUtility.Collectdata(Resp_uapi, dtBound);
            }
            if (Resp_uapi_sme != null && Resp_uapi_sme.IndexOf("RefID") != -1)
            {
                CommonUtility.Collectdata(Resp_uapi_sme, dtBound);
            }
            if (Resp_6e != null && Resp_6e.IndexOf("RefID") != -1)
            {
                CommonUtility.Collectdata(Resp_6e, dtBound);
            }
            if (Resp_qp != null && Resp_qp.IndexOf("RefID") != -1)
            {
                CommonUtility.Collectdata(Resp_qp, dtBound);
            }
            //=======================================================================================

            if (dtBound != null && dtBound.Rows.Count > 0 && journeyType != "RT" && journeyType != "MC")
            {
                dtBound = MergeMultipleSupplierData(dtBound);
            }

            dtBound = DBCommon.GetRequestData.Add_RowID(dtBound);

            return dtBound;
        }

        public DataTable MergeMultipleSupplierData(DataTable dtResponse)
        {
            DataTable dtBound = DBCommon.Schema.SchemaFlights;
            DataRow[] drSelect;
            if (dtResponse != null && dtResponse.Rows.Count > 0)
            {
                drSelect = dtResponse.Select("FltType='" + "O" + "'");
                if (drSelect.Length > 0)
                {
                    dtBound.Merge(drSelect.CopyToDataTable());
                }
                drSelect = dtResponse.Select("FltType='" + "I" + "'");
                if (drSelect.Length > 0)
                {
                    dtBound.Merge(drSelect.CopyToDataTable());
                }
            }
            dtBound = DBCommon.GetRequestData.Add_RowID(dtBound);
            return dtBound;
        }

        public string ConvertDataTableToString(DataTable dtBound,string SearchID, string CompanyID)
        {
            if (dtBound != null && dtBound.Rows.Count > 0)
            {
                CommonFunction objCommonFunction = new CommonFunction();
                dtBound = objCommonFunction.SetRowid(dtBound);
                dtBound.TableName = "AvailabilityInfo";

                DataSet dsBound = new DataSet();
                dsBound.Tables.Add(dtBound);

                AddAirlineDetails objAirlineDetail = new AddAirlineDetails(_configuration);
                objAirlineDetail.Add_Detail(dsBound);

                dsBound.DataSetName = "root";
                string RS_Availability = DBCommon.CommonFunction.DataSetToString(dsBound);

                if (RS_Availability.IndexOf("RefID") != -1)
                {
                    //air_db_cal.rr_Layer objC = new air_db_cal.rr_Layer();
                    RS_Availability = _layer.GetAvailabilityCal(true, true, SearchID, CompanyID, RS_Availability);
                    return RS_Availability;
                }
            }
            return String.Empty;
        }
    }
}
