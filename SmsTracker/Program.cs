using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SmsTracker.Constants;
using SmsTracker.Data;
using SmsTracker.Options;
using SmsTracker.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddRazorPages(options =>
{
    options.Conventions.AuthorizeFolder(NavigationConstants.Tracker);
});

builder.Services.AddSingleton<TwilioService>();
builder.Services.AddOptions<TwilioOptions>().BindConfiguration(TwilioOptions.Twilio);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

//app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();

/* Disable this to re-enable user registration */
app.UseEndpoints(ep =>
{
    ep.MapGet("/Identity/Account/Register",
        ctx => Task.Factory.StartNew(() => ctx.Response.Redirect("/Identity/Account/Login", false, true)));
    ep.MapPost("/Identity/Account/Register",
        ctx => Task.Factory.StartNew(() => ctx.Response.Redirect("/Identity/Account/Login", false, true)));

});

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Sms}/{action=Index}/{id?}");

app.Run();