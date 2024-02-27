using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ApplicationTracker.Common.Contexts;
using ApplicationTracker.Common.Services;
using ApplicationTracker.Model;
using ApplicationTracker.Web.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ApplicationTracker.Common.Contexts.ApplicationDbContext>(options => {
    options.UseInMemoryDatabase("ApplicationTracker");
    // options.UseSqlServer(builder.Configuration.GetConnectionString("ApplicationDbContext") ?? throw new InvalidOperationException("Connection string 'ApplicationDbContext' not found.")));
});


var controllers = builder.Services.AddControllersWithViews();
if (builder.Environment.IsDevelopment())
    controllers.AddRazorRuntimeCompilation();


builder.Services.AddScoped<ApiService<Contact>>();
builder.Services.AddScoped<ApiService<Company>>();
builder.Services.AddScoped<ApiService<Application>>();



var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment()) {

    app.UseExceptionHandler("/Home/Error");
    
    app.UseHsts();

} else {

     // seed database
    using (var scope = app.Services.CreateScope()) {
        try {
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            context.Database.EnsureCreated();
        } catch (Exception) {
            throw;
        }
    }

}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "Area",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
