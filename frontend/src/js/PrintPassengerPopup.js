
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
                }),
                dataType: 'json',
                success: function (response) {
                    if (response.success) {
                        alert("Email sent successfully!");
                        ShowEmailDetails(response.message);
                    } else {
                        alert(response.message || "Error: Email could not be sent.");
                    }
                },
                error: function (jqXHR, textStatus, errorThrown) {
                  
                }
            });
        });
    });

    $(document).ready(function () {
        $('#createPdf').click(function () {
            var bookingRef = '@(Model.BookingRef)';
            var paxSegmentId = '@(Model.Pax_SegmentId)';
            var encodedBookingRef = btoa(bookingRef);
            if (bookingRef.trim().length > 0) {
                var url = '/PrintPopup/PrintPassengerPopupExportToPDF?bookingRef=' + bookingRef + "paxSegmentID=" + paxSegmentId;
                window.open(url, '_blank');
            } else {
                alert("Booking reference is missing.");
            }
        });
    });
