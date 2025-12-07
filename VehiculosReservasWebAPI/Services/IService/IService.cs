namespace VehiculosReservasWebAPI.Services.IService
{
    public interface IService<T> where T : class
    {
        Task<IEnumerable<T>> ObtenerLista();
        Task<T?> ObtenerPorId(int id);
        Task Agregar(T entity);
        Task Editar(T entity);
        Task Eliminar(int id);
    }
}
