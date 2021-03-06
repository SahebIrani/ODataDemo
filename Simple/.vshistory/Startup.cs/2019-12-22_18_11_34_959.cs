using Microsoft.AspNet.OData.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using Simple.SinjulMSBH;

namespace Simple
{
	public class Startup
	{
		public void ConfigureServices(IServiceCollection services)
		{
			services.AddDbContextPool<AppDbContext>(opt => opt.UseInMemoryDatabase(nameof(AppDbContext)));
			services.AddControllers(mvcOptions => mvcOptions.EnableEndpointRouting = false);
			services.AddOData();
		}

		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			if (env.IsDevelopment())
				app.UseDeveloperExceptionPage();

			app.UseHttpsRedirection();

			app.UseMvc(configureRoutes: routeBuilder =>
			{
				routeBuilder.Select().Expand().Filter().OrderBy().MaxTop(85).Count();

				routeBuilder.MapODataServiceRoute("odata", "odata", EdmModelBuilder.GetEdmModel());

				routeBuilder.EnableDependencyInjection();
			});
		}
	}
}
