﻿using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;

namespace GMaster.Controllers
{
    public class Pricing: Controller
    {
        public Pricing(HttpContext context) : base(context)
        {
        }

        public override string Render(string[] path, string body = "", object metadata = null)
        {
            var scaffold = new Scaffold("/Views/Pricing/pricing.html");

            if (context.Request.Query.ContainsKey("modal"))
            {
                return RenderModal(scaffold.Render());
            }
            else
            {
                return base.Render(path, scaffold.Render(), metadata);
            }
        }
    }
}
