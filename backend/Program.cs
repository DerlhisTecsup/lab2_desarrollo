using System.Net.Http.Json;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;

// Modelo de datos para un Pokémon
public class Pokemon
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int Height { get; set; }
    public int Weight { get; set; }
    public string[] Types { get; set; }
}

public class Program
{
    public static void Main(string[] args)
    {
        var host = Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.ConfigureServices(services =>
                {
                    // Configurar CORS para permitir todas las solicitudes
                    services.AddCors(options =>
                    {
                        options.AddDefaultPolicy(builder =>
                        {
                            builder.AllowAnyOrigin()
                                   .AllowAnyMethod()
                                   .AllowAnyHeader();
                        });
                    });
                });

                webBuilder.Configure(app =>
                {
                    app.UseRouting();

                    // Habilitar CORS
                    app.UseCors();

                    app.UseEndpoints(endpoints =>
                    {
                        // Endpoint para obtener datos de un Pokémon específico desde la API pública de Pokémon
                        endpoints.MapGet("/pokemon/{id}", async context =>
                        {
                            var id = context.Request.RouteValues["id"];
                            using var client = new HttpClient();

                            // Consumir la API pública de Pokémon
                            var pokemon = await client.GetFromJsonAsync<Pokemon>($"https://pokeapi.co/api/v2/pokemon/{id}");

                            // Si el Pokémon no se encuentra, devolver un error 404
                            if (pokemon == null)
                            {
                                context.Response.StatusCode = 404;
                                await context.Response.WriteAsJsonAsync(new { message = "Pokémon not found" });
                            }
                            else
                            {
                                // Devolver los datos del Pokémon
                                await context.Response.WriteAsJsonAsync(pokemon);
                            }
                        });

                        // Endpoint de bienvenida
                        endpoints.MapGet("/", async context =>
                        {
                            await context.Response.WriteAsync("Bienvenido a la API de Pokémon");
                        });
                    });
                });
            })
            .Build();

        host.Run();
    }
}
