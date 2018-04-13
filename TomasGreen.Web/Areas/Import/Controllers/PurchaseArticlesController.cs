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

        public ActionResult Index()
        {
            var purchasedArticles = _context.PurchasedArticles.Include(c => c.Company).Include(d => d.PurchasedArticleDetails).OrderBy(p => p.Arrived == false).ThenBy(p => p.Date).ThenBy(c => c.Company.Name);
            foreach(var item in purchasedArticles)
            {
                item.TotalPrice = item.GetTotalPrice();
            }
            return View(purchasedArticles);
        }

        public ActionResult Create(int? id, int? activeTab, int? articleDetailId, int? costDetailId, int? shortageDealingDetailId, int? containerDetailId)
        {
            var model = new SavePurchasedArticleViewModel();
            model.ActiveTab = (activeTab > 0)? (int)activeTab: PurchasedArticleTabs.Article;
            model.PurchasedArticleDetail = new PurchasedArticleDetail();
            model.PurchasedArticleCostDetail = new PurchasedArticleCostDetail();
            model.PurchasedArticleContainerDetail = new PurchasedArticleContainerDetail();
            model.PurchasedArticleShortageDealingDetail = new PurchasedArticleShortageDealingDetail();
           // model.PurchasedArticleDetail.OntheWayWarehouse = _context.Warehouses.Where(w => w.IsOnTheWay == true).FirstOrDefault();
            //model.PurchasedArticleDetail.WarehouseID = model.PurchasedArticleDetail.OntheWayWarehouse.ID;
            if (id > 0)
            {
                model.PurchasedArticle = _context.PurchasedArticles.Include(d => d.PurchasedArticleDetails).Include(c => c.PurchasedArticleCostDetails)
                    .Include(s => s.PurchasedArticleShortageDealingDetails).Include(c => c.PurchasedArticleContainerDetails).Where(p => p.ID == id).FirstOrDefault();
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
                    detail.PaymentType = _context.PaymentTypes.Find(detail.PaymentTypeID);
                }
                foreach (var detail in model.PurchasedArticle.PurchasedArticleShortageDealingDetails)
                {
                    detail.Company = _context.Companies.Where(c => c.ID == detail.CompanyID).FirstOrDefault();
                    detail.Currency = _context.Currencies.Where(c => c.ID == detail.CurrencyID).FirstOrDefault();
                }
                if (articleDetailId > 0)
                {
                    model.PurchasedArticleDetail = _context.PurchasedArticleDetails.Find(articleDetailId);
                    model.ActiveTab = PurchasedArticleTabs.Article;
                }
                if (costDetailId > 0)
                {
                    model.PurchasedArticleCostDetail = _context.PurchasedArticleCostDetails.Find(costDetailId);
                    model.ActiveTab = PurchasedArticleTabs.Cost;
                }
                if (shortageDealingDetailId > 0)
                {
                    model.PurchasedArticleShortageDealingDetail = _context.PurchasedArticleShortageDealingDetails.Find(shortageDealingDetailId);
                    model.ActiveTab = PurchasedArticleTabs.ShortageDealing;
                }
                if (containerDetailId > 0)
                {
                    model.PurchasedArticleContainerDetail = _context.PurchasedArticleContainerDetails.Find(containerDetailId);
                    model.ActiveTab = PurchasedArticleTabs.Container;
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SavePurchasedArticleViewModel model, string savePurchasedArticle, string savePurchasedArticleDetail,
            string savePurchasedArticleCostDetail, string savePurchasedArticleContainerDetail, string savePurchasedArticleShortageDealingDetail)
        {
            try
            {
                var skippedErrors = ModelState.Keys;
                foreach (var key in skippedErrors)
                {
                    ModelState.Remove(key);
                }
                //Save PurchasedArticle
                if (!string.IsNullOrWhiteSpace(savePurchasedArticle))
                {
                    if(model.PurchasedArticle == null)
                    {
                        ModelState.AddModelError("", "Couldn't save.");
                        if (_hostingEnvironment.IsDevelopment())
                        {
                            ModelState.AddModelError("", "Model's purchasedArticle is missing.");
                        }
                        AddPurchasedArticleLists(model);
                        return View(model);
                    }
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
                        
                        var savedPurchasedArticle = _context.PurchasedArticles.Where(p => p.ID == model.PurchasedArticle.ID).FirstOrDefault();
                        if(savedPurchasedArticle == null)
                        {
                            ModelState.AddModelError("", "Couldn't save.");
                            if(_hostingEnvironment.IsDevelopment())
                            {
                                ModelState.AddModelError("", "Couldn't find saved purchasedArticle.");
                            }
                            AddPurchasedArticleLists(model);
                            return View(model);
                        }
                        if(!savedPurchasedArticle.Arrived)
                        {
                            if (savedPurchasedArticle.CompanyID != model.PurchasedArticle.CompanyID)
                            {
                                //rollback article and company balances
                                var purchasedArticleDetails = _context.PurchasedArticleDetails.Where(d => d.PurchasedArticleID == savedPurchasedArticle.ID);
                                foreach (var item in purchasedArticleDetails)
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
                //Save PurchasedArticleDetail
                if (!string.IsNullOrWhiteSpace(savePurchasedArticleDetail))
                {
                    //Check the model
                    if (model.PurchasedArticleDetail == null)
                    {
                        ModelState.AddModelError("", "Couldn't save.");
                        if (_hostingEnvironment.IsDevelopment())
                        {
                            ModelState.AddModelError("", "Model's purchasedArticleDetail is missing.");
                        }
                        AddPurchasedArticleLists(model);
                        return View(model);
                    }

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
                        using (var dbContextTransaction = _context.Database.BeginTransaction())
                        {
                            var totalPerUnit = ArticleHelper.TotalPerUnit(_context, model.PurchasedArticleDetail.ArticleID, model.PurchasedArticleDetail.QtyPackages, model.PurchasedArticleDetail.QtyExtra);
                            var totalPrice = totalPerUnit * model.PurchasedArticleDetail.UnitPrice;
                            model.PurchasedArticleDetail.TotalPerUnit = totalPrice;

                            var guid = Guid.NewGuid();
                            model.PurchasedArticleDetail.Guid = guid;
                            _context.PurchasedArticleDetails.Add(model.PurchasedArticleDetail);
                            await _context.SaveChangesAsync();
                            var savedPurchasedArticleDetail = _context.PurchasedArticleDetails.Include(p => p.PurchasedArticle).Where(d => d.PurchasedArticleID == model.PurchasedArticle.ID && d.Guid == guid).FirstOrDefault();
                            if (savedPurchasedArticleDetail == null)
                            {
                                ModelState.AddModelError("", "Couldn't save.");
                                if (_hostingEnvironment.IsDevelopment())
                                {
                                    ModelState.AddModelError("", "Couldn't save PurchasedArticleDetail");
                                }
                                AddPurchasedArticleLists(model);
                                return View(model);
                            }

                            //***** Article balance go *****
                            //Add ArticleBalanceDetails
                            var articleWarehouseBalanceDetail = GetArticleWarehouseBalanceDetail(savedPurchasedArticleDetail);
                            if(articleWarehouseBalanceDetail == null)
                            {
                                ModelState.AddModelError("", "Couldn't save.");
                                if (_hostingEnvironment.IsDevelopment())
                                {
                                    ModelState.AddModelError("", $"Couldn't create an articleWarehouseBalanceDetail.");
                                }
                                AddPurchasedArticleLists(model);
                                return View(model);
                            }
                            _context.ArticleWarehouseBalanceDetails.Add(articleWarehouseBalanceDetail);
                            var articleInOut = new ArticleInOut
                            {
                                ArticleID = model.PurchasedArticleDetail.ArticleID,
                                WarehouseID = model.PurchasedArticleDetail.WarehouseID,
                                CompanyID = ArticleBalance.GetWarehouseCompany(_context, _context.Warehouses.Where(w => w.ID == model.PurchasedArticleDetail.WarehouseID).FirstOrDefault()),
                                QtyPackages = model.PurchasedArticleDetail.QtyPackages,
                                QtyExtra = model.PurchasedArticleDetail.QtyExtra
                            };
                            var result = ArticleBalance.Add(_context, articleInOut);
                            if (result.Value == false)
                            {
                                ModelState.AddModelError("", "Couldn't save.");
                                if (_hostingEnvironment.IsDevelopment())
                                {
                                    ModelState.AddModelError("", JSonHelper.ToJSon(result));
                                }
                                AddPurchasedArticleLists(model);
                                return View(model);
                            }
                            //***** Article balance stop *****

                            //***** Company balance go *****
                            //Add CompanyCreditDebitBalanceDetail
                            var companyCreditDebitBalanceDetail = GetCompanyCreditDebitBalanceDetailForArticle(savedPurchasedArticleDetail, true);
                            if(companyCreditDebitBalanceDetail == null)
                            {
                                ModelState.AddModelError("", "Couldn't save.");
                                if (_hostingEnvironment.IsDevelopment())
                                {
                                    ModelState.AddModelError("", $"Couldn't create a companyCreditDebitBalanceDetail to save.");
                                }
                                AddPurchasedArticleLists(model);
                                return View(model);
                            }
                            _context.CompanyCreditDebitBalanceDetails.Add(companyCreditDebitBalanceDetail);

                            //Add or change Company Credit and debit
                            var companyCreditDebitBalance = new CompanyCreditDebitBalance
                            {
                                CompanyID = model.PurchasedArticle.CompanyID,
                                CurrencyID = model.PurchasedArticle.CurrencyID,
                                Credit = model.PurchasedArticleDetail.TotalPerUnit
                            };
                            var addCompanyBalanceResult = CompanyBalance.Add(_context, companyCreditDebitBalance);
                            if (!addCompanyBalanceResult.Value)
                            {
                                ModelState.AddModelError("", "Couldn't save.");
                                if (_hostingEnvironment.IsDevelopment())
                                {
                                    ModelState.AddModelError("", JSonHelper.ToJSon(addCompanyBalanceResult));
                                }
                                AddPurchasedArticleLists(model);
                                return View(model);
                            }
                            //***** Company balance stop *****

                            //successful
                            await _context.SaveChangesAsync();
                            dbContextTransaction.Commit();
                        }
                       
                        AddPurchasedArticleLists(model); 
                        return RedirectToAction(nameof(Create), new { id = model.PurchasedArticle.ID });
                    }
                    else
                    {
                        //update PurchasedArticleDetail
                        var savedPurchasedArticleDetailforUpdate = _context.PurchasedArticleDetails.Include(p => p.PurchasedArticle).Where(d => d.ID == model.PurchasedArticleDetail.ID).FirstOrDefault();
                        if (savedPurchasedArticleDetailforUpdate.ID == 0)
                        {
                            ModelState.AddModelError("", "Couldn't save.");
                            if (_hostingEnvironment.IsDevelopment())
                            {
                                ModelState.AddModelError("", $"Couldn't find saved PurchasedArticleDetail.");
                            }
                            AddPurchasedArticleLists(model);
                            return View(model);
                        }
                        if(savedPurchasedArticleDetailforUpdate.ArticleID == model.PurchasedArticleDetail.ArticleID)
                        {
                            //saved and update has the same article. Rollback Qty and Tot. price per unit diff
                            using (var dbContextTransaction = _context.Database.BeginTransaction())
                            {
                                var articleInOut = new ArticleInOut
                                {
                                    ArticleID = model.PurchasedArticleDetail.ArticleID,
                                    WarehouseID = model.PurchasedArticleDetail.WarehouseID,
                                    CompanyID = ArticleBalance.GetWarehouseCompany(_context, _context.Warehouses.Where(w => w.ID == model.PurchasedArticleDetail.WarehouseID).FirstOrDefault()),
                                    QtyPackages = (model.PurchasedArticleDetail.QtyPackages > savedPurchasedArticleDetailforUpdate.QtyPackages) ? (model.PurchasedArticleDetail.QtyPackages - savedPurchasedArticleDetailforUpdate.QtyPackages) : ((savedPurchasedArticleDetailforUpdate.QtyPackages - model.PurchasedArticleDetail.QtyPackages) * -1),
                                    QtyExtra = (model.PurchasedArticleDetail.QtyExtra > savedPurchasedArticleDetailforUpdate.QtyExtra) ? (model.PurchasedArticleDetail.QtyExtra - savedPurchasedArticleDetailforUpdate.QtyExtra) : ((savedPurchasedArticleDetailforUpdate.QtyExtra - model.PurchasedArticleDetail.QtyExtra) * -1)
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
                                        AddPurchasedArticleLists(model);
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
                                    AddPurchasedArticleLists(model);
                                    return View(model);
                                }
                                var savedArticleWarehouseBalanceDetail = _context.ArticleWarehouseBalanceDetails
                                    .Where(d => d.ArticleID == savedPurchasedArticleDetailforUpdate.ArticleID && d.WarehouseID == savedPurchasedArticleDetailforUpdate.WarehouseID &&
                                    d.CompanyID == savedPurchasedArticleDetailforUpdate.PurchasedArticle.CompanyID && d.ArticleWarehouseBalanceDetailTypeID == articleBalanceDetailType.ID &&
                                           d.BalanceChangerID == savedPurchasedArticleDetailforUpdate.ID && d.RowCreatedBySystem == true).FirstOrDefault();
                                if(savedArticleWarehouseBalanceDetail == null)
                                {
                                    ModelState.AddModelError("", "Couldn't save.");
                                    if (_hostingEnvironment.IsDevelopment())
                                    {
                                        ModelState.AddModelError("", $"Couldn't find saved ArticleWarehouseBalanceDetail.");
                                    }
                                    AddPurchasedArticleLists(model);
                                    return View(model);
                                }
                                var articleWarehouseBalanceCompanyID = ArticleBalance.GetWarehouseCompany(_context, _context.Warehouses.Where(w => w.ID == savedPurchasedArticleDetailforUpdate.WarehouseID).FirstOrDefault());
                                var articlewarehouseBalance = _context.ArticleWarehouseBalances.Where(b => b.ArticleID == savedPurchasedArticleDetailforUpdate.ArticleID && b.WarehouseID == savedPurchasedArticleDetailforUpdate.WarehouseID && b.CompanyID == articleWarehouseBalanceCompanyID).FirstOrDefault();
                                if (articlewarehouseBalance == null)
                                {
                                    ModelState.AddModelError("", "Couldn't save.");
                                    if (_hostingEnvironment.IsDevelopment())
                                    {
                                        ModelState.AddModelError("", $"Couldn't saved ArticleWarehouseBalance.");
                                    }
                                    AddPurchasedArticleLists(model);
                                    return View(model);
                                }
                                savedArticleWarehouseBalanceDetail.Comment = $"Updated from Packages {savedArticleWarehouseBalanceDetail.QtyPackages} and Extra {savedArticleWarehouseBalanceDetail.QtyExtra} ";
                                savedArticleWarehouseBalanceDetail.Comment += $"Updated from balance Packages {savedArticleWarehouseBalanceDetail.QtyPackagesOnHandBeforeChange} and Extra {savedArticleWarehouseBalanceDetail.QtyExtraOnHandBeforeChange} ";
                                savedArticleWarehouseBalanceDetail.QtyPackages = model.PurchasedArticleDetail.QtyPackages;
                                savedArticleWarehouseBalanceDetail.QtyExtra = model.PurchasedArticleDetail.QtyExtra;
                                savedArticleWarehouseBalanceDetail.QtyPackagesOnHandBeforeChange = articlewarehouseBalance.QtyPackagesOnhand;
                                savedArticleWarehouseBalanceDetail.QtyExtraOnHandBeforeChange = articlewarehouseBalance.QtyExtraOnhand;

                                _context.ArticleWarehouseBalanceDetails.Update(savedArticleWarehouseBalanceDetail);
                                //Change Company balance
                                var totalPerUnit = ArticleHelper.TotalPerUnit(_context, model.PurchasedArticleDetail.ArticleID, model.PurchasedArticleDetail.QtyPackages, model.PurchasedArticleDetail.QtyExtra);
                                var totalPrice = totalPerUnit * model.PurchasedArticleDetail.UnitPrice;
                                model.PurchasedArticleDetail.TotalPerUnit = totalPrice;
                                var companyCreditDebitBalance = new CompanyCreditDebitBalance
                                {
                                    CompanyID = model.PurchasedArticle.CompanyID,
                                    CurrencyID = model.PurchasedArticle.CurrencyID,
                                    Credit = (model.PurchasedArticleDetail.TotalPerUnit > savedPurchasedArticleDetailforUpdate.TotalPerUnit) ? (model.PurchasedArticleDetail.TotalPerUnit - savedPurchasedArticleDetailforUpdate.TotalPerUnit) : ((savedPurchasedArticleDetailforUpdate.TotalPerUnit - model.PurchasedArticleDetail.TotalPerUnit) * -1)
                                };
                                var addCompanyBalanceResult = CompanyBalance.Add(_context, companyCreditDebitBalance);
                                if (!addCompanyBalanceResult.Value)
                                {
                                    ModelState.AddModelError("", "Couldn't save.");
                                    if (_hostingEnvironment.IsDevelopment())
                                    {
                                        ModelState.AddModelError("", JSonHelper.ToJSon(addCompanyBalanceResult));
                                    }
                                    AddPurchasedArticleLists(model);
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
                                    AddPurchasedArticleLists(model);
                                    return View(model);
                                }
                                var savedCompanyCreditDebitBalanceDetail = _context.CompanyCreditDebitBalanceDetails
                                    .Where(d => d.CompanyID == savedPurchasedArticleDetailforUpdate.PurchasedArticle.CompanyID 
                                    && d.CurrencyID == savedPurchasedArticleDetailforUpdate.PurchasedArticle.CurrencyID
                                    && d.CompanyCreditDebitBalanceDetailTypeID == companyBalanceDetailType.ID
                                    && d.BalanceChangerID == savedPurchasedArticleDetailforUpdate.ID
                                    && d.PaymentTypeID == paymentType.ID && d.RowCreatedBySystem == true).FirstOrDefault();
                                if(savedCompanyCreditDebitBalanceDetail == null)
                                {
                                    ModelState.AddModelError("", "Couldn't save.");
                                    if (_hostingEnvironment.IsDevelopment())
                                    {
                                        ModelState.AddModelError("", $"Couldn't find saved CompanyCreditDebitBalanceDetail");
                                    }
                                    AddPurchasedArticleLists(model);
                                    return View(model);
                                }
                                var savedCompanyCreditDebitBalance = CompanyBalance.GetCompanyCreditDebitBalance(_context, savedCompanyCreditDebitBalanceDetail.CompanyID, savedCompanyCreditDebitBalanceDetail.CurrencyID);
                                savedCompanyCreditDebitBalanceDetail.Credit = model.PurchasedArticleDetail.TotalPerUnit;
                                savedCompanyCreditDebitBalanceDetail.CreditBeforeChange = savedCompanyCreditDebitBalance.Credit;
                                savedCompanyCreditDebitBalanceDetail.DebitBeforeChange = savedCompanyCreditDebitBalance.Debit;
                                _context.CompanyCreditDebitBalanceDetails.Update(savedCompanyCreditDebitBalanceDetail);

                                // Save PurchasedArticleDetail
                                savedPurchasedArticleDetailforUpdate.QtyPackages = model.PurchasedArticleDetail.QtyPackages;
                                savedPurchasedArticleDetailforUpdate.QtyExtra = model.PurchasedArticleDetail.QtyExtra;
                                savedPurchasedArticleDetailforUpdate.UnitPrice = model.PurchasedArticleDetail.UnitPrice;
                                savedPurchasedArticleDetailforUpdate.TotalPerUnit = model.PurchasedArticleDetail.TotalPerUnit;

                                _context.PurchasedArticleDetails.Update(savedPurchasedArticleDetailforUpdate);
                                //successful
                                await _context.SaveChangesAsync();
                                dbContextTransaction.Commit();
                            }

                            AddPurchasedArticleLists(model);
                            return RedirectToAction(nameof(Create), new { id = model.PurchasedArticle.ID });
                        }
                        else 
                        {
                            //Different article.RollBack saved Qty and Price
                            var savedPurchasedArticleDetailforDelete = _context.PurchasedArticleDetails.Include(p => p.PurchasedArticle).Where(d => d.ID == model.PurchasedArticleDetail.ID).FirstOrDefault();
                            if (savedPurchasedArticleDetailforDelete.ID == 0)
                            {
                                ModelState.AddModelError("", "Couldn't save.");
                                if (_hostingEnvironment.IsDevelopment())
                                {
                                    ModelState.AddModelError("", $"Couldn't find saved PurchasedArticleDetail.");
                                }
                                AddPurchasedArticleLists(model);
                                return View(model);
                            }
                            if (savedPurchasedArticleDetailforDelete.WarehouseID != model.PurchasedArticleDetail.WarehouseID 
                                || savedPurchasedArticleDetailforDelete.ArticleID != model.PurchasedArticleDetail.ArticleID)
                            {
                                // RollBack balances and delete saved purchasedArticleDetail

                            }
                           
                                using (var dbContextTransaction = _context.Database.BeginTransaction())
                            {
                                
                                var articleInOut = new ArticleInOut
                                {
                                    ArticleID = savedPurchasedArticleDetailforDelete.ArticleID,
                                    WarehouseID = savedPurchasedArticleDetailforDelete.WarehouseID,
                                    CompanyID = ArticleBalance.GetWarehouseCompany(_context, _context.Warehouses.Where(w => w.ID == savedPurchasedArticleDetailforDelete.WarehouseID).FirstOrDefault()),
                                    QtyPackages = (savedPurchasedArticleDetailforDelete.QtyPackages * -1),
                                    QtyExtra = (savedPurchasedArticleDetailforDelete.QtyExtra * -1)
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
                                        AddPurchasedArticleLists(model);
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
                                    AddPurchasedArticleLists(model);
                                    return View(model);
                                }
                                var savedArticleWarehouseBalanceDetail = _context.ArticleWarehouseBalanceDetails
                                    .Where(d => d.ArticleID == savedPurchasedArticleDetailforUpdate.ArticleID && d.WarehouseID == savedPurchasedArticleDetailforUpdate.WarehouseID &&
                                    d.CompanyID == savedPurchasedArticleDetailforUpdate.PurchasedArticle.CompanyID && d.ArticleWarehouseBalanceDetailTypeID == articleBalanceDetailType.ID &&
                                           d.BalanceChangerID == savedPurchasedArticleDetailforUpdate.ID && d.RowCreatedBySystem == true).FirstOrDefault();
                                if (savedArticleWarehouseBalanceDetail == null)
                                {
                                    ModelState.AddModelError("", "Couldn't save.");
                                    if (_hostingEnvironment.IsDevelopment())
                                    {
                                        ModelState.AddModelError("", $"Couldn't find saved ArticleWarehouseBalanceDetail.");
                                    }
                                    AddPurchasedArticleLists(model);
                                    return View(model);
                                }
                                _context.ArticleWarehouseBalanceDetails.Remove(savedArticleWarehouseBalanceDetail);
                                await _context.SaveChangesAsync();
                                //Rollback company balance too
                                var companyCreditDebitBalance = new CompanyCreditDebitBalance
                                {
                                    CompanyID = savedPurchasedArticleDetailforDelete.PurchasedArticle.CompanyID,
                                    CurrencyID = savedPurchasedArticleDetailforDelete.PurchasedArticle.CurrencyID,
                                    Credit = (savedPurchasedArticleDetailforUpdate.TotalPerUnit * -1)
                                };
                                var addCompanyBalanceResult = CompanyBalance.Add(_context, companyCreditDebitBalance);
                                if (!addCompanyBalanceResult.Value)
                                {
                                    ModelState.AddModelError("", "Couldn't save.");
                                    if (_hostingEnvironment.IsDevelopment())
                                    {
                                        ModelState.AddModelError("", JSonHelper.ToJSon(addCompanyBalanceResult));
                                    }
                                    AddPurchasedArticleLists(model);
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
                                    AddPurchasedArticleLists(model);
                                    return View(model);
                                }
                                var savedCompanyCreditDebitBalanceDetail = _context.CompanyCreditDebitBalanceDetails
                                    .Where(d => d.CompanyID == savedPurchasedArticleDetailforDelete.PurchasedArticle.CompanyID
                                    && d.CurrencyID == savedPurchasedArticleDetailforDelete.PurchasedArticle.CurrencyID
                                    && d.CompanyCreditDebitBalanceDetailTypeID == companyBalanceDetailType.ID
                                    && d.BalanceChangerID == savedPurchasedArticleDetailforDelete.ID
                                    && d.PaymentTypeID == paymentType.ID && d.RowCreatedBySystem == true).FirstOrDefault();
                                if (savedCompanyCreditDebitBalanceDetail == null)
                                {
                                    ModelState.AddModelError("", "Couldn't save.");
                                    if (_hostingEnvironment.IsDevelopment())
                                    {
                                        ModelState.AddModelError("", $"Couldn't find saved CompanyCreditDebitBalanceDetail");
                                    }
                                    AddPurchasedArticleLists(model);
                                    return View(model);
                                }
                                _context.CompanyCreditDebitBalanceDetails.Remove(savedCompanyCreditDebitBalanceDetail);
                                await _context.SaveChangesAsync();

                                //Time to save purchasedArticleDetail with new article
                                var articleInOutForAdd = new ArticleInOut
                                {
                                    ArticleID = model.PurchasedArticleDetail.ArticleID,
                                    WarehouseID = model.PurchasedArticleDetail.WarehouseID,
                                    CompanyID = ArticleBalance.GetWarehouseCompany(_context, _context.Warehouses.Where(w => w.ID == model.PurchasedArticleDetail.WarehouseID).FirstOrDefault()),
                                    QtyPackages = model.PurchasedArticleDetail.QtyPackages,
                                    QtyExtra = model.PurchasedArticleDetail.QtyExtra
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
                                        AddPurchasedArticleLists(model);
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
                                    AddPurchasedArticleLists(model);
                                    return View(model);
                                }
                                
                                //Rollback company balance too
                               

                                //successful
                                await _context.SaveChangesAsync();
                                dbContextTransaction.Commit();
                            }
                            AddPurchasedArticleLists(model);
                            return RedirectToAction(nameof(Create), new { id = model.PurchasedArticle.ID });
                        }
                        //To here
                    }
                }
               
                //Save PurchasedArticleCostDetail
                if (!string.IsNullOrWhiteSpace(savePurchasedArticleCostDetail))
                {
                    //Check the model
                    if (model.PurchasedArticleCostDetail == null)
                    {
                        ModelState.AddModelError("", "Couldn't save.");
                        if (_hostingEnvironment.IsDevelopment())
                        {
                            ModelState.AddModelError("", "Model's purchasedArticleCostDetail is missing.");
                        }
                        AddPurchasedArticleLists(model);
                        return View(model);
                    }

                    model.PurchasedArticleCostDetail.PurchasedArticleID = model.PurchasedArticle.ID;
                    var customModelValidator = PurchasedArticleValidation.PurchasedArticleCostDetailIsValid(_context, model.PurchasedArticleCostDetail);
                    if (customModelValidator.Value == false)
                    {
                        ModelState.AddModelError(customModelValidator.Property, customModelValidator.Message);
                        AddPurchasedArticleLists(model);
                        return View(model);
                    }
                    if(model.PurchasedArticleCostDetail.ID == 0)
                    {
                        // new
                        using (var dbContextTransaction = _context.Database.BeginTransaction())
                        {
                            var guid = Guid.NewGuid();
                            model.PurchasedArticleCostDetail.Guid = guid;
                            _context.PurchasedArticleCostDetails.Add(model.PurchasedArticleCostDetail);
                            await _context.SaveChangesAsync();
                            var savedPurchasedArticleCostDetail = _context.PurchasedArticleCostDetails.Include(p => p.PurchasedArticle)
                                .Where(d => d.Guid == guid && d.PurchasedArticleID == model.PurchasedArticle.ID
                                && d.CompanyID == model.PurchasedArticleCostDetail.CompanyID
                                && d.CurrencyID == model.PurchasedArticleCostDetail.CurrencyID
                                && d.PaymentTypeID == model.PurchasedArticleCostDetail.PaymentTypeID).FirstOrDefault();
                           if(savedPurchasedArticleCostDetail == null)
                           {
                                ModelState.AddModelError("", "Couldn't save.");
                                if (_hostingEnvironment.IsDevelopment())
                                {
                                    ModelState.AddModelError("", $"Couldn't create a PurchasedArticleCostDetail.");
                                }
                                AddPurchasedArticleLists(model);
                                return View(model);
                            }
                            //***** Company balance go *****
                            var PayingCompany = _context.Companies.Where(c => c.ID == savedPurchasedArticleCostDetail.CompanyID).FirstOrDefault();
                            var companyCreditDebitBalance = new CompanyCreditDebitBalance
                            {
                                CompanyID = savedPurchasedArticleCostDetail.CompanyID,
                                CurrencyID = savedPurchasedArticleCostDetail.CurrencyID,
                               
                            };
                            if (PayingCompany.IsOwner)
                            {
                                companyCreditDebitBalance.Debit = savedPurchasedArticleCostDetail.Amount;
                            }
                            else
                            {
                                companyCreditDebitBalance.Credit = savedPurchasedArticleCostDetail.Amount;
                            }
                            
                            var addCompanyBalanceResult = CompanyBalance.Add(_context, companyCreditDebitBalance);
                            if (!addCompanyBalanceResult.Value)
                            {
                                ModelState.AddModelError("", "Couldn't save.");
                                if (_hostingEnvironment.IsDevelopment())
                                {
                                    ModelState.AddModelError("", JSonHelper.ToJSon(addCompanyBalanceResult));
                                }
                                AddPurchasedArticleLists(model);
                                return View(model);
                            }
                            await _context.SaveChangesAsync();
                            //Add CompanyCreditDebitBalanceDetail
                            var companyCreditDebitBalanceDetail = GetCompanyCreditDebitBalanceDetailForCost(savedPurchasedArticleCostDetail, (PayingCompany.IsOwner)? false:true );
                            if (companyCreditDebitBalanceDetail == null)
                            {
                                ModelState.AddModelError("", "Couldn't save.");
                                if (_hostingEnvironment.IsDevelopment())
                                {
                                    ModelState.AddModelError("", $"Couldn't get a companyCreditDebitBalanceDetail to save.");
                                }
                                AddPurchasedArticleLists(model);
                                return View(model);
                            }
                            _context.CompanyCreditDebitBalanceDetails.Add(companyCreditDebitBalanceDetail);
                            //***** Company balance stop *****

                            //successful
                            await _context.SaveChangesAsync();
                            dbContextTransaction.Commit();
                        }
                       
                        AddPurchasedArticleLists(model);
                        return RedirectToAction(nameof(Create), new { id = model.PurchasedArticle.ID, activeTab = PurchasedArticleTabs.Cost });
                    }
                    else
                    {
                      //Update 
                    }
                }
               
                //Save PurchasedArticleShortageDealingDetail
                if (!string.IsNullOrWhiteSpace(savePurchasedArticleShortageDealingDetail))
                {
                    //Check the model
                    if (model.PurchasedArticleShortageDealingDetail == null)
                    {
                        ModelState.AddModelError("", "Couldn't save.");
                        if (_hostingEnvironment.IsDevelopment())
                        {
                            ModelState.AddModelError("", "Model's purchasedArticleShortageDealingDetail is missing.");
                        }
                        AddPurchasedArticleLists(model);
                        return View(model);
                    }
                    model.PurchasedArticleShortageDealingDetail.PurchasedArticleID = model.PurchasedArticle.ID;
                    var customModelValidator = PurchasedArticleValidation.PurchasedArticleShortageDealingDetailIsValid(_context, model.PurchasedArticleShortageDealingDetail);
                    if (customModelValidator.Value == false)
                    {
                        ModelState.AddModelError(customModelValidator.Property, customModelValidator.Message);
                        AddPurchasedArticleLists(model);
                        return View(model);
                    }
                    if (model.PurchasedArticleShortageDealingDetail.ID == 0)
                    {
                        // new
                        _context.PurchasedArticleShortageDealingDetails.Add(model.PurchasedArticleShortageDealingDetail);
                        await _context.SaveChangesAsync();
                        //AddPurchasedArticleLists(model);
                        return RedirectToAction(nameof(Create), new { id = model.PurchasedArticle.ID, activeTab = PurchasedArticleTabs.ShortageDealing });
                    }
                    else
                    {
                        //Update 
                    }

                }
                
                //Save PurchasedArticleContainerDetail
                if (!string.IsNullOrWhiteSpace(savePurchasedArticleContainerDetail))
                {
                    //Check the model
                    if (model.PurchasedArticleContainerDetail == null)
                    {
                        ModelState.AddModelError("", "Couldn't save.");
                        if (_hostingEnvironment.IsDevelopment())
                        {
                            ModelState.AddModelError("", "Model's purchasedArticleContainerDetail is missing.");
                        }
                        AddPurchasedArticleLists(model);
                        return View(model);
                    }

                    model.PurchasedArticleContainerDetail.PurchasedArticleID = model.PurchasedArticle.ID;
                    var customModelValidator = PurchasedArticleValidation.PurchasedArticleContainerDetailIsValid(_context, model.PurchasedArticleContainerDetail);
                    if (customModelValidator.Value == false)
                    {
                        ModelState.AddModelError(customModelValidator.Property, customModelValidator.Message);
                        AddPurchasedArticleLists(model);
                        return View(model);
                    }
                    if (model.PurchasedArticleContainerDetail.ID == 0)
                    {
                        // new
                        _context.PurchasedArticleContainerDetails.Add(model.PurchasedArticleContainerDetail);
                        await _context.SaveChangesAsync();
                        //AddPurchasedArticleLists(model);
                        return RedirectToAction(nameof(Create), new { id = model.PurchasedArticle.ID, activeTab = PurchasedArticleTabs.Container});
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
                AddPurchasedArticleLists(model);
                return View(model);
            }
        }
                    
        public ActionResult DeletePurchasedArticle(int id)
        {
            return View();
        }
        public async Task<IActionResult> EditPurchasedArticleDetail(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var purchasedArticleDetail = _context.PurchasedArticleDetails.AsNoTracking().Where(d => d.ID == id).FirstOrDefault();
            if (purchasedArticleDetail == null)
            {
                return NotFound();
            }
            return RedirectToAction(nameof(Create), new { id = purchasedArticleDetail.PurchasedArticleID, activeTab = PurchasedArticleTabs.Article, articleDetailId = purchasedArticleDetail.ID });
        }
        public async Task<IActionResult> DeletePurchasedArticleDetail(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var purchasedArticleDetail = _context.PurchasedArticleDetails.AsNoTracking().Where(d => d.ID == id).FirstOrDefault();
            if (purchasedArticleDetail == null)
            {
                return NotFound();
            }
            return RedirectToAction(nameof(Create), new { id = purchasedArticleDetail.PurchasedArticleID, activeTab = PurchasedArticleTabs.Article, articleDetailId = purchasedArticleDetail.ID });
        }
        public async Task<IActionResult> EditPurchasedArticleCostDetail(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var purchasedArticleCostDetail = _context.PurchasedArticleCostDetails.AsNoTracking().Where(d => d.ID == id).FirstOrDefault();
            if (purchasedArticleCostDetail == null)
            {
                return NotFound();
            }
            return RedirectToAction(nameof(Create), new { id = purchasedArticleCostDetail.PurchasedArticleID, activeTab = PurchasedArticleTabs.Cost, costDetailId = purchasedArticleCostDetail.ID });
        }
        public async Task<IActionResult> DeletePurchasedArticleCostDetail(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var purchasedArticleCostDetail = _context.PurchasedArticleCostDetails.AsNoTracking().Where(d => d.ID == id).FirstOrDefault();
            if (purchasedArticleCostDetail == null)
            {
                return NotFound();
            }
            return RedirectToAction(nameof(Create), new { id = purchasedArticleCostDetail.PurchasedArticleID, activeTab = PurchasedArticleTabs.Cost, costDetailId = purchasedArticleCostDetail.ID });
        }
        public async Task<IActionResult> EditPurchasedArticleShortageDealingDetail(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var purchasedArticleShortageDealingDetail = _context.PurchasedArticleShortageDealingDetails.AsNoTracking().Where(d => d.ID == id).FirstOrDefault();
            if (purchasedArticleShortageDealingDetail == null)
            {
                return NotFound();
            }
            return RedirectToAction(nameof(Create), new { id = purchasedArticleShortageDealingDetail.PurchasedArticleID, activeTab = PurchasedArticleTabs.ShortageDealing, shortageDealingDetailId = purchasedArticleShortageDealingDetail.ID });
        }
        public async Task<IActionResult> DeletePurchasedArticleShortageDealingDetail(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var purchasedArticleShortageDealingDetail = _context.PurchasedArticleShortageDealingDetails.AsNoTracking().Where(d => d.ID == id).FirstOrDefault();
            if (purchasedArticleShortageDealingDetail == null)
            {
                return NotFound();
            }
            return RedirectToAction(nameof(Create), new { id = purchasedArticleShortageDealingDetail.PurchasedArticleID, activeTab = PurchasedArticleTabs.ShortageDealing, shortageDealingDetailId = purchasedArticleShortageDealingDetail.ID });
        }
        public async Task<IActionResult> EditPurchasedArticleContainerDetail(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var purchasedArticleContainerDetail = _context.PurchasedArticleContainerDetails.AsNoTracking().Where(d => d.ID == id).FirstOrDefault();
            if (purchasedArticleContainerDetail == null)
            {
                return NotFound();
            }
            return RedirectToAction(nameof(Create), new { id = purchasedArticleContainerDetail.PurchasedArticleID, activeTab = PurchasedArticleTabs.Container, containerDetailId = purchasedArticleContainerDetail.ID });
        }
        public async Task<IActionResult> DeletePurchasedArticleContainerDetail(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var purchasedArticleContainerDetail = _context.PurchasedArticleContainerDetails.AsNoTracking().Where(d => d.ID == id).FirstOrDefault();
            if (purchasedArticleContainerDetail == null)
            {
                return NotFound();
            }
            return RedirectToAction(nameof(Create), new { id = purchasedArticleContainerDetail.PurchasedArticleID, activeTab = PurchasedArticleTabs.Container, containerDetailId = purchasedArticleContainerDetail.ID });
        }
        #region
        private void AddPurchasedArticleLists(SavePurchasedArticleViewModel model)
        {
            
            ViewData["WarehouseID"] = new SelectList(_context.Warehouses.Where(c => c.Archive == false).OrderBy(a => a.Name), "ID", "Name", model.PurchasedArticleDetail?.WarehouseID);
            ViewData["ArticleID"] = new SelectList(_context.Articles.Where(c => c.Archive == false).OrderBy(a => a.Name), "ID", "Name", model.PurchasedArticleDetail?.ArticleID);
            ViewData["CompanyID"] = new SelectList(_context.Companies.Where(c => c.IsOwner == false && c.Archive == false).OrderBy(c => c.Name), "ID", "Name", model.PurchasedArticle?.CompanyID);
            ViewData["CurrencyID"] = new SelectList(_context.Currencies.Where(c => c.Archive == false).OrderBy(c => c.Code), "ID", "Code", model.PurchasedArticle?.CurrencyID);
            ViewData["PaymentTypeID"] = new SelectList(_context.PaymentTypes.Where(c => c.Archive == false &&
             c.CompanyCreditDebitBalanceDetailTypeID == (int)(_context.CompanyCreditDebitBalanceDetailTypes.Where(t => t.Name == CompanyBalanceDetailTypeLookUp.Purchase).FirstOrDefault().ID)).OrderBy(c => c.Name), "ID", "Name", model.PurchasedArticleCostDetail?.PaymentTypeID);
            ViewData["CostCompanyID"] = new SelectList(_context.Companies.Where(c => c.Archive == false).OrderBy(c => c.Name), "ID", "Name", model.PurchasedArticleCostDetail?.CompanyID);
            ViewData["CostCurrencyID"] = new SelectList(_context.Currencies.Where(c => c.Archive == false).OrderBy(c => c.Code), "ID", "Code", model.PurchasedArticleCostDetail?.CurrencyID);
            ViewData["ShortageDealingCompanyID"] = new SelectList(_context.Companies.Where(c => c.Archive == false).OrderBy(c => c.Name), "ID", "Name", model.PurchasedArticleShortageDealingDetail?.CompanyID);
            ViewData["ShortageDealingCurrencyID"] = new SelectList(_context.Currencies.Where(c => c.Archive == false).OrderBy(c => c.Code), "ID", "Code", model.PurchasedArticleShortageDealingDetail?.CurrencyID);

        }

        private ArticleWarehouseBalanceDetail GetArticleWarehouseBalanceDetail(PurchasedArticleDetail model)
        {
            ArticleWarehouseBalanceDetail articleWarehouseBalanceDetail = null;
            var customModelValidator = PurchasedArticleValidation.PurchasedArticleDetailIsValid(_context, model);
            if(!customModelValidator.Value)
            {
                return articleWarehouseBalanceDetail;
            }
            if (model.ID == 0)
            {
                return articleWarehouseBalanceDetail;
            }
            if(model.PurchasedArticle == null)
            {
                model.PurchasedArticle = _context.PurchasedArticles.Where(p => p.ID == model.PurchasedArticleID).FirstOrDefault();
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
            articleWarehouseBalanceDetail.CompanyID = model.PurchasedArticle.CompanyID;
            articleWarehouseBalanceDetail.ArticleWarehouseBalanceDetailTypeID = articleBalanceDetailType.ID;
            articleWarehouseBalanceDetail.BalanceChangerID = model.ID;
            articleWarehouseBalanceDetail.Date = model.PurchasedArticle.Date;
            articleWarehouseBalanceDetail.QtyPackages = model.QtyPackages;
            articleWarehouseBalanceDetail.QtyExtra = model.QtyExtra;
            articleWarehouseBalanceDetail.QtyPackagesOnHandBeforeChange = articleOnhand.QtyPackages;
            articleWarehouseBalanceDetail.QtyExtraOnHandBeforeChange = articleOnhand.QtyExtra;
            articleWarehouseBalanceDetail.RowCreatedBySystem = true;

            return articleWarehouseBalanceDetail;
        }
        private CompanyCreditDebitBalanceDetail GetCompanyCreditDebitBalanceDetailForArticle(PurchasedArticleDetail model, bool isCredit)
        {
            CompanyCreditDebitBalanceDetail companyCreditDebitBalanceDetail = null;
            var customModelValidator = PurchasedArticleValidation.PurchasedArticleDetailIsValid(_context, model);
            if (!customModelValidator.Value)
            {
                return companyCreditDebitBalanceDetail;
            }
            if (model.ID == 0)
            {
                return companyCreditDebitBalanceDetail;
            }
            if (model.PurchasedArticle == null)
            {
                model.PurchasedArticle = _context.PurchasedArticles.Where(p => p.ID == model.PurchasedArticleID).FirstOrDefault();
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
            companyCreditDebitBalanceDetail.Date = model.PurchasedArticle.Date;
            companyCreditDebitBalanceDetail.CompanyID = model.PurchasedArticle.CompanyID;
            companyCreditDebitBalanceDetail.CurrencyID = model.PurchasedArticle.CurrencyID;
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
            var creditDebit = CompanyBalance.GetCompanyCreditDebit(_context, model.PurchasedArticle.CompanyID, model.PurchasedArticle.CurrencyID);
            companyCreditDebitBalanceDetail.CreditBeforeChange = creditDebit.Credit;
            companyCreditDebitBalanceDetail.DebitBeforeChange = creditDebit.Debit;
            companyCreditDebitBalanceDetail.RowCreatedBySystem = true;
            
            return companyCreditDebitBalanceDetail;
        }
        private CompanyCreditDebitBalanceDetail GetCompanyCreditDebitBalanceDetailForCost(PurchasedArticleCostDetail model, bool isCredit)
        {
            CompanyCreditDebitBalanceDetail companyCreditDebitBalanceDetail = null;
            var customModelValidator = PurchasedArticleValidation.PurchasedArticleCostDetailIsValid(_context, model);
            if (!customModelValidator.Value)
            {
                return companyCreditDebitBalanceDetail;
            }
            if (model.ID == 0)
            {
                return companyCreditDebitBalanceDetail;
            }
            if (model.PurchasedArticle == null)
            {
                model.PurchasedArticle = _context.PurchasedArticles.Where(p => p.ID == model.PurchasedArticleID).FirstOrDefault();
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
            companyCreditDebitBalanceDetail.Date = model.PurchasedArticle.Date;
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