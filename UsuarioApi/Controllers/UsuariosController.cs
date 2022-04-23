using Microsoft.AspNetCore.Mvc;
using Models;
using UsuarioApi.Services;

namespace UsuarioApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuariosController : ControllerBase
    {
        private readonly UsuarioService _usuarioService;
        public UsuariosController(UsuarioService usuarioService) => _usuarioService = usuarioService;

        [HttpGet("{username}")]
        public ActionResult<Usuario> Get(string username)
        {
            var usuario = _usuarioService.GetByUsername(username);

            if (usuario == null)
                return NotFound();

            return usuario;
        }

        [HttpPost]
        public ActionResult<Usuario> Create(Usuario usuario)
        {
            var buscarUsuario = _usuarioService.GetByUsername(usuario.Username);

            if (buscarUsuario != null)
                return BadRequest("Usuário já cadastrado");

            _usuarioService.Create(usuario);
            return Ok();
        }
    }
}