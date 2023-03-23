using RomaF5patioComidas.Models;

namespace RomaF5patioComidas.Services.BebidasServices
{
    public interface IBebidaService
    {
      
        public Task<IEnumerable<Bebida>> GetBebidas();
        public Task<Bebida> GetById(int? id);
        public Task Create( Bebida bebida);
        public Task Update(Bebida bebida);    
        public Task Delete(Bebida bebida);
    
    }
}
