
function removeDuplicateFlights(arr) {

    let map = new Map()

    for (let i = 0; i < arr.d.length; i++) {

        const s = JSON.stringify(arr.d[i]);

        if (!map.has(s)) {

            map.set(s, arr.d[i]);

        }

    }

    const res = Array.from(map.values())

    return res;

}
window.removeDuplicateFlights = removeDuplicateFlights
//give JSON response group by the flight number
function groupJson(jsonResponse) {
    var groupByFlightNumber = removeDuplicateFlights(jsonResponse).reduce(function (result, current) {

        result[current.FlightNumberCmb] = result[current.FlightNumberCmb] || [];

        result[current.FlightNumberCmb].push(current);

        return result;

    }, {})
    return groupByFlightNumber;
}
window.groupJson = groupJson;
var flagOnwardPrice = 1;
function OnwardPrice(sortF) {
    
    $('#AirlineSort').removeClass('downArrow');
    $('#AirlineSort').removeClass('upArrow');
    $('#PriceSort').removeClass('downArrow');
    $('#PriceSort').removeClass('upArrow');
    $('#DepartSort').removeClass('downArrow');
    $('#DepartSort').removeClass('upArrow');
    $('#ArriveSort').removeClass('downArrow');
    $('#ArriveSort').removeClass('upArrow');
    $('#StopSort').removeClass('downArrow');
    $('#StopSort').removeClass('upArrow');

    var msg = sortF;
    var sortedJsonRes = msg;
    if (flagOnwardPrice == 0) {
        flagOnwardPrice = 1;
        $('#PriceSort').addClass('downArrow');

      
        sortedJsonRes.d.sort(function (obj1, obj2) {
            return parseInt(obj1.FinalFare) - parseInt(obj2.FinalFare);

        });
    }
    else {
        flagOnwardPrice = 0;
        $('#PriceSort').addClass('upArrow');
        
        sortedJsonRes.d.sort(function (obj1, obj2) {
            return parseInt(obj2.FinalFare) - parseInt(obj1.FinalFare);

        });

    }
  
    return sortedJsonRes;
    chkunchk();
}
window.OnwardPrice = OnwardPrice;
var flag = 0;
function OnwardAirline(sortF) {
    flagSort = 'FLIGHT';

   

    var sortedJsonRes = sortF;
    $('#AirlineSort').removeClass('downArrow');
    $('#AirlineSort').removeClass('upArrow');
    $('#PriceSort').removeClass('downArrow');
    $('#PriceSort').removeClass('upArrow');
    $('#DepartSort').removeClass('downArrow');
    $('#DepartSort').removeClass('upArrow');
    $('#ArriveSort').removeClass('downArrow');
    $('#ArriveSort').removeClass('upArrow');
    $('#StopSort').removeClass('downArrow');
    $('#StopSort').removeClass('upArrow');
    
    if (flag == 0) {
        flag = 1;
        $('#AirlineSort').addClass('downArrow');


        sortedJsonRes.d.sort(function (obj1, obj2) {
            if (obj1.FlightName < obj2.FlightName) {
                return -1;
            } else if (obj1.FlightName > obj2.FlightName) {
                return 1;
            } else {
                // If the flight names are the same, sort by number
                return obj1.FlightNumber < obj2.FlightNumber ? -1 :
                  (obj1.FlightNumber > obj2.FlightNumber ? 1 : 0);
            }
        });
    }
    else {
        flag = 0;
        $('#AirlineSort').addClass('upArrow');


        sortedJsonRes.d.sort(function (obj1, obj2) {
            if (obj1.FlightName > obj2.FlightName) {
                return -1;
            } else if (obj1.FlightName < obj2.FlightName) {
                return 1;
            } else {
                // If the flight names are the same, sort by number

                return obj1.FlightNumber > obj2.FlightNumber ? -1 :
                  (obj1.FlightNumber < obj2.FlightNumber ? 1 : 0);
            }
        });
    }

    return sortedJsonRes;
    chkunchk();
}
window.OnwardAirline = OnwardAirline;
var flagDepart = 0;
function OnwardDepart(sortF) {
   
    
    var sortedJsonRes = sortF;
    flagSort = 'DEPART';
    $('#AirlineSort').removeClass('downArrow');
    $('#AirlineSort').removeClass('upArrow');
    $('#PriceSort').removeClass('downArrow');
    $('#PriceSort').removeClass('upArrow');
    $('#DepartSort').removeClass('downArrow');
    $('#DepartSort').removeClass('upArrow');
    $('#ArriveSort').removeClass('downArrow');
    $('#ArriveSort').removeClass('upArrow');
    $('#StopSort').removeClass('downArrow');
    $('#StopSort').removeClass('upArrow');
    jQuery('#tempView').html('');
    if (flagDepart == 0) {
        flagDepart = 1;
        $('#DepartSort').addClass('downArrow');

        sortedJsonRes.d.sort(function (obj1, obj2) {

            return obj1.FlightDepTime < obj2.FlightDepTime ? -1 :
           (obj1.FlightDepTime > obj2.FlightDepTime ? 1 : 0);
        });
    }
    else {
        flagDepart = 0;
        $('#DepartSort').addClass('upArrow');

        sortedJsonRes.d.sort(function (obj1, obj2) {

            return obj1.FlightDepTime > obj2.FlightDepTime ? -1 :
           (obj1.FlightDepTime < obj2.FlightDepTime ? 1 : 0);
        });
    }

    return sortedJsonRes

    chkunchk();
}
window.OnwardDepart = OnwardDepart;
var flagArrive = 0;
function OnwardArrive(sortF) {
   
    flagSort = 'ARRIVE';
    var sortedJsonRes = sortF;
    $('#AirlineSort').removeClass('downArrow');
    $('#AirlineSort').removeClass('upArrow');
    $('#PriceSort').removeClass('downArrow');
    $('#PriceSort').removeClass('upArrow');
    $('#DepartSort').removeClass('downArrow');
    $('#DepartSort').removeClass('upArrow');
    $('#ArriveSort').removeClass('downArrow');
    $('#ArriveSort').removeClass('upArrow');
    $('#StopSort').removeClass('downArrow');
    $('#StopSort').removeClass('upArrow');
    jQuery('#tempView').html('');
    if (flagArrive == 0) {
        flagArrive = 1;
        $('#ArriveSort').addClass('downArrow');

        sortedJsonRes.d.sort(function (obj1, obj2) {

            return obj1.FlightArrTime < obj2.FlightArrTime ? -1 :
           (obj1.FlightArrTime > obj2.FlightArrTime ? 1 : 0);
        });
    }
    else {
        flagArrive = 0;
        $('#ArriveSort').addClass('upArrow');

        sortedJsonRes.d.sort(function (obj1, obj2) {

            return obj1.FlightArrTime > obj2.FlightArrTime ? -1 :
           (obj1.FlightArrTime < obj2.FlightArrTime ? 1 : 0);
        });

    }

    return sortedJsonRes
    chkunchk();
}
window.OnwardArrive = OnwardArrive;


var flagStop = 0;

function OnwardStop(sortF) {
    flagSort = 'STOP';
    var sortedJsonRes = sortF;
    $('#AirlineSort').removeClass('downArrow');
    $('#AirlineSort').removeClass('upArrow');
    $('#PriceSort').removeClass('downArrow');
    $('#PriceSort').removeClass('upArrow');
    $('#DepartSort').removeClass('downArrow');
    $('#DepartSort').removeClass('upArrow');
    $('#ArriveSort').removeClass('downArrow');
    $('#ArriveSort').removeClass('upArrow');
    $('#StopSort').removeClass('downArrow');
    $('#StopSort').removeClass('upArrow');


    
    
    if (flagStop == 0) {
        flagStop = 1;
        $('#StopSort').addClass('downArrow');

        sortedJsonRes.d.sort(function (obj1, obj2) {

            return obj1.connection < obj2.connection ? -1 :
           (obj1.connection > obj2.connection ? 1 : 0);
        });
    }
    else {
        flagStop = 0;
        $('#StopSort').addClass('upArrow');

        sortedJsonRes.d.sort(function (obj1, obj2) {

            return obj1.connection > obj2.connection ? -1 :
           (obj1.connection < obj2.connection ? 1 : 0);
        });

    }

    return sortedJsonRes

    chkunchk();
}
window.OnwardStop = OnwardStop;
var flagOnwardPriceR = 1;
function ReturnPrice(sortF) {
    $("#modelpopupOUTER").css("display", "Block");
    $("#modelpopup").css("background-color", "Gray");
    $('#modelpopup').delay(1).fadeIn(400);
    $('#modelpopupOUTER').delay(1).fadeIn(400);
    $('#AirlineSortR').removeClass('downArrow');
    $('#AirlineSortR').removeClass('upArrow');
    $('#PriceSortR').removeClass('downArrow');
    $('#PriceSortR').removeClass('upArrow');
    $('#DepartSortR').removeClass('downArrow');
    $('#DepartSortR').removeClass('upArrow');
    $('#ArriveSortR').removeClass('downArrow');
    $('#ArriveSortR').removeClass('upArrow');
    $('#StopSortR').removeClass('downArrow');
    $('#StopSortR').removeClass('upArrow');
    flagSort = 'PRICE';

    var sortedJsonRes = sortF;
    if (flagOnwardPriceR == 0) {
        flagOnwardPriceR = 1;
        $('#PriceSortR').addClass('downArrow');
       
        sortedJsonRes.d.sort(function (obj1, obj2) {
            return parseInt(obj1.FinalFare) - parseInt(obj2.FinalFare);

        });
    }
    else {
        flagOnwardPriceR = 0;
        $('#PriceSortR').addClass('upArrow');
       

            sortedJsonRes.d.sort(function (obj1, obj2) {
                return parseInt(obj2.FinalFare) - parseInt(obj1.FinalFare);

        });
        
    }

   
    return sortedJsonRes
    chkunchk();
}
window.ReturnPrice = ReturnPrice;
var flagR = 0;
function ReturnAirline(sortF) {
    $("#modelpopupOUTER").css("display", "Block");
    $("#modelpopup").css("background-color", "Gray");
    $('#modelpopup').delay(1).fadeIn(400);
    $('#modelpopupOUTER').delay(1).fadeIn(400);
    $('#AirlineSortR').removeClass('downArrow');
    $('#AirlineSortR').removeClass('upArrow');
    $('#PriceSortR').removeClass('downArrow');
    $('#PriceSortR').removeClass('upArrow');
    $('#DepartSortR').removeClass('downArrow');
    $('#DepartSortR').removeClass('upArrow');
    $('#ArriveSortR').removeClass('downArrow');
    $('#ArriveSortR').removeClass('upArrow');
    $('#StopSortR').removeClass('downArrow');
    $('#StopSortR').removeClass('upArrow');
    var sortedJsonRes = sortF;
    
    flagSort = 'FLIGHT';
    if (flagR == 0) {
        flagR = 1;
        $('#AirlineSortR').addClass('downArrow');
        sortedJsonRes.d.sort(function (obj1, obj2) {
            if (obj1.FlightName < obj2.FlightName) {
                return -1;
            } else if (obj1.FlightName > obj2.FlightName) {
                return 1;
            } else {
                // If the flight names are the same, sort by number
                return obj1.FlightNumber < obj2.FlightNumber ? -1 :
                  (obj1.FlightNumber > obj2.FlightNumber ? 1 : 0);
            }
        });
    }
    else {
        flagR = 0;
        $('#AirlineSortR').addClass('upArrow');
        sortedJsonRes.d.sort(function (obj1, obj2) {
            if (obj1.FlightName > obj2.FlightName) {
                return -1;
            } else if (obj1.FlightName < obj2.FlightName) {
                return 1;
            } else {
                // If the flight names are the same, sort by number

                return obj1.FlightNumber > obj2.FlightNumber ? -1 :
                  (obj1.FlightNumber < obj2.FlightNumber ? 1 : 0);
            }
        });
    }
 
    return sortedJsonRes
    chkunchk();
}
window.ReturnAirline = ReturnAirline;
var flagDepartR = 0;
function ReturnDepart(sortF) {
    $("#modelpopupOUTER").css("display", "Block");
    $("#modelpopup").css("background-color", "Gray");
    $('#modelpopup').delay(1).fadeIn(400);
    $('#modelpopupOUTER').delay(1).fadeIn(400);
    $('#AirlineSortR').removeClass('downArrow');
    $('#AirlineSortR').removeClass('upArrow');
    $('#PriceSortR').removeClass('downArrow');
    $('#PriceSortR').removeClass('upArrow');
    $('#DepartSortR').removeClass('downArrow');
    $('#DepartSortR').removeClass('upArrow');
    $('#ArriveSortR').removeClass('downArrow');
    $('#ArriveSortR').removeClass('upArrow');
    $('#StopSortR').removeClass('downArrow');
    $('#StopSortR').removeClass('upArrow');
    flagSort = 'DEPART';
    var sortedJsonRes = sortF;
    if (flagDepartR == 0) {
        flagDepartR = 1;
        $('#DepartSortR').addClass('downArrow');
      
        sortedJsonRes.d.sort(function (obj1, obj2) {

            return obj1.FlightDepTime < obj2.FlightDepTime ? -1 :
           (obj1.FlightDepTime > obj2.FlightDepTime ? 1 : 0);
        });
    }
    else {
        flagDepartR = 0;
        $('#DepartSortR').addClass('upArrow');
        sortedJsonRes.d.sort(function (obj1, obj2) {

            return obj1.FlightDepTime > obj2.FlightDepTime ? -1 :
           (obj1.FlightDepTime < obj2.FlightDepTime ? 1 : 0);
        });

    }
  
    return sortedJsonRes
    chkunchk();
}
window.ReturnDepart = ReturnDepart;
var flagArriveR = 0;
function ReturnArrive(msg) {
    let sortF = msg;
    $("#modelpopupOUTER").css("display", "Block");
    $("#modelpopup").css("background-color", "Gray");
    $('#modelpopup').delay(1).fadeIn(400);
    $('#modelpopupOUTER').delay(1).fadeIn(400);
    $('#AirlineSortR').removeClass('downArrow');
    $('#AirlineSortR').removeClass('upArrow');
    $('#PriceSortR').removeClass('downArrow');
    $('#PriceSortR').removeClass('upArrow');
    $('#DepartSortR').removeClass('downArrow');
    $('#DepartSortR').removeClass('upArrow');
    $('#ArriveSortR').removeClass('downArrow');
    $('#ArriveSortR').removeClass('upArrow');
    $('#StopSortR').removeClass('downArrow');
    $('#StopSortR').removeClass('upArrow');
    var sortedJsonRes = sortF;
    flagSort = 'ARRIVE';
    if (flagArriveR == 0) {
        flagArriveR = 1;
        $('#ArriveSortR').addClass('downArrow');
        sortedJsonRes.d.sort(function (obj1, obj2) {

            return obj1.FlightArrTime < obj2.FlightArrTime ? -1 :
           (obj1.FlightArrTime > obj2.FlightArrTime ? 1 : 0);
        });
    }
    else {
        flagArriveR = 0;
        $('#ArriveSortR').addClass('upArrow');
        sortedJsonRes.d.sort(function (obj1, obj2) {

            return obj1.FlightArrTime > obj2.FlightArrTime ? -1 :
           (obj1.FlightArrTime < obj2.FlightArrTime ? 1 : 0);
        });
    }
  
    return sortedJsonRes
    chkunchk();
}
window.ReturnArrive = ReturnArrive;
var flagStopR = 0;
function ReturnStop(msg) {
    let sortF = msg;
    $("#modelpopupOUTER").css("display", "Block");
    $("#modelpopup").css("background-color", "Gray");
    $('#modelpopup').delay(1).fadeIn(400);
    $('#modelpopupOUTER').delay(1).fadeIn(400);
    $('#AirlineSortR').removeClass('downArrow');
    $('#AirlineSortR').removeClass('upArrow');
    $('#PriceSortR').removeClass('downArrow');
    $('#PriceSortR').removeClass('upArrow');
    $('#DepartSortR').removeClass('downArrow');
    $('#DepartSortR').removeClass('upArrow');
    $('#ArriveSortR').removeClass('downArrow');
    $('#ArriveSortR').removeClass('upArrow');
    $('#StopSortR').removeClass('downArrow');
    $('#StopSortR').removeClass('upArrow');
    var sortedJsonRes = sortF;
    flagSort = 'STOP';
    if (flagStopR == 0) {
        flagStopR = 1;
        $('#StopSortR').addClass('downArrow');
        sortedJsonRes.d.sort(function (obj1, obj2) {

            return obj1.connection < obj2.connection ? -1 :
           (obj1.connection > obj2.connection ? 1 : 0);
        });
    }
    else {
        flagStopR = 0;
        $('#StopSortR').addClass('upArrow');
        sortedJsonRes.d.sort(function (obj1, obj2) {
            return parseInt(obj2.FinalFare) - parseInt(obj1.FinalFare);

        });
        }
    
    return sortedJsonRes
    chkunchk();
}
window.ReturnStop = ReturnStop;

function getSortedDetails(sortF, SortCriteria,OR) {

    var msg = sortF
    var sortedmsg;
 
    if (OR == 'I') {
        if (SortCriteria == 'PRICE') {


            sortedmsg = OnwardPrice(msg);
            flagSort = 'PRICE';


        }
        else if (SortCriteria == 'STOP') {
            sortedmsg = OnwardStop(msg);
            flagSort = 'STOP';
        }
        else if (SortCriteria == 'FLIGHT') {
            sortedmsg = OnwardAirline(msg);
            flagSort = 'FLIGHT';
        }
        else if (SortCriteria == 'ARRIVE') {
            sortedmsg = OnwardArrive(msg);
            flagSort = 'ARRIVE';
        }
        else if (SortCriteria == 'DEPART') {
            sortedmsg = OnwardDepart(msg);
            flagSort = 'DEPART';
        }

        else {

            sortedmsg = msg;
            flagSort = 'N';
        }
    }
    else {
        
        if (SortCriteria == 'PRICE') {

            sortedmsg = ReturnPrice(msg);
            flagSort = 'PRICE';

        }
        else if (SortCriteria == 'STOP') {
            sortedmsg = ReturnStop(msg);
            flagSort = 'STOP';
        }
        else if (SortCriteria == 'FLIGHT') {
            sortedmsg = ReturnAirline(msg);
            flagSort = 'FLIGHT';
        }
        else if (SortCriteria == 'ARRIVE') {
            sortedmsg = ReturnArrive(msg);
            flagSort = 'ARRIVE';
        }
        else if (SortCriteria == 'DEPART') {
            sortedmsg = ReturnDepart(msg);
            flagSort = 'DEPART';
        }

        else {

            sortedmsg = msg;
            flagSort = 'N';
        }
    }
    
    groupByFlightNumber = groupJson(sortedmsg);
    return groupByFlightNumber;
}
window.getSortedDetails = getSortedDetails;

function switchTab(tab) {
    var inboundTab = document.querySelector('.inbound');
    var outboundTab = document.querySelector('.outbound');
    var inboundContent = document.querySelector('#inbound-content');
    var outboundContent = document.querySelector('#outbound-content');

    inboundTab.classList.remove('active');
    outboundTab.classList.remove('active');
    inboundContent.classList.remove('active');
    outboundContent.classList.remove('active');

    tab.classList.add('active');
    if (tab.classList.contains('inbound')) {
        inboundContent.classList.add('active');
    } else {
        outboundContent.classList.add('active');
    }
}
window.switchTab = switchTab;
