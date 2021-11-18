using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.IO;
using System.Threading.Tasks;
using static Taoist.Archives.project.Web;

namespace OpticalServer
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
            services.AddControllers();

            //ע��Swagger������������һ���Ͷ��Swagger �ĵ�
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v3", new OpenApiInfo { Title = "�м��������ӻ��ĵ�", Version = "v3" });
                //Determine base path for the application.  
                var basePath = Path.GetDirectoryName(typeof(Program).Assembly.Location);
                //Set the comments path for the swagger json and ui.  
                var xmlPath = Path.Combine(basePath, "OpticalServer.xml");
                c.IncludeXmlComments(xmlPath);
            });


            {
                #region ����
                // ��ӿ������
                services.AddCors(options => {
                    options.AddPolicy(AllowSpecificOrigin, builder => { builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader(); });
                });
                services.AddControllers();


                #endregion
                //���÷���Json
                services.AddControllersWithViews().AddNewtonsoftJson();
            }
            {
                // STEP1: �O�����ķN��ʽ��C HTTP Request �Ƿ�Ϸ�
                services
                    // �z�� HTTP Header �� Authorization �Ƿ��� JWT Bearer Token
                    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                    // �O�� JWT Bearer Token �ęz���x�
                    .AddJwtBearer(options =>
                    {
                        options.TokenValidationParameters = new TokenValidationParameters
                        {
                            ValidateIssuer = true,
                            ValidIssuer = Configuration["Jwt:Issuer"],
                            ValidateAudience = true,
                            ValidAudience = Configuration["Jwt:Issuer"],
                            ValidateLifetime = true,
                            ValidateIssuerSigningKey = true,
                            IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes("db3OIsj+BXE9NZDy0t8W3TcNekrF+2d/1sFnWG4HnV8TZY30iTOdtVWJG8abWvB1GlOgJuQZdcF2Luqm/hccMw=="))
                        };
                    });
            }

        }
        public class CorsMiddleware
        {
            private readonly RequestDelegate _next;
            public CorsMiddleware(RequestDelegate next)
            {
                _next = next;
            }

            public async Task Invoke(HttpContext context)
            {
                if (!context.Response.Headers.ContainsKey("Access-Control-Allow-Origin"))
                {
                    context.Response.Headers.Add("Access-Control-Allow-Origin", "*");
                }
                await _next(context);
            }
        }


        private readonly string AllowSpecificOrigin = "AllowSpecificOrigin";

        UseDirectoryBrowser useDirectoryBrowser = new UseDirectoryBrowser();
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {

            var c = useDirectoryBrowser.Configure(@"D:\����\ģ������");
#if DEBUG
            System.Diagnostics.Process.Start("explorer", c.url);
#endif


            app.UseMiddleware<CorsMiddleware>();

            //�����м����������Swagger��ΪJSON�ս��
            app.UseSwagger();
            //�����м�������swagger-ui��ָ��Swagger JSON�ս��
            app.UseSwaggerUI(c =>
            {//ISC_API/
                c.SwaggerEndpoint("/swagger/v3/swagger.json", "API");
            });

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            // STEP2: ʹ����C���޵� Middleware
            app.UseAuthentication();

            app.UseHttpsRedirection();
            //����Զ��
            app.UseRouting();
            //CORS �м����������Ϊ�ڶ� UseRouting �� UseEndpoints�ĵ���֮��ִ�С� ���ò���ȷ�������м��ֹͣ�������С�
            app.UseCors(AllowSpecificOrigin);
            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}
