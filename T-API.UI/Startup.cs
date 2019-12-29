using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authentication;
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
using T_API.Core.DAL.Concrete;
using T_API.Core.Settings;
using T_API.DAL.Abstract;
using T_API.DAL.Concrete;
using T_API.UI.MappingProfiles;

namespace T_API.UI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {

            var mappingConfig = new MapperConfiguration(mc => { mc.AddProfile(new MappingProfile()); });

            IMapper mapper = mappingConfig.CreateMapper();
            services.AddSingleton(mapper);

            services.AddScoped<IAuthService, AuthManager>();
            services.AddScoped<IDatabaseService, DatabaseManager>();
            services.AddScoped<IDatabaseRepository, DatabaseRepository>();
            services.AddScoped<IUserService, UserManager>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddTransient<IRealDbRepositoryFactory, RealDbRepositoryFactory>();
            services.AddTransient<IRealDbService, RealDbManager>();


            services.AddControllersWithViews().AddRazorRuntimeCompilation();




            var key = Encoding.ASCII.GetBytes(ConfigurationSettings.SecretKey);
            services.AddAuthentication()
                .AddCookie(options =>
                {
                    options.LoginPath = "/Security/Login";
                    options.LogoutPath = "/Security/Logout";
                    options.AccessDeniedPath = "/Security/Login";
                    options.SlidingExpiration = true;
                    options.Cookie = new CookieBuilder()
                    {
                        HttpOnly = true,
                        Name = ".T-API.Security.Cookie",
                        Path = "/",
                        SameSite = SameSiteMode.Lax,
                        SecurePolicy = CookieSecurePolicy.SameAsRequest
                    };
                }).AddJwtBearer(x =>
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
                var defaultPoliceBuilder = new AuthorizationPolicyBuilder(CookieAuthenticationDefaults.AuthenticationScheme, JwtBearerDefaults.AuthenticationScheme);
                defaultPoliceBuilder = defaultPoliceBuilder.RequireAuthenticatedUser();
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
            app.Use(async (context, next) =>
            {
                await next();
                var bearerAuth = context.Request.Headers["Authorization"]
                                     .FirstOrDefault()?.StartsWith("Bearer ") ?? false;
                if (context.Response.StatusCode == 401
                    && !context.User.Identity.IsAuthenticated
                    && !bearerAuth)
                {
                    await context.ChallengeAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                }
            });

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
