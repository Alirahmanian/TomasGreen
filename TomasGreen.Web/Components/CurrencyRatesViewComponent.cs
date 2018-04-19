using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TomasGreen.Web.Data;
using TomasGreen.Model.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using Microsoft.AspNetCore.Html;
using TomasGreen.Web.Services;

namespace TomasGreen.Web.Components
{
    public class LatestCurrencyRates : ViewComponent
    {
        private readonly ApplicationDbContext _context;
        public LatestCurrencyRates(ApplicationDbContext context)
        {
            _context = context;
          
        }
        public IViewComponentResult Invoke()
        {
            var hours = DateTime.Now.Subtract(CurrencyRates.GetLatestRateDate(_context)).Hours;
            var result = $"<b>rates</b>";
            var baseCurrency = _context.Currencies.Where(c => c.IsBase == true).FirstOrDefault();
            if(baseCurrency == null)
            {
                return new HtmlContentViewComponentResult(new HtmlString(result));
            }
            result += $" updated: <b>{hours.ToString()} </b>hour(s) ago";
            result += $" | <b>Base: {baseCurrency.Code}</b>";
            var mainCurrencies = _context.Currencies.Where(c => c.Archive == false && c.IsBase == false).OrderBy(c => c.Code);
            foreach(var currency in mainCurrencies)
            {
                result += $" | {currency.Code}: {currency.Rate.ToString()}"; 
            }
            result += $" | more »»";
            return new HtmlContentViewComponentResult(new HtmlString(result));
        }

       
    }
}
