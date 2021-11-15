document.body.addEventListener('click', function (e) {
    var target = e.target;
    if (target.getAttribute("id").includes("descriptionButton")) {
        var id = target.getAttribute("id");
        var button = document.getElementById(id);
        var text = button.innerText === "Pokaż opis" ? "Ukryj opis" : "Pokaż opis";
        button.innerText = text;
    }
    e.stopPropagation();
});