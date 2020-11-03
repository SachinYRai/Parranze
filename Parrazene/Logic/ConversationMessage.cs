using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Parrazene
{
    public partial class ConversationMessage
    {
        public string GetMessageView()
        {
            return $"{User.UserName}: <br/> {Message}";
        }
    }
}
