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
    placeholder: "JavaScript"
});