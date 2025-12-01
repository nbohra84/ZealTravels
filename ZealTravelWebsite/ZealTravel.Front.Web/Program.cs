using log4net;
using log4net.Config;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.EntityFrameworkCore;
using ZealTravel.Common.Helpers;
using ZealTravel.Front.Web.Extensions;
using ZealTravel.Front.Web.Helper.Flight;
using Microsoft.Extensions.Logging;


var builder = WebApplication.CreateBuilder(args);


var log4netConfigFile = "log4net.config";

var logRepository = LogManager.GetRepository(System.Reflection.Assembly.GetEntryAssembly());
XmlConfigurator.Configure(logRepository, new FileInfo(log4netConfigFile));

// Add log4net to the logging pipeline
builder.Logging.ClearProviders(); // Optional: Removes default logging providers
builder.Logging.AddLog4Net(log4netConfigFile);

builder.Services.AddCustomServices(builder.Configuration);
builder.Services.AddCustomAuthentication(builder.Environment.EnvironmentName);
builder.Services.AddCustomAuthorization();
builder.Services.AddCustomMappers();
builder.Services.AddCustomDatabase(builder.Configuration.GetConnectionString("DefaultConnection"));
builder.Services.AddHttpContextAccessor();


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

var confingBuilder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddEnvironmentVariables()
            .AddCommandLine(args);

var configuration = confingBuilder.Build();
ConfigurationHelper.Initialize(configuration);

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNamingPolicy = null; // Preserve case
        options.JsonSerializerOptions.DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.Never;
    });

builder.WebHost.ConfigureKestrel(options =>
{
    options.Limits.KeepAliveTimeout = TimeSpan.FromMinutes(10);
    options.Limits.RequestHeadersTimeout = TimeSpan.FromMinutes(10);
});


var app = builder.Build();

HttpContextHelper.Configure(app.Services.GetRequiredService<IHttpContextAccessor>());
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
