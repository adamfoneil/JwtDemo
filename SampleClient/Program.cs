using Microsoft.Extensions.Options;

namespace SampleClient
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddHttpClient();

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