// search for available Camera when new patient link is clicked
$("#ilink-new-patient").click(function () {
    Instascan.Camera.getCameras().then(function (cameras) {
        if (cameras.length > 0) {
            scanner.start(cameras[0]);
        } else {
            alert('No cameras found.');
        }
    }).catch(function (e) {
        alert(e);
    });
});

// close camera when new patient modal is closed
$("#imodal-new-patient").on("hide.bs.modal", function () {
    scanner.stop();
});

// attach scanner to video
let scanner = new Instascan.Scanner({
    video: document.getElementById('scanner')
});

// called when QR code is recognized
scanner.addListener('scan', function (citizenQRCode) {
    scanner.stop();
    $.post("/Citizen/GetCitizenByQR", { citizenQRCode: citizenQRCode }, function (citizen) {
        if (citizen == "") {
            alert("invalid QR Code");
        }
    });
});