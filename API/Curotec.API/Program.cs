
using Curotec.API.Middlewares;
using Curotec.Data;
using Curotec.Models.Dtos;
using Curotec.Models.Translator;
using Curotec.Repository;
using Curotec.Repository.Interfaces;
using Curotec.Services;
using Curotec.Services.Interfaces;
using Curotec.Services.Validators;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("AZURE_SQL_CONNECTIONSTRING"), b => b.MigrationsAssembly(typeof(AppDbContext).Assembly.FullName).EnableRetryOnFailure(3)));
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IProcessBatchService, ProcessBatchService>();
builder.Services.AddScoped<IValidator<ProductDto>, ProductValidator>();
builder.Services.AddAutoMapper(typeof(ProductProfile));
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddFluentValidationClientsideAdapters();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
            {

                options.SwaggerDoc("v1", new OpenApiInfo { Title = "Curotec API", Version = "v1" });
                options.EnableAnnotations();

            });

builder.Services.AddLogging();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseMiddleware<CancellationTrackingMiddleware>();
app.UseMiddleware<MetricsMiddleware>();

app.UseCors(options => options.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());// allow any origin, method, and header for CORS for test porpouses
app.UseAuthorization();
app.MapControllers();
app.Run();
