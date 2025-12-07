using Microsoft.EntityFrameworkCore;
using VehiculosReservasWebAPI.Models;
using VehiculosReservasWebAPI.Repositorio.IRepositorio;

namespace VehiculosReservasWebAPI.Repositorio
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly ReservasCocheraContext _context;
        private readonly DbSet<T> _dbSet;
        public Repository(ReservasCocheraContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }
        public async Task Agregar(T entity)
        {
            _dbSet.Add(entity);
            await _context.SaveChangesAsync();
        }

        public async Task Editar(T entity)
        {
            // Obtener la metadata de la entidad
            var entityType = _context.Model.FindEntityType(typeof(T));

            // Obtener la propiedad de la clave primaria
            var key = entityType.FindPrimaryKey().Properties.First();

            // Obtener el valor de la clave de la entidad pasada
            var keyValue = key.PropertyInfo.GetValue(entity);

            // Buscar la entidad existente en la DB
            var existing = await _dbSet.FindAsync(keyValue);
            if (existing == null)
                throw new Exception("Entidad no existe");
            //_dbSet.Update(entity);
            _context.Entry(existing).CurrentValues.SetValues(entity);
            await _context.SaveChangesAsync();
        }

        public async Task Eliminar(int id)
        {
            var existing = await _dbSet.FindAsync(id);
            if (existing == null)
                throw new Exception("Entidad no existe");

            var habilitadoProp = typeof(T).GetProperty("Habilitado");
            if (habilitadoProp != null)
                habilitadoProp.SetValue(existing, false);
            if (existing is Alquiler alquiler)
            {
                var fechaEntregaProp = typeof(T).GetProperty("FechaEntrega");
                if (fechaEntregaProp != null)
                {
                    fechaEntregaProp.SetValue(existing, DateTime.Now);

                    var vehiculo = new Vehiculo { IdVehiculo = alquiler.IdVehiculo, IdEstado = 1 };
                    _context.Vehiculos.Attach(vehiculo);
                    _context.Entry(vehiculo).Property(v => v.IdEstado).IsModified = true;
                }
            }

            var finalizadoProp = typeof(T).GetProperty("Finalizado");
            if (finalizadoProp != null)
                finalizadoProp.SetValue(existing, true);

            await _context.SaveChangesAsync();

        }

        public async Task<IEnumerable<T>> ObtenerLista()
        {
            return await _dbSet.Where(x => EF.Property<bool>(x, "Habilitado") == true).ToListAsync();
        }

        public async Task<T?> ObtenerPorId(int id)
        {
            return await _dbSet.FindAsync(id);
        }
    }
}
