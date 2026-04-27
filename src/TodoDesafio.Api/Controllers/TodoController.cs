using TodoDesafio.Application.Common.Responses;
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
    public async Task<ActionResult<ApiResponse<IEnumerable<TodoItemDto>>>> Get([FromQuery] Status? status, [FromQuery] DateTime? dueDate)
    {
        var todoItemDtos = await _itemService.GetAllAsync(status, dueDate);
        return Ok(ApiResponse<IEnumerable<TodoItemDto>>.SuccessResponse(todoItemDtos, "Items recuperados com sucesso!"));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ApiResponse<TodoItemDto>>> GetById(int id)
    {
        var todoItemDto = await _itemService.GetByIdAsync(id);
        if (todoItemDto == null) return NotFound(ApiResponse<string>.ErrorResponse("Item não encontrado!"));

        return Ok(ApiResponse<TodoItemDto>.SuccessResponse(todoItemDto, "Item recuperado com sucesso!"));
    }

    [HttpPost]
    public async Task<ActionResult<ApiResponse<TodoItemDto>>> Post([FromBody] CreateTodoItemDto itemDto)
    {
        var todoItemDto = await _itemService.CreateAsync(itemDto);
        return CreatedAtAction(nameof(GetById), new { id = todoItemDto.Id },
            ApiResponse<TodoItemDto>.SuccessResponse(todoItemDto, "Item criado/adicionado com sucesso!"));
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<ApiResponse<TodoItemDto>>> Put(int id, [FromBody] UpdateTodoItemDto itemDto)
    {
        var updated = await _itemService.UpdateAsync(id, itemDto);
        if (!updated) return NotFound(ApiResponse<string>.ErrorResponse("Item não encontrado!"));

        return Ok(ApiResponse<UpdateTodoItemDto>.SuccessResponse(itemDto, "Item atualizado com sucesso!"));
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<ApiResponse<TodoItemDto>>> Delete(int id)
    {
        var deleted = await _itemService.DeleteAsync(id);
        if (!deleted) return NotFound(ApiResponse<string>.ErrorResponse("Item não encontrado!"));

        return Ok(ApiResponse<string>.SuccessResponse("Item excluído com sucesso!"));
    }
    
    [ApiExplorerSettings(IgnoreApi = true)]
    [Route("error")]
    public IActionResult HandleError()
    {
        return StatusCode(500, ApiResponse<string>.ErrorResponse("Erro interno no servidor"));
    }
}