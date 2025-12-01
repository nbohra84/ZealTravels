using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZealTravel.Common.Helpers;

namespace ZealTravel.Infrastructure.Spicejet
{
    class GetApiLogin
    {
        public string errorMessage;
        public string GetSignature(string Searchid, string Supplierid, string Password)
        {
            string Signature = string.Empty;
            string GetApiRequest = "";
            string GetApiResponse = "";

            try
            {
                string DomainCode = "WWW";
                int ContractVersion = 420;
                string LocationCode = "";

                if (ConfigurationHelper.GetSetting("ASPNETCORE_ENVIRONMENT").ToLower() != "production")
                {
                    Supplierid = "APITESTID";
                    Password = "Spice@123";
                }

                System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;

                svc_session.ISessionManager sessionManager = new svc_session.SessionManagerClient();

                svc_session.LogonRequest logonRequest = new svc_session.LogonRequest();
                logonRequest.logonRequestData = new svc_session.LogonRequestData();
                logonRequest.logonRequestData.DomainCode = DomainCode;
                logonRequest.logonRequestData.AgentName = Supplierid;  //    Supplierid;
                logonRequest.logonRequestData.Password = Password; // Password;
                logonRequest.ContractVersion = ContractVersion;

                GetApiRequest = GetCommonFunctions.Serialize(logonRequest);

                svc_session.LogonResponse logonResponse = sessionManager.Logon(logonRequest);

                GetApiResponse = GetCommonFunctions.Serialize(logonResponse);

                if (logonResponse != null && logonResponse.Signature != null && logonResponse.Signature != string.Empty)
                {
                    Signature = logonResponse.Signature;
                }
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                //dbCommon.Logger.dbLogg(Password, 0, "GetSignature", "air_spicejet", Supplierid, Searchid, ex.Message + "," + ex.StackTrace);
            }
           // dbCommon.Logger.WriteLogg("", 0, "GetSignature-air_spicejet", "LOGIN", GetApiRequest + Environment.NewLine + GetApiResponse, Supplierid, Searchid);
            return Signature;
        }
        public void OffSignature(string Searchid, string Signature)
        {
            try
            {
                int ContractVersion = 420;
                System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;

                svc_session.ISessionManager objISessionManager = new svc_session.SessionManagerClient();
                svc_session.LogoutRequest objLogoutRequest = new svc_session.LogoutRequest();
                objLogoutRequest.Signature = Signature;
                objLogoutRequest.ContractVersion = ContractVersion;
                svc_session.LogoutResponse objLogoutResponse = objISessionManager.Logout(objLogoutRequest);
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
               // dbCommon.Logger.dbLogg("", 0, "OffSignature", "air_spicejet", "", Searchid, ex.Message + "," + ex.StackTrace);
            }
        }
        public Int32 GetAvailableBalance(string Searchid, string DomainCode, string Supplierid, string Password)
        {
            Int32 ForeignAmount = 0;

            try
            {
                int ContractVersion = 420;
                string Signature = GetSignature(Searchid, Supplierid, Password);
                if (Signature != null && Signature.Length > 0)
                {
                    System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;

                    svc_account.IAccountManager accmgr = new svc_account.AccountManagerClient();
                    svc_account.GetAccountByReferenceRequest avlb = new svc_account.GetAccountByReferenceRequest();
                    avlb.Signature = Signature;
                    avlb.ContractVersion = ContractVersion;
                    svc_account.GetAccountByReferenceRequestData rqsdt = new svc_account.GetAccountByReferenceRequestData();
                    rqsdt.AccountReference = Supplierid;
                    rqsdt.CurrencyCode = "INR";
                    avlb.GetAccountByReferenceReqData = rqsdt;
                    svc_account.GetAccountByReferenceResponse accres = accmgr.GetAccountByReference(avlb);

                    if (accres != null)
                    {
                        ForeignAmount = decimal.ToInt32(accres.Account.ForeignAmount);
                    }
                }
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
               // dbCommon.Logger.dbLogg("", 0, "GetAvailableBalance", "air_spicejet", Supplierid, Searchid, ex.Message + "," + ex.StackTrace);
            }
            return ForeignAmount;
        }
        public Decimal GetAvailableBalance(string Searchid, string Signature, string Supplierid)
        {
            decimal ForeignAmount = 100000;
            //int ContractVersion= 420;
            //try
            //{
            //    if (Signature != null && Signature.Length > 0)
            //    {
            //        System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;

            //        svc_account.IAccountManager accmgr = new svc_account.AccountManagerClient();
            //        svc_account.GetAccountByReferenceRequest avlb = new svc_account.GetAccountByReferenceRequest();
            //        avlb.Signature = Signature;
            //        avlb.ContractVersion = ContractVersion;
            //        svc_account.GetAccountByReferenceRequestData rqsdt = new svc_account.GetAccountByReferenceRequestData();
            //        rqsdt.AccountReference = Supplierid;
            //        rqsdt.CurrencyCode = "INR";
            //        avlb.GetAccountByReferenceReqData = rqsdt;
            //        svc_account.GetAccountByReferenceResponse accres = accmgr.GetAccountByReference(avlb);

            //        if (accres != null)
            //        {
            //            ForeignAmount = accres.Account.ForeignAmount;
            //        }
            //    }
            //}
            //catch (Exception ex)
            //{
            //    errorMessage = ex.Message;
            //    dbCommon.Logger.dbLogg("", 0, "GetAvailableBalance", "air_spicejet", Supplierid, Searchid, ex.Message+ "," + ex.StackTrace);
            //}
            return ForeignAmount;
        }
    }
}
