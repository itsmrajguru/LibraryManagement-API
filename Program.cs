using LibraryManagementAPI.Services;

using Microsoft.EntityFrameworkCore;
using LibraryManagementAPI.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddSwaggerGen();

// Configure EF Core with Pomelo MySQL
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<LibraryDbContext>(options =>
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

// DI: Tell ASP.NET — "When IBookService is needed, give DatabaseBookService"
// AddScoped = new instance per HTTP request (good practice for DbContext)
builder.Services.AddScoped<IBookService, DatabaseBookService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();
app.MapControllers();
app.Run();
