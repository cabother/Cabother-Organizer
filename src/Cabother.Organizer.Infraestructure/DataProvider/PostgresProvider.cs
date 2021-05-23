using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Linq.Dynamic.Core.Exceptions;
using System.Threading;
using System.Threading.Tasks;
using Cabother.Organizer.Application.Interfaces.DataProviders.Teams;
using Cabother.Organizer.Domain.Entities;
using Cabother.Organizer.Infraestructure.Data.Configs;
using Cabother.Validations.Helpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Cabother.Organizer.Infraestructure.DataProvider
{
    /// <summary>
    /// Provedor de informações do banco de dados Postgresql
    /// </summary>
    public sealed class PostgresProvider :
        IValidateTeamName,
        IDisposable
    {
        private readonly OrganizerDbContext _context;
        private readonly ILogger<PostgresProvider> _logger;
        private bool _disposed;

        public PostgresProvider(
            OrganizerDbContext context,
            ILogger<PostgresProvider> logger)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Verifica se o name já existe cadastrado
        /// </summary>
        /// <param name="name">Nome do time pesquisado</param>
        /// <param name="cancellationToken">Objeto para tratativa de solicitações de cancelamento de processamento</param>
        /// <returns>Retorna verdadeiro caso o nome já existe na base</returns>
        async Task<bool> IValidateTeamName.TeamNameExistsAsync(string name, CancellationToken cancellationToken)
        {
            name.ThrowIfNull();

            var duplicateName = await _context.Teams.AnyAsync(x => x.Name.Equals(name), cancellationToken);

            return !duplicateName;
        }

        /// <summary>Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.</summary>
        public void Dispose()
        {
            if (_disposed)
                return;

            _context.Dispose();

            _disposed = true;
        }

        #region Private Methods

        /// <summary>
        /// Aplica a ordenação na consulta
        /// </summary>
        /// <param name="query">Query base que será utilizada</param>
        /// <param name="sortBy">Nome da propriedade que será utilizada para a ordenação</param>
        private void ApplyOrderBy(ref IQueryable<Team> query, string sortBy)
        {
            sortBy ??= "Name";

            try
            {
                _logger.LogDebug("Aplicando ordenação pelo campo '{SortBy}'", sortBy);
                query = query.OrderBy(sortBy);
            }
            catch (ParseException) when (sortBy != "Name")
            {
                _logger.LogWarning("Campo '{SortBy}' informado para ordenação inválido", sortBy);
                _logger.LogInformation("Aplicando ordenação pelo campo 'Name'");
                query = query.OrderBy("Name");
            }
        }

        #endregion
    }
}