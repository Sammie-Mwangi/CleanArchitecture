﻿using App.Application.EntitiesCommandsQueries.Categories.Commands.CreateCategory;
using App.Application.EntitiesCommandsQueries.Product.Queries.GetProduct;
using App.Application.Infrastructure;
using App.Application.Infrastructure.AutoMapper;
using App.Application.Interfaces;
using App.Common.Interfaces;
using App.Infrastructure;
using App.Persistence;
using AutoMapper;
using FluentValidation.AspNetCore;
using MediatR;
using MediatR.Pipeline;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NSwag.AspNetCore;
using System.Reflection;

namespace Web.API
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
            //Add AutoMapper
            services.AddAutoMapper(new Assembly[] { typeof(AutoMapperProfile).GetTypeInfo().Assembly });

            // Add framework services.
            services.AddTransient<INotificationService, NotificationService>();
            services.AddTransient<IDateTime, MachineDateTime>();

            //Add Mediator
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(RequestPreProcessorBehavior<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(RequestPerformanceBehaviour<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(RequestValidationBehaviour<,>));
            services.AddMediatR(typeof(GetProductQueryHandler).GetTypeInfo().Assembly);

            
            // Add DbContext using MYSQL Server Provider
            services.AddDbContext<AppDbContext>(options =>
               options.UseMySql(Configuration.GetConnectionString("AppDatabaseMYSQL")));



            //Add Fluent Validation
            services.AddMvc()
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2)
                .AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<CreateCategoryCommandValidator>());


            //Add AddFluentEmail
            services.AddFluentEmail("noreplyapp@app.app")
                .AddRazorRenderer()
                .AddSmtpSender("localhost", 25);



            //Add swagger Ui services
            services.AddSwaggerDocument();


        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
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

            // Add OpenAPI/Swagger middlewares
            app.UseSwagger(); 
            app.UseSwaggerUi3();

            app.UseMvc();
        }
    }
}
