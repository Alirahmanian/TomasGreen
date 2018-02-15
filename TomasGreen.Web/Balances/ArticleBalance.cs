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
        public void AddArticle(ReceiveArticleWarehouse model)
        {
            var articleWarehouseBalance = _context.ArticleWarehouseBalances.Where(b => b.ArticleID == model.ReceiveArticle.ArticleID && b.WarehouseID == model.WarehouseID).FirstOrDefault();
            articleWarehouseBalance.QtyBoxesIn += model.QtyBoxes;
            articleWarehouseBalance.QtyExtraKgIn += model.QtyExtraKg;
            if(model.QtyBoxes >= articleWarehouseBalance.QtyBoxesReserved)
            {
                articleWarehouseBalance.QtyBoxesReserved = 0;
            }
            else
            {
                articleWarehouseBalance.QtyBoxesReserved -= model.QtyBoxes;
            }
            _context.Update(articleWarehouseBalance);
            //await _context.SaveChangesAsync();


        }
        public void RemoveArticle(ReceiveArticleWarehouse model)
        {
            var articleWarehouseBalance = _context.ArticleWarehouseBalances.Where(b => b.ArticleID == model.ReceiveArticle.ArticleID && b.WarehouseID == model.WarehouseID).FirstOrDefault();
            articleWarehouseBalance.QtyBoxesIn -= model.QtyBoxes;
            articleWarehouseBalance.QtyExtraKgIn -= model.QtyExtraKg;
            //if (model.QtyBoxes >= articleWarehouseBalance.QtyBoxesReserved)
            //{
            //    articleWarehouseBalance.QtyBoxesReserved = 0;
            //}
            //else
            //{
            //    articleWarehouseBalance.QtyBoxesReserved -= model.QtyBoxes;
            //}


        }

    }
}
