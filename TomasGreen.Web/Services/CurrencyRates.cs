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
        private const string key = "618af0cb0f4842d08114545d83dbd9ce";
        public static void FetchRates(ApplicationDbContext _context, bool forceFetch)
        {
            var latestDate = GetLatestRateDate(_context);
            if (latestDate != null)
            {
                if (DateTime.Now.Subtract(latestDate).TotalHours > 24 || forceFetch)
                {
                    var latest = CurrencyRates.GetLatest(_context);
                }
            }
        }
        private static bool GetLatest(ApplicationDbContext _context)
        {
            var jSonCurrency = new JSonCurrency();
            using (var client = new HttpClient(new HttpClientHandler { UseDefaultCredentials = true }))
            {
                var link = "https://openexchangerates.org/api/latest.json?app_id=" + key;
                var response = client.GetStringAsync(link);
                jSonCurrency = JsonConvert.DeserializeObject<JSonCurrency>(response.Result);

            }
            return SaveLatest(_context, jSonCurrency);
        }
        private static bool SaveLatest(ApplicationDbContext _context, JSonCurrency jSonCurrency)
        {
            var updated = 0; var added = 0;
            if (jSonCurrency != null)
            {
                foreach (var rate in jSonCurrency.Rates)
                {
                    var currency = new Currency
                    {
                        Rate = rate.Value,
                        Code = rate.Key,
                        Date = CurrencyRates.ConvertFromUnixTimestamp(double.Parse(jSonCurrency.Timestamp))
                    };
                    currency.IsBase = (currency.Code == jSonCurrency.Base);
                    var savedCurrency = _context.Currencies.Where(c => c.Code == currency.Code).FirstOrDefault();
                    if (savedCurrency != null)
                    {
                        savedCurrency.Date = currency.Date;
                        savedCurrency.Rate = currency.Rate;
                        _context.Currencies.Update(savedCurrency);
                        updated++;
                    }
                    else
                    {
                        _context.Currencies.Add(currency);
                        added++;
                    }
                }
                _context.SaveChanges();
            }
            return (updated > 0 || added > 0);
        }
        public static DateTime GetLatestRateDate(ApplicationDbContext _context)
        {
            return _context.Currencies.Where(c => c.Archive == false).OrderBy(c => c.Date).FirstOrDefault().Date;

        }
        private static DateTime ConvertFromUnixTimestamp(double timestamp)
        {
            DateTime origin = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return origin.AddSeconds(timestamp);
        }
        private static void GetCurrenies(ApplicationDbContext _context)
        {
            using (var client = new HttpClient(new HttpClientHandler { UseDefaultCredentials = true }))
            {
                var link = "https://openexchangerates.org/api/currencies.json";
                var response = client.GetStringAsync(link);
                Dictionary<string, string> currencyList = JsonConvert.DeserializeObject<Dictionary<string, string>>(response.Result);
               
                foreach (var currency in currencyList)
                {
                    var savedCurrency = _context.Currencies.Where(c => c.Code == currency.Key).FirstOrDefault();
                    if(savedCurrency != null)
                    {
                        savedCurrency.Name = currency.Value;
                        _context.Currencies.Update(savedCurrency);
                    }
                }
                _context.SaveChanges();
            }
        }
        private class JSonCode
        {
            public Dictionary<string, string> codes { get; set; }
        }
        private class JSonCurrency
        {
            public string Disclaimer { get; set; }
            public string License { get; set; }
            public string Timestamp { get; set; }
            public string Base { get; set; }
            public Dictionary<string, decimal> Rates { get; set; }

        }



    }
}
