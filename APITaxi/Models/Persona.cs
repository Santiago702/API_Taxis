namespace APITaxi.Models
{
    public class Persona
    {
        public int IdPersona { get; set; }

        public string Foto { get; set; }

        public string Nombre { get; set; }

        public long NumeroCedula { get; set; }

        public long? Telefono { get; set; }

        public string Correo { get; set; }

        public string Direccion { get; set; }

        public string Ciudad { get; set; }


        public bool Estado { get; set; }

        public string GrupoSanguineo { get; set; }

        public string Eps { get; set; }

        public string Arl { get; set; }

        public string DocumentoCedula { get; set; }

        public string DocumentoEps { get; set; }

        public string DocumentoArl { get; set; }

        public int? IdEmpresa { get; set; }

        public string Contrasena { get; set; }

        public int IdRol { get; set; }
    }
}
