using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZealTravel.Domain.Interfaces.Loggers;
using ZealTravelWebsite.Infrastructure.Context;

namespace ZealTravel.Infrastructure.Logger
{
    public class Logger : ILogger
    {
        private readonly ZealdbNContext _context;
        public Logger(ZealdbNContext context)
        {
            _context = context;
        }
        public bool dbLogg(string CompanyID, int BookingRef, string MethodName, string Location, string SearchCriteria, string SearchID, string ErrorMessage)
        {
            bool bStatus = false;
            //try
            //{
            //    //using (SqlConnection connection = new SqlConnection(ConnectionString.dbConnect))
            //    {
            //        SqlCommand cmd = new SqlCommand("LoggerSearch_Proc", connection);
            //        cmd.CommandType = CommandType.StoredProcedure;
            //        cmd.CommandTimeout = 10;

            //        cmd.Parameters.Add(@"ProcNo", SqlDbType.Int).Value = 1;
            //        cmd.Parameters.Add(@"CompanyID", SqlDbType.VarChar).Value = CompanyID;
            //        cmd.Parameters.Add(@"BookingRef", SqlDbType.Int).Value = BookingRef;
            //        cmd.Parameters.Add(@"StaffID", SqlDbType.VarChar).Value = StaffID;
            //        cmd.Parameters.Add(@"SearchID", SqlDbType.VarChar).Value = SearchID;
            //        cmd.Parameters.Add(@"Location", SqlDbType.VarChar).Value = Location;
            //        cmd.Parameters.Add(@"Status", SqlDbType.VarChar).Value = Status;
            //        cmd.Parameters.Add(@"Place", SqlDbType.VarChar).Value = Place;
            //        cmd.Parameters.Add(@"Remark", SqlDbType.VarChar).Value = Remark;
            //        cmd.Parameters.Add(@"Remark2", SqlDbType.VarChar).Value = Remark2;
            //        cmd.Parameters.Add(@"Host", SqlDbType.VarChar).Value = Host;

            //        _context.log;
            //        Int32 Row = cmd.ExecuteNonQuery();
            //        connection.Close();

            //        if (Row > 0)
            //        {
            //            bStatus = true;
            //        }
            //    }
            //}
            //catch (Exception ex)
            //{
            //    dbLogg(CompanyID, BookingRef, Location + "-" + Place, Status, Remark, SearchID, ex.Message);
            //}
            return bStatus;
        }

        public bool dbLoggAPI(string SearchID, string CompanyID, int BookingRef, string Conn, string SearchCriteria, string MethodName, string Request, string Response, string PassengerRequest)
        {
            throw new NotImplementedException();
        }

        public bool dbLoggHotelAPI(string SearchID, string CompanyID, int BookingRef, string Conn, string SearchCriteria, string MethodName, string Request, string Response, string PassengerRequest)
        {
            throw new NotImplementedException();
        }

        public bool dbSearchLogg(string CompanyID, int BookingRef, string StaffID, string SearchID, string Location, string Status, string Place, string Remark, string Remark2, string Host)
        {
            throw new NotImplementedException();
        }
    }
}
