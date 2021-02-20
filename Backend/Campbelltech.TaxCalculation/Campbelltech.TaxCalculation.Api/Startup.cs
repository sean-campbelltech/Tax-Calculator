using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Campbelltech.TaxCalculation.Domain.Calculations;
using Campbelltech.TaxCalculation.Domain.Repositories;
using Campbelltech.TaxCalculation.Domain.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;

namespace Campbelltech.TaxCalculation.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        /// <summary>
        /// This method gets called by the runtime and used to add services to the container.
        /// </summary>
        /// <param name="services">IServiceCollection that secifies the contract for a collection of service descriptors.</param>
        public void ConfigureServices(IServiceCollection services)
        {
            // service registration for DI
            var taxCalculation = new TaxCalculationFactory().Create(services);
            services.AddTransient<ITaxCalculation>(x => taxCalculation);
            services.AddScoped<IPostalCodeTaxRepository, PostalCodeTaxRepository>();
            services.AddScoped<ITaxCalculationRepository, TaxCalculationRepository>();
            services.AddScoped<ITaxCalculationService, TaxCalculationService>();

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Campbelltech.TaxCalculation.Api", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Campbelltech.TaxCalculation.Api v1"));
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
