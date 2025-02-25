using ApiSults.Application.Setup;
using ApiSults.Infrastructure.Data.Setup;
using ApiSults.Infrastructure.Jobs.Setup;
using ApiSults.Infrastructure.Services.Logging.Setup;
using ApiSults.Infrastructure.Services.RefreshData.Setup;
using ApiSults.Presentation.Api.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.ConfigureDataApp(builder.Configuration);
builder.Services.ConfigureApplicationApp(builder.Configuration);
builder.Services.ConfigureLoggingApp();
builder.Services.ConfigureRefreshDataApp();
builder.Services.ConfigureJobApp();
builder.Services.AddHttpClient();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();
app.CreateDataBase();
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

await app.RunAsync();