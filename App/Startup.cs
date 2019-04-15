using System.IO;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Stripe;

public class Startup: Datasilk.Startup
{
    public override void ConfiguringServices(IServiceCollection services)
    {
        base.ConfiguringServices(services);

        services.AddCors(options =>
        {
            options.AddPolicy("google",
                builder =>
                {
                    builder.WithOrigins(
                        "chrome-extension://kdcpigikfhpokfbbklgdeeheajkndiam"
                    )
                    .AllowAnyHeader()
                    .AllowAnyMethod();
                }
            );
        });
    }
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

        //use CORS for cross-domain requests
        app.UseCors("google");

        //set up database
        Query.Sql.connectionString = Server.sqlConnectionString;
        Server.hasAdmin = Query.Users.HasAdmin();

        //load global settings (auth.json)
        if (System.IO.File.Exists(Server.MapPath("auth.json")))
        {
            IConfigurationRoot authConfig = new ConfigurationBuilder()
                .AddJsonFile(Server.MapPath("auth.json")).Build();
            var environment = "development";
            switch (Server.environment)
            {
                case Server.Environment.development:
                    break;
                case Server.Environment.production:
                    environment = "production";
                    break;
            }

            //Google settings
            GMaster.Settings.Google.OAuth2.clientId = authConfig.GetSection("google:OAuth2:clientId").Value;
            GMaster.Settings.Google.OAuth2.secret = authConfig.GetSection("google:OAuth2:secret").Value;
            GMaster.Settings.Google.OAuth2.extensionId = authConfig.GetSection("google:OAuth2:extensionId").Value;
            GMaster.Settings.Google.OAuth2.redirectURI = authConfig.GetSection("google:OAuth2:redirectURI:" + environment).Value;
            //Stripe settings
            GMaster.Settings.Stripe.Keys.publicKey = authConfig.GetSection("stripe:keys:" + environment + ":public").Value;
            GMaster.Settings.Stripe.Keys.privateKey = authConfig.GetSection("stripe:keys:" + environment + ":secret").Value;

            //Stripe Configuration
            StripeConfiguration.SetApiKey(GMaster.Settings.Stripe.Keys.privateKey);
        }


        base.Configured(app, env, config);
    }
}
