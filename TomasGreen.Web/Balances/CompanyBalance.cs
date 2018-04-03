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
        public static PropertyValidation Validate(CompanyBalanceDetail model, bool checkMinusValue)
        {
            var result = new PropertyValidation(true, "Validate", "CompanyBalanceDetail", "", "");
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
            if (model.CompanyBalanceDetailTypeID == 0)
            {
                result.Value = false; result.Property = nameof(model.CompanyBalanceDetailTypeID); result.Message = "Balance detail type undefined.";
                return result;
            }
            if (model.BalanceChangerID == 0)
            {
                result.Value = false; result.Property = nameof(model.BalanceChangerID); result.Message = "Balance changer undefined.";
                return result;
            }
            if (model.Credit == 0 && model.Debit == 0)
            {
                result.Value = false; result.Property = $"{nameof(model.Credit)} or {nameof(model.Debit)}"; result.Message = "model is not valid.";
                return result;
            }
            if(checkMinusValue)
            {
                if (model.Credit < 0)
                {
                    result.Value = false; result.Action = "Add"; result.Property = nameof(model.Credit); result.Message = "Value can not be less then zero.";
                    return result;
                }
                if (model.Debit < 0)
                {
                    result.Value = false; result.Action = "Add"; result.Property = nameof(model.Debit); result.Message = "Value can not be less then zero.";
                    return result;
                }
            }
            

            return result;
        }
        private static bool IsUnique(ApplicationDbContext _context, CompanyBalanceDetail model)
        {
            return _context.CompanyBalanceDetails.Any(d => d.ID != model.ID && d.CompanyID == model.CompanyID && d.CurrencyID == model.CurrencyID 
            && d.CompanyBalanceDetailTypeID == model.CompanyBalanceDetailTypeID && 
            d.BalanceChangerID == model.BalanceChangerID && d.PaymentTypeID == model.PaymentTypeID);
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
                    savedBalance.Debit += model.Debit;
                   
                    _context.Update(savedBalance);
                }
            }
            catch (Exception exception)
            {
                result.Value = false; result.Action = "Add";  result.Property = "Exception"; result.Message = ""; result.SystemMessage= exception.Message.ToString();
                return result;
            }
            result.Value = true;
            return result;
        }
        public static PropertyValidation AddBalanceDetail(ApplicationDbContext _context, CompanyBalanceDetail model)
        {
            var result = Validate(model, false);
            try
            {

            }
            catch (Exception exception)
            {
                result.Value = false; result.Action = "AddBalanceDetail";  result.Property = "Exception"; result.Message = ""; result.SystemMessage = exception.Message.ToString();
                return result;
            }
            result.Value = true;
            return result;

        }
        public static PropertyValidation RemoveBalanceDetail(ApplicationDbContext _context, CompanyBalanceDetail model)
        {
            var result = Validate(model,false);
            try
            {

            }
            catch (Exception exception)
            {
                result.Value = false; result.Action = "RemoveBalanceDetail";  result.Property = "Exception"; result.Message = ""; result.SystemMessage = exception.Message.ToString();
                return result;
            }
            result.Value = true;
            return result;
        }
    }
}
