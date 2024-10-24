using Domain.ReturnsModel;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Server_Side.Services.Interfaces;
using System.Reflection.Metadata;

namespace Server_Side.Services.Classes
{
    public class HandlerService : IHandlerService
    {
        public void Err(string msg)
        {
            throw new Exception($"!!--!!-{msg}");
        }
        public bool IsNull(string? str)
        {
            return string.IsNullOrEmpty(str);
        }

        public bool IsNull<T>(T? obj)
        {
            return obj == null ||  obj is null;
        }
        public bool IsNull(params object[] objs)
        {
            for(int i = 0; i < objs.Length; i++)
            {
                if (objs[i].GetType() == typeof(List<>))
                {
                    if(((List<object>)objs[i]).Count == 0)
                    {
                        return false;
                    }
                } else if (objs[i].GetType() == typeof(IEnumerable<>))
                {
                    if (((IEnumerable<object>)objs[i]).Count() == 0)
                    {
                        return false;
                    }
                }
                if (objs[i] is null)
                {
                    return false;
                }
            }
            return true;
        }
    }
}
