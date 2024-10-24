using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.ReturnsModel
{
    public class ReturnModel
    {
        public bool IsSucceeded { get; set; }
        public string Comment { get; set; }
    }
    public class ReturnModel<T> : ReturnModel
    {
        public T Data;  
    }
}
