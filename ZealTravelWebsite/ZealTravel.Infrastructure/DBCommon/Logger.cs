using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.IO;
using System.Data.SqlClient;
using System.Data;
using System.Web;
using ZealTravel.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;

namespace ZealTravel.Infrastructure.DBCommon
{
    public class Logger 
    {
        public static bool dbSearchLogg(string CompanyID, Int32 BookingRef, string StaffID, string SearchID, string Location, string Status, string Place, string Remark, string Remark2, string Host)
        {
            bool bStatus = false;
            try
            {
                var db = DatabaseContextFactory.CreateDbContext();
                string sql = @"EXECUTE LoggerSearch_Proc
                               @ProcNo = {0},
                               @CompanyID = {1},
                               @BookingRef = {2},
                               @StaffID = {3},
                               @SearchID = {4},
                               @Location = {5},
                               @Status = {6},
                               @Place = {7},
                               @Remark = {8},
                               @Remark2 = {9},
                               @Host = {10}";

                var rowsAffected = db.Database.ExecuteSqlRawAsync(
                    sql,
                    1, // ProcNo
                    CompanyID,
                    BookingRef,
                    StaffID,
                    SearchID,
                    Location,
                    Status,
                    Place,
                    Remark,
                    Remark2,
                    Host);

                if (rowsAffected.Result > 0)
                {
                    bStatus = true;
                }
            }
            catch (Exception ex)
            {
                dbLogg(CompanyID, BookingRef, Location + "-" + Place, Status, Remark, SearchID, ex.Message);
            }
            return bStatus;
        }
        public static bool dbLogg(string CompanyID, Int32 BookingRef, string MethodName, string Location, string SearchCriteria, string SearchID, string ErrorMessage)
        {
            bool Status = false;
            try
            {
                var db = DatabaseContextFactory.CreateDbContext();
                string sql = @"EXECUTE Logger_Proc
                               @ProcNo = {0},
                               @CompanyID = {1},
                               @BookingRef = {2},
                               @MethodName = {3},
                               @Location = {4},
                               @SearchCriteria = {5},
                               @ErrorMessage = {6},
                               @SearchID = {7},
                               @Host = {8}";

                var rowsAffected = db.Database.ExecuteSqlRawAsync(
                    sql,
                    1, // ProcNo
                    CompanyID,
                    BookingRef,
                    MethodName,
                    Location,
                    SearchCriteria,
                    ErrorMessage,
                    SearchID,
                    getHOST());

                if (rowsAffected.Result > 0)
                {
                    Status = true;
                }
            }
            catch (Exception ex)
            {
                WriteLogg(CompanyID, BookingRef, MethodName, Location, ex.Message, SearchCriteria, SearchID);
            }
            return Status;
        }
        public static bool dbLoggHotelAPI(string SearchID, string CompanyID, Int32 BookingRef, string Conn, string SearchCriteria, string MethodName, string Request, string Response, string PassengerRequest)
        {
            bool Status = false;
            try
            {
                return true;
                //using (SqlConnection connection = new SqlConnection(ConnectionString.dbConnect))
                //{
                //    SqlCommand cmd = new SqlCommand("LoggerHotelAPI_Proc", connection);
                //    cmd.CommandType = CommandType.StoredProcedure;
                //    cmd.CommandTimeout = 10;

                //    cmd.Parameters.Add(@"ProcNo", SqlDbType.Int).Value = 1;
                //    cmd.Parameters.Add(@"CompanyID", SqlDbType.VarChar).Value = CompanyID;
                //    cmd.Parameters.Add(@"BookingRef", SqlDbType.Int).Value = BookingRef;
                //    cmd.Parameters.Add(@"SearchID", SqlDbType.VarChar).Value = SearchID;
                //    cmd.Parameters.Add(@"SearchCriteria", SqlDbType.VarChar).Value = SearchCriteria;
                //    cmd.Parameters.Add(@"MethodName", SqlDbType.VarChar).Value = MethodName;
                //    cmd.Parameters.Add(@"Request", SqlDbType.VarChar).Value = Request;
                //    cmd.Parameters.Add(@"Response", SqlDbType.VarChar).Value = Response;
                //    cmd.Parameters.Add(@"PassengerRequest", SqlDbType.VarChar).Value = PassengerRequest;
                //    cmd.Parameters.Add(@"Conn", SqlDbType.VarChar).Value = Conn;

                //    connection.Open();
                //    Int32 Row = cmd.ExecuteNonQuery();
                //    connection.Close();

                //    if (Row > 0)
                //    {
                //        Status = true;
                //    }
                //}
            }
            catch (Exception ex)
            {
                dbLogg(CompanyID, BookingRef, MethodName, Request + "-" + Response + "-" + PassengerRequest + "-" + Conn, SearchCriteria, SearchID, ex.Message);
            }
            return Status;
        }
        public static bool dbLoggAPI(string SearchID, string CompanyID, Int32 BookingRef, string Conn, string SearchCriteria, string MethodName, string Request, string Response, string PassengerRequest)
        {
            bool bStatus = false;
            try
            {
                var db = DatabaseContextFactory.CreateDbContext();
                string sql = @"EXECUTE LoggerAPI_Proc
                               @ProcNo = {0},
                               @CompanyID = {1},
                               @BookingRef = {2},
                               @SearchID = {3},
                               @SearchCriteria = {4},
                               @MethodName = {5},
                               @Request = {6},
                               @Response = {7},
                               @PassengerRequest = {8},
                               @Conn = {9}";

                var rowsAffected = db.Database.ExecuteSqlRawAsync(
                    sql,
                    1, // ProcNo
                    CompanyID,
                    BookingRef,
                    SearchID,
                    SearchCriteria,
                    MethodName,
                    Request,
                    Response,
                    PassengerRequest,
                    Conn);

                if (rowsAffected.Result > 0)
                {
                    bStatus = true;
                }
            }
            catch (Exception ex)
            {
                dbLogg(CompanyID, BookingRef, MethodName, Request + "-" + Response + "-" + PassengerRequest + "-" + Conn, SearchCriteria, SearchID, ex.Message);
            }
            return bStatus;
        }
        public static void WriteLogg(string CompanyID, Int32 BookingRef, string MethodName, string Location, string StoreData, string SearchCriteria, string SearchID)
        {
            try
            {
                string logpath = @"D:\zealLOGG\";
                if (Location.ToUpper() == "AVAILABILITY")
                {
                    logpath += @"BookingEngine\AVAILABILITY\";
                }
                else if (Location.ToUpper() == "FARE")
                {
                    logpath += @"BookingEngine\FARE\\";
                }
                else if (Location.ToUpper() == "SSR")
                {
                    logpath += @"BookingEngine\SSR\\";
                }
                else if (Location.ToUpper() == "PNR")
                {
                    logpath += @"BookingEngine\PNR\\";
                }
                else if (Location.ToUpper() == "TKT")
                {
                    logpath += @"BookingEngine\TKT\\";
                }
                else if (Location.ToUpper() == "CANCEL" || Location.ToUpper() == "REISSUE" || Location.ToUpper() == "RESCHEDULE")
                {
                    logpath += @"BookingChange\\";
                }
                else if (Location.ToUpper() == "ERROR")
                {
                    logpath += @"ERROR\\";
                }
                else if (Location.ToUpper() == "HOTEL")
                {
                    logpath += @"HOTEL\\";
                }
                else if (Location.ToUpper() == "STORE")
                {
                    logpath += @"STORE\\";
                }
                else
                {
                    logpath += @"ERROR\\";
                }

                object lockIndex = new object();
                lock (lockIndex)
                {
                    FileInfo objFileInfo;

                    string filepath = string.Empty;
                    if (!Directory.Exists(logpath))
                    {
                        try
                        {
                            Directory.CreateDirectory(logpath);
                        }
                        catch
                        {
                            logpath = logpath.Replace("D:", "E:");
                            Directory.CreateDirectory(logpath);
                        }
                    }

                    //filepath = logpath + DateTime.Today.ToString("dd-MMM-yyyy") + ".log";
                    filepath = logpath + DateTime.Today.ToString("dd-MMM-yyyy") + "-" + DateTime.Now.Hour.ToString() + ".log";
                    objFileInfo = new FileInfo(filepath);

                    //FileStream fs;
                    //if (objFileInfo.Exists)
                    //{
                    //    if (objFileInfo.Length > 5048576)
                    //    {
                    //        filepath = logpath + DateTime.Today.ToString("dd-MMM-yyyy") + "-" + DateTime.Now.ToString("H-m-s") + ".log";
                    //        fs = new FileStream(filepath, FileMode.Create, FileAccess.ReadWrite);
                    //    }
                    //    else
                    //    {
                    //        fs = new FileStream(filepath, FileMode.Append, FileAccess.Write);
                    //    }
                    //}
                    //else
                    //{
                    //    fs = new FileStream(filepath, FileMode.Create, FileAccess.ReadWrite);
                    //}

                    FileStream fs;
                    if (objFileInfo.Exists)
                    {
                        fs = new FileStream(filepath, FileMode.Append, FileAccess.Write);
                    }
                    else
                    {
                        fs = new FileStream(filepath, FileMode.Create, FileAccess.ReadWrite);
                    }

                    TextWriter m_streamWriter = new StreamWriter(fs);

                    if (!string.IsNullOrEmpty(StoreData))
                    {
                        m_streamWriter.WriteLine(Environment.NewLine);
                        m_streamWriter.WriteLine("START: SearchID: " + SearchID + "=============================================");
                        m_streamWriter.WriteLine(" SearchCriteria: " + SearchCriteria + " Method Name:" + MethodName + " Event:" + DateTime.Now.ToString());
                        m_streamWriter.WriteLine(" BookingRef: " + BookingRef + " CompanyID: " + CompanyID);
                        m_streamWriter.WriteLine(StoreData);
                        m_streamWriter.WriteLine("END: SearchID: " + SearchID + "=============================================");
                        m_streamWriter.WriteLine(Environment.NewLine);
                    }
                    else
                    {
                        m_streamWriter.WriteLine("***********************************************");
                    }

                    m_streamWriter.Close();
                    m_streamWriter.Dispose();
                }
            }
            catch (Exception _ex) //if error comes in log writing
            {
                //return "Error: Failed to Write Log" + _ex.Message;
            }
        }
        public static string getSearchQuery(string AirlineID, string DepartureStation, string ArrivalStation, string BeginDate, int iAdt, int iChd, int iInf)
        {
            string SearchQuery = string.Empty;

            try
            {
                SearchQuery += AirlineID + "-";
                SearchQuery += DepartureStation + "-";
                SearchQuery += ArrivalStation + "-";

                SearchQuery += iAdt.ToString() + "-";
                SearchQuery += iChd.ToString() + "-";
                SearchQuery += iInf.ToString() + "-";

                if (BeginDate.Length > 8)
                {
                    SearchQuery += Convert.ToDateTime(BeginDate).ToString("yyyyMMdd");
                }
                else
                {
                    SearchQuery += BeginDate;
                }
            }
            catch (Exception ex)
            {

            }

            return SearchQuery;
        }
        public static string getSearchQuery(string AirlineID, string DepartureStation, string ArrivalStation, string BeginDate, string EndDate, int iAdt, int iChd, int iInf)
        {
            string SearchQuery = string.Empty;

            try
            {
                SearchQuery += AirlineID + "-";
                SearchQuery += DepartureStation + "-";
                SearchQuery += ArrivalStation + "-";

                SearchQuery += iAdt.ToString() + "-";
                SearchQuery += iChd.ToString() + "-";
                SearchQuery += iInf.ToString() + "-";

                if (BeginDate.Length > 8)
                {
                    SearchQuery += Convert.ToDateTime(BeginDate).ToString("yyyyMMdd") + "-";
                }
                else
                {
                    SearchQuery += BeginDate + "-";
                }

                if (EndDate.Length > 8)
                {
                    SearchQuery += Convert.ToDateTime(EndDate).ToString("yyyyMMdd");
                }
                else
                {
                    SearchQuery += EndDate;
                }
            }
            catch (Exception ex)
            {

            }

            return SearchQuery;
        }
        public static string getSearchQuery(string Availability)
        {
            string SearchQuery = string.Empty;

            try
            {
                DataSet dsResponse = new DataSet();
                dsResponse.ReadXml(new System.IO.StringReader(Availability));

                SearchQuery += dsResponse.Tables[0].Rows[0]["AirlineID"].ToString().Trim() + "-";
                SearchQuery += dsResponse.Tables[0].Rows[0]["Origin"].ToString().Trim() + "-";
                SearchQuery += dsResponse.Tables[0].Rows[0]["Destination"].ToString().Trim() + "-";
                SearchQuery += dsResponse.Tables[0].Rows[0]["Adt"].ToString().Trim() + "-";
                SearchQuery += dsResponse.Tables[0].Rows[0]["Chd"].ToString().Trim() + "-";
                SearchQuery += dsResponse.Tables[0].Rows[0]["Inf"].ToString().Trim() + "-";

                if (dsResponse.Tables[0].Rows[0]["DepDate"].ToString().Trim().Length > 8)
                {
                    SearchQuery += Convert.ToDateTime(dsResponse.Tables[0].Rows[0]["DepDate"].ToString().Trim()).ToString("yyyyMMdd") + "-";
                }
                else
                {
                    SearchQuery += dsResponse.Tables[0].Rows[0]["DepDate"].ToString().Trim() + "-";
                }

                DataRow[] drSelect = dsResponse.Tables[0].Select("FltType='" + "I" + "'");
                if (drSelect.Length > 0)
                {
                    //if (drSelect.CopyToDataTable().Rows[0]["DepDate"].ToString().Trim().Length > 8)
                    //{
                    //    SearchQuery += Convert.ToDateTime(drSelect.CopyToDataTable().Rows[0]["DepDate"].ToString().Trim()).ToString("yyyyMMdd") + "-";
                    //}
                    //else
                    //{
                    //    SearchQuery += drSelect.CopyToDataTable().Rows[0]["DepDate"].ToString().Trim() + "-";
                    //}
                    if (drSelect[0]["DepDate"].ToString().Trim().Length > 8)
                    {
                        SearchQuery += Convert.ToDateTime(drSelect[0]["DepDate"].ToString().Trim()).ToString("yyyyMMdd") + "-";
                    }
                    else
                    {
                        SearchQuery += drSelect[0]["DepDate"].ToString().Trim() + "-";
                    }
                }
            }
            catch (Exception ex)
            {

            }

            return SearchQuery;
        }

        public static string getHOST()
        {
            string Host = string.Empty;
            try
            {
                string userIpAddress = HttpContextHelper.Current?.Request.HttpContext.Connection.RemoteIpAddress.ToString();
                 Host = HttpContextHelper.Current?.Request.Host + "," + userIpAddress;
            }
            catch
            {

            }
            return Host;
        }
    }
}
