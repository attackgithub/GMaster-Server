using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Utility.Serialization;

namespace GMaster.Services
{
    public class Subscriptions : Service
    {
        public Subscriptions(HttpContext context) : base(context)
        {
        }

        public string GetInfo(int userId)
        {
            if (!HasPermissions()) { return ""; }
            try
            {
                //TODO: Get subscription info for user
            }
            catch (Exception ex)
            {
                return Error(ex.Message);
            }
            return "";
        }
    }
}
