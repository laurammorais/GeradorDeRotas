using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using GeradorDeRotas.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models;

namespace GeradorDeRotas.Controllers
{
    public class UsuariosController : Controller
    {
        private readonly UsuarioService _usuarioService;
        public UsuariosController(UsuarioService usuarioService)
        {
            _usuarioService = usuarioService;
        }

        public ActionResult Login() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(Usuario usuario)
        {
            try
            {
                var login = await _usuarioService.GetByUsername(usuario.Username);

                if (usuario.LoginValido(login))
                {
                    var claims = new List<Claim>
                    {
                        new Claim("username", usuario.Username),
                        new Claim(ClaimTypes.NameIdentifier, usuario.Username),
                        new Claim(ClaimTypes.Name, login.Nome.Split(" ").First())
                    };
                    var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

                    await HttpContext.SignInAsync(claimsPrincipal);
                    return RedirectToAction("Index", "Home");
                }

                TempData["loginFalha"] = "Falha";
                return View();
            }
            catch
            {
                return View();
            }
        }

        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return RedirectToAction(nameof(Login));
        }

        public ActionResult Registrar() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Registrar(Usuario usuario)
        {
            try
            {
                if (!usuario.Valido)
                {
                    TempData["registroInvalido"] = "Invalido";
                    return View();
                }

                var sucesso = await _usuarioService.Post(usuario);

                if (sucesso)
                {
                    var claims = new List<Claim>
                    {
                        new Claim("username", usuario.Username),
                        new Claim(ClaimTypes.NameIdentifier, usuario.Username),
                        new Claim(ClaimTypes.Name, usuario.Nome.Split(" ").First())
                    };
                    var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

                    await HttpContext.SignInAsync(claimsPrincipal);

                    TempData["registroSucesso"] = "Sucesso";
                    return View();
                }

                TempData["registroFalha"] = "Falha";
                return View();
            }
            catch
            {
                return View();
            }
        }
    }
}