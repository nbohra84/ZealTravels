var flagSort = "N";
var SortCriteria = "N";
$(document).ready(function () {
  $("body").tooltip({
    selector: "[data-toggle='tooltip']",
    container: "body",
  });
  var data = document.getElementsByClassName("item");
  var $item = $("li.item"), //Cache your DOM selector
    visible = 10, //Set the number of items that will be visible
    index = 0, //Starting index
    endIndex = $item.length;
  var indexto = 0;
  $("div#arrowR").click(function () {
    ($item = $("li.item")), //Cache your DOM selector
      (visible = 10), //Set the number of items that will be visible
      (index = 0), //Starting index
      (endIndex = $item.length);
    if (index < endIndex) {
      if (indexto < endIndex) {
        index++;
        indexto++;
        $item.animate({ left: "-=300px" });
      }
    }
  });

  $("div#arrowL").click(function () {
    ($item = $("li.item")), //Cache your DOM selector
      (visible = 10), //Set the number of items that will be visible
      (index = 0), //Starting index
      (endIndex = $item.length);
    if (indexto > 0) {
      // index--;
      indexto--;
      $item.animate({ left: "+=300px" });
    }
  });
});

function filterclck() {
  var ll = document.getElementById("fare_type");
  if (ll.style.display != "block") {
    ll.style.display = "block";
  } else {
    ll.style.display = "none";
  }
}
window.filterclck = filterclck;

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
  jQuery("#tempView").html("");
  for (var i = 0; i < sortFCurrency.d.length; i++) {
    $("#MyTemplate").tmpl(sortFCurrency.d[i]).appendTo("#tempView");
  }
  var msg = sortF;
  var j = msg.d.length;
  for (i = 0; i < j; i++) {
    var FlightRefid = "spanAmount" + msg.d[i].FlightRefid;
    var amount = parseFloat(msg.d[i].TotalAmount) * parseFloat(currencyAmount);
    if (currency == "INR") {
      document.getElementById(FlightRefid).innerHTML =
        currency + " " + msg.d[i].FlightAmount;
      document.getElementById("Total" + msg.d[i].FlightRefid).innerHTML =
        currency + " " + msg.d[i].FlightAmount;
      document.getElementById(
        "FarebreakupDiv" + msg.d[i].FlightRefid
      ).style.display = "block";
      document.getElementById("ServiceFee" + msg.d[i].FlightRefid).innerHTML =
        currency + " " + msg.d[i].ServiceFee;
      document.getElementById("AHC" + msg.d[i].FlightRefid).innerHTML =
        currency + " " + msg.d[i].AHC;
      document.getElementById("SF" + msg.d[i].FlightRefid).innerHTML =
        currency + " " + msg.d[i].SF;
      document.getElementById("TAXFEE" + msg.d[i].FlightRefid).innerHTML =
        currency + " " + msg.d[i].TaxAmount;
      document.getElementById("BaseFare" + msg.d[i].FlightRefid).innerHTML =
        currency + " " + msg.d[i].BaseAmount;

      document.getElementById("stopsdiv").style.pointerEvents = "auto";
      document.getElementById("PreferredAirlinesdiv").style.pointerEvents =
        "auto";
      document.getElementById("sliderrangediv").style.pointerEvents = "auto";
      document.getElementById("AirlinesChkdiv").style.pointerEvents = "auto";
      document.getElementById(
        "DepartureTimes-slider-rangediv"
      ).style.pointerEvents = "auto";
      document.getElementById(
        "ArrivalTimes-slider-rangediv"
      ).style.pointerEvents = "auto";
      document.getElementById("titleHeaddiv").style.pointerEvents = "auto";
      document.getElementById("FlightMatrixsdiv").style.pointerEvents = "auto";
    } else {
      document.getElementById("FlightMatrixsdiv").style.pointerEvents = "none";
      document.getElementById("stopsdiv").style.pointerEvents = "none";
      document.getElementById("PreferredAirlinesdiv").style.pointerEvents =
        "none";
      document.getElementById("sliderrangediv").style.pointerEvents = "none";
      document.getElementById("AirlinesChkdiv").style.pointerEvents = "none";
      document.getElementById(
        "DepartureTimes-slider-rangediv"
      ).style.pointerEvents = "none";
      document.getElementById(
        "ArrivalTimes-slider-rangediv"
      ).style.pointerEvents = "none";
      document.getElementById("titleHeaddiv").style.pointerEvents = "none";

      var ftotal;
      var fServiceFee;
      var fAHC;
      var fSF;
      var fTaxAmount;
      var fBaseAmount;

      if (msg.d[i].AHC.indexOf(",") > -1) {
        fAHC = parseInt(
          parseFloat(msg.d[i].AHC.replace(/\,/g, "")) *
            parseFloat(currencyAmount)
        );
        document.getElementById("AHC" + msg.d[i].FlightRefid).innerHTML =
          currency + " " + fAHC;
      } else {
        fAHC = parseInt(parseFloat(msg.d[i].AHC) * parseFloat(currencyAmount));
        document.getElementById("AHC" + msg.d[i].FlightRefid).innerHTML =
          currency + " " + fAHC;
      }
      if (msg.d[i].ServiceFee.indexOf(",") > -1) {
        fServiceFee = parseInt(
          parseFloat(msg.d[i].ServiceFee.replace(/\,/g, "")) *
            parseFloat(currencyAmount)
        );
        document.getElementById("ServiceFee" + msg.d[i].FlightRefid).innerHTML =
          currency + " " + fServiceFee;
      } else {
        fServiceFee = parseInt(
          parseFloat(msg.d[i].ServiceFee) * parseFloat(currencyAmount)
        );
        document.getElementById("ServiceFee" + msg.d[i].FlightRefid).innerHTML =
          currency + " " + fServiceFee;
      }
      if (msg.d[i].SF.indexOf(",") > -1) {
        fSF = parseInt(
          parseFloat(msg.d[i].SF.replace(/\,/g, "")) *
            parseFloat(currencyAmount)
        );
        document.getElementById("SF" + msg.d[i].FlightRefid).innerHTML =
          currency + " " + fSF;
      } else {
        fSF = parseInt(parseFloat(msg.d[i].SF) * parseFloat(currencyAmount));
        document.getElementById("SF" + msg.d[i].FlightRefid).innerHTML =
          currency + " " + fSF;
      }

      if (msg.d[i].TaxAmount.indexOf(",") > -1) {
        fTaxAmount = parseInt(
          parseFloat(msg.d[i].TaxAmount.replace(/\,/g, "")) *
            parseFloat(currencyAmount)
        );
        document.getElementById("TAXFEE" + msg.d[i].FlightRefid).innerHTML =
          currency + " " + fTaxAmount;
      } else {
        fTaxAmount = parseInt(
          parseFloat(msg.d[i].TaxAmount) * parseFloat(currencyAmount)
        );
        document.getElementById("TAXFEE" + msg.d[i].FlightRefid).innerHTML =
          currency + " " + fTaxAmount;
      }
      if (msg.d[i].BaseAmount.indexOf(",") > -1) {
        fBaseAmount = parseInt(
          parseFloat(msg.d[i].BaseAmount.replace(/\,/g, "")) *
            parseFloat(currencyAmount)
        );
        document.getElementById("BaseFare" + msg.d[i].FlightRefid).innerHTML =
          currency + " " + fBaseAmount;
      } else {
        fBaseAmount = parseInt(
          parseFloat(msg.d[i].BaseAmount) * parseFloat(currencyAmount)
        );
        document.getElementById("BaseFare" + msg.d[i].FlightRefid).innerHTML =
          currency + " " + fBaseAmount;
      }
      ftotal = parseInt(fServiceFee + fAHC + fSF + fTaxAmount + fBaseAmount);
      document.getElementById("Total" + msg.d[i].FlightRefid).innerHTML =
        currency + " " + parseInt(ftotal);
      document.getElementById(FlightRefid).innerHTML =
        currency + " " + parseInt(ftotal);
      document.getElementById(
        "FarebreakupDiv" + msg.d[i].FlightRefid
      ).style.display = "none";
    }
  }
}

function FareSummaryopen(data) {
  $("#" + data).slideToggle();
}
function FareSummaryclose(data) {
  $("#" + data).css("display", "none");
}
window.FareSummaryopen = FareSummaryopen;
window.FareSummaryclose = FareSummaryclose;
function showtaxandCharges(FlightRefid) {
  $("#TaxAndChargesDIVd" + FlightRefid).css("display") == "none"
    ? $("#TaxAndChargesDIVd" + FlightRefid).slideDown()
    : $("#TaxAndChargesDIVd" + FlightRefid).slideUp();
  $("#TotalTaxDIVd" + FlightRefid).slideUp();
  $("#CommDetailDIVd" + FlightRefid).slideUp();
}
function showtotaltax(FlightRefid) {
  $("#TotalTaxDIVd" + FlightRefid).css("display") == "none"
    ? $("#TotalTaxDIVd" + FlightRefid).slideDown()
    : $("#TotalTaxDIVd" + FlightRefid).slideUp();
  $("#CommDetailDIVd" + FlightRefid).slideUp();
}
function showCommDetail(FlightRefid) {
  $("#CommDetailDIVd" + FlightRefid).css("display") == "none"
    ? $("#CommDetailDIVd" + FlightRefid).slideDown()
    : $("#CommDetailDIVd" + FlightRefid).slideUp();
  $("#TotalTaxDIVd" + FlightRefid).slideUp();
}

function isOfferVisible() {
  // Add your logic here to determine whether the offer price should be visible or not
  // Replace the following line with your actual logic
  // For now, let's assume you want to show the offer price when the "Offer" checkbox is checked
  return document.getElementById("chkdiscccfare").checked;
}

function chkunchk() {
  const offerPriceContainer = document.querySelector(".offer-price");

  if (isOfferVisible()) {
    // Show the offer price by adding the "visible" class
    offerPriceContainer.classList.add("visible");
  } else {
    // Hide the offer price by removing the "visible" class
    offerPriceContainer.classList.remove("visible");
  }
}
//function chkunchk() {
//

//    if (document.getElementById("chkdiscccfare").checked == true) {
//       // Filteronofferfare();
//        $('.discui').css({ display: 'block' });
//        $('.actu').css({ display: 'block' })
//        $(".einr").css('text-decoration', 'line-through red');
//        $(".einr").css('font-size', '15px');

//    }
//    else {
//        $('.discui').css({ display: 'none' });
//        $('.actu').css({ display: 'none' })
//        $(".einr").css('text-decoration', '');

//    }
//}--%>

function showprogerss() {}

(function ($) {
  $(window).load(function () {
    $("#content-2").mCustomScrollbar({
      horizontalScroll: true,
      callbacks: {
        onScroll: function () {
          $("." + this.attr("id") + "-pos").text(mcs.left);
        },
      },
    });
  });
})(jQuery);

var min;
var max;

//function modified to retrieve maximum and minimum for price slider

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
      msg = sortF;
      jQuery("#tempView").html("");
      for (var k = 0; k < flagPreferAirlines.d.length; k++) {
        var asd = "span" + flagPreferAirlines.d[k].Airlines;
        document.getElementById(asd).style.display = "none";
      }

      getCheckedFilters(flagSort);
      //chkunchk();
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
}

//function modified to retrieve maximum and minimum for Departure for gap of 30 mins

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
      var msg = sortF;
      jQuery("#tempView").html("");
      for (var k = 0; k < flagPreferAirlines.d.length; k++) {
        var asd = "span" + flagPreferAirlines.d[k].Airlines;
        document.getElementById(asd).style.display = "none";
      }
      document.getElementById("hdnmindeptime").value = ui.values[0];
      document.getElementById("hdnmaxdeptime").value = ui.values[1];
      getCheckedFilters(flagSort);
      //chkunchk();
    },
  });
  $("#amountDepartureTimes").val("Time:" + "00:00" + "-Time " + "24:00");
}
window.GetMinMaxDepartureTime = GetMinMaxDepartureTime;

//function modified to retrieve maximum and minimum for Arrival for gap of 30 mins

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
      for (var k = 0; k < flagPreferAirlines.d.length; k++) {
        var asd = "span" + flagPreferAirlines.d[k].Airlines;
        document.getElementById(asd).style.display = "none";
      }
      document.getElementById("hdnminarrtime").value = ui.values[0];
      document.getElementById("hdnmaxarrtime").value = ui.values[1];

      getCheckedFilters(flagSort);

      //chkunchk();
    },
  });
  $("#amountArrivalTimes").val("Time:" + "00:00" + "-Time " + "24:00");
}
window.GetMinMaxArrivalTime = GetMinMaxArrivalTime;

var CurrancyDataMsg;

$(document).ready(function () {
  $("#top").hide();
  GetShow();

  showprogerss();
  $("#top").click(function () {
    $("#top").fadeOut();
    window.scrollTo(0, 0);
  });
  $("#SpanModify").click(function () {
    $("#FlightPopUp").css("display", "none");
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
  //flight booking
  $("#flightBook").click(function () {
    $("#FlightPopUp").css("display", "block");
    $.ajax({
      type: "POST",
      url: "k_one.aspx/Book",
      data: "{}",
      contentType: "application/json; charset=utf-8",
      dataType: "json",
      success: function (msg) {
        if (msg.d[0].msgs == 1) {
          var url =
            "GuestAndExistingLogin.aspx?AgentType=Guest&tabid=" +
            msg.d[0].tabid; //GuestAndExistingLogin GuestLogin.aspx
          $(location).attr("href", url);
        } else if (msg.d[0].msgs == 2) {
          var url = "FlightReview.aspx?tabid=" + msg.d[0].tabid;
          $(location).attr("href", url);
        } else {
          var url = "index.aspx";
          $(location).attr("href", url);
        }
      },
      error: function (msg) {
        var url = "index.aspx";
        $(location).attr("href", url);
      },
    });
  });
  $(window).scroll(function () {
    if ($(this).scrollTop() > 500) {
      $("#top").fadeIn();
    } else {
      $("#top").fadeOut();
    }
  });
  $("#Morning").click(function () {
    ShowMorningFlight();
  });
  $("#AfterNoon").click(function () {
    ShowAfterNoonFlight();
  });
  $("#Night").click(function () {
    ShowNightFlight();
  });
  $("#MIDNight").click(function () {
    ShowMIDNightFlight();
  });
  $(".enablec").click(function () {
    $("#FlightPopUp").css("display", "block");
    $("#datashowConcernModifyouter").css("display", "Block");
  });
});
function ShowMorningFlight() {
  $("#modelpopupOUTER").css("display", "Block");
  $("#modelpopup").css("background-color", "Gray");
  $("#modelpopup").delay(1).fadeIn(400);
  $("#modelpopupOUTER").delay(1).fadeIn(400);
  var msg = sortF;
  jQuery("#tempView").html("");
  for (var i = 0; i < msg.d.length; i++) {
    var FlightDepTime = parseFloat(
      msg.d[i].FlightDepTime.substring(0, 2) +
        msg.d[i].FlightDepTime.substring(3, 5)
    );
    if (FlightDepTime >= 500 && FlightDepTime <= 1200) {
      $("#MyTemplate").tmpl(msg.d[i]).appendTo("#tempView");
    }
  }
  $("#modelpopup").delay(1000).fadeOut(400);
  $("#modelpopupOUTER").delay(1000).fadeOut(400);
  //chkunchk();
}
window.ShowMorningFlight = ShowMorningFlight;

function FareM(Airlinename, Fare) {
  $("#modelpopupOUTER").css("display", "Block");
  $("#modelpopup").css("background-color", "Gray");
  $("#modelpopup").delay(1).fadeIn(400);
  $("#modelpopupOUTER").delay(1).fadeIn(400);

  if (Fare != "---") {
    var msg = sortF;
    jQuery("#tempView").html("");
    for (var i = 0; i < msg.d.length; i++) {
      var FlightDepTime = parseFloat(
        msg.d[i].FlightDepTime.substring(0, 2) +
          msg.d[i].FlightDepTime.substring(3, 5)
      );
      if (
        FlightDepTime >= 500 &&
        FlightDepTime <= 1200 &&
        msg.d[i].FlightName == Airlinename
      ) {
        $("#MyTemplate").tmpl(msg.d[i]).appendTo("#tempView");
      }
    }
  }
  $("#modelpopup").delay(1000).fadeOut(400);
  $("#modelpopupOUTER").delay(1000).fadeOut(400);
  //chkunchk();
}
window.FareM = FareM;
function FareA(Airlinename, Fare) {
  $("#modelpopupOUTER").css("display", "Block");
  $("#modelpopup").css("background-color", "Gray");
  $("#modelpopup").delay(1).fadeIn(400);
  $("#modelpopupOUTER").delay(1).fadeIn(400);
  if (Fare != "---") {
    var msg = sortF;
    jQuery("#tempView").html("");
    for (var i = 0; i < msg.d.length; i++) {
      var FlightDepTime = parseFloat(
        msg.d[i].FlightDepTime.substring(0, 2) +
          msg.d[i].FlightDepTime.substring(3, 5)
      );
      if (
        FlightDepTime >= 1200 &&
        FlightDepTime <= 1800 &&
        msg.d[i].FlightName == Airlinename
      ) {
        $("#MyTemplate").tmpl(msg.d[i]).appendTo("#tempView");
      }
    }
  }
  $("#modelpopup").delay(1000).fadeOut(400);
  $("#modelpopupOUTER").delay(1000).fadeOut(400);
  //chkunchk();
}
window.FareA = FareA;
function FareN(Airlinename, Fare) {
  $("#modelpopupOUTER").css("display", "Block");
  $("#modelpopup").css("background-color", "Gray");
  $("#modelpopup").delay(1).fadeIn(400);
  $("#modelpopupOUTER").delay(1).fadeIn(400);
  if (Fare != "---") {
    var msg = sortF;
    jQuery("#tempView").html("");
    for (var i = 0; i < msg.d.length; i++) {
      var FlightDepTime = parseFloat(
        msg.d[i].FlightDepTime.substring(0, 2) +
          msg.d[i].FlightDepTime.substring(3, 5)
      );
      if (
        FlightDepTime >= 1800 &&
        FlightDepTime <= 2400 &&
        msg.d[i].FlightName == Airlinename
      ) {
        $("#MyTemplate").tmpl(msg.d[i]).appendTo("#tempView");
      }
    }
  }
  $("#modelpopup").delay(1000).fadeOut(400);
  $("#modelpopupOUTER").delay(1000).fadeOut(400);
  //chkunchk();
}
window.FareN = FareN;
function FareMN(Airlinename, Fare) {
  $("#modelpopupOUTER").css("display", "Block");
  $("#modelpopup").css("background-color", "Gray");
  $("#modelpopup").delay(1).fadeIn(400);
  $("#modelpopupOUTER").delay(1).fadeIn(400);
  if (Fare != "---") {
    var msg = sortF;
    jQuery("#tempView").html("");
    for (var i = 0; i < msg.d.length; i++) {
      var FlightDepTime = parseFloat(
        msg.d[i].FlightDepTime.substring(0, 2) +
          msg.d[i].FlightDepTime.substring(3, 5)
      );
      if (
        FlightDepTime >= 0o0 &&
        FlightDepTime <= 500 &&
        msg.d[i].FlightName == Airlinename
      ) {
        $("#MyTemplate").tmpl(msg.d[i]).appendTo("#tempView");
      }
    }
  }
  $("#modelpopup").delay(1000).fadeOut(400);
  $("#modelpopupOUTER").delay(1000).fadeOut(400);
  //chkunchk();
}
window.FareMN = FareMN;

function ShowAfterNoonFlight() {
  $("#modelpopupOUTER").css("display", "Block");
  $("#modelpopup").css("background-color", "Gray");
  $("#modelpopup").delay(1).fadeIn(400);
  $("#modelpopupOUTER").delay(1).fadeIn(400);
  var msg = sortF;
  jQuery("#tempView").html("");
  for (var i = 0; i < msg.d.length; i++) {
    var FlightDepTime = parseFloat(
      msg.d[i].FlightDepTime.substring(0, 2) +
        msg.d[i].FlightDepTime.substring(3, 5)
    );
    if (FlightDepTime >= 1200 && FlightDepTime <= 1800) {
      $("#MyTemplate").tmpl(msg.d[i]).appendTo("#tempView");
    }
  }
  $("#modelpopup").delay(1000).fadeOut(400);
  $("#modelpopupOUTER").delay(1000).fadeOut(400);
  //chkunchk();
}
window.ShowAfterNoonFlight = ShowAfterNoonFlight;
function ShowNightFlight() {
  $("#modelpopupOUTER").css("display", "Block");
  $("#modelpopup").css("background-color", "Gray");
  $("#modelpopup").delay(1).fadeIn(400);
  $("#modelpopupOUTER").delay(1).fadeIn(400);
  var msg = sortF;
  jQuery("#tempView").html("");
  for (var i = 0; i < msg.d.length; i++) {
    var FlightDepTime = parseFloat(
      msg.d[i].FlightDepTime.substring(0, 2) +
        msg.d[i].FlightDepTime.substring(3, 5)
    );
    if (FlightDepTime >= 1800 && FlightDepTime <= 2400) {
      $("#MyTemplate").tmpl(msg.d[i]).appendTo("#tempView");
    }
  }
  $("#modelpopup").delay(1000).fadeOut(400);
  $("#modelpopupOUTER").delay(1000).fadeOut(400);
  //chkunchk();
}
window.ShowNightFlight = ShowNightFlight;
function ShowMIDNightFlight() {
  $("#modelpopupOUTER").css("display", "Block");
  $("#modelpopup").css("background-color", "Gray");
  $("#modelpopup").delay(1).fadeIn(400);
  $("#modelpopupOUTER").delay(1).fadeIn(400);
  var msg = sortF;
  jQuery("#tempView").html("");
  for (var i = 0; i < msg.d.length; i++) {
    var FlightDepTime = parseFloat(
      msg.d[i].FlightDepTime.substring(0, 2) +
        msg.d[i].FlightDepTime.substring(3, 5)
    );
    if (FlightDepTime >= 0o0 && FlightDepTime <= 500) {
      $("#MyTemplate").tmpl(msg.d[i]).appendTo("#tempView");
    }
  }
  $("#modelpopup").delay(1000).fadeOut(400);
  $("#modelpopupOUTER").delay(1000).fadeOut(400);
  //chkunchk();
}
window.ShowMIDNightFlight = ShowMIDNightFlight;
var sortF;
var sortFCurrency;
//var groupByFlightNumber;

function GetShow() {
  $("#waitingload").css("display", "Block");
  $("#waitingloadbox").css("display", "Block");
  var SortCriteria = "N";
  flagSort = "N";
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
        $("#titleHeaddiv").hide();
        $(".container.width1170px").hide();
      } else {
        sortF = null;
        sortF = msg;
        sortFCurrency = msg;
        //jQuery('#tempView').html('');

        var jsonResponse = msg;
        //iterating processed JSON response to get the single flight details
        var groupByFlightNumber = groupJson(jsonResponse);

        Object.keys(groupByFlightNumber).forEach(function (key) {
          //var group = groupByFlightNumber[key];

          //group.forEach(function (flight, index) {
          //    var radioButtonId = "Check" + flight.FlightRefid;
          //    $("#" + radioButtonId).prop('checked', index === 0); // Check only the first radio button in each group
          //});

          //$("#MyTemplate").tmpl(group[0]).appendTo("#tempView");
          //$("#multipleDetails").tmpl(group).appendTo("#Price3-" + key);
          //$("#multipleDetails2").tmpl(group).appendTo("#DR-" + key);

          //$("#MyTemplate").tmpl(groupByFlightNumber[key][0]).appendTo("#tempView");
          //$("#multipleDetails").tmpl(groupByFlightNumber[key]).appendTo("#Price3-" + key);
          //$("#multipleDetails2").tmpl(groupByFlightNumber[key]).appendTo("#DR-" + key);

          // $("#Price3-" + key + " .flight-details:first input[type='radio']").prop('checked', true);
          var group = groupByFlightNumber[key];
          // console.log(key);

          $("#MyTemplate").tmpl(group[0]).appendTo("#tempView");
          $("#multipleDetails")
            .tmpl(group)
            .appendTo("#Price3-" + key);
          $("#multipleDetails2")
            .tmpl(group)
            .appendTo("#DR-" + key);

          // Attach click event to group buttons

          //       var radioButtonId = "Check" + group[0].FlightRefid;

          // $("#" + radioButtonId).prop('checked', true);
          //   $("#" + radioButtonId).prop('checked', true);
          //alert(radioButtonId);
          //  $("input[name='refid'][value='" + group[0].FlightRefid + "']").prop('checked', true);
          // Inside the loop for each flight group
          //var firstRadioButton = $("#Price3-" + key + " .flight-details:first input[type='radio']");
          //firstRadioButton.prop('checked', true);
        });

        GetMinMax();
        GetMinMaxDepartureTime();
        GetMinMaxArrivalTime();
        GetFlightPreferAirlines();
        GetFlightStops();
        GetFlightMatrix();
        //$("#waitingload").css("display", "none");
        //$("#waitingloadbox").css("display", "none");

        var cmpid1 = document.getElementById("hdncmpid").value;
        if (cmpid1.indexOf("C-") > -1 || cmpid1 === "") {
          document.getElementById("chkdiscccfare").checked = true;
          $(".hidchk2").css({ display: "none" });
          $(".hidtdscus").css({ display: "none" });
        }

        // chkunchk();
        //                document.getElementById("fare_type").style.border = "0px solid #ff0000";
        //                $("#fare_type").animate({ borderWidth: "1px" }, 500);
        //                $("#fare_type").animate({ borderWidth: "0" }, 0);
      }
    },
    error: function (msg) {},
  });
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
      jQuery("#PreferredAirlines").html("");
      jQuery("#AirlinesChk").html("");
      $("#templeteAirlinesOnward").tmpl(msg.d).appendTo("#AirlinesChk");
      $("#templetePreferredAirlinesOnward")
        .tmpl(msg.d)
        .appendTo("#PreferredAirlines");
    },
    error: function (msg) {},
  });
}
function getCheckedAIRCheckOnward(Airline, spanAirline) {
  $("#modelpopupOUTER").css("display", "Block");
  $("#modelpopup").css("background-color", "Gray");
  $("#modelpopup").delay(1).fadeIn(400);
  $("#modelpopupOUTER").delay(1).fadeIn(400);
  for (var j = 0; j < flagPreferAirlines.d.length; j++) {
    var asd = "span" + flagPreferAirlines.d[j].Airlines;
    document.getElementById(asd).style.display = "none";
  }
  document.getElementById(spanAirline).style.display = "inline";
  jQuery("#tempView").html("");
  var msg = sortF;
  for (var i = 0; i < msg.d.length; i++) {
    if (msg.d[i].FlightName == Airline) {
      $("#MyTemplate").tmpl(msg.d[i]).appendTo("#tempView");
    }
  }
  $("#modelpopup").delay(1000).fadeOut(400);
  $("#modelpopupOUTER").delay(1000).fadeOut(400);
  //chkunchk();
}
window.getCheckedAIRCheckOnward = getCheckedAIRCheckOnward;
function getAll(ck) {
  $("#modelpopupOUTER").css("display", "Block");
  $("#modelpopup").css("background-color", "Gray");
  $("#modelpopup").delay(1).fadeIn(400);
  $("#modelpopupOUTER").delay(1).fadeIn(400);
  //for (var j = 0; j < flagPreferAirlines.d.length; j++) {
  //    var asd = "span" + flagPreferAirlines.d[j].Airlines;
  //    document.getElementById(asd).style.display = 'none';
  //}
  //   jQuery('#tempView').html('');
  if (ck == "CHECK") {
    $("ul.amenViewairline")
      .find("input:checkbox")
      .each(function () {
        this.checked = true;
      });
  } else {
    $("ul.amenViewairline")
      .find("input:checkbox")
      .each(function () {
        {
          this.checked = false;
        }
      });
  }

  getCheckedFilters(flagSort);
  //chkunchk();
}
window.getAll = getAll;

//function BookNow(refid) {

//    {
//        //document.getElementById("Hidden1email").value = refid;
//        jQuery('#divSelectFlight').html('');
//        $("#book_now_button" + refid).removeClass("book_now_button");
//        $("#book_now_button" + refid).addClass("book_now_button")
//        $("#datashowConcernModifyouter").css("display", "Block");
//        $("#FlightPopUp").css("display", "Block");
//        var CompnyID = $("#hdncmpid").val();
//        $.ajax({
//            type: "POST",
//            url: "k_one.aspx/SelectFlight",
//            data: '{refid:"' + refid + '",CompanyID:"' + CompnyID + '"}',
//            contentType: "application/json; charset=utf-8",
//            dataType: "json",
//            success: function (msg) {
//
//                document.getElementById("currency").value = "INR";
//                document.getElementById('stopsdiv').style.pointerEvents = 'auto';
//                document.getElementById('PreferredAirlinesdiv').style.pointerEvents = 'auto';
//                document.getElementById('sliderrangediv').style.pointerEvents = 'auto';
//                document.getElementById('AirlinesChkdiv').style.pointerEvents = 'auto';
//                document.getElementById('DepartureTimes-slider-rangediv').style.pointerEvents = 'auto';
//                document.getElementById('ArrivalTimes-slider-rangediv').style.pointerEvents = 'auto';
//                document.getElementById('titleHeaddiv').style.pointerEvents = 'auto';
//                document.getElementById('FlightMatrixsdiv').style.pointerEvents = 'auto';
//                $('#AirlineSort').removeClass('downArrow');
//                $('#AirlineSort').removeClass('upArrow');
//                $('#PriceSort').removeClass('downArrow');
//                $('#PriceSort').removeClass('upArrow');
//                $('#DepartSort').removeClass('downArrow');
//                $('#DepartSort').removeClass('upArrow');
//                $('#ArriveSort').removeClass('downArrow');
//                $('#ArriveSort').removeClass('upArrow');
//                $('#StopSort').removeClass('downArrow');
//                $('#StopSort').removeClass('upArrow');
//                $('#PriceSort').addClass('downArrow');

//                $("#FlightPopUp").css("display", "none");
//                $("#book_now_button" + refid).removeClass("book_now_buttonAdd");
//                $("#book_now_button" + refid).addClass("book_now_button");

//                $("#divpopup").css("display", "Block");
//                if (msg.d[0].F_Status === true) {

//                    $("#TemplatedivSelectFlight").tmpl(msg.d[0]).appendTo("#divSelectFlight");

//                }
//                else {
//                    //var msg = '<b class="arline-dtl">Note.  </b><span style="color:red;">' + msg.d[0].F_Status_Remark + '</span>';
//                    //$('#divSelectFlightnotavail').html(msg);
//                    //$("#flightnotavail").css("display", "Block");

//                    $("#TemplatedivSelectFlightNotfound").tmpl(msg.d[0]).appendTo("#divSelectFlight");

//                }

//                //if (msg.d[0].FareUpdateMsgChek == 1) {
//                //    GetShow();
//                //    $("#FlightPopUp").css("display", "none");
//                //    $("#book_now_button" + refid).removeClass("book_now_buttonAdd");
//                //    $("#book_now_button" + refid).addClass("book_now_button")
//                //    $("#divpopup").css("display", "Block");
//                //    //if ((screen.width == 1024)) {
//                //    //    $("#divpopup").animate({
//                //    //        height: "auto",
//                //    //        width: "70%",
//                //    //    }, 120);
//                //    //}
//                //    //else {
//                //    //    $("#divpopup").animate({
//                //    //        height: "auto",
//                //    //        width: "55%",
//                //    //    }, 120);
//                //    //}
//                //    $("#TemplatedivSelectFlight").tmpl(msg.d[0]).appendTo("#divSelectFlight");
//                //}
//                //else if (msg.d[0].FareUpdateMsgChek == 0) {
//                //    $("#FlightPopUp").css("display", "none");
//                //    $("#book_now_button" + refid).removeClass("book_now_buttonAdd");
//                //    $("#book_now_button" + refid).addClass("book_now_button")
//                //    $("#divpopup").css("display", "Block");
//                //    //if ((screen.width == 1024)) {
//                //    //    $("#divpopup").animate({
//                //    //        height: "auto",
//                //    //        width: "70%",
//                //    //    }, 120);
//                //    //}
//                //    //else {
//                //    //    $("#divpopup").animate({
//                //    //        height: "auto",
//                //    //        width: "46%",
//                //    //        bottom:"0px",
//                //    //    }, 120);
//                //    //}
//                //    $("#TemplatedivSelectFlight").tmpl(msg.d[0]).appendTo("#divSelectFlight");
//                //}
//                //else if (msg.d[0].FareUpdateMsgChek == 2) {
//                //    GetShow();
//                //    $("#FlightPopUp").css("display", "none");
//                //    $("#book_now_button" + refid).removeClass("book_now_buttonAdd");
//                //    $("#book_now_button" + refid).addClass("book_now_button")
//                //    $("#divpopup").css("display", "Block");
//                //    //if ((screen.width == 1024)) {
//                //    //    $("#divpopup").animate({
//                //    //        height: "auto",
//                //    //        width: "70%",
//                //    //    }, 120);
//                //    //}
//                //    //else {
//                //    //    $("#divpopup").animate({
//                //    //        height: "auto",
//                //    //        width: "46%",
//                //    //    }, 120);
//                //    //}
//                //    $("#TemplatedivSelectFlightNotfound").tmpl(msg.d[0]).appendTo("#divSelectFlight");
//                //}
//            },
//            error: function (msg) {
//                $("#FlightPopUp").css("display", "none");
//                $("#datashowConcernModifyouter").css("display", "none");
//                $("#book_now_button" + refid).removeClass("book_now_buttonAdd");
//                $("#book_now_button" + refid).addClass("book_now_button")
//            }

//        });
//    }

//}

// function BookNow have been indroduce to get id of selected radio buttons by Nehal
function BookNow(FlightNumberCmb) {
  var selectedRadioButtonId = getSelectedRadioButtonId(FlightNumberCmb);

  // Proceed with booking using the selectedRadioButtonId
  if (selectedRadioButtonId !== null) {
    refid = selectedRadioButtonId;
    jQuery("#divSelectFlight").html("");
    $("#book_now_button" + refid).removeClass("book_now_button");
    $("#book_now_button" + refid).addClass("book_now_button");
    $("#datashowConcernModifyouter").css("display", "Block");
    $("#FlightPopUp").css("display", "Block");
    var CompnyID = $("#hdncmpid").val();
    $.ajax({
      type: "POST",
      url: "/flight/SelectFlight",
      data: JSON.stringify({ refid: refid, CompanyID: CompnyID }),
      contentType: "application/json; charset=utf-8",
      dataType: "json",
      success: function (msg) {
        document.getElementById("currency").value = "INR";
        document.getElementById("stopsdiv").style.pointerEvents = "auto";
        document.getElementById("PreferredAirlinesdiv").style.pointerEvents =
          "auto";
        document.getElementById("sliderrangediv").style.pointerEvents = "auto";
        document.getElementById("AirlinesChkdiv").style.pointerEvents = "auto";
        document.getElementById(
          "DepartureTimes-slider-rangediv"
        ).style.pointerEvents = "auto";
        document.getElementById(
          "ArrivalTimes-slider-rangediv"
        ).style.pointerEvents = "auto";
        document.getElementById("titleHeaddiv").style.pointerEvents = "auto";
        document.getElementById("FlightMatrixsdiv").style.pointerEvents =
          "auto";
        $("#AirlineSort").removeClass("downArrow");
        $("#AirlineSort").removeClass("upArrow");
        $("#PriceSort").removeClass("downArrow");
        $("#PriceSort").removeClass("upArrow");
        $("#DepartSort").removeClass("downArrow");
        $("#DepartSort").removeClass("upArrow");
        $("#ArriveSort").removeClass("downArrow");
        $("#ArriveSort").removeClass("upArrow");
        $("#StopSort").removeClass("downArrow");
        $("#StopSort").removeClass("upArrow");
        $("#PriceSort").addClass("downArrow");
        $("#FlightPopUp").css("display", "none");
        $("#book_now_button" + refid).removeClass("book_now_buttonAdd");
        $("#book_now_button" + refid).addClass("book_now_button");
        $("#divpopup").css("display", "Block");
        if (msg.d[0].F_Status === true) {
          $("#TemplatedivSelectFlight")
            .tmpl(msg.d[0])
            .appendTo("#divSelectFlight");
        } else {
          //var msg = '<b class="arline-dtl">Note.  </b><span style="color:red;">' + msg.d[0].F_Status_Remark + '</span>';
          //$('#divSelectFlightnotavail').html(msg);
          //$("#flightnotavail").css("display", "Block");

          $("#TemplatedivSelectFlightNotfound")
            .tmpl(msg.d[0])
            .appendTo("#divSelectFlight");
        }
      },
      error: function (msg) {
        $("#FlightPopUp").css("display", "none");
        $("#datashowConcernModifyouter").css("display", "none");
        $("#book_now_button" + refid).removeClass("book_now_buttonAdd");
        $("#book_now_button" + refid).addClass("book_now_button");
      },
    });
  } else {
    alert("Please select a flight to book.");
  }
}
window.BookNow = BookNow;

function getSelectedRadioButtonId(FlightNumberCmb) {
  var radioButtons = document.getElementsByName(`refid${FlightNumberCmb}`);
  for (var i = 0; i < radioButtons.length; i++) {
    if (radioButtons[i].checked) {
      return radioButtons[i].id.split("Check")[1];
    }
  }
  return null; // Return null if no radio button is selected within the group
}
window.getSelectedRadioButtonId = getSelectedRadioButtonId;
function modifyclk() {
  var ll = document.getElementById("hikjhj");
  if (ll.style.display != "block") {
    ll.style.display = "block";
  } else {
    ll.style.display = "none";
  }
}

//function YesToBook() {
//    $.ajax({
//        type: "POST",
//        url: "k_one.aspx/Book",
//        data: "{}",
//        contentType: "application/json; charset=utf-8",
//        dataType: "json",
//        success: function (msg) {
//            if (msg.d[0].msgs == 1) {
//                var url = "GuestAndExistingLogin.aspx?AgentType=Guest&tabid=" + msg.d[0].tabid;//GuestAndExistingLogin GuestLogin
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

function PromoCheck(refid) {
  $("#Promo" + refid + "").css("disabled", "disabled");
  $("#Questionmark" + refid + "").css("display", "none");
  $("#checkmark" + refid + "").css("display", "none");
  $("#Close" + refid + "").css("display", "none");

  $("#datashowConcernModifyouter").css("display", "Block");
  $("#FlightPopUp").css("display", "Block");
  $.ajax({
    type: "POST",
    url: "k_one.aspx/SelectFlightPromoCheck",
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
function SendSMSOpen(refid) {
  $("#FlightPopUp").css("display", "none");
  $("#datashowConcernModifyouter").css("display", "Block");
  $("#FlightPopUpEmail").css("display", "Block");
  jQuery("#FlightInfoEmail").html("");
  jQuery("#FlightInfoSMS").html("");
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
  var charCode = evt.which ? evt.which : event.keyCode;
  if (charCode > 31 && (charCode < 48 || charCode > 57)) return false;
  return true;
}
function SendSMS() {
  var senderMobile = document.getElementById("senderMobile").value;
  if (senderMobile == "") {
    alert("Please Enter Mobile No");
    return false;
  } else if (senderMobile.length < 10) {
    alert("Please Enter 10 digits Mobile No ");
    return false;
  } else {
    $.ajax({
      type: "POST",
      url: "k_one.aspx/SubmitSMS",
      data:
        '{Mobile:"' +
        senderMobile +
        '",RefId:"' +
        document.getElementById("Hidden1email").value +
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
function emailtofriend(refid) {
  $("#FlightPopUp").css("display", "none");
  $("#datashowConcernModifyouter").css("display", "Block");
  $("#FlightPopUpEmail").css("display", "Block");
  jQuery("#FlightInfoSMS").html("");
  jQuery("#FlightInfoEmail").html("");
  var msg = sortF;
  for (var i = 0; i < msg.d.length; i++) {
    if (msg.d[i].FlightRefid == refid) {
      $("#TemplateFlightInfoEmail").tmpl(msg.d[i]).appendTo("#FlightInfoEmail");

      document.getElementById("Hidden1email").value = refid;
      break;
    }
  }
}
// function Sendmail() {
//     //senderTp  emailTp
//     var Name = document.getElementById("senderTp");
//     var EmailId = document.getElementById("emailTp");
//     var refid = document.getElementById("Hidden1email").value;
//     if (Name.value == "") {
//         alert("Please Enter Your Name");
//         return false;
//     }
//     else if (EmailId.value == "") {
//         alert("Please Enter EmailId");
//         return false;
//     }
//     else if (!filter.test(EmailId.value)) {
//         alert("Please Enter Valid EmailId");
//         return false;
//     }
//     else {
//         $.ajax({
//             type: "POST",
//             url: "k_one.aspx/SubmitEmail",
//             data: '{Name:"' + Name.value + '",EmailId:"' + EmailId.value + '",RefId:"' + document.getElementById("Hidden1email").value + '"}',
//             contentType: "application/json; charset=utf-8",
//             dataType: "json",
//             success: function (msg) {
//                 if (msg.d == "1") {
//                     document.getElementById("senderTp").value = "";
//                     document.getElementById("emailTp").value = "";
//                     alert("Mail is sent successfully  ");
//                     $("#datashowConcernModifyouter").css("display", "none");
//                     $("#FlightPopUpEmail").css("display", "none");
//                     return true;

//                 }
//                 else {
//                     alert("Mail is not sent  ");
//                     return false;
//                 }
//             },
//             error: function (msg) {

//             }

//         });
//     }

// }
function CloseFlightPopUpEmail() {
  $("#datashowConcernModifyouter").css("display", "none");
  $("#FlightPopUpEmail").css("display", "none");
}
function Closedivpopup() {
  $("#divpopup").animate(
    {
      //height: "auto",
      //width: "10px",
    },
    2000
  );
  $("#datashowConcernModifyouter").css("display", "none");
  $("#divpopup").css("display", "none");
  // document.getElementById("Hidden1email").value

  //rootdiv${FlightRefid}
  var divbounces = "rootdiv" + document.getElementById("Hidden1email").value;

  $("#" + divbounces).animate({ borderWidth: "1px" }, 500);
  $("#" + divbounces).animate({ borderWidth: "0" }, 0);
}
window.Closedivpopup = Closedivpopup;
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
function fareruledivshow(data) {
  //var Row1;
  //Row1 = "<table width='550' cellpadding='2' cellspacing='0'><tr style='font-weight:bold;background-color: #000;color:#FFF;border-bottom: #dddddd 1px solid;'><td colspan='9' >Fare rules:-</td></tr><tr><td width='550' align='left'><span class='showfontsizeRule'>" + data + "</td></tr>";
  //var RowAdd = "</table>";
  //var divObj = document.getElementById("divFareRules2");
  //divObj.style.display = "block";
  //divObj.left = '0px';
  //divObj.top = '0px';
  //divObj.innerHTML = Row1 + RowAdd;
  //document.getElementById('farekdu').style.display = "block";

  var ll = document.getElementById("farekdu");
  if (ll.style.display != "none") {
    ll.style.display = "none";
    $("#farekdu").text($("#farekdu").html());
  } else {
    ll.style.display = "block";
    $("#farekdu").html($("#farekdu").text());
  }
}
window.fareruledivshow = fareruledivshow;
//function ddivclose2() {
//    var divObj = document.getElementById("divFareRules2");
//    divObj.style.display = "none";
//}

function GetFlightMatrix() {

  jQuery("#FlightMatrixs2").html("");
  jQuery("#FlightMatrixs").html("");
  $.ajax({
    type: "POST",
    url: "/flight/FlightMatrix",
    data: "{}",
    contentType: "application/json; charset=utf-8",
    dataType: "json",
    success: function (msgs) {
      if (msgs.d.length < 10) {
        $("#content-2").css("display", "none");
        // $("#templeteFlightMatrix").tmpl(msgs.d).appendTo("#FlightMatrixs2");
      } else {
        $("#content-2").css("display", "block");
        // $("#templeteFlightMatrix").tmpl(msgs.d).appendTo("#FlightMatrixs2");
      }
      $("#waitingload").css("display", "none");
      $("#waitingloadbox").css("display", "none");
    },
    error: function (msgs) {},
  });
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
    },
    error: function (msg) {},
  });
}

function getCheckedFilters(SortCriteria) {
  $("#modelpopupOUTER").css("display", "Block");
  $("#modelpopup").css("background-color", "Gray");
  $("#modelpopup").delay(1).fadeIn(400);
  $("#modelpopupOUTER").delay(1).fadeIn(400);

  var msg = sortF;
  //  var newmsg = new Array();
  var chkboxValuestops = new Array();
  var chkboxValueairline = new Array();
  $("ul.amenViewairline")
    .find("input:checkbox")
    .each(function () {
      if (this.checked == true) {
        var v = $(this).val();
        chkboxValueairline.push(v);
      }
    });
  $("ul.amenView")
    .find("input:checkbox")
    .each(function () {
      if (this.checked == true) {
        var v = $(this).val();
        chkboxValuestops.push(v);
      }
    });

  var groupByFlightNumber = getSortedDetails(msg, SortCriteria, "I");

  if (chkboxValuestops.length == 0 || chkboxValueairline.length == 0) {
    Object.keys(groupByFlightNumber).forEach(function (key) {
      jQuery("#tempView").html("");
      jQuery("#Price3-" + key).html("");
      jQuery("#DR-" + key).html("");
    });
  } else {
    Object.keys(groupByFlightNumber).forEach(function (key) {
      jQuery("#tempView").html("");
      jQuery("#Price3-" + key).html("");
      jQuery("#DR-" + key).html("");
    });
    //jQuery('#tempView').html('');

    var minimum = parseFloat(document.getElementById("hdnminval").value);
    var maximum = parseFloat(document.getElementById("hdnmaxval").value);
    var minideptime = parseFloat(
      document.getElementById("hdnmindeptime").value
    );
    var maxideptime = parseFloat(
      document.getElementById("hdnmaxdeptime").value
    );
    var miniarrtime = document.getElementById("hdnminarrtime").value;
    var maxiarrtime = document.getElementById("hdnmaxarrtime").value;

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
          chkboxValuestops.includes(flight.Stop) &&
          chkboxValueairline.includes(flight.FlightName) &&
          TotalAmount >= minimum &&
          TotalAmount <= maximum &&
          FlightDepTime >= minideptime &&
          FlightDepTime <= maxideptime &&
          FlightArrTime >= miniarrtime &&
          FlightArrTime <= maxiarrtime
        );
      });

      if (filteredGroup.length > 0) {
        $("#MyTemplate").tmpl(filteredGroup[0]).appendTo("#tempView");
        $("#multipleDetails")
          .tmpl(filteredGroup)
          .appendTo("#Price3-" + key);
        $("#multipleDetails2")
          .tmpl(filteredGroup)
          .appendTo("#DR-" + key);
      }
    });
  }

  $("#modelpopup").delay(1000).fadeOut(400);
  $("#modelpopupOUTER").delay(1000).fadeOut(400);
  //chkunchk();
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
  //chkunchk();
}

window.getCheckedFAirlines = getCheckedFAirlines;

function checkedRefid(event) {
  const radioButtonName = event.target.name;
  alert(`Clicked Radio Button Name: ${radioButtonName}`);
}
window.checkedRefid = checkedRefid;
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

function FARERULE(FlightRefid) {
  // FareRule(FlightRefid);
  $("#ITINERARY" + FlightRefid + "").css("border-bottom", "0px solid #C0C0C0");
  $("#BAGGAGEDETAILS" + FlightRefid + "").css(
    "border-bottom",
    "0px solid #C0C0C0"
  );
  $("#FARERULE" + FlightRefid + "").css("border-bottom", "3px solid #FEBD25");
  $("#divITINERARY" + FlightRefid + "").slideUp();
  $("#Baggagesummery" + FlightRefid + "").slideUp();
  $("#Farerulesummery" + FlightRefid + "").slideDown();
}
window.FARERULE = FARERULE;

function BAGGAGEDETAILS(FlightRefid) {
  // FareRule(FlightRefid);
  $("#ITINERARY" + FlightRefid + "").css("border-bottom", "0px solid #C0C0C0");
  $("#FARERULE" + FlightRefid + "").css("border-bottom", "0px solid #C0C0C0");
  $("#BAGGAGEDETAILS" + FlightRefid + "").css(
    "border-bottom",
    "3px solid #FEBD25"
  );
  $("#divITINERARY" + FlightRefid + "").slideUp();
  $("#Farerulesummery" + FlightRefid + "").slideUp();
  $("#Baggagesummery" + FlightRefid + "").slideDown();
}
window.BAGGAGEDETAILS = BAGGAGEDETAILS;
function ITINERARY(FlightRefid) {
  $("#ITINERARY" + FlightRefid + "").css("border-bottom", "3px solid #FEBD25");
  $("#FARERULE" + FlightRefid + "").css("border-bottom", "0px solid #C0C0C0");
  $("#BAGGAGEDETAILS" + FlightRefid + "").css(
    "border-bottom",
    "0px solid #C0C0C0"
  );
  $("#divITINERARY" + FlightRefid + "").slideDown();
  $("#Farerulesummery" + FlightRefid + "").slideUp();
  $("#Baggagesummery" + FlightRefid + "").slideUp();
}

window.ITINERARY = ITINERARY;

(function ($) {
  $(window).load(function () {
    var content = $("#content_1"),
      autoScrollTimer = 8000,
      autoScrollTimerAdjust,
      autoScroll;
    content.mCustomScrollbar({
      scrollButtons: {
        enable: true,
      },
      callbacks: {
        whileScrolling: function () {
          autoScrollTimerAdjust = (autoScrollTimer * mcs.topPct) / 100;
        },
        onScroll: function () {
          if (this.data("mCS_trigger") === "internal") {
            AutoScrollOff();
          }
        },
      },
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
      if (!timer) {
        timer = autoScrollTimer;
      }
      content.addClass("auto-scrolling-on").mCustomScrollbar("scrollTo", to, {
        scrollInertia: timer,
        scrollEasing: "easeInOutQuad",
      });
      autoScroll = setTimeout(function () {
        if (content.hasClass("auto-scrolling-to-top")) {
          AutoScrollOn("bottom", autoScrollTimer - autoScrollTimerAdjust);
          content
            .removeClass("auto-scrolling-to-top")
            .addClass("auto-scrolling-to-bottom");
        } else {
          AutoScrollOn("top", autoScrollTimerAdjust);
          content
            .removeClass("auto-scrolling-to-bottom")
            .addClass("auto-scrolling-to-top");
        }
      }, timer);
    }
    function AutoScrollOff() {
      clearTimeout(autoScroll);
      content.removeClass("auto-scrolling-on").mCustomScrollbar("stop");
    }
  });
})(jQuery);

(function ($) {
  $(window).load(function () {
    var content = $("#content_3"),
      autoScrollTimer = 8000,
      autoScrollTimerAdjust,
      autoScroll;
    content.mCustomScrollbar({
      scrollButtons: {
        enable: true,
      },
      callbacks: {
        whileScrolling: function () {
          autoScrollTimerAdjust = (autoScrollTimer * mcs.topPct) / 100;
        },
        onScroll: function () {
          if (this.data("mCS_trigger") === "internal") {
            AutoScrollOff();
          }
        },
      },
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
      if (!timer) {
        timer = autoScrollTimer;
      }
      content.addClass("auto-scrolling-on").mCustomScrollbar("scrollTo", to, {
        scrollInertia: timer,
        scrollEasing: "easeInOutQuad",
      });
      autoScroll = setTimeout(function () {
        if (content.hasClass("auto-scrolling-to-top")) {
          AutoScrollOn("bottom", autoScrollTimer - autoScrollTimerAdjust);
          content
            .removeClass("auto-scrolling-to-top")
            .addClass("auto-scrolling-to-bottom");
        } else {
          AutoScrollOn("top", autoScrollTimerAdjust);
          content
            .removeClass("auto-scrolling-to-bottom")
            .addClass("auto-scrolling-to-top");
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

function SetValueInModifyControl() {
  var ModifyType = $("#modifyfaretype").val();
  if (ModifyType == "R" || ModifyType == "RT") {
    $("#OnwayModDiv").addClass("whcolor");
    $("#RTModDiv").addClass("gbcolor");
    $("#DiscRTModDiv").addClass("whcolor");
  } else if (ModifyType == "O") {
    $("#OnwayModDiv").addClass("gbcolor");
    $("#RTModDiv").addClass("whcolor");
    $("#DiscRTModDiv").addClass("whcolor");
  } else if (ModifyType == "DRT") {
    $("#OnwayModDiv").addClass("whcolor");
    $("#RTModDiv").addClass("whcolor");
    $("#DiscRTModDiv").addClass("gbcolor");
  }
  var Date = $("#DepDate").val();
  $("#oricity").val($("#Hiddenoricity").val());
  $("#desticity").val($("#HiddenArrCity").val());
  $("#FlightSearch_txtDepartureDate").val(Date);
  $("#FlightSearch_txtReturnDate").val(Date);
  $("#adult").val($("#numFLAdults").val());
  $("#child").val($("#numFLChildren").val());
  $("#infant").val($("#numFLInfants").val());

  setpreferedairline();
}
window.SetValueInModifyControl = SetValueInModifyControl;

$(document).ready(function () {
  //   newspicdata();

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
//   function newspicdata() {

//       $.ajax({
//           type: "POST",
//           url: "k_one.aspx/news",
//           data: "{}",
//           contentType: "application/json; charset=utf-8",
//           dataType: "json",
//           success: function (msg) {
//               $('#Slide1').html('');
//               var html = [];
//               $('#Slide2').html('');
//               var html2 = [];
//               $('#Slide3').html('');
//               var html3 = [];
//               if (msg.d.length > 0) {

//                   for (var i = 0; i < msg.d.length; i++) {

//                       if (msg.d[i].slide == "1") {
//                           $('#pop1').show('slow');

//                           if (msg.d[i].slideorder == "1") {
//                               html.push("<div id='Slide1Show1' style='display:block'>");
//                               html.push("<div style='width:280px;padding:5px;height:78px'>" + msg.d[i].slidetext + "</div>");
//                               if (msg.d[i].slideno > 1) {
//                                   html.push("<div style='padding:5px'><a href='javascript:VOID(0);' ><img src='image/left_off.gif' style='width:18px;height:18px;'></a> &nbsp;&nbsp;<a href='javascript:VOID(0);' onclick='msgshowhideRIGHTOne1();'><img src='image/right.gif' style='width:18px;height:18px;'></a></div>");
//                               }
//                               html.push("</div>");

//                           }
//                           if (msg.d[i].slideorder == "2") {
//                               html.push("<div id='Slide1Show2' style='display:none'>");
//                               html.push("<div style='width:280px;padding:5px;height:78px'>" + msg.d[i].slidetext + "</div>");
//                               if (msg.d[i].slideno == 2) {
//                                   html.push("<div style='padding:5px'><a href='javascript:VOID(0);' onclick='msgshowhideLeftOne1();'><img src='image/left.gif' style='width:18px;height:18px;'></a> &nbsp;&nbsp;<a href='javascript:VOID(0);' ><img src='image/right_off.gif' style='width:18px;height:18px;'></a></div>");
//                               }
//                               if (msg.d[i].slideno == 3) {
//                                   html.push("<div style='padding:5px'><a href='javascript:VOID(0);' onclick='msgshowhideLeftTwo11();'><img src='image/left.gif' style='width:18px;height:18px;'></a> &nbsp;&nbsp;<a href='javascript:VOID(0);' onclick='msgshowhideRIGHTTwo1();'><img src='image/right.gif' style='width:18px;height:18px;'></a></div>");
//                               }
//                               html.push("</div>");
//                           }
//                           if (msg.d[i].slideorder == "3") {
//                               html.push("<div id='Slide1Show3' style='display:none'>");
//                               html.push("<div style='width:280px;padding:5px;height:78px'>" + msg.d[i].slidetext + "</div>");
//                               if (msg.d[i].slideno == 3) {
//                                   html.push("<div style='padding:5px'><a href='javascript:VOID(0);' onclick='msgshowhideLeftTwo1();' ><img src='image/left.gif' style='width:18px;height:18px;'></a> &nbsp;&nbsp;<a><img src='image/right_off.gif' style='width:18px;height:18px;'></a></div>");
//                               }
//                               html.push("</div>");
//                           }
//                       }

//                       if (msg.d[i].slide == "2") {
//                           $('#pop2').show('slow');
//                           if (msg.d[i].slideorder == "1") {
//                               html2.push("<div  id='Slide2Show1' style='display:block'>");
//                               html2.push("<div style='width:280px;padding:5px;height:78px'>" + msg.d[i].slidetext + "</div>");
//                               if (msg.d[i].slideno > 1) {
//                                   html2.push("<div style='padding:5px'><a href='javascript:VOID(0);' ><img src='image/left_off.gif' style='width:18px;height:18px;'></a> &nbsp;&nbsp;<a href='javascript:VOID(0);' onclick='msgshowhideRIGHTOne2();'><img src='image/right.gif' style='width:18px;height:18px;'></a></div>");
//                               }
//                               html2.push("</div>");
//                           }
//                           if (msg.d[i].slideorder == "2") {
//                               html2.push("<div id='Slide2Show2' style='display:none'>");
//                               html2.push("<div style='width:280px;padding:5px;height:78px' >" + msg.d[i].slidetext + "</div>");
//                               if (msg.d[i].slideno == 2) {
//                                   html2.push("<div style='padding:5px'><a href='javascript:VOID(0);' onclick='msgshowhideLeftOne2();'><img src='image/left.gif' style='width:18px;height:18px;'></a> &nbsp;&nbsp;<a href='javascript:VOID(0);' ><img src='image/right_off.gif' style='width:18px;height:18px;'></a></div>");
//                               }
//                               if (msg.d[i].slideno == 3) {
//                                   html2.push("<div style='padding:5px'><a href='javascript:VOID(0);' onclick='msgshowhideLeftTwo22();'><img src='image/left.gif' style='width:18px;height:18px;'></a> &nbsp;&nbsp;<a href='javascript:VOID(0);' onclick='msgshowhideRIGHTTwo2();'><img src='image/right.gif' style='width:18px;height:18px;'></a></div>");
//                               }
//                               html2.push("</div>");
//                           }
//                           if (msg.d[i].slideorder == "3") {
//                               html2.push("<div id='Slide2Show3' style='display:none'>");
//                               html2.push("<div style='width:280px;padding:5px;height:78px'>" + msg.d[i].slidetext + "</div>");
//                               if (msg.d[i].slideno == 3) {
//                                   html2.push("<div style='padding:5px'><a href='javascript:VOID(0);' onclick='msgshowhideLeftTwo2();'><img src='image/left.gif' style='width:18px;height:18px;'></a> &nbsp;&nbsp;<a href='javascript:VOID(0);'><img src='image/right_off.gif' style='width:18px;height:18px;'></a></div>");
//                               }
//                               html2.push("</div>");
//                           }
//                       }

//                       if (msg.d[i].slide == "3") {
//                           $('#pop3').show('slow');
//                           if (msg.d[i].slideorder == "1") {
//                               html3.push("<div id='Slide3Show1' style='display:block'>");
//                               html3.push("<div style='width:280px;padding:5px;height:78px'>" + msg.d[i].slidetext + "</div>");
//                               if (msg.d[i].slideno > 1) {
//                                   html3.push("<div style='padding:5px'><a href='javascript:VOID(0);' ><img src='image/left_off.gif' style='width:18px;height:18px;'></a> &nbsp;&nbsp;<a href='javascript:VOID(0);' onclick='msgshowhideRIGHTOne3();'><img src='image/right.gif' style='width:18px;height:18px;'></a></div>");
//                               }
//                               html3.push("</div>");
//                           }
//                           if (msg.d[i].slideorder == "2") {
//                               html3.push("<div id='Slide3Show2' style='display:none'>");
//                               html3.push("<div style='width:280px;padding:5px;height:78px'>" + msg.d[i].slidetext + "</div>");
//                               if (msg.d[i].slideno == 2) {
//                                   html3.push("<div style='padding:5px'><a href='javascript:VOID(0);' onclick='msgshowhideLeftOne3();'><img src='image/left.gif' style='width:18px;height:18px;'></a> &nbsp;&nbsp;<a href='javascript:VOID(0);' ><img src='image/right_off.gif' style='width:18px;height:18px;'></a></div>");
//                               }
//                               if (msg.d[i].slideno == 3) {
//                                   html3.push("<div style='padding:5px'><a href='javascript:VOID(0);' onclick='msgshowhideLeftTwo32();'><img src='image/left.gif' style='width:18px;height:18px;'></a> &nbsp;&nbsp;<a href='javascript:VOID(0);' onclick='msgshowhideRIGHTTwo3();'><img src='image/right.gif' style='width:18px;height:18px;'></a></div>");
//                               }
//                               html3.push("</div>");
//                           }
//                           if (msg.d[i].slideorder == "3") {
//                               html3.push("<div id='Slide3Show3' style='display:none'>");
//                               html3.push("<div style='width:280px;padding:5px;height:78px'>" + msg.d[i].slidetext + "</div>");
//                               if (msg.d[i].slideno == 3) {
//                                   html3.push("<div style='padding:5px'><a href='javascript:VOID(0);' onclick='msgshowhideLeftTwo3();'><img src='image/left.gif' style='width:18px;height:18px;'></a> &nbsp;&nbsp;<a href='javascript:VOID(0);'><img src='image/right_off.gif' style='width:18px;height:18px;'></a></div>");
//                               }
//                               html3.push("</div>");
//                           }

//                       }
//                   }

//               }

//               $('#Slide1').append(html.join(''));
//               $('#Slide2').append(html2.join(''));
//               $('#Slide3').append(html3.join(''));

//           },
//           error: function (msg) {

//           }

//       });

//   }

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

$('.dropdown-mul-2').dropdown({
  limitCount: 10,
  searchable: true
});