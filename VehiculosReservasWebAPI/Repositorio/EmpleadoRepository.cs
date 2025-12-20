using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using VehiculosReservasWebAPI.Models;
using VehiculosReservasWebAPI.Models.Dto.DtoViews;
using VehiculosReservasWebAPI.Repositorio.IRepositorio;

namespace VehiculosReservasWebAPI.Repositorio
{
    public class EmpleadoRepository : IEmpleadoRepository
    {
        private readonly   ReservasCocheraContext _context;
        private string _claveSecreta;
        private readonly IMapper _mapper;
        public  EmpleadoRepository(ReservasCocheraContext context, IConfiguration configuration, IMapper mapper)
        {
            _context = context;
            _claveSecreta = configuration.GetValue<string>("ApiSettings:Secreta");
            _mapper = mapper;
        }
        public async Task<UsuarioLoginRespuestaDto> Login(UsuarioLoginDto usuario)
        {
            //var passwordEncriptada = ObtenerMD5(usuario.Password);
            var usuarioLogueado = _context.Empleados.FirstOrDefault(
                c => c.Usuario.ToLower() == usuario.NombreUsuario.ToLower());

            
            if (usuarioLogueado == null )
             return null; 

            var manejoToken = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_claveSecreta);
            var tokenDescription = new SecurityTokenDescriptor
            {
                Subject = new System.Security.Claims.ClaimsIdentity(new Claim[]
                {
                        new Claim(ClaimTypes.Name,usuarioLogueado.Usuario.ToString()),
                        new Claim(ClaimTypes.Role,usuarioLogueado.Email)
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var Token = manejoToken.CreateToken(tokenDescription);
            UsuarioLoginRespuestaDto response = new UsuarioLoginRespuestaDto()
            {
                Token = manejoToken.WriteToken(Token),
                usuario = _mapper.Map<UsuarioDatosDto>(usuarioLogueado)

            };
            return response;
        }
    }
}
