using System;
using System.Collections.Generic;
using System.Text;
using TomasGreen.Model.Models;
using TomasGreen.Web.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq;




namespace TomasGreen.Web.Balances
{
    public class ArticleBalance
    {
        private readonly ApplicationDbContext _context;

        public ArticleBalance(ApplicationDbContext context)
        {
            _context = context;
        }
        public void AddReceiveArticleToBalance(ReceiveArticleWarehouse model)
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
                    _context.Add(newArticleWarehouseBalance);
                    //await _context.SaveChangesAsync();

                }
                else
                {
                    articleWarehouseBalance.QtyBoxesIn += model.QtyBoxes;
                    articleWarehouseBalance.QtyExtraKgIn += model.QtyExtraKg;
                    if(articleWarehouseBalance.QtyBoxesReserved > 0)
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

            }
        }
        public  void RemoveReceiveArticleFromBalance(ReceiveArticleWarehouse model)
        {
            var articleWarehouseBalance = _context.ArticleWarehouseBalances.Where(b => b.ArticleID == model.ReceiveArticle.ArticleID && b.WarehouseID == model.WarehouseID).FirstOrDefault();
            articleWarehouseBalance.QtyBoxesIn -= model.QtyBoxes;
            articleWarehouseBalance.QtyExtraKgIn -= model.QtyExtraKg;

            articleWarehouseBalance.QtyBoxesOnhand -= model.QtyBoxes;
            if (articleWarehouseBalance.QtyBoxesOnhand < 0)
            {
                articleWarehouseBalance.QtyBoxesReserved += (articleWarehouseBalance.QtyBoxesOnhand * -1);
                articleWarehouseBalance.QtyBoxesOnhand = 0;
            }
            _context.Update(articleWarehouseBalance);
          //  await _context.SaveChangesAsync();
        }

    }
}
