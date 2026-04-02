using IRS.Application.Interfaces;
using IRS.Application.Services;
using IRS.Domain.Interfaces;
using IRS.Infrastructure.Repositories;
using System;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

// Logging
builder.Logging.ClearProviders();
builder.Logging.AddConsole();

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


// DI registrations
builder.Services.AddSingleton<IReservationRepository, InMemoryReservationRepository>();
builder.Services.AddScoped<IReservationService, ReservationService>();
builder.Services.AddSingleton<IInventoryRepository, InMemoryInventoryRepository>();


var app = builder.Build();

// Middleware
app.UseRouting();

// Enable Swagger
app.UseSwagger();
app.UseSwaggerUI();

app.UseRouting();
app.MapControllers();

app.Run();