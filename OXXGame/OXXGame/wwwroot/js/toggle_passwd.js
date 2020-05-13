/**
 * Document info: OXX Test JavaScript File
 *     File name: toggle_passwd.js
 *        Author: Bachelor group 15 - OsloMet
 *
 *   Description: This file includes the necessery code that determines the
 *                behavior of the show/hide password function of password input.
 */


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