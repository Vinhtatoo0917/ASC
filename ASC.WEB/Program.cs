using ASC.DataAccess.Interface;
using ASC.WEB.Configuration;
using ASC.WEB.Data;
using ASC.WEB.Models;
using ASC.WEB.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.General;

var builder = WebApplication.CreateBuilder(args);

builder.Services
        .AddCongfig(builder.Configuration)
        .AddMyDependencyGroup();
builder.Services.AddRazorPages();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = IdentityConstants.ApplicationScheme;
    options.DefaultChallengeScheme = IdentityConstants.ApplicationScheme;
    options.DefaultSignInScheme = IdentityConstants.ExternalScheme;
});



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.MapRazorPages();
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseSession();

app.UseRouting();

app.UseAuthentication(); // Ensure authentication middleware is added
app.UseAuthorization();

app.MapControllerRoute(
    name: "areaRoute",
    pattern: "{area:exists}/{controller=Home}/{action=Index}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

// Run migrations automatically if needed
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var storageSeed = services.GetRequiredService<IIdentitySeed>();
    await storageSeed.Seed(
        services.GetRequiredService<UserManager<IdentityUser>>(),
        services.GetRequiredService<RoleManager<IdentityRole>>(),
        services.GetRequiredService<IOptions<ApplicationSettings>>()
    );
}

using (var scope = app.Services.CreateScope())
{
    var navigationCacheOperations = scope.ServiceProvider.GetRequiredService<INavigationCacheOperations>();
    await navigationCacheOperations.CreateNavigationCacheAsync();
}

app.Run();

