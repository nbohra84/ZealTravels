
    // Get the modal
    var modal = document.getElementById('myModal');

    // Get the image and insert it inside the modal - use its "alt" text as a caption
    // var img = document.getElementById('Image1');
    var modalImg = document.getElementById("img01");
    var captionText = document.getElementById("caption");
    $('.iu').on('click', function () {
        debugger;
        modal.style.display = "block";
        var pat = this.src;
        if (pat.includes("CompanyLogo")) {
            if (this.src.split("CompanyLogo")[1].split("/")[1].trim() == "" || this.src.split("CompanyLogo")[1].split("/")[1].trim().toUpperCase() == "NULL") {
                modalImg.src = "/assets/img/Airline/image/not-found.png";
            } else {
                modalImg.src = this.src;
            }
        } else {
            modalImg.src = "/assets/img/Airline/image/not-found.png";
        }

        captionText.innerHTML = this.alt;
    });

    // Get the <span> element that closes the modal
    //  var span = document.getElementsByClassName("close")[0];
    var span = document.getElementById("cros");
    // When the user clicks on <span> (x), close the modal
    span.onclick = function () {
        modal.style.display = "none";
    }

    var close = document.getElementsByClassName("closebtn");
    var i;

    for (i = 0; i < close.length; i++) {
        close[i].onclick = function () {
            debugger;
            var div = this.parentElement;
            div.style.opacity = "0";
            setTimeout(function () { div.style.display = "none"; }, 600);
        }
    }

    function hideonrerr6() {

        document.getElementById("teleerror").innerHTML = "";
    }
    function hideonrerr7() {
        debugger;
        document.getElementById("cmplogo").innerHTML = "";
        // document.getElementById("atleasterror").innerHTML = "";
    }


    function labelhide() {
        debugger;

        document.getElementById('active').innerHTML = "";
        document.getElementById('actdctmsg').innerHTML = "";


    }

    function checkagentlogo(e) {
        var uploadfile10 = document.getElementById('ContentPlaceHolder1_FileUpload1');
        hideonrerr7();
        if (uploadfile10.value == '') {
            document.getElementById("cmplogo").innerHTML = " Please select a images to upload";
            return false;
        }
    }

    window.checkagentlogo = checkagentlogo;

    function abcdef(abcd) {
        debugger;
        var yroom = 1, user = {}, ucpol = {};
        try {
            user.msgQual = ContentPlaceHolder1_State.value;
            ucpol = $.ajax({ type: "POST", url: "Admin_Detail.aspx/Showcity", async: false, data: '{user: ' + JSON.stringify(user) + '}', contentType: "application/json; charset=utf-8", dataType: "json", success: function (response) { } });
            debugger;
            var datas = JSON.parse(ucpol.responseText).d;
            $("#ContentPlaceHolder1_City").empty();
            $("#ContentPlaceHolder1_City").append(datas);
        }
        catch (e) {
        }
    }
    function hdnfldcry() {
        debugger;
        //  $("#loaddd").modal();
        var cityhdn = document.getElementById("ContentPlaceHolder1_City").value;
        ContentPlaceHolder1_hidcity.value = cityhdn;
        var datah = document.getElementById("ContentPlaceHolder1_dop").value;
        ContentPlaceHolder1_hiddop.value = datah;
        labelhide();


    };
    function DateFormat(field, rules, i, options) {
        var regex = /^(0?[1-9]|[12][0-9]|3[01])[\/\-](0?[1-9]|1[012])[\/\-]\d{4}$/;
        if (!regex.test(field.val())) {
            return "Please enter date in dd/MM/yyyy format."
        }
    }

    function LoadScript() {

        $(function () {
            debugger;
            var today = new Date();
            var dd = today.getDate();
            var mm = today.getMonth() + 1; //January is 0!
            var yyyy = today.getFullYear();

            if (dd < 10) {
                dd = '0' + dd
            }

            if (mm < 10) {
                mm = '0' + mm
            }


            today = dd + '/' + mm + '/' + yyyy;
            $('#ContentPlaceHolder1_dop').val(today);
            $('#ContentPlaceHolder1_dop').datepicker({
                minDate: 0, dateFormat: 'dd/mm/yy', numberOfMonths: 1,
                onSelect: function (dateStr) {
                    var d = $.datepicker.parseDate('dd/mm/yy', dateStr);

                }
            });
            $('[id*=profile1]').bind("click", function () {
                debugger;
                //   $("#form1").validationEngine('attach', { promptPosition: "topRight" });
                if (!$("[id*=upprofile]").validationEngine('validate')) {
                    return false
                }
                else {
                    $("#loaddd").modal();
                    return true
                }
                //var valid = $("#form1").validationEngine('validate');
                //if (valid == true) {
                //    $("#loaddd").modal();
                //}
                //else{ return false}
            });
            $('#ContentPlaceHolder1_agname').on('keypress', function (event) {
                debugger;
                var regex = new RegExp("^[a-zA-Z0-9\b ]+$");
                var key = String.fromCharCode(!event.charCode ? event.which : event.charCode);
                if (!regex.test(key)) {
                    event.preventDefault();
                    document.getElementById("agnamsp").innerHTML = "Don't type any special charector";
                    $("#ContentPlaceHolder1_agname").addClass("wrong");

                    // alert("wrong");
                    return false;
                }
                else {
                    document.getElementById("agnamsp").innerHTML = "";
                    $("#ContentPlaceHolder1_agname").removeClass("wrong").addClass("right");
                }
            });
        });
    }
