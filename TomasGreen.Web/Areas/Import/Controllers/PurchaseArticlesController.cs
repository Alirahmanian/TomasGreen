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
using TomasGreen.Web.Areas.Import.Helpers;

namespace TomasGreen.Web.Areas.Import.Controllers
{
    [Area("Import")]
    public class PurchaseArticlesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly IStringLocalizer<PurchaseArticlesController> _localizer;
        public PurchaseArticlesController(ApplicationDbContext context, IHostingEnvironment hostingEnvironment, IStringLocalizer<PurchaseArticlesController> localizer)
        {
            _context = context;
            _hostingEnvironment = hostingEnvironment;
            _localizer = localizer;
        }

        public ActionResult Index()
        {
            var PurchaseArticles = _context.PurchaseArticles.Include(c => c.Company).Include(d => d.PurchaseArticleDetails).OrderBy(p => p.Arrived == false).ThenBy(p => p.Date).ThenBy(c => c.Company.Name);
            foreach(var item in PurchaseArticles)
            {
                item.TotalPrice = item.GetTotalPrice();
            }
            return View(PurchaseArticles);
        }

        public ActionResult Create(int? id, int? activeTab, int? articleDetailId, int? costDetailId, int? shortageDealingDetailId, int? containerDetailId)
        {
            var model = new SavePurchaseArticleViewModel();
            model.ActiveTab = (activeTab > 0)? (int)activeTab: PurchaseArticleTabs.Article;
            model.PurchaseArticleDetail = new PurchaseArticleDetail();
            model.PurchaseArticleCostDetail = new PurchaseArticleCostDetail();
            model.PurchaseArticleContainerDetail = new PurchaseArticleContainerDetail();
            model.PurchaseArticleShortageDealingDetail = new PurchaseArticleShortageDealingDetail();
           // model.PurchaseArticleDetail.OntheWayWarehouse = _context.Warehouses.Where(w => w.IsOnTheWay == true).FirstOrDefault();
            //model.PurchaseArticleDetail.WarehouseID = model.PurchaseArticleDetail.OntheWayWarehouse.ID;
            if (id > 0)
            {
                model.PurchaseArticle = _context.PurchaseArticles.Include(d => d.PurchaseArticleDetails).Include(c => c.PurchaseArticleCostDetails)
                    .Include(s => s.PurchaseArticleShortageDealingDetails).Include(c => c.PurchaseArticleContainerDetails).Where(p => p.ID == id).FirstOrDefault();
                foreach(var detail in model.PurchaseArticle.PurchaseArticleDetails)
                {
                    detail.OntheWayWarehouse = _context.Warehouses.Where(w => w.ID == detail.WarehouseID).FirstOrDefault();
                    if(detail.ArrivedDate != null)
                    {
                        detail.ArrivedAtWarehouse = _context.Warehouses.Where(w => w.ID == detail.ArrivedAtWarehouseID).FirstOrDefault();
                    }
                }
                foreach (var detail in model.PurchaseArticle.PurchaseArticleCostDetails)
                {
                    detail.Company = _context.Companies.Where(c=> c.ID == detail.CompanyID).FirstOrDefault();
                    detail.Currency = _context.Currencies.Where(c => c.ID == detail.CurrencyID).FirstOrDefault();
                    detail.PaymentType = _context.PaymentTypes.Find(detail.PaymentTypeID);
                }
                foreach (var detail in model.PurchaseArticle.PurchaseArticleShortageDealingDetails)
                {
                    detail.Company = _context.Companies.Where(c => c.ID == detail.CompanyID).FirstOrDefault();
                    detail.Currency = _context.Currencies.Where(c => c.ID == detail.CurrencyID).FirstOrDefault();
                }
                if (articleDetailId > 0)
                {
                    model.PurchaseArticleDetail = _context.PurchaseArticleDetails.Find(articleDetailId);
                    model.ActiveTab = PurchaseArticleTabs.Article;
                }
                if (costDetailId > 0)
                {
                    model.PurchaseArticleCostDetail = _context.PurchaseArticleCostDetails.Find(costDetailId);
                    model.ActiveTab = PurchaseArticleTabs.Cost;
                }
                if (shortageDealingDetailId > 0)
                {
                    model.PurchaseArticleShortageDealingDetail = _context.PurchaseArticleShortageDealingDetails.Find(shortageDealingDetailId);
                    model.ActiveTab = PurchaseArticleTabs.ShortageDealing;
                }
                if (containerDetailId > 0)
                {
                    model.PurchaseArticleContainerDetail = _context.PurchaseArticleContainerDetails.Find(containerDetailId);
                    model.ActiveTab = PurchaseArticleTabs.Container;
                }
            }
            else
            {
                model.PurchaseArticle = new PurchaseArticle();
                model.PurchaseArticle.Date = DateTime.Today;
            }

            AddPurchaseArticleLists(model);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SavePurchaseArticleViewModel model, string savePurchaseArticle, string savePurchaseArticleDetail,
            string savePurchaseArticleCostDetail, string savePurchaseArticleContainerDetail, string savePurchaseArticleShortageDealingDetail)
        {
            try
            {
                var skippedErrors = ModelState.Keys;
                foreach (var key in skippedErrors)
                {
                    ModelState.Remove(key);
                }
                //Save PurchaseArticle
                if (!string.IsNullOrWhiteSpace(savePurchaseArticle))
                {
                    if(model.PurchaseArticle == null)
                    {
                        ModelState.AddModelError("", "Couldn't save.");
                        if (_hostingEnvironment.IsDevelopment())
                        {
                            ModelState.AddModelError("", "Model's PurchaseArticle is missing.");
                        }
                        AddPurchaseArticleLists(model);
                        return View(model);
                    }
                    var customModelValidator = PurchaseArticleValidation.PurchaseArticleIsValid(_context, model.PurchaseArticle);
                    if (customModelValidator.Value == false)
                    {
                        ModelState.AddModelError(customModelValidator.Property, customModelValidator.Message);
                        AddPurchaseArticleLists(model);
                        return View(model);
                    }
                    
                    if (model.PurchaseArticle.ID == 0)
                    {
                        //new Purchase
                        var guid = Guid.NewGuid();
                        model.PurchaseArticle.Guid = guid;
                        _context.PurchaseArticles.Add(model.PurchaseArticle);
                        await _context.SaveChangesAsync();
                        var savedPurchaseArticle = _context.PurchaseArticles.Where(p =>  p.Guid == guid && p.Date == model.PurchaseArticle.Date && p.CompanyID == model.PurchaseArticle.CompanyID && p.CurrencyID == model.PurchaseArticle.CurrencyID ).FirstOrDefault();
                        if (savedPurchaseArticle == null)
                        {
                            ModelState.AddModelError("", "Couldn't save.");
                            AddPurchaseArticleLists(model);
                            return View(model);
                        }
                        model.PurchaseArticle = savedPurchaseArticle;
                        if(model.PurchaseArticleDetail != null)
                        {
                            model.PurchaseArticleDetail.PurchaseArticleID = model.PurchaseArticle.ID;
                        }
                        else
                        {
                            model.PurchaseArticleDetail = new PurchaseArticleDetail { PurchaseArticleID = model.PurchaseArticle.ID };
                        }
                        if(model.PurchaseArticleCostDetail != null)
                        {
                            model.PurchaseArticleCostDetail.PurchaseArticleID = model.PurchaseArticle.ID;
                        }
                        else
                        {
                            model.PurchaseArticleCostDetail = new PurchaseArticleCostDetail { PurchaseArticleID = model.PurchaseArticle.ID };
                        }
                        
                        AddPurchaseArticleLists(model);
                        return View(model);
                    }
                    else
                    {
                        //Update Purchase
                        
                        var savedPurchaseArticle = _context.PurchaseArticles.Where(p => p.ID == model.PurchaseArticle.ID).FirstOrDefault();
                        if(savedPurchaseArticle == null)
                        {
                            ModelState.AddModelError("", "Couldn't save.");
                            if(_hostingEnvironment.IsDevelopment())
                            {
                                ModelState.AddModelError("", "Couldn't find saved PurchaseArticle.");
                            }
                            AddPurchaseArticleLists(model);
                            return View(model);
                        }
                        if(!savedPurchaseArticle.Arrived)
                        {
                            if (savedPurchaseArticle.CompanyID != model.PurchaseArticle.CompanyID)
                            {
                                //rollback article and company balances
                                var PurchaseArticleDetails = _context.PurchaseArticleDetails.Where(d => d.PurchaseArticleID == savedPurchaseArticle.ID);
                                foreach (var item in PurchaseArticleDetails)
                                {

                                }
                            }
                            
                        }
                        else
                        {
                            //Arrived: company cann't be changed but costs and shortages
                        }

                    }

                }
                //Save PurchaseArticleDetail
                if (!string.IsNullOrWhiteSpace(savePurchaseArticleDetail))
                {
                    //Check the model
                    if (model.PurchaseArticleDetail == null)
                    {
                        ModelState.AddModelError("", "Couldn't save.");
                        if (_hostingEnvironment.IsDevelopment())
                        {
                            ModelState.AddModelError("", "Model's PurchaseArticleDetail is missing.");
                        }
                        AddPurchaseArticleLists(model);
                        return View(model);
                    }

                    model.PurchaseArticleDetail.PurchaseArticleID = model.PurchaseArticle.ID;
                    var customModelValidator = PurchaseArticleValidation.PurchaseArticleDetailIsValid(_context, model.PurchaseArticleDetail);
                    if (customModelValidator.Value == false)
                    {
                        ModelState.AddModelError(customModelValidator.Property, customModelValidator.Message);
                        AddPurchaseArticleLists(model);
                        return View(model);
                    }
                    
                    if (model.PurchaseArticleDetail.ID == 0)
                    {
                        //new PurchaseArticleDetail
                        using (var dbContextTransaction = _context.Database.BeginTransaction())
                        {
                            var totalPerUnit = ArticleHelper.TotalPerUnit(_context, model.PurchaseArticleDetail.ArticleID, model.PurchaseArticleDetail.QtyPackages, model.PurchaseArticleDetail.QtyExtra);
                            var totalPrice = totalPerUnit * model.PurchaseArticleDetail.UnitPrice;
                            model.PurchaseArticleDetail.TotalPerUnit = totalPrice;

                            var guid = Guid.NewGuid();
                            model.PurchaseArticleDetail.Guid = guid;
                            _context.PurchaseArticleDetails.Add(model.PurchaseArticleDetail);
                            await _context.SaveChangesAsync();
                            var savedPurchaseArticleDetail = _context.PurchaseArticleDetails.Include(p => p.PurchaseArticle).Where(d => d.PurchaseArticleID == model.PurchaseArticle.ID && d.Guid == guid).FirstOrDefault();
                            if (savedPurchaseArticleDetail == null)
                            {
                                ModelState.AddModelError("", "Couldn't save.");
                                if (_hostingEnvironment.IsDevelopment())
                                {
                                    ModelState.AddModelError("", "Couldn't save PurchaseArticleDetail");
                                }
                                AddPurchaseArticleLists(model);
                                return View(model);
                            }

                            //***** Article balance go *****
                            //Add ArticleBalanceDetails
                            var articleWarehouseBalanceDetail = GetArticleWarehouseBalanceDetail(savedPurchaseArticleDetail);
                            if(articleWarehouseBalanceDetail == null)
                            {
                                ModelState.AddModelError("", "Couldn't save.");
                                if (_hostingEnvironment.IsDevelopment())
                                {
                                    ModelState.AddModelError("", $"Couldn't create an articleWarehouseBalanceDetail.");
                                }
                                AddPurchaseArticleLists(model);
                                return View(model);
                            }
                            _context.ArticleWarehouseBalanceDetails.Add(articleWarehouseBalanceDetail);
                            var articleInOut = new ArticleInOut
                            {
                                ArticleID = model.PurchaseArticleDetail.ArticleID,
                                WarehouseID = model.PurchaseArticleDetail.WarehouseID,
                                CompanyID = ArticleBalance.GetWarehouseCompany(_context, _context.Warehouses.Where(w => w.ID == model.PurchaseArticleDetail.WarehouseID).FirstOrDefault()),
                                QtyPackages = model.PurchaseArticleDetail.QtyPackages,
                                QtyExtra = model.PurchaseArticleDetail.QtyExtra
                            };
                            var result = ArticleBalance.Add(_context, articleInOut);
                            if (result.Value == false)
                            {
                                ModelState.AddModelError("", "Couldn't save.");
                                if (_hostingEnvironment.IsDevelopment())
                                {
                                    ModelState.AddModelError("", JSonHelper.ToJSon(result));
                                }
                                AddPurchaseArticleLists(model);
                                return View(model);
                            }
                            //***** Article balance stop *****

                            //***** Company balance go *****
                            //Add CompanyCreditDebitBalanceDetail
                            var companyCreditDebitBalanceDetail = GetCompanyCreditDebitBalanceDetailForArticle(savedPurchaseArticleDetail, true);
                            if(companyCreditDebitBalanceDetail == null)
                            {
                                ModelState.AddModelError("", "Couldn't save.");
                                if (_hostingEnvironment.IsDevelopment())
                                {
                                    ModelState.AddModelError("", $"Couldn't create a companyCreditDebitBalanceDetail to save.");
                                }
                                AddPurchaseArticleLists(model);
                                return View(model);
                            }
                            _context.CompanyCreditDebitBalanceDetails.Add(companyCreditDebitBalanceDetail);

                            //Add or change Company Credit and debit
                            var companyCreditDebitBalance = new CompanyCreditDebitBalance
                            {
                                CompanyID = model.PurchaseArticle.CompanyID,
                                CurrencyID = model.PurchaseArticle.CurrencyID,
                                Credit = model.PurchaseArticleDetail.TotalPerUnit
                            };
                            var addCompanyBalanceResult = CompanyBalance.Add(_context, companyCreditDebitBalance);
                            if (!addCompanyBalanceResult.Value)
                            {
                                ModelState.AddModelError("", "Couldn't save.");
                                if (_hostingEnvironment.IsDevelopment())
                                {
                                    ModelState.AddModelError("", JSonHelper.ToJSon(addCompanyBalanceResult));
                                }
                                AddPurchaseArticleLists(model);
                                return View(model);
                            }
                            //***** Company balance stop *****

                            //successful
                            await _context.SaveChangesAsync();
                            dbContextTransaction.Commit();
                        }
                       
                        AddPurchaseArticleLists(model); 
                        return RedirectToAction(nameof(Create), new { id = model.PurchaseArticle.ID });
                    }
                    else
                    {
                        //update PurchaseArticleDetail
                        var savedPurchaseArticleDetailforUpdate = _context.PurchaseArticleDetails.Include(p => p.PurchaseArticle).Where(d => d.ID == model.PurchaseArticleDetail.ID).FirstOrDefault();
                        if (savedPurchaseArticleDetailforUpdate.ID == 0)
                        {
                            ModelState.AddModelError("", "Couldn't save.");
                            if (_hostingEnvironment.IsDevelopment())
                            {
                                ModelState.AddModelError("", $"Couldn't find saved PurchaseArticleDetail.");
                            }
                            AddPurchaseArticleLists(model);
                            return View(model);
                        }
                        if(savedPurchaseArticleDetailforUpdate.ArticleID == model.PurchaseArticleDetail.ArticleID)
                        {
                            //saved and update has the same article. Rollback Qty and Tot. price per unit diff
                            using (var dbContextTransaction = _context.Database.BeginTransaction())
                            {
                                var articleInOut = new ArticleInOut
                                {
                                    ArticleID = model.PurchaseArticleDetail.ArticleID,
                                    WarehouseID = model.PurchaseArticleDetail.WarehouseID,
                                    CompanyID = ArticleBalance.GetWarehouseCompany(_context, _context.Warehouses.Where(w => w.ID == model.PurchaseArticleDetail.WarehouseID).FirstOrDefault()),
                                    QtyPackages = (model.PurchaseArticleDetail.QtyPackages > savedPurchaseArticleDetailforUpdate.QtyPackages) ? (model.PurchaseArticleDetail.QtyPackages - savedPurchaseArticleDetailforUpdate.QtyPackages) : ((savedPurchaseArticleDetailforUpdate.QtyPackages - model.PurchaseArticleDetail.QtyPackages) * -1),
                                    QtyExtra = (model.PurchaseArticleDetail.QtyExtra > savedPurchaseArticleDetailforUpdate.QtyExtra) ? (model.PurchaseArticleDetail.QtyExtra - savedPurchaseArticleDetailforUpdate.QtyExtra) : ((savedPurchaseArticleDetailforUpdate.QtyExtra - model.PurchaseArticleDetail.QtyExtra) * -1)
                                };
                                if(articleInOut.QtyPackages > 0 || articleInOut.QtyExtra > 0)
                                {
                                    var articleDiffResult = ArticleBalance.Add(_context, articleInOut);
                                    if (articleDiffResult.Value == false)
                                    {
                                        ModelState.AddModelError("", "Couldn't save.");
                                        if (_hostingEnvironment.IsDevelopment())
                                        {
                                            ModelState.AddModelError("", JSonHelper.ToJSon(articleDiffResult));
                                        }
                                        AddPurchaseArticleLists(model);
                                        return View(model);
                                    }
                                }
                                
                                //Save before fetching balances
                                await _context.SaveChangesAsync();
                                var articleBalanceDetailType = _context.ArticleWarehouseBalanceDetailTypes.Where(t => t.Name == ArticleBalanceDetailTypeLookUp.Purchase).FirstOrDefault();
                                if (articleBalanceDetailType == null)
                                {
                                    ModelState.AddModelError("", "Couldn't save.");
                                    if (_hostingEnvironment.IsDevelopment())
                                    {
                                        ModelState.AddModelError("", $"Couldn't find {ArticleBalanceDetailTypeLookUp.Purchase} as lookup type.");
                                    }
                                    AddPurchaseArticleLists(model);
                                    return View(model);
                                }
                                var savedArticleWarehouseBalanceDetail = _context.ArticleWarehouseBalanceDetails
                                    .Where(d => d.ArticleID == savedPurchaseArticleDetailforUpdate.ArticleID && d.WarehouseID == savedPurchaseArticleDetailforUpdate.WarehouseID &&
                                    d.CompanyID == savedPurchaseArticleDetailforUpdate.PurchaseArticle.CompanyID && d.ArticleWarehouseBalanceDetailTypeID == articleBalanceDetailType.ID &&
                                           d.BalanceChangerID == savedPurchaseArticleDetailforUpdate.ID && d.RowCreatedBySystem == true).FirstOrDefault();
                                if(savedArticleWarehouseBalanceDetail == null)
                                {
                                    ModelState.AddModelError("", "Couldn't save.");
                                    if (_hostingEnvironment.IsDevelopment())
                                    {
                                        ModelState.AddModelError("", $"Couldn't find saved ArticleWarehouseBalanceDetail.");
                                    }
                                    AddPurchaseArticleLists(model);
                                    return View(model);
                                }
                                var articleWarehouseBalanceCompanyID = ArticleBalance.GetWarehouseCompany(_context, _context.Warehouses.Where(w => w.ID == savedPurchaseArticleDetailforUpdate.WarehouseID).FirstOrDefault());
                                var articlewarehouseBalance = _context.ArticleWarehouseBalances.Where(b => b.ArticleID == savedPurchaseArticleDetailforUpdate.ArticleID && b.WarehouseID == savedPurchaseArticleDetailforUpdate.WarehouseID && b.CompanyID == articleWarehouseBalanceCompanyID).FirstOrDefault();
                                if (articlewarehouseBalance == null)
                                {
                                    ModelState.AddModelError("", "Couldn't save.");
                                    if (_hostingEnvironment.IsDevelopment())
                                    {
                                        ModelState.AddModelError("", $"Couldn't saved ArticleWarehouseBalance.");
                                    }
                                    AddPurchaseArticleLists(model);
                                    return View(model);
                                }
                                savedArticleWarehouseBalanceDetail.Comment = $"Updated from Packages {savedArticleWarehouseBalanceDetail.QtyPackages} and Extra {savedArticleWarehouseBalanceDetail.QtyExtra} ";
                                savedArticleWarehouseBalanceDetail.Comment += $"Updated from balance Packages {savedArticleWarehouseBalanceDetail.QtyPackagesOnHandBeforeChange} and Extra {savedArticleWarehouseBalanceDetail.QtyExtraOnHandBeforeChange} ";
                                savedArticleWarehouseBalanceDetail.QtyPackages = model.PurchaseArticleDetail.QtyPackages;
                                savedArticleWarehouseBalanceDetail.QtyExtra = model.PurchaseArticleDetail.QtyExtra;
                                savedArticleWarehouseBalanceDetail.QtyPackagesOnHandBeforeChange = articlewarehouseBalance.QtyPackagesOnhand;
                                savedArticleWarehouseBalanceDetail.QtyExtraOnHandBeforeChange = articlewarehouseBalance.QtyExtraOnhand;

                                _context.ArticleWarehouseBalanceDetails.Update(savedArticleWarehouseBalanceDetail);
                                //Change Company balance
                                var totalPerUnit = ArticleHelper.TotalPerUnit(_context, model.PurchaseArticleDetail.ArticleID, model.PurchaseArticleDetail.QtyPackages, model.PurchaseArticleDetail.QtyExtra);
                                var totalPrice = totalPerUnit * model.PurchaseArticleDetail.UnitPrice;
                                model.PurchaseArticleDetail.TotalPerUnit = totalPrice;
                                var companyCreditDebitBalance = new CompanyCreditDebitBalance
                                {
                                    CompanyID = model.PurchaseArticle.CompanyID,
                                    CurrencyID = model.PurchaseArticle.CurrencyID,
                                    Credit = (model.PurchaseArticleDetail.TotalPerUnit > savedPurchaseArticleDetailforUpdate.TotalPerUnit) ? (model.PurchaseArticleDetail.TotalPerUnit - savedPurchaseArticleDetailforUpdate.TotalPerUnit) : ((savedPurchaseArticleDetailforUpdate.TotalPerUnit - model.PurchaseArticleDetail.TotalPerUnit) * -1)
                                };
                                var addCompanyBalanceResult = CompanyBalance.Add(_context, companyCreditDebitBalance);
                                if (!addCompanyBalanceResult.Value)
                                {
                                    ModelState.AddModelError("", "Couldn't save.");
                                    if (_hostingEnvironment.IsDevelopment())
                                    {
                                        ModelState.AddModelError("", JSonHelper.ToJSon(addCompanyBalanceResult));
                                    }
                                    AddPurchaseArticleLists(model);
                                    return View(model);
                                }
                                await _context.SaveChangesAsync();
                                //Change Company balance detail
                                var companyBalanceDetailType = _context.CompanyCreditDebitBalanceDetailTypes.Where(t => t.Name == CompanyBalanceDetailTypeLookUp.Purchase).FirstOrDefault();
                                var paymentType = _context.PaymentTypes.Where(t => t.Name == CompanyPaymentTypeLookUp.Article).FirstOrDefault();
                                if (companyBalanceDetailType == null || paymentType == null)
                                {
                                    ModelState.AddModelError("", "Couldn't save.");
                                    if (_hostingEnvironment.IsDevelopment())
                                    {
                                        ModelState.AddModelError("", $"Couldn't find {CompanyBalanceDetailTypeLookUp.Purchase} or {CompanyPaymentTypeLookUp.Article} as Lookup types");
                                    }
                                    AddPurchaseArticleLists(model);
                                    return View(model);
                                }
                                var savedCompanyCreditDebitBalanceDetail = _context.CompanyCreditDebitBalanceDetails
                                    .Where(d => d.CompanyID == savedPurchaseArticleDetailforUpdate.PurchaseArticle.CompanyID 
                                    && d.CurrencyID == savedPurchaseArticleDetailforUpdate.PurchaseArticle.CurrencyID
                                    && d.CompanyCreditDebitBalanceDetailTypeID == companyBalanceDetailType.ID
                                    && d.BalanceChangerID == savedPurchaseArticleDetailforUpdate.ID
                                    && d.PaymentTypeID == paymentType.ID && d.RowCreatedBySystem == true).FirstOrDefault();
                                if(savedCompanyCreditDebitBalanceDetail == null)
                                {
                                    ModelState.AddModelError("", "Couldn't save.");
                                    if (_hostingEnvironment.IsDevelopment())
                                    {
                                        ModelState.AddModelError("", $"Couldn't find saved CompanyCreditDebitBalanceDetail");
                                    }
                                    AddPurchaseArticleLists(model);
                                    return View(model);
                                }
                                var savedCompanyCreditDebitBalance = CompanyBalance.GetCompanyCreditDebitBalance(_context, savedCompanyCreditDebitBalanceDetail.CompanyID, savedCompanyCreditDebitBalanceDetail.CurrencyID);
                                savedCompanyCreditDebitBalanceDetail.Credit = model.PurchaseArticleDetail.TotalPerUnit;
                                savedCompanyCreditDebitBalanceDetail.CreditBeforeChange = savedCompanyCreditDebitBalance.Credit;
                                savedCompanyCreditDebitBalanceDetail.DebitBeforeChange = savedCompanyCreditDebitBalance.Debit;
                                _context.CompanyCreditDebitBalanceDetails.Update(savedCompanyCreditDebitBalanceDetail);

                                // Save PurchaseArticleDetail
                                savedPurchaseArticleDetailforUpdate.QtyPackages = model.PurchaseArticleDetail.QtyPackages;
                                savedPurchaseArticleDetailforUpdate.QtyExtra = model.PurchaseArticleDetail.QtyExtra;
                                savedPurchaseArticleDetailforUpdate.UnitPrice = model.PurchaseArticleDetail.UnitPrice;
                                savedPurchaseArticleDetailforUpdate.TotalPerUnit = model.PurchaseArticleDetail.TotalPerUnit;

                                _context.PurchaseArticleDetails.Update(savedPurchaseArticleDetailforUpdate);
                                //successful
                                await _context.SaveChangesAsync();
                                dbContextTransaction.Commit();
                            }

                            AddPurchaseArticleLists(model);
                            return RedirectToAction(nameof(Create), new { id = model.PurchaseArticle.ID });
                        }
                        else 
                        {
                            //Different article.RollBack saved Qty and Price
                            var savedPurchaseArticleDetailforDelete = _context.PurchaseArticleDetails.Include(p => p.PurchaseArticle).Where(d => d.ID == model.PurchaseArticleDetail.ID).FirstOrDefault();
                            if (savedPurchaseArticleDetailforDelete.ID == 0)
                            {
                                ModelState.AddModelError("", "Couldn't save.");
                                if (_hostingEnvironment.IsDevelopment())
                                {
                                    ModelState.AddModelError("", $"Couldn't find saved PurchaseArticleDetail.");
                                }
                                AddPurchaseArticleLists(model);
                                return View(model);
                            }
                            if (savedPurchaseArticleDetailforDelete.WarehouseID != model.PurchaseArticleDetail.WarehouseID 
                                || savedPurchaseArticleDetailforDelete.ArticleID != model.PurchaseArticleDetail.ArticleID)
                            {
                                // RollBack balances and delete saved PurchaseArticleDetail

                            }
                           
                                using (var dbContextTransaction = _context.Database.BeginTransaction())
                            {
                                
                                var articleInOut = new ArticleInOut
                                {
                                    ArticleID = savedPurchaseArticleDetailforDelete.ArticleID,
                                    WarehouseID = savedPurchaseArticleDetailforDelete.WarehouseID,
                                    CompanyID = ArticleBalance.GetWarehouseCompany(_context, _context.Warehouses.Where(w => w.ID == savedPurchaseArticleDetailforDelete.WarehouseID).FirstOrDefault()),
                                    QtyPackages = (savedPurchaseArticleDetailforDelete.QtyPackages * -1),
                                    QtyExtra = (savedPurchaseArticleDetailforDelete.QtyExtra * -1)
                                };
                                if (articleInOut.QtyPackages > 0 || articleInOut.QtyExtra > 0)
                                {
                                    var articleResult = ArticleBalance.Add(_context, articleInOut);
                                    if (articleResult.Value == false)
                                    {
                                        ModelState.AddModelError("", "Couldn't save.");
                                        if (_hostingEnvironment.IsDevelopment())
                                        {
                                            ModelState.AddModelError("", JSonHelper.ToJSon(articleResult));
                                        }
                                        AddPurchaseArticleLists(model);
                                        return View(model);
                                    }
                                    await _context.SaveChangesAsync();
                                }

                                var articleBalanceDetailType = _context.ArticleWarehouseBalanceDetailTypes.Where(t => t.Name == ArticleBalanceDetailTypeLookUp.Purchase).FirstOrDefault();
                                if (articleBalanceDetailType == null)
                                {
                                    ModelState.AddModelError("", "Couldn't save.");
                                    if (_hostingEnvironment.IsDevelopment())
                                    {
                                        ModelState.AddModelError("", $"Couldn't find {ArticleBalanceDetailTypeLookUp.Purchase} as lookup type.");
                                    }
                                    AddPurchaseArticleLists(model);
                                    return View(model);
                                }
                                var savedArticleWarehouseBalanceDetail = _context.ArticleWarehouseBalanceDetails
                                    .Where(d => d.ArticleID == savedPurchaseArticleDetailforUpdate.ArticleID && d.WarehouseID == savedPurchaseArticleDetailforUpdate.WarehouseID &&
                                    d.CompanyID == savedPurchaseArticleDetailforUpdate.PurchaseArticle.CompanyID && d.ArticleWarehouseBalanceDetailTypeID == articleBalanceDetailType.ID &&
                                           d.BalanceChangerID == savedPurchaseArticleDetailforUpdate.ID && d.RowCreatedBySystem == true).FirstOrDefault();
                                if (savedArticleWarehouseBalanceDetail == null)
                                {
                                    ModelState.AddModelError("", "Couldn't save.");
                                    if (_hostingEnvironment.IsDevelopment())
                                    {
                                        ModelState.AddModelError("", $"Couldn't find saved ArticleWarehouseBalanceDetail.");
                                    }
                                    AddPurchaseArticleLists(model);
                                    return View(model);
                                }
                                _context.ArticleWarehouseBalanceDetails.Remove(savedArticleWarehouseBalanceDetail);
                                await _context.SaveChangesAsync();
                                //Rollback company balance too
                                var companyCreditDebitBalance = new CompanyCreditDebitBalance
                                {
                                    CompanyID = savedPurchaseArticleDetailforDelete.PurchaseArticle.CompanyID,
                                    CurrencyID = savedPurchaseArticleDetailforDelete.PurchaseArticle.CurrencyID,
                                    Credit = (savedPurchaseArticleDetailforUpdate.TotalPerUnit * -1)
                                };
                                var addCompanyBalanceResult = CompanyBalance.Add(_context, companyCreditDebitBalance);
                                if (!addCompanyBalanceResult.Value)
                                {
                                    ModelState.AddModelError("", "Couldn't save.");
                                    if (_hostingEnvironment.IsDevelopment())
                                    {
                                        ModelState.AddModelError("", JSonHelper.ToJSon(addCompanyBalanceResult));
                                    }
                                    AddPurchaseArticleLists(model);
                                    return View(model);
                                }
                                await _context.SaveChangesAsync();
                               
                                var companyBalanceDetailType = _context.CompanyCreditDebitBalanceDetailTypes.Where(t => t.Name == CompanyBalanceDetailTypeLookUp.Purchase).FirstOrDefault();
                                var paymentType = _context.PaymentTypes.Where(t => t.Name == CompanyPaymentTypeLookUp.Article).FirstOrDefault();
                                if (companyBalanceDetailType == null || paymentType == null)
                                {
                                    ModelState.AddModelError("", "Couldn't save.");
                                    if (_hostingEnvironment.IsDevelopment())
                                    {
                                        ModelState.AddModelError("", $"Couldn't find {CompanyBalanceDetailTypeLookUp.Purchase} or {CompanyPaymentTypeLookUp.Article} as Lookup types");
                                    }
                                    AddPurchaseArticleLists(model);
                                    return View(model);
                                }
                                var savedCompanyCreditDebitBalanceDetail = _context.CompanyCreditDebitBalanceDetails
                                    .Where(d => d.CompanyID == savedPurchaseArticleDetailforDelete.PurchaseArticle.CompanyID
                                    && d.CurrencyID == savedPurchaseArticleDetailforDelete.PurchaseArticle.CurrencyID
                                    && d.CompanyCreditDebitBalanceDetailTypeID == companyBalanceDetailType.ID
                                    && d.BalanceChangerID == savedPurchaseArticleDetailforDelete.ID
                                    && d.PaymentTypeID == paymentType.ID && d.RowCreatedBySystem == true).FirstOrDefault();
                                if (savedCompanyCreditDebitBalanceDetail == null)
                                {
                                    ModelState.AddModelError("", "Couldn't save.");
                                    if (_hostingEnvironment.IsDevelopment())
                                    {
                                        ModelState.AddModelError("", $"Couldn't find saved CompanyCreditDebitBalanceDetail");
                                    }
                                    AddPurchaseArticleLists(model);
                                    return View(model);
                                }
                                _context.CompanyCreditDebitBalanceDetails.Remove(savedCompanyCreditDebitBalanceDetail);
                                await _context.SaveChangesAsync();

                                //Time to save PurchaseArticleDetail with new article
                                var articleInOutForAdd = new ArticleInOut
                                {
                                    ArticleID = model.PurchaseArticleDetail.ArticleID,
                                    WarehouseID = model.PurchaseArticleDetail.WarehouseID,
                                    CompanyID = ArticleBalance.GetWarehouseCompany(_context, _context.Warehouses.Where(w => w.ID == model.PurchaseArticleDetail.WarehouseID).FirstOrDefault()),
                                    QtyPackages = model.PurchaseArticleDetail.QtyPackages,
                                    QtyExtra = model.PurchaseArticleDetail.QtyExtra
                                };
                                if (articleInOutForAdd.QtyPackages > 0 || articleInOutForAdd.QtyExtra > 0)
                                {
                                    var articleResult = ArticleBalance.Add(_context, articleInOutForAdd);
                                    if (articleResult.Value == false)
                                    {
                                        ModelState.AddModelError("", "Couldn't save.");
                                        if (_hostingEnvironment.IsDevelopment())
                                        {
                                            ModelState.AddModelError("", JSonHelper.ToJSon(articleResult));
                                        }
                                        AddPurchaseArticleLists(model);
                                        return View(model);
                                    }
                                    await _context.SaveChangesAsync();
                                }

                                var articleBalanceDetailTypeForAdd = _context.ArticleWarehouseBalanceDetailTypes.Where(t => t.Name == ArticleBalanceDetailTypeLookUp.Purchase).FirstOrDefault();
                                if (articleBalanceDetailTypeForAdd == null)
                                {
                                    ModelState.AddModelError("", "Couldn't save.");
                                    if (_hostingEnvironment.IsDevelopment())
                                    {
                                        ModelState.AddModelError("", $"Couldn't find {ArticleBalanceDetailTypeLookUp.Purchase} as lookup type.");
                                    }
                                    AddPurchaseArticleLists(model);
                                    return View(model);
                                }
                                
                                //Rollback company balance too
                               

                                //successful
                                await _context.SaveChangesAsync();
                                dbContextTransaction.Commit();
                            }
                            AddPurchaseArticleLists(model);
                            return RedirectToAction(nameof(Create), new { id = model.PurchaseArticle.ID });
                        }
                        //To here
                    }
                }
               
                //Save PurchaseArticleCostDetail
                if (!string.IsNullOrWhiteSpace(savePurchaseArticleCostDetail))
                {
                    //Check the model
                    if (model.PurchaseArticleCostDetail == null)
                    {
                        ModelState.AddModelError("", "Couldn't save.");
                        if (_hostingEnvironment.IsDevelopment())
                        {
                            ModelState.AddModelError("", "Model's PurchaseArticleCostDetail is missing.");
                        }
                        AddPurchaseArticleLists(model);
                        return View(model);
                    }

                    model.PurchaseArticleCostDetail.PurchaseArticleID = model.PurchaseArticle.ID;
                    var customModelValidator = PurchaseArticleValidation.PurchaseArticleCostDetailIsValid(_context, model.PurchaseArticleCostDetail);
                    if (customModelValidator.Value == false)
                    {
                        ModelState.AddModelError(customModelValidator.Property, customModelValidator.Message);
                        AddPurchaseArticleLists(model);
                        return View(model);
                    }
                    if(model.PurchaseArticleCostDetail.ID == 0)
                    {
                        // new
                        using (var dbContextTransaction = _context.Database.BeginTransaction())
                        {
                            var guid = Guid.NewGuid();
                            model.PurchaseArticleCostDetail.Guid = guid;
                            _context.PurchaseArticleCostDetails.Add(model.PurchaseArticleCostDetail);
                            await _context.SaveChangesAsync();
                            var savedPurchaseArticleCostDetail = _context.PurchaseArticleCostDetails.Include(p => p.PurchaseArticle)
                                .Where(d => d.Guid == guid && d.PurchaseArticleID == model.PurchaseArticle.ID
                                && d.CompanyID == model.PurchaseArticleCostDetail.CompanyID
                                && d.CurrencyID == model.PurchaseArticleCostDetail.CurrencyID
                                && d.PaymentTypeID == model.PurchaseArticleCostDetail.PaymentTypeID).FirstOrDefault();
                           if(savedPurchaseArticleCostDetail == null)
                           {
                                ModelState.AddModelError("", "Couldn't save.");
                                if (_hostingEnvironment.IsDevelopment())
                                {
                                    ModelState.AddModelError("", $"Couldn't create a PurchaseArticleCostDetail.");
                                }
                                AddPurchaseArticleLists(model);
                                return View(model);
                            }
                            //***** Company balance go *****
                            var PayingCompany = _context.Companies.Where(c => c.ID == savedPurchaseArticleCostDetail.CompanyID).FirstOrDefault();
                            var companyCreditDebitBalance = new CompanyCreditDebitBalance
                            {
                                CompanyID = savedPurchaseArticleCostDetail.CompanyID,
                                CurrencyID = savedPurchaseArticleCostDetail.CurrencyID,
                               
                            };
                            if (PayingCompany.IsOwner)
                            {
                                companyCreditDebitBalance.Debit = savedPurchaseArticleCostDetail.Amount;
                            }
                            else
                            {
                                companyCreditDebitBalance.Credit = savedPurchaseArticleCostDetail.Amount;
                            }
                            
                            var addCompanyBalanceResult = CompanyBalance.Add(_context, companyCreditDebitBalance);
                            if (!addCompanyBalanceResult.Value)
                            {
                                ModelState.AddModelError("", "Couldn't save.");
                                if (_hostingEnvironment.IsDevelopment())
                                {
                                    ModelState.AddModelError("", JSonHelper.ToJSon(addCompanyBalanceResult));
                                }
                                AddPurchaseArticleLists(model);
                                return View(model);
                            }
                            await _context.SaveChangesAsync();
                            //Add CompanyCreditDebitBalanceDetail
                            var companyCreditDebitBalanceDetail = GetCompanyCreditDebitBalanceDetailForCost(savedPurchaseArticleCostDetail, (PayingCompany.IsOwner)? false:true );
                            if (companyCreditDebitBalanceDetail == null)
                            {
                                ModelState.AddModelError("", "Couldn't save.");
                                if (_hostingEnvironment.IsDevelopment())
                                {
                                    ModelState.AddModelError("", $"Couldn't get a companyCreditDebitBalanceDetail to save.");
                                }
                                AddPurchaseArticleLists(model);
                                return View(model);
                            }
                            _context.CompanyCreditDebitBalanceDetails.Add(companyCreditDebitBalanceDetail);
                            //***** Company balance stop *****

                            //successful
                            await _context.SaveChangesAsync();
                            dbContextTransaction.Commit();
                        }
                       
                        AddPurchaseArticleLists(model);
                        return RedirectToAction(nameof(Create), new { id = model.PurchaseArticle.ID, activeTab = PurchaseArticleTabs.Cost });
                    }
                    else
                    {
                      //Update 
                    }
                }
               
                //Save PurchaseArticleShortageDealingDetail
                if (!string.IsNullOrWhiteSpace(savePurchaseArticleShortageDealingDetail))
                {
                    //Check the model
                    if (model.PurchaseArticleShortageDealingDetail == null)
                    {
                        ModelState.AddModelError("", "Couldn't save.");
                        if (_hostingEnvironment.IsDevelopment())
                        {
                            ModelState.AddModelError("", "Model's PurchaseArticleShortageDealingDetail is missing.");
                        }
                        AddPurchaseArticleLists(model);
                        return View(model);
                    }
                    model.PurchaseArticleShortageDealingDetail.PurchaseArticleID = model.PurchaseArticle.ID;
                    var customModelValidator = PurchaseArticleValidation.PurchaseArticleShortageDealingDetailIsValid(_context, model.PurchaseArticleShortageDealingDetail);
                    if (customModelValidator.Value == false)
                    {
                        ModelState.AddModelError(customModelValidator.Property, customModelValidator.Message);
                        AddPurchaseArticleLists(model);
                        return View(model);
                    }
                    if (model.PurchaseArticleShortageDealingDetail.ID == 0)
                    {
                        // new
                        _context.PurchaseArticleShortageDealingDetails.Add(model.PurchaseArticleShortageDealingDetail);
                        await _context.SaveChangesAsync();
                        //AddPurchaseArticleLists(model);
                        return RedirectToAction(nameof(Create), new { id = model.PurchaseArticle.ID, activeTab = PurchaseArticleTabs.ShortageDealing });
                    }
                    else
                    {
                        //Update 
                    }

                }
                
                //Save PurchaseArticleContainerDetail
                if (!string.IsNullOrWhiteSpace(savePurchaseArticleContainerDetail))
                {
                    //Check the model
                    if (model.PurchaseArticleContainerDetail == null)
                    {
                        ModelState.AddModelError("", "Couldn't save.");
                        if (_hostingEnvironment.IsDevelopment())
                        {
                            ModelState.AddModelError("", "Model's PurchaseArticleContainerDetail is missing.");
                        }
                        AddPurchaseArticleLists(model);
                        return View(model);
                    }

                    model.PurchaseArticleContainerDetail.PurchaseArticleID = model.PurchaseArticle.ID;
                    var customModelValidator = PurchaseArticleValidation.PurchaseArticleContainerDetailIsValid(_context, model.PurchaseArticleContainerDetail);
                    if (customModelValidator.Value == false)
                    {
                        ModelState.AddModelError(customModelValidator.Property, customModelValidator.Message);
                        AddPurchaseArticleLists(model);
                        return View(model);
                    }
                    if (model.PurchaseArticleContainerDetail.ID == 0)
                    {
                        // new
                        _context.PurchaseArticleContainerDetails.Add(model.PurchaseArticleContainerDetail);
                        await _context.SaveChangesAsync();
                        //AddPurchaseArticleLists(model);
                        return RedirectToAction(nameof(Create), new { id = model.PurchaseArticle.ID, activeTab = PurchaseArticleTabs.Container});
                    }
                    else
                    {
                        //Update 
                    }
                }
               
                return RedirectToAction(nameof(Index));
            }
            catch(Exception exception)
            {
                ModelState.AddModelError("", "Couldn't save." + exception.Message.ToString());
                AddPurchaseArticleLists(model);
                return View(model);
            }
        }
                    
        public ActionResult DeletePurchaseArticle(int id)
        {
            return View();
        }
        public async Task<IActionResult> EditPurchaseArticleDetail(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var PurchaseArticleDetail = _context.PurchaseArticleDetails.AsNoTracking().Where(d => d.ID == id).FirstOrDefault();
            if (PurchaseArticleDetail == null)
            {
                return NotFound();
            }
            return RedirectToAction(nameof(Create), new { id = PurchaseArticleDetail.PurchaseArticleID, activeTab = PurchaseArticleTabs.Article, articleDetailId = PurchaseArticleDetail.ID });
        }
        public async Task<IActionResult> DeletePurchaseArticleDetail(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var PurchaseArticleDetail = _context.PurchaseArticleDetails.AsNoTracking().Where(d => d.ID == id).FirstOrDefault();
            if (PurchaseArticleDetail == null)
            {
                return NotFound();
            }
            return RedirectToAction(nameof(Create), new { id = PurchaseArticleDetail.PurchaseArticleID, activeTab = PurchaseArticleTabs.Article, articleDetailId = PurchaseArticleDetail.ID });
        }
        public async Task<IActionResult> EditPurchaseArticleCostDetail(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var PurchaseArticleCostDetail = _context.PurchaseArticleCostDetails.AsNoTracking().Where(d => d.ID == id).FirstOrDefault();
            if (PurchaseArticleCostDetail == null)
            {
                return NotFound();
            }
            return RedirectToAction(nameof(Create), new { id = PurchaseArticleCostDetail.PurchaseArticleID, activeTab = PurchaseArticleTabs.Cost, costDetailId = PurchaseArticleCostDetail.ID });
        }
        public async Task<IActionResult> DeletePurchaseArticleCostDetail(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var PurchaseArticleCostDetail = _context.PurchaseArticleCostDetails.AsNoTracking().Where(d => d.ID == id).FirstOrDefault();
            if (PurchaseArticleCostDetail == null)
            {
                return NotFound();
            }
            return RedirectToAction(nameof(Create), new { id = PurchaseArticleCostDetail.PurchaseArticleID, activeTab = PurchaseArticleTabs.Cost, costDetailId = PurchaseArticleCostDetail.ID });
        }
        public async Task<IActionResult> EditPurchaseArticleShortageDealingDetail(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var PurchaseArticleShortageDealingDetail = _context.PurchaseArticleShortageDealingDetails.AsNoTracking().Where(d => d.ID == id).FirstOrDefault();
            if (PurchaseArticleShortageDealingDetail == null)
            {
                return NotFound();
            }
            return RedirectToAction(nameof(Create), new { id = PurchaseArticleShortageDealingDetail.PurchaseArticleID, activeTab = PurchaseArticleTabs.ShortageDealing, shortageDealingDetailId = PurchaseArticleShortageDealingDetail.ID });
        }
        public async Task<IActionResult> DeletePurchaseArticleShortageDealingDetail(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var PurchaseArticleShortageDealingDetail = _context.PurchaseArticleShortageDealingDetails.AsNoTracking().Where(d => d.ID == id).FirstOrDefault();
            if (PurchaseArticleShortageDealingDetail == null)
            {
                return NotFound();
            }
            return RedirectToAction(nameof(Create), new { id = PurchaseArticleShortageDealingDetail.PurchaseArticleID, activeTab = PurchaseArticleTabs.ShortageDealing, shortageDealingDetailId = PurchaseArticleShortageDealingDetail.ID });
        }
        public async Task<IActionResult> EditPurchaseArticleContainerDetail(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var PurchaseArticleContainerDetail = _context.PurchaseArticleContainerDetails.AsNoTracking().Where(d => d.ID == id).FirstOrDefault();
            if (PurchaseArticleContainerDetail == null)
            {
                return NotFound();
            }
            return RedirectToAction(nameof(Create), new { id = PurchaseArticleContainerDetail.PurchaseArticleID, activeTab = PurchaseArticleTabs.Container, containerDetailId = PurchaseArticleContainerDetail.ID });
        }
        public async Task<IActionResult> DeletePurchaseArticleContainerDetail(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var PurchaseArticleContainerDetail = _context.PurchaseArticleContainerDetails.AsNoTracking().Where(d => d.ID == id).FirstOrDefault();
            if (PurchaseArticleContainerDetail == null)
            {
                return NotFound();
            }
            return RedirectToAction(nameof(Create), new { id = PurchaseArticleContainerDetail.PurchaseArticleID, activeTab = PurchaseArticleTabs.Container, containerDetailId = PurchaseArticleContainerDetail.ID });
        }
        #region
        private void AddPurchaseArticleLists(SavePurchaseArticleViewModel model)
        {
            
            ViewData["WarehouseID"] = new SelectList(_context.Warehouses.Where(c => c.Archive == false).OrderBy(a => a.Name), "ID", "Name", model.PurchaseArticleDetail?.WarehouseID);
            ViewData["ArticleID"] = new SelectList(_context.Articles.Where(c => c.Archive == false).OrderBy(a => a.Name), "ID", "Name", model.PurchaseArticleDetail?.ArticleID);
            ViewData["CompanyID"] = new SelectList(_context.Companies.Where(c => c.IsOwner == false && c.Archive == false).OrderBy(c => c.Name), "ID", "Name", model.PurchaseArticle?.CompanyID);
            ViewData["CurrencyID"] = new SelectList(_context.Currencies.Where(c => c.Archive == false).OrderBy(c => c.Code), "ID", "Code", model.PurchaseArticle?.CurrencyID);
            ViewData["PaymentTypeID"] = new SelectList(_context.PaymentTypes.Where(c => c.Archive == false &&
             c.CompanyCreditDebitBalanceDetailTypeID == (int)(_context.CompanyCreditDebitBalanceDetailTypes.Where(t => t.Name == CompanyBalanceDetailTypeLookUp.Purchase).FirstOrDefault().ID)).OrderBy(c => c.Name), "ID", "Name", model.PurchaseArticleCostDetail?.PaymentTypeID);
            ViewData["CostCompanyID"] = new SelectList(_context.Companies.Where(c => c.Archive == false).OrderBy(c => c.Name), "ID", "Name", model.PurchaseArticleCostDetail?.CompanyID);
            ViewData["CostCurrencyID"] = new SelectList(_context.Currencies.Where(c => c.Archive == false).OrderBy(c => c.Code), "ID", "Code", model.PurchaseArticleCostDetail?.CurrencyID);
            ViewData["ShortageDealingCompanyID"] = new SelectList(_context.Companies.Where(c => c.Archive == false).OrderBy(c => c.Name), "ID", "Name", model.PurchaseArticleShortageDealingDetail?.CompanyID);
            ViewData["ShortageDealingCurrencyID"] = new SelectList(_context.Currencies.Where(c => c.Archive == false).OrderBy(c => c.Code), "ID", "Code", model.PurchaseArticleShortageDealingDetail?.CurrencyID);

        }

        private ArticleWarehouseBalanceDetail GetArticleWarehouseBalanceDetail(PurchaseArticleDetail model)
        {
            ArticleWarehouseBalanceDetail articleWarehouseBalanceDetail = null;
            var customModelValidator = PurchaseArticleValidation.PurchaseArticleDetailIsValid(_context, model);
            if(!customModelValidator.Value)
            {
                return articleWarehouseBalanceDetail;
            }
            if (model.ID == 0)
            {
                return articleWarehouseBalanceDetail;
            }
            if(model.PurchaseArticle == null)
            {
                model.PurchaseArticle = _context.PurchaseArticles.Where(p => p.ID == model.PurchaseArticleID).FirstOrDefault();
            }
            var articleBalanceDetailType = _context.ArticleWarehouseBalanceDetailTypes.Where(t => t.Name == ArticleBalanceDetailTypeLookUp.Purchase).FirstOrDefault();
            if (articleBalanceDetailType == null)
            {
                return articleWarehouseBalanceDetail;
            }
            var articleWarehouseBalanceCompanyID = ArticleBalance.GetWarehouseCompany(_context, _context.Warehouses.Where(w => w.ID == model.WarehouseID).FirstOrDefault());
            var articleOnhand = ArticleBalance.GetArticleOnhands(_context, model.ArticleID, model.WarehouseID, articleWarehouseBalanceCompanyID);

            articleWarehouseBalanceDetail = new ArticleWarehouseBalanceDetail();
            articleWarehouseBalanceDetail.ArticleID = model.ArticleID;
            articleWarehouseBalanceDetail.WarehouseID = model.WarehouseID;
            articleWarehouseBalanceDetail.CompanyID = model.PurchaseArticle.CompanyID;
            articleWarehouseBalanceDetail.ArticleWarehouseBalanceDetailTypeID = articleBalanceDetailType.ID;
            articleWarehouseBalanceDetail.BalanceChangerID = model.ID;
            articleWarehouseBalanceDetail.Date = model.PurchaseArticle.Date;
            articleWarehouseBalanceDetail.QtyPackages = model.QtyPackages;
            articleWarehouseBalanceDetail.QtyExtra = model.QtyExtra;
            articleWarehouseBalanceDetail.QtyPackagesOnHandBeforeChange = articleOnhand.QtyPackages;
            articleWarehouseBalanceDetail.QtyExtraOnHandBeforeChange = articleOnhand.QtyExtra;
            articleWarehouseBalanceDetail.RowCreatedBySystem = true;

            return articleWarehouseBalanceDetail;
        }
        private CompanyCreditDebitBalanceDetail GetCompanyCreditDebitBalanceDetailForArticle(PurchaseArticleDetail model, bool isCredit)
        {
            CompanyCreditDebitBalanceDetail companyCreditDebitBalanceDetail = null;
            var customModelValidator = PurchaseArticleValidation.PurchaseArticleDetailIsValid(_context, model);
            if (!customModelValidator.Value)
            {
                return companyCreditDebitBalanceDetail;
            }
            if (model.ID == 0)
            {
                return companyCreditDebitBalanceDetail;
            }
            if (model.PurchaseArticle == null)
            {
                model.PurchaseArticle = _context.PurchaseArticles.Where(p => p.ID == model.PurchaseArticleID).FirstOrDefault();
            }
            if(model.TotalPerUnit == 0)
            {
                return companyCreditDebitBalanceDetail;
            }
                    
            var paymentType = _context.PaymentTypes.Where(p => p.Name == CompanyPaymentTypeLookUp.Article).FirstOrDefault();
            var companyBalanceDetailType = _context.CompanyCreditDebitBalanceDetailTypes.Where(t => t.Name == CompanyBalanceDetailTypeLookUp.Purchase).FirstOrDefault();
            if (companyBalanceDetailType == null || paymentType == null)
            {
                return companyCreditDebitBalanceDetail;
            }
            companyCreditDebitBalanceDetail = new CompanyCreditDebitBalanceDetail();
            companyCreditDebitBalanceDetail.Date = model.PurchaseArticle.Date;
            companyCreditDebitBalanceDetail.CompanyID = model.PurchaseArticle.CompanyID;
            companyCreditDebitBalanceDetail.CurrencyID = model.PurchaseArticle.CurrencyID;
            companyCreditDebitBalanceDetail.CompanyCreditDebitBalanceDetailTypeID = companyBalanceDetailType.ID;
            companyCreditDebitBalanceDetail.PaymentTypeID = paymentType.ID;
            companyCreditDebitBalanceDetail.BalanceChangerID = model.ID;
            if(isCredit)
            {
                companyCreditDebitBalanceDetail.Credit = model.TotalPerUnit;
            }
            else
            {
                companyCreditDebitBalanceDetail.Debit = model.TotalPerUnit;
            }
            var creditDebit = CompanyBalance.GetCompanyCreditDebit(_context, model.PurchaseArticle.CompanyID, model.PurchaseArticle.CurrencyID);
            companyCreditDebitBalanceDetail.CreditBeforeChange = creditDebit.Credit;
            companyCreditDebitBalanceDetail.DebitBeforeChange = creditDebit.Debit;
            companyCreditDebitBalanceDetail.RowCreatedBySystem = true;
            
            return companyCreditDebitBalanceDetail;
        }
        private CompanyCreditDebitBalanceDetail GetCompanyCreditDebitBalanceDetailForCost(PurchaseArticleCostDetail model, bool isCredit)
        {
            CompanyCreditDebitBalanceDetail companyCreditDebitBalanceDetail = null;
            var customModelValidator = PurchaseArticleValidation.PurchaseArticleCostDetailIsValid(_context, model);
            if (!customModelValidator.Value)
            {
                return companyCreditDebitBalanceDetail;
            }
            if (model.ID == 0)
            {
                return companyCreditDebitBalanceDetail;
            }
            if (model.PurchaseArticle == null)
            {
                model.PurchaseArticle = _context.PurchaseArticles.Where(p => p.ID == model.PurchaseArticleID).FirstOrDefault();
            }
            if (model.Amount == 0)
            {
                return companyCreditDebitBalanceDetail;
            }
           
            
            var paymentType = _context.PaymentTypes.Where(t => t.ID == model.PaymentTypeID).FirstOrDefault();
            var companyBalanceDetailType = _context.CompanyCreditDebitBalanceDetailTypes.Where(t => t.Name == CompanyBalanceDetailTypeLookUp.Purchase).FirstOrDefault();
            if (companyBalanceDetailType == null || paymentType == null)
            {
                return companyCreditDebitBalanceDetail;
            }
            companyCreditDebitBalanceDetail = new CompanyCreditDebitBalanceDetail();
            companyCreditDebitBalanceDetail.Date = model.PurchaseArticle.Date;
            companyCreditDebitBalanceDetail.CompanyID = model.CompanyID;
            companyCreditDebitBalanceDetail.CurrencyID = model.CurrencyID;
            companyCreditDebitBalanceDetail.CompanyCreditDebitBalanceDetailTypeID = companyBalanceDetailType.ID;
            companyCreditDebitBalanceDetail.PaymentTypeID = paymentType.ID;
            companyCreditDebitBalanceDetail.BalanceChangerID = model.ID;
            if(isCredit)
            {
                companyCreditDebitBalanceDetail.Credit = model.Amount;
            }
            else
            {
                companyCreditDebitBalanceDetail.Debit = model.Amount;
            }
            var creditDebit = CompanyBalance.GetCompanyCreditDebit(_context, model.CompanyID, model.CurrencyID);
            companyCreditDebitBalanceDetail.CreditBeforeChange = creditDebit.Credit;
            companyCreditDebitBalanceDetail.DebitBeforeChange = creditDebit.Debit;
            companyCreditDebitBalanceDetail.RowCreatedBySystem = true;

            return companyCreditDebitBalanceDetail;
        }
        #endregion
    }
}