using Microsoft.EntityFrameworkCore;
using RomaF5patioComidas.Data;
using RomaF5patioComidas.Models;

namespace RomaF5patioComidas.Services.MesaService
{
    public class MesaService : IMesaService
    {
        private readonly RomaF5BdContext _context;

        public MesaService(RomaF5BdContext context)
        {
            _context = context;
        }

        public async Task Create(Mesa mesa)
        {
            try
            {
                mesa.Reserva = false;
                mesa.Estado = false;
                _context.Add(mesa);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                throw new DbUpdateException("Error al guardar");
            }

        }

        public async Task Delete(Mesa mesa)
        {
            try
            {
                mesa.Eliminar = true;
                _context.Update(mesa);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                throw new DbUpdateException("Error al eliminar");
            }
            
          
        }

        public async Task<Mesa> GetById(int? id)
        {
           return await _context.Mesa.FindAsync(id) ?? throw new BadHttpRequestException("Elemento no encontrado");
        }


        public async Task<IEnumerable<Mesa>> GetMesa()
        {
            return await _context.Mesa.Where(x => x.Eliminar == false || x.
            Eliminar == null).ToListAsync() ?? throw new BadHttpRequestException("Error de conexion");
        }


        public async Task Update(Mesa mesa)
        {
            try
            {
                mesa.Reserva = false;
                mesa.Estado = false;
                _context.Update(mesa);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                throw new DbUpdateException("Error al guardar objeto");
            }

        }

        public async Task Reserva(Mesa mesa)
        {
            try
            {
                mesa.Reserva = true;
                _context.Update(mesa);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                throw new DbUpdateConcurrencyException("Error al guardar");
            }
        }

        public async Task EliminarReserva(int? id)
        {
            try
            {
                var mesa = await GetById(id);
                mesa.Reserva = false;
                _context.Update(mesa);
            }
            catch (DbUpdateException)
            {
                throw new DbUpdateException();
            }
           
        }
    }
}

