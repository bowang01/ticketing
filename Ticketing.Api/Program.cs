using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;
using Ticketing.Api.GraphQL;
using Ticketing.Api.Middleware;
using Ticketing.Application;
using Ticketing.Infrastructure;
using Ticketing.Infrastructure.Persistence;

var builder = WebApplication.CreateBuilder(args);

//add cors
builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy", policy =>
    {
        policy
            .WithOrigins("http://localhost:5173", "https://localhost:5173")
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials(); // cookie or SignalR
    });
});

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddApplication();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//add graphQL(hot chocolate)
/*builder.Services
    .AddGraphQLServer()
    .AddQueryType<Query>()
    .AddErrorFilter<GraphQlErrorFilter>();*/

var app = builder.Build();

//add exception middleware
app.UseMiddleware<ExceptionHandlingMiddleware>();
//app.MapGraphQL("/graphql");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseCors("CorsPolicy");

app.UseAuthorization();

app.MapControllers();

app.Run();
