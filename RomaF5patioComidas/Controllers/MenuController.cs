using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RomaF5patioComidas.Models;
using RomaF5patioComidas.Services.MenuService;

namespace RomaF5patioComidas.Controllers
{
    [Authorize(Roles = "ADMIN")]
    public class MenuController : Controller
    {

        private readonly IMenuService _service;

        public MenuController(IMenuService service)
        {
            _service = service;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                return View(await _service.GetMenu());

            }
            catch (ArgumentNullException)
            {
                return NotFound();
            }

        }


        public async Task<IActionResult> Details(int? id)
        {
            try
            {
                return View(await _service.GetById(id));
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
                    await _service.Create(menu);                   
                }
                catch (DbUpdateException ex)
                {
                    return NotFound(ex.Message);
                }
                return RedirectToAction(nameof(Index));
            }
            return View(menu);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            try
            {
                return View(await _service.GetById(id));
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
                    await _service.Update(menu);                  
                }
                catch (DbUpdateException ex)
                {
                    return NotFound(ex.InnerException);
                }
                return RedirectToAction(nameof(Index));
            }
            return View(menu);

        }

        public async Task<IActionResult> Delete(int? id)
        {
            try
            {
                return View(await _service.GetById(id));
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
                await _service.Delete(menu);                
            }
            catch (DbUpdateException ex)
            {
                return NotFound(ex.InnerException);
            }
            return RedirectToAction(nameof(Index));
        }

    }
}
