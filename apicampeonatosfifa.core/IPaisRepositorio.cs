using apicampeonatosfifa.dominio;

namespace apicampeonatosfifa.core
{
    public interface IPaisRepositorio
    {
        Task<IEnumerable<Pais>> ObtenerTodos();
        Task<Pais> Obtener(int id);
        Task<Pais> Agregar(Pais pais);
        Task<Pais> Modificar(Pais pais);
        Task<bool> Eliminar(int id);
    }
}