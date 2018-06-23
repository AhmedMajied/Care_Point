function invokeScanner() {
    // search for available Camera
    Instascan.Camera.getCameras().then(function (cameras) {
        if (cameras.length > 0) {
            $("#imodal-new-patient .modal-footer").removeClass("hidden");
            scanner.start(cameras[0]);
        } else {
            alert('No cameras found.');
        }
    }).catch(function (e) {
        alert(e);
    });
}

$("#ilink-new-patient").click(function () {
    invokeScanner();
});

$("#ibtn-rescan-card").click(function () {
    $("#idiv-invalid-card-msg").addClass("hidden");
    $("#imodal-new-patient .modal-footer").removeClass("hidden");
    invokeScanner();
})

// close camera when new patient modal is closed
$("#imodal-new-patient").on("hide.bs.modal", function () {
    scanner.stop();
    $("#idiv-invalid-card-msg").addClass("hidden");
    $("#imodal-new-patient .modal-footer").addClass("hidden");
});

// attach scanner to video
let scanner = new Instascan.Scanner({
    video: document.getElementById('scanner')
});

// called when QR code is recognized
scanner.addListener('scan', function (citizenQRCode) {
    scanner.stop();
    $.post("/Citizen/GetCitizenByQR", { citizenQRCode: citizenQRCode }, function (citizen) {
        $("#imodal-new-patient .modal-footer").addClass("hidden");
        if (citizen == "") {
            $("#idiv-invalid-card-msg").removeClass("hidden");
        }
    });
});