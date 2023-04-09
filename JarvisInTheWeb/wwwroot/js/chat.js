"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/ChatHub").build();

//Disable the send button until connection is established.
document.getElementById("promptBtn").disabled = true;

connection.on("ReceiveMessage", function (user, message) {

    var li = $("<div class=\"row\"><div class=\"col-sm-1 col-2\">" + user + ":</div>" +
        "<div class=\"col-sm col-10\">" +
        message +
        "</div></div>");

    $("#messagesList").append(li);

    var gpt = $("<div class=\"row gptMessage\"><div class=\"col-sm-1 col-2\">GPT:</div>" +
        "<div class=\"col-sm col-10\">" +
        "</div></div>");

    $("#messagesList").append(gpt);

    $("#messagesList").scrollTop($("#messagesList").prop("scrollHeight"));

    $('#promptBtn').prop('disabled', true);
    $('#ThePrompt').prop('disabled', true);
});

connection.on("ReceiveMessageGPTNewLine", function (user, message) {
    var li = $("<div class=\"row gptMessage\"><div class=\"col-sm-1 col-2\">GPT:</div>" +
        "<div class=\"col-sm col-10\">" +
        message + 
        "</div></div>");

    $("#messagesList").append(li);
    $("#messagesList").scrollTop($("#messagesList").prop("scrollHeight"));
});

connection.on("ReceiveMessageGPTContinue", function (user, message) {
    $("#messagesList").find(".gptMessage:last").find(".col-sm").append(message);
    $("#messagesList").scrollTop($("#messagesList").prop("scrollHeight"));

    if (message == "\n> ") {
        $('#promptBtn').prop('disabled', false);
        $('#ThePrompt').prop('disabled', false);
        $('#ThePrompt').focus().val("");
    }
});

connection.start().then(function () {
    document.getElementById("promptBtn").disabled = false;
}).catch(function (err) {
    return console.error(err.toString());
});

document.getElementById("promptBtn").addEventListener("click", function (event) {
    var user = "User";
    var message = document.getElementById("ThePrompt").value;
    connection.invoke("SendMessage", user, message).catch(function (err) {
        return console.error(err.toString());
    });
    event.preventDefault();
});