using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TomasGreen.Model.Models;
using TomasGreen.Web.Data;

namespace TomasGreen.Web.Validations
{
    public static class PackingPlanValidation
    {
        public static PropertyValidatedMessage PackingPlanIsValid(ApplicationDbContext _context, string callingAction, PackingPlan model)
        {
            var result = new PropertyValidatedMessage(true, callingAction, "PackingPlan", "", "");
            if (model.Date == null)
            {
                result.Value = false; result.Property = nameof(model.Date); result.Message = "Please choose a date.";
                return result;
            }
            if (model.ManagerID == 0)
            {
                result.Value = false; result.Property = nameof(model.ManagerID); result.Message = "Please choose a manager.";
                return result;
            }
            if (model.CompanyID == 0)
            {
                result.Value = false; result.Property = nameof(model.CompanyID); result.Message = "Please choose a company.";
                return result;
            }

            return result;
        }

        public static PropertyValidatedMessage PackingPlanMixIsValid(ApplicationDbContext _context, string callingAction, PackingPlanMix model)
        {
            var result = new PropertyValidatedMessage(true, callingAction, "PackingPlanMix", "", "");
            if (model.ID != 0 && model.PackingPlanID == 0)
            {
                result.Value = false; result.Property = nameof(model.PackingPlanID); result.Message = "Please save the packingplan header first.";
                return result;
            }
            if (model.PackagingMaterialPackageID == 0 && model.PackagingMaterialBagID == 0)
            {
                result.Value = false; result.Property = nameof(model.PackagingMaterialBagID); result.Message = "Please choose a packages or bags.";
                return result;
            }
            if (model.Packages == 0 && model.Bags == 0)
            {
                result.Value = false; result.Property = nameof(model.Bags); result.Message = "Please choose a packages or bags.";
                return result;
            }
            if (model.ToWarehouseID == 0)
            {
                result.Value = false; result.Property = nameof(model.ToWarehouseID); result.Message = "Please choose a warehouse.";
                return result;
            }
            if (model.NewArticleID == 0)
            {
                result.Value = false; result.Property = nameof(model.NewArticleID); result.Message = "Please choose an article.";
                return result;
            }
            if (model.NewQtyPackages == 0 && model.NewQtyExtra == 0)
            {
                result.Value = false; result.Property = nameof(model.NewQtyExtra); result.Message = "Please put packages or Extra.";
                return result;
            }
            if (model.PricePerUnit == 0)
            {
                result.Value = false; result.Property = nameof(model.PricePerUnit); result.Message = "Please put price.";
                return result;
            }

            return result;
        }

        public static PropertyValidatedMessage PackingPlanMixArticleIsValid(ApplicationDbContext _context, string callingAction, PackingPlanMixArticle model)
        {
            var result = new PropertyValidatedMessage(true, callingAction, "PackingPlanMixArticle", "", "");
            if (model.PackingPlanMixID == 0)
            {
                result.Value = false; result.Property = nameof(model.PackingPlanMixID); result.Message = "Please save the packingplanMix header first.";
                return result;
            }
            if (model.WarehouseID == 0)
            {
                result.Value = false; result.Property = nameof(model.WarehouseID); result.Message = "Please choose a warehouse.";
                return result;
            }
            if (model.ArticleID == 0)
            {
                result.Value = false; result.Property = nameof(model.ArticleID); result.Message = "Please choose an article.";
                return result;
            }
            if (model.QtyPackages == 0 && model.QtyExtra == 0)
            {
                result.Value = false; result.Property = nameof(model.QtyExtra); result.Message = "Please put packages or bags.";
                return result;
            }
            return result;
        }
    }
}
