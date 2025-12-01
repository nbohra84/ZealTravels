function MyFunction() {
  alert("ggetting error!");
}
function basefare() {
  var x = document.getElementById("basefarewr");
  if (x.style.display === "none") {
    x.style.display = "block";
    $("#minusebase").css("display", "block");
    $("#plusebase").css("display", "none");
    $("#bsfar").css("display", "none");
  } else {
    x.style.display = "none";
    $("#minusebase").css("display", "none");
    $("#plusebase").css("display", "block");
    $("#bsfar").css("display", "block");
  }
}
window.basefare = basefare;
function feesurch() {
  var x = document.getElementById("feesurcharge");
  if (x.style.display === "block") {
    x.style.display = "none";
    $("#minusefee").css("display", "none");
    $("#plusefee").css("display", "block");
    $("#feesurcha").css("display", "block");
  } else {
    x.style.display = "block";
    $("#minusefee").css("display", "block");
    $("#plusefee").css("display", "none");
    $("#feesurcha").css("display", "none");
  }
}
window.feesurch = feesurch;
function otherser() {
  var x = document.getElementById("otherservice");
  if (x.style.display === "block") {
    x.style.display = "none";
    $("#minuseoth").css("display", "none");
    $("#pluseoth").css("display", "block");
    $("#otherserv").css("display", "block");
  } else {
    x.style.display = "block";
    $("#minuseoth").css("display", "block");
    $("#pluseoth").css("display", "none");
    $("#otherserv").css("display", "none");
  }
}
window.otherser = otherser;
$(document).ready(function () {
  $("#clickgstdv").click(function () {
    if ($(this).is(":checked")) {
      $("#gstshow").show();
    } else {
      $("#gstshow").hide();
    }
  });
});
function myclipopse() {
  var ll = document.getElementById("closclippo");

  if (ll.style.display != "block") {
    ll.style.display = "block";
    $("#sedax").css("box-shadow", "0 8px 8px rgba(0,0,0,0.5)");
  } else {
    ll.style.display = "none";
    $("#sedax").css("box-shadow", "0 0px 0px rgba(0,0,0,0.5)");
  }
}
window.myclipopse = myclipopse;

$(document).ready(function () {
  var touch = $("#resp-menu");
  var menu = $(".menu");

  $(touch).on("click", function (e) {
    e.preventDefault();
    menu.slideToggle();
  });

  $(window).resize(function () {
    var w = $(window).width();
    if (w > 767 && menu.is(":hidden")) {
      menu.removeAttr("style");
    }
  });
});
$(document).ready(function () {
  //debugger;
  //var d = new Date();
  //var month = d.getMonth() + 1;
  //var day = d.getDate();
  //var year = d.getFullYear();
  //var dateNow = day + "/" + month + "/" + year;
  //var starts = new Date();
  //starts.setFullYear(starts.getFullYear() - 100);
  //var ends = new Date();
  //ends.setFullYear(ends.getFullYear() - 12);
  ////var d1 = new Date();
  ////var year1 = d1.getFullYear() - 12;
  //var dateNow1 = day - 1 + "/" + month + "/" + ends.getFullYear();
  //var start_now = day + "/" + month + "/" + starts.getFullYear();
  //var starts1 = new Date();
  //starts1.setFullYear(starts1.getFullYear() - 12);
  //var ends1 = new Date();
  //ends1.setFullYear(ends1.getFullYear() - 2);
  //var dateNow2 = day - 1 + "/" + month + "/" + ends1.getFullYear();
  //var start_now2 = day + "/" + month + "/" + starts1.getFullYear();

  //var starts2 = new Date();
  //starts2.setFullYear(starts2.getFullYear() - 2);
  //var ends2 = new Date();
  //ends2.setFullYear(ends2.getFullYear());
  //var dateNow3 = day + "/" + month + "/" + ends2.getFullYear();
  //var start_now3 = day + "/" + month + "/" + starts2.getFullYear();

  var traveldate = TravelDate_CHD_INF.value; ///24/10/2019
  var d = new Date(traveldate);
  var month = d.getMonth() + 1;
  var day = d.getDate();
  var year = d.getFullYear();
  var dateNow = day + "/" + month + "/" + year;
  var starts = new Date(traveldate);
  starts.setFullYear(starts.getFullYear() - 100);
  var ends = new Date();
  ends.setFullYear(ends.getFullYear() - 12);
  //var d1 = new Date();
  //var year1 = d1.getFullYear() - 12;
  var dateNow1 = day - 1 + "/" + month + "/" + ends.getFullYear();
  var start_now = day + "/" + month + "/" + starts.getFullYear();
  var starts1 = new Date(traveldate);
  starts1.setFullYear(starts1.getFullYear() - 12);
  var ends1 = new Date();
  ends1.setFullYear(ends1.getFullYear() - 2);
  var dateNow2 = day + "/" + month + "/" + ends1.getFullYear();
  var start_now2 = day + "/" + month + "/" + starts1.getFullYear();

  var starts2 = new Date(traveldate);
  starts2.setFullYear(starts2.getFullYear() - 2);
  var ends2 = new Date();
  ends2.setFullYear(ends2.getFullYear());
  var dateNow3 =
    ends2.getDate() + "/" + (ends2.getMonth() + 1) + "/" + ends2.getFullYear();
  var start_now3 =
    starts2.getDate() -
    1 +
    "/" +
    (starts2.getMonth() + 1) +
    "/" +
    starts2.getFullYear();

  $(".dtpicker").datepicker({
    autoclose: true,
    format: "dd/mm/yyyy",
    todayHighlight: true,
    startDate: dateNow,
    orientation: "bottom",
    defaultDate: dateNow,
  });
  $(".Adtdob").datepicker({
    autoclose: true,
    format: "dd/mm/yyyy",
    todayHighlight: true,
    startDate: start_now,
    endDate: dateNow1,
    orientation: "bottom",
    defaultDate: dateNow1,
  });
  $(".Chddob").datepicker({
    autoclose: true,
    format: "dd/mm/yyyy",
    todayHighlight: true,
    startDate: start_now2,
    endDate: dateNow2,
    orientation: "bottom",
    defaultDate: dateNow2,
  });
  $(".infdob").datepicker({
    autoclose: true,
    format: "dd/mm/yyyy",
    todayHighlight: true,
    startDate: start_now3,
    endDate: dateNow3,
    orientation: "bottom",
    defaultDate: dateNow3,
  });

  $("#txtMobileNo").keydown(function (event) {
    //debugger;
    if (event.keyCode == 46 || event.keyCode == 8 || event.keyCode == 9) {
      $("#txtMobileNo").css("border-color", "#ccc");
    } else {
      if (
        (event.keyCode < 48 || event.keyCode > 57) &&
        (event.keyCode < 96 || event.keyCode > 105)
      ) {
        $("#txtMobileNo").css("border-color", "red");
        event.preventDefault();
      } else {
        $("#txtMobileNo").css("border-color", "#ccc");
      }
    }
  });

  $("#txtEmailAdd").on("keypress", function () {
    //debugger;
    var re = /([A-Z0-9a-z_-][^@])+?@[^$#<>?]+?\.[\w]{2,4}/.test(this.value);
    if (!re) {
      $("#txtEmailAdd").css("border-color", "red");
    } else {
      $("#txtEmailAdd").css("border-color", "#ccc");
    }
  });
  $("#TXTADDRESS").on("keypress", function () {
    //debugger;
    // alphanumeric without any special charector
    // var re = /^[0-9a-zA-Z\ \']+$/.test(this.value);
    // alphanumeric with space and comma(,)
    var re = /^[0-9a-zA-Z\s\,]+$/.test(this.value);
    if (!re) {
      $("#TXTADDRESS").css("border-color", "red");
    } else {
      $("#TXTADDRESS").css("border-color", "#ccc");
    }
  });
  $("#sidegrossfare").click(function (e) {
    var x = document.getElementById("sidnrtfarediv");
    if (x.style.display === "none") {
      x.style.display = "block";
    } else {
      x.style.display = "none";
    }
  });
  //open popup
  $("#flightinc").on("click", function (event) {
    //debugger;
    event.preventDefault();
    $("#viewflightincl").addClass("is-visible");
  });
  $("#farerul").on("click", function (event) {
    //debugger;
    event.preventDefault();
    $("#viewfarerule").addClass("is-visible");
  });
  $("#farebrakeup").on("click", function (event) {
    //debugger;
    event.preventDefault();
    $("#viewFarebreakup").addClass("is-visible");
  });
  //close popup
  $(".cd-popup").on("click", function (event) {
    if (
      $(event.target).is(".cd-popup-close") ||
      $(event.target).is(".cd-popup")
    ) {
      event.preventDefault();
      $(this).removeClass("is-visible");
    }
  });
  //close popup when clicking the esc keyboard button
  $(document).keyup(function (event) {
    if (event.which == "27") {
      $(".cd-popup").removeClass("is-visible");
    }
  });
  $("#unchkallgst").on("click", function () {
    if ($("#unchkallgst").is(":checked")) {
      if (Boolean($("#hdnHasGST").val())) {
        if (
          $("#txtgstregcompanynm").val().length == 0 &&
          $("#txtgstregno").val().length == 0 &&
          $("#txtgstcompnycontactno").val().length == 0 &&
          $("#txtgstcompnyemail").val().length == 0 &&
          $("#txtgstcompnyaddress").val().length == 0
        ) {
          $("#txtgstregcompanynm").val($("#hdnGSTRegisteredCompany").val());
          $("#txtgstregno").val($("#hdnGSTNumber").val());
          $("#txtgstcompnycontactno").val($("#hdnGSTCompanyContactNo").val());
          $("#txtgstcompnyemail").val($("#hdnGSTCompanyEmail").val());
          $("#txtgstcompnyaddress").val($("#hdnGSTCompanyAddress").val());
        }
      }
    } else if (!$("#unchkallgst").is(":checked")) {
      $("#txtgstregcompanynm").val("");
      $("#txtgstregno").val("");
      $("#txtgstcompnycontactno").val("");
      $("#txtgstcompnyemail").val("");
      $("#txtgstcompnyaddress").val("");
    }
  });
});
