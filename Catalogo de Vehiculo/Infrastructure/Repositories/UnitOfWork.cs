using Catalogo_de_Vehiculo.Domain.Entities;
using Catalogo_de_Vehiculo.Domain.Interfaces;

namespace Catalogo_de_Vehiculo.Infrastructure.Repositories
{
    /// <summary>
    /// Implementación concreta del Unit of Work.
    /// Centraliza el acceso a todos los repositorios con una sola connection string.
    /// </summary>
    public class UnitOfWork : IUnitOfWork
    {
        private readonly string _connectionString;

        private IVehiculoRepository<Vehiculo>? _vehiculos;
        private IAutomovilRepository? _automoviles;
        private ICamionRepository? _camiones;
        private IMotocicletaRepository? _motocicletas;

        public UnitOfWork(string connectionString)
        {
            _connectionString = connectionString;
        }

        public IVehiculoRepository<Vehiculo> Vehiculos =>
            _vehiculos ??= new VehiculoRepository(_connectionString);

        public IAutomovilRepository Automoviles =>
            _automoviles ??= new AutomovilRepository(_connectionString);

        public ICamionRepository Camiones =>
            _camiones ??= new CamionRepository(_connectionString);

        public IMotocicletaRepository Motocicletas =>
            _motocicletas ??= new MotocicletaRepository(_connectionString);

        public void Commit() { }

        public void Dispose() { }
    }
}