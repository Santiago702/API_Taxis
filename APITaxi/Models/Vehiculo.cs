﻿namespace APITaxi.Models
{
    public class Vehiculo
    {
        public int IdVehiculo { get; set; }

        public string Placa { get; set; }

        public bool Estado { get; set; }

        public string Soat { get; set; }

        public string TecnicoMecanica { get; set; }

        public int IdPersonaProp { get; set; }

        public int IdEmpresa { get; set; }
        public DateTime VenceSoat { get; set; }
        public DateTime VenceTecnicoMecanica { get; set; }
    }
}
