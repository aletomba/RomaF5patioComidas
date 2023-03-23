using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RomaF5patioComidas.Data;
using RomaF5patioComidas.Models;
using System.Security.Claims;

namespace RomaF5patioComidas.Services.HomeService
{
    public class LoginService : ILoginService
    {
        private readonly RomaF5BdContext _context;
        public LoginService(RomaF5BdContext context)
        {
            _context = context;
        }

        public async Task<Usuario> VerificarLog(Usuario usuario)
        {
            var user = await _context.Usuario.Include(x => x.IdCategoriaNavigation).
                  Where(x => x.Usuario1 == usuario.Usuario1 && x.Clave == usuario.Clave).
                  FirstOrDefaultAsync();

            if (user == null || _context.Usuario == null)
            {
                throw new Exception();
            }
            return user;
        }     
  
    }
}
