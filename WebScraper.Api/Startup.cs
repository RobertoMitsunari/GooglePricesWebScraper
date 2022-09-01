using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using WebScraper.Api.Application;
using WebScraper.Api.Application.Facade;
using WebScraper.Api.Domain.Contracts;
using WebScraper.Api.Infra.Data;

namespace WebScraper.Api
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
            services.AddControllers();

            services.AddDbContext<ProdutoContext>(options => options.UseSqlServer(Configuration.GetConnectionString("ProdutosConnection")), ServiceLifetime.Transient);

            services.AddScoped<IProdutoRepo, SqlProdutosRepo>();
            services.AddScoped<IPesquisaRepo, SqlPesquisasRepo>();

            services.AddSingleton<IColetorRunner, ColetorRunner>();
            services.AddSingleton<IPesquisaFacade, PesquisaFacade>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

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
