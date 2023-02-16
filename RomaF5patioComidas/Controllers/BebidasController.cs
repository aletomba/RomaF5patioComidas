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
    [Authorize(Roles = "ADMIN")]
    public class BebidasController : Controller
    {
        private readonly RomaF5BdContext _context;

        public BebidasController(RomaF5BdContext context)
        {
            _context = context;
        }

        // GET: Bebidas
        public async Task<IActionResult> Index()
        {
            var romaF5BdContext = _context.Bebida.Include(b => b.IdTipobebidaNavigation).Where(x=>x.Eliminar == false || x.Eliminar == null);
            return View(await romaF5BdContext.ToListAsync());
        }

        // GET: Bebidas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Bebida == null)
            {
                return NotFound();
            }

            var bebida = await _context.Bebida
                .Include(b => b.IdTipobebidaNavigation)
                .FirstOrDefaultAsync(m => m.IdBebida == id);
            if (bebida == null)
            {
                return NotFound();
            }

            return View(bebida);
        }

        // GET: Bebidas/Create
        public IActionResult Create()
        {
            ViewData["IdTipobebida"] = new SelectList(_context.TipoBebida, "IdTipobebida", "Descripcion");
            return View();
        }

        // POST: Bebidas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdBebida,Nombre,Marca,Precio,Stock,IdTipobebida,Eliminar")] Bebida bebida)
        {
            if (ModelState.IsValid)
            {
                _context.Add(bebida);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdTipobebida"] = new SelectList(_context.TipoBebida, "IdTipobebida", "IdTipobebida", bebida.IdTipobebida);
            return View(bebida);
        }

        // GET: Bebidas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Bebida == null)
            {
                return NotFound();
            }

            var bebida = await _context.Bebida.FindAsync(id);
            if (bebida == null)
            {
                return NotFound();
            }
            ViewData["IdTipobebida"] = new SelectList(_context.TipoBebida, "IdTipobebida", "Descripcion", bebida.IdTipobebida);
            return View(bebida);
        }

        // POST: Bebidas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdBebida,Nombre,Marca,Precio,Stock,IdTipobebida,Eliminar")] Bebida bebida)
        {
            if (id != bebida.IdBebida)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(bebida);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BebidaExists(bebida.IdBebida))
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
            ViewData["IdTipobebida"] = new SelectList(_context.TipoBebida, "IdTipobebida", "Descripcion", bebida.IdTipobebida);
            return View(bebida);
        }

        // GET: Bebidas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Bebida == null)
            {
                return NotFound();
            }

            var bebida = await _context.Bebida
                .Include(b => b.IdTipobebidaNavigation)
                .FirstOrDefaultAsync(m => m.IdBebida == id);
            if (bebida == null)
            {
                return NotFound();
            }

            return View(bebida);
        }

        // POST: Bebidas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Bebida == null)
            {
                return Problem("Entity set 'RomaF5BdContext.Bebida'  is null.");
            }
            var bebida = await _context.Bebida.FindAsync(id);
            if (bebida != null)
            {
                bebida.Eliminar = true;
                _context.Update(bebida);
                await _context.SaveChangesAsync();
            }
            
            
            return RedirectToAction(nameof(Index));
        }

        private bool BebidaExists(int id)
        {
          return _context.Bebida.Any(e => e.IdBebida == id);
        }
    }
}
