// Start Search Account
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
        if (!(searchFor === "")) {
            $("#imodal-people-srch-result").modal('show');
            $("#imodal-people-srch-result button.close").prop('disabled', true);
            $("#imodal-people-srch-result .cdiv-custom-alert").addClass('hidden');
            $("#imodal-people-srch-result #itab-non-specialists, #imodal-people-srch-result #itab-doctors, #imodal-people-srch-result #itab-pharmacists").append('<span class="cspn-proxy"><span class="cspn-loader"></span><br />Loading...</span>');
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
//End Search Account

// Start Search MedicalPlace
$("#place-close-button").click(function () {
    $("#idiv-search-place-result .row").remove();
    $("#idiv-search-place-result hr").remove();
});
function MedicalPlaceResultHtml(profilePicture, placeURL, placeName, placeType, placeAddress, placePhone) {
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
            $("#imodal-place-srch-result").modal('show');
            $("#imodal-place-srch-result button.close").prop('disabled', true);
            $("#imodal-place-srch-result .cdiv-custom-alert").addClass('hidden');
            $("#imodal-place-srch-result #idiv-search-place-result").append('<span class="cspn-proxy"><span class="cspn-loader"></span><br />Loading...</span>');
            var model = {
                ServiceType: sType, PlaceName: pName,
                IsDistance: cDistance, IsCost: cCost,
                IsRate: cRate, IsPopularity: cPopularity,
                Latitude: latitude, Longitude: longitude
            }
            $.ajax({
                type: 'POST',
                url: '/MedicalPlace/SearchPlace',
                data: { model },
                dataType: 'json',
                success: function (data) {
                    var places = data.places;
                    var placesCount = places.length;
                    for (var i = 0; i < placesCount; i++) {
                        MedicalPlaceResultHtml(places[i].Photo, "/MedicalPlace/ProfilePage?id=" + places[i].ID, places[i].Name, places[i].placeType, places[i].Address, places[i].Phone);
                    }
                    $("#imodal-place-srch-result #idiv-search-place-result .cspn-proxy").remove();
                    if (placesCount == 0) {
                        $("#imodal-place-srch-result #idiv-search-place-result .cdiv-custom-alert").removeClass('hidden');
                    }
                    $("#imodal-place-srch-result button.close").prop('disabled', false);
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
// End Search MedicalPlace
//searchResult.js

// Start Search Drug
$("#ibtn-close-drug-result").click(function () {
    $("#idiv-search-drug-result .row").remove();
    $("#idiv-search-drug-result hr").remove();
    $("#imodal-drug-title").text("");
});
function searchDrugResult(pharmacyLogo, pharmacyName, pharmacyAddress, pharmacyPhone) {
    html = $("<div class='row'>").append("<div class='col-md-2 cdiv-vcenter'><img style='max-width: 60px;' id='iimg-pharmacy-logo' src='" + pharmacyLogo + "'/></div>")
        .append("<div class='col-md-5 cdiv-vcenter'><label class='clbl-name'>" + pharmacyName + "</label></div>")
        .append("<div class='col-md-4 cdiv-vcenter'> <span class='fa fa-map-marker fa-lg cspn-awsome'></span>&nbsp;&nbsp;" + pharmacyAddress
            + "<br/> <br/><span class='fa fa-phone fa-lg cspn-awsome'></span>&nbsp;&nbsp;" + pharmacyPhone + "</div >");
    $("<div class='row'>").append("</div>");
    $("#idiv-search-drug-result").append(html);
    $("#idiv-search-drug-result").append("<hr>");
}
$("#ibtn-search-drug").click(function () {
    var drugName = $("#iinp-drug-name").val();
    if (drugName != "") {
        $("#imodal-drug-srch-result").modal('show');
        $("#imodal-drug-srch-result button.close").prop('disabled', true);
        $("#imodal-drug-srch-result .cdiv-custom-alert").addClass('hidden');
        $("#imodal-drug-srch-result #idiv-search-drug-result").append('<span class="cspn-proxy"><span class="cspn-loader"></span><br />Loading...</span>');

        getUserLocation().then(function (data) {
            latitude = data.latitude;
            longitude = data.longitude;
            var model = {
                DrugName: drugName,
                Latitude: latitude,
                Longitude: longitude
            }
            $.ajax({
                type: 'POST',
                url: '/Pharmacy/SearchPharmacyMedicine',
                data: { model },
                dataType: 'json',
                success: function (data) {
                    var pharmacies = data.pharmacies;
                    var pharmaciesCount = pharmacies.length;
                    for (var i = 0; i < pharmaciesCount; i++)
                    {
                        searchDrugResult(pharmacies[i].Photo, pharmacies[i].Name, pharmacies[i].Address, pharmacies[i].Phone);
                    }
                    $("#imodal-drug-srch-result #idiv-search-drug-result .cspn-proxy").remove();
                    if (pharmaciesCount == 0) {
                        $("#imodal-drug-srch-result #idiv-search-drug-result .cdiv-custom-alert").removeClass('hidden');
                    }
                    else
                    {
                        $("#imodal-drug-srch-result #imodal-drug-title").text("Pharmacies where " + data.drugName + " is available");
                    }
                    $("#imodal-drug-srch-result button.close").prop('disabled', false);
                }
            });
        });
    }
    else {
        $("#ispan-drug-name-error").text("This Field is Required").css("color", "red");;
    }
})
$("#iinp-drug-name").keydown(function () {
    $("#ispan-drug-name-error").text("");
});
// End Search Drug

