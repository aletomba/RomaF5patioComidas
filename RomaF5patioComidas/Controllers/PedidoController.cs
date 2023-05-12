using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RomaF5patioComidas.Data;
using RomaF5patioComidas.Models;
using RomaF5patioComidas.Models.ViewModels;
using RomaF5patioComidas.Repository;
using RomaF5patioComidas.Services.BebidasServices;
using RomaF5patioComidas.Services.MenuService;
using RomaF5patioComidas.Services.MesaService;
using RomaF5patioComidas.Services.PedidoService;
using RomaF5patioComidas.SessionExtension;

namespace RomaF5patioComidas.Controllers
{
    [Authorize]
    public class PedidoController : Controller
    {

        private readonly RomaF5BdContext _context;
        private readonly IPedidoService _pedidoService;
        private readonly IBebidaService _bebidaService;
        private readonly IRepository<Menu> _repository;
        private readonly IMesaService _mesaService;

        public PedidoController(RomaF5BdContext context, IPedidoService pedidoService,
                                IBebidaService bebidaService, IRepository<Menu> repository, IMesaService mesaService)
        {
            _context = context;
            _pedidoService = pedidoService;
            _bebidaService = bebidaService;            
            _mesaService = mesaService;
            _repository = repository;
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

        [HttpGet]
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

        public async Task<IActionResult> AddCarrito(int id)
        {
            try
            {
                var pedido = await _pedidoService.GetById(id);

                List<ItemPedidosViewModel> item = HttpContext.Session.GetJson<List<ItemPedidosViewModel>>("Item") ?? new List<ItemPedidosViewModel>();

                var pedidoItem = item.Where(c => c.IdPedido == id ).FirstOrDefault();

                if (pedidoItem == null )
                {
                    item.Add(new ItemPedidosViewModel(pedido));
                   
                }
                else
                {
                    pedidoItem.Cantidad += 1;
                }

                HttpContext.Session.SetJson("Item", item);

                ViewData["Success"] = "Pedido agregado!";

                return Redirect(Request.Headers["Referer"].ToString());
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
     
        }


        [HttpGet]
        public ActionResult Create(int? id)
        {

            ViewData["bebidaMarca"] = new SelectList(_bebidaService.GetBebidas().Result, "IdBebida", "Marca");

            ViewData["MenuDescripcion"] = new SelectList(_repository.GetAllAsync().Result, "IdMenu", "Descripcion");

            ViewData["idMesa"] = new SelectList(_mesaService.GetMesa().Result.Where(x => x.IdMesa == id), "IdMesa", "Descripcion");

            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind("PrecioTurno","IdBebida", "CantidadBebida", "Idmenu", "CantidadMenu", "IdMesa", "Estado")] Pedido pedido)
        {
            try
            {
               
                var bebida = await _bebidaService.GetById(pedido.IdBebida);

                var menu = await _repository.GetByIdAsync(pedido.Idmenu);

                var mesa = await _mesaService.GetById(pedido.IdMesa);

                if (mesa.Estado != true || mesa.Estado == null)
                {
                    mesa.Estado = true;

                }
                if (ModelState.IsValid)
                {
                    await _pedidoService.Create(pedido, bebida.Precio, menu.Precio);
                    await this.AddCarrito(pedido.IdPedido);
                    return RedirectToAction(nameof(Create));
                }
            }
            catch (ArgumentNullException ex) { return NotFound(ex.Message); }

            catch (DbUpdateException ex) { return BadRequest(ex.Message); }


            ViewData["bebidaMarca"] = new SelectList(_bebidaService.GetBebidas().Result, "IdBebida", "IdBebida", pedido.IdBebida);

            ViewData["MenuDescripcion"] = new SelectList(_repository.GetAllAsync().Result, "IdMenu", "IdMenu", pedido.Idmenu);

            ViewData["idMesa"] = new SelectList(_mesaService.GetMesa().Result, "IdMesa", "IdMesa", pedido.IdMesa);

            return View(pedido);

        }

        [HttpGet]
        public async Task<ActionResult> Edit(int? id)
        {
         
            try
            {
                var pedido = await _pedidoService.GetById(id);

                ViewData["bebidaMarca"] = new SelectList(_bebidaService.GetBebidas().Result, "IdBebida", "Marca");

                ViewData["MenuDescripcion"] = new SelectList(_repository.GetAllAsync().Result, "IdMenu", "Descripcion");

                ViewData["idMesa"] = new SelectList(_mesaService.GetMesa().Result, "IdMesa", "Descripcion");

                return View(pedido);
            }
            catch (ArgumentNullException ex)
            {
                return NotFound(ex.Message);
            }

        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind("PrecioTurno","IdPedido","IdBebida", "CantidadBebida", "Idmenu", "CantidadMenu", "IdMesa", "Estado")] Pedido pedido)
        {
            try
            {

                var bebida = await _bebidaService.GetById(pedido.IdBebida);

                var menu = await _repository.GetByIdAsync(pedido.Idmenu);

                if (ModelState.IsValid)
                {
                    await _pedidoService.Update(pedido,bebida.Precio,menu.Precio);
                    await this.AddCarrito(pedido.IdPedido);
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

            ViewData["MenuDescripcion"] = new SelectList(_repository.GetAllAsync().Result, "IdMenu", "IdMenu", pedido.Idmenu);

            ViewData["idMesa"] = new SelectList(_mesaService.GetMesa().Result, "IdMesa", "IdMesa", pedido.IdMesa);
            return View(pedido);
        }

        [HttpGet]
        public async Task<ActionResult> Delete(int? id)
        {
            try
            {
                var pedidoBorrar = await _pedidoService.GetById(id);

                ViewData["bebidaMarca"] = new SelectList(_bebidaService.GetBebidas().Result, "IdBebida", "Marca");

                ViewData["MenuDescripcion"] = new SelectList(_repository.GetAllAsync().Result, "IdMenu", "Descripcion");

                ViewData["idMesa"] = new SelectList(_mesaService.GetMesa().Result, "IdMesa", "Descripcion");

                return View (pedidoBorrar);
            }
            catch (ArgumentNullException ex)
            {

                return NotFound(ex.Message);
            }              
           
        }

       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(Pedido pedido)
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
                    return RedirectToAction(nameof(Index));
                }
               
            }
            return RedirectToAction("Delete", "Pedido", new { id = pedido?.IdPedido });
        }

        public async Task<ActionResult> Cobrar(int? id)
        {
            List<ItemPedidosViewModel> items = HttpContext.Session.GetJson<List<ItemPedidosViewModel>>("Item");
            items.Clear();
            HttpContext.Session.SetJson("Item", items);
            if (id == null)
            {
                return NotFound();
            }

            var pedidoRoma = await _pedidoService.Details(id);
            foreach (var item in pedidoRoma)
            {
                item.Estado = false;
                _context.Update(item);//crear un metodo en servicio que haga esto !!
            }

            var mesa = await _mesaService.GetById(id);
            await _mesaService.Update(mesa);

            Response.Cookies.Append("user", "Operacion Exitosa");

            return RedirectToAction("Index", "Mesas");
        }

       
    }
}
