﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RomaF5patioComidas.Data;
using RomaF5patioComidas.Models;

namespace RomaF5patioComidas.Controllers
{
    public class UsuarioController : Controller
    {
        private readonly RomaF5BdContext _context;
        public UsuarioController(RomaF5BdContext context)
        {
            _context = context;
        }

        // GET: UsuarioController
        public async Task<ActionResult> Index()
        {
            var usuario = await _context.Usuario.Include(b => b.IdCategoriaNavigation).Where(x => x.Eliminar == false || x.Eliminar == null).ToListAsync();
            
            return View(usuario);
        
        }

        // GET: UsuarioController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: UsuarioController/Create
        public IActionResult Create()        {
            
            ViewData["IdCategoria"] = new SelectList(_context.CategoriaUser, "IdCategoria", "Categoria");
            
            return View();
        }

        // POST: UsuarioController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Usuario usuario)
        {
            try
            {
                if(usuario == null)
                {
                    return NoContent();
                }

                if (ModelState.IsValid)
                {
                    _context.Add(usuario);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("Index", "Mesas");
                }
                ViewData["IdCategoria"] = new SelectList(_context.CategoriaUser, "IdCategoria", "Categoria",usuario.IdCategoria);
                return View(nameof(Create));
               
            }
            catch(Exception ex)
            {

                return View(ex.Message);
            }
        }

        // GET: UsuarioController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: UsuarioController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: UsuarioController/Delete/5
        public async Task<ActionResult> Delete(int id)
        {
            if (id == null || _context.Usuario == null)
            {
                return NotFound();
            }

            var usuario = await _context.Usuario
                .Include(b => b.IdCategoriaNavigation)
                .FirstOrDefaultAsync(m => m.IdUser == id);
            if (usuario == null)
            {
                return NotFound();
            }

            return View(usuario);
           
        }

        // POST: UsuarioController/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task< ActionResult> DeleteConfirmed(int id)
        {
            if (_context.Bebida == null)
            {
                return Problem("Entity set 'RomaF5BdContext.Bebida'  is null.");
            }
            var usuario = await _context.Usuario.FindAsync(id);
            if (usuario != null)
            {
                usuario.Eliminar = true;
                _context.Update(usuario);
                await _context.SaveChangesAsync();
            }


            return RedirectToAction(nameof(Index));
        }
    }
}
