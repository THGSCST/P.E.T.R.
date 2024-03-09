using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using static System.Net.WebRequestMethods;

namespace PETR_Robot
{
    public static class ApiCalls
    {
        public static async Task<(double, double)> GetB3Quote(string ticker)
        {
            string url = $"https://cotacao.b3.com.br/mds/api/v1/instrumentQuotation/{ticker}";
            using (var httpClient = new HttpClient())
            {
                try
                {
                    var response = await httpClient.GetStringAsync(url);
                    var jObject = JObject.Parse(response);

                    // Navigate through the JSON structure to the first item in the Trad array
                    var currentPriceToken = jObject.SelectToken("$.Trad[0].scty.SctyQtn.curPrc");
                    var priceFluctuationToken = jObject.SelectToken("$.Trad[0].scty.SctyQtn.prcFlcn");

                    if (currentPriceToken != null && priceFluctuationToken != null)
                    {
                        double currentPrice = currentPriceToken.Value<double>();
                        double priceFluctuation = priceFluctuationToken.Value<double>();
                        return (currentPrice, priceFluctuation);
                    }

                    return (-1, -1);
                }
                catch(NullReferenceException nrEx)
                {
                    // NOK from B3: {"BizSts":{"cd":"NOK","desc":"Quotation not available."},"Msg":{"dtTm":"2024-03-05 10:02:00"}}
                    Console.WriteLine($"NOK from B3: {nrEx.Message}");
                    throw;
                }
                catch (HttpRequestException httpEx)
                {
                    // Handle HTTP errors here (e.g., 404 Not Found)
                    Console.WriteLine($"HTTP Request error: {httpEx.Message}");
                    throw;
                }
                catch (Exception ex)
                {
                    // Handle other errors here (e.g., parsing errors)
                    Console.WriteLine($"An error occurred: {ex.Message}");
                    throw;
                }
            }
        }
        public static async Task<(double, double)> GetMFinanceQuote(string ticker)
        {
            string url = $"https://mfinance.com.br/api/v1/stocks/{ticker}";
            using (var httpClient = new HttpClient())
            {
                try
                {
                    var response = await httpClient.GetStringAsync(url);
                    var jObject = JObject.Parse(response);
                    double closingPrice = jObject["lastPrice"].Value<double>();
                    double change = jObject["change"].Value<double>();
                    return (closingPrice, change);
                }
                catch (HttpRequestException httpEx)
                {
                    // Handle HTTP errors here (e.g., 404 Not Found)
                    Console.WriteLine($"HTTP Request error: {httpEx.Message}");
                    throw;
                }
                catch (Exception ex)
                {
                    // Handle other errors here (e.g., parsing errors)
                    Console.WriteLine($"An error occurred: {ex.Message}");
                    throw;
                }
            }
        }
    }
}
