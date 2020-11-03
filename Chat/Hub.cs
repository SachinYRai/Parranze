using Microsoft.AspNetCore.SignalR;
using System;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace SignalRChat
{
    public class Chat : Hub
    {
        public async Task SendMessage(MessageIn request)
        {
            var test = request.GroupId;
            //get user name from id ? inject context?
            await Clients.User(request.UserId).SendAsync("ReceiveMessage", request.UserName, request.Message);
        }

        public async Task AddToGroup(AddToGroupRequest req)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, req.GroupName);

            if (req.IsNewUser)
            {
                await Clients.Group(req.GroupName)
                            .SendAsync("ReceiveMessage", new MessageOut {
                                Message =  $"{req.UserName } has been added to the conversation {req.GroupName}"
    
                            });
            }
        }

        public async Task RemoveFromGroup(string groupName)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);

            await Clients.Group(groupName).SendAsync("ReceiveMessage", $"{Context.ConnectionId} has left the group {groupName}.");
        }

        public async Task SendToGroup(MessageIn request)
        {           
            await Clients.Group(request.GroupId)
                         .SendAsync("ReceiveMessage", new MessageOut
                         {
                             Message = request.UserName + " : <br/> " +request.Message
                         });    
        }

    }

    public class AddToGroupRequest {
        public string UserName{ get; set; }
        public string GroupName { get; set; }
        public bool IsNewUser { get; set; }
    }
    
    public class MessageIn {
        public string UserName { get; set; }
        public string UserId { get; set; } 
        public  string Message { get; set; }
        public string GroupId { get; set; }
    }

        public class MessageOut
        {
            public string UserName { get; set; }
            public string UserId { get; set; }
            public string Message { get; set; }
            public string GroupId { get; set; }
        }
        // create the ui
        // on adding multiple user create group
        // if( group  else send private

    }
