using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using ZealTravelWebsite.Infrastructure.Context;

namespace ZealTravel.Infrastructure.DBCommon
{
    public class Airline_Cancellation_Rescheduling //: ConnectionString
    {
        private readonly ZealdbNContext _context;
        Airline_Cancellation_Rescheduling(ZealdbNContext context)
        {
            _context = context;
        }
        public static DataTable Airline_Cancellation_Rescheduling_Detail()
        {
            DataTable dtBaggage = new DataTable();

            try
            {
                using (SqlConnection connection = new SqlConnection(ConnectionString.dbConnect))
                {
                    SqlCommand cmd = new SqlCommand("Airline_Cancellation_Rescheduling_Charge_Detail_Proc", connection);
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
        public static DataTable Airline_Cancellation_Rescheduling_Detail(string Sector)
        {
            DataTable dtBaggage = new DataTable();

            try
            {
                using (SqlConnection connection = new SqlConnection(ConnectionString.dbConnect))
                {
                    SqlCommand cmd = new SqlCommand("Airline_Cancellation_Rescheduling_Charge_Detail_Proc", connection);
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
