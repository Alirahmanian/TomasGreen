using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TomasGreen.Model.Models;
using TomasGreen.Web.Data;

namespace TomasGreen.Web.Areas.Stock.Controllers
{
    [Area("Stock")]
    public class ArticleWarehouseBalancesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ArticleWarehouseBalancesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Stock/ArticleWarehouseBalances
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.ArticleWarehouseBalances.Include(a => a.Article).Include(a => a.Warehouse);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Stock/ArticleWarehouseBalances/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var articleWarehouseBalance = await _context.ArticleWarehouseBalances
                .Include(a => a.Article)
                .Include(a => a.Warehouse)
                .SingleOrDefaultAsync(m => m.ID == id);
            if (articleWarehouseBalance == null)
            {
                return NotFound();
            }

            return View(articleWarehouseBalance);
        }


        private bool ArticleWarehouseBalanceExists(long id)
        {
            return _context.ArticleWarehouseBalances.Any(e => e.ID == id);
        }
    }
}
