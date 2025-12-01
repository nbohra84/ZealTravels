document.addEventListener('DOMContentLoaded', function() {
    // Use querySelectorAll to select all elements whose id starts with 'bokref_' 
    var links = document.querySelectorAll('[id^="Print_"]');

    // Loop through each matching element and attach the event listener
    links.forEach(function(link) {
        link.addEventListener('click', function(event) {
            bookrefff(event.target); 
        });
    });

    var printButton = document.getElementById('printTicket');

    printButton.addEventListener('click', function() {
        window.print();  
    });
});

function printinvoice(element) {
    var bookingRef = element.getAttribute('accesskey');

    if (!bookingRef) {
        return;
    }

    var encodedBookingRef = btoa(bookingRef);

    window.open("/printInvoice?BookingRef=" + encodedBookingRef,
        "PopupInvoice",
        "location=1,status=1,scrollbars=1,resizable=1,directories=1,toolbar=1,titlebar=1,width=900,height=800");
}

$(document).ready(function () {
    function checkDateDifference() {
        var fromDate = $("#txtfromdate").val();
        var toDate = $("#txtTodate").val();

        if (fromDate && toDate) {
            var fromDateObj = new Date(fromDate);
            var toDateObj = new Date(toDate);

            var timeDifference = toDateObj.getTime() - fromDateObj.getTime();
            var dayDifference = timeDifference / (1000 * 3600 * 24);

            if (dayDifference > 31) {
                $("#errorDiv").show();
            } else {
                $("#errorDiv").hide();
            }
        }
    }

    $("#txtfromdate, #txtTodate").change(function () {
        checkDateDifference();
    });
});

function Perpaxticket(abc) {
    var result = abc.getAttribute('accesskey');

    if (!result) {
        return; 
    }

    // Split the result to get bookingref and segM_ID
    var splitResult = result.split(",");
    var bookingref = splitResult[0];  
    var segM_ID = splitResult[1];   

    var encodedBookingRef = btoa(bookingref);
    var encodedSegM_ID = btoa(segM_ID);

    window.open("/PrintPaxPopup?BookingRef=" + encodedBookingRef + "&PaxSegmentID=" + encodedSegM_ID,
        "Popup51",
        "location=1,status=1,scrollbars=1,resizable=1,directories=1,toolbar=1,titlebar=1,width=940,height=800");
}

function bookrefff(element) {
    var result = element.getAttribute('accesskey');


    if (!result) {
        return;
    }
    var encodedBookingRef = btoa(result); 
    window.open("/PrintPopup?BookingRef=" + encodedBookingRef, 
        "Popup3", 
        "location=1,status=1,scrollbars=1,resizable=1,directories=1,toolbar=1,titlebar=1,width=900,height=800");
}

window.bookrefff = bookrefff;
window.Perpaxticket = Perpaxticket;
window.printinvoice = printinvoice;

$("#ContentPlaceHolder1_companyname").autocomplete({
    source: function (request, response) {
        //debugger;
        $.ajax({
            url: 'Ticket_Report.aspx/getcompnydata',
            data: "{ 'srchtxt': '" + request.term + "'}",
            dataType: "json",
            type: "POST",
            contentType: "application/json; charset=utf-8",
            success: function (data) {

                //var obj = JSON.parse(data.d);

                response($.map(data.d, function (item) {
                    return {
                        label: item,
                        value: item
                    }

                }));
            }
        });

    },

    minLength: 3,
    selectFirst: true,
    highlight: true,
    autoFill: true,
    cacheLength: 10,
    autoFocus: true,
    select: function (event, ui) {
        if ($("#ContentPlaceHolder1_companyname").value === ui.item.label) {
        }
    },
    open: function () {
        $(this).removeClass("ui-corner-all").addClass("ui-corner-top");
    },
    close: function () {
        $(this).removeClass("ui-corner-top").addClass("ui-corner-all");
    },
    search: function (e, u) {
        $(this).addClass('p_loader');
    },
    response: function (e, u) {
        $(this).removeClass('p_loader');
    }
});
$("#ContentPlaceHolder1_restricpax").keypress(function (event) {
    //debugger;
    var inputValue = event.charCode;
    if (!(inputValue >= 65 && inputValue <= 122) && (inputValue != 32 && inputValue != 0)) {
        //debugger;
        event.preventDefault();
    }
    else {
        if ($("#ContentPlaceHolder1_restricpax").val().length >= 3) {
            document.getElementById("searchbydatediv").style.display = "block";
        }
        else {
            document.getElementById("searchbydatediv").style.display = "none";
        }
    }
});
// });

//<![CDATA[
    var theForm = document.forms['form1'];
    if (!theForm) {
        theForm = document.form1;
    }
    function __doPostBack(eventTarget, eventArgument) {
        if (!theForm.onsubmit || (theForm.onsubmit() != false)) {
            theForm.__EVENTTARGET.value = eventTarget;
            theForm.__EVENTARGUMENT.value = eventArgument;
            theForm.submit();
        }
    }
    //]]>

    //<![CDATA[
Sys.WebForms.PageRequestManager._initialize('ctl00$scriptmnger1', 'form1', ['tctl00$ContentPlaceHolder1$UpdatePanel1','ContentPlaceHolder1_UpdatePanel1'], [], ['ctl00$ContentPlaceHolder1$btnexpExcel','ContentPlaceHolder1_btnexpExcel','ctl00$ContentPlaceHolder1$btnExpPDF','ContentPlaceHolder1_btnExpPDF'], 90, 'ctl00');
//]]>

function myclipopse() {
    debugger;
    var ll = document.getElementById('closclippo');

    if (ll.style.display != "block") {
        ll.style.display = "block";
        $('#sedax').css("box-shadow", "0 8px 8px rgba(0,0,0,0.5)");
    }
    else {
        ll.style.display = "none";
        $('#sedax').css("box-shadow", "0 0px 0px rgba(0,0,0,0.5)");
    }
}


$(document).ready(function () {
    var touch = $('#resp-menu');
    var menu = $('.menu');

    $(touch).on('click', function (e) {
        e.preventDefault();
        menu.slideToggle();
    });

    $(window).resize(function () {
        var w = $(window).width();
        if (w > 767 && menu.is(':hidden')) {
            menu.removeAttr('style');
        }
    }); 
});

function googleTranslateElementInit() {
    new google.translate.TranslateElement({pageLanguage: 'en'}, 'google_translate_element');
  }

  Sys.Application.add_load(LoadScript);
  function Openpopup() {
      //debugger;
      var res = getDatediffrence();
      if (res) {
          $("#modalloading").css("display", 'block');
      }
      else {
          return false
      }
  }
  function closepopup() {
      //debugger;
      $("#modalloading").css("display", 'none');
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

  document.addEventListener("DOMContentLoaded", function() {
    // Add event listeners to all open modal buttons
    const openModalButtons = document.querySelectorAll('[id^="openModal_"]');
    openModalButtons.forEach(button => {
        button.addEventListener('click', function() {
            const modalId = this.id.replace('openModal_', 'myModal_'); 
        });
    });

    // Add event listeners to all close buttons
    const closeButtons = document.querySelectorAll('[id^="closeButton_"]');
    closeButtons.forEach(button => {
        button.addEventListener('click', function() {
            const modalId = this.getAttribute('data-modal-id');
            const modal = document.getElementById(modalId);
            if (modal) {
                modal.style.display = 'none';
                modal.style.pointerEvents = 'none';
            }
        });
    });
});

$(document).ready(function () {
function showdetail(modalId) {
    var modal = document.getElementById(modalId);
    if (modal) {
        modal.style.pointerEvents = 'auto'; 
        modal.style.display = 'block'; 
    }
}
window.showdetail = showdetail;
})


$(document).ready(function() {
    $("a[accesskey][data-type='perpaxticket']").on("click", function(event) {
        event.preventDefault();  // Prevent default behavior
        Perpaxticket(this);      // Call the Perpaxticket function
    });
});
