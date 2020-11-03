using Microsoft.AspNetCore.Mvc.ActionConstraints;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Parrazene.Areas.Identity.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Parrazene.Models
{
    public class ChatViewModel : PageModel
    {
        public IEnumerable<User> Users { get; set; }
        public IEnumerable<Conversation> Conversations { get; set; }
        public string Dp { get; set; }

        //IChatDAL chatDAL;

        public ChatViewModel()
        {

        }

    }
}
