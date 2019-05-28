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
                var emails = parameters["emails"];
                var emailList = emails.Split(',');
                var planId = int.Parse(parameters["planId"]);
                scaffold["extensionId"] = Settings.Google.Chrome.Extension.Id;
                scaffold["devkey"] = parameters["key"];
                scaffold["email"] = parameters["email"];
                scaffold["planId"] = planId.ToString();
                scaffold["emails"] = emails;
                scaffold["stripe-key"] = Settings.Stripe.Keys.publicKey;

                //get price based on users
                var plan = Query.Plans.GetList().Where(p => p.planId == planId).First();
                scaffold["price"] = (plan.price * emailList.Length).ToString("C");
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
