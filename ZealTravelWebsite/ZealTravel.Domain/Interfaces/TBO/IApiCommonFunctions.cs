using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZealTravel.Domain.Interfaces.TBO
{
    public interface IApiCommonFunctions
    {
        public string errorMessage { get; set; }
        public string GetApiHttpResponse(string Supplierid, string Searchid, string Companyid, Int32 BookingRef, string Request, string ServiceUrl, string MethodName);
        public Task<string> GetApiHttpResponseAsync(string Supplierid, string Searchid, string Companyid, int BookingRef, string Request, string ServiceUrl, string MethodName);
        public string GetApiHttpResponseRoot(string Supplierid, string Searchid, string Companyid, Int32 BookingRef, string Request, string ServiceUrl, string MethodName);
    }
}
