using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TIRequestResponse;
using CommonComponents;
using ZealTravel.Domain.Interfaces.Akasha;


namespace ZealTravel.Infrastructure.Akaasha
{
    public class TI_Search:ITI_Search
    {
        ITI_DBData _dbData;
        public TI_Search(ITI_DBData dbData) { _dbData = dbData; }
        public string GetFlightSearchRequest(string AirRQ, string SearchType, string SearchID, string SupplierCode, ref string Sector)
        {
            string jsonSearchRQ = "";
            Sector = "D";   // by initially it is Domestric, in execcuting below code if country code not  equesl IN it will assign as Internatonal
            try
            {
                int dbSupplierId = 534;
                string strEmail = "api.traveldestini@gmail.com";
                string strCompany = "Alpha Travels";
                string strMobile = "7827039280";

                DataSet ResponseDs = new DataSet();
                ResponseDs.ReadXml(new System.IO.StringReader(AirRQ));                       

                SearchRQ searchRQ = new SearchRQ();

                string companyId = "FTI";
                string channel = "B2B";

                string Cabin = ResponseDs.Tables["AvailabilityRequest"].Rows[0]["Cabin"].ToString();
                Cabin = Cabin == "Y" ? "EC" : Cabin == "A" ? "PE" : Cabin == "C" ? "BU" : Cabin == "B" ? "FR" : "EC";

                string JourneyType = "DOM";

                byte Adults = Convert.ToByte(ResponseDs.Tables["AvailabilityRequest"].Rows[0]["Adult"].ToString());
                byte Childs = Convert.ToByte(ResponseDs.Tables["AvailabilityRequest"].Rows[0]["Child"].ToString());
                byte Infants = Convert.ToByte(ResponseDs.Tables["AvailabilityRequest"].Rows[0]["Infant"].ToString());

                if (SearchType.Equals("MC"))
                {
                    List<Search> ListSearch = new List<Search>();
                    foreach (DataRow dr in ResponseDs.Tables["AirSrchInfo"].Rows)
                    {
                        //TI_dbData objdb = new TI_dbData();
                        var dtAirline = _dbData.GetAirlineData(dr["DepartureStation"].ToString(), dr["ArrivalStation"].ToString());

                        foreach (var drr in dtAirline)
                        {
                            if (drr.CountryCode.ToString().IndexOf("IN") == -1)
                            {
                                Sector = "I";
                                JourneyType = "INT";
                                break;
                            }
                        }

                        Search objSearch = new Search();
                        objSearch.DepartAP = dr["DepartureStation"].ToString();
                        objSearch.ArrivalAP = dr["ArrivalStation"].ToString();
                        objSearch.DepartTime = "00000000";
                        objSearch.DepartDate = TI_dbData.DateChangeForSearch(dr["StartDate"].ToString());

                        var row = dtAirline.FirstOrDefault(x=>x.CityCode==dr["DepartureStation"].ToString());
                        if (row!=null)
                        {
                            objSearch.DepartCFN = row.CityName.ToString();
                            objSearch.DepartSelected = row.Description.ToString();
                        }

                        row = dtAirline.FirstOrDefault(x=>x.CityCode== dr["ArrivalStation"].ToString());
                        if (row!=null)
                        {
                            objSearch.ArrivalCFN = row.CityName.ToString();
                            objSearch.ArrivalSelected = row.Description.ToString();
                        }

                        ListSearch.Add(objSearch);
                    }
                    searchRQ.Search = ListSearch.ToArray();
                }
                else if (SearchType.Equals("RT"))
                {
                    List<Search> ListSearch = new List<Search>();

                    //TI_dbData objdb = new TI_dbData();
                    var dtAirline = _dbData.GetAirlineData(ResponseDs.Tables["AvailabilityRequest"].Rows[0]["DepartureStation"].ToString(), ResponseDs.Tables["AvailabilityRequest"].Rows[0]["ArrivalStation"].ToString());

                    foreach (var drr in dtAirline)
                    {
                        if (drr.CountryCode.ToString().IndexOf("IN") == -1)
                        {
                            Sector = "I";
                            JourneyType = "INT";
                            break;
                        }
                    }

                    Search objSearch = new Search();
                    objSearch.DepartAP = ResponseDs.Tables["AvailabilityRequest"].Rows[0]["DepartureStation"].ToString();
                    objSearch.ArrivalAP = ResponseDs.Tables["AvailabilityRequest"].Rows[0]["ArrivalStation"].ToString();
                    objSearch.DepartTime = "00000000";
                    objSearch.DepartDate = TI_dbData.DateChangeForSearch(ResponseDs.Tables["AvailabilityRequest"].Rows[0]["StartDate"].ToString());

                    var row = dtAirline.FirstOrDefault(x=>x.CityCode==ResponseDs.Tables["AvailabilityRequest"].Rows[0]["DepartureStation"].ToString() );
                    if (row!=null)
                    {
                        objSearch.DepartCFN = row.CityName.ToString();
                        objSearch.DepartSelected = row.Description.ToString();
                    }

                    row = dtAirline.FirstOrDefault(x=>x.CityCode== ResponseDs.Tables["AvailabilityRequest"].Rows[0]["ArrivalStation"].ToString());
                    if (row != null)
                    {
                        objSearch.ArrivalCFN = row.CityName.ToString();
                        objSearch.ArrivalSelected = row.Description.ToString();
                    }

                    ListSearch.Add(objSearch);

                    Search objSearch2 = new Search();
                    objSearch2.DepartAP = ResponseDs.Tables["AvailabilityRequest"].Rows[0]["ArrivalStation"].ToString();
                    objSearch2.ArrivalAP = ResponseDs.Tables["AvailabilityRequest"].Rows[0]["DepartureStation"].ToString();
                    objSearch2.DepartTime = "00000000";
                    objSearch2.DepartDate = TI_dbData.DateChangeForSearch(ResponseDs.Tables["AvailabilityRequest"].Rows[0]["EndDate"].ToString());

                    row = dtAirline.FirstOrDefault(x => x.CityCode == ResponseDs.Tables["AvailabilityRequest"].Rows[0]["ArrivalStation"].ToString());
                    if (row != null)
                    {
                        objSearch2.DepartCFN = row.CityName.ToString();
                        objSearch2.DepartSelected = row.CityName.ToString();
                    }

                    row = dtAirline.FirstOrDefault(x => x.CityCode == ResponseDs.Tables["AvailabilityRequest"].Rows[0]["DepartureStation"].ToString());
                    if (row != null)
                    {
                        objSearch2.ArrivalCFN = row.CityName.ToString();
                        objSearch2.ArrivalSelected = row.Description.ToString();
                    }

                    ListSearch.Add(objSearch2);

                    searchRQ.Search = ListSearch.ToArray();
                }
                else if (SearchType.Equals("RW"))
                {
                    List<Search> ListSearch = new List<Search>();

                    //TI_dbData objdb = new TI_dbData();
                    var dtAirline = _dbData.GetAirlineData(ResponseDs.Tables["AvailabilityRequest"].Rows[0]["DepartureStation"].ToString(), ResponseDs.Tables["AvailabilityRequest"].Rows[0]["ArrivalStation"].ToString());

                    foreach (var drr in dtAirline)
                    {
                        if (drr.CountryCode.ToString().IndexOf("IN") == -1)
                        {
                            Sector = "I";
                            JourneyType = "INT";
                            break;
                        }
                    }

                    Search objSearch = new Search();
                    objSearch.DepartAP = ResponseDs.Tables["AvailabilityRequest"].Rows[0]["DepartureStation"].ToString();
                    objSearch.ArrivalAP = ResponseDs.Tables["AvailabilityRequest"].Rows[0]["ArrivalStation"].ToString();
                    objSearch.DepartTime = "00000000";
                    objSearch.DepartDate = TI_dbData.DateChangeForSearch(ResponseDs.Tables["AvailabilityRequest"].Rows[0]["StartDate"].ToString());

                    var row = dtAirline.FirstOrDefault(x=>x.CityCode==ResponseDs.Tables["AvailabilityRequest"].Rows[0]["DepartureStation"].ToString() );
                    if (row != null)
                    {
                        objSearch.DepartCFN = row.CityName.ToString();
                        objSearch.DepartSelected = row.CityName.ToString();
                    }

                    row = dtAirline.FirstOrDefault(x=>x.CityCode== ResponseDs.Tables["AvailabilityRequest"].Rows[0]["ArrivalStation"].ToString() );
                    if (row!=null)
                    {
                        objSearch.ArrivalCFN = row.CityName.ToString();
                        objSearch.ArrivalSelected = row.CityName.ToString();
                    }

                    ListSearch.Add(objSearch);

                    Search objSearch2 = new Search();
                    objSearch2.DepartAP = ResponseDs.Tables["AvailabilityRequest"].Rows[0]["ArrivalStation"].ToString();
                    objSearch2.ArrivalAP = ResponseDs.Tables["AvailabilityRequest"].Rows[0]["DepartureStation"].ToString();
                    objSearch2.DepartTime = "00000000";
                    objSearch2.DepartDate = TI_dbData.DateChangeForSearch(ResponseDs.Tables["AvailabilityRequest"].Rows[0]["EndDate"].ToString());

                    row = dtAirline.FirstOrDefault(x=>x.CityCode== ResponseDs.Tables["AvailabilityRequest"].Rows[0]["ArrivalStation"].ToString());
                    if (row != null)
                    {
                        objSearch2.DepartCFN = row.CityName.ToString();
                        objSearch2.DepartSelected = row.CityName.ToString();
                    }

                    row = dtAirline.FirstOrDefault(x => x.CityCode == ResponseDs.Tables["AvailabilityRequest"].Rows[0]["DepartureStation"].ToString());
                    if (row!=null)
                    {
                        objSearch2.ArrivalCFN = row.CityName.ToString();
                        objSearch2.ArrivalSelected = row.CityName.ToString();
                    }

                    ListSearch.Add(objSearch2);

                    searchRQ.Search = ListSearch.ToArray();
                }
                else
                {
                    //TI_dbData objdb = new TI_dbData();
                    var dtAirline = _dbData.GetAirlineData(ResponseDs.Tables["AvailabilityRequest"].Rows[0]["DepartureStation"].ToString(), ResponseDs.Tables["AvailabilityRequest"].Rows[0]["ArrivalStation"].ToString());

                    foreach (var drr in dtAirline)
                    {
                        if (drr.CountryCode.ToString().IndexOf("IN") == -1)
                        {
                            Sector = "I";
                            JourneyType = "INT";
                            break;
                        }
                    }

                    Search objSearch = new Search();
                    objSearch.DepartAP = ResponseDs.Tables["AvailabilityRequest"].Rows[0]["DepartureStation"].ToString();
                    objSearch.ArrivalAP = ResponseDs.Tables["AvailabilityRequest"].Rows[0]["ArrivalStation"].ToString();
                    objSearch.DepartTime = "00000000";
                    objSearch.DepartDate = TI_dbData.DateChangeForSearch(ResponseDs.Tables["AvailabilityRequest"].Rows[0]["StartDate"].ToString());

                    var row = dtAirline.FirstOrDefault(x => x.CityCode == ResponseDs.Tables["AvailabilityRequest"].Rows[0]["DepartureStation"].ToString());
                    if (row != null)
                    {
                        objSearch.DepartCFN = row.CityName.ToString();
                        objSearch.DepartSelected = row.Description.ToString();
                    }

                    row = dtAirline.FirstOrDefault(x => x.CityCode == ResponseDs.Tables["AvailabilityRequest"].Rows[0]["ArrivalStation"].ToString());
                    if (row != null)
                    {
                        objSearch.ArrivalCFN = row.CityName.ToString();
                        objSearch.ArrivalSelected = row.CityName.ToString();
                    }

                    List<Search> ListSearch = new List<Search>();
                    ListSearch.Add(objSearch);

                    searchRQ.Search = ListSearch.ToArray();
                }

                searchRQ.Authentication = new Authentication()
                {
                    URL = "",
                    UrlAuthentication = ""
                };

                searchRQ.CompanySetting = new CompanySetting()
                {
                    FlexiSearchAllowed = false,
                    MultiCityAllowed = true,
                    PageMarkUpEnable = false,
                    AirFlowOptionAllowed = "OTA",
                    CompanyCountryCode = "IN",
                    BrowserAgent = "Mozila 5.0/Windows 7",
                    CountryName = "India",
                    Email = strEmail,
                    IPAddress = "0.0.0.0",
                    Name = strCompany,
                    PhoneNo = strMobile
                };

                searchRQ.GeneralInfo = new GeneralInfo()
                {
                    GUID = SearchID,
                    Currency = "INR",
                    Agent_Curr = "INR",
                    Gross_Curr = "INR",
                    Cabin = Cabin,
                    TripType = SearchType == "OW" ? "OW" : "RT",
                    JourneyType = JourneyType,
                    CompanyId = companyId,
                    Channel = channel, //"B2B",
                    AirFlowOpt = "OTA",
                    IsApiUser = false,
                    Nationality = "IN",
                    LanguageCode = "en",
                    Suppliers = SupplierCode,
                    SupplierId = dbSupplierId,
                    CombinationType = "IN",
                    DecPrefrences = 2,
                    TravelerId = 0,
                    IsRW = SearchType == "RW" ? true : false,
                    SubAgent = new SubAgent()
                    {
                        Id = 0,
                        UserId = 0,
                        BranchId = 0,
                        SaBranchId = 0
                    }
                };

                searchRQ.PaxDetails = new PaxDetails()
                {
                    Adults = Adults,
                    Childs = Childs,
                    Infant = Infants
                };

                searchRQ.AdvanceSearch = new AdvanceSearch()
                {
                    NoOfResult = 0,
                    isFlexi = false,
                    isFareBreakup = false,
                    FilterInResult = true,
                    DirectFlight = false,
                    ForeignResidency = false,
                    SpecificAirlines = new SpecificAirlines()
                    {
                        Airline = new string[0]
                    }
                };

                searchRQ.SplitInfo = new SplitInfo()
                {
                    FlightId = 0,
                    IsDivideImport = false,
                    SplitStatus = false,
                    SplitFrom = ""
                };

                List<TIRequestResponse.GDS> listOfGds = new List<TIRequestResponse.GDS>();
                List<string> listOfMA = new List<string>();
                string excludeGds = string.Empty;
                string includeGds = string.Empty;

                var nationality = searchRQ.GeneralInfo.Nationality;
                var origin = searchRQ.Search.FirstOrDefault().DepartAP;
                var destination = searchRQ.Search.FirstOrDefault().ArrivalAP;
                var foreignResidance = !string.IsNullOrEmpty(searchRQ.GeneralInfo.Channel) && string.Equals(searchRQ.GeneralInfo.Channel, "true", StringComparison.OrdinalIgnoreCase) ? 1 : 0;
                DataTable dtBlackOutCriteria = TI_dbData.GetBlackOutGDSAirline();// (companyId, nationality, origin, destination, foreignResidance);

                string includeAirlines = string.Empty;
                string excludeAirlines = string.Empty;
                if (dtBlackOutCriteria.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtBlackOutCriteria.Rows)
                    {
                        if (!string.IsNullOrEmpty(dr["ExcludeGDS"].ToString()) && string.IsNullOrEmpty(excludeGds))
                            excludeGds = dr["ExcludeGDS"].ToString().TrimEnd(',');
                        if (!string.IsNullOrEmpty(dr["IncludeGDS"].ToString()) && string.IsNullOrEmpty(includeGds))
                            includeGds = dr["IncludeGDS"].ToString().TrimEnd(',');
                        GDS gds = new GDS() { Code = dr["GDS"].ToString() };
                        includeAirlines = dr["IncludeAirline"].ToString().TrimEnd(',');
                        excludeAirlines = dr["ExcludeAirline"].ToString().TrimEnd(',');

                        if (!string.IsNullOrEmpty(includeAirlines))
                        {
                            includeAirlines.Split(',').All(x => { listOfMA.Add(x); return true; });
                            gds.IncludeAirline = new IncludeAirline() { MA = listOfMA.ToArray() };
                            listOfMA.Clear();
                        }

                        if (!string.IsNullOrEmpty(excludeAirlines))
                        {
                            excludeAirlines.Split(',').All(x => { listOfMA.Add(x); return true; });
                            gds.ExcludeAirline = new ExcludeAirline() { MA = listOfMA.ToArray() };
                            listOfMA.Clear();
                        }
                        listOfGds.Add(gds);
                    }
                    searchRQ.IncludeGDS = new IncludeGDS() { GDS = listOfGds.ToArray() };
                }

                searchRQ.ShowPropertyWhileSerialize = true;
                dynamic collectionWrapper = new { SearchRQ = searchRQ };
                jsonSearchRQ = SerializeDeserialize.SerializeInJsonString(collectionWrapper, true);
            }
            catch (Exception ex)
            {
                DBCommon.Logger.dbLogg("", 0, "GetFlightSearchRequest-TI_Search", jsonSearchRQ, AirRQ, SearchID, ex.Message);
            }
            finally
            {

            }
            return jsonSearchRQ;
        }
    }
}
