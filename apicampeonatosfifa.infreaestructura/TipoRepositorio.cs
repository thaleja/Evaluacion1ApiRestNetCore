using apicampeonatosfifa.dominio;
using apicampeonatosfifa.core;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace apicampeonatosfifa.infraestructura
{
    public class TipoRepositorio : ITipoRepositorio
    {
        private readonly FestivosContext _context;

        public TipoRepositorio(FestivosContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<TipoFestivo>> ObtenerTodos()
        {
            return await _context.Tipos.ToListAsync();
        }

        public async Task<TipoFestivo> Obtener(int id)
        {
            return await _context.Tipos.FindAsync(id);
        }

        public async Task<TipoFestivo> Agregar(TipoFestivo tipo)
        {
            _context.Tipos.Add(tipo);
            await _context.SaveChangesAsync();
            return tipo;
        }

        public async Task<TipoFestivo> Modificar(TipoFestivo tipo)
        {
            _context.Entry(tipo).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return tipo;
        }

        public async Task<bool> Eliminar(int id)
        {
            var tipo = await _context.Tipos.FindAsync(id);
            if (tipo == null) return false;

            _context.Tipos.Remove(tipo);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}