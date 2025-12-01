using Microsoft.EntityFrameworkCore;
using ZealTravel.Domain.Interfaces.DBCommon;
using ZealTravelWebsite.Infrastructure.Context;

namespace ZealTravel.Infrastructure.Services.DBLogger
{
    public class DBLoggerService : IDBLoggerService
    {
        private readonly ZealdbNContext _context;

        public DBLoggerService(ZealdbNContext context)
        {
            _context = context;

        }

        public Task<bool> dbSearchLogg(string CompanyID, Int32 BookingRef, string StaffID, string SearchID, string Location, string Status, string Place, string Remark, string Remark2, string Host)
        {
            bool bStatus = false;
            try
            {
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

                var rowsAffected = _context.Database.ExecuteSqlRawAsync(
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
            return Task.FromResult(bStatus);

        }
        public Task<bool> dbLogg(string CompanyID, Int32 BookingRef, string MethodName, string Location, string SearchCriteria, string SearchID, string ErrorMessage)
        {
            bool Status = false;
            try
            {
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

                var rowsAffected = _context.Database.ExecuteSqlRawAsync(
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
            return Task.FromResult(Status);
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
