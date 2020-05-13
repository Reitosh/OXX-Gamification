/**
 * Document info: OXX Test JavaScript File
 *     File name: nav_scroll.js
 *        Author: Bachelor group 15 - OsloMet
 *
 *   Description: Simple on-window-scroll function that changes the
 *                appearance of the navbar when scrolled from top.
 */

$(window).scroll(function () {
    $('nav').toggleClass('navbar-scroll', $(this).scrollTop() > 88);

    // If the current window/page is scrolled
    if ($(this).scrollTop() > 88) {
        $('.navbar .navbar-brand img').attr('src', '../images/brand/logo_svart.png');
        $('.navbar-nav li a').css({ "color": "#2B2B2B" });
        $('.navbar-nav li a').css({ "font-weight": "600" });
    } else {
        $('.navbar .navbar-brand img').attr('src', '../images/brand/logo_hvit.png');
        $('.navbar-nav li a').css({ "color": "#FFFFFF" });
        $('.navbar-nav li a').css({ "font-weight": "400" });
    }
});