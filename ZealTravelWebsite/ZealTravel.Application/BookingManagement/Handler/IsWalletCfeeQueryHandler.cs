using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZealTravel.Application.BookingManagement.Query;
using ZealTravel.Domain.BookingManagement;
using ZealTravel.Domain.Interfaces.Handlers;

namespace ZealTravel.Application.BookingManagement.Handler
{
    public class IsWalletCfeeQueryHandler : IHandlesQueryAsync<IsWalletCfeeQuery, bool>
    {
        private readonly IBookingManagementService _bookingManagementService;
        public IsWalletCfeeQueryHandler(IBookingManagementService bookingManagementService)
        {
            _bookingManagementService = bookingManagementService;
        }
        public async Task<bool> HandleAsync(IsWalletCfeeQuery isWalletCfeeQuery)
        {
            try
            {
                var status = await _bookingManagementService.IsWalletCfee(isWalletCfeeQuery.CompanyID, isWalletCfeeQuery.BookingType, isWalletCfeeQuery.Sector);
                return status;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error handling IsWalletCfee : {ex.Message}", ex);
            }
        }
    }
}

