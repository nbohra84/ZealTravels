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
using Microsoft.Extensions.Logging;
using System.Diagnostics;

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
        private readonly ILogger<GetOneWayFlightService>? _logger;
        
        public GetOneWayFlightService(ITBOService tboService, IAkashaService akashaService, IUAPIServices uapiService, ISpicejetService spicejetService, IGetResponseData getResponseData, IAirlineDetailService airlineDetailService , ICredential credential, ILogger<GetOneWayFlightService>? logger = null)
        {
            _tboService = tboService;
            _akashaService = akashaService;
            _uapiService = uapiService;
            _getResponseData = getResponseData;
            _credential = credential;
            _airlineDetailService = airlineDetailService;
            _spicejetService = spicejetService;
            _logger = logger;
        }
        public string GetAvailableFights(AirAvaibilityModel parameters)
        {
            SetAirRQ(parameters);


            // Dictionary to store API call timings and results
            Dictionary<string, (Stopwatch stopwatch, Task<string> task)> apiCalls = new Dictionary<string, (Stopwatch, Task<string>)>();
            
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
                var sw = Stopwatch.StartNew();
                _Resp_GetQP = _akashaService.GetAirAvailabilityAsync(parameters, qp_SupplierType, qp_Supplier, Sector);
                apiCalls["Akasa"] = (sw, _Resp_GetQP);
            }
            if (uapi_Supplier != null && uapi_Supplier.Length > 0 && AirRQ_uapi != null && AirRQ_uapi.Length > 0)
            {
                parameters.AirRQ = AirRQ_uapi;
                var sw = Stopwatch.StartNew();
                _Resp_uapi = _uapiService.GetAirAvailabilityAsync(parameters, uapi_SupplierType, uapi_Supplier, Sector);
                apiCalls["Galileo"] = (sw, _Resp_uapi);
            }

            if (uapi_Supplier != null && uapi_Supplier.Length > 0 && AirRQ_uapi != null && AirRQ_uapi.Length > 0)
            {
                parameters.AirRQ = AirRQ_uapi;
                var sw = Stopwatch.StartNew();
                _Resp_uapi_sme = _uapiService.GetAirAvailabilityAsync(parameters, uapi_SupplierType, uapi_Supplier, Sector, true);
                apiCalls["Galileo_SME"] = (sw, _Resp_uapi_sme);
            }

            if (Cabin.Equals("Y") && e6_Supplier != null && e6_Supplier.Length > 0 && AirRQ_uapi_6e != null && AirRQ_uapi_6e.Length > 0)
            {
                parameters.AirRQ = AirRQ_uapi_6e;
                var sw = Stopwatch.StartNew();
                _Resp_6e = _uapiService.GetAirAvailability6EAsync(parameters, e6_SupplierType, e6_Supplier, Sector);
                apiCalls["IndiGo_6E"] = (sw, _Resp_6e);
            }

            if (Cabin.Equals("Y") && parameters.JourneyType != "RTLCC" && spicejet_Supplier != null && spicejet_Supplier.Length > 0 && AirRQ_spicejet != null && AirRQ_spicejet.Length > 0)
            {
                parameters.AirRQ = AirRQ_spicejet;
                var sw = Stopwatch.StartNew();
                _Resp_spicejet = _spicejetService.GetAirAvailabilityAsync(parameters, spicejet_SupplierType, spicejet_Supplier, Sector, "LCC");
                apiCalls["Spicejet"] = (sw, _Resp_spicejet);
            }

            if (Cabin.Equals("Y") && parameters.JourneyType != "RTLCC" && spicejet_Supplier_max != null && spicejet_Supplier_max.Length > 0 && AirRQ_spicejet != null && AirRQ_spicejet.Length > 0)
            {
                parameters.AirRQ = AirRQ_spicejet;
                var sw = Stopwatch.StartNew();
                _Resp_spicejetMax = _spicejetService.GetAirAvailabilityAsync(parameters, spicejet_SupplierType_max, spicejet_Supplier_max, Sector, "MAX");
                apiCalls["Spicejet_MAX"] = (sw, _Resp_spicejetMax);
            }

            if (Cabin.Equals("Y") && spicejet_coupon_Supplier != null && spicejet_coupon_Supplier.Length > 0 && AirRQ_spicejet != null && AirRQ_spicejet.Length > 0)
            {
                parameters.AirRQ = AirRQ_spicejet;
                var sw = Stopwatch.StartNew();
                _Resp_spicejetCoupon = _spicejetService.GetAirAvailabilityAsync(parameters, spicejet_coupon_SupplierType, spicejet_coupon_Supplier, Sector, "COUPON");
                apiCalls["Spicejet_COUPON"] = (sw, _Resp_spicejetCoupon);
            }

            if (Cabin.Equals("Y") && spicejet_corporate_Supplier != null && spicejet_corporate_Supplier.Length > 0 && AirRQ_spicejet != null && AirRQ_spicejet.Length > 0)
            {
                parameters.AirRQ = AirRQ_spicejet;
                var sw = Stopwatch.StartNew();
                Resp_spicejet_corporate = _spicejetService.GetAirAvailabilityAsync(parameters, spicejet_corporate_SupplierType, spicejet_corporate_Supplier, Sector, "CORPORATE");
                apiCalls["Spicejet_CORPORATE"] = (sw, Resp_spicejet_corporate);
            }

            // Wait for all API calls to complete
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

            // Log API results and timing
            LogApiResults(apiCalls, new Dictionary<string, string>
            {
                { "Akasa", qpResult },
                { "Galileo", uapiResult },
                { "Galileo_SME", uapiSMEResult },
                { "IndiGo_6E", uapi6eResult },
                { "Spicejet", spicejetResult },
                { "Spicejet_MAX", spicejetMaxResult },
                { "Spicejet_COUPON", spicejetCouponResult },
                { "Spicejet_CORPORATE", spicejetCorporateResult }
            }, parameters.Searchid);


            //var flightResult = _getResponseData.GetResponse(null, null, null, null, null, null, null, qpResult, parameters.JourneyType, null);
            var flightResult = _getResponseData.GetResponse(null, spicejetMaxResult, spicejetResult, spicejetCouponResult, spicejetCorporateResult, uapiResult, uapiSMEResult, uapi6eResult, qpResult, parameters.JourneyType, null);
            //var flightResult = _getResponseData.GetResponse(tboResult, null,null,null,null, uapiResult, uapi6eResult, qpResult, parameters.JourneyType,null);
            var responseString = _getResponseData.ConvertDataTableToString(flightResult, parameters.Searchid, parameters.Companyid);
            return responseString;
        }

        /// <summary>
        /// Async version for progressive loading - uses same logic as GetAvailableFights
        /// but stores results in cache as APIs complete
        /// </summary>
        public async Task<string> GetAvailableFightsAsync(AirAvaibilityModel parameters)
        {
            SetAirRQ(parameters);

            // Count total APIs that will be called (for progress tracking)
            int totalApis = 0;
            if (qp_Supplier != null && qp_Supplier.Length > 0 && AirRQ_qp != null && AirRQ_qp.Length > 0) totalApis++;
            if (uapi_Supplier != null && uapi_Supplier.Length > 0 && AirRQ_uapi != null && AirRQ_uapi.Length > 0) totalApis++; // Galileo
            if (uapi_Supplier != null && uapi_Supplier.Length > 0 && AirRQ_uapi != null && AirRQ_uapi.Length > 0) totalApis++; // Galileo_SME
            if (Cabin.Equals("Y") && e6_Supplier != null && e6_Supplier.Length > 0 && AirRQ_uapi_6e != null && AirRQ_uapi_6e.Length > 0) totalApis++;
            if (Cabin.Equals("Y") && parameters.JourneyType != "RTLCC" && spicejet_Supplier != null && spicejet_Supplier.Length > 0 && AirRQ_spicejet != null && AirRQ_spicejet.Length > 0) totalApis++;
            if (Cabin.Equals("Y") && parameters.JourneyType != "RTLCC" && spicejet_Supplier_max != null && spicejet_Supplier_max.Length > 0 && AirRQ_spicejet != null && AirRQ_spicejet.Length > 0) totalApis++;
            if (Cabin.Equals("Y") && spicejet_coupon_Supplier != null && spicejet_coupon_Supplier.Length > 0 && AirRQ_spicejet != null && AirRQ_spicejet.Length > 0) totalApis++;
            if (Cabin.Equals("Y") && spicejet_corporate_Supplier != null && spicejet_corporate_Supplier.Length > 0 && AirRQ_spicejet != null && AirRQ_spicejet.Length > 0) totalApis++;

            // Initialize cache with total API count
            ProgressiveSearchCache.Initialize(parameters.Searchid, parameters.JourneyType, parameters.Companyid, totalApis);

            // Dictionary to store API call timings and results (same as GetAvailableFights)
            Dictionary<string, (Stopwatch stopwatch, Task<string> task)> apiCalls = new Dictionary<string, (Stopwatch, Task<string>)>();
            
            Task<string> _Resp_GetQP = null;
            Task<string> _Resp_uapi = null;
            Task<string> _Resp_uapi_sme = null;
            Task<string> _Resp_6e = null;
            Task<string> _Resp_spicejet = null;
            Task<string> _Resp_spicejetMax = null;
            Task<string> _Resp_spicejetCoupon = null;
            Task<string> Resp_spicejet_corporate = null;

            // Start all API calls in parallel (same as GetAvailableFights)
            if (qp_Supplier != null && qp_Supplier.Length > 0 && AirRQ_qp != null && AirRQ_qp.Length > 0)
            {
                parameters.AirRQ = AirRQ_qp;
                var sw = Stopwatch.StartNew();
                _Resp_GetQP = _akashaService.GetAirAvailabilityAsync(parameters, qp_SupplierType, qp_Supplier, Sector);
                apiCalls["Akasa"] = (sw, _Resp_GetQP);
                // Initialize watcher status IMMEDIATELY when API task is created
                InitializeAndWatchApi(_Resp_GetQP, "Akasa", parameters.Searchid, sw);
            }
            if (uapi_Supplier != null && uapi_Supplier.Length > 0 && AirRQ_uapi != null && AirRQ_uapi.Length > 0)
            {
                parameters.AirRQ = AirRQ_uapi;
                var sw = Stopwatch.StartNew();
                _Resp_uapi = _uapiService.GetAirAvailabilityAsync(parameters, uapi_SupplierType, uapi_Supplier, Sector);
                apiCalls["Galileo"] = (sw, _Resp_uapi);
                // Initialize watcher status IMMEDIATELY when API task is created
                InitializeAndWatchApi(_Resp_uapi, "Galileo", parameters.Searchid, sw);
            }

            if (uapi_Supplier != null && uapi_Supplier.Length > 0 && AirRQ_uapi != null && AirRQ_uapi.Length > 0)
            {
                parameters.AirRQ = AirRQ_uapi;
                var sw = Stopwatch.StartNew();
                _Resp_uapi_sme = _uapiService.GetAirAvailabilityAsync(parameters, uapi_SupplierType, uapi_Supplier, Sector, true);
                apiCalls["Galileo_SME"] = (sw, _Resp_uapi_sme);
                // Initialize watcher status IMMEDIATELY when API task is created
                InitializeAndWatchApi(_Resp_uapi_sme, "Galileo_SME", parameters.Searchid, sw);
            }

            if (Cabin.Equals("Y") && e6_Supplier != null && e6_Supplier.Length > 0 && AirRQ_uapi_6e != null && AirRQ_uapi_6e.Length > 0)
            {
                parameters.AirRQ = AirRQ_uapi_6e;
                var sw = Stopwatch.StartNew();
                _Resp_6e = _uapiService.GetAirAvailability6EAsync(parameters, e6_SupplierType, e6_Supplier, Sector);
                apiCalls["IndiGo_6E"] = (sw, _Resp_6e);
                // Initialize watcher status IMMEDIATELY when API task is created
                InitializeAndWatchApi(_Resp_6e, "IndiGo_6E", parameters.Searchid, sw);
            }

            if (Cabin.Equals("Y") && parameters.JourneyType != "RTLCC" && spicejet_Supplier != null && spicejet_Supplier.Length > 0 && AirRQ_spicejet != null && AirRQ_spicejet.Length > 0)
            {
                parameters.AirRQ = AirRQ_spicejet;
                var sw = Stopwatch.StartNew();
                _Resp_spicejet = _spicejetService.GetAirAvailabilityAsync(parameters, spicejet_SupplierType, spicejet_Supplier, Sector, "LCC");
                apiCalls["Spicejet"] = (sw, _Resp_spicejet);
                // Initialize watcher status IMMEDIATELY when API task is created
                InitializeAndWatchApi(_Resp_spicejet, "Spicejet", parameters.Searchid, sw);
            }

            if (Cabin.Equals("Y") && parameters.JourneyType != "RTLCC" && spicejet_Supplier_max != null && spicejet_Supplier_max.Length > 0 && AirRQ_spicejet != null && AirRQ_spicejet.Length > 0)
            {
                parameters.AirRQ = AirRQ_spicejet;
                var sw = Stopwatch.StartNew();
                _Resp_spicejetMax = _spicejetService.GetAirAvailabilityAsync(parameters, spicejet_SupplierType_max, spicejet_Supplier_max, Sector, "MAX");
                apiCalls["Spicejet_MAX"] = (sw, _Resp_spicejetMax);
                // Initialize watcher status IMMEDIATELY when API task is created
                InitializeAndWatchApi(_Resp_spicejetMax, "Spicejet_MAX", parameters.Searchid, sw);
            }

            if (Cabin.Equals("Y") && spicejet_coupon_Supplier != null && spicejet_coupon_Supplier.Length > 0 && AirRQ_spicejet != null && AirRQ_spicejet.Length > 0)
            {
                parameters.AirRQ = AirRQ_spicejet;
                var sw = Stopwatch.StartNew();
                _Resp_spicejetCoupon = _spicejetService.GetAirAvailabilityAsync(parameters, spicejet_coupon_SupplierType, spicejet_coupon_Supplier, Sector, "COUPON");
                apiCalls["Spicejet_COUPON"] = (sw, _Resp_spicejetCoupon);
                // Initialize watcher status IMMEDIATELY when API task is created
                InitializeAndWatchApi(_Resp_spicejetCoupon, "Spicejet_COUPON", parameters.Searchid, sw);
            }

            if (Cabin.Equals("Y") && spicejet_corporate_Supplier != null && spicejet_corporate_Supplier.Length > 0 && AirRQ_spicejet != null && AirRQ_spicejet.Length > 0)
            {
                parameters.AirRQ = AirRQ_spicejet;
                var sw = Stopwatch.StartNew();
                Resp_spicejet_corporate = _spicejetService.GetAirAvailabilityAsync(parameters, spicejet_corporate_SupplierType, spicejet_corporate_Supplier, Sector, "CORPORATE");
                apiCalls["Spicejet_CORPORATE"] = (sw, Resp_spicejet_corporate);
                // Initialize watcher status IMMEDIATELY when API task is created
                InitializeAndWatchApi(Resp_spicejet_corporate, "Spicejet_CORPORATE", parameters.Searchid, sw);
            }
            

            // CRITICAL: Don't wait for APIs here! Return immediately so StoreResultAsync can complete independently
            // Final logging and marking complete will happen in background task below
            
            // Start background task to wait for all APIs and do final logging/marking complete
            // This runs independently and doesn't block the method return
            _ = Task.Run(async () =>
            {
                try
                {
                    // Wait for all APIs to complete (in background, doesn't block)
                    await Task.WhenAll(
                        _Resp_GetQP ?? Task.FromResult<string>(null),
                        _Resp_uapi ?? Task.FromResult<string>(null),
                        _Resp_uapi_sme ?? Task.FromResult<string>(null),
                        _Resp_6e ?? Task.FromResult<string>(null),
                        _Resp_spicejet ?? Task.FromResult<string>(null),
                        _Resp_spicejetMax ?? Task.FromResult<string>(null),
                        _Resp_spicejetCoupon ?? Task.FromResult<string>(null),
                        Resp_spicejet_corporate ?? Task.FromResult<string>(null)
                    );

                    // Get results for logging (tasks are now completed)
                    var qpResult = _Resp_GetQP != null ? _Resp_GetQP.Result : null;
                    var uapiResult = _Resp_uapi != null ? _Resp_uapi.Result : null;
                    var uapiSMEResult = _Resp_uapi_sme != null ? _Resp_uapi_sme.Result : null;
                    var uapi6eResult = _Resp_6e != null ? _Resp_6e.Result : null;
                    var spicejetResult = _Resp_spicejet != null ? _Resp_spicejet.Result : null;
                    var spicejetMaxResult = _Resp_spicejetMax != null ? _Resp_spicejetMax.Result : null;
                    var spicejetCouponResult = _Resp_spicejetCoupon != null ? _Resp_spicejetCoupon.Result : null;
                    var spicejetCorporateResult = Resp_spicejet_corporate != null ? Resp_spicejet_corporate.Result : null;

                    // Log API results (same as GetAvailableFights)
                    LogApiResults(apiCalls, new Dictionary<string, string>
                    {
                        { "Akasa", qpResult },
                        { "Galileo", uapiResult },
                        { "Galileo_SME", uapiSMEResult },
                        { "IndiGo_6E", uapi6eResult },
                        { "Spicejet", spicejetResult },
                        { "Spicejet_MAX", spicejetMaxResult },
                        { "Spicejet_COUPON", spicejetCouponResult },
                        { "Spicejet_CORPORATE", spicejetCorporateResult }
                    }, parameters.Searchid);

                    // Mark search as complete (after all APIs finish)
                    ProgressiveSearchCache.MarkComplete(parameters.Searchid);
                }
                catch (Exception ex)
                {
                    ProgressiveSearchCache.MarkComplete(parameters.Searchid);
                }
            });

            // Return immediately - don't wait for APIs!
            // StoreResultAsync tasks will complete independently as each API finishes
            // Results will be available in cache for polling immediately
            return string.Empty; // Return empty - results are in cache, not in return value
        }

        /// <summary>
        /// Initialize watcher status and attach ContinueWith IMMEDIATELY when API task is created
        /// This ensures watcher status exists right away and ContinueWith executes as each API completes
        /// </summary>
        private void InitializeAndWatchApi(Task<string> apiTask, string apiName, string searchId, System.Diagnostics.Stopwatch stopwatch)
        {
            // CRITICAL: Initialize watcher status IMMEDIATELY when API task is created
            // This ensures status exists right away for polling
            var cachedResult = ProgressiveSearchCache.GetResult(searchId);
            if (cachedResult == null) return;

            var watcherStatus = new ZealTravel.Domain.Services.AirelineManagement.WatcherStatus
            {
                ApiName = apiName,
                WatchStartTime = DateTime.UtcNow
            };
            cachedResult.WatcherStatuses[apiName] = watcherStatus;

            // CRITICAL: Attach ContinueWith IMMEDIATELY to the task
            // This ensures it executes as soon as THIS specific API completes, not all at once
            _ = apiTask.ContinueWith(async (completedTask) =>
            {
                try
                {
                    // CRITICAL: Capture stopwatch elapsed time EXACTLY when this task completes
                    // This gives us the accurate API response time for THIS specific API
                    long? apiResponseTimeMs = null;
                    if (stopwatch != null)
                    {
                        apiResponseTimeMs = stopwatch.ElapsedMilliseconds;
                    }
                    
                    var result = completedTask.IsFaulted ? null : await completedTask;
                    
                    // Track API response received
                    watcherStatus.ApiResponseReceivedTime = DateTime.UtcNow;
                    watcherStatus.ApiResponseReceived = !completedTask.IsFaulted;
                    watcherStatus.ApiResponseTimeMs = apiResponseTimeMs;
                    
                    if (completedTask.IsFaulted)
                    {
                        watcherStatus.ErrorMessage = completedTask.Exception?.GetBaseException()?.Message;
                    }

                    // Track format conversion (if result needs conversion)
                    watcherStatus.FormatConversionStartTime = DateTime.UtcNow;
                    // Format conversion happens in GetResponse/ConvertDataTableToString, but we track the time here
                    // The actual conversion is done when GetProgressiveResults is called
                    watcherStatus.FormatConversionEndTime = DateTime.UtcNow;
                    watcherStatus.FormatConversionTimeMs = 0; // Format conversion happens later during merge
                    
                    // Store result IMMEDIATELY when API completes
                    ProgressiveSearchCache.AddResult(searchId, apiName, result);
                }
                catch (Exception ex)
                {
                    // Store null if API failed
                    watcherStatus.ErrorMessage = ex.Message;
                    watcherStatus.ApiResponseReceived = false;
                    ProgressiveSearchCache.AddResult(searchId, apiName, null);
                }
            }, TaskContinuationOptions.ExecuteSynchronously).Unwrap();
        }

        /// <summary>
        /// Get current merged results from cache (for polling)
        /// Merges ALL available results immediately (doesn't wait for all APIs)
        /// </summary>
        public string GetProgressiveResults(string searchId, string journeyType, string companyId)
        {
            var cachedResult = ProgressiveSearchCache.GetResult(searchId);
            if (cachedResult == null)
            {
                return string.Empty;
            }

            // Print watcher logs
            PrintWatcherLogs(cachedResult);

            // Extract results from cache (only those that have completed)
            var qpResult = cachedResult.ApiResults.TryGetValue("Akasa", out var akasa) ? akasa : null;
            var uapiResult = cachedResult.ApiResults.TryGetValue("Galileo", out var galileo) ? galileo : null;
            var uapiSMEResult = cachedResult.ApiResults.TryGetValue("Galileo_SME", out var galileoSME) ? galileoSME : null;
            var uapi6eResult = cachedResult.ApiResults.TryGetValue("IndiGo_6E", out var indigo) ? indigo : null;
            var spicejetResult = cachedResult.ApiResults.TryGetValue("Spicejet", out var spicejet) ? spicejet : null;
            var spicejetMaxResult = cachedResult.ApiResults.TryGetValue("Spicejet_MAX", out var spicejetMax) ? spicejetMax : null;
            var spicejetCouponResult = cachedResult.ApiResults.TryGetValue("Spicejet_COUPON", out var spicejetCoupon) ? spicejetCoupon : null;
            var spicejetCorporateResult = cachedResult.ApiResults.TryGetValue("Spicejet_CORPORATE", out var spicejetCorp) ? spicejetCorp : null;

            // Track format conversion time
            var formatStart = DateTime.UtcNow;

            // Merge results using existing logic (same as GetAvailableFights)
            // This merges ALL available results immediately, not waiting for all APIs
            var flightResult = _getResponseData.GetResponse(
                null, spicejetMaxResult, spicejetResult, spicejetCouponResult, 
                spicejetCorporateResult, uapiResult, uapiSMEResult, uapi6eResult, 
                qpResult, journeyType, null);

            // Convert to XML string
            var responseString = _getResponseData.ConvertDataTableToString(flightResult, searchId, companyId);
            
            var formatEnd = DateTime.UtcNow;
            var formatTime = (long)(formatEnd - formatStart).TotalMilliseconds;

            // Update format conversion time for all watchers that have results
            foreach (var kvp in cachedResult.WatcherStatuses)
            {
                if (kvp.Value.FormatConversionStartTime == null)
                {
                    kvp.Value.FormatConversionStartTime = formatStart;
                    kvp.Value.FormatConversionEndTime = formatEnd;
                    kvp.Value.FormatConversionTimeMs = formatTime;
                }
            }
            
            return responseString;
        }

        /// <summary>
        /// Print watcher logs for all APIs
        /// </summary>
        private void PrintWatcherLogs(ZealTravel.Domain.Services.AirelineManagement.ProgressiveSearchResult cachedResult)
        {
            Console.WriteLine($"═══════════════════════════════════════════════════════");
            Console.WriteLine($"📊 WATCHER STATUS - SearchID: {cachedResult.SearchId}");
            Console.WriteLine($"═══════════════════════════════════════════════════════");
            
            // Expected API names in order
            var expectedApis = new[] { "Akasa", "Galileo", "Galileo_SME", "IndiGo_6E", "Spicejet", "Spicejet_MAX", "Spicejet_COUPON", "Spicejet_CORPORATE" };
            
            foreach (var apiName in expectedApis)
            {
                if (cachedResult.WatcherStatuses.TryGetValue(apiName, out var status))
                {
                    Console.WriteLine($"");
                    Console.WriteLine($"🔍 Watcher: {status.ApiName}");
                    Console.WriteLine($"   Watch Started: {(status.WatchStartTime?.ToString("HH:mm:ss.fff") ?? "N/A")}");
                    
                    if (status.ApiResponseReceived)
                    {
                        Console.WriteLine($"   ✅ API Response: RECEIVED at {status.ApiResponseReceivedTime?.ToString("HH:mm:ss.fff")}");
                        Console.WriteLine($"   ⏱️  API Response Time: {status.ApiResponseTimeMs}ms");
                    }
                    else if (!string.IsNullOrEmpty(status.ErrorMessage))
                    {
                        Console.WriteLine($"   ❌ API Response: FAILED");
                        Console.WriteLine($"   ⚠️  Error: {status.ErrorMessage}");
                    }
                    else
                    {
                        var elapsed = status.WatchStartTime.HasValue 
                            ? (long)(DateTime.UtcNow - status.WatchStartTime.Value).TotalMilliseconds 
                            : 0;
                        Console.WriteLine($"   ⏳ API Response: WATCHING... (elapsed: {elapsed}ms)");
                    }
                    
                    if (status.FormatConversionTimeMs.HasValue)
                    {
                        Console.WriteLine($"   ✅ Format Conversion: COMPLETED in {status.FormatConversionTimeMs}ms");
                    }
                    else
                    {
                        Console.WriteLine($"   ⏳ Format Conversion: PENDING");
                    }
                    
                    if (status.CacheAddSuccess)
                    {
                        Console.WriteLine($"   ✅ Cache Add: SUCCESS at {status.CacheAddEndTime?.ToString("HH:mm:ss.fff")}");
                        Console.WriteLine($"   ⏱️  Cache Add Time: {status.CacheAddTimeMs}ms");
                    }
                    else if (status.CacheAddStartTime.HasValue)
                    {
                        Console.WriteLine($"   ❌ Cache Add: FAILED");
                    }
                    else
                    {
                        Console.WriteLine($"   ⏳ Cache Add: PENDING");
                    }
                }
                else
                {
                    Console.WriteLine($"");
                    Console.WriteLine($"🔍 Watcher: {apiName}");
                    Console.WriteLine($"   ⏳ Status: NOT STARTED");
                }
            }
            
            Console.WriteLine($"═══════════════════════════════════════════════════════");
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

            try
            {
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
                }

                var departureStation = xmldoc.SelectSingleNode("AvailabilityRequest/DepartureStation").InnerText;
                var arrivalStation = xmldoc.SelectSingleNode("AvailabilityRequest/ArrivalStation").InnerText;

                Sector = "D";
                
                if (!_airlineDetailService.IsDomestic(departureStation, arrivalStation).Result)
                {
                    Sector = "I";
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private void LogApiResults(Dictionary<string, (Stopwatch stopwatch, Task<string> task)> apiCalls, Dictionary<string, string> results, string searchId)
        {
            try
            {
                Console.WriteLine("═══════════════════════════════════════════════════════");
                Console.WriteLine($"✈️  [FLIGHT SEARCH API RESULTS] SearchID: {searchId}");
                Console.WriteLine("═══════════════════════════════════════════════════════");

                // Group results by main API name
                var apiGroups = new Dictionary<string, List<(string name, int count, double timeMs)>>();
                
                foreach (var kvp in apiCalls)
                {
                    var apiName = kvp.Key;
                    var stopwatch = kvp.Value.stopwatch;
                    stopwatch.Stop();
                    var elapsedMs = stopwatch.Elapsed.TotalMilliseconds;
                    
                    var result = results.ContainsKey(apiName) ? results[apiName] : null;
                    int resultCount = 0;
                    
                    if (!string.IsNullOrEmpty(result) && result.IndexOf("RefID") != -1)
                    {
                        // Count occurrences of RefID to get number of flight results
                        resultCount = (result.Length - result.Replace("<RefID>", "").Length) / "<RefID>".Length;
                    }

                    // Group by main API (combine variants)
                    string mainApi = apiName;
                    if (apiName.StartsWith("Spicejet_"))
                        mainApi = "Spicejet";
                    else if (apiName.StartsWith("Galileo_"))
                        mainApi = "Galileo";
                    else if (apiName == "IndiGo_6E")
                        mainApi = "IndiGo";

                    if (!apiGroups.ContainsKey(mainApi))
                        apiGroups[mainApi] = new List<(string, int, double)>();

                    apiGroups[mainApi].Add((apiName, resultCount, elapsedMs));
                }

                // Log grouped results
                foreach (var group in apiGroups)
                {
                    var mainApi = group.Key;
                    var variants = group.Value;
                    var totalResults = variants.Sum(v => v.count);
                    var totalTime = variants.Sum(v => v.timeMs);
                    var avgTime = variants.Average(v => v.timeMs);

                    Console.WriteLine($"📊 {mainApi}:");
                    Console.WriteLine($"   Total Results: {totalResults}");
                    Console.WriteLine($"   Total Time: {totalTime:F2} ms");
                    Console.WriteLine($"   Average Time: {avgTime:F2} ms");
                    
                    if (variants.Count > 1)
                    {
                        foreach (var variant in variants)
                        {
                            Console.WriteLine($"   - {variant.name}: {variant.count} results in {variant.timeMs:F2} ms");
                        }
                    }
                    
                    if (_logger != null)
                    {
                        _logger.LogInformation("[FLIGHT SEARCH] {ApiName} returned {ResultCount} results in {TimeMs:F2} ms (SearchID: {SearchID})", 
                            mainApi, totalResults, totalTime, searchId);
                    }
                }

                Console.WriteLine("═══════════════════════════════════════════════════════");
                Console.Out.Flush();
            }
            catch (Exception ex)
            {
                if (_logger != null)
                {
                    _logger.LogError(ex, "Error logging API results for SearchID: {SearchID}", searchId);
                }
                Console.WriteLine($"🔴 [ERROR] Failed to log API results: {ex.Message}");
                Console.Out.Flush();
            }
        }
    }
}
