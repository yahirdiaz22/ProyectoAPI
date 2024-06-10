using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ProyectoAPI
{
    public class Compra
    {
        [Key] // Esto indica que idCliente es la clave primaria
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public int idCompra { get; set; }


        public int cantidadComprada { get; set; }


        public string fecha { get; set; }
        public double precioUnitario { get; set; }
        public string nombre { get; set; }
        public int status { get; set; }
    }
}
