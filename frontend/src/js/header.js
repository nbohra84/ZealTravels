
$(document).ready(function () {
    function myclipopse() {
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

    function googleTranslateElementInit() {
        new google.translate.TranslateElement({ pageLanguage: 'en' }, 'google_translate_element');
    }
    window.myclipopse = myclipopse;
    window.googleTranslateElementInit = googleTranslateElementInit;
})