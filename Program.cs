global using C_.Models;
global using C_.Services.EventService;
global using C_.Dtos.Event;
global using AutoMapper;
global using Microsoft.EntityFrameworkCore;
global using C_.Data;
global using C_.Middlewars;
global using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using C_.Options;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddDbContext<DataContext>(options =>
options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.ConfigureOptions<ConfigureSwaggerOptions>();
builder.Services.AddAutoMapper(typeof(Program).Assembly);
builder.Services.AddScoped<IEventService, EventService>();
builder.Services.AddTransient<GlobalExceptionHandlingMiddleware>();

builder.Services.AddApiVersioning(config=>{
    config.DefaultApiVersion = new ApiVersion(1, 0);
    config.AssumeDefaultVersionWhenUnspecified = true;
    config.ReportApiVersions = true;
    config.ApiVersionReader = new UrlSegmentApiVersionReader();
});


builder.Services.AddVersionedApiExplorer(setup=>{
    setup.GroupNameFormat = "'v'VVV";
    setup.SubstituteApiVersionInUrl = true;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options => 
    {
        var provider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();

        foreach (var description in provider.ApiVersionDescriptions)
        {
            options.SwaggerEndpoint(
                $"/swagger/{description.GroupName}/swagger.json",
                description.ApiVersion.ToString()
            );
        }
    });
}

app.UseHttpsRedirection();

app.UseAuthorization();

// app.Use(async (context, next) =>{

//     try{
//         await next(context);
//     }catch(Exception e){
//         Console.WriteLine("EXLA GAAERORA!!");
//         context.Response.StatusCode = 501;
//     }
// });

app.UseMiddleware<GlobalExceptionHandlingMiddleware>();

app.MapControllers();

app.Run();
