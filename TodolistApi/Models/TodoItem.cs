using System;
namespace TodolistApi.Models
{
	public class TodoItem
	{
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsDone { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}

