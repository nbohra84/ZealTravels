/*
 * Author: Abdullah A Almsaeed
 * Date: 4 Jan 2014
 * Description:
 *      This is a demo file used only for the main dashboard (index.html)
 **/

import Highcharts from "./highcharts";

jQuery(document).ready(function ($) {
    //;
    var tktdetails = document.getElementById('DashboardChartXML').value;
    var xmlDataObj = ParseXML(tktdetails);
    var DailyBookigList = xmlDataObj.getElementsByTagName("DashboardChartResponse");
    var arr1 = [];
    for (var i = 0; i < DailyBookigList.length; i++) {
        //;
        var arr3 = [];
        var CarrierCode = xmlDataObj.getElementsByTagName('CarrierCode')[i].firstChild.data;
        var CarrierName = xmlDataObj.getElementsByTagName('CarrierName')[i].firstChild.data;
        var NoOfPassenger = xmlDataObj.getElementsByTagName('NoOfPassenger')[i].firstChild.data;
        var NoOfBooking = parseInt(xmlDataObj.getElementsByTagName('NoOfBookings')[i].firstChild.data);
        var totalsale = 0;
        totalsale = totalsale + parseInt(xmlDataObj.getElementsByTagName('TotalFare')[i].firstChild.data);
        var nm = CarrierCode + "(" + CarrierName + ")";
        var lnm = "No Of Pax :  " + NoOfPassenger;
        //;
        var item1 = { y: NoOfBooking, name: nm };
        arr3.push(item1);
        var item = { data: arr3, name: lnm };
        //;
        arr1.push(item);
    }
    if (totalsale != "" && totalsale != undefined) {
        document.getElementById("spntotalsale").innerHTML = totalsale;
        document.getElementById("spanlavelup").innerHTML = totalsale;
    }
    else {
        document.getElementById("spntotalsale").innerHTML = 0;
        document.getElementById("spanlavelup").innerHTML = 0;
    }

    var arr2 = [];
    var chart = Highcharts.chart('chartArr', {
        chart: {
            type: 'column'
        },
        legend: {
            align: 'right',
            verticalAlign: 'middle',
            layout: 'vertical'
        },
        xAxis: {
            title: {
                text: 'Airline'
            },
        },
        yAxis: {
            title: {
                text: 'Booking In %'
            },
            tickInterval: 2.5, 
            min: 0
        },
        plotOptions: {
            column: {
                allowPointSelect: true,
                cursor: 'pointer',
                dataLabels: {
                    enabled: true,
                    format: '<b>{point.name}</b>: {point.y} %',
                    style: {
                        color: (Highcharts.theme && Highcharts.theme.contrastTextColor) || 'black'
                    }
                }
            }
        },
        series: arr1,
        responsive: {
            rules: [{
                condition: {
                    maxWidth: 500
                },
                chartOptions: {
                    legend: {
                        align: 'center',
                        verticalAlign: 'bottom',
                        layout: 'horizontal'
                    }
                }
            }]
        }
    });
});

$(function () {

    "use strict";

    //Make the dashboard widgets sortable Using jquery UI
    $(".connectedSortable").sortable({
        placeholder: "sort-highlight",
        connectWith: ".connectedSortable",
        handle: ".box-header, .nav-tabs",
        forcePlaceholderSize: true,
        zIndex: 999999
    });
    $(".connectedSortable .box-header, .connectedSortable .nav-tabs-custom").css("cursor", "move");

    //jQuery UI sortable for the todo list
    $(".todo-list").sortable({
        placeholder: "sort-highlight",
        handle: ".handle",
        forcePlaceholderSize: true,
        zIndex: 999999
    });

    //bootstrap WYSIHTML5 - text editor



    /* jQueryKnob */
    $(".knob").knob();

    //jvectormap data
    var visitorsData = {
        "US": 398, //USA
        "SA": 400, //Saudi Arabia
        "CA": 1000, //Canada
        "DE": 500, //Germany
        "FR": 760, //France
        "CN": 300, //China
        "AU": 700, //Australia
        "BR": 600, //Brazil
        "IN": 800, //India
        "GB": 320, //Great Britain
        "RU": 3000 //Russia
    };
    //World map by jvectormap
    $('#world-map').vectorMap({
        map: 'world_mill_en',
        backgroundColor: "transparent",
        regionStyle: {
            initial: {
                fill: '#e4e4e4',
                "fill-opacity": 1,
                stroke: 'none',
                "stroke-width": 0,
                "stroke-opacity": 1
            }
        },
        series: {
            regions: [{
                values: visitorsData,
                scale: ["#92c1dc", "#ebf4f9"],
                normalizeFunction: 'polynomial'
            }]
        },
        onRegionLabelShow: function (e, el, code) {
            if (typeof visitorsData[code] != "undefined")
                el.html(el.html() + ': ' + visitorsData[code] + ' new visitors');
        }
    });

    //Sparkline charts
    var myvalues = [1000, 1200, 920, 927, 931, 1027, 819, 930, 1021];
    $('#sparkline-1').sparkline(myvalues, {
        type: 'line',
        lineColor: '#92c1dc',
        fillColor: "#ebf4f9",
        height: '50',
        width: '80'
    });
    myvalues = [515, 519, 520, 522, 652, 810, 370, 627, 319, 630, 921];
    $('#sparkline-2').sparkline(myvalues, {
        type: 'line',
        lineColor: '#92c1dc',
        fillColor: "#ebf4f9",
        height: '50',
        width: '80'
    });
    myvalues = [15, 19, 20, 22, 33, 27, 31, 27, 19, 30, 21];
    $('#sparkline-3').sparkline(myvalues, {
        type: 'line',
        lineColor: '#92c1dc',
        fillColor: "#ebf4f9",
        height: '50',
        width: '80'
    });

    //The Calender
    $("#calendar").datepicker();

    //SLIMSCROLL FOR CHAT WIDGET
    $('#chat-box').slimScroll({
        height: '250px'
    });

    /* Morris.js Charts */
    // Sales chart
    var area = new Morris.Area({
        element: 'revenue-chart',
        resize: true,
        data: [
          { y: '2011 Q1', item1: 2666, item2: 2666 },
          { y: '2011 Q2', item1: 2778, item2: 2294 },
          { y: '2011 Q3', item1: 4912, item2: 1969 },
          { y: '2011 Q4', item1: 3767, item2: 3597 },
          { y: '2012 Q1', item1: 6810, item2: 1914 },
          { y: '2012 Q2', item1: 5670, item2: 4293 },
          { y: '2012 Q3', item1: 4820, item2: 3795 },
          { y: '2012 Q4', item1: 15073, item2: 5967 },
          { y: '2013 Q1', item1: 10687, item2: 4460 },
          { y: '2013 Q2', item1: 8432, item2: 5713 }
        ],
        xkey: 'y',
        ykeys: ['item1', 'item2'],
        labels: ['Item 1', 'Item 2'],
        lineColors: ['#a0d0e0', '#3c8dbc'],
        hideHover: 'auto'
    });
    var line = new Morris.Line({
        element: 'line-chart',
        resize: true,
        data: [
          { y: '2011 Q1', item1: 2666 },
          { y: '2011 Q2', item1: 2778 },
          { y: '2011 Q3', item1: 4912 },
          { y: '2011 Q4', item1: 3767 },
          { y: '2012 Q1', item1: 6810 },
          { y: '2012 Q2', item1: 5670 },
          { y: '2012 Q3', item1: 4820 },
          { y: '2012 Q4', item1: 15073 },
          { y: '2013 Q1', item1: 10687 },
          { y: '2013 Q2', item1: 8432 }
        ],
        xkey: 'y',
        ykeys: ['item1'],
        labels: ['Item 1'],
        lineColors: ['#efefef'],
        lineWidth: 2,
        hideHover: 'auto',
        gridTextColor: "#fff",
        gridStrokeWidth: 0.4,
        pointSize: 4,
        pointStrokeColors: ["#efefef"],
        gridLineColor: "#efefef",
        gridTextFamily: "Open Sans",
        gridTextSize: 10
    });

    //Donut Chart
    var donut = new Morris.Donut({
        element: 'sales-chart',
        resize: true,
        colors: ["#3c8dbc", "#f56954", "#00a65a"],
        data: [
          { label: "Download Sales", value: 12 },
          { label: "In-Store Sales", value: 30 },
          { label: "Mail-Order Sales", value: 20 }
        ],
        hideHover: 'auto'
    });

    //Fix for charts under tabs
    $('.box ul.nav a').on('shown.bs.tab', function () {
        area.redraw();
        donut.redraw();
        line.redraw();
    });

    /* The todo list plugin */
    $(".todo-list").todolist({
        onCheck: function (ele) {
            window.console.log("The element has been checked");
            return ele;
        },
        onUncheck: function (ele) {
            window.console.log("The element has been unchecked");
            return ele;
        }
    });

});

jQuery(document).ready(function ($) {
    //debugger;
var tktdetails = document.getElementById('DashboardChartXML').value;
var xmlDataObj = ParseXML(tktdetails);
var DailyBookigList = xmlDataObj.getElementsByTagName("DashboardChartResponse");
    var arr1 = [];
    for (var i = 0; i < DailyBookigList.length; i++) {
        //debugger;
        var arr3 = [];
        var CarrierCode = xmlDataObj.getElementsByTagName('CarrierCode')[i].firstChild.data;
        var CarrierName = xmlDataObj.getElementsByTagName('CarrierName')[i].firstChild.data;
        var NoOfPassenger = xmlDataObj.getElementsByTagName('NoOfPassenger')[i].firstChild.data;
        var NoOfBooking = parseInt(xmlDataObj.getElementsByTagName('NoOfBookings')[i].firstChild.data);
        var totalsale = 0;
        totalsale = totalsale + parseInt(xmlDataObj.getElementsByTagName('TotalFare')[i].firstChild.data);

        var nm = CarrierCode + "(" + CarrierName + ")";
        var lnm = "No Of Pax :  " + NoOfPassenger;
        //debugger;
        var item1 = { y: NoOfBooking, name: nm };
        arr3.push(item1);
        var item = { data: arr3, name: lnm };
        //debugger;
        arr1.push(item);
    }
    if (totalsale != "" && totalsale != undefined) {
        document.getElementById("spntotalsale").innerHTML = totalsale;
        document.getElementById("spanlavelup").innerHTML = totalsale;
    }
    else {
        document.getElementById("spntotalsale").innerHTML = 0;
        document.getElementById("spanlavelup").innerHTML = 0;
    }

    var arr2 = [];
    var chart = Highcharts.chart('container', {
        chart: {
            type: 'column'
        },
        legend: {
            align: 'right',
            verticalAlign: 'middle',
            layout: 'vertical'
        },
        xAxis: {
            title: {
                text: 'Airline'
            },
        },
        yAxis: {
            title: {
                text: 'Booking In %'
            }
        },
        plotOptions: {
            column: {
                allowPointSelect: true,
                cursor: 'pointer',
                dataLabels: {
                    enabled: true,
                    format: '<b>{point.name}</b>: {point.y} %',
                    style: {
                        color: (Highcharts.theme && Highcharts.theme.contrastTextColor) || 'black'
                    }
                }
            }
        },
        series: arr1,
        responsive: {
            rules: [{
                condition: {
                    maxWidth: 500
                },
                chartOptions: {
                    legend: {
                        align: 'center',
                        verticalAlign: 'bottom',
                        layout: 'horizontal'
                    }
                }
            }]
        }
    });
});

// function ParseXML(val) {
//     if (window.DOMParser) {
//         parser = new DOMParser();
//         xmlDoc = parser.parseFromString(val, "text/xml");
//     }
//     else // Internet Explorer
//     {
//         xmlDoc = new ActiveXObject("Microsoft.XMLDOM"); xmlDoc.loadXML(val);
//     }
//     return xmlDoc;
// }

function hideborder() {
    //debugger;
    $("#ContentPlaceHolder1_txtbrefpnr").removeClass("wrong");
}
function bookrefff(abc) {
    //debugger;
    var result = abc.accessKey;
    location.href = "BookingDetail_Air.aspx?BookingRef=" + btoa(result); 
}

function googleTranslateElementInit() {
  new google.translate.TranslateElement({pageLanguage: 'en'}, 'google_translate_element');
}


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

    function Closepopup(abc) {
        //;
        var ll = document.getElementById(abc.id);
        if ($(ll).is(':visible')) {
            // $(ll).hide();
            $(ll).css("display", "none");
        }
        else {
            //$(ll).modal('show');
            $(ll).css("display", "block");
        }
    }
    function showdetailcomp(abc) {
        //;
        var ll = document.getElementById(abc.id);
        if ($(ll).is(':visible')) {
            // $(ll).hide();
            $(ll).css("display", "none");
        }
        else {
            //$(ll).modal('show');
            $(ll).css("display", "block");
        }
    }
    function ParseXML(val) {
        if (window.DOMParser) {
            var parser = new DOMParser();
            var xmlDoc = parser.parseFromString(val, "text/xml");
        }
        else // Internet Explorer
        {
            xmlDoc = new ActiveXObject("Microsoft.XMLDOM"); xmlDoc.loadXML(val);
        }
        return xmlDoc;
    }
    function validatetext() {
        //;
        var updeml = document.getElementById("ContentPlaceHolder1_txtbrefpnr").value;
        if (updeml == null || updeml == "") {
            $("#ContentPlaceHolder1_txtbrefpnr").addClass("wrong");
            return false;
        }
    }
