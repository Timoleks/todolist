namespace TodolistApi.Domain.Models
{
	public class TodoItem : EntityBase
	{
        
        public string? Name { get; set; }
        public bool? IsDone { get; set; }
        public DateTime? CreatedAt { get; set; }
        public virtual Day? Day { get; set; }
        public int? DayId { get; set; }
        public virtual User? User { get; set; }
        public string? UserId { get; set; }
    }
}

