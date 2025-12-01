using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZealTravel.Domain.Interfaces.UniversalAPI
{
    public interface IUniversalApi
    {
        public string GetRequestMC(ArrayList AirlineList, string Cabin, Int16 Adult, Int16 Child, Int16 Infant);
        public string GetRequest(string DepartureStation, string ArrivalStation, string Cabin, ArrayList AirlineList, string BeginDate, string ArrivalDate, int Adult, int Child, int Infant);
        public string GetResponseUapi(string NetworkUserName, string NetworkPassword, string SearchID, string requestData, string Method, string ServiceName);
        public Task<string> GetResponseUapiAsync(string NetworkUserName, string NetworkPassword, string SearchID, string requestData, string Method, string ServiceName);
    }
}
