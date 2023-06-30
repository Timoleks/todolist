namespace TodolistApi.Domain.Models
{
    public class Day : EntityBase
    {
        public DateTimeOffset Date { get; set; }
        public virtual ICollection<TodoItem>? Items { get; set; }
    }
}
