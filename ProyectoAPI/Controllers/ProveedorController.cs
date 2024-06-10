using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ProyectoAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProveedorController : ControllerBase
    {
        private readonly DataContext _context;
        public ProveedorController(DataContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Proveedor>>> MostrarProveedor()
        {
            return await _context.Proveedor.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Proveedor>> GetProveedor(int id)
        {
            var proveedor = await _context.Proveedor.FindAsync(id);

            if (proveedor == null)
            {
                return NotFound();
            }

            return proveedor;
        }

        [HttpPost]
        public async Task<ActionResult<Proveedor>> AgregarProveedor(string nombre, string direccion, string correoElectronico, string telefono, bool status)
        {
            int Nuevostatus = status ? 1 : 0;

            var nuevoProveedor = new Proveedor
            {
                nombre = nombre,
                direccion = direccion,
                correoElectronico = correoElectronico,
                telefono = telefono,
                status = Nuevostatus,
            };

            _context.Proveedor.Add(nuevoProveedor);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetProveedor), new { id = nuevoProveedor.idProveedor }, nuevoProveedor);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> ActualizarProveedor(int id, string nombre, string direccion, string correoElectronico, string telefono)
        {
            var proveedor = await _context.Proveedor.FindAsync(id);

            if (proveedor == null)
            {
                return NotFound();
            }

            proveedor.nombre = nombre;
            proveedor.direccion = direccion;
            proveedor.correoElectronico = correoElectronico;
            proveedor.telefono = telefono;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProveedorExiste(id))
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
        public async Task<IActionResult> EliminarProveedor(int id)
        {
            var proveedor = await _context.Proveedor.FindAsync(id);
            if (proveedor == null)
            {
                return NotFound();
            }

            proveedor.status = 0; // Cambiando el status a 0 en lugar de eliminar

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProveedorExiste(id))
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

        private bool ProveedorExiste(int id)
        {
            return _context.Proveedor.Any(e => e.idProveedor == id);
        }
    }
}
