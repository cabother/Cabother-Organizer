using System;
using System.Collections.Generic;
using Cabother.Organizer.Domain.Events;

namespace Cabother.Organizer.Domain.Entities
{
    /// <summary>
    /// Classe base para as entidades
    /// </summary>
    /// <typeparam name="TId">Tipo utilização para chave primária da tabela</typeparam>
    public abstract class BaseEntity<TId>
    {
        private List<BaseDomainEvent> _events;
        
        /// <summary>
        /// Código da entidade
        /// </summary>
        public TId Id { get; set; }
        
        /// <summary>
        /// Data e hora da criação da entidade
        /// </summary>
        public DateTimeOffset CreatedAt { get; set; }
        
        /// <summary>
        /// Data e hora da última atualização da entidade
        /// </summary>
        public DateTimeOffset UpdatedAt { get; set; }
        
        /// <summary>
        /// Lista de eventos da entidade
        /// </summary>
        public List<BaseDomainEvent> Events => _events ??= new List<BaseDomainEvent>();
    }
}