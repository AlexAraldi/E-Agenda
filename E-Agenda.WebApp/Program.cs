using E_Agenda.Dominio.ModuloCategorias;
using E_Agenda.Dominio.ModuloCompromissos;
using E_Agenda.Dominio.ModuloContatos;
using E_Agenda.Dominio.ModuloDespesas;
using E_Agenda.Dominio.ModuloTarefa;
using E_Agenda.Infraestrutura.Compartilhado;
using E_Agenda.Infraestrutura.ModuloCategorias;
using E_Agenda.Infraestrutura.ModuloCompromissos;
using E_Agenda.Infraestrutura.ModuloContatos;
using E_Agenda.Infraestrutura.ModuloDespesas;
using E_Agenda.Infraestrutura.ModuloTarefa;

namespace E_Agenda.WebApp
{
    public class Program
    {
        private static ContextoDados CriarContextoDeDados(IServiceProvider serviceProvider) 
        {
            return new ContextoDados(true);

        }
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            
            builder.Services.AddControllersWithViews();
            builder.Services.AddScoped<ContextoDados>((_)=> new ContextoDados(true));
            builder.Services.AddScoped<IRepositorioTarefa,RepositorioTarefa>();
            builder.Services.AddScoped<IRepositorioCategoria, RepositorioCategoria>();
            builder.Services.AddScoped<IRepositorioCompromisso, RepositorioCompromisso>();
            builder.Services.AddScoped<IRepositorioContato, RepositorioContato>();
            builder.Services.AddScoped<IRepositorioDespesa, RepositorioDespesa>();


            var app = builder.Build();

            app.UseStaticFiles();
            app.UseRouting();
            app.MapDefaultControllerRoute();

            app.Run();
        }
    }
}
