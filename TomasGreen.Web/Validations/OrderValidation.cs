using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TomasGreen.Model.Models;
using TomasGreen.Web.Data;

namespace TomasGreen.Web.Validations
{
    public static class OrderValidation
    {
        public static PropertyValidation OrderIsValid(ApplicationDbContext _context, Order model)
        {
            if (model.CompanyID == 0)
            {
                return (new PropertyValidation(false, "OrderIsValid", "Order", nameof(model.CompanyID), "Please choose a customer."));
            }
            if (model.OrderDate == null)
            {
                return (new PropertyValidation(false, "ReceivArticleIsValid", "ReceiveArticle", nameof(model.OrderDate), "Please choose an date for order."));
            }

            return (new PropertyValidation(true, "", "", "", ""));
        }
        public static PropertyValidation OrderDetailIsValid(ApplicationDbContext _context, OrderDetail model)
        {
            var result = new PropertyValidation(true, "OrderDetailIsValid", "OrderDetail", "", "");
            if (model.OrderID == 0)
            {
                result.Value = false; result.Property = nameof(model.OrderID); result.Message = "Order id is missing.";
                return result;
            }
            if (model.ArticleID == 0)
            {
                result.Value = false; result.Property = nameof(model.ArticleID); result.Message = "Please choose an article.";
                return result;
            }
            if (model.WarehouseID == 0)
            {
                result.Value = false; result.Property = nameof(model.WarehouseID); result.Message = "Please choose an warehouse.";
                return result;
            }
            if(model.QtyPackages < 0 )
            {
                result.Value = false; result.Property = nameof(model.QtyPackages); result.Message = "Please put a positive value.";
                return result;
            }
            if (model.QtyExtra < 0)
            {
                result.Value = false; result.Property = nameof(model.QtyExtra); result.Message = "Please put a positive value.";
                return result;
            }
            if(model.QtyPackages  == 0 && model.QtyExtra == 0)
            {
                result.Value = false; result.Property = nameof(model.QtyExtra); result.Message = "Please put value for packages or Extra.";
                return result;
            }
            if (model.Price <= 0)
            {
                result.Value = false; result.Property = nameof(model.Price); result.Message = "Please put a positive value.";
                return result;
            }
            if(OrderDetailTotalWeightOrPackage(_context, model) <= 0)
            {
                result.Value = false; result.Property = nameof(model.Extended_Price); result.Message = "Please put value for packages or Extra.";
                return result;

            }

            var unique =  OrderDetailIsUnique(_context, model);
            if(!unique.Value)
            {
                return unique;

            }

            return (new PropertyValidation(true, "", "", "", ""));
        }

        public static PropertyValidation OrderDetailIsUnique(ApplicationDbContext _context, OrderDetail model)
        {
            var orderDetails = _context.OrderDetails.Where(d => d.OrderID == model.OrderID);
            foreach(var orderDetail in orderDetails)
            {
                if(orderDetail.ID != model.ID)
                {
                    if(orderDetail.ArticleID == model.ArticleID && orderDetail.WarehouseID == model.WarehouseID && orderDetail.Price == model.Price)
                    {
                        return (new PropertyValidation(false, "OrderDetailIsUnique", "OrderDetail", "", "There is already an order row with the same article, warehouse and price."));
                    }
                }
            }
            return (new PropertyValidation(true, "", "", "", ""));
        }

        public static ArticleUnit GetArticleUnitByArticle(ApplicationDbContext _context, Article article)
        {
            var unit = _context.ArticleUnits.Where(u => u.ID == article.ArticleUnitID).FirstOrDefault();
            return unit;
        }
        public static bool CheckArticleUnitMeasuresByKG(ApplicationDbContext _context, Article article)
        {
            var unit = _context.ArticleUnits.Where(u => u.ID == article.ArticleUnitID).FirstOrDefault();
            if(unit != null)
            {
                if (unit.MeasuresByKg)
                    return true;
            }
            return false;
        }
        public static decimal OrderDetailTotalWeightOrPackage(ApplicationDbContext _context, OrderDetail model)
        {
             
            var article = _context.Articles.Where(a => a.ID == model.ArticleID).FirstOrDefault();
            if(article != null)
            {
                var isPerKg = CheckArticleUnitMeasuresByKG(_context, article);
                if (isPerKg)
                {
                   return ((model.QtyPackages * article.WeightPerPackage) + model.QtyExtra);
                }
                else
                {
                    return (model.QtyPackages);
                }
            }
            
            return 0;
        }

    }
}
