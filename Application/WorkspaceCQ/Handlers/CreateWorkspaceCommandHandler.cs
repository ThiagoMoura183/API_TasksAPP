using Application.Response;
using Application.WorkspaceCQ.Commands;
using Application.WorkspaceCQ.ViewModels;
using AutoMapper;
using Domain.Entity;
using Infra.Repository.UnitOfWork;
using MediatR;

namespace Application.WorkspaceCQ.Handlers {
    public class CreateWorkspaceCommandHandler(IUnitOfWork unitOfWork, IMapper mapper) : IRequestHandler<CreateWorkspaceCommand, ResponseBase<CreateWorkspaceViewModel>> {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;
        public async Task<ResponseBase<CreateWorkspaceViewModel>> Handle(
            CreateWorkspaceCommand request, 
            CancellationToken cancellationToken
        ) {
            var user = await _unitOfWork.UserRepository.Get(u => u.Id == request.UserId);

            if (user is null) {
                return new ResponseBase<CreateWorkspaceViewModel> {
                    ResponseInfo = new ResponseInfo {
                        Title = "Usuário não encontrado",
                        ErrorDescription = $"Nenhum usuário encontrado com o Id informado: {request.UserId}",
                        HTTPStatus = 400
                    }
                };
            }

            var workspace = new Workspace() {
                User = user,
                Title = request.Title,
            };

            await _unitOfWork.WorkspaceRepository.Create(workspace);
            _unitOfWork.Commit();

            CreateWorkspaceViewModel workspaceViewModel = _mapper.Map<CreateWorkspaceViewModel>(workspace);

            return new ResponseBase<CreateWorkspaceViewModel> {
                ResponseInfo = null,
                Value = workspaceViewModel
            };
        }
    }
}
