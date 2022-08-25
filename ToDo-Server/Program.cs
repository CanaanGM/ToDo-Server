using Microsoft.EntityFrameworkCore;

using ToDo_Server.Data.DbAccess;
using ToDo_Server.Data.Mapper;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


//! extract into an extension class
builder.Services.AddAutoMapper(typeof(MapperProfiles).Assembly);
builder.Services.AddDbContext<SqliteDbContext>(opt => opt.UseSqlite(
    builder.Configuration["ConnectionStrings:SqliteTempBase"]
    ));
//! end extraction ~!

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
