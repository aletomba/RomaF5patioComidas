using Microsoft.EntityFrameworkCore;
using RomaF5patioComidas.Data;
using RomaF5patioComidas.Models;

namespace RomaF5patioComidas.Services.MenuService
{
    public class MenuService : IMenuService
    {
        private readonly RomaF5BdContext _context;

        public MenuService(RomaF5BdContext context)
        {
            _context = context;
        }

        public async Task Create(Menu menu)
        {
            try
            {
                _context.Add(menu);
                await _context.SaveChangesAsync();//exception capturada revisar
            }
            catch (DbUpdateException)
            {
                throw new DbUpdateException();
            }

        }

        public async Task Delete(Menu menu)
        {
            try
            {
                menu.Eliminar = true;
                _context.Update(menu);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                throw new DbUpdateException();
            }

        }

        public async Task<Menu> GetById(int? id)
        {
            return await _context.Menu.FindAsync(id) ?? throw new ArgumentNullException();
        }

        public async Task<IEnumerable<Menu>> GetMenu()
        {
            var menuList = await _context.Menu.Where(x => x.Eliminar == false || x.Eliminar == null).ToListAsync();
            return menuList ?? throw new ArgumentNullException();
        }

        public async Task Update(Menu menu)
        {
            try
            {
                _context.Update(menu);
                await _context.SaveChangesAsync();//falta capturar exepcion de carga de datos
            }
            catch(DbUpdateException)
            {
                throw new DbUpdateException();
            }

        }
    }
}
