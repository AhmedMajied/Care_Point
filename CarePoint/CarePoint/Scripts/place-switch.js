// start genetare Modal Switch Place Data 
function generateModalTabs(tabId, tabName, indx) {
    if (indx == 0) {
        parent = $("<li role='presentation' class='active'>");
    }
    else {
        parent = $("<li role='presentation'>");
    }
    html = $("<a href=" + tabId + " role = 'tab' data-toggle='tab'>" + tabName + "</a></li>");
    $(parent).append(html);
    $("#imodal-place-switch #iul-tabs-place-types").append(parent);
}
function generateModalContent(placeType, indx) {
    if (indx == 0) {
        html = $("<div id='" + placeType + "' role='tabpanel' class='tab-pane active'>").append("</div>");
    }
    else {
        html = $("<div id='" + placeType + "' role='tabpanel' class='tab-pane'>").append("</div>");
    }
    $("#imodal-place-switch .tab-content").append(html);
}
function generateSwitchWorkPlaceModal(placeId, parent,placeType, placeLogo, placeName, placeURL)
{
    btnClick = "onClick=switchWorkPlace(\'" + placeId + '\',\'' + placeType + '\',\'' + placeURL + "\')";
    html = $("<div class='row'>").append("<div class='col-md-2 cdiv-vcenter'><img style='width:50px;height:50px;'id='cimg-medical-place-logo' src='" + placeLogo + "'/></div>")
        .append("<div class='col-md-5 cdiv-vcenter'><label class='clbl-name'>" + placeName + "</label></div>")
        .append("<div class='col-md-4 cdiv-vcenter'> <button  class='btn btn-default'" + btnClick+"> Open profile</button ></div > ")
        .append("</div>");
    $(parent).append(html);
    $(parent).append("<hr>");
}
// End genetare Modal Switch Place Data 
// Start add Cookie For SwitchPlace
function switchWorkPlace(placeId, placeType,placeURL) {
    $.ajax({
        type: 'POST',
        url: "/MedicalPlace/SwitchPlace",
        data: { id: placeId, type: placeType, url: placeURL },
        dataType: 'json',
        success: function (data) {
            window.location = placeURL;
        },
        error: function (msg) {
            alert("Sorry an Error Happened Please Try Again");
        }
    });
}
// End add Cookie For SwitchPlace
//Start Functionality Of GetSpecialist WorkPlaces
$(function () {
    $("#ilink-change-place").click(function () {
        $("#imodal-place-switch button.close").prop('disabled', true);
        $("#imodal-place-switch .cdiv-custom-alert").addClass('hidden');
        $("#imodal-place-switch .tab-content").append('<span class="cspn-proxy"><span class="cspn-loader"></span><br />Loading...</span>');
        var docId = $("#iinput-usr").val();
        $.ajax({
            type: 'POST',
            url: '/Citizen/GetSpecialistWorkPlaces',
            data: { specialistId: docId },
            dataType: 'json',
            success: function (data) {
                var medicalPlaceTypes = data.medicalPlaceTypes;
                var medicalPlaces = data.result;
                var numOfTypes = medicalPlaceTypes.length;
                var numOfPlaces = medicalPlaces.length;
                for (var i = 0; i < numOfTypes; i++)
                {
                    generateModalTabs("#itab-" + medicalPlaceTypes[i].Type, medicalPlaceTypes[i].Type, i);
                    generateModalContent("itab-" + medicalPlaceTypes[i].Type, i);
                }
                for (var i = 0; i < numOfPlaces; i++) {
                    generateSwitchWorkPlaceModal(medicalPlaces[i].ID, "#itab-" + medicalPlaces[i].Type, medicalPlaces[i].Type, medicalPlaces[i].Photo, medicalPlaces[i].Name, medicalPlaces[i].url);
                }
                $("#imodal-place-switch .tab-content .cspn-proxy").remove();
                if (numOfTypes == 0) {
                    $("#imodal-place-switch .tab-content").removeClass('hidden');
                }
                $("#imodal-place-switch button.close").prop('disabled', false);
            },
            error: function (msg) {
                alert("An Error Occured Please Try Again!");
            }
        });
    });
});

$("#imodal-place-switch button.close").click(function () {
    $("#imodal-place-switch #iul-tabs-place-types").empty();
    $("#imodal-place-switch .tab-content").empty();
});
//End Functionality Of GetSpecialist WorkPlaces

// Start Current WorkPlace 
$("#ilink-place-settings").click(function () {
    $.ajax({
        type: 'POST',
        url: '/MedicalPlace/GetCurrentWorkPlace',
        dataType: 'json',
        success: function (data) {
            if (data.url != "#")
                window.location = data.url;
            else
                alert("Please Choose Your WorkPlace First");
        },
        error: function (msg) {
            alert("Sorry An Error Happend Please Try Again");
        }
    });
})
// End Current WorkPlace