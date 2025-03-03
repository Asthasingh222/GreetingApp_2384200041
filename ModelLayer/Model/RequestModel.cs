using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelLayer.Model
{
    public class RequestModel
    {
        public string? Key { get; set; }  // Nullable to allow partial updates
        public string? Value { get; set; } // Nullable to allow partial updates
    }

}
