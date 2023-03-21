using RomaF5patioComidas.Models;

namespace RomaF5patioComidas.Services.MesaService
{
    public interface IMesaService
    {
        public Task<IEnumerable<Mesa>> GetMesa();
        public Task<Mesa> GetById(int? id);
        public Task Create(Mesa mesa);
        public Task Update(Mesa mesa);
        public Task Delete(Mesa mesa);
        public Task Reserva(Mesa mesa);
        public Task EliminarReserva(int? id);
    }
}
