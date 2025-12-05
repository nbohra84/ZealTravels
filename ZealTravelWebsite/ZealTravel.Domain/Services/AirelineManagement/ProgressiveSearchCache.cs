using System.Collections.Concurrent;
using System.Collections.Generic;

namespace ZealTravel.Domain.Services.AirelineManagement
{
    /// <summary>
    /// Simple in-memory cache for progressive search results (One-Way, Round Trip)
    /// Stores results as APIs complete
    /// </summary>
    public static class ProgressiveSearchCache
    {
        private static readonly ConcurrentDictionary<string, ProgressiveSearchResult> _cache = new ConcurrentDictionary<string, ProgressiveSearchResult>();

        /// <summary>
        /// Initialize a new search in cache
        /// </summary>
        public static void Initialize(string searchId, string journeyType, string companyId, int totalApis)
        {
            _cache.TryAdd(searchId, new ProgressiveSearchResult
            {
                SearchId = searchId,
                JourneyType = journeyType,
                CompanyId = companyId,
                ApiResults = new ConcurrentDictionary<string, string>(),
                IsComplete = false,
                CompletedApis = new ConcurrentBag<string>(),
                TotalApis = totalApis,
                WatcherStatuses = new ConcurrentDictionary<string, WatcherStatus>()
            });
        }

        /// <summary>
        /// Add result from a completed API
        /// IMPORTANT: This stores IMMEDIATELY - results are available right away for polling
        /// Uses memory barrier to ensure immediate visibility across threads
        /// </summary>
        public static void AddResult(string searchId, string apiName, string result)
        {
            if (_cache.TryGetValue(searchId, out var cachedResult))
            {
                var cacheAddStart = System.DateTime.UtcNow;
                
                // Store result in cache IMMEDIATELY (even if null - to track that API completed)
                // ConcurrentDictionary indexer is thread-safe and immediately visible to other threads
                cachedResult.ApiResults[apiName] = result;
                
                // Always add to CompletedApis to track progress (API completed, even if no results)
                // This ensures progress bar updates correctly
                cachedResult.CompletedApis.Add(apiName);
                
                // Memory barrier to ensure write is immediately visible to other threads
                System.Threading.Thread.MemoryBarrier();
                
                var cacheAddEnd = System.DateTime.UtcNow;
                var cacheAddTime = (long)(cacheAddEnd - cacheAddStart).TotalMilliseconds;
                
                // Update watcher status
                if (cachedResult.WatcherStatuses.TryGetValue(apiName, out var watcherStatus))
                {
                    watcherStatus.CacheAddStartTime = cacheAddStart;
                    watcherStatus.CacheAddEndTime = cacheAddEnd;
                    watcherStatus.CacheAddTimeMs = cacheAddTime;
                    watcherStatus.CacheAddSuccess = cachedResult.ApiResults.ContainsKey(apiName);
                }
            }
        }

        /// <summary>
        /// Mark search as complete
        /// </summary>
        public static void MarkComplete(string searchId)
        {
            if (_cache.TryGetValue(searchId, out var cachedResult))
            {
                cachedResult.IsComplete = true;
            }
        }

        /// <summary>
        /// Get current cached result
        /// </summary>
        public static ProgressiveSearchResult GetResult(string searchId)
        {
            _cache.TryGetValue(searchId, out var result);
            return result;
        }

        /// <summary>
        /// Remove old searches (cleanup)
        /// </summary>
        public static void Cleanup()
        {
            // Remove searches older than 1 hour
            // This is a simple implementation - can be enhanced later
        }
    }

    /// <summary>
    /// Represents a progressive search result
    /// </summary>
    public class ProgressiveSearchResult
    {
        public string SearchId { get; set; }
        public string JourneyType { get; set; }
        public string CompanyId { get; set; }
        public ConcurrentDictionary<string, string> ApiResults { get; set; } = new ConcurrentDictionary<string, string>();
        public bool IsComplete { get; set; }
        public ConcurrentBag<string> CompletedApis { get; set; } = new ConcurrentBag<string>();
        public int TotalApis { get; set; }
        public ConcurrentDictionary<string, WatcherStatus> WatcherStatuses { get; set; } = new ConcurrentDictionary<string, WatcherStatus>();
    }

    /// <summary>
    /// Tracks watcher status for each API
    /// </summary>
    public class WatcherStatus
    {
        public string ApiName { get; set; }
        public DateTime? WatchStartTime { get; set; }
        public DateTime? ApiResponseReceivedTime { get; set; }
        public bool ApiResponseReceived { get; set; }
        public long? ApiResponseTimeMs { get; set; }
        public DateTime? FormatConversionStartTime { get; set; }
        public DateTime? FormatConversionEndTime { get; set; }
        public long? FormatConversionTimeMs { get; set; }
        public DateTime? CacheAddStartTime { get; set; }
        public DateTime? CacheAddEndTime { get; set; }
        public bool CacheAddSuccess { get; set; }
        public long? CacheAddTimeMs { get; set; }
        public string ErrorMessage { get; set; }
    }
}

