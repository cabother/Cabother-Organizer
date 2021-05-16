using System.Threading;
using System.Threading.Tasks;

namespace Cabother.Organizer.Application.Interfaces.DataProviders.Teams
{
    public interface IValidateTeamName
    {
        /// <summary>
        /// Método responsável por validar se existe um time com mesmo nome já cadastrado
        /// </summary>
        /// <param name="name">Nome do time para validação</param>
        /// <param name="cancellationToken">Objeto para tratativa de solicitações de cancelamento de processamento</param>
        /// <returns>Retorna verdadeiro caso o nome já exista na base</returns>
        Task<bool> TeamNameExistsAsync(string name, CancellationToken cancellationToken = default);
    }
}