using Parrazene.Areas.Identity.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Parrazene
{
    public class ConversationUser : IDataEntity
    {
        public ConversationUser()
        {
            Id = Guid.NewGuid();
        }

        public Guid Id { get; set; }
        public Guid ConversationId { get; set; }
        public Conversation Conversation { get; set; }

        // User Id is in nvarchar form so adding a guid type here will create a new column
        // Adding a string instead

        public string UserId { get; set; }
        public User User { get; set; }
    }
}
