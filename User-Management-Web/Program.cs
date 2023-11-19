using User_Management_Web;
using User_Management_Web.Services;

IConfigurationRoot configuration = new ConfigurationBuilder()
          .SetBasePath(Directory.GetCurrentDirectory())
          .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
          .Build();

var builder = WebApplication.CreateBuilder(new WebApplicationOptions
{
    EnvironmentName = configuration.GetSection("Hosting")["Environment"]
});

// Add services to the container.
builder.Services.AddControllersWithViews();

// Add other services
builder.Services.AddHttpClient();

builder.Services.AddExceptionHandler<CustomExceptionHandler>();
builder.Services.AddSingleton<UserService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
