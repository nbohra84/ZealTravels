using Microsoft.Data.SqlClient;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ZealTravel.Common.CommonUtility
{
    public static class CommonUtility
    {
        public static DataTable Collectdata(string Response, DataTable dtBound)
        {
            DataSet dsResponse = new DataSet();
            dsResponse.ReadXml(new System.IO.StringReader(Response));

            foreach (DataRow dr in dsResponse.Tables[0].Rows)
            {
                dtBound.ImportRow(dr);
            }
            dtBound.AcceptChanges();
            return dtBound;
        }

        public static string RemoveCarrierfromAvailabilityRequest(string TRequest, List<string> RemoveCarrier)
        {
            StringBuilder ReqXml = new StringBuilder();

            try
            {
                DataSet dsResponse = new DataSet();
                dsResponse.ReadXml(new System.IO.StringReader(TRequest));
                List<string> arCarrier = dsResponse.Tables["AirVInfo"].AsEnumerable()
                    .Select(row => row["AirV"].ToString().Trim()).ToList();

                foreach (var carrier in RemoveCarrier)
                {
                    arCarrier.Remove(carrier);
                }

                if (arCarrier.Count > 0)
                {
                    ReqXml.Append("<AvailabilityRequest>");
                    ReqXml.Append("<DepartureStation>" + dsResponse.Tables["AvailabilityRequest"].Rows[0]["DepartureStation"].ToString().Trim() + "</DepartureStation>");
                    ReqXml.Append("<ArrivalStation>" + dsResponse.Tables["AvailabilityRequest"].Rows[0]["ArrivalStation"].ToString().Trim() + "</ArrivalStation>");
                    ReqXml.Append("<Cabin>" + dsResponse.Tables["AvailabilityRequest"].Rows[0]["Cabin"].ToString().Trim() + "</Cabin>");

                    ReqXml.Append("<AirVAry>");
                    foreach (var carrier in arCarrier)
                    {
                        ReqXml.Append("<AirVInfo>");
                        ReqXml.Append("<AirV>" + carrier + "</AirV>");
                        ReqXml.Append("</AirVInfo>");
                    }

                    ReqXml.Append("</AirVAry>");
                    ReqXml.Append("<StartDate>" + dsResponse.Tables["AvailabilityRequest"].Rows[0]["StartDate"].ToString().Trim() + "</StartDate>");
                    ReqXml.Append("<EndDate>" + dsResponse.Tables["AvailabilityRequest"].Rows[0]["EndDate"].ToString().Trim() + "</EndDate>");
                    ReqXml.Append("<Adult>" + dsResponse.Tables["AvailabilityRequest"].Rows[0]["Adult"].ToString().Trim() + "</Adult>");
                    ReqXml.Append("<Child>" + dsResponse.Tables["AvailabilityRequest"].Rows[0]["Child"].ToString().Trim() + "</Child>");
                    ReqXml.Append("<Infant>" + dsResponse.Tables["AvailabilityRequest"].Rows[0]["Infant"].ToString().Trim() + "</Infant>");
                    ReqXml.Append("<SplFare>0</SplFare>");
                    ReqXml.Append("</AvailabilityRequest>");
                }
            }
            catch (Exception ex)
            {
                // dbCommon.Logger.dbLogg("", 0, "RemoveCarrierfromAvailabilityRequest", "apiAir_Caller", TRequest, ReqXml.ToString(), ex.Message);  
            }
            return ReqXml.ToString();
        }

        public static string RemoveCarrierfromAvailabilityRequest_uApi(string TRequest, bool Is6E, bool IsGDS)
        {
            StringBuilder ReqXml = new StringBuilder();

            try
            {
                DataSet dsResponse = new DataSet();
                dsResponse.ReadXml(new System.IO.StringReader(TRequest));

                ReqXml.Append("<AvailabilityRequest>");
                ReqXml.Append("<DepartureStation>" + dsResponse.Tables["AvailabilityRequest"].Rows[0]["DepartureStation"].ToString().Trim() + "</DepartureStation>");
                ReqXml.Append("<ArrivalStation>" + dsResponse.Tables["AvailabilityRequest"].Rows[0]["ArrivalStation"].ToString().Trim() + "</ArrivalStation>");
                ReqXml.Append("<Cabin>" + dsResponse.Tables["AvailabilityRequest"].Rows[0]["Cabin"].ToString().Trim() + "</Cabin>");

                ReqXml.Append("<AirVAry>");

                if (IsGDS)
                {
                    ReqXml.Append("<AirVInfo>");
                    ReqXml.Append("<AirV>" + "GDS" + "</AirV>");
                    ReqXml.Append("</AirVInfo>");
                }

                if (Is6E)
                {
                    ReqXml.Append("<AirVInfo>");
                    ReqXml.Append("<AirV>" + "6E" + "</AirV>");
                    ReqXml.Append("</AirVInfo>");
                }

                ReqXml.Append("</AirVAry>");

                ReqXml.Append("<StartDate>" + dsResponse.Tables["AvailabilityRequest"].Rows[0]["StartDate"].ToString().Trim() + "</StartDate>");
                ReqXml.Append("<EndDate>" + dsResponse.Tables["AvailabilityRequest"].Rows[0]["EndDate"].ToString().Trim() + "</EndDate>");
                ReqXml.Append("<Adult>" + dsResponse.Tables["AvailabilityRequest"].Rows[0]["Adult"].ToString().Trim() + "</Adult>");
                ReqXml.Append("<Child>" + dsResponse.Tables["AvailabilityRequest"].Rows[0]["Child"].ToString().Trim() + "</Child>");
                ReqXml.Append("<Infant>" + dsResponse.Tables["AvailabilityRequest"].Rows[0]["Infant"].ToString().Trim() + "</Infant>");
                ReqXml.Append("<SplFare>0</SplFare>");
                ReqXml.Append("</AvailabilityRequest>");
            }
            catch (Exception ex)
            {
                //dbCommon.Logger.dbLogg("", 0, "RemoveCarrierfromAvailabilityRequest_uApi", "apiAir_Caller", TRequest, ReqXml.ToString(), ex.Message);  
            }
            return ReqXml.ToString();
        }

        public static bool IsCarrierActiveFromClient(string Carriercode, string TRequest)
        {
            try
            {
                DataSet dsResponse = new DataSet();
                dsResponse.ReadXml(new System.IO.StringReader(TRequest));

                // Check if AirVInfo table exists and has rows
                if (dsResponse.Tables["AirVInfo"] == null || dsResponse.Tables["AirVInfo"].Rows.Count == 0)
                {
                    // If no AirVInfo table or empty, return false (carrier not specified)
                    return false;
                }

                List<string> arCarrier = dsResponse.Tables["AirVInfo"].AsEnumerable()
                    .Select(row => row["AirV"].ToString().Trim()).ToList();

                return arCarrier.Contains(Carriercode);
            }
            catch (Exception ex)
            {
                // If any error occurs (e.g., table doesn't exist), return false
                return false;
            }
        }

        public static string AddDefaultCarrierfromAvailabilityRequestMC(string TRequest)
        {
            StringBuilder ReqXml = new StringBuilder();

            try
            {
                DataSet dsResponse = new DataSet();
                dsResponse.ReadXml(new System.IO.StringReader(TRequest));

                ArrayList arCarrier = new ArrayList();
                foreach (DataRow dr in dsResponse.Tables["AirVInfo"].Rows)
                {
                    arCarrier.Add(dr["AirV"].ToString().Trim());
                }
                if (!arCarrier.Contains("6E"))
                {
                    arCarrier.Add("6E");
                }
                arCarrier = RemoveDuplicates(arCarrier);

                ReqXml.Append("<AvailabilityRequest>");
                ReqXml.Append("<Cabin>" + dsResponse.Tables["AvailabilityRequest"].Rows[0]["Cabin"].ToString().Trim() + "</Cabin>");

                ReqXml.Append("<AirSearch>");
                foreach (DataRow dr in dsResponse.Tables["AirSrchInfo"].Rows)
                {
                    ReqXml.Append("<AirSrchInfo>");
                    ReqXml.Append("<DepartureStation>" + dr["DepartureStation"].ToString() + "</DepartureStation>");
                    ReqXml.Append("<ArrivalStation>" + dr["ArrivalStation"].ToString() + "</ArrivalStation>");
                    ReqXml.Append("<StartDate>" + dr["StartDate"].ToString() + "</StartDate>");
                    ReqXml.Append("<EndDate></EndDate>");  /// added on 12 Jul 2023
                    ReqXml.Append("</AirSrchInfo>");
                    //break;  //----- for testing
                }
                ReqXml.Append("</AirSearch>");

                ReqXml.Append("<AirVAry>");
                for (int i = 0; i < arCarrier.Count; i++)
                {
                    ReqXml.Append("<AirVInfo>");
                    ReqXml.Append("<AirV>" + arCarrier[i].ToString() + "</AirV>");
                    ReqXml.Append("</AirVInfo>");
                }
                ReqXml.Append("</AirVAry>");

                ReqXml.Append("<Adult>" + dsResponse.Tables["AvailabilityRequest"].Rows[0]["Adult"].ToString().Trim() + "</Adult>");
                ReqXml.Append("<Child>" + dsResponse.Tables["AvailabilityRequest"].Rows[0]["Child"].ToString().Trim() + "</Child>");
                ReqXml.Append("<Infant>" + dsResponse.Tables["AvailabilityRequest"].Rows[0]["Infant"].ToString().Trim() + "</Infant>");
                ReqXml.Append("</AvailabilityRequest>");
            }
            catch (Exception ex)
            {
                // dbCommon.Logger.dbLogg("", 0, "AddDefaultCarrierfromAvailabilityRequestMC", "air_api_collector", TRequest, ReqXml.ToString(), ex.Message);
            }
            return ReqXml.ToString();
        }

        public static ArrayList RemoveDuplicates(ArrayList items)
        {
            ArrayList noDups = new ArrayList();
            foreach (string strItem in items)
            {
                if (!noDups.Contains(strItem.Trim()))
                {
                    noDups.Add(strItem.Trim());
                }
            }
            return noDups;
        }

        public static string GetAirlineID(string AirRS)
        {
            DataSet dsAvailability = CommonFunction.StringToDataSet(AirRS);
            return dsAvailability.Tables["AvailabilityInfo"].Rows[0]["AirlineID"].ToString().Trim();
        }

        public static DataTable ToDataTable<T>(this List<T> items)
        {
            DataTable dataTable = new DataTable(typeof(T).Name);
            //Get all properties and add columns to DataTable
            foreach (PropertyInfo prop in typeof(T).GetProperties())
            {
                dataTable.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);
            }

            foreach (var item in items)
            {
                var values = new object[typeof(T).GetProperties().Length];
                for (int i = 0; i < typeof(T).GetProperties().Length; i++)
                {
                    values[i] = item.GetType().GetProperty(typeof(T).GetProperties()[i].Name).GetValue(item, null);
                }

                dataTable.Rows.Add(values);
            }

            return dataTable;
        }


        public static bool VerifyCompany(string companyID)
        {
            bool verify = true;
            if (companyID.Equals("A-102"))
            {
                verify = false;
            }
            return verify;
        }

        public static string GetCompanyDetailForPNR(DataTable dtCompany)
        {
            
            if (dtCompany != null && dtCompany.Rows.Count > 0)
            {
                DataTable CompanyInfo = Schema.SchemaCompanyInfo;
                DataRow drAdd = CompanyInfo.NewRow();
                drAdd["CompanyName"] = dtCompany.Rows[0]["CompanyName"].ToString();
                drAdd["PostalCode"] = dtCompany.Rows[0]["PostalCode"].ToString();
                drAdd["StateCode"] = dtCompany.Rows[0]["StateCode"].ToString();
                drAdd["CountryCode"] = dtCompany.Rows[0]["CountryCode"].ToString();
                drAdd["CountryName"] = dtCompany.Rows[0]["CountryName"].ToString();
                drAdd["Email"] = dtCompany.Rows[0]["Company_Email"].ToString();
                drAdd["CityName"] = dtCompany.Rows[0]["City"].ToString();
                drAdd["MobileNo"] = dtCompany.Rows[0]["Company_Mobile"].ToString();
                drAdd["PhoneNo"] = dtCompany.Rows[0]["Company_PhoneNo"].ToString();
                drAdd["Address"] = dtCompany.Rows[0]["Address"].ToString();
                CompanyInfo.Rows.Add(drAdd);

                return CommonFunction.DataTableToString(CompanyInfo, "CompanyInfo", "root");
            }

            return string.Empty;
        }

        public static string GetAirlineID(string AirRS, string FltType)
        {
            DataSet dsAvailability = CommonFunction.StringToDataSet(AirRS);
            DataRow[] drSelect = dsAvailability.Tables[0].Select("FltType='" + FltType + "'");
            if (drSelect.Length > 0)
            {
                return drSelect.CopyToDataTable().Rows[0]["AirlineID"].ToString().Trim();
            }
            return string.Empty;
        }

        public static bool IsFamilyFare(string CarrierCode, string PriceType)
        {
            if (CarrierCode.IndexOf("6E") != -1 || CarrierCode.IndexOf("SG") != -1)
            {
                if (PriceType.ToLower().IndexOf("family") != -1)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
