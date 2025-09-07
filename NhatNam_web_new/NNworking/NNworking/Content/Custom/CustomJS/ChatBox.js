function DisplayChatBox(url) {
    var html = '';
    html += '<button class="open-button" onclick="openForm()" id="open_button">Nhắn tin</button>                                                     ';
    html += '<div class="chat-popup" id="MessageForm">                                                                       ';
    html += '    <div class="availableMsg" id="AvailableMsg">                                                                                                          ';
    html += '       <li onclick="openMsgForm(\'toClient\',\'msgLi1\')" id="msgLi1" class="ReceivedMsg">Server - 1556</li>                                                                              ';
    html += '       <li onclick="openMsgForm(\'toClient\')">Server - 1556</li>                                                                              ';
    html += '       <li onclick="openMsgForm(\'toClient\')">Server - 1556</li>                                                                              ';
    html += '       <li onclick="openMsgForm(\'toClient\')">Server - 1556</li>                                                                              ';
    html += '       <li onclick="openMsgForm(\'toClient\')">Server - 1556</li>                                                                              ';
    html += '       <li onclick="openMsgForm(\'toClient\')">Server - 1556</li>                                                                              ';
    html += '       <li onclick="openMsgForm(\'toClient\')">Server - 1556</li>                                                                              ';
    html += '       <li onclick="openMsgForm(\'toClient\')">Server - 1556</li>                                                                              ';
    html += '       <li onclick="openMsgForm(\'toClient\')">Server - 1556</li>                                                                              ';
    html += '       <li onclick="openMsgForm(\'toClient\')">Server - 1556</li>                                                                              ';
    html += '       <li onclick="openMsgForm(\'toClient\')">Server - 1556</li>                                                                              ';
    html += '       <li onclick="openMsgForm(\'toClient\')">Server - 1556</li>                                                                              ';
    html += '       <li onclick="openMsgForm(\'toClient\')">Server - 1556</li>                                                                              ';
    html += '       <li onclick="openMsgForm(\'toClient\')">Server - 1556</li>                                                                              ';
    html += '       <li onclick="openMsgForm(\'toClient\')">Server - 1556</li>                                                                              ';
    html += '       <li onclick="openMsgForm(\'toClient\')">Server - 1556</li>                                                                              ';
    html += '       <li onclick="openMsgForm(\'toClient\')">Server - 1556</li>                                                                              ';
    html += '       <li onclick="openMsgForm(\'toClient\')">Server - 1556</li>                                                                              ';
    html += '       <li onclick="openMsgForm(\'toClient\')">Server - 1556</li>                                                                              ';
    html += '       <li onclick="openMsgForm(\'toClient\')">Server - 1556</li>                                                                              ';
    html += '       <li onclick="openMsgForm(\'toClient\')">Server - 1556</li>                                                                              ';
    html += '       <li onclick="openMsgForm(\'toClient\')">Server - 1556</li>                                                                              ';
    html += '       <li onclick="openMsgForm(\'toClient\')">Server - 1556</li>                                                                              ';
    html += '       <li onclick="openMsgForm(\'toClient\')">Server - 1556</li>                                                                              ';
    html += '       <li onclick="openMsgForm(\'toClient\')">Server - 1556</li>                                                                              ';
    html += '       <li onclick="openMsgForm(\'toClient\')">Server - 1556</li>                                                                              ';
    html += '    </div>                                                                                                                              ';
    html += '    <div class="msgForm">                                                                                                               ';
    html += '        <div class="form-container">                                                                                                    ';
    html += '            <h3 style="margin-top: 5px;margin-bottom: 5px;">Tin nhắn</h3>                                                               ';
    html += '            <label for="msg"><b>Gửi đến:</b></label>                                                                                    ';
    html += '            <div class="autocomplete" style="width:200px;">                                                                             ';
    html += '                <input id="userNameList" type="text" name="userNameList" autocomplete="false" placeholder="Search username.." onBlur="">                                    ';
    html += '            </div>      <span id="SelectedUserStatus" Style="width: 70px;"> Online </span>                                                                     ';
    html += '                                                                                                                                        ';
    html += '            <div name="msgHistory" id="msgHistory" Style="background-color:#CCC;height: 300px;overflow: scroll;margin-top:5px;"> ';
    html += '            </div>                                                                                                                      ';
    html += '            <label for="msg"><b>Tin nhắn:</b>(bấm phím enter để gửi)</label>                                                                                   ';
    html += '            <textarea placeholder="Type message.." name="msg" id="msg" onfocusin="GetChattingHistory()" required></textarea>                                             ';
    html += '                                                                                                                                        ';
    html += '            <button type="button" class="btn" id="SendMessage" onClick="SendMessage(\'' + url + '\')">Send</button>                                    ';
    html += '            <button type="button" class="btn cancel" onclick="closeForm()">Close</button>                                               ';
    html += '        </div>                                                                                                                          ';
    html += '    </div>                                                                                                                              ';
    html += '</div>                                                                                                                                  ';
    $('#OpenMessage').html(html);
}

$(document).on("keypress", "#msg", function (e) {
    if (e.keyCode == 13) {//&& e.shiftKey
        SendMessage('/AllDept/SendMessageToOther');
        return;
    }
})

function GetChattingHistory() {
    var toClient = $("#userNameList").val();
    var msgHistory = $("#msgHistory").html();
    var alreadyGetHistory = toClient == chatWithUser && msgHistory != '';
    var nochangeUser = toClient == '' && chatWithUser != '';
    if (alreadyGetHistory || nochangeUser)
    {
        return;
    }

    chatWithUser = toClient;
    var staffID = sessionStaffID;
    var msgLi1 = staffID + '-' + toClient;
    $("#" + msgLi1).removeClass("ReceivedMsg");
    msgLi1 = toClient + '-' + staffID;
    openMsgForm(toClient, msgLi1);
}

function CorrectUser(chatWithUser) {
    for(var i = 0; i < userList.length; i ++)
    {
        if(userList[i] == chatWithUser)
        {
            return true;
        }
    }

    return false;
}

function AvailableUser(chatWithUser) {
    for (var i = 0; i < availableUserList.length; i++) {
        if (availableUserList[i] == chatWithUser) {
            return true;
        }
    }

    return false;
}

function openMsgForm(toClient, msgLi1) {
    $("#userNameList").val(toClient);
    chatWithUser = toClient;
    if (!CorrectUser(chatWithUser))
    {
        $("#SelectedUserStatus").html('<span style="color:red;">Invalid user</span>');
        $("#msgHistory").html('');
        return;
    }

    var usserStatus = '';
    if (AvailableUser(chatWithUser)) {
        usserStatus = '<span style="color:green;">Online</span>';
    }
    else
    {
        usserStatus = '<span style="color:red;">Offline</span>';
    }

    $("#SelectedUserStatus").html(usserStatus);
    $("#" + msgLi1).removeClass("ReceivedMsg");
    //// Lấy lịch sử chát sau khi người dùng click vào ô chát với ...
    var staffID = sessionStaffID;
    $.ajax({
        url: '/AllDept/GetChattingHistory',
        dataType: 'Json',
        type: 'POST',
        data: { 'fromClient': staffID, 'toClient': toClient },
        cache: false,
        success: function (data) {
            var msgHistory = ''
            if (data.Status != "OK") {
                //alert(data.Values)
                $("#msgHistory").html(msgHistory);
                return;
            }
            $.each(data.Values, function (i, val) {
                var re = /-?\d+/;
                var m = re.exec(val.NotifyTime);
                var d = new window.Date(parseInt(m));
                if (val.FromClient == staffID)
                {
                    msgHistory += '<span style="color:red;">' + d.getDate() + '/' + (d.getMonth() + 1) + ' ' + d.getHours() + ':' + d.getMinutes() + ' ' + '<b>' + val.FromClient + '->' + val.ToClient + ':</b> ' + val.NotifyContent + '</span></br>';
                }
                else {
                    msgHistory += '<span style="color:blue;">' + d.getDate() + '/' + (d.getMonth() + 1) + ' ' + d.getHours() + ':' + d.getMinutes() + ' ' + '<b>' + val.FromClient + '->' + val.ToClient + ':</b> ' + val.NotifyContent + '</span></br>';
                }
            });
            $("#msgHistory").html(msgHistory);
            $("#msgHistory").scrollTop(1000);
        },
    });

    //// đánh dấu toàn bộ lịch sử là đã đọc
    $.ajax({
        url: '/AllDept/ReadAllMessage',
        dataType: 'Json',
        type: 'POST',
        data: { 'user1': staffID, 'user2': toClient },
        cache: false
    });
}

var availableUserList = new Array();
var userList = new Array();
var chatWithUser = '';
$(document).ready(function () {
    // Lấy danh sách user
    // Lấy danh sách 
    autocomplete(document.getElementById("userNameList"), userList);
})


function GetUser(group, url) {
    $.ajax({
        url: url,
        dataType: 'Json',
        type: 'GET',
        data: { 'groupMachine': group },
        cache: false,
        success: function (data) {
            if (data.Status != "OK") {
                //alert(data.Values)
                return;
            }

            var GroupMachineBT = "";
            GroupMachineBT = '<option value=""></option>';
            $.each(data.Values, function (i, val) {
                GroupMachineBT += '<option value="' + val.StaffID + '">' + val.UserName + '</option>';
                userList.push(val.StaffID);
            });
            $("#SelectUser").html(GroupMachineBT);
        },
    });
}

function SendMessage(url) {
    var toUser = chatWithUser;
    var message = $("#msg").val();
    if (message == '')
    {
        return;
    }

    var msgHistory = '';
    var staffID = sessionStaffID;
    var d = new Date();
    msgHistory = '<span style="color:red;">' + d.getDate() + '/' + (d.getMonth() + 1) + ' ' + d.getHours() + ':' + d.getMinutes() + ' ' + '<b>' + staffID + '->' + toUser + ':</b> ' + message + '</span></br>';
    $.ajax({
        url: url,
        dataType: 'Json',
        type: 'POST',
        data: { 'toUser': toUser, 'message': message },
        cache: false,
        success: function (data) {
            if (data.Status != "OK") {
                alert(data.Values)
                return;
            }

            $("#msg").val("");
            $("#msgHistory").append(msgHistory);
            $("#msgHistory").scrollTop(1000);
        },
    });
}
