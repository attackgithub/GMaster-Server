using Microsoft.AspNetCore.Http;
using System.Linq;

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
                var users = int.Parse(parameters["users"]);
                var planId = int.Parse(parameters["planId"]);
                scaffold["extensionId"] = Settings.Google.Chrome.Extension.Id;
                scaffold["devkey"] = parameters["key"];
                scaffold["email"] = parameters["email"];
                scaffold["planId"] = planId.ToString();
                scaffold["users"] = users.ToString();
                scaffold["stripe-key"] = Settings.Stripe.Keys.publicKey;

                //get price based on users
                var plan = Query.Plans.GetList().Where(p => p.planId == planId).First();
                scaffold["price"] = (plan.price * users).ToString("C");
                scaffold["scheduleId"] = ((int)plan.schedule).ToString();
                scaffold["schedule"] = plan.schedule == Query.Models.PaySchedule.monthly ? "month" : "year";

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
