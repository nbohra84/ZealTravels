using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using ZealTravel.Domain.Interfaces.Handlers;
using ZealTravel.Application.AirlineSupplierManagement.Queries;
using AutoMapper;
using ZealTravel.Domain.Interfaces.AirlineSupplierManagement;
using ZealTravel.Application.AirlineSupplierManagement.Queries;

namespace ZealTravel.Application.AirlineSupplierManagement.Handlers
{
    public class GetAirSupplierQueryHandler : IHandlesQueryAsync<GetAirSupplierQuery, List<AirSupplier>>
    {
        private readonly IAirlineSupplierService _airlineSupplierServicee;
        private readonly IMapper _mapper;

        public GetAirSupplierQueryHandler(IAirlineSupplierService airlineSupplierServicee, IMapper mapper)
        {
            _airlineSupplierServicee = airlineSupplierServicee;
            _mapper = mapper;

        }

        public async Task<List<AirSupplier>> HandleAsync(GetAirSupplierQuery query)
        {
            var suppliers = await _airlineSupplierServicee.GetAirSupplierListAsync();
            return _mapper.Map<List<AirSupplier>>(suppliers);
        }

    }
}

