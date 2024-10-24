namespace Server_Side.Services.Interfaces
{
    public interface IFn
    {
        public string Json<T>(T obj);
        public T Json<T>(string str);
        public void SaveDB(string msg, Action? func = null);
    }
}
