using Microsoft.EntityFrameworkCore;
using RomaF5patioComidas.Data;
using RomaF5patioComidas.Models;

namespace RomaF5patioComidas.Services.BebidasServices
{
    public class BebidaService : IBebidaService
    {
        private readonly RomaF5BdContext _context;


        public BebidaService(RomaF5BdContext context)
        {
            _context = context;
        }

       
        public async Task<IEnumerable<Bebida>> GetBebidas()
        {
            var romaF5BdContext = await _context.Bebida.Include
                                  (b => b.IdTipobebidaNavigation).
                                  Where(x => x.Eliminar == false || x.Eliminar == null).ToListAsync();
            return romaF5BdContext ?? throw new ArgumentNullException();
        }

        public async Task<Bebida> GetById(int? id)
        {

            var bebida = await _context.Bebida
                        .Include(b => b.IdTipobebidaNavigation)
                        .FirstOrDefaultAsync(m => m.IdBebida == id);

            return bebida ?? throw new ArgumentNullException();

        }

        public async Task Create(Bebida bebida)
        {
            try
            {
                _context.Add(bebida);
                await _context.SaveChangesAsync();
            }
            catch(DbUpdateException)
            {
                throw new DbUpdateException("Error al guardar datos");
            }
            
        }

        public async Task Update(Bebida bebida)
        {
            try
            {
                _context.Update(bebida);
                await _context.SaveChangesAsync();
            }
            catch(DbUpdateException)
            {
                throw new DbUpdateException("Error al editar datos");
            }         

        }   

        public async Task Delete(Bebida bebida)
        {
            try
            {
                bebida.Eliminar = true;
                _context.Update(bebida);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                throw new DbUpdateException("Error al eliminar objeto");
            }          
          
        }
    
    }
}
