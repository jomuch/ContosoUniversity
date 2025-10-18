using Microsoft.EntityFrameworkCore;
using ContosoUniversity.Data;
using Microsoft.Extensions.Logging;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();
builder.Services.AddDbContext<SchoolContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("SchoolContext") ?? throw new InvalidOperationException("Connection string 'SchoolContext' not found.")));

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<SchoolContext>();
        DbInitializer.Initialize(context);
        DbExporter.ExportToXml(context, "ExportedData.xml");
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred during DB creation, seeding, or exporting.");
    }
}

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}
else
{
    app.UseDeveloperExceptionPage();
    app.UseMigrationsEndPoint();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.Run();