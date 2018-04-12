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
            if(!PurchasedArticleDetailIsUnique(_context, model))
            {
                result.Value = false; result.Property = nameof(model.ID); result.Message = "Model is not unique."; 
                return result;
            }

            return result;
        }
        public static bool PurchasedArticleDetailIsUnique(ApplicationDbContext _context, PurchasedArticleDetail model)
        {
            var savedPurchasedArticleDetail = _context.PurchasedArticleDetails.Where(d => d.PurchasedArticleID == model.PurchasedArticleID &&
            d.WarehouseID == model.WarehouseID && d.ArticleID == model.ArticleID).FirstOrDefault();
            if(savedPurchasedArticleDetail != null)
            {
                if(savedPurchasedArticleDetail.ID != model.ID)
                {
                    return false;
                }
            }
            return true;
        }
        public static PropertyValidation PurchasedArticleCostDetailIsValid(ApplicationDbContext _context, PurchasedArticleCostDetail model)
        {
            var result = new PropertyValidation(true, "PurchasedArticleCostDetailIsValid", "PurchasedArticleCostDetail", "", "");
            if (model.CompanyID == 0)
            {
                result.Value = false; result.Property = nameof(model.CompanyID); result.Message = "please select a company.";
                return result;
            }
            if (model.CurrencyID == 0)
            {
                result.Value = false; result.Property = nameof(model.CurrencyID); result.Message = "please select a currency.";
                return result;
            }
            if (model.Amount == 0)
            {
                result.Value = false; result.Property = nameof(model.Amount); result.Message = "Please put amount.";
                return result;
            }
            if (model.PurchasedArticleID == 0)
            {
                result.Value = false; result.Property = nameof(model.PurchasedArticleID); result.Message = "Model is not valid."; result.SystemMessage = "PurchasedArticleID is missing.";
                return result;
            }
            if(!PurchasedArticleCostDetailIsUnique(_context, model))
            {
                result.Value = false; result.Property = nameof(model.ID); result.Message = "Model is not unique.";
                return result;
            }
            return result;
        }
        public static bool PurchasedArticleCostDetailIsUnique(ApplicationDbContext _context, PurchasedArticleCostDetail model)
        {
            var savedPurchasedArticleCostDetail = _context.PurchasedArticleCostDetails.Where(d => d.PurchasedArticleID == model.PurchasedArticleID &&
            d.CompanyID == model.CompanyID && d.CurrencyID == model.CurrencyID &&
            d.PaymentTypeID == model.PaymentTypeID).FirstOrDefault();
            if (savedPurchasedArticleCostDetail != null)
            {
                if (savedPurchasedArticleCostDetail.ID != model.ID)
                {
                    return false;
                }
            }
            return true;
        }
        public static PropertyValidation PurchasedArticleShortageDealingDetailIsValid(ApplicationDbContext _context, PurchasedArticleShortageDealingDetail model)
        {
            var result = new PropertyValidation(true, "PurchasedArticleShortageDealingDetailIsValid", "PurchasedArticleShortageDealingDetail", "", "");
            if (model.CompanyID == 0)
            {
                result.Value = false; result.Property = nameof(model.CompanyID); result.Message = "please select a company.";
                return result;
            }
            if (model.CurrencyID == 0)
            {
                result.Value = false; result.Property = nameof(model.CurrencyID); result.Message = "please select a currency.";
                return result;
            }
            if (model.Amount == 0)
            {
                result.Value = false; result.Property = nameof(model.Amount); result.Message = "Please put amount.";
                return result;
            }
            if (model.PurchasedArticleID == 0)
            {
                result.Value = false; result.Property = nameof(model.PurchasedArticleID); result.Message = "Model is not valid."; result.SystemMessage = "PurchasedArticleID is missing.";
                return result;
            }
            if(!PurchasedArticleShortageDealingDetailIsUnique(_context, model))
            {
                result.Value = false; result.Property = nameof(model.ID); result.Message = "Model is not Unique.";
                return result;
            }
            return result;
        }
        public static bool PurchasedArticleShortageDealingDetailIsUnique(ApplicationDbContext _context, PurchasedArticleShortageDealingDetail model)
        {
            var savedPurchasedArticleShortageDealingDetail = _context.PurchasedArticleShortageDealingDetails.Where(d => d.PurchasedArticleID == model.PurchasedArticleID &&
            d.CompanyID == model.CompanyID && d.CurrencyID == model.CurrencyID).FirstOrDefault();
            if (savedPurchasedArticleShortageDealingDetail != null)
            {
                if (savedPurchasedArticleShortageDealingDetail.ID != model.ID)
                {
                    return false;
                }
            }
            return true;
        }
        public static PropertyValidation PurchasedArticleContainerDetailIsValid(ApplicationDbContext _context, PurchasedArticleContainerDetail model)
        {
            var result = new PropertyValidation(true, "PurchasedArticleContainerDetailIsValid", "PurchasedArticleContainerDetail", "", "");
            if (model.ContainerNumber == "")
            {
                result.Value = false; result.Property = nameof(model.ContainerNumber); result.Message = "please put container number.";
                return result;
            }
            if (model.PurchasedArticleID == 0)
            {
                result.Value = false; result.Property = nameof(model.PurchasedArticleID); result.Message = "Model is not valid."; result.SystemMessage = "PurchasedArticleID is missing.";
                return result;
            }
            if(!PurchasedArticleContainerDetailIsUnique(_context, model))
            {
                result.Value = false; result.Property = nameof(model.ID); result.Message = "Model is not unique."; 
                return result;
            }
            return result;
        }
        public static bool PurchasedArticleContainerDetailIsUnique(ApplicationDbContext _context, PurchasedArticleContainerDetail model)
        {
            var savedPurchasedArticleContainerDetail = _context.PurchasedArticleContainerDetails.Where(d => d.PurchasedArticleID == model.PurchasedArticleID &&
            d.ContainerNumber == model.ContainerNumber).FirstOrDefault();
            if (savedPurchasedArticleContainerDetail != null)
            {
                if (savedPurchasedArticleContainerDetail.ID != model.ID)
                {
                    return false;
                }
            }
            return true;
        }
    }
}
