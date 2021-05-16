using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Cabother.Exceptions.Extensions;
using Cabother.Exceptions.Requests;
using Cabother.Organizer.Application.Commands.Teams;
using Cabother.Organizer.Application.Interfaces.DataProviders.Teams;
using Cabother.Organizer.Application.ViewModels.Teams;
using Cabother.Organizer.Domain.Entities;
using Cabother.Organizer.Domain.Events.Teams;
using Cabother.Validations.Helpers;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Cabother.Organizer.Application.Handlers.Teams
{
    public class CreateTeamHandler : IRequestHandler<CreateTeamCommand, TeamViewModel>
    {
        private readonly ILogger<CreateTeamHandler> _logger;
        private readonly IValidateTeamName _validateTeamName;
        private readonly IInsertTeam _insertTeam;
        private readonly IMapper _mapper;

        public CreateTeamHandler(
            ILogger<CreateTeamHandler> logger,
            IValidateTeamName validateTeamName,
            IInsertTeam insertTeam,
            IMapper mapper)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _validateTeamName = validateTeamName ?? throw new ArgumentNullException(nameof(validateTeamName));
            _insertTeam = insertTeam ?? throw new ArgumentNullException(nameof(insertTeam));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<TeamViewModel> Handle(CreateTeamCommand request, CancellationToken cancellationToken)
        {
            request.ThrowIfNull(nameof(request));

            await ValidateDuplicityAsync(request, cancellationToken);

            var newTeam = _mapper.Map<Team>(request);

            newTeam.Events.Add(new CreatedTeamEvent(newTeam));

            var dbTeam = await SaveTeamAsync(newTeam, cancellationToken);

            return _mapper.Map<TeamViewModel>(dbTeam);
        }

        private async Task ValidateDuplicityAsync(CreateTeamCommand request, CancellationToken cancellationToken)
        {
            _logger.LogDebug("Validando duplicidade de nome do time");
            var isDuplicatedTeamName = await _validateTeamName.TeamNameExistsAsync(request.Name, cancellationToken);

            if (isDuplicatedTeamName)
                throw new ConflictException($"Name '{request.Name}' já cadastrado.", "4000");
        }

        private async Task<Team> SaveTeamAsync(Team team, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogDebug("Inserindo time no contexto do banco de dados");
                var dbTeam = await _insertTeam.InsertTeamAsync(team, cancellationToken);

                return dbTeam;
            }
            catch (Exception ex)
            {
                throw HandleException(ex, "5000", "Erro inserindo time no banco de dados");
            }
        }

        /// <summary>
        /// Realiza a tratativa das exceções
        /// </summary>
        /// <param name="ex">Exceção ocorrida</param>
        /// <param name="errorCode">Código de identificação do erro</param>
        /// <param name="message">Mensagem descritiva do erro</param>
        /// <returns>Retorna exceção do tipo <see cref="InternalServerErrorException"/></returns>
        private InternalServerErrorException HandleException(Exception ex, string errorCode, string message)
        {
            var newEx = ex.ToInternalServerErrorException(errorCode, message);

            _logger.LogError(ex, newEx.Message);

            return newEx;
        }
    }
}