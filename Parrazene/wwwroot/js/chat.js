"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/chatHub").build();

//Disable send button until connection is established
document.getElementById("sendButton").disabled = true;

connection.on("ReceiveMessage", function (req) {
   // console.log("Receive Message" +req);
    var msg = req.message;

    var messBox = document.createElement("div");
    messBox.classList.add("received-mess");

    messBox.innerHTML = msg;

    document.getElementById("message-display").appendChild(messBox);
});

connection.start().then(function () {
    document.getElementById("sendButton").disabled = false;
    console.log("connection started");
}).catch(function (err) {
    return console.error(err.toString());
});

// on selecting users find the group on tokenize add or remove event triggered
// any sent message should be sent to the group
// any user deleted remove from group

document.getElementById("sendButton").addEventListener("click", function (event) {

    var users = document.getElementsByClassName("token");
    let groupId = document.getElementById("group-id").value;

    console.log("send button clicked, group Id" + groupId);

    if (groupId.length > 0) {

        var enteredMessage = document.getElementById("messageInput").value;

        $.get("/Chat/GetUserName", function (data) {
            if (enteredMessage.length > 0) {
                connection.invoke("SendToGroup", { groupId, message: enteredMessage, UserName: data }).catch(function (err) {
                    return console.error(err.toString());
                });
                // save message in db if succesfull .. other alternatives ?

                let recipientIds = Array.from(users).map(u => u.dataset.value);

                $.post("/Chat/Send", { ConversationId: groupId, Message: enteredMessage, RecipientIds: recipientIds }, function (data) {
                    if (data == true) {
                        console.log("Conversation saved in db");
                    }
                    else
                        console.log("Conversation save in db failed.")
                });

            }
        });
    }
        event.preventDefault();
});


// online users should be added to the group
$('.auto-multi-select').on("tokenize:tokens:added",
    function (e, value, text) {

        var users = document.getElementsByClassName("token");

        if (users.length > 0) {
            let userIds = Array.from(users).map(u => u.dataset.value);
            var groupId = document.getElementById("group-id").value;
            var groupBool = document.getElementById("group-bool").value;

            //existing convo
            if (groupBool === "true") {

                connection.invoke("AddToGroup", { UserName: text, GroupName: groupId })
                    .catch(function (err) {
                        return console.error(err.toString());
                    });
            }
            else {
                $.post("/Chat/GetGroupId", { userIds: userIds }, function (data) {

                    document.getElementById("group-id").value = data;

                    console.log("Add to group invoked, id = " + data);
                    connection.invoke("AddToGroup", { UserName: text, GroupName: data, IsNewUser : users.length>1})
                        .catch(function (err) {
                            return console.error(err.toString());
                        });
                });
            }
        }                    
});

$("#messageInput").keyup(function (e) {
    if (event.which == 13) {
        $("#sendButton").click();
    }
})

// Conversations functionality
$(".convo a").click(function (e) {
    // clear old messages
    $("#message-display").empty();

    document.getElementById("group-id").value = '';

    var tokenizeSelect = $('.auto-multi-select').tokenize2();
    //clear Tokesn
    tokenizeSelect.trigger('tokenize:clear');

   // update group id
    var convoId = $(this).children(".convo-id")[0].value;
    document.getElementById("group-id").value = convoId;
    document.getElementById("group-bool").value = true;
 
    // update the users
    $.get("/Chat/GetConversationUsers", { convoId }, function (data) {

        $.each(data, function (k, v) {
            //console.log(v);      
            tokenizeSelect.trigger('tokenize:tokens:add', [v.userId, v.userName, true]);
        });
    });

    //load last 10 messages

    $.get("/Chat/GetConvoMessages", { convoId }, function (data) {
        $.each(data, function (k, v) {
            console.log(v);
            console.log(v.message);
            var messBox = document.createElement("div");
            messBox.classList.add("received-mess");

            messBox.innerHTML = v.message + v.dateTime;

            document.getElementById("message-display").prepend(messBox);
        });
    });
})






