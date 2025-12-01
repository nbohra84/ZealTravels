using DocumentFormat.OpenXml.EMMA;
using DocumentFormat.OpenXml.ExtendedProperties;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZealTravel.Common;
using ZealTravel.Domain.Data.Entities;
using ZealTravel.Domain.Interfaces.Akasha;
using ZealTravel.Domain.Interfaces.DBCommon;
using ZealTravel.Domain.Interfaces.FlightManagement.Akasaa;
//using ZealTravel.Domain.Interfaces.TBO;
using ZealTravel.Domain.Models;
using ZealTravel.Domain.Services.AirelineManagement.AkashaAir;

namespace ZealTravel.Domain.Services.AirelineManagement.AkashaAir
{
    public class AkashaService: IAkashaService
    {
        ICredential _credential;
        IGetServices _service;
        IDBLoggerService _dbLoggerService;
        public AkashaService(ICredential credential, IGetServices service, IDBLoggerService dbLoggerService)
        {
            _credential = credential;
            _service = service;
            _dbLoggerService = dbLoggerService;
        }

        public async Task<string> GetAirAvailabilityAsync(AirAvaibilityModel model, string supplierType, string supplier, string sector)
        {
            string apiResponse = string.Empty;
            try
            {

                var dtCred = await _credential.AirlineCredentialDetail<SupplierCREDDetailLccAirline>(supplierType, supplier);
                if (dtCred != null && dtCred.Any())
                {
                    apiResponse = await _service.GetFlightsAsync(model.JourneyType, dtCred.FirstOrDefault().SupplierID, model.Searchid, model.Companyid, model.AirRQ);  
                }

            }
            
            catch (Exception ex)
            {
               await _dbLoggerService.dbLogg(model.Companyid, 0, "GetAirAvailability", "api_QP", model.AirRQ, model.Searchid, ex.Message);
            }
            return apiResponse;
        }

        public async Task<string> GetAirFareAsync(AirAvaibilityModel model)
        {
            string fareResponse = string.Empty;
            try
            {
                fareResponse = await _service.GetFareAsync(model.JourneyType, model.Searchid, model.Companyid, model.AirRQ);
                
            }
            catch (Exception ex)
            {
                await _dbLoggerService.dbLogg(model.Companyid, 0, "GetAirFare", "api_QP", model.AirRQ, model.Searchid, ex.Message);
            }
            return fareResponse;
        }

       
        public async Task<string> GetAirFareRulesAsync(AirAvaibilityModel model)
        {
            string AirlineRules = string.Empty;
            try
            {
              
                AirlineRules = await _service.GetFareRuleAsync(model.JourneyType, model.Searchid, model.Companyid, model.AirRQ);
                //objSvcs.Close();
            }
            catch (Exception ex)
            {
                await _dbLoggerService.dbLogg(model.Companyid, 0, "GetAirFareRule", "api_QP", model.AirRQ, model.Searchid, ex.Message);
            }
            return AirlineRules;
        }

        public async Task<bool> GetAirCommitAsync(AirAvaibilityModel model, int BookingRef, string PassengerRS, string CompanyRS, string GstRS)
        {
            bool PNRstatus = false;
            try
            { 
                string Selected_RS_Availability = CommonFunction.ClearFareRule(model.AirRQ);
                PNRstatus = await _service.GetCommitAsync(model.JourneyType, model.Searchid, model.Companyid, BookingRef, Selected_RS_Availability, PassengerRS, CompanyRS, GstRS);
            }
            catch (Exception ex)
            {
                await _dbLoggerService.dbLogg(model.Companyid, BookingRef , "air_api_collector", "api_QP", model.AirRQ, model.Searchid, ex.Message);
            }
            return PNRstatus;
        }


        //public Task<string> GetAirFareQuoteAsync(AirAvaibilityModel model, string supplierType, string supplier, string sector, string fareRule)
        //{
        //    throw new NotImplementedException();
        //}
    }
}
