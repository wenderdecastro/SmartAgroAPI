﻿using SmartAgroAPI.Models;

namespace SmartAgroAPI.DataTransferObjects
{
    public class LogsSensorDTO
    {

        public LogsSensorDTO()
        {

        }

        public LogsSensorDTO(LogsSensor log)
        {
            this.TemperaturaSolo = log.TemperaturaSolo;
            this.PhSolo = log.PhSolo;
            this.TemperaturaAr = log.TemperaturaAr;
            this.Luminosidade = log.Luminosidade;
            this.UmidadeSolo = log.UmidadeSolo;
            this.DataAtualizacao = log.DataAtualizacao;
        }

        public decimal? UmidadeSolo { get; set; }

        public decimal? TemperaturaAr { get; set; }

        public decimal? UmidadeAr { get; set; }

        public decimal? Luminosidade { get; set; }

        public decimal? TemperaturaSolo { get; set; }

        public decimal? QualidadeAr { get; set; }

        public decimal? PhSolo { get; set; }

        public DateTime? DataAtualizacao { get; set; }

    }


}
