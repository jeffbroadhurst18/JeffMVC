using System;
using System.Net.Http.Headers;
using System.Text;
using AutoMapper;
using JeffShared;
using JeffShared.WeatherModels;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

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
			services.AddScoped<IWeatherRepository, WeatherRepository>();
			services.AddAutoMapper(); //Adds IMapper as injectable type
			services.AddMemoryCache();
			
			services.AddTransient<CreateUser>();

			services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
				.AddJwtBearer(options =>
				{
					options.TokenValidationParameters = new TokenValidationParameters
					{
						ValidateIssuer = true,
						ValidateAudience = true,
						ValidateLifetime = true,
						ValidateIssuerSigningKey = true,
						ValidIssuer = Configuration.GetSection("Tokens").GetSection("Issuer").Value,
						ValidAudience = Configuration.GetSection("Tokens").GetSection("Audience").Value,
						IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration.GetSection("Tokens").GetSection("Key").Value))
					};
				});

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
			
			services.AddDbContext<WeatherDBContext>(options =>
					options.UseSqlServer(Configuration.GetConnectionString("WeatherDBContext"), b => b.MigrationsAssembly("JeffShared")));

			services.Configure<IdentityOptions>(options =>
			{
				// Password settings
				options.Password.RequireDigit = true;
				options.Password.RequiredLength = 6;
				options.Password.RequireNonAlphanumeric = false;
				options.Password.RequireUppercase = true;
				options.Password.RequireLowercase = false;
			});


			services.AddResponseCaching();
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

			//app.UseResponseCaching();

			//app.Use(async (context, next) =>
			//{
			//	context.Response.GetTypedHeaders().CacheControl =
			//		new Microsoft.Net.Http.Headers.CacheControlHeaderValue()
			//		{
			//			Public = true,
			//			MaxAge = TimeSpan.FromSeconds(1)
			//		};

			//	await next();
			//});

			app.UseHttpsRedirection();
			app.UseAuthentication();
			app.UseMvc();
			

			

			//using (var scope = app.ApplicationServices.CreateScope())
			//{
			//	var seeder = scope.ServiceProvider.GetService<CreateUser>();
			//	seeder.CreateFirstUser().Wait(); ; //seeder is async .Wait waits for it to finish without making 
			//							// the whole method async.
			//}
		}

		
		
	}
}
