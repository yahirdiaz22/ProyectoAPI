using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ProyectoAPI
{
    public class Usuario
    {
        [Key] // Esto indica que idCliente es la clave primaria
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int idUsuario { get; set; }

       
        public string nombre { get; set; }

        public string correoElectronico { get; set; }


        public string password { get; set; }

        public int status { get; set; }
    }
}

