using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using apicampeonatosfifa.dominio;
using apicampeonatosfifa.core;

namespace apicampeonatosfifa.core.servicios
{
    public class CalendarioServicio : ICalendarioServicio
    {
        private readonly IFestivoRepositorio _festivoRepositorio;
        private readonly IPaisRepositorio _paisRepositorio;
        private readonly ITipoRepositorio _tipoRepositorio;

        // Constructor que inyecta los tres repositorios necesarios [Image 45]
        public CalendarioServicio(
            IFestivoRepositorio festivoRepositorio,
            IPaisRepositorio paisRepositorio,
            ITipoRepositorio tipoRepositorio)
        {
            _festivoRepositorio = festivoRepositorio;
            _paisRepositorio = paisRepositorio;
            _tipoRepositorio = tipoRepositorio;
        }

        // ===================================================================
        // 1. CÁLCULO MATEMÁTICO DEL DOMINGO DE PASCUA (Fórmula de la Evaluación) [Image 4, 5]
        // ===================================================================
        public DateTime CalcularDomingoPascua(int año)
        {
            int a = año % 19;
            int b = año % 4;
            int c = año % 7;
            int d = (19 * a + 24) % 30;
            int dias = d + (2 * b + 4 * c + 6 * d + 5) % 7;

            // El Domingo de Ramos es 15 de marzo + los días calculados [Image 5]
            DateTime domingoRamos = new DateTime(año, 3, 15).AddDays(dias);

            // El Domingo de Pascua es exactamente 7 días después [Image 5]
            return domingoRamos.AddDays(7);
        }

        // ===================================================================
        // 2. TRASLADO AL SIGUIENTE LUNES (Ley Emiliani / Ley de Puente Festivo) [Image 3]
        // ===================================================================
        public DateTime TrasladarAlSiguienteLunes(DateTime fecha)
        {
            if (fecha.DayOfWeek == DayOfWeek.Monday)
            {
                return fecha;
            }
            int diasHastaLunes = ((int)DayOfWeek.Monday - (int)fecha.DayOfWeek + 7) % 7;
            if (diasHastaLunes == 0) diasHastaLunes = 7;

            return fecha.AddDays(diasHastaLunes);
        }

        // ===================================================================
        // 3. MÉTODOS WEB PRINCIPALES EXIGIDOS POR EL EXAMEN [Image 5, 6]
        // ===================================================================

        // Requisito 2: Validar si una fecha es festiva en un país [Image 5]
        public async Task<bool> EsFestivo(int idPais, int año, int mes, int dia)
        {
            if (!EsFechaValida(año, mes, dia))
            {
                throw new ArgumentException("Fecha No valida");
            }

            DateTime fecha = new DateTime(año, mes, dia);
            var festivos = await _festivoRepositorio.ObtenerTodos();
            var festivosPais = festivos.Where(f => f.IdPais == idPais);
            DateTime pascua = CalcularDomingoPascua(año);

            foreach (var festivo in festivosPais)
            {
                DateTime fechaFestivo;

                switch (festivo.IdTipo)
                {
                    case 1: // Fijo (Año nuevo, Día del Trabajo, etc.) [Image 2, 3]
                        fechaFestivo = new DateTime(año, festivo.Mes, festivo.Dia);
                        break;

                    case 2: // Ley Puente Festivo (Santos Reyes, San José, etc.) [Image 2, 3]
                        fechaFestivo = new DateTime(año, festivo.Mes, festivo.Dia);
                        fechaFestivo = TrasladarAlSiguienteLunes(fechaFestivo);
                        break;

                    case 3: // Basado en Pascua (Jueves Santo, Viernes Santo) [Image 2, 3]
                        fechaFestivo = pascua.AddDays(festivo.DiasPascua);
                        break;

                    case 4: // Basado en Pascua y Ley Puente (Corpus Christi, etc.) [Image 2, 3]
                        fechaFestivo = pascua.AddDays(festivo.DiasPascua);
                        fechaFestivo = TrasladarAlSiguienteLunes(fechaFestivo);
                        break;

                    default:
                        continue;
                }

                if (fechaFestivo.Date == fecha.Date)
                {
                    return true;
                }
            }

            return false;
        }

        // Requisito 3: Obtener lista de festivos de un año [Image 6]
        public async Task<IEnumerable<object>> ObtenerFestivosDeAño(int idPais, int año)
        {
            var festivos = await _festivoRepositorio.ObtenerTodos();
            var festivosPais = festivos.Where(f => f.IdPais == idPais);
            DateTime pascua = CalcularDomingoPascua(año);
            var lista = new List<object>();

            foreach (var festivo in festivosPais)
            {
                DateTime fechaFestivo;

                switch (festivo.IdTipo)
                {
                    case 1:
                        fechaFestivo = new DateTime(año, festivo.Mes, festivo.Dia);
                        break;

                    case 2:
                        fechaFestivo = new DateTime(año, festivo.Mes, festivo.Dia);
                        fechaFestivo = TrasladarAlSiguienteLunes(fechaFestivo);
                        break;

                    case 3:
                        fechaFestivo = pascua.AddDays(festivo.DiasPascua);
                        break;

                    case 4:
                        fechaFestivo = pascua.AddDays(festivo.DiasPascua);
                        fechaFestivo = TrasladarAlSiguienteLunes(fechaFestivo);
                        break;

                    default:
                        continue;
                }

                lista.Add(new
                {
                    festivo = festivo.Nombre,
                    fecha = fechaFestivo.ToString("yyyy-MM-dd")
                });
            }

            return lista.OrderBy(l => ((dynamic)l).fecha);
        }

        // ===================================================================
        // 4. MÉTODOS CRUD REQUERIDOS POR LA INTERFAZ DEL DOCENTE [Image 5, 45]
        // ===================================================================

        // --- CRUD PAIS ---
        public async Task<IEnumerable<Pais>> ObtenerTodosPaises()
        {
            return await _paisRepositorio.ObtenerTodos();
        }

        public async Task<Pais> ObtenerPais(int id)
        {
            return await _paisRepositorio.Obtener(id);
        }

        public async Task<Pais> AgregarPais(Pais pais)
        {
            return await _paisRepositorio.Agregar(pais);
        }

        public async Task<Pais> ModificarPais(Pais pais)
        {
            return await _paisRepositorio.Modificar(pais);
        }

        public async Task<bool> EliminarPais(int id)
        {
            return await _paisRepositorio.Eliminar(id);
        }

        // --- CRUD TIPO (TipoFestivo) --- [Image 45]
        public async Task<IEnumerable<TipoFestivo>> ObtenerTodosTipos()
        {
            return await _tipoRepositorio.ObtenerTodos();
        }

        public async Task<TipoFestivo> ObtenerTipo(int id)
        {
            return await _tipoRepositorio.Obtener(id);
        }

        public async Task<TipoFestivo> AgregarTipo(TipoFestivo tipo)
        {
            return await _tipoRepositorio.Agregar(tipo);
        }

        public async Task<TipoFestivo> ModificarTipo(TipoFestivo tipo)
        {
            return await _tipoRepositorio.Modificar(tipo);
        }

        public async Task<bool> EliminarTipo(int id)
        {
            return await _tipoRepositorio.Eliminar(id);
        }

        // --- CRUD FESTIVO --- [Image 45]
        public async Task<IEnumerable<Festivo>> ObtenerTodosFestivos()
        {
            return await _festivoRepositorio.ObtenerTodos();
        }

        public async Task<Festivo> ObtenerFestivo(int id)
        {
            return await _festivoRepositorio.Obtener(id);
        }

        public async Task<Festivo> AgregarFestivo(Festivo festivo)
        {
            return await _festivoRepositorio.Agregar(festivo);
        }

        public async Task<bool> EliminarFestivo(int id)
        {
            return await _festivoRepositorio.Eliminar(id);
        }

        public async Task<Festivo> ModificarFestivo(Festivo festivo)
        {
            return await _festivoRepositorio.Modificar(festivo);
        }

        // ===================================================================
        // 5. VALIDACIÓN AUXILIAR DE FECHAS
        // ===================================================================
        public bool EsFechaValida(int año, int mes, int dia)
        {
            try
            {
                new DateTime(año, mes, dia);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}