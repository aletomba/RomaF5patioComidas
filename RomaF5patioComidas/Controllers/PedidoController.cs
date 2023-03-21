using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RomaF5patioComidas.Data;
using RomaF5patioComidas.Models;
using RomaF5patioComidas.Services.BebidasServices;
using RomaF5patioComidas.Services.MenuService;
using RomaF5patioComidas.Services.MesaService;
using RomaF5patioComidas.Services.PedidoService;

namespace RomaF5patioComidas.Controllers
{
    [Authorize]
    public class PedidoController : Controller
    {

        private readonly RomaF5BdContext _context;
        private readonly IPedidoService _pedidoService;
        private readonly IBebidaService _bebidaService;
        private readonly IMenuService _menuService;
        private readonly IMesaService _mesaService;

        public PedidoController(RomaF5BdContext context, IPedidoService pedidoService,
                                IBebidaService bebidaService, IMenuService menuService, IMesaService mesaService)
        {
            _context = context;
            _pedidoService = pedidoService;
            _bebidaService = bebidaService;
            _menuService = menuService;
            _mesaService = mesaService;
        }


        public async Task<ActionResult> Index()
        {
            try
            {
                var pedidoRoma = await _pedidoService.GetPedido();
                double? total = 0;
                foreach (var item in pedidoRoma)
                {
                    total += item.Total;
                }
                ViewData["Total"] = total;
                return View(pedidoRoma);
            }
            catch (ArgumentNullException ex)
            {
                return NotFound(ex.Message);
            }

        }


        public async Task<ActionResult> Details(int? id)
        {
            try
            {
                var pedidoRoma = await _pedidoService.Details(id);
                double? total = 0;
                foreach (var item in pedidoRoma)
                {
                    total += item.Total;
                }

                ViewData["Total"] = total;
                ViewData["idmesa"] = id;
                return View(pedidoRoma);
            }
            catch (ArgumentNullException ex)
            {
                return NotFound(ex.Message);
            }

        }




        public ActionResult Create(int? id)
        {

            ViewData["bebidaMarca"] = new SelectList(_bebidaService.GetBebidas().Result, "IdBebida", "Marca");

            ViewData["MenuDescripcion"] = new SelectList(_menuService.GetMenu().Result, "IdMenu", "Descripcion");

            ViewData["idMesa"] = new SelectList(_mesaService.GetMesa().Result.Where(x => x.IdMesa == id), "IdMesa", "Descripcion");

            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind("IdBebida", "CantidadBebida", "Idmenu", "CantidadMenu", "IdMesa", "Estado")] Pedido pedido)
        {
            try
            {
                var bebida = await _bebidaService.GetById(pedido.IdBebida);

                var menu = await _menuService.GetById(pedido.Idmenu);

                var mesa = await _mesaService.GetById(pedido.IdMesa);

                if (mesa.Estado != true || mesa.Estado == null)
                {
                    mesa.Estado = true;

                }
                if (ModelState.IsValid)
                {
                    await _pedidoService.Create(pedido, bebida.Precio, menu.Precio);

                    return RedirectToAction(nameof(Create));
                }
            }
            catch (ArgumentNullException ex) { return NotFound(ex.Message); }

            catch (DbUpdateException ex) { return BadRequest(ex.Message); }


            ViewData["bebidaMarca"] = new SelectList(_bebidaService.GetBebidas().Result, "IdBebida", "IdBebida", pedido.IdBebida);

            ViewData["MenuDescripcion"] = new SelectList(_menuService.GetMenu().Result, "IdMenu", "IdMenu", pedido.Idmenu);

            ViewData["idMesa"] = new SelectList(_mesaService.GetMesa().Result, "IdMesa", "IdMesa", pedido.IdMesa);

            return View(pedido);

        }


        public async Task<ActionResult> Edit(int? id)
        {
         
            try
            {
                var pedido = await _pedidoService.GetById(id);

                ViewData["bebidaMarca"] = new SelectList(_bebidaService.GetBebidas().Result, "IdBebida", "Marca");

                ViewData["MenuDescripcion"] = new SelectList(_menuService.GetMenu().Result, "IdMenu", "Descripcion");

                ViewData["idMesa"] = new SelectList(_mesaService.GetMesa().Result.Where(x=>x.IdMesa==id), "IdMesa", "Descripcion");

                return View(pedido);
            }
            catch (ArgumentNullException ex)
            {
                return NotFound(ex.Message);
            }

        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind("IdBebida", "CantidadBebida", "Idmenu", "CantidadMenu", "IdMesa", "Estado")] Pedido pedido)
        {
            try
            {
                var bebida = await _bebidaService.GetById(pedido.IdBebida);

                var menu = await _menuService.GetById(pedido.Idmenu);

                if (ModelState.IsValid)
                {
                    await _pedidoService.Update(pedido,bebida.Precio,menu.Precio);

                    return RedirectToAction("Details", "Pedido", new { id = pedido.IdMesa });
                }
            }
            catch (ArgumentNullException ex)
            {
                return NotFound(ex.Message);
            }
            catch (DbUpdateException ex)
            {
                return BadRequest(ex.Message);
            }

            ViewData["bebidaMarca"] = new SelectList(_bebidaService.GetBebidas().Result, "IdBebida", "IdBebida", pedido.IdBebida);

            ViewData["MenuDescripcion"] = new SelectList(_menuService.GetMenu().Result, "IdMenu", "IdMenu", pedido.Idmenu);

            ViewData["idMesa"] = new SelectList(_mesaService.GetMesa().Result, "IdMesa", "IdMesa", pedido.IdMesa);
            return View(pedido);
        }


        public async Task<ActionResult> Delete(int? id)
        {
            try
            {
                return View (await _pedidoService.GetForDelete(id));
            }
            catch (ArgumentNullException ex)
            {

                return NotFound(ex.Message);
            }              
            
        }

       
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed([Bind("IdPedido","IdBebida", "CantidadBebida", "Idmenu", "CantidadMenu", "IdMesa", "Estado")] Pedido pedido)
        {
            string eliminar = Request.Form["Del"].ToString();

            if (eliminar.Equals("si"))
            {
                if (pedido != null)
                {
                    if (ModelState.IsValid)
                    {
                        try
                        {
                            await _pedidoService.Delete(pedido);
                        }
                        catch(DbUpdateException ex)
                        {
                            return BadRequest(ex.Message);
                        }
                                              
                    }
                    return View(nameof(Index));
                }
               
            }
            return RedirectToAction("Delete", "Pedido", new { id = pedido?.IdPedido });
        }

        public async Task<ActionResult> Cobrar(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pedidoRoma = await _pedidoService.Details(id);
            foreach (var item in pedidoRoma)
            {
                item.Estado = false;
                _context.Update(item);
            }

            var mesa = await _mesaService.GetById(id);
            await _mesaService.Update(mesa);

            Response.Cookies.Append("user", "Operacion Exitosa");

            return RedirectToAction("Index", "Mesas");
        }

       
    }
}
