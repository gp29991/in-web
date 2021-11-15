document.body.addEventListener("click", function (e) {
    var target = e.target;
    if (target.getAttribute("id").includes("deleteButton")) {
        var baseid = target.getAttribute("id").substring(12);
        var category = document.getElementById("category" + baseid).innerText;
        document.getElementById("deleteDescription").innerHTML = "Czy na pewno chcesz usunąć kategorię \"" + category
            + "\" oraz <strong>wszystkie przypisane do niej wpisy</strong>?"
        document.getElementById("deleteConfirmForm").action = "/Category/DeleteCategory/" + baseid;
    }
    if (target.getAttribute("id").includes("clearButton")) {
        var name = target.getAttribute("id").substring(11);
        document.getElementById("deleteDescription").innerHTML = "Czy na pewno chcesz usunąć "
            + "<strong>wszystkie wpisy przypisane do kategorii domyślnej \"" + name + "\"</strong>?"
        document.getElementById("deleteConfirmForm").action = "/Category/ClearDefaultCategoryEntries?defcat=" + name;
    }
    e.stopPropagation();
});

var currentUrl = new URL(window.location.href);
currentUrl.searchParams.delete("showAlert");
window.history.replaceState("removealertparam", "Widok szczegółowy", currentUrl);