using DocumentFormat.OpenXml.Bibliography;
using DocumentFormat.OpenXml.EMMA;
using DocumentFormat.OpenXml.ExtendedProperties;
using DocumentFormat.OpenXml.Spreadsheet;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZealTravel.Common.CommonUtility;
using ZealTravel.Domain.Data.Entities;
using ZealTravel.Domain.Interfaces.DBCommon;
using ZealTravel.Domain.Interfaces.UniversalAPI;
using ZealTravel.Domain.Models;

namespace ZealTravel.Domain.Services.AirelineManagement.UniversalAPI
{
    public class UAPIServices : IUAPIServices
    {
        ICredential _credential;
        IGetServices _service;
        IGetConfirmForHoldBooking _getConfirmForHoldBooking;
        public UAPIServices(ICredential credential, IGetServices service, IGetConfirmForHoldBooking getConfirmForHoldBooking)
        {
            _credential = credential;
            _service = service;
            _getConfirmForHoldBooking = getConfirmForHoldBooking;
        }
        public async Task<string> GetAirAvailabilityAsync(AirAvaibilityModel model, string supplierType, string supplier, string sector, bool uapiSME = false)
        {
            string apiResponse = string.Empty;
            try
            {
                var dtCred = await _credential.AirlineCredentialDetail<SupplierCREDGalileoAirline>(supplierType, supplier);
                if (dtCred != null && dtCred.Count > 0)
                {
                    var dr = dtCred.FirstOrDefault();
                    String username = dr.NetworkUserName;
                    String password = dr.NetworkPassword;

                    apiResponse = await _service.GetFlightsAsync(model.JourneyType, username, password, dr.TargetBranch.ToString(), model.Searchid, model.Companyid, model.AirRQ, sector, uapiSME);

                }
            }
            catch (Exception ex)
            {
                //Common.Logger.dbLogg(CompanyID, 0, "GetAirAvailability", "api_uapi", AirRQ, SearchID, ex.Message);
            }
            return apiResponse;
        }

        public async Task<string> GetAirAvailability6EAsync(AirAvaibilityModel model, string supplierType, string supplier, string sector)
        {
            string apiResponse = string.Empty;
            try
            {
                var dtCred = await _credential.AirlineCredentialDetail<_6EModel>(supplierType, supplier);
                if (dtCred != null && dtCred.Count > 0)
                {
                    var dr = dtCred.FirstOrDefault();
                    String username = dr.NetworkUserName;
                    String password = dr.NetworkPassword;
                    apiResponse = await _service.GetFlightsAsync(model.JourneyType, username, password, dr.TargetBranch.ToString(), model.Searchid, model.Companyid, model.AirRQ, sector);
                }
            }
            catch (Exception ex)
            {
                //Common.Logger.dbLogg(CompanyID, 0, "GetAirAvailability", "api_uapi", AirRQ, SearchID, ex.Message);
            }
            return apiResponse;
        }

        public async Task<string> GetAirFareRules6EAsync(AirAvaibilityModel model, string supplierType, string supplier)
        {
            string airlineRules = string.Empty;
            try
            {
                var dtCred = await _credential.AirlineCredentialDetail<_6EModel>(supplierType, supplier);
                if (dtCred != null && dtCred.Count > 0)
                {
                    var dr = dtCred.FirstOrDefault();
                    String username = dr.NetworkUserName;
                    String password = dr.NetworkPassword;

                    airlineRules = await _service.GetFareRule(model.JourneyType, username, password, dr.TargetBranch.ToString(), model.Searchid, model.Companyid, model.AirRQ);

                }
            }
            catch (Exception ex)
            {
                //Common.Logger.dbLogg(CompanyID, 0, "GetAirAvailability", "api_uapi", AirRQ, SearchID, ex.Message);
            }
            return airlineRules;
        }

        public async Task<string> GetAirFareRulesAsync(AirAvaibilityModel model, string supplierType, string supplier)
        {
            string airlineRules = string.Empty;
            try
            {
                var dtCred = await _credential.AirlineCredentialDetail<SupplierCREDGalileoAirline>(supplierType, supplier);
                if (dtCred != null && dtCred.Count > 0)
                {
                    var dr = dtCred.FirstOrDefault();
                    String username = dr.NetworkUserName;
                    String password = dr.NetworkPassword;

                    airlineRules = await _service.GetFareRule(model.JourneyType, username, password, dr.TargetBranch.ToString(), model.Searchid, model.Companyid, model.AirRQ);

                }
            }
            catch (Exception ex)
            {
                //Common.Logger.dbLogg(CompanyID, 0, "GetAirAvailability", "api_uapi", AirRQ, SearchID, ex.Message);
            }
            return airlineRules;
        }

        public async Task<string> GetAirFare6EAsync(AirAvaibilityModel model, string supplierType, string supplier)
        {
            string fareResponse = string.Empty;
            try
            {
                var dtCred = await _credential.AirlineCredentialDetail<_6EModel>(supplierType, supplier);
                if (dtCred != null && dtCred.Count > 0)
                {
                    var dr = dtCred.FirstOrDefault();
                    String username = dr.NetworkUserName;
                    String password = dr.NetworkPassword;

                    fareResponse = await _service.GetFare(model.JourneyType, username, password, dr.TargetBranch.ToString(), model.Searchid, model.Companyid, model.AirRQ);

                }
            }
            catch (Exception ex)
            {
                //Common.Logger.dbLogg(CompanyID, 0, "GetAirAvailability", "api_uapi", AirRQ, SearchID, ex.Message);
            }
            return fareResponse;
        }

        public async Task<string> GetAirFareAsync(AirAvaibilityModel model, string supplierType, string supplier)
        {
            string fareResponse = string.Empty;
            try
            {
                var dtCred = await _credential.AirlineCredentialDetail<SupplierCREDGalileoAirline>(supplierType, supplier);
                if (dtCred != null && dtCred.Count > 0)
                {
                    var dr = dtCred.FirstOrDefault();
                    String username = dr.NetworkUserName;
                    String password = dr.NetworkPassword;

                    fareResponse = await _service.GetFare(model.JourneyType, username, password, dr.TargetBranch.ToString(), model.Searchid, model.Companyid, model.AirRQ);

                }
            }
            catch (Exception ex)
            {
                //Common.Logger.dbLogg(CompanyID, 0, "GetAirAvailability", "api_uapi", AirRQ, SearchID, ex.Message);
            }
            return fareResponse;
        }

        public async Task<string> GetAirSSR6EAsync(AirAvaibilityModel model, string supplierType, string supplier)
        {
            string ssrResponse = string.Empty;
            try
            {
                var dtCred = await _credential.AirlineCredentialDetail<_6EModel>(supplierType, supplier);
                if (dtCred != null && dtCred.Count > 0)
                {
                    var dr = dtCred.FirstOrDefault();
                    String username = dr.NetworkUserName;
                    String password = dr.NetworkPassword;

                    ssrResponse = await _service.GetSSR(model.JourneyType, username, password, dr.TargetBranch.ToString(), model.Searchid, model.Companyid, model.AirRQ);

                }
            }
            catch (Exception ex)
            {
                //Common.Logger.dbLogg(CompanyID, 0, "GetAirAvailability", "api_uapi", AirRQ, SearchID, ex.Message);
            }
            return ssrResponse;
        }

        public async Task<string> GetAirSSRAsync(AirAvaibilityModel model, string supplierType, string supplier)
        {
            string ssrResponse = string.Empty;
            try
            {
                var dtCred =await _credential.AirlineCredentialDetail<SupplierCREDGalileoAirline>(supplierType, supplier);
                if (dtCred != null && dtCred.Count > 0)
                {
                    var dr = dtCred.FirstOrDefault();
                    String username = dr.NetworkUserName;
                    String password = dr.NetworkPassword;

                    ssrResponse = await _service.GetSSR(model.JourneyType, username, password, dr.TargetBranch.ToString(), model.Searchid, model.Companyid, model.AirRQ);

                }
            }
            catch (Exception ex)
            {
                //Common.Logger.dbLogg(CompanyID, 0, "GetAirAvailability", "api_uapi", AirRQ, SearchID, ex.Message);
            }
            return ssrResponse;
        }

        public async Task<bool> GetAirCommit6EAsync(AirAvaibilityModel model, string supplierType, string supplier, int BookingRef, string PassengerRS, string CompanyRS, string GstRS, string PaymentType)
        {
            string apiResponse = string.Empty;
            bool PNRstatus = false;
            try
            {
                var dtCred = await _credential.AirlineCredentialDetail<_6EModel>(supplierType, supplier);
                if (dtCred != null && dtCred.Count > 0)
                {
                    var dr = dtCred.FirstOrDefault();
                    String username = dr.Userid;
                    String password = dr.Password;
                    String networkUserName = dr.NetworkUserName;
                    String networkPassword = dr.NetworkPassword;


                    apiResponse = await _service.GetCommit(model.JourneyType, networkUserName, networkPassword, dr.TargetBranch.ToString(), username, password, model.Searchid, model.Companyid, BookingRef, model.AirRQ, PassengerRS, CompanyRS, GstRS, PaymentType);
                    if (apiResponse != null && apiResponse.Length > 1)
                    {
                        bool.TryParse(apiResponse, out PNRstatus);
                    }

                }
            }
            catch (Exception ex)
            {
                //Common.Logger.dbLogg(CompanyID, 0, "GetAirAvailability", "api_uapi", AirRQ, SearchID, ex.Message);
            }
            return PNRstatus;
        }

        public async Task<bool> GetAirCommitAsync(AirAvaibilityModel model, string supplierType, string supplier, int BookingRef, string PassengerRS, string CompanyRS, string GstRS, string PaymentType)
        {
            string apiResponse = string.Empty;
            bool PNRstatus = false;
            try
            {
                var dtCred = await _credential.AirlineCredentialDetail<SupplierCREDGalileoAirline>(supplierType, supplier);
                if (dtCred != null && dtCred.Count > 0)
                {
                    var dr = dtCred.FirstOrDefault();
                    String username = dr.NetworkUserName;
                    String password = dr.NetworkPassword;

                    apiResponse = await _service.GetCommit(model.JourneyType, username, password, dr.TargetBranch.ToString(), username, password, model.Searchid, model.Companyid, BookingRef, model.AirRQ, PassengerRS, CompanyRS, GstRS, PaymentType);
                    if (apiResponse != null && apiResponse.Length > 1)
                    {
                        bool.TryParse(apiResponse, out PNRstatus);
                    }

                }
            }
            catch (Exception ex)
            {
                //Common.Logger.dbLogg(CompanyID, 0, "GetAirAvailability", "api_uapi", AirRQ, SearchID, ex.Message);
            }
            return PNRstatus;
        }

        public async Task<string> GetAirLineBookingStatusData(string universalRecordLocatorCode, string supplier, string supplierType, string searchID)
        {
            string apiResponse = string.Empty;
            bool PNRstatus = false;
            
            try
            {

                if (supplier.Equals("P3822701") || supplier.Equals("P7151745"))
                {
                    var dtCred = await _credential.AirlineCredentialDetail<SupplierCREDGalileoAirline>(supplierType, supplier);

                    if (dtCred != null && dtCred.Count > 0)
                    {
                        var dr = dtCred.FirstOrDefault();
                        String username = dr.NetworkUserName;
                        String password = dr.NetworkPassword;

                        apiResponse = await _service.GetBookingData(universalRecordLocatorCode, dr.TargetBranch.ToString(), username, password, searchID);


                    }
                }
                else if (supplier.Equals("IGS2528") || supplier.Equals("IGS2528"))
                {
                    var dtCred = await _credential.AirlineCredentialDetail<_6EModel>(supplierType, supplier);
                    if (dtCred != null && dtCred.Count > 0)
                    {
                        var dr = dtCred.FirstOrDefault();
                        String username = dr.NetworkUserName;
                        String password = dr.NetworkPassword;

                        apiResponse = await _service.Get6eBookingData(universalRecordLocatorCode, dr.TargetBranch.ToString(), username, password, searchID);

                    }
                }


                if (apiResponse != null && apiResponse.Length > 1)
                {
                    return apiResponse;
                }
                
            }
            catch (Exception ex)
            {
                //Common.Logger.dbLogg(CompanyID, 0, "GetAirAvailability", "api_uapi", AirRQ, SearchID, ex.Message);
            }
            return apiResponse;
        }

        public async Task<bool> GetAirCommitForHoldBookingAsync(string universalRecordLocatorCode, string supplier, string supplierType, string searchID, int bookingRef, string companyID,string fltType)
        {
            string apiResponse = string.Empty;
            bool PNRstatus = false;
            try
            {
                var dtCred = await _credential.AirlineCredentialDetail<SupplierCREDGalileoAirline>(supplierType, supplier);
                if (dtCred != null && dtCred.Count > 0)
                {
                    var dr = dtCred.FirstOrDefault();
                    String username = dr.NetworkUserName;
                    String password = dr.NetworkPassword;

                    PNRstatus = await _service.GetAirCommitForHoldBookingAsync(universalRecordLocatorCode, dr.TargetBranch.ToString(), username, password, searchID,bookingRef,companyID,fltType);
                    if (apiResponse != null && apiResponse.Length > 1)
                    {
                        bool.TryParse(apiResponse, out PNRstatus);
                    }

                }
            }
            catch (Exception ex)
            {
                //Common.Logger.dbLogg(CompanyID, 0, "GetAirAvailability", "api_uapi", AirRQ, SearchID, ex.Message);
            }
            return PNRstatus;
        }

        public async Task<bool> GetAirCommit6EForHoldBookingAsync(string universalRecordLocatorCode, string supplier, string supplierType, string searchID,int bookingRef,string companyID, string fltType)
        {
            string apiResponse = string.Empty;
            bool PNRstatus = false;
            try
            {
                var dtCred = await _credential.AirlineCredentialDetail<_6EModel>(supplierType, supplier);
                if (dtCred != null && dtCred.Count > 0)
                {
                    var dr = dtCred.FirstOrDefault();

                    String username = dr.Userid;
                    String password = dr.Password;
                    String networkUserName = dr.NetworkUserName;
                    String networkPassword = dr.NetworkPassword;

                    PNRstatus = await _service.GetAirCommit6EForHoldBookingAsync(universalRecordLocatorCode, dr.TargetBranch.ToString(), networkUserName, networkPassword, username, password, searchID,bookingRef,companyID, fltType);
                    if (apiResponse != null && apiResponse.Length > 1)
                    {
                        bool.TryParse(apiResponse, out PNRstatus);
                    }

                }
            }
            catch (Exception ex)
            {
                //Common.Logger.dbLogg(CompanyID, 0, "GetAirAvailability", "api_uapi", AirRQ, SearchID, ex.Message);
            }
            return PNRstatus;
        }
    }
}
