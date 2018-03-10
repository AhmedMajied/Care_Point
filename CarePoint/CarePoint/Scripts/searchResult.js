function generateHTML(parent, userName, img, href) {
    if (img == null)
        img = "../Images/notfound.png";
    html = $("<div class='row'>").append("<div class='col-md-2'><img class='cimg-user' src=" + img + "/></div>")
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
                url: '/Search/SearchAccount',
                data: { key: searchBy, value: searchFor },
                dataType: 'json',
                success: function (data) {
                    var dcount = data.doctorsCount;
                   
                    for (var i = 0; i < dcount; i++) {//data.doctors[i].Id
                        generateHTML("#itab-doctors", data.doctors[i].Name, data.doctors[i].Photo, "#");
                    }
                    var ccount = data.citizensCount;
                    for (var i = 0; i < ccount; i++) {//data.citizens[i].Id
                        generateHTML("#itab-non-specialists", data.citizens[i].Name, data.citizens[i].Photo, "#");
                    }
                    var pcount = data.pharmacistsCount;
                    for (var i = 0; i < pcount; i++) { //data.pharmacists[i].Id
                        generateHTML("#itab-pharmacists", data.pharmacists[i].Name, data.pharmacists[i].Photo, "#");
                    }
                    $("#imodal-people-srch-result").modal('show');
                },
                error: function (msg) {
                    console.log(JSON.stringify(msg));
                }
            });
        }
    });
});