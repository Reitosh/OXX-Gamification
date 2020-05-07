/**
 * Document info: OXX Game JavaScript File
 *     File name: nav_scroll.js
 *        Author: Vladimir Maric
 *
 *   Description: Simple on-window-scroll function that changes the
 *                appearance of the navbar when scrolled from top.
 */

$(window).scroll(function () {
    $('nav').toggleClass('navbar-scroll', $(this).scrollTop() > 88);

    // If the current page is scrolled
    if ($(this).scrollTop() > 88) {
        $('.navbar .navbar-brand img').attr('src', '../images/brand/logo_svart.png');
        $('.navbar .navbar-brand img').css('width', '36px');
        $('.navbar-nav li a').css({ "color": "#2B2B2B" });
        $('.navbar-nav li a').css({ "font-size": "16px" });
        $('.navbar-nav li a').css({ "font-weight": "600" });
    // If page not scrolled and screen width is greater than or equal to 768px
    } else if ($(this).scrollTop() < 88 && $(this).width() >= 768) {
        $('.navbar .navbar-brand img').attr('src', '../images/brand/logo_hvit.png');
        $('.navbar .navbar-brand img').css('width', '48px');
        $('.navbar-nav li a').css({ "color": "#FFFFFF" });
        $('.navbar-nav li a').css({ "font-size": "18px" });
        $('.navbar-nav li a').css({ "font-weight": "400" });
    // If page not scrolled and screen width is less than 768px
    } else if ($(this).scrollTop() < 88 && $(this).width() < 768) {
        $('.navbar .navbar-brand img').attr('src', '../images/brand/logo_hvit.png');
        $('.navbar .navbar-brand img').css('width', '36px');
        $('.navbar-nav li a').css({ "color": "#FFFFFF" });
        $('.navbar-nav li a').css({ "font-size": "16px" });
        $('.navbar-nav li a').css({ "font-weight": "400" });
    } 
});