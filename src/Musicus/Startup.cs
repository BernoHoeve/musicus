﻿using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Musicus.Helpers;
using Microsoft.Extensions.Configuration;

namespace Musicus
{
	public class Startup
	{
		public IConfiguration Configuration { get; }

		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		// This method gets called by the runtime. Use this method to add services to the container.
		// For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
		public IServiceProvider ConfigureServices(IServiceCollection services)
		{
			services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
			services.AddSingleton<SignalRHelper>();
			
			services.AddMvc();
			services.AddSignalR();			

			return services.BuildServiceProvider();
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IHostingEnvironment env, SignalRHelper signalRHelper)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}

			app.UseStaticFiles();
			app.UseCors("AllowSpecificOrigin");

			app.UseSignalR(routes =>
			{
				routes.MapHub<MusicusHub>("/musicushub");
			});

			app.UseMvcWithDefaultRoute();

			if (signalRHelper != null)
			{
				signalRHelper.StartStatusUpdate();
			}
		}
	}
}
