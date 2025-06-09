using Application.UserCQ.Commands;
using Application.UserCQ.ViewModels;
using AutoMapper;
using Domain.Entity;

namespace Application.Mappings;
public class ProfileMappings : Profile {
    public ProfileMappings() {
        CreateMap<CreateUserCommand, User>()
            .ForMember(x => x.RefreshToken, x => x.AllowNull())
            .ForMember(x => x.RefreshTokenExpirationTime, x => x.MapFrom(x => AddTenDays()))
            .ForMember(x => x.PasswordHash, x => x.AllowNull());
        CreateMap<User, RefreshTokenViewModel>()
            // Pelo fato do ProfileMapping não ser possível realizar injeção de dependência do authService, vamos permitir nulo e alterar o valor no Handler
            .ForMember(x => x.TokenJWT, x => x.AllowNull());
        CreateMap<RefreshTokenViewModel, UserInfoViewModel>();
    }

    private DateTime AddTenDays() { return DateTime.Now.AddDays(10); }
}
