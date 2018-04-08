using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TomasGreen.Model.Models;
using TomasGreen.Web.Areas.Import.ViewModels;
using TomasGreen.Web.Balances;
using TomasGreen.Web.Data;
using TomasGreen.Web.Validations;
using TomasGreen.Web.Extensions;
using Microsoft.Extensions.Localization;
using TomasGreen.Web.Helpers;
using Microsoft.AspNetCore.Http;

namespace TomasGreen.Web.Areas.Import.Controllers
{
    [Area("Import")]
    public class PurchasedArticlesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly IStringLocalizer<PurchasedArticlesController> _localizer;
        public PurchasedArticlesController(ApplicationDbContext context, IHostingEnvironment hostingEnvironment, IStringLocalizer<PurchasedArticlesController> localizer)
        {
            _context = context;
            _hostingEnvironment = hostingEnvironment;
            _localizer = localizer;
        }
        // GET: PurchaseArticles
        public ActionResult Index()
        {
            var purchasedArticles = _context.PurchasedArticles.Include(c => c.Company).OrderBy(p => p.Arrived == false).ThenBy(p => p.Date).ThenBy(c => c.Company.Name);
            return View(purchasedArticles);
        }

        // GET: PurchaseArticles/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: PurchaseArticles/Create
        public ActionResult Create(int? id, int? PurchasedArticleId)
        {
            var model = new SavePurchasedArticleViewModel();
            model.PurchasedArticleDetail = new PurchasedArticleDetail();
            model.PurchasedArticleCostDetail = new PurchasedArticleCostDetail();
            model.PurchasedArticleDetail.OntheWayWarehouse = _context.Warehouses.Where(w => w.IsOnTheWay == true).FirstOrDefault();
            model.PurchasedArticleDetail.WarehouseID = model.PurchasedArticleDetail.OntheWayWarehouse.ID;
            if (id > 0)
            {
                model.PurchasedArticle = _context.PurchasedArticles.Include(d => d.PurchasedArticleDetails).Include(c => c.PurchasedArticleCostDetails).Where(p => p.ID == id).FirstOrDefault();
                foreach(var detail in model.PurchasedArticle.PurchasedArticleDetails)
                {
                    detail.OntheWayWarehouse = _context.Warehouses.Where(w => w.ID == detail.WarehouseID).FirstOrDefault();
                    if(detail.ArrivedDate != null)
                    {
                        detail.ArrivedAtWarehouse = _context.Warehouses.Where(w => w.ID == detail.ArrivedAtWarehouseID).FirstOrDefault();
                    }
                }
                foreach (var detail in model.PurchasedArticle.PurchasedArticleCostDetails)
                {
                    detail.Company = _context.Companies.Where(c=> c.ID == detail.CompanyID).FirstOrDefault();
                    detail.Currency = _context.Currencies.Where(c => c.ID == detail.CurrencyID).FirstOrDefault();
                }
            }
            else
            {
                model.PurchasedArticle = new PurchasedArticle();
                model.PurchasedArticle.Date = DateTime.Today;
            }


            AddPurchasedArticleLists(model);

            return View(model);
        }

        // POST: PurchaseArticles/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SavePurchasedArticleViewModel model, string SavePurchasedArticle, string SavePurchasedArticleDetail,
            string DeletePurchasedArticleDetail, string SavePurchasedArticleCostDetail, string DeletePurchasedArticleCostDetail)
        {
            try
            {
                var skippedErrors = ModelState.Keys;
                foreach (var key in skippedErrors)
                {
                    ModelState.Remove(key);
                }
                //Save PurchasedArticle
                if (!string.IsNullOrWhiteSpace(SavePurchasedArticle))
                {
                    var customModelValidator = PurchasedArticleValidation.PurchasedArticleIsValid(_context, model.PurchasedArticle);
                    if (customModelValidator.Value == false)
                    {
                        ModelState.AddModelError(customModelValidator.Property, customModelValidator.Message);
                        AddPurchasedArticleLists(model);
                        return View(model);
                    }
                    
                    if (model.PurchasedArticle.ID == 0)
                    {
                        //new Purchase
                        var guid = Guid.NewGuid();
                        model.PurchasedArticle.Guid = guid;
                        _context.PurchasedArticles.Add(model.PurchasedArticle);
                        await _context.SaveChangesAsync();
                        var savedPurchasedArticle = _context.PurchasedArticles.Where(p =>  p.Guid == guid && p.Date == model.PurchasedArticle.Date && p.CompanyID == model.PurchasedArticle.CompanyID && p.CurrencyID == model.PurchasedArticle.CurrencyID ).FirstOrDefault();
                        if (savedPurchasedArticle == null)
                        {
                            ModelState.AddModelError("", "Couldn't save.");
                            AddPurchasedArticleLists(model);
                            return View(model);
                        }
                        model.PurchasedArticle = savedPurchasedArticle;
                        if(model.PurchasedArticleDetail != null)
                        {
                            model.PurchasedArticleDetail.PurchasedArticleID = model.PurchasedArticle.ID;
                        }
                        else
                        {
                            model.PurchasedArticleDetail = new PurchasedArticleDetail { PurchasedArticleID = model.PurchasedArticle.ID };
                        }
                        if(model.PurchasedArticleCostDetail != null)
                        {
                            model.PurchasedArticleCostDetail.PurchasedArticleID = model.PurchasedArticle.ID;
                        }
                        else
                        {
                            model.PurchasedArticleCostDetail = new PurchasedArticleCostDetail { PurchasedArticleID = model.PurchasedArticle.ID };
                        }
                        
                        AddPurchasedArticleLists(model);
                        return View(model);
                    }
                    else
                    {
                        //Update Purchase
                    }

                }
                //Save PurchasedArticleDetail
                if (!string.IsNullOrWhiteSpace(SavePurchasedArticleDetail))
                {
                    model.PurchasedArticleDetail.PurchasedArticleID = model.PurchasedArticle.ID;
                    var customModelValidator = PurchasedArticleValidation.PurchasedArticleDetailIsValid(_context, model.PurchasedArticleDetail);
                    if (customModelValidator.Value == false)
                    {
                        ModelState.AddModelError(customModelValidator.Property, customModelValidator.Message);
                        AddPurchasedArticleLists(model);
                        return View(model);
                    }
                    if (model.PurchasedArticleDetail.ID == 0)
                    {
                        //new PurchasedArticleDetail
                        var totalPerUnit = ArticleHelper.TotalPerUnit(_context, model.PurchasedArticleDetail.ArticleID, model.PurchasedArticleDetail.QtyPackages, model.PurchasedArticleDetail.QtyExtra);
                        var totalPrice = totalPerUnit * model.PurchasedArticleDetail.UnitPrice;
                        model.PurchasedArticleDetail.TotalPerUnit = totalPerUnit;
                        _context.PurchasedArticleDetails.Add(model.PurchasedArticleDetail);
                        await _context.SaveChangesAsync();
                        AddPurchasedArticleLists(model); 
                        return RedirectToAction(nameof(Create), new { id = model.PurchasedArticle.ID });
                    }
                    else
                    {
                        //update PurchasedArticleDetail
                    }
                }
                //Delete PurchasedArticleDetail
                if (!string.IsNullOrWhiteSpace(DeletePurchasedArticleDetail))
                {

                }
                //Save PurchasedArticleCostDetail
                if (!string.IsNullOrWhiteSpace(DeletePurchasedArticleDetail))
                {

                }
                //Delete PurchasedArticleCostDetail
                if (!string.IsNullOrWhiteSpace(DeletePurchasedArticleDetail))
                {

                }
                return RedirectToAction(nameof(Index));
            }
            catch(Exception exception)
            {
                ModelState.AddModelError("", "Couldn't save." + exception.Message.ToString());
                AddPurchasedArticleLists(model);
                return View(model);
            }
        }
        // GET: Sales/Orders/Edit/5
        public async Task<IActionResult> EditRow(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var orderDetail = _context.OrderDetails.Where(d => d.ID == id).FirstOrDefault();
            if (orderDetail == null)
            {
                return NotFound();
            }
            return RedirectToAction(nameof(Create), new { id = orderDetail.OrderID, orderDetailId = orderDetail.ID });
        }
        // GET: PurchaseArticles/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: PurchaseArticles/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        private void AddPurchasedArticleLists(SavePurchasedArticleViewModel model)
        {
            ViewData["ArticleID"] = new SelectList(_context.Articles.Where(c => c.Archive == false).OrderBy(a => a.Name), "ID", "Name", model.PurchasedArticleDetail?.ArticleID);
            ViewData["CompanyID"] = new SelectList(_context.Companies.Where(c => c.IsOwner == false && c.Archive == false).OrderBy(c => c.Name), "ID", "Name", model.PurchasedArticle?.CompanyID);
            ViewData["CurrencyID"] = new SelectList(_context.Currencies.Where(c => c.Archive == false).OrderBy(c => c.Code), "ID", "Code", model.PurchasedArticle?.CurrencyID);
            ViewData["CostTypeID"] = new SelectList(_context.PurchasedArticleCostTypes.Where(c => c.Archive == false).OrderBy(c => c.Name), "ID", "Name", model.PurchasedArticleCostDetail?.PurchasedArticleCostTypeID);
            ViewData["CostCompanyID"] = new SelectList(_context.Companies.Where(c => c.IsOwner == false && c.Archive == false).OrderBy(c => c.Name), "ID", "Name", model.PurchasedArticleCostDetail?.CompanyID);
            ViewData["CostCurrencyID"] = new SelectList(_context.Currencies.Where(c => c.Archive == false).OrderBy(c => c.Code), "ID", "Code", model.PurchasedArticleCostDetail?.CurrencyID);

        }
    }
}