using Application.Response;
using Application.UserCQ.Commands;
using Application.UserCQ.ViewModels;
using AutoMapper;
using Domain.Abstractions;
using Domain.Entity;
using Infra.Persistency;
using MediatR;
using Domain.Enum;

namespace Application.UserCQ.Handlers {
    public class CreateUserCommandHandler(TasksDbContext context, IMapper mapper, IAuthService authService) : IRequestHandler<CreateUserCommand, ResponseBase<RefreshTokenViewModel?>> {
        private readonly TasksDbContext _context = context;
        private readonly IMapper _mapper = mapper;
        private readonly IAuthService _authService = authService;

        public async Task<ResponseBase<RefreshTokenViewModel>> Handle(CreateUserCommand request, CancellationToken cancellationToken) {
            #region Validação de email e username unique
            var isUniqueEmailAndUsername = _authService.IsUniqueEmailAndUsername(request.Email!, request.Username!);

            if (isUniqueEmailAndUsername is ValidationFieldsUserEnum.UsernameAndEmailUnavailable) {
                return new ResponseBase<RefreshTokenViewModel> {
                    ResponseInfo = new() {
                        Title = "Username e email indisponíveis!",
                        ErrorDescription = $"O username {request.Username} e o email {request.Email} já estão sendo utilizados. Tente utilizar outros.",
                        HTTPStatus = 400
                    },
                    Value = null
                };
            }

            if (isUniqueEmailAndUsername is ValidationFieldsUserEnum.UsernameUnavailable) {
                return new ResponseBase<RefreshTokenViewModel> { 
                    ResponseInfo = new() {
                        Title = "Username indisponível!",
                        ErrorDescription = $"O username {request.Username} já está sendo utilizado. Tente utilizar outro.",
                        HTTPStatus = 400                        
                    },
                    Value = null
                };
            }

            if (isUniqueEmailAndUsername is ValidationFieldsUserEnum.EmailUnavailable) {
                return new ResponseBase<RefreshTokenViewModel> {
                    ResponseInfo = new() {
                        Title = "Email indisponível!",
                        ErrorDescription = $"O Email {request.Email} já está sendo utilizado. Tente utilizar outro.",
                        HTTPStatus = 400
                    },
                    Value = null
                };
            }
            #endregion

            // Mapeia para um objeto USER a partir do source "request", que é o CreateUserCommand
            var user = _mapper.Map<User>(request);
            user.RefreshToken = _authService.GenerateRefreshToken();
            user.PasswordHash = _authService.HashingPassword(request.Password!);

            _context.Users.Add(user);
            _context.SaveChanges();

            var refreshTokenoVM = _mapper.Map<RefreshTokenViewModel>(user);
            // Aqui o TokenJWT é null, conforme mapping acima. Logo, vamos atualizar chamando o método da dependência de authService
            refreshTokenoVM.TokenJWT = _authService.GenerateJWT(user.Email!, user.Username!);

            return new ResponseBase<RefreshTokenViewModel>() {
                ResponseInfo = null,
                Value = refreshTokenoVM
            };
        }
    }
}
