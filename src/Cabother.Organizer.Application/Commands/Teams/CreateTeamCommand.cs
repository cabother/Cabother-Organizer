using Cabother.Organizer.Application.ViewModels.Teams;
using MediatR;

namespace Cabother.Organizer.Application.Commands.Teams
{
    public class CreateTeamCommand : BaseTeamCommand, IRequest<TeamViewModel>
    {
    }

    /// <summary>
    /// Valida as propriedades de <see cref="CreateTeamCommand"/>
    /// </summary>
    public class CreateTeamCommandValidator : BaseTeamCommandValidator<CreateTeamCommand>
    {
        public CreateTeamCommandValidator()
            : base()
        {

        }
    }
}