using apicampeonatosfifa.dominio;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace apicampeonatosfifa.core
{
    public interface ICalendarioServicio
    {
        // Métodos de negocio requeridos en la evaluación
        Task<bool> EsFestivo(int idPais, int año, int mes, int dia);
        Task<IEnumerable<object>> ObtenerFestivosDeAño(int idPais, int año);

        // CRUD de Paises (Para cumplir el requisito 1)
        Task<IEnumerable<Pais>> ObtenerTodosPaises();
        Task<Pais> ObtenerPais(int id);
        Task<Pais> AgregarPais(Pais pais);
        Task<Pais> ModificarPais(Pais pais);
        Task<bool> EliminarPais(int id);

        // CRUD de Tipos (Para cumplir el requisito 1)
        Task<IEnumerable<TipoFestivo>> ObtenerTodosTipos();
        Task<TipoFestivo> ObtenerTipo(int id);
        Task<TipoFestivo> AgregarTipo(TipoFestivo tipo);
        Task<TipoFestivo> ModificarTipo(TipoFestivo tipo);
        Task<bool> EliminarTipo(int id);

        // CRUD de Festivos (Para cumplir el requisito 1)
        Task<IEnumerable<Festivo>> ObtenerTodosFestivos();
        Task<Festivo> ObtenerFestivo(int id);
        Task<Festivo> AgregarFestivo(Festivo festivo);
        Task<Festivo> ModificarFestivo(Festivo festivo);
        Task<bool> EliminarFestivo(int id);
    }
}