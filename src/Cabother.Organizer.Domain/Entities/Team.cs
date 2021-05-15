using System;

namespace Cabother.Organizer.Domain.Entities
{
    public class Team : BaseEntity<Guid>
    {
        /// <summary>
        /// Nome do time
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Apelido ou referência, exemplos @dream-tem, #dream-team 
        /// </summary>
        public string Alias { get; set; }

        /// <summary>
        /// Indica a situação atual do time
        /// </summary>
        public TeamStatus Status { get; set; } = TeamStatus.Inactive;
    }
}