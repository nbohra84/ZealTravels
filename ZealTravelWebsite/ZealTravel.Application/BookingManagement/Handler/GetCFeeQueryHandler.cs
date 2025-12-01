using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZealTravel.Application.BookingManagement.Query;
using ZealTravel.Domain.BookingManagement;
using ZealTravel.Domain.Interfaces.Handlers;

namespace ZealTravel.Application.BookingManagement.Handler
{
    public class GetCFeeQueryHandler : IHandlesQueryAsync<GetCfeeQuery, DataTable>
    {
        private readonly IBookingManagementService _bookingManagementService;
        public GetCFeeQueryHandler(IBookingManagementService bookingManagementService)
        {
            _bookingManagementService = bookingManagementService;
        }

        public async Task<DataTable> HandleAsync(GetCfeeQuery query)
        {
            return await _bookingManagementService.GetCfee(query.CompanyID, query.BookingType, query.Sector);
        }
    }
}
