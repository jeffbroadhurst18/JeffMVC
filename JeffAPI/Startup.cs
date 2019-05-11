using System.Net.Http.Headers;
using AutoMapper;
using JeffShared;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace JeffAPI
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
			services.AddHttpClient<ITidesApiClient, TidesApiClient>(client =>
			{
				client.DefaultRequestHeaders.Accept.Clear();
				client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", Configuration.GetSection("Ocp-Apim-Subscription-Key").GetSection("key").Value);
				client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
			});

			services.AddHttpClient<IWeatherApiClient, WeatherApiClient>(client =>
			{
				client.DefaultRequestHeaders.Accept.Clear();
				client.DefaultRequestHeaders.Add("X-RapidAPI-Host", Configuration.GetSection("X-RapidAPI-Host").GetSection("host").Value);
				client.DefaultRequestHeaders.Add("X-RapidAPI-Key", Configuration.GetSection("X-RapidAPI-Key").GetSection("key").Value);
				client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
			});

			services.AddHttpClient<ICountriesAPIClient, CountriesAPIClient>(client => { client.DefaultRequestHeaders.Accept.Clear(); });

			services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
			services.AddSingleton(Configuration);
			services.AddSingleton<ITidesService, TidesService>();
			services.AddSingleton<ICountryService, CountryService>();
			services.AddSingleton<IWeatherService, WeatherService>();
			services.AddAutoMapper(); //Adds IMapper as injectable type

			//============
			services.AddCors(cfg =>
			{

				cfg.AddPolicy("AnyGET", bldr =>
				{
					bldr.AllowAnyHeader().
					AllowAnyMethod().
					AllowAnyOrigin();
				});
			});

			//====
			services.AddDbContext<LocationContext>(options =>
					options.UseSqlServer(Configuration.GetConnectionString("LocationContext")));
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
				app.UseHsts();
			}

			app.UseHttpsRedirection();
			app.UseMvc();
		}
	}
}
