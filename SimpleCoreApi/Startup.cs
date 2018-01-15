using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

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
							.WithMethods("GET", "PUT", "POST", "DELETE") //AllowSpecificMethods;  
																		 //.AllowAnyMethod() //AllowAllMethods;  
																		 //.WithHeaders("Accept", "Content-type", "Origin", "X-Custom-Header"); //AllowSpecificHeaders;  
							.AllowAnyHeader(); //AllowAllHeaders;  
					})
			);
			services.AddMvc();
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
}
