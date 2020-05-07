/**
 * Document info: OXX Game JavaScript File
 *     File name: client_validation.js
 *        Author: Vladimir Maric
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
        errorMessage.innerHTML = "Vennligst oppgi e-post adresse";
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
        errorMessage.innerHTML = "Vennligst oppgi passord";
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
var REGEX_NAME = /^[a-zA-ZàáâäãåąčćęèéêëėįìíîïłńòóôöõøùúûüųūÿýżźñçčšžÀÁÂÄÃÅĄĆČĖĘÈÉÊËÌÍÎÏĮŁŃÒÓÔÖÕØÙÚÛÜŲŪŸÝŻŹÑßÇŒÆČŠŽ∂ð '-]{2,20}$/;
var REGEX_EMAIL = /^(([^<>()\[\]\\.,;:\s@"]+(\.[^<>()\[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
var REGEX_TLF = /^\+(?:[0-9] ?){6,14}[0-9]$/;
var REGEX_PASSWD = /^(?=.*[A-Za-z])(?=.*\d)[A-Za-z\d]{8,}$/;

// Registration form - firstname validation
function regFirstname() {
    inputFirstname = document.forms["formReg"]["firstname"].value;
    accept = REGEX_NAME.test(inputFirstname);
    errorMessage = document.getElementById("form-error-reg-firstname");

    if (inputFirstname == null || inputFirstname == "") {
        errorMessage.innerHTML = "Fornavn er påkrevd";
        return false;
    } else if (!accept) {
        errorMessage.innerHTML = "Fornavnet må være mellom 2 og 20 tegn langt og kun inneholde bokstaver og mellomrom";
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
        errorMessage.innerHTML = "Etternavn er påkrevd";
        return false;
    } else if (!accept) {
        errorMessage.innerHTML = "Etternavnet må være mellom 2 og 20 tegn langt og kun inneholde bokstaver og mellomrom";
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

    if (inputEmail == null || inputEmail == "") {
        errorMessage.innerHTML = "E-post adresse er påkrevd";
        return false;
    } else if (!accept) {
        errorMessage.innerHTML = "Vennligst oppgi en gyldig e-post adresse";
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
        errorMessage.innerHTML = "Telefonnummeret er påkrevd";
        return false;
    } else if (!accept) {
        text = "Kan kun inneholde tall og mellomrom<br/>";
        text += "Landskoden må inkluderes (f.eks. +47)";
        errorMessage.innerHTML = text;
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
        errorMessage.innerHTML = "Passord er påkrevd";
        return false;
    } else if (!accept) {
        errorMessage.innerHTML = "Passordet må inneholde minst 8 tegn, minst en bokstav og ett tall";
        return false;
    } else {
        errorMessage.innerHTML = "";
        return true;
    }
}

// Registration form - password-repeat validation
function regPasswdRepeat() {
    inputPasswdRepeat = document.forms["formReg"]["passwordRepeat"].value;
    inputPasswd = document.forms["formReg"]["password"].value;
    errorMessage = document.getElementById("form-error-reg-passwordRepeat");
    acceptPasswd = regPasswd();

    if (acceptPasswd) {
        if (inputPasswdRepeat == inputPasswd) {
            errorMessage.innerHTML = "";
            return true;
        } else {
            errorMessage.innerHTML = "Stemmer ikke overens med gjentatt passord";
            return false;
        }
    } else {
        return false;
    }
}

// Registration form - all validation
function regAll() {
    acceptFirstname = regFirstname();
    acceptLastname = regLastname();
    acceptEmail = regEmail();
    acceptTlf = regTlf();
    acceptPasswd = regPasswd();
    acceptPasswdRepeat = regPasswdRepeat();

    if (acceptFirstname && acceptLastname && acceptEmail && acceptTlf && acceptPasswd && acceptPasswdRepeat) {
        return true;
    } else {
        return false;
    }
}