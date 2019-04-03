using Datasilk.Mvc;
using Datasilk.Web;
using Microsoft.AspNetCore.Http;

public class Routes: Datasilk.Web.Routes
{
    public override Controller FromControllerRoutes(HttpContext context, Parameters parameters, string name)
    {
        switch (name)
        {
            case "Home": case "":
                return new GMaster.Controllers.Home(context, parameters);
            case "Login":
                return new GMaster.Controllers.Login(context, parameters);
            case "Dashboard":
                return new GMaster.Controllers.Dashboard(context, parameters);
            case "GoogleSignIn":
                return new GMaster.Controllers.GoogleSignIn(context, parameters);
            case "Pricing":
                return new GMaster.Controllers.Pricing(context, parameters);
        }
        return base.FromControllerRoutes(context, parameters, name);
    }

    public override Service FromServiceRoutes(HttpContext context, Parameters parameters, string name)
    {
        switch (name)
        {
            case "AddressBook" :
                return new GMaster.Services.AddressBook(context, parameters);
            case "Google":
                return new GMaster.Services.Google(context, parameters);
            case "Plans":
                return new GMaster.Services.Plans(context, parameters);
            case "Subscriptions":
                return new GMaster.Services.Subscriptions(context, parameters);
            case "User":
                return new GMaster.Services.User(context, parameters);
        }
        return base.FromServiceRoutes(context, parameters, name);
    }
}
