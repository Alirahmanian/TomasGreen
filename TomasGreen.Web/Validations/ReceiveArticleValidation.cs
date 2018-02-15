using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TomasGreen.Model.Models;
using TomasGreen.Web.Data;

namespace TomasGreen.Web.Validations
{
    public static class ReceiveArticleValidation 
    {
        public static ColumValidatedMessage ReceivArticleIsValid(ApplicationDbContext _context, ReceiveArticle model)
        {
            if (model.Date == null)
            {
                return (new ColumValidatedMessage(false, "Date", "Please choose a date."));
            }
            if (model.ArticleID == 0)
            {
                return (new ColumValidatedMessage(false, "Article", "Please choose an article."));
            }
            
            return (new ColumValidatedMessage(true, "", ""));
        }

    }
}
