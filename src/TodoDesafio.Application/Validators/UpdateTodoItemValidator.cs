using FluentValidation;
using TodoDesafio.Application.DTOs;
using TodoDesafio.Domain.Interfaces;

namespace TodoDesafio.Application.Validators;

public class UpdateTodoItemValidator : AbstractValidator<UpdateTodoItemDto>
{
    private readonly ITodoItemRepository _repository;

    public UpdateTodoItemValidator(ITodoItemRepository repository)
    {
        _repository = repository;

        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Title is required")
            .MaximumLength(150)
            .MustAsync(BeUniqueTitle).WithMessage("Title already exists");

        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("Description is required")
            .MaximumLength(500);

        RuleFor(x => x.DueDate)
            .NotEmpty();

        RuleFor(x => x.Status)
            .IsInEnum();
    }

    private async Task<bool> BeUniqueTitle(UpdateTodoItemDto dto, string title, CancellationToken ct)
    {
        return !await _repository.ExistsByTitleAsync(title, dto.Id);
    }
}