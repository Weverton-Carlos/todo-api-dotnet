using TodoDesafio.Application.DTOs;
using TodoDesafio.Application.Interfaces;
using TodoDesafio.Domain.Enums;

namespace TodoDesafio.Api.Controllers;

using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class TodoController : ControllerBase
{
    private readonly ITodoItemService _itemService;

    public TodoController(ITodoItemService itemService)
    {
        _itemService = itemService;
    }

    [HttpGet]
    public async Task<IActionResult> Get([FromQuery] Status? status, [FromQuery] DateTime? dueDate)
    {
        var todoItemDtos = await _itemService.GetAllAsync(status, dueDate);
        return Ok(todoItemDtos);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var todoItemDto = await _itemService.GetByIdAsync(id);
        if (todoItemDto == null) return NotFound();

        return Ok(todoItemDto);
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] CreateTodoItemDto itemDto)
    {
        var todoItemDto = await _itemService.CreateAsync(itemDto);
        return CreatedAtAction(nameof(GetById), new { id = todoItemDto.Id }, todoItemDto);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Put(int id, [FromBody] UpdateTodoItemDto itemDto)
    {
        var updated = await _itemService.UpdateAsync(id, itemDto);
        if (!updated) return NotFound();

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _itemService.DeleteAsync(id);
        if (!deleted) return NotFound();

        return NoContent();
    }
}