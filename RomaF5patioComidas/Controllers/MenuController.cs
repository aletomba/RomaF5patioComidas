using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RomaF5patioComidas.Models;
using RomaF5patioComidas.Repository;
using RomaF5patioComidas.Services.MenuService;

namespace RomaF5patioComidas.Controllers
{
    [Authorize(Roles = "ADMIN")]
    public class MenuController : Controller
    {

     
        private readonly IRepository<Menu> _repository;
        public MenuController(IRepository<Menu> repository)
        {
            
            _repository = repository;   
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                var menues = await _repository.GetAllAsync();
                menues.Where(x => x.Eliminar == false || x.Eliminar == null);
                return View(menues);//await _service.GetMenu());

            }
            catch (ArgumentNullException)
            {
                return NotFound();
            }

        }


        public async Task<IActionResult> Details(int id)
        {
            try
            {
                return View(await _repository.GetByIdAsync(id));
            }
            catch (ArgumentNullException)
            {
                return NotFound();
            }

        }

        public IActionResult Create()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdMenu,Nombre,Precio,Descripcion,Eliminar")] Menu menu)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _repository.AddAsync(menu);
                    await _repository.SaveChangesAsync();//_service.Create(menu);                   
                }
                catch (DbUpdateException ex)
                {
                    return NotFound(ex.Message);
                }
                return RedirectToAction(nameof(Index));
            }
            return View(menu);
        }

        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                return View(await _repository.GetByIdAsync(id));
            }
            catch (ArgumentNullException ex)
            {
                return NotFound(ex.Message);
            }

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([Bind("IdMenu,Nombre,Precio,Descripcion,Eliminar")] Menu menu)
        {

            if (ModelState.IsValid)
            {
                try
                {
                     _repository.Update(menu);
                    await _repository.SaveChangesAsync();
                }
                catch (DbUpdateException ex)
                {
                    return NotFound(ex.InnerException);
                }
                return RedirectToAction(nameof(Index));
            }
            return View(menu);

        }

        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                return View(await _repository.GetByIdAsync(id));
            }
            catch (ArgumentNullException)
            {
                return NotFound();
            }

        }


        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed([Bind("IdMenu,Nombre,Precio,Descripcion,Eliminar")] Menu menu)
        {
            try
            {
                 menu.Eliminar = true;
                 _repository.Update(menu);
                await _repository.SaveChangesAsync();   
            }
            catch (DbUpdateException ex)
            {
                return NotFound(ex.InnerException);
            }
            return RedirectToAction(nameof(Index));
        }

    }
}
