using Microsoft.EntityFrameworkCore;
using AuthSystemDotNet.Data;
using AuthSystemDotNet.Models;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<AppDbContext>(
    options =>  options.UseSqlServer(connectionString: builder.Configuration.GetConnectionString(name: "DefaultConnection")));

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: "AllowBlazorClient",
        policy => policy.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader());
});


builder.Services.AddAuthorization();
builder.Services.AddAuthentication();

builder.Services.AddIdentityApiEndpoints<User>()
    .AddEntityFrameworkStores<AppDbContext>();
// dotnet user-secrets

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

WebApplication app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.UseCors(policyName: "AllowBlazorClient");

app.UseAuthorization();

app.UseSwagger();
app.UseSwaggerUI();
app.MapSwagger();

app.MapGet(pattern: "/", () => "Hello World!");
app.MapIdentityApi<User>();
app.MapControllers();

app.Run();