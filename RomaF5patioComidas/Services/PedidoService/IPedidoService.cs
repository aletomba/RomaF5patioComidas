using RomaF5patioComidas.Models;

namespace RomaF5patioComidas.Services.PedidoService
{
    public interface IPedidoService
    {
        public Task<IEnumerable<Pedido>> GetPedido();
        public Task<Pedido> GetById(int? id);
        public Task Create(Pedido pedido, double? bebidaPrecio, int? menuPrecio);
        public Task Update(Pedido pedido, double? bebidaPrecio, int? menuPrecio);
        public Task Delete(Pedido pedido);
        public Task<List<Pedido>> Details(int? id);       

    }
}
