function show(abc) {

    var tabledatalength = parseInt(document.getElementById("ContentPlaceHolder1_tblcount").value);
    for (var i = 0; i < tabledatalength; i++) {
        var x = document.getElementById("Detaildiv_" + i);
        if (x.style.display === "block") {
            x.style.display = "none";
        }
    }
    var ll = document.getElementById(abc.id);
    if (ll.style.display != "none") {
        ll.style.display = "none";
    }
    else {
        ll.style.display = "block";
    }
}
function showdetail(abc) {
    var ll = document.getElementById(abc.id);
    if ($(ll).is(':visible')) {
        $(ll).hide();
    }
    else {
        $(ll).modal();
    }
}
function hidedetaildiv(abc) {

    var selecteddiv = document.getElementById(abc.id);
    if (selecteddiv.style.display === "block") {
        selecteddiv.style.display = "none";
    }
}
function ParseXML(val) {
    if (window.DOMParser) {
        parser = new DOMParser();
        xmlDoc = parser.parseFromString(val, "text/xml");
    }
    else // Internet Explorer
    {
        xmlDoc = new ActiveXObject("Microsoft.XMLDOM"); xmlDoc.loadXML(val);
    }
    return xmlDoc;
}
function Cancelticket(abc) {
    var result = abc.accessKey;
    // location.href = "Print_Cancel_Req.aspx?BookingRef=" + btoa(result) + "&UpdateId=CallCenter"
    $('#printcanceldata').empty();
    $("#ContentPlaceHolder1_hdnbookref").val('');
    $("#ContentPlaceHolder1_hdndiffrentPnr").val('');
    $("#ContentPlaceHolder1_hdncarriercodeOB").val('');
    $("#ContentPlaceHolder1_hdnbookref").val(result);

    try {
        ucpol = $.ajax({
            url: 'Ticket_Report.aspx/PrintCancelPopup',
            data: "{ 'BookingRef': '" + result + "'}",
            dataType: "json",
            type: "POST",
            contentType: "application/json; charset=utf-8",
            success: function (response) {
                var totaldata = JSON.parse(ucpol.responseText).d;
                if (totaldata != "") {
                    
                    var xmlDataObj = ParseXML(totaldata);
                    Trip = xmlDataObj.getElementsByTagName('Trip')[0].firstChild.nodeValue;
                    var Adt = xmlDataObj.getElementsByTagName('Adt')[0].firstChild.nodeValue;
                    var Chd = xmlDataObj.getElementsByTagName('Chd')[0].firstChild.nodeValue;
                    var Inf = xmlDataObj.getElementsByTagName('Inf')[0].firstChild.nodeValue;
                    var TotalPax = parseInt(Adt) + parseInt(Chd) + parseInt(Inf);
                    var PnrOB = "";
                    var PnrIB = "";
                    var origin = "";
                    var Destination = "";
                    var journydateOB = "";
                    var journydateIB = "";
                    var strhtml = "";
                    var carriercodeOB = "";
                    var carrierNameOB = "";
                    var carriercodeIB = "";
                    var carrierNameIB = "";
                    var paxCount = 0;
                    var segCount = 0;
                    var IsLcc = false;
                    var PaxCounter = 0;
                    var counter = 0;
                    var obcancle = 0;
                    var ibcancle = 0;
                    strhtml = "<div>";
                    if (xmlDataObj.getElementsByTagName("Flight_Detail")['0'].getElementsByTagName("Origin")['0'].childNodes[0]) {
                        origin = xmlDataObj.getElementsByTagName("Flight_Detail")['0'].getElementsByTagName("Origin")['0'].textContent
                    }
                    if (xmlDataObj.getElementsByTagName("Flight_Detail")['0'].getElementsByTagName("Destination")['0'].childNodes[0]) {
                        Destination = xmlDataObj.getElementsByTagName("Flight_Detail")['0'].getElementsByTagName("Destination")['0'].textContent
                    }
                    if (xmlDataObj.getElementsByTagName("Flight_Detail")['0'].getElementsByTagName("DepartureDate_D")['0'].childNodes[0]) {
                        journydateOB = xmlDataObj.getElementsByTagName("Flight_Detail")['0'].getElementsByTagName("DepartureDate_D")['0'].textContent
                    }
                    if (xmlDataObj.getElementsByTagName("Flight_Detail")['0'].getElementsByTagName("Airline_PNR_D")['0'].childNodes[0]) {
                        PnrOB = xmlDataObj.getElementsByTagName("Flight_Detail")['0'].getElementsByTagName("Airline_PNR_D")['0'].textContent
                    }
                    if (xmlDataObj.getElementsByTagName("Flight_Detail")['0'].getElementsByTagName("CarrierCode_D")['0'].childNodes[0]) {
                        carriercodeOB = xmlDataObj.getElementsByTagName("Flight_Detail")['0'].getElementsByTagName("CarrierCode_D")['0'].textContent
                    }
                    if (xmlDataObj.getElementsByTagName("Flight_Detail")['0'].getElementsByTagName("CarrierName_D")['0'].childNodes[0]) {
                        carrierNameOB = xmlDataObj.getElementsByTagName("Flight_Detail")['0'].getElementsByTagName("CarrierName_D")['0'].textContent
                    }
                    strhtml += "<div class='dtl_left'>";
                    strhtml += "<div class='modelpophead'>";
                    strhtml += "<span>Change Request ( <tt id='selectedPNR' class='bold'>";
                    if (Trip == "O" || Trip == "M") {
                        strhtml += "<b>PNR :</b>" + carriercodeOB + " - <tt id=''>" + PnrOB + "</tt></tt>)</span>  <button type='button' class='close' data-dismiss='modal' style='left: 95%;  top: 2%; position:absolute' >&times;</button>";
                    }
                    else {
                        if (xmlDataObj.getElementsByTagName("Flight_Detail")['0'].getElementsByTagName("DepartureDate_A")['0'].childNodes[0]) {
                            journydateIB = xmlDataObj.getElementsByTagName("Flight_Detail")['0'].getElementsByTagName("DepartureDate_A")['0'].textContent
                        }
                        if (xmlDataObj.getElementsByTagName("Flight_Detail")['0'].getElementsByTagName("Airline_PNR_A")['0'].childNodes[0]) {
                            PnrIB = xmlDataObj.getElementsByTagName("Flight_Detail")['0'].getElementsByTagName("Airline_PNR_A")['0'].textContent
                        }
                        if (xmlDataObj.getElementsByTagName("Flight_Detail")['0'].getElementsByTagName("CarrierCode_A")['0'].childNodes[0]) {
                            carriercodeIB = xmlDataObj.getElementsByTagName("Flight_Detail")['0'].getElementsByTagName("CarrierCode_A")['0'].textContent
                        }
                        if (xmlDataObj.getElementsByTagName("Flight_Detail")['0'].getElementsByTagName("CarrierName_A")['0'].childNodes[0]) {
                            carrierNameIB = xmlDataObj.getElementsByTagName("Flight_Detail")['0'].getElementsByTagName("CarrierName_A")['0'].textContent
                        }
                        if (PnrOB == PnrIB) {
                            $("#ContentPlaceHolder1_hdncarriercodeOB").val(carriercodeOB);
                            strhtml += "<b>PNR :</b>" + carriercodeOB + " - <tt id=''>" + PnrOB + "</tt></tt>)</span>  <button type='button' class='close' data-dismiss='modal' style='left: 91%; top: 0%; position:absolute' >&times;</button>";
                        }
                        else {
                            var diffPNR = PnrOB + "," + PnrIB;
                            $("#ContentPlaceHolder1_hdndiffrentPnr").val(diffPNR);
                            strhtml += "<b>PNR :</b>" + carriercodeOB + " - <tt id=''>" + PnrOB + "</tt> | <b>PNR :</b>" + carriercodeIB + " - <tt id=''>" + PnrIB + "</tt></tt>)</span>  <button type='button' class='close' data-dismiss='modal' style='left: 91%; top: 0%; position:absolute' >&times;</button>";
                        }
                    }
                    strhtml += "</div>";
                    strhtml += "</div>";
                    strhtml += "<div class='request_change'>";
                    strhtml += "<input id='BEStatus' value='True' type='hidden'>";
                    strhtml += "<div class='req_row'>";
                    strhtml += "<div id='errMsgRequestType' class='red font10'></div>";
                    strhtml += "<select id='requestType' class='form-control col-md-6'><option value='Select'>-Select-</option><option value='FullCancellation'>Refund</option><option value='PartialCancellation'>Partial Refund</option><option value='FlightCancelled'>Flight Cancelled</option></select>";
                    strhtml += "</div>";
                    strhtml += "<div class='req_row' id='SectorInfo'>";
                    strhtml += "<span class='bold'>Please select Refund Sectors</span><br/>";

                    //<option value='Reissuance'>Change Itinerary / Reissue</option>

                    //strhtml += "<span><input id='sector_0' value='All' name='sectorsSelectors' type='checkbox'>All</span><br/>";

                    //if (Trip == "O") {
                    //    strhtml += "<span><input id='sector_1' name='sectorsSelectors' value='" + origin + "-" + Destination + "' type='checkbox'>" + origin + "-" + Destination + "(<code>" + journydateOB + "</code>)</span>";
                    //}
                    //else {
                    //    if (PnrOB.trim().toUpperCase() == PnrIB.trim().toUpperCase()) {
                    //        if ((carriercodeOB.toUpperCase().indexOf('SG') != -1) || (carriercodeOB.toUpperCase().indexOf('6E') != -1) || (carriercodeOB.toUpperCase().indexOf('G8') != -1) || (carriercodeOB.toUpperCase().indexOf('I5') != -1) || (carriercodeOB.toUpperCase().indexOf('IX') != -1) || (carriercodeOB.toUpperCase().indexOf('LB') != -1) || (carriercodeOB.toUpperCase().indexOf('D7') != -1) || (carriercodeOB.toUpperCase().indexOf('FD') != -1) || (carriercodeOB.toUpperCase().indexOf('AK') != -1) || (carriercodeOB.toUpperCase().indexOf('FZ') != -1)) {
                    //            strhtml += "<span><input id='sector_1' name='sectorsSelectors' value='" + origin + "-" + Destination + "' type='checkbox'>" + origin + "-" + Destination + "(<code>" + journydateOB + "</code>)</span>";
                    //            strhtml += "<br/><span><input id='sector_2' name='sectorsSelectors' value='" + Destination + "-" + origin + "' type='checkbox'>" + Destination + "-" + origin + "(<code>" + journydateIB + "</code>)</span>";
                    //        }
                    //        else {
                    //            strhtml += "<span><input id='sector_1' name='sectorsSelectors' value='" + origin + "-" + Destination + "-" + origin + "' type='checkbox'>" + origin + "-" + Destination + "-" + origin + "(<code>" + journydateOB + "</code>)(<code>" + journydateIB + "</code>)</span>";
                    //        }

                    //    }
                    //    else {
                    //        strhtml += "<span><input id='sector_1' name='sectorsSelectors' value='" + origin + "-" + Destination + "' type='checkbox'>" + origin + "-" + Destination + "(<code>" + journydateOB + "</code>)</span>";
                    //        strhtml += "<br/><span><input id='sector_2' name='sectorsSelectors' value='" + Destination + "-" + origin + "' type='checkbox'>" + Destination + "-" + origin + "(<code>" + journydateIB + "</code>)</span>";
                    //    }

                    //}
                    for (var i = 0; i < TotalPax; i++) {
                        if (xmlDataObj.getElementsByTagName('Outbound')[i].childNodes[0]) {
                            Pax_OBCancel = xmlDataObj.getElementsByTagName('Outbound')[i].firstChild.nodeValue;
                            if (Pax_OBCancel == "1") {
                                obcancle = obcancle + 1;
                            }
                        }
                        if (xmlDataObj.getElementsByTagName('Inbound')[i].childNodes[0]) {
                            Pax_IBCancel = xmlDataObj.getElementsByTagName('Inbound')[i].firstChild.nodeValue;
                            if (Pax_IBCancel == "1") {
                                ibcancle = ibcancle + 1;
                            }
                        }
                    }
                    if (Trip == "O" || Trip == "M") {
                        if (TotalPax == obcancle) {
                            strhtml += "<span id='sector_1'>" + origin + "-" + Destination + "</span>(<code>" + journydateOB + "</code>)";
                        }
                        else {
                            if (Trip == "O" || Trip == "M") {
                                strhtml += "<span><input id='sector_1' name='sectorsSelectors' value='" + origin + "-" + Destination + "' type='checkbox'>" + origin + "-" + Destination + "(<code>" + journydateOB + "</code>)</span>";
                            }
                            else {
                                if ((carriercodeOB.toUpperCase().indexOf('SG') != -1) || (carriercodeOB.toUpperCase().indexOf('6E') != -1) || (carriercodeOB.toUpperCase().indexOf('G8') != -1) || (carriercodeOB.toUpperCase().indexOf('I5') != -1) || (carriercodeOB.toUpperCase().indexOf('IX') != -1) || (carriercodeOB.toUpperCase().indexOf('LB') != -1) || (carriercodeOB.toUpperCase().indexOf('D7') != -1) || (carriercodeOB.toUpperCase().indexOf('FD') != -1) || (carriercodeOB.toUpperCase().indexOf('AK') != -1) || (carriercodeOB.toUpperCase().indexOf('FZ') != -1)) {

                                }
                                else {
                                    strhtml += "<span><input id='sector_1' name='sectorsSelectors' value='" + origin + "-" + Destination + "' type='checkbox'>" + origin + "-" + Destination + "(<code>" + journydateOB + "</code>)</span>";
                                }
                            }
                        }
                    }
                    else {
                        if (PnrOB.trim().toUpperCase() == PnrIB.trim().toUpperCase()) {
                            if ((carriercodeOB.toUpperCase().indexOf('SG') != -1) || (carriercodeOB.toUpperCase().indexOf('6E') != -1) || (carriercodeOB.toUpperCase().indexOf('G8') != -1) || (carriercodeOB.toUpperCase().indexOf('I5') != -1) || (carriercodeOB.toUpperCase().indexOf('IX') != -1) || (carriercodeOB.toUpperCase().indexOf('LB') != -1) || (carriercodeOB.toUpperCase().indexOf('D7') != -1) || (carriercodeOB.toUpperCase().indexOf('FD') != -1) || (carriercodeOB.toUpperCase().indexOf('AK') != -1) || (carriercodeOB.toUpperCase().indexOf('FZ') != -1)) {
                                strhtml += "<div class='col-md-12'>";
                                if (TotalPax == obcancle) {
                                    strhtml += "<div class='col-md-6'><span id='sector_1'>" + origin + "-" + Destination + "</span>(<code>" + journydateOB + "</code>)</div>";
                                    strhtml += "<div class='col-md-6'><input id='sector_2' name='sectorsSelectors' value='" + Destination + "-" + origin + "' type='checkbox'>" + Destination + "-" + origin + "(<code>" + journydateIB + "</code>)</div>";
                                }
                                else if (TotalPax == ibcancle) {
                                    strhtml += "<div class='col-md-6'><input id='sector_1' name='sectorsSelectors' value='" + origin + "-" + Destination + "' type='checkbox'>" + origin + "-" + Destination + "(<code>" + journydateOB + "</code>)</div>";
                                    strhtml += "<div class='col-md-6'><span id='sector_2'>" + Destination + "-" + origin + "</span>(<code>" + journydateIB + "</code>)</div>";
                                }
                                else {
                                    strhtml += "<div class='col-md-6'><input id='sector_1' name='sectorsSelectors' value='" + origin + "-" + Destination + "' type='checkbox'>" + origin + "-" + Destination + "(<code>" + journydateOB + "</code>)</div>";
                                    strhtml += "<div class='col-md-6'><input id='sector_2' name='sectorsSelectors' value='" + Destination + "-" + origin + "' type='checkbox'>" + Destination + "-" + origin + "(<code>" + journydateIB + "</code>)</div>";
                                }
                                strhtml += "</div>";
                            }
                            else {
                                if (TotalPax == obcancle) {
                                    strhtml += "<span id='sector_1'>" + origin + "-" + Destination + "-" + origin + "</span>(<code>" + journydateOB + "</code>)</span>";
                                } else {
                                    strhtml += "<span><input id='sector_1' name='sectorsSelectors' value='" + origin + "-" + Destination + "-" + origin + "' type='checkbox'>" + origin + "-" + Destination + "-" + origin + "(<code>" + journydateOB + "</code>)(<code>" + journydateIB + "</code>)</span>";
                                }
                            }
                        }
                        else {
                            
                            strhtml += "<div class='col-md-12'>";
                            if (TotalPax == obcancle) {
                                strhtml += "<div class='col-md-6'><span id='sector_1'>" + origin + "-" + Destination + "</span>(<code>" + journydateOB + "</code>)</div>";
                                strhtml += "<div class='col-md-6'><input id='sector_2' name='sectorsSelectors' value='" + Destination + "-" + origin + "' type='checkbox'>" + Destination + "-" + origin + "(<code>" + journydateIB + "</code>)</div>";
                            }
                            else if (TotalPax == ibcancle) {
                                strhtml += "<div class='col-md-6'><input id='sector_1' name='sectorsSelectors' value='" + origin + "-" + Destination + "' type='checkbox'>" + origin + "-" + Destination + "(<code>" + journydateOB + "</code>)</div>";
                                strhtml += "<div class='col-md-6'><span id='sector_2'>" + Destination + "-" + origin + "</span>(<code>" + journydateIB + "</code>)</div>";
                            }
                            else {
                                strhtml += "<div class='col-md-6'><input id='sector_1' name='sectorsSelectors' value='" + origin + "-" + Destination + "' type='checkbox'>" + origin + "-" + Destination + "(<code>" + journydateOB + "</code>)</div>";
                                strhtml += "<div class='col-md-6'><input id='sector_2' name='sectorsSelectors' value='" + Destination + "-" + origin + "' type='checkbox'>" + Destination + "-" + origin + "(<code>" + journydateIB + "</code>)</div>";
                            }
                            strhtml += "</div>";
                        }
                    }
                    strhtml += "</div>";
                    strhtml += "<div id='secErrmsg' class='red font10'>";
                    strhtml += "</div>";

                    strhtml += "<div class='req_row'>";
                    strhtml += "<div class='width_100 fleft' id='PassengerInfo'><span class='bold'>Please select Passenger</span>";

                    var Pax_Details = xmlDataObj.getElementsByTagName("Pax_Detail");
                    for (var i = 0; i < TotalPax; i++) {
                        PaxCounter = PaxCounter + 1;
                        paxCount = paxCount + 1;
                        var Title = xmlDataObj.getElementsByTagName('Title')[i].firstChild.nodeValue;
                        var First_Name = xmlDataObj.getElementsByTagName('First_Name')[i].firstChild.nodeValue;
                        var Middle_Name = "";
                        var Last_Name = "";
                        var Pax_SegmentID = "";
                        var Pax_OBCancel = "";
                        var Pax_IBCancel = "";
                        if (xmlDataObj.getElementsByTagName('Middle_Name')[i].childNodes[0]) {
                            Middle_Name = xmlDataObj.getElementsByTagName('Middle_Name')[i].firstChild.nodeValue;
                        }
                        if (xmlDataObj.getElementsByTagName('Last_Name')[i].childNodes[0]) {
                            Last_Name = xmlDataObj.getElementsByTagName('Last_Name')[i].firstChild.nodeValue;
                        }
                        var completeName = Title + " " + First_Name + " " + Middle_Name + " " + Last_Name;

                        if (xmlDataObj.getElementsByTagName('Pax_SegmentID')[i].childNodes[0]) {
                            Pax_SegmentID = xmlDataObj.getElementsByTagName('Pax_SegmentID')[i].firstChild.nodeValue;
                        }
                        if (xmlDataObj.getElementsByTagName('Outbound')[i].childNodes[0]) {
                            Pax_OBCancel = xmlDataObj.getElementsByTagName('Outbound')[i].firstChild.nodeValue;
                        }
                        if (xmlDataObj.getElementsByTagName('Inbound')[i].childNodes[0]) {
                            Pax_IBCancel = xmlDataObj.getElementsByTagName('Inbound')[i].firstChild.nodeValue;
                        }
                        var PaxType = "";
                        if (xmlDataObj.getElementsByTagName("Pax_Detail")[i].getElementsByTagName("PaxType")[0].childNodes[0]) {
                            PaxType = xmlDataObj.getElementsByTagName("Pax_Detail")[i].getElementsByTagName("PaxType")[0].textContent
                        }
                        var sno = i + 1;
                        if (Trip == "R") {
                            if (PnrOB.trim().toUpperCase() == PnrIB.trim().toUpperCase()) {
                                if ((carriercodeOB.toUpperCase().indexOf('SG') != -1) || (carriercodeOB.toUpperCase().indexOf('6E') != -1) || (carriercodeOB.toUpperCase().indexOf('G8') != -1) || (carriercodeOB.toUpperCase().indexOf('I5') != -1) || (carriercodeOB.toUpperCase().indexOf('IX') != -1) || (carriercodeOB.toUpperCase().indexOf('LB') != -1) || (carriercodeOB.toUpperCase().indexOf('D7') != -1) || (carriercodeOB.toUpperCase().indexOf('FD') != -1) || (carriercodeOB.toUpperCase().indexOf('AK') != -1) || (carriercodeOB.toUpperCase().indexOf('FZ') != -1)) {
                                    if (Pax_OBCancel == "1" && Pax_IBCancel != "1") {
                                        strhtml += "<div class='col-md-12'>";
                                        strhtml += "<div class='col-md-6'><span class='paxcancle OBPaxchkbox' style='color:red;' >" + sno + ".  " + completeName + " (CAN)</span> </div>";
                                        strhtml += "<div class='col-md-6'><input class='IBPaxchkbox' name='PaxDetails' id='PaxDetailsIB_" + i + "' value='" + Pax_SegmentID + "' type='checkbox'/>" + sno + ".  " + completeName + " </div></div>";

                                    }
                                    else if (Pax_OBCancel != "1" && Pax_IBCancel == "1") {
                                        strhtml += "<div class='col-md-12'>";
                                        strhtml += "<div class='col-md-6'><input class='OBPaxchkbox' name='PaxDetails' id='PaxDetails_" + i + "' value='" + Pax_SegmentID + "' type='checkbox' />" + sno + ".  " + completeName + " </div>";
                                        strhtml += "<div class='col-md-6'><span class='paxcancle IBPaxchkbox' style='color:red;' >" + sno + ".  " + completeName + " (CAN) </span>  </div></div>";

                                    }
                                    else if (Pax_OBCancel == "1" && Pax_IBCancel == "1") {
                                        strhtml += "<div class='col-md-12'>";
                                        strhtml += "<div class='col-md-6'><span class='paxcancle OBPaxchkbox' style='color:red;' >" + sno + ".  " + completeName + " (CAN)</span>  </div>";
                                        strhtml += "<div class='col-md-6'><span class='paxcancle IBPaxchkbox' style='color:red;' >" + sno + ".  " + completeName + " (CAN)</span></div>";


                                    }
                                    else {
                                        strhtml += "<div class='col-md-12'>";
                                        strhtml += "<div class='col-md-6'><input class='OBPaxchkbox' name='PaxDetails' id='PaxDetails_" + i + "' value='" + Pax_SegmentID + "' type='checkbox'/>" + sno + ".  " + completeName + " </div>";
                                        strhtml += "<div class='col-md-6'><input class='IBPaxchkbox' name='PaxDetails' id='PaxDetailsIB_" + i + "' value='" + Pax_SegmentID + "' type='checkbox'/>" + sno + ".  " + completeName + " </div></div>";
                                    }
                                    counter += 1;
                                }
                                else {
                                    if (Pax_OBCancel == "1") {
                                        strhtml += "<br/><span class='paxcancle ' style='color:red;'>" + sno + ".  " + completeName + "(CAN) </span>";

                                    }
                                    else {
                                        strhtml += "<br/><span><input name='PaxDetails' id='PaxDetails_" + i + "' value='" + Pax_SegmentID + "' type='checkbox'/>" + sno + ".  " + completeName + " </span>";
                                    }
                                    counter += 1;
                                }

                            }
                            else {
                                
                                if (Pax_OBCancel == "1" && Pax_IBCancel != "1") {
                                    strhtml += "<div class='col-md-12'>";
                                    strhtml += "<div class='col-md-6'><span class='paxcancle OBPaxchkbox' style='color:red;'>" + sno + ".  " + completeName + " (CAN)</span> </div>";
                                    strhtml += "<div class='col-md-6'><input class='IBPaxchkbox' name='PaxDetails' id='PaxDetailsIB_" + i + "' value='" + Pax_SegmentID + "' type='checkbox'/>" + sno + ".  " + completeName + " </div></div>";

                                }
                                else if (Pax_OBCancel != "1" && Pax_IBCancel == "1") {
                                    strhtml += "<div class='col-md-12'>";
                                    strhtml += "<div class='col-md-6'><input class='OBPaxchkbox' name='PaxDetails' id='PaxDetails_" + i + "' value='" + Pax_SegmentID + "' type='checkbox' />" + sno + ".  " + completeName + " </div>";
                                    strhtml += "<div class='col-md-6'><span class='paxcancle IBPaxchkbox' style='color:red;'>" + sno + ".  " + completeName + " (CAN)</span> </div></div>";

                                }
                                else if (Pax_OBCancel == "1" && Pax_IBCancel == "1") {
                                    strhtml += "<div class='col-md-12'>";
                                    strhtml += "<div class='col-md-6'><span class='paxcancle OBPaxchkbox' style='color:red;'>" + sno + ".  " + completeName + " (CAN)</span>  </div>";
                                    strhtml += "<div class='col-md-6'><span class='paxcancle IBPaxchkbox' style='color:red;'>" + sno + ".  " + completeName + " (CAN)</span> </div></div>";

                                }
                                else {
                                    strhtml += "<div class='col-md-12'>";
                                    strhtml += "<div class='col-md-6'><input class='OBPaxchkbox' name='PaxDetails' id='PaxDetails_" + i + "' value='" + Pax_SegmentID + "' type='checkbox'/>" + sno + ".  " + completeName + " </div>";
                                    strhtml += "<div class='col-md-6'><input class='IBPaxchkbox' name='PaxDetails' id='PaxDetailsIB_" + i + "' value='" + Pax_SegmentID + "' type='checkbox'/>" + sno + ".  " + completeName + " </div></div>";
                                }
                                counter += 1;
                            }
                        }
                        else {
                            if (Pax_OBCancel == "1") {
                                strhtml += "<br/><span class='paxcancle' style='color:red;'> " + sno + ".  " + completeName + " (CAN) </span>";

                            }
                            else {
                                strhtml += "<br/><span><input name='PaxDetails' id='PaxDetails_" + i + "' value='" + Pax_SegmentID + "' type='checkbox'/>" + sno + ".  " + completeName + " </span>";
                            }
                            counter += 1;
                        }
                    }
                    strhtml += "</div>";
                    strhtml += "<div id='errMsg' class='red font10'>";
                    strhtml += "</div>";
                    strhtml += "</div>";
                    strhtml += "<div class=' req_row'>";
                    strhtml += "<span class='bold'>Please enter remarks <sup class='red'>*</sup></span>";
                    strhtml += "<textarea rows='3' cols='56' id='txtRemarks'></textarea>";

                    strhtml += "<div id='errMsgRemarks' class='red font10'></div>";
                    strhtml += "</div>";

                    strhtml += "<div class='req_row' id='sendReqMsgs' style='border-bottom: medium none;'>";
                    strhtml += "<span class='bold'>Note.</span>";
                    strhtml += "<ol class='note_req'>";
                    strhtml += "<li>Partial Refund will be processed offline.</li>";
                    strhtml += "<li>In case of Infant booking, cancellation will be processed offline.</li>";
                    strhtml += "<li>In case of One sector to be cancel, please send the offline request.</li>";
                    strhtml += "<li>In case of Flight cancellation/ flight reschedule, please select flight cancelled.</li>";
                    strhtml += "<li>Cancellation Charges cannot be retrieved for Partial Cancelled Booking</li>";
                    strhtml += "</ol>";
                    strhtml += "</div>";
                    strhtml += "<li style='color:red;font-size: 12px;'>*Refund will take minimum 24-72 hour after cancellation.</li>";
                    strhtml += "<li style='color:red;font-size: 12px;'>*if any Dispute or No show refund so it may takes more than 7 days.</li>";
                    strhtml += "</div>";
                    strhtml += "<div class='req_row'></div>";
                    strhtml += "<span class='bold align_center fleft width_100 mt' id='CancellationChargesBlock' style='display:none;'>Total Cancellation Charges: <tt id='CancellationCharges'></tt> <br>Total Refund Amount : <tt id='RefundedAmount'></tt></span>";
                    strhtml += "<span class='bold red font10 font10 align_center fleft width_100 mt' id='errCancellationCharges' style='display: none;'></span>";

                    strhtml += "<span class='req_row_btn' id='btnPanel'>";

                    strhtml += "<div id='loaderCanCharges' class='ml align_center fleft width_100 mt mb' style='display:none'>";
                    strhtml += "<img src='images/loaderNew.gif' alt='loading' style='vertical-align:middle;'> <b class='ml mt'>Loading Cancellation Charges...</b>";
                    strhtml += "<div class='clr'></div>";
                    strhtml += "</div> ";

                    strhtml += "<input class='btn_main_s mr' value='View Cancel. Charges' id='CancellationChargesbtn' style='display:none;' type='button'/>";
                    strhtml += "<input class='btn_main_s mr' value='Send Request' id='btnSendChangeReq' type='button' onclick='SendCancelreq();'/>";
                    strhtml += "<input class='btn_main_s' value='Cancel' id='btnSendChangeReqCancel' type='button' data-dismiss='modal'/>";
                    strhtml += "</span>";

                    strhtml += "<div class='req_row red bold align_center' id='processingMsg' style='display: none;'>Your Request is Processing...</div>";
                    strhtml += "</div>";
                    $('#printcanceldata').append(strhtml);

                    $("#btnSendChangeReqPopup").modal();

                }
            }
        });

    }
    catch (e) { }


}
function SendCancelreq() {
    ticketIds = '';
    ticketIdsdif = '';
    Sector = '';
    $('#errMsgRequestType').text('');
    requestType = $("#requestType option:selected").val();
    if (requestType == "Select") {
        $('#errMsgRequestType').append('Please select request type');
        return;
    }
    if ($('input[name="sectorsSelectors"]:checked').length == 0) {
        $('#secErrmsg').text('Please Select Sector.');
        return;
    }
    else {
        $('#secErrmsg').text('');
    }
    if ($('input[name="PaxDetails"]:checked').length == 0) {
        $('#errMsg').text('Please Select Passenger(s).');
        return;
    }
    else {
        $('#errMsg').text('');
    }

    if ($('#txtRemarks').val().length == 0) {
        $('#errMsgRemarks').text('');
        $('#errMsgRemarks').append('Please enter remark(s).');
        return;
    }
    else {
        $('#errMsgRemarks').html('');
    }
    bookingid = $("#ContentPlaceHolder1_hdnbookref").val();

    $('input:checkbox[name=sectorsSelectors]:checked').each(function () {
        if (Sector == '') {
            Sector = $(this).val();
        }
        else {
            Sector = Sector + ',' + $(this).val();
        }
    });
    if (Trip == "R") {
        if ($("#ContentPlaceHolder1_hdndiffrentPnr").val() != "") {
            if ($("#sector_1").is(":checked") && $("#sector_2").is(":not(:checked)")) {

                $('input:checkbox[class=IBPaxchkbox]:checked').each(function () {
                    $(this).attr('checked', false);
                });
                if ($('input[class="OBPaxchkbox"]:checked').length == 0) {
                    $('#errMsg').text('Please Select Outbound Passenger(s).');
                    return;
                }
                else {
                    $('#errMsg').text('');
                    $(".OBPaxchkbox:checked").each(function () {
                        if (ticketIds == '') {
                            ticketIds = $(this).val();
                        }
                        else {
                            ticketIds = ticketIds + ',' + $(this).val();
                        }
                    });
                }
            }
            else if ($("#sector_2").is(":checked") && $("#sector_1").is(":not(:checked)")) {

                $('input:checkbox[class=OBPaxchkbox]:checked').each(function () {
                    $(this).attr('checked', false);
                });
                if ($('input[class="IBPaxchkbox"]:checked').length == 0) {
                    $('#errMsg').text('Please Select Inbound Passenger(s).');
                    return;
                }
                else {
                    $('#errMsg').text('');
                    $(".IBPaxchkbox:checked").each(function () {
                        if (ticketIdsdif == '') {
                            ticketIdsdif = $(this).val();
                        }
                        else {
                            ticketIdsdif = ticketIdsdif + ',' + $(this).val();
                        }
                    });
                }
            }
            else {

                if ($('input[class="IBPaxchkbox"]:checked').length == 0) {
                    $('#errMsg').text('Please Select Inbound Passenger(s).');
                    return;
                }
                else {
                    $(".IBPaxchkbox:checked").each(function () {
                        if (ticketIdsdif == '') {
                            ticketIdsdif = $(this).val();
                        }
                        else {
                            ticketIdsdif = ticketIdsdif + ',' + $(this).val();
                        }
                    });
                }
                if ($('input[class="OBPaxchkbox"]:checked').length == 0) {
                    $('#errMsg').text('Please Select Outbound Passenger(s).');
                    return;
                }
                else {
                    $(".OBPaxchkbox:checked").each(function () {
                        if (ticketIds == '') {
                            ticketIds = $(this).val();
                        }
                        else {
                            ticketIds = ticketIds + ',' + $(this).val();
                        }
                    });
                }
            }
        }
        else {
            if ($("#ContentPlaceHolder1_hdncarriercodeOB").val() != "") {
                var carriercode = $("#ContentPlaceHolder1_hdncarriercodeOB").val();
                if ((carriercode.toUpperCase().indexOf('SG') != -1) || (carriercode.toUpperCase().indexOf('6E') != -1) || (carriercode.toUpperCase().indexOf('G8') != -1) || (carriercode.toUpperCase().indexOf('I5') != -1) || (carriercode.toUpperCase().indexOf('IX') != -1) || (carriercode.toUpperCase().indexOf('LB') != -1) || (carriercode.toUpperCase().indexOf('D7') != -1) || (carriercode.toUpperCase().indexOf('FD') != -1) || (carriercode.toUpperCase().indexOf('AK') != -1) || (carriercode.toUpperCase().indexOf('FZ') != -1)) {

                    if ($("#sector_1").is(":checked") && $("#sector_2").is(":not(:checked)")) {

                        $('input:checkbox[class=IBPaxchkbox]:checked').each(function () {
                            $(this).attr('checked', false);
                        });
                        if ($('input[class="OBPaxchkbox"]:checked').length == 0) {
                            $('#errMsg').text('Please Select Outbound Passenger(s).');
                            return;
                        }
                        else {
                            $('#errMsg').text('');
                            $(".OBPaxchkbox:checked").each(function () {
                                if (ticketIds == '') {
                                    ticketIds = $(this).val();
                                }
                                else {
                                    ticketIds = ticketIds + ',' + $(this).val();
                                }
                            });
                        }
                    }
                    else if ($("#sector_2").is(":checked") && $("#sector_1").is(":not(:checked)")) {

                        $('input:checkbox[class=OBPaxchkbox]:checked').each(function () {
                            $(this).attr('checked', false);
                        });
                        if ($('input[class="IBPaxchkbox"]:checked').length == 0) {
                            $('#errMsg').text('Please Select Inbound Passenger(s).');
                            return;
                        }
                        else {
                            $('#errMsg').text('');
                            $(".IBPaxchkbox:checked").each(function () {
                                if (ticketIdsdif == '') {
                                    ticketIdsdif = $(this).val();
                                }
                                else {
                                    ticketIdsdif = ticketIdsdif + ',' + $(this).val();
                                }
                            });
                        }
                    }
                    else {

                        if ($('input[class="IBPaxchkbox"]:checked').length == 0) {
                            $('#errMsg').text('Please Select Inbound Passenger(s).');
                            return;
                        }
                        else {
                            $(".IBPaxchkbox:checked").each(function () {
                                if (ticketIdsdif == '') {
                                    ticketIdsdif = $(this).val();
                                }
                                else {
                                    ticketIdsdif = ticketIdsdif + ',' + $(this).val();
                                }
                            });
                        }
                        if ($('input[class="OBPaxchkbox"]:checked').length == 0) {
                            $('#errMsg').text('Please Select Outbound Passenger(s).');
                            return;
                        }
                        else {
                            $(".OBPaxchkbox:checked").each(function () {
                                if (ticketIds == '') {
                                    ticketIds = $(this).val();
                                }
                                else {
                                    ticketIds = ticketIds + ',' + $(this).val();
                                }
                            });
                        }
                    }
                }
                else {
                    $('input:checkbox[name=PaxDetails]:checked').each(function () {
                        if (ticketIds == '') {
                            ticketIds = $(this).val();
                        }
                        else {
                            ticketIds = ticketIds + ',' + $(this).val();
                        }
                    });
                }
            }
        }
    }
    else {
        $('input:checkbox[name=PaxDetails]:checked').each(function () {
            if (ticketIds == '') {
                ticketIds = $(this).val();
            }
            else {
                ticketIds = ticketIds + ',' + $(this).val();
            }
        });
    }
    remarks = $('#txtRemarks').val();
    //$('#btnPanel').hide();
    var chkPaxCount = 0;
    $('input:checkbox[name=PaxDetails]:checked').each(function () {
        chkPaxCount++;
    });
    //if (IsLcc && 'FullCancellation' == requestType && paxCount == chkPaxCount && $('#sector_0').is(':checked')) {
    //    $('#confirmationPopup').show();
    //}
    //else {
    //    $('#processingMsg').show();
    $.ajax({
        url: "Ticket_Report.aspx/SendCancelRequest",
        data: JSON.stringify({ BookingRef: bookingid, CancelType: requestType, PaxSegmentID: ticketIds, Remarks: remarks, Sector: Sector, PaxSegmentID2: ticketIdsdif }),
        type: "POST",
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        dataFilter: function (data) { return data; },
        success: SendChangeRequestResp,
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            //error message
        }
    });
    // }
}
function SendChangeRequestResp(resp) {

    if (resp.d == "1") {
        alert("successfully canceled.!");
        $("#btnSendChangeReqPopup").modal('hide');
    }
    else {
        alert("There is something error.!");
        $("#btnSendChangeReqPopup").modal('hide');
    }
}

//======== payment for hold end
function Payticket(abc) {
    for (let i = 0; i < $(".btn.btn-default.paybtn").length; i++) {
        $(".btn.btn-default.paybtn").eq(i).attr("onclick", "  ");
    }

    //alert('Under construction...  ' + result);
   
        var result = abc.accessKey;
        // location.href = "Print_Cancel_Req.aspx?BookingRef=" + btoa(result) + "&UpdateId=CallCenter"
    $('#printPaymentdata').empty();
        $("#ContentPlaceHolder1_hdnbookref").val('');
        $("#ContentPlaceHolder1_hdndiffrentPnr").val('');
        $("#ContentPlaceHolder1_hdncarriercodeOB").val('');
        $("#ContentPlaceHolder1_hdnbookref").val(result);

        try {
            ucpol = $.ajax({
                url: '/Booking/PrintPayment4HoldBookPopup',
                // data: "{ 'BookingRef: '" + result + "'}",
                // data: JSON.stringify({ "BookingRef": result.toString() }),
                data: JSON.stringify(result),      
                dataType: 'text',
                type: "POST",
                contentType: "application/json; charset=utf-8",
                success: function (response) {
                    // var totaldata = JSON.parse(ucpol.responseText).d;
                    
                    if (response != "") {

                        var xmlDataObj = ParseXML(response);
                        
                        Trip = xmlDataObj.getElementsByTagName('Trip')[0].firstChild.nodeValue;
                        var Adt = xmlDataObj.getElementsByTagName('Adt')[0].firstChild.nodeValue;
                        var Chd = xmlDataObj.getElementsByTagName('Chd')[0].firstChild.nodeValue;
                        var Inf = xmlDataObj.getElementsByTagName('Inf')[0].firstChild.nodeValue;
                        var TotalPax = parseInt(Adt) + parseInt(Chd) + parseInt(Inf);
                        var PnrOB = "";
                        var PnrIB = "";
                        var origin = "";
                        var Destination = "";
                        var journydateOB = "";
                        var journydateIB = "";
                        var strhtml = "";
                        var carriercodeOB = "";
                        var carrierNameOB = "";
                        var carriercodeIB = "";
                        var carrierNameIB = "";
                        var paxCount = 0;
                        var segCount = 0;
                        var IsLcc = false;
                        var PaxCounter = 0;
                        var counter = 0;
                        var obcancle = 0;
                        var ibcancle = 0;
                        strhtml = "<div>";
                        if (xmlDataObj.getElementsByTagName("CompanyFlightDetailAirlines")['0'].getElementsByTagName("Origin")['0'].childNodes[0]) {
                            origin = xmlDataObj.getElementsByTagName("CompanyFlightDetailAirlines")['0'].getElementsByTagName("Origin")['0'].textContent
                        }
                        if (xmlDataObj.getElementsByTagName("CompanyFlightDetailAirlines")['0'].getElementsByTagName("Destination")['0'].childNodes[0]) {
                            Destination = xmlDataObj.getElementsByTagName("CompanyFlightDetailAirlines")['0'].getElementsByTagName("Destination")['0'].textContent
                        }
                        if (xmlDataObj.getElementsByTagName("CompanyFlightDetailAirlines")['0'].getElementsByTagName("DepartureDate_D")['0'].childNodes[0]) {
                            journydateOB = xmlDataObj.getElementsByTagName("CompanyFlightDetailAirlines")['0'].getElementsByTagName("DepartureDate_D")['0'].textContent
                        }
                        
                        if (xmlDataObj.getElementsByTagName("CompanyFlightDetailAirlines")['0'].getElementsByTagName("Airline_Pnr_D")['0'].childNodes[0]) {
                            PnrOB = xmlDataObj.getElementsByTagName("CompanyFlightDetailAirlines")['0'].getElementsByTagName("Airline_Pnr_D")['0'].textContent
                        }
                        if (xmlDataObj.getElementsByTagName("CompanyFlightDetailAirlines")['0'].getElementsByTagName("CarrierCode_D")['0'].childNodes[0]) {
                            carriercodeOB = xmlDataObj.getElementsByTagName("CompanyFlightDetailAirlines")['0'].getElementsByTagName("CarrierCode_D")['0'].textContent
                        }
                        if (xmlDataObj.getElementsByTagName("CompanyFlightSegmentDetailAirlinesData")['0'].getElementsByTagName("CarrierName")['0'].childNodes[0]) {
                            carrierNameOB = xmlDataObj.getElementsByTagName("CompanyFlightSegmentDetailAirlinesData")['0'].getElementsByTagName("CarrierName")['0'].textContent
                        }
                        // if (xmlDataObj.getElementsByTagName("Flight_Detail")['0'].getElementsByTagName("CarrierName_D")['0'].childNodes[0]) {
                        //     carrierNameOB = xmlDataObj.getElementsByTagName("Flight_Detail")['0'].getElementsByTagName("CarrierName_D")['0'].textContent
                        // }
                        strhtml += "<div class='dtl_left'>";
                        strhtml += "<div class='modelpophead'>";
                        strhtml += "<span>Payment Request ( <tt id='selectedPNR' class='bold'>";
                        if (Trip == "O" || Trip == "M") {
                            strhtml += "<b>PNR :</b>" + carriercodeOB + " - <tt id=''>" + PnrOB + "</tt></tt>)</span>  <button type='button' class='close' data-dismiss='modal' style='left: 95%;  top: 2%; position:absolute' >&times;</button>";
                        }
                        else {
                            if (xmlDataObj.getElementsByTagName("CompanyFlightDetailAirlines")['0'].getElementsByTagName("DepartureDate_A")['0'].childNodes[0]) {
                                journydateIB = xmlDataObj.getElementsByTagName("CompanyFlightDetailAirlines")['0'].getElementsByTagName("DepartureDate_A")['0'].textContent
                            }
                            if (xmlDataObj.getElementsByTagName("CompanyFlightDetailAirlines")['0'].getElementsByTagName("Airline_Pnr_A")['0'].childNodes[0]) {
                                PnrIB = xmlDataObj.getElementsByTagName("CompanyFlightDetailAirlines")['0'].getElementsByTagName("Airline_Pnr_A")['0'].textContent
                            }
                            if (xmlDataObj.getElementsByTagName("CompanyFlightDetailAirlines")['0'].getElementsByTagName("CarrierCode_A")['0'].childNodes[0]) {
                                carriercodeIB = xmlDataObj.getElementsByTagName("CompanyFlightDetailAirlines")['0'].getElementsByTagName("CarrierCode_A")['0'].textContent
                            }
                            if (xmlDataObj.getElementsByTagName("CompanyFlightDetailAirlines")['0'].getElementsByTagName("CarrierName_A")['0'].childNodes[0]) {
                                carrierNameIB = xmlDataObj.getElementsByTagName("CompanyFlightDetailAirlines")['0'].getElementsByTagName("CarrierName_A")['0'].textContent
                            }
                            if (PnrOB == PnrIB) {
                                $("#ContentPlaceHolder1_hdncarriercodeOB").val(carriercodeOB);
                                strhtml += "<b>PNR :</b>" + carriercodeOB + " - <tt id=''>" + PnrOB + "</tt></tt>)</span>  <button type='button' class='close' data-dismiss='modal' style='left: 91%; top: 0%; position:absolute' >&times;</button>";
                            }
                            else {
                                var diffPNR = PnrOB + "," + PnrIB;
                                $("#ContentPlaceHolder1_hdndiffrentPnr").val(diffPNR);
                                strhtml += "<b>PNR :</b>" + carriercodeOB + " - <tt id=''>" + PnrOB + "</tt> | <b>PNR :</b>" + carriercodeIB + " - <tt id=''>" + PnrIB + "</tt></tt>)</span>  <button type='button' class='close' data-dismiss='modal' style='left: 91%; top: 0%; position:absolute' >&times;</button>";
                            }
                        }
                        strhtml += "</div>";
                        strhtml += "</div>";
                        strhtml += "<div class='request_change'>";
                        strhtml += "<input id='BEStatus' value='True' type='hidden'>";
                        strhtml += "<div class='req_row'>";
                        strhtml += "<div id='errMsgRequestType' class='red font10'></div>";
                        strhtml += "<select id='requestType' class='form-control col-md-6'><option value='Select'>-Select-</option><option value='Prepaid'>Prepaid</option></select>";
                        strhtml += "</div>";
                        strhtml += "<div class='req_row' id='SectorInfo'>";
                        strhtml += "<span class='bold'>Please select Payment Sectors</span><br/>";

                        //<option value='Reissuance'>Change Itinerary / Reissue</option>

                        //strhtml += "<span><input id='sector_0' value='All' name='sectorsSelectors' type='checkbox'>All</span><br/>";

                        //if (Trip == "O") {
                        //    strhtml += "<span><input id='sector_1' name='sectorsSelectors' value='" + origin + "-" + Destination + "' type='checkbox'>" + origin + "-" + Destination + "(<code>" + journydateOB + "</code>)</span>";
                        //}
                        //else {
                        //    if (PnrOB.trim().toUpperCase() == PnrIB.trim().toUpperCase()) {
                        //        if ((carriercodeOB.toUpperCase().indexOf('SG') != -1) || (carriercodeOB.toUpperCase().indexOf('6E') != -1) || (carriercodeOB.toUpperCase().indexOf('G8') != -1) || (carriercodeOB.toUpperCase().indexOf('I5') != -1) || (carriercodeOB.toUpperCase().indexOf('IX') != -1) || (carriercodeOB.toUpperCase().indexOf('LB') != -1) || (carriercodeOB.toUpperCase().indexOf('D7') != -1) || (carriercodeOB.toUpperCase().indexOf('FD') != -1) || (carriercodeOB.toUpperCase().indexOf('AK') != -1) || (carriercodeOB.toUpperCase().indexOf('FZ') != -1)) {
                        //            strhtml += "<span><input id='sector_1' name='sectorsSelectors' value='" + origin + "-" + Destination + "' type='checkbox'>" + origin + "-" + Destination + "(<code>" + journydateOB + "</code>)</span>";
                        //            strhtml += "<br/><span><input id='sector_2' name='sectorsSelectors' value='" + Destination + "-" + origin + "' type='checkbox'>" + Destination + "-" + origin + "(<code>" + journydateIB + "</code>)</span>";
                        //        }
                        //        else {
                        //            strhtml += "<span><input id='sector_1' name='sectorsSelectors' value='" + origin + "-" + Destination + "-" + origin + "' type='checkbox'>" + origin + "-" + Destination + "-" + origin + "(<code>" + journydateOB + "</code>)(<code>" + journydateIB + "</code>)</span>";
                        //        }

                        //    }
                        //    else {
                        //        strhtml += "<span><input id='sector_1' name='sectorsSelectors' value='" + origin + "-" + Destination + "' type='checkbox'>" + origin + "-" + Destination + "(<code>" + journydateOB + "</code>)</span>";
                        //        strhtml += "<br/><span><input id='sector_2' name='sectorsSelectors' value='" + Destination + "-" + origin + "' type='checkbox'>" + Destination + "-" + origin + "(<code>" + journydateIB + "</code>)</span>";
                        //    }

                        //}
                        for (var i = 0; i < TotalPax; i++) {
                            if (xmlDataObj.getElementsByTagName('Outbound')[i].childNodes[0]) {
                                Pax_OBCancel = xmlDataObj.getElementsByTagName('Outbound')[i].firstChild.nodeValue;
                                if (Pax_OBCancel == "1") {
                                    obcancle = obcancle + 1;
                                }
                            }
                            if (xmlDataObj.getElementsByTagName('Inbound')[i].childNodes[0]) {
                                Pax_IBCancel = xmlDataObj.getElementsByTagName('Inbound')[i].firstChild.nodeValue;
                                if (Pax_IBCancel == "1") {
                                    ibcancle = ibcancle + 1;
                                }
                            }
                        }
                        if (Trip == "O" || Trip == "M") {
                            if (TotalPax == obcancle) {
                                strhtml += "<span id='sector_1'>" + origin + "-" + Destination + "</span>(<code>" + journydateOB + "</code>)";
                            }
                            else {
                                if (Trip == "O" || Trip == "M") {
                                    strhtml += "<span><input id='sector_1' name='sectorsSelectors' value='" + origin + "-" + Destination + "' type='checkbox'>" + origin + "-" + Destination + "(<code>" + journydateOB + "</code>)</span>";
                                }
                                else {
                                    if ((carriercodeOB.toUpperCase().indexOf('SG') != -1) || (carriercodeOB.toUpperCase().indexOf('6E') != -1) || (carriercodeOB.toUpperCase().indexOf('G8') != -1) || (carriercodeOB.toUpperCase().indexOf('I5') != -1) || (carriercodeOB.toUpperCase().indexOf('IX') != -1) || (carriercodeOB.toUpperCase().indexOf('LB') != -1) || (carriercodeOB.toUpperCase().indexOf('D7') != -1) || (carriercodeOB.toUpperCase().indexOf('FD') != -1) || (carriercodeOB.toUpperCase().indexOf('AK') != -1) || (carriercodeOB.toUpperCase().indexOf('FZ') != -1)) {

                                    }
                                    else {
                                        strhtml += "<span><input id='sector_1' name='sectorsSelectors' value='" + origin + "-" + Destination + "' type='checkbox'>" + origin + "-" + Destination + "(<code>" + journydateOB + "</code>)</span>";
                                    }
                                }
                            }
                        }
                        else {
                            if (PnrOB.trim().toUpperCase() == PnrIB.trim().toUpperCase()) {
                                if ((carriercodeOB.toUpperCase().indexOf('SG') != -1) || (carriercodeOB.toUpperCase().indexOf('6E') != -1) || (carriercodeOB.toUpperCase().indexOf('G8') != -1) || (carriercodeOB.toUpperCase().indexOf('I5') != -1) || (carriercodeOB.toUpperCase().indexOf('IX') != -1) || (carriercodeOB.toUpperCase().indexOf('LB') != -1) || (carriercodeOB.toUpperCase().indexOf('D7') != -1) || (carriercodeOB.toUpperCase().indexOf('FD') != -1) || (carriercodeOB.toUpperCase().indexOf('AK') != -1) || (carriercodeOB.toUpperCase().indexOf('FZ') != -1)) {
                                    strhtml += "<div class='col-md-12'>";
                                    if (TotalPax == obcancle) {
                                        strhtml += "<div class='col-md-6'><span id='sector_1'>" + origin + "-" + Destination + "</span>(<code>" + journydateOB + "</code>)</div>";
                                        strhtml += "<div class='col-md-6'><input id='sector_2' name='sectorsSelectors' value='" + Destination + "-" + origin + "' type='checkbox'>" + Destination + "-" + origin + "(<code>" + journydateIB + "</code>)</div>";
                                    }
                                    else if (TotalPax == ibcancle) {
                                        strhtml += "<div class='col-md-6'><input id='sector_1' name='sectorsSelectors' value='" + origin + "-" + Destination + "' type='checkbox'>" + origin + "-" + Destination + "(<code>" + journydateOB + "</code>)</div>";
                                        strhtml += "<div class='col-md-6'><span id='sector_2'>" + Destination + "-" + origin + "</span>(<code>" + journydateIB + "</code>)</div>";
                                    }
                                    else {
                                        strhtml += "<div class='col-md-6'><input id='sector_1' name='sectorsSelectors' value='" + origin + "-" + Destination + "' type='checkbox'>" + origin + "-" + Destination + "(<code>" + journydateOB + "</code>)</div>";
                                        strhtml += "<div class='col-md-6'><input id='sector_2' name='sectorsSelectors' value='" + Destination + "-" + origin + "' type='checkbox'>" + Destination + "-" + origin + "(<code>" + journydateIB + "</code>)</div>";
                                    }
                                    strhtml += "</div>";
                                }
                                else {
                                    if (TotalPax == obcancle) {
                                        strhtml += "<span id='sector_1'>" + origin + "-" + Destination + "-" + origin + "</span>(<code>" + journydateOB + "</code>)</span>";
                                    } else {
                                        strhtml += "<span><input id='sector_1' name='sectorsSelectors' value='" + origin + "-" + Destination + "-" + origin + "' type='checkbox'>" + origin + "-" + Destination + "-" + origin + "(<code>" + journydateOB + "</code>)(<code>" + journydateIB + "</code>)</span>";
                                    }
                                }
                            }
                            else {
                                
                                strhtml += "<div class='col-md-12'>";
                                if (TotalPax == obcancle) {
                                    strhtml += "<div class='col-md-6'><span id='sector_1'>" + origin + "-" + Destination + "</span>(<code>" + journydateOB + "</code>)</div>";
                                    strhtml += "<div class='col-md-6'><input id='sector_2' name='sectorsSelectors' value='" + Destination + "-" + origin + "' type='checkbox'>" + Destination + "-" + origin + "(<code>" + journydateIB + "</code>)</div>";
                                }
                                else if (TotalPax == ibcancle) {
                                    strhtml += "<div class='col-md-6'><input id='sector_1' name='sectorsSelectors' value='" + origin + "-" + Destination + "' type='checkbox'>" + origin + "-" + Destination + "(<code>" + journydateOB + "</code>)</div>";
                                    strhtml += "<div class='col-md-6'><span id='sector_2'>" + Destination + "-" + origin + "</span>(<code>" + journydateIB + "</code>)</div>";
                                }
                                else {
                                    strhtml += "<div class='col-md-6'><input id='sector_1' name='sectorsSelectors' value='" + origin + "-" + Destination + "' type='checkbox'>" + origin + "-" + Destination + "(<code>" + journydateOB + "</code>)</div>";
                                    strhtml += "<div class='col-md-6'><input id='sector_2' name='sectorsSelectors' value='" + Destination + "-" + origin + "' type='checkbox'>" + Destination + "-" + origin + "(<code>" + journydateIB + "</code>)</div>";
                                }
                                strhtml += "</div>";
                            }
                        }
                        strhtml += "</div>";
                        strhtml += "<div id='secErrmsg' class='red font10'>";
                        strhtml += "</div>";

                        strhtml += "<div class='req_row'>";
                        strhtml += "<div class='width_100 fleft' id='PassengerInfo'><span class='bold'>Please select Passenger</span>";

                        var Pax_Details = xmlDataObj.getElementsByTagName("Pax_Detail");
                        for (var i = 0; i < TotalPax; i++) {
                            PaxCounter = PaxCounter + 1;
                            paxCount = paxCount + 1;
                            var Title = xmlDataObj.getElementsByTagName('Title')[i].firstChild.nodeValue;
                            var First_Name = xmlDataObj.getElementsByTagName('First_Name')[i].firstChild.nodeValue;
                            var Middle_Name = "";
                            var Last_Name = "";
                            var Pax_SegmentID = "";
                            var Pax_OBCancel = "";
                            var Pax_IBCancel = "";
                            if (xmlDataObj.getElementsByTagName('Middle_Name')[i].childNodes[0]) {
                                Middle_Name = xmlDataObj.getElementsByTagName('Middle_Name')[i].firstChild.nodeValue;
                            }
                            if (xmlDataObj.getElementsByTagName('Last_Name')[i].childNodes[0]) {
                                Last_Name = xmlDataObj.getElementsByTagName('Last_Name')[i].firstChild.nodeValue;
                            }
                            var completeName = Title + " " + First_Name + " " + Middle_Name + " " + Last_Name;

                            if (xmlDataObj.getElementsByTagName('Pax_SegmentId')[i].childNodes[0]) {
                                Pax_SegmentID = xmlDataObj.getElementsByTagName('Pax_SegmentId')[i].firstChild.nodeValue;
                            }
                            if (xmlDataObj.getElementsByTagName('Outbound')[i].childNodes[0]) {
                                Pax_OBCancel = xmlDataObj.getElementsByTagName('Outbound')[i].firstChild.nodeValue;
                            }
                            if (xmlDataObj.getElementsByTagName('Inbound')[i].childNodes[0]) {
                                Pax_IBCancel = xmlDataObj.getElementsByTagName('Inbound')[i].firstChild.nodeValue;
                            }
                            var PaxType = "";
                            if (xmlDataObj.getElementsByTagName("CompanyPaxDetailAirlinesData")[i].getElementsByTagName("PaxType")[0].childNodes[0]) {
                                PaxType = xmlDataObj.getElementsByTagName("CompanyPaxDetailAirlinesData")[i].getElementsByTagName("PaxType")[0].textContent
                            }
                            var sno = i + 1;
                            if (Trip == "R") {
                                if (PnrOB.trim().toUpperCase() == PnrIB.trim().toUpperCase()) {
                                    if ((carriercodeOB.toUpperCase().indexOf('SG') != -1) || (carriercodeOB.toUpperCase().indexOf('6E') != -1) || (carriercodeOB.toUpperCase().indexOf('G8') != -1) || (carriercodeOB.toUpperCase().indexOf('I5') != -1) || (carriercodeOB.toUpperCase().indexOf('IX') != -1) || (carriercodeOB.toUpperCase().indexOf('LB') != -1) || (carriercodeOB.toUpperCase().indexOf('D7') != -1) || (carriercodeOB.toUpperCase().indexOf('FD') != -1) || (carriercodeOB.toUpperCase().indexOf('AK') != -1) || (carriercodeOB.toUpperCase().indexOf('FZ') != -1)) {
                                        if (Pax_OBCancel == "1" && Pax_IBCancel != "1") {
                                            strhtml += "<div class='col-md-12'>";
                                            strhtml += "<div class='col-md-6'><span class='paxcancle OBPaxchkbox' style='color:red;' >" + sno + ".  " + completeName + " (CAN)</span> </div>";
                                            strhtml += "<div class='col-md-6'><input class='IBPaxchkbox' name='PaxDetails' id='PaxDetailsIB_" + i + "' value='" + Pax_SegmentID + "' type='checkbox'/>" + sno + ".  " + completeName + " </div></div>";

                                        }
                                        else if (Pax_OBCancel != "1" && Pax_IBCancel == "1") {
                                            strhtml += "<div class='col-md-12'>";
                                            strhtml += "<div class='col-md-6'><input class='OBPaxchkbox' name='PaxDetails' id='PaxDetails_" + i + "' value='" + Pax_SegmentID + "' type='checkbox' />" + sno + ".  " + completeName + " </div>";
                                            strhtml += "<div class='col-md-6'><span class='paxcancle IBPaxchkbox' style='color:red;' >" + sno + ".  " + completeName + " (CAN) </span>  </div></div>";

                                        }
                                        else if (Pax_OBCancel == "1" && Pax_IBCancel == "1") {
                                            strhtml += "<div class='col-md-12'>";
                                            strhtml += "<div class='col-md-6'><span class='paxcancle OBPaxchkbox' style='color:red;' >" + sno + ".  " + completeName + " (CAN)</span>  </div>";
                                            strhtml += "<div class='col-md-6'><span class='paxcancle IBPaxchkbox' style='color:red;' >" + sno + ".  " + completeName + " (CAN)</span></div>";


                                        }
                                        else {
                                            strhtml += "<div class='col-md-12'>";
                                            strhtml += "<div class='col-md-6'><input class='OBPaxchkbox' name='PaxDetails' id='PaxDetails_" + i + "' value='" + Pax_SegmentID + "' type='checkbox'/>" + sno + ".  " + completeName + " </div>";
                                            strhtml += "<div class='col-md-6'><input class='IBPaxchkbox' name='PaxDetails' id='PaxDetailsIB_" + i + "' value='" + Pax_SegmentID + "' type='checkbox'/>" + sno + ".  " + completeName + " </div></div>";
                                        }
                                        counter += 1;
                                    }
                                    else {
                                        if (Pax_OBCancel == "1") {
                                            strhtml += "<br/><span class='paxcancle ' style='color:red;'>" + sno + ".  " + completeName + "(CAN) </span>";

                                        }
                                        else {
                                            strhtml += "<br/><span><input name='PaxDetails' id='PaxDetails_" + i + "' value='" + Pax_SegmentID + "' type='checkbox'/>" + sno + ".  " + completeName + " </span>";
                                        }
                                        counter += 1;
                                    }

                                }
                                else {
                                    
                                    if (Pax_OBCancel == "1" && Pax_IBCancel != "1") {
                                        strhtml += "<div class='col-md-12'>";
                                        strhtml += "<div class='col-md-6'><span class='paxcancle OBPaxchkbox' style='color:red;'>" + sno + ".  " + completeName + " (CAN)</span> </div>";
                                        strhtml += "<div class='col-md-6'><input class='IBPaxchkbox' name='PaxDetails' id='PaxDetailsIB_" + i + "' value='" + Pax_SegmentID + "' type='checkbox'/>" + sno + ".  " + completeName + " </div></div>";

                                    }
                                    else if (Pax_OBCancel != "1" && Pax_IBCancel == "1") {
                                        strhtml += "<div class='col-md-12'>";
                                        strhtml += "<div class='col-md-6'><input class='OBPaxchkbox' name='PaxDetails' id='PaxDetails_" + i + "' value='" + Pax_SegmentID + "' type='checkbox' />" + sno + ".  " + completeName + " </div>";
                                        strhtml += "<div class='col-md-6'><span class='paxcancle IBPaxchkbox' style='color:red;'>" + sno + ".  " + completeName + " (CAN)</span> </div></div>";

                                    }
                                    else if (Pax_OBCancel == "1" && Pax_IBCancel == "1") {
                                        strhtml += "<div class='col-md-12'>";
                                        strhtml += "<div class='col-md-6'><span class='paxcancle OBPaxchkbox' style='color:red;'>" + sno + ".  " + completeName + " (CAN)</span>  </div>";
                                        strhtml += "<div class='col-md-6'><span class='paxcancle IBPaxchkbox' style='color:red;'>" + sno + ".  " + completeName + " (CAN)</span> </div></div>";

                                    }
                                    else {
                                        strhtml += "<div class='col-md-12'>";
                                        strhtml += "<div class='col-md-6'><input class='OBPaxchkbox' name='PaxDetails' id='PaxDetails_" + i + "' value='" + Pax_SegmentID + "' type='checkbox'/>" + sno + ".  " + completeName + " </div>";
                                        strhtml += "<div class='col-md-6'><input class='IBPaxchkbox' name='PaxDetails' id='PaxDetailsIB_" + i + "' value='" + Pax_SegmentID + "' type='checkbox'/>" + sno + ".  " + completeName + " </div></div>";
                                    }
                                    counter += 1;
                                }
                            }
                            else {
                                if (Pax_OBCancel == "1") {
                                    strhtml += "<br/><span class='paxcancle' style='color:red;'> " + sno + ".  " + completeName + " (CAN) </span>";

                                }
                                else {
                                    strhtml += "<br/><span><input name='PaxDetails' id='PaxDetails_" + i + "' value='" + Pax_SegmentID + "' type='checkbox'/>" + sno + ".  " + completeName + " </span>";
                                }
                                counter += 1;
                            }
                        }
                        strhtml += "</div>";
                        strhtml += "<div id='errMsg' class='red font10'>";
                        strhtml += "</div>";
                        strhtml += "</div>";
                        strhtml += "<div class=' req_row'>";
                        strhtml += "<span class='bold'>Please enter remarks <sup class='red'>*</sup></span>";
                        strhtml += "<textarea rows='3' cols='56' id='txtRemarks'></textarea>";

                        strhtml += "<div id='errMsgRemarks' class='red font10'></div>";
                        strhtml += "</div>";

                        strhtml += "<div class='req_row' id='sendReqMsgs' style='border-bottom: medium none;'>";
                        strhtml += "<span class='bold'>Note.</span>";
                        strhtml += "<ol class='note_req'>";
                        //strhtml += "<li>Partial Refund will be processed offline.</li>";
                        //strhtml += "<li>In case of Infant booking, cancellation will be processed offline.</li>";
                        strhtml += "<li>In case of One sector to be cancel, please send the offline request.</li>";
                        //strhtml += "<li>In case of Flight cancellation/ flight reschedule, please select flight cancelled.</li>";
                       // strhtml += "<li>Cancellation Charges cannot be retrieved for Partial Cancelled Booking</li>";
                        strhtml += "</ol>";
                        strhtml += "</div>";
                        strhtml += "<li style='color:red;font-size: 12px;'>*Payment success status will appear after send request.</li>";
                        //strhtml += "<li style='color:red;font-size: 12px;'>*if any Dispute or No show refund so it may takes more than 7 days.</li>";
                        strhtml += "</div>";
                        strhtml += "<div class='req_row'></div>";
                        strhtml += "<span class='bold align_center fleft width_100 mt' id='CancellationChargesBlock' style='display:none;'>Total Cancellation Charges: <tt id='CancellationCharges'></tt> <br>Total Refund Amount : <tt id='RefundedAmount'></tt></span>";
                        strhtml += "<span class='bold red font10 font10 align_center fleft width_100 mt' id='errCancellationCharges' style='display: none;'></span>";

                        strhtml += "<span class='req_row_btn' id='btnPanel'>";

                        //strhtml += "<div id='loaderCanCharges' class='ml align_center fleft width_100 mt mb' style='display:none'>";
                        //strhtml += "<img src='images/loaderNew.gif' alt='loading' style='vertical-align:middle;'> <b class='ml mt'>Loading Payment Amounts...</b>";
                        //strhtml += "<div class='clr'></div>";
                        //strhtml += "</div> ";

                        //strhtml += "<input class='btn_main_s mr' value='View Cancel. Charges' id='CancellationChargesbtn' style='display:none;' type='button'/>";
                        strhtml += "<input class='btn_main_s mr' value='Pay' id='btnSendPaymentReq' type='button' onclick='SendPaymentReq();'/>";
                        strhtml += "<input class='btn_main_s' value='Cancel' id='btnSendChangeReqCancel' type='button' data-dismiss='modal'/>";
                        strhtml += "</span>";

                        strhtml += "<div class='req_row red bold align_center' id='processingMsg' style='display: none;'>Your Transaction is Processing...</div>";
                        strhtml += "</div>";
						//===== new for waiting popup
						strhtml += "<div class='modal' id='modalloadingPay' style='display: none; background: rgba(0, 0, 0, 0.72);'>";
						strhtml += "<div class='modal-dialog modal-sm'>";
						strhtml += "<div class='modal-content'>";
						strhtml += "<div class='modal-body'>";
						strhtml += "<span class='bold'>Please wait...!</span>";
						strhtml += "</div>";
						strhtml += "</div>";
						strhtml += "</div>";
						strhtml += "</div>";
                        //=============
                        
                        $('#printPaymentdata').append(strhtml);

                        $("#btnSendPaymentReqPopup").modal();
						

                        var popupcancelbtn =document.getElementById("btnSendChangeReqCancel");
                        popupcancelbtn.addEventListener("click",()=>{
                            for (let i = 0; i < $(".btn.btn-default.paybtn").length; i++) {
                                $(".btn.btn-default.paybtn").eq(i).attr("onclick", 
                                    "Payticket(this);");
                            }
                        })
                    }
                } ,
                error: function(xhr, status, error) {
                    console.log('AJAX error:', status, error);
                  }
            });

        }
        catch (e) { 
            console.log(e);
            
        }

}
window.Payticket = Payticket;


function SendPaymentReq() {
    ticketIds = '';
    ticketIdsdif = '';
    Sector = '';

    $('#errMsgRequestType').text('');
    requestType = $("#requestType option:selected").val();
    if (requestType == "Select") {
        $('#errMsgRequestType').append('Please select request type');
        return;
    }
    if ($('input[name="sectorsSelectors"]:checked').length == 0) {
        $('#secErrmsg').text('Please Select Sector.');
        return;
    }
    else {
        $('#secErrmsg').text('');
    }
    if ($('input[name="PaxDetails"]:checked').length == 0) {
        $('#errMsg').text('Please Select Passenger(s).');
        return;
    }
    else {
        $('#errMsg').text('');
    }

    if ($('#txtRemarks').val().length == 0) {
        $('#errMsgRemarks').text('');
        $('#errMsgRemarks').append('Please enter remark(s).');
        return;
    }
    else {
        $('#errMsgRemarks').html('');
    }
    bookingid = $("#ContentPlaceHolder1_hdnbookref").val();

    $('input:checkbox[name=sectorsSelectors]:checked').each(function () {
        if (Sector == '') {
            Sector = $(this).val();
        }
        else {
            Sector = Sector + ',' + $(this).val();
        }
    });
    if (Trip == "R") {
        if ($("#ContentPlaceHolder1_hdndiffrentPnr").val() != "") {
            if ($("#sector_1").is(":checked") && $("#sector_2").is(":not(:checked)")) {

                $('input:checkbox[class=IBPaxchkbox]:checked').each(function () {
                    $(this).attr('checked', false);
                });
                if ($('input[class="OBPaxchkbox"]:checked').length == 0) {
                    $('#errMsg').text('Please Select Outbound Passenger(s).');
                    return;
                }
                else {
                    $('#errMsg').text('');
                    $(".OBPaxchkbox:checked").each(function () {
                        if (ticketIds == '') {
                            ticketIds = $(this).val();
                        }
                        else {
                            ticketIds = ticketIds + ',' + $(this).val();
                        }
                    });
                }
            }
            else if ($("#sector_2").is(":checked") && $("#sector_1").is(":not(:checked)")) {

                $('input:checkbox[class=OBPaxchkbox]:checked').each(function () {
                    $(this).attr('checked', false);
                });
                if ($('input[class="IBPaxchkbox"]:checked').length == 0) {
                    $('#errMsg').text('Please Select Inbound Passenger(s).');
                    return;
                }
                else {
                    $('#errMsg').text('');
                    $(".IBPaxchkbox:checked").each(function () {
                        if (ticketIdsdif == '') {
                            ticketIdsdif = $(this).val();
                        }
                        else {
                            ticketIdsdif = ticketIdsdif + ',' + $(this).val();
                        }
                    });
                }
            }
            else {

                if ($('input[class="IBPaxchkbox"]:checked').length == 0) {
                    $('#errMsg').text('Please Select Inbound Passenger(s).');
                    return;
                }
                else {
                    $(".IBPaxchkbox:checked").each(function () {
                        if (ticketIdsdif == '') {
                            ticketIdsdif = $(this).val();
                        }
                        else {
                            ticketIdsdif = ticketIdsdif + ',' + $(this).val();
                        }
                    });
                }
                if ($('input[class="OBPaxchkbox"]:checked').length == 0) {
                    $('#errMsg').text('Please Select Outbound Passenger(s).');
                    return;
                }
                else {
                    $(".OBPaxchkbox:checked").each(function () {
                        if (ticketIds == '') {
                            ticketIds = $(this).val();
                        }
                        else {
                            ticketIds = ticketIds + ',' + $(this).val();
                        }
                    });
                }
            }
        }
        else {
            if ($("#ContentPlaceHolder1_hdncarriercodeOB").val() != "") {
                var carriercode = $("#ContentPlaceHolder1_hdncarriercodeOB").val();
                if ((carriercode.toUpperCase().indexOf('SG') != -1) || (carriercode.toUpperCase().indexOf('6E') != -1) || (carriercode.toUpperCase().indexOf('G8') != -1) || (carriercode.toUpperCase().indexOf('I5') != -1) || (carriercode.toUpperCase().indexOf('IX') != -1) || (carriercode.toUpperCase().indexOf('LB') != -1) || (carriercode.toUpperCase().indexOf('D7') != -1) || (carriercode.toUpperCase().indexOf('FD') != -1) || (carriercode.toUpperCase().indexOf('AK') != -1) || (carriercode.toUpperCase().indexOf('FZ') != -1)) {

                    if ($("#sector_1").is(":checked") && $("#sector_2").is(":not(:checked)")) {

                        $('input:checkbox[class=IBPaxchkbox]:checked').each(function () {
                            $(this).attr('checked', false);
                        });
                        if ($('input[class="OBPaxchkbox"]:checked').length == 0) {
                            $('#errMsg').text('Please Select Outbound Passenger(s).');
                            return;
                        }
                        else {
                            $('#errMsg').text('');
                            $(".OBPaxchkbox:checked").each(function () {
                                if (ticketIds == '') {
                                    ticketIds = $(this).val();
                                }
                                else {
                                    ticketIds = ticketIds + ',' + $(this).val();
                                }
                            });
                        }
                    }
                    else if ($("#sector_2").is(":checked") && $("#sector_1").is(":not(:checked)")) {

                        $('input:checkbox[class=OBPaxchkbox]:checked').each(function () {
                            $(this).attr('checked', false);
                        });
                        if ($('input[class="IBPaxchkbox"]:checked').length == 0) {
                            $('#errMsg').text('Please Select Inbound Passenger(s).');
                            return;
                        }
                        else {
                            $('#errMsg').text('');
                            $(".IBPaxchkbox:checked").each(function () {
                                if (ticketIdsdif == '') {
                                    ticketIdsdif = $(this).val();
                                }
                                else {
                                    ticketIdsdif = ticketIdsdif + ',' + $(this).val();
                                }
                            });
                        }
                    }
                    else {

                        if ($('input[class="IBPaxchkbox"]:checked').length == 0) {
                            $('#errMsg').text('Please Select Inbound Passenger(s).');
                            return;
                        }
                        else {
                            $(".IBPaxchkbox:checked").each(function () {
                                if (ticketIdsdif == '') {
                                    ticketIdsdif = $(this).val();
                                }
                                else {
                                    ticketIdsdif = ticketIdsdif + ',' + $(this).val();
                                }
                            });
                        }
                        if ($('input[class="OBPaxchkbox"]:checked').length == 0) {
                            $('#errMsg').text('Please Select Outbound Passenger(s).');
                            return;
                        }
                        else {
                            $(".OBPaxchkbox:checked").each(function () {
                                if (ticketIds == '') {
                                    ticketIds = $(this).val();
                                }
                                else {
                                    ticketIds = ticketIds + ',' + $(this).val();
                                }
                            });
                        }
                    }
                }
                else {
                    $('input:checkbox[name=PaxDetails]:checked').each(function () {
                        if (ticketIds == '') {
                            ticketIds = $(this).val();
                        }
                        else {
                            ticketIds = ticketIds + ',' + $(this).val();
                        }
                    });
                }
            }
        }
    }
    else {
        $('input:checkbox[name=PaxDetails]:checked').each(function () {
            if (ticketIds == '') {
                ticketIds = $(this).val();
            }
            else {
                ticketIds = ticketIds + ',' + $(this).val();
            }
        });
    }
    remarks = $('#txtRemarks').val();
    //$('#btnPanel').hide();
    var chkPaxCount = 0;
    $('input:checkbox[name=PaxDetails]:checked').each(function () {
        chkPaxCount++;
    });
    //if (IsLcc && 'FullCancellation' == requestType && paxCount == chkPaxCount && $('#sector_0').is(':checked')) {
    //    $('#confirmationPopup').show();
    //}
    //else {
    //    $('#processingMsg').show();
	
	 $("#modalloadingPay").css("display", 'block');

	 
    $.ajax({
        url: "/Booking/SendPayment4HoldBookRequest",
        data: JSON.stringify({ BookingRef: bookingid, PaymentType: requestType, PaxSegmentID: ticketIds, Remarks: remarks, Sector: Sector, PaxSegmentID2: ticketIdsdif }),
        type: "POST",
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        dataFilter: function (data) { return data; },
        success: function (data, textStatus, jqXHR) {
            SendPaymentRequestResp(data,jqXHR.status)
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            const payload = XMLHttpRequest.responseJSON || JSON.parse(XMLHttpRequest.responseText);
            alert(`Error: ${payload.error}`);
            $("#btnSendPaymentReqPopup").modal('hide');
            for (let i = 0; i < $(".btn.btn-default.paybtn").length; i++) {
                $(".btn.btn-default.paybtn").eq(i).attr("onclick", 
                    "Payticket(this);");
            }
        }
    });
    // }
	// $("#modalloadingPay").css("display", 'none');
}
window.SendPaymentReq= SendPaymentReq;
function SendPaymentRequestResp(resp,statusCode) {
    if (statusCode == 200) {
        alert("Payment has been successfully processed for this ticket.");
        $("#btnSendPaymentReqPopup").modal('hide');
        location.reload();
    }
    else {
        alert("There is something error.!");
        $("#btnSendPaymentReqPopup").modal('hide');
        for (let i = 0; i < $(".btn.btn-default.paybtn").length; i++) {
            $(".btn.btn-default.paybtn").eq(i).attr("onclick", 
                "Payticket(this);");
        }
    }
}
//======== payment for hold end


function bookrefff(abc) {

    var result = abc.accessKey;
    window.open("Print_Popup.aspx?BookingRef=" + btoa(result), "Popup1", "location=1,status=1,scrollbars=1, resizable=1, directories=1, toolbar=1, titlebar=1, width=940, height=800");
}
function printinvoice(element) {
    var bookingRef = element.getAttribute('accesskey');

    if (!bookingRef) {
        return; 
    }

    var encodedBookingRef = btoa(bookingRef);

    window.open("/printInvoice?BookingRef=" + encodedBookingRef,
        "PopupInvoice",
        "location=1,status=1,scrollbars=1,resizable=1,directories=1,toolbar=1,titlebar=1,width=900,height=800");
}
function Perpaxticket(abc) {

    var result = abc.accessKey;
    var bookingref = result.split(",")[0];
    var segM_ID = result.split(",")[1];
    window.open("Print_Pax_Popup.aspx?BookingRef=" + btoa(bookingref) + "&SegmentID=" + btoa(segM_ID), "Popup4", "location=1,status=1,scrollbars=1, resizable=1, directories=1, toolbar=1, titlebar=1, width=940, height=800");
}

function PerpaxticketCus(abc) {

    var result = abc.accessKey;
    var bookingref = result.split(",")[0];
    var segM_ID = result.split(",")[1];
    window.open("Print_Pax_Popup_SA.aspx?BookingRef=" + btoa(bookingref) + "&SegmentID=" + btoa(segM_ID), "Popup5", "location=1,status=1,scrollbars=1, resizable=1, directories=1, toolbar=1, titlebar=1, width=940, height=800");
}

function validatesearchtab() {

    if (($("#ContentPlaceHolder1_txtbookreffilter").val() != "" && $("#ContentPlaceHolder1_restricpnr").val() == "") && $("#ContentPlaceHolder1_restricticket").val() == "") {

        var filter = /^[0-9-+]+$/;
        if (filter.test($("#ContentPlaceHolder1_txtbookreffilter").val().trim())) {
            return true;
        }
        else {
            alert("please enter integer value");
            return false;
        }
    }
    else if (($("#ContentPlaceHolder1_txtbookreffilter").val() == "" && $("#ContentPlaceHolder1_restricpnr").val() != "") && $("#ContentPlaceHolder1_restricticket").val() == "") {

        return true;
    }
    else if (($("#ContentPlaceHolder1_txtbookreffilter").val() == "" && $("#ContentPlaceHolder1_restricpnr").val() == "") && $("#ContentPlaceHolder1_restricticket").val() != "") {

        return true;
    }
    else if ($("#ContentPlaceHolder1_restricpax").val() != "") {
        document.getElementById('ContentPlaceHolder1_txtbookreffilter').value = "";
        document.getElementById('ContentPlaceHolder1_restricpnr').value = "";
        document.getElementById('ContentPlaceHolder1_restricticket').value = "";
        if ($("#ContentPlaceHolder1_txtRestrictfromdate").val() != "" && $("#ContentPlaceHolder1_txtRestricttodate").val() != "") {
            return true;
        }
        else {
            alert("Please enter bookingfrom date and bookingTo date.!");
            return false;
        }
    }
    else if ($("#ContentPlaceHolder1_restricpnr").val() != "" && $("#ContentPlaceHolder1_restricticket").val() != "") {

        document.getElementById('ContentPlaceHolder1_txtbookreffilter').value = "";
        document.getElementById('ContentPlaceHolder1_restricpnr').value = "";
        document.getElementById('ContentPlaceHolder1_restricticket').value = "";
        alert("Please fill one field.!");
        return false
    }
    else {
        document.getElementById('ContentPlaceHolder1_txtbookreffilter').value = "";
        document.getElementById('ContentPlaceHolder1_restricpnr').value = "";
        document.getElementById('ContentPlaceHolder1_restricticket').value = "";
        alert("Please fill one field.!");
        return false
    }
}
function cleartxtfield() {

    document.getElementById('ContentPlaceHolder1_txtbookreffilter').value = "";
    document.getElementById('ContentPlaceHolder1_restricpnr').value = "";
    document.getElementById('ContentPlaceHolder1_restricticket').value = "";
    document.getElementById('ContentPlaceHolder1_restricpax').value = "";
    document.getElementById("searchbydatediv").style.display = "none";
}

function printinvoiceCust(abc) {
    var result = abc.accessKey;
    window.open("Print_Invoice_SA.aspx?BookingRef=" + btoa(result), "Popup6", "location=1,status=1,scrollbars=1, resizable=1, directories=1, toolbar=1, titlebar=1, width=900, height=800");
}

function printTicketCust(abc) {
    var result = abc.accessKey;
    window.open("Print_Popup_SA.aspx?BookingRef=" + btoa(result), "Popup7", "location=1,status=1,scrollbars=1, resizable=1, directories=1, toolbar=1, titlebar=1, width=900, height=800");
}