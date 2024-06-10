using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ProyectoAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompraController : ControllerBase
    {
        private readonly DataContext _context;
        public CompraController(DataContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Compra>>> MostrarCompra()
        {
            return await _context.Compra.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Compra>> GetCompra(int id)
        {
            var compra = await _context.Compra.FindAsync(id);

            if (compra == null)
            {
                return NotFound();
            }

            return compra;
        }

        [HttpPost]
        public async Task<ActionResult<Compra>> AgregarCompra(int cantidadComprada, string fecha,string nombre, double precioUnitario, bool status)
        {
            int Nuevostatus = status ? 1 : 0;

            var nuevaCompra = new Compra
            {
                cantidadComprada = cantidadComprada,
                fecha = fecha,
                nombre = nombre,
                precioUnitario = precioUnitario,
                status = Nuevostatus,
            };

            _context.Compra.Add(nuevaCompra);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetCompra), new { id = nuevaCompra.idCompra }, nuevaCompra);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> ActualizarCompra(int id, int cantidadComprada, string fecha, int precioUnitario, string nombre)
        {
            var compra = await _context.Compra.FindAsync(id);

            if (compra == null)
            {
                return NotFound();
            }

            compra.cantidadComprada = cantidadComprada;
            compra.fecha = fecha;
            compra.nombre = nombre;
            compra.precioUnitario = precioUnitario;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CompraExiste(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> EliminarCompra(int id)
        {
            var compra = await _context.Compra.FindAsync(id);
            if (compra == null)
            {
                return NotFound();
            }

            compra.status = 0; // Cambiando el status a 0 en lugar de eliminar

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CompraExiste(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        private bool CompraExiste(int id)
        {
            return _context.Compra.Any(e => e.idCompra == id);
        }
    }
}
