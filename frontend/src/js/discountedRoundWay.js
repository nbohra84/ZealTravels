var min;
var max;
function GetMinMax() {
    var msg = sortF;
    max = 0;
    $(msg.d).each(function (index, element) {

        var TotalAmount = parseFloat(msg.d[index].TotalAmount); //Rs.${parseInt(Price_sngl_EP)+parseInt(OutBoundTotalFare)+parseInt(InBoundTotalFare)}

        if (TotalAmount > parseFloat(max)) {

            max = parseFloat(msg.d[index].TotalAmount);
        }
    });
    min = max;
    $(msg.d).each(function (index, element) {
        var TotalAmount = parseFloat(msg.d[index].TotalAmount); //Rs.${parseInt(Price_sngl_EP)+parseInt(OutBoundTotalFare)+parseInt(InBoundTotalFare)}
        if (TotalAmount < parseFloat(min)) {
            min = parseFloat(msg.d[index].TotalAmount);
        }
    });

    $("#slider-range").slider({
        range: true,
        min: min,
        max: max,
        values: [min, max],
        slide: function (event, ui) {
            $("#modelpopupOUTER").css("display", "Block");
            $("#modelpopup").css("background-color", "Gray");
            $('#modelpopup').delay(1).fadeIn(400);
            $('#modelpopupOUTER').delay(1).fadeIn(400);
            //  $("#amount").val("Rs." + ui.values[0] + " - Rs." + ui.values[1]);
            $("#amount").val($("#hdncurrencytype").val()+" " + ui.values[0] + " - "+$("#hdncurrencytype").val()+" " + ui.values[1]);
            msg = sortF;
            jQuery('#tempView').html('');
            for (var k = 0; k < flagPreferAirlines.d.length; k++) {
                var asd = "span" + flagPreferAirlines.d[k].Airlines;
                document.getElementById(asd).style.display = 'none';
            }
            var i = 0;
            var j = msg.d.length;
            for (i; i < j; i++) {
                var TotalAmount = parseFloat(msg.d[i].TotalAmount);
                if (TotalAmount >= ui.values[0] && TotalAmount <= ui.values[1]) {
                    $("#MyTemplate").tmpl(msg.d[i]).appendTo("#tempView");
                }
            }
            $('#modelpopup').delay(1000).fadeOut(400);
            $('#modelpopupOUTER').delay(1000).fadeOut(400);
        }
    });
    $("#amount").val($("#hdncurrencytype").val() + " " + min + " - "+$("#hdncurrencytype").val()+" " + max);
}
var minDepartureTime;
var maxDepartureTime;
var minTimed;
var maxTimed;
function GetMinMaxDepartureTime() {
    var msg = sortF;
    maxDepartureTime = 0;
    $(msg.d).each(function (index, element) {
        var FlightDepTime = parseFloat(msg.d[index].FlightDepTime.substring(0, 2) + (msg.d[index].FlightDepTime.substring(3, 5)));
        if (FlightDepTime > parseFloat(maxDepartureTime)) {
            maxDepartureTime = parseFloat(msg.d[index].FlightDepTime.substring(0, 2) + (msg.d[index].FlightDepTime.substring(3, 5)));
            maxTimed = msg.d[index].FlightDepTime;
        }
    });

    minDepartureTime = maxDepartureTime;
    $(msg.d).each(function (index, element) {
        var FlightDepTime = parseFloat(msg.d[index].FlightDepTime.substring(0, 2) + (msg.d[index].FlightDepTime.substring(3, 5)));
        if (FlightDepTime < parseFloat(minDepartureTime)) {
            minDepartureTime = parseFloat(msg.d[index].FlightDepTime.substring(0, 2) + (msg.d[index].FlightDepTime.substring(3, 5)));
            minTimed = msg.d[index].FlightDepTime;
        }
    });
    $("#DepartureTimes-slider-range").slider({
        range: true,
        min: minDepartureTime,
        max: maxDepartureTime,
        values: [minDepartureTime, maxDepartureTime],
        slide: function (event, ui) {
            $("#modelpopupOUTER").css("display", "Block");
            $("#modelpopup").css("background-color", "Gray");
            $('#modelpopup').delay(1).fadeIn(400);
            $('#modelpopupOUTER').delay(1).fadeIn(400);
            // $("#amountDepartureTimes").val("Time:" + ui.values[0] + " - Time:" + ui.values[1]);
            msg = sortF;
            jQuery('#tempView').html('');
            for (var k = 0; k < flagPreferAirlines.d.length; k++) {
                var asd = "span" + flagPreferAirlines.d[k].Airlines;
                document.getElementById(asd).style.display = 'none';
            }
            var i = 0;
            var j = msg.d.length;
            for (i; i < j; i++) {
                var FlightDepTime = parseFloat(msg.d[i].FlightDepTime.substring(0, 2) + msg.d[i].FlightDepTime.substring(3, 5));
                if (FlightDepTime >= ui.values[0] && FlightDepTime <= ui.values[1]) {
                    $("#MyTemplate").tmpl(msg.d[i]).appendTo("#tempView");
                }
            }
            $('#modelpopup').delay(1000).fadeOut(400);
            $('#modelpopupOUTER').delay(1000).fadeOut(400);
        }
    });
    $("#amountDepartureTimes").val("" + minTimed + " To " + maxTimed);
}

var minArrivalTime;
var maxArrivalTime;
var minTimedArrival;
var maxTimedArrival;
function GetMinMaxArrivalTime() {
    var msg = sortF;
    maxArrivalTime = 0;
    $(msg.d).each(function (index, element) {
        var FlightArrTime = parseFloat(msg.d[index].FlightArrTime.substring(0, 2) + (msg.d[index].FlightArrTime.substring(3, 5)));
        if (FlightArrTime > parseFloat(maxArrivalTime)) {
            maxArrivalTime = parseFloat(msg.d[index].FlightArrTime.substring(0, 2) + (msg.d[index].FlightArrTime.substring(3, 5)));
            maxTimedArrival = msg.d[index].FlightArrTime;
        }
    });

    minArrivalTime = maxArrivalTime;
    $(msg.d).each(function (index, element) {
        var FlightArrTime = parseFloat(msg.d[index].FlightArrTime.substring(0, 2) + (msg.d[index].FlightArrTime.substring(3, 5)));
        if (FlightArrTime < parseFloat(minArrivalTime)) {
            minArrivalTime = parseFloat(msg.d[index].FlightArrTime.substring(0, 2) + (msg.d[index].FlightArrTime.substring(3, 5)));
            minTimedArrival = msg.d[index].FlightArrTime;
        }
    });
    $("#ArrivalTimes-slider-range").slider({
        range: true,
        min: minArrivalTime,
        max: maxArrivalTime,
        values: [minArrivalTime, maxArrivalTime],
        slide: function (event, ui) {
            $("#modelpopupOUTER").css("display", "Block");
            $("#modelpopup").css("background-color", "Gray");
            $('#modelpopup').delay(1).fadeIn(400);
            $('#modelpopupOUTER').delay(1).fadeIn(400);
            // $("#amountDepartureTimes").val("Time:" + ui.values[0] + " - Time:" + ui.values[1]);
            msg = sortF;
            jQuery('#tempView').html('');
            for (var k = 0; k < flagPreferAirlines.d.length; k++) {
                var asd = "span" + flagPreferAirlines.d[k].Airlines;
                document.getElementById(asd).style.display = 'none';
            }
            var i = 0;
            var j = msg.d.length;
            for (i; i < j; i++) {
                var FlightArrTime = parseFloat(msg.d[i].FlightArrTime.substring(0, 2) + msg.d[i].FlightArrTime.substring(3, 5));
                if (FlightArrTime >= ui.values[0] && FlightArrTime <= ui.values[1]) {
                    $("#MyTemplate").tmpl(msg.d[i]).appendTo("#tempView");
                }
            }

            $('#modelpopup').delay(1000).fadeOut(400);
            $('#modelpopupOUTER').delay(1000).fadeOut(400);
        }
    });
    $("#amountArrivalTimes").val(" " + minTimedArrival + "  To " + maxTimedArrival);
}
var CurrancyDataMsg;
 function GetCurrancyData() {
        $("#FlightPopUp").css("display", "block");
        $.ajax({
            type: "POST",
            url: "k_one.aspx/CurrancyData",
            data: "{}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (msg) {
                CurrancyDataMsg = msg;
                var sup = '<%=Session["Country"]%>'.toString();
                for (var i = 0; i < msg.d.length; i++) {
                    if (msg.d[i].CountryCode == sup) {
                        $("#currency").append("<option selected='selected'  value=" + msg.d[i].CODE + ">" + msg.d[i].COUNTRY_WITHCODE + "</option>");
                    }
                    else {
                        $("#currency").append("<option value=" + msg.d[i].CODE + ">" + msg.d[i].COUNTRY_WITHCODE + "</option>");
                    }
                }
            },
            error: function (msg) {
            }
        });
    }
    $(document).ready(function () {
        $("body").tooltip({
            selector: "[data-toggle='tooltip']",
            container: "body"
        });

        $('#top').hide();
      //  GetCurrancyData();
        GetShow();
        showprogerss();


        $('#top').click(function () {
            $('#top').fadeOut();
            window.scrollTo(0, 0);
        });
        $('#SpanModify').click(function () {
            $("#FlightPopUp").css("display", "none");
            document.getElementById("modifypart").style.display = "block";
            $('#ModifySearchDomNew').show('slow');

            $("#datashowConcernModifyouter").css("display", "block");
            $("#datashowConcernModifyouter").css("background-color", "Gray");
            window.scrollTo(0, 0);

        });
        $('#ModifySearchDomNewClose').click(function () {
            $('#ModifySearchDomNew').hide('slow');
            document.getElementById("modifypart").style.display = "none";
            $("#datashowConcernModifyouter").css("display", "none");
            $("#datashowConcernModifyouter").css("background-color", "transparent");
        });
        //flight booking

        $('#flightBook').click(function () {
            $("#FlightPopUp").css("display", "block");
            $.ajax({
                type: "POST",
                url: "k_one.aspx/Book",
                data: "{}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (msg) {
                    if (msg.d[0].msgs == 1) {
                        var url = "GuestAndExistingLogin.aspx?AgentType=Guest&tabid=" + msg.d[0].tabid;
                        $(location).attr('href', url);
                    }
                    else if (msg.d[0].msgs == 2) {
                        var url = "FlightReview.aspx?tabid=" + msg.d[0].tabid;
                        $(location).attr('href', url);
                    }
                    else {
                        var url = "index.aspx";
                        $(location).attr('href', url);
                    }
                },
                error: function (msg) {
                    var url = "index.aspx";
                    $(location).attr('href', url);
                }
            });
        });
        $(window).scroll(function () {
            if ($(this).scrollTop() > 500) {
                $('#top').fadeIn();
            }
            else {
                $('#top').fadeOut();
            }
        });
        $('#Morning').click(function () {
            ShowMorningFlight();
        });
        $('#AfterNoon').click(function () {
            ShowAfterNoonFlight();
        });
        $('#Night').click(function () {
            ShowNightFlight();
        });
        $('#MIDNight').click(function () {
            ShowMIDNightFlight();
        });
        $('.enablec').click(function () {

            $("#datashowConcernModifyouter").css("display", "Block");
            $("#FlightPopUp").css("display", "block");
            //$("#modelpopupCalender").css("background-color", "Gray");
            //$('#modelpopupCalender').delay(1).fadeIn(400);
            //$('#modelpopupOUTERCalender').delay(1).fadeIn(400);
        });

        var data = document.getElementsByClassName("item");
        // var $item = document.getElementsByClassName("item"), //Cache your DOM selector
        var $item = $('li.item'), //Cache your DOM selector
          visible = 10, //Set the number of items that will be visible
         index = 0, //Starting index
         //endIndex = ($item.length / visible) - 1; //End index
         //endIndex = ($item.length - visible)
          endIndex = ($item.length);
        var indexto = 0;
        var minusleft = 300;
        var plusleft = 0;
        $('div#arrowR').click(function () {

            $item = $('li.item'), //Cache your DOM selector
              visible = 10, //Set the number of items that will be visible
             index = 0, //Starting index
            //endIndex = ($item.length / visible) - 1; //End index
            //endIndex = ($item.length - visible)
               endIndex = ($item.length);
            if (index < endIndex) {
                if (indexto < endIndex) {
                    // index++;
                    indexto++;
                    minusleft = minusleft + 100;
                    plusleft = plusleft - 100;
                    $item.animate({ 'left': '-=' + minusleft + 'px' });
                }
            }
        });

        $('div#arrowL').click(function () {

            $item = $('li.item'), //Cache your DOM selector
             visible = 10, //Set the number of items that will be visible
            index = 0, //Starting index
            //endIndex = ($item.length / visible) - 1; //End index
            // endIndex = ($item.length - visible)
            endIndex = ($item.length);
            if (indexto > 0) {

                // index--;
                indexto--;
                plusleft = plusleft + 100;
                minusleft = minusleft - 100;
                $item.animate({ 'left': '+=' + plusleft + 'px' });
            }
        });

    });
    function ShowMorningFlight() {
        $("#modelpopupOUTER").css("display", "Block");
        $("#modelpopup").css("background-color", "Gray");
        $('#modelpopup').delay(1).fadeIn(400);
        $('#modelpopupOUTER').delay(1).fadeIn(400);
        var msg = sortF;
        jQuery('#tempView').html('');
        for (var i = 0; i < msg.d.length; i++) {
            var FlightDepTime = parseFloat(msg.d[i].FlightDepTime.substring(0, 2) + (msg.d[i].FlightDepTime.substring(3, 5)));
            if (FlightDepTime >= 500 && FlightDepTime <= 1200) {
                $("#MyTemplate").tmpl(msg.d[i]).appendTo("#tempView");
                var FlightRefid = "spanAmount" + msg.d[i].FlightRefid;


            }
        }
        $('#modelpopup').delay(1000).fadeOut(400);
        $('#modelpopupOUTER').delay(1000).fadeOut(400);
        chkunchk();
    }
    function FareM(Airlinename, Fare) {
        $("#modelpopupOUTER").css("display", "Block");
        $("#modelpopup").css("background-color", "Gray");
        $('#modelpopup').delay(1).fadeIn(400);
        $('#modelpopupOUTER').delay(1).fadeIn(400);

        if (Fare != "---") {
            var msg = sortF;
            jQuery('#tempView').html('');
            for (var i = 0; i < msg.d.length; i++) {
                var FlightDepTime = parseFloat(msg.d[i].FlightDepTime.substring(0, 2) + (msg.d[i].FlightDepTime.substring(3, 5)));
                if (FlightDepTime >= 500 && FlightDepTime <= 1200 && msg.d[i].FlightName == Airlinename) {

                    $("#MyTemplate").tmpl(msg.d[i]).appendTo("#tempView");
                    var FlightRefid = "spanAmount" + msg.d[i].FlightRefid;

                }
            }
        }
        $('#modelpopup').delay(1000).fadeOut(400);
        $('#modelpopupOUTER').delay(1000).fadeOut(400);
        chkunchk();
    }
    function FareA(Airlinename, Fare) {
        $("#modelpopupOUTER").css("display", "Block");
        $("#modelpopup").css("background-color", "Gray");
        $('#modelpopup').delay(1).fadeIn(400);
        $('#modelpopupOUTER').delay(1).fadeIn(400);

        if (Fare != "---") {
            var msg = sortF;
            jQuery('#tempView').html('');
            for (var i = 0; i < msg.d.length; i++) {
                var FlightDepTime = parseFloat(msg.d[i].FlightDepTime.substring(0, 2) + (msg.d[i].FlightDepTime.substring(3, 5)));
                if (FlightDepTime >= 1200 && FlightDepTime <= 1800 && msg.d[i].FlightName == Airlinename) {
                    $("#MyTemplate").tmpl(msg.d[i]).appendTo("#tempView");
                    var FlightRefid = "spanAmount" + msg.d[i].FlightRefid;

                }
            }
        }
        $('#modelpopup').delay(1000).fadeOut(400);
        $('#modelpopupOUTER').delay(1000).fadeOut(400);
        chkunchk();
    }
    function FareN(Airlinename, Fare) {
        $("#modelpopupOUTER").css("display", "Block");
        $("#modelpopup").css("background-color", "Gray");
        $('#modelpopup').delay(1).fadeIn(400);
        $('#modelpopupOUTER').delay(1).fadeIn(400);

        if (Fare != "---") {
            var msg = sortF;
            jQuery('#tempView').html('');
            for (var i = 0; i < msg.d.length; i++) {
                var FlightDepTime = parseFloat(msg.d[i].FlightDepTime.substring(0, 2) + (msg.d[i].FlightDepTime.substring(3, 5)));
                if (FlightDepTime >= 1800 && FlightDepTime <= 2400 && msg.d[i].FlightName == Airlinename) {
                    $("#MyTemplate").tmpl(msg.d[i]).appendTo("#tempView");
                    var FlightRefid = "spanAmount" + msg.d[i].FlightRefid;

                }
            }
        }
        $('#modelpopup').delay(1000).fadeOut(400);
        $('#modelpopupOUTER').delay(1000).fadeOut(400);
        chkunchk();
    }
    function FareMN(Airlinename, Fare) {
        $("#modelpopupOUTER").css("display", "Block");
        $("#modelpopup").css("background-color", "Gray");
        $('#modelpopup').delay(1).fadeIn(400);
        $('#modelpopupOUTER').delay(1).fadeIn(400);

        if (Fare != "---") {
            var msg = sortF;
            jQuery('#tempView').html('');
            for (var i = 0; i < msg.d.length; i++) {
                var FlightDepTime = parseFloat(msg.d[i].FlightDepTime.substring(0, 2) + (msg.d[i].FlightDepTime.substring(3, 5)));
                if (FlightDepTime >= 0 && FlightDepTime <= 500 && msg.d[i].FlightName == Airlinename) {
                    $("#MyTemplate").tmpl(msg.d[i]).appendTo("#tempView");
                    var FlightRefid = "spanAmount" + msg.d[i].FlightRefid;


                }
            }
        }
        $('#modelpopup').delay(1000).fadeOut(400);
        $('#modelpopupOUTER').delay(1000).fadeOut(400);
        chkunchk();
    }
    function ShowAfterNoonFlight() {
        $("#modelpopupOUTER").css("display", "Block");
        $("#modelpopup").css("background-color", "Gray");
        $('#modelpopup').delay(1).fadeIn(400);
        $('#modelpopupOUTER').delay(1).fadeIn(400);
        var msg = sortF;
        jQuery('#tempView').html('');
        for (var i = 0; i < msg.d.length; i++) {
            var FlightDepTime = parseFloat(msg.d[i].FlightDepTime.substring(0, 2) + (msg.d[i].FlightDepTime.substring(3, 5)));
            if (FlightDepTime >= 1200 && FlightDepTime <= 1800) {
                $("#MyTemplate").tmpl(msg.d[i]).appendTo("#tempView");
                var FlightRefid = "spanAmount" + msg.d[i].FlightRefid;

            }
        }
        $('#modelpopup').delay(1000).fadeOut(400);
        $('#modelpopupOUTER').delay(1000).fadeOut(400);
        chkunchk();
    }
    function ShowNightFlight() {
        $("#modelpopupOUTER").css("display", "Block");
        $("#modelpopup").css("background-color", "Gray");
        $('#modelpopup').delay(1).fadeIn(400);
        $('#modelpopupOUTER').delay(1).fadeIn(400);
        var msg = sortF;
        jQuery('#tempView').html('');
        for (var i = 0; i < msg.d.length; i++) {
            var FlightDepTime = parseFloat(msg.d[i].FlightDepTime.substring(0, 2) + (msg.d[i].FlightDepTime.substring(3, 5)));
            if (FlightDepTime >= 1800 && FlightDepTime <= 2400) {
                $("#MyTemplate").tmpl(msg.d[i]).appendTo("#tempView");
                var FlightRefid = "spanAmount" + msg.d[i].FlightRefid;

            }
        }
        $('#modelpopup').delay(1000).fadeOut(400);
        $('#modelpopupOUTER').delay(1000).fadeOut(400);
        chkunchk();
    }
    function ShowMIDNightFlight() {
        $("#modelpopupOUTER").css("display", "Block");
        $("#modelpopup").css("background-color", "Gray");
        $('#modelpopup').delay(1).fadeIn(400);
        $('#modelpopupOUTER').delay(1).fadeIn(400);
        var msg = sortF;
        jQuery('#tempView').html('');
        for (var i = 0; i < msg.d.length; i++) {
            var FlightDepTime = parseFloat(msg.d[i].FlightDepTime.substring(0, 2) + (msg.d[i].FlightDepTime.substring(3, 5)));
            if (FlightDepTime >= 0 && FlightDepTime <= 500) {
                $("#MyTemplate").tmpl(msg.d[i]).appendTo("#tempView");
                var FlightRefid = "spanAmount" + msg.d[i].FlightRefid;

            }
        }

        $('#modelpopup').delay(1000).fadeOut(400);
        $('#modelpopupOUTER').delay(1000).fadeOut(400);
        chkunchk();
    }

    var sortF;
    var sortFCurrency;
    var MatrixData;
    function GetShow() {


        $("#waitingload").css("display", "Block");
        $("#waitingloadbox").css("display", "Block");

        var CompnyID = $("#hdncmpid").val();
        $.ajax({
            type: "POST",
            url: "/flight/ShowDataRoundInternational",
            data: '{CompanyID:"' + CompnyID + '"}',
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (msg) {
                if (msg.d.length === 0) {
                    $("#flightNotFound").show();
                    $("#intresultmaindiv").hide();
                    $("#waitingload").css("display", "none");
                    $("#waitingloadbox").css("display", "none");
                  } else {
                sortF = null;
                sortF = msg
                sortFCurrency = msg;
                jQuery('#tempView').html('');

                for (var i = 0; i < msg.d.length; i++) {


                    $("#MyTemplate").tmpl(msg.d[i]).appendTo("#tempView");

                }
                GetMinMax();
                GetMinMaxDepartureTime();
                GetMinMaxArrivalTime();
                GetFlightPreferAirlines();
                GetFlightStops();
                GetFlightMatrix();
                document.getElementById("fare_type").style.border = "0px solid #ff0000";
                $("#fare_type").animate({ borderWidth: "0px" }, 500);
                $("#fare_type").animate({ borderWidth: "0" }, 0);
            }
            },
            error: function (msg) {
            }
        });
    }

    var flagOnwardPrice = 1;
    function OnwardPrice() {

        for (var j = 0; j < flagPreferAirlines.d.length; j++) {
            var asd = "span" + flagPreferAirlines.d[j].Airlines;
            document.getElementById(asd).style.display = 'none';
        }
        $('#AirlineSort').removeClass('downArrow');
        $('#AirlineSort').removeClass('upArrow');
        $('#PriceSort').removeClass('downArrow');
        $('#PriceSort').removeClass('upArrow');
        $('#DepartSort').removeClass('downArrow');
        $('#DepartSort').removeClass('upArrow');
        $('#ArriveSort').removeClass('downArrow');
        $('#ArriveSort').removeClass('upArrow');
        $('#StopSort').removeClass('downArrow');
        $('#StopSort').removeClass('upArrow');
        ArriveSort
        jQuery('#tempView').html('');
        if (flagOnwardPrice == 0) {
            flagOnwardPrice = 1;
            $('#PriceSort').addClass('downArrow');
            var msg = sortF;
            msg.d.sort(function (obj1, obj2) {

                return parseFloat(obj1.TotalAmount) < parseFloat(obj2.TotalAmount) ? -1 :
                               (parseFloat(obj1.TotalAmount) > parseFloat(obj2.TotalAmount) ? 1 : 0);
            });

        }
        else {
            flagOnwardPrice = 0;
            $('#PriceSort').addClass('upArrow');
            var msg = sortF;

            msg.d.sort(function (obj1, obj2) {

                return parseFloat(obj1.TotalAmount) > parseFloat(obj2.TotalAmount) ? -1 :
                               (parseFloat(obj1.TotalAmount) < parseFloat(obj2.TotalAmount) ? 1 : 0);
            });

        }

        for (var i = 0; i < msg.d.length; i++) {
            $("#MyTemplate").tmpl(msg.d[i]).appendTo("#tempView");
            var FlightRefid = "spanAmount" + msg.d[i].FlightRefid;

        }
    }
    window.OnwardPrice = OnwardPrice;

    var flag = 0;
    function OnwardAirline() {

        for (var j = 0; j < flagPreferAirlines.d.length; j++) {
            var asd = "span" + flagPreferAirlines.d[j].Airlines;
            document.getElementById(asd).style.display = 'none';
        }
        $('#AirlineSort').removeClass('downArrow');
        $('#AirlineSort').removeClass('upArrow');
        $('#PriceSort').removeClass('downArrow');
        $('#PriceSort').removeClass('upArrow');
        $('#DepartSort').removeClass('downArrow');
        $('#DepartSort').removeClass('upArrow');
        $('#ArriveSort').removeClass('downArrow');
        $('#ArriveSort').removeClass('upArrow');
        $('#StopSort').removeClass('downArrow');
        $('#StopSort').removeClass('upArrow');
        jQuery('#tempView').html('');
        if (flag == 0) {
            flag = 1;
            $('#AirlineSort').addClass('downArrow');
            var msg = sortF;
            msg.d.sort(function (obj1, obj2) {

                return obj1.FlightName < obj2.FlightName ? -1 :
                   (obj1.FlightName > obj2.FlightName ? 1 : 0);
            });
        }
        else {
            flag = 0;
            $('#AirlineSort').addClass('upArrow');
            var msg = sortF;

            msg.d.sort(function (obj1, obj2) {

                return obj1.FlightName > obj2.FlightName ? -1 :
                   (obj1.FlightName < obj2.FlightName ? 1 : 0);
            });
        }
        for (var i = 0; i < msg.d.length; i++) {
            $("#MyTemplate").tmpl(msg.d[i]).appendTo("#tempView");
            var FlightRefid = "spanAmount" + msg.d[i].FlightRefid;

            var amount = parseFloat(msg.d[i].TotalAmount) * parseFloat(currencyAmount);

        }

    }
    window.OnwardAirline = OnwardAirline;

    var flagDepart = 0;
    function OnwardDepart() {

        for (var j = 0; j < flagPreferAirlines.d.length; j++) {
            var asd = "span" + flagPreferAirlines.d[j].Airlines;
            document.getElementById(asd).style.display = 'none';
        }
        $('#AirlineSort').removeClass('downArrow');
        $('#AirlineSort').removeClass('upArrow');
        $('#PriceSort').removeClass('downArrow');
        $('#PriceSort').removeClass('upArrow');
        $('#DepartSort').removeClass('downArrow');
        $('#DepartSort').removeClass('upArrow');
        $('#ArriveSort').removeClass('downArrow');
        $('#ArriveSort').removeClass('upArrow');
        $('#StopSort').removeClass('downArrow');
        $('#StopSort').removeClass('upArrow');
        jQuery('#tempView').html('');
        if (flagDepart == 0) {
            flagDepart = 1;
            $('#DepartSort').addClass('downArrow');
            var msg = sortF;
            msg.d.sort(function (obj1, obj2) {

                return obj1.FlightDepTime < obj2.FlightDepTime ? -1 :
                   (obj1.FlightDepTime > obj2.FlightDepTime ? 1 : 0);
            });
        }
        else {
            flagDepart = 0;
            $('#DepartSort').addClass('upArrow');
            var msg = sortF;
            msg.d.sort(function (obj1, obj2) {

                return obj1.FlightDepTime > obj2.FlightDepTime ? -1 :
                   (obj1.FlightDepTime < obj2.FlightDepTime ? 1 : 0);
            });
        }
        for (var i = 0; i < msg.d.length; i++) {
            $("#MyTemplate").tmpl(msg.d[i]).appendTo("#tempView");
            var FlightRefid = "spanAmount" + msg.d[i].FlightRefid;

        }


    }
    window.OnwardDepart = OnwardDepart;
    var flagArrive = 0;
    function OnwardArrive() {

        for (var j = 0; j < flagPreferAirlines.d.length; j++) {
            var asd = "span" + flagPreferAirlines.d[j].Airlines;
            document.getElementById(asd).style.display = 'none';
        }
        $('#AirlineSort').removeClass('downArrow');
        $('#AirlineSort').removeClass('upArrow');
        $('#PriceSort').removeClass('downArrow');
        $('#PriceSort').removeClass('upArrow');
        $('#DepartSort').removeClass('downArrow');
        $('#DepartSort').removeClass('upArrow');
        $('#ArriveSort').removeClass('downArrow');
        $('#ArriveSort').removeClass('upArrow');
        $('#StopSort').removeClass('downArrow');
        $('#StopSort').removeClass('upArrow');
        jQuery('#tempView').html('');
        if (flagArrive == 0) {
            flagArrive = 1;
            $('#ArriveSort').addClass('downArrow');
            var msg = sortF;
            msg.d.sort(function (obj1, obj2) {

                return obj1.FlightArrTime < obj2.FlightArrTime ? -1 :
                   (obj1.FlightArrTime > obj2.FlightArrTime ? 1 : 0);
            });
        }
        else {
            flagArrive = 0;
            $('#ArriveSort').addClass('upArrow');
            var msg = sortF;
            msg.d.sort(function (obj1, obj2) {

                return obj1.FlightArrTime > obj2.FlightArrTime ? -1 :
                   (obj1.FlightArrTime < obj2.FlightArrTime ? 1 : 0);
            });

        }
        for (var i = 0; i < msg.d.length; i++) {
            $("#MyTemplate").tmpl(msg.d[i]).appendTo("#tempView");
            var FlightRefid = "spanAmount" + msg.d[i].FlightRefid;

        }
    }
    window.OnwardArrive = OnwardArrive;

    var flagStop = 0;
    function OnwardStop() {

        for (var j = 0; j < flagPreferAirlines.d.length; j++) {
            var asd = "span" + flagPreferAirlines.d[j].Airlines;
            document.getElementById(asd).style.display = 'none';
        }
        $('#AirlineSort').removeClass('downArrow');
        $('#AirlineSort').removeClass('upArrow');
        $('#PriceSort').removeClass('downArrow');
        $('#PriceSort').removeClass('upArrow');
        $('#DepartSort').removeClass('downArrow');
        $('#DepartSort').removeClass('upArrow');
        $('#ArriveSort').removeClass('downArrow');
        $('#ArriveSort').removeClass('upArrow');
        $('#StopSort').removeClass('downArrow');
        $('#StopSort').removeClass('upArrow');
        jQuery('#tempView').html('');
        if (flagStop == 0) {
            flagStop = 1;
            $('#StopSort').addClass('downArrow');
            var msg = sortF;
            msg.d.sort(function (obj1, obj2) {

                return obj1.connection < obj2.connection ? -1 :
                   (obj1.connection > obj2.connection ? 1 : 0);
            });
        }
        else {
            flagStop = 0;
            $('#StopSort').addClass('upArrow');
            var msg = sortF;
            msg.d.sort(function (obj1, obj2) {

                return obj1.connection > obj2.connection ? -1 :
                   (obj1.connection < obj2.connection ? 1 : 0);
            });

        }
        for (var i = 0; i < msg.d.length; i++) {
            $("#MyTemplate").tmpl(msg.d[i]).appendTo("#tempView");
            var FlightRefid = "spanAmount" + msg.d[i].FlightRefid;

        }
    }
    window.OnwardStop = OnwardStop;

    function showprogerss() {
        //$('#progressbar').css("display", "block");
        //$('#progressbar').progressbar({
        //    warningMarker: 50,
        //    dangerMarker: 75,
        //    maximum: 100,
        //    step: 5
        //});
        //var self = this;
        //this.interval = setInterval(function () {
        //    $('#progressbar').progressbar('stepIt');
        //}, 250);
    }
    var flagPreferAirlines;
    function GetFlightPreferAirlines() {
        $.ajax({
            type: "POST",
            url: "/flight/PreferAirlines",
            data: "{}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (msg) {
                flagPreferAirlines = msg;
                jQuery('#PreferredAirlines').html('');
                jQuery('#AirlinesChk').html('');
                $("#templeteAirlinesOnward").tmpl(msg.d).appendTo("#AirlinesChk");
                $("#templetePreferredAirlinesOnward").tmpl(msg.d).appendTo("#PreferredAirlines");
            },
            error: function (msg) {

            }

        });
    };
    function getCheckedAIRCheckOnward(Airline, spanAirline) {
        $("#modelpopupOUTER").css("display", "Block");
        $("#modelpopup").css("background-color", "Gray");
        $('#modelpopup').delay(1).fadeIn(400);
        $('#modelpopupOUTER').delay(1).fadeIn(400);
        for (var j = 0; j < flagPreferAirlines.d.length; j++) {
            var asd = "span" + flagPreferAirlines.d[j].Airlines;
            document.getElementById(asd).style.display = 'none';
        }
        document.getElementById(spanAirline).style.display = 'inline';
        jQuery('#tempView').html('');
        var msg = sortF;
        for (var i = 0; i < msg.d.length; i++) {
            if (msg.d[i].FlightName == Airline) {
                $("#MyTemplate").tmpl(msg.d[i]).appendTo("#tempView");
                var FlightRefid = "spanAmount" + msg.d[i].FlightRefid;

            }
        }
        $('#modelpopup').delay(1000).fadeOut(400);
        $('#modelpopupOUTER').delay(1000).fadeOut(400);
    }
    window.getCheckedAIRCheckOnward=getCheckedAIRCheckOnward;
    function getAll() {
        $("#modelpopupOUTER").css("display", "Block");
        $("#modelpopup").css("background-color", "Gray");
        $('#modelpopup').delay(1).fadeIn(400);
        $('#modelpopupOUTER').delay(1).fadeIn(400);

        for (var j = 0; j < flagPreferAirlines.d.length; j++) {
            var asd = "span" + flagPreferAirlines.d[j].Airlines;
            document.getElementById(asd).style.display = 'none';
        }
        jQuery('#tempView').html('');
        var msg = sortF;
        for (var i = 0; i < msg.d.length; i++) {
            $("#MyTemplate").tmpl(msg.d[i]).appendTo("#tempView");

            var FlightRefid = "spanAmount" + msg.d[i].FlightRefid;

        }
        $('#modelpopup').delay(1000).fadeOut(400);
        $('#modelpopupOUTER').delay(1000).fadeOut(400);
    }
    window.getAll =getAll;
    function BookNow(refid) {
        $("#FlightPopUp").css("display", "Block");
        $("#datashowConcernModifyouter").css("display", "block");
        document.getElementById("Hidden1email").value = refid;
        jQuery('#divSelectFlight').html('');
        jQuery('#divSelectFlightmotfound').html('');
        $("#book_now_button" + refid).removeClass("book_now_button");
        $("#book_now_button" + refid).addClass("book_now_button")
        $("#datashowConcernModifyouter").css("display", "Block");
        $("#FlightPopUp").css("display", "Block");
        var CompnyID = $("#hdncmpid").val();
        
        $.ajax({
            type: "POST",
            url: "/flight/SelectFlightIntR",
            data: JSON.stringify({
                refid: refid.toString(),
                CompanyID: CompnyID
            }),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (msg) {
                
                //GetShow();
                // $("#<%=Panel1.ClientID%>").show();
                $('#AirlineSort').removeClass('downArrow');
                $('#AirlineSort').removeClass('upArrow');
                $('#PriceSort').removeClass('downArrow');
                $('#PriceSort').removeClass('upArrow');
                $('#DepartSort').removeClass('downArrow');
                $('#DepartSort').removeClass('upArrow');
                $('#ArriveSort').removeClass('downArrow');
                $('#ArriveSort').removeClass('upArrow');
                $('#StopSort').removeClass('downArrow');
                $('#StopSort').removeClass('upArrow');
                $('#PriceSort').addClass('downArrow');
                if (msg.d[0].F_Status === true) {
                    $("#FlightPopUp").css("display", "none");
                    $("#divpopup").css("display", "Block");
                    $("#TemplatedivSelectFlightSPL").tmpl(msg.d[0]).appendTo("#divSelectFlight");
                    $("#TemplatedivSelectFlightSPLR").tmpl(msg.d[0]).appendTo("#divSelectFlight");
                    $("#TemplatedivSelectFlightSPLRFare").tmpl(msg.d[0]).appendTo("#divSelectFlight");
                }
                else {
                    $("#FlightPopUp").css("display", "none");
                    $("#divpopup").css("display", "Block");
                    $("#TemplatedivSelectFlightNotfound").tmpl(msg.d[0]).appendTo("#divSelectFlightmotfound");

                }


                //if (msg.d[0].FareUpdateMsgChek == 1) {
                //    $("#FlightPopUp").css("display", "none");
                //    $("#divpopup").css("display", "Block");
                //    $("#TemplatedivSelectFlightSPL").tmpl(msg.d[0]).appendTo("#divSelectFlight");
                //    $("#TemplatedivSelectFlightSPLR").tmpl(msg.d[0]).appendTo("#divSelectFlight");
                //    $("#TemplatedivSelectFlightSPLRFare").tmpl(msg.d[0]).appendTo("#divSelectFlight");
                //}
                //else if (msg.d[0].FareUpdateMsgChek == 0) {
                //    $("#FlightPopUp").css("display", "none");
                //    $("#divpopup").css("display", "Block");
                //    $("#TemplatedivSelectFlightSPL").tmpl(msg.d[0]).appendTo("#divSelectFlight");
                //    $("#TemplatedivSelectFlightSPLR").tmpl(msg.d[0]).appendTo("#divSelectFlight");
                //    $("#TemplatedivSelectFlightSPLRFare").tmpl(msg.d[0]).appendTo("#divSelectFlight");
                //}
                // <%--else if (msg.d[0].FareUpdateMsgChek == 2) {
                //     $("#FlightPopUp").css("display", "none");
                //     $("#divpopup").css("display", "Block");
                //     $("#TemplatedivSelectFlightNotfound").tmpl(msg.d[0]).appendTo("#divSelectFlightmotfound");
                //     $("#<%=Panel1.ClientID%>").hide();
                // }--%>
            },
            error: function (msg) {
                $("#FlightPopUp").css("display", "none");
                $("#datashowConcernModifyouter").css("display", "none");
                $("#book_now_button" + refid).removeClass("book_now_buttonAdd");
                $("#book_now_button" + refid).addClass("book_now_button");
            }
        });
}
window.BookNow = BookNow;
//function YesToBook() {
//    $.ajax({
//        type: "POST",
//        url: "k_one.aspx/Book",
//        data: "{}",
//        contentType: "application/json; charset=utf-8",
//        dataType: "json",
//        success: function (msg) {
//            if (msg.d[0].msgs === 1) {
//                var url = "GuestAndExistingLogin.aspx?AgentType=Guest&tabid=" + msg.d[0].tabid + "&CUR=INR";
//                $(location).attr('href', url);
//            }
//            else if (msg.d[0].msgs === 2) {
//                var url = "FlightReviewInternational.aspx?tabid=" + msg.d[0].tabid + "&CUR=INR";
//                $(location).attr('href', url);
//            }
//            else {
//                var url = "index.aspx";
//                $(location).attr('href', url);
//            }
//        },
//        error: function (msg) {
//            var url = "index.aspx";
//            $(location).attr('href', url);
//        }
//    });
//}
function PromoCheck(refid) {
    $("#Promo" + refid + "").css("disabled", "disabled");
    $("#Questionmark" + refid + "").css("display", "none");
    $("#checkmark" + refid + "").css("display", "none");
    $("#Close" + refid + "").css("display", "none");
    $("#datashowConcernModifyouter").css("display", "Block");
    $("#FlightPopUp").css("display", "Block");
    $.ajax({
        type: "POST",
        url: "/flight/SelectFlightPromoCheck",
        data: '{refid:"' + refid + '"}',
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (msg) {
            $("#FlightPopUp").css("display", "none");
            $("#datashowConcernModifyouter").css("display", "none");

            if (msg.d[0].msgs == "2") {

                GetShow();
                $("#checkmark" + refid + "").css("display", "inline");
            }
            else if (msg.d[0].msgs == "1") {
                $("#checkmark" + refid + "").css("display", "inline");
            }

            else {
                $("#Close" + refid + "").css("display", "inline");
            }

        },
        error: function (msg) {
            $("#FlightPopUp").css("display", "none");
            $("#datashowConcernModifyouter").css("display", "none");
            $("#Questionmark" + refid + "").css("display", "inline");
        }

    });
}
function Close(FlightRefid) {
    $("#divshow" + FlightRefid + "").hide('slow');
}
function SendSMSOpen(refid) {
    $("#FlightPopUp").css("display", "none");
    $("#datashowConcernModifyouter").css("display", "Block");
    $("#FlightPopUpEmail").css("display", "Block");
    jQuery('#FlightInfoEmail').html('');
    jQuery('#FlightInfoSMS').html('');
    var msg = sortF;
    for (var i = 0; i < msg.d.length; i++) {
        if (msg.d[i].FlightRefid == refid) {
            $("#TemplateFlightInfoSMS").tmpl(msg.d[i]).appendTo("#FlightInfoSMS");

            document.getElementById("Hidden1email").value = refid;
            break;
        }
    }

}
function isNumberKey(evt) {
    var charCode = (evt.which) ? evt.which : event.keyCode
    if (charCode > 31 && (charCode < 48 || charCode > 57))
        return false;
    return true;
}
function SendSMS() {
    var senderMobile = document.getElementById("senderMobile").value;
    if (senderMobile == "") {
        alert("Please Enter Mobile No");
        return false;
    }
    else if (senderMobile.length < 10) {
        alert("Please Enter 10 digits Mobile No ");
        return false;
    }
    else {
        $.ajax({
            type: "POST",
            url: "/flight/SubmitSMSRINT",
            data: '{Mobile:"' + senderMobile + '",RefId:"' + document.getElementById("Hidden1email").value + '",RefIdR:"' + document.getElementById("Hidden1email").value + '"}',
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (msg) {
                if (msg.d == "1") {
                    document.getElementById("senderMobile").value = "";

                    alert("SMS is sent successfully  ");

                    return true;
                }
                else {
                    alert("SMS is not sent  ");
                    return false;
                }
            },
            error: function (msg) {

            }

        });

    }
}
function emailtofriend(refid) {
    $("#FlightPopUp").css("display", "none");
    $("#datashowConcernModifyouter").css("display", "Block");
    $("#FlightPopUpEmail").css("display", "Block");
    jQuery('#FlightInfoSMS').html('');
    jQuery('#FlightInfoEmail').html('');
    var msg = sortF;
    for (var i = 0; i < msg.d.length; i++) {
        if (msg.d[i].FlightRefid == refid) {
            $("#TemplateFlightInfoEmail").tmpl(msg.d[i]).appendTo("#FlightInfoEmail");
            document.getElementById("Hidden1email").value = refid;
            break;
        }
    }
}

function CloseFlightPopUpEmail() {
    $("#datashowConcernModifyouter").css("display", "none");
    $("#FlightPopUpEmail").css("display", "none");
}
function Closedivpopup() {
    $("#datashowConcernModifyouter").css("display", "none");
    $("#divpopup").css("display", "none");
    var divbounces = "rootdiv" + document.getElementById("Hidden1email").value;
    $("#" + divbounces).animate({ borderWidth: "1px" }, 500);
    $("#" + divbounces).animate({ borderWidth: "0" }, 0);
}
window.Closedivpopup= Closedivpopup;
function Detailviewopen(data) {
    var Row1;
    Row1 = "<table width='550' cellpadding='2' cellspacing='0'><tr style='font-weight:bold;background-color: #000;color:#FFF;border-bottom: #dddddd 1px solid;font-family:verdana;'><td colspan='9' >Fare rules:-</td></tr><tr><td width='550' align='left'><span class='showfontsizeRule'>" + data + "</td></tr>";
    var RowAdd = "</table>";
    var divObj = document.getElementById("divFareRules");
    divObj.style.display = "block";
    divObj.left = '0px';
    divObj.top = '0px';
    divObj.innerHTML = Row1 + RowAdd;
}
function ddivclose() {
    var divObj = document.getElementById("divFareRules");
    divObj.style.display = "none";
}
function Detailviewopen2(data) {
    var Row1;
    Row1 = "<table width='550' cellpadding='2' cellspacing='0'><tr style='font-weight:bold;background-color: #000;color:#FFF;border-bottom: #dddddd 1px solid;'><td colspan='9' >Fare rules:-</td></tr><tr><td width='550' align='left'><span class='showfontsizeRule'>" + data + "</td></tr>";
    var RowAdd = "</table>";
    var divObj = document.getElementById("divFareRules2");
    divObj.style.display = "block";
    divObj.left = '0px';
    divObj.top = '0px';
    divObj.innerHTML = Row1 + RowAdd;
}
function ddivclose2() {
    var divObj = document.getElementById("divFareRules2");
    divObj.style.display = "none";
}
function fareruledivshow(data) {

    var ll = document.getElementById('myfakeu');
    if (ll.style.display != "none") {
        ll.style.display = "none";
        $('#myfakeu').text($('#myfakeu').html());
    }
    else {
        ll.style.display = "block";
        $('#myfakeu').html($('#myfakeu').text());
    }
}
window.fareruledivshow = fareruledivshow;
function FAREDETAILS(FlightRefid) {
    // FareRule(FlightRefid);
    $("#ITINERARY" + FlightRefid + "").css('border-bottom', '0px solid #C0C0C0');
    $("#FAREDETAILS" + FlightRefid + "").css('border-bottom', '3px solid #FEBD25');
    $("#FARERULE" + FlightRefid + "").css('border-bottom', '0px solid #C0C0C0');
    $("#BAGGAGEDETAILS" + FlightRefid + "").css('border-bottom', '0px solid #C0C0C0');
    $("#divITINERARY" + FlightRefid + "").slideUp();
    $("#FareSummary" + FlightRefid + "").slideDown();
    $("#Farerulesummery" + FlightRefid + "").slideUp();
    $("#Baggagesummery" + FlightRefid + "").slideUp();
}
window.FAREDETAILS =FAREDETAILS
function ITINERARY(FlightRefid) {
    $("#FAREDETAILS" + FlightRefid + "").css('border-bottom', '0px solid #C0C0C0');
    $("#ITINERARY" + FlightRefid + "").css('border-bottom', '3px solid #FEBD25');
    $("#FARERULE" + FlightRefid + "").css('border-bottom', '0px solid #C0C0C0');
    $("#BAGGAGEDETAILS" + FlightRefid + "").css('border-bottom', '0px solid #C0C0C0');
    $("#divITINERARY" + FlightRefid + "").slideDown();
    $("#FareSummary" + FlightRefid + "").slideUp();
    $("#Farerulesummery" + FlightRefid + "").slideUp();
    $("#Baggagesummery" + FlightRefid + "").slideUp();
}
window.ITINERARY =ITINERARY;
function FARERULE(FlightRefid) {
    // FareRule(FlightRefid);
    $("#ITINERARY" + FlightRefid + "").css('border-bottom', '0px solid #C0C0C0');
    $("#FAREDETAILS" + FlightRefid + "").css('border-bottom', '0px solid #C0C0C0');
    $("#BAGGAGEDETAILS" + FlightRefid + "").css('border-bottom', '0px solid #C0C0C0');
    $("#FARERULE" + FlightRefid + "").css('border-bottom', '3px solid #FEBD25');
    $("#divITINERARY" + FlightRefid + "").slideUp();
    $("#FareSummary" + FlightRefid + "").slideUp();
    $("#Baggagesummery" + FlightRefid + "").slideUp();
    $("#Farerulesummery" + FlightRefid + "").slideDown();
}
window.FARERULE=FARERULE;
function BAGGAGEDETAILS(FlightRefid) {
    // FareRule(FlightRefid);
    $("#ITINERARY" + FlightRefid + "").css('border-bottom', '0px solid #C0C0C0');
    $("#FAREDETAILS" + FlightRefid + "").css('border-bottom', '0px solid #C0C0C0');
    $("#FARERULE" + FlightRefid + "").css('border-bottom', '0px solid #C0C0C0');
    $("#BAGGAGEDETAILS" + FlightRefid + "").css('border-bottom', '3px solid #FEBD25');
    $("#divITINERARY" + FlightRefid + "").slideUp();
    $("#FareSummary" + FlightRefid + "").slideUp();
    $("#Farerulesummery" + FlightRefid + "").slideUp();
    $("#Baggagesummery" + FlightRefid + "").slideDown();
}
window.BAGGAGEDETAILS=BAGGAGEDETAILS;
function showtaxandCharges(FlightRefid) {

    $('#TaxAndChargesDIV' + FlightRefid).css('display') == 'none' ? $('#TaxAndChargesDIV' + FlightRefid).slideDown() : $('#TaxAndChargesDIV' + FlightRefid).slideUp();
    $('#TotalTaxDIV' + FlightRefid).slideUp();
    $('#CommDetailDIV' + FlightRefid).slideUp();
}
function showtotaltax(FlightRefid) {

    $('#TotalTaxDIV' + FlightRefid).css('display') == 'none' ? $('#TotalTaxDIV' + FlightRefid).slideDown() : $('#TotalTaxDIV' + FlightRefid).slideUp();
    $('#CommDetailDIV' + FlightRefid).slideUp();
}
function showCommDetail(FlightRefid) {

    $('#CommDetailDIV' + FlightRefid).css('display') == 'none' ? $('#CommDetailDIV' + FlightRefid).slideDown() : $('#CommDetailDIV' + FlightRefid).slideUp();
    $('#TotalTaxDIV' + FlightRefid).slideUp();
}
function chkunchk() {

    if (document.getElementById("chkdiscccfare").checked == true) {
        $('.discui').css({ display: 'block' });
        $('.actu').css({ display: 'block' })
        $(".einr").css('text-decoration', 'line-through red');
    }
    else {
        $('.discui').css({ display: 'none' });
        $('.actu').css({ display: 'none' })
        $(".einr").css('text-decoration', '');
    }
}
// function GetFlightMatrix() {
//     jQuery('#FlightMatrixs2').html('');
//     jQuery('#FlightMatrixs').html('');
//     $.ajax({
//         type: "POST",
//         url: "k_one.aspx/FlightMatrixRoundInt",
//         data: "{}",
//         contentType: "application/json; charset=utf-8",
//         dataType: "json",
//         success: function (msg) {
//             MatrixData = msg
//             if (msg.d.length < 10) {
//                 $("#content-2").css('display', 'none');
//                 $("#templeteFlightMatrix").tmpl(msg.d).appendTo("#FlightMatrixs2");

//             }
//             else {

//                 $("#content-2").css('display', 'block');
//                 $("#templeteFlightMatrix").tmpl(msg.d).appendTo("#FlightMatrixs");
//             }

//             $("#waitingload").css("display", "none");
//             $("#waitingloadbox").css("display", "none");

//             //  changeCurrency2();


//         },
//         error: function (msg) {

//         }

//     });


// };
function GetFlightStops() {
    jQuery('#stops').html('');

    $.ajax({
        type: "POST",
        url: "/flight/FlightStops",
        data: "{}",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (msg) {

            $("#templetestops").tmpl(msg.d).appendTo("#stops");
            GetFlightStopsR();
        },
        error: function (msg) {

        }

    });
};
function GetFlightStopsR() {
    jQuery('#stopsR').html('');

    $.ajax({
        type: "POST",
        url: "/flight/FlightStopsR",
        data: "{}",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (msg) {
            
            $("#templetestopsR").tmpl(msg.d).appendTo("#stopsR");

        },
        error: function (msg) {

        }

    });
};
function getCheckedF() {
    $("#modelpopupOUTER").css("display", "Block");
    $("#modelpopup").css("background-color", "Gray");
    $('#modelpopup').delay(1).fadeIn(400);
    $('#modelpopupOUTER').delay(1).fadeIn(400);
    var msg2 = sortF;
    var chkboxValue = new Array();
    $('ul.amenView').find('input:checkbox').each(function () {
        if (this.checked == true) {
            var v = $(this).val();
            chkboxValue.push(v);

        }

    });
    if (chkboxValue.length == 0) {
        jQuery('#tempView').html('');

    }
    else {
        var c = 0;
        jQuery('#tempView').html('');
        for (c; c < chkboxValue.length; c++) {
            var msg = sortF;
            for (var i = 0; i < msg.d.length; i++) {
               
                if (chkboxValue[c] == msg.d[i].Stop) {
                    $("#MyTemplate").tmpl(msg.d[i]).appendTo("#tempView");
                    var FlightRefid = "spanAmount" + msg.d[i].FlightRefid;


                }
            }
        }

    }

    $('#modelpopup').delay(1000).fadeOut(400);
    $('#modelpopupOUTER').delay(1000).fadeOut(400);
}
window.getCheckedF = getCheckedF;
function getCheckedFR() {
    $("#modelpopupOUTER").css("display", "Block");
    $("#modelpopup").css("background-color", "Gray");
    $('#modelpopup').delay(1).fadeIn(400);
    $('#modelpopupOUTER').delay(1).fadeIn(400);
    var msg2 = sortF;
    var chkboxValue = new Array();
    $('ul.amenViewR').find('input:checkbox').each(function () {
        if (this.checked == true) {
            var v = $(this).val();
            chkboxValue.push(v);

        }

    });
    if (chkboxValue.length == 0) {
        jQuery('#tempView').html('');

    }
    else {
        var c = 0;
        jQuery('#tempView').html('');

        for (c; c < chkboxValue.length; c++) {
            var msg = sortF;

            for (var i = 0; i < msg.d.length; i++) {

                if (chkboxValue[c] == msg.d[i].StopR) {

                    $("#MyTemplate").tmpl(msg.d[i]).appendTo("#tempView");
                    var FlightRefid = "spanAmount" + msg.d[i].FlightRefid;
                }
            }
        }
    }
    $('#modelpopup').delay(1000).fadeOut(400);
    $('#modelpopupOUTER').delay(1000).fadeOut(400);
}
window.getCheckedFR  =getCheckedFR ;
function getCheckedFAirlines() {
    $("#modelpopupOUTER").css("display", "Block");
    $("#modelpopup").css("background-color", "Gray");
    $('#modelpopup').delay(1).fadeIn(400);
    $('#modelpopupOUTER').delay(1).fadeIn(400);

    var msg2 = sortF;
    var chkboxValue = new Array();
    $('ul.amenViewairline').find('input:checkbox').each(function () {
        if (this.checked == true) {
            var v = $(this).val();
            chkboxValue.push(v);

        }

    });
    if (chkboxValue.length == 0) {
        jQuery('#tempView').html('');

    }
    else {
        var c = 0;
        jQuery('#tempView').html('');
        for (c; c < chkboxValue.length; c++) {
            var msg = sortF;
            for (var i = 0; i < msg.d.length; i++) {
                if (chkboxValue[c] == msg.d[i].FlightName) {
                    $("#MyTemplate").tmpl(msg.d[i]).appendTo("#tempView");
                    var FlightRefid = "spanAmount" + msg.d[i].FlightRefid;

                }
            }
        }
    }
    $('#modelpopup').delay(1000).fadeOut(400);
    $('#modelpopupOUTER').delay(1000).fadeOut(400);
}
window.getCheckedFAirlines= getCheckedFAirlines;
(function ($) {
    $(window).load(function () {
        var content = $("#content_1"), autoScrollTimer = 8000, autoScrollTimerAdjust, autoScroll;
        content.mCustomScrollbar({
            scrollButtons: {
                enable: true
            },
            callbacks: {
                whileScrolling: function () { autoScrollTimerAdjust = autoScrollTimer * mcs.topPct / 100; },
                onScroll: function () { if (this.data("mCS_trigger") === "internal") { AutoScrollOff(); } }
            }
        });
        content.addClass("auto-scrolling-on auto-scrolling-to-bottom");
        // AutoScrollOn("bottom");
        $(".auto-scrolling-toggle").click(function (e) {
            e.preventDefault();
            if (content.hasClass("auto-scrolling-on")) {
                AutoScrollOff();
            } else {
                if (content.hasClass("auto-scrolling-to-top")) {
                    AutoScrollOn("top", autoScrollTimerAdjust);
                } else {
                    AutoScrollOn("bottom", autoScrollTimer - autoScrollTimerAdjust);
                }
            }
        });
        function AutoScrollOn(to, timer) {
            if (!timer) { timer = autoScrollTimer; }
            content.addClass("auto-scrolling-on").mCustomScrollbar("scrollTo", to, { scrollInertia: timer, scrollEasing: "easeInOutQuad" });
            autoScroll = setTimeout(function () {
                if (content.hasClass("auto-scrolling-to-top")) {
                    AutoScrollOn("bottom", autoScrollTimer - autoScrollTimerAdjust);
                    content.removeClass("auto-scrolling-to-top").addClass("auto-scrolling-to-bottom");
                } else {
                    AutoScrollOn("top", autoScrollTimerAdjust);
                    content.removeClass("auto-scrolling-to-bottom").addClass("auto-scrolling-to-top");
                }
            }, timer);
        }
        function AutoScrollOff() {
            clearTimeout(autoScroll);
            content.removeClass("auto-scrolling-on").mCustomScrollbar("stop");
        }
    });
})(jQuery);
function show(FlightRefid) {
    if ($("#DetailViewplus" + FlightRefid + "").css("display") == "block") {
        $("#divshow" + FlightRefid + "").show('slow');
        $("#DetailViewplus" + FlightRefid + "").css("display", "none");
        $("#DetailViewminus" + FlightRefid + "").css("display", "block");
    }
    else {
        $("#divshow" + FlightRefid + "").hide('slow');
        $("#DetailViewplus" + FlightRefid + "").css("display", "block");
        $("#DetailViewminus" + FlightRefid + "").css("display", "none");

    }


}
window.show= show;
(function ($) {
    $(window).load(function () {
        var content = $("#content_3"), autoScrollTimer = 8000, autoScrollTimerAdjust, autoScroll;
        content.mCustomScrollbar({
            scrollButtons: {
                enable: true
            },
            callbacks: {
                whileScrolling: function () { autoScrollTimerAdjust = autoScrollTimer * mcs.topPct / 100; },
                onScroll: function () { if (this.data("mCS_trigger") === "internal") { AutoScrollOff(); } }
            }
        });
        content.addClass("auto-scrolling-on auto-scrolling-to-bottom");
        // AutoScrollOn("bottom");
        $(".auto-scrolling-toggle").click(function (e) {
            e.preventDefault();
            if (content.hasClass("auto-scrolling-on")) {
                AutoScrollOff();
            } else {
                if (content.hasClass("auto-scrolling-to-top")) {
                    AutoScrollOn("top", autoScrollTimerAdjust);
                } else {
                    AutoScrollOn("bottom", autoScrollTimer - autoScrollTimerAdjust);
                }
            }
        });
        function AutoScrollOn(to, timer) {
            if (!timer) { timer = autoScrollTimer; }
            content.addClass("auto-scrolling-on").mCustomScrollbar("scrollTo", to, { scrollInertia: timer, scrollEasing: "easeInOutQuad" });
            autoScroll = setTimeout(function () {
                if (content.hasClass("auto-scrolling-to-top")) {
                    AutoScrollOn("bottom", autoScrollTimer - autoScrollTimerAdjust);
                    content.removeClass("auto-scrolling-to-top").addClass("auto-scrolling-to-bottom");
                } else {
                    AutoScrollOn("top", autoScrollTimerAdjust);
                    content.removeClass("auto-scrolling-to-bottom").addClass("auto-scrolling-to-top");
                }
            }, timer);
        }
        function AutoScrollOff() {
            clearTimeout(autoScroll);
            content.removeClass("auto-scrolling-on").mCustomScrollbar("stop");
        }
    });
})(jQuery);



$(window).load(function () {
    //showmodifystatus();
    SetValueInModifyControl();
    calculatenofpax();
});



function showmodifystatus() {
    if ($("#modifySearch").css("display") == "none") {
        $("#modifySearch").show();
        document.getElementById('modifypart').style.display = "block";
        document.getElementById('mldt1').style.display = "block";
        document.getElementById('pldt1').style.display = "none";
    }
    else {
        $("#modifySearch").hide();
        document.getElementById('modifypart').style.display = "none";
        document.getElementById('mldt1').style.display = "none";
        document.getElementById('pldt1').style.display = "block";
    }
    SetValueInModifyControl();
}

function netfareshowforb2c() {

    document.getElementById("chkdiscccfare").checked = true;
    $('.hidchk').css({ display: 'none' });
    $('.hidtdscus').css({ display: 'none' });
    chkunchk();
    return "";
}


function filterclck() {

    var ll = document.getElementById('fare_type');
    if (ll.style.display != "block") {
        ll.style.display = "block";
    }
    else {
        ll.style.display = "none";
    }
}

function modifyclk() {
    var ll = document.getElementById('hikjhj');
    if (ll.style.display != "block") {
        ll.style.display = "block";
    }
    else {
        ll.style.display = "none";
    }
}
$(document).ready(function () {
    calculatenofpax();
    // newspicdata();

    $('#im1').click(function () {
        $('#pop1').hide('slow');
    });

    $('#im2').click(function () {
        $('#pop2').hide('slow');
    });
    $('#im3').click(function () {
        $('#pop3').hide('slow');
    });
});
function newspicdata() {
    $.ajax({
        type: "POST",
        url: "/flight/news",
        data: "{}",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (msg) {
            $('#Slide1').html('');
            var html = [];
            $('#Slide2').html('');
            var html2 = [];
            $('#Slide3').html('');
            var html3 = [];
            if (msg.d.length > 0) {
                for (var i = 0; i < msg.d.length; i++) {
                    if (msg.d[i].slide == "1") {
                        $('#pop1').show('slow');

                        if (msg.d[i].slideorder == "1") {
                            html.push("<div id='Slide1Show1' style='display:block'>");
                            html.push("<div style='width:280px;padding:5px;height:78px'>" + msg.d[i].slidetext + "</div>");
                            if (msg.d[i].slideno > 1) {
                                html.push("<div style='padding:5px'><a href='javascript:VOID(0);' ><img src='image/left_off.gif' style='width:18px;height:18px;'></a> &nbsp;&nbsp;<a href='javascript:VOID(0);' onclick='msgshowhideRIGHTOne1();'><img src='image/right.gif' style='width:18px;height:18px;'></a></div>");
                            }
                            html.push("</div>");

                        }
                        if (msg.d[i].slideorder == "2") {
                            html.push("<div id='Slide1Show2' style='display:none'>");
                            html.push("<div style='width:280px;padding:5px;height:78px'>" + msg.d[i].slidetext + "</div>");
                            if (msg.d[i].slideno == 2) {
                                html.push("<div style='padding:5px'><a href='javascript:VOID(0);' onclick='msgshowhideLeftOne1();'><img src='image/left.gif' style='width:18px;height:18px;'></a> &nbsp;&nbsp;<a href='javascript:VOID(0);' ><img src='image/right_off.gif' style='width:18px;height:18px;'></a></div>");
                            }
                            if (msg.d[i].slideno == 3) {
                                html.push("<div style='padding:5px'><a href='javascript:VOID(0);' onclick='msgshowhideLeftTwo11();'><img src='image/left.gif' style='width:18px;height:18px;'></a> &nbsp;&nbsp;<a href='javascript:VOID(0);' onclick='msgshowhideRIGHTTwo1();'><img src='image/right.gif' style='width:18px;height:18px;'></a></div>");
                            }
                            html.push("</div>");
                        }
                        if (msg.d[i].slideorder == "3") {
                            html.push("<div id='Slide1Show3' style='display:none'>");
                            html.push("<div style='width:280px;padding:5px;height:78px'>" + msg.d[i].slidetext + "</div>");
                            if (msg.d[i].slideno == 3) {
                                html.push("<div style='padding:5px'><a href='javascript:VOID(0);' onclick='msgshowhideLeftTwo1();' ><img src='image/left.gif' style='width:18px;height:18px;'></a> &nbsp;&nbsp;<a><img src='image/right_off.gif' style='width:18px;height:18px;'></a></div>");
                            }
                            html.push("</div>");
                        }
                    }
                    if (msg.d[i].slide == "2") {
                        $('#pop2').show('slow');
                        if (msg.d[i].slideorder == "1") {
                            html2.push("<div  id='Slide2Show1' style='display:block'>");
                            html2.push("<div style='width:280px;padding:5px;height:78px'>" + msg.d[i].slidetext + "</div>");
                            if (msg.d[i].slideno > 1) {
                                html2.push("<div style='padding:5px'><a href='javascript:VOID(0);' ><img src='image/left_off.gif' style='width:18px;height:18px;'></a> &nbsp;&nbsp;<a href='javascript:VOID(0);' onclick='msgshowhideRIGHTOne2();'><img src='image/right.gif' style='width:18px;height:18px;'></a></div>");
                            }
                            html2.push("</div>");
                        }
                        if (msg.d[i].slideorder == "2") {
                            html2.push("<div id='Slide2Show2' style='display:none'>");
                            html2.push("<div style='width:280px;padding:5px;height:78px' >" + msg.d[i].slidetext + "</div>");
                            if (msg.d[i].slideno == 2) {
                                html2.push("<div style='padding:5px'><a href='javascript:VOID(0);' onclick='msgshowhideLeftOne2();'><img src='image/left.gif' style='width:18px;height:18px;'></a> &nbsp;&nbsp;<a href='javascript:VOID(0);' ><img src='image/right_off.gif' style='width:18px;height:18px;'></a></div>");
                            }
                            if (msg.d[i].slideno == 3) {
                                html2.push("<div style='padding:5px'><a href='javascript:VOID(0);' onclick='msgshowhideLeftTwo22();'><img src='image/left.gif' style='width:18px;height:18px;'></a> &nbsp;&nbsp;<a href='javascript:VOID(0);' onclick='msgshowhideRIGHTTwo2();'><img src='image/right.gif' style='width:18px;height:18px;'></a></div>");
                            }
                            html2.push("</div>");
                        }
                        if (msg.d[i].slideorder == "3") {
                            html2.push("<div id='Slide2Show3' style='display:none'>");
                            html2.push("<div style='width:280px;padding:5px;height:78px'>" + msg.d[i].slidetext + "</div>");
                            if (msg.d[i].slideno == 3) {
                                html2.push("<div style='padding:5px'><a href='javascript:VOID(0);' onclick='msgshowhideLeftTwo2();'><img src='image/left.gif' style='width:18px;height:18px;'></a> &nbsp;&nbsp;<a href='javascript:VOID(0);'><img src='image/right_off.gif' style='width:18px;height:18px;'></a></div>");
                            }
                            html2.push("</div>");
                        }
                    }
                    if (msg.d[i].slide == "3") {
                        $('#pop3').show('slow');
                        if (msg.d[i].slideorder == "1") {
                            html3.push("<div id='Slide3Show1' style='display:block'>");
                            html3.push("<div style='width:280px;padding:5px;height:78px'>" + msg.d[i].slidetext + "</div>");
                            if (msg.d[i].slideno > 1) {
                                html3.push("<div style='padding:5px'><a href='javascript:VOID(0);' ><img src='image/left_off.gif' style='width:18px;height:18px;'></a> &nbsp;&nbsp;<a href='javascript:VOID(0);' onclick='msgshowhideRIGHTOne3();'><img src='image/right.gif' style='width:18px;height:18px;'></a></div>");
                            }
                            html3.push("</div>");
                        }
                        if (msg.d[i].slideorder == "2") {
                            html3.push("<div id='Slide3Show2' style='display:none'>");
                            html3.push("<div style='width:280px;padding:5px;height:78px'>" + msg.d[i].slidetext + "</div>");
                            if (msg.d[i].slideno == 2) {
                                html3.push("<div style='padding:5px'><a href='javascript:VOID(0);' onclick='msgshowhideLeftOne3();'><img src='image/left.gif' style='width:18px;height:18px;'></a> &nbsp;&nbsp;<a href='javascript:VOID(0);' ><img src='image/right_off.gif' style='width:18px;height:18px;'></a></div>");
                            }
                            if (msg.d[i].slideno == 3) {
                                html3.push("<div style='padding:5px'><a href='javascript:VOID(0);' onclick='msgshowhideLeftTwo32();'><img src='image/left.gif' style='width:18px;height:18px;'></a> &nbsp;&nbsp;<a href='javascript:VOID(0);' onclick='msgshowhideRIGHTTwo3();'><img src='image/right.gif' style='width:18px;height:18px;'></a></div>");
                            }
                            html3.push("</div>");
                        }
                        if (msg.d[i].slideorder == "3") {
                            html3.push("<div id='Slide3Show3' style='display:none'>");
                            html3.push("<div style='width:280px;padding:5px;height:78px'>" + msg.d[i].slidetext + "</div>");
                            if (msg.d[i].slideno == 3) {
                                html3.push("<div style='padding:5px'><a href='javascript:VOID(0);' onclick='msgshowhideLeftTwo3();'><img src='image/left.gif' style='width:18px;height:18px;'></a> &nbsp;&nbsp;<a href='javascript:VOID(0);'><img src='image/right_off.gif' style='width:18px;height:18px;'></a></div>");
                            }
                            html3.push("</div>");
                        }
                    }
                }
            }
            $('#Slide1').append(html.join(''));
            $('#Slide2').append(html2.join(''));
            $('#Slide3').append(html3.join(''));
        },
        error: function (msg) {

        }
    });
}
function msgshowhideLeftOne1() {
    $('#Slide1Show2').hide();
    $('#Slide1Show1').show();
}
function msgshowhideLeftTwo1() {
    $('#Slide1Show3').hide();
    $('#Slide1Show2').show();
}
function msgshowhideLeftTwo11() {
    $('#Slide1Show2').hide();
    $('#Slide1Show1').show();
}
function msgshowhideLeftOne2() {
    $('#Slide2Show2').hide();
    $('#Slide2Show1').show();
}
function msgshowhideLeftTwo2() {
    $('#Slide2Show3').hide();
    $('#Slide2Show2').show();
}
function msgshowhideLeftTwo22() {
    $('#Slide2Show2').hide();
    $('#Slide2Show1').show();
}
function msgshowhideLeftOne3() {
    $('#Slide3Show2').hide();
    $('#Slide3Show1').show();
}
function msgshowhideLeftTwo3() {
    $('#Slide3Show3').hide();
    $('#Slide3Show2').show();
}
function msgshowhideLeftTwo32() {
    $('#Slide3Show2').hide();
    $('#Slide3Show1').show();
}
function msgshowhideRIGHTOne1() {
    $('#Slide1Show1').hide();
    $('#Slide1Show2').show();
}
function msgshowhideRIGHTTwo1() {
    $('#Slide1Show2').hide();
    $('#Slide1Show3').show();
}
function msgshowhideRIGHTOne2() {
    $('#Slide2Show1').hide();
    $('#Slide2Show2').show();
}
function msgshowhideRIGHTTwo2() {
    $('#Slide2Show2').hide();
    $('#Slide2Show3').show();
}
function msgshowhideRIGHTOne3() {
    $('#Slide3Show1').hide();
    $('#Slide3Show2').show();
}
function msgshowhideRIGHTTwo3() {
    $('#Slide3Show2').hide();
    $('#Slide3Show3').show();
}
function showfilteee() {

    var ll = document.getElementById('fare_type');

    if (ll.style.display != "none") {
        ll.style.display = "none";
    }
    else {
        ll.style.display = "block";
    }
}
var currency = "INR";
var currencyAmount = 0;
function changeCurrency() {
    currency = document.getElementById("currency").value;
    for (i = 0; i < CurrancyDataMsg.d.length; i++) {
        if (currency == CurrancyDataMsg.d[i].CODE) {
            currencyAmount = CurrancyDataMsg.d[i].CURRENCY_AMOUNT;
            break;
        }
    }
    jQuery('#tempView').html('');
    for (var i = 0; i < sortFCurrency.d.length; i++) {
        $("#MyTemplate").tmpl(sortFCurrency.d[i]).appendTo("#tempView");
    }
    var msg = sortF;
    var j = msg.d.length;
    for (i = 0; i < j; i++) {
        var FlightRefid = "spanAmount" + msg.d[i].FlightRefid;
        var amount = parseFloat(msg.d[i].TotalAmount) * parseFloat(currencyAmount);
        if (currency == "INR") {
            document.getElementById(FlightRefid).innerHTML = currency + " " + msg.d[i].FlightAmount;
            document.getElementById("Total" + msg.d[i].FlightRefid).innerHTML = currency + " " + msg.d[i].FlightAmount;
            document.getElementById("ServiceFee" + msg.d[i].FlightRefid).innerHTML = currency + " " + msg.d[i].ServiceFee;
            document.getElementById("AHC" + msg.d[i].FlightRefid).innerHTML = currency + " " + msg.d[i].AHC;
            document.getElementById("SF" + msg.d[i].FlightRefid).innerHTML = currency + " " + msg.d[i].SF;
            document.getElementById("TAXFEE" + msg.d[i].FlightRefid).innerHTML = currency + " " + msg.d[i].TaxAmount;
            document.getElementById("BaseFare" + msg.d[i].FlightRefid).innerHTML = currency + " " + msg.d[i].BaseAmount;
            document.getElementById('sliderrangediv').style.display = 'block';
        }
        else {
            document.getElementById('sliderrangediv').style.display = 'none';
            var ftotal; var fServiceFee; var fAHC; var fSF; var fTaxAmount; var fBaseAmount;
            if ((msg.d[i].AHC).indexOf(",") > -1) {
                fAHC = parseFloat(parseFloat((msg.d[i].AHC).replace(/\,/g, '')) * parseFloat(currencyAmount));
                document.getElementById("AHC" + msg.d[i].FlightRefid).innerHTML = currency + " " + fAHC.toFixed(3);
            }
            else {
                fAHC = parseFloat(parseFloat(msg.d[i].AHC) * parseFloat(currencyAmount));
                document.getElementById("AHC" + msg.d[i].FlightRefid).innerHTML = currency + " " + fAHC.toFixed(3);
            }
            if ((msg.d[i].ServiceFee).indexOf(",") > -1) {
                fServiceFee = parseFloat(parseFloat((msg.d[i].ServiceFee).replace(/\,/g, '')) * parseFloat(currencyAmount));
                document.getElementById("ServiceFee" + msg.d[i].FlightRefid).innerHTML = currency + " " + fServiceFee.toFixed(3);
            }
            else {
                fServiceFee = parseFloat(parseFloat(msg.d[i].ServiceFee) * parseFloat(currencyAmount));
                document.getElementById("ServiceFee" + msg.d[i].FlightRefid).innerHTML = currency + " " + fServiceFee.toFixed(3);
            }
            if ((msg.d[i].SF).indexOf(",") > -1) {
                fSF = parseFloat(parseFloat((msg.d[i].SF).replace(/\,/g, '')) * parseFloat(currencyAmount));
                document.getElementById("SF" + msg.d[i].FlightRefid).innerHTML = currency + " " + fSF.toFixed(3)
            }
            else {
                fSF = parseFloat(parseFloat(msg.d[i].SF) * parseFloat(currencyAmount));
                document.getElementById("SF" + msg.d[i].FlightRefid).innerHTML = currency + " " + fSF.toFixed(3);
            }

            if ((msg.d[i].TaxAmount).indexOf(",") > -1) {
                fTaxAmount = parseFloat(parseFloat((msg.d[i].TaxAmount).replace(/\,/g, '')) * parseFloat(currencyAmount));
                document.getElementById("TAXFEE" + msg.d[i].FlightRefid).innerHTML = currency + " " + fTaxAmount.toFixed(3);
            }
            else {
                fTaxAmount = parseFloat(parseFloat(msg.d[i].TaxAmount) * parseFloat(currencyAmount));
                document.getElementById("TAXFEE" + msg.d[i].FlightRefid).innerHTML = currency + " " + fTaxAmount.toFixed(3);
            }
            if ((msg.d[i].BaseAmount).indexOf(",") > -1) {
                fBaseAmount = parseFloat(parseFloat((msg.d[i].BaseAmount).replace(/\,/g, '')) * parseFloat(currencyAmount));
                document.getElementById("BaseFare" + msg.d[i].FlightRefid).innerHTML = currency + " " + fBaseAmount.toFixed(3);
            }
            else {
                fBaseAmount = parseFloat(parseFloat(msg.d[i].BaseAmount) * parseFloat(currencyAmount));
                document.getElementById("BaseFare" + msg.d[i].FlightRefid).innerHTML = currency + " " + fBaseAmount.toFixed(3);
            }
            ftotal = parseInt(fServiceFee + fAHC + fSF + fTaxAmount + fBaseAmount);
            document.getElementById("Total" + msg.d[i].FlightRefid).innerHTML = currency + " " + parseInt(amount);
            document.getElementById(FlightRefid).innerHTML = currency + " " + parseInt(amount);
        }
    }
    var MatrixData_C = MatrixData;
    var k = MatrixData_C.d.length;
    for (i = 0; i < k; i++) {
        // if (MatrixData_C.d[i].Airlines != "SG" && MatrixData_C.d[i].Airlines != "6E") {
        if (MatrixData_C.d[i].FareM != "---") {
            document.getElementById("FareM" + MatrixData_C.d[i].Airlines).innerHTML = parseInt(parseInt(MatrixData_C.d[i].FareM) * parseFloat(currencyAmount));
        }
        if (MatrixData_C.d[i].FareA != "---") {
            document.getElementById("FareA" + MatrixData_C.d[i].Airlines).innerHTML = parseInt(parseInt(MatrixData_C.d[i].FareA) * parseFloat(currencyAmount));
        }
        if (MatrixData_C.d[i].FareN != "---") {
            document.getElementById("FareN" + MatrixData_C.d[i].Airlines).innerHTML = parseInt(parseInt(MatrixData_C.d[i].FareN) * parseFloat(currencyAmount));
        }
        if (MatrixData_C.d[i].FareMN != "---") {
            document.getElementById("FareMN" + MatrixData_C.d[i].Airlines).innerHTML = parseInt(parseInt(MatrixData_C.d[i].FareMN) * parseFloat(currencyAmount));
        }
    }
}
function changeCurrency2() {
    currency = document.getElementById("currency").value;
    for (i = 0; i < CurrancyDataMsg.d.length; i++) {
        if (currency == CurrancyDataMsg.d[i].CODE) {
            currencyAmount = CurrancyDataMsg.d[i].CURRENCY_AMOUNT;
            break;
        }
    }
    jQuery('#tempView').html('');
    for (var i = 0; i < sortFCurrency.d.length; i++) {
        $("#MyTemplate").tmpl(sortFCurrency.d[i]).appendTo("#tempView");
    }
    var msg = sortF;
    var j = msg.d.length;
    for (i = 0; i < j; i++) {
        var FlightRefid = "spanAmount" + msg.d[i].FlightRefid;
        var amount = parseFloat(msg.d[i].TotalAmount) * parseFloat(currencyAmount);
        if (currency == "INR") {
            document.getElementById(FlightRefid).innerHTML = currency + " " + msg.d[i].FlightAmount;
            document.getElementById("Total" + msg.d[i].FlightRefid).innerHTML = currency + " " + msg.d[i].FlightAmount;
            document.getElementById("ServiceFee" + msg.d[i].FlightRefid).innerHTML = currency + " " + msg.d[i].ServiceFee;
            document.getElementById("AHC" + msg.d[i].FlightRefid).innerHTML = currency + " " + msg.d[i].AHC;
            document.getElementById("SF" + msg.d[i].FlightRefid).innerHTML = currency + " " + msg.d[i].SF;
            document.getElementById("TAXFEE" + msg.d[i].FlightRefid).innerHTML = currency + " " + msg.d[i].TaxAmount;
            document.getElementById("BaseFare" + msg.d[i].FlightRefid).innerHTML = currency + " " + msg.d[i].BaseAmount;
            document.getElementById('sliderrangediv').style.display = 'block';
        }
        else {
            document.getElementById('sliderrangediv').style.display = 'none';
            var ftotal; var fServiceFee; var fAHC; var fSF; var fTaxAmount; var fBaseAmount;
            if ((msg.d[i].AHC).indexOf(",") > -1) {
                fAHC = parseFloat(parseFloat((msg.d[i].AHC).replace(/\,/g, '')) * parseFloat(currencyAmount));
                document.getElementById("AHC" + msg.d[i].FlightRefid).innerHTML = currency + " " + fAHC.toFixed(3);
            }
            else {
                fAHC = parseFloat(parseFloat(msg.d[i].AHC) * parseFloat(currencyAmount));
                document.getElementById("AHC" + msg.d[i].FlightRefid).innerHTML = currency + " " + fAHC.toFixed(3);
            }
            if ((msg.d[i].ServiceFee).indexOf(",") > -1) {
                fServiceFee = parseFloat(parseFloat((msg.d[i].ServiceFee).replace(/\,/g, '')) * parseFloat(currencyAmount));
                document.getElementById("ServiceFee" + msg.d[i].FlightRefid).innerHTML = currency + " " + fServiceFee.toFixed(3);
            }
            else {
                fServiceFee = parseFloat(parseFloat(msg.d[i].ServiceFee) * parseFloat(currencyAmount));
                document.getElementById("ServiceFee" + msg.d[i].FlightRefid).innerHTML = currency + " " + fServiceFee.toFixed(3);
            }
            if ((msg.d[i].SF).indexOf(",") > -1) {
                fSF = parseFloat(parseFloat((msg.d[i].SF).replace(/\,/g, '')) * parseFloat(currencyAmount));
                document.getElementById("SF" + msg.d[i].FlightRefid).innerHTML = currency + " " + fSF.toFixed(3)
            }
            else {
                fSF = parseFloat(parseFloat(msg.d[i].SF) * parseFloat(currencyAmount));
                document.getElementById("SF" + msg.d[i].FlightRefid).innerHTML = currency + " " + fSF.toFixed(3);
            }
            if ((msg.d[i].TaxAmount).indexOf(",") > -1) {
                fTaxAmount = parseFloat(parseFloat((msg.d[i].TaxAmount).replace(/\,/g, '')) * parseFloat(currencyAmount));
                document.getElementById("TAXFEE" + msg.d[i].FlightRefid).innerHTML = currency + " " + fTaxAmount.toFixed(3);
            }
            else {
                fTaxAmount = parseFloat(parseFloat(msg.d[i].TaxAmount) * parseFloat(currencyAmount));
                document.getElementById("TAXFEE" + msg.d[i].FlightRefid).innerHTML = currency + " " + fTaxAmount.toFixed(3);
            }
            if ((msg.d[i].BaseAmount).indexOf(",") > -1) {
                fBaseAmount = parseFloat(parseFloat((msg.d[i].BaseAmount).replace(/\,/g, '')) * parseFloat(currencyAmount));
                document.getElementById("BaseFare" + msg.d[i].FlightRefid).innerHTML = currency + " " + fBaseAmount.toFixed(3);
            }
            else {
                fBaseAmount = parseFloat(parseFloat(msg.d[i].BaseAmount) * parseFloat(currencyAmount));
                document.getElementById("BaseFare" + msg.d[i].FlightRefid).innerHTML = currency + " " + fBaseAmount.toFixed(3);
            }
            ftotal = parseInt(fServiceFee + fAHC + fSF + fTaxAmount + fBaseAmount);
            document.getElementById("Total" + msg.d[i].FlightRefid).innerHTML = currency + " " + parseInt(amount);
            document.getElementById(FlightRefid).innerHTML = currency + " " + parseInt(amount);
        }
    }
    var MatrixData_C = MatrixData;
    var k = MatrixData_C.d.length;
    for (i = 0; i < k; i++) {
        // if (MatrixData_C.d[i].Airlines != "SG" && MatrixData_C.d[i].Airlines != "6E") {
        if (MatrixData_C.d[i].FareM != "---") {
            document.getElementById("FareM" + MatrixData_C.d[i].Airlines).innerHTML = parseInt(parseInt(MatrixData_C.d[i].FareM) * parseFloat(currencyAmount));
        }
        if (MatrixData_C.d[i].FareA != "---") {
            document.getElementById("FareA" + MatrixData_C.d[i].Airlines).innerHTML = parseInt(parseInt(MatrixData_C.d[i].FareA) * parseFloat(currencyAmount));
        }
        if (MatrixData_C.d[i].FareN != "---") {
            document.getElementById("FareN" + MatrixData_C.d[i].Airlines).innerHTML = parseInt(parseInt(MatrixData_C.d[i].FareN) * parseFloat(currencyAmount));
        }
        if (MatrixData_C.d[i].FareMN != "---") {
            document.getElementById("FareMN" + MatrixData_C.d[i].Airlines).innerHTML = parseInt(parseInt(MatrixData_C.d[i].FareMN) * parseFloat(currencyAmount));
        }
    }
}
function calculatenofpax() {
        
    var totalpax = parseInt($("#adult").val()) + parseInt($("#child").val()) + parseInt($("#infant").val());
    document.getElementById("travlvalue").textContent = totalpax;
    $('#travlvalue').trigger('click');
    $("#passengerbx").css("display", "none");
}
window.calculatenofpax= calculatenofpax;
function GetFlightMatrix() {
    jQuery('#FlightMatrixs2').html('');
    jQuery('#FlightMatrixs').html('');
    $.ajax({
        type: "POST",
        url: "/flight/FlightMatrixRoundInt",
        data: "{}",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (msg) {
            
            MatrixData = msg;
            if (msg.d.length < 10) {
                $("#content-2").css('display', 'none');
                $("#templeteFlightMatrix").tmpl(msg.d).appendTo("#FlightMatrixs2");

            }
            else {

                $("#content-2").css('display', 'block');
                $("#templeteFlightMatrix").tmpl(msg.d).appendTo("#FlightMatrixs");
            }

            $("#waitingload").css("display", "none");
            $("#waitingloadbox").css("display", "none");

            //  changeCurrency2();


        },
        error: function (msg) {
            $("#waitingload").css("display", "none");
            $("#waitingloadbox").css("display", "none");
        }

    });


};
function prefer() {
    var x = document.getElementById("flighimg");
    if (x.style.display === "none") {
      x.style.display = "block";
      $("#hidesec").css("display", "none");
      $("#showsec").css("display", "block");
    } else {
      x.style.display = "none";
      $("#hidesec").css("display", "block");
      $("#showsec").css("display", "none");
    }
  }
  window.prefer = prefer;
  function FareSummaryopen(data) {
    $("#" + data).slideToggle();
  }
  window.FareSummaryopen = FareSummaryopen;
  function FareSummaryclose(data) {
    $("#" + data).css("display", "none");
  }
  window.FareSummaryclose = FareSummaryclose;