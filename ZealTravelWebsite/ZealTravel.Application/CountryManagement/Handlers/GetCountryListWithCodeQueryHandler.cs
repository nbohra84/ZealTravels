using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZealTravel.Application.BookingManagement.Query;
using ZealTravel.Application.CountryManagement.Queries;
using ZealTravel.Domain.BookingManagement;
using ZealTravel.Domain.Interfaces.CountryManagement;
using ZealTravel.Domain.Interfaces.Handlers;
using ZealTravel.Infrastructure.Services.Country;

namespace ZealTravel.Application.CountryManagement.Handlers
{
    public class GetCountryListWithCodeQueryHandler : IHandlesQueryAsync<List<CountryList>>
    {
        private readonly ICountryService _countryService;
        private readonly IMapper _mapper;

        public GetCountryListWithCodeQueryHandler(ICountryService countryService, IMapper mapper)
        {
            _countryService = countryService;
            _mapper = mapper;
        }

        public async Task<List<CountryList>> HandleAsync()
        {
            var data = await _countryService.GetCountryListWithCode();
            return _mapper.Map<List<CountryList>>(data);
        }
    }
}
