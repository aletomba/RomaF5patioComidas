using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using RomaF5patioComidas.Data;
using RomaF5patioComidas.Services.BebidasServices;
using RomaF5patioComidas.Services.HomeService;
using RomaF5patioComidas.Services.MenuService;
using RomaF5patioComidas.Services.MesaService;
using RomaF5patioComidas.Services.PedidoService;
using RomaF5patioComidas.Services.TipoBebidaService;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<RomaF5BdContext>(options => options.UseSqlServer(
    builder.Configuration.GetConnectionString("ConnectionDB")));
//builder.Services.AddDistributedMemoryCache();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(option =>
{
    option.LoginPath = "/Login/Index";
    option.AccessDeniedPath = "/Login/Privacy";
});

builder.Services.AddTransient<IBebidaService, BebidaService>();
builder.Services.AddTransient<ITipobebidaService, TipoBebidaService>();
builder.Services.AddTransient<IloginService,LoginService>();
builder.Services.AddTransient<IMenuService,MenuService>();
builder.Services.AddTransient<IMesaService, MesaService>();
builder.Services.AddTransient<IPedidoService, PedidoService>();



var app = builder.Build();

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
