namespace apicampeonatosfifa.dominio
{
    public class Festivo
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public int Dia { get; set; }
        public int Mes { get; set; }
        public int DiasPascua { get; set; }
        public int IdTipo { get; set; }
        public TipoFestivo Tipo { get; set; }
        public int IdPais { get; set; }
        public Pais Pais { get; set; }
    }
}