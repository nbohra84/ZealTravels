using System.Threading.Tasks;
using ZealTravel.Application.UserManagement.Queries;
using ZealTravel.Domain.Interfaces.Handlers;
using ZealTravel.Domain.Interfaces.Whitelabel;

namespace ZealTravel.Application.Handlers
{
    public class GetWhitelabelAdminidFromHostQueryHandler : IHandlesQueryAsync<string , string>
    {
        private readonly IWhitelabelDetailRepository _whitelabelDetail;

        public GetWhitelabelAdminidFromHostQueryHandler(IWhitelabelDetailRepository whitelabelDetail)
        {
            _whitelabelDetail = whitelabelDetail;
        }

        public async Task<string> HandleAsync(string hostName)
        {
            if (hostName.IndexOf("www.") != -1)
            {
                hostName = hostName.Replace("www.", "").Trim();
            }
            var whitelabelDetail = await _whitelabelDetail.FindAsync(w => w.Host == hostName);
            return whitelabelDetail?.CompanyId;


        }
    }
}



