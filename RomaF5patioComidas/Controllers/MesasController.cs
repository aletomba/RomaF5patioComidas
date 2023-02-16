using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RomaF5patioComidas.Data;
using RomaF5patioComidas.Models;

namespace RomaF5patioComidas.Controllers
{
    [Authorize]
    public class MesasController : Controller
    {
        private readonly RomaF5BdContext _context;

        public MesasController(RomaF5BdContext context)
        {
            _context = context;
        }

        // GET: Mesas
        public async Task<IActionResult> Index()
        {
              return View(await _context.Mesa.Where(x=>x.Eliminar == false || x.Eliminar == null ).ToListAsync());
        }

        // GET: Mesas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Mesa == null)
            {
                return NotFound();
            }

            var mesa = await _context.Mesa
                .FirstOrDefaultAsync(m => m.IdMesa == id);
            if (mesa == null)
            {
                return NotFound();
            }

            return View(mesa);
        }

        // GET: Mesas/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Mesas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create( Mesa mesa)
        {
            if (ModelState.IsValid)
            {
                mesa.Reserva = false;
                mesa.Estado = false;    
                _context.Add(mesa);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(mesa);
        }

        // GET: Mesas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Mesa == null)
            {
                return NotFound();
            }

            var mesa = await _context.Mesa.FindAsync(id);
            if (mesa == null)
            {
                return NotFound();
            }
            return View(mesa);
        }

        // POST: Mesas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdMesa,Descripcion,Estado,Eliminar")] Mesa mesa)
        {
            if (id != mesa.IdMesa)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(mesa);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MesaExists(mesa.IdMesa))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(mesa);
        }
        [HttpGet]
        public async Task<IActionResult> Reserva(int? id)
        {
            if (id == null || _context.Mesa == null)
            {
                return NotFound();
            }

            var mesa = await _context.Mesa.FindAsync(id);
            if (mesa == null)
            {
                return NotFound();
            }
            return View(mesa);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Reserva(int? id, Mesa mesa)
        {
            if (id != mesa.IdMesa)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                try
                {
                    mesa.Reserva = true;
                    _context.Update(mesa);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MesaExists(mesa.IdMesa))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> EliminarReserva(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            try
            {
                var mesa = await _context.Mesa.FindAsync(id);
                mesa.Reserva = false;
                _context.Update(mesa);
            }
            catch (DbUpdateException ex)
            {
                return NotFound(ex.InnerException);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        // GET: Mesas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Mesa == null)
            {
                return NotFound();
            }

            var mesa = await _context.Mesa
                .FirstOrDefaultAsync(m => m.IdMesa == id);
            if (mesa == null)
            {
                return NotFound();
            }

            return View(mesa);
        }

        // POST: Mesas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Mesa == null)
            {
                return Problem("Entity set 'RomaF5BdContext.Mesa'  is null.");
            }
            var mesa = await _context.Mesa.FindAsync(id);
            if (mesa != null)
            {
               mesa.Eliminar = true;
                _context.Update(mesa);
                await _context.SaveChangesAsync();
            }
            
            
            return RedirectToAction(nameof(Index));
        }

        private bool MesaExists(int? id)
        {
          return _context.Mesa.Any(e => e.IdMesa == id);
        }
    }
}
