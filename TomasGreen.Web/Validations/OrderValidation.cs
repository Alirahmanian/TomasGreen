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
        public static PropertyValidatedMessage OrderIsValid(ApplicationDbContext _context, Order model)
        {
            if (model.CompanyID == 0)
            {
                return (new PropertyValidatedMessage(false, "OrderIsValid", "Order", nameof(model.CompanyID), "Please choose a customer."));
            }
            if (model.OrderDate == null)
            {
                return (new PropertyValidatedMessage(false, "ReceivArticleIsValid", "ReceiveArticle", nameof(model.OrderDate), "Please choose an date for order."));
            }

            return (new PropertyValidatedMessage(true, "", "", "", ""));
        }
        public static PropertyValidatedMessage OrderDetailIsValid(ApplicationDbContext _context, OrderDetail model)
        {
            if (model.OrderID == 0)
            {
                return (new PropertyValidatedMessage(false, "OrderDetailIsValid", "OrderDetail", nameof(model.OrderID), "Order id is missing."));
            }
            if (model.ArticleID == 0)
            {
                return (new PropertyValidatedMessage(false, "OrderDetailIsValid", "OrderDetail", nameof(model.ArticleID), "Please choose an article."));
            }
            if (model.WarehouseID == 0)
            {
                return (new PropertyValidatedMessage(false, "OrderDetailIsValid", "OrderDetail", nameof(model.WarehouseID), "Please choose a warehouse."));
            }
            if(model.QtyBoxes < 0 )
            {
                return (new PropertyValidatedMessage(false, "OrderDetailIsValid", "OrderDetail", nameof(model.QtyBoxes), "Please put a positive value."));
            }
            if (model.QtyReserveBoxes < 0)
            {
                return (new PropertyValidatedMessage(false, "OrderDetailIsValid", "OrderDetail", nameof(model.QtyReserveBoxes), "Please put a positive value."));
            }
            if (model.QtyKg < 0)
            {
                return (new PropertyValidatedMessage(false, "OrderDetailIsValid", "OrderDetail", nameof(model.QtyKg), "Please put a positive value."));
            }
            if (model.Price <= 0)
            {
                return (new PropertyValidatedMessage(false, "OrderDetailIsValid", "OrderDetail", nameof(model.Price), "Please put a price."));
            }
            if(OrderDetailITotalWeight(_context, model) <=0)
            {
                return (new PropertyValidatedMessage(false, "OrderDetailIsValid", "OrderDetail", "", "Please put boxes, reserve or extra kg."));

            }

            var unique =  OrderDetailIsUnique(_context, model);
            if(!unique.Value)
            {
                return unique;

            }

            return (new PropertyValidatedMessage(true, "", "", "", ""));
        }

        public static PropertyValidatedMessage OrderDetailIsUnique(ApplicationDbContext _context, OrderDetail model)
        {
            var orderDetails = _context.OrderDetails.Where(d => d.OrderID == model.OrderID);
            foreach(var orderDetail in orderDetails)
            {
                if(orderDetail.ID != model.ID)
                {
                    if(orderDetail.ArticleID == model.ArticleID && orderDetail.WarehouseID == model.WarehouseID && orderDetail.Price == model.Price)
                    {
                        return (new PropertyValidatedMessage(false, "OrderDetailIsUnique", "OrderDetail", "", "There is already an order row with the same article, warehouse and price."));
                    }
                }
            }
            return (new PropertyValidatedMessage(true, "", "", "", ""));
        }

        public static decimal OrderDetailITotalWeight(ApplicationDbContext _context, OrderDetail model)
        {
            var articleBoxWeight = _context.Articles.Where(a => a.ID == model.ArticleID).FirstOrDefault()?.BoxWeight ?? 0;
            var totalWeight = (model.QtyBoxes * articleBoxWeight) + (model.QtyReserveBoxes * articleBoxWeight) + model.QtyKg;
            return totalWeight;
        }

    }
}
