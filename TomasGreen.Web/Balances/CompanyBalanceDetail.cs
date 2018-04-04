using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TomasGreen.Model.Models;
using TomasGreen.Web.Data;


namespace TomasGreen.Web.Balances
{
    public class CompanyBalanceDetail
    {
        public static PropertyValidation Validate(CompanyCreditDebitBalanceDetail model, bool checkMinusValue, bool checkForDelete)
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
            if (model.CompanyCreditDebitBalanceDetailTypeID == 0)
            {
                result.Value = false; result.Property = nameof(model.CompanyCreditDebitBalanceDetailTypeID); result.Message = "Balance detail type undefined.";
                return result;
            }
            if (model.BalanceChangerID == 0)
            {
                result.Value = false; result.Property = nameof(model.BalanceChangerID); result.Message = "Balance changer undefined.";
                return result;
            }
            if (model.Credit == 0 && model.Debit == 0 && checkForDelete == false)
            {
                result.Value = false; result.Property = $"{nameof(model.Credit)} or {nameof(model.Debit)}"; result.Message = "model is not valid.";
                return result;
            }
            if (checkMinusValue)
            {
                if (model.Credit < 0)
                {
                    result.Value = false; result.Action = "Validate"; result.Property = nameof(model.Credit); result.Message = "Value can not be less then zero.";
                    return result;
                }
                if (model.Debit < 0)
                {
                    result.Value = false; result.Action = "Validate"; result.Property = nameof(model.Debit); result.Message = "Value can not be less then zero.";
                    return result;
                }
            }
            return result;
        }
        private static CompanyCreditDebitBalanceDetail FindByIndex(ApplicationDbContext _context, CompanyCreditDebitBalanceDetail model)
        {
            return _context.CompanyCreditDebitBalanceDetails.Where(d => d.CompanyID == model.CompanyID && d.CurrencyID == model.CurrencyID
           && d.CompanyCreditDebitBalanceDetailTypeID == model.CompanyCreditDebitBalanceDetailTypeID &&
           d.BalanceChangerID == model.BalanceChangerID && d.PaymentTypeID == model.PaymentTypeID).FirstOrDefault();

        }
        private static CompanyCreditDebitBalanceDetail FindById(ApplicationDbContext _context, CompanyCreditDebitBalanceDetail model)
        {
            return _context.CompanyCreditDebitBalanceDetails.Where(d => d.ID == model.ID).FirstOrDefault();
        }
        public static PropertyValidation AddBalanceDetail(ApplicationDbContext _context, CompanyCreditDebitBalanceDetail model)
        {
            var result = Validate(model, false, false);
            try
            {
                if (!result.Value)
                {
                    return result;
                }
                var companyBalance = _context.CompanyCreditDebitBalances.Where(b => b.CompanyID == model.CompanyID && b.CurrencyID == model.CurrencyID).FirstOrDefault();
                var savedCompanyBalanceDetail = FindByIndex(_context, model);
                if (savedCompanyBalanceDetail == null)
                {
                    if (companyBalance != null)
                    {
                        model.CreditBeforeChange = companyBalance.Credit;
                        model.DebitBeforeChange = companyBalance.Debit;
                    }
                    _context.CompanyCreditDebitBalanceDetails.Add(model);
                }
                else
                {
                    savedCompanyBalanceDetail.Credit = model.Credit;
                    savedCompanyBalanceDetail.Debit = model.Debit;
                    if (companyBalance != null)
                    {
                        model.CreditBeforeChange = companyBalance.Credit;
                        model.DebitBeforeChange = companyBalance.Debit;
                    }
                    _context.CompanyCreditDebitBalanceDetails.Update(savedCompanyBalanceDetail);
                }
            }
            catch (Exception exception)
            {
                result.Value = false; result.Action = "AddBalanceDetail"; result.Property = "Exception"; result.Message = "Unexpected error."; result.SystemMessage = exception.Message.ToString();
                return result;
            }
            result.Value = true;
            return result;

        }
        public static PropertyValidation DeleteBalanceDetail(ApplicationDbContext _context, CompanyCreditDebitBalanceDetail model)
        {
            var result = Validate(model, false, true);
            try
            {
                if (!result.Value)
                {
                    return result;
                }
                var savedBalanceDetail = FindByIndex(_context, model);
                if (savedBalanceDetail == null)
                {
                    result.Value = false; result.Action = "RemoveBalanceDetail"; result.Message = "Couldn't find.";
                    return result;
                }
                else
                {
                    _context.CompanyCreditDebitBalanceDetails.Remove(savedBalanceDetail);
                }
            }
            catch (Exception exception)
            {
                result.Value = false; result.Action = "RemoveBalanceDetail"; result.Property = "Exception"; result.Message = "Unexpected error."; result.SystemMessage = exception.Message.ToString();
                return result;
            }
            result.Value = true;
            return result;
        }
    }
}
