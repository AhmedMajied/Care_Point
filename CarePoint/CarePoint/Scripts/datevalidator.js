jQuery.validator.addMethod("datevalidator",
    function (value, element, param) {
        var arr = param.split(',');
        var date = parseInt($('select[name='+arr[0]+']').val());
        var month = parseInt($('select[name=' + arr[1] + ']').val());
        var year = parseInt($('select[name=' + arr[2] + ']').val());
        if (isNaN(date) || isNaN(month) || isNaN(year)) {
            return false;
        } else {
            if (date > 31 || date < 1) {
                return false;
            } else if ((month == 4 || month == 6 || month == 9 || month == 11) && date == 31) {
                return false;
            } else if (month == 2) {
                var isleap = (year % 4 == 0 && (year % 100 != 0 || year % 400 == 0));
                if (date > 29 || (date == 29 && !isleap))
                    return false;
            }
            if (month > 12 || month < 1) {
                return false;
            }
            if (year > 2050 || year < 1900) {
                return false;
            }
        }
        return true;
    });

jQuery.validator.unobtrusive.adapters.add
    ("datevalidator", ["param"], function (options) {
        options.rules["datevalidator"] = options.params.param;
        options.messages["datevalidator"] = options.message;
    });

$("#iselect-day,#iselect-month,#iselect-year").change(function () {
    $("#iselect-day").valid();
});