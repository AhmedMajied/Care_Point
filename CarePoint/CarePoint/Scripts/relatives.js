
function addRelative(id, relation) {
    var self = this;
    $.post("/Citizen/AddRelative", { relativeId: id, relationType: relation }).done(
        function (response) {
            if (response.Code == 0) {
                var parentElement = $(self).parent().parent().parent().parent();
                parentElement.children(".dropdown").remove();
                parentElement.append(getRemoveRelationDropDown(id, relation));
            }
            else if (response.Code >= 50001 && response.Code <= 50004) {
                alert(response.Message);
            }
            else
                alert("An Error Occurred, Please Try Again Later!");
        }).fail(function () { alert("An Error Occurred, Please Try Again Later!"); });
}

function removeRelation(id) {
    var self = this;
    $.post("/Citizen/RemoveRelation", { relativeId: id }).done(
        function () {
            if ($(self).parent().parent().parent().hasClass("dropdown")){
                var parentElement = $(self).parent().parent().parent().parent();
                parentElement.children(".dropdown").remove();
                parentElement.append(getMarkAsDropDown(id));
            }
            else {
                $(self).parent().parent().parent().parent().remove();
            }
        }).fail(function () { alert("An Error Occurred, Please Try Again Later!"); });
}