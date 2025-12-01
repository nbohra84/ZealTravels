using System;
using System.Collections.Specialized;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Xml;

namespace ZealTravel.Infrastructure.DBCommon
{
    public class DataUtility : IDisposable
    {
        private SqlConnection mCon;
        private SqlCommand mDataCom;

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (mDataCom != null)
                {
                    mDataCom.Dispose();
                    mDataCom = null;
                }
                if (mCon != null)
                {
                    mCon.Dispose();
                    mCon = null;
                }
            }
            // free native resources
        }

        //#region All private members
        //SqlConnection mCon;
        //SqlCommand mDataCom;
        //#endregion

        #region All private methods
        /// <summary>
        /// This is to open an active conncetion
        /// </summary>
        private void OpenConnection()
        {
            if (mCon == null)
            {
                mCon = new SqlConnection(ConnectionString.dbConnect);
            }

            if (mCon.State == ConnectionState.Closed)
            {
                mCon.Open();
                //mCon.ConnectionTimeout = 200;
                mDataCom = new SqlCommand();
                mDataCom.CommandTimeout = 0;
                mDataCom.Connection = mCon;
            }
        }

        /// <summary>
        /// This is to close an active connection
        /// </summary>
        private void CloseConnection()
        {
            if (mCon.State == ConnectionState.Open)
            {
                mCon.Close();
            }
        }
        private void DisposeConnection()
        {
            if (mDataCom != null)
            {
                mDataCom.Dispose();
                mDataCom = null;
            }
            if (mCon != null)
            {
                mCon.Dispose();
                mCon = null;
            }
        }
        #endregion

        #region All public methods

        /// <summary>
        /// This is to execute all DML using sql query 
        /// </summary>
        /// <param name="strsql">SQL query</param>
        /// <returns>int</returns>
        public int ExecuteSql(string sql)
        {
            try
            {
                OpenConnection();
                mDataCom.CommandType = CommandType.Text;
                mDataCom.CommandText = sql;
                int iresult = mDataCom.ExecuteNonQuery();
                //CloseConnection();
                //DisposeConnection();
                return iresult;
            }
            catch (SqlException)
            {
                return 0;
                //Console.Write(objexec.Message);
            }
            finally
            {
                CloseConnection();
                DisposeConnection();
            }
        }


        /// <summary>
        /// This is to execute all DML using Storedprocedure  
        /// </summary>
        /// <param name="SPName">Stored procedure name</param>
        /// <param name="arraparam">SQL Parameter</param>
        /// <returns>int</returns>
        public int ExecuteSql(string name, SqlParameter[] parameters)
        {
            OpenConnection();
            mDataCom.CommandType = CommandType.StoredProcedure;
            mDataCom.CommandText = name;
            if (mDataCom.Parameters.Count > 0)
            {
                mDataCom.Parameters.Clear();
            }

            if (parameters != null)
            {
                foreach (SqlParameter param1 in parameters)
                {
                    mDataCom.Parameters.Add(param1);
                }
            }
            int result = mDataCom.ExecuteNonQuery();
            CloseConnection();
            DisposeConnection();
            return result;
        }

        /// <summary>
        /// Get XML String
        /// </summary>
        /// <param name="strSql"></param>
        /// <returns></returns>
        public string GetXmlRecordWithParam(string strSql, NameValueCollection nvcParameters)
        {
            StringBuilder strResult = new StringBuilder();
            XmlReader rdr;
            OpenConnection();
            mDataCom.CommandType = CommandType.StoredProcedure;
            mDataCom.CommandText = strSql;
            if (nvcParameters != null)
            {
                foreach (string param1 in nvcParameters)
                {
                    mDataCom.Parameters.AddWithValue(param1, nvcParameters[param1]);
                }
            }

            rdr = mDataCom.ExecuteXmlReader();
            rdr.Read();

            while (rdr.ReadState != ReadState.EndOfFile)
            {
                strResult.Append(rdr.ReadOuterXml());
            }

            rdr.Close();
            CloseConnection();
            DisposeConnection();
            return strResult.ToString();
        }
        public string GetXmlRecord(string strSql)
        {
            StringBuilder strResult = new StringBuilder();
            XmlReader rdr;
            OpenConnection();
            mDataCom.CommandType = CommandType.Text;
            mDataCom.CommandText = strSql;
            rdr = mDataCom.ExecuteXmlReader();
            rdr.Read();

            while (rdr.ReadState != ReadState.EndOfFile)
            {
                strResult.Append(rdr.ReadOuterXml());
            }

            rdr.Close();
            CloseConnection();
            DisposeConnection();
            return strResult.ToString();
        }

        /// <summary>
        /// This is to execute DQL(without Aggregate function)in disconnected mode using SQL string as parameter
        /// </summary>
        /// <param name="strSql">SQL string</param>
        /// <returns>DataTable</returns>
        public DataTable GetDataTable(string sql)
        {
            DataTable dt = new DataTable();
            dt.Locale = System.Globalization.CultureInfo.CurrentUICulture;
            OpenConnection();
            mDataCom.CommandType = CommandType.Text;
            mDataCom.CommandText = sql;
            SqlDataAdapter mDA = new SqlDataAdapter();
            mDA.SelectCommand = mDataCom;
            mDA.Fill(dt);
            CloseConnection();
            DisposeConnection();
            return dt;
        }




        /// <summary>
        /// This is to execute DQL(without Aggregate function)in disconnected mode using SQL string as parameter
        /// </summary>
        /// <param name="strSql">SQL string</param>
        /// <returns>DataSet</returns>
        public DataSet GetDataSet(string sql)
        {
            DataSet ds = new DataSet();
            ds.Locale = System.Globalization.CultureInfo.CurrentUICulture;
            OpenConnection();
            mDataCom.CommandType = CommandType.Text;
            mDataCom.CommandText = sql;
            SqlDataAdapter mDA = new SqlDataAdapter();
            mDA.SelectCommand = mDataCom;
            mDA.Fill(ds);
            CloseConnection();
            DisposeConnection();
            return ds;
        }

        /// <summary>
        /// This is to execute DQL(without Aggregate function)in disconnected mode using Proc Name string as parameter
        /// and Parameter as SqlParameter[]
        /// </summary>
        /// <param name="strSql">SQL string</param>
        /// <returns>DataSet</returns>
        public DataSet GetDataSet(string name, SqlParameter[] parameters)
        {
            DataSet ds = new DataSet();
            try
            {
                OpenConnection();
                mDataCom.CommandType = CommandType.StoredProcedure;
                mDataCom.CommandText = name;
                if (mDataCom.Parameters.Count > 0)
                {
                    mDataCom.Parameters.Clear();
                }

                if (parameters != null)
                {
                    foreach (SqlParameter param1 in parameters)
                    {
                        mDataCom.Parameters.Add(param1);
                    }
                }
                SqlDataAdapter mDA = new SqlDataAdapter();
                mDA.SelectCommand = mDataCom;
                mDA.Fill(ds);
                CloseConnection();
                DisposeConnection();
            }
            catch
            {
                ds = null;
            }
            return ds;
        }
        public int ExecuteSql(string name, NameValueCollection nvcParameters)
        {
            OpenConnection();
            mDataCom.CommandType = CommandType.StoredProcedure;
            mDataCom.CommandText = name;
            if (mDataCom.Parameters.Count > 0)
            {
                mDataCom.Parameters.Clear();
            }

            if (nvcParameters != null)
            {
                foreach (string param1 in nvcParameters)
                {
                    mDataCom.Parameters.AddWithValue(param1, nvcParameters[param1]);
                }
            }
            int result = mDataCom.ExecuteNonQuery();
            CloseConnection();
            DisposeConnection();
            return result;
        }


        /// <summary>
        /// dataSet with NameValueCollection
        /// </summary>
        /// <param name="name"></param>
        /// <param name="nvcParameters"></param>
        /// <returns></returns>
        public DataSet GetDataSet(string name, NameValueCollection nvcParameters)
        {
            DataSet ds = new DataSet();
            try
            {
                OpenConnection();
                mDataCom.CommandType = CommandType.StoredProcedure;
                mDataCom.CommandText = name;
                if (mDataCom.Parameters.Count > 0)
                {
                    mDataCom.Parameters.Clear();
                }

                if (nvcParameters != null)
                {
                    foreach (string param1 in nvcParameters)
                    {
                        mDataCom.Parameters.AddWithValue(param1, nvcParameters[param1]);
                        //mDataCom.Parameters.Add(param1);
                    }
                }
                SqlDataAdapter mDA = new SqlDataAdapter();
                mDA.SelectCommand = mDataCom;
                mDA.Fill(ds);
                CloseConnection();
                DisposeConnection();
            }
            catch (Exception excep)
            {
                //EMailClass objMail = new EMailClass();
                //objMail.SendErrorMail("Error in GetDataSet NameValueCollection ", excep);
                //ds = null;
            }
            return ds;
        }

        /// <summary>
        /// dataSet with NameValueCollection
        /// </summary>
        /// <param name="name"></param>
        /// <param name="nvcParameters"></param>
        /// <returns></returns>
        public DataTable GetDataTable(string name, NameValueCollection nvcParameters)
        {
            DataTable dt = new DataTable();
            try
            {
                OpenConnection();
                mDataCom.CommandType = CommandType.StoredProcedure;
                mDataCom.CommandText = name;
                if (mDataCom.Parameters.Count > 0)
                {
                    mDataCom.Parameters.Clear();
                }

                if (nvcParameters != null)
                {
                    foreach (string param1 in nvcParameters)
                    {
                        string ss = param1;
                        string s1 = nvcParameters[param1];
                        mDataCom.Parameters.AddWithValue(param1, nvcParameters[param1]);
                    }
                }
                SqlDataAdapter mDA = new SqlDataAdapter();
                mDA.SelectCommand = mDataCom;
                mDA.Fill(dt);
                CloseConnection();
                DisposeConnection();
            }
            catch (Exception excep)
            {
                //EMailClass objMail = new EMailClass();
                //objMail.SendErrorMail("Error in GetDataTable NameValueCollection ", excep);
                //dt = null;
            }
            return dt;
        }


        /// <summary>
        /// This is to execute DQL(without Aggregate function)in disconnected mod using Stored Procedure as parameter
        /// </summary>
        /// <param name="SPName">Stored procedure name</param>
        /// <param name="arraparam">SQL Parameter</param>
        /// <returns>DataTable</returns>
        public DataTable GetDataTable(string name, SqlParameter[] parameters)
        {
            DataTable dt = new DataTable();
            dt.Locale = System.Globalization.CultureInfo.CurrentUICulture;
            OpenConnection();
            mDataCom.CommandType = CommandType.StoredProcedure;
            mDataCom.CommandText = name;
            if (mDataCom.Parameters.Count > 0)
            {
                mDataCom.Parameters.Clear();
            }

            if (parameters != null)
            {
                foreach (SqlParameter param in parameters)
                {
                    mDataCom.Parameters.Add(param);
                }
            }
            SqlDataAdapter mDA = new SqlDataAdapter();
            mDA.SelectCommand = mDataCom;
            mDA.Fill(dt);
            CloseConnection();
            DisposeConnection();
            return dt;
        }

        /// <summary>
        /// This is to execute DQL(with Aggregate function) using SQL query as parameter
        /// </summary>
        /// <param name="strSql">String SQL</param>
        /// <returns>bool</returns>
        public bool IsExist(string sql)
        {
            OpenConnection();
            mDataCom.CommandType = CommandType.Text;
            mDataCom.CommandText = sql;
            int result = (int)mDataCom.ExecuteScalar();
            CloseConnection();
            DisposeConnection();
            if (result > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// This is to execute DQL(with Aggregate function) using SQL query as parameter
        /// </summary>
        /// <param name="strSql">String SQL</param>
        /// <returns>int</returns>
        public int CountRecords(string sql)
        {
            OpenConnection();
            mDataCom.CommandType = CommandType.Text;
            mDataCom.CommandText = sql;
            int iResult = (int)mDataCom.ExecuteScalar();
            CloseConnection();
            DisposeConnection();
            return iResult;
        }


        /// <summary>
        /// This is to execute DQL(with Aggregate function) using stored procedure as parameter
        /// </summary>
        /// <param name="SPName">Stored procedure name</param>
        /// <param name="arraparam">SQL Parameter</param>
        /// <returns>bool</returns>
        public bool IsExist(string spName, SqlParameter[] parameters)
        {
            OpenConnection();
            mDataCom.CommandType = CommandType.StoredProcedure;
            mDataCom.CommandText = spName;
            if (mDataCom.Parameters.Count > 0)
            {
                mDataCom.Parameters.Clear();
            }
            if (parameters != null)
            {
                foreach (SqlParameter param in parameters)
                {
                    mDataCom.Parameters.Add(param);
                }
            }
            int iResult = (int)mDataCom.ExecuteScalar();
            CloseConnection();
            DisposeConnection();
            if (iResult > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// This is to execute DQL in connected mode using SQL query as parameter.
        /// </summary>
        /// <param name="strSql">SQL string</param>
        /// <returns>SqlDataReader</returns>
        public SqlDataReader GetDataReader(string sql)
        {
            OpenConnection();
            mDataCom.CommandType = CommandType.Text;
            mDataCom.CommandText = sql;

            SqlDataReader dReader;
            dReader = mDataCom.ExecuteReader(CommandBehavior.CloseConnection);
            return dReader;
        }


        /// <summary>
        /// This is to execute DQL in connected mode using stored procedure
        /// </summary>
        // <param name="SPName">Stored procedure name</param>
        /// <param name="arraparam">SQL Parameter</param>
        /// <returns>SqlDataReader</returns>
        public SqlDataReader GetDataReader(string name, SqlParameter[] parameters)
        {
            OpenConnection();
            mDataCom.CommandType = CommandType.StoredProcedure;
            mDataCom.CommandText = name;
            if (mDataCom.Parameters.Count > 0)
            {
                mDataCom.Parameters.Clear();
            }
            if (parameters != null)
            {
                foreach (SqlParameter param in parameters)
                {
                    mDataCom.Parameters.Add(param);
                }
            }
            SqlDataReader dReader;
            dReader = mDataCom.ExecuteReader(CommandBehavior.CloseConnection);
            return dReader;
        }
        #endregion
    }
}
