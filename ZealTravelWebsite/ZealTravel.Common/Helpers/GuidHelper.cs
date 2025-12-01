using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZealTravel.Common.Helpers
{
    /// <summary>
    /// Provides helper methods for generating GUIDs in various formats.
    /// </summary>
    public static class GuidHelper
    {
        /// <summary>
        /// Generates a new GUID and encodes it as a URL-safe, 24-character Base64 string.
        /// </summary>
        /// <returns>A 24-char, URL-safe Base64 string representation of a GUID.</returns>
        public static string NewGuid24()
        {
            // Create a new GUID
            var guid = Guid.NewGuid();

            // Convert to Base64
            string base64 = Convert.ToBase64String(guid.ToByteArray());


            return base64;
        }
    }
}
