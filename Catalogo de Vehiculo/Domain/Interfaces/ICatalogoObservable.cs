namespace Catalogo_de_Vehiculo.Domain.Interfaces
{
    /// <summary>
    /// Observable: interfaz del sujeto que emite notificaciones.
    /// </summary>
    public interface ICatalogoObservable
    {
        void Suscribir(ICatalogoObserver observer);
        void Desuscribir(ICatalogoObserver observer);
        void Notificar(string mensaje);
    }
}