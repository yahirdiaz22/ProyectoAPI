using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProyectoAPI;
using static ProyectoAPI.DataContext;

[ApiController]
[Route("api/[controller]/[Action]")]
public class UsuarioController : ControllerBase
{
    private readonly DataContext _context;

    public UsuarioController(DataContext context)
    {
        _context = context;
    }

    // GET: api/Usuario
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Usuario>>> GetTodasLosLogin()
    {
        return await _context.Usuario.ToListAsync();
    }

    // GET: api/Usuario/{id}
    [HttpGet("{id}")]
    public async Task<ActionResult<Usuario>> GetLoginPorId(int id)
    {
        var login = await _context.Usuario.FindAsync(id);

        if (login == null)
        {
            return NotFound();
        }

        return login;
    }

    // POST: api/Usuario/CrearLogin
    [HttpPost]
    public async Task<ActionResult<long>> CrearLogin(string nombre, string correoElectronico, string password, bool status)
    {
        // Verificar si el correo electrónico ya está en uso
        var correoExistente = await VerificarCorreoExistente(correoElectronico);
        if (correoExistente.Value)
        {
            return Conflict("Error: El correo electrónico ya está en uso.");
        }

        int estadostatus = status ? 1 : 0;

        var nuevoUsuario = new Usuario
        {
            nombre = nombre,
            correoElectronico = correoElectronico,
            password = password,
            status = estadostatus,
        };

        _context.Usuario.Add(nuevoUsuario);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetLoginPorId), new { id = nuevoUsuario.idUsuario }, nuevoUsuario);
    }

    // PUT: api/Usuario/ActualizarLogin/{id}
    [HttpPut("{id}")]
    public async Task<IActionResult> ActualizarLogin(int id, string nombre, string correoElectronico)
    {
        var usuario = await _context.Usuario.FindAsync(id);

        if (usuario == null)
        {
            return NotFound();
        }

        usuario.nombre = nombre;
        usuario.correoElectronico = correoElectronico;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!UsuarioExiste(id))
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

    // DELETE: api/Usuario/Delete/{id}
    [HttpDelete("Delete/{id}")]
    public async Task<IActionResult> EliminarUsuario(int id)
    {
        var usuario = await _context.Usuario.FindAsync(id);
        if (usuario == null)
        {
            return NotFound();
        }

        usuario.status = 0; // Cambiando el status a 0 en lugar de eliminar

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!UsuarioExiste(id))
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

    private bool UsuarioExiste(int id)
    {
        return _context.Usuario.Any(e => e.idUsuario == id);
    }

    // GET: api/Usuario/VerificarExistencia
    [HttpGet("VerificarExistencia")]
    public async Task<ActionResult<string>> VerificarExistenciaUsuario(string correoElectronico, string password)
    {
        var usuario = await _context.Usuario.FirstOrDefaultAsync(u => u.correoElectronico == correoElectronico && u.password == password);

        if (usuario != null)
        {
            // El usuario existe en la base de datos
            return Ok("El usuario existe.");
        }

        // El usuario no existe en la base de datos o la contraseña es incorrecta
        return NotFound("El usuario no existe o la contraseña es incorrecta.");
    }

    // GET: api/Usuario/VerificarCorreoExistente
    [HttpGet("VerificarCorreoExistente")]
    public async Task<ActionResult<bool>> VerificarCorreoExistente(string correoElectronico)
    {
        var usuarioExistente = await _context.Usuario.FirstOrDefaultAsync(u => u.correoElectronico == correoElectronico);
        return usuarioExistente != null;
    }
}
