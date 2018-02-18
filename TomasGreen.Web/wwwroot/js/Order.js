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
            $ele.append($('<option/>').val(val.ID).text(val.Name));
        }
    )
}
//=======================================
//=======================================
$(document).ready(
   // EnableDisableOrderDetails(),
   // LoadOrderDetails(),
   // LoadItemGroups($('#ArticleCategories'))




);