using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RomaF5patioComidas.Data;
using RomaF5patioComidas.Models;


namespace RomaF5patioComidas.Controllers
{
    public class PedidoController : Controller
    {

        private readonly RomaF5BdContext _context;       

        public PedidoController(RomaF5BdContext context)
        {
            _context = context; 
            
        }

        // GET: PedidoController
        public async Task<ActionResult> Index()
        {

            var pedidoRoma = _context.Pedido.Include(x => x.IdBebidaNavigation).
                Include(x => x.IdmenuNavigation).Include(x=>x.IdMesaNavigation).
                Where(x=>x.Eliminar == false || x.Eliminar == null);
            return View(await pedidoRoma.ToListAsync());
        }

        // GET: PedidoController/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            //var mesaPedido = await _context.Mesa.Include(x => x.Pedido).Where(x => x.Estado == true && x.IdMesa == id).ToListAsync();
            var pedidoRoma = await _context.Pedido.Include(x => x.IdBebidaNavigation)
                                                  .Include(x => x.IdmenuNavigation)
                                                  .Include(x => x.IdMesaNavigation)
                                                  .Where(x=>x.Eliminar == false || x.Eliminar == null && x.IdMesa == id && x.IdMesaNavigation.Estado == true)
                                                  .ToListAsync();
            double? total = 0;
            foreach (var item in pedidoRoma)
            {
                total += item.Total;
            }

            ViewData["Total"] = total;
            return View(pedidoRoma);
        }



        // GET: PedidoController/Create
        public ActionResult Create(int? id)
        {             
         
            ViewData["bebidaMarca"] = new SelectList(_context.Bebida.
                Where(x => x.Eliminar == false || x.Eliminar == null), "IdBebida", "Marca");           
            ViewData["MenuDescripcion"] = new SelectList(_context.Menu, "IdMenu", "Descripcion");
            ViewData["idMesa"] = new SelectList(_context.Mesa.
                Where(x=>x.Eliminar == false || x.Eliminar == null && x.IdMesa ==id), "IdMesa", "Descripcion");

            return View();
        }

        // POST: PedidoController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Pedido pedido)
        {          
            var bebida = await _context.Bebida.FindAsync(pedido.IdBebida);
            var menu = await _context.Menu.FindAsync(pedido.Idmenu);
            var mesa = await _context.Mesa.FindAsync(pedido.IdMesa);

            if (mesa.Estado != true || mesa.Estado == null)
            {
                mesa.Estado = true;
               
            }            
            if (ModelState.IsValid)
            {              
                pedido.Fecha = DateTime.Now;
                var precioBebida = bebida.Precio * pedido.CantidadBebida;
                var precioMenu = menu.Precio * pedido.CantidadMenu;
                pedido.Total = precioBebida + precioMenu;   
                _context.Add(pedido);
                await _context.SaveChangesAsync();                
                return RedirectToAction(nameof(Create));
            }   
            
            ViewData["bebidaMarca"] = new SelectList(_context.Bebida, "IdBebida", "IdBebida", pedido.IdBebida);          
            ViewData["MenuDescripcion"] = new SelectList(_context.Menu, "IdMenu", "IdMenu", pedido.Idmenu);
            ViewData["idMesa"] = new SelectList(_context.Mesa, "IdMesa", "IdMesa", pedido.IdMesa);
            return View(pedido);

        }

        // GET: PedidoController/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null || _context.Pedido == null)
            {
                return NotFound();
            }

            var pedido = await _context.Pedido.FindAsync(id);
            if (pedido == null)
            {
                return NotFound();
            }
            ViewData["bebidaNombre"] = new SelectList(_context.Bebida, "IdBebida", "Nombre", pedido.IdBebida);
            ViewData["bebidaMarca"] = new SelectList(_context.Bebida, "IdBebida", "Marca", pedido.IdBebida);
            ViewData["MenuNombre"] = new SelectList(_context.Menu, "IdMenu", "Nombre", pedido.Idmenu);
            ViewData["MenuDescripcion"] = new SelectList(_context.Menu, "IdMenu", "Descripcion", pedido.Idmenu);
            ViewData["idMesa"] = new SelectList(_context.Mesa.
                Where(x => x.Eliminar == false || x.Eliminar == null), "IdMesa", "Descripcion", pedido.IdMesa);

            return View(pedido);
           
        }

        // POST: PedidoController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int id,Pedido pedido)
        {
            var bebida = await _context.Bebida.FindAsync(pedido.IdBebida);
            var menu = await _context.Menu.FindAsync(pedido.Idmenu);

            if (id != pedido.IdPedido)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    pedido.Total = 0;
                    var precioMenu = pedido.CantidadMenu * menu.Precio;
                    var precioBebida = pedido.CantidadBebida * bebida.Precio;
                    pedido.Total = precioMenu + precioBebida;
                    _context.Update(pedido);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PedidoExists(pedido.IdBebida))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Details","Pedido", new { id = pedido.IdMesa });
            }
            ViewData["bebidaNombre"] = new SelectList(_context.Bebida, "IdBebida", "Nombre", pedido.IdBebida);
            ViewData["bebidaMarca"] = new SelectList(_context.Bebida, "IdBebida", "Marca", pedido.IdBebida);
            ViewData["MenuNombre"] = new SelectList(_context.Menu, "IdMenu", "Nombre", pedido.Idmenu);
            ViewData["MenuDescripcion"] = new SelectList(_context.Menu, "IdMenu", "Descripcion", pedido.Idmenu);
            ViewData["idMesa"] = new SelectList(_context.Mesa.Where(x => x.Eliminar == false || x.Eliminar == null), "IdMesa", "Descripcion", pedido.IdMesa);
            return View(pedido);
        }

        // GET: PedidoController/Delete/5
        public async Task< ActionResult> Delete(int? id)
        {
            if (id == null || _context.Pedido == null)
            {
                return NotFound();
            }

            var pedido = await _context.Pedido
                .Include(b => b.IdBebidaNavigation)
                .Include(b=>b.IdmenuNavigation)
                .Include(b=>b.IdMesaNavigation)
                .FirstOrDefaultAsync(m => m.IdPedido == id);
            if (pedido == null)
            {
                return NotFound();
            }
            return View();
        }

        // POST: PedidoController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(int id)
        {
            if (_context.Pedido == null)
            {
                return Problem("Entity set 'RomaF5BdContext.Bebida'  is null.");
            }
            var pedido = await _context.Pedido.FindAsync(id);
            if (pedido != null)
            {
                pedido.Eliminar = true;
                _context.Update(pedido);
                await _context.SaveChangesAsync();  
            }

            
            return RedirectToAction(nameof(Index));
        }

        public async Task<ActionResult> Cobrar(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var mesa = await _context.Mesa.FindAsync(id);
            mesa.Estado = false;            
            _context.Update(mesa);
            await _context.SaveChangesAsync();
           return RedirectToAction("Index", "Mesas");
        }

        private bool PedidoExists(int id)
        {
            return _context.Pedido.Any(e => e.IdPedido == id);
        }
    }
}
