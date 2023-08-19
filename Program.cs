using Microsoft.EntityFrameworkCore;
using WGO_API.Models.CommentModel;
using WGO_API.Models.MarkerModel;
using WGO_API.Models.ReportModel;
using WGO_API.Models.UserModel;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddDbContext<MarkerContext>(opt =>
    opt.UseSqlite("Data Source=Databases/Markers.db"));
builder.Services.AddDbContext<UserContext>(opt =>
    opt.UseSqlite("Data Source=Databases/Users.db"));
builder.Services.AddDbContext<CommentContext>(opt =>
    opt.UseSqlite("Data Source=Databases/Comments.db"));
builder.Services.AddDbContext<ReportContext>(opt =>
    opt.UseSqlite("Data Source=Databases/Reports.db"));
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
