using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using API2.Data;

var builder = WebApplication.CreateBuilder(args);

// Szolg�ltat�sok regisztr�l�sa
builder.Services.AddControllersWithViews();
// IWeatherDataRepository regisztr�l�sa a WeatherDataRepository implement�ci�val
builder.Services.AddSingleton<IWeatherRepository, WeatherRepository>();

// Opcion�lis: Swagger t�mogat�s fejleszt�shez
//builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
app.UseRouting();

app.MapControllerRoute(
  name: "default",
  pattern: "{controller}/{action=Index}/{id?}");

app.MapGet("/", () => "Hello World!");

// Fejleszt?i k�rnyezet eset�n enged�lyezz�k a fejleszt?i hibakezel�st �s a Swagger UI-t
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
