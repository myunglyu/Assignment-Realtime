"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/chatHub").build();

//Disable the send button until connection is established.
document.getElementById("sendButton").disabled = true;

connection.on("ReceiveMessage", function (user, message) {
    var li = document.createElement("li");
    document.getElementById("messagesList").appendChild(li);
    // We can assign user-supplied strings to an element's textContent because it
    // is not interpreted as markup. If you're assigning in any other way, you 
    // should be aware of possible script injection concerns.
    document.getElementById("messagesContainer").scrollTop = document.getElementById("messagesContainer").scrollHeight;
    li.textContent = `${user}: ${message}`;
});


// Start connection
connection.start().then(function () {
    document.getElementById("sendButton").disabled = true;
    // Request initial user list
    connection.invoke("GetConnectedUsers");
}).catch(function (err) {
    return console.error(err.toString());
});

document.getElementById("sendButton").addEventListener("click", function (event) {
    var user = document.getElementById("userInput").value;
    var message = document.getElementById("messageInput").value;
    connection.invoke("SendMessage", user, message).catch(function (err) {
        return console.error(err.toString());
    });
    event.preventDefault();
});

// Handle user connected
connection.on("UserConnected", function (username) {
    var user = document.getElementById("userInput").value;
    if (user != "Guest"){
        const message = document.createElement("li");
        message.className = "system-message";
        message.textContent = `${username} joined the chat`;
        document.getElementById("messagesList").appendChild(message);
        document.getElementById("sendButton").disabled = false;
    }
});

// Handle user disconnected
connection.on("UserDisconnected", function (username) {
    const message = document.createElement("li");
    message.className = "system-message";
    message.textContent = `${username} left the chat`;
    document.getElementById("messagesList").appendChild(message);
});

// Update user list
connection.on("UpdateUserList", function (users) {
    const usersList = document.getElementById("usersList");
    usersList.innerHTML = "";
    users.forEach(user => {
        const li = document.createElement("li");
        li.className = "list-group-item user-item online";
        li.textContent = user;
        usersList.appendChild(li);
    });
});
