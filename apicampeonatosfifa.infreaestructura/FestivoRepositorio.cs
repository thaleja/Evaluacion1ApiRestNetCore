using apicampeonatosfifa.dominio;
using apicampeonatosfifa.core;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace apicampeonatosfifa.infraestructura
{
    public class FestivoRepositorio : IFestivoRepositorio
    {
        private readonly FestivosContext _context;

        public FestivoRepositorio(FestivosContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Festivo>> ObtenerTodos()
        {
            return await _context.Festivos
                .Include(f => f.Pais)
                .Include(f => f.Tipo)
                .ToListAsync();
        }

        public async Task<Festivo> Obtener(int id)
        {
            return await _context.Festivos
                .Include(f => f.Pais)
                .Include(f => f.Tipo)
                .FirstOrDefaultAsync(f => f.Id == id);
        }

        public async Task<Festivo> Agregar(Festivo festivo)
        {
            _context.Festivos.Add(festivo);
            await _context.SaveChangesAsync();
            return festivo;
        }

        public async Task<Festivo> Modificar(Festivo festivo)
        {
            _context.Entry(festivo).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return festivo;
        }

        public async Task<bool> Eliminar(int id)
        {
            var festivo = await _context.Festivos.FindAsync(id);
            if (festivo == null) return false;

            _context.Festivos.Remove(festivo);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}