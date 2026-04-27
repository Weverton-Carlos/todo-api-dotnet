using AutoMapper;
using FluentValidation;
using FluentValidation.Results;
using TodoDesafio.Application.DTOs;
using TodoDesafio.Application.Interfaces;
using TodoDesafio.Domain.Entities;
using TodoDesafio.Domain.Enums;
using TodoDesafio.Domain.Interfaces;

namespace TodoDesafio.Application.Services;

public class TodoItemService: ITodoItemService
{
    private readonly ITodoItemRepository _todoItemRepository;
    private readonly IMapper _mapper;
    private readonly IValidator<CreateTodoItemDto> _createTodoItemDtoValidator;
    private readonly IValidator<UpdateTodoItemDto> _updateTodoItemDtoValidator;

    public TodoItemService(
        ITodoItemRepository todoItemRepository,
        IMapper mapper,
        IValidator<CreateTodoItemDto> createTodoItemDtoValidator,
        IValidator<UpdateTodoItemDto> updateTodoItemDtoValidator)
    {
        _todoItemRepository = todoItemRepository;
        _mapper = mapper;
        _createTodoItemDtoValidator = createTodoItemDtoValidator;
        _updateTodoItemDtoValidator = updateTodoItemDtoValidator;
    }

    public async Task<IEnumerable<TodoItemDto>> GetAllAsync(Status? status, DateTime? dueDate)
    {
        var todoItems = await _todoItemRepository.GetAllAsync(status, dueDate);

        return _mapper.Map<IEnumerable<TodoItemDto>>(todoItems);
    }

    public async Task<TodoItemDto?> GetByIdAsync(int id)
    {
        var todoItem = await _todoItemRepository.GetByIdAsync(id);
        if (todoItem == null) return null;

        return _mapper.Map<TodoItemDto>(todoItem);
    }

    public async Task<TodoItemDto> CreateAsync(CreateTodoItemDto itemDto)
    {
        var validationResult = await _createTodoItemDtoValidator.ValidateAsync(itemDto);

        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        var todoItem = _mapper.Map<TodoItem>(itemDto);

        await _todoItemRepository.AddAsync(todoItem);
        await _todoItemRepository.SaveAsync();

        return _mapper.Map<TodoItemDto>(todoItem);
    }

    public async Task<bool> UpdateAsync(int id, UpdateTodoItemDto itemDto)
    {
        var validationResult = await _updateTodoItemDtoValidator.ValidateAsync(itemDto);

        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);
        
        var todoItem = await _todoItemRepository.GetByIdAsync(id);
        if (todoItem == null) return false;

        _mapper.Map(itemDto, todoItem);

        _todoItemRepository.Update(todoItem);
        return await _todoItemRepository.SaveAsync();
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var todoItem = await _todoItemRepository.GetByIdAsync(id);
        if (todoItem == null) return false;

        _todoItemRepository.Delete(todoItem);
        return await _todoItemRepository.SaveAsync();
    }
}