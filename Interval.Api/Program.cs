
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.FileProviders;
using System.Reflection;

#if DEBUG
string APP_PATH = Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, System.AppDomain.CurrentDomain.RelativeSearchPath ?? "");
string WEB_PATH = Path.Combine(APP_PATH, "Web_Files");
#else
string WEB_PATH = "Web_Files";
#endif
var builder = WebApplication.CreateBuilder(new WebApplicationOptions()
{
    WebRootPath = WEB_PATH
});

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthorization();
app.MapControllers();

app.UseEndpoints(config =>
{
    config.MapControllers();
});
app.Map("/app", MapCustomMiddleware);

app.Run();

void MapCustomMiddleware(IApplicationBuilder app)
{
    app.UseDefaultFiles().
        UseStaticFiles().
        Run(async context => await context.Response.WriteAsync("ok"));
}

