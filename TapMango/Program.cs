using Microsoft.AspNetCore.Builder;
// ...existing code...
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TapMango.Services; 

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton(new TapMango.Services.RateLimiter(5, 20)); 
builder.Services.AddControllers();
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
