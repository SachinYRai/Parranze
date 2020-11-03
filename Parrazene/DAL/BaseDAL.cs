using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Routing;
using Parrazene.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;

namespace Parrazene
{
    public abstract class BaseDAL
    {
        protected readonly ParrazeneDbContext dbContext;

        public BaseDAL(ParrazeneDbContext context)
        {
            dbContext = context;
        }

        public abstract IDataEntity Add(IDataEntity dataEntity);
        public void Delete(int id)
        {
            var entity = GetById(Guid.NewGuid());

            if (entity != null)
                dbContext.Remove(entity);
        }
        public abstract IDataEntity Update(IDataEntity updatedDataEntity);

        public abstract IDataEntity GetById(Guid id);

        // do I need this?
        public Task<int> Commit()
        {
            return dbContext.SaveChangesAsync();
        }
    }
}
