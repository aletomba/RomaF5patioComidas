﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RomaF5patioComidas.Models
{
    public partial class Mesa
    {
        public Mesa()
        {
            Pedido = new HashSet<Pedido>();
        }

        public int IdMesa { get; set; }
        public string Descripcion { get; set; }
        public bool? Estado { get; set; }
        public bool? Reserva { get; set; }
        public bool? Eliminar { get; set; }
        [Display(Name ="Cliente")]
        public string NombreReserva { get; set; }

        public virtual ICollection<Pedido> Pedido { get; set; }
    }
}