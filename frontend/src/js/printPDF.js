$(document).ready(function () {
    // Common function to handle PDF export
    function exportToPDF(url, data, defaultFileName) {
        $.ajax({
            url: url,
            type: 'POST',
            contentType: 'application/json; charset=utf-8',
            data: JSON.stringify(data),
            xhrFields: {
                responseType: 'blob'
            },
            success: function (response, textStatus, jqXHR) {
                var contentDisposition = jqXHR.getResponseHeader('Content-Disposition');
                var fileName = defaultFileName;
                if (contentDisposition) {
                    var matches = /filename="([^"]*)"/.exec(contentDisposition);
                    if (matches != null && matches[1]) {
                        fileName = matches[1];
                    }
                }
                var blob = response;
                var link = document.createElement('a');
                link.href = URL.createObjectURL(blob);
                link.download = fileName;
                link.click();
            },
            error: function (jqXHR, textStatus, errorThrown) {
                console.error("Error occurred: ", errorThrown);
            }
        });
    }

    // Button click for Invoice PDF
    $("#createPdfInvoicee").click(function (e) {
        e.preventDefault();
        var bookingId = $('#bookingRef').val();
        var decodedBookingId = atob(bookingId);

        var data = {
            BookingRef: bookingId
        };
        
        exportToPDF('/PrintPopup/ExportInvoiceToPDF', data, 'AirInvoice_' + decodedBookingId + '.pdf');
    });
    $("#createPdfPrintTicket").click(function (e) {
        e.preventDefault();
    
        var bookingId = $('#bookingRef').val();
        var fare = $('#hidnhidefare').val();
        var showLogo = $('#hidnshowlogo').val();
        var tax = $('#hidntaxx').val();
        var discount = $('#hdndiscount').val();
        var decodedBookingId = atob(bookingId);
    
        var data = {
            BookingRef: bookingId,
            Tax: tax,
            ShowLogo: showLogo,
            Hidefare: fare,
            Disc: discount
        };
    
        exportToPDF('/PrintPopup/PrintTicketExportToPDF', data, 'Print_' + decodedBookingId + '.pdf');
    });
    

    // Button click for Print Popup PDF
    $("#createPdfPrint").click(function (e) {
        e.preventDefault();

        var bookingId = $('#bookingRef').val();
        var fare = $('#hidnhidefare').val();
        var showLogo = $('#hidnshowlogo').val();
        var tax = $('#hidntaxx').val();
        var discount = $('#hdndiscount').val();
        var decodedBookingId = atob(bookingId);

        var data = {
            BookingRef: bookingId,
            Tax: tax,
            ShowLogo: showLogo,
            Hidefare: fare,
            Disc: discount
        };

        exportToPDF('/PrintPopup/PrintPopupExportToPDF', data, 'Print_' + decodedBookingId + '.pdf');
    });

    // Button click for Passenger Popup PDF
    $("#createPdfPaxee").click(function (e) {
        e.preventDefault();

        var bookingId = $('#bookingRef').val();
        var fare = $('#hidnhidefare').val();
        var showLogo = $('#hidnshowlogo').val();
        var tax = $('#hidntaxx').val();
        var discount = $('#hdndiscount').val();
        var segment = $('#hidnsegmentID').val();
        var decodedBookingId = atob(bookingId);

        var data = {
            BookingRef: bookingId,
            Tax: tax,
            ShowLogo: showLogo,
            Hidefare: fare,
            Disc: discount,
            Pax_SegmentId: segment
        };

        exportToPDF('/PrintPopup/PrintPassengerPopupExportToPDF', data, 'Print_' + decodedBookingId + '.pdf');
    });
});
