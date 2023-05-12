﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RomaF5patioComidas.Models
{
    public partial class Pedido
    {
        public int IdPedido { get; set; }
        [Display(Name = "Bebida")]
        public int IdBebida { get; set; }
        [Display(Name = "Consumibles")]
        public int Idmenu { get; set; }
        public double? Total { get; set; }
        [Display(Name = "Cantidad")]
        public int? CantidadMenu { get; set; }
        [Display(Name = "Cantidad")]
        public int? CantidadBebida { get; set; }
        public bool? Eliminar { get; set; }
        public int? IdMesa { get; set; }
        public DateTime? Fecha { get; set; }
        public bool? Estado { get; set; }

        public int? PrecioTurno { get; set; }

        public virtual Bebida IdBebidaNavigation { get; set; }
        [Display(Name = "Turno")]
        public virtual Mesa IdMesaNavigation { get; set; }
        [Display(Name = "Consumibles")]
        public virtual Menu IdmenuNavigation { get; set; }
    }
}