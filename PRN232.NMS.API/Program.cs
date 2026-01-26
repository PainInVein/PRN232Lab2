using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PRN232.NMS.API.Models.MappingTool;
using PRN232.NMS.API.Models.RequestModels;
using PRN232.NMS.Repo.DBContext;
using PRN232.NMS.Services;
using PRN232.NMS.Services.Interfaces;
using static System.Runtime.InteropServices.JavaScript.JSType;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddScoped<ISystemAccountService, SystemAccountService>();
builder.Services.AddScoped<ITagService, TagService>();
builder.Services.AddScoped<INewsArticleService, NewsArticleService>();

builder.Services.AddControllers();
builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.InvalidModelStateResponseFactory = context =>
    {
        var errors = context.ModelState
            .Where(x => x.Value!.Errors.Count > 0)
            .ToDictionary(
                kvp => kvp.Key,
                kvp => kvp.Value!.Errors.Select(e => e.ErrorMessage).ToArray()
            );

        var response = new
        {
            message = "Validation failed",
            isSuccess = false,
            data = errors,
            errors = (string?)null
        };

        return new BadRequestObjectResult(response);
    };
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


//Mapper
builder.Services.AddAutoMapper(typeof(MappingProfile).Assembly);

var app = builder.Build();
app.UseMiddleware<PRN232.NMS.API.Middlewares.ExceptionMiddleware>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
