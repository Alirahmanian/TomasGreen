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
    public class ArticleBalance
    {
        private readonly ApplicationDbContext _context;

        public ArticleBalance(ApplicationDbContext context)
        {
            _context = context;
        }
        public PropertyValidatedMessage AddPurchasedArticleToBalance(PurchasedArticleWarehouse model)
        {
            var result = new PropertyValidatedMessage(true, "AddPurchasedArticleToBalance", "ArticleWarehouseBalance", "", "");
            try
            {
                var articleWarehouseBalance = new ArticleWarehouseBalance();
                var article = _context.Articles.Where(a => a.ID == model.PurchasedArticle.ArticleID).FirstOrDefault();
                articleWarehouseBalance = _context.ArticleWarehouseBalances.Where(b => b.ArticleID == model.PurchasedArticle.ArticleID && b.Article.ArticleUnitID == article.ArticleUnitID && b.WarehouseID == model.WarehouseID).FirstOrDefault();
                if (articleWarehouseBalance == null)
                {
                    var newArticleWarehouseBalance = new ArticleWarehouseBalance();
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
                    _context.Add(newArticleWarehouseBalance);
                    result.Value = true;
                    return result;

                }
                else
                {
                    articleWarehouseBalance.QtyPackagesIn += model.QtyPackages;
                    articleWarehouseBalance.QtyExtraIn += model.QtyExtra;
                    articleWarehouseBalance.QtyExtraOnhand += model.QtyExtra;
                    articleWarehouseBalance.QtyPackagesOnhand += model.QtyPackages;
                    _context.Update(articleWarehouseBalance);
                    //  await _context.SaveChangesAsync();
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

        public  PropertyValidatedMessage RemovePurchasedArticleFromBalance(PurchasedArticleWarehouse model)
        {
            var result = new PropertyValidatedMessage(true, "RemovePurchasedArticleFromBalance", "ArticleWarehouseBalance", "", "");
            try
            {
                var articleWarehouseBalance = new ArticleWarehouseBalance();
                var article = _context.Articles.Where(a => a.ID == model.PurchasedArticle.ArticleID).FirstOrDefault();
                articleWarehouseBalance = _context.ArticleWarehouseBalances.Where(b => b.ArticleID == model.PurchasedArticle.ArticleID && b.Article.ArticleUnitID == article.ArticleUnitID && b.WarehouseID == model.WarehouseID).FirstOrDefault();
                articleWarehouseBalance.QtyPackagesIn -= model.QtyPackages;
                if (articleWarehouseBalance.QtyPackagesIn < 0)
                {
                    result.Value = false; result.Property = nameof(articleWarehouseBalance.QtyPackagesIn); result.Message = "Value can not be less then zero.";
                    return result;
                }
                articleWarehouseBalance.QtyExtraIn -= model.QtyExtra;
                if (articleWarehouseBalance.QtyExtraIn < 0)
                {
                    result.Value = false; result.Property = nameof(articleWarehouseBalance.QtyExtraIn); result.Message = "Value can not be less then zero.";
                    return result;
                }
                
                articleWarehouseBalance.QtyPackagesOnhand -= model.QtyPackages;
                if (articleWarehouseBalance.QtyPackagesOnhand < 0)
                {
                    result.Value = false; result.Property = nameof(articleWarehouseBalance.QtyPackagesOnhand); result.Message = "Value can not be less then zero.";
                    return result;
                }
                articleWarehouseBalance.QtyExtraOnhand -= model.QtyExtra;
                if (articleWarehouseBalance.QtyExtraOnhand < 0)
                {
                    result.Value = false; result.Property = nameof(articleWarehouseBalance.QtyExtraOnhand); result.Message = "Value can not be less then zero.";
                    return result;
                }
               
                _context.Update(articleWarehouseBalance);
            }
            catch (Exception exception)
            {
                result.Value = false; result.Property = "Exception"; result.Message = exception.Message.ToString();
                return result;
            }

            result.Value = true;
            return result;
        }

        public PropertyValidatedMessage AddOrderDetailToBalance(OrderDetail model)
        {
            var result = new PropertyValidatedMessage(true, "AddOrderDetailToBalance", "ArticleWarehouseBalance", "", "");
            try
            {
                var articleWarehouseBalance = _context.ArticleWarehouseBalances.Where(b => b.ArticleID == model.ArticleID && b.WarehouseID == model.WarehouseID).FirstOrDefault();
                if(articleWarehouseBalance == null)
                {
                    result.Value = false; result.Property = nameof(articleWarehouseBalance.ID); result.Message = "There is no such article in this warehouse.";
                    return result;
                }
                articleWarehouseBalance.QtyPackagesOut += model.QtyPackages;
                articleWarehouseBalance.QtyExtraOut += model.QtyExtra;
                articleWarehouseBalance.QtyPackagesOnhand -= model.QtyPackages;
                if(articleWarehouseBalance.QtyPackagesOnhand < 0)
                {
                    result.Value = false; result.Property = nameof(articleWarehouseBalance.QtyPackagesOnhand); result.Message = "Value can not be less then zero.";
                    return result;
                }
                articleWarehouseBalance.QtyExtraOnhand -= model.QtyExtra;
                if(articleWarehouseBalance.QtyExtraOnhand < 0)
                {
                    result.Value = false; result.Property = nameof(articleWarehouseBalance.QtyExtraOnhand); result.Message = "Value can not be less then zero.";
                    return result;
                }
                _context.Update(articleWarehouseBalance);
               
            }
            catch (Exception exception)
            {
                result.Value = false; result.Property = "Exception"; result.Message = exception.Message.ToString();
                return result;
            }
            result.Value = true;
            return result;
        }

        public PropertyValidatedMessage RemoveOrderDetailFromBalance(OrderDetail model)
        {
            var result = new PropertyValidatedMessage(true, "RemoveOrderDetailFromBalance", "ArticleWarehouseBalance", "", "");
            try
            {
                var articleWarehouseBalance = _context.ArticleWarehouseBalances.Where(b => b.ArticleID == model.ArticleID && b.WarehouseID == model.WarehouseID).FirstOrDefault();
                if (articleWarehouseBalance == null)
                {
                    result.Value = false; result.Property = nameof(articleWarehouseBalance.ID); result.Message = "There is no such article in this warehouse.";
                    return result;
                }
                               
                articleWarehouseBalance.QtyPackagesOut -= model.QtyPackages;
                if(articleWarehouseBalance.QtyPackagesOut < 0)
                {
                    result.Value = false; result.Property = nameof(articleWarehouseBalance.QtyPackagesOut); result.Message = "Value can not be less then zero.";
                    return result;
                }
                articleWarehouseBalance.QtyExtraOut -= model.QtyExtra;
                if(articleWarehouseBalance.QtyExtraOut < 0)
                {
                    result.Value = false; result.Property = nameof(articleWarehouseBalance.QtyExtraOut); result.Message = "Value can not be less then zero.";
                    return result;
                }

                articleWarehouseBalance.QtyPackagesOnhand += model.QtyPackages;
                articleWarehouseBalance.QtyExtraOnhand += model.QtyExtra;

                _context.Update(articleWarehouseBalance);

            }
            catch(Exception exception)
            {
                result.Value = false; result.Property = "Exception"; result.Message = exception.Message.ToString();
                return result;
            }
            return result;
        }

        

    }
}
