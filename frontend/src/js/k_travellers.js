var stops, flightdetailsdivID;
jQuery(document).ready(function () {
  var Adlt = $("#Adlt").val();
  var chld = $("#Child").val();
  var Inf = $("#Infant").val();
  FnSelectedFltDetails();

  FnPaxDetailsInput();
  bindSSRList();

  var cmpid1 = document.getElementById("cmpid").value;
  if (cmpid1.indexOf("C-") > -1 || cmpid1 == "") {
    document.getElementById("Email").readOnly = true;
  }

  $(".chktestalphanumeric").keypress(function (e) {
    var keyCode = e.keyCode || e.which;
    //Regex for Valid Characters i.e. Alphabets and Numbers.
    var regex = /^[A-Za-z0-9]+$/;
    var isValid = regex.test(String.fromCharCode(keyCode));
    if (!isValid) {
      alert("Only Alphabets and Numbers allowed.");
    }
    return isValid;
  });
});

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
function submitdata(ss, e) {
  var dres = true;
  var result = ValidatePax();
  if (result === true) {
    removerdvlidat();

    $("#TextBoxFname1").css({
      "border-color": "#d4d4d4",
      "border-weight": "0px",
      "border-style": "solid",
    });
    $("#TextBoxLName1").css({
      "border-color": "#d4d4d4",
      "border-weight": "0px",
      "border-style": "solid",
    });

    if ($("#CheckBox1").is(":checked")) {
      var res = PostBookingRequest();

      if (res === true) {
        dres = true;
      } else {
        dres = false;
      }
    } else {
      alert("Kindly accept terms and conditions.");
      dres = false;
    }
  } else {
    dres = false;
  }
  return dres;
}
window.submitdata = submitdata;
function PostBookingRequest() {
  var rest = false;
  var data = "&lt;Root&gt;";
  selectedResultXml = document.getElementById("hdnFlightSelectedXml").value;
  var xmlDataObj = ParseXML(selectedResultXml);
  var SearchType = $("#hdnsearchType").val();
  var hostName = $("#hdnhostName").val(); //window.location.host;
  var RowID = xmlDataObj.getElementsByTagName("RowID")[0].firstChild.nodeValue;
  var AdltNo = xmlDataObj.getElementsByTagName("Adt")[0].firstChild.nodeValue; //$("#Adlt").val();
  var ChildNo = xmlDataObj.getElementsByTagName("Chd")[0].firstChild.nodeValue; //$("#Child").val();
  var Infant = xmlDataObj.getElementsByTagName("Inf")[0].firstChild.nodeValue; //$("#Infant").val();
  var Sector =
    xmlDataObj.getElementsByTagName("Sector")[0].firstChild.nodeValue; //$("#Infant").val();

  var AllpaxFullName = [];
  var MobileNo = $("#txtMobileNo").val();
  var Email = $("#Email").val();
  var Adderssss = $("#Address").val();
  //----------------------------  get data from GST Section start --------------------
  var gstreg_Number = $("#txtgstregno").val();
  var gstreg_CompanyName = $("#txtgstregcompanynm").val();
  var gstreg_compnyaddress = $("#txtgstcompnyaddress").val().trim();
  var gstreg_CompanyEmail = $("#txtgstcompnyemail").val();
  var gstreg_Companycontactno = $("#txtgstcompnycontactno").val();
  //----------------------------  get data from GST Section END ----------------------

  for (var i = 1; i <= AdltNo; i++) {
    data += "&lt;PaxDetail&gt;";
    data +=
      "&lt;Title&gt;" +
      document.getElementById("DropDownList" + i).value +
      "&lt;/Title&gt;";
    data +=
      "&lt;Fname&gt;" +
      document.getElementById("TextBoxFname" + i).value +
      "&lt;/Fname&gt;";
    data +=
      "&lt;Lname&gt;" +
      document.getElementById("TextBoxLName" + i).value +
      "&lt;/Lname&gt;";
    data +=
      "&lt;Dob&gt;" +
      document.getElementById("txtdate" + i).value +
      "&lt;/Dob&gt;";

    data +=
      "&lt;FFNOAirlineOB&gt;" +
      document.getElementById("txtffnnoAirline" + i).value +
      "&lt;/FFNOAirlineOB&gt;";
    data +=
      "&lt;FFNNumberOB&gt;" +
      document.getElementById("txtFrequentFlyer" + i).value +
      "&lt;/FFNNumberOB&gt;";

    if ($("#hidenRT").val() == "RW") {
      data +=
        "&lt;FFNOAirlineIB&gt;" +
        document.getElementById("txtffnnoAirlineIB" + i).value +
        "&lt;/FFNOAirlineIB&gt;";
      data +=
        "&lt;FFNNumberIB&gt;" +
        document.getElementById("txtFrequentFlyerIB" + i).value +
        "&lt;/FFNNumberIB&gt;";
    }

    if (Sector != "D") {
      data +=
        "&lt;Nationality&gt;" +
        document.getElementById("txtnationality" + i).value +
        "&lt;/Nationality&gt;";
      data +=
        "&lt;PPNo&gt;" +
        document.getElementById("PassPortno" + i).value +
        "&lt;/PPNo&gt;";
      data +=
        "&lt;PPExpiery&gt;" +
        document.getElementById("txtExpirydate" + i).value +
        "&lt;/PPExpiery&gt;";
    } else {
      data += "&lt;Nationality&gt;&lt;/Nationality&gt;";
      data += "&lt;PPNo&gt;&lt;/PPNo&gt;";
      data += "&lt;PPExpiery&gt;&lt;/PPExpiery&gt;";
    }

    AllpaxFullName.push(
      $("#TextBoxFname" + i)
        .val()
        .toUpperCase() +
        $("#TextBoxLName" + i)
          .val()
          .toUpperCase()
    );
    var gettextMeal = $("#SSRListDDL" + i + " :selected").text();
    if (gettextMeal.indexOf("&") != -1) {
      gettextMeal = gettextMeal.replace("&", "and");
    }
    data += "&lt;SSROB_M&gt;" + gettextMeal + "&lt;/SSROB_M&gt;";
    data +=
      "&lt;SSROBValue_M&gt;" +
      $("#SSRListDDL" + i + " :selected").val() +
      "&lt;/SSROBValue_M&gt;";
    var arrMeal = gettextMeal.split("--");
    var gettextBagg = $("#SSRbaggageDDL" + i + " :selected").text();
    if (gettextBagg.indexOf("&") != -1) {
      gettextBagg = gettextBagg.replace("&", "and");
    }
    data += "&lt;SSROB_B&gt;" + gettextBagg + "&lt;/SSROB_B&gt;";
    data +=
      "&lt;SSROBValue_B&gt;" +
      $("#SSRbaggageDDL" + i + " :selected").val() +
      "&lt;/SSROBValue_B&gt;";
    var arrBagg = gettextBagg.split("--");
    if (SearchType == "R") {
      var gettextMeal_I = $("#SSRListDDL_I" + i + " :selected").text();
      if (gettextMeal_I.indexOf("&") != -1) {
        gettextMeal_I = gettextMeal_I.replace("&", "and");
      }
      data += "&lt;SSRIB_M&gt;" + gettextMeal_I + "&lt;/SSRIB_M&gt;";

      data +=
        "&lt;SSRIBValue_M&gt;" +
        $("#SSRListDDL_I" + i + " :selected").val() +
        "&lt;/SSRIBValue_M&gt;";

      var arr = gettextMeal_I.split("--");
      var gettextBagg_I = $("#SSRbaggageDDL_I" + i + " :selected").text();
      if (gettextBagg_I.indexOf("&") != -1) {
        gettextBagg_I = gettextBagg_I.replace("&", "and");
      }
      data += "&lt;SSRIB_B&gt;" + gettextBagg_I + "&lt;/SSRIB_B&gt;";

      data +=
        "&lt;SSRIBValue_B&gt;" +
        $("#SSRbaggageDDL_I" + i + " :selected").val() +
        "&lt;/SSRIBValue_B&gt;";

      var arrBagg_I = gettextBagg_I.split("--");
    }
    data += "&lt;PaxType&gt;ADT&lt;/PaxType&gt;";
    data += "&lt;MobileNo&gt;" + MobileNo + "&lt;/MobileNo&gt;";
    data += "&lt;Email&gt;" + Email + "&lt;/Email&gt;";
    data += "&lt;Adderssss&gt;" + Adderssss + "&lt;/Adderssss&gt;";
    data += "&lt;gstreg_Number&gt;" + gstreg_Number + "&lt;/gstreg_Number&gt;";
    data +=
      "&lt;gstreg_CompanyName&gt;" +
      gstreg_CompanyName +
      "&lt;/gstreg_CompanyName&gt;";
    data +=
      "&lt;gstreg_compnyaddress&gt;" +
      gstreg_compnyaddress +
      "&lt;/gstreg_compnyaddress&gt;";
    data +=
      "&lt;gstreg_CompanyEmail&gt;" +
      gstreg_CompanyEmail +
      "&lt;/gstreg_CompanyEmail&gt;";
    data +=
      "&lt;gstreg_Companycontactno&gt;" +
      gstreg_Companycontactno +
      "&lt;/gstreg_Companycontactno&gt;";
    data += "&lt;/PaxDetail&gt;";
  }

  if (ChildNo != 0) {
    for (var i = 1; i <= ChildNo; i++) {
      data += "&lt;PaxDetail&gt;";
      var count = parseInt(AdltNo) + i;
      data +=
        "&lt;Title&gt;" +
        document.getElementById("DropDownList" + count).value +
        "&lt;/Title&gt;";
      data +=
        "&lt;Fname&gt;" +
        document.getElementById("TextBoxFname" + count).value +
        "&lt;/Fname&gt;";
      data +=
        "&lt;Lname&gt;" +
        document.getElementById("TextBoxLName" + count).value +
        "&lt;/Lname&gt;";
      data +=
        "&lt;Dob&gt;" +
        document.getElementById("txtdate" + count).value +
        "&lt;/Dob&gt;";

      data += "&lt;FFNOAirlineOB&gt;&lt;/FFNOAirlineOB&gt;";
      data += "&lt;FFNNumberOB&gt;&lt;/FFNNumberOB&gt;";

      if ($("#hidenRT").val() == "RW") {
        data += "&lt;FFNOAirlineIB&gt;&lt;/FFNOAirlineIB&gt;";
        data += "&lt;FFNNumberIB&gt;&lt;/FFNNumberIB&gt;";
      }

      if (Sector != "D") {
        data +=
          "&lt;Nationality&gt;" +
          document.getElementById("txtnationality" + count).value +
          "&lt;/Nationality&gt;";
        data +=
          "&lt;PPNo&gt;" +
          document.getElementById("PassPortno" + count).value +
          "&lt;/PPNo&gt;";
        data +=
          "&lt;PPExpiery&gt;" +
          document.getElementById("txtExpirydate" + count).value +
          "&lt;/PPExpiery&gt;";
      } else {
        data += "&lt;Nationality&gt;&lt;/Nationality&gt;";
        data += "&lt;PPNo&gt;&lt;/PPNo&gt;";
        data += "&lt;PPExpiery&gt;&lt;/PPExpiery&gt;";
      }

      AllpaxFullName.push(
        $("#TextBoxFname" + count)
          .val()
          .toUpperCase() +
          $("#TextBoxLName" + count)
            .val()
            .toUpperCase()
      );
      var gettextMeal = $("#SSRListDDL" + count + " :selected").text();
      if (gettextMeal.indexOf("&") != -1) {
        gettextMeal = gettextMeal.replace("&", "and");
      }
      data += "&lt;SSROB_M&gt;" + gettextMeal + "&lt;/SSROB_M&gt;";

      data +=
        "&lt;SSROBValue_M&gt;" +
        $("#SSRListDDL" + count + " :selected").val() +
        "&lt;/SSROBValue_M&gt;";
      var arrMeal = gettextMeal.split("--");
      var gettextBagg = $("#SSRbaggageDDL" + count + " :selected").text();
      if (gettextBagg.indexOf("&") != -1) {
        gettextBagg = gettextBagg.replace("&", "and");
      }
      data += "&lt;SSROB_B&gt;" + gettextBagg + "&lt;/SSROB_B&gt;";
      data +=
        "&lt;SSROBValue_B&gt;" +
        $("#SSRbaggageDDL" + count + " :selected").val() +
        "&lt;/SSROBValue_B&gt;";
      var arrBagg = gettextBagg.split("--");
      if (SearchType == "R") {
        var gettextMeal_I = $("#SSRListDDL_I" + count + " :selected").text();
        if (gettextMeal_I.indexOf("&") != -1) {
          gettextMeal_I = gettextMeal_I.replace("&", "and");
        }
        data += "&lt;SSRIB_M&gt;" + gettextMeal_I + "&lt;/SSRIB_M&gt;";

        data +=
          "&lt;SSRIBValue_M&gt;" +
          $("#SSRListDDL_I" + count + " :selected").val() +
          "&lt;/SSRIBValue_M&gt;";
        var arr = gettextMeal_I.split("--");
        var gettextBagg_I = $("#SSRbaggageDDL_I" + count + " :selected").text();
        if (gettextBagg_I.indexOf("&") != -1) {
          gettextBagg_I = gettextBagg_I.replace("&", "and");
        }
        data += "&lt;SSRIB_B&gt;" + gettextBagg_I + "&lt;/SSRIB_B&gt;";
        data +=
          "&lt;SSRIBValue_B&gt;" +
          $("#SSRbaggageDDL_I" + count + " :selected").val() +
          "&lt;/SSRIBValue_B&gt;";
        var arrBagg_I = gettextBagg_I.split("--");
      }

      data += "&lt;PaxType&gt;CHD&lt;/PaxType&gt;";
      data += "&lt;MobileNo&gt;" + MobileNo + "&lt;/MobileNo&gt;";
      data += "&lt;Email&gt;" + Email + "&lt;/Email&gt;";
      data += "&lt;Adderssss&gt;" + Adderssss + "&lt;/Adderssss&gt;";
      data +=
        "&lt;gstreg_Number&gt;" + gstreg_Number + "&lt;/gstreg_Number&gt;";
      data +=
        "&lt;gstreg_CompanyName&gt;" +
        gstreg_CompanyName +
        "&lt;/gstreg_CompanyName&gt;";
      data +=
        "&lt;gstreg_compnyaddress&gt;" +
        gstreg_compnyaddress +
        "&lt;/gstreg_compnyaddress&gt;";
      data +=
        "&lt;gstreg_CompanyEmail&gt;" +
        gstreg_CompanyEmail +
        "&lt;/gstreg_CompanyEmail&gt;";
      data +=
        "&lt;gstreg_Companycontactno&gt;" +
        gstreg_Companycontactno +
        "&lt;/gstreg_Companycontactno&gt;";
      data += "&lt;/PaxDetail&gt;";
    }
  }

  if (Infant != 0) {
    for (var i = 1; i <= Infant; i++) {
      data += "&lt;PaxDetail&gt;";
      var count = parseInt(AdltNo) + parseInt(ChildNo) + i;
      data +=
        "&lt;Title&gt;" +
        document.getElementById("DropDownList" + count).value +
        "&lt;/Title&gt;";
      data +=
        "&lt;Fname&gt;" +
        document.getElementById("TextBoxFname" + count).value +
        "&lt;/Fname&gt;";
      data +=
        "&lt;Lname&gt;" +
        document.getElementById("TextBoxLName" + count).value +
        "&lt;/Lname&gt;";
      data +=
        "&lt;Dob&gt;" +
        document.getElementById("txtdate" + count).value +
        "&lt;/Dob&gt;";

      data += "&lt;FFNOAirlineOB&gt;&lt;/FFNOAirlineOB&gt;";
      data += "&lt;FFNNumberOB&gt;&lt;/FFNNumberOB&gt;";

      if ($("#hidenRT").val() == "RW") {
        data += "&lt;FFNOAirlineIB&gt;&lt;/FFNOAirlineIB&gt;";
        data += "&lt;FFNNumberIB&gt;&lt;/FFNNumberIB&gt;";
      }

      if (Sector != "D") {
        data +=
          "&lt;Nationality&gt;" +
          document.getElementById("txtnationality" + count).value +
          "&lt;/Nationality&gt;";
        data +=
          "&lt;PPNo&gt;" +
          document.getElementById("PassPortno" + count).value +
          "&lt;/PPNo&gt;";
        data +=
          "&lt;PPExpiery&gt;" +
          document.getElementById("txtExpirydate" + count).value +
          "&lt;/PPExpiery&gt;";
      } else {
        data += "&lt;Nationality&gt;&lt;/Nationality&gt;";
        data += "&lt;PPNo&gt;&lt;/PPNo&gt;";
        data += "&lt;PPExpiery&gt;&lt;/PPExpiery&gt;";
      }

      data += "&lt;SSROB_M&gt;&lt;/SSROB_M&gt;";
      data += "&lt;SSROBValue_M&gt;&lt;/SSROBValue_M&gt;";
      data += "&lt;SSROB_B&gt;&lt;/SSROB_B&gt;";
      data += "&lt;SSROBValue_B&gt;&lt;/SSROBValue_B&gt;";
      if (SearchType == "R") {
        data += "&lt;SSRIB_M&gt;&lt;/SSRIB_M&gt;";
        data += "&lt;SSRIBValue_M&gt;&lt;/SSRIBValue_M&gt;";
        data += "&lt;SSRIB_B&gt;&lt;/SSRIB_B&gt;";
        data += "&lt;SSRIBValue_B&gt;&lt;/SSRIBValue_B&gt;";
      }
      AllpaxFullName.push(
        $("#TextBoxFname" + count)
          .val()
          .toUpperCase() +
          $("#TextBoxLName" + count)
            .val()
            .toUpperCase()
      );
      data += "&lt;PaxType&gt;INF&lt;/PaxType&gt;";
      data += "&lt;MobileNo&gt;" + MobileNo + "&lt;/MobileNo&gt;";
      data += "&lt;Email&gt;" + Email + "&lt;/Email&gt;";
      data += "&lt;Adderssss&gt;" + Adderssss + "&lt;/Adderssss&gt;";
      data +=
        "&lt;gstreg_Number&gt;" + gstreg_Number + "&lt;/gstreg_Number&gt;";
      data +=
        "&lt;gstreg_CompanyName&gt;" +
        gstreg_CompanyName +
        "&lt;/gstreg_CompanyName&gt;";
      data +=
        "&lt;gstreg_compnyaddress&gt;" +
        gstreg_compnyaddress +
        "&lt;/gstreg_compnyaddress&gt;";
      data +=
        "&lt;gstreg_CompanyEmail&gt;" +
        gstreg_CompanyEmail +
        "&lt;/gstreg_CompanyEmail&gt;";
      data +=
        "&lt;gstreg_Companycontactno&gt;" +
        gstreg_Companycontactno +
        "&lt;/gstreg_Companycontactno&gt;";
      data += "&lt;/PaxDetail&gt;";
    }
  }
  data += "&lt;/Root&gt;";
  var paxvalid = getduplicatevalue(AllpaxFullName);

  if (paxvalid === true) {
    reviewit1();
    document.getElementById("hidenpaxxmlresult").value = data;
    rest = true;
  }
  // document.getElementById('traveldtl').style.display = 'none';
  //  document.getElementById('makepayment').style.display = 'block';
  //document.getElementById('box').style.background = '#374579';
  //document.getElementById('box').style.color = '#fff';
  //document.getElementById('box').style.borderRight = '1px solid #fff';
  //document.getElementById('makeboxdtl').style.background = '#fff';
  //document.getElementById('makeboxdtl').style.color = '#03a9f4';
  //document.getElementById('makeboxdtl').style.borderRight = 'none';
  return rest;
}
window.PostBookingRequest = PostBookingRequest;
function reviewit1() {
  var IBMARKUP = 0;
  var IBTDS = 0;
  var cmpid = document.getElementById("cmpid").value;
  var selectedResultXml = document.getElementById("hdnFlightSelectedXml").value;
  var xmlDataObj = ParseXML(selectedResultXml);
  var TotCFee =
    xmlDataObj.getElementsByTagName("TotalCfee")[0].firstChild.nodeValue;
  var OBMARKUP = parseInt(
    xmlDataObj.getElementsByTagName("TotalMarkup")[0].firstChild.nodeValue
  );
  var OBTDS = parseInt(
    xmlDataObj.getElementsByTagName("TotalTds")[0].firstChild.nodeValue
  );
  if (cmpid.indexOf("C-") > -1 || cmpid == "" || cmpid.indexOf("-SA-") > -1) {
    OBTDS = parseInt(
      xmlDataObj.getElementsByTagName("TotalTds_SA")[0].firstChild.nodeValue
    );
  }
  var getvalue = document.getElementById("hdnFlightSelectedXmlInbond").value;
  if (getvalue != "" || getvalue == undefined) {
    var xmlDataObjIN = ParseXML(getvalue);
    IBMARKUP = parseInt(
      xmlDataObjIN.getElementsByTagName("TotalMarkup")[0].firstChild.nodeValue
    );
    IBTDS = parseInt(
      xmlDataObjIN.getElementsByTagName("TotalTds")[0].firstChild.nodeValue
    );
    if (cmpid.indexOf("C-") > -1 || cmpid == "" || cmpid.indexOf("-SA-") > -1) {
      IBTDS = parseInt(
        xmlDataObjIN.getElementsByTagName("TotalTds_SA")[0].firstChild.nodeValue
      );
    }
  }
  $("#TToalmarkup").val(OBMARKUP + IBMARKUP);
  $("#TotTDS").val(OBTDS + IBTDS);
  var reviewFare = $("#veryfyAmount").html();
  $("#HiddenTotalfare").val(reviewFare);
  $("#hdnTotCfee").val(TotCFee);
}

function CalTotalFareAmt() {
  var _TotalFare = 0;
  var _TotalTds = 0;
  var _TotalCommission = 0;
  var _TotalCommission_SA = 0;

  var xmlDoc = document.getElementById("hdnFlightSelectedXml").value;
  var xmlDataObj = ParseXML(xmlDoc);

  var TripCode_ = "";

  for (
    i = 0;
    i < xmlDataObj.getElementsByTagName("AvailabilityResponseOut").length;
    i++
  ) {
    if (
      TripCode_ === "" ||
      TripCode_ !=
        xmlDataObj
          .getElementsByTagName("Origin")
          [i].firstChild.nodeValue.trim() +
          xmlDataObj
            .getElementsByTagName("Destination")
            [i].firstChild.nodeValue.trim()
    ) {
      TripCode_ =
        xmlDataObj
          .getElementsByTagName("Origin")
          [i].firstChild.nodeValue.trim() +
        xmlDataObj
          .getElementsByTagName("Destination")
          [i].firstChild.nodeValue.trim();

      _TotalFare += parseInt(
        xmlDataObj.getElementsByTagName("TotalFare")[i].firstChild.nodeValue
      );
      _TotalTds += parseInt(
        xmlDataObj.getElementsByTagName("TotalTds")[i].firstChild.nodeValue
      );
      _TotalCommission += parseInt(
        xmlDataObj.getElementsByTagName("TotalCommission")[i].firstChild
          .nodeValue
      );
      _TotalCommission_SA += parseInt(
        xmlDataObj.getElementsByTagName("TotalCommission_SA")[i].firstChild
          .nodeValue
      );
    }
  }

  return {
    TotalFare: _TotalFare,
    TotalTds: _TotalTds,
    TotalCommission: _TotalCommission,
    TotalCommission_SA: _TotalCommission_SA,
  };
}

function FnSelectedFltDetails() {
  //
  //$("#flightsh1").html('');

  /*  commented on 1Aug2023 by cpk
    selectedResultXml = document.getElementById("hdnFlightSelectedXml").value;
    var xmlDataObj = ParseXML(selectedResultXml);
    ////var _arrSelectedFlt = [];
    //var _arrSelectedFltOut = [];
    $('#veryfyAmount').text(parseInt(xmlDataObj.getElementsByTagName('TotalFare')[0].firstChild.nodeValue) - (parseInt(xmlDataObj.getElementsByTagName('TotalTds')[0].firstChild.nodeValue)));
    $('#discountAmount').text(xmlDataObj.getElementsByTagName('TotalCommission')[0].firstChild.nodeValue);


    var cmpid = document.getElementById("cmpid").value;
    if (cmpid.indexOf("C-") > -1 || cmpid == "" || cmpid.indexOf("-SA-") > -1) {
        $('#discountAmount').text(xmlDataObj.getElementsByTagName('TotalCommission_SA')[0].firstChild.nodeValue);
    }

    if (cmpid.indexOf("C-") > -1 || cmpid == "") {
        $('#veryfyAmount').text('');
        $('#veryfyAmount').text(parseInt(xmlDataObj.getElementsByTagName('TotalFare')[0].firstChild.nodeValue) - (parseInt(xmlDataObj.getElementsByTagName('TotalCommission_SA')[0].firstChild.nodeValue) + parseInt(xmlDataObj.getElementsByTagName('TotalTds')[0].firstChild.nodeValue)));
    }
    $('#HiddenDiscount').val($('#discountAmount').html());
    */
  var _TotResult = CalTotalFareAmt();

  $("#veryfyAmount").text(
    parseInt(_TotResult.TotalFare) - parseInt(_TotResult.TotalTds)
  );
  $("#discountAmount").text(_TotResult.TotalCommission);

  var cmpid = document.getElementById("cmpid").value;
  if (cmpid.indexOf("C-") > -1 || cmpid == "" || cmpid.indexOf("-SA-") > -1) {
    $("#discountAmount").text(_TotResult.TotalCommission_SA);
  }

  if (cmpid.indexOf("C-") > -1 || cmpid == "") {
    $("#veryfyAmount").text("");
    $("#veryfyAmount").text(
      parseInt(_TotResult.TotalFare) -
        (parseInt(_TotResult.TotalCommission_SA) +
          parseInt(_TotResult.TotalTds))
    );
  }
  $("#HiddenDiscount").val($("#discountAmount").html());

  //var stops = xmlDataObj.getElementsByTagName('Stops')[0].firstChild.nodeValue;
  //_arrSelectedFlt.push('<div class="col-md-12 col-sm-12 col-xs-12 offset-0"><div class="col-md-12 col-sm-12 col-xs-12 offset-0">');
  //_arrSelectedFlt.push('<div class="col-md-12 col-sm-12 col-xs-12 m-top-10 offset-0 loctxt">');
  //_arrSelectedFlt.push('<i class="fa fa-map-marker" aria-hidden="true"></i> &nbsp; <b id="txtFrom">' + xmlDataObj.getElementsByTagName('DepartureStationName')[0].firstChild.nodeValue + '</b> To <b id="txtTo">' + xmlDataObj.getElementsByTagName('ArrivalStationName')[stops].firstChild.nodeValue + '</b></div>');

  //for (var i = 0; i <= stops; i++) {
  //    var AirLineName = xmlDataObj.getElementsByTagName('CarrierName')[i].firstChild.nodeValue;
  //    var imgName = "//style.zealtravels.in/Airline/airlogo_square/" + xmlDataObj.getElementsByTagName('CarrierCode')[i].firstChild.nodeValue + ".gif";
  //    var DepartureTime = xmlDataObj.getElementsByTagName('DepartureTime')[i].firstChild.nodeValue;
  //    var ArrivalTime = xmlDataObj.getElementsByTagName('ArrivalTime')[i].firstChild.nodeValue;
  //    var ArrivalDate = xmlDataObj.getElementsByTagName('ArrivalDate')[i].firstChild.nodeValue;
  //    var res1 = ArrivalDate.split(",");
  //    var finalres = res1[1].split("-20");
  //    var datetimeArr = finalres[0] + "," + ArrivalTime;

  //    var DepartureStationName = xmlDataObj.getElementsByTagName('DepartureStationName')[i].firstChild.nodeValue;
  //    var DepartureStation = xmlDataObj.getElementsByTagName('DepartureStation')[i].firstChild.nodeValue;
  //    var ArrivalStation = xmlDataObj.getElementsByTagName('ArrivalStation')[i].firstChild.nodeValue;
  //    var ArrivalStationName = xmlDataObj.getElementsByTagName('ArrivalStationName')[i].firstChild.nodeValue;
  //    var CarrierCode = xmlDataObj.getElementsByTagName('CarrierCode')[i].firstChild.nodeValue;
  //    var FlightNumber = xmlDataObj.getElementsByTagName('FlightNumber')[i].firstChild.nodeValue;
  //    var fltCode = CarrierCode + "-" + FlightNumber;
  //    var DepartureDate = xmlDataObj.getElementsByTagName('DepartureDate')[i].firstChild.nodeValue;
  //    var DurationDesc = xmlDataObj.getElementsByTagName('DurationDesc')[i].firstChild.nodeValue;
  //    var res = DepartureDate.split(",");
  //    var finaldate = res[1].split("-20");

  //    var DepartureTime = xmlDataObj.getElementsByTagName('DepartureTime')[i].firstChild.nodeValue;
  //    var dateTime = finaldate[0] + "," + DepartureTime;
  //    _arrSelectedFlt.push('<div class="col-md-12 col-sm-12 col-xs-12 m-top-10 offset-0" style=" padding-bottom:5px; ">');
  //    _arrSelectedFlt.push('<div class="col-md-2 col-sm-2 col-xs-4 offset-0"><img src=' + imgName + ' class="img-responsive" style="height:32px; width:32px;"> </div>');
  //    _arrSelectedFlt.push('<div class="col-md-4 col-sm-4 col-xs-8 airnamty"><b class="airnamec">' + AirLineName + ' </b><br>' + fltCode + '</div>');
  //    _arrSelectedFlt.push('<div class="col-md-6 col-sm-6 col-xs-12 text-right offset-0"><br><b>' + dateTime + '</b></div>');
  //    _arrSelectedFlt.push('</div>');

  //    _arrSelectedFlt.push('<div id="flightdetailsdiv' + i + '" class="col-md-12 col-sm-12 col-xs-12 m-top-10 offset-0 m-btm-10" style="display: block;">');
  //    _arrSelectedFlt.push('<div class="col-md-12 col-sm-12 col-xs-12 offset-0" style="margin-left: 21px;">');
  //    _arrSelectedFlt.push('<div class="col-md-6 col-sm-6 col-xs-12 offset-0">');
  //    _arrSelectedFlt.push('<div class="col-md-12 col-sm-12 col-xs-12 text-center"><b class="ard"><i class="fa fa-plane planedeprt" aria-hidden="true"></i> &nbsp; Departs</b></div>');
  //    _arrSelectedFlt.push('<div class="col-md-12 col-sm-12 col-xs-12 offset-0"> <span class="depatdd"> <b clsaa="boldd">' + DepartureStation + '</b></span> </div>');
  //    _arrSelectedFlt.push('<div class="col-md-12 col-sm-12 col-xs-12 offset-0"><span class="depatee">' + dateTime + '</span> </div>');
  //    _arrSelectedFlt.push('</div>');

  //    _arrSelectedFlt.push('<div class="col-md-6 col-sm-6 col-xs-12 offset-0">');
  //    _arrSelectedFlt.push('<div class="col-md-12 col-sm-12 col-xs-12  text-center"><b class="ard"><i class="fa fa-plane font-awsome-plane planearrv" aria-hidden="true"></i> &nbsp; Arrives</b></div>');
  //    _arrSelectedFlt.push('<div class="col-md-12 col-sm-12 col-xs-12 offset-0"><span class="depatdd1"> <b class="boldd">' + ArrivalStation + '</b> </span> </div>');
  //    _arrSelectedFlt.push('<div class="col-md-12 col-sm-12 col-xs-12 offset-0"><span class="depatee1">' + datetimeArr + ' </span></div>');
  //    _arrSelectedFlt.push('</div></div>');
  //    _arrSelectedFlt.push('<div class="col-md-12 col-sm-12 col-xs-12">');
  //    _arrSelectedFlt.push('<div class="col-md-11 col-sm-11 col-xs-12 m-top-10 layover-txt"><i class="fa fa-clock-o font-awsm-font" aria-hidden="true"></i> &nbsp; Duration &nbsp;&nbsp; ' + DurationDesc + '</div>');
  //    _arrSelectedFlt.push('</div>');
  //    _arrSelectedFlt.push('</div>');
  //    _arrSelectedFlt.push('</div>');
  //}
  var getvalue = document.getElementById("hdnFlightSelectedXmlInbond").value;
  if (getvalue != "") {
    _arrSelectedFltOut = FnSelectedInBondFltDetails(stops);
  }
  //    //merge inbond result into outbond
  //    var new_arrSelectedFlt = _arrSelectedFlt.concat(_arrSelectedFltOut);
  //    new_arrSelectedFlt.push('<div class="col-md-12 col-sm-12 col-xs-12 m-top-20" style="border-top:1px solid #ccc;">');
  //    new_arrSelectedFlt.push('<div id="flightshow" class="col-md-12 col-sm-12 col-xs-12 m-top-5 offset-0 text-center view-flight" onclick="showflight(' + flightdetailsdivID + ')" style="display: none;"> View Complete Flight Details</div>');
  //    new_arrSelectedFlt.push('<div id="flighthide" class="col-md-12 col-sm-12 col-xs-12 m-top-5 offset-0 text-center view-flight" onclick="hideflight(' + flightdetailsdivID + ')" style="display: block;"> Collapse Flight Details</div>');
  //    new_arrSelectedFlt.push('</div>');
  //    new_arrSelectedFlt.push('</div>');
  //    $("#flightsh1").append(new_arrSelectedFlt.join(''));
  //}
  //else {
  //    stops = xmlDataObj.getElementsByTagName('Stops')[0].firstChild.nodeValue;
  //    _arrSelectedFlt.push('<div class="col-md-12 col-sm-12 col-xs-12 m-top-20" style="border-top:1px solid #ccc;">');
  //    _arrSelectedFlt.push('<div id="flightshow" class="col-md-12 col-sm-12 col-xs-12 m-top-5 offset-0 text-center view-flight" onclick="showflight(' + stops + ')" style="display: none;"> View Complete Flight Details</div>');
  //    _arrSelectedFlt.push('<div id="flighthide" class="col-md-12 col-sm-12 col-xs-12 m-top-5 offset-0 text-center view-flight" onclick="hideflight(' + stops + ')" style="display: block;"> Collapse Flight Details</div>');
  //    _arrSelectedFlt.push('</div>');
  //    _arrSelectedFlt.push('</div>');
  //    $("#flightsh1").append(_arrSelectedFlt.join(''));
  //}
}
window.FnSelectedFltDetails = FnSelectedFltDetails;
function FnSelectedInBondFltDetails(_stop) {
  var getvalue = document.getElementById("hdnFlightSelectedXmlInbond").value;
  //var _arrSelectedFlt = [];
  if (getvalue != "") {
    selectedResultXml = document.getElementById(
      "hdnFlightSelectedXmlInbond"
    ).value;
    var xmlDataObj = ParseXML(selectedResultXml);
    var stopIn =
      xmlDataObj.getElementsByTagName("Stops")[0].firstChild.nodeValue;
    var IBprice =
      parseInt(
        xmlDataObj.getElementsByTagName("TotalFare")[0].firstChild.nodeValue
      ) -
      parseInt(
        xmlDataObj.getElementsByTagName("TotalTds")[0].firstChild.nodeValue
      );
    var OBprice = $("#veryfyAmount").html();
    var Sector =
      xmlDataObj.getElementsByTagName("Sector")[0].firstChild.nodeValue;
    var OBdiscountAmount = $("#discountAmount").html();
    var IBdiscountAmount =
      xmlDataObj.getElementsByTagName("TotalCommission")[0].firstChild
        .nodeValue;
    var cmpid = document.getElementById("cmpid").value;
    if (cmpid.indexOf("C-") > -1 || cmpid == "" || cmpid.indexOf("-SA-") > -1) {
      IBdiscountAmount =
        xmlDataObj.getElementsByTagName("TotalCommission_SA")[0].firstChild
          .nodeValue;
    }
    if (cmpid.indexOf("C-") > -1 || cmpid == "") {
      IBprice =
        parseInt(
          xmlDataObj.getElementsByTagName("TotalFare")[0].firstChild.nodeValue
        ) -
        (parseInt(
          xmlDataObj.getElementsByTagName("TotalCommission_SA")[0].firstChild
            .nodeValue
        ) +
          parseInt(
            xmlDataObj.getElementsByTagName("TotalTds")[0].firstChild.nodeValue
          ));
    }
    var totaldiscountAmount = parseInt(OBdiscountAmount);
    var totalprice = parseInt(OBprice);
    if (Sector == "D") {
      if (document.getElementById("hidenRT").value == "RT") {
        totalprice = parseInt(OBprice);
        totaldiscountAmount = parseInt(OBdiscountAmount);
      } else {
        totalprice = parseInt(OBprice) + parseInt(IBprice);
        totaldiscountAmount =
          parseInt(OBdiscountAmount) + parseInt(IBdiscountAmount);
      }
    }
    $("#veryfyAmount").html(totalprice);
    $("#discountAmount").html(totaldiscountAmount);
    $("#HiddenDiscount").val($("#discountAmount").html());
    //_arrSelectedFlt.push('<div class="col-md-12 col-sm-12 col-xs-12 offset-0"><div class="col-md-12 col-sm-12 col-xs-12 offset-0">');
    //_arrSelectedFlt.push('<div class="col-md-12 col-sm-12 col-xs-12 m-top-10 offset-0 loctxt">');
    //_arrSelectedFlt.push('<i class="fa fa-map-marker" aria-hidden="true"></i> &nbsp; <b>' + xmlDataObj.getElementsByTagName('DepartureStationName')[0].firstChild.nodeValue + '</b> To <b>' + xmlDataObj.getElementsByTagName('ArrivalStationName')[stopIn].firstChild.nodeValue + '</b></div>');
    //for (var i = 0; i <= stopIn; i++) {
    //    var id = parseInt(_stop) + 1;
    //    flightdetailsdivID = parseInt(i) + id;
    //    var AirLineName = xmlDataObj.getElementsByTagName('CarrierName')[i].firstChild.nodeValue;
    //    var imgName = "//style.zealtravels.in/Airline/airlogo_square/" + xmlDataObj.getElementsByTagName('CarrierCode')[i].firstChild.nodeValue + ".gif";
    //    var DepartureTime = xmlDataObj.getElementsByTagName('DepartureTime')[i].firstChild.nodeValue;
    //    var ArrivalTime = xmlDataObj.getElementsByTagName('ArrivalTime')[i].firstChild.nodeValue;
    //    var ArrivalDate = xmlDataObj.getElementsByTagName('ArrivalDate')[i].firstChild.nodeValue;
    //    var res1 = ArrivalDate.split(",");
    //    var finalres = res1[1].split("-20");
    //    var datetimeArr = finalres[0] + "," + ArrivalTime;
    //    var DepartureStationName = xmlDataObj.getElementsByTagName('DepartureStationName')[i].firstChild.nodeValue;
    //    var DepartureStation = xmlDataObj.getElementsByTagName('DepartureStation')[i].firstChild.nodeValue;
    //    var ArrivalStation = xmlDataObj.getElementsByTagName('ArrivalStation')[i].firstChild.nodeValue;
    //    var ArrivalStationName = xmlDataObj.getElementsByTagName('ArrivalStationName')[i].firstChild.nodeValue;
    //    var CarrierCode = xmlDataObj.getElementsByTagName('CarrierCode')[i].firstChild.nodeValue;
    //    var FlightNumber = xmlDataObj.getElementsByTagName('FlightNumber')[i].firstChild.nodeValue;
    //    var fltCode = CarrierCode + "-" + FlightNumber;
    //    var DepartureDate = xmlDataObj.getElementsByTagName('DepartureDate')[i].firstChild.nodeValue;
    //    var DurationDesc = xmlDataObj.getElementsByTagName('DurationDesc')[i].firstChild.nodeValue;
    //    var res = DepartureDate.split(",");
    //    var finaldate = res[1].split("-20");
    //    var DepartureTime = xmlDataObj.getElementsByTagName('DepartureTime')[i].firstChild.nodeValue;
    //    var dateTime = finaldate[0] + "," + DepartureTime;
    //    _arrSelectedFlt.push('<div class="col-md-12 col-sm-12 col-xs-12 m-top-10 offset-0" style=" padding-bottom:5px;">');
    //    _arrSelectedFlt.push('<div class="col-md-2 col-sm-2 col-xs-4 offset-0"><img src=' + imgName + ' class="img-responsive" style="height:32px; width:32px;"> </div>');
    //    _arrSelectedFlt.push('<div class="col-md-4 col-sm-4 col-xs-8 airnamty"><b class="airnamec">' + AirLineName + ' </b><br>' + fltCode + '</div>');
    //    _arrSelectedFlt.push('<div class="col-md-6 col-sm-6 col-xs-12 text-right offset-0"><br><b>' + dateTime + '</b></div>');
    //    _arrSelectedFlt.push('</div>');
    //    _arrSelectedFlt.push('<div id="flightdetailsdiv' + flightdetailsdivID + '" class="col-md-12 col-sm-12 col-xs-12 m-top-10 offset-0 m-btm-10" style="display: block;">');
    //    _arrSelectedFlt.push('<div class="col-md-12 col-sm-12 col-xs-12 offset-0">');
    //    _arrSelectedFlt.push('<div class="col-md-6 col-sm-6 col-xs-12 offset-0">');
    //    _arrSelectedFlt.push('<div class="col-md-12 col-sm-12 col-xs-12 text-center"><b class="ard"><i class="fa fa-plane planedeprt" aria-hidden="true"></i> &nbsp; Departs</b></div>');
    //    _arrSelectedFlt.push('<div class="col-md-12 col-sm-12 col-xs-12 offset-0"> <span class="depatdd"><b class="boldd">' + DepartureStation + '</b> </span> </div>');
    //    _arrSelectedFlt.push('<div class="col-md-12 col-sm-12 col-xs-12 offset-0"> <span class="depatee">' + dateTime + '</span> </div>');
    //    _arrSelectedFlt.push('</div>');
    //    _arrSelectedFlt.push('<div class="col-md-6 col-sm-6 col-xs-12 offset-0">');
    //    _arrSelectedFlt.push('<div class="col-md-12 col-sm-12 col-xs-12  text-center"><b class="ard"><i class="fa fa-plane font-awsome-plane planearrv" aria-hidden="true"></i> &nbsp; Arrives</b></div>');
    //    _arrSelectedFlt.push('<div class="col-md-12 col-sm-12 col-xs-12 offset-0"><span class="depatdd1"><b class="boldd">' + ArrivalStation + '</b></span></div>');
    //    _arrSelectedFlt.push('<div class="col-md-12 col-sm-12 col-xs-12 offset-0"><span class="depatee1">' + datetimeArr + '</span></div>');
    //    _arrSelectedFlt.push('</div></div>');
    //    _arrSelectedFlt.push('<div class="col-md-12 col-sm-12 col-xs-12">');
    //    _arrSelectedFlt.push('<div class="col-md-11 col-sm-11 col-xs-12 m-top-10 layover-txt"><i class="fa fa-clock-o font-awsm-font" aria-hidden="true"></i> &nbsp; Duration &nbsp;&nbsp;' + DurationDesc + '</div>');
    //    _arrSelectedFlt.push('</div>');
    //    _arrSelectedFlt.push('</div>');
    //    _arrSelectedFlt.push('</div>');
    //}
    //return _arrSelectedFlt;
  }
  //else { _arrSelectedFlt.push(''); return _arrSelectedFlt; }
}
window.FnSelectedInBondFltDetails = FnSelectedInBondFltDetails;

function FnPaxDetailsInput() {
  $("#showdetails").html("");
  var _arrPaxDetails = [];

  //console.log(document.getElementById("hdnFlightSelectedXml").value);
  selectedResultXml = document.getElementById("hdnFlightSelectedXml").value;
  var xmlDataObj = ParseXML(selectedResultXml);

  var Adlt = parseInt(
    xmlDataObj.getElementsByTagName("Adt")[0].firstChild.nodeValue
  );
  var Child = parseInt(
    xmlDataObj.getElementsByTagName("Chd")[0].firstChild.nodeValue
  );
  var Infant = parseInt(
    xmlDataObj.getElementsByTagName("Inf")[0].firstChild.nodeValue
  );
  var totalpax = Adlt + Child + Infant;
  _arrPaxDetails.push(
    '<div class="col-md-12 col-sm-12 col-xs-12 text-center fst_txt1"><i class="fa fa-exclamation-circle exlcami" aria-hidden="true"></i> Traveller Details</div>'
  );
  var _arrPax = [];
  var counter = 0;
  if (Adlt != 0) {
    $("#Adlt").val(Adlt);
    _arrPax = CreatePaxDiv("Adult", Adlt);
    _arrPaxDetails.push(_arrPax);
  }
  if (Child != 0) {
    $("#Child").val(Child);
    _arrPax = CreatePaxDiv("Child", Child);
    _arrPaxDetails.push(_arrPax);
  }
  if (Infant != 0) {
    $("#Infant").val(Infant);
    _arrPax = CreatePaxDiv("Infant", Infant);
    _arrPaxDetails.push(_arrPax);
  }
  //console.log(_arrPaxDetails);
  $("#showdetails").append(_arrPaxDetails.join(""));
}
window.FnPaxDetailsInput = FnPaxDetailsInput;

function ValidatePax() {
  selectedResultXml = document.getElementById("hdnFlightSelectedXml").value;
  var getvalue2 = document.getElementById("hdnFlightSelectedXmlInbond").value;
  var xmlDataObj = ParseXML(selectedResultXml);
  var xmlDataObj2 = ParseXML(getvalue2);
  var CarrierCode1 =
    xmlDataObj.getElementsByTagName("CarrierCode")[0].firstChild.nodeValue;
  var trip = xmlDataObj.getElementsByTagName("Trip")[0].firstChild.nodeValue;
  var CarrierCode2 = "";
  if (trip == "R") {
    CarrierCode2 =
      xmlDataObj2.getElementsByTagName("CarrierCode")[0].firstChild.nodeValue;
  }
  var Adlt = xmlDataObj.getElementsByTagName("Adt")[0].firstChild.nodeValue;
  var Child = xmlDataObj.getElementsByTagName("Chd")[0].firstChild.nodeValue;
  var Infant = xmlDataObj.getElementsByTagName("Inf")[0].firstChild.nodeValue;
  var sect = xmlDataObj.getElementsByTagName("Sector")[0].firstChild.nodeValue;
  document.getElementById("hiddensector").value = sect;
  var totalpax = parseInt(Adlt) + parseInt(Child) + parseInt(Infant);
  // var Infant = $('#Infant').val();
  var temp = totalpax;
  var msg = "";
  var counter = 0;

  for (var i = 1; i <= totalpax; i++) {
    //Code added by Nehal 20/11/2022 for compulsion to select Title
    var title = $("#DropDownList" + i).val();
    if (title == "") {
      msg = msg + "Please Select Pax " + i + "Title\n";
      counter++;
    }
    //
    var x = $("#TextBoxFname" + i + "")
      .val()
      .trim();
    var y = $("#TextBoxLName" + i + "")
      .val()
      .trim();
    if (x == "" || x == undefined) {
      $("#TextBoxFname" + i + "").css({
        "border-color": "#FB0415",
        "border-weight": "1px",
        "border-style": "solid",
      });
      msg = msg + "Please fill Pax " + i + " First Name.\n";
      counter++;
    } else {
      var X1 = validatenAME(
        $("#TextBoxFname" + i + "")
          .val()
          .trim()
      );
      if (X1 == false || X1 == "" || X1 == undefined) {
        $("#TextBoxFname" + i + "").css({
          "border-color": "#FB0415",
          "border-weight": "1px",
          "border-style": "solid",
        });
        msg =
          msg +
          "Please fill Pax " +
          i +
          " First Name without special character.\n";
        counter++;
      }
    }
    if (y == "" || y == undefined) {
      $("#TextBoxLName" + i + "").css({
        "border-color": "#FB0415",
        "border-weight": "1px",
        "border-style": "solid",
      });
      msg = msg + "Please fill Pax " + i + " Last Name.\n";
      counter++;
    } else {
      var Y1 = validateLNAME(
        $("#TextBoxLName" + i + "")
          .val()
          .trim()
      );
      if (Y1 == false || Y1 == "" || Y1 == undefined) {
        $("#TextBoxLName" + i + "").css({
          "border-color": "#FB0415",
          "border-weight": "1px",
          "border-style": "solid",
        });
        msg =
          msg +
          "Please fill Pax " +
          i +
          " Last Name without special character.\n";
        counter++;
      }
    }
    if (y.length <= 1) {
      $("#TextBoxLName" + i + "").css({
        "border-color": "#FB0415",
        "border-weight": "1px",
        "border-style": "solid",
      });
      msg =
        msg +
        "Please fill the Pax " +
        i +
        " Last Name must be greater than 1 letter.\n";
      counter++;
    }
    if (sect == "I") {
      var x = $("#txtnationality" + i + "").val();
      var y = $("#PassPortno" + i + "").val();
      var z = $("#txtExpirydate" + i + "").val();
      var adtage = $("#txtdate" + i + "").val();
      if (x == "" || x == undefined) {
        $("#txtnationality" + i + "").css({
          "border-color": "#FB0415",
          "border-weight": "1px",
          "border-style": "solid",
        });
        msg = msg + "Please fill  Pax " + i + " Nationality.\n";

        counter++;
      }
      if (y == "" || y == undefined) {
        $("#PassPortno" + i + "").css({
          "border-color": "#FB0415",
          "border-weight": "1px",
          "border-style": "solid",
        });
        msg = msg + "Please fill  Pax " + i + " PassPortNo.\n";

        counter++;
      }
      if (z == "" || z == undefined) {
        $("#txtExpirydate" + i + "").css({
          "border-color": "#FB0415",
          "border-weight": "1px",
          "border-style": "solid",
        });
        msg = msg + "Please fill  Pax " + i + " Passport Expirydate.\n";

        counter++;
      }
      if (adtage == "" || adtage == undefined) {
        $("#txtdate" + i + "").css({
          "border-color": "#FB0415",
          "border-weight": "1px",
          "border-style": "solid",
        });
        msg = msg + "Please fill  Pax " + i + " DOB.\n";

        counter++;
      }
    } else {
      var adtage = $("#txtdate" + i + "").val();
      if (CarrierCode1 == "I5" || CarrierCode2 == "I5") {
        if (adtage == "" || adtage == undefined) {
          $("#txtdate" + i + "").css({
            "border-color": "#FB0415",
            "border-weight": "1px",
            "border-style": "solid",
          });
          msg = msg + "Please fill  Pax " + i + " DOB.\n";

          counter++;
        }
      }
    }
  }
  //----------------------------------------validate GST section start----------------------------------
  if ($("#txtgstregno").val().trim() != "") {
    var gst_REGNO = validateGSTregNO($("#txtgstregno").val().trim());
    var gstaddr = validateAddress($("#txtgstcompnyaddress").val().trim());
    var gstemail = validateEmail($("#txtgstcompnyemail").val());
    var gstmobile = validatePhone($("#txtgstcompnycontactno").val());
    var gstcompnynm = validateAddress($("#txtgstregcompanynm").val().trim());

    if (gst_REGNO == false || gst_REGNO == undefined) {
      $("#txtgstregno").css({
        "border-color": "#FB0415",
        "border-weight": "1px",
        "border-style": "solid",
      });
      msg = msg + "Please fill correct GST no.\n";
      counter++;
    }
    if (gstaddr == false || gstaddr == "" || gstaddr == undefined) {
      $("#txtgstcompnyaddress").css({
        "border-color": "#FB0415",
        "border-weight": "1px",
        "border-style": "solid",
      });
      msg = msg + "Please fill GST Address with no any special character.\n";
      counter++;
    }
    if (gstemail == false || gstemail == "" || gstemail == undefined) {
      $("#txtgstcompnyemail").css({
        "border-color": "#FB0415",
        "border-weight": "1px",
        "border-style": "solid",
      });
      msg = msg + "Please fill  GST Email Address.\n";
      counter++;
    }
    if (gstmobile == false || gstmobile == "" || gstmobile == undefined) {
      $("#txtgstcompnycontactno").css({
        "border-color": "#FB0415",
        "border-weight": "1px",
        "border-style": "solid",
      });
      msg = msg + "Please fill GST Mobile Number.\n";
      counter++;
    }
    if (gstcompnynm == false || gstcompnynm == "" || gstcompnynm == undefined) {
      $("#txtgstregcompanynm").css({
        "border-color": "#FB0415",
        "border-weight": "1px",
        "border-style": "solid",
      });
      msg = msg + "Please fill company name with no any special character.\n";
      counter++;
    }
  }
  //----------------------------------------validate GST section End----------------------------------
  var addr = validateAddress($("#Address").val().trim());
  var E = validateEmail($("#Email").val());
  if (addr == false || addr == "" || addr == undefined) {
    $("#Address").css({
      "border-color": "#FB0415",
      "border-weight": "1px",
      "border-style": "solid",
    });
    msg = msg + "Please fill Address with no any special character.\n";
    counter++;
  }
  if (E == false || E == "" || E == undefined) {
    $("#Email").css({
      "border-color": "#FB0415",
      "border-weight": "1px",
      "border-style": "solid",
    });
    msg = msg + "Please fill  Email Address.\n";

    counter++;
  }
  var t = validatePhone($("#txtMobileNo").val());
  if (t == false || t == "" || t == undefined) {
    $("#txtMobileNo").css({
      "border-color": "#FB0415",
      "border-weight": "1px",
      "border-style": "solid",
    });
    msg = msg + "Please fill  Mobile Number.\n";
    counter++;
  }
  var ValidDOB = validateDOB(Adlt, Child, Infant);
  var validateDOBResult = ValidDOB[0];
  if (validateDOBResult != 0) {
    msg = msg + ValidDOB[1];
  }
  if (msg != "") {
    alert(msg);
    return false;
  } else return true;
}
window.ValidatePax = ValidatePax;

function removerdvlidat() {}

function validateEmail(email) {
  var reg = /^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$/;
  if (reg.test(email)) {
    return true;
  } else {
    return false;
  }
}
window.validateEmail = validateEmail;
function validateGSTregNO(gstno) {
  var reg = /\d{2}[A-Za-z]{5}\d{4}[A-Za-z]{1}\d[Zz]{1}[A-Za-z\d]{1}/;
  if (reg.test(gstno)) {
    return true;
  } else {
    return false;
  }
}
window.validateGSTregNO = validateGSTregNO;
function validatePhone(phoneText) {
  if ($.trim(phoneText).length >= 10) {
    var filter = /^[0-9-+]+$/;
    if (filter.test(phoneText)) {
      return true;
    }
  } else {
    return false;
  }
}
window.validatePhone = validatePhone;
function validatenAME(nAMEText) {
  //reg expression for textbox do not accept any special charector
  //reg expression for textbox contain Alphanumeric space with comma(,)
  var filter = /^[a-zA-Z\s]+$/;
  if (filter.test(nAMEText)) {
    return true;
  } else {
    return false;
  }
}
window.validatenAME = validatenAME;
function validateLNAME(nAMEText) {
  //reg expression for textbox do not accept any special charector
  //reg expression for textbox contain Alphanumeric space with comma(,)
  var filter = /^[a-zA-Z]+$/;
  if (filter.test(nAMEText)) {
    return true;
  } else {
    return false;
  }
}
window.validateLNAME = validateLNAME;
function validateAddress(AddressText) {
  //reg expression for textbox contain Alphanumeric space with comma(,)
  //var filter = /^[0-9a-zA-Z\s\,]+$/;
  //reg expression for textbox contain Alphanumeric with space
  var filter = /^[0-9a-zA-Z\s]+$/;
  if (filter.test(AddressText)) {
    return true;
  } else {
    return false;
  }
}
window.validateAddress = validateAddress;
function CreatePaxDiv(Phase, PaxNo) {
  selectedResultXml = document.getElementById("hdnFlightSelectedXml").value;
  var icons = document.getElementById("hdnlivepathimage").value;
  var xmlDataObj = ParseXML(selectedResultXml);
  var CarrierCode =
    xmlDataObj.getElementsByTagName("CarrierCode")[0].firstChild.nodeValue;
  var sect = xmlDataObj.getElementsByTagName("Sector")[0].firstChild.nodeValue;
  totalpax = PaxNo;
  var _arrPaxDetails = [];
  var _index = 0;
  var ad = parseInt($("#Adlt").val());
  var chld = parseInt($("#Child").val());
  var Inf = parseInt($("#Infant").val());
  var paxlogo = "<image src='" + icons + "adult.png' style='width:25px;' />";
  if (Phase == "Child") {
    _index = ad;
    paxlogo = "<image src='" + icons + "child.png' style='width:25px;' />";
  }
  if (Phase == "Infant") {
    _index = ad + chld;
    paxlogo = "<image src='" + icons + "infant.png' style='width:25px;' />";
  }
  for (var adlt = 1; adlt <= totalpax; adlt++) {
    var p = _index;
    var _ind = p + adlt;
    _arrPaxDetails.push(
      '<div id="showdetails' +
        _ind +
        '" class="col-md-12 col-sm-12 col-xs-12 data_dv1">'
    );
    _arrPaxDetails.push('<div class="col-md-12 col-sm-12 col-xs-12 offset-0">');
    _arrPaxDetails.push(
      '<div class="col-md-2 col-sm-6 col-xs-12 offset-0 adticmo" style="position: absolute;   margin-top: 36px; margin-left:-5px;">' +
        paxlogo +
        " " +
        Phase +
        " " +
        adlt +
        " </div>"
    );
    _arrPaxDetails.push("</div>");
    _arrPaxDetails.push(
      '<div id="showmorebtn' +
        _ind +
        '" class="col-md-12 col-sm-12 col-xs-12 offset-0 m-top-5 pdng-btm-10" style="display: block;">'
    );
    _arrPaxDetails.push('<div class="col-md-12 col-sm-12 col-xs-12 offset-0">');
    _arrPaxDetails.push(
      '<div class="col-md-2 col-sm-2 col-xs-12 f-bold"></div>'
    );
    _arrPaxDetails.push(
      '<div class="col-md-4 col-sm-5 col-xs-12 offset-1 f-bold"></div>'
    );
    _arrPaxDetails.push(
      '<div class="col-md-4 col-sm-5 col-xs-12 offset-1 f-bold"></div>'
    );
    _arrPaxDetails.push(
      '<div class="col-md-2 col-sm-2 col-xs-12 f-bold "></div>'
    );
    _arrPaxDetails.push("</div>");
    if (Phase == "Infant" || Phase == "Child") {
      _arrPaxDetails.push(
        '<div class="col-md-12 col-sm-12 col-xs-12 offset-0 m-top-10">'
      );
      _arrPaxDetails.push(
        '<div class="col-md-1 col-sm-2 col-xs-12 padm0"></div>'
      );
      _arrPaxDetails.push(
        '<div class="col-md-2 col-sm-2 col-xs-12 padm0"><label style="font-weight: 100;">Title</label><div class="col-md-12 offset-0"><select name="DropDownList' +
          _ind +
          '" id="DropDownList' +
          _ind +
          '" class="form-control selc" >'
      );
      _arrPaxDetails.push('<option value="">Select</option>');
      _arrPaxDetails.push('<option value="MSTR">Mstr</option>');
      _arrPaxDetails.push('<option value="MISS">Miss</option>');
      _arrPaxDetails.push("</select>");
      _arrPaxDetails.push("</div></div>");
    } else {
      _arrPaxDetails.push(
        '<div class="col-md-12 col-sm-12 col-xs-12 offset-0 m-top-10">'
      );
      _arrPaxDetails.push(
        '<div class="col-md-1 col-sm-2 col-xs-12 padm0"></div>'
      );
      _arrPaxDetails.push(
        '<div class="col-md-2 col-sm-2 col-xs-12 padm0"><label style="font-weight: 100;">Title</label><div class="col-md-12 offset-0"><select name="DropDownList' +
          _ind +
          '" id="DropDownList' +
          _ind +
          '" class="form-control selc">'
      );
      _arrPaxDetails.push('<option value="">Select</option>');
      _arrPaxDetails.push('<option value="MR">Mr</option>');
      _arrPaxDetails.push('<option value="MRS">Mrs</option>');
      _arrPaxDetails.push('<option value="MS">Ms</option>');
      _arrPaxDetails.push("</select>");
      _arrPaxDetails.push("</div></div>");
    }
    _arrPaxDetails.push(
      '<div class="col-md-3 col-sm-4 col-xs-12 offset-1 padm0"><label style="font-weight: 100;">First Name</label><div class="col-md-12 offset-0"> <input name="TextBox1" style="text-transform:uppercase; font-size:11px;" type="text" id="TextBoxFname' +
        _ind +
        '" onkeypress="return AllowAlphabet(event);" class="form-control" autocomplete="off"></div></div>'
    );
    _arrPaxDetails.push(
      '<div class="col-md-3 col-sm-4 col-xs-12 offset-1 padm0"><label style="font-weight: 100;">Last Name</label><div class="col-md-12 offset-0"><input name="TextBox2" style="text-transform:uppercase; font-size:11px;" type="text" id="TextBoxLName' +
        _ind +
        '" onkeypress="return AllowAlphabet1(event);" class="form-control" autocomplete="off"></div></div>'
    );

    if (Phase == "Infant" || Phase == "Child") {
      if (Phase == "Child") {
        _arrPaxDetails.push(
          '<div class="col-md-2 col-sm-2 col-xs-12 offset-0"><label style="font-weight: 100;">DOB</label><div class="col-md-12 offset-0"><input id="txtdate' +
            _ind +
            '"  tabindex="" type="text" class="form-control Chddob selc" placeholder="DD/MM/YYYY" autocomplete="off"></div></div>'
        );
      } else {
        _arrPaxDetails.push(
          '<div class="col-md-2 col-sm-2 col-xs-12 offset-0"><label style="font-weight: 100;">DOB</label><div class="col-md-12 offset-0"><input id="txtdate' +
            _ind +
            '"  tabindex="" type="text" class="form-control infdob selc" placeholder="DD/MM/YYYY" autocomplete="off"></div></div>'
        );
      }
    } else {
      _arrPaxDetails.push(
        '<div class="col-md-2 col-sm-2 col-xs-12 offset-0"><label style="font-weight: 100;">DOB</label><div class="col-md-12 offset-0"><input id="txtdate' +
          _ind +
          '"  tabindex="" type="text" class="form-control Adtdob selc" placeholder="DD/MM/YYYY" autocomplete="off"></div></div>'
      );
    }

    var nationalty = document.getElementById("hiddenNationality").value;
    var JSONnationalty = JSON.parse(nationalty);

    if (sect == "I") {
      _arrPaxDetails.push(
        '<div class="col-md-12 col-sm-10 col-xs-12 offset-0">'
      );
      _arrPaxDetails.push(
        '<div class="col-md-3 col-sm-10 col-xs-12 panel-body"><select name="Nationality" id="txtnationality' +
          _ind +
          '" class="form-control f-11"><option value=""> Nationality</option>'
      );
      if (JSONnationalty.length > 0) {
        for (var i = 0; i < JSONnationalty.length; i++) {
          // console.log(nationalty[i]);
          _arrPaxDetails.push(
            '<option value="' +
              JSONnationalty[i].CountryCode +
              '">' +
              JSONnationalty[i].CountryName +
              "</option>"
          );
        }
      }

      _arrPaxDetails.push("</select></div>");
      _arrPaxDetails.push(
        '<div class="col-md-3 col-sm-10 col-xs-12 panel-body"><input name="TextBox" type="text" id="PassPortno' +
          _ind +
          '" class="form-control f-11" placeholder="Enter PassportNo"></div>'
      );
      _arrPaxDetails.push(
        '<div class="col-md-3 col-sm-10 col-xs-12 panel-body" style="padding-left:0px;"><input name="TextBox" type="text" id="txtExpirydate' +
          _ind +
          '" class="form-control dtpicker f-11"  placeholder="Enter ExpiryDate"></div>'
      );
      _arrPaxDetails.push("</div>");
    }
    _arrPaxDetails.push("</div>");
    //--------------END here DOB---------------
    if (Phase != "Infant") {
      _arrPaxDetails.push(
        '<div class="col-md-12 col-sm-12 col-xs-12 offset-0 m-top-10 crcre-pnt" onclick="showmoredetail(' +
          _ind +
          ')">'
      );
      _arrPaxDetails.push(
        '<div class="col-md-4 col-sm-2 col-xs-12 offset-0 color-blue" style="font-size: 21px; font-weight: 400;" > <i class="fa fa-plus-circle" style="color: #138be4;" aria-hidden="true" ></i> Add Meal / Baggage</div>'
      );
      _arrPaxDetails.push(
        '<div class="col-md-8 col-sm-10 col-xs-12 clyye">(Meal, Baggage, Frequent Flyer Number etc)</div>'
      );
      _arrPaxDetails.push("</div>");
      _arrPaxDetails.push(
        '<div id="showmore' +
          _ind +
          '" class="col-md-12 col-sm-12 col-xs-12 m-top-10" style="display: none;">'
      );
      _arrPaxDetails.push(
        '<div class="col-md-12 col-sm-12 col-xs-12 text-center offset-0">'
      );
      _arrPaxDetails.push(
        '<div class="col-md-4 col-sm-4 col-xs-12 hisedse"><b>Select Meals</b></div>'
      );
      _arrPaxDetails.push(
        '<div class="col-md-8 col-sm-8 col-xs-12 m-top-10" style=""></div>'
      );
      _arrPaxDetails.push("</div>");

      _arrPaxDetails.push(
        '<div id="ssrDetails' +
          _ind +
          '" class="col-md-12 col-sm-12 col-xs-12 text-center offset-0 " style="margin-top:-15px;">'
      );

      var resultHtml = SSRDetailsDiv(_ind);

      _arrPaxDetails.push(resultHtml);

      _arrPaxDetails.push("</div>");

      _arrPaxDetails.push(
        '<div class="col-md-12 col-sm-12 col-xs-12 text-center offset-0 ">'
      );
      _arrPaxDetails.push(
        '<div class="col-md-4 col-sm-4 col-xs-12 hisedse"><b>Select Baggage</b></div>'
      );
      _arrPaxDetails.push(
        '<div class="col-md-8 col-sm-8 col-xs-12 m-top-10" style=""></div>'
      );
      _arrPaxDetails.push("</div>");

      _arrPaxDetails.push(
        '<div  id="ssrbaggage' +
          _ind +
          '" class="col-md-12 col-sm-12 col-xs-12 text-center offset-0 " style="margin-top:-15px;">'
      );
      var baggageHtml = ssrbaggageDiv(_ind);
      _arrPaxDetails.push(baggageHtml);

      if (Phase == "Adult") {
        _arrPaxDetails.push(
          '<div class="col-md-12 col-sm-12 col-xs-12 offset-0 " style="">'
        );

        _arrPaxDetails.push(
          '<div class="col-md-12 col-sm-10 col-xs-12 flyerbox">'
        );
        _arrPaxDetails.push(
          '<div class="col-md-12 col-sm-12 col-xs-12">Select an airline and enter the Frequent Flyer Number</div>'
        );
        _arrPaxDetails.push(
          '<div class="col-md-12 col-sm-12 col-xs-12 offset-0 m-top-10">'
        );
        _arrPaxDetails.push('<div class="col-md-6 col-sm-6 col-xs-12">');
        _arrPaxDetails.push(
          '<input name="txtffnnoAirline" type="text" id="txtffnnoAirline' +
            _ind +
            '" maxlength="2" class="form-control chktestalphanumeric" placeholder="Enter Frequent Flyer Airline"/>'
        );
        _arrPaxDetails.push("</div>");
        _arrPaxDetails.push(
          '<div class="col-md-6 col-sm-6 col-xs-12"><input name="txtffnno" type="text" maxlength="10" id="txtFrequentFlyer' +
            _ind +
            '" class="form-control chktestalphanumeric" placeholder="Enter Frequent Flyer Number"/></div>'
        );
        _arrPaxDetails.push("</div>");
        _arrPaxDetails.push("</div>");

        if ($("#hidenRT").val() == "RW") {
          _arrPaxDetails.push(
            '<div class="col-md-12 col-sm-10 col-xs-12 flyerbox">'
          );
          _arrPaxDetails.push(
            '<div class="col-md-12 col-sm-12 col-xs-12">Select an airline and enter the Frequent Flyer Number</div>'
          );
          _arrPaxDetails.push(
            '<div class="col-md-12 col-sm-12 col-xs-12 offset-0 m-top-10">'
          );
          _arrPaxDetails.push('<div class="col-md-6 col-sm-6 col-xs-12">');
          _arrPaxDetails.push(
            '<input name="txtffnnoAirlineIB" type="text" id="txtffnnoAirlineIB' +
              _ind +
              '" maxlength="2" class="form-control chktestalphanumeric" placeholder="Enter Frequent Flyer Airline"/>'
          );
          _arrPaxDetails.push("</div>");
          _arrPaxDetails.push(
            '<div class="col-md-6 col-sm-6 col-xs-12"><input name="txtffnnoIB" type="text" maxlength="10" id="txtFrequentFlyerIB' +
              _ind +
              '" class="form-control chktestalphanumeric" placeholder="Enter Frequent Flyer Number"/></div>'
          );
          _arrPaxDetails.push("</div>");
          _arrPaxDetails.push("</div>");
        }

        _arrPaxDetails.push("</div>");
      }
    }
    _arrPaxDetails.push("</div></div>");
    _arrPaxDetails.push("</div>");
    _arrPaxDetails.push("</div>");
  }
  return _arrPaxDetails.join("");
}
window.CreatePaxDiv = CreatePaxDiv;
function replaceAll(str, find, replace) {
  var i = str.indexOf(find);
  if (i > -1) {
    str = str.replace(find, replace);
    i = i + replace.length;
    var st2 = str.substring(i);
    if (st2.indexOf(find) > -1) {
      str = str.substring(0, i) + replaceAll(st2, find, replace);
    }
  }
  return str;
}
window.replaceAll = replaceAll;
function SSRDetailsDiv(_index) {
  var _innerHtml = [];
  selectedResultXml = document.getElementById("hdnFlightSelectedXml").value;
  if (selectedResultXml != "") {
    var xmlDataObj = ParseXML(selectedResultXml);
    stops = xmlDataObj.getElementsByTagName("Stops")[0].firstChild.nodeValue;
    var CarrierCode =
      xmlDataObj.getElementsByTagName("CarrierCode")[0].firstChild.nodeValue;

    //if (CarrierCode == "G8" || CarrierCode == "SG" || CarrierCode == "6E") {

    for (var i = 0; i <= stops; i++) {
      $("#showmore" + i).html("");
      var DepartureStation =
        xmlDataObj.getElementsByTagName("Origin")[i].firstChild.nodeValue;
      var ArrivalStation =
        xmlDataObj.getElementsByTagName("Destination")[i].firstChild.nodeValue;
      var CarrierCode =
        xmlDataObj.getElementsByTagName("CarrierCode")[i].firstChild.nodeValue;
      _innerHtml.push(
        '<div class="col-md-6 col-sm-12 col-xs-12 text-center offset-0 m-top-20">'
      );
      _innerHtml.push(
        '<div class="col-md-4 col-sm-4 col-xs-12 m-top-5 color-blue">'
      );
      _innerHtml.push(
        "<b>" + DepartureStation + "-" + ArrivalStation + "</b></div>"
      );
      _innerHtml.push('<div class="col-md-8 col-sm-4 col-xs-12">');
      _innerHtml.push(
        '<select name="SSRListDDL' +
          _index +
          '" id="SSRListDDL' +
          _index +
          '"onchange="AddInFare()" class="form-control f-11" onclick="bindSSRList(' +
          _index +
          ')">'
      );
      _innerHtml.push('<option value="0">No Meal</option>');
      _innerHtml.push(
        ' </select></div><div class="col-md-1 col-sm-1 col-xs-12"></div></div>'
      );
      //break;
    }

    //}
    //else {
    //    _innerHtml.push('<div class="col-md-12 col-sm-12 col-xs-12 text-center offset-0 ">');
    //    _innerHtml.push('<span style="color:red;">Onward: For meal request contact to our team </span>');
    //    _innerHtml.push('</div>');
    //}
  }
  var selectedResultXml_I = document.getElementById(
    "hdnFlightSelectedXmlInbond"
  ).value;
  if (selectedResultXml_I != "") {
    var xmlDataObj = ParseXML(selectedResultXml_I);
    stops = xmlDataObj.getElementsByTagName("Stops")[0].firstChild.nodeValue;
    var CarrierCode =
      xmlDataObj.getElementsByTagName("CarrierCode")[0].firstChild.nodeValue;

    //if (CarrierCode == "G8" || CarrierCode == "SG" || CarrierCode == "6E") {

    for (var i = 0; i <= stops; i++) {
      $("#showmore" + i).html("");
      var DepartureStation =
        xmlDataObj.getElementsByTagName("Origin")[i].firstChild.nodeValue;
      var ArrivalStation =
        xmlDataObj.getElementsByTagName("Destination")[i].firstChild.nodeValue;
      var CarrierCode =
        xmlDataObj.getElementsByTagName("CarrierCode")[i].firstChild.nodeValue;
      _innerHtml.push(
        '<div class="col-md-6 col-sm-12 col-xs-12 text-center offset-0 m-top-20">'
      );
      _innerHtml.push(
        '<div class="col-md-4 col-sm-4 col-xs-12 m-top-5 color-blue">'
      );
      _innerHtml.push(
        "<b>" + DepartureStation + "-" + ArrivalStation + "</b></div>"
      );
      _innerHtml.push('<div class="col-md-8 col-sm-4 col-xs-12">');
      _innerHtml.push(
        '<select name="SSRListDDL_I' +
          _index +
          '" id="SSRListDDL_I' +
          _index +
          '" onchange="AddInFare()" class="form-control f-11" onclick="bindSSRList(' +
          _index +
          ')">'
      );
      _innerHtml.push('<option value="0">No Meal</option>');
      _innerHtml.push(
        ' </select></div><div class="col-md-1 col-sm-1 col-xs-12"></div></div>'
      );
      break;
    }

    //}
    //else {
    //    _innerHtml.push('<div class="col-md-12 col-sm-12 col-xs-12 text-center offset-0 ">');
    //    _innerHtml.push('<span style="color:red;">Return: For meal request contact to our team </span>');
    //    _innerHtml.push('</div>');
    //}
  }
  return _innerHtml.join("");
}
function ssrbaggageDiv(_index) {
  var _innerHtml = [];
  selectedResultXml = document.getElementById("hdnFlightSelectedXml").value;
  if (selectedResultXml != "") {
    var xmlDataObj = ParseXML(selectedResultXml);
    stops = xmlDataObj.getElementsByTagName("Stops")[0].firstChild.nodeValue;
    var CarrierCode1 =
      xmlDataObj.getElementsByTagName("CarrierCode")[0].firstChild.nodeValue;

    //if (CarrierCode1 == "G8" || CarrierCode1 == "SG" || CarrierCode1 == "6E") {

    for (var i = 0; i <= stops; i++) {
      $("#showmore" + i).html("");
      var DepartureStation =
        xmlDataObj.getElementsByTagName("Origin")[i].firstChild.nodeValue;
      var ArrivalStation =
        xmlDataObj.getElementsByTagName("Destination")[i].firstChild.nodeValue;
      var CarrierCode =
        xmlDataObj.getElementsByTagName("CarrierCode")[i].firstChild.nodeValue;
      _innerHtml.push(
        '<div class="col-md-6 col-sm-12 col-xs-12 text-center offset-0 m-top-20">'
      );
      _innerHtml.push(
        '<div class="col-md-4 col-sm-4 col-xs-12 m-top-5 color-blue">'
      );
      _innerHtml.push(
        "<b>" + DepartureStation + "-" + ArrivalStation + "</b></div>"
      );
      _innerHtml.push('<div class="col-md-8 col-sm-4 col-xs-12">');
      _innerHtml.push(
        '<select name="SSRbaggageDDL' +
          _index +
          '" id="SSRbaggageDDL' +
          _index +
          '" onchange="AddInFare()" class="form-control f-11" onclick="bindSSRList(' +
          _index +
          ')">'
      );
      _innerHtml.push('<option value="0">No Baggage</option>');
      _innerHtml.push(
        ' </select></div><div class="col-md-1 col-sm-1 col-xs-12"></div></div>'
      );
      break;
    }

    //}
    //else {
    //    _innerHtml.push('<div class="col-md-12 col-sm-12 col-xs-12 text-center offset-0 ">');
    //    _innerHtml.push('<span style="color:red;">Onward: For baggage request contact to our team </span>');
    //    _innerHtml.push('</div>');
    //}
  }
  selectedResultXml_I = document.getElementById(
    "hdnFlightSelectedXmlInbond"
  ).value;
  if (selectedResultXml_I != "") {
    var xmlDataObj = ParseXML(selectedResultXml_I);
    stops = xmlDataObj.getElementsByTagName("Stops")[0].firstChild.nodeValue;
    var CarrierCode1 =
      xmlDataObj.getElementsByTagName("CarrierCode")[0].firstChild.nodeValue;

    //if (CarrierCode1 == "G8" || CarrierCode1 == "SG" || CarrierCode1 == "6E") {

    for (var i = 0; i <= stops; i++) {
      $("#showmore" + i).html("");
      var DepartureStation =
        xmlDataObj.getElementsByTagName("Origin")[i].firstChild.nodeValue;
      var ArrivalStation =
        xmlDataObj.getElementsByTagName("Destination")[i].firstChild.nodeValue;
      var CarrierCode =
        xmlDataObj.getElementsByTagName("CarrierCode")[i].firstChild.nodeValue;
      _innerHtml.push(
        '<div class="col-md-6 col-sm-12 col-xs-12 text-center offset-0 m-top-20">'
      );
      _innerHtml.push(
        '<div class="col-md-4 col-sm-4 col-xs-12 m-top-5 color-blue">'
      );
      _innerHtml.push(
        "<b>" + DepartureStation + "-" + ArrivalStation + "</b></div>"
      );
      _innerHtml.push('<div class="col-md-8 col-sm-4 col-xs-12">');
      _innerHtml.push(
        '<select name="SSRbaggageDDL_I' +
          _index +
          '" id="SSRbaggageDDL_I' +
          _index +
          '" onchange="AddInFare()" class="form-control f-11" onclick="bindSSRList(' +
          _index +
          ')">'
      );
      _innerHtml.push('<option value="0">No Baggage</option>');
      _innerHtml.push(
        ' </select></div><div class="col-md-1 col-sm-1 col-xs-12"></div></div>'
      );
      break;
    }

    //}
    //else {
    //    _innerHtml.push('<div class="col-md-12 col-sm-12 col-xs-12 text-center offset-0 ">');
    //    _innerHtml.push('<span style="color:red;">Return: For baggage request contact to our team </span>');
    //    _innerHtml.push('</div>');
    //}
  }
  return _innerHtml.join("");
}
function bindSSRList(_index) {
  var selectedSSR = document.getElementById("hdnSSRavailabilityOut").value;
  if (selectedSSR != "") {
    var xmlDataObj = ParseXML(selectedSSR);
    var Count = xmlDoc.getElementsByTagName("SSRInfo");
    var total = Count.length;
    var CodeType = "";
    var check = $("#SSRListDDL" + _index).hasClass("added");
    if (total > 0 && check == false) {
      $("#SSRListDDL" + _index).addClass("form-control added");
      //$("#SSRListDDL" + _index).append(new Option("No Meal", "0"));
      for (i = 0; i < total; i++) {
        if (
          xmlDataObj.getElementsByTagName("CodeType")[i].firstChild.nodeValue ==
          "M"
        ) {
          CodeType =
            xmlDataObj.getElementsByTagName("CodeType")[i].firstChild.nodeValue;
          var Amount =
            xmlDataObj.getElementsByTagName("Amount")[i].firstChild.nodeValue;
          var AmountINR =
            xmlDataObj.getElementsByTagName("AmountINR")[i].firstChild
              .nodeValue;
          var detail = "";
          if (xmlDataObj.getElementsByTagName("Detail")[0].childNodes[0]) {
            detail =
              xmlDataObj.getElementsByTagName("Detail")[i].firstChild.nodeValue;
          }

          var Description =
            xmlDataObj.getElementsByTagName("Description")[i].firstChild
              .nodeValue;
          var Val = Description + "--" + Amount;
          var Code =
            AmountINR +
            "||" +
            xmlDataObj.getElementsByTagName("Code")[i].firstChild.nodeValue +
            "||" +
            detail;
          $("#SSRListDDL" + _index).append(new Option(Val, Code));
        }
        if (
          xmlDataObj.getElementsByTagName("CodeType")[i].firstChild.nodeValue ==
          "B"
        ) {
          CodeType =
            xmlDataObj.getElementsByTagName("CodeType")[i].firstChild.nodeValue;
          var Amount =
            xmlDataObj.getElementsByTagName("Amount")[i].firstChild.nodeValue;
          var AmountINR =
            xmlDataObj.getElementsByTagName("AmountINR")[i].firstChild
              .nodeValue;
          var detail =
            xmlDataObj.getElementsByTagName("Detail")[i].firstChild.nodeValue;
          var Description =
            xmlDataObj.getElementsByTagName("Description")[i].firstChild
              .nodeValue;
          var Val = Description + "--" + Amount;
          var Code =
            AmountINR +
            "||" +
            xmlDataObj.getElementsByTagName("Code")[i].firstChild.nodeValue +
            "||" +
            detail;
          $("#SSRbaggageDDL" + _index).append(new Option(Val, Code));
        }
      }
    }
  }
  var selectedSSR_I = document.getElementById("hdnSSRavailabilityIn").value;
  if (selectedSSR_I != "") {
    var xmlDataObj = ParseXML(selectedSSR_I);
    var Count = xmlDoc.getElementsByTagName("SSRInfo");
    var total = Count.length;
    var CodeType = "";
    var check = $("#SSRListDDL_I" + _index).hasClass("added");
    if (total > 0 && check == false) {
      $("#SSRListDDL_I" + _index).addClass("form-control added");
      //$("#SSRListDDL_I" + _index).append(new Option("No Meal", "0"));
      for (i = 0; i < total; i++) {
        if (
          xmlDataObj.getElementsByTagName("CodeType")[i].firstChild.nodeValue ==
          "M"
        ) {
          CodeType =
            xmlDataObj.getElementsByTagName("CodeType")[i].firstChild.nodeValue;
          var Amount =
            xmlDataObj.getElementsByTagName("Amount")[i].firstChild.nodeValue;
          var AmountINR =
            xmlDataObj.getElementsByTagName("AmountINR")[i].firstChild
              .nodeValue;
          var detail = "";
          if (xmlDataObj.getElementsByTagName("Detail")[0].childNodes[0]) {
            detail =
              xmlDataObj.getElementsByTagName("Detail")[i].firstChild.nodeValue;
          }

          var Description =
            xmlDataObj.getElementsByTagName("Description")[i].firstChild
              .nodeValue;
          var Val = Description + "--" + Amount;
          var Code =
            AmountINR +
            "||" +
            xmlDataObj.getElementsByTagName("Code")[i].firstChild.nodeValue +
            "||" +
            detail;
          $("#SSRListDDL_I" + _index).append(new Option(Val, Code));
        }
        if (
          xmlDataObj.getElementsByTagName("CodeType")[i].firstChild.nodeValue ==
          "B"
        ) {
          CodeType =
            xmlDataObj.getElementsByTagName("CodeType")[i].firstChild.nodeValue;
          var Amount =
            xmlDataObj.getElementsByTagName("Amount")[i].firstChild.nodeValue;
          var AmountINR =
            xmlDataObj.getElementsByTagName("AmountINR")[i].firstChild
              .nodeValue;
          var detail =
            xmlDataObj.getElementsByTagName("Detail")[i].firstChild.nodeValue;
          var Description =
            xmlDataObj.getElementsByTagName("Description")[i].firstChild
              .nodeValue;
          var Val = Description + "--" + Amount;
          var Code =
            AmountINR +
            "||" +
            xmlDataObj.getElementsByTagName("Code")[i].firstChild.nodeValue +
            "||" +
            detail;
          $("#SSRbaggageDDL_I" + _index).append(new Option(Val, Code));
        }
      }
    }
  }
}
window.bindSSRList = bindSSRList;

function validateDOB(adlt, child, inf) {
  //
  var fnresult = [];
  var total = parseInt(adlt) + parseInt(child) + parseInt(inf);
  var NoofAdlt = parseInt(adlt);
  var init = parseInt(adlt) + 1;
  var initInf = parseInt(adlt) + parseInt(child) + 1;
  var msg = "";
  var MaxC = parseInt(adlt) + parseInt(child);
  var counter = 0;
  for (var t = 1; t <= NoofAdlt; t++) {
    if ($("#txtdate" + t).val() != "") {
      var GetDateval = $("#txtdate" + t).val();
      var res = checkValidDate(GetDateval);
      if (res == false) {
        msg =
          msg +
          "Please fill correct Pax " +
          t +
          " Date of Birth format DD/MM/YYYY.\n";
        counter++;
      }
      if (res == true) {
        var t = getAge(GetDateval);
        if (t < 12) {
          msg =
            msg +
            "Please fill correct Pax " +
            t +
            " Date of Birth your age must be greater than 12 years for travel in this categries.\n";
          counter++;
        }
      }
    }
  }
  for (var i = init; i <= MaxC; i++) {
    if ($("#txtdate" + i).val() == "" || $("#txtdate" + i).val() == undefined) {
      $("#txtdate" + i + "").css({
        "border-color": "#FB0415",
        "border-weight": "1px",
        "border-style": "solid",
      });
      msg = msg + "Please fill the correct " + i + " Date of Birth.\n";
      counter++;
    }
    if ($("#txtdate" + i).val() != "") {
      var GetDateval = $("#txtdate" + i).val();
      var res = checkValidDate(GetDateval);
      if (res == false) {
        msg =
          msg +
          "Please fill the correct " +
          i +
          " Date of Birth format DD/MM/YYYY.\n";
        counter++;
      }
      if (res == true) {
        var t = getAge(GetDateval);
        if (t >= 2 && t < 12) {
          $("#txtdate" + i + "").css({
            "border-color": "#fff",
            "border-weight": "1px",
            "border-style": "solid",
          });
        } else {
          $("#txtdate" + i + "").css({
            "border-color": "#FB0415",
            "border-weight": "1px",
            "border-style": "solid",
          });
          msg =
            msg +
            "Please fill the correct " +
            i +
            " Date of Birth format DD/MM/YYYY.\n";
          counter++;
        }
      }
    }
  }
  for (var j = initInf; j <= total; j++) {
    if ($("#txtdate" + j).val() == "" || $("#txtdate" + j).val() == undefined) {
      $("#txtdate" + j + "").css({
        "border-color": "#FB0415",
        "border-weight": "1px",
        "border-style": "solid",
      });
      msg = msg + "Please fill the correct " + j + " Date of Birth.\n";
      counter++;
    }
    if ($("#txtdate" + j).val() != "") {
      var GetDateval = $("#txtdate" + j).val();
      var res = checkValidDate(GetDateval);
      if (res == false) {
        $("#txtdate" + j + "").css({
          "border-color": "#FB0415",
          "border-weight": "1px",
          "border-style": "solid",
        });
        msg =
          msg +
          "Age must be less than 12 years in textbox " +
          j +
          " and Date of Birth format will be DD/MM/YYYY.\n";
        counter++;
      }
      if (res == true) {
        var t = getAge(GetDateval);
        if (t < 2 && t >= 0) {
          $("#txtdate" + j + "").css({
            "border-color": "#fff",
            "border-weight": "1px",
            "border-style": "solid",
          });
        } else {
          $("#txtdate" + j + "").css({
            "border-color": "#FB0415",
            "border-weight": "1px",
            "border-style": "solid",
          });
          msg =
            msg +
            "Age must be with in less than 2 years in textbox " +
            j +
            " and Date of Birth format will be DD/MM/YYYY.\n";
          counter++;
        }
      }
    }
  }
  fnresult.push(counter);
  fnresult.push(msg);
  return fnresult;
}
function AddInFare() {
  var CurrentPriceIn = 0;
  var CurrentPriceOut = 0;
  var cmpid = document.getElementById("cmpid").value;
  // var _id = document.getElementById("SSRListDDL" + _index);
  selectedResultXml = document.getElementById("hdnFlightSelectedXml").value;

  var xmlDataObj = ParseXML(selectedResultXml);
  $xmlDataObj.find("SelectedResponse").each(function () {
    if (cmpid.indexOf("C-") > -1 || cmpid == "") {
      CurrentPriceOut =
        CurrentPriceOut +
        (parseInt($(this).find("TotalFare").text()) -
          parseInt($(this).find("TotalCommission_SA").text()) +
          parseInt($(this).find("TotalTds").text()));
    } else {
      CurrentPriceOut =
        CurrentPriceOut +
        (parseInt($(this).find("TotalFare").text()) -
          parseInt($(this).find("TotalTds").text()));
    }
  });

  /*//commented on   1-Aug2023//CurrentPriceOut = parseInt(xmlDataObj.getElementsByTagName('TotalFare')[0].firstChild.nodeValue) - (parseInt(xmlDataObj.getElementsByTagName('TotalTds')[0].firstChild.nodeValue));
    if (cmpid.indexOf("C-") > -1 || cmpid == "") {
        CurrentPriceOut = parseInt(xmlDataObj.getElementsByTagName('TotalFare')[0].firstChild.nodeValue) - (parseInt(xmlDataObj.getElementsByTagName('TotalCommission_SA')[0].firstChild.nodeValue) + parseInt(xmlDataObj.getElementsByTagName('TotalTds')[0].firstChild.nodeValue));
    }*/
  var Sector =
    xmlDataObj.getElementsByTagName("Sector")[0].firstChild.nodeValue;
  var adlt = xmlDataObj.getElementsByTagName("Adt")[0].firstChild.nodeValue;
  var child = xmlDataObj.getElementsByTagName("Chd")[0].firstChild.nodeValue;
  var totalpax = parseInt(adlt) + parseInt(child);
  InbondResultXml = document.getElementById("hdnFlightSelectedXmlInbond").value;
  var CurrentPriceIB = 0;
  if (selectedResultXml != "") {
    var ModifiedPrice = 0;
    var tempval = [];
    for (var i = 1; i <= totalpax; i++) {
      var _id = document.getElementById("SSRListDDL" + i);
      var _IdBagg = document.getElementById("SSRbaggageDDL" + i);
      if (_id != null) {
        if (_id.value != 0) {
          var innerText = _id.options[_id.selectedIndex].innerHTML;
          var res = innerText.split("--");
          var Mealprice = res[1];
          ModifiedPrice = parseInt(CurrentPriceOut) + parseInt(Mealprice);
          tempval.push(parseInt(Mealprice));
        }
      }
      if (_IdBagg != null) {
        if (_IdBagg.value != 0) {
          var innerText = _IdBagg.options[_IdBagg.selectedIndex].innerHTML;
          var res = innerText.split("--");
          var Baggprice = res[1];
          ModifiedPrice = parseInt(CurrentPriceOut) + parseInt(Baggprice);
          tempval.push(parseInt(Baggprice));
        }
      }
      //If Inbound fare is preset then run this code of block means round trip
      if (InbondResultXml != "") {
        var xmlDataObjIB = ParseXML(InbondResultXml);
        CurrentPriceIB =
          parseInt(
            xmlDataObjIB.getElementsByTagName("TotalFare")[0].firstChild
              .nodeValue
          ) -
          parseInt(
            xmlDataObjIB.getElementsByTagName("TotalTds")[0].firstChild
              .nodeValue
          );

        if (cmpid.indexOf("C-") > -1 || cmpid == "") {
          CurrentPriceIB =
            parseInt(
              xmlDataObjIB.getElementsByTagName("TotalFare")[0].firstChild
                .nodeValue
            ) -
            (parseInt(
              xmlDataObjIB.getElementsByTagName("TotalCommission_SA")[0]
                .firstChild.nodeValue
            ) +
              parseInt(
                xmlDataObjIB.getElementsByTagName("TotalTds")[0].firstChild
                  .nodeValue
              ));
        }

        var _idIB = document.getElementById("SSRListDDL_I" + i);
        var _idBaggIB = document.getElementById("SSRbaggageDDL_I" + i);
        if (_idIB != null) {
          if (_idIB.value != 0) {
            var innerTextIB = _idIB.options[_idIB.selectedIndex].innerHTML;
            var resIB = innerTextIB.split("--");
            var MealpriceIB = resIB[1];
            ModifiedPrice = parseInt(CurrentPriceIB) + parseInt(MealpriceIB);
            tempval.push(parseInt(MealpriceIB));
          }
        }
        if (_idBaggIB != null) {
          if (_idBaggIB.value != 0) {
            var innerTextIB =
              _idBaggIB.options[_idBaggIB.selectedIndex].innerHTML;
            var resIB = innerTextIB.split("--");
            var BaggpriceIB = resIB[1];
            ModifiedPrice = parseInt(CurrentPriceIB) + parseInt(BaggpriceIB);
            tempval.push(parseInt(BaggpriceIB));
          }
        }
      }
    }
    var totalMealCost = 0;
    for (var i = 0; i < tempval.length; i++) {
      totalMealCost += tempval[i] << 0;
    }
    var totalPrice = parseInt(CurrentPriceOut) + totalMealCost;
    $("#veryfyAmount").html(totalPrice);
    $("#reviewFaretxt").html(totalPrice);
    if (Sector == "D") {
      if (document.getElementById("hidenRT").value == "RT") {
        var totalPrice = parseInt(CurrentPriceOut) + totalMealCost;
        $("#veryfyAmount").html(totalPrice);
        $("#reviewFaretxt").html(totalPrice);
      } else {
        var totalPrice =
          parseInt(CurrentPriceOut) + parseInt(CurrentPriceIB) + totalMealCost;
        $("#veryfyAmount").html(totalPrice);
        $("#reviewFaretxt").html(totalPrice);
      }
    }
    $("#HiddenTotalfare").val($("#reviewFaretxt").html());
  }
}

function AllowAlphabet1(e) {
  keyEntry = e.keyCode ? e.keyCode : e.which;
  if (
    (keyEntry >= "65" && keyEntry <= "90") ||
    (keyEntry >= "97" && keyEntry <= "122") ||
    keyEntry == "46" ||
    keyEntry == "8" ||
    keyEntry == "9" ||
    keyEntry != 32
  )
    return true;
  else return false;
}
window.AllowAlphabet1 = AllowAlphabet1;
function AllowAlphabet(e) {
  keyEntry = e.keyCode ? e.keyCode : e.which;
  if (
    (keyEntry >= "65" && keyEntry <= "90") ||
    (keyEntry >= "97" && keyEntry <= "122") ||
    keyEntry == "46" ||
    keyEntry == "8" ||
    keyEntry == "9" ||
    keyEntry == 32
  )
    return true;
  else return false;
}
window.AllowAlphabet = AllowAlphabet;
// Code For finding duplicate value from array Ajay.
function getduplicatevalue(allpax) {
  var arr = allpax;
  var sorted_arr = arr.slice().sort(); // You can define the comparing function here.
  // JS by default uses a crappy string compare.
  // (we use slice to clone the array so the
  // original array won't be modified)
  var msg = "";
  var results = [];
  for (var i = 0; i < arr.length - 1; i++) {
    if (sorted_arr[i + 1] == sorted_arr[i]) {
      results.push(sorted_arr[i]);
    }
  }
  if (results.length > 0) {
    for (var j = 0; j < results.length; j++) {
      msg = msg + " Pax name " + results[j] + " is duplicate.\n";
    }
    msg = msg + " Pax name can't be same with other pax.";
    alert(msg);
    return false;
  } else {
    return true;
  }
}
// Code For finding duplicate value from array Ajay.
//Meal, Baggage,
function showmoredetail(index) {
  var x = document.getElementById("showmore" + index);
  if (x.style.display === "none") {
    x.style.display = "block";
  } else {
    x.style.display = "none";
  }
  bindSSRList(index);
}
window.showmoredetail = showmoredetail;
//Meal, Baggage,
function uncheckallgstdetail(aa) {
  $("#Showgstdiv").find("input:text").val("");
}
