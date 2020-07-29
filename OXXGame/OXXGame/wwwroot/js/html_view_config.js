
/**
 * Document info: OXX Test JavaScript File
 *     File name: html_view_config.js
 *        Author: Bachelor group 15 - OsloMet
 *
 *   Description: This file includes the necessery code that determines some
 *                configurations of the CodeMirror HTML/CSS/JS editors
 */

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
    placeholder: "Skriv din HTML kode her..."
});

// CSS Editor - View Configuration
var CSSeditor = CodeMirror.fromTextArea(
    document.getElementById('cssEditor'), {
    mode: "text/css",
    theme: "base16-dark",
    lineNumbers: true,
    indentUnit: 4,
    indentWithTabs: true,
    autoCloseTags: true,
    autoCloseBrackets: true,
    scrollbarStyle: "overlay",
    placeholder: "Skriv din CSS kode her..."
});

// JavaScript Editor - View Configuration
var JSeditor = CodeMirror.fromTextArea(document.getElementById('jsEditor'), {
    mode: "text/javascript",
    theme: "base16-dark",
    lineNumbers: true,
    indentUnit: 4,
    indentWithTabs: true,
    autoCloseTags: true,
    autoCloseBrackets: true,
    scrollbarStyle: "overlay",
    placeholder: "Skriv din JavaScript kode her..."
});