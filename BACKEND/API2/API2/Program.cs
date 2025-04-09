using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using API2.Data;

var builder = WebApplication.CreateBuilder(args);

// Szolgáltatások regisztrálása
builder.Services.AddControllersWithViews();
// IWeatherDataRepository regisztrálása a WeatherDataRepository implementációval
builder.Services.AddSingleton<IWeatherRepository, WeatherRepository>();

// Opcionális: Swagger támogatás fejlesztéshez
//builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
app.UseRouting();

app.MapControllerRoute(
  name: "default",
  pattern: "{controller}/{action=Index}/{id?}");

app.MapGet("/", () => "Hello World!");

// Fejleszt?i környezet esetén engedélyezzük a fejleszt?i hibakezelést és a Swagger UI-t
if (app.Environment.IsDevelopment())
{
    //app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
}



//app.UseHttpsRedirection();
//app.UseAuthorization();

//app.MapControllers();

app.UseCors(x => x
     .AllowCredentials()
     .AllowAnyMethod()
     .AllowAnyHeader()
     .WithOrigins("http://127.0.0.1:5500"));

app.Run();
