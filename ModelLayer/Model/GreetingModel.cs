using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelLayer.Model
{
    public class GreetingModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage ="Message cannot be empty")]
        public string Message { get; set; }
    }
}
