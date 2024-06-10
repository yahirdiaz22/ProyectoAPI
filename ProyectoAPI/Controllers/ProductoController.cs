    using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ProyectoAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductoController : ControllerBase
    {
        private readonly DataContext _context;
        public ProductoController(DataContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Producto>>> MostrarProducto()
        {
            return await _context.Producto.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Producto>> GetProducto(int id)
        {
            var producto = await _context.Producto.FindAsync(id);

            if (producto == null)
            {
                return NotFound();
            }

            return producto;
        }

        [HttpPost]
        public async Task<ActionResult<Producto>> AgregarProducto(string nombre, double precio, int cantidad, bool status)
        {
            int Nuevostatus = status ? 1 : 0;

            var nuevoProducto = new Producto
            {
                nombre = nombre,
                precio = precio,
                cantidad = cantidad,
                status = Nuevostatus,
            };

            _context.Producto.Add(nuevoProducto);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetProducto), new { id = nuevoProducto.idProducto }, nuevoProducto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> ActualizarProducto(int id, string nombre, double precio, int cantidad)
        {
            var producto = await _context.Producto.FindAsync(id);

            if (producto == null)
            {
                return NotFound();
            }

            producto.nombre = nombre;
            producto.precio = precio;
            producto.cantidad = cantidad;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductoExiste(id))
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
        public async Task<IActionResult> EliminarProducto(int id)
        {
            var producto = await _context.Producto.FindAsync(id);
            if (producto == null)
            {
                return NotFound();
            }

            producto.status = 0; // Cambiando el status a 0 en lugar de eliminar

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductoExiste(id))
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

        private bool ProductoExiste(int id)
        {
            return _context.Producto.Any(e => e.idProducto == id);
        }
    }
}
