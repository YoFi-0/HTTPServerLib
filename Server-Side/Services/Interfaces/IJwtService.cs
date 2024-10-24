using Domain.ReturnsModel;

namespace Server_Side.Services.Interfaces
{
    public interface IJwtService<T> where T : class
    {
        string Create(T data);
        ReturnJwtModel<T> Get(string EncodedToken);
    }
}
