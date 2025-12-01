const pako = require("pako");
var flagSort = "N";
var GetFlightPreferAirlinesResult;
function SelectFlightUsingRedioButton(_refid, _FlightName, _GrossF, _Stop) {
  $("html, body").animate({ scrollTop: 0 }, "fast");
  SelectFlight(_refid, _FlightName, _GrossF, _Stop);
}
window.SelectFlightUsingRedioButton = SelectFlightUsingRedioButton;

function SelectFlight(_refid, _FlightName, _GrossF, _Stop) {
  //debugger;

  SelectedFlightRefId = _refid;
  $("#selectedFlightRefId" + $("#hdnCurrSrNo").val()).text(_refid);
  /*var totMat_ = 0;

    for (var i = 0; i < sortF.d.length; i++) {

        if (sortF.d[i].FlightRefid == _refid) {
            totMat_ += parseInt((sortF.d[i].GrossF).replace(/,/g, ''));
        }
    }
    _GrossF = totMat_;
    */
  /*
    var reDt_ = [];
    reDt_.push(JSON.stringify(sortF.d.find(item => item.FlightRefid === _refid)));
    for (let i = 0; i < reDt_.length; i++) {
        totMat_ += reDt_[i].TotalFare;
    }*/

  //debugger;
  var hostName = $("#hdnhostName").val();
  var CompnyID = $("#hdncmpid").val();
  var CurrSrNo = parseInt($("#hdnCurrSrNo").val());
  $.ajax({
    type: "POST",
    url: "//" + hostName + "/flight/GetSelectedFlightGrossFare",
    //data: "{'objclsPSQ':" + JSON.stringify(params) + "}",
    data: JSON.stringify({
      SelectedFlightRefId: SelectedFlightRefId,
      SrNo: CurrSrNo,
    }),
    // "{'SelectedFlightRefId':'" +
    // SelectedFlightRefId +
    // "','SrNo':" +
    // CurrSrNo +
    // "}",
    contentType: "application/json; charset=utf-8", //Set Content-Type
    dataType: "json", // Set return Data Type
    cache: false,
    success: function (result) {
      //debugger;
      // alert(result.d);
      _GrossF = result.d;

      $(".flight").removeClass("selected");
      $("#rootdiv" + _refid).addClass("selected");

      $("#bookSummary" + $("#hdnCurrSrNo").val().toString()).html(
        "<span class='resultbox4'>" +
          _FlightName +
          "<span class='resultbox4' style='font-size:6pt;color:#00008B;'>(" +
          _Stop +
          ")</span> | </span><span class='resultbox4'> <i class='fa fa-inr'></i>" +
          _GrossF +
          "</span>"
      );
      $("#book_now_radio_button" + _refid).prop("checked", true);
    },
    error: function (xhr, msg, e) {
      //debugger;
      alert("SelectFlight()\n" + msg + "  " + e + " " + xhr);
      // location.href = "//" + hostName + "/Flight/Index";
    },
  }); //------- end next page

  //$('html, body').animate({ scrollTop: 0 }, 'slow');
  //alert(_elimentId);
}
window.SelectFlight = SelectFlight;

function Continue() {
  //debugger;
  var TotalCities = parseInt($("#hdnTotalCities").val());
  for (var i = 1; i <= TotalCities; i++) {
    if ($("#selectedFlightRefId" + i).text() === "") {
      if (SelectedFlightRefId === "") {
        alert("Please select a flight before continue...!");
        return;
        break;
      } else {
        CallNextFlights(i);
        //$('#hdnCurrSrNo').val(i.toString());
        return;
        break;
      }
    }
  }
  //$('#hdnCurrSrNo').val(TotalCities.toString());
  BookNowFun();
  return;
  if (SelectedFlightRefId === "") {
    //alert('Please select a flight before continue...!');
    //return;
    BookNow(0, 0);
  } else {
    BookNow(SelectedFlightRefId, $("#hdnCurrSrNo").val());
  }
}
window.Continue = Continue;

function PreviousCity() {
  var SrNo = parseInt($("#hdnCurrSrNo").val()) - 1;
  if (SrNo > 0) {
    GetCityBySrNo(SrNo);
  }
}
//var isAjaxInProgress = false;
function GetCityBySrNo(refid, SrNo) {
  //debugger;
  //var hostName = $('#hdnhostName').val();
  //var CompnyID = $("#hdncmpid").val();
  //$('#CenterwaitingDiv').css("display", "block");
  // var ReqList = [];

  //ReqList =  '<%= Session["clsPSQBOList"] %>';
  //var result = GetNextReq();
  //console.log(result);
  //var params ='['+ result + ']' ;
  //var serializedObjectList = $("#<%= hdnclsPSQBOList.ClientID %>").val();
  //var obj_ = JSON.parse(serializedObjectList).filter(function (p) {
  //    return p._SrNo === 2
  //});

  //var params =[];
  //params.push(JSON.stringify(obj_));
  var SrNo = parseInt($("#hdnCurrSrNo").val()) + 1;
  var TotalCities = parseInt($("#hdnTotalCities").val());
  if (SrNo > TotalCities) {
    //$("#btnNextCity").css("display", "none");
    BookNow(refid, 1);
  } else {
    //debugger;
    BookNow(refid, 0);
  }
}

//debugger;
//return params;

function BookNow(refid) {
  //debugger;
  BookNowFunction(refid, 0);
}

function BookNowFunction(refid, _CurrSrNo) {
  //debugger;

  $("#modelpopupOUTERCalender").css("display", "Block");
  $("#modelpopupCalender").css("background-color", "Gray");
  $("#modelpopupCalender").delay(1).fadeIn(400);
  $("#modelpopupOUTERCalender").delay(1).fadeIn(400);
  //debugger;
  //var sup = '<%=Session["AgencyID"]%>'.toString();
  //if (sup == '') {
  //    var url = "guest-login.aspx?refid=" + refid + "&triptype=O";
  //    $(location).attr('href', url);
  // }
  // else {
  //var url = "DomFlight_Review.aspx?refid=" + refid + "&triptype=O";
  //$(location).attr('href', url);
  //GetShowSelectFlight(refid);
  // }

  //var SrNo = parseInt($('#hdnCurrSrNo').val()) + 1;

  $.when(GetShowSelectFlight(refid, _CurrSrNo)).then(
    function successHandler(data) {
      //debugger;
    },
    function errorHandler() {}
  );
}

var StrData;
function BookNowFun() {
  $("#modelpopupOUTERCalender").css("display", "Block");
  $("#modelpopupCalender").css("background-color", "Gray");
  //$('#modelpopupCalender').delay(1).fadeIn(400);
  //$('#modelpopupOUTERCalender').delay(1).fadeIn(400);

  var CompnyID = $("#hdncmpid").val();
  var hostName = $("#hdnhostName").val();
  //_CurrSrNo= parseInt(_CurrSrNo);
  //var CurrSrNo = parseInt($('#hdnCurrSrNo').val());
  var TotalCities = parseInt($("#hdnTotalCities").val());
  //$(("#bookSummary" + CurrSrNo.toString())).text("---");
  //var IsFinal = 0;

  //debugger;
  $.ajax({
    type: "POST",
    //url: "n_SearchDataFilter.aspx/ShowOneWaySelectFlight",
    url: "//" + hostName + "/Flight/SelectFlightOneWayMC",
    //data: '{refid:"' + refid + '",CopanyID:"' + CompnyID + '",_SrNo:"' + _CurrSrNo + '"}',
    data: "{'CopanyID':'" + CompnyID + "'}",
    contentType: "application/json; charset=utf-8",
    dataType: "json",
    success: function (msg) {
      $("#modelpopupOUTERCalender").css("display", "none");
      //debugger;
      jQuery("#itinearyWithFarerule").html("");
      jQuery("#itinearFlightRefList").html("");
      jQuery("#flightNotfound").html("");
      /*if (_CurrSrNo > 0 && (msg.d.find(item => item._SrNo === _CurrSrNo)).F_Status === false) {
                $("#TemplatedivSelectFlightNotfound").tmpl((msg.d.find(item => item._SrNo === _CurrSrNo))).appendTo("#flightNotfound");
                $("#FlightNotfoundPopup").css("display", "Block");
                return;

            } else {*/
      //debugger;
      StrData = msg;
      //if (_CurrSrNo === TotalCities || msg.d.length === TotalCities) {
      if (msg.d.length === TotalCities) {
        //$("#modelpopupOUTERCalender").css("display", "none");
        //$("#itinearyWithFarerule").css("background-color", "Gray");
        //$('#itinearyWithFarerule').fadeIn();
        //$('#ItnaryPopup').fadeIn();
        //debugger;

        var totAmt_ = 0;
        var FlightRefidList = [];
        var IsAllFlightsStatus = 1;

        var FlightOnWrdRetuTimes = [];
        for (var i = 0; i < msg.d.length; i++) {
          //$(("#bookSummary" + msg.d[i]._SrNo)).html("<span class='resultbox4'>" + msg.d[i].FlightName + "<span class='resultbox4' style='font-size:6pt;color:#00008B;'>("+ msg.d[i].Stop +")</span> | </span><span class='resultbox4'> <i class='fa fa-inr'></i>" + msg.d[i].GrossF + "</span>");
          if (msg.d[i].F_Status === true) {
            //$("#MyTemplateitinearyPopup").tmpl(msg.d[i]).appendTo("#itinearyWithFarerule");
            //totAmt_ += msg.d[i].TotalAmount;
            FlightRefidList.push(msg.d[i].FlightRefid);

            var ArrDateDescO =
              msg.d[i]["FlightArrDate" + msg.d[i].connection.toString()];
            var d = ArrDateDescO.substr(ArrDateDescO.indexOf(",") + 1, 11);
            d = d.replace(/[^a-zA-Z0-9]/g, " ");
            var stTime =
              msg.d[i]["FlightArrTime" + msg.d[i].connection.toString()].split(
                ":"
              );

            var hrsToAdd = stTime[0] * 60 * 60 * 1000;
            var mtsToAdd = stTime[1] * 60 * 1000;
            var stDate = Date.parse(d);
            stDate = stDate + (hrsToAdd + mtsToAdd);

            var TripName_ =
              msg.d[i].DepartStationName +
              "  =>  " +
              msg.d[i]["ArrivalStationName" + msg.d[i].connection.toString()];

            FlightOnWrdRetuTimes.push(stDate.toString() + "|" + TripName_);
          } else {
            $("#TemplatedivSelectFlightNotfound")
              .tmpl(msg.d[i])
              .appendTo("#flightNotfound");
            //$("#FlightNotfoundPopup").css("display", "Block");
            //$("#ItnaryPopup").css("display", "Block");
            IsAllFlightsStatus = 0;
            //break;
          }
        }
        if (IsAllFlightsStatus === 1) {
          if (msg.d.length < TotalCities) {
            alert(
              "There are one or more trips with not selected flights. Please make sure to select flights for all trips before submitting the form."
            );
            return;
          }

          //---- check time differance between trips
          //debugger;
          $("#datashowConcernModifyouter").css("display", "Block");

          for (var i = 0; i < FlightOnWrdRetuTimes.length - 1; i++) {
            var st_ = FlightOnWrdRetuTimes[i].split("|");
            var end_ = FlightOnWrdRetuTimes[i + 1].split("|");
            var startDt = st_[0];
            var endDt = end_[0];
            var startTrip = st_[1];
            var endTrip = end_[1];

            if (startDt > endDt) {
              alert(
                "Onward flights arrive next day! \n" +
                  startTrip +
                  "\n" +
                  endTrip
              );
              return;
            } else {
              if (!isNaN(startDt) && !isNaN(endDt)) {
                // Calculate the time difference in milliseconds
                var timeDifference = Math.abs(endDt - startDt);

                // Convert milliseconds to hours
                var hoursDifference = Math.floor(
                  timeDifference / (60 * 60 * 1000)
                );
                if (hoursDifference < 3) {
                  alert(
                    "Onward and return flights selection time difference is too short, please select at least 3 hours time difference! \n" +
                      startTrip +
                      "\n" +
                      endTrip
                  );
                  return;
                }
              }
            }
          }
          //--------------------------

          var currancy_ = "";
          //currancy_ = msg.d[0].Currency.trim();
          if (currancy_ === "") {
            currancy_ = "INR";
          }
          //debugger;
          totAmt_ = 0;
          for (var i = 1; i <= TotalCities; i++) {
            var specificValue = $("#bookSummary" + i)
              .text()
              .split("|");
            var am__ = specificValue[1].toString();
            totAmt_ += parseInt(am__.replace(",", "").trim());
          }
          var totData = [
            {
              FlightRefidList: FlightRefidList,
              Currency: currancy_,
              TotAmount: totAmt_,
              TotalFlights: FlightRefidList.length,
            },
          ];
          jQuery("#itinearFlightRefList").html("");
          $("#MyTemplateitinearyPopup")
            .tmpl(msg.d)
            .appendTo("#itinearyWithFarerule");
          $("#MyTemplateitinearyPopupFlightRefidList")
            .tmpl(totData)
            .appendTo("#itinearFlightRefList");

          $("#datashowConcernModifyouter").css("display", "none");

          $("#ItnaryPopup").css("display", "Block");
        } else {
          $("#FlightNotfoundPopup").css("display", "Block");
          return;
        }
      } else {
        //--------- if not final request go to next city/trip

        if (refid === "" || refid === "0" || refid === 0) {
          alert("Please select a flight before continue...!");
          return;
        }

        //debugger;
        const alreadySelectedSrList = [];
        for (let i = 0; i < msg.d.length; i++) {
          alreadySelectedSrList.push(msg.d[i]._SrNo);
          //$(("#bookSummary" + msg.d[i]._SrNo)).html("<span class='resultbox4'>" + msg.d[i].FlightName + "<span class='resultbox4' style='font-size:6pt;color:#00008B;>'("+ msg.d[i].Stop +")</span> | </span><span class='resultbox4'> <i class='fa fa-inr'></i>" + msg.d[i].GrossF + "</span>");
        }
        for (let i = 1; i <= TotalCities; i++) {
          if (!alreadySelectedSrList.includes(i)) {
            CallNextFlights(i);
            break;
          }
        }

        //CallNextFlights((CurrSrNo + 1));
      }

      //var _selectedFlight = msg.d.find(item => item._SrNo == CurrSrNo);
      //$(("#bookSummary" + CurrSrNo.toString())).html("<span class='resultbox4'>" + _selectedFlight.FlightName + " | </span><span class='resultbox4'> <i class='fa fa-inr'></i>" + _selectedFlight.GrossF + "</span>");

      //}
    },
    error: function (msg) {},
  });
}
// window.BookNowFun = BookNowFun;

function CallNextFlights(SrNo) {
  var hostName = $("#hdnhostName").val();
  SelectedFlightRefId = "";
  //$(("#bookSummary" + SrNo.toString())).text("---");
  //$("#bookSummary" + SrNo.toString()).css('background-color', 'red');
  var TotalCities = parseInt($("#hdnTotalCities").val());
  for (var i = 1; i <= TotalCities; i++) {
    $("#bookSummaryNavg" + i).removeClass("selected");
  }
  $("#bookSummaryNavg" + SrNo.toString()).addClass("selected");

  //debugger;
  $("#numFLAdults").val(reqList[SrNo - 1]._NoOfAdult);
  $("#numFLChildren").val(reqList[SrNo - 1]._NoOfChild);
  $("#numFLInfants").val(reqList[SrNo - 1]._NoOfInfant);
  var _dep = reqList[SrNo - 1]._DepartureStation;
  var _arv = reqList[SrNo - 1]._ArrivalStation;

  var arrdep = _dep.split(",");
  var arrArr = _arv.split(",");

  $("#Hiddenoricity").val(arrdep[0]);
  $("#HiddenArrCity").val(arrArr[0]);
  $("#DepDate").val(reqList[SrNo - 1]._BeginDate);

  $("#depCityWaiting").text($("#Hiddenoricity").val());
  $("#departdate").text($("#DepDate").val());
  $("#ArrCityWaiting").text($("#HiddenArrCity").val());
  $("#paxty1").text($("#numFLAdults").val());
  $("#paxty2").text($("#numFLChildren").val());
  $("#paxty").text($("#numFLInfants").val());

  //$("#CenterwaitingDiv").css("display", "Block");
  $("#modelpopupOUTERCalender").css("display", "Block");

  //console.log("Get show data Ajax call start =>" + new Date().toString());

  $.ajax({
    type: "POST",
    url: "//" + hostName + "/Flight/GetvalueForRequestMulticityBySrNo",
    //data: "{'objclsPSQ':" + JSON.stringify(params) + "}",
    data: JSON.stringify({ _SrNo: SrNo, _IsNewReuslt: 0 }),

    // "{'_SrNo':" + SrNo + ",'_IsNewReuslt':" + 0 + "}",
    contentType: "application/json; charset=utf-8", //Set Content-Type
    dataType: "json", // Set return Data Type
    cache: false,
    success: function (result) {
      //console.log("Get show data Ajax call End =>" + new Date().toString());

      //debugger;
      //var hostName = $('#hdnhostName').val();
      //alert(hostName);
      //var obj = result.d;
      // if (obj != "") {
      //debugger;
      //location.href = "//" + hostName + "/k_multicity.aspx";
      //location.href = "//" + hostName + "/k_multicity1.aspx";
      //GetShow();
      /*$('#hdnCurrSrNo').val(SrNo);
            location.href = "//" + hostName + "/c_multicity.aspx";
            */

      //location.href = "//" + hostName + "/TestPage.aspx";
      //brake;
      _CurrSubPageNo = 0; // reset to sub page numer
      $("#hdnCurrSrNo").val(SrNo);
      // GetFlightPreferAirlines();
      GetFlightMatrix();
      //FlightMatrix();
      GetShowData();
      Reset();
      //$("#CenterwaitingDiv").css("display", "none");

      //----------- get preferd airlines
      $.ajax({
        type: "POST",
        url: "/flight/v1/peferairlines",
        data: "{}",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (msg) {
          // let b = {
          //   d: [
          //     {
          //       __type: "FlightAirlines",
          //       Airlines: "6E",
          //       Curr: null,
          //       Airlinespath:
          //         "//style.zealtravels.in/Airline/airlogo_square/6E.gif",
          //       NoOfAirlines: 1,
          //       TotalFare: 5381,
          //     },
          //     {
          //       __type: "FlightAirlines",
          //       Airlines: "AI",
          //       Curr: null,
          //       Airlinespath:
          //         "//style.zealtravels.in/Airline/airlogo_square/AI.gif",
          //       NoOfAirlines: 1,
          //       TotalFare: 5923,
          //     },
          //     {
          //       __type: "FlightAirlines",
          //       Airlines: "SG",
          //       Curr: null,
          //       Airlinespath:
          //         "//style.zealtravels.in/Airline/airlogo_square/SG.gif",
          //       NoOfAirlines: 1,
          //       TotalFare: 4222,
          //     },
          //   ],
          // };
          //GetFlightPreferAirlines = msg;
          let prefferdArline = [];
          for (let i = 0; i < msg.d.length; i++) {
            prefferdArline.push(msg.d[i].Airlines);
          }
          document.getElementById("hdnpreferredairline").value =
            prefferdArline.join(",");

          jQuery("#AirlinesChk").html("");
          $("#templeteAirlinesOnward").tmpl(msg.d).appendTo("#AirlinesChk");
        },
        error: function (msg) {},
      });

      $("#modelpopupOUTERCalender").css("display", "none");

      //----------- end get prefferd air lines

      // UpdateAvailablityBackGroundMC();
      //    debugger;
      //}

      /*const alreadySelectedSrList = [];
            for (let i = 0; i < msg.d.length; i++) {
             alreadySelectedSrList.push(msg.d[i]._SrNo);
             //$(("#bookSummary" + msg.d[i]._SrNo)).html("<span class='resultbox4'>" + msg.d[i].FlightName + "<span class='resultbox4' style='font-size:6pt;color:#00008B;>'("+ msg.d[i].Stop +")</span> | </span><span class='resultbox4'> <i class='fa fa-inr'></i>" + msg.d[i].GrossF + "</span>");
            }*/
    },
    error: function (xhr, msg, e) {
      //debugger;
      // alert("getresult()\n" + msg + "  " + e + " " + xhr);
      // alert("No Flights Found");
      // setTimeout(() => {
      //   location.href = "//" + hostName + "/Flight/Index";
      // }, 500);
    },
  }); //------- end next page
}
window.CallNextFlights = CallNextFlights;

function UpdateAvailablityBackGroundMC() {
  var SrNo = 2;
  /*for (var i = 1; i <= TotalCities; i++) {
        if ($("#selectedFlightRefId" + i).text() === "") {
            SrNo = i.toString();
            break;
        }
    }
*/
  var hostName = $("#hdnhostName").val();
  $.ajax({
    type: "POST",
    url: "//" + hostName + "/Flight/UpdateAvailablityBackGroundMC",
    //async: false,
    //data: "{'objclsPSQ':" + JSON.stringify(params) + "}",
    //data: "{'_SrNo':" + SrNo + "}",
    data: "{}",
    contentType: "application/json; charset=utf-8", //Set Content-Type
    dataType: "json", // Set return Data Type
    cache: false,
    success: function (result) {
      //debugger;
      //var hostName = $('#hdnhostName').val();
      //alert(hostName);
      //var obj = result.d;
      // if (obj != "") {
      //location.href = "//" + hostName + "/k_multicity.aspx";
      //location.href = "//" + hostName + "/k_multicity1.aspx";
      //GetShow();
      //    debugger;
      //}
    },
    error: function (xhr, msg, e) {
      //debugger;
      //alert("getresult()\n" + msg + "  " + e + " " + xhr);
      // location.href = "//" + hostName + "/Index.aspx";
    },
  }); //------- end next page
}
var chkboxValueit = new Array();
var reqList;
var SelectedFlightRefId;
$(document).ready(function () {
  //debugger;
  var emailReg = /^([\w-\.]+@([\w-]+\.)+[\w-]{2,4})?$/;
  var li_ = $("#hdnclsPSQBOList").val();
  reqList = JSON.parse(li_);
  //reqList = (JSON.stringify($('#hdnclsPSQBOList').val()));
  //reqList = $('#hdnclsPSQBOList').val();

  $("#modelpopupOUTERCalender").css("display", "block");
  $("#modelpopupOUTERCalender").css("background-color", "transparent");

  SelectedFlightRefId = "";
  GetShowData();

  // GetFlightPreferAirlines();

  GetFlightMatrix();

  $("#bookSummaryNavg" + $("#hdnCurrSrNo").val()).addClass("selected");

  // GetFlightMatrix();

  $("#Morning").click(function () {
    ShowMorningFlight();
    chkunchk();
  });
  $("#AfterNoon").click(function () {
    ShowAfterNoonFlight();
    chkunchk();
  });
  $("#Night").click(function () {
    ShowNightFlight();
    chkunchk();
  });
  $("#MIDNight").click(function () {
    ShowMIDNightFlight();
    chkunchk();
  });
  $("#Morningtd").click(function () {
    ShowMorningFlight();
    chkunchk();
  });
  $("#AfterNoontd").click(function () {
    ShowAfterNoonFlight();
    chkunchk();
  });
  $("#Nighttd").click(function () {
    ShowNightFlight();
    chkunchk();
  });
  $("#MIDNighttd").click(function () {
    ShowMIDNightFlight();
    chkunchk();
  });

  $(".enablec").click(function () {
    //debugger;
    $("#modelpopupOUTERCalender").css("display", "Block");
    $("#modelpopupCalender").css("background-color", "Gray");
    $("#modelpopupCalender").delay(1).fadeIn(400);
    $("#modelpopupOUTERCalender").delay(1).fadeIn(400);
  });

  var data = document.getElementsByClassName("item");
  // var $item = document.getElementsByClassName("item"), //Cache your DOM selector
  var $item = $("li.item"), //Cache your DOM selector
    visible = 10, //Set the number of items that will be visible
    index = 0, //Starting index
    //endIndex = ($item.length / visible) - 1; //End index
    //endIndex = ($item.length - visible)
    endIndex = $item.length;
  var indexto = 0;
  $("div#arrowR").click(function () {
    //debugger;;
    ($item = $("li.item")), //Cache your DOM selector
      (visible = 10), //Set the number of items that will be visible
      (index = 0), //Starting index
      //endIndex = ($item.length / visible) - 1; //End index
      //endIndex = ($item.length - visible)
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
    //debugger;
    ($item = $("li.item")), //Cache your DOM selector
      (visible = 10), //Set the number of items that will be visible
      (index = 0), //Starting index
      //endIndex = ($item.length / visible) - 1; //End index
      // endIndex = ($item.length - visible)
      (endIndex = $item.length);
    if (indexto > 0) {
      // index--;
      indexto--;
      $item.animate({ left: "+=300px" });
    }
  });

  $("#emailCancel").click(function () {
    $("#emailBlock").hide();
    $("#emailMsg").html("");
  });

  $("#emailClose").click(function () {
    $("#emailBlock").hide();
    $("#emailMsg").html("");
  });

  $("#ColoredsendEmail").click(function (e) {
    //debugger;
    e.preventDefault();
    if ($.trim($("#addressBox").val()) === "") {
      $("#emailMsg").html("Please enter emailId .");
      $("#emailMsg").css("color", "red");
      $("#addressBox").focus();
      return false;
    }
    if (!emailReg.test($("#addressBox").val())) {
      $("#emailMsg").html("Please enter valid emailId .");
      $("#emailMsg").css("color", "red");
      $("#addressBox").focus();
      return false;
    }
    $("#emailMsg").html("Please wait, email is sending ...");
    $("#emailMsg").css("color", "blue");
    $("#ColoredsendEmail").hide();
    var RefID = chkboxValueit;
    var WithoutFare = false;
    if (document.getElementById("chkshowitnaryfare").checked === true) {
      WithoutFare = true;
    }
    var Email = $("#addressBox").val().trim();
    var param = { RefIdArr: RefID, EmailAdd: Email, ItnaryFare: WithoutFare };
    $.ajax({
      type: "POST",
      url: "n_one.aspx/SendFareQuatation",
      data: JSON.stringify(param),
      contentType: "application/json; charset=utf-8", //Set Content-Type
      dataType: "json", // Set return Data Type
      cache: false,
      success: function (result) {
        var obj = result.d;
        if (obj === true) {
          alert("Flight Fare Quotation send successfully!");
          $("#emailBlock").hide();
          $("#emailMsg").html("");

          $("ul.chkitnary")
            .find("input:checkbox")
            .each(function () {
              if (this.checked === true) {
                this.checked = false;
              }
            });
        } else {
          alert(
            "There is some error for sending quatation.Kindly call to customer care!\n"
          );
        }
      },

      error: function (xhr, msg) {
        ////debugger;
        alert(
          "There is some error for sending quatation.Kindly call to customer care!\n"
        );
      },
    });
  });
});

var isLoading = false;
$(window).scroll(function () {
  if ($(window).scrollTop() + $(window).height() > $(document).height() - 400) {
    if (_CurrSubPageNo > 998) {
      return;
    }
    if (isLoading === true) {
      return;
    }
    isLoading = true;

    //alert("Near bottom!");
    //debugger;
    GetShowData();
    //Reset();
  }
});

jQuery(window).on("load", function () {
  UpdateAvailablityBackGroundMC();
});

function Showitnaryemailpopup(abc) {
  //debugger;
  var status = 0;
  $("ul.chkitnary")
    .find("input:checkbox")
    .each(function () {
      if (this.checked === true) {
        status = 1;
        var v = $(this).val();
        chkboxValueit.push(v);
      }
    });
  if (status > 0) {
    $("#emailBlock").show();
    $("#sendEmail").show();
    $("#addressBox").val("");
    $("#addressBox").focus();
    $("#emailMsg").html("");
  } else {
    alert("Please select itinerary to send mail.");
  }
}

function ShowMorningFlight() {
  $("#modelpopupOUTER").css("display", "Block");
  $("#modelpopup").css("background-color", "Gray");
  $("#modelpopup").delay(1).fadeIn(400);
  $("#modelpopupOUTER").delay(1).fadeIn(400);
  var msg = sortF;
  jQuery("#flightRow").html("");
  for (var i = 0; i < msg.d.length; i++) {
    var FlightDepTime = parseFloat(
      msg.d[i].FlightDepTime.substring(0, 2) +
        msg.d[i].FlightDepTime.substring(3, 5)
    );
    if (FlightDepTime >= 500 && FlightDepTime <= 1200) {
      $("#MyTemplate").tmpl(msg.d[i]).appendTo("#flightRow");
    }
  }
  $("#modelpopup").delay(1000).fadeOut(400);
  $("#modelpopupOUTER").delay(1000).fadeOut(400);
}
function FareM(Airlinename, Fare) {
  $("#modelpopupOUTER").css("display", "Block");
  $("#modelpopup").css("background-color", "Gray");
  $("#modelpopup").delay(1).fadeIn(400);
  $("#modelpopupOUTER").delay(1).fadeIn(400);
  if (Fare != "---") {
    var msg = sortF;
    jQuery("#flightRow").html("");
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
        $("#MyTemplate").tmpl(msg.d[i]).appendTo("#flightRow");
      }
    }
  }
  $("#modelpopup").delay(1000).fadeOut(400);
  $("#modelpopupOUTER").delay(1000).fadeOut(400);
  chkunchk();
}
function FareA(Airlinename, Fare) {
  $("#modelpopupOUTER").css("display", "Block");
  $("#modelpopup").css("background-color", "Gray");
  $("#modelpopup").delay(1).fadeIn(400);
  $("#modelpopupOUTER").delay(1).fadeIn(400);
  if (Fare != "---") {
    var msg = sortF;
    jQuery("#flightRow").html("");
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
        $("#MyTemplate").tmpl(msg.d[i]).appendTo("#flightRow");
      }
    }
  }
  $("#modelpopup").delay(1000).fadeOut(400);
  $("#modelpopupOUTER").delay(1000).fadeOut(400);
  chkunchk();
}
function FareN(Airlinename, Fare) {
  $("#modelpopupOUTER").css("display", "Block");
  $("#modelpopup").css("background-color", "Gray");
  $("#modelpopup").delay(1).fadeIn(400);
  $("#modelpopupOUTER").delay(1).fadeIn(400);
  if (Fare != "---") {
    var msg = sortF;
    jQuery("#flightRow").html("");
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
        $("#MyTemplate").tmpl(msg.d[i]).appendTo("#flightRow");
      }
    }
  }
  $("#modelpopup").delay(1000).fadeOut(400);
  $("#modelpopupOUTER").delay(1000).fadeOut(400);
  chkunchk();
}
function FareMN(Airlinename, Fare) {
  $("#modelpopupOUTER").css("display", "Block");
  $("#modelpopup").css("background-color", "Gray");
  $("#modelpopup").delay(1).fadeIn(400);
  $("#modelpopupOUTER").delay(1).fadeIn(400);
  if (Fare != "---") {
    var msg = sortF;
    jQuery("#flightRow").html("");
    for (var i = 0; i < msg.d.length; i++) {
      var FlightDepTime = parseFloat(
        msg.d[i].FlightDepTime.substring(0, 2) +
          msg.d[i].FlightDepTime.substring(3, 5)
      );
      if (
        FlightDepTime >= 0 &&
        FlightDepTime <= 500 &&
        msg.d[i].FlightName == Airlinename
      ) {
        $("#MyTemplate").tmpl(msg.d[i]).appendTo("#flightRow");
      }
    }
  }
  $("#modelpopup").delay(1000).fadeOut(400);
  $("#modelpopupOUTER").delay(1000).fadeOut(400);
  chkunchk();
}
function ShowAfterNoonFlight() {
  $("#modelpopupOUTER").css("display", "Block");
  $("#modelpopup").css("background-color", "Gray");
  $("#modelpopup").delay(1).fadeIn(400);
  $("#modelpopupOUTER").delay(1).fadeIn(400);
  var msg = sortF;
  jQuery("#flightRow").html("");
  for (var i = 0; i < msg.d.length; i++) {
    var FlightDepTime = parseFloat(
      msg.d[i].FlightDepTime.substring(0, 2) +
        msg.d[i].FlightDepTime.substring(3, 5)
    );
    if (FlightDepTime >= 1200 && FlightDepTime <= 1800) {
      $("#MyTemplate").tmpl(msg.d[i]).appendTo("#flightRow");
    }
  }
  $("#modelpopup").delay(1000).fadeOut(400);
  $("#modelpopupOUTER").delay(1000).fadeOut(400);
}
function ShowNightFlight() {
  $("#modelpopupOUTER").css("display", "Block");
  $("#modelpopup").css("background-color", "Gray");
  $("#modelpopup").delay(1).fadeIn(400);
  $("#modelpopupOUTER").delay(1).fadeIn(400);
  var msg = sortF;
  jQuery("#flightRow").html("");
  for (var i = 0; i < msg.d.length; i++) {
    var FlightDepTime = parseFloat(
      msg.d[i].FlightDepTime.substring(0, 2) +
        msg.d[i].FlightDepTime.substring(3, 5)
    );
    if (FlightDepTime >= 1800 && FlightDepTime <= 2400) {
      $("#MyTemplate").tmpl(msg.d[i]).appendTo("#flightRow");
    }
  }
  $("#modelpopup").delay(1000).fadeOut(400);
  $("#modelpopupOUTER").delay(1000).fadeOut(400);
}
function ShowMIDNightFlight() {
  $("#modelpopupOUTER").css("display", "Block");
  $("#modelpopup").css("background-color", "Gray");
  $("#modelpopup").delay(1).fadeIn(400);
  $("#modelpopupOUTER").delay(1).fadeIn(400);
  var msg = sortF;
  jQuery("#flightRow").html("");
  for (var i = 0; i < msg.d.length; i++) {
    var FlightDepTime = parseFloat(
      msg.d[i].FlightDepTime.substring(0, 2) +
        msg.d[i].FlightDepTime.substring(3, 5)
    );
    if (FlightDepTime >= 0 && FlightDepTime <= 500) {
      $("#MyTemplate").tmpl(msg.d[i]).appendTo("#flightRow");
    }
  }
  $("#modelpopup").delay(1000).fadeOut(400);
  $("#modelpopupOUTER").delay(1000).fadeOut(400);
}

var sortF;
var _CurrSubPageNo = 0;
function base64ToByteArray(string) {
  const binaryString = atob(string); // Decode Base64 to a binary string
  const len = binaryString.length;
  const bytes = new Uint8Array(len); // Create a byte array

  for (let i = 0; i < len; i++) {
    bytes[i] = binaryString.charCodeAt(i); // Convert each character to a byte
  }

  return bytes;
}
function GetShowData() {
  if (_CurrSubPageNo > 998) {
    $("#FlightPopUp").css("display", "none");
    $("#datashowConcernModifyouter").css("display", "none");
    $("#modelpopupOUTERCalender").css("display", "none");
    $("#modelpopupOUTERCalender").css("background-color", "transparent");
    return;
  }

  // //debugger;
  $("#modelpopupOUTERCalender").css("display", "Block");
  $("#datashowConcernModifyouter").css("display", "Block");
  //$("#datashowConcernModifyouter").css("background-color", "Gray");
  //$("#FlightPopUp").css("display", "Block");
  var CompnyID = $("#hdncmpid").val();

  //var _SrNo = $("#hdnCurrSrNo").val();

  //console.log("Get show data from front page start =>" + new Date().toString());
  var hostName = $("#hdnhostName").val();
  $.ajax({
    type: "POST",
    //url: "n_SearchDataFilter.aspx/ShowOneWayMC",
    url: "//" + hostName + "/Flight/ShowOneWayMC",
    //data: '{CompanyID:"' + CompnyID + '",_SrNo:"'+ _SrNo +'",_IsNewReuslt:"'+ _IsNewReuslt +'"}',
    data: JSON.stringify({
      CompanyID: CompnyID,
      _CurrSubPageNo: _CurrSubPageNo,
    }),
    //   '{CompanyID:"' + CompnyID + '",_CurrSubPageNo:"' + _CurrSubPageNo + '"}',
    contentType: "application/json; charset=utf-8",
    dataType: "json",
    success: function (msg) {
      //debugger;
      if (_CurrSubPageNo === 0) {
        sortF = null;
        jQuery("#flightRow").html("");
      }
      //var decompress = LZW.decompress(msg);
      //console.log(msg.d);
      //debugger;
      //var compressData = msg.d;
      // console.log(msg.d);
      // console.log(decodedString);
      const compressData = base64ToByteArray(msg.d);
      //   const compressData = new Uint8Array(decodedString);
      //const data = pako.inflate(compressData,{ to: 'string' });
      // Convert gunzipped byteArray back to ascii string:
      // console.log(compressData);

      var decompress = pako.inflate(compressData, { to: "string" }); // String.fromCharCode.apply(null, new Uint32Array(data));
      _CurrSubPageNo = parseInt(decompress.substring(0, 3));
      var _objListStr = decompress.substring(3);
      var jsonObject = JSON.parse(_objListStr);
      // console.log(jsonObject);

      if (_CurrSubPageNo === 1 || _CurrSubPageNo === 999) {
        sortF = msg;
        sortF.d = jsonObject;
      } else {
        sortF.d = sortF.d.concat(jsonObject);
      }
      //msg.d = sortF.d;

      //alert(msg.d.length);
      //sortFCurrency = msg;
      //jQuery('#flightRow').html('');
      //debugger;
      //------ lazy load start

      //------- lazy load end
      var jsonResponse = msg;
      var groupByFlightNumber = groupJson(jsonResponse);

      Object.keys(groupByFlightNumber).forEach(function (key) {
        var group = groupByFlightNumber[key];

        $("#MyTemplate").tmpl(group[0]).appendTo("#flightRow");
        $("#multipleDetails")
          .tmpl(group)
          .appendTo("#Price3-" + key);
        $("#multipleDetails2")
          .tmpl(group)
          .appendTo("#DR-" + key);
      });
      //for (var i = 0; i < msg.d.length; i++) {
      // for (var i = 0; i < jsonObject.length; i++) {
      //   //for (var i = 0; i < 3;i++) {
      //   //debugger;
      //   //$("#MyTemplate").tmpl(msg.d[i]).appendTo("#flightRow");
      //   $("#MyTemplate").tmpl(jsonObject[i]).appendTo("#flightRow");
      // }

      //console.log("Get show data from front page end =>"  + new Date().toString());

      /*var pageSize = $('html').html().length;
            var kbytes = pageSize / 1024;
            console.log("Page size is Kb =>" + kbytes.toString());
            */

      GetMinMax();
      //                    GetMinMaxDepartureTime();
      //                    GetMinMaxArrivalTime();
      GetFlightPreferAirlines();
      GetMinMaxDepartureTime();
      GetMinMaxArrivalTime();
      GetFlightStops();
      //GetFlightMatrix();
      $("#FlightPopUp").css("display", "none");
      $("#datashowConcernModifyouter").css("display", "none");

      var cmpid1 = document.getElementById("hdncmpid").value;
      if (cmpid1.indexOf("C-") > -1 || cmpid1 === "") {
        //debugger;
        document.getElementById("chkdiscccfare").checked = true;
        $(".hidchk").css({ display: "none" });
        $(".hidtdscus").css({ display: "none" });
      }

      chkunchk();

      $("#modelpopupOUTERCalender").css("display", "none");
      $("#modelpopupOUTERCalender").css("background-color", "transparent");
      isLoading = false;
    },
    error: function (msg) {
      isLoading = false;
    },
  });
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
      // $("#modelpopupOUTER").css("display", "Block");
      // $("#modelpopup").css("background-color", "Gray");
      // $("#modelpopup").delay(1).fadeIn(400);
      // $("#modelpopupOUTER").delay(1).fadeIn(400);

      $("#amount").val("Rs." + ui.values[0] + " - Rs." + ui.values[1]);
      // msg = sortF;

      // jQuery("#flightRow").html("");
      // var i = 0;
      // var j = msg.d.length;

      // for (i; i < j; i++) {
      //   var TotalAmount = parseFloat(msg.d[i].TotalAmount);
      //   if (TotalAmount >= ui.values[0] && TotalAmount <= ui.values[1]) {
      //     $("#MyTemplate").tmpl(msg.d[i]).appendTo("#flightRow");
      //   }
      // }
      chkunchk();

      // $("#modelpopup").delay(1000).fadeOut(400);
      // $("#modelpopupOUTER").delay(1000).fadeOut(400);
    },
    stop: function (event, ui) {
      document.getElementById("hdnminval").value = ui.values[0];
      document.getElementById("hdnmaxval").value = ui.values[1];
      getCheckedFilters(flagSort);
    }
  });
  $("#amount").val("Rs. " + min + " - Rs. " + max);
}
var GetFlightPreferAirlinesResult;
function GetFlightPreferAirlines() {
  $.ajax({
    type: "POST",
    url: "/flight/v1/peferairlines",
    data: "{}",
    contentType: "application/json; charset=utf-8",
    dataType: "json",
    success: function (msg) {
      // let b = {
      //   d: [
      //     {
      //       __type: "FlightAirlines",
      //       Airlines: "6E",
      //       Curr: null,
      //       Airlinespath:
      //         "//style.zealtravels.in/Airline/airlogo_square/6E.gif",
      //       NoOfAirlines: 1,
      //       TotalFare: 5381,
      //     },
      //     {
      //       __type: "FlightAirlines",
      //       Airlines: "AI",
      //       Curr: null,
      //       Airlinespath:
      //         "//style.zealtravels.in/Airline/airlogo_square/AI.gif",
      //       NoOfAirlines: 1,
      //       TotalFare: 5923,
      //     },
      //     {
      //       __type: "FlightAirlines",
      //       Airlines: "SG",
      //       Curr: null,
      //       Airlinespath:
      //         "//style.zealtravels.in/Airline/airlogo_square/SG.gif",
      //       NoOfAirlines: 1,
      //       TotalFare: 4222,
      //     },
      //   ],
      // };
      GetFlightPreferAirlinesResult = msg;
      jQuery("#AirlinesChk").html("");
      $("#templeteAirlinesOnward").tmpl(msg.d).appendTo("#AirlinesChk");
      let prefferdArline = [];
      for (let i = 0; i < msg.d.length; i++) {
        prefferdArline.push(msg.d[i].Airlines);
      }
      document.getElementById("hdnpreferredairline").value =
        prefferdArline.join(",");
    },
    error: function (msg) {},
  });
}

function getCheckedFAirlines() {
  // $("#modelpopupOUTER").css("display", "Block");
  // $("#modelpopup").css("background-color", "Gray");
  // $("#modelpopup").delay(1).fadeIn(400);
  // $("#modelpopupOUTER").delay(1).fadeIn(400);
  // var msg2 = sortF;
  var chkboxValue = new Array();
  $("ul.amenViewairline")
    .find("input:checkbox")
    .each(function () {
      if (this.checked === true) {
        var v = $(this).val();
        chkboxValue.push(v);
      }
    });
    document.getElementById("hdncheckedpreferredairline").value =
    chkboxValue.join(",");
  // var msg;
  // if (chkboxValue.length === 0) {
  //   jQuery("#flightRow").html("");
  //   msg = sortF;

  //   for (var i = 0; i < msg.d.length; i++) {
  //     $("#MyTemplate").tmpl(msg.d[i]).appendTo("#flightRow");
  //   }
  // } else {
  //   var c = 0;
  //   jQuery("#flightRow").html("");
  //   for (c; c < chkboxValue.length; c++) {
  //     msg = sortF;
  //     //debugger;
  //     for (var i = 0; i < msg.d.length; i++) {
  //       if (chkboxValue[c] === msg.d[i].FlightName) {
  //         $("#MyTemplate").tmpl(msg.d[i]).appendTo("#flightRow");
  //       }
  //     }
  //   }
  // }
  // $("#modelpopup").delay(1000).fadeOut(400);
  // $("#modelpopupOUTER").delay(1000).fadeOut(400);
  getCheckedFilters(flagSort);
  chkunchk();
}
window.getCheckedFAirlines = getCheckedFAirlines;
var GetFlightstops;
function GetFlightStops() {
  //if(_CurrSubPageNo>1){return;}

  jQuery("#stops").html("");
  $.ajax({
    type: "POST",
    url: "/Flight/FlightStops",
    data: "{}",
    contentType: "application/json; charset=utf-8",
    dataType: "json",
    success: function (msg) {
      GetFlightstops = msg;
      $("#templetestops").tmpl(msg.d).appendTo("#stops");
      let stops = [];
      for (let i = 0; i < msg.d.length; i++) {
        stops.push(msg.d[i].Stops);
      }
      document.getElementById("hdnstops").value = stops.join(",");
    },
    error: function (msg) {},
  });
}
function Reset() {
  $("#modelpopupOUTER").css("display", "Block");
  $("#modelpopup").css("background-color", "Gray");
  $("#modelpopup").delay(1).fadeIn(400);
  $("#modelpopupOUTER").delay(1).fadeIn(400);

  jQuery("#flightRow").html("");
  var msg = sortF;
  for (var i = 0; i < msg.d.length; i++) {
    $("#MyTemplate").tmpl(msg.d[i]).appendTo("#flightRow");
  }
  var msg2 = GetFlightPreferAirlinesResult;
  jQuery("#AirlinesChk").html("");
  $("#templeteAirlinesOnward").tmpl(msg2.d).appendTo("#AirlinesChk");

  jQuery("#stops").html("");
  var msg3 = GetFlightstops;

  $("#templetestops").tmpl(msg3.d).appendTo("#stops");
  $("#modelpopup").delay(1000).fadeOut(400);
  $("#modelpopupOUTER").delay(1000).fadeOut(400);

  chkunchk();
}
window.Reset = Reset;

function getCheckedF() {
  //debugger;
  // $("#modelpopupOUTER").css("display", "Block");
  // $("#modelpopup").css("background-color", "Gray");
  // $("#modelpopup").delay(1).fadeIn(400);
  // $("#modelpopupOUTER").delay(1).fadeIn(400);
  var msg2 = sortF;
  var chkboxValue = new Array();
  $("ul.amenView")
    .find("input:checkbox")
    .each(function () {
      if (this.checked === true) {
        var v = $(this).val();
        chkboxValue.push(v);
      }
    });
  document.getElementById("hdncheckedstops").value = chkboxValue.join(",");
  // var msg;
  // if (chkboxValue.length === 0) {
  //   jQuery("#flightRow").html("");
  //   msg = sortF;
  //   for (var i = 0; i < msg.d.length; i++) {
  //     $("#MyTemplate").tmpl(msg.d[i]).appendTo("#flightRow");
  //   }
  // } else {
  //   var c = 0;
  //   jQuery("#flightRow").html("");
  //   for (c; c < chkboxValue.length; c++) {
  //     msg = sortF;
  //     for (var i = 0; i < msg.d.length; i++) {
  //       if (chkboxValue[c] === msg.d[i].Stop) {
  //         $("#MyTemplate").tmpl(msg.d[i]).appendTo("#flightRow");
  //       }
  //     }
  //   }
  // }
  // $("#modelpopup").delay(1000).fadeOut(400);
  // $("#modelpopupOUTER").delay(1000).fadeOut(400);
  getCheckedFilters(flagSort);
  chkunchk();
}
window.getCheckedF = getCheckedF;
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
function FareSummaryopen(data) {
  $("#" + data).slideToggle();
}
function FareSummaryclose(data) {
  $("#" + data).css("display", "none");
}
window.FareSummaryopen = FareSummaryopen;
window.FareSummaryclose = FareSummaryclose;
function Close(FlightRefid) {
  $("#divshow" + FlightRefid + "").hide("slow");
  $("#FAREDETAILS" + FlightRefid + "").css(
    "border-bottom",
    "0px solid #C0C0C0"
  );
  $("#ITINERARY" + FlightRefid + "").css("border-bottom", "3px solid #FEBD25");
  $("#FARERULE" + FlightRefid + "").css("border-bottom", "0px solid #C0C0C0");
  $("#BAGGAGEDETAILS" + FlightRefid + "").css(
    "border-bottom",
    "0px solid #C0C0C0"
  );

  $("#divITINERARY" + FlightRefid + "").slideDown();
  $("#FareSummary" + FlightRefid + "").slideUp();
  $("#Farerulesummery" + FlightRefid + "").slideUp();
  $("#Baggagesummery" + FlightRefid + "").slideUp();
}
window.Close = Close;
function CloseBookSmry(FlightRefid) {
  $("#divshowBookSmry" + FlightRefid + "").hide("slow");
  $("#FAREDETAILSBookSmry" + FlightRefid + "").css(
    "border-bottom",
    "0px solid #C0C0C0"
  );
  $("#ITINERARYBookSmry" + FlightRefid + "").css(
    "border-bottom",
    "3px solid #FEBD25"
  );
  $("#FARERULEBookSmry" + FlightRefid + "").css(
    "border-bottom",
    "0px solid #C0C0C0"
  );
  $("#BAGGAGEDETAILSBookSmry" + FlightRefid + "").css(
    "border-bottom",
    "0px solid #C0C0C0"
  );

  $("#divITINERARYBookSmry" + FlightRefid + "").slideDown();
  $("#FareSummaryBookSmry" + FlightRefid + "").slideUp();
  $("#FarerulesummeryBookSmry" + FlightRefid + "").slideUp();
  $("#BaggagesummeryBookSmry" + FlightRefid + "").slideUp();
}
window.closeBookSmry = CloseBookSmry;

function FAREDETAILS(FlightRefid) {
  //FareRule(FlightRefid);
  $("#ITINERARY" + FlightRefid + "").css("border-bottom", "0px solid #C0C0C0");
  $("#FAREDETAILS" + FlightRefid + "").css(
    "border-bottom",
    "3px solid #FEBD25"
  );
  $("#FARERULE" + FlightRefid + "").css("border-bottom", "0px solid #C0C0C0");
  $("#BAGGAGEDETAILS" + FlightRefid + "").css(
    "border-bottom",
    "0px solid #C0C0C0"
  );
  $("#divITINERARY" + FlightRefid + "").slideUp();
  $("#FareSummary" + FlightRefid + "").slideDown();
  $("#Farerulesummery" + FlightRefid + "").slideUp();
  $("#Baggagesummery" + FlightRefid + "").slideUp();
}
window.FAREDETAILS = FAREDETAILS;

function FAREDETAILSBookSmry(FlightRefid) {
  //FareRule(FlightRefid);
  $("#ITINERARYBookSmry" + FlightRefid + "").css(
    "border-bottom",
    "0px solid #C0C0C0"
  );
  $("#FAREDETAILSBookSmry" + FlightRefid + "").css(
    "border-bottom",
    "3px solid #FEBD25"
  );
  $("#FARERULEBookSmry" + FlightRefid + "").css(
    "border-bottom",
    "0px solid #C0C0C0"
  );
  $("#BAGGAGEDETAILSBookSmry" + FlightRefid + "").css(
    "border-bottom",
    "0px solid #C0C0C0"
  );
  $("#divITINERARYBookSmry" + FlightRefid + "").slideUp();
  $("#FareSummaryBookSmry" + FlightRefid + "").slideDown();
  $("#FarerulesummeryBookSmry" + FlightRefid + "").slideUp();
  $("#BaggagesummeryBookSmry" + FlightRefid + "").slideUp();
}
window.FAREDETAILSBookSmry = FAREDETAILSBookSmry;

function FARERULE(FlightRefid) {
  // FareRule(FlightRefid);
  $("#ITINERARY" + FlightRefid + "").css("border-bottom", "0px solid #C0C0C0");
  $("#FAREDETAILS" + FlightRefid + "").css(
    "border-bottom",
    "0px solid #C0C0C0"
  );
  $("#BAGGAGEDETAILS" + FlightRefid + "").css(
    "border-bottom",
    "0px solid #C0C0C0"
  );
  $("#FARERULE" + FlightRefid + "").css("border-bottom", "3px solid #FEBD25");

  $("#divITINERARY" + FlightRefid + "").slideUp();
  $("#FareSummary" + FlightRefid + "").slideUp();
  $("#Baggagesummery" + FlightRefid + "").slideUp();
  $("#Farerulesummery" + FlightRefid + "").slideDown();
}
window.FARERULE = FARERULE;
function FARERULEBookSmry(FlightRefid) {
  // FareRule(FlightRefid);
  $("#ITINERARYBookSmry" + FlightRefid + "").css(
    "border-bottom",
    "0px solid #C0C0C0"
  );
  $("#FAREDETAILSBookSmry" + FlightRefid + "").css(
    "border-bottom",
    "0px solid #C0C0C0"
  );
  $("#BAGGAGEDETAILSBookSmry" + FlightRefid + "").css(
    "border-bottom",
    "0px solid #C0C0C0"
  );
  $("#FARERULEBookSmry" + FlightRefid + "").css(
    "border-bottom",
    "3px solid #FEBD25"
  );

  $("#divITINERARYBookSmry" + FlightRefid + "").slideUp();
  $("#FareSummaryBookSmry" + FlightRefid + "").slideUp();
  $("#BaggagesummeryBookSmry" + FlightRefid + "").slideUp();
  $("#FarerulesummeryBookSmry" + FlightRefid + "").slideDown();
}
window.FARERULEBookSmry = FARERULEBookSmry;

function BAGGAGEDETAILS(FlightRefid) {
  // FareRule(FlightRefid);
  $("#ITINERARY" + FlightRefid + "").css("border-bottom", "0px solid #C0C0C0");
  $("#FAREDETAILS" + FlightRefid + "").css(
    "border-bottom",
    "0px solid #C0C0C0"
  );
  $("#FARERULE" + FlightRefid + "").css("border-bottom", "0px solid #C0C0C0");
  $("#BAGGAGEDETAILS" + FlightRefid + "").css(
    "border-bottom",
    "3px solid #FEBD25"
  );

  $("#divITINERARY" + FlightRefid + "").slideUp();
  $("#FareSummary" + FlightRefid + "").slideUp();

  $("#Farerulesummery" + FlightRefid + "").slideUp();
  $("#Baggagesummery" + FlightRefid + "").slideDown();
}
window.BAGGAGEDETAILS = BAGGAGEDETAILS;

function BAGGAGEDETAILSBookSmry(FlightRefid) {
  // FareRule(FlightRefid);
  $("#ITINERARYBookSmry" + FlightRefid + "").css(
    "border-bottom",
    "0px solid #C0C0C0"
  );
  $("#FAREDETAILSBookSmry" + FlightRefid + "").css(
    "border-bottom",
    "0px solid #C0C0C0"
  );
  $("#FARERULEBookSmry" + FlightRefid + "").css(
    "border-bottom",
    "0px solid #C0C0C0"
  );
  $("#BAGGAGEDETAILSBookSmry" + FlightRefid + "").css(
    "border-bottom",
    "3px solid #FEBD25"
  );

  $("#divITINERARYBookSmry" + FlightRefid + "").slideUp();
  $("#FareSummaryBookSmry" + FlightRefid + "").slideUp();

  $("#FarerulesummeryBookSmry" + FlightRefid + "").slideUp();
  $("#BaggagesummeryBookSmry" + FlightRefid + "").slideDown();
}
window.BAGGAGEDETAILSBookSmry = BAGGAGEDETAILSBookSmry;

function ITINERARY(FlightRefid) {
  $("#FAREDETAILS" + FlightRefid + "").css(
    "border-bottom",
    "0px solid #C0C0C0"
  );
  $("#ITINERARY" + FlightRefid + "").css("border-bottom", "3px solid #FEBD25");
  $("#FARERULE" + FlightRefid + "").css("border-bottom", "0px solid #C0C0C0");
  $("#BAGGAGEDETAILS" + FlightRefid + "").css(
    "border-bottom",
    "0px solid #C0C0C0"
  );
  $("#divITINERARY" + FlightRefid + "").slideDown();
  $("#FareSummary" + FlightRefid + "").slideUp();
  $("#Farerulesummery" + FlightRefid + "").slideUp();
  $("#Baggagesummery" + FlightRefid + "").slideUp();
}
window.ITINERARY = ITINERARY;
function ITINERARYBookSmry(FlightRefid) {
  $("#FAREDETAILSBookSmry" + FlightRefid + "").css(
    "border-bottom",
    "0px solid #C0C0C0"
  );
  $("#ITINERARYBookSmry" + FlightRefid + "").css(
    "border-bottom",
    "3px solid #FEBD25"
  );
  $("#FARERULEBookSmry" + FlightRefid + "").css(
    "border-bottom",
    "0px solid #C0C0C0"
  );
  $("#BAGGAGEDETAILSBookSmry" + FlightRefid + "").css(
    "border-bottom",
    "0px solid #C0C0C0"
  );
  $("#divITINERARYBookSmry" + FlightRefid + "").slideDown();
  $("#FareSummaryBookSmry" + FlightRefid + "").slideUp();
  $("#FarerulesummeryBookSmry" + FlightRefid + "").slideUp();
  $("#BaggagesummeryBookSmry" + FlightRefid + "").slideUp();
}
window.ITINERARYBookSmry = ITINERARYBookSmry;

function FareRule(refid) {
  $("#FarerulePP").css("display", "Block");
  jQuery("#FarerulePP").html("");
  var msga = sortF;
  for (var i = 0; i < msga.d.length; i++) {
    if (msga.d[i].FlightRefid === refid) {
      var p = Fareruledesc(msga.d[i]);
      $("#FarerulePP").append(p);
      break;
    }
  }
  $("#FarerulePP").css("display", "Block");
}
window.FareRule = FareRule;
function CloseFarerulePP() {
  $("#FarerulePP").css("display", "none");
}
window.CloseFarerulePP = CloseFarerulePP;
function Fareruledesc(obj) {
  var head =
    "<div ><div class='FareSummaryClassTitle'>Fare rule:-</div><a href='javascript:void(0);' onclick='CloseFarerulePP();' style='float:right; position:absolute; right:-5px; top:-5px;' ><img src='/assets/img/cross.png' /> </a> " +
    obj.FareRules +
    "</div>";
  var fin = head;
  return fin;
}

function showtaxandCharges(FlightRefid) {
  //debugger;
  $("#TaxAndChargesDIV" + FlightRefid).css("display") === "none"
    ? $("#TaxAndChargesDIV" + FlightRefid).slideDown()
    : $("#TaxAndChargesDIV" + FlightRefid).slideUp();
  $("#TotalTaxDIV" + FlightRefid).slideUp();
  $("#CommDetailDIV" + FlightRefid).slideUp();
}
function showtotaltax(FlightRefid) {
  //debugger;
  $("#TotalTaxDIV" + FlightRefid).css("display") === "none"
    ? $("#TotalTaxDIV" + FlightRefid).slideDown()
    : $("#TotalTaxDIV" + FlightRefid).slideUp();
  $("#CommDetailDIV" + FlightRefid).slideUp();
}
function showCommDetail(FlightRefid) {
  //debugger;
  $("#CommDetailDIV" + FlightRefid).css("display") === "none"
    ? $("#CommDetailDIV" + FlightRefid).slideDown()
    : $("#CommDetailDIV" + FlightRefid).slideUp();
  $("#TotalTaxDIV" + FlightRefid).slideUp();
}
function chkunchk() {
  //debugger;
  if (document.getElementById("chkdiscccfare").checked === true) {
    $(".discui").css({ display: "block" });
    $(".actu").css({ display: "block" });
    $(".einr").css("text-decoration", "line-through red");
  } else {
    $(".discui").css({ display: "none" });
    $(".actu").css({ display: "none" });
    $(".einr").css("text-decoration", "");
  }
}
window.chkunchk = chkunchk;

/*
        function Continue(refid) {
            debugger;

            
            //var dep = reqList[0]._DepartureStation;
            //var dep1 = reqList[1]._DepartureStation;
            //for (var i = 0; i < msg.d.length; i++) {
            //for (var i = 0; i < reqList.length; i++) {
            //    var org[] = reqList[1]._DepartureStation.Split(',');
            //    var dis = org[0];
            //    alert(dis);
            //};



        //var SrNo = parseInt($('#hdnCurrSrNo').val()) + 1;
        //GetCityBySrNo(refid,SrNo);
        //debugger;
        BookNow(refid);
        //GetShowSelectFlight(refid,0)
    };*/

/*function CallNextFlights(SrNo) {


            var CompnyID = $("#hdncmpid").val();
            var hostName = $('#hdnhostName').val();
            SelectedFlightRefId = "";
            $(("#bookSummary" + SrNo.toString())).text("---");

            debugger;
            $("#numFLAdults").val(reqList[SrNo-1]._NoOfAdult);
            $("#numFLChildren").val(reqList[SrNo-1]._NoOfChild);
            $("#numFLInfants").val(reqList[SrNo - 1]._NoOfInfant);
            var _dep = reqList[SrNo - 1]._DepartureStation;
            var _arv =reqList[SrNo - 1]._ArrivalStation;

            var arrdep = _dep.split(",");
            var arrArr = _arv.split(",");

            $("#Hiddenoricity").val(arrdep[0]);
            $("#HiddenArrCity").val(arrArr[0]);
            $("#DepDate").val(reqList[SrNo-1]._BeginDate);

            $("#depCityWaiting").text($("#Hiddenoricity").val());
            $("#departdate").text($("#DepDate").val());
            $("#ArrCityWaiting").text($("#HiddenArrCity").val());
            $("#paxty1").text($("#numFLAdults").val());
            $("#paxty2").text($("#numFLChildren").val());
            $("#paxty").text($("#numFLInfants").val());
      

            $("#CenterwaitingDiv").css("display", "Block");
            
            $.ajax({
                type: "POST",
                url: "//" + hostName + "/n_SearchDataFilter.aspx/GetvalueForRequestMulticityBySrNo",
                //data: "{'objclsPSQ':" + JSON.stringify(params) + "}",
                data: "{'_SrNo':" + SrNo + "}",
                contentType: "application/json; charset=utf-8", //Set Content-Type
                dataType: "json", // Set return Data Type
                cache: false,
                success: function (result) {
                    
                    debugger;
                    //var hostName = $('#hdnhostName').val();
                    //alert(hostName);
                    var obj = result.d;
                    if (obj != "") {
                        debugger;
                        //location.href = "//" + hostName + "/k_multicity.aspx";
                        //location.href = "//" + hostName + "/k_multicity1.aspx";
                        //GetShow();
                        $('#hdnCurrSrNo').val(SrNo);
                        Reset();
                        GetShowData();
                        $("#CenterwaitingDiv").css("display", "none");    
                        debugger;

                    }
                    else {
                        location.href = "//" + hostName + "/k_modifysearch.aspx";
                    }
                },
                error: function (xhr, msg, e) {
                    debugger;
                    alert("getresult()\n" + msg + "  " + e + " " + xhr);
                    location.href = "//" + hostName + "/Index.aspx";
                }
            });//------- end next page

        }
        */

function showFarerule(FlightRefid) {
  $(".FareSummaryClass").parent().hide("slow");
  $(".flight_SUB").hide("slow");
  //debugger;
  //alert(FlightRefid);
  var fRef = FlightRefid;
  if ($("#FarerulesummeryPopup" + fRef + "").css("display") === "none") {
    $("#FarerulesummeryPopup" + fRef + "").show("slow");
  } else {
    $("#FarerulesummeryPopup" + fRef + "").hide("slow");
  }
}
window.showFarerule = showFarerule;
function showBookItemDtl(FlightRefid) {
  $(".FareSummaryClass").parent().hide("slow");
  $(".flight_SUB").hide("slow");
  var fRef = FlightRefid;
  //debugger;
  if ($("#divshowBookSmry" + fRef + "").css("display") === "none") {
    $("#divshowBookSmry" + fRef + "").show("slow");
  } else {
    $("#divshowBookSmry" + fRef + "").hide("slow");
  }
}
window.showBookItemDtl = showBookItemDtl;

function CloseItnaryPopup() {
  $("#ItnaryPopup").css("display", "none");
  //SelectedFlightRefId = "";
  //jQuery('#flightRow').html('');
  //$("#itinearyWithFarerule").css("background-color", "");
  //$('#itinearyWithFarerule').fadeOut();
  //$('#ItnaryPopup').fadeOut();
}
window.CloseItnaryPopup = CloseItnaryPopup;
function CloseFlightNotfoundPopup() {
  $("#FlightNotfoundPopup").css("display", "none");
  //$("#itinearyWithFarerule").css("background-color", "");
  //$('#itinearyWithFarerule').fadeOut();
  //$('#ItnaryPopup').fadeOut();
}
window.CloseFlightNotfoundPopup = CloseFlightNotfoundPopup;
var flagOnwardPrice = 1;
function OnwardPrice() {
  $("#modelpopupOUTER").css("display", "Block");
  $("#modelpopup").css("background-color", "Gray");
  $("#modelpopup").delay(1).fadeIn(400);
  $("#modelpopupOUTER").delay(1).fadeIn(400);

  $("#AirlineSort").css("border-bottom", "0px solid #C0C0C0");
  $("#DepartSort").css("border-bottom", "0px solid #C0C0C0");
  $("#ArriveSort").css("border-bottom", "0px solid #C0C0C0");
  $("#StopSort").css("border-bottom", "0px solid #C0C0C0");
  $("#PriceSort").css("border-bottom", "3px solid #FEBD25");
  jQuery("#flightRow").html("");
  var msg;
  if (flagOnwardPrice === 0) {
    flagOnwardPrice = 1;
    msg = sortF;
    msg.d.sort(function (obj1, obj2) {
      return parseFloat(obj1.TotalAmount) < parseFloat(obj2.TotalAmount)
        ? -1
        : parseFloat(obj1.TotalAmount) > parseFloat(obj2.TotalAmount)
        ? 1
        : 0;
    });
  } else {
    flagOnwardPrice = 0;
    msg = sortF;
    msg.d.sort(function (obj1, obj2) {
      return parseFloat(obj1.TotalAmount) > parseFloat(obj2.TotalAmount)
        ? -1
        : parseFloat(obj1.TotalAmount) < parseFloat(obj2.TotalAmount)
        ? 1
        : 0;
    });
  }
  for (var i = 0; i < msg.d.length; i++) {
    $("#MyTemplate").tmpl(msg.d[i]).appendTo("#flightRow");
  }
  chkunchk();

  $("#modelpopup").delay(1000).fadeOut(400);
  $("#modelpopupOUTER").delay(1000).fadeOut(400);
}
window.OnwardPrice = OnwardPrice;
var flag = 0;
function OnwardAirline() {
  $("#modelpopupOUTER").css("display", "Block");
  $("#modelpopup").css("background-color", "Gray");
  $("#modelpopup").delay(1).fadeIn(400);
  $("#modelpopupOUTER").delay(1).fadeIn(400);

  $("#AirlineSort").css("border-bottom", "3px solid #FEBD25");
  $("#DepartSort").css("border-bottom", "0px solid #C0C0C0");
  $("#ArriveSort").css("border-bottom", "0px solid #C0C0C0");
  $("#StopSort").css("border-bottom", "0px solid #C0C0C0");
  $("#PriceSort").css("border-bottom", "0px solid #C0C0C0");

  jQuery("#flightRow").html("");
  var msg;
  if (flag === 0) {
    flag = 1;
    msg = sortF;
    msg.d.sort(function (obj1, obj2) {
      return obj1.FlightName < obj2.FlightName
        ? -1
        : obj1.FlightName > obj2.FlightName
        ? 1
        : 0;
    });
  } else {
    flag = 0;
    msg = sortF;
    msg.d.sort(function (obj1, obj2) {
      return obj1.FlightName > obj2.FlightName
        ? -1
        : obj1.FlightName < obj2.FlightName
        ? 1
        : 0;
    });
  }
  for (var i = 0; i < msg.d.length; i++) {
    $("#MyTemplate").tmpl(msg.d[i]).appendTo("#flightRow");
  }
  chkunchk();

  $("#modelpopup").delay(1000).fadeOut(400);
  $("#modelpopupOUTER").delay(1000).fadeOut(400);
}
window.OnwardAirline = OnwardAirline;
var flagDepart = 0;
function OnwardDepart() {
  $("#modelpopupOUTER").css("display", "Block");
  $("#modelpopup").css("background-color", "Gray");
  $("#modelpopup").delay(1).fadeIn(400);
  $("#modelpopupOUTER").delay(1).fadeIn(400);

  $("#AirlineSort").css("border-bottom", "0px solid #C0C0C0");
  $("#DepartSort").css("border-bottom", "3px solid #FEBD25");
  $("#ArriveSort").css("border-bottom", "0px solid #C0C0C0");
  $("#StopSort").css("border-bottom", "0px solid #C0C0C0");
  $("#PriceSort").css("border-bottom", "0px solid #C0C0C0");
  jQuery("#flightRow").html("");
  var msg;
  if (flagDepart === 0) {
    flagDepart = 1;
    msg = sortF;
    msg.d.sort(function (obj1, obj2) {
      return obj1.FlightDepTime < obj2.FlightDepTime
        ? -1
        : obj1.FlightDepTime > obj2.FlightDepTime
        ? 1
        : 0;
    });
  } else {
    flagDepart = 0;
    msg = sortF;
    msg.d.sort(function (obj1, obj2) {
      return obj1.FlightDepTime > obj2.FlightDepTime
        ? -1
        : obj1.FlightDepTime < obj2.FlightDepTime
        ? 1
        : 0;
    });
  }
  for (var i = 0; i < msg.d.length; i++) {
    $("#MyTemplate").tmpl(msg.d[i]).appendTo("#flightRow");
  }
  chkunchk();

  $("#modelpopup").delay(1000).fadeOut(400);
  $("#modelpopupOUTER").delay(1000).fadeOut(400);
}
window.OnwardDepart = OnwardDepart;
var flagArrive = 0;
function OnwardArrive() {
  $("#modelpopupOUTER").css("display", "Block");
  $("#modelpopup").css("background-color", "Gray");
  $("#modelpopup").delay(1).fadeIn(400);
  $("#modelpopupOUTER").delay(1).fadeIn(400);

  $("#AirlineSort").css("border-bottom", "0px solid #C0C0C0");
  $("#DepartSort").css("border-bottom", "0px solid #C0C0C0");
  $("#ArriveSort").css("border-bottom", "3px solid #FEBD25");
  $("#StopSort").css("border-bottom", "0px solid #C0C0C0");
  $("#PriceSort").css("border-bottom", "0px solid #C0C0C0");
  jQuery("#flightRow").html("");
  var msg;
  if (flagArrive === 0) {
    flagArrive = 1;
    msg = sortF;
    msg.d.sort(function (obj1, obj2) {
      return obj1.FlightArrTime < obj2.FlightArrTime
        ? -1
        : obj1.FlightArrTime > obj2.FlightArrTime
        ? 1
        : 0;
    });
  } else {
    flagArrive = 0;
    msg = sortF;
    msg.d.sort(function (obj1, obj2) {
      return obj1.FlightArrTime > obj2.FlightArrTime
        ? -1
        : obj1.FlightArrTime < obj2.FlightArrTime
        ? 1
        : 0;
    });
  }
  for (var i = 0; i < msg.d.length; i++) {
    $("#MyTemplate").tmpl(msg.d[i]).appendTo("#flightRow");
  }
  chkunchk();

  $("#modelpopup").delay(1000).fadeOut(400);
  $("#modelpopupOUTER").delay(1000).fadeOut(400);
}
window.OnwardArrive = OnwardArrive;
var flagStop = 0;
function OnwardStop() {
  $("#modelpopupOUTER").css("display", "Block");
  $("#modelpopup").css("background-color", "Gray");
  $("#modelpopup").delay(1).fadeIn(400);
  $("#modelpopupOUTER").delay(1).fadeIn(400);

  $("#AirlineSort").css("border-bottom", "0px solid #C0C0C0");
  $("#DepartSort").css("border-bottom", "0px solid #C0C0C0");
  $("#ArriveSort").css("border-bottom", "0px solid #C0C0C0");
  $("#StopSort").css("border-bottom", "3px solid #FEBD25");
  $("#PriceSort").css("border-bottom", "0px solid #C0C0C0");
  jQuery("#flightRow").html("");
  var msg;
  if (flagStop === 0) {
    flagStop = 1;
    msg = sortF;
    msg.d.sort(function (obj1, obj2) {
      return obj1.connection < obj2.connection
        ? -1
        : obj1.connection > obj2.connection
        ? 1
        : 0;
    });
  } else {
    flagStop = 0;
    msg = sortF;
    msg.d.sort(function (obj1, obj2) {
      return obj1.connection > obj2.connection
        ? -1
        : obj1.connection < obj2.connection
        ? 1
        : 0;
    });
  }
  for (var i = 0; i < msg.d.length; i++) {
    $("#MyTemplate").tmpl(msg.d[i]).appendTo("#flightRow");
  }
  chkunchk();

  $("#modelpopup").delay(1000).fadeOut(400);
  $("#modelpopupOUTER").delay(1000).fadeOut(400);
}
window.OnwardStop = OnwardStop;
function GetFlightMatrix() {
  jQuery("#FlightMatrixs2").html("");
  jQuery("#FlightMatrixs").html("");

  $.ajax({
    type: "POST",
    url: "/Flight/FlightMatrix",
    data: "{}",
    contentType: "application/json; charset=utf-8",
    dataType: "json",
    success: function (msgs) {
      if (msgs.d.length < 10) {
        $("#content-2").css("display", "none");
        $("#templeteFlightMatrix").tmpl(msgs.d).appendTo("#FlightMatrixs2");
      } else {
        $("#content-2").css("display", "block");
        $("#templeteFlightMatrixINT").tmpl(msgs.d).appendTo("#FlightMatrixs");
      }
    },
    error: function (msgs) {},
  });
}
window.GetFlightMatrix = GetFlightMatrix;
function showmodifystatus() {
  //debugger;
  if ($("#modifySearch").css("display") === "none") {
    $("#modifySearch").show();
    document.getElementById("modifypart").style.display = "block";
    document.getElementById("mldt1").style.display = "block";
    document.getElementById("pldt1").style.display = "none";
  } else {
    $("#modifySearch").hide();
    document.getElementById("modifypart").style.display = "none";
    document.getElementById("mldt1").style.display = "none";
    document.getElementById("pldt1").style.display = "block";
  }
  //SetValueInModifyControl();
  //SetValueInModifyControlModifypage();
  // debugger;
  ModifyMulticityshow();
  var _list = JSON.parse($("#hdnclsPSQBOList").val());
  // SetValueInModifyControlModifypage_1(_list);
}
window.showmodifystatus = showmodifystatus;
function SetValueInModifyControlMC() {
  //debugger;
  //var ModifyType = $('#modifyfaretype').val();
  var ModifyType = "MC";
  if (ModifyType === "R" || ModifyType === "RT") {
    $("#OnwayModDiv").addClass("whcolor");
    $("#RTModDiv").addClass("gbcolor");
    $("#DiscRTModDiv").addClass("whcolor");
  } else if (ModifyType === "O") {
    $("#OnwayModDiv").addClass("gbcolor");
    $("#RTModDiv").addClass("whcolor");
    $("#DiscRTModDiv").addClass("whcolor");
  } else if (ModifyType === "DRT") {
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
window.SetValueInModifyControlMC = SetValueInModifyControlMC;
function myclipopse() {
  debugger;
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
// function checksum(value, effectedvalue) {
//   console.log();

//   var type = value.id.split("_")[1];
//   var iddata = value.id.split("_")[0];
//   if (type.toUpperCase() == "PLUS") {
//     if (iddata == "infant") {
//       var abcd = document.getElementById("infant").value;
//       if (abcd < adult_value) {
//         abcd++;
//         document.getElementById(iddata).value = abcd;
//       }
//     }
//     if (adult_value + child_value < 9) {
//       if (iddata == "adult") {
//         adult_value++;
//         document.getElementById(iddata).value = adult_value;
//       }
//       if (iddata == "child") {
//         child_value++;
//         document.getElementById(iddata).value = child_value;
//       }
//     }
//   } else if (type.toUpperCase() == "MINUS") {
//     if (adult_value + child_value > 1) {
//       if (iddata == "adult") {
//         if (adult_value > 1) {
//           adult_value--;
//           document.getElementById(iddata).value = adult_value;
//           document.getElementById("infant").value = 0;
//         }
//       }
//       if (iddata == "child") {
//         if (child_value > 0) {
//           child_value--;
//           document.getElementById(iddata).value = child_value;
//         }
//       }
//     }
//     if (iddata == "infant") {
//       var abcd = document.getElementById("infant").value;
//       if (abcd > 0) {
//         abcd--;
//         document.getElementById(iddata).value = abcd;
//       }
//     }
//   }
// }
window.checksum = checksum;
function prefer() {
  //debugger;
  //var ll = document.getElementById('flighimg');

  //if (ll.style.display != "none") {
  //    ll.style.display = "none";
  //}
  //else {
  //    ll.style.display = "block";
  //}
  //SetValueInModifyControl();
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
//function ModifyRoundWay() {

//}

function closepoprt() {
  document.getElementById("myModalopi").style.display = "none";
}
window.closepoprt = closepoprt;
function showfilteee() {
  var ll = document.getElementById("filters_panel");

  if (ll.style.display != "none") {
    ll.style.display = "none";
  } else {
    ll.style.display = "block";
  }
}
window.showfilteee = showfilteee;
//     $('a.go-right').click(function (event) {
//         //debugger;
//    var pos = $('div.overflow-hidden').scrollLeft() + 50;
//    $('div.overflow-hidden').scrollLeft(pos);
//});

//$('.go-right').click(function () {
//    var far = $('div.overflow-hidden').width();
//    var pos = $('div.overflow-hidden').scrollLeft() + far;
//    $('div.overflow-hidden').animate({ scrollLeft: pos }, far, 'easeOutQuad');
//});

//$('a.go-right').click(function (event) {
//    var pos = $('.overflow-hidden').scrollLeft() + 50;
//    $('.overflow-hidden').scrollLeft(pos);
//});

$(window).load(function () {
  //showmodifystatus();
  SetValueInModifyControl();
  // calculatenofpax();
});

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
      // $("#modelpopupOUTER").css("display", "Block");
      // $("#modelpopup").css("background-color", "Gray");
      // $("#modelpopup").delay(1).fadeIn(400);
      // $("#modelpopupOUTER").delay(1).fadeIn(400);
      // var msg = sortF;
      document.getElementById("hdnminarrtime").value = ui.values[0];
      document.getElementById("hdnmaxarrtime").value = ui.values[1];
      // jQuery("#flightRow").html("");
      // var i = 0;
      // var j = msg.d.length;

      // for (i; i < j; i++) {
      //   var arrivalTimes =
      //     Number(msg.d[i].FlightArrTime.substring(0, 2)) * 60 +
      //     Number(msg.d[i].FlightArrTime.substring(3, 5));

      //   if (arrivalTimes >= ui.values[0] && arrivalTimes <= ui.values[1]) {
      //     $("#MyTemplate").tmpl(msg.d[i]).appendTo("#flightRow");
      //   }
      // }

      getCheckedFilters(flagSort);
      //chkunchk();
      // $("#modelpopup").delay(1000).fadeOut(400);
      // $("#modelpopupOUTER").delay(1000).fadeOut(400);
    },
  });
  $("#amountArrivalTimes").val("Time:" + "00:00" + "-Time " + "24:00");
}
window.GetMinMaxArrivalTime = GetMinMaxArrivalTime;

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
      // $("#modelpopupOUTER").css("display", "Block");
      // $("#modelpopup").css("background-color", "Gray");
      // $("#modelpopup").delay(1).fadeIn(400);
      // $("#modelpopupOUTER").delay(1).fadeIn(400);
      // var msg = sortF;
      document.getElementById("hdnmindeptime").value = ui.values[0];
      document.getElementById("hdnmaxdeptime").value = ui.values[1];
      // jQuery("#flightRow").html("");
      // var i = 0;
      // var j = msg.d.length;

      // for (i; i < j; i++) {
      //   var departureTimes =
      //     Number(msg.d[i].FlightDepTime.substring(0, 2)) * 60 +
      //     Number(msg.d[i].FlightDepTime.substring(3, 5));

      //   if (departureTimes >= ui.values[0] && departureTimes <= ui.values[1]) {
      //     $("#MyTemplate").tmpl(msg.d[i]).appendTo("#flightRow");
      //   }
      // }

      getCheckedFilters(flagSort);
      //chkunchk();
      // $("#modelpopup").delay(1000).fadeOut(400);
      // $("#modelpopupOUTER").delay(1000).fadeOut(400);
    },
  });
  $("#amountDepartureTimes").val("Time:" + "00:00" + "-Time " + "24:00");
}
window.GetMinMaxDepartureTime = GetMinMaxDepartureTime;

function getCheckedFilters(SortCriteria) {
  $("#modelpopupOUTER").css("display", "Block");
  $("#modelpopup").css("background-color", "Gray");
  $("#modelpopup").delay(1).fadeIn(400);
  $("#modelpopupOUTER").delay(1).fadeIn(400);
  jQuery("#flightRow").html("");
  var msg = sortF;
  var groupByFlightNumber = getSortedDetails(msg, SortCriteria, "I");

  // console.log(groupByFlightNumber);x

  //  var newmsg = new Array();
  // var chkboxValuestops = new Array();
  // var chkboxValueairline = new Array();

  var minimum = parseFloat(document.getElementById("hdnminval").value);
  var maximum = parseFloat(document.getElementById("hdnmaxval").value);
  var minideptime = parseFloat(document.getElementById("hdnmindeptime").value);
  var maxideptime = parseFloat(document.getElementById("hdnmaxdeptime").value);
  var miniarrtime = document.getElementById("hdnminarrtime").value;
  var maxiarrtime = document.getElementById("hdnmaxarrtime").value;

  let checkedStops =
    document.getElementById("hdncheckedstops").value != ""
      ? document.getElementById("hdncheckedstops").value.split(",")
      : [];
  let checkedAirline =
    document.getElementById("hdncheckedpreferredairline").value != ""
      ? document.getElementById("hdncheckedpreferredairline").value.split(",")
      : [];
  let prefferdArlines =
    document.getElementById("hdnpreferredairline").value != ""
      ? document.getElementById("hdnpreferredairline").value.split(",")
      : [];
  let stops =
    document.getElementById("hdnstops").value != ""
      ? document.getElementById("hdnstops").value.split(",")
      : [];
  // Determine which airline list to use.
  const airlineFilter = checkedAirline.length
    ? checkedAirline
    : prefferdArlines;

  Object.keys(groupByFlightNumber).forEach(function (key) {
    var group = groupByFlightNumber[key];
    var filteredGroup = group.filter(function (flight) {
      var TotalAmount = parseFloat(flight.TotalAmount);
      // Determine which stop list to use.
      const stopFilter = checkedStops.length ? checkedStops : stops;

      // Allow stop check to pass if both stop lists are empty.
      const isStopValid =
        stopFilter.length === 0 || stopFilter.includes(flight.Stop);
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
        TotalAmount >= minimum &&
        TotalAmount <= maximum &&
        FlightDepTime >= minideptime &&
        FlightDepTime <= maxideptime &&
        FlightArrTime >= miniarrtime &&
        FlightArrTime <= maxiarrtime &&
        airlineFilter.includes(flight.FlightName) &&
        isStopValid
      );
    });
    if (filteredGroup.length > 0) {
      $("#MyTemplate").tmpl(filteredGroup[0]).appendTo("#flightRow");
      $("#multipleDetails")
        .tmpl(filteredGroup)
        .appendTo("#Price3-" + key);
      $("#multipleDetails2")
        .tmpl(filteredGroup)
        .appendTo("#DR-" + key);
    }
  });

  // for (i; i < j; i++) {
  //   var departureTimes =
  //     Number(msg.d[i].FlightDepTime.substring(0, 2)) * 60 +
  //     Number(msg.d[i].FlightDepTime.substring(3, 5));
  //   var arrivalTimes =
  //     Number(msg.d[i].FlightArrTime.substring(0, 2)) * 60 +
  //     Number(msg.d[i].FlightArrTime.substring(3, 5));
  //   var TotalAmount = parseFloat(msg.d[i].TotalAmount);

  //   if (checkedStops.length != 0 && stops.length != 0) {
  //     if (checkedStops.length != 0) {
  //       if (checkedAirline.length != 0) {
  //         if (
  //           departureTimes >= minideptime &&
  //           departureTimes <= maxideptime &&
  //           arrivalTimes >= miniarrtime &&
  //           arrivalTimes <= maxiarrtime &&
  //           TotalAmount >= minimum &&
  //           TotalAmount <= maximum &&
  //           checkedStops.includes(msg.d[i].Stop) &&
  //           checkedAirline.includes(msg.d[i].FlightName)
  //         ) {
  //           $("#MyTemplate").tmpl(msg.d[i]).appendTo("#flightRow");
  //         }
  //       } else {
  //         if (
  //           departureTimes >= minideptime &&
  //           departureTimes <= maxideptime &&
  //           arrivalTimes >= miniarrtime &&
  //           arrivalTimes <= maxiarrtime &&
  //           TotalAmount >= minimum &&
  //           TotalAmount <= maximum &&
  //           checkedStops.includes(msg.d[i].Stop) &&
  //           prefferdArlines.includes(msg.d[i].FlightName)
  //         ) {
  //           $("#MyTemplate").tmpl(msg.d[i]).appendTo("#flightRow");
  //         }
  //       }
  //     } else if (stops.length != 0) {
  //       if (checkedAirline.length != 0) {
  //         if (
  //           departureTimes >= minideptime &&
  //           departureTimes <= maxideptime &&
  //           arrivalTimes >= miniarrtime &&
  //           arrivalTimes <= maxiarrtime &&
  //           TotalAmount >= minimum &&
  //           TotalAmount <= maximum &&
  //           stops.includes(msg.d[i].Stop) &&
  //           checkedAirline.includes(msg.d[i].FlightName)
  //         ) {
  //           $("#MyTemplate").tmpl(msg.d[i]).appendTo("#flightRow");
  //         }
  //       } else {
  //         if (
  //           departureTimes >= minideptime &&
  //           departureTimes <= maxideptime &&
  //           arrivalTimes >= miniarrtime &&
  //           arrivalTimes <= maxiarrtime &&
  //           TotalAmount >= minimum &&
  //           TotalAmount <= maximum &&
  //           stops.includes(msg.d[i].Stop) &&
  //           prefferdArlines.includes(msg.d[i].FlightName)
  //         ) {
  //           $("#MyTemplate").tmpl(msg.d[i]).appendTo("#flightRow");
  //         }
  //       }
  //     }
  //   } else {
  //     if (checkedAirline.length != 0) {
  //       if (
  //         departureTimes >= minideptime &&
  //         departureTimes <= maxideptime &&
  //         arrivalTimes >= miniarrtime &&
  //         arrivalTimes <= maxiarrtime &&
  //         TotalAmount >= minimum &&
  //         TotalAmount <= maximum &&
  //         checkedAirline.includes(msg.d[i].FlightName)
  //       ) {
  //         $("#MyTemplate").tmpl(msg.d[i]).appendTo("#flightRow");
  //       }
  //     } else {
  //       if (
  //         departureTimes >= minideptime &&
  //         departureTimes <= maxideptime &&
  //         arrivalTimes >= miniarrtime &&
  //         arrivalTimes <= maxiarrtime &&
  //         TotalAmount >= minimum &&
  //         TotalAmount <= maximum &&
  //         prefferdArlines.includes(msg.d[i].FlightName)
  //       ) {
  //         $("#MyTemplate").tmpl(msg.d[i]).appendTo("#flightRow");
  //       }
  //     }
  //   }
  // }
  // for (let i = 0; i < j; i++) {
  //   const flight = msg.d[i];

  //   // Calculate departure and arrival times in minutes.
  //   const departureTime =
  //     Number(flight.FlightDepTime.substring(0, 2)) * 60 +
  //     Number(flight.FlightDepTime.substring(3, 5));
  //   const arrivalTime =
  //     Number(flight.FlightArrTime.substring(0, 2)) * 60 +
  //     Number(flight.FlightArrTime.substring(3, 5));

  //   // Parse the total flight amount.
  //   const totalAmount = parseFloat(flight.TotalAmount);

  //   // Determine which airline list to use.
  //   const airlineFilter = checkedAirline.length ? checkedAirline : prefferdArlines;

  //   // Determine which stop list to use.
  //   const stopFilter = checkedStops.length ? checkedStops : stops;

  //   // Allow stop check to pass if both stop lists are empty.
  //   const isStopValid =
  //     stopFilter.length === 0 || stopFilter.includes(flight.Stop);

  //   // Check all conditions at once.
  //   if (
  //     departureTime >= minideptime &&
  //     departureTime <= maxideptime &&
  //     arrivalTime >= miniarrtime &&
  //     arrivalTime <= maxiarrtime &&
  //     totalAmount >= minimum &&
  //     totalAmount <= maximum &&
  //     airlineFilter.includes(flight.FlightName) &&
  //     isStopValid
  //   ) {
  //     $("#MyTemplate").tmpl(flight).appendTo("#flightRow");
  //   }
  // }

  // $("table.AirlinesChkinfotb")
  //   .find("input:checkbox")
  //   .each(function () {
  //     if (this.checked != true) {
  //       var v = $(this).val();
  //       chkboxValueairline.push(v);
  //     }
  //   });
  // $("ul.amenView")
  //   .find("input:checkbox")
  //   .each(function () {
  //     if (this.checked != true) {
  //       var v = $(this).val();
  //       chkboxValuestops.push(v);
  //     }
  //   });

  // var groupByFlightNumber = getSortedDetails(msg, SortCriteria, "I");

  // if (chkboxValuestops.length == 0 || chkboxValueairline.length == 0) {
  //   Object.keys(groupByFlightNumber).forEach(function (key) {
  //     jQuery("#flightRow").html("");
  //     jQuery("#Price3-" + key).html("");
  //     jQuery("#DR-" + key).html("");
  //   });
  // } else {
  //   Object.keys(groupByFlightNumber).forEach(function (key) {
  //     jQuery("#tempView").html("");
  //     jQuery("#Price3-" + key).html("");
  //     jQuery("#DR-" + key).html("");
  //   });
  //   //jQuery('#tempView').html('');

  //   var minimum = parseFloat(document.getElementById("hdnminval").value);
  //   var maximum = parseFloat(document.getElementById("hdnmaxval").value);
  //   var minideptime = parseFloat(
  //     document.getElementById("hdnmindeptime").value
  //   );
  //   var maxideptime = parseFloat(
  //     document.getElementById("hdnmaxdeptime").value
  //   );
  //   var miniarrtime = document.getElementById("hdnminarrtime").value;
  //   var maxiarrtime = document.getElementById("hdnmaxarrtime").value;

  //   Object.keys(groupByFlightNumber).forEach(function (key) {
  //     var group = groupByFlightNumber[key];

  //     // Check if the group satisfies the filters
  //     var filteredGroup = group.filter(function (flight) {
  //       var TotalAmount = parseFloat(flight.TotalAmount);
  //       var FlightDepTime =
  //         parseFloat(
  //           parseFloat(
  //             flight.FlightDepTime.substring(0, 2) +
  //               "." +
  //               flight.FlightDepTime.substring(3, 5)
  //           )
  //         ) * 60;
  //       var FlightArrTime =
  //         parseFloat(
  //           parseFloat(
  //             flight.FlightArrTime.substring(0, 2) +
  //               "." +
  //               flight.FlightArrTime.substring(3, 5)
  //           )
  //         ) * 60;
  //       return (
  //         chkboxValuestops.includes(flight.Stop) &&
  //         chkboxValueairline.includes(flight.FlightName) &&
  //         TotalAmount >= minimum &&
  //         TotalAmount <= maximum &&
  //         FlightDepTime >= minideptime &&
  //         FlightDepTime <= maxideptime &&
  //         FlightArrTime >= miniarrtime &&
  //         FlightArrTime <= maxiarrtime
  //       );
  //     });

  //     console.log(filteredGroup[0]);
  //     if (filteredGroup.length > 0) {
  //       console.log(filteredGroup[0]);

  //       $("#MyTemplate").tmpl(filteredGroup[0]).appendTo("#flightRow");
  //       $("#multipleDetails")
  //         .tmpl(filteredGroup)
  //         .appendTo("#Price3-" + key);
  //       $("#multipleDetails2")
  //         .tmpl(filteredGroup)
  //         .appendTo("#DR-" + key);
  //     }
  //   });
  // }

  $("#modelpopup").delay(1000).fadeOut(400);
  $("#modelpopupOUTER").delay(1000).fadeOut(400);
  //chkunchk();
}
window.getCheckedFilters = getCheckedFilters;
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
    let chkboxValue = new Array();
    $("ul.amenViewairline")
      .find("input:checkbox")
      .each(function () {
        this.checked = true;
        chkboxValue.push($(this).val());
      });
      document.getElementById("hdncheckedpreferredairline").value =
      chkboxValue.join(",");
  } else {
    $("ul.amenViewairline")
      .find("input:checkbox")
      .each(function () {
        {
          this.checked = false;
        }
      });
      document.getElementById("hdncheckedpreferredairline").value =
      "";
  }

  getCheckedFilters(flagSort);
  //chkunchk();
}
window.getAll = getAll;