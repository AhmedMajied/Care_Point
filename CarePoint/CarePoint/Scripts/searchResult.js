function getMarkAsDropDown(id) {
    markAsDropDown = `<div class='col-md-4 dropdown'>
                            <button class='btn btn-default dropdown-toggle' type='button' data-toggle='dropdown'>
                                Mark As
                                <span class='caret'></span>
                            </button>
                            <ul class='dropdown-menu'>
                                <li><a onclick='addRelative.call(this,` + id + `,"Friend");'>Friend</a></li>
                                <li><a onclick='addRelative.call(this,` + id + `,"Parent");'>Parent</a></li>
                                <li><a onclick='addRelative.call(this,` + id + `,"Scion");'>Scion</a></li>
                            </ul>
                        </div>`
    return markAsDropDown;
}
function getRemoveRelationDropDown(id,relation) {
    var removeRelationDropDown = `<div class='col-md-4 dropdown'>
                                    <button class='btn btn-default dropdown-toggle' type='button' data-toggle='dropdown'>
                                        `+ relation + `
                                        <span class='caret'></span>
                                    </button>
                                    <ul class='dropdown-menu'>
                                        <li><a onclick='removeRelation.call(this,` + id + `);'>Remove</a></li>
                                    </ul>
                                </div>`
    return removeRelationDropDown;
}
function generateHTML(parent, userName, img, id, relation) {
    if (img === null) {
        img = '../../Images/notfound.png';
    }
   
    html = $("<div class='row'>").append("<div class='col-md-2'><img class='cimg-user' src='" + img + "'/></div>")
        .append($("<div class='col-md-6'><div class='cdiv-name'>" + userName + "</div>"));
    if (relation === "None")
        html.append(getMarkAsDropDown(id));
    else
        html.append(getRemoveRelationDropDown(id,relation));
    $(parent).append(html);
    $(parent).append("<hr>");
}
$("#ibtn-close").click(function () {
    $("#itab-non-specialists .row").remove();
    $("#itab-non-specialists hr").remove();
    $("#itab-doctors .row").remove();
    $("#itab-doctors hr").remove();
    $("#itab-pharmacists .row").remove();
    $("#itab-pharmacists hr").remove();
});
$(function () {
    $("#ibtn-srch-account").click(function () {
        var searchBy = $("#iselect-srch-by").val();
        var searchFor = $("#idiv-searchfor").val();
        $("#imodal-people-srch-result button.close").prop('disabled', true);
        $("#imodal-people-srch-result .cdiv-custom-alert").addClass('hidden');
        $("#imodal-people-srch-result #itab-non-specialists, #imodal-people-srch-result #itab-doctors, #imodal-people-srch-result #itab-pharmacists").append('<span class="cspn-proxy"><span class="cspn-loader"></span><br />Loading...</span>');

        if (!(searchFor === "")) {
            $.ajax({
                type: 'POST',
                url: '/Citizen/SearchAccount',
                data: { key: searchBy, value: searchFor },
                dataType: 'json',
                success: function (data) {
                    var citizens = data.citizens;
                    var ccount = citizens.length;
                    for (var i = 0; i < ccount; i++) {
                        generateHTML("#itab-non-specialists", citizens[i].Name, citizens[i].Photo, citizens[i].Id, citizens[i].Relation);
                    }
                    $("#imodal-people-srch-result #itab-non-specialists .cspn-proxy").remove();
                    if (ccount == 0) {
                        $("#imodal-people-srch-result #itab-non-specialists .cdiv-custom-alert").removeClass('hidden');
                    }
                    var doctors = data.doctors;
                    var dcount = doctors.length;
                    for (i = 0; i < dcount; i++) {
                        generateHTML("#itab-doctors", doctors[i].Name, doctors[i].Photo, doctors[i].Id, doctors[i].Relation);
                    }
                    $("#imodal-people-srch-result #itab-doctors .cspn-proxy").remove();
                    if (dcount == 0) {
                        $("#imodal-people-srch-result #itab-doctors .cdiv-custom-alert").removeClass('hidden');
                    }
                    var pharmacists = data.pharmacists;
                    var pcount = pharmacists.length;
                    for (i = 0; i < pcount; i++) {
                        generateHTML("#itab-pharmacists", pharmacists[i].Name, pharmacists[i].Photo, pharmacists[i].Id, pharmacists[i].Relation);
                    }
                    $("#imodal-people-srch-result #itab-pharmacists .cspn-proxy").remove();
                    if (pcount == 0) {
                        $("#imodal-people-srch-result #itab-pharmacists .cdiv-custom-alert").removeClass('hidden');
                    }
                    $("#imodal-people-srch-result button.close").prop('disabled', false);
                },
                error: function (msg) {
                    console.log(JSON.stringify(msg));
                }
            });
        }
        else {
            $("#cspan-searchfor-error").text("Please Enter This Field").css("color","red");
        }
    });
});
$("#idiv-searchfor").keydown(function () {
    $("#cspan-searchfor-error").text("");
});


// start functionality for search medicalPlace
$("#place-close-button").click(function () {
    $("#idiv-search-place-result .row").remove();
    $("#idiv-search-place-result hr").remove();
});
function MedicalPlaceResultHtml(profilePicture, placeURL, placeName, placeType, placeAddress, placePhone) {
    if (profilePicture === null) {
        profilePicture = '../../Images/placenotfound.png';
    }
    html = $("<div class='row'>").append("<div class='col-md-2'><img id='iimg-place'style='width: 40px;' src='" + profilePicture + "'/></div>").append("<div class='col-md-7'> <a href=" + placeURL + ">" + placeName + "</a>" + " (" + placeType + ") " + "<h5> <b>Address: </b>" + placeAddress + "</h5><h5><b>Phone: </b>" + placePhone + "</h5></div> ")
    $("<div class='row'>").append("</div>");
    $("#idiv-search-place-result").append(html);
    $("#idiv-search-place-result").append("<hr>");
}
function getUserLocation() {
    return $.getJSON("http://freegeoip.net/json/").then(function (data) {
        return {
            latitude: data.latitude,
            longitude: data.longitude
        }
    });
}
$("#ibtn-srch-place").click(function () {
    var sType = $("#iinp-service-type").val();
    var pName = $("#iinp-place-type").val();
    var cDistance = $("#ichk-distance").is(':checked');
    var cCost = $("#ichk-cost").is(':checked');
    var cRate = $("#ichk-rate").is(':checked');
    var cPopularity = $("#ichk-popularity").is(':checked');
    var latitude, longitude;
    // to get current location of user
    getUserLocation().then(function (data) {
        latitude = data.latitude;
        longitude = data.longitude;
    if ((sType != "" || pName != "") && (cDistance || cCost || cRate || cPopularity)) {
        $("#ibtn-srch-place").prop("disabled", true);
        $("#iiloading-place-result").css("display", "block");
        var model = {
            serviceType: sType, placeName: pName,
            checkDistance: cDistance, checkCost: cCost,
            checkRate: cRate, checkPopularity: cPopularity,
            latitude: latitude, longitude: longitude
        }
        $.ajax({
            type: 'POST',
            url: '/MedicalPlace/SearchPlace',
            data: { model },
            dataType: 'json',
            success: function (data) {
                data.forEach(function (place) {
                    MedicalPlaceResultHtml(place.Photo, "/MedicalPlace/ProfilePage?id=" + place.ID, place.Name, place.placeType, place.Address, place.Phone)
                });
                $("#iiloading-place-result").css("display", "none");
                $("#ibtn-srch-place").prop("disabled", false);
                $("#imodal-place-srch-result").modal('show');
            }
        });
    }
    else {
        if (sType == "" && pName == "") {
            $("#cspan-service-place-error").text("please fill at least one field").css("color", "red");
        }
        if (!(cDistance || cCost || cRate || cPopularity)) {
            $("#cspan-priority-error").text("select at least one option").css("color", "red");

        }

    }
    });
});
$("#iinp-service-type").keydown(function () {
    $("#cspan-service-place-error").text("");
});
$("#iinp-place-type").keydown(function () {
    $("#cspan-service-place-error").text("");
});
$('#ichk-distance').on('change', function () {
    $('#cspan-priority-error').text("");
});
$('#ichk-cost').on('change', function () {
    $('#cspan-priority-error').text("");
});
$('#ichk-rate').on('change', function () {
    $('#cspan-priority-error').text("");
});
$('#ichk-popularity').on('change', function () {
    $('#cspan-priority-error').text("");
});

