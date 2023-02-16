using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using RomaF5patioComidas.Data;
using RomaF5patioComidas.Models;
using System.Diagnostics;
using System.Security.Claims;

namespace RomaF5patioComidas.Controllers
{
    public class HomeController : Controller
    {
        private readonly RomaF5BdContext _context;

        public HomeController(RomaF5BdContext context)
        {
            _context= context;
        }
        [HttpGet]
        public IActionResult Index()
        {
            ClaimsPrincipal c = HttpContext.User;
            if (c.Identity != null)
            {
                if (c.Identity.IsAuthenticated)
                {
                    return RedirectToAction("index", "Mesas");  
                }
            }
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Index(Usuario usuario)
        {
           var user =  await _context.Usuario.Include(x=>x.IdCategoriaNavigation).
                Where(x=> x.Usuario1 == usuario.Usuario1 && x.Clave == usuario.Clave).
                FirstOrDefaultAsync();
            if(user == null)
            {
                ViewData["Mensaje"] = "usuario no encontrado";
                
                return View(nameof(Index));
            }            
           else
            {

                List<Claim> c = new List<Claim>()
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Usuario1),
                    new Claim(ClaimTypes.Role,user.IdCategoriaNavigation.Categoria)
                };
                ClaimsIdentity ci = new(c, CookieAuthenticationDefaults.AuthenticationScheme);
                AuthenticationProperties p = new();
                p.AllowRefresh = true;
                p.IsPersistent = user.MantenerActivo;

                if (!user.MantenerActivo)
                {
                    p.ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(10);
                }
                else
                {
                    p.ExpiresUtc = DateTimeOffset.UtcNow.AddDays(1);
                }

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,new ClaimsPrincipal(ci),p);
                return RedirectToAction("index", "Mesas");
            }     

        }

        public async Task<IActionResult> Salir()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return View(nameof(Index));
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}