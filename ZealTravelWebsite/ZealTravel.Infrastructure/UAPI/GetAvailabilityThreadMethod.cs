using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Data;
using System.Collections;

namespace ZealTravel.Infrastructure.UAPI
{
    class GetAvailabilityThreadMethod
    {
        private string NetworkUserName;
        private string NetworkPassword;
        private string TargetBranch;

        private string SearchID;
        private string CompanyID;
        private string RQ_Flight;

        public string Sector;
        private DataTable dtBound;
        public string Done;
        public string Done_O;
        public string Done_I;
        string ress = string.Empty;

        string Adt;
        string Chd;
        string Inf;

        bool Is6E;
        string DepartureStation;
        string ArrivalStation;
        bool UAPISME;
        public GetAvailabilityThreadMethod(string NetworkUserName, string NetworkPassword, string TargetBranch, string SearchID, string CompanyID, string RQ_Flight, string Sector, bool uapiSME = false)
        {
            this.NetworkUserName = NetworkUserName;
            this.NetworkPassword = NetworkPassword;
            this.TargetBranch = TargetBranch;
            this.RQ_Flight = RQ_Flight;
            this.SearchID = SearchID;
            this.CompanyID = CompanyID;
            this.Sector = Sector;
            this.UAPISME = uapiSME;
            XmlDocument xmldoc = new XmlDocument();
            xmldoc.LoadXml(RQ_Flight);

            DepartureStation = xmldoc.SelectSingleNode("AvailabilityRequest/DepartureStation").InnerText;
            ArrivalStation = xmldoc.SelectSingleNode("AvailabilityRequest/ArrivalStation").InnerText;
            string BeginDate = xmldoc.SelectSingleNode("AvailabilityRequest/StartDate").InnerText;
            string EndDate = xmldoc.SelectSingleNode("AvailabilityRequest/EndDate").InnerText;
            Adt = (xmldoc.SelectSingleNode("AvailabilityRequest/Adult").InnerText);
            Chd = (xmldoc.SelectSingleNode("AvailabilityRequest/Child").InnerText);
            Inf = (xmldoc.SelectSingleNode("AvailabilityRequest/Infant").InnerText);
            string Cabin = xmldoc.SelectSingleNode("AvailabilityRequest/Cabin").InnerText;

            dtBound = DBCommon.Schema.SchemaFlights;

            ArrayList AirlineList = new ArrayList();
            XmlNodeList nodeList = xmldoc.SelectNodes("AvailabilityRequest/AirVAry/AirVInfo");
            foreach (XmlNode no in nodeList)
            {
                AirlineList.Add(no.FirstChild.InnerText);
            }
            Is6E = false;
            if (AirlineList != null && AirlineList.Contains("6E"))
            {
                Is6E = true;
            }
        }
        public DataTable GetResponse()
        {
            dtBound.TableName = "AvailabilityResponse";
            return dtBound;
        }

        public void GET_FLT()
        {
            try
            {
                dtBound = GetOneWay("O");
            }
            catch (Exception ex)
            {

            }
            this.Done = "D";
        }
        public async Task<string> GET_FLT_Async()
        {
            //DataTable dt = new DataTable();
            //dt.TableName = "AvailabilityResponse";
            try
            {
                Console.WriteLine($"[GET_FLT_Async] Starting GetOneWayAsync for SearchID: {SearchID}");
                dtBound = await GetOneWayAsync("O");
                Console.WriteLine($"[GET_FLT_Async] GetOneWayAsync completed - dtBound rows: {dtBound?.Rows?.Count ?? 0}, SearchID: {SearchID}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[GET_FLT_Async] Exception in GetOneWayAsync: {ex.Message}");
                Console.WriteLine($"[GET_FLT_Async] StackTrace: {ex.StackTrace}");
                Console.WriteLine($"[GET_FLT_Async] SearchID: {SearchID}");
            }
            this.Done = "D";
            return this.Done;
        }

        public void GetOutbound()
        {
            try
            {
                dtBound.Merge(GetOneWay("O"));
            }
            catch (Exception ex)
            {

            }
            this.Done_O = "D";
        }
        public async Task<string> GetOutboundAsync()
        {
            try
            {
                dtBound.Merge(await GetOneWayAsync("O"));
            }
            catch (Exception ex)
            {

            }
            this.Done_O = "D";
            return this.Done_O;
        }
        public void GetInbound()
        {
            try
            {
                dtBound.Merge(GetOneWay("I"));
            }
            catch (Exception ex)
            {

            }
            this.Done_I = "D";
        }
        public async Task<string> GetInboundAsync()
        {
            try
            {
                dtBound.Merge(await GetOneWayAsync("I"));
            }
            catch (Exception ex)
            {

            }
            this.Done_I = "D";
            return this.Done_I;
        }
        private DataTable GetOneWay(string FltType)
        {
            DataTable dtFltBound = DBCommon.Schema.SchemaFlights;
            string reqq = string.Empty;

            try
            {
                string AdultBTR = string.Empty;
                string ChildBTR = string.Empty;
                string InfantBTR = string.Empty;

                GetAvailability objGA = new GetAvailability(TargetBranch);
                reqq = objGA.GetAvailabilityRequest(SearchID, RQ_Flight, FltType, Sector, ref AdultBTR, ref ChildBTR, ref InfantBTR, UAPISME);

                CommonUapi objcuapi = new CommonUapi();
                ress = objcuapi.GetResponseUapi(NetworkUserName, NetworkPassword, SearchID, reqq, "AirService", "AVAILABILITY");

                if (Is6E)
                {
                    GetAvailabilityResponse6E objRes = new GetAvailabilityResponse6E(NetworkUserName, NetworkPassword, TargetBranch, Adt, Chd, Inf, DepartureStation, ArrivalStation);
                    dtFltBound = objRes.SetAvailabilityResponse(SearchID, CompanyID, Sector, false, false, RQ_Flight, ress, FltType, AdultBTR, ChildBTR, InfantBTR);
                }
                else
                {
                    GetAvailabilityResponse2 objRes = new GetAvailabilityResponse2(NetworkUserName, NetworkPassword, TargetBranch, Adt, Chd, Inf, DepartureStation, ArrivalStation);
                    dtFltBound = objRes.SetAvailabilityResponse(SearchID, CompanyID, Sector, false, false, RQ_Flight, ress, FltType, AdultBTR, ChildBTR, InfantBTR, UAPISME);
                }

                if (dtFltBound != null && dtFltBound.Rows.Count > 0)
                {
                    dtFltBound = objcuapi.RemoveUncombinedDataOW(SearchID, RQ_Flight, ress, dtFltBound);
                }

                //if (dtFltBound != null && dtFltBound.Rows.Count > 0)
                //{
                //    dtFltBound = objcuapi.RemoveClosePriceTypeWiseFare(SearchID, dtFltBound, SearchType, dtFltBound.Rows[0]["AirlineID"].ToString(), "6E", Sector);
                //    if (UserName.Equals("KTDEL306")|| UserName.Equals("KTDEL324"))
                //    {
                //        dtFltBound = objcuapi.OnlyPriceTypeWiseFare(SearchID, dtFltBound, SearchType, dtFltBound.Rows[0]["AirlineID"].ToString(), "6E", Sector);
                //    }
                //}
            }
            catch (Exception ex)
            {
                DBCommon.Logger.dbLogg(CompanyID, 0, "GET-" + FltType, "Availability-UAPI", "", SearchID, ex.Message);
            }

            DBCommon.Logger.WriteLogg(CompanyID, 0, "GET-" + FltType + "-UAPI", "Availability", reqq + Environment.NewLine + ress, "", SearchID);
            return dtFltBound;
        }
        private  async Task<DataTable> GetOneWayAsync(string FltType)
        {
            DataTable dtFltBound = DBCommon.Schema.SchemaFlights;
            string reqq = string.Empty;

            try
            {
                string AdultBTR = string.Empty;
                string ChildBTR = string.Empty;
                string InfantBTR = string.Empty;

                GetAvailability objGA = new GetAvailability(TargetBranch);
                reqq = objGA.GetAvailabilityRequest(SearchID, RQ_Flight, FltType, Sector, ref AdultBTR, ref ChildBTR, ref InfantBTR, UAPISME);

                Console.WriteLine($"[GetOneWayAsync] Making API call - SearchID: {SearchID}, FltType: {FltType}, Sector: {Sector}, Request length: {reqq?.Length ?? 0}");
                
                CommonUapi objcuapi = new CommonUapi();
                ress =await objcuapi.GetResponseUapiAsync(NetworkUserName, NetworkPassword, SearchID, reqq, "AirService", "AVAILABILITY");
                
                Console.WriteLine($"[GetOneWayAsync] API response received - Length: {ress?.Length ?? 0}, HasRefID: {(ress?.IndexOf("RefID") ?? -1) != -1}");

                if (Is6E)
                {
                    GetAvailabilityResponse6E objRes = new GetAvailabilityResponse6E(NetworkUserName, NetworkPassword, TargetBranch, Adt, Chd, Inf, DepartureStation, ArrivalStation);
                    dtFltBound = objRes.SetAvailabilityResponse(SearchID, CompanyID, Sector, false, false, RQ_Flight, ress, FltType, AdultBTR, ChildBTR, InfantBTR);
                    //GetAvailabilityResponse2 objRes = new GetAvailabilityResponse2(NetworkUserName, NetworkPassword, TargetBranch, Adt, Chd, Inf, DepartureStation, ArrivalStation);
                    //dtFltBound = objRes.SetAvailabilityResponse(SearchID, CompanyID, Sector, false, false, RQ_Flight, ress, FltType, AdultBTR, ChildBTR, InfantBTR);
                }
                else
                {
                    
                    GetAvailabilityResponse2 objRes = new GetAvailabilityResponse2(NetworkUserName, NetworkPassword, TargetBranch, Adt, Chd, Inf, DepartureStation, ArrivalStation);
                    dtFltBound =objRes.SetAvailabilityResponse(SearchID, CompanyID, Sector, false, false, RQ_Flight, ress, FltType, AdultBTR, ChildBTR, InfantBTR, UAPISME);
                    
                    Console.WriteLine($"[GetOneWayAsync] SetAvailabilityResponse returned - Rows: {dtFltBound?.Rows?.Count ?? 0}");
                }


                if (dtFltBound != null && dtFltBound.Rows.Count > 0)
                {
                    dtFltBound = objcuapi.RemoveUncombinedDataOW(SearchID, RQ_Flight, ress, dtFltBound);
                    Console.WriteLine($"[GetOneWayAsync] After RemoveUncombinedDataOW - Rows: {dtFltBound?.Rows?.Count ?? 0}");
                }


                //if (dtFltBound != null && dtFltBound.Rows.Count > 0)
                //{
                //    dtFltBound = objcuapi.RemoveClosePriceTypeWiseFare(SearchID, dtFltBound, SearchType, dtFltBound.Rows[0]["AirlineID"].ToString(), "6E", Sector);
                //    if (UserName.Equals("KTDEL306")|| UserName.Equals("KTDEL324"))
                //    {
                //        dtFltBound = objcuapi.OnlyPriceTypeWiseFare(SearchID, dtFltBound, SearchType, dtFltBound.Rows[0]["AirlineID"].ToString(), "6E", Sector);
                //    }
                //}
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[GetOneWayAsync] Exception in GetOneWayAsync: {ex.Message}");
                Console.WriteLine($"[GetOneWayAsync] StackTrace: {ex.StackTrace}");
                Console.WriteLine($"[GetOneWayAsync] SearchID: {SearchID}, FltType: {FltType}");
                DBCommon.Logger.dbLogg(CompanyID, 0, "GET-" + FltType, "Availability-UAPI", "", SearchID, ex.Message);
            }

            DBCommon.Logger.WriteLogg(CompanyID, 0, "GET-" + FltType + "-UAPI", "Availability", reqq + Environment.NewLine + ress, "", SearchID);
            
            Console.WriteLine($"[GetOneWayAsync] Returning DataTable - Rows: {dtFltBound?.Rows?.Count ?? 0}, SearchID: {SearchID}, Response length: {ress?.Length ?? 0}");
            
            return dtFltBound;
        }
        //======================================================================================================================
        public void GetFlightRT()
        {
            string reqq = string.Empty;
            string ress = string.Empty;
            try
            {
                string AdultBTR = string.Empty;
                string ChildBTR = string.Empty;
                string InfantBTR = string.Empty;

                GetAvailability objGA = new GetAvailability(TargetBranch);
                reqq = objGA.GetAvailabilityRequestRT(SearchID, RQ_Flight, Sector, ref AdultBTR, ref ChildBTR, ref InfantBTR, UAPISME);

                CommonUapi objcuapi = new CommonUapi();
                ress = objcuapi.GetResponseUapi(NetworkUserName, NetworkPassword, SearchID, reqq, "AirService", "AVAILABILITY");

                if (Is6E)
                {
                    GetAvailabilityResponse6ERT2 objRes = new GetAvailabilityResponse6ERT2(NetworkUserName, NetworkPassword, TargetBranch, Adt, Chd, Inf, DepartureStation, ArrivalStation);
                    dtBound = objRes.SetAvailabilityResponse(SearchID, CompanyID, Sector, true, false, RQ_Flight, ress, "", AdultBTR, ChildBTR, InfantBTR);
                }
                else
                {
                    GetAvailabilityResponseRT2 objRes = new GetAvailabilityResponseRT2(NetworkUserName, NetworkPassword, TargetBranch, Adt, Chd, Inf, DepartureStation, ArrivalStation);
                    dtBound = objRes.SetAvailabilityResponse(SearchID, CompanyID, Sector, true, false, RQ_Flight, ress, "", AdultBTR, ChildBTR, InfantBTR, UAPISME);
                }

                if (dtBound != null && dtBound.Rows.Count > 0)
                {
                    dtBound = objcuapi.RemoveUncombinedData(SearchID, RQ_Flight, ress, dtBound);
                }
            }
            catch (Exception ex)
            {
                DBCommon.Logger.dbLogg(CompanyID, 0, "GET_FLT_RT", "Availability-UAPI", "", SearchID, ex.Message);
            }

            DBCommon.Logger.WriteLogg(CompanyID, 0, "GET_FLT_RT-UAPI", "PNR", reqq + Environment.NewLine + ress, "", SearchID);
            this.Done = "D";
        }
        public async Task<string> GetFlightRT_Async()
        {
            string reqq = string.Empty;
            string ress = string.Empty;
            //DataTable dtBound_ = new DataTable();
            try
            {
                string AdultBTR = string.Empty;
                string ChildBTR = string.Empty;
                string InfantBTR = string.Empty;

                GetAvailability objGA = new GetAvailability(TargetBranch);
                reqq = objGA.GetAvailabilityRequestRT(SearchID, RQ_Flight, Sector, ref AdultBTR, ref ChildBTR, ref InfantBTR, UAPISME);

                CommonUapi objcuapi = new CommonUapi();
                ress = await objcuapi.GetResponseUapiAsync(NetworkUserName, NetworkPassword, SearchID, reqq, "AirService", "AVAILABILITY");

                if (Is6E)
                {
                    GetAvailabilityResponse6ERT2 objRes = new GetAvailabilityResponse6ERT2(NetworkUserName, NetworkPassword, TargetBranch, Adt, Chd, Inf, DepartureStation, ArrivalStation);
                    dtBound = objRes.SetAvailabilityResponse(SearchID, CompanyID, Sector, true, false, RQ_Flight, ress, "", AdultBTR, ChildBTR, InfantBTR);
                }
                else
                {
                    GetAvailabilityResponseRT2 objRes = new GetAvailabilityResponseRT2(NetworkUserName, NetworkPassword, TargetBranch, Adt, Chd, Inf, DepartureStation, ArrivalStation);
                    dtBound = objRes.SetAvailabilityResponse(SearchID, CompanyID, Sector, true, false, RQ_Flight, ress, "", AdultBTR, ChildBTR, InfantBTR, UAPISME);
                }

                if (dtBound != null && dtBound.Rows.Count > 0)
                {
                    dtBound = objcuapi.RemoveUncombinedData(SearchID, RQ_Flight, ress, dtBound);
                }
            }
            catch (Exception ex)
            {
                DBCommon.Logger.dbLogg(CompanyID, 0, "GET_FLT_RT", "Availability-UAPI", "", SearchID, ex.Message);
            }

            DBCommon.Logger.WriteLogg(CompanyID, 0, "GET_FLT_RT-UAPI", "PNR", reqq + Environment.NewLine + ress, "", SearchID);
            this.Done = "D";
            return this.Done;
        }
        //======================================================================================================================
        public void GetFlightMultiCity()
        {
            string reqq = string.Empty;
            string ress = string.Empty;
            try
            {
                string AdultBTR = string.Empty;
                string ChildBTR = string.Empty;
                string InfantBTR = string.Empty;

                //GetAvailability objGA = new air_uapi.GetAvailability(TargetBranch);
                //reqq = objGA.GetAvailabilityRequestMC(SearchID, RQ_Flight, Sector, ref AdultBTR, ref ChildBTR, ref InfantBTR);

                //CommonUapi objcuapi = new air_uapi.CommonUapi();
                //ress = objcuapi.GetResponseUapi(NetworkUserName, NetworkPassword, SearchID, reqq, "AirService", "AVAILABILITY");
                //dtBound = objGA.SetAvailabilityResponse(SearchID, CompanyID, Sector, false, true, RQ_Flight, ress, "");
            }
            catch (Exception ex)
            {
                DBCommon.Logger.dbLogg(CompanyID, 0, "GET_FLT_MC", "Availability-UAPI", "", SearchID, ex.Message);
            }

            DBCommon.Logger.WriteLogg(CompanyID, 0, "GET_FLT_MC-UAPI", "PNR", reqq + Environment.NewLine + ress, "", SearchID);
            this.Done = "D";
        }
    }
}
