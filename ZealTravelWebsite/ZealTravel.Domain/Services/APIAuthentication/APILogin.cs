using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using ZealTravel.Common;
using ZealTravel.Domain.Interfaces.APIAuthtentication;

namespace ZealTravel.Domain.Services.APIAuthentication
{
    //public class APILogin:IAPILogin
    //{
    //    public string errorMessage;
    //    public string GetAgencyid;
    //    public string GetMemberid;
    //    IAPIRequest _apiRequest;
    //    public APILogin(IAPIRequest apiRequest)
    //    {
    //        _apiRequest = apiRequest;
    //    }
    //    public string GetTokenid(string Supplierid, string Password, string Searchid, string Companyid, string EndUserIp)
    //    {
    //        string GetTokenid = string.Empty;
    //        string GetApiRequest = string.Empty;
    //        string GetApiResponse = string.Empty;

    //        try
    //        {
    //            GetApiRequest = _apiRequest.GetLoginRequest(Supplierid, Password, EndUserIp);

    //            GetApiCommonFunctions objApiCommonFunctions = new GetApiCommonFunctions();
    //            GetApiResponse = objApiCommonFunctions.GetApiHttpResponseRoot(Supplierid, Searchid, Companyid, 0, GetApiRequest, GetApiServiceURL.getauthenticate_url, "Authenticate");
    //            if (GetApiResponse != null && GetApiResponse.IndexOf("root") != -1)
    //            {
    //                XmlDocument xmldoc = new XmlDocument();
    //                xmldoc.LoadXml(GetApiResponse);
    //                GetTokenid = xmldoc.SelectSingleNode("root/TokenId").InnerText;
    //                GetAgencyid = xmldoc.SelectSingleNode("root/Member/AgencyId").InnerText;
    //                GetMemberid = xmldoc.SelectSingleNode("root/Member/MemberId").InnerText;
    //            }
    //        }
    //        catch (Exception ex)
    //        {
    //            errorMessage = ex.Message;
    //            dbCommon.Logger.dbLogg(Companyid, 0, "GetTokenid", "air_tbo-GetApiLogin", Supplierid, Searchid, errorMessage);
    //        }
    //        return GetTokenid;
    //    }
    //    public void OffTokenid(string Searchid, string Supplierid, string CompanyID, string TokenId, string TokenMemberId, string TokenAgencyId, string EndUserIp)
    //    {
    //        string GetTokenid = string.Empty;
    //        string GetApiRequest = string.Empty;
    //        string GetApiResponse = string.Empty;

    //        try
    //        {
    //            GetApiRequest = GetApiRequests.GetLogoutRequest(TokenId, TokenMemberId, TokenAgencyId, EndUserIp);

    //            GetApiCommonFunctions objApiCommonFunctions = new GetApiCommonFunctions();
    //            GetApiResponse = objApiCommonFunctions.GetApiHttpResponseRoot(Supplierid, Searchid, CompanyID, 0, GetApiRequest, GetApiServiceURL.getlogout_url, "Authenticate");
    //            if (GetApiResponse != null && GetApiResponse.IndexOf("root") != -1)
    //            {
    //                XmlDocument xmldoc = new XmlDocument();
    //                xmldoc.LoadXml(GetApiResponse);
    //            }
    //        }
    //        catch (Exception ex)
    //        {
    //            errorMessage = ex.Message;
    //            dbCommon.Logger.dbLogg(CompanyID, 0, "OffTokenid", "air_tbo-GetApiLogin", Supplierid, Searchid, errorMessage);
    //        }
    //    }
    //    public Decimal GetAvailableBalance(string Searchid, string Supplierid, string CompanyID, string Password, string EndUserIp)
    //    {
    //        Decimal ForeignAmount = 0;
    //        string GetApiRequest = string.Empty;
    //        string GetApiResponse = string.Empty;

    //        try
    //        {
    //            string Tokenid = GetTokenid(Searchid, Supplierid, CompanyID, Password, EndUserIp);
    //            if (Tokenid != null && Tokenid.Length > 0)
    //            {
    //                GetApiRequest = GetApiRequests.GetAgencyBalanceRequest(Tokenid, GetMemberid, GetAgencyid, EndUserIp);

    //                GetApiCommonFunctions objApiCommonFunctions = new GetApiCommonFunctions();
    //                GetApiResponse = objApiCommonFunctions.GetApiHttpResponseRoot(Supplierid, Searchid, CompanyID, 0, GetApiRequest, GetApiServiceURL.getAgencyBalance_url, "GetAgencyBalance");

    //                if (GetApiResponse != null && GetApiResponse.IndexOf("root") != -1)
    //                {
    //                    XmlDocument xmldoc = new XmlDocument();
    //                    xmldoc.LoadXml(GetApiResponse);
    //                }
    //            }
    //        }
    //        catch (Exception ex)
    //        {
    //            errorMessage = ex.Message;
    //            dbCommon.Logger.dbLogg(CompanyID, 0, "GetAvailableBalance", "air_tbo-GetApiLogin", Supplierid, Searchid, errorMessage);
    //        }
    //        return ForeignAmount;
    //    }
    //}
}
