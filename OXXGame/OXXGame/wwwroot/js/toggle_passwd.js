/**********************************************/
/********* OXX Game JavaScript File ***********/
/************ Filename: login.js **************/
/**********************************************/


// Password reveal toggle button
$(".passwd-toggle").click(function () {

    $(this).toggleClass("fa-eye fa-eye-slash");
    var passwdInput = $($(this).attr("toggle"));
    if (passwdInput.attr("type") == "password") {
        passwdInput.attr("type", "text");
    } else {
        passwdInput.attr("type", "password");
    }
});