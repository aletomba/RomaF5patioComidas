using RomaF5patioComidas.Models;

namespace RomaF5patioComidas.Services.TipoBebidaService
{
    public interface ITipobebidaService
    {
        public  Task<List<TipoBebida>> GetTipoBebidas();
    }
}
