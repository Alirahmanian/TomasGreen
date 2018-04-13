﻿//=======================================
//=======================================
function EnableDisablePurchasedArticleTabs() {
    if ($("#PurchasedArticle_ID").val() === "" || $("#PurchasedArticle_ID").val() === "0") {
        $('#PurchasedArticleTabs').hide();
        //$("#PurchasedArticleTabs").find(".tab").prop("disabled", true);
        //$("#PurchasedArticleDetail_Mess").text('Please save order header first.');

    }
    else {
        $('#PurchasedArticleTabs').show();
        //$("#PurchasedArticleTabs").find(".tab").prop("disabled", false);
        PutActiveTab();
        $("#PurchasedArticleCostDetail_PaymentTypeID  option").filter(function () {
            return $.trim($(this).text()) == 'Shortage'
        }).remove(); 
    }
   
}
//=======================================
//=======================================
function ChangeActiveTab(index) {
    $("#ActiveTab").val(index);
    PutActiveTab();
}
//=======================================
//=======================================
function PutActiveTab() {
    if ($("#ActiveTab").val() !== "") {
        $('.PurchasedArticle_Taps li').removeClass('active');
        $('.tab-pane').removeClass('active show');
        $('.PurchasedArticle_Taps li').eq($("#ActiveTab").val()).addClass('active');
        $('.tab-pane').eq($("#ActiveTab").val()).addClass('active');
    }
    else {
        $('.PurchasedArticle_Taps li').removeClass('active');
        $('.tab-pane').removeClass('active, show');
        $('.PurchasedArticle_Taps li').eq(0).addClass('active');
        $('.tab-pane').eq(0).addClass('active');
    }

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
    
    return isPurchasedArticleValid;
}
//=======================================
//=======================================
function ValidatePurchasedArticleDetail() {
    var isPurchasedArticleDetailValid = true;
    $(".valid_WarehouseID").remove();
    $(".valid_ArticleID").remove();
    $(".valid_QtyPackages").remove();
    $(".valid_UnitPrice").remove();

    if ($('#PurchasedArticleDetail_WarehouseID').val() === "0" || $('#PurchasedArticleDetail_WarehouseID').val() === "Select") {
        $("#PurchasedArticleDetail_WarehouseID").closest('div').append('<p class="valid_WarehouseID" style="color:red">Please choose a warehouse.</p>');
        isPurchasedArticleDetailValid = false;
    }

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
function ValidatePurchasedArticleCostDetail() {
    var isPurchasedArticleCostDetailValid = true;
    $(".valid_PaymentTypeID").remove();
    $(".valid_CompanyID").remove();
    $(".valid_CurrencyID").remove();
    $(".valid_Amount").remove();
    
    
    if ($('#PurchasedArticleCostDetail_PaymentTypeID').val() === "0" || $('#PurchasedArticleCostDetail_PaymentTypeID').val() === "Select") {
        $("#PurchasedArticleCostDetail_PaymentTypeID").closest('div').append('<p class="valid_PaymentID" style="color:red">Please choose a payment type.</p>');
        isPurchasedArticleCostDetailValid = false;
    }
    if ($('#PurchasedArticleCostDetail_CompanyID').val() === "0" || $('#PurchasedArticleCostDetail_CompanyID').val() === "Select") {
        $("#PurchasedArticleCostDetail_CompanyID").closest('div').append('<p class="valid_CompanyID" style="color:red">Please choose a company.</p>');
        isPurchasedArticleCostDetailValid = false;
    }
    if ($('#PurchasedArticleCostDetail_CurrencyID').val() === "0" || $('#PurchasedArticleCostDetail_CurrencyID').val() === "Select") {
        $("#PurchasedArticleCostDetail_CurrencyID").closest('div').append('<p class="valid_CurrencyID" style="color:red">Please choose a currency.</p>');
        isPurchasedArticleCostDetailValid = false;
    }
    if (parseInt($('#PurchasedArticleCostDetail_Amount').val(), 10) === 0 ) {
        $("#PurchasedArticleCostDetail_Amount").closest('div').append('<p class="valid_Amount" style="color:red">Please put amount.</p>');
        isPurchasedArticleCostDetailValid = false;
    }

    

    return isPurchasedArticleCostDetailValid;
}
//=======================================
//=======================================
$(document).ready(
    EnableDisablePurchasedArticleTabs()

);