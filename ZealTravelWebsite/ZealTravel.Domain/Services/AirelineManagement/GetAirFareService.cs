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
using ZealTravel.Domain.Interfaces.DBCommon;
using ZealTravel.Domain.Interfaces.FlightManagement.Akasaa;
using ZealTravel.Domain.Interfaces.Spicejet;
using ZealTravel.Domain.Interfaces.TBO;
using ZealTravel.Domain.Interfaces.UniversalAPI;
using ZealTravel.Domain.Models;
using ZealTravel.Domain.Services.AirelineManagement.AkashaAir;

namespace ZealTravel.Domain.Services.AirelineManagement
{
    public class GetAirFareService : IGetAirFareService
    {
        ITBOService _tboService;
        IUAPIServices _uapiService;
        IAkashaService _akashaService;
        ISpicejetService _spicejetService;

        public GetAirFareService(ITBOService tboService, IAkashaService akashaService, IUAPIServices uapiService, ISpicejetService spicejetService)
        {
            _tboService = tboService;
            _akashaService = akashaService;
            _uapiService = uapiService;
            _spicejetService = spicejetService;

        }

        public async Task<string> GetAirFareQuote(AirAvaibilityModel parameters)
        {
            string Supplier = CommonUtility.GetAirlineID(parameters.AirRQ);

            if (Supplier.Equals("OWN"))
            {
                DataSet dsAvailability = CommonFunction.StringToDataSet(parameters.AirRQ);
                foreach (DataTable table in dsAvailability.Tables)
                {
                    foreach (DataRow dr in table.Rows)
                    {
                        dr["IsPriceChanged"] = false;
                        dr["FareStatus"] = true;
                        dr["FareQuote"] = "Updated";
                    }
                }

                dsAvailability.AcceptChanges();
                return CommonFunction.DataSetToString(dsAvailability, "root");
            }
            //else if (Supplier.Equals("ZealTravels") || Supplier.Equals("MAAZ852"))
            //{
            //    api_tbo objtbo = new api_tbo(JourneyType, SearchID, CompanyID, Supplier, "API");
            //    return objtbo.GetAirFare(AirRS);
            //}
            else if (Supplier.Equals("P3822701") || Supplier.Equals("P7151745"))
            {
                // api_uapi objtbo = new api_uapi(JourneyType, SearchID, CompanyID, Supplier, "GDS");
                return await _uapiService.GetAirFareAsync(parameters, "GDS", Supplier);
               // return objtbo.GetAirFare(AirRS);
            }
            else if (Supplier.Equals("IGS2528") || Supplier.Equals("IGS2528"))
            {
                //api_uapi objtbo = new api_uapi(JourneyType, SearchID, CompanyID, Supplier, "LCC");
                //return objtbo.GetAirFare(AirRS);
                return await _uapiService.GetAirFare6EAsync(parameters, "LCC", Supplier);
            }
            else if (Supplier.Equals("MAAXT98402") || Supplier.Equals("CPNMAA0030") || Supplier.Equals("MAAXTA8402") || Supplier.Equals("APITESTID"))
            {
                //api_spicejet objuapi = new api_spicejet(JourneyType, SearchID, CompanyID, Supplier, "LCC");
                //return objuapi.GetAirFare(AirRS);
                return await _spicejetService.GetAirFareAsync(parameters);
            }
            else if (Supplier.Equals("QPMAA8752B"))
            {
                //api_qp objtbo = new api_qp(JourneyType, SearchID, CompanyID, Supplier, "LCC");
                return await _akashaService.GetAirFareAsync(parameters);
            }

            return string.Empty;
        }
    }
}
