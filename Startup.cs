using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using My_app_backend.Models;
using My_app_backend.Services;

namespace My_app_backend
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<ArticlestoreDatabaseSettings>(
                Configuration.GetSection(nameof(ArticlestoreDatabaseSettings)));

            services.AddSingleton<IArticlestoreDatabaseSettings>(sp =>
                sp.GetRequiredService<IOptions<ArticlestoreDatabaseSettings>>().Value);

            services.AddSingleton<ArticleService>();

            services.Configure<UserstoreDatabaseSettings>(
                Configuration.GetSection(nameof(UserstoreDatabaseSettings)));

            services.AddSingleton<IUserstoreDatabaseSettings>(sp =>
                sp.GetRequiredService<IOptions<UserstoreDatabaseSettings>>().Value);
            services.AddSingleton<UserService>();

            services.Configure<CategorystoreDatabaseSettings>(
                Configuration.GetSection(nameof(CategorystoreDatabaseSettings)));

            services.AddSingleton<ICategorystoreDatabaseSettings>(sp =>
                sp.GetRequiredService<IOptions<CategorystoreDatabaseSettings>>().Value);

            services.AddSingleton<CategoryService>();

            services.AddCors( options => 
            {
                options.AddPolicy("EnableCORS", builder => 
                {
                    builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod().Build();
                });
            });

            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseCors("EnableCORS");

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
