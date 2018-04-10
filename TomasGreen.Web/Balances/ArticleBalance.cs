using System;
using System.Collections.Generic;
using System.Text;
using TomasGreen.Model.Models;
using TomasGreen.Web.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using TomasGreen.Web.Validations;

namespace TomasGreen.Web.Balances
{
    public static class ArticleBalance
    {

        public static bool WarehouseIsCoutomers(ApplicationDbContext _contex, int warehouseID)
        {
            return (_contex.Warehouses.Any(w => w.ID == warehouseID && w.IsCustomers == true));
        }
        public static int GetWarehouseCompany(ApplicationDbContext _contex, Warehouse warehouse)
        {
            if(warehouse.CompanySectionID != null)
            {
                var companyID = _contex.CompanySections.Where(s => s.ID == warehouse.CompanySectionID).FirstOrDefault().CompanyID;
                return companyID;
            }
            return 0;
        }
        public static int GetWarehouseCompany(ApplicationDbContext _contex, int warehouseID)
        {
            var warehouse = _contex.Warehouses.Where(w => w.ID == warehouseID).FirstOrDefault();
            if(warehouse.CompanySectionID != null)
            {
                var companyID = _contex.CompanySections.Where(s => s.ID == warehouse.CompanySectionID).FirstOrDefault().CompanyID;
                return companyID;
            }

            return 0;
        }
        private static PropertyValidation ValidateValues(ArticleWarehouseBalance model)
        {
            var result = new PropertyValidation(true, "Validate", "ArticleWarehouseBalance", "", "");
            if (model.QtyPackagesIn < 0)
            {
                result.Value = false; result.Property = nameof(model.QtyPackagesIn); result.Message = "Value can not be less then zero.";
                return result;
            }
            if (model.QtyExtraIn < 0 )
            {
                result.Value = false; result.Property = nameof(model.QtyExtraIn); result.Message = "Value can not be less then zero.";
                return result;
            }
            if (model.QtyPackagesOut < 0)
            {
                result.Value = false; result.Property = nameof(model.QtyPackagesOut); result.Message = "Value can not be less then zero.";
                return result;
            }
            if (model.QtyExtraOut < 0)
            {
                result.Value = false; result.Property = nameof(model.QtyExtraOut); result.Message = "Value can not be less then zero.";
                return result;
            }
            if (model.QtyPackagesOnhand < 0)
            {
                result.Value = false; result.Property = nameof(model.QtyPackagesOnhand); result.Message = "Value can not be less then zero.";
                return result;
            }
            if (model.QtyExtraOnhand < 0)
            {
                result.Value = false; result.Property = nameof(model.QtyExtraOnhand); result.Message = "Value can not be less then zero.";
                return result;
            }
            return result;
        }
        public static PropertyValidation ValidateArticleInOut(ArticleInOut model)
        {
            var result = new PropertyValidation(true, "ValidateArticleInOut", "ArticleWarehouseBalance", "", "");
            if(model.ArticleID == 0)
            {
                result.Value = false; result.Property = nameof(model.ArticleID); result.Message = "Article undefined.";
                return result;
            }
            if (model.WarehouseID == 0)
            {
                result.Value = false; result.Property = nameof(model.WarehouseID); result.Message = "Warehouse undefined.";
                return result;
            }
            if (model.CompanyID == 0)
            {
                result.Value = false; result.Property = nameof(model.WarehouseID); result.Message = "Company undefined.";
                return result;
            }

            if(model.QtyPackages == 0 && model.QtyExtra == 0)
            {
                result.Value = false; result.Property = nameof(model.QtyPackages); result.Message = "model is not valid.";
                return result;
            }

            return result;
        }
        public static PropertyValidation Add(ApplicationDbContext _context, ArticleInOut model)
        {
            var result = ValidateArticleInOut(model);
            try
            {
                if(!result.Value)
                {
                    result.Message += " Couldn't save article balance.";
                    return result;
                }
              
                var article = _context.Articles.Where(a => a.ID == model.ArticleID).FirstOrDefault();
                var savedArticleWarehouseBalance = _context.ArticleWarehouseBalances
                    .Where(b => b.ArticleID == model.ArticleID
                           && b.Article.ArticleUnitID == article.ArticleUnitID 
                           && b.WarehouseID == model.WarehouseID 
                           && b.CompanyID == model.CompanyID)
                    .FirstOrDefault();

                if (savedArticleWarehouseBalance == null)
                {
                    var newArticleWarehouseBalance = new ArticleWarehouseBalance();

                    newArticleWarehouseBalance.CompanyID = model.CompanyID;
                    newArticleWarehouseBalance.Company = _context.Companies.Where(c => c.ID == model.CompanyID).FirstOrDefault();

                    newArticleWarehouseBalance.Warehouse = _context.Warehouses.Where(w => w.ID == model.WarehouseID).FirstOrDefault();
                    newArticleWarehouseBalance.WarehouseID = model.WarehouseID;

                    newArticleWarehouseBalance.Article = article;
                    newArticleWarehouseBalance.ArticleID = article.ID;

                    newArticleWarehouseBalance.QtyPackagesIn = model.QtyPackages;
                    newArticleWarehouseBalance.QtyExtraIn = model.QtyExtra;

                    newArticleWarehouseBalance.QtyPackagesOut = 0;
                    newArticleWarehouseBalance.QtyExtraOut = 0;

                    newArticleWarehouseBalance.QtyPackagesOnhand = model.QtyPackages;
                    newArticleWarehouseBalance.QtyExtraOnhand = model.QtyExtra;
                    var validValues = ValidateValues(newArticleWarehouseBalance);
                    if(!validValues.Value)
                    {
                        return validValues;
                    }
                    _context.Add(newArticleWarehouseBalance);
                    result.Value = true;
                    return result;

                }
                else
                {
                    savedArticleWarehouseBalance.QtyPackagesIn += model.QtyPackages;
                    savedArticleWarehouseBalance.QtyExtraIn += model.QtyExtra;
                    savedArticleWarehouseBalance.QtyExtraOnhand = (savedArticleWarehouseBalance.QtyExtraIn - savedArticleWarehouseBalance.QtyExtraOut);
                    savedArticleWarehouseBalance.QtyPackagesOnhand = (savedArticleWarehouseBalance.QtyPackagesIn - savedArticleWarehouseBalance.QtyPackagesOut);
                    var validValues = ValidateValues(savedArticleWarehouseBalance);
                    if (!validValues.Value)
                    {
                        return validValues;
                    }
                    _context.Update(savedArticleWarehouseBalance);
                }
            }
            catch (Exception exception)
            {
                result.Value = false; result.Property = "Exception"; result.Message = exception.Message.ToString();
                return result;
            }
            result.Value = true;
            return result;
        }
        public static PropertyValidation Reduce(ApplicationDbContext _context, ArticleInOut model)
        {
            var result = new PropertyValidation(true, "Reduce", "ArticleWarehouseBalance", "", "");
            try
            {
                var article = _context.Articles.Where(a => a.ID == model.ArticleID).FirstOrDefault();
                var savedArticleWarehouseBalance = _context.ArticleWarehouseBalances
                    .Where(b => b.ArticleID == model.ArticleID
                           && b.Article.ArticleUnitID == article.ArticleUnitID
                           && b.WarehouseID == model.WarehouseID
                           && b.CompanyID == model.CompanyID)
                    .FirstOrDefault();
                if (savedArticleWarehouseBalance == null)
                {
                    result.Value = false; result.Property = nameof(savedArticleWarehouseBalance.ID); result.Message = "There is no such article in this warehouse.";
                    return result;
                }
                savedArticleWarehouseBalance.QtyPackagesOut += model.QtyPackages;
                savedArticleWarehouseBalance.QtyExtraOut += model.QtyExtra;
                savedArticleWarehouseBalance.QtyPackagesOnhand = (savedArticleWarehouseBalance.QtyPackagesIn - savedArticleWarehouseBalance.QtyPackagesOut);
                savedArticleWarehouseBalance.QtyExtraOnhand = (savedArticleWarehouseBalance.QtyExtraIn - savedArticleWarehouseBalance.QtyExtraOut);
                var validValues = ValidateValues(savedArticleWarehouseBalance);
                if (!validValues.Value)
                {
                    return validValues;
                }
                _context.Update(savedArticleWarehouseBalance);
            }
            catch (Exception exception)
            {
                result.Value = false; result.Property = "Exception"; result.Message = exception.Message.ToString();
                return result;
            }
            result.Value = true;
            return result;
        }




    }
}
