using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace ZealTravel.Infrastructure.CommonComponents
{
    public class DBConnection
    {
        private static string logServiceName = ((ConfigurationManager.AppSettings["LogServiceName"] != null) ? ConfigurationManager.AppSettings["LogServiceName"] : "CommonComponents");

        private static string logModuleName = "CommonComponents";

        private static string logFileName = "DBConnection";

        private static ILogger logger;

        private string companyId;

        public DBConnection(string CompanyId)
        {
            companyId = CompanyId;
            logger = new Logger();
        }

        public static string ConnectionString(string companyId, string dbCode)
        {
            logger = new Logger();
            string empty = string.Empty;
            return "data source=216.172.109.204; initial catalog=zealdb_N;user id=sa;pwd=YUv@an#sh12#$";
        }

        public DataSet ExecuteDataSet(List<SqlParameter> parameters, string spName, string dataBaseName)
        {
            string connectionString = ConnectionString(companyId, dataBaseName);
            DataSet dataSet = null;
            try
            {
                using SqlConnection sqlConnection = new SqlConnection(connectionString);
                SqlCommand sqlCommand = new SqlCommand();
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter();
                sqlCommand.CommandText = spName;
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.Connection = sqlConnection;
                foreach (SqlParameter parameter in parameters)
                {
                    sqlCommand.Parameters.Add(parameter);
                }

                sqlDataAdapter.SelectCommand = sqlCommand;
                if (!sqlConnection.State.Equals(ConnectionState.Open))
                {
                    sqlConnection.Open();
                }

                dataSet = new DataSet();
                sqlDataAdapter.Fill(dataSet);
                sqlConnection.Close();
            }
            catch (Exception ex)
            {
                logger.AddToLog("---- In Catch:ExecuteDataSet() ----");
                logger.AddToLog("---- Error : StackTrace ----" + ex.Message + " : " + ex.StackTrace);
            }
            finally
            {
                if (logger.stringBuilderLog.Length > 0)
                {
                    Logger.WriteTraceInfo(logServiceName, logModuleName, logFileName, logger.stringBuilderLog.ToString(), companyId);
                }
            }

            return dataSet;
        }

        public DataTable ExecuteDataTable(List<SqlParameter> parameters, string spName, string dataBaseName)
        {
            string connectionString = ConnectionString(companyId, dataBaseName);
            DataTable dataTable = null;
            try
            {
                using SqlConnection sqlConnection = new SqlConnection(connectionString);
                SqlCommand sqlCommand = new SqlCommand();
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter();
                sqlCommand.CommandText = spName;
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.Connection = sqlConnection;
                foreach (SqlParameter parameter in parameters)
                {
                    sqlCommand.Parameters.Add(parameter);
                }

                sqlDataAdapter.SelectCommand = sqlCommand;
                if (!sqlConnection.State.Equals(ConnectionState.Open))
                {
                    sqlConnection.Open();
                }

                dataTable = new DataTable();
                sqlDataAdapter.Fill(dataTable);
                sqlConnection.Close();
            }
            catch (Exception ex)
            {
                logger.AddToLog("---- In Catch:ExecuteDataTable() ----");
                logger.AddToLog("---- Error : StackTrace ----" + ex.Message + " : " + ex.StackTrace);
            }
            finally
            {
                if (logger.stringBuilderLog.Length > 0)
                {
                    Logger.WriteTraceInfo(logServiceName, logModuleName, logFileName, logger.stringBuilderLog.ToString(), companyId);
                }
            }

            return dataTable;
        }

        public object GetScalarValue(List<SqlParameter> parameters, string spName, string dataBaseName)
        {
            string connectionString = ConnectionString(companyId, dataBaseName);
            object result = "";
            try
            {
                using SqlConnection sqlConnection = new SqlConnection(connectionString);
                SqlCommand sqlCommand = new SqlCommand();
                sqlCommand.CommandText = spName;
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.Connection = sqlConnection;
                foreach (SqlParameter parameter in parameters)
                {
                    sqlCommand.Parameters.Add(parameter);
                }

                if (!sqlConnection.State.Equals(ConnectionState.Open))
                {
                    sqlConnection.Open();
                }

                result = sqlCommand.ExecuteScalar();
                sqlConnection.Close();
            }
            catch (Exception ex)
            {
                logger.AddToLog("---- In Catch:GetScalarValue() ----");
                logger.AddToLog("---- Error : StackTrace ----" + ex.Message + " : " + ex.StackTrace);
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

        public object GetXMLValue(List<SqlParameter> parameters, string spName, string dataBaseName)
        {
            string connectionString = ConnectionString(companyId, dataBaseName);
            object result = null;
            try
            {
                using SqlConnection sqlConnection = new SqlConnection(connectionString);
                SqlCommand sqlCommand = new SqlCommand();
                sqlCommand.CommandText = spName;
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.Connection = sqlConnection;
                foreach (SqlParameter parameter in parameters)
                {
                    sqlCommand.Parameters.Add(parameter);
                }

                if (!sqlConnection.State.Equals(ConnectionState.Open))
                {
                    sqlConnection.Open();
                }

                XmlReader xmlReader = sqlCommand.ExecuteXmlReader();
                if (xmlReader.Read())
                {
                    XmlDocument xmlDocument = new XmlDocument();
                    xmlDocument.Load(xmlReader);
                    result = xmlDocument.OuterXml;
                    xmlDocument = null;
                }

                sqlConnection.Close();
            }
            catch (Exception ex)
            {
                logger.AddToLog("---- In Catch:GetScalarValue() ----");
                logger.AddToLog("---- Error : StackTrace ----" + ex.Message + " : " + ex.StackTrace);
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
