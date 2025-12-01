using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using System.Configuration;
using Microsoft.Extensions.Logging;

namespace ZealTravel.Infrastructure.CommonComponents
{
    public class FlightDBOperation
    {
        private string logServiceName = ConfigurationManager.AppSettings["LogServiceName"];

        private string logModuleName = "CommonComponents";

        private string logFileName = "FlightDBOperation";

        private string companyId;

        private ILogger _logger;
        IConfiguration _configuration;

        public FlightDBOperation(string CompanyId, ILogger logger)
        {
           // _configuration = configuration;
            companyId = CompanyId;
            _logger = logger;
        }

        public string GetB2CApiKey()
        {
            DBConnection dBConnection = new DBConnection(companyId);
            string result = "";
            //logger = new Logger();
            try
            {
                List<SqlParameter> list = new List<SqlParameter>();
                list.Add(new SqlParameter("@CompanyId", companyId));
                List<SqlParameter> parameters = list;
                result = (string)dBConnection.GetScalarValue(parameters, "USP_GetApiKey_B2C_TCS4", "TRM");
            }
            catch (Exception ex)
            {
                //logger.AddToLog("GetB2CApiKey: Error :  ");
                //logger.AddToLog(ex.Message);
                //logger.AddToLog("stack Trace :  ");
                //logger.AddToLog(ex.StackTrace);
            }
            finally
            {
                //if (logger.stringBuilderLog.Length > 0)
                //{
                //    Logger.WriteTraceInfo(logServiceName, logModuleName, logFileName, logger.stringBuilderLog.ToString(), companyId);
                //}
            }

            return result;
        }

        public List<Supplier> GetFlightActiveSupplier(string channelCode, bool onbehalfBooking, int AgentId, string Origin, string Destination)
        {
            logger = new Logger();
            DBConnection dBConnection = new DBConnection(companyId);
            List<Supplier> result = new List<Supplier>();
            List<SqlParameter> list = new List<SqlParameter>();
            list.Add(new SqlParameter("@CompanyId", companyId));
            list.Add(new SqlParameter("@ChannelCode", channelCode));
            list.Add(new SqlParameter("@OnBehalfBooking", onbehalfBooking));
            list.Add(new SqlParameter("@ClientId", AgentId));
            list.Add(new SqlParameter("@Origin", Origin));
            list.Add(new SqlParameter("@Destination", Destination));
            List<SqlParameter> list2 = list;
            DataTable dataTable = TableFlightActiveSupplier();
            try
            {
                if (dataTable.Rows.Count > 0)
                {
                    result = (from x in dataTable.AsEnumerable()
                              select new Supplier
                              {
                                  SupplierCode = x.Field<string>("Code").ToString(),
                                  SupplierTimeOut = Convert.ToInt32(x.Field<string>("SupplierTimeOut")),
                                  SupplierId = Convert.ToInt32(x.Field<int>("ProductSupplierId")),
                                  CompanyNationality = x.Field<string>("CompanyNationality").ToString()
                              }).ToList();
                }
            }
            catch (Exception ex)
            {
                logger.AddToLog("GetFlightActiveSupplier: Error :  ");
                logger.AddToLog(ex.Message);
                logger.AddToLog("stack Trace :  ");
                logger.AddToLog(ex.StackTrace);
            }
            finally
            {
                if (logger.stringBuilderLog.Length > 0)
                {
                    Logger.WriteTraceInfo(logServiceName, logModuleName, logFileName, logger.stringBuilderLog.ToString(), companyId);
                }
            }

            return result;
        }

        private DataTable TableFlightActiveSupplier()
        {
            logger = new Logger();
            DBConnection dBConnection = new DBConnection(companyId);
            try
            {
                DataSet dataSet = new DataSet();
                List<SqlParameter> list = new List<SqlParameter>();
                list.Add(new SqlParameter("@ProcNo", 3));
                List<SqlParameter> parameters = list;
                dataSet = dBConnection.ExecuteDataSet(parameters, "TI_Airline_Proc", "TRM");
                if (dataSet.Tables.Count > 0 && dataSet.Tables[0].Rows.Count > 0)
                {
                    return dataSet.Tables[0];
                }
            }
            catch (Exception ex)
            {
                logger.AddToLog("TableFlightActiveSupplier: Error :  ");
                logger.AddToLog(ex.Message);
                logger.AddToLog("stack Trace :  ");
                logger.AddToLog(ex.StackTrace);
            }
            finally
            {
                if (logger.stringBuilderLog.Length > 0)
                {
                    Logger.WriteTraceInfo(logServiceName, logModuleName, logFileName, logger.stringBuilderLog.ToString(), companyId);
                }
            }

            return null;
        }

        public string GetStaffPCC(string gdsCode, string staffId)
        {
            string result = "";
            logger = new Logger();
            DBConnection dBConnection = new DBConnection(companyId);
            try
            {
                List<SqlParameter> list = new List<SqlParameter>();
                list.Add(new SqlParameter("@CompanyID", companyId));
                list.Add(new SqlParameter("@GDSID", gdsCode));
                list.Add(new SqlParameter("@StaffId", staffId));
                List<SqlParameter> parameters = list;
                DataTable dataTable = dBConnection.ExecuteDataSet(parameters, "USP_GETSTAFFPCC", "TRM").Tables[0];
                if (dataTable.Rows.Count > 0 && string.IsNullOrEmpty(Convert.ToString(dataTable.Rows[0]["PCCNo"])))
                {
                    result = Convert.ToString(dataTable.Rows[0]["PCCNo"]);
                }
            }
            catch (Exception ex)
            {
                logger.AddToLog("GetStaffPCC: Error :  ");
                logger.AddToLog(ex.Message);
                logger.AddToLog("stack Trace :  ");
                logger.AddToLog(ex.StackTrace);
            }
            finally
            {
                if (logger.stringBuilderLog.Length > 0)
                {
                    Logger.WriteTraceInfo(logServiceName, logModuleName, logFileName, logger.stringBuilderLog.ToString(), companyId);
                }
            }

            return result;
        }

        public Dictionary<string, string> GetSupplierCredential(string channelCode, string agentId, string gdsCode)
        {
            Dictionary<string, string> result = new Dictionary<string, string>();
            logger = new Logger();
            DBConnection dBConnection = new DBConnection(companyId);
            try
            {
                DataSet dataSet = new DataSet();
                List<SqlParameter> list = new List<SqlParameter>();
                list.Add(new SqlParameter("@CompanyId", companyId));
                list.Add(new SqlParameter("@ChannelCode", channelCode));
                list.Add(new SqlParameter("@AgentId", agentId));
                list.Add(new SqlParameter("@GdsCode", gdsCode));
                List<SqlParameter> parameters = list;
                dataSet = dBConnection.ExecuteDataSet(parameters, "USP_GetFlightSupplierCredential_TCS4", "TRM");
                if (dataSet.Tables.Count > 0 && dataSet.Tables[0].Rows.Count > 0)
                {
                    result = (from x in dataSet.Tables[0].AsEnumerable()
                              select new
                              {
                                  ParamKey = x.Field<string>("ParamKey").ToString().ToUpper(),
                                  ParamValue = x.Field<string>("ParamValue").ToString()
                              }).ToDictionary(x => x.ParamKey, x => x.ParamValue);
                }
            }
            catch (Exception ex)
            {
                logger.AddToLog("GetSupplierCredential: Error :  ");
                logger.AddToLog(ex.Message);
                logger.AddToLog("stack Trace :  ");
                logger.AddToLog(ex.StackTrace);
            }
            finally
            {
                if (logger.stringBuilderLog.Length > 0)
                {
                    Logger.WriteTraceInfo(logServiceName, logModuleName, logFileName, logger.stringBuilderLog.ToString(), companyId);
                }
            }

            return result;
        }

        public DataTable GetAirlineBaggages(string gdsCode, string SourceCountry, string DestinationCountry)
        {
            logger = new Logger();
            DBConnection dBConnection = new DBConnection(companyId);
            try
            {
                DataSet dataSet = new DataSet();
                List<SqlParameter> list = new List<SqlParameter>();
                list.Add(new SqlParameter("@SourceCountry", SourceCountry));
                list.Add(new SqlParameter("@DestinationCountry", DestinationCountry));
                list.Add(new SqlParameter("@GdsCode", gdsCode));
                list.Add(new SqlParameter("@QueryNo", 1));
                List<SqlParameter> parameters = list;
                dataSet = dBConnection.ExecuteDataSet(parameters, "AirlineBaggages_proc", "TRM");
                if (dataSet.Tables.Count > 0 && dataSet.Tables[0].Rows.Count > 0)
                {
                    return dataSet.Tables[0];
                }
            }
            catch (Exception ex)
            {
                logger.AddToLog("GetAirlineBaggages: Error :  ");
                logger.AddToLog(ex.Message);
                logger.AddToLog("stack Trace :  ");
                logger.AddToLog(ex.StackTrace);
            }
            finally
            {
                if (logger.stringBuilderLog.Length > 0)
                {
                    Logger.WriteTraceInfo(logServiceName, logModuleName, logFileName, logger.stringBuilderLog.ToString(), companyId);
                }
            }

            return null;
        }

        public void GetFlightFromCache(string guid, string flightIndex, ref string flight, ref string searchRQ, ref string farerule)
        {
            logger = new Logger();
            DBConnection dBConnection = new DBConnection(companyId);
            try
            {
                string[] array = flightIndex.Split('_');
                logger.AddToLog("GetFlightFromCache: flightIndex :: " + flightIndex);
                List<SqlParameter> list = new List<SqlParameter>();
                list.Add(new SqlParameter("@guid", guid));
                list.Add(new SqlParameter("@fidx", array[0]));
                list.Add(new SqlParameter("@companyid", companyId));
                List<SqlParameter> list2 = list;
                if (array.Length > 1)
                {
                    list2.Add(new SqlParameter("@Gds", array[1]));
                }

                DataTable dataTable = dBConnection.ExecuteDataSet(list2, "USP_GetFlightFromCache_TCS4", "TRM").Tables[0];
                if (dataTable.Rows.Count > 0 && Convert.ToString(dataTable.Rows[0]["Flight"]) != "")
                {
                    flight = Convert.ToString(dataTable.Rows[0]["Flight"]);
                    searchRQ = Convert.ToString(dataTable.Rows[0]["SearchRequest"]);
                    farerule = Convert.ToString(dataTable.Rows[0]["FareRule"]);
                }
            }
            catch (Exception ex)
            {
                logger.AddToLog("GetFlightFromCache: Error :  ");
                logger.AddToLog(ex.Message);
                logger.AddToLog("stack Trace :  ");
                logger.AddToLog(ex.StackTrace);
            }
            finally
            {
                if (logger.stringBuilderLog.Length > 0)
                {
                    Logger.WriteTraceInfo(logServiceName, logModuleName, logFileName, logger.stringBuilderLog.ToString(), companyId);
                }
            }
        }

        public void GetFlightFromCache(string guid, string flightIndex, ref string flight, ref string searchRQ)
        {
            logger = new Logger();
            DBConnection dBConnection = new DBConnection(companyId);
            try
            {
                string[] array = flightIndex.Split('_');
                logger.AddToLog("GetFlightFromCache: flightIndex :: " + flightIndex);
                List<SqlParameter> list = new List<SqlParameter>();
                list.Add(new SqlParameter("@guid", guid));
                list.Add(new SqlParameter("@fidx", array[0]));
                list.Add(new SqlParameter("@companyid", companyId));
                List<SqlParameter> list2 = list;
                if (array.Length > 1)
                {
                    list2.Add(new SqlParameter("@Gds", array[1]));
                }

                DataTable dataTable = dBConnection.ExecuteDataSet(list2, "USP_GetFlightFromCache_TCS4", "TRM").Tables[0];
                if (dataTable.Rows.Count > 0 && Convert.ToString(dataTable.Rows[0]["Flight"]) != "")
                {
                    flight = Convert.ToString(dataTable.Rows[0]["Flight"]);
                    searchRQ = Convert.ToString(dataTable.Rows[0]["SearchRequest"]);
                }
            }
            catch (Exception ex)
            {
                logger.AddToLog("GetFlightFromCache: Error :  ");
                logger.AddToLog(ex.Message);
                logger.AddToLog("stack Trace :  ");
                logger.AddToLog(ex.StackTrace);
            }
            finally
            {
                if (logger.stringBuilderLog.Length > 0)
                {
                    Logger.WriteTraceInfo(logServiceName, logModuleName, logFileName, logger.stringBuilderLog.ToString(), companyId);
                }
            }
        }

        public void UpdateFlightAysnc(string guid, string fFlight, int fIdx, string supplierCode, string farerule)
        {
            Task.Run(delegate
            {
                UpdateFlightSearchCache(guid, fFlight, fIdx, supplierCode, farerule);
            });
        }

        public void InsertFlightSearchCacheAysnc(string guid, string searchRQ, Dictionary<string, string> searchResultList, string supplierCode = "")
        {
            Task.Run(delegate
            {
                InsertFlightSearchCache(guid, searchRQ, searchResultList, supplierCode);
            });
        }

        private void UpdateFlightSearchCache(string guid, string fFlight, int fIdx, string supplierCode, string farerule)
        {
            try
            {
                logger.AddToLog("UpdateFlightSearchCache:  fIdx :  " + fIdx + "  supplierCode : " + supplierCode);
                using SqlConnection sqlConnection = new SqlConnection(DBConnection.ConnectionString(companyId, "TRM"));
                using SqlCommand sqlCommand = new SqlCommand();
                sqlCommand.Connection = sqlConnection;
                sqlCommand.Parameters.Clear();
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.CommandText = "USP_UpdateFlightSearchCache_TCS4";
                sqlCommand.Parameters.Add(new SqlParameter("@companyid", companyId));
                sqlCommand.Parameters.Add(new SqlParameter("@guid", guid));
                sqlCommand.Parameters.Add(new SqlParameter("@flight", fFlight));
                sqlCommand.Parameters.Add(new SqlParameter("@fidx", fIdx));
                sqlCommand.Parameters.Add(new SqlParameter("@Gds", supplierCode));
                sqlCommand.Parameters.Add(new SqlParameter("@farerule", farerule));
                sqlConnection.Open();
                sqlCommand.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                logger.AddToLog("UpdateFlightSearchCache: Error :  ");
                logger.AddToLog(ex.Message);
                logger.AddToLog("stack Trace :  ");
                logger.AddToLog(ex.StackTrace);
            }
            finally
            {
                if (logger.stringBuilderLog.Length > 0)
                {
                    Logger.WriteTraceInfo(logServiceName, logModuleName, logFileName, logger.stringBuilderLog.ToString(), companyId);
                }
            }
        }

        private void InsertFlightSearchCache(string guid, string searchRQ, Dictionary<string, string> searchResultList, string supplierCode = "")
        {
            int num = 0;
            try
            {
                using SqlConnection sqlConnection = new SqlConnection(DBConnection.ConnectionString(companyId, "TRM"));
                using (SqlCommand sqlCommand = new SqlCommand())
                {
                    sqlCommand.Connection = sqlConnection;
                    SqlParameter sqlParameter = new SqlParameter("@output", SqlDbType.Int);
                    sqlParameter.Direction = ParameterDirection.Output;
                    SqlParameter sqlParameter2 = sqlParameter;
                    sqlCommand.Parameters.Clear();
                    sqlCommand.CommandType = CommandType.StoredProcedure;
                    sqlCommand.CommandText = "USP_InsertFlightSearchCache_TCS4";
                    sqlCommand.Parameters.Add(new SqlParameter("@companyid", companyId));
                    sqlCommand.Parameters.Add(new SqlParameter("@guid", guid));
                    sqlCommand.Parameters.Add(new SqlParameter("@searchrequest", searchRQ));
                    sqlCommand.Parameters.Add(new SqlParameter("@createdby", SqlDbType.BigInt));
                    sqlCommand.Parameters.Add(new SqlParameter("@SupplierCode", supplierCode));
                    sqlCommand.Parameters.Add(sqlParameter2);
                    sqlConnection.Open();
                    sqlCommand.ExecuteNonQuery();
                    num = Convert.ToInt32(sqlParameter2.Value);
                }

                if (num == 0)
                {
                    return;
                }

                DataTable dataTable = new DataTable();
                dataTable.Columns.AddRange(new DataColumn[4]
                {
                new DataColumn("CacheId", typeof(int)),
                new DataColumn("FIdx", typeof(int)),
                new DataColumn("Gds", typeof(string)),
                new DataColumn("Flight", typeof(string))
                });
                foreach (KeyValuePair<string, string> searchResult in searchResultList)
                {
                    string[] array = searchResult.Key.Split('_');
                    dataTable.Rows.Add(num, array[0], array[1], searchResult.Value);
                }

                using SqlBulkCopy sqlBulkCopy = new SqlBulkCopy(sqlConnection);
                sqlBulkCopy.DestinationTableName = "flight_search_cache";
                sqlBulkCopy.ColumnMappings.Add("CacheId", "CacheId");
                sqlBulkCopy.ColumnMappings.Add("FIdx", "FIdx");
                sqlBulkCopy.ColumnMappings.Add("Gds", "Gds");
                sqlBulkCopy.ColumnMappings.Add("Flight", "Flight");
                sqlBulkCopy.WriteToServer(dataTable);
            }
            catch (Exception ex)
            {
                logger.AddToLog("InsertFlightSearchCache: Error :  ");
                logger.AddToLog(ex.Message);
                logger.AddToLog("stack Trace :  ");
                logger.AddToLog(ex.StackTrace);
            }
            finally
            {
                if (logger.stringBuilderLog.Length > 0)
                {
                    Logger.WriteTraceInfo(logServiceName, logModuleName, logFileName, logger.stringBuilderLog.ToString(), companyId);
                }
            }
        }

        public SSRList GetFlightSSRList(string gds, string ssrType, string LanguageCode = "en")
        {
            logger = new Logger();
            DBConnection dBConnection = new DBConnection("");
            DataTable dataTable = null;
            SSRList result = null;
            try
            {
                List<SqlParameter> list = new List<SqlParameter>();
                list.Add(new SqlParameter("@GDS_CODE", gds));
                list.Add(new SqlParameter("@TYPE", ssrType));
                list.Add(new SqlParameter("@LanguageCode", LanguageCode));
                List<SqlParameter> list2 = list;
                dataTable = TableFlightSSRList();
                SSRList sSRList = new SSRList();
                sSRList.FlightSSR = (from x in dataTable.AsEnumerable()
                                     select new SSRListFlightSSR
                                     {
                                         SSRCode = x.Field<string>("SSRCODE").ToString(),
                                         SSRName = x.Field<string>("SSRNAME").ToString(),
                                         SSRType = x.Field<string>("SSRTYPE").ToString(),
                                         SSRGds = x.Field<string>("GDS").ToString()
                                     }).ToList();
                result = sSRList;
            }
            catch (Exception ex)
            {
                logger.AddToLog("GetFlightSSRList: Error :  ");
                logger.AddToLog(ex.Message);
                logger.AddToLog("stack Trace :  ");
                logger.AddToLog(ex.StackTrace);
            }
            finally
            {
                if (logger.stringBuilderLog.Length > 0)
                {
                    Logger.WriteTraceInfo(logServiceName, logModuleName, logFileName, logger.stringBuilderLog.ToString(), "TI");
                }
            }

            return result;
        }

        private DataTable TableFlightSSRList()
        {
            DataTable dataTable = new DataTable();
            dataTable.Clear();
            dataTable.Columns.Add("SSRCODE");
            dataTable.Columns.Add("SSRName");
            dataTable.Columns.Add("Type");
            dataTable.Columns.Add("SSRTYPE");
            dataTable.Columns.Add("GDS");
            DataSet dataSet = new DataSet();
            dataSet.ReadXml("D:\\TI\\SuportXML\\FlightSSRList.xml");
            dataTable.Merge(dataSet.Tables[0]);
            return dataTable;
        }

        public AirportList GetAirportList(string LanguageCode = "en")
        {
            logger = new Logger();
            CompanyDBSection companyDBSection = (CompanyDBSection)ConfigurationManager.GetSection("CompanyDBSection");
            if (companyDBSection != null)
            {
                CompanyDB companyDB = (from CompanyDB s in companyDBSection.CompanyDB
                                       where !string.IsNullOrEmpty(s.companyid)
                                       select s).FirstOrDefault();
                if (companyDB != null && !string.IsNullOrEmpty(companyDB.companyid))
                {
                    companyId = companyDB.companyid;
                }
            }

            logger.AddToLog("GetAirportList: companyId :" + companyId);
            DBConnection dBConnection = new DBConnection(companyId);
            DataTable dataTable = new DataTable();
            AirportList airportList = new AirportList();
            int num = 0;
            try
            {
                List<SqlParameter> list = new List<SqlParameter>();
                list.Add(new SqlParameter("@langCode", LanguageCode));
                dataTable = TableGetAirportList();
                AirportList airportList2 = new AirportList();
                airportList2.Airport = (from x in dataTable.AsEnumerable()
                                        select new AirportData
                                        {
                                            name = x.Field<string>("CityName_EN").ToString() + ", " + x.Field<string>("CountryName_EN").ToString() + " - " + x.Field<string>("AirprtName_EN").ToString() + "(" + x.Field<string>("AirportCode").ToString() + ")",
                                            displayname = x.Field<string>("CityName").ToString() + ", " + x.Field<string>("CountryName").ToString() + " - " + x.Field<string>("AirportName").ToString() + "(" + x.Field<string>("AirportCode").ToString() + ")",
                                            airportcode = x.Field<string>("AirportCode").ToString(),
                                            cityname = x.Field<string>("CityName").ToString(),
                                            countryname = x.Field<string>("CountryName_EN").ToString(),
                                            citycode = x.Field<string>("CityCode").ToString(),
                                            countrycode = x.Field<string>("CountryCode").ToString(),
                                            type = "Airport"
                                        }).ToList();
                airportList = airportList2;
                airportList.Airport.Select(delegate (AirportData myObject)
                {
                    myObject.name = myObject.name.Replace("()", "");
                    myObject.displayname = myObject.displayname.Replace("()", "");
                    return myObject;
                }).ToList();
                airportList.Airport.Select(delegate (AirportData myObject)
                {
                    myObject.type = ((myObject.airportcode == "") ? "City" : "Airport");
                    return myObject;
                }).ToList();
            }
            catch (Exception ex)
            {
                logger.AddToLog("GetAirportList: Error :");
                logger.AddToLog(ex.Message);
                logger.AddToLog("stack trace :");
                logger.AddToLog(ex.StackTrace);
                airportList = null;
            }
            finally
            {
                if (logger.stringBuilderLog.Length > 0)
                {
                    Logger.WriteTraceInfo(logServiceName, logModuleName, logFileName, logger.stringBuilderLog.ToString(), companyId);
                }
            }

            return airportList;
        }

        public DataTable TableGetAirportList()
        {
            DBConnection dBConnection = new DBConnection(companyId);
            List<SqlParameter> list = new List<SqlParameter>();
            list.Add(new SqlParameter("@ProcNo", 2));
            List<SqlParameter> parameters = list;
            DataSet dataSet = dBConnection.ExecuteDataSet(parameters, "TI_Airline_Proc", "TRM");
            return dataSet.Tables[0];
        }

        public AirlineList GetAirlineList()
        {
            logger = new Logger();
            DBConnection dBConnection = new DBConnection("");
            DataTable dataTable = new DataTable();
            AirlineList result = new AirlineList();
            try
            {
                List<SqlParameter> list = new List<SqlParameter>();
                dataTable = TableAirlineList();
                AirlineList airlineList = new AirlineList();
                airlineList.Airline = (from x in dataTable.AsEnumerable()
                                       select new AirlineData
                                       {
                                           AirlineCode = x.Field<string>("AirlineCode").ToString(),
                                           AirlineName = x.Field<string>("AirlineName").ToString(),
                                           AirlineLogo = x.Field<string>("AirlineLogo").ToString(),
                                           AirlineLongName = ((Convert.ToString(x.Field<string>("LongAirlineName")) != "") ? Convert.ToString(x.Field<string>("LongAirlineName")) : Convert.ToString(x.Field<string>("AirlineName")))
                                       }).ToList();
                result = airlineList;
            }
            catch (Exception ex)
            {
                logger.AddToLog("GetAirlineList: Error :");
                logger.AddToLog(ex.Message);
                logger.AddToLog("stack trace :");
                logger.AddToLog(ex.StackTrace);
                result = null;
            }
            finally
            {
                if (logger.stringBuilderLog.Length > 0)
                {
                    Logger.WriteTraceInfo(logServiceName, logModuleName, logFileName, logger.stringBuilderLog.ToString(), "TI");
                }
            }

            return result;
        }

        private DataTable TableAirlineList()
        {
            DataTable dataTable = new DataTable();
            dataTable.Clear();
            dataTable.Columns.Add("AirlineCode");
            dataTable.Columns.Add("AirlineName");
            dataTable.Columns.Add("AirlineLogo");
            dataTable.Columns.Add("LongAirlineName");
            DataSet dataSet = new DataSet();
            dataSet.ReadXml("D:\\TI\\SuportXML\\AirlineList.xml");
            dataTable.Merge(dataSet.Tables[0]);
            return dataTable;
        }

        public decimal RoeFromBaseCurrencyToExchangeCurrency(string exchangeCurrencyCode)
        {
            decimal result = default(decimal);
            try
            {
                DBConnection dBConnection = new DBConnection(companyId);
                List<SqlParameter> list = new List<SqlParameter>();
                list.Add(new SqlParameter("@CompanyId", companyId));
                list.Add(new SqlParameter("@Currency", exchangeCurrencyCode));
                list.Add(new SqlParameter("@QueryType", 1));
                List<SqlParameter> parameters = list;
                DataSet dataSet = dBConnection.ExecuteDataSet(parameters, "Fsp_GetConversionRate", "TRM");
                if (dataSet != null && dataSet.Tables.Count > 0 && dataSet.Tables[0].Rows.Count > 0)
                {
                    result = Convert.ToDecimal(dataSet.Tables[0].Rows[0]["Conversion"]);
                    return result;
                }
            }
            catch (Exception ex)
            {
                logger.AddToLog("RoeFromBaseCurrencyToExchangeCurrency: Error :  ");
                logger.AddToLog(ex.Message);
                logger.AddToLog("RoeFromBaseCurrencyToExchangeCurrency: stack Trace :  ");
                logger.AddToLog(ex.StackTrace);
                result = default(decimal);
            }
            finally
            {
                if (logger.stringBuilderLog.Length > 0)
                {
                    Logger.WriteTraceInfo(logServiceName, logModuleName, logFileName, logger.stringBuilderLog.ToString(), companyId);
                }
            }

            return result;
        }

        public DataTable GetBlackOutGDSAirline(string companyId, string nationality, string origin, string destination, int foreignResidence)
        {
            logger = new Logger();
            logger.AddToLog("GetBlackOutGDSAirline: start : Nationality : " + nationality + "  and Origin : " + origin + "  and Destination : " + destination + "  and ForaignResidance : " + foreignResidence);
            DBConnection dBConnection = new DBConnection(companyId);
            DataTable dataTable = new DataTable();
            try
            {
                List<SqlParameter> list = new List<SqlParameter>();
                list.Add(new SqlParameter("@CompanyId", companyId));
                list.Add(new SqlParameter("@Nationality", nationality));
                list.Add(new SqlParameter("@Origin", origin));
                list.Add(new SqlParameter("@Destination", destination));
                list.Add(new SqlParameter("@ForaignResidency", foreignResidence));
                list.Add(new SqlParameter("@queryType", "GETRULE"));
                return dBConnection.ExecuteDataSet(list, "spGetAirlineBlackoutRuleDetails", "TRM").Tables[0];
            }
            catch (Exception ex)
            {
                logger.AddToLog("GetBlackOutGDSAirline: Error :  ");
                logger.AddToLog(ex.StackTrace);
                return null;
            }
        }

        public string GetB2BApiKey(string channel, int subAgentId)
        {
            DBConnection dBConnection = new DBConnection(companyId);
            string result = "";
            logger = new Logger();
            try
            {
                List<SqlParameter> list = new List<SqlParameter>();
                list.Add(new SqlParameter("@CompanyId", companyId));
                list.Add(new SqlParameter("@Channel", channel));
                list.Add(new SqlParameter("@SubAgentId", subAgentId));
                List<SqlParameter> parameters = list;
                result = (string)dBConnection.GetScalarValue(parameters, "USP_GetApiKey_B2C_TCS4", "TRM");
            }
            catch (Exception ex)
            {
                logger.AddToLog("GetB2CApiKey: Error :  ");
                logger.AddToLog(ex.Message);
                logger.AddToLog("stack Trace :  ");
                logger.AddToLog(ex.StackTrace);
            }
            finally
            {
                if (logger.stringBuilderLog.Length > 0)
                {
                    Logger.WriteTraceInfo(logServiceName, logModuleName, logFileName, logger.stringBuilderLog.ToString(), companyId);
                }
            }

            return result;
        }

        public bool getQueueSetting(string companyId)
        {
            bool result = false;
            logger = new Logger();
            DBConnection dBConnection = new DBConnection(companyId);
            try
            {
                List<SqlParameter> list = new List<SqlParameter>();
                list.Add(new SqlParameter("@CompanyId", companyId));
                List<SqlParameter> parameters = list;
                DataTable dataTable = dBConnection.ExecuteDataSet(parameters, "fsp_GetCompanySystemSettings", "TRM").Tables[0];
                if (dataTable.Rows.Count > 0)
                {
                    result = Convert.ToBoolean(dataTable.Rows[0]["isQueue"]);
                }
            }
            catch (Exception ex)
            {
                logger.AddToLog("getQueueSetting: Error :  " + ex.Message);
                logger.AddToLog("stack Trace : " + ex.StackTrace);
            }
            finally
            {
                if (logger.stringBuilderLog.Length > 0)
                {
                    Logger.WriteTraceInfo(logServiceName, logModuleName, logFileName, logger.stringBuilderLog.ToString(), companyId);
                }
            }

            return result;
        }

        public string GetQueueNumber(string companyId, string gdsCode, string queueType)
        {
            DBConnection dBConnection = new DBConnection(companyId);
            string result = "0";
            logger = new Logger();
            try
            {
                List<SqlParameter> list = new List<SqlParameter>();
                list.Add(new SqlParameter("@CompanyID", companyId));
                list.Add(new SqlParameter("@GDSID", gdsCode));
                list.Add(new SqlParameter("@QueueType", queueType));
                List<SqlParameter> parameters = list;
                DataTable dataTable = dBConnection.ExecuteDataSet(parameters, "usp_QueueManagementGet", "TRM").Tables[0];
                if (dataTable.Rows.Count > 0)
                {
                    result = Convert.ToString(dataTable.Rows[0]["QueueNumber"]);
                }
            }
            catch (Exception ex)
            {
                logger.AddToLog("GetQueueNumber: Error : " + ex.Message);
                logger.AddToLog("stack Trace : " + ex.StackTrace);
            }
            finally
            {
                if (logger.stringBuilderLog.Length > 0)
                {
                    Logger.WriteTraceInfo(logServiceName, logModuleName, logFileName, logger.stringBuilderLog.ToString(), companyId);
                }
            }

            return result;
        }
    }
}
