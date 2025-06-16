using Application.WorkspaceCQ.ViewModels;
using AutoMapper;
using Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Mappings {
    public class WorkspaceMappings : Profile {
        public WorkspaceMappings() {
            // Para o UserId da ViewModel, é mapeado para o ID do usuário da entidade Workspace
            CreateMap<Workspace, CreateWorkspaceViewModel>()
                .ForMember(x => x.UserId, x => x.MapFrom(x => x.User!.Id)); 

        }
    }
}
