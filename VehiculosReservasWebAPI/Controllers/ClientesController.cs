using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VehiculosReservasWebAPI.Models;
using VehiculosReservasWebAPI.Models.Dto.DtoAbm;
using VehiculosReservasWebAPI.Models.Dto.DtoViews;
using VehiculosReservasWebAPI.Services.IService;

namespace VehiculosReservasWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientesController : ControllerBase
    {
        private readonly IService<Cliente> _clienteService;
        private readonly IMapper _mapper;
        public ClientesController(IService<Cliente> clienteService, IMapper mapper)
        {
            _clienteService = clienteService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> ObtenerListadoClientes()
        {
            var clientes = await _clienteService.ObtenerLista();
            var dto = _mapper.Map<List<ClienteDto>>(clientes);
            return Ok(dto);
        }
        [HttpPost]
        public async Task<IActionResult> AgregarCliente(ClienteDto NuevoCliente)
        {
            var modeloCli = _mapper.Map<Cliente>(NuevoCliente);
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
 
            await _clienteService.Agregar(modeloCli);
            return Ok("Cliente Creado Correctamente");
        }
        [HttpPut]
        public async Task<IActionResult> EditarCliente(ClienteDto ClienteModificado)
        {
            var modeloCli = _mapper.Map<Cliente>(ClienteModificado);
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            await _clienteService.Editar(modeloCli);
            return Ok("Cliente Modificado correctamente!");
        }
      [HttpDelete]
        public async Task<IActionResult> EliminarCliente(int id)
        {
            await _clienteService.Eliminar(id);
            return Ok("Cliente Eliminado Correctamente!");
        }  
        [HttpGet("ObtenerClientePorId")]
        public async Task<IActionResult> ObtenerClientePorId(int id)
        {
            var cliModelo = await _clienteService.ObtenerPorId(id);
            var dto = _mapper.Map<ClienteDto>(cliModelo);
            return Ok(dto);

        }
    }
}
