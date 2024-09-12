using Microsoft.EntityFrameworkCore;
using TaskManagerData.Contexts;
using TaskManagerProject.Services.Interfaces;
using TaskManagerProject.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using TaskManagerProject.DTOs;
using Microsoft.AspNetCore.Http.Features;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddControllersWithViews();

        // Register the DbContext
        builder.Services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

        builder.Services.AddScoped<IUserProfileService, UserProfileService>();
        builder.Services.AddScoped<IProjectService, ProjectService>();
        builder.Services.AddScoped<IAccountService, AccountService>();
        builder.Services.AddScoped<ITaskService, TaskService>();

        builder.Services.AddHttpClient<IAccountService, AccountService>();

        builder.Services.Configure<ApiUrls>(builder.Configuration.GetSection("ApiUrls"));


        builder.Services.Configure<FormOptions>(options =>
        {
            options.MultipartBodyLengthLimit = 10 * 1024 * 1024; 
        });

        builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddCookie(options =>
            {
                options.LoginPath = "/Account/Login";
                options.LogoutPath = "/Account/Logout";
            });

        var app = builder.Build();

        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/UserProfile/Error");
            app.UseHsts();
        }

        app.UseHttpsRedirection();
        app.UseStaticFiles();

        app.UseRouting();

        app.UseAuthentication();
        app.UseAuthorization();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllerRoute(
                name: "default",
                pattern: "{controller=Account}/{action=Login}/{id?}");
        });

        app.Run();
    }
}