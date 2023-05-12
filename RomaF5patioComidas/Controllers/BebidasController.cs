using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RomaF5patioComidas.Data;
using RomaF5patioComidas.Models;
using RomaF5patioComidas.Services.BebidasServices;
using RomaF5patioComidas.Services.TipoBebidaService;

namespace RomaF5patioComidas.Controllers
{
    [Authorize(Roles = "ADMIN")]
    public class BebidasController : Controller
    {
        private readonly IBebidaService _BebidaService;
        private readonly ITipobebidaService _TipoBebidaservice;
      
        public BebidasController(IBebidaService service, ITipobebidaService tipoBebidaservice)
        {
            _BebidaService = service;           
            _TipoBebidaservice = tipoBebidaservice;
        }


        public async Task<IActionResult> Index()
        {
            try
            {
                var listaBebidas = _BebidaService.GetBebidas();
                return View(await listaBebidas);
            }
            catch (ArgumentNullException)
            {
                return NotFound();
            }

        }


        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            try
            {
                var bebida = await _BebidaService.GetById(id);
                return View(bebida);
            }
            catch (ArgumentNullException)
            {
                return BadRequest();
            }
        }


        public IActionResult Create()
        {
            
            ViewData["IdTipobebida"] = new SelectList(_TipoBebidaservice.GetTipoBebidas().Result, "IdTipobebida", "Descripcion");
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdBebida,Nombre,Marca,Precio,Stock,IdTipobebida,Eliminar")]
        Bebida bebida)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _BebidaService.Create(bebida);
                }
                catch (DbUpdateException ex)
                {
                    return NotFound(ex.Message);
                }
                
                return RedirectToAction(nameof(Index));
            }           
           
            ViewData["IdTipobebida"] = new SelectList(_TipoBebidaservice.GetTipoBebidas().Result, "IdTipobebida", "IdTipobebida", bebida.IdTipobebida);
            return View(bebida);
        }
        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            try
            {
                var bebida = await _BebidaService.GetById(id);
                ViewData["IdTipobebida"] = new SelectList(_TipoBebidaservice.GetTipoBebidas().Result, "IdTipobebida", "Descripcion");
                return View(bebida);
            }
            catch (Exception)
            {
                return NoContent();
            }

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([Bind("IdBebida,Nombre,Marca,Precio,Stock,IdTipobebida")] Bebida bebida)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _BebidaService.Update(bebida);
                }
                catch (DbUpdateException ex)
                {
                    return BadRequest(ex.Message);
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdTipobebida"] = new SelectList(_TipoBebidaservice.GetTipoBebidas().Result, "IdTipobebida", "Descripcion", bebida.IdTipobebida);
            return View(bebida);
        }


        public async Task<IActionResult> Delete(int id)
        {          
            try
            {
                return View(await _BebidaService.GetById(id));       
            }
            catch (ArgumentNullException ex)
            {
                return BadRequest(ex.Message);
            }           

        }


        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Bebida bebida)
        {
            try
            {
                await _BebidaService.Delete(bebida);
            }
            catch (DbUpdateException ex)
            {
                return BadRequest(ex.Message);
            }

            return RedirectToAction(nameof(Index));
        }

    }
}
