using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Parrazene
{
    public static class Extensions
    {
        public static bool IsLoggedIn(this ClaimsPrincipal claimsPrincipal) => claimsPrincipal.Identity.IsAuthenticated;
    }
}
