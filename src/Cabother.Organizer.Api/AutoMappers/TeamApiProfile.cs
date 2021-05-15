using AutoMapper;
using Cabother.Organizer.Api.ViewModels.Teams;
using Cabother.Organizer.Application.Commands.Teams;


namespace Cabother.Organizer.Api.AutoMappers
{
    public class TeamApiProfile : Profile
    {
        public TeamApiProfile()
        {
            CreateMap<CreateTeamViewModelRequest, CreateTeamCommand>();
        }
    }
}