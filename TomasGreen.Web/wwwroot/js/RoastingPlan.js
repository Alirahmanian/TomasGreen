var local = {};
local.Select = "Выбрать";
//=======================================
//=======================================
function getLangPath() {
    if (window.location.href.indexOf('/en/') != -1) {
       return "/en/";
    }
    if (window.location.href.indexOf('/ru/') != -1) {
      return "/ru/";
    }
    return "/en/";
}
//=======================================
//=======================================
function getLang() {
    if (window.location.href.indexOf('/en/') != -1) {
        return "en";
    }
    if (window.location.href.indexOf('/ru/') != -1) {
        return "ru";
    }
    return "en";
}
//=======================================
//=======================================
function getLocal(txt) {
    if (getLang() === "ru") {
        var res = eval("local."+ txt);
        if (res !== "" && res !== "undefined")
            return res;
        else
            return txt;
    }
    return txt;
}
//=======================================
//=======================================
function LoadWarehouses(company) {
    if ($(Warehouse).val() !== "0" && $(Warehouse).val() !== "Select") {
        var urlWithLang = getLangPath() + "Roasting/RoastingPlans/GetWarehousesByCompanyId";

        $.ajax({
            type: "GET",
            url: urlWithLang,
            data: { 'warehouseId': $(Warehouse).val() },
            dataType: "json",
            success: function (data) {
                renderArticles($("#CompanyID"), data);
            },
            error: function (error) {
                console.log(error);
            }
        })
    }
    else {
        $('#ArticleID').find('option:not(:first)').remove();
    }

}
//=======================================
//=======================================
function LoadCompanies(Warehouse) {
    if ($(Warehouse).val() !== "0" && $(Warehouse).val() !== "Select") {
        var urlWithLang = getLangPath() + "Roasting/RoastingPlans/GetCompaniesByWarehouseId";

        $.ajax({
            type: "GET",
            url: urlWithLang,
            data: { 'warehouseId': $(Warehouse).val() },
            dataType: "json",
            success: function (data) {
                renderArticles($("#CompanyID"), data);
            },
            error: function (error) {
                console.log(error);
            }
        })
    }
    else {
        $('#ArticleID').find('option:not(:first)').remove();
    }

}
//=======================================
//=======================================
function LoadArticles(Warehouse) {
    if ($(Warehouse).val() !== "0" && $(Warehouse).val() !== "Select") {
        var urlWithLang = getLangPath() + "Roasting/RoastingPlans/GetArticlesByWarehouseId";
        
        $.ajax({
            type: "GET",
            url:  urlWithLang,
            data: { 'warehouseId': $(Warehouse).val() },
            dataType: "json",
            success: function (data) {
                renderArticles($("#ArticleID"), data);
            },
            error: function (error) {
                console.log(error);
            }
        })
    }
    else {
        $('#ArticleID').find('option:not(:first)').remove();
    }

}
//=======================================
//=======================================
function renderCompanies(element, data) {
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
            $ele.append($('<option/>').val(val.id).text(val.name));
        }
    )
}
//=======================================
//=======================================
$(document).ready(
   



);