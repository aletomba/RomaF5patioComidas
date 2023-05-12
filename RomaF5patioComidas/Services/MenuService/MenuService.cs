using Microsoft.EntityFrameworkCore;
using RomaF5patioComidas.Data;
using RomaF5patioComidas.Models;
using RomaF5patioComidas.Repository;

namespace RomaF5patioComidas.Services.MenuService
{
    public class MenuService 
    {
        private readonly IRepository<Menu> _menuRepository;
        public MenuService(IRepository<Menu> menuRepository)
        {
            _menuRepository = menuRepository;
        }


        public async Task CreateAsync(Menu menu)
        {
            
            try
            {
                await _menuRepository.AddAsync(menu);
                await _menuRepository.SaveChangesAsync();//exception capturada revisar
            }
            catch (DbUpdateException)
            {
                throw new DbUpdateException();
            }

        }

    //    public async Task Delete(Menu menu)
    //    {
    //        try
    //        {
    //            menu.Eliminar = true;
    //            _context.Update(menu);
    //            await _context.SaveChangesAsync();
    //        }
    //        catch (DbUpdateException)
    //        {
    //            throw new DbUpdateException();
    //        }

    //    }

    //    public async Task<Menu> GetById(int? id)
    //    {
    //        return await _context.Menu.FindAsync(id) ?? throw new ArgumentNullException();
    //    }

    //    public async Task<IEnumerable<Menu>> GetMenu()
    //    {
    //        var menuList = await _context.Menu.Where(x => x.Eliminar == false || x.Eliminar == null).ToListAsync();
    //        return menuList ?? throw new ArgumentNullException();
    //    }

    //    public async Task Update(Menu menu)
    //    {
    //        try
    //        {
    //            _context.Update(menu);
    //            await _context.SaveChangesAsync();//falta capturar exepcion de carga de datos
    //        }
    //        catch(DbUpdateException)
    //        {
    //            throw new DbUpdateException();
    //        }

    //    }
    }
}
