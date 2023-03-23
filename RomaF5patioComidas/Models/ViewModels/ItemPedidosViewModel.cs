using System.ComponentModel.DataAnnotations;

namespace RomaF5patioComidas.Models.ViewModels
{
    public class ItemPedidosViewModel
    {
        public int IdPedido { get; set; }
        [Display(Name = "Bebida")]
        public string Bebida { get; set; }

        [Display(Name = "Cantidad")]      
        public int? CantidadBebida { get; set;}
        public string Menu { get; set; }

        [Display(Name = "Cantidad")]
        public int? CantidadMenu { get; set; }
        public string Mesa { get; set; }

        [Display(Name = "Fecha y Hora")]
        public DateTime Fecha { get; set; }
        public double? Total { get; set; }
        public int Cantidad { get; set; }

        public ItemPedidosViewModel()
        {

        }
        public ItemPedidosViewModel(Pedido pedido)
        {
            IdPedido = pedido.IdPedido;
            Bebida = pedido.IdBebidaNavigation.Marca;
            CantidadBebida = pedido.CantidadBebida;
            Menu = pedido.IdmenuNavigation.Descripcion;
            CantidadMenu = pedido.CantidadMenu;
            Mesa = pedido.IdMesaNavigation.Descripcion;
            Fecha = pedido.Fecha;
            Total = pedido.Total;
            Cantidad = 1;
        }


    }
}
