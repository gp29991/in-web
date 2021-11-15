$(function () {
    $("#datepicker").datepicker();
});

$(document).ready(function () {
    if (document.getElementById("datepicker").value === "") {
        $("#datepicker").datepicker("setDate", new Date());
    }
    if (document.getElementById("datepicker").value.includes("00:00:00")) {
        var date = document.getElementById("datepicker").value.substring(0, 10);
        $("#datepicker").datepicker("setDate", date);
    }
    document.getElementById("editForm").action = "/Finances/EditEntry/" + document.getElementById("identification").value;
});