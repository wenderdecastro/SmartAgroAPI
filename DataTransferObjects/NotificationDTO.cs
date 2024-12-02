﻿using SmartAgroAPI.Models;

namespace SmartAgroAPI.DataTransferObjects
{
    public class NotificationDTO
    {

        public NotificationDTO(Notificacao notificacao)
        {
            Id = notificacao.Id;
            Mensagem = notificacao.Mensagem;
            LogsSensorId = notificacao.LogsSensorId;
            Propriedade = notificacao.Propriedade;
            UsuarioId = notificacao.UsuarioId;
            TipoNotificacaoId = notificacao.TipoNotificacaoId;
            DataCriacao = notificacao.DataCriacao;
        }
        public int Id { get; set; }
        public string? Mensagem { get; set; }

        public string? Propriedade { get; set; }

        public int LogsSensorId { get; set; }


        public Guid UsuarioId { get; set; }

        public int TipoNotificacaoId { get; set; }

        public DateTime? DataCriacao { get; set; } = DateTime.Now;
    }
}
