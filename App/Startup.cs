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

        base.Configured(app, env, config);
    }
}
