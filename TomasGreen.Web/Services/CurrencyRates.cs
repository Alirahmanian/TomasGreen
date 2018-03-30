using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using TomasGreen.Model.Models;
using TomasGreen.Web.Data;

namespace TomasGreen.Web.Services
{
    public static class CurrencyRates
    {
        public static JSonCurrency GetLatest(ApplicationDbContext _context, ExternalApi api, ExternalApiFunction function)
        {
            var functionIsApis = _context.ExternalApiFunctions.Any(f => f.ID == function.ID && f.ExternalApiID == api.ID);
            var jSonCurrency = new JSonCurrency();
            if (!functionIsApis)
            {
                return jSonCurrency;
            }
            else
            {
                using (var client = new HttpClient(new HttpClientHandler { UseDefaultCredentials = true }))
                {
                    var url = $"{api.Link}{function.Name}?app_id={api.Key}";
                    var link = "https://openexchangerates.org/api/latest.json?app_id=618af0cb0f4842d08114545d83dbd9ce";
                    var response = client.GetStringAsync(url);
                    jSonCurrency = JsonConvert.DeserializeObject<JSonCurrency>(response.Result);
                    if (jSonCurrency != null)
                    {
                        List<Currency> Currencies = new List<Currency>();
                        foreach (var rate in jSonCurrency.Rates)
                        {
                            var currency = new Currency
                            {
                                Rate = rate.Value,
                                Code = rate.Key,
                                Date = CurrencyRates.ConvertFromUnixTimestamp(double.Parse(jSonCurrency.Timestamp))
                            };
                            Currencies.Add(currency);
                        }


                    }
                }
                return jSonCurrency;
            }

           
        }
        public static void GetCurrencies(ApplicationDbContext _context)
        {

        }

        public static DateTime ConvertFromUnixTimestamp(double timestamp)
        {
            DateTime origin = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return origin.AddSeconds(timestamp);
        }

        public class JSonCurrency
        {
            public string Disclaimer { get; set; }
            public string License { get; set; }
            public string Timestamp { get; set; }
            public string Base { get; set; }
            public Dictionary<string, decimal> Rates { get; set; }

           
        }
        


    }
}
