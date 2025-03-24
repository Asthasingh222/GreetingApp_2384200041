using System;
using System.ComponentModel.DataAnnotations;

namespace ModelLayer.Model
{
    public class GreetingModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Message cannot be empty")]
        public string Message { get; set; }

        public Guid? UserId { get; set; } // Nullable to match the database schema
    }
}
