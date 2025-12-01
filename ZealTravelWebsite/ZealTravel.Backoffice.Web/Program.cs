using log4net.Config;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.EntityFrameworkCore;
using ZealTravel.Backoffice.Web.Extensions;
var builder = WebApplication.CreateBuilder(args);


var log4netConfigFile = "log4net.config";
var log4netConfigPath = Path.Combine(Directory.GetCurrentDirectory(), log4netConfigFile);
XmlConfigurator.Configure(new FileInfo(log4netConfigPath));

builder.Services.AddCustomServices(builder.Configuration);
builder.Services.AddCustomAuthentication(builder.Environment.EnvironmentName);
builder.Services.AddCustomAuthorization();
builder.Services.AddCustomMappers();
builder.Services.AddCustomDatabase(builder.Configuration.GetConnectionString("DefaultConnection"));

builder.Services.AddControllersWithViews();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.Configure<ForwardedHeadersOptions>(options =>
{
    options.KnownNetworks.Clear();
    options.KnownProxies.Clear();
    options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
});


var app = builder.Build();
app.UseForwardedHeaders();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.UseSession();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Account}/{action=Login}/{id?}");

app.MapControllers();

app.Run();