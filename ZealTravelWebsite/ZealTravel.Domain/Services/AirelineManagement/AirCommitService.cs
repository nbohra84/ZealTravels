using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZealTravel.Common;
using ZealTravel.Common.CommonUtility;
using ZealTravel.Common.Helpers.Flight;
using ZealTravel.Domain.AgencyManagement;
using ZealTravel.Domain.Interfaces.AirlineManagement;
using ZealTravel.Domain.Interfaces.Akasha;
using ZealTravel.Domain.Interfaces.CallCenterManagement;
using ZealTravel.Domain.Interfaces.DBCommon;
using ZealTravel.Domain.Interfaces.FlightManagement.Akasaa;
using ZealTravel.Domain.Interfaces.PNRManagement;
using ZealTravel.Domain.Interfaces.Spicejet;
using ZealTravel.Domain.Interfaces.TBO;
using ZealTravel.Domain.Interfaces.UniversalAPI;
using ZealTravel.Domain.Models;
using ZealTravel.Domain.Services.AirelineManagement.AkashaAir;

namespace ZealTravel.Domain.Services.AirelineManagement
{
    public class AirCommitService : IAirCommitService
    {
        ITBOService _tboService;
        IUAPIServices _uapiService;
        IAkashaService _akashaService;
        ISpicejetService _spicejetService;
        IAgencyService _agencyService;
        IAirlineDetailService _airlineDetailService;
        IPNRService _pnrService;
        ICallCenterService _callCenterService;
        IDBLoggerService _dbLoggerService;

        public AirCommitService(ITBOService tboService, IAkashaService akashaService, IUAPIServices uapiService, ISpicejetService spicejetService,  IAgencyService agencyService, IAirlineDetailService airlineDetailService, IPNRService pnrService, ICallCenterService callCenterService, IDBLoggerService dBLoggerService)
        {
            _tboService = tboService;
            _akashaService = akashaService;
            _uapiService = uapiService;
            _spicejetService = spicejetService;
            _agencyService = agencyService;
            _airlineDetailService = airlineDetailService;
            _pnrService = pnrService;
            _callCenterService = callCenterService;
            _dbLoggerService = dBLoggerService;
        }

       

        public async Task<bool> GetConfirm(AirAvaibilityModel airAvaibility,  int BookingRef,  string PassengerRS, string GstRS,string PaymentType)
        {
            bool PnrStatus = false;
            if (CommonUtility.VerifyCompany(airAvaibility.Companyid))
            {
                var compantDetails = await _agencyService.GetCompanyDetailbyCompanyID(airAvaibility.Companyid);
                string CompanyRS =  CommonUtility.GetCompanyDetailForPNR(compantDetails);
                if (airAvaibility.JourneyType.Equals("OW"))
                {
                    string SupplierOutbound = CommonUtility.GetAirlineID(airAvaibility.AirRQ , "O");
                    string AirRSOutbound = SelectedResponseHelper.GetSelectedResponse(airAvaibility.AirRQ, "O");

                    string AirRSInbound = string.Empty;
                    string SupplierInbound = CommonUtility.GetAirlineID(airAvaibility.AirRQ, "I");
                    if (SupplierInbound != null && SupplierInbound.Length > 0)
                    {
                        AirRSInbound = SelectedResponseHelper.GetSelectedResponse(airAvaibility.AirRQ, "I");
                    }

                    if (await AirlinePnrValidationStatus(airAvaibility.Companyid, AirRSOutbound))
                    {
                        if (AirRSInbound.Equals(string.Empty) || await AirlinePnrValidationStatus(airAvaibility.Companyid, AirRSInbound))
                        {
                            if (SupplierOutbound.Equals("OWN"))
                            {
                              //  api_db objCall = new api_db(SearchID, CompanyID);
                            }
                            else if (SupplierOutbound.Equals("ZealTravels") || SupplierOutbound.Equals("MAAZ852"))
                            {
                                // api_tbo objCall = new api_tbo(JourneyType, SearchID, CompanyID, SupplierOutbound, "API");
                                //_tboService.GetCommit(AirRSOutbound, BookingRef, PassengerRS, CompanyRS, GstRS);
                            }
                            else if (SupplierOutbound.Equals("P3822701") || SupplierOutbound.Equals("P7151745"))
                            {
                                //api_uapi objCall = new api_uapi(JourneyType, SearchID, CompanyID, SupplierOutbound, "GDS");
                                //PnrStatus = true;
                                airAvaibility.AirRQ = AirRSOutbound;
                                PnrStatus =await _uapiService.GetAirCommitAsync(airAvaibility,"GDS", SupplierOutbound, BookingRef, PassengerRS, CompanyRS, GstRS,PaymentType);
                            }
                            else if (SupplierOutbound.Equals("IGS2528") || SupplierOutbound.Equals("IGS2528"))
                            {
                                //api_uapi objCall = new api_uapi(JourneyType, SearchID, CompanyID, SupplierOutbound, "LCC");
                                airAvaibility.AirRQ = AirRSOutbound;
                                PnrStatus = await _uapiService.GetAirCommit6EAsync(airAvaibility, "LCC", SupplierOutbound, BookingRef, PassengerRS, CompanyRS, GstRS,PaymentType);
                                
                            }
                            else if (SupplierOutbound.Equals("MAAXT98402") || SupplierOutbound.Equals("CPNMAA0030") || SupplierOutbound.Equals("MAAXTA8402") || SupplierOutbound.Equals("APITESTID"))
                            {
                                airAvaibility.AirRQ = AirRSOutbound;
                                PnrStatus = await _spicejetService.GetAirCommitAsync(airAvaibility, BookingRef, PassengerRS, CompanyRS, GstRS,PaymentType);
                                //objCall.GetCommit(AirRSOutbound, BookingRef, PassengerRS, CompanyRS, GstRS);
                            }
                            else if (SupplierOutbound.Equals("QPMAA8752B") || SupplierOutbound.Equals("QPMAA8752B"))
                            {
                                airAvaibility.AirRQ = AirRSOutbound;
                                PnrStatus = await _akashaService.GetAirCommitAsync(airAvaibility, BookingRef, PassengerRS, CompanyRS, GstRS);
                            }


                            if (PnrStatus && AirRSInbound.IndexOf("RefID") != -1)
                            {
                                if (SupplierInbound.Equals("OWN"))
                                {
                                    // api_db objCall = new api_db(SearchID, CompanyID);
                                }
                                else if (SupplierInbound.Equals("ZealTravels") || SupplierInbound.Equals("MAAZ852"))
                                {
                                    //api_tbo objCall = new api_tbo(JourneyType, SearchID, CompanyID, SupplierInbound, "API");
                                    //PnrStatus = true;  //commited on 12Sep2023 objCall.GetCommit(AirRSInbound, BookingRef, PassengerRS, CompanyRS, GstRS);
                                }
                                else if (SupplierInbound.Equals("P3822701") || SupplierInbound.Equals("P7151745"))
                                {
                                    // api_uapi objCall = new api_uapi(JourneyType, SearchID, CompanyID, SupplierInbound, "GDS");
                                    // PnrStatus = true;  //commited on 12Sep2023 objCall.GetCommit(AirRSInbound, BookingRef, PassengerRS, CompanyRS, GstRS);
                                    airAvaibility.AirRQ = AirRSInbound;
                                    PnrStatus = await _uapiService.GetAirCommitAsync(airAvaibility, "GDS", SupplierInbound, BookingRef, PassengerRS, CompanyRS, GstRS,PaymentType);
                                }

                                else if (SupplierInbound.Equals("IGS2528") || SupplierInbound.Equals("IGS2528"))
                                {
                                    // api_uapi objCall = new api_uapi(JourneyType, SearchID, CompanyID, SupplierInbound, "LCC");
                                    //PnrStatus = true;  //commited on 12Sep2023 objCall.GetCommit(AirRSInbound, BookingRef, PassengerRS, CompanyRS, GstRS);
                                    airAvaibility.AirRQ = AirRSInbound;
                                    PnrStatus = await _uapiService.GetAirCommit6EAsync(airAvaibility, "LCC", SupplierInbound, BookingRef, PassengerRS, CompanyRS, GstRS,PaymentType);
                                }
                                else if (SupplierInbound.Equals("MAAXT98402") || SupplierInbound.Equals("CPNMAA0030") || SupplierInbound.Equals("MAAXTA8402") || SupplierInbound.Equals("APITESTID"))
                                {
                                    // api_spicejet objCall = new api_spicejet(JourneyType, SearchID, CompanyID, SupplierInbound, "LCC");
                                    // PnrStatus = true;  //commited on 12Sep2023 objCall.GetCommit(AirRSInbound, BookingRef, PassengerRS, CompanyRS, GstRS);
                                    airAvaibility.AirRQ = AirRSInbound;
                                    PnrStatus = await _spicejetService.GetAirCommitAsync(airAvaibility, BookingRef, PassengerRS, CompanyRS, GstRS,PaymentType);
                                }
                                else if (SupplierInbound.Equals("QPMAA8752B") || SupplierInbound.Equals("QPMAA8752B"))
                                {
                                    // api_qp objCall = new api_qp(JourneyType, SearchID, CompanyID, SupplierInbound, "LCC");
                                    //PnrStatus = objCall.GetCommit(AirRSInbound, BookingRef, PassengerRS, CompanyRS, GstRS);
                                    airAvaibility.AirRQ = AirRSInbound;
                                    PnrStatus = await _akashaService.GetAirCommitAsync(airAvaibility, BookingRef, PassengerRS, CompanyRS, GstRS);
                                }
                            }
                        }
                    }
                }
                else if (airAvaibility.JourneyType.Equals("RT") || airAvaibility.JourneyType.Equals("RTLCC") || airAvaibility.JourneyType.Equals("MC"))
                {
                    string Supplier = CommonUtility.GetAirlineID(airAvaibility.AirRQ);
                    if (await AirlinePnrValidationStatus(airAvaibility.Companyid, airAvaibility.AirRQ))
                    {
                        if (Supplier.Equals("OWN"))
                        {
                           // api_db objCall = new api_db(SearchID, CompanyID);
                        }
                        else if (Supplier.Equals("ZealTravels") || Supplier.Equals("MAAZ852"))
                        {
                           // api_tbo objCall = new api_tbo(JourneyType, SearchID, CompanyID, Supplier, "API");
                            //PnrStatus = objCall.GetCommit(AirRS, BookingRef, PassengerRS, CompanyRS, GstRS);
                        }
                        else if (Supplier.Equals("P3822701") || Supplier.Equals("P7151745"))
                        {
                            PnrStatus = await _uapiService.GetAirCommitAsync(airAvaibility, "GDS", Supplier, BookingRef, PassengerRS, CompanyRS, GstRS,PaymentType);
                        }
                        else if (Supplier.Equals("IGS2528") || Supplier.Equals("IGS2528"))
                        {
                            PnrStatus = await _uapiService.GetAirCommit6EAsync(airAvaibility, "LCC", Supplier, BookingRef, PassengerRS, CompanyRS, GstRS, PaymentType);
                        }
                        else if (Supplier.Equals("MAAXT98402") || Supplier.Equals("CPNMAA0030") || Supplier.Equals("MAAXTA8402") || Supplier.Equals("APITESTID"))
                        {
                            // api_spicejet objCall = new api_spicejet(JourneyType, SearchID, CompanyID, Supplier, "LCC");
                            //PnrStatus = objCall.GetCommit(AirRS, BookingRef, PassengerRS, CompanyRS, GstRS);
                            PnrStatus = await _spicejetService.GetAirCommitAsync(airAvaibility, BookingRef, PassengerRS, CompanyRS, GstRS,PaymentType);
                        }
                        else if (Supplier.Equals("QPMAA8752B") || Supplier.Equals("QPMAA8752B"))
                        {
                            PnrStatus = await _akashaService.GetAirCommitAsync(airAvaibility, BookingRef, PassengerRS, CompanyRS, GstRS);
                        }
                    }
                }
            }

            if (!PnrStatus)
            {
                await _callCenterService.FallInToAirlineCallCenter(BookingRef, airAvaibility.Companyid);
            }
            return PnrStatus;
        }

        private async Task<bool> AirlinePnrValidationStatus(string CompanyID, string TResponse)
        {
            bool Status = false;
            string CarrierCode = "";

            try
            {
                DataSet dsAvailability = CommonFunction.StringToDataSet(TResponse);
                DataRow dr = dsAvailability.Tables[0].Rows[0];
                CarrierCode = dr["CarrierCode"].ToString().Trim();
                string PriceType = dr["PriceType"].ToString().Trim();
                string AirlineID = dr["AirlineID"].ToString().Trim();
                string Sector = dr["Sector"].ToString().Trim();
                string DepartureDate = dr["DepartureDate"].ToString().Trim();


                if (Sector == "")
                {
                    if (dsAvailability.Tables[0].Columns.Contains("ArrivalStation") && dr["ArrivalStation"] != null)
                    {
                        
                        bool bINT = await _airlineDetailService.IsDomestic(dr["DepartureStation"].ToString().Trim(), (dr["ArrivalStation"] == null ? "" : dr["ArrivalStation"].ToString().Trim()));
                        if (bINT == false)
                        {
                            Sector = "I";
                        }
                        else
                        {
                            Sector = "D";
                        }
                    }
                }

               
                Status = CommonUtility.IsFamilyFare(CarrierCode, PriceType);
                if (Status.Equals(false))
                {
                    int i = await _pnrService.GetPNRstatusByPriceTypeFare(CompanyID, AirlineID, CarrierCode, Sector, PriceType);
                    if (i != -1)
                    {
                        if (i.Equals(0))
                        {
                            _dbLoggerService.dbLogg(CompanyID, 0, "", "Airline_Pnr_Validation_Status", CarrierCode, "", PriceType + " not allowded");
                            return false;
                        }
                        return true;
                    }
                    else
                    {
                        Status = await _pnrService.GetPNRstatus(CompanyID, dsAvailability.Tables[0].Rows[0]["CarrierCode"].ToString().Trim(), Sector.ToString().Trim());
                        if (Status)
                        {
                            Status = await _airlineDetailService.GetDateValidationDays(dsAvailability.Tables[0].Rows[0]["CarrierCode"].ToString().Trim(), Sector.ToString().Trim(), dsAvailability.Tables[0].Rows[0]["DepartureDate"].ToString().Trim());
                        }
                    }
                }
                //}
            }
            catch (Exception ex)
            {
                _dbLoggerService.dbLogg(CompanyID, 0, "", "Airline_Pnr_Validation_Status", CarrierCode, "", ex.Message);
            }

            return Status;
        }
    }
}
