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
using TomasGreen.Web.Data;
using TomasGreen.Web.Extensions;

namespace TomasGreen.Web.Areas.Import.Controllers
{
    [Area("Import")]
    public class ArticleUnitsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly IStringLocalizer<ArticleUnitsController> _localizer;
        public ArticleUnitsController(ApplicationDbContext context, IHostingEnvironment hostingEnvironment, IStringLocalizer<ArticleUnitsController> localizer)
        {
            _context = context;
            _hostingEnvironment = hostingEnvironment;
            _localizer = localizer;
        }
        // GET: Import/ArticleUnits
        public async Task<IActionResult> Index()
        {
            return View(await _context.ArticleUnits.Include(a => a.Articles).ToListAsync());
        }

        // GET: Import/ArticleUnits/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var articleUnit = await _context.ArticleUnits
                .SingleOrDefaultAsync(m => m.ID == id);
            if (articleUnit == null)
            {
                return NotFound();
            }

            return View(articleUnit);
        }

        // GET: Import/ArticleUnits/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Import/ArticleUnits/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create( ArticleUnit articleUnit)
        {
            if (ModelState.IsValid)
            {
                var result = Validate(articleUnit);
                if(!result.Value )
                {
                    ModelState.AddModelError("", _localizer["Couldn't save."]);
                    if (_hostingEnvironment.IsDevelopment())
                    {
                        ModelState.AddModelError("", JSonHelper.ToJSon(result));
                    }
                    return View(articleUnit);
                }
                _context.Add(articleUnit);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(articleUnit);
        }

        // GET: Import/ArticleUnits/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var articleUnit = await _context.ArticleUnits.SingleOrDefaultAsync(m => m.ID == id);
            if (articleUnit == null)
            {
                return NotFound();
            }
            return View(articleUnit);
        }

        // POST: Import/ArticleUnits/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id,  ArticleUnit articleUnit)
        {
            if (id != articleUnit.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var result = Validate(articleUnit);
                    if (!result.Value)
                    {
                        ModelState.AddModelError("", _localizer["Couldn't save."]);
                        if (_hostingEnvironment.IsDevelopment())
                        {
                            ModelState.AddModelError("", JSonHelper.ToJSon(result));
                        }
                        return View(articleUnit);
                    }
                    _context.Update(articleUnit);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ArticleUnitExists(articleUnit.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(articleUnit);
        }

        // GET: Import/ArticleUnits/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var articleUnit = await _context.ArticleUnits
                .SingleOrDefaultAsync(m => m.ID == id);
            if (articleUnit == null)
            {
                return NotFound();
            }

            return View(articleUnit);
        }

        // POST: Import/ArticleUnits/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var articleUnit = await _context.ArticleUnits.SingleOrDefaultAsync(m => m.ID == id);
            if(IsRelated(articleUnit))
            {
                ViewBag.Error = _localizer["Couldn't delete. The Post is already related to other entities."];
                return View(articleUnit);
            }
            _context.ArticleUnits.Remove(articleUnit);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ArticleUnitExists(int id)
        {
            return _context.ArticleUnits.Any(e => e.ID == id);
        }

        #region
        private PropertyValidation Validate(ArticleUnit model)
        {
            var result = model.Validate();
            if(result.Value)
            {
                var nameIsTaken = _context.ArticleUnits.Any(u => u.Name == model.Name && u.ID != model.ID);
                if(nameIsTaken)
                {
                    result.Value = false; result.Property = nameof(model.Name); result.Message = "Name is already taken.";
                    return result;
                }
                var measurementIsTaken = false;
                if (model.MeasuresByG == true)
                {
                    measurementIsTaken = _context.ArticleUnits.Any(u => u.MeasuresByG == true && u.ID != model.ID);
                    if (measurementIsTaken)
                    {
                        result.Value = false; result.Property = nameof(model.MeasuresByG); result.Message = "Measurement is already taken.";
                        return result;
                    }
                }
                if (model.MeasuresByKg == true)
                {
                    measurementIsTaken = _context.ArticleUnits.Any(u => u.MeasuresByKg == true && u.ID != model.ID);
                    if (measurementIsTaken)
                    {
                        result.Value = false; result.Property = nameof(model.MeasuresByKg); result.Message = "Measurement is already taken.";
                        return result;
                    }
                }
                if (model.MeasuresByTon == true)
                {
                    measurementIsTaken = _context.ArticleUnits.Any(u => u.MeasuresByTon == true && u.ID != model.ID);
                    if (measurementIsTaken)
                    {
                        result.Value = false; result.Property = nameof(model.MeasuresByTon); result.Message = "Measurement is already taken.";
                        return result;
                    }
                }
                if (model.MeasuresByPiece == true)
                {
                    measurementIsTaken = _context.ArticleUnits.Any(u => u.MeasuresByPiece == true && u.ID != model.ID);
                    if (measurementIsTaken)
                    {
                        result.Value = false; result.Property = nameof(model.MeasuresByPiece); result.Message = "Measurement is already taken.";
                        return result;
                    }
                }
                if(model.MeasurePerKg > 0)
                {
                    var measurePerKg = _context.ArticleUnits.Any(u => u.MeasurePerKg == model.MeasurePerKg && u.ID != model.ID);
                    if (measurePerKg)
                    {
                        result.Value = false; result.Property = nameof(model.MeasurePerKg); result.Message = "There is already a unit with the same value for 'In kg'.";
                        return result;
                    }
                }

            }
            return result;
        }

        private bool IsRelated(ArticleUnit model)
        {
            return  _context.Articles.Any(a => a.ArticleUnitID == model.ID);
        }

        #endregion
    }
}
