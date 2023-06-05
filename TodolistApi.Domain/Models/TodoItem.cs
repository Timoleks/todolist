namespace TodolistApi.Domain.Models
{
	public class TodoItem : EntityBase
	{
        
        public string? Name { get; set; }
        public bool? IsDone { get; set; }
        public DateTime? CreatedAt { get; set; }
         
    }
}

