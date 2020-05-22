/**
 * Document info: OXX Test JavaScript File
 *     File name: admin_template.js
 *        Author: Bachelor group 15 - OsloMet
 *
 *   Description: This file includes the necessery code that determines some
 *                configurations of the CodeMirror editors that are used to
 *                create/edit task templates
 */

// Template Editor - View Configuration
var Templates = CodeMirror.fromTextArea(
    document.getElementById('taskTemplate'), {
    theme: "base16-dark",
    lineNumbers: true,
    identUnit: 4,
    indentWithTabs: true,
    indentWithTags: true,
    autoCloseBrackets: true,
    scrollbarStyle: "overlay",
    placeholder: "Skriv template her..."
});

// Sets size (width and height) to the given editor
Templates.setSize("100%", "280");
