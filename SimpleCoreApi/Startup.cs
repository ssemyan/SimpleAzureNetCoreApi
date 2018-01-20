using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace SimpleCoreApi
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
			services.AddCors(
				options => options.AddPolicy("AllowCors",
					builder =>
					{
						builder
								//.WithOrigins("http://localhost:4456") //AllowSpecificOrigins;  
								//.WithOrigins("http://localhost:4456", "http://localhost:4457") //AllowMultipleOrigins;  
							.AllowAnyOrigin() //AllowAllOrigins;  
								//.WithMethods("GET") //AllowSpecificMethods;  
								//.WithMethods("GET", "PUT") //AllowSpecificMethods;  
								//.WithMethods("GET", "PUT", "POST") //AllowSpecificMethods;  
								//.WithMethods("GET", "PUT", "POST", "DELETE") //AllowSpecificMethods;  
							.AllowAnyMethod() //AllowAllMethods;  
								//.WithHeaders("Accept", "Content-type", "Origin", "X-Custom-Header"); //AllowSpecificHeaders;  
							.AllowAnyHeader(); //AllowAllHeaders;  
					})
			);

			services.AddMvc();

			// Load the app settings
			services.Configure<AppSettings>(Configuration.GetSection("AppSettings"));
		}

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

			//Enable CORS policy "AllowCors"  
			app.UseCors("AllowCors");

			app.UseMvc();
        }
    }

	// Class used to hold the app settings
	public class AppSettings
	{
		public string ServiceBusKeyVaultId { get; set; }
		public string DatabaseKeyVaultId { get; set; }
		public string QueueName { get; set; }
		public string LocalDbConnectionString { get; set; }
		public string LocalServiceBusConnectionString { get; set; }
	}
}
