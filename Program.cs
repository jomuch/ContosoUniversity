using ContosoUniversity.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();
builder.Services.AddControllersWithViews();

if (!builder.Environment.EnvironmentName.Equals("Testing", StringComparison.OrdinalIgnoreCase))
{
    builder.Services.AddDbContext<SchoolContext>(options =>
        options.UseSqlServer(
            builder.Configuration.GetConnectionString("SchoolContext")
            ?? throw new InvalidOperationException("Connection string 'SchoolContext' not found.")
        )
    );
}

var app = builder.Build();

if (!builder.Environment.EnvironmentName.Equals("Testing", StringComparison.OrdinalIgnoreCase))
{
    using var scope = app.Services.CreateScope();
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<SchoolContext>();
        context.Database.Migrate();
        DbInitializer.Initialize(context);
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred seeding the database.");
    }
}

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}"
);
app.MapRazorPages();

app.Run();

public partial class Program { }
