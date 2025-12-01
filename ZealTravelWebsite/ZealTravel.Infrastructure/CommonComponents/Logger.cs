using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZealTravel.Infrastructure.CommonComponents
{
    public class Logger
    {
        public StringBuilder stringBuilderLog;

        public Logger()
        {
            stringBuilderLog = new StringBuilder();
        }

        public static void WriteTraceInfo(string logServiceName, string logModuleName, string logFileName, string logMessage, string companyId)
        {
            Task task = Task.Run(delegate
            {
                LogType type = ((ConfigurationManager.AppSettings["LogToWrite"] != null && Convert.ToString(ConfigurationManager.AppSettings["LogToWrite"]).ToUpper() == "S3BUCKET") ? LogType.S3Bucket : LogType.Server);
                AppLoggeFactory.GetLogger(type).WriteTraceInfoAsync(logServiceName, logModuleName, logFileName, logMessage, companyId);
            });
            task.Wait();
        }

        public static void WriteTraceInfo(string logServiceName, string logModuleName, string logFileName, string gdsPnr, string logMessage, string companyId)
        {
            Task task = Task.Run(delegate
            {
                LogType type = ((ConfigurationManager.AppSettings["LogToWrite"] != null && Convert.ToString(ConfigurationManager.AppSettings["LogToWrite"]).ToUpper() == "S3BUCKET") ? LogType.S3Bucket : LogType.Server);
                AppLoggeFactory.GetLogger(type).WriteTraceInfoAsync(logServiceName, logModuleName, logFileName, gdsPnr, logMessage, companyId);
            });
            task.Wait();
        }

        public void AddToLog(string input)
        {
            stringBuilderLog.Append(DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss.fff") + " :: " + input + Environment.NewLine);
        }
    }
}
