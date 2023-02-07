
using AuthService.API.Extensions;
using DotNetCoreSample.Auth.Service.Helper;

var builder = WebApplication.CreateBuilder(args);
await builder.RegisterAllDIServices();

var app = builder.Build();
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", string.Concat(builder.Configuration.GetValue<string>("Swagger:Title"), " ", builder.Configuration.GetValue<string>("Swagger:Version")));
});
app.UseReDoc(options =>
{
    options.DocumentTitle = "Swagger Demo Documentation";
    options.SpecUrl = "/swagger/v1/swagger.json";
});

app.UseHttpLogging();
app.UseCors(options => options
             .SetIsOriginAllowedToAllowWildcardSubdomains()
             .WithOrigins(
                          "http://localhost:3000"
                          )
             .AllowAnyHeader()
             .AllowAnyMethod()
             .AllowCredentials()
);

// Tells the application to transmit the cookie through HTTPS only.  
app.ConfigureExceptionHandler(app.Logger);
app.UseHttpsRedirection();
app.UseSession();
app.UseRouting();


app.Use(async (context, next) =>
{
    var path = context.Request.Path;
    if (path.Value.Contains("/swagger/", StringComparison.OrdinalIgnoreCase))
    {
        if (!context.User.Identity.IsAuthenticated)
        {
            context.Response.Redirect("/login");
            return;
        }
        return;
    }

    await next();
});

app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.UseMiddleware<JwtMiddleware>();

app.Run();
