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
                    return (new PropertyValidatedMessage(true, "", "", "", ""));

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
                    return (new PropertyValidatedMessage(true, "", "", "", ""));
                }
            }
            catch (Exception exception)
            {
                return (new PropertyValidatedMessage(false, "AddReceiveArticleToBalance", "ArticleWarehouseBalance", "Exception", exception.Message.ToString()));
            }
            return (new PropertyValidatedMessage(true, "", "", "", ""));
        }

        public PropertyValidatedMessage RemoveReceiveArticleFromBalance(ReceiveArticleWarehouse model)
        {
            try
            {
                var articleWarehouseBalance = _context.ArticleWarehouseBalances.Where(b => b.ArticleID == model.ReceiveArticle.ArticleID && b.WarehouseID == model.WarehouseID).FirstOrDefault();
                articleWarehouseBalance.QtyBoxesIn -= model.QtyBoxes;
                if (articleWarehouseBalance.QtyBoxesIn < 0)
                {
                    return (new PropertyValidatedMessage(false, "RemoveReceiveArticleFromBalance", "ArticleWarehouseBalance", nameof(articleWarehouseBalance.QtyBoxesIn), "Value can not be less then zero."));
                }
                articleWarehouseBalance.QtyExtraKgIn -= model.QtyExtraKg;
                if (articleWarehouseBalance.QtyExtraKgIn < 0)
                {
                    return (new PropertyValidatedMessage(false, "RemoveReceiveArticleFromBalance", "ArticleWarehouseBalance", nameof(articleWarehouseBalance.QtyExtraKgIn), "Value can not be less then zero."));
                }
                
                articleWarehouseBalance.QtyBoxesOnhand -= model.QtyBoxes;
                if (articleWarehouseBalance.QtyBoxesOnhand < 0)
                {
                    return (new PropertyValidatedMessage(false, "RemoveReceiveArticleFromBalance", "ArticleWarehouseBalance", nameof(articleWarehouseBalance.QtyBoxesOnhand), "Value can not be less then zero."));
                }
                articleWarehouseBalance.QtyExtraKgOnhand -= model.QtyExtraKg;
                if (articleWarehouseBalance.QtyExtraKgOnhand < 0)
                {
                    return (new PropertyValidatedMessage(false, "RemoveReceiveArticleFromBalance", "ArticleWarehouseBalance", nameof(articleWarehouseBalance.QtyExtraKgOnhand), "Value can not be less then zero."));
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
                return (new PropertyValidatedMessage(false, "", "", "Exception", exception.Message.ToString()));
            }

            
            return(new PropertyValidatedMessage(true, "", "", "", ""));
        }

        public PropertyValidatedMessage AddOrderDetailToBalance(OrderDetail model)
        {
            try
            {
                var articleWarehouseBalance = _context.ArticleWarehouseBalances.Where(b => b.ArticleID == model.ArticleID && b.WarehouseID == model.WarehouseID).FirstOrDefault();
               // if(articleWarehouseBalance)

            }
            catch (Exception exception)
            {
                return (new PropertyValidatedMessage(false, "", "", "Exception", exception.Message.ToString()));
            }

            return (new PropertyValidatedMessage(true, "", "", "", ""));
        }

        public PropertyValidatedMessage RemoveOrderDetailFromBalance(OrderDetail model)
        {
            return (new PropertyValidatedMessage(true, "", "", "", ""));
        }

    }
}
