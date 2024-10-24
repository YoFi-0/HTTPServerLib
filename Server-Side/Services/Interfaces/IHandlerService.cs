using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Server_Side.Services.Interfaces
{
    public interface IHandlerService
    {
        public void Err(string msg);
        public bool IsNull(string? str);
        public bool IsNull<T>(T? obj);
        public bool IsNull(params object[] objs);
    }
}
