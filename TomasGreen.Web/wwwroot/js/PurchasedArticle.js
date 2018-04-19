//=======================================
//=======================================
function EnableDisablePurchaseArticleTabs() {
    if ($("#PurchaseArticle_ID").val() === "" || $("#PurchaseArticle_ID").val() === "0") {
        $('#PurchaseArticleTabs').hide();
        //$("#PurchaseArticleTabs").find(".tab").prop("disabled", true);
        //$("#PurchaseArticleDetail_Mess").text('Please save order header first.');

    }
    else {
        $('#PurchaseArticleTabs').show();
        //$("#PurchaseArticleTabs").find(".tab").prop("disabled", false);
        PutActiveTab();
        $("#PurchaseArticleCostDetail_PaymentTypeID  option").filter(function () {
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
        $('.PurchaseArticle_Taps li').removeClass('active');
        $('.tab-pane').removeClass('active show');
        $('.PurchaseArticle_Taps li').eq($("#ActiveTab").val()).addClass('active');
        $('.tab-pane').eq($("#ActiveTab").val()).addClass('active');
    }
    else {
        $('.PurchaseArticle_Taps li').removeClass('active');
        $('.tab-pane').removeClass('active, show');
        $('.PurchaseArticle_Taps li').eq(0).addClass('active');
        $('.tab-pane').eq(0).addClass('active');
    }

}
//=======================================
//=======================================
function ValidatePurchaseArticle() {
    var isPurchaseArticleValid = true;
    if ($('#PurchaseArticle_CompanyID').val() === "0" || $('#PurchaseArticle_CompanyID').val() === "Select") {
        $("#PurchaseArticle_CompanyID").closest('div').append('<p class="validMess" style="color:red">Please choose a company.</p>');
        isPurchaseArticleValid = false;
    }
    else {
        $("#PurchaseArticle_CompanyID").closest('div').closest('.validMess').attr('visibility', 'visible');
    }
    if ($('#PurchaseArticle_CurrencyID').val() === "0" || $('#PurchaseArticle_CurrencyID').val() === "Select") {
        $("#PurchaseArticle_CurrencyID").closest('div').append('<p style="color:red">Please choose a currency.</p>');
        isPurchaseArticleValid = false;
    }
    
    return isPurchaseArticleValid;
}
//=======================================
//=======================================
function ValidatePurchaseArticleDetail() {
    var isPurchaseArticleDetailValid = true;
    $(".valid_WarehouseID").remove();
    $(".valid_ArticleID").remove();
    $(".valid_QtyPackages").remove();
    $(".valid_UnitPrice").remove();

    if ($('#PurchaseArticleDetail_WarehouseID').val() === "0" || $('#PurchaseArticleDetail_WarehouseID').val() === "Select") {
        $("#PurchaseArticleDetail_WarehouseID").closest('div').append('<p class="valid_WarehouseID" style="color:red">Please choose a warehouse.</p>');
        isPurchaseArticleDetailValid = false;
    }

    if ($('#PurchaseArticleDetail_ArticleID').val() === "0" || $('#PurchaseArticleDetail_ArticleID').val() === "Select") {
        $("#PurchaseArticleDetail_ArticleID").closest('div').append('<p class="valid_ArticleID" style="color:red">Please choose an article.</p>');
        isPurchaseArticleDetailValid = false;
    }
   
    if (parseInt($('#PurchaseArticleDetail_QtyPackages').val(), 10) === 0 && parseFloat($('#PurchaseArticleDetail_QtyExtra').val(), 10) === 0) {
        $("#PurchaseArticleDetail_QtyPackages").closest('div').append('<p class="valid_QtyPackages" style="color:red">Please put packages or extra.</p>');
        isPurchaseArticleDetailValid = false;
    }
    
    if (parseFloat($('#PurchaseArticleDetail_UnitPrice').val(), 10) === 0) {
        $("#PurchaseArticleDetail_UnitPrice").closest('div').append('<p class="valid_UnitPrice" style="color:red">Please put price.</p>');
        isPurchaseArticleDetailValid = false;
    }
    
    return isPurchaseArticleDetailValid;
}
//=======================================
//=======================================
function ValidatePurchaseArticleCostDetail() {
    var isPurchaseArticleCostDetailValid = true;
    $(".valid_PaymentTypeID").remove();
    $(".valid_CompanyID").remove();
    $(".valid_CurrencyID").remove();
    $(".valid_Amount").remove();
    
    
    if ($('#PurchaseArticleCostDetail_PaymentTypeID').val() === "0" || $('#PurchaseArticleCostDetail_PaymentTypeID').val() === "Select") {
        $("#PurchaseArticleCostDetail_PaymentTypeID").closest('div').append('<p class="valid_PaymentID" style="color:red">Please choose a payment type.</p>');
        isPurchaseArticleCostDetailValid = false;
    }
    if ($('#PurchaseArticleCostDetail_CompanyID').val() === "0" || $('#PurchaseArticleCostDetail_CompanyID').val() === "Select") {
        $("#PurchaseArticleCostDetail_CompanyID").closest('div').append('<p class="valid_CompanyID" style="color:red">Please choose a company.</p>');
        isPurchaseArticleCostDetailValid = false;
    }
    if ($('#PurchaseArticleCostDetail_CurrencyID').val() === "0" || $('#PurchaseArticleCostDetail_CurrencyID').val() === "Select") {
        $("#PurchaseArticleCostDetail_CurrencyID").closest('div').append('<p class="valid_CurrencyID" style="color:red">Please choose a currency.</p>');
        isPurchaseArticleCostDetailValid = false;
    }
    if (parseInt($('#PurchaseArticleCostDetail_Amount').val(), 10) === 0 ) {
        $("#PurchaseArticleCostDetail_Amount").closest('div').append('<p class="valid_Amount" style="color:red">Please put amount.</p>');
        isPurchaseArticleCostDetailValid = false;
    }

    

    return isPurchaseArticleCostDetailValid;
}
//=======================================
//=======================================
$(document).ready(
    EnableDisablePurchaseArticleTabs()

);