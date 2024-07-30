using AutoMapper;
using TheProjectTascamon.Models;
using TheProjectTascamon.ViewModel;

namespace TheProjectTascamon
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<User, LoginViewModel>().ReverseMap();
            CreateMap<RegisterViewModel, User>().ReverseMap();
            CreateMap<PokemonViewModel, Pokemon>().ReverseMap();
            CreateMap<Move, MoveViewModel>().ReverseMap();
        }
    }
}
