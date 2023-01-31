using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RomaF5patioComidas.Data;
using RomaF5patioComidas.Models;

namespace RomaF5patioComidas.Controllers
{
    public class TipoBebidasController : Controller
    {
        private readonly RomaF5BdContext _context;

        public TipoBebidasController(RomaF5BdContext context)
        {
            _context = context;
        }

        // GET: TipoBebidas
        public async Task<IActionResult> Index()
        {
              return View(await _context.TipoBebida.ToListAsync());
        }

        // GET: TipoBebidas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.TipoBebida == null)
            {
                return NotFound();
            }

            var tipoBebida = await _context.TipoBebida
                .FirstOrDefaultAsync(m => m.IdTipobebida == id);
            if (tipoBebida == null)
            {
                return NotFound();
            }

            return View(tipoBebida);
        }

        // GET: TipoBebidas/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: TipoBebidas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdTipobebida,Litros,Eliminar,Descripcion")] TipoBebida tipoBebida)
        {
            if (ModelState.IsValid)
            {
                _context.Add(tipoBebida);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(tipoBebida);
        }

        // GET: TipoBebidas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.TipoBebida == null)
            {
                return NotFound();
            }

            var tipoBebida = await _context.TipoBebida.FindAsync(id);
            if (tipoBebida == null)
            {
                return NotFound();
            }
            return View(tipoBebida);
        }

        // POST: TipoBebidas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdTipobebida,Litros,Eliminar,Descripcion")] TipoBebida tipoBebida)
        {
            if (id != tipoBebida.IdTipobebida)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(tipoBebida);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TipoBebidaExists(tipoBebida.IdTipobebida))
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
            return View(tipoBebida);
        }

        // GET: TipoBebidas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.TipoBebida == null)
            {
                return NotFound();
            }

            var tipoBebida = await _context.TipoBebida
                .FirstOrDefaultAsync(m => m.IdTipobebida == id);
            if (tipoBebida == null)
            {
                return NotFound();
            }

            return View(tipoBebida);
        }

        // POST: TipoBebidas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.TipoBebida == null)
            {
                return Problem("Entity set 'RomaF5BdContext.TipoBebida'  is null.");
            }
            var tipoBebida = await _context.TipoBebida.FindAsync(id);
            if (tipoBebida != null)
            {
                _context.TipoBebida.Remove(tipoBebida);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TipoBebidaExists(int id)
        {
          return _context.TipoBebida.Any(e => e.IdTipobebida == id);
        }
    }
}
