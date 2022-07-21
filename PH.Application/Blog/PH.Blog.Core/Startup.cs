using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using PH.Core;
using PH.Core.Application.Attributes;
using PH.Authentication.JwtBearer;
using PH.Blog.Web.Core;
using Serilog;

namespace PH.Blog.Core
{
    [Startup(1)]
    public class Startup : StartupModule
    {
        public override void ConfigureServices(IServiceCollection services)
        {
            //services.AddFrameworkSerilog();
            services.AddFrameworkService();
            services.AddJwtBearerService<JwtConfig>();
            services.AddAppAuthorization();
        }

        public override void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseRouting();
            app.UseCors();
            app.UseSpecificationException();
            app.UseFrameworkSwagger();

            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(configure =>
            {
                configure.MapControllers();
            });
        }
    }
}