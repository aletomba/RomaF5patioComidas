using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RomaF5patioComidas.Models;
using RomaF5patioComidas.Services.MesaService;

namespace RomaF5patioComidas.Controllers
{
    [Authorize]
    public class MesasController : Controller
    {
       
        private readonly IMesaService _service;

        public MesasController(IMesaService service)
        {
            
            _service = service;
        }


        public async Task<IActionResult> Index()
        {
            try
            {
                ViewData["Cobrado"] = Request.Cookies["user"];
                Response.Cookies.Delete("user");

                return View(await _service.GetMesa());
            }
            catch (BadHttpRequestException ex)
            {
                return BadRequest(ex.Message);
            }

        }


        public async Task<IActionResult> Details(int? id)
        {
            if (id != null) return NotFound();
            try
            {
                return View( await _service.GetById(id));                
            }          
            catch (BadHttpRequestException ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [Authorize(Roles = "ADMIN")]
        public IActionResult Create()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Mesa mesa)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _service.Create(mesa);                    
                }
                catch (DbUpdateException ex)
                {
                    return NotFound(ex.Message);
                }
                return RedirectToAction(nameof(Index));

            }
            return View(mesa);
        }


        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id != null) return NotFound();
            try
            {
                return View(await _service.GetById(id));
            }          
            catch (BadHttpRequestException ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([Bind("IdMesa,Descripcion,Estado,Eliminar")] Mesa mesa)
        {          

            if (ModelState.IsValid)
            {
                try
                {
                    await _service.Update(mesa);
                }
                catch (DbUpdateException ex)
                {
                  return NotFound(ex.Message);
                }
                return RedirectToAction(nameof(Index));
            }
            return View(mesa);
        }


        [HttpGet]
        public async Task<IActionResult> Reserva(int? id)
        {
            if (id != null) return NotFound();
            try
            {
                return View(await _service.GetById(id));
            }           
            catch (BadHttpRequestException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Reserva(Mesa mesa)
        {           
            if (ModelState.IsValid)
            {
                try
                {
                    await _service.Reserva(mesa);
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    return BadRequest(ex.Message);
                }
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EliminarReserva(int? id)
        {
            if (id != null) return NotFound();
            try
            {
                await _service.EliminarReserva(id);
            }
            catch (DbUpdateException ex)
            {
                return NotFound(ex.Message);
            }           
            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id != null) return NotFound();
            try
            {
                var mesa = await _service.GetById(id);
                return View(mesa);
            }         
            catch (DbUpdateException ex)
            {
                return BadRequest(ex.Message);
            }          
        }


        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Mesa mesa)
        {
            try
            {
               await _service.Delete(mesa);
            }
            catch(DbUpdateException ex)
            {
                return BadRequest(ex.Message);
            }

            return RedirectToAction(nameof(Index));
        }
      
    }
}
