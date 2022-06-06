using ContractManagment_Al_Doori_.Models.Entities.Identity;
using ContractManagment_Al_Doori_.Models.Infrastructure.Database;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ContractManagment_Al_Doori_.Models.ApplicationServices.Abstraction;
using UTBEShop.Models.ApplicationServices.Implementation;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();


#region DbContext And Connection String

builder.Services.AddDbContext<ContractDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("SQLServerLocalConnection")));

#endregion


#region Configuration for Identity and Roles 
builder.Services.AddIdentity<User,Role>()
    .AddEntityFrameworkStores<ContractDbContext>()
    .AddDefaultTokenProviders();

//Modifciation of Idenetity password and requirements 
//The below code can affect the data seeding of the admin 
//Can Effect => Creating Users of all types 
builder.Services.Configure<IdentityOptions>(Options =>
{
    Options.Password.RequireDigit = false;
    Options.Password.RequiredLength = 2;
    Options.Password.RequireNonAlphanumeric = false;
    Options.Password.RequireUppercase = false;
    Options.Password.RequireLowercase = true;
    Options.Password.RequiredUniqueChars = 0;

    Options.Lockout.AllowedForNewUsers = true;
    Options.Lockout.MaxFailedAccessAttempts = 10;
    Options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(30);

    //unique Email Each User 
    Options.User.RequireUniqueEmail = true;


});
#endregion


#region Configure Application Cookies for login path
builder.Services.ConfigureApplicationCookie(options =>
{
    options.Cookie.HttpOnly = true;
    options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
    options.LoginPath = "/Security/Account/Login";
    options.LogoutPath = "/Security/Account/Logout";

    options.SlidingExpiration = true;
});
#endregion


#region Configure Application Session Cookie

builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    //Set a short timeout for easy testing 
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    //Make the session cokkie essential 
    options.Cookie.IsEssential = true;
});


#endregion


#region Connect the Secuirty Interface Implementation with the Implementation 
builder.Services.AddScoped<ISecurityApplicationService, SecurityIdentityApplicationService>();
builder.Services.AddControllersWithViews();
#endregion


var app = builder.Build();

#region Adding the admin and manager to database when the app start building between phase of Build and run 
//we used (using) because after what inside using will be used will be delteded or dispacthed
using (var scope = app.Services.CreateScope())
{
    UserManager<User> userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
    await DatabaseFake.EnsureAdminCreated(userManager);
}
#endregion

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

#region Use of Session
app.UseSession();
#endregion


app.UseRouting();

#region Authentication and Authorization
app.UseAuthentication();
app.UseAuthorization();
#endregion


#region Map Controllers and Routing



app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
                  name: "areas",
                  pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
                );
});

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=SplashScreen}/{id?}");

#endregion
app.Run();

