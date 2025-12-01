$(document).ready(function () {
  $("#tickref").click(function () {
    var result = document.getElementById("bookingref").value;
    if (result == "" || result == "Booking Reference") {
      $("#bookingref").addClass("wrong");
      return false;
    } else {
      $("#bookingref").removeClass("wrong");
      location.href = "Print_Ticket.aspx?BookingRef=" + btoa(result);
    }
  });
  $(".mc").click(function () {
    document.getElementById("heading").innerHTML = "";
    document.getElementById("descrp").innerHTML = "";
    document.getElementById("ViewMorediv").style.display = "none";
  });
});

function viewstaffnotifi() {
  location.href = "view_staff_notification.aspx";
}
function viewticketreport() {
  location.href = "TicketReport.aspx";
}
function viewmore(sbc) {
  document.getElementById("heading").innerHTML = sbc.accessKey.split("#")[0];
  document.getElementById("descrp").innerHTML = sbc.accessKey.split("#")[1];
  document.getElementById("ViewMorediv").style.display = "block";
}
function bookrefff(abc) {
  var result = abc.accessKey;

  window.open(
    "Print_Popup.aspx?BookingRef=" + btoa(result),
    "Popup2",
    "location=1,status=1,scrollbars=1, resizable=1, directories=1, toolbar=1, titlebar=1, width=940, height=800"
  );
}

function bookrefffdepartures(abc) {
  var result = abc.accessKey;
  window.open(
    "Departure_Printmail.aspx?BookingRef=" + result,
    "Popup29",
    "location=1,status=1,scrollbars=1, resizable=1, directories=1, toolbar=1, titlebar=1, width=940, height=800"
  );
}

function Canceltktdetail(abc) {
  var result = abc.accessKey;
  window.open(
    "print_can_req.aspx?BookingRef=" + btoa(result),
    "Popup8",
    "location=1,status=1,scrollbars=1, resizable=1, directories=1, toolbar=1, titlebar=1, width=940, height=600"
  );
}
function bookrefffhotel(abc) {
  var result = abc.accessKey;
  window.open(
    "hotel_popup.aspx?BookingRef=" + btoa(result),
    "Popup1",
    "location=1,status=1,scrollbars=1, resizable=1, directories=1, toolbar=1, titlebar=1, width=900, height=800"
  );
}
function showdetail(abc) {
  var ucpol = "";
  usre = {};
  var data1 = abc.accessKey;
  document.getElementById("Compnynm").innerHTML = "";
  document.getElementById("name").innerHTML = "";
  document.getElementById("mobile").innerHTML = "";
  document.getElementById("email").innerHTML = "";
  document.getElementById("panname").innerHTML = "";
  document.getElementById("panno").innerHTML = "";
  document.getElementById("spanaccountt").innerHTML = "";
  document.getElementById("spangst").innerHTML = "";
  // document.getElementById("Cmpnydatat").innerHTML = "";
  try {
    ucpol = $.ajax({
      url: `DashBoard/Showpopup?srchtxt=${data1}`,
      type: "GET",
      success: function (response) {
        var totaldata = ucpol.responseText;
        if (totaldata != "") {
          document.getElementById("Compnynm").innerHTML =
            totaldata.split(",")[0];
          document.getElementById("name").innerHTML =
            totaldata.split(",")[1] + totaldata.split(",")[2];
          document.getElementById("mobile").innerHTML = totaldata.split(",")[3];
          document.getElementById("email").innerHTML = totaldata.split(",")[4];
          document.getElementById("panname").innerHTML =
            totaldata.split(",")[5];
          document.getElementById("panno").innerHTML = totaldata.split(",")[6];
          document.getElementById("spanaccountt").innerHTML =
            totaldata.split(",")[8] + " | " + totaldata.split(",")[7];
          document.getElementById("spangst").innerHTML =
            totaldata.split(",")[9];
          $("#myModal").modal();
        }
      },
    });
  } catch (e) {}
}
window.showdetail= showdetail;
function showdetailcomp(abc) {
  console.log(abc.id);
  if ($(`#${abc.id}`).is(":visible")) {
    $(`#${abc.id}`).hide();
  } else {
    $(`#${abc.id}`).modal();
  }
}
window.showdetailcomp = showdetailcomp;
function showdetailcompcancel(abc) {
  var ll = document.getElementById(abc.id);
  if ($(ll).is(":visible")) {
    $(ll).hide();
  } else {
    $(ll).modal();
  }
}
function toggle(abc) {
  var ele = document.getElementById("toggleText" + abc);
  if (ele.style.display == "block") {
    ele.style.display = "none";
  } else {
    ele.style.display = "block";
  }
}
function flightsearch_1() {
  document.getElementById("f1").className = "serch-option_2";
  document.getElementById("f2").className = "serch-option_3";
  document.getElementById("f3").className = "serch-option_3";
  document.getElementById("f4").className = "serch-option_3";
  document.getElementById("flightsearch").style.display = "block";
  document.getElementById("trainsearch").style.display = "none";
  document.getElementById("dmtsearch").style.display = "none";
  document.getElementById("rechargesearch").style.display = "none";
}
function flightsearch_2() {
  document.getElementById("f1").className = "serch-option_3";
  document.getElementById("f2").className = "serch-option_2";
  document.getElementById("f3").className = "serch-option_3";
  document.getElementById("f4").className = "serch-option_3";
  document.getElementById("flightsearch").style.display = "none";
  document.getElementById("trainsearch").style.display = "block";
  document.getElementById("dmtsearch").style.display = "none";
  document.getElementById("rechargesearch").style.display = "none";
}

function flightsearch_3() {
  document.getElementById("f1").className = "serch-option_3";
  document.getElementById("f2").className = "serch-option_3";
  document.getElementById("f3").className = "serch-option_2";
  document.getElementById("f4").className = "serch-option_3";
  document.getElementById("flightsearch").style.display = "none";
  document.getElementById("trainsearch").style.display = "none";
  document.getElementById("dmtsearch").style.display = "block";
  document.getElementById("rechargesearch").style.display = "none";
}

function flightsearch_4() {
  document.getElementById("f1").className = "serch-option_3";
  document.getElementById("f2").className = "serch-option_3";
  document.getElementById("f3").className = "serch-option_3";
  document.getElementById("f4").className = "serch-option_2";
  document.getElementById("flightsearch").style.display = "none";
  document.getElementById("trainsearch").style.display = "none";
  document.getElementById("dmtsearch").style.display = "none";
  document.getElementById("rechargesearch").style.display = "block";
}

function Updateprofilepage(abc) {
  var result = abc.accessKey;
  location.href = "ModifyProfile.aspx?value=" + btoa(result);
}
