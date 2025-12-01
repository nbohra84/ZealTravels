document.addEventListener('DOMContentLoaded', function() {
    // Use querySelectorAll to select all elements whose id starts with 'bokref_' 
    var links = document.querySelectorAll('[id^="bokref_"]');

    // Loop through each matching element and attach the event listener
    links.forEach(function (link) {
        link.addEventListener('click', function (event) {
            bookrefff(event.target);
        });
    });
});

function validatesearchtab() {
    //debugger;
    if (($("#ContentPlaceHolder1_txtbookreffilter").val() != "" && $("#ContentPlaceHolder1_restricpnr").val() == "") && $("#ContentPlaceHolder1_restricticket").val() == "") {
        //debugger;
        var filter = /^[0-9-+]+$/;
        if (filter.test($("#ContentPlaceHolder1_txtbookreffilter").val().trim())) {
            return true;
        }
        else {
            alert("please enter integer value");
            event.preventDefault();
            return false;
        }
    }
    else if (($("#ContentPlaceHolder1_txtbookreffilter").val() == "" && $("#ContentPlaceHolder1_restricpnr").val() != "") && $("#ContentPlaceHolder1_restricticket").val() == "") {
        //debugger;
        return true;
    }
    else if (($("#ContentPlaceHolder1_txtbookreffilter").val() == "" && $("#ContentPlaceHolder1_restricpnr").val() == "") && $("#ContentPlaceHolder1_restricticket").val() != "") {
        //debugger;
        return true;
    }
    else if ($("#ContentPlaceHolder1_restricpax").val() != "") {
        document.getElementById('ContentPlaceHolder1_txtbookreffilter').value = "";
        document.getElementById('ContentPlaceHolder1_restricpnr').value = "";
        document.getElementById('ContentPlaceHolder1_restricticket').value = "";
        if ($("#ContentPlaceHolder1_txtRestrictfromdate").val() != "" && $("#ContentPlaceHolder1_txtRestricttodate").val() != "") {
            return true;
        }
        else {
            alert("Please enter bookingfrom date and bookingTo date.!");
            event.preventDefault();
            return false;
        }
    }
    else if ($("#ContentPlaceHolder1_restricpnr").val() != "" && $("#ContentPlaceHolder1_restricticket").val() != "") {
        //debugger;
        document.getElementById('ContentPlaceHolder1_txtbookreffilter').value = "";
        document.getElementById('ContentPlaceHolder1_restricpnr').value = "";
        document.getElementById('ContentPlaceHolder1_restricticket').value = "";
        alert("Please fill one field.!");
        event.preventDefault();
        return false
    }
    else {
        document.getElementById('ContentPlaceHolder1_txtbookreffilter').value = "";
        document.getElementById('ContentPlaceHolder1_restricpnr').value = "";
        document.getElementById('ContentPlaceHolder1_restricticket').value = "";
        alert("Please fill one field.!");
        event.preventDefault();
        return false;
    }
}

// $('#searchbydatediv').show();  // Or any logic you use to display the div
// $('#ContentPlaceHolder1_txtRestrictfromdate').datepicker('refresh');  // Reinitialize datepicker when visible
// $('#ContentPlaceHolder1_txtRestricttodate').datepicker('refresh');

function LoadScript() {
    var d = new Date();
    var dateNow = d.toISOString().split('T')[0]; // Use ISO string for date formatting
    $(".datetopic").datepicker({
        autoclose: true,
        format: 'yyyy-mm-dd',
        todayHighlight: true,
        orientation: 'bottom',
        defaultDate: dateNow
    });

    $("#ContentPlaceHolder1_companyname").autocomplete({
        source: function (request, response) {
            $("#tosectordiv1").hide(); // Use .hide() for better readability
            $.ajax({
                url: 'Ladger_Report.aspx/getcompnydata',
                data: JSON.stringify({ srchtxt: request.term }), // Use JSON.stringify for clarity
                dataType: "json",
                type: "POST",
                contentType: "application/json; charset=utf-8",
                success: function (data) {
                    response($.map(data.d, function (item) {
                        return { label: item, value: item };
                    }));
                }
            });
        },
        minLength: 3,
        selectFirst: true,
        autoFocus: true,
        select: function (event, ui) {
            // Handle selection if necessary
        },
        open: function () {
            $(this).removeClass("ui-corner-all").addClass("ui-corner-top");
        },
        close: function () {
            $(this).removeClass("ui-corner-top").addClass("ui-corner-all");
        },
        search: function () {
            $(this).addClass('p_loader');
        },
        response: function () {
            $(this).removeClass('p_loader');
        }
    });

    $("#ContentPlaceHolder1_restricpax").keypress(function (event) {
        var inputValue = event.charCode;
        if (!(inputValue >= 65 && inputValue <= 122) && (inputValue !== 32 && inputValue !== 0)) {
            event.preventDefault();
        } else {
            $("#searchbydatediv").toggle(this.value.length >= 3); // Simplified toggle logic
        }
    });
}
function ShowDetaildiv(abc) {
    var tabledatalength = parseInt(document.getElementById("ContentPlaceHolder1_tblcount").value);
    for (var i = 0; i < tabledatalength; i++) {
        var x = document.getElementById("PaymentsDiv_" + i);
        //var y = document.getElementById("RemarkDiv_"+i);
        var z = document.getElementById("DebitDiv_" + i);
        if (x.style.display === "block") {
            x.style.display = "none";
        }
        //if (y.style.display === "block") {
        //    y.style.display = "none";
        //}
        if (z.style.display === "block") {
            z.style.display = "none";
        }
    }
    var selecteddiv = document.getElementById(abc.id);
    if (selecteddiv.style.display === "block") {
        selecteddiv.style.display = "none";
    } else {
        selecteddiv.style.display = "block";
    }
}
function hidedetaildiv(abc) {
    //debugger;
    var selecteddiv = document.getElementById(abc.id);
    if (selecteddiv.style.display === "block") {
        selecteddiv.style.display = "none";
    }
}
function showdetail(abc) {
    //debugger;
    var ll = document.getElementById(abc.id);
    if ($(ll).is(':visible')) {
        $(ll).hide();
    }
    else {
        $(ll).modal();
    }
}

function Perpaxticketttt(abc) {
    var result = abc.getAttribute('accesskey');

    // Split the result to get bookingref and segM_ID
    var splitResult = result.split(",");
    var bookingref = splitResult[0];  
    var segM_ID = splitResult[1];     

    var encodedBookingRef = btoa(bookingref);
    var encodedSegM_ID = btoa(segM_ID);

    window.open("/PrintPassengerPopup?BookingRef=" + encodedBookingRef + "&C_PassengerID=" + encodedSegM_ID, 
                "Popup51", 
                "location=1,status=1,scrollbars=1,resizable=1,directories=1,toolbar=1,titlebar=1,width=940,height=800");
}

function Perpaxticketttt_sa(abc) {

    var splitResult = result.split(",");
    var bookingref = splitResult[0];  
    var segM_ID = splitResult[1];     

    var encodedBookingRef = btoa(bookingref);
    var encodedSegM_ID = btoa(segM_ID);

    window.open("/PrintPassengerPopup?BookingRef=" + encodedBookingRef + "&C_PassengerID=" + encodedSegM_ID, 
                "Popup51", 
                "location=1,status=1,scrollbars=1,resizable=1,directories=1,toolbar=1,titlebar=1,width=940,height=800");
}
function bookrefff() {
    var result = document.getElementById('hiddenBookingReference').value;

    if (!result) {
        return;
    }
    var encodedBookingRef = btoa(result); 
    window.open("/PrintPopup?BookingRef=" + encodedBookingRef, "Popup3", "location=1,status=1,scrollbars=1,resizable=1,directories=1,toolbar=1,titlebar=1,width=900,height=800");
}
function printTicketCust(abc) {
    var result = abc.accessKey;
    window.open("Print_Popup_SA.aspx?BookingRef=" + btoa(result), "Popup2", "location=1,status=1,scrollbars=1, resizable=1, directories=1, toolbar=1, titlebar=1, width=900, height=800");
}
function bookrefffhotel(abc) {
    //debugger;
    var result = abc.accessKey;
    window.open("hotel_popup.aspx?BookingRef=" + btoa(result), "Popup1", "location=1,status=1,scrollbars=1, resizable=1, directories=1, toolbar=1, titlebar=1, width=900, height=800");
}
function bookAirOffline(abc) {
    //debugger;
    var result = abc.accessKey;
    window.open("Departure_Printmail.aspx?BookingRef=" + result, "Popup21", "location=1,status=1,scrollbars=1, resizable=1, directories=1, toolbar=1, titlebar=1, width=900, height=700");
}
function getDatediffrence() {
    //debugger;
    var fromdate = new Date(document.getElementById("ContentPlaceHolder1_txtfromdate").value);
    var Todate = new Date(document.getElementById("ContentPlaceHolder1_txtTodate").value);
    var no_day = Math.ceil((((Todate) - fromdate) + 1) / (1000 * 60 * 60 * 24));
    if (no_day > 31) {
        alert("Maximum 31 days report can be seen.!");
        return false
    }
    else {
        return true
    }
}
function showdetailRemark(abc) {
    //debugger;
    var ll = document.getElementById(abc.id);
    if ($(ll).is(':visible')) {
        $(ll).hide();
    }
    else {
        $(ll).show();
    }
}
function Closepopupremark(abc) {
    //debugger;
    var selecteddiv = document.getElementById(abc.id);
    $(selecteddiv).hide();
}
function toggleDiv(divId) {
    var selectedDiv = document.getElementById(divId);
    selectedDiv.style.display = (selectedDiv.style.display === "block") ? "none" : "block";
}

function toggleVisibilityById(id) {
    var element = document.getElementById(id);
    $(element).toggle(); // Use jQuery for consistency
}

function showModalById(id) {
    var element = document.getElementById(id);
    if ($(element).is(':visible')) {
        $(element).hide();
    } else {
        $(element).modal();
    }
}

function printPopup(url, result) {
    window.open(`${url}?BookingRef=${btoa(result)}`, "Popup", "location=1,status=1,scrollbars=1,resizable=1,directories=1,toolbar=1,titlebar=1,width=900,height=800");
}

function validateDateDifference() {
    var fromDate = new Date($("#ContentPlaceHolder1_txtfromdate").val());
    var toDate = new Date($("#ContentPlaceHolder1_txtTodate").val());
    var noOfDays = Math.ceil((toDate - fromDate + 1) / (1000 * 60 * 60 * 24));
    if (noOfDays > 31) {
        alert("Maximum 31 days report can be seen.");
        return false;
    }
    return true;
}

// Function to clear input fields
function clearFields() {
    $("#ContentPlaceHolder1_txtbookreffilter").val("");
    $("#ContentPlaceHolder1_restricpnr").val("");
    $("#ContentPlaceHolder1_restricticket").val("");
    $("#ContentPlaceHolder1_restricpax").val("");
    $("#ContentPlaceHolder1_txtRestrictfromdate").val("");
    $("#ContentPlaceHolder1_txtRestricttodate").val("");
    $("#searchbydatediv").hide(); // Hide the date div if necessary
}

$(document).ready(function() {
    // Listen for input change on the 'restricpax' field
    $("#ContentPlaceHolder1_restricpax").on("input", function() {
        if ($(this).val().trim() !== "") {
            $("#searchbydatediv").css("display", "block");

            // Initialize datepickers after showing the date fields
            $('#ContentPlaceHolder1_txtRestrictfromdate').datepicker({
                format: 'yyyy-mm-dd', 
                autoclose: true,       
                todayHighlight: true   
            });
            $('#ContentPlaceHolder1_txtRestricttodate').datepicker({
                format: 'yyyy-mm-dd',
                autoclose: true,
                todayHighlight: true
            });
        } else {
            // Hide the 'searchbydatediv' when no input is given
            $("#searchbydatediv").css("display", "none");
        }
    });
       // Open the datepicker when the calendar icon is clicked for "Booking From"
       $('#Span2').on('click', function() {
        $('#ContentPlaceHolder1_txtRestrictfromdate').datepicker('show');
    });

    // Open the datepicker when the calendar icon is clicked for "Booking To"
    $('#Span3').on('click', function() {
        $('#ContentPlaceHolder1_txtRestricttodate').datepicker('show');
    });
});

// Function to validate search tab
function validateSearchTab() {
    var bookRef = $("#ContentPlaceHolder1_txtbookreffilter").val().trim();
    var pnr = $("#ContentPlaceHolder1_restricpnr").val().trim();
    var ticket = $("#ContentPlaceHolder1_restricticket").val().trim();
    var pax = $("#ContentPlaceHolder1_restricpax").val().trim();
    var messageArea = $("#messageArea"); 

    // Clear previous messages
    messageArea.text("");

    // Check if all fields are empty
    if (!bookRef && !pnr && !ticket && !pax) {
        messageArea.text("Please fill at least one field.");
        event.preventDefault();
        return false;
    }

    // Validate based on filled fields
    if (bookRef && !pnr && !ticket) {
        if (!/^[0-9-+]+$/.test(bookRef)) {
            messageArea.text("Please enter an integer value.");
            event.preventDefault();
            return false;
        }
        return true; // Valid input
    } 
    else if (!bookRef && pnr && !ticket) {
        return true; // Valid since PNR is filled
    } 
    else if (!bookRef && !pnr && ticket) {
        return true; // Valid since ticket is filled
    } 
    else if (pax) {
        //clearFields();
        if ($("#ContentPlaceHolder1_txtRestrictfromdate").val() && $("#ContentPlaceHolder1_txtRestricttodate").val()) {
            return true;
        } else {
            messageArea.text("Please enter booking from date and booking to date.");
            event.preventDefault();
            return false;
        }
    } 
    else if (pnr && ticket) {
        clearFields();
        messageArea.text("Please fill one field.");
        event.preventDefault();
        return false;
    } 
    else {
        clearFields();
        messageArea.text("Please fill one field.");
        event.preventDefault();
        return false;
    }

    return true; 
}

// Wait for the DOM to fully load
document.addEventListener("DOMContentLoaded", function() {
    var applyButton = document.getElementById("applyButton");
    
    if (applyButton) {
        applyButton.addEventListener("click", function(event) {
            //event.preventDefault(); // Prevent the default form submission
            validateSearchTab(); // Call the validation function
        });
    } else {
        console.error("Apply button not found.");
    }
});

// Function to clear input fields
function cleartxtfield() {
    document.getElementById('ContentPlaceHolder1_txtbookreffilter').value = "";
    document.getElementById('ContentPlaceHolder1_restricpnr').value = "";
    document.getElementById('ContentPlaceHolder1_restricticket').value = "";
    document.getElementById('ContentPlaceHolder1_restricpax').value = "";
    //document.getElementById("searchbydatediv").style.display = "none";
    $("#hiddenBookingReference").val("");
    $("#hiddenPnr").val("");
    $("#hiddenTicketNumber").val("");
    $("#hiddenPassengerName").val("");
    
}

// Wait for the DOM to fully load
document.addEventListener("DOMContentLoaded", function() {
    // Get the button element
    var clearButton = document.getElementById("clearButton");
    
    // Attach the click event listener
    clearButton.addEventListener("click", function(event) {
        event.preventDefault(); // Prevent the default anchor behavior
        cleartxtfield(); // Call the clear function
    });
});

$(document).ready(function() {
    var today = new Date();
    var formattedToday = today.toISOString().split('T')[0]; // Format to 'YYYY-MM-DD'

    // Initialize datepickers
    $('#txtfromdate').datepicker({
        autoclose: true,
        format: 'yyyy-mm-dd',
        todayHighlight: true,
    }); // Set the input value to today's date

    $('#txtTodate').datepicker({
        autoclose: true,
        format: 'yyyy-mm-dd',
        todayHighlight: true,
    }); // Set the input value to today's date

    // Attach click events to icons
    $('#fromspan').on('click', function() {
        $('#txtfromdate').datepicker('show').focus();
    });

    $('#Tospan').on('click', function() {
        $('#txtTodate').datepicker('show').focus();
    });
    if($(".search-btn1").hasClass("validate-date-range")){
        // Function to check date range
        function checkDateRange() {
            var fromDate = $('#txtfromdate').datepicker('getDate');
            var toDate = $('#txtTodate').datepicker('getDate');
            
            if (fromDate && toDate) {
                var timeDiff = Math.abs(toDate - fromDate);
                var dayDiff = Math.ceil(timeDiff / (1000 * 3600 * 24)); // Convert ms to days

                if (dayDiff > 31) {
                    $('#errorDiv').show(); // Show error message
                    $(".search-btn1").prop('disabled', true);
                } else {
                    $('#errorDiv').hide(); // Hide error message if within range
                    $(".search-btn1").prop('disabled', false);
                }
            }
        }

        // Event listeners for date changes
        $('#txtfromdate').on('change', checkDateRange);
        $('#txtTodate').on('change', checkDateRange);
    }
});


function showDetail(modalId) {
    var modal = document.getElementById(modalId);
    
    if (modal) {
        modal.style.pointerEvents = 'auto'; 
        modal.style.display = 'block'; 
    }
}


function closeModal() {
    // Hide the modal
    document.getElementById('divModel').style.display = 'none';
}

// Optional: Close modal when clicking outside of it
window.onclick = function(event) {
    const modal = document.getElementById('divModel');
    if (event.target === modal) {
        closeModal();
    }
};



//Extra js

function flightsearch_1() {

    document.getElementById("mainbgboxqwe").style.display = "block";
    document.getElementById("mainbgboxqwe1").style.display = "none";
    document.getElementById("mainbgboxqwe2").style.display = "none";
    document.getElementById("mainbgboxqwe3").style.display = "none";
    document.getElementById("mainbgboxqwe4").style.display = "none";
    


}

function HotelSearch() {
    debugger;

    // document.getElementById('mainbgboxqwe').style.display = "none";
    document.getElementById("mainbgboxqwe").style.display = "none";
    document.getElementById("mainbgboxqwe2").style.display = "none";
    document.getElementById("mainbgboxqwe1").style.display = "block";
    document.getElementById("mainbgboxqwe3").style.display = "none";
    document.getElementById("mainbgboxqwe4").style.display = "none";
}

function toudfgfghg() {
    debugger;

    // document.getElementById('mainbgboxqwe').style.display = "none";
    document.getElementById("mainbgboxqwe").style.display = "none";
    document.getElementById("mainbgboxqwe1").style.display = "none";
    document.getElementById("mainbgboxqwe2").style.display = "block";
    document.getElementById("mainbgboxqwe3").style.display = "none";
    document.getElementById("mainbgboxqwe4").style.display = "none";
}

function carclkuort() {
    debugger;

    // document.getElementById('mainbgboxqwe').style.display = "none";
    document.getElementById("mainbgboxqwe").style.display = "none";
    document.getElementById("mainbgboxqwe1").style.display = "none";
    document.getElementById("mainbgboxqwe2").style.display = "none";
    document.getElementById("mainbgboxqwe3").style.display = "block";
    document.getElementById("mainbgboxqwe4").style.display = "none";
}

function visacfhyr() {
    debugger;

    // document.getElementById('mainbgboxqwe').style.display = "none";
    document.getElementById("mainbgboxqwe").style.display = "none";
    document.getElementById("mainbgboxqwe1").style.display = "none";
    document.getElementById("mainbgboxqwe2").style.display = "none";
    document.getElementById("mainbgboxqwe3").style.display = "none";
    document.getElementById("mainbgboxqwe4").style.display = "block";
}


$(document).ready(function () {
if($("#content-slider").length > 0){
    $("#content-slider").lightSlider({
        loop: true,
        keyPress: true
    });
}
if($("#image-gallery").length > 0){
    $('#image-gallery').lightSlider({
        gallery: true,
        item: 1,
        thumbItem: 9,
        slideMargin: 0,
        speed: 500,
        auto: true,
        loop: true,
        onSliderLoad: function () {
            $('#image-gallery').removeClass('cS-hidden');
        }
    });
}

});


var stat;
$(document).ready(function () {
if($(".location").length > 0){
    $(".location").select2(
       {
           placeholder: "Search for a country",
           minimumInputLength: 3,
           width: '100%', maximumSelectionSize: 1,
           allowClear: true,
           ajax: {
               url: '/WebMetthod.aspx/GetCountrylist',
               dataType: 'json',
               type: "POST",
               params: {
                   contentType: 'application/json; charset=utf-8'
               },
               quietMillis: 0o0,
               data: function (term, page) {
                   return JSON.stringify({ searchStr: term });
               },
               results: function (data, page) {
                   //debugger;
                   return { results: JSON.parse(data.d) };
               }
           }
       });

    }
    if($("chosen-select").length > 0){
    $('.chosen-select').select2({ width: '100%', maximumSelectionSize: 1 });

    $('.chosen-select').change(function () {
        debugger;
        var selectedCountry = $(".chosen-select option:selected").val();
        var res1 = $(".chosen-select option:selected").text();
        $("#IndexSearchEngine2_hdnTours").val(res1);
    });
}
    if($(".location").length > 0){
    $(".location").select2(
       {
           placeholder: "Search for a country",
           minimumInputLength: 3,
           width: '100%', maximumSelectionSize: 1,
           allowClear: true,
           ajax: {
               url: '/WebMetthod.aspx/GetCountrylist',
               dataType: 'json',
               type: "POST",
               params: {
                   contentType: 'application/json; charset=utf-8'
               },
               quietMillis: 0o0,
               data: function (term, page) {
                   return JSON.stringify({ searchStr: term });
               },
               results: function (data, page) {
                   //debugger;
                   return { results: JSON.parse(data.d) };
               }
           }
       });
    }
    //$(".location").on("select2-selecting", function (e) {
    //    debugger;
    //    var loc = e.val;
    //    document.getElementById("IndexSearchEngine2_hdnvisatocontry").value = loc;
    //});
});

function Validatedatatourdb(ab) {
    debugger; 
    if ($(".validatess option:selected").text().toUpperCase() == "SELECT") {
        alert("please enter country");
        return false
    }
}

$('body').on('focus', ".datetopicar", function () {
    $('.datetopicar').datepicker({
        minDate: 0, dateFormat: 'yy-mm-dd', numberOfMonths: 1
    });
});

$(document).ready(function () {
     
    $("#sumManualPassenger").click(function () {
        debugger;
        //var totalpax = parseInt($("#adult").val()) + parseInt($("#child").val()) + parseInt($("#infant").val()); 
        //document.getElementById("totalnoofpax").textContent = totalpax;            
        //$('#totalnoofpax').trigger('click');
        calculatenofpax();
    });
    //$(".wow").focusout(function () {
    //    debugger;
    //    calculatenofpax();
    //}); 
});
function calculatenofpax() {
    debugger;
    var totalpax = parseInt($("#adult").val()) + parseInt($("#child").val()) + parseInt($("#infant").val());
    document.getElementById("totalnoofpax").textContent = totalpax;
    $('#totalnoofpax').trigger('click');
}
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

function Validatedata(abc) {
    //debugger;
    if (!$("[id*=localusediv]").validationEngine('validate')) {
        return false;
    }
}
function Validatedataoutstation(abc) {
    //debugger;
    if (!$("[id*=Outstationdiv]").validationEngine('validate')) {
        return false;
    }
}
function ValidatedataTransferab(abc) {
    //debugger 1;
    if (!$("[id*=Transferdiv]").validationEngine('validate')) {
        return false;
    }
}

$("#btnlocaluse").click(function () {
    //debugger;
    stat = 1;
    $("#localusediv").show(500);
    $("#Outstationdiv").hide(500);
    $("#Transferdiv").hide(500);

});
$("#btnoutstation").click(function () {
    //debugger;
    stat = 2;
    $("#localusediv").hide(500);
    $("#Outstationdiv").show(500);
    $("#Transferdiv").hide(500);
});
$("#btntransferqw").click(function () {
    //debugger;
    stat = 3;
    $("#localusediv").hide(500);
    $("#Outstationdiv").hide(500);
    $("#Transferdiv").show(500);
});




function onlydecimal(evt, element) {

    var charCode = (evt.which) ? evt.which : event.keyCode;
    if (evt.keyCode == 8) {
        return true;
    }
    else if (
        (charCode != 45 || $(element).val().indexOf('-') != -1) &&      // “-” CHECK MINUS, AND ONLY ONE.
        (charCode != 46 || $(element).val().indexOf('.') != -1) &&      // “.” CHECK DOT, AND ONLY ONE.
        (charCode < 48 || charCode > 57))
        return false;
    else
        return true;
}

function MyFun(abc) {
    //debugger;
    alert(abc);
    location.href = location.href;
}

function MyFunction() {
    //debugger;
    try {
        if (stat == 1) {
            $("#localusediv").show();
            $("#Outstationdiv").hide();
            $("#Transferdiv").hide();

        }
        if (stat == 2) {
            $("#localusediv").hide();
            $("#Outstationdiv").show();
            $("#Transferdiv").hide();
        }
        if (stat == 3) {
            $("#localusediv").hide();
            $("#Outstationdiv").hide();
            $("#Transferdiv").show();
        }
    }
    catch (e) { }
}
function openCitydvclk(evt, cityName) {
    var i, tabcontent, tablinks;
    tabcontent = document.getElementsByClassName("tabcontent");
    for (i = 0; i < tabcontent.length; i++) {
        tabcontent[i].style.display = "none";
    }
    tablink = document.getElementsByClassName("tablink");
    for (i = 0; i < tablink.length; i++) {
        tablink[i].className = tablink[i].className.replace(" active", "");
    }
    document.getElementById(cityName).style.display = "block";
    evt.currentTarget.className += " active";
}

function absolutebox() {
    var x = document.getElementById('absouludv');
    if (x.style.display === 'block') {
        x.style.display = 'none';
    } else {
        x.style.display = 'block';
    }
}

function absoluteboxTransfer() {
    var x = document.getElementById('absouludvTransfer');
    if (x.style.display === 'block') {
        x.style.display = 'none';
    } else {
        x.style.display = 'block';
    }
}



function absoladultbox() {
    var x = document.getElementById('adultboxcli');
    if (x.style.display === 'block') {
        x.style.display = 'none';
    } else {
        x.style.display = 'block';
    }
    $('#IndexSearchEngine2_PkgnoofADT').val($('#ActivityAdult').val());
    $('#IndexSearchEngine2_PkgnoofChd').val($('#ActivityChild').val());
}

function absolTransferadultbox() {
    var x = document.getElementById('adultboxcliTransfer');
    if (x.style.display === 'block') {
        x.style.display = 'none';
    } else {
        x.style.display = 'block';
    }
    $('#IndexSearchEngine2_PkgnoofADT').val($('#TransferAdult').val());
    $('#IndexSearchEngine2_PkgnoofChd').val($('#TransferChild').val());
}




function tournotifo() {
    var x = document.getElementById('tournoti');
    if (x.style.display === 'block') {
        x.style.display = 'none';
    } else {
        x.style.display = 'block';
    }
}

function hidesowpaxboxholiday() {
    debugger;
    var x = document.getElementById('PaxBoxHoliday');
    if (x.style.display === 'block') {
        x.style.display = 'none';
    } else {
        x.style.display = 'block';
    }

    $('#IndexSearchEngine2_PkgnoofADT').val($('#IndexSearchEngine2_txtAdultholiday').val());
    $('#IndexSearchEngine2_PkgnoofChd').val($('#IndexSearchEngine2_txtChildholiday').val());

}

function hidesowpaxboxMeal() {
    debugger;
    var x = document.getElementById('PaxBoxMeal');
    if (x.style.display === 'block') {
        x.style.display = 'none';
    } else {
        x.style.display = 'block';
    }

    $('#IndexSearchEngine2_PkgnoofADT').val($('#IndexSearchEngine2_txtmealadult').val());
    $('#IndexSearchEngine2_PkgnoofChd').val($('#IndexSearchEngine2_txtmealchild').val());

}

function ValidatedataPackage(abc) {
    debugger;
    if (!$("[id*=PACKAGE]").validationEngine('validate')) {
        return false
    }
}
function ValidatedataMeal(abc) {
    debugger;
    if (!$("[id*=MEAL]").validationEngine('validate')) {
        return false
    }
}

function ValidatedataTour(abc) {
    debugger;
    if (!$("[id*=activiti]").validationEngine('validate')) {
        return false
    }
}

function ValidatedataVisa(dd) {
    debugger;
    if ($(".visafromcountry option:selected").text().toUpperCase() == "SELECT YOUR COUNTRY") {
        alert("Please select your country.");
        return false;
    }
    else if ($(".visafromState option:selected").text().toUpperCase() == "SELECT YOUR STATE") {
        alert("Please select your state.");
        return false;
    }
    else if ($(".visaToCountry option:selected").text().toUpperCase() == "SELECT VISA COUNTRY") {
        alert("Please select your country for visa required.");
        return false;
    }
}
function ValidatedataTransfer(abc) {
    debugger;
    if (!$("[id*=TRANSFER]").validationEngine('validate')) {
        return false
    }
}

function currencdvclk() {
    var x = document.getElementById('currencyidq');
    if (x.style.display === 'block') {
        x.style.display = 'none';
    } else {
        x.style.display = 'block';
    }
}

function setcountrycityontextbox(aaa) {
    debugger;
    var CountryCity = aaa.accessKey;
    $('#IndexSearchEngine2_txtactivitydestination').val(CountryCity);
    absolutebox();
}
function setcountrycityontextboxTransfer(aaa) {
    debugger;
    var CountryCity = aaa.accessKey;
    $('#IndexSearchEngine2_txttransferdestination').val(CountryCity);
    absoluteboxTransfer();
}

$(document).ready(function () {
if($(".chosen-select").length > 0){

    $('.chosen-select').select2({ width: '100%', maximumSelectionSize: 1 });
}
    $('.visafromcountry').change(function () {
        debugger;
        var selectedFromCountry = $(".visafromcountry option:selected").val();
        var res1 = $(".visafromcountry option:selected").text();
        $("#IndexSearchEngine2_hdnvisafromcontry").val(selectedFromCountry);
    });

    $('.visafromState').change(function () {
        debugger;
        var selectedFromstatus = $(".visafromState option:selected").val();
        var res1 = $(".visafromState option:selected").text();
        $("#IndexSearchEngine2_hdnvisafromstate").val(selectedFromstatus);
    });

    $('.visaToCountry').change(function () {
        debugger;
        var selectedToCountry = $(".visaToCountry option:selected").val();
        var res1 = $(".visaToCountry option:selected").text();
        $("#IndexSearchEngine2_hdnvisatocontry").val(selectedToCountry);
    });
    $('.validatess').change(function () {
        debugger;
        var selectedCountry = $(".validatess option:selected").val();
        var res1 = $(".validatess option:selected").text();
        $("#IndexSearchEngine2_hdnTours").val(res1);
    });
    $('#ActivityAdult').prop('disabled', true);
    $(document).on('click', '.plus', function () {
        $('#ActivityAdult').val(parseInt($('#ActivityAdult').val()) + 1);
    });
    $(document).on('click', '.minus', function () {
        $('#ActivityAdult').val(parseInt($('#ActivityAdult').val()) - 1);
        if ($('#ActivityAdult').val() == 0) {
            $('#ActivityAdult').val(1);
        }
    });

    $('#ActivityChild').prop('disabled', true);
    $(document).on('click', '.plusch', function () {
        $('#ActivityChild').val(parseInt($('#ActivityChild').val()) + 1);
    });
    $(document).on('click', '.minusch', function () {
        $('#ActivityChild').val(parseInt($('#ActivityChild').val()) - 1);
        if ($('#ActivityChild').val() == 0) {
            $('#ActivityChild').val(1);
        }
    });
    $("#btnactivitypax").click(function () {
        debugger;
        var totalpax = parseInt($("#ActivityAdult").val()) + parseInt($("#ActivityChild").val());
        $('#totalnoofpaxActivicy').val(totalpax);
        $('#IndexSearchEngine2_PkgnoofADT').val($('#ActivityAdult').val());
        $('#IndexSearchEngine2_PkgnoofChd').val($('#ActivityChild').val());
        //document.getElementById("totalnoofpaxActivicy").textContent = totalpax;
        $('#totalnoofpaxActivicy').trigger('click');
    });
    $(document).on('click', '#totalnoofpaxActivicy', function () {
        //debugger;
        $(this).prop('Counter', 0).animate({
            Counter: $(this).text()
        }, {
            duration: 1000,
            easing: 'swing',
            step: function (now) {
                $(this).text(Math.ceil(now));
            }
        });

    });



    $('#fls').click(function () {
        $('#mainbgboxqwe').addClass('mainbgbox1');
        $('#mainbgboxqwe').removeClass('mainbgbox2');
        $('#mainbgboxqwe').removeClass('mainbgbox3');
        $('#mainbgboxqwe').removeClass('mainbgbox4');
        $('#mainbgboxqwe').removeClass('mainbgbox5');
        $('#mainbgboxqwe').removeClass('mainbgbox6');
        $('#mainbgboxqwe').removeClass('mainbgbox7');
        $('#mainbgboxqwe').removeClass('mainbgbox8');
        $('#mainbgboxqwe').removeClass('mainbgbox9');
    });

    $('#toursi').click(function () {
        $('#mainbgboxqwe').removeClass('mainbgbox1');
        $('#mainbgboxqwe').addClass('mainbgbox2');
        $('#mainbgboxqwe').removeClass('mainbgbox3');
        $('#mainbgboxqwe').removeClass('mainbgbox4');
        $('#mainbgboxqwe').removeClass('mainbgbox5');
        $('#mainbgboxqwe').removeClass('mainbgbox6');
        $('#mainbgboxqwe').removeClass('mainbgbox7');
        $('#mainbgboxqwe').removeClass('mainbgbox8');
        $('#mainbgboxqwe').removeClass('mainbgbox9');
    });


    $('#abcde').click(function () {
        $('#mainbgboxqwe').removeClass('mainbgbox1');
        $('#mainbgboxqwe').removeClass('mainbgbox2');
        $('#mainbgboxqwe').addClass('mainbgbox3');
        $('#mainbgboxqwe').removeClass('mainbgbox4');
        $('#mainbgboxqwe').removeClass('mainbgbox5');
        $('#mainbgboxqwe').removeClass('mainbgbox6');
        $('#mainbgboxqwe').removeClass('mainbgbox7');
        $('#mainbgboxqwe').removeClass('mainbgbox8');
        $('#mainbgboxqwe').removeClass('mainbgbox9');
    });


    $('#activities').click(function () {
        $('#mainbgboxqwe').removeClass('mainbgbox1');
        $('#mainbgboxqwe').removeClass('mainbgbox2');
        $('#mainbgboxqwe').removeClass('mainbgbox3');
        $('#mainbgboxqwe').addClass('mainbgbox4');
        $('#mainbgboxqwe').removeClass('mainbgbox5');
        $('#mainbgboxqwe').removeClass('mainbgbox6');
        $('#mainbgboxqwe').removeClass('mainbgbox7');
        $('#mainbgboxqwe').removeClass('mainbgbox8');
        $('#mainbgboxqwe').removeClass('mainbgbox9');
    });

    $('#Transferli').click(function () {
        $('#mainbgboxqwe').removeClass('mainbgbox1');
        $('#mainbgboxqwe').removeClass('mainbgbox2');
        $('#mainbgboxqwe').removeClass('mainbgbox3');
        $('#mainbgboxqwe').removeClass('mainbgbox4');
        $('#mainbgboxqwe').addClass('mainbgbox5');
        $('#mainbgboxqwe').removeClass('mainbgbox6');
        $('#mainbgboxqwe').removeClass('mainbgbox7');
        $('#mainbgboxqwe').removeClass('mainbgbox8');
        $('#mainbgboxqwe').removeClass('mainbgbox9');
    });


    $('#Mealli').click(function () {
        $('#mainbgboxqwe').removeClass('mainbgbox1');
        $('#mainbgboxqwe').removeClass('mainbgbox2');
        $('#mainbgboxqwe').removeClass('mainbgbox3');
        $('#mainbgboxqwe').removeClass('mainbgbox4');
        $('#mainbgboxqwe').removeClass('mainbgbox5');
        $('#mainbgboxqwe').addClass('mainbgbox6');
        $('#mainbgboxqwe').removeClass('mainbgbox7');
        $('#mainbgboxqwe').removeClass('mainbgbox8');
        $('#mainbgboxqwe').removeClass('mainbgbox9');
    });


    $('#visadivid').click(function () {
        $('#mainbgboxqwe').removeClass('mainbgbox1');
        $('#mainbgboxqwe').removeClass('mainbgbox2');
        $('#mainbgboxqwe').removeClass('mainbgbox3');
        $('#mainbgboxqwe').removeClass('mainbgbox4');
        $('#mainbgboxqwe').removeClass('mainbgbox5');
        $('#mainbgboxqwe').removeClass('mainbgbox6');
        $('#mainbgboxqwe').addClass('mainbgbox7');
        $('#mainbgboxqwe').removeClass('mainbgbox8');
        $('#mainbgboxqwe').removeClass('mainbgbox9');
    });

    $('#toursbxdq').click(function () {
        $('#mainbgboxqwe').removeClass('mainbgbox1');
        $('#mainbgboxqwe').removeClass('mainbgbox2');
        $('#mainbgboxqwe').removeClass('mainbgbox3');
        $('#mainbgboxqwe').removeClass('mainbgbox4');
        $('#mainbgboxqwe').removeClass('mainbgbox5');
        $('#mainbgboxqwe').removeClass('mainbgbox6');
        $('#mainbgboxqwe').removeClass('mainbgbox7');
        $('#mainbgboxqwe').addClass('mainbgbox8');
        $('#mainbgboxqwe').removeClass('mainbgbox9');
    });

    $('#cartaxiidf').click(function () {
        $('#mainbgboxqwe').removeClass('mainbgbox1');
        $('#mainbgboxqwe').removeClass('mainbgbox2');
        $('#mainbgboxqwe').removeClass('mainbgbox3');
        $('#mainbgboxqwe').removeClass('mainbgbox4');
        $('#mainbgboxqwe').removeClass('mainbgbox5');
        $('#mainbgboxqwe').removeClass('mainbgbox6');
        $('#mainbgboxqwe').removeClass('mainbgbox7');
        $('#mainbgboxqwe').removeClass('mainbgbox8');
        $('#mainbgboxqwe').addClass('mainbgbox9');
    });


    $('#TransferAdult').prop('disabled', true);
    $(document).on('click', '.plusAdtTransf', function () {
        $('#TransferAdult').val(parseInt($('#TransferAdult').val()) + 1);
    });
    $(document).on('click', '.minusAdtTransf', function () {
        $('#TransferAdult').val(parseInt($('#TransferAdult').val()) - 1);
        if ($('#TransferAdult').val() == 0) {
            $('#TransferAdult').val(1);
        }
    });

    $('#TransferChild').prop('disabled', true);
    $(document).on('click', '.pluschdTransfer', function () {
        $('#TransferChild').val(parseInt($('#TransferChild').val()) + 1);
    });
    $(document).on('click', '.minuschdTransfer', function () {
        $('#TransferChild').val(parseInt($('#TransferChild').val()) - 1);
        if ($('#TransferChild').val() == 0) {
            $('#TransferChild').val(1);
        }
    });
    $("#btnTransferpax").click(function () {
        debugger;
        var totalpax = parseInt($("#TransferAdult").val()) + parseInt($("#TransferChild").val());
        $('#totalnoofpaxTransfer').val(totalpax);
        $('#IndexSearchEngine2_PkgnoofADT').val($('#TransferAdult').val());
        $('#IndexSearchEngine2_PkgnoofChd').val($('#TransferChild').val());
        //document.getElementById("totalnoofpaxActivicy").textContent = totalpax;
        $('#totalnoofpaxTransfer').trigger('click');
    });

    $(document).on('click', '#totalnoofpaxTransfer', function () {
        //debugger;
        $(this).prop('Counter', 0).animate({
            Counter: $(this).text()
        }, {
            duration: 1000,
            easing: 'swing',
            step: function (now) {
                $(this).text(Math.ceil(now));
            }
        });
    });



    $('#IndexSearchEngine2_txtAdultholiday').prop('disabled', true);
    $(document).on('click', '.plusAdtHolid', function () {
        $('#IndexSearchEngine2_txtAdultholiday').val(parseInt($('#IndexSearchEngine2_txtAdultholiday').val()) + 1);
    });
    $(document).on('click', '.minusAdtHolid', function () {
        $('#IndexSearchEngine2_txtAdultholiday').val(parseInt($('#IndexSearchEngine2_txtAdultholiday').val()) - 1);
        if ($('#IndexSearchEngine2_txtAdultholiday').val() == 0) {
            $('#IndexSearchEngine2_txtAdultholiday').val(1);
        }
    });

    $('#IndexSearchEngine2_txtChildholiday').prop('disabled', true);
    $(document).on('click', '.pluschdHolid', function () {
        $('#IndexSearchEngine2_txtChildholiday').val(parseInt($('#IndexSearchEngine2_txtChildholiday').val()) + 1);
    });
    $(document).on('click', '.minuschdHolid', function () {
        $('#IndexSearchEngine2_txtChildholiday').val(parseInt($('#IndexSearchEngine2_txtChildholiday').val()) - 1);
        if ($('#IndexSearchEngine2_txtChildholiday').val() == 0) {
            $('#IndexSearchEngine2_txtChildholiday').val(1);
        }
    });


    $("#btnholidaypax").click(function () {
        debugger;
        var totalpax = parseInt($("#IndexSearchEngine2_txtAdultholiday").val()) + parseInt($("#IndexSearchEngine2_txtChildholiday").val());
        $('#IndexSearchEngine2_totalnoofpaxHolidays').val(totalpax);
        $('#IndexSearchEngine2_PkgnoofADT').val($('#IndexSearchEngine2_txtAdultholiday').val());
        $('#IndexSearchEngine2_PkgnoofChd').val($('#IndexSearchEngine2_txtChildholiday').val());
        //document.getElementById("totalnoofpaxActivicy").textContent = totalpax;
        $('#IndexSearchEngine2_totalnoofpaxHolidays').trigger('click');
    });
    $(document).on('click', '#IndexSearchEngine2_totalnoofpaxHolidays', function () {
        //debugger;
        $(this).prop('Counter', 0).animate({
            Counter: $(this).text()
        }, {
            duration: 1000,
            easing: 'swing',
            step: function (now) {
                $(this).text(Math.ceil(now));
            }
        });
    });



    $('#IndexSearchEngine2_txtmealadult').prop('disabled', true);
    $(document).on('click', '.plusAdtMeal', function () {
        $('#IndexSearchEngine2_txtmealadult').val(parseInt($('#IndexSearchEngine2_txtmealadult').val()) + 1);
    });
    $(document).on('click', '.minusAdtMeal', function () {
        $('#IndexSearchEngine2_txtmealadult').val(parseInt($('#IndexSearchEngine2_txtmealadult').val()) - 1);
        if ($('#IndexSearchEngine2_txtmealadult').val() == 0) {
            $('#IndexSearchEngine2_txtmealadult').val(1);
        }
    });

    $('#IndexSearchEngine2_txtmealchild').prop('disabled', true);
    $(document).on('click', '.pluschdMeal', function () {
        $('#IndexSearchEngine2_txtmealchild').val(parseInt($('#IndexSearchEngine2_txtmealchild').val()) + 1);
    });
    $(document).on('click', '.minuschdMeal', function () {
        $('#IndexSearchEngine2_txtmealchild').val(parseInt($('#IndexSearchEngine2_txtmealchild').val()) - 1);
        if ($('#IndexSearchEngine2_txtmealchild').val() == 0) {
            $('#IndexSearchEngine2_txtmealchild').val(1);
        }
    });


    $("#btnMealpax").click(function () {
        debugger;
        var totalpax = parseInt($("#IndexSearchEngine2_txtmealadult").val()) + parseInt($("#IndexSearchEngine2_txtmealchild").val());
        $('#IndexSearchEngine2_txttotnoofpaxMeal').val(totalpax);
        $('#IndexSearchEngine2_PkgnoofADT').val($('#IndexSearchEngine2_txtmealadult').val());
        $('#IndexSearchEngine2_PkgnoofChd').val($('#IndexSearchEngine2_txtmealchild').val());
        //document.getElementById("totalnoofpaxActivicy").textContent = totalpax;
        $('#IndexSearchEngine2_txttotnoofpaxMeal').trigger('click');
    });
    $(document).on('click', '#IndexSearchEngine2_txttotnoofpaxMeal', function () {
        //debugger;
        $(this).prop('Counter', 0).animate({
            Counter: $(this).text()
        }, {
            duration: 1000,
            easing: 'swing',
            step: function (now) {
                $(this).text(Math.ceil(now));
            }
        });
    });

});

