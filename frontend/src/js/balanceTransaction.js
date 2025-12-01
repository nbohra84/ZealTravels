function LoadScript() {
    //$(document).ready(function () {

    try {
        if ($("#ContentPlaceHolder1_companyname").val().length > 0) {
            $('#hiddenonload').show();
        }
        else {
            $('#hiddenonload').hide();
        }
        $("#form1").validationEngine('attach', { promptPosition: "topRight" });
    }
    catch (e) { }
    // $('body').attr('onload', 'checkload();');
    $('[id*=ContentPlaceHolder1_update]').bind("click", function () {

        $("table[id*=radiopaymentfor] input").removeClass("validate[required]");
        if (!$("[id*=form1]").validationEngine('validate')) {
            return false
        }
        else {
            if (parseInt($("#ContentPlaceHolder1_Txtbookinref").val()) > 0) {

                $("table[id*=radiopaymentfor] input").addClass("validate[required]");
                if (!$("[id*=form1]").validationEngine('validate')) {
                    return false
                }
            }
        }
        $("#form1").validationEngine('attach', { promptPosition: "topRight" });
    });
    $("#ContentPlaceHolder1_companyname").autocomplete({
        source: function (request, response) {
            $("#tosectordiv1").css("display", "none");
            console.log("running");

            $.ajax({
                url: '/agency/searchAgency',
                data: JSON.stringify({ srchtxt: request.term }), // Using JSON.stringify for correct data format
                dataType: "json",
                type: "POST",
                contentType: "application/json; charset=utf-8",
                success: function (data) {
                    // Check if data is empty and show a "No results found" message if true
                    if (data.length === 0) {
                        response([{
                            label: "No results found",
                            value: ""
                        }]);
                    } else {
                        response($.map(data, function (item) {
                            return {
                                label: item,  // Use the proper field based on your server response
                                value: item
                            };
                        }));
                    }
                },
                error: function (xhr, status, error) {
                    console.error("Error during autocomplete request: " + error);
                }
            });
        },
        minLength: 3,
        selectFirst: true,
        highlight: true,
        autoFill: true,
        cacheLength: 10,
        autoFocus: true,
        select: function (event, ui) {
            if ($("#ContentPlaceHolder1_companyname").val() != "") {
                if (ui.item.label == "No results found") {
                    $('#hiddenonload').hide();
                } else {
                    $('#hiddenonload').show();
                    var val = ui.item.label;
                    clearLabelValue();
                    custregb(val);
                }
            }
        },
        open: function () {
            $(this).removeClass("ui-corner-all").addClass("ui-corner-top");
        },
        close: function () {
            $(this).removeClass("ui-corner-top").addClass("ui-corner-all");
        },
        search: function () {
            $(this).addClass('p_loader');
        },
        response: function () {
            $(this).removeClass('p_loader');
        }
    });

    //});
}
window.LoadScript = LoadScript;

$(document).ready(function () {
    // On input event for the company name input field
    $('#ContentPlaceHolder1_companyname').on('input', function () {
        var searchText = $(this).val();

        // If the search text has 3 or more characters, trigger the AJAX search
        if (searchText.length >= 3) {
            $.ajax({
                url: '/agency/searchAgency',
                type: 'GET',
                data: { searchText: searchText },
                success: function (data) {
                    if (Array.isArray(data) && data.length > 0) {
                        var resultsHtml = '';
                        data.forEach(function (agency) {
                            resultsHtml += '<li class="ui-menu-item" role="menuitem"><a class="searchResults ui-corner-all" tabindex="-1">' + agency + '</a></li>';
                        });
                        $('.ui-autocomplete').html(resultsHtml);
                        $('.ui-autocomplete').addClass('show');
                        $('.ui-autocomplete a').addClass('result-active-item');
                    } else {
                        $('.ui-autocomplete').html('<li class="ui-menu-item" role="menuitem"><a class="searchResults ui-corner-all" tabindex="-1">No agencies found.</a></li>');
                        $('.ui-autocomplete').addClass('show');
                        $('.ui-autocomplete a').addClass('result-active-item');
                    }
                },
                error: function () {
                    $('.ui-autocomplete').html('<li class="ui-menu-item" role="menuitem"><a class="searchResults ui-corner-all" tabindex="-1">An error occurred while fetching agencies.</a></li>');
                    $('.ui-autocomplete').addClass('show');
                    $('.ui-autocomplete a').addClass('result-active-item');
                }
            });
        } else {
            $('.ui-autocomplete').removeClass('show');
            $('.ui-autocomplete a').removeClass('result-active-item');
        }
    });

    // Click event to populate the input field with selected agency and show #hiddenonload
    $(document).on('click', '.ui-menu-item a', function () {
        var selectedAgency = $(this).text();  // Get the text of the clicked suggestion

        // Populate the input field with the selected agency name
        $('#ContentPlaceHolder1_companyname').val(selectedAgency);

        // Show the #hiddenonload element when a suggestion is clicked
        $('#hiddenonload').show();

        // Hide the autocomplete list after selection
        $('.ui-autocomplete').removeClass('show');
        $('.ui-autocomplete a').removeClass('result-active-item');

        // Trigger change event to get the balance after selection
        $('#ContentPlaceHolder1_companyname').trigger('change');
    });

    // Additional initialization to attach validation engine
    try {
        $("#form1").validationEngine('attach', { promptPosition: "topRight" });
    } catch (e) { }

    // If there is some value in the field on page load, show #hiddenonload
    if ($("#ContentPlaceHolder1_companyname").val().length > 0) {
        $('#hiddenonload').show();
    } else {
        $('#hiddenonload').hide();
    }
});

$("#ContentPlaceHolder1_companyname").on("change", function () {
    var companyName = $(this).val();
    if (companyName.trim() !== "") {
        $.ajax({
            url: "/agency/getcompanybalancebyaccountid",  // Your route for balance
            type: "GET",
            data: { companyNameWithAccountId: companyName },  // Send the company name with account id
            success: function (response) {
                if (response.availableBalance !== undefined) {
                    $("#totbal").text(response.availableBalance);  // Update the balance display
                }
            },
            error: function () {
                $("#totbal").text("Error fetching balance");  // Display error if AJAX call fails
            }
        });
    }
});

function checkload() {

    document.getElementById('cash').checked = "true";
    // $('#hiddenonload').show();
}
function valueChanged() {
    document.getElementById("credit").checked = true;
    document.getElementById("cash").checked = false;
};
function valueChanged1() {
    document.getElementById("credit").checked = false;
    document.getElementById("cash").checked = true;
};

window.checkload = checkload;
window.valueChanged = valueChanged;
window.valueChanged1 = valueChanged1;

function custregb(val) {

    // $("#txtEmail").val()
    // var srct = $("#companyname").val()
    var yroom = 1, user = {}, ucpol = "";
    try {
        var abcder = val.split('[');
        var xxxxxx = abcder[1].split(']');
        user.msgQual = xxxxxx[0];
        ucpol = $.ajax({ type: "POST", url: "/agency/balancetransaction", async: false, data: '{user: ' + JSON.stringify(user) + '}', contentType: "application/json; charset=utf-8", dataType: "json", success: function (response) { } });
        var totaldata = JSON.parse(ucpol.responseText).d.split(',');
        // $('#totbal').val = totaldata[0];
        $('#ContentPlaceHolder1_totbal').val = "";
        document.getElementById("ContentPlaceHolder1_totbal").innerHTML = totaldata[0];
        document.getElementById("<%= hdnbalance.ClientID %>").value = totaldata[0];
        // $('#totbal').append(totaldata[0]);
    }
    catch (e) {
    }
}
window.custregb = custregb;

function clearLabelValue() {
    var labelObj = $('#ContentPlaceHolder1_totbal');
    labelObj.val = "";
}
window.clearLabelValue = clearLabelValue;
$("checked").attr("disabled", "disabled");
$("credit").attr("disabled", "disabled");

$(document).ready(function () {
    $('#TransactionType').change(function () {
        var selectedType = $(this).val();
        if (selectedType) {
            $.ajax({
                url: '/agency/GetTransactionEvents',
                type: 'POST',
                contentType: 'application/json',
                dataType: 'json', // Ensure correct response format
                data: JSON.stringify(selectedType), // Pass the string directly
                success: function (data) {
                    if (data && Array.isArray(data.eventDetails) && data.eventDetails.length > 0) {
                        var eventDropdown = $('#EventId');
                        eventDropdown.empty();

                        var firstEvent = data.eventDetails[0];
                        eventDropdown.append($('<option>', {
                            value: firstEvent.eventId,
                            text: firstEvent.eventName,
                            selected: true
                        }));

                        for (var i = 1; i < data.eventDetails.length; i++) {
                            var event = data.eventDetails[i];
                            eventDropdown.append($('<option>', {
                                value: event.eventId,
                                text: event.eventName
                            }));
                        }
                    } else {
                        $('#EventId').empty();
                        $('#EventId').append($('<option>', { value: "", text: "No events available" }));
                    }
                },
                error: function (error) {
                    console.log("Error retrieving event details:", error);
                    alert('Error retrieving event details: ' + error.responseText);
                }
            });

        } else {
            $('#EventId').empty();
        }
    });
});


$(document).ready(function () {
    // Initialize script
    try {
        // Handle showing hidden elements based on company name value
        if ($("#ContentPlaceHolder1_companyname").val().length > 0) {
            $('#hiddenonload').show();
        } else {
            $('#hiddenonload').hide();
        }

        // Form validation
        $("#form1").validationEngine('attach', { promptPosition: "topRight" });

        // Handle form submission
        $('[id$=ContentPlaceHolder1_update]').bind("click", function () {

            $("table[id*=radiopaymentfor] input").removeClass("validate[required]");
            if (!$("[id*=form1]").validationEngine('validate')) {
                return false;
            } else {
                if (parseInt($("#ContentPlaceHolder1_Txtbookinref").val()) > 0) {
                    $("table[id*=radiopaymentfor] input").addClass("validate[required]");
                    if (!$("[id*=form1]").validationEngine('validate')) {
                        return false;
                    }
                }
            }
            $("#form1").validationEngine('attach', { promptPosition: "topRight" });
        });

        // Autocomplete for company name input
        $("#ContentPlaceHolder1_companyname").autocomplete({
            source: function (request, response) {
                $("#tosectordiv1").css("display", "none");
                $.ajax({
                    url: '/agency/searchAgency',
                    data: JSON.stringify({ srchtxt: request.term }),
                    dataType: "json",
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    success: function (data) {
                        response($.map(data.d, function (item) {
                            return { label: item, value: item };
                        }));
                    }
                });
            },
            minLength: 3,
            selectFirst: true,
            highlight: true,
            autoFill: true,
            cacheLength: 10,
            autoFocus: true,
            select: function (event, ui) {
                if ($("#ContentPlaceHolder1_companyname").val() !== "") {
                    if (ui.item.label === "No Record Found") {
                        $('#hiddenonload').hide();
                    } else {
                        $('#hiddenonload').show();
                        var val = ui.item.label;
                        clearLabelValue();
                        custregb(val);
                    }
                }
            },
            open: function () {
                $(this).removeClass("ui-corner-all").addClass("ui-corner-top");
            },
            close: function () {
                $(this).removeClass("ui-corner-top").addClass("ui-corner-all");
            },
            search: function () {
                $(this).addClass('p_loader');
            },
            response: function () {
                $(this).removeClass('p_loader');
            }
        });

    } catch (e) {
        console.error("Error in LoadScript: ", e);
    }

    // Additional helper functions

    // Function to clear label value
    function clearLabelValue() {
        $('#ContentPlaceHolder1_totbal').val("");
    }

    // Function to update balance info based on the company name value
    function custregb(val) {
        try {
            var abcder = val.split('[');
            var xxxxxx = abcder[1].split(']');
            var user = { msgQual: xxxxxx[0] };

            $.ajax({
                type: "POST",
                url: "/agency/balancetransaction",
                async: false,
                data: JSON.stringify({ user: user }),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    var totaldata = JSON.parse(response.d).split(',');
                    $('#ContentPlaceHolder1_totbal').html(totaldata[0]);
                    $('#hdnbalance').val(totaldata[0]);
                }
            });
        } catch (e) {
            console.error("Error in custregb: ", e);
        }
    }

    // Set checkboxes (if any are disabled or need initialization)
    $("checked").attr("disabled", "disabled");
    $("credit").attr("disabled", "disabled");
});