namespace VehiculosReservasWebAPI.Repositorio.IRepositorio
{
    public interface IRepository<T> where T : class
    {
        Task<IEnumerable<T>> ObtenerLista();
        Task<T?> ObtenerPorId(int id);
        Task Agregar(T entity);
        Task Editar(T entity);
        Task Eliminar(int id);
    }
}
