using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using ZealTravel.Infrastructure.DBCommon;
using ZealTravel.Infrastructure.Akaasha;

namespace ZealTravel.Infrastructure.Spicejet
{
    class GetApiFlightFareRule
    {
        public string errorMessage;
        //-----------------------------------------------------------------------------------------------
        private string Searchid;
        private string CompanyID;
        private Int32 BookingRef;
        private Int32 ContractVersion;
        //-----------------------------------------------------------------------------------------------
        public GetApiFlightFareRule(string Searchid, string CompanyID, Int32 BookingRef)
        {
            this.ContractVersion = 420;
            this.Searchid = Searchid;
            this.CompanyID = CompanyID;
            this.BookingRef = BookingRef;
        }
        //-----------------------------------------------------------------------------------------------
        public string GetFareRule(DataTable dtBound)
        {
            DataTable dtFareRule = Schema.SchemaFareRule;
            string Supplierid = dtBound.Rows[0]["AirlineID"].ToString();
            string Signature = dtBound.Rows[0]["Api_SessionID"].ToString();

            DataTable dtSelected = dtBound.Copy();
            string[] column = { "Origin", "Destination", "ClassOfService", "FareBasisCode", "RuleNumber" };
            dtSelected = dtSelected.DefaultView.ToTable("dtRules", false, column);

            DataColumn[] keyColumns = new DataColumn[] { dtSelected.Columns["ClassOfService"], dtSelected.Columns["FareBasisCode"], dtSelected.Columns["RuleNumber"] };
            GetCommonFunctions.RemoveDuplicatesDatatableColumns(dtSelected, keyColumns);
            foreach (DataRow dr in dtSelected.Rows)
            {
                string AirlineRule = GetFlightFareRule(ContractVersion, Supplierid, Signature, "SG", dr["ClassOfService"].ToString(), dr["FareBasisCode"].ToString(), dr["RuleNumber"].ToString());

                DataRow drAdd = dtFareRule.NewRow();
                drAdd["Origin"] = dr["Origin"].ToString();
                drAdd["Destination"] = dr["Destination"].ToString();
                drAdd["RuleDetail"] = AirlineRule;
                dtFareRule.Rows.Add(drAdd);
            }

            if (dtFareRule != null && dtFareRule.Rows.Count > 0)
            {
                dtFareRule.TableName = "FareRuleInfo";
                DataSet dsBound = new DataSet();
                dsBound.Tables.Add(dtFareRule.Copy());
                dsBound.DataSetName = "root";
                return GetCommonFunctions.DataSetToString(dsBound);
            }
            return "";
        }
        private string GetFlightFareRule(int ContractVersion, string Supplierid, string Signature, string CarrierCode, string ClassOfService, string FareBasisCode, string RuleNumber)
        {
            string GetApiRequest = "";
            string GetApiResponse = "";
            try
            {
                System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;

                svc_content.FareRuleRequestData objFareRuleRequestData = new svc_content.FareRuleRequestData();
                objFareRuleRequestData.CarrierCode = CarrierCode;
                objFareRuleRequestData.ClassOfService = ClassOfService;
                objFareRuleRequestData.FareBasisCode = FareBasisCode;
                objFareRuleRequestData.RuleNumber = RuleNumber;

                GetApiRequest = GetCommonFunctions.Serialize(objFareRuleRequestData);

                svc_content.ContentManagerClient objContentManagerClient = new svc_content.ContentManagerClient();
                svc_content.FareRuleInfo objFareRuleInfo = objContentManagerClient.GetFareRuleInfo(ContractVersion, false, "", Signature, objFareRuleRequestData);

                GetApiResponse = GetCommonFunctions.Serialize(objFareRuleInfo);

                byte[] buffer = objFareRuleInfo.Data;
                string Rule = CommonQP.ConvertRtfToPlainText(Encoding.UTF8.GetString(buffer, 0, buffer.Length));
                
               // System.Windows.Forms.RichTextBox rtBox = new System.Windows.Forms.RichTextBox();
               // rtBox.Rtf = Rule;
                return (Rule);
            }
            catch (Exception ex)
            {
                this.errorMessage = ex.Message;
               // dbCommon.Logger.dbLogg(CompanyID, BookingRef, "GetFlightFareRule", "air_spicejet", Supplierid, Searchid, ex.Message + "," + ex.StackTrace);
            }
           // dbCommon.Logger.WriteLogg(CompanyID, BookingRef, "GetFlightFareRule-air_spicejet", "RULE", GetApiRequest + Environment.NewLine + GetApiResponse, "", Searchid);
            return "Conatct to our Operation Team or Airline CallCenter as per Airline Policy TnC Apply";
        }
    }
}
