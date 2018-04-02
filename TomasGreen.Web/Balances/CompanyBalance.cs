using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TomasGreen.Model.Models;
using TomasGreen.Web.Data;

namespace TomasGreen.Web.Balances
{
    public static class CompanyBalance
    {
        public static PropertyValidation Validate(CompanyCreditDebitBalance model)
        {
            var result = new PropertyValidation(true, "Validate", "CompanyCreditDebitBalance", "", "");
            if (model.CompanyID == 0)
            {
                result.Value = false; result.Property = nameof(model.CompanyID); result.Message = "Company undefined.";
                return result;
            }
            if (model.CurrencyID == 0)
            {
                result.Value = false; result.Property = nameof(model.CurrencyID); result.Message = "Currency undefined.";
                return result;
            }
            if (model.Credit == 0 && model.Debit == 0 )
            {
                result.Value = false; result.Property = $"{nameof(model.Credit)} or {nameof(model.Debit)}"; result.Message = "model is not valid.";
                return result;
            }

            return result;
        }
        public static PropertyValidation Add(ApplicationDbContext _context, CompanyCreditDebitBalance model)
        {
            var result = Validate(model);
            try
            {
                if (!result.Value)
                {
                    result.Message += " Couldn't save company balance.";
                    return result;
                }
                var savedBalance = _context.CompanyCreditDebitBalances.Where(b => b.CompanyID == model.CompanyID && b.CurrencyID == model.CurrencyID).FirstOrDefault();
                if (savedBalance == null)
                {
                    _context.Add(model);
                    result.Value = true;
                    return result;

                }
                else
                {
                    savedBalance.Credit += model.Credit;
                    savedBalance.Debit += model.Credit;
                    _context.Update(savedBalance);
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
    }
}
