using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using PRN232.NMS.API.Models.MappingTool;
using PRN232.NMS.API.Models.RequestModels;
using PRN232.NMS.API.Models.ResponseModels;
using PRN232.NMS.Services;
using PRN232.NMS.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddScoped<ISystemAccountService, SystemAccountService>();
builder.Services.AddScoped<ITagService, TagService>();
builder.Services.AddScoped<INewsArticleService, NewsArticleService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();

builder.Services.AddControllers();

//Customizing Model Validation Error Response
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

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseHttpsRedirection();

app.UseExceptionHandler(appError =>
{
    appError.Run(async context =>
    {
        context.Response.StatusCode = StatusCodes.Status500InternalServerError;
        context.Response.ContentType = "application/json";
        var feature = context.Features.Get<IExceptionHandlerFeature>();
        var ex = feature?.Error;
        var response = new ResponseDTO<object>(
            "An error occurred while processing your request.",
            false,
            null,
            null);
        await context.Response.WriteAsJsonAsync(response);
    });
});

app.UseAuthorization();

app.MapControllers();

app.Run();
