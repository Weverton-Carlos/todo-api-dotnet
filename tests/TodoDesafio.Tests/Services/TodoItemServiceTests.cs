using FluentAssertions;
using Moq;
using TodoDesafio.Application.DTOs;
using TodoDesafio.Application.Services;
using TodoDesafio.Domain.Entities;
using TodoDesafio.Domain.Enums;
using TodoDesafio.Domain.Interfaces;

namespace TodoDesafio.Tests.Services;

public class TodoItemServiceTests
{
    private readonly Mock<ITodoItemRepository> _repositoryMock;
    private readonly TodoItemService _service;

    public TodoItemServiceTests()
    {
        _repositoryMock = new Mock<ITodoItemRepository>();
        _service = new TodoItemService(_repositoryMock.Object);
    }
    
    [Fact]
    public async Task CreateAsync_ShouldCreateTodo()
    {
        // Arrange
        var dto = new CreateTodoItemDto
        {
            Title = "Test",
            Description = "Desc",
            Status = Status.Pending,
            DueDate = DateTime.UtcNow
        };

        _repositoryMock
            .Setup(r => r.AddAsync(It.IsAny<TodoItem>()))
            .Returns(Task.CompletedTask);

        _repositoryMock
            .Setup(r => r.SaveAsync())
            .ReturnsAsync(true);

        // Act
        var result = await _service.CreateAsync(dto);

        // Assert
        result.Should().NotBeNull();
        result.Title.Should().Be(dto.Title);

        _repositoryMock.Verify(r => r.AddAsync(It.IsAny<TodoItem>()), Times.Once);
        _repositoryMock.Verify(r => r.SaveAsync(), Times.Once);
    }
    
    
    [Fact]
    public async Task GetByIdAsync_ShouldReturnTodo_WhenExists()
    {
        // Arrange
        var todo = new TodoItem
        {
            Id = 1,
            Title = "Test"
        };

        _repositoryMock
            .Setup(r => r.GetByIdAsync(1))
            .ReturnsAsync(todo);

        // Act
        var result = await _service.GetByIdAsync(1);

        // Assert
        result.Should().NotBeNull();
        result!.Id.Should().Be(1);
    }
    
    [Fact]
    public async Task GetByIdAsync_ShouldReturnNull_WhenNotFound()
    {
        // Arrange
        _repositoryMock
            .Setup(r => r.GetByIdAsync(1))
            .ReturnsAsync((TodoItem?)null);

        // Act
        var result = await _service.GetByIdAsync(1);

        // Assert
        result.Should().BeNull();
    }
    
    public async Task UpdateAsync_ShouldUpdate_WhenExists()
    {
        var todo = new TodoItem { Id = 1 };

        _repositoryMock
            .Setup(r => r.GetByIdAsync(1))
            .ReturnsAsync(todo);

        _repositoryMock
            .Setup(r => r.SaveAsync())
            .ReturnsAsync(true);

        var dto = new UpdateTodoItemDto
        {
            Title = "Updated",
            Status = Status.Completed,
            DueDate = DateTime.UtcNow
        };

        var result = await _service.UpdateAsync(1, dto);

        result.Should().BeTrue();
        todo.Title.Should().Be("Updated");
    }
    
    [Fact]
    public async Task DeleteAsync_ShouldMarkAsDeleted_WhenExists()
    {
        var todo = new TodoItem { Id = 1 };

        _repositoryMock
            .Setup(r => r.GetByIdAsync(1))
            .ReturnsAsync(todo);

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
}