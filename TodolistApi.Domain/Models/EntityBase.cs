using System.ComponentModel.DataAnnotations;

namespace TodolistApi.Domain.Models
{
    public abstract class EntityBase
    {
        [Key]
        public int Id { get; set; }

        
    }
}
