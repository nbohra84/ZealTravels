using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZealTravel.Domain.BookingManagement;
using ZealTravel.Domain.Interfaces.AirlineManagement;

namespace ZealTravel.Infrastructure.Spicejet
{
    class GetCommit
    {
        public async Task<bool> GetCommitOneWay(string Searchid, string Companyid, int BookingRef, string RS_Availability, string RS_Passenger, string RS_Company, string RS_GstDetail, IBookingManagementService bookingService,string PaymentType)
        {
                DataTable dtBound = GetCommonFunctions.StringToDataSet(RS_Availability).Tables["AvailabilityInfo"];
                DataTable dtPassengerInfo = GetCommonFunctions.StringToDataSet(RS_Passenger).Tables["PassengerInfo"];
                DataTable dtComppanyInfo = GetCommonFunctions.StringToDataSet(RS_Company).Tables["CompanyInfo"];
                DataTable dtGstInfo = new DataTable();
                if (RS_GstDetail != null && RS_GstDetail.IndexOf("GstInfo") != -1)
                {
                    dtGstInfo = GetCommonFunctions.StringToDataSet(RS_GstDetail).Tables["GstInfo"];
                }

                if (dtBound != null && dtBound.Rows.Count > 0 && dtPassengerInfo != null && dtPassengerInfo.Rows.Count > 0 && dtComppanyInfo != null && dtComppanyInfo.Rows.Count > 0)
                {
                    if (!PaymentType.ToUpper().Equals("PAYMENTHOLD"))
                    {
                        GetBooking objBooking = new GetBooking(Searchid, Companyid, BookingRef, bookingService);
                        return await objBooking.GetCommitOneWay(dtBound, dtPassengerInfo, dtComppanyInfo, dtGstInfo);
                    
                    }
                    else
                    {
                        GetBookingAsHold objholdbooking = new GetBookingAsHold(Searchid, Companyid, BookingRef, bookingService);
                        return await objholdbooking.GetCommitOneWayAsHold(dtBound, dtPassengerInfo, dtComppanyInfo, dtGstInfo);
                    }
                    
                }
            
            return false;
        }
        public async Task<bool> GetCommitRT(string Searchid, string Companyid, int BookingRef, string RS_Availability, string RS_Passenger, string RS_Company, string RS_GstDetail, IBookingManagementService bookingService,string PaymentType)
        {
          
                DataTable dtBound = GetCommonFunctions.StringToDataSet(RS_Availability).Tables["AvailabilityInfo"];
                DataRow[] drOutbound = dtBound.Select("FltType='" + "O" + "'");
                DataRow[] drInbound = dtBound.Select("FltType='" + "I" + "'");

                DataTable dtPassengerInfo = GetCommonFunctions.StringToDataSet(RS_Passenger).Tables["PassengerInfo"];
                DataTable dtComppanyInfo = GetCommonFunctions.StringToDataSet(RS_Company).Tables["CompanyInfo"];
                DataTable dtGstInfo = new DataTable();
                if (RS_GstDetail != null && RS_GstDetail.IndexOf("GstInfo") != -1)
                {
                    dtGstInfo = GetCommonFunctions.StringToDataSet(RS_GstDetail).Tables["GstInfo"];
                }

                if (drOutbound.Length > 0 && drInbound.Length > 0 && dtPassengerInfo != null && dtPassengerInfo.Rows.Count > 0 && dtComppanyInfo != null && dtComppanyInfo.Rows.Count > 0)
                {

                    if (!PaymentType.ToUpper().Equals("PAYMENTHOLD"))
                    {

                        GetBooking objBooking = new GetBooking(Searchid, Companyid, BookingRef, bookingService);
                        return await objBooking.GetCommitRT(drOutbound.CopyToDataTable(), drInbound.CopyToDataTable(), dtPassengerInfo, dtComppanyInfo, dtGstInfo);
                    }
                    else
                    {
                        GetBookingAsHold objholdbooking = new GetBookingAsHold(Searchid, Companyid, BookingRef, bookingService);
                        return await objholdbooking.GetCommitRTAsHold(drOutbound.CopyToDataTable(), drInbound.CopyToDataTable(), dtPassengerInfo, dtComppanyInfo, dtGstInfo);
                    }
                    
                }
            
            return false;
        }
    }
}
