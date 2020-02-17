/**********************************************/
/********* OXX Game JavaScript File ***********/
/******* Filename: client-validation.js *******/
/**********************************************/

/* Login Validation */

// Login form - username validation
function login_uname() {
    inputUname = document.forms["formLogin"]["uname"].value;
    errorMessage = document.getElementById("form-login-uname-error");

    if (inputUname == null || inputUname == "") {
        errorMessage.innerHTML = "Vennligst oppgi din e-post adresse";
        return false;
    } else {
        errorMessage.innerHTML = "";
        return true;
    }
}

// Login form - password validation
function login_passwd() {
    inputPasswd = document.forms["formLogin"]["passwd"].value;
    errorMessage = document.getElementById("form-login-passwd-error");

    if (inputPasswd == null || inputPasswd == "") {
        errorMessage.innerHTML = "Vennligst oppgi ditt passord";
        return false;
    } else {
        errorMessage.innerHTML = "";
        return true;
    }
}

// Login form - all validation
function login_all() {
    acceptUname = login_uname();
    acceptPasswd = login_passwd();

    if (acceptUname && acceptPasswd) {
        return true;
    } else {
        return false;
    }
}

/* End Login Validation */