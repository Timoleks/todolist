using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TodolistApi.Domain.Models
{
    [Table("AspNetUsers")]
    public class User
    {
        [Key]
        public string Id { get; set; }
    }
}
