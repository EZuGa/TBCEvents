global using C_.Models;
global using C_.Services.EventService;
global using C_.Dtos.Event;
global using AutoMapper;
global using Microsoft.EntityFrameworkCore;
global using C_.Data;
global using C_.Middlewars;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddDbContext<DataContext>(options =>
options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAutoMapper(typeof(Program).Assembly);
builder.Services.AddScoped<IEventService, EventService>();
builder.Services.AddTransient<GlobalExceptionHandlingMiddleware>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
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
