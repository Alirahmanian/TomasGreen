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
    public static class OnthewayArticlesBalance
    {

        public static List<PurchasedArticleWarehouse> GetOnthewayArticles(ApplicationDbContext _context)
        {
            var list = _context.PurchasedArticleWarehouses.Where(a => a.ArrivedDate == null).Include(p => p.PurchasedArticle).ThenInclude(pa => pa.Article).Include(w => w.Warehouse).OrderBy(a => a.PurchasedArticle.Date).ThenBy(a => a.PurchasedArticle.Article).ToList();
            return list;
            
        }
    }
}
