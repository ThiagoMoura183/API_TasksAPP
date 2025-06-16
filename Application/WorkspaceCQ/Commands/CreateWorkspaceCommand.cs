using Application.Response;
using Application.WorkspaceCQ.ViewModels;
using MediatR;

namespace Application.WorkspaceCQ.Commands {
    public record CreateWorkspaceCommand : IRequest<ResponseBase<CreateWorkspaceViewModel>> {
        public string? Title { get; set; }
        public Guid? UserId { get; set; }
    }
}
