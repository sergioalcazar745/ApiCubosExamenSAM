using ApiCubosExamenSAM.Helpers;
using ApiCubosExamenSAM.Models;
using ApiCubosExamenSAM.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace ApiCubosExamenSAM.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private RepositoryUsuarios repo;
        private HelperOAuthToken helper;
        public AuthController(RepositoryUsuarios repo, HelperOAuthToken helper)
        {
            this.repo = repo;
            this.helper = helper;
        }

        [HttpPost]
        public async Task<ActionResult<string>> Login(LoginModel model)
        {
            Usuario empleado = await this.repo.LoginAsync(model.Username, model.Password);
            if (empleado != null)
            {
                SigningCredentials credentials = new SigningCredentials(this.helper.GetKeyToken(), SecurityAlgorithms.HmacSha256);
                string jsonUsuario = JsonConvert.SerializeObject(empleado);
                Claim[] informacion = new[]
                {
                    new Claim("UserData", jsonUsuario)
                };

                JwtSecurityToken token = new JwtSecurityToken(
                    claims: informacion,
                    issuer: this.helper.Issuer,
                    audience: this.helper.Audience,
                    signingCredentials: credentials,
                    expires: DateTime.UtcNow.AddMinutes(5),
                    notBefore: DateTime.UtcNow
                    );
                return Ok(new { response = new JwtSecurityTokenHandler().WriteToken(token) });
            }
            else
            {
                return BadRequest(new { response = "No existe el usuario" });
            }
        }
    }
}
