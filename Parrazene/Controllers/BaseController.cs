using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Parrazene.Areas.Identity.Data;
using Parrazene.Data;
using Parrazene.Models;

namespace Parrazene.Controllers
{
    [Authorize]
    public abstract class BaseController : Controller
    {
        public UserManager<User> UserManager { get; }
        public ParrazeneDbContext DbContext { get; }

        public BaseController(UserManager<User> userManager, ParrazeneDbContext dbContext)
        {
            UserManager = userManager;
            DbContext = dbContext;
        }
        public User CurrentUser => DbContext.Users.FirstOrDefault(u=>u.Id == UserManager.GetUserId(User));
    }
}