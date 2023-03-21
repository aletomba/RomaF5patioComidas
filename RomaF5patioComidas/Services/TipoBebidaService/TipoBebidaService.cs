using Microsoft.EntityFrameworkCore;
using RomaF5patioComidas.Data;
using RomaF5patioComidas.Models;

namespace RomaF5patioComidas.Services.TipoBebidaService
{
    public class TipoBebidaService:ITipobebidaService
    {
        private readonly RomaF5BdContext _context;
        public TipoBebidaService(RomaF5BdContext context)
        {
            _context = context; 
        }
        public async Task<List<TipoBebida>> GetTipoBebidas()
        {
            return await _context.TipoBebida.ToListAsync() ?? throw new ArgumentNullException();
        }
    }
}
