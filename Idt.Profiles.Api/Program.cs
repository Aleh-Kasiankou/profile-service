using Idt.Profiles.Api.Extensions.Configuration;
using Idt.Profiles.Api.Extensions.Startup;
using Idt.Profiles.Api.Middleware.ExceptionHandling;

var builder = WebApplication.CreateBuilder(args);

builder.ConfigureSerilogLogging();
builder.Services.AddRouting(options => options.LowercaseUrls = true);
builder.Services.AddControllers();
builder.Services.ConfigureSwagger();
builder.Services.AddEndpointsApiExplorer();
builder.Services.ConfigureRepositories(builder.Configuration);
builder.Services.ConfigureServices(builder.Configuration);
builder.Services.ConfigureMessageBroker();
builder.Services.ConfigureRequestPipelineServices();
builder.Services.ConfigureValidators();

var app = builder.Build();

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.MapControllers();

app.ConfigureRecurringJobs();
await app.SetUpDbIndices();
app.Run();