function checkValidDate(dateStr) {
  // dateStr must be of format month day year with either slashes
  // or dashes separating the parts. Some minor changes would have
  // to be made to use day month year or another format.
  // This function returns True if the date is valid.

  var slash1 = dateStr.indexOf("/");
  if (slash1 == -1) {
    slash1 = dateStr.indexOf("-");
  }
  // if no slashes or dashes, invalid date
  if (slash1 == -1) {
    return false;
  }
  var dateDay = dateStr.substring(0, slash1);
  var dateMonthAndYear = dateStr.substring(slash1 + 1, dateStr.length);
  var slash2 = dateMonthAndYear.indexOf("/");
  if (slash2 == -1) {
    slash2 = dateMonthAndYear.indexOf("-");
  }
  // if not a second slash or dash, invalid date
  if (slash2 == -1) {
    return false;
  }
  var dateMonth = dateMonthAndYear.substring(0, slash2);
  var dateYear = dateMonthAndYear.substring(
    slash2 + 1,
    dateMonthAndYear.length
  );
  if (dateMonth == "" || dateDay == "" || dateYear == "") {
    return false;
  }
  // if any non-digits in the month, invalid date
  for (var x = 0; x < dateMonth.length; x++) {
    var digit = dateMonth.substring(x, x + 1);
    if (digit < "0" || digit > "9") {
      return false;
    }
  }
  // convert the text month to a number
  var numMonth = 0;
  for (var x = 0; x < dateMonth.length; x++) {
    digit = dateMonth.substring(x, x + 1);
    numMonth *= 10;
    numMonth += parseInt(digit);
  }
  if (numMonth <= 0 || numMonth > 12) {
    return false;
  }
  // if any non-digits in the day, invalid date
  for (var x = 0; x < dateDay.length; x++) {
    digit = dateDay.substring(x, x + 1);
    if (digit < "0" || digit > "9") {
      return false;
    }
  }
  // convert the text day to a number
  var numDay = 0;
  for (var x = 0; x < dateDay.length; x++) {
    digit = dateDay.substring(x, x + 1);
    numDay *= 10;
    numDay += parseInt(digit);
  }
  if (numDay <= 0 || numDay > 31) {
    return false;
  }
  // February can't be greater than 29 (leap year calculation comes later)
  if (numMonth == 2 && numDay > 29) {
    return false;
  }
  // check for months with only 30 days
  if (numMonth == 4 || numMonth == 6 || numMonth == 9 || numMonth == 11) {
    if (numDay > 30) {
      return false;
    }
  }
  // if any non-digits in the year, invalid date
  for (var x = 0; x < dateYear.length; x++) {
    digit = dateYear.substring(x, x + 1);
    if (digit < "0" || digit > "9") {
      return false;
    }
  }
  // convert the text year to a number
  var numYear = 0;
  for (var x = 0; x < dateYear.length; x++) {
    digit = dateYear.substring(x, x + 1);
    numYear *= 10;
    numYear += parseInt(digit);
  }
  // Year must be a 2-digit year or a 4-digit year
  if (dateYear.length != 2 && dateYear.length != 4) {
    return false;
  }
  // if 2-digit year, use 50 as a pivot date
  if (numYear < 50 && dateYear.length == 2) {
    numYear += 2000;
  }
  if (numYear < 100 && dateYear.length == 2) {
    numYear += 1900;
  }
  if (numYear <= 0 || numYear > 9999) {
    return false;
  }
  // check for leap year if the month and day is Feb 29
  if (numMonth == 2 && numDay == 29) {
    var div4 = numYear % 4;
    var div100 = numYear % 100;
    var div400 = numYear % 400;
    // if not divisible by 4, then not a leap year so Feb 29 is invalid
    if (div4 != 0) {
      return false;
    }
    // at this point, year is divisible by 4. So if year is divisible by
    // 100 and not 400, then it's not a leap year so Feb 29 is invalid
    if (div100 == 0 && div400 != 0) {
      return false;
    }
  }
  // date is valid
  return true;
}
window.checkValidDate = checkValidDate;
function validateDate() {
  var date = document.getElementById("txtdate").value;
  var result = checkValidDate(date);
  var getageToday = getAge(date);
}
function getAge(dateString) {
  var _arrDate = dateString.split("/");
  var Newdate = _arrDate[1] + "/" + _arrDate[0] + "/" + _arrDate[2];
  var today = new Date();
  var birthDate = new Date(Newdate);
  var age = today.getFullYear() - birthDate.getFullYear();
  var m = today.getMonth() - birthDate.getMonth();
  if (m < 0 || (m === 0 && today.getDate() < birthDate.getDate())) {
    age--;
  }
  return age;
}
window.getAge = getAge;
//$(document).ready(function($){

//        $("#txtdate1").mask("99/99/9999", { placeholder: "mm/dd/yyyy" });
//        $("#txtdate2").mask("99/99/9999", { placeholder: "mm/dd/yyyy" });
//        $("#txtdate3").mask("99/99/9999", { placeholder: "mm/dd/yyyy" });
//        $("#txtdate4").mask("99/99/9999", { placeholder: "mm/dd/yyyy" });
//        $("#txtdate5").mask("99/99/9999", { placeholder: "mm/dd/yyyy" });
//        $("#txtdate6").mask("99/99/9999", { placeholder: "mm/dd/yyyy" });
//        $("#txtdate7").mask("99/99/9999", { placeholder: "mm/dd/yyyy" });
//        $("#txtdate8").mask("99/99/9999", { placeholder: "mm/dd/yyyy" });
//        $("#txtdate9").mask("99/99/9999", { placeholder: "mm/dd/yyyy" });

//    });
