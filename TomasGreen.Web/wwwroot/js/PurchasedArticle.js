//=======================================
//=======================================
function EnableDisablePurchasedArticleTabs() {
    if ($("#PurchasedArticle_ID").val() === "" || $("#PurchasedArticle_ID").val() === "0") {
        $("#PurchasedArticleTaps").find(":input").prop("disabled", true);
        //$("#PurchasedArticleDetail_Mess").text('Please save order header first.');

    }
    else {
        $("#PurchasedArticleTaps").find(":input").prop("disabled", false);
       
    }
    $("#PurchasedArticleDetail_TotalPerUnit").prop('disabled', true);
}
//=======================================
//=======================================
function ValidatePurchasedArticle() {
    var isPurchasedArticleValid = true;
    if ($('#PurchasedArticle_CompanyID').val() === "0" || $('#PurchasedArticle_CompanyID').val() === "Select") {
        $("#PurchasedArticle_CompanyID").closest('div').append('<p class="validMess" style="color:red">Please choose a company.</p>');
        isPurchasedArticleValid = false;
    }
    else {
        $("#PurchasedArticle_CompanyID").closest('div').closest('.validMess').attr('visibility', 'visible');
    }
    if ($('#PurchasedArticle_CurrencyID').val() === "0" || $('#PurchasedArticle_CurrencyID').val() === "Select") {
        $("#PurchasedArticle_CurrencyID").closest('div').append('<p style="color:red">Please choose a currency.</p>');
        isPurchasedArticleValid = false;
    }
    else {

    }
    return isPurchasedArticleValid;
}
//=======================================
//=======================================
function ValidatePurchasedArticleDetail() {
    var isPurchasedArticleDetailValid = true;
    $(".valid_ArticleID").remove();
    $(".valid_QtyPackages").remove();
    $(".valid_UnitPrice").remove();

    if ($('#PurchasedArticleDetail_ArticleID').val() === "0" || $('#PurchasedArticleDetail_ArticleID').val() === "Select") {
        $("#PurchasedArticleDetail_ArticleID").closest('div').append('<p class="valid_ArticleID" style="color:red">Please choose an article.</p>');
        isPurchasedArticleDetailValid = false;
    }
   
    if (parseInt($('#PurchasedArticleDetail_QtyPackages').val(), 10) === 0 && parseFloat($('#PurchasedArticleDetail_QtyExtra').val(), 10) === 0) {
        $("#PurchasedArticleDetail_QtyPackages").closest('div').append('<p class="valid_QtyPackages" style="color:red">Please put packages or extra.</p>');
        isPurchasedArticleDetailValid = false;
    }
    
    if (parseFloat($('#PurchasedArticleDetail_UnitPrice').val(), 10) === 0) {
        $("#PurchasedArticleDetail_UnitPrice").closest('div').append('<p class="valid_UnitPrice" style="color:red">Please put price.</p>');
        isPurchasedArticleDetailValid = false;
    }
    
    return isPurchasedArticleDetailValid;
}
//=======================================
//=======================================
$(document).ready(
    EnableDisablePurchasedArticleTabs()
   




);