$(function () {
    $('#GetJsonData').click(function (event) {
        event.preventDefault();
        $.ajax({
            type: "GET",
            url: '/Home/GetJson',
            cache: false,
            data: {pageNum : 1},
            success: GetJsonData
        })
    })

    $('#Pager').on("click", "span", function (event) {
        event.preventDefault();
        var page = $(this).text();
        var data;
        if (page == ">") {
            data = parseInt($('#currentPage').text()) + 1;
            if ($('#lastPage').empty())
                var lastPage = parseInt($('#currentPage').text());
            else
                var lastPage = parseInt($('#lastPage').text());
            if (data == lastPage + 1)
                data--;
        }
        else if (page == "<") {
            data = parseInt($('#currentPage').text()) - 1;
            if (data == 0)
                data++;
        }
        else data = page;
            
        $.ajax({
            type: "GET",
            url: '/Home/GetJson',
            cache: false,
            data: { pageNum: data },
            success: GetJsonData
        })
    })
});
    
function GetJsonData(data) {
    var results = $('#Json');
    results.empty();

    var contentArray = [];
    contentArray[0] = '<p>Page ' + data.numOfPage + ' of ' + data.numOfPages;
    contentArray[1] = "<ul>";

    for (var i = 0; i < data.humansInfo.length; i++) {

        contentArray.push("<li>id: " + data.humansInfo[i].id + ", name: " + data.humansInfo[i].name + ", phone: " + data.humansInfo[i].phone + "</li>");
    }
    contentArray.push("</ul>");
    results.append(contentArray.join(""));
    
    var pager = $('#Pager');
    pager.empty();

    pagerArray = [];
    pagerArray.push('<span id="back" class="btn btn-default"><</span>')
    var firstId = "firstPage";
    if (data.numOfPage == 1)
        firstId = "currentPage";
        
    pagerArray.push('<span id="' + firstId + '" class="btn btn-default">1</span>')
    if (data.numOfPage - 4 > 1) {
        pagerArray.push('<span id="back" class="btn btn-default">...</span>')
        for (var i = -2; i < 0; ++i) {
            pagerArray.push('<span id="' + (data.numOfPage + i) + '" class="btn btn-default">' + (data.numOfPage + i) + '</span>')
        }
    }
    else {
        for (var i = 2; i < data.numOfPage; ++i) {
            pagerArray.push('<span id="' + i + '" class="btn btn-default">' + i + '</span>')
        }
    }
    if (data.numOfPage != 1 && data.numOfPage != data.numOfPages) {
        pagerArray.push('<span id="currentPage" class="btn btn-default">' + data.numOfPage + '</span>')
    }
    if (data.numOfPages - data.numOfPage > 4) {
        for (var i = 1; i <= 2; ++i) {
            pagerArray.push('<span id="' + (data.numOfPage + i) + '" class="btn btn-default">' + (data.numOfPage + i) + '</span>')
        }
        pagerArray.push('<span id="back" class="btn btn-default">...</span>')
    }
    else {
        for (var i = data.numOfPage + 1; i < data.numOfPages; ++i) {
            pagerArray.push('<span id="' + i + '" class="btn btn-default">' + i + '</span>')
        }
    }
    var lastId = "lastPage";
    if (data.numOfPage == data.numOfPages)
        lastId = "currentPage";
    pagerArray.push('<span id="' + lastId + '" class="btn btn-default">' + data.numOfPages + '</span>')
    pagerArray.push('<span id="forward" class="btn btn-default">></span>')
    pager.append(pagerArray.join(""));



}

function AppendComment(data) {
    if (data != "") {
        var comments = $('#ulComments');
        comments.append('<li>' + data + '</li>')
        $('form').trigger('reset');
    }
}