using AutoMapper;
using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using Moq;
using TodoDesafio.Application.DTOs;
using TodoDesafio.Application.Mappings;
using TodoDesafio.Application.Services;
using TodoDesafio.Domain.Entities;
using TodoDesafio.Domain.Enums;
using TodoDesafio.Domain.Extensions;
using TodoDesafio.Domain.Interfaces;
using Xunit;

namespace TodoDesafio.Tests.Services;

public class TodoItemServiceTests
{
    private readonly Mock<ITodoItemRepository> _repositoryMock;
    private readonly Mock<IValidator<CreateTodoItemDto>> _createValidatorMock;
    private readonly Mock<IValidator<UpdateTodoItemDto>> _updateValidatorMock;
    private readonly TodoItemService _service;
    private readonly IMapper _mapper;

    public TodoItemServiceTests()
    {
        _repositoryMock = new Mock<ITodoItemRepository>();
        _createValidatorMock = new Mock<IValidator<CreateTodoItemDto>>();
        _updateValidatorMock = new Mock<IValidator<UpdateTodoItemDto>>();

        var config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<TodoItemProfile>();
        });

        _mapper = config.CreateMapper();

        _service = new TodoItemService(
            _repositoryMock.Object,
            _mapper,
            _createValidatorMock.Object,
            _updateValidatorMock.Object
        );
    }
    
    [Fact]
    public async Task CreateAsync_ShouldCreateTodo_WhenValid()
    {
        var dto = new CreateTodoItemDto
        {
            Title = "Test",
            Description = "Desc",
            Status = Status.Pending.GetDescription(),
            DueDate = DateTime.UtcNow
        };

        _createValidatorMock
            .Setup(v => v.ValidateAsync(It.IsAny<CreateTodoItemDto>(), default))
            .ReturnsAsync(new ValidationResult());

        _repositoryMock
            .Setup(r => r.AddAsync(It.IsAny<TodoItem>()))
            .Returns(Task.CompletedTask);

        _repositoryMock
            .Setup(r => r.SaveAsync())
            .ReturnsAsync(true);

        var result = await _service.CreateAsync(dto);

        result.Should().NotBeNull();
        result.Title.Should().Be(dto.Title);

        _repositoryMock.Verify(r => r.AddAsync(It.IsAny<TodoItem>()), Times.Once);
    }
    
    [Fact]
    public async Task CreateAsync_ShouldThrowException_WhenInvalid()
    {
        var dto = new CreateTodoItemDto();

        var failures = new List<ValidationFailure>
        {
            new ValidationFailure("Title", "Title is required")
        };

        _createValidatorMock
            .Setup(v => v.ValidateAsync(It.IsAny<CreateTodoItemDto>(), default))
            .ReturnsAsync(new ValidationResult(failures));

        await Assert.ThrowsAsync<ValidationException>(() => _service.CreateAsync(dto));

        _repositoryMock.Verify(r => r.AddAsync(It.IsAny<TodoItem>()), Times.Never);
    }
    
    [Fact]
    public async Task GetByIdAsync_ShouldReturnTodo_WhenExists()
    {
        var todo = new TodoItem { Id = 1, Title = "Test" };

        _repositoryMock
            .Setup(r => r.GetByIdAsync(1))
            .ReturnsAsync(todo);

        var result = await _service.GetByIdAsync(1);

        result.Should().NotBeNull();
        result!.Id.Should().Be(1);
    }
    
    [Fact]
    public async Task GetByIdAsync_ShouldReturnNull_WhenNotFound()
    {
        _repositoryMock
            .Setup(r => r.GetByIdAsync(1))
            .ReturnsAsync((TodoItem?)null);

        var result = await _service.GetByIdAsync(1);

        result.Should().BeNull();
    }
    
    //Update
    [Fact]
    public async Task UpdateAsync_ShouldUpdate_WhenExists()
    {
        // Arrange
        var entity = new TodoItem { Id = 1, Title = "Old" };

        var dto = new UpdateTodoItemDto
        {
            Title = "Updated",
            Description = "Desc",
            Status = Status.Completed.GetDescription(),
            DueDate = DateTime.UtcNow
        };

        _updateValidatorMock
            .Setup(v => v.ValidateAsync(It.IsAny<UpdateTodoItemDto>(), default))
            .ReturnsAsync(new ValidationResult());

        _repositoryMock
            .Setup(r => r.GetByIdAsync(1))
            .ReturnsAsync(entity);

        _repositoryMock
            .Setup(r => r.SaveAsync())
            .ReturnsAsync(true);

        // Act
        var result = await _service.UpdateAsync(1, dto);

        // Assert
        result.Should().BeTrue();
        entity.Title.Should().Be("Updated");
    }
    
    
    [Fact]
    public async Task UpdateAsync_ShouldUpdateTodo_WhenValid()
    {
        // Arrange
        var entity = new TodoItem { Id = 1, Title = "Old" };

        var dto = new UpdateTodoItemDto
        {
            Title = "Updated",
            Description = "Desc",
            Status = Status.Completed.GetDescription(),
            DueDate = DateTime.UtcNow
        };

        _updateValidatorMock
            .Setup(v => v.ValidateAsync(It.IsAny<UpdateTodoItemDto>(), default))
            .ReturnsAsync(new ValidationResult());

        _repositoryMock
            .Setup(r => r.GetByIdAsync(1))
            .ReturnsAsync(entity);

        _repositoryMock
            .Setup(r => r.SaveAsync())
            .ReturnsAsync(true);

        // Act
        var result = await _service.UpdateAsync(1, dto);

        // Assert
        result.Should().BeTrue();
        entity.Title.Should().Be("Updated");

        _repositoryMock.Verify(r => r.Update(It.IsAny<TodoItem>()), Times.Once);
    }
    
    [Fact]
    public async Task UpdateAsync_ShouldThrowException_WhenInvalid()
    {
        // Arrange
        var dto = new UpdateTodoItemDto();

        var failures = new List<ValidationFailure>
        {
            new ValidationFailure("Title", "Title is required")
        };

        _updateValidatorMock
            .Setup(v => v.ValidateAsync(It.IsAny<UpdateTodoItemDto>(), default))
            .ReturnsAsync(new ValidationResult(failures));

        // Act & Assert
        await Assert.ThrowsAsync<ValidationException>(() => _service.UpdateAsync(1, dto));

        _repositoryMock.Verify(r => r.Update(It.IsAny<TodoItem>()), Times.Never);
    }

    [Fact]
    public async Task UpdateAsync_ShouldReturnFalse_WhenNotFound()
    {
        // Arrange
        var dto = new UpdateTodoItemDto
        {
            Title = "Updated"
        };

        _updateValidatorMock
            .Setup(v => v.ValidateAsync(It.IsAny<UpdateTodoItemDto>(), default))
            .ReturnsAsync(new ValidationResult());

        _repositoryMock
            .Setup(r => r.GetByIdAsync(1))
            .ReturnsAsync((TodoItem?)null);

        // Act
        var result = await _service.UpdateAsync(1, dto);

        // Assert
        result.Should().BeFalse();
    }
    
    
    //Delete
    [Fact]
    public async Task DeleteAsync_ShouldReturnTrue_WhenExists()
    {
        var entity = new TodoItem { Id = 1 };

        _repositoryMock
            .Setup(r => r.GetByIdAsync(1))
            .ReturnsAsync(entity);

        _repositoryMock
            .Setup(r => r.SaveAsync())
            .ReturnsAsync(true);

        var result = await _service.DeleteAsync(1);

        result.Should().BeTrue();
    }
    
    [Fact]
    public async Task GetAllAsync_ShouldFilterByStatus()
    {
        var list = new List<TodoItem>
        {
            new TodoItem { Status = Status.Pending },
            new TodoItem { Status = Status.Completed }
        };

        _repositoryMock
            .Setup(r => r.GetAllAsync(Status.Pending, null))
            .ReturnsAsync(list.Where(x => x.Status == Status.Pending).ToList());

        var result = await _service.GetAllAsync(Status.Pending, null);

        result.Should().HaveCount(1);
    }
    
    public static IEnumerable<object[]> GetStatusTestData()
    {
        yield return new object[] { Status.Pending, 1 };
        yield return new object[] { Status.InProgress, 1 };
        yield return new object[] { Status.Completed, 1 };
        yield return new object[] { null, 3 };
    }
    
    [Theory]
    [MemberData(nameof(GetStatusTestData))]
    public async Task GetAllAsync_ShouldFilterCorrectly(Status? filterStatus, int expectedCount)
    {
        // Arrange
        var list = new List<TodoItem>
        {
            new TodoItem { Status = Status.Pending },
            new TodoItem { Status = Status.InProgress },
            new TodoItem { Status = Status.Completed }
        };

        _repositoryMock
            .Setup(r => r.GetAllAsync(filterStatus, null))
            .ReturnsAsync(filterStatus == null
                ? list
                : list.Where(x => x.Status == filterStatus).ToList());

        // Act
        var result = await _service.GetAllAsync(filterStatus, null);

        // Assert
        result.Should().HaveCount(expectedCount);
    }
    
    public static IEnumerable<object[]> GetDueDateTestData()
    {
        var today = DateTime.UtcNow.Date;

        yield return new object[] { today, 1 };
        yield return new object[] { today.AddDays(1), 1 };
        yield return new object[] { today.AddDays(10), 0 };
        yield return new object[] { null, 3 };
    }
    
    [Theory]
    [MemberData(nameof(GetDueDateTestData))]
    public async Task GetAllAsync_ShouldFilterByDueDate(DateTime? filterDate, int expectedCount)
    {
        var today = DateTime.UtcNow.Date;

        var list = new List<TodoItem>
        {
            new TodoItem { DueDate = today },
            new TodoItem { DueDate = today.AddDays(1) },
            new TodoItem { DueDate = today.AddDays(2) }
        };

        _repositoryMock
            .Setup(r => r.GetAllAsync(null, It.IsAny<DateTime?>()))
            .ReturnsAsync((Status? status, DateTime? dueDate) =>
            {
                if (dueDate == null)
                    return list;

                return list
                    .Where(x => x.DueDate.Date == dueDate.Value.Date)
                    .ToList();
            });

        var result = await _service.GetAllAsync(null, filterDate);

        result.Should().HaveCount(expectedCount);
    }
}