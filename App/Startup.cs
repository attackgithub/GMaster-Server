﻿using System.IO;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

public class Startup: Datasilk.Startup
{
    public override void Configured(IApplicationBuilder app, IHostingEnvironment env, IConfigurationRoot config)
    {
        //set up HTTPS
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }
        else
        {
            app.UseHsts();
        }
        app.UseHttpsRedirection();

        //set up database
        Query.Sql.connectionString = Server.sqlConnectionString;
        Server.hasAdmin = Query.Users.HasAdmin();

        //load global settings (auth.json)
        if (File.Exists(Server.MapPath("auth.json")))
        {
            IConfigurationRoot authConfig = new ConfigurationBuilder()
                .AddJsonFile(Server.MapPath("auth.json")).Build();
            
            GMaster.Settings.Google.OAuth2.clientId = authConfig.GetSection("google:OAuth2:clientId").Value;
            GMaster.Settings.Google.OAuth2.secret = authConfig.GetSection("google:OAuth2:secret").Value;
            GMaster.Settings.Google.OAuth2.extensionId = authConfig.GetSection("google:OAuth2:extensionId").Value;
            switch (Server.environment)
            {
                case Server.Environment.development:
                    GMaster.Settings.Google.OAuth2.redirectURI = authConfig.GetSection("google:OAuth2:redirectURI:development").Value;
                    break;
                case Server.Environment.production:
                    GMaster.Settings.Google.OAuth2.redirectURI = authConfig.GetSection("google:OAuth2:redirectURI:production").Value;
                    break;
            }
            
            
        }


        base.Configured(app, env, config);
    }
}
