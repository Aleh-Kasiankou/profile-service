using Idt.Profiles.Api.Extensions.Configuration;
using Idt.Profiles.Api.Extensions.Startup;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.ConfigureRepositories(builder.Configuration);
builder.Services.ConfigureServices(builder.Configuration);
builder.Services.ConfigureMessageBroker();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.MapControllers();

app.ConfigureRecurringJobs();
app.Run();


// TODO ADD LOGGING
// TODO DOCUMENT SWAGGER ENDPOINTS
// TODO URLS TO LOWER CASE
// TODO IMPROVE SAVED FILES ORGANISATION