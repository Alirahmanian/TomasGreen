var ArticleCategories = []
//=======================================
//=======================================
function LoadArticleCategories(element) {
    if (ItemGroups.length === 0) {
        $.ajax
            ({
                type: "GET",
                url: 'Sales/Orders/GetArticleCategories',
                success: function (data) {
                    ItemGroups = data;
                    renderArticleCategories(element);
                },
                error: function (xhr, status, error) {
                    console.log(error);
                    console.log(errxhr.responseTextor);
                }
            })
    }
    else {
        renderArticleCategories(element);
    }
}
//=======================================
//=======================================
function renderArticleCategories(element) {
    var $ele = $(element);
    $ele.empty();
    $ele.append($('<option/>').val('0').text('Select'));
    $.each(ArticleCategories,
        function (i, val) {
            $ele.append($('<option/>').val(val.ID).text(val.Name));
        }
    )
}
//=======================================
//=======================================
function LoadArticles(ArticleCategory) {
    $.ajax({
        type: "GET",
        url: "GetArticlesByCategoryId",
        data: { 'categoryId': $(ArticleCategory).val() },
        dataType: "json",
        success: function (data) {
            renderArticles($("#OrderDetail_ArticleID"), data);
           // renderArticles($('#OrderDetail.ArticleID'), data);
        },
        error: function (error) {
            console.log(error);
        }
    })
}
//=======================================
//=======================================
function renderArticles(element, data) {
    var $ele = $(element);
    $ele.empty();
    $ele.append($('<option/>').val('0').text('Select'));
    $.each(data,
        function (i, val) {
            $ele.append($('<option/>').val(val.id).text(val.name));
        }
    )
}
//=======================================
//=======================================
function LoadWarehouses(Article) {
    $.ajax({
        type: "GET",
        url: "GetWarehousesByArticleID",
        data: { 'articleId': $("#OrderDetail_ArticleID").val() },
        success: function (data) {
            renderWarehouses($("#OrderDetail_WarehouseID"), data);
        },
        error: function (error) {
            console.log(error);
        }
    })
}
//=======================================
//=======================================
function renderWarehouses(element, data) {
    var $ele = $(element);
    $ele.empty();
    $ele.append($('<option/>').val('0').text('Select'));
    $.each(data,
        function (i, val) {
            $ele.append($('<option/>').val(val.id).text(val.name + val.articlesonhand));
        }
    )
}
//=======================================
//=======================================
function LoadCustomerInfo(customer) {
    //$.ajax({
    //    type: "GET",
    //    url: "GetCompanyInfoForOrder",
    //    data: { 'customerId': $(customer).val() },
    //    dataType: "html",
    //    success: function (response) {
    //        $('#CustomerInfoPlaceHolder').html(response);
    //        $('#CustomerInfoPlaceHolder').dialog('open');
    //    },
    //    error: function (error) {
    //        console.log(error);
    //    }
    //})
    $("#CustomerInfoPlaceHolder").load("GetCompanyInfoForOrder",
        { customerId: $(customer).val() });
}

//=======================================
//=======================================
function LoadArticleWarehoseBalance(warehouse) {
    $("#ArticleWarehouseBalance").load("GetArticleWarehoseBalance",
        { articleId: $("#OrderDetail_ArticleID").val(), warehouseId: $(warehouse).val()});
}
//=======================================
//=======================================
function EnableDisableOrderDetails() {
    if ($("#Order_ID").val() === "" || $("#Order_ID").val() === "0") {
        $("#Section1").find(":input").prop("disabled", true);
        //$("#OrderDetail_Tab").removeClass("active");
        //$("#OrderDetail_Tab").addClass("active");
        $("#OrdedDetail_Mess").text('Please save order header first.');
        
    }
    else {
        $("#Section1").find(":input").prop("disabled", false);
        //$("#OrderDetail_Tab").addClass("active");
        $("#OrdedDetail_Mess").text('');
    }
   // $("#OrderDetail_Extended_Price").prop('disabled', true);
}
//=======================================
//=======================================
function ValidateOrder() {
    var isOrderValid = true;
    if ($('#Order_CompanyID').val() === "0" || $('#Order_CompanyID').val() === "Select") {
        $("#Order_CompanyID").closest('div').append('<p style="color:red">Please choose a customer.</p>');
       // $('#Order_CompanyID').data('val', 'false');
        isOrderValid = false;
    }
    $("#Order_AmountPaid").val(function (i, v) { return v.replace("kr", ""); }).val();
    
    return isOrderValid;
}
//=======================================
//=======================================
function ValidateOrderDetails() {
    var isAllValid = true;
    if ($('#HOrderID').val() !== 'undefined' && $("#HOrderID").val() !== "") {
        if ($('#ItemGroup').val() === "0" || $('#ItemGroup').val() === "Select") {
            isAllValid = false;
            $('#ItemGroup').siblings('span.error').css('visibility', 'visible');
        }
        else {
            $('#ItemGroup').siblings('span.error').css('visibility', 'hidden');
        }
        if ($('#Item').val() === "0" || $('#Item').val() === "Select") {
            isAllValid = false;
            $('#Item').siblings('span.error').css('visibility', 'visible');
        }
        else {
            $('#Item').siblings('span.error').css('visibility', 'hidden');
        }
        if ($('#Warehouse').val() === "0" || $('#Warehouse').val() === "Select") {
            isAllValid = false;
            $('#Warehouse').siblings('span.error').css('visibility', 'visible');
        }
        else {
            $('#Warehouse').siblings('span.error').css('visibility', 'hidden');
        }
        if (!$.isNumeric($('#Boxes').val())) {
            isAllValid = false;
            $('#Boxes').siblings('span.error').css('visibility', 'visible');
        }
        else {
            $('#Boxes').siblings('span.error').css('visibility', 'hidden');
        }

        if ($('#ReserveBoxes').val() !== "" && !$.isNumeric($('#ReserveBoxes').val())) {
            isAllValid = false;
            $('#ReserveBoxes').siblings('span.error').css('visibility', 'visible');
        }
        else {
            $('#ReserveBoxes').siblings('span.error').css('visibility', 'hidden');
        }
        if ($('#ExtraKg').val() !== "" && !$.isNumeric($('#ReserveBoxes').val())) {
            isAllValid = false;
            $('#ExtraKg').siblings('span.error').css('visibility', 'visible');
        }
        else {
            $('#ExtraKg').siblings('span.error').css('visibility', 'hidden');
        }
        if (!$.isNumeric($('#Price').val())) {
            isAllValid = false;
            $('#Price').siblings('span.error').css('visibility', 'visible');
        }
        else {
            $('#Price').siblings('span.error').css('visibility', 'hidden');
        }
        if (isAllValid) {
            CalcExtendexPrice();
        }
    }
}
//=======================================
//=======================================
$(document).ready(
    EnableDisableOrderDetails(),
   // LoadOrderDetails(),
   // LoadItemGroups($('#ArticleCategories'))
 



);