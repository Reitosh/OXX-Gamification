function decodeChars(TypeScript) {
    var txt = document.createElement("textarea");
    txt.innerHTML = TypeScript;
    return txt.value;
}

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
    placeholder: "TypeScript"
});

var JavaScriptComp = CodeMirror.fromTextArea(
    document.getElementById('JavaScript'), {
    mode: "text/javascript",
    theme: "base16-dark",
    lineNumbers: true,
    indentUnit: 4,
    indentWithTabs: true,
    autoCloseTags: true,
    autoCloseBrackets: true,
    scrollbarStyle: "overlay",
    readOnly: "nocursor",
    placeholder: "Kompilert TypeScript"
});

var errorOutput = CodeMirror.fromTextArea(
    document.getElementById('error'), {
    mode: "application/typescript",
    theme: "base16-dark",
    readOnly: "nocursor",
    scrollbarStyle: "overlay",
    placeholder: "Errors",
});
errorOutput.setSize("521", "150");