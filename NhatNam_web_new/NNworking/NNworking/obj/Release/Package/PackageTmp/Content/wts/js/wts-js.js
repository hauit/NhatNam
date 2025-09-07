var OptionID = "";
var MONo = "";
var Shift = "";
var MachineId = "";
var WorkID = "";

var optionID1 = "";
var optionID2 = "";
var optionID3 = "";
var optionID4 = "";
var optionID5 = "";
var optionID6 = "";

var WorkID1 = "";
var WorkID2 = "";
var WorkID3 = "";
var WorkID4 = "";
var WorkID5 = "";
var WorkID6 = "";

var palet = 0;

$(document).ready(function () {
    var popupStatus = 0;
    /** các hàm dùng chug cho popup**/
    $(this).keydown(function (event) {
        if (event.which == 27) {
            disablePopup(); 
        }
    });

    $("#background-popup").click(function () {
        disablePopup();
    });

    function loadBackgroundToPopup() {
        $("#background-popup").css("opacity", "0.2");
        $("#background-popup").fadeIn(200);
    }

    function disablePopup() {
        if (popupStatus == 1) {
            $("#ShiftButton").fadeOut(200);
            $("#MachineGroupButton").fadeOut(200);
            $("#OptionIDButton").fadeOut(200);
            $("#background-popup").fadeOut(200);
            popupStatus = 0;
        }
    }
    /** kết thúc các hàm dùng chung của popup**/

    /** load các popup của các phần **/
    function loadShiftPopup() {
        if (popupStatus == 0) {
            $("#ShiftButton").fadeIn(200);
            loadBackgroundToPopup();
            popupStatus = 1;
        }
    }

    function loadMachineGroupPopup() {
        if (popupStatus == 0) {
            $("#MachineGroupButton").fadeIn(200);
            loadBackgroundToPopup();
            popupStatus = 1;
        }
    }

    function loadOptionIDPopup() {
        if (popupStatus == 0) {
            $("#OptionIDButton").fadeIn(200);
            loadBackgroundToPopup();
            popupStatus = 1;
        }
    }
    /** kết thúc các hàm load popup**/

    /** các hàm gọi sự kiện click **/
    $("#ShiftBT").click(function () {
        loadShiftPopup();
    });

    $("#MachineBT").click(function () {
        loadMachineGroupPopup();
    });

    $(".optionID").click(function () {
        loadOptionIDPopup();
    });
})

function ShiftValue(data) {
    Shift = data;
    alert(Shift);
}
function MachineGroup(data) {
    MachineId = data;
    alert(MachineId);
}

function OptionIDValue(NC) {
    if (palet == 1)
    {
        optionID1 = NC;
    }
    else if (palet == 2)
    {
        optionID2 = NC;
    }
    else if (palet == 3) {
        optionID3 = NC;
    }
    else if (palet == 4) {
        optionID4 = NC;
    }
    else if (palet == 5) {
        optionID5 = NC;
    }
    else if (palet == 6) {
        optionID6 = NC;
    }
    var MONoButton = '<input class="btn btn-green" style="margin-top: 10px;" type="Button" tabindex="" id=" + val + " name="" value="MoNo 01" onclick="MONoValue(' + palet + ',"lenh")"> ';
    MONoButton += '<input class="btn btn-green" style="margin-top: 10px;" type="Button" tabindex="" id=" + val + " name="" value="MoNo 01" onclick="MONoValue(' + palet + ',"lenh")"> ';//thêm lệnh và
    $("#OptionIDButton").html(MONoButton);
}

function PaletNumber(STT) {
    palet = STT;
    alert(palet);
    $.ajax({
        url: '@Url.Action("ListOptionID", "WTSF2")',
        dataType: 'Json',
        type: 'GET',
        data: '',
        cache: false,
        success: function (data) {
            var OptionIDBT = "";
            $.each(data, function (i, val) {
                OptionIDBT += '<input class="btn btn-green" style="margin-top: 10px;" type="Button" tabindex="" id="' + val + '" name=""  value="' + val + '" onclick="OptionIDValue(\'' + val + '\')"> ';
            });
            $("#OptionIDButton").html(OptionIDBT);
        },
    });
}
function MONoValue(palet,lenh) {
    alert(lenh);
    $("#OK"+palet).val('1');
}
