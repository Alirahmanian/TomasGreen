﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TomasGreen.Model.Models;
using TomasGreen.Web.Data;

namespace TomasGreen.Web.Validations
{
    public static class PurchasedArticleValidation 
    {
        public static PropertyValidation PurchasedArticleIsValid(ApplicationDbContext _context, PurchasedArticle model)
        {
            var result = new PropertyValidation(true, "AddPurchasedArticleToBalance", "ArticleWarehouseBalance", "", "");
            if (model.Date == null)
            {
                result.Value = false; result.Property = nameof(model.Date); result.Message = "Please choose a date.";
                return result;
            }
           
            if(model.CompanyID == 0)
            {
                result.Value = false; result.Property = nameof(model.CompanyID); result.Message = "Please choose a supplier.";
                return result;
            }
            
            return result;
        }

    }
}
