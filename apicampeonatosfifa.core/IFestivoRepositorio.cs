using apicampeonatosfifa.dominio;

namespace apicampeonatosfifa.core
{
    public interface IFestivoRepositorio
    {
        Task<IEnumerable<Festivo>> ObtenerTodos();
        Task<Festivo> Obtener(int id);
        Task<Festivo> Agregar(Festivo festivo);
        Task<Festivo> Modificar(Festivo festivo);
        Task<bool> Eliminar(int id);
    }
}
