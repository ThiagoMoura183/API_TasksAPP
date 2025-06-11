using Application.Response;
using Application.UserCQ.Commands;
using Application.UserCQ.ViewModels;
using AutoMapper;
using Domain.Abstractions;
using Infra.Repository.UnitOfWork;
using MediatR;
using Microsoft.Extensions.Configuration;

namespace Application.UserCQ.Handlers {
    public class LoginUserCommandHandler(IUnitOfWork unitOfWork, IAuthService authService, IConfiguration configuration, IMapper mapper) : IRequestHandler<LoginUserCommand, ResponseBase<RefreshTokenViewModel>> {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IAuthService _authService = authService;
        private readonly IConfiguration _configuration = configuration;
        private readonly IMapper _mapper = mapper;

        public async Task<ResponseBase<RefreshTokenViewModel>> Handle(LoginUserCommand request, CancellationToken cancellationToken) {
            var user = await _unitOfWork.UserRepository.Get(u => u.Email == request.Email);

            if (user is null) {
                return new ResponseBase<RefreshTokenViewModel>() {
                    ResponseInfo = new ResponseInfo() {
                        Title = "Usuário não encontrado",
                        ErrorDescription = $"Nenhum usuário encontrado para o e-mail informado - {request.Email}",
                        HTTPStatus = 404
                    },
                    Value = null
                };
            }

            var hashPasswordRequest = _authService.HashingPassword(request.Password!);
            if (hashPasswordRequest != user.PasswordHash) {
                return new ResponseBase<RefreshTokenViewModel>() {
                    ResponseInfo = new ResponseInfo() {
                        Title = "Senha incorreta",
                        ErrorDescription = $"Senha incorreta para o e-mail informado - {request.Email}",
                        HTTPStatus = 404
                    },
                    Value = null
                };
            }

            _ = int.TryParse(_configuration["JWT:RefreshTokenExpirationTimeInDays"], out int refreshTokenExpirationTimeInDays);
            user.RefreshToken = _authService.GenerateRefreshToken();
            user.RefreshTokenExpirationTime = DateTime.Now.AddDays(refreshTokenExpirationTimeInDays);

            await _unitOfWork.UserRepository.Update(user);
            _unitOfWork.Commit(); // Atualiza as informações de refreshToken no banco, conforme alterações acima

            RefreshTokenViewModel refreshTokenVM = _mapper.Map<RefreshTokenViewModel>(user);
            refreshTokenVM.TokenJWT = _authService.GenerateJWT(user.Email!, user.Username!);

            return new ResponseBase<RefreshTokenViewModel>() {
                ResponseInfo = null,
                Value = refreshTokenVM
            };
        }
    }
}
