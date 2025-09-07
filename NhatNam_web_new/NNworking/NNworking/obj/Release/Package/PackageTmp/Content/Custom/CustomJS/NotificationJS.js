

$(function () {
    var chat = $.connection.technicalNotify;
    chat.client.hello = function (message) {
        var notification_item = '';
        var notification_item_sub = '';
        notification_item_sub += '<li class="header">You have ' + message.RowNumber + ' notifications</li>';
        notification_item_sub += '            <li>                                   ';
        notification_item_sub += '                <!-- inner menu: contains the actual data -->     ';
        notification_item_sub += '                <ul class="menu">       ';
        $.each(message.Values, function (key, val) {
            if (key > 6)
            {
                return;
            }

            notification_item += '<li>';
            notification_item += '<a>';
            notification_item += '<i class="fa fa-hand-o-right text-aqua"></i>';
            notification_item += '<span>';
            notification_item += val.NotifyCaption;
            notification_item += '</span>';
            notification_item += '</a>';
            notification_item += '</li>';
                                                                                    
            notification_item_sub += '<li onclick="XemNoiDungThongBao(\'' + val.FromClient + '\',\'' + val.NotifyCaption + '\',\'' + val.NotifyContent + '\')">';
            notification_item_sub += '    <a href="#">';
            notification_item_sub += '        <i class="fa fa-address-card text-aqua"></i>' + val.NotifyCaption;
            notification_item_sub += '    </a>';
            notification_item_sub += '</li>';

        });

        notification_item_sub += '    </ul>                                       ';
        notification_item_sub += '</li>                                           ';
        notification_item_sub += '<li class="footer"><a href="#">View all</a></li>';

        if (notification_item == '')
        {
            return;
        }

        $("#notify_info").css('background-color', 'red');
        $("#notify_info_items").prepend(notification_item);
        $("#notify_info_items_sub").prepend(notification_item_sub);
        $('.label-warning').html(message.RowNumber);
    };

    chat.client.acceptNewNotify = function (message) {
        var number = parseInt($('.label-warning').html());
        number = number + 1;
        var notification_item = '';
        var notification_item_sub = '';
        notification_item_sub += '<li class="header">You have ' + number + ' notifications</li>';
        notification_item_sub += '            <li>                                   ';
        notification_item_sub += '                <!-- inner menu: contains the actual data -->     ';
        notification_item_sub += '                <ul class="menu">       ';
        notification_item += '<li onclick="XemNoiDungThongBao(\'' + message.FromClient + '\',\'' + message.NotifyCaption + '\',\'' + message.NotifyContent + '\')">';
        notification_item += '<a>';
        notification_item += '<i class="fa fa-hand-o-right text-aqua"></i>';
        notification_item += '<span>';
        notification_item += message.NotifyCaption;
        notification_item += '</span>';
        notification_item += '</a>';
        notification_item += '</li>';

        notification_item_sub += '<li class="number" onclick="XemNoiDungThongBao(\'' + message.FromClient + '\',\'' + message.NotifyCaption + '\',\'' + message.NotifyContent + '\')">';
        notification_item_sub += '    <a href="#">';
        notification_item_sub += '        <i class="fa fa-address-card text-aqua"></i>' + message.NotifyCaption;
        notification_item_sub += '    </a>';
        notification_item_sub += '</li>';

        notification_item_sub += '    </ul>                                       ';
        notification_item_sub += '</li>                                           ';
        notification_item_sub += '<li class="footer"><a href="#">View all</a></li>';

        if (notification_item == '') {
            return;
        }

        $("#notify_info").css('background-color', 'red');
        $("#notify_info_items").prepend(notification_item);
        $("#notify_info_items_sub").prepend(notification_item_sub);
        $('.label-warning').html(number);

        var staffID = sessionStaffID;
        var msgHistory = '';
        msgHistory += '</br><b>' + message.FromClient + '->' + message.ToClient + ':</b> ' + message.NotifyContent;

        $("#msgHistory").append(msgHistory);
        $("#msgHistory").scrollTop(1000);
    };

    chat.client.messageToAvailableUser = function (message) {
        var StaffID = sessionStaffID;
        if (StaffID != message.ToClient) {
            return;
        }

        alert(message.NotifyContent);
        new Notification(message.NotifyContent);
    };

    chat.client.messageToMachine = function (message) {
        GetNewMessage();

        if (chatWithUser == message.FromClient) {
            AppendMessage(message);
        }
        document.getElementById("open_button").style.backgroundColor = "red";
        var messageOption = {
            clickToHide: true,
            autoHide: true,
            autoHideDelay: 2000,
            showAnimation: 'slideDown',
            showDuration: 10,
            hideAnimation: 'slideUp',
            hideDuration: 10,
            gap: 3
        }
        $.notify(message.ToClient + ': ' + message.NotifyContent, messageOption);
        new Notification(message.ToClient + ': ' + message.NotifyContent);
    };

    chat.client.displayReplyForm = function (message) {
        var StaffID = sessionStaffID;
        var messageHtml = '';
        messageHtml += message.NotifyContent + '</Br>';
        messageHtml += '<textarea placeholder="Type message.." name="msgReply" id = "msgReply" required></textarea></Br>';
        messageHtml += '<input class="btn btn-green" style="margin-top: 10px;" type="Button" tabindex="" id="ReadMessage" name="" value="Trả lời" onclick="ReadMessage(\'' + message.ID + '\',\'' + message.FromClient + '\')">';

        $("#MessageContent").html(messageHtml);
        if (StaffID == message.ToClient) {
            $("#DisPlayMessage").fadeIn(200);
        }
    };

    function GetNewMessage() {
        var StaffID = sessionStaffID;
        chat.server.startAcquireChatHistory(sessionStaffID);
    };

    function AppendMessage(message) {
        var msgHistory = '';
        var staffID = sessionStaffID;
        var re = /-?\d+/;
        var m = re.exec(message.NotifyTime);
        var d = new window.Date(Date.parse(m.input));
        msgHistory += '<span style="color:blue;"><b>' + d.getDate() + '/' + (d.getMonth() + 1) + ' ' + d.getHours() + ':' + d.getMinutes() + ' ' + message.FromClient + ' -> ' + message.ToClient + ': ' + message.NotifyContent + '</b></span></br>';
        //msgHistory += '<b>' + message.FromClient + '->' + message.ToClient + ':</b> ' + message.NotifyContent + '</br>';
        $("#msgHistory").append(msgHistory);
        $("#msgHistory").scrollTop(1000);
    }

    chat.client.startAcquireChatHistory = function (message) {
        var staffID = sessionStaffID;
        var chatWith = '';
        $.each(message.Values, function (key, val) {
            var tagID = val.History;
            if (val.ViewStatus != true)
            {
                chatWith += '<li id="' + tagID + '" class="ChatWidth ReceivedMsg" onclick="openMsgForm(\'' + val.FromClient + '\',\'' + tagID + '\')">' + val.FromClient + '</li>';
                return;
            }

            chatWith += '<li id="' + tagID + '" class="ChatWidth" onclick="openMsgForm(\'' + val.FromClient + '\',\'' + tagID + '\')">' + val.FromClient + '</li>';
        });
        $("#AvailableMsg").html(chatWith);
    };

    chat.client.updateAvailableUserList = function (availableUser) {
        availableUserList.push(availableUser);
    };

    chat.client.messageToToolManagement = function (notify) {
        XemNoiDungThongBao(notify.FromClient, notify.NotifyCaption, notify.NotifyContent)
    };

    chat.client.ReloadStandTimeManualChecking = function () {
        $("#StandTimeManualCheckingList").dxDataGrid("instance").refresh();
    };

    chat.client.getListAvailableUser = function (availableUser) {
        availableUserList = availableUser;
    };

    chat.client.removeAvailableUserInList = function (unAvailableUser) {
        var index = availableUserList.indexOf(unAvailableUser);
        if (index !== -1) availableUserList.splice(index, 1);
    };

    $.connection.hub.start().done(function () {
        var a = $.connection.technicalNotify;
        a.server.startAcquireNotification(sessionStaffID);
        a.server.startAcquireChatHistory(sessionStaffID);
    });
});
