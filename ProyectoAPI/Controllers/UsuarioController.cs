// Controllers/TodoController.cs
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProyectoAPI;
using static ProyectoAPI.DataContext;
using Microsoft.AspNetCore.Mvc.Abstractions;
using System;

[ApiController]
[Route("api/[controller]/[Action]")]
public class UsuarioController : ControllerBase
{

    private readonly DataContext _context;

    public UsuarioController(DataContext context)
    {
        _context = context;
    }

    // GET: api/Todo
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Usuario>>> GetTodasLosLogin()
    {
        return await _context.Usuario.ToListAsync();
    }

    // GET: api
    [HttpGet("{id}")]
    public async Task<ActionResult<Usuario>>
        GetLoginPorId(int id)
    {
        var Login = await _context.Usuario.FindAsync(id);

        if (Login == null)
        {
            return NotFound();
        }

        return Login;
    }
    [HttpPost]
    public async Task<ActionResult<long>> CrearLogin(string nombre,string correoElectronico, string password, bool status)
    {
        int estadostatus = status ? 1 : 0;

        var NuevoUsuario = new Usuario
        {
            nombre = nombre,
            correoElectronico = correoElectronico,
            password = password,
            status = estadostatus,
        };

        _context.Usuario.Add(NuevoUsuario);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetLoginPorId), new { id = NuevoUsuario.idUsuario }, NuevoUsuario);
    }


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

    // DELETE: api/Cliente/DeleteCliente/5
    [HttpDelete("Delete/{id}")]
    public async Task<IActionResult> EliminarUsuario(int id)
    {
        var categoria = await _context.Usuario.FindAsync(id);
        if (categoria == null)
        {
            return NotFound();
        }

        categoria.status = 0; // Cambiando el status a 0 en lugar de eliminar

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

    // POST: api/Usuario/VerificarExistencia
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


}