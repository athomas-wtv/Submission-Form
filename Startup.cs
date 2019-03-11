using IST_Submission_Form.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace IST_Submission_Form
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
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(o => 
                {
                    o.LoginPath = new PathString("/Index");
                    // o.AccessDeniedPath = new PathString("/Login");
                });
                
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            // Adding Databases to use in App
            services.AddDbContext<ISTProjectsContext>(options => { options.UseSqlServer(Configuration["ConnectionStrings:ISTProjectsContext"]); });
            services.AddDbContext<StaffDirectoryContext>(options => { options.UseSqlServer(Configuration["ConnectionStrings:StaffDirectoryContext"]); });

            services.AddSingleton<ILdapService, LdapService>();
            services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            services
                .AddFluentEmail("ist@fayette.kyschools.us")
                .AddRazorRenderer()
                .AddSmtpSender("ketsmail.us", 25);
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
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();
            app.UseAuthentication();
            app.UseStatusCodePages();
            app.UseStatusCodePagesWithReExecute("/Errors/{0}", "?code={0} - Not Found");

            app.UseMvc();
        }
    }
}
