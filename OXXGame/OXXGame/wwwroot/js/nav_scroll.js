/**********************************************/
/********* OXX Game JavaScript File ***********/
/******* Filename: client-validation.js *******/
/**********************************************/

$(window).scroll(function () {
    $('nav').toggleClass('navbar-scroll', $(this).scrollTop() > 88);

    if ($(this).scrollTop() > 88) {
        $('.navbar .navbar-brand img').attr('src', '../images/brand/logo_svart.png');
        $('.navbar .navbar-brand img').css('width', '36px');
        $('.navbar-nav li a').css('color', '#282828');
    } else {
        $('.navbar .navbar-brand img').attr('src', '../images/brand/logo_hvit.png');
        $('.navbar .navbar-brand img').css('width', '42px');
        $('.navbar-nav li a').css('color', '#ffffff');
    }

});