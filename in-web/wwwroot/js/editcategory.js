document.addEventListener("DOMContentLoaded", function () {
    document.getElementById("editForm").action = "/Category/EditCategory/" + document.getElementById("identification").value;
});