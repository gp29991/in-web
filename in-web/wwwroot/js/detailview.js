document.body.addEventListener("click", function (e) {
    var target = e.target;
    if (target.getAttribute("id").includes("descriptionButton")) {
        var id = target.getAttribute("id");
        var button = document.getElementById(id);
        var text = button.innerText === "Pokaż opis" ? "Ukryj opis" : "Pokaż opis";
        button.innerText = text;
    }
    if (target.getAttribute("id").includes("deleteButton")) {
        var baseid = target.getAttribute("id").substring(12);
        var amount = document.getElementById("amount" + baseid).innerText;
        var date = document.getElementById("date" + baseid).innerText;
        var category = document.getElementById("category" + baseid).innerText;
        var name = document.getElementById("name" + baseid).innerText;
        var description = document.getElementById("descriptionRow" + baseid).innerText;
        document.getElementById("deleteDescription").innerHTML = "Kwota: " + amount + "<br>"
            + "Data: " + date + "<br>"
            + "Kategoria: " + category + "<br>"
            + "Nazwa: " + name + "<br>"
            + "Opis: " + description + "<br>";
        document.getElementById("deleteConfirmForm").action = "/Finances/DeleteEntry/" + baseid;
    }
    e.stopPropagation();
});