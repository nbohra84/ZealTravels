var arroldtaxpaxwise = [];
var arroldtotalpaxwise = [];
var temptax = 0;
var temptotalfare = 0;
var count1 = 0;
var count = 0;
var emailReg = /^([\w-\.]+@([\w-]+\.)+[\w-]{2,4})?$/;
$(document).ready(function () {
    $('#btnEditTransactionFee').click(function (e) {
        debugger;
        if (this.value == 'Add Markup') {
            e.preventDefault();
            this.value = 'Apply Markup';
            $('#divEditTxnFeeDiv').show();
        }
        else {
            if ($.trim($('#CurrentTxnFee').val()) == '') {
                e.preventDefault();
                alert('Transaction Fee Should not empty.');
            }
            else if (validateDecimal($('#CurrentTxnFee').val())) {
                debugger;
                $('#Action').val('TransFee');

                $('#hidntaxx').val($('#CurrentTxnFee').val());

                if (count1 == 0) {
                    debugger;
                    var arrnopax = [];
                    if ($('#hdnisSinglefare').val() == "True") {
                        var td = document.getElementsByClassName('frebrkuptsx');
                        for (var t = 0; t < td.length; t++) {
                            if (td[t].className == "frebrkuptsx") {
                                var farebreakupoldtaxOB = parseInt(td[t].innerHTML);
                                arroldtaxpaxwise.push(parseInt(td[t].innerHTML));
                                var farebreakuptotOB = parseInt($('#CurrentTxnFee').val()) + farebreakupoldtaxOB;
                                td[t].innerHTML = farebreakuptotOB;

                            }
                        }

                        var tdtotal = document.getElementsByClassName('frebrkuptotl');
                        for (var t = 0; t < tdtotal.length; t++) {
                            if (tdtotal[t].className == "frebrkuptotl") {
                                var totalfatre = tdtotal[t].innerHTML;
                                var fare = parseInt(totalfatre.split("X")[0].trim());
                                arroldtotalpaxwise.push(fare);
                                var noofpax = parseInt(totalfatre.split("X")[1].trim());
                                arrnopax.push(noofpax);
                                var total = fare + parseInt($('#CurrentTxnFee').val());
                                tdtotal[t].innerHTML = total + " X " + noofpax;
                            }
                        }

                    }
                    else {


                        var td = document.getElementsByClassName('frebrkuptsx');
                        for (var t = 0; t < td.length; t++) {
                            if (td[t].className == "frebrkuptsx") {
                                var farebreakupoldtaxOB = parseInt(td[t].innerHTML);
                                arroldtaxpaxwise.push(parseInt(td[t].innerHTML));
                                var farebreakuptotOB = parseInt($('#CurrentTxnFee').val()) + farebreakupoldtaxOB;
                                td[t].innerHTML = farebreakuptotOB;
                            }
                        }

                        var tdtotal = document.getElementsByClassName('frebrkuptotl');
                        for (var t = 0; t < tdtotal.length; t++) {
                            if (tdtotal[t].className == "frebrkuptotl") {
                                var totalfatre = tdtotal[t].innerHTML;
                                var fare = parseInt(totalfatre.split("X")[0].trim());
                                arroldtotalpaxwise.push(fare);
                                var noofpax = parseInt(totalfatre.split("X")[1].trim());
                                arrnopax.push(noofpax);
                                var total = fare + parseInt($('#CurrentTxnFee').val());
                                tdtotal[t].innerHTML = total + " X " + noofpax;
                            }
                        }
                    }

                    var totalpax = 0;
                    for (var i = 0; i < arrnopax.length; i++) {
                        totalpax = totalpax + arrnopax[i];
                    }
                    var oldtax = parseInt(document.getElementById("oldtotaltax").innerHTML);
                    temptax = oldtax;
                    var tottax = parseInt($('#CurrentTxnFee').val()) * totalpax + oldtax;
                    document.getElementById("oldtotaltax").innerHTML = tottax;

                    var oldamt = parseInt(document.getElementById("oldtotalamt").innerHTML);
                    temptotalfare = oldamt;
                    var totamt = parseInt($('#CurrentTxnFee').val()) * totalpax + oldamt;
                    document.getElementById("oldtotalamt").innerHTML = totamt;

                    $('#CurrentTxnFee').val('');
                    this.value = 'Add Markup';
                    $('#divEditTxnFeeDiv').hide();
                }

                else {
                    debugger;
                    var arrnopax = [];
                    if ($('#hdnisSinglefare').val() == "True") {
                        var td = document.getElementsByClassName('frebrkuptsx');
                        for (var t = 0; t < td.length; t++) {
                            if (td[t].className == "frebrkuptsx") {
                                var farebreakuptotOB = parseInt($('#CurrentTxnFee').val()) + arroldtaxpaxwise[t];
                                td[t].innerHTML = farebreakuptotOB;
                            }
                        }
                        var tdtotal = document.getElementsByClassName('frebrkuptotl');
                        for (var t = 0; t < tdtotal.length; t++) {
                            if (tdtotal[t].className == "frebrkuptotl") {
                                var totalfatre = tdtotal[t].innerHTML;
                                var fare = arroldtotalpaxwise[t];
                                var noofpax = parseInt(totalfatre.split("X")[1].trim());
                                arrnopax.push(noofpax);
                                var total = fare + parseInt($('#CurrentTxnFee').val());
                                tdtotal[t].innerHTML = total + " X " + noofpax;
                            }
                        }

                    }
                    else {
                        var td = document.getElementsByClassName('frebrkuptsx');
                        for (var t = 0; t < td.length; t++) {
                            if (td[t].className == "frebrkuptsx") {
                                var farebreakuptotOB = parseInt($('#CurrentTxnFee').val()) + arroldtaxpaxwise[t];
                                td[t].innerHTML = farebreakuptotOB;
                            }
                        }
                        var tdtotal = document.getElementsByClassName('frebrkuptotl');
                        for (var t = 0; t < tdtotal.length; t++) {
                            if (tdtotal[t].className == "frebrkuptotl") {
                                var totalfatre = tdtotal[t].innerHTML;
                                var fare = arroldtotalpaxwise[t];
                                var noofpax = parseInt(totalfatre.split("X")[1].trim());
                                arrnopax.push(noofpax);
                                var total = fare + parseInt($('#CurrentTxnFee').val());
                                tdtotal[t].innerHTML = total + " X " + noofpax;
                            }
                        }
                    }
                    debugger;
                    var totalpax = 0;
                    for (var i = 0; i < arrnopax.length; i++) {
                        totalpax = totalpax + arrnopax[i];
                    }
                    var tottax = parseInt($('#CurrentTxnFee').val()) * totalpax + temptax;
                    document.getElementById("oldtotaltax").innerHTML = tottax;

                    var totamt = parseInt($('#CurrentTxnFee').val()) * totalpax + temptotalfare;
                    document.getElementById("oldtotalamt").innerHTML = totamt;

                    $('#CurrentTxnFee').val('');
                    this.value = 'Add Markup';
                    $('#divEditTxnFeeDiv').hide();
                    // count1 = count1 + 1;
                }
                count1 = count1 + 1;
            }
            else {
                e.preventDefault();
                alert('Please Enter Decimal only as a Transaction Fee.');
            }
        }
    });
    function validateDecimal(comvalue) {
        var RE = /^\d*(\.\d{1})?\d{0,1}$/;
        if (RE.test(comvalue)) {
            return true;
        } else {
            return false;
        }
    }
    $('#btnAddDiscount').click(function (e) {
        debugger;
        if (this.value == 'Add Discount') {
            e.preventDefault();
            this.value = 'Apply Discount';
            $('#CurrentDiscount').val(document.getElementById("spndiscount").innerHTML);
            $('#Discountdiv').hide();
            $('#divApplyDiscount').show();
        }
        else {
            if ($.trim($('#CurrentDiscount').val()) == '') {
                e.preventDefault();
                alert('Discount Amount Should not empty.');
            }
            else if (validateDecimal($('#CurrentDiscount').val())) {
                debugger;
                var oldamt = parseInt(document.getElementById("oldtotalamt").innerHTML);
                var totamt = 0;
                if (count == 0) {
                    totamt = oldamt - parseInt($('#CurrentDiscount').val());
                }
                else {
                    totamt = (oldamt + parseInt(document.getElementById("spndiscount").innerHTML)) - parseInt($('#CurrentDiscount').val());
                }
                $('#Action').val('Discount');
                $('#hdndiscount').val($('#CurrentDiscount').val());
                document.getElementById("oldtotalamt").innerHTML = totamt;
                $('#Discountdiv').show();
                var totdisc = parseInt($('#CurrentDiscount').val());
                document.getElementById("spndiscount").innerHTML = totdisc;
                this.value = 'Add Discount';
                $('#CurrentDiscount').val('');
                $('#divApplyDiscount').hide();
                count = count + 1;
            }
            else {
                e.preventDefault();
                alert('Please Enter Decimal only as a Discount.');
            }
        }
    });
    $('#printTicket').click(function (e) {
        $("#btnPanel").hide();
        window.print();
        $("#btnPanel").show();
    });
    function printDiv(divName) {
        var printContents = document.getElementById(divName).innerHTML;
        var originalContents = document.body.innerHTML;

        document.body.innerHTML = printContents;

        window.print();

        document.body.innerHTML = originalContents;
    }
    window.printDiv = printDiv;
    //Show Block of Email
    $("#emailTicket").click(function () {
        $("#emailBlock").show();
        $('#sendEmail').show();
        $("#addressBox").val("")
        $("#addressBox").focus();
        $("#emailMsg").html("");
        return false;
    });
    //Email block should be hide when we click on cancell and close
    $("#emailCancel").click(function () {
        $("#emailBlock").hide();
        $("#emailMsg").html("");
    });

    $("#emailClose").click(function () {
        $("#emailBlock").hide();
        $("#emailMsg").html("");
    });

    $('#logobutton').click(function (e) {
        // console.log("clilcked");
        console.log($(this).val());
        
        
        if ($(this).val() == 'Show logo') {
            // console.log("clilcked");
            e.preventDefault();
            $('#hidnshowlogo').val('True');
            $(this).val('Hide logo');
            $('#logodiv').show();
        } else {
            $(this).val('Show logo');
            $('#hidnshowlogo').val('False');
            $('#logodiv').hide();
        }
    });

    $('#btnhidefare').click(function (e) {
        if (this.value == 'Hide Fare') {
            e.preventDefault();
            $('#hidnhidefare').val('True');
            this.value = 'Show Fare';
            $('#divfarebreakup').hide();
            $('#divfaredetail').hide();
            $('#divfaredetailnew').show();
        }
        else {
            this.value = 'Hide Fare';
            $('#hidnhidefare').val('False');
            $('#divfaredetailnew').hide();
            $('#divfarebreakup').show();
            $('#divfaredetail').show();
        }
    });

});


function ShowEmailDetails(resp) {
    $("#ColoredsendEmail").show();
    if (resp == "success") {
        $("#emailMessage").html("Email sent successfully.");
        $("#emailMessage").css('color', 'green');
        $("#emailBlock").hide();
    } else {
        $("#emailMessage").html("Email not sent.");
        $("#emailMessage").css('color', 'red');
        $("#emailBlock").hide();
    }
}



$(document).ready(function () {
    // Attach click handler to the "Send Email" button
    $("#ColoredsendEmail").click(function (e) {
        e.preventDefault();

        // Validate email input
        var emailVal = $.trim($("#addressBox").val());
        if (emailVal === "") {
            $("#emailMsg").html("Please enter emailId.");
            $("#emailMsg").css('color', 'red');
            $("#addressBox").focus();
            return false;
        }

        $("#emailMsg").html("Please wait, email is sending ...");
        $("#emailMsg").css('color', 'blue');
        $("#ColoredsendEmail").hide();

        // Collecting values from hidden fields
        var bookingId = $('#bookingRef').val();
        var emailId = emailVal;
        var tax = $('#hidntaxx').val();
        var disc = $('#hdndiscount').val();
        var logo = $('#hidnshowlogo').val();
        var fare = $('#hidnhidefare').val();

        // AJAX request to send the email
        $.ajax({
            url: '/PrintPopup/SendPrintPopupTicketMail',
            type: 'POST',
            contentType: 'application/json; charset=utf-8',
            data: JSON.stringify({
                BookingRef: bookingId,
                Email: emailId,
                Tax: tax,
                Logo: logo,
                HideFare: fare,
                Disc: disc
            }),
            dataType: 'json',
            success: function (response) {
                if (response.success) {
                    $("#emailBlock").hide();
                    $("#emailMsg").html("");
                    alert("Email sent successfully!");
                    ShowEmailDetails(response.message);
                } else {
                    $("#emailBlock").hide();
                    $("#emailMsg").html("");
                    alert(response.message || "Error: Email could not be sent.");
                }
            },
            error: function (jqXHR, textStatus, errorThrown) {
                alert("Error sending email, please try again later.");
            }
        });
    });
});
