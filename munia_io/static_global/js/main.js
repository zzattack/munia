(function ($) {
    "use strict";

    // Remove preload class once page is fully loaded
    $(window).on('load', function () {
        $('body').removeClass('preload');

        // set active class on navbar selected page
        var page = window.location.pathname.substring(1);
        if (page.length == 0) page = "index.html";
        $('nav ul li a[href*="' + page + '"]').addClass('active');
    });

    // Add class to navigation when scrolling down

    $(window).on('scroll', function () {
        var scroll = $(window).scrollTop();
        if (scroll >= 20) {
            $('.header-main').addClass('fade-in');
        } else {
            $('.header-main').removeClass('fade-in');
        }
    });

    // Add class when mobile navigation icon is clicked
    $('.nav-toggle').on('click', function () {
        $('body').toggleClass('no-scroll');
        $('.header-main').toggleClass('active');
    });

    // Prevent background from scrolling on mobile when navigation is toggled
    $('html, body').on('touchmove', function(ev) {
        ev.preventDefault();
    });

    $('a#connect').click(function () {
        var aTag = $("a[name='" + $(this).attr("href") + "']");
        $('html,body').animate({scrollTop: $(aTag).offset().top}, 'slow');
        return false;
    });

    $('form.button').click(function() {
       $(this).submit();
    });
})(jQuery);