$(function () {
    $("#datepicker").datepicker();
});

$(document).ready(function () {
    if (document.getElementById("datepicker").value === "") {
        $("#datepicker").datepicker("setDate", new Date());
    }
});