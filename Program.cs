using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Top5.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.


builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("CanEditMenu",
        policy => policy.RequireRole("Admin", "Staff"));
    options.FallbackPolicy = new AuthorizationPolicyBuilder()
    .RequireAuthenticatedUser()
    .Build();
});


builder.Services.AddControllersWithViews();



builder.Services.AddDbContext<AppDbContext>(
    options => options.UseSqlServer(
        builder.Configuration
          .GetConnectionString("Top5Db")));



builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = false)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<AppDbContext>();
string? jwt_key = builder.Configuration["Jwt:Key"];
if (string.IsNullOrEmpty(jwt_key))
{
    throw new Exception("Missing Jwt:Key in appsettings.json");
}

builder.Services.AddAuthentication()
    .AddJwtBearer(options =>
    {
        TokenValidationParameters rules = new TokenValidationParameters();
        rules.ValidateIssuer = true;
        rules.ValidIssuer = builder.Configuration["Jwt:Issuer"];
        rules.ValidateAudience = true;
        rules.ValidAudience = builder.Configuration["Jwt:Audience"];
        rules.ValidateLifetime = true;
        rules.ValidateIssuerSigningKey = true;
        rules.IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(jwt_key));
        options.TokenValidationParameters = rules;
    });

var app = builder.Build();



using (IServiceScope scope = app.Services.CreateScope())
{
    AppDbContext db_context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    await db_context.Database.MigrateAsync();

    await RoleSeed.Run(
        scope.ServiceProvider,
        app.Configuration["AdminEmail"],
        app.Configuration["StaffEmail"]);
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}


app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

// Our own route. It goes BEFORE the default route: the first match wins.
app.MapControllerRoute(
    name: "menu",
    pattern: "menu/{day:alpha=today}",
    defaults: new { controller = "Menu", action = "Show" });

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapRazorPages();

app.Run();
