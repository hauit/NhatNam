
function GetWorkingFollow(url,url2) {
    $.ajax({
        url: url,
        dataType: 'JSON',
        type: 'POST',
        traditional: true,
        cache: false,
        success: function (data) {
            var list = '';
            $.each(data, function (i, val) {
                list += '<li>';
                var a = val.Alias + ".html";
                list += '<a href="' + url2.replace(".html", a) + '">';
                list += val.Caption;
                list += '</a>';
                list += '</li>';
            });
            $("#notify_WorkFollow_items").html(list);
        },
    });
}

function GetDataForReportNumber1(url,reportID) {
    $.ajax({
        url: url,
        dataType: 'JSON',
        type: 'POST',
        traditional: true,
        cache: false,
        data: { 'reportID': reportID,'json':true},
        success: function (data) {
            if (data == "NG")
            {
                return;
            }

            var list = '';
            $.each(data, function (i, val) {
                list += '<li>';
                list += val.MoNo;
                list += '</li>';
            });
            $("#ReportContent").html(list);
        },
    });

}