using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NLog;
using Repositories.EFCore;
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
})
    .AddCustomerCsvFormatter()
    .AddXmlDataContractSerializerFormatters()  // You can get XML data return
    .AddApplicationPart(typeof(Presentation.AssemblyReference).Assembly) 
    .AddNewtonsoftJson();

builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.SuppressConsumesConstraintForFormFileParameters = true;
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
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
    app.UseSwaggerUI();
}

if(app.Environment.IsProduction())
{
    app.UseHsts(); // Coded after configure ExpectionHandler.
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
