function finalbookOne_n() {
    debugger;
    var cmpid = document.getElementById("hdncmpid").value;
    if (cmpid === "" || cmpid.indexOf("C-") > -1) {
        window.location.href = "n_flight_detail.aspx";
    }
    else {
        window.location.href = "n_FlightDisplay.aspx";
    }
}

function finalbookround_n() {
    debugger;
    var cmpid = document.getElementById("hdncmpid").value;
    if (cmpid === "" || cmpid.indexOf("C-") > -1) {
        window.location.href = "n_flight_detail.aspx";
    }
    else {
        window.location.href = "n_FlightDisplay.aspx";
    }
}

function finalbookrt_n() {
    debugger;
    var cmpid = document.getElementById("hdncmpid").value;
    if (cmpid === "" || cmpid.indexOf("C-") > -1) {
        window.location.href = "n_flight_detail.aspx";
    }
    else {
        window.location.href = "n_FlightDisplay.aspx";
    }
}

function finalbookint_n() {
    debugger;
    var cmpid = document.getElementById("hdncmpid").value;
    if (cmpid === "" || cmpid.indexOf("C-") > -1) {
        window.location.href = "n_flight_detail.aspx";
    }
    else {
        window.location.href = "n_FlightDisplay.aspx";
    }
}

function finalbookMulticity_n() {
    var cmpid = document.getElementById("hdncmpid").value;
    if (cmpid === "" || cmpid.indexOf("C-") > -1) {
        window.location.href = "/flight/Travellers";
    }
    else {
        window.location.href = "/flight/Travellers";
    }
}
window.finalbookMulticity_n = finalbookMulticity_n;


function finalbookOne_k() { 
    var cmpid = document.getElementById("hdncmpid").value;
    var varURL = document.getElementById("hdnURL").value;

    if (varURL.indexOf("tourista") > -1) {
        if (cmpid === "" || cmpid.indexOf("C-") > -1) {
            window.location.href = "flightdetail_desing.aspx";
        }
        else { 
            window.location.href = "/flight/Travellers";
        }
    }
    else {
        if (cmpid === "") {
            window.location.href = "flightdetail_desing.aspx";
        }
        else {
            window.location.href = "/flight/Travellers";
        }
    }
}
window.finalbookOne_k = finalbookOne_k;

function finalbookround_k() {
    debugger;
    var cmpid = document.getElementById("hdncmpid").value;
    var varURL = document.getElementById("hdnURL").value;

    if (varURL.indexOf("tourista") > -1) {
        if (cmpid === "" || cmpid.indexOf("C-") > -1) {
            window.location.href = "flightdetail_desing.aspx";
        }
        else {
            window.location.href = "/flight/Travellers";
        }
    }
    else {
        if (cmpid === "") {
            window.location.href = "flightdetail_desing.aspx";
        }
        else {
            window.location.href = "/flight/Travellers";
        }
    }

    //if (cmpid === "" || cmpid.indexOf("C-") > -1) {
    //    window.location.href = "flightdetail_desing.aspx";
    //}
    //else {
    //    window.location.href = "k_FlightDisplay.aspx";
    //}
}
window.finalbookround_k =finalbookround_k;
function finalbookrt_k() {
    debugger;
    var cmpid = document.getElementById("hdncmpid").value;
    var varURL = document.getElementById("hdnURL").value;


    if (varURL.indexOf("tourista") > -1) {
        if (cmpid === "" || cmpid.indexOf("C-") > -1) {
            window.location.href = "flightdetail_desing.aspx";
        }
        else {
            window.location.href = "k_travellers.aspx";
        }
    }
    else {
        if (cmpid === "") {
            window.location.href = "flightdetail_desing.aspx";
        }
        else {
            window.location.href = "k_travellers.aspx";
        }
    }

    //if (cmpid === "" || cmpid.indexOf("C-") > -1) {
    //    window.location.href = "flightdetail_desing.aspx";
    //}
    //else {
    //    window.location.href = "k_FlightDisplay.aspx";
    //}
}

function finalbookint_k() {
    var cmpid = document.getElementById("hdncmpid").value;
    var varURL = document.getElementById("hdnURL").value;

    if (varURL.indexOf("tourista") > -1) {
        if (cmpid === "" || cmpid.indexOf("C-") > -1) {
            window.location.href = "flightdetail_desing.aspx";
        }
        else {
            window.location.href = "/flight/Travellers";
        }
    }
    else {
        if (cmpid === "") {
            window.location.href = "flightdetail_desing.aspx";
        }
        else {
            window.location.href = "/flight/Travellers";
        }
    }

    //if (cmpid === "" || cmpid.indexOf("C-") > -1) {
    //    window.location.href = "flightdetail_desing.aspx";
    //}
    //else {
    //    window.location.href = "k_FlightDisplay.aspx";
    //}
}
window.finalbookint_k = finalbookint_k;

function finalbookMulticity_k() {
    debugger;
    var cmpid = document.getElementById("hdncmpid").value;
    var varURL = document.getElementById("hdnURL").value;

    if (varURL.indexOf("tourista") > -1) {
        if (cmpid === "" || cmpid.indexOf("C-") > -1) {
            window.location.href = "flightdetail_desing.aspx";
        }
        else {
            window.location.href = "k_travellers.aspx";
        }
    }
    else {
        if (cmpid === "") {
            window.location.href = "flightdetail_desing.aspx";
        }
        else {
            window.location.href = "k_travellers.aspx";
        }
    }

    //if (cmpid === "" || cmpid.indexOf("C-") > -1) {
    //    window.location.href = "flightdetail_desing.aspx";
    //}
    //else {
    //    window.location.href = "k_FlightDisplay.aspx";
    //}
}