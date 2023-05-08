using ApiCubosExamenSAM.Models;
using ApiCubosExamenSAM.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Security.Claims;

namespace ApiCubosExamenSAM.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UsuariosController : ControllerBase
    {
        RepositoryUsuarios repo;
        public UsuariosController(RepositoryUsuarios repo)
        {
            this.repo = repo;
        }

        [HttpGet]
        [Route("[action]")]
        public async Task<ActionResult<Usuario>> Perfil()
        {
            Claim claim = HttpContext.User.Claims.SingleOrDefault(x => x.Type == "UserData");
            if (claim != null)
            {
                string jsonUsuario = claim.Value;
                Usuario empleado = JsonConvert.DeserializeObject<Usuario>(jsonUsuario);
                return Ok(new { response = empleado });
            }
            else
            {
                return BadRequest(new { response = "No existe el usuario" });
            }
        }

        [HttpGet]
        [Route("[action]")]
        public async Task<ActionResult<List<CompraCubo>>> PedidosUsuario()
        {
            Claim claim = HttpContext.User.Claims.SingleOrDefault(x => x.Type == "UserData");
            string jsonUsuario = claim.Value;
            Usuario usuario = JsonConvert.DeserializeObject<Usuario>(jsonUsuario);
            return Ok(new {response = await this.repo.PedidosUsuarioAsync(usuario.IdUsuario)});
        }

        [HttpPost]
        [Route("[action]")]
        public async Task<ActionResult> InsertPedido(CompraCuboModel model)
        {
            Claim claim = HttpContext.User.Claims.SingleOrDefault(x => x.Type == "UserData");
            string jsonUsuario = claim.Value;
            Usuario usuario = JsonConvert.DeserializeObject<Usuario>(jsonUsuario);
            await this.repo.InsertPedidoAsync(usuario.IdUsuario, model.IdCubo, model.FechaPedido);
            return Ok();
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("[action]")]
        public async Task<ActionResult> InsertUsuario(UsuarioModel model)
        {
            await this.repo.CrearUsuarioAsync(model.Nombre, model.Email, model.Password, model.Imagen);
            return Ok();
        }
    }
}
