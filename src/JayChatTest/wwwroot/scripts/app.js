// Message type enum
var messageTypes = {
    REGISTER: 0,
    MESSAGE: 1
}


var url = "ws://" + window.location.hostname + ":80/Chat";

var output;

function init() {
    output = $(".chatScreen");
    doWebSocket();
}

function doWebSocket() {
    websocket = new WebSocket(url);

    websocket.onopen = function (evt) {
        onOpen(evt)
    };

    websocket.onclose = function (evt) {
        onClose(evt)
    };

    websocket.onmessage = function (evt) {
        onMessage(evt)
    };

    websocket.onerror = function (evt) {
        onError(evt)
    };
}

function onOpen(evt) {
    writeToScreen("CONNECTED");
    //send ("WebSocket rocks");
}

function onClose(evt) {
    writeToScreen("DISCONNECTED");
}

function onMessage(evt) {
    var message = JSON.parse(evt.data);
    writeToScreen('<span style="color: red;">' + message.username + '</span>: <span style="color: blue;">' + message.message + '</span>');
    //websocket.close ();
}

function onError(evt) {
    writeToScreen('<span style="color: red;">ERROR: ' + evt.data + '</span>');
}

function send(message) {
    //writeToScreen ("SENT: " + message);
    websocket.send(message);
}

function writeToScreen(message) {
    $(".chatScreen").append("<p>" + message + "</p>");
}

function sendMessage() {
    var message = {
        requestTypeId: messageTypes.MESSAGE,
        message: $('.inputMessage').val()
    }

    send(JSON.stringify(message));

    //var input = $('.inputMessage').val();
    //send("{requestTypeId:1, message: '" + input + "'}");
}

function registerUser() {
    // Register the user
    var registration = {
        requestTypeId: messageTypes.REGISTER,
        username: $("#usernameTxt").val()
    }
    send(JSON.stringify(registration));

    // switch from registration window to chat
    $("#registrationDiv").addClass("hide");
    $("#chatDiv").removeClass("hide");
}

function sendMessageFormClick() {
    sendMessage();
    event.preventDefault();
}

window.addEventListener("load", init, false);