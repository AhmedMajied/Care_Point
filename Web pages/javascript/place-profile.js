let maxSlotDuration = 360; //6 hours
var workSlotColor = "#44ff44cc";
var emptySlotColor = "#ffffff";

function breakSlots(slots) {
    for(var i=0; i<slots.length; i++) {
        var duration = slots[i].durationInMinutes;
        var slotType = slots[i].type;
        var slotDescription = slots[i].description;
        if(duration > maxSlotDuration) {
            slots[i].durationInMinutes = maxSlotDuration;
            duration -= maxSlotDuration;
            slots.splice(i, 0, {type: slotType, durationInMinutes: duration, description: slotDescription});
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
    if(slotType == 'work-slot') {
        slotColor = workSlotColor;
    }
    else {
        slotColor = emptySlotColor;
    }
    $("#" + slotID).css({
        "transform": "rotate(" + offset + "deg) translate3d(0,0,0)"
    });
    $("#" + slotID + " span").css({
        "transform"       : "rotate(" + sizeRotation + "deg) translate3d(0,0,0)",
        "background-color": slotColor
    });
}

function createPieChart(pieElement, serviceIndex, slots) {
    var offset = 0;
    for(var i=0; i<slots.length; i++) {
        var slot = slots[i];
        var size = slotSize(slot.durationInMinutes);
        var slotID = "slice-" + serviceIndex + "-" + i;
        var slotType = slot.type;
        addSlot(slotID, slotType, size, pieElement, offset);
        offset += size;
    }
}

$(document).ready(function() {
    $('.piechart').each(function(serviceIndex) {
        var slots = [
            {type: 'work-slot', durationInMinutes: 60, description: 'Working from 2 to 3'},
            {type: 'empty-slot', durationInMinutes: 240, description: 'Off from 4 to 5'},
            {type: 'work-slot', durationInMinutes: 90, description: 'Working from 6 to 7'},
            {type: 'empty-slot', durationInMinutes: 40, description: 'Off from 8 to 9'},
            {type: 'work-slot', durationInMinutes: 120, description: 'Working from 10 to 11'},
            {type: 'empty-slot', durationInMinutes: 170, description: 'Off from 12 to 13'},
        ];
        breakSlots(slots);
        createPieChart($(this), serviceIndex, slots);
    });

    $('.btn-toggle').on('click', function() {
        $(this).find('.btn').toggleClass('active btn-primary btn-default');
        $(this).closest('.cdiv-schedule-visualized').find('.clock').toggleClass('hidden');
    });
});