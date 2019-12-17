using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using T_API.BLL.Abstract;
using T_API.BLL.Concrete;
using T_API.Core.MappingProfiles;
using T_API.Core.Settings;

namespace T_API.UI
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

            var mappingConfig = new MapperConfiguration(mc => { mc.AddProfile(new MappingProfile()); });

            IMapper mapper = mappingConfig.CreateMapper();
            services.AddSingleton(mapper);

            services.AddControllersWithViews().AddRazorRuntimeCompilation();
            var key = Encoding.ASCII.GetBytes(ConfigurationSettings.SecretKey);
            services.AddAuthentication()
                .AddCookie(options =>
                    {
                        options.LoginPath = "/Security/Login";
                        options.LogoutPath = "/Security/Logout";
                        options.AccessDeniedPath = "/Store/Home";
                        options.SlidingExpiration = true;
                        options.Cookie = new CookieBuilder()
                        {
                            HttpOnly = true,
                            Name = ".T-API.Security.Cookie",
                            Path = "/",
                            SameSite = SameSiteMode.Lax,
                            SecurePolicy = CookieSecurePolicy.SameAsRequest
                        };
                    })
                .AddJwtBearer(x =>
                {
                    x.RequireHttpsMetadata = false;
                    x.SaveToken = true;
                    x.RefreshOnIssuerKeyNotFound = true;
                    x.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(key),
                        ValidateIssuer = false,
                        ValidateAudience = false,

                    };
                });
            services.AddAuthorization(options =>
            {
                var defaultPoliceBuilder=new AuthorizationPolicyBuilder(CookieAuthenticationDefaults.AuthenticationScheme,JwtBearerDefaults.AuthenticationScheme);
                defaultPoliceBuilder=defaultPoliceBuilder.RequireAuthenticatedUser();
                options.DefaultPolicy = defaultPoliceBuilder.Build();


            });
            
            services.AddSession(opt =>
            {
                opt.Cookie.Name = ".T_API.Session";
                opt.Cookie.IsEssential = true;
            });
            services.AddDistributedMemoryCache();


            services.AddScoped<IAuthService, AuthManager>();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseSession();

            app.UseAuthentication();
            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute("areas", "{area:exists}/{controller=Home}/{action=Index}/{id?}");
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
