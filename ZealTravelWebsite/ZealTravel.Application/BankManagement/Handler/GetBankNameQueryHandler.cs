using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZealTravel.Application.BankManagement.Query;
using ZealTravel.Domain.Interfaces.Backoffice;
using ZealTravel.Domain.Interfaces.Handlers;

namespace ZealTravel.Application.BankManagement.Handler
{
    public class GetBankNameQueryHandler : IHandlesQueryAsync<BankNameQuery, List<GetBankNameQuery>>
    {
        private readonly IBankService _bankService;
        private readonly IMapper _mapper;

        public GetBankNameQueryHandler(IBankService bankService, IMapper mapper)
        {
            _bankService = bankService;
            _mapper = mapper;
        }

        public async Task<List<GetBankNameQuery>> HandleAsync(BankNameQuery query)
        {
            var bankDetails = await _bankService.GetBankNameCode();

            var bankDetailsQueryList = bankDetails.Select(b => new GetBankNameQuery
            {
                BankName = b.BankName,
                BankCode = b.BankCode
            }).ToList();

            return bankDetailsQueryList;
        }
    }
}
