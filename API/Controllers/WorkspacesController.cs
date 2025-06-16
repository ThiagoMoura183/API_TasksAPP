using Application.WorkspaceCQ.Commands;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using ZstdSharp.Unsafe;

namespace API.Controllers {
    public static class WorkspacesController {
        public static void WorkspacesRoutes(this WebApplication app) {
            // "Tags" é o nome que até então era o nome da controller (no padrão MVC)
            // "Group" é a rota, exemplo: --> Workspaces <-- /hello-world
            var group = app.MapGroup("Workspaces").WithTags("Workspaces");

            group.MapPost("create", () => CreateWorkspace);
            //group.MapPut("edit", () => EditWorkspace);
            //group.MapDelete("delete", () => DeleteWorkspace);
            //group.MapGet("get-all", () => GetAllWorkspace);
            //group.MapGet("get", () => GetWorkspace);

            // Delegates
        }


        public static async Task<IResult> CreateWorkspace(
            [FromServices] IMediator _mediator, 
            [FromBody] CreateWorkspaceCommand command
        ) {
            var result = await _mediator.Send(command);

            return result.ResponseInfo is null ? Results.Ok(result.Value) : Results.BadRequest(result.ResponseInfo);

        }

        //public async Task<IResult> EditWorkspace([FromServices] IMediator _mediator) {

        //}

        //public static async Task<IResult> DeleteWorkspace([FromServices] IMediator _mediator) {

        //}

        //public static async Task<IResult> GetAllWorkspace([FromServices] IMediator _mediator) {

        //}

        //public static async Task<IResult> GetWorkspace([FromServices] IMediator _mediator) {

        //}
    }
}
