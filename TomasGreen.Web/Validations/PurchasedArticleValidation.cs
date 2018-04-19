using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TomasGreen.Model.Models;
using TomasGreen.Web.Data;

namespace TomasGreen.Web.Validations
{
    public static class PurchaseArticleValidation 
    {
        public static PropertyValidation PurchaseArticleIsValid(ApplicationDbContext _context, PurchaseArticle model)
        {
            var result = new PropertyValidation(true, "PurchaseArticleIsValid", "PurchaseArticle", "", "");
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
        public static PropertyValidation PurchaseArticleDetailIsValid(ApplicationDbContext _context, PurchaseArticleDetail model)
        {
            var result = new PropertyValidation(true, "PurchaseArticleDetailIsValid", "PurchaseArticleDetail", "", "");
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
            if (model.PurchaseArticleID == 0)
            {
                result.Value = false; result.Property = nameof(model.PurchaseArticleID); result.Message = "Model is not valid."; result.SystemMessage = "PurchaseArticleID is missing.";
                return result;
            }
            if(!PurchaseArticleDetailIsUnique(_context, model))
            {
                result.Value = false; result.Property = nameof(model.ID); result.Message = "Model is not unique."; 
                return result;
            }

            return result;
        }
        public static bool PurchaseArticleDetailIsUnique(ApplicationDbContext _context, PurchaseArticleDetail model)
        {
            var savedPurchaseArticleDetail = _context.PurchaseArticleDetails.Where(d => d.PurchaseArticleID == model.PurchaseArticleID &&
            d.WarehouseID == model.WarehouseID && d.ArticleID == model.ArticleID).FirstOrDefault();
            if(savedPurchaseArticleDetail != null)
            {
                if(savedPurchaseArticleDetail.ID != model.ID)
                {
                    return false;
                }
            }
            return true;
        }
        public static PropertyValidation PurchaseArticleCostDetailIsValid(ApplicationDbContext _context, PurchaseArticleCostDetail model)
        {
            var result = new PropertyValidation(true, "PurchaseArticleCostDetailIsValid", "PurchaseArticleCostDetail", "", "");
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
            if (model.PurchaseArticleID == 0)
            {
                result.Value = false; result.Property = nameof(model.PurchaseArticleID); result.Message = "Model is not valid."; result.SystemMessage = "PurchaseArticleID is missing.";
                return result;
            }
            if(!PurchaseArticleCostDetailIsUnique(_context, model))
            {
                result.Value = false; result.Property = nameof(model.ID); result.Message = "Model is not unique.";
                return result;
            }
            return result;
        }
        public static bool PurchaseArticleCostDetailIsUnique(ApplicationDbContext _context, PurchaseArticleCostDetail model)
        {
            var savedPurchaseArticleCostDetail = _context.PurchaseArticleCostDetails.Where(d => d.PurchaseArticleID == model.PurchaseArticleID &&
            d.CompanyID == model.CompanyID && d.CurrencyID == model.CurrencyID &&
            d.PaymentTypeID == model.PaymentTypeID).FirstOrDefault();
            if (savedPurchaseArticleCostDetail != null)
            {
                if (savedPurchaseArticleCostDetail.ID != model.ID)
                {
                    return false;
                }
            }
            return true;
        }
        public static PropertyValidation PurchaseArticleShortageDealingDetailIsValid(ApplicationDbContext _context, PurchaseArticleShortageDealingDetail model)
        {
            var result = new PropertyValidation(true, "PurchaseArticleShortageDealingDetailIsValid", "PurchaseArticleShortageDealingDetail", "", "");
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
            if (model.PurchaseArticleID == 0)
            {
                result.Value = false; result.Property = nameof(model.PurchaseArticleID); result.Message = "Model is not valid."; result.SystemMessage = "PurchaseArticleID is missing.";
                return result;
            }
            if(!PurchaseArticleShortageDealingDetailIsUnique(_context, model))
            {
                result.Value = false; result.Property = nameof(model.ID); result.Message = "Model is not Unique.";
                return result;
            }
            return result;
        }
        public static bool PurchaseArticleShortageDealingDetailIsUnique(ApplicationDbContext _context, PurchaseArticleShortageDealingDetail model)
        {
            var savedPurchaseArticleShortageDealingDetail = _context.PurchaseArticleShortageDealingDetails.Where(d => d.PurchaseArticleID == model.PurchaseArticleID &&
            d.CompanyID == model.CompanyID && d.CurrencyID == model.CurrencyID).FirstOrDefault();
            if (savedPurchaseArticleShortageDealingDetail != null)
            {
                if (savedPurchaseArticleShortageDealingDetail.ID != model.ID)
                {
                    return false;
                }
            }
            return true;
        }
        public static PropertyValidation PurchaseArticleContainerDetailIsValid(ApplicationDbContext _context, PurchaseArticleContainerDetail model)
        {
            var result = new PropertyValidation(true, "PurchaseArticleContainerDetailIsValid", "PurchaseArticleContainerDetail", "", "");
            if (model.ContainerNumber == "")
            {
                result.Value = false; result.Property = nameof(model.ContainerNumber); result.Message = "please put container number.";
                return result;
            }
            if (model.PurchaseArticleID == 0)
            {
                result.Value = false; result.Property = nameof(model.PurchaseArticleID); result.Message = "Model is not valid."; result.SystemMessage = "PurchaseArticleID is missing.";
                return result;
            }
            if(!PurchaseArticleContainerDetailIsUnique(_context, model))
            {
                result.Value = false; result.Property = nameof(model.ID); result.Message = "Model is not unique."; 
                return result;
            }
            return result;
        }
        public static bool PurchaseArticleContainerDetailIsUnique(ApplicationDbContext _context, PurchaseArticleContainerDetail model)
        {
            var savedPurchaseArticleContainerDetail = _context.PurchaseArticleContainerDetails.Where(d => d.PurchaseArticleID == model.PurchaseArticleID &&
            d.ContainerNumber == model.ContainerNumber).FirstOrDefault();
            if (savedPurchaseArticleContainerDetail != null)
            {
                if (savedPurchaseArticleContainerDetail.ID != model.ID)
                {
                    return false;
                }
            }
            return true;
        }
    }
}
