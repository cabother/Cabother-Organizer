using System.Threading;
using System.Threading.Tasks;
using Cabother.Organizer.Domain.Entities;

namespace Cabother.Organizer.Application.Interfaces.DataProviders.Teams
{
    public interface IInsertTeam
    {
        /// <summary>
        /// Inclui uma novo time no banco de dados 
        /// </summary>
        /// <param name="team">Time a ser adicionado</param>
        /// <param name="cancellationToken">Objeto para gestão das solicitações de cancelamento do processamento</param>
        /// <returns>Retorna a time com as informações atualizadas após a inserção</returns>
        Task<Team> InsertTeamAsync(Team team, CancellationToken cancellationToken = default);
    }
}