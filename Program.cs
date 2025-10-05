using ContosoUniversity.Data;
using ContosoUniversity.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();
builder.Services.AddControllers();

if (builder.Environment.EnvironmentName == "Testing")
{
    builder.Services.AddDbContext<SchoolContext>(options =>
        options.UseInMemoryDatabase("TestDb"));
}
else
{
    builder.Services.AddDbContext<SchoolContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
}

builder.Services.AddScoped<ICourseService, DbExportService>();

var app = builder.Build();

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
app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<SchoolContext>();
    try
    {
        context.Database.Migrate();
        DbInitializer.Initialize(context);
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred creating or seeding the database.");
    }
}

app.Run();

public partial class Program { }
