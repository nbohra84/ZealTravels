using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZealTravel.Domain.Interfaces.AirlineManagement;
using ZealTravel.Domain.Interfaces.UniversalAPI;

namespace ZealTravel.Domain.Services.AirelineManagement
{
    public class GetAirlineBookingService : IGetAirlineBookingService
    {
        IUAPIServices _iuapiService;
        public GetAirlineBookingService(IUAPIServices iuapiService)
        {
            _iuapiService = iuapiService;
        }

        public async Task<string> GetAirlineBookingData(string universalRecordLocatorCode, string supplier, string searchID)
        {
            string BookingResponse = string.Empty;
            string supplierType = string.Empty;
            try
            {
                if (supplier.Equals("P3822701") || supplier.Equals("P7151745"))
                {
                    supplierType = "GDS";
                }
                else if (supplier.Equals("IGS2528") || supplier.Equals("IGS2528"))
                {
                    supplierType = "LCC";
                }

                BookingResponse = await _iuapiService.GetAirLineBookingStatusData(universalRecordLocatorCode, supplier,supplierType, searchID);

                if (!string.IsNullOrEmpty(BookingResponse))
                {
                    return BookingResponse;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            return BookingResponse;
        }
    }
}
