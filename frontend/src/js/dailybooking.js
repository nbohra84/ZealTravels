
$(document).ready(function() {
    function LoadScript() {
        var d = new Date();
        var month = d.getMonth() + 1;
        var day = d.getDate();
        var year = d.getFullYear();
        var dateNow = year + "-" + month + "-" + day;
        $(".datetopic").datepicker({
            autoclose: true,
            format: 'yyyy-mm-dd',
            todayHighlight: true,
            orientation: 'bottom',
            defaultViewDate: dateNow
        });
    }
    window.LoadScript = LoadScript;
    LoadScript();
});



function myclipopse() {
debugger;
var ll = document.getElementById('closclippo');

if (ll.style.display != "block") {
ll.style.display = "block";
$('#sedax').css("box-shadow", "0 8px 8px rgba(0,0,0,0.5)");
}
else {
ll.style.display = "none";
$('#sedax').css("box-shadow", "0 0px 0px rgba(0,0,0,0.5)");
}
}


$(document).ready(function () {
var touch = $('#resp-menu');
var menu = $('.menu');

$(touch).on('click', function (e) {
e.preventDefault();
menu.slideToggle();
});

$(window).resize(function () {
var w = $(window).width();
if (w > 767 && menu.is(':hidden')) {
menu.removeAttr('style');
}
}); 
});


 $(document).ready(function () {
     var touch = $('#resp-menu');
     var menu = $('.menu');

     $(touch).on('click', function (e) {
         e.preventDefault();
         menu.slideToggle();
     });

     $(window).resize(function () {
         var w = $(window).width();
         if (w > 767 && menu.is(':hidden')) {
             menu.removeAttr('style');
         }
     }); 
 });



function googleTranslateElementInit() {
new google.translate.TranslateElement({pageLanguage: 'en'}, 'google_translate_element');
}


$(document).ready(function () {
    $('#searchTerm').on('input', function () {
        var searchText = $(this).val();

        if (searchText.length >= 3) {
            $.ajax({
                url: '/agency/searchAgency',
                type: 'GET',
                data: { searchText: searchText },
                success: function (data) {
                    if (Array.isArray(data) && data.length > 0) {
                        var resultsHtml = '';
                        data.forEach(function (agency) {
                            // Append each agency as a list item with the <a> tag inside
                            resultsHtml += '<li class="ui-menu-item" role="menuitem"><a class="searchResults ui-corner-all" tabindex="-1">' + agency + '</a></li>';
                        });
                        // Insert the results into the <ul> element
                        $('.ui-autocomplete').html(resultsHtml);
                        $('.ui-autocomplete').addClass('show');  // Show the <ul> by adding 'show' class
                        // Add class to each <a> tag when the list is shown
                        $('.ui-autocomplete a').addClass('result-active-item');
                    } else {
                        // In case no results, add a message inside the <ul>
                        $('.ui-autocomplete').html('<li class="ui-menu-item" role="menuitem"><a class="searchResults ui-corner-all" tabindex="-1">No agencies found.</a></li>');
                        $('.ui-autocomplete').addClass('show');  // Show the <ul> even if there are no results
                        // Add class to the <a> tag even when there are no results
                        $('.ui-autocomplete a').addClass('result-active-item');
                    }
                },
                error: function () {
                    $('.ui-autocomplete').html('<li class="ui-menu-item" role="menuitem"><a class="searchResults ui-corner-all" tabindex="-1">An error occurred while fetching agencies.</a></li>');
                    $('.ui-autocomplete').addClass('show');  // Show the <ul> in case of error
                    // Add class to the <a> tag in case of error
                    $('.ui-autocomplete a').addClass('result-active-item');
                }
            });
        } else {
            // Hide the <ul> when search term is too short
            $('.ui-autocomplete').removeClass('show');  // Hide the <ul>
            // Remove class from each <a> tag when hiding the list
            $('.ui-autocomplete a').removeClass('result-active-item');
        }
    });

    // Click event handler to populate input with selected suggestion
    $(document).on('click', '.ui-menu-item a', function () {
        var selectedAgency = $(this).text();  // Get the text of the clicked suggestion
        $('#searchTerm').val(selectedAgency);  // Set the value of the input to the selected suggestion
        
        // Hide the suggestion list after selection
        $('.ui-autocomplete').removeClass('show');  // Hide the <ul>
        
        // Optionally, you can also remove the active class from the <a> tag
        $('.ui-autocomplete a').removeClass('result-active-item');
    });
});