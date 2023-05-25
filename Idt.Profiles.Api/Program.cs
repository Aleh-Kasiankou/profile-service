using Idt.Profiles.Api.Extensions.Configuration;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers(options =>
    options.SuppressAsyncSuffixInActionNames = false);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.ConfigureRepositories(builder.Configuration);
builder.Services.ConfigureServices(builder.Configuration);

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

// TODO ADD LOGGING
// TODO DOCUMENT SWAGGER ENDPOINTS
// TODO URLS TO LOWER CASE