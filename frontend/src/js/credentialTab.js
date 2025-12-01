var stat;
$(document).ready(function () {
    $("#btnproductstatus").click(function () {
        stat = 1;
        $("#Supplier_Detail_Hotel_Div").hide();
        $("#ContentPlaceHolder1_Supplier_Product_Status_Div").show();
        $("#Supplier_Product_Detail_Div").hide();
        $("#Supplier_Detail_Galileo_Airline_Div").hide();
        $("#Supplier_Detail_API_Airline_Div").hide();
        $("#AirlinePnrMakeDays_Div").hide();
        $("#Supplier_Detail_Div").hide();
        $("#Supplier_Detail_Lcc_Airline_Div").hide();

        $("#UAPI_FOP_Div").hide();
        $("#UAPI_CC_Div").hide();
    });
    $("#btnhoteldetail").click(function () {
        debugger;
        stat = 2;
        $("#Supplier_Detail_Hotel_Div").show();
        $("#ContentPlaceHolder1_Supplier_Product_Status_Div").hide();
        $("#Supplier_Product_Detail_Div").hide();
        $("#Supplier_Detail_Galileo_Airline_Div").hide();
        $("#Supplier_Detail_API_Airline_Div").hide();
        $("#AirlinePnrMakeDays_Div").hide();
        $("#Supplier_Detail_Div").hide();
        $("#Supplier_Detail_Lcc_Airline_Div").hide();

        $("#UAPI_FOP_Div").hide();
        $("#UAPI_CC_Div").hide();
    });
    $("#btnproductdetail").click(function () {
        debugger;
        stat = 3;
        $("#Supplier_Detail_Hotel_Div").hide();
        $("#ContentPlaceHolder1_Supplier_Product_Status_Div").hide();
        $("#Supplier_Product_Detail_Div").show();
        $("#Supplier_Detail_Galileo_Airline_Div").hide();
        $("#Supplier_Detail_API_Airline_Div").hide();
        $("#AirlinePnrMakeDays_Div").hide();
        $("#Supplier_Detail_Div").hide();
        $("#Supplier_Detail_Lcc_Airline_Div").hide();

        $("#UAPI_FOP_Div").hide();
        $("#UAPI_CC_Div").hide();
    });
    $("#btnGalileoairline").click(function () {
        debugger;
        stat = 4;
        $("#Supplier_Detail_Hotel_Div").hide();
        $("#ContentPlaceHolder1_Supplier_Product_Status_Div").hide();
        $("#Supplier_Product_Detail_Div").hide();
        $("#Supplier_Detail_Galileo_Airline_Div").show();
        $("#Supplier_Detail_API_Airline_Div").hide();
        $("#AirlinePnrMakeDays_Div").hide();
        $("#Supplier_Detail_Div").hide();
        $("#Supplier_Detail_Lcc_Airline_Div").hide();

        $("#UAPI_FOP_Div").hide();
        $("#UAPI_CC_Div").hide();
    });

    $("#btnAPIairline").click(function () {
        debugger;
        stat = 5;
        $("#Supplier_Detail_Hotel_Div").hide();
        $("#ContentPlaceHolder1_Supplier_Product_Status_Div").hide();
        $("#Supplier_Product_Detail_Div").hide();
        $("#Supplier_Detail_Galileo_Airline_Div").hide();
        $("#Supplier_Detail_API_Airline_Div").show();
        $("#AirlinePnrMakeDays_Div").hide();
        $("#Supplier_Detail_Div").hide();
        $("#Supplier_Detail_Lcc_Airline_Div").hide();
        $("#UAPI_FOP_Div").hide();
        $("#UAPI_CC_Div").hide();

    });

    $("#btnSupplierdetail").click(function () {
        debugger;
        stat = 6;
        $("#Supplier_Detail_Hotel_Div").hide();
        $("#ContentPlaceHolder1_Supplier_Product_Status_Div").hide();
        $("#Supplier_Product_Detail_Div").hide();
        $("#Supplier_Detail_Galileo_Airline_Div").hide();
        $("#Supplier_Detail_API_Airline_Div").hide();
        $("#AirlinePnrMakeDays_Div").hide();
        $("#Supplier_Detail_Div").show();
        $("#Supplier_Detail_Lcc_Airline_Div").hide();

        $("#UAPI_FOP_Div").hide();
        $("#UAPI_CC_Div").hide();

    });

    $("#btnLccAirline").click(function () {
        debugger;
        stat = 7;
        $("#Supplier_Detail_Hotel_Div").hide();
        $("#ContentPlaceHolder1_Supplier_Product_Status_Div").hide();
        $("#Supplier_Product_Detail_Div").hide();
        $("#Supplier_Detail_Galileo_Airline_Div").hide();
        $("#Supplier_Detail_API_Airline_Div").hide();
        $("#Supplier_Detail_Div").hide();
        $("#AirlinePnrMakeDays_Div").hide();
        $("#Supplier_Detail_Lcc_Airline_Div").show();

        $("#UAPI_FOP_Div").hide();
        $("#UAPI_CC_Div").hide();
    });


    $("#btnPnrMakeDaysAirline").click(function () {
        debugger;
        stat = 8;
        $("#Supplier_Detail_Hotel_Div").hide();
        $("#ContentPlaceHolder1_Supplier_Product_Status_Div").hide();
        $("#Supplier_Product_Detail_Div").hide();
        $("#Supplier_Detail_Galileo_Airline_Div").hide();
        $("#Supplier_Detail_API_Airline_Div").hide();
        $("#Supplier_Detail_Div").hide();
        $("#Supplier_Detail_Lcc_Airline_Div").hide();
        $("#AirlinePnrMakeDays_Div").show();

        $("#UAPI_FOP_Div").hide();
        $("#UAPI_CC_Div").hide();
    });

    $("#btnUAPIFOP").click(function () {
        debugger;
        stat = 9;
        $("#Supplier_Detail_Hotel_Div").hide();
        $("#ContentPlaceHolder1_Supplier_Product_Status_Div").hide();
        $("#Supplier_Product_Detail_Div").hide();
        $("#Supplier_Detail_Galileo_Airline_Div").hide();
        $("#Supplier_Detail_API_Airline_Div").hide();
        $("#Supplier_Detail_Div").hide();
        $("#Supplier_Detail_Lcc_Airline_Div").hide();
        $("#AirlinePnrMakeDays_Div").hide();

        $("#UAPI_FOP_Div").show();
        $("#UAPI_CC_Div").hide();
    });

    $("#btnUAPICCdetail").click(function () {
        debugger;
        stat = 10;
        $("#Supplier_Detail_Hotel_Div").hide();
        $("#ContentPlaceHolder1_Supplier_Product_Status_Div").hide();
        $("#Supplier_Product_Detail_Div").hide();
        $("#Supplier_Detail_Galileo_Airline_Div").hide();
        $("#Supplier_Detail_API_Airline_Div").hide();
        $("#Supplier_Detail_Div").hide();
        $("#Supplier_Detail_Lcc_Airline_Div").hide();
        $("#AirlinePnrMakeDays_Div").hide();

        $("#UAPI_FOP_Div").hide();
        $("#UAPI_CC_Div").show();
    });

})
window.onload = function () {
    function crossdivcred() {
        debugger;

        $("#ContentPlaceHolder1_mpeDetails_backgroundElement").hide();
        $("#ContentPlaceHolder1_pnlDetails").hide();

    }
    window.crossdivcred = crossdivcred;
    function MyFunction() {
        try {
            if (stat == 1) {
                $("#Supplier_Detail_Hotel_Div").hide();
                $("#ContentPlaceHolder1_Supplier_Product_Status_Div").show();
                $("#Supplier_Product_Detail_Div").hide();
                $("#Supplier_Detail_Galileo_Airline_Div").hide();
                $("#Supplier_Detail_API_Airline_Div").hide();
                $("#Supplier_Detail_Div").hide();
                $("#AirlinePnrMakeDays_Div").hide();
                $("#Supplier_Detail_Lcc_Airline_Div").hide();

                $("#UAPI_FOP_Div").hide();
                $("#UAPI_CC_Div").hide();
            }
            if (stat == 2) {
                $("#Supplier_Detail_Hotel_Div").show();
                $("#ContentPlaceHolder1_Supplier_Product_Status_Div").hide();
                $("#Supplier_Product_Detail_Div").hide();
                $("#Supplier_Detail_Galileo_Airline_Div").hide();
                $("#Supplier_Detail_API_Airline_Div").hide();
                $("#Supplier_Detail_Div").hide();
                $("#AirlinePnrMakeDays_Div").hide();
                $("#Supplier_Detail_Lcc_Airline_Div").hide();

                $("#UAPI_FOP_Div").hide();
                $("#UAPI_CC_Div").hide();
            }
            if (stat == 3) {
                $("#Supplier_Detail_Hotel_Div").hide();
                $("#ContentPlaceHolder1_Supplier_Product_Status_Div").hide();
                $("#Supplier_Product_Detail_Div").show();
                $("#Supplier_Detail_Galileo_Airline_Div").hide();
                $("#Supplier_Detail_API_Airline_Div").hide();
                $("#Supplier_Detail_Div").hide();
                $("#AirlinePnrMakeDays_Div").hide();
                $("#Supplier_Detail_Lcc_Airline_Div").hide();
                $("#UAPI_FOP_Div").hide();
                $("#UAPI_CC_Div").hide();

            }
            if (stat == 4) {
                $("#Supplier_Detail_Hotel_Div").hide();
                $("#ContentPlaceHolder1_Supplier_Product_Status_Div").hide();
                $("#Supplier_Product_Detail_Div").hide();
                $("#Supplier_Detail_Galileo_Airline_Div").show();
                $("#Supplier_Detail_API_Airline_Div").hide();
                $("#Supplier_Detail_Div").hide();
                $("#AirlinePnrMakeDays_Div").hide();
                $("#Supplier_Detail_Lcc_Airline_Div").hide();

                $("#UAPI_FOP_Div").hide();
                $("#UAPI_CC_Div").hide();
            }
            if (stat == 5) {
                $("#Supplier_Detail_Hotel_Div").hide();
                $("#ContentPlaceHolder1_Supplier_Product_Status_Div").hide();
                $("#Supplier_Product_Detail_Div").hide();
                $("#Supplier_Detail_Galileo_Airline_Div").hide();
                $("#Supplier_Detail_API_Airline_Div").show();
                $("#Supplier_Detail_Div").hide();
                $("#AirlinePnrMakeDays_Div").hide();
                $("#Supplier_Detail_Lcc_Airline_Div").hide();

                $("#UAPI_FOP_Div").hide();
                $("#UAPI_CC_Div").hide();

            }
            if (stat == 6) {
                $("#Supplier_Detail_Hotel_Div").hide();
                $("#ContentPlaceHolder1_Supplier_Product_Status_Div").hide();
                $("#Supplier_Product_Detail_Div").hide();
                $("#Supplier_Detail_Galileo_Airline_Div").hide();
                $("#Supplier_Detail_API_Airline_Div").hide();
                $("#Supplier_Detail_Div").show();
                $("#AirlinePnrMakeDays_Div").hide();
                $("#Supplier_Detail_Lcc_Airline_Div").hide();
                $("#UAPI_FOP_Div").hide();
                $("#UAPI_CC_Div").hide();

            }
            if (stat == 7) {
                $("#Supplier_Detail_Hotel_Div").hide();
                $("#ContentPlaceHolder1_Supplier_Product_Status_Div").hide();
                $("#Supplier_Product_Detail_Div").hide();
                $("#Supplier_Detail_Galileo_Airline_Div").hide();
                $("#Supplier_Detail_API_Airline_Div").hide();
                $("#Supplier_Detail_Div").hide();
                $("#AirlinePnrMakeDays_Div").hide();
                $("#Supplier_Detail_Lcc_Airline_Div").show();

                $("#UAPI_FOP_Div").hide();
                $("#UAPI_CC_Div").hide();
            }
            if (stat == 8) {

                $("#Supplier_Detail_Hotel_Div").hide();
                $("#ContentPlaceHolder1_Supplier_Product_Status_Div").hide();
                $("#Supplier_Product_Detail_Div").hide();
                $("#Supplier_Detail_Galileo_Airline_Div").hide();
                $("#Supplier_Detail_API_Airline_Div").hide();
                $("#Supplier_Detail_Div").hide();
                $("#Supplier_Detail_Lcc_Airline_Div").hide();
                $("#AirlinePnrMakeDays_Div").show();

                $("#UAPI_FOP_Div").hide();
                $("#UAPI_CC_Div").hide();
            }
            if (stat == 9) {
                $("#Supplier_Detail_Hotel_Div").hide();
                $("#ContentPlaceHolder1_Supplier_Product_Status_Div").hide();
                $("#Supplier_Product_Detail_Div").hide();
                $("#Supplier_Detail_Galileo_Airline_Div").hide();
                $("#Supplier_Detail_API_Airline_Div").hide();
                $("#Supplier_Detail_Div").hide();
                $("#Supplier_Detail_Lcc_Airline_Div").hide();
                $("#AirlinePnrMakeDays_Div").hide();

                $("#UAPI_FOP_Div").show();
                $("#UAPI_CC_Div").hide();
            }
            if (stat == 10) {
                $("#Supplier_Detail_Hotel_Div").hide();
                $("#ContentPlaceHolder1_Supplier_Product_Status_Div").hide();
                $("#Supplier_Product_Detail_Div").hide();
                $("#Supplier_Detail_Galileo_Airline_Div").hide();
                $("#Supplier_Detail_API_Airline_Div").hide();
                $("#Supplier_Detail_Div").hide();
                $("#Supplier_Detail_Lcc_Airline_Div").hide();
                $("#AirlinePnrMakeDays_Div").hide();

                $("#UAPI_FOP_Div").hide();
                $("#UAPI_CC_Div").show();
            }
        }
        catch (e) { }
    }
    window.MyFunction = MyFunction;
    function onlydecimal(evt, element) {
        debugger;
        var charCode = (evt.which) ? evt.which : event.keyCode
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
    window.onlydecimal = onlydecimal;
    function onlyint(event) {
        var key = window.event ? event.keyCode : event.which;
        if (event.keyCode == 8 || event.keyCode == 46
            || event.keyCode == 37 || event.keyCode == 39) {
            return true;
        }
        else if (key < 48 || key > 57) {
            return false;
        }
        else return true;
    }
    window.onlyint = onlyint;
    function UpdateAPIDetail(aaa) {
        debugger;

    }

    window.UpdateAPIDetail = UpdateAPIDetail;
    function openCity(evt, cityName) {
        var i, tabcontent, tablinks;
        tabcontent = document.getElementsByClassName("tabcontent");
        for (i = 0; i < tabcontent.length; i++) {
            tabcontent[i].style.display = "none";
        }
        tablinks = document.getElementsByClassName("tablinks");
        for (i = 0; i < tablinks.length; i++) {
            tablinks[i].className = tablinks[i].className.replace(" active", "");
        }
        document.getElementById(cityName).style.display = "block";
        evt.currentTarget.className += " active";
    }

    function openCity(evt, cityName) {
        var i, tabcontent, tablinks;
        tabcontent = document.getElementsByClassName("tabcontent");
        for (i = 0; i < tabcontent.length; i++) {
            tabcontent[i].style.display = "none";
        }
        tablinks = document.getElementsByClassName("tablinks");
        for (i = 0; i < tablinks.length; i++) {
            tablinks[i].className = tablinks[i].className.replace(" active", "");
        }
        document.getElementById(cityName).style.display = "block";
        evt.currentTarget.className += " active";
    }

    window.openCity = openCity;
}