﻿using SmartAgroAPI.Models;

namespace SmartAgroAPI.DataTransferObjects
{
    public class UserDTO
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? RecoveryCode { get; set; }
        public DateTime? CodeExpiration { get; set; }

        public UserDTO()
        {

        }

        public UserDTO(Usuario user)
        {
            Id = user.Id;
            Email = user.Email;
            Phone = user.Telefone;
            Name = user.Nome;
            CodeExpiration = user.ExpiracaoCodigo;
            RecoveryCode = user.CodigoVerificacao;
        }

    }
}
