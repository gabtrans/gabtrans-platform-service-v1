using GabTrans.Api.Configuration;
using GabTrans.Domain.Models;
using GabTrans.Infrastructure.Repositories;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

//builder.Host.UseSerilog((ctx, cfg) => cfg.ReadFrom.Configuration(new ConfigurationBuilder().AddJsonFile("appsettings.json").Build()));

builder.Host.UseSerilog((ctx, services, cfg) => cfg.ReadFrom.Configuration(ctx.Configuration).ReadFrom.Services(services).WriteTo.Console().WriteTo.AzureBlobStorage(
    connectionString: StaticData.StorageConnectionStrings,
    storageContainerName: "logs",
    storageFileName: $"log_{DateTime.Now.ToString("yyyy-MM-dd")}.txt",
    outputTemplate: "{Timestamp:HH:mm:ss.fff zzz} [{Level}] {Message}{NewLine}{Exception}")
);

await ApplicationRepository.LoadAppInfoAsync();

builder.Services.InstallServices(builder.Configuration, typeof(IServiceInstaller).Assembly);

var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment() || app.Environment.IsProduction())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();

   // app.UseSwaggerAuthorized();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseSerilogRequestLogging();
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseCors("SecurePolicy"); // Must come before UseAuthorization

//app.UseCors(x => x.AllowAnyMethod().AllowAnyHeader().SetIsOriginAllowed(origin => true).AllowCredentials());

app.Use(async (context, next) =>
{
    context.Response.Headers.StrictTransportSecurity = "max-age=31536000; includeSubDomains; preload";
    context.Response.Headers.XFrameOptions = "DENY";
    context.Response.Headers.XContentTypeOptions = "nosniff";
    context.Response.Headers["Referrer-Policy"] = "strict-origin-when-cross-origin";
    context.Response.Headers["Permissions-Policy"] = "geolocation=(), microphone=()";
    context.Response.Headers.ContentSecurityPolicy = "default-src 'self'; script-src 'self'";
    await next();
});

app.UseAuthentication();

app.UseRateLimiter();

app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");
});

app.Run();
