/**
 * Document info: OXX Test JavaScript File
 *     File name: client_validation.js
 *        Author: Bachelor group 15 - OsloMet
 *   
 *   Description: This file includes all the necessary methods for the 
 *                client-side validation of login and registration form.
 */

/*======================================================================
  Login Form Validation 
  ======================================================================*/

// Login form - email validation
function loginEmail() {
    inputEmail = document.forms["formLogin"]["email"].value;
    errorMessage = document.getElementById("form-error-login-email");

    if (inputEmail == null || inputEmail == "") {
        errorMessage.innerHTML = "Vennligst skriv inn e-postaddresse";
        return false;
    } else {
        errorMessage.innerHTML = "";
        return true;
    }
}

// Login form - password validation
function loginPasswd() {
    inputPasswd = document.forms["formLogin"]["password"].value;
    errorMessage = document.getElementById("form-error-login-password");

    if (inputPasswd == null || inputPasswd == "") {
        errorMessage.innerHTML = "Vennligst skriv inn ditt passord";
        return false;
    } else {
        errorMessage.innerHTML = "";
        return true;
    }
}

// Login form - all validation
function loginAll() {
    acceptEmail = loginEmail();
    acceptPasswd = loginPasswd();

    if (acceptEmail && acceptPasswd) {
        return true;
    } else {
        return false;
    }
}

/*======================================================================
  Registration Form Validation
  ======================================================================*/

/* Regular Expression variables */
var REGEX_TLF = /^((0047)?|(\+47)?)\d{8}$/
var REGEX_NAME = /^[a-zA-ZæøöåäéÆØÖÅÄÉ '-]{2,40}$/;
var REGEX_PASSWD = /^(?=.*[A-ZÆØÅÉ])(?=.*\d)[a-zA-ZæøöåäéÆØÖÅÄÉ .,:;!?@#$£&%*+-=~^<>(){}\d]{8,255}$/;
var REGEX_EMAIL = /^(([^<>()\[\]\\.,;:\s@"]+(\.[^<>()\[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,8}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;

// Registration form - firstname validation
function regFirstname() {
    inputFirstname = document.forms["formReg"]["firstname"].value;
    accept = REGEX_NAME.test(inputFirstname);
    errorMessage = document.getElementById("form-error-reg-firstname");

    if (inputFirstname == null || inputFirstname == "") {
        errorMessage.innerHTML = "Vennligst skriv inn ditt fornavn";
        return false;
    } else if (!accept) {
        errorMessage.innerHTML = "Fornavnet må være mellom 2 og 40 tegn langt og kun inneholde bokstaver og mellomrom";
        return false;
    } else {
        errorMessage.innerHTML = "";
        return true;
    }
}

// Registration form - lastname validation
function regLastname() {
    inputLastname = document.forms["formReg"]["lastname"].value;
    accept = REGEX_NAME.test(inputLastname);
    errorMessage = document.getElementById("form-error-reg-lastname");

    if (inputLastname == null || inputLastname == "") {
        errorMessage.innerHTML = "Vennligst skriv inn ditt etternavn";
        return false;
    } else if (!accept) {
        errorMessage.innerHTML = "Etternavnet må være mellom 2 og 40 tegn langt og kun inneholde bokstaver og mellomrom";
        return false;
    } else {
        errorMessage.innerHTML = "";
        return true;
    }
}

// Registration form - email validation
function regEmail() {
    inputEmail = document.forms["formReg"]["email"].value;
    accept = REGEX_EMAIL.test(inputEmail);
    errorMessage = document.getElementById("form-error-reg-email");

    if (inputEmail == null || inputEmail == "Vennligst skriv inn din e-postadresse") {
        errorMessage.innerHTML = errMsg;
        return false;
    } else if (!accept) {
        errorMessage.innerHTML = "Ugyldig e-postadresse";
        return false;
    } else {
        errorMessage.innerHTML = "";
        return true;
    }
}

// Registration form - tlf validation
function regTlf() {
    inputTlf = document.forms["formReg"]["tlf"].value;
    accept = REGEX_TLF.test(inputTlf);
    errorMessage = document.getElementById("form-error-reg-tlf");

    if (inputTlf == null || inputTlf == "") {
        errorMessage.innerHTML = "Vennligst skriv inn ditt telefonnummer";
        return false;
    } else if (!accept) {
        errorMessage.innerHTML = "Ugyldig norsk telefonnummer";
        return false;
    } else {
        errorMessage.innerHTML = "";
        return true;
    }
}

// Registration form - password validation
function regPasswd() {
    inputPasswd = document.forms["formReg"]["password"].value;
    accept = REGEX_PASSWD.test(inputPasswd);
    errorMessage = document.getElementById("form-error-reg-password");

    if (inputPasswd == null || inputPasswd == "") {
        errorMessage.innerHTML = "Skriv inn et passord";
        return false;
    } else if (!accept) {
        errorMessage.innerHTML = "Passordet må bestå av minst 8 tegn, en stor bokstav og ett tall";
        return false;
    } else {
        errorMessage.innerHTML = "";
        return true;
    }
}

// Registration form - all validation
function regAll() {
    acceptFirstname = regFirstname();
    acceptLastname = regLastname();
    acceptEmail = regEmail();
    acceptTlf = regTlf();
    acceptPasswd = regPasswd();

    if (acceptFirstname && acceptLastname && acceptEmail && acceptTlf && acceptPasswd) {
        return true;
    } else {
        return false;
    }
}