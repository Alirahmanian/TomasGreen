using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TomasGreen.Web.Data;
using TomasGreen.Model.Models;

namespace TomasGreen.Web.Areas.Import.Components
{
    public class ArticleWarehouseAvailability : ViewComponent
    {
        private readonly ApplicationDbContext _context;

        public ArticleWarehouseAvailability(ApplicationDbContext context)
        {
            _context = context;
        }
        public IViewComponentResult Invoke(int articleId,  int warehouseId)
        {
            var articleWarehouseBalance = new ArticleWarehouseBalance();
            if (articleId > 0 && warehouseId > 0)
            {
                return View(_context.ArticleWarehouseBalances.Where(a => a.ArticleID == articleId && a.WarehouseID == warehouseId).FirstOrDefault());
            }
            return View(articleWarehouseBalance);
        }
    }
}
