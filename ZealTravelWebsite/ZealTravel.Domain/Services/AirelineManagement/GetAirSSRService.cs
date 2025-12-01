using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZealTravel.Common;
using ZealTravel.Common.CommonUtility;
using ZealTravel.Domain.Interfaces.AirlineManagement;
using ZealTravel.Domain.Interfaces.Akasha;
using ZealTravel.Domain.Interfaces.Spicejet;
using ZealTravel.Domain.Interfaces.TBO;
using ZealTravel.Domain.Interfaces.UniversalAPI;
using ZealTravel.Domain.Models;

namespace ZealTravel.Domain.Services.AirelineManagement
{
    public class GetAirSSRService : IGetAirSSRService
    {
        ITBOService _tboService;
        IUAPIServices _uapiService;
        IAkashaService _akashaService;
        ISpicejetService _spicejetService;

        public GetAirSSRService(ITBOService tboService, IAkashaService akashaService, IUAPIServices uapiService, ISpicejetService spicejetService)
        {
            _tboService = tboService;
            _akashaService = akashaService;
            _uapiService = uapiService;
            _spicejetService = spicejetService; 
        }


        public async Task<string> GetAirSSR(AirAvaibilityModel parameters)
        {
            string TResponse = string.Empty;
            string Supplier = CommonUtility.GetAirlineID(parameters.AirRQ);

            if (Supplier.Equals("OWN"))
            {
                return "";
            }
            //else if (Supplier.Equals("ZealTravels") || Supplier.Equals("MAAZ852"))
            //{
            //    api_tbo objtbo = new api_tbo(JourneyType, SearchID, CompanyID, Supplier, "API");
            //    return objtbo.GetSSR(AirRS);
            //}
            else if (Supplier.Equals("P3822701") || Supplier.Equals("P7151745"))
            {
                return await _uapiService.GetAirSSRAsync(parameters, "GDS", Supplier);
            }
            else if (Supplier.Equals("IGS2528") || Supplier.Equals("IGS2528"))
            {
                return await _uapiService.GetAirSSR6EAsync(parameters, "LCC", Supplier);
            }
            else if (Supplier.Equals("MAAXT98402") || Supplier.Equals("CPNMAA0030") || Supplier.Equals("MAAXTA8402") || Supplier.Equals("APITESTID"))
            {
                return await _spicejetService.GetAirSSRAsync(parameters);

            }

                return TResponse;
        }
    }
}
