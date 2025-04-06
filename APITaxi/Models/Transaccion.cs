namespace APITaxi.Models
{
    public class Transaccion
    {
        public int IdTransaccion { get; set; }
        public int IdPersona { get; set; }
        public string Accion { get; set; }
        public string Modelo { get; set; }

        public DateTime Fecha { get; set; }
        public TimeSpan Hora { get; set; }
    }
}
