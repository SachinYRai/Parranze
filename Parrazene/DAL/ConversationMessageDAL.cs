using Microsoft.EntityFrameworkCore;
using Parrazene.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Parrazene
{
    public class ConversationMessageDAL : BaseDAL, IConversationMessageDAL
    {
        public ConversationMessageDAL(ParrazeneDbContext context) : base(context)
        {
        }

        public override IDataEntity Add(IDataEntity dataEntity)
        {
            dbContext.ConversationMessages.Add(dataEntity as ConversationMessage);
            return dataEntity;
        }

        public override IDataEntity GetById(Guid id)
        {
            return dbContext.ConversationMessages.Find(id);
        }

        public override IDataEntity Update(IDataEntity updatedDataEntity)
        {
            var entity = dbContext.ConversationMessages.Attach(updatedDataEntity as ConversationMessage);

            entity.State = EntityState.Modified;

            return entity as IDataEntity;
        }

        public IEnumerable<ConversationMessage> GetConversationMessages(string id)
        {
            Guid.TryParseExact(id, "D", out var convoId);

            return dbContext.ConversationMessages
                            .Include(cm=>cm.User)
                            .Where(c => c.Conversation.Id == convoId)
                            .OrderByDescending(m => m.DateCreated)
                            .Take(10);
        }
    }
}
