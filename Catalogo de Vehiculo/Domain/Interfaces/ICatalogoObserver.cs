namespace Catalogo_de_Vehiculo.Domain.Interfaces
{
    /// <summary>
    /// Observer: interfaz que deben implementar los que quieran 
    /// recibir notificaciones de cambios en el catálogo.
    /// </summary>
    public interface ICatalogoObserver
    {
        void OnCatalogoActualizado(string mensaje);
    }
}