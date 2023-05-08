using ApiCubosExamenSAM.Models;
using ApiCubosExamenSAM.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ApiCubosExamenSAM.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CubosController : ControllerBase
    {
        RepositoryCubos repo;
        public CubosController(RepositoryCubos repo)
        {
            this.repo = repo;
        }

        [HttpGet]
        public async Task<ActionResult<List<Cubo>>> Cubos()
        {
            return Ok(new { response = await this.repo.GetCubosAsync() });
        }

        [HttpGet]
        [Route("{marca}")]
        public async Task<ActionResult<Cubo>> CubosByMarca(string marca)
        {
            return Ok(new { response = await this.repo.GetCubosByMarcaAsync(marca) });
        }

        [HttpGet]
        [Route("[action]")]
        public async Task<ActionResult<List<string>>> MarcasCubo()
        {
            return Ok(new { response = await this.repo.GetMarcasCubo() });
        }

        [HttpPost]
        public async Task<ActionResult> InsertCubo(CuboModel cubo)
        {
            await this.repo.InsertCuboAsync(cubo.Nombre, cubo.Marca, cubo.Imagen, cubo.Precio);
            return Ok();
        }
    }
}
