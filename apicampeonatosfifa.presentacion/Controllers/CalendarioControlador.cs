using Microsoft.AspNetCore.Mvc;
using apicampeonatosfifa.dominio;
using apicampeonatosfifa.core;
using System;
using System.Threading.Tasks;

namespace presentacion.Controllers
{
    [ApiController]
    [Route("api/calendario")]
    public class CalendarioControlador : ControllerBase
    {
        private readonly ICalendarioServicio _servicio;

        public CalendarioControlador(ICalendarioServicio servicio)
        {
            _servicio = servicio;
        }

        // 1. VERIFICAR SI UNA FECHA ES FESTIVA (Requisito 2 de la evaluación)
        // Ruta: /api/calendario/verificar/{idPais}/{año}/{mes}/{dia}
        [HttpGet("verificar/{idPais}/{año}/{mes}/{dia}")]
        public async Task<IActionResult> VerificarFecha(int idPais, int año, int mes, int dia)
        {
            // Validamos que el mes y día sean válidos para el año ingresado
            if (mes < 1 || mes > 12 || dia < 1 || dia > DateTime.DaysInMonth(año, mes))
            {
                return Ok("Fecha No valida");
            }

            try
            {
                bool resultado = await _servicio.EsFestivo(idPais, año, mes, dia);
                return Ok(resultado ? "Es Festivo" : "No es festivo");
            }
            catch (Exception)
            {
                return Ok("Fecha No valida");
            }
        }

        // 2. LISTAR LOS FESTIVOS DE UN AÑO (Requisito 3 de la evaluación)
        // Ruta: /api/calendario/festivos/{idPais}/{año}
        [HttpGet("festivos/{idPais}/{año}")]
        public async Task<IActionResult> ObtenerFestivos(int idPais, int año)
        {
            try
            {
                var listado = await _servicio.ObtenerFestivosDeAño(idPais, año);
                return Ok(listado);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // 3. ENDPOINTS PARA EL CRUD DE PAÍSES (Requisito 1 de la evaluación)
        [HttpGet("paises")]
        public async Task<IActionResult> ObtenerPaises() => Ok(await _servicio.ObtenerTodosPaises());

        [HttpGet("paises/{id}")]
        public async Task<IActionResult> ObtenerPais(int id) => Ok(await _servicio.ObtenerPais(id));

        [HttpPost("paises")]
        public async Task<IActionResult> CrearPais([FromBody] Pais pais) => Ok(await _servicio.AgregarPais(pais));

        [HttpPut("paises")]
        public async Task<IActionResult> ModificarPais([FromBody] Pais pais) => Ok(await _servicio.ModificarPais(pais));

        [HttpDelete("paises/{id}")]
        public async Task<IActionResult> EliminarPais(int id) => Ok(await _servicio.EliminarPais(id));
    }
}