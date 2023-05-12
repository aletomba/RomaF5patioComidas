using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using RomaF5patioComidas.Data;
using RomaF5patioComidas.Models;
using RomaF5patioComidas.Repository;
using RomaF5patioComidas.Services.BebidasServices;
using RomaF5patioComidas.Services.HomeService;
using RomaF5patioComidas.Services.MesaService;
using RomaF5patioComidas.Services.PedidoService;
using RomaF5patioComidas.Services.TipoBebidaService;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews()
  .AddRazorOptions(options =>
   {
       options.ViewLocationFormats.Add("/Components/ItemPedidos/Default.cshtml");
   });
builder.Services.AddDbContext<RomaF5BdContext>(options => options.UseSqlServer(
    builder.Configuration.GetConnectionString("ConnectionDB")));
//builder.Services.AddDistributedMemoryCache();
builder.Services.AddDistributedMemoryCache();

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromHours(2);
    options.Cookie.IsEssential = true;
});
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(option =>
{
    option.LoginPath = "/Login/Index";
    option.AccessDeniedPath = "/Login/Privacy";
});

builder.Services.AddTransient<IBebidaService, BebidaService>();
builder.Services.AddTransient<ITipobebidaService, TipoBebidaService>();
builder.Services.AddTransient<ILoginService,LoginService>();
builder.Services.AddScoped <IRepository<Menu>,Repository<Menu>>();
builder.Services.AddTransient<IMesaService, MesaService>();
builder.Services.AddTransient<IPedidoService, PedidoService>();



var app = builder.Build();
app.UseSession();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Login/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Mesas}/{action=Index}/{id?}");

app.Run();
