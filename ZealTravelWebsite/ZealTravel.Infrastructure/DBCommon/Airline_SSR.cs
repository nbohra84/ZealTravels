using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace ZealTravel.Infrastructure.DBCommon
{
    public class Airline_SSR : ConnectionString
    {
        public string getSSRCode(string CarrierCode, string ProductClass, string DepartureDateTime)
        {
            string SSRcode = string.Empty;
            try
            {
                bool IsGreaterTenHour = IsMoreThan10Hours(DepartureDateTime);
                using (SqlConnection connection = new SqlConnection(dbCommon.ConnectionString.ConnectionLIVE))
                {
                    SqlCommand cmd = new SqlCommand("Airline_SSR_List_Proc", connection);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 10;
                    cmd.Parameters.Add(@"ProcNo", SqlDbType.Int).Value = 3;
                    cmd.Parameters.Add(@"CarrierCode", SqlDbType.VarChar).Value = CarrierCode;
                    cmd.Parameters.Add(@"ProductClass", SqlDbType.VarChar).Value = ProductClass;
                    cmd.Parameters.Add(@"IsGreaterTenHour", SqlDbType.Bit).Value = IsGreaterTenHour;

                    connection.Open();
                    if (cmd.ExecuteScalar() != null)
                    {
                        SSRcode = cmd.ExecuteScalar().ToString();
                    }
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                dbCommon.Logger.dbLogg("", 0, DepartureDateTime, "getSSRdbList", CarrierCode, ProductClass, ex.Message);
            }
            return SSRcode;
        }
        private bool IsMoreThan10Hours(string DepartureDate)
        {
            try
            {
                DateTime dt = Convert.ToDateTime(DepartureDate);
                TimeSpan ts = dt.Subtract(DateTime.Now);
                if (ts.TotalHours > 10)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
        }
        public DataTable getSSRResults(string FltType, string CarrierCode)
        {
            DataTable dtSSR = dbCommon.Schema.SchemaSSR;
            dtSSR.Columns.Add("FltType", typeof(string));
            try
            {
                int iRow = 1;
                DataTable dtSSRlist = getSSRdbList(CarrierCode);
                if (dtSSRlist != null && dtSSRlist.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtSSRlist.Rows)
                    {
                        DataRow drAdd = dtSSR.NewRow();
                        drAdd["RowID"] = iRow;
                        drAdd["CarrierCode"] = dr["CarrierCode"].ToString();
                        drAdd["Code"] = dr["SsrCode"].ToString();
                        drAdd["CodeType"] = dr["SsrType"].ToString();
                        drAdd["Amount"] = dr["Amount"].ToString();
                        drAdd["Description"] = dr["Description"].ToString();
                        drAdd["Detail"] = dr["AdditionalRule"].ToString();
                        dtSSR.Rows.Add(drAdd);
                        iRow++;
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return dtSSR;
        }
        public DataTable getSSRdbList(string CarrierCode)
        {
            DataTable dtSSR = new DataTable();
            try
            {
                using (SqlConnection connection = new SqlConnection(dbCommon.ConnectionString.ConnectionLIVE))
                {
                    SqlCommand cmd = new SqlCommand("Airline_SSR_List_Proc", connection);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 10;
                    cmd.Parameters.Add(@"ProcNo", SqlDbType.Int).Value = 1;
                    cmd.Parameters.Add(@"CarrierCode", SqlDbType.VarChar).Value = CarrierCode;
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    adapter.Fill(dtSSR);
                }
            }
            catch (Exception ex)
            {
                dbCommon.Logger.dbLogg("", 0, "", "getSSRdbList", CarrierCode, "", ex.Message);
            }

            return dtSSR;
        }
    }
}
