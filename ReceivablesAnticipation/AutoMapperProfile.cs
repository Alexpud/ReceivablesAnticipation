using AutoMapper;
using Domain.Entities;
using ReceivablesAnticipation.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReceivablesAnticipation
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Transaction, TransactionDTO>();
            CreateMap<TransactionDTO, Transaction>();

            CreateMap<TransactionAnticipation, TransactionAnticipationDTO>();
            CreateMap<TransactionAnticipationDTO, TransactionAnticipation>();
        }
    }
}
