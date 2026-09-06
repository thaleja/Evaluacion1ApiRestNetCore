using apicampeonatosfifa.dominio;
using apicampeonatosfifa.core;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace apicampeonatosfifa.infraestructura
{
    public class PaisRepositorio : IPaisRepositorio
    {
        private readonly FestivosContext _context;

        public PaisRepositorio(FestivosContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Pais>> ObtenerTodos()
        {
            return await _context.Paises.ToListAsync();
        }

        public async Task<Pais> Obtener(int id)
        {
            return await _context.Paises.FindAsync(id);
        }

        public async Task<Pais> Agregar(Pais pais)
        {
            _context.Paises.Add(pais);
            await _context.SaveChangesAsync();
            return pais;
        }

        public async Task<Pais> Modificar(Pais pais)
        {
            _context.Entry(pais).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return pais;
        }

        public async Task<bool> Eliminar(int id)
        {
            var pais = await _context.Paises.FindAsync(id);
            if (pais == null) return false;

            _context.Paises.Remove(pais);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}