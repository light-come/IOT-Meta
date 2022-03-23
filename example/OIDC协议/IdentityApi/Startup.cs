using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;

namespace IdentityApi
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

           
            services.AddAuthentication("token")

             // JWT tokens
             .AddJwtBearer("token", options =>
             {
                 options.SaveToken = true;
                 options.Authority = "http://localhost:7776";
                 options.RequireHttpsMetadata = false;
                 options.TokenValidationParameters = new TokenValidationParameters
                 {
                     //ValidateIssuerSigningKey = false,
                     //ValidateIssuer = false,
                     ValidateAudience = true //是否验证Audience
                 };
                 options.Audience = "invoice";
             })

             // reference tokens
             .AddOAuth2Introspection("introspection", options =>
             {
                 options.Authority = "http://localhost:7776";

                 options.ClientId = "ClientId";
                 options.ClientSecret = "ClientSecret";
             });

            //services.AddAuthentication("Bearer")
            //.AddJwtBearer("Bearer", options =>
            //{
            //    options.SaveToken = true;
            //    options.Authority = "http://localhost:7776";
            //    options.RequireHttpsMetadata = false;
            //    options.TokenValidationParameters = new TokenValidationParameters
            //    {
            //        ValidateIssuerSigningKey = false,
            //        ValidateIssuer = false,
            //        ValidateAudience = false //是否验证Audience
            //    };
            //    options.Audience = "invoice";
            //});


            services.AddControllers();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseAuthentication();//添加鉴权认证

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
