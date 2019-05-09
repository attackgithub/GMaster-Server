using System;

namespace GMaster.Common.Stripe
{
    public static class User
    {
        public static Query.Models.User GetUserFromCustomerId(string customerId)
        {
            var user = Query.Users.GetByStripeCustomerId(customerId);
            if (user != null)
            {
                return user;
            }
            else
            {
                throw new Exception("Stripe customer Id was not found in User table");
            }
        }
    }
}
