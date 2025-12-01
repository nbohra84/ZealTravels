using System;
using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace ZealTravel.Common.Helpers
{
    public class CurrencyHelper
    {
        private const string ApiKey = "b43ef2180d84bc3ef24ed7206b1d3f2e";
        private const string BaseUrl = "http://data.fixer.io/api/latest";

        public static async Task<string> GetCurrencyConvertToINR(string CURR)
        {
            string totalAmount = "100.0"; // Default value if something fails
            try
            {
                CURR = CURR.ToUpper().Trim();
                string url = $"{BaseUrl}?access_key={ApiKey}&symbols={CURR},INR";

                using (HttpClient client = new HttpClient())
                {
                    HttpResponseMessage response = await client.GetAsync(url);
                    response.EnsureSuccessStatusCode();

                    string jsonResponse = await response.Content.ReadAsStringAsync();
                    var jsonDocument = JsonDocument.Parse(jsonResponse);

                    if (jsonDocument.RootElement.GetProperty("success").GetBoolean())
                    {
                        var rates = jsonDocument.RootElement.GetProperty("rates");
                        if (rates.TryGetProperty("INR", out JsonElement inrElement) &&
                            rates.TryGetProperty(CURR, out JsonElement currElement))
                        {
                            double eurToInr = inrElement.GetDouble();
                            double eurToCurr = currElement.GetDouble();
                            double inrToCurr = Math.Round(eurToCurr / eurToInr, 2);

                            totalAmount = inrToCurr.ToString();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Log the exception if necessary
            }

            return totalAmount;
        }
    }
}

