using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZealTravel.Infrastructure.Spicejet
{
    class GetApiApplyPromotionRequest
    {
        public string errorMessage;
        //-----------------------------------------------------------------------------------------------
        private string Searchid;
        private string CompanyID;
        private Int32 BookingRef;
        private Int32 ContractVersion;
        //-----------------------------------------------------------------------------------------------
        public GetApiApplyPromotionRequest(string Searchid, string CompanyID, Int32 BookingRef)
        {
            this.ContractVersion = 420;
            this.Searchid = Searchid;
            this.CompanyID = CompanyID;
            this.BookingRef = BookingRef;
        }
        //================================================================================================================================================
        public Decimal GetApplyPromotionStatus(string Supplierid, string Signature, string PromotionCode)
        {
            Decimal TotalCost = 0;
            string GetApiRequest = string.Empty;
            string GetApiResponse = string.Empty;

            try
            {
                System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;

                svc_booking.ApplyPromotionRequest objApplyPromotionRequest = new svc_booking.ApplyPromotionRequest();

                svc_booking.ApplyPromotionRequestData objApplyPromotionRequestData = new svc_booking.ApplyPromotionRequestData();
                objApplyPromotionRequestData.PromotionCode = PromotionCode;

                svc_booking.PointOfSale objPointOfSale = new svc_booking.PointOfSale();
                objPointOfSale.State = svc_booking.MessageState.New;
                objPointOfSale.StateSpecified = true;

                objApplyPromotionRequestData.SourcePointOfSale = objPointOfSale;
                objApplyPromotionRequestData.SourcePointOfSale.AgentCode = "AG"; ;
                objApplyPromotionRequestData.SourcePointOfSale.OrganizationCode = Supplierid;
                objApplyPromotionRequestData.SourcePointOfSale.DomainCode = "WWW";
                objApplyPromotionRequestData.SourcePointOfSale.LocationCode = string.Empty;
                objApplyPromotionRequestData.SourcePointOfSale.State = svc_booking.MessageState.New;
                objApplyPromotionRequestData.SourcePointOfSale.StateSpecified = true;

                objApplyPromotionRequest.ApplyPromotionReqData = objApplyPromotionRequestData;

                objApplyPromotionRequest.ContractVersion = ContractVersion;
                objApplyPromotionRequest.Signature = Signature;

                GetApiRequest = GetCommonFunctions.Serialize(objApplyPromotionRequest);

                svc_booking.ApplyPromotionResponse objApplyPromotionResponse = new svc_booking.ApplyPromotionResponse();
                svc_booking.IBookingManager objIBookingManager = new svc_booking.BookingManagerClient();
                objApplyPromotionResponse = objIBookingManager.ApplyPromotion(objApplyPromotionRequest);

                string RecordLocator = objApplyPromotionResponse.BookingUpdateResponseData.Success.RecordLocator;
                TotalCost = objApplyPromotionResponse.BookingUpdateResponseData.Success.PNRAmount.TotalCost;

                GetApiResponse = GetCommonFunctions.Serialize(objApplyPromotionResponse);
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
               // dbCommon.Logger.dbLogg(CompanyID, BookingRef, "GetApplyPromotionStatus", "air_spicejet", Supplierid, Searchid, ex.Message + "," + ex.StackTrace);
            }
           // dbCommon.Logger.WriteLogg(CompanyID, BookingRef, "GetApplyPromotionStatus-air_spicejet", "PNR", GetApiRequest + Environment.NewLine + GetApiResponse, Supplierid, Searchid);
            return TotalCost;
        }
    }
}
