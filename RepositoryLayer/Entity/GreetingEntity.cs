using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RepositoryLayer.Entity
{
    public class GreetingEntity
    {
        [Key]
        public int Id { get; set; }  // Primary Key

        [Required]
        public string GreetingMessage { get; set; }

        public Guid? UserId { get; set; }  // Nullable Foreign Key

        [ForeignKey("UserId")]
        public UserEntity? User { get; set; }  // Nullable Navigation Property
    }
}
