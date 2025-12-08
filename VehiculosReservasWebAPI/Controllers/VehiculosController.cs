using AutoMapper;
using Microsoft.AspNetCore.Http;
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
    public class VehiculosController : ControllerBase
    {
        private readonly IService<Vehiculo> _VehiculoService;
        //private readonly IAlquilerRepository _AlquilerRepository2;
        private readonly IVehiculoRepository _VehiculoRepository;
        private readonly IMapper _mapper;
        public VehiculosController(IService<Vehiculo> VehiculoService, IMapper mapper, IVehiculoRepository VehiculoRepository)
        {
            _VehiculoService = VehiculoService;
            _mapper = mapper;
            _VehiculoRepository = VehiculoRepository;
        }

        [HttpGet]
        public async Task<IActionResult> ObtenerListadoVehiculos()
        {
            var vehiculos = await _VehiculoRepository.ListadoVehiculos();
            var dto = _mapper.Map<List<ListaVehiculoDto>>(vehiculos);
            return Ok(dto);
        }
        [HttpPost]
        public async Task<IActionResult>AgregarVehiculo(VehiculoDto NuevoVehiculo)
        {
            var modeloVehiculo = _mapper.Map<Vehiculo>(NuevoVehiculo);
            await _VehiculoService.Agregar(modeloVehiculo);
            return Ok("Vehiculo Agregado Correctamente !");
        }
        [HttpPut]
        public async Task<IActionResult> EditarVehiculo(VehiculoDto ModificaVehiculo)
        {
            var modeloVehiculo = _mapper.Map<Vehiculo>(ModificaVehiculo);
            await _VehiculoService.Editar(modeloVehiculo);
            return Ok("Vehiculo Modificado Correctamente");
        }


    }
}
