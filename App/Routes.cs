using Datasilk.Mvc;
using Datasilk.Web;
using Microsoft.AspNetCore.Http;

public class Routes: Datasilk.Web.Routes
{
    public override Controller FromControllerRoutes(HttpContext context, Parameters parameters, string name)
    {
        switch (name)
        {
            case "home": case "":
                return new GMaster.Controllers.Home(context, parameters);
            case "login":
                return new GMaster.Controllers.Login(context, parameters);
            case "dashboard":
                return new GMaster.Controllers.Dashboard(context, parameters);
            case "googlesignin":
                return new GMaster.Controllers.GoogleSignIn(context, parameters);
            case "features":
                return new GMaster.Controllers.Features(context, parameters);
            case "pay":
                return new GMaster.Controllers.Pay(context, parameters);
            case "subscription":
                return new GMaster.Controllers.Subscription(context, parameters);
            case "gmail_version":
                return new GMaster.Controllers.GmailVersion(context, parameters);
            case "gmailjs":
                return new GMaster.Controllers.GmailJs(context, parameters);
            case "gmailcss":
                return new GMaster.Controllers.GmailCss(context, parameters);
        }
        return base.FromControllerRoutes(context, parameters, name);
    }

    public override Service FromServiceRoutes(HttpContext context, Parameters parameters, string name)
    {
        switch (name)
        {
            case "addressbook" :
                return new GMaster.Services.AddressBook(context, parameters);
            case "google":
                return new GMaster.Services.Google(context, parameters);
            case "plans":
                return new GMaster.Services.Plans(context, parameters);
            case "stripewebhook":
                return new GMaster.Services.StripeWebHook(context, parameters);
            case "subscriptions":
                return new GMaster.Services.Subscriptions(context, parameters);
            case "user":
                return new GMaster.Services.User(context, parameters);
        }
        return base.FromServiceRoutes(context, parameters, name);
    }
}
