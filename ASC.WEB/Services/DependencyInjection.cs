using ASC.DataAccess.Interface;
using ASC.WEB.Configuration;
using ASC.WEB.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ASC.WEB.Services
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddCongfig(this IServiceCollection services, IConfiguration config)
        {
            var connectionString = config.GetConnectionString("DefaultConnection") ??
                throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
            services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connectionString));
            services.AddOptions();
            services.Configure<ApplicationSettings>(config.GetSection("AppSettings"));
            return services;
        }

        public static IServiceCollection AddMyDependencyGroup(this IServiceCollection services)
        {
            // Add ApplicationDbContext
            services.AddScoped<DbContext, ApplicationDbContext>();

            // Add IdentityUser
            services.AddIdentity<IdentityUser, IdentityRole>((options) =>
            {
                options.User.RequireUniqueEmail = true;
            }).AddEntityFrameworkStores<ApplicationDbContext>().AddDefaultTokenProviders();

            // Add services
            services.AddTransient<IEmailSender, AuthMessgageSender>();
            services.AddTransient<ISmsSender, AuthMessgageSender>();
            services.AddSingleton<IIdentitySeed, IdentitySeed>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            // ....

            services.AddSession();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddDistributedMemoryCache();
            services.AddSingleton<INavigationCacheOperations, NavigationCacheOperations>();

            // Add RazorPages, MVC
            services.AddRazorPages();
            services.AddDatabaseDeveloperPageExceptionFilter();
            services.AddControllersWithViews();

            return services;
        }

    }
}
