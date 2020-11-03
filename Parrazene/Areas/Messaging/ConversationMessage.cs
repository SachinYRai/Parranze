using Parrazene.Areas.Identity.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Parrazene
{
    public partial class ConversationMessage : IDataEntity
    {
        public ConversationMessage()
        {
            DateCreated = DateTime.Now;
            Id = Guid.NewGuid();
        }

        public Guid Id { get; set; }
        public User User { get; set; }

        public DateTime DateCreated { get; private set; }

        public string Message { get; set; }

        public Conversation Conversation { get; set; }
    }
}
