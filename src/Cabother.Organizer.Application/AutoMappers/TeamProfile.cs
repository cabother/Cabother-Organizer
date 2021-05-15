using AutoMapper;
using Cabother.Organizer.Application.Commands.Teams;
using Cabother.Organizer.Application.ViewModels.Teams;
using Cabother.Organizer.Domain.Entities;

namespace Cabother.Organizer.Application.AutoMappers
{
    public class TeamProfile : Profile
    {
        public TeamProfile()
        {
            CreateMap<CreateTeamCommand, Team>()
                .ForMember(x => x.Id, opt => opt.Ignore())
                .ForMember(x => x.CreatedAt, opt => opt.Ignore())
                .ForMember(x => x.UpdatedAt, opt => opt.Ignore())
                .ForMember(x => x.Events, opt => opt.Ignore());

            CreateMap<Team, TeamViewModel>();
        }
    }
}