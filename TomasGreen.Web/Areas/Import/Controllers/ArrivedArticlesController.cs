using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TomasGreen.Model.Models;
using TomasGreen.Web.Data;
using TomasGreen.Web.Areas.Import.ViewModels;
using TomasGreen.Web.Balances;
using TomasGreen.Web.Validations;
using TomasGreen.Web.Extensions;
using TomasGreen.Web.BaseModels;
using Microsoft.Extensions.Localization;

namespace TomasGreen.Web.Areas.Import.Controllers
{
    [Area("Import")]
    public class ArrivedArticlesController : BaseController
    {

        private readonly ApplicationDbContext _context;
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly IStringLocalizer<ArrivedArticlesController> _localizer;
        public ArrivedArticlesController(ApplicationDbContext context, IHostingEnvironment hostingEnvironment, IStringLocalizer<ArrivedArticlesController> localizer)
        {
            _context = context;
            _hostingEnvironment = hostingEnvironment;
            _localizer = localizer;
        }
        // GET: Sales/Orders
        public IActionResult Index()
        {
            List<OnthewayArticlesViewModel> arrivedArticlesViewModelList = new List<OnthewayArticlesViewModel>();
            

            var articlesOntheway = GetOnthewayArticles(_context);
            foreach (var articleontheway in articlesOntheway)
            {
                var onthewayArticleViewModel = new OnthewayArticlesViewModel();
                onthewayArticleViewModel.PurchasedArticleWarehouse = articleontheway;
                onthewayArticleViewModel.PurchasedDate = articleontheway.PurchasedArticle.Date;
                onthewayArticleViewModel.ExpectedToArrive = articleontheway.PurchasedArticle.ExpectedToArrive;
                onthewayArticleViewModel.Warehouse = articleontheway.Warehouse;
                onthewayArticleViewModel.Company = articleontheway.PurchasedArticle.Company ?? new Company();
                onthewayArticleViewModel.Article = articleontheway.PurchasedArticle.Article ?? new Article();
                onthewayArticleViewModel.ContainerNumber = articleontheway.PurchasedArticle.ContainerNumber;
                arrivedArticlesViewModelList.Add(onthewayArticleViewModel);
            }
            return View(arrivedArticlesViewModelList);
        }

        // GET: Stock/Articles/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var SavedPurchasedArticleWarehouses = await _context.PurchasedArticleWarehouses.Include(p => p.PurchasedArticle).ThenInclude(a => a.Article).SingleOrDefaultAsync(m => m.ID == id);
            if (SavedPurchasedArticleWarehouses == null)
            {
                return NotFound();
            }
            if(SavedPurchasedArticleWarehouses.ArrivedDate != null)
            {
                return RedirectToAction(nameof(Index));
            }
            var model = new ArrivedArticleViewModel
            {
                PurchasedArticleWarehouseID = SavedPurchasedArticleWarehouses.ID,
                ContainerNumber = SavedPurchasedArticleWarehouses.PurchasedArticle.ContainerNumber,
                Article = SavedPurchasedArticleWarehouses.PurchasedArticle.Article,
                ArrivedDate = DateTime.Today,
            };
            ViewBag.ArrivedAtWarehouseID = new SelectList(_context.Warehouses.Where(w => w.IsOnTheWay == false).OrderBy(w => w.Name), "ID", "Name", model?.Warehouse?.ID);
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? PurchasedArticleWarehouseID, ArrivedArticleViewModel model)
        {
            if (PurchasedArticleWarehouseID != model.PurchasedArticleWarehouseID)
            {
                return NotFound();
            }

            var SavedPurchasedArticleWarehouse = _context.PurchasedArticleWarehouses.Where(p => p.ID == model.PurchasedArticleWarehouseID).FirstOrDefault();
            if(SavedPurchasedArticleWarehouse == null)
            {
                return NotFound();
            }
            var result = Validate(model);
            if (!result.Value)
            {
                ModelState.AddModelError("", _localizer["Couldn't save."]);
                if (_hostingEnvironment.IsDevelopment())
                {
                    ModelState.AddModelError("", JSonHelper.ToJSon(result));
                }
                return View(model);
            }
            SavedPurchasedArticleWarehouse.ArrivedAtWarehouseID = model.Warehouse.ID;
            SavedPurchasedArticleWarehouse.ArrivedDate = model.ArrivedDate;
            SavedPurchasedArticleWarehouse.QtyPackagesArrived = model.QtyPackagesArrived;
            SavedPurchasedArticleWarehouse.QtyExtraArrived = model.QtyExtraArrived;
            SavedPurchasedArticleWarehouse.Notes = model.Notes;
            SavedPurchasedArticleWarehouse.Warehouse = _context.Warehouses.Where(w => w.ID == SavedPurchasedArticleWarehouse.WarehouseID).FirstOrDefault();
           
           
            //Shift order's Warehouse 
            var shiftWarehouseResult = ShiftOrdersWarehouse(SavedPurchasedArticleWarehouse);
            if(!shiftWarehouseResult.Value)
            {
                ModelState.AddModelError("", _localizer["Couldn't save."]);
                if (_hostingEnvironment.IsDevelopment())
                {
                    ModelState.AddModelError("", JSonHelper.ToJSon(result));
                }
                return View(model);
            }
           // _context.Update(SavedPurchasedArticleWarehouse);
           // await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PurchasedArticleWarehouseExists(int id)
        {
            return _context.PurchasedArticleWarehouses.Any(p => p.ID == id);
        }



        #region
        private PropertyValidation ShiftOrdersWarehouse(PurchasedArticleWarehouse purchasedArticleWarehouse)
        {
            var result = new PropertyValidation { Value = true, Action = "ArrivedArticles|ShiftOrdersWarehouse", Model = "PurchasedArticleWarehouse", Property = "", Message = "" };
            try
            {
                var isOnthewayWarehouse = _context.Warehouses.Any(w => w.IsOnTheWay == true && w.ID == purchasedArticleWarehouse.WarehouseID);
                if(!isOnthewayWarehouse)
                {
                    result.Value = true;
                    return result;
                }
               
               
                var purchasedArticle = _context.PurchasedArticles.Where(p => p.ID == purchasedArticleWarehouse.PurchasedArticleID).FirstOrDefault();
                var orderDetails = _context.OrderDetails.Include(d => d.Order).Where(o => o.Order.LoadedDate == null)
                    .Where(o => o.Order.OrderDate >= purchasedArticle.Date && o.Order.OrderDate <= purchasedArticleWarehouse.ArrivedDate)
                    .Where(d => d.ArticleID == purchasedArticle.ArticleID && d.WarehouseID == purchasedArticleWarehouse.WarehouseID)
                    .OrderBy(d => d.Order.OrderDate).ToList();
                var QtyPackagesArrived = purchasedArticleWarehouse.QtyPackagesArrived;
                decimal QtyExtraArrived = purchasedArticleWarehouse.QtyExtraArrived;
                 var totalQtyPackagesToBeShifted = 0;
                 decimal totalQtyExtraToBeShifted = 0;
                //foreach (var item in orderDetails)
                //{
                //    totalQtyPackages += item.QtyPackages;
                //    totalQtyExtra += item.QtyExtra;
                //}
                //if(totalQtyPackages > purchasedArticleWarehouse.QtyPackagesArrived || totalQtyExtra > purchasedArticleWarehouse.QtyExtraArrived)
                //{
                //    result.Value = false; result.Property = "QtyPackagesArrived"; result.Message = "Arrived article is less than Orderd.";
                //    return result;
                //}
                using (var dbContextTransaction = _context.Database.BeginTransaction())
                {
                    foreach (var orderDetail in orderDetails)
                    {
                        if (QtyPackagesArrived > 0 || QtyExtraArrived > 0)
                        {
                            totalQtyPackagesToBeShifted += orderDetail.QtyPackages;
                            totalQtyExtraToBeShifted += orderDetail.QtyExtra;
                            if ((QtyPackagesArrived - orderDetail.QtyPackages) >= 0 && (QtyExtraArrived - orderDetail.QtyExtra) >= 0)
                            {
                                // no issue
                                QtyPackagesArrived -= orderDetail.QtyPackages;
                                QtyExtraArrived -= orderDetail.QtyExtra;
                                orderDetail.WarehouseID = (int)purchasedArticleWarehouse.ArrivedAtWarehouseID;
                                _context.Update(orderDetail);
                            }
                            else
                            {
                                orderDetail.Notes = $"Arrived article doesn't cover this row with ordered packages:{orderDetail.QtyPackages} and Extra: {orderDetail.QtyExtra} .";
                                var order = _context.Orders.Where(o => o.ID == orderDetail.OrderID).FirstOrDefault();
                                if (order != null)
                                {
                                    order.HasIssue = true;
                                    if (orderDetail.QtyPackages > 0)
                                    {
                                        if ((QtyPackagesArrived - orderDetail.QtyPackages) >= 0)
                                        {
                                            QtyPackagesArrived -= orderDetail.QtyPackages;
                                        }
                                        else
                                        {
                                            orderDetail.QtyPackages = QtyPackagesArrived;
                                            QtyPackagesArrived = 0;
                                        }
                                    }
                                    if (orderDetail.QtyExtra > 0)
                                    {
                                        if((QtyExtraArrived - orderDetail.QtyExtra) >= 0)
                                        {
                                            QtyExtraArrived -= orderDetail.QtyExtra;
                                        }
                                        else
                                        {
                                            orderDetail.QtyExtra = QtyExtraArrived;
                                            QtyExtraArrived = 0;
                                        }
                                       
                                    }
                                    orderDetail.WarehouseID = (int)purchasedArticleWarehouse.ArrivedAtWarehouseID;
                                    var totalWeightOrPackage = OrderValidation.OrderDetailTotalWeightOrPackage(_context, orderDetail);
                                    orderDetail.Extended_Price = orderDetail.Price * totalWeightOrPackage;
                                    _context.Update(orderDetail);
                                    _context.Update(order);
                                }
                            }
                        }
                    }
                    purchasedArticle.HasIssue = purchasedArticleWarehouse.HasIssue();
                    purchasedArticle.Arrived = true;
                    _context.Update(purchasedArticle);
                    
                    //Adjust On the way warehouse
                    var onthewayWarehouse = _context.Warehouses.Where(w => w.ID == purchasedArticleWarehouse.WarehouseID).FirstOrDefault();
                    var articleInOutForUndoReduce = new ArticleInOut
                    {
                        ArticleID = purchasedArticleWarehouse.PurchasedArticle.ArticleID,
                        WarehouseID = (int)purchasedArticleWarehouse.WarehouseID,
                        CompanyID = ArticleBalance.GetWarehouseCompany(_context, onthewayWarehouse),
                        QtyPackagesOut = totalQtyPackagesToBeShifted, //(purchasedArticleWarehouse.QtyPackagesArrived - QtyPackagesArrived),
                        QtyExtraOut = totalQtyExtraToBeShifted //(purchasedArticleWarehouse.QtyExtraArrived - QtyExtraArrived)
                    };
                    var UndoReduceResult = ArticleBalance.UndoReduce(_context, articleInOutForUndoReduce);
                    if (!UndoReduceResult.Value)
                    {
                        result.Value = false; result.Property = UndoReduceResult.Property; result.Message = UndoReduceResult.Message;
                        return result;
                    }
                    _context.SaveChanges();
                    var articleInOutForUndoAdd = new ArticleInOut
                    {
                        ArticleID = purchasedArticleWarehouse.PurchasedArticle.ArticleID,
                        WarehouseID = (int)purchasedArticleWarehouse.WarehouseID,
                        CompanyID = ArticleBalance.GetWarehouseCompany(_context, onthewayWarehouse),
                        QtyPackagesIn = purchasedArticleWarehouse.QtyPackages,
                        QtyExtraIn = purchasedArticleWarehouse.QtyExtra
                    };
                    var UndoAddResult = ArticleBalance.UndoAdd(_context, articleInOutForUndoAdd);
                    if (!UndoAddResult.Value)
                    {
                        result.Value = false; result.Property = UndoAddResult.Property; result.Message = UndoAddResult.Message;
                        return result;
                    }
                    _context.SaveChanges();
                    //Add or update Arrived warehouse
                    var arrivedWarehouse = _context.Warehouses.Where(w => w.ID == purchasedArticleWarehouse.ArrivedAtWarehouseID).FirstOrDefault();
                    var articleInOutForAdd = new ArticleInOut
                    {
                        ArticleID = purchasedArticleWarehouse.PurchasedArticle.ArticleID,
                        WarehouseID = (int)purchasedArticleWarehouse.ArrivedAtWarehouseID,
                        CompanyID = ArticleBalance.GetWarehouseCompany(_context, arrivedWarehouse),
                        QtyPackagesIn = purchasedArticleWarehouse.QtyPackagesArrived,
                        QtyExtraIn = purchasedArticleWarehouse.QtyExtraArrived
                    };
                    var AddResult = ArticleBalance.Add(_context, articleInOutForAdd);
                    if (!AddResult.Value)
                    {
                        result.Value = false; result.Property = AddResult.Property; result.Message = AddResult.Message;
                        return result;
                    }
                    _context.SaveChanges();
                    var articleInOutForReduce = new ArticleInOut
                    {
                        ArticleID = purchasedArticleWarehouse.PurchasedArticle.ArticleID,
                        WarehouseID = (int)purchasedArticleWarehouse.ArrivedAtWarehouseID,
                        CompanyID = ArticleBalance.GetWarehouseCompany(_context, arrivedWarehouse),
                        QtyPackagesOut = (purchasedArticleWarehouse.QtyPackagesArrived - QtyPackagesArrived),
                        QtyExtraOut = (purchasedArticleWarehouse.QtyExtraArrived - QtyExtraArrived)
                    };
                    var ReduceResult = ArticleBalance.Reduce(_context, articleInOutForReduce);
                    if (!ReduceResult.Value)
                    {
                        result.Value = false; result.Property = ReduceResult.Property; result.Message = ReduceResult.Message;
                        return result;
                    }
                    _context.SaveChanges();
                    //if (QtyPackagesArrived > 0 || QtyExtraArrived > 0)
                    //{

                    //}
                    dbContextTransaction.Commit();
                }



            }
            catch (Exception exception)
            {
                result.Value = false; result.Property = "exception"; result.Message = exception.Message.ToString();
            }
            return result;
        }

        private List<PurchasedArticleWarehouse> GetOnthewayArticles(ApplicationDbContext _context)
        {
            var list = _context.PurchasedArticleWarehouses.Where(a => a.ArrivedDate == null && a.Warehouse.IsOnTheWay == true).Include(p => p.PurchasedArticle).ThenInclude(pa => pa.Article).Include(w => w.Warehouse).OrderBy(a => a.PurchasedArticle.Date).ThenBy(a => a.PurchasedArticle.Article).ToList();
            return list;

        }
        private PropertyValidation Validate(ArrivedArticleViewModel model)
        {
            var result = new PropertyValidation {Value = true, Action="ArrivedArticles", Model="PurchasedArticleWarehouse", Property = "", Message = "" };
            if (model.QtyPackagesArrived  < 0 )
            {
                result.Value = false; result.Property = nameof(model.QtyPackagesArrived); result.Message = "Please put a valid number.";
                return result;
            }
            if (model.QtyExtraArrived < 0)
            {
                result.Value = false; result.Property = nameof(model.QtyExtraArrived); result.Message = "Please put a valid number.";
                return result;
            }
            if (model.QtyPackagesArrived == 0 && model.QtyExtraArrived == 0)
            {
                result.Value = false; result.Property = nameof(model.QtyPackagesArrived); result.Message = "Please put a value bigger than zero.";
                return result;
            }
            if (model.Warehouse.ID == 0)
            {
                result.Value = false; result.Property = nameof(model.QtyPackagesArrived); result.Message = "Please select a warehouse.";
                return result;
            }
            return result;
        }
        #endregion
    }
}
