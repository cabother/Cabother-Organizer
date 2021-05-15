using FluentValidation;

namespace Cabother.Organizer.Application.Commands.Teams
{
    public class BaseTeamCommand
    {
        public string Name { get; set; }
    }

    /// <summary>
    /// Valida as propriedades de <see cref="BaseTeamCommandValidator"/>
    /// </summary>
    /// <typeparam name="T">Tipo da entidade filha que será validada</typeparam>
    public class BaseTeamCommandValidator<T> : AbstractValidator<T>
        where T : BaseTeamCommand
    {
        protected BaseTeamCommandValidator()
        {
            RuleFor(x => x.Name)
                .NotNull().WithMessage("Nome do time é obrigatório.")
                .NotEmpty().WithMessage("Nome do time é obrigatório.")
                .MinimumLength(2).WithMessage("Nome do time deve ter no mínimo 2 caracteres.")
                .MaximumLength(50).WithMessage("Nome do time deve ter no máximo 50 caracteres.");
        }
    }
}