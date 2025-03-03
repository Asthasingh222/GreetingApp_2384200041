using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelLayer.Model
{
    public class ResponseModel<T>
    {
        public bool success { get; set; } = false;
        public string message { get; set; } = "";
        public T data { get; set; } =default(T);
    }
}
