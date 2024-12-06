using MovieApp.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Configurarea bazei Recombee
string recombeeDatabase = "proiect-recom-filme-prod";
string recombeeToken = "JQBPtARfOrmt46i19cedXCkxGPuC50H0LX4e1RCdFIT9SwjMqt3ZIhj4urETDbP3";

// Inițializare RecombeeService
var recombeeService = new RecombeeService(recombeeDatabase, recombeeToken);

// Configurarea bazei de date
// recombeeService.ConfigureDatabase();

// Încărcarea datelor din CSV
string csvFilePath = "NetflixOriginals.csv";
recombeeService.LoadDataFromCsv(csvFilePath);


builder.Services.AddSingleton(recombeeService);

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

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
