using Microsoft.AspNetCore.Mvc;
using RomaF5patioComidas.Services.PedidoService;

namespace RomaF5patioComidas.Components
{
    public class PedidosViewComponent:ViewComponent
    {
        private readonly IPedidoService _service;

        public PedidosViewComponent(IPedidoService service)
        {
            _service = service; 
        }

        //public async Task<IViewComponentResult>InvokeAsync()=> View();
    }
}
