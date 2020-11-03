using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Net.Mail;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Parrazene.Areas.Identity.Data;
using Parrazene.Data;
using Parrazene.Models;

namespace Parrazene.Controllers
{
    [Authorize]
    public class ChatController : BaseController
    {
        private ConversationDAL ConvoDal;
        public ConversationUserDAL ConversationUserDAL { get; }
        public ConversationMessageDAL ConversationMessageDAL { get; }


        public ChatController(UserManager<User> userManager, ParrazeneDbContext dbContext, IConversationDAL conversationDAL, IConversationUserDAL conversationUserDAL, IConversationMessageDAL conversationMessageDAL) : base(userManager, dbContext)
        {
            ConvoDal = conversationDAL as ConversationDAL;
            ConversationUserDAL = conversationUserDAL as ConversationUserDAL;
            ConversationMessageDAL = conversationMessageDAL as ConversationMessageDAL;
        } 

        public IActionResult Index()
        {
           return View(new ChatViewModel { Users = DbContext.Users , Conversations=GetUserConvesrations()}) ;
        }


        private IEnumerable<Conversation> GetUserConvesrations()
        {
            return DbContext.ConversationUsers
                           .Include(cu=>cu.Conversation)
                           .ThenInclude(c=>c.LatestMessage)
                           .ThenInclude(m=>m.User)
                           .Where(cu => cu.UserId == CurrentUser.Id)
                           .Select(c => c.Conversation)
                           .Distinct();
        }

        [HttpGet]
        public IEnumerable<object> GetUsers(string query)
        {
            return DbContext.Users
                            .Select(u => new {  u.Id,  u.Name })
                            .Where(x=>x.Id != CurrentUser.Id &&  !string.IsNullOrEmpty(x.Name) && x.Name.Contains(query));
        }

        [HttpPost]
        public string GetGroupId(List<string> userIds)
        {
            userIds.Add(CurrentUser.Id);

            return ConvoDal.GetOrCreateConversationId(userIds);
        }

        [HttpGet]
        public string GetUserName()
        {
            return UserManager.GetUserName(User) ?? "Unknown";
        }

        [HttpPost]
        public async Task<bool> Send(MessageSubmission messageSubmission)
        {
            Guid.TryParseExact(messageSubmission.ConversationId, "D",out var convoId);

            if (convoId == null)
                return false;

            var convo = ConvoDal.GetById(convoId) as Conversation;

            using (var transaction = DbContext.Database.BeginTransaction())
            {
                try
                {
                    if (convo != null)
                     {  // ConvoDal.Update(convoID);
                          var newMessage = new ConversationMessage
                          {
                              Message = messageSubmission.Message,
                              User = CurrentUser,
                              Conversation = convo 
                          };

                          ConversationMessageDAL.Add(newMessage);
                          await ConversationMessageDAL.Commit();

                          ConvoDal.Update(convo);
                          convo.LatestMessage = newMessage;
                          await  ConvoDal.Commit();
 
                          // update related entities of the convo some how?
                          // TODO next
                        // Add user to the same conversation?

                     }
                      else
                      {// Initial Creation
                         
                              
                                  var newConvo = new Conversation(convoId);
                                  var users = new List<User>();

                                  var newMessage = new ConversationMessage
                                  {
                                      Message = messageSubmission.Message,
                                      User = CurrentUser,
                                      Conversation = newConvo
                                  };

                                  var conversationUsers = new List<ConversationUser>();

                                  conversationUsers.Add(new ConversationUser
                                  {
                                      Conversation = newConvo,
                                      User = CurrentUser
                                  });

                                  foreach (var recipientId in messageSubmission.RecipientIds)
                                  {
                                      var user = DbContext.Users.Find(recipientId);

                                      conversationUsers.Add( new ConversationUser 
                                      { 
                                          Conversation = newConvo,
                                          User = user
                                      });

                                      users.Add(user);
                                  }

                                  //Update Conversation
                                  newConvo.Name = users?.Select(u=>u.Name)?.Aggregate((a, b) => a+ ", " + b);
                                 
;
                                  ConvoDal.Add(newConvo);

                                  await ConvoDal.Commit();

                                  foreach (var cu in conversationUsers)
                                  {
                                      ConversationUserDAL.Add(cu);
                                      await ConversationUserDAL.Commit();
                                  }

                                  ConversationMessageDAL.Add(newMessage);
                                  await ConversationMessageDAL.Commit();

                                  // Update Convo after message is created

                                  ConvoDal.Update(newConvo);
                                  newConvo.LatestMessage = newMessage;

                                  await ConvoDal.Commit();                             
                       }

                    transaction.Commit();
                }
                    catch (Exception e)
                    {
                        transaction.Dispose();

                        throw new ArgumentException(e.Message);
                    }
            }

            return true;
        }

        [HttpGet]
        public IEnumerable<TokenizeToken> GetConversationUsers(string convoId)
        {
            return DbContext.ConversationUsers
                            .Where(c => c.ConversationId.ToString() == convoId && c.UserId != CurrentUser.Id)
                            .Select(x => new TokenizeToken { UserId = x.UserId, UserName=x.User.UserName});
        }

        [HttpGet]
        public IEnumerable<ConvoMessage> GetConvoMessages(string convoId)
        {
            // return the last 10 message
            return ConversationMessageDAL.GetConversationMessages(convoId)
                                         .Select(x => new ConvoMessage { Message = x.GetMessageView(), DateTime = x.DateCreated.ToLongTimeString() });
        }

        public class ConvoMessage {
            public string Message { get; set; }
            public  string DateTime { get; set; }
        }


        public class MessageSubmission { 
            public string ConversationId { get; set; }

            public string Message { get; set; }

            public string[] RecipientIds { get; set; }
        }

        public class TokenizeToken 
        { 
            public string UserId { get; set; }
            public string UserName { get; set; }
        }

    }
}