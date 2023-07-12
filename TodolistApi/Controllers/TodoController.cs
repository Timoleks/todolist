using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.EntityFrameworkCore;
using TodolistApi.Domain.Models;
using TodolistApi.Service.Repository;

namespace TodolistApi.Controllers;

[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
[Authorize(Roles = "ADMIN")]
[ApiController]
[Route("[controller]/[action]")]
public class TodoController : ControllerBase
{
    private readonly ILogger<TodoController> _logger;
    private readonly IRepository<TodoItem> _repository;

    public TodoController(ILogger<TodoController> logger, IRepository<TodoItem> todoItemRepository)
    {
        ArgumentNullException.ThrowIfNull(logger);
        ArgumentNullException.ThrowIfNull(todoItemRepository);

        _logger = logger;
        _repository = todoItemRepository;
    }
    

    [HttpPost]
    public async Task<IActionResult> AddItem(string name)
    {
        var userId = HttpContext.User.Identities
            .FirstOrDefault()
            .Claims.FirstOrDefault(claim => claim.Type == JwtRegisteredClaimNames.Sub).Value;

        try
        {
            var todoItem = new TodoItem { Name = name };
            todoItem.UserId = userId;
            _repository.Insert(todoItem);
            await _repository.SaveChangesAsync();
            return Ok(todoItem.Id);
        }
        catch(Exception ex) 
        {
            _logger.LogError(new EventId(), ex, ex.Message);
            return BadRequest();
        }
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        try
        {
           
            var userId = HttpContext.User.Identities
                .FirstOrDefault()
                .Claims.FirstOrDefault(claim => claim.Type == JwtRegisteredClaimNames.Sub).Value;

            return Ok(_repository.Get()
                .Where(record => userId == record.UserId)
                .Take(5000)
                .ToArray());
        }
        catch (Exception ex)
        {
            _logger.LogError(new EventId(), ex, ex.Message);
            return Problem();
        }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetItem(int id)
    {
        var userId = HttpContext.User.Identities
            .FirstOrDefault()
            .Claims.FirstOrDefault(claim => claim.Type == JwtRegisteredClaimNames.Sub).Value;

        try
        {
            var item = _repository
                .Get()
                .Where(record => record.Id == id && userId == record.UserId)
                .ToArray()
                .SingleOrDefault();

            return 
                item == null
                    ? NotFound() 
                    : Ok(item);
        }
        catch (Exception ex)
        {
            _logger.LogError(new EventId(), ex, ex.Message);
            return BadRequest();
        }
    }

    

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteItem(int id)
    {
        try
        {
            var userId = HttpContext.User.Identities
                .FirstOrDefault()
                .Claims.FirstOrDefault(claim => claim.Type == JwtRegisteredClaimNames.Sub).Value;

            var item = _repository.Get()
                .Where(record => record.Id == id && userId == record.UserId)
                .ToArray()
                .SingleOrDefault();

            if (item is null)
                return NotFound();

            _repository.Delete(item);
            await _repository.SaveChangesAsync();
            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(new EventId(), ex, ex.Message);
            return BadRequest();
        }
    }

    [HttpPut]
    public async Task<IActionResult> Update(int id, string newName)
    {
        try
        {
            var userId = HttpContext.User.Identities
                .FirstOrDefault()
                .Claims.FirstOrDefault(claim => claim.Type == JwtRegisteredClaimNames.Sub).Value;

            var item = _repository
                .Get()
                .FirstOrDefault(record => record.Id == id && userId == record.UserId);

            if (item is null)
                return NotFound();
            item.Name = newName;
            _repository.Update(item);
            await _repository.SaveChangesAsync();
            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(new EventId(), ex, ex.Message);
            return BadRequest();
        }
    }
}
