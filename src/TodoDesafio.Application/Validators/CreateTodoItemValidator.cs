using FluentValidation;
using TodoDesafio.Application.DTOs;
using TodoDesafio.Domain.Interfaces;

namespace TodoDesafio.Application.Validators;

public class CreateTodoItemValidator : AbstractValidator<CreateTodoItemDto>
{
    private readonly ITodoItemRepository _repository;

    public CreateTodoItemValidator(ITodoItemRepository repository)
    {
        _repository = repository;

        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Title is required")
            .MaximumLength(150).WithMessage("Title must be at most 150 characters")
            .MustAsync(BeUniqueTitle).WithMessage("Title already exists");

        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("Description is required")
            .MaximumLength(500).WithMessage("Description must be at most 500 characters");

        RuleFor(x => x.DueDate)
            .NotEmpty().WithMessage("Due date is required");
        // 👇 NÃO validar se é passado (regra permite)

        RuleFor(x => x.Status)
            .IsInEnum().WithMessage("Invalid status value");
    }

    private async Task<bool> BeUniqueTitle(string title, CancellationToken cancellationToken)
    {
        return !await _repository.ExistsByTitleAsync(title);
    }
}