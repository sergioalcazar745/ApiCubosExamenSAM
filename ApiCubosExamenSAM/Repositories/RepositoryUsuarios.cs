using ApiCubosExamenSAM.Data;
using ApiCubosExamenSAM.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace ApiCubosExamenSAM.Repositories
{
    [Authorize]
    public class RepositoryUsuarios
    {
        private CubosContext context;
        public RepositoryUsuarios(CubosContext context)
        {
            this.context = context;
        }

        public async Task<Usuario> LoginAsync(string username, string password)
        {
            return this.context.Usuarios.FirstOrDefault(x => x.Email == username && x.Password == password);
        }

        public async Task<int> MaxIdUsuarioAsync()
        {
            if(this.context.Usuarios.Count() == 0)
            {
                return 1;
            }
            else
            {
                return this.context.Usuarios.Max(x => x.IdUsuario) + 1;
            }
        }

        public async Task<int> MaxIdPedidoAsync()
        {
            if (this.context.CompraCubos.Count() == 0)
            {
                return 1;
            }
            else
            {
                return this.context.CompraCubos.Max(x => x.IdPedido) + 1;
            }
        }

        [AllowAnonymous]
        public async Task CrearUsuarioAsync(string nombre, string email, string password, string imagen)
        {
            Usuario usuario = new Usuario
            {
                IdUsuario = await this.MaxIdUsuarioAsync(),
                Nombre = nombre,
                Email = email,
                Password = password,
                Imagen = imagen
            };

            await this.context.Usuarios.AddAsync(usuario);
            await this.context.SaveChangesAsync();
        }

        public async Task<List<CompraCubo>> PedidosUsuarioAsync(int idusuario)
        {
            var response = from datos in this.context.CompraCubos
                           where datos.IdUsuario == idusuario
                           select datos;

            return await response.ToListAsync();                            
        }

        public async Task InsertPedidoAsync(int idusuario, int idcubo, DateTime fechapedido)
        {
            CompraCubo compra = new CompraCubo
            {
                IdPedido = await this.MaxIdPedidoAsync(),
                IdCubo = idcubo,
                IdUsuario = idusuario,
                FechaPedido = fechapedido
            };

            await this.context.AddAsync(compra);
            await this.context.SaveChangesAsync();
        }
    }
}
