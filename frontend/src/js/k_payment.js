jQuery(document).ready(function () {
    debugger;
    var Adlt = $("#Adlt").val();
    var chld = $("#Child").val();
    var Inf = $("#Infant").val();
    var cmpid1 = document.getElementById("cmpid").value;
	
	
    //if (cmpid1.indexOf("C-") > -1 || cmpid1 == "")
    //{
    //    document.getElementById('txtEmailAdd').readOnly = true;
    //}

	    $('#btnFinalPayNow').click(function () {
        debugger;
        if (validatePaymentGatwaySelection()) {
            
            

                document.getElementById("myModal").style.display = "block";
                var PaxCharge = $('#hdnPaymentcharge').val();
                var hostName = $('#hdnhostName').val();
                var regemail = $("#hdnregisteremail").val();
				
                $.ajax({
                    type: "POST",
                    url: '//' + hostName + '/k_payment.aspx/Finalsubmition',
                    data: JSON.stringify({ 'PaxChargeDetails': PaxCharge, 'RegisEmail': regemail }),
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: OnFinalSuccess2,
                    //success: function (response) {

                    //},

                    failure: function (response) {
                        alert(response.d);
                    }
                });
            
            // after complete the process

        }
    });
});

function OnFinalSuccess2(response) {

    debugger;

    if (response.d.length > 1) {
        //popupTicketList(response);
        
        var bookrefList = "";
        jQuery('#printTicketList').html('');
            for (let i = 0; i < response.d.length; i++) {
                var result1 = response.d[i];
                /*alert("Press OK to Print next trip ticket =>" + (i + 1));
                OnFinalSuccess(response.d[i]);
                break;*/

                /*if (bookrefList != "") {
                    bookrefList += "|";
                }
                var rerI__ = btoa((parseInt(result1.split("^")[0])).toString());

                bookrefList += rerI__;*/
                var tktData = [{ result: result1, _SrNo: ((i + 1).toString())}];
                
                $("#MyTemplatePrintTickets").tmpl(tktData).appendTo("#printTicketList");
                
        }
        //alert("Booking success...!\n\n " + bookrefList);
        $("#myModal").css("display", "none");
        $("#PrintTicketPopup").css("display", "Block");

    } else {
        OnFinalSuccess(response.d[0]);
    }

}

function OnFinalSuccess(response) {
    OnFinalSuccessNew(response,"");
}


function OnFinalSuccessNew(response,SearchType) {

    debugger;
    
    var result = response;
    var ptype = result.split("!")[1];
    var bookref = parseInt(result.split("^")[0]);
    
    var cardtype = result.split("^")[1];
    var url = result.split("^")[2].split("!")[0];
    var paymentid = parseInt(result.split("!")[2]);

        if (result != "") {
            var hostName = $('#hdnhostName').val();
			if (bookref > 0 && (ptype.toUpperCase() == "PREPAID" || ptype.toUpperCase() == "PAYMENTHOLD")) {
            //if (bookref > 0 && ptype.toUpperCase() == "PREPAID") {
                //location.href = "//" + hostName + "/Print_Ticket.aspx?BookingRef=" + btoa(bookref.toString());
                //location.href = "//" + hostName + "/Print_Ticket.aspx?BookingRef=" + (bookrefList.toString());
				if (SearchType === "MC") {
                    var urlToOpen = "//" + hostName + "/Print_Ticket.aspx?BookingRef=" + btoa(bookref.toString());
                    window.open(urlToOpen);
                } else {
                    location.href = "//" + hostName + "/Print_Ticket.aspx?BookingRef=" + btoa(bookref.toString());
                }
            }
            else if (paymentid > 0 && ptype.toUpperCase() != "PREPAID") {
                window.location = url;
            }
            else if (bookref == 0 || paymentid == 0) {
                location.href = "//" + hostName + "/errorpage";
            }
            else {
                window.location = url;
            }
        }
        else { location.href = "//" + hostName + "/Index.aspx"; }
   
}

function ClosePrintTicketPopup() {
    $("#PrintTicketPopup").css("display", "none");
    var hostName = $('#hdnhostName').val();
    location.href = "//" + hostName + "/Flight/Index";
    
}
window.ClosePrintTicketPopup = ClosePrintTicketPopup;

/*function OnFinalSuccess(response) {
    var result = response.d;
    var ptype = result.split("!")[1];
    var bookref = parseInt(result.split("^")[0]);
    var cardtype = result.split("^")[1];
    var url = result.split("^")[2].split("!")[0];
    var paymentid = parseInt(result.split("!")[2]);
    if (result != "") {
        var hostName = $('#hdnhostName').val();
        if (bookref > 0 && ptype.toUpperCase() == "PREPAID") {
            location.href = "//" + hostName + "/Print_Ticket.aspx?BookingRef=" + btoa(bookref.toString());
        }
        else if (paymentid > 0 && ptype.toUpperCase() != "PREPAID") {
            window.location = url;
        }
        else if (bookref == 0 || paymentid == 0) {
            location.href = "//" + hostName + "/errorPage.aspx?errorMSG=" + url;
        }
        else {
            window.location = url;
        }
    }
    else { location.href = "//" + hostName + "/Index.aspx"; }
}*/
function prepaid() {
    debugger;
    var fare = $('#HiddenTotalfare').val();
    var dis = document.getElementById("HiddenDiscount").value;
    var finalFare = parseInt($('#HiddenTotalfare').val());
    var disco = $('#HiddenDiscount').val();
    $('#sidecommission').html(disco);
    var ttotCFee = $('#hdnTotCfee').val();
    $('#FlightdisplayCfee').html(ttotCFee);
    var markup = $('#TToalmarkup').val();
    var tds = $('#TotTDS').val();

    var cmpid1 = document.getElementById("cmpid").value;
    if (cmpid1.indexOf("C-") > -1 || cmpid1 == "")
    {
        var totf = parseInt(disco) + finalFare;
        $('#sidegrossfare').html(totf);
        var netfare = parseInt(finalFare);
        var totnetfare = parseInt(ttotCFee) + netfare;
        $('#sideNetfare').html(totnetfare);
    }
    else
    {
        var netfare = parseInt(finalFare) - parseInt(disco);
        netfare = (netfare + parseInt($('#TotTDS').val())) - parseInt($('#TToalmarkup').val());
        $('#sidegrossfare').html(finalFare);
        var totnetfare = parseInt(ttotCFee) + netfare;
        $('#sideNetfare').html(totnetfare);
        $('#FlightdisplayTDS').html($('#TotTDS').val());
    }
    $('#HiddenTotalfare').val(fare);
    $('#HiddenDiscount').val(disco);
    $('#TotTDS').val(tds);
    $('#TToalmarkup').val(markup);
}
function calulateCharge(_FIXED, _Charges, _PERCNT) {
    debugger;
    var PG_Name = $("input[name='optradio']:checked").val();
    var pgnme = $("input[name='optradio']:checked").val().split("|")[0];
    var hhdncardName = $("input[name='optradio']:checked").val().split("|")[1];
    var _MerchantCode = $("input[name='optradio']:checked").val().split("|")[2];

    var chargestr = "FIXED--" + _FIXED + ",Charges--" + _Charges + ",PERCNT--" + _PERCNT + ",PG_Name--" + pgnme + ",CardName--" + hhdncardName + ",MerchantCode--" + _MerchantCode;
    $('input[id$=hdnPaymentcharge]').val(chargestr);

    var TotalFare = parseInt($('#HiddenTotalfare').val());
    var disco = document.getElementById("HiddenDiscount").value;
    var cmpid1 = document.getElementById("cmpid").value;
    var PrevnetFare;
    if (cmpid1.indexOf("C-") > -1 || cmpid1 == "") {
        PrevnetFare = TotalFare;
        var totf = parseInt(disco) + TotalFare;
        $('#sidegrossfare').html(totf);
    }
    else {
        PrevnetFare = TotalFare - parseInt(disco);
        $('#sidegrossfare').html(TotalFare);
    }

    var ttotCFee = 0;
    var isValidcfee = $('#hdnValidcfee').val();
    if (isValidcfee == "0" && _MerchantCode.indexOf("Prepaid") > -1) {
        ttotCFee = 0;
        $('#FlightdisplayCfee').html(ttotCFee);
    }
    else {
        ttotCFee = $('#hdnTotCfee').val();
        $('#FlightdisplayCfee').html(ttotCFee);
    }

    $('#sidecommission').html(disco);

    var NetFare = TotalFare;
    var TotalTax = 0; var CompleteFare = TotalFare;
    var CompleteFareNet = PrevnetFare;
    var netTax = 0;

    if (_FIXED == true) {
        CompleteFare = TotalFare + parseInt(_Charges);
        CompleteFareNet = PrevnetFare + parseInt(_Charges);

        netTax = (PrevnetFare + Math.round(parseInt(_Charges)));
        $('#conveniecfee').html(Math.round(parseInt(_Charges)));
    }
    else if (_PERCNT == true) {

        TotalTax = (TotalFare * parseFloat(_Charges)) / 100;
        CompleteFare = Math.round(TotalTax) + TotalFare;
        netTax = ((parseInt(ttotCFee) + PrevnetFare) * parseFloat(_Charges)) / 100;
        CompleteFareNet = netTax + PrevnetFare;

        $('#conveniecfee').html(Math.round(netTax));
    }

    var finalFare = Math.round(CompleteFare);
    var finalNetFare = Math.round(CompleteFareNet);

    if (cmpid1.indexOf("C-") > -1 || cmpid1 == "") {
        var netfare = parseInt(finalNetFare);
        var totnetfare = parseInt(ttotCFee) + netfare;
        $('#sideNetfare').html(totnetfare);

    }
    else {
        var netfare = parseInt(finalNetFare);
        netfare = (netfare + parseInt($('#TotTDS').val())) - parseInt($('#TToalmarkup').val());
        var totnetfare = parseInt(ttotCFee) + netfare;
        $('#sideNetfare').html(totnetfare);
        $('#FlightdisplayTDS').html($('#TotTDS').val());

    }

    $('#Finalcharges').html(netTax);
}
function validatePaymentGatwaySelection() {
    if ($("input:radio[name=optradio]").is(":checked")) {
	//if ($("input:radio[name=optradio]").is(":checked") || ($('#chkIsHold').is(":checked"))) {    
    // alert('successfull');
        return true;
    }
    else {
        alert("Kindly select any one payment option...");
        return false;
    }
}


