using System;
using System.Data;
using System.Data.SqlClient;
namespace ZealTravel.Infrastructure.DBCommon
{
    public class AirlineBaggage : ConnectionString
    {
        public static DataSet Airline_Cancellation_Rescheduling_Fare_Rule_Baggage_Detail(string Sector)
        {
            DataSet dsDetail = new DataSet();

            try
            {
                using (SqlConnection connection = new SqlConnection(ConnectionString.dbConnect))
                {
                    SqlCommand cmd = new SqlCommand("get_Cancellation_Rescheduling_Fare_Rule_Baggage_Detail_Proc", connection);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 10;
                    cmd.Parameters.Add(@"ProcNo", SqlDbType.Int).Value = 1;
                    cmd.Parameters.Add(@"Sector", SqlDbType.VarChar, 1).Value = Sector;
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    adapter.TableMappings.Add("Table", "Airline_Cancellation_Rescheduling_Charge");
                    adapter.TableMappings.Add("Table1", "Airline_Fare_Rule");
                    adapter.TableMappings.Add("Table2", "Airline_Baggage");
                    adapter.Fill(dsDetail);
                }
            }
            catch (Exception ex)
            {
                
                Logger.dbLogg("", 0, "", "", "", "", ex.Message);
            }

            return dsDetail;
        }
        public static DataTable AirlineBaggageDetail()
        {
            DataTable dtBaggage = new DataTable();

            try
            {
                using (SqlConnection connection = new SqlConnection(ConnectionString.dbConnect))
                {
                    SqlCommand cmd = new SqlCommand("Airline_Baggage_Detail_Proc", connection);
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
        public static DataTable AirlineBaggageDetail(string Sector)
        {
            DataTable dtBaggage = new DataTable();

            try
            {
                using (SqlConnection connection = new SqlConnection(ConnectionString.dbConnect))
                {
                    SqlCommand cmd = new SqlCommand("Airline_Baggage_Detail_Proc", connection);
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
