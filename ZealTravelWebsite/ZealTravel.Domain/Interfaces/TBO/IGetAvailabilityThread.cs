using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZealTravel.Domain.Interfaces.TBO
{
    public interface IGetAvailabilityThread
    {
        public void GetOutbound();
        public Task<string> GetOutboundAsync();
        public void GetInbound();
        public Task<string> GetInboundAsync();
        public void GetRT();
        public Task<string> GetRT_Async();
        public void GetRTLCC();
        public Task<string> GetRTLCC_Async();
        public void GetMC();
        public Task<string> GetMC_Async();
    }
}
