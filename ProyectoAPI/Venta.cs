﻿using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ProyectoAPI
{
    public class Venta
    {
        [Key] // Esto indica que idCliente es la clave primaria
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int idVenta { get; set; }

        public int cantidadVendida { get; set; }

        public string nombre { get; set; }

        public string fecha { get; set; }

        public int status { get; set; }
    }
}
