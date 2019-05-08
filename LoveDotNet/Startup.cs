using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using System.Net.Http;
using System;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.FileProviders;
using System.IO;
using Blazor.FileReader;
using Microsoft.AspNetCore.Components.Server;
using LoveDotNet.Helpers;

namespace LoveDotNet
{
    public class Startup
    {
        public Startup(IWebHostEnvironment env)
        {
            AppConst.WebRootPath = env.WebRootPath;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().AddNewtonsoftJson(
                options => options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
                );
            services.AddRazorPages();
            services.AddServerSideBlazor();
            // Setup user state
            services.AddScoped<UserState>();
            services.AddScoped<BlogState>();
            services.AddScoped<IFileReaderService, FileReaderService>();

            // Setup DBcontext
            var connection = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\jypjy\source\repos\LoveDotNet.Blazor\Data\lovedotnet.mdf;Integrated Security=True;Connect Timeout=30";
            services.AddDbContext<MyDBContext>(options => options.UseMySql(connection));

            // Setup HttpClient for server side in a client side compatible fashion
            services.AddScoped(s =>
            {
                // Creating the URI helper needs to wait until the JS Runtime is initialized, so defer it.
                var uriHelper = s.GetRequiredService<IUriHelper>();
                return new HttpClient
                {
                    BaseAddress = new Uri(uriHelper.GetBaseUri())
                };
            });

            services.AddSingleton<IFileProvider>(
            new PhysicalFileProvider(
                Path.Combine(Directory.GetCurrentDirectory(), "wwwroot")));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            app.UseStaticFiles();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
                endpoints.MapControllers();
                endpoints.MapHub<ComponentHub>(
                    ComponentHub.DefaultPath,
                    o =>
                    {
                        o.ApplicationMaxBufferSize = 102400000; // larger size
                        o.TransportMaxBufferSize = 102400000; // larger size
                    });
                endpoints.MapFallbackToPage("/_Host");
            });
        }
    }
}
