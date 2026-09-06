using apicampeonatosfifa.dominio;

namespace apicampeonatosfifa.core
{
    public interface ITipoRepositorio
    {
        Task<IEnumerable<TipoFestivo>> ObtenerTodos();
        Task<TipoFestivo> Obtener(int id);
        Task<TipoFestivo> Agregar(TipoFestivo tipo);
        Task<TipoFestivo> Modificar(TipoFestivo tipo);
        Task<bool> Eliminar(int id);
    }
}