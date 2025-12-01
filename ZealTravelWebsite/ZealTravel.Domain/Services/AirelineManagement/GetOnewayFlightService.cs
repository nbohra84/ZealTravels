using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZealTravel.Domain.Interfaces.Akasha;
using ZealTravel.Domain.Interfaces.DBCommon;
using ZealTravel.Domain.Interfaces.AirelineManagement;
using ZealTravel.Domain.Interfaces.TBO;
using ZealTravel.Domain.Interfaces.UniversalAPI;
using ZealTravel.Domain.Models;
using ZealTravel.Common.CommonUtility;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Collections;
using System.Data;
using ZealTravel.Domain.Data.Entities;
using DocumentFormat.OpenXml.Spreadsheet;
using System.Xml;
using ZealTravel.Domain.Interfaces.Spicejet;

namespace ZealTravel.Domain.Services.AirelineManagement
{
    public class GetOneWayFlightService : IGetOneWayFlightService
    {
        public GetOneWayFlightService() { }
        ITBOService _tboService;
        IUAPIServices _uapiService;
        IAkashaService _akashaService;
        ISpicejetService _spicejetService;
        IGetResponseData _getResponseData;

        ICredential _credential;

        private string AirRQ;
        private string AirRQ_tbo;
        private string AirRQ_spicejet;
        private string AirRQ_uapi;
        private string AirRQ_uapi_6e;
        private string AirRQ_qp;
        //============================================================
        private string tbo_Supplier;
        private string tbo_SupplierType;

        private string spicejet_Supplier;
        private string spicejet_SupplierType;

        private string spicejet_Supplier_max;
        private string spicejet_SupplierType_max;

        private string spicejet_corporate_Supplier;
        private string spicejet_corporate_SupplierType;

        private string spicejet_coupon_Supplier;
        private string spicejet_coupon_SupplierType;

        private string uapi_Supplier;
        private string uapi_SupplierType;

        private string e6_Supplier;
        private string e6_SupplierType;

        private string qp_Supplier;
        private string qp_SupplierType;
        //============================================================
        private bool SGCarrierActiveFromClient;
        private bool E6CarrierActiveFromClient;
        private bool GDSCarrierActiveFromClient;
        private string Sector;

        private string Cabin;
        private IAirlineDetailService _airlineDetailService;    
        public GetOneWayFlightService(ITBOService tboService, IAkashaService akashaService, IUAPIServices uapiService, ISpicejetService spicejetService, IGetResponseData getResponseData, IAirlineDetailService airlineDetailService , ICredential credential)
        {
            _tboService = tboService;
            _akashaService = akashaService;
            _uapiService = uapiService;
            _getResponseData = getResponseData;
            _credential = credential;
            _airlineDetailService = airlineDetailService;
            _spicejetService = spicejetService;

        }
        public string GetAvailableFights(AirAvaibilityModel parameters)
        {
            SetAirRQ(parameters);


            Task<string> _Resp_GetQP = null;
            Task<string> _Resp_uapi = null;
            Task<string> _Resp_uapi_sme = null;
            Task<string> _Resp_6e = null;
            Task<string> _Resp_spicejet = null;
            Task<string> _Resp_spicejetMax = null;
            Task<string> _Resp_spicejetCoupon = null;
            Task<string> Resp_spicejet_corporate = null;

            if (qp_Supplier != null && qp_Supplier.Length > 0 && AirRQ_qp != null && AirRQ_qp.Length > 0)
            {
                parameters.AirRQ = AirRQ_qp;
               _Resp_GetQP = _akashaService.GetAirAvailabilityAsync(parameters, qp_SupplierType, qp_Supplier,Sector);
            }
            if (uapi_Supplier != null && uapi_Supplier.Length > 0 && AirRQ_uapi != null && AirRQ_uapi.Length > 0)
            {
                parameters.AirRQ = AirRQ_uapi;
                _Resp_uapi = _uapiService.GetAirAvailabilityAsync(parameters, uapi_SupplierType, uapi_Supplier, Sector);
            }

            if (uapi_Supplier != null && uapi_Supplier.Length > 0 && AirRQ_uapi != null && AirRQ_uapi.Length > 0)
            {
                parameters.AirRQ = AirRQ_uapi;
                _Resp_uapi_sme = _uapiService.GetAirAvailabilityAsync(parameters, uapi_SupplierType, uapi_Supplier, Sector,true);
            }


            if (Cabin.Equals("Y") && e6_Supplier != null && e6_Supplier.Length > 0 && AirRQ_uapi_6e != null && AirRQ_uapi_6e.Length > 0)
            {
                parameters.AirRQ = AirRQ_uapi_6e;
                _Resp_6e = _uapiService.GetAirAvailability6EAsync(parameters, e6_SupplierType, e6_Supplier, Sector);
            }

            if (Cabin.Equals("Y") && parameters.JourneyType != "RTLCC" && spicejet_Supplier != null && spicejet_Supplier.Length > 0 && AirRQ_spicejet != null && AirRQ_spicejet.Length > 0)
            {
                parameters.AirRQ = AirRQ_spicejet;
                _Resp_spicejet = _spicejetService.GetAirAvailabilityAsync(parameters, spicejet_SupplierType, spicejet_Supplier, Sector, "LCC");
            }

            if (Cabin.Equals("Y") && parameters.JourneyType != "RTLCC" && spicejet_Supplier_max != null && spicejet_Supplier_max.Length > 0 && AirRQ_spicejet != null && AirRQ_spicejet.Length > 0)
            {
                parameters.AirRQ = AirRQ_spicejet;
               _Resp_spicejetMax = _spicejetService.GetAirAvailabilityAsync(parameters, spicejet_SupplierType_max, spicejet_Supplier_max, Sector, "MAX");
            }

            if (Cabin.Equals("Y") && spicejet_coupon_Supplier != null && spicejet_coupon_Supplier.Length > 0 && AirRQ_spicejet != null && AirRQ_spicejet.Length > 0)
            {
                parameters.AirRQ = AirRQ_spicejet;
               _Resp_spicejetCoupon = _spicejetService.GetAirAvailabilityAsync(parameters, spicejet_coupon_SupplierType, spicejet_coupon_Supplier, Sector, "COUPON");
            }

            if (Cabin.Equals("Y") && spicejet_corporate_Supplier != null && spicejet_corporate_Supplier.Length > 0 && AirRQ_spicejet != null && AirRQ_spicejet.Length > 0)
            {
                parameters.AirRQ = AirRQ_spicejet;
                Resp_spicejet_corporate = _spicejetService.GetAirAvailabilityAsync(parameters, spicejet_corporate_SupplierType, spicejet_corporate_Supplier, Sector, "CORPORATE");
            }

            if (_Resp_GetQP != null) _Resp_GetQP.Wait();
            if (_Resp_uapi != null) _Resp_uapi.Wait();
            if (_Resp_uapi_sme != null) _Resp_uapi_sme.Wait();
            
            if (_Resp_6e != null) _Resp_6e.Wait();

            if (_Resp_spicejet != null) _Resp_spicejet.Wait();
            if (_Resp_spicejetMax != null) _Resp_spicejetMax.Wait();
            if (_Resp_spicejetCoupon != null) _Resp_spicejetCoupon.Wait();
            if (Resp_spicejet_corporate != null) Resp_spicejet_corporate.Wait();

            var qpResult = _Resp_GetQP != null ? _Resp_GetQP.Result : null;
            var uapiResult = _Resp_uapi != null ? _Resp_uapi.Result : null;
            var uapiSMEResult = _Resp_uapi_sme != null ? _Resp_uapi_sme.Result : null;
            var uapi6eResult = _Resp_6e != null ? _Resp_6e.Result : null;
            var spicejetResult = _Resp_spicejet != null ? _Resp_spicejet.Result : null;
            var spicejetMaxResult = _Resp_spicejetMax != null ? _Resp_spicejetMax.Result : null;
            var spicejetCouponResult = _Resp_spicejetCoupon != null ? _Resp_spicejetCoupon.Result : null;
            var spicejetCorporateResult = Resp_spicejet_corporate != null ? Resp_spicejet_corporate.Result : null;


            //var flightResult = _getResponseData.GetResponse(null, null, null, null, null, null, null, qpResult, parameters.JourneyType, null);
            var flightResult = _getResponseData.GetResponse(null, spicejetMaxResult, spicejetResult, spicejetCouponResult, spicejetCorporateResult, uapiResult, uapiSMEResult, uapi6eResult, qpResult, parameters.JourneyType, null);
            //var flightResult = _getResponseData.GetResponse(tboResult, null,null,null,null, uapiResult, uapi6eResult, qpResult, parameters.JourneyType,null);
            var responseString = _getResponseData.ConvertDataTableToString(flightResult, parameters.Searchid, parameters.Companyid);
            return responseString;
        }

        private void SetAirRQ(AirAvaibilityModel parameters)
        {
            AirRQ = parameters.AirRQ;
            AirRQ_tbo = parameters.AirRQ;
            AirRQ_spicejet = parameters.AirRQ;
            AirRQ_uapi = parameters.AirRQ;
            AirRQ_uapi_6e = parameters.AirRQ;
            AirRQ_qp = parameters.AirRQ;

            XmlDocument xmldoc = new XmlDocument();
            xmldoc.LoadXml(AirRQ);
            Cabin = xmldoc.SelectSingleNode("AvailabilityRequest/Cabin").InnerText;

           
            if (parameters.JourneyType.Equals("MC"))
            {
                SGCarrierActiveFromClient = true;
                E6CarrierActiveFromClient = true;
                GDSCarrierActiveFromClient = true;

            }
            else
            {
                SGCarrierActiveFromClient = CommonUtility.IsCarrierActiveFromClient("SG", AirRQ);
                E6CarrierActiveFromClient = CommonUtility.IsCarrierActiveFromClient("6E", AirRQ);
                GDSCarrierActiveFromClient = CommonUtility.IsCarrierActiveFromClient("GDS", AirRQ);
            }

            var suppliers = _credential.GetSuppliers();
            if (suppliers != null && suppliers.Count > 0)
            {
                var lccSuppliers = suppliers.Where(s => s.Supplier_Type == "LCC").ToList();
                if (lccSuppliers.Count > 0)
                {
                    if (parameters.JourneyType != "MC")
                    {
                        foreach (var supplier in lccSuppliers)
                        {
                            if (supplier.Fare_Type.Equals("MAX") && supplier.CarrierCode.Equals("SG"))
                            {
                                spicejet_Supplier_max = supplier.SupplierID;
                                spicejet_SupplierType_max = supplier.Supplier_Type;
                            }
                            else if (supplier.Fare_Type.Equals("Regular") && supplier.CarrierCode.Equals("SG"))
                            {
                                spicejet_Supplier = supplier.SupplierID;
                                spicejet_SupplierType = supplier.Supplier_Type;
                            }
                            else if (supplier.Fare_Type.Equals("COR") && supplier.CarrierCode.Equals("SG"))
                            {
                                spicejet_corporate_Supplier = supplier.SupplierID;
                                spicejet_corporate_SupplierType = supplier.Supplier_Type;
                            }
                            else if (supplier.Fare_Type.Equals("CPN") && supplier.CarrierCode.Equals("SG"))
                            {
                                spicejet_coupon_Supplier = supplier.SupplierID;
                                spicejet_coupon_SupplierType = supplier.Supplier_Type;
                            }
                            else if (supplier.CarrierCode.Equals("6E"))
                            {
                                e6_Supplier = supplier.SupplierID;
                                e6_SupplierType = supplier.Supplier_Type;
                            }
                            else if (supplier.CarrierCode.Equals("QP"))
                            {
                                qp_Supplier = supplier.SupplierID;
                                qp_SupplierType = supplier.Supplier_Type;
                            }
                        }
                    }
                    else
                    {
                        foreach (var supplier in lccSuppliers)
                        {
                            if (supplier.Fare_Type.Equals("MAX") && supplier.CarrierCode.Equals("SG"))
                            {
                                spicejet_Supplier_max = supplier.SupplierID;
                                spicejet_SupplierType_max = supplier.Supplier_Type;
                            }
                            else if (supplier.Fare_Type.Equals("Regular") && supplier.CarrierCode.Equals("SG"))
                            {
                                spicejet_Supplier = supplier.SupplierID;
                                spicejet_SupplierType = supplier.Supplier_Type;
                            }
                            else if (supplier.Fare_Type.Equals("COR") && supplier.CarrierCode.Equals("SG"))
                            {
                                spicejet_corporate_Supplier = supplier.SupplierID;
                                spicejet_corporate_SupplierType = supplier.Supplier_Type;
                            }
                            else if (supplier.Fare_Type.Equals("CPN") && supplier.CarrierCode.Equals("SG"))
                            {
                                spicejet_coupon_Supplier = supplier.SupplierID;
                                spicejet_coupon_SupplierType = supplier.Supplier_Type;
                            }
                            else if (supplier.CarrierCode.Equals("6E"))
                            {
                                e6_Supplier = supplier.SupplierID;
                                e6_SupplierType = supplier.Supplier_Type;
                            }
                            else if (supplier.CarrierCode.Equals("QP"))
                            {
                                qp_Supplier = supplier.SupplierID;
                                qp_SupplierType = supplier.Supplier_Type;
                            }
                        }
                    }
                }

                if (E6CarrierActiveFromClient && e6_Supplier != null && e6_SupplierType.Length > 0)
                {
                    this.AirRQ_uapi_6e = CommonUtility.RemoveCarrierfromAvailabilityRequest_uApi(AirRQ, true, false);
                }

                var apiSuppliers = suppliers.Where(s => s.Supplier_Type == "API").ToList();
                if (apiSuppliers.Count > 0)
                {
                    var apiSupplier = apiSuppliers.First();
                    tbo_Supplier = apiSupplier.SupplierID;
                    tbo_SupplierType = apiSupplier.Supplier_Type;
                }
                if (tbo_Supplier != null && tbo_Supplier.Length > 0)
                {
                    List<SupplierApiAirline> dtCarriers = _credential.InactiveCarriersAPI(tbo_Supplier);


                    if (dtCarriers != null && dtCarriers.Count > 0)
                    {
                        List<string> Carriers = new List<string>();
                        foreach (var carrier in dtCarriers)
                        {
                            Carriers.Add(carrier.CarrierCode);
                        }

                        if (Carriers != null && Carriers.Count > 0)
                        {
                            this.AirRQ_tbo = CommonUtility.RemoveCarrierfromAvailabilityRequest(AirRQ, Carriers);
                        }
                    }
                }


                if (parameters.JourneyType != "MC")
                {
                    var gdsSuppliers = suppliers.Where(s => s.Supplier_Type == "GDS").ToList();
                    if (gdsSuppliers.Count > 0)
                    {
                        var gdsSupplier = gdsSuppliers.First();
                        uapi_Supplier = gdsSupplier.SupplierID;
                        uapi_SupplierType = gdsSupplier.Supplier_Type;

                        if (uapi_Supplier != null && uapi_Supplier.Length > 0 && GDSCarrierActiveFromClient)
                        {
                            this.AirRQ_uapi = CommonUtility.RemoveCarrierfromAvailabilityRequest_uApi(AirRQ, false, true);
                        }
                    }
                }

                if (parameters.JourneyType == "MC")
                {
                    var gdsSuppliers = suppliers.Where(s => s.Supplier_Type == "GDS").ToList();
                    if (gdsSuppliers.Count > 0)
                    {
                        var gdsSupplier = gdsSuppliers.First();
                        uapi_Supplier = gdsSupplier.SupplierID;
                        uapi_SupplierType = gdsSupplier.Supplier_Type;

                        if (uapi_Supplier != null && uapi_Supplier.Length > 0 && GDSCarrierActiveFromClient)
                        {
                            this.AirRQ_uapi_6e = CommonUtility.AddDefaultCarrierfromAvailabilityRequestMC(AirRQ);
                        }
                    }
                }

                var departureStation = xmldoc.SelectSingleNode("AvailabilityRequest/DepartureStation").InnerText;
                var arrivalStation = xmldoc.SelectSingleNode("AvailabilityRequest/ArrivalStation").InnerText;

                Sector = "D";
                
                if (!_airlineDetailService.IsDomestic(departureStation, arrivalStation).Result)
                {
                    Sector = "I";
                }
            }
        }
    }
}
