using AspNetCoreRateLimit;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NLog;
using Presentation.ActionFilters;
using Repositories.EFCore;
using Services;
using Services.Contracts;
using WebApi.Extensions;

var builder = WebApplication.CreateBuilder(args);

// There is a file name for Log Process infos write. 
LogManager.LoadConfiguration(string.Concat(Directory.GetCurrentDirectory(), "/nLog.config"));


// Add services to the container.
builder.Services.AddControllers(config =>
{
    config.RespectBrowserAcceptHeader = true;   // Default value is false
    config.ReturnHttpNotAcceptable = true;  // return 406 while HttpFormat is not okey.
    config.CacheProfiles.Add("5 Min", new CacheProfile() { Duration = 300 }); // Caching Profile
})

.AddXmlDataContractSerializerFormatters()  // You can get XML data return
.AddCustomerCsvFormatter()
.AddApplicationPart(typeof(Presentation.AssemblyReference).Assembly);
//.AddNewtonsoftJson(opt => opt.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore)
//.AddJsonOptions(opt => opt.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve);
// Serialize sorununu çözmek için Category entity'sinde [JsonIgnore] özelliði uygulandý.

builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.SuppressConsumesConstraintForFormFileParameters = true;
});



builder.Services.ConfigureJWT(builder.Configuration);
builder.Services.ConfigureIdentity();

builder.Services.AddHttpContextAccessor();
builder.Services.ConfigureRateLimitOption();
builder.Services.AddMemoryCache();
builder.Services.ConfigureHttpCacheHeaders();
builder.Services.ConfigureResponseCaching();
builder.Services.ConfigureVersioning();
builder.Services.AddScoped<IBookLinks, BookLinks>();
builder.Services.AddCustomMediaTypes();
builder.Services.ConfigureDataShaper();
builder.Services.ConfigureCors();
builder.Services.ConfigureActionFilters();
builder.Services.AddEndpointsApiExplorer();
builder.Services.ConfigureSwagger();
builder.Services.ConfigureSqlContext(builder.Configuration); // (service,config)=> You don't have to define Service parametres but Config is required.
builder.Services.ConfigureRepositoryManager();
builder.Services.ConfigureServiceManager();
builder.Services.ConfigureLoggerService();
builder.Services.AddAutoMapper(typeof(Program));  // You can call it with just 1 one row. You don't have to code an Extension.



var app = builder.Build();

// ExpectionHandler Configuration
var logger = app.Services.GetRequiredService<ILoggerService>();
app.ConfigureExpectionHandler(logger);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(s =>
    {
        s.SwaggerEndpoint("/swagger/v1/swagger.json","AkcaApi v1");
        s.SwaggerEndpoint("/swagger/v2/swagger.json", "AkcaApi v2");
    });
}

if(app.Environment.IsProduction())
{
    app.UseHsts(); // Coded after configure ExpectionHandler.
}

app.UseHttpsRedirection();

app.UseIpRateLimiting();
app.UseCors("CorsPolicy");
app.UseResponseCaching();  // It has to be after UseCors define.
app.UseHttpCacheHeaders();

app.UseAuthentication();  // Inserted for Identity Model.
app.UseAuthorization();

app.MapControllers();

app.Run();
