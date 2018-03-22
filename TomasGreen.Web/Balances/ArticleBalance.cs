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

            if(model.QtyPackagesIn == 0 && model.QtyPackagesOut == 0 && model.QtyExtraIn == 0 && model.QtyExtraOut == 0)
            {
                result.Value = false; result.Property = nameof(model.QtyPackagesIn); result.Message = "model is not valid.";
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
                    result.Message += " Coultn't save article balance.";
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

                    newArticleWarehouseBalance.QtyPackagesIn = model.QtyPackagesIn;
                    newArticleWarehouseBalance.QtyExtraIn = model.QtyExtraIn;

                    newArticleWarehouseBalance.QtyPackagesOut = 0;
                    newArticleWarehouseBalance.QtyExtraOut = 0;

                    newArticleWarehouseBalance.QtyPackagesOnhand = model.QtyPackagesIn;
                    newArticleWarehouseBalance.QtyExtraOnhand = model.QtyExtraIn;

                    _context.Add(newArticleWarehouseBalance);
                    result.Value = true;
                    return result;

                }
                else
                {
                    savedArticleWarehouseBalance.QtyPackagesIn += model.QtyPackagesIn;
                    savedArticleWarehouseBalance.QtyExtraIn += model.QtyExtraIn;
                    savedArticleWarehouseBalance.QtyExtraOnhand += model.QtyExtraIn;
                    savedArticleWarehouseBalance.QtyPackagesOnhand += model.QtyPackagesIn;
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

        public static PropertyValidation Undo_Add(ApplicationDbContext _context, ArticleInOut model)
        {
            var result = new PropertyValidation(true, "Undo_Add", "ArticleWarehouseBalance", "", "");
            try
            {
               
                var article = _context.Articles.Where(a => a.ID == model.ArticleID).FirstOrDefault();
                var savedArticleWarehouseBalance = _context.ArticleWarehouseBalances
                    .Where(b => b.ArticleID == model.ArticleID 
                           && b.Article.ArticleUnitID == article.ArticleUnitID
                           && b.WarehouseID == model.WarehouseID
                           && b.CompanyID == model.CompanyID)
                    .FirstOrDefault();
                if(savedArticleWarehouseBalance == null)
                {
                    result.Value = false; result.Property = nameof(savedArticleWarehouseBalance.ID); result.Message = "Could't find.";
                    return result;
                }

                savedArticleWarehouseBalance.QtyPackagesIn -= model.QtyPackagesIn;
                if (savedArticleWarehouseBalance.QtyPackagesIn < 0)
                {
                    result.Value = false; result.Property = nameof(savedArticleWarehouseBalance.QtyPackagesIn); result.Message = "Value can not be less then zero.";
                    return result;
                }

                savedArticleWarehouseBalance.QtyExtraIn -= model.QtyExtraIn;
                if (savedArticleWarehouseBalance.QtyExtraIn < 0)
                {
                    result.Value = false; result.Property = nameof(savedArticleWarehouseBalance.QtyExtraIn); result.Message = "Value can not be less then zero.";
                    return result;
                }

                savedArticleWarehouseBalance.QtyPackagesOnhand -= model.QtyPackagesIn;
                if (savedArticleWarehouseBalance.QtyPackagesOnhand < 0)
                {
                    result.Value = false; result.Property = nameof(savedArticleWarehouseBalance.QtyPackagesOnhand); result.Message = "Value can not be less then zero.";
                    return result;
                }
                savedArticleWarehouseBalance.QtyExtraOnhand -= model.QtyExtraIn;
                if (savedArticleWarehouseBalance.QtyExtraOnhand < 0)
                {
                    result.Value = false; result.Property = nameof(savedArticleWarehouseBalance.QtyExtraOnhand); result.Message = "Value can not be less then zero.";
                    return result;
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

        public static PropertyValidation Reduce(ApplicationDbContext _context,  ArticleInOut model)
        {
            var result = new PropertyValidation(true, "Undo_Add", "ArticleWarehouseBalance", "", "");
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
                savedArticleWarehouseBalance.QtyPackagesOut += model.QtyPackagesOut;
                savedArticleWarehouseBalance.QtyExtraOut += model.QtyExtraOut;
                savedArticleWarehouseBalance.QtyPackagesOnhand -= model.QtyPackagesOut;
                if (savedArticleWarehouseBalance.QtyPackagesOnhand < 0)
                {
                    result.Value = false; result.Property = nameof(savedArticleWarehouseBalance.QtyPackagesOnhand); result.Message = "Value can not be less then zero.";
                    return result;
                }
                savedArticleWarehouseBalance.QtyExtraOnhand -= model.QtyExtraOut;
                if (savedArticleWarehouseBalance.QtyExtraOnhand < 0)
                {
                    result.Value = false; result.Property = nameof(savedArticleWarehouseBalance.QtyExtraOnhand); result.Message = "Value can not be less then zero.";
                    return result;
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

        public static PropertyValidation Undo_Reduce(ApplicationDbContext _context, ArticleInOut model)
        {
            var result = new PropertyValidation(true, "RemoveOrderDetailFromBalance", "ArticleWarehouseBalance", "", "");
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

                savedArticleWarehouseBalance.QtyPackagesOut -= model.QtyPackagesOut;
                if (savedArticleWarehouseBalance.QtyPackagesOut < 0)
                {
                    result.Value = false; result.Property = nameof(savedArticleWarehouseBalance.QtyPackagesOut); result.Message = "Value can not be less then zero.";
                    return result;
                }
                savedArticleWarehouseBalance.QtyExtraOut -= model.QtyExtraOut;
                if (savedArticleWarehouseBalance.QtyExtraOut < 0)
                {
                    result.Value = false; result.Property = nameof(savedArticleWarehouseBalance.QtyExtraOut); result.Message = "Value can not be less then zero.";
                    return result;
                }

                savedArticleWarehouseBalance.QtyPackagesOnhand += model.QtyPackagesOut;
                savedArticleWarehouseBalance.QtyExtraOnhand += model.QtyExtraOut;

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

        //public PropertyValidation AddPurchasedArticleToBalance(PurchasedArticleWarehouse model)
        //{
        //    var result = new PropertyValidation(true, "AddPurchasedArticleToBalance", "ArticleWarehouseBalance", "", "");
        //    try
        //    {
        //        var articleWarehouseBalance = new ArticleWarehouseBalance();
        //        var article = _context.Articles.Where(a => a.ID == model.PurchasedArticle.ArticleID).FirstOrDefault();
        //        articleWarehouseBalance = _context.ArticleWarehouseBalances.Where(b => b.ArticleID == model.PurchasedArticle.ArticleID && b.Article.ArticleUnitID == article.ArticleUnitID && b.WarehouseID == model.WarehouseID).FirstOrDefault();
        //        if (articleWarehouseBalance == null)
        //        {
        //            var newArticleWarehouseBalance = new ArticleWarehouseBalance();
        //            newArticleWarehouseBalance.Warehouse = _context.Warehouses.Where(w => w.ID == model.WarehouseID).FirstOrDefault();
        //            newArticleWarehouseBalance.WarehouseID = model.WarehouseID;
                   
        //            newArticleWarehouseBalance.Article = article;
        //            newArticleWarehouseBalance.ArticleID = article.ID;
        //            newArticleWarehouseBalance.QtyPackagesIn = model.QtyPackages;
        //            newArticleWarehouseBalance.QtyExtraIn = model.QtyExtra;
        //            newArticleWarehouseBalance.QtyPackagesOut = 0;
        //            newArticleWarehouseBalance.QtyExtraOut = 0;
        //            newArticleWarehouseBalance.QtyPackagesOnhand = model.QtyPackages;
        //            newArticleWarehouseBalance.QtyExtraOnhand = model.QtyExtra;
        //            _context.Add(newArticleWarehouseBalance);
        //            result.Value = true;
        //            return result;

        //        }
        //        else
        //        {
        //            articleWarehouseBalance.QtyPackagesIn += model.QtyPackages;
        //            articleWarehouseBalance.QtyExtraIn += model.QtyExtra;
        //            articleWarehouseBalance.QtyExtraOnhand += model.QtyExtra;
        //            articleWarehouseBalance.QtyPackagesOnhand += model.QtyPackages;
        //            _context.Update(articleWarehouseBalance);
        //            //  await _context.SaveChangesAsync();
        //        }
        //    }
        //    catch (Exception exception)
        //    {
        //        result.Value = false; result.Property = "Exception"; result.Message = exception.Message.ToString();
        //        return result;
        //    }
        //    result.Value = true;
        //    return result;
        //}

        //public  PropertyValidation RemovePurchasedArticleFromBalance(PurchasedArticleWarehouse model)
        //{
        //    var result = new PropertyValidation(true, "RemovePurchasedArticleFromBalance", "ArticleWarehouseBalance", "", "");
        //    try
        //    {
        //        var articleWarehouseBalance = new ArticleWarehouseBalance();
        //        var article = _context.Articles.Where(a => a.ID == model.PurchasedArticle.ArticleID).FirstOrDefault();
        //        articleWarehouseBalance = _context.ArticleWarehouseBalances.Where(b => b.ArticleID == model.PurchasedArticle.ArticleID && b.Article.ArticleUnitID == article.ArticleUnitID && b.WarehouseID == model.WarehouseID).FirstOrDefault();
        //        articleWarehouseBalance.QtyPackagesIn -= model.QtyPackages;
        //        if (articleWarehouseBalance.QtyPackagesIn < 0)
        //        {
        //            result.Value = false; result.Property = nameof(articleWarehouseBalance.QtyPackagesIn); result.Message = "Value can not be less then zero.";
        //            return result;
        //        }
        //        articleWarehouseBalance.QtyExtraIn -= model.QtyExtra;
        //        if (articleWarehouseBalance.QtyExtraIn < 0)
        //        {
        //            result.Value = false; result.Property = nameof(articleWarehouseBalance.QtyExtraIn); result.Message = "Value can not be less then zero.";
        //            return result;
        //        }
                
        //        articleWarehouseBalance.QtyPackagesOnhand -= model.QtyPackages;
        //        if (articleWarehouseBalance.QtyPackagesOnhand < 0)
        //        {
        //            result.Value = false; result.Property = nameof(articleWarehouseBalance.QtyPackagesOnhand); result.Message = "Value can not be less then zero.";
        //            return result;
        //        }
        //        articleWarehouseBalance.QtyExtraOnhand -= model.QtyExtra;
        //        if (articleWarehouseBalance.QtyExtraOnhand < 0)
        //        {
        //            result.Value = false; result.Property = nameof(articleWarehouseBalance.QtyExtraOnhand); result.Message = "Value can not be less then zero.";
        //            return result;
        //        }
               
        //        _context.Update(articleWarehouseBalance);
        //    }
        //    catch (Exception exception)
        //    {
        //        result.Value = false; result.Property = "Exception"; result.Message = exception.Message.ToString();
        //        return result;
        //    }

        //    result.Value = true;
        //    return result;
        //}

        //public PropertyValidation AddOrderDetailToBalance(OrderDetail model)
        //{
        //    var result = new PropertyValidation(true, "AddOrderDetailToBalance", "ArticleWarehouseBalance", "", "");
        //    try
        //    {
        //        var articleWarehouseBalance = _context.ArticleWarehouseBalances.Where(b => b.ArticleID == model.ArticleID && b.WarehouseID == model.WarehouseID).FirstOrDefault();
        //        if(articleWarehouseBalance == null)
        //        {
        //            result.Value = false; result.Property = nameof(articleWarehouseBalance.ID); result.Message = "There is no such article in this warehouse.";
        //            return result;
        //        }
        //        articleWarehouseBalance.QtyPackagesOut += model.QtyPackages;
        //        articleWarehouseBalance.QtyExtraOut += model.QtyExtra;
        //        articleWarehouseBalance.QtyPackagesOnhand -= model.QtyPackages;
        //        if(articleWarehouseBalance.QtyPackagesOnhand < 0)
        //        {
        //            result.Value = false; result.Property = nameof(articleWarehouseBalance.QtyPackagesOnhand); result.Message = "Value can not be less then zero.";
        //            return result;
        //        }
        //        articleWarehouseBalance.QtyExtraOnhand -= model.QtyExtra;
        //        if(articleWarehouseBalance.QtyExtraOnhand < 0)
        //        {
        //            result.Value = false; result.Property = nameof(articleWarehouseBalance.QtyExtraOnhand); result.Message = "Value can not be less then zero.";
        //            return result;
        //        }
        //        _context.Update(articleWarehouseBalance);
               
        //    }
        //    catch (Exception exception)
        //    {
        //        result.Value = false; result.Property = "Exception"; result.Message = exception.Message.ToString();
        //        return result;
        //    }
        //    result.Value = true;
        //    return result;
        //}

        //public PropertyValidation RemoveOrderDetailFromBalance(OrderDetail model)
        //{
        //    var result = new PropertyValidation(true, "RemoveOrderDetailFromBalance", "ArticleWarehouseBalance", "", "");
        //    try
        //    {
        //        var articleWarehouseBalance = _context.ArticleWarehouseBalances.Where(b => b.ArticleID == model.ArticleID && b.WarehouseID == model.WarehouseID).FirstOrDefault();
        //        if (articleWarehouseBalance == null)
        //        {
        //            result.Value = false; result.Property = nameof(articleWarehouseBalance.ID); result.Message = "There is no such article in this warehouse.";
        //            return result;
        //        }
                               
        //        articleWarehouseBalance.QtyPackagesOut -= model.QtyPackages;
        //        if(articleWarehouseBalance.QtyPackagesOut < 0)
        //        {
        //            result.Value = false; result.Property = nameof(articleWarehouseBalance.QtyPackagesOut); result.Message = "Value can not be less then zero.";
        //            return result;
        //        }
        //        articleWarehouseBalance.QtyExtraOut -= model.QtyExtra;
        //        if(articleWarehouseBalance.QtyExtraOut < 0)
        //        {
        //            result.Value = false; result.Property = nameof(articleWarehouseBalance.QtyExtraOut); result.Message = "Value can not be less then zero.";
        //            return result;
        //        }

        //        articleWarehouseBalance.QtyPackagesOnhand += model.QtyPackages;
        //        articleWarehouseBalance.QtyExtraOnhand += model.QtyExtra;

        //        _context.Update(articleWarehouseBalance);

        //    }
        //    catch(Exception exception)
        //    {
        //        result.Value = false; result.Property = "Exception"; result.Message = exception.Message.ToString();
        //        return result;
        //    }
        //    return result;
        //}

        

    }
}
