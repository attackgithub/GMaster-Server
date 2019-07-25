using System;
using System.Linq;
using Microsoft.AspNetCore.Http;

namespace GMaster.Services
{
    public class Teams : Service
    {
        public Teams(HttpContext context, Parameters parameters) : base(context, parameters) { }

        public string Index(int subscriptionId)
        {
            if (!HasPermissions()) { return Error(); }
            try
            {
                var subscriptions = Query.Subscriptions.GetSubscriptions(User.userId);
                var subscription = subscriptions.Where(a => a.subscriptionId == subscriptionId).FirstOrDefault();
                if (subscription != null)
                {
                    if(subscription.roleType <= Query.Models.RoleType.moderator)
                    {
                        var members = Query.TeamMembers.GetList(subscription.teamId).Select(a => new { a.email, a.roleType, a.name });
                        return JsonResponse(members);
                    }
                }
            }
            catch (Exception) { }
            return Empty();
        }
    }
}
