using Microsoft.EntityFrameworkCore;
using MovieApp.Controllers;
using MovieApp.DataBase;
using MovieApp.Services;
using System;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();

// Configure CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigins", policy =>
    {
        policy.WithOrigins("http://localhost:3000") // Add allowed origins (e.g., frontend URL)
              .AllowAnyHeader()
              .AllowAnyMethod();
    });

    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});
// Add services to the container.
// Configurarea bazei Recombee


// Configurarea bazei de date
builder.Services.AddDbContext<MovieAppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<MovieService>();
builder.Services.AddScoped<CSVService>();
builder.Services.AddScoped<FavoriteMovieService>();
builder.Services.AddScoped<PreferenceFormService>();
builder.Services.AddScoped<RecombeeService>(sp =>
{
    var dbContext = sp.GetRequiredService<MovieAppDbContext>();
    var formService = sp.GetRequiredService<PreferenceFormService>();
    //var movie = sp.GetRequiredService<MovieService>();
    var favMovie = sp.GetService<FavoriteMovieService>();
    string recombeeDatabase = "proiect-recom-filme-prod";
    string recombeeToken = "JQBPtARfOrmt46i19cedXCkxGPuC50H0LX4e1RCdFIT9SwjMqt3ZIhj4urETDbP3";
    return new RecombeeService(recombeeDatabase, recombeeToken, dbContext, formService, favMovie);
});
builder.Services.AddScoped<RecommandationController>();


//builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Load data from CSV on application startup
using (var scope = app.Services.CreateScope())
{
    var recombeeService = scope.ServiceProvider.GetRequiredService<RecombeeService>();
    var dbContext = scope.ServiceProvider.GetRequiredService<MovieAppDbContext>();
    string csvFilePath = "NetflixOriginals.csv"; // Ensure this path is correct
    //recombeeService.ConfigureDatabase();
    //recombeeService.LoadDataFromCsv(csvFilePath);
}

app.UseCors("AllowSpecificOrigins");
app.Use(async (context, next) =>
{
    Console.WriteLine($"Request: {context.Request.Method} {context.Request.Path}");
    await next.Invoke();
    Console.WriteLine($"Response Status Code: {context.Response.StatusCode}");
}); 
app.UseRouting();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
