using VehiculosReservasWebAPI.Repositorio.IRepositorio;
using VehiculosReservasWebAPI.Services.IService;

namespace VehiculosReservasWebAPI.Services
{
    public class Service<T> : IService<T> where T : class
    {
        private readonly IRepository<T> _repository;

        public Service(IRepository<T> repository)
        {
            _repository = repository;
        }

        public async Task Agregar(T entity)
        {
            await _repository.Agregar(entity);
        }

        public async Task Editar(T entity)
        {
            await _repository.Editar(entity);
        }

        public async Task Eliminar(int id)
        {
            await _repository.Eliminar(id);
        }

        public async Task<IEnumerable<T>> ObtenerLista()
        {
            return await _repository.ObtenerLista();
        }

        public async Task<T?> ObtenerPorId(int id)
        {
            return await _repository.ObtenerPorId(id);
        }
    }
}
