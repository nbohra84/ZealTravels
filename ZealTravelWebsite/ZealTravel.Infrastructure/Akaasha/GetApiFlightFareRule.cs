using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using ZealTravel.Domain.Interfaces.FlightManagement.Akasaa;
using Newtonsoft.Json;
using System.Xml;
using ZealTravel.Domain.Models;
using ZealTravel.Domain.Interfaces.DBCommon;

namespace ZealTravel.Infrastructure.Akaasha
{
    public class GetApiFlightFareRule: IGetApiFlightFareRule
    {
        public string errorMessage;
        //-----------------------------------------------------------------------------------------------
        private string Searchid;
        private string CompanyID;
        private Int32 BookingRef;
        private Int32 ContractVersion;
        ICredential _credential;
        //-----------------------------------------------------------------------------------------------

        public GetApiFlightFareRule(ICredential credential)
        {
            _credential = credential;
        }

        public GetApiFlightFareRule(string Searchid, string CompanyID, Int32 BookingRef)
        {
            this.ContractVersion = 420;
            this.Searchid = Searchid;
            this.CompanyID = CompanyID;
            this.BookingRef = BookingRef;
        }

        //-----------------------------------------------------------------------------------------------
        public string GetFareRule(DataTable dtBound, string Searchid, string CompanyID, Int32 BookingRef)
        {
            var commonQP = new CommonQP(_credential);
            string _Token = commonQP.GetTokenAsync("", "").GetAwaiter().GetResult();
            return GetFareRule(dtBound, _Token);
        }

        public string GetFareRule(DataTable dtBound, string _Token)
        {
            DataTable dtFareRule = DBCommon.Schema.SchemaFareRule;
            string Supplierid = dtBound.Rows[0]["AirlineID"].ToString();
            string Signature = dtBound.Rows[0]["Api_SessionID"].ToString();

            DataTable dtSelected = dtBound.Copy();
            string[] column = { "Origin", "Destination", "ClassOfService", "FareBasisCode", "RuleNumber" };
            dtSelected = dtSelected.DefaultView.ToTable("dtRules", false, column);

            DataColumn[] keyColumns = new DataColumn[] { dtSelected.Columns["ClassOfService"], dtSelected.Columns["FareBasisCode"], dtSelected.Columns["RuleNumber"] };
            GetCommonFunctions.RemoveDuplicatesDatatableColumns(dtSelected, keyColumns);
            foreach (DataRow dr in dtSelected.Rows)
            {
                string AirlineRule = GetFlightFareRule(ContractVersion, Supplierid, Signature, "SG", dr["ClassOfService"].ToString(), dr["FareBasisCode"].ToString(), dr["RuleNumber"].ToString(), dtBound, _Token);

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
        private string GetFlightFareRule(int ContractVersion, string Supplierid, string Signature, string CarrierCode, string ClassOfService, string FareBasisCode, string RuleNumber, DataTable dtBound, string _Token)
        {
            string GetApiRequest = "";
            string GetApiResponse = "";



            try
            {


                CS_FARE_SSR objfare = new CS_FARE_SSR();
                string _JournyKey = "";
                string _SegmantKey = "";
                string JsonFlightFareRulesRQ = objfare.GetFlightFareRulesRQ_RTF(Searchid, dtBound, out _JournyKey, out _SegmantKey);

                Task<string> jsonSearchRSAw = Task.Run(() => CommonQP.GetResponseQpFareRuleAsync(Searchid, JsonFlightFareRulesRQ, "FareRule", _Token, _JournyKey, _SegmantKey));
                string JsonFlightFareRulesRS = jsonSearchRSAw.GetAwaiter().GetResult();

                //===== convert RTF encodedata

                string xmlString = Newtonsoft.Json.JsonConvert.DeserializeXmlNode(JsonFlightFareRulesRS, "text").OuterXml;
                System.Xml.XmlDocument xmlDoc = new System.Xml.XmlDocument();
                xmlDoc.LoadXml(xmlString);
                System.Xml.XmlNode root = xmlDoc.DocumentElement;
                var _txt = root.ChildNodes[0]["text"].InnerText;
                var bytes = Convert.FromBase64String(_txt);
                var text = Encoding.ASCII.GetString(bytes);
                string JsonFlightFareRulesRS_PlaneText = CommonQP.ConvertRtfToPlainText(text); // CommonQP.ConvertRtfToPlainText(text);
                return JsonFlightFareRulesRS_PlaneText;
            }
            catch (Exception ex)
            {
                this.errorMessage = ex.Message;
                DBCommon.Logger.dbLogg(CompanyID, BookingRef, "GetFlightFareRule", "air_AkasaAir", Supplierid, Searchid, ex.Message + "," + ex.StackTrace);
            }
            DBCommon.Logger.WriteLogg(CompanyID, BookingRef, "GetFlightFareRule-air_AkasaAir", "RULE", GetApiRequest + Environment.NewLine + GetApiResponse, "", Searchid);
            return "Conatct to our Operation Team or Airline CallCenter as per Airline Policy TnC Apply";
        }
    }
}
