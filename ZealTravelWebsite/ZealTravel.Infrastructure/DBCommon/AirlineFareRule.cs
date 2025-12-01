using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace ZealTravel.Infrastructure.DBCommon
{
    public class AirlineFareRule : ConnectionString
    {
        public static DataTable AirlineFareRuleDetail()
        {
            DataTable dtBaggage = new DataTable();

            try
            {
                using (SqlConnection connection = new SqlConnection(ConnectionString.dbConnect))
                {
                    SqlCommand cmd = new SqlCommand("Airline_Fare_Rule_Detail_Proc", connection);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 10;
                    cmd.Parameters.Add(@"ProcNo", SqlDbType.Int).Value = 3;
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    adapter.Fill(dtBaggage);
                }
            }
            catch (Exception ex)
            {
                Logger.dbLogg("", 0, "", "", "", "", ex.Message);
            }

            return dtBaggage;
        }
        public static DataTable AirlineFareRuleDetail(string Sector)
        {
            DataTable dtBaggage = new DataTable();

            try
            {
                using (SqlConnection connection = new SqlConnection(ConnectionString.dbConnect))
                {
                    SqlCommand cmd = new SqlCommand("Airline_Fare_Rule_Detail_Proc", connection);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 10;
                    cmd.Parameters.Add(@"ProcNo", SqlDbType.Int).Value = 5;
                    cmd.Parameters.Add(@"Sector", SqlDbType.VarChar, 1).Value = Sector;
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    adapter.Fill(dtBaggage);
                }
            }
            catch (Exception ex)
            {
                Logger.dbLogg("", 0, "", "", "", "", ex.Message);
            }

            return dtBaggage;
        }
    }
}
