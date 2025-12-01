var checkvalue = "";
var totMonthToShow = 2;
var dominName = "https://localhost:4682/MyProject";
$(document).ready(function () {

    var d = new Date();
    var month = d.getMonth() + 1;
    var day = d.getDate();
    var year = d.getFullYear();
    var dateNow = year + "-" + month + "-" + day;
    $("#hotelCityAirport").autocomplete({
        autoFocus: true,
        source: function (request, response) {
            $.ajax({
                //url: 'WebMetthod.aspx/GetCountryCityList_Hotel',
                //data: "{ 'strinputCountry': '" + request.term + "'}",
                url: 'WebMetthod.aspx/GetHotelSetor',
                data: "{ 'searchStr': '" + request.term + "'}",

                dataType: "json",
                type: "POST",
                contentType: "application/json; charset=utf-8",
                success: function (data) {
                   
                    //var obj = JSON.parse(data.d);
                    //response($.map(obj, function (item) {
                    //var obj = JSON.parse(data.d);
                    response($.map(data.d, function (item) {
                        return {
                            //label: item._CityName + "(" + item._CityCode + "-" + item._CountryCode + ")",
                            //value: item._CityCode
                            label: item,
                            value: item
                        }
                    }));
                },
                error: function (response) {
                    alert(response.responseText);
                },
                failure: function (response) {
                    alert(response.responseText);
                }
            });
            //$.ajax({
            //    url: dominName + "/DashBoard.aspx",
            //    dataType: "json",

            //    data: { term: request.term },
            //    success: function (data) {
            //        response($.map(data, function (item) {
            //            return {
            //                label: item.Code,
            //                value: item.Code,
            //            };
            //        }));
            //    }
            //});
        },
        minLength: 3,
        select: function (event, ui) {
            $(this).val(ui.item.label);
            $("#hotelCheckIn").focus();
            event.preventDefault();
        }
    });

    if ($(window).width() > 767) {
        totMonthToShow = 2;
    }
    else {
        totMonthToShow = 1;
    }
    //var today = new Date(new Date().getTime() + 1 * 60 * 60 * 1000);
    //var dd = today.getDate();
    //var mm = today.getMonth() + 1; //January is 0!

    //var yyyy = today.getFullYear();
    //if (dd < 10) {
    //    dd = '0' + dd
    //}
    //if (mm < 10) {
    //    mm = '0' + mm
    //}
    //var fromDate = dd + '-' + mm + '-' + yyyy;

    //var tomorowDate = new Date(new Date().getTime() + 30 * 60 * 60 * 1000);


    //var ddR = tomorowDate.getDate();
    //var mmR = tomorowDate.getMonth() + 1;

    //var yyyyR = tomorowDate.getFullYear();
    //if (ddR < 10) {
    //    ddR = '0' + ddR
    //}
    //if (mmR < 10) {
    //    mmR = '0' + mmR
    //}
    //var toRDate = ddR + '-' + mmR + '-' + yyyyR;
    //$("#hotelCheckIn").val(fromDate);
    // $("#hotelCheckOut").val(toRDate);



    //$("#hotelCityAirport").click(function (event) {
    //    this.focus();
    //    this.setSelectionRange(0, 9999);
    //    if ($(window).width() > 767) {
    //        $('html, body').animate({ scrollTop: 500 }, 1000);
    //    }

    //});


    $(".room1child1, .room1child2, .room2child1, .room2child2, .room2").hide();
    $("select#rooms1child").change(function () {
        $(this).find("option:selected").each(function () {
            if ($(this).attr("value") == "1") {
                $(".room1 .slctbx").addClass("child1").removeClass("child2");
                $(".room1 .txtbxhtl").addClass("child1").removeClass("child2");
                $(".room1 .dtepik").addClass("child1").removeClass("child2");
                $(".room1 .room1child1").addClass("child1").removeClass("child2").show(200);
                $(".room1 .room1child2").hide();
            }
            else if ($(this).attr("value") == "2") {
                $(".room1 .slctbx").addClass("child2").removeClass("child1");
                $(".room1 .txtbxhtl").addClass("child2").removeClass("child1");
                $(".room1 .dtepik").addClass("child2").removeClass("child1");
                $(".room1 .room1child1").addClass("child2").show(200);
                $(".room1 .room1child2").addClass("child2").show(200);
            }
            else {
                $(".room1 .slctbx").removeClass("child1 child2");
                $(".room1 .txtbxhtl").removeClass("child1 child2");
                $(".room1 .dtepik").removeClass("child1 child2");
                $(".room1 .room1child1").hide();
                $(".room1 .room1child2").hide();
            }
        });
    }).change();

    $("select#rooms2child").change(function () {
        $(this).find("option:selected").each(function () {
            if ($(this).attr("value") == "1") {
                $(".room2 .room2child1").show(200);
                $(".room2 .room2child2").hide();
            }
            else if ($(this).attr("value") == "2") {
                $(".room2 .room2child1").show(200);
                $(".room2 .room2child2").show(200);
            }
            else {
                $(".room2 .room2child1").hide(200);
                $(".room2 .room2child2").hide(200);
            }
        });
    }).change();

    $("select#rooms3child").change(function () {
        $(this).find("option:selected").each(function () {
            if ($(this).attr("value") == "1") {
                $(".room3 .room3child1").show(200);
                $(".room3 .room3child2").hide();
            }
            else if ($(this).attr("value") == "2") {
                $(".room3 .room3child1").show(200);
                $(".room3 .room3child2").show(200);
            }
            else {
                $(".room3 .room3child1").hide(200);
                $(".room3 .room3child2").hide(200);
            }
        });
    }).change();

    $("select#rooms4child").change(function () {
        $(this).find("option:selected").each(function () {
            if ($(this).attr("value") == "1") {
                $(".room4 .room4child1").show(200);
                $(".room4 .room4child2").hide();
            }
            else if ($(this).attr("value") == "2") {
                $(".room4 .room4child1").show(200);
                $(".room4 .room4child2").show(200);
            }
            else {
                $(".room4 .room4child1").hide(200);
                $(".room4 .room4child2").hide(200);
            }
        });
    }).change();


    $("select#rooms5child").change(function () {
        $(this).find("option:selected").each(function () {
            if ($(this).attr("value") == "1") {
                $(".room5 .room5child1").show(200);
                $(".room5 .room5child2").hide();
            }
            else if ($(this).attr("value") == "2") {
                $(".room5 .room5child1").show(200);
                $(".room5 .room5child2").show(200);
            }
            else {
                $(".room5 .room5child1").hide(200);
                $(".room5 .room5child2").hide(200);
            }
        });
    }).change();



    $("select#rooms6child").change(function () {
        $(this).find("option:selected").each(function () {
            if ($(this).attr("value") == "1") {
                $(".room6 .room6child1").show(200);
                $(".room6 .room6child2").hide();
            }
            else if ($(this).attr("value") == "2") {
                $(".room6 .room6child1").show(200);
                $(".room6 .room6child2").show(200);
            }
            else {
                $(".room6 .room6child1").hide(200);
                $(".room6 .room6child2").hide(200);
            }
        });
    }).change();


    $("select#rooms").change(function () {
       
        $(this).find("option:selected").each(function () {
            if ($(this).attr("value") == "2") {
                $(".room2").show();
                $(".room2.room3").hide();
                $(".room4").hide();
                $(".room5").hide();
                $(".room6").hide();
                $(".room1").addClass("fullwidth").removeClass("lesswidth");//.css({ 'width':'100%', 'padding-bottom':'15px' });
                //$(".stick").css({ 'margin-top':'-199px' });
            }
            else if ($(this).attr("value") == "3") {
                $(".room2").show();
                $(".room2.room3").show();
                $(".room4").hide();
                $(".room5").hide();
                $(".room6").hide();
                try {
                    $(".room1, .room2").addClass("fullwidth").removeClass("lesswidth");//.css({ 'width':'100%', 'padding-bottom':'15px' });
                    $(".room2").addClass("lesswidth").removeClass("fullwidth");//.css({ 'width':'87%', 'padding-bottom':'15px' });
                    $(".room2.room3").addClass("lesswidth".removeClass("fullwidth"));//.css({ 'width':'87%', 'padding-bottom':'0px' });
                    //$(".stick").css({ 'margin-top':'-277px' });
                }
                catch (err) { }
            }
            else if ($(this).attr("value") == "4") {
                $(".room2").show();
                $(".room2.room3").show();
                $(".room2.room4").show();
                $(".room2.room5").hide();
                $(".room2.room6").hide();
                try {
                    $(".room1, .room2").addClass("fullwidth").removeClass("lesswidth");
                    $(".room2").addClass("lesswidth").removeClass("fullwidth");
                    $(".room2.room3").addClass("lesswidth".removeClass("fullwidth"));
                    $(".room2.room4").addClass("lesswidth".removeClass("fullwidth"));
                }
                catch (err) { }
            }
            else if ($(this).attr("value") == "5") {
                $(".room2").show();
                $(".room2.room3").show();
                $(".room2.room4").show();
                $(".room2.room5").show();
                $(".room2.room6").hide();
                try {
                    $(".room1, .room2").addClass("fullwidth").removeClass("lesswidth");
                    $(".room2").addClass("lesswidth").removeClass("fullwidth");
                    $(".room2.room3").addClass("lesswidth".removeClass("fullwidth"));
                    $(".room2.room4").addClass("lesswidth".removeClass("fullwidth"));
                    $(".room2.room5").addClass("lesswidth".removeClass("fullwidth"));
                }
                catch (err) { }
            }
            else if ($(this).attr("value") == "6") {
                $(".room2").show();
                $(".room2.room3").show();
                $(".room2.room4").show();
                $(".room2.room5").show();
                $(".room2.room6").show();
                try {
                    $(".room1, .room2").addClass("fullwidth").removeClass("lesswidth");
                    $(".room2").addClass("lesswidth").removeClass("fullwidth");
                    $(".room2.room3").addClass("lesswidth".removeClass("fullwidth"));
                    $(".room2.room4").addClass("lesswidth".removeClass("fullwidth"));
                    $(".room2.room5").addClass("lesswidth".removeClass("fullwidth"));
                    $(".room2.room6").addClass("lesswidth".removeClass("fullwidth"));
                }
                catch (err) { }
            }
            else {
                $(".room2").hide();
                $(".room3").hide();
                $(".room4").hide();
                $(".room5").hide();
                $(".room6").hide();
                $(".room1").addClass("lesswidth").removeClass("fullwidth");//.css({ 'width':'90%', 'padding-bottom':'0px' });
                //$(".stick").css({ 'margin-top':'-120px' });
            }
        });
    }).change();
    //---------------------Bind Last Search Control in Search Engine-----------------------

    // BindLastSearchInHotelsControl();


});

$("#AHotel_Search").click(function (event) {
    // $.get("../handlers/hotelremovesession.ashx", function (response) {
   
    var hostName = $("#hdnhostName").val(); //window.location.host;
    var searchfrom = $("#hotelsearchplace").val();
    var hotelCityAirport = $("#hotelCityAirport").val();
    var hotelCheckIn = $("#hotelCheckIn").val();
    var hotelCheckOut = $("#hotelCheckOut").val();
    if (hotelCityAirport == "") {
        alert("Hotel City or Airport field is blank!");
        $("#hotelCityAirport").focus();
    }

    else if (hotelCheckIn == "") {
        alert("Hotel CheckIn field is blank!");
        $("#hotelCheckIn").focus();
        event.preventDefault();
    }

    else if (hotelCheckOut == "") {
        alert("Hotel CheckOut field is blank!");
        $("#hotelCheckOut").focus();
    }
    else if ($("#rooms1adult").val() == "") {
        alert("Kindly select adult!");
        $("#rooms1adult").focus();
    }

        //else if ($("#rooms1child").val() == "") {
        //    alert("Kindly select adult!");
        //    $("#rooms1child").focus();
        //}

    else {

        $("#hotelarea").text(hotelCityAirport);
        $("#hotelchkhin").text(hotelCheckIn);
        $("#hotelchkout").text(hotelCheckOut);



        //$('#AHotel_Search').addClass('loading');
        $('#hotelloder').css("display", "block");
        $('#AHotel_Search').text('Please wait...');
        var noofroom = $("#rooms").val();
        var searchrequest;
        searchrequest = hotelCityAirport + "|" + hotelCheckIn + "|" + hotelCheckOut + "~";


        if (noofroom == "1") {
            //searchrequest += noofroom + "|";
            searchrequest += $("#rooms1adult").val() + "|";
            var rooms1child = $("#rooms1child").val();

            if (rooms1child == "0") {
                searchrequest += rooms1child + "|0|0|";
            }
            else if (rooms1child == "1") {
                searchrequest += rooms1child + "|" + $("#rooms1childage1").val() + "|0|";
            }
            else if (rooms1child == "2") {
                searchrequest += rooms1child + "|" + $("#rooms1childage1").val() + "|" + $("#rooms1childage2").val();
            }
        }
        else if (noofroom == "2") {
            //searchrequest += noofroom + "|"; 
            searchrequest += $("#rooms1adult").val() + "|";
            var rooms1child = $("#rooms1child").val();
            var rooms2child = $("#rooms2child").val();

            if (rooms1child == "0") {
                searchrequest += rooms1child + "|0|0|";
            }
            else if (rooms1child == "1") {
                searchrequest += rooms1child + "|" + $("#rooms1childage1").val() + "|0|";
            }
            else if (rooms1child == "2") {
                searchrequest += rooms1child + "|" + $("#rooms1childage1").val() + "|" + $("#rooms1childage2").val();
            }
            searchrequest += "~" + $("#rooms2adult").val() + "|";
            if (rooms2child == "0") {
                searchrequest += rooms2child + "|0|0|";
            }
            else if (rooms2child == "1") {
                searchrequest += rooms2child + "|" + $("#rooms2childage1").val() + "|0|";
            }
            else if (rooms2child == "2") {
                searchrequest += rooms2child + "|" + $("#rooms2childage1").val() + "|" + $("#rooms2childage2").val();
            }

        }
        else if (noofroom == "3") {
            //searchrequest += noofroom + "|";
            searchrequest += $("#rooms1adult").val() + "|";
            var rooms1child = $("#rooms1child").val();
            var rooms2child = $("#rooms2child").val();
            var rooms3child = $("#rooms3child").val();

            if (rooms1child == "0") {
                searchrequest += rooms1child + "|0|0|";
            }
            else if (rooms1child == "1") {
                searchrequest += rooms1child + "|" + $("#rooms1childage1").val() + "|0|";
            }
            else if (rooms1child == "2") {
                searchrequest += rooms1child + "|" + $("#rooms1childage1").val() + "|" + $("#rooms1childage2").val();
            }
            searchrequest += "~" + $("#rooms2adult").val() + "|";
            if (rooms2child == "0") {
                searchrequest += rooms2child + "|0|0|";
            }
            else if (rooms2child == "1") {
                searchrequest += rooms2child + "|" + $("#rooms2childage1").val() + "|0|";
            }
            else if (rooms2child == "2") {
                searchrequest += rooms2child + "|" + $("#rooms2childage1").val() + "|" + $("#rooms2childage2").val();
            }
            searchrequest += "~" + $("#rooms3adult").val() + "|";
            if (rooms3child == "0") {
                searchrequest += rooms3child + "|0|0|";
            }
            else if (rooms3child == "1") {
                searchrequest += rooms3child + "|" + $("#rooms3childage1").val() + "|0|";
            }
            else if (rooms3child == "2") {
                searchrequest += rooms3child + "|" + $("#rooms3childage1").val() + "|" + $("#rooms3childage2").val();
            }
        }      
        else if (noofroom == "4") {
            //searchrequest += noofroom + "|";
            searchrequest += $("#rooms1adult").val() + "|";
            var rooms1child = $("#rooms1child").val();
            var rooms2child = $("#rooms2child").val();
            var rooms3child = $("#rooms3child").val();
            var rooms4child = $("#rooms4child").val();

            if (rooms1child == "0") {
                searchrequest += rooms1child + "|0|0|";
            }
            else if (rooms1child == "1") {
                searchrequest += rooms1child + "|" + $("#rooms1childage1").val() + "|0|";
            }
            else if (rooms1child == "2") {
                searchrequest += rooms1child + "|" + $("#rooms1childage1").val() + "|" + $("#rooms1childage2").val();
            }
            searchrequest += "~" + $("#rooms2adult").val() + "|";
            if (rooms2child == "0") {
                searchrequest += rooms2child + "|0|0|";
            }
            else if (rooms2child == "1") {
                searchrequest += rooms2child + "|" + $("#rooms2childage1").val() + "|0|";
            }
            else if (rooms2child == "2") {
                searchrequest += rooms2child + "|" + $("#rooms2childage1").val() + "|" + $("#rooms2childage2").val();
            }
            searchrequest += "~" + $("#rooms3adult").val() + "|";
            if (rooms3child == "0") {
                searchrequest += rooms3child + "|0|0|";
            }
            else if (rooms3child == "1") {
                searchrequest += rooms3child + "|" + $("#rooms3childage1").val() + "|0|";
            }
            else if (rooms3child == "2") {
                searchrequest += rooms3child + "|" + $("#rooms3childage1").val() + "|" + $("#rooms3childage2").val();
            }
            searchrequest += "~" + $("#rooms4adult").val() + "|";
            if (rooms4child == "0") {
                searchrequest += rooms4child + "|0|0|";
            }
            else if (rooms4child == "1") {
                searchrequest += rooms4child + "|" + $("#rooms4childage1").val() + "|0|";
            }
            else if (rooms4child == "2") {
                searchrequest += rooms4child + "|" + $("#rooms4childage1").val() + "|" + $("#rooms4childage2").val();
            }

        }
        else if (noofroom == "5") {
            //searchrequest += noofroom + "|";
            searchrequest += $("#rooms1adult").val() + "|";
            var rooms1child = $("#rooms1child").val();
            var rooms2child = $("#rooms2child").val();
            var rooms3child = $("#rooms3child").val();
            var rooms4child = $("#rooms4child").val();
            var rooms5child = $("#rooms5child").val();
            if (rooms1child == "0") {
                searchrequest += rooms1child + "|0|0|";
            }
            else if (rooms1child == "1") {
                searchrequest += rooms1child + "|" + $("#rooms1childage1").val() + "|0|";
            }
            else if (rooms1child == "2") {
                searchrequest += rooms1child + "|" + $("#rooms1childage1").val() + "|" + $("#rooms1childage2").val();
            }
            searchrequest += "~" + $("#rooms2adult").val() + "|";
            if (rooms2child == "0") {
                searchrequest += rooms2child + "|0|0|";
            }
            else if (rooms2child == "1") {
                searchrequest += rooms2child + "|" + $("#rooms2childage1").val() + "|0|";
            }
            else if (rooms2child == "2") {
                searchrequest += rooms2child + "|" + $("#rooms2childage1").val() + "|" + $("#rooms2childage2").val();
            }
            searchrequest += "~" + $("#rooms3adult").val() + "|";
            if (rooms3child == "0") {
                searchrequest += rooms3child + "|0|0|";
            }
            else if (rooms3child == "1") {
                searchrequest += rooms3child + "|" + $("#rooms3childage1").val() + "|0|";
            }
            else if (rooms3child == "2") {
                searchrequest += rooms3child + "|" + $("#rooms3childage1").val() + "|" + $("#rooms3childage2").val();
            }
            searchrequest += "~" + $("#rooms4adult").val() + "|";
            if (rooms4child == "0") {
                searchrequest += rooms4child + "|0|0|";
            }
            else if (rooms4child == "1") {
                searchrequest += rooms4child + "|" + $("#rooms4childage1").val() + "|0|";
            }
            else if (rooms4child == "2") {
                searchrequest += rooms4child + "|" + $("#rooms4childage1").val() + "|" + $("#rooms4childage2").val();
            }
            searchrequest += "~" + $("#rooms5adult").val() + "|";
            if (rooms5child == "0") {
                searchrequest += rooms5child + "|0|0|";
            }
            else if (rooms5child == "1") {
                searchrequest += rooms5child + "|" + $("#rooms5childage1").val() + "|0|";
            }
            else if (rooms5child == "2") {
                searchrequest += rooms5child + "|" + $("#rooms5childage1").val() + "|" + $("#rooms5childage2").val();
            }
        }
        else if (noofroom == "6") {
            //searchrequest += noofroom + "|";
            searchrequest += $("#rooms1adult").val() + "|";
            var rooms1child = $("#rooms1child").val();
            var rooms2child = $("#rooms2child").val();
            var rooms3child = $("#rooms3child").val();
            var rooms4child = $("#rooms4child").val();
            var rooms5child = $("#rooms5child").val();
            var rooms6child = $("#rooms6child").val();
            if (rooms1child == "0") {
                searchrequest += rooms1child + "|0|0|";
            }
            else if (rooms1child == "1") {
                searchrequest += rooms1child + "|" + $("#rooms1childage1").val() + "|0|";
            }
            else if (rooms1child == "2") {
                searchrequest += rooms1child + "|" + $("#rooms1childage1").val() + "|" + $("#rooms1childage2").val();
            }
            searchrequest += "~" + $("#rooms2adult").val() + "|";
            if (rooms2child == "0") {
                searchrequest += rooms2child + "|0|0|";
            }
            else if (rooms2child == "1") {
                searchrequest += rooms2child + "|" + $("#rooms2childage1").val() + "|0|";
            }
            else if (rooms2child == "2") {
                searchrequest += rooms2child + "|" + $("#rooms2childage1").val() + "|" + $("#rooms2childage2").val();
            }
            searchrequest += "~" + $("#rooms3adult").val() + "|";
            if (rooms3child == "0") {
                searchrequest += rooms3child + "|0|0|";
            }
            else if (rooms3child == "1") {
                searchrequest += rooms3child + "|" + $("#rooms3childage1").val() + "|0|";
            }
            else if (rooms3child == "2") {
                searchrequest += rooms3child + "|" + $("#rooms3childage1").val() + "|" + $("#rooms3childage2").val();
            }

            searchrequest += "~" + $("#rooms4adult").val() + "|";
            if (rooms4child == "0") {
                searchrequest += rooms4child + "|0|0|";
            }
            else if (rooms4child == "1") {
                searchrequest += rooms4child + "|" + $("#rooms4childage1").val() + "|0|";
            }
            else if (rooms4child == "2") {
                searchrequest += rooms4child + "|" + $("#rooms4childage1").val() + "|" + $("#rooms4childage2").val();
            }
            searchrequest += "~" + $("#rooms5adult").val() + "|";
            if (rooms5child == "0") {
                searchrequest += rooms5child + "|0|0|";
            }
            else if (rooms5child == "1") {
                searchrequest += rooms5child + "|" + $("#rooms5childage1").val() + "|0|";
            }
            else if (rooms5child == "2") {
                searchrequest += rooms5child + "|" + $("#rooms5childage1").val() + "|" + $("#rooms5childage2").val();
            }

            searchrequest += "~" + $("#rooms6adult").val() + "|";
            if (rooms6child == "0") {
                searchrequest += rooms6child + "|0|0|";
            }
            else if (rooms6child == "1") {
                searchrequest += rooms6child + "|" + $("#rooms6childage1").val() + "|0|";
            }
            else if (rooms6child == "2") {
                searchrequest += rooms6child + "|" + $("#rooms6childage1").val() + "|" + $("#rooms6childage2").val();
            }
        }

        searchrequest = searchrequest.replace('&', '%26');
        //var searchtype = "";
       
        var params = "";
        params = [{
            "_SearchRequest": searchrequest, "_No_Of_Rooms": noofroom, "_Searchtype": "", "_Place": searchfrom
        }];
        $.ajax({
            type: "POST",
            url: "//" + hostName + "/HotelLandingpage.aspx/GetHotelDetail",
            data: "{'objReqPsq':" + JSON.stringify(params) + "}",
            contentType: "application/json; charset=utf-8", //Set Content-Type
            dataType: "json", // Set return Data Type
            cache: false,
            success: function (result) {
                var hostName = $("#hdnhostName").val(); //window.location.host;
                var obj = result.d;
                if (obj != "") {
                    location.href = "//" + hostName + "/Hotel_Search_List.aspx?s=" + searchrequest + "&r=" + noofroom;
                }
                else {
                    $('#hotelloder').css("display", "none");
                    alert("No data found.please try again");
                    location.href = "//" + hostName + "/index.aspx";
                }
            },
            error: function (xhr, msg, e) {
               
                $('#hotelloder').css("display", "none");
                alert("" + msg + "  " + e + " " + xhr);
                location.href = "//" + hostName + "/index.aspx";
            }
        });
        //var url = "dashboard.aspx";

        //var url = "Hotel_Search_List.aspx?s=" + searchrequest.replace('&', '%26') + "&r=" + noofroom;
        //window.location.href = url;

        //var url = "hotel-listing.aspx";
        //    window.location.href = url;
    }

    // });

});





function drawArrow(input) {
    var $input = $(input),
    widget = $input.datepicker('widget'), direction;

    setTimeout(function () {
        console.log($input.offset());
        console.log(widget.offset());
        var inputOffset = $input.offset(),
            widgetOffset = widget.offset();

        direction = inputOffset.top > widgetOffset.top ? 'down' : 'up';

        widget.addClass('direction-' + direction);

        var arrow = widget.find('.datepicker-arrow');

        if (!arrow.length) {
            arrow = $('<div class="datepicker-arrow"/>')
            .css({
                width: 0,
                height: 0,
                borderStyle: 'solid',
                margin: '0 10px',
                position: 'absolute',
            }).appendTo(widget);
        }

        arrow.css({
            left: inputOffset.left - widgetOffset.left,
            borderColor: direction === 'up' ? 'transparent transparent #cb2026' : '#cb2026 transparent transparent',
            borderWidth: direction === 'up' ? '0 10px 10px 10px' : '10px 10px 0 10px',
            top: direction === 'up' ? '-10px' : 'auto',
            bottom: direction === 'up' ? 'auto' : '-10px'
        });


    }, 10);
}

$('#hotelCheckIn').datepicker({
    numberOfMonths: totMonthToShow,
    minDate: 0,
    maxDate: "+11M +26D",
    dateFormat: 'dd-mm-yy',
    beforeShow: function (input, inst) {
        //
        drawArrow(input);
        setTimeout(function () {
            $('.ui-datepicker').css('z-index', 99999999999999);
        }, 0);
    },
    onSelect: function (selectedDate) {
       
        var t1 = selectedDate;
        var dateSplitChar = "-";
        var dateArray = t1.split(dateSplitChar);
        var yearString = dateArray[2].toString();
        var dayString = dateArray[0].toString();
        var monthString = dateArray[1].toString();
        var Date1 = monthString + '/' + dayString + '/' + yearString;
        var d1 = new Date(new Date(Date1).getTime() + 30 * 60 * 60 * 1000);
        $("#hotelCheckOut").datepicker("option", "minDate", d1.getDate() + '-' + (d1.getMonth() + 1) + '-' + d1.getFullYear());


    },
    onClose: function (selectedDate) {
       
        var t1 = selectedDate;
        var dateSplitChar = "-";
        try {
            var dateArray = t1.split(dateSplitChar);
            var yearString = dateArray[2].toString();
            var dayString = dateArray[0].toString();
            var monthString = dateArray[1].toString();
            var Date1 = monthString + '/' + dayString + '/' + yearString;
        }
        catch (ex) { }
        var d1 = new Date(new Date(Date1).getTime() + 30 * 60 * 60 * 1000);
        $("#hotelCheckOut").datepicker("option", "minDate", d1.getDate() + '-' + (d1.getMonth() + 1) + '-' + d1.getFullYear());

        $("#hotelCheckOut").focus();
    }
});
$('#hotelCheckOut').datepicker('widget').wrap('<div class="datepicker-custom"/>');

$("#hotelCheckOut").datepicker({
    numberOfMonths: totMonthToShow,
    minDate: 0,
    maxDate: "+11M +26D",
    dateFormat: 'dd-mm-yy',
    beforeShow: function (input, inst) {
        //
        drawArrow(input);
        setTimeout(function () {
            $('.ui-datepicker').css('z-index', 99999999999999);
        }, 0);
    },
    onSelect: function (selectedDate) {
       
        if ($('#hotelCheckIn').val() >= selectedDate) {
           
            var t1 = selectedDate;
            var dateSplitChar = "-";
            var dateArray = t1.split(dateSplitChar);
            var yearString = dateArray[2].toString();
            var dayString = dateArray[0].toString();
            var monthString = dateArray[1].toString();
            var Date1 = monthString + '/' + dayString + '/' + yearString;
            //var d1 = new Date(new Date(Date1).getTime() + 30 * 60 * 60 * 1000);
            var d1 = new Date(new Date(Date1).getTime());
            // $("#hotelCheckOut").datepicker("option", "minDate", d1.getDate() + '-' + (d1.getMonth() + 1) + '-' + d1.getFullYear());
        }
        else {
        }
    }
    ,
    onClose: function (selectedDate) {
        //$("#hotelCheckIn").datepicker("option", "maxDate", selectedDate);

    }
});



$(".datepkg").datepicker({
    numberOfMonths: 1,
    minDate: 0,
    maxDate: "+11M +26D",
    dateFormat: 'dd-mm-yy',
    beforeShow: function (input, inst) {
        //
        drawArrow(input);
        setTimeout(function () {
            $('.ui-datepicker').css('z-index', 99999999999999);
        }, 0);
    },
    onSelect: function (selectedDate) {
       
    }
    ,
    onClose: function (selectedDate) {
       

    }
});



$('.stkeng').bind('inview', function (event, visible) {

    if (visible == true) {
        // element is now visible in the viewport
        $('.hotel_search_engine_cnt').addClass('stick').removeClass('nostick');
        // alert('added... pleasecheck body')
        // console.log($(this).attr('stick'));
    } else {

        var window_top = $(window).scrollTop();
        var div_top = $('.stkeng').offset().top;
        if (window_top > div_top) {
            $('.hotel_search_engine_cnt').addClass('stick').removeClass('nostick');
        } else {
            $('.hotel_search_engine_cnt').removeClass('stick').addClass('nostick');
        }
        // $('.hotel_search_engine_cnt').removeClass('stick');
        //alert('removed... pleasecheck body');
    }
});
$('.stkeng').trigger('inview');



function BindLastSearchInHotelsControl() {
   
    if (localStorage.getItem('HotelSearchStorage') != null) {
        var resSearch = JSON.parse(localStorage.getItem('HotelSearchStorage'));
        var stypr = resSearch.stype;
        var r = resSearch.Rooms;
        $('#hotelCityAirport').val(resSearch.AllParamConfig[0].split('|')[0]);
        $('#hotelCheckIn').val(resSearch.AllParamConfig[0].split('|')[1]);
        $('#hotelCheckOut').val(resSearch.AllParamConfig[0].split('|')[2]);
        $("#HiddenrequestType").val(stypr);
        $('#rooms').val(r);

        for (var i = 1; i <= r; i++) {
            if (i !== 1)
            { $('#room' + i + '').show(); }
            var RoomsAllValue = resSearch.AllParamConfig[i].split('|');
            $('#rooms' + i + 'adult').val(RoomsAllValue[0])
            $('#rooms' + i + 'child').val(RoomsAllValue[1])
            if (RoomsAllValue[1] !== '0') {
                for (c = 1; c <= RoomsAllValue[1]; c++) {
                    $('.room' + i + 'child' + c + '').show();
                    if (c == 1)
                    { $('#rooms' + i + 'childage' + c + '').val(RoomsAllValue[2]); } else { $('#rooms' + i + 'childage' + c + '').val(RoomsAllValue[3]); }
                }
            }
            if (RoomsAllValue[4] !== '0') {
                $('#rooms' + i + 'Infant').val(RoomsAllValue[4]);
            }
        }
    }
}