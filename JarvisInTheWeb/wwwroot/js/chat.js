"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/ChatHub").build();

//Disable the send button until connection is established.
document.getElementById("promptBtn").disabled = true;

connection.on("ReceiveMessage", function (user, message) {
    var li = document.createElement("li");

    var li = $("<div class=\"row\"><div class=\"col-sm-1\">" + user + ":</div>" +
        "<div class=\"col-sm\">" +
        message +
        "</div></div>");

    $("#messagesList").append(li);

    $("#messagesList").scrollTop($("#messagesList").prop("scrollHeight"));

    // We can assign user-supplied strings to an element's textContent because it
    // is not interpreted as markup. If you're assigning in any other way, you 
    // should be aware of possible script injection concerns.
});

connection.on("ReceiveMessageGPT", function (user, message) {
    var li = $("<div class=\"row\"><div class=\"col-sm-1\" class=\"gptMessage\">GPT:</div>" +
        "<div class=\"col-sm\" style=\"color:#007cff\">" +
        message + 
        "</div></div>");

    $("#messagesList").append(li);

    $("#messagesList").scrollTop($("#messagesList").prop("scrollHeight"));

    // We can assign user-supplied strings to an element's textContent because it
    // is not interpreted as markup. If you're assigning in any other way, you 
    // should be aware of possible script injection concerns.
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