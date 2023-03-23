using Microsoft.AspNetCore.Mvc;
using RomaF5patioComidas.Models.ViewModels;
using RomaF5patioComidas.SessionExtension;

namespace RomaF5patioComidas.Components
{
    public class ItemPedidosViewComponent : ViewComponent
    {

        public IViewComponentResult Invoke()
        {
            List<ItemPedidosViewModel>? Item = HttpContext.Session.GetJson<List<ItemPedidosViewModel>>("Item");
            CarritoPedido? Carrito;

            if (Item == null || Item.Count == 0)
            {
                Carrito = null;
            }
            else
            {
                Carrito = new()
                {
                    //Id = Item.Where(x => x.)
                    Numeropedidos = Item.Sum(x => x.Cantidad),
                    TotalMonto = Item.Sum(x => x.Cantidad * x.Total)
                };
            }

            return View(Carrito);
        }

    }
}
