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
        public static PropertyValidatedMessage ReceivArticleIsValid(ApplicationDbContext _context, ReceiveArticle model)
        {
            if (model.Date == null)
            {
                return (new PropertyValidatedMessage(false, "ReceivArticleIsValid", "ReceiveArticle", "Date", "Please choose a date."));
            }
            if (model.ArticleID == 0)
            {
                return (new PropertyValidatedMessage(false, "ReceivArticleIsValid", "ReceiveArticle", "ArticleID", "Please choose an article."));
            }
            
            return (new PropertyValidatedMessage(true, "", "", "", ""));
        }

    }
}
