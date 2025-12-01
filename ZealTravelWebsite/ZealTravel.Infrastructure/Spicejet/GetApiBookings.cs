using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZealTravel.Infrastructure.Spicejet
{
    class GetApiBookings
    {
        public string errorMessage;
        //-----------------------------------------------------------------------------------------------
        //-----------------------------------------------------------------------------------------------
        private string Searchid;
        private string CompanyID;
        private Int32 BookingRef;
        private Int32 ContractVersion;
        //-----------------------------------------------------------------------------------------------
        public GetApiBookings(string Searchid, string CompanyID, Int32 BookingRef)
        {
            this.ContractVersion = 420;
            this.Searchid = Searchid;
            this.CompanyID = CompanyID;
            this.BookingRef = BookingRef;
        }
        //================================================================================================================================================
        public string GetBookingData(string Signature, string RecordLocator)
        {
            string GetApiRequest = string.Empty;
            string GetApiResponse = string.Empty;

            try
            {
                System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;
                svc_booking.GetBookingRequest objBookingRequest = new svc_booking.GetBookingRequest();
                objBookingRequest.Signature = Signature;
                objBookingRequest.ContractVersion = ContractVersion;
                objBookingRequest.GetBookingReqData = new svc_booking.GetBookingRequestData();
                objBookingRequest.GetBookingReqData.GetBookingBy = svc_booking.GetBookingBy.RecordLocator;
                objBookingRequest.GetBookingReqData.GetBookingBySpecified = true;

                objBookingRequest.GetBookingReqData.GetByRecordLocator = new svc_booking.GetByRecordLocator();
                objBookingRequest.GetBookingReqData.GetByRecordLocator.RecordLocator = RecordLocator;

                GetApiRequest = GetCommonFunctions.Serialize(objBookingRequest);

                svc_booking.IBookingManager objIBookingManager = new svc_booking.BookingManagerClient();
                svc_booking.GetBookingResponse objBookingResponse = objIBookingManager.GetBooking(objBookingRequest);

                GetApiResponse = GetCommonFunctions.Serialize(objBookingResponse);
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                //dbCommon.Logger.dbLogg(CompanyID, BookingRef, "GetBookingData", "air_spicejet", "", Searchid, ex.Message + "," + ex.StackTrace);
            }

            //dbCommon.Logger.WriteLogg(CompanyID, BookingRef, "GetBookingData-air_spicejet", "PNR", GetApiRequest + Environment.NewLine + GetApiResponse, RecordLocator, Searchid);
            return GetApiResponse;
        }
        public string GetBookingData(string Supplierid, string Password, string RecordLocator)
        {
            string GetApiRequest = string.Empty;
            string GetApiResponse = string.Empty;

            try
            {
                GetApiLogin objLogin = new GetApiLogin();
                string Signature = objLogin.GetSignature(Searchid, Supplierid, Password);
                if (Signature != null && Signature.Length > 0)
                {
                    System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;
                    svc_booking.GetBookingRequest objBookingRequest = new svc_booking.GetBookingRequest();
                    objBookingRequest.Signature = Signature;
                    objBookingRequest.ContractVersion = ContractVersion;
                    objBookingRequest.GetBookingReqData = new svc_booking.GetBookingRequestData();
                    objBookingRequest.GetBookingReqData.GetBookingBy = svc_booking.GetBookingBy.RecordLocator;
                    objBookingRequest.GetBookingReqData.GetBookingBySpecified = true;

                    objBookingRequest.GetBookingReqData.GetByRecordLocator = new svc_booking.GetByRecordLocator();
                    objBookingRequest.GetBookingReqData.GetByRecordLocator.RecordLocator = RecordLocator;

                    GetApiRequest = GetCommonFunctions.Serialize(objBookingRequest);

                    svc_booking.IBookingManager objIBookingManager = new svc_booking.BookingManagerClient();
                    svc_booking.GetBookingResponse objBookingResponse = objIBookingManager.GetBooking(objBookingRequest);

                    GetApiResponse = GetCommonFunctions.Serialize(objBookingResponse);
                }
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
               // dbCommon.Logger.dbLogg(CompanyID, BookingRef, "GetBookingData", "air_spicejet", Supplierid, Searchid, ex.Message + "," + ex.StackTrace);
            }

           // dbCommon.Logger.WriteLogg(CompanyID, BookingRef, "GetBookingData-air_spicejet", "PNR", GetApiRequest + Environment.NewLine + GetApiResponse, RecordLocator, Searchid);
            return GetApiResponse;
        }
        public string GetBookingDataBySignature(string Supplierid, string Signature, string RecordLocator)
        {
            string GetApiRequest = string.Empty;
            string GetApiResponse = string.Empty;

            try
            {
                //GetApiLogin objLogin = new GetApiLogin();
                //string Signature = objLogin.GetSignature(Searchid, Supplierid, Password);
                if (Signature != null && Signature.Length > 0)
                {
                    System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;
                    svc_booking.GetBookingRequest objBookingRequest = new svc_booking.GetBookingRequest();
                    objBookingRequest.Signature = Signature;
                    objBookingRequest.ContractVersion = ContractVersion;
                    objBookingRequest.GetBookingReqData = new svc_booking.GetBookingRequestData();
                    objBookingRequest.GetBookingReqData.GetBookingBy = svc_booking.GetBookingBy.RecordLocator;
                    objBookingRequest.GetBookingReqData.GetBookingBySpecified = true;

                    objBookingRequest.GetBookingReqData.GetByRecordLocator = new svc_booking.GetByRecordLocator();
                    objBookingRequest.GetBookingReqData.GetByRecordLocator.RecordLocator = RecordLocator;

                    GetApiRequest = GetCommonFunctions.Serialize(objBookingRequest);

                    svc_booking.IBookingManager objIBookingManager = new svc_booking.BookingManagerClient();
                    svc_booking.GetBookingResponse objBookingResponse = objIBookingManager.GetBooking(objBookingRequest);

                    GetApiResponse = GetCommonFunctions.Serialize(objBookingResponse);
                }
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                // dbCommon.Logger.dbLogg(CompanyID, BookingRef, "GetBookingData", "air_spicejet", Supplierid, Searchid, ex.Message + "," + ex.StackTrace);
            }

            // dbCommon.Logger.WriteLogg(CompanyID, BookingRef, "GetBookingData-air_spicejet", "PNR", GetApiRequest + Environment.NewLine + GetApiResponse, RecordLocator, Searchid);
            return GetApiResponse;
        }
    }
}
