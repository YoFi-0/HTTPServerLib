using Domain.ReturnsModel;
using Server_Side.DataBase;
using Server_Side.Repositorys.Interfaces;
using Server_Side.Services.Interfaces;
using System.Collections;

namespace Server_Side.Repositorys.Classes
{
    public class Repository<T>:IRepository<T> where T : class
    {
        private readonly ApplicationDbContext _context;
        private readonly IHandlerService _handler;
        public Repository(ApplicationDbContext context, IHandlerService handler)
        {
            _context = context;
            _handler = handler;
        }
        public void Delete(int id, bool? save = null)
        {
            T? Entity = _context.Set<T>().Find(id);
            if (_handler.IsNull(Entity))
            {
                _handler.Err("لم يتم العثور على العنصر المطلوب حذفه");
            }
            if (save == true)
            {
                _context.SaveChanges();
            }
            _context.Set<T>().Remove(Entity);
        }
        public T Get(int id)
        {
            var data = _context.Set<T>().Find(id);
            if (_handler.IsNull(data))
            {
                _handler.Err("لم يتم العثور على العنصر المطلوب");
            }
            return data!;
        }

        public IEnumerable<T> GetAll()
        {
            return _context.Set<T>().ToList();
        }

        public T Insert(T entity, bool? save = null)
        {
            if (_handler.IsNull(entity))
            {
                _handler.Err("لا يوجد بيانات لإدخلها");
            }
            var action = _context.Set<T>().Add(entity);
            if (save == true)
            {
                _context.SaveChanges();
            }
            return action.Entity;
        }
        public void InsertBulk(List<T> entity, bool? save = null)
        {
            if (_handler.IsNull(entity))
            {
                _handler.Err("لا يوجد بيانات لإدخلها");
            }
            if (save == true)
            {
                _context.SaveChanges();
            }
            _context.Set<T>().AddRange(entity);
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }

        public void Update(T entity, bool? save = null)
        {
            if (_handler.IsNull(entity))
            {
                _handler.Err("لا يوجد بيانات لتحديثها"); ;
            }
            if (save == true)
            {
                _context.SaveChanges();
            }
            _context.Entry(entity).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
        }

        public DatabaseType UpdateValues<DatabaseType, DtoType>(DatabaseType target, DtoType dto)
        {
            var targetType = typeof(DatabaseType);
            var dtoType = dto!.GetType();

            foreach (var property in dtoType.GetProperties())
            {
                if (property.Name == "Id" || property.Name == "Password")
                {
                    continue;
                }
                if (typeof(IEnumerable).IsAssignableFrom(property.PropertyType) && property.PropertyType != typeof(string))
                {
                    continue;
                }
                var targetProperty = targetType.GetProperty(property.Name);
                if (targetProperty != null && property.CanRead && targetProperty.CanWrite)
                {
                    var newValue = property.GetValue(dto);
                    if (newValue != null)
                    {
                        targetProperty.SetValue(target, newValue);
                    }
                }
            }
            return target;
        }
    }
}
