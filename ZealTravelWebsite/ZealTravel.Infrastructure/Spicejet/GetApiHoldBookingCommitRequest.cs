using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZealTravel.Infrastructure.Spicejet
{
    class GetApiHoldBookingCommitRequest
    {
        public string errorMessage;
        //-----------------------------------------------------------------------------------------------
        //private string Searchid;
        //private string CompanyID;
        //private Int32 BookingRef;
        private Int32 ContractVersion;
        //-----------------------------------------------------------------------------------------------

        public GetApiHoldBookingCommitRequest(/*string Searchid, string CompanyID, Int32 BookingRef*/)
        {
            this.ContractVersion = 420;
            //this.Searchid = Searchid;
            //this.CompanyID = CompanyID;
            //this.BookingRef = BookingRef;
        }
        //================================================================================================================================================
        public string GetCommit(string Supplierid, string Signature,Int32 PaxCount,string RecordLocato)
        {
            string RecordLocator = string.Empty;

            string GetApiRequest = string.Empty;
            string GetApiResponse = string.Empty;
            //string Supplierid = string.Empty;

            try
            {
                //Supplierid = dtBound.Rows[0]["AirlineID"].ToString().Trim();
                //string Signature = dtBound.Rows[0]["Api_SessionID"].ToString().Trim();
                //int Adt = int.Parse(dtBound.Rows[0]["Adt"].ToString());
                //int Chd = int.Parse(dtBound.Rows[0]["Chd"].ToString());
                //int Inf = int.Parse(dtBound.Rows[0]["Inf"].ToString());
                //int PaxCount = Chd + Adt;

                System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;

                svc_booking.BookingCommitRequestData objBookingCommitRequestData = new svc_booking.BookingCommitRequestData();
                svc_booking.PointOfSale objPointOfSale = new svc_booking.PointOfSale();

                objPointOfSale.State = svc_booking.MessageState.New;
                objPointOfSale.StateSpecified = true;

                objBookingCommitRequestData.State = svc_booking.MessageState.New;
                objBookingCommitRequestData.StateSpecified = true;

                objBookingCommitRequestData.CurrencyCode = "INR";

                objBookingCommitRequestData.PaxCount = Convert.ToInt16(PaxCount.ToString());
                objBookingCommitRequestData.PaxCountSpecified = true;

                objBookingCommitRequestData.BookingID = 0;
                objBookingCommitRequestData.BookingIDSpecified = true;

                objBookingCommitRequestData.BookingParentID = 0;
                objBookingCommitRequestData.BookingParentIDSpecified = true;

                objBookingCommitRequestData.SourcePOS = objPointOfSale;
                objBookingCommitRequestData.SourcePOS.AgentCode = "AG"; ;
                objBookingCommitRequestData.SourcePOS.OrganizationCode = Supplierid;
                objBookingCommitRequestData.SourcePOS.DomainCode = "WWW";
                objBookingCommitRequestData.SourcePOS.LocationCode = string.Empty;

                objBookingCommitRequestData.RestrictionOverride = false;
                objBookingCommitRequestData.RestrictionOverrideSpecified = true;

                objBookingCommitRequestData.ChangeHoldDateTime = false;
                objBookingCommitRequestData.ChangeHoldDateTimeSpecified = true;


                objBookingCommitRequestData.WaiveNameChangeFee = false;
                objBookingCommitRequestData.WaiveNameChangeFeeSpecified = true;

                objBookingCommitRequestData.RecordLocator = RecordLocato;

                objBookingCommitRequestData.WaivePenaltyFee = false;
                objBookingCommitRequestData.WaivePenaltyFeeSpecified = true;

                objBookingCommitRequestData.WaiveSpoilageFee = false;
                objBookingCommitRequestData.WaiveSpoilageFeeSpecified = true;

                objBookingCommitRequestData.DistributeToContacts = true;
                objBookingCommitRequestData.DistributeToContactsSpecified = true;

                svc_booking.BookingCommitRequest objBookingCommitRequest = new svc_booking.BookingCommitRequest();
                objBookingCommitRequest.BookingCommitRequestData = objBookingCommitRequestData;
                objBookingCommitRequest.ContractVersion = ContractVersion;
                objBookingCommitRequest.Signature = Signature;

                GetApiRequest = GetCommonFunctions.Serialize(objBookingCommitRequest);

                svc_booking.IBookingManager objIBookingManager = new svc_booking.BookingManagerClient();
                svc_booking.BookingCommitResponse objBookingCommitResponse = objIBookingManager.BookingCommit(objBookingCommitRequest);
                if (objBookingCommitResponse != null && objBookingCommitResponse.BookingUpdateResponseData != null && objBookingCommitResponse.BookingUpdateResponseData.Success != null)
                {
                    RecordLocator = objBookingCommitResponse.BookingUpdateResponseData.Success.RecordLocator;
                }

                GetApiResponse = GetCommonFunctions.Serialize(objBookingCommitResponse);
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                //dbCommon.Logger.dbLogg(CompanyID, BookingRef, "GetCommit", "air_spicejet", Supplierid, Searchid, ex.Message + "," + ex.StackTrace);
            }
            //  dbCommon.Logger.WriteLogg(CompanyID, BookingRef, "GetCommit-air_spicejet", "PNR", GetApiRequest + Environment.NewLine + GetApiResponse, Supplierid, Searchid);
            return RecordLocator;
        }
    }
}
