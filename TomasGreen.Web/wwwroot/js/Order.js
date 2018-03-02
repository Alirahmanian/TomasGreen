var ArticleCategories = []
//=======================================
//=======================================
function LoadArticleCategories(element) {
    if (ArticleCategories.length === 0) {
        $.ajax
            ({
                type: "GET",
                url: 'GetArticleCategories',
                success: function (data) {
                    ArticleCategories = data;
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
    if ($(ArticleCategory).val() !== "0" && $(ArticleCategory).val() !== "Select") {
        $.ajax({
            type: "GET",
            url: "https://localhost:44378/en/Import/Orders/GetArticlesByCategoryId",
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
    else {
        $('#OrderDetail_ArticleID').find('option:not(:first)').remove();
        $('#OrderDetail_WarehouseID').find('option:not(:first)').remove();
    }
    
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
    if ($(Article).val() !== "0" && $(Article).val() !== "Select") {
        $.ajax({
            type: "GET",
            url: "https://localhost:44378/en/Import/Orders/GetWarehousesByArticleID",
            data: { 'articleId': $("#OrderDetail_ArticleID").val() },
            success: function (data) {
                renderWarehouses($("#OrderDetail_WarehouseID"), data);
            },
            error: function (error) {
                console.log(error);
            }
        })
    }
    else {

        $('#OrderDetail_WarehouseID').find('option:not(:first)').remove();
    }
   
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
    if ($(customer).val() !== "0" && $(customer).val() !== "Select") {
        $("#CustomerInfoPlaceHolder").load("GetCompanyInfoForOrder",
            { customerId: $(customer).val() });
    }
    else {
        $("#CustomerInfoPlaceHolder").text('');
    }
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
    
}

//=======================================
//=======================================
function LoadArticleWarehoseBalance(warehouse) {
    if ($(warehouse).val() !== "" && $(warehouse).val() !== "0" && $(warehouse).val() !== "Select") {
        if ($("#OrderDetail_ArticleID").val() !== "" && $("#OrderDetail_ArticleID").val() !== "0" && $("#OrderDetail_ArticleID").val() !== "Select") {
            $("#ArticleWarehouseBalance").load("GetArticleWarehouseBalance",
                { articleId: $("#OrderDetail_ArticleID").val(), warehouseId: $(warehouse).val() });
        }
      
    }
    else {
        $("#ArticleWarehouseBalance").text('');
    }
    
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
    LoadArticleWarehoseBalance($("#ArticleWarehouseBalance"))
   // LoadOrderDetails(),
   // LoadItemGroups($('#ArticleCategories'))
 



);