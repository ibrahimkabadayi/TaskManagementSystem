function GoBack(pageTitle) {
    if (!pageTitle || pageTitle === "Index") {
        window.location.href = '/Home/Home';
    } else {
        window.location.href = '/Home/' + pageTitle;
    }
}

function ReturnToMenu() {
    window.location.href = '/Home/Home';
}