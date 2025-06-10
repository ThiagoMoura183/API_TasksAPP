using Application.Response;
using Application.UserCQ.Commands;
using Application.UserCQ.ViewModels;
using AutoMapper;
using Domain.Abstractions;
using Infra.Persistency;
using MediatR;
using Microsoft.Extensions.Configuration;
using System.Reflection.Metadata.Ecma335;

namespace Application.UserCQ.Handlers {
    public class RefreshTokenCommandHandler(TasksDbContext context, IAuthService authService, IConfiguration configuration, IMapper mapper) : IRequestHandler<RefreshTokenCommand, ResponseBase<RefreshTokenViewModel>> {
        private readonly TasksDbContext _context = context;
        private readonly IAuthService _authService = authService;
        private readonly IConfiguration _configuration = configuration;
        private readonly IMapper _mapper = mapper;

        public async Task<ResponseBase<RefreshTokenViewModel>> Handle(RefreshTokenCommand request, CancellationToken cancellationToken) {
            var user = _context.Users.FirstOrDefault(u => u.Username == request.Username);

            if (user is null || user.RefreshToken != request.RefreshToken || user.RefreshTokenExpirationTime < DateTime.Now) {
                return new ResponseBase<RefreshTokenViewModel>() {
                    ResponseInfo = new ResponseInfo() {
                        Title = "Token ",
                        ErrorDescription = $"Refresh token inválido ou expirado. Faça login novamente.",
                        HTTPStatus = 404
                    },
                    Value = null
                };
            }

            user.RefreshToken = _authService.GenerateRefreshToken();
            _ = int.TryParse(_configuration["JWT:RefreshTokenExpirationTimeInDays"], out int refreshTokenExpirationTimeInDays);
            user.RefreshToken = _authService.GenerateRefreshToken();
            user.RefreshTokenExpirationTime = DateTime.Now.AddDays(refreshTokenExpirationTimeInDays);
            _context.Update(user);
            _context.SaveChanges();

            RefreshTokenViewModel refreshTokenVM = _mapper.Map<RefreshTokenViewModel>(user);
            refreshTokenVM.TokenJWT = _authService.GenerateJWT(user.Email!, user.Username!);

            return new ResponseBase<RefreshTokenViewModel> {
                ResponseInfo = null,
                Value = refreshTokenVM
            };
        }
    }
}
