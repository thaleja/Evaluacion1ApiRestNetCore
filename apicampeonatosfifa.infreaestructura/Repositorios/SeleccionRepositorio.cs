using apicampeonatosfifa.core;
using apicampeonatosfifa.core.repositorios;
using apicampeonatosfifa.dominio;
using apicampeonatosfifa.infraestructura;
using apicampeonatosfifa.infraestructura.Persistencia;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace apicampeonatosfifa.infraestructura.Repositorios
{
    public class SeleccionRepositorio : ISeleccionRepositorio
    {
        private readonly CampeonatosFIFAContext _context;

        // Inyectamos el contexto de la base de datos del profesor
        public SeleccionRepositorio(CampeonatosFIFAContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Seleccion>> ObtenerTodos()
        {
            return await _context.Selecciones.ToListAsync();
        }

        public async Task<Seleccion> Obtener(int Id)
        {
            return await _context.Selecciones.FindAsync(Id);
        }

        public async Task<IEnumerable<Seleccion>> Buscar(int IndiceDato, string Texto)
        {
            // Búsqueda simple por el nombre de la selección
            return await _context.Selecciones
                .Where(s => s.Nombre.Contains(Texto))
                .ToListAsync();
        }

        public async Task<Seleccion> Agregar(Seleccion seleccion)
        {
            _context.Selecciones.Add(seleccion);
            await _context.SaveChangesAsync();
            return seleccion;
        }

        public async Task<Seleccion> Modificar(Seleccion seleccion)
        {
            _context.Entry(seleccion).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return seleccion;
        }

        public async Task<bool> Eliminar(int Id)
        {
            var seleccion = await _context.Selecciones.FindAsync(Id);
            if (seleccion == null) return false;

            _context.Selecciones.Remove(seleccion);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}