var chckclick = 0;
$(document).ready(function () {
    // jssor_slider2_starter('slider2_container');
    //code start for calender

    var today1 = new Date();
    var dd1 = today1.getDate();
    var mm1 = today1.getMonth() + 1; //January is 0!
    var yyyy1 = today1.getFullYear();

    if (dd1 < 10) {
        dd1 = '0' + dd1
    }

    if (mm1 < 10) {
        mm1 = '0' + mm1
    }

    today1 = dd1 + '/' + mm1 + '/' + yyyy1;
    $('#depdate').val(today1);
    // $('#outdatepicker').val(today);
    $('#retdate').val(today1);
    $('#depdate').datepicker({
        minDate: 0, dateFormat: 'dd/mm/yy', numberOfMonths: 3,
        onSelect: function (dateStr) {
            var d = $.datepicker.parseDate('dd/mm/yy', dateStr);

            $('#retdate').datepicker('setDate', d);
            var start = $(this).datepicker('getDate');

            $('#retdate').datepicker('option', 'minDate', new Date(start.getTime()));

        }
    });

    $('#retdate').datepicker({
        minDate: 0, dateFormat: 'dd/mm/yy', numberOfMonths: 3,

    });
    $("#oricity").autocomplete({
        source: function (request, response) {
            $.ajax({
                url: "/flight/GetSector",
                data: { searchTerm: request.term },
                dataType: "json",
                type: "GET",
                contentType: "application/json; charset=utf-8",
                dataFilter: function (data) { return data; },
                success: function (data) {
                    
                    response($.map(data, function (item) {
                        return {
                            label: item,
                            val: item
                        }
                        //return { value: item.Sector }

                    }))
                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    alert(errorThrown);
                }
            });
        },
        minLength: 3,    // MINIMUM 1 CHARACTER TO START WITH.
        select: function (event, ui) {
            $(this).val(ui.item.label);
            $("#desticity").focus();
            // event.preventDefault();
            $('#maindvbx').text($("#oricity").val());
            $.fn.maindvbx = function () {
                var text = this.text().trim().split(" ");
                var first = text.shift();
                //this.html((text.length > 0 ? "<h1 id='maindvbx'>" + first + "</h1> " : first) + text.join(" "));
                this.html((text.length > 0 ? "" + first + "" : first));
            };
            $("#maindvbx").maindvbx();
        }
    });

    $("#desticity").autocomplete({
        source: function (request, response) {
            $.ajax({
                url: "/flight/GetSector",
                data: { searchTerm: request.term },
                dataType: "json",
                type: "GET",
                contentType: "application/json; charset=utf-8",
                dataFilter: function (data) { return data; },
                success: function (data) {
                    response($.map(data, function (item) {
                        console.log(item);
                        
                        return {
                            label: item,
                            val: item
                        }
                        //return { value: item.Sector }

                    }))
                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    alert(errorThrown);
                }
            });
        },
        minLength: 3,    // MINIMUM 1 CHARACTER TO START WITH.
        select: function (event, ui) {
            console.log(event);
            
            $(this).val(ui.item.label);
            $("#FlightSearch_txtDepartureDate").focus();
            event.preventDefault();
        }
    });




    $('#odr').click(function () {
        //alert('clcikec');
        $("#depdate").focus();
    });
    $('#ord').click(function () {
        //alert('clcikec');
        $("#retdate").focus();
    });
    //classdata();
});


function checkload() {

    document.getElementById('oneway').checked = "true";
    document.getElementById('eco').checked = true;
    $(".dat").css("opacity", "0.6");
}
function radioc() {

    document.getElementById('eco').checked = true;

}
function checkdiable() {

    

    document.getElementById('retdate').disabled = true;
    $(".dat").css("opacity", "0.6");

}
function checkdiable1() {



    document.getElementById('retdate').disabled = false;
    $(".dat").css("opacity", "1");

}
function checkdiable2() {
    document.getElementById('retdate').disabled = false;
    $(".dat").css("opacity", "1");
}


var adult_value = 1;
var child_value = 0;
var infant_value = 0
//$(document).ready(function () {
//    document.getElementById('eco').checked = true;
//    classdata();
//});
function checksum(value, effectedvalue) {

    var type = value.id.split('_')[1];
    var iddata = value.id.split('_')[0];
    if (type.toUpperCase() == 'PLUS') {

        if (iddata == "infant") {

            var abcd = document.getElementById('infant').value;
            if (abcd < adult_value) {
                abcd++;
                document.getElementById(iddata).value = abcd;
            }
        }
        if ((adult_value + child_value) < 9) {
            if (iddata == "adult") {
                adult_value++;
                document.getElementById(iddata).value = adult_value;

            }
            if (iddata == "child") {
                child_value++;
                document.getElementById(iddata).value = child_value;
            }

        }

    }
    else if (type.toUpperCase() == 'MINUS') {
        if ((adult_value + child_value) > 1) {
            if (iddata == "adult") {
                if (adult_value > 1) {
                    adult_value--;
                    document.getElementById(iddata).value = adult_value;
                    document.getElementById('infant').value = 0;
                }
            }
            if (iddata == "child") {
                if (child_value > 0) {
                    child_value--;
                    document.getElementById(iddata).value = child_value;
                }
            }
        }
        if (iddata == "infant") {
            var abcd = document.getElementById('infant').value;
            if (abcd > 0) {
                abcd--;
                document.getElementById(iddata).value = abcd;
            }

        }
    }
    //classdata();
}
window.checksum = checksum


function classdata() {
    
    var checkeddata = "";

    if (document.getElementById('eco').checked == true) {
        checkeddata = "Economy(Y)";
    }

    else if (document.getElementById('busi').checked == true) {
        checkeddata = "Business(C)";
    }

    else {

    }
    document.getElementById('result').innerHTML = (parseInt(document.getElementById('adult').value) + parseInt(document.getElementById('child').value) + parseInt(document.getElementById('infant').value)) + " Travellers ," + checkeddata;

}

$(document).ready(function () {
    $("#don").click(function () {
        $("#travel_select").hide();
    });
});

$(document).ready(function () {

    $(".Chkval").click(function () {


        this.style.border = "";
    });
});

//..................................................................................................................
//Function add by Amit Start here
//Send Variable to Web fjunction for Avaibility
function validate() {

    var txtFlightCityFrom = document.getElementById('oricity').value;
    var txtFlightCityTo = document.getElementById('desticity').value;
    if (txtFlightCityFrom == "") {
        document.getElementById('oricity').style.border = "1px solid red";
        return false;
    }
    if (txtFlightCityTo == "") {
        document.getElementById('desticity').style.border = "1px solid red";
        return false;
    }
    return true;
}
function getSearchType() {
    
    var _type;
    var rates = document.getElementsByName('r1');
    var rate_value;
    for (var i = 0; i < rates.length; i++) {
        if (rates[i].checked) {
            _type = rates[i].value;
        }
    }
    return _type;
}