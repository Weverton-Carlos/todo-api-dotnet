using AutoMapper;
using TodoDesafio.Application.DTOs;
using TodoDesafio.Domain.Entities;
using TodoDesafio.Domain.Extensions;

namespace TodoDesafio.Application.Mappings;

public class TodoItemProfile : Profile
{
    public TodoItemProfile()
    {
        // Entity -> DTO
        CreateMap<TodoItem, TodoItemDto>()
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.GetDescription()));

        // DTO -> Entity
        CreateMap<CreateTodoItemDto, TodoItem>();

        CreateMap<UpdateTodoItemDto, TodoItem>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.IsDeleted, opt => opt.Ignore());
    }
}