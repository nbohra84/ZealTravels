
//============================commonJS/homepage2.js?_tver=0011================//

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
    $('.Mdepartfirst').val(today1);
    $('#Mdepartdate_2').val(today1);
    if (window.matchMedia("(max-width: 700px)").matches) {
        $('#depdate').datepicker({
            minDate: 0, dateFormat: 'dd/mm/yy', numberOfMonths: 1,
            onSelect: function (dateStr) {
                var d = $.datepicker.parseDate('dd/mm/yy', dateStr);
                $('#retdate').datepicker('setDate', d);
                var start = $(this).datepicker('getDate');
                $('#retdate').datepicker('option', 'minDate', new Date(start.getTime()));
            },
            onClose: function (selectedDate) {
                if ($('#retdate').css('display') == 'block') {
                    $("#retdate").focus();
                }
            }

        });
    }
    else {
        $('#depdate').datepicker({
            minDate: 0, dateFormat: 'dd/mm/yy', numberOfMonths: 3,
            onSelect: function (dateStr) {
                var d = $.datepicker.parseDate('dd/mm/yy', dateStr);
                $('#retdate').datepicker('setDate', d);
                var start = $(this).datepicker('getDate');
                $('#retdate').datepicker('option', 'minDate', new Date(start.getTime()));
            },
            onClose: function (selectedDate) {
                if ($('#retdate').css('display') == 'block') {
                    $("#retdate").focus();
                }
            }

        });
    }


    //$('body').on('focus', "#retdate", function () {
    //$('#retdate').datepicker({
    //    minDate: 0, dateFormat: 'dd/mm/yy', numberOfMonths: 3,
    //    onSelect: function (dateStr) {
    //       
    //        if ($('#depdate').val() >= dateStr) {
    //            var d = $.datepicker.parseDate('dd/mm/yy', dateStr);
    //            $('#depdate').datepicker('setDate', d);
    //            var start = $(this).datepicker('getDate');
    //        }
    //    },
    //    onClose: function (selectedDate) {
    //    }

    //});


    //$('#retdate').datepicker({
    //    minDate: 0, dateFormat: 'dd/mm/yy', numberOfMonths: 3
    //});

    if (window.matchMedia("(max-width: 700px)").matches) {
        $('#retdate').datepicker({
            minDate: 0, dateFormat: 'dd/mm/yy', numberOfMonths: 1,
        });
    }
    else {
        $('#retdate').datepicker({
            minDate: 0, dateFormat: 'dd/mm/yy', numberOfMonths: 3,
        });
    }

    //});




    $("#oricity").autocomplete({
        autoFocus: true,
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
        }
    });

    $("#desticity").autocomplete({
        autoFocus: true,
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
            //$("#depdate").focus();

        },
        minLength: 3,    // MINIMUM 1 CHARACTER TO START WITH.    
        select: function (event, ui) {
            $(this).val(ui.item.label);
            $("#depdate").focus();
            // event.preventDefault();
        }
    });

    $('.datetopiccar').datepicker({
        minDate: 0, dateFormat: 'yy-mm-dd', numberOfMonths: 1
    });


     $('body').click(function () {
               
        $('#manual_flightTravelers').css("display", "none");
        //calculatenofpax();
     });
     $('.modal-content, .mduenwe').on('click', function (e) {
         e.stopPropagation();
     });

 


    $('body').on('focus', ".multicitysector", function () {
        $(".multicitysector").autocomplete({
            autoFocus: true,
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
                var txtdata = $(this).attr("id").split("_");
                if (txtdata[0] == "Morigin") {
                    $("#Mdestination_" + txtdata[1]).focus();
                }
                else if (txtdata[0] == "Mdestination") {
                    $("#Mdepartdate_" + txtdata[1]).focus();
                }
            }
        });
    });


    //================== Multicity Calender validation start==================================//

    var mindatesel = today1;
    $("#Mdepartdate_1").datepicker({
        minDate: mindatesel, maxDate: '+1Y',
        selectedDate: '0',
        dateFormat: 'dd/mm/yy',
        numberOfMonths: 3,
        showButtonPanel: true,
        closeText: '',
        onSelect: function (selectedDate) {
           
            //var d = $.datepicker.parseDate('dd/mm/yy', selectedDate);
            //$('#Mdepartdate_2').datepicker('setDate', d);
            //var start = $(this).datepicker('getDate');
            //$('#Mdepartdate_2').datepicker('option', 'minDate', new Date(start.getTime()));
            $('#Mdepartdate_2').val(selectedDate);
           
            if ($('#myCityDiv').html() != "") {
                $('#myCityDiv').find(':input').each(function () {
                    switch (this.type) {
                        case 'text':
                            $(this).val('');
                            break;
                    }
                });
            }
        },
        onClose: function (selectedDate) {

        }
    }).datepicker('setDate', mindatesel);
    $('body').on('focus', ".Mdepart", function () {
       
        // $('.Mdepart').live('focus', function () {
       
        var Id = this.id.split('_')[1];
        var mymindate = '';
        var mymaxdate = '';
        if (Id == 2) {
            mymindate = $.trim($("#Mdepartdate_" + (Id - 1)).val());
            //mymaxdate = '0';
        }
        else {
            mymindate = $.trim($("#Mdepartdate_" + (Id - 1)).val());
            mymaxdate = $.trim($("#Mdepartdate_" + (parseInt(Id) + 1)).val());
        }
        var $this = $(this);
        $this.datepicker({
            minDate: mymindate, maxDate: mymaxdate,
            selectedDate: '0',
            dateFormat: 'dd/mm/yy',
            numberOfMonths: 3,
            showButtonPanel: true,
            closeText: '',
            onSelect: function () {
                if (Id == 3) {
                    mymaxdate = $("#Mdepartdate_" + Id).val();
                    $("#Mdepartdate_" + (Id - 1)).datepicker('option', 'maxDate', mymaxdate);
                }
                else {
                    mymindate = $("#Mdepartdate_" + Id).val();
                    var minforreturn = parseInt(Id) + 1;
                    $("#Mdepartdate_" + minforreturn).datepicker('option', 'minDate', mymindate);
                }
            },
            onClose: function () {

            }

        });

        $this.datepicker("show");
    });
    //================== Multicity Calender validation end==================================//

    $("[id*=select_all]").bind("click",
           function () {
               if ($(this).prop("checked") == true) {
                   $("[id*=IndexSearchEngine2_Chkbocpreferedairline] input").each(function () {
                      
                       $(this).attr("checked", "checked");
                       $("[id*=IndexSearchEngine2_Chkbocpreferedairline] input").prop("checked", true);
                   });
               }
               else if ($(this).prop("checked") == false) {
                   $("[id*=IndexSearchEngine2_Chkbocpreferedairline] input").removeAttr("checked");
               }
               //if ($(this).is(":checked")) {
               //   
               //    $("[id*=IndexSearchEngine2_Chkbocpreferedairline] input").each(function () {
               //       
               //        $(this).attr("checked", "checked"); 
               //    });
               //} else {
               //   
               //    $("[id*=IndexSearchEngine2_Chkbocpreferedairline] input").removeAttr("checked");
               //}
           });


    function radioc() {
        document.getElementById('eco').checked = true;
    }

    $('#odr').click(function () {
        //alert('clcikec');
        $("#depdate").focus();
    });
    $('#ord').click(function () {
        //alert('clcikec');
        $("#retdate").focus();
    });
    classdata();


	
});//===== end doc ready 


function checkload() {
    document.getElementById('oneway').checked = "true";
    document.getElementById('eco').checked = true;
    $(".dat").css("opacity", "0.6");
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

// function checksum(value, effectedvalue) {

//     var type = value.id.split('_')[1];
//     var iddata = value.id.split('_')[0];
//     if (type.toUpperCase() == 'PLUS') {

//         if (iddata == "infant") {

//             var abcd = document.getElementById('infant').value;
//             if (abcd < adult_value) {
//                 abcd++;
//                 document.getElementById(iddata).value = abcd;
//             }
//         }
//         if ((adult_value + child_value) < 9) {
//             if (iddata == "adult") {
//                 adult_value++;
//                 document.getElementById(iddata).value = adult_value;

//             }
//             if (iddata == "child") {
//                 child_value++;
//                 document.getElementById(iddata).value = child_value;
//             }

//         }

//     }
//     else if (type.toUpperCase() == 'MINUS') {
//         if ((adult_value + child_value) > 1) {
//             if (iddata == "adult") {
//                 if (adult_value > 1) {
//                     adult_value--;
//                     document.getElementById(iddata).value = adult_value;
//                     document.getElementById('infant').value = 0;
//                 }
//             }
//             if (iddata == "child") {
//                 if (child_value > 0) {
//                     child_value--;
//                     document.getElementById(iddata).value = child_value;
//                 }
//             }
//         }
//         if (iddata == "infant") {
//             var abcd = document.getElementById('infant').value;
//             if (abcd > 0) {
//                 abcd--;
//                 document.getElementById(iddata).value = abcd;
//             }

//         }
//     }
//     classdata();
// }



function classdata() {
   
    var checkeddata = "";

    var ecoElement = document.getElementById('eco');
    var busiElement = document.getElementById('busi');

    if (ecoElement && ecoElement.checked == true) {
        checkeddata = "Economy(Y)";
    }

    else if (busiElement && busiElement.checked == true) {
        checkeddata = "Business(C)";
    }

    else {

    }
    
    var resultElement = document.getElementById('result');
    var adultElement = document.getElementById('adult');
    var childElement = document.getElementById('child');
    var infantElement = document.getElementById('infant');
    
    if (resultElement && adultElement && childElement && infantElement) {
        var adultValue = parseInt(adultElement.value) || 0;
        var childValue = parseInt(childElement.value) || 0;
        var infantValue = parseInt(infantElement.value) || 0;
        resultElement.innerHTML = (adultValue + childValue + infantValue) + " Travellers ," + checkeddata;
    }

}
window.classdata=classdata;
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
    if (txtFlightCityFrom == txtFlightCityTo) {
        alert("Departure and Arrival Location can not same.Kindly change your search criteria!");
        return false;
    }
    var Adlt = $('#adult').val();
    var child = $('#child').val();
    var Inf = $('#infant').val();
    var totalpax = parseInt(Adlt) + parseInt(child) + parseInt(Inf);
    if (totalpax > 9) {
        alert('Total passengers are greater than 9.Kindly change your search criteria!');
        return false;
    }
    //check infant
    var x = parseInt(Inf);
    var t = parseInt(Adlt);
    if (x > t) {
        alert("Total adult should not be less than infant!.");
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

//================================Old Result Page Start===========================

//function getresult() {
//   
//    var rseultvalidation = validate()
//    if (rseultvalidation == true) {
//        var AirLines = "";
//        // Circular loader removed - using progress bar instead
        // $('#CenterwaitingDiv').css("display", "block");
//        $('#depCityWaiting').text($("#oricity").val());
//        $('#ArrCityWaiting').text($("#desticity").val());
//        var serchctydate = $("#oricity").val() + "!" + $("#desticity").val() + "#" + $("#depdate").val() + "," + $("#retdate").val();
//        $('#departdate').text($("#depdate").val());
//        $('#returndate').text($("#retdate").val());
//       
//        var PreferedList = [];
//        //PreferedList = PreferedAirLine();
//        $("[id*=Chkbocpreferedairline] input:checked").each(function () {
//            PreferedList.push($(this).val());
//        });
//        AirLines = PreferedList.toString();
//        // }
//        var spclfare = 0;
//        if (document.getElementById('Specialfa').checked == true) {
//            spclfare = 1;
//        }
//        var travelClass = $('#ddltravelClass').val();
//        var mySelection = getSearchType();
//        if (mySelection == "O") {
//            $('#departdate').text($("#datepicker").val());
//            // $('#returndate').text($("#datepicker").val());
//            $('#paxty').text($("#infant").val());
//            $('#paxty1').text($("#adult").val());
//            $('#paxty2').text($("#child").val());
//            document.getElementById('oneshoi').style.display = "block";
//            document.getElementById('rshoi').style.display = "none";
//            document.getElementById('returndate').style.display = "none";
//        }
//        if (mySelection == "R") {
//            $('#departdate').text($("#datepicker").val());
//            $('#returndate').text($("#outdatepicker1").val());
//            $('#paxty').text($("#infant").val());
//            $('#paxty1').text($("#adult").val());
//            $('#paxty2').text($("#child").val());
//            document.getElementById('oneshoi').style.display = "none";
//            document.getElementById('rshoi').style.display = "block";
//        }
//        if (mySelection == "DRT") {
//            $('#departdate').text($("#datepicker").val());
//            $('#returndate').text($("#outdatepicker1").val());
//            $('#paxty').text($("#infant").val());
//            $('#paxty1').text($("#adult").val());
//            $('#paxty2').text($("#child").val());
//            document.getElementById('oneshoi').style.display = "none";
//            document.getElementById('rshoi').style.display = "block";
//        }
//        var params = "";
//        params = [{ "_DepartureStation": $("#oricity").val(), "_ArrivalStation": $("#desticity").val(), "_BeginDate": $("#depdate").val(), "_EndDate": $("#retdate").val(), "_NoOfadult": $("#adult").val(), "_NoOfChild": $("#child").val(), "_NoOfInfant": $("#infant").val(), "_SearchType": $("input[name='r1']:checked").val(), "_PreferedAirlines": AirLines, "_TravelClass": travelClass, "_SpecialFare": spclfare, "_Place": "I" }];
//        $.ajax({
//            type: "POST",
//            url: "LandingPage.aspx/GetvalueForRequest",
//            data: "{'objclsPSQ':" + JSON.stringify(params) + "}",
//            contentType: "application/json; charset=utf-8", //Set Content-Type
//            dataType: "json", // Set return Data Type
//            cache: false,
//            success: function (result) {
//               
//                var hostName = $('#hdnhostName').val();
//                //alert(hostName);
//                var obj = result.d;
//                if (obj != "") {
//                    if (mySelection == "O") {
//                        location.href = "//" + hostName + "/FlightSearch.aspx";
//                    }
//                    else if (obj == "D") {
//                        location.href = "//" + hostName + "/FlightSearch-Round.aspx";
//                    }
//                    else if (obj == "I") {
//                        //location.href = "//" + hostName + "/Myproject/FlightSearchInt.aspx";
//                        // location.href = "//" + hostName + "/FlightSearchInt.aspx?value=" + btoa(serchctydate);
//                        location.href = "//" + hostName + "/FlightSearchInt.aspx";
//                    }
//                    else if (mySelection == "DRT") {
//                        location.href = "//" + hostName + "/DiscRT.aspx";
//                    }
//                }
//                else {
//                    location.href = "//" + hostName + "/modifysearch.aspx";
//                }
//            },
//            error: function (xhr, msg, e) {
//               
//                alert("getresult()\n" + msg + "  " + e + " " + xhr);
//                location.href = "//" + hostName + "/Index.aspx";
//            }
//        });
//    }
//    else { }
//}

//================================Old Result Page End=============================

//================================N Result Page Start=============================

function getresult() {
   
    $('#manual_flightTravelers').css("display", "none");
    var rseultvalidation = validate()
    if (rseultvalidation == true) {
        var AirLines = "";                
        // Circular loader removed - using progress bar instead
        // $('#CenterwaitingDiv').css("display", "block");
        $('#depCityWaiting').text($("#oricity").val());
        $('#ArrCityWaiting').text($("#desticity").val());
        var serchctydate = $("#oricity").val() + "!" + $("#desticity").val() + "#" + $("#depdate").val() + "," + $("#retdate").val();
        $('#departdate').text($("#depdate").val());
        $('#returndate').text($("#retdate").val());
       
        var PreferedList = [];
        //PreferedList = PreferedAirLine();
        $("[id*=Chkbocpreferedairline] input:checked").each(function () {
            PreferedList.push($(this).val());
        });
        AirLines = PreferedList.toString();
        // }
        var spclfare = 0;
        if (document.getElementById('Specialfa')!=null && document.getElementById('Specialfa').checked == true) {
            spclfare = 1;
        }
        var travelClass = $('#ddltravelClass').val();
        var mySelection = getSearchType();
        if (mySelection == "O") {
            $('#departdate').text($("#datepicker").val());
            // $('#returndate').text($("#datepicker").val());
            $('#paxty').text($("#infant").val());
            $('#paxty1').text($("#adult").val());
            $('#paxty2').text($("#child").val());
            document.getElementById('oneshoi')!=null?document.getElementById('oneshoi').style.display = "block":"";
            document.getElementById('rshoi')!=null?document.getElementById('rshoi').style.display = "none":"";
            document.getElementById('returndate')!=null?document.getElementById('returndate').style.display = "none":"";
        }
        if (mySelection == "R") {
            $('#departdate').text($("#datepicker").val());
            $('#returndate').text($("#outdatepicker1").val());
            $('#paxty').text($("#infant").val());
            $('#paxty1').text($("#adult").val());
            $('#paxty2').text($("#child").val());
            document.getElementById('oneshoi')!=null?document.getElementById('oneshoi').style.display = "none":"";
            document.getElementById('rshoi')!=null?document.getElementById('rshoi').style.display="block":"";
        }
        if (mySelection == "DRT") {
            $('#departdate').text($("#datepicker").val());
            $('#returndate').text($("#outdatepicker1").val());
            $('#paxty').text($("#infant").val());
            $('#paxty1').text($("#adult").val());
            $('#paxty2').text($("#child").val());
            document.getElementById('oneshoi')!=null?document.getElementById('oneshoi').style.display = "none":"";
            document.getElementById('rshoi')!=null?document.getElementById('rshoi').style.display = "block":"";
        }
        var hostName = $('#hdnhostName').val();
        // Use relative URL to avoid CORS issues - always use current origin
        var requestUrl = "/Flight/GetvalueForRequest";
        var params = "";
        params = [{ "_DepartureStation": $("#oricity").val(), "_ArrivalStation": $("#desticity").val(), "_BeginDate": $("#depdate").val(), "_EndDate": $("#retdate").val(), "_NoOfAdult": $("#adult").val(), "_NoOfChild": $("#child").val(), "_NoOfInfant": $("#infant").val(), "_SearchType": $("input[name='r1']:checked").val(), "_PreferedAirlines": AirLines, "_TravelClass": travelClass, "_SpecialFare": spclfare, "_Place": "I" }];        
        $.ajax({
            type: "POST",
            url: requestUrl,
            data: JSON.stringify({ objclsPSQ: params }),
            contentType: "application/json; charset=utf-8", //Set Content-Type
            dataType: "json", // Set return Data Type
            cache: false,
            success: function (result) {
               
                // var hostName = $('#hdnhostName').val();   
                var obj = result.d;
                
                console.log('[Progressive] Search response received:', {
                    d: obj,
                    progressive: result.progressive,
                    searchId: result.searchId,
                    fullResult: result
                });
                
                // Check if progressive loading is enabled (one-way and round trip)
                if (obj != "") {
                    if (obj == "O") {
                        var url = "//" + hostName + "/Flight/OneWay";
                        // For progressive loading: add searchId if available
                        if (result.progressive && result.searchId) {
                            url += "?searchId=" + encodeURIComponent(result.searchId);
                            sessionStorage.setItem('progressiveSearchId', result.searchId);
                        }
                        location.href = url;
                    }
                    else if (obj == "D") {
                        var url = "//" + hostName + "/flight/round";
                        // For progressive loading: add searchId if available
                        if (result.progressive && result.searchId) {
                            url += "?searchId=" + encodeURIComponent(result.searchId);
                            sessionStorage.setItem('progressiveSearchId', result.searchId);
                            console.log('[Progressive RT] Redirecting to round trip with searchId:', result.searchId);
                        }
                        location.href = url;
                    }
                    else if (obj == "I") {
                        var url = "//" + hostName + "/flight/int";
                        // For progressive loading: add searchId if available
                        if (result.progressive && result.searchId) {
                            url += "?searchId=" + encodeURIComponent(result.searchId);
                            sessionStorage.setItem('progressiveSearchId', result.searchId);
                            console.log('[Progressive RT] Redirecting to international round trip with searchId:', result.searchId);
                        }
                        location.href = url;
                    }
                    // else if (obj == "DRT") {
                    //     location.href = "//" + hostName + "/k_rt.aspx";
                    // }
                }
                else {
                    if (mySelection == "O") {
                        var url = "//" + hostName + "/Flight/OneWay";
                        // For progressive loading: add searchId if available
                        if (result.progressive && result.searchId) {
                            url += "?searchId=" + encodeURIComponent(result.searchId);
                            sessionStorage.setItem('progressiveSearchId', result.searchId);
                        }
                        location.href = url;
                    }
                else if (mySelection == "R") {
                    var url = "//" + hostName + "/flight/round";
                    // Check if we have searchId in sessionStorage for progressive loading
                    var storedSearchId = sessionStorage.getItem('progressiveSearchId');
                    if (storedSearchId) {
                        url += "?searchId=" + encodeURIComponent(storedSearchId);
                        console.log('[Progressive RT] Error handler - Using stored searchId:', storedSearchId);
                    }
                    location.href = url;
                }
                else if (mySelection == "DRT") {
                    var url = "//" + hostName + "/flight/int";
                    // Check if we have searchId in sessionStorage for progressive loading
                    var storedSearchId = sessionStorage.getItem('progressiveSearchId');
                    if (storedSearchId) {
                        url += "?searchId=" + encodeURIComponent(storedSearchId);
                        console.log('[Progressive RT] Error handler - Using stored searchId:', storedSearchId);
                    }
                    location.href = url;
                }
                    // else if (mySelection == "MRT") {
                    //     location.href = "//" + hostName + "/k_int.aspx";
                    // }
                    // else{
                    //     location.href = "//" + hostName + "/Flight/OneWay";
                    // }
                }
            },
            error: function (xhr, msg, e) {
                // Extract proper error message from response
                var errorMsg = msg || "Unknown error";
                var errorDetails = "";
                
                if (xhr.responseJSON) {
                    if (xhr.responseJSON.error) {
                        errorMsg = xhr.responseJSON.error;
                    }
                    if (xhr.responseJSON.details) {
                        errorDetails = "\nDetails: " + xhr.responseJSON.details;
                    } else if (xhr.responseJSON.modelState && Array.isArray(xhr.responseJSON.modelState)) {
                        var modelErrors = xhr.responseJSON.modelState.map(function(item) {
                            return item.key + ": " + (item.errors ? item.errors.join(", ") : "");
                        }).join("; ");
                        if (modelErrors) {
                            errorDetails = "\nModel Errors: " + modelErrors;
                        }
                    }
                } else if (xhr.responseText) {
                    try {
                        var errorObj = JSON.parse(xhr.responseText);
                        errorMsg = errorObj.error || errorMsg;
                        if (errorObj.details) {
                            errorDetails = "\nDetails: " + errorObj.details;
                        }
                    } catch (parseError) {
                        errorMsg = xhr.responseText || xhr.statusText || errorMsg;
                    }
                }
                
                console.error("getresult() Error:", {
                    message: errorMsg,
                    details: errorDetails,
                    status: xhr.status,
                    statusText: xhr.statusText,
                    response: xhr.responseJSON || xhr.responseText
                });
                
                alert("getresult()\nError: " + errorMsg + errorDetails + "\nStatus: " + (xhr.status || "N/A"));
                
                // Redirect to OneWay page instead of Index.aspx
                var mySelection = getSearchType();
                if (mySelection == "O") {
                    location.href = "//" + hostName + "/Flight/OneWay";
                }
                else if (mySelection == "R") {
                    location.href = "//" + hostName + "/Flight/round";
                }
                else if (mySelection == "DRT") {
                    location.href = "//" + hostName + "/Flight/Int";
                }
                else {
                    location.href = "//" + hostName + "/Flight/OneWay";
                }
            }
        });
    }
    else { }
}
window.getresult=getresult;
//================================N Result Page End===============================





//================================k Result Page Start=============================

//function getresult() {
//   
//    var rseultvalidation = validate()
//    if (rseultvalidation == true) {
//        var AirLines = "";
//        // Circular loader removed - using progress bar instead
        // $('#CenterwaitingDiv').css("display", "block");
//        $('#depCityWaiting').text($("#oricity").val());
//        $('#ArrCityWaiting').text($("#desticity").val());
//        var serchctydate = $("#oricity").val() + "!" + $("#desticity").val() + "#" + $("#depdate").val() + "," + $("#retdate").val();
//        $('#departdate').text($("#depdate").val());
//        $('#returndate').text($("#retdate").val());
//       
//        var PreferedList = [];
//        //PreferedList = PreferedAirLine();
//        $("[id*=Chkbocpreferedairline] input:checked").each(function () {
//            PreferedList.push($(this).val());
//        });
//        AirLines = PreferedList.toString();
//        // }
//        var spclfare = 0;
//        if (document.getElementById('Specialfa').checked == true) {
//            spclfare = 1;
//        }
//        var travelClass = $('#ddltravelClass').val();
//        var mySelection = getSearchType();
//        if (mySelection == "O") {
//            $('#departdate').text($("#datepicker").val());
//            // $('#returndate').text($("#datepicker").val());
//            $('#paxty').text($("#infant").val());
//            $('#paxty1').text($("#adult").val());
//            $('#paxty2').text($("#child").val());
//            document.getElementById('oneshoi').style.display = "block";
//            document.getElementById('rshoi').style.display = "none";
//            document.getElementById('returndate').style.display = "none";
//        }
//        if (mySelection == "R") {
//            $('#departdate').text($("#datepicker").val());
//            $('#returndate').text($("#outdatepicker1").val());
//            $('#paxty').text($("#infant").val());
//            $('#paxty1').text($("#adult").val());
//            $('#paxty2').text($("#child").val());
//            document.getElementById('oneshoi').style.display = "none";
//            document.getElementById('rshoi').style.display = "block";
//        }
//        if (mySelection == "DRT") {
//            $('#departdate').text($("#datepicker").val());
//            $('#returndate').text($("#outdatepicker1").val());
//            $('#paxty').text($("#infant").val());
//            $('#paxty1').text($("#adult").val());
//            $('#paxty2').text($("#child").val());
//            document.getElementById('oneshoi').style.display = "none";
//            document.getElementById('rshoi').style.display = "block";
//        }
//        var hostName = $('#hdnhostName').val();
//        var params = "";
//        params = [{ "_DepartureStation": $("#oricity").val(), "_ArrivalStation": $("#desticity").val(), "_BeginDate": $("#depdate").val(), "_EndDate": $("#retdate").val(), "_NoOfadult": $("#adult").val(), "_NoOfChild": $("#child").val(), "_NoOfInfant": $("#infant").val(), "_SearchType": $("input[name='r1']:checked").val(), "_PreferedAirlines": AirLines, "_TravelClass": travelClass, "_SpecialFare": spclfare, "_Place": "I" }];
//        $.ajax({
//            type: "POST",
//            url: "//" + hostName + "/k_one.aspx/GetvalueForRequest",
//            data: "{'objclsPSQ':" + JSON.stringify(params) + "}",
//            contentType: "application/json; charset=utf-8", //Set Content-Type
//            dataType: "json", // Set return Data Type
//            cache: false,
//            success: function (result) {
//               
//                // var hostName = $('#hdnhostName').val();   
//                var obj = result.d;
//                if (obj != "") {
//                    if (mySelection == "O") {
//                        location.href = "//" + hostName + "/k_one.aspx";
//                    }
//                    else if (obj == "D") {
//                        location.href = "//" + hostName + "/k_round.aspx";
//                    }
//                    else if (obj == "I") {
//                        location.href = "//" + hostName + "/k_int.aspx";
//                    }
//                    else if (mySelection == "DRT") {
//                        location.href = "//" + hostName + "/k_rt.aspx";
//                    }
//                }
//                else {
//                    location.href = "//" + hostName + "/modifysearch.aspx";
//                }
//            },
//            error: function (xhr, msg, e) {
//               
//                alert("getresult()\n" + msg + "  " + e + " " + xhr);
//                location.href = "//" + hostName + "/Index3.aspx";
//            }
//        });
//    }
//    else { }
//}

//================================k Result Page End===============================



function PreferedAirLine() {

    var values = [];
    var listBox = document.getElementById("lstFruits");
    for (var i = 0; i < listBox.options.length; i++) {
        if (listBox.options[i].selected) {
            values.push(listBox.options[i].value);
            //values += listBox.options[i].innerHTML + " " + listBox.options[i].value + "\n";
        }
    }
    return values;
}
function prefered() {

    if (document.getElementById('pref').checked == true) {
        document.getElementById('pfair').style.display = "block";
    }
    else {
        document.getElementById('pfair').style.display = "none";
    }

}
window.prefered=prefered;


function oneshow() {
    //debugger;
    document.getElementById('fltOWRW').style.display = "block"
    document.getElementById('Trip2').checked = false;
    document.getElementById('Trip1').checked = true;
    document.getElementById('Trip3').checked = false;
    document.getElementById('Trip4').checked = false;
    $('#oneshowa').removeClass("whcolor");
    $('#oneshowa').addClass("gbcolor");
    $('#roundshowa').addClass("whcolor");
    $('#DiscRTa').addClass("whcolor");
    $('#DiscRTa').removeClass("gbcolor");
    $('#roundshowa').removeClass("gbcolor");

    $('#multiRT').addClass("whcolor");
    $('#multiRT').removeClass("gbcolor");
    $(".dhirajK").removeClass("dhirajK").addClass("mybxdighi");
    document.getElementById('retdate').style.display = "none";
    document.getElementById('Fltnotmulticitybtn').style.display = "block";
    document.getElementById('multicitybtn').style.display = "none";
    document.getElementById('multicitydiv').style.display = "none";
	
	// getApiTokenListBackGround();
}
function roundshow() {
   
    document.getElementById('fltOWRW').style.display = "block"
    document.getElementById('Trip2').checked = true;
    document.getElementById('Trip1').checked = false;
    document.getElementById('Trip3').checked = false;
    document.getElementById('Trip4').checked = false;

    $('#oneshowa').removeClass("gbcolor");
    $('#oneshowa').addClass("whcolor");
    $('#roundshowa').removeClass("whcolor");
    $('#roundshowa').addClass("gbcolor");
    $('#DiscRTa').addClass("whcolor");
    $('#DiscRTa').removeClass("gbcolor");

    $('#multiRT').addClass("whcolor");
    $('#multiRT').removeClass("gbcolor");
    $(".dhirajK").removeClass("dhirajK").addClass("mybxdighi");
    document.getElementById('retdate').style.display = "block";
    document.getElementById('Fltnotmulticitybtn').style.display = "block";
    document.getElementById('multicitybtn').style.display = "none";
    document.getElementById('multicitydiv').style.display = "none";
	
	// getApiTokenListBackGround();
}
function DiscRTshow() {
   
    document.getElementById('fltOWRW').style.display = "block";
    document.getElementById('Trip3').checked = true;
    document.getElementById('Trip1').checked = false;
    document.getElementById('Trip2').checked = false;
    document.getElementById('Trip4').checked = false;

    $('#oneshowa').addClass("whcolor");
    $('#roundshowa').addClass("whcolor");
    $('#roundshowa').removeClass("gbcolor");
    $('#DiscRTa').addClass("gbcolor");
    $('#DiscRTa').removeClass("whcolor");
    $('#oneshowa').removeClass("gbcolor");
    $('#multiRT').addClass("whcolor");
    $('#multiRT').removeClass("gbcolor");

    //document.getElementById('roundway').style.display = "block";
    $(".dhirajK").removeClass("dhirajK").addClass("mybxdighi");
    document.getElementById('retdate').style.display = "block";
    document.getElementById('Fltnotmulticitybtn').style.display = "block";
    document.getElementById('multicitybtn').style.display = "none";
    document.getElementById('multicitydiv').style.display = "none";

	// getApiTokenListBackGround();
}
window.oneshow = oneshow;
window.roundshow = roundshow;
window.DiscRTshow = DiscRTshow;


//==============================Multicity start====================================================//
function Multicityshow() {
    //debugger;
    document.getElementById('fltOWRW').style.display = "none";
    document.getElementById('Trip4').checked = true;
    document.getElementById('Trip3').checked = false;
    document.getElementById('Trip1').checked = false;
    document.getElementById('Trip2').checked = false;

    $('#oneshowa').addClass("whcolor");
    $('#roundshowa').addClass("whcolor");
    $('#DiscRTa').addClass("whcolor");
    $('#roundshowa').removeClass("gbcolor");
    $('#DiscRTa').removeClass("gbcolor");
    $('#oneshowa').removeClass("gbcolor");
    $('#multiRT').addClass("gbcolor");
    $('#multiRT').removeClass("whcolor");
    document.getElementById('multicitydiv').style.display = "block";
    document.getElementById('Fltnotmulticitybtn').style.display = "none";
    document.getElementById('multicitybtn').style.display = "block";
    $(".mybxdighi").removeClass("mybxdighi").addClass("dhirajK");
	
	// getApiTokenListBackGround();
}
window.Multicityshow = Multicityshow;
//================== Multicity Add Sector start==================================//
var star = 0;
var index = 0;
function addCity(selectedName, selectedValue) {
   
    var ni = document.getElementById('my' + selectedName + 'Div');
    if (star == 1) {
        try {
            document.getElementById("Remove" + selectedName + index).style.display = "none";
        }
        catch (e) { }

        index = index + 1;
    }
    else {
        index = parseInt(selectedValue) + 2
    }
    if (index <= 5) {
        star = 1;
        var selectedIdName = selectedName + 'Div' + index;
        var selecteddiv = document.createElement('li');
        selecteddiv.setAttribute('id', selectedIdName);
        var selectedhtm = '<div class="col-md-12 col-sm-12 col-xs-12 multicityrow offset-0">';
        selectedhtm += '<div class="col-md-4 col-sm-6 col-xs-6 m-top-10 offset-0 mbottpad">';
        //selectedhtm += '<label for="fromsector" class="label-text">From</label>';
        selectedhtm += '<div class="">';
        selectedhtm += '<span role="status" aria-live="polite" class="ui-helper-hidden-accessible"></span>';
        selectedhtm += '<input type="text" id="Morigin_' + index + '" placeholder="Type Departure Location Here" onfocus="if (this.value =="Type Departure Location Here") {this.value=""};"   class="classic form-control input-lg multicitysector"';
        selectedhtm += 'title="" name="AirlineData[0].MFrom" autocomplete="off" onkeydown = "return (event.keyCode!=13);" onclick="this.setSelectionRange(0, this.value.length)">';
        selectedhtm += '</div>';
        selectedhtm += '</div>';
        selectedhtm += '<div class="col-md-4 col-sm-6 col-xs-6 m-top-10 offset-0 mbottpad">';
        //selectedhtm += '<label for="ToSector" class="label-text">To</label>';
        selectedhtm += '<div class="">';
        selectedhtm += '<span role="status" aria-live="polite" class="ui-helper-hidden-accessible"></span>';
        selectedhtm += '<input type="text" id="Mdestination_' + index + '" placeholder="Type Arrival Location Here" onfocus="if (this.value =="Type Departure Location Here") {this.value=""};"  class="classic form-control input-lg multicitysector"';
        selectedhtm += 'title="" name="AirlineData[0].MTo" autocomplete="off" onkeydown="return (event.keyCode!=13);" onclick="this.setSelectionRange(0, this.value.length)" >';
        selectedhtm += '</div>';
        selectedhtm += '</div>';
        selectedhtm += '<div class="col-md-4 col-sm-6 col-xs-8 m-top-10 offset-0 mbottpad">';
        // selectedhtm += '<label class="label-text"> Departs on</label>';
        selectedhtm += '<input type="text" id="Mdepartdate_' + index + '"  placeholder="dd/mm/yyyy" readonly="false" name="AirlineData[0].MDeparture" class="classic form-control input-lg cal-1 Mdepart" style="cursor:pointer">';
        selectedhtm += '</div>';
        selectedhtm += '<div class="crossdivbox">';
        selectedhtm += '<a id="Remove' + selectedName + index + '" title="Remove ' + selectedName + '" href="javascript:void(0);" onclick=removeCity("' + selectedName + '","' + index + '")>' +
       '<img class="multi_clsBtn" src="/assets/img/outcross.png"></a><div class="clr"></div>';
        selectedhtm += '</div>';
        selectedhtm += '</div>';
        selecteddiv.innerHTML = selectedhtm;
        ni.appendChild(selecteddiv);
    }
    else {
        alert("you can select only five sector");
        index = index - 1;
        document.getElementById("Remove" + selectedName + index).style.display = "block";
    }
}
window.addCity= addCity;

//================== Multicity Add Sector end==================================//

//================== Multicity remove Sector start==================================//

function removeCity(selectedName, selectedValue) {
   
    var masterdiv = document.getElementById('my' + selectedName + 'Div')
    var downdivnum = selectedName + 'Div' + selectedValue;
    var downdiv = document.getElementById(downdivnum);
    if (downdiv) {
       
        masterdiv.removeChild(downdiv);
        index = index - 1;
        document.getElementById("Remove" + selectedName + index).style.display = "block";
    }
}
window.removeCity =removeCity ;

//================== Multicity remove Sector end==================================//

/*
function getresultMulticity() {
   
    var rseultvalidation = validatemulticity()
    if (rseultvalidation == true) {
        // Circular loader removed - using progress bar instead
        // $("#CenterwaitingDivMulticity").css("display", "block");
        $('#paxtyMcity').text($("#infant").val());
        $('#paxtyMcity1').text($("#adult").val());
        $('#paxtyMcity2').text($("#child").val());
       
        var multirow = $(".multicityrow");
        var data = "&lt;Root&gt;";
        var muldetail = "";
        for (var i = 0; i < multirow.length; i++) {
            var row = i + 1;
            data += "&lt;SectorDetail&gt;";
            data += "&lt;Source&gt;" + document.getElementById('Morigin_' + row).value + "&lt;/Source&gt;";
            data += "&lt;Destinstion&gt;" + document.getElementById('Mdestination_' + row).value + "&lt;/Destinstion&gt;";
            data += "&lt;DepartDate&gt;" + document.getElementById('Mdepartdate_' + row).value + "&lt;/DepartDate&gt;";
            data += "&lt;/SectorDetail&gt;";
            muldetail += "<div class='col-md-12 mysearchboxmain'> <div class='col-md-4 col-xs-4 searboxdt'>" +
                document.getElementById('Morigin_' + row).value.split(")")[0].split("(")[1] +
                "</div><div class='col-md-4 col-xs-4 searboxdt1' >" +
                document.getElementById('Mdepartdate_' + row).value +
                "</div> <div class='col-md-4 col-xs-4 searboxdt2'>" +

                 document.getElementById('Mdestination_' + row).value.split(")")[0].split("(")[1] +
                "</div> </div>";
        }
        data += "&lt;/Root&gt;";
       
        $('#multicitydetail').html(muldetail);
        var AirLines = "";

        var PreferedList = [];

        $("[id*=Chkbocpreferedairline] input:checked").each(function () {
            PreferedList.push($(this).val());
        });
        AirLines = PreferedList.toString();
        var spclfare = 0;
        if (document.getElementById('Specialfa').checked == true) {
            spclfare = 1;
        }
        var travelClass = $('#ddltravelClass').val();
        var mySelection = getSearchType();
        if (mySelection == "MRT") {

        }
        var params = "";
        params = [{ "_Multisector": data, "_NoOfAdult": $("#adult").val(), "_NoOfChild": $("#child").val(), "_NoOfInfant": $("#infant").val(), "_SearchType": $("input[name='r1']:checked").val(), "_TravelClass": travelClass, "_SpecialFare": spclfare, "_Place": "I" }];
        $.ajax({
            type: "POST",
            url: "LandingPage.aspx/GetvalueForRequestMulticity",
            data: "{'objclsPSQ':" + JSON.stringify(params) + "}",
            contentType: "application/json; charset=utf-8", //Set Content-Type
            dataType: "json", // Set return Data Type
            cache: false,
            success: function (result) {
               
                var hostName = $('#hdnhostName').val();
                //alert(hostName);
                var obj = result.d;
                if (obj != "") {
                   
                    if (mySelection == "MRT") {
                        location.href = "k_multicity.aspx";
                    }

                }
                else {
                    location.href = "//" + hostName + "/Flight/Index";
                }
            },
            error: function (xhr, msg, e) {
               
                alert("getresult()\n" + msg + "  " + e + " " + xhr);
                location.href = "//" + hostName + "/Index.aspx";
            }
        });
    }
    else { }
}
*/

function getresultMulticity() {
    //debugger;
	
	
	
    var rseultvalidation = validatemulticity()
    if (rseultvalidation == true) {
        // Circular loader removed - using progress bar instead
        // $("#CenterwaitingDivMulticity").css("display", "block");
        $('#paxtyMcity').text($("#infant").val());
        $('#paxtyMcity1').text($("#adult").val());
        $('#paxtyMcity2').text($("#child").val());
        // Circular loader removed - using progress bar instead
        // $('#CenterwaitingDiv').css("display", "block");
        //debugger;
        var multirow = $(".multicityrow");
        
        var muldetail = "";
        var __Source = "";
        var __DepartDate = "";
        var reqList = [];

        var AirLines = "";
        var PreferedList = [];
        $("[id*=Chkbocpreferedairline] input:checked").each(function () {
            PreferedList.push($(this).val());
        });
        AirLines = PreferedList.toString();
        var spclfare = 0;
        if (document.getElementById('Specialfa').checked == true) {
            spclfare = 1;
        }
        var travelClass = $('#ddltravelClass').val();
        var mySelection = getSearchType();
        var data = '';
        data += "&lt;Root&gt;";
        for (var i = 0; i < multirow.length; i++) {
            var row = i + 1;
       
            data += "&lt;SectorDetail&gt;";
            data += "&lt;Source&gt;" + document.getElementById('Morigin_' + row).value + "&lt;/Source&gt;";
            data += "&lt;Destinstion&gt;" + document.getElementById('Mdestination_' + row).value + "&lt;/Destinstion&gt;";
            data += "&lt;DepartDate&gt;" + document.getElementById('Mdepartdate_' + row).value + "&lt;/DepartDate&gt;";
            data += "&lt;/SectorDetail&gt;";
        }
        data += "&lt;/Root&gt;";

        for (var i = 0; i < multirow.length; i++) {
            var row = i + 1;
            /*var data = "";
            if (i == 0) {
                __Source = document.getElementById('Morigin_' + row).value;
                __DepartDate = document.getElementById('Mdepartdate_' + row).value;
            }
            data += "&lt;SectorDetail&gt;";
            data += "&lt;Source&gt;" + __Source + "&lt;/Source&gt;";
            data += "&lt;Destinstion&gt;" + document.getElementById('Mdestination_' + row).value + "&lt;/Destinstion&gt;";
            data += "&lt;DepartDate&gt;" + __DepartDate + "&lt;/DepartDate&gt;";
            data += "&lt;/SectorDetail&gt;";
            */

            muldetail += "<div class='col-md-12 mysearchboxmain'> <div class='col-md-4 col-xs-4 searboxdt'>" +
                document.getElementById('Morigin_' + row).value.split(")")[0].split("(")[1] +
                "</div><div class='col-md-4 col-xs-4 searboxdt1' >" +
                document.getElementById('Mdepartdate_' + row).value +
                "</div> <div class='col-md-4 col-xs-4 searboxdt2'>" +

                document.getElementById('Mdestination_' + row).value.split(")")[0].split("(")[1] +
                "</div> </div>";
            

            reqList.push({ "_Multisector": data, "_DepartureStation": document.getElementById('Morigin_' + row).value, "_ArrivalStation": document.getElementById('Mdestination_' + row).value, "_BeginDate": document.getElementById('Mdepartdate_' + row).value, "_EndDate": "", "_NoOfAdult": $("#adult").val(), "_NoOfChild": $("#child").val(), "_NoOfInfant": $("#infant").val(), "_SearchType": "O", "_PreferedAirlines": AirLines, "_TravelClass": travelClass, "_SpecialFare": spclfare, "_Place": "I", "_SrNo": row });
            
        }

        //debugger;
        $('#multicitydetail').html(muldetail);

        var params = "";
        var hostName = $('#hdnhostName').val();
        //params = [{ "_Multisector": reqList, "_NoOfadult": $("#adult").val(), "_NoOfChild": $("#child").val(), "_NoOfInfant": $("#infant").val(), "_SearchType": $("input[name='r1']:checked").val(), "_PreferedAirlines": AirLines, "_TravelClass": travelClass, "_SpecialFare": spclfare, "_Place": "I" }];
        params = reqList;        
        //debugger;
        $.ajax({
            type: "POST",
            //url: "//" + hostName + "/LandingPage.aspx/GetvalueForRequestMulticity",
            url: "//" + hostName + "/flight/GetvalueForRequestMulticity",
            // data: "{'objclsPSQ':" + JSON.stringify(params) + "}",
            data: JSON.stringify({ objclsPSQ: params }),
            contentType: "application/json; charset=utf-8", //Set Content-Type
            dataType: "json", // Set return Data Type
            async: true,
			cache: false,
            success: function (result) {
                //debugger;
                //var hostName = $('#hdnhostName').val();
                //alert(hostName);
                var obj = result.d;
                if (obj != "") {
                console.log(hostName);
                
                    if (mySelection == "MRT") {
                        //location.href = "//" + hostName + "/k_multicity.aspx";
                        //location.href = "//" + hostName + "/k_multicity1.aspx";
                       location.href = "//" + hostName + "/Flight/multicity";
					   //location.href = "//" + hostName + "/TestPage.aspx";
                        
                    }

                }
                else {
                    location.href = "//" + hostName + "/Flight/Index";
                }
            },
            error: function (xhr, msg, e) {
                //debugger;
                // Extract proper error message from response
                var errorMsg = msg || "Unknown error";
                if (xhr.responseJSON && xhr.responseJSON.error) {
                    errorMsg = xhr.responseJSON.error;
                } else if (xhr.responseText) {
                    try {
                        var errorObj = JSON.parse(xhr.responseText);
                        errorMsg = errorObj.error || errorMsg;
                    } catch (parseError) {
                        errorMsg = xhr.responseText || xhr.statusText || errorMsg;
                    }
                }
                
                console.error("getresultMulticity() Error:", {
                    message: errorMsg,
                    status: xhr.status,
                    statusText: xhr.statusText,
                    response: xhr.responseJSON || xhr.responseText
                });
                
                alert("getresultMulticity()\nError: " + errorMsg + "\nStatus: " + (xhr.status || "N/A"));
                // Redirect to Flight/Index instead of Index.aspx
                location.href = "//" + hostName + "/Flight/Index";
            }
        });
    }
    else { }
}
window.getresultMulticity = getresultMulticity;

//========= get Tokens
function getApiTokenListBackGround() {
   
    var hostName = $('#hdnhostName').val();
        $.ajax({
            type: "POST",
            //url: "//" + hostName + "/LandingPage.aspx/GetvalueForRequestMulticity",
            url: "//" + hostName + "/n_SearchDataFilter.aspx/GetApiTokenListBackGround",
            data: "{}",
            contentType: "application/json; charset=utf-8", //Set Content-Type
            dataType: "json", // Set return Data Type
            async: true,
            cache: false,
            success: function (result) {
               

            },
            error: function (xhr, msg, e) {
                
            }
        });
}

jQuery(window).on("load", function () {
        //    getApiTokenListBackGround();
});


window.setInterval(function () {
    setApiSessionKeepAlivBySchedule();  //calling every 9 mini
}, 540000)

function setApiSessionKeepAlivBySchedule() {
   
    var hostName = $('#hdnhostName').val();
    $.ajax({
        type: "POST",
        //url: "//" + hostName + "/LandingPage.aspx/GetvalueForRequestMulticity",
        url: "//" + hostName + "/n_SearchDataFilter.aspx/SetApiSessionKeepAlivBySchedule",
        data: "{}",
        contentType: "application/json; charset=utf-8", //Set Content-Type
        dataType: "json", // Set return Data Type
        async: true,
        cache: false,
        success: function (result) {
           

        },
        error: function (xhr, msg, e) {

        }
    });
};


//========= end  get  Tokens


function validatemulticity() {
    //debugger;
    var flag = 0;
    $('#multicitydiv').find(':input').each(function () {
        if (this.type == 'text') {
            if ($(this).val() == "") {
                $(this).css("border", "1px solid red");
                flag = 1;
            }
        }
    });
    if (flag == 1) {
        return false;
    }
    var Adlt = $('#adult').val();
    var child = $('#child').val();
    var Inf = $('#infant').val();
    var totalpax = parseInt(Adlt) + parseInt(child) + parseInt(Inf);
    if (totalpax > 9) {
        alert('Total passengers are greater than 9.Kindly change your search criteria!');
        return false;
    }
    //check infant
    var x = parseInt(Inf);
    var t = parseInt(Adlt);
    if (x > t) {
        alert("Total adult should not be less than infant!.");
        return false;
    }
    return true;
}
//==============================Multicity end====================================================//



function changesvalues(asd) {

    if (asd.id.toUpperCase() == "ADULT") {
        var htmll = "";
        document.getElementById("child").innerHTML = "";
        var adultvalue = 1;
        try {
            var adultvalue = 10 - parseInt(adult.value);
        }
        catch (e)
        { }
        for (var i = 0; i < adultvalue; i++) {
            htmll += "<option value='" + i + "'>" + i + "</option>";
        }
        document.getElementById("child").innerHTML = htmll;

    }

    var html2
    document.getElementById("infant").innerHTML = "";


    try {
        adultvalue = parseInt(adult.value);
    }
    catch (e)
    { }
    for (var j = 0; j < adultvalue + 1; j++) {
        html2 += "<option value='" + j + "'>" + j + "</option>";
    }
    document.getElementById("infant").innerHTML = html2;
}
window.changesvalues=changesvalues;
function Fromtoswap() {
    // 'fromsector', 'ToSector', 'id'

    var temp = "";
    temp = oricity.value;
    oricity.value = desticity.value;
    desticity.value = temp;
}
window.Fromtoswap=Fromtoswap;


//minuse plus js start
jQuery(document).ready(function(){
    jQuery("#sumManualPassenger").click(function () {
        //debugger;
        //var totalpax = parseInt($("#adult").val()) + parseInt($("#child").val()) + parseInt($("#infant").val()); 
        //document.getElementById("totalnoofpax").textContent = totalpax;            
        //$('#totalnoofpax').trigger('click');
        calculatenofpax();
    });
});
function calculatenofpax() {
    //debugger;
    var totalpax = parseInt($("#adult").val()) + parseInt($("#child").val()) + parseInt($("#infant").val());
    document.getElementById("totalnoofpax").textContent = totalpax;
    jQuery('#totalnoofpax').trigger('click');
}
window.calculatenofpax=calculatenofpax;
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

}
window.checksum=checksum;
function prefer() {
    var x = document.getElementById('flighimg');
    if (x.style.display === 'none') {
        x.style.display = 'block';
        $('#hidesec').css('display', 'none');
        $('#showsec').css('display', 'block');
    } else {
        x.style.display = 'none';
        $('#hidesec').css('display', 'block');
        $('#showsec').css('display', 'none');
    }
} 
window.prefer=prefer;
//End 

