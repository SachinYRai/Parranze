using Microsoft.AspNetCore.Mvc.Formatters;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Parrazene
{
     public  class Conversation : IDataEntity
    {

        public Conversation(Guid id)
        {
            DateCreated = DateTime.Now;
            Id = id;
        }

         public Guid Id { get; set; }

#nullable enable
        public string? Name { get; set; }

        public DateTime DateCreated { get; set; }

        [InverseProperty("Conversation")]
        public IEnumerable<ConversationMessage> Messages { get; set; }

        public Guid? LatestMessageId { get; set; }
        public ConversationMessage? LatestMessage { get; set; }
    }
}
