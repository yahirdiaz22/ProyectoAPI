using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ProyectoAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VentaController : ControllerBase
    {
        private readonly DataContext _context;
        public VentaController(DataContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Venta>>> MostrarVenta()
        {
            return await _context.Venta.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Venta>> GetVenta(int id)
        {
            var venta = await _context.Venta.FindAsync(id);

            if (venta == null)
            {
                return NotFound();
            }

            return venta;
        }

        [HttpPost]
        public async Task<ActionResult<Venta>> AgregarVenta(int cantidadVendida, string fecha,string nombre, bool status)
        {
            int Nuevostatus = status ? 1 : 0;

            var nuevaVenta = new Venta
            {
                cantidadVendida = cantidadVendida,
                fecha = fecha,
                nombre= nombre,
                status = Nuevostatus,
            };

            _context.Venta.Add(nuevaVenta);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetVenta), new { id = nuevaVenta.idVenta }, nuevaVenta);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> ActualizarVenta(int id, int cantidadVendida, string fecha, string nombre)
        {
            var venta = await _context.Venta.FindAsync(id);

            if (venta == null)
            {
                return NotFound();
            }

            venta.cantidadVendida = cantidadVendida;
            venta.fecha = fecha;
            venta.nombre= nombre;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!VentaExiste(id))
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
        public async Task<IActionResult> EliminarVenta(int id)
        {
            var venta = await _context.Venta.FindAsync(id);
            if (venta == null)
            {
                return NotFound();
            }

            venta.status = 0; // Cambiando el status a 0 en lugar de eliminar

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!VentaExiste(id))
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

        private bool VentaExiste(int id)
        {
            return _context.Venta.Any(e => e.idVenta == id);
        }
    }
}
