using Microsoft.AspNetCore.Http;

namespace GMaster.Controllers
{
    public class Pay : Controller
    {
        public Pay(HttpContext context, Parameters parameters) : base(context, parameters)
        {
        }

        public override string Render(string[] path, string body = "", object metadata = null)
        {
            if (path[1].ToLower() == "stripe")
            {
                var scaffold = new Scaffold("/Views/Google/Pay/stripe.html");
                scaffold.Data["stripe-key"] = Settings.Stripe.Keys.publicKey;
                scaffold.Data["planId"] = parameters["planId"];
                scaffold.Data["users"] = parameters["users"];
                scaffold.Data["stripe-key"] = Settings.Stripe.Keys.publicKey;
                title = "Gmaster - Secure Pay with Stripe";
                AddScript("https://js.stripe.com/v3/");
                AddScript("/js/views/google/pay/stripe.js");
                return RenderModal(scaffold.Render());
            }
            else if (path[1].ToLower() == "paypal")
            {

            }
            return Error404();
        }
    }
}
