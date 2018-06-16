var latitude, longitude;
if (navigator.geolocation) {
    navigator.geolocation.getCurrentPosition(function (p) {
        var LatLng = new google.maps.LatLng(p.coords.latitude, p.coords.longitude);
        latitude = p.coords.latitude;
        longitude = p.coords.longitude;
        $("#iinput-latitude").val(latitude);
        $("#iinput-longitude").val(longitude);
        var mapOptions = {
            center: LatLng,
            zoom: 13,
            mapTypeId: google.maps.MapTypeId.ROADMAP
        };
        var map = new google.maps.Map(document.getElementById("idiv-map"), mapOptions);
        var marker = new google.maps.Marker({
            position: LatLng,
            map: map,
        });
        google.maps.event.addListener(map, 'click', function (event) {
            uluru = { lat: event.latLng.lat(), lng: event.latLng.lng() }
            marker.setMap(null);
            marker = new google.maps.Marker({
                position: uluru,
                map: map
            });
            latitude = event.latLng.lat();
            longitude = event.latLng.lng();
            var infowindow = new google.maps.InfoWindow({
                content: "<div style ='height:60px;width:200px;font-weight:bold'>Medical Place Location" + "</div>"
            });
            infowindow.open(map, marker);

            $("#iinput-latitude").val(latitude);
            $("#iinput-longitude").val(longitude);
        });
    });
} else {
    alert('Geo Location feature is not supported in this browser.');
    latitude = 30.0594838;
    longitude = 31.2934839;
    $("#iinput-latitude").val(latitude);
    $("#iinput-longitude").val(longitude);
}


// form action with suitable controller 
$(document).ready(function () {
    $("#iselect-options-place-types").change(function () {
        var selected = $(this).children(":selected").text();
        if (selected == "Pharmacy") {
            $("#iform-add-new-place").attr("action","/Pharmacy/AddPharmacy");
        }
        else {
            $("#iform-add-new-place").attr("action","/MedicalPlace/AddMedicalPlace");
        }
    });
});