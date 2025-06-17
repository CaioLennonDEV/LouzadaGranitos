using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using LouzadaGranitos.Core.Interfaces;
using LouzadaGranitos.Data;
using LouzadaGranitos.Data.Repositories;
using LouzadaGranitos.Services;
using LouzadaGranitos.ConsoleApp;

namespace LouzadaGranitos.ConsoleApp
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();
            
            // Configurar banco de dados
            var configuration = host.Services.GetRequiredService<IConfiguration>();
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            
            // Inicializar banco
            LouzadaGranitos.Data.DatabaseInitializer.Initialize(connectionString);
            LouzadaGranitos.Data.DatabaseInitializer.LimparERecriarAdmin(connectionString);
            LouzadaGranitos.Data.DatabaseInitializer.ListarUsuarios(connectionString);

            // Executar aplicação
            var app = host.Services.GetRequiredService<ConsoleApplication>();
            await app.RunAsync();
        }

        static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((context, services) =>
                {
                    var connectionString = context.Configuration.GetConnectionString("DefaultConnection");
                    
                    // Registrar repositórios
                    services.AddScoped<IUsuarioRepository>(provider => new UsuarioRepository(connectionString));
                    services.AddScoped<IPedreiraRepository>(provider => new PedreiraRepository(connectionString));
                    services.AddScoped<IBlocoRepository>(provider => new BlocoRepository(connectionString));
                    services.AddScoped<IChapaRepository>(provider => new ChapaRepository(connectionString));
                    services.AddScoped<ISerragemBlocoRepository>(provider => new SerragemBlocoRepository(connectionString));
                    
                    // Registrar serviços
                    services.AddScoped<AutenticacaoService>();
                    services.AddScoped<CadastroService>();
                    
                    // Registrar aplicação principal
                    services.AddScoped<ConsoleApplication>(provider => 
                        new ConsoleApplication(
                            provider.GetRequiredService<AutenticacaoService>(),
                            provider.GetRequiredService<CadastroService>(),
                            provider.GetRequiredService<IUsuarioRepository>(),
                            provider.GetRequiredService<IPedreiraRepository>(),
                            provider.GetRequiredService<IBlocoRepository>(),
                            provider.GetRequiredService<IChapaRepository>(),
                            provider.GetRequiredService<ISerragemBlocoRepository>()
                        ));
                });
    }
} 