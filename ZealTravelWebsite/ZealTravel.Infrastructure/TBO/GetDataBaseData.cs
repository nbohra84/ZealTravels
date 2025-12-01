using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZealTravel.Infrastructure.TBO
{
    class GetDataBaseData
    {
        //public static string GetMulticitySector(string mRequest)
        //{
        //    bool bDomestic = true;
        //    DataSet dsRequest = DBCommon.CommonFunction.StringToDataSet(mRequest);
        //    foreach (DataRow dr in dsRequest.Tables["AirSrchInfo"].Rows)
        //    {
        //        DBCommon.Airline_Detail objad = new DBCommon.Airline_Detail();
        //        bDomestic = objad.IsDomestic(dr["DepartureStation"].ToString(), dr["ArrivalStation"].ToString());
        //        if (bDomestic.Equals(false))
        //        {
        //            break;
        //        }
        //    }

        //    if (bDomestic)
        //    {
        //        return "D";
        //    }
        //    else
        //    {
        //        return "I";
        //    }
        //}
    //    public bool IsTicketAutoMode(string Companyid, string CarrierCode, string Sector)
    //    {
    //        try
    //        {
    //            using (SqlConnection connection = new SqlConnection())
    //            {
    //                SqlCommand cmd = new SqlCommand("Group_Commission_Rule_New_Proc", connection);
    //                cmd.CommandType = CommandType.StoredProcedure;
    //                cmd.CommandTimeout = 10;
    //                cmd.Parameters.Add(@"ProcNo", SqlDbType.Int).Value = 11;
    //                cmd.Parameters.Add(@"CompanyID", SqlDbType.VarChar).Value = Companyid;
    //                cmd.Parameters.Add(@"CarrierCode", SqlDbType.VarChar).Value = CarrierCode;
    //                cmd.Parameters.Add(@"Sector", SqlDbType.VarChar).Value = Sector;

    //                connection.Open();
    //                bool Status = (bool)cmd.ExecuteScalar();
    //                connection.Close();

    //                return Status;
    //            }
    //        }
    //        catch (Exception ex)
    //        {

    //        }
    //        return false;
    //    }
    //    public bool SetTimePriceStatus(Int32 BookingRef, bool IsTimeChanged, bool IsPriceChanged, string Conn)
    //    {
    //        try
    //        {
    //            using (SqlConnection connection = new SqlConnection())
    //            {
    //                SqlCommand cmd = new SqlCommand("Company_Flight_Segment_Rule_Detail_Airline_Proc", connection);
    //                cmd.CommandType = CommandType.StoredProcedure;
    //                cmd.CommandTimeout = 10;
    //                cmd.Parameters.Add(@"ProcNo", SqlDbType.Int).Value = 2;
    //                cmd.Parameters.Add(@"BookingRef", SqlDbType.Int).Value = BookingRef;
    //                cmd.Parameters.Add(@"IsTimeChanged", SqlDbType.Bit).Value = IsTimeChanged;
    //                cmd.Parameters.Add(@"IsPriceChanged", SqlDbType.Bit).Value = IsPriceChanged;
    //                cmd.Parameters.Add(@"Conn", SqlDbType.VarChar).Value = Conn;

    //                connection.Open();
    //                int i = cmd.ExecuteNonQuery();
    //                connection.Close();

    //                if (i > 0)
    //                {
    //                  return true;
    //                }
    //            }
    //        }
    //        catch (Exception ex)
    //        {
              
    //        }

    //        return false;
    //    }
    //
    }
}

