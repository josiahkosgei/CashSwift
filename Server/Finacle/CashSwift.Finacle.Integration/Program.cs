using MediatR;
using System.Reflection;
using CashSwift.Finacle.Integration.CQRS.Helpers;
using CashSwift.Finacle.Integration.Extensions;
using Microsoft.OpenApi.Models;
using FluentValidation;
using CashSwift.Finacle.Integration.CQRS.Validation;
using NLog.Web;
using Microsoft.EntityFrameworkCore;
using CashSwift.Finacle.Integration.DataAccess;
using CashSwift.Finacle.Integration.CQRS.Services.IServices;
using CashSwift.Finacle.Integration.CQRS.Services;
using CashSwift.Library.Standard.Logging;
using CashSwift.Finacle.Integration.Filters;
using CashSwift.Finacle.Integration.DataAccess.Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.AspNetCore.Mvc.ApiExplorer;

var builder = WebApplication.CreateBuilder(args);


ConfigurationManager configuration = builder.Configuration;
IWebHostEnvironment environment = builder.Environment;
builder.Configuration
    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
    .AddJsonFile($"appsettings.{environment.EnvironmentName}.json", true, true);


builder.Services.AddControllers(options =>
{
    options.Filters.Add<HMACAuthenticationFilter>();
})
    .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.WriteIndented = true;
                options.JsonSerializerOptions.Converters.Add(new CustomJsonConverterForType());
                options.JsonSerializerOptions.Converters.Add(new ExceptionConverter());
            });
builder.Services.AddSingleton<IAPIUserDataAccess, APIUserDataAccess>();
//builder.Services.AddSingleton<ITranslationDataAccess, TranslationDataAccess>();
builder.Services.AddTransient<IIntegrationDataAccess, IntegrationDataAccess>();
builder.Services.Configure<SOAServerConfiguration>(configuration.GetSection("SOAServerConfiguration"));
builder.Services.AddTransient<IAccountManagerService, AccountManagerService>();
builder.Services.AddTransient<ICashSwiftAPILogger, CashSwiftAPILogger>();
builder.Services.AddTransient<DepositorServerContextProcedures>();
builder.Services.AddScoped<HMACAuthenticationFilter>();
builder.Services.AddDbContext<DepositorServerContext>(options =>
    options.UseSqlServer(configuration.GetConnectionString("Default"), builder =>
        {
            builder.EnableRetryOnFailure(5, TimeSpan.FromSeconds(10), null);
        }));

builder.Services.AddApiVersioning(opt =>
                                    {
                                        opt.DefaultApiVersion = new ApiVersion(1, 0);
                                        opt.AssumeDefaultVersionWhenUnspecified = true;
                                        opt.ReportApiVersions = true;
                                        opt.ApiVersionReader = ApiVersionReader.Combine(
                                        new QueryStringApiVersionReader("api-version"),
                                        new HeaderApiVersionReader("X-Version"),
                                        new MediaTypeApiVersionReader("ver"));
                                    });

builder.Services.AddVersionedApiExplorer(setup =>
{
    setup.GroupNameFormat = "'v'VVV";
    setup.SubstituteApiVersionInUrl = true;
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//builder.Services.AddSwaggerGen(swaggerOptions =>
//            {
//                swaggerOptions.SwaggerDoc("v1", new OpenApiInfo { Title = "CashSwift Finacle Integration Api", Version = "v1" });
//            });
builder.Services.ConfigureOptions<ConfigureSwaggerOptions>();

builder.Services.AddHttpContextAccessor();
builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());

builder.Services.AddMediatR(Assembly.GetExecutingAssembly());
builder.Services.AddValidatorsFromAssemblyContaining<AccountValidationValidator>();


// NLog: Setup NLog for Dependency injection
builder.Logging.ClearProviders();
builder.Host.UseNLog();

var app = builder.Build();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{


var apiVersionDescriptionProvider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();

app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.DefaultModelsExpandDepth(-1);
    foreach (var description in apiVersionDescriptionProvider.ApiVersionDescriptions)
    {
        options.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json",
            description.GroupName.ToUpperInvariant());
    }
});
//}
//app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthorization();
app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
app.MigrateDatabase<DepositorServerContext>((context, services) => { }).Run();
