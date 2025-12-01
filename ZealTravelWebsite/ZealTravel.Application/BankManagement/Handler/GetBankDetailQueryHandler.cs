using AutoMapper;
using System.Collections.Generic;
using System.Threading.Tasks;
using ZealTravel.Application.BankManagement.Query;
using ZealTravel.Domain.Interfaces.Handlers;
using ZealTravel.Domain.Interfaces.Backoffice;

namespace ZealTravel.Application.BankManagement.Handler
{
    public class GetBankDetailQueryHandler : IHandlesQueryAsync<GetBankDetailQuery, List<AdminBankDetails>>
    {
        private readonly IBankService _bankService;
        private readonly IMapper _mapper;

        public GetBankDetailQueryHandler(IBankService bankService, IMapper mapper)
        {
            _bankService = bankService;
            _mapper = mapper;
        }

        public async Task<List<AdminBankDetails>> HandleAsync(GetBankDetailQuery query)
        {
            if (string.IsNullOrEmpty(query.CompanyId))
            {
                throw new ArgumentException("CompanyId is required", nameof(query.CompanyId));
            }

            var bankDetails = await _bankService.GetBankDetails(query.CompanyId.ToString());

            if (bankDetails == null || bankDetails.Count == 0)
            {
                return new List<AdminBankDetails>(); 
            }

            return _mapper.Map<List<AdminBankDetails>>(bankDetails);
        }
    }

}
