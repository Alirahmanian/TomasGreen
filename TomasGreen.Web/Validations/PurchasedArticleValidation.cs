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
        public static PropertyValidatedMessage PurchasedArticleIsValid(ApplicationDbContext _context, PurchasedArticle model)
        {
            var result = new PropertyValidatedMessage(true, "AddPurchasedArticleToBalance", "ArticleWarehouseBalance", "", "");
            if (model.Date == null)
            {
                result.Value = false; result.Property = nameof(model.Date); result.Message = "Please choose a date.";
                return result;
            }
            if (model.ArticleID == 0)
            {
                result.Value = false; result.Property = nameof(model.ArticleID); result.Message = "Please choose an article.";
                return result;
            }
            if(model.CompanyID == 0)
            {
                result.Value = false; result.Property = nameof(model.CompanyID); result.Message = "Please choose a supplier.";
                return result;
            }
            if (model.UnitPrice == 0)
            {
                result.Value = false; result.Property = nameof(model.UnitPrice); result.Message = "Please put unit price.";
                return result;
            }
            return result;
        }

    }
}
