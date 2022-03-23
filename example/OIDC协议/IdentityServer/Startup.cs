using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Linq;
using IdentityServer.Template;

namespace IdentityServer
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
            services.AddMvc();

            services
              .AddIdentityServer(y =>
              {
                  y.Events.RaiseErrorEvents = true;
                  y.Events.RaiseInformationEvents = true;
                  y.Events.RaiseFailureEvents = true;
                  y.Events.RaiseSuccessEvents = true;

                  //see https://identityserver4.readthedocs.io/en/latest/topics/resources.html
                  y.EmitStaticAudienceClaim = true;
              })
              .AddInMemoryPersistedGrants()
              .AddInMemoryApiResources(Identity_config.ApiResources)

              .AddDeveloperSigningCredential()//开发环境 
              .AddInMemoryApiScopes(Identity_config.GetApiScopes())
              .AddInMemoryIdentityResources(Identity_config.IdentityResources())
              .AddTestUsers(Identity_config.GetTestUsers().ToList())
              .AddInMemoryClients(Identity_config.GetClients())
              .AddJwtBearerClientAuthentication();

            services.AddControllers();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseIdentityServer();//添加认证服务器

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }

}
