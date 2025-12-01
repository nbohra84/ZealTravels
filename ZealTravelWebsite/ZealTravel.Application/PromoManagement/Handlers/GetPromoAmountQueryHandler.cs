using System.Threading.Tasks;
using ZealTravel.Application.UserManagement.Queries;
using ZealTravel.Domain.Interfaces.Handlers;
using ZealTravel.Domain.Interfaces.PromoManagement;
using ZealTravel.Domain.Interfaces.Whitelabel;

namespace ZealTravel.Application.Handlers
{
    public class GetPromoAmountQueryHandler : IHandlesQueryAsync<string , int>
    {
        private readonly ICompanyPromoDetailRepository _promoDetail;

        public GetPromoAmountQueryHandler(ICompanyPromoDetailRepository promoDetail)
        {
            _promoDetail = promoDetail;
        }

        public async Task<int> HandleAsync(string promocode)
        {
            var promoDeal = 0;
            var promoDetail = await _promoDetail.FindAsync(w => w.Promocode == promocode);
            if (promoDetail != null)
            {
                promoDeal = promoDetail.PromoDeal.Value;
            }
            return promoDeal;
        }
    }
}



