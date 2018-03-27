function generateHTML(parent, userName, img, href) {
    if (img == null) {
        img = '../Images/notfound.png';
    }
    html = $("<div class='row'>").append("<div class='col-md-2'><img class='cimg-user' src='"+img+"'/></div>")
        .append($("<div class='col-md-6'><div class='cdiv-name'>" + userName + "</div>")).append("<div class='col-md-4 dropdown'><button class='btn btn-default dropdown-toggle' type='button' data-toggle='dropdown'>Mark As<span class='caret'></span></button><ul class='dropdown-menu'><li><a href='" + href + "'>Friend</a></li><li><a href='" + href + "'>Parent</a></li><li><a href='" + href + "'>Sibling</a></li><li><a href='" + href + "'>Non-relative</a></li></ul></div>");
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
    $("#ibtn-acc").click(function () {
        var searchBy = $("#iselect-srch-by").val();
        var searchFor = $("#idiv-searchfor").val();
        if (!(searchFor === "")) {
            $.ajax({
                type: 'POST',
                url: '/Citizen/SearchAccount',
                data: { key: searchBy, value: searchFor },
                dataType: 'json',
                success: function (data) {
                    var citizens = data[0];
                    var ccount = data[0].length;
                    for (var i = 0; i < ccount; i++) {//citizens[i].Id
                        generateHTML("#itab-non-specialists", citizens[i].Name, citizens[i].Photo, "#");
                    }
                    var doctors = data[1];
                    var dcount = data[1].length;
                    for (var i = 0; i < dcount; i++) {//doctors[i].Id
                        generateHTML("#itab-doctors", doctors[i].Name, doctors[i].Photo, "#");
                    }
                    var pharmacists = data[2];
                    var pcount = data[2].length;
                    for (var i = 0; i < pcount; i++) { //pharmacists[i].Id
                        generateHTML("#itab-pharmacists", pharmacists[i].Name, pharmacists[i].Photo, "#");
                    }
                    $("#imodal-people-srch-result").modal('show');
                },
                error: function (msg) {
                    console.log(JSON.stringify(msg));
                }
            });
        }
        else {
            $("#cspan-searchfor-error").text("Please Enter This Field");
        }
    });
});
$("#idiv-searchfor").keydown(function () {
    $("#cspan-searchfor-error").text("");
});

$("#place-close-button").click(function () {
    $("#idiv-search-place-result .row").remove();
    $("#idiv-search-place-result hr").remove();
});
function MedicalPlaceResultHtml(profilePicture, placeURL, placeName, placeType, placeAddress, placePhone, isSpecialist, isJoined, joinStaffLink) {
    if (profilePicture == null) {
        profilePicture = '../Images/placenotfound.png';
    }
    html = $("<div class='row'>").append("<div class='col-md-2'><img id='iimg-place'src='" + profilePicture + "'/></div>").append("<div class='col-md-7'> <a href=" + placeURL + ">" + placeName + "</a>" + " (" + placeType + ") " + "<h5> <b>Address: </b>" + placeAddress + "</h5><h5><b>Phone: </b>" + placePhone + "</h5></div> ")
        .append("<div class='col-md-3'>")
    if (isSpecialist && (!isJoined)) {
        html.append("<a href="+joinStaffLink+"><button class='btn btn-default' type='button'>Join staff</button></div></a>")
    }
    else if (isSpecialist && isJoined) {
        html.append("<span class='fa fa-check-circle-o'></span> joined</div>")
    }
    $("<div class='row'>").append("</div>");
    $("#idiv-search-place-result").append(html);
    $("#idiv-search-place-result").append("<hr>");
}
$("#ibtn-search-place").click(function () {
    var sType = $("#iinp-service-type").val();
    var pType = $("#iinp-place-type").val();
    var cDistance = $("#ichk-distane").is(':checked');
    var cCost = $("#ichk-cost").is(':checked');
    var cRate = $("#ichk-rate").is(':checked');
    var cPopularity = $("#ichk-popularity").is(':checked');
    var latitude, longitude;
    // to get current location of user
    $.getJSON("http://freegeoip.net/json/", function (data) {
        latitude = data.latitude;
        longitude = data.longitude;
    });
    if ((sType != "" || pType != "" )&& (cDistance || cCost || cRate || cPopularity)) {
        var model = {
            serviceType: sType, placeType: pType,
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
                    MedicalPlaceResultHtml(place.Photo, "/MedicalPlace/ProfilePage?id=" + place.ID, place.Name, place.placeType, place.Address, place.Phone, place.isSpecialist, place.isJoined,"#")
                });
                $("#imodal-place-srch-result").modal('show');
            },
            error: function (msg) {
                console.log(JSON.stringify(msg));
            }
        });
    }
    else {
        if (sType == "" && pType == "") {
            $("#cspan-service-place-error").text("please fill at least one field");
        }
        if (!(cDistance || cCost || cRate || cPopularity)) {
            $("#cspan-priority-error").text("select at least one option");

        }

    }
});
$("#iinp-service-type").keydown(function () {
    $("#cspan-service-place-error").text("");
});
$("#iinp-place-type").keydown(function () {
    $("#cspan-service-place-error").text("");
});
$('#ichk-distane').on('change', function () {
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