using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TomasGreen.Model.Models;
using TomasGreen.Web.Data;

namespace TomasGreen.Web.Validations
{
    public static class RoastingPlanValidation
    {
        public static PropertyValidatedMessage RoastingPlanIsValid(ApplicationDbContext _context, string callingAction, RoastingPlan model)
        {
            var result = new PropertyValidatedMessage(true, callingAction, "RoastingPlan", "", "");
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
            if (model.FromWarehouseID == 0)
            {
                result.Value = false; result.Property = nameof(model.FromWarehouseID); result.Message = "Please choose a wharehouse.";
                return result;
            }
            if (model.ArticleID == 0)
            {
                result.Value = false; result.Property = nameof(model.ArticleID); result.Message = "Please choose an article.";
                return result;
            }
            if (model.QtyPackages < 0)
            {
                result.Value = false; result.Property = nameof(model.QtyPackages); result.Message = "Please put a value bigger than zero.";
                return result;
            }
            if (model.QtyExtra < 0)
            {
                result.Value = false; result.Property = nameof(model.QtyExtra); result.Message = "Please put a value bigger than zero.";
                return result;
            }
            if (model.QtyPackages == 0 && model.QtyExtra == 0)
            {
                result.Value = false; result.Property = nameof(model.QtyPackages); result.Message = "Please put a value bigger than zero.";
                return result;
            }
            if (model.PricePerUnit <= 0)
            {
                result.Value = false; result.Property = nameof(model.PricePerUnit); result.Message = "Please put a value bigger than zero.";
                return result;
            }
            if (model.Salt <= 0)
            {
                result.Value = false; result.Property = nameof(model.Salt); result.Message = "Please put a value bigger than zero.";
                return result;
            }
            if (model.ToWarehouseID == 0)
            {
                result.Value = false; result.Property = nameof(model.ToWarehouseID); result.Message = "Please choose a wharehouse.";
                return result;
            }
            if (model.NewArticleID == 0)
            {
                result.Value = false; result.Property = nameof(model.NewArticleID); result.Message = "Please choose an article.";
                return result;
            }
            if (model.NewQtyPackages < 0)
            {
                result.Value = false; result.Property = nameof(model.NewQtyPackages); result.Message = "Please put a value bigger than zero.";
                return result;
            }
            if (model.NewQtyExtra < 0)
            {
                result.Value = false; result.Property = nameof(model.NewQtyExtra); result.Message = "Please put a value bigger than zero.";
                return result;
            }
            if (model.QtyPackages == 0 && model.QtyExtra == 0)
            {
                result.Value = false; result.Property = nameof(model.QtyPackages); result.Message = "Please put a value bigger than zero.";
                return result;
            }
            if(model.ArticleID == model.NewArticleID)
            {
                result.Value = false; result.Property = nameof(model.NewArticleID); result.Message = "Please select different article.";
                return result;
            }
            
            if(model.PackagingMaterialPackageID != 0 && model.PackagingMaterialBagID != 0)
            {
                if(model.PackagingMaterialPackageID == model.PackagingMaterialBagID)
                {
                    result.Value = false; result.Property = nameof(model.PackagingMaterialBagID); result.Message = "Please select different Packaging material.";
                    return result;
                }
            }
            if(model.Packages < 0)
            {
                result.Value = false; result.Property = nameof(model.Packages); result.Message = "Please put a value bigger than zero.";
                return result;
            }
            if (model.Bags < 0)
            {
                result.Value = false; result.Property = nameof(model.Bags); result.Message = "Please put a value bigger than zero.";
                return result;
            }
            if (model.PackagingMaterialPackageID >= 0 && model.Packages == 0)
            {
                result.Value = false; result.Property = nameof(model.Packages); result.Message = "Please put a value bigger than zero.";
                return result;
            }
            if (model.PackagingMaterialBagID >= 0 && model.Bags == 0)
            {
                result.Value = false; result.Property = nameof(model.Bags); result.Message = "Please put a value bigger than zero.";
                return result;
            }
            if(model.Packages > model.NewQtyPackages)
            {
                result.Value = false; result.Property = nameof(model.Packages); result.Message = "Change the number of boxes to match the number of new packages.";
                return result;
            }
            return result;
        }
            
           
        
    }
}
