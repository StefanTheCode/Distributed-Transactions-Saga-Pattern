using HotelService.Application.Common;
using HotelService.Application.Common.Models;
using HotelService.Application.Services.BookingService.Command;
using HotelService.Application.Services.BookingService.Query;
using HotelService.Infrastructure.Persistence;
using MassTransit;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Saga.Core.Concrete.Brokers;
using Saga.Core.DependencyRegisters;
using System.Collections.Generic;
using System.Reflection;

namespace HotelService
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        //TO DO: Separate those in methods
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddDbContext<HotelDbContext>(options =>
                   options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));


            services.AddScoped(typeof(IHotelDbContext), typeof(HotelDbContext));

            services.AddSagaCore(Configuration);

            services.AddMediatR(Assembly.GetExecutingAssembly());

            services.AddScoped<IRequestHandler<Create, Result>, CreateHandler>();
            services.AddScoped<IRequestHandler<GetByDestination, HotelResponse>, GetByDestinationHandler>();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "HotelService", Version = "v1" });
            });

            services.AddCors(opt =>
            {
                opt.AddPolicy("CorsPolicy", policy =>
                {
                    policy.AllowAnyMethod().AllowAnyHeader().WithOrigins("https://localhost:7235");
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
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "HotelService v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors("CorsPolicy");

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}