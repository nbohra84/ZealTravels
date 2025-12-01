function BookNow() {
  var Arrivaltimeonward;
  var departtimereturn;
  var msg = sortF;
  var msg2 = sortFR;
  var ArrDateDescO;
  var DepDateDescR;
  for (var i = 0; i < msg.d.length; i++) {
    if (
      msg.d[i].FlightRefid == document.getElementById("HiddenRefOnward").value
    ) {
      SameFlightName = msg.d[i].FlightName;
      if (msg.d[i].connection == 1) {
        ArrDateDescO = msg.d[i].FlightArrDate1;
        Arrivaltimeonward = msg.d[i].FlightArrTime1;
      }
      if (msg.d[i].connection == 2) {
        ArrDateDescO = msg.d[i].FlightArrDate2;
        Arrivaltimeonward = msg.d[i].FlightArrTime2;
      }
      if (msg.d[i].connection == 3) {
        ArrDateDescO = msg.d[i].FlightArrDate3;
        Arrivaltimeonward = msg.d[i].FlightArrTime3;
      }
      break;
    }
  }
  for (var i = 0; i < msg2.d.length; i++) {
    if (
      msg2.d[i].FlightRefid == document.getElementById("HiddenRefReturn").value
    ) {
      SameFlightName2 = msg2.d[i].FlightName;
      departtimereturn = msg2.d[i].FlightDepTime1;
      DepDateDescR = msg2.d[i].FlightDepDate1;
      break;
    }
  }
  var d = ArrDateDescO.substr(ArrDateDescO.indexOf(",") + 1, 11);
  d = d.replace(/[^a-zA-Z0-9]/g, " ");
  var stDate = Date.parse(d);
  d = DepDateDescR.substr(DepDateDescR.indexOf(",") + 1, 11);
  d = d.replace(/[^a-zA-Z0-9]/g, " ");
  var edDate = Date.parse(d);
  if (parseInt(stDate) == parseInt(edDate)) {
    var tm = departtimereturn;
    var tm2 = Arrivaltimeonward;
    var tmdif = parseInt(tm) - parseInt(tm2);
    if (tmdif < 3) {
      alert(
        "Onward and return flights selection time difference is too short, please select at least 3 hours time difference!"
      );
      return false;
    } else {
      flightSelect();
    }
  } else if (parseInt(stDate) > parseInt(edDate)) {
    alert("Onward flights arrive next day");
    return false;
  } else {
    flightSelect();
  }
}
window.BookNow = BookNow;
function flightSelect() {
  jQuery("#divSelectFlight").html("");
  jQuery("#divSelectFlightmotfound").html("");
  $("#datashowConcernModifyouter").css("display", "Block");
  $("#FlightPopUp").css("display", "Block");
  var refidOnward = document.getElementById("HiddenRefOnward").value;
  var refidReturn = document.getElementById("HiddenRefReturn").value;
  var CompnyID = $("#hdncmpid").val();
  $.ajax({
    type: "POST",
    url: "/flight/SelectFlightR",
    data: JSON.stringify({ refidOnward: refidOnward,refidReturn: refidReturn, CompanyID: CompnyID }),
    contentType: "application/json; charset=utf-8",
    dataType: "json",
    success: function (msg) {
      $("#Panel1").show();
      if (msg.d[0].F_Status === true) {
        $("#FlightPopUp").css("display", "none");
        $("#divpopup").css("display", "Block");
        $("#MyTemplateSelectOnwardPopupFarerule")
          .tmpl(msg.d[0])
          .appendTo("#fareruleDivOwnward");
        $("#MyTemplateSelectRetuPopupFarerule")
          .tmpl(msg.d[1])
          .appendTo("#fareruleDivReturn");
        $("#TemplatedivSelectFlight")
          .tmpl(msg.d[0])
          .appendTo("#divSelectFlight");
        $("#TemplatedivSelectFlightR")
          .tmpl(msg.d[1])
          .appendTo("#divSelectFlight");
      } else {
        $("#FlightPopUp").css("display", "none");
        $("#divpopup").css("display", "Block");
        $(".panel_heightset.panel-body .fareruledivbox").css("display", "none");
        $("#TemplatedivSelectFlightNotfound")
          .tmpl(msg.d[0])
          .appendTo("#divSelectFlightmotfound");
        $("#<%=Panel1.ClientID%>").hide();
      }

      //if(msg.d[0].FareUpdateMsgChek==1)
      //{
      //  //  GetShow();
      //    $("#FlightPopUp").css("display", "none");
      //    $("#divpopup").css("display", "Block");
      //    //if ((screen.width==1024))
      //    //{
      //    //    $("#divpopup").animate({
      //    //        height:"auto",
      //    //        width:"100%"
      //    //    } , 2000);
      //    //}
      //    //else
      //    //{
      //    //    $("#divpopup").animate({
      //    //        height:"auto",
      //    //        width:"100%"
      //    //    } , 2000);
      //    //}
      //    $("#MyTemplateSelectOnwardPopupFarerule").tmpl(msg.d[0]).appendTo("#fareruleDivOwnward");
      //    $("#MyTemplateSelectRetuPopupFarerule").tmpl(msg.d[1]).appendTo("#fareruleDivReturn");
      //    $("#TemplatedivSelectFlight").tmpl(msg.d[0]).appendTo("#divSelectFlight");
      //    $("#TemplatedivSelectFlightR").tmpl(msg.d[1]).appendTo("#divSelectFlight");
      //}
      //else if(msg.d[0].FareUpdateMsgChek==0)
      //{
      //    $("#FlightPopUp").css("display", "none");
      //    $("#divpopup").css("display", "Block");
      //    //if ((screen.width==1024))
      //    //{
      //    //    $("#divpopup").animate({
      //    //        height:"auto",
      //    //        width:"100%"
      //    //    } , 2000);
      //    //}
      //    //else
      //    //{
      //    //    $("#divpopup").animate({
      //    //        height:"auto",
      //    //        width:"100%"
      //    //    } , 2000);
      //    //}
      //    $("#MyTemplateSelectOnwardPopupFarerule").tmpl(msg.d[0]).appendTo("#fareruleDivOwnward");
      //    $("#MyTemplateSelectRetuPopupFarerule").tmpl(msg.d[1]).appendTo("#fareruleDivReturn");
      //    $("#TemplatedivSelectFlight").tmpl(msg.d[0]).appendTo("#divSelectFlight");
      //    $("#TemplatedivSelectFlightR").tmpl(msg.d[1]).appendTo("#divSelectFlight");
      //}
      // <%--else if(msg.d[0].FareUpdateMsgChek==2)
      // {
      //    // GetShow();
      //     $("#FlightPopUp").css("display", "none");
      //     $("#divpopup").css("display", "Block");
      //     //if ((screen.width==1024))
      //     //{
      //     //    $("#divpopup").animate({
      //     //        height:"auto",
      //     //        width:"100%"
      //     //    } , 2000);
      //     //}
      //     //else
      //     //{
      //     //    $("#divpopup").animate({

      //     //        height:"auto",
      //     //        width:"100%",
      //     //    } , 2000);
      //     //}
      //     $("#TemplatedivSelectFlightNotfound").tmpl(msg.d[0]).appendTo("#divSelectFlightmotfound");
      //     $("#<%=Panel1.ClientID%>").hide();
      // }--%>
    },
    error: function (msg) {
      $("#FlightPopUp").css("display", "none");
      $("#datashowConcernModifyouter").css("display", "none");
    },
  });
}

function Closedivpopup() {
  //$("#divpopup").animate({

  //    height:"auto",
  //    width:"10px"
  //} , 2000);

  $("#datashowConcernModifyouter").css("display", "none");
  $("#divpopup").css("display", "none");
  $("#fareruleDiv").css("display", "none");
  var FlightRefid = $("#HiddenRefOnward").val();
  var FlightRefidR = $("#HiddenRefReturn").val();
  document.getElementById("FarerulesummeryPopup" + FlightRefid + "").innerHTML =
    "";
  document.getElementById(
    "FarerulesummeryPopupR" + FlightRefidR + ""
  ).innerHTML = "";
}
window.Closedivpopup = Closedivpopup;
//function YesToBook(FlightRefidOnward,FlightRefidReturn)
//{
//    var Arrivaltimeonward;
//    var departtimereturn;
//    var msg= sortF;
//    var msg2= sortFR;
//    var ArrDateDescO = document.getElementById("ArrDateDescO").value;
//    var d=ArrDateDescO.substr(ArrDateDescO.indexOf(",")+1,11);
//    d=d.replace(/[^a-zA-Z0-9]/g, " ");
//    var stDate =Date.parse(d);
//    var DepDateDescR = document.getElementById("DepDateDescR").value;
//    d=DepDateDescR.substr(DepDateDescR.indexOf(",")+1,11);
//    d=d.replace(/[^a-zA-Z0-9]/g, " ");
//    var edDate =Date.parse(d);
//    for (var i = 0; i < msg.d.length; i++) {
//        if (msg.d[i].FlightRefid == FlightRefidOnward) {
//            if (msg.d[i].connection == 1) {
//                ArrDateDescO= msg.d[i].FlightArrDate1;
//                Arrivaltimeonward= msg.d[i].FlightArrTime1;
//            }
//            if (msg.d[i].connection == 2) {
//                ArrDateDescO= msg.d[i].FlightArrDate2;
//                Arrivaltimeonward= msg.d[i].FlightArrTime2;
//            }
//            if (msg.d[i].connection == 3) {
//                ArrDateDescO= msg.d[i].FlightArrDate3;
//                Arrivaltimeonward= msg.d[i].FlightArrTime3;
//            }
//            break;
//        }
//    }
//    for (var i = 0; i < msg2.d.length; i++) {
//        if (msg2.d[i].FlightRefid == FlightRefidReturn) {
//            departtimereturn=msg2.d[i].FlightDepTime1;
//            DepDateDescR=msg2.d[i].FlightDepDate1;
//            break;
//        }
//    }
//    if(parseInt(stDate) == parseInt(edDate))
//    {
//        var tm =departtimereturn;
//        var tm2 =Arrivaltimeonward;
//        var tmdif=parseInt(tm)-parseInt(tm2);
//        if(tmdif < 3)
//        {
//            alert("Onward and return flights selection time difference is too short, please select at least 3 hours time difference!");
//            return false;
//        }
//        else
//        {
//            flightbook();
//        }
//    }
//    else if(parseInt(stDate) > parseInt(edDate))
//    {
//        alert("Onward flights arrive next day");
//        return false;
//    }
//    else
//    {
//        flightbook();
//    }
//}

//function flightbook()
//{
//    $.ajax({
//        type: "POST",
//        url: "k_one.aspx/Book",
//        data: "{}",
//        contentType: "application/json; charset=utf-8",
//        dataType: "json",
//        success: function (msg) {
//            if (msg.d[0].msgs == 1) {
//                var url = "GuestAndExistingLogin.aspx?AgentType=Guest&tabid=" + msg.d[0].tabid;
//                $(location).attr('href', url);
//            }
//            else if (msg.d[0].msgs == 2) {
//                var url = "FlightReview.aspx?tabid=" + msg.d[0].tabid;
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

$(window).load(function () {
  //showmodifystatus();
  SetValueInModifyControl();
  calculatenofpax();
});

$(document).ready(function () {
  calculatenofpax();
  window.onscroll = function () {
    myFunction();
  };

  var navbar = document.getElementById("navbar");
  var sticky = navbar.offsetTop;
  function myFunction() {
    if (window.pageYOffset >= sticky) {
      navbar.classList.add("sticky");
    } else {
      navbar.classList.remove("sticky");
    }
  }
  $("#top").hide();
  GetShow();
  showprogerss();
  $("#waitingload").css("display", "Block");
  $("#waitingloadbox").css("display", "Block");
  //         GetShowR();
  $("#top").click(function () {
    $("#top").fadeOut();
    window.scrollTo(0, 0);
  });
  $("#SpanModify").click(function () {
    $("#ModifySearchDomNew").show("slow");
    document.getElementById("modifypart").style.display = "block";
    $("#datashowConcernModifyouter").css("display", "block");
    $("#datashowConcernModifyouter").css("background-color", "Gray");
    window.scrollTo(0, 0);
  });
  $("#ModifySearchDomNewClose").click(function () {
    $("#ModifySearchDomNew").hide("slow");
    document.getElementById("modifypart").style.display = "none";
    $("#datashowConcernModifyouter").css("display", "none");
    $("#datashowConcernModifyouter").css("background-color", "transparent");
  });
  $("#FilterYourSearchPlus").click(function () {
    //$('#FilterYourSearchPlus').hide('fast');
    //$('#FilterYourSearchMinus').show('fast');
    $("#FilterYourSearchDiv").show();
    $("#calendarfiltui").show();
    //$('#coltop').css("height", "310px");
    //$('#container_bottomR').css("height", "240px");
  });

  //$('#FilterYourSearchMinus').click(function () {
  //    $('#FilterYourSearchPlus').show('fast');
  //    $('#FilterYourSearchMinus').hide('fast');
  //    //$('#coltop').css("height", "150px");
  //    $('#FilterYourSearchDiv').show('fast');
  //    //$('#container_bottomR').css("height", "40px");
  //});
  $(".enablec").click(function () {
    $("#datashowConcernModifyouter").css("display", "Block");
    $("#FlightPopUp").css("display", "block");
    //$("#modelpopupCalender").css("background-color", "Gray");
    //$('#modelpopupCalender').delay(1).fadeIn(400);
    //$('#modelpopupOUTERCalender').delay(1).fadeIn(400);
  });
});
function calculatenofpax() {
  var totalpax =
    parseInt($("#adult").val()) +
    parseInt($("#child").val()) +
    parseInt($("#infant").val());
  document.getElementById("travlvalue").textContent = totalpax;
  $("#travlvalue").trigger("click");
  $("#passengerbx").css("display", "none");
}
window.calculatenofpax = calculatenofpax;
$(window).scroll(function () {
  if ($(this).scrollTop() > 500) {
    $("#top").fadeIn();
  } else {
    $("#top").fadeOut();
  }
});
var sortF;
var sortFR;
var SortCriteria;
var flagSort = "N";
function GetShow() {
  var CompnyID = $("#hdncmpid").val();
  $.ajax({
    type: "POST",
    url: "/flight/ShowData",
    data: '{CompanyID:"' + CompnyID + '"}',
    contentType: "application/json; charset=utf-8",
    dataType: "json",
    success: function (msg) {
      if (msg.d.length === 0) {
        $("#flightNotFound").show();
        $("#roundresultmaindiv").hide();
        // $(".container.width1170px").hide();
      } else {
        sortF = null;
        sortF = msg;
        jQuery("#tempView").html("");
        SortCriteria = "N";
        flagSort = "N";
        var jsonResponse = msg;
        //iterating processed JSON response to get the single flight details
        var groupByFlightNumber = groupJson(jsonResponse);
        Object.keys(groupByFlightNumber).forEach(function (key) {
          $("#MyTemplate")
            .tmpl(groupByFlightNumber[key][0])
            .appendTo("#tempView");
          $("#MyTemplatePrice")
            .tmpl(groupByFlightNumber[key])
            .appendTo("#Price3-" + key);
          $("#MyTemplateDetail")
            .tmpl(groupByFlightNumber[key])
            .appendTo("#DR-" + key);
        });
        var inboundContent = document.querySelector("#inbound-content");
        inboundContent.classList.add("active");
        GetShowR();
    }
    },
    error: function (msg) {},
  });
}
function GetShowR() {
  var CompnyID = $("#hdncmpid").val();
  $.ajax({
    type: "POST",
    url: "/flight/ShowDataR",
    data: '{CompanyID:"' + CompnyID + '"}',
    contentType: "application/json; charset=utf-8",
    dataType: "json",
    success: function (msg) {
      sortFR = null;
      sortFR = msg;
      jQuery("#tempViewR").html("");
      var jsonResponse = sortFR;
      //iterating processed JSON response to get the single flight details
      var groupByFlightNumber = groupJson(jsonResponse);
      Object.keys(groupByFlightNumber).forEach(function (key) {
        $("#MyTemplateR")
          .tmpl(groupByFlightNumber[key][0])
          .appendTo("#tempViewR");
        $("#MyTemplatePriceR")
          .tmpl(groupByFlightNumber[key])
          .appendTo("#PriceT-" + key);
        $("#MyTemplateDetailR")
          .tmpl(groupByFlightNumber[key])
          .appendTo("#DRR-" + key);
      });

      selectFlightbind();
      GetMinMax();
      GetMinMaxDepartureTime();
      GetMinMaxArrivalTime();
      GetFlightPreferAirlines();
      GetFlightStops();
      $("#waitingload").css("display", "none");
      $("#waitingloadbox").css("display", "none");

      //                 GetFlightMatrix();

      var cmpid1 = document.getElementById("hdncmpid").value;
      if (cmpid1.indexOf("C-") > -1 || cmpid1 == "") {
        document.getElementById("chkdiscccfare").checked = true;
        $(".hidchk2").css({ display: "none" });
        $(".hidtdscus").css({ display: "none" });
      }
      chkunchk();
    },
    error: function (msg) {},
  });
}

function selectFlightbind() {
  var msg = sortF;
  var OnwardAmount;
  var ReturnAmount;
  var total = 0;
  var OnwardAmount_Final;
  var ReturnAmount_Final;
  var total_Final = 0;

  for (var i = 0; i < msg.d.length; i++) {
    if (i == 0) {
      $("#flightSelectOutbound").html("");
      var html = [];
      html.push("<table width='100%' ><tr>");
      html.push("<td class='flimobselec' style='' valign='top'>");
      html.push(
        "<img  src=" +
          msg.d[i].logo +
          " alt=Onward flight style='width:26px;height:26px' class='borderade' />  <span class='myfindvpo'>" +
          msg.d[i].FlightName +
          " - " +
          msg.d[i].FlightNumber +
          "</span> </td>"
      );
      html.push("<td class='myfindv flimobselec1' style=padding:0px>");
      // html.push(msg.d[i].SRC + "→" + msg.d[i].DEST);
      html.push(
        "<span class='fdtbox1'><span class='showfontsize mobdevir1'>" +
          msg.d[i].FlightDepTime +
          " </span> <span class='defjk'> " +
          msg.d[i].SRC +
          " </span>" +
          msg.d[i].FlightDepDate +
          "</span> <span class='fdtbox2'> <i class='fa fa-plane planeki1' ></i>  </span>   <span class='fdtbox3 twmjk'><span class='showfontsize mobdevir1'> " +
          msg.d[i].FlightArrTime +
          "</span> <span class='defjk'>" +
          msg.d[i].DEST +
          "</span>" +
          msg.d[i].FlightArrDate +
          "</span> <span class='fdtbotx4'><span class='mobdevir1'> " +
          msg.d[i].Layover1 +
          "</span>" +
          msg.d[i].Stop +
          " </span></td>"
      );
      html.push("</tr></table>");
      $("#flightSelectOutbound").append(html.join(""));
      OnwardAmount = msg.d[i].TotalAmount;
      OnwardAmount_Final = msg.d[i].FinalFare;

      document.getElementById("HiddenRefOnward").value = msg.d[i].FlightRefid;
      document.getElementById("HiddenOnwardAmount").value =
        msg.d[i].TotalAmount;
      document.getElementById("HiddenOnwardAmount_FinalFare").value =
        msg.d[i].FinalFare;
      if (msg.d[0].connection == 1) {
        document.getElementById("ArrDateDescO").value = msg.d[0].FlightArrDate1;
        document.getElementById("ArrTimeDescO").value = msg.d[0].FlightArrTime1;
      }
      if (msg.d[0].connection == 2) {
        document.getElementById("ArrDateDescO").value = msg.d[0].FlightArrDate2;
        document.getElementById("ArrTimeDescO").value = msg.d[0].FlightArrTime2;
      }
      if (msg.d[0].connection == 3) {
        document.getElementById("ArrDateDescO").value = msg.d[0].FlightArrDate3;
        document.getElementById("ArrTimeDescO").value = msg.d[0].FlightArrTime3;
      }
    }
  }
  var msg2 = sortFR;

  for (var i = 0; i < msg2.d.length; i++) {
    if (i == 0) {
      $("#flightSelectInbound").html("");
      var html = [];
      html.push("<table width='100%'><tr>");
      html.push("<td class='flimobselec' style='' valign='top'>");
      html.push(
        "<img  src=" +
          msg2.d[i].logo +
          " alt='Return flight' style='width:26px;height:26px' class='borderade' />  <span class='myfindvpo'>" +
          msg2.d[i].FlightName +
          " - " +
          msg2.d[i].FlightNumber +
          "</span></td>"
      );
      html.push(
        "<td  class='myfindv flimobselec1' vertical-align='top' style=''>"
      );
      // html.push(msg2.d[i].SRC + "→" + msg2.d[i].DEST);
      html.push(
        "<span class='fdtbox1'><span class='showfontsize mobdevir1'>" +
          msg2.d[i].FlightDepTime +
          "</span> <span class='defjk'>" +
          msg2.d[i].SRC +
          "</span> " +
          msg2.d[i].FlightDepDate +
          " </span> <span class='fdtbox2'><i class='fa fa-plane planeki2' ></i> </span><span class='fdtbox3 '> <span class='showfontsize mobdevir1'> " +
          msg2.d[i].FlightArrTime +
          "</span> <span class='defjk'> " +
          msg2.d[i].DEST +
          " </span> " +
          msg2.d[i].FlightArrDate +
          "</span> <span class='fdtbotx4'> <span class='mobdevir1'>" +
          msg2.d[i].Layover1 +
          " </span> " +
          msg2.d[i].Stop +
          " </span></td>"
      );
      html.push("</tr></table>");
      $("#flightSelectInbound").append(html.join(""));
      ReturnAmount = msg2.d[i].TotalAmount;
      ReturnAmount_Final = msg2.d[i].FinalFare;

      document.getElementById("HiddenRefReturn").value = msg2.d[i].FlightRefid;
      document.getElementById("HiddenReturnAmount").value =
        msg2.d[i].TotalAmount;
      document.getElementById("HiddenReturnAmount_FinalFare").value =
        msg2.d[i].FinalFare;
      if (msg2.d[0].connection == 1) {
        document.getElementById("DepDateDescR").value =
          msg2.d[0].FlightDepDate1;
        document.getElementById("DepTimeDescR").value =
          msg2.d[0].FlightDepTime1;
      }
      if (msg2.d[0].connection == 2) {
        document.getElementById("DepDateDescR").value =
          msg2.d[0].FlightDepDate1;
        document.getElementById("DepTimeDescR").value =
          msg2.d[0].FlightDepTime1;
      }
      if (msg2.d[0].connection == 3) {
        document.getElementById("DepDateDescR").value =
          msg2.d[0].FlightDepDate1;
        document.getElementById("DepTimeDescR").value =
          msg2.d[0].FlightDepTime1;
      }
    }
  }
  total = parseInt(OnwardAmount) + parseInt(ReturnAmount);
  total_Final = parseInt(OnwardAmount_Final) + parseInt(ReturnAmount_Final);

  $("#flightSelectAmount").html("");
  var html = [];
  html.push("<table width='100%'><tr>");
  html.push("<td style='padding:0px'> <div class='mainkfd'> ");
  html.push(
    "<span class='totalamount'> " +
      $("#hdncurrencytype").val() +
      " <span class='einr' style='text-decoration: line-through red;' id='myTargetElement'> </span>    <span class='actu' id='myTargetElement_Final'>  </span> </span>  <a  href='javascript:void(0);' class='book_now_button' onclick='BookNow()' > Book </a> </div></td>  "
  );
  html.push("</tr></table>");
  $("#flightSelectAmount").append(html.join(""));
  var options = {
    useEasing: true, // toggle easing
    useGrouping: true, // 1,000,000 vs 1000000
    separator: ",", // character to use as a separator
    decimal: ".", // character to use as a decimal
  };
  var useOnComplete = false;
  var useEasing = true;
  var useGrouping = true;
  var demo = new countUp("myTargetElement", 0, total, 0, 0, options);
  demo.start();

  var demo_Final = new countUp(
    "myTargetElement_Final",
    0,
    total_Final,
    0,
    0,
    options
  );
  demo_Final.start();
}
window.selectFlightbind = selectFlightbind;

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
  document.getElementById("hdnminval").value = min;
  document.getElementById("hdnmaxval").value = max;

  $("#slider-range").slider({
    range: true,
    min: min,
    max: max,
    values: [min, max],
    slide: function (event, ui) {
      $("#amount").val(
        $("#hdncurrencytype").val() +
          " " +
          ui.values[0] +
          " - " +
          $("#hdncurrencytype").val() +
          " " +
          ui.values[1]
      );
    },
    stop: function (event, ui) {
      document.getElementById("hdnminval").value = ui.values[0];
      document.getElementById("hdnmaxval").value = ui.values[1];

      getCheckedFilters(flagSort);
      chkunchk();
    },
  });
  $("#amount").val(
    $("#hdncurrencytype").val() +
      " " +
      min +
      " - " +
      $("#hdncurrencytype").val() +
      " " +
      max
  );

  //**********************return
  GetMinMaxR();
}

function GetMinMaxDepartureTime() {
  document.getElementById("hdnmindeptime").value = 0;
  document.getElementById("hdnmaxdeptime").value = 1440;

  $("#DepartureTimes-slider-range").slider({
    range: true,
    min: 0,
    max: 1440,
    step: 30,
    values: [0, 1440],
    slide: function (event, ui) {
      var hours1 = Math.floor(ui.values[0] / 60);
      var minutes1 = ui.values[0] - hours1 * 60;

      if (hours1.length == 1) hours1 = "0" + hours1;
      if (minutes1.length == 1) minutes1 = "0" + minutes1;
      if (minutes1 == 0) minutes1 = "00";

      var hours2 = Math.floor(ui.values[1] / 60);
      var minutes2 = ui.values[1] - hours2 * 60;
      if (hours2.length == 1) hours2 = "0" + hours2;
      if (minutes2.length == 1) minutes2 = "0" + minutes2;
      if (minutes2 == 0) minutes2 = "00";

      $("#amountDepartureTimes").val(
        "Time:" + hours1 + ":" + minutes1 + "-Time: " + hours2 + ":" + minutes2
      );
    },
    stop: function (event, ui) {
      document.getElementById("hdnmindeptime").value = ui.values[0];
      document.getElementById("hdnmaxdeptime").value = ui.values[1];
      getCheckedFilters(flagSort);

      chkunchk();
    },
  });
  $("#amountDepartureTimes").val("Time:" + "00:00" + "-Time " + "24:00");
}
function GetMinMaxArrivalTime() {
  document.getElementById("hdnminarrtime").value = 0;
  document.getElementById("hdnmaxarrtime").value = 1440;

  $("#ArrivalTimes-slider-range").slider({
    range: true,
    min: 0,
    max: 1440,
    step: 30,
    values: [0, 1440],
    slide: function (event, ui) {
      var hours1 = Math.floor(ui.values[0] / 60);
      var minutes1 = ui.values[0] - hours1 * 60;

      if (hours1.length == 1) hours1 = "0" + hours1;
      if (minutes1.length == 1) minutes1 = "0" + minutes1;
      if (minutes1 == 0) minutes1 = "00";

      var hours2 = Math.floor(ui.values[1] / 60);
      var minutes2 = ui.values[1] - hours2 * 60;
      if (hours2.length == 1) hours2 = "0" + hours2;
      if (minutes2.length == 1) minutes2 = "0" + minutes2;
      if (minutes2 == 0) minutes2 = "00";

      $("#amountArrivalTimes").val(
        "Time:" + hours1 + ":" + minutes1 + "-Time: " + hours2 + ":" + minutes2
      );
    },
    stop: function (event, ui) {
      jQuery("#tempView").html("");

      document.getElementById("hdnminarrtime").value = ui.values[0];
      document.getElementById("hdnmaxarrtime").value = ui.values[1];

      getCheckedFilters(flagSort);
      chkunchk();
    },
  });
  $("#amountArrivalTimes").val("Time:" + "00:00" + "-Time " + "24:00");
}

var minR;
var maxR;
function GetMinMaxR() {
  var msg = sortFR;
  maxR = 0;
  $(msg.d).each(function (index, element) {
    var TotalAmount = parseFloat(msg.d[index].TotalAmount); //Rs.${parseInt(Price_sngl_EP)+parseInt(OutBoundTotalFare)+parseInt(InBoundTotalFare)}

    if (TotalAmount > parseFloat(maxR)) {
      maxR = parseFloat(msg.d[index].TotalAmount);
    }
  });

  minR = maxR;
  $(msg.d).each(function (index, element) {
    var TotalAmount = parseFloat(msg.d[index].TotalAmount); //Rs.${parseInt(Price_sngl_EP)+parseInt(OutBoundTotalFare)+parseInt(InBoundTotalFare)}
    if (TotalAmount < parseFloat(minR)) {
      minR = parseFloat(msg.d[index].TotalAmount);
    }
  });
  document.getElementById("hdnminvalr").value = min;
  document.getElementById("hdnmaxvalr").value = max;

  $("#slider-rangeR").slider({
    range: true,
    min: minR,
    max: maxR,
    values: [minR, maxR],
    slide: function (event, ui) {
      $("#amountR").val(
        $("#hdncurrencytype").val() +
          " " +
          ui.values[0] +
          " - " +
          $("#hdncurrencytype").val() +
          " " +
          ui.values[1]
      );
    },
    stop: function (event, ui) {
      document.getElementById("hdnminvalr").value = ui.values[0];
      document.getElementById("hdnmaxvalr").value = ui.values[1];

      getCheckedFiltersR(SortCriteria);
      chkunchk();
    },
  });
  $("#amountR").val(
    $("#hdncurrencytype").val() +
      " " +
      min +
      " - " +
      $("#hdncurrencytype").val() +
      " " +
      max
  );

  GetMinMaxDepartureTimeR();
}
var minDepartureTimeR;
var maxDepartureTimeR;
var minTimedR;
var maxTimedR;
function GetMinMaxDepartureTimeR() {
  document.getElementById("hdnmindeptimer").value = 0;
  document.getElementById("hdnmaxdeptimer").value = 1440;

  $("#DepartureTimes-slider-rangeR").slider({
    range: true,
    min: 0,
    max: 1440,
    step: 30,
    values: [0, 1440],
    slide: function (event, ui) {
      var hours1 = Math.floor(ui.values[0] / 60);
      var minutes1 = ui.values[0] - hours1 * 60;

      if (hours1.length == 1) hours1 = "0" + hours1;
      if (minutes1.length == 1) minutes1 = "0" + minutes1;
      if (minutes1 == 0) minutes1 = "00";

      var hours2 = Math.floor(ui.values[1] / 60);
      var minutes2 = ui.values[1] - hours2 * 60;
      if (hours2.length == 1) hours2 = "0" + hours2;
      if (minutes2.length == 1) minutes2 = "0" + minutes2;
      if (minutes2 == 0) minutes2 = "00";

      $("#amountDepartureTimesR").val(
        "Time:" + hours1 + ":" + minutes1 + "-Time: " + hours2 + ":" + minutes2
      );
    },
    stop: function (event, ui) {
      document.getElementById("hdnmindeptimer").value = ui.values[0];
      document.getElementById("hdnmaxdeptimer").value = ui.values[1];
      getCheckedFiltersR(SortCriteria);

      chkunchk();
    },
  });
  $("#amountDepartureTimesR").val("Time:" + "00:00" + "-Time " + "24:00");
  GetMinMaxArrivalTimeR();
}
var minArrivalTimeR;
var maxArrivalTimeR;
var minTimedArrivalR;
var maxTimedArrivalR;
function GetMinMaxArrivalTimeR() {
  document.getElementById("hdnminarrtimer").value = 0;
  document.getElementById("hdnmaxarrtimer").value = 1440;

  $("#ArrivalTimes-slider-rangeR").slider({
    range: true,
    min: 0,
    max: 1440,
    step: 30,
    values: [0, 1440],
    slide: function (event, ui) {
      var hours1 = Math.floor(ui.values[0] / 60);
      var minutes1 = ui.values[0] - hours1 * 60;

      if (hours1.length == 1) hours1 = "0" + hours1;
      if (minutes1.length == 1) minutes1 = "0" + minutes1;
      if (minutes1 == 0) minutes1 = "00";

      var hours2 = Math.floor(ui.values[1] / 60);
      var minutes2 = ui.values[1] - hours2 * 60;
      if (hours2.length == 1) hours2 = "0" + hours2;
      if (minutes2.length == 1) minutes2 = "0" + minutes2;
      if (minutes2 == 0) minutes2 = "00";

      $("#amountArrivalTimesR").val(
        "Time:" + hours1 + ":" + minutes1 + "-Time: " + hours2 + ":" + minutes2
      );
    },
    stop: function (event, ui) {
      document.getElementById("hdnminarrtimer").value = ui.values[0];
      document.getElementById("hdnmaxarrtimer").value = ui.values[1];

      getCheckedFiltersR(SortCriteria);

      chkunchk();
    },
  });
  $("#amountArrivalTimesR").val("Time:" + "00:00" + "-Time " + "24:00");
}

function getAll(ck, checkboxContainer) {
  $("#modelpopupOUTER").css("display", "Block");
  $("#modelpopup").css("background-color", "Gray");
  $("#modelpopup").delay(1).fadeIn(400);
  $("#modelpopupOUTER").delay(1).fadeIn(400);
  if (ck == "CHECK") {
    $(checkboxContainer)
      .find("input:checkbox")
      .each(function () {
        this.checked = true;
      });
  } else {
    $(checkboxContainer)
      .find("input:checkbox")
      .each(function () {
        this.checked = false;
      });
  }
  if (checkboxContainer == "ul.amenViewairline") {
    getCheckedFilters(flagSort);
  } else {
    getCheckedFiltersR(flagSort);
  }
  chkunchk();
}
window.getAll = getAll;
function showChk(FlightRefid) {
  if ($("#DetailViewplusChk" + FlightRefid + "").css("display") == "block") {
    $("#divshowChk" + FlightRefid + "").show("slow");
    $("#DetailViewplusChk" + FlightRefid + "").css("display", "none");
    $("#DetailViewminusChk" + FlightRefid + "").css("display", "block");
  } else {
    $("#divshowChk" + FlightRefid + "").hide("slow");
    $("#DetailViewplusChk" + FlightRefid + "").css("display", "block");
    $("#DetailViewminusChk" + FlightRefid + "").css("display", "none");
  }
}
function show(FlightRefid) {
  if ($("#DetailViewplus" + FlightRefid + "").css("display") == "block") {
    $("#divshow" + FlightRefid + "").show("slow");
    $("#DetailViewplus" + FlightRefid + "").css("display", "none");
    $("#DetailViewminus" + FlightRefid + "").css("display", "block");
  } else {
    $("#divshow" + FlightRefid + "").hide("slow");
    $("#DetailViewplus" + FlightRefid + "").css("display", "block");
    $("#DetailViewminus" + FlightRefid + "").css("display", "none");
  }
}
window.show = show;
function showRChk(FlightRefid) {
  if ($("#DetailViewplusRChk" + FlightRefid + "").css("display") == "block") {
    $("#divshowRChk" + FlightRefid + "").show("slow");
    $("#DetailViewplusRChk" + FlightRefid + "").css("display", "none");
    $("#DetailViewminusRChk" + FlightRefid + "").css("display", "block");
  } else {
    $("#divshowRChk" + FlightRefid + "").hide("slow");
    $("#DetailViewplusRChk" + FlightRefid + "").css("display", "block");
    $("#DetailViewminusRChk" + FlightRefid + "").css("display", "none");
  }
}
window.showRChk = showRChk;
function showR(FlightRefid) {
  if ($("#DetailViewplusR" + FlightRefid + "").css("display") == "block") {
    $("#divshowR" + FlightRefid + "").show("slow");
    $("#DetailViewplusR" + FlightRefid + "").css("display", "none");
    $("#DetailViewminusR" + FlightRefid + "").css("display", "block");
  } else {
    $("#divshowR" + FlightRefid + "").hide("slow");
    $("#DetailViewplusR" + FlightRefid + "").css("display", "block");
    $("#DetailViewminusR" + FlightRefid + "").css("display", "none");
  }
}
window.showR = showR
function FAREDETAILS(FlightRefid) {
  $("#ITINERARY" + FlightRefid + "").css("border-bottom", "0px solid #FEBD25");
  $("#FAREDETAILS" + FlightRefid + "").css(
    "border-bottom",
    "3px solid #FEBD25"
  );
  $("#FARERULERO" + FlightRefid + "").css("border-bottom", "0px solid #C0C0C0");
  $("#BAGGAGEDETAILSRO" + FlightRefid + "").css(
    "border-bottom",
    "0px solid #C0C0C0"
  );

  $("#divITINERARY" + FlightRefid + "").slideUp();
  $("#FareSummaryO" + FlightRefid + "").slideDown();
  $("#BaggagesummeryO" + FlightRefid + "").slideUp();
  $("#FarerulesummeryO" + FlightRefid + "").slideUp();
}
window.FAREDETAILS = FAREDETAILS;
function ITINERARY(FlightRefid) {
  $("#ITINERARY" + FlightRefid + "").css("border-bottom", "3px solid #FEBD25");
  $("#FAREDETAILS" + FlightRefid + "").css(
    "border-bottom",
    "0px solid #C0C0C0"
  );
  $("#FARERULERO" + FlightRefid + "").css("border-bottom", "0px solid #C0C0C0");
  $("#BAGGAGEDETAILSRO" + FlightRefid + "").css(
    "border-bottom",
    "0px solid #C0C0C0"
  );

  $("#divITINERARY" + FlightRefid + "").slideDown();
  $("#FareSummaryO" + FlightRefid + "").slideUp();
  $("#BaggagesummeryO" + FlightRefid + "").slideUp();
  $("#FarerulesummeryO" + FlightRefid + "").slideUp();
}
window.ITINERARY = ITINERARY;
function FARERULERO(FlightRefid) {
  //   FareRule(FlightRefid);
  $("#ITINERARY" + FlightRefid + "").css("border-bottom", "0px solid #C0C0C0");
  $("#FAREDETAILS" + FlightRefid + "").css(
    "border-bottom",
    "0px solid #C0C0C0"
  );
  $("#FARERULERO" + FlightRefid + "").css("border-bottom", "3px solid #FEBD25");
  $("#BAGGAGEDETAILSRO" + FlightRefid + "").css(
    "border-bottom",
    "0px solid #C0C0C0"
  );

  $("#divITINERARY" + FlightRefid + "").slideUp();
  $("#FareSummaryO" + FlightRefid + "").slideUp();
  $("#BaggagesummeryO" + FlightRefid + "").slideUp();
  $("#FarerulesummeryO" + FlightRefid + "").slideDown();
}
window.FARERULERO = FARERULERO;
function BAGGAGEDETAILSRO(FlightRefid) {
  $("#ITINERARY" + FlightRefid + "").css("border-bottom", "0px solid #C0C0C0");
  $("#FAREDETAILS" + FlightRefid + "").css(
    "border-bottom",
    "0px solid #C0C0C0"
  );
  $("#FARERULERO" + FlightRefid + "").css("border-bottom", "0px solid #C0C0C0");
  $("#BAGGAGEDETAILSRO" + FlightRefid + "").css(
    "border-bottom",
    "3px solid #FEBD25"
  );

  $("#divITINERARY" + FlightRefid + "").slideUp();
  $("#FareSummaryO" + FlightRefid + "").slideUp();
  $("#BaggagesummeryO" + FlightRefid + "").slideDown();
  $("#FarerulesummeryO" + FlightRefid + "").slideUp();
}
window.BAGGAGEDETAILSRO = BAGGAGEDETAILSRO;
function showtaxandChargesOB(FlightRefid) {
  $("#TaxAndChargesDIVOB" + FlightRefid).css("display") == "none"
    ? $("#TaxAndChargesDIVOB" + FlightRefid).slideDown()
    : $("#TaxAndChargesDIVOB" + FlightRefid).slideUp();
  $("#TotalTaxDIVOB" + FlightRefid).slideUp();
  $("#CommDetailDIVOB" + FlightRefid).slideUp();
}
window.showtaxandChargesOB = showtaxandChargesOB;
function showtotaltaxOB(FlightRefid) {
  $("#TotalTaxDIVOB" + FlightRefid).css("display") == "none"
    ? $("#TotalTaxDIVOB" + FlightRefid).slideDown()
    : $("#TotalTaxDIVOB" + FlightRefid).slideUp();
  $("#CommDetailDIVOB" + FlightRefid).slideUp();
}
window.showtaxandChargesOB = showtaxandChargesOB;
function showCommDetailOB(FlightRefid) {
  $("#CommDetailDIVOB" + FlightRefid).css("display") == "none"
    ? $("#CommDetailDIVOB" + FlightRefid).slideDown()
    : $("#CommDetailDIVOB" + FlightRefid).slideUp();
  $("#TotalTaxDIVOB" + FlightRefid).slideUp();
}
window.showCommDetailOB = showCommDetailOB;
function FAREDETAILSChk(FlightRefid) {
  $("#ITINERARYChk" + FlightRefid + "").css(
    "border-bottom",
    "0px solid #FEBD25"
  );
  $("#FAREDETAILSChk" + FlightRefid + "").css(
    "border-bottom",
    "3px solid #FEBD25"
  );
  $("#FARERULEROChk" + FlightRefid + "").css(
    "border-bottom",
    "0px solid #C0C0C0"
  );
  $("#BAGGAGEDETAILSROChk" + FlightRefid + "").css(
    "border-bottom",
    "0px solid #C0C0C0"
  );

  $("#divITINERARYChk" + FlightRefid + "").slideUp();
  $("#FareSummaryOChkk" + FlightRefid + "").slideDown();
  $("#BaggagesummeryOChk" + FlightRefid + "").slideUp();
  $("#FarerulesummeryOChk" + FlightRefid + "").slideUp();
}
window.FAREDETAILSChk = FAREDETAILSChk;
function ITINERARYChk(FlightRefid) {
  $("#ITINERARYChk" + FlightRefid + "").css(
    "border-bottom",
    "3px solid #FEBD25"
  );
  $("#FAREDETAILSChk" + FlightRefid + "").css(
    "border-bottom",
    "0px solid #C0C0C0"
  );
  $("#FARERULEROChk" + FlightRefid + "").css(
    "border-bottom",
    "0px solid #C0C0C0"
  );
  $("#BAGGAGEDETAILSROChk" + FlightRefid + "").css(
    "border-bottom",
    "0px solid #C0C0C0"
  );

  $("#divITINERARYChk" + FlightRefid + "").slideDown();
  $("#FareSummaryOChkk" + FlightRefid + "").slideUp();
  $("#BaggagesummeryOChk" + FlightRefid + "").slideUp();
  $("#FarerulesummeryOChk" + FlightRefid + "").slideUp();
}
window.ITINERARYChk = ITINERARYChk;
function FARERULEROChk(FlightRefid) {
  //   FareRule(FlightRefid);
  $("#ITINERARYChk" + FlightRefid + "").css(
    "border-bottom",
    "0px solid #C0C0C0"
  );
  $("#FAREDETAILSChk" + FlightRefid + "").css(
    "border-bottom",
    "0px solid #C0C0C0"
  );
  $("#FARERULEROChk" + FlightRefid + "").css(
    "border-bottom",
    "3px solid #FEBD25"
  );
  $("#BAGGAGEDETAILSROChk" + FlightRefid + "").css(
    "border-bottom",
    "0px solid #C0C0C0"
  );

  $("#divITINERARYChk" + FlightRefid + "").slideUp();
  $("#FareSummaryOChkk" + FlightRefid + "").slideUp();
  $("#BaggagesummeryOChk" + FlightRefid + "").slideUp();
  $("#FarerulesummeryOChk" + FlightRefid + "").slideDown();
}
window.FARERULEROChk = FARERULEROChk;
function BAGGAGEDETAILSROChk(FlightRefid) {
  $("#ITINERARYChk" + FlightRefid + "").css(
    "border-bottom",
    "0px solid #C0C0C0"
  );
  $("#FAREDETAILSChk" + FlightRefid + "").css(
    "border-bottom",
    "0px solid #C0C0C0"
  );
  $("#FARERULEROChk" + FlightRefid + "").css(
    "border-bottom",
    "0px solid #C0C0C0"
  );
  $("#BAGGAGEDETAILSROChk" + FlightRefid + "").css(
    "border-bottom",
    "3px solid #FEBD25"
  );

  $("#divITINERARYChk" + FlightRefid + "").slideUp();
  $("#FareSummaryOChkk" + FlightRefid + "").slideUp();
  $("#BaggagesummeryOChk" + FlightRefid + "").slideDown();
  $("#FarerulesummeryOChk" + FlightRefid + "").slideUp();
}
window.BAGGAGEDETAILSROChk = BAGGAGEDETAILSROChk;
function showtaxandChargesOBChkk(FlightRefid) {
  $("#TaxAndChargesDIVOBChkk" + FlightRefid).css("display") == "none"
    ? $("#TaxAndChargesDIVOBChkk" + FlightRefid).slideDown()
    : $("#TaxAndChargesDIVOBChkk" + FlightRefid).slideUp();
  $("#TotalTaxDIVOBChkk" + FlightRefid).slideUp();
  $("#CommDetailDIVOBChkk" + FlightRefid).slideUp();
}
window.showtaxandChargesOBChkk = showtaxandChargesOBChkk;
function showtotaltaxOBChkk(FlightRefid) {
  $("#TotalTaxDIVOBChkk" + FlightRefid).css("display") == "none"
    ? $("#TotalTaxDIVOBChkk" + FlightRefid).slideDown()
    : $("#TotalTaxDIVOBChkk" + FlightRefid).slideUp();
  $("#CommDetailDIVOBChkk" + FlightRefid).slideUp();
}
window.showtotaltaxOBChkk = showtotaltaxOBChkk;
function showCommDetailOBChkk(FlightRefid) {
  $("#CommDetailDIVOBChkk" + FlightRefid).css("display") == "none"
    ? $("#CommDetailDIVOBChkk" + FlightRefid).slideDown()
    : $("#CommDetailDIVOBChkk" + FlightRefid).slideUp();
  $("#TotalTaxDIVOBChkk" + FlightRefid).slideUp();
}
window.showCommDetailOBChkk = showCommDetailOBChkk;

function FAREDETAILSRChk(FlightRefid) {
  $("#ITINERARYRChk" + FlightRefid + "").css(
    "border-bottom",
    "0px solid #FEBD25"
  );
  $("#FAREDETAILSRChk" + FlightRefid + "").css(
    "border-bottom",
    "3px solid #FEBD25"
  );
  $("#FARERULERChk" + FlightRefid + "").css(
    "border-bottom",
    "0px solid #C0C0C0"
  );
  $("#BAGGAGEDETAILSRChk" + FlightRefid + "").css(
    "border-bottom",
    "0px solid #C0C0C0"
  );

  $("#divITINERARYRChk" + FlightRefid + "").slideUp();
  $("#FareSummaryRChkk" + FlightRefid + "").slideDown();
  $("#BaggagesummeryRChk" + FlightRefid + "").slideUp();
  $("#FarerulesummeryRChk" + FlightRefid + "").slideUp();
}
window.FAREDETAILSRChk = FAREDETAILSRChk;

function ITINERARYRChk(FlightRefid) {
  $("#ITINERARYRChk" + FlightRefid + "").css(
    "border-bottom",
    "3px solid #FEBD25"
  );
  $("#FAREDETAILSRChk" + FlightRefid + "").css(
    "border-bottom",
    "0px solid #C0C0C0"
  );
  $("#FARERULERChk" + FlightRefid + "").css(
    "border-bottom",
    "0px solid #C0C0C0"
  );
  $("#BAGGAGEDETAILSRChk" + FlightRefid + "").css(
    "border-bottom",
    "0px solid #C0C0C0"
  );

  $("#divITINERARYRChk" + FlightRefid + "").slideDown();
  $("#FareSummaryRChkk" + FlightRefid + "").slideUp();
  $("#BaggagesummeryRChk" + FlightRefid + "").slideUp();
  $("#FarerulesummeryRChk" + FlightRefid + "").slideUp();
}
window.ITINERARYRChk = ITINERARYRChk;


function FARERULERChk(FlightRefid) {
  //   FareRule(FlightRefid);
  $("#ITINERARYRChk" + FlightRefid + "").css(
    "border-bottom",
    "0px solid #C0C0C0"
  );
  $("#FAREDETAILSRChk" + FlightRefid + "").css(
    "border-bottom",
    "0px solid #C0C0C0"
  );
  $("#FARERULERChk" + FlightRefid + "").css(
    "border-bottom",
    "3px solid #FEBD25"
  );
  $("#BAGGAGEDETAILSRChk" + FlightRefid + "").css(
    "border-bottom",
    "0px solid #C0C0C0"
  );

  $("#divITINERARYRChk" + FlightRefid + "").slideUp();
  $("#FareSummaryRChkk" + FlightRefid + "").slideUp();
  $("#BaggagesummeryRChk" + FlightRefid + "").slideUp();
  $("#FarerulesummeryRChk" + FlightRefid + "").slideDown();
}
window.FARERULERChk = ITINERARYRChk;
function BAGGAGEDETAILSRChk(FlightRefid) {
  $("#ITINERARYRChk" + FlightRefid + "").css(
    "border-bottom",
    "0px solid #C0C0C0"
  );
  $("#FAREDETAILSRChk" + FlightRefid + "").css(
    "border-bottom",
    "0px solid #C0C0C0"
  );
  $("#FARERULERChk" + FlightRefid + "").css(
    "border-bottom",
    "0px solid #C0C0C0"
  );
  $("#BAGGAGEDETAILSRChk" + FlightRefid + "").css(
    "border-bottom",
    "3px solid #FEBD25"
  );

  $("#divITINERARYRChk" + FlightRefid + "").slideUp();
  $("#FareSummaryRChkk" + FlightRefid + "").slideUp();
  $("#BaggagesummeryRChk" + FlightRefid + "").slideDown();
  $("#FarerulesummeryRChk" + FlightRefid + "").slideUp();
}
window.BAGGAGEDETAILSRChk = BAGGAGEDETAILSRChk;

function showtaxandChargesIBChk(FlightRefid) {
  $("#TaxAndChargesDIVIBChk" + FlightRefid).css("display") == "none"
    ? $("#TaxAndChargesDIVIBChk" + FlightRefid).slideDown()
    : $("#TaxAndChargesDIVIBChk" + FlightRefid).slideUp();
  $("#TotalTaxDIVIBChk" + FlightRefid).slideUp();
  $("#CommDetailDIVIBChk" + FlightRefid).slideUp();
}
window.showtaxandChargesIBChk = showtaxandChargesIBChk;
function showtotaltaxIBChk(FlightRefid) {
  $("#TotalTaxDIVIBChk" + FlightRefid).css("display") == "none"
    ? $("#TotalTaxDIVIBChk" + FlightRefid).slideDown()
    : $("#TotalTaxDIVIBChk" + FlightRefid).slideUp();
  $("#CommDetailDIVIBChk" + FlightRefid).slideUp();
}
window.showtotaltaxIBChk = showtotaltaxIBChk;
function showCommDetailIBChk(FlightRefid) {
  $("#CommDetailDIVIBChk" + FlightRefid).css("display") == "none"
    ? $("#CommDetailDIVIBChk" + FlightRefid).slideDown()
    : $("#CommDetailDIVIBChk" + FlightRefid).slideUp();
  $("#TotalTaxDIVIBChk" + FlightRefid).slideUp();
}
window.showCommDetailIBChk = showCommDetailIBChk;
function FAREDETAILSR(FlightRefid) {
  // FareRuleR(FlightRefid);

  $("#ITINERARYR" + FlightRefid + "").css("border-bottom", "0px solid #FEBD25");
  $("#FAREDETAILSR" + FlightRefid + "").css(
    "border-bottom",
    "3px solid #FEBD25"
  );
  $("#BAGGAGEDETAILSR" + FlightRefid + "").css(
    "border-bottom",
    "0px solid #C0C0C0"
  );
  $("#FARERULER" + FlightRefid + "").css("border-bottom", "0px solid #C0C0C0");

  $("#divITINERARYR" + FlightRefid + "").slideUp();
  $("#FareSummaryRR" + FlightRefid + "").slideDown();
  $("#BaggagesummeryR" + FlightRefid + "").slideUp();
  $("#FarerulesummeryR" + FlightRefid + "").slideUp();
}
window.FAREDETAILSR = FAREDETAILSR;
function ITINERARYR(FlightRefid) {
  $("#ITINERARYR" + FlightRefid + "").css("border-bottom", "3px solid #FEBD25");
  $("#FAREDETAILSR" + FlightRefid + "").css(
    "border-bottom",
    "0px solid #C0C0C0"
  );
  $("#BAGGAGEDETAILSR" + FlightRefid + "").css(
    "border-bottom",
    "0px solid #C0C0C0"
  );
  $("#FARERULER" + FlightRefid + "").css("border-bottom", "0px solid #C0C0C0");

  $("#divITINERARYR" + FlightRefid + "").slideDown();
  $("#FareSummaryRR" + FlightRefid + "").slideUp();
  $("#BaggagesummeryR" + FlightRefid + "").slideUp();
  $("#FarerulesummeryR" + FlightRefid + "").slideUp();
}
window.ITINERARYR = ITINERARYR;
function FARERULER(FlightRefid) {
  //  FareRuleR(FlightRefid);

  $("#ITINERARYR" + FlightRefid + "").css("border-bottom", "0px solid #C0C0C0");
  $("#FAREDETAILSR" + FlightRefid + "").css(
    "border-bottom",
    "0px solid #C0C0C0"
  );
  $("#BAGGAGEDETAILSR" + FlightRefid + "").css(
    "border-bottom",
    "0px solid #C0C0C0"
  );
  $("#FARERULER" + FlightRefid + "").css("border-bottom", "3px solid #FEBD25");

  $("#divITINERARYR" + FlightRefid + "").slideUp();
  $("#FareSummaryRR" + FlightRefid + "").slideUp();
  $("#BaggagesummeryR" + FlightRefid + "").slideUp();
  $("#FarerulesummeryR" + FlightRefid + "").slideDown();
}
window.FARERULER = FARERULER;
function BAGGAGEDETAILSR(FlightRefid) {
  $("#ITINERARYR" + FlightRefid + "").css("border-bottom", "0px solid #C0C0C0");
  $("#FAREDETAILSR" + FlightRefid + "").css(
    "border-bottom",
    "0px solid #C0C0C0"
  );
  $("#BAGGAGEDETAILSR" + FlightRefid + "").css(
    "border-bottom",
    "3px solid #FEBD25"
  );
  $("#FARERULER" + FlightRefid + "").css("border-bottom", "0px solid #C0C0C0");

  $("#divITINERARYR" + FlightRefid + "").slideUp();
  $("#FareSummaryRR" + FlightRefid + "").slideUp();
  $("#BaggagesummeryR" + FlightRefid + "").slideDown();
  $("#FarerulesummeryR" + FlightRefid + "").slideUp();
}
window.BAGGAGEDETAILSR = BAGGAGEDETAILSR;
function showtaxandChargesIB(FlightRefid) {
  $("#TaxAndChargesDIVIB" + FlightRefid).css("display") == "none"
    ? $("#TaxAndChargesDIVIB" + FlightRefid).slideDown()
    : $("#TaxAndChargesDIVIB" + FlightRefid).slideUp();
  $("#TotalTaxDIVIB" + FlightRefid).slideUp();
  $("#CommDetailDIVOB" + FlightRefid).slideUp();
}
window.showtaxandChargesIB = showtaxandChargesIB;
function showtotaltaxIB(FlightRefid) {
  $("#TotalTaxDIVIB" + FlightRefid).css("display") == "none"
    ? $("#TotalTaxDIVIB" + FlightRefid).slideDown()
    : $("#TotalTaxDIVIB" + FlightRefid).slideUp();
  $("#CommDetailDIVIB" + FlightRefid).slideUp();
}
window.showtotaltaxIB = showtotaltaxIB
function showCommDetailIB(FlightRefid) {
  $("#CommDetailDIVIB" + FlightRefid).css("display") == "none"
    ? $("#CommDetailDIVIB" + FlightRefid).slideDown()
    : $("#CommDetailDIVIB" + FlightRefid).slideUp();
  $("#TotalTaxDIVIB" + FlightRefid).slideUp();
}
window.showCommDetailIB = showCommDetailIB;
function chkunchk() {
  if (document.getElementById("chkdiscccfare").checked == true) {
    $(".discui").css({ display: "block" });
    $(".actu").css({ display: "block" });
    $(".einr").css("text-decoration", "line-through red");
    // $(".einr").css('font-size', '16px');
  } else {
    $(".discui").css({ display: "none" });
    $(".actu").css({ display: "none" });
    $(".einr").css("text-decoration", "");
  }
}
window.chkunchk = chkunchk

function Detailviewopen(data) {
  var Row1;

  Row1 =
    "<table width='550' cellpadding='2' cellspacing='0'><tr style='font-weight:bold;background-color: #000;color:#FFF;border-bottom: #dddddd 1px solid;font-family:verdana;'><td colspan='9' >Fare rules:-</td></tr><tr><td width='550' align='left'><span class='showfontsizeRule'>" +
    data +
    "</td></tr>";

  var RowAdd = "</table>";
  var divObj = document.getElementById("divFareRules");
  divObj.style.display = "block";

  divObj.left = "0px";
  divObj.top = "0px";
  divObj.innerHTML = Row1 + RowAdd;
}

function ddivclose() {
  var divObj = document.getElementById("divFareRules");
  divObj.style.display = "none";
}
function DetailviewopenR(data) {
  var Row1;

  Row1 =
    "<table width='550' cellpadding='2' cellspacing='0'><tr style='font-weight:bold;background-color: #000;color:#FFF;border-bottom: #dddddd 1px solid;'><td colspan='9' >Fare rules:-</td></tr><tr><td width='550' align='left'><span class='showfontsizeRule'>" +
    data +
    "</td></tr>";

  var RowAdd = "</table>";
  var divObj = document.getElementById("divFareRulesR");
  divObj.style.display = "block";

  divObj.left = "0px";
  divObj.top = "0px";
  divObj.innerHTML = Row1 + RowAdd;
}

function ddivcloseR() {
  var divObj = document.getElementById("divFareRulesR");
  divObj.style.display = "none";
}
function Detailviewopen2(data) {
  var Row1;

  Row1 =
    "<table width='550' cellpadding='2' cellspacing='0'><tr style='font-weight:bold;background-color: #000;color:#FFF;border-bottom: #dddddd 1px solid;font-family:verdana;'><td colspan='9' >Fare rules:-</td></tr><tr><td width='550' align='left'><span class='showfontsizeRule'>" +
    data +
    "</td></tr>";

  var RowAdd = "</table>";
  var divObj = document.getElementById("divFareRules2");
  divObj.style.display = "block";

  divObj.left = "0px";
  divObj.top = "0px";
  divObj.innerHTML = Row1 + RowAdd;
}
function fareruledivshow(abc) {
  var FlightRefid = $("#HiddenRefOnward").val();
  var FlightRefidR = $("#HiddenRefReturn").val();

  $("#FarerulesummeryPopup" + FlightRefid + "").html(
    $("#FarerulesummeryPopup" + FlightRefid + "").text()
  );
  $("#FarerulesummeryPopupR" + FlightRefidR + "").html(
    $("#FarerulesummeryPopupR" + FlightRefidR + "").text()
  );

  if (
    $("#FarerulesummeryPopup" + FlightRefid + "").css("display") === "none" &&
    $("#FarerulesummeryPopupR" + FlightRefidR + "").css("display") == "none" &&
    $("#fareruleDiv").css("display") == "none"
  ) {
    $("#fareruleDiv").css("display", "block");
    $("#FarerulesummeryPopup" + FlightRefid + "").show("slow");
    $("#FarerulesummeryPopupR" + FlightRefidR + "").show("slow");
  } else {
    $("#FarerulesummeryPopup" + FlightRefid + "").hide("slow");
    $("#FarerulesummeryPopupR" + FlightRefidR + "").hide("slow");
    $("#fareruleDiv").css("display", "none");
  }
}
window.fareruledivshow = fareruledivshow;
function ddivclose2() {
  var divObj = document.getElementById("divFareRules2");
  divObj.style.display = "none";
}
function Detailviewopen3(data) {
  var Row1;

  Row1 =
    "<table width='550' cellpadding='2' cellspacing='0'><tr style='font-weight:bold;background-color: #000;color:#FFF;border-bottom: #dddddd 1px solid;font-family:verdana;'><td colspan='9' >Fare rules:-</td></tr><tr><td width='550' align='left'><span class='showfontsizeRule'>" +
    data +
    "</td></tr>";

  var RowAdd = "</table>";
  var divObj = document.getElementById("divFareRules3");
  divObj.style.display = "block";

  divObj.left = "0px";
  divObj.top = "0px";
  divObj.innerHTML = Row1 + RowAdd;
}

function ddivclose3() {
  var divObj = document.getElementById("divFareRules3");
  divObj.style.display = "none";
}
function FareSummaryopen(data) {
  $("#" + data).slideToggle();
}
window.FareSummaryopen = FareSummaryopen;
function FareSummaryclose(data) {
  $("#" + data).css("display", "none");
}
window.FareSummaryclose = FareSummaryclose;
function FareSummaryopenR(data) {
  $("#" + data).slideToggle();
}
window.FareSummaryopenR = FareSummaryopenR;
function FareSummarycloseR(data) {
  $("#" + data).css("display", "none");
}
window.FareSummarycloseR = FareSummarycloseR;
function flightchange(ID) {
  var msg = sortF;
  var OnwardAmount;
  var ReturnAmount;
  var total = 0;
  var OnwardAmount_Final;
  var ReturnAmount_Final;
  var total_Final = 0;

  for (var i = 0; i < msg.d.length; i++) {
    try {
      if ($("#CheckChk" + msg.d[i].FlightRefid + "").is(":checked")) {
        $("#activeChk" + msg.d[i].FlightRefid + "").addClass("dhirajK");
        var dd = $("#hdnobchkdata").val();
        if (dd != "") {
          $("#active" + dd + "").removeClass("dhirajK");
        } else {
          $("#active" + msg.d[i].FlightRefid + "").removeClass("dhirajK");
        }
        $("#hdnobchkdata").val(msg.d[i].FlightRefid);
      } else if ($("#Check" + msg.d[i].FlightRefid + "").is(":checked")) {
        $("#active" + msg.d[i].FlightRefid + "").addClass("dhirajK");
        var dd1 = $("#hdnobchkdata").val();
        if (dd1 != "") {
          $("#activeChk" + dd1 + "").removeClass("dhirajK");
        } else {
          $("#activeChk" + msg.d[i].FlightRefid + "").removeClass("dhirajK");
        }
        $("#hdnobchkdata").val(msg.d[i].FlightRefid);
      } else {
        var dd = $("#hdnobchkdata").val();
        $("#activeChk" + dd + "").removeClass("dhirajK");
        $("#active" + dd + "").removeClass("dhirajK");
      }
    } catch (e) {}
    if (ID == msg.d[i].FlightRefid) {
      $("#flightSelectOutbound").html("");
      var html = [];
      html.push("<table width='100%'><tr>");
      html.push("<td class='flimobselec' style=padding:0px>");
      html.push(
        "<img  src=" +
          msg.d[i].logo +
          " alt=Onward flight style='width:26px;height:26px' class='borderade' />  <span class='myfindvpo'>" +
          msg.d[i].FlightName +
          " - " +
          msg.d[i].FlightNumber +
          "</span></td>"
      );
      html.push("<td class='myfindv flimobselec1' style=padding:0px>");
      //  html.push(msg.d[i].SRC + "→" + msg.d[i].DEST);
      html.push(
        "<span class='fdtbox1'><span class='showfontsize mobdevir1'> " +
          msg.d[i].FlightDepTime +
          "  </span> <span class='defjk'> " +
          msg.d[i].SRC +
          " </span>" +
          msg.d[i].FlightDepDate +
          "  </span> <span class='fdtbox2'> <i class='fa fa-plane planeki1'></i> </span> <span class='fdtbox3 twmjk'><span class='showfontsize mobdevir1'>   " +
          msg.d[i].FlightArrTime +
          " </span> <span class='defjk'> " +
          msg.d[i].DEST +
          " </span>" +
          msg.d[i].FlightArrDate +
          "</span> <span class='fdtbotx4'><span class='mobdevir1'> " +
          msg.d[i].Layover1 +
          "  </span>" +
          msg.d[i].Stop +
          "</span></td>"
      );
      html.push("</tr></table>");
      $("#flightSelectOutbound").append(html.join(""));
      OnwardAmount = msg.d[i].TotalAmount;
      OnwardAmount_Final = msg.d[i].FinalFare;

      document.getElementById("HiddenRefOnward").value = msg.d[i].FlightRefid;
      document.getElementById("HiddenOnwardAmount").value =
        msg.d[i].TotalAmount;
      document.getElementById("HiddenOnwardAmount_FinalFare").value =
        msg.d[i].FinalFare;

      if (msg.d[i].connection == 1) {
        document.getElementById("ArrDateDescO").value = msg.d[i].FlightArrDate1;
        document.getElementById("ArrTimeDescO").value = msg.d[i].FlightArrTime1;
      }
      if (msg.d[i].connection == 2) {
        document.getElementById("ArrDateDescO").value = msg.d[i].FlightArrDate2;
        document.getElementById("ArrTimeDescO").value = msg.d[i].FlightArrTime2;
      }
      if (msg.d[i].connection == 3) {
        document.getElementById("ArrDateDescO").value = msg.d[i].FlightArrDate3;
        document.getElementById("ArrTimeDescO").value = msg.d[i].FlightArrTime3;
      }
      break;
    }
  }

  ReturnAmount = document.getElementById("HiddenReturnAmount").value;
  ReturnAmount_Final = document.getElementById(
    "HiddenReturnAmount_FinalFare"
  ).value;

  total = parseInt(OnwardAmount) + parseInt(ReturnAmount);
  total_Final = parseInt(OnwardAmount_Final) + parseInt(ReturnAmount_Final);

  $("#flightSelectAmount").html("");
  var html = [];
  html.push("<table><tr>");
  html.push("<td style='padding:4px'> <div class='mainkfd Dkay'>");
  html.push(
    "<span class='totalamount'> " +
      $("#hdncurrencytype").val() +
      " <span class='einr' style='color:#000;font-weight:bold;text-decoration: line-through red;' id='myTargetElement'> </span>   <span class='actu' id='myTargetElement_Final'>   </span> </span> <a  href='javascript:void(0);' class='book_now_button'  onclick='BookNow()'> Book </a></div> </td>"
  );
  html.push("</tr></table>");
  $("#flightSelectAmount").append(html.join(""));
  var options = {
    useEasing: true, // toggle easing
    useGrouping: true, // 1,000,000 vs 1000000
    separator: ",", // character to use as a separator
    decimal: ".", // character to use as a decimal
  };
  var useOnComplete = false;
  var useEasing = true;
  var useGrouping = true;
  var demo = new countUp("myTargetElement", 0, total, 0, 0, options);
  demo.start();

  var demo_Final = new countUp(
    "myTargetElement_Final",
    0,
    total_Final,
    0,
    0,
    options
  );
  demo_Final.start();

  $("#flightSelectAmount").fadeTo(100, 0.1).fadeTo(200, 1.0);
  //$("#flightSelectOutbound").toggle("bounce", { times: 1 }, "fast");
  //$("#flightSelectOutbound").show("bounce", { times: 1 }, "fast");

  chkunchk();
}
window.flightchange = flightchange;
function flightchangeR(ID) {
  var OnwardAmount;
  var ReturnAmount;
  var total = 0;
  var OnwardAmount_Final;
  var ReturnAmount_Final;
  var total_Final = 0;

  var msg2 = sortFR;
  for (var i = 0; i < msg2.d.length; i++) {
    try {
      if ($("#CheckRChk" + msg2.d[i].FlightRefid + "").is(":checked")) {
        $("#activeRChk" + msg2.d[i].FlightRefid + "").addClass("dhirajK");
        var dd = $("#hdnibchkdata").val();
        if (dd != "") {
          $("#activeR" + dd + "").removeClass("dhirajK");
        } else {
          $("#activeR" + msg2.d[i].FlightRefid + "").removeClass("dhirajK");
        }
        $("#hdnibchkdata").val(msg2.d[i].FlightRefid);
      } else if ($("#CheckR" + msg2.d[i].FlightRefid + "").is(":checked")) {
        $("#activeR" + msg2.d[i].FlightRefid + "").addClass("dhirajK");
        var dd1 = $("#hdnibchkdata").val();
        if (dd1 != "") {
          $("#activeRChk" + dd1 + "").removeClass("dhirajK");
        } else {
          $("#activeRChk" + msg2.d[i].FlightRefid + "").removeClass("dhirajK");
        }
        $("#hdnibchkdata").val(msg2.d[i].FlightRefid);
      } else {
        var dd = $("#hdnibchkdata").val();
        $("#activeRChk" + dd + "").removeClass("dhirajK");
        $("#activeR" + dd + "").removeClass("dhirajK");
      }
    } catch (e) {}

    if (ID == msg2.d[i].FlightRefid) {
      $("#flightSelectInbound").html("");

      var html = [];
      html.push("<table width='100%' ><tr>");
      html.push("<td class='flimobselec' style='padding:0px'>");
      html.push(
        "<img src=" +
          msg2.d[i].logo +
          " alt=Return flight style='width:26px;height:26px' class='borderade' />  <span class='myfindvpo'>" +
          msg2.d[i].FlightName +
          " - " +
          msg2.d[i].FlightNumber +
          "</span></td>"
      );
      html.push("<td class='myfindv flimobselec1' style=padding:0px>");
      // html.push(msg2.d[i].SRC + "→" + msg2.d[i].DEST);
      html.push(
        "<span class='fdtbox1'><span class='showfontsize mobdevir1'> " +
          msg2.d[i].FlightDepTime +
          "</span> <span class='defjk'> " +
          msg2.d[i].SRC +
          " </span>" +
          msg2.d[i].FlightDepDate +
          " </span> <span class='fdtbox2'>  <i class='fa fa-plane planeki2'></i> </span> <span class='fdtbox3 twmjk'><span class='showfontsize mobdevir1'>  " +
          msg2.d[i].FlightArrTime +
          " </span> <span class='defjk'>" +
          msg2.d[i].DEST +
          " </span>" +
          msg2.d[i].FlightArrDate +
          "  </span> <span class='fdtbotx4'> <span class='mobdevir1'>" +
          msg2.d[i].Layover1 +
          " </span> " +
          msg2.d[i].Stop +
          " </span> </td>"
      );
      html.push("</tr></table>");
      $("#flightSelectInbound").append(html.join(""));
      ReturnAmount = msg2.d[i].TotalAmount;
      ReturnAmount_Final = msg2.d[i].FinalFare;

      document.getElementById("HiddenRefReturn").value = msg2.d[i].FlightRefid;
      document.getElementById("HiddenReturnAmount").value =
        msg2.d[i].TotalAmount;
      document.getElementById("HiddenReturnAmount_FinalFare").value =
        msg2.d[i].FinalFare;

      if (msg2.d[i].connection == 1) {
        document.getElementById("DepDateDescR").value =
          msg2.d[i].FlightDepDate1;
        document.getElementById("DepTimeDescR").value =
          msg2.d[i].FlightDepTime1;
      }
      if (msg2.d[i].connection == 2) {
        document.getElementById("DepDateDescR").value =
          msg2.d[i].FlightDepDate1;
        document.getElementById("DepTimeDescR").value =
          msg2.d[i].FlightDepTime1;
      }
      if (msg2.d[i].connection == 3) {
        document.getElementById("DepDateDescR").value =
          msg2.d[i].FlightDepDate1;
        document.getElementById("DepTimeDescR").value =
          msg2.d[i].FlightDepTime1;
      }

      break;
    }
  }

  OnwardAmount = document.getElementById("HiddenOnwardAmount").value;
  OnwardAmount_Final = document.getElementById(
    "HiddenOnwardAmount_FinalFare"
  ).value;

  total = parseInt(OnwardAmount) + parseInt(ReturnAmount);
  total_Final = parseInt(OnwardAmount_Final) + parseInt(ReturnAmount_Final);

  $("#flightSelectAmount").html("");
  var html = [];
  html.push("<table><tr>");
  html.push("<td style='padding:4px'> <div class='mainkfd Dkay1'>");
  html.push(
    "<span class='totalamount'>" +
      $("#hdncurrencytype").val() +
      " <span class='einr' style='color:#000;font-weight:bold;text-decoration: line-through red;' id='myTargetElement'> </span>    <span class='actu' id='myTargetElement_Final'> </span>  </span> <a  href='javascript:void(0);' class='book_now_button'  onclick='BookNow()' > Book </a> </div> </td>"
  );
  html.push("</tr></table>");
  $("#flightSelectAmount").append(html.join(""));
  var options = {
    useEasing: true, // toggle easing
    useGrouping: true, // 1,000,000 vs 1000000
    separator: ",", // character to use as a separator
    decimal: ".", // character to use as a decimal
  };
  var useOnComplete = false;
  var useEasing = true;
  var useGrouping = true;
  var demo = new countUp("myTargetElement", 0, total, 0, 0, options);
  demo.start();

  var demo_Final = new countUp(
    "myTargetElement_Final",
    0,
    total_Final,
    0,
    0,
    options
  );
  demo_Final.start();

  $("#flightSelectAmount").fadeTo(100, 0.1).fadeTo(200, 1.0);
  //$("#flightSelectInbound").toggle("bounce", { times: 1 }, "fast");
  //$("#flightSelectInbound").show("bounce", { times: 1 }, "fast");
  chkunchk();
}
window.flightchangeR = flightchangeR;
function emailtofriend() {
  $("#datashowConcernModifyouter").css("display", "Block");
  $("#FlightPopUpEmail").css("display", "Block");
  jQuery("#FlightInfoSMS").html("");
  jQuery("#FlightInfoEmail").html("");
  var msg = sortF;
  for (var i = 0; i < msg.d.length; i++) {
    $("#TemplateFlightInfoEmail").tmpl(msg.d[0]).appendTo("#FlightInfoEmail");
    break;
  }
}

function CloseFlightPopUpEmail() {
  $("#datashowConcernModifyouter").css("display", "none");
  $("#FlightPopUpEmail").css("display", "none");
}

function CloseFlightfilter() {
  $("#FilterYourSearchDiv").css("display", "none");
}

function SendSMSOpen() {
  $("#datashowConcernModifyouter").css("display", "Block");
  $("#FlightPopUpEmail").css("display", "Block");
  jQuery("#FlightInfoEmail").html("");
  jQuery("#FlightInfoSMS").html("");
  var msg = sortF;
  for (var i = 0; i < msg.d.length; i++) {
    $("#TemplateFlightInfoSMS").tmpl(msg.d[0]).appendTo("#FlightInfoSMS");
    break;
  }
}
function isNumberKey(evt) {
  var charCode = evt.which ? evt.which : event.keyCode;
  if (charCode > 31 && (charCode < 48 || charCode > 57)) return false;
  return true;
}
function SendSMS() {
  var senderMobile = document.getElementById("senderMobile").value;
  var refid = document.getElementById("HiddenRefOnward").value;
  var refidR = document.getElementById("HiddenRefReturn").value;
  if (senderMobile == "") {
    alert("Please Enter Mobile No");
    return false;
  } else if (senderMobile.length < 10) {
    alert("Please Enter 10 digits Mobile No ");
    return false;
  } else {
    $.ajax({
      type: "POST",
      url: "/flight/SubmitSMSR",
      data:
        '{Mobile:"' +
        senderMobile +
        '",RefId:"' +
        document.getElementById("HiddenRefOnward").value +
        '",RefIdR:"' +
        document.getElementById("HiddenRefReturn").value +
        '"}',
      contentType: "application/json; charset=utf-8",
      dataType: "json",
      success: function (msg) {
        if (msg.d == "1") {
          document.getElementById("senderMobile").value = "";
          alert("SMS is sent successfully  ");
          return true;
        } else {
          alert("SMS is not sent  ");
          return false;
        }
      },
      error: function (msg) {},
    });
  }
}
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
      } else if (msg.d[0].msgs == "1") {
        $("#checkmark" + refid + "").css("display", "inline");
      } else {
        $("#Close" + refid + "").css("display", "inline");
      }
    },
    error: function (msg) {
      $("#FlightPopUp").css("display", "none");
      $("#datashowConcernModifyouter").css("display", "none");
      $("#Questionmark" + refid + "").css("display", "inline");
    },
  });
}
function PromoCheckR(refid) {
  $("#PromoR" + refid + "").css("disabled", "disabled");
  $("#QuestionmarkR" + refid + "").css("display", "none");
  $("#checkmarkR" + refid + "").css("display", "none");
  $("#CloseR" + refid + "").css("display", "none");

  $("#datashowConcernModifyouter").css("display", "Block");
  $("#FlightPopUp").css("display", "Block");
  $.ajax({
    type: "POST",
    url: "/flight/SelectFlightPromoCheckR",
    data: '{refid:"' + refid + '"}',
    contentType: "application/json; charset=utf-8",
    dataType: "json",
    success: function (msg) {
      $("#FlightPopUp").css("display", "none");
      $("#datashowConcernModifyouter").css("display", "none");

      if (msg.d[0].msgs == "2") {
        GetShow();
        $("#checkmarkR" + refid + "").css("display", "inline");
      } else if (msg.d[0].msgs == "1") {
        $("#checkmarkR" + refid + "").css("display", "inline");
      } else {
        $("#CloseR" + refid + "").css("display", "inline");
      }
    },
    error: function (msg) {
      $("#FlightPopUp").css("display", "none");
      $("#datashowConcernModifyouter").css("display", "none");
      $("#QuestionmarkR" + refid + "").css("display", "inline");
    },
  });
}

function modifyclk() {
  var ll = document.getElementById("hikjhj");
  if (ll.style.display != "block") {
    ll.style.display = "block";
  } else {
    ll.style.display = "none";
  }
}
function GetFlightStops() {
  jQuery("#stops").html("");
  $.ajax({
    type: "POST",
    url: "/flight/FlightStops",
    data: "{}",
    contentType: "application/json; charset=utf-8",
    dataType: "json",
    success: function (msg) {
      $("#templetestops").tmpl(msg.d).appendTo("#stops");
      $("#templetestops").tmpl(msg.d).appendTo("#stops1");
      GetFlightStopsR();
    },
    error: function (msg) {},
  });
}

function getCheckedF() {
  $("#modelpopupOUTER").css("display", "Block");
  $("#modelpopup").css("background-color", "Gray");
  $("#modelpopup").delay(1).fadeIn(400);
  $("#modelpopupOUTER").delay(1).fadeIn(400);
  var msg2 = sortF;
  var chkboxValue = new Array();
  $("ul.amenView")
    .find("input:checkbox")
    .each(function () {
      if (this.checked == true) {
        var v = $(this).val();
        chkboxValue.push(v);
      }
    });
  if (chkboxValue.length == 0) {
    jQuery("#tempView").html("");
  } else {
    var c = 0;
    jQuery("#tempView").html("");
    for (c; c < chkboxValue.length; c++) {
      var msg = sortF;
      for (var i = 0; i < msg.d.length; i++) {
        if (chkboxValue[c] == msg.d[i].Stop) {
          $("#MyTemplate").tmpl(msg.d[i]).appendTo("#tempView");
        }
      }
    }
  }
  $("#modelpopup").delay(1000).fadeOut(400);
  $("#modelpopupOUTER").delay(1000).fadeOut(400);
  chkunchk();
}

function getCheckedFilters(SortCriteria) {
  $("#modelpopupOUTER").css("display", "Block");
  $("#modelpopup").css("background-color", "Gray");
  $("#modelpopup").delay(1).fadeIn(400);
  $("#modelpopupOUTER").delay(1).fadeIn(400);
  var msg = sortF;
  var chkboxValueStops = new Array();
  var chkboxValueAir = new Array();
  $("ul.amenView")
    .find("input:checkbox")
    .each(function () {
      if (this.checked == true) {
        var v = $(this).val();
        chkboxValueStops.push(v);
      }
    });

  $("ul.amenViewairline")
    .find("input:checkbox")
    .each(function () {
      if (this.checked == true) {
        var v = $(this).val();
        chkboxValueAir.push(v);
      }
    });

  var groupByFlightNumber = getSortedDetails(msg, SortCriteria, "I");

  if (chkboxValueStops.length == 0 || chkboxValueAir.length == 0) {
    //jQuery('#tempView').html('');
    Object.keys(groupByFlightNumber).forEach(function (key) {
      jQuery("#tempView").html("");
      jQuery("#Price3-" + key).html("");
      jQuery("#DR-" + key).html("");
    });
  } else {
    //var c = 0;
    // jQuery('#tempView').html('');
    Object.keys(groupByFlightNumber).forEach(function (key) {
      jQuery("#tempView").html("");
      jQuery("#Price3-" + key).html("");
      jQuery("#DR-" + key).html("");
    });

    var minimum = parseFloat(document.getElementById("hdnminval").value);
    var maximum = parseFloat(document.getElementById("hdnmaxval").value);
    var minideptime = parseFloat(
      document.getElementById("hdnmindeptime").value
    );
    var maxideptime = parseFloat(
      document.getElementById("hdnmaxdeptime").value
    );
    var miniarrtime = parseFloat(
      document.getElementById("hdnminarrtime").value
    );
    var maxiarrtime = parseFloat(
      document.getElementById("hdnmaxarrtime").value
    );

    Object.keys(groupByFlightNumber).forEach(function (key) {
      var group = groupByFlightNumber[key];
      // Check if the group satisfies the filters
      var filteredGroup = group.filter(function (flight) {
        var TotalAmount = parseFloat(flight.TotalAmount);
        var FlightDepTime =
          parseFloat(
            parseFloat(
              flight.FlightDepTime.substring(0, 2) +
                "." +
                flight.FlightDepTime.substring(3, 5)
            )
          ) * 60;
        var FlightArrTime =
          parseFloat(
            parseFloat(
              flight.FlightArrTime.substring(0, 2) +
                "." +
                flight.FlightArrTime.substring(3, 5)
            )
          ) * 60;

        return (
          chkboxValueStops.includes(flight.Stop) &&
          chkboxValueAir.includes(flight.FlightName) &&
          TotalAmount >= minimum &&
          TotalAmount <= maximum &&
          FlightDepTime >= minideptime &&
          FlightDepTime <= maxideptime &&
          FlightArrTime >= miniarrtime &&
          FlightArrTime <= maxiarrtime
        );
      });

      if (filteredGroup.length > 0) {
        // alert('grp')
        $("#MyTemplate").tmpl(filteredGroup[0]).appendTo("#tempView");
        $("#MyTemplatePrice")
          .tmpl(filteredGroup)
          .appendTo("#Price3-" + key);
        $("#MyTemplateDetail")
          .tmpl(filteredGroup)
          .appendTo("#DR-" + key);
      }
    });
  }
  $("#modelpopup").delay(1000).fadeOut(400);
  $("#modelpopupOUTER").delay(1000).fadeOut(400);
  chkunchk();
}
window.getCheckedFilters = getCheckedFilters;

function getCheckedFAirlines() {
  $("#modelpopupOUTER").css("display", "Block");
  $("#modelpopup").css("background-color", "Gray");
  $("#modelpopup").delay(1).fadeIn(400);
  $("#modelpopupOUTER").delay(1).fadeIn(400);

  var msg2 = sortF;
  var chkboxValue = new Array();
  $("ul.amenViewairline")
    .find("input:checkbox")
    .each(function () {
      if (this.checked == true) {
        var v = $(this).val();
        chkboxValue.push(v);
      }
    });
  if (chkboxValue.length == 0) {
    jQuery("#tempView").html("");
  } else {
    var c = 0;
    jQuery("#tempView").html("");
    for (c; c < chkboxValue.length; c++) {
      var msg = sortF;
      for (var i = 0; i < msg.d.length; i++) {
        if (chkboxValue[c] == msg.d[i].FlightName) {
          $("#MyTemplate").tmpl(msg.d[i]).appendTo("#tempView");
        }
      }
    }
  }
  $("#modelpopup").delay(1000).fadeOut(400);
  $("#modelpopupOUTER").delay(1000).fadeOut(400);
  chkunchk();
}
window.getCheckedFAirlines = getCheckedFAirlines;
function GetFlightStopsR() {
  jQuery("#stopsR").html("");

  $.ajax({
    type: "POST",
    url: "/flight/FlightStopsR",
    data: "{}",
    contentType: "application/json; charset=utf-8",
    dataType: "json",
    success: function (msg) {
      $("#templetestopsR").tmpl(msg.d).appendTo("#stopsR");
    },
    error: function (msg) {},
  });
}
window.GetFlightStopsR = GetFlightStopsR;
function getCheckedFiltersR(SortCriteria) {
  $("#modelpopupOUTER").css("display", "Block");
  $("#modelpopup").css("background-color", "Gray");
  $("#modelpopup").delay(1).fadeIn(400);
  $("#modelpopupOUTER").delay(1).fadeIn(400);

  var chkboxValueStops = new Array();
  var chkboxValueAir = new Array();
  $("ul.amenViewR")
    .find("input:checkbox")
    .each(function () {
      if (this.checked == true) {
        var v = $(this).val();
        chkboxValueStops.push(v);
      }
    });

  $("ul.amenViewairlineR")
    .find("input:checkbox")
    .each(function () {
      if (this.checked == true) {
        var v = $(this).val();
        chkboxValueAir.push(v);
      }
    });

  var jsonResponse = sortFR;

  var groupByFlightNumber = groupJson(jsonResponse);
  var groupByFlightNumber = getSortedDetails(jsonResponse, SortCriteria, "R");

  if (chkboxValueStops.length == 0 || chkboxValueAir.length == 0) {
    Object.keys(groupByFlightNumber).forEach(function (key) {
      jQuery("#tempViewR").html("");
      jQuery("#PriceT-" + key).html("");
      jQuery("#DRR-" + key).html("");
    });
  } else {
    Object.keys(groupByFlightNumber).forEach(function (key) {
      jQuery("#tempViewR").html("");
      jQuery("#PriceT-" + key).html("");
      jQuery("#DRR-" + key).html("");
    });

    var minimumr = parseFloat(document.getElementById("hdnminvalr").value);
    var maximumr = parseFloat(document.getElementById("hdnmaxvalr").value);
    var minideptimer = parseFloat(
      document.getElementById("hdnmindeptimer").value
    );
    var maxideptimer = parseFloat(
      document.getElementById("hdnmaxdeptimer").value
    );
    var miniarrtimer = parseFloat(
      document.getElementById("hdnminarrtimer").value
    );
    var maxiarrtimer = parseFloat(
      document.getElementById("hdnmaxarrtimer").value
    );

    Object.keys(groupByFlightNumber).forEach(function (key) {
      var group = groupByFlightNumber[key];
      // Check if the group satisfies the filters
      var filteredGroup = group.filter(function (flight) {
        var TotalAmount = parseFloat(flight.TotalAmount);
        var FlightDepTime =
          parseFloat(
            parseFloat(
              flight.FlightDepTime.substring(0, 2) +
                "." +
                flight.FlightDepTime.substring(3, 5)
            )
          ) * 60;
        var FlightArrTime =
          parseFloat(
            parseFloat(
              flight.FlightArrTime.substring(0, 2) +
                "." +
                flight.FlightArrTime.substring(3, 5)
            )
          ) * 60;

        return (
          chkboxValueStops.includes(flight.Stop) &&
          chkboxValueAir.includes(flight.FlightName) &&
          TotalAmount >= minimumr &&
          TotalAmount <= maximumr &&
          FlightDepTime >= minideptimer &&
          FlightDepTime <= maxideptimer &&
          FlightArrTime >= miniarrtimer &&
          FlightArrTime <= maxiarrtimer
        );
      });

      if (filteredGroup.length > 0) {
        // alert('grp')
        $("#MyTemplateR").tmpl(filteredGroup[0]).appendTo("#tempViewR");
        $("#MyTemplatePriceR")
          .tmpl(filteredGroup)
          .appendTo("#PriceT-" + key);
        $("#MyTemplateDetailR")
          .tmpl(filteredGroup)
          .appendTo("#DRR-" + key);
      }
    });
  }
  $("#modelpopup").delay(1000).fadeOut(400);
  $("#modelpopupOUTER").delay(1000).fadeOut(400);
  chkunchk();
}
window.getCheckedFiltersR = getCheckedFiltersR;
function getCheckedFR() {
  $("#modelpopupOUTER").css("display", "Block");
  $("#modelpopup").css("background-color", "Gray");
  $("#modelpopup").delay(1).fadeIn(400);
  $("#modelpopupOUTER").delay(1).fadeIn(400);
  var msg2 = sortFR;
  var chkboxValue = new Array();
  $("ul.amenViewR")
    .find("input:checkbox")
    .each(function () {
      if (this.checked == true) {
        var v = $(this).val();
        chkboxValue.push(v);
      }
    });
  if (chkboxValue.length == 0) {
    jQuery("#tempViewR").html("");
  } else {
    var c = 0;
    jQuery("#tempViewR").html("");
    for (c; c < chkboxValue.length; c++) {
      var msg = sortFR;
      for (var i = 0; i < msg.d.length; i++) {
        if (chkboxValue[c] == msg.d[i].Stop) {
          $("#MyTemplateR").tmpl(msg.d[i]).appendTo("#tempViewR");
        }
      }
    }
  }
  $("#modelpopup").delay(1000).fadeOut(400);
  $("#modelpopupOUTER").delay(1000).fadeOut(400);
  chkunchk();
}
window.getCheckedFR = getCheckedFR;

function getCheckedFAirlinesR() {
  $("#modelpopupOUTER").css("display", "Block");
  $("#modelpopup").css("background-color", "Gray");
  $("#modelpopup").delay(1).fadeIn(400);
  $("#modelpopupOUTER").delay(1).fadeIn(400);

  var msg2 = sortFR;
  var chkboxValue = new Array();
  $("ul.amenViewairlineR")
    .find("input:checkbox")
    .each(function () {
      if (this.checked == true) {
        var v = $(this).val();
        chkboxValue.push(v);
      }
    });
  if (chkboxValue.length == 0) {
    jQuery("#tempViewR").html("");
  } else {
    var c = 0;
    jQuery("#tempViewR").html("");
    for (c; c < chkboxValue.length; c++) {
      var msg = sortFR;
      for (var i = 0; i < msg.d.length; i++) {
        if (chkboxValue[c] == msg.d[i].FlightName) {
          $("#MyTemplateR").tmpl(msg.d[i]).appendTo("#tempViewR");
        }
      }
    }
  }
  $("#modelpopup").delay(1000).fadeOut(400);
  $("#modelpopupOUTER").delay(1000).fadeOut(400);
  chkunchk();
}
window.getCheckedFAirlinesR = getCheckedFAirlinesR;
function GetFlightPreferAirlines() {
  $.ajax({
    type: "POST",
    url: "/flight/PreferAirlines",
    data: "{}",
    contentType: "application/json; charset=utf-8",
    dataType: "json",
    success: function (msg) {
      jQuery("#AirlinesChk").html("");
      $("#templeteAirlinesOnward").tmpl(msg.d).appendTo("#AirlinesChk");
      GetFlightPreferAirlinesR();
    },
    error: function (msg) {},
  });
}
window.GetFlightPreferAirlines = GetFlightPreferAirlines;
function GetFlightPreferAirlinesR() {
  $.ajax({
    type: "POST",
    url: "/flight/PreferAirlinesR",
    data: "{}",
    contentType: "application/json; charset=utf-8",
    dataType: "json",
    success: function (msg) {
      jQuery("#AirlinesChkR").html("");
      $("#templeteAirlinesOnwardR").tmpl(msg.d).appendTo("#AirlinesChkR");
    },
    error: function (msg) {},
  });
}
window.GetFlightPreferAirlinesR = GetFlightPreferAirlinesR;
$(document).ready(function () {
  $("body").tooltip({
    selector: "[data-toggle='tooltip']",
    container: "body",
  });

  // newspicdata();

  $("#im1").click(function () {
    $("#pop1").hide("slow");
  });

  $("#im2").click(function () {
    $("#pop2").hide("slow");
  });
  $("#im3").click(function () {
    $("#pop3").hide("slow");
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
      $("#Slide1").html("");
      var html = [];
      $("#Slide2").html("");
      var html2 = [];
      $("#Slide3").html("");
      var html3 = [];
      if (msg.d.length > 0) {
        for (var i = 0; i < msg.d.length; i++) {
          if (msg.d[i].slide == "1") {
            $("#pop1").show("slow");

            if (msg.d[i].slideorder == "1") {
              html.push("<div id='Slide1Show1' style='display:block'>");
              html.push(
                "<div style='width:280px;padding:5px;height:78px'>" +
                  msg.d[i].slidetext +
                  "</div>"
              );
              if (msg.d[i].slideno > 1) {
                html.push(
                  "<div style='padding:5px'><a href='javascript:VOID(0);' ><img  src='image/left_off.gif' style='width:18px;height:18px;'></a> &nbsp;&nbsp;<a href='javascript:VOID(0);' onclick='msgshowhideRIGHTOne1();'><img  src='image/right.gif' style='width:18px;height:18px;'></a></div>"
                );
              }
              html.push("</div>");
            }
            if (msg.d[i].slideorder == "2") {
              html.push("<div id='Slide1Show2' style='display:none'>");
              html.push(
                "<div style='width:280px;padding:5px;height:78px'>" +
                  msg.d[i].slidetext +
                  "</div>"
              );
              if (msg.d[i].slideno == 2) {
                html.push(
                  "<div style='padding:5px'><a href='javascript:VOID(0);' onclick='msgshowhideLeftOne1();'><img  src='image/left.gif' style='width:18px;height:18px;'></a> &nbsp;&nbsp;<a href='javascript:VOID(0);' ><img src='image/right_off.gif' style='width:18px;height:18px;'></a></div>"
                );
              }
              if (msg.d[i].slideno == 3) {
                html.push(
                  "<div style='padding:5px'><a href='javascript:VOID(0);' onclick='msgshowhideLeftTwo11();'><img  src='image/left.gif' style='width:18px;height:18px;'></a> &nbsp;&nbsp;<a href='javascript:VOID(0);' onclick='msgshowhideRIGHTTwo1();'><img  src='image/right.gif' style='width:18px;height:18px;'></a></div>"
                );
              }
              html.push("</div>");
            }
            if (msg.d[i].slideorder == "3") {
              html.push("<div id='Slide1Show3' style='display:none'>");
              html.push(
                "<div style='width:280px;padding:5px;height:78px'>" +
                  msg.d[i].slidetext +
                  "</div>"
              );
              if (msg.d[i].slideno == 3) {
                html.push(
                  "<div style='padding:5px'><a href='javascript:VOID(0);' onclick='msgshowhideLeftTwo1();' ><img  src='image/left.gif' style='width:18px;height:18px;'></a> &nbsp;&nbsp;<a><img src='image/right_off.gif' style='width:18px;height:18px;'></a></div>"
                );
              }
              html.push("</div>");
            }
          }

          if (msg.d[i].slide == "2") {
            $("#pop2").show("slow");
            if (msg.d[i].slideorder == "1") {
              html2.push("<div  id='Slide2Show1' style='display:block'>");
              html2.push(
                "<div style='width:280px;padding:5px;height:78px'>" +
                  msg.d[i].slidetext +
                  "</div>"
              );
              if (msg.d[i].slideno > 1) {
                html2.push(
                  "<div style='padding:5px'><a href='javascript:VOID(0);' ><img  src='image/left_off.gif' style='width:18px;height:18px;'></a> &nbsp;&nbsp;<a href='javascript:VOID(0);' onclick='msgshowhideRIGHTOne2();'><img  src='image/right.gif' style='width:18px;height:18px;'></a></div>"
                );
              }
              html2.push("</div>");
            }
            if (msg.d[i].slideorder == "2") {
              html2.push("<div id='Slide2Show2' style='display:none'>");
              html2.push(
                "<div style='width:280px;padding:5px;height:78px' >" +
                  msg.d[i].slidetext +
                  "</div>"
              );
              if (msg.d[i].slideno == 2) {
                html2.push(
                  "<div style='padding:5px'><a href='javascript:VOID(0);' onclick='msgshowhideLeftOne2();'><img  src='image/left.gif' style='width:18px;height:18px;'></a> &nbsp;&nbsp;<a href='javascript:VOID(0);' ><img  src='image/right_off.gif' style='width:18px;height:18px;'></a></div>"
                );
              }
              if (msg.d[i].slideno == 3) {
                html2.push(
                  "<div style='padding:5px'><a href='javascript:VOID(0);' onclick='msgshowhideLeftTwo22();'><img  src='image/left.gif' style='width:18px;height:18px;'></a> &nbsp;&nbsp;<a href='javascript:VOID(0);' onclick='msgshowhideRIGHTTwo2();'><img  src='image/right.gif' style='width:18px;height:18px;'></a></div>"
                );
              }
              html2.push("</div>");
            }
            if (msg.d[i].slideorder == "3") {
              html2.push("<div id='Slide2Show3' style='display:none'>");
              html2.push(
                "<div style='width:280px;padding:5px;height:78px'>" +
                  msg.d[i].slidetext +
                  "</div>"
              );
              if (msg.d[i].slideno == 3) {
                html2.push(
                  "<div style='padding:5px'><a href='javascript:VOID(0);' onclick='msgshowhideLeftTwo2();'><img  src='image/left.gif' style='width:18px;height:18px;'></a> &nbsp;&nbsp;<a href='javascript:VOID(0);'><img  src='image/right_off.gif' style='width:18px;height:18px;'></a></div>"
                );
              }
              html2.push("</div>");
            }
          }

          if (msg.d[i].slide == "3") {
            $("#pop3").show("slow");
            if (msg.d[i].slideorder == "1") {
              html3.push("<div id='Slide3Show1' style='display:block'>");
              html3.push(
                "<div style='width:280px;padding:5px;height:78px'>" +
                  msg.d[i].slidetext +
                  "</div>"
              );
              if (msg.d[i].slideno > 1) {
                html3.push(
                  "<div style='padding:5px'><a href='javascript:VOID(0);' ><img  src='image/left_off.gif' style='width:18px;height:18px;'></a> &nbsp;&nbsp;<a href='javascript:VOID(0);' onclick='msgshowhideRIGHTOne3();'><img  src='image/right.gif' style='width:18px;height:18px;'></a></div>"
                );
              }
              html3.push("</div>");
            }
            if (msg.d[i].slideorder == "2") {
              html3.push("<div id='Slide3Show2' style='display:none'>");
              html3.push(
                "<div style='width:280px;padding:5px;height:78px'>" +
                  msg.d[i].slidetext +
                  "</div>"
              );
              if (msg.d[i].slideno == 2) {
                html3.push(
                  "<div style='padding:5px'><a href='javascript:VOID(0);' onclick='msgshowhideLeftOne3();'><img  src='image/left.gif' style='width:18px;height:18px;'></a> &nbsp;&nbsp;<a href='javascript:VOID(0);' ><img  src='image/right_off.gif' style='width:18px;height:18px;'></a></div>"
                );
              }
              if (msg.d[i].slideno == 3) {
                html3.push(
                  "<div style='padding:5px'><a href='javascript:VOID(0);' onclick='msgshowhideLeftTwo32();'><img src='image/left.gif' style='width:18px;height:18px;'></a> &nbsp;&nbsp;<a href='javascript:VOID(0);' onclick='msgshowhideRIGHTTwo3();'><img  src='image/right.gif' style='width:18px;height:18px;'></a></div>"
                );
              }
              html3.push("</div>");
            }
            if (msg.d[i].slideorder == "3") {
              html3.push("<div id='Slide3Show3' style='display:none'>");
              html3.push(
                "<div style='width:280px;padding:5px;height:78px'>" +
                  msg.d[i].slidetext +
                  "</div>"
              );
              if (msg.d[i].slideno == 3) {
                html3.push(
                  "<div style='padding:5px'><a href='javascript:VOID(0);' onclick='msgshowhideLeftTwo3();'><img  src='image/left.gif' style='width:18px;height:18px;'></a> &nbsp;&nbsp;<a href='javascript:VOID(0);'><img  src='image/right_off.gif' style='width:18px;height:18px;'></a></div>"
                );
              }
              html3.push("</div>");
            }
          }
        }
      }

      $("#Slide1").append(html.join(""));
      $("#Slide2").append(html2.join(""));
      $("#Slide3").append(html3.join(""));
    },
    error: function (msg) {},
  });
}

function msgshowhideLeftOne1() {
  $("#Slide1Show2").hide();
  $("#Slide1Show1").show();
}
function msgshowhideLeftTwo1() {
  $("#Slide1Show3").hide();
  $("#Slide1Show2").show();
}
function msgshowhideLeftTwo11() {
  $("#Slide1Show2").hide();
  $("#Slide1Show1").show();
}
function msgshowhideLeftOne2() {
  $("#Slide2Show2").hide();
  $("#Slide2Show1").show();
}
function msgshowhideLeftTwo2() {
  $("#Slide2Show3").hide();
  $("#Slide2Show2").show();
}
function msgshowhideLeftTwo22() {
  $("#Slide2Show2").hide();
  $("#Slide2Show1").show();
}
function msgshowhideLeftOne3() {
  $("#Slide3Show2").hide();
  $("#Slide3Show1").show();
}
function msgshowhideLeftTwo3() {
  $("#Slide3Show3").hide();
  $("#Slide3Show2").show();
}
function msgshowhideLeftTwo32() {
  $("#Slide3Show2").hide();
  $("#Slide3Show1").show();
}

function msgshowhideRIGHTOne1() {
  $("#Slide1Show1").hide();
  $("#Slide1Show2").show();
}
function msgshowhideRIGHTTwo1() {
  $("#Slide1Show2").hide();
  $("#Slide1Show3").show();
}
function msgshowhideRIGHTOne2() {
  $("#Slide2Show1").hide();
  $("#Slide2Show2").show();
}
function msgshowhideRIGHTTwo2() {
  $("#Slide2Show2").hide();
  $("#Slide2Show3").show();
}

function msgshowhideRIGHTOne3() {
  $("#Slide3Show1").hide();
  $("#Slide3Show2").show();
}
function msgshowhideRIGHTTwo3() {
  $("#Slide3Show2").hide();
  $("#Slide3Show3").show();
}

function clickfiterfn() {
  $("#FilterYourSearchDiv").show();
}
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
