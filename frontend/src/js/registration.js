
function signIn() {
    document.getElementById('SignIn_cont').style.display = "block";
    $('#f1').css("opacity", "0.1");
    $('#f1').css({ position: 'fixed' });
}

function cl() {

    document.getElementById('SignIn_cont').style.display = "none";
    $('#f1').css("opacity", "1.0");
    $('#f1').css({ position: 'relative' });
}

function less1() {
    document.getElementById('more').style.display = 'block';
    document.getElementById('less').style.display = 'none';
    document.getElementById('more2').style.display = 'block';
    document.getElementById('less2').style.display = 'none';
}
function more1() {
    document.getElementById('more').style.display = 'none';
    document.getElementById('less').style.display = 'block';
    document.getElementById('more2').style.display = 'block';
    document.getElementById('less2').style.display = 'block';
}

function DateFormat(field, rules, i, options) {
    var regex = /^(0?[1-9]|[12][0-9]|3[01])[\/\-](0?[1-9]|1[012])[\/\-]\d{4}$/;
    if (!regex.test(field.val())) {
        return "Please enter date in dd/MM/yyyy format."
    }
}


function IsNumeric(sText) {
    var ValidChars = "0123456789.";
    var IsNumber = true;
    var Char;

    for (i = 0; i < sText.length && IsNumber == true; i++) {
        Char = sText.charAt(i);
        if (ValidChars.indexOf(Char) == -1) {
            IsNumber = false;
        }
    }
    return IsNumber;
}
