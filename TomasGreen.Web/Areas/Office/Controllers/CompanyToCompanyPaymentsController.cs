using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using TomasGreen.Web.Data;

namespace TomasGreen.Web.Areas.Office.Controllers
{
    [Area("Office")]
    public class CompanyToCompanyPaymentsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly IStringLocalizer<CompanyToCompanyPaymentsController> _localizer;
        public CompanyToCompanyPaymentsController(ApplicationDbContext context, IHostingEnvironment hostingEnvironment, IStringLocalizer<CompanyToCompanyPaymentsController> localizer)
        {
            _context = context;
            _hostingEnvironment = hostingEnvironment;
            _localizer = localizer;
        }
        // GET: CompanyToCompanyPayments
        public ActionResult Index()
        {
            var companyToCompanyParments = _context.CompanyToCompanyPayments.Include( c => c.PayingCompany)
                .Include(c => c.Currency).Where(p => p.Archive == false).OrderBy(d => d.Date).ThenBy(c => c.PayingCompany.Name).ToList();
            return View();
        }

        // GET: CompanyToCompanyPayments/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: CompanyToCompanyPayments/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: CompanyToCompanyPayments/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: CompanyToCompanyPayments/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: CompanyToCompanyPayments/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: CompanyToCompanyPayments/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: CompanyToCompanyPayments/Delete/5
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
    }
}