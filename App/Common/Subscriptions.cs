using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GMaster.Common
{
    public static class Subscriptions
    {
        public static Models.Subscriptions.Refund CalculateRefund(Query.Models.Subscription subscription)
        {

            var refund = new Models.Subscriptions.Refund()
            {

            };
            return refund;
        }
    }
}
