using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TomasGreen.Model.Models;
using TomasGreen.Web.Data;

namespace TomasGreen.Web.Validations
{
    public static class PurchasedArticleValidation 
    {
        public static PropertyValidation PurchasedArticleIsValid(ApplicationDbContext _context, PurchasedArticle model)
        {
            var result = new PropertyValidation(true, "PurchasedArticleIsValid", "PurchasedArticle", "", "");
            if (model.Date == null)
            {
                result.Value = false; result.Property = nameof(model.Date); result.Message = "Please choose a date.";
                return result;
            }
            if(model.CompanyID == 0)
            {
                result.Value = false; result.Property = nameof(model.CompanyID); result.Message = "Please choose a supplier.";
                return result;
            }
            if (model.CurrencyID == 0)
            {
                result.Value = false; result.Property = nameof(model.CurrencyID); result.Message = "Please choose a currency.";
                return result;
            }
           
            return result;
        }

        public static PropertyValidation PurchasedArticleDetailIsValid(ApplicationDbContext _context, PurchasedArticleDetail model)
        {
            var result = new PropertyValidation(true, "PurchasedArticleDetailIsValid", "PurchasedArticleDetail", "", "");
            if (model.WarehouseID == 0)
            {
                result.Value = false; result.Property = nameof(model.WarehouseID); result.Message = "Warehouse is missing.";
                return result;
            }
            if (model.ArticleID == 0)
            {
                result.Value = false; result.Property = nameof(model.ArticleID); result.Message = "Please choose an article.";
                return result;
            }
            if (model.QtyPackages == 0 && model.QtyExtra == 0)
            {
                result.Value = false; result.Property = nameof(model.QtyPackages); result.Message = "Please put Packages or extra.";
                return result;
            }
            if (model.UnitPrice == 0)
            {
                result.Value = false; result.Property = nameof(model.UnitPrice); result.Message = "Please put price.";
                return result;
            }
            if (model.PurchasedArticleID == 0)
            {
                result.Value = false; result.Property = nameof(model.PurchasedArticleID); result.Message = "Model is not valid."; result.SystemMessage = "PurchasedArticleID is missing.";
                return result;
            }

            return result;
        }

    }
}
