
function updatetxt(abcd) {

    var bankName = $('#bankName').val(); // Get the bank name
    var bankLogoCode = $('#bankLogoCode').val(); // Get the bank logo code

    // Do something with the values, e.g., display them
    document.getElementById('updateclick').accessKey = abcd.id;
    document.getElementById('updateclick').style.display = 'block';
    document.getElementById("sucmsg2").innerHTML = "";
    document.getElementById("errmsg2").innerHTML = "";
    var data = abcd.accessKey.split(",");
    updbankcode.value = data[0];
    //updtddlbnk.value = data[1];
    updbranchnm.value = data[2];
    updaccno.value = data[3];
    var acc = document.getElementById("updaccno").value;
    var banklogoUpdatedCode = data[data.length - 1];
    var bankNameUpdated = data[1];
    
    $('#bankDropdown').val(banklogoUpdatedCode);

    $('#BankLogoCode').val(banklogoUpdatedCode);

    $('#BankName').val(bankNameUpdated);

    $('#UpdatedBankId').val(data[10]);

    updb2b.checked = true;

    document.getElementById('BankNameUpdated').value = bankNameUpdated;
    document.getElementById('BankLogoCodeUpdated').value = banklogoUpdatedCode;

    // var bankName = data[1]; // Get bank name from data[1]
    // var bankLogoCode = data[10]; // Get bank logo code from data[10]

    //updbankname.value = data[1];
    //document.getElementById("ContentPlaceHolder1_updtddlbnk").value = data[11];
    //document.getElementById("bnkid").value = data[10];

    if (data[4] == "True") {
        updstatus.checked = true;

    }
    else {
        updstatus.checked = false;
    }
    if (data[5] == "True") {
        updb2b.checked = true;

    }
    else {
        updb2b.checked = false;
    }
    if (data[6] == "True") {
        updb2c.checked = true;
    }
    else {
        updb2c.checked = false;
    }
    if (data[8] == "True") {
        updb2b2b.checked = true;
    }
    else {
        updb2b2b.checked = false;
    }
    if (data[9] == "True") {
        updb2b2c.checked = true;
    }
    else {
        updb2b2c.checked = false;
    }
    if (data[7] == "True") {
        updd2b.checked = true;
    }
    else {
        updd2b.checked = false;
    }

    $('#bankDropdown').on('change', function () {
     $('#BankLogoCode').val(this.value);
    // $('select option:selected').text(); 
    // console.log($('#bankDropdown option:selected').text());
    $('#BankName').val($('#bankDropdown option:selected').text());

    var selectedBankName = $('#bankDropdown option:selected').text();
    var selectedBankLogoCode = this.value;

    // Set the hidden fields with the selected values
    document.getElementById('BankNameUpdated').value = selectedBankName;
    document.getElementById('BankLogoCodeUpdated').value = selectedBankLogoCode;
    }
    ); 
}

window.updatetxt = updatetxt;

function closebtn() {
    document.getElementById('updateclick').style.display = 'none';
    hidespanerr();

}
window.closebtn = closebtn;
function addnew() {
    //document.getElementById('addnewtxt').accessKey = abcede.id;
    document.getElementById('addnewtxt').style.display = 'block';
    document.getElementById("sucmsg2").innerHTML = "";
    document.getElementById("errmsg2").innerHTML = "";
      // Add hidden input fields for Add New dynamically
      if (!$('#Add_BankName').length) { // Check if the field doesn't exist already
        $('#AddBankDetailForm').append('<input type="hidden" id="Add_BankName" name="NewBankDetail.BankName" />');
        $('#AddBankDetailForm').append('<input type="hidden" id="Add_BankLogoCode" name="NewBankDetail.BankLogoCode" />');
    }

}
window.addnew = addnew;


function closebtn1() {
    document.getElementById('addnewtxt').style.display = 'none';

    document.getElementById('Spanbanklog').innerText = "";
    // clear textbox and uncheck check box Ajay

    $("#addnewtxt").find("input:text").val('');
    $('#addnewtxt').find('input[type=checkbox]:checked').removeAttr('checked');
    hidespanerr11();
}

window.closebtn1 = closebtn1;

var tabledatalength = document.getElementById('bankCount').value;
var countalways = document.getElementById('bankCount').value;

var specialKeys = new Array();
specialKeys.push(8); //Backspace
specialKeys.push(9); //Tab
specialKeys.push(46); //Delete
specialKeys.push(36); //Home
specialKeys.push(35); //End
specialKeys.push(37); //Left
specialKeys.push(39); //Right
function IsAlphaNumeric(e) {

    var id = e.currentTarget.id;
    $('#' + id + '').removeClass("wrong").addClass("right");
    var keyCode = e.keyCode == 0 ? e.charCode : e.keyCode;
    var ret = ((keyCode >= 48 && keyCode <= 57) || (keyCode >= 65 && keyCode <= 90) || (keyCode >= 97 && keyCode <= 122) || (specialKeys.indexOf(e.keyCode) != -1 && e.charCode != e.keyCode));
    document.getElementById("error").style.display = ret ? "none" : "inline";
    document.getElementById("spclchar").style.display = ret ? "none" : "inline";

    return ret;
}
window.IsAlphaNumeric = IsAlphaNumeric;
function sendFile(abc) {
    var formData = new FormData();

    formData.append('file', $('#' + abc + '')[0].files[0]);
    var hostnm = $('#ContentPlaceHolder1_hdnhostName').val();

    var other_data = $('form').serializeArray();
    formData.append("txtImageName", "Ajay");
    $.ajax({
        type: 'post',
        url: 'http://' + hostnm + '/Upload_Image.ashx',
        data: formData,
        async: false,
        success: function (status) {
            if (status != 'error') {

            }
        },
        processData: false,
        contentType: false,
        error: function () {
            alert("Whoops something went wrong to upload your logo. Kindly call to support team!");
        }
    });
}

window.sendFile = sendFile;



function addnewbank() {


    document.getElementById("sucmsg2").innerHTML = "";
    document.getElementById("errmsg2").innerHTML = "";
    var addbankd = document.getElementById("addbankcode").value;
    if (addbankd == null || addbankd == "") {
        $("#addbankcode").addClass("wrong");
        document.getElementById("spadbnk").innerHTML =
            "Bank Code is required";
        return false;
    }

    var selbank = document.getElementById("ContentPlaceHolder1_ddlbnknmcode").value;
    if (selbank == null || selbank == "") {
        $("#ContentPlaceHolder1_ddlbnknmcode").addClass("wrong");
        document.getElementById("sspnbnk").innerHTML =
            "Bank Name is required";
        return false;
    }

    var upaccno = document.getElementById("addaccno").value;
    if (upaccno == null || upaccno == "") {
        $("#addaccno").addClass("wrong");
        document.getElementById("adaccnoum").innerHTML =
            "Account Number  is required";
        return false;
    }
    var upbranch = document.getElementById("txtbranchnm").value;
    if (upbranch == null || upbranch == "") {
        $("#txtbranchnm").addClass("wrong");
        document.getElementById("Spanbranchnm").innerHTML =
            "Branch Name is required";
        return false;
    }
    var bnkname = $("#ContentPlaceHolder1_ddlbnknmcode option:selected").text();
    var logo = $("#ContentPlaceHolder1_ddlbnknmcode option:selected").val();


    var yroom = 1, user = {}, ucpol = "";
    try {
        user.msgQual = addbankcode.value + "," + bnkname + "," + txtbranchnm.value + "," + addaccno.value + "," + adddb2b.checked + "," + addb2c.checked + "," + addb2b2b.checked + "," + adddb2b2c.checked + "," + addddb2b2c.checked + "," + adddstausus.checked + "," + logo;
        ucpol = $.ajax({ type: "POST", url: "/admin/AddBankDetail", async: false, data: '{user: ' + JSON.stringify(user) + '}', contentType: "application/json; charset=utf-8", dataType: "json", success: function (response) { } });

        var totaldata = JSON.parse(ucpol.responseText).d.split(',');
        if (totaldata == 'Success') {
            var htmldata = "";
            var lengthdata = tabledatalength;
            tabledatalength++;
            countalways++;
            var dataa = user.msgQual;
            htmldata += "<tr id='tablerowdata_" + lengthdata + "'><td class='ui-border-wo-topleft' align='center' style='border-left:1px solid #ccc;'> " + (lengthdata + 1) + "</td>";
            htmldata += "<td id='Banlog_" + lengthdata + "' class='ui-border-wo-topleft' align='center'><img id='imglo_" + lengthdata + "' src='/assets/img/BankLogo/" + logo + ".png' style='height:25px; width:75px;'/></td>";

            htmldata += " <td id='BankName_" + lengthdata + "' class='ui-border-wo-topleft' align='center' > " + bnkname + "</td>";
            htmldata += "<td id='Accountno_" + lengthdata + "' class='ui-border-wo-topleft' align='center'> " + addaccno.value + "</td>";
            htmldata += "<td id='Branch_" + lengthdata + "' class='ui-border-wo-topleft' align='center'> " + txtbranchnm.value + "</td>";

            htmldata += "<td id='Bankcode_" + lengthdata + "' class='ui-border-wo-topleft' align='center'> " + addbankcode.value + "</td>";

            htmldata += "<td class='ui-border-wo-topleft'>";
            htmldata += "<a id='test_" + lengthdata + "'   accesskey='" + dataa + "' class='txt_blue' onclick='updatetxt(this)'>Update</a>&nbsp;</td>";
            htmldata += " <td class='ui-border-wo-topleft'><a id='test12_" + lengthdata + "'class='txt_blue' accesskey='" + dataa + "' onclick='deletetxt(this)'>Delete</a>&nbsp;</td></tr>";
            $('#tabledata').append(htmldata);
            $('#sucmsg1').append(totaldata[0]);
            $('#sucmsg1').css("display", "block");
            setTimeout(function () { closebtn1(); }, 3000);
        }

        else {
            $('#errmsg1').append(totaldata[0]);
            $('#errmsg1').css("display", "block")
        }

        addbankcode.value = "";
        addaccno.value = "";
        txtbranchnm.value = "";
    }

    catch (e) {
    }
    if (countalways > 0) {
        document.getElementById('Nodatatadiv').style.display = "none";
    }
    else {
        document.getElementById('Nodatatadiv').style.display = "block";
    }
}

window.addnewbank = addnewbank;
function deletetxt(abcede) {

    // document.getElementById("sucmsg2").innerHTML = "";
    // document.getElementById("errmsg2").innerHTML = "";

    var vret = false;
    var data = abcede.accessKey.split(",");
    {
        vret = confirm('Delete ' + data[1] + '?')
    }

    if (vret == true) {
        try {
            // Send the Id directly in the AJAX request
            $.ajax({
                type: "POST",
                url: "/admin/DeleteBankDetail",  // Replace with your actual URL
                async: false,
                data: JSON.stringify({ Id: data[10] }),  // Only send the bank Id
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    // Assuming the response contains a success flag or message
                    var totaldata = JSON.parse(response.d).split(',');
                    if (totaldata == 'Success') {
                        // Remove the row from the table
                        $("#tablerowdata_" + abcede.id.split('_')[1]).remove();
                        countalways--;  // Update count
                        $('#sucmsg2').append("Deleted successfully");
                        $('#sucmsg2').css("display", "block");
                        
                    } else {
                        $('#errmsg2').append(totaldata[0]);
                        $('#errmsg2').css("display", "block");
                    }
                },
                error: function (error) {
                    console.error(error);
                    // $('#errmsg2').append("Error deleting bank");
                    $('#errmsg2').css("display", "block");
                }
            });
        } catch (e) {
            console.error(e);
        }
        var messageDiv = document.getElementById('message');
        messageDiv.style.display = 'block';
        setTimeout(function() {
            location.reload();
        }, 500);
    }
}

window.deletetxt = deletetxt;

function LoadScript() {


    $(function () {
        $('#addbnkname').keydown(function (e) {

            var id = e.currentTarget.id;
            if (e.shiftKey || e.ctrlKey || e.altKey) {


                e.preventDefault();
            } else {
                var key = e.keyCode;
                if (!((key == 8) || (key == 32) || (key == 46) || (key >= 35 && key <= 40) || (key >= 65 && key <= 90))) {


                    $('#' + id + '').addClass("wrong");
                    e.preventDefault();
                }
                else {
                    $('#' + id + '').removeClass("wrong").addClass("right");
                }
            }
        });
        $('#updbankname').keydown(function (e) {

            var id = e.currentTarget.id;
            if (e.shiftKey || e.ctrlKey || e.altKey) {
                e.preventDefault();
            } else {
                var key = e.keyCode;
                if (!((key == 8) || (key == 32) || (key == 46) || (key >= 35 && key <= 40) || (key >= 65 && key <= 90))) {
                    $('#' + id + '').addClass("wrong");
                    e.preventDefault();
                }
                else {
                    $('#' + id + '').removeClass("wrong").addClass("right");
                }

            }
        });
        $('#updaccno').keydown(function (e) {
            var id = e.currentTarget.id;
            if (e.shiftKey || e.ctrlKey || e.altKey) {
                e.preventDefault();
            } else {
                var key = e.keyCode;
                if (!((key == 8) || (key == 46) || (key >= 35 && key <= 40) || (key >= 48 && key <= 57) || (key >= 96 && key <= 105))) {
                    $('#' + id + '').addClass("wrong");
                    e.preventDefault();
                }
                else {
                    $('#' + id + '').removeClass("wrong").addClass("right");
                }
            }
        });
        $('#addaccno').keydown(function (e) {

            var id = e.currentTarget.id;
            if (e.shiftKey || e.ctrlKey || e.altKey) {


                e.preventDefault();
            } else {
                var key = e.keyCode;
                if (!((key == 8) || (key == 46) || (key >= 35 && key <= 40) || (key >= 48 && key <= 57) || (key >= 96 && key <= 105))) {

                    $('#' + id + '').addClass("wrong");
                    e.preventDefault();
                }
                else {
                    $('#' + id + '').removeClass("wrong").addClass("right");
                }

            }
        });

    });
}

window.LoadScript = LoadScript;
function hidespanerr() {

    document.getElementById("spadbnk").innerHTML = "";
    document.getElementById("spnaddbkname").innerHTML = "";
    document.getElementById("adaccnoum").innerHTML = "";
    document.getElementById("Spanbranchnm").innerHTML = "";
    document.getElementById("sucmsg").innerHTML = "";
    document.getElementById("sucmsg1").innerHTML = "";
    document.getElementById("errmsg").innerHTML = "";
    document.getElementById("errmsg1").innerHTML = "";
    document.getElementById("error").innerHTML = "";
    document.getElementById("Spanbanklog").innerHTML = "";


}
window.hidespanerr = hidespanerr;
function hidespanerr11() {
    document.getElementById("updatebankmsg").innerHTML = "";
    document.getElementById("updatebnknmmsg").innerHTML = "";
    document.getElementById("updaccno1").innerHTML = "";
    document.getElementById("Spanupdbrnchnm").innerHTML = "";
    document.getElementById("sucmsg").innerHTML = "";
    document.getElementById("sucmsg1").innerHTML = "";
    document.getElementById("errmsg").innerHTML = "";
    document.getElementById("errmsg1").innerHTML = "";
    document.getElementById("error").innerHTML = "";
    document.getElementById("SpanUploadLog").innerHTML = "";


}
window.hidespanerr11 = hidespanerr11;

$(document).ready(function () {
    // Function to fetch and populate the bank dropdown
    function loadBanks() {
        // Call the backend API to get all banks
        $.ajax({
            url: '/Admin/GetAllBanks', // The URL to your backend API
            type: 'GET',
            success: function (data) {
                var bankDropdown = $('#bankDropdown');
                bankDropdown.empty(); // Clear existing options
                bankDropdown.append('<option value="">Select Bank</option>'); // Add default option

                // Store the previously selected bank value (if any)
                var selectedBankCode = bankDropdown.data('selected-bank'); // Assuming you store it in data attribute

                // Loop through the data and populate the dropdown
                if (data && data.length > 0) {
                    $.each(data, function (index, bank) {
                        var selected = (selectedBankCode === bank.bankCode) ? 'selected' : ''; // Check if this bank is selected
                        bankDropdown.append('<option value="' + bank.bankCode + '" ' + selected + '>' + bank.bankName + '</option>');
                    });
                }
                if (selectedBankCode) {
                    var selectedBank = data.find(bank => bank.bankCode === selectedBankCode);
                    if (selectedBank) {
                        $('#BankName').val(selectedBank.bankName); // Set the selected bank name
                        // Optional: Set the bank logo if you have a logo field
                        // $('#bankLogoImage').attr('src', '/assets/img/BankLogo/' + selectedBank.bankLogoCode + '.png');
                    }
                }
                else {
                    bankDropdown.append('<option value="">No Banks Available</option>');
                }
            },
            error: function (jqXHR, textStatus, errorThrown) {
                alert("Error loading banks");
            }
        });
    }
    loadBanks();

    function loadBanksSelect() {
        // Call the backend API to get all banks
        $.ajax({
            url: '/Admin/GetAllBanks', // The URL to your backend API
            type: 'GET',
            success: function (data) {
                var bankDropdownSelect = $('#bankDropdownSelect');
                bankDropdownSelect.empty(); // Clear existing options

                // Get the previously selected bank code from the hidden field or data attribute
                var selectedBankCode = bankDropdownSelect.data('selected-bank') || $('#BankName').val(); // Check hidden field or data attribute

                // Default option when no bank is selected
                bankDropdownSelect.append('<option value="">Select Bank</option>');

                // Loop through the data and populate the dropdown
                if (data && data.length > 0) {
                    $.each(data, function (index, bank) {
                        // Check if this bank is selected
                        var selected = (selectedBankCode === bank.bankCode) ? 'selected' : '';
                        bankDropdownSelect.append('<option value="' + bank.bankCode + '" data-name="' + bank.bankName + '" data-code="' + bank.bankCode + '" ' + selected + '>' + bank.bankName + '</option>');
                    });

                    // If a bank is already selected, set the hidden fields
                    if (selectedBankCode) {
                        var selectedBank = data.find(bank => bank.bankCode === selectedBankCode);
                        if (selectedBank) {
                            $('#BankName').val(selectedBank.bankName); // Set the selected bank name
                            // Optional: Set the bank logo if you have a logo field
                            // $('#bankLogoImage').attr('src', '/assets/img/BankLogo/' + selectedBank.bankLogoCode + '.png');
                        }
                    }
                } else {
                    bankDropdownSelect.append('<option value="">No Banks Available</option>'); // Handle no banks scenario
                }
            },
            error: function (jqXHR, textStatus, errorThrown) {
                alert("Error loading banks");
            }
        });
    }

    $(document).ready(function () {
        // Update hidden fields when the user selects a bank
        $('#bankDropdownSelect').change(function () {
            var selectedOption = $(this).find('option:selected');
            var bankName = selectedOption.data('name'); // Get the bank name from data attribute
            var bankCode = selectedOption.data('code'); // Get the bank code from data attribute

            // Set the hidden fields with the selected bank details
            $('#BankLogoCode').val(bankCode);
            $('#BankName').val(bankName);

        });

        loadBanksSelect(); // Initialize the dropdown
    });

});