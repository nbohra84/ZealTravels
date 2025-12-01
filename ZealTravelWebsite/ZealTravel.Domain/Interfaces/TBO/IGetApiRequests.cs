using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZealTravel.Domain.Interfaces.TBO
{
    public interface IGetApiRequests
    {
        /// <summary>
        /// Login Request
        /// </summary>
        /// <param name="UserName"></param>
        /// <param name="Password"></param>
        /// <param name="EndUserIp"></param>
        /// <returns></returns>
        public string GetLoginRequest(string UserName, string Password, string EndUserIp);
        public string GetAgencyBalanceRequest(string TokenId, string TokenMemberId, string TokenAgencyId, string EndUserIp);
        public string GetLogoutRequest(string TokenId, string TokenMemberId, string TokenAgencyId, string EndUserIp);
        public string GetOneWaySearchRequest(string Tokenid, string Origin, string Destination, string BeginDate, int Adt, int Chd, int Inf, string Cabin, ArrayList CarrierList, string EndUserIp);
        public string GetRoundWaySearchRequest(string Tokenid, string Origin, string Destination, string BeginDate, string EndDate, int Adt, int Chd, int Inf, string Cabin, ArrayList CarrierList, string EndUserIp);
        public string GetRoundWaySpecialDomesticSearchRequest(string Tokenid, string Origin, string Destination, string BeginDate, string EndDate, int Adt, int Chd, int Inf, string Cabin, ArrayList CarrierList, string EndUserIp);
        public string GetFareRuleFareQuoteSSRRequest(string Tokenid, string TraceId, string ResultIndex, string EndUserIp);
    }
}
