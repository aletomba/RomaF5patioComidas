using RomaF5patioComidas.Models;

namespace RomaF5patioComidas.Services.HomeService
{
    public interface IloginService 
    { 
        public Task<Usuario> verificarLog(Usuario usuario);
      
    }
}
