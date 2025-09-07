function getClientName(url) {
    var curentvalue = $('#clientName').html();
    if (curentvalue != '') {
        return;
    }

    $.ajax({
        url: url,
        dataType: 'JSON',
        type: 'GET',
        traditional: true,
        cache: false,
        success: function (data) {
            if (data == "NG") {
                return;
            }
            var caption = "Xin chào: " + data.staffName
            $('#clientName').html(caption);
        },
    });
}
$(document).ready(function () {
    $("#notify_info").click(function () {
        $("#notify_info").css('background-color', '#68BD56');
    });
})

function FormatDate(datetime) {
    let d = new Date(datetime),
        month = '' + (d.getMonth() + 1),
        day = '' + d.getDate(),
        year = '' + d.getFullYear(),
        hour = '' + d.getHours(),
        minute = '' + d.getMinutes(),
        seconds = '' + d.getSeconds();

    month = month.padStart(2, '0');
    day = day.padStart(2, '0');
    hour = hour.padStart(2, '0');
    minute = minute.padStart(2, '0');
    seconds = seconds.padStart(2, '0');


    return [day, month, year].join('-') + ' ' + [hour, minute, seconds].join(':');
}
