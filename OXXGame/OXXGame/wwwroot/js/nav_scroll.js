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

    if ($(this).scrollTop() > 88) {
        $('.navbar .navbar-brand img').attr('src', '../images/brand/logo_svart.png');
        $('.navbar .navbar-brand img').css('width', '36px');
        $('.navbar-nav li a').css({ "color": "#2B2B2B"});
    } else {
        $('.navbar .navbar-brand img').attr('src', '../images/brand/logo_hvit.png');
        $('.navbar .navbar-brand img').css('width', '42px');
        $('.navbar-nav li a').css({ "color": "#FFFFFF"});
    }

});