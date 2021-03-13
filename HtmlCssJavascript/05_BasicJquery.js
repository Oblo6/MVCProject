$(document).ready(function () {
    console.log("Script loaded.");
    var inputText;
    $("#sOutput1").text("");
    $("#sOutput2").html("");
    $("#txtOutput1").val("");
    $("#btnSetText").click(function () {
        inputText = $("#txtInput1").val();
        $("#sOutput1").text(inputText);
    });
    $("#btnSetHtml").click(function () {
        inputText = $("#txtInput1").val();
        $("#sOutput2").html('<label style="color: blue; font-size: 24px;"><b>'+ inputText + '</b></label>');
    });
    $("#btnSetValue").click(function () {
        inputText = $("txtInput1");
        $("#txtOutput1").val(inputText);
    });
});