using TeamSyncB.services;
using TeamSyncB.data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSingleton<ApplicationDbContext>();

builder.Services.AddScoped<IUserService, UserService>();

var app = builder.Build();

app.UseHttpsRedirection();

app.MapControllers();
app.MapGet("", () => "running");

app.Run();
