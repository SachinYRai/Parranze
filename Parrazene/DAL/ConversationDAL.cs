using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Parrazene.Areas.Identity.Data;
using Parrazene.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;

namespace Parrazene 
{
    public class ConversationDAL : BaseDAL, IConversationDAL
    {
        public ConversationDAL(ParrazeneDbContext context) : base(context)
        {
        }

        public override IDataEntity Add(IDataEntity dataEntity)
        {
           dbContext.Conversations.Add(dataEntity as Conversation);

            return dataEntity;
        }

        public override IDataEntity GetById(Guid id)
        {
            return dbContext.Conversations.Find(id);
        }

        public override IDataEntity Update(IDataEntity updatedDataEntity)
        {
            var entity = dbContext.Conversations.Attach(updatedDataEntity as Conversation);

            entity.State = EntityState.Modified;

            return entity as IDataEntity;

        }

        public string GetOrCreateConversationId(List<string> userIds)
        {
            var userConvos = (from cu in dbContext.ConversationUsers
                                     where (from cu2 in dbContext.ConversationUsers
                                            where userIds.Any(uId => uId == cu2.UserId)
                                            select cu2.ConversationId).Contains(cu.ConversationId)

                                     group cu by cu.ConversationId into g
                                     where g.Count() == userIds.Count()

                                     select g.Key);

            var userConversation= (from cu in dbContext.ConversationUsers
                             where userConvos.Contains(cu.ConversationId) && userIds.Contains(cu.UserId)

                             group cu by cu.ConversationId into g
                             where g.Count() == userIds.Count()

                             select g.Key).SingleOrDefault();

            return (userConversation != default(Guid) ? userConversation : Guid.NewGuid()).ToString();

        }
    }
}