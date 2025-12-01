using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZealTravel.Domain.Data.Entities;
using ZealTravel.Domain.Interfaces.DBCommon;
using ZealTravel.Domain.Interfaces.TBO;
using ZealTravel.Domain.Models;

namespace ZealTravel.Domain.Services.AirelineManagement.TBO
{
    public class TBOService:ITBOService
    {
        IGetServices _tboService;
        ICredential _credential;
        public TBOService(IGetServices tboService,ICredential cred) { _tboService = tboService; _credential = cred; }
        //public async string GetAirAvailability(AirAvaibilityModel model)
        //{
        //    string apiResponse = string.Empty;
        //    try
        //    {
        //        var dtCred = await _credential.AirlineCredentialDetail<SupplierDetailApiAirline>(model.SupplierType, model.Supplierid);
        //        if (dtCred != null && dtCred.Count > 0)
        //        {
        //            //svc_tbo.ItboServiceClient ojSvcs = new svc_tbo.ItboServiceClient();
        //           // tboService ojSvcs = new tboService();
        //            apiResponse = _tboService.GetFlights(model.JourneyType, dtCred.FirstOrDefault()?.UserId, dtCred.FirstOrDefault()?.Password.ToString(), model.Searchid, model.Companyid, model.AirRQ, model.EndUserIp);
        //            //ojSvcs.Close();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        //DBCommon.Logger.dbLogg(model.Companyid, 0, "GetAirAvailability", "api_Tbo", model.AirRQ, model.Searchid, ex.Message);
        //    }
        //    return apiResponse;
        //}
        public async Task<string> GetAirAvailabilityAsync(AirAvaibilityModel model)
        {
            string apiResponse = string.Empty;
            try
            {
                
                var supplierDetails = _credential.GetSuppliers().Where(x=>x.Supplier_Type=="API");
                if (supplierDetails.Any())
                {
                    var supp = supplierDetails.FirstOrDefault();
                    var dtCred = await _credential.AirlineCredentialDetail<SupplierDetailApiAirline>(supp.Supplier_Type, supp.SupplierID);
                    if (dtCred != null && dtCred.Count > 0)
                    {
                        //svc_tbo.ItboServiceClient ojSvcs = new svc_tbo.ItboServiceClient();
                        //tboService ojSvcs = new tboService();
                        apiResponse = await _tboService.GetFlightsAsync(model.JourneyType, dtCred.FirstOrDefault()?.UserId.ToString(), dtCred.FirstOrDefault()?.Password.ToString(), model.Searchid, model.Companyid, model.AirRQ, model.EndUserIp);
                        //ojSvcs.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                //dbCommon.Logger.dbLogg(CompanyID, 0, "GetAirAvailability", "api_Tbo", AirRQ, SearchID, ex.Message);
            }
            return apiResponse;
        }

    }
}
