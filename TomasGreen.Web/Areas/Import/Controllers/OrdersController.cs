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
    public class OrdersController : BaseController
    {
        private readonly ApplicationDbContext _context;
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly IStringLocalizer<OrdersController> _localizer;
        public OrdersController(ApplicationDbContext context, IHostingEnvironment hostingEnvironment, IStringLocalizer<OrdersController> localizer)
        {
            _context = context;
            _hostingEnvironment = hostingEnvironment;
            _localizer = localizer;
        }

        // GET: Sales/Orders
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Orders.Include(o => o.Company).Include(d => d.OrderDetails);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Sales/Orders/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Orders
                .Include(o => o.Company)
                .SingleOrDefaultAsync(m => m.ID == id);
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        // GET: Sales/Orders/Create
        public IActionResult Create(int? id, int? orderDetailId)
        {
            var model = new SaveOrderViewModel();
            model.OrderDetail = new OrderDetail();
            if (id > 0)
            {
                model.Order = _context.Orders.Where(o => o.ID == id).FirstOrDefault();
                model.Order.OrderDetails = _context.OrderDetails.Include(a => a.Article).Include(w => w.Warehouse).Where(d => d.OrderID == model.Order.ID).ToList();
                if(model.Order != null)
                {
                    if(orderDetailId > 0)
                    {
                        model.OrderDetail = _context.OrderDetails.Include(w => w.Warehouse).Include(a =>a.Article).Where(d => d.ID == orderDetailId).AsNoTracking().FirstOrDefault();
                        model.ArticleCategory = _context.ArticleCategories.Where(c => c.ID == model.OrderDetail.Article.ArticleCategoryID).FirstOrDefault();
                    }
                    else
                    {
                        model.OrderDetail.OrderID = model.Order.ID;
                    }
                    AddOrderLists(model);
                }
            }
            else
            {
                model.Order = new Order();
                model.Order.OrderDate = DateTime.Today;
                AddOrderLists(model);
            }
            return View(model);
        }

        // POST: Sales/Orders/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SaveOrderViewModel model, string SaveOrder, string SaveOrderDetail, string DeleteOrder)
        {
            //Save order
            if (!string.IsNullOrWhiteSpace(SaveOrder))
            {
                var customModelValidator = OrderValidation.OrderIsValid(_context, model.Order);
                if (customModelValidator.Value == false)
                {
                    ModelState.AddModelError(customModelValidator.Property, customModelValidator.Message);
                    AddOrderLists(model);
                    return View(model);
                }
                var skippedErrors = ModelState.Keys;
                foreach (var key in skippedErrors)
                {
                    ModelState.Remove(key);
                }
                if (model.Order.ID == 0)
                {
                    //new order
                    var guid = Guid.NewGuid();
                    model.Order.Guid = guid;
                    //model.Order.OrderNumber = NewOrderNumber(model.Order); later
                    _context.Add(model.Order);
                    await _context.SaveChangesAsync();
                    var savedOrder = _context.Orders.Where(o => o.OrderDate == model.Order.OrderDate && o.CompanyID == model.Order.CompanyID && o.Guid == guid).FirstOrDefault();
                    if (savedOrder != null)
                    {
                        model.Order = savedOrder;
                        AddOrderLists(model);
                        return RedirectToAction(nameof(Create), new { id = model.Order.ID });
                    }
                    else
                    {
                        ModelState.AddModelError("", "Could't save Order.");
                        AddOrderLists(model);
                        return View(model);
                    }
                }
                else
                {
                    //update
                    var savedOrder = _context.Orders.AsNoTracking().Where(o => o.ID == model.Order.ID).FirstOrDefault();
                    if (savedOrder == null)
                    {
                        ModelState.AddModelError("", "Could't find Order.");
                        AddOrderLists(model);
                        return View(model);
                    }
                    if (savedOrder.CompanyID != model.Order.CompanyID)
                    {
                        var orderDetails = _context.OrderDetails.Any(d => d.OrderID == savedOrder.ID);
                        if (orderDetails)
                        {
                            ModelState.AddModelError("", "Could't save. Order has rows with different company. Please remove rows first or delete the order. ");
                            AddOrderLists(model);
                            return View(model);
                        }
                    }
                    //Adjust previous customer balance
                    //Adjust current customer balance
                    savedOrder.Cash = model.Order.Cash;
                    savedOrder.Coments = model.Order.Coments;
                    savedOrder.CompanyID = model.Order.CompanyID;
                    savedOrder.Confirmed = model.Order.Confirmed;
                    savedOrder.EmployeeID = model.Order.EmployeeID;
                    savedOrder.OrderDate = model.Order.OrderDate;
                    savedOrder.PaymentWarning = model.Order.PaymentWarning;
                    savedOrder.TransportFee = model.Order.TransportFee;
                    savedOrder.CurrencyID = model.Order.CurrencyID;
                    _context.Update(savedOrder);
                    await _context.SaveChangesAsync();
                    AddOrderLists(model);
                    return View(model);
                }

            }
            //Delete order
            if (!string.IsNullOrWhiteSpace(DeleteOrder))
            {
                if (model.Order.ID == 0)
                {
                    ModelState.AddModelError("", _localizer["Please save the order header before adding articles to it."]);
                    AddOrderLists(model);
                    return View(model);
                }
                var orderToBeDeleted = _context.Orders.Include(d => d.OrderDetails).AsNoTracking().Where(o => o.ID == model.Order.ID).FirstOrDefault();
                if (orderToBeDeleted == null)
                {
                    ModelState.AddModelError("", "Could't find Order.");
                    AddOrderLists(model);
                    return View(model);
                }
                var orderTotalPrice = orderToBeDeleted.GetTotalPrice();
                //Adjust cutomer balance
                foreach (var orderDetail in orderToBeDeleted.OrderDetails)
                {
                    var articlsInOutForReduce = new ArticleInOut
                    {
                        ArticleID = orderDetail.ArticleID,
                        WarehouseID = orderDetail.WarehouseID,
                        CompanyID = ArticleBalance.GetWarehouseCompany(_context, _context.Warehouses.Where(w => w.ID == orderDetail.WarehouseID).AsNoTracking().FirstOrDefault()),
                        QtyPackages = (orderDetail.QtyPackages * -1),
                        QtyExtra = (orderDetail.QtyExtra * -1)
                    };
                    var AddResult = ArticleBalance.Add(_context, articlsInOutForReduce);
                    if (AddResult.Value == false)
                    {
                        ModelState.AddModelError("", _localizer["Couldn't Delete order."]);
                        if (_hostingEnvironment.IsDevelopment())
                        {
                            ModelState.AddModelError("", JSonHelper.ToJSon(AddResult));
                        }
                        AddOrderLists(model);
                        return View(model);
                    }
                    _context.OrderDetails.Remove(orderDetail);
                }
                //Adjust cutomer balance
                _context.Orders.Remove(orderToBeDeleted);
                await _context.SaveChangesAsync();
                AddOrderLists(model);
                return RedirectToAction(nameof(Index));

            }

            //save Order details
            if (!string.IsNullOrWhiteSpace(SaveOrderDetail) )
            {
                if (model.Order.ID == 0)
                {
                    ModelState.AddModelError("", _localizer["Please save the order header before adding articles to it."]);
                    AddOrderLists(model);
                    return View(model);
                }
                var totalWeightOrPackage = OrderValidation.OrderDetailTotalWeightOrPackage(_context, model.OrderDetail);
                model.OrderDetail.Extended_Price = model.OrderDetail.Price * totalWeightOrPackage;
                if(model.OrderDetail.OrderID == 0)
                {
                    model.OrderDetail.OrderID = model.Order.ID;
                }
                var customValidator = OrderValidation.OrderDetailIsValid(_context, model.OrderDetail);
                if (!customValidator.Value)
                {
                    ModelState.AddModelError("", "Couldn't save.");
                    if (_hostingEnvironment.IsDevelopment())
                    {
                        ModelState.AddModelError("", JSonHelper.ToJSon(customValidator));
                    }
                    AddOrderLists(model);
                    return View(model);
                }
                if (model.OrderDetail.ID > 0)
                {
                    //Old OrderDetail has to be added back first
                    var savedOrderDetail = _context.OrderDetails.Where(d => d.ID == model.OrderDetail.ID).AsNoTracking().FirstOrDefault();
                    if (savedOrderDetail != null)
                    {
                        var articlsInOutForUndoReduce = new ArticleInOut
                        {
                            ArticleID = savedOrderDetail.ArticleID,
                            WarehouseID = savedOrderDetail.WarehouseID,
                            CompanyID = ArticleBalance.GetWarehouseCompany(_context, _context.Warehouses.Where(w => w.ID == savedOrderDetail.WarehouseID).AsNoTracking().FirstOrDefault()),
                            QtyPackages = (savedOrderDetail.QtyPackages * -1),
                            QtyExtra = (savedOrderDetail.QtyExtra * -1)
                        };
                        var undoReduceResult = ArticleBalance.Reduce(_context, articlsInOutForUndoReduce);
                        if (undoReduceResult.Value == false)
                        {
                            ModelState.AddModelError("", _localizer["Couldn't save."]);
                            if (_hostingEnvironment.IsDevelopment())
                            {
                                ModelState.AddModelError("", JSonHelper.ToJSon(undoReduceResult));
                            }
                            AddOrderLists(model);
                            return View(model);
                        }
                        var companyBalance = new CompanyCreditDebitBalance
                        {
                            CompanyID = _context.Orders.Where(o => o.ID == savedOrderDetail.OrderID).FirstOrDefault().CompanyID,
                            CurrencyID = _context.Orders.Where(o => o.ID == savedOrderDetail.OrderID).FirstOrDefault().CurrencyID,
                            Debit = (savedOrderDetail.Extended_Price * -1)
                        };
                        var undoDebitResult = CompanyBalance.Add(_context, companyBalance);
                        if(!undoDebitResult.Value)
                        {
                            ModelState.AddModelError("", _localizer["Couldn't save."]);
                            if (_hostingEnvironment.IsDevelopment())
                            {
                                ModelState.AddModelError("", JSonHelper.ToJSon(undoDebitResult));
                            }
                            AddOrderLists(model);
                            return View(model);
                        }
                        _context.SaveChanges();
                        savedOrderDetail.ArticleID = model.OrderDetail.ArticleID;
                        savedOrderDetail.WarehouseID = model.OrderDetail.WarehouseID;
                        savedOrderDetail.QtyPackages = model.OrderDetail.QtyPackages;
                        savedOrderDetail.QtyExtra = model.OrderDetail.QtyExtra;
                        savedOrderDetail.Price = model.OrderDetail.Price;
                        savedOrderDetail.Extended_Price = model.OrderDetail.Extended_Price;

                        var articleInOut = new ArticleInOut
                        {
                            ArticleID = savedOrderDetail.ArticleID,
                            WarehouseID = savedOrderDetail.WarehouseID,
                            CompanyID = ArticleBalance.GetWarehouseCompany(_context, _context.Warehouses.Where(w => w.ID == savedOrderDetail.WarehouseID).FirstOrDefault()),
                            QtyPackages = savedOrderDetail.QtyPackages,
                            QtyExtra = savedOrderDetail.QtyExtra
                        };
                        var result = ArticleBalance.Reduce(_context, articleInOut);
                        if (result.Value == false)
                        {
                            ModelState.AddModelError("", _localizer["Couldn't save."]);
                            if (_hostingEnvironment.IsDevelopment())
                            {
                                ModelState.AddModelError("", JSonHelper.ToJSon(result));
                            }
                            AddOrderLists(model);
                            return View(model);
                        }
                      
                        companyBalance.Debit = savedOrderDetail.Extended_Price;
                        var AddDebitResult = CompanyBalance.Add(_context, companyBalance);
                        if (!undoDebitResult.Value)
                        {
                            ModelState.AddModelError("", _localizer["Couldn't save."]);
                            if (_hostingEnvironment.IsDevelopment())
                            {
                                ModelState.AddModelError("", JSonHelper.ToJSon(undoDebitResult));
                            }
                            AddOrderLists(model);
                            return View(model);
                        }
                        _context.OrderDetails.Update(savedOrderDetail);
                           
                    }
                }
                else
                {
                    // new order detail
                    var articleInOut = new ArticleInOut
                    {
                        ArticleID = model.OrderDetail.ArticleID,
                        WarehouseID = model.OrderDetail.WarehouseID,
                        CompanyID = ArticleBalance.GetWarehouseCompany(_context, _context.Warehouses.Where(w => w.ID == model.OrderDetail.WarehouseID).FirstOrDefault()),
                        QtyPackages = model.OrderDetail.QtyPackages,
                        QtyExtra = model.OrderDetail.QtyExtra
                    };
                    var result = ArticleBalance.Reduce(_context, articleInOut);
                    if (result.Value == false)
                    {
                        ModelState.AddModelError("", _localizer["Couldn't save."]);
                        if (_hostingEnvironment.IsDevelopment())
                        {
                            ModelState.AddModelError("", JSonHelper.ToJSon(result));
                        }
                        AddOrderLists(model);
                        return View(model);
                    }
                    var companyBalance = new CompanyCreditDebitBalance
                    {
                        CompanyID = model.Order.CompanyID,
                        CurrencyID = model.Order.CurrencyID,
                        Debit = model.OrderDetail.Extended_Price
                    };
                    var AddDebitResult = CompanyBalance.Add(_context, companyBalance);
                    if (!AddDebitResult.Value)
                    {
                        ModelState.AddModelError("", _localizer["Couldn't save."]);
                        if (_hostingEnvironment.IsDevelopment())
                        {
                            ModelState.AddModelError("", JSonHelper.ToJSon(AddDebitResult));
                        }
                        AddOrderLists(model);
                        return View(model);
                    }
                    _context.OrderDetails.Add(model.OrderDetail);
                      
                }
                await _context.SaveChangesAsync();
                AddOrderLists(model);
                return RedirectToAction(nameof(Create), new { id = model.Order.ID });
                
               
            }
           
            
            
            AddOrderLists(model);
            return View(model);
        }

        // GET: Sales/Orders/Edit/5
        public async Task<IActionResult> EditRow(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var orderDetail = _context.OrderDetails.Where(d => d.ID == id).FirstOrDefault();
            if(orderDetail == null)
            {
                return NotFound();
            }
            return RedirectToAction(nameof(Create), new { id= orderDetail.OrderID, orderDetailId = orderDetail.ID});
        }

        // GET: Sales/Orders/Delete/5
        public async Task<IActionResult> DeleteRow(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var orderDetail = await _context.OrderDetails.Include(o => o.Order).SingleOrDefaultAsync(m => m.ID == id);
            if (orderDetail == null)
            {
                return NotFound();
            }
            var orderId = orderDetail.OrderID;
            orderDetail.Order.TotalPrice -= (orderDetail.Order.TotalPrice - orderDetail.Extended_Price) >= 0 ? orderDetail.Extended_Price : 0;
            var articleInOutForUndoReduce = new ArticleInOut
            {
                ArticleID = orderDetail.ArticleID,
                WarehouseID = orderDetail.WarehouseID,
                CompanyID = ArticleBalance.GetWarehouseCompany(_context, _context.Warehouses.Where(w => w.ID == orderDetail.WarehouseID).FirstOrDefault()),
                QtyPackages = (orderDetail.QtyPackages * -1),
                QtyExtra = (orderDetail.QtyExtra * -1)
            };
            var UndoReduceResult = ArticleBalance.Reduce(_context, articleInOutForUndoReduce);
            if (!UndoReduceResult.Value)
            {
                ModelState.AddModelError("", _localizer["Couldn't delete."]);
                if (_hostingEnvironment.IsDevelopment())
                {
                    ModelState.AddModelError("", JSonHelper.ToJSon(UndoReduceResult));
                }
                ViewBag.ErrorMessage = _localizer["Couldn't delete."];
                return RedirectToAction(nameof(Create), orderId);
            }
            _context.Update(orderDetail.Order);
            _context.Remove(orderDetail);
            _context.SaveChanges();
            return RedirectToAction(nameof(Create), new { Id = orderId });
        }

        // POST: Sales/Orders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var order = await _context.Orders.SingleOrDefaultAsync(m => m.ID == id);
            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OrderExists(int id)
        {
            return _context.Orders.Any(e => e.ID == id);
        }

        private void AddOrderLists(SaveOrderViewModel model)
        {
            if (model.Order?.ID > 0)
            {
                model.Order.OrderDetails = _context.OrderDetails.Include(a => a.Article).Include(w => w.Warehouse).Where(d => d.OrderID == model.Order.ID).ToList();
            }
            ViewData["CurrencyID"] = new SelectList(_context.Currencies.Where(c => c.Archive == false).OrderBy(c => c.Code), "ID", "Code", model.Order?.CurrencyID);
            ViewData["CompanyID"] = new SelectList(_context.Companies, "ID", "Name", model.Order?.CompanyID);
            ViewData["EmployeeID"] = new SelectList(_context.Employees, "ID", "FullName", model.Order?.EmployeeID);
            ViewData["ArticleCategoryID"] = new SelectList(_context.ArticleCategories, "ID", "Name", model.ArticleCategory?.ID);
            if (model.Order.ID > 0)
            {
                if (model.ArticleCategory != null)
                {
                    ViewData["ArticleID"] = new SelectList(_context.Articles.Where(a => a.ArticleCategoryID == model.ArticleCategory.ID), "ID", "Name", model.OrderDetail?.ArticleID);
                }
                if (model.OrderDetail != null && model.OrderDetail.ArticleID != 0)
                {

                    var warehouses = (from aw in _context.ArticleWarehouseBalances
                                      where aw.ArticleID == model.OrderDetail.ArticleID && (aw.QtyExtraOnhand > 0 || aw.QtyPackagesOnhand > 0)
                                      join a in _context.Articles on aw.ArticleID equals a.ID
                                      join w in _context.Warehouses on aw.WarehouseID equals w.ID
                                      orderby w.Name
                                      select new Warehouse
                                      {
                                          ID = w.ID,
                                          Name = w.Name
                                      }
                                    ).ToList();
                    //var warehouses = _context.ArticleWarehouseBalances.Where(b => b.ArticleID == model.OrderDetail.ArticleID && b.WarehouseID == model.OrderDetail.WarehouseID)
                    //    .Where(b => b.QtyPackagesOnhand > 0 || b.QtyExtraOnhand > 0).Include(w => w.Warehouse).Select(b => b.Warehouse).ToList();
                    ViewData["WarehouseID"] = new SelectList(warehouses, "ID", "Name", model.OrderDetail?.WarehouseID);
                }
            }
            
           
        }
        
        private string NewOrderNumber(Order model)
        {
            var result = "";
           
            return result;
        }

        #region Ajax Calls
        public JsonResult GetCurrencyRate(int currencyId)
        {
            var currency = _context.Currencies.Where(c => c.ID == currencyId).FirstOrDefault();
            return new JsonResult(currency);
        }
        public JsonResult GetArticleCategories()
        {
            var articleCategories = _context.ArticleCategories.Where(c => c.Archive != true).OrderBy(c => c.Name);
            return new JsonResult(articleCategories);
        }

        public JsonResult GetArticlesByCategoryId (int categoryId)
        {
            var articles = from a in _context.Articles where a.ArticleCategoryID == categoryId orderby a.Name select new { ID = a.ID, Name = a.Name };

           // var articles = _context.Articles.Where(a => a.ArticleCategoryID == categoryId).OrderBy(c => c.Name).ToList();
            return new JsonResult(articles.ToList());
        }
        public JsonResult GetWarehousesByArticleID(int articleId)
        {
            var warehouses= (from aw in _context.ArticleWarehouseBalances where aw.ArticleID == articleId && (aw.QtyExtraOnhand > 0 || aw.QtyPackagesOnhand > 0)
                                 join a in _context.Articles on aw.ArticleID equals a.ID
                                 join w in _context.Warehouses on aw.WarehouseID equals w.ID
                                 orderby w.Name
                                 select new
                                 {
                                     Id = w.ID, Name = w.Name, 
                                     Articlesonhand = _localizer["[Package"] + ": " + aw.QtyPackagesOnhand.ToString() + ", " + _localizer["Extra"] + ": " + aw.QtyExtraOnhand.ToString() + "]"
                                 }
                                );
            return new JsonResult(warehouses.ToList());
        }

        public ActionResult GetCompanyInfoForOrder(int customerId)
        {
            var customer = _context.Companies.Where(c => c.ID == customerId).FirstOrDefault();
            return PartialView("_CustomerDetailsForOrderPartialView", customer);
        }

        public ActionResult GetOrderForPickList(int orderId)
        {
            var order = _context.Orders.AsNoTracking().Include(c => c.Company).Include(e => e.Employee).Where(c => c.ID == orderId).FirstOrDefault();
            if (order != null)
            {
                order.OrderDetails = _context.OrderDetails.AsNoTracking().Include(a => a.Article).Include(w => w.Warehouse).Where(d => d.OrderID == order.ID).ToList();
            }
                
            return PartialView("_OrderPickListPartialView", order);
        }

        public ActionResult GetArticleWarehouseBalance(int articleId, int warehouseId)
        {
            var balance = _context.ArticleWarehouseBalances.Where(b => b.ArticleID == articleId && b.WarehouseID == warehouseId).Include(a => a.Article).Include(w => w.Warehouse).FirstOrDefault();
            return PartialView("_ArticleWarehouseBalancePartialView", balance);
        }
        public ActionResult GetOrderDetails(int orderId)
        {
            var orderDetails = _context.OrderDetails.Where(d => d.OrderID == orderId).ToList();
            return PartialView("_OrderDetailsPartialView", orderDetails);
        }
       
        #endregion
    }
}
