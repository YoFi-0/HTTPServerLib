using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.ReturnsModel
{
    public class ReturnJwtModel<T>
    {
        public DateTime? expireDate { get; set; }
        public T Data { get; set; }
        public bool IsValid { get; set; }
    }
}
