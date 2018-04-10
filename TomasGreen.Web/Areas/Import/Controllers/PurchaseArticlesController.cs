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
        public ActionResult Create(int? id, int? purchasedArticleId)
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
                
                if(purchasedArticleId > 0)
                {
                    model.PurchasedArticleDetail = _context.PurchasedArticleDetails.Where(d => d.ID == purchasedArticleId).FirstOrDefault();
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
            string DeletePurchasedArticleDetail, string SavePurchasedArticleCostDetail, string DeletePurchasedArticleCostDetail, string SavePurchasedArticleContainerDetail, string DeletePurchasedArticleContainerDetail)
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
                if (!string.IsNullOrWhiteSpace(SavePurchasedArticleDetail))
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
                            model.PurchasedArticleDetail.TotalPerUnit = totalPerUnit;
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
                            var guid = Guid.NewGuid();
                            model.PurchasedArticleDetail.Guid = guid;
                            _context.PurchasedArticleDetails.Add(model.PurchasedArticleDetail);
                            await _context.SaveChangesAsync();
                            var savedPurchasedArticleDetail = _context.PurchasedArticleDetails.Include(p => p.PurchasedArticle).Where(d => d.PurchasedArticleID == model.PurchasedArticle.ID && d.Guid == guid).FirstOrDefault();
                            if(savedPurchasedArticleDetail == null)
                            {
                                ModelState.AddModelError("", "Couldn't save.");
                                if (_hostingEnvironment.IsDevelopment())
                                {
                                    ModelState.AddModelError("", "Couldn't save PurchasedArticleDetail");
                                }
                                AddPurchasedArticleLists(model);
                                return View(model);
                            }
                            //Add ArticleBalanceDetails
                            var articleWarehouseBalanceCompanyID = ArticleBalance.GetWarehouseCompany(_context, _context.Warehouses.Where(w => w.ID == model.PurchasedArticleDetail.WarehouseID).FirstOrDefault());
                            var articlewarehouseBalance = _context.ArticleWarehouseBalances.Where(b => b.ArticleID == savedPurchasedArticleDetail.ArticleID && b.WarehouseID == savedPurchasedArticleDetail.WarehouseID && b.CompanyID == articleWarehouseBalanceCompanyID).FirstOrDefault();
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
                            var articleBalanceDetailType = _context.ArticleWarehouseBalanceDetailTypes.Where(t => t.Name == ArticleBalanceDetailTypeLookUp.Purchase).FirstOrDefault();
                            if(articleBalanceDetailType == null)
                            {
                                ModelState.AddModelError("", "Couldn't save.");
                                if (_hostingEnvironment.IsDevelopment())
                                {
                                    ModelState.AddModelError("", $"Couldn't find {ArticleBalanceDetailTypeLookUp.Purchase} as lookup type.");
                                }
                                AddPurchasedArticleLists(model);
                                return View(model);
                            }
                            var articleWarehouseBalanceDetail = new ArticleWarehouseBalanceDetail
                            {
                                ArticleID = savedPurchasedArticleDetail.ArticleID,
                                WarehouseID = savedPurchasedArticleDetail.WarehouseID,
                                CompanyID = savedPurchasedArticleDetail.PurchasedArticle.CompanyID,
                                ArticleWarehouseBalanceDetailTypeID = articleBalanceDetailType.ID,
                                BalanceChangerID = savedPurchasedArticleDetail.ID,
                                Date = savedPurchasedArticleDetail.PurchasedArticle.Date,
                                QtyPackages = savedPurchasedArticleDetail.QtyPackages,
                                QtyExtra = savedPurchasedArticleDetail.QtyExtra,
                                QtyPackagesOnHandBeforeChange = articlewarehouseBalance.QtyPackagesOnhand,
                                QtyExtraOnHandBeforeChange = articlewarehouseBalance.QtyExtraOnhand,
                                RowCreatedBySystem = true
                            };
                            _context.ArticleWarehouseBalanceDetails.Add(articleWarehouseBalanceDetail);
                            // Changing company balance
                            var companyCreditDeitBalance = _context.CompanyCreditDebitBalances.Where(b => b.CompanyID == savedPurchasedArticleDetail.PurchasedArticle.CompanyID && b.CurrencyID == savedPurchasedArticleDetail.PurchasedArticle.CurrencyID).FirstOrDefault();
                            if(companyCreditDeitBalance == null)
                            {
                                ModelState.AddModelError("", "Couldn't save.");
                                if (_hostingEnvironment.IsDevelopment())
                                {
                                    ModelState.AddModelError("", $"Couldn't find saved CompanyCreditDebitBalance.");
                                }
                                AddPurchasedArticleLists(model);
                                return View(model);
                            }
                            var paymentType = _context.PaymentTypes.Where(p => p.Name == CompanyPaymentTypeLookUp.Article).FirstOrDefault();
                            var companyBalanceDetailType = _context.CompanyCreditDebitBalanceDetailTypes.Where(t => t.Name == CompanyBalanceDetailTypeLookUp.Purchase).FirstOrDefault();
                            if(companyBalanceDetailType == null || paymentType == null)
                            {
                                ModelState.AddModelError("", "Couldn't save.");
                                if (_hostingEnvironment.IsDevelopment())
                                {
                                    ModelState.AddModelError("", $"Couldn't find {CompanyBalanceDetailTypeLookUp.Purchase} or {CompanyPaymentTypeLookUp.Article} as Lookup types");
                                }
                                AddPurchasedArticleLists(model);
                                return View(model);
                            }
                            var companyCreditDebitBalanceDetail = new CompanyCreditDebitBalanceDetail()
                            {
                                Date = savedPurchasedArticleDetail.PurchasedArticle.Date,
                                CompanyID = savedPurchasedArticleDetail.PurchasedArticle.CompanyID,
                                CurrencyID = savedPurchasedArticleDetail.PurchasedArticle.CurrencyID,
                                CompanyCreditDebitBalanceDetailTypeID = companyBalanceDetailType.ID,
                                PaymentTypeID = paymentType.ID,
                                BalanceChangerID = savedPurchasedArticleDetail.ID,
                                Credit = savedPurchasedArticleDetail.TotalPerUnit,
                                CreditBeforeChange = companyCreditDeitBalance.Credit,
                                DebitBeforeChange = companyCreditDeitBalance.Debit,
                                RowCreatedBySystem = true

                            };
                            _context.CompanyCreditDebitBalanceDetails.Add(companyCreditDebitBalanceDetail);

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
                               
                                await _context.SaveChangesAsync();
                                //Rollback company balance too
                               

                                //successful
                                await _context.SaveChangesAsync();
                                dbContextTransaction.Commit();
                            }
                            AddPurchasedArticleLists(model);
                            return RedirectToAction(nameof(Create), new { id = model.PurchasedArticle.ID });
                        }
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
                //Save PurchasedArticleContainerDetail
                if (!string.IsNullOrWhiteSpace(SavePurchasedArticleContainerDetail))
                {

                }
                //Delete PurchasedArticleContainerDetail
                if (!string.IsNullOrWhiteSpace(DeletePurchasedArticleContainerDetail))
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
        // GET: 
        public async Task<IActionResult> EditRow(int? id)
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
            return RedirectToAction(nameof(Create), new { id = purchasedArticleDetail.PurchasedArticleID, purchasedArticleId = purchasedArticleDetail.ID });
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