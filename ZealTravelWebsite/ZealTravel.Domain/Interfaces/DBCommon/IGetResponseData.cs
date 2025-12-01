using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZealTravel.Domain.Interfaces.DBCommon
{
    public interface IGetResponseData
    {
        DataTable GetResponse(string Resp_tbo, string Resp_spicejet_max, string Resp_spicejet, string Resp_spicejet_coupon, string Resp_spicejet_corporate, string Resp_uapi, string Resp_uapi_sme, string Resp_6e, string Resp_qp, string journeyType, string Resp_db = null);
        string ConvertDataTableToString(DataTable dtBound, string SearchID, string CompanyID);
    }
}
