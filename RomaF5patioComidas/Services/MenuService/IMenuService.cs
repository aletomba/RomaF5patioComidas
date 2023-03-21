using RomaF5patioComidas.Models;

namespace RomaF5patioComidas.Services.MenuService
{
    public interface IMenuService
    {
        public Task<IEnumerable<Menu>> GetMenu();
        public Task<Menu>GetById(int? id);
        public Task Create(Menu menu);
        public Task Update(Menu menu);  
        public Task Delete(Menu menu); 
    }
}
