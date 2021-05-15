using Cabother.Organizer.Api.Filters;
using Microsoft.AspNetCore.Mvc;

namespace Cabother.Organizer.Api.Controllers.V1
{
    [ApiController]
    [ApiVersion("2")]
    [ApiExplorerSettings(GroupName = "v1")]
    [ServiceFilter(typeof(ApplicationExceptionFilterAttribute))]
    [Route("api/v{version:apiVersion}/teams")]
    public class TeamController : ControllerBase
    {
        
    }
}