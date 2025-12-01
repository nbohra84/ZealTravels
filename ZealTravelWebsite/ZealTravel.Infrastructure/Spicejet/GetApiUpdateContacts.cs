using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ZealTravel.Infrastructure.Spicejet
{
    class GetApiUpdateContacts
    {
        public string errorMessage;
        //-----------------------------------------------------------------------------------------------
        private string Searchid;
        private string CompanyID;
        private Int32 BookingRef;
        private Int32 ContractVersion;
        //-----------------------------------------------------------------------------------------------
        public GetApiUpdateContacts(string Searchid, string CompanyID, Int32 BookingRef)
        {
            this.ContractVersion = 420;
            this.Searchid = Searchid;
            this.CompanyID = CompanyID;
            this.BookingRef = BookingRef;
        }
        public bool GetUpdateContacts(DataTable dtBound, DataTable dtPassenger, DataTable dtCompany, DataTable dtGst)
        {
            string RecordLocator = string.Empty;
            string GetApiRequest = string.Empty;
            string GetApiResponse = string.Empty;
            string Supplierid = string.Empty;

            try
            {
                Supplierid = dtBound.Rows[0]["AirlineID"].ToString().Trim();
                string Signature = dtBound.Rows[0]["Api_SessionID"].ToString().Trim();
                int Adt = int.Parse(dtBound.Rows[0]["Adt"].ToString());
                int Chd = int.Parse(dtBound.Rows[0]["Chd"].ToString());
                int Inf = int.Parse(dtBound.Rows[0]["Inf"].ToString());
                int PaxCount = Chd + Adt;

                string CompanyName = dtCompany.Rows[0]["CompanyName"].ToString().Trim().ToUpper();
                string PostalCode = dtCompany.Rows[0]["PostalCode"].ToString().Trim().ToUpper();
                string StateCode = dtCompany.Rows[0]["StateCode"].ToString().Trim().ToUpper();
                string CountryCode = dtCompany.Rows[0]["CountryCode"].ToString().Trim().ToUpper();
                string Email = dtCompany.Rows[0]["Email"].ToString().Trim().ToUpper();
                string CityName = dtCompany.Rows[0]["CityName"].ToString().Trim().ToUpper();
                string Mob = dtCompany.Rows[0]["MobileNo"].ToString().Trim().ToUpper();
                string CompanyAddress = dtCompany.Rows[0]["Address"].ToString().Trim().ToUpper();
                if (CompanyAddress.Length > 52)
                {
                    CompanyAddress = CompanyAddress.Substring(0, 52).Trim();
                }
                CompanyAddress = Regex.Replace(CompanyAddress, @"\r\n?|\n|\t", " ");

                bool IsGSTApplicable = false;
                string GSTCompanyAddress = string.Empty;
                string GSTCompanyContactNumber = string.Empty;
                string GSTCompanyName = string.Empty;
                string GSTNumber = string.Empty;
                string GSTCompanyEmail = string.Empty;
                if (dtGst != null && dtGst.Rows.Count > 0)
                {
                    GSTCompanyAddress = dtGst.Rows[0]["GSTCompanyAddress"].ToString().Trim().ToUpper();
                    GSTCompanyContactNumber = dtGst.Rows[0]["GSTCompanyContactNumber"].ToString().Trim().ToUpper();
                    GSTCompanyName = dtGst.Rows[0]["GSTCompanyName"].ToString().Trim().ToUpper();
                    GSTNumber = dtGst.Rows[0]["GSTNumber"].ToString().Trim().ToUpper();
                    GSTCompanyEmail = dtGst.Rows[0]["GSTCompanyEmail"].ToString().Trim().ToUpper();

                    if (GSTNumber.Length > 0 && GSTCompanyEmail.Length > 0 && GSTCompanyName.Length > 0)
                    {
                        IsGSTApplicable = true;
                    }
                }

                System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;

                svc_booking.UpdateContactsRequestData objUpdateContactsRequestData = new svc_booking.UpdateContactsRequestData();
                if (IsGSTApplicable)
                {
                    objUpdateContactsRequestData.BookingContactList = new svc_booking.BookingContact[2];
                }
                else
                {
                    objUpdateContactsRequestData.BookingContactList = new svc_booking.BookingContact[1];
                }

                objUpdateContactsRequestData.BookingContactList[0] = new svc_booking.BookingContact();
                objUpdateContactsRequestData.BookingContactList[0].State = svc_booking.MessageState.New;
                objUpdateContactsRequestData.BookingContactList[0].StateSpecified = true;

                objUpdateContactsRequestData.BookingContactList[0].TypeCode = "P";
                objUpdateContactsRequestData.BookingContactList[0].Names = new svc_booking.BookingName[1];
                objUpdateContactsRequestData.BookingContactList[0].Names[0] = new svc_booking.BookingName();

                objUpdateContactsRequestData.BookingContactList[0].Names[0].Title = dtPassenger.Rows[0]["Title"].ToString().Trim().ToUpper();
                objUpdateContactsRequestData.BookingContactList[0].Names[0].FirstName = dtPassenger.Rows[0]["First_Name"].ToString().Trim().ToUpper();
                objUpdateContactsRequestData.BookingContactList[0].Names[0].MiddleName = "";
                objUpdateContactsRequestData.BookingContactList[0].Names[0].LastName = dtPassenger.Rows[0]["Last_Name"].ToString().Trim().ToUpper();

                objUpdateContactsRequestData.BookingContactList[0].EmailAddress = Email;
                objUpdateContactsRequestData.BookingContactList[0].CompanyName = CompanyName;
                objUpdateContactsRequestData.BookingContactList[0].AddressLine1 = CompanyAddress;
                objUpdateContactsRequestData.BookingContactList[0].City = CityName;

                objUpdateContactsRequestData.BookingContactList[0].WorkPhone = Mob;
                objUpdateContactsRequestData.BookingContactList[0].HomePhone = Mob;
                objUpdateContactsRequestData.BookingContactList[0].OtherPhone = Mob;

                objUpdateContactsRequestData.BookingContactList[0].ProvinceState = StateCode;
                objUpdateContactsRequestData.BookingContactList[0].PostalCode = PostalCode;
                objUpdateContactsRequestData.BookingContactList[0].CountryCode = CountryCode;
                objUpdateContactsRequestData.BookingContactList[0].CultureCode = "en-GB";

                objUpdateContactsRequestData.BookingContactList[0].DistributionOption = svc_booking.DistributionOption.Email;
                objUpdateContactsRequestData.BookingContactList[0].DistributionOptionSpecified = true;

                objUpdateContactsRequestData.BookingContactList[0].NotificationPreference = svc_booking.NotificationPreference.None;
                objUpdateContactsRequestData.BookingContactList[0].NotificationPreferenceSpecified = true;

                if (IsGSTApplicable)
                {
                    objUpdateContactsRequestData.BookingContactList[1] = new svc_booking.BookingContact();
                    objUpdateContactsRequestData.BookingContactList[1].State = svc_booking.MessageState.New;
                    objUpdateContactsRequestData.BookingContactList[1].StateSpecified = true;

                    objUpdateContactsRequestData.BookingContactList[1].TypeCode = "G";

                    objUpdateContactsRequestData.BookingContactList[1].EmailAddress = GSTCompanyEmail;
                    objUpdateContactsRequestData.BookingContactList[1].CompanyName = GSTCompanyName;
                    objUpdateContactsRequestData.BookingContactList[1].CustomerNumber = GSTNumber;
                    objUpdateContactsRequestData.BookingContactList[1].WorkPhone = GSTCompanyContactNumber;


                    objUpdateContactsRequestData.BookingContactList[1].DistributionOption = svc_booking.DistributionOption.Email;
                    objUpdateContactsRequestData.BookingContactList[1].DistributionOptionSpecified = true;

                    objUpdateContactsRequestData.BookingContactList[1].NotificationPreference = svc_booking.NotificationPreference.None;
                    objUpdateContactsRequestData.BookingContactList[1].NotificationPreferenceSpecified = true;
                }

                svc_booking.UpdateContactsRequest objUpdateContactsRequest = new svc_booking.UpdateContactsRequest();
                objUpdateContactsRequest.updateContactsRequestData = objUpdateContactsRequestData;
                objUpdateContactsRequest.ContractVersion = ContractVersion;
                objUpdateContactsRequest.Signature = Signature;

                GetApiRequest = GetCommonFunctions.Serialize(objUpdateContactsRequest);

                svc_booking.IBookingManager objIBookingManager = new svc_booking.BookingManagerClient();
                svc_booking.UpdateContactsResponse GetUpdateContactsResponse = objIBookingManager.UpdateContacts(objUpdateContactsRequest);
                if (GetUpdateContactsResponse != null && GetUpdateContactsResponse.BookingUpdateResponseData != null && GetUpdateContactsResponse.BookingUpdateResponseData.Success != null)
                {
                    RecordLocator = GetUpdateContactsResponse.BookingUpdateResponseData.Success.RecordLocator;
                }

                GetApiResponse = GetCommonFunctions.Serialize(GetUpdateContactsResponse);
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                //dbCommon.Logger.dbLogg(CompanyID, BookingRef, "GetUpdateContacts", "air_spicejet", Supplierid, Searchid, ex.Message + "," + ex.StackTrace);
            }

           // dbCommon.Logger.WriteLogg(CompanyID, BookingRef, "GetUpdateContacts-air_spicejet", "PNR", GetApiRequest + Environment.NewLine + GetApiResponse, Supplierid, Searchid);
            if (RecordLocator.Equals(string.Empty))
            {
                return true;
            }
            return false;
        }
    }
}
