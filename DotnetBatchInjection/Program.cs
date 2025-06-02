using DotnetBatchInjection.Apis;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.AddApplicationServices();

var app = builder.Build();

app.UseExceptionHandling();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
    await app.MigrateAsync();
}

app.UseHttpsRedirection();

app.MapAtsApi();

app.Run();
