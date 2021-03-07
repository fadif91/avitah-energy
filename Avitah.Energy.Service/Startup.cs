using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Avitah.Energy.Service.Model;
using Avitah.Energy.Service.Repository;
using Avitah.Energy.Service.Services;
using MongoDB.Bson.Serialization;

namespace Avitah.Energy.Service
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddGrpc();
            BsonClassMap.RegisterClassMap<DataSource>(map => map.AutoMap());
            BsonClassMap.RegisterClassMap<Mapping>(map => map.AutoMap());
            BsonClassMap.RegisterClassMap<Data>(map => map.AutoMap());
            BsonClassMap.RegisterClassMap<Historical>(map => map.AutoMap());
            services.AddSingleton<IEnergyRepository, EnergyRepository>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGrpcService<EnergyService>();

                endpoints.MapGet("/", async context =>
                {
                    await context.Response.WriteAsync("Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");
                });
            });
        }
    }
}
