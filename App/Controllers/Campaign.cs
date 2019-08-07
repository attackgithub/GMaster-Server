﻿using System;
using Microsoft.AspNetCore.Http;

namespace GMaster.Controllers
{
    public class Campaign : Controller
    {
        public Campaign(HttpContext context, Parameters parameters) : base(context, parameters)
        {
        }

        public override string Render(string[] path, string body = "", object metadata = null)
        {
            if (!HasPermissions(Query.Models.LogApi.Names.CampaignDetails)) { return ""; }
            if (path.Length <= 1)
            {
                context.Response.StatusCode = 500;
                return "URL is missing parameters";
            }
            try
            {
                var campaignId = int.Parse(path[1]);
                var html = "Campaign " + campaignId;
                if (parameters.ContainsKey("nolayout"))
                {
                    return RenderCORS(html);
                }
                return base.Render(path, html, metadata);
            }
            catch (Exception)
            {
                return Error("Error retrieving campaign details (10102). Please report error to " + Settings.ContactInfo.CustomerService.email);
            }
        }
    }
}
