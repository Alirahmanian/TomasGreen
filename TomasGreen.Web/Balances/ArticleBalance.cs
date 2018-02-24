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
        public PropertyValidatedMessage AddReceiveArticleToBalance(ReceiveArticleWarehouse model)
        {
            var result = new PropertyValidatedMessage(true, "AddReceiveArticleToBalance", "ArticleWarehouseBalance", "", "");
            try
            {
                var articleWarehouseBalance = _context.ArticleWarehouseBalances.Where(b => b.ArticleID == model.ReceiveArticle.ArticleID && b.WarehouseID == model.WarehouseID).FirstOrDefault();
                if (articleWarehouseBalance == null)
                {
                    var article = _context.Articles.Where(a => a.ID == model.ReceiveArticle.ArticleID).FirstOrDefault();

                    var newArticleWarehouseBalance = new ArticleWarehouseBalance();
                    newArticleWarehouseBalance.Article = article;
                    newArticleWarehouseBalance.Warehouse = _context.Warehouses.Where(w => w.ID == model.WarehouseID).FirstOrDefault();
                    newArticleWarehouseBalance.ArticleID = model.ReceiveArticle.ArticleID;
                    newArticleWarehouseBalance.WarehouseID = model.WarehouseID;
                    newArticleWarehouseBalance.QtyBoxesIn = model.QtyBoxes;
                    newArticleWarehouseBalance.QtyExtraKgIn = model.QtyExtraKg;
                    newArticleWarehouseBalance.QtyBoxesOut = 0;
                    newArticleWarehouseBalance.QtyExtraKgOut = 0;
                    newArticleWarehouseBalance.QtyBoxesReserved = 0;
                    newArticleWarehouseBalance.QtyBoxesOnhand = model.QtyBoxes;
                    newArticleWarehouseBalance.QtyExtraKgOnhand = model.QtyExtraKg;
                    _context.Add(newArticleWarehouseBalance);
                    //await _context.SaveChangesAsync();
                    result.Value = true;
                    return result;

                }
                else
                {
                    articleWarehouseBalance.QtyBoxesIn += model.QtyBoxes;
                    articleWarehouseBalance.QtyExtraKgIn += model.QtyExtraKg;
                    articleWarehouseBalance.QtyExtraKgOnhand += model.QtyExtraKg;
                    if (articleWarehouseBalance.QtyBoxesReserved > 0)
                    {
                        if (model.QtyBoxes >= articleWarehouseBalance.QtyBoxesReserved)
                        {
                            var QtyBoxesRest = model.QtyBoxes - articleWarehouseBalance.QtyBoxesReserved;

                            articleWarehouseBalance.QtyBoxesReserved = 0;
                            articleWarehouseBalance.QtyBoxesOnhand += QtyBoxesRest;
                        }
                        else
                        {
                            articleWarehouseBalance.QtyBoxesReserved -= model.QtyBoxes;
                        }
                    }
                    else
                    {
                        articleWarehouseBalance.QtyBoxesOnhand += model.QtyBoxes;
                    }
                    
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

        public PropertyValidatedMessage RemoveReceiveArticleFromBalance(ReceiveArticleWarehouse model)
        {
            var result = new PropertyValidatedMessage(true, "RemoveReceiveArticleFromBalance", "ArticleWarehouseBalance", "", "");
            try
            {
                var articleWarehouseBalance = _context.ArticleWarehouseBalances.Where(b => b.ArticleID == model.ReceiveArticle.ArticleID && b.WarehouseID == model.WarehouseID).FirstOrDefault();
                articleWarehouseBalance.QtyBoxesIn -= model.QtyBoxes;
                if (articleWarehouseBalance.QtyBoxesIn < 0)
                {
                    result.Value = false; result.Property = nameof(articleWarehouseBalance.QtyBoxesIn); result.Message = "Value can not be less then zero.";
                    return result;
                }
                articleWarehouseBalance.QtyExtraKgIn -= model.QtyExtraKg;
                if (articleWarehouseBalance.QtyExtraKgIn < 0)
                {
                    result.Value = false; result.Property = nameof(articleWarehouseBalance.QtyExtraKgIn); result.Message = "Value can not be less then zero.";
                    return result;
                }
                
                articleWarehouseBalance.QtyBoxesOnhand -= model.QtyBoxes;
                if (articleWarehouseBalance.QtyBoxesOnhand < 0)
                {
                    result.Value = false; result.Property = nameof(articleWarehouseBalance.QtyBoxesOnhand); result.Message = "Value can not be less then zero.";
                    return result;
                }
                articleWarehouseBalance.QtyExtraKgOnhand -= model.QtyExtraKg;
                if (articleWarehouseBalance.QtyExtraKgOnhand < 0)
                {
                    result.Value = false; result.Property = nameof(articleWarehouseBalance.QtyExtraKgOnhand); result.Message = "Value can not be less then zero.";
                    return result;
                }

                
                //if (articleWarehouseBalance.QtyBoxesOnhand < 0)
                //{
                //    articleWarehouseBalance.QtyBoxesReserved += (articleWarehouseBalance.QtyBoxesOnhand * -1);
                //    articleWarehouseBalance.QtyBoxesOnhand = 0;
                //}
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
                articleWarehouseBalance.QtyBoxesOut += model.QtyBoxes;
                articleWarehouseBalance.QtyExtraKgOut += model.QtyKg;
                articleWarehouseBalance.QtyBoxesReserved += model.QtyReserveBoxes;
                articleWarehouseBalance.QtyBoxesOnhand -= model.QtyBoxes;
                if(articleWarehouseBalance.QtyBoxesOnhand < 0)
                {
                    result.Value = false; result.Property = nameof(articleWarehouseBalance.QtyBoxesOnhand); result.Message = "Value can not be less then zero.";
                    return result;
                }
                articleWarehouseBalance.QtyExtraKgOnhand -= model.QtyKg;
                if(articleWarehouseBalance.QtyExtraKgOnhand < 0)
                {
                    result.Value = false; result.Property = nameof(articleWarehouseBalance.QtyExtraKgOnhand); result.Message = "Value can not be less then zero.";
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
                // What to do in case model has reserve?
                articleWarehouseBalance.QtyBoxesReserved -= model.QtyReserveBoxes;
                if (articleWarehouseBalance.QtyBoxesReserved < 0)
                {
                    result.Value = false; result.Property = nameof(articleWarehouseBalance.QtyBoxesReserved); result.Message = "Value can not be less then zero.";
                    return result;
                }
                articleWarehouseBalance.QtyBoxesOut -= model.QtyBoxes;
                if(articleWarehouseBalance.QtyBoxesOut < 0)
                {
                    result.Value = false; result.Property = nameof(articleWarehouseBalance.QtyBoxesOut); result.Message = "Value can not be less then zero.";
                    return result;
                }
                articleWarehouseBalance.QtyExtraKgOut -= model.QtyKg;
                if(articleWarehouseBalance.QtyExtraKgOut < 0)
                {
                    result.Value = false; result.Property = nameof(articleWarehouseBalance.QtyExtraKgOut); result.Message = "Value can not be less then zero.";
                    return result;
                }

                articleWarehouseBalance.QtyBoxesOnhand += model.QtyBoxes;
                articleWarehouseBalance.QtyExtraKgOnhand += model.QtyKg;

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
