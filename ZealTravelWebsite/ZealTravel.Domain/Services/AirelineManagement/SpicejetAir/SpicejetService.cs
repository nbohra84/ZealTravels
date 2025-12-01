using DocumentFormat.OpenXml.EMMA;
using DocumentFormat.OpenXml.ExtendedProperties;
using Microsoft.Identity.Client;
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
using ZealTravel.Domain.Interfaces.FlightManagement.Spicejet;
using ZealTravel.Domain.Interfaces.Spicejet;
using ZealTravel.Domain.Models;

namespace ZealTravel.Domain.Services.AirelineManagement.SpicejetAir
{
    public class SpicejetService: ISpicejetService
    {
        ICredential _credential;
        IGetServices _service;
        IDBLoggerService _dbLoggerService;

        public SpicejetService(ICredential credential, IGetServices service, IDBLoggerService dbLoggerService)
        {
            _credential = credential;
            _service = service;
            _dbLoggerService = dbLoggerService;
        }

        public async Task<string> GetAirAvailabilityAsync(AirAvaibilityModel model, string supplierType, string supplier, string sector, string priceType)
        {
            string apiResponse = string.Empty;
            try
            {

                var dtCred = await _credential.AirlineCredentialDetail<SupplierCREDDetailLccAirline>(supplierType, supplier);
                if (dtCred != null && dtCred.Any())
                {
                    var agentID = dtCred.FirstOrDefault().AgentID;
                    var password = dtCred.FirstOrDefault().Password;

                    apiResponse = await _service.GetFlightsAsync(model.JourneyType, agentID, password, priceType, model.Searchid, model.Companyid, model.AirRQ, sector);  
                }

            }
            
            catch (Exception ex)
            {
                await _dbLoggerService.dbLogg(model.Companyid, 0, "GetAirAvailability", "api_spicejet", model.AirRQ, model.Searchid, ex.Message);
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
                await _dbLoggerService.dbLogg(model.Companyid, 0, "GetAirFare", "api_spicejet", model.AirRQ, model.Searchid, ex.Message);
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
                await _dbLoggerService.dbLogg(model.Companyid, 0, "GetAirFareRule", "api_spicejet", model.AirRQ, model.Searchid, ex.Message);
            }
            return AirlineRules;
        }

        public async Task<string> GetAirSSRAsync(AirAvaibilityModel model)
        {
            string AirlineSSRList = string.Empty;
            try
            {
                AirlineSSRList = await _service.GetSSRAsync(model.JourneyType, model.Searchid, model.Companyid, model.AirRQ);
            }
            catch (Exception ex)
            {
                await _dbLoggerService.dbLogg(model.Companyid, 0, "GetAirSSR", "api_spicejet", model.AirRQ, model.Searchid, ex.Message);
            }
            return AirlineSSRList;
        }
        public async Task<bool> GetAirCommitAsync(AirAvaibilityModel model, int BookingRef, string PassengerRS, string CompanyRS, string GstRS, string PaymentType)
        {
            bool PNRstatus = false;
            try
            {
                string Selected_RS_Availability = CommonFunction.ClearFareRule(model.AirRQ);
                 PNRstatus = await _service.GetCommitAsync(model.JourneyType, model.Searchid, model.Companyid, BookingRef, Selected_RS_Availability, PassengerRS, CompanyRS, GstRS, PaymentType);
                //if (apiResponse != null && apiResponse.Length > 1)
                //{
                //    bool.TryParse(apiResponse, out PNRstatus);
                //}
            }
            catch (Exception ex)
            {
                await _dbLoggerService.dbLogg(model.Companyid, BookingRef , "GetCommit", "api_spicejet", model.AirRQ, model.Searchid, ex.Message);
            }
            return PNRstatus;
        }

        public async Task<bool> GetAirCommitForHoldBookingAsync(string Supplierid, string NetworkPassword, string SearchID, int BookingRef, string CompanyID, Decimal TotalApiFare, string RecordLocator, int Paxcount, string FltType)
        {
            bool confirmBooking = false;
            try
            {
                confirmBooking = await _service.GetCommitForHoldBookingAsync(Supplierid, NetworkPassword, SearchID, BookingRef, CompanyID, TotalApiFare, RecordLocator, Paxcount,FltType);
            }catch (Exception ex)
            {
                await _dbLoggerService.dbLogg(CompanyID, BookingRef, "GetCommit", "api_spicejet", "searchCriteria", SearchID, ex.Message);

            }
            return confirmBooking;
        }


        //public Task<string> GetAirFareQuoteAsync(AirAvaibilityModel model, string supplierType, string supplier, string sector, string fareRule)
        //{
        //    throw new NotImplementedException();
        //}
    }
}
