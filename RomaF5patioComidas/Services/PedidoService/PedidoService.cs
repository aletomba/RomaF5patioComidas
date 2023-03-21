using Microsoft.EntityFrameworkCore;
using RomaF5patioComidas.Data;
using RomaF5patioComidas.Models;

namespace RomaF5patioComidas.Services.PedidoService
{
    public class PedidoService : IPedidoService
    {
        private readonly RomaF5BdContext _context;

        public PedidoService(RomaF5BdContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Pedido>> GetPedido()
        {
            return await _context.Pedido.Include(x => x.IdBebidaNavigation).
                Include(x => x.IdmenuNavigation).Include(x => x.IdMesaNavigation).
                Where(x => x.Eliminar == false || x.Eliminar == null && x.Fecha.Value.Date == DateTime.Today.Date)
                .ToListAsync() ?? throw new ArgumentNullException();
        }

        public async Task<Pedido> GetById(int? id)
        {
            return await _context.Pedido.FindAsync(id) ?? throw new ArgumentNullException("Pedido no encontrado");
        }

        public async Task Create(Pedido pedido,double? bebidaPrecio,int? menuPrecio)
        {
            try
            {
                pedido.Fecha = DateTime.Now;
                pedido.Estado = true;
                var precioBebida = bebidaPrecio * pedido.CantidadBebida;
                var precioMenu = menuPrecio * pedido.CantidadMenu;
                pedido.Total = precioBebida + precioMenu;
                _context.Add(pedido);
                await _context.SaveChangesAsync();
            }
            catch(DbUpdateException)
            {
                throw new DbUpdateException("Error al guardar datos");
            }           
          
        }

        public async Task Update(Pedido pedido,double? bebidaPrecio,int? menuPrecio)
        {
            try
            {                
                var precioMenu = pedido.CantidadMenu * menuPrecio;
                var precioBebida = pedido.CantidadBebida * bebidaPrecio;
                pedido.Total = precioMenu + precioBebida;
                pedido.Fecha = DateTime.Now;
                pedido.Estado = true;
                _context.Update(pedido);
                await _context.SaveChangesAsync();
            }
            catch(DbUpdateException)
            {
                throw new DbUpdateException("Error al carga datos");
            }
          
        }


        public async Task Delete(Pedido pedido)
        {
            try
            {
                pedido.Eliminar = true;
                _context.Update(pedido);
                await _context.SaveChangesAsync();
            }
            catch(DbUpdateException)
            {
                throw new DbUpdateException("Error al borrar Pedido");
            }
            
        }

        public async Task<List<Pedido>> Details(int? id)
        {
            var pedidoRoma = await _context.Pedido.Include(x => x.IdBebidaNavigation)
                                                   .Include(x => x.IdmenuNavigation)
                                                   .Include(x => x.IdMesaNavigation)
                                                   .Where(x => x.Eliminar == false || x.Eliminar == null 
                                                    && x.IdMesa == id && x.Estado == true)
                                                   .ToListAsync();
            return pedidoRoma ?? throw new ArgumentNullException();
        }

        public async Task<Pedido> GetForDelete(int? id)
        {
            return  await _context.Pedido.Include(x => x.IdBebidaNavigation)
                                                   .Include(x => x.IdmenuNavigation)
                                                   .Include(x => x.IdMesaNavigation)
                                                   .Where(x => x.Eliminar == false || x.Eliminar == null
                                                    && x.IdPedido == id && x.Estado == true)
                                                   .FirstOrDefaultAsync() ?? throw new ArgumentNullException("Pedido no encontrado");
        }
    }
}
