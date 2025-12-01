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
    public class GetAirFareRulesService : IGetAirFareRulesService
    {
        ITBOService _tboService;
        IUAPIServices _uapiService;
        IAkashaService _akashaService;
        ISpicejetService _spicejetService;

        public GetAirFareRulesService(ITBOService tboService, IAkashaService akashaService, IUAPIServices uapiService, ISpicejetService spicejetService)
        {
            _tboService = tboService;
            _akashaService = akashaService;
            _uapiService = uapiService;
            _spicejetService = spicejetService;
        }


        public async Task<string>  GetAirFareRules(AirAvaibilityModel parameters)
        {
            string Supplier = CommonUtility.GetAirlineID(parameters.AirRQ);

            if (Supplier.Equals("OWN"))
            {
                DataSet dsAvailability = CommonFunction.StringToDataSet(parameters.AirRQ);
                foreach (DataTable table in dsAvailability.Tables)
                {
                    foreach (DataRow dr in table.Rows)
                    {
                        dr["FareRule"] = "Contact to callcenter for more details";
                    }
                }

                dsAvailability.AcceptChanges();
                return CommonFunction.DataSetToString(dsAvailability, "root");
            }
            //else if (Supplier.Equals("ZealTravels") || Supplier.Equals("MAAZ852"))
            //{
            //    api_tbo objtbo = new api_tbo(JourneyType, SearchID, CompanyID, Supplier, "API");
            //    return objtbo.GetAirFareRule(AirRS);
            //}
            else if (Supplier.Equals("P3822701") || Supplier.Equals("P7151745"))
            {
                //api_uapi objKafila = new api_uapi(JourneyType, SearchID, CompanyID, Supplier, "GDS");
                //return objKafila.GetAirFareRule(AirRS);
                return await _uapiService.GetAirFareRulesAsync(parameters, "GDS", Supplier);
            }
            else if (Supplier.Equals("IGS2528") || Supplier.Equals("IGS2528"))
            {
                //api_uapi objKafila = new api_uapi(JourneyType, SearchID, CompanyID, Supplier, "LCC");
                //return objKafila.GetAirFareRule(AirRS);
                return await _uapiService.GetAirFareRules6EAsync(parameters, "LCC", Supplier);
            }
            else if (Supplier.Equals("MAAXT98402") || Supplier.Equals("CPNMAA0030") || Supplier.Equals("MAAXTA8402") || Supplier.Equals("APITESTID"))
            {
                //api_spicejet objuapi = new api_spicejet(JourneyType, SearchID, CompanyID, Supplier, "LCC");
                //return objuapi.GetAirFareRule(AirRS);
                return await _spicejetService.GetAirFareRulesAsync(parameters);
            }
            //}
            else if (Supplier.Equals("QPMAA8752B"))
            {

                return await _akashaService.GetAirFareRulesAsync(parameters);
            }



            return "";
        }
    }
}
