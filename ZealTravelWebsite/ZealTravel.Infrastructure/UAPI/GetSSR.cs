using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZealTravel.Infrastructure.UAPI
{
    class GetSSR
    {
        public DataTable GetSSRData(string JourneyType, string Searchid, string Companyid, DataTable dtBound)
        {
            DataTable dtAddOnData = new DataTable();
            string CarrierCode = dtBound.Rows[0]["CarrierCode"].ToString().Trim().ToUpper();
            if (CarrierCode.Equals("6E"))
            {

            }
            else
            {
                dtAddOnData = GetSSRFromDataBase("AI", "O");
                if (JourneyType.Equals("RT"))
                {
                    dtAddOnData.Merge(GetSSRFromDataBase("AI", "I"));
                }
            }
            return dtAddOnData;
        }
        private DataTable GetSSRFromTxtFile(string JourneyType, string Searchid, string Companyid, string FltType)
        {
            return null;
        }
        private DataTable GetSSRFromDataBase(string CarrierCode, string FltType)
        {
            DataTable dtSSR = DBCommon.Schema.SchemaSSR;

            int iRow = 1;
            CommonUapi objssr = new CommonUapi();
            DataTable dtSSRlist = objssr.GetdbSSRList(CarrierCode, "");
            if (dtSSRlist != null && dtSSRlist.Rows.Count > 0)
            {
                foreach (DataRow dr in dtSSRlist.Rows)
                {
                    DataRow drAdd = dtSSR.NewRow();
                    drAdd["RowID"] = iRow;
                    drAdd["CarrierCode"] = dr["CarrierCode"].ToString();
                    drAdd["Code"] = dr["SsrCode"].ToString();
                    drAdd["CodeType"] = dr["SsrType"].ToString();
                    drAdd["Amount"] = dr["Amount"].ToString();
                    drAdd["Description"] = dr["Description"].ToString();
                    drAdd["Detail"] = dr["AdditionalRule"].ToString();
                    drAdd["FltType"] = FltType;
                    dtSSR.Rows.Add(drAdd);
                    iRow++;
                }
            }
            return dtSSR;
        }
    }
}
