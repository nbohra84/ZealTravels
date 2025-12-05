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
// Add console logging for immediate visibility
builder.Logging.AddConsole();
builder.Logging.SetMinimumLevel(LogLevel.Debug);

// Build configuration FIRST to ensure appsettings.json is loaded before environment variables
// Note: appsettings.Development.json will override appsettings.json in Development mode
var confingBuilder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddJsonFile("appsettings.Development.json", optional: true, reloadOnChange: true) // Development overrides
            .AddEnvironmentVariables() // Environment variables can override
            .AddCommandLine(args);

var configuration = confingBuilder.Build();
ConfigurationHelper.Initialize(configuration);

// Get connection string from the built configuration
var connectionString = configuration.GetConnectionString("DefaultConnection");

if (!string.IsNullOrEmpty(connectionString))
{
    try
    {
        // Parse connection string to extract database name
        var connectionStringBuilder = new Microsoft.Data.SqlClient.SqlConnectionStringBuilder(connectionString);
        string databaseName = connectionStringBuilder.InitialCatalog;
        string serverName = connectionStringBuilder.DataSource;
        
        Console.WriteLine("═══════════════════════════════════════════════════════");
        Console.WriteLine($"🗄️  [DATABASE] === DATABASE CONNECTION INFO ===");
        Console.WriteLine($"🗄️  [DATABASE] Server: {serverName}");
        Console.WriteLine($"🗄️  [DATABASE] Database Name: {databaseName}");
        Console.WriteLine($"🗄️  [DATABASE] Connection String (masked): {connectionString.Replace(connectionStringBuilder.Password ?? "", "***HIDDEN***")}");
        Console.WriteLine($"🗄️  [DATABASE] Source: appsettings.json (with env var override if present)");
        Console.WriteLine("═══════════════════════════════════════════════════════");
        Console.Out.Flush();
        
        // Verify it's the correct database
        if (databaseName != "zealdb_N8_20241108")
        {
            Console.WriteLine($"⚠️  [DATABASE] WARNING: Expected database 'zealdb_N8_20241108' but got '{databaseName}'");
            Console.WriteLine($"⚠️  [DATABASE] This might be overridden by environment variable or user secrets!");
            Console.Out.Flush();
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine("═══════════════════════════════════════════════════════");
        Console.WriteLine($"🔴 [DATABASE] ERROR parsing connection string: {ex.Message}");
        Console.WriteLine($"🔴 [DATABASE] Connection String: {connectionString}");
        Console.WriteLine("═══════════════════════════════════════════════════════");
        Console.Out.Flush();
    }
}
else
{
    Console.WriteLine("═══════════════════════════════════════════════════════");
    Console.WriteLine($"🔴 [DATABASE] WARNING: Connection string is NULL or EMPTY!");
    Console.WriteLine("═══════════════════════════════════════════════════════");
    Console.Out.Flush();
}

builder.Services.AddCustomServices(builder.Configuration);
builder.Services.AddCustomAuthentication(builder.Environment.EnvironmentName);
builder.Services.AddCustomAuthorization();
builder.Services.AddCustomMappers();
builder.Services.AddCustomDatabase(connectionString);
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

// Configuration already built above - ConfigurationHelper already initialized

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNamingPolicy = null; // Preserve case
        options.JsonSerializerOptions.DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.Never;
        options.JsonSerializerOptions.PropertyNameCaseInsensitive = true; // Allow case-insensitive matching
    });

// Add CORS policy
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
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
    // Add detailed exception page for development
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// CORS middleware - Required for API endpoints
app.UseCors("AllowAll");

app.UseAuthentication();
app.UseAuthorization();
app.UseSession();

// Use UseEndpoints for proper CORS middleware integration
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Account}/{action=Login}/{id?}");
    endpoints.MapControllers();
});

app.Run();
