using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TomasGreen.Model.Models;
using TomasGreen.Web.Data;

namespace TomasGreen.Web.Helpers
{
    public static class ArticleHelper
    {
        public static decimal TotalPerUnit(ApplicationDbContext _context, int id, int QtyPackages, decimal QtyExtra)
        {
            var article = _context.Articles.Where(a => a.ID == id).FirstOrDefault();
            if (article != null)
            {
                var isPerKg = CheckArticleUnitMeasuresByKG(_context, article);
                if (isPerKg)
                {
                    return ((QtyPackages * article.WeightPerPackage) + QtyExtra);
                }
                else
                {
                    return (QtyPackages);
                }
            }

            return 0;
        }

        public static bool CheckArticleUnitMeasuresByKG(ApplicationDbContext _context, Article article)
        {
            var unit = _context.ArticleUnits.Where(u => u.ID == article.ArticleUnitID).FirstOrDefault();
            if (unit != null)
            {
                if (unit.MeasuresByKg)
                    return true;
            }
            return false;
        }
    }
}
