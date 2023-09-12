using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;
using TeamsApi.Context;
using TeamsApi.Extensions;
using TeamsApi.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySQL(builder.Configuration.GetConnectionString("CnnStr")!));

//Add Mvc options
builder.Services.AddFluentValidationAutoValidation().AddFluentValidationClientsideAdapters();
builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Adicionar os Servicios
builder.Services.AddScoped<ITeamService, TeamService>();
builder.Services.AddScoped<ITeamMemberService, TeamMemberService>();

builder.Services.AddServices();
builder.Services.AddMapping();

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