using ContosoUniversity.Data;
using ContosoUniversity.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddRazorPages();

// Register DbExportService for DI
builder.Services.AddScoped<IDataExportService, DbExportService>();

// Use SQL Server unless we're in a testing environment
if (!builder.Environment.EnvironmentName.Equals("Testing", StringComparison.OrdinalIgnoreCase))
{
    builder.Services.AddDbContext<SchoolContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
}
else
{
    // In-memory DB for integration tests
    builder.Services.AddDbContext<SchoolContext>(options =>
        options.UseInMemoryDatabase("InMemoryDbForTesting"));
}

var app = builder.Build();

// Seed the database
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<SchoolContext>();

    try
    {
        if (!app.Environment.IsEnvironment("Testing"))
        {
            context.Database.Migrate();          // SQL Server migrations
            DbInitializer.Initialize(context);   // Seed SQL Server DB
        }
        else
        {
            DbInitializer.SeedInMemory(context); // Seed InMemory DB
        }
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred creating or seeding the database.");
    }
}

// Configure HTTP request pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.MapRazorPages();

app.Run();

// Make Program class public and partial for integration tests
public partial class Program { }
