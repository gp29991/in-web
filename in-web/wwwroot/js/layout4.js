$(function () {
    $("#startdatepicker").datepicker({ onSelect: function () { $("#enddatepicker").datepicker("option", "minDate", $("#startdatepicker").datepicker("getDate")); } });
    $("#enddatepicker").datepicker();
});
$(document).ready(function () {
    $("#enddatepicker").datepicker("option", "minDate", $("#startdatepicker").datepicker("getDate"));
});

document.getElementById("viewPeriod").addEventListener("change", function () {
    var style = this.value === "other" ? "visible" : "hidden";
    document.getElementById("startDateLabel").style.visibility = style;
    document.getElementById("startDatePicker").style.visibility = style;
    document.getElementById("endDateLabel").style.visibility = style;
    document.getElementById("endDatePicker").style.visibility = style;
    var days = parseInt(this.value);
    if (!isNaN(days)) {
        if (days < 0) {
            $("#startdatepicker").datepicker("setDate", days);
            $("#enddatepicker").datepicker("setDate", new Date());
        } else {
            $("#startdatepicker").datepicker("setDate", new Date());
            $("#enddatepicker").datepicker("setDate", days);
        }
        $("#enddatepicker").datepicker("option", "minDate", $("#startdatepicker").datepicker("getDate"));
    }
});
document.addEventListener("DOMContentLoaded", function () {
    if (document.getElementById("viewPeriod").value === "other") {
        document.getElementById("startDateLabel").style.visibility = "visible";
        document.getElementById("startDatePicker").style.visibility = "visible";
        document.getElementById("endDateLabel").style.visibility = "visible";
        document.getElementById("endDatePicker").style.visibility = "visible";
    }
});