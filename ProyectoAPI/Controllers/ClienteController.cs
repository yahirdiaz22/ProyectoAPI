using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ProyectoAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClienteController : ControllerBase
    {
        private readonly DataContext _context;
        public ClienteController(DataContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Cliente>>> MostrarCliente()
        {
            return await _context.Cliente.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Cliente>> GetCliente(int id)
        {
            var cliente = await _context.Cliente.FindAsync(id);

            if (cliente == null)
            {
                return NotFound();
            }

            return cliente;
        }

        [HttpPost]
        public async Task<ActionResult<Cliente>> AgregarCliente(string nombre, string direccion, string correoElectronico, string telefono, bool status)
        {
            int Nuevostatus = status ? 1 : 0;

            var nuevoCliente = new Cliente
            {
                nombre = nombre,
                direccion = direccion,
                correoElectronico = correoElectronico,
                telefono = telefono,
                status = Nuevostatus,
            };

            _context.Cliente.Add(nuevoCliente);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetCliente), new { id = nuevoCliente.idCliente }, nuevoCliente);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> ActualizarCliente(int id, string nombre, string direccion, string correoElectronico, string telefono)
        {
            var cliente = await _context.Cliente.FindAsync(id);

            if (cliente == null)
            {
                return NotFound();
            }

            cliente.nombre = nombre;
            cliente.direccion = direccion;
            cliente.correoElectronico = correoElectronico;
            cliente.telefono = telefono;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ClienteExiste(id))
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
        public async Task<IActionResult> EliminarCliente(int id)
        {
            var cliente = await _context.Cliente.FindAsync(id);
            if (cliente == null)
            {
                return NotFound();
            }

            cliente.status = 0; // Cambiando el status a 0 en lugar de eliminar

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ClienteExiste(id))
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

        private bool ClienteExiste(int id)
        {
            return _context.Cliente.Any(e => e.idCliente == id);
        }
    }
}
