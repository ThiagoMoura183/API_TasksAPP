using Application.Response;
using Application.UserCQ.ViewModels;
using MediatR;

namespace Application.UserCQ.Commands {
    public record RefreshTokenCommand : IRequest<ResponseBase<RefreshTokenViewModel>> {
        public string? Username { get; set; }
        public string? RefreshToken { get; set; }
    }
}
