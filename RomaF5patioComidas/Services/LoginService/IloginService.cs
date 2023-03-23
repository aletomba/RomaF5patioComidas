using RomaF5patioComidas.Models;

namespace RomaF5patioComidas.Services.HomeService
{
    public interface ILoginService 
    { 
        public Task<Usuario> VerificarLog(Usuario usuario);
      
    }
}
