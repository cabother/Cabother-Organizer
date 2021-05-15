using System.Linq;
using System.Net;
using Cabother.Exceptions;
using Cabother.Exceptions.Databases;
using Cabother.Exceptions.Requests;
using Cabother.Organizer.Api.ViewModels;
using FluentValidation;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Hosting;

namespace Cabother.Organizer.Api.Filters
{
    public class ApplicationExceptionFilterAttribute : ExceptionFilterAttribute
    {
        private readonly IWebHostEnvironment _env;

        public ApplicationExceptionFilterAttribute(IWebHostEnvironment env)
        {
            _env = env;
        }

        public override void OnException(ExceptionContext context)
        {
            var problem = new ErrorViewModel
            {
                Status = 500,
                UserMessage = context.Exception.Message,
                Detail = _env.IsDevelopment() ? context.Exception.ToString() : null,
                Type = context.Exception.GetType().Name,
                ErrorCode = context.Exception is BaseException baseException ? baseException.ErrorCode : null
            };

            switch (context.Exception)
            {
                case ConflictException:
                    problem.Status = (int)HttpStatusCode.Conflict;
                    context.Result = new ConflictObjectResult(problem);
                    return;

                case EntityNotFoundException:
                    problem.Status = (int)HttpStatusCode.NotFound;
                    context.Result = new NotFoundObjectResult(problem);
                    return;

                case BadRequestException:
                    problem.Status = (int)HttpStatusCode.BadRequest;
                    context.Result = new BadRequestObjectResult(problem);
                    return;

                case ValidationException validationEx:
                    problem.Status = (int)HttpStatusCode.BadRequest;
                    problem.UserMessage = validationEx.Errors.First().ErrorMessage;
                    context.Result = new BadRequestObjectResult(problem);
                    return;

                default:
                    var result = new ObjectResult(problem);
                    result.StatusCode = problem.Status;
                    context.Result = result;
                    return;
            }
        }
    }
}