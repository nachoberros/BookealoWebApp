using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BookealoWebApp.Server.Controllers
{
    public abstract class BookealoBaseController : ControllerBase
    {
        protected int? AccountId
        {
            get
            {
                var stringAccountId = User?.Claims.FirstOrDefault(c => c.Type == "accountId")?.Value;
                if (int.TryParse(stringAccountId, out int accountId))
                {
                    return accountId;
                }
                return null;
            }
        }
    }
}
