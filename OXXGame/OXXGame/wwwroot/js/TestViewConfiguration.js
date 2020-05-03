/**********************************************/
/********* OXX Game JavaScript File ***********/
/******* Filename: TestViewConfiguration.js ***/
/**********************************************/

    

function DisableButton(form) {
    form.submit.disabled = true;
}

var editor = CodeMirror.fromTextArea(
    document.getElementById('input'), {
    mode: "text/x-csharp",
    theme: "base16-dark",
    lineNumbers: true,
    indentUnit: 4,
    indentWithTabs: true,
    autoCloseTags: true,
    autoCloseBrackets: true,
    scrollbarStyle: "overlay",
});
editor.setSize("700", "400");

var outputWindow = CodeMirror.fromTextArea(
    document.getElementById('output'), {
    mode: "text/x-csharp",
    theme: "base16-dark",
    scrollbarStyle: "overlay",
    readOnly: "nocursor",
});
outputWindow.setSize("500", "150");



