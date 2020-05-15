/**
 * Document info: OXX Test JavaScript File
 *     File name: ts_view_config.js
 *        Author: Bachelor group 15 - OsloMet
 *
 *   Description: This file includes the necessery code that determines some
 *                configurations of the CodeMirror TypeScript editors
 */

// TypeScript Editor - View Configuration
var TypeScriptEditor = CodeMirror.fromTextArea(
    document.getElementById('TypeScriptEditor'), {
    mode: "application/typescript",
    theme: "base16-dark",
    lineNumbers: true,
    indentUnit: 4,
    indentWithTabs: true,
    autoCloseTags: true,
    autoCloseBrackets: true,
    scrollbarStyle: "overlay",
    placeholder: "Skriv din TypeScript kode her..."
});

// JavaScript Output Editor - View Configuration
var JavaScriptComp = CodeMirror.fromTextArea(
    document.getElementById('JavaScript'), {
    mode: "text/javascript",
    theme: "base16-dark",
    lineNumbers: true,
    indentUnit: 4,
    indentWithTabs: true,
    autoCloseTags: true,
    autoCloseBrackets: true,
    readOnly: "nocursor",
    scrollbarStyle: "overlay",
});

// TypeScript Error Output - View Configuration
var errorOutput = CodeMirror.fromTextArea(
    document.getElementById('error'), {
    mode: "application/typescript",
    theme: "base16-dark",
    readOnly: "nocursor",
    scrollbarStyle: "overlay",
    placeholder: "Errors",
});

// Function that decodes the characters
function decodeChars(TypeScript) {
    var txt = document.createElement("textarea");
    txt.innerHTML = TypeScript;
    return txt.value;
}

