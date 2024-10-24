using Domain.Model;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Server_Side.DataBase;
using Server_Side.Services.Interfaces;

namespace Server_Side.Services.Classes
{
    public class Fn : IFn
    {   
        private readonly ApplicationDbContext _context;
        private readonly IHandlerService _handler;
        private readonly IJwtService<JWTDataModel> _jwt;
        public Fn(ApplicationDbContext context, IHandlerService handler, IJwtService<JWTDataModel> jwt)
        {
            _context = context;
            _handler = handler;
            _jwt = jwt;
        }
        public string Json<T>(T obj)
        {
            return JsonConvert.SerializeObject(obj);
        }
        public T Json<T>(string str)
        {
            return JsonConvert.DeserializeObject<T>(str)!;
        }

        public void SaveDB(string msg, Action? func = null)
        {
            try
            {
                _context.SaveChanges();
            }
            catch (Exception ex) {
                
                if (!_handler.IsNull(func))
                {
                    func!.Invoke();
                } else
                {
                    _handler.Err(msg);
                }
            }
        }
    }
}
