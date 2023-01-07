using Microsoft.Extensions.Options;
using SampleClient.Models;

namespace SampleClient
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.Configure<ServerOptions>(builder.Configuration.GetSection(nameof(ServerOptions)));

            builder.Services.AddDistributedMemoryCache();
            builder.Services.AddSession(config =>
            {
                config.IdleTimeout = TimeSpan.FromMinutes(20);                
            });

            builder.Services.AddRazorPages();           

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");                
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();            
            app.UseRouting();
            app.UseAuthorization();
            app.UseSession();

            app.MapRazorPages();
            
            app.Run();
        }
    }
}