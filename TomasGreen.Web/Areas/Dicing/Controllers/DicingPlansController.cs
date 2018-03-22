using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using TomasGreen.Model.Models;
using TomasGreen.Web.BaseModels;
using TomasGreen.Web.Data;
using TomasGreen.Web.Validations;
using TomasGreen.Web.Balances;
using TomasGreen.Web.Extensions;
using TomasGreen.Web.Areas.Packing.ViewModels;
using TomasGreen.Web.Areas.Dicing.ViewModels;

namespace TomasGreen.Web.Areas.Dicing.Controllers
{
    [Area("Dicing")]
    public class DicingPlansController : BaseController
    {
        private readonly ApplicationDbContext _context;
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly IStringLocalizer<DicingPlansController> _localizer;

        public DicingPlansController(ApplicationDbContext context, IHostingEnvironment hostingEnvironment, IStringLocalizer<DicingPlansController> localizer)
        {
            _context = context;
            _hostingEnvironment = hostingEnvironment;
            _localizer = localizer;
        }

        // GET: Packing/PackingPlans
        public async Task<IActionResult> Index()
        {
            return View(await _context.DicingPlans
                            .Include(e => e.Manager)
                            .Include(c => c.Company)
                            .Include(w => w.Warehouse)
                            .Include(a => a.Article)
                            .ToListAsync());
        }
        
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var DicingPlan = await _context.DicingPlans
                            .Include(e => e.Manager)
                            .Include(c => c.Company)
                            .Include(w => w.Warehouse)
                            .Include(a => a.Article)
                            .Include(d => d.DicingPlanDetails)
                            .SingleOrDefaultAsync(m => m.ID == id);
            if (DicingPlan == null)
            {
                return NotFound();
            }

            return View(DicingPlan);
        }

       
        public IActionResult Create(int? id, int? dicingPlanDetailId )
        {
            var model = new DicingPlanViewModel();
            model.DicingPlan = new DicingPlan();
            model.DicingPlan.Date = DateTime.Today;
            model.DicingPlanDetail = new DicingPlanDetail();
            if (id > 0)
            {
                model.DicingPlan = _context.DicingPlans.Include(d => d.DicingPlanDetails).Where(p => p.ID == id).FirstOrDefault();
                if (model.DicingPlan != null && dicingPlanDetailId > 0)
                {
                    model.DicingPlanDetail = _context.DicingPlanDetails.Where(d => d.ID == dicingPlanDetailId).FirstOrDefault();
                }
            }
           
            GetPackingPlanLists(model);

            return View(model);
        }
        private void GetPackingPlanLists(DicingPlanViewModel model)
        {
            if (model.DicingPlan.ID > 0)
            {
                ViewBag.SavedDicingPlanID = model.DicingPlan.ID;
                ViewBag.SavedCompanyID = model.DicingPlan.CompanyID;
            }
            ViewData["Warehouse"] = new SelectList(_context.Warehouses, "ID", "Name", model?.DicingPlan?.WarehouseID);
            ViewData["ArticleID"] = new SelectList(_context.Articles, "ID", "Name", model?.DicingPlan?.ArticleID);
            ViewData["CompanyID"] = new SelectList(_context.Companies, "ID", "Name", model?.DicingPlan?.CompanyID);
            ViewData["ManagerID"] = new SelectList(_context.Employees, "ID", "FullName", model?.DicingPlan?.ManagerID);
            //ViewData["PackagingMaterialPackageID"] = new SelectList(_context.PackagingMaterials, "ID", "Name", model?.PackingPlanMix?.PackagingMaterialPackageID);
            //ViewData["PackagingMaterialBagID"] = new SelectList(_context.PackagingMaterials, "ID", "Name", model?.PackingPlanMix?.PackagingMaterialBagID);

            //if (model?.PackingPlan?.CompanyID > 0)
            //{
            //    ViewData["WarehouseID"] = new SelectList(GetFromWarehouseList(model.PackingPlan.CompanyID), "ID", "Name", model.PackingPlanMixArticle.WarehouseID);
            //    ViewData["ToWarehouseID"] = new SelectList(GetToWarehouseList(model.PackingPlan.CompanyID), "ID", "Name", model.PackingPlanMix.ToWarehouseID);
            //    if (model.PackingPlanMixArticle.WarehouseID > 0)
            //    {
            //        ViewData["ArticleID"] = new SelectList(GetArticleList(model.PackingPlanMixArticle.WarehouseID, model.PackingPlan.CompanyID), "ID", "Name", model.PackingPlanMixArticle.ArticleID);
            //    }
            //}
        }
    }
}