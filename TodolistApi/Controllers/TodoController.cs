using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TodolistApi.Models;

namespace TodolistApi.Controllers;

[ApiController]
[Route("[controller]")]
public class TodoController : ControllerBase
{
    private static ConcurrentDictionary<int, TodoItem> _todos = new ();
    private static int Count = 0;

    

    [HttpPost]
    public async Task<IActionResult> AddItem(string name)
    {
        TodoItem item = new TodoItem();
        item.Name = name;
        item.CreatedAt = DateTime.Now;
        item.Id = Interlocked.Increment(ref Count);

        if(_todos.TryAdd(item.Id,item))
        {
            return Ok(item);
        }

        return BadRequest();
    }

    [HttpGet]
    public async Task<IActionResult> GetItem(int id)
    {
        var itemExists = _todos.TryGetValue(id,out var item);

        if (itemExists)
        {
            return Ok(item);
        }

        return NotFound();
    }

    [HttpDelete]
    public async Task <IActionResult> Delete(int id)
    {
        if (_todos.TryRemove(id, out _) is false)
            return BadRequest();
         
        return Ok("mes");
    }

    [HttpPut]
    public async Task<IActionResult> Update()
    {


        return Ok();
    }
}

