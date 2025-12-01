$(document).ready(function () {
  var today = new Date();
  var dd = today.getDate();
  var mm = today.getMonth() + 1; //January is 0!
  var yyyy = today.getFullYear();
  if (dd < 10) {
    dd = "0" + dd;
  }
  if (mm < 10) {
    mm = "0" + mm;
  }
  today = dd + "/" + mm + "/" + yyyy;
  $("#FlightSearch_txtDepartureDate").val(today);
  $("#FlightSearch_txtReturnDate").val(today);
  $("#FlightSearch_txtDepartureDate").datepicker({
    minDate: 0,
    dateFormat: "dd/mm/yy",
    numberOfMonths: 2,
    onSelect: function (dateStr) {
      //
      var d = $.datepicker.parseDate("dd/mm/yy", dateStr);
      $("#FlightSearch_txtReturnDate").datepicker("setDate", d);
      var start = $(this).datepicker("getDate");
      $("#FlightSearch_txtReturnDate").datepicker(
        "option",
        "minDate",
        new Date(start.getTime())
      );
      //$("#FlightSearch_txtReturnDate").focus();
    },
    onClose: function (selectedDate) {
      if ($("#oneway").css("display") == "block") {
        $("#FlightSearch_txtReturnDate").focus();
        $this.datepicker("show");
      }
    },
  });

  $("#FlightSearch_txtReturnDate").datepicker({
    minDate: 0,
    dateFormat: "dd/mm/yy",
    numberOfMonths: 2,
    onSelect: function (dateStr) {
      //
      if (
        $("#FlightSearch_txtDepartureDate").val() >=
        $.datepicker.parseDate("dd/mm/yy", dateStr)
      ) {
        //
        var t1 = dateStr;
        var d1 = $.datepicker.parseDate("dd/mm/yy", t1);
        $("#FlightSearch_txtReturnDate").datepicker("setDate", d1);
        var start1 = $(this).datepicker("getDate");
        $("#FlightSearch_txtReturnDate").datepicker(
          "option",
          "minDate",
          new Date(start1.getTime())
        );
        var t2 = dateStr;
        var d2 = $.datepicker.parseDate("dd/mm/yy", t2);
        $("#FlightSearch_txtDepartureDate").datepicker("setDate", d2);
      } else {
        var t1 = dateStr;
        var d1 = $.datepicker.parseDate("dd/mm/yy", t1);
        $("#FlightSearch_txtReturnDate").datepicker("setDate", d1);
        $("#FlightSearch_txtDepartureDate").datepicker(
          "setDate",
          $("#FlightSearch_txtDepartureDate").val()
        );
      }
    },
    onClose: function (selectedDate) {
      $("#ddlTravelClass").focus();
    },
  });

  // $("#oricity").autocomplete({
  //     autoFocus: true
  // });
  // $("#desticity").autocomplete({
  //     autoFocus: true
  // });

  //================== Multicity Calender validation start==================================//

  var mindatesel = today;
  $("#Mdepartdate_1")
    .datepicker({
      minDate: mindatesel,
      maxDate: "+1Y",
      selectedDate: "0",
      dateFormat: "dd/mm/yy",
      numberOfMonths: 2,
      showButtonPanel: true,
      closeText: "",
      onSelect: function (selectedDate) {
        //var d = $.datepicker.parseDate('dd/mm/yy', selectedDate);
        //$('#Mdepartdate_2').datepicker('setDate', d);
        //var start = $(this).datepicker('getDate');
        //$('#Mdepartdate_2').datepicker('option', 'minDate', new Date(start.getTime()));
        $("#Mdepartdate_2").val(selectedDate);

        if ($("#myCityDiv").html() != "") {
          $("#myCityDiv")
            .find(":input")
            .each(function () {
              switch (this.type) {
                case "text":
                  $(this).val("");
                  break;
              }
            });
        }
      },
      onClose: function (selectedDate) {},
    })
    .datepicker("setDate", mindatesel);
  $("body").on("focus", ".Mdepart", function () {
    // $('.Mdepart').live('focus', function () {

    var Id = this.id.split("_")[1];
    var mymindate = "";
    var mymaxdate = "";
    if (Id === 2) {
      mymindate = $.trim($("#Mdepartdate_" + (Id - 1)).val());
      //mymaxdate = '0';
    } else {
      mymindate = $.trim($("#Mdepartdate_" + (Id - 1)).val());
      mymaxdate = $.trim($("#Mdepartdate_" + (parseInt(Id) + 1)).val());
    }
    var $this = $(this);

    $this.datepicker({
      minDate: mymindate,
      maxDate: mymaxdate,
      selectedDate: "0",
      dateFormat: "dd/mm/yy",
      numberOfMonths: 2,
      showButtonPanel: true,
      closeText: "",
      onSelect: function () {
        if (Id === 3) {
          mymaxdate = $("#Mdepartdate_" + Id).val();
          $("#Mdepartdate_" + (Id - 1)).datepicker(
            "option",
            "maxDate",
            mymaxdate
          );
        } else {
          mymindate = $("#Mdepartdate_" + Id).val();
          var minforreturn = parseInt(Id) + 1;
          $("#Mdepartdate_" + minforreturn).datepicker(
            "option",
            "minDate",
            mymindate
          );
        }
      },
      onClose: function () {},
    });
    $this.datepicker("show");
  });
  //================== Multicity Calender validation end==================================//

  //================== Multicity Sector start==================================//
  $("body").on("focus", ".multicitysector", function () {
    $(".multicitysector").autocomplete({
      autoFocus: true,
      source: function (request, response) {
        $.ajax({
          url: "/flight/GetSector",
          data: { searchTerm: request.term },
          dataType: "json",
          type: "GET",
          contentType: "application/json; charset=utf-8",
          dataFilter: function (data) {
            return data;
          },
          success: function (data) {
            response(
              $.map(data, function (item) {
                return {
                  label: item,
                  val: item,
                };
                //return { value: item.Sector }
              })
            );
          },
          error: function (XMLHttpRequest, textStatus, errorThrown) {
            alert(errorThrown);
          },
        });
      },
      minLength: 3, // MINIMUM 1 CHARACTER TO START WITH.
      select: function (event, ui) {
        $(this).val(ui.item.label);
        var txtdata = $(this).attr("id").split("_");
        if (txtdata[0] == "Morigin") {
          $("#Mdestination_" + txtdata[1]).focus();
        } else if (txtdata[0] == "Mdestination") {
          $("#Mdepartdate_" + txtdata[1]).focus();
        }
      },
    });
  });
  //================== Multicity Sector End==================================//

  $("[id*=select_all]").bind("click", function () {
    if ($(this).prop("checked") == true) {
      $("[id*=modifyengine_k_Chkbocpreferedairline] input").each(function () {
        $(this).attr("checked", "checked");
        $("[id*=modifyengine_k_Chkbocpreferedairline] input").prop(
          "checked",
          true
        );
      });
    } else if ($(this).prop("checked") == false) {
      $("[id*=modifyengine_k_Chkbocpreferedairline] input").removeAttr(
        "checked"
      );
    }
    //if ($(this).is(":checked")) {
    //
    //    $("[id*=modifyengine_k_Chkbocpreferedairline] input").each(function () {
    //
    //        $(this).attr("checked", "checked");
    //    });
    //} else {
    //
    //    $("[id*=modifyengine_k_Chkbocpreferedairline] input").removeAttr("checked");
    //}
  });
  if ($("#modmenudv").length) {
    //    console.log($("#modifyfaretype").val());
    //     console.log($("#modmenudv").children("div"))
    for (let i = 0; i < $("#modmenudv").children("div").length; i++) {
      if ($("#modmenudv").children("div").eq(i).attr("id")) {
        if (
          $("#modmenudv").children("div").eq(i).children("input").val() ==
          $("#modifyfaretype").val()
        ) {
          $("#modmenudv")
            .children("div")
            .eq(i)
            .children("input")
            .attr("checked", "checked");
        }
      }
    }
  }
  //   console.log($('input[name=radioName]:checked', '#modmenudv').val());
});

function ModifyOneWay() {
  document.getElementById("Trip1").checked = true;
  document.getElementById("Trip2").checked = false;
  document.getElementById("Trip3").checked = false;

  $("#modifyfaretype").val("O");
  $("#OnwayModDiv").addClass("gbcolor");
  $("#RTModDiv").addClass("whcolor");
  $("#DiscRTModDiv").addClass("whcolor");
  $("#DiscRTModDiv").removeClass("gbcolor");
  $("#RTModDiv").removeClass("gbcolor");
  document.getElementById("oneway").style.display = "none";

  try {
    document.getElementById("Trip4").checked = false;
    document.getElementById("fltOWRW").style.display = "block";
    $("#multiRTModDiv").addClass("whcolor");
    $("#multiRTModDiv").removeClass("gbcolor");
    document.getElementById("multicitybtn").style.display = "none";
    document.getElementById("multicitydiv").style.display = "none";
    document.getElementById("Fltnotmulticitybtn").style.display = "block";
  } catch (e) {}
}
window.ModifyOneWay = ModifyOneWay;
function ModifyRoundWay() {
  document.getElementById("Trip1").checked = false;
  document.getElementById("Trip2").checked = true;
  document.getElementById("Trip3").checked = false;

  $("#modifyfaretype").val("R");
  $("#OnwayModDiv").removeClass("gbcolor");
  $("#OnwayModDiv").addClass("whcolor");
  $("#RTModDiv").addClass("gbcolor");
  $("#DiscRTModDiv").addClass("whcolor");
  $("#DiscRTModDiv").removeClass("gbcolor");
  document.getElementById("oneway").style.display = "block";

  try {
    document.getElementById("Trip4").checked = false;
    $("#multiRTModDiv").addClass("whcolor");
    $("#multiRTModDiv").removeClass("gbcolor");
    document.getElementById("fltOWRW").style.display = "block";
    document.getElementById("multicitybtn").style.display = "none";
    document.getElementById("multicitydiv").style.display = "none";
    document.getElementById("Fltnotmulticitybtn").style.display = "block";
  } catch (e) {}
  document.getElementById("myModalopi").style.display = "block";
}
window.ModifyRoundWay = ModifyRoundWay;
function ModifyDiskRT() {
  document.getElementById("Trip1").checked = false;
  document.getElementById("Trip2").checked = false;
  document.getElementById("Trip3").checked = true;
  $("#modifyfaretype").val("DRT");
  $("#OnwayModDiv").addClass("whcolor");
  $("#OnwayModDiv").removeClass("gbcolor");
  $("#RTModDiv").addClass("whcolor");
  $("#RTModDiv").removeClass("gbcolor");
  $("#DiscRTModDiv").addClass("gbcolor");
  $("#DiscRTModDiv").removeClass("whcolor");
  document.getElementById("oneway").style.display = "block";
  try {
    document.getElementById("Trip4").checked = false;
    $("#multiRTModDiv").addClass("whcolor");
    $("#multiRTModDiv").removeClass("gbcolor");
    document.getElementById("fltOWRW").style.display = "block";
    document.getElementById("multicitybtn").style.display = "none";
    document.getElementById("multicitydiv").style.display = "none";
    document.getElementById("Fltnotmulticitybtn").style.display = "block";
  } catch (e) {}
}
window.ModifyDiskRT = ModifyDiskRT;
//==============================Multicity start====================================================//
function ModifyMulticityshow() {
  $("#modifyfaretype").val("MRT");
  document.getElementById("fltOWRW").style.display = "none";
  document.getElementById("Trip4").checked = true;
  document.getElementById("Trip3").checked = false;
  document.getElementById("Trip1").checked = false;
  document.getElementById("Trip2").checked = false;
  $("#OnwayModDiv").addClass("whcolor");
  $("#OnwayModDiv").removeClass("gbcolor");
  $("#RTModDiv").addClass("whcolor");
  $("#RTModDiv").removeClass("gbcolor");
  $("#DiscRTModDiv").addClass("whcolor");
  $("#DiscRTModDiv").removeClass("gbcolor");
  $("#multiRTModDiv").addClass("gbcolor");
  $("#multiRTModDiv").removeClass("whcolor");
  document.getElementById("multicitydiv").style.display = "block";
  document.getElementById("Fltnotmulticitybtn").style.display = "none";
  document.getElementById("multicitybtn").style.display = "block";
}
window.ModifyMulticityshow = ModifyMulticityshow;

//================== Multicity Add Sector start==================================//
var star = 0;
var index = 0;
function addCity(selectedName, selectedValue) {
  var ni = document.getElementById("my" + selectedName + "Div");
  if (star === 1) {
    try {
      document.getElementById("Remove" + selectedName + index).style.display =
        "none";
    } catch (e) {}

    index = index + 1;
  } else {
    index = parseInt(selectedValue) + 2;
  }
  if (index <= 5) {
    star = 1;
    var selectedIdName = selectedName + "Div" + index;
    var selecteddiv = document.createElement("li");
    selecteddiv.setAttribute("id", selectedIdName);
    var selectedhtm =
      '<div class="col-md-12 col-sm-12 col-xs-12 multicityrow offset-0">';
    selectedhtm += '<div class="col-md-4 col-sm-6 m-top-10">';
    selectedhtm += '<label for="fromsector" class="lblclssw">From</label>';
    selectedhtm += '<div class="">';
    selectedhtm +=
      '<span role="status" aria-live="polite" class="ui-helper-hidden-accessible"></span>';
    selectedhtm +=
      '<input type="text" id="Morigin_' +
      index +
      '" placeholder="Type Departure Location Here" onfocus="if (this.value =="Type Departure Location Here") {this.value=""};"   class="classic form-control multicitysector"';
    selectedhtm +=
      'title="" name="AirlineData[0].MFrom" autocomplete="off" onkeydown = "return (event.keyCode!=13);" onclick="this.setSelectionRange(0, this.value.length)">';
    selectedhtm += "</div>";
    selectedhtm += "</div>";
    selectedhtm += '<div class="col-md-4 col-sm-6 m-top-10">';
    selectedhtm += '<label for="ToSector" class="lblclssw">To</label>';
    selectedhtm += '<div class="">';
    selectedhtm +=
      '<span role="status" aria-live="polite" class="ui-helper-hidden-accessible"></span>';
    selectedhtm +=
      '<input type="text" id="Mdestination_' +
      index +
      '" placeholder="Type Arrival Location Here" onfocus="if (this.value =="Type Departure Location Here") {this.value=""};"  class="classic form-control multicitysector"';
    selectedhtm +=
      'title="" name="AirlineData[0].MTo" autocomplete="off" onkeydown="return (event.keyCode!=13);" onclick="this.setSelectionRange(0, this.value.length)" >';
    selectedhtm += "</div>";
    selectedhtm += "</div>";
    selectedhtm += '<div class="col-md-3 col-sm-6 m-top-10 ">';
    selectedhtm += '<label class="lblclssw"> Departs on</label>';
    selectedhtm +=
      '<input type="text" id="Mdepartdate_' +
      index +
      '"  placeholder="dd/mm/yyyy" readonly="false" name="AirlineData[0].MDeparture" class="classic form-control cal-1 Mdepart" style="cursor:pointer"> <div class="calendar-icon multicicond" style="background: url(/assets/img/home-sprite.png)"> </div>';
    selectedhtm += "</div>";
    selectedhtm += '<div class="col-md-1 col-sm-6 m-top-10 wertyhb">';
    selectedhtm +=
      '<a id="Remove' +
      selectedName +
      index +
      '" title="Remove ' +
      selectedName +
      '" href="javascript:void(0);" onclick=removeCity("' +
      selectedName +
      '","' +
      index +
      '")>' +
      '<img class="multi_clsBtn" src="/assets/img/outcross.png"></a><div class="clr"></div>';
    selectedhtm += "</div>";
    selectedhtm += "</div>";
    selecteddiv.innerHTML = selectedhtm;
    ni.appendChild(selecteddiv);
  } else {
    alert("you can select only five sector");
    index = index - 1;
    document.getElementById("Remove" + selectedName + index).style.display =
      "block";
  }
}
window.addCity = addCity;

//================== Multicity Add Sector end==================================//

//================== Multicity remove Sector start==================================//

function removeCity(selectedName, selectedValue) {
  var masterdiv = document.getElementById("my" + selectedName + "Div");
  var downdivnum = selectedName + "Div" + selectedValue;
  var downdiv = document.getElementById(downdivnum);
  if (downdiv) {
    masterdiv.removeChild(downdiv);
    index = index - 1;
    document.getElementById("Remove" + selectedName + index).style.display =
      "block";
  }
}
window.removeCity = removeCity;

//================== Multicity remove Sector end==================================//

function getSearchType() {
  var _type;
  var rates = document.getElementsByName("r1");
  var rate_value;
  for (var i = 0; i < rates.length; i++) {
    if (rates[i].checked) {
      _type = rates[i].value;
    }
  }
  return _type;
}
function getModifyMulticityresult() {
  var hostName = $("#hdnhostName").val();
  var rseultvalidation = validatemulticity();
  if (rseultvalidation === true) {
    $("#CenterwaitingDivMulticity").css("display", "block");
    $("#paxtyMcity").text($("#infant").val());
    $("#paxtyMcity1").text($("#adult").val());
    $("#paxtyMcity2").text($("#child").val());

    var multirow = $(".multicityrow");
    var data = "&lt;Root&gt;";
    var muldetail = "";
    for (var i = 0; i < multirow.length; i++) {
      var row = i + 1;
      data += "&lt;SectorDetail&gt;";
      data +=
        "&lt;Source&gt;" +
        document.getElementById("Morigin_" + row).value +
        "&lt;/Source&gt;";
      data +=
        "&lt;Destinstion&gt;" +
        document.getElementById("Mdestination_" + row).value +
        "&lt;/Destinstion&gt;";
      data +=
        "&lt;DepartDate&gt;" +
        document.getElementById("Mdepartdate_" + row).value +
        "&lt;/DepartDate&gt;";
      data += "&lt;/SectorDetail&gt;";
      muldetail +=
        "<div class='col-md-12 mysearchboxmain'> <div class='col-md-4 col-xs-4 searboxdt'>" +
        document
          .getElementById("Morigin_" + row)
          .value.split(")")[0]
          .split("(")[1] +
        "</div><div class='col-md-4 col-xs-4 searboxdt1' >" +
        document.getElementById("Mdepartdate_" + row).value +
        "</div> <div class='col-md-4 col-xs-4 searboxdt2'>" +
        document
          .getElementById("Mdestination_" + row)
          .value.split(")")[0]
          .split("(")[1] +
        "</div> </div>";
    }
    data += "&lt;/Root&gt;";

    var AirLines = "";
    $("#multicitydetail").html(muldetail);
    var PreferedList = [];

    $("[id*=Chkbocpreferedairline] input:checked").each(function () {
      PreferedList.push($(this).val());
    });
    AirLines = PreferedList.toString();
    var spclfare = 0;
    //if (document.getElementById('Specialfa').checked == true) {
    //    spclfare = 1;
    //}
    var Currency = "";
    // var chkdropdoen = $("#modifyengine_k_ddlcurrency").is(":visible");
    var chkdropdoen = $("#divcurreny").is(":visible");
    if (chkdropdoen) {
      Currency = $("#modifyengine_k_ddlcurrency option:selected").val();
    } else {
      Currency = "INR";
    }
    var travelClass = $("#ddlTravelClass").val();
    var Accid = $("#hdnAccountNo").val();
    var mySelection = getSearchType();
    if (mySelection === "MRT") {
    }
    //new code start
    var muldetail = "";
    var reqList = [];
    var data = "";
    data += "&lt;Root&gt;";
    for (var i = 0; i < multirow.length; i++) {
      var row = i + 1;

      data += "&lt;SectorDetail&gt;";
      data +=
        "&lt;Source&gt;" +
        document.getElementById("Morigin_" + row).value +
        "&lt;/Source&gt;";
      data +=
        "&lt;Destinstion&gt;" +
        document.getElementById("Mdestination_" + row).value +
        "&lt;/Destinstion&gt;";
      data +=
        "&lt;DepartDate&gt;" +
        document.getElementById("Mdepartdate_" + row).value +
        "&lt;/DepartDate&gt;";
      data += "&lt;/SectorDetail&gt;";
    }
    data += "&lt;/Root&gt;";

    for (var i = 0; i < multirow.length; i++) {
      var row = i + 1;
      /*var data = "";
        if (i == 0) {
            __Source = document.getElementById('Morigin_' + row).value;
            __DepartDate = document.getElementById('Mdepartdate_' + row).value;
        }
        data += "&lt;SectorDetail&gt;";
        data += "&lt;Source&gt;" + __Source + "&lt;/Source&gt;";
        data += "&lt;Destinstion&gt;" + document.getElementById('Mdestination_' + row).value + "&lt;/Destinstion&gt;";
        data += "&lt;DepartDate&gt;" + __DepartDate + "&lt;/DepartDate&gt;";
        data += "&lt;/SectorDetail&gt;";
        */

      muldetail +=
        "<div class='col-md-12 mysearchboxmain'> <div class='col-md-4 col-xs-4 searboxdt'>" +
        document
          .getElementById("Morigin_" + row)
          .value.split(")")[0]
          .split("(")[1] +
        "</div><div class='col-md-4 col-xs-4 searboxdt1' >" +
        document.getElementById("Mdepartdate_" + row).value +
        "</div> <div class='col-md-4 col-xs-4 searboxdt2'>" +
        document
          .getElementById("Mdestination_" + row)
          .value.split(")")[0]
          .split("(")[1] +
        "</div> </div>";

      reqList.push({
        _Multisector: data,
        _DepartureStation: document.getElementById("Morigin_" + row).value,
        _ArrivalStation: document.getElementById("Mdestination_" + row).value,
        _BeginDate: document.getElementById("Mdepartdate_" + row).value,
        _EndDate: "",
        _NoOfAdult: $("#adult").val(),
        _NoOfChild: $("#child").val(),
        _NoOfInfant: $("#infant").val(),
        _SearchType: "O",
        _PreferedAirlines: AirLines,
        _TravelClass: travelClass,
        _SpecialFare: spclfare,
        _Place: "I",
        _SrNo: row,
      });
    }
    // new code ends

    var params = "";
    params = reqList;
    // console.log(params);
    // params = [
    //   {
    //     _ArrivalStation: "Ahmedabad , India - Ahmedabad (AMD)",
    //     _BeginDate: "31/01/2025",
    //     _DepartureStation: "Delhi , India - Indira Gandhi Airport (DEL)",
    //     _EndDate: "",
    //     _Multisector:
    //       "&lt;Root&gt;&lt;SectorDetail&gt;&lt;Source&gt;Delhi , India - Indira Gandhi Airport (DEL)&lt;/Source&gt;&lt;Destinstion&gt;Ahmedabad , India - Ahmedabad (AMD)&lt;/Destinstion&gt;&lt;DepartDate&gt;31/01/2025&lt;/DepartDate&gt;&lt;/SectorDetail&gt;&lt;SectorDetail&gt;&lt;Source&gt;Ahmedabad , India - Ahmedabad (AMD)&lt;/Source&gt;&lt;Destinstion&gt;Mumbai , India - Chhatrapati Shivaji Airport (BOM)&lt;/Destinstion&gt;&lt;DepartDate&gt;28/02/2025&lt;/DepartDate&gt;&lt;/SectorDetail&gt;&lt;/Root&gt;",
    //     _NoOfChild: "0",
    //     _NoOfInfant: "0",
    //     _NoOfadult: "1",
    //     _Place: "I",
    //     _PreferedAirlines: "GDS,6E,SG,G8,AK,IX,2T,TR,ZO,W5,FZ,G9,TZ",
    //     _SearchType: "O",
    //     _SpecialFare: 0,
    //     _SrNo: 1,
    //     _TravelClass: "Y",
    //   },
    //   {
    //     _ArrivalStation: "Mumbai , India - Chhatrapati Shivaji Airport (BOM)",
    //     _BeginDate: "28/02/2025",
    //     _DepartureStation: "Ahmedabad , India - Ahmedabad (AMD)",
    //     _EndDate: "",
    //     _Multisector:
    //       "&lt;Root&gt;&lt;SectorDetail&gt;&lt;Source&gt;Delhi , India - Indira Gandhi Airport (DEL)&lt;/Source&gt;&lt;Destinstion&gt;Ahmedabad , India - Ahmedabad (AMD)&lt;/Destinstion&gt;&lt;DepartDate&gt;31/01/2025&lt;/DepartDate&gt;&lt;/SectorDetail&gt;&lt;SectorDetail&gt;&lt;Source&gt;Ahmedabad , India - Ahmedabad (AMD)&lt;/Source&gt;&lt;Destinstion&gt;Mumbai , India - Chhatrapati Shivaji Airport (BOM)&lt;/Destinstion&gt;&lt;DepartDate&gt;28/02/2025&lt;/DepartDate&gt;&lt;/SectorDetail&gt;&lt;/Root&gt;",
    //     _NoOfChild: "0",
    //     _NoOfInfant: "0",
    //     _NoOfadult: "1",
    //     _Place: "I",
    //     _PreferedAirlines: "GDS,6E,SG,G8,AK,IX,2T,TR,ZO,W5,FZ,G9,TZ",
    //     _SearchType: "O",
    //     _SpecialFare: 0,
    //     _SrNo: 2,
    //     _TravelClass: "Y",
    //   },
    // ];
    console.log(params);

    $.ajax({
      type: "POST",
      url: "//" + hostName + "/Flight/GetvalueForRequestMulticity",
      data: JSON.stringify({ objclsPSQ: params }),
      contentType: "application/json; charset=utf-8", //Set Content-Type
      dataType: "json", // Set return Data Type
      cache: false,
      success: function (result) {
        var hostName = $("#hdnhostName").val();
        //alert(hostName);
        var obj = result.d;
        console.log(obj);
        console.log(mySelection);

        if (Accid != "") {
          if (obj != "") {
            if (mySelection == "MRT") {
              location.href = "/Flight/multicity";
            }
          } else {
            location.href = "//" + hostName + "/Flight/Index";
          }
        } else {
          if (obj != "") {
            if (mySelection === "MRT") {
              location.href = "/Flight/multicity";
            }
          } else {
            location.href = "//" + hostName + "/Flight/Index";
          }
        }
      },
      error: function (xhr, msg, e) {
        // Extract proper error message from response
        var errorMsg = msg || "Unknown error";
        if (xhr.responseJSON && xhr.responseJSON.error) {
          errorMsg = xhr.responseJSON.error;
        } else if (xhr.responseText) {
          try {
            var errorObj = JSON.parse(xhr.responseText);
            errorMsg = errorObj.error || errorMsg;
          } catch (parseError) {
            errorMsg = xhr.responseText || xhr.statusText || errorMsg;
          }
        }
        console.error("getresult() Error:", {
          message: errorMsg,
          status: xhr.status,
          statusText: xhr.statusText,
          response: xhr.responseJSON || xhr.responseText
        });
        alert("getresult()\nError: " + errorMsg + "\nStatus: " + (xhr.status || "N/A"));
        // location.href = "//" + hostName + "/Index.aspx";
      },
    });
  } else {
  }
}
window.getModifyMulticityresult = getModifyMulticityresult;
function validatemulticity() {
  var flag = 0;
  $("#multicitydiv")
    .find(":input")
    .each(function () {
      if (this.type === "text") {
        if ($(this).val() === "") {
          $(this).css("border", "1px solid red");
          flag = 1;
        }
      }
    });
  if (flag === 1) {
    return false;
  }
  var adlt = $("#adult").val();
  var child = $("#child").val();
  var inf = $("#infant").val();
  var totalpax = parseInt(adlt) + parseInt(child) + parseInt(inf);
  if (totalpax > 9) {
    alert(
      "Total passengers are greater than 9.Kindly change your search criteria!"
    );
    return false;
  }
  //check infant
  var x = parseInt(inf);
  var t = parseInt(adlt);
  if (x > t) {
    alert("Total adult should not be less than infant!.");
    return false;
  }
  return true;
}
window.validatemulticity = validatemulticity;
//==============================Multicity end====================================================//

function validateModify() {
  $("#depCityWaiting").text($("#oricity").val());
  $("#ArrCityWaiting").text($("#desticity").val());

  var errcount = 0;
  var fareType = $("#modifyfaretype").val();
  if ($("#oricity").val() === "" || $("#oricity").val() == undefined) {
    $("#oricity").css("border", "1px solid red");
    errcount++;
  }
  if ($("#desticity").val() === "" || $("#desticity").val() == undefined) {
    $("#desticity").css("border", "1px solid red");
    errcount++;
  }
  if ($("#oricity").val() === $("#desticity").val()) {
    alert(
      "Departure and Arrival Location can not same.Kindly change your search criteria!"
    );
    errcount++;
  }
  if (fareType === "O") {
    if (
      $("#FlightSearch_txtDepartureDate").val() === "" ||
      $("#FlightSearch_txtDepartureDate").val() == undefined
    ) {
      $("#FlightSearch_txtDepartureDate").css("border", "1px solid red");
      errcount++;
    }
    $("#departdate").text($("#FlightSearch_txtDepartureDate").val());
    // $('#returndate').text($("#FlightSearch_txtDepartureDate").val());
    $("#paxty").text($("#infant").val());
    $("#paxty1").text($("#adult").val());
    $("#paxty2").text($("#child").val());
    document.getElementById("oneshoi").style.display = "block";
    document.getElementById("rshoi").style.display = "none";
  }
  if (fareType === "R") {
    if (
      $("#FlightSearch_txtDepartureDate").val() === "" ||
      $("#FlightSearch_txtDepartureDate").val() == undefined
    ) {
      $("#FlightSearch_txtDepartureDate").css("border", "1px solid red");
      errcount++;
    }
    if (
      $("#FlightSearch_txtReturnDate").val() === "" ||
      $("#FlightSearch_txtReturnDate").val() == undefined
    ) {
      $("#FlightSearch_txtReturnDate").css("border", "1px solid red");
      errcount++;
    }
    $("#departdate").text($("#FlightSearch_txtDepartureDate").val());
    $("#returndate").text($("#FlightSearch_txtReturnDate").val());
    $("#paxty").text($("#infant").val());
    $("#paxty1").text($("#adult").val());
    $("#paxty2").text($("#child").val());
    document.getElementById("oneshoi").style.display = "none";
    document.getElementById("rshoi").style.display = "block";
  }
  if (fareType === "DRT") {
    if (
      $("#FlightSearch_txtDepartureDate").val() === "" ||
      $("#FlightSearch_txtDepartureDate").val() == undefined
    ) {
      $("#FlightSearch_txtDepartureDate").css("border", "1px solid red");
      errcount++;
    }
    if (
      $("#FlightSearch_txtReturnDate").val() === "" ||
      $("#FlightSearch_txtReturnDate").val() == undefined
    ) {
      $("#FlightSearch_txtReturnDate").css("border", "1px solid red");
      errcount++;
    }
    $("#departdate").text($("#FlightSearch_txtDepartureDate").val());
    $("#returndate").text($("#FlightSearch_txtReturnDate").val());
    $("#paxty").text($("#infant").val());
    $("#paxty1").text($("#adult").val());
    $("#paxty2").text($("#child").val());
    document.getElementById("oneshoi").style.display = "none";
    document.getElementById("rshoi").style.display = "block";
  }
  var adlt = $("#adult").val();
  var child = $("#child").val();
  var inf = $("#infant").val();
  var totalpax = parseInt(adlt) + parseInt(child) + parseInt(inf);
  if (totalpax > 9) {
    alert(
      "Total passengers are greater than 9.Kindly change your search criteria!"
    );
    errcount++;
  }
  //check infant
  var x = parseInt(inf);
  var t = parseInt(adlt);
  if (x > t) {
    alert("Total Adult should not be less than infant!.");
    errcount++;
  }
  return errcount;
}
window.validateModify = validateModify;
function changesvalues(asd) {
  if (asd.id.toUpperCase() === "DDLADLT") {
    var htmll = "";
    document.getElementById("child").innerHTML = "";
    var adultvalue = 1;
    try {
      adultvalue = 10 - parseInt(ddlAdlt.value);
    } catch (e) {}
    for (var i = 0; i < adultvalue; i++) {
      htmll += "<option value='" + i + "'>" + i + "</option>";
    }
    document.getElementById("child").innerHTML = htmll;

    var html2 = "";
    document.getElementById("infant").innerHTML = "";

    try {
      adultvalue = parseInt(ddlAdlt.value);
    } catch (e) {}
    for (var j = 0; j < adultvalue + 1; j++) {
      html2 += "<option value='" + j + "'>" + j + "</option>";
    }
    document.getElementById("infant").innerHTML = html2;
  }
}
window.changesvalues = changesvalues;

function getModifyresult() {
  var hostName = $("#hdnhostName").val();
  var rseultvalidation = validateModify();
  if (rseultvalidation === 0) {
    $("#CenterwaitingDiv").css("display", "block");
    var travelClass = $("#ddlTravelClass").val();
    var Accid = $("#hdnAccountNo").val();
    var PreferedList = [];
    var AirLines = "";
    $("[id*=Chkbocpreferedairline] input:checked").each(function () {
      PreferedList.push($(this).val());
    });
    AirLines = PreferedList.toString();
    var mySelection = $("#modifyfaretype").val();
    var Currency = "";
    //var chkdropdoen = $("#modifyengine_k_ddlcurrency").is(":visible");
    var chkdropdoen = $("#divcurreny").is(":visible");
    if (chkdropdoen) {
      Currency = $("#modifyengine_k_ddlcurrency option:selected").val();
    } else {
      Currency = "INR";
    }
    var params = "";
    var serchctydate =
      $("#oricity").val() +
      "!" +
      $("#desticity").val() +
      "#" +
      $("#FlightSearch_txtDepartureDate").val() +
      "," +
      $("#FlightSearch_txtReturnDate").val();
    params = [
      {
        _DepartureStation: $("#oricity").val(),
        _ArrivalStation: $("#desticity").val(),
        _BeginDate: $("#FlightSearch_txtDepartureDate").val(),
        _EndDate: $("#FlightSearch_txtReturnDate").val(),
        _NoOfAdult: $("#adult").val(),
        _NoOfChild: $("#child").val(),
        _NoOfInfant: $("#infant").val(),
        _SearchType: $("#modifyfaretype").val(),
        _PreferedAirlines: AirLines,
        _companyId: Accid,
        _TravelClass: travelClass,
        _Place: "M",
        _Currency: Currency,
      },
    ];
    console.log(mySelection);

    $.ajax({
      type: "POST",
      url: "//" + hostName + "/Flight/GetvalueForRequest",
      data: JSON.stringify({ objclsPSQ: params }),
      contentType: "application/json; charset=utf-8", //Set Content-Type
      dataType: "json", // Set return Data Type
      cache: false,
      success: function (result) {
        var hostName = $("#hdnhostName").val(); //window.location.host;
        var obj = result.d;
        if (Accid != "") {
          if (obj != "") {
            if (obj == "O") {
              location.href =
                "//" + hostName + "/Flight/OneWay?value=" + btoa(ACCID);
            } else if (obj == "D") {
              location.href =
                "//" + hostName + "/Flight/round?value=" + btoa(ACCID);
            } else if (obj == "I") {
              location.href =
                "//" + hostName + "/Flight/Int?value=" + btoa(ACCID);
            }
            // else if (obj == "DRT") {
            //     location.href = "//" + hostName + "/k_rt.aspx?value=" + btoa(ACCID);
            // }
          } else {
            if (mySelection == "O") {
              location.href = "//" + hostName + "/Flight/OneWay";
            } else if (mySelection == "R") {
              location.href = "//" + hostName + "/Flight/round";
            } else if (mySelection == "DRT") {
              location.href = "//" + hostName + "/Flight/Int";
            }
            // else if (mySelection == "DRT") {
            //     location.href = "//" + hostName + "/k_rt.aspx?value=" + btoa(ACCID);
            // }
          }
        } else {
          if (obj != "") {
            if (obj === "O") {
              location.href = "//" + hostName + "/Flight/OneWay";
            } else if (obj === "D") {
              location.href = "//" + hostName + "/Flight/round";
            } else if (obj == "I") {
              location.href = "//" + hostName + "/Flight/Int";
            }
            // else if (mySelection === "DRT") {
            //     location.href = "//" + hostName + "/k_rt.aspx";
            // }
          } else {
            if (mySelection == "O") {
              location.href = "//" + hostName + "/Flight/OneWay";
            } else if (mySelection == "R") {
              location.href = "//" + hostName + "/Flight/Round";
            } else if (mySelection == "DRT") {
              location.href = "//" + hostName + "/Flight/Int";
            }
            // else{
            //     location.href = "//" + hostName + "/Flight/OneWay";
            // }
          }
        }
      },
      error: function (xhr, msg, e) {
        // Extract proper error message from response
        var errorMsg = msg || "Unknown error";
        if (xhr.responseJSON && xhr.responseJSON.error) {
          errorMsg = xhr.responseJSON.error;
        } else if (xhr.responseText) {
          try {
            var errorObj = JSON.parse(xhr.responseText);
            errorMsg = errorObj.error || errorMsg;
          } catch (parseError) {
            errorMsg = xhr.responseText || xhr.statusText || errorMsg;
          }
        }
        console.error("getModifyresult() Error:", {
          message: errorMsg,
          status: xhr.status,
          statusText: xhr.statusText,
          response: xhr.responseJSON || xhr.responseText
        });
        alert("getModifyresult()\nError: " + errorMsg + "\nStatus: " + (xhr.status || "N/A"));
        // Redirect based on search type instead of staying on error
        var hostName = $("#hdnhostName").val();
        var mySelection = $("#modifyfaretype").val();
        if (mySelection == "O") {
          location.href = "//" + hostName + "/Flight/OneWay";
        } else if (mySelection == "R") {
          location.href = "//" + hostName + "/Flight/round";
        } else if (mySelection == "DRT") {
          location.href = "//" + hostName + "/Flight/Int";
        } else {
          location.href = "//" + hostName + "/Flight/OneWay";
        }
      },
    });
  }
}
window.getModifyresult = getModifyresult;

//================================k Result Page End===============================

function Fromtoswap() {
  var temp = "";
  temp = oricity.value;
  oricity.value = desticity.value;
  desticity.value = temp;
}
window.Fromtoswap = Fromtoswap;
function SetValueInModifyControl() {
  var ModifyType = $("#modifyfaretype").val();
  if (ModifyType === "R" || ModifyType === "RT") {
    $("#OnwayModDiv").addClass("whcolor");
    $("#RTModDiv").addClass("gbcolor");
    $("#DiscRTModDiv").addClass("whcolor");
    document.getElementById("oneway").style.display = "block";
  } else if (ModifyType === "O") {
    $("#OnwayModDiv").addClass("gbcolor");
    $("#RTModDiv").addClass("whcolor");
    $("#DiscRTModDiv").addClass("whcolor");
    document.getElementById("oneway").style.display = "none";
  } else if (ModifyType == "DRT") {
    $("#OnwayModDiv").addClass("whcolor");
    $("#RTModDiv").addClass("whcolor");
    $("#DiscRTModDiv").addClass("gbcolor");
    document.getElementById("oneway").style.display = "block";
  } else if (ModifyType == "MC") {
    var psqXML = document.getElementById("psqXMl").value;
    var xmlDataObj = ParseXML(psqXML);
    // console.log(destCity);
    if (
      xmlDataObj.getElementsByTagName("SectorDetail").length > 2 &&
      xmlDataObj.getElementsByTagName("SectorDetail").length < 6
    ) {
      for (
        let i = 0;
        i < xmlDataObj.getElementsByTagName("SectorDetail").length - 2;
        i++
      ) {
        addCity("City", 1);
      }
    }
    // addCity("City", 1);
    setTimeout(() => {
      for (
        let i = 0;
        i < xmlDataObj.getElementsByTagName("SectorDetail").length;
        i++
      ) {
        //    console.log(i);
        $(`#Morigin_${i + 1}`).val(
          xmlDataObj
            .getElementsByTagName("SectorDetail")
            [i].getElementsByTagName("Source")[0].firstChild.nodeValue
        );
        $(`#Mdestination_${i + 1}`).val(
          xmlDataObj
            .getElementsByTagName("SectorDetail")
            [i].getElementsByTagName("Destinstion")[0].firstChild.nodeValue
        );
        $(`#Mdepartdate_${i + 1}`).val(
          xmlDataObj
            .getElementsByTagName("SectorDetail")
            [i].getElementsByTagName("DepartDate")[0].firstChild.nodeValue
        );
      }
    }, 500);

    $("#adult").val($("#numFLAdults").val());
    $("#child").val($("#numFLChildren").val());
    $("#infant").val($("#numFLInfants").val());
  }
  if (ModifyType != "MC") {
    var psqXML = document.getElementById("psqXMl").value;
    var xmlDataObj = ParseXML(psqXML);


    var destCity =
      xmlDataObj.getElementsByTagName("DepartureStation")[0].firstChild
        .nodeValue;
    var ArrCity =
      xmlDataObj.getElementsByTagName("ArrivalStation")[0].firstChild.nodeValue;
    var depdate =
      xmlDataObj.getElementsByTagName("StartDate")[0].firstChild.nodeValue;
    var retrnDate =
      xmlDataObj.getElementsByTagName("EndDate")[0].firstChild.nodeValue;
    var adultNo = $("#numFLAdults").val();
    var chldNo = $("#numFLChildren").val();
    var InfantNo = $("#numFLInfants").val();
    var ArrDate = $("#ReturnDate").val();
    $("#oricity").val(destCity);
    $("#desticity").val(ArrCity);
    $("#FlightSearch_txtDepartureDate").val(depdate);
    $("#FlightSearch_txtReturnDate").val(retrnDate);
    $("#adult").val($("#numFLAdults").val());
    $("#child").val($("#numFLChildren").val());
    $("#infant").val($("#numFLInfants").val());
    setpreferedairline();
  }
}
window.SetValueInModifyControl = SetValueInModifyControl;

function SetValueInModifyControlModifypage() {
  var psqXML = document.getElementById("psqXMl").value;
  var xmlDataObj = ParseXMLLL(psqXML);
  var Searchtype = $("#hdnsearchtypemcity").val();
  $("#modifyfaretype").val(Searchtype);
  var ModifyType = $("#modifyfaretype").val();
  if (ModifyType == "MRT") {
    var nodes = xmlDataObj.getElementsByTagName("SectorDetail"); //Get the <node> tags
    var Totalsector = nodes.length;
    if (Totalsector > 2) {
      for (var i = 0; i < Totalsector; i++) {
        if (i >= 2) {
          addCity("City", "1");
        }
      }
    }
    for (var j = 0; j < Totalsector; j++) {
      var row = j + 1;
      document.getElementById("Morigin_" + row).value =
        xmlDataObj.getElementsByTagName("Source")[j].firstChild.nodeValue;
      document.getElementById("Mdestination_" + row).value =
        xmlDataObj.getElementsByTagName("Destinstion")[j].firstChild.nodeValue;
      document.getElementById("Mdepartdate_" + row).value =
        xmlDataObj.getElementsByTagName("DepartDate")[j].firstChild.nodeValue;
    }
    activemulticity();
    setpreferedairline();
  } else {
    activenonmulticity();
    setpreferedairline();
  }
}
window.SetValueInModifyControlModifypage = SetValueInModifyControlModifypage;
function activemulticity() {
  $("#adult").val($("#numFLAdults").val());
  $("#child").val($("#numFLChildren").val());
  $("#infant").val($("#numFLInfants").val());

  document.getElementById("fltOWRW").style.display = "none";
  document.getElementById("Trip4").checked = true;
  document.getElementById("Trip3").checked = false;
  document.getElementById("Trip1").checked = false;
  document.getElementById("Trip2").checked = false;
  $("#OnwayModDiv").addClass("whcolor");
  $("#OnwayModDiv").removeClass("gbcolor");
  $("#RTModDiv").addClass("whcolor");
  $("#RTModDiv").removeClass("gbcolor");
  $("#DiscRTModDiv").addClass("whcolor");
  $("#DiscRTModDiv").removeClass("gbcolor");
  $("#multiRTModDiv").addClass("gbcolor");
  $("#multiRTModDiv").removeClass("whcolor");
  document.getElementById("multicitydiv").style.display = "block";
  document.getElementById("Fltnotmulticitybtn").style.display = "none";
  document.getElementById("multicitybtn").style.display = "block";
}
window.activemulticity = activemulticity;
function activenonmulticity() {
  var psqXML = document.getElementById("psqXMl").value;
  var xmlDataObj = ParseXMLLL(psqXML);
  var Searchtype =
    xmlDataObj.getElementsByTagName("SearchType")[0].firstChild.nodeValue;
  $("#modifyfaretype").val(Searchtype);
  var ModifyType = $("#modifyfaretype").val();
  if (ModifyType == "R" || ModifyType == "RT") {
    document.getElementById("Trip4").checked = false;
    document.getElementById("Trip3").checked = false;
    document.getElementById("Trip1").checked = false;
    document.getElementById("Trip2").checked = true;

    $("#OnwayModDiv").addClass("whcolor");
    $("#OnwayModDiv").removeClass("gbcolor");

    $("#RTModDiv").addClass("gbcolor");
    $("#RTModDiv").removeClass("whcolor");

    $("#DiscRTModDiv").addClass("whcolor");
    $("#DiscRTModDiv").removeClass("gbcolor");

    $("#multiRTModDiv").addClass("whcolor");
    $("#multiRTModDiv").removeClass("gbcolor");

    document.getElementById("oneway").style.display = "block";
  } else if (ModifyType == "O") {
    document.getElementById("Trip4").checked = false;
    document.getElementById("Trip3").checked = false;
    document.getElementById("Trip1").checked = true;
    document.getElementById("Trip2").checked = false;

    $("#OnwayModDiv").addClass("gbcolor");
    $("#OnwayModDiv").removeClass("whcolor");

    $("#RTModDiv").addClass("whcolor");
    $("#RTModDiv").removeClass("gbcolor");

    $("#DiscRTModDiv").addClass("whcolor");
    $("#DiscRTModDiv").removeClass("gbcolor");

    $("#multiRTModDiv").addClass("whcolor");
    $("#multiRTModDiv").removeClass("gbcolor");

    document.getElementById("oneway").style.display = "none";
  } else if (ModifyType == "DRT") {
    document.getElementById("Trip4").checked = false;
    document.getElementById("Trip3").checked = true;
    document.getElementById("Trip1").checked = false;
    document.getElementById("Trip2").checked = false;

    $("#OnwayModDiv").addClass("whcolor");
    $("#OnwayModDiv").removeClass("gbcolor");

    $("#RTModDiv").addClass("whcolor");
    $("#RTModDiv").removeClass("gbcolor");

    $("#DiscRTModDiv").addClass("gbcolor");
    $("#DiscRTModDiv").removeClass("whcolor");

    $("#multiRTModDiv").addClass("whcolor");
    $("#multiRTModDiv").removeClass("gbcolor");
    document.getElementById("oneway").style.display = "block";
  }

  $("#oricity").val($("#Hiddenoricity").val());
  $("#desticity").val($("#HiddenArrCity").val());
  $("#adult").val($("#numFLAdults").val());
  $("#child").val($("#numFLChildren").val());
  $("#infant").val($("#numFLInfants").val());
  var depdate =
    xmlDataObj.getElementsByTagName("StartDate")[0].firstChild.nodeValue;
  var retrnDate =
    xmlDataObj.getElementsByTagName("EndDate")[0].firstChild.nodeValue;
  if (Searchtype != "O") {
    $("#FlightSearch_txtDepartureDate").val(depdate);
    $("#FlightSearch_txtReturnDate").val(retrnDate);
  } else {
    $("#FlightSearch_txtDepartureDate").val(depdate);
    $("#FlightSearch_txtReturnDate").val(depdate);
  }
  document.getElementById("multicitydiv").style.display = "none";
  document.getElementById("Fltnotmulticitybtn").style.display = "block";
  document.getElementById("multicitybtn").style.display = "none";
  document.getElementById("fltOWRW").style.display = "block";
}
window.activenonmulticity = activenonmulticity;

function ParseXML(val) {
  if (window.DOMParser) {
    parser = new DOMParser();
    xmlDoc = parser.parseFromString(val, "text/xml");
  } // Internet Explorer
  else {
    xmlDoc = new ActiveXObject("Microsoft.XMLDOM");
    xmlDoc.loadXML(val);
  }
  return xmlDoc;
}
window.ParseXML = ParseXML;
function setpreferedairline() {
  var PreferedList = [];
  var prefAirlineXML = document.getElementById("hdnpreferedairline").value;
  var prefairxmlDataObj = ParseXML(prefAirlineXML);
  var nodes = prefairxmlDataObj.getElementsByTagName("AirVInfo");
  for (var i = 0; i < nodes.length; i++) {
    if (prefairxmlDataObj.getElementsByTagName("AirV")[i].childNodes[0]) {
      var airlinecod =
        prefairxmlDataObj.getElementsByTagName("AirV")[i].firstChild.nodeValue;
      PreferedList.push(airlinecod);
    }
  }

  // var chkdropdoen = $("#modifyengine_k_ddlcurrency").is(":visible");
  var chkdropdoen = $("#divcurreny").is(":visible");
  if (chkdropdoen) {
    $(
      "#modifyengine_k_ddlcurrency option[value='" +
        $("#hdncurrencytype").val() +
        "']"
    ).attr("selected", "selected");
  }

  $("[id*=Chkbocpreferedairline] input").each(function () {
    if (jQuery.inArray($(this).val(), PreferedList) !== -1) {
      $(this).prop("checked", true);
    } else {
      $(this).prop("checked", false);
    }
  });
}
window.setpreferedairline = setpreferedairline;
