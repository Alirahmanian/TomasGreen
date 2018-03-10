var local = {};
local.Select = "Выбрать";
local.enPackingPlanConfirmMess = "You have selected different company.This will remove mix and packing details for this packing plan including article and Company balances.Are you sure you will do that ?";
local.ruPackingPlanConfirmMess = "Вы выбрали другую компанию. Это позволит удалить информацию о смешивании и упаковке для этого плана упаковки, включая товар и балансы компании. Вы уверены, что это сделаете?";
//=======================================
//=======================================
function getLangPath() {
    if (window.location.href.indexOf('/en/') !== -1) {
        return "/en/";
    }
    if (window.location.href.indexOf('/ru/') !== -1) {
        return "/ru/";
    }
    return "/en/";
}
//=======================================
//=======================================
function getLang() {
    if (window.location.href.indexOf('/en/') !== -1) {
        return "en";
    }
    if (window.location.href.indexOf('/ru/') !== -1) {
        return "ru";
    }
    return "en";
}
//=======================================
//=======================================
function getLocal(txt) {
    if (getLang() === "ru") {
        var res = eval("local." + txt);
        if (res !== "" && res !== "undefined")
            return res;
        else
            return txt;
    }
    return txt;
}
//=======================================
//=======================================
function LoadFromAndToWarehousesForCompany(Company) {
    if ($(Company).val() !== "0" && $(Company).val() !== getLocal('Select')) {
        var urlWithLang = getLangPath() + "Packing/PackingPlans/";
        LoadWarehouses(Company, urlWithLang + "GetToWarehousesByCompany", $("#PackingPlanMix_ToWarehouseID")); 
        LoadWarehouses(Company, urlWithLang + "GetFromWarehousesByCompany", $("#PackingPlanMixArticle_WarehouseID"));

    }
    //else {
    //    $('#FromWarehouseID').find('option:not(:first)').remove();
    //}

}
//=======================================
//=======================================
function LoadWarehouses(Company, urlWithLang, target) {

    $.ajax({
        type: "GET",
        url: urlWithLang,
        data: { 'companyID': $(Company).val() },
        dataType: "json",
        success: function (data) {
            renderWarehouses($(target), data);
        },
        error: function (error) {
            console.log(error);
        }
    });

    //else {
    //    $('#FromWarehouseID').find('option:not(:first)').remove();
    //}

}

//=======================================
//=======================================
function LoadArticles(Warehouse) {
    if ($(Warehouse).val() !== "0" && $(Warehouse).val() !== getLocal('Select')) {
        var urlWithLang = getLangPath() + "Packing/PackingPlans/GetArticlesByWarehouse";

        $.ajax({
            type: "GET",
            url: urlWithLang,
            data: { 'warehouseId': $(Warehouse).val(), 'companyID': $("#PackingPlan_CompanyID").val() },
            dataType: "json",
            success: function (data) {
                renderArticles($("#PackingPlanMixArticle_ArticleID"), data);
            },
            error: function (error) {
                console.log(error);
            }
        })
    }
    //else {
    //    $('#ArticleID').find('option:not(:first)').remove();
    //}

}
//=======================================
//=======================================
function renderWarehouses(element, data) {
    var $ele = $(element);
    $ele.empty();
    $ele.append($('<option/>').val('0').text(getLocal('Select')));
    var counter = 0;
    $.each(data,
        function (i, val) {
            $ele.append($('<option/>').val(val.id).text(val.name));
            counter++;
        }
    )
    if (counter === 1) {
        $ele.val($("#element option:second").val());
    }
}
//=======================================
//=======================================
function renderArticles(element, data) {
    var $ele = $(element);
    $ele.empty();
    $ele.append($('<option/>').val('0').text(getLocal('Select')));
    $.each(data,
        function (i, val) {
            $ele.append($('<option/>').val(val.id).text(val.name + val.articlesonhand));
        }
    )
}

function ValidateIfCompanyIsChanged() {
    if ($("#SavedCompanyID").val() !== "") {
        if ($("#PackingPlan_CompanyID").val() !== $("#SavedCompanyID").val()) {
            if (confirm((getLang() === "en") ? getLocal("enPackingPlanConfirmMess") : getLocal("ruPackingPlanConfirmMess")) {
                return true;
            }
            else {
                return false;
            }
        }
    }
    return true;
}


//=======================================
//=======================================
$(document).ready(function () {

});

  


