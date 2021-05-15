using System.Collections.Generic;
using Cabother.Validations.Helpers;

namespace Cabother.Organizer.Application.ViewModels
{
    public class PagedListViewModel<T> where T : class
    {
        public PagedListViewModel(List<T> records, uint offset, uint limit)
        {
            records.ThrowIfNullOrEmpty(nameof(records));

            Offset = offset;
            Limit = limit;
            Records = records;
        }

        /// <summary>
        /// Quantidade de registros ignorados antes do 1 registro encontrado
        /// </summary>
        public uint Offset { get; }

        /// <summary>
        /// Limite de registros retornados pela consulta
        /// </summary>
        public uint Limit { get; }

        /// <summary>
        /// Lista de itens
        /// </summary>
        public List<T> Records { get; }

        /// <summary>
        /// Quantidade total de registros
        /// </summary>
        public uint RecordCount => (uint)(Records?.Count ?? 0);
    }
}