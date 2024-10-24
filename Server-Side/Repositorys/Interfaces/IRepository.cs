using Domain.ReturnsModel;
using Microsoft.EntityFrameworkCore;
using Server_Side.DataBase;
using Server_Side.Services.Interfaces;

namespace Server_Side.Repositorys.Interfaces
{
    public interface IRepository<T> where T : class
    {
        public void Delete(int id, bool? save = null);

        public T Get(int id);

        public IEnumerable<T> GetAll();

        public T Insert(T entity, bool? save = null);

        public void InsertBulk(List<T> entity, bool? save = null);

        public void SaveChanges();

        public void Update(T entity, bool? save = null);

        public DatabaseType UpdateValues<DatabaseType, DtoType>(DatabaseType target, DtoType dto);
    }

}

