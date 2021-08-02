using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FactoryScheduler.Authentication.Service.Entities;
using FactoryScheduler.Authentication.Service.Interfaces;
using FactoryScheduler.Authentication.Service.Models;
using FactoryScheduler.Authentication.Service.MongoDB;
using FactoryScheduler.Authentication.Service.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;

namespace FactoryScheduler.Authentication.Service
{
    //TODO: Eventually we want to split this out into multiple projects
    //WebApi
    //Database
    //Common
    //Authentication
    //Scheduler

    public class Startup
    {
        private const string AllowedClientOriginSetting = "AllowedOrigin";
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddControllers(options =>
            {
                options.SuppressAsyncSuffixInActionNames = false;
            });
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "FactoryScheduler.Authentication.Service", Version = "v1" });
            });

            ConfigureMongoDb(services);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "FactoryScheduler.Authentication.Service v1"));
                app.UseCors(builder =>
                {
                    builder.WithOrigins(Configuration.GetSection(AllowedClientOriginSetting).Value)
                           .AllowAnyMethod()
                           .AllowAnyHeader();

                });
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
        private void ConfigureMongoDb(IServiceCollection services)
        {
            var settings = GetMongoDbSettings();
            services.AddMongoDB(settings);
            services.AddMongoDbRepository<WorkBuildingRepository, WorkBuilding>(settings.WorkBuildingCollectionName);
            services.AddMongoDbRepository<WorkAreaRepository, WorkArea>(settings.WorkAreaCollectionName);
            // services.AddMongoDbRepository<WorkStationUsersRepository, WorkStation_Users>(settings.WorkStationUsersCollectionName);

        }
        private FactorySchedulerDatabaseSettings GetMongoDbSettings() =>
            Configuration.GetSection(nameof(FactorySchedulerDatabaseSettings)).Get<FactorySchedulerDatabaseSettings>();

    }
}
