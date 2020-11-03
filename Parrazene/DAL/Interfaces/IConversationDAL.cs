using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Parrazene
{
   public interface IConversationDAL
    {
        public string GetOrCreateConversationId(List<string> userIds);
    }
}
