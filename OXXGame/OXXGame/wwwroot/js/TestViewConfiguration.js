/**
 * Document info: OXX Test JavaScript File
 *     File name: TestViewConfiguration.js
 *        Author: Bachelor group 15 - OsloMet
 *
 *   Description: This file includes the necessery code that determines some
 *                configurations and functions of the CodeMirror editors
 */

function DisableButton(form) {
    form.submit.disabled = true;
}

// C# Editor - View Configuration
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
editor.setSize("100%", "360");

// C# Output Editor - View Configuration
var outputWindow = CodeMirror.fromTextArea(
    document.getElementById('output'), {
    mode: "text/x-csharp",
    theme: "base16-dark",
    scrollbarStyle: "overlay",
    readOnly: "nocursor",
});
outputWindow.setSize("100%", "360");

// HTML Editor - View Configuration
var HTMLeditor = CodeMirror.fromTextArea(
    document.getElementById('htmlEditor'), {
    mode: "htmlmixed",
    theme: "base16-dark",
    lineNumbers: true,
    indentUnit: 4,
    indentWithTabs: true,
    autoCloseTags: true,
    autoCloseBrackets: true,
    scrollbarStyle: "overlay",
    placeholder: "HTML"
});

// CSS Editor - View Configuration
var CSSeditor = CodeMirror.fromTextArea(
    document.getElementById('CSSEditor'), {
    mode: "text/css",
    theme: "base16-dark",
    lineNumbers: true,
    indentUnit: 4,
    indentWithTabs: true,
    autoCloseTags: true,
    autoCloseBrackets: true,
    scrollbarStyle: "overlay",
    placeholder: "CSS"
});

// JavaScript Editor - View Configuration
var JSeditor = CodeMirror.fromTextArea(document.getElementById('Js'), {
    mode: "text/javascript",
    theme: "base16-dark",
    lineNumbers: true,
    indentUnit: 4,
    indentWithTabs: true,
    autoCloseTags: true,
    autoCloseBrackets: true,
    scrollbarStyle: "overlay",
    placeholder: "JavaScript/Vue/React (React støtter foreløpig ikke JSX)"
});

// Function which toggles between what editor is viewed
function showHtml() {
    var srcHtml = HTMLeditor.getValue();
    var srcCSS = CSSeditor.getValue();
    var srcJs = JSeditor.getValue();
    var userInput = srcHtml + "<style type=text/css>" + srcCSS + "</style>" + "<script>" + srcJs + "<" + "/" + "script>";
    var vueTag = "<script src=\"https:\/\/npmcdn.com\/vue\/dist\/vue.js\"><\/script>"
    var reactTag = "<script src=\"https:\/\/unpkg.com\/react@16\/umd\/react.development.js\" crossorigin><\/script>" +
        "<script src=\"https:\/\/unpkg.com\/react-dom@16\/umd\/react-dom.development.js\" crossorigin><\/script>"
    var fullInput;

    if ("@Model.task.category" == "Vue.js") {
        fullInput = vueTag + userInput;
    } else if ("@Model.task.category" == "React") {
        fullInput = reactTag + userInput;
    } else if ("@Model.task.category" == "HTML" || "@Model.task.category" == "JavaScript" || "@Model.task.category" == "CSS") {
        fullInput = userInput;
    }

    document.getElementById('iframe').srcdoc = fullInput;
    document.getElementById('kode').value = fullInput;
}

