/**
 * Document info: OXX Test JavaScript File
 *     File name: csharp_view_config.js
 *        Author: Bachelor group 15 - OsloMet
 *
 *   Description: This file includes the necessery code that determines some
 *                configurations of the CodeMirror C# editors
 */


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
    placeholder: "Skriv din C# kode her..."
});

// C# Output Editor - View Configuration
var outputWindow = CodeMirror.fromTextArea(
    document.getElementById('output'), {
    mode: "text/x-csharp",
    theme: "base16-dark",
    readOnly: "nocursor",
    scrollbarStyle: "overlay",
});

