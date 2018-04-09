using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TomasGreen.Model.Models;
using TomasGreen.Web.Data;

namespace TomasGreen.Web.Balances
{
    public class ArticleBalanceDetail
    {
        public static PropertyValidation Validate(ArticleWarehouseBalanceDetail model, bool checkMinusValue, bool checkForDelete)
        {
            var result = new PropertyValidation(true, "Validate", "ArticleWarehouseBalanceDetail", "", "");
            if (model.ArticleID == 0)
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
                result.Value = false; result.Property = nameof(model.CompanyID); result.Message = "Company undefined.";
                return result;
            }
            if (model.ArticleWarehouseBalanceDetailTypeID == 0)
            {
                result.Value = false; result.Property = nameof(model.ArticleWarehouseBalanceDetailTypeID); result.Message = "Article balance detail type undefined.";
                return result;
            }
            if (model.BalanceChangerID == 0)
            {
                result.Value = false; result.Property = nameof(model.BalanceChangerID); result.Message = "Balance changer undefined.";
                return result;
            }
            if (model.QtyPackages == 0 && model.QtyExtra == 0 && checkForDelete == false)
            {
                result.Value = false; result.Property = $"{nameof(model.QtyPackages)} or {nameof(model.QtyExtra)}"; result.Message = "model is not valid.";
                return result;
            }
            
            return result;
        }
        private static ArticleWarehouseBalanceDetail FindByIndex(ApplicationDbContext _context, ArticleWarehouseBalanceDetail model)
        {
            return _context.ArticleWarehouseBalanceDetails.Where(d => d.ArticleID == model.ArticleID && d.WarehouseID == model.WarehouseID && d.CompanyID == model.CompanyID 
           && d.ArticleWarehouseBalanceDetailTypeID == model.ArticleWarehouseBalanceDetailTypeID &&
           d.BalanceChangerID == model.BalanceChangerID).FirstOrDefault();

        }
        private static ArticleWarehouseBalanceDetail FindById(ApplicationDbContext _context, ArticleWarehouseBalanceDetail model)
        {
            return _context.ArticleWarehouseBalanceDetails.Where(d => d.ID == model.ID).FirstOrDefault();
        }
        public static PropertyValidation AddBalanceDetail(ApplicationDbContext _context, ArticleWarehouseBalanceDetail model)
        {
            var result = Validate(model, false, false);
            try
            {
                if (!result.Value)
                {
                    return result;
                }
                var articleWarehouseBalance = _context.ArticleWarehouseBalances.Where(b => b.ArticleID == model.ArticleID && b.WarehouseID == model.WarehouseID && b.CompanyID == model.CompanyID).FirstOrDefault();
                var savedArtilceWarehouseBalanceDetail = FindByIndex(_context, model);
                if (savedArtilceWarehouseBalanceDetail == null)
                {
                    if (articleWarehouseBalance != null)
                    {
                        model.QtyPackagesOnHandBeforeChange = articleWarehouseBalance.QtyPackagesOnhand;
                        model.QtyExtraOnHandBeforeChange = articleWarehouseBalance.QtyExtraOnhand;
                    }
                    _context.ArticleWarehouseBalanceDetails.Add(model);
                }
                else
                {
                    savedArtilceWarehouseBalanceDetail.QtyPackages = model.QtyPackages;
                    savedArtilceWarehouseBalanceDetail.QtyExtra = model.QtyExtra;
                    if (articleWarehouseBalance != null)
                    {
                        model.QtyPackagesOnHandBeforeChange = articleWarehouseBalance.QtyPackagesOnhand;
                        model.QtyExtraOnHandBeforeChange = articleWarehouseBalance.QtyExtraOnhand;
                    }
                    _context.ArticleWarehouseBalanceDetails.Update(savedArtilceWarehouseBalanceDetail);
                }
            }
            catch (Exception exception)
            {
                result.Value = false; result.Action = "AddBalanceDetail"; result.Property = "Exception"; result.Message = "Unexpected error."; result.SystemMessage = exception.Message.ToString();
                return result;
            }
            result.Value = true;
            return result;

        }
        public static PropertyValidation DeleteBalanceDetail(ApplicationDbContext _context, ArticleWarehouseBalanceDetail model)
        {
            var result = Validate(model, false, true);
            try
            {
                if (!result.Value)
                {
                    return result;
                }
                var savedArticleWarehouseBalanceDetail = FindByIndex(_context, model);
                if (savedArticleWarehouseBalanceDetail == null)
                {
                    result.Value = false; result.Action = "DeleteBalanceDetail"; result.Message = "Couldn't find.";
                    return result;
                }
                else
                {
                    _context.ArticleWarehouseBalanceDetails.Remove(savedArticleWarehouseBalanceDetail);
                }
            }
            catch (Exception exception)
            {
                result.Value = false; result.Action = "DeleteBalanceDetail"; result.Property = "Exception"; result.Message = "Unexpected error."; result.SystemMessage = exception.Message.ToString();
                return result;
            }
            result.Value = true;
            return result;
        }
    }
}
