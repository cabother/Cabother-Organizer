using System;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using Cabother.Organizer.Api.Filters;
using Cabother.Organizer.Api.ViewModels;
using Cabother.Organizer.Api.ViewModels.Teams;
using Cabother.Organizer.Application.Commands.Teams;
using Cabother.Organizer.Application.ViewModels.Teams;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Cabother.Organizer.Api.Controllers.V1
{
    [ApiController]
    [ApiVersion("1")]
    [ApiExplorerSettings(GroupName = "v1")]
    [ServiceFilter(typeof(ApplicationExceptionFilterAttribute))]
    [Route("api/v{version:apiVersion}/teams")]
    public class TeamController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public TeamController(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        /// <summary>
        /// Insere novos times
        /// </summary>
        /// <param name="request">Objeto com as informações do time</param>
        /// <returns>Retorna o time cadastrado</returns>
        [HttpPost(Name = "Create")]
        [SwaggerOperation(
            Summary = "Insere novos times",
            Description = "Cadastro de novos times",
            OperationId = "CreateTeam",
            Tags = new[] { "teams" })]
        [SwaggerResponse((int)HttpStatusCode.Created, "Time criado com sucesso", typeof(SuccessResponseViewModel<TeamViewModel>))]
        [SwaggerResponse((int)HttpStatusCode.BadRequest, "Informações incorretas", typeof(ErrorResponseViewModel))]
        [SwaggerResponse((int)HttpStatusCode.NotFound, "Dependências não encontradas", typeof(ErrorResponseViewModel))]
        public async Task<IActionResult> CreateAsync([FromBody] CreateTeamViewModelRequest request)
        {
            var command = _mapper.Map<CreateTeamCommand>(request);

            var commandResponse = await _mediator.Send(command);

            var apiResponse = new SuccessResponseViewModel<TeamViewModel>(commandResponse);

            return CreatedAtRoute("GetTeamById", new { id = commandResponse.Id }, apiResponse);
        }
    }
}