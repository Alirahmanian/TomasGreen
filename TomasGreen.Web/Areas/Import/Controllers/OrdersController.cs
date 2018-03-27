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
            var applicationDbContext = _context.Orders.Include(o => o.Company);
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
        public IActionResult Create(int? id)
        {
            var model = new SaveOrderViewModel();
            model.OrderDetail = new OrderDetail();
            if (id > 0)
            {
                model.Order = _context.Orders.Where(o => o.ID == id).FirstOrDefault();
                model.Order.OrderDetails = _context.OrderDetails.Include(a => a.Article).Include(w => w.Warehouse).Where(d => d.OrderID == model.Order.ID).ToList();
                if(model.Order != null)
                {
                    model.OrderDetail.OrderID = model.Order.ID;
                }
            }
            else
            {
                model.Order = new Order();
                model.Order.OrderDate = DateTime.Today;
            }
           
            ViewData["CompanyID"] = new SelectList(_context.Companies, "ID", "Name");
            ViewData["EmployeeID"] = new SelectList(_context.Employees, "ID", "FullName");
            ViewData["ArticleCategoryID"] = new SelectList(_context.ArticleCategories, "ID", "Name");
           // ViewData["ArticleID"] = new SelectList(_context.Articles, "ID", "Name");
           // ViewData["WarehouseID"] = new SelectList(_context.Warehouses, "ID", "Name");
            return View(model);
        }

        // POST: Sales/Orders/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SaveOrderViewModel model, string SaveOrder, string AddOrderDetail)
        {
            if (!string.IsNullOrWhiteSpace(AddOrderDetail) )
            {
                
                if (model.Order.ID == 0)
                {
                    ModelState.AddModelError("", _localizer["Please save the order header before adding articles to it."]);
                    AddOrderLists(model);
                    return View(model);
                }
            }
            
            if (!ModelState.IsValid)
            {
                AddOrderLists(model);
                return View(model);
            }
            var customModelValidator = OrderValidation.OrderIsValid(_context, model.Order);
            if (customModelValidator.Value == false)
            {
                ModelState.AddModelError(customModelValidator.Property, customModelValidator.Message);
                AddOrderLists(model);
                return View(model);
            }

            if (ModelState.IsValid)
            {
                if(model.Order.ID == 0)
                {
                    //new order
                    var guid = Guid.NewGuid();
                    model.Order.Guid = guid;
                    _context.Add(model.Order);
                    await _context.SaveChangesAsync();
                    var savedOrder = _context.Orders.Where(o => o.OrderDate == model.Order.OrderDate && o.CompanyID == model.Order.CompanyID && o.Guid == guid).FirstOrDefault();
                    if (savedOrder != null)
                    {
                        model.Order = savedOrder;
                        AddOrderLists(model);
                        ViewBag.OrderID = model.Order.ID;//savedOrder.ID;
                        return View(model);
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
                    _context.Update(model.Order);
                    if (!string.IsNullOrWhiteSpace(AddOrderDetail))
                    {
                        if(model.Order.ID != 0)
                        {
                            //var articlePackageWeight = _context.Articles.Where(a => a.ID == model.OrderDetail.ArticleID).FirstOrDefault()?.PackageWeight ?? 0;
                            //var totalWeight = (model.OrderDetail.QtyBoxes * articlePackageWeight) + (model.OrderDetail.QtyReserveBoxes * articlePackageWeight) + model.OrderDetail.QtyKg;

                            var totalWeightOrPackage = OrderValidation.OrderDetailTotalWeightOrPackage(_context, model.OrderDetail);
                            model.OrderDetail.Extended_Price = model.OrderDetail.Price * totalWeightOrPackage;
                            if(model.OrderDetail.OrderID == 0)
                            {
                                model.OrderDetail.OrderID = model.Order.ID;
                            }
                            var customValidator = OrderValidation.OrderDetailIsValid(_context, model.OrderDetail);
                            if(!customValidator.Value)
                            {
                                ModelState.AddModelError("", "Couldn't save.");
                                if (_hostingEnvironment.IsDevelopment())
                                {
                                    ModelState.AddModelError("", JSonHelper.ToJSon(customValidator));
                                }
                                AddOrderLists(model);
                                return View(model);
                            }
                            model.OrderDetail.Warehouse = _context.Warehouses.Where(w => w.ID == model.OrderDetail.WarehouseID).FirstOrDefault();
                            if (model.OrderDetail.ID > 0)
                            {
                                //Old OrderDetail has to be added back first
                                var savedOrderDetail = _context.OrderDetails.Where(d => d.ID == model.OrderDetail.ID).FirstOrDefault();
                                if(savedOrderDetail != null)
                                {
                                    model.Order.TotalPrice -= (model.Order.TotalPrice - model.OrderDetail.Extended_Price) > 0 ? model.OrderDetail.Extended_Price : 0;
                                    savedOrderDetail.Warehouse = _context.Warehouses.Where(w => w.ID == savedOrderDetail.WarehouseID).FirstOrDefault();
                                    
                                    var articlsInOutForAdd = new ArticleInOut
                                    {
                                        ArticleID = savedOrderDetail.ArticleID,
                                        WarehouseID = savedOrderDetail.WarehouseID,
                                        CompanyID = ArticleBalance.GetWarehouseCompany(_context, savedOrderDetail.Warehouse),
                                        QtyPackagesIn = savedOrderDetail.QtyPackages,
                                        QtyExtraIn = savedOrderDetail.QtyExtra
                                    };
                                    var AddResult = ArticleBalance.Add(_context, articlsInOutForAdd);
                                    if (AddResult.Value == false)
                                    {
                                        ModelState.AddModelError("", _localizer["Couldn't save."]);
                                        if (_hostingEnvironment.IsDevelopment())
                                        {
                                            ModelState.AddModelError("", JSonHelper.ToJSon(AddResult));
                                        }
                                        AddOrderLists(model);
                                        return View(model);
                                    }
                                }
                            }
                            var articleInOut = new ArticleInOut
                            {
                                ArticleID = model.OrderDetail.ArticleID,
                                WarehouseID = model.OrderDetail.WarehouseID,
                                CompanyID = ArticleBalance.GetWarehouseCompany(_context, model.OrderDetail.Warehouse),
                                QtyPackagesOut = model.OrderDetail.QtyPackages,
                                QtyExtraOut = model.OrderDetail.QtyExtra
                            };
                            var result = ArticleBalance.Reduce(_context, articleInOut);
                            if(result.Value == false)
                            {
                                ModelState.AddModelError("", _localizer["Couldn't save."]);
                                if (_hostingEnvironment.IsDevelopment())
                                {
                                    ModelState.AddModelError("", JSonHelper.ToJSon(result));
                                }
                                AddOrderLists(model);
                                return View(model);
                            }

                            _context.OrderDetails.Add(model.OrderDetail);
                            model.Order.TotalPrice += model.OrderDetail.Extended_Price;
                            _context.Update(model.Order);
                            await _context.SaveChangesAsync();

                            AddOrderLists(model);
                            return View(model);
                        }
                        else
                        {
                            //
                            ModelState.AddModelError("", "Please save order before trying to add details.");
                            AddOrderLists(model);
                            return View(model);
                        }
                    }
                }
            }
            else
            {
                ModelState.AddModelError("", _localizer["Couldn't save."]);
                AddOrderLists(model);
                return View(model);
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

            var order = await _context.Orders.SingleOrDefaultAsync(m => m.ID == id);
            if (order == null)
            {
                return NotFound();
            }
            ViewData["CompanyID"] = new SelectList(_context.Companies, "ID", "Name", order.CompanyID);
            return View(order);
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
            orderDetail.Order.TotalPrice -= (orderDetail.Order.TotalPrice - orderDetail.Extended_Price) > 0 ? orderDetail.Extended_Price : 0;
            var articleInOutForUndoReduce = new ArticleInOut
            {
                ArticleID = orderDetail.ArticleID,
                WarehouseID = orderDetail.WarehouseID,
                CompanyID = ArticleBalance.GetWarehouseCompany(_context, _context.Warehouses.Where(w => w.ID == orderDetail.WarehouseID).FirstOrDefault()),
                QtyPackagesOut = orderDetail.QtyPackages,
                QtyExtraOut = orderDetail.QtyExtra
            };
            var UndoReduceResult = ArticleBalance.UndoReduce(_context, articleInOutForUndoReduce);
            if (!UndoReduceResult.Value)
            {
                ModelState.AddModelError("", _localizer["Couldn't delete."]);
                if (_hostingEnvironment.IsDevelopment())
                {
                    ModelState.AddModelError("", JSonHelper.ToJSon(UndoReduceResult));
                }
                ViewBag.ErrorMessage = _localizer["Couldn't delete."];
                return RedirectToAction("Create", orderId);
            }
            _context.Update(orderDetail.Order);
            _context.Remove(orderDetail);
            _context.SaveChanges();
            return RedirectToAction("Create", orderId);
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
                    ViewData["WarehouseID"] = new SelectList((from aw in _context.ArticleWarehouseBalances
                                                              where aw.ArticleID == model.OrderDetail.ArticleID
                                                              join a in _context.Articles on aw.ArticleID equals a.ID
                                                              join w in _context.Warehouses on aw.WarehouseID equals w.ID
                                                              orderby w.Name
                                                              select new
                                                              {
                                                                  Id = w.ID,
                                                                  Name = w.Name,
                                                                  Articlesonhand = _localizer["[Package"] + ":" + aw.QtyPackagesOnhand.ToString() + ", " + _localizer["Extra"] + ": " + aw.QtyExtraOnhand.ToString() + "]"
                                                              }
                                    ), "ID", "Name", model.OrderDetail.WarehouseID);
                }
            }
            
           
        }
       

        #region Ajax Calls
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

        public ActionResult GetArticleWarehouseBalance(int articleId, int warehouseId)
        {
            var balance = _context.ArticleWarehouseBalances.Where(b => b.ArticleID == articleId && b.WarehouseID == warehouseId).Include(a => a.Article).Include(w => w.Warehouse).FirstOrDefault();
            return PartialView("_ArticleWarehouseBalancePartialView", balance);
        }
        #endregion
    }
}
