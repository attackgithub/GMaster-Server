using Datasilk.Mvc;
using Datasilk.Web;
using Microsoft.AspNetCore.Http;

public class Routes: Datasilk.Web.Routes
{
    public override Controller FromControllerRoutes(HttpContext context, string name)
    {
        switch (name)
        {
            case "Home": case "":
                return new GMaster.Controllers.Home(context);
        }
        return base.FromControllerRoutes(context, name);
    }

    public override Service FromServiceRoutes(HttpContext context, string name)
    {
        switch (name)
        {
            case "AddressBook" :
                return new GMaster.Services.AddressBook(context);
        }
        return base.FromServiceRoutes(context, name);
    }
}
