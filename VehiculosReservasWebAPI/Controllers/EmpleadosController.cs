using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using VehiculosReservasWebAPI.Models;
using VehiculosReservasWebAPI.Models.Dto.DtoAbm;
using VehiculosReservasWebAPI.Models.Dto.DtoViews;
using VehiculosReservasWebAPI.Repositorio.IRepositorio;
using VehiculosReservasWebAPI.Services.IService;

namespace VehiculosReservasWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmpleadosController : ControllerBase
    {
        private readonly IService<Empleado> _EmpleadoService;
        private readonly IEmpleadoRepository _EmpleadoRepository;
        private readonly IVehiculoRepository _VehiculoRepository;
        private readonly IMapper _mapper;
        public EmpleadosController(IService<Empleado> EmpleadoService, IMapper mapper, IEmpleadoRepository EmpleadoRepository)
        {
            _EmpleadoService = EmpleadoService;
            _mapper = mapper;
            _EmpleadoRepository = EmpleadoRepository;
        }
        [HttpPost]
        public async Task<IActionResult> Login(UsuarioLoginDto log)
        {
            var usuarioLogueado = _EmpleadoRepository.Login(log);
           
            return Ok(usuarioLogueado);
        }
    }
}
