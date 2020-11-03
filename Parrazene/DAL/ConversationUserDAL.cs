using Microsoft.EntityFrameworkCore;
using Parrazene.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Parrazene
{
    public class ConversationUserDAL : BaseDAL, IConversationUserDAL
    {
        public ConversationUserDAL(ParrazeneDbContext context) : base(context)
        {
        }

        public override IDataEntity Add(IDataEntity dataEntity)
        {
            dbContext.ConversationUsers.Add(dataEntity as ConversationUser);

            return dataEntity;
        }

        public override IDataEntity GetById(Guid id)
        {
            return dbContext.ConversationUsers.Find(id);
        }

        public override IDataEntity Update(IDataEntity updatedDataEntity)
        {
            var entity = dbContext.ConversationUsers.Attach(updatedDataEntity as ConversationUser);

            entity.State = EntityState.Modified;

            return entity as IDataEntity;
        }
    }
}
