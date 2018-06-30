let maxSlotDuration = 360; //6 hours
var workSlotColor = "#44ff44cc";
var emptySlotColor = "#ffffff";

function breakSlots(slots) {
    for (var i = 0; i < slots.length; i++) {
        var duration = slots[i].durationInMinutes;
        var slotType = slots[i].type;
        var slotDescription = slots[i].description;
        if (duration > maxSlotDuration) {
            slots[i].durationInMinutes = maxSlotDuration;
            duration -= maxSlotDuration;
            slots.splice(i, 0, { type: slotType, durationInMinutes: duration, description: slotDescription });
        }
    }
}

function slotSize(dataNum) {
    let minutesPer12Hours = 720;
    return (dataNum / minutesPer12Hours) * 360;
}

function addSlot(slotID, slotType, size, pieElement, offset) {
    pieElement.append("<div id='" + slotID + "' class='slice'><span></span></div>");
    var offset = offset - 1;
    var sizeRotation = -179 + size;
    var slotColor;
    if (slotType == 'work-slot') {
        slotColor = workSlotColor;
    }
    else {
        slotColor = emptySlotColor;
    }
    $("#" + slotID).css({
        "transform": "rotate(" + offset + "deg) translate3d(0,0,0)"
    });
    $("#" + slotID + " span").css({
        "transform": "rotate(" + sizeRotation + "deg) translate3d(0,0,0)",
        "background-color": slotColor
    });
}

function createPieChart(pieElement, serviceIndex, slots) {
    var offset = 0;
    for (var i = 0; i < slots.length; i++) {
        var slot = slots[i];
        var size = slotSize(slot.durationInMinutes);
        var slotID = "slice-" + serviceIndex + "-" + i;
        var slotType = slot.type;
        addSlot(slotID, slotType, size, pieElement, offset);
        offset += size;
    }
}

$(document).ready(function () {
    var count = $("#ihidden-count").val();
    var services = [];
    var service = {};
    for (var i = 0; i < count; i++) {
        service = {};
        var id = $("#ihidden-service-id-" + i + "").val();
        var daySelected = $("#iselect-options-" + id + " option:selected").val();
        service = { ID: id, Day: daySelected };
        services.push(service);
    }
    GetServicesTime(services);
    $('.btn-toggle').on('click', function () {
        $(this).find('.btn').toggleClass('active btn-primary btn-default');
        $(this).closest('.cdiv-schedule-visualized').find('.clock').toggleClass('hidden');
    });
});
function GetServicesTime(services) {
    $.ajax({
        type: 'POST',
        url: "/MedicalPlace/GetServicesSlots",
        data: { services: services },
        dataType: 'json',
        success: function (data) {
            for (var i = 0; i < data.length; i++)
            {
                var id = data[i].ID;
                var AM = data[i].AM;
                var PM = data[i].PM;
                breakSlots(AM);
                createPieChart($("#piechart-" + id + "-AM"), id + "0", AM);
                breakSlots(PM);
                createPieChart($("#piechart-" + id + "-PM"), id + "1", PM);
            }
        },
        error: function (msg) {
            alert("Sorry an Error Happened Please Try Again");
        }
    });
}
function changeDay(serviceId) {
    $("#piechart-" + serviceId + "-AM").empty();
    $("#piechart-" + serviceId + "-PM").empty();
    var daySelected = $("#iselect-options-" + serviceId + " option:selected").val();
    service = [{ ID: serviceId, day: daySelected }];
    GetServicesTime(service);
}