$(document).ready(function () {
    $('#itbl-medical-history').DataTable({
        "order": [[0, "desc"]],
        "iDisplayLength": 10,
        "bPaginate": true,
        "sPaginationType": "full_numbers"
    });
});