using Application.Response;
using Application.UserCQ.ViewModels;
using MediatR;

namespace Application.UserCQ.Commands {
    public record LoginUserCommand : IRequest<ResponseBase<RefreshTokenViewModel>> {
        public string? Email { get; set; }
        public string? Password { get; set; }
    }
}
