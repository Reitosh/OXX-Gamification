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



