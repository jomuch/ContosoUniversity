using ContosoUniversity.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using System;
using ContosoUniversity.Services;

public partial class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddRazorPages();
        builder.Services.AddControllers(); // <-- ADDED: Enables API Controllers
        
        // Database Registration
        builder.Services.AddDbContext<SchoolContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

        // Service Registration
        builder.Services.AddScoped<ICourseService, ContosoUniversity.Services.DbExportService>(); 

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
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
        app.UseRouting();

        app.UseAuthorization();

        app.MapRazorPages();
        app.MapControllers(); // <-- ADDED: Maps API Controller routes (fixes 404)

        // Data Seeding Logic
        using (var scope = app.Services.CreateScope())
        {
            var services = scope.ServiceProvider;
            var context = services.GetRequiredService<SchoolContext>();
            var logger = services.GetRequiredService<ILogger<Program>>();

            try
            {
                context.Database.Migrate(); 
                DbInitializer.Initialize(context); 
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred creating or seeding the database.");
            }
        }

        app.Run();
    }
}